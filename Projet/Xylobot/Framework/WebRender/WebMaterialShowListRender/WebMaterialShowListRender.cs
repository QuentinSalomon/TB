﻿using System.IO;
using System.Reflection;
using WebCore;

namespace Framework
{
    public class WebMaterialShowListRender : WebPropertyRender
    {
        public WebMaterialShowListRender(PropertyDescription property, object obj)
        {
            Model = obj;
            PropDescription = property;
        }   

        public override void BuildControl(WebPageBuilder pagebuilder)
        {
            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "Framework.WebRender.WebMaterialShowListRender.Files.MaterialList.html")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%title%>", Title);
                tmp = tmp.Replace("<%id%>", PropDescription.PropertyInfo.Name);
                pagebuilder.ApendHtml(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "Framework.WebRender.WebMaterialShowListRender.Files.MaterialList.css")))
            {
                string tmp = file.ReadToEnd();

                pagebuilder.ApendCss(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "Framework.WebRender.WebMaterialShowListRender.Files.MaterialList.js")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%id%>", PropDescription.PropertyInfo.Name);
                string tmpVal = (Model as VirutosoWebController).Partitions.Replace("££", "\",\"");
                tmp = tmp.Replace("<%values%>", "\"" + tmpVal + "\"");
                pagebuilder.ApendJs(tmp);
            }
            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "Framework.WebRender.WebMaterialShowListRender.Files.MaterialListCallBack.js")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%id%>", PropDescription.PropertyInfo.Name);
                SubsciptionCallBackMethode = tmp;
            }
            pagebuilder.RegisterSubscription(PropDescription.PropertyInfo.Name, SubsciptionCallBackMethode, location);
        }

        public string Title { get; set; }
    }
}
