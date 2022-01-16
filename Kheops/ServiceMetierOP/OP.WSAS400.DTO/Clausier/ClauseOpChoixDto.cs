using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Clausier
{
    [DataContract]
    public class ClauseOpChoixDto
    {
        [DataMember]
        public string IdLot { get; set; } // Exemple CG: Remplace la noption du Key du dictionnaire
        [DataMember]
        public string Origine { get; set; } // KPCLAUSE ou HPCLAUSE
        [DataMember]
        public long Idunique { get; set; } // KCAID. 
        [DataMember]
        public string Rub { get; set; }
        [DataMember]
        public string SRub { get; set; }
        [DataMember]
        public int Seq { get; set; }
        [DataMember]
        public int Version { get; set; }
        [DataMember]
        public string Retenue { get; set; } // O/N
        [DataMember]
        public int Avenant { get; set; }

        [DataMember]
        public string OrigineClause { get; set; } // Offre ou Police. Offre si on est en affaire nouvelle issue d'une offre et que la CG de l'offre <> CG générée
        [DataMember]
        public long IdClause { get; set; }

        [DataMember]
        public string Libclause { get; set; }
    }

}
