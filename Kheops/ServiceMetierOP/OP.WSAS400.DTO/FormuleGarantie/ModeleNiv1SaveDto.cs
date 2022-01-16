using System.Collections.Generic;
using System.Runtime.Serialization;


namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class ModeleNiv1SaveDto : ModeleNiveauSave_Base
    {
        [DataMember]
        public override List<ModeleSousNivSaveDto> Modeles { get; set; }
        
        [DataMember]
        public string Action { get; set; }
    }
}
