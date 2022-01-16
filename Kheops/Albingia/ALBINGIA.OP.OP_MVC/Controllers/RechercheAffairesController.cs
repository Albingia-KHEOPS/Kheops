using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.CustomResult;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRecherche;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Services = Albingia.Kheops.OP.Application.Port.Driver;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class RechercheAffairesController : ControllersBase<ModeleRecherchePage> {
        [ErrorHandler]
        public ActionResult Index(string id) {
            return View(this.model);
        }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetDefaultFiltreAffaire() {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IReferentielPort>()) {
                var filtre = new FiltreAffaire();
                var catego = client.Channel.GetCiblesCategories();
                filtre.Branches = catego.GroupBy(x => x.Branche.Code).Select(g => {
                    var branche = g.First().Branche;
                    return new LabeledValue(branche.Code, branche.Libelle);
                }).OrderBy(b => b.Label).ToList();
                filtre.Cibles = catego.GroupBy(x => x.Cible.ID).Select(g => {
                    var cible = g.First().Cible;
                    return new LabeledValue { Id = cible.ID, Code = cible.Code, Label = cible.Description, Parent = g.First().Branche.Code };
                }).OrderBy(c => c.Label).ToList();
                filtre.Situations = client.Channel.GetSituations().Where(x => x.Code.ContainsChars()).Select(s => new LabeledValue(s.Code, s.Libelle)).OrderBy(s => s.Label).ToList();
                filtre.Etats = client.Channel.GetEtats().Select(e => new LabeledValue(e.Code, e.Libelle)).OrderBy(e => e.Label).ToList();
                return JsonNetResult.NewResultToGet(filtre);
            }
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult Search(FiltreAffaire filter) {
            var result = SearchAffair(filter);
            return JsonNetResult.NewResultToGet(result.ListResult ?? Enumerable.Empty<ModeleListResultRecherche>());
        }

        internal static ModeleResultRecherche SearchAffair(FiltreAffaire filter) {
            ModeleResultRecherche resultData = null;

            //Mise en cache des paramètres de recherche
            //if (parametresRecherche.AccesRecherche == AlbConstantesMetiers.TypeAccesRecherche.Standard && parametresRecherche.CritereParam == AlbConstantesMetiers.CriterParam.Standard) {
            //    AlbSessionHelper.ParametresRecherches = parametresRecherche;
            //}

            var parameters = new ModeleParametresRechercheDto {
                Etat = filter.CodeEtat ?? string.Empty,
                Branche = filter.CodeBranche ?? string.Empty,
                Cible = filter.CodeCible ?? string.Empty,
                CodeOffre = filter.Code ?? string.Empty,
                DDateDebut = filter.DateMin,
                DDateFin = filter.DateMax,
                NumAliment = filter.Version.StringValue(),
                Situation = filter.CodeSituation ?? string.Empty,
                IsActif = filter.GetEnCoursOnly,
                IsInactif = filter.GetInactifsOnly,
                GestionnaireCode = filter.Gestionnaire?.Code ?? string.Empty,
                PreneurAssuranceCode = filter.PreneurAssurance?.Code ?? default,
                PreneurAssuranceSIREN = int.TryParse(filter.Siren, out int s) ? s : default,
                PreneurAssuranceCP = filter.PreneurAssuranceVille?.CodePostal.StringValue(),
                SaufEtat = filter.ExcludeCodeEtat,
                SouscripteurCode = filter.Souscripteur?.Code ?? string.Empty,
                CabinetCourtageId = filter.Courtier?.Code ?? default,
                Type = filter.GetOffresOnly ? AlbConstantesMetiers.TYPE_OFFRE : filter.GetContratsOnly ? AlbConstantesMetiers.TYPE_CONTRAT : string.Empty,
                TypeDateRecherche = Enum.TryParse<AlbConstantesMetiers.TypeDateRecherche>(filter.CodeTypeDate, out var td) ? td : default,
                SouscripteurNom = string.Empty,
                AdresseRisqueVoie = filter.TexteAdresse ?? string.Empty,
                ExcludedCodeOffres = filter.ExcludedAffairs.Select(f => (ipb: f.CodeOffre, alx: f.Version)).ToArray()
            };

            if (filter.SortingField.ContainsChars()) {
                parameters.SortingName = filter.SortingField;
                parameters.SortingOrder = filter.Sorting;
            }
            else if (filter.Code.ContainsChars()) {
                parameters.SortingName = "PBIPB";
                parameters.SortingOrder = "ASC";
            }
            else {
                string order = "DESC";
                switch (parameters.TypeDateRecherche) {
                    case AlbConstantesMetiers.TypeDateRecherche.Saisie:
                        parameters.SortingName = "dateSaisie";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.Effet:
                        parameters.SortingName = "dateEffet";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.MAJ:
                        parameters.SortingName = "dateMaj";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.Creation:
                        parameters.SortingName = "dateCreation";
                        break;
                    default:
                        order = parameters.SortingOrder;
                        break;
                }
                parameters.SortingOrder = order;
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>()) {
                var screenClient = client.Channel;
                //var paramRechercheDto = ObjectMapperManager.DefaultInstance.GetMapper<ModeleParametresRecherche, ModeleParametresRechercheDto>().Map(parametresRecherche);
                //parameters.SouscripteurNom = string.Empty;
                var result = screenClient.RechercherOffresContrat(parameters, filter.Mode.ToString().ParseCode<ModeConsultation>());

                var isUserHorse = MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0;

                if (result != null) {
                    var listResultRecherche = new List<ModeleListResultRecherche>();

                    var i = 0;
                    foreach (var m in result.LstOffres) {
                        List<string> cibles = new List<string> { "RCCEQ", "GRCEQSAL", "IMACEQ", "GRCEQCLI", "EQDOM" };
                        if (!isUserHorse && cibles.Any(c => m.Branche.Cible.Code.Contains(c))) {
                            continue;
                        }

                        m.Compteur = i;
                        var recherche = (ModeleListResultRecherche)m;
                        //recherche.TypeDate = parametresRecherche.TypeDateRecherche;
                        listResultRecherche.Add(recherche);
                        i++;
                    }

                    //var totalLines = result.NbCount < paramRechercheDto.LineCount ? result.NbCount : paramRechercheDto.LineCount;
                    resultData = new ModeleResultRecherche {
                        NbCount = result.NbCount,
                        //StartLigne = paramRechercheDto.StartLine,
                        //EndLigne = paramRechercheDto.EndLine,
                        //PageNumber = parametresRecherche.PageNumber,
                        //LineCount = totalLines,
                        ListResult = listResultRecherche,
                        //CodeBranche = paramRechercheDto.Branche
                    };
                }
            }
            if (resultData == null) {
                resultData = new ModeleResultRecherche();
            }

            //resultData.AccesRecherche = parametresRecherche.AccesRecherche;
            //resultData.CritereParam = parametresRecherche.CritereParam;
            //resultData.TypeDate = parametresRecherche.TypeDateRecherche;
            return resultData;
        }
    }
}
