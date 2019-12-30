using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Linesurf.UI.Elements
{
    public class TextButton : UIElement
    {
        private SpriteFont font;
        private string text = default!;
        private Color textColor;
        private Color hoverColor;
        private Color bgColor;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                Rectangle.Size = font.MeasureString(value).ToPoint();
                Rectangle.Inflate(10, 0);
            }
        }


        /// <inheritdoc/>
        /// <param name="sf">The font to use for the text</param>
        /// <param name="t">The text of the button</param>
        /// <param name="tc">The color of the text when not hovering</param>
        /// <param name="hc">The color of the text when hovering</param>
        /// <param name="bg">The background color</param>
        public TextButton(float x, float y, SpriteFont sf, string t, Color tc, Color hc, Color bg) : base(x, y)
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
            textColor = Hovering() ? Color.Yellow : Color.White;
            CheckClicked();
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(LinesurfGame.Pixel, Rectangle, bgColor);
            spriteBatch.DrawString(font, Text, position, textColor);
        }
    }
}