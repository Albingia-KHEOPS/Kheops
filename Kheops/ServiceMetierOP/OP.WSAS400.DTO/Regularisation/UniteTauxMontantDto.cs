﻿using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class UniteTauxMontantDto
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Label { get; set; }
    }
}