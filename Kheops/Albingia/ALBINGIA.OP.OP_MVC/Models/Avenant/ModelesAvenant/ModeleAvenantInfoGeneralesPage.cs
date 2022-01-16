using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Avenant.ModelesAvenant
{
    public class ModeleAvenantInfoGeneralesPage : MetaModelsBase
    {
        #region Informations Cachées

        public string CodeContrat { get; set; }
        public long VersionContrat { get; set; }
        public string Type { get; set; }
        public bool ExistEcheancier { get; set; }
        public string txtParamRedirect { get; set; }
        public DateTime? DateStatistique { get; set; }
        public string PeriodiciteHisto { get; set; }
        public DateTime? ProchEchHisto { get; set; }
        public DateTime? DebPeriodHisto { get; set; }
        public string EtatHisto { get; set; }
        public string SituationHisto { get; set; }

        #endregion
        #region 1er Bloc

        [Display(Name = "N° avenant*")]
        public string NumAvenant { get; set; }
        [Display(Name = "N° interne")]
        public string NumInterne { get; set; }
        [Display(Name = "Début d'effet avt")]
        public DateTime? DateDebEffetAvt { get; set; }
        public TimeSpan? HeureDebEffetAvt { get; set; }
        public DateTime? DateResil { get; set; }
        [Display(Name = "Motif*")]
        public string Motif { get; set; }
        public string MotifLib { get; set; }
        [Display(Name = "Réf. de gestion")]
        public string Description { get; set; }
        public List<AlbSelectListItem> Motifs { get; set; }

        #endregion
        #region 2e Bloc

        [Display(Name = "Identification*")]
        public string Identification { get; set; }
        [Display(Name = "Cible*")]
        public string CibleCode { get; set; }
        public string CibleLib { get; set; }
        [Display(Name = "Mots-Clés")]
        public string MotClef1 { get; set; }
        public List<AlbSelectListItem> MotsClefs1 { get; set; }
        public string MotClef2 { get; set; }
        public List<AlbSelectListItem> MotsClefs2 { get; set; }
        public string MotClef3 { get; set; }
        public List<AlbSelectListItem> MotsClefs3 { get; set; }
        [Display(Name = "Observations")]
        public string Observations { get; set; }
        [Display(Name = "Devise")]
        public string DeviseCode { get; set; }
        public string DeviseLib { get; set; }
        [Display(Name = "Régime de taxe")]
        public string RegimeTaxe { get; set; }
        public List<AlbSelectListItem> RegimesTaxe { get; set; }
        [Display(Name = "CATNAT possible")]
        public bool SoumisCATNAT { get; set; }
        [Display(Name = "STOP")]
        public string Stop { get; set; }
        public List<AlbSelectListItem> Stops { get; set; }
        [Display(Name = "Antécédents")]
        public string Antecedent { get; set; }
        public List<AlbSelectListItem> Antecedents { get; set; }
        public string ObservationAntecedents { get; set; }

        #endregion
        #region 3e Bloc

        [Display(Name = "Périodicité*")]
        public string Periodicite { get; set; }
        public List<AlbSelectListItem> Periodicites { get; set; }
        [Display(Name = "Ech. principale")]
        public DateTime? EcheancePrincipale { get; set; }
        [Display(Name = "Prochaine échéance")]
        public DateTime? ProchaineEcheance { get; set; }
        public DateTime? PeriodeDeb { get; set; }
        public DateTime? PeriodeFin { get; set; }
        [Display(Name = "Début d'effet*")]
        public DateTime? DateDebEffet { get; set; }
        public TimeSpan? HeureDebEffet { get; set; }
        public bool EffetCheck { get; set; }
        [Display(Name = "Fin effet")]
        public DateTime? DateFinEffet { get; set; }
        public TimeSpan? HeureFinEffet { get; set; }
        public bool DureeCheck { get; set; }
        [Display(Name = "Durée")]
        public int? Duree { get; set; }
        public string DureeString { get; set; }
        public List<AlbSelectListItem> Durees { get; set; }
        [Display(Name = "Date d'accord*")]
        public DateTime? DateAccord { get; set; }
        [Display(Name = "Préavis de résil. (mois)")]
        public int Preavis { get; set; }
        public string PartBenef { get; set; }
        public bool OppBenef { get; set; }

        #endregion
        #region 4e Bloc

        [Display(Name = "Indice référence")]
        public string IndiceReference { get; set; }
        public List<AlbSelectListItem> IndicesReference { get; set; }
        [Display(Name = "Valeur")]
        public string Valeur { get; set; }
        public string AperiteurCode { get; set; }
        [Display(Name = "Apériteur")]
        public string AperiteurNom { get; set; }
        [Display(Name = "Nature contrat")]
        public string NatureContrat { get; set; }
        public List<AlbSelectListItem> NaturesContrat { get; set; }
        [Display(Name = "Part Albingia (%)")]
        public string PartAlbingia { get; set; }
        [Display(Name = "Frais apériteur (%)")]
        public string FraisApe { get; set; }
        [Display(Name = "Couverture (%)")]
        public string Couverture { get; set; }
        [Display(Name = "Intercalaire courtier ?")]
        public bool Intercalaire { get; set; }
        public string SouscripteurCode { get; set; }
        [Display(Name = "Souscripteur*")]
        public string SouscripteurNom { get; set; }
        public string GestionnaireCode { get; set; }
        [Display(Name = "Gestionnaire*")]
        public string GestionnaireNom { get; set; }

        #endregion

        public float PartAperiteur { get; set; }
        public int IdInterlocuteurAperiteur { get; set; }
        public string NomInterlocuteurAperiteur { get; set; }
        public string ReferenceAperiteur { get; set; }
        public float FraisAccAperiteur { get; set; }
        public float CommissionAperiteur { get; set; }
        public string PartBenefDB { get; set; }
        public bool LTA { get; set; }

        /// <summary>
        /// B3101
        /// Modification Date fin effet dans le modif hors avenant
        /// </summary>
        public bool CanModifEndEffectDate { get; set; }

        public ContratDto LoadAvenantDto(Int16 numAvenant)
        {
            ContratDto contrat = new ContratDto();

            contrat.CodeContrat = this.CodeContrat;
            contrat.VersionContrat = this.VersionContrat;
            contrat.Type = this.Type;
            contrat.NumAvenant = numAvenant;
            contrat.DateEffetAvenant = this.DateDebEffetAvt;
            contrat.HeureEffetAvenant = this.HeureDebEffetAvt;
            contrat.ZoneStop = !string.IsNullOrEmpty(this.Stop) ? this.Stop : string.Empty;
            contrat.DescriptionAvenant = !string.IsNullOrEmpty(this.Description) ? this.Description.Trim() : string.Empty;
            contrat.Descriptif = !string.IsNullOrEmpty(this.Identification) ? this.Identification.Trim() : string.Empty;
            contrat.CodeMotsClef1 = !string.IsNullOrEmpty(this.MotClef1) ? this.MotClef1 : string.Empty;
            contrat.CodeMotsClef2 = !string.IsNullOrEmpty(this.MotClef2) ? this.MotClef2 : string.Empty;
            contrat.CodeMotsClef3 = !string.IsNullOrEmpty(this.MotClef3) ? this.MotClef3 : string.Empty;
            contrat.Observations = !string.IsNullOrEmpty(this.Observations) ? this.Observations.Trim() : string.Empty;
            contrat.Devise = !string.IsNullOrEmpty(this.DeviseCode) ? this.DeviseCode : string.Empty;
            contrat.CodeRegime = !string.IsNullOrEmpty(this.RegimeTaxe) ? this.RegimeTaxe : string.Empty;
            contrat.SoumisCatNat = this.SoumisCATNAT ? "O" : "N";
            contrat.Antecedent = !string.IsNullOrEmpty(this.Antecedent) ? this.Antecedent : string.Empty;
            contrat.Description = !string.IsNullOrEmpty(this.ObservationAntecedents) ? HttpUtility.UrlDecode(this.ObservationAntecedents.Trim()) : string.Empty;
            contrat.PeriodiciteCode = !string.IsNullOrEmpty(this.Periodicite) ? this.Periodicite : string.Empty;
            if (this.EcheancePrincipale.HasValue)
            {
                contrat.Jour = Convert.ToInt16(this.EcheancePrincipale.Value.Day);
                contrat.Mois = Convert.ToInt16(this.EcheancePrincipale.Value.Month);
            }
            if (this.ProchaineEcheance.HasValue)
            {
                contrat.ProchaineEchJour = Convert.ToInt16(this.ProchaineEcheance.Value.Day);
                contrat.ProchaineEchMois = Convert.ToInt16(this.ProchaineEcheance.Value.Month);
                contrat.ProchaineEchAnnee = Convert.ToInt16(this.ProchaineEcheance.Value.Year);
            }
            if (this.DateDebEffet.HasValue)
            {
                contrat.DateEffetJour = Convert.ToInt16(this.DateDebEffet.Value.Day);
                contrat.DateEffetMois = Convert.ToInt16(this.DateDebEffet.Value.Month);
                contrat.DateEffetAnnee = Convert.ToInt16(this.DateDebEffet.Value.Year);
                if (this.HeureDebEffet.HasValue)
                {
                    contrat.DateEffetHeure = AlbConvert.ConvertTimeToIntMinute(this.HeureDebEffet.Value).Value;
                    this.DateDebEffet = new DateTime(contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour, this.HeureDebEffet.Value.Hours, this.HeureDebEffet.Value.Minutes, 0);
                }
                else
                {
                    this.DateDebEffet = new DateTime(contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour, 0, 0, 0);
                }
            }
            if (this.DateFinEffet.HasValue && EffetCheck)
            {
                contrat.FinEffetJour = Convert.ToInt16(this.DateFinEffet.Value.Day);
                contrat.FinEffetMois = Convert.ToInt16(this.DateFinEffet.Value.Month);
                contrat.FinEffetAnnee = Convert.ToInt16(this.DateFinEffet.Value.Year);
            }
            if (this.HeureFinEffet.HasValue)
                contrat.FinEffetHeure = AlbConvert.ConvertTimeToIntMinute(this.HeureFinEffet.Value).Value;
            contrat.DureeGarantie = this.Duree.HasValue ? this.Duree.Value : 0;
            contrat.UniteDeTemps = !string.IsNullOrEmpty(this.DureeString) ? this.DureeString : string.Empty;

            if (contrat.DureeGarantie > 0 && DureeCheck)
            {
                var dateFin = AlbConvert.GetFinPeriode(this.DateDebEffet, this.Duree.HasValue ? this.Duree.Value : 0, this.DureeString);
                contrat.FinEffetAnnee = Int16.Parse(dateFin.Value.Year.ToString());
                contrat.FinEffetMois = Int16.Parse(dateFin.Value.Month.ToString());
                contrat.FinEffetJour = Int16.Parse(dateFin.Value.Day.ToString());
                contrat.FinEffetHeure = Int16.Parse(dateFin.Value.Hour.ToString() + dateFin.Value.Minute.ToString().PadLeft(2, '0'));
            }

            if (contrat.PeriodiciteCode == "U" || contrat.PeriodiciteCode == "E" || contrat.PeriodiciteCode == "R")
            {
                contrat.DateFinDernierePeriodeAnnee = contrat.FinEffetAnnee;
                contrat.DateFinDernierePeriodeMois = contrat.FinEffetMois;
                contrat.DateFinDernierePeriodeJour = contrat.FinEffetJour;
            }

            if (this.DateAccord.HasValue)
            {
                contrat.DateAccordJour = Convert.ToInt16(this.DateAccord.Value.Day);
                contrat.DateAccordMois = Convert.ToInt16(this.DateAccord.Value.Month);
                contrat.DateAccordAnnee = Convert.ToInt16(this.DateAccord.Value.Year);
            }
            contrat.Preavis = this.Preavis;
            contrat.IndiceReference = !string.IsNullOrEmpty(this.IndiceReference) ? this.IndiceReference : string.Empty;
            contrat.Valeur = !string.IsNullOrEmpty(this.Valeur) ? float.Parse(this.Valeur) : 0;
            contrat.AperiteurCode = !string.IsNullOrEmpty(this.AperiteurCode) ? this.AperiteurCode : string.Empty;
            contrat.NatureContrat = !string.IsNullOrEmpty(this.NatureContrat) ? this.NatureContrat : string.Empty;
            contrat.PartAlbingia = !string.IsNullOrEmpty(this.PartAlbingia) ? float.Parse(this.PartAlbingia) : 0;
            contrat.FraisAperition = !string.IsNullOrEmpty(this.FraisApe) ? float.Parse(this.FraisApe) : 0;
            contrat.Couverture = !string.IsNullOrEmpty(this.Couverture) ? float.Parse(this.Couverture) : 0;
            contrat.IntercalaireExiste = this.Intercalaire ? "O" : "N";
            contrat.SouscripteurCode = !string.IsNullOrEmpty(this.SouscripteurCode) ? this.SouscripteurCode : string.Empty;
            contrat.GestionnaireCode = !string.IsNullOrEmpty(this.GestionnaireCode) ? this.GestionnaireCode : string.Empty;
            if (this.PeriodeDeb.HasValue)
            {
                contrat.DateDebutDernierePeriodeJour = Convert.ToInt16(this.PeriodeDeb.Value.Day);
                contrat.DateDebutDernierePeriodeMois = Convert.ToInt16(this.PeriodeDeb.Value.Month);
                contrat.DateDebutDernierePeriodeAnnee = Convert.ToInt16(this.PeriodeDeb.Value.Year);
            }
            if (this.PeriodeFin.HasValue)
            {
                contrat.DateFinDernierePeriodeJour = Convert.ToInt16(this.PeriodeFin.Value.Day);
                contrat.DateFinDernierePeriodeMois = Convert.ToInt16(this.PeriodeFin.Value.Month);
                contrat.DateFinDernierePeriodeAnnee = Convert.ToInt16(this.PeriodeFin.Value.Year);
            }
            contrat.DateStatistique = this.DateStatistique.HasValue ? AlbConvert.ConvertDateToInt(this.DateStatistique).Value : 0;

            #region YPOCOAS
            if (!string.IsNullOrEmpty(this.FraisApe))
                contrat.FraisAperition = float.Parse(this.FraisApe);
            contrat.AperiteurCode = this.AperiteurCode;
            contrat.PartAperiteur = this.PartAperiteur;
            contrat.IdInterlocuteurAperiteur = this.IdInterlocuteurAperiteur;
            contrat.NomInterlocuteurAperiteur = this.NomInterlocuteurAperiteur;
            contrat.ReferenceAperiteur = this.ReferenceAperiteur;
            contrat.FraisAccAperiteur = this.FraisAccAperiteur;
            contrat.CommissionAperiteur = this.CommissionAperiteur;

            #endregion

            contrat.LTA = this.LTA ? "O" : "N";
            return contrat;
        }
    }
}