using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    [DataContract]
    public class ConnexiteDto
    {
        [DataMember]
        public Int64 NumConnexite { get; set; }

        [DataMember]
        public string CodeAffaire1 { get; set; }
        [DataMember]
        public Int32 Version1 { get; set; }
        [DataMember]
        public string Type1 { get; set; }
        [DataMember]
        public string CodeBranche1 { get; set; }
        [DataMember]
        public string LibBranche1 { get; set; }
        [DataMember]
        public string CodeCible1 { get; set; }
        [DataMember]
        public string LibCible1 { get; set; }
        [DataMember]
        public Int32 CodeAss1 { get; set; }
        [DataMember]
        public string NomAss1 { get; set; }
        [DataMember]
        public string Ad1_1 { get; set; }
        [DataMember]
        public string Ad2_1 { get; set; }
        [DataMember]
        public string Dep1 { get; set; }
        [DataMember]
        public string Cp1 { get; set; }
        [DataMember]
        public string Ville1 { get; set; }

        [DataMember]
        public string CodeAffaire2 { get; set; }
        [DataMember]
        public Int32 Version2 { get; set; }
        [DataMember]
        public string Type2 { get; set; }
        [DataMember]
        public string CodeBranche2 { get; set; }
        [DataMember]
        public string LibBranche2 { get; set; }
        [DataMember]
        public string CodeCible2 { get; set; }
        [DataMember]
        public string LibCible2 { get; set; }
        [DataMember]
        public Int32 CodeAss2 { get; set; }
        [DataMember]
        public string NomAss2 { get; set; }
        [DataMember]
        public string Ad1_2 { get; set; }
        [DataMember]
        public string Ad2_2 { get; set; }
        [DataMember]
        public string Dep2 { get; set; }
        [DataMember]
        public string Cp2 { get; set; }
        [DataMember]
        public string Ville2 { get; set; }

        [DataMember]
        public Int64 CodeObsv { get; set; }
        [DataMember]
        public string Observations { get; set; }
    }
}
