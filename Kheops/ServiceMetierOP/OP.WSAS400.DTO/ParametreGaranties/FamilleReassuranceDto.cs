using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreGaranties
{
    public class FamilleReassuranceDto
    {

        [DataMember]
        [Column(Name = "CODEBRANCHE")]
        public string CodeBranche { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEBRANCHE")]
        public string LibelleBranche { get; set; }

        [DataMember]
        [Column(Name = "CODESOUSBRANCHE")]
        public string CodeSousBranche { get; set; }
        [DataMember]
        [Column(Name = "LIBELLESOUSBRANCHE")]
        public string LibelleSousBranche { get; set; }

        [DataMember]
        [Column(Name = "CODECATEGORIE")]
        public string CodeCategorie { get; set; }
        [DataMember]
        [Column(Name = "LIBELLECATEGORIE")]
        public string LibelleCategorie { get; set; }

        [DataMember]
        [Column(Name = "CODEFAMILLE")]
        public string CodeFamille { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEFAMILLE")]
        public string LibelleFamille { get; set; }

        [DataMember]
        [Column(Name = "CODEGARANTIE")]
        public string CodeGarantie { get; set; }

    }
}
