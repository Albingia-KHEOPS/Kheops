using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreTypeValeur
{
    public class ModeleDetailsTypeValeurDto
    {
        [DataMember]
        [Column(Name = "CODETYPEVALEUR")]
        public string CodeTypeValeur { get; set; }

        [DataMember]
        [Column(Name = "LIBELLETYPEVALEUR")]
        public string DescriptionTypeValeur { get; set; }

        [DataMember]
        public List<ModeleLigneTypeValeurCompatibleDto> ListeTypesValeurCompatible { get; set; }
           

    }
}
