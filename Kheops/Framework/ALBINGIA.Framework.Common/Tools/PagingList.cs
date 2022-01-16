using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Tools {
    public class PagingList<T> {
        public int NbTotalLines { get; set; }
        public IEnumerable<T> List { get; set; }
        public int PageNumber { get; set; }
        public IDictionary<string, decimal> Totals { get; set; }
        public bool CurrentPageIsOutOfRange => PageNumber < 1 || PageNumber > 1 && NbTotalLines > 0 && List?.Any() != true;
    }
}
