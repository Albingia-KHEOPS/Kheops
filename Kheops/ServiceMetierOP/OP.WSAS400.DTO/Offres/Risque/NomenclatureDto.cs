using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres.Risque
{
     [DataContract]
    public class NomenclatureDto
    {
         [DataMember]
         [Column(Name = "NUMCOMBO")]
         public int NumeroCombo { get; set; }

         [DataMember]
         [Column(Name = "IDVALEUR")]
         public Int64 IdValeur { get; set; }

         [DataMember]
         [Column(Name = "NIVCOMBO")]
         public string NiveauCombo { get; set; }

         [DataMember]
         [Column(Name = "LIBELLECOMBO")]
         public string LibelleCombo { get; set; }

         [DataMember]
         [Column(Name = "CODENOMEN")]
         public string CodeNomenclature { get; set; }

         [DataMember]
         [Column(Name = "LIBNOMEN")]
         public string LibelleNomenclature { get; set; }

         [DataMember]
         [Column(Name = "ORDREVAL")]
         public Double OrdreNomenclature { get; set; }
    }
}
