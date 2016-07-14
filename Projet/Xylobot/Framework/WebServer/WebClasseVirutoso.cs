using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                return _sequencer.PartitionProgress;
            }
        }

        //public StaticListPartitionXylo Partitions
        //{
        //    get
        //    {
        //        return _principalPlaylist.Partitions;
        //    }
        //}

        private Sequencer _sequencer;
        private Playlist _principalPlaylist;
    }

    [WebRenderCustom(nameof(VirutosoWebController.PartitionProgress), typeof(WebRoundProgressBarRender))]
    [WebRenderCustom(nameof(VirutosoWebController.PartitionTitle), typeof(WebMaterialStringRefreshRender))]
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
