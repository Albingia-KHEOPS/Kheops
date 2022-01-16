using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.MetaData
{
    public class ContactAddresse_MetaData : ModelBase
    {
        /// <summary>
        /// Constructeur initialisant l'index de debut de tabulation 
        /// </summary>
        /// <param name="beginIndex">Index de départ</param>
        public ContactAddresse_MetaData(int beginIndex = 0, bool argReadOnly = false, bool argSaisieHexavia = false)
        {
            FirstIndex = beginIndex;
            ReadOnly = argReadOnly;
            SaisieHexavia = argSaisieHexavia;
        }

        /// <summary>
        /// Gets or sets the first index.
        /// </summary>
        /// <value>
        /// The first index.
        /// </value>
        public int FirstIndex { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [read only]; otherwise, <c>false</c>.
        /// </value>
        public bool ReadOnly { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [saisie hexavia].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [saisie hexavia]; otherwise, <c>false</c>.
        /// </value>
        public bool SaisieHexavia { get; set; }
        /// <summary>
        /// Gets or sets the no chrono.
        /// </summary>
        /// <value>
        /// The no chrono.
        /// </value>
        public int? NoChrono { get; set; }
        /// <summary>
        /// Gets or sets the distribution.
        /// </summary>
        /// <value>
        /// The distribution.
        /// </value>
        [Display(Name = "Bâtiment | ZI")]
        public string Batiment { get; set; }
        /// <summary>
        /// Gets or sets the no.
        /// </summary>
        /// <value>
        /// The no.
        /// </value>
        [Display(Name = "N° | Ext | Voie")]
        public string No { get; set; }
        public string No2 { get; set; }
        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        [Display(Name = "Ext.")]
        public string Extension { get; set; }
        /// <summary>
        /// Gets or sets the voie.
        /// </summary>
        /// <value>
        /// The voie.
        /// </value>
        [Display(Name = "voie")]
        public string Voie { get; set; }
        /// <summary>
        /// Gets or sets the distribution.
        /// </summary>
        /// <value>
        /// The distribution.
        /// </value>
        [Display(Name = "Distribution")]
        public string Distribution { get; set; }
        /// <summary>
        /// Gets or sets the code postal.
        /// </summary>
        /// <value>
        /// The code postal.
        /// </value>
        [Display(Name = "CP | Ville")]
        public string CodePostal { get; set; }
        /// <summary>
        /// Gets or sets the ville.
        /// </summary>
        /// <value>
        /// The ville.
        /// </value>
        [Display(Name = "Ville")]
        public string Ville { get; set; }
        /// <summary>
        /// Gets or sets the code postal cedex.
        /// </summary>
        /// <value>
        /// The code postal cedex.
        /// </value>
        [Display(Name = "CP | Ville (Cedex)")]
        public string CodePostalCedex { get; set; }
        /// <summary>
        /// Gets or sets the ville cedex.
        /// </summary>
        /// <value>
        /// The ville cedex.
        /// </value>
        [Display(Name="Ville")]
        public string VilleCedex { get; set; }
        /// <summary>
        /// Gets or sets the pays.
        /// </summary>
        /// <value>
        /// The pays.
        /// </value>
        [Display(Name="Pays")]
        public string Pays { get; set; }

        /// <summary>
        /// Gets or sets the matricule hexavia.
        /// </summary>
        /// <value>
        /// The matricule hexavia.
        /// </value>
        public string MatriculeHexavia { get; set; }
    }
}