using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Albingia.Kheops.OP.DataAdapter
{
    internal class FalseSuccesIndicator : ISuccessIndicator
    {
        public bool ShouldCommit { get { return false; } set { } }
    }
}
