using System.Reflection;
using Linesurf.Framework;
using Linesurf.Framework.UI;
using Linesurf.Framework.UI.Elements;
using Microsoft.Xna.Framework.Input;
using SpriteFontPlus;

namespace Linesurf;

public class LinesurfGame : Game
{
    readonly GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch = default!;
    SpriteFont fontNormal = default!;
    WeightedFramerate drawRate = new WeightedFramerate(6);
    WeightedFramerate updateRate = new WeightedFramerate(6);
    readonly bool isDebug = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ==
                   "Debug";

    DynamicSpriteFont dynFontNormal = default!;
    UI test = default!;
    Label testFPS = default!;

    public LinesurfGame()
    {
        graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = true;
        graphics.SynchronizeWithVerticalRetrace = true;
        TargetElapsedTime = TimeSpan.FromMilliseconds(1);
        graphics.PreparingDeviceSettings += (sender, args) =>
        {
            graphics.PreferMultiSampling = true;
            args.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 2;
        };
    }

    protected override void Initialize()
    {
        graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        fontNormal = Content.Load<SpriteFont>("fontnormal");

        dynFontNormal = DynamicSpriteFont.FromTtf(typeof(Program).Assembly.GetManifestResourceStream("Raleway-Regular.ttf"), 30);
        test = new UI(spriteBatch, graphics);
        test.AddElement(new Label(30, 30, dynFontNormal, "Laaaaaaa", Color.Red, Color.CornflowerBlue));
        testFPS = new Label(0, 0, dynFontNormal, "not calculated yet lol", Color.White);
        test.AddElement(testFPS);
        test.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        updateRate.Update();
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        base.Update(gameTime);
        test.Update();
    }

    protected override void Draw(GameTime gameTime)
    {
        drawRate.Update();
        GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin();
        testFPS.Text = $"{(int)drawRate.Framerate}FPS\n{(int)updateRate.Framerate}";
        test.Draw();

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
