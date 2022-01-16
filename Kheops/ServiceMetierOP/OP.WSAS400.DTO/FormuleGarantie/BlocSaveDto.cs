using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class BlocSaveDto
    {
        [DataMember]
        public bool MAJ { get; set; }
        [DataMember]
        public Int64 GuidId { get; set; }
        [DataMember]
        public bool isChecked { get; set; }
        [DataMember]
        public List<ModeleSaveDto> Modeles { get; set; }
    }
}
