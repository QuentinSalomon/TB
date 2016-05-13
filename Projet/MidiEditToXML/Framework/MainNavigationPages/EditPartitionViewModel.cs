using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class EditPartitionViewModel : BaseViewModel
    {
        public PartitionMidi PartitionMidi { get { return FrameworkController.Instance.PartitionMidi; } }
    }
}
