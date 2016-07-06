using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//todo : Delete fichier
namespace Framework
{
    public class UserControlShowPartitionModel :  INotifyPropertyChanged
    {
        public Sequencer Sequencer { get { return FrameworkController.Instance.Sequencer; } }
        public event PropertyChangedEventHandler PropertyChanged;

        
        public void DoPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
