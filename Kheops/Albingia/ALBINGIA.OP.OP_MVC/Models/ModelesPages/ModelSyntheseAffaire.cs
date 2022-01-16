using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using OP.WSAS400.DTO.Regularisation;
using OP.WSAS400.DTO.AffaireNouvelle;
using System.Collections.Generic;
using System.Linq;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModelSyntheseAffaire : MetaModelsBase
    {
        public AffaireDto Affaire { get; set; }
        public List<ModeleAvenantAlerte> AlertesAvenant { get; set; }
        public string NumeroAffaire => Affaire?.Numero;
        public bool IsOffre => Affaire?.TypeAffaire == AffaireType.Offre;
        public bool IsAvenant => Affaire?.NumeroAvenant > 0 && Affaire?.TypeAffaire == AffaireType.Contrat;
        public bool HasAlertes => AlertesAvenant?.Any() ?? false;
        public bool IsKheopsAffaire => Affaire?.IsKheops ?? false;
        public string TypeAffaire => Affaire?.TypeAffaire.ToString();
        public string BrancheCible
        {
            get
            {
                if (Affaire?.Branche != null && Affaire?.Cible != null)
                {
                    return $"{Affaire.Branche.Code} - {Affaire.Cible.Description}";
                }
                return Affaire?.Branche?.Code ?? string.Empty;
            }
        }
        public bool HasOffreOrigine => Affaire?.OffreOrigine != null;
        public string OffreOrigine => Affaire?.OffreOrigine is null ? string.Empty : $"{Affaire.OffreOrigine.CodeAffaire} - {Affaire.OffreOrigine.NumeroAliment}";
        public string CodeOffreOrigineLink => Affaire?.OffreOrigine is null ? string.Empty : $"{Affaire.OffreOrigine.CodeAffaire}_{Affaire.OffreOrigine.NumeroAliment}_{AffaireType.Offre.AsCode()}ConsultOnly";
        public string CodeOffreOrigine => Affaire?.OffreOrigine is null ? string.Empty : $"{Affaire.OffreOrigine.CodeAffaire}_{Affaire.OffreOrigine.NumeroAliment}_{AffaireType.Offre.AsCode()}";
        public string CodePreneur => (Affaire?.Preneur?.Code).GetValueOrDefault() < 1 ? string.Empty : Affaire.Preneur.Code.ToString();
        public string Preneur => CodePreneur.Length == 0 ? string.Empty : $"{Affaire.Preneur.Code} - {Affaire.Preneur.NomAssure}";
        public string PreneurVille => Affaire?.Preneur?.Adresse is null ? string.Empty : $"{Affaire.Preneur.Adresse.CodePostal} {Affaire.Preneur.Adresse.Ville}";

        public string CourtierApporteur => Affaire?.CourtierApporteur is null || Affaire.CourtierApporteur.Code < 1 ? string.Empty : $"{Affaire.CourtierApporteur.Code} - {Affaire.CourtierApporteur.Nom}";
        public string CourtierApporteurVille => Affaire?.CourtierApporteur?.Adresse is null ? string.Empty : $"{Affaire.CourtierApporteur.Adresse.CodePostal} {Affaire.CourtierApporteur.Adresse.Ville}";
        public string CodeApporteur => CourtierApporteur.IsEmptyOrNull() ? string.Empty : Affaire.CourtierApporteur.Code.ToString();

        public string CourtierGestionnaire => Affaire?.CourtierGestionnaire is null || Affaire.CourtierGestionnaire.Code < 1 ? string.Empty : $"{Affaire.CourtierGestionnaire.Code} - {Affaire.CourtierGestionnaire.Nom}";
        public string CourtierGestionnaireVille => Affaire?.CourtierGestionnaire?.Adresse is null ? string.Empty : $"{Affaire.CourtierGestionnaire.Adresse.CodePostal} {Affaire.CourtierGestionnaire.Adresse.Ville}";
        public string CodeGestionnaire => CourtierGestionnaire.IsEmptyOrNull() ? string.Empty : Affaire.CourtierGestionnaire.Code.ToString();
        public string CodeInterlocuteur => Affaire?.CodeInterlocuteur ?? string.Empty;

        public string CourtierPayeur => Affaire?.CourtierPayeur is null || Affaire?.CourtierPayeur.Code < 1 ? string.Empty : $"{Affaire.CourtierPayeur.Code} - {Affaire.CourtierPayeur.Nom}";
        public string CourtierPayeurVille => Affaire?.CourtierPayeur?.Adresse is null ? string.Empty : $"{Affaire.CourtierPayeur.Adresse.CodePostal} {Affaire.CourtierPayeur.Adresse.Ville}";
        public string CodePayeur => CourtierPayeur.IsEmptyOrNull() ? string.Empty : Affaire.CourtierPayeur.Code.ToString();

        public string Encaissement => (Affaire?.Encaissement?.Code).IsEmptyOrNull() ? string.Empty : $"{Affaire.Encaissement.Code} - {Affaire.Encaissement.LibelleLong}";
        public string Delegation { get; set; }
        public string Inspecteur { get; set; }
        public string Secteur { get; set; }
        public string Souscripteur => Affaire?.Souscripteur is null ? string.Empty : Affaire.Souscripteur.Code;
        public string Gestionnaire => Affaire?.Gestionnaire is null ? string.Empty : Affaire.Gestionnaire.Code;

        public string DateProchaineEcheance => Affaire?.ProchaineEcheance?.Date?.ToShortDateString();
        public string PeriodiciteAffaire => (Affaire?.Periodicite?.Code).IsEmptyOrNull() ? string.Empty : $"{Affaire.Periodicite.Code} - {Affaire.Periodicite.LibelleLong}";
        //public string EcheancePrincipale => Affaire?.ProchaineEcheance?.Date is null ? string.Empty : Affaire.ProchaineEcheance.Date?.ToString("dd/MM");
        public string EcheancePrincipale => Affaire?.Echeance is null || Affaire?.Echeance?.Year == 1 ? string.Empty : Affaire.Echeance?.ToString("dd/MM");
        public string Preavis => Affaire is null ? string.Empty : $"{Affaire.PreavisMois} mois";
        public string NatureAffaire
        {
            get
            {
                if ((Affaire?.Nature?.LibelleLong).IsEmptyOrNull())
                {
                    return string.Empty;
                }
                if ((Affaire?.Nature?.Code).IsEmptyOrNull())
                {
                    return Affaire.Nature.LibelleLong;
                }
                return $"{Affaire.Nature.Code} - {Affaire.Nature.LibelleLong}";
            }
        }
        public string PourcentagePart => (Affaire?.PartAlbingia ?? 0).ToString("F2") + " %";
        public string RegimeTaxes => (Affaire?.RegimeTaxe?.Code).IsEmptyOrNull() ? string.Empty : $"{Affaire.RegimeTaxe.Code} - {Affaire.RegimeTaxe.LibelleLong}";
        public string HasCATNAT => (Affaire?.SoumisCatNat ?? false) ? Booleen.Oui.ToString() : Booleen.Non.ToString();
        public bool HasIndice => (Affaire?.IndiceReference?.Code).ContainsChars();
        public string LibelleIndice => HasIndice ? $"{Affaire.IndiceReference.Code} - {Affaire.IndiceReference.LibelleLong}" : string.Empty;
        public string ValeurIndice => HasIndice ? Affaire.Valeur.ToString() : string.Empty;
        public string MontantRef => (Affaire?.MontantReference ?? 0).ToString("N2");
        public string CodeSTOP => (Affaire?.CodeSTOP?.Code).IsEmptyOrNull() ? string.Empty : $"{Affaire.CodeSTOP.Code} - {Affaire.CodeSTOP.LibelleLong}";
        public string PourcentageTauxCommission => (Affaire?.TauxCommission ?? 0).ToString("F2") + " %";
        public string PourcentageTauxCommissionCATNAT => (Affaire?.TauxCommissionCATNAT ?? 0).ToString("F2") + " %";
        public string DerniereActionLibelle {
            get {
                var dttr = Affaire?.DernierTraitement?.LibelleLong;
                var ttr = Affaire?.TypeTraitement?.LibelleLong;
                if (Affaire.ModeRegularisation != null && Affaire.ModeRegularisation.Code != RegularisationMode.Standard.AsCode()) {
                    return dttr;
                }
                return ttr ?? dttr;
            }
        }

        public string DerniereActionMotif => !(Affaire?.MotifResiliation?.Code).IsEmptyOrNull() ?
            $"{Affaire.MotifResiliation.Code} - {Affaire.MotifResiliation.LibelleLong}" :
            !(Affaire?.MotifRegularisation?.Code).IsEmptyOrNull() ?
               $"{Affaire.MotifRegularisation.Code} - {Affaire.MotifRegularisation.LibelleLong}" :
            Affaire?.NumeroAvenant > 0 && !(Affaire?.MotifAvenant?.Code).IsEmptyOrNull() ? 
                $"{Affaire.MotifAvenant.Code} - {Affaire.MotifAvenant.LibelleLong}" : 
                (Affaire?.MotifSituation?.Code).IsEmptyOrNull() ? 
                    string.Empty : 
                    $"{Affaire.MotifSituation.Code} - {Affaire.MotifSituation.LibelleLong}";
        public string EtatAffaire => (Affaire?.EtatAffaire?.Code).IsEmptyOrNull() ? string.Empty : $"{Affaire.EtatAffaire.Code} - {Affaire.EtatAffaire.LibelleLong}";
        public string SituationAffaire => (Affaire?.SituationAffaire?.Code).IsEmptyOrNull() ? string.Empty : $"{Affaire.SituationAffaire.Code} - {Affaire.SituationAffaire.LibelleLong}";
        public string DateSituation => Affaire?.DateMajTraitement?.ToShortDateString();
        public List<CreationAffaireNouvelleContratDto> getLstContrat { get; set; }
    /*public string getContrat => Affaire?.OffreOrigine.CodeAffaire?*/
    }
}