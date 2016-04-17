using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class EditPlaylistViewModel : BaseViewModel
    {
        public Playlist Playlist { get { return FrameworkController.Instance.Playlist; } }
    }
}
