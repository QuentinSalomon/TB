using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sanford.Multimedia.Midi;

namespace Framework
{
    /// <summary>
    /// Interaction logic for UserControlEditPartition.xaml
    /// </summary>
    public partial class UserControlEditPartition : UserControl
    {
        public UserControlEditPartition()
        {
            InitializeComponent();

            for (int j = 7; j >= 5; j--)
            {
                for (int i = NotesConvert.tabNote.Length - 1; i >= 0; i--)
                {
                    Label label = new Label();
                    label.Name = "Label" + i.ToString();
                    label.Content = NotesConvert.tabNote[i] + '\t' + j.ToString();
                    StackPanelNotesPitch.Children.Add(label);
                }
            }

            //Récupération de la partition (sans le beginInvoke ça ne fonctionne pas)
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CurrentPartition = DataContext as PartitionMidi;
            }), null);

            PartitionXylo = new PartitionXylo();
        }

        public PartitionMidi CurrentPartition { get; set; }

        public PartitionXylo PartitionXylo { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadPartition();
            SavePartition();
        }

        private void LoadPartition()
        {
            PartitionXylo.Clear();
            int i = 0;
            Sequence sequence = new Sequence("NyanCat.mid");
            foreach (Track t in sequence)
            {
                foreach (MidiEvent midiEvent in t.Iterator())
                {
                    if (midiEvent.MidiMessage.MessageType == MessageType.Channel)
                    {
                        int tick = midiEvent.AbsoluteTicks;
                        ChannelMessage msg = ((ChannelMessage)midiEvent.MidiMessage);
                        if (msg.Command == ChannelCommand.NoteOn)
                            if (msg.MidiChannel == 0)
                            {
                                Note tmpNote = NotesConvert.IdToNote(msg.Data1, tick);
                                if (tmpNote.Octave >= 4 && tmpNote.Octave <= 6)
                                {
                                    tmpNote.Name = tmpNote.Name + "NyanCat" + (i++).ToString();
                                    tmpNote.Octave += 1;
                                    PartitionXylo.Notes.Add(tmpNote);
                                }
                            }
                        //else if (msg.Command == ChannelCommand.NoteOff)
                        //{
                        //    PartitionXylo.Add(NotesConvert.IdToNote(msg.Data1, tick));
                        //}
                    }
                }
            }
        }
        private void SavePartition()
        {
            PartitionXylo.SaveToFile("test.xml");
        }
    }
}
