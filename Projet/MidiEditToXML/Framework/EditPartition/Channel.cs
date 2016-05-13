using Concept.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [IntlConceptName("Framework.Channel.Name", "Channels")]
    [ConceptSmallImage(typeof(Channel), "/Images/Channelt32x32.png")]
    [ConceptLargeImage(typeof(Channel), "/Images/Channel64x64.png")]
    public class Channel : ConceptComponent
    {
        #region Constructeur

        public Channel()
        {
            Notes = new StaticListNote();
        }

        #endregion

        [ConceptAutoCreate]
        [IntlConceptName("Framework.Partition.Notes", "Notes")]
        public StaticListNote Notes { get; protected set; }
    }

    [ConceptSmallImage(typeof(StaticListChannel), "/Images/Channel32x32.png")]
    [ConceptLargeImage(typeof(StaticListChannel), "/Images/Channel64x64.png")]
    public class StaticListChannel : ConceptStaticList<Channel>
    { }
}
