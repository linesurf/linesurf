using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Linesurf.Framework.Map
{
    public class LineSegment
    {
        public List<Point> SegmentPoints;
        public CurveType CurveType;
    }

    public enum CurveType
    {
        Linear,
        Bezier,
        ThatOneUglyAssCurveThatOldOsuUses
    }
}