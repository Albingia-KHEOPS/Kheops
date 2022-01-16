using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesBlocs;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.Bloc;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class BlocsController : ControllersBase<ModeleBlocPage>
    {
        #region Méthode Publique
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {


            model.PageTitle = "Blocs";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult Recherche(string code, string description)
        {
            var ToReturn = new List<ModeleBloc>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var result = voletsBlocsCategoriesClient.BlocsGet(code, description).ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => ToReturn.Add((ModeleBloc)m));
            }

            return PartialView("ListeBlocs", ToReturn);
        }

        [ErrorHandler]
        public ActionResult ConsultBloc(string codeId, string readOnly)
        {
            ModeleBloc toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                BlocDto result = voletsBlocsCategoriesClient.BlocInfoGet(codeId);
                toReturn = new ModeleBloc
                {
                    GuidId = result.GuidId.ToString(),
                    Code = result.Code,
                    Description = result.Description,
                    DateCreation = result.DateCreation,
                    ReadOnly = (readOnly == "1"),
                    ListeBlocsIncompatibles = new List<ModeleLigneBloc>(),
                    ListeBlocsAssocies = new List<ModeleLigneBloc>(),
                    ListeReferentielBlocsIncompatibles = new List<AlbSelectListItem>(),
                    ListeReferentielBlocsAssocies = new List<AlbSelectListItem>()
                };

                //Chargement des blocs associés
                var lstBlocsIncompatibles = voletsBlocsCategoriesClient.GetListeBlocsIncompatiblesAssocies(codeId, "I");
                if (lstBlocsIncompatibles.Any())
                    lstBlocsIncompatibles.ForEach(elm => toReturn.ListeBlocsIncompatibles.Add((ModeleLigneBloc)elm));

                //chargement des blocs incompatibles
                var lstBlocsAssocies = voletsBlocsCategoriesClient.GetListeBlocsIncompatiblesAssocies(codeId, "A");
                if (lstBlocsAssocies.Any())
                    lstBlocsAssocies.ForEach(elm => toReturn.ListeBlocsAssocies.Add((ModeleLigneBloc)elm));

                //Chargement de la liste des blocs incompatibles "ajoutables"
                var lstBlocsReferentielIncompatibles = voletsBlocsCategoriesClient.GetListeBlocsReferentielIncompatiblesAssocies(codeId, "I");
                if (lstBlocsReferentielIncompatibles.Any())
                    lstBlocsReferentielIncompatibles.ForEach(elm => toReturn.ListeReferentielBlocsIncompatibles.Add(
                        new AlbSelectListItem
                        {
                            Text = elm.Code + " - " + elm.Description,
                            Value = elm.GuidId.ToString(),
                            Selected = false,
                            Title = elm.Code + " - " + elm.Description
                        }));
                toReturn.ListeBlocsIncompatibles.ForEach(elm => elm.ListeReferentielBlocsIncompatibles = toReturn.ListeReferentielBlocsIncompatibles.ToList());
                toReturn.ListeBlocsIncompatibles.ForEach(
                    m => m.ListeReferentielBlocsIncompatibles.Add(new AlbSelectListItem
                    {
                        Selected = true,
                        Value = m.GuidId,
                        Text = m.Code + " - " + m.Description,
                        Title = m.Code + " - " + m.Description
                    })
                    );

                //Chargement de la liste des blocs associés "ajoutables"
                var lstBlocsReferentielAssocies = voletsBlocsCategoriesClient.GetListeBlocsReferentielIncompatiblesAssocies(codeId, "A");
                if (lstBlocsReferentielAssocies.Any())
                    lstBlocsReferentielAssocies.ForEach(elm => toReturn.ListeReferentielBlocsAssocies.Add(
                        new AlbSelectListItem
                        {
                            Text = elm.Code + " - " + elm.Description,
                            Value = elm.GuidId.ToString(),
                            Selected = false,
                            Title = elm.Code + " - " + elm.Description
                        }));
                toReturn.ListeBlocsAssocies.ForEach(elm => elm.ListeReferentielBlocsAssocies = toReturn.ListeReferentielBlocsAssocies.ToList());
                toReturn.ListeBlocsAssocies.ForEach(
                    m => m.ListeReferentielBlocsAssocies.Add(new AlbSelectListItem
                    {
                        Selected = true,
                        Value = m.GuidId,
                        Text = m.Code + " - " + m.Description,
                        Title = m.Code + " - " + m.Description
                    })
                    );

            }
            return PartialView("DetailsBloc", toReturn);
        }

        [ErrorHandler]
        public ActionResult InitialiserBloc()
        {
            return PartialView("DetailsBloc", new ModeleBloc());
        }

        [ErrorHandler]
        public void Enregistrer(string codeId, string code, string description, string update)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                voletsBlocsCategoriesClient.BlocInfoSet(codeId, code.Replace(" ", ""), description, update, GetUser());
            }
        }

        [ErrorHandler]
        public string Supprimer(string code)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                return voletsBlocsCategoriesClient.SupprimerBloc(code, UserId);
            }
        }

        [ErrorHandler]
        public ActionResult EnregistrerBlocIncompatibleAssocie(string idAssociation, string mode, string typeBloc, string idBloctraite, string idBlocIncompatibleAssocie)
        {
            if (!string.IsNullOrEmpty(idAssociation) && !string.IsNullOrEmpty(typeBloc) && !string.IsNullOrEmpty(idBloctraite) && !string.IsNullOrEmpty(idBlocIncompatibleAssocie))
            {
                if (idAssociation != idBlocIncompatibleAssocie)
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var serviceContext=client.Channel;
                        string err = serviceContext.EnregistrerBlocIncompatibleAssocie(idAssociation, mode, idBloctraite, idBlocIncompatibleAssocie, typeBloc, GetUser());
                        if (!string.IsNullOrEmpty(err))
                            throw new AlbFoncException(err, trace: true, sendMail: true, onlyMessage: true);
                    }
                }
            }
            return ConsultBloc(idBloctraite, "0");
        }

        #endregion

        

    }
}
