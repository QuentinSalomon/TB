using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            _threadActualise = new Thread(Actualise);
            _threadActualise.Start();
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

        public StaticListPartitionXylo Partitions
        {
            get
            {
                return _principalPlaylist.Partitions;
            }

        }

        private void Actualise()
        {
            while(!_finishThread)
            {
                PartitionProgress = _sequencer.PartitionProgress;
                Thread.Sleep(300);
            }
        }

        public void Finish()
        {
            _finishThread = true;
        }

        private Thread _threadActualise;
        bool _finishThread = false;
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

            WebMaterialShowListRender render2;
            //render2 = Find(nameof(VirutosoWebController.Partitions)) as WebMaterialShowListRender;
            //render2.List = model.Partitions;
        }
    }
}
