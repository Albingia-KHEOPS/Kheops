using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie
{
    public class ModeleContactAdresse
    {
        public ModeleContactAdresse()
        {
            Context = "";
            FirstIndex = 0;
            ReadOnly = false;
            SaisieHexavia = false;
        }
        /// <summary>
        /// Constructeur initialisant l'index de debut de tabulation 
        /// </summary>
        /// <param name="beginIndex">Index de départ</param>
        public ModeleContactAdresse(int beginIndex = 0, bool argReadOnly = false, bool argSaisieHexavia = false, string argContext = "")
        {
            Context = argContext;
            FirstIndex = beginIndex;
            ReadOnly = argReadOnly;
            SaisieHexavia = argSaisieHexavia;
        }


        public string Context { get; set; }
        public int FirstIndex { get; set; }
        public bool ReadOnly { get; set; }
        public bool IsModifHorsAvn { get; set; }
        public bool SaisieHexavia { get; set; }
        public int? NoChrono { get; set; }
        [Display(Name = "Bâtiment | ZI")]
        public string Batiment { get; set; }
        [Display(Name = "N° | Ext | Voie")]
        public string No { get; set; }
        public string No2 { get; set; }
        [Display(Name = "Ext.")]
        public string Extension { get; set; }
        [Display(Name = "voie")]
        public string Voie { get; set; }
        [Display(Name = "Distribution")]
        public string Distribution { get; set; }
        [Display(Name = "CP | Ville")]
        public string CodePostal { get; set; }
        [Display(Name = "Ville")]
        public string Ville { get; set; }
        [Display(Name = "CP | Ville (Cedex)")]
        public string CodePostalCedex { get; set; }
        [Display(Name = "Ville")]
        public string VilleCedex { get; set; }
        [Display(Name = "Pays")]
        public string Pays { get; set; }
        public string MatriculeHexavia { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsReadOnlyDisplay { get; set; }
    }
}