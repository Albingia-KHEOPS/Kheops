using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Affaire
{

    public class Affaire
    {
        private const string ErrorTextMandatory = "la valeur est obligatoire";
        public static readonly Regex IpbRegex = new Regex(@"^[A-Z0-9]{1,9}$", RegexOptions.Singleline | RegexOptions.Compiled);

        public TypologieModele Typologie =>
            this.NatureContrat?.Code == NatureAffaireCode.Coassurance.AsString()
                ? TypologieModele.Coassurance
                : this.IntercalaireExiste
                    ? TypologieModele.ITC
                    : TypologieModele.Standard;
        public NatureAffaire NatureContrat { get; set; }

        public IEnumerable<ValidationErrorBasic> ValidateFields()
        {
            if (this.Etat == Domain.Affaire.EtatAffaire.Validee) {
                if (!DateEffet.HasValue) {
                    yield return new ValidationErrorBasic(nameof(DateEffet), ErrorTextMandatory);
                }
                if (!DateAccord.HasValue) {
                    yield return new ValidationErrorBasic(nameof(DateAccord), ErrorTextMandatory);
                }
                if (!Periode.Debut.HasValue) {
                    yield return new ValidationErrorBasic(nameof(Periode.Debut), ErrorTextMandatory);
                }
                if (!Periode.Fin.HasValue) {
                    yield return new ValidationErrorBasic(nameof(Periode.Fin), ErrorTextMandatory);
                }
                if (Souscripteur is null) {
                    yield return new ValidationErrorBasic(nameof(Souscripteur), ErrorTextMandatory);
                } else if (!Souscripteur.IsSouscripteur) {
                    yield return new ValidationErrorBasic(nameof(Souscripteur), "doit est marqué comme souscripteur");

                }
                if (Gestionnaire is null) {
                    yield return new ValidationErrorBasic(nameof(Gestionnaire), ErrorTextMandatory);
                } else if (!Gestionnaire.IsGestionnaire) {
                    yield return new ValidationErrorBasic(nameof(Gestionnaire), "doit est marqué comme gestionnaire");
                }
            }

        }
        
        virtual public DateTime? DateSaisie { get; set; }
        virtual public DateTime? DateSaisieOffre { get; set; }
        virtual public DateTime? DateEffet { get; set; }
        virtual public DateTime DateModeleApplicable {
            get
            {
                if (TypeAffaire == AffaireType.Offre)
                {
                    return DateSaisie ?? DateTime.MinValue;
                }
                return DateSaisieOffre ?? DateEffet.Value;
            } 
        }

        virtual public DateTime? DateAccord { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public DateTime DateModification { get; set; }
        virtual public string CodeUserCreate { get; set; }
        virtual public string CodeUserUpdate { get; set; }
        virtual public DateTime? DateFin { get; set; }
        virtual public DateTime? DateFinCalculee {
            get {
                var unit = UniteeDuree?.Code.AsEnum<UniteDureeValeur>() ?? UniteDureeValeur.Empty;
                return DateFin ?? (
                    (Duree > 0 && unit != UniteDureeValeur.Empty)
                        ? DateEffet?.AddUnit(Duree, unit)
                        : null 
                );
            }
        }

        virtual public DateTime? DateSituation { get; set; }
        virtual public UniteDuree UniteeDuree { get; set; }
        virtual public int Duree { get; set; }

        virtual public string Descriptif { get; set; }
        virtual public string CodeAffaire { get; set; }
        virtual public string CodeOffre { get; set; }
        virtual public bool IsKheops { get; set; }
        virtual public int NumeroAliment { get; set; }
        virtual public AffaireType TypeAffaire { get; set; }
        
        virtual public DateTime? DateValidation { get; set; }
        virtual public int DelaisRelanceJours { get; set; }

        /// <summary>
        /// ' ', 'O', 'N'
        /// </summary>
        virtual public Utilisateur Souscripteur { get; set; }
        virtual public Utilisateur Gestionnaire { get; set; }
        /// <summary>
        /// (V validé / A / N / R)
        /// </summary>
        virtual public EtatAffaire Etat { get; set; }
        virtual public Etat EtatAffaire { get; set; }
        virtual public Situation SituationAffaire { get; set; }
        virtual public string Observations { get; set; }
        virtual public string DesignationAvenant { get; set; }
        virtual public Int64 NumChronoOsbv { get; set; }
        virtual public SituationAffaire Situation { get; set; }
        virtual public string CodeMotifSituation { get; set; }
        virtual public MotifSituation MotifSituation { get; set; }
        virtual public MotifAvenant MotifAvenant { get; set; }
        virtual public MotifResiliation MotifResiliation { get; set; }
        virtual public Regularisation.Regularisation Regularisation { get; set; }
        virtual public TypeTraitement TypeTraitement { get; set; }
        virtual public TypeTraitement DernierTraitement { get; set; }

        virtual public bool IsValidated => Etat == Domain.Affaire.EtatAffaire.Realisee || Etat == Domain.Affaire.EtatAffaire.Validee;

        virtual public Cible Cible { get; set; }
        virtual public CibleCatego CibleCategorie { get; set; }

        virtual public Branche Branche { get; set; }
        virtual public string SousBranche { get; set; }

        virtual public string Categorie { get; set; }
        virtual public Devise Devise { get; set; }
        virtual public Intervenant Interlocuteur { get; set; }
        virtual public AffaireMetadata Metadata { get; set; }
        virtual public Assure Preneur { get; set; }
        virtual public Courtier CourtierGestionnaire { get; set; }
        virtual public Courtier CourtierApporteur { get; set; }
        virtual public Courtier CourtierPayeur { get; set; }
        virtual public string ReferenceCourtier { get; set; }
        virtual public bool? IsAttenteDocumentsCourtier { get; set; }
        virtual public bool IsHisto { get; set; }
        virtual public int NumeroAvenant { get; set; }
        virtual public int PreavisMois { get; set; }
        virtual public Indice IndiceReference { get; set; }
        virtual public decimal ValeurIndiceActualisee { get; set; }
        virtual public decimal ValeurIndiceOrigine { get; set; }
        virtual public bool SoumisCatNat { get; set; }
        virtual public bool IntercalaireExiste { get; set; }
        virtual public int IdAdresse { get; set; }
        virtual public decimal BaseCATNATCalculee { get; set; }
        public Periode Periode { get; set; }
        public Echeance ProchaineEcheance { get; set; }
        public DateTime? DateEffetAvenant { get; set; }
        public Periodicite Periodicite { get; set; }
        public Expressions Expressions { get; set; } = new Expressions();
        public DateTime? Echeance { get; set; }
        virtual public NatureTravauxAffaire NatureTravaux { get; set; }
        virtual public decimal TauxCommission { get; set; }
        virtual public decimal TauxCommissionCATNAT { get; set; }
        virtual public TypeAccord TypeAccord { get; set; }
        virtual public AffaireId OffreOrigine { get; set; }
        virtual public int FraisAccessoires { get; set; }
        virtual public ApplicationFraisAccessoire ApplicationFraisAccessoire { get; set; }
        virtual public TypePolice TypePolice { get; set; }
        virtual public Encaissement Encaissement { get; set; }
        virtual public RegimeTaxe RegimeTaxe { get; set; }
        virtual public CodeSTOP CodeSTOP { get; set; }
        virtual public decimal PartAlbingia { get; set; }
        /// <summary>
        /// JDTFF
        /// </summary>
        virtual public decimal MontantReference1 { get; set; }
        /// <summary>
        /// JDTMC
        /// </summary>
        virtual public decimal MontantReference2 { get; set; }

        virtual public TarifAffaire TarifAffaireLCI { get; set; }

        virtual public TarifAffaire TarifAffaireFRH { get; set; }

        virtual public TypeMontant TypeMontantAssiette { get; set; }

        virtual public Gareat GareatTheorique { get; set; }
        virtual public Gareat GareatRetenu { get; set; }

        public void ChangeTarifsConditions(TarifAffaire tarifLCI, TarifAffaire tarifFRH, TypeMontant typeMontant = TypeMontant.HT) {
            TarifAffaireLCI.Reset(tarifLCI);
            TarifAffaireFRH.Reset(tarifFRH);
            if (TypeMontantAssiette != typeMontant) {
                TypeMontantAssiette = typeMontant;
            }
        }

        virtual public string TypeRemiseEnVigueur { get; set; }
        //[DataMember]
        //[Column(Name = "PERIODICITECODE")]
        //public string PeriodiciteCode { get; set; }
        //[DataMember]
        //[Column(Name = "PERIODICITENOM")]
        //public string PeriodiciteNom { get; set; }
        //[DataMember]
        //[DataMember]
        //[Column(Name = "CODEMOTSCLEF1")]
        //public string CodeMotsClef1 { get; set; }
        //[DataMember]
        //[Column(Name = "CODEMOTSCLEF2")]
        //public string CodeMotsClef2 { get; set; }
        //[DataMember]
        //[Column(Name = "CODEMOTSCLEF3")]
        //public string CodeMotsClef3 { get; set; }
        //[DataMember]
        //[Column(Name = "PREAVIS")]
        //public int Preavis { get; set; }
        //[DataMember]
        //[Column(Name = "FINEFFETJOUR")]
        //public Int16 FinEffetJour { get; set; }
        //[DataMember]
        //[Column(Name = "FINEFFETMOIS")]
        //public Int16 FinEffetMois { get; set; }
        //[DataMember]
        //[Column(Name = "FINEFFETANNEE")]
        //public Int16 FinEffetAnnee { get; set; }
        //[DataMember]
        //[Column(Name = "FINEFFETHEURE")]
        //public int FinEffetHeure { get; set; }
        //[DataMember]
        //[Column(Name = "DUREEGARANTIE")]
        //public int DureeGarantie { get; set; }
        //[DataMember]
        //[Column(Name = "UNITETEMPS")]
        //public string UniteDeTemps { get; set; }
        //[DataMember]
        //[Column(Name = "DATESTATISTIQUE")]
        //public int DateStatistique { get; set; }
        //[DataMember]
        //[Column(Name = "INDICEREFERENCE")]
        //public string IndiceReference { get; set; }
        //[DataMember]
        //[Column(Name = "LIBINDICE")]
        //public string LibelleIndicReference { get; set; }
        //[DataMember]
        //[Column(Name = "VALEUR")]
        //public Single Valeur { get; set; }
        //[DataMember]
        //[Column(Name = "NATURECONTRAT")]
        //public string NatureContrat { get; set; }
        //[DataMember]
        //[Column(Name = "LIBELLENATURECONTRAT")]
        //public string LibelleNatureContrat { get; set; }
        //[DataMember]
        //[Column(Name = "PARTALBINGIA")]
        //public Single? PartAlbingia { get; set; }
        //[DataMember]
        //[Column(Name = "APERITEURCODE")]
        //public string AperiteurCode { get; set; }
        //[DataMember]
        //[Column(Name = "APERITEURNOM")]
        //public string AperiteurNom { get; set; }
        //[DataMember]
        //[Column(Name = "FRAISAPERITION")]
        //public Single? FraisAperition { get; set; }
        //[DataMember]
        //[Column(Name = "INTERCALAIREEXISTE")]
        //public string IntercalaireExiste { get; set; }
        //[DataMember]
        //[Column(Name = "COUVERTURE")]
        //public Single Couverture { get; set; }

        //[DataMember]
        //[Column(Name = "COURTIERAPPORTEUR")]
        //public int CourtierApporteur { get; set; }
        //[DataMember]
        //[Column(Name = "NOMCOURTIERAPPO")]
        //public string NomCourtierAppo { get; set; }
        //[DataMember]
        //[Column(Name = "VILLEAPPORTEUR")]
        //public string VilleCourtierApporteur { get; set; }
        //[DataMember]
        //[Column(Name = "COURTIERGESTIONNAIRE")]
        //public int CourtierGestionnaire { get; set; }
        //[DataMember]
        //[Column(Name = "NOMCOURTIERGEST")]
        //public string NomCourtierGest { get; set; }
        //[DataMember]
        //[Column(Name = "VILLEGESTIONNAIRE")]
        //public string VilleCourtierGestionnaire { get; set; }
        //[DataMember]
        //[Column(Name = "COURTIERPAYEUR")]
        //public int CourtierPayeur { get; set; }
        //[DataMember]
        //[Column(Name = "NOMCOURTIERPAYEUR")]
        //public string NomCourtierPayeur { get; set; }
        //[DataMember]
        //[Column(Name = "VILLEPAYEUR")]
        //public string VilleCourtierPayeur { get; set; }

        //[DataMember]
        //[Column(Name = "REFCOURTIER")]
        //public string RefCourtier { get; set; }
        //[DataMember]
        //[Column(Name = "NOMINTERLOCUTEUR")]
        //public string NomInterlocuteur { get; set; }
        //[DataMember]
        //[Column(Name = "CODEINTERLOCUTEUR")]
        //public int CodeInterlocuteur { get; set; }
        //[DataMember]
        //[Column(Name = "NOMPRENASSUR")]
        //public string NomPreneurAssurance { get; set; }
        //[DataMember]
        //[Column(Name = "CODEPRENASSUR")]
        //public int CodePreneurAssurance { get; set; }
        //[DataMember]
        //[Column(Name = "PRENEURESTASSURE")]
        //public string PreneurEstAssure { get; set; }
        //[DataMember]
        //[Column(Name = "DEPASSUR")]
        //public string DepAssure { get; set; }
        //[DataMember]
        //[Column(Name = "VILLEASSUR")]
        //public string VilleAssure { get; set; }
        //[DataMember]
        //[Column(Name = "CODEPOSTALASSUR")]
        //public string CodePostalAssure { get; set; }
        ////[DataMember]
        ////[Column(Name = "ETATENTETE")]
        ////public string EtatEntete { get; set; }
        //[DataMember]
        //[Column(Name = "LIBETAT")]
        //public string LibelleEtat { get; set; }
        //[DataMember]
        //[Column(Name = "SITUATION")]
        //public string Situation { get; set; }
        //[DataMember]
        //[Column(Name = "LIBSITUATION")]
        //public string LibelleSituation { get; set; }
        //[DataMember]
        //[Column(Name = "ADRESSECONTRAT")]
        //public int AdresseContrat { get; set; }

        //[DataMember]
        //[Column(Name = "TYPE")]
        //public string Type { get; set; }

        //[DataMember]
        //[Column(Name = "SOUMISCATNAT")]
        //public string SoumisCatNat { get; set; }
        ////[DataMember]
        ////[Column(Name = "MONTANTREF1")]
        ////public Double MontantRef1 { get; set; }
        ////[DataMember]
        ////[Column(Name = "MONTANTREF2")]
        ////public Double MontantRef2 { get; set; }
        //[DataMember]
        //[Column(Name = "NUMINTERNEAVENANT")]
        //public Int16 NumInterneAvenant { get; set; }
        //[DataMember]
        //[Column(Name = "NUMAVENANT")]
        //public Int16 NumAvenant { get; set; }
        //[Column(Name = "DATEEFFETAVNJOUR")]
        //public Int16 DateEffetAvenantJour { get; set; }
        //[Column(Name = "DATEEFFETAVNMOIS")]
        //public Int16 DateEffetAvenantMois { get; set; }
        //[Column(Name = "DATEEFFETAVNANNEE")]
        //public Int16 DateEffetAvenantAnnee { get; set; }
        //[Column(Name = "HEUREAVN")]
        //public Int32 HeureAvn { get; set; }
        //[DataMember]
        //[Column(Name = "MOTIFAVN")]
        //public string MotifAvenant { get; set; }
        //[DataMember]
        //[Column(Name = "LIBMOTIFAVN")]
        //public string LibMotifAvenant { get; set; }
        //[DataMember]
        //[Column(Name = "DESCRAVENANT")]
        //public string DescriptionAvenant { get; set; }

        //[DataMember]
        //[Column(Name = "PROCHECHJ")]
        //public Int16 ProchaineEchJour { get; set; }
        //[DataMember]
        //[Column(Name = "PROCHECHM")]
        //public Int16 ProchaineEchMois { get; set; }
        //[DataMember]
        //[Column(Name = "PROCHECHA")]
        //public Int16 ProchaineEchAnnee { get; set; }

        //[DataMember]
        //[Column(Name = "ANTECEDENT")]
        //public string Antecedent { get; set; }

        //[DataMember]
        //[Column(Name = "DESCRIPTION")]
        //public string Description { get; set; }
        //[DataMember]
        //public bool IsCheckedEcheance { get; set; }
        //[DataMember]
        //[Column(Name = "NBASSUADD")]
        //public int NbAssuresAdditionnels { get; set; }

        //#region Blocage termes

        //[DataMember]
        //[Column(Name = "DATEDEBDERNIEREPERIODEJOUR")]
        //public Int16 DateDebutDernierePeriodeJour { get; set; }
        //[DataMember]
        //[Column(Name = "DATEDEBDERNIEREPERIODEMOIS")]
        //public Int16 DateDebutDernierePeriodeMois { get; set; }
        //[DataMember]
        //[Column(Name = "DATEDEBDERNIEREPERIODEANNEE")]
        //public Int16 DateDebutDernierePeriodeAnnee { get; set; }
        //[DataMember]
        //[Column(Name = "DATEFINDERNIEREPERIODEJOUR")]
        //public Int16 DateFinDernierePeriodeJour { get; set; }
        //[DataMember]
        //[Column(Name = "DATEFINDERNIEREPERIODEMOIS")]
        //public Int16 DateFinDernierePeriodeMois { get; set; }
        //[DataMember]
        //[Column(Name = "DATEFINDERNIEREPERIODEANNEE")]
        //public Int16 DateFinDernierePeriodeAnnee { get; set; }
        //[DataMember]
        //[Column(Name = "ZONESTOP")]
        //public string ZoneStop { get; set; }


        //#endregion

        //[DataMember]
        //[Column(Name = "PARTAPERITEUR")]
        //public float PartAperiteur { get; set; }

        //[DataMember]
        //[Column(Name = "IDINTERLOCUTEURAPE")]
        //public int IdInterlocuteurAperiteur { get; set; }

        //[DataMember]
        //[Column(Name = "LIBINTERLOCUTEURAPE")]
        //public string NomInterlocuteurAperiteur { get; set; }

        //[DataMember]
        //[Column(Name = "REFERENCEAPERITEUR")]
        //public string ReferenceAperiteur { get; set; }

        //[DataMember]
        //[Column(Name = "FRAISACCAPERITEUR")]
        //public float FraisAccAperiteur { get; set; }

        //[DataMember]
        //[Column(Name = "COMMISSIONAPERITEUR")]
        //public float CommissionAperiteur { get; set; }

        //[DataMember]
        //[Column(Name = "TYPERETOUR")]
        //public string TypeRetour { get; set; }
        //[DataMember]
        //[Column(Name = "LIBRETOUR")]
        //public string LibRetour { get; set; }

        //[DataMember]
        //public RisqueDto Risque { get; set; }
        //[DataMember]
        //public List<RisqueDto> Risques { get; set; }

        //[DataMember]
        //public bool IsMonoRisque { get; set; }

        //[DataMember]
        //public DateTime? DateEffetAvenant { get; set; }
        //[DataMember]
        //public TimeSpan? HeureEffetAvenant { get; set; }

        //[DataMember]
        //[Column(Name = "PARTBENEF")]
        //public string PartBenefDB { get; set; }

        //[Column(Name = "NBOPPBENEF")]
        //public Int32 NbOppBenef { get; set; }

        //[DataMember]
        //public bool HasOppBenef { get; set; }

        //[DataMember]
        //[Column(Name = "REGULEID")]
        //public Int64 ReguleId { get; set; }

        //[DataMember]
        //[Column(Name = "DATERESILANNNEE")]
        //public Int32 DateResilAnnee { get; set; }
        //[DataMember]
        //[Column(Name = "DATERESILMOIS")]
        //public Int32 DateResilMois { get; set; }
        //[DataMember]
        //[Column(Name = "DATERESILJOUR")]
        //public Int32 DateResilJour { get; set; }
        //[DataMember]
        //[Column(Name = "DATERESILHEURE")]
        //public Int32 DateResilHeure { get; set; }
        //[DataMember]
        //public Int64 CountRsq { get; set; }
    }

}

