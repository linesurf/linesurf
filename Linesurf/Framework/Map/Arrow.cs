namespace Linesurf.Framework.Map
{
    public class Arrow : HitObject
    {
        public ArrowDirection Direction;
    }

    public enum ArrowDirection
    {
        Up,Down,Left,Right
    }
}