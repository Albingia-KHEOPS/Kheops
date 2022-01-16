using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.DocumentGestion
{
    [DataContract]
    public class CopieDocDto
    {
       
        [DataMember]
        [Column(Name = "KHQIPB")]
        public string Code { get; set; }

        [DataMember]
        [Column(Name = "KHQALX")]
        public int Version { get; set; }

        [DataMember]
        [Column(Name = "KHQTYP")]
        public string Type { get; set; }

        [DataMember]
        [Column(Name = "KHQAVN")]
        public int Avenant { get; set; }

        [DataMember]
        [Column(Name = "KHQOLDC")]
        public string OldCheminComplet { get; set; }

        [DataMember]
        [Column(Name = "KHQCODE")]
        public Int64 NewGuid { get; set; }

        [DataMember]
        [Column(Name = "KHQNOMD")]
        public string NewCheminComplet { get; set; }

        [DataMember]
        [Column(Name = "KHQTABLE")]
        public string TableCible { get; set; }
      
       


    }
}
