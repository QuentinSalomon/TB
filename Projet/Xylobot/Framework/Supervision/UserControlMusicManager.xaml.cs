using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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

        private void ButtonPlayPause_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((Sequencer)DataContext).PlayPause();
        }

        private void ButtonNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((Sequencer)DataContext).Next();
        }

        private void ButtonStop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((Sequencer)DataContext).Stop();
        }
    }

    public class IsPlayingToImageConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            if((bool)value)
                return new BitmapImage(new Uri(@"/Framework;component/Images/Pause32x32.png", UriKind.RelativeOrAbsolute));
            else
                return new BitmapImage(new Uri(@"/Framework;component/Images/Play32x32.png", UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
