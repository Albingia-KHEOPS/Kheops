using System;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO {
    [DataContract]
    public class SelectionObjet {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Selected { get; set; }
        [DataMember]
        public DateTime? DateDebut { get; set; }
        [DataMember]
        public DateTime? DateFin { get; set; }
        [DataMember]
        public decimal Valeur { get; set; }
        [DataMember]
        public string UniteValeur { get; set; }
        [DataMember]
        public string TypeValeur { get; set; }
    }
}