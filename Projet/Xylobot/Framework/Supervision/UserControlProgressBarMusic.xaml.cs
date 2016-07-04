﻿using System;
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
