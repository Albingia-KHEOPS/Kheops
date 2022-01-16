using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.ChoixClauses
{
    [DataContract]
    public class ChoixClausePieceJointeDto
    {
        [DataMember]
        public string Contexte { get; set; }
        [DataMember]
        public string Emplacement { get; set; }
        [DataMember]
        public string SousEmplacement { get; set; }
        [DataMember]
        public Int64 Ordre { get; set; }
        [DataMember]
        public string AppliqueA { get; set; }

        [DataMember]
        public Int32 CodeRisque { get; set; }
        [DataMember]
        public string Risque { get; set; }
        [DataMember]
        public Int32 CodeObjet { get; set; }
        [DataMember]
        public RisqueDto ObjetsRisqueAll { get; set; }

        [DataMember]
        public List<ChoixClauseListePieceJointeDto> PiecesJointes { get; set; }

        [DataMember]
        public bool Modifiable { get; set; }
    }
}
