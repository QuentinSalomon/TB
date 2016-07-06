using Common;
using Concept.Model;
using Concept.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Framework
{
    //[ConceptView(typeof(UserControlSequencer))]
    [IntlConceptName("Framework.Sequencer.Name", "Sequencer")]
    [ConceptSmallImage(typeof(Playlist), "/Images/Sequencer32x32.png")]
    [ConceptLargeImage(typeof(Playlist), "/Images/Sequencer64x64.png")]
    public class Sequencer : ConceptComponent
    {
        #region Constructor

        public Sequencer()
        {
            Xylobot = new Xylobot();
            SpeedPlay = 1.0;
            threadXyloBot = new Thread(ManageXylobot);
            threadXyloBot.Start();
            Errors = new ObservableCollection<string>();
        }

        #endregion

        #region Propriétés
        
        [IntlConceptName("Framework.Sequencer.Xylobot", "Xylobot")]
        public Xylobot Xylobot
        {
            get { return _xylobot; }
            set
            {
                if (_xylobot != value)
                {
                    _xylobot = value;
                    DoPropertyChanged(XylobotPropertyName);
                }
            }
        }
        private Xylobot _xylobot;
        public const string XylobotPropertyName = "Xylobot";
        
        [IntlConceptName("Framework.Sequencer.Playlist", "Playlist")]
        public Playlist Playlist
        {
            get { return _playlist; }
            set
            {
                if (_playlist != value)
                {
                    _playlist = value;
                    DoPropertyChanged(PlaylistPropertyName);
                }
            }
        }
        private Playlist _playlist;
        public const string PlaylistPropertyName = "Playlist";
        
        [IntlConceptName("Framework.Sequencer.CurrentPartition", "CurrentPartition")]
        public PartitionXylo CurrentPartition
        {
            get { return _currentPartition; }
            set
            {
                if (_currentPartition != value)
                {
                    _currentPartition = value;
                    DoPropertyChanged(CurrentPartitionPropertyName);
                }
            }
        }
        private PartitionXylo _currentPartition;
        public const string CurrentPartitionPropertyName = "CurrentPartition";
        
        [IntlConceptName("Framework.Sequencer.Tempo", "Tempo")]
        public UInt16 Tempo
        {
            get { return _tempo; }
            set
            {
                if (_tempo != value)
                {
                    _tempo = value;
                    DoPropertyChanged(TempoPropertyName);
                    Xylobot.SendTempo(value);
                }
            }
        }
        private UInt16 _tempo;
        public const string TempoPropertyName = "Tempo";
        
        [IntlConceptName("Framework.Sequencer.SpeedPlay", "SpeedPlay")]
        public double SpeedPlay
        {
            get { return _speedPlay; }
            set
            {
                if (_speedPlay != value)
                {
                    _speedPlay = value;
                    DoPropertyChanged(SpeedPlayPropertyName);
                    _speedPlayChanged = true;
                }
            }
        }
        private double _speedPlay;
        public const string SpeedPlayPropertyName = "SpeedPlay";
        
        [IntlConceptName("Framework.Sequencer.PartitionProgress", "PartitionProgress")]
        public double PartitionProgress
        {
            get { return _partitionProgress; }
            private set
            {
                if (_partitionProgress != value)
                {
                    _partitionProgress = value;
                    DoPropertyChanged(PartitionProgressPropertyName);
                }
            }
        }
        private double _partitionProgress;
        public const string PartitionProgressPropertyName = "PartitionProgress";

        public ObservableCollection<string> Errors { get; set; }

        #endregion

        #region Methods

        public void ManageXylobot()
        {
            Xylobot.Init();
            while (_actionsThread != ActionsThread.Terminate)
            {
                Thread.Sleep(10);
                switch (_actionsThread)
                {
                    case ActionsThread.PlayPlaylist:

                        PlayPlaylist(Playlist);
                        _actionsThread = ActionsThread.Nothing;
                        _stop = false; //reset le stop au cas ou la fonction s'est termninée avec un stop
                        break;
                    case ActionsThread.PlayOneNote:
                        _actionsThread = ActionsThread.Nothing;

                        List<Note> note = new List<Note>();
                        note.Add(_noteToPlay);
                        Xylobot.Stop(); //Vide le buffer de l'arduino
                        Xylobot.SendNotes(note);    //Charge la note dans le buffer
                        Xylobot.Start();    //Lance la lecture du buffer
                        break;
                    case ActionsThread.ChangeKeyHitTime:
                        _actionsThread = ActionsThread.Nothing;

                        Xylobot.Stop();
                        Xylobot.SendKeyHitTime(_indexKey, _keyHitTime);
                        Xylobot.Keys[_indexKey].HitTime = _keyHitTime;
                        _actionsThread = ActionsThread.PlayOneNote;
                        break;
                    default:
                        break;
                }
            }
        }

        private void Init()
        {
            Xylobot.Init();
            if(!Xylobot.IsInit)
                Application.Current.Dispatcher.Invoke(new Action(() =>
                    Errors.Add("Init USB failed.")));
        }

        private void PlayPartition(PartitionXylo partition)
        {
            List<Note> notes = new List<Note>();
            int k = 0, i;
            try {
                while (Xylobot.ArduinoCurrentTick < partition.Notes[partition.Notes.Count - 1].Tick || k < partition.Notes.Count)
                {
                    if (_stop)
                    {
                        Xylobot.Stop();
                        break;
                    }
                    else if (_pause)
                    {
                        Xylobot.Pause();
                        while (_pause && !_stop)
                            Thread.Sleep(50);
                        if(!_stop)
                            Xylobot.Start();
                    }
                    else {
                        //Application des changement pour la lecture de la partition
                        ApplyChange();

                        //Prise des notes à envoyer
                        for (i = 0; i < Xylobot.ArduinoNoteSizeAvaible; i++)
                        {
                            if (k + i >= partition.Notes.Count)
                                break;
                            notes.Add(partition.Notes[k + i]);
                        }
                        k += i;
                        if (k == 1251)
                            ;
                        Xylobot.SendNotes(notes);

                        ActualiseProgress();

                        notes.Clear();
                        Thread.Sleep(50);
                    }
                }
            }
            catch (Exception e) {
                Application.Current.Dispatcher.Invoke(new Action(() => this.Errors.Add(e.Message)));
            }
        }

        private void PlayPlaylist(Playlist playlist)
        {
            try
            {
                _pause = false;
                _stop = false;
                Xylobot.Stop(); //Stop l'execution en cours pour jouer la playlist
                Xylobot.Start();//Start la musique
                foreach (PartitionXylo p in playlist.Partitions)
                {
                    if (!_stop)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                            CurrentPartition = p)); 
                        PlayPartition(p);
                    }
                    else {
                        break;
                    }
                }
                Application.Current.Dispatcher.Invoke(new Action(() =>
                    CurrentPartition = null));

            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(new Action(() => this.Errors.Add(e.Message)));
            }

        }

        public void PlayPause()
        {
            if (_actionsThread == ActionsThread.Nothing)
            {
                _pause = false;
                _actionsThread = ActionsThread.PlayPlaylist;
            }
            else
                _pause = !_pause;
        }

        public void ChangeKeyHitTime(Note n, double hitTime)
        {
            _indexKey = (n.High + (n.Octave - Xylobot.startOctaveXylophone) * Xylobot.octaveSize);
            _keyHitTime = hitTime;
            _noteToPlay = n;
            if (!_stop) //Arrête si une playlist est en cours de lecture
                Stop();
            _actionsThread = ActionsThread.ChangeKeyHitTime;
        }

        public void InitKeysHitTime(double[] hitTimes)
        {

        }

        public void Mute()
        {
            _muteChanged = true;
        }

        public void Stop()
        {
            _stop = true;
        }

        public void Finish()
        {
            if (Xylobot.IsInit)
                Xylobot.Stop();
            else
                Xylobot.AbortInit = true;
            //_pause = false;
            _stop = true;
            _actionsThread = ActionsThread.Terminate;
            threadXyloBot.Join();
        }

        private void ApplyChange()
        {
            if (_speedPlayChanged)
            {
                _speedPlayChanged = false;
                Xylobot.SendSpeedFactor(SpeedPlay);
            }
            if (_tempoChanged)
            {
                _tempoChanged = false;
                Xylobot.SendTempo(Tempo);
            }
            if (_muteChanged)
            {
                int i;
                if (!_isMute)
                    for (i = 0; i < Xylobot.numberKeysXylophone; i++)
                        Xylobot.SendKeyHitTime(i, 3);
                else
                    for (i = 0; i < Xylobot.numberKeysXylophone; i++)
                        Xylobot.SendKeyHitTime(i, Xylobot.Keys[i].HitTime);
                _muteChanged = false;
                _isMute = !_isMute;
            }
        }

        private void ActualiseProgress()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
                PartitionProgress = (double)Xylobot.ArduinoCurrentTick / CurrentPartition.Notes[CurrentPartition.Notes.Count - 1].Tick
            ));
        }

        #endregion

        #region Wpf Commands

        public WpfCommand CommandPlayPlaylist
        {
            get
            {
                if (_commandPlayPlaylist == null)
                {
                    _commandPlayPlaylist = new WpfCommand();
                    _commandPlayPlaylist.Executed += (sender, e) =>
                    {
                        if (_actionsThread == ActionsThread.Nothing)
                            _actionsThread = ActionsThread.PlayPlaylist;
                        else
                            _pause = !_pause;
                    };

                    _commandPlayPlaylist.CanExecuteChecking += (sender, e) =>
                    {
                        e.CanExecute = true;
                    };
                }
                return _commandPlayPlaylist;
            }
        }
        private WpfCommand _commandPlayPlaylist;

        public WpfCommand CommandInit
        {
            get
            {
                if (_commandInit == null)
                {
                    _commandInit = new WpfCommand();
                    _commandInit.Executed += (sender, e) =>
                    {
                        Init();
                    };

                    _commandInit.CanExecuteChecking += (sender, e) =>
                    {
                        e.CanExecute = true;
                    };
                }
                return _commandInit;
            }
        }
        private WpfCommand _commandInit;

        public WpfCommand CommandStop
        {
            get
            {
                if (_commandStop == null)
                {
                    _commandStop = new WpfCommand();
                    _commandStop.Executed += (sender, e) =>
                    {
                        _stop = true;
                    };

                    _commandStop.CanExecuteChecking += (sender, e) =>
                    {
                        e.CanExecute = true;
                    };
                }
                return _commandStop;
            }
        }
        private WpfCommand _commandStop;

        #endregion

        #region private

        private enum ActionsThread { Terminate = -1, Nothing, PlayPlaylist, PlayOneNote, ChangeKeyHitTime }

        private Thread threadXyloBot;
        private ActionsThread _actionsThread = ActionsThread.Nothing;
        private bool _pause = true, _stop = true, _tempoChanged = false, _speedPlayChanged = false, _muteChanged = false, _isMute = false;
       
        private Note _noteToPlay;   //Note à jouer pour PlayOneNote

        int _indexKey;      //Index de la touche pour ChangeKeyHitTime
        double _keyHitTime; //Temps de frappe pour ChangeKeyHitTime
        #endregion
    }
}
