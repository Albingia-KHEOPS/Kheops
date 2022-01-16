using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO {
    [DataContract]
    public class FusionConnexitesDto {
        [DataMember]
        public Folder Affaire { get; set; }
        [DataMember]
        public Folder AffaireCible { get; set; }
        [DataMember]
        public TypeConnexite TypeConnexite { get; set; }
        [DataMember]
        public string Observations { get; set; }
    }
}
