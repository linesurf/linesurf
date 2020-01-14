using System;

namespace Linesurf.Framework.Utils
{
    public static class MathUtils
    {
        public static float Map(this float value, float fromLow, float fromHigh, float toLow, float toHigh) =>
            (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;

        public static float MinusPercent(this float value, float percentage) => value - value * (percentage / 100);

        public static byte MinusPercent(this byte value, float percentage) =>
            (byte) (value - value * (percentage / 100));

        public static byte PlusPercent(this byte value, float percentage) =>
            (byte) MathF.Max(value + value * percentage / 100, 255);
    }
}