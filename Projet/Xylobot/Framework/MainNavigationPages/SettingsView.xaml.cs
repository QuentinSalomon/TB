using System;
using System.Windows;
using System.Windows.Controls;

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
        
        public Settings Settings { get; set; }

        public int CurrentIdNote {
            get { return _currentIdNote; }
            set
            {                
                _currentIdNote = value;
                Note n;
                n = IdToNoteTest(CurrentIdNote);
                TextBlockKeyTitle.Text = "Note : " + n.HighString + "   \tOctave : " + n.Octave;
                //Settings.Keys[CurrentIdNote].HitTime = Truncate(Settings.Keys[CurrentIdNote].HitTime);
                Settings.ChangeKey(CurrentIdNote, Truncate(Settings.Keys[CurrentIdNote].HitTime));
                TextBlockHitTime.Text = Settings.Keys[CurrentIdNote].HitTime.ToString();
            }
        }
        private int _currentIdNote;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Settings = (DataContext as SettingsViewModel).Settings;
            CurrentIdNote = 0;
        }

        private void ButtonLessTime_Click(object sender, RoutedEventArgs e)
        {
            Note n;
            //Settings.Keys[CurrentIdNote].HitTime = Truncate(Settings.Keys[CurrentIdNote].HitTime - 0.1);
            Settings.ChangeKey(CurrentIdNote, Truncate(Settings.Keys[CurrentIdNote].HitTime - 0.1));
            TextBlockHitTime.Text = Settings.Keys[CurrentIdNote].HitTime.ToString();
            n = IdToNoteTest(CurrentIdNote);
            (DataContext as SettingsViewModel).Sequencer.ChangeKeyHitTime(n,
                Settings.Keys[CurrentIdNote].HitTime);
        }

        private void ButtonMoreTime_Click(object sender, RoutedEventArgs e)
        {
            Note n;
            //Settings.Keys[CurrentIdNote].HitTime = Truncate(Settings.Keys[CurrentIdNote].HitTime + 0.1);
            Settings.ChangeKey(CurrentIdNote, Truncate(Settings.Keys[CurrentIdNote].HitTime + 0.1));
            TextBlockHitTime.Text = Settings.Keys[CurrentIdNote].HitTime.ToString();
            n = IdToNoteTest(CurrentIdNote);
            (DataContext as SettingsViewModel).Sequencer.ChangeKeyHitTime(n,
                Settings.Keys[CurrentIdNote].HitTime);
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentIdNote > 0)
                CurrentIdNote--;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentIdNote < Xylobot.numberKeysXylophone - 1)
                CurrentIdNote++;
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

        private void ButtonShutDown_Click(object sender, RoutedEventArgs e)
        {
            WindowMessageBoxConfirmation w = new WindowMessageBoxConfirmation();
            w.Text = "Voulez-vous vraiment fermer l'application et éteindre le Virtuoso?";
            w.Width = 400;
            if (w.Execute() == true)
            {
                System.Diagnostics.Process.Start("ShutDown", "/p");
                FrameworkController.Instance.Unload();
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                UserControlCodeSettings.Reset();
        }
    }
}
