using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Common
{
    [DataContract]
    public class BlocageSaisieDto
    {
        [DataMember]
        public string Etat { get; set; }

        [DataMember]
        public string HeureReprise { get; set; }

        [DataMember]
        public string MinuteReprise { get; set; }
    }
}
