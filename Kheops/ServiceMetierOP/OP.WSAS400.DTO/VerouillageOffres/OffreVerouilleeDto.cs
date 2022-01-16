using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.VerouillageOffres
{
    public class OffreVerouilleeDto
    {
        [DataMember]
        [Column(Name = "KAVID")]
        public Int64 ID { get; set; }

        [DataMember]
        [Column(Name = "KAVTYP")]
        public string Type { get; set; }

        [DataMember]
        [Column(Name = "KAVIPB")]
        public string NumOffre { get; set; }


        [DataMember]
        [Column(Name = "KAVALX")]
        public Int32 Version { get; set; }

        [DataMember]
        [Column(Name = "KAVCRU")]
        public string Utilisateur { get; set; }

        [DataMember]
        [Column(Name = "KAVCRD")]
        public int DateVerouillage { get; set; }

        [DataMember]
        [Column(Name = "KAVCRH")]
        public  Int32 Heure { get; set; }

        [DataMember]
        [Column(Name = "KAVAVN")]
        public Int32 NumAvenant { get; set; }

    }
}
