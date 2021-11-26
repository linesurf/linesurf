namespace Linesurf.Framework.Utils;

public static class MusicUtils
{
    public static float ToMsOffset(float bpm) => 60_000f / bpm;

    public static float ToBpm(float ms) => 60_000f / ms;
}
