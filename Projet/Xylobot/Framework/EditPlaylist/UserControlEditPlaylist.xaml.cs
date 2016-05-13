using Common;
using Concept.Model;
using Concept.Utils;
using Concept.Utils.Wpf;
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

namespace Framework
{
    /// <summary>
    /// Interaction logic for UserControlEditPlaylist.xaml
    /// </summary>
    public partial class UserControlEditPlaylist : UserControl
    {
        public UserControlEditPlaylist()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(() =>
            {
                CurrentPlaylist = DataContext as Playlist;
            }), null);
        }

        public Playlist CurrentPlaylist { get; set; }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            var messages = new MessageCollection();
            PartitionXylo partition = new PartitionXylo();
            partition.LoadFromFile("test.xml", PluginClassManager.AllFactories, messages);
            if (messages.Count > 0)
                ConceptMessage.ShowError(string.Format("Error while loading the configuration file:\n{0}", messages.Text), "Loading Error");
            else
                CurrentPlaylist.Partitions.Add(partition);
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
