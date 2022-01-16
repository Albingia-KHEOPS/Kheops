using Albingia.Common;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle
{
    public class AnInformationsGeneralesPage : MetaModelsBase
    {
        public string CodeContrat { get; set; }
        public long VersionContrat { get; set; }
        public string Type { get; set; }
        public string txtParamRedirect { get; set; }
        [Display(Name = "Identification *")]
        public string Identification { get; set; }
        [Display(Name = "Cible")]
        public string Cible { get; set; }
        public string CibleLib { get; set; }
        public string connexite { get; set; }
        //ExistEcheance=true si un echeancier existe
        public bool ExistEcheancier { get; set; }
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

        [Display(Name = "Observations")]
        public string Observations { get; set; }

        [Display(Name = "Devise *")]
        public List<AlbSelectListItem> Devises { get; set; }
        public string Devise { get; set; }

        [Display(Name = "Périodicité *")]
        public List<AlbSelectListItem> Periodicites { get; set; }
        public string Periodicite { get; set; }

        [Display(Name = "STOP")]
        public string Stop { get; set; }
        public List<AlbSelectListItem> Stops { get; set; }

        [Display(Name = "Ech. principale")]
        public DateTime? EcheancePrincipale { get; set; }

        [Display(Name = "Préavis de résil.(mois)")]
        public int Preavis { get; set; }

        [Display(Name = "Effet des garanties")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? EffetGaranties { get; set; }

        [Display(Name = "Heure d'effet")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")]
        public TimeSpan? HeureEffet { get; set; }

        public bool EffetCheck { get; set; }

        [Display(Name = "Fin d'effet")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? FinEffet { get; set; }

        [Display(Name = "Heure fin d'effet")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")]
        public TimeSpan? HeureFinEffet { get; set; }

        [Display(Name = "Durée")]
        public bool DureeCheck { get; set; }

        [Display(Name = "Durée")]
        public int? Duree { get; set; }
        public List<AlbSelectListItem> Durees { get; set; }
        public string DureeString { get; set; }

        [Display(Name = "Date accord*")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? DateAccord { get; set; }

        [Display(Name = "Date statistique *")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? DateStatistique { get; set; }

        [Display(Name = "Indice référence")]
        public List<AlbSelectListItem> IndicesReference { get; set; }
        public string IndiceReference { get; set; }

        [Display(Name = "Valeur")]
        public string Valeur { get; set; }

        [Display(Name = "Nature contrat *")]
        public List<AlbSelectListItem> NaturesContrat { get; set; }
        public string NatureContrat { get; set; }

        [Display(Name = "Part Albingia (%)")]
        public string PartAlbingia { get; set; }

        [Display(Name = "Comm. Apé. (%)")]
        public string FraisApe { get; set; }

        [Display(Name = "Intercalaire courtier ?")]
        public bool Intercalaire { get; set; }

        [Display(Name = "Couverture (%)")]
        public string Couverture { get; set; }

        [Display(Name = "Gestionnaire *")]
        public string GestionnaireNom { get; set; }
        public string GestionnaireCode { get; set; }

        [Display(Name = "Souscripteur *")]
        public string SouscripteurNom { get; set; }

        public string SouscripteurCode { get; set; }

        public string AperiteurCode { get; set; }

        [Display(Name = "Apériteur")]
        public string AperiteurNom { get; set; }

        [Display(Name = "Antécédents")]
        public string Antecedent { get; set; }

        public string Description { get; set; }

        public List<AlbSelectListItem> Antecedents { get; set; }

        [Display(Name = "Regime de taxe ")]
        public List<AlbSelectListItem> RegimesTaxe { get; set; }
        public string RegimeTaxe { get; set; }
        [Display(Name = "CATNAT possible")]
        public bool SoumisCatNat { get; set; }
        public bool IsMonoRisque { get; set; }

        public DateTime? PeriodeDeb { get; set; }
        public DateTime? PeriodeFin { get; set; }
        [Display(Name = "Prochaine échéance")]
        public DateTime? ProchaineEcheance { get; set; }

        public float PartAperiteur { get; set; }
        public int IdInterlocuteurAperiteur { get; set; }
        public string NomInterlocuteurAperiteur { get; set; }
        public string ReferenceAperiteur { get; set; }
        public float FraisAccAperiteur { get; set; }
        public float CommissionAperiteur { get; set; }
        public string PartBenefDB { get; set; }
        public string PartBenef { get; set; }
        public bool OppBenef { get; set; }
        public bool LTA { get; set; }

        /// <summary>
        /// B3101
        /// Modification Date fin effet dans le modif hors avenant
        /// </summary>
        public bool CanModifEndEffectDate { get; set; }

        internal ContratDto LoadContratDto()
        {
            var contrat = new ContratDto();
            #region Base
            contrat.CodeContrat = this.CodeContrat;
            contrat.VersionContrat = this.VersionContrat;
            contrat.Type = this.Type;
            contrat.Descriptif = this.Identification;
            contrat.CodeMotsClef1 = this.MotClef1;
            contrat.CodeMotsClef2 = this.MotClef2;
            contrat.CodeMotsClef3 = this.MotClef3;
            contrat.Devise = this.Devise;
            contrat.PeriodiciteCode = this.Periodicite;
            contrat.ZoneStop = !string.IsNullOrEmpty(this.Stop) ? this.Stop : string.Empty;
            if (this.EcheancePrincipale.HasValue)
            {
                contrat.Mois = Int16.Parse(this.EcheancePrincipale.Value.Month.ToString());
                contrat.Jour = Int16.Parse(this.EcheancePrincipale.Value.Day.ToString());
            }
            if (this.EffetGaranties.HasValue)
            {
                contrat.DateEffetAnnee = Int16.Parse(this.EffetGaranties.Value.Year.ToString());
                contrat.DateEffetMois = Int16.Parse(this.EffetGaranties.Value.Month.ToString());
                contrat.DateEffetJour = Int16.Parse(this.EffetGaranties.Value.Day.ToString());
                if (this.HeureEffet.HasValue)
                {
                    contrat.DateEffetHeure = AlbConvert.ConvertTimeToIntMinute(this.HeureEffet).Value;
                    this.EffetGaranties = new DateTime(contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour, this.HeureEffet.Value.Hours, this.HeureEffet.Value.Minutes, 0);
                }
                else
                {
                    this.EffetGaranties = new DateTime(contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour, 0, 0, 0);
                }
            }
            if (this.FinEffet.HasValue && EffetCheck)
            {
                contrat.FinEffetAnnee = Int16.Parse(this.FinEffet.Value.Year.ToString());
                contrat.FinEffetMois = Int16.Parse(this.FinEffet.Value.Month.ToString());
                contrat.FinEffetJour = Int16.Parse(this.FinEffet.Value.Day.ToString());
                if (this.HeureFinEffet.HasValue)
                    contrat.FinEffetHeure = AlbConvert.ConvertTimeToIntMinute(this.HeureFinEffet).Value;
            }
            if (this.Duree.HasValue && DureeCheck)
            {
                contrat.DureeGarantie = this.Duree.Value;
                contrat.UniteDeTemps = this.DureeString;

                if (this.EffetGaranties.HasValue)
                {
                    var dateFin = AlbConvert.GetFinPeriode(this.EffetGaranties, this.Duree.HasValue ? this.Duree.Value : 0, this.DureeString);
                    contrat.FinEffetAnnee = Int16.Parse(dateFin.Value.Year.ToString());
                    contrat.FinEffetMois = Int16.Parse(dateFin.Value.Month.ToString());
                    contrat.FinEffetJour = Int16.Parse(dateFin.Value.Day.ToString());
                    contrat.FinEffetHeure = Int16.Parse(dateFin.Value.Hour.ToString() + dateFin.Value.Minute.ToString().PadLeft(2, '0'));
                }
            }
            if (contrat.PeriodiciteCode == "U" || contrat.PeriodiciteCode == "E")
            {
                contrat.DateFinDernierePeriodeAnnee = contrat.FinEffetAnnee;
                contrat.DateFinDernierePeriodeMois = contrat.FinEffetMois;
                contrat.DateFinDernierePeriodeJour = contrat.FinEffetJour;
            }
            if (this.DateAccord.HasValue)
            {
                contrat.DateAccordAnnee = Int16.Parse(this.DateAccord.Value.Year.ToString());
                contrat.DateAccordMois = Int16.Parse(this.DateAccord.Value.Month.ToString());
                contrat.DateAccordJour = Int16.Parse(this.DateAccord.Value.Day.ToString());
            }
            if (this.DateStatistique.HasValue) {
                contrat.DateStatistique = AlbConvert.ConvertDateToInt(this.DateStatistique).Value;
            }
            else if (this.DateAccord.HasValue) {
                contrat.DateStatistique = AlbConvert.ConvertDateToInt(this.DateAccord).Value;
            }
            contrat.NatureContrat = this.NatureContrat;
            contrat.PartAlbingia = float.TryParse(this.PartAlbingia, out float f) ? (float?)f : null;
            contrat.Couverture = float.TryParse(this.Couverture, out f) ? f : 0;
            contrat.SouscripteurCode = this.SouscripteurCode;
            contrat.GestionnaireCode = this.GestionnaireCode;
            contrat.CodeRegime = this.RegimeTaxe;
            contrat.Antecedent = this.Antecedent;
            contrat.Description = this.Description;
            #endregion
            #region OBSV
            //contrat.Observations = this.Observations;

            if (!string.IsNullOrEmpty(this.Observations))
            {
                contrat.Observations = this.Observations.Replace("\r\n", "<br>").Replace("\n", "<br>");
            }
            else
            {
                contrat.Observations = string.Empty;
            }

            #endregion
            #region YPRTENT
            contrat.Preavis = this.Preavis;
            contrat.IndiceReference = this.IndiceReference;
            if (!string.IsNullOrEmpty(this.Valeur))
                contrat.Valeur = float.Parse(this.Valeur.Replace(".", ","));
            else contrat.Valeur = 0;
            contrat.IntercalaireExiste = this.Intercalaire ? "O" : "N";
            contrat.SoumisCatNat = this.SoumisCatNat ? "O" : "N";

            if (this.ProchaineEcheance.HasValue)
            {
                contrat.ProchaineEchAnnee = Int16.Parse(this.ProchaineEcheance.Value.Year.ToString());
                contrat.ProchaineEchMois = Int16.Parse(this.ProchaineEcheance.Value.Month.ToString());
                contrat.ProchaineEchJour = Int16.Parse(this.ProchaineEcheance.Value.Day.ToString());
            }
            if (this.PeriodeDeb.HasValue)
            {
                contrat.DateDebutDernierePeriodeAnnee = Int16.Parse(this.PeriodeDeb.Value.Year.ToString());
                contrat.DateDebutDernierePeriodeMois = Int16.Parse(this.PeriodeDeb.Value.Month.ToString());
                contrat.DateDebutDernierePeriodeJour = Int16.Parse(this.PeriodeDeb.Value.Day.ToString());
            }
            if (this.PeriodeFin.HasValue)
            {
                contrat.DateFinDernierePeriodeAnnee = Int16.Parse(this.PeriodeFin.Value.Year.ToString());
                contrat.DateFinDernierePeriodeMois = Int16.Parse(this.PeriodeFin.Value.Month.ToString());
                contrat.DateFinDernierePeriodeJour = Int16.Parse(this.PeriodeFin.Value.Day.ToString());
            }
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
            #endregion
            #region KPENT
            if (this.DateStatistique.HasValue)
                contrat.DateStatistique = AlbConvert.ConvertDateToInt(this.DateStatistique).Value;
            else contrat.DateStatistique = 0;
            #endregion

            contrat.LTA = this.LTA ? "O" : "N";

            return contrat;
        }

    }
}