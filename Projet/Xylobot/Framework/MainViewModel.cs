namespace Framework
{
    public class MainViewModel : BaseViewModel
    {
        #region Constructor

        public MainViewModel()
        {
            CurrentPlaylistViewModel = new CurrentPlaylistViewModel();
            EditPlaylistViewModel = new EditPlaylistViewModel();
            SupervisionViewModel = new SupervisionViewModel();
            SettingsViewModel = new SettingsViewModel();
        }

        #endregion

        public CurrentPlaylistViewModel CurrentPlaylistViewModel { get; set; }
        public EditPlaylistViewModel EditPlaylistViewModel { get; private set; }
        public SupervisionViewModel SupervisionViewModel { get; private set; }
        public SettingsViewModel SettingsViewModel { get; private set; }
    }
}
