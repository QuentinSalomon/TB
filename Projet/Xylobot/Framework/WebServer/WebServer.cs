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
            Server.RegisterWebApp("Virtuoso", 
                new WebMaterialApp(new CustomVirutosoWebView(_virutosoWebController)));

            Server.WoopsaServer.WebServer.Routes.Add("/", HTTPMethod.GET, 
                new RouteHandlerRedirect("Web/Virtuoso", WoopsaRedirection.Temporary));
        }

        public void Close()
        {
            Server.Dispose();
        }
    }
}
