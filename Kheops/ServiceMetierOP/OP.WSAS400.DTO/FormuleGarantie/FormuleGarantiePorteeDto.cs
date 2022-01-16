using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class FormuleGarantiePorteeDto
    {
        [DataMember]
        public bool isReadOnly { get; set; }
        [DataMember]
        public Int64 IdGarantie { get; set; }
        [DataMember]
        public Int64 SequenceGarantie { get; set; }
        [DataMember]
        public string CodeGarantie { get; set; }
        [DataMember]
        public string Garantie { get; set; }
        [DataMember]
        public string LibelleGarantie { get; set; }

        [DataMember]
        public Int64 IdPortee { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public List<ParametreDto> Actions { get; set; }

        [DataMember]
        public RisqueDto Risque { get; set; }

        [DataMember]
        public List<ParametreDto> UnitesTaux { get; set; }
        [DataMember]
        public List<ParametreDto> TypesCalTaux { get; set; }

        [DataMember]
        public bool ReportCal { get; set; }

    }
}
