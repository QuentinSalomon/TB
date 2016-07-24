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
