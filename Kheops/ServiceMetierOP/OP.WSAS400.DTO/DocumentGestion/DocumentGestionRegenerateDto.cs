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
    public class DocumentGestionRegenerateDto
    {
        [DataMember]
        [Column(Name="IDDOC")]
        public Int64 IdDocument { get; set; }
        [DataMember]
        [Column(Name = "NOMDOCUMENT")]
        public string NomDocument { get; set; }
        [DataMember]
        [Column(Name = "TYPEDOC")]
        public string TypeDocument { get; set; }
        [DataMember]
        [Column(Name = "ORIGINEDOC")]
        public string OrigineDocument { get; set; }
        [DataMember]
        [Column(Name = "LIENLIBRE")]
        public Int64 LienLibre { get; set; }
        [DataMember]
        [Column(Name = "TRANSFORME")]
        public string Transforme { get; set; }
         [DataMember]
        [Column(Name = "LIBDOC")]
        public string LibDoc { get; set; }
    }
}
