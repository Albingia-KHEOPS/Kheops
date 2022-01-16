using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ParametreClauses
{
    [DataContract]
    public class ParamRattachSaisieDto : ParamClauses_Base
    {
        [DataMember]
        public string CodeAttachService { get; set; }
        [DataMember]
        public string AttachService { get; set; }
        [DataMember]
        public string CodeAttachActeGestion { get; set; }
        [DataMember]
        public string AttachActeGestion { get; set; }
        [DataMember]
        public string CodeAttachEtape { get; set; }
        [DataMember]
        public string AttachEtape { get; set; }
        [DataMember]
        public string CodeAttachContexte { get; set; }
        [DataMember]
        public string AttachContexte { get; set; }
        [DataMember]
        public Int64 IdAttachEGDI { get; set; }
        [DataMember]
        public bool AttachEG { get; set; }
        [DataMember]
        public bool AttachDI { get; set; }
        [DataMember]
        public string CodeAttachEGDI { get; set; }
        [DataMember]
        public string ClauseNom1 { get; set; }
        [DataMember]
        public string ClauseNom2 { get; set; }
        [DataMember]
        public int ClauseNom3 { get; set; }
        [DataMember]
        public string LibelleClause { get; set; }
        [DataMember]
        public Int64 AttachOrdre { get; set; }
        [DataMember]
        public bool Obligatoire { get; set; }
        [DataMember]
        public bool Proposee { get; set; }
        [DataMember]
        public bool Suggeree { get; set; }
        [DataMember]
        public bool ImpressAnnexe { get; set; }
        [DataMember]
        public string CodeAnnexe { get; set; }
        [DataMember]
        public string StyleWord { get; set; }
        [DataMember]
        public string Taille { get; set; }
        [DataMember]
        public bool Gras { get; set; }
        [DataMember]
        public bool Souligne { get; set; }
        [DataMember]
        public int Version { get; set; }
    }
}
