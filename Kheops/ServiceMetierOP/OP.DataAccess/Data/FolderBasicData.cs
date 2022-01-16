using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess.Data {
    public class FolderBasicData : AffaireBaseData {
        private string baseOffre;

        public string BaseOffre {
            get => Typ == "O" ? string.Empty : this.baseOffre;
            set => baseOffre = value;
        }

        public string Ttr { get; set; }
        public string Eta { get; set; }
        public string Sit { get; set; }
        public string Stf { get; set; }
        public string UIn { get; set; }
        public string Asnom { get; set; }
        public int Asid { get; set; }
        public int Bur { get; set; }
        public string Budbu { get; set; }
        public string Bra { get; set; }
        public string Cible { get; set; }
        public string Ref { get; set; }
    }
}
