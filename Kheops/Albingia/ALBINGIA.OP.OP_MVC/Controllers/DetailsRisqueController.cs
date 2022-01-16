using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using Mapster;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Ecran.DetailsObjetRisque;
using OP.WSAS400.DTO.Ecran.DetailsRisque;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OPServiceContract.IAdministration;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class DetailsRisqueController : RisqueController<ModeleDetailsRisquePage>
    {
        public override bool? IsAvnDisabled
        {
            get
            {
                if (IsReadonly || IsModifHorsAvenant || !this.model.IsModeAvenant)
                {
                    return null;
                }
                if (!(this.model.IsTraceAvnExist || this.model.IsAvenantModificationLocale || this.model.IsAvnRefreshUserUpdate || this.model.IsAvnRefreshUserUpdate))
                {
                    return true;
                }
                return false;
            }
        }

        public override bool IsReadonly
        {
            get
            {
                this.model.InitPoliceId();
                return base.IsReadonly || this.model.IsRisqueSortiAvn || this.model.HasAllObjetsSortisAvn;
            }
        }

        [ErrorHandler]
        [WhitespaceFilter]
        [AlbVerifLockedOffer("id")]
        public ActionResult Index(string id)
        {
            InitModeles();
            id = InitializeParams(id);

            return LoadView(id);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        [WhitespaceFilter]
        public RedirectToRouteResult DetailsRisqueEnregistrer(ModeleDetailsRisquePage model)
        {
            var numAvn = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNID);
            var folder = string.Format("{0}_{1}_{2}", model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type);
            if (AllowUpdate) {
                string resultCode = EnregistrementEnteteRisque(model);
                if (!string.IsNullOrEmpty(model.txtParamRedirect))
                {
                    var tabParamRedirect = model.txtParamRedirect.Split('/');
                    return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                }
                if (model.RedirectRisque == AlbOpConstants.REDIRECTRSQ_OPEN_IS)
                {
                    return null;
                }
                if (model.RedirectRisque == AlbOpConstants.REDIRECTRSQ_ADD_OBJ)
                {
                    return RedirectToAction("Index", "DetailsObjetRisque", new { id = model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type + "_" + resultCode + "_" + GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue) + GetFormatModeNavig(model.ModeNavig), returnHome = model.txtSaveCancel, guidTab = model.TabGuid });
                }
                else if (model.RedirectRisque == AlbOpConstants.REDIRECTRSQ_OPEN_OBJ && !string.IsNullOrEmpty(model.OpenObjet))
                {
                    return RedirectionDetailsObjet(model.Offre.CodeOffre, model.Offre.Version, model.Offre.Type, model.OpenObjet, model.TabGuid, model.AddParamType, model.AddParamValue, model.ModeNavig);
                }
                else
                {
                    return RedirectToAction("Index", "ChoixClauses", new { id = model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type + "_¤DetailsRisque¤Index¤" + model.Offre.CodeOffre + "£" + model.Offre.Version + "£" + model.Offre.Type + "£" + model.Code + GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue) + GetFormatModeNavig(model.ModeNavig), returnHome = model.txtSaveCancel, guidTab = model.TabGuid });
                }
            }

            if (!string.IsNullOrEmpty(model.txtParamRedirect))
            {
                var tabParamRedirect = model.txtParamRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            if (model.RedirectRisque == AlbOpConstants.REDIRECTRSQ_OPEN_IS)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(model.OpenObjet))
            {
                return RedirectionDetailsObjet(model.Offre.CodeOffre, model.Offre.Version, model.Offre.Type, model.OpenObjet, model.TabGuid, model.AddParamType, model.AddParamValue + (model.IsForceReadOnly ? "||FORCEREADONLY|1" : ""), model.ModeNavig);
            }
            else
            {
                return RedirectToAction("Index", "ChoixClauses", new { id = model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type + "_¤DetailsRisque¤Index¤" + model.Offre.CodeOffre + "£" + model.Offre.Version + "£" + model.Offre.Type + "£" + model.Code + GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue + (model.IsForceReadOnly ? "||FORCEREADONLY|1" : "")) + GetFormatModeNavig(model.ModeNavig), returnHome = model.txtSaveCancel, guidTab = model.TabGuid });
            }
        }

        [ErrorHandler]
        public string EnregistrementEnteteRisque(ModeleDetailsRisquePage model)
        {
            var folder = string.Format("{0}_{1}_{2}", model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type);
            if (AllowUpdate)
            {
                DetailsRisqueSetQueryDto query = new DetailsRisqueSetQueryDto();
                query.Offre = new OffreDto();

                query.Offre.CodeOffre = model.Offre.CodeOffre;
                query.Offre.Version = model.Offre.Version;
                query.Offre.Type = model.Offre.Type;
                query.Offre.NumAvenant = string.IsNullOrEmpty(model.NumAvenantPage) ? 0 : int.Parse(model.NumAvenantPage);

                List<RisqueDto> risques = new List<RisqueDto>();
                risques.Add(buildModifierOffre(model));
                query.Offre.Risques = risques;

                DetailsRisqueSetResultDto result = null;
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var screenClient = client.Channel;
                    result = screenClient.DetailsRisqueSet(query, GetUser());

                    if (string.IsNullOrEmpty(model.RedirectRisque) && !IsModifHorsAvenant)
                    {
                        var retGenClause = screenClient.GenerateClause(model.Offre.Type, model.Offre.CodeOffre, Convert.ToInt32(model.Offre.Version),
                                                         new ParametreGenClauseDto { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque), NuRisque = Convert.ToInt32(model.Code) });
                        if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                        {
                            throw new AlbFoncException(retGenClause.MsgErreur);
                        }
                    }

                }
                if (result != null)
                    return result.Code;
            }
            return string.Empty;
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult DetailsRisquesSupprimer(string id, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {

            string codeOffre = id.Split('_')[0];
            int? version = Convert.ToInt32(id.Split('_')[1]);
            string type = id.Split('_')[2];
            int codeRisque = Convert.ToInt32(id.Split('_')[3]);
            string codeBranche = id.Split('_')[4];

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient = client.Channel;
                DetailsRisqueDelQueryDto query = new DetailsRisqueDelQueryDto();
                List<RisqueDto> risques = new List<RisqueDto>();
                risques.Add(new RisqueDto { Code = codeRisque });
                query.offre = new OffreDto { CodeOffre = codeOffre, Version = version, Branche = new BrancheDto { Code = codeBranche, Cible = new CibleDto() }, Risques = risques, Type = type };

                screenClient.DetailsRisqueDel(query);
                return RedirectToAction("Index", "MatriceRisque", new { id = codeOffre + "_" + version.ToString() + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
            }

        }

        [ErrorHandler]
        [WhitespaceFilter]
        public RedirectToRouteResult DetailsRisqueCopier(string id)
        {
            var paramId = InitializeParams(id);
            string[] tId = paramId.Split('_');
            using (
                var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var policeServicesClient = client.Channel;
                Int32.TryParse(tId[1], out int version);
                Int32.TryParse(tId[3], out int numRsq);
               

                var newNumRsq = policeServicesClient.CopierRisque(tId[0], numRsq, tId[4], GetUser(), version, tId[2]);
                if (newNumRsq == 0)
                {
                    new AlbException(new Exception("Aucun risque n'a été copié"));
                }
                return RedirectToAction("Index", "MatriceRisque", new { id });
            }
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string codeRisque,string CopieBnsPb, string codeObjet, string tabGuid, string modeNavig, string addParamType, string addParamValue, string readonlyDisplay, bool isModeConsultationEcran, bool isForceReadOnly)
        {
            if (cible == "DetailsObjetRisque")
            {
                return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + codeRisque + "_" + codeObjet + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
            }
            if (cible == "DetailsRisque")
            {
                return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + codeRisque + "_" + CopieBnsPb + tabGuid + BuildAddParamString(addParamType, addParamValue + (readonlyDisplay == "true" ? "||FORCEREADONLY|1" : "||AVNREFRESHUSERUPDATE|1||IGNOREREADONLY|1")) + GetFormatModeNavig(model.ModeNavig) });
            }
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            var isOffreReadonly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn);

            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue + ((isModeConsultationEcran || isOffreReadonly) && !isForceReadOnly ? string.Empty : "||IGNOREREADONLY|1")) + GetFormatModeNavig(modeNavig) });
        }

        [ErrorHandler]
        public void DeleteFormuleGarantie(string codeOffre, string version, string type, string codeRsq, string codeOption)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = client.Channel;
                serviceContext.DeleteFormuleGarantieRsq(codeOffre, version, type, codeRsq, "R");
            }
        }

        [ErrorHandler]
        public string LoadCodeClass(string codeActivite)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = client.Channel;
                return serviceContext.LoadComplementNum1("KHEOP", "TREAC", codeActivite);
            }
        }

        [ErrorHandler]
        public string LoadCodeClassByCible(string codeCible, string codeActivite)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = client.Channel;
                return serviceContext.LoadCodeClassByCible(codeCible, codeActivite);
            }
        }

        [ErrorHandler]
        public ActionResult ReloadNomenclaturesCombo(string codeOffre, string version, string type, string codeRisque, string codeObjet, string cible)
        {
            ModeleInformationsGeneralesRisqueNomenclatures toReturn = null;

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient = client.Channel;
                var result = screenClient.GetListesNomenclatureOnly(codeOffre, version, type, codeRisque, codeObjet, cible);
                if (result != null)
                {
                    toReturn = LoadListesNomenclatures(result.Nomenclatures1, result.Nomenclatures2, result.Nomenclatures3, result.Nomenclatures4, result.Nomenclatures5, toReturn);
                }
            }
            if (toReturn == null)
                toReturn = new ModeleInformationsGeneralesRisqueNomenclatures();
            if (string.IsNullOrEmpty(codeObjet) || codeObjet == "0")
                toReturn.IsModeObjet = false;
            else
                toReturn.IsModeObjet = true;
            return PartialView("ListesNomenclature", toReturn);
        }

        [ErrorHandler]
        public ActionResult LoadSpecificNomenclatureCombo(Int64 idNomenclatureParent, int numeroCombo, string cible, string idNom1, string idNom2, string idNom3, string idNom4, string idNom5)
        {
            ModeleListeNomenclatures toReturn = new ModeleListeNomenclatures();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient = client.Channel;
                var result = screenClient.GetSpecificListeNomenclature(idNomenclatureParent, numeroCombo, cible, idNom1, idNom2, idNom3, idNom4, idNom5);
                if (result != null && result.Any())
                {
                    var tempNomen1 = result != null ? result.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
                    if (tempNomen1 != null && tempNomen1.Any())
                        toReturn.Nomenclatures = tempNomen1.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
                    else
                        toReturn.Nomenclatures = new List<AlbSelectListItem>();

                    toReturn.LibelleNomenclature = result != null && result.Any() && !string.IsNullOrEmpty(result.FirstOrDefault().LibelleCombo) ? result.FirstOrDefault().LibelleCombo : "Nomenclature 1";
                    toReturn.NiveauNomenclature = result != null && result.Any() ? result.FirstOrDefault().NiveauCombo : string.Empty;
                    toReturn.NomenclatureReadOnly = true;
                    toReturn.NumeroCombo = numeroCombo.ToString();
                }
                else
                {
                    return null;
                }
            }

            return PartialView("DropDownNomenclature", toReturn);
        }
        [ErrorHandler]
        public ActionResult GetActivites(string codeBranche, string codeCible, int pageNumber, string code, string nom, bool search)
        {
            ModeleRechercheActivite model = new ModeleRechercheActivite();
            model = AlbTransverse.GetListActivites(codeBranche, codeCible, pageNumber, code, nom);
            if (!string.IsNullOrEmpty(code) || !string.IsNullOrEmpty(nom) || search)
            {
                return PartialView("RechercheActivite/ResultRechecheActivite", model);
            }
            return PartialView("RechercheActivite/RechercheActiv", model);
        }

        #region Méthode Privée

        private ActionResult LoadView(string id)
        {
            model.Bandeau = null;
            DetailsRisqueGetResultDto infos = new DetailsRisqueGetResultDto();
            string[] tId = id.Split('_');
            var folder = new Folder(id.Split('_'));
            string numRsq = folder.OriginalArray[3];
            if (tId[2] == "O")
            {
                model.Offre = new Offre_MetaModel();

                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var CommonOffreClient = chan.Channel;
                    model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                }

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var policeServicesClient = client.Channel;
                    if (string.IsNullOrEmpty(tId[3].ToString()))
                        tId[3] = "0";

                    infos = policeServicesClient.GetInfoDetailRsq(tId[0], tId[1], tId[2], tId[3], "0", model.ModeNavig.ParseCode<ModeConsultation>(), model.NumAvenantPage,
                        ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Any(el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()), model.Offre.Branche.Code, model.Offre.Branche.Cible.Code, MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                    model.Offre.Risques = infos.Offre.Risques;
                    model.Offre.CountRsq = infos.Offre.CountRsq;
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                }
            }
            else if (tId[2] == "P")
            {

                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var CommonOffreClient = chan.Channel;
                    var infosBase = CommonOffreClient.LoadaffaireNouvelleBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
                    model.Contrat = new ContratDto()
                    {
                        CodeContrat = infosBase.CodeOffre,
                        VersionContrat = Convert.ToInt64(infosBase.Version),
                        Type = infosBase.Type,
                        Branche = infosBase.Branche.Code,
                        BrancheLib = infosBase.Branche.Nom,
                        Cible = infosBase.Branche.Cible.Code,
                        CibleLib = infosBase.Branche.Cible.Nom,
                        // CourtierGestionnaire = infosBase.CabinetGestionnaire.Code,
                        Descriptif = infosBase.Descriptif,
                        //CodeInterlocuteur = infosBase.CabinetGestionnaire.Code,
                        NomInterlocuteur = infosBase?.CabinetGestionnaire?.Inspecteur,
                        CodePreneurAssurance = Convert.ToInt32(infosBase.PreneurAssurance.Code),
                        NomPreneurAssurance = infosBase.PreneurAssurance.NomAssure,
                        PeriodiciteCode = infosBase.Periodicite,
                        DateEffetAvenant = AlbConvert.ConvertIntToDate(infosBase.DateAvnAnnee * 10000 + infosBase.DateAvnMois * 100 + infosBase.DateAvnJour),

                        DateEffetAnnee = infosBase.DateEffetAnnee,
                        DateEffetMois = infosBase.DateEffetMois,
                        DateEffetJour = infosBase.DateEffetJour,
                        DateEffetHeure = infosBase.DateEffetHeure,

                        FinEffetAnnee = infosBase.FinEffetAnnee,
                        FinEffetMois = infosBase.FinEffetMois,
                        FinEffetJour = infosBase.FinEffetJour,
                        FinEffetHeure = infosBase.FinEffetHeure,

                        DureeGarantie = infosBase.Duree,
                        UniteDeTemps = infosBase.UniteTemps,
                        Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                        Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur,
                    };
                }

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var serviceContext = client.Channel;
                    infos = serviceContext.GetInfoDetailRsq(
                        tId[0], tId[1], tId[2], tId[3], "0",
                        model.ModeNavig.ParseCode<ModeConsultation>(),
                        model.NumAvenantPage,
                        ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Any(el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()),
                        model.Contrat.Branche,
                        model.Contrat.Cible,
                        MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                    //model.Contrat.Risques = CastObjectServices.ConvertToSpecificRisqueDtoAffNouv(infos.Offre.Risques);
                    model.Contrat.Risques = infos.Offre.Risques;
                    //model.Contrat.DateEffetAvenant = infos.DateEffetAvn;
                    model.DateEffetAvenantModificationLocale = infos.DateModifRsqAvn;
                    model.Contrat.ProchaineEchAnnee = Convert.ToInt16(infos.EchAnnee);
                    model.Contrat.ProchaineEchMois = Convert.ToInt16(infos.EchMois);
                    model.Contrat.ProchaineEchJour = Convert.ToInt16(infos.EchJour);
                    model.Contrat.CountRsq = infos.Offre.CountRsq;
                    model.ObjetsRisqueMetaData.DateMinModificationObjet =  infos.Offre.Risques.FirstOrDefault(x => x.Code.ToString() == numRsq).Objets.Min(x => x.DateModifAvenantModificationLocale);   //.FirstOrDefault(x => x.Code.ToString() == numObj).DateModifAvenantModificationLocale;
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.IsModeAvenant = true;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.IsModeAvenant = true;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.IsModeAvenant = true;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.IsModeAvenant = true;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.IsModeAvenant = true;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                }
            }

            // assigne flag Modif hors avn before others processes using it
            this.model.IsModifHorsAvenant = IsModifHorsAvenant;

            if (model.Offre != null || model.Contrat != null)
            {
                setDataDetailsRisque(id);
            }

            // Le cas d'un contrat
            if (model.Contrat != null)
            {
                // Une trace avenant existe-elle en base ou l'utilisateur vient-il de cocher la case ?
                if (model.IsTraceAvnExist || model.IsAvnRefreshUserUpdate || model.IsAvnRefreshUserUpdate)
                {
                    model.IsAvenantModificationLocale = true;
                }

                //if (model.IsModeAvenant && !model.IsAvenantModificationLocale && !model.IsForceReadOnly) {
                //    return RedirectToAction("Index", "DetailsRisque", new { id = model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type + "_" + model.Code + GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue + "||FORCEREADONLY|1") + GetFormatModeNavig(model.ModeNavig) });
                //}
                if (model.IsTraceAvnExist || model.IsAvnRefreshUserUpdate || model.IsAvnRefreshUserUpdate)
                {
                    if (!model.DateEffetAvenantModificationLocale.HasValue)
                    {
                        model.DateEffetAvenantModificationLocale = model.InformationsGenerales.DateEntreeGarantie > model.DateEffetAvenant ? model.InformationsGenerales.DateEntreeGarantie : model.DateEffetAvenant;
                    }
                }
            }

            if (infos != null)
            {
                model.HasFormules = infos.HasFormules;
                var cibles = infos.Cibles.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Description), Descriptif = m.Description, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Description) }).ToList();
                if (!string.IsNullOrEmpty(model.InformationsGenerales.Cible))
                {
                    var sItem = cibles.FirstOrDefault(x => x.Value == model.InformationsGenerales.Cible);
                    if (sItem != null)
                    {
                        sItem.Selected = true;
                    }
                }
                model.InformationsGenerales.Cibles = cibles;
                if (infos.IsExistLoupe)
                {
                    model.InformationsGenerales.IsExistLoupe = true;
                }

                var unites = infos.Unites.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (!string.IsNullOrEmpty(model.Unite))
                {
                    var sItem = unites.FirstOrDefault(x => x.Value == model.Unite);
                    if (sItem != null)
                    {
                        sItem.Selected = true;
                    }
                }
                model.Unites = unites;

                var types = infos.Types.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle), Descriptif = m.CodeTpcn1.ToString() }).ToList();
                if (!string.IsNullOrEmpty(model.Type))
                {
                    var sItem = types.FirstOrDefault(x => x.Value == model.Type);
                    if (sItem != null)
                    {
                        sItem.Selected = true;
                    }
                }
                model.Types = types;

                var valeursHT = new List<AlbSelectListItem>
                                            {
                                                new AlbSelectListItem {Text = "H - HT", Value = "H", Selected = false, Title = "H - HT"},
                                                new AlbSelectListItem {Text = "T - TTC", Value = "T", Selected = false, Title = "T - TTC"}
                                            };
                if (!string.IsNullOrEmpty(model.ValeurHT))
                {
                    var sItem = valeursHT.FirstOrDefault(x => x.Value == model.ValeurHT);
                    if (sItem != null)
                    {
                        sItem.Selected = true;
                    }
                }
                model.ValeursHT = valeursHT;

                //Assignation des libellés
                if (model.ObjetsRisqueMetaData != null && model.ObjetsRisqueMetaData.Objets != null
                    && model.Unites != null && model.Types != null && model.ValeursHT != null)
                {
                    foreach (var obj in model.ObjetsRisqueMetaData.Objets)
                    {
                        //Unité libellé
                        if (model.Unites.Find(elm => elm.Value == obj.Unite) != null)
                        {
                            string unite = model.Unites.Find(elm => elm.Value == obj.Unite).Text;
                            if (!string.IsNullOrEmpty(unite) && unite.Split('-').Length > 1)
                            {
                                obj.UniteLibelle = unite.Split('-')[1].Trim();
                            }
                        }
                        //Type libellé
                        if (model.Types.Find(elm => elm.Value == obj.Type) != null)
                        {
                            string type = model.Types.Find(elm => elm.Value == obj.Type).Text;
                            if (!string.IsNullOrEmpty(type) && type.Split('-').Length > 1)
                            {
                                obj.TypeLibelle = type.Split('-')[1].Trim();
                            }
                        }
                        //Valeur HT libellé
                        if (model.ValeursHT.Find(elm => elm.Value == obj.ValeurHT) != null)
                        {
                            string valHT = model.ValeursHT.Find(elm => elm.Value == obj.ValeurHT).Text;
                            if (!string.IsNullOrEmpty(valHT) && valHT.Split('-').Length > 1)
                            {
                                obj.ValeurHTLibelle = valHT.Split('-')[1].Trim();
                            }
                        }
                    }
                    model.ObjetsRisqueMetaData.DateModificationAvenant = model.DateEffetAvenantModificationLocale;
           
                   
                }

                // Nomenclature de risques
                model.InformationsGenerales.CodesApe = infos.CodesApe.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.InformationsGenerales.CodesTre = infos.CodesTre.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.InformationsGenerales.CodesClasse = infos.CodesClasse.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.InformationsGenerales.Territorialites = infos.Territorialites.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();

                model.InformationsGenerales.ListesNomenclatures = LoadListesNomenclaturesNew(infos.Nomenclatures1, infos.Nomenclatures2, infos.Nomenclatures3, infos.Nomenclatures4, infos.Nomenclatures5, model.InformationsGenerales.ListesNomenclatures);

                model.InformationsGenerales.TypesRisque = infos.TypesRisque.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.InformationsGenerales.TypesMateriel = infos.TypesMateriel.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.InformationsGenerales.NaturesLieux = infos.NaturesLieux.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();

                model.InformationsGenerales.ModeRisque = true;
                int monoObjet;
                int.TryParse(model.MonoObjet, out monoObjet);
                model.InformationsGenerales.ModeMultiObjet = monoObjet == 0;

                model.DateDebHisto = AlbConvert.ConvertDateToStr(infos.DateDebHisto);
                model.HeureDebHisto = AlbConvert.GetTimeFromDate(infos.DateDebHisto).ToString();

            }
            //  }

            model.PageTitle = string.Format("Risque {0}", model.Code);
            if (this.model.ObjetsRisqueMetaData != null)
            {
                this.model.ObjetsRisqueMetaData.IsReadOnly = model.IsReadOnly;
                // Tester sur le modification hors avenant
                this.model.ObjetsRisqueMetaData.IsModifHorsAvenant = model.IsModifHorsAvenant;
                this.model.ObjetsRisqueMetaData.IsModeAvenant = model.IsModeAvenant;
                this.model.ObjetsRisqueMetaData.IsIndexe = model.IsIndexe;
                this.model.ObjetsRisqueMetaData.IsAvnDisabled = IsAvnDisabled.GetValueOrDefault();
            }
            model.InformationsGenerales.ListesNomenclatures.Nomenclature1.NomenclatureReadOnly = model.InformationsGenerales.ListesNomenclatures.Nomenclature1.Nomenclatures.Any() && (model.InformationsGenerales.ReadOnly || model.InformationsGenerales.NomenclatureRisqueModifiable);
            model.InformationsGenerales.ListesNomenclatures.Nomenclature2.NomenclatureReadOnly = model.InformationsGenerales.ListesNomenclatures.Nomenclature2.Nomenclatures.Any() && (model.InformationsGenerales.ReadOnly || model.InformationsGenerales.NomenclatureRisqueModifiable);
            model.InformationsGenerales.ListesNomenclatures.Nomenclature3.NomenclatureReadOnly = model.InformationsGenerales.ListesNomenclatures.Nomenclature3.Nomenclatures.Any() && (model.InformationsGenerales.ReadOnly || model.InformationsGenerales.NomenclatureRisqueModifiable);
            model.InformationsGenerales.ListesNomenclatures.Nomenclature4.NomenclatureReadOnly = model.InformationsGenerales.ListesNomenclatures.Nomenclature4.Nomenclatures.Any() && (model.InformationsGenerales.ReadOnly || model.InformationsGenerales.NomenclatureRisqueModifiable);
            model.InformationsGenerales.ListesNomenclatures.Nomenclature5.NomenclatureReadOnly = model.InformationsGenerales.ListesNomenclatures.Nomenclature5.Nomenclatures.Any() && (model.InformationsGenerales.ReadOnly || model.InformationsGenerales.NomenclatureRisqueModifiable);
            model.InformationsGenerales.ListesNomenclatures.IsModeObjet = false;

            model.InformationsGenerales.CodePeriodicite = model.Offre != null
                ? model.Offre.Periodicite.Code
                : model.Contrat != null ? model.Contrat.PeriodiciteCode : string.Empty;

            if (model.IsModeAvenant)
            {
                if (model.InformationsGenerales.DateSortieGarantie.HasValue)
                {
                    DateTime? finEffetRisqueContrat = null;
                    //DateTime? finEffetContrat = AlbConvert.ConvertIntToDate(model.Contrat.FinEffetAnnee * 10000 + model.Contrat.FinEffetMois * 100 + model.Contrat.FinEffetJour);
                    DateTime? finEffetContrat = AlbConvert.ConvertIntToDateHour(Convert.ToInt64(model.Contrat.FinEffetAnnee) * 100000000 + Convert.ToInt64(model.Contrat.FinEffetMois) * 1000000 + Convert.ToInt64(model.Contrat.FinEffetJour) * 10000 + Convert.ToInt64(model.Contrat.FinEffetHeure));
                    finEffetRisqueContrat = finEffetContrat;

                    if (finEffetRisqueContrat.HasValue)//Si date de fin trouvée
                    {
                        if (model.DateEffetAvenantModificationLocale.HasValue)
                        {
                            model.InformationsGenerales.IsDateFinGarantieModifiable = model.InformationsGenerales.DateSortieGarantie.Value >= model.DateEffetAvenantModificationLocale.Value.AddDays(-1) && model.InformationsGenerales.DateSortieGarantie.Value <= finEffetRisqueContrat.Value;
                        }
                        else
                        {
                            model.InformationsGenerales.IsDateFinGarantieModifiable = false;
                        }
                    }
                    else//Sinon = tacite, pas de date de fin effet
                    {
                        model.InformationsGenerales.IsDateFinGarantieModifiable = true;
                    }
                }
                else
                {
                    model.InformationsGenerales.IsDateFinGarantieModifiable = true;
                }
            }
            else
            {
                model.InformationsGenerales.IsDateFinGarantieModifiable = true;
            }

            // store date avn + date fin to check change on update
            this.model.InitDatesCopy();

            return View(this.model);
        }

        private RisqueDto buildModifierOffre(ModeleDetailsRisquePage model)
        {
            int monoObjet;
            int.TryParse(model.MonoObjet, out monoObjet);
            var toReturn = new RisqueDto
            {
                Descriptif = string.IsNullOrEmpty(model.InformationsGenerales.Descriptif) ? string.Empty : model.InformationsGenerales.Descriptif,
                CodeApe = monoObjet > 0 ? model.CodeApeMonoObjet : model.InformationsGenerales.CodeApe,
                Territorialite = monoObjet > 0 ? model.TerritorialiteMonoObjet : model.InformationsGenerales.Territorialite,
                CodeTre = monoObjet > 0 ? model.CodeTreMonoObjet : model.InformationsGenerales.CodeTre,
                CodeClasse = monoObjet > 0 ? model.CodeClasseMonoObjet : model.InformationsGenerales.CodeClasse,
                TypeRisque = monoObjet > 0 ? model.TypeRisque : model.InformationsGenerales.TypeRisque,
                TypeMateriel = monoObjet > 0 ? model.TypeMateriel : model.InformationsGenerales.TypeMateriel,
                NatureLieux = monoObjet > 0 ? model.NatureLieux : model.InformationsGenerales.NatureLieux,
                Nomenclature1 = model.InformationsGenerales.ListesNomenclatures.Nomenclature1.Nomenclature,
                Nomenclature2 = model.InformationsGenerales.ListesNomenclatures.Nomenclature2.Nomenclature,
                Nomenclature3 = model.InformationsGenerales.ListesNomenclatures.Nomenclature3.Nomenclature,
                Nomenclature4 = model.InformationsGenerales.ListesNomenclatures.Nomenclature4.Nomenclature,
                Nomenclature5 = model.InformationsGenerales.ListesNomenclatures.Nomenclature5.Nomenclature,
                IsTraceAvnExist = model.IsAvenantModificationLocale,
                DateEffetAvenantModificationLocale = model.DateEffetAvenantModificationLocale,
                IsRisqueTemporaire = model.InformationsGenerales.IsRisqueTemporaire
            };

            if (monoObjet == 0)
            {


                toReturn.Valeur = model.Valeur;

                toReturn.Unite = new ParametreDto
                {
                    Code = model.Unite
                };
                toReturn.Type = new ParametreDto
                {
                    Code = model.TypeObjets
                };
                toReturn.ReportValeur = (!string.IsNullOrEmpty(model.ReportValeur) ? model.ReportValeur : "N");
                toReturn.ReportObligatoire = model.ReportObligatoire;

            }

            toReturn.ChronoDesi = model.ChronoDesi;
            toReturn.Designation = !string.IsNullOrEmpty(model.InformationsGenerales.Designation) ? model.InformationsGenerales.Designation.Replace("\r\n", "<br>").Replace("\n", "<br>") : string.Empty;
            toReturn.Valeur = model.Valeur;
            toReturn.Unite = new ParametreDto { Code = model.Unite };
            toReturn.Type = new ParametreDto { Code = model.ActiverReport ? model.TypeObjets : model.Type };
            toReturn.ValeurHT = model.ValeurHT;
            //toReturn.CoutM2 = model.CoutM2;

            toReturn.Code = Convert.ToInt32(model.Code);

            if (model.InformationsGenerales.DateEntreeGarantie.HasValue)
            {
                if (model.InformationsGenerales.HeureEntreeGarantie.HasValue)
                    toReturn.EntreeGarantie = new DateTime(
                        model.InformationsGenerales.DateEntreeGarantie.Value.Year,
                        model.InformationsGenerales.DateEntreeGarantie.Value.Month,
                        model.InformationsGenerales.DateEntreeGarantie.Value.Day,
                        model.InformationsGenerales.HeureEntreeGarantie.Value.Hours,
                        model.InformationsGenerales.HeureEntreeGarantie.Value.Minutes, 0);
                else
                    toReturn.EntreeGarantie = new DateTime(
                        model.InformationsGenerales.DateEntreeGarantie.Value.Year,
                        model.InformationsGenerales.DateEntreeGarantie.Value.Month,
                        model.InformationsGenerales.DateEntreeGarantie.Value.Day,
                        0, 0, 0);
            }
            if (model.InformationsGenerales.DateSortieGarantie.HasValue)
            {
                if (model.InformationsGenerales.HeureSortieGarantie.HasValue)
                    toReturn.SortieGarantie = new DateTime(
                        model.InformationsGenerales.DateSortieGarantie.Value.Year,
                        model.InformationsGenerales.DateSortieGarantie.Value.Month,
                        model.InformationsGenerales.DateSortieGarantie.Value.Day,
                        model.InformationsGenerales.HeureSortieGarantie.Value.Hours,
                        model.InformationsGenerales.HeureSortieGarantie.Value.Minutes, 0);
                else
                    toReturn.SortieGarantie = new DateTime(
                        model.InformationsGenerales.DateSortieGarantie.Value.Year,
                        model.InformationsGenerales.DateSortieGarantie.Value.Month,
                        model.InformationsGenerales.DateSortieGarantie.Value.Day,
                        0, 0, 0);
            }

            if (model.InformationsGenerales.DateEntreeDescr.HasValue)
            {
                if (model.InformationsGenerales.HeureEntreeDescr.HasValue)
                {
                    toReturn.DateEntreeDescr = new DateTime(
                        model.InformationsGenerales.DateEntreeDescr.Value.Year,
                        model.InformationsGenerales.DateEntreeDescr.Value.Month,
                        model.InformationsGenerales.DateEntreeDescr.Value.Day,
                        model.InformationsGenerales.HeureEntreeDescr.Value.Hours,
                        model.InformationsGenerales.HeureEntreeDescr.Value.Minutes, 0);
                }
                else
                {
                    toReturn.DateEntreeDescr = new DateTime(
                        model.InformationsGenerales.DateEntreeDescr.Value.Year,
                        model.InformationsGenerales.DateEntreeDescr.Value.Month,
                        model.InformationsGenerales.DateEntreeDescr.Value.Day,
                        0, 0, 0);
                }
            }
            if (model.InformationsGenerales.DateSortieDescr.HasValue)
            {
                if (model.InformationsGenerales.HeureSortieDescr.HasValue)
                {
                    toReturn.DateSortieDescr = new DateTime(
                        model.InformationsGenerales.DateSortieDescr.Value.Year,
                        model.InformationsGenerales.DateSortieDescr.Value.Month,
                        model.InformationsGenerales.DateSortieDescr.Value.Day,
                        model.InformationsGenerales.HeureSortieDescr.Value.Hours,
                        model.InformationsGenerales.HeureSortieDescr.Value.Minutes, 0);
                }
                else
                {
                    toReturn.DateSortieDescr = new DateTime(
                        model.InformationsGenerales.DateSortieDescr.Value.Year,
                        model.InformationsGenerales.DateSortieDescr.Value.Month,
                        model.InformationsGenerales.DateSortieDescr.Value.Day,
                        0, 0, 0);
                }
            }

            toReturn.Cible = new CibleDto
            {
                Code = monoObjet > 0 ? model.CibleMonoObjet : model.InformationsGenerales.Cible
            };

            if (model.ContactAdresse == null)
            {
                toReturn.AdresseRisque = new AdressePlatDto();
            }
            else
            {
                int numVoie = 0;
                int cp = 0;
                int cpCedex = 0;
                string cpFormat = !string.IsNullOrEmpty(model.ContactAdresse.CodePostal) && model.ContactAdresse.CodePostal != "0"
                    ? model.ContactAdresse.CodePostal.Length >= 3
                        ? model.ContactAdresse.CodePostal.Substring(model.ContactAdresse.CodePostal.Length - 3, 3)
                        : model.ContactAdresse.CodePostal
                    : string.Empty;
                string cpCedexFormat = !string.IsNullOrEmpty(model.ContactAdresse.CodePostalCedex) && model.ContactAdresse.CodePostalCedex != "0"
                    ? model.ContactAdresse.CodePostalCedex.Length >= 3
                        ? model.ContactAdresse.CodePostalCedex.Substring(model.ContactAdresse.CodePostalCedex.Length - 3, 3)
                        : model.ContactAdresse.CodePostalCedex
                    : string.Empty;

                toReturn.AdresseRisque = new AdressePlatDto
                {
                    MatriculeHexavia = model.ContactAdresse.MatriculeHexavia,
                    NumeroChrono = model.ContactAdresse.NoChrono.HasValue ? model.ContactAdresse.NoChrono.Value : 0,
                    BoitePostale = model.ContactAdresse.Distribution,
                    ExtensionVoie = model.ContactAdresse.Extension,
                    NomVoie = model.ContactAdresse.Voie,
                    NumeroVoie = Int32.TryParse(model.ContactAdresse.No.Split(new char[] { '/', '-' })[0], out numVoie) ? numVoie : -1,
                    NumeroVoie2 = model.ContactAdresse.No.Contains("/") || model.ContactAdresse.No.Contains("-") ? model.ContactAdresse.No.Split(new char[] { '/', '-' })[1] : "",
                    Batiment = model.ContactAdresse.Batiment,
                    CodePostal = Int32.TryParse(cpFormat, out cp) ? cp : -1,
                    NomVille = model.ContactAdresse.Ville,
                    CodePostalCedex = Int32.TryParse(cpCedexFormat, out cpCedex) ? cpCedex : -1,
                    NomCedex = model.ContactAdresse.VilleCedex,
                    Departement = !string.IsNullOrEmpty(model.ContactAdresse.CodePostal) && model.ContactAdresse.CodePostal.Length == 5 ? model.ContactAdresse.CodePostal.Substring(0, 2) : string.Empty,
                    Latitude = !model.ContactAdresse.Latitude.IsEmptyOrNull() ? Convert.ToDecimal(model.ContactAdresse.Latitude.Replace(".", ",")) : 0,
                    Longitude = !model.ContactAdresse.Longitude.IsEmptyOrNull() ? Convert.ToDecimal(model.ContactAdresse.Longitude.Replace(".", ",")) : 0
                };
            }

            return toReturn;
        }

        private void setDataDetailsRisque(string id)
        {
            model.AfficherBandeau = base.DisplayBandeau(true, id);
            model.AfficherNavigation = model.AfficherBandeau;
            if (model.AfficherBandeau)
            {
                model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                model.Navigation = new Navigation_MetaModel();
                model.Navigation.Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE;
                if (model.Offre != null)
                {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.Navigation.IdOffre = model.Offre.CodeOffre;
                    model.Navigation.Version = model.Offre.Version;
                    model.CountRsq = model.Offre.CountRsq;
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

                    //if (model.IsModeAvenant)
                    //{
                    //    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    //}
                    //else
                    //{
                    //    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    //}
                    model.Navigation.IdOffre = model.Contrat.CodeContrat;
                    model.Navigation.Version = int.Parse(model.Contrat.VersionContrat.ToString());
                    model.CountRsq = model.Contrat.CountRsq;

                }
            }


            string[] tId = id.Split('_');
            if (!string.IsNullOrEmpty(tId[3].ToString()))
            {
                var currentRsq = new RisqueDto();

                if (model.Offre != null)
                    currentRsq = model.Offre.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString()));
                else if (model.Contrat != null)
                    //currentRsq = CastObjectServices.ConvertToSpecificRisqueDtoPolices(model.Contrat.Risques).FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString()));
                    currentRsq = model.Contrat.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString()));
                if (currentRsq != null)
                {
                    //model.DateEffetAvenantModificationLocale = currentRsq.IsTraceAvnExist ? currentRsq.DateEffetAvenantModificationLocale : null;
                    model.DateEffetAvenantModificationLocale = currentRsq.IsTraceAvnExist || (model.DateEffetAvenantModificationLocale != null && model.ModeNavig == ModeConsultation.Historique.ToString()) ? model.DateEffetAvenantModificationLocale : null;
                    model.AvnCreationRsq = currentRsq.AvnCreationRsq;
                    model.IsTraceAvnExist = currentRsq.IsTraceAvnExist || (model.DateEffetAvenantModificationLocale != null && model.ModeNavig == ModeConsultation.Historique.ToString());
                    model.IsIndexe = currentRsq.isIndexe;
                    model.Code = currentRsq.Code.ToString();
                    model.MonoObjet = currentRsq.CodeObjet.ToString();
                    //if (model.MonoObjet == "1")
                    if (currentRsq.Objets != null && currentRsq.Objets.Count == 1)
                    {
                        model.CibleMonoObjet = currentRsq.Objets[0].Cible.Code;
                        model.RisqueObj = currentRsq.Objets[0].Descriptif;
                        model.CodeApeMonoObjet = currentRsq.Objets[0].CodeApe;
                        model.CodeTreMonoObjet = currentRsq.Objets[0].CodeTre;
                        model.CodeClasseMonoObjet = currentRsq.Objets[0].CodeClasse;
                        model.TerritorialiteMonoObjet = currentRsq.Objets[0].Territorialite;
                        model.Nomenclature1MonoObjet = currentRsq.Objets[0].Nomenclature1;
                    }
                    else
                    {
                        model.RisqueObj = currentRsq.Descriptif;
                    }
                    model.ChronoDesi = currentRsq.ChronoDesi;

                    model.Valeur = currentRsq.Valeur;
                    model.Unite = currentRsq.Unite.Code;
                    model.Type = currentRsq.Type.Code;
                    model.ValeurHT = currentRsq.ValeurHT;
                    //model.CoutM2 = currentRsq.CoutM2;

                    if (currentRsq.AdresseRisque != null)
                    {
                        int depart = 0;
                        Int32.TryParse(currentRsq.AdresseRisque.Departement, out depart);
                        string codePostal = string.Empty;
                        string codePX = string.Empty;
                        //string codePostal = depart.ToString("D2") + currentObj.AdresseObjet.CodePostal.ToString("D3");
                        //string codePX = depart.ToString("D2") + currentObj.AdresseObjet.CodePostalCedex.ToString("D3");
                        if (depart > 0)
                        {
                            codePostal = depart.ToString("D2") + currentRsq.AdresseRisque.CodePostal.ToString("D3");
                            codePX = depart.ToString("D2") + currentRsq.AdresseRisque.CodePostalCedex.ToString("D3");
                        }
                        else
                        {
                            codePostal = currentRsq.AdresseRisque.CodePostal.ToString();
                            codePX = currentRsq.AdresseRisque.CodePostalCedex.ToString();
                        }

                        model.ContactAdresse = new ModeleContactAdresse
                        {
                            Batiment = currentRsq.AdresseRisque.Batiment,
                            CodePostal = codePostal,
                            CodePostalCedex = codePX,
                            MatriculeHexavia = currentRsq.AdresseRisque.MatriculeHexavia,
                            No = currentRsq.AdresseRisque.NumeroVoie == 0 ? string.Empty : currentRsq.AdresseRisque.NumeroVoie.ToString(),
                            No2 = currentRsq.AdresseRisque.NumeroVoie2,
                            NoChrono = currentRsq.AdresseRisque.NumeroChrono,
                            Pays = currentRsq.AdresseRisque.NomPays,
                            Ville = currentRsq.AdresseRisque.NomVille,
                            VilleCedex = currentRsq.AdresseRisque.NomCedex,
                            Voie = currentRsq.AdresseRisque.NomVoie,
                            Distribution = currentRsq.AdresseRisque.BoitePostale,
                            Extension = currentRsq.AdresseRisque.ExtensionVoie,
                            IsModifHorsAvn = model.IsModifHorsAvenant,
                            Latitude = currentRsq.AdresseRisque.Latitude?.ToString(),
                            Longitude = currentRsq.AdresseRisque.Longitude?.ToString()
                        };
                        //if (!string.IsNullOrEmpty(model.ContactAdresse.CodePostal) && !string.IsNullOrEmpty(currentRsq.AdresseRisque.Departement))
                        //    model.ContactAdresse.CodePostal = currentRsq.AdresseRisque.Departement + model.ContactAdresse.CodePostal;
                        //if (!string.IsNullOrEmpty(model.ContactAdresse.CodePostalCedex) && !string.IsNullOrEmpty(currentRsq.AdresseRisque.Departement))
                        //    model.ContactAdresse.CodePostalCedex = currentRsq.AdresseRisque.Departement + model.ContactAdresse.CodePostalCedex;
                    }
                    else
                        model.ContactAdresse = new ModeleContactAdresse(11, false, true);


                    if (currentRsq.Objets.Count == 1)
                    {
                        //model.InformationsGenerales = InformationsGenerales.SetInfosGenerales((InformationGeneraleTransverse)currentRsq.Objets[0]);
                        //model.InformationsGenerales.ReadOnly = true;
                        model.ContactAdresse.SaisieHexavia = false;
                    }
                    else
                    {
                        model.ContactAdresse.SaisieHexavia = true;
                    }

                    model.ContactAdresse.ReadOnly = model.IsReadOnly;

                    model.InformationsGenerales = InformationsGenerales.SetInfosGenerales((InformationGeneraleTransverse)currentRsq);
                    model.InformationsGenerales.ReadOnly = false;

                    model.InformationsGenerales.IsReadOnly = model.IsReadOnly;
                    model.InformationsGenerales.DisplayInfosValeur = false;

                    model.InformationsGenerales.IsModeAvenant = model.IsModeAvenant;
                    model.InformationsGenerales.NumAvenantCreationRsq = model.AvnCreationRsq.ToString();
                    model.InformationsGenerales.NumAvenantCourant = model.NumAvenantPage;

                    //SetReportValues(Common.CastObjectServices.ConvertToSpecificObjetDtoToPol(currentRsq.Objets.ToList()));
                    SetReportValues(currentRsq.Objets.ToList());
                    if (!string.IsNullOrEmpty(model.EnsembleType))
                    {
                        using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                        {
                            var serviceContext = client.Channel;
                            var result = serviceContext.GetListeTypeValeurComp(model.EnsembleType, string.Empty, string.Empty);
                            var typesObjets = result.Select(m => new AlbSelectListItem { Value = m.CodeTypeValeurComp, Text = string.Format("{0} - {1}", m.CodeTypeValeurComp, m.DescriptionTypeValeurComp), Selected = false, Title = string.Format("{0} - {1}", m.CodeTypeValeurComp, m.DescriptionTypeValeurComp) }).ToList();
                            if (!string.IsNullOrEmpty(model.TypeObjets))
                            {
                                var sItem = typesObjets.FirstOrDefault(x => x.Value == model.TypeObjets);
                                if (sItem != null)
                                    sItem.Selected = true;
                            }
                            model.TypesLstObjets = typesObjets;
                        }
                    }
                    else
                        model.TypesLstObjets = new List<AlbSelectListItem>();

                    if (model.Offre != null)
                    {
                        model.ObjetsRisqueMetaData.CodeOffre = model.Offre.CodeOffre;
                        model.ObjetsRisqueMetaData.Version = model.Offre.Version.ToString();
                        model.ObjetsRisqueMetaData.Type = model.Offre.Type;
                        model.ObjetsRisqueMetaData.Periodicite = model.Offre.Periodicite.Code;
                    }
                    else if (model.Contrat != null)
                    {
                        model.ObjetsRisqueMetaData.CodeOffre = model.Contrat.CodeContrat;
                        model.ObjetsRisqueMetaData.Version = model.Contrat.VersionContrat.ToString();
                        model.ObjetsRisqueMetaData.Type = model.Contrat.Type;
                        model.ObjetsRisqueMetaData.Periodicite = model.Contrat.PeriodiciteCode;
                        model.ObjetsRisqueMetaData.TypePolice = model.Contrat.TypePolice;
                    }
                    model.ObjetsRisqueMetaData.CodeRisque = currentRsq.Code.ToString();
                    model.ObjetsRisqueMetaData.LibRisque = currentRsq.Descriptif;


                    model.ObjetsRisqueMetaData.Objets = currentRsq.Objets.Select(
                        m => new ModeleDetailsObjetRisque
                        {
                            Code = m.Code,
                            Descriptif = m.Descriptif,
                            DateEntree = m.EntreeGarantie,
                            DateSortie = m.SortieGarantie,
                            IndiceOrigine = m.IndiceOrigine,
                            IndiceActualise = m.IndiceActualise,
                            Valeur = m.Valeur.ToString(),
                            Unite = m.Unite.Code,
                            Type = m.Type.Code,
                            ValeurHT = m.ValeurHT,
                            EnsembleType = m.EnsembleType,
                            HasInventaires = m.Inventaires != null && m.Inventaires.Count > 0,
                            IsAlertePeriode = m.IsAlertePeriode,
                            IsSortiAvenant = m.IsSortiAvenant,
                            IsAfficheAvenant = m.IsAfficheAvenant,
                            NumAvenantCreationRsq = m.AvnCreationObj.ToString(),
                            DateEffetAvenantOBJ = m.DateEffetAvenantOBJ


                        }
                    ).ToList();


                    if (currentRsq.Objets != null && currentRsq.Objets.Count > 1)
                    {
                        model.InformationsGenerales.CibleModifiable = (int.TryParse(model.NumAvenantPage, out int a) ? a : 0).IsIn(0, this.model.AvnCreationRsq);
                        model.InformationsGenerales.DateModifiable = true;
                        model.InformationsGenerales.NomenclatureRisqueModifiable = true;
                        if (model.ObjetsRisqueMetaData.Objets.Exists(elm => elm.IsAlertePeriode))
                            model.IsAlertePeriode = true;
                        if (model.ObjetsRisqueMetaData.Objets.Select(elm => elm.IndiceOrigine).Distinct().Count() > 1)
                            model.IsAlertePeriode = true;
                    }

                    var minDateDeb = (from o in currentRsq.Objets where o.EntreeGarantie != null select o.EntreeGarantie).Min();
                    var maxDateDeb = (from o in currentRsq.Objets where o.EntreeGarantie != null select o.EntreeGarantie).Max();
                    if (minDateDeb != null)
                    {
                        SetControlDate(minDateDeb.Value.ToString(), "DebMinObjet");
                    }
                    if (maxDateDeb != null)
                    {
                        SetControlDate(maxDateDeb.Value.ToString(), "DebMaxObjet");
                    }

                    var maxDateFin = (from o in currentRsq.Objets where o.SortieGarantie != null select o.SortieGarantie).Max();
                    var minDateFin = (from o in currentRsq.Objets where o.SortieGarantie != null select o.SortieGarantie).Min();
                    if (maxDateFin != null)
                    {
                        SetControlDate(maxDateFin.Value.ToString(), "FinMaxObjet");
                    }
                    if (minDateFin != null)
                    {
                        SetControlDate(minDateFin.Value.ToString(), "FinMinObjet");
                    }

                    var minDateAvt = (from o in currentRsq.Objets where o.EntreeGarantie != null && o.AvnCreationObj.ToString() == model.NumAvenantPage select o.EntreeGarantie).Min();
                    if (minDateAvt != null)
                    {
                        SetControlDate(minDateAvt.Value.ToString(), "DebObjetAvn");
                    }
                    var maxDateAvt = (from o in currentRsq.Objets where o.SortieGarantie != null && o.AvnCreationObj.ToString() == model.NumAvenantPage select o.SortieGarantie).Max();
                    if (maxDateAvt != null)
                    {
                        SetControlDate(maxDateAvt.Value.ToString(), "FinObjetAvn");
                    }

                    //Nomenclature de risques
                    model.InformationsGenerales.CodeApe = currentRsq.CodeApe;
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature1.Nomenclature = currentRsq.Nomenclature1;
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature2.Nomenclature = currentRsq.Nomenclature2;
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature3.Nomenclature = currentRsq.Nomenclature3;
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature4.Nomenclature = currentRsq.Nomenclature4;
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature5.Nomenclature = currentRsq.Nomenclature5;
                    model.InformationsGenerales.Territorialite = currentRsq.Territorialite;
                    model.InformationsGenerales.CodeTre = currentRsq.CodeTre;
                    model.InformationsGenerales.LibTre = currentRsq.LibTre;
                    model.InformationsGenerales.CodeClasse = currentRsq.CodeClasse;

                    model.InformationsGenerales.TypeRisque = currentRsq.TypeRisque;
                    model.InformationsGenerales.TypeMateriel = currentRsq.TypeMateriel;
                    model.InformationsGenerales.NatureLieux = currentRsq.NatureLieux;
                    model.NatureLieux = currentRsq.NatureLieux;
                    model.TypeMateriel = currentRsq.TypeMateriel;
                    model.TypeRisque = currentRsq.TypeRisque;
                }
            }
            else
            {
                model.Code = "0";
                model.MonoObjet = "1";
                model.ChronoDesi = 0;
                model.InformationsGenerales.ReadOnly = true;
                model.InformationsGenerales.IsModifHorsAvenant = model.IsModifHorsAvenant;
            }
            if (model.Offre != null)
            {
                model.Branche = model.Offre.Branche.Code;
                model.InformationsGenerales.DateDebEffet = model.Offre.DateEffetGarantie;
                model.InformationsGenerales.DateFinEffet = model.Offre.DateFinEffetGarantie;

                if (model.Offre.DateEffetGarantie.HasValue)
                {
                    if (model.Offre.DureeGarantie.HasValue && model.Offre.DureeGarantie.Value > 0 && model.Offre.UniteDeTemps != null)
                    {
                        DateTime dateFinCalcul = model.Offre.DateEffetGarantie.Value;
                        switch (model.Offre.UniteDeTemps.Code)
                        {
                            case "M":
                                model.Offre.DateFinEffetGarantie = dateFinCalcul.AddMonths(model.Offre.DureeGarantie.Value).AddMinutes(-1);
                                break;
                            case "J":
                                model.Offre.DateFinEffetGarantie = dateFinCalcul.AddDays(model.Offre.DureeGarantie.Value).AddMinutes(-1);
                                break;
                            case "A":
                                model.Offre.DateFinEffetGarantie = dateFinCalcul.AddYears(model.Offre.DureeGarantie.Value).AddMinutes(-1);
                                break;
                            default: break;
                        }
                    }
                }

                if (model.Offre.DateEffetGarantie.HasValue)
                {
                    SetControlDate(model.Offre.DateEffetGarantie.Value.ToString(), "DebOffre");
                }
                if (model.Offre.DateFinEffetGarantie.HasValue)
                {
                    SetControlDate(model.Offre.DateFinEffetGarantie.Value.ToString(), "FinOffre");
                }

                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Risque", Convert.ToInt32(model.Code));
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            }
            else if (model.Contrat != null)
            {
                model.Branche = model.Contrat.Branche;

                if (model.Contrat.DateEffetAnnee != 0 && model.Contrat.DateEffetMois != 0 && model.Contrat.DateEffetJour != 0)
                    model.InformationsGenerales.DateDebEffet = new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour);
                else model.InformationsGenerales.DateDebEffet = null;
                if (model.Contrat.FinEffetAnnee != 0 && model.Contrat.FinEffetMois != 0 && model.Contrat.FinEffetJour != 0)
                    model.InformationsGenerales.DateFinEffet = new DateTime(model.Contrat.FinEffetAnnee, model.Contrat.FinEffetMois, model.Contrat.FinEffetJour);
                else model.InformationsGenerales.DateFinEffet = null;

                if (model.InformationsGenerales.DateDebEffet.HasValue)
                {
                    if (model.Contrat.DureeGarantie > 0 && model.Contrat.UniteDeTemps != null)
                    {
                        DateTime dateFinCalcul = model.InformationsGenerales.DateDebEffet.Value;
                        switch (model.Contrat.UniteDeTemps)
                        {
                            case "M":
                                dateFinCalcul = dateFinCalcul.AddMonths(model.Contrat.DureeGarantie).AddDays(-1);
                                model.Contrat.FinEffetMois = (short)dateFinCalcul.Month;
                                model.Contrat.FinEffetJour = (short)dateFinCalcul.Day;
                                model.Contrat.FinEffetAnnee = (short)dateFinCalcul.Year;
                                break;
                            case "J":
                                dateFinCalcul = dateFinCalcul.AddDays(model.Contrat.DureeGarantie).AddDays(-1);
                                model.Contrat.FinEffetMois = (short)dateFinCalcul.Month;
                                model.Contrat.FinEffetJour = (short)dateFinCalcul.Day;
                                model.Contrat.FinEffetAnnee = (short)dateFinCalcul.Year;
                                break;
                            case "A":
                                dateFinCalcul = dateFinCalcul.AddYears(model.Contrat.DureeGarantie).AddDays(-1);
                                model.Contrat.FinEffetMois = (short)dateFinCalcul.Month;
                                model.Contrat.FinEffetJour = (short)dateFinCalcul.Day;
                                model.Contrat.FinEffetAnnee = (short)dateFinCalcul.Year;
                                break;
                            default: break;
                        }
                    }
                }

                if (model.Contrat.DateEffetAnnee != 0 && model.Contrat.DateEffetMois != 0 && model.Contrat.DateEffetJour != 0)
                {
                    var timeDeb = AlbConvert.ConvertIntToTimeMinute(model.Contrat.DateEffetHeure);
                    SetControlDate(new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour, timeDeb.HasValue ? timeDeb.Value.Hours : 0, timeDeb.HasValue ? timeDeb.Value.Minutes : 0, 0).ToString(), "DebOffre");
                }
                if (model.Contrat.FinEffetAnnee != 0 && model.Contrat.FinEffetMois != 0 && model.Contrat.FinEffetJour != 0)
                {
                    var timeFin = AlbConvert.ConvertIntToTimeMinute(model.Contrat.FinEffetHeure);
                    SetControlDate(new DateTime(model.Contrat.FinEffetAnnee, model.Contrat.FinEffetMois, model.Contrat.FinEffetJour, timeFin.HasValue ? timeFin.Value.Hours : 0, timeFin.HasValue ? timeFin.Value.Minutes : 0, 0).ToString(), "FinOffre");
                }

                model.DateEffetAvenant = model.Contrat.DateEffetAvenant;
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Risque", Convert.ToInt32(model.Code));
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
            }
        }

        /// <summary>
        /// MAJ les Report Values
        /// </summary>
        /// <param name="objets">Objet de type ObjetDto</param>
        private void SetReportValues(List<ObjetDto> objets)
        {
            Int64? sumObj = 0;
            string unitObj = string.Empty;
            string typeObj = string.Empty;
            string valHTObj = string.Empty;
            string ensembleTypeObj = string.Empty;
            //Int64? sumCout = 0;

            GetReportValues(objets, out sumObj, out unitObj, out typeObj, out valHTObj, out ensembleTypeObj);

            model.ReportValeur = "N";
            model.ReportObligatoire = "N";

            if (sumObj != null && sumObj != 0 && unitObj != null)
            {
                model.ValeurObjets = sumObj;
                model.UniteObjets = unitObj;
                model.TypeObjets = string.IsNullOrEmpty(typeObj) ? model.Type : typeObj;
                // model.TypeObjets = string.IsNullOrEmpty(model.Type) ? string.IsNullOrEmpty(typeObj) ? string.Empty : typeObj : model.Type;

                model.EnsembleType = ensembleTypeObj;
                //model.CoutM2Objets = sumCout;

                model.ReportValeur = "O";

                if (valHTObj != null)
                {
                    model.ValeurHTObjets = valHTObj;
                    model.ActiverReport = ensembleTypeObj != null;
                    model.Valeur = model.ValeurObjets;
                    model.Unite = model.UniteObjets;
                    model.Type = model.TypeObjets;
                    model.ValeurHT = model.ValeurHTObjets;
                    //model.CoutM2 = model.CoutM2Objets;

                    model.ReportObligatoire = "O";

                }
            }
            //else { 
            //    //On tente de mélanger des types d'objet non compatibles entre eux => reset des valeurs/type du risque
            //    model.ValeurObjets = null;
            //    model.UniteObjets = null;
            //    model.TypeObjets = null;
            //    model.EnsembleType = null;

            //    model.ValeurHTObjets = null;
            //    model.Valeur = null;
            //    model.Unite = null;
            //    model.Type = null;
            //    model.ValeurHT = null;
            //}
            if (objets.Count == 1)
                model.ActiverReport = true;
        }

        /// <summary>
        /// Vérifie si le report doit être effectué ou pas
        /// </summary>
        /// <param name="objets">Objet Dto</param>
        /// <param name="sumObj">la somme de l'objet comme paramètre de retour</param>
        /// <param name="unitObj">Unite utilisé comme paramètre de retour</param>
        /// <param name="typeObj">Type utilisé comme paramètre de retour</param>
        /// <param name="valHTObj">Valeur HT utilisée comme paramètre de retour</param>
        private void GetReportValues(List<ObjetDto> objets, out Int64? sumObj, out string unitObj, out string typeObj, out string valHTObj, out string ensembleTypeObj)
        {
            sumObj = 0;
            unitObj = string.Empty;
            typeObj = string.Empty;
            valHTObj = string.Empty;
            ensembleTypeObj = string.Empty;
            //sumCout = 0;

            foreach (ObjetDto obj in objets)
            {
                if (sumObj != null && obj.Valeur != 0 && ((model.IsModeAvenant && !obj.IsSortiAvenant) || (!model.IsModeAvenant)))
                    sumObj += obj.Valeur;

                //if (sumCout != null && obj.CoutM2 != 0 && ((model.IsModeAvenant && !obj.IsSortiAvenant) || (!model.IsModeAvenant)))
                //    sumCout += obj.CoutM2;

                if (unitObj != null && obj.Unite.Code != string.Empty)
                {
                    if (unitObj != string.Empty && unitObj != obj.Unite.Code)
                    {
                        sumObj = null;
                        unitObj = null;
                        typeObj = null;
                        valHTObj = null;
                        ensembleTypeObj = null;
                        // sumCout = null;
                        break;
                    }
                    else
                        unitObj = obj.Unite.Code;
                }
                //else
                //{
                //    sumObj = null;
                //    unitObj = null;
                //    typeObj = null;
                //    valHTObj = null;
                //    ensembleTypeObj = null;
                //    break;
                //}

                if (typeObj != null && obj.Type.Code != string.Empty)
                {
                    if (typeObj != string.Empty && typeObj != obj.Type.Code && ensembleTypeObj != obj.EnsembleType)
                    {
                        sumObj = null;
                        unitObj = null;
                        typeObj = null;
                        valHTObj = null;
                        ensembleTypeObj = null;
                        //sumCout = null;
                        break;
                    }
                    else if (typeObj != string.Empty && typeObj != obj.Type.Code)//&& ensembleTypeObj != obj.EnsembleType)
                    {
                        typeObj = null;
                    }
                    else if (!string.IsNullOrEmpty(ensembleTypeObj) && ensembleTypeObj != obj.EnsembleType)
                    {
                        typeObj = null;
                        ensembleTypeObj = null;
                        break;
                    }
                    //else if(typeObj != string.Empty && typeObj != obj.Type.Code && ensembleTypeObj == obj.EnsembleType)
                    else
                        typeObj = obj.Type.Code;
                }
                //else
                //{
                //    sumObj = null;
                //    unitObj = null;
                //    typeObj = null;
                //    valHTObj = null;
                //    ensembleTypeObj = null;
                //    break;
                //}

                if (ensembleTypeObj != null)
                {
                    if (ensembleTypeObj != string.Empty && ensembleTypeObj != obj.EnsembleType)
                        ensembleTypeObj = null;
                    else
                        ensembleTypeObj = obj.EnsembleType;
                }

                if (valHTObj != null)
                {
                    if (valHTObj != string.Empty && valHTObj != obj.ValeurHT)
                        valHTObj = null;
                    else
                        valHTObj = obj.ValeurHT;
                }
            }
        }

        private void SetControlDate(string date, string typeDate)
        {
            switch (typeDate)
            {
                case "DebOffre":
                    model.DateDebStr = date.Split(' ')[0];
                    model.HeureDebStr = date.Split(' ')[1].Split(':')[0];
                    model.MinuteDebStr = date.Split(' ')[1].Split(':')[1];
                    break;
                case "FinOffre":
                    model.DateFinStr = date.Split(' ')[0];
                    model.HeureFinStr = date.Split(' ')[1].Split(':')[0];
                    model.MinuteFinStr = date.Split(' ')[1].Split(':')[1];
                    break;
                case "DebMinObjet":
                    model.DateDebMinObj = date.Split(' ')[0];
                    model.HeureDebMinObj = date.Split(' ')[1].Split(':')[0];
                    model.MinuteDebMinObj = date.Split(' ')[1].Split(':')[1];
                    break;
                case "DebMaxObjet":
                    model.DateDebMaxObj = date.Split(' ')[0];
                    model.HeureDebMaxObj = date.Split(' ')[1].Split(':')[0];
                    model.MinuteDebMaxObj = date.Split(' ')[1].Split(':')[1];
                    break;
                case "FinMaxObjet":
                    model.DateFinMaxObj = date.Split(' ')[0];
                    model.HeureFinMaxObj = date.Split(' ')[1].Split(':')[0];
                    model.MinuteFinMaxObj = date.Split(' ')[1].Split(':')[1];
                    break;

                case "FinMinObjet":
                    model.DateFinMinObj = date.Split(' ')[0];
                    model.HeureFinMinObj = date.Split(' ')[1].Split(':')[0];
                    model.MinuteFinMinObj = date.Split(' ')[1].Split(':')[1];
                    break;
                case "DebObjetAvn":
                    model.DateDebMinObjAvn = date.Split(' ')[0];
                    model.HeureDebMinObjAvn = date.Split(' ')[1].Split(':')[0];
                    model.MinuteDebMinObjAvn = date.Split(' ')[1].Split(':')[1];
                    break;
                case "FinObjetAvn":
                    model.DateFinMaxObjAvn = date.Split(' ')[0];
                    model.HeureFinMaxObjAvn = date.Split(' ')[1].Split(':')[0];
                    model.MinuteFinMaxObjAvn = date.Split(' ')[1].Split(':')[1];
                    break;
            }

        }

        private void InitModeles()
        {
            model.InformationsGenerales = new ModeleInformationsGeneralesRisque();
            model.InformationsGenerales.ListesNomenclatures = new ModeleInformationsGeneralesRisqueNomenclatures();
            model.InformationsGenerales.ListesNomenclatures.Nomenclature1 = new ModeleListeNomenclatures() { NumeroCombo = "1" };
            model.InformationsGenerales.ListesNomenclatures.Nomenclature2 = new ModeleListeNomenclatures() { NumeroCombo = "2" };
            model.InformationsGenerales.ListesNomenclatures.Nomenclature3 = new ModeleListeNomenclatures() { NumeroCombo = "3" };
            model.InformationsGenerales.ListesNomenclatures.Nomenclature4 = new ModeleListeNomenclatures() { NumeroCombo = "4" };
            model.InformationsGenerales.ListesNomenclatures.Nomenclature5 = new ModeleListeNomenclatures() { NumeroCombo = "5" };


            model.InformationsGenerales.Cibles = new List<AlbSelectListItem>();
            model.InformationsGenerales.Types = new List<AlbSelectListItem>();
            model.InformationsGenerales.Unites = new List<AlbSelectListItem>();
            model.InformationsGenerales.ValeursHT = new List<AlbSelectListItem>();

            model.ObjetsRisqueMetaData = new ModeleObjetsRisque();
            model.ObjetsRisqueMetaData.Objets = new List<ModeleDetailsObjetRisque>();

        }

        private ModeleInformationsGeneralesRisqueNomenclatures LoadListesNomenclatures(List<NomenclatureDto> sourceNomenclature1, List<NomenclatureDto> sourceNomenclature2, List<NomenclatureDto> sourceNomenclature3, List<NomenclatureDto> sourceNomenclature4, List<NomenclatureDto> sourceNomenclature5, ModeleInformationsGeneralesRisqueNomenclatures destination = null)
        {
            ModeleInformationsGeneralesRisqueNomenclatures toReturn = null;
            if (destination == null)
            {
                toReturn = new ModeleInformationsGeneralesRisqueNomenclatures();
                toReturn.Nomenclature1 = new ModeleListeNomenclatures() { NumeroCombo = "1" };
                toReturn.Nomenclature2 = new ModeleListeNomenclatures() { NumeroCombo = "2" };
                toReturn.Nomenclature3 = new ModeleListeNomenclatures() { NumeroCombo = "3" };
                toReturn.Nomenclature4 = new ModeleListeNomenclatures() { NumeroCombo = "4" };
                toReturn.Nomenclature5 = new ModeleListeNomenclatures() { NumeroCombo = "5" };
            }
            else
                toReturn = destination;

            var tempNomen1 = sourceNomenclature1 != null ? sourceNomenclature1.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
            if (tempNomen1 != null && tempNomen1.Any())
                toReturn.Nomenclature1.Nomenclatures = tempNomen1.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
            else
                toReturn.Nomenclature1.Nomenclatures = new List<AlbSelectListItem>();

            var tempNomen2 = sourceNomenclature2 != null ? sourceNomenclature2.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
            if (tempNomen2 != null && tempNomen2.Any())
                toReturn.Nomenclature2.Nomenclatures = tempNomen2.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
            else
                toReturn.Nomenclature2.Nomenclatures = new List<AlbSelectListItem>();

            var tempNomen3 = sourceNomenclature3 != null ? sourceNomenclature3.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
            if (tempNomen3 != null && tempNomen3.Any())
                toReturn.Nomenclature3.Nomenclatures = tempNomen3.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
            else
                toReturn.Nomenclature3.Nomenclatures = new List<AlbSelectListItem>();

            var tempNomen4 = sourceNomenclature4 != null ? sourceNomenclature4.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
            if (tempNomen4 != null && tempNomen4.Any())
                toReturn.Nomenclature4.Nomenclatures = tempNomen4.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
            else
                toReturn.Nomenclature4.Nomenclatures = new List<AlbSelectListItem>();

            var tempNomen5 = sourceNomenclature5 != null ? sourceNomenclature5.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
            if (tempNomen5 != null && tempNomen5.Any())
                toReturn.Nomenclature5.Nomenclatures = tempNomen5.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
            else
                toReturn.Nomenclature5.Nomenclatures = new List<AlbSelectListItem>();

            SetSelectedItemNomenclature(toReturn.Nomenclature1.Nomenclatures, toReturn.Nomenclature1.Nomenclature);
            SetSelectedItemNomenclature(toReturn.Nomenclature2.Nomenclatures, toReturn.Nomenclature2.Nomenclature);
            SetSelectedItemNomenclature(toReturn.Nomenclature3.Nomenclatures, toReturn.Nomenclature3.Nomenclature);
            SetSelectedItemNomenclature(toReturn.Nomenclature4.Nomenclatures, toReturn.Nomenclature4.Nomenclature);
            SetSelectedItemNomenclature(toReturn.Nomenclature5.Nomenclatures, toReturn.Nomenclature5.Nomenclature);

            toReturn.Nomenclature1.LibelleNomenclature = sourceNomenclature1 != null && sourceNomenclature1.Any() && !string.IsNullOrEmpty(sourceNomenclature1.FirstOrDefault().LibelleCombo) ? sourceNomenclature1.FirstOrDefault().LibelleCombo : "Nomenclature 1";
            toReturn.Nomenclature2.LibelleNomenclature = sourceNomenclature2 != null && sourceNomenclature2.Any() && !string.IsNullOrEmpty(sourceNomenclature2.FirstOrDefault().LibelleCombo) ? sourceNomenclature2.FirstOrDefault().LibelleCombo : "Nomenclature 2";
            toReturn.Nomenclature3.LibelleNomenclature = sourceNomenclature3 != null && sourceNomenclature3.Any() && !string.IsNullOrEmpty(sourceNomenclature3.FirstOrDefault().LibelleCombo) ? sourceNomenclature3.FirstOrDefault().LibelleCombo : "Nomenclature 3";
            toReturn.Nomenclature4.LibelleNomenclature = sourceNomenclature4 != null && sourceNomenclature4.Any() && !string.IsNullOrEmpty(sourceNomenclature4.FirstOrDefault().LibelleCombo) ? sourceNomenclature4.FirstOrDefault().LibelleCombo : "Nomenclature 4";
            toReturn.Nomenclature5.LibelleNomenclature = sourceNomenclature5 != null && sourceNomenclature5.Any() && !string.IsNullOrEmpty(sourceNomenclature5.FirstOrDefault().LibelleCombo) ? sourceNomenclature5.FirstOrDefault().LibelleCombo : "Nomenclature 5";

            toReturn.Nomenclature1.NiveauNomenclature = sourceNomenclature1 != null && sourceNomenclature1.Any() ? sourceNomenclature1.FirstOrDefault().NiveauCombo : string.Empty;
            toReturn.Nomenclature2.NiveauNomenclature = sourceNomenclature2 != null && sourceNomenclature2.Any() ? sourceNomenclature2.FirstOrDefault().NiveauCombo : string.Empty;
            toReturn.Nomenclature3.NiveauNomenclature = sourceNomenclature3 != null && sourceNomenclature3.Any() ? sourceNomenclature3.FirstOrDefault().NiveauCombo : string.Empty;
            toReturn.Nomenclature4.NiveauNomenclature = sourceNomenclature4 != null && sourceNomenclature4.Any() ? sourceNomenclature4.FirstOrDefault().NiveauCombo : string.Empty;
            toReturn.Nomenclature5.NiveauNomenclature = sourceNomenclature5 != null && sourceNomenclature5.Any() ? sourceNomenclature5.FirstOrDefault().NiveauCombo : string.Empty;

            toReturn.Nomenclature1.NomenclatureReadOnly = true;
            toReturn.Nomenclature2.NomenclatureReadOnly = true;
            toReturn.Nomenclature3.NomenclatureReadOnly = true;
            toReturn.Nomenclature4.NomenclatureReadOnly = true;
            toReturn.Nomenclature5.NomenclatureReadOnly = true;

            return toReturn;
        }
        //SAB :07/08/2017 #EPURATIONREQUETE
        private ModeleInformationsGeneralesRisqueNomenclatures LoadListesNomenclaturesNew(List<NomenclatureDto> sourceNomenclature1, List<NomenclatureDto> sourceNomenclature2, List<NomenclatureDto> sourceNomenclature3, List<NomenclatureDto> sourceNomenclature4, List<NomenclatureDto> sourceNomenclature5, ModeleInformationsGeneralesRisqueNomenclatures destination = null)
        {
            ModeleInformationsGeneralesRisqueNomenclatures toReturn = null;
            if (destination == null)
            {
                toReturn = new ModeleInformationsGeneralesRisqueNomenclatures();
                toReturn.Nomenclature1 = new ModeleListeNomenclatures() { NumeroCombo = "1" };
                toReturn.Nomenclature2 = new ModeleListeNomenclatures() { NumeroCombo = "2" };
                toReturn.Nomenclature3 = new ModeleListeNomenclatures() { NumeroCombo = "3" };
                toReturn.Nomenclature4 = new ModeleListeNomenclatures() { NumeroCombo = "4" };
                toReturn.Nomenclature5 = new ModeleListeNomenclatures() { NumeroCombo = "5" };
            }
            else
                toReturn = destination;

            var tempNomen1 = sourceNomenclature1 != null ? sourceNomenclature1.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
            if (tempNomen1 != null && tempNomen1.Any())
                toReturn.Nomenclature1.Nomenclatures = tempNomen1.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
            else
                toReturn.Nomenclature1.Nomenclatures = new List<AlbSelectListItem>();

            var tempNomen2 = sourceNomenclature2 != null ? sourceNomenclature2.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
            if (tempNomen2 != null && tempNomen2.Any())
                toReturn.Nomenclature2.Nomenclatures = tempNomen2.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
            else
                toReturn.Nomenclature2.Nomenclatures = new List<AlbSelectListItem>();

            var tempNomen3 = sourceNomenclature3 != null ? sourceNomenclature3.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
            if (tempNomen3 != null && tempNomen3.Any())
                toReturn.Nomenclature3.Nomenclatures = tempNomen3.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
            else
                toReturn.Nomenclature3.Nomenclatures = new List<AlbSelectListItem>();

            var tempNomen4 = sourceNomenclature4 != null ? sourceNomenclature4.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
            if (tempNomen4 != null && tempNomen4.Any())
                toReturn.Nomenclature4.Nomenclatures = tempNomen4.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
            else
                toReturn.Nomenclature4.Nomenclatures = new List<AlbSelectListItem>();

            var tempNomen5 = sourceNomenclature5 != null ? sourceNomenclature5.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature)) : null;
            if (tempNomen5 != null && tempNomen5.Any())
                toReturn.Nomenclature5.Nomenclatures = tempNomen5.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
            else
                toReturn.Nomenclature5.Nomenclatures = new List<AlbSelectListItem>();

            SetSelectedItemNomenclature(toReturn.Nomenclature1.Nomenclatures, toReturn.Nomenclature1.Nomenclature);
            SetSelectedItemNomenclature(toReturn.Nomenclature2.Nomenclatures, toReturn.Nomenclature2.Nomenclature);
            SetSelectedItemNomenclature(toReturn.Nomenclature3.Nomenclatures, toReturn.Nomenclature3.Nomenclature);
            SetSelectedItemNomenclature(toReturn.Nomenclature4.Nomenclatures, toReturn.Nomenclature4.Nomenclature);
            SetSelectedItemNomenclature(toReturn.Nomenclature5.Nomenclatures, toReturn.Nomenclature5.Nomenclature);

            toReturn.Nomenclature1.LibelleNomenclature = sourceNomenclature1 != null && sourceNomenclature1.Any() && !string.IsNullOrEmpty(sourceNomenclature1.FirstOrDefault().LibelleCombo) ? sourceNomenclature1.FirstOrDefault().LibelleCombo : "Nomenclature 1";
            toReturn.Nomenclature2.LibelleNomenclature = sourceNomenclature2 != null && sourceNomenclature2.Any() && !string.IsNullOrEmpty(sourceNomenclature2.FirstOrDefault().LibelleCombo) ? sourceNomenclature2.FirstOrDefault().LibelleCombo : "Nomenclature 2";
            toReturn.Nomenclature3.LibelleNomenclature = sourceNomenclature3 != null && sourceNomenclature3.Any() && !string.IsNullOrEmpty(sourceNomenclature3.FirstOrDefault().LibelleCombo) ? sourceNomenclature3.FirstOrDefault().LibelleCombo : "Nomenclature 3";
            toReturn.Nomenclature4.LibelleNomenclature = sourceNomenclature4 != null && sourceNomenclature4.Any() && !string.IsNullOrEmpty(sourceNomenclature4.FirstOrDefault().LibelleCombo) ? sourceNomenclature4.FirstOrDefault().LibelleCombo : "Nomenclature 4";
            toReturn.Nomenclature5.LibelleNomenclature = sourceNomenclature5 != null && sourceNomenclature5.Any() && !string.IsNullOrEmpty(sourceNomenclature5.FirstOrDefault().LibelleCombo) ? sourceNomenclature5.FirstOrDefault().LibelleCombo : "Nomenclature 5";

            toReturn.Nomenclature1.NiveauNomenclature = sourceNomenclature1 != null && sourceNomenclature1.Any() ? sourceNomenclature1.FirstOrDefault().NiveauCombo : string.Empty;
            toReturn.Nomenclature2.NiveauNomenclature = sourceNomenclature2 != null && sourceNomenclature2.Any() ? sourceNomenclature2.FirstOrDefault().NiveauCombo : string.Empty;
            toReturn.Nomenclature3.NiveauNomenclature = sourceNomenclature3 != null && sourceNomenclature3.Any() ? sourceNomenclature3.FirstOrDefault().NiveauCombo : string.Empty;
            toReturn.Nomenclature4.NiveauNomenclature = sourceNomenclature4 != null && sourceNomenclature4.Any() ? sourceNomenclature4.FirstOrDefault().NiveauCombo : string.Empty;
            toReturn.Nomenclature5.NiveauNomenclature = sourceNomenclature5 != null && sourceNomenclature5.Any() ? sourceNomenclature5.FirstOrDefault().NiveauCombo : string.Empty;

            toReturn.Nomenclature1.NomenclatureReadOnly = true;
            toReturn.Nomenclature2.NomenclatureReadOnly = true;
            toReturn.Nomenclature3.NomenclatureReadOnly = true;
            toReturn.Nomenclature4.NomenclatureReadOnly = true;
            toReturn.Nomenclature5.NomenclatureReadOnly = true;

            return toReturn;
        }

        private static void SetSelectedItemNomenclature(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            if (!string.IsNullOrEmpty(compareValue) && lookUp != null)
            {
                selectedItem = lookUp.FirstOrDefault(elm => elm.Text.Split('-')[0].Trim() == compareValue.Trim());
                if (selectedItem != null)
                    selectedItem.Selected = true;
            }
        }
        #region Redirections

        private RedirectToRouteResult RedirectionDetailsObjet(string codeOffre, int? version, string type, string codeObjet, string tabGuid, string addParamType, string addParamValue, string modeNavig)
        {
            var openObj = codeObjet.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None)[0];
            var forceReadOnly = codeObjet.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None)[1] == "1" ? "||FORCEREADONLY|1" : string.Empty;
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (!string.IsNullOrEmpty(numAvn) && int.Parse(numAvn) != 0)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceContext = client.Channel;
                    forceReadOnly = serviceContext.CheckObjetSorit(codeOffre, version, type, numAvn, openObj) ? "||FORCEREADONLY|1" : string.Empty;
                }
            }
            forceReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version.ToString() + "_" + type, numAvn) ? string.Empty : forceReadOnly;
            return RedirectToAction("Index", "DetailsObjetRisque", new { id = codeOffre + "_" + version + "_" + type + "_" + openObj + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue + forceReadOnly) + GetFormatModeNavig(modeNavig) });
        }

        #endregion

        #endregion

    }
}
