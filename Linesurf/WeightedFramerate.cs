using System;

namespace Linesurf
{
    public struct WeightedFramerate
    {
        double currentFrametimes;
        double weight;
        int numerator;

        public DateTime LastUpdate { get; private set; }

        public double Framerate => numerator / currentFrametimes is var framerate && double.IsInfinity(framerate) ? 0 : framerate;

        public WeightedFramerate(int oldFrameWeight)
        {
            LastUpdate = DateTime.Now;
            currentFrametimes = 0;
            numerator = oldFrameWeight;
            weight = oldFrameWeight / (oldFrameWeight - 1d);
        }

        public void Update()
        {
            var timeSinceLastFrame = (DateTime.Now - LastUpdate).TotalSeconds;
            currentFrametimes /= weight;
            currentFrametimes += timeSinceLastFrame;
            LastUpdate = DateTime.Now;
        }
    }
}
