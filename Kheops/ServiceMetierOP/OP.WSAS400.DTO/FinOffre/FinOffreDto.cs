using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.FinOffre
{
    [DataContract]
    public class FinOffreDto : _DTO_Base
    {
        [DataMember]
        public FinOffreInfosDto FinOffreInfosDto { get; set; }
        [DataMember]
        public FinOffreAnnotationDto FinOffreAnnotationDto { get; set; }

        public FinOffreDto()
        {
            this.FinOffreInfosDto = new FinOffreInfosDto();
            this.FinOffreAnnotationDto = new FinOffreAnnotationDto();
        }
    }
}
