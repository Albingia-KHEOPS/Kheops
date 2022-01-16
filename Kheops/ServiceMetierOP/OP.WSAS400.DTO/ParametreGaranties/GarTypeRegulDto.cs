using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ParametreGaranties
{

    [DataContract]
    public class GarTypeRegulDto
    {
        [DataMember]
        [Column(Name= "CODEGARANTIE")]
        public string CodeGarantie { get; set; }

        [DataMember]
        [Column(Name="CODETYPEREGUL")]
        public string CodeTypeRegul { get; set; }

        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }

        [DataMember]
        [Column(Name = "GRILLE")]
        public string Grille { get; set; }

       
    }

}
