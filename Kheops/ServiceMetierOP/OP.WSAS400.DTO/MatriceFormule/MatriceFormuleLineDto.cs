using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.MatriceFormule
{
    public class MatriceFormuleLineDto
    {
        [Column(Name="TYPEENREGISTREMENT")]
        public string TypeEnregistrement { get; set; }
        [Column(Name="CODERISQUE")]
        public Int32 CodeRisque { get; set; }
        [Column(Name = "CODEOBJET")]
        public Int32 CodeObjet { get; set; }
        [Column(Name = "HASINVENTAIRE")]
        public string hasInventaire { get; set; }
        [Column(Name = "ISAFFECTE")]
        public string isAffecte { get; set; }
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [Column(Name = "MONOOBJET")]
        public Int64 MonoObjet { get; set; }
        [Column(Name = "NUMCHRONO")]
        public Int32 NumChronoLine { get; set; }
        [Column(Name = "CREATEAVT")]
        public Int32 CreateAvt { get; set; }
        [Column(Name = "MODIFAVT")]
        public Int32 ModifAvt { get; set; }
        [Column(Name = "DATEAVT")]
        public Int64 DateAvt { get; set; }
        [Column(Name = "DATEFINAVT")]
        public Int64 DateFinAvt { get; set; }

    }
}
