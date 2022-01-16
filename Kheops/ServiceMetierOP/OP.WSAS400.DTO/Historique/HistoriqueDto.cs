using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Historique
{
    [DataContract]
    public class HistoriqueDto
    {
        [DataMember]
        public bool IsContractuel { get; set; }
        [DataMember]
        public List<HistoriqueLigneDto> ListHistorique { get; set; }
         
    }
}
