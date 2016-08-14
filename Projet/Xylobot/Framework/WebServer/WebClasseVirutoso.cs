using Concept.Model;
using Concept.Utils;
using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using WebCore;
using WebMaterial;
using WebProgressBar;

namespace Framework
{
    public class VirutosoWebController
    {
        public VirutosoWebController(Sequencer sequencer, Playlist principalPlaylist)
        {
            _sequencer = sequencer;
            _principalPlaylist = principalPlaylist;
            _timer = new System.Timers.Timer(500);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            PartitionProgress = _sequencer.PartitionProgress;
        }

        public string PartitionTitle
        {
            get
            {
                if (_sequencer.CurrentPartition != null)
                    return _sequencer.CurrentPartition.Title;
                else
                    return "Aucune partition en cours";
            }
        }

        public double PartitionProgress
        {
            get
            {
                return _partitionProgress;
            }
            private set
            {
                _partitionProgress = value;
            }
        }
        private double _partitionProgress;

        public string Partitions
        {
            get
            {
                string tmp = "";
                for (int i = 0; i < _principalPlaylist.Partitions.Count; i++)
                {
                    tmp += _principalPlaylist.Partitions[i].Title;
                    if (i != _principalPlaylist.Partitions.Count - 1)
                        tmp += "££";
                }
                return tmp;
            }
        }

        public string Catalogue
        {
            get
            {
                string path = FrameworkController.Instance.Settings.DefaultPathLoadFile + "\\Catalogue\\";
                string tmp = "";
                int i = 0;
                string[] extensions = { ".midi", ".mid" };
                string[] fileNames = Directory.GetFiles(path, "*.*")
                    .Where(f => extensions.Contains(new FileInfo(f).Extension.ToLower())).ToArray();

                foreach (string fileName in fileNames)
                {
                    tmp += fileName.Remove(0, path.Length);
                    if (i++ != fileNames.Length - 1)
                        tmp += "££";
                }
                return tmp;
            }
            set { _selectFileName = value; }
        }

        public void DoRequest()
        {
            if (_selectFileName != null && _selectFileName != "")
            {
                PartitionXylo partitionXylo = new PartitionXylo();
                PartitionMidi partitionMidi = new PartitionMidi();
                var messages = new MessageCollection();

                //p.LoadFromFile(FrameworkController.Instance.Settings.DefaultPathLoadFile + "\\Catalogue\\"
                //    + _selectFileName, PluginClassManager.AllFactories, messages);
                //p.Name = _selectFileName.Split('.')[0];
                //if (messages.Count == 0)
                partitionMidi.Load(FrameworkController.Instance.Settings.DefaultPathLoadFile + "\\Catalogue\\" + _selectFileName);
                partitionXylo = partitionMidi.ConvertCompleteToPartitionXylo();
                Application.Current.Dispatcher.Invoke(new Action(() => _principalPlaylist.AddPartition(partitionXylo)));
                
            }
        }

        private string _selectFileName;
        private System.Timers.Timer _timer;
        private Sequencer _sequencer;
        private Playlist _principalPlaylist;
    }

    [WebRenderCustom(nameof(VirutosoWebController.PartitionProgress), typeof(WebRoundProgressBarRender))]
    [WebRenderCustom(nameof(VirutosoWebController.PartitionTitle), typeof(WebMaterialStringRefreshRender))]
    [WebRenderCustom(nameof(VirutosoWebController.Partitions), typeof(WebMaterialShowListRender))]
    [WebRenderCustom(nameof(VirutosoWebController.Catalogue), typeof(WebMaterialSelectListRender))]
    public class CustomVirutosoWebView : WebMaterialView
    {
        public CustomVirutosoWebView(VirutosoWebController model)
        {
            InitDefautlView(model);
            WebRoundProgressBarRender render;

            render = Find(nameof(VirutosoWebController.PartitionProgress)) as WebRoundProgressBarRender;
            render.Color = "33cc33";
            render.ValMin = 0;
            render.ValMax = 1;

            WebMaterialStringRefreshRender render2;

            render2 = Find(nameof(VirutosoWebController.PartitionTitle)) as WebMaterialStringRefreshRender;
            render2.Title = "Partition courante";

            WebMaterialShowListRender render3;

            render3 = Find(nameof(VirutosoWebController.Partitions)) as WebMaterialShowListRender;
            render3.Title = "Liste de lecture";

            WebMaterialSelectListRender render4;

            render4 = Find(nameof(VirutosoWebController.Catalogue)) as WebMaterialSelectListRender;
            render4.Title = "Catalogue des partitions";

            WebMaterialVoidRender render5;

            render5 = Find(nameof(VirutosoWebController.DoRequest)) as WebMaterialVoidRender;
            render5.Title = "Demande d'ajout";
        }
    }
}
