using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
            _timer = new Timer(500);
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
                    tmp += "\"" + _principalPlaylist.Partitions[i].Title + "\"";
                    if (i != _principalPlaylist.Partitions.Count - 1)
                        tmp += ",";
                }
                return tmp;
            }
        }


        private Timer _timer;
        private Sequencer _sequencer;
        private Playlist _principalPlaylist;
    }

    [WebRenderCustom(nameof(VirutosoWebController.PartitionProgress), typeof(WebRoundProgressBarRender))]
    [WebRenderCustom(nameof(VirutosoWebController.PartitionTitle), typeof(WebMaterialStringRefreshRender))]
    [WebRenderCustom(nameof(VirutosoWebController.Partitions), typeof(WebMaterialShowListRender))]
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
