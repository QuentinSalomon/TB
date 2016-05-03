using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Note
    {
        public Note() { }
        public Note(byte pitch, UInt32 tick)
        {
            Pitch = pitch;
            Tick = tick;
        }

        public byte Pitch { get; set; }
        public UInt32 Tick { get; set; }
    }
}
