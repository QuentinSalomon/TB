using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class FileManagementViewModel : BaseViewModel
    {
        public FileManagement FileManagement { get { return FrameworkController.Instance.FileManagement; } }
    }
}
