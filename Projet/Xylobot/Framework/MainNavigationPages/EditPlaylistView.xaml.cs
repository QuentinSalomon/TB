using Concept.Model;
using Concept.Utils;
using Concept.Utils.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                        GridButtons.Opacity = 0.5;
                        ButtonBackToPlaylists.IsEnabled = false;
                        ButtonLoadToPlayPartition.IsEnabled = false;
                        TextBlockTitle.Text = "Playlists";
                    }
                    else
                    {
                        ListBoxImageSource = new BitmapImage(new Uri(@"/Framework;component/Images/Note32x32.png", UriKind.RelativeOrAbsolute));
                        GridButtons.Opacity = 1;
                        ButtonBackToPlaylists.IsEnabled = true;
                        ButtonLoadToPlayPartition.IsEnabled = true;
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
                {
                    WindowMessageBoxAutoClosed w = new WindowMessageBoxAutoClosed();
                    w.TypeWindow = TypeWindow.Information;

                    if (((EditPlaylistViewModel)DataContext).Playlist.AddPartition(p))
                        w.Text = "Partition ajoutée à la liste de lecture";
                    else
                        w.Text = "Partition déjà présente dans la liste de lecture";

                    w.Show();
                }
            }
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
                string path = FrameworkController.Instance.Settings.DefaultPathLoadFile + "\\";

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
