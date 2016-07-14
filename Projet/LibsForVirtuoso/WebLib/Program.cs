using System;
using WebCore;

using Woopsa;
using WebMaterial;

namespace Material
{
    class Program
    {
        static void Main(string[] args)
        {
            Chrono chrono = new Chrono();

            ConceptWebServer server = new ConceptWebServer();
            server.RegisterWebApp("cs",new WebMaterialApp(new MyCustGRaphView(chrono)));
            
            
            MaterialMenuView menuview = new MaterialMenuView();
            menuview.RegisterSubView("standard", new WebMaterialView(chrono));
            menuview.RegisterSubView("custom", new MyCustGRaphView(chrono));
            server.RegisterWebApp("chrono", new WebMaterialApp(menuview));

            server.WoopsaServer.WebServer.Routes.Add("/", HTTPMethod.GET, new RouteHandlerRedirect("Web/chrono", WoopsaRedirection.Temporary));

            Console.WriteLine("Server is running...");
            Console.ReadKey();
            server.Dispose();
        }
    }

    [WebRenderCustom(nameof(Chrono.millisecondes), typeof(WebMaterialStringRefreshRender))]
    [WebRenderCustom(nameof(Chrono.secondes), typeof(WebMaterialStringRefreshRender))]
    [WebRenderCustom(nameof(Chrono.minutes), typeof(WebMaterialStringRefreshRender))]
    [WebRenderCustom(nameof(Chrono.Name), typeof(WebMaterialEditStringRender))]
    //[WebRenderCustom(nameof(Chrono.Running), typeof(WebMaterialBoolRender))]
    
    class MyCustGRaphView : WebMaterialView
    {
        public MyCustGRaphView(object model)
        {
            InitDefautlView(model);
        }
    }

    /*
    [WebRenderCustom(nameof(Chrono.millisecondes), typeof(WebMaterialStringRefreshRender))]
    [WebRenderCustom(nameof(Chrono.secondes), typeof(WebMaterialStringRefreshRender))]
    [WebRenderCustom(nameof(Chrono.minutes), typeof(WebMaterialStringRefreshRender))]
    [WebRenderCustom(nameof(Chrono.Name), typeof(WebMaterialEditStringRender))]
    class MyCustGRaphView : WebMaterialView
    {
        public MyCustGRaphView(object model)
        {
            InitDefautlView(model);
        }
    }*/
}
