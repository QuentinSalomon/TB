using System.Windows;
using System.Windows.Controls;

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
