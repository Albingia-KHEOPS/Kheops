using System.Runtime.Serialization;

namespace OP.WSAS400.DTO {
    [DataContract]
    public class SelectionFormuleOption {
        [DataMember]
        public int NumFormule { get; set; }
        [DataMember]
        public string NomFormule { get; set; }
        [DataMember]
        public int NumOption { get; set; }
        [DataMember]
        public bool Selected { get; set; }
    }
}