using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebCore;

namespace Framework
{
    class WebMaterialSelectListRender : WebPropertyRender
    {
        public WebMaterialSelectListRender(PropertyDescription property, object obj)
        {
            Model = obj;
            PropDescription = property;
        }

        public override void BuildControl(WebPageBuilder pagebuilder)
        {
            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.WebMaterialSelectListRender.Files.MaterialListSelect.html")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%title%>", PropDescription.PropertyInfo.Name);
                tmp = tmp.Replace("<%id%>", PropDescription.PropertyInfo.Name);
                pagebuilder.ApendHtml(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.WebMaterialSelectListRender.Files.MaterialListSelect.css")))
            {
                string tmp = file.ReadToEnd();

                pagebuilder.ApendCss(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.WebMaterialSelectListRender.Files.MaterialListSelect.js")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%id%>", PropDescription.PropertyInfo.Name);
                tmp = tmp.Replace("<%path%>", location + "/" + PropDescription.PropertyInfo.Name);
                string tmpVal = (Model as VirutosoWebController).Catalogue.Replace("££", "\",\"");
                tmp = tmp.Replace("<%values%>", "\"" + tmpVal + "\"");
                pagebuilder.ApendJs(tmp);
            }
            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.WebMaterialSelectListRender.Files.MaterialListSelectCallBack.js")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%id%>", PropDescription.PropertyInfo.Name);
                SubsciptionCallBackMethode = tmp;
            }
            //pagebuilder.RegisterProperty(PropDescription.PropertyInfo.Name, id.ToString(), location);
            pagebuilder.RegisterSubscription(PropDescription.PropertyInfo.Name, SubsciptionCallBackMethode, location);
        }
    }
}
