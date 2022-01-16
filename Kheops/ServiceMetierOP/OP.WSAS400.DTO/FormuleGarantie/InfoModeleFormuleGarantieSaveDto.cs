using System.Runtime.Serialization;
using System.Collections.Generic;
using OP.WSAS400.DTO.FormuleGarantie.GarantieModele;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class InfoModeleFormuleGarantieSaveDto : InfoFormuleGarantieSave_Base
    {
        [DataMember]
        public int IdBloc { get; set; }
        [DataMember]
        public long IdGar { get; set; }
        [DataMember]
        public List<GarantiesDto> ParamsGaranties { get; set; }
        [DataMember]
        public KpgartarDto GarantieTarif { get; set; }
        [DataMember]
        public string CodeAvenant { get; set; }

        public InfoModeleFormuleGarantieSaveDto() { }

        public InfoModeleFormuleGarantieSaveDto(InfoFormuleGarantieSave_Base infoBase) {
            CodeOffre = infoBase.CodeOffre;
            Version = infoBase.Version;
            Type = infoBase.Type;
            CodeFormule = infoBase.CodeFormule;
            CodeOption = infoBase.CodeOption;
            User = infoBase.User;
            DateNow = infoBase.DateNow;
            IdBloc = infoBase.Id;
        }
    }
}
