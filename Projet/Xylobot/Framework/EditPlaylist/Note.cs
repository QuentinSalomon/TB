using Concept.Model;

namespace Framework
{
    [IntlConceptName("Framework.Note.Name", "NotesMidi")]
    public class Note : ConceptComponent
    {
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
        [IntlConceptName("Framework.Note.High", "High")]
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
        [IntlConceptName("Framework.Note.HighString", "HighString")]
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

    public class StaticListNote : ConceptStaticList<Note>
    { }
}
