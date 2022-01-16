using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.ModelesClauses;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRecherche;
using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;
using EmitMapper;
using Mapster;
using OP.WSAS400.DTO.Contrats;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using OP.WSAS400.DTO.LTA;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using OPServiceContract.IAdministration;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class RechercheSaisieController : ControllersBase<ModeleRecherchePage>
    {
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            if (MvcApplication.ENVIRONMENT_NAME.ToLower() == AlbOpConstants.OPENV_PROD.ToLower() || MvcApplication.ENVIRONMENT_NAME.ToLower() == AlbOpConstants.OPENV_HOTFIX.ToLower())
            {
                var isBlocked = GetBlocageSaisieAS400();
                if (isBlocked)
                {
                    return PartialView("serviceoff");
                }
            }
            return Init(id);
        }


        #region ControllersBase Init
        protected override string FormatId(string id)
        {
            return WebUtility.UrlDecode(id);
        }

        protected override void SetPageTitle()
        {
            model.PageTitle = "Recherche d'une offre ou d'un contrat";
        }

        protected override string InitializeParams(string id, bool strictMode = true)
        {
            if (id.ContainsChars())
            {
                //Uniquement pour le test ci-dessous
                base.InitializeParams(id, strictMode);
                var parameters = model.AllParameters;
                var folderIds = parameters.FolderId.Split('_');
                if (folderIds.Length >= 3)
                {
                    if (AlbSessionHelper.ParametresRecherches != null && !parameters.IsReloadSearchParam)
                    {
                        ConvertParamRechercheToPage(AlbSessionHelper.ParametresRecherches, model);
                    }

                    if (id.Contains("addParam"))
                    {
                        var queryAddParams = id.Split(new[] { "addParam" }, StringSplitOptions.None);
                        model.AddParamVerrouille = "addParam" + queryAddParams[1] + "addParam";
                    }

                    model.OffreVerrouille = parameters.Folder.CodeOffre;
                    model.VersionVerrouille = parameters.Folder.Version.ToString();
                    model.TypeVerrouille = parameters.Folder.Type;
                    model.AvnVerrouille = parameters.NumeroAvenant.StringValue("0");

                    if (parameters.IsReloadSearchParam)
                    {
                        model.LoadParamOffre = true;
                    }
                    if (model.AllParameters.LockingUser.ContainsChars())
                    {
                        model.UserVerrouille = model.AllParameters.LockingUser;
                    }
                }
                //Sauvegarde réelle des paramètres
                id = base.InitializeParams(id, strictMode);
            }
            return id;
        }

        protected override void LoadInfoPage(string context)
        {
            //supression des offres vérouillées par l'utilisateur
            LoadListesRecherche(model);

            // affichage des impayés
            this.model.ListeRelances = new ModelRelances();
            //try
            //{
            //    if (this.model.ProfileKheops?.ShowImpayesOnStartup == true)
            //    {
            //        this.model.ListeRelances.DoNotShowAgainForToday = false;
            //        using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            //        {
            //            var pagingList = client.Channel.GetUserRelances();
            //            if (pagingList != null)
            //            {
            //                this.model.ListeRelances.Relances = pagingList.List.Adapt<List<RelanceOffre>>();
            //                this.model.ListeRelances.NombreRelances = pagingList.NbTotalLines;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        this.model.ListeRelances.DoNotShowAgainForToday = true;
            //    }
            //}
            //catch (Exception e)
            //{
            //    AlbSessionHelper.MessageErreurEcran = "<b>Erreur lors de la récupération des relance.</b>";
            //    new Framework.Common.AlbingiaExceptions.AlbTechException(e, true, true);
            //    this.model.ListeRelances.DoNotShowAgainForToday = true;
            //}
        }

        protected override void UpdateModel()
        {
            model.PageEnCours = NomsInternesEcran.RechercheSaisie.ToString();
            model.CritereParam = AlbConstantesMetiers.CriterParam.Standard;
            model.AlbEmplacement = NomsInternesEcran.RechercheSaisie.ToString();
            model.SearchInTemplate = false;
            model.IsCheckOffre = true;
            model.IsCheckContrat = true;
            model.IsCheckEnCours = true;
            model.IsCheckInactif = true;
            model.IsCheckEtat = false;
            model.IsCheckApporteur = true;
            model.IsCheckGestionnaire = true;
            model.ModeNavig = ModeConsultation.Standard.AsCode();
            DisplayBandeau();
        }
        #endregion

        [ErrorHandler]
        public PartialViewResult Recherche(ModeleParametresRecherche parametresRecherche)
        {
            ModeleResultRecherche resultData = GetResultSearchData(ref parametresRecherche);
            return PartialView("BodyResultatsRecherche", resultData);
        }

        internal static ModeleResultRecherche GetResultSearchData(ref ModeleParametresRecherche parametresRecherche)
        {
            ModeleResultRecherche resultData = null;
            if (!string.IsNullOrEmpty(parametresRecherche.CodeOffre))
            {
                parametresRecherche = new ModeleParametresRecherche
                {
                    CodeOffre = parametresRecherche.CodeOffre,
                    NumAliment = parametresRecherche.NumAliment,
                    PageNumber = parametresRecherche.PageNumber,
                    AccesRecherche = parametresRecherche.AccesRecherche,
                    CritereParam = parametresRecherche.CritereParam,
                    ModeNavig = parametresRecherche.ModeNavig
                };
            }

            #region Préparation des critères de recherche
            if (!string.IsNullOrEmpty(parametresRecherche.DateDebut))
            {
                parametresRecherche.DDateDebut = Convert.ToDateTime(parametresRecherche.DateDebut);
            }

            if (!string.IsNullOrEmpty(parametresRecherche.DateFin))
            {
                parametresRecherche.DDateFin = Convert.ToDateTime(parametresRecherche.DateFin);
            }

            if (!string.IsNullOrEmpty(parametresRecherche.Sorting))
            {
                if (parametresRecherche.Sorting != "undefined")
                {
                    var tableTri = parametresRecherche.Sorting.Split('_');
                    parametresRecherche.SortingName = tableTri[0];
                    parametresRecherche.SortingOrder = tableTri[1];
                }
            }
            else if (!string.IsNullOrEmpty(parametresRecherche.CodeOffre))
            {
                parametresRecherche.SortingName = "PBIPB";
                parametresRecherche.SortingOrder = "ASC";
            }
            else
            {
                var order = "DESC";
                switch (parametresRecherche.TypeDateRecherche)
                {
                    case AlbConstantesMetiers.TypeDateRecherche.Saisie:
                        parametresRecherche.SortingName = "dateSaisie";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.Effet:
                        parametresRecherche.SortingName = "dateEffet";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.MAJ:
                        parametresRecherche.SortingName = "dateMaj";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.Creation:
                        parametresRecherche.SortingName = "dateCreation";
                        break;
                    default:
                        order = parametresRecherche.SortingOrder;
                        break;
                }
                parametresRecherche.SortingOrder = order;
            }
            parametresRecherche.StartLine = ((parametresRecherche.PageNumber - 1) * MvcApplication.PAGINATION_PAGE_SIZE) + 1;
            parametresRecherche.EndLine = (parametresRecherche.PageNumber) * MvcApplication.PAGINATION_PAGE_SIZE;
            parametresRecherche.LineCount = MvcApplication.PAGINATION_SIZE;

            if (parametresRecherche.CheckOffre && !parametresRecherche.CheckContrat)
            {
                parametresRecherche.Type = AlbConstantesMetiers.TYPE_OFFRE;
            }
            else if ((!parametresRecherche.CheckOffre && parametresRecherche.CheckContrat) || parametresRecherche.CritereParam == AlbConstantesMetiers.CriterParam.ContratOnly)
            {
                parametresRecherche.Type = AlbConstantesMetiers.TYPE_CONTRAT;
            }
            else
            {
                parametresRecherche.Type = string.Empty;
            }

            if (string.IsNullOrEmpty(parametresRecherche.TypeContrat))
            {
                parametresRecherche.TypeContrat = string.Empty;
            }

            #endregion

            //Mise en cache des paramètres de recherche
            if ((parametresRecherche.AccesRecherche == AlbConstantesMetiers.TypeAccesRecherche.Standard || parametresRecherche.AccesRecherche == AlbConstantesMetiers.TypeAccesRecherche.BlackListed) && parametresRecherche.CritereParam == AlbConstantesMetiers.CriterParam.Standard)
            {
                AlbSessionHelper.ParametresRecherches = parametresRecherche;
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var screenClient = client.Channel;
                var paramRechercheDto = ObjectMapperManager.DefaultInstance.GetMapper<ModeleParametresRecherche, ModeleParametresRechercheDto>().Map(parametresRecherche);
                paramRechercheDto.SouscripteurNom = string.Empty;
                var result = screenClient.RechercherOffresContrat(paramRechercheDto, parametresRecherche.ModeNavig.ParseCode<ModeConsultation>());

                var isUserHorse = MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0;

                if (result != null)
                {
                    var listResultRecherche = new List<ModeleListResultRecherche>();

                    var i = 0;
                    foreach (var m in result.LstOffres)
                    {
                        List<string> cibles = new List<string> { "RCCEQ", "GRCEQSAL", "IMACEQ", "GRCEQCLI", "EQDOM" };
                        if (!isUserHorse && cibles.Any(c => m.Branche.Cible.Code.Contains(c)))
                        {
                            continue;
                        }

                        m.Compteur = i;
                        var recherche = (ModeleListResultRecherche)m;
                        recherche.TypeDate = parametresRecherche.TypeDateRecherche;
                        listResultRecherche.Add(recherche);
                        i++;
                    }

                    var totalLines = result.NbCount < paramRechercheDto.LineCount ? result.NbCount : paramRechercheDto.LineCount;
                    resultData = new ModeleResultRecherche
                    {
                        NbCount = result.NbCount,
                        StartLigne = paramRechercheDto.StartLine,
                        EndLigne = paramRechercheDto.EndLine,
                        PageNumber = parametresRecherche.PageNumber,
                        LineCount = totalLines,
                        ListResult = listResultRecherche,
                        CodeBranche = paramRechercheDto.Branche
                    };
                }
            }
            if (resultData == null)
            {
                resultData = new ModeleResultRecherche();
            }

            resultData.AccesRecherche = parametresRecherche.AccesRecherche;
            resultData.CritereParam = parametresRecherche.CritereParam;
            resultData.TypeDate = parametresRecherche.TypeDateRecherche;
            return resultData;
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult DernierNumeroVersionOffre(string codeOffre, string type, string version)
        {
            var toReturn = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var rechercheSaisieClient = client.Channel;
                toReturn.Data = rechercheSaisieClient.DernierNumeroVersionOffreMotifSituation(codeOffre, type, version);
            }
            return toReturn;
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult CreationNouvelleVersionOffre(string codeOffre, string version, string type, string etat, string situation)
        {
            var offreId = new AffaireId { TypeAffaire = AffaireType.Offre, IsHisto = false, CodeAffaire = codeOffre, NumeroAliment = int.Parse(version) };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffairePort>())
            {
                var v = client.Channel.TryLockAffaire(offreId, "VERSION OFFRE");
                if (v != null)
                {
                    throw new AlbFoncException($"L'offre est verrouillée par {v.User}, impossible de créer une nouvelle version.");
                }
            }
            var toReturn = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            try
            {
                if (string.IsNullOrEmpty(etat) || string.IsNullOrEmpty(situation))
                {
                    toReturn.Data = false;
                }
                else
                {
                    if ((etat.ToUpper().Equals("V") && situation.ToUpper().Equals("A")) || ((etat.ToUpper().Equals("V") || etat.ToUpper().Equals("N") || etat.ToUpper().Equals("A")) && situation.ToUpper().Equals("W")))
                    {
                        using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
                        {
                            var rechercheSaisieClient = client.Channel;
                            toReturn.Data = rechercheSaisieClient.CreationNouvelleVersionOffre(codeOffre, version, type, GetUser(), "V");
                        }
                    }
                    else
                    {
                        toReturn.Data = false;
                    }
                }
            }
            finally
            {
                try
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffairePort>())
                    {
                        client.Channel.UnockAffaireList(new[] { offreId });
                    }
                }
                catch (Exception ex)
                {
                    AlbLog.Log($"Unable to unlock {codeOffre}{Environment.NewLine}{ex}", AlbLog.LogTraceLevel.Erreur);
                }
            }
            return toReturn;
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(
            string cible, string job,
            string paramOffre, string paramCreate, string tabGuid, string modeNavig = "modeNavigSmodeNavig", string newWindow = "",
            string typeAvt = "", string modeAvt = "", bool ignoreReadonly = false, string typeAvtResil = "")
        {

            newWindow = newWindow.ContainsChars() ? ("_" + newWindow) : newWindow;
            var typeOffre = string.Empty;
            if (!string.IsNullOrEmpty(paramOffre))
            {
                typeOffre = paramOffre.Split('_')[2];
            }

            // Parametre additionnel
            var addParam = string.Empty;

            if (typeOffre == AlbConstantesMetiers.TYPE_CONTRAT)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
                {
                    var serviceContext = client.Channel;
                    var workParam = string.Empty;

                    if (paramOffre != null && paramOffre.Split('_').Length > 3 && paramOffre.Split('_')[3] != "0")
                    {
                        workParam = "||" + AlbParameterName.AVNID + "|" + paramOffre.Split('_')[3];
                        workParam += "||" + AlbParameterName.AVNIDEXTERNE + "|" + paramOffre.Split('_')[3];
                    }

                    if (!string.IsNullOrEmpty(modeAvt))
                    {
                        if (modeAvt == "UPDATE")
                        {
                            typeAvt = serviceContext.GetTypeAvenant(paramOffre);
                        }

                        workParam += "||" + AlbParameterName.AVNMODE + "|" + modeAvt;
                    }
                    if (!string.IsNullOrEmpty(typeAvt))
                    {
                        workParam += "||" + AlbParameterName.AVNTYPE + "|" + typeAvt;
                    }

                    if (!string.IsNullOrEmpty(typeAvtResil))
                    {
                        workParam += "||" + AlbParameterName.AVNTYPERESIL + "|" + typeAvtResil;
                    }

                    if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
                    {
                        if (paramOffre != null)
                        {
                            var reguleId = serviceContext.GetNumRegule(paramOffre.Split('_')[0], paramOffre.Split('_')[1], paramOffre.Split('_')[2], paramOffre.Split('_')[3]);
                            workParam += "||" + AlbParameterName.REGULEID + "|" + reguleId;

                            if (reguleId > 0)
                            {
                                string[] tId = paramOffre.Split('_');
                                LigneRegularisationDto result = null;
                                using (var clientReg = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
                                {
                                    var serviceContextReg = clientReg.Channel;
                                    result = serviceContextReg.GetRegularisationByID(tId[0], tId[1], tId[2], reguleId);
                                }
                                if (result != null)
                                {
                                    if (result.NumRegule > 0)
                                    {
                                        workParam += "||" + AlbParameterName.REGULEID + "|" + result.NumRegule.ToString();
                                    }

                                    if (!string.IsNullOrEmpty(result.RegulMode))
                                    {
                                        workParam += "||" + AlbParameterName.REGULMOD + "|" + result.RegulMode;
                                    }

                                    if (!string.IsNullOrEmpty(result.RegulType))
                                    {
                                        workParam += "||" + AlbParameterName.REGULTYP + "|" + result.RegulType;
                                    }

                                    if (!string.IsNullOrEmpty(result.RegulNiv))
                                    {
                                        workParam += "||" + AlbParameterName.REGULNIV + "|" + result.RegulNiv;
                                    }

                                    if (!string.IsNullOrEmpty(result.RegulAvn))
                                    {
                                        workParam += "||" + AlbParameterName.REGULAVN + "|" + result.RegulAvn;
                                    }

                                }
                            }
                        }
                        workParam += "||" + AlbParameterName.ACTEGESTIONREGULE + "|REGUL";
                    }
                    if (ignoreReadonly)
                    {
                        workParam += "||" + AlbParameterName.IGNOREREADONLY + "|1";
                    }

                    if (cible == "ControleFin")
                    {
                        workParam += "||" + AlbParameterName.VALIDATION + "|1";
                    }

                    if (!string.IsNullOrEmpty(workParam))
                    {
                        addParam = "addParam" + AlbOpConstants.GLOBAL_TYPE_ADD_PARAM_AVN + "|||" + workParam.Substring(2) + "addParam";
                    }

                    if (paramOffre != null)
                    {
                        paramOffre = paramOffre.Split('_')[0] + "_" + paramOffre.Split('_')[1] + "_" + paramOffre.Split('_')[2];
                        var offreMere = serviceContext.GetOffreMere(paramOffre);

                        if (!string.IsNullOrEmpty(offreMere))
                        {
                            return RedirectToAction(
                                "ChoixRisques",
                                "CreationAffaireNouvelle",
                                new
                                {
                                    id =
                                    AlbParameters.BuildPipedParams(new Dictionary<PipedParameter, IEnumerable<string>> {
                                        { PipedParameter.ACTION, new List<string> { "OffreToAffaire" } },
                                        { PipedParameter.IPB, new List<string> { offreMere.Split('_')[0].Trim(), paramOffre.Split('_')[0] } },
                                        { PipedParameter.ALX, new List<string> { offreMere.Split('_')[1], paramOffre.Split('_')[1] } },
                                        { PipedParameter.TYP, new List<string> { "O" } },
                                        { PipedParameter.GUIDKEY, new List<string> { GetSurroundedTabGuid(tabGuid) } }
                                    })
                                });
                        }
                    }
                }
            }
            else if (typeOffre == AlbConstantesMetiers.TYPE_OFFRE)
            {
                var workParam = string.Empty;
                if (cible == "ControleFin")
                {
                    workParam += "||" + AlbParameterName.VALIDATION + "|1";
                }
                if (!string.IsNullOrEmpty(workParam))
                {
                    addParam = "addParam" + AlbOpConstants.GLOBAL_TYPE_ADD_PARAM_AVN + "|||" + workParam.Substring(2) + "addParam";
                }
            }

            if (!string.IsNullOrEmpty(paramCreate) || cible == "CreationSaisie")
            {
                return RedirectToAction(job, cible, new { id = string.Concat(paramCreate, GetSurroundedTabGuid(tabGuid), modeNavig, newWindow) });
            }

            if (cible == "AnCreationContrat")
            {
                return RedirectToAction(job, cible, new { id = string.Concat(newWindow, GetSurroundedTabGuid(tabGuid), modeNavig) });
            }

            if (cible == "AnnulationQuittances")
            {
                return RedirectToAction(job, cible, new { id = string.Concat(paramOffre, GetSurroundedTabGuid(tabGuid), addParam, modeNavig, newWindow, "reprise1reprise") });
            }

            if (string.Compare(cible, "CommonNavigation", true) == 0)
            {
                return RedirectToAction(job, cible, new { id = string.Concat(paramOffre, "¤cibleRechercheSaisieciblejobIndexjob", GetSurroundedTabGuid(tabGuid), newWindow) });
            }

            return RedirectToAction(job, cible, new { id = string.Concat(paramOffre, GetSurroundedTabGuid(tabGuid), addParam, modeNavig, newWindow) });
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult CreateCopyOffre(string codeOffreCopy, string versionCopy, string type, string tabGuid, string newWindow = "")
        {
            newWindow = !string.IsNullOrEmpty(newWindow) ? "_" + newWindow : newWindow;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var serviceContext = client.Channel;
                var toReturn = serviceContext.CheckOffreCreate(codeOffreCopy, versionCopy, type);
                if (!string.IsNullOrEmpty(toReturn))
                {
                    throw new AlbFoncException(toReturn);
                }
            }
            return RedirectToAction("Index", type == AlbConstantesMetiers.TYPE_OFFRE ? "CreationSaisie" : "AnCreationContrat", new { id = codeOffreCopy + "_" + versionCopy + "_" + type + "_C" + GetSurroundedTabGuid(tabGuid), newWindow });
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult CopyOffre(string codeOffre, string version, string codeOffreCopy, string versionCopy, string type)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var toReturn = client.Channel.CheckOffreCopy(codeOffre, version, codeOffreCopy, versionCopy, type);
                if (!string.IsNullOrEmpty(toReturn))
                {
                    throw new AlbFoncException(toReturn);
                }
            }
            return RedirectToAction("Index", "CreationSaisie", new { id = codeOffreCopy + "_" + versionCopy + "_C_" + codeOffre + "_" + version + "_" + type });
        }

        [ErrorHandler]
        public string VerifyTraceOffre(string codeOffre, string version, string type)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                return serviceContext.VerifyTraceOffre(codeOffre, version, type, "BASE", "BASE") ? "OK" : "KO";
            }
        }

        [ErrorHandler]
        public void RefusOffre(string codeOffre, string version, string codeMotif)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                serviceContext.RefusOffre(codeOffre, version, codeMotif);
            }
        }

        [ErrorHandler]
        public ActionResult GetCibles(string codeBranche, string codeCible)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                var toReturn = new ModeleListeCibles();
                var result = serviceContext.GetCibles(codeBranche, true, CacheUserRights.UserRights.Any(
                el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()), MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                if (result == null)
                {
                    return PartialView("ListeCibles", toReturn);
                }

                toReturn.Cible = string.Empty;
                toReturn.Cibles = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = codeCible != string.Empty ? m.Code == codeCible : false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();

                var exists = false;

                if (!string.IsNullOrEmpty(codeBranche) && !string.IsNullOrEmpty(codeCible))
                {
                    foreach (var item in toReturn.Cibles)
                    {
                        exists = item.Title.StartsWith(codeCible);
                    }
                }
                if (codeCible != string.Empty && exists)
                {
                    if (toReturn.Cibles.Find(c => c.Value == codeCible) != null)
                    {
                        toReturn.Cibles.Find(c => c.Value == codeCible).Selected = true;
                    }
                }

                return PartialView("ListeCibles", toReturn);
            }
        }

        [ErrorHandler]
        public ActionResult GetBandeau(string id, string modeNavig = null)
        {
            #region vérification des paramètres
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            var tIds = id.Split('_');
            if (modeNavig == null)
            {
                modeNavig = tIds.Length == 3 ? ModeConsultation.Standard.AsCode() : ModeConsultation.Historique.AsCode();
            }
            else if (tIds.Length != 4)
            {
                return null;
            }

            string code = tIds[0];
            string version = tIds[1];
            string type = tIds[2];
            string avenant = tIds.Length == 3 ? null : tIds[3];
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(type))
            {
                return null;
            }

            if (!int.TryParse(version, out int iVersion))
            {
                return null;
            }

            if (avenant != null && !int.TryParse(avenant, out int iAvenant))
            {
                return null;
            }
            #endregion

            var bandeau = GetInfoBandeauRechercheSaisie(id, ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>());
            if (bandeau != null)
            {
                bandeau.ContexteBandeau = "recherche";
                if (type == AlbConstantesMetiers.TYPE_OFFRE)
                {
                    return PartialView("Entete/InfoBandeau", bandeau);
                }
                else if (type == AlbConstantesMetiers.TYPE_CONTRAT)
                {
                    SetBlocageInfoMessage(bandeau, bandeau.StopContentieux, bandeau.Stop);
                    return PartialView("Entete/InfoBandeau", bandeau);
                }
            }
            return null;
        }

        [ErrorHandler]
        public ActionResult GetRechercheCopieOffre(string typeEcran, bool searchInTemplate)
        {
            var rechercheCopieOffre = new ModeleRecherchePage
            {
                CritereParam = typeEcran == AlbConstantesMetiers.SCREEN_TYPE_OFFRE ? AlbConstantesMetiers.CriterParam.CopyOffre : AlbConstantesMetiers.CriterParam.ContratOnly,
                ProvenanceParam = "connexite",
                SituationParam = string.Empty,
                ListCabinetCourtage = new ModeleRechercheAvancee(),
                ListPreneurAssurance = new ModeleRechercheAvancee(),
                AlbEmplacement = "RechercheCopieOffre",
                SearchInTemplate = searchInTemplate
            };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var screenClient = client.Channel;
                var query = new RechercheSaisieGetQueryDto();
                var result = screenClient.RechercheSaisieGet(query);
                if (result != null)
                {
                    var cibles = result.Cibles.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    rechercheCopieOffre.ModeleCibles = new ModeleListeCibles { Cibles = cibles };
                    var branches = result.Branches.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    rechercheCopieOffre.Branches = branches;
                    var etats = result.Etats.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    rechercheCopieOffre.Etats = etats;
                    var situations = result.Situation.Select(m => new AlbSelectListItem
                    {
                        Value = m.Code,
                        Text = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty,
                        Selected = false,
                        Title = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty
                    }).ToList();
                    rechercheCopieOffre.Situations = situations;
                    var listRefus = result.ListRefus.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    rechercheCopieOffre.ListRefus = listRefus;
                    rechercheCopieOffre.DateTypes = AlbTransverse.InitDateType;
                }
            }

            return View("RechercheSaisieCopieOffre", rechercheCopieOffre);
        }

        [ErrorHandler]
        public ActionResult GetCreationSaisieDiv(string type)
        {
            var toReturn = new ModeleDivCreation { TypeEcran = type };
            return PartialView("DivCreation", toReturn);
        }

        #region Cabinet Courtage Recherche avancée

        [HttpPost]
        public ActionResult GetCabinetsCourtagesRechercherByName(string nomCabinetRecherche, int pageNumber, string order, int by, string contexte)
        {
            if (nomCabinetRecherche.Length == 0)
            {
                nomCabinetRecherche = "%";
            }
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            var model = new ModeleRechercheAvancee
            {
                StartLigne = ((pageNumber - 1) * PageSize) + 1,
                EndLigne = (pageNumber) * PageSize,
                LineCount = MvcApplication.PAGINATION_SIZE,
                PageNumber = pageNumber,
                Order = order,
                By = by,
                CabinetsPreneurs = new List<ModeleCommonCabinetPreneur>(),
                Contexte = contexte
            };


            var cabinetCourtage =
                CabinetCourtageGet(new CabinetCourtageGetQueryDto { NomCabinet = nomCabinetRecherche, DebutPagination = model.StartLigne, FinPagination = model.EndLigne, Order = order, By = by });

            if (cabinetCourtage.CabinetCourtages.Any())
            {
                cabinetCourtage.CabinetCourtages.ForEach(cabinet => model.CabinetsPreneurs.Add((ModeleCommonCabinetPreneur)cabinet));
            }

            model.NbCount = cabinetCourtage.CabinetCourtageCount;

            if (model.NbCount < model.LineCount)
            {
                model.LineCount = model.NbCount;
            }
            return PartialView("ResultRechercheAvanceeCabinetCourtier", model);
        }

        [HttpPost]
        public ActionResult GetCabinetsCourtagesRechercherByCode(string codeCabinetRecherche, string contexte)
        {
            var model = new ModeleRechercheAvancee { Contexte = contexte };
            if (int.TryParse(codeCabinetRecherche, out int iCodeCabinetRecherche))
            {
                model.CabinetsPreneurs = GetCabinetsCourtagesByCodeImplementation(iCodeCabinetRecherche, 0, 50);
            }
            return PartialView("ResultRechercheAvanceeCabinetCourtier", model);
        }

        private List<ModeleCommonCabinetPreneur> GetCabinetsCourtagesByCodeImplementation(int code, int debutPagination, int finPagination)
        {
            var toReturn = new List<ModeleCommonCabinetPreneur>();
            var cabinetCourtage =
                CabinetCourtageGet(new CabinetCourtageGetQueryDto { Code = code, DebutPagination = debutPagination, FinPagination = finPagination, Order = "ASC", By = 1 });

            if (cabinetCourtage.CabinetCourtages.Any())
            {
                cabinetCourtage.CabinetCourtages.ForEach(cabinet => toReturn.Add((ModeleCommonCabinetPreneur)cabinet));
            }

            return toReturn;
        }

        private CabinetCourtageGetResultDto CabinetCourtageGet(CabinetCourtageGetQueryDto query)
        {
            CabinetCourtageGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.CabinetCourtageGet(query, false);
            }
            return toReturn;
        }

        private AssureGetResultDto AssureGet(AssureGetQueryDto query)
        {
            AssureGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.AssuresGet(query, false);
            }
            return toReturn;
        }

        #endregion

        #region Preneur Assurance Recherche avancée

        private AssureGetResultDto PreneurAssuranceGet(AssureGetQueryDto query)
        {
            AssureGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.AssuresGet(query, false);
            }
            return toReturn;
        }

        [ErrorHandler]
        public ActionResult GetPreneurAssuranceRechercher(string code, string name, string cp, int pageNumber, string order, int by)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "%";
            }
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            var model = new ModeleRechercheAvancee
            {
                StartLigne = ((pageNumber - 1) * PageSize) + 1,
                EndLigne = (pageNumber) * PageSize,
                LineCount = MvcApplication.PAGINATION_SIZE,
                PageNumber = pageNumber,
                Order = order,
                By = by,
                CabinetsPreneurs = new List<ModeleCommonCabinetPreneur>(),
                IsLastPage = false
            };

            var preneurAssurance = PreneurAssuranceGet(new AssureGetQueryDto
            {
                Code = code,
                NomAssure = name,
                CodePostal = cp,
                DebutPagination = model.StartLigne,
                FinPagination = model.EndLigne,
                Order = model.Order,
                By = model.By
            });

            if (preneurAssurance.Assures.Any())
            {
                preneurAssurance.Assures.ForEach(assure => model.CabinetsPreneurs.Add((ModeleCommonCabinetPreneur)assure));
            }

            model.NbCount = preneurAssurance.AssuresCount;

            if (model.NbCount < model.LineCount)
            {
                model.LineCount = model.NbCount;
                if (model.StartLigne <= model.NbCount && model.NbCount <= model.EndLigne)
                {
                    model.EndLigne = model.NbCount;
                    model.IsLastPage = true;
                }
            }
            else if (model.LineCount == model.EndLigne)
            {
                model.IsLastPage = true;
            }

            return PartialView("ResultRechercheAvanceePreneurAssurance", model);
        }

        #endregion

        [ErrorHandler]
        public string ChangeModeNavig(string currentModeNavig)
        {
            var currentMode = currentModeNavig.ParseCode<ModeConsultation>();
            switch (currentMode)
            {
                case ModeConsultation.Standard:
                    return ModeConsultation.Historique.AsCode();
                case ModeConsultation.Historique:
                    return ModeConsultation.Standard.AsCode();
                default:
                    return string.Empty;
            }
        }

        [ErrorHandler]
        public string VerifierVerrouillage(string codeOffre, string version, string type, string numAvn)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient = client.Channel;
                return voletsBlocsCategoriesClient.GetUserVerrou(codeOffre, version, type, numAvn);
            }
        }

        [ErrorHandler]
        public void ClearSession()
        {
            Session["ParamtresRecherches"] = null;
        }

        [ErrorHandler]
        public void ConfirmReprise(string codeOffre, string version, string type, string codeAvt, string typeAvt)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var serviceContext = client.Channel;
                serviceContext.ConfirmReprise(codeOffre, version, type, codeAvt, typeAvt, GetUser());
            }
        }

        [ErrorHandler]
        public string GetUserRole(string codeOffre, string version, string type, string branche, string typeAvt)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetEtatOffre(codeOffre, version, type);
                if (result == "C")
                {
                    return "ERROR_CITRIX";
                }

                if (result == "R")
                {
                    return "ERROR_REALISE";
                }
            }
            var userRights = CacheUserRights.UserRights.Any(elU => (elU.Branche == branche || elU.Branche == "**") && elU.TypeDroit != TypeDroit.V.ToString());

            return !userRights ? "ERROR_RIGHTS" : string.Empty;
        }


        [ErrorHandler]
        public string CheckPrimeAvt(string codeContrat, string version, string type, string codeAvn)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var serviceContext = client.Channel;
                return serviceContext.CheckPrimeAvt(codeContrat, version, type, codeAvn);
            }
        }

        [ErrorHandler]
        public ActionResult GetListClausier()
        {
            var lstClause = new Clausier
            {
                SearchScreen = true,
                MotCles1 = new List<AlbSelectListItem>(),
                MotCles2 = new List<AlbSelectListItem>(),
                MotCles3 = new List<AlbSelectListItem>(),
                Rubriques = new List<AlbSelectListItem>(),
                ModeleDDLSousRubrique = new ModeleDDLSousRubrique()
            };

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient = client.Channel;
                var result = screenClient.InitClausier();
                // Récuperer les mots clés et les rubriques/sousrubriques                
                lstClause.MotCles1 = result.MotsCles1.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                lstClause.MotCles2 = result.MotsCles2.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                lstClause.MotCles3 = result.MotsCles3.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                lstClause.Rubriques = result.Rubriques.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                lstClause.ModeleDDLSousRubrique = new ModeleDDLSousRubrique();
                lstClause.ModeleDDLSequence = new ModeleDDLSequence();

            }

            return PartialView("~/Views/Shared/Clausier.cshtml", lstClause);
        }

        [ErrorHandler]
        public string CheckPrimeSoldee(string codeAffaire, string version, string type, string codeAvn)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var serviceContext = client.Channel;
                return serviceContext.GetHasPrimeSoldee(codeAffaire, version, type, codeAvn) ? "1" : "0";
            }
        }

        [ErrorHandler]
        public PartialViewResult RechercheParCodeDossier(string codeDossier, string searchWithDetails)
        {

            var modeleParametresRecherche = new ModeleParametresRecherche { CritereParam = AlbConstantesMetiers.CriterParam.Standard };
            bool.TryParse(searchWithDetails, out bool withSearchDetails);
            if (withSearchDetails)
            {
                if (AlbSessionHelper.ParametresRecherches != null)
                {
                    modeleParametresRecherche = AlbSessionHelper.ParametresRecherches;
                    modeleParametresRecherche.SessionSearch = true;
                }
                modeleParametresRecherche.CodeOffre = codeDossier;
                AlbSessionHelper.ParametresRecherches = modeleParametresRecherche;
                modeleParametresRecherche.AccesRecherche = AlbConstantesMetiers.TypeAccesRecherche.Rapide;
                var modele = ConvertParamRechercheToPage(modeleParametresRecherche);
                modele = LoadListesRecherche(modele);
                return PartialView("BodyRechercheSaisie", modele);

            }
            else
            {
                modeleParametresRecherche = new ModeleParametresRecherche
                {
                    CodeOffre = codeDossier,
                    CabinetCourtageId = 0,
                    CabinetCourtageIsApporteur = true,
                    CabinetCourtageIsGestionnaire = true,
                    CritereParam = AlbConstantesMetiers.CriterParam.Standard,
                    TypeDateRecherche = AlbConstantesMetiers.TypeDateRecherche.Saisie,
                    EndLine = 0,
                    IsActif = true,
                    IsInactif = true,
                    ModeNavig = "S",
                    LineCount = 0,
                    PageNumber = 1,
                    StartLine = 0,
                    AccesRecherche = AlbConstantesMetiers.TypeAccesRecherche.Rapide
                };
                return Recherche(modeleParametresRecherche);
            }


        }

        [ErrorHandler]
        [HttpPost]
        public void RepriveAvenant(Folder folder)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffairePort>())
            {
                client.Channel.RepriseAvenant(folder.Adapt<AffaireId>());
            }
        }

        private static ModeleRecherchePage ConvertParamRechercheToPage(ModeleParametresRecherche parametresRecherche, ModeleRecherchePage model = null)
        {
            if (model == null)
            {
                model = new ModeleRecherchePage();
            }

            model.AdresseRisqueCP = parametresRecherche.AdresseRisqueCP;
            model.AdresseRisqueVille = parametresRecherche.AdresseRisqueVille;
            model.AdresseRisqueVoie = parametresRecherche.AdresseRisqueVoie;
            model.Branche = parametresRecherche.Branche;
            model.CabinetCourtageId = parametresRecherche.CabinetCourtageId > 0 ? parametresRecherche.CabinetCourtageId.ToString() : string.Empty;
            model.CabinetCourtageNom = parametresRecherche.CabinetCourtageNom;
            model.CheckContrat = parametresRecherche.CheckContrat;
            model.CheckOffre = parametresRecherche.CheckOffre;
            model.CritereParam = parametresRecherche.CritereParam;
            model.DateDebut = parametresRecherche.DDateDebut;
            model.DateFin = parametresRecherche.DDateFin;
            model.DateType = parametresRecherche.TypeDateRecherche.ToString();
            model.Etat = parametresRecherche.Etat;
            model.GestionnaireCode = parametresRecherche.GestionnaireCode;
            model.GestionnaireNom = parametresRecherche.GestionnaireNom;
            model.MotsClefs = parametresRecherche.MotsClefs;
            model.OffreId = parametresRecherche.CodeOffre;
            model.PreneurAssuranceCP = parametresRecherche.PreneurAssuranceCP;
            model.PreneurAssuranceId = parametresRecherche.PreneurAssuranceCode > 0 ? parametresRecherche.PreneurAssuranceCode.ToString() : string.Empty;
            model.PreneurAssuranceNom = parametresRecherche.PreneurAssuranceNom;
            model.PreneurAssuranceSIREN = parametresRecherche.PreneurAssuranceSIREN > 0 ? parametresRecherche.PreneurAssuranceSIREN.ToString() : string.Empty;
            model.PreneurAssuranceVille = parametresRecherche.PreneurAssuranceVille;
            model.SearchInTemplate = parametresRecherche.IsTemplate;
            model.Situation = parametresRecherche.Situation;
            model.SouscripteurCode = parametresRecherche.SouscripteurCode;
            model.SouscripteurNom = parametresRecherche.SouscripteurNom;
            model.ModeleCibles = new ModeleListeCibles { Cible = parametresRecherche.Cible };
            model.IsCheckApporteur = parametresRecherche.CabinetCourtageIsApporteur;
            model.IsCheckContrat = parametresRecherche.CheckContrat;
            model.IsCheckEnCours = parametresRecherche.IsActif;
            model.IsCheckEtat = parametresRecherche.SaufEtat;
            model.IsCheckGestionnaire = parametresRecherche.CabinetCourtageIsGestionnaire;
            model.IsCheckOffre = parametresRecherche.CheckOffre;
            model.IsCheckInactif = parametresRecherche.IsInactif;
            model.AccesRecherche = parametresRecherche.AccesRecherche;
            model.SessionSearch = parametresRecherche.SessionSearch;

            return model;
        }

        private static bool GetBlocageSaisieAS400()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetBlocageSaisieAS400();
                return result.Etat != "O";
            }
        }

        private static ModeleRecherchePage LoadListesRecherche(ModeleRecherchePage page)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var serviceContext = client.Channel;
                var query = new RechercheSaisieGetQueryDto
                {
                    IsAdmin = CacheUserRights.IsUserAdmin,
                    IsUserHorse = MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0
                };
                var result = serviceContext.RechercheSaisieGet(query);

                if (result == null)
                {
                    return page;
                }

                var branches = result.Branches.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                page.Branches = branches;
                var cibles = result.Cibles.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (page.ModeleCibles == null)
                {
                    page.ModeleCibles = new ModeleListeCibles();
                }

                page.ModeleCibles.Cibles = cibles;
                var etats = result.Etats.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                page.Etats = etats;
                var situations = result.Situation.Select(m => new AlbSelectListItem()
                {
                    Value = m.Code,
                    Text = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty,
                    Selected = false,
                    Title = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty
                }).ToList();
                page.Situations = situations;
                var listRefus = result.ListRefus.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                page.ListRefus = listRefus;

                page.ListCabinetCourtage = new ModeleRechercheAvancee() { Contexte = "RecherchePrincipale" };
                page.ListPreneurAssurance = new ModeleRechercheAvancee() { Contexte = "RecherchePrincipale" };
                page.DateTypes = AlbTransverse.InitDateType;
            }
            return page;
        }

        public JsonResult GetIp()
        {
            var ipAdress = Request.UserHostAddress;
            string machineName = string.Empty;
            try
            {
                System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(ipAdress);

                machineName = hostEntry.HostName;
            }
            catch
            {
                // Machine not found...
            }

            return new JsonResult
            {
                Data = string.Format("{0} - {1}", ipAdress, machineName),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult OpenLTA(string codeAffaire, int version, string type, int avenant, string modeNavig, bool isReadOnly)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetInfoLTA(codeAffaire, version, type, avenant, AlbEnumInfoValue.GetEnumValue<ModeConsultation>(modeNavig));
                var model = ((LTA)result);
                model.ReadOnly = isReadOnly;

                return PartialView("ViewLTA", model);
            }
        }

        public void SaveInfoLTA(string codeAffaire, int version, string type, int avenant,
            bool isDateCheck, bool isDureeCheck,
            string dateDebLTA, string heureDebLTA, string dateFinLTA, string heureFinLTA,
            int dureeLTA, string dureeLTAString,
            float seuilLTA)
        {
            DateTime? dateFin = null;
            if (isDureeCheck)
            {
                var dateDeb = AlbConvert.ConvertStrToDate(dateDebLTA);
                var hourDeb = AlbConvert.ConvertIntToTimeMinute(AlbConvert.ConvertStrToIntHour(heureDebLTA));

                //long longDate = ((long)dateDeb) * 10000 + hourDeb;

                dateFin = AlbConvert.GetFinPeriode(new DateTime(dateDeb.Value.Year, dateDeb.Value.Month, dateDeb.Value.Day, hourDeb.Value.Hours, hourDeb.Value.Minutes, 0), dureeLTA, dureeLTAString);
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                var dto = new LTADto
                {
                    DateDeb = AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(dateDebLTA)),
                    HeureDeb = Convert.ToInt16(AlbConvert.ConvertStrToIntHour(heureDebLTA)),
                    DateFin = dateFin.HasValue ? AlbConvert.ConvertDateToInt(dateFin) : AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(dateFinLTA)),
                    HeureFin = dateFin.HasValue ? Convert.ToInt16(AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(dateFin))) : Convert.ToInt16(AlbConvert.ConvertStrToIntHour(heureFinLTA)),
                    DureeLTA = isDureeCheck ? dureeLTA : 0,
                    DureeLTAString = isDureeCheck ? dureeLTAString : string.Empty,
                    SeuilLTA = seuilLTA
                };
                serviceContext.SetInfoLTA(codeAffaire, version, type, avenant, dto);
            }
        }

        public string CreateJsonContract()
        {
            string filePath = ConfigurationManager.AppSettings["ContractJsonFullPath"];
            ContractJsonDto objectJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ContractJsonDto>(System.IO.File.ReadAllText(filePath));

            var gestionnaire =
                CabinetCourtageGet(new CabinetCourtageGetQueryDto(objectJson.Courtier.Gestionnaire.Code.ToInteger() ?? 0, false));
            var apporteur =
                CabinetCourtageGet(new CabinetCourtageGetQueryDto(objectJson.Courtier.Apporteur.Code.ToInteger() ?? 0, false));
            var payeur =
                CabinetCourtageGet(new CabinetCourtageGetQueryDto(objectJson.Courtier.Payeur.Code.ToInteger() ?? 0, false));
            var assure =
                AssureGet(new AssureGetQueryDto() { Code = objectJson.Assure.Code, NomAssure = "", DebutPagination = 1, FinPagination = 1, Order = "ASC", By = 1 });

            if (!gestionnaire.CabinetCourtages.Any(x => x.Code != 0) || !apporteur.CabinetCourtages.Any(x => x.Code != 0) || !payeur.CabinetCourtages.Any(x => x.Code != 0) || !assure.Assures.Any())
            {
                throw new AlbException(new Exception("Courtier ou Assure inexistant"));
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var user = GetUser();

                var serviceContext = client.Channel;

                switch (objectJson.Type)
                {
                    // Création d'un contrat
                    case "P":
                        objectJson = serviceContext.CreationContractsKheops(objectJson, user);
                        break;
                    // Création d'une offre
                    case "O":
                        objectJson = serviceContext.CreationOffersKheops(objectJson, user);
                        break;
                }

                return objectJson.CodeAffaire;
            }
        }


        public void CorrectionECM()
        {
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                client.Channel.CorrectionECM(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false);
            }
        }
    }
}
