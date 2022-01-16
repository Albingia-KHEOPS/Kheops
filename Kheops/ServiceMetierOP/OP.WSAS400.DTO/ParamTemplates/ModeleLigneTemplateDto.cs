using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParamTemplates
{
    public class ModeleLigneTemplateDto
    {
        [DataMember]
        [Column(Name = "ID")]
        public Int64 GuidId { get; set; }

        [DataMember]
        [Column(Name = "CODE")]
        public string CodeTemplate { get; set; }

        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string DescriptionTemplate { get; set; }

        [DataMember]
        [Column(Name = "TYPE")]
        public string TypeTemplate { get; set; }

        [DataMember]
        [Column(Name = "DATECREATION")]
        public int DateCreation { get; set; }

        [DataMember]
        [Column(Name = "USERCREATION")]
        public string UserCreation { get; set; }

        [DataMember]
        [Column(Name = "DATEMODIFICATION")]
        public int DateModification { get; set; }

        [DataMember]
        [Column(Name = "USERMODIFICATION")]
        public string UserModification { get; set; }

        [DataMember]
        [Column(Name = "CIBLEREF")]
        public Int64 CibleRef { get; set; }

        [DataMember]
        [Column(Name = "SITUATION")]
        public string Situation { get; set; }

        [Column(Name = "DEFAULTTEMPLATE")]
        public string DefaultTemplate { get; set; }
        
        [DataMember]
        public bool Default
        {
            get
            {
                return DefaultTemplate == "O";
            }
        }
    }
}
