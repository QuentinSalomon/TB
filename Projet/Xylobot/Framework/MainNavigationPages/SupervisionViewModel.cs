namespace Framework
{
    public class SupervisionViewModel : BaseViewModel
    {
        public Sequencer Sequencer { get { return FrameworkController.Instance.Sequencer; } }
    }
}
