using System;
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
        DateTime oldTime = DateTime.Now;
        TimeSpan updateTimeDateTime = TimeSpan.Zero;
        TimeSpan updateGameTime;
        WeightedFramerate drawRate = new WeightedFramerate(4);
        WeightedFramerate updateRate = new WeightedFramerate(4);
        float x;
        bool timerOn = false;
        bool showText = false;
        float currentTime = 0;
        int counter = 1;
        float countDuration = 1f;
        SoundEffect effect;
        bool playSoundEffect = false;
        Song song;
        public static Texture2D Pixel = default!;

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontNormal = Content.Load<SpriteFont>("fontnormal");
            effect = Content.Load<SoundEffect>("normal-hitnormal");
            
            this.song = Content.Load<Song>("shutter");
            MediaPlayer.Volume = 0.17f;
            MediaPlayer.Play(song);
            timerOn = true;
        }

        protected override void Update(GameTime gameTime)
        {            
            if (timerOn)
            {
                    currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    showText = false; 
            }
            else showText = true;
            if (currentTime >= countDuration)
            {
                counter++;
              
                currentTime -= countDuration; 
            }
            if (counter % 461 == 0) playSoundEffect = true;
            if (playSoundEffect)
            {
                effect.Play(0.20f, 0f, 0f);
                playSoundEffect = false;
            }

            updateGameTime = gameTime.ElapsedGameTime;
            updateTimeDateTime = DateTime.Now.Subtract(oldTime);
            oldTime = DateTime.Now;
            base.Update(gameTime);
            updateRate.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.DimGray);
            spriteBatch.Begin();
            spriteBatch.DrawString(fontNormal, (int) updateRate.Framerate + " updates per second", new Vector2(0, 0), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, (int) drawRate.Framerate + " draws per second", new Vector2(0, 20), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, updateGameTime.TotalMilliseconds + "ms update (gameTime)", new Vector2(0, 40), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, (int) (1000/updateGameTime.TotalMilliseconds) + " updates/s (gameTime)", new Vector2(0, 60), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, updateTimeDateTime.TotalMilliseconds + "ms update (DateTime)", new Vector2(0, 80), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, (int) (1000/updateTimeDateTime.TotalMilliseconds) + " updates/s (DateTime)", new Vector2(0, 100), Color.CornflowerBlue);
            
            spriteBatch.DrawString(fontNormal, "OWO!!!!!!!!", new Vector2(0,200), Color.Aqua);
            if (showText) spriteBatch.DrawString(fontNormal, "uwu owo, what's this??????", new Vector2(0,210), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
            drawRate.Update();
        }


    }
}
