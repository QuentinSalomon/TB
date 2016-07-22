using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Framework
{
    /// <summary>
    /// Interaction logic for WindowMessageBoxAutoClosed.xaml
    /// </summary>
    public partial class WindowMessageBoxAutoClosed : Window
    {
        public WindowMessageBoxAutoClosed()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Timer t = new Timer();
            t.Interval = 3000;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.Close();
            }), null);
        }

        public TypeWindow TypeWindow
        {
            get { return _typeWindow; }
            set
            {
                _typeWindow = value;
                switch(value)
                {
                    case TypeWindow.Error:
                        Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xC0, 0x00, 0x00));
                        Title = "Error";
                        Image.Source = new BitmapImage(new Uri(@"/Framework;component/Images/Error32x32.png", UriKind.RelativeOrAbsolute));
                        break;
                    case TypeWindow.Warning:
                        Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xF5, 0x9B, 0x00));
                        Title = "Warning";
                        Image.Source = new BitmapImage(new Uri(@"/Framework;component/Images/Warning32x32.png", UriKind.RelativeOrAbsolute));
                        break;
                    case TypeWindow.Information:
                        Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x70, 0xC0));
                        Title = "Information";
                        Image.Source = new BitmapImage(new Uri(@"/Framework;component/Images/Information32x32.png", UriKind.RelativeOrAbsolute));
                        break;
                    default:
                        break;
                }
            }
        }
        TypeWindow _typeWindow;

        public string Text
        {
            get { return AccessText.Text; }
            set { AccessText.Text = value; }
        }
    }

    public enum TypeWindow { Error, Warning, Information}
}
