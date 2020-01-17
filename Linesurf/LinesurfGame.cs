using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using Linesurf.Framework;
using Linesurf.Framework.Map.Objects;
using Linesurf.Framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;


namespace Linesurf
{
    public class LinesurfGame : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch = default!;
        SpriteFont fontNormal = default!;
        WeightedFramerate drawRate = new WeightedFramerate(6);
        WeightedFramerate updateRate = new WeightedFramerate(6);
        Bitmap bitmap = default!;
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

            bitmap = new Bitmap(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawString("Hello from GDI. ジャッズ　ピアノ.  다만, 누구든지 성별·\n露津男分学聞場氏職人説選権家広演。\nतकनीकी सिद्धांत परिभाषित जिवन", new Font(FontFamily.GenericSansSerif, 20f), new SolidBrush(System.Drawing.Color.AntiqueWhite), 0,100);
            }
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Bmp);
            var gdiOut = Texture2D.FromStream(GraphicsDevice, stream);
            //good things about this:
            //it works with Internationale Text
            //it looks nicer than SpriteBatch.DrawString() with spritefonts.
            //bad things:
            //it still looks bad
            //CPU rendered. too much text and we might DIE! of low FPS.
            

            graphics.GraphicsDevice.Clear(Color.Black);        
            spriteBatch.Begin();
            spriteBatch.DrawString(fontNormal,
                $"{(int)drawRate.Framerate}FPS\n{(int)updateRate.Framerate}UPS\n{bezierSegment.Length} bezier length",
                Vector2.Zero, Color.White);

            spriteBatch.DrawSegment(bezierSegment, 20, Color.White);
            spriteBatch.DrawSegment(linearSegment, 20, Color.White);
            spriteBatch.Draw(gdiOut, new Rectangle(0,0,bitmap.Width, bitmap.Height), Color.White);

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