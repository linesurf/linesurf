using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
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

        bool timerOn = false;
        SoundEffect effect = default!;
        Song song = default!;

        bool debounce = false;
        bool isDebug = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration == "Debug";

        Stopwatch audioStart = new Stopwatch();

        float Timer => audioStart.Elapsed.Milliseconds;

        public static Texture2D Pixel = default!;

        float bpmOffset = 60_000f / 171.27f;
        float songOffset = 0;
        public LinesurfGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            Window.AllowUserResizing = true;

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


            MediaPlayer.Volume = 0.175f;
        }


        protected override void Update(GameTime gameTime)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(song);
                audioStart.Restart();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                MediaPlayer.Stop();
                bpmOffset = new Random().Next(100, 1001);
            }


            if (timerOn)
            {
                if ((audioStart.Elapsed.TotalMilliseconds - songOffset) % bpmOffset < updateRate.LastLatency.TotalMilliseconds)
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

            base.Update(gameTime);
            updateRate.Update();
        }


        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(new Color((uint) Map(MathF.Pow(Timer % bpmOffset, 3), MathF.Pow(bpmOffset, 3), 0, 0, 255)));
            spriteBatch.Begin();

            spriteBatch.DrawString(fontNormal, MathF.Round(updateRate.Framerate) + " updates per second", new Vector2(0, 0), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, MathF.Round(drawRate.Framerate) + " draws per second", new Vector2(0, 20), Color.CornflowerBlue);

            spriteBatch.DrawString(fontNormal, updateRate.LastLatency.TotalMilliseconds + "ms update latency", new Vector2(0, 40), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, drawRate.LastLatency.TotalMilliseconds + " ms draw latency", new Vector2(0, 60), Color.CornflowerBlue);

            spriteBatch.DrawString(fontNormal, MediaPlayer.PlayPosition.TotalMilliseconds + "ms player", new Vector2(0, 120), Color.Wheat);
            spriteBatch.DrawString(fontNormal, (int) audioStart.Elapsed.TotalMilliseconds + "ms timer", new Vector2(0, 140), Color.Wheat);
            spriteBatch.DrawString(fontNormal, (int) (audioStart.Elapsed.TotalMilliseconds % bpmOffset) + "ms to beat", new Vector2(0, 160), Color.White);

            if (isDebug)
            {

                spriteBatch.DrawString(fontNormal, "debug build", new Vector2(GraphicsDevice.Viewport.Width - fontNormal.MeasureString("debug build").X, 0), Color.IndianRed);
            }

            spriteBatch.End();

            base.Draw(gameTime);
            drawRate.Update();
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            MediaPlayer.Pause();
            audioStart.Stop();
            base.OnDeactivated(sender, args);
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            MediaPlayer.Resume();
            audioStart.Start();
            base.OnActivated(sender, args);
        }

        public static float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh) 
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
