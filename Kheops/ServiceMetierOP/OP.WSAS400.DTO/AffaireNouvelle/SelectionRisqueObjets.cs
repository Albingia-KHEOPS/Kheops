using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO {
    [DataContract]
    public class SelectionRisqueObjets {
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
        public string CodeUniteValeur { get; set; }
        [DataMember]
        public string CodeTypeValeur { get; set; }
        [DataMember]
        public List<SelectionObjet> Objets { get; set; }
    }
}