namespace Linesurf.Framework.Map.Objects;

public class Arrow : HitObject
{
    public ArrowDirection Direction { get; }
}

public enum ArrowDirection
{
    Up,
    Left,
    Right,
    Down
}
