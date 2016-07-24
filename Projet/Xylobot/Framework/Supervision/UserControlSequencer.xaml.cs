using System;
using System.Windows.Controls;

namespace Framework
{
    /// <summary>
    /// Interaction logic for UserControlSequencer.xaml
    /// </summary>
    public partial class UserControlSequencer : UserControl
    {
        public UserControlSequencer()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(() =>
            {
                //ListboxErrors.ItemsSource = (DataContext as Sequencer).Errors;
            }), null);
        }
    }
}
