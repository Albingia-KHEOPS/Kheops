using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CoAssureurs;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Partenaire;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers.AffaireNouvelle
{
    public class AnCoAssureursController : ControllersBase<AnCoAssureursPage>
    {
        #region Méthodes Publiques
        [AlbVerifLockedOffer("id", "P")]
        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult Index(string id)
        {
            return Init(id);
        }

        #region ControllersBase Init
        protected override void SetPageTitle() {
            model.PageTitle = "Co-Assureurs";
        }

        protected override string InitializeParams(string id, bool strictMode = true) {
            string initialId = id;
            id = base.InitializeParams(id, strictMode);
            #region vérification des paramètres
            if (string.IsNullOrEmpty(id))
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", trace: true, sendMail: true, onlyMessage: true);
            string[] tIds = id.Split('_');
            if (tIds.Length != 3)
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
            id = base.InitializeParams(initialId, strictMode);
            return id;
        }

        protected override void LoadInfoPage(string context) {
            if (LoadModel(model.AllParameters.Folder.CodeOffre, model.AllParameters.Folder.Version.ToString(), model.AllParameters.Folder.Type, model.NumAvenantPage, model.ModeNavig, model.TabGuid) == null) {
                throw new AlbFoncException("Erreur de chargement de la page: Albingia n'est pas apériteur du contrat/offre selectionné(e)", trace: true, sendMail: true, onlyMessage: true);
            }
        }

        protected override void UpdateModel() {
            SetBandeauNavigation(model.DossierCourant);
        }
        #endregion

        /// <summary>
        /// Obtient les détails d'un co assureur (popup edition)
        /// </summary>
        /// <param name="guidId">Le GuidId du coassureur selectionné</param>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult GetCoAssureurDetail(string guidId, string codeContrat, string versionContrat, string typeContrat, string modeNavig)
        {
            //récupération du coassureur correspondant au guidId
            CoAssureur coAssureurEdit = null;
            string key = GetKeyFormules(
                                                string.Format("{0}{4}{1}{4}{2}{4}{3}", codeContrat, versionContrat, typeContrat, guidId, MvcApplication.SPLIT_CONST_HTML)
                                                );
            dynamic tempCoassDto = null;
            if (AlbSessionHelper.CoAssureursUtilisateurs.TryGetValue(key, out tempCoassDto))
            {
                coAssureurEdit = (CoAssureur)tempCoassDto;
            }
            if (coAssureurEdit == null)
                throw new AlbFoncException("Impossible d'ouvrir le coassureur sélectionné, veuillez recharger la page", trace: false, sendMail: false, onlyMessage: true);
            else
                coAssureurEdit.ModeCoass = true;
            return PartialView("EditionCoAssureur", coAssureurEdit);
        }

        /// <summary>
        /// Obtient un nouveau co assureur (pop in)
        /// </summary>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult GetNouveauCoAssureur()
        {
            CoAssureur toReturn = new CoAssureur
            {
                ModeCoass = true
            };
            return PartialView("EditionCoAssureur", toReturn);
        }

        /// <summary>
        /// Enregistre un nouveau co assureur en base
        /// </summary>
        /// <param name="guidId">GUID id (à zero normalement)</param>
        /// <param name="data">Les données du co-assureur</param>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult EnregistrerCoAssureur(string guidId, string codeContrat, string versionContrat, string typeContrat, string codeAvn, string tabGuid, string data, string typeOperation, string addParamType, string addParamValue)
        {
            var serialiserCoAssureur = AlbJsExtendConverter<CoAssureurDto>.GetSerializer();
            CoAssureurDto coAssureurDto = null;
            if (!string.IsNullOrEmpty(data))
                coAssureurDto = serialiserCoAssureur.ConvertToType<List<CoAssureurDto>>(serialiserCoAssureur.DeserializeObject(data)).FirstOrDefault();
            if (coAssureurDto == null)
                coAssureurDto = new CoAssureurDto();
            string returnMessage = null;
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            var folder = string.Format("{0}_{1}_{2}", codeContrat, versionContrat, typeContrat);
            var isReadOnly = GetIsReadOnly(tabGuid, folder, codeAvn);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));

            if (!isReadOnly || isModifHorsAvn)
            {
                coAssureurDto.TypeOperation = typeOperation;
                coAssureurDto.GuidId = guidId;
                if (typeOperation == "U")
                {
                    #region enregistrement en cache
                    //récupération de la clé et vérification de son existance dans le cache
                    string key = GetKeyFormules(
                                                 string.Format("{0}{4}{1}{4}{2}{4}{3}", codeContrat, versionContrat, typeContrat, guidId, MvcApplication.SPLIT_CONST_HTML)
                                                 );
                    dynamic tempCoassDto = null;
                    if (AlbSessionHelper.CoAssureursUtilisateurs.TryGetValue(key, out tempCoassDto))
                    {
                        if (tempCoassDto.TypeOperation == "I")
                            coAssureurDto.TypeOperation = "I";
                        //effacage de l'objet dans le cache
                        AlbSessionHelper.CoAssureursUtilisateurs.Remove(key);
                        //Ajout du coassureur mis  à jour
                        AlbSessionHelper.CoAssureursUtilisateurs.Add(key, coAssureurDto);

                    }
                    #endregion
                }
                else if (typeOperation == "I")
                {
                    #region enregistrement en cache
                    coAssureurDto.GuidId = coAssureurDto.Code;
                    //récupération de la clé et vérification de son existance dans le cache
                    string keyModel = string.Format("{0}{4}{1}{4}{2}{4}{3}", GetUser(), codeContrat, versionContrat, typeContrat, MvcApplication.SPLIT_CONST_HTML);
                    string key = GetKeyFormules(
                                                string.Format("{0}{4}{1}{4}{2}{4}{3}", codeContrat, versionContrat, typeContrat, coAssureurDto.GuidId, MvcApplication.SPLIT_CONST_HTML)
                                                );

                    dynamic tempCoassDto = null;
                    //Si le coass existe dans le cache
                    if (AlbSessionHelper.CoAssureursUtilisateurs.TryGetValue(key, out tempCoassDto))
                    {
                        //Mais qu'il a été effacé 
                        if (tempCoassDto.TypeOperation == "D")
                        {
                            //On accepte l'ajout
                            AlbSessionHelper.CoAssureursUtilisateurs.Remove(key);
                            AlbSessionHelper.CoAssureursUtilisateurs.Add(key, coAssureurDto);
                        }
                        else//Sinon erreur
                        {
                            throw new AlbFoncException("Impossible d'ajouter le coassureur, il est déjà présent dans la liste des coassureurs de ce contrat.");
                        }
                    }
                    else
                    {
                        AlbSessionHelper.CoAssureursUtilisateurs.Add(key, coAssureurDto);
                    }


                    #endregion
                }
                else if (typeOperation == "D")
                {
                    #region enregistrement en cache
                    //récupération de la clé et vérification de son existance dans le cache
                    string key = GetKeyFormules(
                                                 string.Format("{0}{4}{1}{4}{2}{4}{3}", codeContrat, versionContrat, typeContrat, guidId, MvcApplication.SPLIT_CONST_HTML)
                                                 );
                    dynamic tempCoassDto = null;
                    if (AlbSessionHelper.CoAssureursUtilisateurs.TryGetValue(key, out tempCoassDto))
                    {
                        //si la ligne dans le cache est insert, cela veut dire que la ligne n'existe pas encore dans la bdd, on retire juste la ligne du cache
                        if (tempCoassDto.TypeOperation == "I")
                        {
                            AlbSessionHelper.CoAssureursUtilisateurs.Remove(key);
                        }
                        else
                        {
                            //effacage de l'objet dans le cache
                            AlbSessionHelper.CoAssureursUtilisateurs.Remove(key);
                            //Ajout de la nouvelle période mise  à jour
                            AlbSessionHelper.CoAssureursUtilisateurs.Add(key, coAssureurDto);
                        }
                    }
                    #endregion
                }
            }
            if (string.IsNullOrEmpty(returnMessage))
                return PartialView("ListeCoAssureurs", RechargerCoAssureurs(codeContrat, versionContrat, typeContrat, codeAvn, ModeConsultation.Standard));
            throw new AlbFoncException(returnMessage);
        }

        [HandleJsonError]
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string partCouverte, string partAlbingia, string codeContrat, string version, string type, string dateAVNModificationLocale, string tabGuid, string paramRedirect, string modeNavig, string addParamType, string addParamValue, string readonlyDisplay, bool isModeConsultationEcran)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            var folder = string.Format("{0}_{1}_{2}", codeContrat, version, type);
            var isOffreReadonly = GetIsReadOnly(tabGuid, folder, numAvn);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(numAvn) ? "0" : numAvn));
            string message = string.Empty;
            string keyFormules = GetKeyFormules(
                                             string.Format("{0}{3}{1}{3}{2}", codeContrat, version, type, MvcApplication.SPLIT_CONST_HTML)
                                             );
            //Sauvegarde des données du cache en base de donnée
            if ((!isOffreReadonly || isModifHorsAvn) && cible != "AnInformationsGenerales") //si pas en readonly et si l'utilisateur n'a pas cliqué sur annuler (ie cible = infogen)
            {
                //Vérification des données du cache
                var listeCoassCache = GetCoAssureursFromCache(keyFormules, numAvn, modeNavig, true);
                double dPartCouverte = 0;
                double dPartAlbingia = 0;
                double.TryParse(partCouverte, out dPartCouverte);
                double.TryParse(partAlbingia, out dPartAlbingia);
                double total = 0;
                total = listeCoassCache.Sum(elm => elm.PourcentPart);
                if (total + dPartAlbingia != dPartCouverte)
                    throw new AlbFoncException("La somme des % de parts + la part Albingia ne peut pas être différente à la part couverte");

                ////T 3997 : Vérification des partenaires
                //VerificationPartenairesCoAssureurs(listeCoassCache);
                //Enregistrement cache
                string typeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    message = serviceContext.EnregistrerListeCoAssureurs(codeContrat, version, type, typeAvt, numAvn, listeCoassCache, GetUser());
                }
            }

            //Vidage du cache si on quitte l'écran   
            if (!isOffreReadonly || isModifHorsAvn)
                InitCacheCoassureur(keyFormules);
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
           
            if (cible == "AnCoAssureurs") {
                addParamValue += readonlyDisplay == "true" ? "||FORCEREADONLY|1" : "||AVNREFRESHUSERUPDATE|1||IGNOREREADONLY|1";
            }
            else {
                addParamValue += isModeConsultationEcran || isOffreReadonly ? string.Empty : "||IGNOREREADONLY|1";
            }

            string id = AlbParameters.BuildStandardId(
               new Folder(new[] { codeContrat, version, type }),
               tabGuid,
               addParamValue,
               modeNavig);
            return RedirectToAction(job, cible, new { id });
        }
        #endregion

        #region Méthodes Privées

        /// <summary>
        /// Chargement initial de la page avec les paramètres de l'offre/contrat
        /// </summary>
        private AnCoAssureursPage LoadModel(string numeroContrat, string version, string type, string codeAvn, string modenavig, string tabGuid)
        {
            AnCoAssureursPage tempModel = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                tempModel = (AnCoAssureursPage)client.Channel.InitCoAssureurs(type, numeroContrat, version, codeAvn, modenavig.ParseCode<ModeConsultation>());
            }
            if (tempModel != null) {
                model.IsModeAvenant = tempModel.IsModeAvenant;
                model.IsAvenantModificationLocale = tempModel.IsAvenantModificationLocale;
                model.IsTraceAvnExist = tempModel.IsTraceAvnExist;
                model.DateEffetAvenantModificationLocale = tempModel.DateEffetAvenantModificationLocale;
                model.DateEffetAvenant = tempModel.DateEffetAvenant;
                model.ListeCoAssureurs = tempModel.ListeCoAssureurs;
                model.PartAlbingia = tempModel.PartAlbingia;
                model.PartCouverte = tempModel.PartCouverte;
                model.EstVerrouillee = tempModel.EstVerrouillee;

                var folder = string.Format("{0}_{1}_{2}", numeroContrat, version, type);
                model.IsReadOnly = GetIsReadOnly("tabGuid" + tabGuid + "tabGuid", folder, codeAvn);
                model.IsModifHorsAvenant = GetIsModifHorsAvn("tabGuid" + tabGuid + "tabGuid", string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
                //Initialisation du cache
                if (!model.IsReadOnly || model.IsModifHorsAvenant) {
                    string keyFormules = GetKeyFormules(string.Join(MvcApplication.SPLIT_CONST_HTML, new[] { numeroContrat, version, type }));
                    InitCacheCoassureur(keyFormules);
                    GetCoAssureursFromCache(keyFormules, codeAvn, modenavig);
                }
                //Fin Init cache
                InfosBaseDto infosBase = null;
                using (var commonOffreClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                    infosBase = commonOffreClient.Channel.LoadaffaireNouvelleBase(numeroContrat, version, type, codeAvn, model.ModeNavig);
                }

                model.Contrat = new ContratDto() {
                    CodeContrat = infosBase.CodeOffre,
                    VersionContrat = Convert.ToInt64(infosBase.Version),
                    Type = infosBase.Type,
                    Branche = infosBase.Branche.Code,
                    BrancheLib = infosBase.Branche.Nom,
                    Cible = infosBase.Branche.Cible.Code,
                    CibleLib = infosBase.Branche.Cible.Nom,
                    CourtierGestionnaire = infosBase.CabinetGestionnaire.Code,
                    Descriptif = infosBase.Descriptif,
                    CodeInterlocuteur = infosBase.CabinetGestionnaire.Code,
                    NomInterlocuteur = infosBase.CabinetGestionnaire.Inspecteur,
                    CodePreneurAssurance = Convert.ToInt32(infosBase.PreneurAssurance.Code),
                    NomPreneurAssurance = infosBase.PreneurAssurance.NomAssure,
                    PeriodiciteCode = infosBase.Periodicite,
                    DateEffetAnnee = infosBase.DateEffetAnnee,
                    DateEffetMois = infosBase.DateEffetMois,
                    DateEffetJour = infosBase.DateEffetJour,
                    Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                    Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                };
            }

            return tempModel;
        }

        private void SetBandeauNavigation(string id)
        {
            model.AfficherBandeau = true;
            model.AfficherNavigation = model.AfficherBandeau;
            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
            model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
            string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            switch (typeAvt)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                    model.Bandeau.StyleBandeau = model.ScreenType;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                    model.Bandeau.StyleBandeau = model.ScreenType;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                    model.Bandeau.StyleBandeau = model.ScreenType;
                    break;
                default:
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    break;
            }
            //Gestion des Etapes
            model.Navigation = new Navigation_MetaModel();
            model.Navigation.Etape = Navigation_MetaModel.ECRAN_INFOGENERALE;
            //Affichage de la navigation latérale en arboresence                            
            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("CoAssureurs");
            model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
        }

        /// <summary>
        /// recharge uniquement les co-assureurs 
        /// </summary>
        /// <returns></returns>
        [ErrorHandler]
        private List<CoAssureur> RechargerCoAssureurs(string codeContrat, string versionContrat, string typeContrat, string codeAvn, ModeConsultation modeNavig)
        {
            List<CoAssureur> toReturn = new List<CoAssureur>();

            string keyFormules = GetKeyFormules(
                                             string.Format("{0}{3}{1}{3}{2}", codeContrat, versionContrat, typeContrat, MvcApplication.SPLIT_CONST_HTML)
                                             );
            var res = GetCoAssureursFromCache(keyFormules, codeAvn, modeNavig.ToString());
            foreach (CoAssureurDto elm in res)
            {
                toReturn.Add((CoAssureur)elm);
            }
            return toReturn;
        }

        #region Gestion du cache

        private void InitCacheCoassureur(string key)
        {
            var tempCollec = new List<string>();
            foreach (KeyValuePair<string, dynamic> item in AlbSessionHelper.CoAssureursUtilisateurs)
            {
                if (item.Key.Contains(key))
                {
                    tempCollec.Add(item.Key);
                }
            }
            foreach (string elm in tempCollec)
            {
                AlbSessionHelper.CoAssureursUtilisateurs.Remove(elm);
            }
        }

        private List<CoAssureurDto> GetCoAssureursFromCache(string key, string avenant, string modeNavig, bool cacheOnly = false)
        {
            List<CoAssureurDto> toReturn = new List<CoAssureurDto>();

            foreach (KeyValuePair<string, dynamic> item in AlbSessionHelper.CoAssureursUtilisateurs)
            {
                if (item.Key.Contains(key))
                {
                    toReturn.Add(item.Value);
                }
            }
            if (toReturn.Count == 0 && !cacheOnly)
            {
                toReturn = GetCoAssureursFromDb(key, avenant, modeNavig);

                //Initialisation de la variable de session
                foreach (CoAssureurDto elm in toReturn)
                {
                    string keyElm = string.Format("{0}{2}{1}", key, elm.Code, MvcApplication.SPLIT_CONST_HTML);
                    AlbSessionHelper.CoAssureursUtilisateurs.Add(keyElm, elm);
                }
            }
            return toReturn;
        }

        private static List<CoAssureurDto> GetCoAssureursFromDb(string keyConditions, string avenant, string modeNavig)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                string[] tKeyCondition = keyConditions.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
                return serviceContext.InitCoAssureurs(tKeyCondition[3], tKeyCondition[1], tKeyCondition[2], avenant, modeNavig.ParseCode<ModeConsultation>()).ListeCoAssureurs;
            }
        }

        private string GetKeyFormules(string suffixeKey)
        {
            return GetUser() + MvcApplication.SPLIT_CONST_HTML + suffixeKey;
        }

        /// <summary>
        /// Vérification des co-assureurs et des interlocuteurs co-assureurs
        /// </summary>
        /// <param name="list"> Liste des co-assureurs </param>
        private void VerificationPartenairesCoAssureurs(List<CoAssureurDto> coAssureurs)
        {
            var partenaires = new PartenairesDto
            {
                Coassureurs = coAssureurs.Select(x => new PartenaireDto
                {
                    Code = x.Code,
                    Nom = x.Nom,
                    CodeInterlocuteur = x.IdInterlocuteur,
                    NomInterlocuteur = x.Interlocuteur
                }).ToList()
            };
            VerificationPartenaires(partenaires);
        }
        #endregion

        #endregion
    }
}
