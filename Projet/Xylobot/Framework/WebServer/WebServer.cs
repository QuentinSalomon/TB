using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCore;
using WebMaterial;
using Woopsa;

namespace Framework
{
    public class VirtuosoWebServer
    {
        ConceptWebServer Server { get; set; }
        private VirutosoWebController _virutosoWebController;

        public VirtuosoWebServer(Sequencer sequencer, Playlist principalPlaylist)
        {
            _virutosoWebController = new VirutosoWebController(sequencer, principalPlaylist);

            Server = new ConceptWebServer();
            MaterialMenuView menuview = new MaterialMenuView();
            menuview.RegisterSubView("custom", new CustomVirutosoWebView(_virutosoWebController));
            Server.RegisterWebApp("Virtuoso", new WebMaterialApp(menuview));

            Server.WoopsaServer.WebServer.Routes.Add("/", HTTPMethod.GET, new RouteHandlerRedirect("Web/Virtuoso", WoopsaRedirection.Temporary));
        }

        public void Close()
        {
            Server.Dispose();
        }
    }
}
