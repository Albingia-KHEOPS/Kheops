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
    public class EngagementPeriodeDto
    {
        [DataMember]
        [Column(Name = "CODE")]
        public Int64 Code { get; set; }
        [DataMember]
        [Column(Name = "MODEACTIF")]
        public string Actif { get; set; }
        //Dates
        [DataMember]
        [Column(Name = "PART")]
        public Single Part { get; set; }
        [DataMember]
        [Column(Name = "ENGAGTOTAL")]
        public Int64 EngagementTotal { get; set; }
        [DataMember]
        [Column(Name = "ENGAGALBINGIA")]
        public Int64 EngagementAlbingia { get; set; }
        [DataMember]
        [Column(Name = "MODEUTILISE")]
        public string Utilise { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBUT")]
        public int DateDebut { get; set; }
        [DataMember]
        [Column(Name = "DATEFIN")]
        public int DateFin { get; set; }
        [DataMember]
        [Column(Name = "KDOECO")]
        public string EnregistrementEnCours { get; set; }
        [DataMember]
        [Column(Name = "INHPENG")]
        public String INHPENG { get; set; }
        [DataMember]
        
        public string Action { get; set; }
    }
}
