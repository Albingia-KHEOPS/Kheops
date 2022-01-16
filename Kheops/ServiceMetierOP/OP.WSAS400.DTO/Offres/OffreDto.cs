using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Personnes;
using OP.WSAS400.DTO.Offres.Aperiteur;
using OP.WSAS400.DTO.Offres.Risque;
using System.Data.Linq.Mapping;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.FinOffre;

namespace OP.WSAS400.DTO.Offres
{
    /// <summary>
    /// DTO de l offre
    /// </summary>
    [DataContract]
    public class OffreDto //: _Offre_Base
    {
        [Column(Name = "NATURECONTRAT")]
        public string NatureContratStr { get; set; }

        [Column(Name = "INTERCALAIRE")]
        public string IntercalaireStr { get; set; }

        [DataMember]
        public string ContratMere { get; set; }
        [DataMember]
        public int NumAvenant { get; set; }
        [DataMember]
        public DateTime? DateCreation { get; set; }
        [DataMember]
        public string TypeAvt { get; set; }


        [DataMember]
        public string CodeOffreCopy { get; set; }
        [DataMember]
        public string VersionCopy { get; set; }
        [DataMember]
        public bool CopyMode { get; set; }
        /// <summary>
        /// Gets or sets the code offre.
        /// </summary>
        /// <value>
        /// The code offre.
        /// </value>
        [DataMember]
        [Column(Name="CODEOFFRE")]
        public string CodeOffre { get; set; }

        [DataMember]
        public string KheopsStatut { get; set; }

        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>
        [DataMember]
        public BrancheDto Branche { get; set; }
        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>
        /// 
        [DataMember]
        public CibleDto Cible { get; set; }
        ///// <summary>
        ///// Gets or sets the Cible.
        ///// </summary>
        ///// <value>
        ///// The Cible.
        ///// </value>
     

        ///// <summary>
        ///// Gets or sets the branche.
        ///// </summary>
        ///// <value>
        ///// The branche.
        ///// </value>
        //[DataMember]
        //public enSituationOffres Situation { get; set; }

        //ECM 03/04/2012   
        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>
        [DataMember]
        [Column(Name="ETAT")]
        public string Etat { get; set; }        // A = OuvertEtValidable ; V = Validé ; R = Réalisé ; N = OuvertNonValidable ; vide = Indéterminé

        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>
        [DataMember]
        [Column(Name="CODESIT")]
        public string Situation { get; set; }   // A = OuvertEtValidable ; V = Validé ; R = Réalisé ; N = OuvertNonValidable ; vide = Indéterminé

        [DataMember]
        [Column(Name="LIBSIT")]
        public string SituationLib { get; set; } 
        [DataMember]
        public string TypeAccord { get; set; }
        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>
        ///    

        /// <summary>
        /// Gets or sets the date saisie.
        /// </summary>
        /// <value>
        /// The date saisie.
        /// </value>
        [DataMember]
        public DateTime? DateSaisie { get; set; }

        /// <summary>
        /// Gets or sets the date enregistrement.
        /// </summary>
        /// <value>
        /// The date enregistrement.
        /// </value>
        [DataMember]
        public DateTime? DateEnregistrement { get; set; }

        /// <summary>
        /// Gets or sets the cabinet apporteur.
        /// </summary>
        /// <value>
        /// The cabinet apporteur.
        /// </value>
        [DataMember]
        public CabinetCourtageDto CabinetApporteur { get; set; }

        /// <summary>
        /// Gets or sets the cabinet gestionnaire.
        /// </summary>
        /// <value>
        /// The cabinet gestionnaire.
        /// </value>
        [DataMember]
        public CabinetCourtageDto CabinetGestionnaire { get; set; }

        /// <summary>
        /// Gets or sets the cabinet autres.
        /// </summary>
        /// <value>
        /// The cabinet autres.
        /// </value>
        [DataMember]
        public List<CabinetAutreDto> CabinetAutres { get; set; }



        /// <summary>
        /// Gets or sets the ref chez courtier.
        /// </summary>
        /// <value>
        /// The ref chez courtier.
        /// </value>
        [DataMember]
        public String RefChezCourtier { get; set; }

        /// <summary>
        /// Gets or sets the code interlocuteur.
        /// </summary>
        /// <value>
        /// The code interlocuteur.
        /// </value>
        [DataMember]
        public String CodeInterlocuteur { get; set; }
        [DataMember]
        public string NomInterlocuteur { get; set; }

        /// <summary>
        /// Gets or sets the preneur assurance.
        /// </summary>
        /// <value>
        /// The preneur assurance.
        /// </value>
        [DataMember]
        public AssureDto PreneurAssurance { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [DataMember]
        [Column(Name="VERSION")]
        public int? Version { get; set; }

        /// <summary>
        /// Gets or sets the motifs refus.
        /// </summary>
        /// <value>
        /// The motifs refus.
        /// </value>
        [DataMember]
        [Column(Name="MOTIFREFUS")]
        public String MotifRefus { get; set; }

        /// <summary>
        /// Gets or sets the souscripteur.
        /// </summary>
        /// <value>
        /// The souscripteur.
        /// </value>
        [DataMember]
        public SouscripteurDto Souscripteur { get; set; }

        /// <summary>
        /// Gets or sets the gestionnaire.
        /// </summary>
        /// <value>
        /// The gestionnaire.
        /// </value>
        [DataMember]
        public GestionnaireDto Gestionnaire { get; set; }

        /// <summary>
        /// Gets or sets the mot cle1.
        /// </summary>
        /// <value>
        /// The mot cle1.
        /// </value>
        [DataMember]
        public String MotCle1 { get; set; }

        /// <summary>
        /// Gets or sets the mot cle2.
        /// </summary>
        /// <value>
        /// The mot cle2.
        /// </value>
        [DataMember]
        public String MotCle2 { get; set; }

        /// <summary>
        /// Gets or sets the mot cle3.
        /// </summary>
        /// <value>
        /// The mot cle3.
        /// </value>
        [DataMember]
        public String MotCle3 { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public String Type { get; set; }

        /// <summary>
        /// Gets or sets the Fin Offre Infos.
        /// </summary>
        /// <value>
        /// The Fin Offre Infos.
        /// </value>
        [DataMember]
        public FinOffreInfosDto ModeleFinOffreInfos { get; set; }

        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>
        [DataMember]
        [Column(Name="DESCRIPTIF")]
        public String Descriptif { get; set; }

        /// <summary>
        /// Gets or sets the observation.
        /// </summary>
        /// <value>
        /// The observation.
        /// </value>
        [DataMember]
        public String Observation { get; set; }

        /// <summary>
        /// Gets or sets the qualite.
        /// </summary>
        /// <value>
        /// The qualite.
        /// </value>
        [DataMember]
        [Column(Name="CODEQUALITE")]
        public String Qualite { get; set; }

         [DataMember]
        [Column(Name="QUALITELIB")]
        public String QualiteLib { get; set; }
        
        /// <summary>
        /// Gets or sets the bonification.
        /// </summary>
        /// <value>
        /// The bonification.
        /// </value>
        [DataMember]
        public BonificationsDto Bonification { get; set; }

        #region SaisieOffre

        /// <summary>
        /// Gets or sets the devise.
        /// </summary>
        /// <value>
        /// The devise.
        /// </value>
        [DataMember]
        public ParametreDto Devise { get; set; }

        /// <summary>
        /// Gets or sets the periodicite.
        /// </summary>
        /// <value>
        /// The periodicite.
        /// </value>
        [DataMember]
        public ParametreDto Periodicite { get; set; }

        /// <summary>
        /// Gets or sets the echeance principale.
        /// </summary>
        /// <value>
        /// The echeance principale.
        /// </value>
        [DataMember]
        public DateTime? EcheancePrincipale { get; set; }

        /// <summary>
        /// Gets or sets the effet garantie.
        /// </summary>
        /// <value>
        /// The effet garantie.
        /// </value>
        [DataMember]
        public DateTime? DateEffetGarantie { get; set; }

        /// <summary>
        /// Gets or sets the heure effet.
        /// </summary>
        /// <value>
        /// The heure effet.
        /// </value>
        [DataMember]
        public TimeSpan? HeureEffet { get; set; }

        /// <summary>
        /// Gets or sets the fin effet.
        /// </summary>
        /// <value>
        /// The fin effet.
        /// </value>
        [DataMember]
        public DateTime? DateFinEffetGarantie { get; set; }

        /// <summary>
        /// Gets or sets the heure fin.
        /// </summary>
        /// <value>
        /// The heure fin.
        /// </value>
        [DataMember]
        public TimeSpan? HeureFin { get; set; }

        /// <summary>
        /// Gets or sets the duree.
        /// </summary>
        /// <value>
        /// The duree.
        /// </value>
        [DataMember]
        public int? DureeGarantie { get; set; }

        [DataMember]
        public DateTime? EffetGarantie { get; set; }
        /// <summary>
        /// Gets or sets the duree unite.
        /// </summary>
        /// <value>
        /// The duree unite.
        /// </value>
        [DataMember]
        public ParametreDto UniteDeTemps { get; set; }

        /// <summary>
        /// Gets or sets the indice.
        /// </summary>
        /// <value>
        /// The indice.
        /// </value>
        [DataMember]
        public ParametreDto IndiceReference { get; set; }

        /// <summary>
        /// Gets or sets the valeur.
        /// </summary>
        /// <value>
        /// The valeur.
        /// </value>
        [DataMember]
        public decimal Valeur { get; set; }

        /// <summary>
        /// Gets or sets the nature contrat.
        /// </summary>
        /// <value>
        /// The nature contrat.
        /// </value>
        [DataMember]
        public ParametreDto NatureContrat { get; set; }

        /// <summary>
        /// Gets or sets the part albingia.
        /// </summary>
        /// <value>
        /// The part albingia.
        /// </value>
        [DataMember]
        public decimal? PartAlbingia { get; set; }

        /// <summary>
        /// Gets or sets the couverture.
        /// </summary>
        /// <value>
        /// The couverture.
        /// </value>
        [DataMember]
        public int? Couverture { get; set; }

        /// <summary>
        /// Gets or sets the aperiteur.
        /// </summary>
        /// <value>
        /// The aperiteur.
        /// </value>
        [DataMember]
        public AperiteurDto Aperiteur { get; set; }

        /// <summary>
        /// Gets or sets the part aperiteur.
        /// </summary>
        /// <value>
        /// The part aperiteur.
        /// </value>
        [DataMember]
        public decimal? PartAperiteur { get; set; }

        /// <summary>
        /// Gets or sets the frais aperiteur.
        /// </summary>
        /// <value>
        /// The frais aperiteur.
        /// </value>
        [DataMember]
        public decimal? FraisAperition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OffreDto"/> is intercalaire.
        /// </summary>
        /// <value>
        ///   <c>true</c> if intercalaire; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IntercalaireCourtier { get; set; }

        /// <summary>
        /// Gets or sets the risque.
        /// </summary>
        /// <value>
        /// The risque.
        /// </value>
        [DataMember]
        public RisqueDto Risque { get; set; }

        /// <summary>
        /// Gets or sets the risques.
        /// </summary>
        /// <value>
        /// The risques.
        /// </value>
        [DataMember]
        public List<RisqueDto> Risques { get; set; }

        #endregion

        [DataMember]
        public DateTime? DateMAJ { get; set; }

        [DataMember]
        public DateTime? DateStatistique { get; set; }
        [DataMember]
        public bool HasDoubleSaisie { get; set; }     
        [DataMember]
        public string CodeRegime { get; set; }
        //[DataMember]
        //public string LibelleRegime { get; set; }
        [DataMember]
        public string SoumisCatNat { get; set; }
        //[DataMember]
        //public decimal MontantReference { get; set; }
        //[DataMember]
        //public string Indexation { get; set; }
        //[DataMember]
        //public string LCI { get; set; }
        //[DataMember]
        //public string Assiette { get; set; }
        //[DataMember]
        //public string Franchise { get; set; }
        //[DataMember]
        //public int Preavis { get; set; }
        //[DataMember]
        //public string CodeAction { get; set; }
        //[DataMember]
        //public string LibelleAction { get; set; }
        [DataMember]
        [Column(Name="LIBETAT")]
        public string EtatLib { get; set; }
        //[DataMember]
        //public string LibelleSituation { get; set; }
        //[DataMember]
        //public int DateSituationJour { get; set; }
        //[DataMember]
        //public int DateSituationMois { get; set; }
        //[DataMember]
        //public int DateSituationAnnee { get; set; }
        //[DataMember]
        //public string CodeUsrCreateur { get; set; }
        //[DataMember]
        //public string NomUsrCreateur { get; set; }
        //[DataMember]
        //public string CodeUsrModificateur { get; set; }
        //[DataMember]
        //public string NomUsrModificateur { get; set; }
        [DataMember]
        public bool IsMonoRisque { get; set; }

        [DataMember]
        [Column(Name = "CODECATEGORIE")]
        public string CodeCategorie { get; set; }

        [DataMember]
        [Column(Name = "CODESOUSBRANCE")]
        public string CodeSousBranche { get; set; }

        #region adresse
        [DataMember]
        public AdressePlatDto AdresseOffre { get; set; }
        [DataMember]
        public int IdAdresseOffre { get; set; }
        [DataMember]
        public string RefCourtier { get; set; }
        #endregion

        [DataMember]
        public InterlocuteurDto Interlocuteur { get; set; }
        
        //private List<ElementAssureContratDto> elementAssures;
        [DataMember]
        public List<ElementAssureContratDto> ElementAssures { get; set; }

        [DataMember]
        public int Compteur { get; set; }
        //public List<ElementAssureContratDto> ElementAssures
        //{
        //    get
        //    {
        //        List<ElementAssureContratDto> result = new List<ElementAssureContratDto>();
        //        if (elementAssures != null)
        //        {
        //            result = elementAssures.ToList();
        //        }
        //        return result;
        //    }
        //}

        //public void AjouterElementAssure(ElementAssureContratDto argElementAssure)
        //{
        //    if (elementAssures == null)
        //    {
        //        elementAssures = new List<ElementAssureContratDto>();
        //    }

        //    if (argElementAssure.ElementPrincipal)
        //    {
        //        elementAssures.Where(x => x.ElementPrincipal).Select(x => x.ElementPrincipal = false);
        //    }
        //    elementAssures.Add(argElementAssure);
        //}

        [DataMember]
        public string PartBenef { get; set; }
        [DataMember]
        public Int32 GenerDoc { get; set; }

        [DataMember]
        public Int64 NbAssuresAdditionnels { get; set; }

        [DataMember]
        public Int64 NumAvnExterne { get; set; }

        [DataMember]
        public bool HasOppBenef { get; set; }

        [DataMember]
        public bool HasSusp { get; set; }
        [DataMember]
        public DateTime? DateFinSusp { get; set; }

        [DataMember]
        public long RegulId { get; set; }

        [DataMember]
        public bool HasPrimeSoldee { get; set; }

        [DataMember]
        public bool LTA { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="OffreDto"/> class.
        /// </summary>
        public OffreDto()
        {
            this.CodeOffre = String.Empty;
            this.Branche = new BrancheDto();
            this.CabinetApporteur = new CabinetCourtageDto();
            this.DateSaisie = null;
            this.DateMAJ = null;
            this.PreneurAssurance = new AssureDto();
            this.PreneurAssurance.NomSecondaires = new List<string>();
            this.Version = _DTO_Base._undefinedInt;
            this.MotifRefus = String.Empty;
            this.Type = String.Empty;
            this.CodeInterlocuteur = String.Empty;
            this.MotCle1 = String.Empty;
            this.MotCle2 = String.Empty;
            this.MotCle3 = String.Empty;
            this.Descriptif = String.Empty;
            this.Observation = String.Empty;
            this.Qualite = String.Empty;
            this.Bonification = new BonificationsDto();
            this.Devise = new ParametreDto();
            this.Periodicite = new ParametreDto();
            this.EcheancePrincipale = null;
            this.DateEffetGarantie = null;
            this.HeureEffet = null;
            this.DateFinEffetGarantie = null;
            this.HeureFin = null;
            this.DureeGarantie = _DTO_Base._undefinedInt;
            this.UniteDeTemps = new ParametreDto();
            this.IndiceReference = new ParametreDto();
            this.Valeur = _DTO_Base._undefinedInt;
            this.NatureContrat = new ParametreDto();
            this.PartAlbingia = _DTO_Base._undefinedInt;
            this.Couverture = _DTO_Base._undefinedInt;
            this.Aperiteur = new AperiteurDto();
            this.PartAperiteur = _DTO_Base._undefinedInt;
            this.FraisAperition = _DTO_Base._undefinedInt;
            this.IntercalaireCourtier = false;
            this.Risque = new RisqueDto();
            this.Risques = new List<RisqueDto>();
            this.CabinetAutres = new List<CabinetAutreDto>();
            this.AdresseOffre = new AdressePlatDto();
            this.ModeleFinOffreInfos = new FinOffreInfosDto();
            //this.HasSusp = false;
        }

        
    }
}