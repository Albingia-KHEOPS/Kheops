using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
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
    public class LigneRegularisationGarantieDto
    {
        [DataMember]
        [Column(Name = "CODERSQ")]
        public Int32 CodeRisque { get; set; }
        [DataMember]
        [Column(Name = "LIBRSQ")]
        public string RisqueIdentification { get; set; }
        [DataMember]
        [Column(Name = "CODTAXEREGIME")]
        public string CodeTaxeRegime { get; set; }
        [DataMember]
        [Column(Name = "TAXEREGIME")]
        public string RegimeTaxe { get; set; }
        [DataMember]   
        [Column(Name = "CODEFOR")]
        public Int32 CodeFormule { get; set; }
        [DataMember]
        [Column(Name = "LIBFOR")]
        public string FormuleDescriptif { get; set; }

        [DataMember]
        [Column(Name = "LETTREFOR")]
        public string LettreFor { get; set; }

        [DataMember]
        [Column(Name = "IDGAR")]
        public long IdGar { get; set; }

        [DataMember]
        [Column(Name = "CODEGAR")]
        public string CodeGar { get; set; }
        [DataMember]
        [Column(Name = "LIBGAR")]
        public string LibGarantie { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBGAR")]
        public Int64 DateDebPeriodGenerale { get; set; }
        [DataMember]
        [Column(Name = "DATEFINGAR")]
        public Int64 DateFinPeriodGenerale { get; set; }
        [DataMember]
        [Column(Name = "CODETYPEREGULE")]
        public string CodeTypeRegule { get; set; }
        [DataMember]
        [Column(Name = "LIBTYPEREGULE")]
        public string TypeRegul { get; set; }
        [DataMember]
        [Column(Name = "CODTAXEGAR")]
        public string CodeTaxeGar { get; set; }
       
    }
}
