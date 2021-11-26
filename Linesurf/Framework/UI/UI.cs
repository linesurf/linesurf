using Linesurf.Framework.UI.Elements;

namespace Linesurf.Framework.UI;

public class UI
{
    readonly List<UIElement> elements = new List<UIElement>();
    readonly SpriteBatch spriteBatch;
    readonly GraphicsDeviceManager graphics;

    public UI(SpriteBatch sb, GraphicsDeviceManager g)
    {
        spriteBatch = sb;
        graphics = g;
    }

    public void AddElement(UIElement e) => elements.Add(e);

    public void Initialize()
    {
        foreach (var e in elements)
        {
            e.Initialize();
        }
    }

    public void Update()
    {
        foreach (var e in elements)
        {
            e.Update();
        }
    }

    public void Draw()
    {
        foreach (var e in elements)
        {
            e.Draw(spriteBatch, graphics);
        }
    }

}
