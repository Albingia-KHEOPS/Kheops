using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class FormGarDto
    {
        [DataMember]
        public FormuleGarantieDto FormuleGarantie { get; set; }
        [DataMember]
        public FormuleGarantieSaveDto FormuleGarantieSave { get; set; }
        [DataMember]
        public FormuleGarantieDto FormuleGarantieHisto { get; set; }
    }
}
