using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Linesurf
{
    public class LinesurfGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameTime toShowTime;
        SpriteFont font;
        public static Texture2D Pixel;

        public LinesurfGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
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
            
        }

        protected override void Update(GameTime gameTime)
        {
            gameTime = toShowTime;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.DimGray);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, gameTime.ElapsedGameTime.TotalMilliseconds + "ms", Vector2.One, Color.Aqua);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
