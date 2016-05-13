using Concept.Model;
using Concept.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    //[ConceptView(typeof(UserControlNote))]
    [IntlConceptName("Framework.Note.Name", "NotesMidi")]
    [ConceptSmallImage(typeof(PartitionMidi), "/Images/Note32x32.png")]
    [ConceptLargeImage(typeof(PartitionMidi), "/Images/Note64x64.png")]
    public class Note : ConceptComponent
    {

        #region Propriétés

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.Note.Octave", "Octave")]
        public double Octave
        {
            get { return _octave; }
            set
            {
                if (_octave != value)
                {
                    _octave = value;
                    DoPropertyChanged(OctavePropertyName);
                }
            }
        }
        private double _octave;
        public const string OctavePropertyName = "Octave";

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.Note.Tick", "Tick")]
        public int Tick
        {
            get { return _tick; }
            set
            {
                if (_tick != value)
                {
                    _tick = value;
                    DoPropertyChanged(TickPropertyName);
                }
            }
        }
        private int _tick;
        public const string TickPropertyName = "Tick";


        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.Note.Note", "Note")]
        public string High
        {
            get { return _high; }
            set
            {
                if (_high != value)
                {
                    _high = value;
                    DoPropertyChanged(HighPropertyName);
                }
            }
        }
        private string _high;
        public const string HighPropertyName = "High";
        #endregion

    }

    [ConceptSmallImage(typeof(StaticListNote), "/Images/Note32x32.png")]
    [ConceptLargeImage(typeof(StaticListNote), "/Images/Note64x64.png")]
    public class StaticListNote : ConceptStaticList<Note>
    { }
}
