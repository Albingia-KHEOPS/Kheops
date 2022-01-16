using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Data {
    public class DbCountOptions: DbSelectOptions {
        public DbCountOptions(bool isSelfConnected = false) : base(isSelfConnected) {
            Count = 0;
            CommandType = CommandType.Text;
        }

        public int Count { get; set; }
    }
}
