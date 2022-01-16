using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Partenaire;
using OP.WSAS400.DTO.Ecran.ConfirmationSaisie;
using OP.WSAS400.DTO.Ecran.CreationSaisie;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Personnes;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    [AlbAjaxRedirect]
    public class ConfirmationSaisieController : ControllersBase<ModeleConfirmationSaisiePage>
    {
        #region Méthodes Publiques

        
        [AlbAjaxRedirect]
        [ErrorHandler]
        public ActionResult Offre(string id)
        {
            model.PageTitle = "Création saisie";
            model.AfficherNavigation = true;
            model.Navigation = new Navigation_MetaModel
            {
                Etape = Navigation_MetaModel.ECRAN_CONFIRMATION
            };

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IConfirmationSaisie>())
            {
                var screenClient = channelClient.Channel;
                var query = new ConfirmationSaisieGetQueryDto();
                var tabTemp = new string[0];
                try
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        id = InitializeParams(id);
                        tabTemp = id.Split('_');
                    }
                    else
                    {
                        return RedirectToAction("Index", "RechercheSaisie");
                    }
                }
                catch
                {
                    return RedirectToAction("Index", "RechercheSaisie");
                }
                //TODO Prise en charge du paramètre avenant
                if (tabTemp.Length >= 3)
                {
                    model.CodeOffre = tabTemp[0];
                    model.Version = tabTemp[1];
                    model.Type = tabTemp[2];
                    query.CodeOffre = tabTemp[0];
                    int version = 0;
                    int.TryParse(tabTemp[1].ToString(), out version);
                    query.VersionOffre = version;
                    query.Type = tabTemp[2];
                }
                var result = screenClient.ConfirmationSaisieGet(query);
                model.PageTitle = "Confirmation de création de saisie";

                if (result != null && result.Offre != null)
                {
                    model.MotifRefus = result.Offre.MotifRefus;

                    model.ApporteurAdresse = new ModeleContactAdresse(2, argContext: "Apporteur")
                    {
                        ReadOnly = true
                    };
                    if (result.Offre.CabinetApporteur != null)
                    {
                        model.ApporteurCourtierNom = result.Offre.CabinetApporteur.NomCabinet;
                        if (result.Offre.CabinetApporteur.Adresse != null)
                        {
                            model.ApporteurAdresse.No = result.Offre.CabinetApporteur.Adresse.NumeroVoie == 0 ? string.Empty : result.Offre.CabinetApporteur.Adresse.NumeroVoie.ToString();
                            model.ApporteurAdresse.No2 = result.Offre.CabinetApporteur.Adresse.NumeroVoie2;
                            model.ApporteurAdresse.Extension = result.Offre.CabinetApporteur.Adresse.ExtensionVoie;
                            model.ApporteurAdresse.Voie = result.Offre.CabinetApporteur.Adresse.NomVoie;
                            model.ApporteurAdresse.Distribution = result.Offre.CabinetApporteur.Adresse.Batiment;
                            model.ApporteurAdresse.CodePostal = result.Offre.CabinetApporteur.Adresse.CodePostal == 0 ? string.Empty : result.Offre.CabinetApporteur.Adresse.CodePostal.ToString();
                            model.ApporteurAdresse.Ville = result.Offre.CabinetApporteur.Adresse.NomVille;
                        }
                        if (result.Offre.CabinetApporteur.Delegation != null)
                        {
                            model.ApporteurCourtierDelegation = result.Offre.CabinetApporteur.Delegation.Nom;
                            if (result.Offre.CabinetApporteur.Delegation.Inspecteur != null)
                            {
                                model.ApporteurCourtierInspecteur = result.Offre.CabinetApporteur.Delegation.Inspecteur.Nom;
                            }
                        }
                        model.ApporteurCourtierValide = result.Offre.CabinetApporteur.EstValide;
                        model.ConfirmationSaisie = result.Offre.CabinetApporteur.EstValide;
                    }
                    model.PreneurAdresse = new ModeleContactAdresse(13, argContext: "Preneur")
                    {
                        ReadOnly = true
                    };
                    if (result.Offre.PreneurAssurance != null)
                    {
                        model.PreneurNom = result.Offre.PreneurAssurance.NomAssure;
                        if (result.Offre.PreneurAssurance.Adresse != null)
                        {
                            //using(model.PreneurAdresse )
                            //{
                            model.PreneurAdresse.No = result.Offre.PreneurAssurance.Adresse.NumeroVoie == 0 ? string.Empty : result.Offre.PreneurAssurance.Adresse.NumeroVoie.ToString();
                            model.PreneurAdresse.No2 = result.Offre.PreneurAssurance.Adresse.NumeroVoie2;
                            model.PreneurAdresse.Extension = result.Offre.PreneurAssurance.Adresse.ExtensionVoie;
                            model.PreneurAdresse.Voie = result.Offre.PreneurAssurance.Adresse.NomVoie;
                            model.PreneurAdresse.Distribution = result.Offre.PreneurAssurance.Adresse.Batiment;
                            model.PreneurAdresse.CodePostal = result.Offre.PreneurAssurance.Adresse.CodePostal == 0 ? string.Empty : result.Offre.PreneurAssurance.Adresse.CodePostal.ToString();
                            model.PreneurAdresse.Ville = result.Offre.PreneurAssurance.Adresse.NomVille;
                            //};
                        }
                        model.ConfirmationSaisie = model.ConfirmationSaisie && result.Offre.PreneurAssurance.EstActif;
                    }
                    model.InfoSaisieDateSaisie = result.Offre.DateSaisie;
                    model.InfoSaisieNoSaisieAttribuee = result.Offre.CodeOffre;
                    model.InfoSaisieNoAliment = result.Offre.Version.ToString();
                    model.InfoSaisieBrancheNom = result.Offre.Branche.Nom;

                    model.Offre = CacheModels.GetOffreFromCache(model.CodeOffre, Int32.Parse(model.Version), model.Type, model.ModeNavig.ParseCode<ModeConsultation>());
                    //Je passe par une liste pour pouvoir ajouter l'élément vide.
                    List<AlbSelectListItem> motifsRefus = result.MotifsRefus.Select
                    (
                        mr =>
                        new AlbSelectListItem { Value = mr.Code, Text = string.Format("{0} - {1}", mr.Code, mr.Libelle), Selected = false, Title = string.Format("{0} - {1}", mr.Code, mr.Libelle) }
                    ).ToList();

                    model.MotifsRefus = motifsRefus;
                    model.MotifsAttente = motifsRefus;
                }
            }

            model.Bandeau = null;
            model.AfficherBandeau = base.DisplayBandeau(true, id);
            model.AfficherNavigation = model.AfficherBandeau;
            model.AfficherArbre = true;
            if (model.AfficherBandeau)
            {
                model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                model.Navigation.IdOffre = model.Offre.CodeOffre;
                model.Navigation.Version = model.Offre.Version;
                model.Bandeau.HasDoubleSaisie = false;//on simule l'absence de double saisie dans cet écran pour ne pas afficher le bouton d'accès
            }
            model.NavigationArbre = GetNavigationArbre("InfoSaisie", returnEmptyTree: true);
            model.IsReadOnly =
                GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version.ToString() + "_" + model.Offre.Type, model.NumAvenantPage);
            if (!model.IsReadOnly)
                model.MotifRefus = string.Empty;

            SetViewBagCanevas(model.Offre.CodeOffre);

            return PartialView(model);
        }

        [HttpPost]
        [ErrorHandler]
        public ActionResult Offre(string id, string motif, string confirmation, string attente, string refus, string actionSuivante, string tabGuid, string paramRedirect, string addParamType, string addParamValue, bool readOnly)
        {

            //Binding
            ConfirmationSaisieSetQueryDto query = new ConfirmationSaisieSetQueryDto();

            query.CodeOffre = id.Split('_')[0];
            query.RefusImmediat = Convert.ToBoolean(refus);
            query.Attente = Convert.ToBoolean(attente);
            if (query.RefusImmediat || query.Attente)
            {
                string codeMotifRefus = motif;
                if (String.IsNullOrEmpty(codeMotifRefus) && query.RefusImmediat)
                {
                    throw new AlbFoncException("Le motif de refus n'est pas renseigné", trace: false, sendMail: false, onlyMessage: true);
                }
                if (String.IsNullOrEmpty(codeMotifRefus) && query.Attente)
                {
                    throw new AlbFoncException("Le motif de mise en attente n'est pas renseigné", trace: false, sendMail: false, onlyMessage: true);
                }
                ParametreDto motifRefus = new ParametreDto { Code = codeMotifRefus };
                query.MotifRefus = motifRefus;
            }

            if (!readOnly)
            {
                //Ecriture
                ConfirmationSaisieSetResultDto result = null;
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IConfirmationSaisie>())
                {
                    var client = channelClient.Channel;
                    result = client.ConfirmationSaisieSet(query);
                }
            }


            if (!string.IsNullOrEmpty(actionSuivante))
            {
                switch (actionSuivante)
                {
                    case "Suivant":
                        //Redirection vers la page sélectionnée dans le menu
                        if (!string.IsNullOrEmpty(paramRedirect))
                        {
                            var tabParamRedirect = paramRedirect.Split('/');
                            return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                        }
                        return RedirectionAction(id.Split('_')[0], id.Split('_')[1], id.Split('_')[2], "ModifierOffre", "Index", tabGuid, paramRedirect, addParamType, addParamValue, string.Empty);
                    case "Terminer":
                        return RedirectionAction(id.Split('_')[0], id.Split('_')[1], id.Split('_')[2], "RechercheSaisie", "Index", tabGuid, paramRedirect, addParamType, addParamValue, "loadParam");
                }
            }
            return null;
            //return RedirectToAction("Index", "RechercheSaisie");
        }

        [ErrorHandler]
        public RedirectToRouteResult RedirectionAction(string codeOffre, string version, string type, string cible, string job, string tabGuid, string paramRedirect, string addParamType, string addParamValue, string loadParamOffre = "")
        {

            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (string.IsNullOrEmpty(numAvn))
                numAvn = "0";
            //Redirection vers la page sélectionnée dans le menu
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            if (cible == "RechercheSaisie")
            {
                var folder = string.Format("{0}_{1}_{2}", codeOffre, version, type);
                var isReadOnly = GetIsReadOnly(tabGuid, folder, numAvn);
                var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, numAvn));

                //Déverouillage de l'offre/contrat
                ALBINGIA.OP.OP_MVC.Common.CommonVerouillage.DeverrouilleFolder(codeOffre, version, type, numAvn, tabGuid, false, isReadOnly, isModifHorsAvn);
            }
            return string.IsNullOrEmpty(codeOffre) || string.IsNullOrEmpty(version) ? RedirectToAction(job, cible) : RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + (string.IsNullOrEmpty(loadParamOffre) ? "" : "_" + loadParamOffre) + tabGuid + BuildAddParamString(addParamType, addParamValue) });
        }

        [ErrorHandler]
        public ActionResult LoadInfoBase(string codeOffre, string version, string type, bool readOnly)
        {

            ModeleCreationSaisiePage model = LoadInformationsBase(codeOffre + "_" + version + "_" + type + "_" + readOnly);
            SetViewBagCanevas(codeOffre);
            model.ContactAdresse.Context = "adrbase";
            return PartialView("../CreationSaisie/BodyCreationSaisie", model);
        }

        //[HandleJsonError]
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult SaveInfoBase(string codeOffre, string version, string type, string dateSaisie, string heureSaisie, string codeSouscripteur, string codeGestionnaire,
            string codeInterlocuteur, string reference, string codePreneur, string identification, string motClef1, string motClef2, string motClef3,
            string observations,
            string batiment, string numVoie, string extensionVoie, string nomVoie, string boitePostale, string cp, string nomVille, string cpCdx, string nomVilleCdx,
            string nomPays, string matricule, string numeroChrono, string job, string cible, string preneurEstAssure,
            string codeCourtierGestionnaire, string nomCourtierGestionnaire, string nomInterlocuteur, string nomPreneurAssurence,
            string tabGuid, string modeNavig)
        {
            if (DateTime.Parse(dateSaisie) > DateTime.Now)
            {
                throw new AlbFoncException("La date de saisie est supérieure à la date du jour");
            }
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICreationOffre>())
            {
                var serviceContext = channelClient.Channel;
                var tDate = dateSaisie.Split('/');
                var tHour = heureSaisie.Split(':');
                OffreDto offre = new OffreDto
                {
                    CodeOffre = codeOffre,
                    Version = string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0,
                    Type = type,
                    DateSaisie = new DateTime(Convert.ToInt16(tDate[2]), Convert.ToInt16(tDate[1]), Convert.ToInt16(tDate[0]), Convert.ToInt16(tHour[0]), Convert.ToInt16(tHour[1]), 0),
                    Souscripteur = new SouscripteurDto { Code = codeSouscripteur },
                    Gestionnaire = new GestionnaireDto { Id = codeGestionnaire },
                    CodeInterlocuteur = codeInterlocuteur,
                    RefCourtier = reference,
                    PreneurAssurance = new AssureDto
                    {
                        Code = codePreneur,
                        PreneurEstAssure = preneurEstAssure.ToUpper() == "TRUE"
                    },
                    Descriptif = identification,
                    MotCle1 = motClef1,
                    MotCle2 = motClef2,
                    MotCle3 = motClef3,
                    Observation = observations.Replace("\r\n", "<br>"),
                    AdresseOffre = new AdressePlatDto
                    {
                        Batiment = batiment,
                        NumeroVoie = Int32.TryParse(numVoie.Split(new char[] { '/', '-' })[0], out int num) ? num : -1,
                        NumeroVoie2 = !string.IsNullOrEmpty(numVoie) ? (numVoie.Contains("/") || numVoie.Contains("-") ? numVoie.Split(new char[] { '/', '-' })[1] : "") : "",
                        ExtensionVoie = extensionVoie,
                        NomVoie = nomVoie,
                        BoitePostale = boitePostale,
                        CodePostal = !string.IsNullOrEmpty(cp) ? Convert.ToInt32(cp) : -1,
                        NomVille = nomVille,
                        CodePostalCedex = !string.IsNullOrEmpty(cpCdx) ? Convert.ToInt32(cpCdx) : -1,
                        NomCedex = nomVilleCdx,
                        NomPays = nomPays,
                        MatriculeHexavia = matricule,
                        NumeroChrono = !string.IsNullOrEmpty(numeroChrono) ? Convert.ToInt32(numeroChrono) : 0,
                        Departement = !string.IsNullOrEmpty(cp) && cp.Length == 5 ? cp.Substring(0, 2) : string.Empty
                    }
                };

                if (codeCourtierGestionnaire == "0" || string.IsNullOrEmpty(codeCourtierGestionnaire))
                    throw new AlbFoncException(AlbOpConstants.CABINET_GESTIONNAIRE_EMPTY_ERROR);
                ////T 3997 : Vérification des partenaires
                //VerificationPartenaireOffre(codeOffre, version, type, codeCourtierGestionnaire, nomCourtierGestionnaire, codeInterlocuteur, nomInterlocuteur, codePreneur, nomPreneurAssurence);
                serviceContext.SaveInfoBase(offre);
            }
            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + GetFormatModeNavig(modeNavig) });
        }
        #endregion

        #region Méthodes privées

        private ModeleCreationSaisiePage LoadInformationsBase(string id)
        {
            ModeleCreationSaisiePage model = new ModeleCreationSaisiePage();

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICreationOffre>())
            {
                var screenClient = channelClient.Channel;
                var query = new CreationSaisieGetQueryDto();
                var result = screenClient.CreationSaisieGet(query);
                if (result != null)
                {
                    List<AlbSelectListItem> branches = result.Branches.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Nom), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Nom) }).ToList();
                    branches = branches.Where(el => CacheUserRights.UserRights.Any(elU => (elU.Branche == el.Value || elU.Branche == "**") && elU.TypeDroit != TypeDroit.V.ToString())).ToList();
                    List<AlbSelectListItem> cibles = new List<AlbSelectListItem>();

                    model.InformationSaisie.Branches = branches;
                    if (cibles.Count > 0)
                        model.InformationSaisie.InformationCible.Cibles = cibles;

                    string[] tId = id.Split('_');
                    model.Offre = CacheModels.GetOffreFromCache(tId[0], int.Parse(tId[1]), tId[2], ModeConsultation.Standard);
                    model.IsReadOnly = tId[3].ToLower() == "true";
                    model.IsReadOnlyDisplay = model.IsReadOnly;
                    #region LoadInfoOffre

                    model.CodeOffre = model.Offre.CodeOffre;
                    model.Version = model.Offre.Version;
                    model.EditMode = true;
                    model.InformationSaisie = new ModeleInformationSaisie
                    {
                        Branche = model.Offre.Branche.Code,
                        Branches = (model.InformationSaisie != null && model.InformationSaisie.Branches != null) ? model.InformationSaisie.Branches : new List<AlbSelectListItem>(),
                        InformationCible = new ModeleInfoSaisieCible
                        {
                            Cible = model.Offre.Branche.Cible.Code,
                            CibleLibelle = model.Offre.Branche.Cible.Code + " - " + model.Offre.Branche.Cible.Nom,
                            IsReadOnly = true,
                            IsConfirmation = true
                        },
                        EditMode = true,
                        IsConfirmation = true,
                        IsReadOnlyDisplay = model.IsReadOnlyDisplay,
                        InformationTemplate = new ModeleInfoSaisieTemplate { IsReadOnlyDisplay = model.IsReadOnlyDisplay }
                    };

                    if (model.Offre.Souscripteur != null)
                    {
                        model.InformationSaisie.CodeSouscripteur = model.Offre.Souscripteur.Code;
                        model.InformationSaisie.Souscripteurs = model.Offre.Souscripteur.Code + " - " + model.Offre.Souscripteur.Nom;
                    }

                    if (model.Offre.Gestionnaire != null)
                    {
                        model.InformationSaisie.CodeGestionnaire = model.Offre.Gestionnaire.Id;
                        model.InformationSaisie.Gestionnaires = model.Offre.Gestionnaire.Id + " - " + model.Offre.Gestionnaire.Nom;
                    }

                    if (model.Offre.DateSaisie != null)
                    {
                        model.InformationSaisie.DateSaisie = new DateTime(model.Offre.DateSaisie.Value.Year, model.Offre.DateSaisie.Value.Month, model.Offre.DateSaisie.Value.Day);
                        model.InformationSaisie.HeureSaisieString = new TimeSpan(model.Offre.DateSaisie.Value.Hour, model.Offre.DateSaisie.Value.Minute, model.Offre.DateSaisie.Value.Second);
                    }
                    model.CabinetCourtage = new ModeleCabinetCourtage
                    {
                        CodeCabinetCourtage = model.Offre.CabinetApporteur.Code,
                        NomCabinetCourtage = model.Offre.CabinetApporteur.NomCabinet,
                        CodeInterlocuteur = model.Offre.CodeInterlocuteur,
                        NomInterlocuteur = model.Offre.CodeInterlocuteur == "0" ? string.Empty : model.Offre.CodeInterlocuteur + " - " + model.Offre.NomInterlocuteur,
                        Reference = model.Offre.RefChezCourtier,
                        EditMode = true,
                        IsReadOnlyDisplay = model.IsReadOnlyDisplay
                    };
                    model.PreneurAssurance = new ModelePreneurAssurance { IsReadOnlyDisplay = model.IsReadOnlyDisplay };
                    if (model.Offre.PreneurAssurance != null)
                    {
                        model.PreneurAssurance.CodePreneurAssurance = !string.IsNullOrEmpty(model.Offre.PreneurAssurance.Code)
                            ? Convert.ToInt32(model.Offre.PreneurAssurance.Code) : 0;
                        model.PreneurAssurance.PreneurEstAssure = model.Offre.PreneurAssurance.PreneurEstAssure;
                    }

                    model.Description = new ModeleDescription
                    {
                        MotClef1 = model.Offre.MotCle1,
                        MotClef2 = model.Offre.MotCle2,
                        MotClef3 = model.Offre.MotCle3,
                        Descriptif = model.Offre.Descriptif,
                        Observation = model.Offre.Observation,
                        IsReadOnlyDisplay = model.IsReadOnlyDisplay
                    };

                    if (model.Offre.AdresseOffre != null)
                    {
                        model.ContactAdresse = new ModeleContactAdresse
                        {
                            Batiment = model.Offre.AdresseOffre.Batiment,
                            Distribution = model.Offre.AdresseOffre.BoitePostale,
                            Extension = model.Offre.AdresseOffre.ExtensionVoie,
                            MatriculeHexavia = model.Offre.AdresseOffre.MatriculeHexavia,
                            No = model.Offre.AdresseOffre.NumeroVoie == 0 ? string.Empty : model.Offre.AdresseOffre.NumeroVoie.ToString(),
                            No2 = model.Offre.AdresseOffre.NumeroVoie2,
                            NoChrono = model.Offre.AdresseOffre.NumeroChrono,
                            Voie = model.Offre.AdresseOffre.NomVoie,
                            //ReadOnly = true,
                            SaisieHexavia = true,
                            FirstIndex = 16,
                            Context = "adrbase",
                            IsReadOnlyDisplay = model.IsReadOnlyDisplay
                        };
                        int depart = 0;
                        Int32.TryParse(model.Offre.AdresseOffre.Departement, out depart);

                        string codePostal = string.Empty;
                        string codePX = string.Empty;
                        //string codePostal = depart.ToString("D2") + currentObj.AdresseObjet.CodePostal.ToString("D3");
                        //string codePX = depart.ToString("D2") + currentObj.AdresseObjet.CodePostalCedex.ToString("D3");
                        if (depart > 0)
                        {
                            codePostal = depart.ToString("D2") + model.Offre.AdresseOffre.CodePostal.ToString("D3");
                            codePX = depart.ToString("D2") + model.Offre.AdresseOffre.CodePostalCedex.ToString("D3");
                        }
                        else
                        {
                            codePostal = model.Offre.AdresseOffre.CodePostal.ToString();
                            codePX = model.Offre.AdresseOffre.CodePostalCedex.ToString();
                        }

                        model.ContactAdresse.CodePostal = model.Offre.AdresseOffre.CodePostal == 0 ? string.Empty : model.Offre.AdresseOffre.CodePostal.ToString("D3");
                        model.ContactAdresse.CodePostalCedex = model.Offre.AdresseOffre.CodePostalCedex == 0 ? string.Empty : model.Offre.AdresseOffre.CodePostalCedex.ToString("D3");
                        model.ContactAdresse.Ville = model.Offre.AdresseOffre.NomVille;
                        model.ContactAdresse.VilleCedex = model.Offre.AdresseOffre.NomCedex;
                        model.ContactAdresse.Pays = model.Offre.AdresseOffre.NomPays;
                        model.ContactAdresse.CodePostal = codePostal;
                        model.ContactAdresse.CodePostalCedex = codePX;
                    }
                    else
                    {
                        model.ContactAdresse = new ModeleContactAdresse(16, true, true);
                    }

                    model.ContactAdresse.ReadOnly = false;
                    model.ContactAdresse.IsReadOnlyDisplay = model.IsReadOnlyDisplay;

                    #endregion

                    model.InformationSaisie.InformationTemplate = null;

                    if (model.Description != null)
                    {
                        model.Description.MotsClefs1 = LstMotsCles(model.Offre.Branche.Code, model.Offre.Branche.Cible.Code);
                        model.Description.MotsClefs2 = LstMotsCles(model.Offre.Branche.Code, model.Offre.Branche.Cible.Code);
                        model.Description.MotsClefs3 = LstMotsCles(model.Offre.Branche.Code, model.Offre.Branche.Cible.Code);
                    }

                    AlbSelectListItem motclef1 = model.Description.MotsClefs1.FirstOrDefault(m => m.Value == model.Offre.MotCle1);
                    if (motclef1 != null) motclef1.Selected = true;
                    AlbSelectListItem motclef2 = model.Description.MotsClefs2.FirstOrDefault(m => m.Value == model.Offre.MotCle2);
                    if (motclef2 != null) motclef2.Selected = true;
                    AlbSelectListItem motclef3 = model.Description.MotsClefs3.FirstOrDefault(m => m.Value == model.Offre.MotCle3);
                    if (motclef3 != null) motclef3.Selected = true;

                }
            }

            model.InformationSaisie.InformationCible.IsReadOnly = model.IsReadOnly;
            model.InformationSaisie.IsReadOnlyDisplay = model.IsReadOnlyDisplay;
            model.Description.IsReadOnly = model.IsReadOnly;
            model.Description.IsReadOnlyDisplay = model.IsReadOnlyDisplay;
            model.IsConfirmation = true;

            return model;
        }

        private List<AlbSelectListItem> LstMotsCles(string codeBranche, string codeCible)
        {
            var value = new List<AlbSelectListItem>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.ObtenirMotClef(codeBranche, codeCible);
                if (result.Any())
                {
                    result.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code,
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }));
                }

            }
            return value;
        }

        /// <summary>
        /// Set la valeur d'une propriété du ViewBag permettant d'identifier si l'offre en cours d'utilisation est un Canevas
        /// </summary>
        /// <param name="codeOffre">Le code de l'offre en cours d'utilisation</param>
        private void SetViewBagCanevas(string codeOffre)
        {
            if (codeOffre != null)
            {
                ViewData["IsCanevas"] = codeOffre.Trim().Substring(0, 2) == "CV" ? true : false;
            }
            else
            {
                ViewData["IsCanevas"] = false;
            }
        }
        /// <summary>
        /// Vérification des partenaires / offre
        /// </summary>
        /// <param name="codeCourtierGestionnaire">Code courtier gestionnaire</param>
        /// <param name="nomCourtierGestionnaire">Nom courtier gestionnaire</param>
        /// <param name="codeInterlocuteur">Code interlocuteur</param>
        /// <param name="nomInterlocuteur">Nom interlocuteur</param>
        /// <param name="codePreneur">Code preneur</param>
        /// <param name="nomPreneurAssurence">nom Preneur Assurence</param>
        private void VerificationPartenaireOffre(string codeOffre, string version, string type, string codeCourtierGestionnaire, string nomCourtierGestionnaire, string codeInterlocuteur, string nomInterlocuteur, string codePreneur, string nomPreneurAssurence)
        {
            if (CanUseLablat())
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {

                    var partenaires = new PartenairesDto
                    {
                        CourtierGestionnaire = new PartenaireDto
                        {
                            Code = codeCourtierGestionnaire,
                            Nom = nomCourtierGestionnaire,
                            CodeInterlocuteur = codeInterlocuteur.ParseInt().Value,
                            NomInterlocuteur = nomInterlocuteur
                        },
                        PreneurAssurance = new PartenaireDto
                        {
                            Code = codePreneur,
                            Nom = nomPreneurAssurence
                        },
                        AssuresAdditionnels = channelClient.Channel.GetListAssuresAdditionnelsInfosBase(
                                                             codeOffre.Trim(),
                                                             version,
                                                             type,
                                                             "0",
                                                             ModeConsultation.Standard
                                                            )
                    };
                    VerificationPartenaires(partenaires);
                }
            }
        }
        #endregion
    }
}
