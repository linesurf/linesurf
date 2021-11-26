using Microsoft.Xna.Framework.Input;

namespace Linesurf.Framework.UI.Elements;

public abstract class UIElement
{
    protected Rectangle Rectangle = Rectangle.Empty;
    MouseState oldState = default;


    protected Vector2 position;
    /// <summary>
    /// The position of the element as a Vector2
    /// </summary>
    public Vector2 Position
    {
        get => position;
        set
        {
            position = value;
            Rectangle.Offset(value);
        }
    }

    /// <param name="v">The position vector</param>
    public UIElement(float x, float y)
    {
        Position = new Vector2(x, y);
    }

    public abstract void Initialize();

    public abstract void Update();

    public abstract void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics);

    protected void CheckClicked()
    {
        if (Rectangle.Intersects(new Rectangle(Mouse.GetState().Position, Point.Zero)))
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                OnClick(EventArgs.Empty);
            }
        }

        oldState = Mouse.GetState();
    }

    protected bool Hovering() => Rectangle.Intersects(new Rectangle(Mouse.GetState().Position, Point.Zero));

    public event EventHandler? Click;

    protected virtual void OnClick(EventArgs e) => Click?.Invoke(this, e);

}
