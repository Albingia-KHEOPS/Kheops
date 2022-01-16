using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.Avenant.ModelesAvenant;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Offres.Indice;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers.Avenant
{
    public class AvenantInfoGeneralesController : ControllersBase<ModeleAvenantInfoGeneralesPage>
    {
        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult Index(string id)
        {
            Model.PageTitle = "Informations générales";
            id = InitializeParams(id);
            LoadInfoPage(id);
            if ((Model.ActeGestionRegule?.Trim()).IsEmptyOrNull())
            {
                Model.ActeGestionRegule = AlbConstantesMetiers.TYPE_AVENANT_MODIF;
                Model.AddParamValue = string.Join("|", model.AddParamValue, "|ACTEGESTIONREGULE", AlbConstantesMetiers.TYPE_AVENANT_MODIF);
            }
            return View(this.model);
        }

        [ErrorHandler]
        public string ControleEcheance(string codeContrat, string version, string type, string modeNavig, string tabGuid, string codeAvn, string dateDeb, string dateFin, string prochaineEcheance, string periodicite, string echeancePrincipale)
        {
            if (!GetIsReadOnly(tabGuid, codeContrat + "_" + version + "_" + type, codeAvn)
              && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                if (!string.IsNullOrEmpty(prochaineEcheance))
                {
                    var dEch = AlbConvert.ConvertStrToDate(prochaineEcheance);
                    var dDeb = AlbConvert.ConvertStrToDate(dateDeb);
                    var dFin = AlbConvert.ConvertStrToDate(dateFin);

                    if (dEch != null)
                    {
                        if (dDeb != null && dDeb > dEch) { throw new AlbFoncException("La date de prochaine échéance doit être supérieure ou égale à la date d'effet de garanties."); }
                        if (dFin != null && dEch > dFin) { throw new AlbFoncException("La date de prochaine échéance doit être inférieure ou égale à la date d'effet de garanties."); }
                    }
                }
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    return client.Channel.ControleEcheance(prochaineEcheance, periodicite, echeancePrincipale);
                }
            }
            else
                return string.Empty;
        }

        [ErrorHandler]
        public void SupprimerEcheances(string codeOffre, string version, string type, string codeAvn, string tabGuid)
        {
            //Supprime uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = client.Channel;
                    serviceContext.SupprimerEcheances(codeOffre, version, type);
                }
            }
        }

        [ErrorHandler]
        public string ChangePreavisResil(string codeContrat, string version, string type, string dateEffet, string dateFinEffet, string dateAvenant, string periodicite, string echeancePrincipale, string splitHtmlChar, string acteGestion, string modeNavig, string tabGuid, string codeAvn)
        {
            var folder = string.Format("{0}_{1}_{2}", codeContrat, version, type);
            var isReadOnly = GetIsReadOnly(tabGuid, folder, codeAvn);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            if ((!isReadOnly || isModifHorsAvn)
               && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = client.Channel;
                    return serviceContext.ChangePreavisResil(codeContrat, version, codeAvn, dateEffet, dateFinEffet, dateAvenant, periodicite, echeancePrincipale, splitHtmlChar, GetUser(), acteGestion);
                }
            }

            return string.Empty;
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetValeurIndiceByCode(string indiceString, string dateEffet)
        {
            IndiceGetResultDto result = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                result = screenClient.IndiceGet(new IndiceGetQueryDto { Code = indiceString, DateEffet = dateEffet });
            }

            JsonResult toReturn = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            toReturn.Data = (ModeleIndice)result;
            return toReturn;
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Enregistrer(ModeleAvenantInfoGeneralesPage model)
        {
            var numAvn = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNID);
            var folder = string.Format("{0}_{1}_{2}", model.CodeContrat, model.VersionContrat, model.Type);
            var isReadOnly = GetIsReadOnly(model.TabGuid, folder, numAvn);
            var isModifHorsAvn = GetIsModifHorsAvn(model.TabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(numAvn) ? "0" : numAvn));

            bool hasIS = InformationsSpecifiquesBrancheController.HasIS(
             new AffaireId
             {
                 CodeAffaire = model.CodeContrat,
                 NumeroAliment = (int)model.VersionContrat,
                 TypeAffaire = model.Type.ParseCode<AffaireType>(),
                 NumeroAvenant = int.TryParse(model.NumAvenantPage, out int numAvt) && numAvt >= 0 ? numAvt : default(int?)
             }, 0);

            if ((!isReadOnly || isModifHorsAvn) && ModeConsultation.Historique != model.ModeNavig.ParseCode<ModeConsultation>())
            {

                if (model.ActeGestion == AlbConstantesMetiers.TRAITEMENT_AVNRM && model.DateDebEffetAvt.HasValue && model.HeureDebEffetAvt.HasValue)
                {

                    var dateRemiseVig = new DateTime(model.DateDebEffetAvt.Value.Year, model.DateDebEffetAvt.Value.Month,
                                 model.DateDebEffetAvt.Value.Day, model.HeureDebEffetAvt.Value.Hours,
                                 model.HeureDebEffetAvt.Value.Minutes, model.HeureDebEffetAvt.Value.Seconds);

                    /// BUG 1154
                    //if (model.DateResil >= dateRemiseVig)
                    //{
                    //    throw new AlbFoncException("La date de remise en vigueur ne doit pas être antérieure à la date de résiliation", false, false, true); 
                    //}
                }
                //JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleAvenantInfoGeneralesPage>.GetSerializer();
                //model = serialiser.ConvertToType<ModeleAvenantInfoGeneralesPage>(serialiser.DeserializeObject(modelStr));
                Int16 iNumAvenant = 0;
                Int16.TryParse(numAvn, out iNumAvenant);
                var contrat = model.LoadAvenantDto(iNumAvenant);
                //Sauvegarde uniquement si l'écran n'est pas en readonly
                string MsgStr = string.Empty;
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = client.Channel;

                    MsgStr = serviceContext.ControleSousGest(model.SouscripteurCode, model.GestionnaireCode);

                    if (!string.IsNullOrEmpty(MsgStr))
                    {
                        throw new AlbFoncException(MsgStr, false, false, true);
                    }

                    // B3101
                    // Vérifiaction Changement date fin effet au niveau du modif hors avenant
                    // La methode remplace serviceContext.InfoGeneralesContratModifier
                    InfoGeneralesContratModifier(contrat, model.ModeNavig, isModifHorsAvn);
                    // serviceContext.EnregistrerAvenant(contrat, GetUser(), isModifHorsAvn);
                }
                //if (FileContentManager.GetConfigValue("IgnoreISBranche") == "true" && string.IsNullOrEmpty(model.txtParamRedirect))
                //{
                //on n'execute pas les IS Branche, par conséquent on génère les clauses ici
                if (!isModifHorsAvn)
                {
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                    {
                        var serviceontext = chan.Channel;
                        RetGenClauseDto retGenClause = serviceontext.GenerateClause(model.Type, model.CodeContrat,
                          Convert.ToInt32(model.VersionContrat),
                          new ParametreGenClauseDto
                          {
                              ActeGestion = "**",
                              Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale)
                          });
                        if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                        {
                            throw new AlbFoncException(retGenClause.MsgErreur);
                        }
                    }
                }
                //}

            }

            if (!string.IsNullOrEmpty(model.txtParamRedirect))
            {
                var tabParamRedirect = model.txtParamRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE) == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR)
            {
                return
                RedirectToAction("Index", "ChoixClauses", new
                {
                    id = model.CodeContrat + "_"
                + model.VersionContrat + "_" +
                model.Type + "_¤AvenantInfoGenerales¤Index¤" +
                model.CodeContrat + "£" + model.VersionContrat + "£" +
                model.Type + GetSurroundedTabGuid(model.TabGuid) +
                BuildAddParamString(model.AddParamType, model.AddParamValue) +
                GetFormatModeNavig(model.ModeNavig),
                    returnHome = model.txtSaveCancel,
                    guidTab = model.TabGuid
                });
            }

            return !hasIS ?
            RedirectToAction("Index", "AnnulationQuittances", new
            {
                id = model.CodeContrat + "_" +
            model.VersionContrat + "_" + model.Type +
            GetSurroundedTabGuid(model.TabGuid) +
            BuildAddParamString(model.AddParamType, model.AddParamValue) +
            GetFormatModeNavig(model.ModeNavig),
                returnHome = model.txtSaveCancel,
                guidTab = model.TabGuid
            }) : RedirectToAction("Index", "InformationsSpecifiquesBranche",
                        new
                        {
                            id = model.CodeContrat + "_" + model.VersionContrat + "_" + model.Type
                                + GetSurroundedTabGuid(model.TabGuid)
                                + BuildAddParamString(model.AddParamType, model.AddParamValue)
                                + GetFormatModeNavig(model.ModeNavig),
                            returnHome = model.txtSaveCancel,
                            guidTab = model.TabGuid
                        });
        }

        #region Méthodes privées

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var result = client.Channel.GetAvenant(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                if (result != null)
                {
                    SetDropDownListPage(result);
                    SetContentData(result);
                    SetBandeauNavigation(result.Avenant, id);
                    model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                }
            }
        }

        private void SetBandeauNavigation(ContratDto avenant, string id)
        {
            model.AfficherBandeau = true;
            model.AfficherNavigation = model.AfficherBandeau;
            model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            switch (typeAvt)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                    model.Bandeau.StyleBandeau = model.ScreenType;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                    model.Bandeau.StyleBandeau = model.ScreenType;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                    model.Bandeau.StyleBandeau = model.ScreenType;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    break;
                default:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    break;
            }
            //Gestion des Etapes
            model.Navigation = new Navigation_MetaModel();
            model.Navigation.Etape = Navigation_MetaModel.ECRAN_INFOGENERALE;
            model.Navigation.ScreenType = model.ScreenType;
            //Affichage de la navigation latérale en arboresence                            
            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoGen");
            model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
            model.NavigationArbre.NumAvn = avenant.NumAvenant;
        }

        /// <summary>
        /// Remplissage des listes déroulantes de la page
        /// </summary>
        private void SetDropDownListPage(AvenantInfoDto result)
        {
            List<AlbSelectListItem> motsClef1 = result.MotsClef.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
            model.MotsClefs1 = motsClef1;
            List<AlbSelectListItem> motsClef2 = result.MotsClef.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
            model.MotsClefs2 = motsClef2;
            List<AlbSelectListItem> motsClef3 = result.MotsClef.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
            model.MotsClefs3 = motsClef3;
            List<AlbSelectListItem> regimesTaxe = result.RegimesTaxe.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
            model.RegimesTaxe = regimesTaxe;
            List<AlbSelectListItem> antecedents = result.Antecedents.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
            model.Antecedents = antecedents;
            List<AlbSelectListItem> stops = result.Stops.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
            model.Stops = stops;

            List<AlbSelectListItem> periodicites = result.Periodicites.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Descriptif = m.CodeTpcn1.ToString() }).ToList();
            model.Periodicites = periodicites;
            List<AlbSelectListItem> durees = result.Durees.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
            model.Durees = durees;
            List<AlbSelectListItem> indices = result.Indices.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
            model.IndicesReference = indices;
            List<AlbSelectListItem> naturesContrat = result.NaturesContrat.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
            model.NaturesContrat = naturesContrat;
            List<AlbSelectListItem> motifs = result.Motifs.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
            model.Motifs = motifs;
        }

        /// <summary>
        /// Récupération des informations de l'avenant
        /// </summary>
        private void SetContentData(AvenantInfoDto model)
        {
            ContratDto avenant = model.Avenant;

            base.model.Contrat = avenant;

            base.model.CodeContrat = avenant.CodeContrat;
            base.model.VersionContrat = avenant.VersionContrat;
            base.model.Type = avenant.Type;
            base.model.PeriodiciteHisto = model.PeriodiciteHisto;
            base.model.ProchEchHisto = model.ProchEchHisto;
            base.model.DebPeriodHisto = model.DebPeriodHisto;
            base.model.EtatHisto = model.EtatHisto;
            base.model.SituationHisto = model.SituationHisto;

            #region 1er Bloc

            base.model.NumAvenant = avenant.NumAvenant.ToString();
            base.model.NumInterne = avenant.NumInterneAvenant.ToString();
            base.model.DateDebEffetAvt = avenant.DateEffetAvenant;
            base.model.HeureDebEffetAvt = avenant.HeureEffetAvenant;
            base.model.Motif = avenant.MotifAvenant;
            base.model.MotifLib = avenant.LibMotifAvenant;
            base.model.Description = avenant.DescriptionAvenant;
            base.model.DateResil = AlbConvert.ConvertIntToDateHour(Convert.ToInt64(model.Avenant.DateResilAnnee) * 100000000 + Convert.ToInt64(model.Avenant.DateResilMois) * 1000000 + Convert.ToInt64(model.Avenant.DateResilJour) * 10000 + Convert.ToInt64(model.Avenant.DateResilHeure));

            #endregion
            #region 2e Bloc

            base.model.Identification = avenant.Descriptif;
            base.model.CibleCode = avenant.Cible;
            base.model.CibleLib = avenant.CibleLib;
            base.model.MotClef1 = avenant.CodeMotsClef1;
            base.model.MotClef2 = avenant.CodeMotsClef2;
            base.model.MotClef3 = avenant.CodeMotsClef3;
            base.model.Observations = avenant.Observations;
            base.model.DeviseCode = avenant.Devise;
            base.model.DeviseLib = avenant.LibelleDevise;
            base.model.RegimeTaxe = avenant.CodeRegime;
            base.model.SoumisCATNAT = avenant.SoumisCatNat == "O";
            base.model.Stop = avenant.ZoneStop;
            base.model.Antecedent = avenant.Antecedent;
            base.model.ObservationAntecedents = avenant.Description;

            #endregion
            #region 3e Bloc

            base.model.Periodicite = avenant.PeriodiciteCode;
            //Existence d'un echeancier
            //using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            //{
            // var finOffreClient=chan.Channel;
            //    //var echeancesDto = finOffreClient.GetAllEcheances(avenant.CodeContrat, avenant.VersionContrat.ToString(), avenant.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
            //    //model.ExistEcheancier = echeancesDto.Count > 0 ? true : false;
            //    model.ExistEcheancier = finOffreClient.PossedeEcheances(avenant.CodeContrat, avenant.VersionContrat.ToString(), avenant.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());

            //}
            base.model.ExistEcheancier = model.ExistEcheancier;

            int mois = Int32.Parse(avenant.Mois.ToString());
            int jour = Int32.Parse(avenant.Jour.ToString());
            if (mois != 0 && jour != 0)
            {
                int annee = 2012; //2012 : année bisextile
                base.model.EcheancePrincipale = new DateTime(annee, mois, jour);
            }
            if (avenant.ProchaineEchJour != 0 && avenant.ProchaineEchMois != 0 && avenant.ProchaineEchAnnee != 0)
            {
                base.model.ProchaineEcheance = new DateTime(avenant.ProchaineEchAnnee, avenant.ProchaineEchMois, avenant.ProchaineEchJour);
            }
            if (avenant.DateDebutDernierePeriodeAnnee != 0 && avenant.DateDebutDernierePeriodeMois != 0 && avenant.DateDebutDernierePeriodeJour != 0)
            {
                base.model.PeriodeDeb = new DateTime(avenant.DateDebutDernierePeriodeAnnee, avenant.DateDebutDernierePeriodeMois, avenant.DateDebutDernierePeriodeJour);
            }
            if (avenant.DateFinDernierePeriodeAnnee != 0 && avenant.DateFinDernierePeriodeMois != 0 && avenant.DateFinDernierePeriodeJour != 0)
            {
                base.model.PeriodeFin = new DateTime(avenant.DateFinDernierePeriodeAnnee, avenant.DateFinDernierePeriodeMois, avenant.DateFinDernierePeriodeJour);
            }
            if (avenant.DateEffetAnnee != 0 && avenant.DateEffetMois != 0 && avenant.DateEffetJour != 0)
            {
                base.model.DateDebEffet = new DateTime(avenant.DateEffetAnnee, avenant.DateEffetMois, avenant.DateEffetJour);
                base.model.HeureDebEffet = AlbConvert.ConvertIntToTimeMinute(Convert.ToInt32(avenant.DateEffetHeure.ToString()));
            }
            else base.model.DateDebEffet = null;
            if (avenant.FinEffetAnnee != 0 && avenant.FinEffetMois != 0 && avenant.FinEffetJour != 0)
            {
                base.model.DateFinEffet = new DateTime(avenant.FinEffetAnnee, avenant.FinEffetMois, avenant.FinEffetJour);
                base.model.HeureFinEffet = AlbConvert.ConvertIntToTimeMinute(Convert.ToInt32(avenant.FinEffetHeure.ToString()));
            }
            else base.model.DateFinEffet = null;
            if (avenant.DureeGarantie == 0)
                base.model.Duree = null;
            else base.model.Duree = avenant.DureeGarantie;
            base.model.DureeString = avenant.UniteDeTemps;
            if (avenant.DateAccordAnnee != 0 && avenant.DateAccordMois != 0 && avenant.DateAccordJour != 0)
                base.model.DateAccord = new DateTime(avenant.DateAccordAnnee, avenant.DateAccordMois, avenant.DateAccordJour);
            else base.model.DateAccord = null;
            base.model.Preavis = avenant.Preavis;
            base.model.PartBenef = avenant.PartBenefDB;
            base.model.OppBenef = model.HasOppBenef;

            base.model.LTA = avenant.LTA == "O";

            #endregion
            #region 4e Bloc

            base.model.IndiceReference = avenant.IndiceReference;
            base.model.Valeur = avenant.Valeur.ToString().Replace(",", ".");
            if (!string.IsNullOrEmpty(avenant.AperiteurCode) && !string.IsNullOrEmpty(avenant.AperiteurNom))
            {
                base.model.AperiteurCode = avenant.AperiteurCode;
                base.model.AperiteurNom = avenant.AperiteurCode + " - " + avenant.AperiteurNom;
            }
            base.model.NatureContrat = avenant.NatureContrat;
            if (avenant.PartAlbingia.HasValue)
                base.model.PartAlbingia = avenant.PartAlbingia.Value.ToString();
            if (avenant.FraisAperition != null)
                base.model.FraisApe = avenant.FraisAperition.ToString();
            base.model.Couverture = avenant.Couverture.ToString();
            base.model.Intercalaire = avenant.IntercalaireExiste == "O";
            base.model.SouscripteurCode = avenant.SouscripteurCode;
            base.model.SouscripteurNom = avenant.SouscripteurCode + " - " + avenant.SouscripteurNom;
            base.model.GestionnaireCode = avenant.GestionnaireCode;
            base.model.GestionnaireNom = avenant.GestionnaireCode + " - " + avenant.GestionnaireNom;

            base.model.PartAperiteur = avenant.PartAperiteur;
            base.model.IdInterlocuteurAperiteur = avenant.IdInterlocuteurAperiteur;
            base.model.NomInterlocuteurAperiteur = avenant.NomInterlocuteurAperiteur;
            base.model.ReferenceAperiteur = avenant.ReferenceAperiteur;
            base.model.FraisAccAperiteur = avenant.FraisAccAperiteur;
            base.model.CommissionAperiteur = avenant.CommissionAperiteur;


            #endregion

            // B3101
            // Condition de modification de date fin effet
            base.model.CanModifEndEffectDate = CanModifEndEffectDate(avenant);
        }




        /// <summary>
        /// B3101
        /// Vérification de modif date fin effet
        /// </summary>
        /// <param name="contrat"></param>
        /// <returns></returns>
        private bool CanModifEndEffectDate(ContratDto contrat)
        {
            var result = false;
            try
            {
                // Vérification date fin effet
                var hasEndEffectDate = contrat.FinEffetAnnee != 0 && contrat.FinEffetMois != 0 && contrat.FinEffetJour != 0;
                //si pas date fin effet - Modification
                if (!hasEndEffectDate)
                {
                    result = true;
                }
                else
                {
                    // Date fin effet existe
                    // Vérification dans le trace KPHAVH
                    // Si pas de trace = blocage modif. date fin effet
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        result = client.Channel.HaveTraceOfEndEffectDate(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type, contrat.NumAvenant.ToString());

                    }


                }

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        /// <summary>
        /// B3101
        /// Modification Info générale 
        /// </summary>
        /// <param name="currentContrat"></param>
        /// <param name="isModifHorsAvn"></param>
        private void InfoGeneralesContratModifier(ContratDto currentContrat, string modeNavig, bool isModifHorsAvn)
        {

            try
            {

                if (currentContrat != null)
                {

                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        var serviceContext = client.Channel;
                        if (isModifHorsAvn)
                        {
                            // Récuperation du contrat de la BDD
                            var savedContrat = serviceContext.GetEndEffectDate(currentContrat.CodeContrat, currentContrat.VersionContrat.ToString(), currentContrat.Type);
                            serviceContext.EnregistrerAvenant(currentContrat, GetUser(), isModifHorsAvn);
                            if (savedContrat != null)
                            {
                                // Vérification si la date a été changée
                                if (savedContrat.FinEffetAnnee != currentContrat.FinEffetAnnee ||
                                    savedContrat.FinEffetMois != currentContrat.FinEffetMois ||
                                    savedContrat.FinEffetJour != currentContrat.FinEffetJour)
                                {
                                    var user = GetUser();
                                    // Trace contrat dans KPHAVH avec la nouvelle valeur du date fin effet
                                    serviceContext.TraceContratInfoModifHorsAvn(currentContrat.CodeContrat, currentContrat.VersionContrat.ToString(), currentContrat.Type, currentContrat.NumAvenant.ToString(), GetUser());
                                    // B2568
                                    // Trace résilisation à titre conservatoire
                                    // si la valeur du date est null donc le type de trace est une annulation sinon on trace une modification
                                    // Tenir compte seulement du date sans les heures
                                    ResiliationTraceType traceResiliationType = currentContrat.FinEffetAnnee == 0 && currentContrat.FinEffetMois == 0 && currentContrat.FinEffetJour == 0 ? ResiliationTraceType.Cancellation : ResiliationTraceType.Modification;
                                    serviceContext.TraceResiliation(currentContrat.CodeContrat, currentContrat.VersionContrat.ToString(), currentContrat.Type, currentContrat.NumAvenant.ToString(), user, traceResiliationType);

                                }
                            }
                        }
                        else
                        {
                            // Cas Readonly = flase
                            serviceContext.EnregistrerAvenant(currentContrat, GetUser(), isModifHorsAvn);
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
    }
}
