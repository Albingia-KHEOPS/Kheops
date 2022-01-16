using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Business {
    public class Observation {
        public int? Id { get; set; }
        public Folder Folder { get; set; }
        public bool IsGenerale { get; set; }
        public string Texte { get; set; }
        public string Type => IsGenerale ? "GENERALE" : string.Empty;
    }
}
