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

namespace Framework
{
    [ConceptView(typeof(UserControlSequencer))]
    [IntlConceptName("Framework.Sequencer.Name", "Sequencer")]
    [ConceptSmallImage(typeof(Playlist), "/Images/Sequencer32x32.png")]
    [ConceptLargeImage(typeof(Playlist), "/Images/Sequencer64x64.png")]
    public class Sequencer : ConceptComponent
    {
        #region Constructor

        public Sequencer()
        {
            threadXyloBot = new Thread(ManageXylobot);
            threadXyloBot.Start();
            Errors = new ObservableCollection<string>();
        }

        #endregion

        #region Propriétés

        [ConceptViewVisible(true)] //TODO: not visible
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

        public ObservableCollection<string> Errors { get; set; }

        #endregion

        #region Methods

        public void ManageXylobot()
        {

        }

        public void Init()
        {
            //Errors.Add("blabla");
            try {
                Xylobot.XyloCommunication.Init();
            }
            catch (Exception e) {
                Errors.Add(e.Message);
            }
        }

        public void PlayPartition(PartitionXylo partition)
        {
            List<Note> notes = new List<Note>();
            PartitionXylo tmpPartition = new PartitionXylo();
            tmpPartition.CopyFrom(partition);
            try {
                while (tmpPartition.Notes.Count > 0)
                {
                    for (int i = 0; i < Xylobot.XyloCommunication.ArduinoNoteSizeAvaible; i++)
                    {
                        if (i >= partition.Notes.Count)
                            break;
                        notes.Add(tmpPartition.Notes[i]);
                    }
                    //Envoie des notes et recupération
                    Xylobot.XyloCommunication.SendNotes(notes);
                    notes.Clear();
                    Thread.Sleep(50);
                }
            }
            catch (Exception e) {
                Errors.Add(e.Message);
            }
        }

        public void PlayPlayist(Playlist playlist)
        {
            Xylobot.XyloCommunication.SendMessage(SendTypeMessage.Stop); //Stop l'execution en cours pour jouer la playlist
            Xylobot.XyloCommunication.SendMessage(SendTypeMessage.Start);//Start la musique
            foreach (PartitionXylo p in playlist)
                PlayPartition(p);

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
                        PlayPlayist(Playlist);
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

        #endregion

        #region private

        private Thread threadXyloBot;
        private bool _pause = true, _stop = true;

        #endregion
    }
}
