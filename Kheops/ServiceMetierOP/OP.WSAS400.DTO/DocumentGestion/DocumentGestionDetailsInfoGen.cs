using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.DocumentGestion
{
    [DataContract]
    public class DocumentGestionDetailsInfoGen
    {
        [DataMember]
        [Column(Name = "TYPEDOCUMENT")]
        public string TypeDocument { get; set; }

        [DataMember]
        [Column(Name = "NBEXEMPLAIRE")]
        public int NombreExemplaire { get; set; }

        [DataMember]
        [Column(Name="LOTID")]
        public Int64 LotId { get; set; }

        [DataMember]
        [Column(Name = "COURRIERID")]
        public Int64 CourrierId { get; set; }
        [DataMember]
        [Column(Name = "COURRIERCODE")]
        public string CourrierCode { get; set; }
        [DataMember]
        [Column(Name = "COURRIERLIB")]
        public string CourrierLib { get; set; }

        [DataMember]
        [Column(Name = "DOCLIBRE")]
        public string DocLibre { get; set; }
        [DataMember]
        [Column(Name = "DOCGENER")]
        public string DocGener { get; set; }
        

    }
}
