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
        public static readonly int[] idxBlackNote = new int[]
        {1,3,6,8,10};

        public static readonly int octaveSize = 12;

        public static Note IdToNote(int id, int intensity, int tick)
        {
            Note note = new Note();
            note.Octave = (byte)(id / octaveSize - 1);
            note.High = (byte)(id % octaveSize);
            note.HighString = tabNote[id % octaveSize];
            note.Tick = tick;
            note.Intensity = (byte)intensity;
            return note;
        } 

    }
}
