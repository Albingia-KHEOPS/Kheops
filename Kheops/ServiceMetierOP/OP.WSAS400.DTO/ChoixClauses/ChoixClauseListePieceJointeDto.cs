using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ChoixClauses
{
    [DataContract]
    public class ChoixClauseListePieceJointeDto
    { 
        [DataMember]
        [Column(Name="PIECEID")]
        public Int64 PieceJointeId { get; set; }
        [Column(Name = "PIECEDATEAJT")]
        public Int32 PieceDateAjt { get; set; }
        [DataMember]
        [Column(Name = "PIECEACTEGES")]
        public string Acte { get; set; }
        [DataMember]
        [Column(Name = "AVENANT")]
        public Int64 Avenant { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Titre { get; set; }
        [DataMember]
        [Column(Name = "CHEMIN")]
        public string Chemin { get; set; }
        [DataMember]
        [Column(Name = "NOMPIECE")]
        public string Fichier { get; set; }
        [DataMember]
        [Column(Name = "REFPIECE")]
        public string Reference { get; set; }

        [DataMember]
        public DateTime? DateAjout { get; set; }
    }
}
