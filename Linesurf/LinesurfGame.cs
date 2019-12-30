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
        SpriteBatch spriteBatch;
        GameTime toShowTime;
        SpriteFont font;
        public static Texture2D Pixel;
        private float x;
        private Vector2 vector2 = new Vector2(1, 50);
        private bool timeron = false;
        private bool showthing = false;
        private float currentTime = 0;
        int counter = 1;
        int limit = 999999;
        float countDuration = 1f;
        private SoundEffect effect;
        bool playsoundeffect = false;
        Song song;
        public LinesurfGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000);
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("fontnormal");
            effect = Content.Load<SoundEffect>("normal-hitnormal");
            
            this.song = Content.Load<Song>("shutter");
            MediaPlayer.Volume = 0.17f;
            MediaPlayer.Play(song);
            timeron = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (timeron) if (gameTime != null)
                {
                    currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    showthing = false;
                }
                else showthing = true;
            if (currentTime >= countDuration)
            {
                counter++;
              
                currentTime -= countDuration; 
            }
            if (counter % 461 == 0) playsoundeffect = true;
            if (playsoundeffect)
            {
                effect.Play(0.20f, 0f, 0f);
                playsoundeffect = false;
            }
            
            gameTime = toShowTime;
            
            
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.DimGray);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "uwu owo, what's this??????? " + gameTime.ElapsedGameTime.TotalMilliseconds + "ms " + counter.ToString(), Vector2.One, Color.Aqua);
            
            spriteBatch.DrawString(font, "OWO!!!!!!!!", vector2, Color.Aqua);
            if (showthing) spriteBatch.DrawString(font, "uwu owo, what's this??????", Vector2.One, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
