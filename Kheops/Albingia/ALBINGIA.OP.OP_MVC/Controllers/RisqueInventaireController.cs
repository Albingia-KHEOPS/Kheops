using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Extension;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.Inventaires;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie;
using ALBINGIA.OP.OP_MVC.Models.ModelesInventaire;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using Mapster;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Ecran.DetailsRisque;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Domain = Albingia.Kheops.OP.Domain;
using Services = Albingia.Kheops.OP.Application.Port.Driver;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class RisqueInventaireController: RemiseEnVigueurController<Inventaire_MetaModel> {
        public override bool? IsAvnDisabled {
            get {
                if (IsReadonly || IsModifHorsAvenant || (int.TryParse(this.model.NumAvenantPage, out int avn) ? avn : 0) <= 0) {
                    return null;
                }
                AffaireId affaireId = null;
                int rsq = int.TryParse(this.model.CodeRisque, out int r) ? r : default;
                if (this.model.CodePolicePage.IsEmptyOrNull()) {
                    return true;
                }
                affaireId = new AffaireId {
                    CodeAffaire = this.model.CodePolicePage,
                    NumeroAliment = int.Parse(this.model.VersionPolicePage),
                    TypeAffaire = AffaireType.Contrat,
                    NumeroAvenant = avn
                };
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IRisquePort>()) {
                    return client.Channel.IsAvnDisabled(affaireId, rsq);
                }
            }
        }

        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult Index(string id) {
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        public ActionResult SaveLineInventaire(string codeOffre, string version, string type, string codeInven, string typeInven, ModeleInventaireGridRow data) {
            //JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleInventaireGridRow>.GetSerializer();

            var inventaireLigne = data;
        //    var inventaireLigne = JsonConvert.DeserializeObject<ModeleInventaireGridRow>(data, new JsonSerializerSettings {
        //        Converters = new List<JsonConverter> { new DecimalConverter() },
        //        DateFormatString = AlbConvert.AppCulture.DateTimeFormat.ShortDatePattern + ' ' + AlbConvert.AppCulture.DateTimeFormat.LongTimePattern
        //});
            //var inventaireLigne = serialiser.ConvertToType<ModeleInventaireGridRow>(serialiser.DeserializeObject(data));

            string resultCheck = CheckLineInventaire(inventaireLigne);
            if (!string.IsNullOrEmpty(resultCheck)) {
                throw new Exception(resultCheck);
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                var result = serviceContext.SaveLineInventaire(codeOffre, Convert.ToInt32(version), type,
                                                               Convert.ToInt32(codeInven),
                                                               Convert.ToInt32(typeInven),
                                                               ModeleInventaireGridRow.LoadDto(inventaireLigne));
                var model = new ModeleInventaireGridRow();
                model = ((ModeleInventaireGridRow)result);
                var naturesLieu = result.NaturesLieu.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (!string.IsNullOrEmpty(model.NatureLieu)) {
                    var sItem = naturesLieu.FirstOrDefault(x => x.Value == model.NatureLieu);
                    if (sItem != null) {
                        sItem.Selected = true;
                    }
                }
                model.NaturesLieu = naturesLieu;

                var listpays = result.ListPays.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (!string.IsNullOrEmpty(model.Pays)) {
                    var sItem = listpays.FirstOrDefault(x => x.Value == model.Pays);
                    if (sItem != null) {
                        sItem.Selected = true;
                    }
                }

                model.ListPays = listpays;
                var codesMat = result.CodesMat.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (!string.IsNullOrEmpty(model.CodeMateriel)) {
                    var sItem = codesMat.FirstOrDefault(x => x.Value == model.CodeMateriel);
                    if (sItem != null) {
                        sItem.Selected = true;
                    }
                }

                model.CodesMateriel = codesMat;
                var codesExtension = result.CodesExtension.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (!string.IsNullOrEmpty(model.CodeExtension)) {
                    var sItem = codesExtension.FirstOrDefault(x => x.Value == model.CodeExtension);
                    if (sItem != null) {
                        sItem.Selected = true;
                    }
                }
                model.CodesExtension = codesExtension;

                var codesQualite = result.CodesQualite.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.CodesQualite = codesQualite;
                if (!string.IsNullOrEmpty(model.CodeQualite)) {
                    var sItem = codesQualite.FirstOrDefault(x => x.Value == model.CodeQualite);
                    if (sItem != null) {
                        sItem.Selected = true;
                    }
                }
                var codesRenonce = result.CodesRenonce.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.CodesRenonce = codesRenonce;
                if (!string.IsNullOrEmpty(model.CodeRenonce)) {
                    var sItem = codesRenonce.FirstOrDefault(x => x.Value == model.CodeRenonce);
                    if (sItem != null) {
                        sItem.Selected = true;
                    }
                }
                var codesRsqLoc = result.CodesRsqLoc.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.CodesRsqLoc = codesRsqLoc;
                if (!string.IsNullOrEmpty(model.CodeRsqLoc)) {
                    var sItem = codesRsqLoc.FirstOrDefault(x => x.Value == model.CodeRsqLoc);
                    if (sItem != null) {
                        sItem.Selected = true;
                    }
                }


                model.InventaireType = Convert.ToInt64(typeInven);

                return PartialView("InventaireRow", model);
            }
        }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetEmptyInventaire() {
            return CustomResult.JsonNetResult.NewResultToGet(new Inventaire {
                Infos = new List<InventoryItem>(),
                UniteListe = new List<Models.LabeledValue>(),
                TypeListe = new List<Models.LabeledValue>(),
                TaxeListe = new List<Models.LabeledValue>(),
                CodesExtensions = new List<Models.LabeledValue>()
            });
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetRefListesInventaire(Albingia.Kheops.OP.Domain.Referentiel.CibleFiltre filtre) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IReferentielPort>()) {
                return CustomResult.JsonNetResult.NewResultToGet(new {
                    unitesValeurs = client.Channel.GetUnitesValeursRisques(filtre).Select(u => new Models.LabeledValue(u.Code, u.Libelle)),
                    typesValeurs = client.Channel.GetTypesValeursRisques(filtre).Select(t => new Models.LabeledValue(t.Code, t.Libelle)),
                    typesTaxes = new List<Models.LabeledValue> { new Models.LabeledValue("HT", "HT"), new Models.LabeledValue("TTC", "TTC") }
                });
            }
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetGarantieInventaire(Models.FormuleGarantie.IdentifiantGarantie garantieId, Domain.Referentiel.CibleFiltre filtre) {
            var inventaire = new Inventaire {
                Infos = new List<InventoryItem>(),
                UniteListe = new List<Models.LabeledValue>(),
                TypeListe = new List<Models.LabeledValue>(),
                TaxeListe = new List<Models.LabeledValue>(),
                CodesExtensions = new List<Models.LabeledValue>()
            };
            Albingia.Kheops.DTO.InventaireDto invt;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>()) {
                invt = client.Channel.GetGarantieInventaire(
                    garantieId.Affaire.Adapt<Domain.Affaire.AffaireId>(),
                    garantieId.NumOption,
                    garantieId.NumFormule,
                    garantieId.CodeBloc,
                    garantieId.Sequence,
                    garantieId.IsReadonly);
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IReferentielPort>()) {
                if (invt != null) {
                    inventaire = invt.Adapt<Inventaire>();
                    inventaire.UniteListe = client.Channel.GetUnitesValeursRisques(filtre).Select(u => new Models.LabeledValue(u.Code, u.Libelle)).ToList();
                    inventaire.TypeListe = client.Channel.GetTypesValeursRisques(filtre).Select(t => new Models.LabeledValue(t.Code, t.Libelle)).ToList();
                    inventaire.TaxeListe = new List<Models.LabeledValue> {
                    new Models.LabeledValue(Domain.ModeTaxation.HorsTaxes.AsString(), Domain.ModeTaxation.HorsTaxes.AsString()),
                    new Models.LabeledValue(Domain.ModeTaxation.ToutesTaxesComprises.AsString(), Domain.ModeTaxation.ToutesTaxesComprises.AsString()) };

                    if (inventaire.NumeroType == 3) {
                        inventaire.CodesExtensions = client.Channel.GetCodeIndisponibilites(filtre).Select(x => new Models.LabeledValue(x.Code, x.Libelle)).ToList();
                    }
                    else if (inventaire.NumeroType == 5) {
                        inventaire.CodesExtensions = client.Channel.GetCodesIndisponibilitesTournage(filtre).Select(x => new Models.LabeledValue(x.Code, x.Libelle)).ToList();
                    }
                }
            }

            return CustomResult.JsonNetResult.NewResultToGet(inventaire);
        }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetEmptyInventoryItem(int type, string branche, string cible) {
            object item = null;
            IEnumerable list = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IReferentielPort>()) {
                switch (type) {
                    case 3:
                        item = new PersonneDesigneeIndispo() { IsNew = true };
                        list = client.Channel.GetCodeIndisponibilites(new Domain.Referentiel.CibleFiltre(branche, cible)).Select(x => new Models.LabeledValue(x.Code, x.Libelle));
                        break;
                    case 5:
                        item = new PersonneDesigneeIndispoTournage() { IsNew = true };
                        list = client.Channel.GetCodesIndisponibilitesTournage(new Domain.Referentiel.CibleFiltre(branche, cible)).Select(x => new Models.LabeledValue(x.Code, x.Libelle));
                        break;
                }
            }
            return CustomResult.JsonNetResult.NewResultToGet(new { item, list });
        }

        [ErrorHandler]
        public ActionResult LoadInventaires(string codeOffre, string version, string type, string codeAvn, string codeRisque, string codeObjet, string codeInven, string typeInven, bool fullScreen, string tabGuid, string branche, string cible, int codeFormule, string codeGarantie, string ecranProvenance, string modeNavig, bool isForceReadOnly) {
            var model = new Inventaire_MetaModel();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                var result = serviceContext.GetInventaire(codeOffre, Convert.ToInt32(version), type, codeAvn, ecranProvenance, Convert.ToInt32(codeRisque), Convert.ToInt32(codeObjet), codeFormule, codeGarantie, typeInven, Convert.ToInt32(codeInven), branche, cible, modeNavig.ParseCode<ModeConsultation>());
                if (result != null) {
                    model = (Inventaire_MetaModel)result;
                    var naturesLieu = result.NaturesLieu.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.NaturesLieu = naturesLieu;
                    var codesMat = result.CodesMateriel.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.CodesMateriel = codesMat;
                    var codesExtension = result.CodesExtension.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.CodesExtension = codesExtension;

                    var listePays = result.ListePays.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.ListePays = listePays;


                    var codesQualite = result.CodesQualite.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.CodesQualite = codesQualite;
                    var codesRenonce = result.CodesRenonce.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.CodesRenonce = codesRenonce;
                    var codesRsqLoc = result.CodesRsqLoc.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.CodesRsqLoc = codesRsqLoc;


                    model.FullScreen = fullScreen;
                    if (model.InventaireInfos == null)
                        model.InventaireInfos = new List<ModeleInventaireGridRow>();

                    model.InventaireInfos.Insert(0, new ModeleInventaireGridRow { InventaireType = model.InventaireType });
                    model.IsReadOnly = isForceReadOnly ? isForceReadOnly : GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn);
                }
            }

            return PartialView("InventaireGrid", model);
        }
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult SaveInventaire(Inventaire_MetaModel InvModel)
        {
        
         if (AllowUpdate && ModeConsultation.Standard == InvModel.ModeNavig.ParseCode<ModeConsultation>()) { 
                if (!string.IsNullOrEmpty(InvModel.Description)) {
                    InvModel.Description = InvModel.Description.Replace("\r\n", "<br>").Replace("\n", "<br>");
                }
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    client.Channel.SaveInventaire(InvModel.CodePolicePage, InvModel.VersionPolicePage, InvModel.TypePolicePage, InvModel.EcranProvenance, InvModel.CodeRisque , InvModel.CodeObjet , InvModel.Code.ToString(), InvModel.Descriptif , InvModel.Observations, InvModel.Valeur.ToString(), InvModel.UniteLst, InvModel.TypeLst, InvModel.TaxeLst, InvModel.ActiverReport, string.Empty, string.Empty, InvModel.CodeFormule, InvModel.CodeOption);
                }
            }

            if (!string.IsNullOrEmpty(InvModel.txtParamRedirect)) {
                var tabParamRedirect = InvModel.txtParamRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            
            Enum.TryParse(InvModel.EcranProvenance, out NomsInternesEcran provenance);
            bool isFromFormuleGarantie = provenance == NomsInternesEcran.FormuleGarantie;
            if (!isFromFormuleGarantie) {
                InvModel.AddParamValue += InvModel.IsIgnoreReadOnly ? ("||" + AlbParameterName.FORCEREADONLY + "|1") : string.Empty;
            }
            
           return RedirectToAction("Index", isFromFormuleGarantie ? "CreationFormuleGarantie" : "DetailsObjetRisque", new { id = AlbParameters.BuildFullId(
                new Folder(new[] { InvModel.CodePolicePage, InvModel.VersionPolicePage, InvModel.TypePolicePage }),
                isFromFormuleGarantie ? new[] { InvModel.CodeFormule, InvModel.CodeOption, InvModel.FormGen } : new[] { InvModel.CodeRisque, InvModel.CodeObjet },
                InvModel.TabGuid,
                InvModel.AddParamValue,
                InvModel.ModeNavig), returnHome = InvModel.txtSaveCancel,
                guidTab = InvModel.TabGuid
           });
        }

        [AlbAjaxRedirect]
        public void DeleteLineInventaire(string codeInven) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                serviceContext.DeleteLineInventaire(codeInven);
            }
        }

        [ErrorHandler]
        public string CalculBudget(string codeInventaire, string typeInventaire) {
            string toReturn;

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var screenClient = client.Channel;
                toReturn = screenClient.DetailsInventaireGetSumBudget(codeInventaire, typeInventaire);
            }

            return toReturn;
        }

        [HttpPost]
        [ErrorHandler]
        public void AddOrUpdateItem(Models.FormuleGarantie.IdentifiantGarantie garantieId, InventoryItem item) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>()) {
                client.Channel.AddOrUpdateGarantieInventaireItem(
                    garantieId.Affaire.Adapt<Domain.Affaire.AffaireId>(),
                    garantieId.NumOption,
                    garantieId.NumFormule,
                    garantieId.CodeBloc,
                    garantieId.Sequence,
                    TypeInventaireItem(item));
            }
        }

        [HttpPost]
        [ErrorHandler]
        public void DeleteItem(Models.FormuleGarantie.IdentifiantGarantie garantieId, InventoryItem item) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>()) {
                client.Channel.DeleteGarantieInventaireItem(
                    garantieId.Affaire.Adapt<Domain.Affaire.AffaireId>(),
                    garantieId.NumOption,
                    garantieId.NumFormule,
                    garantieId.CodeBloc,
                    garantieId.Sequence,
                    item.LineNumber);
            }
        }

        [HttpPost]
        [ErrorHandler]
        public void DeleteWholeInventaire(Models.FormuleGarantie.IdentifiantGarantie garantieId) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>()) {
                client.Channel.DeleteWholeGarantieInventaire(
                    garantieId.Affaire.Adapt<Domain.Affaire.AffaireId>(),
                    garantieId.NumOption,
                    garantieId.NumFormule,
                    garantieId.CodeBloc,
                    garantieId.Sequence);
            }
        }

        [HttpPost]
        [ErrorHandler]
        public void ValidateInventaireGarantie(Models.FormuleGarantie.IdentifiantGarantie garantieId, Inventaire inventaire) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>()) {
                client.Channel.SaveInventaire(
                    garantieId.Affaire.Adapt<Domain.Affaire.AffaireId>(),
                    garantieId.NumOption,
                    garantieId.NumFormule,
                    garantieId.CodeBloc,
                    garantieId.Sequence,
                    inventaire.Adapt<Albingia.Kheops.DTO.InventaireDto>());
            }
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string codeRisque, string codeObjet, string tabGuid, string modeNavig,
            string addParamType, string addParamValue, bool isForceReadOnly) {
            return RedirectToAction(job, cible, new { id = AlbParameters.BuildFullId(
                new Folder(new[] { codeOffre, version, type }),
                new[] { codeRisque, codeObjet },
                tabGuid,
                addParamValue + (isForceReadOnly ? "||" + AlbParameterName.FORCEREADONLY + "|1" : string.Empty),
                modeNavig)});
        }

        [ErrorHandler]
        [WhitespaceFilter]
        public PartialViewResult LoadInventaire(string id, string idGaran, bool isReadonly) {
            id = InitializeParams(id);
            model.IdGarantie = idGaran;
            model.IsReadOnly = isReadonly;
            LoadInfoPage(id);
            return PartialView("RisqueInventaireBody", model);
        }

        #region Private Methods
        private string GetKeyFormules(string suffixeKey) {
            return GetUser() + MvcApplication.SPLIT_CONST_HTML + suffixeKey;
        }

        private Domain.Inventaire.InventaireItem TypeInventaireItem(InventoryItem item) {
            if (item == null) {
                return null;
            }

            Domain.Inventaire.InventaireItem itemDto = null;
            if (item is PersonneDesigneeIndispoTournage) {
                itemDto = ((PersonneDesigneeIndispoTournage)item).Adapt<Domain.Inventaire.PersonneDesigneeIndispoTournage>();
            }
            else if (item is PersonneDesigneeIndispo) {
                itemDto = ((PersonneDesigneeIndispo)item).Adapt<Domain.Inventaire.PersonneDesigneeIndispo>();
            }
            else {
                itemDto = item.Adapt<Domain.Inventaire.InventaireItem>();
            }
            return itemDto;
        }

        private ModeleFormuleGarantieSave GetFormuleSaveFromCache(string key) {
            dynamic formulesGar;
            AlbSessionHelper.FormulesGarantiesSaveUtilisateurs.TryGetValue(key, out formulesGar);
            return formulesGar;
        }

        protected override void LoadInfoPage(string id) {
            var tId = id.Split('_');
            /******************/
            string codeObjet = string.Empty;
            string codeRisque = string.Empty;
            NomsInternesEcran provenance = NomsInternesEcran.RisqueObjet;

            string codeFile = string.Empty;
            switch (provenance) {
                case NomsInternesEcran.RisqueObjet:
                    codeFile = tId[0] + "_" + tId[1] + "_" + tId[3] + "_" + tId[6];
                    model.CodeRisque = tId[3];
                    model.CodeObjet = tId[4];
                    model.Type = tId[5];
                    model.Code = Convert.ToInt32(tId[6]);
                    model.CodeFormule = "0";
                    model.CodeGarantie = "";
                    model.EcranProvenance = NomsInternesEcran.RisqueObjet.ToString();
                    codeObjet = tId[4];
                    break;
            }
            
            RisqueDto currentRsq = null;
            DetailsRisqueGetResultDto infos = new DetailsRisqueGetResultDto();
            if (tId[2] == AlbConstantesMetiers.TYPE_OFFRE) {
                model.Offre = new Offre_MetaModel();

                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                    var CommonOffreClient = chan.Channel;
                    model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                }

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
                    var serviceContext = client.Channel;
                    model.Offre.Risques = serviceContext.ObtenirInfosRisquesInventaire(model.ModeNavig.ParseCode<ModeConsultation>(), tId[0], Convert.ToInt32(tId[1]), tId[2], model.NumAvenantPage);
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                    currentRsq = model.Offre.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString(CultureInfo.CurrentCulture)));
                }
            }
            else if (tId[2] == AlbConstantesMetiers.TYPE_CONTRAT) {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                    var CommonOffreClient = chan.Channel;
                    var infosBase = CommonOffreClient.LoadaffaireNouvelleBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
                    model.Contrat = new ContratDto() {
                        CodeContrat = infosBase.CodeOffre,
                        VersionContrat = Convert.ToInt64(infosBase.Version),
                        Type = infosBase.Type,
                        Branche = infosBase.Branche.Code,
                        BrancheLib = infosBase.Branche.Nom,
                        Cible = infosBase.Branche.Cible.Code,
                        CibleLib = infosBase.Branche.Cible.Nom,
                      //  CourtierGestionnaire = infosBase.CabinetGestionnaire.Code,
                        Descriptif = infosBase.Descriptif,
                      //  CodeInterlocuteur = infosBase.CabinetGestionnaire.Code,
                        NomInterlocuteur = infosBase?.CabinetGestionnaire?.Inspecteur, 
                        CodePreneurAssurance = Convert.ToInt32(infosBase.PreneurAssurance.Code),
                        NomPreneurAssurance = infosBase.PreneurAssurance.NomAssure,
                        PeriodiciteCode = infosBase.Periodicite,
                        // IndiceReference = infosBase.IndiceReference
                        Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                        Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                    };
                }

                using (var chann = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var serviceContext = chann.Channel;
                    model.Contrat.Risques = serviceContext.ObtenirInfosRisquesInventaire(model.ModeNavig.ParseCode<ModeConsultation>(), tId[0], Convert.ToInt32(tId[1]), tId[2], model.NumAvenantPage);
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                    currentRsq = model.Contrat.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString(CultureInfo.CurrentCulture)));
                }
            }

            model.PageTitle = "Description de l'inventaire";
            if (model.Offre != null) {
                model.CodePbr = model.Offre.Periodicite.Code;
            }
            if (model.Contrat != null) {
                model.CodePbmer = model.Contrat.TypePolice;
                model.CodePbr = model.Contrat.PeriodiciteCode;
            }
            if (model.Offre != null || model.Contrat != null) {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }
            
            SetArbreNavigation(model.EcranProvenance);
            model.Bandeau = null;
            SetBandeauNavigation(id);
            
            var branche = string.Empty;
            var cible = string.Empty;

            if (model.Offre != null) {
                if (model.Offre.DateEffetGarantie.HasValue) {
                    model.DateDebStr = model.Offre.DateEffetGarantie.Value.ToString().Split(' ')[0];
                    model.HeureDebStr = model.Offre.DateEffetGarantie.Value.ToString().Split(' ')[1].Split(':')[0];
                    model.MinuteDebStr = model.Offre.DateEffetGarantie.Value.ToString().Split(' ')[1].Split(':')[1];
                }
                if (model.Offre.DateFinEffetGarantie.HasValue) {
                    model.DateFinStr = model.Offre.DateFinEffetGarantie.Value.ToString().Split(' ')[0];
                    model.HeureFinStr = model.Offre.DateFinEffetGarantie.Value.ToString().Split(' ')[1].Split(':')[0];
                    model.MinuteFinStr = model.Offre.DateFinEffetGarantie.Value.ToString().Split(' ')[1].Split(':')[1];
                }
                branche = model.Offre.Branche.Code;

                if (model.EcranProvenance == NomsInternesEcran.FormuleGarantie.ToString()) {
                    var risque1 = model.Offre.Risques.FindAll(r => r.Code == Convert.ToInt32(codeRisque)).FirstOrDefault();
                    if (risque1 != null) {
                        if (string.IsNullOrEmpty(codeObjet))
                            cible = risque1.Cible.Code;
                        else {
                            var obj1 = risque1.Objets.FindAll(o => o.Code == Convert.ToInt32(codeObjet)).FirstOrDefault();
                            if (obj1 != null)
                                cible = obj1.Cible.Code;
                        }
                    }
                }
                else {
                    var risque1 = model.Offre.Risques.FindAll(r => r.Code == Convert.ToInt32(model.CodeRisque)).FirstOrDefault();
                    if (risque1 != null) {
                        var obj1 = risque1.Objets.FindAll(o => o.Code == Convert.ToInt32(model.CodeObjet)).FirstOrDefault();
                        if (obj1 != null)
                            cible = obj1.Cible.Code;
                    }
                }

            }
            else if (model.Contrat != null) {
                if (model.Contrat.DateEffetAnnee != 0 && model.Contrat.DateEffetMois != 0 && model.Contrat.DateEffetJour != 0) {
                    model.DateDebStr = new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour).ToString(CultureInfo.CurrentCulture).Split(' ')[0];
                    model.HeureDebStr = new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour).ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':')[0];
                    model.MinuteDebStr = new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour).ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':')[1];
                }
                if (model.Contrat.FinEffetAnnee != 0 && model.Contrat.FinEffetMois != 0 && model.Contrat.FinEffetJour != 0) {
                    var finEffetHour = AlbConvert.ConvertIntToTimeMinute(model.Contrat.FinEffetHeure);
                    model.DateFinStr = new DateTime(model.Contrat.FinEffetAnnee, model.Contrat.FinEffetMois, model.Contrat.FinEffetJour).ToString(CultureInfo.CurrentCulture).Split(' ')[0];
                    model.HeureFinStr = finEffetHour.HasValue ? finEffetHour.Value.Hours.ToString() : "23";
                    model.MinuteFinStr = finEffetHour.HasValue ? finEffetHour.Value.Minutes.ToString() : "59";
                }
                branche = model.Contrat.Branche;

                if (model.EcranProvenance == NomsInternesEcran.FormuleGarantie.ToString()) {
                    var risque = model.Contrat.Risques.FirstOrDefault(r => r.Code == Convert.ToInt32(codeRisque));
                    if (risque != null) {
                        if (string.IsNullOrEmpty(codeObjet))
                            cible = risque.Cible.Code;
                        else {
                            var obj1 = risque.Objets.FirstOrDefault(o => o.Code == Convert.ToInt32(codeObjet));
                            if (obj1 != null)
                                cible = obj1.Cible.Code;
                        }
                    }
                }
                else {
                    var risque = model.Contrat.Risques.FirstOrDefault(r => r.Code == Convert.ToInt32(model.CodeRisque));
                    if (risque != null) {
                        var objet = risque.Objets.FirstOrDefault(o => o.Code == Convert.ToInt32(model.CodeObjet));
                        if (objet != null)
                            cible = objet.Cible == null ? string.Empty : objet.Cible.Code;
                    }
                }
            }
            LoadDataInventaire(id, branche, cible);

            // Valeurs de l'objet
            if (currentRsq != null) {
                var currentObj = currentRsq.Objets.FirstOrDefault(x => x.Code == Convert.ToInt32(codeObjet.ToString(CultureInfo.CurrentCulture)));
                if (currentObj != null) {
                    model.ValeurObjet = currentObj.Valeur.HasValue ? currentObj.Valeur.Value.ToString() : string.Empty;
                    model.UniteObjet = model.UniteLsts != null && !string.IsNullOrEmpty(currentObj.Unite.Code) && model.UniteLsts.Find(elm => elm.Value == currentObj.Unite.Code) != null ? model.UniteLsts.Find(elm => elm.Value == currentObj.Unite.Code).Text : string.Empty;
                    model.TypeObjet = model.TypeLsts != null && !string.IsNullOrEmpty(currentObj.Type.Code) && model.TypeLsts.Find(elm => elm.Value == currentObj.Type.Code) != null ? model.TypeLsts.Find(elm => elm.Value == currentObj.Type.Code).Text : string.Empty;
                    model.ValeurObjetHT = model.TaxeLsts != null && !string.IsNullOrEmpty(currentObj.ValeurHT) && model.TaxeLsts.Find(elm => elm.Value == currentObj.ValeurHT) != null ? model.TaxeLsts.Find(elm => elm.Value == currentObj.ValeurHT).Text : string.Empty;
                }
            }

            this.model.IsAvnDisabled = IsAvnDisabled;
            this.model.IsModifHorsAvenant = this.IsModifHorsAvenant;
        }

        private void LoadDataInventaire(string id, string branche, string cible) {
            var model = new Inventaire_MetaModel();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                base.model.Branche = branche;
                base.model.Cible = cible;

                InventaireDto result = null;
                if (base.model.Offre != null)
                    result = serviceContext.GetInventaire(base.model.Offre.CodeOffre, base.model.Offre.Version.Value, base.model.Offre.Type, string.Empty, base.model.EcranProvenance, Convert.ToInt32(base.model.CodeRisque), Convert.ToInt32(base.model.CodeObjet), Convert.ToInt32(base.model.CodeFormule), base.model.CodeGarantie, base.model.Type, base.model.Code, branche, cible, base.model.ModeNavig.ParseCode<ModeConsultation>());
                else if (base.model.Contrat != null)
                    result = serviceContext.GetInventaire(base.model.Contrat.CodeContrat, int.Parse(base.model.Contrat.VersionContrat.ToString()), base.model.Contrat.Type, base.model.NumAvenantPage, base.model.EcranProvenance, Convert.ToInt32(base.model.CodeRisque), Convert.ToInt32(base.model.CodeObjet), Convert.ToInt32(base.model.CodeFormule), base.model.CodeGarantie, base.model.Type, base.model.Code, branche, cible, base.model.ModeNavig.ParseCode<ModeConsultation>());
                if (result == null)
                    return;
                model = ((Inventaire_MetaModel)result);

                base.model.ActiverReport = result.ReportVal == "O";
                base.model.Valeur = model.Valeur;
                base.model.UniteLst = model.UniteLst;
                base.model.TypeLst = model.TypeLst;
                base.model.TaxeLst = model.TaxeLst;
                base.model.Description = model.Description;
                base.model.Descriptif = model.Descriptif;
                base.model.InventaireType = model.InventaireType;
                base.model.InventaireInfos = model.InventaireInfos ?? new List<ModeleInventaireGridRow>();

                base.model.InventaireInfos.Insert(0, new ModeleInventaireGridRow { InventaireType = base.model.InventaireType });


                var unites = result.Unites.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (!string.IsNullOrEmpty(base.model.UniteLst)) {
                    var sItem = unites.FirstOrDefault(x => x.Value == base.model.UniteLst);
                    if (sItem != null) {
                        sItem.Selected = true;
                    }
                }
                base.model.UniteLsts = unites;

                var types = result.Types.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (!string.IsNullOrEmpty(base.model.TypeLst)) {
                    var sItem = unites.FirstOrDefault(x => x.Value == base.model.TypeLst);
                    if (sItem != null) {
                        sItem.Selected = true;
                    }
                }
                base.model.TypeLsts = types;
                var valeursHT = new List<AlbSelectListItem>
                                        {
                                            new AlbSelectListItem {Text = "H - HT", Value = "H", Selected = false, Title = "H - HT"},
                                            new AlbSelectListItem {Text = "T - TTC", Value = "T", Selected = false, Title = "T - TTC"}
                                        };
                base.model.TaxeLsts = valeursHT;

                var naturesLieu = result.NaturesLieu.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                base.model.NaturesLieu = naturesLieu;

                var listePays = result.ListePays.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                base.model.ListePays = listePays;

                var codesMat = result.CodesMateriel.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                base.model.CodesMateriel = codesMat;
                var codesExtension = result.CodesExtension.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                base.model.CodesExtension = codesExtension;
                base.model.InventaireInfos.ForEach(i => {
                    i.NaturesLieu = naturesLieu;
                    i.CodesMateriel = codesMat;
                });

                var codesQualite = result.CodesQualite.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                base.model.CodesQualite = codesQualite;
                var codesRenonce = result.CodesRenonce.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                base.model.CodesRenonce = codesRenonce;
                var codesRsqLoc = result.CodesRsqLoc.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                base.model.CodesRsqLoc = codesRsqLoc;
            }
        }

        private string CheckLineInventaire(ModeleInventaireGridRow inventaireLine) {
            if (inventaireLine.InventaireType == 1 || inventaireLine.InventaireType == 2 || inventaireLine.InventaireType == 6 || inventaireLine.InventaireType == 7) {
                //if ((inventaireLine.AccidentSeul || inventaireLine.AvantProd || inventaireLine.CapitalDeces != 0 || inventaireLine.CapitalIP != 0
                //    || inventaireLine.CodePostal != 0 || !string.IsNullOrEmpty(inventaireLine.DateDebStr) || !string.IsNullOrEmpty(inventaireLine.DateFinStr)
                //    || !string.IsNullOrEmpty(inventaireLine.DateNaissanceStr) || !string.IsNullOrEmpty(inventaireLine.Fonction) || !string.IsNullOrEmpty(inventaireLine.HeureDebStr)
                //    || !string.IsNullOrEmpty(inventaireLine.HeureFinStr) || !string.IsNullOrEmpty(inventaireLine.Lieu) || inventaireLine.Montant != 0
                //    || inventaireLine.NbEvenement != 0 || inventaireLine.NbPers != 0 || !string.IsNullOrEmpty(inventaireLine.Nom)
                //    || !string.IsNullOrEmpty(inventaireLine.Prenom) || !string.IsNullOrEmpty(inventaireLine.Ville)) && string.IsNullOrEmpty(inventaireLine.Designation))
                //{
                //    throw new AlbFoncException("La désignation doit être renseignée.");
                //    //return "La désignation doit être renseignée.";
                //}

                if (string.IsNullOrEmpty(inventaireLine.Designation))
                    throw new AlbFoncException("La désignation doit être renseignée.");

            }

            if (inventaireLine.InventaireType == 3 || inventaireLine.InventaireType == 4 || inventaireLine.InventaireType == 5) {
                //if (!string.IsNullOrEmpty(inventaireLine.Nom) || !string.IsNullOrEmpty(inventaireLine.Prenom) || !string.IsNullOrEmpty(inventaireLine.Fonction)
                //        || !string.IsNullOrEmpty(inventaireLine.DateNaissanceStr))
                if (string.IsNullOrEmpty(inventaireLine.Nom)) {
                    throw new AlbFoncException("Le nom doit être renseigné.");
                }
            }
            DateTime? dateDeb = null;
            if (inventaireLine.DateDeb.HasValue) {
                if (inventaireLine.HeureDeb.HasValue) {
                    dateDeb = new DateTime(inventaireLine.DateDeb.Value.Year, inventaireLine.DateDeb.Value.Month, inventaireLine.DateDeb.Value.Day,
                        inventaireLine.HeureDeb.Value.Hours, inventaireLine.HeureDeb.Value.Minutes, 0);
                }
                else {
                    dateDeb = new DateTime(inventaireLine.DateDeb.Value.Year, inventaireLine.DateDeb.Value.Month, inventaireLine.DateDeb.Value.Day, 0, 0, 0);
                }
            }

            DateTime? dateFin = null;
            if (inventaireLine.DateFin.HasValue) {
                if (inventaireLine.HeureFin.HasValue) {
                    dateFin = new DateTime(inventaireLine.DateFin.Value.Year, inventaireLine.DateFin.Value.Month, inventaireLine.DateFin.Value.Day,
                        inventaireLine.HeureFin.Value.Hours, inventaireLine.HeureFin.Value.Minutes, 0);
                }
                else {
                    dateFin = new DateTime(inventaireLine.DateFin.Value.Year, inventaireLine.DateFin.Value.Month, inventaireLine.DateFin.Value.Day, 0, 0, 0);
                }
            }

            if (dateDeb > dateFin) {
                throw new AlbFoncException("Les dates d'entrée et de sortie ne sont pas cohérentes.");
                //return "Les dates d'entrée et de sortie ne sont pas cohérentes.";
            }

            return string.Empty;
        }
        private void SetBandeauNavigation(string id) {

            if (model.AfficherBandeau) {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Offre != null) {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.Navigation = new Navigation_MetaModel {
                        Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null) {
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt) {
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
                    model.Navigation = new Navigation_MetaModel {
                        Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }
        private void SetArbreNavigation(string provenance) {
            if (model.Offre != null) {
                //Affichage de la navigation latérale en arboresence
                if (provenance == NomsInternesEcran.RisqueObjet.ToString()) {
                    model.NavigationArbre = GetNavigationArbre("Risque", codeRisque: Convert.ToInt32(model.CodeRisque));
                }
                else {
                    this.model.NavigationArbre = GetNavigationArbre(
                       etape: "Formule",
                       codeFormule: this.model.CodeFormule.ParseInt().Value,
                       numOption: this.model.CodeOption.ParseInt().Value);
                }
            }
            else if (model.Contrat != null) {
                if (provenance == NomsInternesEcran.RisqueObjet.ToString()) {
                    model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Risque", codeRisque: Convert.ToInt32(model.CodeRisque));
                }
                else {
                    model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Formule", codeFormule: Convert.ToInt32(model.CodeFormule));
                }
            }
        }

        #endregion
    }
    class DecimalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal) || objectType == typeof(decimal?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Float || token.Type == JTokenType.Integer)
            {
                return token.ToObject<decimal>();
            }
            if (token.Type == JTokenType.String)
            {
                // customize this to suit your needs
                return Decimal.Parse(token.ToString(),
                       System.Globalization.CultureInfo.GetCultureInfo("fr-FR"));
            }
            if (token.Type == JTokenType.Null && objectType == typeof(decimal?))
            {
                return null;
            }
            throw new JsonSerializationException("Unexpected token type: " +
                                                  token.Type.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
