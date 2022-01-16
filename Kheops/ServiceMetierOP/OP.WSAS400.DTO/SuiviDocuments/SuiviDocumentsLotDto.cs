using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.SuiviDocuments
{
    [DataContract]
    public class SuiviDocumentsLotDto
    {
        [DataMember]
        public Int64 LotId { get; set; }
        [DataMember]
        public string LotLibelle { get; set; }
        [DataMember]
        public string LotUser { get; set; }
        [DataMember]
        public string NomUser { get; set; }
        [DataMember]
        public string PrenomUser { get; set; }
        [DataMember]
        public string UniteService { get; set; }
        [DataMember]
        public string TypeAffaire { get; set; }
        [DataMember]
        public string CodeOffre { get; set; }
        [DataMember]
        public Int32 Version { get; set; }
        [DataMember]
        public string ActeGestion { get; set; }
        [DataMember]
        public string ActeGestionLib { get; set; }
        [DataMember]
        public Int32 NumInterne { get; set; }
        [DataMember]
        public Int64 NumExterne { get; set; }

        [DataMember]
        public List<SuiviDocumentsLotDetailsDto> SuiviDocumentsListLotDetail { get; set; }
    }
}
