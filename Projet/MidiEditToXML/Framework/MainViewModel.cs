using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class MainViewModel : BaseViewModel
    {
        #region Constructor

        public MainViewModel()
        {
            EditPartitionViewModel = new EditPartitionViewModel();
        }

        #endregion

        public EditPartitionViewModel EditPartitionViewModel { get; private set; }
    }
}
