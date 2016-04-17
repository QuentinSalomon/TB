using Concept.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [IntlConceptName("Framework.Playlist.Name", "Playlist principal")]
    [ConceptSmallImage(typeof(Playlist), "/Images/Playlist32x32.png")]
    [ConceptLargeImage(typeof(Playlist), "/Images/Playlist64x64.png")]
    public class Playlist : ConceptComponent
    {
        #region Properties

        [ConceptViewVisible]
        [IntlConceptName("Framework.Playlist.Description", "Description")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    DoPropertyChanged(DescriptionPropertyName);
                }
            }
        }
        private string _description;
        public const string DescriptionPropertyName = "Description";

        #endregion
    }
}
