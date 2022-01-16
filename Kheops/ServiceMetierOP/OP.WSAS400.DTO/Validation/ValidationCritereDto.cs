using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Validation
{
    [DataContract]
    public class ValidationCritereDto : Validation_Base
    {
        [DataMember]
        [Column(Name="LIBELLE")]
        public string Critere { get; set; }
        [DataMember]
        [Column(Name="NIVEAU")]
        public string NiveauValidation { get; set; }
    }
}
