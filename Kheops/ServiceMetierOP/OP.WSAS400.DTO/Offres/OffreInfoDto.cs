using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Offres
{
    public class OffreInfoDto
    {
        [Column(Name = "CODECOURTIER")]
        public int CodeCourtier { get; set; }
        [Column(Name = "CODEASSURE")]
        public int CodeAssure { get; set; }
        [Column(Name = "DATEEFFETA")]
        public Int16 DateEffetAnnee { get; set; }
        [Column(Name = "DATEEFFETM")]
        public Int16 DateEffetMois { get; set; }
        [Column(Name = "DATEEFFETJ")]
        public Int16 DateEffetJour { get; set; }
        [Column(Name = "FINEFFETANNEE")]
        public Int16 FinEffetAnnee { get; set; }
        [Column(Name = "FINEFFETMOIS")]
        public Int16 FinEffetMois { get; set; }
        [Column(Name = "FINEFFETJOUR")]
        public Int16 FinEffetJour { get; set; }
        [Column(Name = "CODEBRANCHE")]
        public string CodeBranche { get; set; }
        [Column(Name = "FRAISACC")]
        public Int64 FraisAccessoires { get; set; }
        [Column(Name = "ATTENTAT")]
        public string Attentat { get; set; }
        [Column(Name = "MONTANTFRAIS")]
        public Int32 MontantFrais { get; set; }
        [Column(Name = "APPLICATIONACC")]
        public string ApplicationAcc { get; set; }
    }
}
