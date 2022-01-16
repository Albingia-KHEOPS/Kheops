using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.GestionDocument
{
    public class ElementGenerateurConditionBrancheDto
    {
        [Column(Name="CODECONDITION")]
        public int CodeCondition { get; set; }
        [Column(Name="SERVICE")]
        public string Service { get; set; }
        [Column(Name="ACTEGESTION")]
        public string ActeGestion { get; set; }
        [Column(Name="ETAPE")]
        public string Etape { get; set; }
        [Column(Name="CONTEXTE")]
        public string Contexte { get; set; }
        [Column(Name="ELEMENTGENERATEUR")]
        public string ElementGenerateur { get; set; }
        [Column(Name="CODEELEMENT")]
        public int CodeElement { get; set; }
        [Column(Name="BRANCHE")]
        public string Branche { get; set; }
        
        [Column(Name="NBENREGISTREMENTS")]
        public int NbEnregistrements { get; set; }
    }
}
