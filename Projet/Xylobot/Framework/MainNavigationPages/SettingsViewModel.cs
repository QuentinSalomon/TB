namespace Framework
{
    public class SettingsViewModel : BaseViewModel
    {
        public Settings Settings { get { return FrameworkController.Instance.Settings; } }
        public Sequencer Sequencer { get { return FrameworkController.Instance.Sequencer; } }
    }
}
