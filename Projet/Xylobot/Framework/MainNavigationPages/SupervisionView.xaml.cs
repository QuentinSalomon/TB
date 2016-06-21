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

            //Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    ListboxErrors.ItemsSource = (DataContext as SupervisionViewModel).Sequencer.Errors;
            //}), null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SupervisionViewModel).Sequencer.Xylobot.SendTempo(2000);
        }
    }
}
