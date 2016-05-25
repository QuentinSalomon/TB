using Concept.Model;
using Concept.Utils;
using Concept.Utils.Wpf;
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
            PartitionMidi = new PartitionMidi();
            FileManagement = new FileManagement();
            LoadConfiguration();
        }

        public void Unload()
        {
            SaveConfiguration();
        }

        #endregion

        #region Public Properties

        public PartitionMidi PartitionMidi { get; private set; }
        public FileManagement FileManagement { get; private set; }

        #endregion

        #region Load / Save Configuration

        public void LoadConfiguration()
        {
            var messages = new MessageCollection();
            FileManagement.LoadFromFile("FileConfig.xml", PluginClassManager.AllFactories, messages);
            if (messages.Count > 0)
                ConceptMessage.ShowError(string.Format("Error while loading the configuration file:\n{0}", messages.Text), "Loading Error");
        }
        public void SaveConfiguration()
        {
            FileManagement.SaveToFile("FileConfig.xml");
        }

        #endregion
    }
}
