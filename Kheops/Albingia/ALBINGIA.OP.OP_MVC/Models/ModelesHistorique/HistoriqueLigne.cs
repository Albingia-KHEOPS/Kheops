using ALBINGIA.Framework.Common.Tools;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesHistorique
{
    public class HistoriqueLigne
    {
        public Int32 NumInterne { get; set; }
        public Int32 NumExterne { get; set; }
        public string Reference { get; set; }
        public string Designation { get; set; }
        public Int64 DateEffetInt { get; set; }
        public Int64 DateCreationInt { get; set; }
        public string Motif { get; set; }
        public string LibMotif { get; set; }
        public string Etat { get; set; }
        public string LibEtat { get; set; }
        public string Situation { get; set; }
        public string LibSituation { get; set; }
        public string Qualite { get; set; }
        public string LibQualite { get; set; }
        public string TypeRetour { get; set; }
        public string LibRetour { get; set; }
        public Int64 DateRetourInt { get; set; }
        public string Traitement { get; set; }
        public string LibTraitement { get; set; }
        public string ReguleId { get; set; }

        public string DateEffet { get { return AlbConvert.ConvertDateToStr(AlbConvert.ConvertIntToDate(Convert.ToInt32(this.DateEffetInt))); } }
        public string DateCreation { get { return AlbConvert.ConvertDateToStr(AlbConvert.ConvertIntToDate(Convert.ToInt32(this.DateCreationInt))); } }
        public string DateRetour { get { return AlbConvert.ConvertDateToStr(AlbConvert.ConvertIntToDate(Convert.ToInt32(this.DateRetourInt))); } }
    }
}