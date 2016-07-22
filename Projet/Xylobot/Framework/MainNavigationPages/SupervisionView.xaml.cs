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
            Sequencer sequencer = (DataContext as SupervisionViewModel).Sequencer;
            if (sequencer.SpeedPlay > 0.51)
            {
                sequencer.SpeedPlay = sequencer.SpeedPlay - 0.1;
                TextBlockSpeed.Text = sequencer.SpeedPlay.ToString();
            }
        }

        private void ButtonMoreSpeed_Click(object sender, RoutedEventArgs e)
        {
            Sequencer sequencer = (DataContext as SupervisionViewModel).Sequencer;
            if (sequencer.SpeedPlay < 2)
            {
                sequencer.SpeedPlay = sequencer.SpeedPlay + 0.1;
                TextBlockSpeed.Text = sequencer.SpeedPlay.ToString();
            }
        }

    }
}
