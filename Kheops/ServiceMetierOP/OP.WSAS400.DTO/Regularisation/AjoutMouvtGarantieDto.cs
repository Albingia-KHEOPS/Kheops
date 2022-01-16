using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class AjoutMouvtGarantieDto
    {
        [DataMember]
        public string StrReturn { get; set; }
        [DataMember]
        public List<LigneMouvtGarantieDto> LignesMouvement { get; set; }
    }
}
