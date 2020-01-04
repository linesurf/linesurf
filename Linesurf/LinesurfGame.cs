using System;
using System.Collections.Generic;
using System.Reflection;
using Linesurf.Framework;
using Linesurf.Framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
namespace Linesurf
{
    public class LinesurfGame : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch = default!;
        SpriteFont fontNormal = default!;
        public static string UpdateToDrawLog = "";
        WeightedFramerate drawRate = new WeightedFramerate(6);
        WeightedFramerate updateRate = new WeightedFramerate(6);

        bool timerOn = false;
        SoundEffect effect = default!;
        Song song = default!;

        readonly bool isDebug = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ==
                       "Debug";

        public static Texture2D Pixel = default!;

        List<Vector2> curvePoints =
            ComputeCurvePoints(10, new[] {new Point(10, 20), new Point(300, 300), new Point(0, 330),});

        MusicClock musicClock = new MusicClock(
            new TimingPoint(54, 120),
            new TimingPoint(44554, 115),
            new TimingPoint(44554+MusicUtils.ToMsOffset(115), 110),
            new TimingPoint(44554+MusicUtils.ToMsOffset(115)+MusicUtils.ToMsOffset(110), 105),
            new TimingPoint(44554+MusicUtils.ToMsOffset(115)+MusicUtils.ToMsOffset(110)+MusicUtils.ToMsOffset(105), 100),
            new TimingPoint(44554+MusicUtils.ToMsOffset(115)+MusicUtils.ToMsOffset(110)+MusicUtils.ToMsOffset(105)+MusicUtils.ToMsOffset(110), 95f),
            new TimingPoint(44554+MusicUtils.ToMsOffset(115)+MusicUtils.ToMsOffset(110)+MusicUtils.ToMsOffset(105)+MusicUtils.ToMsOffset(110)+MusicUtils.ToMsOffset(95), 90),
            new TimingPoint(58750, 96),
            new TimingPoint(58750+MusicUtils.ToMsOffset(96), 102),
            new TimingPoint(58750+MusicUtils.ToMsOffset(96)+MusicUtils.ToMsOffset(102), 108),
            new TimingPoint(58750+MusicUtils.ToMsOffset(96)+MusicUtils.ToMsOffset(102)+MusicUtils.ToMsOffset(108), 114),
            new TimingPoint(58750+MusicUtils.ToMsOffset(96)+MusicUtils.ToMsOffset(102)+MusicUtils.ToMsOffset(108)+MusicUtils.ToMsOffset(114), 120));

        public LinesurfGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;

            TargetElapsedTime = TimeSpan.FromMilliseconds(1);
        }

        protected override void Initialize()
        {
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });

            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferMultiSampling = true;
            GraphicsDevice.PresentationParameters.MultiSampleCount = 2;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontNormal = Content.Load<SpriteFont>("fontnormal");
            effect = Content.Load<SoundEffect>("normal-hitnormal");

            song = Content.Load<Song>("music");
            MediaPlayer.MediaStateChanged += (sender, e) => { timerOn = true; };

            MediaPlayer.Play(song);
            musicClock.AudioTime.Restart();
            MediaPlayer.Volume = 0.175f;
            
        }


        protected override void Update(GameTime gameTime)
        {
            updateRate.Update();
            UpdateToDrawLog += $"U:{updateRate.LastMilliseconds}ms ";
            musicClock.Snapshot(updateRate);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (timerOn)
            {
                if (musicClock.CheckBeat(updateRate))
                {
                    effect.Play(0.20f, 0f, -1f);

                }
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            drawRate.Update();
            Console.WriteLine(UpdateToDrawLog + "D:{0}ms ",drawRate.LastMilliseconds);
            UpdateToDrawLog = "";
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue.Darken(70));
            spriteBatch.Begin();

            spriteBatch.DrawString(fontNormal, MathF.Round(updateRate.Framerate) + " updates per second",
                new Vector2(0, 0), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, MathF.Round(drawRate.Framerate) + " draws per second",
                new Vector2(0, 20), Color.CornflowerBlue);

            spriteBatch.DrawString(fontNormal, updateRate.LastMilliseconds + "ms update latency",
                new Vector2(0, 40), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, drawRate.LastLatency.TotalMilliseconds + " ms draw latency",
                new Vector2(0, 60), Color.CornflowerBlue);

            spriteBatch.DrawString(fontNormal, MediaPlayer.PlayPosition.TotalMilliseconds + "ms player",
                new Vector2(0, 120), Color.Wheat);
            spriteBatch.DrawString(fontNormal, (int) musicClock.AudioTime.Elapsed.TotalMilliseconds + "ms timer",
                new Vector2(0, 140), Color.Wheat);
            spriteBatch.DrawString(fontNormal,
                (int) (musicClock.AudioTime.Elapsed.TotalMilliseconds % musicClock.BpmOffset) + "ms to beat",
                new Vector2(0, 160), Color.White);

            spriteBatch.DrawString(fontNormal,
                $"{musicClock.Bpm} bpm ({musicClock.BpmOffset} ms)",
                new Vector2(0, 180), Color.White);

            spriteBatch.DrawString(fontNormal,
                musicClock.SongOffset + "ms offset", new Vector2(0, 200), Color.White);

            if (isDebug)
            {
                spriteBatch.DrawString(fontNormal, "debug build",
                    new Vector2(GraphicsDevice.Viewport.Width - fontNormal.MeasureString("debug build").X, 0),
                    Color.IndianRed);
            }
            
            
            
            //also part of the shit code
            
            for(var x = 0; x < curvePoints.Count-1; x++)
            {
                spriteBatch.DrawLine( curvePoints[x], curvePoints[x + 1], Color.Black, 2);
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        //shit code area copied over
        //please dont mind the shit
        
        
        
        public static List<Vector2> ComputeCurvePoints(int steps, Point[] pointsQ)
        {   
            var curvePoints = new List<Vector2>();
            for (float x = 0; x < 1; x += 1 / (float)steps)
            {
                curvePoints.Add(GetBezierPointRecursive(x, pointsQ).ToVector2());
            }   
            return curvePoints; 
        }

//Calculates a point on the bezier curve based on the timeStep.
        private static Point GetBezierPointRecursive(float timeStep, IReadOnlyList<Point> ps)
        {   
            if (ps.Count > 2)
            {
                var newPoints = new List<Point>();
                for (var x = 0; x < ps.Count-1; x++)
                {
                    newPoints.Add(InterpolatedPoint(ps[x], ps[x + 1], timeStep));
                }
                return GetBezierPointRecursive(timeStep, newPoints.ToArray());
            }
            else
            {
                return InterpolatedPoint(ps[0], ps[1], timeStep);
            }
        }

//Gets the linearly interpolated point at t between two given points (with manual rounding).
        private static Point InterpolatedPoint(Point p1, Point p2, float t)
        {
            var (x, y) = (Vector2.Multiply(p2.ToVector2() - p1.ToVector2(), t) + p1.ToVector2());
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }

        

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            MediaPlayer.Pause();
            musicClock.AudioTime.Stop();
            base.OnDeactivated(sender, args);
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            MediaPlayer.Resume();
            musicClock.AudioTime.Start();
            base.OnActivated(sender, args);
        }
    }
}