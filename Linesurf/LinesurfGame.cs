using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
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

        PrivateFontCollection privateFontCollection = new PrivateFontCollection();
        Font fontNormalGDI;
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
            
            privateFontCollection.AddFontFile(@".\Content\Raleway-Regular.ttf");
            fontNormalGDI = new Font(privateFontCollection.Families[0], 14, FontStyle.Regular);
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
            //GDI+ (System.Drawing.Common)
            bitmap = new Bitmap(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawString("Hello from GDI.", fontNormalGDI , Brushes.White, 0,100);
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
            //for reference. it seems like on my pc at least 300 is about the maximum without losing FPS.
            //also, many of the type names conflict heavily with monogame making everything very confusing (Color, Rectangle, Point, etc.)
            
            graphics.GraphicsDevice.Clear(Color.Black);        
            spriteBatch.Begin();
            spriteBatch.DrawString(fontNormal,
                $"{(int)drawRate.Framerate}FPS\n{(int)updateRate.Framerate}UPS\n{bezierSegment.Length} bezier length",
                Vector2.Zero, Color.White);
            
            
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