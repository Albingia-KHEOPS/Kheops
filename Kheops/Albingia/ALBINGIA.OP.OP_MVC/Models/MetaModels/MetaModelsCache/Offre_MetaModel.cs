using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.FinOffre;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Aperiteur;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Personnes;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache
{
    public class Offre_MetaModel
    {

        /// <summary>
        /// Gets or sets the code offre.
        /// </summary>
        /// <value>
        /// The code offre.
        /// </value>        
        public string CodeOffre { get; set; }

        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>        
        public BrancheDto Branche { get; set; }

        ///// <summary>
        ///// Gets or sets the branche.
        ///// </summary>
        ///// <value>
        ///// The branche.
        ///// </value>        
        //public enEtatOffres Etat { get; set; }

        ///// <summary>
        ///// Gets or sets the branche.
        ///// </summary>
        ///// <value>
        ///// The branche.
        ///// </value>        
        //public enSituationOffres Situation { get; set; }

        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>        
        public string Etat { get; set; }
        //ECM 03/04/2012    Changement des enum (Etat & Situation) en string 
        // A = OuvertEtValidable ; V = Validé ; R = Réalisé ; N = OuvertNonValidable ; vide = Indéterminé

        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>        
        public string Situation { get; set; }
        //ECM 03/04/2012    Changement des enum (Etat & Situation) en string 
        // A = OuvertEtValidable ; V = Validé ; R = Réalisé ; N = OuvertNonValidable ; vide = Indéterminé

        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>        
        public AdressePlatDto AdresseOffre { get; set; }

        /// <summary>
        /// Gets or sets the date saisie.
        /// </summary>
        /// <value>
        /// The date saisie.
        /// </value>        
        public DateTime? DateSaisie { get; set; }

        /// <summary>
        /// Gets or sets the date enregistrement.
        /// </summary>
        /// <value>
        /// The date enregistrement.
        /// </value>        
        public DateTime? DateEnregistrement { get; set; }

        /// <summary>
        /// Gets or sets the cabinet apporteur.
        /// </summary>
        /// <value>
        /// The cabinet apporteur.
        /// </value>        
        public CabinetCourtageDto CabinetApporteur { get; set; }

        /// <summary>
        /// Gets or sets the cabinet gestionnaire.
        /// </summary>
        /// <value>
        /// The cabinet gestionnaire.
        /// </value>        
        public CabinetCourtageDto CabinetGestionnaire { get; set; }

        /// <summary>
        /// Gets or sets the ref chez courtier.
        /// </summary>
        /// <value>
        /// The ref chez courtier.
        /// </value>        
        public String RefChezCourtier { get; set; }

        /// <summary>
        /// Gets or sets the code interlocuteur.
        /// </summary>
        /// <value>
        /// The code interlocuteur.
        /// </value>        
        public String CodeInterlocuteur { get; set; }

        public string NomInterlocuteur { get; set; }

        /// <summary>
        /// Gets or sets the preneur assurance.
        /// </summary>
        /// <value>
        /// The preneur assurance.
        /// </value>        
        public AssureDto PreneurAssurance { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>        
        public int? Version { get; set; }

        /// <summary>
        /// Gets or sets the motifs refus.
        /// </summary>
        /// <value>
        /// The motifs refus.
        /// </value>        
        public String MotifRefus { get; set; }

        /// <summary>
        /// Gets or sets the souscripteur.
        /// </summary>
        /// <value>
        /// The souscripteur.
        /// </value>        
        public SouscripteurDto Souscripteur { get; set; }

        /// <summary>
        /// Gets or sets the gestionnaire.
        /// </summary>
        /// <value>
        /// The gestionnaire.
        /// </value>        
        public GestionnaireDto Gestionnaire { get; set; }

        /// <summary>
        /// Gets or sets the mot cle1.
        /// </summary>
        /// <value>
        /// The mot cle1.
        /// </value>        
        public String MotCle1 { get; set; }

        /// <summary>
        /// Gets or sets the mot cle2.
        /// </summary>
        /// <value>
        /// The mot cle2.
        /// </value>        
        public String MotCle2 { get; set; }

        /// <summary>
        /// Gets or sets the mot cle3.
        /// </summary>
        /// <value>
        /// The mot cle3.
        /// </value>        
        public String MotCle3 { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>        
        public String Type { get; set; }

        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>        
        public String Descriptif { get; set; }

        /// <summary>
        /// Gets or sets the observation.
        /// </summary>
        /// <value>
        /// The observation.
        /// </value>        
        public String Observation { get; set; }

        /// <summary>
        /// Gets or sets the qualite.
        /// </summary>
        /// <value>
        /// The qualite.
        /// </value>        
        public String Qualite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Offre_MetaModel"/> is bonification.
        /// </summary>
        /// <value>
        ///   <c>true</c> if bonification; otherwise, <c>false</c>.
        /// </value>
        public bool Bonification { get; set; }

        /// <summary>
        /// Gets or sets the taux bonification.
        /// </summary>
        /// <value>
        /// The taux bonification.
        /// </value>
        public string TauxBonification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Offre_MetaModel"/> is anticipe.
        /// </summary>
        /// <value>
        ///   <c>true</c> if anticipe; otherwise, <c>false</c>.
        /// </value>
        public bool Anticipe { get; set; }

        /// <summary>
        /// Gets or sets the devise.
        /// </summary>
        /// <value>
        /// The devise.
        /// </value>
        public ParametreDto Devise { get; set; }

        /// <summary>
        /// Gets or sets the periodicite.
        /// </summary>
        /// <value>
        /// The periodicite.
        /// </value>
        public ParametreDto Periodicite { get; set; }
        /// <summary>
        /// Gets or sets the ModeleFinOffreInfos.
        /// </summary>
        /// <value>
        /// The ModeleFinOffreInfos.
        /// </value>
        public FinOffreInfosDto ModeleFinOffreInfos { get; set; }

        /// <summary>
        /// Gets or sets the echeance principale.
        /// </summary>
        /// <value>
        /// The echeance principale.
        /// </value>
        public DateTime? EcheancePrincipale { get; set; }

        /// <summary>
        /// Gets or sets the effet garantie.
        /// </summary>
        /// <value>
        /// The effet garantie.
        /// </value>
        public DateTime? DateEffetGarantie { get; set; }

        public DateTime? DateStatistique { get; set; }

        /// <summary>
        /// Gets or sets the heure effet.
        /// </summary>
        /// <value>
        /// The heure effet.
        /// </value>
        public TimeSpan? HeureEffet { get; set; }

        /// <summary>
        /// Gets or sets the duree garantie.
        /// </summary>
        /// <value>
        /// The duree garantie.
        /// </value>
        public int? DureeGarantie { get; set; }

        /// <summary>
        /// Gets or sets the date fin effet garantie.
        /// </summary>
        /// <value>
        /// The date fin effet garantie.
        /// </value>
        public DateTime? DateFinEffetGarantie { get; set; }

        /// <summary>
        /// Gets or sets the heure fin.
        /// </summary>
        /// <value>
        /// The heure fin.
        /// </value>
        public TimeSpan? HeureFin { get; set; }

        /// <summary>
        /// Gets or sets the unite de temps.
        /// </summary>
        /// <value>
        /// The unite de temps.
        /// </value>
        public ParametreDto UniteDeTemps { get; set; }

        /// <summary>
        /// Gets or sets the indice reference.
        /// </summary>
        /// <value>
        /// The indice reference.
        /// </value>
        public ParametreDto IndiceReference { get; set; }

        /// <summary>
        /// Gets or sets the valeur.
        /// </summary>
        /// <value>
        /// The valeur.
        /// </value>
        public decimal Valeur { get; set; }

        /// <summary>
        /// Gets or sets the nature contrat.
        /// </summary>
        /// <value>
        /// The nature contrat.
        /// </value>
        public ParametreDto NatureContrat { get; set; }

        /// <summary>
        /// Gets or sets the part albingia.
        /// </summary>
        /// <value>
        /// The part albingia.
        /// </value>
        public decimal? PartAlbingia { get; set; }

        /// <summary>
        /// Gets or sets the couverture.
        /// </summary>
        /// <value>
        /// The couverture.
        /// </value>
        public int? Couverture { get; set; }

        /// <summary>
        /// Gets or sets the aperiteur.
        /// </summary>
        /// <value>
        /// The aperiteur.
        /// </value>
        public AperiteurDto Aperiteur { get; set; }

        /// <summary>
        /// Gets or sets the part aperiteur.
        /// </summary>
        /// <value>
        /// The part aperiteur.
        /// </value>
        public decimal? PartAperiteur { get; set; }

        /// <summary>
        /// Gets or sets the frais aperition.
        /// </summary>
        /// <value>
        /// The frais aperition.
        /// </value>
        public decimal? FraisAperition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [intercalaire courtier].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [intercalaire courtier]; otherwise, <c>false</c>.
        /// </value>
        public bool IntercalaireCourtier { get; set; }

        /// <summary>
        /// Gets or sets the risque.
        /// </summary>
        /// <value>
        /// The risque.
        /// </value>
        public RisqueDto Risque { get; set; }

        public List<RisqueDto> Risques { get; set; }

        public DoubleSaisie_MetaModel DblSaisieAutresOffres { get; set; }

        public List<CabinetAutreDto> CabinetAutres { get; set; }

        public bool HasDoubleSaisie { get; set; }

        public string CodeRegime { get; set; }
        //public string LibelleRegime { get; set; }
        public string SoumisCatNat { get; set; }
        public string SouscripteurCode { get; set; }
        public string SouscripteurNom { get; set; }
        public string GestionnaireNom { get; set; }
        public string GestionnaireCode { get; set; }
        //public decimal MontantReference { get; set; }
        //public string Indexation { get; set; }
        //public string LCI { get; set; }
        //public string Assiette { get; set; }
        //public string Franchise { get; set; }
        //public int Preavis { get; set; }
        //public string CodeAction { get; set; }
        //public string LibelleAction { get; set; }
        public string LibelleEtat { get; set; }
        //public string LibelleSituation { get; set; }
        public int DateSituationJour { get; set; }
        public int DateSituationMois { get; set; }
        public int DateSituationAnnee { get; set; }
        public DateTime? DateSituation { get; set; }
        //public string CodeUsrCreateur { get; set; }
        //public string NomUsrCreateur { get; set; }
        //public string CodeUsrModificateur { get; set; }
        //public string NomUsrModificateur { get; set; }
        public DateTime? DateMAJ { get; set; }
        public bool IsMonoRisque { get; set; }
        public string PartBenef { get; set; }
        public Int64 NbAssuAdditionnel { get; set; }
        public bool OppBenef { get; set; }
        public Int64 CountRsq { get; set; }
        public bool LTA { get; set; }

        public static explicit operator Offre_MetaModel(OffreDto offreDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<OffreDto, Offre_MetaModel>().Map(offreDto);
        }

        public OffreDto ToOffreDto()
        {
            OffreDto result = ObjectMapperManager.DefaultInstance.GetMapper<Offre_MetaModel, OffreDto>().Map(this);
            result.Bonification.Bonification = this.Bonification;
            result.Bonification.TauxBonification = this.TauxBonification;
            result.Bonification.Anticipe = this.Anticipe;
            return result;
        }
        public void LoadOffre(OffreDto offreDto)
        {
            if (offreDto != null)
            {
                CodeOffre = offreDto.CodeOffre;
                Branche = offreDto.Branche;
                CabinetApporteur = offreDto.CabinetApporteur;

                DateSaisie = offreDto.DateSaisie;
                PreneurAssurance = offreDto.PreneurAssurance;
                Version = offreDto.Version;
                MotifRefus = offreDto.MotifRefus;
                Type = offreDto.Type;
                CodeInterlocuteur = offreDto.CodeInterlocuteur;
                NomInterlocuteur = offreDto.NomInterlocuteur;
                MotCle1 = offreDto.MotCle1;
                MotCle2 = offreDto.MotCle2;
                MotCle3 = offreDto.MotCle3;
                Descriptif = offreDto.Descriptif;
                Observation = offreDto.Observation;
                Qualite = offreDto.Qualite;
                Bonification = offreDto.Bonification == null ? false : offreDto.Bonification.Bonification;
                TauxBonification = offreDto.Bonification == null ? string.Empty : offreDto.Bonification.TauxBonification;
                Anticipe = offreDto.Bonification == null ? false : offreDto.Bonification.Anticipe;
                Devise = offreDto.Devise;
                Etat = offreDto.Etat;
                Situation = offreDto.Situation;
                AdresseOffre = offreDto.AdresseOffre;
                DateEnregistrement = offreDto.DateEnregistrement;
                CabinetGestionnaire = offreDto.CabinetGestionnaire;
                CabinetAutres = offreDto.CabinetAutres != null ? offreDto.CabinetAutres.ToList() : new List<CabinetAutreDto>();
                RefChezCourtier = offreDto.RefChezCourtier;
                Souscripteur = offreDto.Souscripteur;
                Gestionnaire = offreDto.Gestionnaire;
                Periodicite = offreDto.Periodicite;
                EcheancePrincipale = offreDto.EcheancePrincipale;
                DateEffetGarantie = offreDto.DateEffetGarantie;
                HeureEffet = offreDto.HeureEffet;
                DateFinEffetGarantie = offreDto.DateFinEffetGarantie;
                HeureFin = offreDto.HeureFin;
                DureeGarantie = offreDto.DureeGarantie;
                UniteDeTemps = offreDto.UniteDeTemps;
                IndiceReference = offreDto.IndiceReference;
                Valeur = offreDto.Valeur;
                NatureContrat = offreDto.NatureContrat;
                PartAlbingia = offreDto.PartAlbingia;
                Couverture = offreDto.Couverture;
                Aperiteur = offreDto.Aperiteur;
                PartAperiteur = offreDto.PartAperiteur;
                FraisAperition = offreDto.FraisAperition;
                IntercalaireCourtier = offreDto.IntercalaireCourtier;
                Risque = offreDto.Risque;
                Risques = offreDto.Risques != null ? offreDto.Risques.ToList() : new List<RisqueDto>();
                HasDoubleSaisie = offreDto.HasDoubleSaisie;
                CodeRegime = offreDto.CodeRegime;
                //LibelleRegime = offreDto.LibelleRegime;
                SoumisCatNat = offreDto.SoumisCatNat;
                SouscripteurCode = offreDto.Souscripteur != null ? offreDto.Souscripteur.Code : string.Empty;
                SouscripteurNom = offreDto.Souscripteur != null ? offreDto.Souscripteur.Nom : string.Empty;
                GestionnaireCode = offreDto.Gestionnaire != null ? offreDto.Gestionnaire.Id : string.Empty;
                GestionnaireNom = offreDto.Gestionnaire != null ? offreDto.Gestionnaire.Nom : string.Empty;
                //MontantReference = offreDto.MontantReference;
                //Indexation = offreDto.Indexation;
                //LCI = offreDto.LCI;
                //Assiette = offreDto.Assiette;
                //Franchise = offreDto.Franchise;               
                //CodeAction = offreDto.CodeAction;
                //LibelleAction = offreDto.LibelleAction;
                //Preavis = offreDto.Preavis;
                //LibelleSituation = offreDto.LibelleSituation;
                LibelleEtat = offreDto.EtatLib;
                //if (offreDto.DateSituationAnnee != 0 && offreDto.DateSituationMois != 0 && offreDto.DateSituationJour != 0)
                //    DateSituation = new DateTime(offreDto.DateSituationAnnee, offreDto.DateSituationMois, offreDto.DateSituationJour);
                //CodeUsrCreateur = offreDto.CodeUsrCreateur;
                //NomUsrCreateur = offreDto.NomUsrCreateur;
                //CodeUsrModificateur = offreDto.CodeUsrModificateur;
                //NomUsrModificateur = offreDto.NomUsrModificateur;
                DateMAJ = offreDto.DateMAJ;
                IsMonoRisque = offreDto.IsMonoRisque;
                DateStatistique = offreDto.DateStatistique;
                PartBenef = offreDto.PartBenef;
                NbAssuAdditionnel = offreDto.NbAssuresAdditionnels;
                OppBenef = offreDto.HasOppBenef;
                LTA = offreDto.LTA;
            }
        }

        public void LoadInfosOffre(InfosBaseDto InfosBaseDto)
        {
            CodeOffre = InfosBaseDto.CodeOffre;
            Version = InfosBaseDto.Version;
            Type = InfosBaseDto.Type;
            Branche = ObjectMapperManager.DefaultInstance.GetMapper<BrancheDto, BrancheDto>().Map(InfosBaseDto.Branche);
            CabinetGestionnaire = ObjectMapperManager.DefaultInstance.GetMapper<CabinetCourtageDto, CabinetCourtageDto>().Map(InfosBaseDto.CabinetGestionnaire);
            Descriptif = InfosBaseDto.Descriptif;
            CodeInterlocuteur = InfosBaseDto.CabinetGestionnaire.Code.ToString();
            NomInterlocuteur = InfosBaseDto.CabinetGestionnaire.Inspecteur;
            PreneurAssurance = ObjectMapperManager.DefaultInstance.GetMapper<AssureDto, AssureDto>().Map(InfosBaseDto.PreneurAssurance);
            Periodicite = new ParametreDto() { Code = InfosBaseDto.Periodicite };
            IndiceReference = new ParametreDto() { Code = InfosBaseDto.IndiceReference };

            var dateDeb = InfosBaseDto.DateEffetAnnee*10000 + InfosBaseDto.DateEffetMois*100 + InfosBaseDto.DateEffetJour;
            DateEffetGarantie = AlbConvert.ConvertIntToDateHour((long)dateDeb * 10000 + InfosBaseDto.DateEffetHeure);

            var dateFin = InfosBaseDto.FinEffetAnnee * 10000 + InfosBaseDto.FinEffetMois * 100 + InfosBaseDto.FinEffetJour;
            if (dateFin != 0)
                DateFinEffetGarantie = AlbConvert.ConvertIntToDateHour((long)dateFin * 10000 + InfosBaseDto.FinEffetHeure);
            
            Etat = InfosBaseDto.Etat;
        }
    }
}
