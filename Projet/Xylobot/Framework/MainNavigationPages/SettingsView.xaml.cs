using Common;
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
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }
        
        public int CurrentIdNote {
            get { return _currentIdNote; }
            set
            {                
                _currentIdNote = value;
                Note n;
                n = IdToNoteTest(CurrentIdNote);
                TextBlockKeyTitle.Text = "Note : " + n.HighString + "   \tOctave : " + n.Octave;
                (DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime = Truncate((DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime);
                TextBlockHitTime.Text = (DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime.ToString();
            }
        }
        private int _currentIdNote;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentIdNote = 0;
        }

        private void ButtonLessTime_Click(object sender, RoutedEventArgs e)
        {
            Note n;
            (DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime = Truncate((DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime - 0.1);
            TextBlockHitTime.Text = (DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime.ToString();
            n = IdToNoteTest(CurrentIdNote);
            (DataContext as SettingsViewModel).Sequencer.ChangeKeyHitTime(n,
                (DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime);
        }

        private void ButtonMoreTime_Click(object sender, RoutedEventArgs e)
        {
            Note n;
            (DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime = Truncate((DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime + 0.1);
            TextBlockHitTime.Text = (DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime.ToString();
            n = IdToNoteTest(CurrentIdNote);
            (DataContext as SettingsViewModel).Sequencer.ChangeKeyHitTime(n,
                (DataContext as SettingsViewModel).Settings.Keys[CurrentIdNote].HitTime);
        }


        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentIdNote > 0)
            {
                CurrentIdNote--;
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentIdNote < Xylobot.numberKeysXylophone - 1)
            {
                CurrentIdNote++;
            }
        }

        private double Truncate(double value)
        {
            return (double)Math.Truncate((decimal)(100 * value)) / 100;
        }

        private static readonly string[] tabNote = new string[]
        { "DO", "DO#", "RE", "RE#", "MI", "FA", "FA#", "SOL", "SOL#", "LA", "LA#", "SI" };
        private static Note IdToNoteTest(int id)
        {
            Note note = new Note();
            note.Octave = (byte)(id / Xylobot.octaveSize + Xylobot.startOctaveXylophone);
            note.High = (byte)(id % Xylobot.octaveSize);
            note.HighString = tabNote[id % Xylobot.octaveSize];
            note.Tick = 0;
            note.Intensity = 64;
            return note;
        }
    }
}
