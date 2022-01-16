using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque
{
    public class ModeleInformationsGeneralesRisque
    {
        public bool ModeRisque { get; set; }
        public bool ModeMultiObjet { get; set; }
        public bool ReadOnly { get; set; }
        public bool MultiObjet { get; set; }
        public bool CibleModifiable { get; set; }
        public bool DateModifiable { get; set; }
        public bool DateModifiableAvn { get; set; }
        public bool NomenclatureRisqueModifiable { get; set; }
        public bool IsRisqueIndexe { get; set; }
        public double IndiceActualise { get; set; }
        public double IndiceOrigine { get; set; }
        public string IndiceCode { get; set; }
        public bool IsReportValeur { get; set; }
        public string NumAvenantCourant { get; set; }
        public string NumAvenantCreationRsq { get; set; }
        public bool IsModeAvenant { get; set; }
        public bool IsModifHorsAvenant { get; set; }
        public string CodePeriodicite { get; set; }
        public bool IsExistLoupe { get; set; } 

        public ModeleInformationsGeneralesRisqueNomenclatures ListesNomenclatures { get; set; }

        #region Info du risque dans l'objet
        [Display(Name = "Risque")]
        public string DescRisque { get; set; }

        public DateTime? DateDebRisque { get; set; }
        public DateTime? DateFinRisque { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "Code")]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>
        [Display(Name = "Descriptif *")]
        [Required]
        public String Descriptif { get; set; }

        /// <summary>
        /// Gets or sets the designation.
        /// </summary>
        /// <value>
        /// The designation.
        /// </value>
        [Display(Name = "Désignation")]
        public String Designation { get; set; }

        [Display(Name = "Cible *")]
        [Required]
        public String Cible { get; set; }
        /// <summary>
        /// Gets or sets the cibles.
        /// </summary>
        /// <value>
        /// The cibles.
        /// </value>
        public List<AlbSelectListItem> Cibles { get; set; }

        public DateTime? DateDebEffet { get; set; }
        public DateTime? DateFinEffet { get; set; }

        /// <summary>
        /// Gets or sets the date entree garantie.
        /// </summary>
        /// <value>
        /// The date entree garantie.
        /// </value>
        [Display(Name = "Période de garantie du")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? DateEntreeGarantie { get; set; }

        [Display(Name = "Heure entrée")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")]
        public TimeSpan? HeureEntreeGarantie { get; set; }

        /// <summary>
        /// Gets or sets the date sortie garantie.
        /// </summary>
        /// <value>
        /// The date sortie garantie.
        /// </value>
        [Display(Name = "au")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? DateSortieGarantie { get; set; }
        public DateTime? DateSortieGarantieHisto { get; set; }

        [Display(Name = "Heure sortie")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")]
        public TimeSpan? HeureSortieGarantie { get; set; }
        public TimeSpan? HeureSortieGarantieHisto { get; set; }

        /// <summary>
        /// Gets or sets the valeur.
        /// </summary>
        /// <value>
        /// The valeur.
        /// </value>
        [Display(Name = "Valeur")]
        public Int64? Valeur { get; set; }

        [Display(Name = "Unité")]
        public String Unite { get; set; }
        /// <summary>
        /// Gets or sets the unites.
        /// </summary>
        /// <value>
        /// The unites.
        /// </value>
        public List<AlbSelectListItem> Unites { get; set; }

        [Display(Name = "Type")]
        public String Type { get; set; }
        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>
        /// The types.
        /// </value>
        public List<AlbSelectListItem> Types { get; set; }

        [Display(Name = "HT/TTC")]
        public String ValeurHT { get; set; }
        public List<AlbSelectListItem> ValeursHT { get; set; }

        public bool DisplayInfosValeur { get; set; }

        [Display(Name = "Cout/m²")]
        public Int64? CoutM2 { get; set; }

        public bool IsReadOnly { get; set; }

        [Display(Name = "Code APE")]
        public string CodeApe { get; set; }
        public List<AlbSelectListItem> CodesApe { get; set; }

        [Display(Name = "Code activité")]
        public string CodeTre { get; set; }
        public string LibTre { get; set; }
        public List<AlbSelectListItem> CodesTre{ get; set; }

        [Display(Name = "Classe")]
        public string CodeClasse { get; set; }
        public List<AlbSelectListItem> CodesClasse { get; set; }

        [Display(Name = "Territorialité")]
        public string Territorialite { get; set; }
        public List<AlbSelectListItem> Territorialites { get; set; }
                
        [Display(Name = "Type risque")]
        public string TypeRisque { get; set; }
        public List<AlbSelectListItem> TypesRisque { get; set; }

        [Display(Name = "Type matériel")]
        public string TypeMateriel { get; set; }
        public List<AlbSelectListItem> TypesMateriel { get; set; }
    
        [Display(Name = "Nature lieux")]
        public string NatureLieux { get; set; }
        public List<AlbSelectListItem> NaturesLieux { get; set; }

        [Display(Name="Période descriptive du")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? DateEntreeDescr { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        public TimeSpan? HeureEntreeDescr { get; set; }
        [Display(Name = "au")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? DateSortieDescr { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        public TimeSpan? HeureSortieDescr { get; set; }
        public bool IsRisqueTemporaire { get; set; }
        public bool IsDateFinGarantieModifiable { get; set; }

        public bool? IsAvnDisabled { get; set; }
    }
}