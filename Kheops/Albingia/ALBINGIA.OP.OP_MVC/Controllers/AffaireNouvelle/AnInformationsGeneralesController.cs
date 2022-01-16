using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Mvc.Common;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CoAssureurs;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Ecran.ModifierOffre;
using OP.WSAS400.DTO.Offres.Indice;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using static ALBINGIA.Framework.Common.Constants.AlbOpConstants;

namespace ALBINGIA.OP.OP_MVC.Controllers.AffaireNouvelle
{
    public class AnInformationsGeneralesController : ControllersBase<AnInformationsGeneralesPage>
    {
        [ErrorHandler]
        [AlbVerifLockedOffer("id", "P")]
        [WhitespaceFilter]
        public ActionResult Index(string id)
        {
            return Init(id);
        }

        #region ControllersBase Init
        protected override string FormatId(string id) {
            return ClientWorkEnvironment == OPENV_DEV ? id : string.IsNullOrEmpty(id) ? HttpContext.Request["paramWinOpen"] : id;
        }

        protected override void SetPageTitle() {
            model.PageTitle = "Informations Générales Contrat";
        }

        protected override void LoadInfoPage(string context) {
            if (context.IsEmptyOrNull()) {
                return;
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var tId = context.Split('_');
                if (tId.Length == 3) {
                    model.Contrat = client.Channel.GetContratSansRisques(
                        model.AllParameters.Folder.CodeOffre,
                        model.AllParameters.Folder.Version.ToString(),
                        model.AllParameters.Folder.Type,
                        model.NumAvenantPage,
                        model.ModeNavig.ParseCode<ModeConsultation>());

                    UnlockOffreSource();
                }
            }
        }

        protected override void UpdateModel() {
            if (Model.Contrat != null) {
                SetBandeauNavigation();
                SetContenData(model.Contrat);
                LoadDropDownListPage(model.Contrat.Branche, model.Contrat.Cible);
                Model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type);
                // Mise à jour de l'état du contrat si état = A et pas en lecture seule
                if (!Model.IsReadOnly) {
                    UpdateEtatContrat();
                }
            }
        }
        #endregion

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult AnInformationsGeneralesEnregistrer(AnInformationsGeneralesPage model) {
            var folder = new Folder(new[] { model.CodeContrat, model.VersionContrat.ToString(), model.Type });
            folder.NumeroAvenant = int.Parse(model.NumAvenantPage);
            var isReadOnly = GetIsReadOnly(model.TabGuid, folder.Identifier, model.NumAvenantPage);
            var isModifHorsAvn = GetIsModifHorsAvn(model.TabGuid, folder.FullIdentifier);

            bool hasIS = InformationsSpecifiquesBrancheController.HasIS(
               new AffaireId
               {
                   CodeAffaire = folder.CodeOffre,
                   NumeroAliment = folder.Version,
                   TypeAffaire = folder.Type.ParseCode<AffaireType>(),
                   NumeroAvenant = int.TryParse(model.NumAvenantPage, out int numAvt) && numAvt >= 0 ? numAvt : default(int?)
               }, 0);

            if ((!isReadOnly || isModifHorsAvn) && ModeConsultation.Standard == model.ModeNavig.ParseCode<ModeConsultation>())
            {
                //Sauvegarde uniquement si l'écran n'est pas en readonly
                var contrat = model.LoadContratDto();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var msgStr = client.Channel.ControleSousGest(model.SouscripteurCode, model.GestionnaireCode);

                    if (!string.IsNullOrEmpty(msgStr))
                    {
                        throw new AlbFoncException(msgStr);
                    }
                    // B3101
                    // Vérifiaction Changement date fin effet au niveau du modif hors avenant
                    // La methode remplace serviceContext.InfoGeneralesContratModifier
                    InfoGeneralesContratModifier(contrat, model.ModeNavig, isModifHorsAvn);
                }
                if (!isModifHorsAvn)
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                    {
                        var retGenClause = client.Channel.GenerateClause(
                            model.Type,
                            model.CodeContrat,
                            Convert.ToInt32(model.VersionContrat),
                            new ParametreGenClauseDto
                            {
                                ActeGestion = "**",
                                Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale)
                            });

                        if (retGenClause != null && retGenClause.MsgErreur.ContainsChars()) {
                            throw new AlbFoncException(retGenClause.MsgErreur);
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(model.txtParamRedirect))
                return  !hasIS
                    ? RedirectToAction("Index", "ChoixClauses",
                        new
            {
                id = model.CodeContrat + "_" + model.VersionContrat + "_" + model.Type +
                     "_¤AnInformationsGenerales¤Index¤" + model.CodeContrat + "£" + model.VersionContrat +
                     "£" + model.Type + GetSurroundedTabGuid(model.TabGuid) +
                     BuildAddParamString(model.AddParamType, model.AddParamValue) +
                     GetFormatModeNavig(model.ModeNavig),
                returnHome = model.txtSaveCancel, guidTab = model.TabGuid
                        })
                    : RedirectToAction("Index", "InformationsSpecifiquesBranche",
                        new
                        {
                            id = model.CodeContrat + "_" + model.VersionContrat + "_" + model.Type
                                + GetSurroundedTabGuid(model.TabGuid)
                                + BuildAddParamString(model.AddParamType, model.AddParamValue)
                                + GetFormatModeNavig(model.ModeNavig),
                            returnHome = model.txtSaveCancel
                        });
            var tabParamRedirect = model.txtParamRedirect.Split('/');
            return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
        }
        [HttpPost]
        [ErrorHandler]
        public JsonResult GetValeurIndiceByCode(string indiceString, string dateEffet)
        {
            var toReturn = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = GetValeurIndiceByCodeImplementation(indiceString, dateEffet)
            };
            return toReturn;
        }
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job)
        {
            return RedirectToAction(job, cible);
        }

        [ErrorHandler]
        public void SupprimerEcheances(string codeOffre, string version, string type, string codeAvn, string tabGuid)
        {
            //Supprime uniquement si l'écran n'est pas en readonly
            if (GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn)) return;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                finOffreClient.SupprimerEcheances(codeOffre, version, type);
            }
        }

        [ErrorHandler]
        public string ChangePreavisResil(string codeContrat, string version, string type, string dateEffet, string dateFinEffet, string dateAvenant, string periodicite, string echeancePrincipale, string splitHtmlChar, string acteGestion, string modeNavig, string tabGuid, string codeAvn)
        {
            if (GetIsReadOnly(tabGuid, codeContrat + "_" + version + "_" + type, codeAvn) ||
                ModeConsultation.Standard !=
                modeNavig.ParseCode<ModeConsultation>()) return string.Empty;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                return serviceContext.ChangePreavisResil(codeContrat, version, string.Empty, dateEffet, dateFinEffet, dateAvenant, periodicite, echeancePrincipale, splitHtmlChar, GetUser(), acteGestion);
            }
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
                    var serviceContext=client.Channel;
                    return serviceContext.ControleEcheance(prochaineEcheance, periodicite, echeancePrincipale);
                }
            }
            else
                return string.Empty;
        }

        [ErrorHandler]
        public ActionResult GetAperiteurDetail(string guidId, string codeContrat, string versionContrat, string typeContrat, string modeNavig,
                                               string nomAperiteur, float partAlbingia,
                                               int interloAperiteur, string interloAperiteurLib, string referenceAperiteur, float fraisAccAperiteur, float commissionAperiteur,
                                               bool offreReadOnly, bool isModifHorsAvn)
        {
            //récupération du coassureur correspondant au guidId
            CoAssureur coAssureurEdit = null;
            if (!string.IsNullOrEmpty(guidId))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.GetCoAssureurDetail(typeContrat, codeContrat, versionContrat, guidId, modeNavig.ParseCode<ModeConsultation>(), false);
                    if (result != null)
                    {
                        coAssureurEdit = (CoAssureur)result;
                        coAssureurEdit.IdInterlocuteur = interloAperiteur;
                        coAssureurEdit.Interlocuteur = interloAperiteurLib;
                        coAssureurEdit.Reference = referenceAperiteur;
                        coAssureurEdit.FraisAcc = fraisAccAperiteur;
                        coAssureurEdit.CommissionAperiteur = commissionAperiteur;
                        coAssureurEdit.IsReadonly = offreReadOnly;
                        coAssureurEdit.IsModifHorsAvn = isModifHorsAvn;

                    }
                }
            }
            if (coAssureurEdit == null)
            {
                coAssureurEdit = new CoAssureur
                {
                    Code = guidId,
                    Nom = nomAperiteur,
                    //PourcentPart = 100 - partAlbingia,
                    IdInterlocuteur = interloAperiteur,
                    Interlocuteur = interloAperiteurLib,
                    Reference = referenceAperiteur,
                    FraisAcc = fraisAccAperiteur,
                    CommissionAperiteur = commissionAperiteur,
                    IsReadonly = offreReadOnly,
                    IsModifHorsAvn = isModifHorsAvn
                };
            }
            coAssureurEdit.ModeCoass = false;
            return PartialView("/Views/AnCoAssureurs/EditionCoAssureur.cshtml", coAssureurEdit);
        }

        #region Méthodes privées
        private void LoadDropDownListPage(string branche, string cible)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ISaisieOffre>())
            {
                var screenClient=client.Channel;
                var result = screenClient.ModifierOffreGet(new ModifierOffreGetQueryDto {
                    CodeOffre = model.AllParameters.Folder.CodeOffre,
                    Version = model.AllParameters.Folder.Version,
                    Type = model.AllParameters.Folder.Type
                });
                if (result != null) {
                    List<AlbSelectListItem> motsCles1 = result.MotsCles.Select(
                        m => new AlbSelectListItem {
                            Value = m.Code,
                            Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
                            Selected = false,
                            Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
                        }).ToList();
                    model.MotsClefs1 = motsCles1;

                    var motsCles2 = result.MotsCles.Select(
                            m => new AlbSelectListItem {
                                Value = m.Code,
                                Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
                                Selected = false,
                                Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
                            }).ToList();
                    model.MotsClefs2 = motsCles2;

                    var motsCles3 = result.MotsCles.Select(
                            m => new AlbSelectListItem {
                                Value = m.Code,
                                Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
                                Selected = false,
                                Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
                            }).ToList();
                    model.MotsClefs3 = motsCles3;

                    var devises = result.Devises.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.Devises = devises;

                    var periodicites = result.Periodicites.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle), Descriptif = m.CodeTpcn1.ToString(CultureInfo.InvariantCulture) }).ToList();
                    model.Periodicites = periodicites;

                    var indices = result.Indices.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.IndicesReference = indices;

                    var naturescontrat = result.NaturesContrat.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.NaturesContrat = naturescontrat;

                    var durees = result.Durees.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.Durees = durees;

                    var regimesTaxe = result.RegimesTaxe.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.RegimesTaxe = regimesTaxe;

                    var antecedents = result.Antecedents.Select(
                           m => new AlbSelectListItem {
                               Value = m.Code,
                               Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
                               Selected = false,
                               Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
                           }).ToList();
                    model.Antecedents = antecedents;
                    var stops = result.Stops.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }).ToList();
                    model.Stops = stops;
                }
            }
        }

        private void UnlockOffreSource() {
            if (this.model.Contrat is null) {
                return;
            }

            var accesOffre = MvcApplication.ListeAccesAffaires.FirstOrDefault(x => x.ModeAcces == AccesOrigine.EtablirAffaireNouvelle && x.TabGuid.ToString("N") == Model.TabGuid);
            if (accesOffre != null) {
                FolderController.DeverrouillerAffaire(Model.TabGuid, new AffaireId {
                    CodeAffaire = accesOffre.Code,
                    NumeroAliment = accesOffre.Version,
                    TypeAffaire = AffaireType.Offre
                });
            }
        }

        private void SetContenData(ContratDto contrat)
        {
            model.CodeContrat = contrat.CodeContrat;
            model.VersionContrat = contrat.VersionContrat;
            model.Type = contrat.Type;
            model.Identification = contrat.Descriptif;
            model.Observations = contrat.Observations;
            model.Cible = contrat.Cible;
            model.CibleLib = contrat.CibleLib;
            model.Devise = contrat.Devise;
            model.MotClef1 = contrat.CodeMotsClef1;
            model.MotClef2 = contrat.CodeMotsClef2;
            model.MotClef3 = contrat.CodeMotsClef3;
            model.Periodicite = contrat.PeriodiciteCode;
            model.Preavis = contrat.Preavis;
            model.Antecedent = contrat.Antecedent;
            model.Description = contrat.Description;
            model.Stop = contrat.ZoneStop;

            if (contrat.DateEffetAnnee != 0 && contrat.DateEffetMois != 0 && contrat.DateEffetJour != 0)
            {
                model.EffetGaranties = new DateTime(contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour);
                model.HeureEffet = AlbConvert.ConvertIntToTimeMinute(Convert.ToInt32(contrat.DateEffetHeure.ToString()));

            }
            else model.EffetGaranties = null;
            if (contrat.DateAccordAnnee != 0 && contrat.DateAccordMois != 0 && contrat.DateAccordJour != 0)
                model.DateAccord = new DateTime(contrat.DateAccordAnnee, contrat.DateAccordMois, contrat.DateAccordJour);
            else model.DateAccord = null;
            if (contrat.FinEffetAnnee != 0 && contrat.FinEffetMois != 0 && contrat.FinEffetJour != 0)
            {
                model.FinEffet = new DateTime(contrat.FinEffetAnnee, contrat.FinEffetMois, contrat.FinEffetJour);
                model.HeureFinEffet = AlbConvert.ConvertIntToTimeMinute(Convert.ToInt32(contrat.FinEffetHeure.ToString()));
            }
            // B3101
            // Condition de modification de date fin effet
            model.CanModifEndEffectDate = CanModifEndEffectDate(contrat);
            if (contrat.DureeGarantie == 0)
                model.Duree = null;
            else model.Duree = contrat.DureeGarantie;
            model.DateStatistique = AlbConvert.ConvertIntToDate(contrat.DateStatistique);
            var mois = int.Parse(contrat.Mois.ToString());
            var jour = int.Parse(contrat.Jour.ToString());
            if (mois != 0 && jour != 0)
            {
                var annee = DateTime.Now.Year;
                model.EcheancePrincipale = new DateTime(annee, mois, jour);
            }
            if (!string.IsNullOrEmpty(contrat.UniteDeTemps))
            {
                model.DureeString = contrat.UniteDeTemps;
            }
            if (!string.IsNullOrEmpty(contrat.IndiceReference))
                model.IndiceReference = contrat.IndiceReference;
            model.Valeur = contrat.Valeur.ToString(CultureInfo.InvariantCulture).Replace(",", ".");
            model.NatureContrat = !string.IsNullOrEmpty(contrat.NatureContrat) ? contrat.NatureContrat : "";
            if (contrat.PartAlbingia != null)
                model.PartAlbingia = contrat.PartAlbingia.Value.ToString(CultureInfo.InvariantCulture);
            //model.PartAlbingia = contrat.PartAlbingia.Value.ToString().Replace(",", ".");
            if (!string.IsNullOrEmpty(contrat.AperiteurCode) && !string.IsNullOrEmpty(contrat.AperiteurNom))
            {
                model.AperiteurCode = contrat.AperiteurCode;
                model.AperiteurNom = contrat.AperiteurCode + " - " + contrat.AperiteurNom;
            }
            if (contrat.FraisAperition != null)
                model.FraisApe = contrat.FraisAperition.ToString();
            model.Intercalaire = contrat.IntercalaireExiste == "O";
            model.Couverture = contrat.Couverture.ToString(CultureInfo.InvariantCulture);
            model.SouscripteurCode = contrat.SouscripteurCode;
            model.SouscripteurNom = contrat.SouscripteurCode + " - " + contrat.SouscripteurNom;
            model.GestionnaireCode = contrat.GestionnaireCode;
            model.GestionnaireNom = contrat.GestionnaireCode + " - " + contrat.GestionnaireNom;
            model.RegimeTaxe = contrat.CodeRegime;
            model.SoumisCatNat = contrat.SoumisCatNat == "O";
            model.IsMonoRisque = contrat.IsMonoRisque;
            //Existence d'un echeancier
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                model.ExistEcheancier = client.Channel.PossedeEcheances(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
            }

            if (contrat.DateDebutDernierePeriodeJour != 0 && contrat.DateDebutDernierePeriodeMois != 0 && contrat.DateDebutDernierePeriodeAnnee != 0)
            {
                model.PeriodeDeb = new DateTime(contrat.DateDebutDernierePeriodeAnnee, contrat.DateDebutDernierePeriodeMois, contrat.DateDebutDernierePeriodeJour);
            }
            if (contrat.DateFinDernierePeriodeJour != 0 && contrat.DateFinDernierePeriodeMois != 0 && contrat.DateFinDernierePeriodeAnnee != 0)
            {
                model.PeriodeFin = new DateTime(contrat.DateFinDernierePeriodeAnnee, contrat.DateFinDernierePeriodeMois, contrat.DateFinDernierePeriodeJour);
            }
            if (contrat.ProchaineEchJour != 0 && contrat.ProchaineEchMois != 0 && contrat.ProchaineEchAnnee != 0)
            {
                model.ProchaineEcheance = new DateTime(contrat.ProchaineEchAnnee, contrat.ProchaineEchMois, contrat.ProchaineEchJour);
            }
            model.PartAperiteur = contrat.PartAperiteur;
            model.IdInterlocuteurAperiteur = contrat.IdInterlocuteurAperiteur;
            model.NomInterlocuteurAperiteur = contrat.NomInterlocuteurAperiteur;
            model.ReferenceAperiteur = contrat.ReferenceAperiteur;
            model.FraisAccAperiteur = contrat.FraisAccAperiteur;
            model.CommissionAperiteur = contrat.CommissionAperiteur;
            model.PartBenef = contrat.PartBenefDB;
            model.OppBenef = contrat.HasOppBenef;
            model.LTA = contrat.LTA == "O";
        }

        private void SetBandeauNavigation()
        {
            model.AfficherBandeau = true;
            model.AfficherNavigation = model.AfficherBandeau;
            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
            model.Bandeau = GetInfoBandeau(model.AllParameters.Folder.Type);
            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
            //Gestion des Etapes
            model.Navigation = new Navigation_MetaModel { Etape = Navigation_MetaModel.ECRAN_INFOGENERALE };
            //Affichage de la navigation latérale en arboresence                            
            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoGen");

        }

        private void UpdateEtatContrat()
        {
            if (Model.Contrat.Etat != "A") return;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                client.Channel.UpdateEtatContrat(model.Contrat.CodeContrat, model.Contrat.VersionContrat, model.Contrat.Type, "N");
            }
            Model.Contrat.Etat = "N";
        }

        private ModeleIndice GetValeurIndiceByCodeImplementation(string indiceString, string dateEffet)
        {
            return (ModeleIndice)IndiceGet(new IndiceGetQueryDto { Code = indiceString, DateEffet = dateEffet });
        }

        private IndiceGetResultDto IndiceGet(IndiceGetQueryDto query)
        {
            IndiceGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient=client.Channel;
                toReturn = screenClient.IndiceGet(query);
            }
            return toReturn;
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
                        result = client.Channel.HaveTraceOfEndEffectDate(contrat.CodeContrat,contrat.VersionContrat.ToString() , contrat.Type ,contrat.NumAvenant.ToString());

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
        private void InfoGeneralesContratModifier(ContratDto currentContrat ,string modeNavig, bool isModifHorsAvn)
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
                            serviceContext.InfoGeneralesContratModifier(currentContrat, GetUser(), isModifHorsAvn);
                            if (savedContrat != null)
                            {
                                // Vérification si la date a été changée
                                if (savedContrat.FinEffetAnnee != currentContrat.FinEffetAnnee ||
                                    savedContrat.FinEffetMois != currentContrat.FinEffetMois ||
                                    savedContrat.FinEffetJour != currentContrat.FinEffetJour)
                                {
                                    var user = GetUser();
                                    // Trace contrat dans KPHAVH avec la nouvelle valeur du date fin effet
                                    serviceContext.TraceContratInfoModifHorsAvn(currentContrat.CodeContrat, currentContrat.VersionContrat.ToString(), currentContrat.Type, currentContrat.NumAvenant.ToString(), user);
                                    // B2568
                                    // Trace résilisation à titre conservatoire 
                                    // si la valeur du date est null donc le type de trace est une annulation sinon on trace une modification
                                    // Tenir compte seulement du date sans les heures
                                    ResiliationTraceType traceResiliationType = currentContrat.FinEffetAnnee == 0 && currentContrat.FinEffetMois == 0 && currentContrat.FinEffetJour == 0  ? ResiliationTraceType.Cancellation : ResiliationTraceType.Modification;
                                    serviceContext.TraceResiliation(currentContrat.CodeContrat, currentContrat.VersionContrat.ToString(), currentContrat.Type, currentContrat.NumAvenant.ToString(), user, traceResiliationType);

                                }
                            }
                        }
                        else
                        {
                            // Cas Readonly = flase
                            serviceContext.InfoGeneralesContratModifier(currentContrat, GetUser(), isModifHorsAvn);
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
