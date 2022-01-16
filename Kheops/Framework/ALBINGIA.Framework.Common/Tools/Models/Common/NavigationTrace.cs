using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALBINGIA.Framework.Common.Models.Common
{
    public class NavigationTrace
    {
        public string CurrentController { get; set; }
        public string CurrentAction { get; set; }
        public string CurrentParam { get; set; }
        public bool ReloadScreen { get; set; }

    }
}
