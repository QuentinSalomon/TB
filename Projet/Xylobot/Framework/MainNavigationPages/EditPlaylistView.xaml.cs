using Common;
using Concept.Model;
using Concept.Utils;
using Concept.Utils.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class EditPlaylistView : UserControl
    {
        public EditPlaylistView()
        {
            InitializeComponent();
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
                    (DataContext as EditPlaylistViewModel).Playlist.Partitions.Add(p);
            }
        }

        private void BorderRemove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void BorderRemoveAll_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
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
