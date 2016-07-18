using Common;
using Concept.Model;
using Concept.Utils;
using Concept.Utils.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
    /// Interaction logic for EditPlaylistView.xaml
    /// </summary>
    public partial class EditPlaylistView : UserControl, INotifyPropertyChanged
    {
        #region Property Change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        #region Properties

        public ImageSource ListBoxImageSource
        {
            get { return _listBoxImageSource; }
            set
            {
                _listBoxImageSource = value;
                OnPropertyChanged("ListBoxImageSourceProperty");
            }
        }
        private ImageSource _listBoxImageSource;

        private Playlist CurrentPlaylist { get; set; }

        ObservableCollection<ListBoxItemPlaylist> ListPlaylist { get; set; }

        bool? ShowPlaylists
        {
            get { return _showPlaylists; }
            set
            {
                if (value != null)
                {
                    _showPlaylists = value;
                    if (value == true)
                    {
                        ListBoxImageSource = new BitmapImage(new Uri(@"/Framework;component/Images/Playlist32x32.png", UriKind.RelativeOrAbsolute));
                        ButtonBackToPlaylists.Visibility = Visibility.Collapsed;
                        //GridPlaylists.Visibility = Visibility.Visible;
                        //GridButtons.Visibility = Visibility.Collapsed;
                        TextBlockTitle.Text = "Playlists";
                    }
                    else
                    {
                        ListBoxImageSource = new BitmapImage(new Uri(@"/Framework;component/Images/Note32x32.png", UriKind.RelativeOrAbsolute));
                        ButtonBackToPlaylists.Visibility = Visibility.Visible;
                        //GridButtons.Visibility = Visibility.Visible;
                        //GridPlaylists.Visibility = Visibility.Collapsed;
                        TextBlockTitle.Text = (ListBoxPlaylist.SelectedItem as ListBoxItemPlaylist).Title;
                    }
                    ActualizeListBox();
                }
            }
        }
        private bool? _showPlaylists;

        #endregion

        public EditPlaylistView()
        {
            InitializeComponent();
            ListPlaylist = new ObservableCollection<ListBoxItemPlaylist>();
            CurrentPlaylist = new Playlist();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ListBoxPlaylist.ItemsSource = ListPlaylist;
            ActualizeListBox();
            ShowPlaylists = true;
        }

        #region Click Methods

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
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
                    CurrentPlaylist.Partitions.Add(p);
            }
        }

        private void ButtonRemovePartition_Click(object sender, RoutedEventArgs e)
        {
            CurrentPlaylist.Partitions.Remove(ListBoxPlaylist.SelectedItem as PartitionXylo);
        }

        private void ButtonLoadToPlayPartition_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxPlaylist.SelectedIndex != -1)
            {
                PartitionXylo p = new PartitionXylo();
                var messages = new MessageCollection();
                ListBoxItemPlaylist item = ((ListBoxItemPlaylist)ListBoxPlaylist.SelectedItem);

                p.LoadFromFile(item.Path + item.Title, PluginClassManager.AllFactories, messages);
                p.Name = item.Title.Split('.')[0];
                if (messages.Count > 0)
                    ConceptMessage.ShowError(string.Format("Error while loading the configuration file:\n{0}", messages.Text), "Loading Error");
                else
                    ((EditPlaylistViewModel)DataContext).Playlist.AddPartition(p);
            }
        }

        private void ButtonRemovePlaylist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAddToPlayPlaylist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonEditPlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxPlaylist.SelectedIndex != -1)
                ShowPlaylists = false;
        }

        private void ButtonBackToPlaylists_Click(object sender, RoutedEventArgs e)
        {
            ShowPlaylists = true;
        }

        #endregion

        private void ActualizeListBox()
        {
            if (ShowPlaylists == true)
            {
                string path = FrameworkController.Instance.Settings.DefaultPathLoadFile;

                ListPlaylist.Clear();
                foreach (string s in Directory.GetDirectories(path))
                    ListPlaylist.Add(new ListBoxItemPlaylist(s.Remove(0, path.Length), path));
            }
            else if (ShowPlaylists == false)
            {
                ListBoxItemPlaylist playlistInfos = ListBoxPlaylist.SelectedItem as ListBoxItemPlaylist;
                string path = playlistInfos.Path + playlistInfos.Title + "\\";

                ListPlaylist.Clear();
                foreach (string file in Directory.GetFiles(path, "*.xml"))
                    ListPlaylist.Add(new ListBoxItemPlaylist(file.Remove(0, path.Length), path));
            }
        }

        private void ListBoxPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxPlaylist.SelectedIndex != -1 && ShowPlaylists == true)
                ShowPlaylists = false;
        }
    }

    public class ListBoxItemPlaylist
    {
        public ListBoxItemPlaylist(string title, string path)
        {
            Title = title;
            Path = path;
        }

        public string Path { get; set; }

        public string Title { get; set; }
    }

    public class ListBoxItemPartition
    {
        public ListBoxItemPartition(string title, string path)
        {
            Title = title;
            Path = path;
        }

        public string Path { get; set; }

        public string Title { get; set; }
    }

    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            ListBoxItem item = (ListBoxItem)value;
            ListBox ListBox = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;
            int index = ListBox.ItemContainerGenerator.IndexFromContainer(item);
            return index.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
