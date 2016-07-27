namespace Framework
{
    public class EditPlaylistViewModel : BaseViewModel
    {
        public Playlist Playlist { get { return FrameworkController.Instance.Playlist; } }
        //public Sequencer Sequencer { get { return FrameworkController.Instance.Sequencer; } }
    }
}
