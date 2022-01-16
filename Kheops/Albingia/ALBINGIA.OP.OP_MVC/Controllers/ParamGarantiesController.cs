using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamGaranties;
using EmitMapper;
using OP.WSAS400.DTO.ParametreGaranties;
using OPServiceContract.IAdministration;
using OPServiceContract.IClausesRisquesGaranties;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamGarantiesController : ControllersBase<ModeleParamGarantiesPage>
    {
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrages des garanties";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            model.AdditionalParam = "**";
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult RechercheGaranties(string codeGarantie, string designationGarantie, string userRights)
        {
            var toReturn = new List<LigneGarantie>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=channelClient.Channel;
                var result = serviceContext.GetGaranties(codeGarantie, designationGarantie, userRights, false);
                if (result.Any())
                    result.ForEach(m => toReturn.Add((LigneGarantie)m));
                toReturn.ForEach(elm => elm.AdditionalParam = userRights);
            }
            //Paliatif pour résoudre le problème de décallage des colonnes
            return PartialView("ListeGaranties", toReturn);
        }
        [ErrorHandler]
        public ActionResult GetDetailsGarantie(string codeGarantie, string userRights)
        {
            var toReturn = new LigneGarantie();

            if (!string.IsNullOrEmpty(codeGarantie))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=channelClient.Channel;
                    var result = serviceContext.GetGarantie(codeGarantie.Replace(" ", "").Replace("'","''"), userRights);
                    toReturn = (LigneGarantie)result;
                    toReturn.AdditionalParam = userRights;
                    toReturn.ModeOperation = "U";
                }
            }
            else
            {
                toReturn.AdditionalParam = userRights;
                toReturn.ModeOperation = "I";
                toReturn.GarTypeReguls = new List<ModeleGarTypeRegul>();
            }
            //Chargement des DDL
            SetLists(toReturn);

            return PartialView("DetailsGarantie", toReturn);
        }

        [ErrorHandler]
        public ActionResult AddTypeRegul(string codeGarantie, string codeTypeRegul)
        {

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext = channelClient.Channel;
                var exists = serviceContext.IsTypeRegulAssociated(codeGarantie, codeTypeRegul);
                if (exists)
                {
                    throw new AlbFoncException("Le type de régularisation est déjà associé à cette garantie", trace: false, sendMail: false, onlyMessage: true);
                }
                var dtos = serviceContext.AddTypeRegul(codeGarantie,codeTypeRegul);
                var model = ObjectMapperManager.DefaultInstance.GetMapper<List<GarTypeRegulDto>, List<ModeleGarTypeRegul>>().Map(dtos);
                return PartialView("ListeTypeRegul", model);
            }
        }


        [ErrorHandler]
        public ActionResult DeleteTypeRegul(string codeGarantie, string codeTypeRegul)
        {

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext = channelClient.Channel;
                var dtos = serviceContext.DeleteTypeRegul(codeGarantie, codeTypeRegul);
                var model = ObjectMapperManager.DefaultInstance.GetMapper< List <GarTypeRegulDto> , List<ModeleGarTypeRegul>>().Map(dtos);
                return PartialView("ListeTypeRegul", model);
            }
        }

        [ErrorHandler]
        public ActionResult EnregistrerGarantie(LigneGarantie modelSave, string ancienCodeTypeInventaire) {
            if (!string.IsNullOrEmpty(modelSave.CodeGarantie) && !string.IsNullOrEmpty(modelSave.DesignationGarantie)) {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>()) {
                    //Vérifier s'il y a des inventaires liés à  cette garantie
                    if (!string.IsNullOrEmpty(ancienCodeTypeInventaire) && (ancienCodeTypeInventaire != modelSave.CodeTypeInventaire || !modelSave.IsLieInventaire))
                    {
                        var offresInvenGarDto = client.Channel.GetListOffreInventaireByGarantie(modelSave.CodeGarantie);
                        var offresInvenGarModel = new List<OffreInventaireGarantie>();
                        if (offresInvenGarDto.Any()) {
                            offresInvenGarDto.ForEach(o => offresInvenGarModel.Add((OffreInventaireGarantie)o));
                            return PartialView("ConfirmModifTypeInventaire", offresInvenGarModel);
                        }
                    }
                    return ModificationGarantie(modelSave, client.Channel);
                }
            }
            throw new AlbFoncException("Code et désignation ne peuvent être vides", trace: false, sendMail: false, onlyMessage: true);
        }

        [ErrorHandler]
        public ActionResult ConfirmModificationGarantie(LigneGarantie modelSave, string ancienCodeTypeInventaire, string listCodesInventaires)
        {
            if (!string.IsNullOrEmpty(modelSave.CodeGarantie) && !string.IsNullOrEmpty(modelSave.DesignationGarantie))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceContext=channelClient.Channel;
                    listCodesInventaires += ";**fin";
                    var toReturn = serviceContext.SupprimerListInventairesByCodeInventaire(listCodesInventaires);
                    if (!string.IsNullOrEmpty(toReturn))
                        throw new AlbFoncException(toReturn);
                }
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=channelClient.Channel;
                    return ModificationGarantie(modelSave, serviceContext);
                }
            }
            throw new AlbFoncException("Code et désignation ne peuvent être vides", trace: false, sendMail: false, onlyMessage: true);
        }
        [ErrorHandler]
        public ActionResult SupprimerGarantie(string codeGarantie, string userRights)
        {
            if (!string.IsNullOrEmpty(codeGarantie))
            {
                var toReturn = new List<LigneGarantie>();
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=channelClient.Channel;
                    string error = serviceContext.SupprimerGarantie(codeGarantie.Replace(" ", "").Replace("'", "''"), userRights);
                    if (!string.IsNullOrEmpty(error))
                        throw new AlbFoncException(error, trace: true, sendMail: true, onlyMessage: true);

                    //Rechargement des garanties
                    var result = serviceContext.GetGaranties(string.Empty, string.Empty, userRights, false);
                    if (result.Any())
                        result.ForEach(m => toReturn.Add((LigneGarantie)m));
                    toReturn.ForEach(elm => elm.AdditionalParam = userRights);
                }
                return PartialView("ListeGaranties", toReturn);
            }
            throw new AlbFoncException("Impossible de supprimer une garantie sans code", trace: true, sendMail: true, onlyMessage: true);
        }
        [ErrorHandler]
        public ActionResult EditTypesValeur(string codeGarantie, string userRights)
        {
            if (!string.IsNullOrEmpty(codeGarantie))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=channelClient.Channel;
                    //Rechargement des types de valeur de la garantie
                    var toReturn = GetModelTypesValeur(codeGarantie, userRights, serviceContext);
                    return PartialView("TypesValeur", toReturn);
                }

            }
            throw new AlbFoncException("Enregistrer d'abord la garantie avant de poursuivre", trace: true, sendMail: true, onlyMessage: true);
        }
        [ErrorHandler]
        public ActionResult EditTypeValeur(string id, string codeGarantie, string userRights)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=channelClient.Channel;
                var editTypeValeur = new EditTypeValeur();
                if (!string.IsNullOrEmpty(id))
                {
                    var ligneTypeValeur = new LigneTypeValeur();
                    var result = serviceContext.GetGarTypeValeurById(int.Parse(id), codeGarantie, userRights);
                    if (result != null)
                    {
                        ligneTypeValeur = (LigneTypeValeur)result;
                        editTypeValeur.EditedId = ligneTypeValeur.Id;
                        editTypeValeur.EditedNumOrdre = ligneTypeValeur.NumOrdre;
                        editTypeValeur.DDLTypesValeur = LoadDDLTypesValeur(codeGarantie, id.ToString());
                        editTypeValeur.DDLTypesValeur.SelectedTypeValeur = ligneTypeValeur.CodeTypeValeur;
                    };
                }
                else
                {
                    float numOrdre = 0;
                    var listeTypesValeur = GetGarTypesValeur(codeGarantie, userRights, serviceContext);
                    if (listeTypesValeur.Any())
                        numOrdre = listeTypesValeur.Max(elm => elm.NumOrdre) + 1;
                    editTypeValeur.EditedNumOrdre = numOrdre;
                    editTypeValeur.DDLTypesValeur = LoadDDLTypesValeur(codeGarantie, string.Empty);
                }
                return PartialView("EditTypeValeur", editTypeValeur);
            }
        }

        [ErrorHandler]
        public ActionResult EnregistrerTypeValeur(string id, float numOrdre, string codeTypeValeur, string codeGarantie, string userRights)
        {
            int idTypeVal = !string.IsNullOrEmpty(id) ? int.Parse(id) : 0;
            string modeOperation = id != "0" ? "U" : "I";
            if (!string.IsNullOrEmpty(codeGarantie))
            {
                //Enregistrement BDD
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=channelClient.Channel;
                    var typeValeur = new TypeValeurDto
                    {
                        Id = idTypeVal,
                        CodeGarantie = codeGarantie,
                        CodeTypeValeur = codeTypeValeur,
                        NumOrdre = numOrdre
                    };
                    var retourMsg = serviceContext.EnregistrerGarTypeValeur(typeValeur, modeOperation, userRights);
                    if (!string.IsNullOrEmpty(retourMsg)) throw new AlbFoncException(retourMsg);

                    //Rechargement des type de valeur de la garantie
                    var listeTypesValeur = GetGarTypesValeur(codeGarantie, userRights, serviceContext);
                    return PartialView("ListTypesValeur", listeTypesValeur);
                }
            }
            throw new AlbFoncException("Enregistrer d'abord la garantie avant de poursuivre", trace: false, sendMail: false, onlyMessage: true);
        }
        [ErrorHandler]
        public ActionResult SupprimerTypeValeur(string codeGarantie, string id, string userRights)
        {
            int idTypeVal = !string.IsNullOrEmpty(id) ? int.Parse(id) : 0;
            string modeOperation = id != "0" ? "U" : "I";
            if (!string.IsNullOrEmpty(codeGarantie))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=channelClient.Channel;
                    string error = serviceContext.SupprimerGarTypeValeur(codeGarantie, idTypeVal, userRights);
                    if (!string.IsNullOrEmpty(error))
                        throw new AlbFoncException(error, trace: true, sendMail: true, onlyMessage: true);

                    //Rechargement des types de valeur
                    var listeTypesValeur = GetGarTypesValeur(codeGarantie, userRights, serviceContext);
                    return PartialView("ListTypesValeur", listeTypesValeur);
                }
            }
            throw new AlbFoncException("Impossible de supprimer le type de valeur", trace: true, sendMail: true, onlyMessage: true);
        }
        [ErrorHandler]
        public ActionResult EditFamillesReassurance(string codeGarantie, string userRights)
        {
            if (!string.IsNullOrEmpty(codeGarantie))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=channelClient.Channel;
                    //Rechargement des familles de réassurance de la garantie
                    var toReturn = GetModelFamillesReassurance(codeGarantie, userRights, serviceContext);
                    return PartialView("FamillesReassurance", toReturn);
                }
            }
            throw new AlbFoncException("Enregistrer d'abord la garantie avant de poursuivre", trace: true, sendMail: true, onlyMessage: true);
        }
        [ErrorHandler]
        public ActionResult EditFamilleReassurance(string codeBranche, string codeSousBranche, string codeCategorie, string codeGarantie, string codeFamille, string userRights)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=channelClient.Channel;
                var editFamilleReassurance = new EditFamilleReassurance();

                editFamilleReassurance.DDLBranches = LoadDDLBranches();
                editFamilleReassurance.DDLFamilles = LoadDDLFamilles();

                if (!string.IsNullOrEmpty(codeBranche))
                { 
                    editFamilleReassurance.DDLBranches.SelectedBranche = codeBranche;
                    editFamilleReassurance.DDLFamilles.SelectedFamille = codeFamille;

                    editFamilleReassurance.DDLSousBranche = LoadDDLSousBranches(codeBranche);
                    editFamilleReassurance.DDLSousBranche.SelectedSousBranche = codeSousBranche;

                    editFamilleReassurance.DDLCategories = LoadDDLCategories(codeBranche, codeSousBranche);
                    editFamilleReassurance.DDLCategories.SelectedCategorie = codeCategorie;

                    editFamilleReassurance.CodeBrancheEdited = codeBranche;
                    editFamilleReassurance.CodeSousBrancheEdited = codeSousBranche;
                    editFamilleReassurance.CodeCategorieEdited = codeCategorie;
                    editFamilleReassurance.CodeFamilleEdited = codeFamille;

                }
                else
                {

                    editFamilleReassurance.DDLSousBranche = new DDLSousBranches();
                    editFamilleReassurance.DDLSousBranche.SousBranches = new List<AlbSelectListItem>();

                    editFamilleReassurance.DDLCategories = new DDLCategories();
                    editFamilleReassurance.DDLCategories.Categories = new List<AlbSelectListItem>();
                }
                return PartialView("EditFamilleReassurance", editFamilleReassurance);
            }
        }
        [ErrorHandler]
        public ActionResult SupprimerFamilleReassurance(string codeGarantie, string codeBranche, string codeSousBranche, string codeCategorie, string codeFamille, string userRights)
        {
            if (!string.IsNullOrEmpty(codeGarantie))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=channelClient.Channel;
                    string error = serviceContext.SupprimerFamilleReassurance(codeGarantie, codeBranche, codeSousBranche, codeCategorie, codeFamille, userRights);
                    if (!string.IsNullOrEmpty(error))
                        throw new AlbFoncException(error, trace: true, sendMail: true, onlyMessage: true);

                    //Rechargement des familles de réassurances
                    var toReturn = GetModelFamillesReassurance(codeGarantie, userRights, serviceContext);
                    return PartialView("ListFamillesReassurance", toReturn.ListeLigneFamilleReassurance);
                }
            }
            throw new AlbFoncException("Impossible de supprimer la famille de reassurance", trace: true, sendMail: true, onlyMessage: true);
        }
        [ErrorHandler]
        public ActionResult GetSousBranches(string codeBranche)
        {
            var toReturn = LoadDDLSousBranches(codeBranche);
            return PartialView("DDLSousBranche", toReturn);
        }
        [ErrorHandler]
        public ActionResult GetCategories(string codeBranche, string codeSousBranche)
        {
            var toReturn = LoadDDLCategories(codeBranche, codeSousBranche);
            return PartialView("DDLCategories", toReturn);
        }
        [ErrorHandler]
        public string GetFamille(string codeCategorie)
        {
            string famille = string.Empty;
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=channelClient.Channel;
                famille = serviceContext.GetGarFamille(codeCategorie);
            }
            return famille;
        }

        [ErrorHandler]
        public ActionResult EnregistrerFamilleReassurance(string id, string codeGarantie,
            string codeBranche, string codeSousBranche, string codeCategorie, string codeFamille,
            string codeBrancheAncien, string codeSousBrancheAncien, string codeCategorieAncien, string codeFamilleAncien,
            string userRights)
        {

            string modeOperation = !string.IsNullOrEmpty(id) ? "U" : "I";
            if (!string.IsNullOrEmpty(codeGarantie))
            {
                //Enregistrement BDD
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=channelClient.Channel;
                    var familleReassurance = new FamilleReassuranceDto
                    {
                        CodeGarantie = codeGarantie,
                        CodeBranche = codeBranche,
                        CodeSousBranche = codeSousBranche,
                        CodeCategorie = codeCategorie,
                        CodeFamille = codeFamille == null ? string.Empty : codeFamille
                    };
                    var familleReassuranceAncienne = new FamilleReassuranceDto
                    {
                        CodeBranche = codeBrancheAncien,
                        CodeSousBranche = codeSousBrancheAncien,
                        CodeCategorie = codeCategorieAncien,
                        CodeFamille = codeFamilleAncien == null ? string.Empty : codeFamilleAncien
                    };
                    var retourMsg = serviceContext.EnregistrerFamilleReassurance(familleReassurance, familleReassuranceAncienne, modeOperation, userRights);
                    if (!string.IsNullOrEmpty(retourMsg)) throw new AlbFoncException(retourMsg);

                    //Rechargement des familles de réassurance de la garantie                  
                    var listeFamillesReassurances = GetGarFamillesReassurance(codeGarantie, userRights, serviceContext);
                    return PartialView("ListFamillesReassurance", listeFamillesReassurances);
                }
            }
            throw new AlbFoncException("Enregistrer d'abord la garantie avant de poursuivre", trace: false, sendMail: false, onlyMessage: true);
        }
        
        #region méthodes privées
        private void SetLists(LigneGarantie ligneGarantie)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=channelClient.Channel;
                var result = serviceContext.GetParamGarantieListes();
                ligneGarantie.Taxes = result.Taxes.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                ligneGarantie.CatNats = result.CatNats.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                ligneGarantie.TypesDefinition = result.TypesDefinition.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                ligneGarantie.TypesInformation = result.TypesInformation.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();

                ligneGarantie.TypesGrille = result.TypesGrille.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                ligneGarantie.TypeReguls = result.TypesRegul.Select(m=> new AlbSelectListItem() { Value = m.Code.ToString() ,Text = string.Format("{0} - {1}" ,m.Code ,m.Libelle) , Selected = false ,Title = string.Format("{0} - {1}" ,m.Code ,m.Libelle) }).ToList();
                if (result.TypesInventaire != null)
                    ligneGarantie.TypesInventaire = result.TypesInventaire.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                else ligneGarantie.TypesInventaire = new List<AlbSelectListItem>();
            }
        }
        public DDLTypesValeur LoadDDLTypesValeur(string codeGarantie, string id)
        {
            var ddlTypesValeur = new DDLTypesValeur();
            var typesValeur = new List<AlbSelectListItem>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=channelClient.Channel;
                var result = serviceContext.LoadGarListTypesValeur(codeGarantie, id);
                typesValeur = result.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
            }
            ddlTypesValeur.TypesValeur = typesValeur;
            return ddlTypesValeur;
        }
        private DetailsTypesValeur GetModelTypesValeur(string codeGarantie, string userRights, IVoletsBlocsCategories serviceContext)
        {
            var toReturn = new DetailsTypesValeur
            {
                ListeLigneTypesValeur = GetGarTypesValeur(codeGarantie, userRights, serviceContext)
            };
            return toReturn;
        }
        private static List<LigneTypeValeur> GetGarTypesValeur(string codeGarantie, string userRights, IVoletsBlocsCategories serviceContext)
        {
            var listeTypesValeur = new List<LigneTypeValeur>();
            var result = serviceContext.GetGarTypesValeur(codeGarantie, userRights);
            if (result.Any())
                result.ForEach(m => listeTypesValeur.Add((LigneTypeValeur)m));
            listeTypesValeur.ForEach(elm => elm.AdditionalParam = userRights);
            return listeTypesValeur;
        }
        private DetailsFamillesReassurance GetModelFamillesReassurance(string codeGarantie, string userRights, IVoletsBlocsCategories serviceContext)
        {
            var toReturn = new DetailsFamillesReassurance
            {
                ListeLigneFamilleReassurance = GetGarFamillesReassurance(codeGarantie, userRights, serviceContext)
            };
            return toReturn;
        }
        private static List<LigneFamilleReassurance> GetGarFamillesReassurance(string codeGarantie, string userRights, IVoletsBlocsCategories serviceContext)
        {
            var listeFamillesReassurance = new List<LigneFamilleReassurance>();
            var result = serviceContext.GetGarFamillesReassurance(codeGarantie, userRights);
            if (result.Any())
                result.ForEach(m => listeFamillesReassurance.Add((LigneFamilleReassurance)m));
            listeFamillesReassurance.ForEach(elm => elm.AdditionalParam = userRights);
            return listeFamillesReassurance;
        }

        public DDLBranches LoadDDLBranches()
        {
            var ddlBranches = new DDLBranches();
            var branches = new List<AlbSelectListItem>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=channelClient.Channel;
                var result = serviceContext.LoadListBranches();
                branches = result.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
            }
            ddlBranches.Branches = branches;
            return ddlBranches;
        }
        public DDLSousBranches LoadDDLSousBranches(string codeBranche)
        {
            var ddlSousBranches = new DDLSousBranches();
            var sousBranches = new List<AlbSelectListItem>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=channelClient.Channel;
                var result = serviceContext.LoadListSousBranches(codeBranche);
                sousBranches = result.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
            }
            ddlSousBranches.SousBranches = sousBranches;
            return ddlSousBranches;
        }
        public DDLCategories LoadDDLCategories(string codeBranche, string codeSousBranche)
        {
            var ddlCategories = new DDLCategories();
            var categories = new List<AlbSelectListItem>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=channelClient.Channel;
                var result = serviceContext.LoadListCategories(codeBranche, codeSousBranche);
                categories = result.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
            }
            ddlCategories.Categories = categories;
            return ddlCategories;
        }

        public DDLFamilles LoadDDLFamilles()
        {
            var ddlFamilles = new DDLFamilles();
            var familles = new List<AlbSelectListItem>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=channelClient.Channel;
                var result = serviceContext.LoadListFamilles(string.Empty, string.Empty, "REASS", "GARAN", string.Empty, null, false, false, null);
                familles = result.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
            }
            ddlFamilles.Familles = familles;
            return ddlFamilles;
        }

        private ActionResult ModificationGarantie(LigneGarantie garantieLigne, IVoletsBlocsCategories serviceContext)
        {
            var toReturn = new List<LigneGarantie>();
            if (string.IsNullOrEmpty(garantieLigne.Abrege) && !string.IsNullOrEmpty(garantieLigne.DesignationGarantie))
            {
                if (garantieLigne.DesignationGarantie.Length <= 30) {
                    garantieLigne.Abrege = garantieLigne.DesignationGarantie;
                }
                else {
                    garantieLigne.Abrege = garantieLigne.DesignationGarantie.Substring(0, 30);
                }
            }

            var garantie = new ParamGarantieDto
            {
                CodeGarantie = garantieLigne.CodeGarantie,
                DesignationGarantie = garantieLigne.DesignationGarantie,
                Abrege = garantieLigne.Abrege,
                CodeTaxe = garantieLigne.CodeTaxe,
                CodeCatNat = garantieLigne.CodeCatNat,
                GarantieCommune = garantieLigne.IsGarantieCommune.BoolToYesNo(),
                CodeTypeDefinition = garantieLigne.CodeTypeDefinition,
                CodeTypeInformation = garantieLigne.CodeTypeInformation,
                Regularisable = garantieLigne.IsRegularisable.BoolToYesNo(),
                CodeTypeGrille = garantieLigne.CodeTypeGrille,
                Inventaire = garantieLigne.IsLieInventaire.BoolToYesNo(),
                CodeTypeInventaire = garantieLigne.CodeTypeInventaire,
                AttentatGareat = garantieLigne.IsAttentatGareat.BoolToYesNo()
            };
            var retourMsg = serviceContext.EnregistrerGarantie(garantie, garantieLigne.ModeOperation, garantieLigne.AdditionalParam);
            if (!string.IsNullOrEmpty(retourMsg)) throw new AlbFoncException(retourMsg);

            var result = serviceContext.GetGaranties(string.Empty, string.Empty, garantieLigne.AdditionalParam, false);
            if (result.Any()) {
                result.ForEach(m => toReturn.Add((LigneGarantie)m));
            }
            toReturn.ForEach(elm => elm.AdditionalParam = garantieLigne.AdditionalParam);
            return PartialView("ListeGaranties", toReturn);
        }
        #endregion
    }
}
