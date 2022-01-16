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
    public class DocumentGestionInfoDestDto
    {
        [DataMember]
        [Column(Name = "RAISONSOCIALE")]
        public string RaisonSociale { get; set; }
        [DataMember]
        [Column(Name = "NOMINTER")]
        public string NomInter { get; set; }
        [DataMember]
        [Column(Name = "PRENOMINTER")]
        public string PrenomInter { get; set; }
        [DataMember]
        [Column(Name = "EMAILINTER")]
        public string EmailInter { get; set; }
        [DataMember]
        [Column(Name = "BATIMENT")]
        public string Batiment { get; set; }
        [DataMember]
        [Column(Name = "NUMEROVOIE")]
        public Int64 NumeroVoie { get; set; }
        [DataMember]
        [Column(Name = "EXTENSIONVOIE")]
        public string ExtensionVoie { get; set; }
        [DataMember]
        [Column(Name = "NOMVOIE")]
        public string NomVoie { get; set; }
        [Column(Name = "DEPARTEMENT")]
        public string Departement { get; set; }
        [Column(Name = "CODEPOSTAL")]
        public Int64 CP { get; set; }
        [DataMember]
        [Column(Name = "NOMVILLE")]
        public string NomVille { get; set; }
        [DataMember]
        public string CodePostal { get; set; }
    }
}
