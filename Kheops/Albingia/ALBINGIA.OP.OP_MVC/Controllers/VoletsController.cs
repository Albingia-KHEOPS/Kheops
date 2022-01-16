using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesCategories;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesVolets;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract.IAdministration;
using OPServiceContract.ISaisieCreationOffre;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class VoletsController : ControllersBase<ModeleVoletPage>
    {
        #region Méthode Publique
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {

            model.PageTitle = "Volets";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            SetDetailVolet();
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult Recherche(string code, string description)
        {
            var toReturn = new List<ModeleVolet>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategories=channelClient.Channel;
                var result = voletsBlocsCategories.VoletsGet(code, description).ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => toReturn.Add((ModeleVolet)m));
            }

            return PartialView("ParametreVBM/ListVolets", toReturn);
        }

        [ErrorHandler]
        public ActionResult ConsultVolet(string codeId, string readOnly)
        {
            ModeleVolet toReturn;
            var tstRsqGarantie = false;
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategories=channelClient.Channel;
                var result = voletsBlocsCategories.VoletInfoGet(codeId);
                toReturn = new ModeleVolet
                {
                    GuidId = result.GuidId.ToString(CultureInfo.InvariantCulture),
                    Code = result.Code,
                    Description = result.Description,
                    DateCreation = result.DateCreation,
                    ReadOnly = (readOnly == "1"), // TODO : a enlever,
                    IsVoletGeneral = result.IsVoletGeneral,
                    IsVoletCollapse = result.IsVoletCollapse
                };
                if (result.CategoriesVolet != null && result.CategoriesVolet.Any())
                {
                    result.CategoriesVolet.ToList().ForEach(m => toReturn.CategoriesVolet.Add((ModeleCategorie)m));
                }
                //if (result.Categories != null && result.Categories.Any())
                //{
                //  toReturn.Categories = result.Categories.Select(m => new AlbSelectListItem { Value = m.GuidId, Text = string.Concat(m.CodeBranche, ", ", m.Code, ", ", m.Description), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Description) }).ToList();
                //  tstRsqGarantie = true;

                //}
                if (result.Branches != null && result.Branches.Any())
                {
                    toReturn.Branche = result.Branches.Select(br => new AlbSelectListItem { Text = string.Concat(br.Code, " - ", br.Libelle), Value = br.Code, Title = string.Concat(br.Code, " - ", br.Libelle) }).ToList();
                    if (toReturn.Branche.Find(br => br.Value == result.Branche) != null)
                        toReturn.Branche.Find(br => br.Value == result.Branche).Selected = true;
                    tstRsqGarantie = true;
                }
                if (tstRsqGarantie)
                {
                    using (var channelClientPol = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                    {
                        var policeServices=channelClientPol.Channel;
                        List<ParametreDto> caracteres = policeServices.ObtenirParametres(string.Empty, string.Empty, string.Empty, string.Empty, "KHEOP", "CARAC", ModeConsultation.Standard).ToList();
                        toReturn.Caracteres = caracteres.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Code) }).ToList();
                    }
                }
            }
            return PartialView("EditorTemplates/ConsultVolet", toReturn);
        }


        [ErrorHandler]
        public ActionResult GetCiblesBranche(string branche)
        {

            ModeleCibleCarac cibleCarac;
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var policeServices=channelClient.Channel;
                var cibles = policeServices.GetCibles(branche, false, true, true).ToList();

                var caracteres = policeServices.ObtenirParametres(string.Empty, string.Empty, string.Empty, string.Empty, "KHEOP", "CARAC", ModeConsultation.Standard).ToList();
                if ((cibles == null || cibles.Count == 0) && (caracteres == null || caracteres.Count == 0))
                {
                    return null;
                }
                cibleCarac = new ModeleCibleCarac();
                cibleCarac.Cibles = cibles.Select(br => new AlbSelectListItem { Text = string.Concat(br.Code, "-", br.Libelle), Value = br.Code, Title = string.Concat(br.Code, "-", br.Libelle) }).ToList();
                cibleCarac.Caracteres = caracteres.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code, Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Code) }).ToList();
            }

            return PartialView("CiblesCarac", cibleCarac);
        }


        [ErrorHandler]
        public ActionResult InitialiserVolet()
        {
            ModeleVolet model = new ModeleVolet
            {
                Branche = new List<AlbSelectListItem>()
            };
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var context=channelClient.Channel;
                var branches = context.BranchesGet();
                if (branches.Any())
                {
                    model.Branche = SlectListBranches(branches);
                }
            }

            return PartialView("EditorTemplates/ConsultVolet", model);
        }

        [ErrorHandler]
        public void Enregistrer(string codeId, string code, string description, string update, string branche, bool isVoletGeneral, bool isVoletCollapse)
        {
            string sIsVoletGeneral = (isVoletGeneral ? "O" : "N");
            string sIsVoletCollapse = (isVoletCollapse ? "RP" : "");
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategories=channelClient.Channel;
                //voletsBlocsCategories.VoletInfoSet(codeId, code.Replace(" ", "").Replace("'", "''"), description.Replace("'", "''"), branche, sIsVoletGeneral, sIsVoletCollapse, update, GetUser());
                voletsBlocsCategories.VoletInfoSet(codeId, code.Replace(" ", ""), description, branche, sIsVoletGeneral, sIsVoletCollapse, update, GetUser());
            }
        }

        [ErrorHandler]
        public string Supprimer(string code)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategories=channelClient.Channel;
                return voletsBlocsCategories.SupprimerVolet(code, UserId);
            }
        }

        [ErrorHandler]
        public ActionResult AssocieCategorie(string codeIdVolet, string codeVolet, string codeIdCategorie, string codeCaractere)
        {
            ModeleVolet ToReturn = new ModeleVolet();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategories=channelClient.Channel;
                var categorie = voletsBlocsCategories.ObtenirCategorieByCode(codeIdCategorie);

                voletsBlocsCategories.EnregistrerVoletByCategorie("", categorie.CodeBranche, categorie.Code, codeIdCategorie, codeVolet, codeIdVolet, codeCaractere, "");

                var result = voletsBlocsCategories.VoletInfoGet(codeIdVolet);
                if (result.CategoriesVolet != null && result.CategoriesVolet.Any())
                {
                    result.CategoriesVolet.ToList().ForEach(m => ToReturn.CategoriesVolet.Add((ModeleCategorie)m));
                }
            }
            return PartialView("ParametreVBM/ListCategories", ToReturn.CategoriesVolet);
        }

        #endregion

        #region Méthode Privée

        private void SetDetailVolet()
        {
        }
        private List<AlbSelectListItem> SlectListBranches(List<BrancheDto> branchesDto)
        {

            return branchesDto.Select(br => new AlbSelectListItem { Text = string.Concat(br.Code, " - ", br.Nom), Value = br.Code, Title = string.Concat(br.Code, " - ", br.Nom) }).ToList();

        }
        #endregion

    }
}
