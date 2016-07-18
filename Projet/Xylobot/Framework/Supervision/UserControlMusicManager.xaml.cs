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
    /// Interaction logic for UserControlMusicManager.xaml
    /// </summary>
    public partial class UserControlMusicManager : UserControl
    {
        public UserControlMusicManager()
        {
            InitializeComponent();
        }

        private void ImagePlayPause_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Sequencer)DataContext).PlayPause();
        }

        private void ImageStop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Sequencer)DataContext).Stop();
        }

        private void ImageNext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Sequencer)DataContext).Next();
        }
    }
}
