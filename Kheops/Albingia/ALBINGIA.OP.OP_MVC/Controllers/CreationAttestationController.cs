using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationAttestation;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Attestation;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class CreationAttestationController : ControllersBase<ModeleCreationAttestationPage>
    {
        #region Méthodes publiques

        [ErrorHandler]
        
        [WhitespaceFilter]
        public ActionResult Index(string id)
        {
            model.PageTitle = "Création Attestation";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        public ActionResult OpenTabRsqObj(string codeContrat, string version, string type, string codeAvn, string lotId, string exercice, string periodeDeb, string periodeFin,
            string typeAttes, bool integralite)
        {
            ModeleAttestationRsqObj model = new ModeleAttestationRsqObj();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var risque = new List<ModeleRisque>();
                var result = serviceContext.OpenTabRsqObj(codeContrat, version, type, codeAvn, lotId, exercice, AlbConvert.ConvertStrToDate(periodeDeb), AlbConvert.ConvertStrToDate(periodeFin), typeAttes, integralite, GetUser());

                if (result != null)
                {
                    model = (ModeleAttestationRsqObj)result;
                }

                model.PeriodeDeb = periodeDeb;
                model.PeriodeFin = periodeFin;
            }

            return PartialView("AttesRsqObj", model);
        }

        [HttpPost]
        public JsonResult GetContratBandeau(ContratDto contrat, string id)
        {
            return new JsonResult
            {
                Data = new
                {
                    infosContrat = new
                    {
                        contrat.CodeContrat,
                        Version = contrat.VersionContrat.ToString(),
                        Type = contrat.Type,
                        DisplayTypeContrat = (!string.IsNullOrEmpty(contrat.LibTypeContrat) ? string.Format("{0} - {1}", contrat.TypePolice, contrat.LibTypeContrat) : model.BandeauContrat.TypeContrat),
                        Identification = contrat.Descriptif,
                        Assure = contrat.CodePreneurAssurance + " - " + contrat.NomPreneurAssurance,
                        Souscripteur = contrat.SouscripteurCode + " - " + contrat.SouscripteurNom,
                        DateDebutEffet = contrat.DateEffetJour > 0 ? new DateTime(contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour).ToString("dd/MM/yyyy") : string.Empty,
                        DateFinEffet = contrat.FinEffetJour > 0 ? new DateTime(contrat.FinEffetAnnee, contrat.FinEffetMois, contrat.FinEffetJour).ToString("dd/MM/yyyy") : string.Empty,
                        ProchainEcheance = contrat.ProchaineEchJour > 0 ? new DateTime(contrat.ProchaineEchAnnee, contrat.ProchaineEchMois, contrat.ProchaineEchJour).ToString("dd/MM/yyyy") : string.Empty,
                        HeureDebutEffet = contrat.DateEffetJour > 0 ? contrat.DateEffetHeure.ToString().PadLeft(4, '0') : string.Empty,
                        //DateFinEffet = (!string.IsNullOrEmpty(model.BandeauContrat.DateFinEffet)) ? DateTime.Parse(model.BandeauContrat.DateFinEffet).ToString("dd/MM/yyyy") : string.Empty,
                        contrat.PeriodiciteCode,
                        contrat.PeriodiciteNom,
                        Periodicite = contrat.PeriodiciteCode + " - " + contrat.PeriodiciteNom,
                        PartAlbingia = contrat.Couverture,
                        //Echeance = (!string.IsNullOrEmpty(model.BandeauContrat.Echeance)) ? DateTime.Parse(model.BandeauContrat.Echeance).ToString("dd/MM/yyyy") : string.Empty,
                        NatureContrat = contrat.LibelleNatureContrat,
                        contrat.CourtierApporteur,
                        NomCourtierApporteur = contrat.NomCourtierAppo,
                        Apporteur = contrat.CourtierApporteur + " - " + contrat.NomCourtierAppo,
                        TypeRetourPiece = contrat.TypeRetour,
                        LibelleRetourPiece = contrat.LibRetour,
                        Observation = contrat.Observations,
                        CodeGestionnaire = contrat.GestionnaireCode,
                        NomGestionnaire = contrat.GestionnaireNom,
                        Gestionnaire = contrat.GestionnaireCode + " - " + contrat.GestionnaireNom,
                        CourtierGestionnaire = contrat.CourtierGestionnaire + " - " + contrat.NomCourtierGest,
                        //ContratMere = model.BandeauContrat.ContratMere,
                        IsLightVersion = true,
                        LblDebutEffet = (contrat.DateEffetJour > 0 ? AlbConvert.ConvertStrToDate(string.Format("{0}/{1}/{2}", contrat.DateEffetJour, contrat.DateEffetMois, contrat.DateEffetAnnee)).ToString("dd/MM/yyyy") : HttpUtility.HtmlEncode(AlbConstantesMetiers.DateVide)),
                        LblFinEffet = (contrat.FinEffetJour > 0 ? AlbConvert.ConvertStrToDate(string.Format("{0}/{1}/{2}", contrat.FinEffetJour, contrat.FinEffetMois, contrat.FinEffetAnnee)).ToString("dd/MM/yyyy") : HttpUtility.HtmlEncode(AlbConstantesMetiers.DateVide)),
                        //Type = context.Mode.GetAttributeOfType<DisplayAttribute>().Name,
                        //RegimeTaxe = context.RegimeTaxe,
                        CodeRegimeTaxe = contrat.CodeRegime,
                        LibelleRegimeTaxe = contrat.LibelleRegime,
                        RegimeTaxe = contrat.CodeRegime + " - " + contrat.LibelleRegime,
                        CodeDevise = contrat.Devise,
                        contrat.LibelleDevise,
                        Devise = contrat.Devise + " - " + contrat.LibelleDevise
                    }
                }
            };
        }

        [ErrorHandler]
        public string ValidSelectionRsqObj(string codeContrat, string version, string type, string lotId, string selRsqObj)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                serviceContext.ValidSelectRsqObj(codeContrat, version, type, lotId, selRsqObj, GetUser());
            }
            return string.Empty;
        }

        [ErrorHandler]
        public ActionResult OpenTabGarantie(string codeContrat, string version, string type, string codeAvn, string lotId, string exercice, string periodeDeb, string periodeFin,
            string typeAttes, bool integralite)
        {
            ModeleAttestationGarantie model = new ModeleAttestationGarantie();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.OpenTabGarantie(codeContrat, version, type, codeAvn, lotId, exercice, AlbConvert.ConvertStrToDate(periodeDeb), AlbConvert.ConvertStrToDate(periodeFin), typeAttes, integralite, GetUser());
                if (result != null)
                {
                    model = (ModeleAttestationGarantie)result;
                }
            }

            model.PeriodeDeb = periodeDeb;
            model.PeriodeFin = periodeFin;

            #region Filtres

            List<AlbSelectListItem> filtresRsq = new List<AlbSelectListItem>();
            List<AlbSelectListItem> filtresFor = new List<AlbSelectListItem>();

            model.Risques.ForEach(elm =>
            {
                filtresRsq.Add(new AlbSelectListItem
                {
                    Value = elm.Code,
                    Text = "Risque " + elm.Code
                });

                elm.Formules.ForEach(form =>
                {
                    filtresFor.Add(new AlbSelectListItem
                    {
                        Value = form.LettreFormule,
                        Text = "Formule " + form.LettreFormule
                    });

                });
            });

            model.FiltresGarantie = filtresFor;
            model.FiltresRisque = filtresRsq;

            #endregion

            #region Alimentation du cache

            string keyAttestationGar = GetKeyAttestationGar(
                string.Format("{0}{4}{1}{4}{2}{4}{3}", codeContrat, version, type, model.LotId, MvcApplication.SPLIT_CONST_HTML));

            DeleteAttestationGarCache(keyAttestationGar);
            SetAttestationGarCache(keyAttestationGar, model.Risques);

            #endregion

            return PartialView("AttesGarantie", model);
        }

        [ErrorHandler]
        public void ChangeStatutCheckGar(string codeContrat, string version, string type, string lotId, string codeGar, bool isChecked)
        {
            var searchIdGar = !string.IsNullOrEmpty(codeGar) ? Convert.ToInt32(codeGar) : 0;
            var keyAttestationGar = GetKeyAttestationGar(
                 string.Format("{0}{4}{1}{4}{2}{4}{3}", codeContrat, version, type, lotId, MvcApplication.SPLIT_CONST_HTML));
            var modelGar = GetAttestationGarFromCache(keyAttestationGar);
            if (modelGar != null && modelGar.Any())
            {
                modelGar.ForEach(r =>
                {
                    r.Formules.ForEach(f =>
                    {
                        f.Garanties.ForEach(g1 =>
                        {
                            var changeStatus1 = false;
                            if (g1.IdGaran == searchIdGar)
                            {
                                g1.IsUsed = isChecked;
                                changeStatus1 = true;
                            }
                            g1.Garanties.ForEach(g2 =>
                            {
                                var changeStatus2 = false;
                                if (g2.IdGaran == searchIdGar || changeStatus1)
                                {
                                    g2.IsUsed = isChecked;
                                    changeStatus2 = true;
                                }
                                g2.Garanties.ForEach(g3 =>
                                {
                                    var changeStatus3 = false;
                                    if (g3.IdGaran == searchIdGar || changeStatus2)
                                    {
                                        g3.IsUsed = isChecked;
                                        changeStatus3 = true;
                                    }
                                    g3.Garanties.ForEach(g4 =>
                                    {
                                        if (g4.IdGaran == searchIdGar || changeStatus3)
                                        {
                                            g4.IsUsed = isChecked;
                                        }
                                    });
                                });
                            });
                        });
                    });
                });
            }
            SetAttestationGarCache(keyAttestationGar, modelGar);
        }

        [ErrorHandler]
        public string ValidSelectionGar(string codeContrat, string version, string type, string lotId, string selGarantie)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                serviceContext.ValidSelectionGar(codeContrat, version, type, lotId, selGarantie, GetUser());
            }
            return string.Empty;
        }

        [ErrorHandler]
        public string ChangeExercice(string codeContrat, string version, string type, string exercice)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.ChangeExercicePeriode(codeContrat, version, type, !string.IsNullOrEmpty(exercice) ? Convert.ToInt16(exercice) : 0, null, null);
                if (!string.IsNullOrEmpty(result))
                {
                    var err = result.Split('_')[2];
                    if (!string.IsNullOrEmpty(err))
                    {
                        var errMsg = string.Empty;
                        switch (err)
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

                        throw new AlbFoncException(errMsg, true, true, true);
                    }
                    return result.Split('_')[0] + "_" + result.Split('_')[1];
                }
                throw new AlbFoncException("Plage de dates invalide", true, true, true);
            }

            //var periodeDeb = "01/01/" + exercice;
            //var periodeFin = "31/12/" + exercice;
            //return periodeDeb + "_" + periodeFin;
        }

        [ErrorHandler]
        public string ChangePeriode(string codeContrat, string version, string type, string periodeDeb, string periodeFin)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.ChangeExercicePeriode(codeContrat, version, type, 0, AlbConvert.ConvertStrToDate(periodeDeb), AlbConvert.ConvertStrToDate(periodeFin));

                if (!string.IsNullOrEmpty(result))
                {
                    var err = result.Split('_')[2];
                    if (!string.IsNullOrEmpty(err))
                    {
                        var errMsg = string.Empty;
                        switch (err)
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

                        throw new AlbFoncException(errMsg, true, true, true);
                    }
                    return result.Split('_')[0] + "_" + result.Split('_')[1];
                }
                throw new AlbFoncException("Plage de dates invalide", true, true, true);
            }
            //return string.Empty;
        }

        [ErrorHandler]
        public ActionResult SearchGarantie(string codeContrat, string version, string type, string codeAvn, string lotId, string searchGarantie)
        {
            string keyAttestationGar = GetKeyAttestationGar(
                 string.Format("{0}{4}{1}{4}{2}{4}{3}", codeContrat, version, type, lotId, MvcApplication.SPLIT_CONST_HTML));

            ModeleAttestationGarantie model = new ModeleAttestationGarantie
            {
                Risques = GetAttestationGarFromCache(keyAttestationGar)
            };

            model.Risques.ForEach(r =>
               {
                   r.Formules.ForEach(f =>
                   {
                       f.Garanties.ForEach(g1 =>
                       {
                           g1.Garanties.ForEach(g2 =>
                           {
                               g2.Garanties.ForEach(g3 =>
                               {
                                   g3.Garanties.ForEach(g4 =>
                                   {
                                       g4.IsShown = string.IsNullOrEmpty(searchGarantie) || g4.CodeGarantie.ToLower().Trim().Contains(searchGarantie.ToLower()) || g4.LibelleGarantie.ToLower().Trim().Contains(searchGarantie.ToLower());
                                   });

                                   g3.IsShown = string.IsNullOrEmpty(searchGarantie) || g3.CodeGarantie.ToLower().Trim().Contains(searchGarantie.ToLower()) || g3.LibelleGarantie.ToLower().Trim().Contains(searchGarantie.ToLower());
                               });

                               g2.IsShown = string.IsNullOrEmpty(searchGarantie) || g2.CodeGarantie.ToLower().Trim().Contains(searchGarantie.ToLower()) || g2.LibelleGarantie.ToLower().Trim().Contains(searchGarantie.ToLower());
                           });

                           g1.IsShown = string.IsNullOrEmpty(searchGarantie) || g1.CodeGarantie.ToLower().Trim().Contains(searchGarantie.ToLower()) || g1.LibelleGarantie.ToLower().Trim().Contains(searchGarantie.ToLower());
                       });
                   });
               });

            SetAttestationGarCache(keyAttestationGar, model.Risques);

            return PartialView("AttesListGarantie", model);
        }

        [ErrorHandler]
        public string Suivant(string codeContrat, string version, string type, string codeAvenant, string tabGuid,
            string addParamType, string addParamValue, string codeAvenantExterne, string modeNavig,
            string lotId, string exercice, string periodeDeb, string periodeFin, string typeAttes, bool integralite)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.ValidPeriodeAttestation(codeContrat, version, type, lotId, exercice, AlbConvert.ConvertStrToDate(periodeDeb), AlbConvert.ConvertStrToDate(periodeFin), typeAttes, integralite, GetUser());
                return result;
            }
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeContrat, string version, string type, string codeAvenant, string tabGuid,
            string addParamType, string addParamValue, string codeAvenantExterne, string modeNavig, string lotId)
        {
            int codeAvn = 0;
            //Int32.TryParse(codeAvenant, out codeAvn);
            Int32.TryParse(codeAvenant, out codeAvn);

            var numAvn = (codeAvn).ToString(CultureInfo.InvariantCulture);
            var folder = string.Format("{0}_{1}_{2}", codeContrat, version, type);
            var isReadOnly = GetIsReadOnly(tabGuid, folder, numAvn);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, numAvn));

            if (cible == "ChoixClauses" && !isModifHorsAvn)
            {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                 var serviceontext=chan.Channel;
                    RetGenClauseDto retGenClause = serviceontext.GenerateClause(type, codeContrat,
                      Convert.ToInt32(version),
                      new ParametreGenClauseDto
                      {
                          ActeGestion = "ATTES",
                          Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Attestation),
                          NuRisque = Convert.ToInt32(lotId),
                          NuObjet = 0,
                          NuFormule = 0,
                          NuOption = 0,
                          IdAttesKpAtt = !string.IsNullOrEmpty(lotId) ? Convert.ToInt32(lotId) : 0
                      });
                    if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                    {
                        throw new AlbFoncException(retGenClause.MsgErreur);
                    }
                }

                return RedirectToAction("Index", "ChoixClauses", new { id = codeContrat + "_" + version + "_" + type + "_¤CreationAttestation¤Index¤" + codeContrat + "£" + version + "£" + type + tabGuid + BuildAddParamString(addParamType, addParamValue + "||IGNOREREADONLY|1") + GetFormatModeNavig(modeNavig) });
            }



            Common.CommonVerouillage.DeverrouilleFolder(codeContrat, version, type, numAvn, tabGuid,
                !isReadOnly && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>(),
                isReadOnly, isModifHorsAvn);

            return RedirectToAction("Index", "RechercheSaisie", new { id = codeContrat + "_" + version + "_" + type + "_loadParam" + tabGuid });
        }

        #endregion

        #region Méthodes privées

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            model.IsReadOnly = false;
            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_ATTES;

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], string.Empty, model.ModeNavig.ParseCode<ModeConsultation>());
                SetBandeauNavigation(model.Contrat, id);
                SetContentData(model.Contrat);
            }
        }

        private void SetContentData(ContratDto contrat)
        {
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
            model.Alertes = new List<ModeleAvenantAlerte>();
            model.TypesAttes = new List<AlbSelectListItem>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetInfoAttestation(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.TypePolice, GetUser());
                model.Alertes = GetInfoAlertes(result);
                ParametreDto typeContrat = result.TypesContrat.Find(el => el.Code == model.Contrat.TypePolice);
                model.LibTypeContrat = typeContrat != null ? typeContrat.Descriptif : string.Empty;
                List<AlbSelectListItem> typesAttes = result.TypesAttes.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.TypesAttes = typesAttes;
                model.TypeAttes = "01";

            }
        }

        private void SetBandeauNavigation(ContratDto contrat, string id)
        {
            model.AfficherBandeau = true;
            model.AfficherNavigation = model.AfficherBandeau;
            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_ATTES;
            model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_ATTES;
            //Gestion des Etapes
            model.Navigation = new Navigation_MetaModel();
            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoSaisie", returnEmptyTree: true);
        }

        private List<ModeleAvenantAlerte> GetInfoAlertes(AttestationDto result)
        {
            ModeleAttestation attestation = (ModeleAttestation)result;
            return attestation.Alertes != null && attestation.Alertes.Any() ? attestation.Alertes : new List<ModeleAvenantAlerte>();
        }

        private List<ModeleAttestationRisque> FillRisqueGarantie()
        {
            List<ModeleAttestationRisque> model = new List<ModeleAttestationRisque>();

            model.Add(new ModeleAttestationRisque
            {
                Code = "1",
                Objets = new List<ModeleAttestationObjet>(),
                CodesObj = "O1/O2",
                Formules = FillRisqueGarantieFor("1")
            });

            model.Add(new ModeleAttestationRisque
            {
                Code = "2",
                CodesObj = "O1",
                Objets = new List<ModeleAttestationObjet>(),
                Formules = FillRisqueGarantieFor("2")
            });

            model.Add(new ModeleAttestationRisque
            {
                Code = "3",
                CodesObj = "O1",
                Objets = new List<ModeleAttestationObjet>(),
                Formules = FillRisqueGarantieFor("3")
            });

            return model;
        }

        private List<ModeleAttestationFormule> FillRisqueGarantieFor(string codeRsq)
        {
            List<ModeleAttestationFormule> model = new List<ModeleAttestationFormule>();
            List<ModeleAttestationObjet> objets = new List<ModeleAttestationObjet>();
            List<ModeleAttestationGarantieNiv1> garNiv1 = new List<ModeleAttestationGarantieNiv1>();
            List<ModeleAttestationGarantieNiv2> garNiv2 = new List<ModeleAttestationGarantieNiv2>();
            List<ModeleAttestationGarantieNiv3> garNiv3 = new List<ModeleAttestationGarantieNiv3>();

            if (codeRsq == "1")
            {
                objets.Add(new ModeleAttestationObjet
                {
                    Code = "1"
                });
                objets.Add(new ModeleAttestationObjet
                {
                    Code = "2"
                });

                garNiv3.Add(new ModeleAttestationGarantieNiv3
                {
                    CodeGarantie = "GAR3",
                    LibelleGarantie = "Lib Gar 3"
                });

                garNiv2.Add(new ModeleAttestationGarantieNiv2
                {
                    CodeGarantie = "GAR2",
                    LibelleGarantie = "Lib Gar 2",
                    Garanties = garNiv3
                });

                garNiv1.Add(new ModeleAttestationGarantieNiv1
                {
                    CodeGarantie = "DOMSEJ",
                    LibelleGarantie = "Dommages en séjour",
                    Montant = "200000",
                    DateDebut = new DateTime(2015, 1, 1),
                    DateFin = new DateTime(2015, 12, 31),
                    Garanties = garNiv2
                });

                garNiv2 = new List<ModeleAttestationGarantieNiv2>();
                garNiv2.Add(new ModeleAttestationGarantieNiv2
                {
                    CodeGarantie = "GAR2",
                    LibelleGarantie = "Lib Gar 2"
                });

                garNiv1.Add(new ModeleAttestationGarantieNiv1
                {
                    CodeGarantie = "VOLVAL",
                    LibelleGarantie = "Vol des valeurs",
                    Montant = "150000",
                    DateDebut = new DateTime(2015, 1, 1),
                    DateFin = new DateTime(2015, 12, 31),
                    Garanties = garNiv2
                });

                model.Add(new ModeleAttestationFormule
                {
                    LettreFormule = "A",
                    Objets = objets,
                    Garanties = garNiv1
                });
            }

            if (codeRsq == "2")
            {
                objets.Add(new ModeleAttestationObjet
                {
                    Code = "1"
                });

                garNiv1 = new List<ModeleAttestationGarantieNiv1>();
                garNiv1.Add(new ModeleAttestationGarantieNiv1
                {
                    CodeGarantie = "ANNORG",
                    LibelleGarantie = "Annulation de l'organisateur",
                    Montant = "300000",
                    DateDebut = new DateTime(2015, 1, 1),
                    DateFin = new DateTime(2015, 12, 31)
                });

                garNiv1.Add(new ModeleAttestationGarantieNiv1
                {
                    CodeGarantie = "INTEMP",
                    LibelleGarantie = "Annulation pour intempérie",
                    Montant = "150000",
                    DateDebut = new DateTime(2015, 1, 1),
                    DateFin = new DateTime(2015, 12, 31)
                });

                garNiv1.Add(new ModeleAttestationGarantieNiv1
                {
                    CodeGarantie = "INDISP",
                    LibelleGarantie = "Annulation pour indisponibilité de personne",
                    Montant = "250000",
                    DateDebut = new DateTime(2015, 1, 1),
                    DateFin = new DateTime(2015, 12, 31),
                    Exclu = true
                });

                model.Add(new ModeleAttestationFormule
                {
                    LettreFormule = "B",
                    Objets = objets,
                    Garanties = garNiv1
                });
            }

            if (codeRsq == "3")
            {
                objets.Add(new ModeleAttestationObjet
                {
                    Code = "1"
                });

                garNiv1 = new List<ModeleAttestationGarantieNiv1>();
                garNiv1.Add(new ModeleAttestationGarantieNiv1
                {
                    CodeGarantie = "RCORG",
                    LibelleGarantie = "Responsabilité civile de l'organisateur",
                    Montant = "300000",
                    DateDebut = new DateTime(2015, 1, 1),
                    DateFin = new DateTime(2015, 12, 31)
                });


                model.Add(new ModeleAttestationFormule
                {
                    LettreFormule = "C",
                    Objets = objets,
                    Garanties = garNiv1
                });
            }

            return model;
        }

        private string GetKeyAttestationGar(string suffixeKey)
        {
            return GetUser() + MvcApplication.SPLIT_CONST_HTML + suffixeKey;
        }

        private List<ModeleAttestationRisque> GetAttestationGarFromDb(string keyAttestationGar)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                string[] tKeyAttestation = keyAttestationGar.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
                var result = serviceContext.LoadAttestationGarantie(tKeyAttestation[1], tKeyAttestation[2], tKeyAttestation[3], tKeyAttestation[4]);

                if (result != null && result.Risques != null)
                {
                    var model = (ModeleAttestationGarantie)result;
                    return model.Risques;
                }

                return new List<ModeleAttestationRisque>();
            }
        }

        private List<ModeleAttestationRisque> GetAttestationGarFromCache(string keyAttestationGar)
        {
            dynamic attestationGar;
            return AlbSessionHelper.AttestationGarUtilisateurs.TryGetValue(keyAttestationGar, out attestationGar) ? attestationGar
                : GetAttestationGarFromDb(keyAttestationGar);
        }

        private void SetAttestationGarCache(string key, List<ModeleAttestationRisque> newAttestationGar)
        {
            dynamic attestationGar;
            if (AlbSessionHelper.AttestationGarUtilisateurs.TryGetValue(key, out attestationGar))
            {
                AlbSessionHelper.AttestationGarUtilisateurs.Remove(key);
            }
            AlbSessionHelper.AttestationGarUtilisateurs.Add(key, newAttestationGar);
        }

        private void DeleteAttestationGarCache(string key)
        {
            dynamic attestationGar;
            if (AlbSessionHelper.ConditionsTarifairesUtilisateurs.TryGetValue(key, out attestationGar))
            {
                AlbSessionHelper.ConditionsTarifairesUtilisateurs.Remove(key);
            }
        }

        #endregion

    }
}
