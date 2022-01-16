using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public abstract class ModeleNiveauSave_Base
    {
        [DataMember]
        public bool MAJ { get; set; }
        [DataMember]
        public string GuidGarantie { get; set; }
        [DataMember]
        public string Caractere { get; set; }
        [DataMember]
        public string Nature { get; set; }
        [DataMember]
        public string NatureParam { get; set; }
        [DataMember]
        public string ParamNatMod { get; set; }

        [DataMember]
        public virtual List<ModeleSousNivSaveDto> Modeles { get; set; }

    }
}
