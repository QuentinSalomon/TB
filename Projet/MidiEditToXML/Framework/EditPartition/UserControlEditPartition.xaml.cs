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
using System.Windows.Forms;

namespace Framework
{
    /// <summary>
    /// Interaction logic for UserControlEditPartition.xaml
    /// </summary>
    public partial class UserControlEditPartition : System.Windows.Controls.UserControl
    {
        public UserControlEditPartition()
        {
            InitializeComponent();

            for (int j = 7; j >= 5; j--)
            {
                for (int i = NotesConvert.tabNote.Length - 1; i >= 0; i--)
                {
                    System.Windows.Controls.Label label = new System.Windows.Controls.Label();
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

        }

        #region event

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = FrameworkController.Instance.FileManagement.DefaultPathLoadFile;
            dlg.Filter = "midi files (*.mid,*.midi)|*.mid;*.midi";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CurrentPartition.Load(dlg.FileName);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            List<Channel> channels = new List<Channel>();
            PartitionXylo partitionXylo = new PartitionXylo();
            WindowChannelsSelect window = new WindowChannelsSelect();

            for (int i = 0; i < CurrentPartition.Channels.Count; i++)
                channels.Add(CurrentPartition.Channels[i]);

            if (window.Execute(channels) == true)
            {
                partitionXylo = CurrentPartition.ConvertToPartitionXylo(channels);

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.InitialDirectory = FrameworkController.Instance.FileManagement.PathSaveFile;
                dlg.Filter = "xml files (*.xml)|*.xml";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    partitionXylo.SaveToFile(dlg.FileName);
            }
        }

        #endregion
    }
}
