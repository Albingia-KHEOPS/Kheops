using Albingia.Kheops.Common;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.CustomResult;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.FormuleGarantie;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using EmitMapper;
using OP.DataAccess;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Condition;
using OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie;
using OP.WSAS400.DTO.Engagement;
using OPServiceContract;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class ConditionsGarantieController : RemiseEnVigueurController<ModeleConditionsGarantiePage> {
        private readonly CacheConditions cacheConditions;

        public ConditionsGarantieController(CacheConditions cacheConditions) {
            this.cacheConditions = cacheConditions;
        }

        protected override bool GetIsReadOnly(string guid, string currentFolder, string numAvenant = "0", bool isPopup = false, string modeAvenant = "") {
            bool rdo = base.GetIsReadOnly(guid, currentFolder, numAvenant, isPopup, modeAvenant);
            if (Model.TypePolicePage == AlbConstantesMetiers.TYPE_OFFRE || rdo || Model.CodePolicePage.IsEmptyOrNull() || this.model.CodeFormule.IsEmptyOrNull()) {
                return rdo;
            }

            string keyConditions = GetKeyConditions(new IdentifiantOption {
                Affaire = new Models.Affaire { CodeOffre = Model.CodePolicePage, Version = int.Parse(Model.VersionPolicePage), Type = AlbConstantesMetiers.TYPE_CONTRAT },
                NumFormule = int.Parse(this.model.CodeFormule),
                NumOption = int.Parse(this.model.CodeOption)
            });
            var conditions = GetConditionsGaranties(keyConditions, Model.NumAvenantPage, Model.ModeNavig, false);
            return conditions is null ? rdo : conditions.IsAvnDisabled;
        }

        [WhitespaceFilter]
        
        [ErrorHandler]
        public ActionResult Index(string id) {
            this.model.NomEcran = NomsInternesEcran.ConditionsGaranties;
            id = InitializeParams(id);

            string[] tId = id.Split('_');
            string keyConditions = GetKeyConditions(new IdentifiantOption {
                Affaire = new Models.Affaire(id),
                NumFormule = int.Parse(tId[3]),
                NumOption = int.Parse(tId[4])
            });

            if (tId.Length >= 6) {
                model.CodeRisque = tId[5];
            }

            DeleteConditionFromCache(keyConditions);
            LoadInfoPage(id);
            return View(model);
        }

        /// <summary>
        /// Sauvegarde la ligne de condition garantie
        /// </summary>
        [ErrorHandler]
        public ActionResult SaveGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, string codeCondition, string objRow, string modeNavig, bool isReadOnly) {
            ModeleConditionsGarantie modelConditionaGarantie = new ModeleConditionsGarantie();
            JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleConditionsLigneGarantie>.GetSerializer();
            var conditionsLigneGarantie = serialiser.ConvertToType<List<ModeleConditionsLigneGarantie>>(serialiser.DeserializeObject(objRow));
            if (CheckRowGarantie(conditionsLigneGarantie[0])) {
                this.model.ModeNavig = modeNavig;
                string keyConditions = GetKeyConditions(new IdentifiantOption {
                    Affaire = new Models.Affaire { CodeOffre = codeOffre, Version = int.Parse(version), Type = type },
                    NumFormule = int.Parse(codeFormule),
                    NumOption = int.Parse(codeOption)
                });
                ConditionRisqueGarantieGetResultDto conditions = GetConditionsGaranties(keyConditions, codeAvn, modeNavig, isReadOnly, true);
                if (conditions == null) {
                    return null;
                }

                EnsembleGarantieDto garantie = conditions.LstEnsembleLigne.ToList().FindAll(l => l.Id == codeGarantie).FirstOrDefault();
                var isSaved = false;
                var newCodeTarif = 0;
                if (garantie != null) {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                        try {
                            newCodeTarif = client.Channel.SaveCondition(ModeleConditionsLigneGarantie.LoadDto(conditionsLigneGarantie[0]), codeAvn);
                            if (newCodeTarif > 0)
                                isSaved = true;
                            else
                                throw new Exception();
                        }
                        catch (Exception ex) {
                            new AlbTechException(ex);
                        }
                    }
                    garantie.LstLigneGarantie.FindAll(g => g.Code.Contains(string.Format("_{0}", codeGarantie))).ForEach(c => {
                        c.AssietteUnite = conditionsLigneGarantie[0].AssietteUnite;
                        c.AssietteValeur = conditionsLigneGarantie[0].AssietteValeur;
                        c.AssietteType = conditionsLigneGarantie[0].AssietteType;
                        c.MAJ = isSaved ? string.Empty : string.IsNullOrEmpty(c.MAJ) ? AlbConstantesMetiers.Traitement.UpdateCondition.AsCode() : c.MAJ;

                    });

                    string codeTarif = string.Format("{0}_{1}", codeCondition, codeGarantie);
                    if (newCodeTarif > 0) {
                        codeTarif = string.Format("{0}_{1}", newCodeTarif, codeGarantie);
                    }
                    var condition = garantie.LstLigneGarantie.FindAll(g => g.Code == conditionsLigneGarantie[0].Code).FirstOrDefault();
                    if (condition != null) {
                        condition.Code = codeTarif;
                    }
                    LigneGarantieDto majTarif = SaveTarif(garantie.LstLigneGarantie.FindAll(g => g.Code == codeTarif).FirstOrDefault(), conditionsLigneGarantie[0]);

                    modelConditionaGarantie = (ModeleConditionsGarantie)garantie;

                    modelConditionaGarantie.LstLigneGarantie = new List<ModeleConditionsLigneGarantie> {
                        (ModeleConditionsLigneGarantie) majTarif
                    };

                    SetConditionFromCache(keyConditions, conditions);
                }

                modelConditionaGarantie.LCIUnites = garantie.LCIUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelConditionaGarantie.LCITypes = garantie.LCITypes.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelConditionaGarantie.FranchiseUnites = garantie.FranchiseUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelConditionaGarantie.FranchiseTypes = garantie.FranchiseTypes.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelConditionaGarantie.AssietteUnites = garantie.AssietteUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelConditionaGarantie.AssietteTypes = garantie.AssietteTypes.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelConditionaGarantie.TauxForfaitHTUnites = garantie.TauxForfaitHTUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();

                return PartialView("LigneCondition", modelConditionaGarantie);
            }
            else {
                throw new AlbFoncException("La ligne de condition n'est pas valide : format non valide");
            }
        }

        [HandleJsonError]
        [HttpPost]
        public void SaveConditionsGarantie(Guid tab, int formule, int option, ModeleConditionsLigneGarantie ligneConditions, bool firstUpdate = false) {
            var acces = MvcApplication.ListeAccesAffaires.FirstOrDefault(a => a.TabGuid == tab);
            if (acces is null) {
                throw new BusinessValidationException(new ValidationError("Invalid Tab Guid", nameof(tab)));
            }
            if (!CheckRowGarantie(ligneConditions)) {
                throw new BusinessValidationException(new ValidationError("La ligne de condition n'est pas valide : format non valide"));
            }
            var conditonsDto = BuildConditionGarantie(ligneConditions);
            if (firstUpdate) {
                using (var clientAffaire = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffairePort>())
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFormulePort>()) {
                    AffaireId affaireId = clientAffaire.Channel.GetAffaireId(acces.Code, acces.Version, acces.Avenant);
                    this.cacheConditions.SaveConditionsGarantie(acces, conditonsDto, client.Channel.GetConditionsGaranties(affaireId, option, formule));
                }
            }
            else {
                this.cacheConditions.SaveConditionsGarantie(acces, conditonsDto);
            }
        }

        [HttpGet]
        public JsonResult CancelConditionsGarantie(Guid tab, int idGarantie) {
            var acces = MvcApplication.ListeAccesAffaires.FirstOrDefault(a => a.TabGuid == tab);
            if (acces is null) {
                throw new BusinessValidationException(new ValidationError("Invalid Tab Guid", nameof(tab)));
            }
            var conditions = this.cacheConditions.RollbackCondition(acces, idGarantie);
            return JsonNetResult.NewResultToGet(conditions);
        }

        [ErrorHandler]
        public ActionResult CancelGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, string codeCondition,
            string oldFRHExpr, string oldLCIExpr, string modeNavig, bool isReadOnly) {
            ModeleConditionsGarantie modelCondGar = new ModeleConditionsGarantie();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                this.model.ModeNavig = modeNavig;
                string keyConditions = GetKeyConditions(new IdentifiantOption {
                    Affaire = new Models.Affaire { CodeOffre = codeOffre, Version = int.Parse(version), Type = type },
                    NumFormule = int.Parse(codeFormule),
                    NumOption = int.Parse(codeOption)
                });
                ConditionRisqueGarantieGetResultDto conditions = GetConditionsGaranties(keyConditions, codeAvn, modeNavig, isReadOnly, true);
                if (conditions == null)
                    return null;
                EnsembleGarantieDto garantie = conditions.LstEnsembleLigne.ToList().FindAll(l => l.Id == codeGarantie).FirstOrDefault();

                var oldGarantie = serviceContext.CancelGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, codeGarantie, codeCondition, oldFRHExpr, oldLCIExpr, base.model.ModeNavig.ParseCode<ModeConsultation>(), isReadOnly);
                if (oldGarantie != null) {
                    modelCondGar = (ModeleConditionsGarantie)oldGarantie;
                }

                modelCondGar.LCIUnites = garantie.LCIUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelCondGar.LCITypes = garantie.LCITypes.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelCondGar.FranchiseUnites = garantie.FranchiseUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelCondGar.FranchiseTypes = garantie.FranchiseTypes.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelCondGar.AssietteUnites = garantie.AssietteUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelCondGar.AssietteTypes = garantie.AssietteTypes.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                modelCondGar.TauxForfaitHTUnites = garantie.TauxForfaitHTUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();

            }
            return PartialView("LigneCondition", modelCondGar);
        }

        /// <summary>
        /// Sauvegarde l'expression complexe choisie pour la condition
        /// </summary>
        [ErrorHandler]
        public void SaveExprComplexe(string typeExpr, string codeCondition, string codeExpr, string codeFormule, string codeOption) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                serviceContext.AffectExpressionCondition(typeExpr, codeCondition, codeExpr);
            }
        }
        [ErrorHandler]
        public void SaveExprComplexeGenRsq(string typeExpr, string typeAppel, string codeExpr, string codeFormule, string codeOption, string codeOffre, string version, string type, string codeAvn, string codeRisque, string unite, string modeNavig) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                AlbConstantesMetiers.TypeAppel arg_typeAppel = AlbConstantesMetiers.TypeAppel.Generale;
                AlbConstantesMetiers.ExpressionComplexe arg_typeVue = AlbConstantesMetiers.ExpressionComplexe.Franchise;
                Enum.TryParse(typeExpr, out arg_typeVue);
                Enum.TryParse(typeAppel, out arg_typeAppel);
                serviceContext.EnregistrementExpCompGeneraleRisque(codeOffre, version, type, codeAvn, codeFormule, codeOption, codeRisque, codeExpr, unite, arg_typeVue, arg_typeAppel, modeNavig.ParseCode<ModeConsultation>());
            }
        }

        /// <summary>
        /// Duplique la 1ère condition d'une garantie
        /// </summary>
        [ErrorHandler]
        public string DuplicateCondition(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, string codeCondition, string modeNavig, bool isReadOnly) {
            var result = string.Empty;
            model.ModeNavig = modeNavig;
            string keyConditions = GetKeyConditions(new IdentifiantOption {
                Affaire = new Models.Affaire { CodeOffre = codeOffre, Version = int.Parse(version), Type = type },
                NumFormule = int.Parse(codeFormule),
                NumOption = int.Parse(codeOption)
            });
            ConditionRisqueGarantieGetResultDto conditions = GetConditionsGaranties(keyConditions, codeAvn, modeNavig, isReadOnly, true);
            if (conditions == null)
                throw new AlbFoncException(string.Format("{0}-ERRORCACHE", User));

            EnsembleGarantieDto garantie = conditions.LstEnsembleLigne.ToList().FindAll(l => l.Id == codeGarantie).FirstOrDefault();
            if (garantie != null) {
                int maxNumTar = Convert.ToInt32(garantie.LstLigneGarantie.ToList().FindAll(g => g.Code.Contains("_" + codeGarantie)).Max(g => g.NumeroTarif)) + 1;
                LigneGarantieDto firstTarif = garantie.LstLigneGarantie.ToList().FindAll(g => g.Code == string.Format("{0}_{1}", codeCondition, codeGarantie) && g.NumeroTarif == "1").FirstOrDefault();
                if (firstTarif != null) {
                    garantie.LstLigneGarantie.Add(CopyTarif(firstTarif, maxNumTar, codeGarantie));

                    SetConditionFromCache(keyConditions, conditions);
                    result = maxNumTar.ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// Supprime une condition de garantie
        /// </summary>
        [ErrorHandler]
        public string DeleteCondition(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, string codeCondition, string modeNavig, bool isReadOnly) {
            model.ModeNavig = modeNavig;
            string keyConditions = GetKeyConditions(new IdentifiantOption {
                Affaire = new Models.Affaire { CodeOffre = codeOffre, Version = int.Parse(version), Type = type },
                NumFormule = int.Parse(codeFormule),
                NumOption = int.Parse(codeOption)
            });
            ConditionRisqueGarantieGetResultDto conditions = GetConditionsGaranties(keyConditions, codeAvn, modeNavig, isReadOnly, true);
            if (conditions == null)
                return "KO";


            EnsembleGarantieDto garantie = conditions.LstEnsembleLigne.ToList().FindAll(l => l.Id == codeGarantie).FirstOrDefault();
            if (garantie != null) {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    var serviceContext = client.Channel;
                    serviceContext.DeleteCondition(codeCondition);
                    garantie.LstLigneGarantie.RemoveAll(g => g.Code == string.Format("{0}_{1}", codeCondition, codeGarantie));
                    SetConditionFromCache(keyConditions, conditions);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Ouvre la div flottante suivant le type qu'on lui donne (LCI ou Franchise)
        /// </summary>
        [ErrorHandler]
        public ActionResult OpenExprComplexe(string codeOffre, string version, string type, string typeExpr, bool isReadOnly, bool isGenRsq, string typeAppel, string modeNavig) {
            ModeleConditionsExprComplexe model;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                var result = serviceContext.RecuperationConditionComplexe(codeOffre, version, type, typeExpr);
                model = (ModeleConditionsExprComplexe)result;
                model.IsReadOnly = isReadOnly;
                model.isGenRsq = isGenRsq;
            }
            return PartialView("ConditionsExpressionComplexe", model);
        }

        [ErrorHandler]
        public ActionResult TrierExprComplexe(string codeOffre, string version, string type, string typeExpr, string typeAppel, string modeNavig, string colTri, string imgTri) {
            var model = new ModeleConditionsExprComplexe {
                Expressions = new List<ModeleConditionsExprComplexeDetails>()
            };

            var newExpression = new List<ModeleConditionsExprComplexeDetails>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                var result = serviceContext.RecuperationConditionComplexe(codeOffre, version, type, typeExpr);
                result.Expressions.ForEach(elm => newExpression.Add((ModeleConditionsExprComplexeDetails)elm));

                if (!string.IsNullOrEmpty(colTri)) {
                    switch (colTri) {
                        case "CodeExpr":
                            if (imgTri == "tri_asc") {
                                newExpression = newExpression.OrderByDescending(elm => elm.Code).ToList();
                            }
                            else {
                                newExpression = newExpression.OrderBy(elm => elm.Code).ToList();
                            }
                            break;
                        case "LibelleExpr":
                            if (imgTri == "tri_asc") {
                                newExpression = newExpression.OrderByDescending(elm => elm.Libelle).ToList();
                            }
                            else {
                                newExpression = newExpression.OrderBy(elm => elm.Libelle).ToList();
                            }
                            break;
                    }
                }
            }

            model.Expressions = newExpression;

            return PartialView("ConditionsExpressionComplexeLignes", model);
        }

        /// <summary>
        /// Supprime l'expression sélectionnée
        /// </summary>
        [ErrorHandler]
        public void DeleteExpression(string typeExpr, string typeAppel, string codeExpr) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                serviceContext.SuppressionExpression(typeExpr, typeAppel, codeExpr, "");
            }
        }

        /// <summary>
        /// Affiche les détails de l'expression sélectionnée
        /// </summary>
        [ErrorHandler]
        public ActionResult DisplayDetails(string codeOffre, string version, string type, string codeAvn, string typeExpr, string codeExpr, bool isReadOnly, string isModif, string modeNavig) {
            ModeleConditionsExprComplexeDetails model = new ModeleConditionsExprComplexeDetails { LstLigneGarantie = new List<ModeleConditionsLigneGarantie>() };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;

                var result = serviceContext.RecuperationDetailComplexe(codeOffre, version, type, codeAvn, codeExpr, typeExpr, modeNavig.ParseCode<ModeConsultation>());

                if (!string.IsNullOrEmpty(codeOffre)) {
                    result.LstLigneGarantie.ToList().ForEach(elem => model.LstLigneGarantie.Add((ModeleConditionsLigneGarantie)elem));
                    model.Libelle = result.Libelle;
                    model.Code = result.Code;
                    model.Descriptif = result.Descriptif;
                    model.Id = result.Id;
                }

                foreach (var item in model.LstLigneGarantie) {
                    if (item.LCIValeur.IsEmptyOrNull()
                        || double.Parse(item.LCIValeur, AlbConvert.AppCulture) == 0D && item.LCIUnite.IsEmptyOrNull() && item.LCIType.IsEmptyOrNull()) {
                        item.LCIValeur = string.Empty;
                    }

                    if (item.FranchiseValeur.IsEmptyOrNull()
                        || double.Parse(item.FranchiseValeur, AlbConvert.AppCulture) == 0D && item.FranchiseUnite.IsEmptyOrNull() && item.FranchiseType.IsEmptyOrNull()) {
                        item.FranchiseValeur = string.Empty;
                    }
                }

                model.Type = result.Type;

                model.UnitesLCINew = result.UnitesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "CPX").ToList();
                model.UnitesFranchiseNew = result.UnitesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "CPX").ToList();
                model.UnitesConcurrence = result.UnitesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "CPX").ToList();

                model.TypesLCINew = result.TypesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
                model.TypesFranchiseNew = result.TypesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
                model.TypesConcurrence = result.TypesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();

                model.IsReadOnly = isReadOnly || isModif != "O";
            }
            return PartialView("ConditionsExpressionComplexeDetails", model);
        }

        /// <summary>
        /// Sauvegarde les détails de l'expression sélectionnée
        /// </summary>
        [ErrorHandler]
        public string SaveDetails(string codeOffre, string version, string type, string typeExpr, string codeExpr, string libelle, string description, string obj) {
            string toReturn = string.Empty;
            int? idExpr;
            AlbNullableInt.TryParse(codeExpr, out idExpr);
            JavaScriptSerializer djsDeserializer = AlbJsExtendConverter<ModeleConditionsLigneGarantie>.GetSerializer();
            var listDataExprDetail = new ModeleConditionsExprComplexeDetails();
            bool lineNotNull = false;
            if (obj != "" && obj != "]") {
                lineNotNull = true;
                listDataExprDetail.LstLigneGarantie = djsDeserializer.ConvertToType<List<ModeleConditionsLigneGarantie>>(djsDeserializer.DeserializeObject(obj));
                //CheckDataExpr(listDataExprDetail.LstLigneGarantie, typeExpr);
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                toReturn = serviceContext.EnregistrementConditionComplexe(lineNotNull ? ModeleConditionsExprComplexeDetails.LoadDto(listDataExprDetail) : null, type, typeExpr, codeOffre, version, idExpr, libelle, description);
            }
            return toReturn;
        }

        /// <summary>
        /// Supprime le détail sélectionné
        /// </summary>
        [ErrorHandler]
        public void DeleteDetails(string typeExpr, string codeDetail) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                serviceContext.SupressionDetail(typeExpr, codeDetail);
            }
        }

        /// <summary>
        /// Charge la liste des conditions de garantie pour les modes plein écran
        /// et écran normal.
        /// </summary>
        [WhitespaceFilter]
        [ErrorHandler]
        public ActionResult LoadConditions(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, bool fullScreen, bool isReadOnly, string modeNavig) {
            var modelInfos = new ModeleConditionsInfosCondition();
            this.model.ModeNavig = modeNavig;
            this.model.CodeFormule = codeFormule;
            this.model.CodeOption = codeOption;
            string keyConditions = GetKeyConditions(new IdentifiantOption {
                Affaire = new Models.Affaire { CodeOffre = codeOffre, Version = int.Parse(version), Type = type },
                NumFormule = int.Parse(codeFormule),
                NumOption = int.Parse(codeOption)
            });
            ConditionRisqueGarantieGetResultDto result = GetConditionsGaranties(keyConditions, codeAvn, modeNavig, isReadOnly, true);
            if (result == null)
                return null;

            modelInfos = LoadInfoCondition(result, type, fullScreen);
            modelInfos.FullScreen = fullScreen;
            modelInfos.IsReadOnly = isReadOnly;
            this.model.InformationsCondition = modelInfos;
            return PartialView("TableauConditions", this.model);
        }

        /// <summary>
        /// Charge les détails de la garantie
        /// </summary>
        [ErrorHandler]
        public ActionResult DetailsGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, string modeNavig, bool isReadonly = false) {
            return RedirectToAction("LoadDetailsGarantie", "CreationFormuleGarantie", new { codeOffre = codeOffre, version = version, type = type, codeAvn = codeAvn, codeFormule = codeFormule, codeOption = codeOption, codeGarantie = codeGarantie, modeNavig = modeNavig, isReadonly = true });
        }

        /// <summary>
        /// Charge la page de redirection
        /// </summary>
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string codeFormule, string codeOption, string tabGuid, string modeNavig, string addParamType, string addParamValue) {
            if (cible == "CreationFormuleGarantie") {
                cible = job == "Index" ? "FormuleGarantie" : cible;
            }

            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + codeFormule + "_" + codeOption + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
        }

        [HandleJsonError]
        [AlbAjaxRedirect]
        public string ValidationConditions(string argCodeOffre, string argType, string argVersion, string codeAvn, string argCodeFormule, string argCodeOption
                , string argValeurLCI, string argUniteLCI, string argTypeLCI, string argIndexeLCI
                , string argValeurFranchise, string argUniteFranchise, string argTypeFranchise, string argIndexeFranchise
                , string argExpAssiette, string argLettre, string argLibelle, string tabGuid, string saveCancel, string paramRedirect
                , string argValeurLCIRisque, string argUniteLCIRisque, string argTypeLCIRisque
                , string argValeurFranchiseRisque, string argUniteFranchiseRisque, string argTypeFranchiseRisque,
                 string argLienCpxLCI, string argLienCpxFranchise, string argLienCpxLCIRisque, string argLienCpxFranchiseRisque, string modeNavig,
                 decimal? primeGareatTheorique, decimal? primeGareat,
                 bool isReadOnly) {
            this.model.ModeNavig = modeNavig;
            this.model.CodeFormule = argCodeFormule;
            this.model.CodeOption = argCodeOption;
            string keyConditions = GetKeyConditions(new IdentifiantOption {
                Affaire = new Models.Affaire { CodeOffre = argCodeOffre, Version = int.Parse(argVersion), Type = argType },
                NumFormule = int.Parse(argCodeFormule),
                NumOption = int.Parse(argCodeOption)
            });
            ConditionRisqueGarantieGetResultDto result = GetConditionsGaranties(keyConditions, codeAvn, modeNavig, isReadOnly, true);
            if (result == null) {
                return "KO";
            }

            if (result.LstEnsembleLigne.Any(x => x.IsAttentatGareat)) {
                var gareat = result.LstEnsembleLigne.FirstOrDefault(l => l.Code == Albingia.Kheops.OP.Domain.Formule.Garantie.CodeGareatAttent && l.LBloc == Albingia.Kheops.OP.Domain.Formule.Garantie.CodeGareat);
                if (gareat != null) {
                    var tarif = gareat.LstLigneGarantie.First();
                    if (primeGareatTheorique > decimal.Zero) {
                        tarif.TauxForfaitHTValeur = primeGareat.ToString();
                        tarif.PrimeValeur = new Valeurs {
                            ValeurActualise = primeGareat ?? primeGareatTheorique.Value,
                            ValeurOrigine = primeGareat ?? primeGareatTheorique.Value
                        };
                    }
                    else {
                        tarif.TauxForfaitHTValeur = string.Empty;
                        tarif.PrimeValeur = new Valeurs();
                    }
                    tarif.TauxForfaitHTUnite = "D";
                    tarif.MAJ = "U";
                }
            }

            foreach (var tarifObligatoire in result.LstEnsembleLigne.Where(x =>
                x.LstLigneGarantie.First().TauxForfaitHTObligatoire.AsBoolean().Value
                || x.LstLigneGarantie.First().AssietteObligatoire.AsBoolean().Value
                || x.LstLigneGarantie.First().LCIObligatoire.AsBoolean().Value
                || x.LstLigneGarantie.First().FranchiseObligatoire.AsBoolean().Value)) {
                var ligne = tarifObligatoire.LstLigneGarantie.First();
                ligne.MAJ = "U";
            }

            var infosContrat = new ConditionRisqueGarantieGetResultDto {
                LCI = argValeurLCI,
                UniteLCI = argUniteLCI,
                TypeLCI = argTypeLCI,
                IsIndexeLCI = !string.IsNullOrEmpty(argIndexeLCI) && bool.Parse(argIndexeLCI),
                LienComplexeLCIGenerale = argLienCpxLCI,

                Franchise = argValeurFranchise,
                UniteFranchise = argUniteFranchise,
                TypeFranchise = argTypeFranchise,
                IsIndexeFranchise = !string.IsNullOrEmpty(argIndexeFranchise) && bool.Parse(argIndexeFranchise),
                LienComplexeFranchiseGenerale = argLienCpxFranchise,

                LCIRisque = argValeurLCIRisque,
                UniteLCIRisque = argUniteLCIRisque,
                TypeLCIRisque = argTypeLCIRisque,
                LienComplexeLCIRisque = argLienCpxLCIRisque,

                FranchiseRisque = argValeurFranchiseRisque,
                UniteFranchiseRisque = argUniteFranchiseRisque,
                TypeFranchiseRisque = argTypeFranchiseRisque,
                LienComplexeFranchiseRisque = argLienCpxFranchiseRisque,

                ExpAssiette = argExpAssiette,

                LstEnsembleLigne = result.LstEnsembleLigne.Where(x => x.LstLigneGarantie.Any(y => !string.IsNullOrWhiteSpace(y.MAJ))).ToList()
            };
            
            var a = GetIsReadOnly(tabGuid, argCodeOffre + "_" + argVersion + "_" + argType, codeAvn);
            var b = modeNavig;
            var 
                c = ModeConsultation.Historique.AsCode();
            if (!GetIsReadOnly(tabGuid, argCodeOffre + "_" + argVersion + "_" + argType, codeAvn) && !IsModifHorsAvenant && modeNavig != ModeConsultation.Historique.AsCode()) {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFormule>()) {
                    client.Channel.ValiderConditions(
                        new AffaireId {
                            CodeAffaire = argCodeOffre,
                            NumeroAliment = int.Parse(argVersion),
                            TypeAffaire = argType.ParseCode<AffaireType>()
                        },
                        int.Parse(argCodeOption),
                        int.Parse(argCodeFormule),
                        infosContrat);
                }
            }

            CallIsInHpeng(argCodeOffre, argVersion, argType, modeNavig, codeAvn);
            DeleteConditionFromCache(keyConditions);
            return string.Empty;
        }
        public static void CallIsInHpeng(string codeOffre, string version, string type, string modeNavig, string codeAvenant) {
            var acteGestion = "**";
            if (type == "P")
            {
                var lstPeriod = new List<EngagementPeriodeDto>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    lstPeriod = client.Channel.IsInHpeng(codeOffre);
                    if (lstPeriod.Count != 0)
                    {
                        foreach (EngagementPeriodeDto Period in lstPeriod)
                        {
                            CommonRepository.LoadAS400EngagementAvn(codeOffre, version, type, Period.Code.ToString(), codeAvenant, GetUser(), acteGestion);
                        }
                    }
                }

            }
            else
            {
                CommonRepository.LoadAS400Engagement(codeOffre, version, type, modeNavig.ParseCode<ModeConsultation>(), codeAvenant, GetUser(), acteGestion);
            }

        }
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult RedirectValidationConditions(string paramRedirect, string argCodeOffre, string argType, string argVersion, string argCodeFormule, string argCodeOption, string argCodeRisque, string argLettre, string argLibelle, string tabGuid, string saveCancel, string modeNavig, string addParamType, string addParamValue, bool isForceReadOnly) {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            Model.NumAvenantPage = numAvn;
            this.model.CodeFormule = argCodeFormule;
            this.model.CodeOption = argCodeOption;
            if (!string.IsNullOrEmpty(paramRedirect)) {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            if (!GetIsReadOnly(tabGuid, argCodeOffre + "_" + argVersion + "_" + argType, numAvn) && modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Standard) {
                GenerateClausesEtape(argCodeOffre, argType, argVersion, argCodeFormule, argCodeOption, argCodeRisque, AlbConstantesMetiers.Etapes.Garantie);
            }
            var idProvenance = string.Join("£", new[] { argCodeOffre, argVersion, argType, argCodeFormule, argCodeOption, argCodeRisque });
            return RedirectToAction("Index", "ChoixClauses", new { id = argCodeOffre + "_" + argVersion + "_" + argType + "_¤ConditionsGarantie¤Index¤" + idProvenance + tabGuid + BuildAddParamString(addParamType, addParamValue + (isForceReadOnly ? "||FORCEREADONLY|1" : string.Empty)) + GetFormatModeNavig(modeNavig), returnHome = saveCancel, guidTab = tabGuid });
        }

        [ErrorHandler]
        public ActionResult ReloadFiltre(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption,
            string typeFiltre, string garantie, string voletbloc, string niveau, string modeNavig) {
            ModeleConditionsFiltre model = new ModeleConditionsFiltre {
                FiltreType = typeFiltre,
                FiltreListe = GetListeFiltre(codeOffre, version, type, codeAvn, codeFormule, codeOption, typeFiltre, garantie, voletbloc, niveau, modeNavig)
            };
            return PartialView("ConditionFiltre", model);
        }

        [ErrorHandler]
        public string CheckConditionInCache(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string modeNavig, bool isReadOnly) {
            model.ModeNavig = modeNavig;
            var keyConditions = GetKeyConditions(new IdentifiantOption {
                Affaire = new Models.Affaire { CodeOffre = codeOffre, Version = int.Parse(version), Type = type },
                NumFormule = int.Parse(codeFormule),
                NumOption = int.Parse(codeOption)
            });
            var result = GetConditionsGaranties(keyConditions, codeAvn, modeNavig, isReadOnly, true);
            if (result == null)
                return "KO";
            return string.Empty;
        }

        public ExportToCSVResult<ModeleConditionGarantieExport> ExportFile(string id) {
            if (string.IsNullOrEmpty(id)) {
                return null;
            }
            var paramList = id.Split('_');
            if (paramList.Count() != 9) {
                return null;
            }
            var codeOffre = paramList[0];
            var version = paramList[1];
            var type = paramList[2];
            var codeFormule = paramList[3];
            var codeOption = paramList[4];
            var fileName = paramList[5];
            var isReadOnly = paramList[6];
            var modeNavig = paramList[7];
            var codeAvn = paramList[8];
            var keyConditions = GetKeyConditions(new IdentifiantOption {
                Affaire = new Models.Affaire { CodeOffre = codeOffre, Version = int.Parse(version), Type = type },
                NumFormule = int.Parse(codeFormule),
                NumOption = int.Parse(codeOption)
            });
            var result = GetConditionsGaranties(keyConditions, codeAvn, AlbEnumInfoValue.GetEnumInfo(ModeConsultation.Standard), isReadOnly == "True", true);

            var lstTarif = new List<ModeleConditionGarantieExport>();
            if (result != null) {
                var lstGarantie = result.LstEnsembleLigne;

                lstGarantie.ForEach(l => l.LstLigneGarantie.ForEach(t => lstTarif.Add(new ModeleConditionGarantieExport {
                    NiveauGarantie = l.Niveau,
                    CodeGarantie = l.Code,
                    LibGarantie = l.Description,
                    FrhValeur = t.FranchiseValeur,
                    FrhUnite = t.FranchiseUnite,
                    FrhType = !string.IsNullOrEmpty(t.LibFRHComplexe) ? t.LibFRHComplexe : t.FranchiseType,
                    LciValeur = t.LCIValeur,
                    LciUnite = t.LCIUnite,
                    LciType = !string.IsNullOrEmpty(t.LibLCIComplexe) ? t.LibLCIComplexe : t.LCIType,
                    AssValeur = t.AssietteValeur,
                    AssUnite = t.AssietteUnite,
                    AssType = t.AssietteType,
                    TxHtValeur = t.TauxForfaitHTValeur,
                    TxHtUnite = t.TauxForfaitHTUnite,
                    TxHtMin = t.TauxForfaitHTMinimum
                })));


            }

            const string columns = "Niveau;Code Garantie;Libellé Garantie;Franchise Valeur;Franchise Unité;Franchise Type;LCI Valeur;LCI Unité;LCI Type;Assiette Valeur;Assiette Unité;Assiette Type;Taux/Forfait HT Valeur;Taux/Forfait HT Unité;Taux/Forfait HT Min";
            var ret = new ExportToCSVResult<ModeleConditionGarantieExport>(lstTarif, fileName, columns);
            return ret;
        }

        /// <summary>
        /// Ouvre le référentiel des expressions complexes
        /// </summary>
        [ErrorHandler]
        public ActionResult OpenReferentiel(string mode, string type, string codeExpr, bool isReadOnly) {
            return PartialView("ListExprCompReferentiel", LoadExprCompReferentiel(mode, type, codeExpr, isReadOnly));
        }

        [HttpPost]
        public JsonResult ComputeGareat(ModeleConditionsGarantiePage modelConditions) {
            var gareat = ComputeGareat(
                modelConditions.AffaireId,
                int.Parse(modelConditions.CodeRisque),
                decimal.TryParse(modelConditions.InformationsContrat.LCIGenerale.Valeur, out var d) ? new decimal?(d) : null,
                (modelConditions.InformationsCondition?.ListGaranties?.Any() ?? false)
                    ? modelConditions.InformationsCondition.ListGaranties.Sum(g => g.PrimeGareat)
                    : null);

            return Json(new {
                gareat.Prime,
                Tranche = $"{gareat.CodeTranche} - {gareat.TauxTranche.ToString("P2")}",
                TauxRetenu = Math.Round(gareat.TauxRetenu * 100M, 2)
            });
        }

        [HttpPost]
        public JsonResult ComputeGareatEng(ModeleConditionsGarantiePage modelConditions)
        {
            ComputeGareatEng(
                modelConditions.AffaireId,
                decimal.TryParse(modelConditions.InformationsContrat.LCIGenerale.Valeur, out var d) ? new decimal?(d) : null);

            return Json(new
            {
              //  gareat.Prime,
              //  Tranche = $"{gareat.CodeTranche} - {gareat.TauxTranche.ToString("P2")}",
               // TauxRetenu = Math.Round(gareat.TauxRetenu * 100M, 2)
            });
        }
        /// <summary>
        /// Recherche les expressions complexes dans le référentiel
        /// dont le code ou le libellé contient "searchExpr"
        /// </summary>
        [ErrorHandler]
        public ActionResult SearchExprReferentiel(string type, string codeExpr) {
            return PartialView("TabExprCompReferentiel", LoadExprCompReferentiel("ref", type, codeExpr));
        }

        /// <summary>
        /// Valide et copie l'expression complexe
        /// du référentiel
        /// </summary>
        [ErrorHandler]
        public string ValidSelExprReferentiel(string codeOffre, string version, string type, string mode, string typeExpr, string idExpr, string splitCharHtml) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                return serviceContext.ValidSelExprReferentiel(codeOffre, version, type, mode, typeExpr, idExpr, splitCharHtml);
            }
        }

        [ErrorHandler]
        public string DuplicateExpr(string codeOffre, string version, string type, string codeAvn, string typeExpr, string codeExpr) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                return serviceContext.DuplicateExpr(codeOffre, version, type, codeAvn, typeExpr, codeExpr);
            }
        }

        #region Méthodes Privées

        private static void GenerateClausesEtape(string codeOffre, string type, string version, string codeFormule, string codeOption, string codeRisque, AlbConstantesMetiers.Etapes etape) {
            int argVersion;
            int argCodeFor;
            int argCodeOpt;
            int argCodeRsq;

            codeRisque = "0" + codeRisque;

            if (int.TryParse(version, out argVersion) && int.TryParse(codeFormule, out argCodeFor) && int.TryParse(codeOption, out argCodeOpt) && int.TryParse(codeRisque, out argCodeRsq)) {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    RetGenClauseDto retGenClause = client.Channel.GenerateClause(type, codeOffre,
                      argVersion,
                      new ParametreGenClauseDto {
                          ActeGestion = "**",
                          Letape = AlbEnumInfoValue.GetEnumInfo(etape),
                          NuFormule = argCodeFor,
                          NuOption = argCodeOpt,
                          NuRisque = argCodeRsq
                      });
                    if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur)) {
                        throw new AlbFoncException(retGenClause.MsgErreur);
                    }
                }
            }
        }

        private static ConditionGarantieDto BuildConditionGarantie(ModeleConditionsLigneGarantie condition) {
            var assiette = decimal.TryParse(condition.AssietteValeur, out var d) ? d : default;
            var franchise = decimal.TryParse(condition.FranchiseValeur, out d) ? d : default;
            var lci = decimal.TryParse(condition.LCIValeur, out d) ? d : default;
            var prime = decimal.TryParse(condition.TauxForfaitHTValeur, out d) ? d : default;
            var primePro = decimal.TryParse(condition.TauxForfaitHTMinimum, out d) ? d : default;
            long idTarif = long.TryParse(condition.Code.Split('_').First(), out var sh) && sh > 0 ? sh : default;
            return new ConditionGarantieDto {
                AssietteGarantie = new ValeursOptionsTarif {
                    ValeurActualise = assiette,
                    ValeurOrigine = assiette,
                    Unite = new Unite { Code = condition.AssietteUnite },
                    Base = new BaseDeCalcul { Code = condition.AssietteType },
                    IsModifiable = !condition.AssietteLectureSeule.AsBoolean(),
                    IsObligatoire = condition.AssietteObligatoire.AsBoolean()
                },
                CodeGarantie = condition.CodeGarantie,
                IdGarantie = condition.IdGarantie,
                TarifsGarantie = new TarifGarantieDto {
                    Id = idTarif,
                    Franchise = new ValeursOptionsTarif {
                        Base = new BaseDeCalcul { Code = condition.FranchiseType },
                        ExpressionComplexe = long.TryParse(condition.LienFRHComplexe.Split('¤')[0], out long l) && l > 0
                            ? new ExpressionComplexeBase { Id = l } : null,
                        IsModifiable = !condition.FranchiseLectureSeule.AsBoolean(),
                        IsObligatoire = condition.FranchiseObligatoire.AsBoolean(),
                        Unite = new Unite { Code = condition.FranchiseUnite },
                        ValeurActualise = franchise,
                        ValeurOrigine = franchise,
                        ValeurTravail = franchise
                    },
                    LCI = new ValeursOptionsTarif {
                        Base = new BaseDeCalcul { Code = condition.LCIType },
                        ExpressionComplexe = long.TryParse(condition.LienLCIComplexe.Split('¤')[0], out l) && l > 0
                            ? new ExpressionComplexeBase { Id = l } : null,
                        IsModifiable = !condition.LCILectureSeule.AsBoolean(),
                        IsObligatoire = condition.LCIObligatoire.AsBoolean(),
                        Unite = new Unite { Code = condition.LCIUnite },
                        ValeurActualise = lci,
                        ValeurOrigine = lci,
                        ValeurTravail = lci
                    },
                    PrimeValeur = new ValeursOptionsTarif {
                        Base = new BaseDeCalcul { Code = string.Empty },
                        ExpressionComplexe = null,
                        IsModifiable = !condition.TauxForfaitHTLectureSeule.AsBoolean(),
                        IsObligatoire = condition.TauxForfaitHTObligatoire.AsBoolean(),
                        Unite = new Unite { Code = condition.TauxForfaitHTUnite },
                        ValeurActualise = prime,
                        ValeurOrigine = prime,
                        ValeurTravail = prime
                    },
                    PrimeProvisionnelle = primePro
                }
            };
        }

        protected override void LoadInfoPage(string id) {
            string[] tId = id.Split('_');
            var (codeAffaire, version, type, codeFormule, codeOption, _) = tId;
            switch (type) {
                case "O":
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        var CommonOffreClient = chan.Channel;
                        model.Offre = new Offre_MetaModel();
                        global::OP.WSAS400.DTO.Common.InfosBaseDto infosBaseDto = CommonOffreClient.LoadInfosBase(codeAffaire, version, type, model.NumAvenantPage, model.ModeNavig);
                        model.Offre.LoadInfosOffre(infosBaseDto);
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                        model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                    }
                    break;
                case "P":
                    using (var proxy = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        var infosBase = proxy.Channel.LoadInfosBase(codeAffaire, version, type, model.NumAvenantPage, model.ModeNavig);
                        this.model.Contrat = new ContratDto() {
                            CodeContrat = infosBase.CodeOffre,
                            VersionContrat = Convert.ToInt64(infosBase.Version),
                            Type = infosBase.Type,
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
                            IndiceReference = infosBase.IndiceReference,
                            Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                            Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur,
                            NumAvenant = infosBase.NumAvenant
                        };
                        model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                    }
                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt) {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.ScreenType = AlbConstantesMetiers.TYPE_AVENANT_REGUL;
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
                    break;
            }
            model.PageTitle = "Conditions de garanties";
            if (model.Offre != null || model.Contrat != null) {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            model.Bandeau = null;
            SetBandeauNavigation(id);

            model.CodeFormule = codeFormule;
            model.CodeOption = codeOption;
            if (model.Offre != null) {
                LoadDataConditionsGarantie(model.Offre.CodeOffre.Trim(), model.Offre.Version, model.Offre.Type, string.Empty, model.CodeFormule, model.CodeOption, model.ModeNavig, false);
            }
            else if (model.Contrat != null) {
                LoadDataConditionsGarantie(model.Contrat.CodeContrat.Trim(), int.Parse(model.Contrat.VersionContrat.ToString()), model.Contrat.Type, model.NumAvenantPage, model.CodeFormule, model.CodeOption, model.ModeNavig, false);
            }

            SetArbreNavigation();
            if (type == "O") {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            }
            else {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
            }
        }

        private void LoadDataConditionsGarantie(string codeOffre, int? version, string type, string codeAvn, string codeFormule, string codeOption, string modeNavig, bool loadFormule) {
            model.ModeNavig = modeNavig;
            string keyConditions = GetKeyConditions(new IdentifiantOption {
                Affaire = new Models.Affaire { CodeOffre = codeOffre, Version = version.GetValueOrDefault(), Type = type },
                NumFormule = int.Parse(codeFormule),
                NumOption = int.Parse(codeOption)
            });
            DeleteConditionFromCache(keyConditions);
            ConditionRisqueGarantieGetResultDto result = GetConditionsGaranties(keyConditions, codeAvn, model.ModeNavig, model.IsReadOnly, loadFormule: loadFormule);
            if (result != null) {
                SetConditionFromCache(keyConditions, result);
                LoadInfoContrat(result);
                model.InformationsCondition = LoadInfoCondition(result, type);
                this.model.InfosGareat = LoadInfosGareat(result);
                model.InformationsCondition.IsReadOnly = this.model.IsReadOnly;
            }
        }

        private void LoadInfoContrat(ConditionRisqueGarantieGetResultDto result) {
            model.InformationsContrat = new ModeleConditionsInfosContrat();
            model.InformationsContrat.ExpAssiette = result.ExpAssiette;
            LoadLCIFranchise(result);
        }

        private void LoadLCIFranchise(ConditionRisqueGarantieGetResultDto result) {
            var unitesLCI = result.UnitesLCI.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
            var typesLCI = result.TypesLCI.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();

            var unitesFranchise = result.UnitesFranchise.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
            var typesFranchise = result.TypesFranchise.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();

            var unitesLCIRisque = result.UnitesLCI.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
            var typesLCIRisque = result.TypesLCI.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();

            var unitesFranchiseRisque = result.UnitesFranchise.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
            var typesFranchiseRisque = result.TypesFranchise.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();

            //Région Generale
            model.InformationsContrat.LCIGenerale = new Models.ModelesLCIFranchise.ModeleLCIFranchise {
                TypeVue = AlbConstantesMetiers.ExpressionComplexe.LCI,
                TypeAppel = AlbConstantesMetiers.TypeAppel.Generale,
                Valeur = result.LCI,
                Unite = result.UniteLCI,
                Unites = unitesLCI,
                Type = result.TypeLCI,
                Types = typesLCI,
                IsIndexe = result.IsIndexeLCI,
                LienComplexe = result.LienComplexeLCIGenerale,
                LibComplexe = result.LibComplexeLCIGenerale,
                CodeComplexe = result.CodeComplexeLCIGenerale
            };
            model.InformationsContrat.FranchiseGenerale = new Models.ModelesLCIFranchise.ModeleLCIFranchise {
                TypeVue = AlbConstantesMetiers.ExpressionComplexe.Franchise,
                TypeAppel = AlbConstantesMetiers.TypeAppel.Generale,
                Valeur = result.Franchise,
                Unite = result.UniteFranchise,
                Unites = unitesFranchise,
                Type = result.TypeFranchise,
                Types = typesFranchise,
                IsIndexe = result.IsIndexeFranchise,
                LienComplexe = result.LienComplexeFranchiseGenerale,
                LibComplexe = result.LibComplexeFranchiseGenerale,
                CodeComplexe = result.CodeComplexeFranchiseGenerale
            };
            //Région Risque
            model.InformationsContrat.LCIRisque = new Models.ModelesLCIFranchise.ModeleLCIFranchise {
                TypeVue = AlbConstantesMetiers.ExpressionComplexe.LCI,
                TypeAppel = AlbConstantesMetiers.TypeAppel.Risque,
                Valeur = result.LCIRisque,
                Unite = result.UniteLCIRisque,
                Unites = unitesLCIRisque,
                Type = result.TypeLCIRisque,
                Types = typesLCIRisque,
                LienComplexe = result.LienComplexeLCIRisque,
                LibComplexe = result.LibComplexeLCIRisque,
                CodeComplexe = result.CodeComplexeLCIRisque

            };
            model.InformationsContrat.FranchiseRisque = new Models.ModelesLCIFranchise.ModeleLCIFranchise {
                TypeVue = AlbConstantesMetiers.ExpressionComplexe.Franchise,
                TypeAppel = AlbConstantesMetiers.TypeAppel.Risque,
                Valeur = result.FranchiseRisque,
                Unite = result.UniteFranchiseRisque,
                Unites = unitesFranchiseRisque,
                Type = result.TypeFranchiseRisque,
                Types = typesFranchiseRisque,
                LienComplexe = result.LienComplexeFranchiseRisque,
                LibComplexe = result.LibComplexeFranchiseRisque,
                CodeComplexe = result.CodeComplexeFranchiseRisque
            };

        }

        private ModeleConditionsInfosCondition LoadInfoCondition(ConditionRisqueGarantieGetResultDto result, string type, bool fullScreen = false) {
            var conditions = new ModeleConditionsInfosCondition {
                CodeBranche = result.CodeBranche,
                CodeCible = result.CodeCible,
                Formule = !string.IsNullOrEmpty(result.Formule) ? result.Formule : string.Empty,
                AppliqueA = !string.IsNullOrEmpty(result.AppliqueA) ? result.AppliqueA : string.Empty,
                Garantie = !string.IsNullOrEmpty(result.Garantie) ? result.Garantie : string.Empty,
                IsAvnDisabled = result.IsAvnDisabled,
                FiltreGarantie = result.Garanties != null ? new ModeleConditionsFiltre { FiltreType = "Garantie", FiltreListe = result.Garanties.Select(g => new AlbSelectListItem { Text = string.Format("{0}", g.Libelle), Value = g.Code.Trim(), Selected = false, Title = string.Format("{0} - {1}", g.Libelle, g.Descriptif) }).ToList() } : new ModeleConditionsFiltre(),
                FiltreVoletsBlocs = result.VoletsBlocs != null ? new ModeleConditionsFiltre { FiltreType = "VoletBloc", FiltreListe = result.VoletsBlocs.Select(vb => new AlbSelectListItem { Text = string.Format("{0}", vb.Libelle), Value = vb.Code.Trim(), Selected = false, Title = string.Format("{0} - {1}", vb.Libelle.Trim(), vb.Descriptif.Trim()) }).ToList() } : new ModeleConditionsFiltre(),
                FiltreNiveau = result.Niveaux != null ? new ModeleConditionsFiltre { FiltreType = "Niveau", FiltreListe = result.Niveaux.Select(n => new AlbSelectListItem { Text = string.Format("{0}", n.Libelle), Value = n.Code.Trim(), Selected = false, Title = string.Format("{0}", n.Libelle) }).ToList() } : new ModeleConditionsFiltre()
            };

            conditions.ListGaranties = ObjectMapperManager.DefaultInstance.GetMapper<List<EnsembleGarantieDto>, List<ModeleConditionsGarantie>>().Map(result.LstEnsembleLigne.ToList());

            conditions.ListGaranties.ForEach(elem => {
                elem.LCIUnites = result.LstEnsembleLigne[0].LCIUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                elem.LCITypes = result.LstEnsembleLigne[0].LCITypes.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                elem.FranchiseUnites = result.LstEnsembleLigne[0].FranchiseUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                elem.FranchiseTypes = result.LstEnsembleLigne[0].FranchiseTypes.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                elem.AssietteUnites = result.LstEnsembleLigne[0].AssietteUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                elem.AssietteTypes = result.LstEnsembleLigne[0].AssietteTypes.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                elem.TauxForfaitHTUnites = result.LstEnsembleLigne[0].TauxForfaitHTUnites.Select(m => new AlbSelectListItem { Text = string.Format("{0} - {1}", m.Code, m.Libelle), Value = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
            });


            conditions.FullScreen = fullScreen;
            conditions.Type = type;

            if (result.LstRisque != null) {
                conditions.LstRisque = new Models.ModelesRisque.ModeleRisque {
                    Code = result.LstRisque.Code.ToString(),
                    Designation = result.LstRisque.Descriptif
                };

                foreach (var obj in result.LstRisque.Objets) {
                    conditions.LstRisque.Objets.Add(new Models.ModelesObjet.ModeleObjet {
                        Code = obj.Code.ToString(),
                        Designation = obj.Descriptif
                    });
                }
            }
            else {
                conditions.LstRisque = new Models.ModelesRisque.ModeleRisque();
                conditions.LstRisque.Objets = new List<Models.ModelesObjet.ModeleObjet>();
            }


            return conditions;
        }

        private ModelInfosGareat LoadInfosGareat(ConditionRisqueGarantieGetResultDto result) {
            if (!result.LstEnsembleLigne.Any(x => x.IsAttentatGareat)) {
                return null;
            }
            var ligne = result.LstEnsembleLigne.FirstOrDefault(l => l.Code == Albingia.Kheops.OP.Domain.Formule.Garantie.CodeGareatAttent)?.LstLigneGarantie.First();
            if (ligne is null) {
                return null;
            }

            // init prime gareat for each Garantie
            var garantiesGareat = result.LstEnsembleLigne.Where(x => x.IsAttentatGareat).Select(x => x.LstLigneGarantie.First());
            decimal? primeGaranties = !garantiesGareat.Any() ? null
                : garantiesGareat.Sum(x => {
                    if (x.TauxForfaitHTUnite.IsEmptyOrNull()) {
                        return null;
                    }
                    bool isTauxPcent = x.TauxForfaitHTUnite == Albingia.Kheops.OP.Domain.UniteBase.PourCent.AsCode();
                    bool isTauxPml = x.TauxForfaitHTUnite == Albingia.Kheops.OP.Domain.UniteBase.PourMille.AsCode();
                    decimal? primeMin = decimal.TryParse(x.TauxForfaitHTMinimum, out var dec) ? dec : default(decimal?);
                    decimal? prime = !isTauxPcent && !isTauxPml ? decimal.TryParse(x.TauxForfaitHTValeur, out dec) ? dec : default(decimal?) : null;
                    decimal? taux = isTauxPcent || isTauxPml
                        ? decimal.TryParse(x.TauxForfaitHTValeur, out dec) ? (dec / (isTauxPcent ? 100 : 1000)) : default(decimal?)
                        : null;
                    decimal? assiette = x.AssietteUnite != "D" ? null : decimal.TryParse(x.AssietteValeur, out dec) ? dec : default(decimal?);

                    if (prime.HasValue) {
                        return prime > primeMin.GetValueOrDefault() ? prime : primeMin;
                    }
                    if (assiette.GetValueOrDefault() == default || taux.GetValueOrDefault() == default) {
                        return null;
                    }
                    prime = assiette * taux;
                    return prime > primeMin.GetValueOrDefault() ? prime : primeMin;
                });
            
            // compute expected value
            var gareat = ComputeGareat(
                Model.AffaireId,
                int.Parse(this.model.CodeRisque),
                decimal.TryParse(this.model.InformationsContrat.LCIGenerale.Valeur, out var d) ? new decimal?(d) : null, primeGaranties);

            decimal? valeur = null;
            var primeGareat = Math.Round(gareat.Prime, 2, MidpointRounding.AwayFromZero);
            if (!decimal.TryParse(ligne.TauxForfaitHTValeur, out d)
                || d == (decimal.TryParse(ligne.TauxForfaitHTValeurOrigine, out var d2) ? d2 : 0)) {

                valeur = null;

            }
            else {
                valeur = d;
            }
            return new ModelInfosGareat {
                Prime = valeur.HasValue && valeur != primeGareat ? valeur : null,
                PrimeTheorique = primeGareat,
                CodeTranche = gareat.CodeTranche,
                TauxCommissions = gareat.TauxCommissions,
                TauxFraisGeneraux = gareat.TauxFraisGeneraux,
                TauxTranche = gareat.TauxTranche,
                TauxRetenu = Math.Round(gareat.TauxRetenu * 100M, 2, MidpointRounding.AwayFromZero),
                CodeRegimeTaxe = gareat.CodeRegimeTaxe
            };
        }

        private void CheckDataExpr(List<ModeleConditionsLigneGarantie> argLigne, string argType) {
            //-- Verification de tous les valeurs
            foreach (ModeleConditionsLigneGarantie temp in argLigne) {

                if (argType == "LCI") {
                    if (!string.IsNullOrEmpty(temp.ConcurrenceValeur))
                        ParseDouble(temp.ConcurrenceValeur);
                    ParseDouble(temp.LCIValeur);
                    if (string.IsNullOrEmpty(temp.UniteLCINew) && string.IsNullOrEmpty(temp.LCIUnite)) { throw new AlbFoncException("L'unité de la LCI est obligatoire."); }
                }
                else {
                    ParseDouble(temp.FranchiseValeur);
                    if (string.IsNullOrEmpty(temp.UniteFranchiseNew) && string.IsNullOrEmpty(temp.FranchiseUnite)) { throw new Exception("L'unité de la franchise est obligatoire."); }
                }
            }
        }

        private double ParseDouble(string value) {
            try {
                return double.Parse(value);
            }
            catch (Exception) {

                throw new AlbFoncException("La ligne des détails de l'expression n'est pas valide : format non valide");
            }

        }

        private bool CheckRowGarantie(ModeleConditionsLigneGarantie row) {
            try {
                bool toReturn = true;

                row.LCIValeur = row.LCIValeur.Replace(".", ",");
                row.FranchiseValeur = row.FranchiseValeur.Replace(".", ",");
                row.AssietteValeur = row.AssietteValeur.Replace(".", ",");
                row.TauxForfaitHTValeur = row.TauxForfaitHTValeur.Replace(".", ",");

                //Contrôle LCI
                if (row.LCIObligatoire == "O" && row.LCIUnite != "CPX" && string.IsNullOrEmpty(row.LCIValeur))
                    toReturn = false;
                // bug 2492
                if ((!string.IsNullOrEmpty(row.LCIValeur) && (!string.IsNullOrEmpty(row.LCIUnite) || (string.IsNullOrEmpty(row.LCIUnite) && row.LCIType == "XXX")) && !string.IsNullOrEmpty(row.LCIType))
                    || (string.IsNullOrEmpty(row.LCIValeur) && string.IsNullOrEmpty(row.LCIUnite) && (string.IsNullOrEmpty(row.LCIType) || row.LCIType == "XXX") && row.LCIObligatoire == "N")
                    || (row.LCIUnite == "CPX")) { }
                else
                    toReturn = false;

                //Contrôle Franchise
                if (row.FranchiseObligatoire == "O" && row.FranchiseUnite != "CPX" && string.IsNullOrEmpty(row.FranchiseValeur))
                    toReturn = false;

                if ((!string.IsNullOrEmpty(row.FranchiseValeur) && (!string.IsNullOrEmpty(row.FranchiseUnite) || (string.IsNullOrEmpty(row.FranchiseUnite) && row.FranchiseType == "XXX")) && !string.IsNullOrEmpty(row.FranchiseType))
                    || (string.IsNullOrEmpty(row.FranchiseValeur) && string.IsNullOrEmpty(row.FranchiseUnite) && (string.IsNullOrEmpty(row.FranchiseType) || row.FranchiseType == "XXX") && row.FranchiseObligatoire == "N")
                    || (row.FranchiseUnite == "CPX")) { }
                else
                    toReturn = false;

                if ((!string.IsNullOrEmpty(row.AssietteValeur) && !string.IsNullOrEmpty(row.AssietteUnite) && !string.IsNullOrEmpty(row.AssietteType))
                    || (string.IsNullOrEmpty(row.AssietteValeur) && string.IsNullOrEmpty(row.AssietteUnite) && string.IsNullOrEmpty(row.AssietteType) && row.AssietteObligatoire == "N")) { }
                else
                    toReturn = false;
                if (!string.IsNullOrEmpty(row.AssietteValeur)) {
                    if (row.AssietteUnite == "%" && (Convert.ToDecimal(row.AssietteValeur) < 0 || Convert.ToDecimal(row.AssietteValeur) > 100))
                        toReturn = false;
                }

                //Contrôle Taux Forfait
                if ((!string.IsNullOrEmpty(row.TauxForfaitHTValeur) && !string.IsNullOrEmpty(row.TauxForfaitHTUnite))
                    || (string.IsNullOrEmpty(row.TauxForfaitHTValeur) && string.IsNullOrEmpty(row.TauxForfaitHTUnite) && row.TauxForfaitHTObligatoire == "N")) { }
                else
                    toReturn = false;
                if (!string.IsNullOrEmpty(row.TauxForfaitHTValeur)) {
                    if (row.TauxForfaitHTUnite == "%" && (Convert.ToDecimal(row.TauxForfaitHTValeur) < 0 || Convert.ToDecimal(row.TauxForfaitHTValeur) > 100))
                        toReturn = false;
                    //ECM 2017-12-20 : Suppression du contrôle de la prime minimum < prime lorsqu'on est en devise (D) (vu avec JDA)
                    //if (!string.IsNullOrEmpty(row.TauxForfaitHTMinimum))
                    //{
                    //    if (Convert.ToDecimal(row.TauxForfaitHTValeur) < Convert.ToDecimal(row.TauxForfaitHTMinimum) && row.TauxForfaitHTUnite != "%" && row.TauxForfaitHTUnite != "%0")
                    //        toReturn = false;
                    //}
                }

                return toReturn;
            }
            catch (Exception) {
                throw new Exception("La ligne de condition n'est pas valide : format non valide");
            }
        }
        private List<AlbSelectListItem> GetListeFiltre(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption,
            string typeFiltre, string garantie, string voletbloc, string niveau, string modeNavig) {
            List<AlbSelectListItem> model = new List<AlbSelectListItem>();

            switch (typeFiltre) {
                case "Garantie":
                    typeFiltre = "G";
                    break;
                case "VoletBloc":
                    typeFiltre = "V";
                    break;
                case "Niveau":
                    typeFiltre = "N";
                    break;
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                var result = serviceContext.GetListeFiltre(codeOffre, version, type, codeAvn, codeFormule, codeOption, typeFiltre, garantie, voletbloc, niveau, modeNavig.ParseCode<ModeConsultation>());

                if (result != null)
                    model = result.Select(f => new AlbSelectListItem {
                        Text = string.Format("{0}", f.Libelle),
                        Value = f.Code.Trim(),
                        Selected = false,
                        Title = !string.IsNullOrEmpty(f.Descriptif) ? string.Format("{0} - {1}", f.Libelle, f.Descriptif) : f.Libelle
                    }).ToList();
            }

            return model;
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
                            model.Bandeau.StyleBandeau = model.ScreenType;
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
                    //model.NavigationArbre = GetNavigationArbreRegule(ContentData, "Regule");
                    //model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
                }
            }
        }
        private void SetArbreNavigation() {
            if (model.Offre != null) {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Condition", codeFormule: Convert.ToInt32(model.CodeFormule));
            }
            else if (model.Contrat != null) {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Condition", codeFormule: Convert.ToInt32(model.CodeFormule));
            }
        }


        private LigneGarantieDto CopyTarif(LigneGarantieDto firstTarif, int maxNumTar, string codeGarantie) {
            LigneGarantieDto newTarif = new LigneGarantieDto {
                Code = string.Format("{0}_{1}", maxNumTar, codeGarantie),
                Id = firstTarif.Id,
                NumeroTarif = maxNumTar.ToString(),
                LCIValeur = firstTarif.LCIValeur,
                LCIUnite = firstTarif.LCIUnite,
                LCIUnites = firstTarif.LCIUnites,
                LCIType = firstTarif.LCIType,
                LCITypes = firstTarif.LCITypes,
                LCILectureSeule = firstTarif.LCILectureSeule,
                LCIObligatoire = firstTarif.LCIObligatoire,
                LienLCIComplexe = firstTarif.LienLCIComplexe,
                FranchiseValeur = firstTarif.FranchiseValeur,
                FranchiseUnite = firstTarif.FranchiseUnite,
                FranchiseUnites = firstTarif.FranchiseUnites,
                FranchiseType = firstTarif.FranchiseType,
                FranchiseTypes = firstTarif.FranchiseTypes,
                FranchiseLectureSeule = firstTarif.FranchiseLectureSeule,
                FranchiseObligatoire = firstTarif.FranchiseObligatoire,
                FranchiseMini = firstTarif.FranchiseMini,
                FranchiseMaxi = firstTarif.FranchiseMaxi,
                FranchiseDebut = firstTarif.FranchiseDebut,
                FranchiseFin = firstTarif.FranchiseFin,
                LienFRHComplexe = firstTarif.LienFRHComplexe,
                AssietteValeur = firstTarif.AssietteValeur,
                AssietteUnite = firstTarif.AssietteUnite,
                AssietteUnites = firstTarif.AssietteUnites,
                AssietteType = firstTarif.AssietteType,
                AssietteTypes = firstTarif.AssietteTypes,
                AssietteLectureSeule = firstTarif.AssietteLectureSeule,
                AssietteObligatoire = firstTarif.AssietteObligatoire,
                TauxForfaitHTValeur = firstTarif.TauxForfaitHTValeur,
                TauxForfaitHTUnite = firstTarif.TauxForfaitHTUnite,
                TauxForfaitHTUnites = firstTarif.TauxForfaitHTUnites,
                TauxForfaitHTMinimum = firstTarif.TauxForfaitHTMinimum,
                TauxForfaitHTLectureSeule = firstTarif.TauxForfaitHTLectureSeule,
                TauxForfaitHTObligatoire = firstTarif.TauxForfaitHTObligatoire,
                ConcurrenceValeur = firstTarif.ConcurrenceValeur,
                ConcurrenceUnite = firstTarif.ConcurrenceUnite,
                ConcurrenceUnites = firstTarif.ConcurrenceUnites,
                ConcurrenceType = firstTarif.ConcurrenceType,
                ConcurrenceTypes = firstTarif.ConcurrenceTypes,
                UniteLCINew = firstTarif.UniteLCINew,
                UnitesLCINew = firstTarif.UnitesLCINew,
                TypeLCINew = firstTarif.TypeLCINew,
                TypesLCINew = firstTarif.TypesLCINew,
                UniteFranchiseNew = firstTarif.UniteFranchiseNew,
                UnitesFranchiseNew = firstTarif.UnitesFranchiseNew,
                TypeFranchiseNew = firstTarif.TypeFranchiseNew,
                TypesFranchiseNew = firstTarif.TypesFranchiseNew,
                UniteConcurrence = firstTarif.UniteConcurrence,
                UnitesConcurrence = firstTarif.UnitesConcurrence,
                TypeConcurrence = firstTarif.TypeConcurrence,
                TypesConcurrence = firstTarif.TypesConcurrence,
                Type = firstTarif.Type,
                Niveau = firstTarif.Niveau,
                CVolet = firstTarif.CVolet,
                CBloc = firstTarif.CBloc,
                MAJ = AlbConstantesMetiers.Traitement.InsertCondition.AsCode()
            };

            return newTarif;
        }

        private LigneGarantieDto SaveTarif(LigneGarantieDto ligne, ModeleConditionsLigneGarantie conditionsLigneGarantie) {
            LigneGarantieDto result = ligne;

            result.LCIValeur = conditionsLigneGarantie.LCIValeur;
            result.LCIUnite = conditionsLigneGarantie.LCIUnite;
            result.LCIType = conditionsLigneGarantie.LCIType;
            result.LibLCIComplexe = conditionsLigneGarantie.LibLCIComplexe;
            result.LienLCIComplexe = conditionsLigneGarantie.LienLCIComplexe;

            result.FranchiseValeur = conditionsLigneGarantie.FranchiseValeur;
            result.FranchiseUnite = conditionsLigneGarantie.FranchiseUnite;
            result.FranchiseType = conditionsLigneGarantie.FranchiseType;
            result.LibFRHComplexe = conditionsLigneGarantie.LibFRHComplexe;
            result.LienFRHComplexe = conditionsLigneGarantie.LienFRHComplexe;

            result.TauxForfaitHTValeur = conditionsLigneGarantie.TauxForfaitHTValeur;
            result.TauxForfaitHTUnite = conditionsLigneGarantie.TauxForfaitHTUnite;
            result.TauxForfaitHTMinimum = conditionsLigneGarantie.TauxForfaitHTMinimum;

            result.MAJ = result.MAJ != AlbConstantesMetiers.Traitement.InsertCondition.AsCode() ? AlbConstantesMetiers.Traitement.UpdateCondition.AsCode() : result.MAJ;

            return result;
        }

        private string GetKeyConditions(IdentifiantOption id) {
            return string.Join(
                MvcApplication.SPLIT_CONST_HTML,
                new[] {
                    GetUser(),
                    //model.TabGuid,
                    id.Affaire.BuildId(MvcApplication.SPLIT_CONST_HTML),
                    id.NumFormule.ToString(),
                    id.NumOption.ToString() /*+ GetSurroundedTabGuid(model.TabGuid)*/,
                    model.ModeNavig
                });
        }

        private ConditionRisqueGarantieGetResultDto GetConditionsGaranties(string key, string codeAvn, string modeNavig, bool isReadOnly, bool onlyFromCache = false, bool loadFormule = false) {
            return AlbSessionHelper.ConditionsTarifairesUtilisateurs.TryGetValue(key, out dynamic conditionsGar) || onlyFromCache
                ? conditionsGar
                : GetConditionsGarantiesFromDb(key, codeAvn, modeNavig, isReadOnly, GetUser(), loadFormule);
        }

        private void SetConditionFromCache(string key, ConditionRisqueGarantieGetResultDto newConditionsGar) {
            if (!AlbSessionHelper.ConditionsTarifairesUtilisateurs.TryGetValue(key, out dynamic conditionsGar)) {
                AlbSessionHelper.ConditionsTarifairesUtilisateurs.Add(key, null);
            }
            AlbSessionHelper.ConditionsTarifairesUtilisateurs[key] = newConditionsGar;
        }

        private void DeleteConditionFromCache(string key) {
            if (key != null) {
                AlbSessionHelper.ConditionsTarifairesUtilisateurs.Remove(key);
            }
        }

        private static ConditionRisqueGarantieGetResultDto GetConditionsGarantiesFromDb(string keyConditions, string codeAvn, string modeNavig, bool isReadOnly, string user, bool loadFormule) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                string[] tKeyCondition = keyConditions.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
                var query = new ConditionRisqueGarantieGetQueryDto { NumeroOffre = tKeyCondition[1], version = tKeyCondition[2], Type = tKeyCondition[3], CodeFormule = tKeyCondition[4], CodeOption = tKeyCondition[5] };
                var result = client.Channel.ObtenirFullConditions(query, codeAvn, modeNavig.ParseCode<ModeConsultation>(), isReadOnly, loadFormule, user);
                
                return result;
            }
        }

        private ModeleConditionsListExprComplexe LoadExprCompReferentiel(string mode, string type, string codeExpr, bool isReadOnly = false) {
            ModeleConditionsListExprComplexe model = new ModeleConditionsListExprComplexe {
                IsReadOnly = isReadOnly,
                Type = type,
                Mode = mode,
                ListExpressions = new List<ModeleConditionsExprComplexeDetails>()
            };

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = client.Channel;
                var result = serviceContext.LoadListExprComplexeReferentiel(type, codeExpr);
                if (result != null && result.Any()) {
                    model.ListExpressions = result.Select(m => new ModeleConditionsExprComplexeDetails { Id = m.Id, Code = m.Code, Libelle = m.Libelle, Modifiable = mode == "mdl" ? true : m.Modifiable }).ToList();
                }
            }

            return model;
        }

        private GareatStateDto ComputeGareat(AffaireId affaireId, int numeroRisque, decimal? lciGenerale, decimal? primeGaranties) {
            var gareat = new GareatStateDto();
            using (var clientFor = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFormule>()) {
                var risque = clientFor.Channel.GetRisque(affaireId, numeroRisque);
                gareat.LCIGenerale = lciGenerale != decimal.Zero ? lciGenerale : null;
                gareat.PrimeGaranties = primeGaranties;
                gareat.CodeRegimeTaxe = risque.RegimeTaxe.Code;
                gareat = clientFor.Channel.ComputeGareat(affaireId, gareat);
            }
            return gareat;
        }

        private void ComputeGareatEng(AffaireId affaireId, decimal? lciGenerale)
        {
            var gareats = new List<GareatStateDto>();
            var gareat = new GareatStateDto();
            
            using (var clientRsq = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquePort>())
            using (var clientFor = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFormule>())
            using (var clientGar = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())

            {
                var risques = clientRsq.Channel.GetRisques(affaireId);
                gareat.LCIGenerale = lciGenerale != decimal.Zero ? lciGenerale : null;
                
                foreach (var risque in risques)
                {
                    var formules = clientGar.Channel.GetFormuleIdByRsq(affaireId.CodeAffaire, risque.Numero);
                    foreach (var formule in formules)
                    {
                        var sum = clientGar.Channel.GetSumPrimeGarantie(affaireId.CodeAffaire, formule.Code);
                        gareat.PrimeGaranties = sum;
                        gareat.CodeRegimeTaxe = risque.RegimeTaxe.Code;
                        gareat = clientFor.Channel.ComputeGareat(affaireId, gareat);
                        gareat.CodeRisqe = risque.Numero.ToString();
                        gareats.Add(gareat);
                        if (gareat.PrimeGaranties != 0 )
                        {
                            var LstId = clientGar.Channel.GetIdGar(affaireId.CodeAffaire,formule.Code);
                            if (LstId.Count != 0 ) {
                                clientGar.Channel.UpdateKpgartar(LstId.FirstOrDefault().Code, gareat.Prime);
                            }
                        }
                    }


                }
                   
            }
          
        }

        #endregion
    }
}
