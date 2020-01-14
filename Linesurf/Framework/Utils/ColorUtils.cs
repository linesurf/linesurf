﻿using Microsoft.Xna.Framework;

namespace Linesurf.Framework.Utils
{
    public static class ColorUtils
    {
        public static Color Gray(byte b, byte a = 0xFF) => new Color(b, b, b, a);


        public static Color Darken(this Color color, float percent, bool reduceAlpha = false)
            => new Color(color.R.MinusPercent(percent),
                color.G.MinusPercent(percent),
                color.B.MinusPercent(percent),
                reduceAlpha ? color.A.MinusPercent(percent) : color.A);
        
        public static Color Brighten(this Color color, float percent, bool augmentAlpha = false)
            => new Color(color.R.PlusPercent(percent),
                color.G.PlusPercent(percent),
                color.B.PlusPercent(percent),
                augmentAlpha ? color.A.PlusPercent(percent) : color.A);
    }
}