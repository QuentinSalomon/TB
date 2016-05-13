﻿using Concept.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    //public class Note
    //{
    //    public Note() { }
    //    public Note(byte pitch, UInt32 tick)
    //    {
    //        Pitch = pitch;
    //        Tick = tick;
    //    }

    //    public byte Pitch { get; set; }
    //    public UInt32 Tick { get; set; }
    //}

    [IntlConceptName("Common.Note.Name", "NotesMidi")]
    [ConceptSmallImage(typeof(Note), "/Images/Note32x32.png")]
    [ConceptLargeImage(typeof(Note), "/Images/Note64x64.png")]
    public class Note : ConceptComponent
    {

        #region Propriétés

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Common.Note.Octave", "Octave")]
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
        [IntlConceptName("Common.Note.Tick", "Tick")]
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
        [IntlConceptName("Common.Note.High", "High")]
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
        [IntlConceptName("Common.Note.HighString", "HighString")]
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

        #endregion

    }

    [ConceptSmallImage(typeof(StaticListNote), "/Images/Note32x32.png")]
    [ConceptLargeImage(typeof(StaticListNote), "/Images/Note64x64.png")]
    public class StaticListNote : ConceptStaticList<Note>
    { }
}
