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
            PropDescription = property;
            Model = obj;
        }   

        public StaticListPartitionXylo List { get; set; }

        public override void BuildControl(WebPageBuilder pagebuilder)
        {
            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.Files.MaterialList.html")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%title%>", PropDescription.PropertyInfo.Name);
                tmp = tmp.Replace("<%id%>", id.ToString());
                tmp = tmp.Replace("<%value%>", PropDescription.PropertyInfo.GetValue(Model).ToString());
                pagebuilder.ApendHtml(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.Files.MaterialString.css")))
            {
                string tmp = file.ReadToEnd();
                tmp = tmp.Replace("<%id%>", id.ToString());
                tmp = tmp.Replace("<%count%>", List.Count.ToString());
                string tmpValues = "";
                for (int i = 0; i < List.Count; i++)
                    tmpValues += "'" + List[i].Title + "',";
                tmpValues.Remove(tmpValues.Length - 1); //Suppression de la dernière ,
                tmp = tmp.Replace("<%values%>", tmpValues);
                pagebuilder.ApendCss(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.Files.MaterialString.js")))
            {
                string tmp = file.ReadToEnd();

                pagebuilder.ApendJs(tmp);
            }

            using (StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Framework.WebRender.Files.MaterialStringCallBack.js")))
            {
                string tmp = file.ReadToEnd();
               
                SubsciptionCallBackMethode = tmp;
            }

            pagebuilder.RegisterProperty(PropDescription.PropertyInfo.Name, id.ToString(), location);
            pagebuilder.RegisterSubscription(PropDescription.PropertyInfo.Name, SubsciptionCallBackMethode, location);
        }
    }
}
