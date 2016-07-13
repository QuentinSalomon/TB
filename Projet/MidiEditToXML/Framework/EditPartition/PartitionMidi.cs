using Concept.Model;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
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
                    if ((n.Octave >= 5 && n.Octave <= 7) || (n.Octave == 8 && n.High == 0))
                        notes.Add(new Note(n));
                }
            notes.Sort(CompareNoteByTick);

            AdjustingMonoTempo(notes);
            
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

        private void AdjustingMonoTempo(List<Note> notes)
        {
            int[] tmpTempoTicks = new int[Tempos.Count];

            double tempoFactor;
            int i, j = 0;
            //sauvegarde les ticks des tempos
            for (i=0; i<Tempos.Count; i++)
                tmpTempoTicks[i] = Tempos[i].Tick;
            //Avance jusqu'à la note du deuxième tempo
            while (notes[j].Tick < Tempos[1].Tick)
                j++;

            //Corrige le tick des notes en fonction des tempos jusqu'à l'avant dernier tempo
            for (i = 1; i < Tempos.Count - 1; i++)
            {
                tempoFactor = Tempo / Tempos[i].Value;

                while (notes[j].Tick < tmpTempoTicks[i + 1])
                {
                    notes[j].Tick = (int)((notes[j].Tick - tmpTempoTicks[i]) * tempoFactor) + tmpTempoTicks[i];
                    j++;
                }
                int tickOffset = tmpTempoTicks[i + 1];
                tmpTempoTicks[i + 1] = (int)((tmpTempoTicks[i + 1] - tmpTempoTicks[i]) * tempoFactor) + tmpTempoTicks[i];
                tickOffset = tmpTempoTicks[i + 1] - tickOffset;

                for (int k = Tempos.Count-1; k > i; k--)
                    tmpTempoTicks[k] += tickOffset;
                for (int k = notes.Count-1; k >= j; k--)
                    notes[k].Tick += tickOffset;
            }

            //Corrige les dernières notes
            tempoFactor = Tempo / Tempos[i].Value;
            while (j < notes.Count)
            {
                notes[j].Tick = (int)((notes[j].Tick - Tempos[i].Tick) * tempoFactor) + Tempos[i].Tick;
                j++;
            }
        }

        public void Load(string filename)
        {
            Clear();
            Sequence sequence = new Sequence(filename);
            string[] tabString = filename.Split('\\');
            string title = tabString[tabString.Length - 1];
            Title = title.Split('.')[0];
            int numNote = 0, numTempo = 0;
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
                            //Data1 = id de la note.
                            //Data2 = intensité de la note.
                            Note tmpNote = NotesConvert.IdToNote(msg.Data1, msg.Data2, tick);
                            tmpNote.Name = Title + "_" + (numNote++).ToString();
                            if (Channels.Count - msg.MidiChannel <= 0)
                            {
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
                    else if (midiEvent.MidiMessage.MessageType == MessageType.Meta)
                    {
                        int tick = midiEvent.AbsoluteTicks;
                        MetaMessage msg = ((MetaMessage)midiEvent.MidiMessage);
                        if (msg.MetaType == MetaType.Tempo)
                        {
                            int tempo = 0;

                            // If this platform uses little endian byte order.
                            if (BitConverter.IsLittleEndian)
                            {
                                int d = msg.Length - 1;

                                // Pack tempo.
                                for (int i = 0; i < msg.Length; i++)
                                {
                                    tempo |= msg[d] << (8 * i);
                                    d--;
                                }
                            }
                            // Else this platform uses big endian byte order.
                            else
                            {
                                // Pack tempo.
                                for (int i = 0; i < msg.Length; i++)
                                {
                                    tempo |= msg[i] << (8 * i);
                                }
                            }
                            Tempo tmpTempo = new Tempo(tick, tempo);
                            tmpTempo.Name = "Tempo" + (numTempo++).ToString();
                            Tempos.Add(tmpTempo);
                        }
                    }
                }
            }
            Tempo = Tempos[0].Value;
        }

        #endregion

        [ConceptViewVisible]
        [ConceptAutoCreate]
        [IntlConceptName("Framework.PartitionMidi.Channels", "Channels")]
        public StaticListChannel Channels { get; protected set; }

        [ConceptViewVisible]
        [ConceptAutoCreate]
        [IntlConceptName("Framework.PartitionMidi.Tempos", "Tempos")]
        public StaticListTempo Tempos { get; protected set; }
    }
}
