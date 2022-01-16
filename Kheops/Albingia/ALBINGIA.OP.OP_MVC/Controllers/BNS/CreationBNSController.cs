using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.BNS;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ALBINGIA.OP.OP_MVC.Controllers.BNS
{
    public class CreationBNSController : ControllersBase<ModeleBNSPage>
    {
        [ErrorHandler]
        // GET: CreationBNS
        public RedirectToRouteResult Index(string id)
        {
            id = HttpUtility.UrlDecode(id);
            model.PageTitle = "BNS";
            id = InitializeParams(id);
            //return null;
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), id, model.NumAvenantPage);
            GetPeriodeCourtierInfos(id);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                model.Context = client.Channel.EnsureContext(model.Context);
                model.Context.User = GetUser();
                model.Context.ModeleAvtRegul = new AvenantRegularisationDto()
                {
                    TypeAvt = "REGUL",
                    NumAvt = model.Contrat.NumAvenant,
                    NumInterneAvt = model.Contrat.NumInterneAvenant,
                    ObservationsAvt = "",
                    MotifAvt = "",
                    DescriptionAvt = ""
                };
                model.Context = client.Channel.ValidateStepAndGetNext(model.Context);
            }

            //PresetContextContrat();
            return RedirectToAction("CalculBNS" + model.Context.Scope.ToString(), "Regularisation", new RouteValueDictionary(model.Context).ToRouteValueDictionaryWithCollection());
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

            var reguleId = model.HasBNS ? model.Regularisations.FirstOrDefault().NumRegule.ToString() : "0"; // GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);
            var acteGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
            var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);

            var isReadonly = GetIsReadOnly(model.TabGuid, tId[0] + "_" + tId[1] + "_" + tId[2], (model.Contrat.NumAvenant).ToString(), modeAvenant: model.AvnMode);
            model.IsReadOnly = isReadonly;

            model.InfoBNS = GetInfoCreateBNS(tId[0], tId[1], tId[2], typeAvt, model.AvnMode, isReadonly || model.AvnMode == "CONSULT", model.AvnMode == "CONSULT" ? string.Empty : lotId, reguleId, regulMode);

            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULPB;

            if (!string.IsNullOrEmpty(acteGestionRegule))
            {
                model.AddParamValue = model.AddParamValue.Replace("ACTEGESTIONREGULE|AVNMD", "ACTEGESTIONREGULE|REGUL");
            }
            else
            {
                model.AddParamValue += "||ACTEGESTIONREGULE|REGUL";
            }
            model.AddParamValue = model.AddParamValue.Replace("REGULEID|0", "REGULEID|" + reguleId);
            if (model.AddParamValue.IndexOf("LOTID") > -1)
            {
                model.AddParamValue = model.AddParamValue.Replace("LOTID|0", "LOTID|" + (string.IsNullOrEmpty(lotId) ? model.LotId.ToString() : lotId));
            }
            else
            {
                model.AddParamValue += "||LOTID|" + (string.IsNullOrEmpty(lotId) ? model.LotId.ToString() : lotId);
            }

            model.ActeGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
            RegularisationNavigator.StandardInitContext(model, RegularisationStep.ChoixPeriodeCourtier);
            SetContextData();
        }

        private ModeleCreationRegule GetInfoCreateBNS(string codeContrat, string version, string type, string typeAvt, string modeAvt, bool isReadonly, string lotId, string reguleId, string regulMode)
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
                model.MotifAvt = "M3";

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

                if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL)
                {
                    model.ReguleId = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0;
                }
            }

            return model;
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

                if (contrat.FinEffetAnnee == 0 || contrat.FinEffetMois == 0 || contrat.FinEffetJour == 0)
                {
                    throw new AlbFoncException("Le contrat nécessite une date de fin pour la BNS.");
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

                var regule = GetInfoBNS(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.NumAvenantPage);
                model.Regularisations = regule != null && regule.Regularisations != null && regule.Regularisations.Any() ? regule.Regularisations.Where(x => x.RegulMode == "BNS").ToList() : new List<ModeleLigneRegularisation>();
                model.Alertes = GetInfoAlertes(regule.Alertes);
                ParametreDto typeContrat = regule != null && regule.TypesContrat != null && regule.TypesContrat.Any() ? regule.TypesContrat.Find(el => el.Code == model.Contrat.TypePolice) : null;
                model.LibTypeContrat = typeContrat != null ? typeContrat.Descriptif : string.Empty;
                model.HasBNS = model.Regularisations.Count > 0;
            }
        }

        private void SetContextData()
        {
            if (model.Context == null)
            {
                RegularisationNavigator.StandardInitContext(model, RegularisationStep.ChoixPeriodeCourtier);
            }
            model.Context.TypeAvt = "REGUL";
            model.Context.ComputeDone = false;
            model.Context.ValidationDone = false;
            model.Context.DateDebut = model.Contrat.DateEffetAnnee.ToString().PadLeft(4, '0') + model.Contrat.DateEffetMois.ToString().PadLeft(2, '0') + model.Contrat.DateEffetJour.ToString().PadLeft(2, '0');
            model.Context.DateFin = model.Contrat.FinEffetAnnee.ToString().PadLeft(4, '0') + model.Contrat.FinEffetMois.ToString().PadLeft(2, '0') + model.Contrat.FinEffetJour.ToString().PadLeft(2, '0');
            model.Context.Gestionnaire = model.Contrat.GestionnaireCode;
            model.Context.Souscripteur = model.Contrat.SouscripteurCode;
            model.Context.CodeEnc = model.InfoBNS.CodeQuittancement;
            model.Context.CodeICT = model.InfoBNS.CodeCourtier;
            model.Context.CodeICC = model.InfoBNS.CodeCourtierCom;
            model.Context.TauxCom = model.InfoBNS.TauxHCATNAT.ToString();
            model.Context.TauxComCATNAT = model.InfoBNS.TauxCATNAT.ToString();
            model.Context.RegimeTaxe = Model.Contrat.CodeRegime;
            model.Context.KeyValues = new string[] { model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.Contrat.NumInterneAvenant + "tabGuid" + model.TabGuid + "tabGuid" + "addParamAVN|||" + model.AddParamValue + "addParam" };
        }

        private List<ModeleAvenantAlerte> GetInfoAlertes(List<ModeleAvenantAlerte> Alertes)
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
            return Alertes != null && Alertes.Any() ? Alertes : new List<ModeleAvenantAlerte>();
        }

        private ModeleRegularisation GetInfoBNS(string codeContrat, string version, string type, string codeAvn)
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
    }
}