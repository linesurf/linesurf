using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SpriteFontPlus;

namespace Linesurf.Framework.UI.Elements
{
    public class TextButton : UIElement
    {
        DynamicSpriteFont font;
        string text;
        Color textColor;
        Color hoverColor;
        Color bgColor;
        Color drawColor;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                Rectangle.Size = font.MeasureString(value).ToPoint();
                Rectangle.Inflate(10,0);
            }
        }


        /// <inheritdoc/>
        /// <param name="sf">The font to use for the text</param>
        /// <param name="t">The text of the button</param>
        /// <param name="tc">The color of the text when not hovering</param>
        /// <param name="hc">The color of the text when hovering</param>
        /// <param name="bg">The background color</param>
        public TextButton(float x, float y, DynamicSpriteFont sf, string t, Color tc, Color hc, Color bg) : base(x, y)
        {
            font = sf;
            Text = t;
            textColor = tc;
            hoverColor = hc;
            bgColor = bg;
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {
            drawColor = Hovering() ? hoverColor : textColor;
            CheckClicked();
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.DrawRectangle(Rectangle, bgColor);
            spriteBatch.DrawString(font,Text,position,drawColor);
        }
    }
}