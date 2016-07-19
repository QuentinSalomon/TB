using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using System.Windows.Shapes;

namespace Framework
{
    /// <summary>
    /// Interaction logic for WindowSelectUsbPort.xaml
    /// </summary>
    public partial class WindowSelectUsbPort : Window
    {
        public WindowSelectUsbPort()
        {
            InitializeComponent();
        }

        public bool? Execute(ref string portName, string defaultPortName)
        {
            foreach (string s in SerialPort.GetPortNames())
                ListBoxPortName.Items.Add(s);

            ShowDialog();

            if (ListBoxPortName.Items.Count > 0)
            {
                if(ListBoxPortName.SelectedItem != null)
                    portName = ListBoxPortName.SelectedItem as string;
                if (portName == "" || !(portName.ToLower()).StartsWith("com"))
                    portName = defaultPortName;
            }
            else
            {
                portName = defaultPortName;
            }

            return DialogResult;
        }


        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ListBoxPortName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonOk.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ListBoxPortName.Items.Count == 1)
            {
                ListBoxPortName.SelectedIndex = 0;
                ButtonOk_Click(null, null);
            }
            else if (ListBoxPortName.Items.Count == 0)
            {
                ButtonCancel_Click(null, null);
            }
        }
    }
}
