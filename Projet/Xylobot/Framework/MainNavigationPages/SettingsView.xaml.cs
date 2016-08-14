using System;
using System.Timers;
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
                _currentNoteCalibration = IdToNoteCalibration(CurrentIdNote);
                TextBlockKeyTitle.Text = "Note : " + _currentNoteCalibration.HighString + 
                        "   \tOctave : " + _currentNoteCalibration.Octave;
                TextBlockHitTime.Text = Settings.Keys[CurrentIdNote].HitTime.ToString();
            }
        }
        private int _currentIdNote;

        private Note _currentNoteCalibration;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Settings = (DataContext as SettingsViewModel).Settings;
            CurrentIdNote = 0;
            InitTimerCalibration();
        }

        #region Functions Composants

        private void ButtonLessTime_Click(object sender, RoutedEventArgs e)
        {
            Settings.ChangeKey(CurrentIdNote, Round(Settings.Keys[CurrentIdNote].HitTime - 0.1));
            TextBlockHitTime.Text = Settings.Keys[CurrentIdNote].HitTime.ToString();
        }

        private void ButtonMoreTime_Click(object sender, RoutedEventArgs e)
        {
            Settings.ChangeKey(CurrentIdNote, Round(Settings.Keys[CurrentIdNote].HitTime + 0.1));
            TextBlockHitTime.Text = Settings.Keys[CurrentIdNote].HitTime.ToString();
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

        private void CheckBoxPlayNote_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxPlayNote.IsChecked == true)
                CheckBoxPlayGamme.IsChecked = false;
        }

        private void CheckBoxPlayGamme_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxPlayGamme.IsChecked == true)
                CheckBoxPlayNote.IsChecked = false;
        }

        private void ButtonSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings.SaveToFile("FileConfig.xml");
            Settings.NeedSaved = false;
            WindowMessageBoxAutoClosed w = new WindowMessageBoxAutoClosed();
            w.TypeWindow = TypeWindow.Information;
            w.Text = "Données sauvegardée.";
            //w.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xA6, 0x00));
            w.Show();
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
            //Reset le code à chaque affichage des settings
            if ((bool)e.NewValue)
            {
                UserControlCodeSettings.Reset();
                _timerCalibration.Start();
            }
            else
            {
                _timerCalibration.Stop();
                (DataContext as SettingsViewModel).Sequencer.StopChangeKeyHitTime();
            }

        }

        #endregion

        private double Round(double value)
        {
            return (double)Math.Truncate((decimal)(100 * value)) / 100;
        }

        private static readonly string[] tabNote = new string[]
        { "DO", "DO#", "RE", "RE#", "MI", "FA", "FA#", "SOL", "SOL#", "LA", "LA#", "SI" };
        private static Note IdToNoteCalibration(int id)
        {
            Note note = new Note();
            note.Octave = (byte)(id / Xylobot.octaveSize + Xylobot.startOctaveXylophone);
            note.High = (byte)(id % Xylobot.octaveSize);
            note.HighString = tabNote[id % Xylobot.octaveSize];
            note.Tick = 0;
            note.Intensity = 0;
            return note;
        }

        #region Timer calibration

        private void InitTimerCalibration()
        {
            if (_timerCalibration == null)
            {
                _timerCalibration = new Timer(1000);
                _timerCalibration.Elapsed += OnTimedEvent;
                _timerCalibration.AutoReset = true;
                _timerCalibration.Stop();
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if(CheckBoxPlayNote.IsChecked == true)
                        (DataContext as SettingsViewModel).Sequencer.ChangeKeyHitTimeWithPlay(_currentNoteCalibration,
                            Settings.Keys[CurrentIdNote].HitTime);
                    else if (CheckBoxPlayGamme.IsChecked == true)
                        (DataContext as SettingsViewModel).Sequencer.ChangeKeyHitTimeWithGamme(_currentNoteCalibration,
                            Settings.Keys[CurrentIdNote].HitTime);
                    else
                        (DataContext as SettingsViewModel).Sequencer.ChangeKeyHitTime(_currentNoteCalibration,
                            Settings.Keys[CurrentIdNote].HitTime);
                }
            ));
        }

        private Timer _timerCalibration;

        #endregion
    }
}
