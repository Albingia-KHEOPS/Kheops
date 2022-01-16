using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WAS400.DTO.User
{
    [DataContract]
    public class UserRightsDto
    {
        [DataMember]
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }
        [DataMember]
        [Column(Name = "RIGHTS")]
        public string Rights { get; set; }
    }
}