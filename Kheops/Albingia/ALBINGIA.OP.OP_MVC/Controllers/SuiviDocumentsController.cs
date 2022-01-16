using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesSuiviDocuments;
using OP.WSAS400.DTO.SuiviDocuments;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class SuiviDocumentsController : ControllersBase<ModeleSuiviDocumentsPage>
    {
        
        #region Méthodes publiques
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            if (id.ToLower().Contains("test.html"))
                return null;
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        public ActionResult SearchDoc(string displayType, string filtre, string modeNavig, bool readOnly)
        {
            SuiviDocFiltreDto filtreDto = null;
            JavaScriptSerializer serializerModelAvtModif = AlbJsExtendConverter<SuiviDocFiltreDto>.GetSerializer();
            filtreDto = serializerModelAvtModif.ConvertToType<SuiviDocFiltreDto>(serializerModelAvtModif.DeserializeObject(filtre));

            ModeleSuiviDocumentsPage model = new ModeleSuiviDocumentsPage
            {
                DisplayType = displayType,
                ListSuiviDocuments = GetListSuiviDocuments(filtreDto, modeNavig, readOnly)
            };

            return PartialView("SuiviDocumentListDoc", model);
        }

        [ErrorHandler]
        public ActionResult OpenViewDoc(string displayType, string numAffaire, string version, string type, string avenant, string userName, string situation, string modeNavig, bool readOnly, string warning)
        {
            SuiviDocFiltreDto filtre = new SuiviDocFiltreDto
            {
                CodeOffre = numAffaire,
                Version = version,
                Type = type,
                Avenant = !string.IsNullOrEmpty(avenant) ? Convert.ToInt32(avenant) : -1,
                User = warning == "W" ? GetUser() : userName,
                Situation = situation,
                DateDebSituation = !string.IsNullOrEmpty(userName) ? AlbConvert.ConvertDateToInt(DateTime.Now).Value : 0
            };

            var affichages = new List<AlbSelectListItem>();
            affichages.Add(new AlbSelectListItem { Value = "T", Text = "Tout", Descriptif = "Tout", Title = "Tout" });
            affichages.Add(new AlbSelectListItem { Value = "A", Text = "Actes de gestion", Descriptif = "Actes de gestion", Title = "Actes de gestion", Selected = true });



            ModeleSuiviDocumentsPage model = new ModeleSuiviDocumentsPage
            {
                DisplayType = displayType,
                DisplaySearch = true,
                Situations = AlbEnumInfoValue.GetListEnumInfo<AlbConstantesMetiers.EditionSituations>(),
                TypesDestinataire = null,
                ListSuiviDocuments = GetListSuiviDocuments(filtre, modeNavig, readOnly),
                Filtre = filtre,
                ModeAffichages = affichages

            };

            return PartialView("SuiviDocumentsBody", model);
        }

        [ErrorHandler]
        public ActionResult OpenViewDocBis(string displayType, string numAffaire, string version, string type, string avenant, string userName, string situation, string modeNavig, bool readOnly, string warning)
        {

            if (IsModuleGestDocOpen())
            {
                var machineName = AlbNetworkInfo.GetMachineName(Request);
                var ip = Request.UserHostAddress;
#if DEBUG
                ip = AlbNetworkInfo.GetIpMachine();
#endif
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var Context = client.Channel;
                    Context.SetTraceLog(string.Empty, string.Empty, string.Empty, 0, "COMPNM", GetUser(), DateTime.Now.ToString(CultureInfo.InvariantCulture), string.Concat(ip, "_", machineName));
                }

                var versionInt = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var Context = client.Channel;
                    Context.OuvrirGestionDocument(numAffaire, versionInt, type, machineName, string.Empty, ip);
                }
                return null;
            }
            else
            {
                return OpenViewDoc(displayType, numAffaire, version, type, avenant, userName, situation, modeNavig, readOnly, warning);
            }
        }
        
        [ErrorHandler]
        public string OpenGED(string codeAffaire, string version, string type, string userName)
        {
            var machineName = AlbNetworkInfo.GetMachineName(Request);
            var ip = Request.UserHostAddress;
#if DEBUG
            ip = AlbNetworkInfo.GetIpMachine();
#endif
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var context = client.Channel;
                context.OpenGED(codeAffaire, Int32.TryParse(version, out var vers) ? vers : 0, type, userName, ip, machineName);
            }
            return string.Empty;
        }

        [ErrorHandler]
        public bool GenerateDocuments(string numAffaire, string version, string type, string avenant, string lotId)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                return Context.GenerateDocuments(numAffaire, version, type, avenant, lotId);
            }
        }

        [ErrorHandler]
        public bool PrintDocuments(string lotId)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                return Context.PrintDocuments(lotId, GetUser());
            }
        }

        [ErrorHandler]
        public bool ReeditDocument(string idDoc)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                return Context.ReeditDocument(idDoc, GetUser());
            }
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult RedirectWarning(string codeOffre, string version, string type, bool warning, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {
            return RedirectToAction(
                "Index",
                "SuiviDocuments",
                new {
                    id = AlbParameters.BuildFullId(new Folder(new[] { codeOffre, version, type }), warning ? new[] { "W" } : null, tabGuid, addParamValue, modeNavig)
                });
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string param)
        {
            if (!string.IsNullOrEmpty(param))
                return RedirectToAction(job, cible, new { id = param });
            return RedirectToAction(job, cible);
        }

        [ErrorHandler]
        public ActionResult OpenPJ(string docId)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                var result = Context.OpenPJ(docId);
                return PartialView("SuiviDocumentListPJ", result);
            }
        }

        [ErrorHandler]
        public ActionResult OpenSuiviDoc(string id)
        {
            ModeleSuiviDocumentsPage model = new ModeleSuiviDocumentsPage();
            id = InitializeParams(id);
            var tId = id.Split('_');
            SuiviDocFiltreDto filtre = new SuiviDocFiltreDto
            {

                CodeOffre = tId[0],
                Version = tId[1],
                Type = tId[2],
                Avenant = -1,
                User = tId.Length > 3 && tId[3] == "W" ? GetUser() : string.Empty,
                Warning = tId.Length > 3 ? tId[3] : string.Empty
            };
            model.DisplaySearch = false;
            model.Filtre = filtre;

            model.ListSuiviDocuments = GetListSuiviDocuments(filtre, model.ModeNavig, true);

            return PartialView("SuiviDocumentsBody", model);
        }

        [ErrorHandler]
        public ActionResult EditDocumentsECM(string lstDocIdLogo, string lstDocIdNoLogo)
        {
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            if (!string.IsNullOrEmpty(lstDocIdLogo))
                lstDocIdLogo = lstDocIdLogo.Replace(MvcApplication.SPLIT_CONST_HTML, "|").Substring(0, lstDocIdLogo.Replace(MvcApplication.SPLIT_CONST_HTML, "|").Length - 1);
            if (!string.IsNullOrEmpty(lstDocIdNoLogo))
                lstDocIdNoLogo = lstDocIdNoLogo.Replace(MvcApplication.SPLIT_CONST_HTML, "|").Substring(0, lstDocIdNoLogo.Replace(MvcApplication.SPLIT_CONST_HTML, "|").Length - 1);

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                var lstDocsDownload = Context.EditerDocuments(lstDocIdLogo, lstDocIdNoLogo);
                if (lstDocsDownload.Any())
                {

                }
            }
            return null;
        }
        [ErrorHandler]
        public string EditDocuments(string listeDocIdLogo, string listeDocIdNOLogo)
        {
            List<string> listeDocumentsATelecharger = null;
            if (!string.IsNullOrEmpty(listeDocIdLogo))
                listeDocIdLogo = listeDocIdLogo.Replace(OP_MVC.MvcApplication.SPLIT_CONST_HTML, "|").Substring(0, listeDocIdLogo.Replace(OP_MVC.MvcApplication.SPLIT_CONST_HTML, "|").Length - 1);
            if (!string.IsNullOrEmpty(listeDocIdNOLogo))
                listeDocIdNOLogo = listeDocIdNOLogo.Replace(OP_MVC.MvcApplication.SPLIT_CONST_HTML, "|").Substring(0, listeDocIdNOLogo.Replace(OP_MVC.MvcApplication.SPLIT_CONST_HTML, "|").Length - 1);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                listeDocumentsATelecharger = Context.EditerDocuments(listeDocIdLogo, listeDocIdNOLogo);
                if (listeDocumentsATelecharger.Any())
                {
                    var fileRet = new StringBuilder();
                    listeDocumentsATelecharger.ForEach(file => fileRet.Append(file + "||"));
                    int length = fileRet.ToString().Length;
                    return fileRet.ToString().Substring(0, length - 2);
                }


            }
            return null;
        }
        [ErrorHandler]
        public string RefreshDocuments(string docId)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                return Context.RefreshDocuments(docId);
            }
        }

        [ErrorHandler]
        public void SaveBloc(string docFullPath)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                Context.SaveBloc(docFullPath);
            }
        }

        #endregion

        #region Méthodes privées

        protected override void LoadInfoPage(string id)
        {
            model.DisplayType = AlbConstantesMetiers.DISPLAY_AFFAIRE;

            model.PageTitle = "Suivi de documents";
            model.AfficherBandeau = base.DisplayBandeau(false, id);
            model.AfficherNavigation = model.AfficherBandeau;
            #region Navigation Arbre
            SetArbreNavigation();
            #endregion
            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

            string[] tId = id.Split('_');
            SuiviDocFiltreDto filtre = new SuiviDocFiltreDto
            {
                Avenant = -1,
                CodeOffre = tId[0],
                Version = tId[1],
                Type = tId[2],
                User = tId.Length > 3 && tId[3] == "W" ? GetUser() : string.Empty,
                Warning = tId.Length > 3 ? tId[3] : string.Empty
            };

            model.DisplaySearch = true;
            model.Filtre = filtre;
            model.DisplayType = AlbConstantesMetiers.DISPLAY_GENERAL;
            model.Situations = AlbEnumInfoValue.GetListEnumInfo<AlbConstantesMetiers.EditionSituations>();
            model.TypesDestinataire = null;
            model.ListSuiviDocuments = GetListSuiviDocuments(filtre, model.ModeNavig, model.IsReadOnly);
            var affichages = new List<AlbSelectListItem>();
            affichages.Add(new AlbSelectListItem { Value = "T", Text = "Tout", Descriptif = "Tout", Title = "Tout" });
            affichages.Add(new AlbSelectListItem { Value = "A", Text = "Actes de gestion", Descriptif = "Actes de gestion", Title = "Actes de gestion" });
            model.ModeAffichages = affichages;

        }

        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Fin");
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Fin");
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
                        Etape = Navigation_MetaModel.ECRAN_INFOFIN,
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
                        Etape = Navigation_MetaModel.ECRAN_INFOFIN,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }

        private SuiviDocumentsListe GetListSuiviDocuments(SuiviDocFiltreDto filtre, string modeNavig, bool readOnly)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                filtre.PageNumber = filtre.PageNumber == 0 ? 1 : filtre.PageNumber;
                filtre.StartLine = ((filtre.PageNumber - 1) * PageSizeDoc) + 1;
                filtre.EndLine = filtre.PageNumber * PageSizeDoc;

                var result = Context.GetListSuiviDocuments(filtre, modeNavig.ParseCode<ModeConsultation>(), readOnly);

                if (result != null)
                {
                    var docTab = (SuiviDocumentsListe)result;
                   
                    return (SuiviDocumentsListe)docTab;
                }
            }
            return new SuiviDocumentsListe { SuiviDocumentsListeLot = new List<SuiviDocumentsLot>(), SuiviDocumentsPlat = new List<SuiviDocumentsPlat>() };
        }

        //private string GetWordBlocDocumentPath(string docId)
        //{
        //    var resFile = string.Empty;
        //    using (var serviceContext = new FinOffreClient())
        //    {
        //        resFile = serviceContext.UpdDocCPCS(docId).Trim();
        //        if (string.IsNullOrEmpty(resFile))
        //            throw new AlbFoncException("Impossible d'afficher le document", true, true);
        //        if (resFile.ToLower().Contains("erreur"))
        //            throw new AlbFoncException(resFile, true, true);

        //        var elmFilePath = resFile.ToLower().Split(new[] { AlbOpConstants.ClientWorkEnvironment.ToLower() }, StringSplitOptions.None);
        //        string toReturn = elmFilePath.Any() && elmFilePath.Length > 1 ? elmFilePath[1].Replace("\\", "/") : string.Empty;
        //        return toReturn + "-__-DMP";
        //    }

        //}

        #endregion

    }
}
