using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    public class InfoConnexitePlatDto
    {
        [Column(Name = "CODECONNEXITE")]
        public string CodeConnexite { get; set; }
        [Column(Name = "LIBCONNEXITE")]
        public string LibConnexite { get; set; }

        [Column(Name = "CODEAFFAIRE1")]
        public string CodeAffaire1 { get; set; }
        [Column(Name = "VERSION1")]
        public Int32 Version1 { get; set; }
        [Column(Name = "TYPE1")]
        public string Type1 { get; set; }
        [Column(Name = "CODEBRANCHE1")]
        public string CodeBranche1 { get; set; }
        [Column(Name = "LIBBRANCHE1")]
        public string LibBranche1 { get; set; }
        [Column(Name = "CODECIBLE1")]
        public string CodeCible1 { get; set; }
        [Column(Name = "LIBCIBLE1")]
        public string LibCible1 { get; set; }
        [Column(Name = "NUMCONX1")]
        public Int64 NumConx1 { get; set; }
        [Column(Name = "CODEASS1")]
        public Int32 CodeAss1 { get; set; }
        [Column(Name = "NOMASS1")]
        public string NomAss1 { get; set; }
        [Column(Name = "AD1_1")]
        public string Ad1_1 { get; set; }
        [Column(Name = "AD2_1")]
        public string Ad2_1 { get; set; }
        [Column(Name = "DEP1")]
        public string Dep1 { get; set; }
        [Column(Name = "CP1")]
        public string Cp1 { get; set; }
        [Column(Name = "VIL1")]
        public string Ville1 { get; set; }

        [Column(Name = "CODEAFFAIRE2")]
        public string CodeAffaire2 { get; set; }
        [Column(Name = "VERSION2")]
        public Int32 Version2 { get; set; }
        [Column(Name = "TYPE2")]
        public string Type2 { get; set; }
        [Column(Name = "CODEBRANCHE2")]
        public string CodeBranche2 { get; set; }
        [Column(Name = "LIBBRANCHE2")]
        public string LibBranche2 { get; set; }
        [Column(Name = "CODECIBLE2")]
        public string CodeCible2 { get; set; }
        [Column(Name = "LIBCIBLE2")]
        public string LibCible2 { get; set; }
        [Column(Name = "NUMCONX2")]
        public Int64 NumConx2 { get; set; }
        [Column(Name = "CODEASS2")]
        public Int32 CodeAss2 { get; set; }
        [Column(Name = "NOMASS2")]
        public string NomAss2 { get; set; }
        [Column(Name = "AD1_2")]
        public string Ad1_2 { get; set; }
        [Column(Name = "AD2_2")]
        public string Ad2_2 { get; set; }
        [Column(Name = "DEP2")]
        public string Dep2 { get; set; }
        [Column(Name = "CP2")]
        public string Cp2 { get; set; }
        [Column(Name = "VIL2")]
        public string Ville2 { get; set; }

        [Column(Name = "CODEOBSV")]
        public Int64 CodeObsv { get; set; }
        [Column(Name = "OBSV")]
        public string Observations { get; set; }

    }
}
