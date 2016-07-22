using Common;
using Concept.Model;
using Concept.Utils;
using Concept.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace Framework
{
    [IntlConceptName("Framework.Playlist.Name", "Playlist principal")]
    [ConceptSmallImage(typeof(Playlist), "/Images/Playlist32x32.png")]
    [ConceptLargeImage(typeof(Playlist), "/Images/Playlist64x64.png")]
    public class Playlist : ConceptComponent
    {
        #region Properties

        [ConceptViewVisible]
        [IntlConceptName("Framework.Playlist.Title", "Title")]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    DoPropertyChanged(TitlePropertyName);
                }
            }
        }
        private string _title;
        public const string TitlePropertyName = "Title";

        [ConceptAutoCreate]
        [IntlConceptName("Framework.Playlist.Partitions", "Partitions")]
        public StaticListPartitionXylo Partitions { get; protected set; }

        #endregion

        public bool AddPartition(PartitionXylo partition)
        {
            bool existInPlaylist = false;
            foreach (PartitionXylo p in Partitions)
                if (p.Title == partition.Title)
                    existInPlaylist = true;
            if (!existInPlaylist)
                Partitions.Add(partition);
            return !existInPlaylist;
        }

        public void ClearPartitions()
        {
            Partitions.Clear();
        }

        #region WPF command

        public WpfCommand CommandAddPartition
        {
            get
            {
                if (_commandAddPartition == null)
                {
                    _commandAddPartition = new WpfCommand();
                    _commandAddPartition.Executed += (sender, e) =>
                    {
                        PartitionXylo p = new PartitionXylo();
                        var messages = new MessageCollection();
                        OpenFileDialog fileDlg = new OpenFileDialog();

                        fileDlg.InitialDirectory = FrameworkController.Instance.Settings.DefaultPathLoadFile;
                        fileDlg.Filter = "xml files (*.xml)|*.xml";
                        fileDlg.RestoreDirectory = true;
                        if (fileDlg.ShowDialog() == DialogResult.OK)
                        {
                            p.LoadFromFile(fileDlg.FileName, PluginClassManager.AllFactories, messages);
                            p.Name = fileDlg.FileName;
                            if (messages.Count > 0)
                                ConceptMessage.ShowError(string.Format("Error while loading the configuration file:\n{0}", messages.Text), "Loading Error");
                            else
                                AddPartition(p);
                        }
                    };

                    _commandAddPartition.CanExecuteChecking += (sender, e) =>
                    {
                        e.CanExecute = true;
                    };
                }
                return _commandAddPartition;
            }
        }
        private WpfCommand _commandAddPartition;

        public WpfCommand CommandClearPartitions
        {
            get
            {
                if (_commandClearPartitions == null)
                {
                    _commandClearPartitions = new WpfCommand();
                    _commandClearPartitions.Executed += (sender, e) =>
                    {
                        Partitions.Clear();
                    };

                    _commandClearPartitions.CanExecuteChecking += (sender, e) =>
                    {
                        e.CanExecute = true;
                    };
                }
                return _commandClearPartitions;
            }
        }
        private WpfCommand _commandClearPartitions;

    #endregion
    }
}
