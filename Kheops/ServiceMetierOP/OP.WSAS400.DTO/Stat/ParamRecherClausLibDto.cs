using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Stat
{
    [DataContract]
    public class ParamRecherClausLibDto
    {
        [DataMember]
        public string DateSaisieDebut { get; set; }
        [DataMember]
        public string DateSaisieFin { get; set; }
        [DataMember]
        public string DateCreationDebut { get; set; }
        [DataMember]
        public string DateCreationFin { get; set; }
        [DataMember]
        public string SortingBy { get; set; }
        [DataMember]
        public string Order { get; set; }
        [DataMember]
        public int LineCount { get; set; }
        [DataMember]
        public int StartLine { get; set; }
        [DataMember]
        public int EndLine { get; set; }
        [DataMember]
        public int PageNumber { get; set; }


    }
}
