using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.Modeles;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.SMP
{
    [DataContract]
    public class SMPTaiteDto

    {
        [DataMember]
        [Column(Name = "KDRID")]
        public int Id { get; set; }

        [DataMember]
        [Column(Name = "KDRSMF")]
        public int SmpCptF { get; set; }

        [DataMember]
        [Column(Name = "KDRSMP")]
        public int SmpCpt { get; set; }


        public SMPTaiteDto() {
            this.Id = 0;
            this.SmpCptF = 0;
            this.SmpCpt = 0;
        }
    }
}
