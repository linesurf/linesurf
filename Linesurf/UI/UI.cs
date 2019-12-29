using System.Collections.Generic;
using Linesurf.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Linesurf.UI
{
    public class UI
    {
        readonly List<UIElement> elements = new List<UIElement>();
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;

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
}