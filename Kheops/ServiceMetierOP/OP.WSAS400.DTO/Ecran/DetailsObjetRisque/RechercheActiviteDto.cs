using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Ecran.DetailsObjetRisque
{

    [DataContract]
   public  class RechercheActiviteDto
    {
        [DataMember]
        public List<ActiviteDto> ListActivites { get; set; }
        [DataMember]
        public int ResultCount { get; set; }
    }
}
