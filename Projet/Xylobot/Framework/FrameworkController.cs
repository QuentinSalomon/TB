using Concept.Model;
using Concept.Utils;
using Concept.Utils.Wpf;

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
            Playlist = new Playlist() { Title = "Liste de lecture" };
            Settings = new Settings();
            Sequencer = new Sequencer();
            Sequencer.Playlist = Playlist;

            WebServer = new VirtuosoWebServer(Sequencer, Playlist);

            LoadConfiguration();
        }

        public void Unload()
        {
            SaveConfiguration();
            WebServer.Close();
            Sequencer.Finish();
        }

        #endregion

        #region Public Properties

        public Playlist Playlist { get; private set; }
        public Settings Settings { get; private set; }
        public Sequencer Sequencer { get; private set; }
        public VirtuosoWebServer WebServer { get; private set; }

        #endregion

        #region Load / Save Configuration

        public void LoadConfiguration()
        {
            var messages = new MessageCollection();
            Settings.LoadFromFile("FileConfig.xml", PluginClassManager.AllFactories, messages);
            if (messages.Count > 0)
                ConceptMessage.ShowError(string.Format("Error while loading the configuration file:\n{0}", messages.Text), "Loading Error");
        }
        public void SaveConfiguration()
        {
            Settings.SaveToFile("FileConfig.xml");
        }

        #endregion
    }
}
