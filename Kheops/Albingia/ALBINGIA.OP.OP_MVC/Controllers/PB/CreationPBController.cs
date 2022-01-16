using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.PB;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using ALBINGIA.OP.OP_MVC.Models.Regularisation;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALBINGIA.OP.OP_MVC.Models.ModelesNavigationArbre;
using Albingia.Kheops.OP.Domain.Affaire;
using OP.WSAS400.DTO.Avenant;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.Framework.Common.AlbingiaExceptions;

namespace ALBINGIA.OP.OP_MVC.Controllers.PB
{
    public class CreationPBController : ControllersBase<ModelePBPage>
    {
        // GET: CreationPB
        public ActionResult Index(string id)
        {
            id = HttpUtility.UrlDecode(id);
            model.PageTitle = "Création de la PB";
            id = InitializeParams(id);
            bool isHisto = Model.ModeNavig == ModeConsultation.Historique.AsCode();
            // ignore hisot mode in this page
            Model.ModeNavig = ModeConsultation.Standard.AsCode();
            LoadInfoPage(id);
            if (model.Contrat.ReguleId == 0)
            {
                var folder = string.Format("{0}_{1}_{2}", model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(CultureInfo.InvariantCulture), model.Contrat.Type);
                var isReadOnly = !GetIsReadOnly(model.TabGuid, folder, model.Contrat.NumAvenant.ToString(CultureInfo.InvariantCulture));

                var isModifHorsAvn = GetIsModifHorsAvn(model.TabGuid, string.Format("{0}_{1}", folder, model.Contrat.NumAvenant.ToString(CultureInfo.InvariantCulture)));

                Common.CommonVerouillage.DeverrouilleFolder(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.Contrat.NumAvenant.ToString(), model.TabGuid, !isReadOnly && !isHisto, isReadOnly, isModifHorsAvn);
            }

            PresetContextContrat();
            return View(this.model);
        }

        [ErrorHandler]
        public ActionResult Step1_ChoixPeriode(string id)
        {
            id = HttpUtility.UrlDecode(id);
            model.PageTitle = "Choix de la période PB";
            id = InitializeParams(id);
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), id, model.NumAvenantPage);
            GetPeriodeCourtierInfos(id);

            PresetContextContrat();

            return View(model);
        }

        [ErrorHandler]
        public ActionResult ChangeExercice(string codeContrat, string version, string type, string codeAvn, string typeAvt, string exercice, string lotId, string reguleId, string regulMode, string deleteMod, string cancelMod)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                return SetNewPeriod(codeContrat, version, type, codeAvn, typeAvt, Convert.ToInt16(exercice), null, null, lotId, reguleId, regulMode, deleteMod, cancelMod, client);
            }
        }

        [ErrorHandler]
        public ActionResult ChangePeriode(string codeContrat, string version, string type, string codeAvn, string typeAvt, string periodeDeb, string periodeFin, string lotId, string reguleId, string regulMode, string deleteMod, string cancelMod)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                return SetNewPeriod(codeContrat, version, type, codeAvn, typeAvt, null, periodeDeb, periodeFin, lotId, reguleId, regulMode, deleteMod, cancelMod, client);
            }
        }

        [ErrorHandler]
        public void SupressionDatesRegularisation(string reguleId)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                serviceContext.DeleteReguleP(reguleId);
            }
        }


        #region Méthodes privées

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var CommonOffreClient = chan.Channel;
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
                    Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                    Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                };
                SetContentData(model.Contrat);
                SetBandeauNavigation(model.Contrat, id);

            }

            model.AvnMode = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNMODE);
            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULPB;

        }

        private void SetContentData(ContratDto contrat)
        {

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            using (var clientRegul = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var serviceContext = client.Channel;
                var serviceContextRegul = clientRegul.Channel;
                var result = serviceContext.GetInfoRegulPage(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type, model.NumAvenantPage);
                if (result != null)
                {
                    contrat.DateEffetAnnee = result.DateEffetAnnee;
                    contrat.DateEffetMois = result.DateEffetMois;
                    contrat.DateEffetJour = result.DateEffetJour;
                    contrat.FinEffetAnnee = result.FinEffetAnnee;
                    contrat.FinEffetMois = result.FinEffetMois;
                    contrat.FinEffetJour = result.FinEffetJour;
                    contrat.PeriodiciteCode = result.PeriodiciteCode;
                    contrat.PeriodiciteNom = result.PeriodiciteNom;
                    contrat.LibelleNatureContrat = result.LibelleNatureContrat;
                    contrat.PartAlbingia = result.PartAlbingia;
                    contrat.ProchaineEchAnnee = result.ProchaineEchAnnee;
                    contrat.ProchaineEchMois = result.ProchaineEchMois;
                    contrat.ProchaineEchJour = result.ProchaineEchJour;
                    contrat.CodeRegime = result.CodeRegime;
                    contrat.LibelleRegime = result.LibelleRegime;
                    contrat.Devise = result.Devise;
                    contrat.LibelleDevise = result.LibelleDevise;
                    contrat.CourtierGestionnaire = result.CourtierGestionnaire;
                    contrat.CourtierApporteur = result.CourtierApporteur;
                    contrat.NomCourtierGest = result.NomCourtierGest;
                    contrat.NomCourtierAppo = result.NomCourtierAppo;
                    contrat.SouscripteurCode = result.SouscripteurCode;
                    contrat.SouscripteurNom = result.SouscripteurNom;
                    contrat.GestionnaireCode = result.GestionnaireCode;
                    contrat.GestionnaireNom = result.GestionnaireNom;
                }


                model.Contrat.TypePolice = !string.IsNullOrEmpty(contrat.TypePolice) ? contrat.TypePolice : "S";
                if (contrat.DateEffetAnnee != 0 && contrat.DateEffetMois != 0 && contrat.DateEffetJour != 0)
                {
                    model.EffetGaranties = new DateTime(contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour);
                }
                else model.EffetGaranties = null;
                if (contrat.FinEffetAnnee != 0 && contrat.FinEffetMois != 0 && contrat.FinEffetJour != 0)
                {
                    model.FinEffet = new DateTime(contrat.FinEffetAnnee, contrat.FinEffetMois, contrat.FinEffetJour);
                    model.FinEffetHeure = AlbConvert.ConvertIntToTimeMinute(contrat.FinEffetHeure);
                }
                else if (contrat.DureeGarantie > 0)
                {
                    model.FinEffet = AlbConvert.GetFinPeriode(model.EffetGaranties, contrat.DureeGarantie, contrat.UniteDeTemps);
                    model.FinEffetHeure = new TimeSpan(23, 59, 0);
                }
                else model.FinEffet = null;

                if (contrat.ProchaineEchAnnee != 0 && contrat.ProchaineEchMois != 0 && contrat.ProchaineEchJour != 0)
                {
                    model.Echeance = new DateTime(contrat.ProchaineEchAnnee, contrat.ProchaineEchMois, contrat.ProchaineEchJour);
                }

                var regule = GetInfoPB(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.NumAvenantPage);
                model.Regularisations = regule != null && regule.Regularisations != null && regule.Regularisations.Any() ? regule.Regularisations.Where(x => x.RegulMode == "PB").ToList() : new List<ModeleLigneRegularisation>();
                model.Alertes = GetInfoAlertes(regule.Alertes);
                ParametreDto typeContrat = regule != null && regule.TypesContrat != null && regule.TypesContrat.Any() ? regule.TypesContrat.Find(el => el.Code == model.Contrat.TypePolice) : null;
                model.LibTypeContrat = typeContrat != null ? typeContrat.Descriptif : string.Empty;

                var listTypeRegul = serviceContextRegul.GetModeleTypeRegul(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.Contrat.NumAvenant.ToString());
                model.CanCreatePB = listTypeRegul.Any(x => x.Code.Equals("PB", StringComparison.InvariantCulture));
            }
        }

        private void SetBandeauNavigation(ContratDto contrat, string id)
        {
            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            model.AfficherBandeau = true;
            model.AfficherNavigation = model.AfficherBandeau;
            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULPB;
            model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
            //Gestion des Etapes
            model.Navigation = new Navigation_MetaModel
            {
                Etape = Navigation_MetaModel.ECRAN_REGULE,
                IdOffre = model.Contrat.CodeContrat,
                Version = int.Parse(model.Contrat.VersionContrat.ToString()),
            };

            model.NavigationArbre = GetNavigationArbreRegule(model, "Regule");

            model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
        }

        protected override ModeleNavigationArbre GetNavigationArbreRegule(MetaModelsBase contentData, string etape)
        {
            contentData.NavigationArbre = new ModeleNavigationArbre();
            if (contentData.Contrat != null && contentData.Contrat.CodeContrat != null)
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var commonOffreClient = channelClient.Channel;
                    contentData.NavigationArbre = (ModeleNavigationArbre)commonOffreClient.GetHierarchy(contentData.Contrat.CodeContrat, int.Parse(contentData.Contrat.VersionContrat.ToString()), contentData.Contrat.Type, contentData.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>(), model.ActeGestionRegule);
                }
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaire>())
                {
                    if (contentData.NumAvenantPage.ParseInt().Value > 0)
                    {
                        var alertesAvenant = client.Channel.GetListAlertesAvenant(new AffaireId
                        {
                            CodeAffaire = contentData.Contrat.CodeContrat,
                            IsHisto = contentData.ModeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique,
                            NumeroAliment = (int)contentData.Contrat.VersionContrat,
                            TypeAffaire = AffaireType.Contrat,
                            NumeroAvenant = contentData.NumAvenantPage.ParseInt().Value
                        });
                        this.model.NavigationArbre.AlertesAvenant = CreationAvenantController.GetInfoAlertes(new AvenantDto { Alertes = alertesAvenant });
                    }
                }
            }

            contentData.NavigationArbre.Etape = etape;
            contentData.NavigationArbre.ModeNavig = contentData.ModeNavig;
            contentData.NavigationArbre.IsReadOnly = contentData.IsReadOnly;
            contentData.NavigationArbre.ScreenType = contentData.ScreenType;
            contentData.NavigationArbre.IsValidation = contentData.IsValidation;
            var data = contentData as ModelePBPage;
            if (data?.Context != null)
            {
                RegularisationNavigator.Initialize(contentData.NavigationArbre, data.Context);
            }

            return contentData.NavigationArbre;
        }

        private ModeleRegularisation GetInfoPB(string codeContrat, string version, string type, string codeAvn)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetInfoRegularisation(codeContrat, version, type, codeAvn, GetUser());

                if (result != null)
                {
                    ModeleRegularisation regularisation = (ModeleRegularisation)result;
                    return regularisation;
                }
                return null;
            }
        }

        private List<ModeleAvenantAlerte> GetInfoAlertes(List<ModeleAvenantAlerte> Alertes)
        {
            GetLienAlerte(Alertes);
            return Alertes != null && Alertes.Any() ? Alertes : new List<ModeleAvenantAlerte>();
        }

        private static void GetLienAlerte(List<ModeleAvenantAlerte> Alertes)
        {
            if (Alertes != null && Alertes.Any())
            {
                foreach (ModeleAvenantAlerte elm in Alertes)
                {
                    switch (elm.TypeAlerte)
                    {
                        case AlbOpConstants.SUSPEN:
                            elm.LienMessage = "Visu des suspensions";
                            break;
                        case AlbOpConstants.QUITT:
                            elm.LienMessage = "Visu des quittances";
                            break;
                        default:
                            elm.LienMessage = "Voir";
                            break;
                    }
                }
            }
        }

        private void GetPeriodeCourtierInfos(string id)
        {
            string[] tId = id.Split('_');
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext = client.Channel;
                var infosBase = serviceContext.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
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
                    IsTemporaire = infosBase.IsTemporaire,
                    Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                    Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur,
                    NumAvenant = infosBase.NumAvenant
                };
            }

            SetContentData(model.Contrat);

            model.AvnMode = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNMODE);
            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            var lotId = GetAddParamValue(model.AddParamValue, AlbParameterName.LOTID);

            var regulType = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULTYP);

            var reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);
            var acteGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
            var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);

            var isReadonly = GetIsReadOnly(model.TabGuid, tId[0] + "_" + tId[1] + "_" + tId[2], (model.Contrat.NumAvenant).ToString(), modeAvenant: model.AvnMode);
            model.IsReadOnly = isReadonly;

            model.InfoPB = GetInfoCreatePB(tId[0], tId[1], tId[2], typeAvt, model.AvnMode, isReadonly || model.AvnMode == "CONSULT", model.AvnMode == "CONSULT" ? string.Empty : lotId, reguleId, regulMode);

            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULPB;

            if (!string.IsNullOrEmpty(acteGestionRegule))
            {
                model.AddParamValue = model.AddParamValue.Replace("ACTEGESTIONREGULE|AVNMD", "ACTEGESTIONREGULE|REGUL");
            }
            else
            {
                model.AddParamValue += "||ACTEGESTIONREGULE|REGUL";
            }

            model.ActeGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
            RegularisationNavigator.StandardInitContext(model, RegularisationStep.ChoixPeriodeCourtier);
            SetBandeauNavigation(model.Contrat, id);
        }

        private ModeleCreationRegule GetInfoCreatePB(string codeContrat, string version, string type, string typeAvt, string modeAvt, bool isReadonly, string lotId, string reguleId, string regulMode)
        {
            ModeleCreationRegule model = new ModeleCreationRegule
            {
                ModeAvt = modeAvt,
                TypeAvt = typeAvt,
                Alertes = new List<ModeleAvenantAlerte>()
            };

            RegularisationInfoDto result;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                result = client.Channel.Init(
                    new Folder { CodeOffre = codeContrat, Version = int.Parse(version), Type = type },
                    new RegularisationContext
                    {
                        User = GetUser(),
                        TypeAvt = typeAvt,
                        Mode = regulMode.ParseCode<RegularisationMode>(),
                        LotId = long.TryParse(lotId, out long x) ? x : default,
                        RgId = long.TryParse(reguleId, out long y) ? y : default,
                        IsReadOnlyMode = isReadonly
                    });
            }

            if (result != null)
            {
                model = (ModeleCreationRegule)result;
                if (isReadonly)
                {
                    model.ModeAvt = modeAvt;
                    model.IsReadOnly = true;
                    base.model.IsReadOnly = true;
                    model.Alertes = GetInfoAlertes(this.model.Alertes);
                }
                else
                {
                    model.ModeAvt = modeAvt;
                    model.IsReadOnly = false;
                    model.Alertes = GetInfoAlertes(this.model.Alertes);
                }

                List<AlbSelectListItem> courtiers = new List<AlbSelectListItem>();
                result.Courtiers.ForEach(el => courtiers.Add(new AlbSelectListItem { Value = el.Id.ToString(), Text = el.Id + " - " + el.Descriptif, Selected = false, Title = el.Id + " - " + el.Descriptif }));
                model.Courtiers = courtiers;
                List<AlbSelectListItem> quittancements = new List<AlbSelectListItem>();
                result.Quittancements.ForEach(el => quittancements.Add(new AlbSelectListItem { Value = el.Code, Text = el.Code + " - " + el.Libelle, Selected = false, Title = el.Code + " - " + el.Libelle }));
                model.Quittancements = quittancements;

                List<AlbSelectListItem> motifs = new List<AlbSelectListItem>();
                result.Motifs.ForEach(m => motifs.Add(new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Code + " - " + m.Libelle }));
                model.Motifs = motifs;

                if (!string.IsNullOrEmpty(model.RetourPGM))
                {
                    var err = result.RetourPGM.Split('_')[2];
                    if (!string.IsNullOrEmpty(err))
                    {
                        model.ErreurPGM = GetErreurMsg(err);
                    }
                }

                if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_PB)
                {
                    model.ReguleId = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0;
                }
            }

            return model;
        }

        private ActionResult SetNewPeriod(string codeContrat, string version, string type, string codeAvn, string typeAvt, int? exercice, string periodeDeb, string periodeFin, string lotId, string reguleId, string regulMode, string deleteMod, string cancelMod, Framework.Common.ServiceFactory.ServiceClientFactory.Client<IRegularisation> client)
        {
            var result = client.Channel.Init(
                new Folder { CodeOffre = codeContrat, Version = int.Parse(version), Type = type, NumeroAvenant = int.Parse(codeAvn) },
                new RegularisationContext
                {
                    Exercice = exercice.GetValueOrDefault(),
                    DateDebut = periodeDeb,
                    DateFin = periodeFin,
                    User = GetUser(),
                    TypeAvt = typeAvt,
                    Mode = regulMode.ParseCode<RegularisationMode>(),
                    LotId = long.TryParse(lotId, out long x) ? x : default,
                    RgId = long.TryParse(reguleId, out long y) ? y : default
                },
                deleteMod.ContainsChars() ? CauseResetRegularisation.Delete : cancelMod.ContainsChars() ? CauseResetRegularisation.Cancel : CauseResetRegularisation.Reset);

            if (result is null || string.IsNullOrWhiteSpace(result.RetourPGM))
            {
                throw new AlbFoncException("Plage de dates invalide", true, true, true);
            }
            var codeErr = result.RetourPGM.Split('_')[2];
            if (!int.TryParse(codeErr, out var i) && codeErr.ContainsChars())
            {
                throw new AlbFoncException(codeErr, true, true, true);
            }

            if (codeErr.ContainsChars())
            {
                var errMsg = GetErreurMsg(codeErr);
                throw new AlbFoncException(errMsg, true, true, true);
            }

            var model = (ModeleCreationRegule)result;
            if (!string.IsNullOrWhiteSpace(reguleId) && reguleId != "0")
            {
                model.ReguleId = Convert.ToInt32(reguleId);
            }

            model.Alertes = GetInfoAlertes(model.Alertes);
            List<AlbSelectListItem> courtiers = new List<AlbSelectListItem>();
            result.Courtiers.ForEach(el => courtiers.Add(new AlbSelectListItem { Value = el.Id.ToString(), Text = el.Id.ToString() + " - " + el.Descriptif, Selected = false, Title = el.Id.ToString() + " - " + el.Descriptif }));
            model.Courtiers = courtiers;
            List<AlbSelectListItem> quittancements = new List<AlbSelectListItem>();
            result.Quittancements.ForEach(el => quittancements.Add(new AlbSelectListItem { Value = el.Code, Text = el.Code + " - " + el.Libelle, Selected = false, Title = el.Code + " - " + el.Libelle }));
            model.Quittancements = quittancements;
            model.LotId = result.LotId;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private void PresetContextContrat()
        {
            model.Context = new RegularisationContext()
            {
                IdContrat = new IdContratDto()
                {
                    CodeOffre = model.Contrat.CodeContrat,
                    Version = int.Parse(model.Contrat.VersionContrat.ToString()),
                    Type = model.Contrat.Type
                }
            };
        }

        private string GetErreurMsg(string codeErreur)
        {
            var errMsg = string.Empty;
            switch (codeErreur)
            {
                case "01":
                    errMsg = "Plage de dates invalide";
                    break;
                case "02":
                    errMsg = "Dernier avenant non validé";
                    break;
                case "03":
                    errMsg = "Période > à la prochaine échéance";
                    break;
                case "04":
                    errMsg = "Changement de nature du contrat dans la période";
                    break;
                case "05":
                    errMsg = "Changement de part du contrat dans la période";
                    break;
                case "06":
                    errMsg = "Changement de coassureurs dans la période";
                    break;
                default:
                    errMsg = string.Empty;
                    break;
            }

            return errMsg;
        }

        #endregion
    }
}