using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Assures;

namespace OP.WSAS400.DTO.Adresses
{
    /// <summary>
    /// Dto de l'adresse
    /// </summary>
    [DataContract]

    public class GeoAdrDto 
    {
      
        [DataMember]
        [Column(Name = "VERSION")]
        public int Version { get; set; }

       
        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        
      
        [DataMember]
        [Column(Name = "NUMOFFRE")]
        public string NumOffre { get; set; }
        
        [DataMember]
        [Column(Name = "ADRESSE")]
        public string Adresse { get; set; }

        [DataMember]
        [Column(Name = "LNG")]
        public string Lng { get; set; }

        [DataMember]
        [Column(Name = "LAT")]
        public string Lat { get; set; }
       
        
    }
}