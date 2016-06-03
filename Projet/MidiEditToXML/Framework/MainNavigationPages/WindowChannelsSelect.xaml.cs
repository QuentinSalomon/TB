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
using System.Windows.Shapes;

namespace Framework
{
    /// <summary>
    /// Interaction logic for WindowChannelsSelect.xaml
    /// </summary>
    public partial class WindowChannelsSelect : Window
    {
        public WindowChannelsSelect()
        {
            InitializeComponent();
        }

        public bool? Execute(List<Channel> channels)
        {
            for(int i=0; i< channels.Count; i++)
            {
                TextBlock txt = new TextBlock();
                txt.Name = "Label" + i.ToString();
                txt.Text = "Channel " + i.ToString();
                txt.Width = 80;
                txt.HorizontalAlignment = HorizontalAlignment.Center;
                StackPanelLabels.Children.Add(txt);

                CheckBox c = new CheckBox();
                c.Name = "CheckBox" + i.ToString();
                c.DataContext = channels[i];
                c.Width = 80;
                c.HorizontalAlignment = HorizontalAlignment.Center;
                StackPanelCheckBoxs.Children.Add(c);
            }
            this.Width = channels.Count * 80;
            this.ResizeMode = ResizeMode.NoResize;

            ShowDialog();

            if (DialogResult == true)
            {
                channels.Clear();
                foreach (object o in StackPanelCheckBoxs.Children)
                    if (o is CheckBox)
                        if ((o as CheckBox).IsChecked == true)
                            channels.Add((o as CheckBox).DataContext as Channel);
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
    }
}
