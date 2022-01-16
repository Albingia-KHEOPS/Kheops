using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamCible;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamCibleController : ControllersBase<ModeleParamCiblePage>
    {

        #region Membres privés
        public static readonly string MODE_EDITION = "U";
        public static readonly string MODE_CREATION = "I";
        public static readonly string MODE_DUPLICATION = "D";
        #endregion
        #region Méthodes Publiques
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrages des cibles";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult GetFamillesActiv(string codeConcept, string codeFamille)
        {

            var ToReturn = new ModeleRechercheFamille();
            ToReturn.ListFamilles = new List<LigneFamille>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var result = voletsBlocsCategoriesClient.GetFamilles(codeConcept, codeFamille, string.Empty, string.Empty, string.Empty).ToList();
                if (result != null && result.Count > 0)
                {
                    result.ForEach(m => ToReturn.ListFamilles.Add((LigneFamille)m));
                    ToReturn.concept = result.FirstOrDefault().CodeConcpet;
                    ToReturn.famille = result.FirstOrDefault().CodeFamille;
                }

            }

            if (!string.IsNullOrEmpty(codeConcept) || !string.IsNullOrEmpty(codeFamille))
            {
                return PartialView("RechercheFamille", ToReturn);
            }
            return PartialView("ListFamilles", ToReturn);
        }
        [ErrorHandler]
        public ActionResult Recherche(string code, string description)
        {
            var ToReturn = new List<LigneCible>();
            //--------------------
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var result = voletsBlocsCategoriesClient.LoadListCible(code, description).ToList();
                if (result != null && result.Count > 0)
                    result.ForEach(m => ToReturn.Add((LigneCible)m));
            }

            return PartialView("ParamListCibles", ToReturn);
        }

        [ErrorHandler]
        public ActionResult ConsultCible(string codeId, string readOnly)
        {
            DetailsCible toReturn = new DetailsCible();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                if (!string.IsNullOrEmpty(codeId))
                {
                    //Récupération des informations de base de la cible sélectionnée
                    toReturn = (DetailsCible)voletsBlocsCategoriesClient.GetCible(codeId);
                    toReturn.Mode = "Update";
                }
                else
                    toReturn.Mode = "Insert";

                if (toReturn.DateCreation != null && toReturn.DateCreation != "0")
                    toReturn.DateCreation = AlbConvert.ConvertIntToDate(int.Parse(toReturn.DateCreation)).Value.ToString("dd/MM/yyyy");
                //Récupération de la liste des BSC de la cible sélectionnée
                toReturn.ListeBSC = new List<LigneBSC>();
                int Id = -1;
                if (int.TryParse(codeId, out Id))
                {
                    var result = voletsBlocsCategoriesClient.GetCibleBranches(Id).ToList();
                    if (result != null)
                    {
                        result.ForEach(m => toReturn.ListeBSC.Add((LigneBSC)m));
                    }
                }

                //Récupération des branches   
                toReturn.ListeBranches = new DropListBranches();
                toReturn.ListeBranches.Branches = new List<AlbSelectListItem>();
                var vBranches = voletsBlocsCategoriesClient.GetBranches(Id);
                if (vBranches != null)
                {
                    vBranches.ForEach(branche => toReturn.ListeBranches.Branches.Add(new AlbSelectListItem { Text = branche.Code + " - " + branche.Libelle, Value = branche.Code, Selected = false, Title = branche.Code + " - " + branche.Libelle }));
                }

                //Initialisation des sous branches
                toReturn.ListeSousBranches = new DropListSousBranches();
                toReturn.ListeSousBranches.SousBranches = new List<AlbSelectListItem>();

                //Initialisation des catégories
                toReturn.ListeCategories = new DropListCategories();
                toReturn.ListeCategories.Categories = new List<AlbSelectListItem>();

                toReturn.Grilles = new List<AlbSelectListItem>();
                var vGrilles = voletsBlocsCategoriesClient.GetGrilles();
                if (vGrilles != null)
                {
                    vGrilles.ForEach(grille => toReturn.Grilles.Add(new AlbSelectListItem { Text = grille.Code + " - " + grille.Libelle, Value = grille.Code, Selected = false, Title = grille.Code + " - " + grille.Libelle }));
                    //var res = toReturn.Grilles.Find(elm => elm.Value == toReturn.Grille);
                    //if (res != null)
                    //    toReturn.Grilles.Find(elm => elm.Value == toReturn.Grille).Selected = true;
                }
            }

            return PartialView("ParamDetailCible", toReturn);
        }


        [ErrorHandler]
        public ActionResult GetSousBranches(string codeBranche, string drlSousBranches)
        {
            DropListSousBranches toReturn = new DropListSousBranches();
            toReturn.SousBranches = new List<AlbSelectListItem>();

            if (!string.IsNullOrEmpty(codeBranche))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    var result = voletsBlocsCategoriesClient.GetSousBranches(codeBranche);
                    result.ForEach(m => toReturn.SousBranches.Add(new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Code + " - " + m.Libelle }));
                }
            }

            return PartialView("DrlSousBranches", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetCategories(string codeBranche, string codeSousBranche, string idCat)
        {
            DropListCategories toReturn = new DropListCategories();
            toReturn.Categories = new List<AlbSelectListItem>();

            if (!string.IsNullOrEmpty(codeBranche) && !string.IsNullOrEmpty(codeSousBranche))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    var result = voletsBlocsCategoriesClient.GetCategories(codeBranche, codeSousBranche);
                    result.ForEach(m => toReturn.Categories.Add(new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Code + " - " + m.Libelle }));
                }
            }

            return PartialView("DrlCategories", toReturn);
        }

        [ErrorHandler]
        public void Enregistrer(string mode, string codeId, string codeLib, string description, string grille, string famille, string concept)
        {
            int result = 1;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                if (mode == "Update")
                    voletsBlocsCategoriesClient.ModifierCible(codeId.Replace(" ", "").Replace("'", "''"), description.Replace("'", "''"), grille, famille, concept, GetUser());
                else if (mode == "Insert")
                    result = voletsBlocsCategoriesClient.AjouterCible(codeLib.Replace(" ", "").Replace("'", "''"), description.Replace("'", "''"), grille, famille, concept, GetUser());
                if (result == 0)
                    throw new AlbFoncException("Le Code Cible existe déjà", false, false, true);
                if (result == -1)
                    throw new AlbFoncException("Le Code ne doit pas être vide", false, false, true);
            }

        }

        [ErrorHandler]
        public string SupprimerCible(string code)
        {
            var toReturn = new List<LigneCible>();
            int Id;
            if (int.TryParse(code, out Id))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    return voletsBlocsCategoriesClient.SupprimerCible(Id, UserId);
                }
            }
            return "Code incorrect";
        }

        [ErrorHandler]
        public ActionResult SupprimerBranche(int codeGuidCible, string codeBSC)
        {
            var toReturn = new DropListBranches();
            toReturn.Branches = new List<AlbSelectListItem>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                string strReturn = voletsBlocsCategoriesClient.SupprimerCibleBSCByGuidId(codeBSC, UserId);
                if (!string.IsNullOrEmpty(strReturn))
                    throw new AlbFoncException(strReturn, trace: true, sendMail: true, onlyMessage: true);
                var vBranches = voletsBlocsCategoriesClient.GetBranches(codeGuidCible);
                if (vBranches != null)
                {
                    vBranches.ForEach(branche => toReturn.Branches.Add(new AlbSelectListItem { Text = branche.Libelle, Value = branche.Code, Selected = false, Title = branche.Code + " - " + branche.Libelle }));
                }
            }
            return PartialView("DrlBranches", toReturn);
        }

        [ErrorHandler]
        public ActionResult AssocierBranche(string guidIdCible, string codeCible, string codeBranche, string codeSousBranche, string codeCategorie)
        {
            var toReturn = new List<LigneBSC>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                int retour = 0;
                if (!string.IsNullOrEmpty(codeBranche))
                {
                    retour = voletsBlocsCategoriesClient.AjouterBSCCible(guidIdCible, codeCible, codeBranche, codeSousBranche, codeCategorie);
                }
                else retour = -2;

                //Récupération de la liste des BSC de la cible sélectionnée                
                int Id = -1;
                if (int.TryParse(guidIdCible, out Id))
                {
                    var result = voletsBlocsCategoriesClient.GetCibleBranches(Id).ToList();
                    if (result != null)
                    {
                        result.ForEach(m => toReturn.Add((LigneBSC)m));
                    }
                }
                return PartialView("ParamListBSC", toReturn);
            }
        }

        [ErrorHandler]
        public bool ExisteCiblePortefeuille(string guidIdCible)
        {
            int Id;
            if (int.TryParse(guidIdCible, out Id))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    return voletsBlocsCategoriesClient.ExisteCiblePortefeuille(Id);
                }
            }
            return false;
        }

        [ErrorHandler]
        public ActionResult EditBSC(int codeGuidCible, string guid, string codeBranche, string codeSousBranche, string codeCategorie)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                //Récupération des branches   
                var branches = new List<AlbSelectListItem>();
                var vBranches = voletsBlocsCategoriesClient.GetBranchesEdit(codeGuidCible, codeBranche);
                if (vBranches != null)
                {
                    vBranches.ForEach(branche => branches.Add(new AlbSelectListItem { Text = branche.Libelle, Value = branche.Code, Selected = false, Title = branche.Code + " - " + branche.Libelle }));
                }
                //Récupération des sous branches   
                var sousBranches = new List<AlbSelectListItem>();
                var vSousBranches = voletsBlocsCategoriesClient.GetSousBranches(codeBranche);
                if (vSousBranches != null)
                {
                    vSousBranches.ForEach(sousBranche => sousBranches.Add(new AlbSelectListItem { Text = sousBranche.Libelle, Value = sousBranche.Code, Selected = false, Title = sousBranche.Code + " - " + sousBranche.Libelle }));
                }
                //Récupération des catégories
                string vsousbranche = !string.IsNullOrEmpty(codeSousBranche.Substring(codeBranche.Length)) ? codeSousBranche.Substring(codeBranche.Length) : codeSousBranche;
                var categories = new List<AlbSelectListItem>();
                var varCategories = voletsBlocsCategoriesClient.GetCategories(codeBranche, vsousbranche);
                if (varCategories != null)
                {
                    varCategories.ForEach(categorie => categories.Add(new AlbSelectListItem { Value = categorie.Code, Text = categorie.Libelle, Selected = false, Title = categorie.Code }));
                }
                var editBSC = new EditBSC
                {
                    GuidId = guid,
                    Branches = branches,
                    Branche = codeBranche,
                    SousBranches = sousBranches,
                    SousBranche = vsousbranche,
                    Categories = categories,
                    Categorie = codeCategorie
                };
                return PartialView("EditBSC", editBSC);
            }
        }
        [ErrorHandler]
        public ActionResult ModifierBranche(int codeGuidCible, string codeBSC, string codeBranche, string codeSousBranche, string codeCategorie)
        {
            var toReturn = new List<LigneBSC>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                int retour = voletsBlocsCategoriesClient.ModifierCibleBSCByGuidId(codeGuidCible.ToString(), codeBSC, codeBranche, codeSousBranche, codeCategorie);
                //Récupération de la liste des BSC de la cible sélectionnée                
                var result = voletsBlocsCategoriesClient.GetCibleBranches(codeGuidCible).ToList();
                if (result != null)
                {
                    result.ForEach(m => toReturn.Add((LigneBSC)m));
                }
                return PartialView("ParamListBSC", toReturn);
            }
        }


        /// <summary>
        /// Permet d'accéder à la liste des templates liés à cette cible
        /// </summary>
        [ErrorHandler]
        public ActionResult EditTemplates(string idCible, string cible)
        {
            EditTemplate model = new EditTemplate();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetCibleTemplate(idCible, cible);
                model = (EditTemplate)result;
            }

            model.Cible = cible;
            return PartialView("ParamCibleAddTemplate", model);
        }

        /// <summary>
        /// Rafraichit la liste des templates liés
        /// à une cible
        /// </summary>
        [ErrorHandler]
        public ActionResult RefreshListTemplate(string idCible, string cible)
        {
            EditTemplate model = new EditTemplate();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetCibleTemplate(idCible, cible);
                model = (EditTemplate)result;
            }

            model.Cible = cible;
            return PartialView("ParamCibleListTemplate", model.Templates);
        }

        [ErrorHandler]
        public void UpdateTemplate(string idTemp, bool isChecked)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.UpdateCibleTemplate(idTemp, isChecked);
            }
        }

        /// <summary>
        /// Supprime une liaison entre une cible
        /// et un template
        /// </summary>
        [ErrorHandler]
        public ActionResult DeleteTemplate(string idCible, string idTemp)
        {
            EditTemplate model = new EditTemplate();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.DelCibleTemplate(idCible, idTemp);
                model = (EditTemplate)result;
            }

            return PartialView("ParamCibleListTemplate", model.Templates);
        }

        /// <summary>
        /// Associe un template à une cible
        /// </summary>
        [ErrorHandler]
        public ActionResult AssociateTemplate(string idCible, string idTemp)
        {
            EditTemplate model = new EditTemplate();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.AssociateCibleTemplate(idCible, idTemp);
                model = (EditTemplate)result;
            }
            return PartialView("ParamCibleListTemplate", model.Templates);
        }

        [ErrorHandler]
        public string GetParamTemplate(string idTemp)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                return serviceContext.GetParamTemplate(idTemp, AlbOpConstants.TEMPLATE_FLAG);
            }
        }

        #endregion

        #region Méthodes Privées
        #region DropDowlLists
        public List<AlbSelectListItem> LoadDDLRestrictions()
        {
            var restrictions = new List<AlbSelectListItem>();
            string[] names = Enum.GetNames(typeof(AlbConstantesMetiers.RestrictionsEnum));
            if (names.Length > 0)
                restrictions.Insert(0, new AlbSelectListItem { Value = names[0], Text = names[0], Selected = true });
            for (int i = 1; i <= names.Length - 1; i++)
            {
                restrictions.Insert(i, new AlbSelectListItem { Value = names[i], Text = names[i], Selected = false });
            }
            return restrictions;
        }
        public List<AlbSelectListItem> LoadDDLFiltres()
        {
            var filtres = new List<AlbSelectListItem>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;

                var filtresResult = voletsBlocsCategoriesClient.LoadListFiltres(string.Empty, string.Empty, string.Empty);
                filtres = filtresResult.Select(
                     f => new AlbSelectListItem
                     {
                         Value = f.Code.ToString(),
                         Text = !string.IsNullOrEmpty(f.Code.ToString()) || !string.IsNullOrEmpty(f.Libelle) ? string.Format("{0} - {1}", f.Code, f.Libelle) : "",
                         Selected = false,
                         Title = !string.IsNullOrEmpty(f.Code.ToString()) || !string.IsNullOrEmpty(f.Libelle) ? string.Format("{0} - {1}", f.Code, f.Libelle) : "",
                     }).ToList();
            }
            return filtres;
        }
        #endregion
        #endregion
    }
}
