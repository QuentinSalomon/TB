using System.Windows;
using System.Windows.Controls;

namespace Framework
{
    /// <summary>
    /// Interaction logic for UserControlProgressBarMusic.xaml
    /// </summary>
    public partial class UserControlProgressBarMusic : UserControl
    {
        public UserControlProgressBarMusic()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RectangleProgress.Width = 0;
            EllipseProgress.Margin = new Thickness(0, 0, 0, 0);
        }

        public double Progression {
            get { return Progression; }
            set {
                RectangleProgress.Width = value;
                EllipseProgress.Margin = new Thickness(value-5, 0, 0, 0);
                Progression = value;
            } }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                double progress = (double)(DataContext as double?) * this.ActualWidth;
                RectangleProgress.Width = progress;
                EllipseProgress.Margin = new Thickness(progress - 5, 0, 0, 0);
            }
        }
    }
}
