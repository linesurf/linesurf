using System;
using System.Collections.Generic;
using Linesurf.Framework.Utils;
using Microsoft.Xna.Framework;

namespace Linesurf.Framework.Map.Objects;

public sealed class LinearSegment : LineSegment
{
    public LinearSegment(Point[] points) : base(points)
    {
        if (points.Length != 2) throw new ArgumentException("Line initialized without 2 points.");

        Length = GetLength();
    }

    protected override float GetLength() => MathUtils.LinearDistance(SegmentPoints[0], SegmentPoints[1]);

    public override Vector2[] GetTruncated(int steps)
    {
        if (steps < 2) throw new ArgumentException("Truncated line calcualted with < 2 steps");

        float t;
        float x;
        float y;
        var vector2s = new Vector2[steps];

        for (var i = 0; i < steps; i++)
        {
            t = MathUtils.Map(i, 0, steps - 1, 0, 1);
            x = (1 - t) * SegmentPoints[0].X + t * SegmentPoints[1].X;
            y = (1 - t) * SegmentPoints[0].Y + t * SegmentPoints[1].Y;
            vector2s[i] = new Vector2(x, y);
        }
        return vector2s;
    }
}
