using System.Collections.Immutable;
using Microsoft.Xna.Framework;

namespace Linesurf.Framework.Map
{
    public class LineSegment
    { 
        public ImmutableArray<Point> SegmentPoints { get; }
        public CurveType CurveType { get; }
    }

    public enum CurveType
    {
        Linear,
        Bezier,
    }
}