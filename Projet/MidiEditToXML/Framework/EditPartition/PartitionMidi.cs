﻿using Concept.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [ConceptView(typeof(UserControlEditPartition))]
    [IntlConceptName("Framework.PartitionMidi.Name", "PartitionMidi")]
    [ConceptSmallImage(typeof(PartitionMidi), "/Images/Partition32x32.png")]
    [ConceptLargeImage(typeof(PartitionMidi), "/Images/Partition64x64.png")]
    public class PartitionMidi : ConceptComponent
    {
        #region Constructeur

        public PartitionMidi()
        {

        }

        #endregion

        #region Propriétés

        [ConceptSerialized]
        [ConceptViewVisible]
        [IntlConceptName("Framework.PartitionMidi.Title", "Title")]
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
        [IntlConceptName("Framework.PartitionMidi.Tempo", "Tempo")]
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

        public PartitionXylo ConvertToPartitionXylo(Channel[] channels)
        {
            PartitionXylo partitionXylo = new PartitionXylo();
            foreach (Channel ch in channels)
                foreach(Note n in ch.Notes)
                    if (n.Octave >= 4 && n.Octave <= 6) //TODO: Octave 5-7
                        partitionXylo.Notes.Add(new Note(n));
            partitionXylo.Tempo = Tempo;
            partitionXylo.Title = Title;
            return partitionXylo;
        }

        [ConceptViewVisible]
        [ConceptAutoCreate]
        [IntlConceptName("Framework.PartitionMidi.Channels", "Channels")]
        public StaticListChannel Channels { get; protected set; }
    }
}
