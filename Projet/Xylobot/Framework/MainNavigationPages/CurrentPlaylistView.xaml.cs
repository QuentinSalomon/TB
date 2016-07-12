using Common;
using Concept.Model;
using Concept.Utils;
using Concept.Utils.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for CurrentPlaylistView.xaml
    /// </summary>
    public partial class CurrentPlaylistView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public CurrentPlaylistView()
        {
            InitializeComponent();
            ListBoxImageSource = new BitmapImage(new Uri(@"/Framework;component/Images/Note32x32.png", UriKind.RelativeOrAbsolute));
        }

        private void BorderAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PartitionXylo p = new PartitionXylo();
            var messages = new MessageCollection();
            OpenFileDialog fileDlg = new OpenFileDialog();

            fileDlg.InitialDirectory = FrameworkController.Instance.Settings.DefaultPathLoadFile;
            fileDlg.Filter = "txt files (*.xml)|*.xml";
            fileDlg.RestoreDirectory = true;
            if (fileDlg.ShowDialog() == true)
            {
                p.LoadFromFile(fileDlg.FileName, PluginClassManager.AllFactories, messages);
                p.Name = fileDlg.FileName;
                if (messages.Count > 0)
                    ConceptMessage.ShowError(string.Format("Error while loading the configuration file:\n{0}", messages.Text), "Loading Error");
                else
                    (DataContext as CurrentPlaylistViewModel).Playlist.Partitions.Add(p);
            }
        }

        private void BorderRemove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxPlaylist.SelectedIndex != -1)
                ((CurrentPlaylistViewModel)DataContext).Playlist.Remove(ListBoxPlaylist.SelectedItem as PartitionXylo);
        }

        private void BorderClear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((CurrentPlaylistViewModel)DataContext).Playlist.Clear();
        }

        public ImageSource ListBoxImageSource {
            get { return _listBoxImageSource; }
            set
            {
                _listBoxImageSource = value;
                OnPropertyChanged("ListBoxImageSourceProperty");
            }
        }
        private ImageSource _listBoxImageSource;
    }
}
