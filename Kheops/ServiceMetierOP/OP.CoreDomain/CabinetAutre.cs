using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    public class CabinetAutre
    {
        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Courtier { get; set; }
        public string Delegation { get; set; }
        public DateTime? EnregistrementDate { get; set; }
        public TimeSpan? EnregistrementHeure { get; set; }
        public DateTime? SaisieDate { get; set; }
        public TimeSpan? SaisieHeure { get; set; }
        public string Souscripteur { get; set; }
        public string CodeSouscripteur { get; set; }
        public string Motif { get; set; }
        public string LibelleMotif { get; set; }
        public string MotifRefusGestionnaire { get; set; }
        public string MotifRefusDemandeur { get; set; }
    }
}
