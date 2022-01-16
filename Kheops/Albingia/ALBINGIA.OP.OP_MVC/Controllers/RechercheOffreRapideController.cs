using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModeleRechercheOffreRapide;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using EmitMapper;
using OP.WSAS400.DTO.OffresRapide;
using OPServiceContract.IAdministration;
using OPServiceContract.ISaisieCreationOffre;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class RechercheOffreRapideController : ControllersBase<ModeleRechercheOffreRapidePage>
    {

        public ActionResult Index()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;

                model.PageTitle = "Recherche offre rapide";
                model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;


                var result = voletsBlocsCategoriesClient.GetRechercheOffresRapideReferentiel();

                model.TypeTraitements = result.TypeTraitements.Select(i => new AlbSelectListItem
                {
                    Value = i.Code,
                    Text = i.Libelle,
                    Title = string.Format("{0} - {1}", i.Code, i.Libelle)
                }).ToList();

                model.Branches = result.Branches.Select(i => new AlbSelectListItem
                {
                    Value = i.Code,
                    Text = i.Libelle,
                    Title = string.Format("{0} - {1}", i.Code, i.Libelle)
                }).ToList();

                model.Cibles = result.Cibles.Select(i => new AlbSelectListItem
                {
                    Value = i.Code,
                    Text = i.Libelle,
                    Title = string.Format("{0} - {1}", i.Code, i.Libelle)
                }).ToList();


                model.Periodicites = result.Periodicites.Select(i => new AlbSelectListItem
                {
                    Value = i.Code,
                    Text = i.Libelle,
                    Title = string.Format("{0} - {1}", i.Code, i.Libelle)
                }).ToList();


                model.Result = new ModeleOffreRapideResult { Offres = new List<ModeleOffreRapideInfo>() };
                model.Result.PageNumber = 1;
                model.Result.PageSize = PageSize;


                DisplayBandeau();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult RechercherOffreRapide(ModeleOffreRapideFiltre filtre)
        {

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var filtreDto = ObjectMapperManager.DefaultInstance.GetMapper<ModeleOffreRapideFiltre, OffreRapideFiltreDto>().Map(filtre);

                if (filtre.TypeOffres != null && filtre.TypeOffres.Length == 1)
                    filtreDto.TypeOffre = filtre.TypeOffres[0];


                var mapper = ObjectMapperManager.DefaultInstance.GetMapper<OffreRapideInfoDto, ModeleOffreRapideInfo>();



                filtreDto.StartLine = filtre.PageNumber * PageSize - PageSize - 1;
                filtreDto.EndLine = filtre.PageNumber * PageSize - 1;

                var dto = voletsBlocsCategoriesClient.GetOffresRapides(filtreDto);

                var modele = ObjectMapperManager.DefaultInstance.GetMapper<OffreRapideResultDto, ModeleOffreRapideResult>().Map(dto);

                modele.Offres = dto.Offres.Select(i => mapper.Map(i)).ToList();


                modele.NbCount = dto.NbCount;
                modele.PageNumber = filtre.PageNumber;
                modele.PageSize = PageSize;


                return PartialView("ListeOffresRapides", modele);
            }
        }

        [HttpGet]
        public JsonResult RechercherCibles(string codeBranche = null)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var policeServiceClient=client.Channel;
                var list = new List<AlbSelectListItem>
                {
                    new AlbSelectListItem
                    {
                        Value = string.Empty,
                        Text = string.Empty,
                        Title = string.Empty
                    }
                };


                var cibles = policeServiceClient.GetCibles(codeBranche == string.Empty ? null : codeBranche, true, true, true);

                cibles.ForEach(c => list.Add(new AlbSelectListItem
                {
                    Value = c.Code,
                    Text = c.Libelle,
                    Title = string.Format("{0} - {1}", c.Code, c.Libelle)
                }));




                return Json(list, JsonRequestBehavior.AllowGet); 
            }
        }

    }


}