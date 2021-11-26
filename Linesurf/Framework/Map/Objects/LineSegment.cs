using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Xna.Framework;

namespace Linesurf.Framework.Map.Objects;

public abstract class LineSegment
{
    public readonly Point[] SegmentPoints;
    public float Length;
    protected LineSegment(Point[] points) => SegmentPoints = points;

    protected abstract float GetLength();
    public abstract Vector2[] GetTruncated(int steps);
}
