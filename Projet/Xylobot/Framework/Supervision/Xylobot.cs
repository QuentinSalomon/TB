using Concept.Model;
using Concept.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [IntlConceptName("Framework.Xylobot.Name", "Xylobot")]
    [ConceptSmallImage(typeof(Playlist), "/Images/Xylophone32x32.png")]
    [ConceptLargeImage(typeof(Playlist), "/Images/Xylophone64x64.png")]
    public class Xylobot : ConceptComponent
    {
        [ConceptViewVisible(false)]
        public SerialUsb SerialUsb { get; set; }
    }
}
