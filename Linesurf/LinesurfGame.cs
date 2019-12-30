﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
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

        float timer;
        float timerDurationMs;

        public const float MsOffset = 1;

        public static Texture2D Pixel = default!;

        public LinesurfGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            Window.AllowUserResizing = true;

            TargetElapsedTime = TimeSpan.FromMilliseconds(MsOffset);
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
            MediaPlayer.ActiveSongChanged += (sender, e) => timerOn = true;
            timer = 2202;
            timerDurationMs = 461;

            MediaPlayer.Volume = 0.17f;
            MediaPlayer.Play(song);
        }

        protected override void Update(GameTime gameTime)
        {
            if (timerOn)
            {
                timer -= MsOffset;
                if (timer < 0)
                {
                    effect.Play(0.20f, 0f, 0f);
                    timer = timerDurationMs - timer;
                }
            }

            base.Update(gameTime);
            updateRate.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.DimGray);
            spriteBatch.Begin();

            spriteBatch.DrawString(fontNormal, (int) updateRate.Framerate + " updates per second", new Vector2(0, 0), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, (int) drawRate.Framerate + " draws per second", new Vector2(0, 20), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, updateRate.LastLatency.TotalMilliseconds + "ms update latency", new Vector2(0, 40), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, drawRate.LastLatency.TotalMilliseconds + " ms draw latency", new Vector2(0, 60), Color.CornflowerBlue);

            spriteBatch.DrawString(fontNormal, timer + " timer", new Vector2(0, 80), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, timerDurationMs + " timer duration", new Vector2(0, 100), Color.CornflowerBlue);


            spriteBatch.End();

            base.Draw(gameTime);
            drawRate.Update();
        }


    }
}
