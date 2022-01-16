using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Data {
    public class DbSPOptions: DbExecOptions {
        public DbSPOptions(bool isSelfConnected = false) : base(isSelfConnected) { }

        public override CommandType CommandType {
            get { return CommandType.StoredProcedure; }
            set { }
        }

        public DbSelectOptions ToSelectOptions() {
            return new DbSelectOptions(isSelfConnected) {
                DbConnection = DbConnection,
                CommandType = CommandType,
                Parameters = Parameters,
                PrivateTransaction = PrivateTransaction,
                SqlText = SqlText,
                UseDapper = UseDapper,
                AllowMissingColumnMappings = false
            };
        }
    }
}
