using Albingia.Kheops.OP.Application.Port.Driver;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleEngagements;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.NavigationArbre;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class EngagementsConnexiteController : ControllersBase<ModeleEngagementsConnexitePage>
    {
        private const string TYPE_CONTRAT = "P";
        private const string CODE_ENGAGEMENT = "20";

        [ErrorHandler]
        public ActionResult Index(string id)
        {
            this.model.PageTitle = "Engagements de connexité";
            SetProvGuidTab(id);
            id = InitializeParams(id);
            LoadInfoPage(id);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IReferentielPort>()) {
                this.model.Connexites = ConnexitesController.BuildModel(
                    new Models.Affaire { CodeOffre = this.model.Contrat.CodeContrat, Version = (int)this.model.Contrat.VersionContrat, Type = model.Contrat.Type },
                    client.Channel.GetFamillesReassurances(),
                    this.model.ModeNavig != "H" ? ModeConsultation.Standard : ModeConsultation.Historique,
                    TypeConnexite.Engagement);
            }
            return View(this.model);
        }

        [ErrorHandler]
        public ActionResult UpdateEngagementTraite(string idEngagement, string parameters, string numConnexite, string typeOffre, string versionOffre, string codeOffre, DateTime? dateDeb, DateTime? dateFin)
        {
            string[] tabParams1 = parameters.Split('_');
            if (tabParams1.Length > 1)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient=client.Channel;
                    if (idEngagement != "-9999")//Mise à jours
                    {
                        for (int i = 1; i < tabParams1.Length; i++)
                        {
                            string[] tabParams2 = tabParams1[i].Split(new[] { "idTraite" }, StringSplitOptions.None);
                            int montant = int.Parse(tabParams2[0]);

                            string[] tabParams3 = tabParams2[1].Split(new[] { "codeFamille" }, StringSplitOptions.None);

                            finOffreClient.UpdateEngagementTraite(codeOffre, versionOffre, typeOffre, 0, 0, int.Parse(idEngagement),
                                int.Parse(tabParams3[0]), tabParams3[1], montant, montant, GetUser(), "U");
                        }
                    }
                    else//Insertion
                    {
                        int dateD = AlbConvert.ConvertDateToInt(dateDeb.Value).Value;
                        int dateF = AlbConvert.ConvertDateToInt(dateFin.Value).Value;
                        //Première étape insertion
                        string[] tabParams2 = tabParams1[1].Split(new[] { "codeFamille" }, StringSplitOptions.None);
                        string codeFamille = tabParams2[1];
                        long idEng = finOffreClient.UpdateEngagementTraite(codeOffre, versionOffre, typeOffre, dateD, dateF, 0,
                              0, codeFamille, 0, 0, GetUser(), "I");

                        //Deuxième étape mise à jours
                        for (int i = 2; i < tabParams1.Length; i++)
                        {
                            string[] tabParams3 = tabParams1[i].Split(new[] { "codeFamille" }, StringSplitOptions.None);
                            int montant = int.Parse(tabParams3[0]);
                            string codeFamille2 = tabParams3[1];
                            finOffreClient.AddEngagementFamille(idEng, dateD, dateF, codeFamille2, montant, montant, GetUser());
                            //Création des traités
                        }
                    }
                }
            }
            LoadMontantsEngagement(new ModeleEngagement(), model.IdeConnexiteEngagement);
            return PartialView("MontantEngagements", model.ModeleEngagement);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string traite, string argModelCommentForce, string tabGuid, string paramRedirect, string modeNavig, string addParamType, string addParamValue)
        {
            if (cible == "RechercheSaisie")
                if (GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE) == "AVNMD" ||
                    GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE) == "AVNRM")
                    cible = "AnMontantReference";
                else
                    return RedirectToAction(job, cible);
            if (cible == "AttentatGareat" || cible == "Quittance" || cible == "AnMontantReference")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var serviceContext=client.Channel;
                    serviceContext.SetTrace(new TraceDto
                    {
                        CodeOffre = codeOffre.PadLeft(9, ' '),
                        Version = Convert.ToInt32(version),
                        Type = type,
                        EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement),
                        NumeroOrdreDansEtape = 61,
                        NumeroOrdreEtape = 1,
                        Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement),
                        Risque = 0,
                        Objet = 0,
                        IdInventaire = 0,
                        Formule = 0,
                        Option = 0,
                        Niveau = string.Empty,
                        CreationUser = GetUser(),
                        PassageTag = "O",
                        PassageTagClause = string.Empty
                    });
                }
            }

            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            if (cible == "EngagementTraite")
                return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + traite + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
            else
                return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });

        }
        
        #region Méthodes Privées
        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            if (tId[2] == "P")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                }
                var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                switch (typeAvt)
                {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                        break;
                    default:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                        break;
                }

            }
            if (model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
                model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
            }

            #region Navigation Arbre
            SetArbreNavigation();
            #endregion
            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion
            //Chargement des engagements
            //LoadEngagements(tId[0], tId[1], tId[2]);
            LoadEngagementsConnexites(tId[0], tId[1], tId[2]);
        }
        private void LoadEngagementsConnexites(string codeOffre, string version, string type)
        {
            string numConnexiteEngagament = GetNumeroConnexite(codeOffre, version, type, CODE_ENGAGEMENT);
            model.NumConnexite = numConnexiteEngagament;
            //Contrats connexes
            if (!string.IsNullOrEmpty(numConnexiteEngagament))
            {
                string ideConnexiteEngagement = LoadContratsConnexes(type, numConnexiteEngagament);
                //Montant des engagements de connexités  
                model.ModeleEngagement = new ModeleEngagement();
                model.ModeleEngagement.AllTraitesEngagements = model.AllTraitesContratsConnexes;
                LoadMontantsEngagement(model.ModeleEngagement, ideConnexiteEngagement);

            }
        }
        private string LoadContratsConnexes(string type, string numConnexiteEngagament)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                var allTraitesContratsConnexes = new List<ModeleTraite>();
                model.ContratsConnexes = ModeleContratConnexe.GetContratsConnexes(finOffreClient.GetContratsConnexesTraite(type, CODE_ENGAGEMENT, numConnexiteEngagament));
                if (model.ContratsConnexes.Any())
                {
                    model.IdeConnexiteEngagement = model.ContratsConnexes[0].IdeConnexite.ToString();
                    foreach (var contratConnexe in model.ContratsConnexes)
                    {
                        contratConnexe.Traites = ModeleTraite.GetEngagementsTraites(finOffreClient.GetEngagementsTraites(contratConnexe.IdEngagaement));
                        allTraitesContratsConnexes.AddRange(contratConnexe.Traites);
                    }
                    model.AllTraitesContratsConnexes = allTraitesContratsConnexes;
                    model.ListCodeTraites = new List<ParametreDto>();
                    foreach (var traite in allTraitesContratsConnexes)
                    {
                        if (!(model.ListCodeTraites.Select(t => t.Code).ToList().Contains(traite.CodeTraite)))
                            model.ListCodeTraites.Add(new ParametreDto { Code = traite.CodeTraite, Libelle = traite.LibelleTraite });
                    }
                    foreach (var contratConnexe in model.ContratsConnexes)
                    {
                        contratConnexe.AllTraites = model.AllTraitesContratsConnexes;
                        contratConnexe.ListCodeTraites = model.ListCodeTraites;
                    }


                    //int count = model.AllTraitesContratsConnexes.Count;
                    int count = model.ListCodeTraites.Count;
                    decimal[] totalPartAlbingia = new decimal[count];
                    decimal[] totalPartTotale = new decimal[count];

                    for (int i = 0; i < count; i++)
                    {
                        foreach (var eng in model.ContratsConnexes)
                        {
                            //var traite = eng.Traites.Find(t => t.CodeTraite == model.AllTraitesContratsConnexes[i].CodeTraite);
                            var traite = eng.Traites.Find(t => t.CodeTraite == model.ListCodeTraites[i].Code);
                            totalPartAlbingia[i] += traite != null ? Convert.ToDecimal(traite.EngagementAlbingia) : 0;
                            totalPartTotale[i] += traite != null ? Convert.ToDecimal(traite.EngagementTotal) : 0;
                        }
                    }
                    model.TotalPartAlbingia = totalPartAlbingia;
                    model.TotalPartTotale = totalPartTotale;
                }
            }
            return model.IdeConnexiteEngagement;
        }
        private void LoadMontantsEngagement(ModeleEngagement modeleEngagement, string ideConnexiteEngagement)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                modeleEngagement.Engagements = (!string.IsNullOrEmpty(ideConnexiteEngagement) && ideConnexiteEngagement != "0") ? ModeleEngagementConnexite.GetEngagements(finOffreClient.GetEngagementsConnexite(ideConnexiteEngagement)) : new List<ModeleEngagementConnexite>();
                //Engagements = ModeleEngagementConnexite.GetEngagements(finOffreClient.GetEngagementsConnexite(ideConnexiteEngagement))
                foreach (var eng in modeleEngagement.Engagements)
                {
                    eng.Traites = ModeleTraite.GetEngagementsTraites(finOffreClient.GetEngagementsTraites(eng.IdEngagaement));
                }
                modeleEngagement.ListCodeTraites = new List<ParametreDto>();
                if (modeleEngagement.AllTraitesEngagements != null)
                    foreach (var traite in modeleEngagement.AllTraitesEngagements)
                    {
                        if (!(modeleEngagement.ListCodeTraites.Select(t => t.Code).ToList().Contains(traite.CodeTraite)))
                            modeleEngagement.ListCodeTraites.Add(new ParametreDto { Code = traite.CodeTraite, Libelle = traite.LibelleTraite });
                    }
                foreach (var eng in modeleEngagement.Engagements)
                {
                    eng.AllTraites = modeleEngagement.AllTraitesEngagements;
                    eng.ListCodeTraites = modeleEngagement.ListCodeTraites;
                }
            }
        }

        private void SetBandeauNavigation(string id)
        {
            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Contrat != null)
                {
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
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }
        private void SetArbreNavigation()
        {
            if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Engagement");
            }
        }

        private void SetProvGuidTab(string id)
        {
            this.model.Provenance = this.model.AllParameters.Provenance;
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
        //Validation des données avant la redirection
        //private bool CheckPeriodesDiscontinues(string codeContrat, string versionContrat, string type)
        //{
        //    bool toReturn = true;
        //    //Récupération du contrat
        //    using (var serviceContext = new AffaireNouvelleClient())
        //    {
        //        model.Contrat = serviceContext.GetContrat(codeContrat, versionContrat, type);
        //    }
        //    if (model.Contrat == null)
        //        toReturn = false;
        //    if (toReturn)
        //    {
        //        MiseAZero();
        //        LoadEngagementPeriodes(codeContrat, versionContrat, type);
        //        GetDateControle();
        //        string sDateCreationContrat = model.Contrat.DateCreationAnnee.ToString() + (model.Contrat.DateCreationMois < 10 ? "0" + model.Contrat.DateCreationMois.ToString() : model.Contrat.DateCreationMois.ToString()) + (model.Contrat.DateCreationJour < 10 ? "0" + model.Contrat.DateCreationJour.ToString() : model.Contrat.DateCreationJour.ToString());
        //        int iDateCreationContrat = int.Parse(sDateCreationContrat);
        //        if (model.EngagementPeriodes != null && model.EngagementPeriodes.Count > 0)
        //        {
        //            int datePrecedente = 1;
        //            var periodesActives = model.EngagementPeriodes.FindAll(elm => elm.Actif == "A");
        //            foreach (ModeleEngagementPeriode periode in periodesActives)
        //            {
        //                if (datePrecedente != 1)
        //                {
        //                    if (periode.DateDebut != datePrecedente + 1)
        //                    {
        //                        if (iDateCreationContrat >= model.DateControle)
        //                        {
        //                            throw new AlbFoncException("Il ne peut y avoir de périodes discontinues", trace: true, sendMail: true, onlyMessage: true);
        //                        }
        //                    }
        //                }
        //                datePrecedente = periode.DateFin;
        //            }
        //        }

        //    }
        //    return toReturn;
        //}


        #endregion
    }
}
