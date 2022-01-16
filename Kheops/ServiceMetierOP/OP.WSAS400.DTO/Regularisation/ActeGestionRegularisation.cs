using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class ActeGestionRegularisation
    {        
        [DataMember]
        [Column(Name = "CODEAVN")]
        public Int32 CodeAvn { get; set; }

        [DataMember]
        [Column(Name = "REGULEMOD")]
        public RegularisationMode ReguleMode { get; set; }
        // B3938 
        // Récupération de la valeur texte de "RegularisationMode"
        [DataMember]
        [Column(Name = "REGULEMODVALUE")]
        public string ReguleModeStringValue { get; set; }
    }
}
