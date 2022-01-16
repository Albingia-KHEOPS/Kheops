using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.AttentatGareat
{
    [DataContract]
    public class AttentatParametreDto : _AttentatGareat_Base
    {
        [DataMember]
        public bool Standard { get; set; }
        [DataMember]
        public bool Soumis { get; set; }
        [DataMember]
        public string Tranche { get; set; }
        [DataMember]
        public string TauxCession { get; set; }
        [DataMember]
        public string FraisGestion { get; set; }
        [DataMember]
        public string Commission { get; set; }
        [DataMember]
        public string Facture { get; set; }
    }
}
