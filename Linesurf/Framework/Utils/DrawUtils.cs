using Linesurf.Framework.Map.Objects;
using MonoGame.Extended;

namespace Linesurf.Framework.Utils;

public static class DrawUtils
{
    public static void DrawSegment(this SpriteBatch spriteBatch, LineSegment bc, int steps, Color color, float thickness = 1f, int layerDepth = 0)
    {
        var v2s = bc.GetTruncated(steps);
        for (var i = 0; i < steps - 1; i++) spriteBatch.DrawLine(v2s[i], v2s[i + 1], color, thickness, layerDepth);
    }
}
