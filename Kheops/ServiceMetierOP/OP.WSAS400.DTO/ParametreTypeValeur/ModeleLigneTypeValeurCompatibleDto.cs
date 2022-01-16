using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreTypeValeur
{
    public class ModeleLigneTypeValeurCompatibleDto
    {
        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }

        [DataMember]
        [Column(Name = "CODETYPEVALEURCOMP")]
        public string CodeTypeValeurComp { get; set; }

        [DataMember]
        [Column(Name = "LIBELLETYPEVALEURCOMP")]
        public string DescriptionTypeValeurComp { get; set; }
    }
}
