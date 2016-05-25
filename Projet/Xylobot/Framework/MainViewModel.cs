using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class MainViewModel : BaseViewModel
    {
        #region Constructor

        public MainViewModel()
        {
            EditPlaylistViewModel = new EditPlaylistViewModel();
            SupervisionViewModel = new SupervisionViewModel();
            SettingsViewModel = new SettingsViewModel();
        }

        #endregion

        public EditPlaylistViewModel EditPlaylistViewModel { get; private set; }
        public SupervisionViewModel SupervisionViewModel { get; private set; }
        public SettingsViewModel SettingsViewModel { get; private set; }
    }
}
