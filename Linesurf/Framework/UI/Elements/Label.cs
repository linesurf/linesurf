using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace Linesurf.Framework.UI.Elements
{
    public class Label : UIElement
    {
        DynamicSpriteFont font;
        string text;
        Color textColor;
        Color hoverColor;
        Color drawColor;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                Rectangle.Size = font.MeasureString(value).ToPoint();
            }
        }


        /// <inheritdoc/>
        /// <param name="sf">The font to use for the text</param>
        /// <param name="t">The text of the button</param>
        /// <param name="tc">The color of the text when not hovering</param>
        /// <param name="hc">The color of the text when hovering</param>
        public Label(float x, float y, DynamicSpriteFont sf, string t, Color tc, Color hc) : base(x, y)
        {
            font = sf;
            Text = t;
            textColor = tc;
            hoverColor = hc;
        }
        /// <inheritdoc/>
        /// <param name="sf">The font to use for the text</param>
        /// <param name="t">The text of the button</param>
        /// <param name="tc">The color of the text</param>
        public Label(float x, float y, DynamicSpriteFont sf, string t, Color tc) : base(x, y)
        {
            font = sf;
            Text = t;
            textColor = tc;
            hoverColor = tc;
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
            spriteBatch.DrawString(font,Text,position,drawColor);
        }
    }
}