using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace OP.WSAS400.DTO.Engagement
{
    public class PeriodeConnexiteDto
    {
        [DataMember]
        public long CodeEngagement { get; set; }
        [DataMember]
        public DateTime? DateDebut { get; set; }
        [DataMember]
        public DateTime? DateFin { get; set; }
        [DataMember]
        public string CodeOffre { get; set; }
        [DataMember]
        public int Version { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public bool IsInactive { get; set; }
        [DataMember]
        public bool IsUsed { get; set; }
        [DataMember]
        public string User { get; set; }
        [DataMember]
        public Dictionary<string, long> Traites { get; set; }
        [DataMember]
        public bool IsEnCours { get; set; }
        [DataMember]
        public int Ordre { get; set; }
    }
}
