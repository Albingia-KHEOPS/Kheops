using System.Runtime.Serialization;
using System.Collections.Generic;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class ModeleSousNivSaveDto : ModeleNiveauSave_Base
    {
        [DataMember]
        public override List<ModeleSousNivSaveDto> Modeles { get; set; }

        [DataMember]
        public bool IsLastAuthorizedNiveau { get; private set; }

        public ModeleSousNivSaveDto()
        {
            IsLastAuthorizedNiveau = true;
            Modeles = null;
        }

        public ModeleSousNivSaveDto(bool isLastNieau)
        {
            IsLastAuthorizedNiveau = isLastNieau;
            Modeles = new List<ModeleSousNivSaveDto>();
        }

    }
}
