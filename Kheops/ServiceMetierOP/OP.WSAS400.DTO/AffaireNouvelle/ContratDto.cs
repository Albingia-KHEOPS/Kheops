using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Data.Linq.Mapping;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class ContratDto
    {
        [DataMember]
        [Column(Name = "DESCRIPTIF")]
        public string Descriptif { get; set; }
        [DataMember]
        [Column(Name = "CODECONTRAT")]
        public string CodeContrat { get; set; }
        [DataMember]
        [Column(Name = "VERSCONTRAT")]
        public Int64 VersionContrat { get; set; }
        [DataMember]
        [Column(Name = "LIBTYPECONTRAT")]
        public string LibTypeContrat { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETA")]
        public Int16 DateEffetAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETM")]
        public Int16 DateEffetMois { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETJ")]
        public Int16 DateEffetJour { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETJH")]
        public int DateEffetHeure { get; set; }
        [DataMember]
        [Column(Name = "DATEACCORDA")]
        public Int16 DateAccordAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEACCORDM")]
        public Int16 DateAccordMois { get; set; }
        [DataMember]
        [Column(Name = "DATEACCORDJ")]
        public Int16 DateAccordJour { get; set; }
        [DataMember]
        [Column(Name = "DATECRA")]
        public Int16 DateCreationAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATECRM")]
        public Int16 DateCreationMois { get; set; }
        [DataMember]
        [Column(Name = "DATECRJ")]
        public Int16 DateCreationJour { get; set; }
        [DataMember]
        [Column(Name = "CONTRATREMP")]
        public string ContratRemplace { get; set; }
        [DataMember]
        [Column(Name = "CODECONTRATREMP")]
        public string CodeContratRemplace { get; set; }
        [DataMember]
        [Column(Name = "VERSCONTRATREMP")]
        public Int64 ContratRemplaceAliment { get; set; }
        [DataMember]
        [Column(Name = "SOUSCODE")]
        public string SouscripteurCode { get; set; }
        [DataMember]
        [Column(Name = "SOUSNOM")]
        public string SouscripteurNom { get; set; }
        [DataMember]
        [Column(Name = "GESCODE")]
        public string GestionnaireCode { get; set; }
        [DataMember]
        [Column(Name = "GESNOM")]
        public string GestionnaireNom { get; set; }
        [DataMember]
        [Column(Name = "ETAT")]
        public string Etat { get; set; }
        [DataMember]
        [Column(Name = "OBSERVATIONS")]
        public string Observations { get; set; }
        [DataMember]
        [Column(Name = "NUMCHRONOOSBV")]
        public Int64 NumChronoOsbv { get; set; }
        [DataMember]
        [Column(Name = "CIBLE")]
        public string Cible { get; set; }
        [DataMember]
        [Column(Name = "CIBLELIB")]
        public string CibleLib { get; set; }
        [DataMember]
        [Column(Name = "TYPEPOLICE")]
        public string TypePolice { get; set; }
        [DataMember]
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }
        [DataMember]
        [Column(Name = "BRANCHELIB")]
        public string BrancheLib { get; set; }
        [DataMember]
        [Column(Name = "SOUSBRANCHE")]
        public string SousBranche { get; set; }
        [DataMember]
        [Column(Name = "CATEGORIE")]
        public string Categorie { get; set; }
        [DataMember]
        [Column(Name = "DEVISE")]
        public string Devise { get; set; }
        [DataMember]
        [Column(Name = "LIBDEVISE")]
        public string LibelleDevise { get; set; }
        [DataMember]
        [Column(Name = "PERIODICITECODE")]
        public string PeriodiciteCode { get; set; }
        [DataMember]
        [Column(Name = "PERIODICITENOM")]
        public string PeriodiciteNom { get; set; }
        [DataMember]
        [Column(Name = "ECHJOUR")]
        public Int16 Jour { get; set; }
        [DataMember]
        [Column(Name = "ECHMOIS")]
        public Int16 Mois { get; set; }
        [DataMember]
        [Column(Name = "CODEMOTSCLEF1")]
        public string CodeMotsClef1 { get; set; }
        [DataMember]
        [Column(Name = "CODEMOTSCLEF2")]
        public string CodeMotsClef2 { get; set; }
        [DataMember]
        [Column(Name = "CODEMOTSCLEF3")]
        public string CodeMotsClef3 { get; set; }
        [DataMember]
        [Column(Name = "PREAVIS")]
        public int Preavis { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETJOUR")]
        public Int16 FinEffetJour { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETMOIS")]
        public Int16 FinEffetMois { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETANNEE")]
        public Int16 FinEffetAnnee { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETHEURE")]
        public int FinEffetHeure { get; set; }
        [DataMember]
        [Column(Name = "DUREEGARANTIE")]
        public int DureeGarantie { get; set; }
        [DataMember]
        [Column(Name = "UNITETEMPS")]
        public string UniteDeTemps { get; set; }
        [DataMember]
        [Column(Name = "DATESTATISTIQUE")]
        public int DateStatistique { get; set; }
        [DataMember]
        [Column(Name = "INDICEREFERENCE")]
        public string IndiceReference { get; set; }
        [DataMember]
        [Column(Name = "LIBINDICE")]
        public string LibelleIndicReference { get; set; }
        [DataMember]
        [Column(Name = "VALEUR")]
        public Single Valeur { get; set; }
        [DataMember]
        [Column(Name = "NATURECONTRAT")]
        public string NatureContrat { get; set; }
        [DataMember]
        [Column(Name = "LIBELLENATURECONTRAT")]
        public string LibelleNatureContrat { get; set; }
        [DataMember]
        [Column(Name = "PARTALBINGIA")]
        public Single? PartAlbingia { get; set; }
        [DataMember]
        [Column(Name = "APERITEURCODE")]
        public string AperiteurCode { get; set; }
        [DataMember]
        [Column(Name = "APERITEURNOM")]
        public string AperiteurNom { get; set; }
        [DataMember]
        [Column(Name = "FRAISAPERITION")]
        public Single? FraisAperition { get; set; }
        [DataMember]
        [Column(Name = "INTERCALAIREEXISTE")]
        public string IntercalaireExiste { get; set; }
        [DataMember]
        [Column(Name = "COUVERTURE")]
        public Single Couverture { get; set; }

        [DataMember]
        [Column(Name = "COURTIERAPPORTEUR")]
        public int CourtierApporteur { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERAPPO")]
        public string NomCourtierAppo { get; set; }
        [DataMember]
        [Column(Name = "VILLEAPPORTEUR")]
        public string VilleCourtierApporteur { get; set; }
        [DataMember]
        [Column(Name = "COURTIERGESTIONNAIRE")]
        public int CourtierGestionnaire { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERGEST")]
        public string NomCourtierGest { get; set; }
        [DataMember]
        [Column(Name = "VILLEGESTIONNAIRE")]
        public string VilleCourtierGestionnaire { get; set; }
        [DataMember]
        [Column(Name = "COURTIERPAYEUR")]
        public int CourtierPayeur { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERPAYEUR")]
        public string NomCourtierPayeur { get; set; }
        [DataMember]
        [Column(Name = "VILLEPAYEUR")]
        public string VilleCourtierPayeur { get; set; }

        [DataMember]
        [Column(Name = "REFCOURTIER")]
        public string RefCourtier { get; set; }
        [DataMember]
        [Column(Name = "NOMINTERLOCUTEUR")]
        public string NomInterlocuteur { get; set; }
        [DataMember]
        [Column(Name = "CODEINTERLOCUTEUR")]
        public int CodeInterlocuteur { get; set; }
        [DataMember]
        [Column(Name = "NOMPRENASSUR")]
        public string NomPreneurAssurance { get; set; }
        [DataMember]
        [Column(Name = "CODEPRENASSUR")]
        public int CodePreneurAssurance { get; set; }
        [DataMember]
        [Column(Name = "PRENEURESTASSURE")]
        public string PreneurEstAssure { get; set; }
        [DataMember]
        [Column(Name = "DEPASSUR")]
        public string DepAssure { get; set; }
        [DataMember]
        [Column(Name = "VILLEASSUR")]
        public string VilleAssure { get; set; }
        [DataMember]
        [Column(Name = "CODEPOSTALASSUR")]
        public string CodePostalAssure { get; set; }
        [DataMember]
        [Column(Name = "LIBETAT")]
        public string LibelleEtat { get; set; }
        [DataMember]
        [Column(Name = "SITUATION")]
        public string Situation { get; set; }
        [DataMember]
        [Column(Name = "LIBSITUATION")]
        public string LibelleSituation { get; set; }
        [DataMember]
        [Column(Name = "ADRESSECONTRAT")]
        public int AdresseContrat { get; set; }
        [DataMember]
        [Column(Name = "CODEENCAISSEMENT")]
        public string CodeEncaissement { get; set; }
        [DataMember]
        [Column(Name = "LIBENCAISSEMENT")]
        public string LibelleEncaissement { get; set; }
        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }

        [DataMember]
        [Column(Name = "CODEREGIME")]
        public string CodeRegime { get; set; }
        [DataMember]
        [Column(Name = "LIBREGIME")]
        public string LibelleRegime { get; set; }
        [DataMember]
        [Column(Name = "SOUMISCATNAT")]
        public string SoumisCatNat { get; set; }
        [DataMember]
        [Column(Name = "NUMINTERNEAVENANT")]
        public Int16 NumInterneAvenant { get; set; }
        [DataMember]
        [Column(Name = "NUMAVENANT")]
        public Int16 NumAvenant { get; set; }
        [Column(Name = "DATEEFFETAVNJOUR")]
        public Int16 DateEffetAvenantJour { get; set; }
        [Column(Name = "DATEEFFETAVNMOIS")]
        public Int16 DateEffetAvenantMois { get; set; }
        [Column(Name = "DATEEFFETAVNANNEE")]
        public Int16 DateEffetAvenantAnnee { get; set; }
        [Column(Name = "HEUREAVN")]
        public Int32 HeureAvn { get; set; }
        [DataMember]
        [Column(Name = "MOTIFAVN")]
        public string MotifAvenant { get; set; }
        [DataMember]
        [Column(Name = "LIBMOTIFAVN")]
        public string LibMotifAvenant { get; set; }
        [DataMember]
        [Column(Name = "DESCRAVENANT")]
        public string DescriptionAvenant { get; set; }

        [DataMember]
        [Column(Name = "PROCHECHJ")]
        public Int16 ProchaineEchJour { get; set; }
        [DataMember]
        [Column(Name = "PROCHECHM")]
        public Int16 ProchaineEchMois { get; set; }
        [DataMember]
        [Column(Name = "PROCHECHA")]
        public Int16 ProchaineEchAnnee { get; set; }

        [DataMember]
        [Column(Name = "ANTECEDENT")]
        public string Antecedent { get; set; }

        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
         [DataMember]
        public bool IsCheckedEcheance { get; set; }

        [DataMember]
        [Column(Name = "NBASSUADD")]
        public int NbAssuresAdditionnels { get; set; }

        #region Blocage termes

        [DataMember]
        [Column(Name = "DATEDEBDERNIEREPERIODEJOUR")]
        public Int16 DateDebutDernierePeriodeJour { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBDERNIEREPERIODEMOIS")]
        public Int16 DateDebutDernierePeriodeMois { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBDERNIEREPERIODEANNEE")]
        public Int16 DateDebutDernierePeriodeAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEFINDERNIEREPERIODEJOUR")]
        public Int16 DateFinDernierePeriodeJour { get; set; }
        [DataMember]
        [Column(Name = "DATEFINDERNIEREPERIODEMOIS")]
        public Int16 DateFinDernierePeriodeMois { get; set; }
        [DataMember]
        [Column(Name = "DATEFINDERNIEREPERIODEANNEE")]
        public Int16 DateFinDernierePeriodeAnnee { get; set; }
        [DataMember]
        [Column(Name = "ZONESTOP")]
        public string ZoneStop { get; set; }


        #endregion

        [DataMember]
        [Column(Name = "PARTAPERITEUR")]
        public float PartAperiteur { get; set; }

        [DataMember]
        [Column(Name = "IDINTERLOCUTEURAPE")]
        public int IdInterlocuteurAperiteur { get; set; }

        [DataMember]
        [Column(Name = "LIBINTERLOCUTEURAPE")]
        public string NomInterlocuteurAperiteur { get; set; }

        [DataMember]
        [Column(Name = "REFERENCEAPERITEUR")]
        public string ReferenceAperiteur { get; set; }

        [DataMember]
        [Column(Name = "FRAISACCAPERITEUR")]
        public float FraisAccAperiteur { get; set; }

        [DataMember]
        [Column(Name = "COMMISSIONAPERITEUR")]
        public float CommissionAperiteur { get; set; }

        [DataMember]
        [Column(Name = "TYPERETOUR")]
        public string TypeRetour { get; set; }
        [DataMember]
        [Column(Name = "LIBRETOUR")]
        public string LibRetour { get; set; }

        [DataMember]
        public RisqueDto Risque { get; set; }
        [DataMember]
        public List<RisqueDto> Risques { get; set; }

        [DataMember]
        public bool IsMonoRisque { get; set; }

        [DataMember]
        public DateTime? DateEffetAvenant { get; set; }
        [DataMember]
        public TimeSpan? HeureEffetAvenant { get; set; }

        [DataMember]
        [Column(Name = "PARTBENEF")]
        public string PartBenefDB { get; set; }

        [Column(Name="NBOPPBENEF")]
        public Int32 NbOppBenef { get; set; }

        [DataMember]
        public bool HasOppBenef { get; set; }

        [DataMember]
        [Column(Name="REGULEID")]
        public Int64 ReguleId { get; set; }

        [DataMember]
        [Column(Name = "DATERESILANNNEE")]
        public Int32 DateResilAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATERESILMOIS")]
        public Int32 DateResilMois { get; set; }
        [DataMember]
        [Column(Name = "DATERESILJOUR")]
        public Int32 DateResilJour { get; set; }
        [DataMember]
        [Column(Name = "DATERESILHEURE")]
        public Int32 DateResilHeure { get; set; }

        [DataMember]
        public Int64 CountRsq { get; set; }

        [DataMember]
        public bool IsTemporaire { get; set; }

        [DataMember]
        [Column(Name ="LTA")]
        public string LTA { get; set; }

        /// <summary>
        /// Delegation
        /// </summary>
        [DataMember]
        public string Delegation { get; set; }
        /// <summary>
        /// Inspecteur
        /// </summary>
        [DataMember]
        public string Inspecteur { get; set; }

        [Column(Name = "CODEACTION")]
        [DataMember]
        public string TypeTraitement { get; set; }
    }
}
