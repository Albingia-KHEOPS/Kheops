using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Historique
{
    [DataContract]
    public class HistoriqueLigneDto
    {
        [DataMember]
        [Column(Name = "NUMINTERNE")]
        public Int32 NumInterne { get; set; }
        [DataMember]
        [Column(Name = "NUMEXTERNE")]
        public Int32 NumExterne { get; set; }
        [DataMember]
        [Column(Name = "REFERENCE")]
        public string Reference { get; set; }
        [DataMember]
        [Column(Name = "DESIGNATION")]
        public string Designation { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFET")]
        public Int64 DateEffetInt { get; set; }
        [DataMember]
        [Column(Name = "DATECREATION")]
        public Int64 DateCreationInt { get; set; }
        [DataMember]
        [Column(Name = "MOTIF")]
        public string Motif { get; set; }
        [DataMember]
        [Column(Name = "LIBMOTIF")]
        public string LibMotif { get; set; }
        [DataMember]
        [Column(Name = "ETAT")]
        public string Etat { get; set; }
        [DataMember]
        [Column(Name = "LIBETAT")]
        public string LibEtat { get; set; }
        [DataMember]
        [Column(Name = "SITUATION")]
        public string Situation { get; set; }
        [DataMember]
        [Column(Name = "LIBSITUATION")]
        public string LibSituation { get; set; }
        [DataMember]
        [Column(Name = "QUALITE")]
        public string Qualite { get; set; }
        [DataMember]
        [Column(Name = "LIBQUALITE")]
        public string LibQualite { get; set; }
        [DataMember]
        [Column(Name = "TYPERETOUR")]
        public string TypeRetour { get; set; }
        [DataMember]
        [Column(Name = "LIBRETOUR")]
        public string LibRetour { get; set; }
        [DataMember]
        [Column(Name = "DATERETOUR")]
        public Int64 DateRetourInt { get; set; }
        [DataMember]
        [Column(Name = "TRAITEMENT")]
        public string Traitement { get; set; }
        [DataMember]
        [Column(Name = "LIBTRAITEMENT")]
        public string LibTraitement { get; set; }
        [DataMember]
        public string ReguleId { get; set; }
      
    
    

    }
}
