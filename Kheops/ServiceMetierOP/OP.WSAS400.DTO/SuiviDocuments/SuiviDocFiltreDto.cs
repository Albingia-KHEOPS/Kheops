using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.SuiviDocuments
{
    [DataContract]
    public class SuiviDocFiltreDto
    {
        [DataMember]
        public string CodeOffre { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string User { get; set; }
        [DataMember]
        public Int32 Avenant { get; set; }
        [DataMember]
        public Int32 NumLot { get; set; }
        [DataMember]
        public string Situation { get; set; }
        [DataMember]
        public Int32 DateDebSituation { get; set; }
        [DataMember]
        public Int32 DateFinSituation { get; set; }
        [DataMember]
        public string UniteService { get; set; }
        [DataMember]
        public Int32 DateDebEdition { get; set; }
        [DataMember]
        public Int32 DateFinEdition { get; set; }
        [DataMember]
        public string TypeDestinataire { get; set; }
        [DataMember]
        public Int32 CodeDestinataire { get; set; }
        [DataMember]
        public string CodeInterlocuteur { get; set; }
        [DataMember]
        public string TypeDoc { get; set; }
        [DataMember]
        public string CourrierType { get; set; }
        [DataMember]
        public string Warning { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public int StartLine { get; set; }
        [DataMember]
        public int EndLine { get; set; }
    }
}
