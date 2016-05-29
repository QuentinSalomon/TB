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

        private void LoadPartition(string filename)
        {
            CurrentPartition.Clear();
            Sequence sequence = new Sequence(filename);
            string[] tabString = filename.Split('\\');
            string title = tabString[tabString.Length - 1];
            CurrentPartition.Title = title.Split('.')[0];
            int numNote=0;
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
                            tmpNote.Name = CurrentPartition.Title + "_" + (numNote++).ToString();
                            if (CurrentPartition.Channels.Count - msg.MidiChannel <= 0)
                                for (int i = 0; i <= msg.MidiChannel - CurrentPartition.Channels.Count; i++)
                                {
                                    Channel ch = new Channel();
                                    ch.Name = CurrentPartition.Channels.Count.ToString();
                                    CurrentPartition.Channels.Add(ch);
                                }
                            CurrentPartition.Channels[msg.MidiChannel].Notes.Add(tmpNote);
                        }
                    }
                }
            }
        }

        private void SavePartition(string filename)
        {
            //Channel[] chs = new Channel[1];
            //chs[0] = CurrentPartition.Channels[0];
            //PartitionXylo = CurrentPartition.ConvertToPartitionXylo(chs);
            //PartitionXylo.SaveToFile(filename);
        }

        #region event

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadPartition(FrameworkController.Instance.FileManagement.DefaultPathLoadFile +"\\NyanCat.mid");
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SavePartition(FrameworkController.Instance.FileManagement.PathSaveFile +"\\NyanCat.xml");
        }

        #endregion
    }
}
