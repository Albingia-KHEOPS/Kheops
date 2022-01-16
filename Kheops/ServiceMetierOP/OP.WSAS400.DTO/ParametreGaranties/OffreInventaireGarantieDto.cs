using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreGaranties
{
    public class OffreInventaireGarantieDto
    {
        [DataMember]
        [Column(Name = "CODEOFFRE")]
        public string CodeOffre { get; set; }
        [DataMember]
        [Column(Name = "VERSION")]
        public Int64 Version { get; set; }
        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        [DataMember]
        [Column(Name = "CODEINVENTAIRE")]
        public Int64 CodeInventaire { get; set; }
        [DataMember]
        [Column(Name = "CODEFORMULE")]
        public Int64 CodeFormule { get; set; }
        [DataMember]
        [Column(Name = "CODEGARANTIE")]
        public string CodeGarantie { get; set; }
        [DataMember]
        [Column(Name = "LETTREFORMULE")]
        public string LettreFormule { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEFORMULE")]
        public string LibelleFormule { get; set; }
    }
}
