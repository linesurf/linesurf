using System;
using Microsoft.Xna.Framework;

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

        public static float LinearDistance(Point a, Point b) => 
            MathF.Sqrt(MathF.Pow(a.X - b.X, 2) + MathF.Pow(a.Y - b.Y, 2));
        public static float LinearDistance(Vector2 a, Vector2 b) => 
            MathF.Sqrt(MathF.Pow(a.X - b.X, 2) + MathF.Pow(a.Y - b.Y, 2));
        
        public static Vector2[] QuadraticBezierPoints(int steps, params Point[] p)
        {
            if (p.Length != 3) throw new ArgumentException("For quadratic Bézier curve calculations number of points given must be 3.");
            if(steps < 2) throw new ArgumentException("There must be at least two steps for Bézier calculation.");
            
            float t;
            float x;
            float y;
            var vector2s = new Vector2[steps];
            
            for (var i = 0; i < steps; i++)
            {
                t = MathUtils.Map(i, 0, steps - 1, 0, 1);
                x = (1 - t) * (1 - t) * p[0].X + 2 * (1 - t) * t * p[1].X + t * t * p[2].X;
                y = (1 - t) * (1 - t) * p[0].Y + 2 * (1 - t) * t * p[1].Y + t * t * p[2].Y;
                vector2s[i] = new Vector2(x,y);
            }
            return vector2s;
        }
        
        
        //based on https://malczak.linuxpl.com/blog/quadratic-bezier-curve-length/
        public static float BezierLength(params Point[] p)
        {
            if (p.Length != 3) throw new ArgumentException("For quadratic Bézier curve calculations number of points given must be 3.");
            //definition
            Point a, b;
            a.X = p[0].X - 2 * p[1].X + p[2].X;
            a.Y = p[0].Y - 2 * p[1].Y + p[2].Y;
            b.X = 2 * p[1].X - 2 * p[0].X;
            b.Y = 2 * p[1].Y - 2 * p[0].Y;
            var A = 4 * (MathF.Pow(a.X, 2) + MathF.Pow(a.Y, 2));
            var B = 4f * (a.X * b.X + a.Y * b.Y); //needs to be 4f since otherwise it goes very large and like yeah nope
            var C = MathF.Pow(b.X, 2) + MathF.Pow(b.Y, 2);
            
            //precalculations
            var twoSqrtABC = 2 * MathF.Sqrt(A + B + C);
            var sqrtA = MathF.Sqrt(A);
            var sqrtACubed = 2 * A * sqrtA;
            var twoSqrtC = 2 * MathF.Sqrt(C);
            var BOverSqrtA = B / sqrtA;

            return (sqrtACubed * twoSqrtABC + sqrtA * B * (twoSqrtABC - twoSqrtC) +
                    (4 * C * A - B * B) * MathF.Log((2 * sqrtA + BOverSqrtA + twoSqrtABC) / (BOverSqrtA + twoSqrtC))) / (4 * sqrtACubed);
        }
    }
}