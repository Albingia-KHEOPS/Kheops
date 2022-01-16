using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    [DataContract]
    public class ModeleDetailsConnexitesEngPeriodeDto
    {
        [DataMember]
        [Column(Name = "CODE")]
        public Int64 Code { get; set; }
        [DataMember]
        [Column(Name = "MODEACTIF")]
        public string Actif { get; set; }
        [DataMember]
        [Column(Name = "MODEUTILISE")]
        public string Utilise { get; set; }

        [Column(Name = "DATEDEBUT")]
        public int? DateDebutInt { get; set; }

        [Column(Name = "DATEFIN")]
        public int? DateFinInt { get; set; }
        [DataMember]
        [Column(Name = "CODEENGAGEMENT")]
        public string CodeEngagement { get; set; }
        [DataMember]
        [Column(Name = "VALEURENGAGEMENT")]
        public Int64 ValeurEngagement { get; set; }

        [DataMember]
        public DateTime? DateDebut { get; set; }
        [DataMember]
        public DateTime? DateFin { get; set; }


    }
}
