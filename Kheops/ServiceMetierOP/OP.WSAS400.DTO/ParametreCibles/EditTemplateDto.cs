using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.ParamTemplates;

namespace OP.WSAS400.DTO.ParametreCibles
{
    [DataContract]
    public class EditTemplateDto
    {
        [DataMember]
        public bool isReadOnly { get; set; }
        [DataMember]
        public string Cible { get; set; }
        [DataMember]
        public List<ModeleLigneTemplateDto> Templates { get; set; }
    }
}
