using Concept.Model;
using Sanford.Multimedia.Midi;
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

        #region Methods

        public PartitionXylo ConvertToPartitionXylo(List<Channel> channels)
        {
            PartitionXylo partitionXylo = new PartitionXylo();
            List<Note> notes = new List<Note>();
            foreach (Channel ch in channels)
                foreach (Note n in ch.Notes)
                {
                    n.Octave += 1;
                    if ((n.Octave >= 5 && n.Octave <= 7) || (n.Octave == 8 && n.High == 0)) //TODO: Octave 5-7
                        notes.Add(new Note(n));
                }
            notes.Sort(CompareNoteByTick);
            foreach (Note n in notes)
                partitionXylo.Notes.Add(n);
            partitionXylo.Tempo = Tempo;
            partitionXylo.Title = Title;
            return partitionXylo;
        }

        private static int CompareNoteByTick(Note n1, Note n2)
        {
            if (n1.Tick > n2.Tick)
                return 1;
            else if (n1.Tick < n2.Tick)
                return -1;
            else
                return 0;
        }

        public void Load(string filename)
        {
            Clear();
            Sequence sequence = new Sequence(filename);
            string[] tabString = filename.Split('\\');
            string title = tabString[tabString.Length - 1];
            Title = title.Split('.')[0];
            int numNote = 0;
            foreach (Track t in sequence)
            {
                foreach (MidiEvent midiEvent in t.Iterator())
                {
                    if (midiEvent.MidiMessage.MessageType == MessageType.Channel)
                    {
                        int tick = midiEvent.AbsoluteTicks;
                        ChannelMessage msg = ((ChannelMessage)midiEvent.MidiMessage);
                        if (msg.Command == ChannelCommand.NoteOn)
                        {
                            Note tmpNote = NotesConvert.IdToNote(msg.Data1, tick);
                            tmpNote.Name = Title + "_" + (numNote++).ToString();
                            if (Channels.Count - msg.MidiChannel <= 0) {
                                int nbChannel = Channels.Count;
                                for (int i = 0; i <= msg.MidiChannel - nbChannel; i++)
                                {
                                    Channel ch = new Channel();
                                    ch.Name = Channels.Count.ToString();
                                    Channels.Add(ch);
                                }
                            }
                            Channels[msg.MidiChannel].Notes.Add(tmpNote);
                        }
                    }
                }
            }
        }

        #endregion

        [ConceptViewVisible]
        [ConceptAutoCreate]
        [IntlConceptName("Framework.PartitionMidi.Channels", "Channels")]
        public StaticListChannel Channels { get; protected set; }
    }
}
