﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class SupervisionViewModel : BaseViewModel
    {
        public Xylobot Xylobot { get { return FrameworkController.Instance.Xylobot; } }
    }
}
