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

        public static Note IdToNote(int id, int tick)
        {
            Note note = new Note();
            note.Octave = id / 12;
            note.High = tabNote[id % 12];
            note.Tick = tick;
            return note;
        }

    }

    //class Note
    //{
    //    public string Name { get; set; }
    //    public string FullName { get; set; }
    //    public int Octave { get; set; }
    //    public long Tick { get; set; }
    //    public long Duration { get; set; }

    //    public override string ToString()
    //    {
    //        return FullName;
    //    }
    //}
}
