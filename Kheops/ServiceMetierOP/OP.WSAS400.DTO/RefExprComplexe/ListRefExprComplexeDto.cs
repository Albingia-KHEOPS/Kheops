using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.RefExprComplexe
{
    [DataContract]
    public class ListRefExprComplexeDto
    {
        [DataMember]
        public List<ParametreDto> ListLCI { get; set; }
        [DataMember]
        public List<ParametreDto> ListFranchise { get; set; }
    }

}
