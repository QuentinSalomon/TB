using System;
using System.IO;
using System.Reflection;
using WebCore;

namespace WebMaterial
{
    public class WebMaterialShowListRender : WebPropertyRender
    {
        public WebMaterialShowListRender(PropertyDescription property, object obj)
        {
            PropDescription = property;
            Model = obj;
        }

        public override void BuildControl(WebPageBuilder pagebuilder)
        {
            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebMaterial.Render.WebMaterialEditStringRender.Files.MaterialString.html")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%title%>", PropDescription.PropertyInfo.Name);
                tmp = tmp.Replace("<%id%>", id.ToString());
                tmp = tmp.Replace("<%value%>", PropDescription.PropertyInfo.GetValue(Model).ToString());
                pagebuilder.ApendHtml(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebMaterial.Render.WebMaterialEditStringRender.Files.MaterialString.css")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%id%>", id.ToString());
                pagebuilder.ApendCss(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebMaterial.Render.WebMaterialEditStringRender.Files.MaterialString.js")))
            {
                string tmp = file.ReadToEnd();

                pagebuilder.ApendJs(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebMaterial.Render.WebMaterialEditStringRender.Files.MaterialStringCallBack.js")))
            {
                string tmp = file.ReadToEnd();
               
                SubsciptionCallBackMethode = tmp;
            }

            pagebuilder.RegisterProperty(PropDescription.PropertyInfo.Name, id.ToString(), location);
            pagebuilder.RegisterSubscription(PropDescription.PropertyInfo.Name, SubsciptionCallBackMethode, location);
        }
    }
}
