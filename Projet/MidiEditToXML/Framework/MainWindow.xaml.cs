using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Framework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public PartitionMidi PartitionMidi { get { return FrameworkController.Instance.PartitionMidi; } }

        public FileManagement FileManagement { get { return FrameworkController.Instance.FileManagement; } }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            List<Channel> channels = new List<Channel>();
            PartitionXylo partitionXylo = new PartitionXylo();
            WindowChannelsSelect window = new WindowChannelsSelect();

            for (int i = 0; i < PartitionMidi.Channels.Count; i++)
                channels.Add(PartitionMidi.Channels[i]);

            if (window.Execute(channels) == true)
            {
                partitionXylo = PartitionMidi.ConvertToPartitionXylo(channels);

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.InitialDirectory = FileManagement.PathSaveFile;
                dlg.Filter = "xml files (*.xml)|*.xml";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    partitionXylo.SaveToFile(dlg.FileName);
            }
        }

        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = FileManagement.DefaultPathLoadFile;
            dlg.Filter = "midi files (*.mid,*.midi)|*.mid;*.midi";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PartitionMidi.Load(dlg.FileName);
            }
        }

        private void MenuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
