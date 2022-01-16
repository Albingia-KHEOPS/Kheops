using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    public class UtilisateurKheops {
        public string Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public FamilleUtilisateur Famille { get; set; }
    }
}
