using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Linesurf
{
    public class LinesurfGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch = default!;
        SpriteFont fontNormal = default!;

        WeightedFramerate drawRate = new WeightedFramerate(4);
        WeightedFramerate updateRate = new WeightedFramerate(4);

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
            updateRate.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.DimGray);
            spriteBatch.Begin();
            spriteBatch.DrawString(fontNormal, (int) updateRate.Framerate + " updates per second", new Vector2(0, 0), Color.CornflowerBlue);
            spriteBatch.DrawString(fontNormal, (int) drawRate.Framerate + " draws per second", new Vector2(0, 20), Color.CornflowerBlue);

            spriteBatch.End();
            base.Draw(gameTime);
            drawRate.Update();
        }

    }
}
