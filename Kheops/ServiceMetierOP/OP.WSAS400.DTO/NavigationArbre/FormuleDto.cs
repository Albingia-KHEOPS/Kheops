using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.NavigationArbre
{
    [DataContract]
    public class FormuleDto
    {
        [DataMember]
        public string Alpha;

        [DataMember]
        public int CodeFormule { get; set; }

        [DataMember]
        public int CodeOption { get; set; }

        [DataMember]
        public OptionDto Option;

        [DataMember]
        public List<OptionDto> Options;

        [DataMember]
        public string TagFormule { get; set; }

        [DataMember]
        public bool CreateModifAvn { get; set; }

        [DataMember]
        public bool isBadDate { get; set; }

        public FormuleDto()
        {
            Alpha = string.Empty;
            Option = new OptionDto();
            Options = new List<OptionDto>();
        }
    }
}
