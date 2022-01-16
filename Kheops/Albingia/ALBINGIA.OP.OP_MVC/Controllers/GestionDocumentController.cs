using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModeleGestionDocument;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    [Obsolete]
    public class GestionDocumentController : ControllersBase<ModeleGestionDocumentPage>
    {
        #region Méthodes Publiques
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            LoadInfoPage(id);

            return View(model);
        }

        [ErrorHandler]
        public ActionResult OpenDocument(string codeOffre, string version, string type, string codeAvn, string codeDocument, string modeNavig)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreServices=client.Channel;
                //var result = finOffreServices.InitGestionDocumentCreation(codeOffre, version, type, codeAvn, codeDocument, modeNavig.ParseCode<ModeConsultation>());

                ModeleGestionDocumentCreation modele = new ModeleGestionDocumentCreation();
                //if (result != null)
                //{
                //    modele = ((ModeleGestionDocumentCreation)result);
                //    var typesDocument = result.TypesDocument.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                //    modele.TypesDocument = typesDocument;
                //    //var typesPartenaire = result.TypesPartenaire.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                //    //modele.TypesPartenaire = typesPartenaire;
                //    var typesCourrier = result.TypesCourrier.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                //    modele.TypesCourrier = typesCourrier;
                //    var typesCourrierPart = result.TypesCourrierPart.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                //    modele.TypesCourrierPart = typesCourrierPart;
                //    var destinataires = new List<AlbSelectListItem>();
                //    AlbSelectListItem destinataire = null;
                //    destinataire = new AlbSelectListItem { Value = "To", Text = "To", Selected = false, Title = "To" };
                //    destinataires.Add(destinataire);
                //    destinataire = new AlbSelectListItem { Value = "Cc", Text = "Cc", Selected = false, Title = "Cc" };
                //    destinataires.Add(destinataire);
                //    destinataire = new AlbSelectListItem { Value = "CCi", Text = "Cci", Selected = false, Title = "Cci" };
                //    destinataires.Add(destinataire);
                //    modele.DestinatairesPart = destinataires;


                //    foreach (GestionDocumentCourrier courriers in modele.Courriers)
                //    {
                //        courriers.TypesPartenaire = new List<AlbSelectListItem>();
                //        result.TypesPartenaire.ForEach(m => courriers.TypesPartenaire.Add(new AlbSelectListItem()
                //        {
                //            Value = m.Code,
                //            Text = string.Format("{0} - {1}", m.Code, m.Libelle),
                //            Title = string.Format("{0} - {1}", m.Code, m.Libelle),
                //            Selected = (m.Code == courriers.TypePartenaire)
                //        }));
                //    }

                //    foreach (GestionDocumentCourrier emails in modele.Emails)
                //    {
                //        emails.TypesPartenaire = new List<AlbSelectListItem>();
                //        result.TypesPartenaire.ForEach(m => emails.TypesPartenaire.Add(new AlbSelectListItem()
                //        {
                //            Value = m.Code,
                //            Text = string.Format("{0} - {1}", m.Code, m.Libelle),
                //            Title = string.Format("{0} - {1}", m.Code, m.Libelle),
                //            Selected = (m.Code == emails.TypePartenaire)
                //        }));
                //    }

                //}

                return PartialView("GestionDocumentCreation", modele);
            }
        }

        ///// <summary>
        ///// Recherche du nom du partenaire à partir du code.
        ///// </summary>
        ///// <param name="codePartenaire">Code partenaire.</param>
        ///// <param name="typePartenaire">Type partenaire.</param>
        ///// <returns></returns>
        //[AjaxException]
        //public JsonResult GetPartenaireByCode(string codePartenaire, string typePartenaire)
        //{
        //    JsonResult toReturn = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    toReturn.Data = new CabinetCourtage_JSON_MetaData();

        //    int code;
        //    switch (typePartenaire)
        //    {
        //        case "ASS":
        //            if (int.TryParse(codePartenaire, out code))
        //            {
        //                toReturn.Data = GetPreneursAssuranceByCodeImplementation(code);
        //            }
        //            break;
        //        case "COURT":
        //            if (int.TryParse(codePartenaire, out code))
        //            {
        //                toReturn.Data = GetCabinetCourtageByCodeImplementation(code);
        //            }
        //            break;
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //public JsonResult GetPartenaireByName(string nomPartenaire, string typePartenaire)
        //{
        //    JsonResult toReturn = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        //    switch (typePartenaire)
        //    {
        //        case "ASS":
        //            toReturn.Data = new List<PreneurAssurance_JSON_MetaData>();
        //            if (!string.IsNullOrEmpty(nomPartenaire))
        //            {
        //                toReturn.Data = GetAssureByNameImplementation(nomPartenaire);
        //            }
        //            break;
        //        case "COURT":
        //            toReturn.Data = new List<CabinetCourtage_JSON_MetaData>();
        //            if (!string.IsNullOrEmpty(nomPartenaire))
        //            {
        //                toReturn.Data = GetCabinetsCourtagesByNameImplementation(nomPartenaire, 1, 10, "ASC", 1);
        //            }
        //            break;
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //[AjaxException]
        //public JsonResult GetInterlocuteurs(string nomInterlocuteur, string codePartenaire, string typePartenaire)
        //{
        //    JsonResult toReturn = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    List<CabinetCourtage_JSON_MetaData> cabinetCourtageJsonMetaDatas = new List<CabinetCourtage_JSON_MetaData>();
        //    InterlocuteurGetQueryDto query = new InterlocuteurGetQueryDto { Nom = nomInterlocuteur };
        //    int codePartenaireInt = int.MinValue;
        //    if (!String.IsNullOrWhiteSpace(codePartenaire))
        //    {
        //        int.TryParse(codePartenaire, out codePartenaireInt);
        //    }
        //    if (codePartenaireInt != int.MinValue)
        //    {
        //        query.CodeCabinetCourtage = codePartenaireInt;
        //    }

        //    InterlocuteurGetResultDto result = interlocuteurGet(query);
        //    foreach (InterlocuteurDto interlocuteurDto in result.Interlocuteurs)
        //    {
        //        CabinetCourtage_JSON_MetaData cabinetCourtageJsonMetaData =
        //            new CabinetCourtage_JSON_MetaData
        //            {
        //                Nom = interlocuteurDto.CabinetCourtage.NomCabinet,
        //                Code = interlocuteurDto.CabinetCourtage.Code,
        //                Type = interlocuteurDto.CabinetCourtage.Type,
        //                NomInterlocuteur = interlocuteurDto.Nom,
        //                IdInterlocuteur = interlocuteurDto.Id,
        //                ValideInterlocuteur = interlocuteurDto.EstValide,
        //                CodePostal = interlocuteurDto.CabinetCourtage.Adresse.Ville.CodePostal,
        //                Ville = interlocuteurDto.CabinetCourtage.Adresse.Ville.Nom,
        //                EstValide = interlocuteurDto.CabinetCourtage.EstValide
        //            };
        //        cabinetCourtageJsonMetaDatas.Add(cabinetCourtageJsonMetaData);
        //    }

        //    toReturn.Data = cabinetCourtageJsonMetaDatas;
        //    return toReturn;
        //}

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string paramRedirect, string tabGuid, string modeNavig)
        {
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] + tabGuid + GetFormatModeNavig(modeNavig) });
            }

            if (cible == "RechercheSaisie") {
                return RedirectToAction(job, cible);
            }
            return RedirectToAction(job, cible, new { id = AlbParameters.BuildStandardId(new Folder(new[] { codeOffre, version, type }), tabGuid, null, modeNavig) });
        }

        #endregion

        #region Méthodes Privées

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            switch (tId[2])
            {
                case "O":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                    {
                        var policeServicesClient=client.Channel;
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    }
                    break;
                case "P":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        var serviceContext=client.Channel;
                        model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                    }
                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
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
            model.PageTitle = "Gestion des documents";
            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), tId[0] + "_" + tId[1] + "_" + tId[2], model.NumAvenantPage);

            #region Navigation Arbre
            SetArbreNavigation();
            #endregion
            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion
            if (model.Offre != null)
                LoadInfoGestionDoc(model.Offre.CodeOffre, model.Offre.Version, model.Offre.Type, model.Offre.Branche.Code, model.ModeNavig);
            else if (model.Contrat != null)
                LoadInfoGestionDoc(model.Contrat.CodeContrat, int.Parse(model.Contrat.VersionContrat.ToString()), model.Contrat.Type, model.Contrat.Branche, model.ModeNavig);
        }

        private void LoadInfoGestionDoc(string codeOffre, int? version, string type, string branche, string modeNavig)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreServices=client.Channel;
                //var result = finOffreServices.InitGestionDocument(codeOffre, version, type, branche, AlbSessionHelper.ConnectedUser, modeNavig.ParseCode<ModeConsultation>());

                List<ModeleGestionDistribution> distributions = new List<ModeleGestionDistribution>();
                //if (result != null)
                //{
                //    distributions = ((ModeleGestionDocumentPage)result).Distributions;
                //}

                model.Distributions = distributions;
            }
        }

        #region Recherche Infos Courtier

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private CabinetCourtage_JSON_MetaData GetCabinetCourtageByCodeImplementation(int code)
        //{
        //    CabinetCourtage_JSON_MetaData toReturn = new CabinetCourtage_JSON_MetaData();
        //    CabinetCourtageGetResultDto cabinetCourtageGetResultDto = CabinetCourtageGet(new CabinetCourtageGetQueryDto { Code = code });
        //    if (cabinetCourtageGetResultDto.CabinetCourtages.Count > 0)
        //    {
        //        toReturn = DtoToJsonCourtier(cabinetCourtageGetResultDto.CabinetCourtages[0]);
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private CabinetCourtageGetResultDto CabinetCourtageGet(CabinetCourtageGetQueryDto query)
        //{
        //    CabinetCourtageGetResultDto toReturn = null;
        //    using (PoliceServicesClient screenClient = new PoliceServicesClient())
        //    {
        //        toReturn = screenClient.CabinetCourtageGet(query);
        //    }
        //    if (toReturn.Result == enIOAS400Results.failure)
        //    {
        //        throw new ArgumentException("Probleme de retour de webservice");
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private CabinetCourtage_JSON_MetaData DtoToJsonCourtier(CabinetCourtageDto cabinetCourtageDto)
        //{
        //    return new CabinetCourtage_JSON_MetaData { Code = cabinetCourtageDto.Code, Nom = cabinetCourtageDto.NomCabinet, Type = cabinetCourtageDto.Type, EstValide = cabinetCourtageDto.EstValide };
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private List<CabinetCourtage_JSON_MetaData> GetCabinetsCourtagesByNameImplementation(string name, int debutPagination, int finPagination, string Order, int By)
        //{
        //    List<CabinetCourtage_JSON_MetaData> toReturn = new List<CabinetCourtage_JSON_MetaData>();
        //    CabinetCourtageGetResultDto cabinetCourtageGetResultDto =
        //        CabinetCourtageGet(new CabinetCourtageGetQueryDto { NomCabinet = name, DebutPagination = debutPagination, FinPagination = finPagination, Order = Order, By = By });
        //    if (cabinetCourtageGetResultDto.CabinetCourtages.Count > 0)
        //    {
        //        foreach (CabinetCourtageDto cabinetCourtageDto in cabinetCourtageGetResultDto.CabinetCourtages)
        //        {
        //            toReturn.Add(DtoToJsonAvecAdresseEtNomsSecondaires(cabinetCourtageDto));
        //        }
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private CabinetCourtage_JSON_MetaData DtoToJsonAvecAdresseEtNomsSecondaires(CabinetCourtageDto cabinetCourtageDto)
        //{
        //    CabinetCourtage_JSON_MetaData toReturn = DtoToJsonCourtier(cabinetCourtageDto);
        //    toReturn.NomSecondaires = cabinetCourtageDto.NomSecondaires.ToArray();
        //    if (cabinetCourtageDto.Adresse != null && cabinetCourtageDto.Adresse.Ville != null)
        //    {
        //        toReturn.CodePostal = cabinetCourtageDto.Adresse.Ville.CodePostal;
        //        toReturn.Ville = cabinetCourtageDto.Adresse.Ville.Nom;
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private InterlocuteurGetResultDto interlocuteurGet(InterlocuteurGetQueryDto query)
        //{
        //    InterlocuteurGetResultDto toReturn = null;
        //    using (PoliceServicesClient screenClient = new PoliceServicesClient())
        //    {
        //        toReturn = screenClient.InterlocuteursGet(query);
        //    }
        //    return toReturn;
        //}

        #endregion

        #region Recherche Infos Assure

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //public PreneurAssurance_JSON_MetaData GetPreneursAssuranceByCodeImplementation(int code)
        //{
        //    PreneurAssurance_JSON_MetaData toReturn = new PreneurAssurance_JSON_MetaData();
        //    AssureGetResultDto assureGetResultDto = PreneurAssuranceGet(new AssureGetQueryDto { Code = code.ToString() });
        //    if (assureGetResultDto.Assures.Count > 0)
        //    {
        //        toReturn = DtoToJsonAssure(assureGetResultDto.Assures[0]);
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private AssureGetResultDto PreneurAssuranceGet(AssureGetQueryDto query)
        //{
        //    AssureGetResultDto toReturn = null;
        //    using (var screenClient = new PoliceServicesClient())
        //    {
        //        toReturn = screenClient.AssuresGet(query);
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private List<PreneurAssurance_JSON_MetaData> GetAssureByNameImplementation(string name)
        //{
        //    var toReturn = new List<PreneurAssurance_JSON_MetaData>();
        //    AssureGetResultDto preneurAssuranceGetResultDto =
        //        PreneurAssuranceGet(new AssureGetQueryDto { NomAssure = name, DebutPagination = 1, FinPagination = 10, Order = "ASC", By = 1 });
        //    if (preneurAssuranceGetResultDto.Assures.Count > 0)
        //    {
        //        foreach (var preneurAssuranceDto in preneurAssuranceGetResultDto.Assures)
        //        {
        //            toReturn.Add(DtoToJsonAssureAvecSiren(preneurAssuranceDto));
        //        }
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private PreneurAssurance_JSON_MetaData DtoToJsonAssure(AssureDto preneurAssuranceDto)
        //{
        //    var toReturn = new PreneurAssurance_JSON_MetaData { Code = preneurAssuranceDto.Code, Nom = preneurAssuranceDto.NomAssure, NomSecondaires = preneurAssuranceDto.NomSecondaires.ToArray() };
        //    if (preneurAssuranceDto.Adresse != null && preneurAssuranceDto.Adresse.Ville != null)
        //    {
        //        toReturn.Ville = preneurAssuranceDto.Adresse.Ville.Nom;
        //        if (!String.IsNullOrEmpty(preneurAssuranceDto.Adresse.Ville.CodePostal) && preneurAssuranceDto.Adresse.Ville.CodePostal.Length >= 2)
        //        {
        //            toReturn.Departement = preneurAssuranceDto.Adresse.Ville.CodePostal.Substring(0, 2);
        //        }
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private PreneurAssurance_JSON_MetaData DtoToJsonAssureAvecSiren(AssureDto preneurAssuranceDto)
        //{
        //    var toReturn = DtoToJsonAssure(preneurAssuranceDto);
        //    if (preneurAssuranceDto.Siren != 0)
        //    {
        //        toReturn.SIREN = preneurAssuranceDto.Siren.ToString(CultureInfo.CurrentCulture);
        //    }
        //    return toReturn;
        //}

        #endregion
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
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL: //AMO PAS SUR
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            model.Bandeau.StyleBandeau = model.ScreenType;
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
        #endregion
    }
}
