using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesQuittances;
using ALBINGIA.OP.OP_MVC.Models.Regularisation;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Cotisations;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleQuittancePage : MetaModelsBase, IRegulModel
    {
        public string PeriodiciteContrat { get; set; }
        [Display(Name = "Périodicité")]
        public String Periodicite { get; set; }
        [Display(Name = "Opération")]
        public String Operation { get; set; }
        [Display(Name = "Indice")]
        public String Indice { get; set; }
        [Display(Name = "Période du")]
        public String PeriodeDebut { get; set; }
        [Display(Name = "au")]
        public String PeriodeFin { get; set; }
        //ExistEcheance=true si un echeancier existe
        public bool ExistEcheancier { get; set; }
        public string TypeCalcul { get; set; }
        public bool IsEcheanceNonTraite { get; set; }

        public FormulesInformation FormulesInfo { get; set; }
        public ListFormules Formules { get; set; }
        public int EffetAnnee { get; set; }

        [Display(Name = "Commentaires")]
        public string CommentForce { get; set; }
        public bool MontantForce { get; set; }

        public string ModeAffichage { get; set; }
        public bool ModeCalculForce { get; set; }//Mode d'affichage si un calcul forcé a été effectué
        /// <summary>
        /// Affiche la case à cocher "A échéance"
        /// </summary>
        public bool DisplayEch { get; set; }
        public string Etat { get; set; }

        /// <summary>
        /// Affiche la case à cocher "A émettre"
        /// </summary>
        public bool DisplayEmission { get; set; }
        public bool AEmission { get; set; }
        public string NumQuittance { get; set; }
        public string TypeAvt { get; set; }
        public string ModAvt { get; set; }
        public string ReguleId { get; set; }

        public bool ModifPeriode { get; set; }
        public bool IsModifDateFin { get; set; }
        public DateTime? DateDebEffetContrat { get; set; }
        public DateTime? DateFinEffetContrat { get; set; }
        public bool ShowVisuQuittance  { get; set; }
        public VisualisationQuittances ModeleVisualisationQuittances { get; set; }

        public RegularisationContext Context { get; set; }

        public IdContratDto IdContrat
        {
            get { return new IdContratDto { CodeOffre = CodePolicePage, Version = Int32.Parse(VersionPolicePage), Type = TypePolicePage }; }
        }

        public long RgId
        {
            get {
                Int64.TryParse(ReguleId, out var id);
                return id;
            }
        }

        public long LotId
        {
            get {
                Int64.TryParse(InformationGeneraleTransverse.GetAddParamValue(AddParamValue, AlbParameterName.LOTID), out var id);
                return id;
            }
        }

        public void LoadQuittances(List<QuittanceDto> quittancesDto)
        {
            FormulesInfo = new FormulesInformation();
            if (quittancesDto.Count > 0)
            {
                QuittanceDto quittanceDto0 = quittancesDto[0];
                Periodicite = string.IsNullOrEmpty(quittanceDto0.LibellePeriodicite) ? quittanceDto0.CodePeriodicite : quittanceDto0.CodePeriodicite + "-" + quittanceDto0.LibellePeriodicite;
                Operation = string.IsNullOrEmpty(quittanceDto0.LibelleOperation) ? quittanceDto0.CodeOperation.ToString() : quittanceDto0.CodeOperation + "-" + quittanceDto0.LibelleOperation;
                Indice = quittanceDto0.indice.ToString();
                if (quittanceDto0.DebutPeriodeAnnee != 0 && quittanceDto0.DebutPeriodeMois != 0 && quittanceDto0.DebutPeriodeJour != 0) {
                    PeriodeDebut = new DateTime(quittanceDto0.DebutPeriodeAnnee, quittanceDto0.DebutPeriodeMois, quittanceDto0.DebutPeriodeJour).ToShortDateString();
                }
                if (quittanceDto0.FinPeriodeAnnee != 0 && quittanceDto0.FinPeriodeMois != 0 && quittanceDto0.FinPeriodeJour != 0) {
                    PeriodeFin = new DateTime(quittanceDto0.FinPeriodeAnnee, quittanceDto0.FinPeriodeMois, quittanceDto0.FinPeriodeJour).ToShortDateString();
                }
            }

            FormulesInfo.Commission = LoadQuittanceCommission(quittancesDto);
            FormulesInfo.Totaux = LoadQuittanceTotaux(quittancesDto);
            Formules = new ListFormules
            {
                Formules = LoadQuittanceFormules(quittancesDto)
            };
        }

        public static Commission LoadQuittanceCommission(List<QuittanceDto> quittancesDto)
        {
            var commission = new Commission();
            if (quittancesDto.Count > 0)
            {
                QuittanceDto quittanceDto0 = quittancesDto[0];
                commission.MontantHRCatNatRetenu = Convert.ToDecimal(quittanceDto0.MontantCommissHRCatNatRetenu);
                commission.MontantCatNatRetenu = Convert.ToDecimal(quittanceDto0.MontantCommissRetenu);
                commission.MontantTotalRetenu = Convert.ToDecimal(quittanceDto0.TotalCommissRetenu);
                commission.TauxCatNatRetenu = quittanceDto0.TauxCatNatRetenu.ToString();
                commission.TauxHRCatNatRetenu = quittanceDto0.TauxHRCatNatRetenu.ToString();
                commission.TauxStd = quittanceDto0.TauxStd;
                commission.TauxStdCatNat = quittanceDto0.TauxStdCatNat;
                commission.Periodicite = quittanceDto0.Periodicite;
                if (quittanceDto0.ProchEchAn > 0) {
                    commission.ProchaineEcheance = new DateTime(quittanceDto0.ProchEchAn, quittanceDto0.ProchEchMois, quittanceDto0.ProchEchJour);
                }
                commission.MontantHTHRCN = quittanceDto0.MntHTHC;
                commission.MontantCN = quittanceDto0.MntC;
            }
            return commission;
        }
        public static Totaux LoadQuittanceTotaux(List<QuittanceDto> quittancesDto)
        {
            var totaux = new Totaux();
            if (quittancesDto.Count > 0)
            {
                QuittanceDto quittanceDto0 = quittancesDto[0];
                totaux.TotalHorsCatNatHT = Convert.ToDecimal(quittanceDto0.TotalHorsCatNatHT);
                totaux.TotalHorsCatNatTaxe = Convert.ToDecimal(quittanceDto0.TotalHorsCatNatTaxe);
                totaux.TotalHorsCatNatTTC = Convert.ToDecimal(quittanceDto0.TotalHorsCatNatTTC);

                totaux.CatNatHT = Convert.ToDecimal(quittanceDto0.CatNatHT);
                totaux.CatNatTaxe = Convert.ToDecimal(quittanceDto0.CatNatTaxe);
                totaux.CatNatTTC = Convert.ToDecimal(quittanceDto0.CatNatTTC);

                totaux.GareatHT = quittanceDto0.GareatHT;
                totaux.GareatTaxe = quittanceDto0.GareatTaxe;
                totaux.GareatTTC = quittanceDto0.GareatTTC;

                totaux.TotalHorsFraisHT = Convert.ToDecimal(quittanceDto0.TotalHorsFraisHT);
                totaux.TotalHorsFraisTaxe = Convert.ToDecimal(quittanceDto0.TotalHorsFraisTaxe);
                totaux.TotalHorsFraisTTC = Convert.ToDecimal(quittanceDto0.TotalHorsFraisTTC);

                totaux.FraisHT = Convert.ToDecimal(quittanceDto0.FraisHT);
                totaux.FraisTaxe = Convert.ToDecimal(quittanceDto0.FraisTaxe);
                totaux.FraisTTC =Convert.ToDecimal(quittanceDto0.FraisTTC);

                totaux.FGATaxe = Convert.ToDecimal(quittanceDto0.FGATaxe);
                totaux.FGATTC = Convert.ToDecimal(quittanceDto0.FGATTC);

                totaux.TotalHT = Convert.ToDecimal(quittanceDto0.MontantTotalHT);
                totaux.TotalTaxe = quittanceDto0.MontantTotalTaxe.ToString();
                totaux.TotalTTC = quittanceDto0.MontantTotalTTC.ToString(); 

            }
            return totaux;
        }
        public List<Formule> LoadQuittanceFormules(List<QuittanceDto> quittancesDto)
        {
            var formules = new List<Formule>();
            if (quittancesDto.Count > 0)
            {
                foreach (var quittanceDto in quittancesDto)
                {
                    var formule = new Formule();
                    formule.Libelle = string.IsNullOrEmpty(quittanceDto.LibelleFormule) ? quittanceDto.LettreFormule : quittanceDto.LettreFormule + "-" + quittanceDto.LibelleFormule;
                    formule.Risque = string.IsNullOrEmpty(quittanceDto.LibelleRisque) ? quittanceDto.CodeRisque.ToString() : quittanceDto.CodeRisque + "-" + quittanceDto.LibelleRisque;
                    formule.HtHorsCatnat = quittanceDto.HtHorsCatnat.ToString();
                    formule.CatNat = quittanceDto.CatNat.ToString();
                    formule.Taxes = quittanceDto.Taxes.ToString();
                    formule.Ttc = quittanceDto.Ttc.ToString(); ;
                    formule.FinEffet = (quittanceDto.FinEffetAnnee != 0 && quittanceDto.FinEffetMois != 0 && quittanceDto.FinEffetJour != 0) ? new DateTime(quittanceDto.FinEffetAnnee, quittanceDto.FinEffetMois, quittanceDto.FinEffetJour).ToShortDateString() : string.Empty;
                    formules.Add(formule);
                }
            }
            return formules;
        }
    }
}