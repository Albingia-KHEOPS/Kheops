using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using OP.DataAccess;
using OP.Services.BLServices;
using OP.Services.Common;
using OP.WSAS400.DTO;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;

namespace OP.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class StepFinder : IStepFinder
    {
        private readonly IDbConnection dbConnection;
        private readonly IAffairePort affaireService;
        private readonly OffreRepository offreRepository;
        private readonly RegularisationRepository regularisationRepository;
        private readonly AffaireNouvelleRepository affaireNouvelleRepository;
        private readonly IRegularisationPort regularisationService;

        private readonly FolderService folderService;

        public StepFinder(IDbConnection dbConnection, IAffairePort affaireService, FolderService folderService, OffreRepository offreRepository, RegularisationRepository regularisationRepository, AffaireNouvelleRepository affaireNouvelleRepository, IRegularisationPort regularisationService)
        {
            this.dbConnection = dbConnection;
            this.affaireService = affaireService;
            this.folderService = folderService;
            this.offreRepository = offreRepository;
            this.regularisationRepository = regularisationRepository;
            this.affaireNouvelleRepository = affaireNouvelleRepository;
            this.regularisationService = regularisationService;
        }

        public StepContext Find(StepContext stepContext)
        {
            if (!Enum.IsDefined(typeof(ContextStepName), stepContext.Target)) {
                throw new ArgumentException($"{nameof(stepContext)}.{nameof(stepContext.Target)}");
            }

            if (this.folderService.IsCitrix(stepContext.Folder))
            {
                stepContext.IsAborted = true;
                stepContext.DenyReason = "Citrix";
                return stepContext;
            }

            if (stepContext.Origin.IsEmptyOrNull())
            {
                return FindWithoutOrigin(stepContext);
            }

            throw new NotImplementedException();
        }

        public StepContext FindNext(StepContext stepContext)
        {

            throw new NotImplementedException();
        }

        public StepContext FindPrevious(StepContext stepContext)
        {
            throw new NotImplementedException();
        }

        private StepContext FindWithoutOrigin(StepContext stepContext)
        {
            switch (stepContext.Target)
            {
                case ContextStepName.ConsulterRegule:
                case ContextStepName.ConsulterPB:
                    return TryConsultRegularisation(stepContext);
                case ContextStepName.ConsulterOuEditer:
                case ContextStepName.Edition:
                case ContextStepName.EditionRegularisation:
                case ContextStepName.EditionBNS:
                case ContextStepName.EditionPB:
                case ContextStepName.EngagementPeriodes:
                case ContextStepName.EtablirAffaireNouvelle:
                case ContextStepName.DoubleSaisie:
                case ContextStepName.RetourPieces:
                case ContextStepName.PrisePosition:
                case ContextStepName.BlocageDeblocageTermes:
                case ContextStepName.Validation:
                    return TryGetEditStep(stepContext);
                default:
                    stepContext.IsAborted = true;
                    stepContext.DenyReason = "Pas de page cible identifiable";
                    return stepContext;
            }
        }

        private StepContext TryGetEditStep(StepContext stepContext)
        {
            string user = WCFHelper.GetFromHeader("UserAS400");
            var affaireId = stepContext.Folder.Adapt<AffaireId>();
            bool isReadonlyEdit = stepContext.Target.IsIn(ContextStepName.Edition, ContextStepName.EditionRegularisation) && stepContext.IsReadonlyTarget;

            // affair is either history or current avenant, the lazy evaluation of "or" is VERY important here
            if (TryAccessHistory(stepContext, out Affaire affaire) || !IsAllowedEditingAffair(stepContext, user, out affaire)) {
                if (affaire is null) {
                    return stepContext;
                }
                // always allow modify Retour Piece
                if (stepContext.Target != ContextStepName.RetourPieces) {
                    SetAvnParams(stepContext, affaire);
                    return stepContext;
                }
            }

            stepContext.Folder.NumeroAvenant = affaire.NumeroAvenant;
            if (!CheckOffre(stepContext, affaire)) {
                return stepContext;
            }

            if (!CheckConsulOrEdit(stepContext, affaire)) {
                return stepContext;
            }

            var verrou = this.affaireService.GetCurrentLock(affaireId);
            
            if (stepContext.IsReadonlyTarget) {
                if (verrou != null) {
                    stepContext.IsUserLocking = verrou.User == user;
                    stepContext.LockingUser = verrou.User;
                }
            }
            else {
                if (!EnsureEdit(stepContext, affaire)) {
                    return stepContext;
                }
                
                if (verrou != null) {
                    stepContext.IsUserLocking = verrou.User == user;
                    stepContext.LockingUser = verrou.User;
                }
            }
            if (verrou is null) {
                return FindUniqueEditStep(stepContext, affaire);
            }
            else {
                if (isReadonlyEdit) {
                    FindUniqueEditStep(stepContext, affaire);
                }
                stepContext.DenyReason = "locked";
                stepContext.IsReadonlyTarget = true;
                return stepContext;
            }
        }

        private bool IsAllowedEditingAffair(StepContext stepContext, string user, out Affaire affair)
        {
            affair = this.folderService.GetBasicAffaire(stepContext.Folder, fromHistory: stepContext.ModeNavig == ModeConsultation.Historique.AsCode());
            if (affair is null)
            {
                stepContext.IsAborted = true;
                stepContext.DenyReason = "L'affaire sélectionnée n'existe pas";
                return false;
            }
            if (stepContext.IsReadonlyTarget)
            {
                return true;
            }
            string br = affair.Branche.Code;
            var userRights = CommonOffre.GetAllUserRights().Where(x => x.Utilisateur == user);
            if (!userRights.Any(x => (x.Branche == br || x.Branche == "**") && x.TypeDroit != TypeDroit.V.ToString()))
            {
                stepContext.IsAborted = true;
                stepContext.DenyReason = $"Vous n'avez pas les droits de modification sur la branche {br}";
                return false;
            }
            return true;
        }

        private bool TryAccessHistory(StepContext stepContext, out Affaire affaire)
        {
            affaire = default;
            if (stepContext.IsModeHisto)
            {
                affaire = this.folderService.GetBasicAffaire(stepContext.Folder, true);
                if (affaire is null)
                {
                    stepContext.IsAborted = true;
                    affaire = this.folderService.GetBasicAffaire(stepContext.Folder);
                    if (affaire is null)
                    {
                        stepContext.DenyReason = "L'affaire sélectionnée n'existe pas";
                    }
                    else
                    {
                        stepContext.DenyReason = "L'affaire sélectionnée n'est pas présente dans l'historique";
                    }
                }

                stepContext.IsReadonlyTarget = stepContext.Target != ContextStepName.RetourPieces;
                return true;
            }
            return false;
        }

        private StepContext FindUniqueEditStep(StepContext stepContext, Affaire affaire)
        {
            if (stepContext.Target != 0) {
                if (stepContext.Target.IsIn(
                    ContextStepName.EtablirAffaireNouvelle,
                    ContextStepName.PrisePosition,
                    ContextStepName.DoubleSaisie,
                    ContextStepName.RetourPieces,
                    ContextStepName.ClassementSansSuite,
                    ContextStepName.BlocageDeblocageTermes)) {
                    return stepContext;
                }
                if (stepContext.Target == ContextStepName.EditionRegularisation) {
                    // reset to edition to ensure regul/regul+avn
                    stepContext.Target = ContextStepName.Edition;
                }
                if (!stepContext.Target.IsIn(ContextStepName.Edition, ContextStepName.Validation, ContextStepName.EditionBNS, ContextStepName.EditionPB, ContextStepName.EngagementPeriodes)) {
                    throw new ArgumentException($"{nameof(stepContext)}.{nameof(stepContext.Target)}");
                }
            }
            if (affaire.NumeroAvenant == 0 && stepContext.Target != ContextStepName.EditionBNS)
            {
                var targetBefore = stepContext.Target;
                SetEditStepNonAvn(stepContext, affaire);
                if (targetBefore != stepContext.Target) {
                    return stepContext;
                }
            }
            else
            {
                SetAvnParams(stepContext, affaire);
            }
            if (stepContext.NewProcessCode.IsEmptyOrNull() && affaire.TypeAffaire != AffaireType.Offre) {
                if (stepContext.Target != ContextStepName.EditionBNS) {
                    stepContext.Target = GetEditStepFromTraitement(stepContext, affaire);
                }
            }
            else
            {
                ReassignTargetContrat(stepContext, affaire);
            }

            return stepContext;
        }

        private StepContext TryConsultRegularisation(StepContext stepContext) {
            var affaireId = stepContext.Folder.Adapt<AffaireId>();
            var currentId = new AffaireId {
                CodeAffaire = affaireId.CodeAffaire,
                IsHisto = false,
                NumeroAliment = affaireId.NumeroAliment,
                TypeAffaire = AffaireType.Contrat
            };
            AffaireDto current = this.affaireService.GetAffaire(currentId);
            if (current is null) {
                throw new ArgumentException($"{nameof(stepContext)}.{nameof(stepContext.Folder)}");
            }
            AffaireDto affaire = null;
            if (!stepContext.IsModeHisto) {
                affaire = current;
            }
            if (affaire is null || affaire.NumeroAvenant != stepContext.Folder.NumeroAvenant) {
                affaireId.IsHisto = true;
                affaire = this.affaireService.GetAffaire(affaireId);

                // check whether the histo flag is irrelevant
                if (affaire is null && current.NumeroAvenant == affaireId.NumeroAvenant) {
                    affaireId.IsHisto = false;
                    stepContext.ModeNavig = ModeConsultation.Standard.AsCode();
                    affaire = current;
                }
            }
            if (affaire is null
                || affaire.TypeTraitement.Code != AlbConstantesMetiers.TYPE_AVENANT_REGUL
                    && affaire.TypeTraitement.Code != AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF
                    && affaire.TypeTraitement.Code != AlbConstantesMetiers.TYPE_AVENANT_PB) {
                throw new ArgumentException($"{nameof(stepContext)}.{nameof(stepContext.Folder)}");
            }
            if (affaireId.IsHisto) {
                // force navigation mode (IsModeHisto)
                stepContext.ModeNavig = ModeConsultation.Historique.AsCode();
            }
            stepContext.IsReadonlyTarget = true;
            SetAvnParams(stepContext, affaire.Adapt<Affaire>());
            return stepContext;
        }

        static bool? CheckNewProcess(StepContext stepContext, Affaire affaire) {
            if (stepContext.NewProcessCode.IsEmptyOrNull()) {
                return null;
            }
            if (affaire.Etat != EtatAffaire.Validee) {
                return false;
            }
            bool invalidOperation = false;
            switch (stepContext.NewProcessCode) {
                case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                    if (!affaire.Situation.IsIn(SituationAffaire.Resiliee, SituationAffaire.EnCours)) {
                        invalidOperation = true;
                    }
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    if (affaire.Situation != SituationAffaire.EnCours) {
                        invalidOperation = true;
                    }
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    if (affaire.Situation != SituationAffaire.Resiliee) {
                        invalidOperation = true;
                    }
                    break;
                default:
                    break;
            }
            return !invalidOperation;
        }

        private void SetAvnParams(StepContext stepContext, Affaire affaire)
        {
            if (stepContext.Folder.Type == AffaireType.Offre.AsCode())
            {
                stepContext.AvnParams = null;
                return;
            }
            if (affaire.NumeroAvenant < 1 && affaire.TypeAffaire == AffaireType.Contrat && stepContext.Target != ContextStepName.EditionBNS)
            {
                if (stepContext.AvnParams != null)
                {
                    stepContext.AvnParams.Clear();
                }
                return;
            }
            if (stepContext.Target == ContextStepName.EditionBNS) {
                if (!stepContext.AvnParams?.Any() ?? true) {
                    stepContext.AvnParams = new Dictionary<string, string> {
                        { AlbParameterName.AVNID.ToString(), affaire.NumeroAvenant.ToString() },
                        { AlbParameterName.AVNTYPE.ToString(), AlbConstantesMetiers.TYPE_AVENANT_REGUL },
                        { AlbParameterName.AVNIDEXTERNE.ToString(), affaire.NumeroAvenant.ToString() },
                        { AlbParameterName.AVNMODE.ToString(), stepContext.IsModeHisto || stepContext.IsReadonlyTarget ? "CONSULT" : "UPDATE" }
                    };
                }
                else
                {
                    var bns = regularisationService.GetBNSDtoByAffaire(affaire.Adapt<AffaireId>());
                    //stepContext.IsReadonlyTarget = (bns != null && bns.CodeEtat == "V");
                    stepContext.AvnParams[AlbParameterName.AVNMODE.ToString()] = bns == null ? "CREATE" : bns.CodeEtat == "V" ? "CONSULT" : "UPDATE";

                }
            }
            else {
                string avnMode = stepContext.IsModeHisto || stepContext.IsReadonlyTarget ? "CONSULT" : (affaire.Etat == EtatAffaire.Validee ? "CREATE" : "UPDATE");
                if (!stepContext.AvnParams?.Any() ?? true) {
                    stepContext.AvnParams = new Dictionary<string, string> {
                        { AlbParameterName.AVNID.ToString(), affaire.NumeroAvenant.ToString() },
                        { AlbParameterName.AVNTYPE.ToString(), affaire.TypeTraitement.Code },
                        { AlbParameterName.AVNIDEXTERNE.ToString(), affaire.NumeroAvenant.ToString() },
                        { AlbParameterName.AVNMODE.ToString(), avnMode }
                    };
                }
            }
            SetAvnParamsRegul(stepContext, affaire);
        }

        private void SetAvnParamsRegul(StepContext stepContext, Affaire affair)
        {
            if (stepContext.Target != ContextStepName.EditionBNS
                && !affair.TypeTraitement.Code.IsIn(AlbConstantesMetiers.TYPE_AVENANT_REGUL, AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF, AlbConstantesMetiers.TYPE_AVENANT_PB)
                && !stepContext.NewProcessCode.IsIn(AlbConstantesMetiers.TYPE_AVENANT_REGUL, AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF, AlbConstantesMetiers.TYPE_AVENANT_PB))
            {
                return;
            }
            if (stepContext.NewProcessCode.IsIn(AlbConstantesMetiers.TYPE_AVENANT_REGUL, AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)) {
                if (!stepContext.AvnParams?.Any() ?? true) {
                    throw new ArgumentException($"{nameof(stepContext)}.{nameof(stepContext.AvnParams)}");
                }
            }
            else {
                var regul = this.regularisationRepository.GetByFolder(stepContext.Folder, affair.NumeroAvenant);
                if (regul != null) {
                    stepContext.AvnParams.Add(AlbParameterName.REGULEID.ToString(), regul.NumRegule.ToString());
                    stepContext.AvnParams.Add(AlbParameterName.REGULMOD.ToString(), regul.RegulMode);
                    stepContext.AvnParams.Add(AlbParameterName.REGULTYP.ToString(), regul.RegulType);
                    stepContext.AvnParams.Add(AlbParameterName.REGULNIV.ToString(), regul.RegulNiv);
                    stepContext.AvnParams.Add(AlbParameterName.REGULAVN.ToString(), regul.RegulAvn);
                }
            }
        }

        private void ReassignTargetContrat(StepContext stepContext, Affaire affaire) {
            if (affaire.TypeAffaire == AffaireType.Offre || stepContext.Target != ContextStepName.Edition || stepContext.NewProcessCode.IsEmptyOrNull()) {
                return;
            }
            if (affaire.NumeroAvenant == 0 && (!stepContext.AvnParams?.Any() ?? true)) {
                stepContext.AvnParams = new Dictionary<string, string> {
                    { AlbParameterName.AVNMODE.ToString(), "CREATE" }
                };
            }
            if (stepContext.AvnParams?.Any() ?? false) {
                switch (stepContext.NewProcessCode) {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        if (affaire.NumeroAvenant == 0) {
                            stepContext.Target = ContextStepName.CreationAvenant;
                        }
                        else if (stepContext.AvnParams[AlbParameterName.AVNMODE.ToString()] != "CONSULT") {
                            stepContext.Target = stepContext.AvnParams[AlbParameterName.AVNMODE.ToString()] == "CREATE"
                                ? ContextStepName.CreationAvenant
                                : ContextStepName.EditionAvenant;
                        }
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        if (stepContext.AvnParams[AlbParameterName.AVNMODE.ToString()] != "CONSULT") {
                            stepContext.Target = stepContext.AvnParams[AlbParameterName.AVNMODE.ToString()] == "CREATE"
                                ? ContextStepName.CreationRegularisation
                                : stepContext.NewProcessCode == AlbConstantesMetiers.TYPE_AVENANT_REGUL
                                    ? ContextStepName.EditionRegularisation
                                    : ContextStepName.EditionRegularisationEtAvenant;
                        }
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        stepContext.Target = ContextStepName.EditionResiliation;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                        stepContext.Target = ContextStepName.EditionRemiseEnVigueur;
                        break;
                    default:
                        break;
                }
                stepContext.AvnParams[AlbParameterName.AVNTYPE.ToString()] = stepContext.NewProcessCode;
            }
        }

        private bool CheckOffre(StepContext stepContext, Affaire offer)
        {
            if (stepContext.Folder.Type == AlbConstantesMetiers.TYPE_OFFRE)
            {
                if (stepContext.Target == ContextStepName.EtablirAffaireNouvelle) {
                    return CheckEtablirAN(stepContext, offer);
                }
                if (!CheckRealise(stepContext, offer)) {
                    return false;
                }
                var allOffers = this.offreRepository.GetAllByIPB(stepContext.Folder.CodeOffre);
                int max = allOffers.Max(o => o.Alx);
                var latestOffer = allOffers.First(o => o.Alx == max);
                if (stepContext.Target == ContextStepName.PrisePosition) {
                    // ok sauf offre réalisée ou double saisie
                    return true;
                }
                else if (stepContext.Target == ContextStepName.DoubleSaisie)
                {
                    if (max != offer.NumeroAliment)
                    {
                        stepContext.DenyReason = $"Veuillez sélectionner la version {max} pour toute modification de l'offre {stepContext.Folder.CodeOffre}";
                    }
                }
                else if (offer.IsValidated && offer.Situation == SituationAffaire.EnCours)
                {
                    if (latestOffer.Eta == EtatAffaire.Validee.AsCode())
                    {
                        stepContext.DenyReason = "validee";
                        stepContext.SuggestNewVersion = true;
                        stepContext.NextVersion = max + 1;
                    }
                    else
                    {
                        stepContext.DenyReason = $"Veuillez sélectionner la version {max} pour toute modification de l'offre {stepContext.Folder.CodeOffre}";
                    }
                }
                else if (offer.Situation == SituationAffaire.SansSuite && stepContext.Target != ContextStepName.DoubleSaisie)
                {
                    if (max == offer.NumeroAliment)
                    {
                        stepContext.DenyReason = (offer.Etat == EtatAffaire.Validee ? "validee" : "sans suite");
                        stepContext.SuggestNewVersion = true;
                        stepContext.NextVersion = max + 1;
                    }
                    else
                    {
                        stepContext.DenyReason = $"Veuillez sélectionner la version {max} pour toute modification de l'offre {stepContext.Folder.CodeOffre}";
                    }
                }
                else if (offer.CodeMotifSituation.ContainsChars())
                {
                    if (offer.IsValidated)
                    {
                        stepContext.DenyReason = "reprise";
                        stepContext.SuggestNewVersion = true;
                        stepContext.NextVersion = max + 1;
                    }
                }

                if (stepContext.DenyReason.ContainsChars())
                {
                    stepContext.IsReadonlyTarget = true;
                    return false;
                }
            }
            else if (stepContext.Target == ContextStepName.PrisePosition) {
                stepContext.Target = ContextStepName.ClassementSansSuite;
                if (offer.Situation != SituationAffaire.EnCours && offer.Situation != SituationAffaire.Resiliee)
                {
                    stepContext.IsAborted = true;
                    stepContext.DenyReason = "La situation du contrat ne permet pas le traitement.";
                    return false;
                } else if (offer.NumeroAvenant != 0 )
                {
                    stepContext.IsAborted = true;
                    stepContext.DenyReason = "Traitement impossible pour un avenant.";
                    return false;
                }
                else if (this.affaireNouvelleRepository.ContratHasPrimesEnCours(offer.CodeAffaire, offer.NumeroAliment.ToString(), offer.TypeAffaire.AsCode()))
                {
                    stepContext.IsAborted = true;
                    stepContext.DenyReason = "La cotisation ne peut être annulée pour cause de règlement, veuillez contacter le service comptable.";
                    return false;
                }
                else if (this.affaireNouvelleRepository.ContratHasPrimesReglees(offer.CodeAffaire, offer.NumeroAliment.ToString(), offer.TypeAffaire.AsCode()))
                {
                    stepContext.IsAborted = true;
                    stepContext.DenyReason = "Prime(s) réglée(s), contacter le service comptable pour annulation du règlement.";
                    return false;
                }
            }
            return true;
        }

        private bool CheckConsulOrEdit(StepContext stepContext, Affaire affaire) {
            bool isValid = true;
            if (stepContext.Target == ContextStepName.ConsulterOuEditer) {
                stepContext.Target = ContextStepName.Edition;
                if (affaire.Etat == EtatAffaire.Validee || affaire.Etat == EtatAffaire.Realisee) {
                    stepContext.IsReadonlyTarget = true;
                    if (affaire.NumeroAvenant > 0) {
                        stepContext.Target = ContextStepName.EditionAvenant;
                        SetAvnParams(stepContext, affaire);
                    }
                    isValid = false;
                }
            }
            else if (stepContext.Target == ContextStepName.Validation) {
                if (affaire.Situation != SituationAffaire.EnCours || affaire.Etat != EtatAffaire.NonValidee) {
                    isValid = false;
                }
            }

            if (!isValid) {
                stepContext.DenyReason = $"Impossible d'accéder à la fonctionnalité";
                stepContext.IsAborted = true;
            }

            return isValid;
        }

        private bool CheckEtablirAN(StepContext stepContext, Affaire affaire) {
            if (affaire.TypeAffaire != AffaireType.Offre) {
                return true;
            }
            if (!affaire.Etat.IsIn(EtatAffaire.Validee, EtatAffaire.Realisee) && affaire.Situation != SituationAffaire.EnCours) {
                stepContext.IsAborted = true;
                stepContext.DenyReason = "Impossible d'établir l'affaire nouvelle. L'offre doit être validée et en cours.";
                return false;
            }
            return true;
        }

        private void SetEditStepNonAvn(StepContext stepContext, Affaire affair)
        {
            if (affair.TypeAffaire == AffaireType.Offre && (affair.CodeMotifSituation.ContainsChars() || affair.Situation == SituationAffaire.Inconnu))
            {
                stepContext.Target = ContextStepName.ConfirmerOffre;
            }
            else
            {
                var an = this.affaireNouvelleRepository.GetNouvelleAffaire(stepContext.Folder);
                if (an?.Sit == "A") {
                    stepContext.Target = ContextStepName.NouvelleAffaire;
                    stepContext.PipedParams = new Dictionary<PipedParameter, IEnumerable<string>> {
                        { PipedParameter.ACTION, new List<string> { "OffreToAffaire" } },
                        { PipedParameter.IPB, new List<string> { an.OIpb, an.Ipb } },
                        { PipedParameter.ALX, new List<string> { an.OAlx.ToString(), an.Alx.ToString() } },
                        { PipedParameter.TYP, new List<string> { "O" } },
                        { PipedParameter.GUIDKEY, new List<string> { stepContext.TabGuid } }
                    };
                }
            }
        }

        static bool CheckRealise(StepContext stepContext, Affaire affaire)
        {
            if (affaire.Etat == EtatAffaire.Realisee)
            {
                if (affaire.TypeAffaire != AffaireType.Offre) {
                    throw new BusinessValidationException(new ValidationError("Seules les offres peuvent être réalisées."));
                }
                if (!stepContext.Target.IsIn(ContextStepName.EtablirAffaireNouvelle, ContextStepName.PrisePosition)) {
                    stepContext.IsReadonlyTarget = true;
                    stepContext.DenyReason = "L'offre est réalisée, vous ne pouvez pas la modifier";
                    stepContext.SuggestNewVersion = true;
                }
                return false;
            }
            return true;
        }

        static ContextStepName GetEditStepFromTraitement(StepContext stepContext, Affaire affair) {
            if (stepContext.Target == ContextStepName.EngagementPeriodes) {
                return stepContext.Target;
            }
            if (stepContext.Target == ContextStepName.EditionPB)
            {
                return stepContext.Target;
            }
            if ((affair.TypeTraitement.Type == TraitementAffaire.AffaireNouvelle && stepContext.Target != ContextStepName.Validation) || stepContext.IsModifHorsAvenant) {
                return ContextStepName.Edition;
            }
            if (!stepContext.IsReadonlyTarget)
            {
                if (stepContext.Target == ContextStepName.Validation) {
                    return ContextStepName.ControleFin;
                }
                if (affair.TypeAffaire == AffaireType.Contrat) {
                    if ((affair.IsValidated
                      || affair.Situation == SituationAffaire.SansSuite
                      || affair.CodeMotifSituation.ContainsChars())
                      && affair.TypeTraitement.Type != TraitementAffaire.Regularisation) {
                        return ContextStepName.CreationAvenant;
                    }
                    else {
                        if (affair.TypeTraitement.Type == TraitementAffaire.Regularisation) {
                            return ContextStepName.CreationRegularisation;
                        }
                    }
                }
            }
            return affair.TypeAffaire == AffaireType.Contrat ? ContextStepName.EditionAvenant : stepContext.Target;
        }

        static bool EnsureEdit(StepContext stepContext, Affaire affaire)
        {
            bool? newProcessOk = CheckNewProcess(stepContext, affaire);
            if (newProcessOk.HasValue) {
                if (newProcessOk.Value) {
                    return true;
                }
                stepContext.IsAborted = true;
                stepContext.DenyReason = "La situation du contrat est invalide pour cette opération";
                return false;
            }
            else {
                if (affaire.TypeAffaire == AffaireType.Contrat && stepContext.Target != ContextStepName.EditionBNS) {
                    if (affaire.Etat == EtatAffaire.Validee) {
                        if (!stepContext.IsModifHorsAvenant && !stepContext.Target.IsIn(ContextStepName.EngagementPeriodes, ContextStepName.RetourPieces, ContextStepName.ClassementSansSuite, ContextStepName.BlocageDeblocageTermes)) {
                            stepContext.IsReadonlyTarget = true;
                            stepContext.DenyReason = "Le contrat est validé, vous ne pouvez pas le modifier";
                        }
                    }
                    else if (affaire.Etat != EtatAffaire.Validee && (stepContext.IsModifHorsAvenant || stepContext.Target.IsIn(ContextStepName.EngagementPeriodes, ContextStepName.RetourPieces))) {
                        stepContext.IsReadonlyTarget = true;
                        stepContext.DenyReason = "Le contrat doit être validé pour réaliser ce processus";
                    }
                    else if (affaire.Situation == SituationAffaire.SansSuite) {
                        stepContext.IsReadonlyTarget = true;
                        stepContext.DenyReason = "Le contrat est sans suite, vous ne pouvez pas le modifier";
                    }

                    if (stepContext.IsReadonlyTarget) {
                        stepContext.IsAborted = stepContext.Target == ContextStepName.EditionRegularisation;
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
