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
namespace Linesurf
{
    public class LinesurfGame : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch = default!;
        SpriteFont fontNormal = default!;
        WeightedFramerate drawRate = new WeightedFramerate(6);
        WeightedFramerate updateRate = new WeightedFramerate(6);
        Texture2D line;
        bool timerOn = false;
        SoundEffect effect = default!;
        Song song = default!;

        readonly bool isDebug = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ==
                       "Debug";

        public static Texture2D Pixel = default!;

        Vector2[] curvePoints =
            QuadraticBezierPoints(10, new Point(10, 10), new Point(400, 10), new Point(400, 400));

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
            graphics.PreparingDeviceSettings += (sender, args) => 
            { 
                graphics.PreferMultiSampling = true;
                args.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 2;
            };
        }

        protected override void Initialize()
        {
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });

            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontNormal = Content.Load<SpriteFont>("fontnormal");
            effect = Content.Load<SoundEffect>("normal-hitnormal");
            line = Content.Load<Texture2D>("line");
            song = Content.Load<Song>("music");
            MediaPlayer.MediaStateChanged += (sender, e) => { timerOn = true; };

            MediaPlayer.Play(song);
            musicClock.AudioTime.Restart();
            MediaPlayer.Volume = 0.175f;
            
        }


        protected override void Update(GameTime gameTime)
        {
            updateRate.Update();
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
            graphics.GraphicsDevice.Clear(Color.Black);
            curvePoints =
                QuadraticBezierPoints((int)MathUtils.Map(Math.Abs(Mouse.GetState().Y), 0, GraphicsDevice.Viewport.Height, 2, 20), new Point(10, 10), new Point(400, 10), new Point(400, 400));
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
            
            for(var x = 0; x < curvePoints.Length-1; x++)
            {
                spriteBatch.DrawLine(line, curvePoints[x], curvePoints[x + 1], Color.White, 5);
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        
        //bezier code i wrote.
        //bezier curve is
        //x = (1 - t) * (1 - t) * p[0].x + 2 * (1 - t) * t * p[1].x + t * t * p[2].x;
        //y = (1 - t) * (1 - t) * p[0].y + 2 * (1 - t) * t * p[1].y + t * t * p[2].y;
        //where p[0] and p[2] are the end points and p[1] is the control point
        //t is how far along we are the curve    

//we really didnt need 10 methods for that did we
        public static Vector2[] QuadraticBezierPoints(int steps, params Point[] p)
        {
            if (p.Length != 3) throw new ArgumentException("For quadratic Bézier curve calculation number of points given must be 3.");
            if(steps < 2) throw new ArgumentException("There must be at least two steps for Bézier calculation.");
            float t;
            float x;
            float y;
            var vector2s = new Vector2[steps];
            for (var i = 0; i < steps; i++)
            {
                t = MathUtils.Map(i, 0, steps - 1, 0, 1);
                
                x = (1 - t) * (1 - t) * p[0].X + 2 * (1 - t) * t * p[1].X + t * t * p[2].X;
                y = (1 - t) * (1 - t) * p[0].Y + 2 * (1 - t) * t * p[1].Y + t * t * p[2].Y;
                vector2s[i] = new Vector2(x,y);
            }

            return vector2s;
        }
        
        //the shit has been greatily reduced

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