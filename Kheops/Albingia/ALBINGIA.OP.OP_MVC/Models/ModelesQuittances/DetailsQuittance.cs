using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Cotisations;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class DetailsQuittance
    {
        public string ModeAffichage;
        public string NumQuittanceVisu;

        #region Informations détails

        public string NatureContrat { get; set; }
        public string Part { get; set; }
        public string Avenant { get; set; }
        public string Devise { get; set; }
        public string Operation { get; set; }
        public string Capital { get; set; }
        public string Periodicite { get; set; }
        public string DateDebutPeriodicite { get; set; }
        public string DateFinPeriodicite { get; set; }
        public string ValeurIndice { get; set; }
        public string Emission { get; set; }
        public string DateEmission { get; set; }
        public string Accord { get; set; }
        public string DateEcheance { get; set; }
        public string Mouvement { get; set; }
        public string Situation { get; set; }
        public string DateSituation { get; set; }
        public string CodeRelance { get; set; }
        public string DateRelance { get; set; }
        public string Comptabilise { get; set; }
        public string DateComptabilise { get; set; }

        public string ARegler { get; set; }
        public string Regle { get; set; }
        public string ResteARegler { get; set; }

        public string CourtierComptable { get; set; }
        public string CourtierComptableCompet { get; set; }
        public string Encaissement { get; set; }
        public bool Icone { get; set; }
        public int NombreCocourtier { get; set; }



        public string UserCreation { get; set; }
        public string DateCreation { get; set; }
        public string UserUpdate { get; set; }
        public string DateUpdate { get; set; }

        public string CodeNatureContrat { get; set; }
        public string ComplementTitre { get; set; }
        #endregion

        //public DetailsQuittanceCommission Commission { get; set; }
        //public DetailsQuittanceTotaux Totaux { get; set; }
        public Totaux Totaux { get; set; }
        public Commission Commission { get; set; }
        public void LoadInformationsDetails(QuittanceDetailDto quittanceDetailDto)
        {
            NatureContrat = string.IsNullOrEmpty(quittanceDetailDto.LibelleNatureContrat) ? quittanceDetailDto.CodeNatureContrat : quittanceDetailDto.CodeNatureContrat + "-" + quittanceDetailDto.LibelleNatureContrat;
            Part = quittanceDetailDto.Part.ToString();
            Avenant = quittanceDetailDto.Avenant.ToString();
            Devise = quittanceDetailDto.Devise;
            Operation = string.IsNullOrEmpty(quittanceDetailDto.LibelleOperation) ? quittanceDetailDto.CodeOperation.ToString() : quittanceDetailDto.CodeOperation + "-" + quittanceDetailDto.LibelleOperation;
            Capital = quittanceDetailDto.Capital.ToString();

            CodeNatureContrat = quittanceDetailDto.CodeNatureContrat;
            Periodicite = string.IsNullOrEmpty(quittanceDetailDto.LibellePeriodicite) ? quittanceDetailDto.CodePeriodicite : quittanceDetailDto.CodePeriodicite + "-" + quittanceDetailDto.LibellePeriodicite;
            if (!string.IsNullOrEmpty(CodeNatureContrat))
            {
                if (CodeNatureContrat == "A" || CodeNatureContrat == "E")
                    ComplementTitre = "Toutes les valeurs correspondent à la part totale";
                if (CodeNatureContrat == "C" || CodeNatureContrat == "D")
                    ComplementTitre = "Toutes les valeurs correspondent à la part Albingia";
            }

            if (quittanceDetailDto.DebutPeriodeAnnee != 0 && quittanceDetailDto.DebutPeriodeMois != 0 && quittanceDetailDto.DebutPeriodeJour != 0)
                DateDebutPeriodicite = new DateTime(quittanceDetailDto.DebutPeriodeAnnee, quittanceDetailDto.DebutPeriodeMois, quittanceDetailDto.DebutPeriodeJour).ToShortDateString();
            /* SAB: bug 2512 Coulm vu avec FDU */
            //if ((quittanceDetailDto.CodePeriodicite == "T" || quittanceDetailDto.CodePeriodicite == "S" || quittanceDetailDto.CodePeriodicite == "A" || quittanceDetailDto.CodePeriodicite == "R") && (quittanceDetailDto.FinPeriodeAnnee != 0 && quittanceDetailDto.FinPeriodeMois != 0 && quittanceDetailDto.FinPeriodeJour != 0))
            if (quittanceDetailDto.FinPeriodeAnnee != 0 && quittanceDetailDto.FinPeriodeMois != 0 && quittanceDetailDto.FinPeriodeJour != 0)
                DateFinPeriodicite = new DateTime(quittanceDetailDto.FinPeriodeAnnee, quittanceDetailDto.FinPeriodeMois, quittanceDetailDto.FinPeriodeJour).ToShortDateString();

            DateTime? finEffetDossier = null;
            if (quittanceDetailDto.DateFinEffetPoliceAnnee > 0 && quittanceDetailDto.DateFinEffetPoliceMois > 0 && quittanceDetailDto.DateFinEffetPoliceJour > 0)
                finEffetDossier = new DateTime(quittanceDetailDto.DateFinEffetPoliceAnnee, quittanceDetailDto.DateFinEffetPoliceMois, quittanceDetailDto.DateFinEffetPoliceJour);
            if (!finEffetDossier.HasValue)
            {
                finEffetDossier = AlbConvert.GetFinPeriode(AlbConvert.ConvertStrToDate(DateDebutPeriodicite), quittanceDetailDto.DureeEffetPolice, quittanceDetailDto.UniteDureeEffetPolice);
            }
            /* SAB: bug 2512 Coulm vu avec FDU */
            //if (string.IsNullOrEmpty(DateFinPeriodicite))
            //    DateFinPeriodicite = finEffetDossier.HasValue ? finEffetDossier.Value.ToString("dd/MM/yyyy") : string.Empty;

            Emission = string.IsNullOrEmpty(quittanceDetailDto.LibelleEmission) ? quittanceDetailDto.CodeEmission : quittanceDetailDto.CodeEmission + "-" + quittanceDetailDto.LibelleEmission;
            if (quittanceDetailDto.DateEmissionAnnee != 0 && quittanceDetailDto.DateEmissionMois != 0 && quittanceDetailDto.DateEmissionJour != 0)
                DateEmission = new DateTime(quittanceDetailDto.DebutPeriodeAnnee, quittanceDetailDto.DateEmissionMois, quittanceDetailDto.DateEmissionJour).ToShortDateString();

            if (quittanceDetailDto.DateEcheanceAnnee != 0 && quittanceDetailDto.DateEcheanceMois != 0 && quittanceDetailDto.DateEcheanceJour != 0)
                DateEcheance = new DateTime(quittanceDetailDto.DateEcheanceAnnee, quittanceDetailDto.DateEcheanceMois, quittanceDetailDto.DateEcheanceJour).ToShortDateString();

            Situation = string.IsNullOrEmpty(quittanceDetailDto.LibelleSituation) ? quittanceDetailDto.CodeSituation : quittanceDetailDto.CodeSituation + "-" + quittanceDetailDto.LibelleSituation;
            if (quittanceDetailDto.DateSituationAnnee != 0 && quittanceDetailDto.DateSituationMois != 0 && quittanceDetailDto.DateSituationJour != 0)
                DateSituation = new DateTime(quittanceDetailDto.DateSituationAnnee, quittanceDetailDto.DateSituationMois, quittanceDetailDto.DateSituationJour).ToShortDateString();

            CodeRelance = string.IsNullOrEmpty(quittanceDetailDto.LibelleRelance) || quittanceDetailDto.LibelleRelance.Trim() == "-" ? quittanceDetailDto.CodeRelance : quittanceDetailDto.CodeRelance + "-" + quittanceDetailDto.LibelleRelance;
            if (quittanceDetailDto.DateRelanceAnnee != 0 && quittanceDetailDto.DateRelanceMois != 0 && quittanceDetailDto.DateRelanceJour != 0)
                DateRelance = new DateTime(quittanceDetailDto.DateRelanceAnnee, quittanceDetailDto.DateRelanceMois, quittanceDetailDto.DateRelanceJour).ToShortDateString();

            ValeurIndice = quittanceDetailDto.Indice.ToString();
            Accord = string.IsNullOrEmpty(quittanceDetailDto.LibelleAccord) ? quittanceDetailDto.CodeAccord : quittanceDetailDto.CodeAccord + "-" + quittanceDetailDto.LibelleAccord;
            Mouvement = string.IsNullOrEmpty(quittanceDetailDto.LibelleMouvement) ? quittanceDetailDto.CodeMouvement : quittanceDetailDto.CodeMouvement + "-" + quittanceDetailDto.LibelleMouvement;
            Comptabilise = quittanceDetailDto.Comptabilise;
            DateComptabilise = quittanceDetailDto.DateComptabiliseMois + "/" + quittanceDetailDto.DateComptabiliseAnnee;
            Encaissement = string.IsNullOrEmpty(quittanceDetailDto.LibelleEncaissement) ? quittanceDetailDto.CodeEncaissement : quittanceDetailDto.CodeEncaissement + "-" + quittanceDetailDto.LibelleEncaissement;

            Icone = quittanceDetailDto.PNICTIcone == quittanceDetailDto.PKICTIcone ? true : false;
            CourtierComptable = string.IsNullOrEmpty(quittanceDetailDto.NomCourtier) ? quittanceDetailDto.CodeCourtier.ToString() : quittanceDetailDto.CodeCourtier + "-" + quittanceDetailDto.NomCourtier;
            CourtierComptableCompet = CourtierComptable + "-" + quittanceDetailDto.CpCourtier + "-" + quittanceDetailDto.VillCourtier + "-" + quittanceDetailDto.DepCourtier;
            NombreCocourtier = quittanceDetailDto.NombreCocourtier;
            ARegler = quittanceDetailDto.TTCARgler.ToString();
            Regle = quittanceDetailDto.Regle.ToString();
            ResteARegler = (quittanceDetailDto.TTCARgler - quittanceDetailDto.Regle).ToString();

            UserCreation = quittanceDetailDto.UserCreation;
            if (quittanceDetailDto.DateCreationAnnee != 0 && quittanceDetailDto.DateCreationMois != 0 && quittanceDetailDto.DateCreationJour != 0)
                DateCreation = new DateTime(quittanceDetailDto.DateCreationAnnee, quittanceDetailDto.DateCreationMois, quittanceDetailDto.DateCreationJour).ToShortDateString();
            UserUpdate = quittanceDetailDto.UserUpdate;
            if (quittanceDetailDto.DateUserUpdateAnnee != 0 && quittanceDetailDto.DateUserUpdateMois != 0 && quittanceDetailDto.DateUserUpdateJour != 0)
                DateUpdate = new DateTime(quittanceDetailDto.DateUserUpdateAnnee, quittanceDetailDto.DateUserUpdateMois, quittanceDetailDto.DateUserUpdateJour).ToShortDateString();

        }
    }
}