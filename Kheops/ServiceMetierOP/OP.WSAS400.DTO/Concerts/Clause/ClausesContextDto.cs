using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Clause
{
      [DataContract]
    public class ClausesContextDto
    {
        [DataMember]
        [Column(Name = "ORIGINE")]
        public string Origine { get; set; }
        [DataMember]
        [Column(Name = "ETATTITRE")]
        public string EtatTitre { get; set; }
        [DataMember]
        [Column(Name = "ETAPE")]
        public string Etape { get; set; }
        [DataMember]
        [Column(Name = "CONTEXTE")]
        public string Contexte { get; set; }
        [DataMember]
        [Column(Name = "CTXLBL")]
        public string ContexteLabel { get; set; }
        [DataMember]
        [Column(Name = "ORIGINEFILTRE")]
        public string OrigineFiltre { get; set; }

    }
}
