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
    /// Interaction logic for UserControlThreeButtons.xaml
    /// </summary>
    public partial class UserControlThreeButtons : UserControl
    {
        public UserControlThreeButtons()
        {
            InitializeComponent();
        }

        public string Text1
        {
            get { return TextBlock1.Text; }
            set { TextBlock1.Text = value; }
        }

        public string Text2
        {
            get { return TextBlock2.Text; }
            set { TextBlock2.Text = value; }
        }

        public string Text3
        {
            get { return TextBlock3.Text; }
            set { TextBlock3.Text = value; }
        }

        public Brush Background1
        {
            get { return Border1.Background; }
            set { Border1.Background = value; }
        }

        public Brush Background2
        {
            get { return Border2.Background; }
            set { Border2.Background = value; }
        }

        public Brush Background3
        {
            get { return Border3.Background; }
            set { Border3.Background = value; }
        }

        public ImageSource Source1
        {
            get { return Image1.Source; }
            set { Image1.Source = value; }
        }

        public ImageSource Source2
        {
            get { return Image2.Source; }
            set { Image2.Source = value; }
        }

        public ImageSource Source3
        {
            get { return Image3.Source; }
            set { Image3.Source = value; }
        }

        public ImageSource SourceGeneral
        {
            get { return ImageGeneral1.Source; }
            set {
                ImageGeneral1.Source = value;
                ImageGeneral2.Source = value;
                ImageGeneral3.Source = value;
            }
        }

    }
}
