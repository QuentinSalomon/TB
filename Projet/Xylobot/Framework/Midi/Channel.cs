using Concept.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [IntlConceptName("Framework.Channel.Name", "Channel")]
    public class Channel : ConceptComponent
    {
        #region Constructeur

        public Channel()
        {

        }

        #endregion

        [ConceptViewVisible]
        [ConceptAutoCreate]
        [IntlConceptName("Framework.Channel.Notes", "Notes")]
        public StaticListNote Notes { get; protected set; }
    }
    
    public class StaticListChannel : ConceptStaticList<Channel>
    { }
}
