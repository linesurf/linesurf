using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Linesurf
{
    public class LinesurfGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch = default!;
        SpriteFont fontNormal = default!;

        WeightedFramerate drawRate = new WeightedFramerate(6);
        WeightedFramerate updateRate = new WeightedFramerate(6);
        double timer;
        bool timerOn = false;
        SoundEffect effect = default!;
        Song song = default!;
        
        bool debounce = false;
        bool isDebug = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration == "Debug";
        bool a;
        Stopwatch audioStart = new Stopwatch();

        public static Texture2D Pixel = default!;

        double bpmOffset = 60000d / 171.27;
        double songOffset = 0;
        public LinesurfGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            Window.AllowUserResizing = true;

            //songPosition = new Stopwatch();
            TargetElapsedTime = TimeSpan.FromMilliseconds(1);
        }

        protected override void Initialize()
        {
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontNormal = Content.Load<SpriteFont>("fontnormal");
            effect = Content.Load<SoundEffect>("normal-hitnormal");

            song = Content.Load<Song>("shutter");
            MediaPlayer.MediaStateChanged += (sender, e) =>
            {
                timerOn = true;
                audioStart.Restart();
            };


            MediaPlayer.Volume = 0.17f;
        }


        protected override void Update(GameTime gameTime)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(song);
                timer = songOffset;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (timerOn)
            {
                timer += updateRate.LastLatency.TotalMilliseconds;

                if (timer % bpmOffset < updateRate.LastLatency.TotalMilliseconds)
                {
                    if (!debounce)
                    {
                        effect.Play(0.20f, 0f, 0f);
                        debounce = true;
                        Console.Write("Ting! ");
                    }
                }
                else
                {
                    debounce = false;
                }
            }
            
            Console.WriteLine("Update: {0} ms", updateRate.LastLatency.TotalMilliseconds);
            
            base.Update(gameTime);
            updateRate.Update();
        }


        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.DimGray);
            spriteBatch.Begin();

            spriteBatch.DrawString(fontNormal, Math.Round(updateRate.Framerate) + " updates per second", new Vector2(0, 0), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, Math.Round(drawRate.Framerate) + " draws per second", new Vector2(0, 20), Color.CornflowerBlue);

            spriteBatch.DrawString(fontNormal, updateRate.LastLatency.TotalMilliseconds + "ms update latency", new Vector2(0, 40), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, drawRate.LastLatency.TotalMilliseconds + " ms draw latency", new Vector2(0, 60), Color.CornflowerBlue);

            spriteBatch.DrawString(fontNormal, MediaPlayer.PlayPosition.TotalMilliseconds + "ms player", new Vector2(0, 120), Color.Wheat);
            spriteBatch.DrawString(fontNormal, (int) timer + "ms timer", new Vector2(0, 140), Color.Wheat);
            spriteBatch.DrawString(fontNormal, (int) (timer % bpmOffset) + "ms to beat", new Vector2(0,160), Color.White);
            
            
            if (isDebug)
            {

                spriteBatch.DrawString(fontNormal, "debug build", new Vector2(GraphicsDevice.Viewport.Width-fontNormal.MeasureString("debug build").X, 0) , Color.IndianRed);
            }

            spriteBatch.End();

            base.Draw(gameTime);
            drawRate.Update();
        }
    }
}
