using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class SettingsViewModel : BaseViewModel
    {
        public Settings Settings { get { return FrameworkController.Instance.Settings; } }
        public Sequencer Sequencer { get { return FrameworkController.Instance.Sequencer; } }
    }
}
