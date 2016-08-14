using Concept.Model;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;

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

        #endregion

        #region Methods

        public PartitionXylo ConvertToPartitionXylo(List<Channel> channels)
        {
            PartitionXylo partitionXylo = new PartitionXylo();
            List<Note> notes = new List<Note>();
            foreach (Channel ch in channels)
                foreach (Note n in ch.Notes)
                {   //TODO : Vérifier condition if
                    //if ((n.Octave >= Xylobot.startOctaveXylophone && n.Octave <= Xylobot.lastOctaveXylophone)
                    //    || (n.Octave == PartitionXylo.lastNoteOctave && n.High == PartitionXylo.lastNoteHigh))
                    if (n.Octave*Xylobot.octaveSize + n.High >= Xylobot.startOctaveXylophone*Xylobot.octaveSize &&
                        n.Octave * Xylobot.octaveSize + n.High <= Xylobot.startOctaveXylophone * Xylobot.octaveSize + Xylobot.numberKeysXylophone)
                        notes.Add(new Note(n));
                }
            notes.Sort(CompareNoteByTick);

            AdjustingMonoTempo(notes);

            foreach (Note n in notes)
                partitionXylo.Notes.Add(n);
            partitionXylo.Tempo = 1;// Tempo;
            partitionXylo.Title = Title;
            partitionXylo.Name = Title;
            return partitionXylo;
        }

        public PartitionXylo ConvertCompleteToPartitionXylo()
        {
            PartitionXylo partitionXylo = new PartitionXylo();
            List<Note> notes = new List<Note>();
            foreach (Channel ch in Channels)
                foreach (Note n in ch.Notes)
                {   //TODO : Vérifier condition if
                    //if ((n.Octave >= Xylobot.startOctaveXylophone && n.Octave <= Xylobot.lastOctaveXylophone)
                    //    || (n.Octave == PartitionXylo.lastNoteOctave && n.High == PartitionXylo.lastNoteHigh))
                    if (n.Octave * Xylobot.octaveSize + n.High >= Xylobot.startOctaveXylophone * Xylobot.octaveSize &&
                        n.Octave * Xylobot.octaveSize + n.High <= Xylobot.startOctaveXylophone * Xylobot.octaveSize + Xylobot.numberKeysXylophone)
                        notes.Add(new Note(n));
                }
            notes.Sort(CompareNoteByTick);

            AdjustingMonoTempo(notes);

            foreach (Note n in notes)
                partitionXylo.Notes.Add(n);
            partitionXylo.Tempo = 1;// Tempo;
            partitionXylo.Title = Title;
            partitionXylo.Name = Title;
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

        //private void AdjustingMonoTempo(List<Note> notes)
        //{
        //    if (Tempos.Count > 1)
        //    {
        //        int[] tmpTempoTicks = new int[Tempos.Count];

        //        double tempoFactor;
        //        int i, j = 0;
        //        //sauvegarde les ticks des tempos
        //        for (i = 0; i < Tempos.Count; i++)
        //            tmpTempoTicks[i] = Tempos[i].Tick;
        //        //Avance jusqu'à la note du deuxième tempo
        //        while (notes[j].Tick < Tempos[1].Tick)
        //            j++;

        //        //Corrige le tick des notes en fonction des tempos jusqu'à l'avant dernier tempo
        //        for (i = 1; i < Tempos.Count - 1; i++)
        //        {
        //            tempoFactor = Tempo / Tempos[i].Value;

        //            while (notes[j].Tick < tmpTempoTicks[i + 1])
        //            {
        //                notes[j].Tick = (int)((notes[j].Tick - tmpTempoTicks[i]) * tempoFactor) + tmpTempoTicks[i];
        //                if (j < notes.Count - 1)
        //                    j++;
        //                else
        //                    break;
        //            }
        //            int tickOffset = tmpTempoTicks[i + 1];
        //            tmpTempoTicks[i + 1] = (int)((tmpTempoTicks[i + 1] - tmpTempoTicks[i]) * tempoFactor) + tmpTempoTicks[i];
        //            tickOffset = tmpTempoTicks[i + 1] - tickOffset;

        //            for (int k = Tempos.Count - 1; k > i; k--)
        //                tmpTempoTicks[k] += tickOffset;
        //            for (int k = notes.Count - 1; k >= j; k--)
        //                notes[k].Tick += tickOffset;
        //        }

        //        //Corrige les dernières notes
        //        tempoFactor = Tempo / Tempos[i].Value;
        //        while (j < notes.Count)
        //        {
        //            notes[j].Tick = (int)((notes[j].Tick - Tempos[i].Tick) * tempoFactor) + Tempos[i].Tick;
        //            j++;
        //        }
        //    }
        //}

        private void AdjustingMonoTempo(List<Note> notes)
        {
            double correctTick;
            int k = notes.Count-1;
            for (int i = Tempos.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                    correctTick = 0;
                else {
                    correctTick = Tempos[0].Tick / Tempos[0].Value;
                    for (int j = 1; j < i; j++)
                        correctTick += (Tempos[j].Tick - Tempos[j - 1].Tick) / Tempos[j].Value;
                }

                //Correction des notes par tempo
                while (notes[k].Tick >= Tempos[i].Tick)
                {
                    notes[k].Tick = (int)
                        ((notes[k].Tick - correctTick) / Tempos[i].Value + correctTick);
                    if (--k < 0)
                        break;
                }
            }
        }

        public void Load(string filename)
        {
            Clear();
            Sequence sequence = new Sequence(filename);
            string[] tabString = filename.Split('\\');
            string[] titleSplit = tabString[tabString.Length - 1].Split('.');
            Title = "";
            Title += titleSplit[0];
            for (int i = 1; i < titleSplit.Length - 1; i++)
                Title += '.' + titleSplit[i];
            foreach (Track t in sequence)
            {
                foreach (MidiEvent midiEvent in t.Iterator())
                {
                    int tick = midiEvent.AbsoluteTicks;
                    if (midiEvent.MidiMessage.MessageType == MessageType.Channel)
                    {
                        ChannelMessage msg = ((ChannelMessage)midiEvent.MidiMessage);
                        if (msg.Command == ChannelCommand.NoteOn)
                            GetMidiNoteOn(msg, tick);
                    }
                    else if (midiEvent.MidiMessage.MessageType == MessageType.Meta)
                    {
                        MetaMessage msg = ((MetaMessage)midiEvent.MidiMessage);
                        if (msg.MetaType == MetaType.Tempo)
                            GetMidiTempo(msg, tick, sequence);
                    }
                }
            }
        }

        private void GetMidiTempo(MetaMessage msg, int tick, Sequence sequence)
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
            Tempo tmpTempo = new Tempo(tick, (double)tempo / sequence.Division / 1000);
            tmpTempo.Name = "Tempo" + (Tempos.Count).ToString();
            Tempos.Add(tmpTempo);
        }

        private void GetMidiNoteOn(ChannelMessage msg, int tick)
        {
            //Data1 = id de la note.
            //Data2 = intensité de la note.
            Note tmpNote = NotesConvert.IdToNote(msg.Data1, msg.Data2, tick);
            //ajout des canaux innexistants
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
            tmpNote.Name = Title + "_" + msg.MidiChannel.ToString() + "_" +
                (Channels[msg.MidiChannel].Notes.Count).ToString();
            Channels[msg.MidiChannel].Notes.Add(tmpNote);
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
