using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRetours;
using OP.WSAS400.DTO.AffaireNouvelle;
using OPServiceContract.IAdministration;
using OPServiceContract.IAffaireNouvelle;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class RetoursController : ControllersBase<ModeleRetoursPage>
    {

        [ErrorHandler]
        public ActionResult Index(string id)
        {
            model.PageTitle = "Retour des pièces signées";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult ValiderRetours(string codeContrat, string version, string type, string codeAvt, string tabGuid,
            string dateRetourPreneur, string typeAccordPreneur, string listeLignesCoAss, string modeNavig)
        {

            var serialiserCoAssureur = AlbJsExtendConverter<RetourCoassureurDto>.GetSerializer();
            var coAssureurDto = serialiserCoAssureur.ConvertToType<List<RetourCoassureurDto>>(serialiserCoAssureur.DeserializeObject(listeLignesCoAss));

            DateTime dDate;
            coAssureurDto.ForEach(elm => elm.DateRetourCoAss = (DateTime.TryParse(elm.SDateRetourCoAss, out dDate) ? int.Parse(dDate.ToString("yyyyMMdd")) : 0));

            var preneurDto = new RetourPreneurDto
            {
                DateRetour = (DateTime.TryParse(dateRetourPreneur, out dDate) ? int.Parse(dDate.ToString("yyyyMMdd")) : 0),
                TypeAccord = typeAccordPreneur
            };

            var folder = string.Format("{0}_{1}_{2}_{3}", codeContrat, version, type, string.IsNullOrEmpty(codeAvt) ? "0" : codeAvt);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, folder);

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                client.Channel.EnregistrerRetours(codeContrat, version, type, codeAvt, preneurDto, coAssureurDto, modeNavig.ParseCode<ModeConsultation>(), GetUser(), isModifHorsAvn);
            }

            //Déverouillage de l'offre/contrat
            CommonVerouillage.DeverrouilleFolder(codeContrat, version, type, codeAvt, tabGuid, true, false, isModifHorsAvn);

            if (modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Standard)
                return RedirectToAction("Index", "RechercheSaisie", new { id = codeContrat + "_" + version + "_" + type + "_" + "loadParam" });
            return null;
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeContrat, string version, string type, string tabGuid, string paramRedirect)
        {
            FolderController.DeverrouillerAffaire(tabGuid);
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            return cible != "RechercheSaisie" ?
                RedirectToAction(job, cible, new { id = codeContrat + "_" + version + "_" + type + GetSurroundedTabGuid(tabGuid) })
                : RedirectToAction("Index", "RechercheSaisie", new { id = codeContrat + "_" + version + "_" + type + AlbOpConstants.LOCKUSER_SPLITSTR + "loadParam" + GetSurroundedTabGuid(tabGuid) });
        }

        [ErrorHandler]
        public ActionResult OpenRetours(string id, string modeNavig)
        {
            var idParsed = InitializeParams(id);
            model.ModeNavig = modeNavig;
            var tId = idParsed.Split('_');

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                model.Contrat = client.Channel.GetContrat(tId[0], tId[1], tId[2], tId[3], modeNavig.ParseCode<ModeConsultation>());
            }

            var result = LoadRetoursSignatures(tId[0], tId[1], tId[2], tId[3], model.ModeNavig);
            if (result != null) {
                model.TypeAccordPreneurActuel = result.TypeAccordPreneurActuel;
                model.TypeAccordPreneur = result.TypeAccordPreneur;
                model.DateRetourPreneur = result.DateRetourPreneur > 0 ? result.DateRetourPreneur : AlbConvert.ConvertDateToInt(DateTime.Now).Value;
                model.DateRetourPreneurActuel = result.DateRetourPreneurActuel;
                model.IsReglementRecu = result.IsReglementRecu;
                model.ListeRetoursCoAss = result.ListeRetoursCoAss;
            }

            return PartialView("RetoursBody", this.model);
        }

        [ErrorHandler]
        public void FermerRetours(string codeContrat, string version, string type, string codeAvt, string tabGuid, string modeNavig)
        {
            var folder = string.Format("{0}_{1}_{2}_{3}", codeContrat, version, type, string.IsNullOrEmpty(codeAvt) ? "0" : codeAvt);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, folder);
            //Deverouillage du contrat
            ALBINGIA.OP.OP_MVC.Common.CommonVerouillage.DeverrouilleFolder(codeContrat, version, type, codeAvt, tabGuid, false, false, isModifHorsAvn);
        }

        #region Méthodes Privées
        protected override void LoadInfoPage(string id)
        {
            var tId = id.Split('_');
            if (tId[2] == "P")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    model.Contrat = client.Channel.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                }
                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
            }
            if (model.Contrat != null)
            {
                model.AfficherBandeau = DisplayBandeau(true, id);
                model.AfficherNavigation = false;// model.AfficherBandeau;
                model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
            }
            #region Navigation Arbre
            SetArbreNavigation();
            #endregion
            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion
            //LoadEngagementPeriodes(tId[0], tId[1], tId[2]);
            LoadRetoursSignatures(tId[0], tId[1], tId[2]);

        }

        private ModeleRetoursPage LoadRetoursSignatures(string code, string version, string type, string codeAvt, string modeNavig)
        {
            ModeleRetoursPage model = new ModeleRetoursPage();

            model.TypeAccordPreneurActuel = GetListeAccord();
            //model.ModelFileDescriptions = new FileDescriptions { ListFileDescription = IOFileManager.GetAllFileDescription(AlbOpConstants.UploadParthReturnedDocument, HttpUtility.UrlDecode(string.Format("{0}-{1}", code, version)), MvcApplication.SPLIT_CONST_FILE) };
            model.TypeAccordPreneur = GetListeAccord();
            //SetSelectedItem(model.TypeAccordPreneur, "N");
            //model.DateRetourPreneur = ALBINGIA.Framework.Common.Tools.AlbConvert.ConvertDateToInt(DateTime.Now).HasValue ? ALBINGIA.Framework.Common.Tools.AlbConvert.ConvertDateToInt(DateTime.Now).Value : 0;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetRetourPreneur2(code, version, type, codeAvt, modeNavig.ParseCode<ModeConsultation>());
                SetSelectedItem(model.TypeAccordPreneurActuel, result.TypeAccord);
                model.DateRetourPreneurActuel = result.DateRetour;
                //SLA Spec V6
                SetSelectedItem(model.TypeAccordPreneur, result.TypeAccord);
                model.DateRetourPreneur = result.DateRetour;
                //Fin SLA Spec V6
                model.IsReglementRecu = (result.IsReglementRecu == "O" ? true : false);

                var resCoass = serviceContext.GetRetoursCoassureurs2(code, version, type, codeAvt, modeNavig.ParseCode<ModeConsultation>());
                if (resCoass == null) return null;
                model.ListeRetoursCoAss = new List<ModeleLigneRetourCoAss>();
                resCoass.ForEach(elm => model.ListeRetoursCoAss.Add((ModeleLigneRetourCoAss)elm));
                model.ListeRetoursCoAss.ForEach(elm => elm.TypeAccordCoAss = GetListeAccord());
                model.ListeRetoursCoAss.ForEach(elm => SetSelectedItem(elm.TypeAccordCoAss, elm.TypeAccordVal));
            }

            return model;
        }

        private void LoadRetoursSignatures(string code, string version, string type)
        {
            model.TypeAccordPreneurActuel = GetListeAccord();
            //model.ModelFileDescriptions = new FileDescriptions { ListFileDescription = IOFileManager.GetAllFileDescription(AlbOpConstants.UploadParthReturnedDocument, HttpUtility.UrlDecode(string.Format("{0}-{1}", code, version)), MvcApplication.SPLIT_CONST_FILE) };
            model.TypeAccordPreneur = GetListeAccord();
            SetSelectedItem(model.TypeAccordPreneur, "N");
            model.DateRetourPreneur = ALBINGIA.Framework.Common.Tools.AlbConvert.ConvertDateToInt(DateTime.Now).HasValue ? ALBINGIA.Framework.Common.Tools.AlbConvert.ConvertDateToInt(DateTime.Now).Value : 0;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetRetourPreneur2(code, version, type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                SetSelectedItem(model.TypeAccordPreneurActuel, result.TypeAccord);
                model.DateRetourPreneurActuel = result.DateRetour;
                model.IsReglementRecu = (result.IsReglementRecu == "O" ? true : false);
                //
                var resCoass = serviceContext.GetRetoursCoassureurs(code, version, type);
                if (resCoass == null) return;
                model.ListeRetoursCoAss = new List<ModeleLigneRetourCoAss>();
                resCoass.ForEach(elm => model.ListeRetoursCoAss.Add((ModeleLigneRetourCoAss)elm));
                model.ListeRetoursCoAss.ForEach(elm => elm.TypeAccordCoAss = GetListeAccord());
                model.ListeRetoursCoAss.ForEach(elm => SetSelectedItem(elm.TypeAccordCoAss, elm.TypeAccordVal));
            }
        }

        private void SetBandeauNavigation(string id)
        {
            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Contrat != null)
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
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }
        private void SetArbreNavigation()
        {
            if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle(string.Empty, returnEmptyTree: true, isTransverseAllowed: true);
            }
        }

        private static void SetSelectedItem(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            selectedItem = lookUp.FirstOrDefault(elm => elm.Value == compareValue);
            if (selectedItem != null)
                selectedItem.Selected = true;
        }

        private List<AlbSelectListItem> GetListeAccord()
        {

            var listeType = new List<AlbSelectListItem>();

            //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var lstTyp = serviceContext.GetListeTypesAccord();
                lstTyp.ForEach(elm => listeType.Add(new AlbSelectListItem
                {
                    Value = elm.Code,
                    Text = elm.Code + " - " + elm.Libelle,
                    Title = elm.Code + " - " + elm.Libelle,
                    Selected = false
                }
                ));
            }
            return listeType;
        }

        private bool VerrouillerContrat(string code, string version, string type, string numAvenant, string tabGuid, string flow)
        {
            string lockUser = string.Empty;
            bool isInCache = false;
            int iVersion = 0;
            int iAvenant = 0;
            if (!int.TryParse(version, out iVersion))
                return false;
            if (string.IsNullOrEmpty(numAvenant))
                numAvenant = "0";
            if (!int.TryParse(numAvenant, out iAvenant))
                return false;
            //Vérif si contrat en cache
            if (!OfferAccesAuthorization.OfferInCache(GetUser(), code, version, type + "_" + numAvenant, MvcApplication.SPLIT_CONST_HTML, out lockUser, out isInCache))
            {
                //Si non, vérif si contrat dans table verrou

                if (!AlbTransverse.IsOffreVerrrouille(code, version, type, numAvenant))
                {
                    //Si non, verrouillage
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var voletsBlocsCategoriesClient = client.Channel;
                        voletsBlocsCategoriesClient.AjouterOffreVerouille("PRODU", type,
                                                                          code.Trim(),
                                                                          iVersion,
                                                                          iAvenant, 0, 0, "", "Contrat", "GESTION",
                                                                          "O",
                                                                          GetUser(),
                                                                          string.Format(
                                                                            "Verrouillage de l'affaire à {0}",
                                                                            DateTime.Now.ToString(
                                                                              CultureInfo.CurrentCulture)));


                        OfferAccesAuthorization.VerouillerOffre(GetUser(), code, version, type + "_" + numAvenant, MvcApplication.SPLIT_CONST_HTML);

                        AlbUserRoles.SetUserProfil(tabGuid, GetUser(), code, version, type, lockUser, false, false, false, numAvn: numAvenant, currentFlow: flow);

                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
