namespace Framework
{
    public class CurrentPlaylistViewModel : BaseViewModel
    {
        public Playlist Playlist { get { return FrameworkController.Instance.Playlist; } }
        public Sequencer Sequencer { get { return FrameworkController.Instance.Sequencer; } }
    }
}
