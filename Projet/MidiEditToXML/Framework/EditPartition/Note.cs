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
    [IntlConceptName("Framework.Note.Name", "Note")]
    [ConceptSmallImage(typeof(Note), "/Images/Note32x32.png")]
    [ConceptLargeImage(typeof(Note), "/Images/Note64x64.png")]
    public class Note : ConceptComponent
    {
        #region Constructeur

        public Note()
        {

        }

        public Note(Note n)
        {
            Name = n.Name;
            Octave = n.Octave;
            Tick = n.Tick;
            High = n.High;
            HighString = n.HighString;
            Intensity = n.Intensity;
        }

        #endregion

        #region Propriétés

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.Note.Octave", "Octave")]
        public byte Octave
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
        private byte _octave;
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
        //[ConceptViewVisible]
        [IntlConceptName("Framework.Note.High", "Note")]
        public byte High
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
        private byte _high;
        public const string HighPropertyName = "High";

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.Note.HighString", "Note")]
        public string HighString
        {
            get { return _highString; }
            set
            {
                if (_highString != value)
                {
                    _highString = value;
                    DoPropertyChanged(HighStringPropertyName);
                }
            }
        }
        private string _highString;
        public const string HighStringPropertyName = "HighString";

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.Note.Intensity", "Intensity")]
        public byte Intensity
        {
            get { return _intensity; }
            set
            {
                if (_intensity != value)
                {
                    _intensity = value;
                    DoPropertyChanged(IntensityPropertyName);
                }
            }
        }
        private byte _intensity;
        public const string IntensityPropertyName = "Intensity";

        #endregion

    }

    [ConceptSmallImage(typeof(StaticListNote), "/Images/Note32x32.png")]
    [ConceptLargeImage(typeof(StaticListNote), "/Images/Note64x64.png")]
    public class StaticListNote : ConceptStaticList<Note>
    { }
}
