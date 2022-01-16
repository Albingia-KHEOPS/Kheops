using Albingia.Common;
using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.CustomResult;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Ajax;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.FormuleGarantie;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesFormGarantie;
using ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie;
using ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using Mapster;
using Newtonsoft.Json;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.Offres.Risque;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Consts = ALBINGIA.Framework.Common.Constants.AlbConstantesMetiers;
using Domain = Albingia.Kheops.OP.Domain;
using Services = Albingia.Kheops.OP.Application.Port.Driver;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class FormuleGarantieController : RisqueController<ModeleFormuleGarantiePage>
    {
        public override bool IsReadonly
        {
            get
            {
                bool isReadonly = base.IsReadonly;
                if (this.model.Contrat is null || this.model.Contrat.NumAvenant == 0 || isReadonly)
                {
                    return isReadonly;
                }
                if (this.model.OptionsFormule is null
                    || this.model.OptionsFormule.Options?.Count != 1
                    || this.model.OptionsFormule.Applications is null)
                {
                    return false;
                }
                /* a verifier
                 * var option = this.model.OptionsFormule.Options.First();
                DateTime d = this.model.OptionsFormule.DateEffetAvenantContrat ?? default;
                if (option.DateAvenantModif.HasValue && option.DateAvenantModif >= this.model.OptionsFormule.DateEffetAvenantContrat) {
                    d = option.DateAvenantModif.Value;
                }*/
                return this.model.OptionsFormule.Applications.HasAllObjetsSortis(this.model.OptionsFormule.DateEffetAvenantContrat.Value);
            }
        }

        [ErrorHandler]
        public ActionResult Index(string id, bool isRefresh = false)
        {
            model.ModeDuplicate = id.Contains("modeDuplication") && id.Split(new[] { "modeDuplication" }, StringSplitOptions.None)[1] == "1";
            id = id.Contains("modeDuplication") ? id.Split(new[] { "modeDuplication" }, StringSplitOptions.None)[0] : id;
#if DEBUG
            System.Diagnostics.Trace.WriteLine($"id: {id}");
#endif
            var affaire = new Affaire(id);
            if (affaire.NumeroAvenant > 0)
            {
                id = InitAvenantFormule(id, affaire);
#if DEBUG
                System.Diagnostics.Trace.WriteLine($"id after init avn: {id}");
#endif
            }
            id = InitializeParams(id);
            LoadInfoPage(id, affaire);
            var formule = LoadFormule(affaire, int.Parse(model.CodeFormule), !isRefresh);
            LoadInfoApplications(affaire, formule);
            if (model.Contrat != null)
            {
                LoadDataAvenant();
            }

            bool reload = false;
            if (reload)
            {
                SetOptionsFormule(affaire, new Formule(), "");
                return RedirectToAction("Index", "FormuleGarantie",
                    new
                    {
                        id = $"{affaire.CodeOffre}_{affaire.Version}_{affaire.Type}_2_1_0tabGuid{model.TabGuid}tabGuidmodeNavig{model.ModeNavig}modeNavig",
                        isRefresh = true
                    });
            }
            return View(model);
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult ChangeApplications(Affaire affaire, PageParamContext pageContext, int numFormule, int numRisque, IEnumerable<int> numsObjets, DateTime? dateAvenant, int? numOption = null)
        {
            var affid = affaire.Adapt<Domain.Affaire.AffaireId>();
            Albingia.Kheops.DTO.FormuleDto formula = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                if (numFormule > 0)
                {
                    (numFormule, numOption) = client.Channel.SetApplication(affid, numFormule, numRisque, numsObjets, dateAvenant, numOption);
                    formula = client.Channel.GetFormuleAffaire(affid, numFormule);
                }
                else
                {
                    formula = client.Channel.InitFormuleAffaire(affaire.Adapt<Domain.Affaire.AffaireId>(), numRisque, numsObjets, dateAvenant);
                }
            }

            return JsonNetResult.NewResultToGet(new
            {
                id = affaire.Identifier,
                formule = formula.FormuleNumber,
                option = numOption.GetValueOrDefault(1),
                fullId = pageContext.BuildFullString($"{affaire.Identifier}_{formula.FormuleNumber}_{numOption.StringValue("1", true)}_0")
            });
        }

        [HttpPost]
        [HandleJsonError]
        public void SetOptionsFormule(Affaire affaire, Formule formule, string libelle)
        {
            var dto = formule.Adapt<Albingia.Kheops.DTO.FormuleDto>();
            dto.AffaireId = affaire.Adapt<Domain.Affaire.AffaireId>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.SetSelection(dto);
            }
        }

        public void AddOrRemoveVolet(Affaire affaire, int numFormule, int numOption, Volet volet, DateTime dateAvenant)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.SetSelectionVolet(
                    affaire.Adapt<Domain.Affaire.AffaireId>(),
                    volet.Adapt<Albingia.Kheops.DTO.OptionsDetailVoletDto>(),
                    numFormule,
                    numOption,
                    dateAvenant);
            }
        }

        [HttpPost]
        [HandleJsonError]
        public void AddOrRemoveBloc(Affaire affaire, int numFormule, int numOption, Bloc bloc, DateTime dateAvenant)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.SetSelectionBloc(
                    affaire.Adapt<Domain.Affaire.AffaireId>(),
                    bloc.Adapt<Albingia.Kheops.DTO.OptionsDetailBlocDto>(),
                    numFormule,
                    numOption,
                    dateAvenant);
            }
        }

        [HttpPost]
        [HandleJsonError]
        public void AddOrRemoveGarantie(IdentifiantGarantie garantieId, Models.FormuleGarantie.Garantie garantie, DateTime dateAvenant)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.SetSelectionGarantie(
                    garantieId.Affaire.Adapt<Domain.Affaire.AffaireId>(),
                    garantie.Adapt<Albingia.Kheops.DTO.GarantieDto>(),
                    garantieId.NumFormule,
                    garantieId.NumOption,
                    dateAvenant);
            }
        }

        [HttpPost]
        [HandleJsonError]
        public void ValidateOptionsFormule(Affaire affaire, Formule formule, string libelle, int numOption)
        {
            var dto = formule.Adapt<Albingia.Kheops.DTO.FormuleDto>();
            dto.AffaireId = affaire.Adapt<Domain.Affaire.AffaireId>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.SetSelection(dto);
                client.Channel.ValidateFormuleAffaire(new Domain.Formule.FormuleId
                {
                    CodeAffaire = affaire.CodeOffre,
                    IsHisto = false,
                    NumeroAliment = affaire.Version,
                    NumeroAvenant = affaire.NumeroAvenant,
                    NumeroFormule = formule.Numero,
                    TypeAffaire = affaire.Type.ParseCode<Domain.Affaire.AffaireType>()
                });
            }
        }

        [HttpPost]
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult CancelChanges(Affaire affaire, PageParamContext pageContext)
        {
            if (!pageContext.IsReadonly)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
                {
                    client.Channel.CancelFormuleAffaireChanges(affaire.Adapt<Domain.Affaire.AffaireId>());
                }
            }
            return RedirectToAction("Index", "MatriceFormule", new { id = pageContext.BuildFullString(affaire.CodeOffre + "_" + affaire.Version + "_" + affaire.Type) });
        }

        [HttpPost]
        [ErrorHandler]
        public void ValidatePortees(IdentifiantGarantie garantieId, PorteesGarantie portees)
        {
            var listePortees = portees.ObjetsRisque.Adapt<ICollection<Albingia.Kheops.DTO.PorteeGarantieDto>>();
            foreach (var p in listePortees)
            {
                p.CodeAction = portees.CodeAction;
                p.GarantieId = garantieId.Sequence;
                p.RisqueId = int.Parse(portees.CodeRisque);
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.AddOrUpdatePortees(
                    garantieId.Affaire.Adapt<Domain.Affaire.AffaireId>(),
                    garantieId.NumOption,
                    garantieId.NumFormule,
                    garantieId.CodeBloc,
                    garantieId.Sequence,
                    listePortees,
                    portees.ReportCalcul);
            }
        }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetEmptyDetails()
        {
            return JsonNetResult.NewResultToGet(new DetailsGarantie());
        }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetEmptyPortee()
        {
            return JsonNetResult.NewResultToGet(new PorteesGarantie());
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetDetailsGarantie(IdentifiantGarantie garantieId, Domain.Referentiel.CibleFiltre filtre, DateTime? dateAvenant = null)
        {
            if (garantieId.Sequence > 0)
            {
                Albingia.Kheops.DTO.GarantieDetailsDto garantie;
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
                {
                    garantie = client.Channel.GetGarantieDetails(
                    new Domain.Affaire.AffaireId
                    {
                        CodeAffaire = garantieId.Affaire.CodeOffre,
                        IsHisto = garantieId.IsHisto,
                        NumeroAvenant = garantieId.Affaire.NumeroAvenant,
                        NumeroAliment = garantieId.Affaire.Version,
                        TypeAffaire = garantieId.Affaire.Type.ParseCode<Domain.Affaire.AffaireType>()
                    },
                    1,
                    garantieId.NumFormule,
                    garantieId.CodeBloc,
                    garantieId.Sequence,
                    garantieId.IsReadonly,
                    dateAvenant);
                }

                var model = garantie.Adapt<DetailsGarantie>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IReferentielPort>())
                {
                    model.TypesEmissions = client.Channel.GetTypeEmissions().Select(t => LabeledValue.Create(t.Code, t.Libelle));
                    model.CodesTaxes = client.Channel.GetTaxes().Select(t => LabeledValue.Create(t.Code, t.Libelle));
                    model.AlimentationsAssiettes = client.Channel.GetAlimentations().Select(a => LabeledValue.Create(a.Code, a.Libelle));
                    model.DureeUnites = client.Channel.GetUnitesDuree().Select(a => LabeledValue.Create(a.Code, a.Libelle));
                    model.Applications = client.Channel.GetPeriodesApplications().Select(a => LabeledValue.Create(a.Code, a.Libelle));
                }

                model.DetailsValeurs = GetDetailsValeursGarantie(garantieId).Adapt<DetailsValeursGarantie>();

                return JsonNetResult.NewResultToGet(model);
            }

            return JsonNetResult.NewResultToGet(new DetailsGarantie
            {
                Code = "code",
                Libelle = "libellé",
                CodeCaractere = "O"
            });
        }

        [HttpPost]
        [HandleJsonError]
        public void SaveDetailsGarantie(Affaire affaire, int numOption, int numFormule, DetailsGarantie details)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.SetGarantieDetails(
                    new Domain.Formule.OptionId
                    {
                        CodeAffaire = affaire.CodeOffre,
                        IsHisto = false,
                        NumeroAliment = affaire.Version,
                        NumeroAvenant = affaire.NumeroAvenant,
                        NumeroFormule = numFormule,
                        NumeroOption = numOption,
                        TypeAffaire = affaire.Type.ParseCode<Domain.Affaire.AffaireType>()
                    },
                    details.Adapt<Albingia.Kheops.DTO.GarantieDetailsDto>());
            }
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult StartAvenantFormule(Affaire affaire, int numOption, int numFormule, DateTime dateEffet)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.StartNewAvenant(affaire.Adapt<Domain.Affaire.AffaireId>(), numOption, numFormule, dateEffet);
            }
            return GetFormule(affaire, numFormule, numOption);
        }

        [HttpPost]
        [HandleJsonError]
        public JsonResult GetFormule(Affaire affaire, int numFormule, int numOption)
        {
            this.model.CodeFormule = numFormule.ToString();
            this.model.CodeOption = (numOption < 1 ? 1 : numOption).ToString();
            return JsonNetResult.NewResultToGet(LoadFormule(affaire, numFormule));
        }

        [HttpPost]
        [HandleJsonError]
        public JsonResult CancelAvenantFormule(Affaire affaire, int numOption, int numFormule)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.CancelFormuleAffaireChanges(affaire.Adapt<Domain.Affaire.AffaireId>());
            }
            model.IsReadOnly = false;
            return GetFormule(affaire, numFormule, numOption);
        }

        [HttpPost]
        [HandleJsonError]
        public void UpdateDateAvenantFormule(Affaire affaire, int numOption, int numFormule, DateTime dateEffet)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.SetDateEffetOption(affaire.Adapt<Domain.Affaire.AffaireId>(), numOption, numFormule, dateEffet);
                var errors = client.Channel.CheckGarantiesDatesInFormule(affaire.Adapt<Domain.Affaire.AffaireId>(), numOption, numFormule);
                if (errors.Any())
                {
                    throw new BusinessValidationException(
                        errors,
                        string.Join(Environment.NewLine, errors.Select(e => $"{e.TargetType} {e.TargetCode} : {e.Error} ")));
                }
            }
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetSingleGarantie(IdentifiantGarantie garantieId)
        {
            Formule f;
            Models.FormuleGarantie.Garantie g;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                g = client.Channel.GetGarantie(
                garantieId.Affaire.Adapt<Domain.Affaire.AffaireId>(),
                garantieId.NumOption,
                garantieId.NumFormule,
                garantieId.CodeBloc,
                garantieId.Sequence,
                garantieId.IsReadonly).Adapt<Models.FormuleGarantie.Garantie>();
                f = client.Channel.GetFormuleAffaire(garantieId.Affaire.Adapt<Domain.Affaire.AffaireId>(), garantieId.NumFormule).Adapt<Formule>();
            }
            model.CodeFormule = garantieId.NumFormule.ToString();
            model.CodeOption = garantieId.NumOption.ToString();
            MapPorteesObjetsGarantie(f, g);
            return JsonNetResult.NewResultToGet(g);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string codeFormule, string codeOption, string codeRisque, string codeObjet, string branche,
            string tabGuid, string modeNavig, string libelle, string lettreLib, string saveCancel, string paramRedirect, string addParamType, string addParamValue,
            string readonlyDisplay, bool isModeConsultationEcran, bool isForceReadOnly, bool sessionLost, bool isModeConsult)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            var isOffreReadonly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn);
            if (paramRedirect.ContainsChars())
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            var ContentIS = "";
            string splitC = "#**#";
            var strparam = type + splitC + codeOffre + splitC + version + splitC + codeFormule + splitC + codeOption;

            using (var serviceContext = new DbInteraction())
            {
                ContentIS = serviceContext.LoadDbData(modeNavig, "0", codeRisque, codeFormule, codeOption, "Garantie", codeOffre, version, type, branche, "Garanties", "", "", splitC, strparam);
            }

            var folder = new Folder(new[] { codeOffre, version, type });
            switch (cible?.ToLower())
            {
                case "informationsspecifiquesgarantie":
                    bool hasIS = InformationsSpecifiquesGarantieController.HasIS(
                        new Domain.Affaire.AffaireId
                        {
                            CodeAffaire = codeOffre,
                            NumeroAliment = int.Parse(version),
                            TypeAffaire = type.ParseCode<Domain.Affaire.AffaireType>(),
                            NumeroAvenant = int.TryParse(numAvn, out int num) && num >= 0 ? num : default(int?)
                        },
                        int.Parse(codeRisque),
                        int.TryParse(codeObjet, out num) && num >= 0 ? num : default,
                        int.Parse(codeOption),
                        int.Parse(codeFormule));
                    if (hasIS) { cible = "ConditionsGarantie"; }
                    return RedirectToAction(job, cible, new
                    {
                        id = AlbParameters.BuildFullId(folder, new[] { codeFormule, codeOption, codeRisque }, tabGuid, addParamValue + (isForceReadOnly ? "||FORCEREADONLY|1" : string.Empty), modeNavig),
                        returnHome = saveCancel,
                        guidTab = tabGuid
                    });
                case "matriceformule":
                    CheckFormule(codeOffre, version, type, codeFormule, codeOption);
                    break;
                case "creationformulegarantie":
                    addParamValue += isModeConsult ? string.Empty :
                          readonlyDisplay == "true" ? "||FORCEREADONLY|1" :
                          (sessionLost ? "||IGNOREREADONLY|1" : "||AVNREFRESHUSERUPDATE|1||IGNOREREADONLY|1");
                    return RedirectToAction(job, cible, new
                    {
                        id = AlbParameters.BuildFullId(folder, new[] { codeFormule, codeOption }, tabGuid, addParamValue, modeNavig, isModeConsult ? new[] { "ConsultOnly" } : null)
                    });
            }

            return RedirectToAction(job, cible, new
            {
                id = AlbParameters.BuildStandardId(folder, tabGuid, addParamValue + ((isModeConsultationEcran || isOffreReadonly) && !isForceReadOnly ? string.Empty : "||IGNOREREADONLY|1"), modeNavig)
            });
        }

        private static void CheckFormule(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                channelClient.Channel.CheckFormule(codeOffre, version, type, codeFormule, codeOption);
            }
        }

        private Formule GetModelFormule(Affaire affaire, int numFormule)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                var affid = affaire.Adapt<Domain.Affaire.AffaireId>();
                Albingia.Kheops.DTO.FormuleDto formuleDto = null;
                var data = client.Channel.GetFormulesAffaire(affid, model.IsReadOnly);
                int codeOption = this.model.CodeOption.ParseInt().Value;
                if (numFormule > 0)
                {
                    formuleDto = data.Formules.First(x => x.FormuleNumber == numFormule);
                    if (affaire.Type == Consts.TYPE_OFFRE && !formuleDto.Options.Any(o => o.OptionNumber == codeOption))
                    {
                        // add new option
                        formuleDto = client.Channel.InitOptionFormuleOffre(affid, formuleDto.FormuleNumber, formuleDto.Risque.Numero, new int[0], codeOption);
                    }
                }
                else
                {
                    if (!data.Formules.Where(x => x.Options.Any()).Any())
                    {
                        formuleDto = client.Channel.InitFirstFormuleAffaire(affid);
                        model.CodeFormule = formuleDto.FormuleNumber.ToString();
                        model.CodeOption = formuleDto.Options.First().OptionNumber.ToString();
                    }
                    else
                    {
                        // user must select one Risque
                        if (affaire.NumeroAvenant > 0)
                        {
                            model.NewFormulaDateAvenant = data.DateEffetAvenant;
                        }
                    }
                }

                if (formuleDto != null)
                {
                    Formule f = formuleDto?.Adapt<Formule>();
                    f.Applications = GetInfoApplication(affaire, f);
                    f.DateEffetAvenantContrat = data.DateEffetAvenant;
                    using (var clientRef = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IReferentielPort>())
                    {
                        f.ListeNatures = clientRef.Channel.GetNaturesGarantie()
                            .Where(n => n.Code != "A")
                            .Select(n => new LabeledValue(n.Code, n.Libelle)).ToArray();
                    }
                    MapPorteesObjets(f);
                    f.IsAvnDisabled = affaire.NumeroAvenant > 0 && !f.Options.First(o => o.Numero == codeOption).IsModifiedForAvenant;
#if DEBUG
                    System.Diagnostics.Trace.WriteLine($"formula: {f.Numero}");
#endif
                    return f;
                }
            }
#if DEBUG
            System.Diagnostics.Trace.WriteLine($"no formula");
#endif
            return null;
        }

        private void MapPorteesObjets(Formule f)
        {
            if (f == null || f.Risque == null) return;
            bool isMultiObjets = f.Options.First(o => o.Numero == model.CodeOption.ParseInt().Value).Applications.Count(a => a.Niveau == Domain.Formule.ApplicationNiveau.Objet) > 1
                || f.Risque.Objets.Count > 1;
            f.Options.First().Volets
                .SelectMany(v => v.Blocs.SelectMany(b => b.Garanties.Where(g => g.MayHavePortee || isMultiObjets)))
                .ToList()
                .ForEach(g1 =>
                {
                    MapPorteesObjetsGarantie(f, g1);
                });
        }

        private void MapPorteesObjetsGarantie(Formule f, Models.FormuleGarantie.Garantie garantie)
        {
            var option = f.Options.First(o => o.Numero == model.CodeOption.ParseInt().Value);
            if (!garantie.MayHavePortee && option.Applications.Count() < 2 && f.Risque.Objets.Count < 2)
            {
                return;
            }
            if (!garantie.HasPortees)
            {
                InitializePortees(garantie, option.Applications.ToArray(), f.Risque);
            }
            IEnumerable<LabeledValue> unitesPrimes;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IReferentielPort>())
            {
                unitesPrimes = client.Channel.GetUnitesPrimes(new CibleFiltre { CodeCible = f.Risque.Cible.Code, CodeBranche = f.Risque.Branche.Code }).Select(u => new LabeledValue(u.Code, u.Libelle));
            }
            bool isWholeRisque = !option.Applications.Any(ap => ap.NumObjet > 0);
            garantie.Portees.UnitesTaux = unitesPrimes;
            garantie.Portees.CodeRisque = f.Risque.Numero.ToString();
            garantie.Portees.DesignationRisque = f.Risque.Designation;
            garantie.Portees.AlimentationAssiette = garantie.TypeAlimentation.Code;
            if (garantie.HasPortees)
            {
                if (isWholeRisque)
                {
                    MapPorteesFromRisqueObjets(f, garantie);
                }
                else
                {
                    MapPorteesFromApplications(f, garantie);
                }
            }
        }

        private void MapPorteesFromApplications(Models.FormuleGarantie.Formule f, Models.FormuleGarantie.Garantie g1)
        {
            if (f == null || f.Risque == null) return;
            var option = f.Options.First(o => o.Numero == model.CodeOption.ParseInt().Value);
            foreach (var ap in option.Applications)
            {
                var objet = f.Risque.Objets.First(o => o.Code == ap.NumObjet);
                var p = g1.Portees.ObjetsRisque.FirstOrDefault(x => x.NumObjet == objet.Code);
                if (p == null)
                {
                    p = new PorteeObjet
                    {
                        NumObjet = objet.Code,
                        PrimeMntCalcule = 0,
                        Valeur = 0,
                        IsSelected = false
                    };

                    g1.Portees.ObjetsRisque.Add(p);
                }
                else
                {
                    p.IsSelected = true;
                }

                p.CodeType = objet.Type;
                p.CodeUnite = objet.Unite;
                p.LabelObjet = objet.Description;
                p.Valeur = objet.Valeur;
            }
        }

        private void MapPorteesFromRisqueObjets(Formule f, Models.FormuleGarantie.Garantie g1)
        {
            if (f == null || f.Risque == null) return;
            foreach (var o in f.Risque.Objets)
            {
                var p = g1.Portees.ObjetsRisque.FirstOrDefault(x => x.NumObjet == o.Code);
                if (p == null)
                {
                    p = new PorteeObjet
                    {
                        NumObjet = o.Code,
                        PrimeMntCalcule = 0,
                        Valeur = 0,
                        IsSelected = false
                    };

                    g1.Portees.ObjetsRisque.Add(p);
                }
                else
                {
                    p.IsSelected = true;
                }
                p.CodeType = o.Type;
                p.CodeUnite = o.Unite;
                p.LabelObjet = o.Description;
                p.Valeur = o.Valeur;
            }
        }

        private void LoadInfoPage(string id, Affaire affaire)
        {
            switch (affaire.Type)
            {
                case Consts.TYPE_OFFRE:
                    LoadOffre(affaire);
                    break;
                case Consts.TYPE_CONTRAT:
                    LoadContrat(affaire);
                    DefineAvenantData();
                    break;
            }

            if (this.model.Offre == null && this.model.Contrat == null)
            {
                throw new Exception("L'offre ou le contrat ne peut pas être null");
            }

            DefineReadonly(affaire.Type);

            this.model.PageTitle = "Formule de garanties";
            this.model.Bandeau = null;
            LoadBandeau(id, affaire);
            if (this.model.ModeNavig == ModeConsultation.Historique.AsCode())
            {
                LoadRisque(id, affaire);
            }

            this.model.CodeFormule = "0";
            this.model.CodeOption = "0";
            this.model.LettreLib = "A";
            this.model.Libelle = string.Empty;
            this.model.IsHisto = affaire.IsHisto;
            string[] contextIds = id.Split('_');
            if (id.Split('_').Length > 3)
            {
                this.model.CodeFormule = contextIds[3];
                this.model.CodeOption = contextIds[4].ParseInt(null).GetValueOrDefault(1) < 1 ? "1" : contextIds[4];
                this.model.FormGen = contextIds.Length > 5 ? contextIds[5] : "0";
                if (affaire.Type == Consts.TYPE_OFFRE)
                {
                    this.model.PageTitle += $" - Option {model.CodeOption}";
                }
            }
            //LoadApplique(id, affaire);
            SetArbreNavigation();
        }

        private string InitAvenantFormule(string id, Affaire affaire)
        {
            var idList = id.Split('_');
            this.model.CodeFormule = idList[3];
            this.model.CodeOption = idList[4].Split(new[] { PageParamContext.TabGuidKey }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "1";
            var f = LoadFormule(affaire, int.Parse(this.model.CodeFormule));
            if (f != null)
            {
                var option = f.Options.First(x => x.Numero == int.Parse(this.model.CodeOption));
                if (!option.IsModifiedForAvenant)
                {
                    var (prefix, context) = PageParamContext.BuildFromString(id);
                    context.IsLocked = true;
                    id = context.BuildFullString(prefix);

                    if (AlbSessionHelper.CurrentFolders.TryGetValue(new FolderKey(AlbSessionHelper.ConnectedUser, context.TabGuid, affaire), out var info))
                    {
                        info.ReadOnlyFolder = true;
                    }
                }
            }

            return id;
        }

        private Formule LoadFormule(Affaire affaire, int numeroFormule, bool clearCache = false)
        {
            if (clearCache)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
                {
                    client.Channel.CancelFormuleAffaireChanges(affaire.Adapt<Domain.Affaire.AffaireId>());
                }
            }
            var f = GetModelFormule(affaire, numeroFormule);

            AssignRisque(f, model.CodeOption.ParseInt().Value);
            model.OptionsFormule = f;
            return f;
        }

        private void LoadInfoApplications(Affaire affaire, Formule formule)
        {
            if (formule?.Applications != null)
            {
                this.model.InfoApplication = formule?.Applications;
            }
            else
            {
                InfoApplication infoApp = GetInfoApplication(affaire, formule);
                this.model.InfoApplication = infoApp;
            }
        }

        private InfoApplication GetInfoApplication(Affaire affaire, Formule formule)
        {
            var affid = affaire.Adapt<Domain.Affaire.AffaireId>();
            List<Albingia.Kheops.DTO.FormuleDto> formuleList;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                formuleList = client.Channel.GetFormulesAffaire(affid, model.IsReadOnly).Formules.ToList();
            }
            IEnumerable<Albingia.Kheops.DTO.RisqueDto> risques;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IRisquePort>())
            {
                risques = client.Channel.GetRisques(affid);
            }
            int codeOption = model.CodeOption.ParseInt().Value;
            var infoApp = new InfoApplication
            {
                IsModeOffre = affaire.Type == Consts.TYPE_OFFRE,
                Risques = risques.Select(dto => dto.Adapt<Risque>()).ToList()
            };
#if DEBUG
            System.Diagnostics.Trace.WriteLine($"GetInfoApplication: affaire {affaire.CodeOffre}");
#endif
            if (formule == null)
            {
#if DEBUG
                System.Diagnostics.Trace.WriteLine($"GetInfoApplication: no formula");
#endif
                infoApp.NumFormule = 0;
                infoApp.NumRisque = 0;
                infoApp.LibelleFormule = string.Empty;
                infoApp.LettreAlpha = string.Empty;
                infoApp.NumsObjets = new int[0];
                infoApp.DateAvenantOption = model.NewFormulaDateAvenant;
            }
            else
            {
#if DEBUG
                System.Diagnostics.Trace.WriteLine($"GetInfoApplication: formula {formule.Numero}");
#endif
                infoApp.NumFormule = formule.Numero;
                infoApp.NumRisque = formule.Risque.Numero;
                infoApp.LibelleFormule = formule.Libelle;
                infoApp.LettreAlpha = formule.Alpha;
                infoApp.NumsObjets = formule.Options.FirstOrDefault(x => x.Numero == codeOption)?.Applications.Any(ap => ap.Niveau == Domain.Formule.ApplicationNiveau.Risque) ?? true
                    ? new int[0] : formule.Options.First(x => x.Numero == codeOption).Applications.Select(ap => ap.NumObjet).ToArray();
                infoApp.DateAvenantOption = formule.DateEffetAvenantContrat;
            }

            var rsqList = infoApp.Risques.ToDictionary(r => r.Numero);
            IEnumerable<Albingia.Kheops.DTO.ApplicationDto> allApplications = null;
            if (affid.TypeAffaire == Domain.Affaire.AffaireType.Contrat || formule is null)
            {
                allApplications = formuleList.SelectMany(f => f.Options).SelectMany(o => o.Applications);
            }
            else
            {
                allApplications = formuleList
                    .SelectMany(f => f.Options.Where(o => f.FormuleNumber != formule.Numero || o.OptionNumber == codeOption))
                    .SelectMany(o => o.Applications);
            }
            int[] numRisquesAppliques = allApplications.Where(ap => ap.Niveau == Domain.Formule.ApplicationNiveau.Risque)
                .Select(ap => ap.NumRisque)
                .ToArray();

            var objAppliques = allApplications.Where(ap => ap.Niveau != Domain.Formule.ApplicationNiveau.Risque).Select(ap => new { ap.NumObjet, ap.NumRisque }).ToArray();

            foreach (var rsq in rsqList.Keys.Where(k => numRisquesAppliques.Contains(k)))
            {
                foreach (var objet in rsqList[rsq].Objets)
                {
                    objet.IsApplique = true;
                }
            }

            foreach (var rsq in rsqList.Keys.Where(k => !numRisquesAppliques.Contains(k)))
            {
                var apps = allApplications.Where(ap => ap.NumRisque == rsq && infoApp.NumsObjets.Contains(ap.NumObjet));
                foreach (var objet in rsqList[rsq].Objets)
                {
                    objet.IsApplique = apps.Any(ap => ap.NumObjet == objet.Code) || objAppliques.Any(x => objet.NumRisque == x.NumRisque && objet.Code == x.NumObjet);
                }
            }

            infoApp.InitializeLibelleApplication();
            return infoApp;
        }

        private void LoadRisque(string id, Affaire affaire)
        {
            var idParts = id.Split('_');
            List<RisqueObjetPlatDto> risqueList = null;
            using (var clientRisque = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                if (idParts.Length > 3)
                {
                    string codeFormule = id.Split('_')[3];
                    string codeOption = id.Split('_')[4];
                    risqueList = clientRisque.Channel.GetRisqueObjetFormule(
                        affaire.CodeOffre,
                        affaire.Version.ToString(),
                        affaire.Type,
                        affaire.NumeroAvenant.ToString(),
                        codeFormule,
                        codeOption,
                        model.ModeNavig.ParseCode<ModeConsultation>());
                }
                else
                {
                    risqueList = clientRisque.Channel.GetEarliestRisqueObjetFormule(affaire.CodeOffre, affaire.Version.ToString(), affaire.Type);
                }
            }

            if (risqueList?.Any() == true)
            {
                var r = risqueList.First();
                model.NumRisque = (int)r.CodeRsq;
                model.NbObjets = risqueList.Select(x => x.CodeObj).Distinct().Count();
                model.CodeCible = r.CodeCible;
                model.Cible = r.Cible;
                model.CibleLib = r.DescCible;
            }
        }

        private void AssignRisque(Formule formule, int? numOption = null)
        {
            if (formule != null && (this.model.NumRisque < 1 || this.model.ModeNavig == ModeConsultation.Historique.AsCode()))
            {
                if (numOption.GetValueOrDefault() == 0)
                {
                    numOption = 1;
                }
                model.NumRisque = formule.Risque.Numero;
                model.NbObjets = formule.Risque.Objets.Count;
                model.NbObjetsSelectionnes = formule.Options.First(x => x.Numero == numOption).Applications.Count(a => a.Niveau == Domain.Formule.ApplicationNiveau.Objet);
                if (model.NbObjetsSelectionnes == 0)
                {
                    model.NbObjetsSelectionnes = formule.Risque.Objets.Count;
                }
                model.CodeCible = formule.Risque.Cible.Id;
                model.Cible = formule.Risque.Cible.Code;
                model.CibleLib = formule.Risque.Cible.Label;
            }
        }

        private void InitializePortees(Models.FormuleGarantie.Garantie garantie, IEnumerable<Application> applications, Risque risque)
        {
            if (garantie != null)
            {
                garantie.Portees = new PorteesGarantie
                {
                    CodeAction = garantie.HasTariffs ? Domain.Formule.ActionValue.Accorde.AsString() : null,
                    TypesCalculs = Enum.GetValues(typeof(Domain.Formule.TypeCalcul))
                        .Cast<Domain.Formule.TypeCalcul>()
                        .Select(v => new LabeledValue(v.AsString(), string.IsNullOrWhiteSpace(v.AsString()) ? string.Empty : v.ToString()))
                };

                if (applications.Count() == 1 && applications.Single().NumObjet == 0)
                {
                    garantie.Portees.ObjetsRisque = risque.Objets.Select(o => new PorteeObjet
                    {
                        NumObjet = o.Code,
                        CodeType = o.Type,
                        CodeUnite = o.Unite,
                        LabelObjet = o.Description,
                        Valeur = o.Valeur,
                        IsSelected = garantie.HasTariffs
                    }).ToList();
                }
                else
                {
                    var objets = risque.Objets.ToDictionary(o => o.Code);
                    garantie.Portees.ObjetsRisque = applications.Select(ap => new PorteeObjet
                    {
                        NumObjet = ap.NumObjet,
                        CodeType = objets[ap.NumObjet].Type,
                        CodeUnite = objets[ap.NumObjet].Unite,
                        LabelObjet = objets[ap.NumObjet].Description,
                        Valeur = objets[ap.NumObjet].Valeur,
                        IsSelected = garantie.HasTariffs
                    }).ToList();
                }
            }
        }

        private void LoadBandeau(string id, Affaire affaire)
        {
            if (affaire.CodeOffre.IsEmptyOrNull())
            {
                model.AfficherBandeau = false;
            }
            else
            {
                using (var policeService = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    model.AfficherBandeau = policeService.Channel.TestExistanceOffre(affaire.CodeOffre);
                }
            }

            model.AfficherNavigation = model.AfficherBandeau;
            model.Bandeau = null;
            SetBandeauNavigation(id);
        }

        private void DefineReadonly(string type)
        {
            if (type == AlbConstantesMetiers.TYPE_OFFRE)
            {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            }
            else
            {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
            }
            this.model.IsModifHorsAvenant = IsModifHorsAvenant;
        }

        private void DefineAvenantData()
        {
            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            switch (typeAvt)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                    break;
                default:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    break;
            }

            if (model.ScreenType != AlbConstantesMetiers.SCREEN_TYPE_CONTRAT)
            {
                model.IsModeAvenant = true;
                model.DateEffetAvenant = model.Contrat.DateEffetAvenant;
            }
        }

        private void LoadContrat(Affaire affaire)
        {
            using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var CommonOffreClient = chan.Channel;
                var infosBase = CommonOffreClient.LoadInfosBase(affaire.CodeOffre, affaire.Version.ToString(), affaire.Type, model.NumAvenantPage, model.ModeNavig);
                model.Contrat = new ContratDto()
                {
                    CodeContrat = infosBase.CodeOffre,
                    VersionContrat = Convert.ToInt64(infosBase.Version),
                    Type = infosBase.Type,
                    NumAvenant = infosBase.NumAvenant,
                    NumInterneAvenant = infosBase.NumAvenant,
                    Branche = infosBase.Branche.Code,
                    BrancheLib = infosBase.Branche.Nom,
                    Cible = infosBase.Branche.Cible.Code,
                    CibleLib = infosBase.Branche.Cible.Nom,
                    CourtierGestionnaire = infosBase.CabinetGestionnaire.Code,
                    Descriptif = infosBase.Descriptif,
                    CodeInterlocuteur = infosBase.CabinetGestionnaire.Code,
                    NomInterlocuteur = infosBase.CabinetGestionnaire.Inspecteur,
                    CodePreneurAssurance = Convert.ToInt32(infosBase.PreneurAssurance.Code),
                    NomPreneurAssurance = infosBase.PreneurAssurance.NomAssure,
                    PeriodiciteCode = infosBase.Periodicite,
                    DateEffetAvenant = AlbConvert.ConvertIntToDate((infosBase.DateAvnAnnee * 10000) + (infosBase.DateAvnMois * 100) + infosBase.DateAvnJour),
                    Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                    Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                };
            }

            if (model.Contrat != null && model.Contrat.Branche.ContainsChars())
            {
                model.Branche = model.Contrat.Branche;
                model.BrancheLib = model.Contrat.BrancheLib;
            }

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var policeServicesClient = channelClient.Channel;
                model.Contrat.Risques = policeServicesClient.ObtenirRisques(model.ModeNavig.ParseCode<ModeConsultation>(), affaire.CodeOffre, affaire.Version, affaire.Type, affaire.NumeroAvenant.ToString());
            }
        }

        private void LoadDataAvenant()
        {
            if (model.IsTraceAvnExist)
            {
                // Une trace avenant existe-elle en base ?
                model.IsAvenantModificationLocale = true;
            }

            if (model.IsAvnRefreshUserUpdate)
            {
                // l'utilisateur vient-il de cocher la case?
                model.IsAvenantModificationLocale = true;
            }

            if (model.CodeFormule == "0")
            {
                // cas de creation de formule
                model.IsAvenantModificationLocale = true;
            }

            LoadInfoDatesAvenant();
        }

        private void LoadInfoDatesAvenant()
        {
            var dateModifRsq = default(DateTime?);

            if (model.NumRisque != 0)
            {
                var risque = model.Contrat.Risques.FirstOrDefault(m => m.Code == model.NumRisque);

                dateModifRsq = risque.DateEffetAvenantModificationLocale;
                if (!dateModifRsq.HasValue)
                {
                    dateModifRsq = !risque.EntreeGarantie.HasValue || model.DateEffetAvenant > risque.EntreeGarantie
                        ? model.DateEffetAvenant
                        : risque.EntreeGarantie;
                }
            }

            if (model.IsAvenantModificationLocale && !model.DateEffetAvenantModificationLocale.HasValue)
            {
                model.DateEffetAvenantModificationLocale = dateModifRsq;
            }

            model.DateModificationRsq = dateModifRsq.ToString(AlbConvert.DateFormat);
            model.DateFinEffet = model.Contrat.FinEffetAnnee == 0 ?
                string.Empty
                : new DateTime(model.Contrat.FinEffetJour, model.Contrat.FinEffetMois, model.Contrat.FinEffetAnnee).ToString(AlbConvert.DateFormat);
        }

        private void LoadOffre(Affaire affaire)
        {
            using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var CommonOffreClient = chan.Channel;
                model.Offre = new Offre_MetaModel();
                model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(affaire.CodeOffre, affaire.Version.ToString(), affaire.Type, model.NumAvenantPage, model.ModeNavig));
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                if (model.Offre.Branche != null && !string.IsNullOrEmpty(model.Offre.Branche.Code))
                {
                    model.Branche = model.Offre.Branche.Code;
                    model.BrancheLib = model.Offre.Branche.Nom;
                }
            }

            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
        }

        private void SetBandeauNavigation(string id)
        {
            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());

                //Gestion des Etapes
                if (model.Offre != null)
                {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null)
                {
                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            model.Bandeau.StyleBandeau = model.ScreenType;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }

        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                // Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre(
                    etape: "Formule",
                    codeFormule: this.model.CodeFormule.ParseInt().Value,
                    numOption: this.model.CodeOption.ParseInt().Value);
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Formule", codeFormule: Convert.ToInt32(model.CodeFormule));
            }
        }

        private List<ModeleFormVolet> SetListVolets()
        {
            var volets = new List<ModeleFormVolet>();

            volets.Add(new ModeleFormVolet
            {
                Code = "ANNUL",
                Description = "Annulation",
                Blocs = SetListBlocs()
            });

            volets.Add(new ModeleFormVolet
            {
                Code = "DOMMAG",
                Description = "Dommages"
            });

            return volets;
        }

        private List<ModeleFormBloc> SetListBlocs()
        {
            var blocs = new List<ModeleFormBloc>();
            var modeles = new List<ModeleFormModele>();
            modeles.Add(new ModeleFormModele { Code = "Modele" });

            blocs.Add(new ModeleFormBloc
            {
                Code = "ANNORG",
                Description = "Annulation de l'organisateur",
                Modeles = modeles
            });


            blocs.Add(new ModeleFormBloc
            {
                Code = "ANNOTS",
                Description = "Annulation de l'organisateur (tous risques sauf)"
            });


            return blocs;
        }

        private List<ModeleFormModele> SetListModeles()
        {
            return new List<ModeleFormModele>();
        }

        private void SetLibAppliqueA(FormuleDto formule, List<RisqueDto> risques)
        {
            var selectedRisque = string.Empty;
            var selectedObjet = string.Empty;

            if (!string.IsNullOrEmpty(model.ObjetRisqueCode))
            {
                var countSelectedObjet = -1;
                if (model.ObjetRisqueCode.Split(';').Length > 1)
                {
                    countSelectedObjet = model.ObjetRisqueCode.Split(';')[1].Split('_').Length;
                }
                var rsqDto = risques.FirstOrDefault(m => m.Code.ToString() == model.ObjetRisqueCode.Split(';')[0]);

                if (rsqDto != null)
                {
                    if (risques.Count() == 1)
                    {
                        if (rsqDto.Objets.Count() == 1 || rsqDto.Objets.Count() == countSelectedObjet || countSelectedObjet == -1)
                        {
                            foreach (var obj in rsqDto.Objets)
                            {
                                selectedObjet += "_" + obj.Code;
                            }
                            if (!string.IsNullOrEmpty(selectedObjet))
                            {
                                model.ObjetRisqueCode = rsqDto.Code + ";" + selectedObjet.Substring(1);
                                model.ObjetRisque = "à l'ensemble du risque";
                            }
                        }
                        else
                        {
                            foreach (var obj in rsqDto.Objets)
                            {
                                if (("_" + model.ObjetRisqueCode.Split(';')[1] + "_").Replace("_" + obj.Code + "_", "") != "_" + model.ObjetRisqueCode.Split(';')[1] + "_")
                                {
                                    selectedObjet += "_" + obj.Code;
                                }
                            }
                            if (!string.IsNullOrEmpty(selectedObjet))
                            {
                                model.ObjetRisqueCode = rsqDto.Code + ";" + selectedObjet.Substring(1);
                                model.ObjetRisque = "à l'objet(s) du risque";
                                //model.ObjetRisque = "à " + countSelectedObjet + " objet(s) du risque";
                            }
                        }
                    }
                    else
                    {
                        var rsq = risques.FirstOrDefault(m => m.Code.ToString() == model.ObjetRisqueCode.Split(';')[0]);

                        if (rsq.Objets.Count() == 1 || rsq.Objets.Count() == countSelectedObjet || countSelectedObjet == -1)
                        {
                            foreach (var obj in rsq.Objets)
                            {
                                selectedObjet += "_" + obj.Code;
                            }
                            if (!string.IsNullOrEmpty(selectedObjet))
                            {
                                model.ObjetRisqueCode = rsq.Code + ";" + selectedObjet.Substring(1);
                                model.ObjetRisque = "au risque " + rsq.Code + " '" + rsq.Designation + "'";
                            }
                        }
                        else
                        {
                            foreach (var obj in rsq.Objets)
                            {
                                if (("_" + model.ObjetRisqueCode.Split(';')[1] + "_").Replace("_" + obj.Code + "_", "") != "_" + model.ObjetRisqueCode.Split(';')[1] + "_")
                                {
                                    selectedObjet += "_" + obj.Code;
                                }
                            }
                            if (!string.IsNullOrEmpty(selectedObjet))
                            {
                                model.ObjetRisqueCode = rsq.Code + ";" + selectedObjet.Substring(1);
                                model.ObjetRisque = "à l'objet(s) du risque " + rsq.Code + " '" + rsq.Designation + "'";
                                //model.ObjetRisque = "à " + countSelectedObjet + " objet(s) du risque " + rsq.Code + " '" + rsq.Designation + "'";
                            }
                        }
                    }
                }
                else if (model.Contrat != null)
                {
                    //model.ObjetRisqueCode = model.ObjetRisqueCode;
                    var rsqContrat = model.Contrat.Risques.FirstOrDefault(m => m.Code.ToString() == model.ObjetRisqueCode.Split(';')[0]);

                    if (rsqContrat != null)
                    {
                        if (model.Contrat.Risques.Count() == 1)
                        {
                            if (rsqContrat.Objets.Count() == 1 || rsqContrat.Objets.Count() == countSelectedObjet || countSelectedObjet == -1)
                            {
                                foreach (var obj in rsqContrat.Objets)
                                {
                                    selectedObjet += "_" + obj.Code;
                                }
                                if (!string.IsNullOrEmpty(selectedObjet))
                                {
                                    model.ObjetRisqueCode = rsqContrat.Code + ";" + selectedObjet.Substring(1);
                                    model.ObjetRisque = "à l'ensemble du risque";
                                }
                            }
                            else
                            {
                                foreach (var obj in rsqContrat.Objets)
                                {
                                    if (("_" + model.ObjetRisqueCode.Split(';')[1] + "_").Replace("_" + obj.Code + "_", "") != "_" + model.ObjetRisqueCode.Split(';')[1] + "_")
                                    {
                                        selectedObjet += "_" + obj.Code;
                                    }
                                }
                                if (!string.IsNullOrEmpty(selectedObjet))
                                {
                                    model.ObjetRisqueCode = rsqContrat.Code + ";" + selectedObjet.Substring(1);
                                    model.ObjetRisque = "à l'objet(s) du risque";
                                    //model.ObjetRisque = "à " + countSelectedObjet + " objet(s) du risque";
                                }
                            }
                        }
                        else
                        {
                            var rsq = model.Contrat.Risques.FirstOrDefault(m => m.Code.ToString() == model.ObjetRisqueCode.Split(';')[0]);

                            if (rsq.Objets.Count() == 1 || rsq.Objets.Count() == countSelectedObjet || countSelectedObjet == -1)
                            {
                                foreach (var obj in rsq.Objets)
                                {
                                    selectedObjet += "_" + obj.Code;
                                }
                                if (!string.IsNullOrEmpty(selectedObjet))
                                {
                                    model.ObjetRisqueCode = rsq.Code + ";" + selectedObjet.Substring(1);
                                    model.ObjetRisque = "au risque " + rsq.Code + " '" + rsq.Designation + "'";
                                }
                            }
                            else
                            {
                                foreach (var obj in rsq.Objets)
                                {
                                    if (("_" + model.ObjetRisqueCode.Split(';')[1] + "_").Replace("_" + obj.Code + "_", "") != "_" + model.ObjetRisqueCode.Split(';')[1] + "_")
                                    {
                                        selectedObjet += "_" + obj.Code;
                                    }
                                }
                                if (!string.IsNullOrEmpty(selectedObjet))
                                {
                                    model.ObjetRisqueCode = rsq.Code + ";" + selectedObjet.Substring(1);
                                    model.ObjetRisque = "à l'objet(s) du risque " + rsq.Code + " '" + rsq.Designation + "'";
                                    //model.ObjetRisque = "à " + countSelectedObjet + " objet(s) du risque " + rsq.Code + " '" + rsq.Designation + "'";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (!formule.OffreAppliqueA)
                {
                    if (risques?.Any() == true)
                    {
                        foreach (var obj in risques[0].Objets)
                        {
                            selectedObjet += "_" + obj.Code;
                        }
                        model.ObjetRisqueCode = risques[0].Code + ";" + selectedObjet.Substring(1);
                        if (risques.Count() == 1)
                        {
                            model.ObjetRisque = "à l'ensemble du risque";
                        }
                        else
                        {
                            model.ObjetRisque = "au risque " + risques[0].Code + " '" + risques[0].Designation + "'";
                        }
                    }
                    else
                    {
                        model.ObjetRisque = "à l'ensemble du contrat";
                    }
                }
            }
        }

        private void SetListRisquesListObjets(List<RisqueDto> risques)
        {
            if (risques?.Any() != true)
            {
                return;
            }

            model.ObjetsRisquesAll = new ModeleFormuleGarantieLstObjRsq
            {
                TableName = "ListRsqObj"
            };

            var list = new List<ModeleRisque>();
            risques.ForEach(m => list.Add((ModeleRisque)m));
            model.ObjetsRisquesAll.Risques = list;
            model.ObjetsRisquesAll.IsReadonly = model.IsReadOnly;
        }

        private GarantieDetailInfoDto GetDetailsValeursGarantie(Models.FormuleGarantie.IdentifiantGarantie identifiant)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                return channelClient.Channel.GetInfoDetailsGarantie(
                    identifiant.Affaire.CodeOffre,
                    identifiant.Affaire.Version.ToString(),
                    identifiant.Affaire.Type,
                    identifiant.Sequence.ToString(),
                    identifiant.NumFormule.ToString(),
                    identifiant.NumOption.ToString());
            }
        }

        private Models.FormuleGarantie.Formule GetFormuleTest()
        {
            var data = JsonConvert.DeserializeObject<ModeleFormuleGarantie>(System.IO.File.ReadAllText(Server.MapPath("~/knockout/components/options-formule/test-data/test-options-formules.json")));

            var formule = new Models.FormuleGarantie.Formule()
            {
                ListeNatures = new List<LabeledValue>
                {
                    LabeledValue.Create("C", "C - Comprise"),
                    LabeledValue.Create("E", "E - Exclue")
                },
                //ListeNatures = refService.GetNaturesGarantie().Select(n => new LabeledValue { Code = n.Code, Label = n.Libelle, IsCodePrefixLabel = true }),
                Options = new List<Models.FormuleGarantie.Option>() { new Models.FormuleGarantie.Option() }
            };

            formule.Options.First().Volets = data.Volets.Select(v =>
            {
                var volet = new Models.FormuleGarantie.Volet();
                volet.AvenantModifie = v.ModifAvt;
                volet.Caractere = LabeledValue.Create(v.Caractere, "V.");
                volet.Code = v.Code;
                volet.IsChecked = v.isChecked;
                volet.IsCollapsed = v.IsVoletCollapse == "RP";
                volet.Libelle = v.Description;
                volet.Type = "V";

                volet.Blocs = v.Blocs.Select(b =>
                {
                    var bloc = new Models.FormuleGarantie.Bloc();
                    bloc.Type = "B";
                    bloc.Caractere = LabeledValue.Create(b.Caractere, "B.");
                    bloc.Code = b.Code;
                    bloc.IsChecked = b.isChecked;
                    bloc.IsCollapsed = v.isChecked;
                    bloc.Libelle = b.Description;

                    Func<IModeleGarantieNiveau, Models.FormuleGarantie.Garantie> f = null;
                    f = (g) =>
                    {
                        var garantie = new Models.FormuleGarantie.Garantie();
                        garantie.Caractere = LabeledValue.Create(g.Caractere, g.LibCaractere);
                        garantie.Code = g.FCTCodeGarantie;
                        garantie.UniqueId = g.Code;
                        garantie.Libelle = g.Description;
                        garantie.IsChecked = g.isChecked;
                        garantie.InventairePossible = g.InvenPossible == "O";
                        garantie.IdInventaire = g.CodeInven;
                        garantie.IsFlagModifie = g.FlagModif == "O";
                        garantie.Nature = LabeledValue.Create(g.ParamNatModVal ?? g.Nature, "");
                        garantie.NatureModifiable = g.ParamNatMod == "O";
                        garantie.Niveau = g.Niveau;
                        if (g.Niveau == 1)
                        {
                            garantie.TypeAlimentation = LabeledValue.Create(((ModeleGarantieNiveau1)g).AlimAssiette, "");
                            //TODO: portee
                            garantie.HasPortees = !((ModeleGarantieNiveau1)g).Action.IsEmptyOrNull();
                        }
                        if (g.Niveau == 2)
                        {
                            garantie.InventairePossible = true;
                            garantie.TypeInventaire = null;
                        }
                        //TODO: date sortie
                        garantie.DateSortie = null;

                        garantie.SousGaranties = g.Garanties?.Select(gN =>
                        {
                            return f(gN);
                        }).ToList();

                        return garantie;
                    };

                    bloc.Garanties = b.Modeles.First().Modeles.Select(g1 => f(g1)).ToList();

                    return bloc;
                }).ToList();

                return volet;
            }).ToList();
            return formule;
        }
    }
}
