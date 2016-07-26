using Common;
using Concept.Model;
using Concept.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace Framework
{
    [IntlConceptName("Framework.Sequencer.Name", "Sequencer")]
    public class Sequencer : ConceptComponent
    {
        #region Constructor

        public Sequencer()
        {
            Xylobot = new Xylobot();
            SpeedPlay = 1.0;
            _threadXyloBot = new Thread(ManageXylobot);
            _threadXyloBot.Start();
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
        
        public bool IsPlaying
        {
            get { return _isPlaying; }
            private set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    DoPropertyChanged(IsPlayingPropertyName);
                }
            }
        }
        private bool _isPlaying;
        public const string IsPlayingPropertyName = "IsPlaying";

        public ObservableCollection<string> Errors { get; set; }

        #endregion

        #region Methods

        private void ManageXylobot()
        {
            Init();
            while (_actionsThread != ActionsThread.Terminate)
            {
                Thread.Sleep(10);

                if (Xylobot.IsInit)
                    switch (_actionsThread)
                    {
                        case ActionsThread.PlayPlaylist:

                            PlayPlaylist();
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
            else
                for (int i = 0; i < Xylobot.numberKeysXylophone; i++)
                    Xylobot.SendKeyHitTime(i, FrameworkController.Instance.Settings.Keys[i].HitTime); //TODO : à voir
        }

        private void PlayPartition(PartitionXylo partition)
        {
            List<Note> notes = new List<Note>();
            int k = 0, i;
            try {
                Xylobot.SendTempo((ushort)(partition.Tempo * 1000)); //tempo ms à micros
                while (Xylobot.ArduinoCurrentTick < partition.Notes[partition.Notes.Count - 1].Tick || k < partition.Notes.Count)
                {
                    if (_stop || _next)
                    {
                        _next = false;
                        Xylobot.Stop();
                        ActualiseProgress();
                        break;
                    }
                    else if (_pause)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                                   IsPlaying = false));
                        Xylobot.Pause();
                        while (_pause && !_stop && !_next)
                            Thread.Sleep(50);
                        if(!_stop)
                            Xylobot.Start();
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                                   IsPlaying = true));
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

        private void PlayPlaylist()
        {
            try
            {
                _pause = false;
                _stop = false;
                _next = false;
                while(Playlist.Partitions.Count > 0)
                {
                    Xylobot.Stop(); //Fait un reset avant la prochaine partition
                    if (!_stop)
                    {
                        Xylobot.Start();//Start la musique
                        Application.Current.Dispatcher.Invoke(new Action(() => {
                            CurrentPartition = Playlist.Partitions[0];
                            IsPlaying = true;
                            })); 
                        PlayPartition(Playlist.Partitions[0]);
                        if (!_stop)
                            Application.Current.Dispatcher.Invoke(new Action(() => 
                                Playlist.Partitions.RemoveAt(0)));
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                            PartitionProgress = 0));
                    }
                    else {
                        break;
                    }
                }
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    CurrentPartition = null;
                    IsPlaying = false;
                }));

            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(new Action(() => Errors.Add(e.Message)));
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

        public void Stop()
        {
            _stop = true;
        }

        public void Next()
        {
            if(Playlist.Partitions.Count > 0)
                _next = true;
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
            _threadXyloBot.Join();
        }

        private void ApplyChange()
        {
            if (_speedPlayChanged)
            {
                _speedPlayChanged = false;
                Xylobot.SendSpeedFactor(SpeedPlay);
            }
        }

        private void ActualiseProgress()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
                PartitionProgress = (double)Xylobot.ArduinoCurrentTick / CurrentPartition.Notes[CurrentPartition.Notes.Count - 1].Tick
            ));
        }

        #endregion

        #region private

        private enum ActionsThread { Terminate = -1, Nothing, PlayPlaylist, PlayOneNote, ChangeKeyHitTime }

        private Thread _threadXyloBot;
        private ActionsThread _actionsThread = ActionsThread.Nothing;
        private bool _pause = true, _stop = true, _next = false, _speedPlayChanged = false;
       
        private Note _noteToPlay;   //Note à jouer pour PlayOneNote

        int _indexKey;      //Index de la touche pour ChangeKeyHitTime
        double _keyHitTime; //Temps de frappe pour ChangeKeyHitTime

        #endregion
    }
}
