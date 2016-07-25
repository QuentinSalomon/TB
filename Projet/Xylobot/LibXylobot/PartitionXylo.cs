using Concept.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [IntlConceptName("Common.PartitionXylo.Name", "PartitionXylo")]
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
        [IntlConceptName("Common.PartitionXylo.Title", "Title")]
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
        [IntlConceptName("Common.PartitionXylo.Tempo", "Tempo")]
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

        [ConceptAutoCreate]
        [IntlConceptName("Common.PartitionXylo.Notes", "Notes")]
        public StaticListNote Notes { get; protected set; }

        #endregion
    }

    [ConceptSmallImage(typeof(StaticListPartitionXylo), "../../../Images/Partition32x32.png")]
    [ConceptLargeImage(typeof(StaticListPartitionXylo), "../../../Images/Partition64x64.png")]
    public class StaticListPartitionXylo : ConceptStaticList<PartitionXylo>
    { }
}
