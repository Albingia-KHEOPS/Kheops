using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreGaranties
{
    public class TypeValeurDto
    {
        [DataMember]
        [Column(Name = "ID")]
        public Int64 Id { get; set; }
        [DataMember]
        [Column(Name = "CODEGARANTIE")]
        public string CodeGarantie { get; set; }
        [DataMember]
        [Column(Name = "NUMORDRE")]
        public Single NumOrdre { get; set; }
        [DataMember]
        [Column(Name = "CODETYPEVALEUR")]
        public string CodeTypeValeur { get; set; }
        [DataMember]
        [Column(Name = "LIBELLETYPEVALEUR")]
        public string LibelleTypeValeur { get; set; }
    }
}
