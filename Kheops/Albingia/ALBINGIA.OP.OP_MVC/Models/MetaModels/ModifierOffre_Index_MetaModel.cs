using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ALBINGIA.OP.OP_MVC.Models.ModelesFinOffre;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;

namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    /// <summary>
    /// ModifierOffre_Index_MetaData
    /// </summary>
    public class ModifierOffre_Index_MetaModel : MetaModelsBase
    {
        #region Informations à saisir
        public string txtParamRedirect { get; set; }

        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>
        [Display(Name = "Identification *")]
        public string Descriptif { get; set; }

        /// <summary>
        /// Gets or sets the cible.
        /// </summary>
        /// <value>
        /// The cible.
        /// </value>
        [Display(Name = "Cible")]
        public string Cible { get; set; }
        public string CibleLib { get; set; }
        /// <summary>
        /// Gets or sets the mots clefs.
        /// </summary>
        /// <value>
        /// The mots clefs.
        /// </value>
        [Display(Name = "Mots-clés")]
        public List<AlbSelectListItem> MotsClefs1 { get; set; }
        public List<AlbSelectListItem> MotsClefs2 { get; set; }
        public List<AlbSelectListItem> MotsClefs3 { get; set; }

        /// <summary>
        /// Gets or sets the mot clef1.
        /// </summary>
        /// <value>
        /// The mot clef1.
        /// </value>
        public string MotClef1 { get; set; }

        /// <summary>
        /// Gets or sets the mot clef2.
        /// </summary>
        /// <value>
        /// The mot clef2.
        /// </value>
        public string MotClef2 { get; set; }

        /// <summary>
        /// Gets or sets the mot clef3.
        /// </summary>
        /// <value>
        /// The mot clef3.
        /// </value>
        public string MotClef3 { get; set; }

        /// <summary>
        /// Gets or sets the observations.
        /// </summary>
        /// <value>
        /// The observations.
        /// </value>
        [Display(Name = "Observations")]
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the devises.
        /// </summary>
        /// <value>
        /// The devises.
        /// </value>
        [Display(Name = "Devise *")]
        public List<AlbSelectListItem> Devises { get; set; }

        public string Devise { get; set; }

      
        /// <summary>
        /// Gets or sets the periodicites.
        /// </summary>
        /// <value>
        /// The periodicites.
        /// </value>
        [Display(Name = "Périodicité *")]
        public List<AlbSelectListItem> Periodicites { get; set; }

        public string Periodicite { get; set; }

        /// <summary>
        /// Gets or sets the echeance principale.
        /// </summary>
        /// <value>
        /// The echeance principale.
        /// </value>
        [Display(Name = "Ech. principale")]
        public DateTime? EcheancePrincipale { get; set; }

        /// <summary>
        /// Gets or sets the effet garanties.
        /// </summary>
        /// <value>
        /// The effet garanties.
        /// </value>
        //[Display(Name = "Effet des garanties")]
        //[DisplayFormat(DataFormatString = "{0:d}", NullDisplayText = "", ApplyFormatInEditMode = true)]
        [Display(Name = "Début d'effet")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? EffetGaranties { get; set; }

        [Display(Name = "Date statistique")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? DateStatistique { get; set; }

        /// <summary>
        /// Gets or sets the heure saisie string.
        /// </summary>
        /// <value>
        /// The heure saisie string.
        /// </value>
        [Display(Name = "Heure d'effet")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")]
        public TimeSpan? HeureEffet { get; set; }

        public bool EffetCheck { get; set; }

        /// <summary>
        /// Gets or sets the fin effet.
        /// </summary>
        /// <value>
        /// The fin effet.
        /// </value>
        [Display(Name = "Fin effet")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? FinEffet { get; set; }

        /// <summary>
        /// Gets or sets the heure fin effet.
        /// </summary>
        /// <value>
        /// The heure fin effet.
        /// </value>
        [Display(Name = "Heure fin d'effet")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")]
        public TimeSpan? HeureFinEffet { get; set; }

        [Display(Name = "Durée")]
        public bool DureeCheck { get; set; }

        /// <summary>
        /// Gets or sets the duree.
        /// </summary>
        /// <value>
        /// The duree.
        /// </value>
        [Display(Name = "Durée")]
        public int? Duree { get; set; }

        /// <summary>
        /// Gets or sets the durees.
        /// </summary>
        /// <value>
        /// The durees.
        /// </value>
        public List<AlbSelectListItem> Durees { get; set; }

        public string DureeString { get; set; }

        /// <summary>
        /// Gets or sets the indice reference.
        /// </summary>
        /// <value>
        /// The indice reference.
        /// </value>
        [Display(Name = "Indice référence")]
        public List<AlbSelectListItem> IndicesReference { get; set; }

        public string IndiceReference { get; set; }

        /// <summary>
        /// Gets or sets the valeur.
        /// </summary>
        /// <value>
        /// The valeur.
        /// </value>
        [Display(Name = "Valeur")]
        public string Valeur { get; set; }

        /// <summary>
        /// Gets or sets the nature contrat.
        /// </summary>
        /// <value>
        /// The nature contrat.
        /// </value>
        [Display(Name = "Nature contrat *")]
        public List<AlbSelectListItem> NaturesContrat { get; set; }

        public string NatureContrat { get; set; }

        /// <summary>
        /// Gets or sets the part albingia.
        /// </summary>
        /// <value>
        /// The part albingia.
        /// </value>
        [Display(Name = "Part Albingia (%)")]
        public string PartAlbingia { get; set; }

        /// <summary>
        /// Gets or sets the aperiteur code.
        /// </summary>
        /// <value>
        /// The aperiteur code.
        /// </value>
        public string AperiteurCode { get; set; }

        /// <summary>
        /// Gets or sets the aperiteur nom.
        /// </summary>
        /// <value>
        /// The aperiteur nom.
        /// </value>
        [Display(Name = "Apériteur")]
        public string AperiteurNom { get; set; }

        /// <summary>
        /// Gets or sets the couverture.
        /// </summary>
        /// <value>
        /// The couverture.
        /// </value>
        [Display(Name = "Couverture (%)")]
        public int? Couverture { get; set; }

        /// <summary>
        /// Gets or sets the part aperiteur.
        /// </summary>
        /// <value>
        /// The part aperiteur.
        /// </value>
        public string PartAperiteur { get; set; }

        /// <summary>
        /// Gets or sets the frais ape.
        /// </summary>
        /// <value>
        /// The frais ape.
        /// </value>
        [Display(Name = "Comm. Apé. (%)")]
        public string FraisApe { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ModifierOffre_Index_MetaModel"/> is intercalaire.
        /// </summary>
        /// <value>
        ///   <c>true</c> if intercalaire; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Intercalaire courtier ?")]
        public bool Intercalaire { get; set; }

        /// <summary>
        /// Gets or sets the souscripteur code.
        /// </summary>
        /// <value>
        /// The souscripteur code.
        /// </value>
        public string SouscripteurCode { get; set; }

        /// <summary>
        /// Gets or sets the souscripteur nom.
        /// </summary>
        /// <value>
        /// The souscripteur nom.
        /// </value>
        [Display(Name = "Souscripteur *")]
        public string SouscripteurNom { get; set; }

        /// <summary>
        /// Gets or sets the gestionnaire code.
        /// </summary>
        /// <value>
        /// The gestionnaire code.
        /// </value>
        public string GestionnaireCode { get; set; }

        /// <summary>
        /// Gets or sets the gestionnaire nom.
        /// </summary>
        /// <value>
        /// The gestionnaire nom.
        /// </value>
        [Display(Name = "Gestionnaire *")]
        public string GestionnaireNom { get; set; }
        [Display(Name = "Regime de taxe ")]
        public List<AlbSelectListItem> RegimesTaxe { get; set; }
        public string RegimeTaxe { get; set; }
        [Display(Name = "CATNAT possible")]
        public bool SoumisCatNat { get; set; }
        public bool IsMonoRisque { get; set; }

        public string PartBenef { get; set; }
        public bool OppBenef { get; set; }

        public bool LTA { get; set; }

        public string Antecedent { get; set; }

        public string Description { get; set; }

        public int ValiditeOffre { get; set; }
        
        public DateTime? DateProjet { get; set; }

        public bool Relance { get; set; }

        public int RelanceValeur { get; set; }

        public int Preavis { get; set; }



        public ModeleFinOffreInfos ModeleFinOffreInfos { get; set; }
       // public ModeleFinOffreAnnotation ModeleFinOffreAnnotation { get; set; }
        #endregion

        public ModifierOffre_Index_MetaModel()
        {
            this.Descriptif = string.Empty;
            this.Cible = string.Empty;
            this.MotsClefs1 = new List<AlbSelectListItem>();
            this.MotsClefs2 = new List<AlbSelectListItem>();
            this.MotsClefs3 = new List<AlbSelectListItem>();
            this.Observations = string.Empty;
            this.Devises = new List<AlbSelectListItem>();
            this.Periodicites = new List<AlbSelectListItem>();
            this.Durees = new List<AlbSelectListItem>();
            this.IndicesReference = new List<AlbSelectListItem>();
            this.Valeur = string.Empty;
            this.NatureContrat = string.Empty;
            this.NaturesContrat = new List<AlbSelectListItem>();
            ModeleFinOffreInfos = new ModeleFinOffreInfos();
           // ModeleFinOffreAnnotation = new ModeleFinOffreAnnotation();
            //this.Intercalaire = false;
        }
    }
}