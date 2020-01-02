using System;
using System.Diagnostics;
using Linesurf.Framework.Utils;

namespace Linesurf.Framework
{
    /// <summary>
    /// A basic clock to keep track of the time into a song; Create when the song starts.
    /// </summary>
    public struct MusicClock
    {
        double snapshotElapsed;

        public readonly Stopwatch AudioTime;
        public bool Debounce;
        public float SongOffset;
        public float BpmOffset;

        public float Bpm
        {
            get => MusicUtils.ToBpm(BpmOffset);
            set => BpmOffset = MusicUtils.ToMsOffset(value);
        }

        public readonly TimingPoint[] TimingPoints;
        public int CurrentTimingPointIndex;

        public MusicClock(params TimingPoint[] timingPoints) : this()
        {
            snapshotElapsed = 0;
            TimingPoints = timingPoints;
            AudioTime = new Stopwatch();
            AudioTime.Stop();
            SongOffset = TimingPoints[0].Offset;
            Bpm = TimingPoints[0].Bpm;
            Debounce = false;
        }

        /// <summary>
        /// Returns true if the current millisecond the clock is on is a beat.
        /// </summary>
        /// <param name="updateRate"></param>
        /// <returns></returns>
        public bool CheckBeat(in WeightedFramerate updateRate)
        {
            if (((snapshotElapsed - SongOffset) % BpmOffset) < updateRate.LastMilliseconds)
            {
                if (Debounce)
                    return false;

                LinesurfGame.UpdateToDrawLog += $"T ({snapshotElapsed}ms el - {SongOffset}ms so)[{(snapshotElapsed - SongOffset)}] % {BpmOffset}ms bpm[{(snapshotElapsed - SongOffset) % BpmOffset}] < {updateRate.LastMilliseconds}ms tolerance ";

                return Debounce = true;
            }
            return Debounce = false;
        }


        /// <summary>
        /// Takes a snapshot of the current elapsed time and updates timing if required.
        /// Should be called before any other operations are done.
        /// </summary>
        public void Snapshot(in WeightedFramerate updateRate)
        {
            snapshotElapsed = AudioTime.Elapsed.TotalMilliseconds;

            if (CurrentTimingPointIndex + 1 == TimingPoints.Length) return;

            ref var nextTp = ref TimingPoints[CurrentTimingPointIndex + 1];
            if (Math.Abs(snapshotElapsed - nextTp.Offset) < updateRate.LastMilliseconds)
            {
                CurrentTimingPointIndex++;
                SongOffset = nextTp.Offset;
                Bpm = nextTp.Bpm;
                Debounce = false;
                LinesurfGame.UpdateToDrawLog += "TC ";
            }
        }
    }

    public readonly struct TimingPoint
    {
        public readonly float Offset;
        public readonly float Bpm;

        public TimingPoint(float offset, float bpm)
        {
            Offset = offset;
            Bpm = bpm;
        }
    }
}