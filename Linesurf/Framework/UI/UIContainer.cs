using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace Linesurf.Framework.UI
{
    public abstract class UIContainer : UIElement
    {
        public List<UIElement> Elements = new List<UIElement>();

    }
}