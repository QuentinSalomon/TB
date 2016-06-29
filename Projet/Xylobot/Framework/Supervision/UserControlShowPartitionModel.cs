using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class UserControlShowPartitionModel : INotifyPropertyChanged
    {
        public PartitionXylo Partition { get { return FrameworkController.Instance.Sequencer.CurrentPartition; } }


        public event PropertyChangedEventHandler PropertyChanged;

        public void DoPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
