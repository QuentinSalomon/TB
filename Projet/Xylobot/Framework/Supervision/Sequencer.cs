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

        [ConceptViewVisible(false)]
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

        [ConceptViewVisible(false)]
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

        [ConceptViewVisible(true)]
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

        [ConceptViewVisible(true)]
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

        [ConceptViewVisible(true)]
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

        public ObservableCollection<string> Errors { get; set; }

        #endregion

        #region Methods

        public void ManageXylobot()
        {
            Xylobot.Init();
            while (_actionsThread != -1)
            {
                Thread.Sleep(10);
                switch (_actionsThread)
                {
                    case 1:
                        PlayPlaylist(Playlist);
                        if(_actionsThread != -1)
                            _actionsThread = 0;
                        break;
                    default:
                        break;
                }
            }
        }

        public void Init()
        {
            Xylobot.Init();
        }

        public void PlayPartition(PartitionXylo partition)
        {
            List<Note> notes = new List<Note>();
            int k = 0,i;
            try {
                while (k < partition.Notes.Count)
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
                        //Check les changement de tempo et de vitesse de lecture
                        if (_speedPlayChanged)
                        {
                            _speedPlayChanged = false;
                            Xylobot.SendSpeedFactor(SpeedPlay);
                        }
                        //if (_tempoChanged)
                        //{
                        //    _tempoChanged = false;
                        //    Xylobot.SendTempo(Tempo);
                        //}

                        //Prise des notes à envoyer
                        for (i = 0; i < Xylobot.ArduinoNoteSizeAvaible; i++)
                        {
                            if (k + i >= partition.Notes.Count)
                                break;
                            notes.Add(partition.Notes[k + i]);
                        }
                        k += i;
                        //Envoie des notes et recupération
                        Xylobot.SendNotes(notes);
                        notes.Clear();
                        Thread.Sleep(50);
                    }
                }

                //Attente de la fin de la pratition en cours avant de lancer la suivante
                while(Xylobot.ArduinoCurrentTick < partition.Notes[partition.Notes.Count-1].Tick)
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
                        if (!_stop)
                            Xylobot.Start();
                    }
                    Xylobot.SendNotes(notes);
                    Thread.Sleep(50);
                }
            }
            catch (Exception e) {
                Errors.Add(e.Message);
            }
        }

        public void PlayPlaylist(Playlist playlist)
        {
            try
            {
                Xylobot.Stop(); //Stop l'execution en cours pour jouer la playlist
                _pause = false;
                _stop = false;
                Xylobot.Start();//Start la musique
                foreach (PartitionXylo p in playlist.Partitions)
                {
                    if (!_stop)
                    {
                        CurrentPartition = p;
                        PlayPartition(p);
                    }
                    else {
                        //CurrentPartition = null;
                        break;
                    }
                }
                CurrentPartition = null;
            }
            catch (Exception e)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => this.Errors.Add(e.Message)));
            }

        }

        public void PlayPause()
        {
            if (_actionsThread == 0)
                _actionsThread = 1;
            else
                _pause = !_pause;
        }

        public void PlayNote(Note n)
        {
            Stop();
            List<Note> note = new List<Note>();
            note.Add(n);
            Xylobot.SendNotes(note);
            Xylobot.Start();
        }

        public void ChangeKeyHitTime(int index, double hitTime)
        {
            Xylobot.SendKeyHitTime(index, hitTime);
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
            _actionsThread = -1;
            threadXyloBot.Join();
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
                        if (_actionsThread == 0)
                            _actionsThread = 1;
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

        private Thread threadXyloBot;
        private bool _pause = true, _stop = true, _tempoChanged = false, _speedPlayChanged = false;
        private int _actionsThread=0;

        #endregion
    }
}
