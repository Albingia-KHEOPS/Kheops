using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace OP.WSAS400.DTO.OffresRapide
{
    [DataContract]
    public class OffreRapideFiltreDto
    {
        [DataMember]
        public string CodeOffre { get; set; }
        [DataMember]
        public int? Version { get; set; }
        [DataMember]
        public string TypeOffre { get; set; }
        [DataMember]
        public int? CodeAvenant { get; set; }
        [DataMember]
        public string TypeTraitement { get; set; }
        [DataMember]
        public DateTime? DateEffetAvnDebut { get; set; }
        [DataMember]
        public DateTime? DateEffetAvnFin { get; set; }
        [DataMember]
        public string CodePeriodicite { get; set; }
        [DataMember]
        public string CodeBranche { get; set; }
        [DataMember]
        public string CodeCible { get; set; } 
        [DataMember]
        public string UserCrea { get; set; } 
        [DataMember]
        public string UserMaj { get; set; }
        [DataMember]
        public string SortingBy { get; set; }
        [DataMember]
        public int LineCount { get; set; }
        [DataMember]
        public int StartLine { get; set; }
        [DataMember]
        public int EndLine { get; set; }
        [DataMember]
        public int NbCount { get; set; }
    }
}
