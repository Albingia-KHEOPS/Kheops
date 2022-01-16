using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class CoAssureurDto
    {
        [DataMember]
        [Column(Name = "GUIDID")]
        public string GuidId { get; set; }
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "NOM")]
        public string Nom { get; set; }
        [DataMember]
        [Column(Name = "CODEPOSTAL")]
        public string CodePostal { get; set; }
        [DataMember]
        [Column(Name = "VILLE")]
        public string Ville { get; set; }
        [DataMember]
        [Column(Name = "POURCENTPART")]
        public Single PourcentPart { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBUT")]
        public string DateDebutDB { get; set; }
        [DataMember]
        [Column(Name = "DATEFIN")]
        public string DateFinDB { get; set; }
        [DataMember]
        [Column(Name = "INTERLOCUTEUR")]
        public string Interlocuteur { get; set; }
        [DataMember]
        [Column(Name = "IDINTERLOCUTEUR")]
        public int IdInterlocuteur { get; set; }
        [DataMember]
        [Column(Name = "REFERENCE")]
        public string Reference { get; set; }
        [DataMember]
        [Column(Name = "FRAISACC")]
        public Single FraisAcc { get; set; }
        [DataMember]
        [Column(Name = "COMMISSIONAPE")]
        public Single CommissionAperiteur { get; set; }
        [Column(Name="FRAISAPEALB")]
        [DataMember]
        public Single FraisApeAlb { get; set; }
        [DataMember]
        public DateTime? DateDebut { get; set; }
        [DataMember]
        public DateTime? DateFin { get; set; }
        [DataMember]
        public string TypeOperation { get; set; }
    }
}
