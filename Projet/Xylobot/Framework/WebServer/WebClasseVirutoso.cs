using Common;
using Concept.Model;
using Concept.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                string[] fileNames = Directory.GetFiles(path, "*.xml");

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
                PartitionXylo p = new PartitionXylo();
                var messages = new MessageCollection();

                p.LoadFromFile(FrameworkController.Instance.Settings.DefaultPathLoadFile + "\\Catalogue\\" + _selectFileName, PluginClassManager.AllFactories, messages);
                p.Name = _selectFileName.Split('.')[0];
                if (messages.Count == 0)
                    Application.Current.Dispatcher.Invoke(new Action(() => _principalPlaylist.AddPartition(p)));
                
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
        }
    }
}
