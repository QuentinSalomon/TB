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

namespace Framework
{
    [ConceptView(typeof(UserControlEditPlaylist))]
    [IntlConceptName("Framework.Playlist.Name", "Playlist principal")]
    [ConceptSmallImage(typeof(Playlist), "/Images/Playlist32x32.png")]
    [ConceptLargeImage(typeof(Playlist), "/Images/Playlist64x64.png")]
    public class Playlist : ConceptComponent
    {
        #region Properties

        [ConceptViewVisible]
        [IntlConceptName("Framework.Playlist.Description", "Description")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    DoPropertyChanged(DescriptionPropertyName);
                }
            }
        }
        private string _description;
        public const string DescriptionPropertyName = "Description";

        #endregion

        [ConceptAutoCreate]
        [IntlConceptName("Framework.Playlist.Partitions", "Partitions")]
        public StaticListPartitionXylo Partitions { get; protected set; }

        #region WPF command

        public WpfCommand CommandAddPartition
        {
            get
            {
                if (_commandAddPartitione == null)
                {
                    _commandAddPartitione = new WpfCommand();
                    _commandAddPartitione.Executed += (sender, e) =>
                    {
                        PartitionXylo p = new PartitionXylo();
                        var messages = new MessageCollection();
                        OpenFileDialog fileDlg = new OpenFileDialog();

                        fileDlg.InitialDirectory = FrameworkController.Instance.Settings.DefaultPathLoadFile;
                        fileDlg.Filter = "txt files (*.xml)|*.xml";
                        fileDlg.RestoreDirectory = true;
                        if (fileDlg.ShowDialog() == DialogResult.OK)
                        {
                            p.LoadFromFile(fileDlg.FileName, PluginClassManager.AllFactories, messages);
                            p.Name = fileDlg.FileName;
                            if (messages.Count > 0)
                                ConceptMessage.ShowError(string.Format("Error while loading the configuration file:\n{0}", messages.Text), "Loading Error");
                            else
                                Partitions.Add(p);
                        }
                    };

                    _commandAddPartitione.CanExecuteChecking += (sender, e) =>
                    {
                        e.CanExecute = true;
                    };
                }
                return _commandAddPartitione;
            }
        }
        private WpfCommand _commandAddPartitione;

        #endregion
    }
}
