using System;
using System.Diagnostics;

namespace Linesurf
{
    public struct WeightedFramerate
    {
        double currentFrametimes;
        double weight;
        int numerator;

        public Stopwatch Stopwatch { get; }

        public TimeSpan LastLatency { get; private set; }

        public double Framerate => numerator / currentFrametimes is var framerate && double.IsInfinity(framerate) ? 0 : framerate;

        public WeightedFramerate(int oldFrameWeight)
        {
            LastLatency = default;
            Stopwatch = new Stopwatch();
            currentFrametimes = 0;
            numerator = oldFrameWeight;
            weight = oldFrameWeight / (oldFrameWeight - 1d);
        }

        public void Update()
        {
            LastLatency = Stopwatch.Elapsed;
            var timeSinceLastFrame = Stopwatch.Elapsed.TotalSeconds;
            currentFrametimes /= weight;
            currentFrametimes += timeSinceLastFrame;
            Stopwatch.Restart();
        }
    }
}
