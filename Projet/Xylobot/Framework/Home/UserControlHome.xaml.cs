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
    }
}
