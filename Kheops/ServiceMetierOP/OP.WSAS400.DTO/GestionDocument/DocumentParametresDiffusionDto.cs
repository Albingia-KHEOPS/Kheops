using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.GestionDocument
{
    public class DocumentParametresDiffusionDto
    {
        [Column(Name="CODEDIFFUSION")]
        public int CodeDiffusion { get; set; }
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
        [Column(Name="CODEDOCUMENT")]
        public int CodeDocument { get; set; }
        [Column(Name="NUMORDRE")]
        public int NumOrdre { get; set; }
        [Column(Name="TYPEENVOI")]
        public string TypeEnvoi { get; set; }
        [Column(Name="TYPEDIFFUSION")]
        public string TypeDiffusion { get; set; }
        [Column(Name="TYPEDESTINATAIRE")]
        public string TypeDestinataire { get; set; }
        [Column(Name="TYPEINTERVENANT")]
        public string TypeIntervenant { get; set; }
        [Column(Name="FONCTIONINTERLOCUTEUR")]
        public string FonctionInterlocuteur { get; set; }
        [Column(Name="NATURE")]
        public string Nature { get; set; }
        [Column(Name="NBEXEMPLAIRE")]
        public int NbExemplaire { get; set; }
        [Column(Name="TYPEACCOMPAGNANT")]
        public string TypeAccompagnant { get; set; }
        [Column(Name="DOCUMENTACCOMPAGNANT")]
        public string DocumentAccompagnant { get; set; }
        [Column(Name="CODEACCOMPAGNANT")]
        public int CodeAccompagnant { get; set; }
    }
}
