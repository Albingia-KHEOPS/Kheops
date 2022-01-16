using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.AutoComplete;
using ALBINGIA.OP.OP_MVC.Models.ModelesContextMenu;
using ALBINGIA.OP.OP_MVC.Models.ModelesInventaire;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamFiltres;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamInventaire;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamTemplates;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamTypesValeur;
using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;
using OP.WSAS400.DTO.GestionIntervenants;
using OP.WSAS400.DTO.Inventaires;
using OP.WSAS400.DTO.Offres.Aperiteur;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.ParametreGaranties;
using OP.WSAS400.DTO.ParamIS;
using OP.WSAS400.DTO.Personnes;
using OPServiceContract.IAdministration;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.IHexavia;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class AutoCompleteController : Controller
    {
        #region Cabinet Courtage
        #region Méthodes Publiques

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetCabinetCourtageByCode(string codeString)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetCabinetCourtageByCodeImplementation(codeString)
            };
            return toReturn;
        }
        public JsonResult GetCabinetsCourtagesByName(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetCabinetsCourtagesByNameImplementation(term, 1, 20, "ASC", 2)
            };
            return toReturn;
        }
        
        [ErrorHandler]
        public JsonResult GetInterlocuteurs(string nomInterlocuteur, string codeCabinetCourtage)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            List<ModeleCommonCabinetPreneur> lstInterlocteurs = new List<ModeleCommonCabinetPreneur>();

            if (!string.IsNullOrEmpty(nomInterlocuteur))
            {
                InterlocuteurGetQueryDto query = new InterlocuteurGetQueryDto { Nom = nomInterlocuteur };
                int codeCabinetCourtageInt = int.MinValue;
                if (!String.IsNullOrWhiteSpace(codeCabinetCourtage))
                {
                    int.TryParse(codeCabinetCourtage, out codeCabinetCourtageInt);
                }
                if (codeCabinetCourtageInt != int.MinValue)
                {
                    query.CodeCabinetCourtage = codeCabinetCourtageInt;
                }
                InterlocuteurGetResultDto interlocuteur = interlocuteurGet(query);
                if (interlocuteur.Interlocuteurs.Any())
                {
                    interlocuteur.Interlocuteurs.ForEach(interlo => lstInterlocteurs.Add((ModeleCommonCabinetPreneur)interlo));
                }

            }
            if (lstInterlocteurs.Count == 0)
                lstInterlocteurs.Add(new ModeleCommonCabinetPreneur() { NomInterlocuteur = string.Empty });
            toReturn.Data = lstInterlocteurs;
            return toReturn;
        }

        #endregion
        #region Méthodes Privées
        private ModeleCommonCabinetPreneur GetCabinetCourtageByCodeImplementation(string code)
        {
            ModeleCommonCabinetPreneur toReturn = new ModeleCommonCabinetPreneur() { IdInterlocuteur = string.Empty, NomInterlocuteur = string.Empty };
            int result;
            if (int.TryParse(code, out result))
            {
                CabinetCourtageGetResultDto cabinetCourtage = CabinetCourtageGet(new CabinetCourtageGetQueryDto { Code = result });
                if (cabinetCourtage.CabinetCourtages.Any())
                    toReturn = (ModeleCommonCabinetPreneur)cabinetCourtage.CabinetCourtages.FirstOrDefault();
            }
            return toReturn;
        }
        private List<ModeleCommonCabinetPreneur> GetCabinetsCourtagesByNameImplementation(string name, int debutPagination, int finPagination, string order, int by)
        {
            List<ModeleCommonCabinetPreneur> toReturn = new List<ModeleCommonCabinetPreneur>();
            if (!string.IsNullOrEmpty(name))
            {
                CabinetCourtageGetResultDto cabinetCourtage =
                    CabinetCourtageGet(new CabinetCourtageGetQueryDto { NomCabinet = name, DebutPagination = debutPagination, FinPagination = finPagination, Order = order, By = by });

                if (cabinetCourtage.CabinetCourtages.Any())
                    cabinetCourtage.CabinetCourtages.ForEach(cabinet => toReturn.Add((ModeleCommonCabinetPreneur)cabinet));
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleCommonCabinetPreneur { Nom = string.Empty });
            return toReturn;
        }
        private CabinetCourtageGetResultDto CabinetCourtageGet(CabinetCourtageGetQueryDto query)
        {
            CabinetCourtageGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.CabinetCourtageGet(query, true);
            }
            return toReturn;
        }

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private CabinetCourtage_JSON_MetaData DtoToJsonAvecAdresseEtNomsSecondaires(CabinetCourtageDto cabinetCourtageDto)
        //{
        //    CabinetCourtage_JSON_MetaData toReturn = DtoToJson(cabinetCourtageDto);
        //    toReturn.NomSecondaires = cabinetCourtageDto.NomSecondaires.ToArray();
        //    if (cabinetCourtageDto.Adresse != null && cabinetCourtageDto.Adresse.Ville != null)
        //    {
        //        toReturn.CodePostal = cabinetCourtageDto.Adresse.Ville.CodePostal;
        //        toReturn.Ville = cabinetCourtageDto.Adresse.Ville.Nom;
        //    }
        //    return toReturn;
        //}

        // // [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //private CabinetCourtage_JSON_MetaData DtoToJson(CabinetCourtageDto cabinetCourtageDto)
        //{
        //    return new CabinetCourtage_JSON_MetaData { Code = cabinetCourtageDto.Code, Nom = cabinetCourtageDto.NomCabinet, Type = cabinetCourtageDto.Type, Delegation = cabinetCourtageDto.Delegation.Nom, EstValide = cabinetCourtageDto.EstValide, ValideInterlocuteur = cabinetCourtageDto.ValideInterlocuteur };
        //}
        private InterlocuteurGetResultDto interlocuteurGet(InterlocuteurGetQueryDto query)
        {
            InterlocuteurGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.InterlocuteursGet(query);
            }
            return toReturn;
        }

        #endregion
        #endregion
        #region Preneur d'assurance
        #region Méthodes Publiques

        [ErrorHandler]
        public JsonResult GetAssureByName(string term)
        {
            var toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetAssureByNameImplementation(term)
            };
            return toReturn;
        }
        
        [ErrorHandler]
        public JsonResult GetPreneurAssuranceByCode(string codeString)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetPreneursAssuranceByCodeImplementation(codeString)
            };
            return toReturn;
        }
        public ModeleCommonCabinetPreneur GetPreneursAssuranceByCodeImplementation(string code)
        {
            ModeleCommonCabinetPreneur toReturn = new ModeleCommonCabinetPreneur();
            int result;
            if (int.TryParse(code, out result) && code.Trim().Length <= 7)
            {
                AssureGetResultDto preneurAssurance = PreneurAssuranceGet(new AssureGetQueryDto { Code = code, DebutPagination = 1, FinPagination = 20, Order = "ASC", By = 2 });
                if (preneurAssurance.Assures.Any())
                    toReturn = (ModeleCommonCabinetPreneur)preneurAssurance.Assures.FirstOrDefault();
            }
            return toReturn;
        }

        #endregion
        #region Méthodes Privées
        private List<ModeleCommonCabinetPreneur> GetAssureByNameImplementation(string name)
        {
            var toReturn = new List<ModeleCommonCabinetPreneur>();
            if (!string.IsNullOrEmpty(name))
            {
                AssureGetResultDto preneurAssurance =
                    PreneurAssuranceGet(new AssureGetQueryDto { NomAssure = name, DebutPagination = 1, FinPagination = 20, Order = "ASC", By = 2 });

                if (preneurAssurance.Assures.Any())
                    preneurAssurance.Assures.ForEach(assure => toReturn.Add((ModeleCommonCabinetPreneur)assure));
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleCommonCabinetPreneur { Nom = string.Empty });
            return toReturn;
        }
        private AssureGetResultDto PreneurAssuranceGet(AssureGetQueryDto query)
        {
            AssureGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.AssuresGet(query, true);
            }
            return toReturn;
        }

        #endregion
        #endregion
        #region Souscripteur
        #region Méthodes Publiques
        public JsonResult GetSouscripteursByName(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetSouscripteursByNameImplementation(term.ToUpper())
            };
            return toReturn;
        }

        public JsonResult SearchSouscripteur(string term) {
            var result = GetSouscripteursByNameImplementation(term.ToUpperInvariant());
            if (result.Count == 1 && result.Single().Nom.IsEmptyOrNull()) {
                result.Clear();
            }
            return new JsonResult {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public JsonResult SearchCourtier(string term) {
            var result = GetCabinetsCourtagesByNameImplementation(term.ToUpperInvariant(), 1, 20, "ASC", 2);
            if (result.Count == 1 && result.Single().Code.IsEmptyOrNull()) {
                result.Clear();
            }
            return new JsonResult {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public JsonResult SearchCodePostal(string term) {
            if (!Regex.IsMatch(term, @"^\d{3,5}$")) {
                throw new ArgumentException(nameof(term));
            }
            var result = new List<Ville>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAdresseHexavia>()) {
                result.AddRange(client.Channel.SearchVilleByCP(int.Parse(term)).Select(x => new Ville { CodePostal = x.cp, Nom = x.nom }));
            }

            return new JsonResult {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public JsonResult SearchPreneurAssurance(string term) {
            var result = GetAssureByNameImplementation(term.ToUpperInvariant());
            if (result.Count == 1 && result.Single().Nom.IsEmptyOrNull()) {
                result.Clear();
            }
            return new JsonResult {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public JsonResult SearchGestionnaire(string term) {
            var result = GetGestionnairesByNameImplementation(term.ToUpperInvariant());
            if (result.Count == 1 && result.Single().Nom.IsEmptyOrNull()) {
                result.Clear();
            }
            return new JsonResult {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        #endregion
        #region Méthodes Privées
        private List<ModeleCommonGestionnaireSouscripteurAperiteur> GetSouscripteursByNameImplementation(string name)
        {
            var toReturn = new List<ModeleCommonGestionnaireSouscripteurAperiteur>();
            if (!string.IsNullOrEmpty(name))
            {
                SouscripteursGetResultDto souscripteurs =
                  souscripteursGet(new SouscripteursGetQueryDto { NomSouscripteur = name, DebutPagination = 1, FinPagination = 10 });

                if (souscripteurs.SouscripteursDto.Any())
                    souscripteurs.SouscripteursDto.ForEach(souscripteur => toReturn.Add((ModeleCommonGestionnaireSouscripteurAperiteur)souscripteur));
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleCommonGestionnaireSouscripteurAperiteur { Nom = string.Empty });
            return toReturn;
        }
        private SouscripteursGetResultDto souscripteursGet(SouscripteursGetQueryDto query)
        {
            SouscripteursGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.SouscripteursGet(query);
            }
            return toReturn;
        }

        #endregion
        #endregion
        #region Gestionnaire
        #region Méthodes Publiques
        public JsonResult GetGestionnairesByName(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetGestionnairesByNameImplementation(term.ToUpper())
            };
            return toReturn;
        }

        #endregion
        #region Méthodes Privées
        private List<ModeleCommonGestionnaireSouscripteurAperiteur> GetGestionnairesByNameImplementation(string name)
        {
            var toReturn = new List<ModeleCommonGestionnaireSouscripteurAperiteur>();
            if (!string.IsNullOrEmpty(name))
            {
                GestionnairesGetResultDto gestionnaires =
                    GestionnairesGet(new GestionnairesGetQueryDto { NomGestionnaire = name, DebutPagination = 1, FinPagination = 10 });

                if (gestionnaires.GestionnairesDto.Any())
                    gestionnaires.GestionnairesDto.ForEach(gestionnaire => toReturn.Add((ModeleCommonGestionnaireSouscripteurAperiteur)gestionnaire));
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleCommonGestionnaireSouscripteurAperiteur { Nom = string.Empty });
            return toReturn;
        }
        private GestionnairesGetResultDto GestionnairesGet(GestionnairesGetQueryDto query)
        {
            GestionnairesGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.GestionnairesGet(query);
            }
            return toReturn;
        }

        #endregion
        #endregion
        #region Apériteur
        #region Méthodes Publiques
        public JsonResult GetAperiteursByName(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetAperiteursByNameImplementation(term.ToUpper())
            };
            return toReturn;
        }
        
        [ErrorHandler]
        public JsonResult GetAperiteursByCode(string codeString)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetAperiteursByCodeImplementation(codeString.ToUpper())
            };
            return toReturn;
        }

        [ErrorHandler]
        public JsonResult GetAperiteursByCodeNum(string codeString)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetAperiteursByCodeNumImplementation(codeString)
            };
            return toReturn;
        }
        
        [ErrorHandler]
        public JsonResult GetInterlocuteursAperiteur(string nomInterlocuteur, string codeAperiteur)
        {
            JsonResult toReturn = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //InterlocuteurGetQueryDto query = new InterlocuteurGetQueryDto { Nom = nomInterlocuteur };
            List<ModeleCommonCabinetPreneur> cabinetCourtage = new List<ModeleCommonCabinetPreneur>();

            InterlocuteurGetResultDto result = interlocuteurApeGet(nomInterlocuteur, codeAperiteur);
            foreach (InterlocuteurDto interlocuteurDto in result.Interlocuteurs)
            {
                ModeleCommonCabinetPreneur cabinetCourtageJsonMetaData =
                    new ModeleCommonCabinetPreneur
                    {
                        NomInterlocuteur = interlocuteurDto.Nom,
                        IdInterlocuteur = interlocuteurDto.Id.ToString()
                    };
                cabinetCourtage.Add(cabinetCourtageJsonMetaData);
            }

            if (cabinetCourtage.Count == 0)
                cabinetCourtage.Add(new ModeleCommonCabinetPreneur() { NomInterlocuteur = string.Empty });

            toReturn.Data = cabinetCourtage;
            return toReturn;
        }

        #endregion
        #region Méthodes Privées
        private List<ModeleCommonGestionnaireSouscripteurAperiteur> GetAperiteursByNameImplementation(string name)
        {
            var toReturn = new List<ModeleCommonGestionnaireSouscripteurAperiteur>();
            if (!string.IsNullOrEmpty(name))
            {
                AperiteurGetResultDto aperiteurs =
                    AperiteursGet(new AperiteurGetQueryDto { Nom = name, DebutPagination = 1, FinPagination = 30 });

                if (aperiteurs.AperiteursDto.Any())
                    aperiteurs.AperiteursDto.ForEach(aperiteur => toReturn.Add((ModeleCommonGestionnaireSouscripteurAperiteur)aperiteur));
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleCommonGestionnaireSouscripteurAperiteur { Nom = string.Empty });
            return toReturn;
        }
        private AperiteurGetResultDto AperiteursGet(AperiteurGetQueryDto query)
        {
            AperiteurGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.AperiteursGet(query);
            }
            return toReturn;
        }
        
        [ErrorHandler]
        private ModeleCommonGestionnaireSouscripteurAperiteur GetAperiteursByCodeImplementation(string code)
        {
            ModeleCommonGestionnaireSouscripteurAperiteur toReturn = new ModeleCommonGestionnaireSouscripteurAperiteur();
            if (!string.IsNullOrEmpty(code))
            {
                AperiteurGetResultDto aperiteurs =
                    AperiteursGet(new AperiteurGetQueryDto { Code = code });
                if (aperiteurs.AperiteursDto.Any())
                    toReturn = (ModeleCommonGestionnaireSouscripteurAperiteur)aperiteurs.AperiteursDto.FirstOrDefault();
            }
            return toReturn;
        }

        [ErrorHandler]
        private ModeleCommonGestionnaireSouscripteurAperiteur GetAperiteursByCodeNumImplementation(string code)
        {
            ModeleCommonGestionnaireSouscripteurAperiteur toReturn = new ModeleCommonGestionnaireSouscripteurAperiteur();
            Int64 iCode;
            if (!string.IsNullOrEmpty(code) && Int64.TryParse(code, out iCode))
            {
                AperiteurGetResultDto aperiteurs;

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var screenClient = client.Channel;
                    aperiteurs = screenClient.AperiteursGetByCodeNum(iCode);
                }
                if (aperiteurs != null && aperiteurs.AperiteursDto.Any())
                    toReturn = (ModeleCommonGestionnaireSouscripteurAperiteur)aperiteurs.AperiteursDto.FirstOrDefault();
            }
            return toReturn;
        }
        
        [ErrorHandler]
        private InterlocuteurGetResultDto interlocuteurApeGet(string nomInterlocuteur, string codeAperiteur)
        {
            InterlocuteurGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.InterlocuteursAperiteurGet(nomInterlocuteur, codeAperiteur);
            }
            return toReturn;
        }

        #endregion
        #endregion
        #region Partenaire
        #region Méthodes Publiques

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
        //                toReturn.Data = GetPreneursAssuranceByCodeImplementation(code.ToString());
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


        #endregion
        #region Méthodes Privées



        #endregion
        #endregion
        #region OrganismesOpp

        #region Méthodes Publiques
            
        [ErrorHandler]
        public JsonResult GetOrganismesByName(string term, string typeOppBenef)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetOrganismesByNameImplementation(term.ToUpper(), typeOppBenef)
            };
            return toReturn;
        }
        public JsonResult GetOrganismesByCode(string term, string typeOppBenef)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetOrganismeByCodeImplementation(term, typeOppBenef)
            };
            return toReturn;
        }

        #endregion
        #region Méthodes Privées
        private List<ModeleOrganisme> GetOrganismesByNameImplementation(string value, string typeOppBenef)
        {
            var toReturn = new List<ModeleOrganisme>();
            if (!string.IsNullOrEmpty(value))
            {
                var listOrganisme = OrganismesGet(value, "Name", typeOppBenef);
                if (listOrganisme.Any())
                    listOrganisme.ForEach(organisme => toReturn.Add((ModeleOrganisme)organisme));
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleOrganisme { Nom = string.Empty });
            return toReturn;
        }
        private ModeleOrganisme GetOrganismeByCodeImplementation(string value, string typeOppBenef)
        {
            var toReturn = new ModeleOrganisme();
            if (string.IsNullOrEmpty(value)) return toReturn;
            var listOrganisme = OrganismesGet(value, "Code", typeOppBenef);
            if (listOrganisme.Any())
                toReturn = (ModeleOrganisme)listOrganisme.FirstOrDefault();
            return toReturn;
        }
        private List<OrganismeOppDto> OrganismesGet(string value, string mode, string typeOppBenef)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var contexte = client.Channel;
                return contexte.OrganismesGet(value, mode, typeOppBenef);
            }
        }

        #endregion
        #endregion
        #region CP/Ville
        #region Méthodes Publiques
        public JsonResult GetCodePostal(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new List<ModeleInventaireCPVille_JSON>()
            };

            if (!String.IsNullOrEmpty(term))
            {
                toReturn.Data = GetCodePostalVilleImplementation(term.ToUpper(), "CP");
            }

            return toReturn;
        }
        public JsonResult GetVille(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new List<ModeleInventaireCPVille_JSON>()
            };

            if (!String.IsNullOrEmpty(term))
            {
                toReturn.Data = GetCodePostalVilleImplementation(term.ToUpper(), "VILLE");
            }

            return toReturn;
        }
        [ErrorHandler]
        public ActionResult GetVillesByCP(string codePostal)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAdresseHexavia>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetVillesByCP(codePostal);

                List<AlbSelectListItem> lstVille = result != null ? result.Select(m => new AlbSelectListItem { Value = m.Libelle, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList() : new List<AlbSelectListItem>();
                ModeleInventaireListeCPVille model = new ModeleInventaireListeCPVille
                {
                    Type = "Ville",
                    Listes = lstVille
                };
                return PartialView("/Views/RisqueInventaire/InventaireListeCPVille.cshtml", model);
            }
        }

        [ErrorHandler]
        public ActionResult GetCPByVille(string ville)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAdresseHexavia>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetCPByVille(ville);

                List<AlbSelectListItem> lstCP = result != null ? result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList() : new List<AlbSelectListItem>();

                ModeleInventaireListeCPVille model = new ModeleInventaireListeCPVille
                {
                    Type = "CP",
                    Listes = lstCP
                };
                return PartialView("/Views/RisqueInventaire/InventaireListeCPVille.cshtml", model);
            }
        }
        #endregion
        #region Méthodes Privées
        private List<ModeleInventaireCPVille_JSON> GetCodePostalVilleImplementation(string value, string mode)
        {
            var listOrganisme = CodePostalVilleGet(value, mode);
            var toReturn = listOrganisme.Select(item => (ModeleInventaireCPVille_JSON)item).ToList();
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleInventaireCPVille_JSON { CodePostal = string.Empty, Ville = string.Empty });
            return toReturn;
        }
        private List<CPVilleDto> CodePostalVilleGet(string value, string mode)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAdresseHexavia>())
            {
                var contexte = client.Channel;
                return contexte.GetCodePostalVille(value, mode);
            }
        }
        #endregion
        #endregion
        #region Famille
        #region Méthodes publiques
        public JsonResult GetFamillesByCode(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetFamillesByCodeImplementation(term, "Code")
            };
            return toReturn;
        }
        public JsonResult GetFamillesByName(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetFamillesByCodeImplementation(term, "Name")
            };
            return toReturn;
        }
        #endregion
        #region Méthodes privées
        private List<ModeleCommonConceptFamilleCode> GetFamillesByCodeImplementation(string value, string mode)
        {
            var toReturn = new List<ModeleCommonConceptFamilleCode>();
            if (!string.IsNullOrEmpty(value))
            {
                var listFamilles = FamillesGet(value, mode);
                if (listFamilles.Any())
                    listFamilles.ForEach(famille => toReturn.Add((ModeleCommonConceptFamilleCode)famille));
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleCommonConceptFamilleCode { Libelle = string.Empty });
            return toReturn;
        }
        private List<ParametreDto> FamillesGet(string value, string mode)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient = client.Channel;
                return voletsBlocsCategoriesClient.FamillesGet(value, mode, string.Empty, string.Empty);
            }
        }
        #endregion
        #endregion
        #region Concept
        #region Méthodes publiques
        public JsonResult GetConceptByCode(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetConceptByCodeImplementation(term.ToUpper())
            };
            return toReturn;
        }
        public JsonResult GetConceptByLib(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetConceptByLibImplementation(term.ToUpper())
            };
            return toReturn;
        }
        #endregion
        #region Méthodes privées
        private List<ParametreDto> GetConceptByCodeImplementation(string term)
        {
            //string paramFam = "KHEOP";
            List<ParametreDto> toReturn = new List<ParametreDto>();
            //var masterRole = Common.CacheUserRights.UserRights.Exists(el => el.TypeDroit == TypeDroit.M.ToString());

            //if ((paramFam.StartsWith(term) && !masterRole) || (masterRole))
            //{
            if (!string.IsNullOrEmpty(term))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    toReturn = serviceContext.LoadListConcepts(term, string.Empty, true, true);
                    //toReturn = serviceContext.LoadListConcepts(term, string.Empty, true, masterRole);
                }
            }
            //}
            if (toReturn.Count == 0)
                toReturn.Add(new ParametreDto { Libelle = string.Empty });
            return toReturn;
        }

        private List<ParametreDto> GetConceptByLibImplementation(string term)
        {
            List<ParametreDto> toReturn = new List<ParametreDto>();
            if (!string.IsNullOrEmpty(term))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    toReturn = serviceContext.LoadListConcepts(string.Empty, term, true, true);
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ParametreDto { Libelle = string.Empty });
            return toReturn;
        }
        #endregion


        #endregion
        #region Inventaire
        #region Méthodes publiques
        public JsonResult GetInventaireByCode(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetInventaireByCodeImplementation(term.ToUpper())
            };
            return toReturn;
        }

        #endregion
        #region Méthodes privées
        private List<LigneInventaires> GetInventaireByCodeImplementation(string term)
        {
            List<LigneInventaires> toReturn = new List<LigneInventaires>();
            var masterRole = Common.CacheUserRights.UserRights.Any(el => el.TypeDroit == TypeDroit.M.ToString());

            if (!string.IsNullOrEmpty(term))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    //var result = serviceContext.LoadListInventaireAutoComp(term, string.Empty, true, masterRole);
                    var result = serviceContext.LoadInventaire(term, string.Empty, masterRole);
                    if (result != null && result.Any())
                    {
                        foreach (var item in result)
                        {
                            toReturn.Add((LigneInventaires)item);
                        }
                    }
                }
            }

            if (toReturn.Count == 0)
                toReturn.Add(new LigneInventaires { Libelle = string.Empty });
            return toReturn;
        }
        #endregion
        #endregion
        #region Filtre
        #region Méthodes publiques
        public JsonResult GetFiltresByCode(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetFiltreImplementation(term, string.Empty)
            };
            return toReturn;
        }
        public JsonResult GetFiltresByName(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetFiltreImplementation(string.Empty, term)
            };
            return toReturn;
        }

        #endregion
        #region Méthodes privées

        private List<ModeleLigneFiltre> GetFiltreImplementation(string termCode, string termName)
        {
            List<ModeleLigneFiltre> toReturn = new List<ModeleLigneFiltre>();
            if (!string.IsNullOrEmpty(termCode) || !string.IsNullOrEmpty(termName))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    var result = serviceContext.LoadListFiltres(termCode, termName, string.Empty);
                    if (result.Any())
                        result.ForEach(elm => toReturn.Add((ModeleLigneFiltre)elm));
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleLigneFiltre { DescriptionFiltre = string.Empty });
            return toReturn;
        }

        #endregion
        #endregion
        #region Type valeur


        #region Méthodes publiques
        public JsonResult GetTypeValeurByCode(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetTypeValeurImplementation(term, string.Empty)
            };
            return toReturn;
        }
        public JsonResult GetTypeValeurByName(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetTypeValeurImplementation(string.Empty, term)
            };
            return toReturn;
        }

        #endregion
        #region Méthodes privées

        private List<ModeleLigneTypeValeur> GetTypeValeurImplementation(string termCode, string termName)
        {
            List<ModeleLigneTypeValeur> toReturn = new List<ModeleLigneTypeValeur>();
            if (!string.IsNullOrEmpty(termCode) || !string.IsNullOrEmpty(termName))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    var result = serviceContext.LoadListTypesValeur(termCode, termName);
                    if (result.Any())
                        result.ForEach(elm => toReturn.Add((ModeleLigneTypeValeur)elm));
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleLigneTypeValeur { Description = string.Empty });
            return toReturn;
        }

        #endregion

        #endregion
        #region Garantie
        #region Méthodes publiques
        public JsonResult GetGarantieByCode(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetGarantieByCodeImplementation(term.ToUpper())
            };
            return toReturn;
        }
        public JsonResult GetGarantieByDescription(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetGarantieByDescriptionImplementation(term.ToUpper())
            };
            return toReturn;
        }
        #endregion
        #region Méthodes privées
        private List<ParamGarantieDto> GetGarantieByCodeImplementation(string term)
        {
            List<ParamGarantieDto> toReturn = new List<ParamGarantieDto>();
            if (!string.IsNullOrEmpty(term))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    toReturn = serviceContext.GetGaranties(term, string.Empty, string.Empty, true);
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ParamGarantieDto { DesignationGarantie = string.Empty });
            return toReturn;
        }

        private List<ParamGarantieDto> GetGarantieByDescriptionImplementation(string term)
        {
            List<ParamGarantieDto> toReturn = new List<ParamGarantieDto>();
            if (!string.IsNullOrEmpty(term))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    toReturn = serviceContext.GetGaranties(string.Empty, term, string.Empty, true);
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ParamGarantieDto { DesignationGarantie = string.Empty });
            return toReturn;
        }
        #endregion


        #endregion
        #region Referentiels IS
        #region Méthodes publiques
        public JsonResult GetReferentielISByName(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetReferentielISImplementation(term)
            };
            return toReturn;
        }

        #endregion
        #region Méthodes privées

        private List<LigneModeleISDto> GetReferentielISImplementation(string termName)
        {
            List<LigneModeleISDto> toReturn = new List<LigneModeleISDto>();
            if (!string.IsNullOrEmpty(termName))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    toReturn = serviceContext.GetISReferenciel(termName, true);
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new LigneModeleISDto { Code = string.Empty });
            return toReturn;
        }

        #endregion


        #endregion
        #region Templates
        #region Méthodes publiques
        public JsonResult GetTemplateByCodeCNVA(string term, string type = "")
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetTemplateImplementationCNVA(string.Concat("CV", term), string.Empty, type)
            };
            return toReturn;
        }
        public JsonResult GetTemplateByCode(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetTemplateImplementation(term, string.Empty)
            };
            return toReturn;
        }
        
        [ErrorHandler]
        public JsonResult GetTemplateByName(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetTemplateImplementation(string.Empty, term)
            };
            return toReturn;
        }
        
        [ErrorHandler]
        public JsonResult GetTemplateByCodeCible(string term, string cible)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetTemplateImplementation(term, string.Empty, cible)
            };
            return toReturn;
        }
        
        [ErrorHandler]
        public JsonResult GetTemplateByNameCible(string term, string cible)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetTemplateImplementation(string.Empty, term, cible)
            };
            return toReturn;
        }

        #endregion
        #region Méthodes privées

        private List<ModeleLigneTemplate> GetTemplateImplementation(string termCode, string termName, string codeCible = "")
        {
            List<ModeleLigneTemplate> toReturn = new List<ModeleLigneTemplate>();
            if (!string.IsNullOrEmpty(termCode) || !string.IsNullOrEmpty(termName))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    var result = serviceContext.LoadListTemplates(0, termCode, termName, string.Empty, codeCible, string.Empty, true, false);
                    if (result.Any())
                        result.ForEach(elm => toReturn.Add((ModeleLigneTemplate)elm));
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleLigneTemplate { DescriptionTemplate = string.Empty });
            return toReturn;
        }

        private List<ModeleLigneTemplate> GetTemplateImplementationCNVA(string termCode, string termName, string type = "")
        {
            List<ModeleLigneTemplate> toReturn = new List<ModeleLigneTemplate>();
            if (!string.IsNullOrEmpty(termCode) || !string.IsNullOrEmpty(termName))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = client.Channel;
                    var result = serviceContext.LoadListTemplatesCNVA(termCode, type, true, true);
                    if (result.Any())
                        result.ForEach(elm => toReturn.Add((ModeleLigneTemplate)elm));
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleLigneTemplate { DescriptionTemplate = string.Empty });
            return toReturn;
        }

        #endregion
        #endregion
        #region Menu actions
        #region Méthodes publiques
        
        [ErrorHandler]
        public JsonResult GetMenuActionByName(string term, string type, string etat, string situation, string periodicite, string branche, string copyOffre, string modeNavig)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                //Data = GetMenuActionImplementation(term, type)
            };
            if (ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
                toReturn.Data = GetMenuActionImplementation(term, type, branche, situation, etat);
            if (ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>())
                toReturn.Data = GetMenuActionHistoriqueImplementation(term, type);
            return toReturn;
        }

        #endregion
        #region Méthodes privées
        private List<ModeleListItem> GetMenuActionHistoriqueImplementation(string termName, string type)
        {
            List<ModeleListItem> toReturn = new List<ModeleListItem>();
            if (!string.IsNullOrEmpty(termName) && !string.IsNullOrEmpty(type))
            {
                var result = MvcApplication.AlbAllFlatContextMenuUsers != null
                   ? MvcApplication.AlbAllFlatContextMenuUsers.FindAll(
                   ctxMenu => ctxMenu.Utilisateur.ToLower() == AlbSessionHelper.ConnectedUser.ToLower() &&
                       ctxMenu.text == "Consulter").ToList()
                   : null;
                if (result != null)
                    result.FindAll(elm => elm.text.ToLower().Contains(termName.ToLower())).ForEach(menu => toReturn.Add(menu));
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleListItem { text = string.Empty });
            return toReturn;
        }

        private List<ModeleListItem> GetMenuActionImplementation(string termName, string type, string brche, string sit, string etat, string typeAccord = "")
        {
            var toReturn = new List<ModeleListItem>();
            if (!string.IsNullOrEmpty(termName) && !string.IsNullOrEmpty(type))
            {
                switch (type)
                {
                    case AlbConstantesMetiers.TYPE_OFFRE:
                        type = AlbConstantesMetiers.TYPE_MENU_OFFRE; break;
                    case AlbConstantesMetiers.TYPE_CONTRAT:
                        type = AlbConstantesMetiers.TYPE_MENU_CONTRAT; break;
                }

                var result = MvcApplication.AlbAllFlatContextMenuUsers != null
               ? MvcApplication.AlbAllFlatContextMenuUsers.FindAll(
                 ctxMenu => ctxMenu.Utilisateur.ToLower() == AlbSessionHelper.ConnectedUser.ToLower() &&
                 (ctxMenu.typeOffreContrat == type || ctxMenu.typeOffreContrat == "*"))
               : null;
                if (result != null)
                {
                    // result.FindAll(elm => elm.text.ToUpper().Contains(termName.ToUpper())).ForEach(menu => toReturn.Add(menu));
                    foreach (ModeleListItem menu in result.FindAll(elm => elm.text.ToUpper().Contains(termName.ToUpper())))
                    {
                        var alwMenu = !(AlbContextMenu.REPRISE.DisplayName() == menu.text && (sit == "X" || sit == "W" || sit == "N") && (typeAccord != "" || typeAccord != "N"));
                        if (AlbContextMenu.OPMODIFIER.DisplayName() == menu.text &&
                             (sit != "X" && sit != "A" && sit != string.Empty))
                            alwMenu = false;
                        var alwType = type == AlbConstantesMetiers.TYPE_MENU_POLICE ? AlbConstantesMetiers.TYPE_MENU_CONTRAT : type;

                        var alwEtat = !string.IsNullOrEmpty(menu.AlwEtat) ? menu.AlwEtat.PadRight(10, ' ') : string.Empty;
                        alwEtat = !string.IsNullOrEmpty(menu.AlwEtat) ?
                                            alwType == AlbConstantesMetiers.TYPE_MENU_OFFRE ? alwEtat.Substring(0, 5) :
                                            alwType == AlbConstantesMetiers.TYPE_MENU_CONTRAT ? alwEtat.Substring(5, 5) : alwEtat :
                                            string.Empty;

                        var alwBrche = !string.IsNullOrEmpty(menu.AlwBranche) ? menu.AlwBranche.PadRight(10, ' ') : string.Empty;
                        brche = brche != "CO" ? "HCO" : "CO";

                        if (alwMenu && (((!string.IsNullOrEmpty(menu.typeOffreContrat)) &&
                            (menu.typeOffreContrat == alwType || menu.typeOffreContrat == "*") &&
                            (!string.IsNullOrEmpty(alwEtat)) &&
                            (alwEtat.Contains(etat) || alwEtat.Trim() == "*" || alwEtat.Trim() == "*    *") &&
                            (!string.IsNullOrEmpty(alwBrche)) &&
                            (alwBrche.Trim() == "*" || alwBrche.Trim() == "*    *" || alwBrche == brche))))
                        {
                            toReturn.Add(menu);
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(termName) && string.IsNullOrEmpty(type))
            {
                var result = MvcApplication.AlbAllFlatContextMenuUsers != null
                  ? MvcApplication.AlbAllFlatContextMenuUsers.FindAll(
                    ctxMenu => ctxMenu.Utilisateur.ToLower() == AlbSessionHelper.ConnectedUser.ToLower())
                  : null;
                if (result != null)
                {
                    result.Where(elm => elm.text.ToUpper().Contains(termName.ToUpper()) && elm.menu == AlbContextMenu.CREER)
                        .ToList()
                        .ForEach(menu => toReturn.Add(menu));
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleListItem { text = string.Empty });
            return toReturn;
        }
        #endregion
        #endregion
        #region Intervenants

        #region Méthodes publiques
        
        [ErrorHandler]
        public JsonResult GetIntervenantByCode(string codeString, string fromAffaireOnly, string codeOffre, string type, string version)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetIntervenantByCodeImplementation(codeOffre, type, version, codeString, fromAffaireOnly)
            };
            return toReturn;
        }
        public JsonResult GetIntervenantByName(string term, string codeDossier, string typeDossier, string versionDossier, string typeIntervenant, string fromAffaireOnly)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetIntervenantImplementation(term, codeDossier, typeDossier, versionDossier, typeIntervenant, fromAffaireOnly)
            };
            return toReturn;
        }
        public JsonResult GetInterlocuteursByIntervenantByName(string term, Int64 codeIntervenant)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetInterlocuteurByIntervenantImplementation(term, codeIntervenant)
            };
            return toReturn;
        }

        #endregion
        #region Méthodes privées

        private List<IntervenantDto> GetIntervenantImplementation(string termName, string codeDossier, string typeDossier, string versionDossier, string typeIntervenant, string fromAffaireOnly)
        {
            List<IntervenantDto> toReturn = new List<IntervenantDto>();
            if (!string.IsNullOrEmpty(termName))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var serviceContext = client.Channel;
                    toReturn = serviceContext.GetListeIntervenantsAutocomplete(codeDossier, versionDossier, typeDossier, termName, typeIntervenant, string.Empty, !string.IsNullOrEmpty(fromAffaireOnly));
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new IntervenantDto { CodeIntervenant = -1 });
            return toReturn;
        }

        private IntervenantDto GetIntervenantByCodeImplementation(string codeOffre, string type, string version, string termCode, string fromAffaireOnly)
        {
            IntervenantDto toReturn = new IntervenantDto();
            int result;
            if (!string.IsNullOrEmpty(termCode) && int.TryParse(termCode, out result))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var serviceContext = client.Channel;
                    toReturn = serviceContext.GetIntervenantByCodeAutocomplete(codeOffre, type, version, termCode, !string.IsNullOrEmpty(fromAffaireOnly));
                }
            }
            return toReturn;
        }

        private List<IntervenantDto> GetInterlocuteurByIntervenantImplementation(string termName, Int64 codeIntervenant)
        {
            List<IntervenantDto> toReturn = new List<IntervenantDto>();
            if (!string.IsNullOrEmpty(termName))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var serviceContext = client.Channel;
                    toReturn = serviceContext.GetListeInterlocuteursByIntervenant(codeIntervenant, termName);
                }
            }
            if (toReturn.Count == 0)
                toReturn.Add(new IntervenantDto { CodeIntervenant = -1 });
            return toReturn;
        }


        #endregion
        #endregion
        #region User
        #region Méthodes publiques
        public JsonResult GetUserByName(string term)
        {
            JsonResult toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetUserByNameImplementation(term.ToUpper())
            };
            return toReturn;
        }

        #endregion
        #region Méthodes privées
        private List<ModeleCommonGestionnaireSouscripteurAperiteur> GetUserByNameImplementation(string name)
        {
            var toReturn = new List<ModeleCommonGestionnaireSouscripteurAperiteur>();
            if (!string.IsNullOrEmpty(name))
            {
                List<UtilisateurDto> users = utilisateursGet(name);
                if (users.Any())
                    users.ForEach(utilisateur => toReturn.Add((ModeleCommonGestionnaireSouscripteurAperiteur)utilisateur));
            }
            if (toReturn.Count == 0)
                toReturn.Add(new ModeleCommonGestionnaireSouscripteurAperiteur { Code = string.Empty, Nom = string.Empty, Prenom = string.Empty });
            return toReturn;
        }
        private List<UtilisateurDto> utilisateursGet(string name)
        {
            List<UtilisateurDto> toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.UtilisateursGet(name);
            }
            return toReturn;
        }


        #endregion
        #endregion
    }
}
