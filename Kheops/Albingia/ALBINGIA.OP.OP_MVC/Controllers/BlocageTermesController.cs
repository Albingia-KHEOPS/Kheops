using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesBlocageTermes;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IAffaireNouvelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class BlocageTermesController : ControllersBase<ModeleBlocageTermesPage>
    {
        #region Méthodes Publiques

        [AlbVerifLockedOffer("id", "P")]
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            #region vérification des paramètres
            if (string.IsNullOrEmpty(id))
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", trace: true, sendMail: true, onlyMessage: true);
            string[] tIds = id.Split('_');
            if (tIds.Length != 4)
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", trace: true, sendMail: true, onlyMessage: true);
            string numeroContrat = tIds[0];
            string version = tIds[1];
            string type = tIds[2];
            int iVersion;
            if (string.IsNullOrEmpty(numeroContrat) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(type))
                throw new AlbFoncException("Erreur de chargement de la page:Un des trois paramètres est vide (numéro coffre/Contrat, Version, Type)", trace: true, sendMail: true, onlyMessage: true);
            if (!int.TryParse(version, out iVersion))
                throw new AlbFoncException("Erreur de chargement de la page:Version non valide", trace: true, sendMail: true, onlyMessage: true);
            #endregion
            model.PageTitle = "Blocage des termes";
            model.NiveauDroit = GetNiveauDroit(tIds[3]);
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), numeroContrat + "_" + version + "_" + type, model.NumAvenantPage);
            LoadInfoPage(numeroContrat, version, type);
            SetBandeauNavigation();
            return View(model);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public ActionResult Valider(string codeContrat, string versionContrat, string type, string tabGuid, string codeZoneStop, string selectedZoneStop, string niveauDroit, string acteGestion, string modeNavig, string codeAvn)
        {
                      model.NiveauDroit = GetNiveauDroit(niveauDroit);

            if (model.NiveauDroit == AlbConstantesMetiers.DroitBlocageTerme.Niveau1)
            {
                SaveZoneStop(codeContrat, versionContrat, type, selectedZoneStop);
                int codeBloc = 0;
                if (!int.TryParse(selectedZoneStop, out codeBloc) && codeZoneStop != selectedZoneStop)
                    return DeblocageDesTermes(codeContrat, versionContrat, type, true, "Init", acteGestion,model.NiveauDroit, modeNavig, tabGuid, codeAvn);
            }
            else if (model.NiveauDroit == AlbConstantesMetiers.DroitBlocageTerme.Niveau2)
            {
                SaveZoneStop(codeContrat, versionContrat, type, selectedZoneStop);
                return Redirection("RechercheSaisie", "Index", string.Empty);
            }
            else if (model.NiveauDroit == AlbConstantesMetiers.DroitBlocageTerme.Niveau3)
            {
                int codeBloc = 0;
                //Si nouvelle zone est non numerique (ie deblocage) et que l'ancien code était numerique (ie bloqué) => refus
                if (!int.TryParse(selectedZoneStop, out codeBloc) && int.TryParse(codeZoneStop, out codeBloc) && !string.IsNullOrEmpty(selectedZoneStop))
                    throw new AlbFoncException("Vous n'êtes pas autorisé à débloquer un contrat", trace: false, sendMail: false, onlyMessage: true);
                else
                {
                    SaveZoneStop(codeContrat, versionContrat, type, selectedZoneStop);
                    return Redirection("RechercheSaisie", "Index", string.Empty);
                }
            }
            return Redirection("RechercheSaisie", "Index", string.Empty);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public ActionResult DeblocageDesTermes(string codeContrat, string versionContrat, string type, bool emission, string mode, string acteGestion, AlbConstantesMetiers.DroitBlocageTerme niveauDroit, string modeNavig, string tabGuid, string codeAvn)
        {
            if (ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                if (emission)
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        var serviceContext=client.Channel;
                        var result = serviceContext.GetEcheanceEmission(codeContrat, versionContrat, type, mode, GetUser(), acteGestion, niveauDroit,emission);
                        if (result != null && result.DateDebutDernierePeriodeAnnee > 0 &&
                                              result.DateDebutDernierePeriodeJour > 0 &&
                                              result.DateDebutDernierePeriodeMois > 0 &&
                                              result.DateEcheanceEmissionAnnee > 0 &&
                                              result.DateEcheanceEmissionJour > 0 &&
                                              result.DateEcheanceEmissionMois > 0 &&
                                              result.DateFinDernierePeriodeAnnee > 0 &&
                                              result.DateFinDernierePeriodeJour > 0 &&
                                              result.DateFinDernierePeriodeMois > 0)
                        {
                            ModeleDeblocageTermes toReturn = new ModeleDeblocageTermes();

                            toReturn.DateEmissionEcheance = new DateTime(result.DateEcheanceEmissionAnnee, result.DateEcheanceEmissionMois, result.DateEcheanceEmissionJour);
                            toReturn.DebutPeriodeEcheance = new DateTime(result.DateDebutDernierePeriodeAnnee, result.DateDebutDernierePeriodeMois, result.DateDebutDernierePeriodeJour);
                            toReturn.FinPeriodeEcheance = new DateTime(result.DateFinDernierePeriodeAnnee, result.DateFinDernierePeriodeMois, result.DateFinDernierePeriodeJour);
                            if (string.IsNullOrEmpty(toReturn.MsgErreur))
                            {
                                return PartialView("DeblocageTermes", toReturn);
                            }
                            else
                            {
                                throw new AlbFoncException(toReturn.MsgErreur, trace: false, sendMail: false, onlyMessage: true);
                            }
                        }

                    }
                }
            }
            return Redirection("RechercheSaisie", "Index", string.Empty);
        }
        
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string param)
        {
            if (cible.ToUpper() == "RECHERCHESAISIE") {
                FolderController.DeverrouillerAffaire(param);
            }
            return RedirectToAction(job, cible, new { id = param });
        }

        #endregion

        #region Méthodes Privées

        private void SaveZoneStop(string codeContrat, string versionContrat, string type, string selectedZoneStop)
        {
            int iVersion = 0;
            if (string.IsNullOrEmpty(codeContrat) || string.IsNullOrEmpty(versionContrat) || string.IsNullOrEmpty(type) || !int.TryParse(versionContrat, out iVersion))
            {
                throw new AlbFoncException("Paramètres incorrects", trace: true, sendMail: true, onlyMessage: true);
            }

            //Sauvegarde
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                serviceContext.SaveZoneStop(codeContrat, versionContrat, type, selectedZoneStop, GetUser());
            }
        }

        private AlbConstantesMetiers.DroitBlocageTerme GetNiveauDroit(string niveauDroit)
        {
            //int nivDroit = 0;
            //if (int.TryParse(niveauDroit, out nivDroit))
            //{
                return (AlbConstantesMetiers.DroitBlocageTerme)Enum.Parse(typeof(AlbConstantesMetiers.DroitBlocageTerme), niveauDroit);
            //}
            //return AlbConstantesMetiers.DroitBlocageTerme.Niveau3;
        }

        /// <summary>
        /// Chargement initial de la page avec les paramètres de l'offre/contrat
        /// </summary>
        [ErrorHandler]
        private void LoadInfoPage(string numeroContrat, string version, string type)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                model.Contrat = serviceContext.GetContrat(numeroContrat, version, type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
            }
            model.ZonesStop = GetListeZonesStop();

            if (model.Contrat != null)
            {
                if (model.Contrat.DateEffetAnnee != 0 && model.Contrat.DateEffetMois != 0 && model.Contrat.DateEffetJour != 0)
                {
                    model.DateDebutEffet = new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour);
                }
                if (model.Contrat.FinEffetAnnee != 0 && model.Contrat.FinEffetMois != 0 && model.Contrat.FinEffetJour != 0)
                {
                    model.DateFinEffet = new DateTime(model.Contrat.FinEffetAnnee, model.Contrat.FinEffetMois, model.Contrat.FinEffetJour);
                }
                if (!string.IsNullOrEmpty(model.Contrat.PeriodiciteCode) && !string.IsNullOrEmpty(model.Contrat.PeriodiciteNom))
                {
                    model.Periodicite = string.Format("{0} - {1}", model.Contrat.PeriodiciteCode, model.Contrat.PeriodiciteNom);
                }
                if (model.Contrat.Jour > 0 && model.Contrat.Mois > 0)
                {
                    model.EcheancePrincipale = new DateTime(DateTime.Now.Year, model.Contrat.Mois, model.Contrat.Jour);
                }
                if (model.Contrat.DateDebutDernierePeriodeAnnee != 0 && model.Contrat.DateDebutDernierePeriodeMois != 0 && model.Contrat.DateDebutDernierePeriodeJour != 0)
                {
                    model.DernierePeriodeDebut = new DateTime(model.Contrat.DateDebutDernierePeriodeAnnee, model.Contrat.DateDebutDernierePeriodeMois, model.Contrat.DateDebutDernierePeriodeJour);
                }
                if (model.Contrat.DateFinDernierePeriodeAnnee != 0 && model.Contrat.DateFinDernierePeriodeMois != 0 && model.Contrat.DateFinDernierePeriodeJour != 0)
                {
                    model.DernierePeriodeFin = new DateTime(model.Contrat.DateFinDernierePeriodeAnnee, model.Contrat.DateFinDernierePeriodeMois, model.Contrat.DateFinDernierePeriodeJour);
                }
                if (model.Contrat.ProchaineEchAnnee != 0 && model.Contrat.ProchaineEchMois != 0 && model.Contrat.ProchaineEchJour != 0)
                {
                    model.ProchaineEcheance = new DateTime(model.Contrat.ProchaineEchAnnee, model.Contrat.ProchaineEchMois, model.Contrat.ProchaineEchJour);
                }
                //selection zone stop
                model.ZoneStop = model.Contrat.ZoneStop;
                if (model.ZonesStop.FirstOrDefault(elm => elm.Value == model.ZoneStop) != null)
                {
                    model.ZonesStop.FirstOrDefault(elm => elm.Value == model.ZoneStop).Selected = true;
                }
            }

        }

        private List<AlbSelectListItem> GetListeZonesStop()
        {
            var toReturn = new List<AlbSelectListItem>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var lstResult = serviceContext.GetListeZonesStop();
                if (lstResult != null && lstResult.Any())
                {
                    lstResult.ForEach(elm => toReturn.Add(new AlbSelectListItem
                    {
                        Value = elm.Code,
                        Text = string.Format("{0} - {1}", elm.Code, elm.Libelle),
                        Title = string.Format("{0} - {1}", elm.Code, elm.Libelle),
                        Selected = false
                    }));
                }
            }
            return toReturn;
        }

        private void SetBandeauNavigation()
        {
            model.AfficherBandeau = true;
            model.AfficherNavigation = model.AfficherBandeau;
            model.Bandeau = base.GetInfoBandeau(model.AllParameters.FolderId.Split('_').Skip(2).FirstOrDefault());
            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            switch (typeAvt)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL: //AMO PAS SUR
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                    model.Bandeau.StyleBandeau = model.ScreenType;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                    model.Bandeau.StyleBandeau = model.ScreenType;
                    break;
                default:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    break;
            }
            //Gestion des Etapes
            model.Navigation = new Navigation_MetaModel();
            model.Navigation.Etape = Navigation_MetaModel.ECRAN_INFOGENERALE;
            //Affichage de la navigation latérale en arboresence                            
            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("BlocageTermes");
            model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
        }

        #endregion
    }
}
