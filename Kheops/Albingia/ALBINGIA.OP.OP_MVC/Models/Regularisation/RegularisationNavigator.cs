using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Controllers.Regularisation;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesNavigationArbre;
using ALBINGIA.OP.OP_MVC.Models.Regularisation;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ALBINGIA.OP.OP_MVC.Models
{
    public class RegularisationNavigator
    {
        internal readonly static string SaisieRegul = "Saisie Régul";
        internal readonly static string SaisieBNS = "Saisie BNS";
        internal readonly static string SingleRisquePrefix = "Risque ";
        internal readonly static string SingleGarantiePrefix = "Garantie ";
        internal readonly static string SingleRisqueRegulPrefix = "Saisie Risque ";
        internal readonly static string DefaultLinkUrl = "/Regularisation/RichStep";

        internal static void Initialize(ModeleNavigationArbre navTree, RegularisationContext context)
        {
            if (navTree != null && context != null && context.Step != RegularisationStep.Invalid)
            {
                if (context.Step == RegularisationStep.ChoixMode)
                {
                    navTree.RegularisationStepList.Clear();
                }
                else
                {
                    ResetSteps(navTree);
                    context = InitContext(context);
                    SetStepsState(navTree, context);
                }

                navTree.LastValidNumAvt = context.LastValidNumAvt;
                navTree.LastValidRgId = context.LastValidRgId;
                navTree.LastNumAvt = context.LastNumAvt;

                navTree.IsMonoGarantie = context.NbGaranties == 1;
                navTree.IsMonoRisque = context.NbRisques == 1;
                navTree.IsReguleValidated = context.ValidationDone;
            }
        }

        /// <summary>
        /// Allows to create a RegularisationContext class if necessary in Standard mode
        /// </summary>
        internal static void StandardInitContext(IRegulModel model, RegularisationStep step, long rsqId = 0, long garId = 0)
        {
            if (model != null && model.Context == null)
            {
                string mod = null;
                string niv = null;
                string avn = null;
                string typ = null;
                string avnmod = null;
                bool isHisto = false;
                if (model is MetaModelsBase baseModel)
                {
                    mod = baseModel.AllParameters.ModeRegularisation;
                    niv = baseModel.AllParameters.NiveauRegularisation;
                    typ = baseModel.AllParameters.TypeRegularisation;
                    avn = baseModel.AllParameters.IsHistoRegularisation ? "O" : "N";
                    avnmod = baseModel.AllParameters.ModeConsultationAvenant;
                    isHisto = baseModel.ModeNavig == "H";
                }
                else
                {
                    mod = InformationGeneraleTransverse.GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);
                    niv = InformationGeneraleTransverse.GetAddParamValue(model.AddParamValue, AlbParameterName.REGULNIV);
                    avn = InformationGeneraleTransverse.GetAddParamValue(model.AddParamValue, AlbParameterName.REGULAVN);
                    typ = InformationGeneraleTransverse.GetAddParamValue(model.AddParamValue, AlbParameterName.REGULTYP);
                    avnmod = InformationGeneraleTransverse.GetAddParamValue(model.AddParamValue, AlbParameterName.AVNMODE);
                }

                List<RegulMatriceDto> matrice = default;
                IdContratDto contrat = model.IdContrat;
                List<RegularisationStateDto> states = default;
                Task.WhenAll(new[] {
                    Task.Run( () => {
                        using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                        {
                            matrice = client.Channel.GetRegulMatrice(contrat.CodeOffre, contrat.Version, contrat.Type, model.LotId.ToString(), model.RgId);
                        }
                    }),
                    Task.Run( () => {
                        using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
                        {
                            states = client.Channel.GetCurrentStates(model.RgId).ToList();
                        }
                    }),
                    Task.Run( () => {
                        if (model.IdContrat.TypeContrat.IsEmptyOrNull())
                        {
                            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
                            {
                                contrat = client.Channel.GetContratInfo(contrat.CodeOffre, contrat.Version.ToString(), contrat.Type);
                            }
                        }
                    })
                }).Wait();
                if (contrat != null) {
                    contrat.IsHisto = isHisto;
                }
                model.Context = InitContext(new RegularisationContext {
                    RgId = model.RgId,
                    IsReadOnlyMode = avnmod == "CONSULT",
                    LotId = model.LotId,
                    IdContrat = contrat,
                    Mode = RegularisationController.ParseMode(mod),
                    Scope = niv.ParseCode<RegularisationScope>(),
                    Type = typ,
                    RgHisto = Char.Parse(avn),
                    Step = step,
                    States = states,
                    Matrix = matrice,
                    RsqId = rsqId,
                    GrId = garId,
                    IsMultiRC = matrice.All(x=> AlbOpConstants.AllRCGar.Contains(x.GarLabel))
                        && matrice.Select(x => x.RisqueId).Distinct().Count() == 1
                        && matrice.Select(x => x.GarLabel).Distinct().Count() >= 2
                        && matrice.Count(x => x.GarLabel == AlbOpConstants.RCFrance) == 1
                        && matrice.Count(x => x.GarLabel == AlbOpConstants.RCExport) <= 1
                        && matrice.Count(x => x.GarLabel == AlbOpConstants.RCUSA) <= 1,
                    HasMultiRC = matrice.GroupBy(x => x.Formule).Any(gp => gp.Count(x => AlbOpConstants.AllRCGar.Contains(x.GarLabel)) > 1)
                });
            }
            else
            {
                if (model.Context.Step != step)
                {
                    model.Context.Step = step;
                }
            }
        }

        private static RegularisationContext InitContext(RegularisationContext context)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                context = client.Channel.EnsureContext(context);
            }

            return context;
        }

        private static void ResetSteps(ModeleNavigationArbre navTree)
        {
            navTree.RegularisationStepList.Clear();
            navTree.RegularisationStepList.Add(new RegularisationStateModel {
                Label = "Contrat",
                Step = RegularisationStep.Contrat,
                TreeLevel = 1
            });

            navTree.RegularisationStepList.Add(new RegularisationStateModel {
                Label = "Régularisation",
                Step = RegularisationStep.ChoixPeriodeCourtier,
                TreeLevel = 1
            });

            navTree.RegularisationStepList.Add(new RegularisationStateModel {
                Label = "Choix Risques",
                Step = RegularisationStep.ChoixRisques,
                TreeLevel = 1
            });

            navTree.RegularisationStepList.Add(new RegularisationStateModel {
                Label = "Garanties",
                Step = RegularisationStep.ChoixGaranties,
                TreeLevel = 2
            });

            navTree.RegularisationStepList.Add(new RegularisationStateModel {
                Label = "Périodes",
                Step = RegularisationStep.ChoixPeriodesGarantie,
                TreeLevel = 3
            });

            navTree.RegularisationStepList.Add(new RegularisationStateModel {
                Label = navTree.ScreenType == "BNS" ? SaisieBNS : SaisieRegul,
                Step = RegularisationStep.Regularisation,
                TreeLevel = 4
            });

            navTree.RegularisationStepList.Add(new RegularisationStateModel {
                Label = "Cotisations",
                Step = RegularisationStep.Cotisations,
                TreeLevel = 1
            });
        }

        private static void SetCurrentStep(ModeleNavigationArbre navTree, RegularisationContext context)
        {
            var currentStep = navTree.RegularisationStepList.FirstOrDefault(s => s.Step == context.Step);
            if (currentStep != null)
            {
                currentStep.IsCurrent = true;
                currentStep.IsVisible = true;
                currentStep.IsHighlighted = true;
                currentStep.IsLink = false;
                currentStep.Link = string.Empty;
            }
        }

        private static void SetStepsState(ModeleNavigationArbre navTree, RegularisationContext context)
        {
            SetCurrentStep(navTree, context);
            if (context.Step == RegularisationStep.ChoixPeriodeCourtier && context.AccessMode == AccessMode.CREATE)
            {
                navTree.RegularisationStepList.RemoveAll(state => state.Step != RegularisationStep.ChoixPeriodeCourtier);
            }
            else
            {
                RemoveInvalidSteps(navTree, context);
                FormatLabels(navTree, context);
                ActivateLinks(navTree, context);
                SetLinkLevels(navTree, context);
            }
        }

        private static void RemoveInvalidSteps(ModeleNavigationArbre navTree, RegularisationContext context)
        {
            if (navTree.IsValidation)
            {
                navTree.RegularisationStepList.RemoveAll(state => state.Step != RegularisationStep.Cotisations);
            }

            if (context.Mode == RegularisationMode.Standard)
            {
                RemoveInvalidStepsStandard(navTree, context);
            }
            else
            {
                RemoveInvalidStepsNonStandard(navTree, context);
            }
        }

        private static void RemoveInvalidStepsStandard(ModeleNavigationArbre navTree, RegularisationContext context)
        {
            if (context.Step != RegularisationStep.Regularisation)
            {
                navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.Regularisation);
            }

            if (!context.ValidationDone)
            {
                navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.Cotisations);
            }

            switch (context.Step)
            {
                case RegularisationStep.ChoixGaranties:
                    navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.ChoixPeriodesGarantie);
                    break;
                case RegularisationStep.ChoixRisques:
                    navTree.RegularisationStepList.RemoveAll(state =>
                        state.Step == RegularisationStep.ChoixPeriodesGarantie
                        || state.Step == RegularisationStep.ChoixGaranties);
                    break;
                case RegularisationStep.ChoixPeriodeCourtier:
                case RegularisationStep.Cotisations:
                    if (context.NbRisques == 1)
                    {
                        if (context.NbGaranties > 1)
                        {
                            navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.ChoixPeriodesGarantie);
                        }
                    }
                    else
                    {
                        navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.ChoixGaranties || state.Step == RegularisationStep.ChoixPeriodesGarantie);
                    }
                    break;
                case RegularisationStep.ChoixPeriodesGarantie:
                case RegularisationStep.Regularisation:
                    break;
            }
        }

        private static void RemoveInvalidStepsNonStandard(ModeleNavigationArbre navTree, RegularisationContext context)
        {
            navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.ChoixPeriodesGarantie || state.Step == RegularisationStep.ChoixGaranties);

            if (!context.ValidationDone)
            {
                navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.Cotisations);
            }
            if (context.Mode == RegularisationMode.BNS)
            {
                navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.ChoixPeriodeCourtier);
            }

            switch (context.Step)
            {
                case RegularisationStep.ChoixRisques:
                    navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.Regularisation);
                    break;
                case RegularisationStep.Regularisation:
                    if (context.NbRisques == 1 || context.Scope == RegularisationScope.Contrat)
                    {
                        navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.ChoixRisques);
                    }
                    break;
                case RegularisationStep.Cotisations:
                    if (context.NbRisques > 1)
                    {
                        navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.Regularisation);
                    }
                    else
                    {
                        navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.ChoixRisques);
                    }
                    break;
                case RegularisationStep.ChoixPeriodeCourtier:
                    if (!(context.ValidationDone && (context.NbRisques == 1 || context.Scope == RegularisationScope.Contrat)))
                    {
                        navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.Regularisation);
                    }

                    if (context.NbRisques == 1 || context.Scope == RegularisationScope.Contrat)
                    {
                        navTree.RegularisationStepList.RemoveAll(state => state.Step == RegularisationStep.ChoixRisques);
                    }
                    break;
                default:
                    break;
            }
        }

        private static void FormatLabels(ModeleNavigationArbre navTree, RegularisationContext context)
        {
            RegularisationStateModel state;
            if (context.Mode == RegularisationMode.Standard)
            {
                if (context.NbRisques == 1)
                {
                    state = navTree.RegularisationStepList.FirstOrDefault(s => s.Step == RegularisationStep.ChoixRisques);
                    if (state != null)
                    {
                        state.Label = SingleRisquePrefix + context.RsqId.ToString().PadLeft(2, '0');
                    }

                    if (context.NbGaranties == 1)
                    {
                        state = navTree.RegularisationStepList.FirstOrDefault(s => s.Step == RegularisationStep.ChoixGaranties);
                        if (state != null)
                        {
                            state.Label = SingleGarantiePrefix + context.GrLabel;
                        }
                    }
                }
                else if (context.RsqId > 0 && context.Matrix.Where(m => m.RisqueId == context.RsqId).Select(m => m.GarId).Distinct().Count() == 1)
                {
                    state = navTree.RegularisationStepList.FirstOrDefault(s => s.Step == RegularisationStep.ChoixGaranties);
                    if (state != null)
                    {
                        state.Label = SingleGarantiePrefix + context.GrLabel;
                    }
                }
            }
            else
            {
                if (context.RsqId > 0 && context.NbRisques > 1)
                {
                    state = navTree.RegularisationStepList.FirstOrDefault(s => s.Step == RegularisationStep.Regularisation);
                    if (state != null)
                    {
                        state.Label = SingleRisqueRegulPrefix + context.RsqId.ToString().PadLeft(2, '0');
                    }
                }
            }
        }

        private static string GetStandardStep(RegularisationStep step)
        {
            switch (step)
            {
                case RegularisationStep.ChoixPeriodeCourtier:
                    return RegulSteps.Step1_ChoixPeriode.ToString();
                case RegularisationStep.ChoixGaranties:
                    return RegulSteps.Step3_ChoixGarantie.ToString();
                case RegularisationStep.ChoixPeriodesGarantie:
                    return RegulSteps.Step4_ChoixPeriodeGarantie.ToString();
                case RegularisationStep.ChoixRisques:
                    return RegulSteps.Step2_ChoixRisque.ToString();
                case RegularisationStep.Regularisation:
                    return RegulSteps.Step5_RegulContrat.ToString();
                default:
                    break;
            }

            return string.Empty;
        }

        private static void ActivateLinks(ModeleNavigationArbre navTree, RegularisationContext context)
        {
            navTree.RegularisationStepList.ForEach(state => {
                if (state.Step != RegularisationStep.Cotisations)
                {
                    ActivateLink(state, context);
                }
            });

            ActivateLinkCotisation(navTree, context);
        }

        private static void ActivateLink(RegularisationStateModel state, RegularisationContext context)
        {
            if (state.Label.StartsWith(SingleGarantiePrefix)
                || state.Label.StartsWith(SingleRisquePrefix)
                || state.Label.StartsWith(SingleRisqueRegulPrefix)
                || state.Step == context.Step)
            {
                state.IsLink = false;
                state.Link = string.Empty;
            }
            else
            {
                state.IsLink = true;
                state.Link = GetLinkUrl(context, state.Step);
            }
        }

        private static void ActivateLinkCotisation(ModeleNavigationArbre navTree, RegularisationContext context)
        {
            RegularisationStateModel state = navTree.RegularisationStepList.FirstOrDefault(s => s.Step == RegularisationStep.Cotisations);
            if (state == null)
            {
                return;
            }

            if (context.ValidationDone
                || context.Step == state.Step
                || navTree.Etape != "Regule" && navTree.Cotisation && navTree.ModeNavig == ModeConsultation.Standard.AsCode())
            {
                state.IsLink = true;
                state.Link = GetLinkUrl(context, state.Step);
                state.IsLinkVisited = navTree.TagCotisation == "O";
                state.IsHighlighted = navTree.Etape == "Cotisation";
            }
            else
            {
                state.IsLink = false;
            }
        }

        private static string GetLinkUrl(RegularisationContext context, RegularisationStep step)
        {
            if (step == RegularisationStep.Cotisations)
            {
                return "Quittance/Index/"
                   + context.IdContrat.CodeOffre + "_" + context.IdContrat.Version.ToString(CultureInfo.CurrentCulture) + "_" + context.IdContrat.Type;
            }

            if (step == RegularisationStep.Contrat)
            {
                var model = new ModeleRegularisationPage();
                String url = String.Empty;
                String codeAvn = "0";
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = client.Channel;
                    var listRegul = serviceContext.GetListeRegularisation(context.IdContrat.CodeOffre, context.IdContrat.Version.ToString(), context.IdContrat.Type);
                    var lastRegul = listRegul.SingleOrDefault<LigneRegularisationDto>(x=>x.NumRegule == context.RgId);
                    if (lastRegul != null)
                    {
                        codeAvn = lastRegul.CodeAvn.ToString();
                        if (codeAvn == "0")
                        {
                            url = "AnInformationsGenerales/Index";
                        }
                        else
                        {
                            url = "AvenantInfoGenerales/Index";
                        }
                    }
                    else
                    {
                        // Afficher le contrat
                        url = "AnInformationsGenerales/Index";
                    }
                    context.LastValidNumAvt = codeAvn;
                    context.LastNumAvt = codeAvn; //(lastRegul != null) ? lastRegul.CodeAvn.ToString() : "0";
                    if (lastRegul != null && lastRegul.NumRegule != 0)
                    {
                        context.LastValidRgId = lastRegul.NumRegule;
                    }
                    else
                    {
                        context.LastValidRgId = 0;
                    }

                    url = url + "/" + context.IdContrat.CodeOffre + "_" + context.IdContrat.Version.ToString(CultureInfo.CurrentCulture) + "_" + context.IdContrat.Type;
                    return url;

                }
            }

            if (context.Mode == RegularisationMode.Standard)
            {
                return "CreationRegularisation/"
                    + GetStandardStep(step) + "/"
                    + context.IdContrat.CodeOffre + "_" + context.IdContrat.Version.ToString(CultureInfo.CurrentCulture) + "_" + context.IdContrat.Type;
            }


            return DefaultLinkUrl;
        }

        private static void SetLinkLevels(ModeleNavigationArbre navTree, RegularisationContext context)
        {
            if (context.Mode != RegularisationMode.Standard)
            {
                foreach (var item in navTree.RegularisationStepList)
                {
                    if (item.Label == SaisieRegul)
                    {
                        item.TreeLevel = 1;
                    }
                    else if (item.Label.StartsWith(SingleRisqueRegulPrefix))
                    {
                        item.TreeLevel = 2;
                    }
                }
            }
        }
    }
}