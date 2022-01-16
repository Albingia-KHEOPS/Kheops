using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie.GarantieModele
{
    [DataContract]
    public class GaranNivModel
    {
        [Column(Name = "CODENIV")]
        public Int32 CodeNiv { get; set; }
        [Column(Name = "CODEGARNIV")]
        public string CodeGarNiv { get; set; }
        [Column(Name = "C2MGANIV")]
        public string C2Mganiv { get; set; }
        [Column(Name = "DESCRNIV")]
        public string DescrNiv { get; set; }
        [Column(Name = "CARACNIV")]
        public string CaracNiv { get; set; }
        [Column(Name = "NATURENIV")]
        public string NatureNiv { get; set; }
        [Column(Name = "CODEPARENTNIV")]
        public Int32 CodeParentNiv { get; set; }
        [Column(Name = "CODENIVNIV")]
        public Int32 CodeNiv1Niv { get; set; }
        [Column(Name = "NIVEAUNIV")]
        public Int16 NiveauNiv { get; set; }
        [Column(Name = "TRINIV")]
        public string TriNiv { get; set; }
        [Column(Name = "DEFGARANTIENIV")]
        public string DefGarantieNiv { get; set; }
        [Column(Name = "CATNATNIV")]
        public string CatNavNiv { get; set; }
        [Column(Name = "INDEXEENIV")]
        public string IndexeeNiv { get; set; }
        [Column(Name = "CODETAXENIV")]
        public string CodeTaxeNiv { get; set; }
        [Column(Name = "ASSIETTEVALEURNIV")]
        public double AssietteValeurNiv { get; set; }
        [Column(Name = "ASSIETTEUNITENIV")]
        public string AssietteUniteNiv { get; set; }
        [Column(Name = "ASSIETTEBASENIV")]
        public string AssietteBaseNiv { get; set; }
        [Column(Name = "ASSIETTEMODIFIABLENIV")]
        public string AssietteModifiableNiv { get; set; }
        [Column(Name = "TYPECONTROLEDATENIV")]
        public string TypeControleDateNiv { get; set; }
        [Column(Name = "MONTANTREFNIV")]
        public string MontantRefNiv { get; set; }
        [Column(Name = "ASSOBLIGATOIRENIV")]
        public string AssObligatoireNiv { get; set; }
        [Column(Name = "PARAMNATMODNIV")]
        public string ParamNatModNiv { get; set; }
        [Column(Name = "MODEALIMNIV")]
        public string ModeAlimNiv { get; set; }
        [Column(Name = "INVENPOSSIBLE")]
        public string InvenPossible { get; set; }
        [Column(Name = "TPCNNIV")]
        public double TpcnNiv { get; set; }

        [Column(Name = "PRIMODNIV")]
        public string PrimodNiv { get; set; }
        [Column(Name = "PRIOBLNIV")]
        public string PriOblNiv { get; set; }
        [Column(Name = "PRIVALNIV")]
        public double PriValNiv { get; set; }
        [Column(Name = "PRIUNTNIV")]
        public string PriUntNiv { get; set; }
        [Column(Name = "PRIBASNIV")]
        public string PriBasNiv { get; set; }
        [Column(Name = "LCIMODNIV")]
        public string LciModNiv { get; set; }
        [Column(Name = "LCIOBLNIV")]
        public string LciOblNiv { get; set; }
        [Column(Name = "LCIVALNIV")]
        public double LciValNiv { get; set; }
        [Column(Name = "LCIUNTNIV")]
        public string LciUntNiv { get; set; }
        [Column(Name = "LCIBASNIV")]
        public string LciBasNiv { get; set; }
        [Column(Name = "FRHMODNIV")]
        public string FrhModNiv { get; set; }
        [Column(Name = "FRHOBLNIV")]
        public string FrhOblNiv { get; set; }
        [Column(Name = "FRHVALNIV")]
        public double FrhValNiv { get; set; }
        [Column(Name = "FRHUNTNIV")]
        public string FrhUntNiv { get; set; }
        [Column(Name = "FRHBASNIV")]
        public string FrhBasNiv { get; set; }
        [Column(Name = "FRHMODMINNIV")]
        public string FrhModMinNiv { get; set; }
        [Column(Name = "FRHOBLMINNIV")]
        public string FrhOblMinNiv { get; set; }
        [Column(Name = "FRHVALMINNIV")]
        public double FrhValMinNiv { get; set; }
        [Column(Name = "FRHUNTMINNIV")]
        public string FrhUntMinNiv { get; set; }
        [Column(Name = "FRHBASMINNIV")]
        public string FrhBasMinNiv { get; set; }
        [Column(Name = "FRHMODMAXNIV")]
        public string FrhModMaxNiv { get; set; }
        [Column(Name = "FRHOBLMAXNIV")]
        public string FrhOblMaxNiv { get; set; }
        [Column(Name = "FRHVALMAXNIV")]
        public double FrhValMaxNiv { get; set; }
        [Column(Name = "FRHUNTMAXNIV")]
        public string FrhUntMaxNiv { get; set; }
        [Column(Name = "FRHBASMAXNIV")]
        public string FrhBasMaxNiv { get; set; }
    }
}
