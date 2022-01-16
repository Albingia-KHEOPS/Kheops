using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesDetailsObjet;
using ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.DataAccess;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Ecran.DetailsInventaire;
using OP.WSAS400.DTO.Ecran.DetailsObjetRisque;
using OP.WSAS400.DTO.Ecran.DetailsRisque;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class DetailsObjetRisqueController : RemiseEnVigueurController<ModeleDetailsObjetRisquePage> {
        public override bool IsReadonly {
            get {
                if (Model.IsObjetSorti) {
                    return true;
                }
                Model.InitPoliceId();
                return base.IsReadonly;
            }
        }

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
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquePort>()) {
                    return client.Channel.IsAvnDisabled(affaireId, rsq);
                }
            }
        }

        
        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult Index(string id) {
            string newId = InitializeParams(id);
            var resultRedirect = LoadInfoPage(newId);
            if (model.IsObjetSorti && model.CodeObjet != "0")
            {
                model.IsReadOnly = true;
                model.ListInventaires.IsReadOnly = true;
                model.ContactAdresse.IsReadOnlyDisplay = true;
                model.InformationsGenerales.IsReadOnly = true;
            }

            bool copyAdr = GetAddParamValue(model.AddParamValue, AlbParameterName.COPY_ADR_FROM_HEADER) == "1";

            if(copyAdr)
            {
                InitialiserRisqueAvecAdresse();
            }

            if (resultRedirect != null) {
                return resultRedirect;
            }
            return View(model);
        }

        private void InitialiserRisqueAvecAdresse()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                var adresse = screenClient.ObtenirAdresse(Model.CodePolicePage, Model.VersionPolicePage, Model.TypePolicePage);


                model.ContactAdresse.Batiment = adresse.Batiment;
                model.ContactAdresse.CodePostal = (adresse.CodePostal != 0) ? adresse.Departement + adresse.CodePostal.ToString() : string.Empty;
                model.ContactAdresse.CodePostalCedex = (adresse.CodePostalCedex != 0) ? adresse.Departement + adresse.CodePostalCedex.ToString() : string.Empty;
                //model.ContactAdresse.Context = adresse.
                model.ContactAdresse.Distribution = adresse.BoitePostale;
                model.ContactAdresse.Extension = adresse.ExtensionVoie;
                //model.ContactAdresse.FirstIndex = adresse.
                model.ContactAdresse.Latitude = (adresse.Latitude.HasValue) ? adresse.Latitude.Value.ToString() : string.Empty;
                model.ContactAdresse.Longitude = (adresse.Longitude.HasValue) ? adresse.Longitude.Value.ToString() : string.Empty;
                model.ContactAdresse.No = adresse.NumeroVoie.ToString();
                model.ContactAdresse.No2 = adresse.NumeroVoie2;
                model.ContactAdresse.Pays = adresse.NomPays.Replace("- ","");
                model.ContactAdresse.Ville = adresse.NomVille;
                model.ContactAdresse.VilleCedex = adresse.NomCedex;
                model.ContactAdresse.Voie = adresse.NomVoie;
                model.ContactAdresse.MatriculeHexavia = adresse.MatriculeHexavia;
            }

        }


        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult DetailsObjetEnregistrer(ModeleDetailsObjetRisquePage saveModel)
        {
            var numAvn = GetAddParamValue(saveModel.AddParamValue, AlbParameterName.AVNID);
            saveModel.NumAvenantPage = numAvn;
            var folder = string.Format("{0}_{1}_{2}", saveModel.Offre.CodeOffre, saveModel.Offre.Version.Value.ToString(CultureInfo.InvariantCulture), saveModel.Offre.Type);

            // Sauvegarde uniquement si l'écran n'est pas en readonly
            if (AllowUpdate && ModeConsultation.Historique != saveModel.ModeNavig.ParseCode<ModeConsultation>()) {
                DetailsObjetRisqueSetQueryDto query = null;
                query = new DetailsObjetRisqueSetQueryDto
                {
                    Offre = 
                        new OffreDto
                        {
                            Type = saveModel.Offre.Type,
                            CodeOffre = saveModel.Offre.CodeOffre,
                            Version = saveModel.Offre.Version,
                            AdresseOffre = new AdressePlatDto()
                        }
                };

                var risques = new List<RisqueDto>
                {
                    new RisqueDto
                    {
                        AdresseRisque = new AdressePlatDto(),
                        Code = Convert.ToInt32(saveModel.CodeRisque),
                        Descriptif = !string.IsNullOrEmpty(saveModel.DescrRisque) ? saveModel.DescrRisque : saveModel.InformationsGenerales.Descriptif,
                        Cible = new CibleDto
                        {
                            Code = saveModel.InformationsGenerales.Cible
                        }
                    }
                };
                query.Offre.Risques = risques;
                var objets = new List<ObjetDto>() ; // { buildModifierOffre(saveModel) };
                query.Offre.Risques[0].Objets = objets;

                //DetailsObjetRisqueSetResultDto result;               
                string result = string.Empty;
                var riskId = 0;
                int.TryParse(saveModel.CodeRisque, out riskId);
                // Wi 3492 - T 3493
                //information risque
                RisqueDto previousInfoRisk = GetRisk(saveModel.Offre.CodeOffre, riskId, saveModel.Offre.Version, saveModel.Offre.Type, numAvn);
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var screenClient = client.Channel;
                    result = screenClient.DetailsObjetRisqueSet(query, GetUser());
                }
                // Wi 3492 - T 3493
                // contrôle de la " Valeur totale risque"
                ControlRiskTotalValue(saveModel.Offre.CodeOffre, saveModel.Offre.Type, saveModel.Offre.Version, riskId, numAvn, previousInfoRisk);

                if (!string.IsNullOrEmpty(saveModel.txtParamRedirect))
                {
                    var tabParamRedirect = saveModel.txtParamRedirect.Split('/');
                    return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                }
                if (saveModel.CodeRisque == "0")
                    saveModel.CodeRisque = result.Split('_')[0];
                if (saveModel.CodeObjet == "0" || string.IsNullOrEmpty(saveModel.CodeObjet))
                    saveModel.CodeObjet = result.Split('_')[1];
                /* si existe des is */
                string splitChars = MvcApplication.SPLIT_CONST_HTML;
                var sbParams = new StringBuilder();

                if (saveModel.Offre != null)
                    sbParams.Append(saveModel.Offre.Type + MvcApplication.SPLIT_CONST_HTML + saveModel.Offre.CodeOffre + MvcApplication.SPLIT_CONST_HTML);
                sbParams.Append(saveModel.Offre.Version + MvcApplication.SPLIT_CONST_HTML);
                sbParams.Append(saveModel.CodeRisque + MvcApplication.SPLIT_CONST_HTML);
                sbParams.Append(saveModel.CodeObjet);
                string strParameters = sbParams.ToString();
                var parameters = DbIOParam.GetParams(HttpUtility.UrlDecode(strParameters), HttpUtility.UrlDecode(splitChars));
                //string etapeIs = "Objet";
                //string section = "Objets";
                //var idModele = DbIOParam.PrepareIsIdModele(saveModel.Branche, section);
                //List<ModeleISDto> isModeleEntete = null;
                //CacheIS.AllISEnteteModelesDto.ForEach(elm =>
                //{
                //    if (isModeleEntete == null)
                //    {
                //        isModeleEntete = new List<ModeleISDto>();
                //    }

                //    if (elm.NomModele.ToLower() == idModele.ToLower())
                //        isModeleEntete.Add(elm);
                //});

                //var ContentIS = "";
                //string splitC = "#**#";
                //var strparam = saveModel.Offre.Type + splitC + saveModel.Offre.CodeOffre + splitC + saveModel.Offre.Version + splitC + saveModel.CodeRisque + splitC + saveModel.CodeObjet;

                bool hasIS = InformationsSpecifiquesObjetsController.HasIS(
                    new AffaireId {
                        CodeAffaire = saveModel.Offre.CodeOffre,
                        NumeroAliment = saveModel.Offre.Version.Value,
                        TypeAffaire = saveModel.Offre.Type.ParseCode<AffaireType>(),
                        NumeroAvenant = int.TryParse(saveModel.NumAvenantPage, out int num) && num >= 0 ? num : default(int?)
                    },
                    int.Parse(saveModel.CodeRisque),
                    int.Parse(saveModel.CodeObjet));

                //using (var serviceContext = new DbInteraction())
                //    ContentIS = serviceContext.LoadDbData(saveModel.ModeNavig, saveModel.CodeObjet, saveModel.CodeRisque, "", "", etapeIs, saveModel.Offre.CodeOffre, saveModel.Offre.Version.ToString(), saveModel.Offre.Type, saveModel.Branche, HttpUtility.UrlDecode(section), "", "", splitC, strparam);

                if (!string.IsNullOrEmpty(saveModel.ParamOpenInven)) {
                    return RedirectToAction("Index", "RisqueInventaire", new { id = saveModel.Offre.CodeOffre + "_" + saveModel.Offre.Version + "_" + saveModel.Offre.Type + "_" + saveModel.CodeRisque + "_" + saveModel.CodeObjet + "_" + saveModel.ParamOpenInven + GetSurroundedTabGuid(saveModel.TabGuid) + BuildAddParamString(saveModel.AddParamType, saveModel.AddParamValue) + GetFormatModeNavig(saveModel.ModeNavig) });
                }

                if (string.IsNullOrEmpty(saveModel.ListInventaires.TypeInventaire)) {
                    if (hasIS) {
                        return RedirectToAction("Index", "InformationsSpecifiquesObjets", new { id = saveModel.Offre.CodeOffre + "_" + saveModel.Offre.Version + "_" + saveModel.Offre.Type + "_" + saveModel.CodeRisque + "_" + saveModel.CodeObjet + GetSurroundedTabGuid(saveModel.TabGuid) + BuildAddParamString(saveModel.AddParamType, saveModel.AddParamValue) + GetFormatModeNavig(saveModel.ModeNavig), returnHome = saveModel.txtSaveCancel });
                    }
                    return RedirectToAction("Index", "DetailsRisque", new { id = saveModel.Offre.CodeOffre + "_" + saveModel.Offre.Version + "_" + saveModel.Offre.Type + "_" + saveModel.CodeRisque + GetSurroundedTabGuid(saveModel.TabGuid) + BuildAddParamString(saveModel.AddParamType, saveModel.AddParamValue) + GetFormatModeNavig(saveModel.ModeNavig) });
                }

                string param = saveModel.Offre.CodeOffre + "_" + saveModel.Offre.Version + "_" + saveModel.Offre.Type + "_" + saveModel.CodeRisque + "_" + saveModel.CodeObjet + "_" + saveModel.ListInventaires.TypeInventaire + "_" + saveModel.ListInventaires.CodeInventaire + GetSurroundedTabGuid(saveModel.TabGuid) + BuildAddParamString(saveModel.AddParamType, saveModel.AddParamValue) + GetFormatModeNavig(saveModel.ModeNavig);
                return RedirectToAction("Index", "RisqueInventaire", new { id = param, returnHome = saveModel.txtSaveCancel, guidTab = saveModel.TabGuid });
            }

            if (!string.IsNullOrEmpty(saveModel.txtParamRedirect))
            {
                var tabParamRedirect = saveModel.txtParamRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (!string.IsNullOrEmpty(saveModel.ParamOpenInven))
                return RedirectToAction("Index", "RisqueInventaire", new { id = saveModel.Offre.CodeOffre + "_" + saveModel.Offre.Version + "_" + saveModel.Offre.Type + "_" + saveModel.CodeRisque + "_" + saveModel.CodeObjet + "_" + saveModel.ParamOpenInven + GetSurroundedTabGuid(saveModel.TabGuid) + BuildAddParamString(saveModel.AddParamType, saveModel.AddParamValue + (saveModel.IsForceReadOnly ? "||" + AlbParameterName.FORCEREADONLY + "|1" : string.Empty)) + GetFormatModeNavig(saveModel.ModeNavig) });

            return RedirectToAction("Index", "InformationsSpecifiquesObjets", new { id = saveModel.Offre.CodeOffre + "_" + saveModel.Offre.Version + "_" + saveModel.Offre.Type + "_" + saveModel.CodeRisque + "_" + saveModel.CodeObjet + GetSurroundedTabGuid(saveModel.TabGuid) + BuildAddParamString(saveModel.AddParamType, saveModel.AddParamValue + (saveModel.IsForceReadOnly ? "||" + AlbParameterName.FORCEREADONLY + "|1" : string.Empty)) + GetFormatModeNavig(saveModel.ModeNavig), returnHome = saveModel.txtSaveCancel, guidTab = saveModel.TabGuid });
        }

        [HttpPost]
        [ErrorHandler]
        public void DetailsObjetInventairesSupprimer(string codeOffre, string version, string codeRisque, string codeObjet, string codeInven, string numDescr, string type)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient = client.Channel;
                var query = new DetailsInventaireDelQueryDto
                {
                    CodeOffre = codeOffre,
                    Version = Convert.ToInt32(version),
                    CodeRisque = Convert.ToInt32(codeRisque),
                    CodeObjet = Convert.ToInt32(codeObjet),
                    CodeInventaire = codeInven,
                    NumDescription = numDescr,
                    Type = type
                };

                screenClient.DetailsInventaireDel(query);
            }
        }
        [ErrorHandler]
        public ActionResult GetActivites(string codeBranche, string codeCible, int pageNumber, string code, string nom)
        {
            ModeleRechercheActivite model = new ModeleRechercheActivite();
            model = AlbTransverse.GetListActivites(codeBranche, codeCible, pageNumber, code, nom);
            if (!string.IsNullOrEmpty(code) || !string.IsNullOrEmpty(nom))
            {
                return PartialView("RechercheActivite/ResultRechecheActivite", model);
            }
            return PartialView("RechercheActivite/RechercheActiv", model);
        }
        [HttpPost]
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult DetailsObjetSupprimer(string id, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {

            string codeOffre = id.Split('_')[0];
            int? version = Convert.ToInt32(id.Split('_')[1]);
            string typeOffre = id.Split('_')[2];
            int codeRisque = Convert.ToInt32(id.Split('_')[3]);
            int codeObjet = Convert.ToInt32(id.Split('_')[4]);
            string codeBranche = id.Split('_')[5];

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient = client.Channel;
                var query = new DetailsObjetRisqueDelQueryDto();

                var objets = new List<ObjetDto> { new ObjetDto { Code = codeObjet } };

                var risques = new List<RisqueDto> { new RisqueDto { Code = codeRisque, Objets = objets } };

                query.offre = new OffreDto { CodeOffre = codeOffre, Version = version, Branche = new BrancheDto { Code = codeBranche, Cible = new CibleDto() }, Risques = risques, Type = typeOffre };

                // Wi 3492 - T 3493
                //information risque
                RisqueDto previousInfoRisk = GetRisk(codeOffre, codeRisque, version, typeOffre, GetAddParamValue(addParamValue, AlbParameterName.AVNID));
                screenClient.DetailsObjetRisqueDel(query);
                // Wi 3492 - T 3493
                // contrôle de la " Valeur totale risque"
                ControlRiskTotalValue(codeOffre, typeOffre, version, codeRisque, GetAddParamValue(addParamValue, AlbParameterName.AVNID), previousInfoRisk);
            }

            return RedirectToAction("Index", "DetailsRisque", new { id = codeOffre + "_" + version + "_" + typeOffre + "_" + codeRisque + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });

        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string tabGuid, string codeRisque, string codeObjet, string param, string modeNavig, string addParamType, string addParamValue, bool isForceReadOnly)
        {
            switch (cible)
            {
                case "DetailsRisque":
                    return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + codeRisque + tabGuid + BuildAddParamString(addParamType, addParamValue + (isForceReadOnly ? "||" + AlbParameterName.IGNOREREADONLY + "|1" : string.Empty)) + GetFormatModeNavig(modeNavig) });
                //return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + codeRisque + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
                case "RisqueInventaire":
                    return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + NomsInternesEcran.RisqueObjet + "_" + codeRisque + "_" + codeObjet + "_" + param + tabGuid + BuildAddParamString(addParamType, addParamValue + (isForceReadOnly ? "||" + AlbParameterName.FORCEREADONLY + "|1" : string.Empty)) + GetFormatModeNavig(modeNavig) });
                //return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + codeRisque + "_" + codeObjet + "_" + param + tabGuid });
                default:
                    return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue + (isForceReadOnly ? "||" + AlbParameterName.IGNOREREADONLY + "|1" : string.Empty)) + GetFormatModeNavig(modeNavig) });
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
        public void SaveValeurModeAvn(string codeOffre, string version, string type, string codeAvn, string tabGuid, string valeur)
        {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var screenClient = client.Channel;
                    screenClient.SaveValeurModeAvn(codeOffre, version, type, valeur);
                }
            }
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult IsEnteteContainAddress(string codeOffre, string version, string type, string codeAvn)
        {
            bool result = false;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                result = client.Channel.IsEnteteContainAddress(
                    codeOffre,
                    version,
                   type,
                    codeAvn);
            }

            return new JsonResult
            {
                Data = new
                {
                    result = result
                }
            };
        }

        #region Méthode Privée
        private new ActionResult LoadInfoPage(string id)
        {
            InitModeles();
            model.Bandeau = null;
            DetailsRisqueGetResultDto infos = new DetailsRisqueGetResultDto();
            

            var folder = new Folder(id.Split('_'));
            string numRsq = folder.OriginalArray[3];
            model.ModeCreationRisque = numRsq == "0";

            string numObj = "0";
            if (folder.OriginalArray.Length > 4 && folder.OriginalArray[4].ContainsChars())
            {
                numObj = folder.OriginalArray[4];
            }

            if (folder.Type == "O")
            {
                model.Offre = new Offre_MetaModel();

                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var CommonOffreClient = chan.Channel;
                    model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(folder.CodeOffre, folder.Version.ToString(), folder.Type, model.NumAvenantPage, model.ModeNavig));
                }
             
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var policeServicesClient = client.Channel;
                    infos = policeServicesClient.GetInfoDetailRsq(folder.CodeOffre, folder.Version.ToString(), folder.Type, numRsq, numObj, model.ModeNavig.ParseCode<ModeConsultation>(), model.NumAvenantPage,
                    ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Any(el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()), model.Offre.Branche.Code, model.Offre.Branche.Cible.Code, MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                    model.Offre.Risques = infos.Offre.Risques;
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                }
            }
            else if (folder.Type == "P")
            {

                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var CommonOffreClient = chan.Channel;
                    var infosBase = CommonOffreClient.LoadInfosBase(folder.CodeOffre, folder.Version.ToString(), folder.Type, model.NumAvenantPage, model.ModeNavig);
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
                        IndiceReference = infosBase.IndiceReference,
                        DateEffetAvenant = AlbConvert.ConvertIntToDate(infosBase.DateAvnAnnee * 10000 + infosBase.DateAvnMois * 100 + infosBase.DateAvnJour),
                        HeureEffetAvenant = new TimeSpan((int)(infosBase.DateAvnHeure / 10000), (int)(infosBase.DateAvnHeure / 100) % 100, 0),

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
                        Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                


                    };
                }
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var serviceContext = chan.Channel;
                    infos = serviceContext.GetInfoDetailRsq(folder.CodeOffre, folder.Version.ToString(), folder.Type, numRsq, numObj, model.ModeNavig.ParseCode<ModeConsultation>(), model.NumAvenantPage,
                    ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Any(el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()), model.Contrat.Branche, model.Contrat.Cible, MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                    model.Contrat.Risques = infos.Offre.Risques;

                    model.DateEffetAvenantModificationLocale = infos.DateModifRsqAvn;
                    if ( numObj != "0" )
                    model.DateModifAvenantModificationLocale =  infos.Offre.Risques.FirstOrDefault(x => x.Code.ToString() == numRsq).Objets.FirstOrDefault(x => x.Code.ToString() == numObj).DateModifAvenantModificationLocale;
                    model.Contrat.ProchaineEchAnnee = Convert.ToInt16(infos.EchAnnee);
                    model.Contrat.ProchaineEchMois = Convert.ToInt16(infos.EchMois);
                    model.Contrat.ProchaineEchJour = Convert.ToInt16(infos.EchJour);

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
           
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient = client.Channel;

                if (model.Offre != null || model.Contrat != null)
                {
                    SetDataDetailsRisque(
                        folder,
                        numRsq.ContainsChars() ? int.Parse(numRsq) : default(int?),
                        numObj.ContainsChars() ? int.Parse(numObj) : default(int?));
                }

                string periodicite = string.Empty;
                string typepolice = string.Empty;

                // DetailsObjetRisqueGetQueryDto query = null;
                //Le cas d'une offre
                if (model.Offre != null)
                {
                    var rsqCible = model.Offre.Risques.FindAll(r => r.Code == Convert.ToInt32(numRsq)).FirstOrDefault();
                    if (rsqCible != null)
                    {
                        model.InformationsGenerales.DateDebRisque = rsqCible.EntreeGarantie;
                        model.InformationsGenerales.DateFinRisque = rsqCible.SortieGarantie;
                    }

                    periodicite = model.Offre.Periodicite.Code;
                }
                //Le cas d'un contrat
                else if (model.Contrat != null)
                {
                    if (model.IsIgnoreReadOnly || model.CodeObjet == "0")//Pour le cas de cet écran, le ignorereadonly n'est passé que d'une création d'objet dans l'avenant
                        model.IsAvenantModificationLocale = true;
                    if (model.IsModeAvenant && !model.IsAvenantModificationLocale && !model.IsForceReadOnly)
                        return RedirectToAction("Index", "DetailsObjetRisque", new { id = model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type + "_" + model.CodeRisque + "_" + model.CodeObjet + GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue + "||FORCEREADONLY|1") + GetFormatModeNavig(model.ModeNavig) });

                    var rsqCible = model.Contrat.Risques.FindAll(r => r.Code == Convert.ToInt32(numRsq)).FirstOrDefault();
                   
                    if (rsqCible != null)
                    {
                        model.InformationsGenerales.DateDebRisque = rsqCible.EntreeGarantie;
                        model.InformationsGenerales.DateFinRisque = rsqCible.SortieGarantie;
                        model.InformationsGenerales.MultiObjet = model.InformationsGenerales.MultiObjet || (rsqCible.Objets != null && rsqCible.Objets.Count > 1);
                    }
                    //else
                    //{
                    //    cible = model.Contrat.Cible;
                    //}
                    periodicite = model.Contrat.PeriodiciteCode;
                    typepolice = model.Contrat.TypePolice;
                }
                //if (string.IsNullOrEmpty(cible))
                //    cible = model.InformationsGenerales.Cible;
                //SAB :09/08/2017 #EPURATIONREQUETE
                //var result = screenClient.DetailsObjetRisqueGet(query, model.NumAvenantPage, branche, cible, model.ModeNavig.ParseCode<ModeConsultation>(), ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Exists(
                //        el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()));
                if (infos != null)
                {
                    model.HasFormules = infos.HasFormules;
                    List<AlbSelectListItem> cibles = infos.Cibles.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Description), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Description) }).ToList();
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
                    if (!string.IsNullOrEmpty(model.InformationsGenerales.Unite))
                    {
                        var sItem = unites.FirstOrDefault(x => x.Value == model.InformationsGenerales.Unite);
                        if (sItem != null)
                        {
                            sItem.Selected = true;
                        }
                    }
                    model.InformationsGenerales.Unites = unites;
                    var types = infos.Types.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle), Descriptif = m.CodeTpcn1.ToString() }).ToList();
                    if (!string.IsNullOrEmpty(model.InformationsGenerales.Type))
                    {
                        var sItem = types.FirstOrDefault(x => x.Value == model.InformationsGenerales.Type);
                        if (sItem != null)
                        {
                            sItem.Selected = true;
                        }

                    }
                    model.InformationsGenerales.Types = types;

                    var typesInventaires = infos.TypesInventaire.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    if (!string.IsNullOrEmpty(model.ListInventaires.TypeInventaire))
                    {
                        var sItem = typesInventaires.FirstOrDefault(x => x.Value == model.ListInventaires.TypeInventaire);
                        if (sItem != null)
                        {
                            sItem.Selected = true;
                        }
                    }
                    model.ListInventaires.TypesInventaire = typesInventaires;
                    //model.ListInventaires.CodeInventaire = screenClient.GetNewId("KBEID").ToString(CultureInfo.CurrentCulture);
                    model.ListInventaires.CodeInventaire = screenClient.GetNewInventaireId(folder.CodeOffre, folder.Version.ToString(), folder.Type, "OB", numRsq, numObj, string.Empty, string.Empty).ToString(CultureInfo.CurrentCulture);
                    model.ListInventaires.IsReadOnly = model.IsReadOnly;
                    //Test sur Modif. hors avenant
                    model.ListInventaires.IsModifHorsAvenant = model.IsModifHorsAvenant;

                    var valeursHT = new List<AlbSelectListItem>
                                        {
                                            new AlbSelectListItem {Text = "H - HT", Value = "H", Selected = false, Title = "H - HT"},
                                            new AlbSelectListItem {Text = "T - TTC", Value = "T", Selected = false, Title = "T - TTC"}
                                        };
                    if (!string.IsNullOrEmpty(model.InformationsGenerales.ValeurHT))
                    {
                        var sItem = valeursHT.FirstOrDefault(x => x.Value == model.InformationsGenerales.ValeurHT);
                        if (sItem != null)
                        {
                            sItem.Selected = true;
                        }
                    }

                    else
                    {
                        if ((string.IsNullOrEmpty(numObj) || numObj == "1") && (model.InformationsGenerales.Valeur == null || model.InformationsGenerales.Valeur.Value == 0) && string.IsNullOrEmpty(model.InformationsGenerales.Type) && periodicite != "R" && typepolice != "M")
                        {
                            model.InformationsGenerales.ValeurHT = "H";
                        }
                    }
                    model.InformationsGenerales.ValeursHT = valeursHT;
                    //Nomenclature d'objets
                    model.InformationsGenerales.CodesApe = infos.CodesApe.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.InformationsGenerales.CodesTre = infos.CodesTre.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.InformationsGenerales.CodesClasse = infos.CodesClasse.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.InformationsGenerales.Territorialites = infos.Territorialites.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    var tempNomen1 = infos.Nomenclatures1.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature));
                    if (tempNomen1 != null && tempNomen1.Any())
                        model.InformationsGenerales.ListesNomenclatures.Nomenclature1.Nomenclatures = tempNomen1.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
                    else
                        model.InformationsGenerales.ListesNomenclatures.Nomenclature1.Nomenclatures = new List<AlbSelectListItem>();

                    var tempNomen2 = infos.Nomenclatures2.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature));
                    if (tempNomen2 != null && tempNomen2.Any())
                        model.InformationsGenerales.ListesNomenclatures.Nomenclature2.Nomenclatures = tempNomen2.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
                    else
                        model.InformationsGenerales.ListesNomenclatures.Nomenclature2.Nomenclatures = new List<AlbSelectListItem>();

                    var tempNomen3 = infos.Nomenclatures3.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature));
                    if (tempNomen3 != null && tempNomen3.Any())
                        model.InformationsGenerales.ListesNomenclatures.Nomenclature3.Nomenclatures = tempNomen3.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
                    else
                        model.InformationsGenerales.ListesNomenclatures.Nomenclature3.Nomenclatures = new List<AlbSelectListItem>();

                    var tempNomen4 = infos.Nomenclatures4.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature));
                    if (tempNomen4 != null && tempNomen4.Any())
                        model.InformationsGenerales.ListesNomenclatures.Nomenclature4.Nomenclatures = tempNomen4.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
                    else
                        model.InformationsGenerales.ListesNomenclatures.Nomenclature4.Nomenclatures = new List<AlbSelectListItem>();

                    var tempNomen5 = infos.Nomenclatures5.FindAll(elm => !string.IsNullOrEmpty(elm.CodeNomenclature));
                    if (tempNomen5 != null && tempNomen5.Any())
                        model.InformationsGenerales.ListesNomenclatures.Nomenclature5.Nomenclatures = tempNomen5.Select(m => new AlbSelectListItem { Value = m.IdValeur.ToString(), Text = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature), Selected = false, Title = string.Format("{0} - {1}", m.CodeNomenclature.Trim(), m.LibelleNomenclature) }).ToList();
                    else
                        model.InformationsGenerales.ListesNomenclatures.Nomenclature5.Nomenclatures = new List<AlbSelectListItem>();

                    SetSelectedItemNomenclature(model.InformationsGenerales.ListesNomenclatures.Nomenclature1.Nomenclatures, model.InformationsGenerales.ListesNomenclatures.Nomenclature1.Nomenclature);
                    SetSelectedItemNomenclature(model.InformationsGenerales.ListesNomenclatures.Nomenclature2.Nomenclatures, model.InformationsGenerales.ListesNomenclatures.Nomenclature2.Nomenclature);
                    SetSelectedItemNomenclature(model.InformationsGenerales.ListesNomenclatures.Nomenclature3.Nomenclatures, model.InformationsGenerales.ListesNomenclatures.Nomenclature3.Nomenclature);
                    SetSelectedItemNomenclature(model.InformationsGenerales.ListesNomenclatures.Nomenclature4.Nomenclatures, model.InformationsGenerales.ListesNomenclatures.Nomenclature4.Nomenclature);
                    SetSelectedItemNomenclature(model.InformationsGenerales.ListesNomenclatures.Nomenclature5.Nomenclatures, model.InformationsGenerales.ListesNomenclatures.Nomenclature5.Nomenclature);


                    model.InformationsGenerales.ListesNomenclatures.Nomenclature1.LibelleNomenclature = infos.Nomenclatures1.Any() && !string.IsNullOrEmpty(infos.Nomenclatures1.FirstOrDefault().LibelleCombo) ? infos.Nomenclatures1.FirstOrDefault().LibelleCombo : "Nomenclature 1";
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature2.LibelleNomenclature = infos.Nomenclatures2.Any() && !string.IsNullOrEmpty(infos.Nomenclatures2.FirstOrDefault().LibelleCombo) ? infos.Nomenclatures2.FirstOrDefault().LibelleCombo : "Nomenclature 2";
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature3.LibelleNomenclature = infos.Nomenclatures3.Any() && !string.IsNullOrEmpty(infos.Nomenclatures3.FirstOrDefault().LibelleCombo) ? infos.Nomenclatures3.FirstOrDefault().LibelleCombo : "Nomenclature 3";
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature4.LibelleNomenclature = infos.Nomenclatures4.Any() && !string.IsNullOrEmpty(infos.Nomenclatures4.FirstOrDefault().LibelleCombo) ? infos.Nomenclatures4.FirstOrDefault().LibelleCombo : "Nomenclature 4";
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature5.LibelleNomenclature = infos.Nomenclatures5.Any() && !string.IsNullOrEmpty(infos.Nomenclatures5.FirstOrDefault().LibelleCombo) ? infos.Nomenclatures5.FirstOrDefault().LibelleCombo : "Nomenclature 5";

                    model.InformationsGenerales.ListesNomenclatures.Nomenclature1.NiveauNomenclature = infos.Nomenclatures1.Any() ? infos.Nomenclatures1.FirstOrDefault().NiveauCombo : string.Empty;
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature2.NiveauNomenclature = infos.Nomenclatures2.Any() ? infos.Nomenclatures2.FirstOrDefault().NiveauCombo : string.Empty;
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature3.NiveauNomenclature = infos.Nomenclatures3.Any() ? infos.Nomenclatures3.FirstOrDefault().NiveauCombo : string.Empty;
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature4.NiveauNomenclature = infos.Nomenclatures4.Any() ? infos.Nomenclatures4.FirstOrDefault().NiveauCombo : string.Empty;
                    model.InformationsGenerales.ListesNomenclatures.Nomenclature5.NiveauNomenclature = infos.Nomenclatures5.Any() ? infos.Nomenclatures5.FirstOrDefault().NiveauCombo : string.Empty;

                    model.InformationsGenerales.NomenclatureRisqueModifiable = true;
                    model.InformationsGenerales.TypesRisque = infos.TypesRisque.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.InformationsGenerales.TypesMateriel = infos.TypesMateriel.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle), Descriptif = m.Descriptif }).ToList();
                    model.InformationsGenerales.NaturesLieux = infos.NaturesLieux.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();

                    model.DateDebHisto = AlbConvert.ConvertDateToStr(infos.DateDebHisto);
                    model.HeureDebHisto = AlbConvert.GetTimeFromDate(infos.DateDebHisto).ToString();

                }
            }
            model.InformationsGenerales.ListesNomenclatures.Nomenclature1.NomenclatureReadOnly = model.InformationsGenerales.ListesNomenclatures.Nomenclature1.Nomenclatures.Any() && (model.InformationsGenerales.ReadOnly || model.InformationsGenerales.NomenclatureRisqueModifiable);
            model.InformationsGenerales.ListesNomenclatures.Nomenclature2.NomenclatureReadOnly = model.InformationsGenerales.ListesNomenclatures.Nomenclature2.Nomenclatures.Any() && (model.InformationsGenerales.ReadOnly || model.InformationsGenerales.NomenclatureRisqueModifiable);
            model.InformationsGenerales.ListesNomenclatures.Nomenclature3.NomenclatureReadOnly = model.InformationsGenerales.ListesNomenclatures.Nomenclature3.Nomenclatures.Any() && (model.InformationsGenerales.ReadOnly || model.InformationsGenerales.NomenclatureRisqueModifiable);
            model.InformationsGenerales.ListesNomenclatures.Nomenclature4.NomenclatureReadOnly = model.InformationsGenerales.ListesNomenclatures.Nomenclature4.Nomenclatures.Any() && (model.InformationsGenerales.ReadOnly || model.InformationsGenerales.NomenclatureRisqueModifiable);
            model.InformationsGenerales.ListesNomenclatures.Nomenclature5.NomenclatureReadOnly = model.InformationsGenerales.ListesNomenclatures.Nomenclature5.Nomenclatures.Any() && (model.InformationsGenerales.ReadOnly || model.InformationsGenerales.NomenclatureRisqueModifiable);

            this.model.IsAvnDisabled = IsAvnDisabled;
            model.InformationsGenerales.CodePeriodicite = model.Offre != null
              ? model.Offre.Periodicite.Code
              : model.Contrat != null ? model.Contrat.PeriodiciteCode : string.Empty;
            model.InformationsGenerales.IsModeAvenant = model.IsModeAvenant;
            if (model.ListInventaires != null) {
                model.ListInventaires.IsAvnDisabled = IsAvnDisabled;
            }
            if (model.ContactAdresse != null) {
                model.ContactAdresse.ReadOnly = model.IsReadOnly || IsAvnDisabled == true;
            }

            if (model.IsModeAvenant)
            {
                if (model.InformationsGenerales.DateSortieGarantie.HasValue)
                {
                    DateTime? finEffetRisqueContrat = null;

                    if (model.InformationsGenerales.MultiObjet)//Si multi objet on prend la date du risque comme référence, sinon le contrat
                        finEffetRisqueContrat = model.InformationsGenerales.DateFinRisque;
                    else
                    {
                        DateTime? finEffetContrat = AlbConvert.ConvertIntToDateHour(Convert.ToInt64(model.Contrat.FinEffetAnnee) * 100000000 + Convert.ToInt64(model.Contrat.FinEffetMois) * 1000000 + Convert.ToInt64(model.Contrat.FinEffetJour) * 10000 + Convert.ToInt64(model.Contrat.FinEffetHeure));
                        //DateTime? finEffetContrat = AlbConvert.ConvertIntToDate(model.Contrat.FinEffetAnnee * 10000 + model.Contrat.FinEffetMois * 100 + model.Contrat.FinEffetJour);
                        finEffetRisqueContrat = finEffetContrat;
                    }
                    if (finEffetRisqueContrat.HasValue)//Si date de fin trouvée (du contrat ou risque)
                    {
                        //vu avec FDU 20160418 : faire sauter les tests sauf celui sur la date de modif dans l'avn
                        //if (model.DateEffetAvenantModificationLocale.HasValue)
                        //    model.InformationsGenerales.IsDateFinGarantieModifiable = model.InformationsGenerales.DateSortieGarantie.Value >= model.DateEffetAvenantModificationLocale.Value.AddDays(-1) && model.InformationsGenerales.DateSortieGarantie.Value <= finEffetRisqueContrat.Value;
                        //else
                        //    model.InformationsGenerales.IsDateFinGarantieModifiable = false;
                        if (!model.DateEffetAvenantModificationLocale.HasValue)
                        {
                            model.InformationsGenerales.IsDateFinGarantieModifiable = false;
                        }
                        else
                        {
                            model.InformationsGenerales.IsDateFinGarantieModifiable = true;
                        }
                    }
                    else//Sinon = tacite, pas de date de fin effet
                    {
                        model.InformationsGenerales.IsDateFinGarantieModifiable = true;
                    }

                    if (model.InformationsGenerales.DateSortieGarantie.Value < model.DateEffetAvenant.Value.AddMinutes(-1))
                    {
                        model.IsObjetSorti = true;
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

            //model.PageEnCours = "detailobjet";
            model.PageTitle = model.CodeObjet != "0" ? string.Format("Objet {0} du risque {1}", model.CodeObjet, model.CodeRisque) : "Création d'un objet";
            return null;
        }


        private void SetDataDetailsRisque(Folder folder, int? numRsq, int? numObj)
        {
            string infoBulleRisque = string.Empty;
            string infoBulleObjet = string.Empty;
            model.AfficherBandeau = base.DisplayBandeau(true, folder.Identifier);
            model.AfficherNavigation = model.AfficherBandeau;
            if (model.AfficherBandeau)
            {
                model.Bandeau = base.GetInfoBandeau(folder.Type);
                //Gestion des Etapes
                model.Navigation = new Navigation_MetaModel();
                model.Navigation.Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE;
                if (model.Offre != null)
                {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.Navigation.IdOffre = model.Offre.CodeOffre;
                    model.Navigation.Version = model.Offre.Version;
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
                    model.Navigation.IdOffre = model.Contrat.CodeContrat;
                    model.Navigation.Version = int.Parse(model.Contrat.VersionContrat.ToString());
                }
            }

            if (numRsq.HasValue)
            {
                RisqueDto currentRsq = null;
                if (numRsq > 0) {
                    if (model.Offre != null) {
                        currentRsq = model.Offre.Risques.FirstOrDefault(x => x.Code == numRsq);
                    }
                    else if (model.Contrat != null) {
                        currentRsq = model.Contrat.Risques.FirstOrDefault(x => x.Code == numRsq);
                    }
                }
                else {
                    this.model.AvnCreationObj = int.Parse(model.NumAvenantPage);
                }

                if (currentRsq != null)
                {
                    model.CountObj = currentRsq.Objets.Count();
                    model.CodeRisque = currentRsq.Code.ToString(CultureInfo.CurrentCulture);

                    model.DescrRisque = currentRsq.Descriptif;
                    model.DateEffetAvenantModificationLocale = currentRsq.DateEffetAvenantModificationLocale;
                    model.IsAvenantModificationLocale = currentRsq.IsTraceAvnExist;
                    model.IsRisqueIndexe = currentRsq.isIndexe;

                    if (numObj.GetValueOrDefault() > 0)
                    {
                        var currentObj = currentRsq.Objets.FirstOrDefault(x => x.Code == numObj);

                        if (currentObj != null)
                        {
                            model.ChronoDesi = currentObj.ChronoDesi;
                            model.CodeObjet = currentObj.Code.ToString(CultureInfo.CurrentCulture);
                            model.AvnCreationObj = currentObj.AvnCreationObj;

                            if (currentObj.AdresseObjet != null)
                            {
                                int depart = 0;
                                int.TryParse(currentObj.AdresseObjet.Departement, out depart);
                                string codePostal = string.Empty;
                                string codePX = string.Empty;
                                if (depart > 0)
                                {
                                    codePostal = depart.ToString("D2") + currentObj.AdresseObjet.CodePostal.ToString("D3");
                                    codePX = depart.ToString("D2") + currentObj.AdresseObjet.CodePostalCedex.ToString("D3");
                                }
                                else
                                {
                                    codePostal = currentObj.AdresseObjet.CodePostal.ToString();
                                    codePX = currentObj.AdresseObjet.CodePostalCedex.ToString();
                                }

                                model.ContactAdresse.IsModifHorsAvn = model.IsModifHorsAvenant;
                                model.ContactAdresse.NoChrono = currentObj.AdresseObjet.NumeroChrono;
                                model.ContactAdresse.Distribution = currentObj.AdresseObjet.BoitePostale;
                                model.ContactAdresse.No = currentObj.AdresseObjet.NumeroVoie == 0 ? string.Empty : currentObj.AdresseObjet.NumeroVoie.ToString();
                                model.ContactAdresse.No2 = currentObj.AdresseObjet.NumeroVoie2;
                                model.ContactAdresse.Extension = currentObj.AdresseObjet.ExtensionVoie;
                                model.ContactAdresse.Voie = currentObj.AdresseObjet.NomVoie;
                                model.ContactAdresse.Batiment = currentObj.AdresseObjet.Batiment;
                                model.ContactAdresse.CodePostal = codePostal;
                                model.ContactAdresse.Ville = currentObj.AdresseObjet.NomVille;
                                model.ContactAdresse.CodePostalCedex = codePX;
                                model.ContactAdresse.VilleCedex = currentObj.AdresseObjet.NomCedex;
                                model.ContactAdresse.Latitude = currentObj.AdresseObjet.Latitude?.ToString();
                                model.ContactAdresse.Longitude = currentObj.AdresseObjet.Longitude.ToString();
                                model.ContactAdresse.MatriculeHexavia = currentObj.AdresseObjet.MatriculeHexavia;
                            }
                            
                            model.ListInventaires.Inventaires = new ModeleInventaireObjet();
                            if (currentObj.Inventaires != null)
                            {
                                if (model.Offre != null)
                                    model.ListInventaires.Inventaires = new ModeleInventaireObjet().Load(currentObj.Inventaires.ToList(), model.Offre.CodeOffre, model.Offre.Version, currentRsq.Code, currentObj.Code);
                                else if (model.Contrat != null)
                                    model.ListInventaires.Inventaires = new ModeleInventaireObjet().Load(currentObj.Inventaires.ToList(), model.Contrat.CodeContrat, int.Parse(model.Contrat.VersionContrat.ToString()), currentRsq.Code, currentObj.Code);
                            }

                            model.InformationsGenerales = InformationsGenerales.SetInfosGenerales((InformationGeneraleTransverse)currentObj);
                            model.InformationsGenerales.DescRisque = currentRsq.Descriptif;
                            model.InformationsGenerales.IsRisqueIndexe = model.IsRisqueIndexe;
                            infoBulleObjet = GetInfoBulleObjet(currentObj);
                            model.InformationsGenerales.IndiceActualise = currentObj.IndiceActualise;
                            model.InformationsGenerales.IndiceOrigine = currentObj.IndiceOrigine;
                            model.InformationsGenerales.IndiceCode = model.Offre != null ? model.Offre.IndiceReference.Code : model.Contrat != null ? model.Contrat.IndiceReference : string.Empty;
                            model.InformationsGenerales.IsReportValeur = currentObj.IsReportvaleur;
                            if (currentRsq.Objets != null && currentRsq.Objets.Count == 1)
                            {
                                model.InformationsGenerales.DateModifiable = true;
                                model.InformationsGenerales.CibleModifiable = (int.TryParse(model.NumAvenantPage, out int a) ? a : 0).IsIn(0, this.model.AvnCreationObj);
                            }
                            //Nomenclature d'objets
                            model.InformationsGenerales.CodeApe = currentObj.CodeApe;
                            model.InformationsGenerales.ListesNomenclatures.Nomenclature1.Nomenclature = currentObj.Nomenclature1;
                            model.InformationsGenerales.ListesNomenclatures.Nomenclature2.Nomenclature = currentObj.Nomenclature2;
                            model.InformationsGenerales.ListesNomenclatures.Nomenclature3.Nomenclature = currentObj.Nomenclature3;
                            model.InformationsGenerales.ListesNomenclatures.Nomenclature4.Nomenclature = currentObj.Nomenclature4;
                            model.InformationsGenerales.ListesNomenclatures.Nomenclature5.Nomenclature = currentObj.Nomenclature5;
                            model.InformationsGenerales.Territorialite = currentObj.Territorialite;
                            model.InformationsGenerales.CodeTre = currentObj.CodeTre;
                            model.InformationsGenerales.LibTre = currentObj.LibTre;
                            model.InformationsGenerales.CodeClasse = currentObj.CodeClasse;


                            model.InformationsGenerales.TypeRisque = currentObj.TypeRisque;
                            model.InformationsGenerales.TypeMateriel = currentObj.TypeMateriel;
                            model.InformationsGenerales.NatureLieux = currentObj.NatureLieux;
                            model.InformationsGenerales.IsRisqueTemporaire = currentObj.IsRisqueTemporaire;
                        }
                    }
                    else
                    {
                        model.CodeObjet = "0";
                        model.ChronoDesi = 0;
                        model.InformationsGenerales.Cible = currentRsq.Cible.Code;
                        model.InformationsGenerales.DescRisque = currentRsq.Descriptif;
                        model.InformationsGenerales.MultiObjet = true;
                        model.ListInventaires.Inventaires = new ModeleInventaireObjet();

                        //Si mode avenant et creation d'objet dans un risque existant
                        if (model.IsModeAvenant)
                        {
                            model.InformationsGenerales.DateEntreeGarantie = model.DateEffetAvenantModificationLocale;
                            model.InformationsGenerales.HeureEntreeGarantie = new TimeSpan();
                            model.InformationsGenerales.DateSortieGarantie = currentRsq.SortieGarantie;
                            model.InformationsGenerales.HeureSortieGarantie = AlbConvert.GetTimeFromDate(currentRsq.SortieGarantie);
                        }
                        model.DescrRisque = currentRsq.Descriptif;


                    }

                    infoBulleRisque = GetInfoBulleRisque(currentRsq);

                }
                else
                {
                    model.InformationsGenerales.CibleModifiable = (int.TryParse(model.NumAvenantPage, out int a) ? a : 0).IsIn(0, this.model.AvnCreationObj);
                    model.InformationsGenerales.DateModifiable = true;
                    model.CodeRisque = numRsq.StringValue();
                    model.CodeObjet = "0";
                    model.ChronoDesi = 0;
                    model.ListInventaires.Inventaires = new ModeleInventaireObjet();

                    //SLA 30.01.14 : réactivation demandée pour l'offre
                    if (model.Offre != null && model.Offre.Branche != null && model.Offre.Branche.Cible != null)
                    {
                        model.InformationsGenerales.Cible = model.Offre.Branche.Cible.Code;
                    }

                    if (model.Contrat != null)
                    {
                        model.InformationsGenerales.Cible = model.Contrat.Cible;
                    }

                    //Si mode avenant + creation d'objet et création de risque
                    if (model.IsModeAvenant)
                    {
                        model.DateEffetAvenantModificationLocale = model.Contrat != null ? model.Contrat.DateEffetAvenant : DateTime.Now;
                        model.HeureEffetAvenant = model.Contrat != null ? model.Contrat.HeureEffetAvenant : new TimeSpan();
                        model.InformationsGenerales.DateEntreeGarantie = model.DateEffetAvenantModificationLocale;
                        model.InformationsGenerales.HeureEntreeGarantie = model.HeureEffetAvenant;
                    }
                }

                if (model.Offre != null)
                {
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
                        model.DateDebStr = model.Offre.DateEffetGarantie.Value.ToString(CultureInfo.CurrentCulture).Split(' ')[0];
                        model.HeureDebStr = model.Offre.DateEffetGarantie.Value.ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':')[0];
                        model.MinuteDebStr = model.Offre.DateEffetGarantie.Value.ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':')[1];
                    }
                    if (model.Offre.DateFinEffetGarantie.HasValue)
                    {
                        model.DateFinStr = model.Offre.DateFinEffetGarantie.Value.ToString(CultureInfo.CurrentCulture).Split(' ')[0];
                        model.HeureFinStr = model.Offre.DateFinEffetGarantie.Value.ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':')[0];
                        model.MinuteFinStr = model.Offre.DateFinEffetGarantie.Value.ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':')[1];
                    }
                    //Affichage de la navigation latérale en arboresence
                    model.NavigationArbre = GetNavigationArbre("Risque", codeRisque: Convert.ToInt32(model.CodeRisque));
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type);
                    this.model.IsModifHorsAvenant = GetIsModifHorsAvn(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type);
                }
                else if (model.Contrat != null)
                {
                    if (model.Contrat.DateEffetAnnee != 0 && model.Contrat.DateEffetMois != 0 && model.Contrat.DateEffetJour != 0)
                        model.InformationsGenerales.DateDebEffet = new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour);
                    if (model.Contrat.FinEffetAnnee != 0 && model.Contrat.FinEffetMois != 0 && model.Contrat.FinEffetJour != 0)
                        model.InformationsGenerales.DateFinEffet = new DateTime(model.Contrat.FinEffetAnnee, model.Contrat.FinEffetMois, model.Contrat.FinEffetJour);

                    if (model.InformationsGenerales.DateDebEffet.HasValue)
                    {
                        if (model.Contrat.DureeGarantie > 0 && model.Contrat.UniteDeTemps != null)
                        {
                            DateTime dateFinCalcul = model.InformationsGenerales.DateDebEffet.Value;
                            switch (model.Contrat.UniteDeTemps)
                            {
                                case "M":
                                    dateFinCalcul = dateFinCalcul.AddMonths(model.Contrat.DureeGarantie).AddMinutes(-1);
                                    model.Contrat.FinEffetMois = (short)dateFinCalcul.Month;
                                    model.Contrat.FinEffetJour = (short)dateFinCalcul.Day;
                                    model.Contrat.FinEffetAnnee = (short)dateFinCalcul.Year;
                                    break;
                                case "J":
                                    dateFinCalcul = dateFinCalcul.AddDays(model.Contrat.DureeGarantie).AddMinutes(-1);
                                    model.Contrat.FinEffetMois = (short)dateFinCalcul.Month;
                                    model.Contrat.FinEffetJour = (short)dateFinCalcul.Day;
                                    model.Contrat.FinEffetAnnee = (short)dateFinCalcul.Year;
                                    break;
                                case "A":
                                    dateFinCalcul = dateFinCalcul.AddYears(model.Contrat.DureeGarantie).AddMinutes(-1);
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
                        model.DateDebStr = new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour).ToString(CultureInfo.CurrentCulture).Split(' ')[0];
                        model.HeureDebStr = timeDeb.HasValue ? timeDeb.Value.Hours.ToString() : "0";
                        model.MinuteDebStr = timeDeb.HasValue ? timeDeb.Value.Minutes.ToString() : "0";
                        //model.HeureDebStr = new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour).ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':')[0];
                        //model.MinuteDebStr = new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour).ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':')[1];
                    }
                    if (model.Contrat.FinEffetAnnee != 0 && model.Contrat.FinEffetMois != 0 && model.Contrat.FinEffetJour != 0)
                    {
                        var timeFin = AlbConvert.ConvertIntToTimeMinute(model.Contrat.FinEffetHeure);
                        model.DateFinStr = new DateTime(model.Contrat.FinEffetAnnee, model.Contrat.FinEffetMois, model.Contrat.FinEffetJour).ToString(CultureInfo.CurrentCulture).Split(' ')[0];
                        model.HeureFinStr = timeFin.HasValue ? timeFin.Value.Hours.ToString() : "0";
                        model.MinuteFinStr = timeFin.HasValue ? timeFin.Value.Minutes.ToString() : "0";
                        //model.HeureFinStr = new DateTime(model.Contrat.FinEffetAnnee, model.Contrat.FinEffetMois, model.Contrat.FinEffetJour).ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':')[0];
                        //model.MinuteFinStr = new DateTime(model.Contrat.FinEffetAnnee, model.Contrat.FinEffetMois, model.Contrat.FinEffetJour).ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':')[1];

                        if (model.IsModeAvenant && model.CodeObjet == "0")
                        {
                            model.InformationsGenerales.DateSortieGarantie = model.InformationsGenerales.DateSortieGarantie == null ? new DateTime(model.Contrat.FinEffetAnnee, model.Contrat.FinEffetMois, model.Contrat.FinEffetJour) : model.InformationsGenerales.DateSortieGarantie;
                            model.InformationsGenerales.HeureSortieGarantie = model.InformationsGenerales.HeureSortieGarantie == null ? AlbConvert.ConvertIntToTimeMinute(model.Contrat.FinEffetHeure) : model.InformationsGenerales.HeureSortieGarantie;
                        }
                    }
                    model.DateEffetAvenant = model.Contrat.DateEffetAvenant;
                    model.HeureEffetAvenant = model.Contrat.HeureEffetAvenant;
                    //Affichage de la navigation latérale en arboresence
                    model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Risque", codeRisque: Convert.ToInt32(model.CodeRisque));
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                    this.model.IsModifHorsAvenant = GetIsModifHorsAvn(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type);
                }

                model.InformationsGenerales.IsReadOnly = model.IsReadOnly;
                model.InformationsGenerales.IsModifHorsAvenant = model.IsModifHorsAvenant;
                model.InformationsGenerales.IsAvnDisabled = IsAvnDisabled;
                model.InformationsGenerales.DateModifiable = true;
                if (model.ListInventaires != null && model.ListInventaires.Inventaires != null) {
                    model.ListInventaires.Inventaires.IsReadOnly = model.IsReadOnly;
                }
                model.TitleInfoBulle = infoBulleRisque + Environment.NewLine + infoBulleObjet;
                model.InformationsGenerales.ListesNomenclatures.IsModeObjet = true;
            }
            //Si mode avenant et objet non créé dans l'avenant
            if (model.IsModeAvenant && (model.NumAvenantPage != model.AvnCreationObj.ToString() && model.CodeObjet != "0") && model.IsObjetSorti == true)
            {
                model.InformationsGenerales.DateModifiableAvn = false;
            }
            else
            {
                model.InformationsGenerales.DateModifiableAvn = true;
            }
            model.DateEffetAvenantModificationLocale = (model.IsAvenantModificationLocale || model.ModeCreationRisque) ? model.DateEffetAvenantModificationLocale : null;


        }

        private string GetInfoBulleRisque(RisqueDto currentRsq)
        {
            DateTime? dateEntree = null;
            DateTime? dateSortie = null;
            if (currentRsq.Objets.Count > 1)
            {
                dateEntree = currentRsq.EntreeGarantie;
                dateSortie = currentRsq.SortieGarantie;
            }
            else if (currentRsq.Objets.Count == 1) //mono objet
            {
                dateEntree = currentRsq.Objets[0].EntreeGarantie;
                dateSortie = currentRsq.Objets[0].SortieGarantie;
            }

            string infoBulleRisque = string.Empty;
            if (dateEntree.HasValue && dateSortie.HasValue)
                infoBulleRisque = currentRsq.Code + ", " + currentRsq.Descriptif + ", du " + dateEntree.Value.ToShortDateString() + " au " + dateSortie.Value.ToShortDateString();
            else if (dateEntree.HasValue)
                infoBulleRisque = currentRsq.Code + ", " + currentRsq.Descriptif + ", du " + dateEntree.Value.ToShortDateString();
            else if (dateSortie.HasValue)
                infoBulleRisque = currentRsq.Code + ", " + currentRsq.Descriptif + ", jusqu'au " + dateSortie.Value.ToShortDateString();
            else
                infoBulleRisque = currentRsq.Code + ", " + currentRsq.Descriptif;
            model.TitleInfoBulle = infoBulleRisque;
            return "Risque : " + infoBulleRisque;
        }

        private static string GetInfoBulleObjet(ObjetDto currentObj)
        {
            string infoBulleObjet = string.Empty;
            DateTime? dateEntree = currentObj.EntreeGarantie;
            DateTime? dateSortie = currentObj.SortieGarantie;
            if (dateEntree.HasValue && dateSortie.HasValue)
                infoBulleObjet = currentObj.Code + ", " + currentObj.Descriptif + ", du " + dateEntree.Value.ToShortDateString() + " au " + dateSortie.Value.ToShortDateString();
            else if (dateEntree.HasValue)
                infoBulleObjet = currentObj.Code + ", " + currentObj.Descriptif + ", du " + dateEntree.Value.ToShortDateString();
            else if (dateSortie.HasValue)
                infoBulleObjet = currentObj.Code + ", " + currentObj.Descriptif + ", jusqu'au " + dateSortie.Value.ToShortDateString();
            else
                infoBulleObjet = currentObj.Code + ", " + currentObj.Descriptif;
            return "Objet : " + infoBulleObjet;
        }

        private ObjetDto buildModifierOffre(ModeleDetailsObjetRisquePage model)
        {
            var toReturn = new ObjetDto
            {
                Code = Convert.ToInt32(model.CodeObjet),
                ChronoDesi = model.ChronoDesi,
                Descriptif = model.InformationsGenerales.Descriptif,
                Cible = new CibleDto
                {
                    Code = model.InformationsGenerales.Cible
                },
                DateModificationObjetRisque = model.DateModificationObjetRisque,
                Designation = !string.IsNullOrEmpty(model.InformationsGenerales.Designation) ? model.InformationsGenerales.Designation.Replace("\r\n", "<br>").Replace("\n", "<br>") : string.Empty,
                CodeApe = model.InformationsGenerales.CodeApe,
                Nomenclature1 = model.InformationsGenerales.ListesNomenclatures.Nomenclature1.Nomenclature,
                Nomenclature2 = model.InformationsGenerales.ListesNomenclatures.Nomenclature2.Nomenclature,
                Nomenclature3 = model.InformationsGenerales.ListesNomenclatures.Nomenclature3.Nomenclature,
                Nomenclature4 = model.InformationsGenerales.ListesNomenclatures.Nomenclature4.Nomenclature,
                Nomenclature5 = model.InformationsGenerales.ListesNomenclatures.Nomenclature5.Nomenclature,
                Territorialite = model.InformationsGenerales.Territorialite,
                CodeTre = model.InformationsGenerales.CodeTre,
                CodeClasse = model.InformationsGenerales.CodeClasse,
                TypeRisque = model.InformationsGenerales.TypeRisque,
                TypeMateriel = model.InformationsGenerales.TypeMateriel,
                NatureLieux = model.InformationsGenerales.NatureLieux,
                IsAvenantModificationLocale = model.IsAvenantModificationLocale,
                DateEffetAvenantModificationLocale = model.DateEffetAvenantModificationLocale,
                IsRisqueTemporaire = model.InformationsGenerales.IsRisqueTemporaire
            };




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
                        model.InformationsGenerales.DateEntreeDescr.Value.Day, 0, 0, 0);
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
                        model.InformationsGenerales.DateSortieDescr.Value.Day, 0, 0, 0);
                }
            }


            toReturn.Valeur = model.InformationsGenerales.Valeur;

            toReturn.Unite = new ParametreDto
            {
                Code = model.InformationsGenerales.Unite
            };

            toReturn.Type = new ParametreDto
            {
                Code = model.InformationsGenerales.Type
            };

            toReturn.ValeurHT = model.InformationsGenerales.ValeurHT;

            toReturn.CoutM2 = model.InformationsGenerales.CoutM2;

            Int32 numVoie = 0;
            Int32 cp = 0;
            Int32 cpCedex = 0;

            string cpFormat = !string.IsNullOrEmpty(model.ContactAdresse.CodePostal) ? model.ContactAdresse.CodePostal.Length >= 3 ? model.ContactAdresse.CodePostal.Substring(model.ContactAdresse.CodePostal.Length - 3, 3)
                                                                                                                                    : model.ContactAdresse.CodePostal
                                                                                      : string.Empty;
            string cpCedexFormat = !string.IsNullOrEmpty(model.ContactAdresse.CodePostalCedex) ? model.ContactAdresse.CodePostalCedex.Length >= 3 ? model.ContactAdresse.CodePostalCedex.Substring(model.ContactAdresse.CodePostalCedex.Length - 3, 3)
                                                                                                                        : model.ContactAdresse.CodePostalCedex
                                                                          : string.Empty;

            toReturn.AdresseObjet = new AdressePlatDto
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
                Departement = !string.IsNullOrEmpty(model.ContactAdresse.CodePostal) && model.ContactAdresse.CodePostal.Length == 5 ? model.ContactAdresse.CodePostal.Substring(0, 2) :
                              (!string.IsNullOrEmpty(model.ContactAdresse.CodePostalCedex) && model.ContactAdresse.CodePostalCedex.Length >= 2 ? model.ContactAdresse.CodePostalCedex.Substring(0, 2) : ""),
                Latitude = !model.ContactAdresse.Latitude.IsEmptyOrNull() ? Convert.ToDecimal(model.ContactAdresse.Latitude.Replace(".", ",")) : 0,
                Longitude = !model.ContactAdresse.Longitude.IsEmptyOrNull() ? Convert.ToDecimal(model.ContactAdresse.Longitude.Replace(".", ",")) : 0
            };

            return toReturn;
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
            model.InformationsGenerales.DisplayInfosValeur = true;
            model.ListInventaires = new ModeleDetailsObjetInventaire();

            model.ContactAdresse = new ModeleContactAdresse(11, false, true);
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

        /// <summary>
        /// Wi 3492 - T 3493
        /// Contrôle de la valeur totale risque  - Ecran Risque - Réinitialisation Valeur Totale Risque
        /// Condition initiale : Branche RT - Cible ENR - Offre/AN/Avenant
        /// Si après ajouté ,modifié ou supprimé objet de risque l'ensemble des type de valeur des objets de ce risque
        /// devient hétérogène , au retour sur l'écran "Risque" les 4 champs qui composent la "Valeur Totale Risque"
        /// Doivent etre vide 
        /// => L'utilisateur conserve la possiblité de ressaisir les champs qui composent la "Valeur Totale Risque"
        /// </summary>
        /// <param name="code">Code offre / contrat</param>
        /// <param name="type">Type : O ou P</param>
        /// <param name="version">Version</param>
        /// <param name="riskId">Code risque</param>
        /// <param name="numAvn">Numero avenant</param>
        /// <param name="previousInfoRisk">information risque</param>
        private void ControlRiskTotalValue(string code, string type, int? version, int riskId, string numAvn, RisqueDto previousInfoRisk)
        {
            try
            {
                RisqueDto currentInfoRisk = null;
                const string brancheRt = "RT";
                const string cibleEnr = "ENR";

                // Validation condition Branche RT - Cible ENR - Multiobjet hétérogène
                if (previousInfoRisk?.Objets?.Count >= 1 && previousInfoRisk?.Cible?.CodeBranche == brancheRt && previousInfoRisk?.Cible.Code == cibleEnr)
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                    {

                        // informations risque par code risque
                        currentInfoRisk = GetRisk(code, riskId, version, type, numAvn);


                        if (currentInfoRisk?.Objets?.Count >= 1)
                        {

                            bool? mixedRiskPreviousState = IsMixedRiskValue(previousInfoRisk?.Objets);


                            var mixedRiskCurrentState = IsMixedRiskValue(currentInfoRisk?.Objets);

                            if (mixedRiskPreviousState == false && mixedRiskCurrentState == true)
                            {

                                // vider la "Valeur Totale Risque"
                                client.Channel.UpdateRiskTotalValue(code, version, riskId, null, "", "", "");
                            }
                            else
                            {

                                if (mixedRiskPreviousState == true && mixedRiskCurrentState == false)
                                {
                                    //recalculer la "Valeur Totale Risque"
                                    var riskvalue = currentInfoRisk.Objets.Where(x => x.Valeur != null).Sum(x => x.Valeur);
                                    var riskUnit = currentInfoRisk.Objets.FirstOrDefault()?.Unite?.Code ?? "";
                                    var riskType = currentInfoRisk.Objets.FirstOrDefault()?.Type?.Code ?? "";
                                    var riskRate = currentInfoRisk.Objets.FirstOrDefault()?.ValeurHT ?? "";
                                    client.Channel.UpdateRiskTotalValue(code, version, riskId, riskvalue, riskUnit, riskType, riskRate);
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        /// <summary>
        /// Test si les objets du risque sont hétérogènes. 
        /// si nombre des objets = 1 => homogènes
        /// si les (types ou unités ou taux) sont différents => hétérogènes sinon homogènes
        /// </summary>
        /// <param name="objets"> liste des objets</param>
        /// <returns></returns>
        private bool? IsMixedRiskValue(List<ObjetDto> objets)
        {
            if (objets != null)
            {
                if (objets.Count == 1)
                {
                    return false;
                }
                else
                {
                    return ((objets?.Select(x => x.Type.Code).Distinct().Skip(1).Any() == true) ||
                                       (objets?.Select(x => x.Unite.Code).Distinct().Skip(1).Any() == true) ||
                                       (objets?.Select(x => x.ValeurHT).Distinct().Skip(1).Any() == true));
                }
            }
            return null;
        }
        /// <summary>
        /// Informations risque par code risque
        /// </summary>
        /// <param name="code"> code offre / contrat</param>
        /// <param name="riskCode">code risque</param>
        /// <param name="version">version offre / contrat</param>
        /// <param name="type">type offre / contrat</param>
        /// <param name="codeavn">Numéro Avn</param>
        /// <returns></returns>
        private RisqueDto GetRisk(string code, int riskCode, int? version, string type, string codeavn)
        {
            RisqueDto risk = null;

            using (var clientRisk = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                risk = clientRisk.Channel.ObtenirRisque(model.ModeNavig.ParseCode<ModeConsultation>(), code, riskCode, version, type, codeavn);
            }
            return risk;
        }

        #endregion

    }
}
