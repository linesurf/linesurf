using System;
using System.Collections.Generic;
using Linesurf.Framework.Utils;
using Microsoft.Xna.Framework;

namespace Linesurf.Framework.Map.Objects
{
    public sealed class BezierSegment : LineSegment
    {
        public BezierSegment(Point[] points) : base(points)
        {
            if (points.Length != 3) throw new ArgumentException("Bézier curve initialized without 3 points.");

            Length = GetLength();
        }

        
        //based on https://malczak.linuxpl.com/blog/quadratic-bezier-curve-length/
        protected override float GetLength()
        {
            //definitions
            Point a, b;
            a.X = SegmentPoints[0].X - 2 * SegmentPoints[1].X + SegmentPoints[2].X;
            a.Y = SegmentPoints[0].Y - 2 * SegmentPoints[1].Y + SegmentPoints[2].Y;
            b.X = 2 * SegmentPoints[1].X - 2 * SegmentPoints[0].X;
            b.Y = 2 * SegmentPoints[1].Y - 2 * SegmentPoints[0].Y;
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

        public override Vector2[] GetTruncated(int steps)
        {           
            if(steps < 2) throw new ArgumentException("Truncated Bézier cannot be calcualted with < 2 steps");
            
            float t;
            float x;
            float y;
            var vector2s = new Vector2[steps];
            
            for (var i = 0; i < steps; i++)
            {
                t = MathUtils.Map(i, 0, steps - 1, 0, 1);
                x = (1 - t) * (1 - t) * SegmentPoints[0].X + 2 * (1 - t) * t * SegmentPoints[1].X + t * t * SegmentPoints[2].X;
                y = (1 - t) * (1 - t) * SegmentPoints[0].Y + 2 * (1 - t) * t * SegmentPoints[1].Y + t * t * SegmentPoints[2].Y;
                vector2s[i] = new Vector2(x,y);
            }
            return vector2s;
        }
    }
}