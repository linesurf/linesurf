using System;
using System.Diagnostics;

namespace Linesurf
{
    public struct WeightedFramerate
    {
        float currentFrametimes;
        readonly float weight;
        readonly int numerator;

        public Stopwatch Stopwatch { get; }

        public TimeSpan LastLatency { get; private set; }

        public float Framerate => numerator / currentFrametimes is var framerate && float.IsInfinity(framerate)
            ? 0
            : framerate;

        public WeightedFramerate(int oldFrameWeight)
        {
            LastLatency = default;
            Stopwatch = new Stopwatch();
            currentFrametimes = 0;
            numerator = oldFrameWeight;
            weight = oldFrameWeight / (oldFrameWeight - 1f);
        }

        public void Update()
        {
            LastLatency = Stopwatch.Elapsed;
            var timeSinceLastFrame = (float) Stopwatch.Elapsed.TotalSeconds;
            currentFrametimes /= weight;
            currentFrametimes += timeSinceLastFrame;
            Stopwatch.Restart();
        }
    }
}