using System.Collections.Immutable;
using Microsoft.Xna.Framework.Media;

namespace Linesurf.Framework.Map
{
    public class Map
    {
        public Map(Song song)
        {
            Song = song;
        }

        public ImmutableArray<LineSegment> LineSegments { get; }
        public Song Song { get; }
        public ImmutableArray<HitObject> Objects { get; }
    }
}