using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ControleFin
{
    [DataContract]
    public class ControleFinDto : _DTO_Base
    {
        [DataMember]
        public List<ControleFinControleDto> ControleFinListeControleDto { get; set; }

        public ControleFinDto()
        {
            ControleFinListeControleDto = new List<ControleFinControleDto>();
        }
    }
}
