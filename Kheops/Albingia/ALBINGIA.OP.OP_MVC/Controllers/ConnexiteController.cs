using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.ModelesConnexite;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRecherche;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using OP.WSAS400.DTO.Engagement;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ConnexiteController : ControllersBase<ModeleConnexitePage>
    {
        #region Constantes
        private const string TYPE_CONTRAT = "P";
        private const string CODE_ENGAGEMENT = "20";
        private const string CODE_REMPLACEMENT = "01";
        private const string CODE_INFORMATION = "10";
        private const string CODE_RESILIATION = "30";
        private const string CODE_REGULARISATION = "40";

        private const string ENGAGEMENT = "d'engagement";
        private const string INFORMATION = "d'information";
        private const string RESILIATION = "de résiliation";
        private const string REMPLACEMENT = "de remplacement";
        private const string REGULARISATION = "de régularisation";

        private const string SITUATION_X = "X";
        private const string SITUATION_N = "N";
        private const string SITUATION_W = "W";
        // Messages
        private const string MESSAGE_ERREUR_PAGE_NULL = "Erreur de chargement de la page:identifiant de la page est null";
        private const string MESSAGE_ERREUR_PAGE_PARAMVIDE = "Erreur de chargement de la page:Un des trois paramètres est vide (numéro coffre/Contrat, Version, Type)";
        private const string MESSAGE_ERREUR_PAGE_VERSION_INVALIDE = "Erreur de chargement de la page:Version non valide";
        private const string MESSAGE_CONTRAT_NONREFERENCE = "Ce contrat n’est pas référencé";
        private const string MESSAGE_CONTRAT_AJOUT_IMPOSSIBLE = "Impossible d'ajouter le contrat courant en tant qu'un contrat connexe";
        private const string MESSAGE_CONTRAT_EXISTATNT = "Ce contrat est déjà dans la connexité";
        private const string MESSAGE_CONTRAT_NONACTIF = "Ce contrat n’est pas actif";

        enum TypeAction { U/*Update*/, I/*Insert*/ };

        #endregion

        #region Méthodes Publiques

        [AlbVerifLockedOffer("id", TYPE_CONTRAT)]
        [ErrorHandler]
        public ActionResult OpenConnexites(string id)
        {
            id = InitializeParams(id);
            if (id.StartsWith("CV"))
                throw new AlbFoncException("L'écran des connexités n'est pas disponible en création/modification de cannevas", true, true);
            LoadInfoPage(id);
            return PartialView("Index", model);
        }

        [ErrorHandler]
        public ActionResult AjouterConnexite(string codeTypeConnexite, int codeObservation, string observation, string numConnexite)
        {
            var toReturn = new ModeleAjoutConnexite
              {
                  CodeTypeConnexite = codeTypeConnexite,
                  Commentaire = observation,
                  NumConnexite = numConnexite,
              };

            return PartialView("AjoutConnexite", toReturn);
        }

        [ErrorHandler]
        public ActionResult AfficherConnexitePleinEcran(string numContrat, string version, string type, string codeTypeConnexite, bool isConnexiteReadOnly)
        {
            //var toReturn = new ModelePleinEcranConnexite();
            //string numConnexite = GetNumeroConnexite(numContrat, version, type, codeTypeConnexite);
            //toReturn.ContratTraite = numContrat.ToUpper().PadLeft(9, ' ') + "-" + version;
            //toReturn.listeConnexite = LoadContratsConnexes(numContrat, version, type, codeTypeConnexite, numConnexite);

            var toReturn = GetInfoContratsConnexes(numContrat, version, type, codeTypeConnexite);
            if (toReturn.InfoConnexite.listeConnexite != null && toReturn.InfoConnexite.listeConnexite.Count > 0)
                SetConnexitePleinEcran(toReturn.InfoConnexite, toReturn.NumeroConnexite, true);
            else
                SetConnexitePleinEcran(toReturn.InfoConnexite, toReturn.NumeroConnexite);

            toReturn.InfoConnexite.CodeTypeConnexite = codeTypeConnexite;
            toReturn.InfoConnexite.IsConnexiteReadOnly = isConnexiteReadOnly;
            //if (toReturn.listeConnexite != null && toReturn.listeConnexite.Count > 0)
            //    SetConnexitePleinEcran(toReturn, numConnexite, true);
            //else
            //    SetConnexitePleinEcran(toReturn, numConnexite);

            //toReturn.CodeTypeConnexite = codeTypeConnexite;

            return PartialView("ConnexitePleinEcran", toReturn.InfoConnexite);
        }

        private ModeleInfoConnexite GetInfoContratsConnexes(string codeContrat, string version, string type, string codeTypeConnexite)
        {
            ModeleInfoConnexite toReturn = new ModeleInfoConnexite();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetInfoContratsConnexes(codeContrat, version, type, codeTypeConnexite);
                if (result != null)
                {
                    toReturn.NumeroConnexite = result.NumeroConnexite;
                    toReturn.InfoConnexite = new ModelePleinEcranConnexite
                    {
                        ContratTraite = codeContrat.ToUpper().PadLeft(9, ' ') + "-" + version
                    };
                    var contratsConnexes = new List<ModeleLigneConnexite>();
                    if (result.ContratsConnexes != null && result.ContratsConnexes.Any())
                    {


                        if (codeTypeConnexite == "20")
                        {
                            contratsConnexes = ModeleLigneConnexite.LoadContratsConnexesEng(result.ContratsConnexes);

                        }
                        else
                        {
                            contratsConnexes = ModeleLigneConnexite.LoadContratsConnexes(result.ContratsConnexes);
                        }

                        if (contratsConnexes.Any())
                        {
                            contratsConnexes = contratsConnexes.OrderBy(elm => elm.Contrat == (codeContrat + "-" + version) ? -1 : 1)
                            .ThenByDescending(elm => elm.Contrat).ToList();
                        }
                        toReturn.InfoConnexite.listeConnexite = contratsConnexes;
                    }
                }
            }
            return toReturn;
        }



        [ErrorHandler]
        public ActionResult UpdateConnexites(string numeroContrat, string version, string codeTypeConnexite, bool isConnexiteReadOnly)
        {
            var resultMeth = new ModeleConnexitePage();
            ModeleInfoConnexite result = GetInfoContratsConnexes(numeroContrat, version, TYPE_CONTRAT, codeTypeConnexite);
            var contratTraite = numeroContrat + "-" + version;
            //var numConnexite = GetNumeroConnexite(numeroContrat, version, TYPE_CONTRAT, codeTypeConnexite);
            switch (codeTypeConnexite)
            {
                case CODE_ENGAGEMENT:
                    resultMeth.Engagement = new ModeleEngagement
                      {
                          ContratTraite = contratTraite,
                          ConnexitesEngagement = result.InfoConnexite.listeConnexite ?? new List<ModeleLigneConnexite>(),// LoadContratsConnexes(numeroContrat, version, TYPE_CONTRAT, codeTypeConnexite, numConnexite),
                          NumConnexiteEngagement = result.NumeroConnexite,//numConnexite,
                          IsConnexiteReadOnly = isConnexiteReadOnly
                      };
                    if (resultMeth.Engagement.ConnexitesEngagement != null && resultMeth.Engagement.ConnexitesEngagement.Count > 0)
                    {
                        SetConnexiteEngagement(resultMeth, true);
                    }
                    else
                    {
                        SetConnexiteEngagement(resultMeth);
                    }
                    resultMeth.TypeAffichage = "GESTION";
                    resultMeth.Engagement.TypeAffichage = "GESTION";
                    resultMeth.Engagement.PeriodesDeConnexites = LoadPeriodesDeConnexites(numeroContrat,version,TYPE_CONTRAT,model.NumAvenantPage,model.ModeNavig.ParseCode<ModeConsultation>());
                    return PartialView("/Views/Connexite/ListeConnexiteEngagement.cshtml", resultMeth.Engagement);

                case CODE_REMPLACEMENT:
                    resultMeth.Remplacement = new ModeleRemplacement
                      {
                          ContratTraite = contratTraite,
                          ConnexitesRemplacement = result.InfoConnexite.listeConnexite, //LoadContratsConnexes(numeroContrat, version, TYPE_CONTRAT, codeTypeConnexite, numConnexite)
                          IsConnexiteReadOnly = isConnexiteReadOnly
                      };
                    if (resultMeth.Remplacement.ConnexitesRemplacement != null && resultMeth.Remplacement.ConnexitesRemplacement.Count > 0)
                    {
                        resultMeth.Remplacement.CodeObservationRemplacement = resultMeth.Remplacement.ConnexitesRemplacement[0].CodeObservation;
                        resultMeth.Remplacement.ObservationRemplacement = resultMeth.Remplacement.ConnexitesRemplacement[0].Commentaire;
                        resultMeth.Remplacement.NumConnexiteRemplacement = result.NumeroConnexite;
                    }
                    else
                    {
                        resultMeth.Remplacement.CodeObservationRemplacement = 0;
                        resultMeth.Remplacement.ObservationRemplacement = string.Empty;
                    }
                    return PartialView("/Views/Connexite/ListeConnexiteRemplacement.cshtml", resultMeth.Remplacement);

                case CODE_INFORMATION:
                    resultMeth.Information = new ModeleInformation
                      {
                          ContratTraite = contratTraite,
                          ConnexitesInformation = result.InfoConnexite.listeConnexite,// LoadContratsConnexes(numeroContrat, version, TYPE_CONTRAT, codeTypeConnexite, numConnexite),
                          NumConnexiteInformation = result.NumeroConnexite, //numConnexite
                          IsConnexiteReadOnly = isConnexiteReadOnly
                      };
                    if (resultMeth.Information.ConnexitesInformation != null && resultMeth.Information.ConnexitesInformation.Count > 0)
                    {
                        SetConnexiteInfo(resultMeth, true);
                    }
                    else
                    {
                        SetConnexiteInfo(resultMeth);
                    }
                    return PartialView("/Views/Connexite/ListeConnexiteInformation.cshtml", resultMeth.Information);
                case CODE_RESILIATION:
                    resultMeth.Resiliation = new ModeleResiliation
                      {
                          ContratTraite = contratTraite,
                          ConnexitesResiliation = result.InfoConnexite.listeConnexite,// LoadContratsConnexes(numeroContrat, version, TYPE_CONTRAT, codeTypeConnexite, numConnexite),
                          NumConnexiteResiliation = result.NumeroConnexite, //numConnexite
                          IsConnexiteReadOnly = isConnexiteReadOnly
                      };
                    if (resultMeth.Resiliation.ConnexitesResiliation != null && resultMeth.Resiliation.ConnexitesResiliation.Count > 0)
                    {
                        SetConnexiteResiliation(resultMeth, true);
                    }
                    else
                    {
                        SetConnexiteResiliation(resultMeth);
                    }
                    return PartialView("/Views/Connexite/ListeConnexiteResiliation.cshtml", resultMeth.Resiliation);
                case CODE_REGULARISATION:
                    resultMeth.Regularisation = new ModeleRegularisation
                    {
                        ContratTraite = contratTraite,
                        ConnexitesRegularisation = result.InfoConnexite.listeConnexite,// LoadContratsConnexes(numeroContrat, version, TYPE_CONTRAT, codeTypeConnexite, numConnexite),
                        NumConnexiteRegularisation = result.NumeroConnexite, //numConnexite
                        IsConnexiteReadOnly = isConnexiteReadOnly
                    };
                    if (resultMeth.Regularisation.ConnexitesRegularisation != null && resultMeth.Regularisation.ConnexitesRegularisation.Count > 0)
                    {
                        SetConnexiteRegularisation(resultMeth, true);
                    }
                    else
                    {
                        SetConnexiteRegularisation(resultMeth);
                    }
                    return PartialView("/Views/Connexite/ListeConnexiteRegularisation.cshtml", resultMeth.Regularisation);
                default:
                    resultMeth.Engagement = new ModeleEngagement
                      {
                          ContratTraite = contratTraite,
                          ConnexitesEngagement = result.InfoConnexite.listeConnexite ?? new List<ModeleLigneConnexite>(), //LoadContratsConnexes(numeroContrat, version, TYPE_CONTRAT, codeTypeConnexite, numConnexite),
                          NumConnexiteEngagement = result.NumeroConnexite, //numConnexite
                          IsConnexiteReadOnly = isConnexiteReadOnly
                      };
                    if (resultMeth.Engagement.ConnexitesEngagement != null && resultMeth.Engagement.ConnexitesEngagement.Count > 0)
                    {
                        SetConnexiteEngagement(resultMeth, true);
                    }
                    else
                    {
                        SetConnexiteEngagement(resultMeth);
                    }
                    return PartialView("/Views/Connexite/ListeConnexiteEngagement.cshtml", resultMeth.Engagement);
            }

        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string path, string parametres, string existEngCnx, string tabGuid, string addParamType, string addParamValue)
        {
            #region vérification des paramètres
            if (string.IsNullOrEmpty(parametres))
                throw new AlbFoncException(MESSAGE_ERREUR_PAGE_NULL, true, true);
            var tIds = parametres.Split('_');
            if (tIds.Length != 3)
                throw new AlbFoncException(MESSAGE_ERREUR_PAGE_NULL, true, true);
            var numeroContrat = tIds[0];
            var version = tIds[1];
            var type = tIds[2];
            int iVersion;
            if (string.IsNullOrEmpty(numeroContrat) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(type))
                throw new AlbFoncException(MESSAGE_ERREUR_PAGE_PARAMVIDE, true, true);
            if (!int.TryParse(version, out iVersion))
                throw new AlbFoncException(MESSAGE_ERREUR_PAGE_VERSION_INVALIDE, true, true);
            #endregion

            var cible = path.Contains("cible") ? path.Split(new[] { "cible" }, StringSplitOptions.None)[1] : path;
            var job = path.Contains("job") ? path.Split(new[] { "job" }, StringSplitOptions.None)[1] : path;
            //string param = path.Contains("param") ? path.Split(new[] { "param" }, StringSplitOptions.None)[1] : string.Empty;            
            if (!string.IsNullOrEmpty(existEngCnx) && existEngCnx == "True")
                return RedirectToAction("Index", "EngagementPeriodes", new { id = string.Format("{0}{1}", parametres, BuildAddParamString(addParamType, addParamValue)), guidTab = tabGuid });
            return cible == "RechercheSaisie" ? RedirectToAction(job, cible) : RedirectToAction(job, cible, new { id = parametres });
        }

        [ErrorHandler]
        public string SauvegarderConnexite(string codeOffreConnexe, string versionConnexe, string codeOffreCourant, string versionCourant, string typeOffreCourant, string codeTypeConnexite, string numConnexite, string codeObservation, string observation, string branche, string sousBranche, string categorie)
        {
            ContratDto contrat;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                contrat = client.Channel.GetContrat(codeOffreConnexe, versionConnexe, TYPE_CONTRAT, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                if (contrat == null) return MESSAGE_CONTRAT_NONREFERENCE;
                if (codeTypeConnexite == CODE_ENGAGEMENT && (contrat.Situation == SITUATION_X || contrat.Situation == SITUATION_N || contrat.Situation == SITUATION_W))
                    return MESSAGE_CONTRAT_NONACTIF;
                if ((codeOffreConnexe.ToUpper() + "-" + versionConnexe) == (codeOffreCourant.ToUpper() + "-" + versionCourant))
                    return MESSAGE_CONTRAT_AJOUT_IMPOSSIBLE;
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                if (!string.IsNullOrEmpty(numConnexite) && client.Channel.IsContratInConnexite(codeOffreConnexe, versionConnexe, TYPE_CONTRAT, codeTypeConnexite, numConnexite))
                    return MESSAGE_CONTRAT_EXISTATNT;
                if (client.Channel.IsContratConnexe(codeOffreConnexe, versionConnexe, TYPE_CONTRAT, codeTypeConnexite))
                    return "Connexe";
                var mode = !string.IsNullOrEmpty(numConnexite) ? TypeAction.U.ToString() : TypeAction.I.ToString();
                if (codeTypeConnexite == "20")
                {
                    return client.Channel.AddConnexiteEngagement(codeOffreConnexe, versionConnexe, TYPE_CONTRAT, contrat.Branche, contrat.SousBranche, contrat.Categorie, codeOffreCourant, versionCourant, typeOffreCourant,
                        branche, sousBranche, categorie, codeObservation, observation, codeTypeConnexite, numConnexite, mode);
                }
                return client.Channel.AddConnexite(codeOffreConnexe, versionConnexe, TYPE_CONTRAT, contrat.Branche, contrat.SousBranche, contrat.Categorie, codeOffreCourant, versionCourant, typeOffreCourant,
                    branche, sousBranche, categorie, codeObservation, observation, codeTypeConnexite, numConnexite, mode);
            }
        }

        [ErrorHandler]
        public void SauvegarderObservationConnexite(string codeOffre, string version, string type, int codeObservation, string observation, string acteGestion, string addParamValue)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                finOffreClient.SaveObsvConnexite(codeOffre, version, type, codeObservation, observation, acteGestion, GetAddParamValue(addParamValue, AlbParameterName.REGULEID));
            }
        }
        [ErrorHandler]
        public ActionResult LoadDetailsConnexites(string codeOffre, string version, string type, string codeAvn, string modeNavig)
        {
            ModeleDetailsConnexitesEng Toreturn = new ModeleDetailsConnexitesEng();
            var listeModesActif = new List<AlbSelectListItem>();
            AlbSelectListItem modeActif = null;
            modeActif = new AlbSelectListItem { Value = "A", Text = "Actif", Selected = true, Title = "Actif" };
            listeModesActif.Add(modeActif);
            modeActif = new AlbSelectListItem { Value = "", Text = "Tous", Selected = false, Title = "Tous" };
            listeModesActif.Add(modeActif);
            Toreturn.ListeModesActif = listeModesActif;

            var listeModesUtilise = new List<AlbSelectListItem>();
            AlbSelectListItem modeUtilise = null;
            modeUtilise = new AlbSelectListItem { Value = "O", Text = "O", Selected = false, Title = "Oui" };
            listeModesUtilise.Add(modeActif);
            modeUtilise = new AlbSelectListItem { Value = "N", Text = "N", Selected = false, Title = "Non" };
            listeModesUtilise.Add(modeUtilise);
            Toreturn.ListeModesUtilise = listeModesUtilise;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                var result = finOffreClient.GetEngagementPeriodesDetails(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());
                if (result != null)
                {
                    var grouped = result.GroupBy(i => i.Code);
                    Toreturn.ConnexitesEngPeriodes = new List<ModeleDetailsConnexitesEngPeriode>();
                    foreach (var item in grouped)
                    {
                        var periodeEngDto = item.ElementAt(0);
                        Toreturn.ConnexitesEngPeriodes.Add(new ModeleDetailsConnexitesEngPeriode
                        {
                            Code = periodeEngDto.Code,
                            DateDebut = periodeEngDto.DateDebut,
                            DateFin = periodeEngDto.DateFin,
                            Utilise = periodeEngDto.Utilise.Equals("U", StringComparison.InvariantCulture),
                            LstEngagmentTraite = item.Select(i => new ModeleEngTrait
                            {
                                CodeEngagement = i.CodeEngagement,
                                ValeurEngagement = i.ValeurEngagement,
                            }).ToList(),
                        });
                    }
                }
            }
            return PartialView("DetailsConnexitesEngagement", Toreturn);
        }

        //SupprimerConnexite
        [ErrorHandler]
        public string SupprimerConnexite(string codeOffreConnexe, string versionConnexe, string typeOffreConnexe, string numConnexite, string codeTypeConnexite, string ideConnexite)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                if (codeTypeConnexite == "20")
                {
                     finOffreClient.DeleteConnexiteEng(codeOffreConnexe, Convert.ToInt32(versionConnexe), typeOffreConnexe );
                    return string.Empty;

                }
                return finOffreClient.DeleteConnexite(codeOffreConnexe, versionConnexe, typeOffreConnexe, numConnexite, codeTypeConnexite, ideConnexite, GetUser());
            }
        }

        public static ModeleConnexitePage LoadInfoConnexite(string id, bool isReadOnly, bool isModifHorsAvn, string modeNavig, ModeleConnexitePage resultMeth = null)
        {
            if (id.StartsWith("CV"))
                throw new AlbFoncException("L'écran des connexités n'est pas disponible en création/modification de cannevas", false, false, true);

            #region vérification des paramètres
            if (string.IsNullOrEmpty(id))
                throw new AlbFoncException(MESSAGE_ERREUR_PAGE_NULL, true, true);
            var tIds = id.Split('_');
            if (tIds.Length < 3)
                throw new AlbFoncException(MESSAGE_ERREUR_PAGE_NULL, true, true);
            var numeroContrat = tIds[0];
            var version = tIds[1];
            var type = tIds[2];
            var codeAvn = tIds.Length > 3 ? tIds[3] : "0";
            int iVersion;
            if (string.IsNullOrEmpty(numeroContrat) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(type))
                throw new AlbFoncException(MESSAGE_ERREUR_PAGE_PARAMVIDE, true, true);
            if (!int.TryParse(version, out iVersion))
                throw new AlbFoncException(MESSAGE_ERREUR_PAGE_VERSION_INVALIDE, true, true);
            #endregion
            if (resultMeth == null) {
                resultMeth = new ModeleConnexitePage();
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var contrat = client.Channel.GetContrat(numeroContrat, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());
                resultMeth.Branche = contrat.Branche;
                resultMeth.SousBranche = contrat.SousBranche;
                resultMeth.Categorie = contrat.Categorie;
            }

            var contratTraite = numeroContrat + "-" + version;
            //Chargement des contrats connexes
            resultMeth.Engagement = new ModeleEngagement
            {
                ContratTraite = contratTraite,
                NumConnexiteEngagement = GetNumeroConnexite(numeroContrat, version, type, CODE_ENGAGEMENT)
            };
            resultMeth.Engagement.ConnexitesEngagement = LoadContratsConnexes(numeroContrat, version, type, CODE_ENGAGEMENT, resultMeth.Engagement.NumConnexiteEngagement) ?? new List<ModeleLigneConnexite>();
            resultMeth.Engagement.PeriodesDeConnexites = LoadPeriodesDeConnexites(numeroContrat, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());
            if (resultMeth.Engagement.ConnexitesEngagement != null && resultMeth.Engagement.ConnexitesEngagement.Count > 0)
            {
                SetConnexiteEngagement(resultMeth, true);
            }
            else
            {
                SetConnexiteEngagement(resultMeth);
            }

            resultMeth.Information = new ModeleInformation
              {
                  ContratTraite = contratTraite,
                  NumConnexiteInformation = GetNumeroConnexite(numeroContrat, version, type, "10"),
              };
            resultMeth.Information.ConnexitesInformation = LoadContratsConnexes(numeroContrat, version, type, "10", resultMeth.Information.NumConnexiteInformation);
            if (resultMeth.Information.ConnexitesInformation != null && resultMeth.Information.ConnexitesInformation.Count > 0)
            {
                SetConnexiteInfo(resultMeth, true);
            }
            else
            {
                SetConnexiteInfo(resultMeth);
            }
            resultMeth.Remplacement = new ModeleRemplacement
              {
                  ContratTraite = contratTraite,
                  NumConnexiteRemplacement = GetNumeroConnexite(numeroContrat, version, type, "01")
              };
            resultMeth.Remplacement.ConnexitesRemplacement = LoadContratsConnexes(numeroContrat, version, type, "01", resultMeth.Remplacement.NumConnexiteRemplacement);
            if (resultMeth.Remplacement.ConnexitesRemplacement != null && resultMeth.Remplacement.ConnexitesRemplacement.Count > 0)
            {
                resultMeth.Remplacement.CodeObservationRemplacement = resultMeth.Remplacement.ConnexitesRemplacement[0].CodeObservation;
                resultMeth.Remplacement.ObservationRemplacement = resultMeth.Remplacement.ConnexitesRemplacement[0].Commentaire;
            }
            else
            {
                resultMeth.Remplacement.CodeObservationRemplacement = 0;
                resultMeth.Remplacement.ObservationRemplacement = string.Empty;
            }

            resultMeth.Resiliation = new ModeleResiliation
              {
                  ContratTraite = contratTraite,
                  NumConnexiteResiliation = GetNumeroConnexite(numeroContrat, version, type, "30")
              };
            resultMeth.Resiliation.ConnexitesResiliation = LoadContratsConnexes(numeroContrat, version, type, "30", resultMeth.Resiliation.NumConnexiteResiliation);
            if (resultMeth.Resiliation.ConnexitesResiliation != null && resultMeth.Resiliation.ConnexitesResiliation.Count > 0)
            {
                SetConnexiteResiliation(resultMeth, true);
            }
            else
            {
                SetConnexiteResiliation(resultMeth);
            }

            resultMeth.Regularisation = new ModeleRegularisation
            {
                ContratTraite = contratTraite,
                NumConnexiteRegularisation = GetNumeroConnexite(numeroContrat, version, type, "40")
            };
            resultMeth.Regularisation.ConnexitesRegularisation = LoadContratsConnexes(numeroContrat, version, type, "40", resultMeth.Regularisation.NumConnexiteRegularisation);
            if (resultMeth.Regularisation.ConnexitesRegularisation != null && resultMeth.Regularisation.ConnexitesRegularisation.Count > 0)
            {
                SetConnexiteRegularisation(resultMeth, true);
            }
            else
            {
                SetConnexiteRegularisation(resultMeth);
            }

            resultMeth.CaractereSplit = MvcApplication.SPLIT_CONST_HTML;

            resultMeth.IsReadOnly = isReadOnly;
            resultMeth.IsModifHorsAvenant = isModifHorsAvn; 
            return resultMeth;
        }

        [ErrorHandler]
        public ActionResult GetBodyRechercheSaisieForConnexites()
                {
            var model = new ModeleRecherchePage
            {
                    CritereParam = AlbConstantesMetiers.CriterParam.ContratOnly,
                    ProvenanceParam = "connexite",
                    SituationParam = string.Empty,
                    ListCabinetCourtage = new ModeleRechercheAvancee(),
                    ListPreneurAssurance = new ModeleRechercheAvancee()
                };

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var screenClient=client.Channel;
                var query = new RechercheSaisieGetQueryDto();
                var result = screenClient.RechercheSaisieGet(query);
                if (result != null)
                {
                    var cibles = result.Cibles.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.ModeleCibles = new ModeleListeCibles();
                    model.ModeleCibles.Cibles = cibles;
                    var branches = result.Branches.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.Branches = branches;
                    var etats = result.Etats.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.Etats = etats;
                    var situations = result.Situation.Select(m => new AlbSelectListItem
                    {
                        Value = m.Code,
                        Text = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty,
                        Selected = false,
                        Title = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty
                    }).ToList();
                    model.Situations = situations;
                    var listRefus = result.ListRefus.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.ListRefus = listRefus;
                    model.DateTypes = AlbTransverse.InitDateType;
                }
            }

            return PartialView("/Views/RechercheSaisie/BodyRechercheSaisie.cshtml", model);
        }

        [ErrorHandler]
        public ActionResult AfficherConfirmationAction(string codeTypeConnexite, string numConnexite, string observationActuelle, long codeObservationActuelle, string codeOffreActuelle, string versionOffreActuelle, string typeOffreActuelle, string brancheActuelle, string sousBrancheActuelle, string catActuelle, long ideConnexiteActuelle, string codeOffreAjoutee, string versionOffreAjoutee)
        {
            var toReturn = new ModeleConfirmationAction();
            switch (codeTypeConnexite)
            {
                case CODE_ENGAGEMENT: toReturn.TypeConnexite = ENGAGEMENT;
                    break;
                case CODE_REMPLACEMENT: toReturn.TypeConnexite = REMPLACEMENT;
                    break;
                case CODE_INFORMATION: toReturn.TypeConnexite = INFORMATION;
                    break;
                case CODE_RESILIATION: toReturn.TypeConnexite = RESILIATION;
                    break;
                case CODE_REGULARISATION: toReturn.TypeConnexite = REGULARISATION;
                    break;
                default: toReturn.TypeConnexite = ENGAGEMENT;
                    break;
            }
            toReturn.codeTypeConnexite = codeTypeConnexite;

            toReturn.NumConnexiteActuelle = numConnexite;
            toReturn.CodeObservationConnexiteActuelle = codeObservationActuelle;
            toReturn.ObservationConnexiteActuelle = observationActuelle;
            toReturn.NumOffreActuelle = codeOffreActuelle.ToUpper().PadLeft(9, ' ');
            toReturn.VersionOffreActuelle = versionOffreActuelle;
            toReturn.TypeOffreActuelle = typeOffreActuelle;
            toReturn.BrancheOffreActuelle = brancheActuelle;
            toReturn.SousBrancheOffreActuelle = sousBrancheActuelle;
            toReturn.CatOffreActuelle = catActuelle;
            toReturn.IdeConnexiteActuelle = ideConnexiteActuelle.ToString(CultureInfo.InvariantCulture);

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetInfoConfirmationAction(codeOffreActuelle, versionOffreActuelle, typeOffreActuelle, codeTypeConnexite,
                                    numConnexite, codeOffreAjoutee, versionOffreAjoutee, TYPE_CONTRAT, model.ModeNavig.ParseCode<ModeConsultation>());
                if (result != null)
                {
                    var contratsConnexes = ModeleLigneConnexite.LoadContratsConnexes(result.ContratsConnexesActuels);
                    if (contratsConnexes.Any())
                        contratsConnexes = contratsConnexes.OrderBy(elm => elm.Contrat == (codeOffreActuelle + "-" + versionOffreActuelle) ? -1 : 1)
                                                .ThenByDescending(elm => elm.Contrat).ToList();

                    toReturn.GroupeConnexiteActuel = OrderList(contratsConnexes, codeOffreActuelle, versionOffreActuelle);

                    if (!string.IsNullOrEmpty(result.NumConnexiteOrigine))
                    {
                        toReturn.NumConnexiteOrigine = result.NumConnexiteOrigine;
                        var contratsConnexesOrigine = ModeleLigneConnexite.LoadContratsConnexes(result.ContratsConnexesOrigines);
                        if (contratsConnexesOrigine.Any())
                            contratsConnexesOrigine = contratsConnexesOrigine.OrderBy(elm => elm.Contrat == (codeOffreAjoutee + "-" + versionOffreAjoutee) ? -1 : 1)
                                                    .ThenByDescending(elm => elm.Contrat).ToList();

                        if (contratsConnexesOrigine.Count > 0)
                        {
                            toReturn.CodeObservationConnexiteOrigine = contratsConnexesOrigine[0].CodeObservation;
                            toReturn.ObservationConnexiteOrigine = contratsConnexesOrigine[0].Commentaire;
                            toReturn.IdeConnexiteOrigine = contratsConnexesOrigine[0].IdeConnexite.ToString(CultureInfo.InvariantCulture);
                        }
                        toReturn.GroupeConnexiteOrigine = OrderList(contratsConnexesOrigine, codeOffreAjoutee, versionOffreAjoutee);
                    }

                    if (result.ContratOrigine != null)
                    {
                        toReturn.BrancheOffreOrigine = result.ContratOrigine.Branche;
                        toReturn.SousBrancheOffreOrigine = result.ContratOrigine.SousBranche;
                        toReturn.CatOffreOrigine = result.ContratOrigine.Categorie;
                    }


                }
            }

            //toReturn.GroupeConnexiteActuel = OrderList(LoadContratsConnexes(codeOffreActuelle, versionOffreActuelle, typeOffreActuelle, codeTypeConnexite, numConnexite), codeOffreActuelle, versionOffreActuelle);

            //var numConnexiteOrigine = GetNumeroConnexite(codeOffreAjoutee, versionOffreAjoutee, TYPE_CONTRAT, codeTypeConnexite);
            toReturn.NumOffreOrigine = codeOffreAjoutee.ToUpper().PadLeft(9, ' ');
            toReturn.VersionOffreOrigine = versionOffreAjoutee;
            toReturn.TypeOffreOrigine = TYPE_CONTRAT;
            //using (var serviceContext = new AffaireNouvelleClient())
            //{
            //    var contratOrigine = serviceContext.GetContrat(toReturn.NumOffreOrigine, toReturn.VersionOffreOrigine, TYPE_CONTRAT, model.ModeNavig.ParseCode<ModeConsultation>());
            //    if (contratOrigine != null)
            //    {
            //        toReturn.BrancheOffreOrigine = contratOrigine.Branche;
            //        toReturn.SousBrancheOffreOrigine = contratOrigine.SousBranche;
            //        toReturn.CatOffreOrigine = contratOrigine.Categorie;
            //    }
            //}
            //if (!string.IsNullOrEmpty(numConnexiteOrigine))
            //{
            //    toReturn.NumConnexiteOrigine = numConnexiteOrigine;
            //    var contratsConnexes = LoadContratsConnexes(codeOffreAjoutee, versionOffreAjoutee, TYPE_CONTRAT, codeTypeConnexite, numConnexiteOrigine);
            //    if (contratsConnexes.Count > 0)
            //    {
            //        toReturn.CodeObservationConnexiteOrigine = contratsConnexes[0].CodeObservation;
            //        toReturn.ObservationConnexiteOrigine = contratsConnexes[0].Commentaire;
            //        toReturn.IdeConnexiteOrigine = contratsConnexes[0].IdeConnexite.ToString(CultureInfo.InvariantCulture);
            //    }
            //    toReturn.GroupeConnexiteOrigine = OrderList(contratsConnexes, codeOffreAjoutee, versionOffreAjoutee);
            //}
            return PartialView("ConfirmationAction", toReturn);
        }

        [ErrorHandler]
        public void FusionnerDetacherConnexite(string numOffreOrigine, string typeOffreOrigine, string versionOffreOrigine, string brancheOrigine, string sousBrancheOrigine, string categorieOrigine,
          string numConnexiteOrigine, long codeObsvOrigine, string obsvOrigine, string ideConnexiteOrigine,
          string numOffreActuelle, string typeOffreActuelle, string versionOffreActuelle, string brancheActuelle, string sousBrancheActuelle, string categorieActuelle,
          string numConnexiteActuelle, long codeObsvActuelle, string obsvActuelle, string codeTypeConnexite,
          string modeAction)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                var retourMessage = finOffreClient.FusionDetachConnexite(numOffreOrigine, typeOffreOrigine, versionOffreOrigine, brancheOrigine, sousBrancheOrigine, categorieOrigine, numConnexiteOrigine, codeObsvOrigine, obsvOrigine, ideConnexiteOrigine, numOffreActuelle, typeOffreActuelle, versionOffreActuelle, brancheActuelle, sousBrancheActuelle, categorieActuelle, numConnexiteActuelle, codeObsvActuelle, obsvActuelle, codeTypeConnexite, GetUser(), modeAction);
                if (!string.IsNullOrEmpty(retourMessage)) throw new AlbFoncException(retourMessage);
            }
        }

        [ErrorHandler]
        [HttpPost]
        public ActionResult EnregistrerPeriodeConnexite(ModelePeriodeDeConnexite periodeDeConnexite)
        {
            
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffre=client.Channel;
                var dto = new PeriodeConnexiteDto
                              {
                                  CodeEngagement = periodeDeConnexite.Code,
                                  DateDebut = periodeDeConnexite.DateDebut,
                                  DateFin = periodeDeConnexite.DateFin,
                                  Traites = periodeDeConnexite.ListeDeTraites.ToDictionary(i => i.CodeEngagement ?? string.Empty, i => i.ValeurEngagement),
                                  CodeOffre = periodeDeConnexite.CodeOffre,
                                  Version = periodeDeConnexite.Version,
                                  Type = periodeDeConnexite.Type,
                                  User = GetUser()

                              };

                finOffre.SavePeriodeCnx(dto, dto.CodeEngagement == -1 ? "INSERT" : "UPDATE");
            }
            var contentData = new CommonNavigationController().LoadConnexites(string.Format("{0}_{1}_{2}", periodeDeConnexite.CodeOffre, periodeDeConnexite.Version, periodeDeConnexite.Type), false, "GESTION");

            return PartialView("ListePeriodeConnexiteEngagemenet", contentData.Connexite.Engagement);
        }

        [ErrorHandler]
        [HttpPost]
        public ActionResult SupprimerPeriodeConnexite(ModelePeriodeDeConnexite periodeDeConnexite)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffre=client.Channel;
                var dto = new PeriodeConnexiteDto
                              {
                                  CodeEngagement = periodeDeConnexite.Code,
                                  User = GetUser()
                              };

                finOffre.SavePeriodeCnx(dto, "DELETE");
            }

            var contentData = new CommonNavigationController().LoadConnexites(string.Format("{0}_{1}_{2}", periodeDeConnexite.CodeOffre, periodeDeConnexite.Version, periodeDeConnexite.Type), false, "GESTION");

            return PartialView("ListePeriodeConnexiteEngagemenet", contentData.Connexite.Engagement);
        }



        #endregion

        #region Méthodes Privées
        private static void SetConnexiteRegularisation(ModeleConnexitePage resultMeth, bool setValue = false)
        {
            resultMeth.Regularisation.CodeObservationRegularisation = setValue ? resultMeth.Regularisation.ConnexitesRegularisation[0].CodeObservation : 0;
            resultMeth.Regularisation.ObservationRegularisation = setValue ? resultMeth.Regularisation.ConnexitesRegularisation[0].Commentaire : string.Empty;
            resultMeth.Regularisation.IdeConnexiteRegularisation = setValue ? resultMeth.Regularisation.ConnexitesRegularisation[0].IdeConnexite : 0;
        }

        private static void SetConnexiteResiliation(ModeleConnexitePage resultMeth, bool setValue = false)
        {
            resultMeth.Resiliation.CodeObservationResiliation = setValue ? resultMeth.Resiliation.ConnexitesResiliation[0].CodeObservation : 0;
            resultMeth.Resiliation.ObservationResiliation = setValue ? resultMeth.Resiliation.ConnexitesResiliation[0].Commentaire : string.Empty;
            resultMeth.Resiliation.IdeConnexiteResiliation = setValue ? resultMeth.Resiliation.ConnexitesResiliation[0].IdeConnexite : 0;
        }

        private static void SetConnexiteInfo(ModeleConnexitePage resultMeth, bool setValue = false)
        {
            resultMeth.Information.CodeObservationInformation = setValue ? resultMeth.Information.ConnexitesInformation[0].CodeObservation : 0;
            resultMeth.Information.ObservationInformation = setValue ? resultMeth.Information.ConnexitesInformation[0].Commentaire : string.Empty;
            resultMeth.Information.IdeConnexiteInformation = setValue ? resultMeth.Information.ConnexitesInformation[0].IdeConnexite : 0;
        }

        private static void SetConnexiteEngagement(ModeleConnexitePage resultMeth, bool setValue = false)
        {
            resultMeth.Engagement.ExistEngCnx = setValue;
            resultMeth.Engagement.CodeObservationEngagement = setValue ? resultMeth.Engagement.ConnexitesEngagement[0].CodeObservation : 0;
            resultMeth.Engagement.ObservationEngagement = setValue ? resultMeth.Engagement.ConnexitesEngagement[0].Commentaire : string.Empty;
            resultMeth.Engagement.IdeConnexiteEngagement = setValue ? resultMeth.Engagement.ConnexitesEngagement[0].IdeConnexite : 0;
        }

        private static void SetConnexitePleinEcran(ModelePleinEcranConnexite toReturn, string numConnexite, bool setValue = false)
        {
            toReturn.CodeObservation = setValue ? toReturn.listeConnexite[0].CodeObservation : 0;
            toReturn.Observation = setValue ? toReturn.listeConnexite[0].Commentaire : string.Empty;
            toReturn.IdeConnexite = setValue ? toReturn.listeConnexite[0].IdeConnexite : 0;
            toReturn.NumConnexite = setValue ? numConnexite : string.Empty; ;
        }

        /// <summary>
        /// Chargement initial de la page avec les paramètres de l'offre/contrat
        /// </summary>
        protected override void LoadInfoPage(string id)
        {
            var tabGuid = string.Format("tabGuid{0}tabGuid", model.TabGuid);
            var folder = string.Join("_", id.Split('_').Take(3));
            //string.Format("{0}_{1}_{2}", id.Split('_')[0], id.Split('_')[1], id.Split('_')[2]);

            model.IsReadOnly = GetIsReadOnly(tabGuid, folder, model.NumAvenantPage);
            model.IsModifHorsAvenant = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(model.NumAvenantPage) ? "0" : model.NumAvenantPage));

            LoadInfoConnexite(id + "_" + model.NumAvenantPage, model.IsReadOnly, model.IsModifHorsAvenant, model.ModeNavig, model);
        }

        private static List<ModeleLigneConnexite> LoadContratsConnexes(string codeOffre, string version, string type, string codeTypeConnexite, string numConnexite)
        {
            var contratsConnexes = new List<ModeleLigneConnexite>();

            if (!string.IsNullOrEmpty(codeTypeConnexite) && !string.IsNullOrEmpty(numConnexite)) {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                    var channel = client.Channel;
                    if (codeTypeConnexite == "20") {
                        contratsConnexes = ModeleLigneConnexite.LoadContratsConnexesEng(channel.GetContratsConnexesEngagement(type, codeTypeConnexite, numConnexite));
                    }
                    else {
                        contratsConnexes = ModeleLigneConnexite.LoadContratsConnexes(channel.GetContratsConnexes(type, codeTypeConnexite, numConnexite));
                    }
                }
            }
            //tri
            if (contratsConnexes.Any())
            {
                contratsConnexes = contratsConnexes
                    .OrderBy(elm => elm.Contrat == (codeOffre + "-" + version) ? -1 : 1)
                    .ThenByDescending(elm => elm.Contrat).ToList();
            }

            return contratsConnexes;
        }
        private static List<ModelePeriodeDeConnexite> LoadPeriodesDeConnexites(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var result = new List<ModelePeriodeDeConnexite>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var service=client.Channel;
                var liste = service.GetPeriodesConnexitesEngagement(codeOffre, version, type, codeAvn, modeNavig);
                foreach (var item in liste.GroupBy(i => i.Code))
                {
                    var first = item.FirstOrDefault();
                    if (first == null) continue;

                    result.Add(new ModelePeriodeDeConnexite
                                   {
                                       Code = first.Code,
                                       DateDebut = first.DateDebut,
                                       DateFin = first.DateFin,
                                       IsActif = first.Actif.Equals("A", StringComparison.InvariantCulture),
                                       IsUtilisee = first.Utilise.Equals("U", StringComparison.InvariantCulture),
                                       ListeDeTraites = item.Select(i => new ModeleEngTrait { CodeEngagement = i.CodeEngagement, ValeurEngagement = i.ValeurEngagement }).ToList()
                                   });
                }
            }

            return result;
        }

        private static string GetNumeroConnexite(string codeOffre, string version, string type, string codeTypeConnexite)
        {
            //Récupérer le numéro de connexité du contrat s'il exite
            var numConnexite = string.Empty;
            if (!string.IsNullOrEmpty(codeTypeConnexite))
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient=client.Channel;
                    numConnexite = finOffreClient.GetNumeroConnexite(codeOffre, version, type, codeTypeConnexite);
                }
            return numConnexite;

        }

        private static List<ModeleLigneConnexite> OrderList(List<ModeleLigneConnexite> contratsConnexes, string codeOffre, string versionOffre)
        {
            var retourListe = new List<ModeleLigneConnexite>();
            foreach (var contratConnexe in contratsConnexes)
            {
                if (contratConnexe.Contrat != (codeOffre + "-" + versionOffre).ToUpper().PadLeft(9, ' ')) continue;
                retourListe.Add(contratConnexe);
                contratsConnexes.Remove(contratConnexe);
                break;
            }
            retourListe.AddRange(contratsConnexes);
            return retourListe;
        }


        #endregion
    }
}
