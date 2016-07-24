using Concept.Utils.Wpf;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public ImageSource ListBoxImageSource {
            get { return _listBoxImageSource; }
            set
            {
                _listBoxImageSource = value;
                OnPropertyChanged("ListBoxImageSourceProperty");
            }
        }
        private ImageSource _listBoxImageSource;

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxPlaylist.SelectedIndex != -1)
            {
                ((CurrentPlaylistViewModel)DataContext).Playlist.RemovePartitionAt(ListBoxPlaylist.SelectedIndex);
                ListBoxPlaylist.ItemsSource = null;
                ListBoxPlaylist.ItemsSource = ((CurrentPlaylistViewModel)DataContext).Playlist.Partitions;
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            WindowMessageBoxConfirmation w = new WindowMessageBoxConfirmation();
            w.Text = "Voulez-vous vraiment supprimer toute la liste de lecture?";
            w.Width = 400;
            if (w.Execute() == true)
                ((CurrentPlaylistViewModel)DataContext).Playlist.ClearPartitions();
        }
    }
}
