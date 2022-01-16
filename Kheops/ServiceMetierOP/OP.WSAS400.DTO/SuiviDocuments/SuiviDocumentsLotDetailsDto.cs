using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.SuiviDocuments
{
    [DataContract]
    public class SuiviDocumentsLotDetailsDto
    {
        [DataMember]
        public Int64 LotDetailId { get; set; }
        [DataMember]
        public string CodeSituation { get; set; }
        [DataMember]
        public Int32 DateSituation { get; set; }
        [DataMember]
        public Int32 HeureSituation { get; set; }
        [DataMember]
        public string UserSituation { get; set; }
        [DataMember]
        public string TypeDestinataire { get; set; }
        [DataMember]
        public string TypeIntervenant { get; set; }
        [DataMember]
        public Int32 CodeDestinataire { get; set; }
        [DataMember]
        public string NomDestinataire { get; set; }
        [DataMember]
        public Int32 CodeInterlocuteur { get; set; }
        [DataMember]
        public string NomInterlocuteur { get; set; }
        [DataMember]
        public string CodeDiffusion { get; set; }
        [DataMember]
        public string LibDiffusion { get; set; }

        [DataMember]
        public List<SuiviDocumentsDocDto> SuiviDocumentsListDoc { get; set; }
    }
}
