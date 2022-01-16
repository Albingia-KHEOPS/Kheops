using OP.WSAS400.DTO.Offres.Parametres;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.LTA
{
    [DataContract]
    public class LTADto
    {
        [DataMember]
        public List<ParametreDto> Durees { get; set; }

        [DataMember]
        [Column(Name = "DATEDEB")]
        public Int32? DateDeb { get; set; }
        [DataMember]
        [Column(Name = "HEUREDEB")]
        public Int16? HeureDeb { get; set; }
        [DataMember]
        [Column(Name = "DATEFIN")]
        public Int32? DateFin { get; set; }
        [DataMember]
        [Column(Name = "HEUREFIN")]
        public Int16? HeureFin { get; set; }
        [DataMember]
        [Column(Name = "DUREE")]
        public int DureeLTA { get; set; }
        [DataMember]
        [Column(Name = "UNITEDUREE")]
        public string DureeLTAString { get; set; }
        [DataMember]
        [Column(Name = "SEUILSP")]
        public Single SeuilLTA { get; set; }
    }
}
