using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModeleTraites;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.SMP;
using OP.WSAS400.DTO.Traite;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class EngagementTraiteController : ControllersBase<ModeleEngagementTraitePage>
    {
        public override bool IsReadonly => base.IsReadonly || this.model.IsReadOnly;

        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult Index(string id, AccessMode accessMode)
        {
            SetCdPeriodeGuidTab(id, out string formattedId);
            formattedId = InitializeParams(formattedId);
            this.model.CurrentAccessMode = accessMode;
            model.NomEcran = NomsInternesEcran.DetailsEngagement;
            LoadInfoPage(formattedId);

            if (!string.IsNullOrEmpty(model.AccessMode))
            {
                return AutoReadOnlyView("EngagementTraiteBody", null, this.model, IsReadonly, IsModifHorsAvenant);
            }

            return AutoReadOnlyView(null, null, this.model, IsReadonly, IsModifHorsAvenant);
        }

        [ErrorHandler]
        public ActionResult Update(string codeOffre, string version, string type, string codeAvn, string argEngagementTraite, string argModelCommentForce,
            string tabGuid, string saveCancel, string lienCpxLCI, string modeNavig, string codeTraite, string codePeriode, string accessMode)
        {
            UpdateEngagementTraite(codeOffre, version, type, codeAvn, argEngagementTraite, tabGuid, lienCpxLCI, modeNavig, codeTraite, codePeriode, accessMode);
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                UpdateCommentaireOnly(codeOffre, version, type, argModelCommentForce, codePeriode);
            }

            if (saveCancel == "1")
            {
                return RedirectToRoute("/RechercheSaisie/Index");
            }
            model.NomEcran = NomsInternesEcran.DetailsEngagement;
            model.CodePeriodeEng = codePeriode;
            model.AccessMode = accessMode;
            model.IsAjax = true;
            return PartialView("EngagementTraiteBody", model);
        }

        [ErrorHandler]
        [HandleJsonError]
        public ActionResult UpdateTraites(AffaireId affaireId, string tabGuid, string saveCancel, EngagementTraiteDto engagementTraite, string accessMode, AccessMode actionEngagement)
        {
            if (!IsReadonly)
            {
                if (engagementTraite.Traite.CommentForce.ContainsChars())
                {
                    engagementTraite.Traite.CommentForce = engagementTraite.Traite.CommentForce.Replace("\r\n", "<br>").Replace("\n", "<br>");
                }
                List<SMPTaiteDto> ListSmpVn = new List<SMPTaiteDto>();
                if (engagementTraite.Traite.IdVentilation != 0)
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                    {

                        var serviceContext = client.Channel;
                        var res = serviceContext.GetSmpT(engagementTraite.Traite.IdVentilation);
                        ListSmpVn = CalculSMP(res, engagementTraite.Traite.TotalSMP);
                        foreach (SMPTaiteDto element in ListSmpVn)
                        {
                            serviceContext.SaveSmpT(element.SmpCptF, element.Id);
                        }
                    }
                }
                else
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffairePort>())
                    {
                        client.Channel.SaveEngagementsTraites(affaireId, engagementTraite);
                    }
                    ConditionsGarantieController.CallIsInHpeng(affaireId.CodeAffaire,
                                                               affaireId.NumeroAliment.ToString(),
                                                               affaireId.TypeAffaire.AsCode(),
                                                               ModeConsultation.Historique.AsCode(),
                                                               affaireId.NumeroAvenant.ToString());
                }
            }

            if (saveCancel == "1")
            {
                return RedirectToRoute("/RechercheSaisie/Index");
            }

            string id = string.Join("_", new[] { affaireId.CodeAffaire.Trim(), affaireId.NumeroAliment.ToString(), affaireId.TypeAffaire.AsCode(), engagementTraite.CodeTraite });
            this.model.PageTitle = "Détail engagement";
            this.model.ModeNavig = affaireId.IsHisto ? ModeConsultation.Historique.AsCode() : string.Empty;
            this.model.NumAvenantPage = affaireId.NumeroAvenant.ToString();
            this.model.AccessMode = accessMode;
            this.model.CodePeriodeEng = engagementTraite.CodePeriode;
            LoadInfoPage(id);

            this.model.NomEcran = NomsInternesEcran.DetailsEngagement;
            this.model.IsAjax = true;
            this.model.CurrentAccessMode = actionEngagement;
            return PartialView("EngagementTraiteBody", this.model);
        }

        public List<SMPTaiteDto> CalculSMP(List<SMPTaiteDto> ListSmp, int Total)
        {
            int totalSmp = 0;
            int indice = 1;
            double SmpCptForce;
            int totalSmpN = 0;
            List<SMPTaiteDto> ListSmpVn = new List<SMPTaiteDto>();
            //Calcul Total SMP
            foreach (SMPTaiteDto element in ListSmp)
            {
                totalSmp += element.SmpCpt;
            }

            //Calcul % et Smp Forcé  
            foreach (SMPTaiteDto element in ListSmp)
            {
                SmpCptForce = ((double)element.SmpCpt / totalSmp) * Total;
                if (indice < ListSmp.Count)
                {
                    totalSmpN += Convert.ToInt32(Math.Round(SmpCptForce));
                    ListSmpVn.Add(new SMPTaiteDto { SmpCptF = Convert.ToInt32(Math.Round(SmpCptForce)), Id = element.Id });
                }
                else
                {
                    ListSmpVn.Add(new SMPTaiteDto { SmpCptF = Convert.ToInt32(Total - totalSmpN), Id = element.Id });
                }
                indice++;
            }
            return ListSmpVn;
        }


        [ErrorHandler]
        [AlbAjaxRedirect]
        public ActionResult UpdateRedirect(string codeOffre, string version, string type, string codeAvn, string argEngagementTraite, string argModelCommentForce,
             string tabGuid, string saveCancel, string paramRedirect, string lienCpxLCI, string modeNavig, string codeTraite, string codePeriode, string accessMode)
        {
            UpdateEngagementTraite(codeOffre, version, type, codeAvn, argEngagementTraite, tabGuid, lienCpxLCI, modeNavig, codeTraite, codePeriode, accessMode);
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                UpdateCommentaireOnly(codeOffre, version, type, argModelCommentForce, codePeriode);
            }
            var tabParamRedirect = paramRedirect.Split('/');

            return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] + (!string.IsNullOrEmpty(accessMode) ? "accessMode" + accessMode + "accessMode" : string.Empty) });

        }

        private void UpdateEngagementTraite(string codeOffre, string version, string type, string codeAvn, string argEngagementTraite, string tabGuid, string lienCpxLCI, string modeNavig, string codeTraite, string codePeriode, string accesMode)
        {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleTraiteUpdate>.GetSerializer();
                var engagementTraite = serialiser.ConvertToType<List<ModeleTraiteUpdate>>(serialiser.DeserializeObject(argEngagementTraite));

                if (engagementTraite != null)
                {
                    Double dLCIValeur = 0;

                    TraiteDto traiteDto = new TraiteDto
                    {
                        LCIValeur = Double.TryParse(engagementTraite[0].LCIValeur, out dLCIValeur) ? dLCIValeur : 0,
                        LCIUnite = engagementTraite[0].LCIUnite,
                        LCIType = engagementTraite[0].LCIType,
                        LCIIndexee = engagementTraite[0].LCIIndexee,
                        LienComplexeLCI = !string.IsNullOrEmpty(lienCpxLCI) ? Convert.ToInt64(lienCpxLCI) : 0

                    };

                    traiteDto.TraiteInfoRsqVen = new TraiteInfoRsqVenDto();
                    List<TraiteRisqueDto> lstTraiteRisqueDto = new List<TraiteRisqueDto>();

                    engagementTraite.ForEach(elem =>
                    {
                        var lstVentilationsRisques = new List<TraiteVentilationDto>();
                        engagementTraite.Where(rsq => rsq.CodeRisque == elem.CodeRisque).ToList().ForEach(ventilation =>
                        {
                            lstVentilationsRisques.Add(new TraiteVentilationDto { IdVentilation = ventilation.IdVentilation, SMP = "0" });
                        });
                        lstTraiteRisqueDto.Add(new TraiteRisqueDto { CodeRisque = elem.CodeRisque, TraiteVentilations = lstVentilationsRisques });
                    });
                    traiteDto.TraiteInfoRsqVen.TraiteRisques = lstTraiteRisqueDto;

                    List<TraiteVentilationDto> lstTraiteVentilationDto = new List<TraiteVentilationDto>();
                    engagementTraite.ForEach(elem =>
                    {
                        engagementTraite.Where(ven => ven.IdVentilation == elem.IdVentilation).ToList().ForEach(ventilation =>
                        {
                            lstTraiteVentilationDto.Add(new TraiteVentilationDto { IdVentilation = ventilation.IdVentilation, EngagementVentilationForce = ventilation.ValueVen, VentilationSel = ventilation.SelectedVen });
                        }
                        );
                    }
                    );
                    traiteDto.TraiteInfoRsqVen.TraiteVentilations = lstTraiteVentilationDto;

                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                    {
                        client.Channel.UpdateTraite(codeOffre, version, type, traiteDto, GetUser(), codePeriode);
                    }
                    ConditionsGarantieController.CallIsInHpeng(codeOffre,
                                                               version,
                                                               type,
                                                               ModeConsultation.Historique.AsCode(),
                                                               codeAvn);
                }

                string id = string.Format("{0}_{1}_{2}_{3}", codeOffre, version, type, codeTraite);
                model.PageTitle = "Détail engagement";
                model.ModeNavig = modeNavig;
                model.NumAvenantPage = codeAvn;
                model.CodePeriodeEng = codePeriode;
                model.AccessMode = accesMode;
                LoadInfoPage(id);
            }
        }

        private void UpdateCommentaireOnly(string codeOffre, string version, string type, string argModelCommentForce, string codePeriode)
        {
            if (!string.IsNullOrEmpty(argModelCommentForce))
            {
                argModelCommentForce = argModelCommentForce.Replace("\r\n", "<br>").Replace("\n", "<br>");
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                client.Channel.SaveEngagementCommentaire(codeOffre, version, type, argModelCommentForce, codePeriode);
            }
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string argModelCommentForce, string codeSMP,
            string codeTraite, string tabGuid, string paramRedirect, string modeNavig, string addParamType, string addParamValue, string codePeriodeEng, string accessMode, AccessMode actionEngagement)
        {
            this.model.CurrentAccessMode = actionEngagement;
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (!IsReadonly && !IsModifHorsAvenant)
            {
                UpdateCommentaireOnly(codeOffre, version, type, argModelCommentForce, codePeriodeEng);
            }
            if (paramRedirect.ContainsChars())
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (cible == "RechercheSaisie")
            {
                return RedirectToAction(job, cible);
            }

            string[] contextInfos = null;
            if (codeSMP.ContainsChars())
            {
                contextInfos = new[] { codeSMP };
            }
            else if (cible?.ToLower() == "engagementtraite")
            {
                contextInfos = new[] { codeTraite + "cdPeriode" + codePeriodeEng + "cdPeriode" };
            }
            else if (codePeriodeEng.ContainsChars())
            {
                contextInfos = new[] { "cdPeriode" + codePeriodeEng + "cdPeriode" };
            }
            return RedirectToAction(job, cible, new
            {
                id = AlbParameters.BuildFullId(
                    new Folder(new[] { codeOffre, version, type }),
                    contextInfos,
                    tabGuid,
                    addParamValue,
                    modeNavig,
                    new[] { accessMode.IsEmptyOrNull() ? string.Empty : "accessMode" + accessMode + "accessMode" }),
                actionEngagement
            });
        }

        #region Méthode Privée
        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            switch (tId[2])
            {
                case AlbConstantesMetiers.TYPE_OFFRE:
                    model.Offre = AlbSessionHelper.GetCurrentEntityById(tId[0] + "_" + tId[1] + "_" + tId[2]);
                    if (model.Offre == null)
                    {
                        model.Offre = new Offre_MetaModel();
                        using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                        {
                            var CommonOffreClient = chan.Channel;
                            model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                        }
                    }

                    LoadDataTraite(id);
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    break;
                case AlbConstantesMetiers.TYPE_CONTRAT:
                    model.Contrat = AlbSessionHelper.GetCurrentEntityById(tId[0] + "_" + tId[1] + "_" + tId[2]);
                    if (model.Contrat == null)
                    {
                        using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                        {
                            var CommonOffreClient = chan.Channel;
                            var infosBase = CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
                            model.Contrat = new ContratDto()
                            {
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
                                Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                                Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                            };
                        }
                    }
                    LoadDataTraite(id);
                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    //model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    break;
            }
            model.PageTitle = "Détail engagement";
            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            SetArbreNavigation();
            Model.Bandeau = null;
            SetBandeauNavigation(id);

            Model.IsReadOnly = IsReadonly;
            Model.IsModifHorsAvenant = IsModifHorsAvenant;
        }

        private TraiteDto GetDataTraite(string id)
        {
            string traite = string.Empty;

            string[] tId = id.Split('_');

            if (!string.IsNullOrEmpty(id) && id.Split('_').Length > 3)
                traite = id.Split('_')[3].Trim();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                return serviceContext.InitTraite(tId[0], tId[1], tId[2], model.NumAvenantPage, traite, AlbEnumInfoValue.GetEnumValue<ModeConsultation>(model.ModeNavig), model.IsReadOnly, GetUser(), model.ActeGestion, model.CodePeriodeEng, model.AccessMode);
            }
        }

        private void LoadDataTraite(string id)
        {
            ModeleEngagementTraitePage modele = new ModeleEngagementTraitePage();

            string traite = string.Empty;
            if (!string.IsNullOrEmpty(id) && id.Split('_').Length > 3)
            {
                traite = id.Split('_')[3].Trim();
            }
            string[] tId = id.Split('_');

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                TraiteDto result = null;
                DateTime? finEffetDossier = new DateTime();
                string codePeriode = string.Empty;
                if (model.Offre != null)
                {
                    result = serviceContext.InitTraite(model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, model.NumAvenantPage, traite, model.ModeNavig.ParseCode<ModeConsultation>(), model.IsReadOnly, GetUser(), model.ActeGestion, model.CodePeriodeEng, model.AccessMode);
                    finEffetDossier = model.Offre.DateFinEffetGarantie;
                    if (!finEffetDossier.HasValue)
                    {
                        finEffetDossier = AlbConvert.GetFinPeriode(modele.DDebutEffet, model.Offre.DureeGarantie.HasValue ? model.Offre.DureeGarantie.Value : 0, model.Offre.UniteDeTemps != null ? model.Offre.UniteDeTemps.Code : string.Empty);
                    }
                    codePeriode = model.Offre.Periodicite.Code;
                }
                else if (model.Contrat != null)
                {
                    result = serviceContext.InitTraite(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.NumAvenantPage, traite, model.ModeNavig.ParseCode<ModeConsultation>(), model.IsReadOnly, GetUser(), model.ActeGestion, model.CodePeriodeEng, model.AccessMode);
                    finEffetDossier = AlbConvert.ConvertStrToDate(string.Format("{0}/{1}/{2}", model.Contrat.FinEffetJour, model.Contrat.FinEffetMois, model.Contrat.FinEffetAnnee));
                    if (!finEffetDossier.HasValue)
                    {
                        finEffetDossier = AlbConvert.GetFinPeriode(result.DDebutEffet, model.Contrat.DureeGarantie, model.Contrat.UniteDeTemps);
                    }
                    codePeriode = model.Contrat.PeriodiciteCode;
                }

                if (result != null)
                {
                    modele = ((ModeleEngagementTraitePage)result);
                }
                model.NomTraite = modele.NomTraite;
                model.CodeTraite = traite;
                model.DDebutEffet = modele.DDebutEffet;
                model.DFinEffet = modele.DFinEffet;
                model.EngagementTotal = modele.EngagementTotal;
                model.PartAlb = modele.PartAlb;
                model.EngagementAlbingia = modele.EngagementAlbingia;
                model.CommentForce = modele.CommentForce;

                if (!model.DFinEffet.HasValue)
                {
                    model.DFinEffet = finEffetDossier;
                }

                List<AlbSelectListItem> unites = result.LCIUnites.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                //if (!string.IsNullOrEmpty(model.LCIUnite))
                //{
                //    var sItem = unites.FirstOrDefault(x => x.Value == model.LCIUnite);
                //    if (sItem != null)
                //        sItem.Selected = true;
                //}
                //model.LCIUnites = unites;
                if (!string.IsNullOrEmpty(result.LCIUnite))
                {
                    var sItem = unites.FirstOrDefault(x => x.Value == result.LCIUnite);
                    if (sItem != null)
                        sItem.Selected = true;
                }

                List<AlbSelectListItem> types = result.LCITypes.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                //if (!string.IsNullOrEmpty(model.LCIType))
                //{
                //    var sItem = types.FirstOrDefault(x => x.Value == model.LCIType);
                //    if (sItem != null)
                //        sItem.Selected = true;
                //}
                //model.LCITypes = types;
                if (!string.IsNullOrEmpty(result.LCIType))
                {
                    var sItem = types.FirstOrDefault(x => x.Value == result.LCIType);
                    if (sItem != null)
                        sItem.Selected = true;
                }
                model.TraiteInfoRsqVen = modele.TraiteInfoRsqVen;

                //Région LCI Generale
                model.LCIGenerale = new Models.ModelesLCIFranchise.ModeleLCIFranchise
                {
                    TypeVue = AlbConstantesMetiers.ExpressionComplexe.LCI,
                    TypeAppel = AlbConstantesMetiers.TypeAppel.Generale,
                    Valeur = result.LCIValeur.ToString(),
                    Unite = result.LCIUnite,
                    Unites = unites,
                    Type = result.LCIType,
                    Types = types,
                    IsIndexe = result.LCIIndexee,
                    LienComplexe = result.LienComplexeLCI.ToString(),
                    LibComplexe = result.LibComplexeLCI,
                    CodeComplexe = result.CodeComplexeLCI
                };
                modele.LCIGenerale = model.LCIGenerale;
            }
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
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null)
                {
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
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
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
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
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Engagement");
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Engagement");
            }
        }

        private void SetCdPeriodeGuidTab(string id, out string outid)
        {
            model.CodePeriodeEng = id.Contains("cdPeriode") ? id.Split(new[] { "cdPeriode" }, StringSplitOptions.None)[1] : string.Empty;
            outid = id.Replace("cdPeriode" + model.CodePeriodeEng + "cdPeriode", string.Empty);
        }
        #endregion
    }
}
