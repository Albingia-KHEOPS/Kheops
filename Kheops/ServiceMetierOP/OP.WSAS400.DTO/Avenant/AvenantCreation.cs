using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Avenant {
    //[DataContract]
    public struct AvenantCreation {
        public string ipb;
        public int alx;
        public string date;
        public string description;
        public string observations;
    }
}
