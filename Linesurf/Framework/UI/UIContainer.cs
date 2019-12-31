using System.Collections.Immutable;

namespace Linesurf.Framework.UI
{
    public abstract class UIContainer : UIElement
    {
        public ImmutableArray<UIElement> Elements { get; }
    }
}