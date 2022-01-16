using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Data {
    public class DbExecOptions: DbStatementOptions {
        public DbExecOptions(bool isSelfConnected = false) : base(isSelfConnected) {
            ReturnedValue = 0;
        }

        public int ReturnedValue { get; set; }
    }
}
