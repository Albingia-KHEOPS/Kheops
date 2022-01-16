using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class GarantieModele
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? DateAppli { get; set; }
        public DateTime? DateCreation { get; set; }
        public string Typologie { get; set; }
        public string GuidId { get; set; }
        public bool ReadOnly { get; set; }
        public string Update { get; set; }
        public List<ModeleGarantie> LstModeleGarantie { get; set; }

        public GarantieModele()
        {
            Code = String.Empty;
            Description = String.Empty;
            DateAppli = new DateTime();
            DateCreation = new DateTime();
            Typologie = String.Empty;
            GuidId = String.Empty;
            ReadOnly = false;
            Update = String.Empty;
            LstModeleGarantie = new List<ModeleGarantie>();
        }

    }

    //[Serializable]
    public class ModeleGarantie
    {
        public string Branche { get; set; }
        public string Cible { get; set; }
        public string Volet { get; set; }
        public string Bloc { get; set; }
        public string Typologie { get; set; }
    }
}
