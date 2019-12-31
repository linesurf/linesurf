using System.Diagnostics;
using Linesurf.Framework.Utils;

namespace Linesurf.Framework
{
    public struct MusicClock
    {
        public readonly Stopwatch AudioStart;
        public bool Debounce;
        public float SongOffset;
        public float BpmOffset;
        public float Bpm;

        /// <summary>
        /// A basic clock to keep track of the time into a song; Create when the song starts.
        /// </summary>
        /// <param name="offset">The exact time when the song starts in milliseconds.</param>
        /// <param name="songBpm">The beats per minute of the song.</param>
        public MusicClock(float offset, float songBpm)
        {
            AudioStart = new Stopwatch();
            BpmOffset = MusicUtils.ToMsOffset(songBpm);
            Bpm = songBpm;
            SongOffset = offset;
            Debounce = false;
        }

        public bool CheckBeat(ref WeightedFramerate updateRate)
        {
            if ((AudioStart.Elapsed.TotalMilliseconds - SongOffset) % BpmOffset <
                updateRate.LastLatency.TotalMilliseconds)
            {
                if (Debounce) return false;
                Debounce = true;
                return true;
            }

            Debounce = false;
            return false;
        }
    }
}