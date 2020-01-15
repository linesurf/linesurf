using System;
using System.Reflection;
using Linesurf.Framework;
using Linesurf.Framework.Map.Objects;
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
        WeightedFramerate drawRate = new WeightedFramerate(6);
        WeightedFramerate updateRate = new WeightedFramerate(6);
        Texture2D line;
        bool timerOn = false;
        SoundEffect effect = default!;
        Song song = default!;

        readonly bool isDebug = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ==
                       "Debug";

        public static Texture2D Pixel = default!;


        readonly Point[] curveControl = {new Point(200, 200), new Point(400, 200), new Point(400, 400)};
        readonly Point[] lineaControl = {new Point(400, 400), new Point(800, 400)};
        
        BezierSegment bezierSegment;
        LinearSegment linearSegment;
        
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
            
            linearSegment = new LinearSegment(lineaControl);

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
            curveControl[1] = Mouse.GetState().Position;
            bezierSegment = new BezierSegment(curveControl);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            drawRate.Update();
                
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(fontNormal,
                $"{(int)drawRate.Framerate}FPS\n{(int)updateRate.Framerate}UPS\n{bezierSegment.Length} bezier length",
                Vector2.Zero, Color.White);

                
            spriteBatch.DrawSegment(bezierSegment, 20, Color.White);
            spriteBatch.DrawSegment(linearSegment, 20, Color.White);

            if (isDebug)
            {
                spriteBatch.DrawString(fontNormal, "debug build",
                    new Vector2(GraphicsDevice.Viewport.Width - fontNormal.MeasureString("debug build").X, 0),
                    Color.Red);
            }
            
            
            spriteBatch.End();
            base.Draw(gameTime);
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