using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class CreationAvenantController : ControllersBase<ModeleCreationAvenantPage>
    {
        private const string BorderValuePrefix = "|###||";
        private const string BorderValueSuffix = "||###|";

        #region Méthodes publiques

        [ErrorHandler]
        [AlbVerifLockedOffer("id")]
        public ActionResult Index(string id)
        {
            #region CR809 changes

            string oldIdParam = id;
            string[] tId = id.Split('_');
            id = InitializeParams(id);
            #endregion
            LoadInfoPage(id);

            if (model.TypeAvt == AlbConstantesMetiers.TYPE_AVENANT_RESIL)
            {
                model.FinEffet = model.AvenantResiliation.FinGarantieHisto;
            }

            return View(model);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string codeAvenant, string tabGuid,
            string addParamType, string addParamValue, string codeAvenantExterne, string modeNavig, string reguleId)
        {
            int.TryParse(codeAvenant, out int codeAvn);

            string modeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNMODE);
            if (modeAvt == "CREATE")
            {
                var folder = string.Format("{0}_{1}_{2}", codeOffre, version, type);
                var numAvn = (codeAvn - 1).ToString(CultureInfo.InvariantCulture);
                var isReadOnly = GetIsReadOnly(tabGuid, folder, numAvn);
                var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, numAvn));
            }

            if (cible == "RechercheSaisie")
            {
                return RedirectToAction("Index", "RechercheSaisie", new { id = codeOffre + "_" + version + "_" + type + "_loadParam" + tabGuid });
            }
            else
            {
                var isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn.ToString())
                    || ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>();

                if (isReadOnly)
                {
                    return RedirectToAction(job, cible, new { id = string.Format("{0}_{1}_{2}_{3}{4}{5}{6}", codeOffre, version, type, codeAvenant, tabGuid, "addParam" + addParamType + "|||" + addParamValue + "addParam", "modeNavig" + modeNavig + "modeNavig") });
                }

                string typeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
                string acteGestionRegule = GetAddParamValue(addParamValue, AlbParameterName.ACTEGESTIONREGULE);
                string addParam = "addParam" + AlbOpConstants.GLOBAL_TYPE_ADD_PARAM_AVN + "|||" +
                                        AlbParameterName.AVNID + "|" + codeAvenant +
                                        "||" + AlbParameterName.AVNTYPE + "|" + typeAvt +
                                        "||" + AlbParameterName.AVNIDEXTERNE + "|" + codeAvenantExterne +
                                        "||" + AlbParameterName.AVNMODE + "|UPDATE" + // force update after first screen
                                        (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? "||" + AlbParameterName.REGULEID + "|" + reguleId : string.Empty) +
                                        (!string.IsNullOrEmpty(acteGestionRegule) ? "||" + AlbParameterName.ACTEGESTIONREGULE + "|" + acteGestionRegule : string.Empty) +
                                        "addParam";

                var myId = string.Format("{0}_{1}_{2}_{3}{4}{5}", codeOffre, version, type, codeAvenant, tabGuid, addParam);

                if (cible == "AnnulationQuittances")
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        var serviceContext=client.Channel;
                        var result = serviceContext.ContratHasQuittances(codeOffre, version, type);
                        if (!result)
                            cible = "EngagementPeriodes";
                    }
                }

                return RedirectToAction(job, cible, new { id = myId });
            }
        }

        [ErrorHandler]
        public JsonResult CreateAvenant(string codeOffre, string version, string type, string typeAvt, string modeAvt,
            string souscripteur, string gestionnaire,
            string argModeleAvtModif, string argModeleAvtResil, string argModeleRemiseVig)
        {
            ModeleAvenantModification modeleAvtModif = null;
            ModeleAvenantResiliation modeleAvtResil = null;
            ModeleAvenantRemiseVigueur modeleRemiseVig = null;


            if ((typeAvt == AlbConstantesMetiers.TYPE_AVENANT_MODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF) && !string.IsNullOrEmpty(argModeleAvtModif))
            {
                JavaScriptSerializer serializerModelAvtModif = AlbJsExtendConverter<ModeleAvenantModification>.GetSerializer();
                modeleAvtModif = serializerModelAvtModif.ConvertToType<ModeleAvenantModification>(serializerModelAvtModif.DeserializeObject(argModeleAvtModif));
            }
            if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_RESIL && !string.IsNullOrEmpty(argModeleAvtResil))
            {
                JavaScriptSerializer serializerModelAvtResil = AlbJsExtendConverter<ModeleAvenantResiliation>.GetSerializer();
                modeleAvtResil = serializerModelAvtResil.ConvertToType<ModeleAvenantResiliation>(serializerModelAvtResil.DeserializeObject(argModeleAvtResil));
            }
            if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR && !string.IsNullOrEmpty(argModeleRemiseVig))
            {
                JavaScriptSerializer serializerModelAvtRemiseVig = AlbJsExtendConverter<ModeleAvenantRemiseVigueur>.GetSerializer();
                modeleRemiseVig = serializerModelAvtRemiseVig.ConvertToType<ModeleAvenantRemiseVigueur>(serializerModelAvtRemiseVig.DeserializeObject(argModeleRemiseVig));
            }

            if (modeleAvtModif != null || modeleAvtResil != null || modeleRemiseVig != null)
            {
                if (modeleAvtModif != null && modeleAvtModif.DateEffetAvt.HasValue)
                {
                    modeleAvtModif.DateEffetAvt = modeleAvtModif.DateEffetAvt.Value.AddHours(modeleAvtModif.HeureEffetAvt?.Hours ?? 0);
                    modeleAvtModif.DateEffetAvt = modeleAvtModif.DateEffetAvt.Value.AddMinutes(modeleAvtModif.HeureEffetAvt?.Minutes ?? 0);
                }
                //modeleAvtModif.DateEffetAvt?.AddHours(modeleAvtModif.HeureEffetAvt?.Hours ?? 0).AddMinutes(modeleAvtModif.HeureEffetAvt?.Minutes ?? 0);

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAvenant>()) {
                    try
                    {
                        var folder = new Folder { CodeOffre = codeOffre, Version = int.Parse(version), Type = type };
                        switch (typeAvt)
                        {
                            case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                                if (modeAvt == AccessMode.CREATE.ToString() && modeleAvtModif.TypeAvt == AlbConstantesMetiers.TYPE_AVENANT_MODIF)
                                {
                                    return Json(client.Channel.CreationAvenantModif(
                                        codeOffre,
                                        folder.Version,
                                        modeleAvtModif.DateEffetAvt?.ToString("s"),
                                        modeleAvtModif.DescriptionAvt,
                                        modeleAvtModif.ObservationsAvt));
                                }
                                return Json(client.Channel.CreateOrUpdate(folder, typeAvt, modeAvt, ModeleAvenantModification.LoadDto(modeleAvtModif), null, null, null, GetUser()));
                            case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                                return Json(client.Channel.CreateOrUpdate(folder, typeAvt, modeAvt, null, ModeleAvenantResiliation.LoadDto(modeleAvtResil), null, null, GetUser()));
                            case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                                return Json(client.Channel.CreateOrUpdate(folder, typeAvt, modeAvt, null, null, ModeleAvenantRemiseVigueur.LoadDto(modeleRemiseVig), null, GetUser()));
                        }
                    }
                    catch (Exception ex) {
#if DEBUG
                        throw new AlbFoncException($"Erreur lors de la mise en historique:{Environment.NewLine}{ex}", trace: true, onlyMessage: true);
#else
                        throw new AlbFoncException("Erreur lors de la mise en historique", trace: true, sendMail: true, onlyMessage: true);
#endif
                    }
                }
            }
            return Json(new[] { "Erreur lors de la mise en historique" });
        }

        [ErrorHandler]
        public ActionResult ReloadAvenantResil()
        {
            ModeleAvenantResiliation model = new ModeleAvenantResiliation { Motifs = new List<AlbSelectListItem>() };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.ReloadAvenantResilMotif();

                if (result != null && result.Any())
                {
                    List<AlbSelectListItem> motifs = new List<AlbSelectListItem>();
                    result.ForEach(m => motifs.Add(new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Code + " - " + m.Libelle }));
                    model.Motifs = motifs;
                }
            }
            return PartialView("AvenantResilMotif", model);
        }

        #endregion

        #region Méthodes privées

        private void InitializeRemiseEnVigueur()
        {
            using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRemiseEnVigueur>())
            {
                var revClient = chan.Channel;
                var infosBase = revClient.InitializeRemiseEnVigueurParameters(model.Contrat.CodeContrat, (int)model.Contrat.VersionContrat, model.Contrat.Type);

                //model.AvenantRemiseEnVigueur.TypeGestion = infosBase.Type;
            }

        }

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            //model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type); //false;
            model.TypeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
           // model.NumAvenantPage = GetAddParamValue(model.AddParamValue, AlbNameVarAddParam.AVNID);
            if (model.TypeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
            {
                if (!string.IsNullOrEmpty(model.AddParamValue))
                {
                    model.AddParamValue = model.AddParamValue.Replace("AVNMODE|CREATE", "AVNMODE|UPDATE");
                }
                else
                {
                    model.AddParamValue += "||AVNMODE|UPDATE";
                }
            }
            model.ModeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNMODE);
            model.LayoutModeAvt = model.ModeAvt;
            switch (model.TypeAvt)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                    break;

            }

            using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
             var CommonOffreClient=chan.Channel;
                var infosBase = CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
                model.Contrat = new ContratDto()
                {
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
                    Etat = infosBase.Etat,
                    Situation = infosBase.Situation,
                    NatureContrat = infosBase.Nature,
                    LibelleNatureContrat = infosBase.LibNature,
                    Observations = infosBase.Observation,
                    NumAvenant = infosBase.NumAvenant,
                    Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                    Inspecteur = infosBase.CabinetGestionnaire?.Inspecteur
                };

                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type); //false;
                //Réinitialisation de l'Id de régule si mode création
                if (model.ModeAvt == "CREATE")
                {
                    model.Contrat.ReguleId = 0;
                }
                if (model.IsReadOnly)
                    model.ModeAvt = "UPDATE";
            }


            SetContentData(model.Contrat);

            model.Navigation = new Navigation_MetaModel
            {
                Etape = Navigation_MetaModel.ECRAN_INFOGENERALE
            };
            if (model.ModeAvt == "")
                model.ModeAvt = "UPDATE";
            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoSaisie", returnEmptyTree: true);

            if (!string.IsNullOrEmpty(GetAddParamValue(model.AddParamValue, AlbParameterName.AVNID)))
            {
                model.Bandeau = null;
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
                model.AfficherArbre = true;
                if (model.AfficherBandeau)
                    model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                model.Bandeau.StyleBandeau = model.ScreenType;
            }

            model.LayoutModeAvt = model.ModeAvt;

            #region Gestion de l'acte de Gestion de la régule

            if (model.TypeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
            {
                var acteGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
                if (!string.IsNullOrEmpty(acteGestionRegule))
                {
                    model.AddParamValue = model.AddParamValue.Replace("ACTEGESTIONREGULE|REGUL", "ACTEGESTIONREGULE|AVNMD");
                }
                else
                {
                    model.AddParamValue += "||ACTEGESTIONREGULE|AVNMD";
                }
                model.ActeGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
            }

            #endregion
        }

        private void SetContentData(ContratDto contrat)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetInfoAvenantPage(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type,model.NumAvenantPage, model.TypeAvt, model.ModeAvt, GetUser(), model.ModeNavig);
                if (result != null)
                {
                    if (result.contrat != null)
                    {
                        model.Contrat.DateEffetAnnee = result.contrat.DateEffetAnnee;
                        model.Contrat.DateEffetMois = result.contrat.DateEffetMois;
                        model.Contrat.DateEffetJour = result.contrat.DateEffetJour;
                        model.Contrat.DateEffetHeure = result.contrat.DateEffetHeure;
                        model.Contrat.FinEffetAnnee = result.contrat.FinEffetAnnee;
                        model.Contrat.FinEffetMois = result.contrat.FinEffetMois;
                        model.Contrat.FinEffetJour = result.contrat.FinEffetJour;
                        model.Contrat.FinEffetHeure = result.contrat.FinEffetHeure;
                        model.Contrat.NomCourtierGest = result.contrat.NomCourtierGest;
                        model.Contrat.SouscripteurCode = result.contrat.SouscripteurCode;
                        model.Contrat.SouscripteurNom = result.contrat.SouscripteurNom;
                        model.Contrat.GestionnaireCode = result.contrat.GestionnaireCode;
                        model.Contrat.GestionnaireNom = result.contrat.GestionnaireNom;
                        model.Contrat.TypeRetour = result.contrat.TypeRetour;
                        model.Contrat.LibRetour = result.contrat.LibRetour;
                        model.Contrat.PeriodiciteCode = result.contrat.PeriodiciteCode;
                        model.Contrat.PeriodiciteNom = result.contrat.PeriodiciteNom;
                        model.Contrat.ProchaineEchAnnee = result.contrat.ProchaineEchAnnee;
                        model.Contrat.ProchaineEchMois = result.contrat.ProchaineEchMois;
                        model.Contrat.ProchaineEchJour = result.contrat.ProchaineEchJour;
                        model.Contrat.DateEffetAvenant = AlbConvert.ConvertIntToDate(result.contrat.DateEffetAnnee * 10000 + result.contrat.DateEffetMois * 100 + result.contrat.DateEffetJour);
                        model.Contrat.ReguleId = result.contrat.ReguleId;

                        model.Contrat.TypePolice = !string.IsNullOrEmpty(result.contrat.TypePolice) ? result.contrat.TypePolice : "S";

                        if (result.contrat.DateEffetAnnee != 0 && result.contrat.DateEffetMois != 0 && result.contrat.DateEffetJour != 0) {
                            model.EffetGaranties = new DateTime(result.contrat.DateEffetAnnee, result.contrat.DateEffetMois, result.contrat.DateEffetJour);
                            this.model.HeureEffetGaranties = result.contrat.DateEffetHeure.ToString().PadLeft(4, '0');
                        }
                        else {
                            model.EffetGaranties = null;
                        }
                        if (result.contrat.FinEffetAnnee != 0 && result.contrat.FinEffetMois != 0 && result.contrat.FinEffetJour != 0)
                        {
                            model.FinEffet = new DateTime(result.contrat.FinEffetAnnee, result.contrat.FinEffetMois, result.contrat.FinEffetJour);
                            model.FinEffetHeure = AlbConvert.ConvertIntToTimeMinute(result.contrat.FinEffetHeure);
                        }
                        else if (result.contrat.DureeGarantie > 0)
                        {
                            model.FinEffet = AlbConvert.GetFinPeriode(model.EffetGaranties, result.contrat.DureeGarantie, result.contrat.UniteDeTemps);
                            model.FinEffetHeure = new TimeSpan(23, 59, 0);
                        }
                        else model.FinEffet = null;

                        if (result.contrat.ProchaineEchAnnee != 0 && result.contrat.ProchaineEchMois != 0 && result.contrat.ProchaineEchJour != 0)
                        {
                            model.Echeance = new DateTime(result.contrat.ProchaineEchAnnee, result.contrat.ProchaineEchMois, result.contrat.ProchaineEchJour);
                        }
                        model.Alertes = new List<ModeleAvenantAlerte>();
                    }

                    var modeAvt = model.ModeAvt?.ToUpper() == "CREATE" ? "Création" : "Modif.";
                    switch (model.TypeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.AvenantModification = GetInfoAvenantModif(result.avenant);
                            model.PageTitle = string.Format("{0} acte de gestion n° {1}", modeAvt, model.AvenantModification.NumInterneAvt);
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.AvenantModification = GetInfoAvenantModif(result.avenant);
                            model.PageTitle = string.Format("{0} acte de gestion n° {1}", modeAvt, model.AvenantModification.NumInterneAvt);
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.AvenantResiliation = GetInfoAvenantResil(result.avenant);

                            model.PageTitle = string.Format("{0} acte de gestion n° {1}", modeAvt, model.AvenantResiliation.NumInterneAvt);
                            break;

                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.AvenantRemiseEnVigueur = GetInfoAvenantRemiseEnVigueur(result.avenant);
                            model.PageTitle = string.Format("{0} acte de gestion n° {1}", modeAvt, model.AvenantRemiseEnVigueur.NumInterneAvt);

                            break;
                        default:
                            break;
                    }
                    model.Alertes = GetInfoAlertes(result.avenant);
                    ParametreDto typeContrat = result.avenant.TypesContrat.Find(el => el.Code == model.Contrat.TypePolice);
                    model.LibTypeContrat = typeContrat != null ? typeContrat.Descriptif : string.Empty;
                }
            }

        }

        private ModeleAvenantModification GetInfoAvenantModif(AvenantDto result)
        {
            ModeleAvenantModification modele = new ModeleAvenantModification();
            if (result.AvenantModif != null)
            {
                modele = (ModeleAvenantModification)result.AvenantModif;
                List<AlbSelectListItem> motifs = new List<AlbSelectListItem>();
                result.AvenantModif.Motifs.ForEach(m => motifs.Add(new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Code + " - " + m.Libelle }));
                modele.Motifs = motifs;
                model.NumAvenantExterne = modele.NumAvt.ToString();
            }

            if (model.ModeAvt == "CREATE")
            {
                //if (model.Contrat.Etat == "V" && model.Contrat.Situation == "X" && (model.Contrat.PeriodiciteCode == "U" || model.Contrat.PeriodiciteCode == "E"))
                //{
                //    if (AlbConvert.ConvertTimeToInt(model.FinEffetHeure) == 0 || AlbConvert.ConvertTimeToIntMinute(model.FinEffetHeure) == 2359)
                //    {
                //        modele.DateEffetAvt = model.FinEffet.Value.AddDays(1);
                //        modele.HeureEffetAvt = new TimeSpan(0, 0, 0);
                //    }
                //    else
                //    {
                //        modele.DateEffetAvt = model.FinEffet.Value;
                //        TimeSpan ts = TimeSpan.FromMinutes(1);
                //        modele.HeureEffetAvt = model.FinEffetHeure.Value.Add(ts);
                //    }
                //}
                //else
                //{
                modele.DateEffetAvt = null;
                modele.HeureEffetAvt = null;
                //}
                if (modele.Motifs.Exists(elm => elm.Value == "M1"))
                    modele.MotifAvt = "M1";
                modele.DescriptionAvt = string.Empty;
            }
            else
            {
                if (modele.DateEffetAvt == null)
                    modele.DateEffetAvt = model.EffetGaranties;
                if (modele.HeureEffetAvt == null)
                    modele.HeureEffetAvt = new TimeSpan(0);
            }

            modele.TypeAvt = model.TypeAvt;
            modele.ModeAvt = model.ModeAvt;

            modele.Etat = model.Contrat.Etat;
            modele.Situation = model.Contrat.Situation;
            modele.Periodicite = model.Contrat.PeriodiciteCode;

            return modele;
        }

        private ModeleAvenantResiliation GetInfoAvenantResil(AvenantDto result)
        {
            ModeleAvenantResiliation modele = new ModeleAvenantResiliation();
            if (result.AvenantResil != null)
            {
                modele = (ModeleAvenantResiliation)result.AvenantResil;
                List<AlbSelectListItem> motifs = new List<AlbSelectListItem>();
                result.AvenantResil.Motifs.ForEach(m => motifs.Add(new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Code + " - " + m.Libelle }));
                modele.Motifs = motifs;

                List<AlbSelectListItem> datesFin = new List<AlbSelectListItem>();
                result.AvenantResil.DatesFin.ForEach(m => datesFin.Add(new AlbSelectListItem { Value = m.Code, Text = m.Libelle, Selected = false, Title = m.Libelle }));
                modele.DatesFin = datesFin;

                //SAB bug2022
                if (model.ModeAvt == "CREATE")
                {
                    modele.DescriptionAvt = string.Empty;
                }
                model.NumAvenantExterne = modele.NumAvt.ToString();
            }


            if (modele.DateFinGarantie == null)
                modele.DateFinGarantie = model.FinEffet;

            modele.TypeAvt = model.TypeAvt;
            modele.ModeAvt = model.ModeAvt;
            modele.AvenantEch = model.Contrat.IsCheckedEcheance;

            //modele.AvenantEch = GetAddParamValue(model.AddParamValue, AlbNameVarAddParam.AVNTYPERESIL) == "0";
            if (modele.AvenantEch)
            {
                modele.HeureFinGarantie = new TimeSpan(23, 59, 0);
            }
            else
            {
                if (modele.DateFinGarantie == null)
                    modele.HeureFinGarantie = null;
            }
            modele.DateFin = modele.DateFinGarantie.HasValue ? AlbConvert.ConvertDateToStr(modele.DateFinGarantie.Value) : string.Empty;
            return modele;
        }
        private ModeleAvenantRemiseVigueur GetInfoAvenantRemiseEnVigueur(AvenantDto result)
        {
            ModeleAvenantRemiseVigueur modele = new ModeleAvenantRemiseVigueur();
            if (result.AvenantRemiseEnVigueur != null)
            {
                modele = (ModeleAvenantRemiseVigueur)result.AvenantRemiseEnVigueur;
                model.NumAvenantExterne = modele.NumAvt.ToString();
            }


            //modele.DateResil = modele.DateResil.HasValue ? AlbConvert.ConvertDateToStr(modele.DateResil.Value) : string.Empty;

            modele.TypeAvt = model.TypeAvt;
            modele.ModeAvt = model.ModeAvt;

            modele.Etat = model.Contrat.Etat;
            modele.Situation = model.Contrat.Situation;
            modele.Periodicite = model.Contrat.PeriodiciteCode;
            return modele;
        }
        internal static List<ModeleAvenantAlerte> GetInfoAlertes(AvenantDto result)
        {
            ModeleAvenant avenant = (ModeleAvenant)result;
            GetLienAlerte(avenant);
            return avenant.Alertes != null && avenant.Alertes.Any() ? avenant.Alertes : new List<ModeleAvenantAlerte>();
        }

        private static void GetLienAlerte(ModeleAvenant avenant)
        {
            if (avenant.Alertes != null && avenant.Alertes.Any())
            {
                foreach (ModeleAvenantAlerte elm in avenant.Alertes)
                {
                    switch (elm.TypeAlerte)
                    {
                        case AlbOpConstants.SUSPEN:
                            elm.LienMessage = "Visu des suspensions";
                            break;
                        case AlbOpConstants.QUITT:
                            elm.LienMessage = "Visu des quittances";
                            break;
                        case AlbOpConstants.ENGCNX:
                            elm.LienMessage = "Visu des connexites d'engagement";
                            break;
                        case AlbOpConstants.INTERV:
                            elm.LienMessage = "";
                            break;
                        default:
                            elm.LienMessage = "Voir";
                            break;
                    }
                }
            }
        }

        #endregion

    }
}
