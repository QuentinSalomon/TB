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
using Sanford.Multimedia.Midi;
using System.Windows.Forms;

namespace Framework
{
    /// <summary>
    /// Interaction logic for UserControlEditPartition.xaml
    /// </summary>
    public partial class UserControlEditPartition : System.Windows.Controls.UserControl
    {
        public UserControlEditPartition()
        {
            InitializeComponent();

            //for (int j = 7; j >= 5; j--)
            //{
            //    for (int i = NotesConvert.tabNote.Length - 1; i >= 0; i--)
            //    {
            //        System.Windows.Controls.Label label = new System.Windows.Controls.Label();
            //        label.Name = "Label" + i.ToString();
            //        label.Content = NotesConvert.tabNote[i] + '\t' + j.ToString();
            //        StackPanelNotesPitch.Children.Add(label);
            //    }
            //}

            //Récupération de la partition (sans le beginInvoke ça ne fonctionne pas)
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CurrentPartition = DataContext as PartitionMidi;
            }), null);
        }

        public PartitionMidi CurrentPartition { get; set; }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            
        }
    }
}
