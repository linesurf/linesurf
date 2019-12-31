using Microsoft.Xna.Framework;

namespace Linesurf.Framework.Utils
{
    public static class ColorUtils
    {
        public static Color Gray(byte b, byte a = 0xFF) => new Color(b,b, b,a);


        public static Color Darken(this Color color, float percent, bool reduceAlpha = false)
            => new Color(color.R.MinusPercent(percent),
                         color.G.MinusPercent(percent),
                         color.B.MinusPercent(percent),
                         reduceAlpha ? color.A.MinusPercent(percent): color.A);
    }
}