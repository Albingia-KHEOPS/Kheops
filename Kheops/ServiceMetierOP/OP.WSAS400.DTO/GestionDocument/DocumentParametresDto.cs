using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.GestionDocument
{
    public class DocumentParametresDto
    {
        [Column(Name="CODEDOCUMENT")]
        public int CodeDocument { get; set; }
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
        [Column(Name="CODEELEMENTGENERATEUR")]
        public int CodeElementGenerateur { get; set; }
        [Column(Name="NUMORDRE")]
        public decimal NumOrdre { get; set; }
        [Column(Name="TYPEDOC")]
        public string TypeDoc { get; set; }
        [Column(Name="CODEDOC")]
        public string CodeDoc { get; set; }
        [Column(Name="NATUREGENERATION")]
        public string NatureGeneration { get; set; }
        [Column(Name="CODEDIFFUSION")]
        public int CodeDiffusion { get; set; }
        [Column(Name="ACTIONENCHAINEE")]
        public string ActionEnchainee { get; set; }
        [Column(Name="CODETEXTE")]
        public int CodeTexte { get; set; }
    }
}
