using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    static class NotesConvert
    {
        public static readonly string[] tabNote = new string[] 
        { "DO", "DO#", "RE", "RE#", "MI", "FA", "FA#", "SOL", "SOL#", "LA", "LA#", "SI" };

        public static Note IdToNote(int id, int intensity, int tick)
        {
            Note note = new Note();
            note.Octave = (byte)(id / 12 -1);
            note.High = (byte)(id % 12);
            note.HighString = tabNote[id % 12];
            note.Tick = tick;
            note.Intensity = (byte)intensity;
            return note;
        } 

    }
}
