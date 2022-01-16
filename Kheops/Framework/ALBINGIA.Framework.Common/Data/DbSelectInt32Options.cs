using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Data {
    public class DbSelectInt32Options: DbSelectOptions {
        private IEnumerable<int> integerList;
        public DbSelectInt32Options(bool isSelfConnected = false) : base(isSelfConnected) {
            integerList = null;
        }
        public IEnumerable<int> IntegerList {
            get { return integerList; }
            set {
                if (integerList == null) {
                    integerList = value;
                }
            }
        }
    }
}
