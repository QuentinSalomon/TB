using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Framework
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void UserControlHomeHeig_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowMessageBoxInformation w = new WindowMessageBoxInformation();
            w.Text = "Développez à la Heig de Cheseaux-noréaz.";
            w.Background = UserControlHomeHeig.Background;
            w.ImageSource = new BitmapImage(new Uri(@"/Framework;component/Images/LogoHeig32x32.png", UriKind.RelativeOrAbsolute));
            w.ShowDialog();
        }
        private void UserControlHomeConceptHmi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowMessageBoxInformation w = new WindowMessageBoxInformation();
            w.Text = "Concept Hmi est un logiciel développé par Objectis visant à simplifier le développement d'application.";
            w.Background = UserControlHomeConcept.Background;
            w.ImageSource = new BitmapImage(new Uri(@"/Framework;component/Images/Concept32x32.png", UriKind.RelativeOrAbsolute));
            w.ShowDialog();
        }
        private void UserControlHomeWebPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowMessageBoxInformation w = new WindowMessageBoxInformation();
            w.Text = "Scannez le Qr code pour accéder à la page web.";
            w.Background = UserControlHomeWebPage.Background;
            w.ImageSource = new BitmapImage(new Uri(@"/Framework;component/Images/QrCode32x32.png", UriKind.RelativeOrAbsolute));
            w.ShowDialog();
        }
        private void UserControlHomeWoopsa_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowMessageBoxInformation w = new WindowMessageBoxInformation();
            w.Text = "Woopsa est un protocol de communication web orienté objet";
            w.Background = UserControlHomeWoopsa.Background;
            w.ImageSource = new BitmapImage(new Uri(@"/Framework;component/Images/Woopsa32x32.png", UriKind.RelativeOrAbsolute));
            w.ShowDialog();
        }
        private void UserControlHomeSmartphone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowMessageBoxInformation w = new WindowMessageBoxInformation();
            w.Text = "Connectez votre smartphone pour interagir avec le Virtuoso.";
            w.Background = UserControlHomeSmartphone.Background;
            w.ImageSource = new BitmapImage(new Uri(@"/Framework;component/Images/Smartphone32x32.png", UriKind.RelativeOrAbsolute));
            w.ShowDialog();
        }
        private void UserControlHomeWifi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowMessageBoxInformation w = new WindowMessageBoxInformation();
            w.Text = "Connectez-vous au wifi et accédez à la page web.";
            w.Background = UserControlHomeWifi.Background;
            w.ImageSource = new BitmapImage(new Uri(@"/Framework;component/Images/Wifi32x32.png", UriKind.RelativeOrAbsolute));
            w.ShowDialog();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UserControlHomeHeig.ImageWidth = 256;
        }
    }
}
