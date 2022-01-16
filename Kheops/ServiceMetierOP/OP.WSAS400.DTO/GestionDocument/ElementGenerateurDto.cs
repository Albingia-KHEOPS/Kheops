using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.GestionDocument
{
    public class ElementGenerateurDto
    {
        [Column(Name="CODEELEMENT")]
        public int CodeElement { get; set; }
        [Column(Name="SERVICE")]
        public string Service { get; set; }
        [Column(Name="ACTEGESTION")]
        public string ActeGestion { get; set; }
        [Column(Name="ETAPE")]
        public string Etape { get; set; }
        [Column(Name="CONTEXTE")]
        public string Contexte { get; set; }
        [Column(Name="CODECONTEXTE")]
        public int CodeContexte { get; set; }
        [Column(Name="NUMORDRE")]
        public decimal NumOrdre { get; set; }
        [Column(Name="ELEMENTGENERATEUR")]
        public string  ElementGenerateur { get; set; }
        [Column(Name="LIBELLE1")]
        public string Libelle1 { get; set; }
        [Column(Name="LIBELLE2")]
        public string Libelle2 { get; set; }
    }
}
