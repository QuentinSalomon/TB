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
    /// Interaction logic for UserControlSequencer.xaml
    /// </summary>
    public partial class UserControlSequencer : UserControl
    {
        public UserControlSequencer()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(() =>
            {
                ListboxErrors.ItemsSource = (DataContext as Sequencer).Errors;
            }), null);
        }
    }
}
