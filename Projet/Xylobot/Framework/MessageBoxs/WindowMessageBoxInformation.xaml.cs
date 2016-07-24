using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Framework
{
    /// <summary>
    /// Interaction logic for WindowMessageBoxInformation.xaml
    /// </summary>
    public partial class WindowMessageBoxInformation : Window
    {
        public WindowMessageBoxInformation()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return AccessText.Text; }
            set { AccessText.Text = value; }
        }

        public ImageSource ImageSource
        {
            get { return Image.Source; }
            set { Image.Source = value; }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
