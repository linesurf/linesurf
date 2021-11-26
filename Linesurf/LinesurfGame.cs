using System.Reflection;
using FontStashSharp;
using Linesurf.Framework;
using Linesurf.Framework.UI;
using Linesurf.Framework.UI.Elements;
using Linesurf.Framework.Utils;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Linesurf;

public class LinesurfGame : Game
{
    readonly GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch = default!;
    SpriteFont fontNormal = default!;
    FontSystem fontSystem = default!;
    WeightedFramerate drawRate = new(6);
    WeightedFramerate updateRate = new(6);
    readonly bool isDebug = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ==
                   "Debug";

    bool timerOn = false;
    readonly SoundEffect effect = default!;
    readonly Song song = default!;

    DynamicSpriteFont dynFontNormal = default!;
    UI test = default!;
    Label testFPS = default!;

    MusicClock musicClock = new(
     new TimingPoint(54, 120),
     new TimingPoint(44554, 115),
     new TimingPoint(44554 + MusicUtils.ToMsOffset(115), 110),
     new TimingPoint(44554 + MusicUtils.ToMsOffset(115) + MusicUtils.ToMsOffset(110), 105),
     new TimingPoint(44554 + MusicUtils.ToMsOffset(115) + MusicUtils.ToMsOffset(110) + MusicUtils.ToMsOffset(105), 100),
     new TimingPoint(44554 + MusicUtils.ToMsOffset(115) + MusicUtils.ToMsOffset(110) + MusicUtils.ToMsOffset(105) + MusicUtils.ToMsOffset(110), 95f),
     new TimingPoint(44554 + MusicUtils.ToMsOffset(115) + MusicUtils.ToMsOffset(110) + MusicUtils.ToMsOffset(105) + MusicUtils.ToMsOffset(110) + MusicUtils.ToMsOffset(95), 90),
     new TimingPoint(58750, 96),
     new TimingPoint(58750 + MusicUtils.ToMsOffset(96), 102),
     new TimingPoint(58750 + MusicUtils.ToMsOffset(96) + MusicUtils.ToMsOffset(102), 108),
     new TimingPoint(58750 + MusicUtils.ToMsOffset(96) + MusicUtils.ToMsOffset(102) + MusicUtils.ToMsOffset(108), 114),
     new TimingPoint(58750 + MusicUtils.ToMsOffset(96) + MusicUtils.ToMsOffset(102) + MusicUtils.ToMsOffset(108) + MusicUtils.ToMsOffset(114), 120));

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

        effect = Content.Load<SoundEffect>("normal-hitnormal");
        song = Content.Load<Song>("music");
        MediaPlayer.MediaStateChanged += (sender, e) => { timerOn = true; };

        MediaPlayer.Play(song);
        musicClock.AudioTime.Restart();
        MediaPlayer.Volume = 0.175f;
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

        fontSystem = new();
        fontSystem.AddFont(typeof(Program).Assembly.GetManifestResourceStream("Raleway-Regular.ttf"));

        dynFontNormal = fontSystem.GetFont(30);

        test = new UI(spriteBatch, graphics);
        test.AddElement(new Label(30, 30, dynFontNormal, "Laaaaaaa", Color.Red, Color.CornflowerBlue));
        testFPS = new Label(0, 0, dynFontNormal, "not calculated yet lol", Color.White);
        test.AddElement(testFPS);
        test.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        updateRate.Update();
        musicClock.Snapshot(updateRate);

        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (timerOn)
        {
            if (musicClock.CheckBeat(updateRate))
            {
                effect.Play(0.20f, 0f, -1f);

            }
        }

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

    protected override void OnDeactivated(object sender, EventArgs args)
    {
        MediaPlayer.Pause();
        musicClock.AudioTime.Stop();
        base.OnDeactivated(sender, args);
    }

    protected override void OnActivated(object sender, EventArgs args)
    {
        MediaPlayer.Resume();
        musicClock.AudioTime.Start();
        base.OnActivated(sender, args);
    }
}
