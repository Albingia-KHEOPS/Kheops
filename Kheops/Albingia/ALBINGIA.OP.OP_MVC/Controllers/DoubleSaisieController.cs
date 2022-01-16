using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesDoubleSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class DoubleSaisieController : ControllersBase<ModeleDoubleSaisiePage>
    {
        #region Méthodes Publiques
        [ErrorHandler]
        [AlbAjaxRedirect]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            var tId = id.Split('_');
            model.PageTitle = "Création double saisie";

            //using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            //{
            //    var serviceContext=client.Channel;
            //    var newId = serviceContext.GetOffreLastVersion(tId[0], tId[1], tId[2], GetUser());
            //    if (!string.IsNullOrEmpty(newId))
            //    {
            //        tId[1] = newId;
            //        id = tId[0] + "_" + newId + "_" + tId[2];
            //    }
            //}

            model.Offre = CacheModels.GetOffreFromCache(tId[0], int.Parse(tId[1]), tId[2]);
            model.Offre.HasDoubleSaisie = false;
            LoadBandeau(id);
            LoadPage(tId);
            return View(model);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public ActionResult EnregistrerDemandeur(string codeOffre, string version, string type, string argDemandeur, string tabGuid, string paramRedirect, string addParamType, string addParamValue)
        {
            JavaScriptSerializer serializer = AlbJsExtendConverter<ModeleDoubleSaisieAdd>.GetSerializer();
            var demandeurModel = serializer.ConvertToType<ModeleDoubleSaisieAdd>(serializer.DeserializeObject(argDemandeur));

            //Get Offre From Cache
            var offreMetaModel = model.Offre = CacheModels.GetOffreFromCache(codeOffre, int.Parse(version), type);
            DateTime dateNow = DateTime.Now;
            
            var offreCabinetCourtage = new CabinetAutreDto
            {
                CodeOffre = codeOffre,
                Version = version,
                Type = type,
                Code = demandeurModel.Code,
                Courtier = demandeurModel.Courtier,
                Souscripteur = string.Empty,
                CodeSouscripteur = string.Empty,
                Delegation = demandeurModel.Delegation,
                EnregistrementDate = dateNow,
                EnregistrementHeure = new TimeSpan(dateNow.Hour, dateNow.Minute, dateNow.Second),
                Action = demandeurModel.Action,
                Motif = demandeurModel.Action == "INI" ? "Apporteur initial" : demandeurModel.Action == "REF" ? "Refus" : demandeurModel.MotifRemp,
                Interlocuteur = demandeurModel.Interlocuteur,
                Reference = demandeurModel.Reference
            };

            if (!string.IsNullOrEmpty(demandeurModel.Souscripteur))
            {
                offreCabinetCourtage.Souscripteur = demandeurModel.Souscripteur.Split('-')[1].Trim();
                offreCabinetCourtage.CodeSouscripteur = demandeurModel.Souscripteur.Split('-')[0].Trim();
            }

            if (demandeurModel.SaisieDate != null)
            {
                if (demandeurModel.SaisieHeure != null)
                {
                    offreCabinetCourtage.SaisieDate = new DateTime(demandeurModel.SaisieDate.Value.Year, demandeurModel.SaisieDate.Value.Month, demandeurModel.SaisieDate.Value.Day, demandeurModel.SaisieHeure.Value.Hours, demandeurModel.SaisieHeure.Value.Minutes, 0);
                    offreCabinetCourtage.SaisieHeure = new TimeSpan(demandeurModel.SaisieHeure.Value.Hours, demandeurModel.SaisieHeure.Value.Minutes, 0);
                }
                else
                {
                    offreCabinetCourtage.SaisieDate = new DateTime(demandeurModel.SaisieDate.Value.Year, demandeurModel.SaisieDate.Value.Month, demandeurModel.SaisieDate.Value.Day, 0, 0, 0);
                    offreCabinetCourtage.SaisieHeure = new TimeSpan(0, 0, 0);
                }
            }

            if (model.Offre != null && model.Offre.CabinetAutres != null)
            {
                if (model.Offre.CabinetAutres == null)
                    model.Offre.CabinetAutres = new List<CabinetAutreDto>();
                model.Offre.CabinetAutres.Add(offreCabinetCourtage);
            }
            //Set Offre To Cache
            CacheModels.SetOffreCache(offreMetaModel, codeOffre, version, type);
            //Ecriture des informations 
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IDoubleSaisie>())
            {
                client.Channel.EnregistrerDoubleSaisie(offreCabinetCourtage, GetUser());
            }

            //Redirection vers la page sélectionnée dans le menu
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            return RedirectToAction("Index", "RechercheSaisie");

            //ObtenirCabinetHisto();

            //if (demandeurModel.Action == "REM") //REM = "Remplacé
            //    return RedirectToAction("Index", "DoubleSaisie", new { @id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) });

            //return PartialView("ListHisto", model.CourtiersHistorique);
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string paramRedirect)
        {
            //Redirection vers la page sélectionnée dans le menu
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            return RedirectToAction(job, cible);
        }

        #endregion

        #region Méthodes Privées

        private void LoadPage(string[] tId)
        {
            string branche = string.Empty;
            string cible = string.Empty;

            if (model.Offre != null)
            {
                if (model.Offre.Branche != null)
                {
                    branche = model.Offre.Branche.Code;
                    if (model.Offre.Branche.Cible != null)
                        cible = model.Offre.Branche.Cible.Code;
                }

                if (model.Offre.CabinetApporteur != null)
                {
                    model.CourtierApporteur = new ModeleDoubleSaisieCourtier
                    {
                        Code = model.Offre.CabinetApporteur.Code,
                        Courtier = model.Offre.CabinetApporteur.NomCabinet,
                        //Delegation = model.Offre.CabinetApporteur.Delegation.Nom,
                        SaisieDate = model.Offre.DateSaisie,
                        SaisieHeure = new TimeSpan(model.Offre.DateSaisie.Value.Hour,
                                                model.Offre.DateSaisie.Value.Minute,
                                                model.Offre.DateSaisie.Value.Second)
                    };

                    if (model.Offre.CabinetApporteur.Delegation != null)
                        model.CourtierApporteur.Delegation = model.Offre.CabinetApporteur.Delegation.Nom;

                    model.CourtierApporteur.Souscripteur = model.Offre.Souscripteur != null ? model.Offre.Souscripteur.Nom : string.Empty;
                    model.CourtierApporteur.SouscripteurCode = model.Offre.Souscripteur != null ? model.Offre.Souscripteur.Code : string.Empty;
                    if (model.Offre.DateEnregistrement.HasValue)
                    {
                        model.CourtierApporteur.EnregistrementDate = model.Offre.DateEnregistrement;
                        model.CourtierApporteur.EnregistrementHeure = new TimeSpan(model.Offre.DateEnregistrement.Value.Hour,
                                                                                    model.Offre.DateEnregistrement.Value.Minute,
                                                                                    model.Offre.DateEnregistrement.Value.Second);
                    }

                    if (model.Offre.CabinetGestionnaire != null)
                    {
                        model.CourtierGestionnaire = new ModeleDoubleSaisieCourtier
                        {
                            Code = model.Offre.CabinetGestionnaire.Code,
                            Courtier = model.Offre.CabinetGestionnaire.NomCabinet,
                            //Delegation = model.Offre.CabinetGestionnaire.Delegation.Nom
                        };
                        if (model.Offre.CabinetGestionnaire.Delegation != null)
                            model.CourtierGestionnaire.Delegation = model.Offre.CabinetGestionnaire.Delegation.Nom;
                    }

                    ObtenirCabinetHisto();
                }
                switch (model.Offre.Type)
                {
                    case "O":
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                        break;
                    case "P":
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                        break;
                    case "A":
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        break;
                    default:
                        break;
                }
                //Mise en commentaire le 2015-04-01 pour permettre la saisie de double saisie
                //model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type);
            }

            model.CourtierDemandeur = new ModeleDoubleSaisieCourtier();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IDoubleSaisie>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.ObtenirDoubleSaisieListes(branche, cible);
                if (result != null)
                {
                    List<AlbSelectListItem> motifsRemp = result.MotifsRemp.Select(
                        m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }
                    ).ToList();
                    model.MotifsRemp = motifsRemp;

                    List<AlbSelectListItem> notificationsApporteur = result.NotificationsApporteur.Select(
                        m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }
                    ).ToList();
                    model.NotificationsApporteur = new List<AlbSelectListItem>();

                    List<AlbSelectListItem> notificationsDemandeur = result.NotificationsDemandeur.Select(
                        m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }
                    ).ToList();
                    model.NotificationsDemandeur = new List<AlbSelectListItem>();
                }
            }
        }

        private void ObtenirCabinetHisto()
        {
            model.CourtiersHistorique = new List<ModeleDoubleSaisieCourtier>();
            if (model.Offre.DblSaisieAutresOffres != null)
            {
                if (model.Offre.DblSaisieAutresOffres.DblSaisieAutresOffres != null)
                {
                    model.CourtiersHistorique = model.Offre.DblSaisieAutresOffres.DblSaisieAutresOffres.Where(
                        oa => oa.CabinetApporteur != null
                    ).Select(
                        oa => new ModeleDoubleSaisieCourtier
                        {
                            Code = oa.CabinetApporteur.Code,
                            Courtier = oa.CabinetApporteur.NomCabinet,
                            Delegation = (oa.CabinetApporteur.Delegation == null ? string.Empty : oa.CabinetApporteur.Delegation.Nom),
                            Souscripteur = (oa.Souscripteur == null ? string.Empty : oa.Souscripteur.Nom),
                            SouscripteurCode = (oa.Souscripteur == null ? string.Empty : oa.Souscripteur.Code),
                            SaisieDate = oa.DateSaisie,
                            SaisieHeure = new TimeSpan(oa.DateSaisie.Value.Hour, oa.DateSaisie.Value.Minute, oa.DateSaisie.Value.Second),
                            EnregistrementDate = oa.DateEnregistrement,
                            EnregistrementHeure = new TimeSpan(oa.DateEnregistrement.Value.Hour, oa.DateEnregistrement.Value.Minute, oa.DateEnregistrement.Value.Second),
                            Motif = oa.MotifRefus
                        }
                    ).ToList();
                }
            }
            else
            {
                if (model.Offre.CabinetAutres != null)
                {
                    model.Offre.CabinetAutres.ForEach(
                        offCab => model.CourtiersHistorique.Add(
                            new ModeleDoubleSaisieCourtier
                            {
                                Code = Convert.ToInt32(offCab.Code),
                                Courtier = offCab.Courtier,
                                Delegation = offCab.Delegation,
                                Souscripteur = offCab.Souscripteur,
                                SouscripteurCode = offCab.CodeSouscripteur,
                                SaisieDate = offCab.SaisieDate,
                                SaisieHeure = offCab.SaisieHeure,
                                EnregistrementDate = offCab.EnregistrementDate,
                                EnregistrementHeure = offCab.EnregistrementHeure,
                                Motif = offCab.Motif,
                                LibelleMotif = offCab.LibelleMotif
                            }
                        )
                    );
                }
            }
        }

        private void LoadBandeau(string id)
        {
            model.Bandeau = null;
            model.AfficherBandeau = base.DisplayBandeau(true, id);
            model.AfficherNavigation = false;
            if (model.AfficherBandeau)
            {
                model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                model.Navigation = new Navigation_MetaModel();
                model.Navigation.Etape = Navigation_MetaModel.ECRAN_SAISIE;
                model.Navigation.IdOffre = model.Offre.CodeOffre;
                model.Navigation.Version = model.Offre.Version;
            }

            //Affichage de la navigation latérale en arboresence
            if (string.IsNullOrEmpty(model.Offre.MotifRefus))
                model.NavigationArbre = GetNavigationArbre(string.Empty);
            else
                model.NavigationArbre = GetNavigationArbre(string.Empty, returnEmptyTree: true);

        }

        #endregion

    }
}

