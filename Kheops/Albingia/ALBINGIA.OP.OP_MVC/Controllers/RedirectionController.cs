using Albingia.Common;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Mvc.Common;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.CustomResult;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using OP.WSAS400.DTO;
using OPServiceContract;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class RedirectionController : BaseController {
        public const string HomeUrl = "/RechercheSaisie/Index";

        [HttpPost]
        [ErrorHandler]
        public JsonResult Auto(StepContext stepContext)
        {
            string url = null;
            if (stepContext.Target == ContextStepName.Deverrouiller && stepContext.Origin.IsEmptyOrNull()) {
                FolderController.DeverrouillerAffaire(stepContext.TabGuid);
                url = HomeUrl;
            }
            else {
                var initial = stepContext.Clone();
                if (stepContext.Target != ContextStepName.Attestation) {
                    using (var service = ServiceClientFactory.GetClient<IStepFinder>()) {
                        stepContext = service.Channel.Find(stepContext);
                    }
                }

                url = BuildUrl(initial, stepContext);
            }
            return JsonNetResult.NewResultToGet(new { url, context = stepContext });
        }

        [HttpGet]
        [ErrorHandler]
        public ActionResult AutoUnlock(string id) {
            FolderController.DeverrouillerAffaire(id);
            return Redirect(HomeUrl);
        }

        private string BuildUrl(StepContext contextBefore, StepContext stepContext)
        {
            if (!stepContext.IsAborted)
            {
                string tabGuid = GetSurroundedTabGuid(stepContext.TabGuid);
                string mode = PageParamContext.ModeNavigKey + stepContext.ModeNavig + PageParamContext.ModeNavigKey;
                string consultMode = stepContext.IsReadonlyTarget ? AlbParameters.ConsultOnlyKey : string.Empty;
                string controller = stepContext.Folder.Type == AlbConstantesMetiers.TYPE_OFFRE ?
                    "ModifierOffre"
                    : stepContext.IsModifHorsAvenant ?
                        "AnCreationContrat"
                        : (stepContext.Folder.NumeroAvenant == 0 ? "AnInformationsGenerales" : "AvenantInfoGenerales");
                string addParam = null;
                if (stepContext.AvnParams?.Any(p => p.Value != null) ?? false)
                {
                    addParam = PageParamContext.ParamKey + "AVN|||" + string.Join("||", stepContext.AvnParams.Select(x => $"{x.Key}|{x.Value}")) + PageParamContext.ParamKey;
                }
                string codeAffaire = stepContext.Target == ContextStepName.RetourPieces
                    ? (stepContext.Folder.FullIdentifier + "_")
                    : (stepContext.Folder.Identifier
                        + (stepContext.Target == ContextStepName.BlocageDeblocageTermes ? ("_" + stepContext.NiveauDroitTermes) : string.Empty));
                string id = HttpUtility.UrlEncode(codeAffaire + tabGuid + addParam + mode + consultMode);
                InitSessionAccesRequest(contextBefore, stepContext);
                switch (stepContext.Target) {
                    case ContextStepName.Validation:
                        if (stepContext.Folder.Type == "O")
                        {
                            return $"/ValidationOffre/Index?id={id}";
                        }
                        else { return null; }
                    case ContextStepName.NouvelleAffaire:
                        return $"/CreationAffaireNouvelle/ChoixRisques?id={HttpUtility.UrlEncode(AlbParameters.BuildPipedParams(stepContext.PipedParams))}";
                    case ContextStepName.EtablirAffaireNouvelle:
                        return $"/CreationAffaireNouvelle/Index?id={id}";
                    case ContextStepName.DoubleSaisie:
                    case ContextStepName.CreationAvenant:
                    case ContextStepName.ControleFin:
                        return $"/{stepContext.Target}/Index?id={id}";
                    case ContextStepName.EditionResiliation:
                    case ContextStepName.EditionRemiseEnVigueur:
                        return $"/{ContextStepName.CreationAvenant}/Index?id={id}";
                    case ContextStepName.ConfirmerOffre:
                        return $"/ConfirmationSaisie/Offre?id={id}";
                    case ContextStepName.Edition:
                        return $"/{controller}/Index?id={id}";
                    case ContextStepName.EditionAvenant:
                        return $"/AvenantInfoGenerales/Index?id={id}";
                    case ContextStepName.EngagementPeriodes:
                        return $"/EngagementPeriodes/Index/{id}accessModerechercheaccessMode";
                    case ContextStepName.CreationRegularisation:
                    case ContextStepName.EditionRegularisation:
                    //case ContextStepName.EditionBNS:
                    case ContextStepName.EditionRegularisationEtAvenant:
                    case ContextStepName.ConsulterRegule:
                        return $"/{ContextStepName.CreationRegularisation}/Step1_ChoixPeriode?id={id}";
                    case ContextStepName.EditionPB:
                    case ContextStepName.ConsulterPB:
                        return $"/{ContextStepName.CreationPB}/Step1_ChoixPeriode?id={id}";
                    case ContextStepName.EditionBNS:
                        return $"/{ContextStepName.CreationBNS}/Index?id={id}";
                    case ContextStepName.RetourPieces:
                        return $"/Retours/OpenRetours/{id}";
                    case ContextStepName.Attestation:
                        id = HttpUtility.UrlEncode(codeAffaire + tabGuid + mode);
                        return $"/CreationAttestation/Index?id={id}";
                    case ContextStepName.PrisePosition:
                        return $"/{stepContext.Target}/Index?id={id}";
                    case ContextStepName.ClassementSansSuite:
                        return $"/{stepContext.Target}/Index?id={id}";
                    case ContextStepName.BlocageDeblocageTermes:
                        return $"/BlocageTermes/Index?id={id}";
                }
            }
            return null;
        }

        private void InitSessionAccesRequest(StepContext contextBefore, StepContext stepContext) {
            if (!contextBefore.IsReadonlyTarget && stepContext.IsReadonlyTarget) {
                // return because the edit action was cancelled
                return;
            }
            AccesAffaire acces = new AccesAffaire(AccesOrigine.Consulter) {
                Code = stepContext.Folder.CodeOffre,
                Version = stepContext.Folder.Version,
                Avenant = stepContext.IsModeHisto ? stepContext.Folder.NumeroAvenant : default(int?),
                TabGuid = Guid.TryParse(stepContext.TabGuid, out var g) ? g : default
            };
            if (!stepContext.IsReadonlyTarget) {
                if (stepContext.Target == ContextStepName.NouvelleAffaire) {
                    // add two session locks, one for Offre and the other for Contrat
                    acces.ModeAcces = AccesOrigine.Modifier;
                    acces.Type = Albingia.Kheops.OP.Domain.Affaire.AffaireType.Contrat.AsCode();
                    MvcApplication.ListeAccesAffaires.Add(acces);
                    acces = new AccesAffaire(AccesOrigine.EtablirAffaireNouvelle) {
                        Code = stepContext.PipedParams[PipedParameter.IPB].First(),
                        Version = int.Parse(stepContext.PipedParams[PipedParameter.ALX].First()),
                        Type = Albingia.Kheops.OP.Domain.Affaire.AffaireType.Offre.AsCode(),
                        Avenant = null,
                        TabGuid = g
                    };
                    MvcApplication.ListeAccesAffaires.Add(acces);
                    return;
                }
                switch (stepContext.Target) {
                    case ContextStepName.NouvelleAffaire:
                        acces.ModeAcces = AccesOrigine.EtablirAffaireNouvelle;
                        break;
                    case ContextStepName.CreationAvenant:
                    case ContextStepName.CreationRegularisation:
                        acces.ModeAcces = AccesOrigine.CreationAvenant;
                        break;
                    case ContextStepName.EditionResiliation:
                        acces.ModeAcces = AccesOrigine.Resiliation;
                        break;
                    case ContextStepName.ConfirmerOffre:
                    case ContextStepName.Edition:
                    case ContextStepName.DoubleSaisie:
                    case ContextStepName.ControleFin:
                    case ContextStepName.Validation:
                        acces.ModeAcces = stepContext.IsModifHorsAvenant ? AccesOrigine.ModifierHorsAvenant : AccesOrigine.Modifier;
                        break;
                    case ContextStepName.EditionAvenant:
                        acces.ModeAcces = AccesOrigine.Avenant;
                        break;
                    case ContextStepName.EditionRegularisation:
                    case ContextStepName.EditionBNS:
                    case ContextStepName.EditionPB:
                        acces.ModeAcces = AccesOrigine.Regularisation;
                        break;
                    case ContextStepName.EditionRegularisationEtAvenant:
                        acces.ModeAcces = AccesOrigine.RegularisationEtAvenant;
                        break;
                    case ContextStepName.EditionRemiseEnVigueur:
                        acces.ModeAcces = AccesOrigine.RemiseEnVigueur;
                        break;
                    case ContextStepName.EngagementPeriodes:
                        acces.ModeAcces = AccesOrigine.Engagements;
                        break;
                    case ContextStepName.EtablirAffaireNouvelle:
                        acces.ModeAcces = AccesOrigine.EtablirAffaireNouvelle;
                        break;
                    case ContextStepName.RetourPieces:
                        acces.ModeAcces = AccesOrigine.RetourPiece;
                        break;
                    case ContextStepName.Attestation:
                        acces.ModeAcces = AccesOrigine.Attestation;
                        // no lock needed for this process
                        acces.Valider();
                        break;
                    case ContextStepName.PrisePosition:
                        acces.ModeAcces = AccesOrigine.PrisePosition;
                        break;
                    case ContextStepName.BlocageDeblocageTermes:
                        acces.ModeAcces = AccesOrigine.BlocageDeblocageTermes;
                        break;
                }
            }
            MvcApplication.ListeAccesAffaires.Add(acces);
        }
    }
}
