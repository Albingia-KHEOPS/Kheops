using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Data {
    public class DbSelectStringsOptions: DbSelectOptions {
        private IEnumerable<string> stringList;
        public DbSelectStringsOptions(bool isSelfConnected = false) : base(isSelfConnected) {
            stringList = null;
        }
        public IEnumerable<string> StringList {
            get { return stringList; }
            set {
                if (stringList == null) {
                    stringList = value;
                }
            }
        }
    }
}
