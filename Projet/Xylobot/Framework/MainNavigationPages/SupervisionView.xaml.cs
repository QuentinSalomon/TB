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
    /// Interaction logic for SupervisionView.xaml
    /// </summary>
    public partial class SupervisionView : UserControl
    {
        public SupervisionView()
        {
            InitializeComponent();
        }

        private void ButtonLessSpeed_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SupervisionViewModel).Sequencer.Xylobot.SendTempo(2000);
        }

        private void ButtonMoreSpeed_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SupervisionViewModel).Sequencer.Xylobot.SendTempo(4000);
        }

        private void ImageNext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void ImagePlayPause_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as SupervisionViewModel).Sequencer.PlayPause();
        }

        private void ImageStop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as SupervisionViewModel).Sequencer.Stop();
        }
    }
}
