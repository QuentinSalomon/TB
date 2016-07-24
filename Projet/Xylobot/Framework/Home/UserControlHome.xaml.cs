using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Framework
{
    /// <summary>
    /// Interaction logic for UserControlHome.xaml
    /// </summary>
    public partial class UserControlHome : UserControl
    {
        public UserControlHome()
        {
            InitializeComponent();
        }

        public string Source
        {
            get { return Image.Source.ToString(); }
            set { Image.Source = new BitmapImage(new Uri(@value, UriKind.RelativeOrAbsolute)); }
        }

        public Brush Color
        {
            get { return Grid.Background; }
            set { Grid.Background = value; }
        }

        public string Text
        {
            get { return TextBlock.Text; }
            set { TextBlock.Text = value; }
        }

        public double ImageWidth
        {
            get { return Image.ActualWidth; }
            set { Image.Width = value; }
        }
    }
}
