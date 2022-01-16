using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Data {
    public class DbSelectOptions: DbStatementOptions {
        public DbSelectOptions(bool isSelfConnected = false) : base(isSelfConnected) {
            AllowMissingColumnMappings = false;
            UseDapper = true;
        }
        public bool AllowMissingColumnMappings { get; set; }
    }
}
