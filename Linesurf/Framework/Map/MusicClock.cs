using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Linesurf.Framework.Utils;
namespace Linesurf.Framework.Map
{
   
    
    public struct MusicClock 
    {
        public Stopwatch audioStart;
        public bool debounce;
        public float songOffset;
        public float bpmOffset;
        public float bpm;
        /// <summary>
        /// A basic clock to keep track of the time into a song; Create when the song starts.
        /// </summary>
        /// <param name="Offset">The exact time when the song starts in milliseconds.</param>
        /// <param name="songbpm">The beats per minute of the song.</param>
        public MusicClock(float Offset, float songbpm)
        {
            audioStart = new Stopwatch();
            bpmOffset = MusicUtils.ToMsOffset(songbpm);
            bpm = songbpm;
            songOffset = Offset;
            debounce = false;
        }
        public bool CheckBeat(ref WeightedFramerate updateRate)
        {
            if ((audioStart.Elapsed.TotalMilliseconds - songOffset) % bpmOffset < updateRate.LastLatency.TotalMilliseconds)
            {
                if (!debounce)
                {
                    
                    debounce = true;
                    return true;
                }
            }
            else
            {
                debounce = false;
                return false;
            }
            return false;
        }
    }
}
