using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class SupervisionViewModel : BaseViewModel
    {
        public Sequencer Sequencer { get { return FrameworkController.Instance.Sequencer; } }
    }
}
