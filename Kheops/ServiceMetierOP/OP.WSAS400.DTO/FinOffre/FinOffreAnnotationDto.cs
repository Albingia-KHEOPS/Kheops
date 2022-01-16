using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.FinOffre
{
    [DataContract]
    public class FinOffreAnnotationDto
    {
        [DataMember]
        public string AnnotationFin { get; set; }

        public FinOffreAnnotationDto()
        {
            AnnotationFin = string.Empty;
        }
    }
}
