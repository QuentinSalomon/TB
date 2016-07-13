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
                ((CurrentPlaylistViewModel)DataContext).Playlist.RemoveAt(ListBoxPlaylist.SelectedIndex);
        }
    }
}
