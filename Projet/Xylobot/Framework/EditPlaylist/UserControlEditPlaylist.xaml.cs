using Common;
using Concept.Model;
using Concept.Utils;
using Concept.Utils.Wpf;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UserControlEditPlaylist.xaml
    /// </summary>
    public partial class UserControlEditPlaylist : UserControl
    {
        public UserControlEditPlaylist()
        {
            InitializeComponent();

            string[] filePaths = Directory.GetFiles(FrameworkController.Instance.Settings.DefaultPathLoadFile, "*.xml");

            foreach (String s in filePaths)
            {
                string[] tmp = s.Split('\\');
                ListBoxCatalogue.Items.Add(tmp[tmp.Length-1]);
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                CurrentPlaylist = DataContext as Playlist;
            }), null);
        }

        public Playlist CurrentPlaylist { get; set; }
    }
}
