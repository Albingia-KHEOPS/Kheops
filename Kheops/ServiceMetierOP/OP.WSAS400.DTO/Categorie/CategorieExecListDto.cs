using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Categorie
{
    public class CategorieExecListDto
    {    
        [DataMember]
        [Column(Name = "GUID")]
        public Int64 GuidId { get; set; }   
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }       
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }    
        [DataMember]
        [Column(Name = "BRANCHE")]
        public string CodeBranche { get; set; }
        [DataMember]
        [Column(Name = "CARACTERE")]
        public string Caractere { get; set; }
    }
}
