﻿using System.Windows;
using System.Windows.Controls;

namespace Framework
{
    /// <summary>
    /// Interaction logic for WindowConfirmation.xaml
    /// </summary>
    public partial class WindowMessageBoxConfirmation : Window
    {
        public WindowMessageBoxConfirmation()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return AccessText.Text; }
            set { AccessText.Text = value; }
        }

        public new double Width
        {
            get { return StackPanel1.ActualWidth; }
            set
            {
                StackPanel1.Width = value;
                TextBlock.Width = value - 52;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public bool? Execute()
        {
            ShowDialog();
            return DialogResult;
        }
    }
}
