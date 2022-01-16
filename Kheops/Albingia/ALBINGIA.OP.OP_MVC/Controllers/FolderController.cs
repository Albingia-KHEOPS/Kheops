using Albingia.Common;
using Albingia.Kheops.Mvc;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Mvc.Common;
using Albingia.Mvc.Controllers;
using Albingia.Kheops.DTO;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.CustomResult;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.ModelesRecherche;
using Mapster;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ALBINGIA.Framework.Common.CacheTools;
using Albingia.Kheops.OP.Application.Port.Driver;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    [ModelLessLoader]
    public class FolderController : BaseController, IModelLessController {
        public Guid TabGuid { get; set; }

        [HttpGet]
        public string GetFolderUrl(string code, int version, string type, bool readOnly, string targetController = "", string tabGuid = null) {
            return GenerateFolderUrl(code, version, type, readOnly, targetController, tabGuid);
        }

        [HttpGet]
        public JsonResult GetEmptyEnteteContrat() {
            return new JsonResult {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new {
                    CodeOffre = string.Empty,
                    Version = string.Empty,
                    Type = string.Empty,
                    DisplayTypeContrat = string.Empty,
                    Identification = string.Empty,
                    Assure = string.Empty,
                    Souscripteur = string.Empty,
                    DateDebutEffet =  string.Empty,
                    DateFinEffet = string.Empty,
                    Periodicite = string.Empty,
                    Echeance = string.Empty,
                    NatureContrat = string.Empty,
                    Courtier = string.Empty,
                    RetourPiece = string.Empty,
                    Observation = string.Empty,
                    Gestionnaire = string.Empty,
                    ContratMere = string.Empty,
                    IsLightVersion = true,
                    LblDebutEffet = string.Empty,
                    LblFinEffet = string.Empty
                }
            };
        }

        [HttpPost]
        public JsonResult GetContratBandeau(Folder contrat, bool isHisto = false) {
            AffaireDto affaire = null;
            AffaireId affaireId = contrat.Adapt<AffaireId>();
            affaireId.IsHisto = isHisto;
            using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                affaire = client.Channel.GetAffaire(contrat.Adapt<AffaireId>());
            }
            RegularisationBandeauContratDto bandeauContrat = null;
            using (var client = ServiceClientFactory.GetClient<IRegularisation>()) {
                bandeauContrat = client.Channel.GetBandeauContratInfo(contrat.CodeOffre, contrat.Version.ToString(), contrat.Type);
            }

            return new JsonResult {
                Data = new {
                    CodeOffre = affaire.CodeAffaire,
                    Version = affaire.NumeroAliment.ToString(),
                    Type = affaire.TypeAffaire.AsCode(),
                    DisplayTypeContrat = affaire.TypePolice is null ? string.Empty : $"{affaire.TypePolice.Code} - {affaire.TypePolice.LibelleLong}",
                    bandeauContrat.Identification,
                    bandeauContrat.Assure,
                    bandeauContrat.Souscripteur,
                    DateDebutEffet = (!string.IsNullOrEmpty(bandeauContrat.DateDebutEffet)) ? DateTime.Parse(bandeauContrat.DateDebutEffet).ToString("dd/MM/yyyy") : string.Empty,
                    DateFinEffet = (!string.IsNullOrEmpty(bandeauContrat.DateFinEffet)) ? DateTime.Parse(bandeauContrat.DateFinEffet).ToString("dd/MM/yyyy") : string.Empty,
                    bandeauContrat.Periodicite,
                    Echeance = (!string.IsNullOrEmpty(bandeauContrat.Echeance)) ? DateTime.Parse(bandeauContrat.Echeance).ToString("dd/MM/yyyy") : string.Empty,
                    bandeauContrat.NatureContrat,
                    bandeauContrat.Courtier,
                    bandeauContrat.RetourPiece,
                    Observation = bandeauContrat.Observation.Replace("<br>", "\r\n"),
                    bandeauContrat.Gestionnaire,
                    bandeauContrat.ContratMere,
                    IsLightVersion = true,
                    LblDebutEffet = (!string.IsNullOrEmpty(bandeauContrat.DateDebutEffet)) ? DateTime.Parse(bandeauContrat.DateDebutEffet).ToString("dd/MM/yyyy") : HttpUtility.HtmlEncode(AlbConstantesMetiers.DateVide),
                    LblFinEffet = (!string.IsNullOrEmpty(bandeauContrat.DateFinEffet)) ? DateTime.Parse(bandeauContrat.DateFinEffet).ToString("dd/MM/yyyy") : HttpUtility.HtmlEncode(AlbConstantesMetiers.DateVide),
                }
            };
        }

        [HttpGet]
        public JsonResult GetBlacklistAffaires(string code, string name, string codeInterlocuteur, TypePartenaireKheops typeBlacklist) {
            var searchModel = new ModeleParametresRecherche {
                AccesRecherche = AlbConstantesMetiers.TypeAccesRecherche.Standard,
                CheckOffre = true,
                CheckContrat = true,
                CritereParam = AlbConstantesMetiers.CriterParam.Standard,
                IsActif = true,
                IsInactif = true,
                ModeNavig = ModeConsultation.Standard.AsCode(),
                PageNumber = 1,
                TypeDateRecherche = AlbConstantesMetiers.TypeDateRecherche.MAJ
            };
            switch (typeBlacklist) {
                case TypePartenaireKheops.CourtierGestionnaire:
                case TypePartenaireKheops.Interlocuteur:
                    searchModel.CabinetCourtageIsGestionnaire = true;
                    searchModel.CabinetCourtageId = int.Parse(code);
                    break;
                case TypePartenaireKheops.CourtierApporteur:
                    searchModel.CabinetCourtageIsApporteur = true;
                    searchModel.CabinetCourtageId = int.Parse(code);
                    break;
                case TypePartenaireKheops.CourtierPayeur:
                    break;
                case TypePartenaireKheops.PreneurAssurance:
                    searchModel.PreneurAssuranceCode = int.Parse(code);
                    break;
                case TypePartenaireKheops.AssureAdditionnel:
                    break;
                case TypePartenaireKheops.CoAssureur:
                    break;
                case TypePartenaireKheops.InterlocuteurCoAssureur:
                    break;
                case TypePartenaireKheops.Intervenant:
                    break;
                case TypePartenaireKheops.InterlocuteurIntervenant:
                    break;
                case TypePartenaireKheops.CourtierAdditionnel:
                    break;
                default:
                    break;
            }

            var resultsModel = RechercheSaisieController.GetResultSearchData(ref searchModel);
            var results = resultsModel.ListResult.Select(x => new BlacklistAffaire {
                Affaire = new Models.Blacklisting.Affaire {
                    Branche = x.Branche,
                    Code = x.OffreContratNum,
                    Libelle = x.DescriptifRisque,
                    NumeroAvenant = x.NumAvenant,
                    Version = int.Parse(x.Version),
                    Type = x.Type,
                    DisplayType = x.Type == AlbConstantesMetiers.TYPE_OFFRE ? (x.HasDoubleSaisie ? "OO" : x.Type)
                        : x.ContratMere == "M" ? "M" : x.ContratMere == "A" ? "A" : x.Type
                },
                Cible = x.Cible,
                DateMAJ = (x.DateMaj?.ToShortDateString() ?? string.Empty),
                Etat = x.Etat,
                Situation = x.Situation,
                Preneur = x.PreneurAssuranceNom,
                Courtier = x.CourtierGestionnaireNom,
                Qualite = x.Qualite
            });

            return JsonNetResult.NewResultToGet(results.ToArray());
        }

        [HttpPost]
        public void Deverrouiller(string tabGuid) {
            DeverrouillerAffaire(tabGuid);
        }

        /// <summary>
        /// Deletes locks in DB and in Session.
        /// </summary>
        /// <param name="tabGuid">The guid of the user browser tab</param>
        /// <param name="affaireId">Defines a specific code if the guid is used for multiple policies. Default is NULL</param>
        internal static void DeverrouillerAffaire(string tabGuid, AffaireId affaireId = null) {
            try {
                var listeAcces = MvcApplication.ListeAccesAffaires.Where(x =>
                    x.TabGuid.ToString("N") == tabGuid.Replace(PageParamContext.TabGuidKey, string.Empty).ToLower()
                    && (affaireId is null || x.Code.ToIPB() == affaireId.CodeAffaire && x.Version == affaireId.NumeroAliment)).ToList();

                bool deleteAll = affaireId is null;
                listeAcces.ForEach(acces => {
                    if (acces.ModeAcces != AccesOrigine.Consulter) {
                        using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                            if (deleteAll) {
                                affaireId = acces.Avenant.HasValue ? new AffaireId { CodeAffaire = acces.Code, TypeAffaire = AffaireType.Contrat, IsHisto = true, NumeroAliment = acces.Version, NumeroAvenant = acces.Avenant }
                                    : client.Channel.GetCurrentAffaireId(acces.Code, acces.Version);
                            }
                            client.Channel.UnockAffaireList(new[] { affaireId });
                        }
                    }
                    MvcApplication.ListeAccesAffaires.Remove(acces);
                });
            }
            catch {
                //todo: log somehow
            }
        }

        internal static void DeverrouillerAffairesUser() {
            if (AlbSessionHelper.ConnectedUser.IsEmptyOrNull()) {
                return;
            }
            try {
                using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                    client.Channel.UnockAffaires();
                }
            }
            catch {
                //todo: log somehow
            }
        }

        internal static string GenerateFolderUrl(string code, int version = 0, string type = AlbConstantesMetiers.TYPE_CONTRAT, bool readOnly = false, string targetController = "", string tabGuid = null) {
            ContratDto contrat = null;
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var serviceContext = client.Channel;
                contrat = serviceContext.GetBasicFolder(new Folder(code, version, type[0]));
            }

            if (targetController.ContainsChars() && !Regex.IsMatch(targetController, @"^[A-Za-z]\w+$")) {
                return null;
            }

            string folderId = string.Join("_", new[] { code, version.ToString(), type });

            // validate guid format
            Guid? guid = tabGuid!=null ? Guid.TryParse(tabGuid.Replace(PageParamContext.TabGuidKey, string.Empty), out var g) ? g : default(Guid?) : default(Guid?);
            string urlGuid = $"{PageParamContext.TabGuidKey}{guid?.ToString("N")}{PageParamContext.TabGuidKey}";

            string mode = $"{AlbParameters.ModeNavigKey}{ModeConsultation.Standard.AsCode()}{AlbParameters.ModeNavigKey}";
            string avnParam;
            if (contrat.NumInterneAvenant > 0) {
                if (targetController.IsEmptyOrNull()) {
                    targetController = "AvenantInfoGenerales";
                }
                avnParam = $"{PageParamContext.ParamKey}AVN|||{AlbParameterName.AVNTYPE}|{contrat.TypeTraitement}||{AlbParameterName.AVNID}|{contrat.NumInterneAvenant}{PageParamContext.ParamKey}";
            }
            else {
                if (targetController.IsEmptyOrNull()) {
                    targetController = type[0] == AlbConstantesMetiers.TYPE_OFFRE[0] ? "ModifierOffre" : "AnInformationsGenerales";
                }
                avnParam = string.Empty;
            }

            return $"/{targetController}/Index/{folderId}{urlGuid}{avnParam}{mode}" + (readOnly ? "ConsultOnly" : string.Empty);
        }
    }
}