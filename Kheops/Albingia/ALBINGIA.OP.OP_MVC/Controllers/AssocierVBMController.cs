using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModeleCibles;
using ALBINGIA.OP.OP_MVC.Models.ModelesBlocs;
using ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesVolets;
using OP.WSAS400.DTO.Bloc;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Volet;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class AssocierVBMController : ControllersBase<ModeleAssocierVBMPage>
    {

        #region Propriété Publique

        #endregion


        #region Méthode Publique
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Association Volets - Blocs - Models";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            SetDetailAssociation();

            return View(model);
        }

        [ErrorHandler]
        public ActionResult ChargerCible(string codeBranche)
        {
            List<ModeleCible> toReturn = ObtenirListCibles(codeBranche);
            return PartialView("ParametreVBM/ListCibles", toReturn);
        }

        [ErrorHandler]
        public ActionResult ChargerVolet(string codeId, string codeBranche)
        {
            List<ModeleVolet> toReturn = ObtenirListVolets(codeId, codeBranche);
            return PartialView("ParametreVBM/ListVolets", toReturn);
        }

        [ErrorHandler]
        public ActionResult ChargerBloc(string codeId)
        {
            List<ModeleBloc> toReturn = ObtenirListBlocs(codeId);
            return PartialView("ParametreVBM/ListBlocs", toReturn);
        }

        [ErrorHandler]
        public ActionResult ChargerModele(string codeId)
        {
            List<ModeleGarantieModele> toReturn = ObtenirListModeles(codeId);
            return PartialView("ParametreVBM/ListGarantieModeles", toReturn);
        }

        [ErrorHandler]
        public ActionResult EnregistrerVoletByCible(string codeId, string codeBranche, string codeCible, string codeIdCible, string codeVolet, string codeCaractere, double ordreVolet)
        {
            if (!string.IsNullOrEmpty(codeBranche) && !string.IsNullOrEmpty(codeCible) && !string.IsNullOrEmpty(codeIdCible) && !string.IsNullOrEmpty(codeVolet) && !string.IsNullOrEmpty(codeCaractere))
            {
                EnregistrerVolet(codeId, codeBranche, codeCible, codeIdCible, codeVolet, string.Empty, codeCaractere, ordreVolet, GetUser());
                List<ModeleVolet> toReturn = ObtenirListVolets(codeIdCible, codeBranche);
                return PartialView("ParametreVBM/ListVolets", toReturn);
            }
            else
                throw new AlbFoncException("Il a des valeurs nulles ou vides", sendMail: false, trace: false);
        }

        [ErrorHandler]
        public ActionResult EnregistrerBlocByCible(string codeId, string codeBranche, string codeCible, string codeVolet, string codeIdVolet, string codeBloc, string codeIdBloc, string codeCaractere, double ordreBloc)
        {
            if (!string.IsNullOrEmpty(codeBranche) && !string.IsNullOrEmpty(codeCible) && !string.IsNullOrEmpty(codeIdVolet) && !string.IsNullOrEmpty(codeVolet) && !string.IsNullOrEmpty(codeBloc) && !string.IsNullOrEmpty(codeCaractere))
            {
                EnregistrerBloc(codeId, codeBranche, codeCible, codeVolet, codeIdVolet, codeBloc, codeIdBloc, codeCaractere, ordreBloc, GetUser());
                List<ModeleBloc> toReturn = ObtenirListBlocs(codeIdVolet);
                return PartialView("ParametreVBM/ListBlocs", toReturn);
            }
            else
                throw new AlbFoncException("Il a des valeurs nulles ou vides", sendMail: false, trace: false);
        }


        [ErrorHandler]
        public ActionResult EnregistrerModeleByCible(string codeId, string codeIdBloc, string dateApp, string codeTypo, string codeModele)
        {
            EnregistrerModele(codeId, codeIdBloc, dateApp, codeTypo, codeModele, GetUser());
            List<ModeleGarantieModele> toReturn = ObtenirListModeles(codeIdBloc);
            return PartialView("ParametreVBM/ListGarantieModeles", toReturn);
        }

        [ErrorHandler]
        public ActionResult SupprimerVolet(string codeId, string codeIdCible, string codeBranche)
        {
            if (!string.IsNullOrEmpty(codeId) && !string.IsNullOrEmpty(codeIdCible) && !string.IsNullOrEmpty(codeBranche))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    string strReturn = serviceContext.SupprimerVoletByCategorie(codeId, codeBranche, codeIdCible, UserId);
                    if (!string.IsNullOrEmpty(strReturn))
                        throw new AlbFoncException(strReturn, trace: true, sendMail: true, onlyMessage: true);
                }
                List<ModeleVolet> toReturn = ObtenirListVolets(codeIdCible, codeBranche);
                return PartialView("ParametreVBM/ListVolets", toReturn);
            }
            else
                throw new AlbTechException(new System.Exception("Impossible de supprimer le volet, paramètres incorrects"), sendMail: true, trace: false);
        }

        [ErrorHandler]
        public ActionResult SupprimerBloc(string codeId, string codeIdVolet)
        {
            if (!string.IsNullOrEmpty(codeId) && !string.IsNullOrEmpty(codeIdVolet))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    string strReturn = serviceContext.SupprimerBlocByCategorie(codeId, codeIdVolet, UserId);
                    if (!string.IsNullOrEmpty(strReturn))
                        throw new AlbFoncException(strReturn, trace: true, sendMail: true, onlyMessage: true);
                }

                List<ModeleBloc> toReturn = ObtenirListBlocs(codeIdVolet);
                return PartialView("ParametreVBM/ListBlocs", toReturn);
            }
            else
                throw new AlbTechException(new System.Exception("Impossible de supprimer le bloc, paramètres incorrects"), sendMail: true, trace: false);

        }

        [ErrorHandler]
        public ActionResult SupprimerModele(string codeId, string codeIdBloc)
        {
            SupprimerModele(codeId);

            List<ModeleGarantieModele> toReturn = ObtenirListModeles(codeIdBloc);
            return PartialView("ParametreVBM/ListGarantieModeles", toReturn);
        }

        #endregion

        #region Méthode Privée

        private void SetDetailAssociation()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var branches = serviceContext.BranchesGet().ToList();
                model.Branches = branches.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Nom, Selected = false }).ToList();

                model.Cibles = new List<ModeleCible>();

                List<DtoVolet> volets = serviceContext.VoletsGetList().ToList();
                model.VoletsList = volets.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + ", " + m.Description, Selected = false }).ToList();

                List<ParametreDto> caracteresVolet = serviceContext.ObtenirParametres("KHEOP", "CARAC").ToList();
                model.CaracteresVolet = caracteresVolet.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code, Selected = false }).ToList();
                //suppression du caractere A
                AlbSelectListItem A = model.CaracteresVolet.Find(elm => elm.Value == "A");
                if (A != null)
                    model.CaracteresVolet.Remove(A);


                List<BlocDto> blocs = serviceContext.BlocsGetList().ToList();
                model.BlocsList = blocs.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + ", " + m.Description, Selected = false }).ToList();

                List<ParametreDto> caracteresBloc = serviceContext.ObtenirParametres("KHEOP", "CARAC").ToList();
                model.CaracteresBloc = caracteresBloc.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code, Selected = false }).ToList();
                //suppression du caractere A
                A = model.CaracteresBloc.Find(elm => elm.Value == "A");
                if (A != null)
                    model.CaracteresBloc.Remove(A);

                List<GarantieModeleDto> garantieModeles = serviceContext.ModelesGetList().ToList();
                model.ModelesList = garantieModeles.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + ", " + m.Description, Selected = false }).ToList();

                List<ParametreDto> typologies = serviceContext.ObtenirParametres("KHEOP", "MODTY").ToList();
                model.TypologiesList = typologies.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code, Selected = false }).ToList();
            }
        }

        private List<ModeleCible> ObtenirListCibles(string codeBranche)
        {
            var toReturn = new List<ModeleCible>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                List<CibleDto> result = serviceContext.CiblesGet(codeBranche).ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => toReturn.Add((ModeleCible)m));
            }
            return toReturn;
        }

        private List<ModeleVolet> ObtenirListVolets(string codeId, string codeBranche)
        {
            var toReturn = new List<ModeleVolet>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                List<DtoVolet> result = serviceContext.VoletsGetByCible(codeId, codeBranche).ToList();
                if (result != null && result.Count > 0)
                    result.ForEach(m => toReturn.Add((ModeleVolet)m));
            }

            return toReturn;
        }

        private List<ModeleBloc> ObtenirListBlocs(string codeId)
        {
            var toReturn = new List<ModeleBloc>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                List<BlocDto> result = serviceContext.BlocsGetByVolet(codeId).ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => toReturn.Add((ModeleBloc)m));
            }
            return toReturn;
        }

        private List<ModeleGarantieModele> ObtenirListModeles(string codeId)
        {
            var toReturn = new List<ModeleGarantieModele>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                List<GarantieModeleDto> result = serviceContext.GarantieModelesGetByBloc(codeId).ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => toReturn.Add((ModeleGarantieModele)m));
            }

            return toReturn;
        }

        private void EnregistrerVolet(string codeId, string codeBranche, string codeCible, string codeIdCible, string codeVolet, string codeIdVolet, string codeCaractere, double ordreVolet, string user = "")
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.EnregistrerVoletByCible(codeId, codeBranche, codeCible, codeIdCible, codeVolet, codeIdVolet, codeCaractere, ordreVolet, user);
            }
        }

        private void EnregistrerBloc(string codeId, string codeBranche, string codeCible, string codeVolet, string codeIdVolet, string codeBloc, string codeIdBloc, string codeCaractere, double ordreBloc, string user = "")
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.EnregistrerBlocByCible(codeId, codeBranche, codeCible, codeVolet, codeIdVolet, codeBloc, codeIdBloc, codeCaractere, ordreBloc, user);
            }
        }

        private void EnregistrerModele(string codeId, string codeIdBloc, string dateApp, string codeTypo, string codeModele, string user = "")
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                string result = serviceContext.EnregistrerModeleByCible(codeId, codeIdBloc, dateApp, codeTypo, codeModele, user);
                if (!string.IsNullOrEmpty(result))
                    throw new AlbFoncException(result);
            }
        }

        private void SupprimerModele(string codeId)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                string strReturn = serviceContext.SupprimerModeleByCategorie(codeId, UserId);
                if (!string.IsNullOrEmpty(strReturn))
                    throw new AlbFoncException(strReturn, trace: true, sendMail: true, onlyMessage: true);
            }
        }

        #endregion

    }
}
