namespace Linesurf.Framework.Map
{
    public class Arrow : HitObject
    {
        public ArrowDirection Direction { get; }
    }

    public enum ArrowDirection
    {
            Up,
        Left, Right,
           Down
    }
}