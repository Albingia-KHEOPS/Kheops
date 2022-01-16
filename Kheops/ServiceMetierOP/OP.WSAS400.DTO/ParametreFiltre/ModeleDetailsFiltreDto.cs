using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreFiltre
{
    public class ModeleDetailsFiltreDto
    {
        [DataMember]
        [Column(Name = "ID")]
        public Int64 GuidIdFiltre { get; set; }

        [DataMember]
        [Column(Name = "CODEFILTRE")]
        public string CodeFiltre { get; set; }

        [DataMember]
        [Column(Name = "LIBELLEFILTRE")]
        public string DescriptionFiltre { get; set; }

        [DataMember]
        [Column(Name = "DATECREATION")]
        public int DateCreation { get; set; }

        [DataMember]
        public List<ModeleLigneBrancheCibleDto> ListePairesBrancheCible { get; set; }

    }
}
