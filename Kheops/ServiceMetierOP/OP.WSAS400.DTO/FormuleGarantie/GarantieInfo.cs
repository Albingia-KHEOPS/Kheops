using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class GarantieInfo
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        [Column(Name = "CODEFORMULE")]
        public int CodeFormule { get; set; }

        [DataMember]
        [Column(Name = "CODEOPTION")]
        public int CodeOption { get; set; }

        [DataMember]
        [Column(Name = "CODEGARANTIE")]
        public string CodeGarantie { get; set; }

        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }

    }
}
