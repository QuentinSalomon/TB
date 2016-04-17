using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public sealed class FrameworkController
    {
        #region Singleton

        private FrameworkController()
        { }

        public static FrameworkController Instance
        {
            get { return _instance; }
        }
        private static readonly FrameworkController _instance = new FrameworkController();

        #endregion

        #region Load / Unload

        public void Load()
        {
            Playlist = new Playlist() { Description = "My Machine Description" };
        }

        public void Unload()
        {

        }

        #endregion

        #region Public Properties

        public Playlist Playlist { get; private set; }

        #endregion
    }
}
