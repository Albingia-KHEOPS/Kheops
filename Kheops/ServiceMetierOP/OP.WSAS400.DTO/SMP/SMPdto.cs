using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.SMP
{
    [DataContract]
    public class SMPdto : SMP_Base
    {
        [DataMember]
        [Column(Name = "NomTraite")]
        public string NomTraite { get; set; }
        
        [DataMember]
        [Column(Name = "Risque")]
        public int Risque { get; set; }

        [DataMember]
        [Column(Name = "NOMVENTILATION")]
        public string Ventilation { get; set; }

        [DataMember]
        public List<LigneSMPdto> ListeGarantie { get; set; }
        [DataMember]
        public List<ParametreDto> Types { get; set; }

        [DataMember]
        [Column(Name = "SMPtotal")]
        public Int64 SMPtotal { get; set; }
    }
}
