using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Cotisations
{
    public class FraisAccessoiresDto
    {
        [DataMember]
        [Column(Name = "TYPEFRAIS")]
        public string TypeFrais { get; set; }
        [DataMember]
        public List<ParametreDto> TypesFrais { get; set; }

        [DataMember]
        public Int32 FraisStandards { get; set; }

        [DataMember]
        [Column(Name = "FRAISRETENUS")]
        public Int32 FraisRetenus { get; set; }

        [DataMember]
        [Column(Name = "TAXEATTENTAT")]
        public string AppliqueTaxeAttentat { get; set; }

        [DataMember]
        [Column(Name = "TAXEATTENTATVALUE")]        
        public double TaxeAttentatValue {get;set;}

        [DataMember]
        [Column(Name = "FRAISSPECIFIQUES")]
        public Int32 FraisSpecifiques { get; set; }

        [DataMember]
        [Column(Name = "CODECOMMENTAIRES")]
        public Int64 CodeCommentaires { get; set; }

        [DataMember]
        [Column(Name = "COMMENTAIRES")]
        public string Commentaires { get; set; }

        [Column(Name = "TYPETRAITEMENT")]
        public string TypeTraitement { get; set; }

        [DataMember]
        [Column(Name = "ANNEEEFFET")]
        public int AnneeEffet { get; set; }

        [DataMember]
        [Column(Name = "HASPRIPES")]        
        public string HasPripes {get;set;}

        [DataMember]
        [Column(Name = "FRAISRETPRIPES")]
        public Int32 FraisRetPripes { get; set; }

        [DataMember]
        [Column(Name = "FRAIS")]
        public Int32 Frais { get; set; }

        [DataMember]
        [Column(Name="ATTCATEGO")]
        public string AttCatego { get; set; }

        [DataMember]
        public string ModifFraisStd { get; set; }
        
    }
}
