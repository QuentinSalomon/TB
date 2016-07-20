using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WebCore;

namespace Framework
{
    public class WebMaterialShowListRender : WebPropertyRender
    {
        public WebMaterialShowListRender(PropertyDescription property, object obj)
        {
            Model = (obj as string);
            PropDescription = property;
        }   

        public override void BuildControl(WebPageBuilder pagebuilder)
        {
            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.Files.MaterialList.html")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%title%>", PropDescription.PropertyInfo.Name);
                tmp = tmp.Replace("<%id%>", PropDescription.PropertyInfo.Name);
                //tmp = tmp.Replace("<%value%>", _item.PropertyInfo.GetValue(Model).ToString());
                pagebuilder.ApendHtml(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.Files.MaterialList.css")))
            {
                string tmp = file.ReadToEnd();

                pagebuilder.ApendCss(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.Files.MaterialList.js")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%id%>", PropDescription.PropertyInfo.Name);
                tmp = tmp.Replace("<%values%>", Model as string);
                pagebuilder.ApendJs(tmp);
            }
            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.Files.MaterialListCallBack.js")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%values%>", Model as string);
                SubsciptionCallBackMethode = tmp;
            }
            pagebuilder.RegisterProperty(PropDescription.PropertyInfo.Name, id.ToString(), location);
            pagebuilder.RegisterSubscription(PropDescription.PropertyInfo.Name, SubsciptionCallBackMethode, location);
        }
    }
}
