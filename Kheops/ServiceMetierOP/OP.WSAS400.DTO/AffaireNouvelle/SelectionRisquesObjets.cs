using ALBINGIA.Framework.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO {
    [DataContract]
    public class SelectionRisquesObjets {
        [DataMember]
        public Folder Folder { get; set; }
        [DataMember]
        public string CodeAffaireNouvelle { get; set; }
        [DataMember]
        public List<SelectionRisqueObjets> Risques { get; set; }
        [DataMember]
        public List<SelectionFormuleOption> AvailableOptions { get; set; }
    }
}