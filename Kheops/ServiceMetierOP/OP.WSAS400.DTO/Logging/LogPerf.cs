using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Logging
{
    [DataContract]
    public class LogPerf
    {
        [Column(Name = "DATELOG")]
        [DataMember]
        public string DateLog { get; set; }
        [Column(Name = "USER")]
        [DataMember]
        public string User { get; set; }
        [Column(Name = "SCREEN")]
        [DataMember]
        public string Screen { get; set; }
        [Column(Name = "ACTION")]
        [DataMember]
        public string Action { get; set; }
        [Column(Name = "ELAPSEDTIME")]
        [DataMember]
        public string ElapsedTime { get; set; }

    }
}
