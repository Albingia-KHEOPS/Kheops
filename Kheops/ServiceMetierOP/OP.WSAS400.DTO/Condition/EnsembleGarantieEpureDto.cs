using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Condition
{
    public class EnsembleGarantieEpureDto
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public List<LigneGarantieDto> LstLigneGarantie { get; set; }
    }
}
