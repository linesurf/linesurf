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


namespace Linesurf
{
    public class LinesurfGame : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch = default!;
        SpriteFont fontNormal = default!;
        WeightedFramerate drawRate = new WeightedFramerate(6);
        WeightedFramerate updateRate = new WeightedFramerate(6);


        readonly bool isDebug = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ==
                       "Debug";

        public static Texture2D Pixel = default!;


        readonly Point[] curveControl = {new Point(200, 200), new Point(400, 200), new Point(400, 400)};
        readonly Point[] lineaControl = {new Point(400, 400), new Point(800, 400)};
        
        BezierSegment bezierSegment;
        LinearSegment linearSegment;

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
    
            
        }


        protected override void Update(GameTime gameTime)
        {
            updateRate.Update();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();    

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

        

    }
}