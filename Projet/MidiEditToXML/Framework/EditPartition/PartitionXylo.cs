using Concept.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [IntlConceptName("Framework.PartitionXylo.Name", "PartitionXylo")]
    [ConceptSmallImage(typeof(PartitionXylo), "/Images/Partition32x32.png")]
    [ConceptLargeImage(typeof(PartitionXylo), "/Images/Partition64x64.png")]
    public class PartitionXylo : ConceptComponent
    {
        #region Constructeur

        public PartitionXylo()
        {

        }

        #endregion

        #region Propriétés

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.PartitionXylo.Title", "Title")]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    DoPropertyChanged(TitlePropertyName);
                }
            }
        }
        private string _title;
        public const string TitlePropertyName = "Title";

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.PartitionXylo.Tempo", "Tempo")]
        public double Tempo
        {
            get { return _tempo; }
            set
            {
                if (_tempo != value)
                {
                    _tempo = value;
                    DoPropertyChanged(TempoPropertyName);
                }
            }
        }
        private double _tempo;
        public const string TempoPropertyName = "Tempo";

        #endregion

        [ConceptSerialized]
        [ConceptAutoCreate]
        [IntlConceptName("Framework.PartitionXylo.Notes", "Notes")]
        public StaticListNote Notes { get; protected set; }

        public static readonly int minOctave = 5, maxOctave = 7, lastNoteOctave = 8, lastNoteHigh = 0;
    }
}
