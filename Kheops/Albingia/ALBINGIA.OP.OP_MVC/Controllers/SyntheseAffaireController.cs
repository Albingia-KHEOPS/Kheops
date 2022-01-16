using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.AffaireNouvelle;
using OPServiceContract;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class SyntheseAffaireController : BaseController
    {

        public ActionResult Index(string codeAffaire, string version, string type, string numeroAvenant)
        {
            int? avn = int.TryParse(numeroAvenant, out int i) ? (i >= 0 ? i : default(int?)) : default;
            AffaireId affaireId = new AffaireId
            {
                CodeAffaire = codeAffaire,
                NumeroAliment = int.Parse(version),
                IsHisto = avn.HasValue,
                NumeroAvenant = avn,
                TypeAffaire = type.ParseCode<AffaireType>()
            };
            return View(BuildModel(affaireId));
        }

        [HttpPost]
        public PartialViewResult Popup(AffaireId affaireId)
        {
            return PartialView("BlocsSynthese", BuildModel(affaireId));
        }


        [HttpPost]
        public PartialViewResult PopupS(AffaireId affaireId)
        {
            return PartialView("Index", BuildModel(affaireId));
        }
        [ErrorHandler]
        public ActionResult getContrat(string codeOffre, string version)
        {
            ModelSyntheseAffaire toReturn = new ModelSyntheseAffaire();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                var result = screenClient.LstContrats(codeOffre, version);
                toReturn.getLstContrat = result;
                return PartialView("BoitesDialogueDetails/BoiteContrats", toReturn);
            }
        }

        public static  ModelSyntheseAffaire BuildModel(AffaireId affaireId)
        {
            AffaireDto affaireDto = null;
            DelegationDto dtoDelegation = null;
            List<CreationAffaireNouvelleContratDto> lstContratDto = new List<CreationAffaireNouvelleContratDto>();
            Affaire a = new Affaire();
            List<string> lstContrat = new List<string>();

            var alertesAvenant = new List<AvenantAlerteDto>();
            using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                affaireDto = client.Channel.GetAffaire(affaireId);
            }
            if (affaireDto.NumeroAvenant > 0) {
                affaireId.NumeroAvenant = affaireDto.NumeroAvenant;
                using (var client = ServiceClientFactory.GetClient<ICommonAffaire>()) {
                    alertesAvenant = client.Channel.GetListAlertesAvenant(affaireId);
                }
            }
            
            if (affaireDto != null) {
                using (var client = ServiceClientFactory.GetClient<IPoliceServices>()) {
                    dtoDelegation = client.Channel.ObtenirDelegation(affaireDto.CourtierApporteur.Code);
                }
                if (affaireDto.TypeAffaire == AffaireType.Offre)
                {
                    using (var client = ServiceClientFactory.GetClient<IPoliceServices>())
                    {
                        lstContratDto = client.Channel.LstContrats(affaireDto.CodeAffaire, affaireDto.NumeroAliment.ToString());
                    
                    }
                }
            }
            string textTypeAffaire = affaireId.TypeAffaire == AffaireType.Contrat ? "du contrat" : "de l'offre";
            return new ModelSyntheseAffaire
            {
                Affaire = affaireDto,
                AlertesAvenant = CreationAvenantController.GetInfoAlertes(new AvenantDto { Alertes = alertesAvenant }),
                PageTitle = $"Synthèse {textTypeAffaire} {affaireId.CodeAffaire} - {affaireId.NumeroAliment}",
                CodePolicePage = affaireId.CodeAffaire.Trim(),
                VersionPolicePage = affaireId.NumeroAliment.ToString(),
                TypePolicePage = affaireId.TypeAffaire.AsCode(),
                AfficherBandeau = true,
                NumAvenantPage = affaireId.NumeroAvenant?.ToString(),
                Delegation = dtoDelegation?.Nom ?? string.Empty,
                Inspecteur = dtoDelegation?.Inspecteur?.Nom ?? string.Empty,
                Secteur = dtoDelegation?.Secteur is null ? string.Empty : $"{dtoDelegation.Secteur} - {dtoDelegation.LibSecteur}",
                getLstContrat = lstContratDto,
            };
        }

    }
}