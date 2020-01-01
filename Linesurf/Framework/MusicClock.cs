using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Linesurf.Framework.Utils;

namespace Linesurf.Framework
{
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

        //TODO: Make a dictionary-type thing where we can just address millisecond offsets with a certain tolerance.
        public readonly TimingPoint[] TimingPoints;

        /// <summary>
        /// A basic clock to keep track of the time into a song; Create when the song starts.
        /// </summary>
        /// 

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
        public bool CheckBeat(ref WeightedFramerate updateRate)
        {
            if (Math.Abs((snapshotElapsed - SongOffset) % BpmOffset) < 
                updateRate.LastMilliseconds)
            {
                if (Debounce) return false;
                Debounce = true;
                return true;
            }

            Debounce = false;
            return false;
        }

        
        /// <summary>
        /// Takes a snapshot of the current elapsed time and updates timing if required.
        /// Should be called before any other operations are done.
        /// </summary>
        public void Snapshot(ref WeightedFramerate updateRate)
        {
            snapshotElapsed = AudioTime.Elapsed.TotalMilliseconds;
            foreach (var t in TimingPoints)
            {
                if (Math.Abs(snapshotElapsed - t.Offset) < updateRate.LastMilliseconds)
                {
                    SongOffset = t.Offset;
                    Bpm = t.Bpm;
                    Debounce = true;
                    return; //no need to look through other stuff
                }
            }
        }
    }

    public struct TimingPoint
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