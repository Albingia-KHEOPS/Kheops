using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using ALBINGIA.OP.OP_MVC.Models.ModelesNavigationArbre;
using ALBINGIA.OP.OP_MVC.Models.ModelesObjet;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using ALBINGIA.OP.OP_MVC.Models.Regularisation;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegulGar;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleSaisieRegulGarantie;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
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
using System.Web.Script.Serialization;

namespace ALBINGIA.OP.OP_MVC.Controllers.Regularisation {
    public class CreationRegularisationController : ControllersBase<ModeleRegularisationPage>
    {
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = HttpUtility.UrlDecode(id);
            model.PageTitle = "Création régularisation";
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
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeContrat, string version, string type, string codeAvenant, string tabGuid,
            string codeRsq, string codeFor, string codeOpt, string idGar, string lotID, string reguleId, string codeGAr, string libgar,
            string addParamType, string addParamValue, string codeAvenantExterne, string modeNavig, string typAvt,
            string acteGestionRegule, string dateDebReg, string dateFinReg, bool isReadonly, bool isSaveAndQuit)
        {
            var acteGestReg = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);

            if (isSaveAndQuit)
            {
                return RedirectToAction("Index", "RechercheSaisie", new { id = codeContrat + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue + acteGestReg) + GetFormatModeNavig(modeNavig) });
            }

            if (cible != "RechercheSaisie")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var Context = client.Channel;
                    string retour = Context.CheckDatesPeriodAllRsqIntegrity(codeContrat, version, type, lotID, typAvt, dateDebReg, dateFinReg, reguleId);
                }
            }
            if (!string.IsNullOrEmpty(codeAvenant))
            {
                // MAJ du code avenant dans le addParam
                var curCodeAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
                if (!string.IsNullOrEmpty(curCodeAvn))
                {
                    addParamValue = addParamValue.Replace("AVNID|" + curCodeAvn, "AVNID|" + codeAvenant);
                    addParamValue = addParamValue.Replace("AVNIDEXTERNE|" + curCodeAvn, "AVNIDEXTERNE|" + codeAvenant);
                }
                else
                {
                    addParamValue = addParamValue + "||AVNID|" + codeAvenant + "||AVNIDEXTERNE|" + codeAvenant;
                }
            }

            if (!string.IsNullOrEmpty(acteGestionRegule))
            {
                acteGestReg = acteGestReg.Replace("AVNMD", "REGUL");
            }
            else
            {
                acteGestReg += "||ACTEGESTIONREGULE|REGUL";
            }

            if (cible == "AvenantInfoGenerales")
            {
                return RedirectToAction(job, cible, new { id = codeContrat + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue + acteGestReg) + GetFormatModeNavig(modeNavig) });
            }

            if (cible == "Quittance")
            {
                var paramArray = addParamValue.ToParamDictionary();
                if (typAvt.ContainsChars()) {
                    paramArray[AlbParameterName.AVNTYPE.ToString()] = typAvt;
                }
                if (!paramArray.TryGetValue(AlbParameterName.REGULEID.ToString(), out string rgid) || (int.TryParse(rgid, out int i) ? i : 0) < 1) {
                    paramArray[AlbParameterName.REGULEID.ToString()] = reguleId;
                }
                return RedirectToAction(job, cible, new { id = codeContrat + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, paramArray.RebuildAddParamString()) + GetFormatModeNavig(modeNavig) });
            }
            if (cible == "RechercheSaisie")
            {
                Int32.TryParse(codeAvenant, out int codeAvn);

                var numAvn = codeAvn.ToString(CultureInfo.InvariantCulture);
                var folder = string.Format("{0}_{1}_{2}", codeContrat, version, type);
                var isReadOnly = GetIsReadOnly(tabGuid, folder, numAvn);
                var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, numAvn));

                Common.CommonVerouillage.DeverrouilleFolder(codeContrat, version, type, (codeAvn).ToString(), tabGuid,
                    !isReadOnly && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>(), isReadOnly, isModifHorsAvn);

                return RedirectToAction("Index", "RechercheSaisie", new { id = codeContrat + "_" + version + "_" + type + "_loadParam" + tabGuid });
            }

            string id = codeContrat + "_" + version + "_" + type + "_" + codeRsq + "_" + codeFor + "_" + codeOpt + "_" + idGar + "_" + lotID + "_" + reguleId + "_" + codeGAr + "_" + libgar;

            // Lancement de prog KDA301CL
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.MouvementsGarPeriodeAS400(codeContrat, version, type, codeRsq, codeFor, idGar, lotID, reguleId);

                if (result != null && !string.IsNullOrEmpty(result))
                {
                    throw new AlbFoncException(result, true, true, true);
                }
                else
                {
                    return RedirectToAction(job, cible, new { id = id + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
                }
            }
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult OpenAvnModif(string codeContrat, string version, string type, string codeAvn, string modeAvt, string typeAvt, string lotId, string reguleId, string tabGuid, string modeNavig)
        {
            string addParam = string.Empty;
            string workParam = string.Empty;

            if (!string.IsNullOrEmpty(modeAvt))
            {
                workParam += "||" + AlbParameterName.AVNMODE + "|" + modeAvt;
            }
            if (!string.IsNullOrEmpty(typeAvt))
                workParam += "||" + AlbParameterName.AVNTYPE + "|" + typeAvt;

            if (modeAvt == "UPDATE")
            {
                workParam += "||" + AlbParameterName.AVNID + "|" + (string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn);
                workParam += "||" + AlbParameterName.AVNIDEXTERNE + "|" + (string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn);
            }
            workParam += "||" + AlbParameterName.REGULEID + "|" + (string.IsNullOrEmpty(reguleId) ? "0" : reguleId);

            if (!string.IsNullOrEmpty(workParam))
                addParam = "addParam" + AlbOpConstants.GLOBAL_TYPE_ADD_PARAM_AVN + "|||" + workParam.Substring(2) + "addParam";

            string paramContrat = codeContrat + "_" + version + "_" + type;

            return RedirectToAction("Index", "CreationAvenant", new { id = string.Concat(paramContrat, tabGuid, addParam, modeNavig) });
        }

        [ErrorHandler]
        
        public ActionResult OpenRegule(string codeContrat, string version, string type, string codeAvn, string modeAvt, bool isReadonly, string typeAvt, string lotId, string reguleId, string id, string regulMode)
        {
            return PartialView("CreationRegule", GetInfoCreateRegule(codeContrat, version, type, typeAvt, modeAvt, isReadonly, lotId, reguleId, regulMode));
        }

        [ErrorHandler]
        public ActionResult DeleteRegule(string codeContrat, string version, string type, string codeAvn, string reguleId)
        {
            var model = new ModeleRegularisationPage { Regularisations = new List<ModeleLigneRegularisation>() };

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.DeleteRegule(codeContrat, version, type, codeAvn, reguleId);
                if (result != null && result.Any())
                {
                    result.ForEach(el =>
                    {
                        model.Regularisations.Add((ModeleLigneRegularisation)el);
                    });
                }
            }
            return PartialView("ListeRegularisations", model);
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
            result.Courtiers.ForEach(el => courtiers.Add(new AlbSelectListItem { Value = el.Code.ToString(), Text = el.Code.ToString() + " - " + el.Libelle, Selected = false, Title = el.Code.ToString() + " - " + el.Libelle }));
            model.Courtiers = courtiers;
            List<AlbSelectListItem> quittancements = new List<AlbSelectListItem>();
            result.Quittancements.ForEach(el => quittancements.Add(new AlbSelectListItem { Value = el.Code, Text = el.Code + " - " + el.Libelle, Selected = false, Title = el.Code + " - " + el.Libelle }));
            model.Quittancements = quittancements;
            model.LotId = result.LotId;
            return PartialView("CreationReguleCourtier", model);
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

        [ErrorHandler]
        public ActionResult ReloadListRegule(string codeContrat, string version, string type)
        {
            var model = new ModeleRegularisationPage();
            model.Regularisations = GetInfoRegularisation(codeContrat, version, type);
            return PartialView("ListeRegularisations", model);
        }

        [ErrorHandler]
        public ActionResult ReloadListRsqRegule(string lotId, string reguleId, bool isReadonly)
        {
            var rsq = new List<ModeleRisque>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.ReloadListRsqRegule(lotId, reguleId, isReadonly);
                if (result != null)
                {
                    result.ForEach(r =>
                    {
                        var objets = new List<ModeleObjet>();

                        r.Objets.ForEach(o =>
                        {
                            var objet = (ModeleObjet)o;
                            objet.Cible = new ParametreDto { Code = o.Cible.Code, Libelle = o.Cible.Nom };
                            objets.Add(objet);
                        });

                        var risque = (ModeleRisque)r;
                        risque.Cible = new ParametreDto { Code = r.Cible.Code, Libelle = r.Cible.Nom };
                        risque.Objets = objets;
                        rsq.Add(risque);
                    });
                }
            }

            return PartialView("TabRisquesRegularisation", rsq);
        }

        [ErrorHandler]
        public ActionResult OpenGarRegule(string lotId, string reguleId, string codeContrat, string version, string type, string typeAvt, string codeAvn, string codeRsq, string dateDeb, string dateFin, bool isReadonly)
        {
            ModeleGarantiesRegularisation model = new ModeleGarantiesRegularisation();
            var garanties = new List<ModeleGarantie>();
            var risque = new ModeleRisque();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetListGarRegule(lotId, reguleId, codeRsq, isReadonly);
                if (result != null)
                {
                    risque = (ModeleRisque)result.Risque;
                    risque.EntreeGarantie = result.Risque.DateEntreeDescr;
                    risque.SortieGarantie = result.Risque.DateSortieDescr;
                    result.Garanties.ForEach(g =>
                    {
                        var garan = (ModeleGarantie)g;
                        garan.EntreeGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(g.DateDebGar));
                        garan.SortieGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(g.DateFinGar));

                        garanties.Add(garan);
                    });
                }
            }

            model.CodeAvn = Convert.ToInt32(codeAvn);
            model.TypeAvt = typeAvt;
            model.DateDebGar = AlbConvert.ConvertStrToDate(dateDeb);
            model.DateFinGar = AlbConvert.ConvertStrToDate(dateFin);
            model.Risque = risque;
            model.Garanties = garanties;

            return PartialView("ListeGarantiesRegularisation", model);
        }

        [ErrorHandler]
        public ActionResult ReloadListGarRegule(string lotId, string reguleId, string codeRsq, bool isReadonly)
        {
            var garanties = new List<ModeleGarantie>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetListGarRegule(lotId, reguleId, codeRsq, isReadonly);
                if (result != null)
                {
                    result.Garanties.ForEach(g =>
                    {
                        var garan = (ModeleGarantie)g;
                        garan.EntreeGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(g.DateDebGar));
                        garan.SortieGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(g.DateFinGar));

                        garanties.Add(garan);
                    });
                }
            }

            return PartialView("TabGarantiesRegularisation", garanties);
        }

        [ErrorHandler]
        public ActionResult OpenApplique(string codeContrat, string version, string type, string codeAvn, string codeFor)
        {
            ModeleRisque model = GetAppliqueA(codeContrat, version, type, codeAvn, codeFor);
            return PartialView("/Views/CreationRegularisationGarantie/AppliqueFormule.cshtml", model);
        }

        [ErrorHandler]
        
        public ActionResult UpdateTypeRegul(string traitement, string codeOffre, string version, string type, string datefin, string periodicite, string id)
        {
            Int32? DateEffetAvt = 0;
            if (!string.IsNullOrEmpty(datefin))
            {
                DateEffetAvt = AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(datefin).Value.AddDays(1));
            }
            var codeT = traitement.Split('-')[0];
            var libT = traitement.Split('-')[1];
            var valTypes = new List<string>();
            valTypes.Add("AVNRG - Régularisation puis avenant");

            var valDdl = new List<AlbSelectListItem>();

            valTypes.ForEach(
            elem => valDdl.Add(AlbSelectListItem.ConvertToAlbSelect(elem, ""))
            );

            ModeleLigneRegularisation obj = new ModeleLigneRegularisation();

            //using (var serviceContext = new AffaireNouvelleClient())
            //{
            //    obj = (ModeleLigneRegularisation)serviceContext.GetTypeRegul(codeOffre, version, type);
            //}

            //if (obj.LibTraitement == "")
            //    obj.LibTraitement = "Régularisation puis avenant";

            obj.CodeTraitement = codeT;
            obj.LibTraitement = libT;
            obj.TypesTraitement = valDdl;
            obj.Periodicite = periodicite;

            if (periodicite != "U" && periodicite != "E")
            {
                obj.DateDebAvn = DateEffetAvt;
            }

            //obj.DateDebAvn = obj.DateDeb;
            return PartialView("/Views/CreationRegularisation/UpdateTypeRegul.cshtml", obj);
        }

        [ErrorHandler]
        public void DoUpdateType(string codeOffre, string version, string type, string typeTraitement, string dateDebutAvn, string codeAvn, string numReg)
        {
            string newType = typeTraitement.Split('-')[0];

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                serviceContext.UpdateTypeRegul(codeOffre, version, type, newType, dateDebutAvn, codeAvn, numReg);
            }
        }

        #region CreationPeriodRegule

        [ErrorHandler]
        public ActionResult OpenPeriodGar(string codeContrat, string version, string type, string codeAvenant, string codeRsq, string idGar, string lotId, string reguleId, bool isReadonly)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                RegularisationGarInfo model = new RegularisationGarInfo();
                var result = serviceContext.GetInfoRegularisationGarantie(new RegularisationContext
                {
                    IdContrat = new IdContratDto
                    {
                        CodeOffre = codeContrat,
                        Version = Int32.Parse(version),
                        Type = type
                    },
                    ModeleAvtRegul = new AvenantRegularisationDto
                    {
                        NumAvt = Int64.Parse(codeAvenant)
                    },
                    LotId = Int64.Parse(lotId),
                    GrId = Int64.Parse(idGar),
                    RsqId = Int64.Parse(codeRsq),
                    RgId = Int64.Parse(reguleId),
                    IsReadOnlyMode = isReadonly,
                    Mode = RegularisationMode.Standard,
                    Scope = RegularisationScope.Garantie
                });

                if (result != null && !string.IsNullOrEmpty(result.ErreurStr))
                {
                    throw new AlbFoncException(result.ErreurStr, true, true, true);
                }
                else
                {
                    model = (RegularisationGarInfo)result;
                    model.idLot = lotId;
                    model.idregul = reguleId;
                    if (model.ListPeriodRegulGar != null && model.ListPeriodRegulGar.Any())
                    {
                        model.ListPeriodRegulGar.ForEach(el => el.idregul = reguleId);
                        model.ListPeriodRegulGar.ForEach(el => el.idLot = lotId);
                        model.ListPeriodRegulGar.ForEach(el => el.idGar = idGar);
                        model.ListPeriodRegulGar.ForEach(el => el.codeGar = result.GarantieInfo.CodeGarantie);
                        model.ListPeriodRegulGar.ForEach(el => el.codeOpt = result.GarantieInfo.CodeOption.ToString());
                        model.ListPeriodRegulGar.ForEach(el => el.codeRsq = codeRsq);
                        model.ListPeriodRegulGar.ForEach(el => el.codeFor = result.GarantieInfo.CodeFormule.ToString());
                        model.ListPeriodRegulGar.ForEach(el => el.libGar = result.GarantieInfo.Libelle);
                    }
                    return PartialView("CreationPeriodRegule", model);
                }
            }
        }

        [ErrorHandler]
        public ActionResult SaveLineMouvtPeriode(string codeOffre, string version, string type, string codeAvn, string dateDeb, string dateFin, int dateDebMin, int dateFinMax, string codersq, string codefor, string codegar, string idregul, string idlot)
        {
            int iDateDeb = 0;
            int iDateFin = 0;
            int.TryParse(dateDeb, out iDateDeb);
            int.TryParse(dateFin, out iDateFin);
            DateTime? dDateDebMin = AlbConvert.ConvertIntToDate(dateDebMin);
            DateTime? dDateFinMax = AlbConvert.ConvertIntToDate(dateFinMax);

            /* Controle des dates */

            if (iDateDeb > iDateFin)
            {
                throw new AlbFoncException("La date de debut doit être inférieur à la date de fin");
            }
            else if ((iDateDeb < dateDebMin) || (iDateDeb > dateFinMax))
            {
                throw new AlbFoncException("La date de début doit être comprise entre " + dDateDebMin.Value.ToString("dd/MM/yyyy") + " et " + dDateFinMax.Value.ToString("dd/MM/yyyy"));
            }
            if (iDateFin < iDateDeb)
            {
                throw new AlbFoncException("La date de fin doit être supérieur à la date de debut");
            }
            else if ((iDateFin < dateDebMin) || (iDateFin > dateFinMax))
            {
                throw new AlbFoncException("La date de fin doit être comprise entre " + dDateDebMin.Value.ToString("dd/MM/yyyy") + " et " + dDateFinMax.Value.ToString("dd/MM/yyyy"));
            }
            var result = new AjoutMouvtGarantieDto();
            var listgar = new List<PerioderegulGarantie>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                result = serviceContext.AjouterMouvtPeriod(codeOffre, version, type, codersq, codefor, codegar, idregul, idlot, iDateDeb, iDateFin);

                if (string.IsNullOrEmpty(result.StrReturn))
                {
                    result.LignesMouvement.ForEach(el =>
                    {
                        listgar.Add((PerioderegulGarantie)el);
                    });
                    return PartialView("ListPeriodRegule", listgar);
                }
                else
                {
                    throw new AlbFoncException(result.StrReturn);
                }
            }
        }

        [ErrorHandler]
        [HttpPost]
        public ActionResult SupprimerMouvtPeriode(RegularisationContext context)
        {
            var listgar = new List<PerioderegulGarantie>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.SupprimerMouvtPeriod(context);

                if (result != null && result.Any())
                {
                    result.ForEach(el =>
                    {
                        listgar.Add((PerioderegulGarantie)el);
                    });
                }
            }
            return PartialView("ListPeriodRegule", listgar);
        }

        [ErrorHandler]
        public ActionResult ReloadListPeriodReguleGar(string codeOffre, string version, string type, string codeRsq, string codeFor, string codeGar, string codeRegul, string code)
        {

            var model = new List<PerioderegulGarantie>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.ReloadMouvtPeriod(codeOffre, version, type, codeRsq, codeFor, codeGar, codeRegul);
                if (result != null && result.Any())
                {
                    result.ForEach(el =>
                    {
                        model.Add((PerioderegulGarantie)el);
                    });
                }
            }
            return PartialView("ListPeriodRegule", model);
        }

        [ErrorHandler]
        public string RetourEcran(string codeOffre, string version, string type, string codeAvn, string reguleId, string codersq, string codefor, string codegar, string dateDebReg,
            string dateFinReg, string controle)
        {
            string toReturn = "Erreur";
            if (!string.IsNullOrEmpty(controle))
            {

                if (ChekChevauchementDates(codeOffre, version, type, codeAvn, reguleId, codersq, codefor, codegar, dateDebReg, dateFinReg))

                    return string.Empty;
                else
                    return toReturn;
            }
            return string.Empty; ;
        }

        [ErrorHandler]
        public bool ChekChevauchementDates(string codeOffre, string version, string type, string codeAvn, string reguleId, string codersq, string codefor, string codegar, string dateDebReg,
            string dateFinReg)
        {
            bool toReturn = true;
            var listgar = new List<PerioderegulGarantie>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetListDatesPeriod(codeOffre, version, type, reguleId, codersq, codefor, codegar);
                if (result != null && result.Any())
                {
                    result.ForEach(el =>
                    {
                        listgar.Add((PerioderegulGarantie)el);
                    });
                    var max_Period = (from tab1 in listgar select tab1.PeriodeRegulFin).Max();
                    var min_Period = (from tab1 in listgar select tab1.PeriodeRegulDeb).Min();

                    if (max_Period != Convert.ToInt32(dateFinReg) || min_Period != Convert.ToInt32(dateDebReg))
                    {
                        toReturn = false;
                        throw new AlbFoncException("La régularisation de la garantie n'a pas été faite sur toute la période à régulariser");
                    }
                    //if (listgar[0].Situation == "V")
                    //{
                    for (int i = 1; i <= listgar.Count - 1; i++)
                    {
                        //if (listgar[i].Situation == "V")
                        //{
                        if (listgar[i].PeriodeRegulDeb < listgar[i - 1].PeriodeRegulFin)
                        {
                            int iDateDeb = 0;
                            int iDateFin = 0;
                            int.TryParse(listgar[i].PeriodeRegulDeb.ToString(), out iDateDeb);
                            int.TryParse(listgar[i - 1].PeriodeRegulFin.ToString(), out iDateFin);
                            DateTime? dDateDeb = AlbConvert.ConvertIntToDate(iDateDeb);
                            DateTime? dDateFin = AlbConvert.ConvertIntToDate(iDateFin);
                            toReturn = false;
                            throw new AlbFoncException("Attention les dates " + dDateFin.Value.ToString("dd/MM/yyyy") + " et " + dDateDeb.Value.ToString("dd/MM/yyyy") + " se chevauchent");
                        }

                        DateTime? date1 = AlbConvert.ConvertIntToDate(listgar[i - 1].PeriodeRegulFin);
                        DateTime? PeriodeFin = AlbConvert.GetFinPeriode(date1.Value, 2, AlbOpConstants.Jour);
                        Int64? PeriodeFinPlus = PeriodeFin.Value.Year * 10000 + PeriodeFin.Value.Month * 100 + PeriodeFin.Value.Day;
                        if (listgar[i].PeriodeRegulDeb > PeriodeFinPlus)
                        {
                            int iDateDeb = 0;
                            int iDateFin = 0;
                            int.TryParse(listgar[i].PeriodeRegulDeb.ToString(), out iDateDeb);
                            int.TryParse(listgar[i - 1].PeriodeRegulFin.ToString(), out iDateFin);
                            DateTime? dDateDeb = AlbConvert.ConvertIntToDate(iDateDeb);
                            DateTime? dDateFin = AlbConvert.ConvertIntToDate(iDateFin);
                            toReturn = false;
                            throw new AlbFoncException("Attention les dates " + dDateFin.Value.ToString("dd/MM/yyyy") + " et " + dDateDeb.Value.ToString("dd/MM/yyyy") + " se discontinuent");
                        }
                    }
                    //else
                    //{
                    //    toReturn = false;
                    //    throw new AlbFoncException("Veuillez régulariser toutes vos garanties, ou supprimer la/les période(s) ");
                    //}
                }
                //}
                //else
                //{
                //    toReturn = false;
                //    throw new AlbFoncException("Veuillez régulariser toutes vos garanties");
                //}
            }
            return toReturn;
        }


        #endregion

        #region SaisiePeriodRegule
        [ErrorHandler]
        public ActionResult OpenSaisiePeriodRegul(string codeContrat, string version, string type, string codeAvenant, string codeRsq, string codeFor, string codeOpt, string idGar, string lotId, string reguleId, string codeGar, string libGar, string idregulgar)
        {
            ModeleSaisieRegulGarantiePage toReturn = new ModeleSaisieRegulGarantiePage();
            toReturn.modelInfoInitregul = new SaisieInfoRegulPeriod();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.InitSaisieGarRegul(idregulgar, codeAvenant);
                toReturn.modelInfoInitregul.InfoInitregul = (InfoInitSaisieRegul)result.LignePeriodRegul;
                toReturn.modelInfoInitregul.UnitesDef = result.UnitesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "UM").ToList();
                toReturn.modelInfoInitregul.CodesTaxesDef = result.CodesTaxesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
                toReturn.modelInfoInitregul.UnitesPrev = result.UnitesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "UM").ToList();
                toReturn.modelInfoInitregul.CodesTaxesPrev = result.CodesTaxesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
                toReturn.AppliqueA = serviceContext.GetAppliqueRegule(codeContrat, version, type, codeAvenant, codeFor);
                toReturn.CodeGar = codeGar;
                toReturn.lotId = lotId;
                toReturn.codeOpt = codeOpt;
                toReturn.idGar = idGar;
                toReturn.reguleId = reguleId;
                toReturn.codeFor = codeFor;
                toReturn.codeRsq = codeRsq;
                toReturn.libGar = libGar;
                toReturn.idregulgar = idregulgar;
                // toReturn.modelInfoInitregul.InfoInitregul.TypeReguleGar_STD="GA";
                return PartialView("SaisiePeriodRegul", toReturn);

            }
        }

        [ErrorHandler]
        public ActionResult ReloadEcranSaisie(string codeContrat, string version, string type, string codeAvenant, string codeRsq,
            string assiettePrev, string valeurPrev, string unitePrev, string codetaxePrev, string assietteDef, string valeurDef, string uniteDef, string codetaxeDef,
            string cotisForceHT, string cotisForceTaxe, string coeff,
            string idregulgar)
        {
            ModeleSaisieRegulGarantiePage toReturn = new ModeleSaisieRegulGarantiePage();
            toReturn.modelInfoInitregul = new SaisieInfoRegulPeriod();
            toReturn.modelInfoInitregul.InfoInitregul = new InfoInitSaisieRegul();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.ReloadSaisieGarRegul(codeContrat, version, type, codeAvenant, idregulgar, codeRsq, assiettePrev, valeurPrev, unitePrev, codetaxePrev, assietteDef, valeurDef, uniteDef, codetaxeDef, cotisForceHT, cotisForceTaxe, coeff);
                toReturn.modelInfoInitregul.InfoInitregul = (InfoInitSaisieRegul)result.LignePeriodRegul;
                toReturn.modelInfoInitregul.UnitesDef = result.UnitesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "UM").ToList();
                toReturn.modelInfoInitregul.UnitesPrev = result.UnitesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "UM").ToList();
                toReturn.modelInfoInitregul.CodesTaxesPrev = result.CodesTaxesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
                toReturn.modelInfoInitregul.CodesTaxesDef = result.CodesTaxesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
                toReturn.modelInfoInitregul.InfoInitregul.relaod = true;
                return PartialView("DataRegularisationPeriod", toReturn.modelInfoInitregul);
            }
        }

        [ErrorHandler]
        public ActionResult GetPopupConfirmMntRegul(string reguleGarId)
        {
            var model = new ConfirmSaisieRegule();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetPopupConfirm(reguleGarId);
                if (result != null)
                {
                    model = (ConfirmSaisieRegule)result;
                }
            }
            return PartialView("ConfirmationMontantRegule", model);
        }

        [ErrorHandler]
        public JsonResult ValidSaisiePeriodRegule(string codeContrat, string version, string type, string codeAvn, string codeRsq,
            string reguleGarId, string typeRegule, string dataRow, string addParamValue)
        {
            JavaScriptSerializer serialiser = AlbJsExtendConverter<InfoInitSaisieRegul>.GetSerializer();
            var model = serialiser.Deserialize<InfoInitSaisieRegul>(dataRow);

            var modelDto = InfoInitSaisieRegul.LoadDto(model);


            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.ValidSaisiePeriodRegule(codeContrat, version, type, codeAvn, codeRsq, reguleGarId, typeRegule, modelDto);
                if (result != null)
                {
                    if (!string.IsNullOrEmpty(result))
                        throw new AlbFoncException(result, false, false, true);
                }
            }

            return IsSimplifiedRegule(codeContrat, version, type, reguleGarId, addParamValue);
        }

        [ErrorHandler]
        public JsonResult IsSimplifiedRegule(string codeContrat, string version, string type, string reguleGarId, string addParamValue)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var regulMode = GetAddParamValue(addParamValue, AlbParameterName.REGULMOD);
                var regulType = GetAddParamValue(addParamValue, AlbParameterName.REGULTYP);
                var regulNiveau = GetAddParamValue(addParamValue, AlbParameterName.REGULNIV);
                var regulAvn = GetAddParamValue(addParamValue, AlbParameterName.REGULAVN);
                var lotId = GetAddParamValue(addParamValue, AlbParameterName.LOTID);
                var typeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
                var reguleId = GetAddParamValue(addParamValue, AlbParameterName.REGULEID);
                reguleGarId = (String.IsNullOrEmpty(reguleGarId)) ? GetAddParamValue(addParamValue, AlbParameterName.REGULGARID) : reguleGarId;


                var rgContext = new RegularisationContext()
                {
                    RgId = reguleId.ContainsChars() ? Convert.ToInt64(reguleId) : default(long),
                    TypeAvt = typeAvt,
                    CodeFormule = 0,
                    IdContrat = new IdContratDto
                    {
                        CodeOffre = codeContrat,
                        Type = type,
                        Version = Convert.ToInt32(version)
                    },
                    LotId = lotId.ContainsChars() ? Convert.ToInt64(lotId) : default(long),
                    Mode = RegularisationMode.Standard,
                    RegimeTaxe = "",
                    Scope = RegularisationScope.Garantie,
                    User = GetUser(),
                    Type = regulType,
                    RgHisto = Char.Parse(regulAvn),
                    RgGrId = Int64.Parse(reguleGarId)
                };

                var isSimplifiedRegule = client.Channel.IsSimplifiedReguleFlow(rgContext);

                return new JsonResult
                {
                    Data = new
                    {
                        IsSimplifiedRegule = isSimplifiedRegule
                    }
                };
            }
        }

        #endregion

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
            var isReadonly = GetIsReadOnly(model.TabGuid, tId[0] + "_" + tId[1] + "_" + tId[2], (model.Contrat.NumAvenant).ToString(), modeAvenant: model.AvnMode);

            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);
            if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
            {
                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                var reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);

                model.InfoRegule = GetInfoCreateRegule(tId[0], tId[1], tId[2], typeAvt, reguleId == "0" ? "CREATE" : "UPDATE", isReadonly, string.Empty, reguleId, regulMode);
                var acteGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
                if (!string.IsNullOrEmpty(acteGestionRegule))
                {
                    model.AddParamValue = model.AddParamValue.Replace("ACTEGESTIONREGULE|AVNMD", "ACTEGESTIONREGULE|REGUL");
                }
                else
                {
                    model.AddParamValue += "||ACTEGESTIONREGULE|REGUL";
                }
                model.ActeGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
            }
            else
            {
                if (model.AvnMode == "UPDATE")
                {
                    var reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);
                    model.InfoRegule = GetInfoCreateRegule(tId[0], tId[1], tId[2], typeAvt, reguleId == "0" ? "CREATE" : "UPDATE", isReadonly, string.Empty, reguleId, regulMode);
                }
                else if (model.AvnMode == "CREATE")
                {
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                    var reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);

                    model.InfoRegule = GetInfoCreateRegule(tId[0], tId[1], tId[2], typeAvt, "CONSULT", isReadonly, string.Empty, reguleId, regulMode);
                    var acteGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
                    if (!string.IsNullOrEmpty(acteGestionRegule))
                    {
                        model.AddParamValue = model.AddParamValue.Replace("ACTEGESTIONREGULE|AVNMD", "ACTEGESTIONREGULE|REGUL");
                    }
                    else
                    {
                        model.AddParamValue += "||ACTEGESTIONREGULE|REGUL";
                    }
                    model.ActeGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
                }
                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
            }
        }

        private void SetContentData(ContratDto contrat)
        {

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
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

                var regule = GetInfoRegule(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.NumAvenantPage);
                model.Regularisations = new List<ModeleLigneRegularisation>();
                if (regule != null && regule.Regularisations != null && regule.Regularisations.Any())
                {
                    regule.Regularisations.RemoveAll(reg => reg.RegulMode == "PB");
                    regule.Regularisations.RemoveAll(reg => reg.RegulMode == "BNS");
                    model.Regularisations = regule.Regularisations;
                }
                model.Regularisations = regule != null && regule.Regularisations != null && regule.Regularisations.Any() ? regule.Regularisations : new List<ModeleLigneRegularisation>();
                model.Alertes = GetInfoAlertes(regule.Alertes);
                ParametreDto typeContrat = regule != null && regule.TypesContrat != null && regule.TypesContrat.Any() ? regule.TypesContrat.Find(el => el.Code == model.Contrat.TypePolice) : null;
                model.LibTypeContrat = typeContrat != null ? typeContrat.Descriptif : string.Empty;
            }
        }

        protected override ModeleNavigationArbre GetNavigationArbreRegule(MetaModelsBase contentData, string etape)
        {
            contentData.NavigationArbre = new ModeleNavigationArbre();
            if (contentData.Contrat != null && contentData.Contrat.CodeContrat != null)
            {
                var folder = new Folder(contentData.Contrat.CodeContrat, (int)contentData.Contrat.VersionContrat, contentData.Contrat.Type[0]) {
                    NumeroAvenant = int.TryParse(contentData.NumAvenantPage, out int i) && i > 0 ? i : default
                };
                contentData.NavigationArbre = BuildNavigationArbre(folder);
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaire>()) {
                    if (contentData.NumAvenantPage.ParseInt().Value > 0) {
                        var alertesAvenant = client.Channel.GetListAlertesAvenant(new AffaireId {
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
            var data = contentData as ModeleRegularisationPage;
            if (data?.Context != null)
            {
                RegularisationNavigator.Initialize(contentData.NavigationArbre, data.Context);
            }

            return contentData.NavigationArbre;
        }

        private void SetBandeauNavigation(ContratDto contrat, string id)
        {
            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            model.AfficherBandeau = true;
            // sab 16022017 bug 2295
            //model.AfficherNavigation = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? false : model.AfficherBandeau;
            model.AfficherNavigation = model.AfficherBandeau;
            var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);
            if (regulMode == "PB")
            {
                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULPB;
            }
            else if (regulMode == "BNS")
            {
                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBNS;
            }
            else if (regulMode == "BURNER")
            {
                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBURNER;
            }
            else
            {
                model.ScreenType = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF : AlbConstantesMetiers.SCREEN_TYPE_REGUL;
            }
            model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
            model.Bandeau.StyleBandeau = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF : AlbConstantesMetiers.SCREEN_TYPE_REGUL;
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

        private ModeleCreationRegule GetInfoCreateRegule(string codeContrat, string version, string type, string typeAvt, string modeAvt, bool isReadonly, string lotId, string reguleId, string regulMode)
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

                if (regulMode != "STAND" && string.IsNullOrEmpty(model.MotifAvt))
                {
                    model.MotifAvt = "M3";
                }
                if (isReadonly)
                {
                    model.ModeAvt = modeAvt;
                    model.IsReadOnly = true;
                    base.model.IsReadOnly = true;
                    model.Alertes = GetInfoAlertes(model.Alertes);
                }
                else
                {
                    model.ModeAvt = modeAvt;
                    model.IsReadOnly = false;
                    model.Alertes = GetInfoAlertes(model.Alertes);
                }

                List<AlbSelectListItem> courtiers = new List<AlbSelectListItem>();
                result.Courtiers.ForEach(el => courtiers.Add(new AlbSelectListItem { Value = el.Code.ToString(), Text = el.Code + " - " + el.Libelle, Selected = false, Title = el.Code + " - " + el.Libelle }));
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

                if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL)
                {
                    model.ReguleId = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0;
                }
            }

            return model;
        }

        private ModeleRegularisation GetInfoRegule(string codeContrat, string version, string type, string codeAvn)
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

        private List<ModeleLigneRegularisation> GetInfoRegularisation(string codeContrat, string version, string type)
        {
            List<ModeleLigneRegularisation> model = new List<ModeleLigneRegularisation>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetListeRegularisation(codeContrat, version, type);
                if (result != null && result.Any())
                {
                    result.ForEach(el =>
                    {
                        model.Add((ModeleLigneRegularisation)el);
                    });
                }
            }
            return model;
        }

        private ModeleRisque GetAppliqueA(string codeContrat, string version, string type, string codeAvn, string codeFor)
        {
            var rsq = new ModeleRisque();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetAppliqueRegule(codeContrat, version, type, codeAvn, codeFor);
                if (result != null)
                {
                    rsq = (ModeleRisque)result;
                }
            }
            return rsq;
        }

        #endregion



        [ErrorHandler]
        
        public ActionResult Step1_ChoixPeriode_Lock(string id)
        {
            return Content(true.ToString());
        }

        [ErrorHandler]
        
        public RedirectToRouteResult Step1_ChoixPeriode_FromNavig_Consult(string id)
        {

            id = HttpUtility.UrlDecode(id);
            string initialId = id;
            initialId = InitializeParams(initialId);
            //id = initialId;
            string[] tId = initialId.Split('_');
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), initialId, model.NumAvenantPage) || model.AllParameters.IsConsultOnly;

            LigneRegularisationDto result = null;
            var parameters = id.Substring(0, id.LastIndexOf("addParam") + 8).ToParamDictionary();
            var postParams = id.Substring(id.LastIndexOf("addParam") + 8);

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var serviceContext = client.Channel;

                if (!parameters.ContainsKey(AlbParameterName.REGULEID.ToString()))
                    result = serviceContext.GetLastRegularisation(tId[0], tId[1], tId[2], model.NumAvenantPage);

                else
                    result = serviceContext.GetRegularisationByID(tId[0], tId[1], tId[2], model.RgId);
            }
            if (result != null)
            {
                parameters.IncludeValue(AlbParameterName.AVNMODE, Model.IsReadOnly ? "CONSULT" : "UPDATE", Model.IsReadOnly);
                parameters.IncludeValue(AlbParameterName.REGULEID, result.NumRegule);
                parameters.IncludeValue(AlbParameterName.REGULMOD, result.RegulMode);
                parameters.IncludeValue(AlbParameterName.REGULTYP, result.RegulType);
                parameters.IncludeValue(AlbParameterName.REGULNIV, result.RegulNiv);
                parameters.IncludeValue(AlbParameterName.REGULAVN, result.RegulAvn);
                parameters.IncludeValue("TYPEAVT", result.CodeTraitement);
            }

            return RedirectToAction("Step1_ChoixPeriode", new { id = parameters.RebuildAddParamString() + postParams });
        }

        [ErrorHandler]
        public ActionResult Step1_ChoixPeriode(string id)
        {
            id = HttpUtility.UrlDecode(id);
            model.PageTitle = "Création régularisation";
            id = InitializeParams(id);
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), id, model.NumAvenantPage);
            GetPeriodeCourtierInfos(id);

            PresetContextContrat();

            return View(model);
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

            model.InfoRegule = GetInfoCreateRegule(tId[0], tId[1], tId[2], typeAvt, model.AvnMode, isReadonly || model.AvnMode == "CONSULT", model.AvnMode == "CONSULT" ? string.Empty : lotId, reguleId, regulMode);

            model.ScreenType = regulType == "S" ? AlbConstantesMetiers.SCREEN_TYPE_REGUL : AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;

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

        [ErrorHandler]
        
        [HttpPost]
        public JsonResult CancelQuittance(string lotId, string reguleId, string codeContrat, string version, string type, string typeAvt, string codeAvn,
            string exercice, string codeICT, string codeICC, string tauxCom, string tauxComCATNAT, string codeEnc, string mode,
            string souscripteur, string gestionnaire, string argModeleAvtRegule,
            string id, string addParamValue, RegularisationContext context)
        {

            ModeleAvenantRegularisation modeleAvtRegule = null;
            if (!string.IsNullOrEmpty(argModeleAvtRegule))
            {
                JavaScriptSerializer serializerModelAvtRegule = AlbJsExtendConverter<ModeleAvenantRegularisation>.GetSerializer();
                modeleAvtRegule = serializerModelAvtRegule.ConvertToType<ModeleAvenantRegularisation>(serializerModelAvtRegule.DeserializeObject(argModeleAvtRegule));
            }



            var regulMode = GetAddParamValue(addParamValue, AlbParameterName.REGULMOD);
            var regulType = GetAddParamValue(addParamValue, AlbParameterName.REGULTYP);
            var regulNiveau = GetAddParamValue(addParamValue, AlbParameterName.REGULNIV);
            var regulAvn = GetAddParamValue(addParamValue, AlbParameterName.REGULAVN);

            // B1738 Régularisations - Règle des avenants de « régul puis avenant de modif »
            var avnType = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);



            if (modeleAvtRegule == null)
            {
                var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);

                modeleAvtRegule = new ModeleAvenantRegularisation
                {
                    NumAvt = (String.IsNullOrEmpty(numAvn)) ? 0 : long.Parse(numAvn),
                    NumInterneAvt = (String.IsNullOrEmpty(numAvn)) ? 0 : long.Parse(numAvn)

                };
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var rgContext = new RegularisationContext()
                {
                    TypeAvt = typeAvt,
                    CodeEnc = codeEnc,
                    CodeFormule = 0,
                    CodeICC = codeICC,
                    CodeICT = codeICT,
                    Exercice = Convert.ToInt32("0" + exercice),
                    Gestionnaire = gestionnaire,
                    IdContrat = new IdContratDto
                    {
                        CodeOffre = codeContrat,
                        Type = type,
                        Version = Convert.ToInt32(version)
                    },
                    LotId = lotId.ContainsChars() ? Convert.ToInt64(lotId) : default(long),
                    Souscripteur = souscripteur,
                    ModeleAvtRegul = ModeleAvenantRegularisation.LoadDto(modeleAvtRegule),
                    Mode = RegularisationMode.Standard,
                    RegimeTaxe = "",
                    RgId = reguleId.ContainsChars() ? Convert.ToInt64(reguleId) : default(long),
                    Scope = RegularisationScope.Garantie,
                    Step = RegularisationStep.ChoixPeriodeCourtier,
                    User = GetUser(),
                    TauxCom = tauxCom,
                    TauxComCATNAT = tauxComCATNAT,
                    Type = regulType,
                    RgHisto = Char.Parse(regulAvn),
                    IsReadOnlyMode = mode == string.Empty,
                    IsSetReguleAlreadyCalled = true
                };

                rgContext = client.Channel.ValidateStepAndGetNext(rgContext, true);

                if (!string.IsNullOrEmpty(rgContext.Error?.Code))
                {
                    if (rgContext.Error.Code.Equals("NORISKREGUL", StringComparison.InvariantCulture) ||
                        rgContext.Error.Code.Equals("NOGARREGUL", StringComparison.InvariantCulture))
                        throw new AlbFoncException(rgContext.Error.Label, true, true, true);

                    throw new AlbFoncException("Erreur lors de la génération de l'avenant", true, true, true);
                }

                var idRegule = mode.Equals("CREATE", StringComparison.InvariantCulture) ? rgContext.RgId.ToString() : reguleId.ToString();
                var codeAvnInt = string.IsNullOrEmpty(codeAvn) ? 0 : Convert.ToInt32(codeAvn);

                var numAvn = mode.Equals("CREATE", StringComparison.InvariantCulture) ? codeAvnInt++ : codeAvnInt;

                return new JsonResult
                {
                    Data = new
                    {
                        Result = "SUCCESS",
                        IdRegule = idRegule,
                        NumAvn = numAvn,
                        IdRsq = (rgContext.SimpleRegule != null) ? rgContext.SimpleRegule.IdRsq.ToString() : "0",
                        IdGar = (rgContext.SimpleRegule != null) ? rgContext.SimpleRegule.IdGar.ToString() : "0",
                        CodeGar = (rgContext.SimpleRegule != null) ? rgContext.SimpleRegule.CodeGar : "0",
                        RgGrId = rgContext.RgGrId.ToString(),
                        IsSimplified = rgContext.IsSimplifiedRegule,
                        rgContext.IsMultiRC
                    }
                };
            }
        }


        [ErrorHandler]
        
        [HttpPost]
        public JsonResult Step1_ChoixPeriode_Next(string lotId, string reguleId, string codeContrat, string version, string type, string typeAvt, string codeAvn,
            string exercice, string dateDeb, string dateFin, string codeICT, string codeICC, string tauxCom, string tauxComCATNAT, string codeEnc, string mode,
            string souscripteur, string gestionnaire, string argModeleAvtRegule,
            string id, string addParamValue)
        {

            ModeleAvenantRegularisation modeleAvtRegule = null;
            if (!string.IsNullOrEmpty(argModeleAvtRegule))
            {
                JavaScriptSerializer serializerModelAvtRegule = AlbJsExtendConverter<ModeleAvenantRegularisation>.GetSerializer();
                modeleAvtRegule = serializerModelAvtRegule.ConvertToType<ModeleAvenantRegularisation>(serializerModelAvtRegule.DeserializeObject(argModeleAvtRegule));
            }



            var regulMode = GetAddParamValue(addParamValue, AlbParameterName.REGULMOD);
            var regulType = GetAddParamValue(addParamValue, AlbParameterName.REGULTYP);
            var regulNiveau = GetAddParamValue(addParamValue, AlbParameterName.REGULNIV);
            var regulAvn = GetAddParamValue(addParamValue, AlbParameterName.REGULAVN);

            // B1738 Régularisations - Règle des avenants de « régul puis avenant de modif »
            var avnType = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            CheckEndRegulPeriodDate(codeContrat, version, type, dateFin, avnType);


            if (modeleAvtRegule == null)
            {
                var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);

                modeleAvtRegule = new ModeleAvenantRegularisation
                {
                    NumAvt = (String.IsNullOrEmpty(numAvn)) ? 0 : long.Parse(numAvn),
                    NumInterneAvt = (String.IsNullOrEmpty(numAvn)) ? 0 : long.Parse(numAvn)

                };
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var rgContext = new RegularisationContext()
                {
                    TypeAvt = typeAvt,
                    CodeEnc = codeEnc,
                    CodeFormule = 0,
                    CodeICC = codeICC,
                    CodeICT = codeICT,
                    DateDebut = dateDeb,
                    DateFin = dateFin,
                    Exercice = Convert.ToInt32("0" + exercice),
                    Gestionnaire = gestionnaire,
                    IdContrat = new IdContratDto
                    {
                        CodeOffre = codeContrat,
                        Type = type,
                        Version = Convert.ToInt32(version)
                    },
                    LotId = lotId.ContainsChars() ? Convert.ToInt64(lotId) : default,
                    Souscripteur = souscripteur,
                    ModeleAvtRegul = ModeleAvenantRegularisation.LoadDto(modeleAvtRegule),
                    Mode = RegularisationMode.Standard,
                    RegimeTaxe = "",
                    RgId = reguleId.ContainsChars() ? Convert.ToInt64(reguleId) : default,
                    Scope = RegularisationScope.Garantie,
                    Step = RegularisationStep.ChoixPeriodeCourtier,
                    User = GetUser(),
                    TauxCom = tauxCom,
                    TauxComCATNAT = tauxComCATNAT,
                    Type = regulType,
                    RgHisto = char.Parse(regulAvn),
                    IsReadOnlyMode = mode == string.Empty
                };

                rgContext = client.Channel.ValidateStepAndGetNext(rgContext);

                if (!string.IsNullOrEmpty(rgContext.Error?.Code))
                {
                    if (rgContext.Error.Code.Equals("NORISKREGUL", StringComparison.InvariantCulture) ||
                        rgContext.Error.Code.Equals("NOGARREGUL", StringComparison.InvariantCulture))
                        throw new AlbFoncException(rgContext.Error.Label, true, true, true);

                    throw new AlbFoncException("Erreur lors de la génération de l'avenant", true, true, true);
                }

                var idRegule = mode.Equals("CREATE", StringComparison.InvariantCulture) ? rgContext.RgId.ToString() : reguleId.ToString();
                var codeAvnInt = string.IsNullOrEmpty(codeAvn) ? 0 : Convert.ToInt32(codeAvn);

                var numAvn = mode.Equals("CREATE", StringComparison.InvariantCulture) ? codeAvnInt++ : codeAvnInt;

                return new JsonResult
                {
                    Data = new
                    {
                        Result = "SUCCESS",
                        IdRegule = idRegule,
                        NumAvn = numAvn,
                        IdRsq = rgContext.SimpleRegule is null ? (rgContext.NbRisques == 1 ? rgContext.RsqId.ToString() : "0") : rgContext.SimpleRegule.IdRsq.ToString(),
                        IdGar = rgContext.SimpleRegule is null ? (rgContext.NbGaranties == 1 || rgContext.IsMultiRC ? rgContext.GrId.ToString() : "0") : rgContext.SimpleRegule.IdGar.ToString(),
                        CodeGar = rgContext.SimpleRegule is null ? "0" : rgContext.SimpleRegule.CodeGar,
                        RgGrId = rgContext.RgGrId.ToString(),
                        IsSimplified = rgContext.IsSimplifiedRegule,
                        rgContext.IsMultiRC,
                        rgContext.NbRisques,
                        rgContext.NbGaranties
                    }
                };
            }
        }


        [ErrorHandler]
        
        public ActionResult Step2_ChoixRisque(string id)
        {
            id = HttpUtility.UrlDecode(id);
            string outId;
            model.PageTitle = "Choix des risques";
            outId = InitializeParams(id);
            Step2_ChoixRisque_LoadInfo(id);

            return View(model);
        }

        private void Step2_ChoixRisque_LoadInfo(string id)
        {

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext = client.Channel;
                var infosBase = serviceContext.LoadInfosBase(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenantPage, model.ModeNavig);
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
            }

            SetContentData(model.Contrat);
            //model.AddParamValue += "||ACTEGESTIONREGULE|REGUL";
            var reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);
            var lotId = GetAddParamValue(model.AddParamValue, AlbParameterName.LOTID);
            var mode = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNMODE);
            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            model.ActeGestionRegule = (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF) ? AlbConstantesMetiers.TYPE_AVENANT_REGUL : AlbConstantesMetiers.TYPE_AVENANT_MODIF;
            var isReadonly = GetIsReadOnly(model.TabGuid, $"{model.CodePolicePage}_{model.VersionPolicePage}_{model.TypePolicePage}", (model.Contrat.NumAvenant).ToString(), modeAvenant: mode);
            model.IsReadOnly = isReadonly;

            model.ModelRisques = new ModeleRisquesRegularisation();
            var rsq = new List<ModeleRisque>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetListRsqRegule(lotId, reguleId, mode);

                if (result != null)
                {
                    if (!string.IsNullOrEmpty(result.ErreurAvt))
                    {
                        if (result.CodeErreurAvt.Equals("NO_RISK_REGUL", StringComparison.InvariantCulture))
                            throw new AlbFoncException(result.ErreurAvt, true, true, true);

                        throw new AlbFoncException("Erreur lors de la génération de l'avenant", true, true, true);
                    }
                    result.Risques.ForEach(r =>
                    {
                        var objets = new List<ModeleObjet>();

                        r.Objets.ForEach(o =>
                        {
                            var objet = (ModeleObjet)o;
                            objet.Cible = new ParametreDto { Code = o.Cible.Code, Libelle = o.Cible.Nom };
                            objets.Add(objet);
                        });

                        var risque = (ModeleRisque)r;
                        risque.Cible = new ParametreDto { Code = r.Cible.Code, Libelle = r.Cible.Nom };
                        risque.Objets = objets;
                        rsq.Add(risque);
                    });
                    reguleId = result.ReguleId.ToString();
                    model.ModelRisques.DateDebRsq = result.PeriodeRegule.DateDeb;
                    model.ModelRisques.DateFinRsq = result.PeriodeRegule.DateFin;
                }
            }
            model.ModelRisques.CodeAvn = Convert.ToInt32(model.NumAvenantPage);
            model.ModelRisques.TypeAvt = typeAvt;
            model.ModelRisques.Risques = rsq;

            RegularisationNavigator.StandardInitContext(model, RegularisationStep.ChoixRisques);
            SetBandeauNavigation(model.Contrat, id);
            if (!model.NavigationArbre.IsMonoRisque)
            {
                model.IsValidRegul = IsValidRegul(reguleId);
            }
        }


        [ErrorHandler]
        
        public ActionResult Step3_ChoixGarantie(string id)
        {
            id = HttpUtility.UrlDecode(id);
            string outId;
            model.PageTitle = "Choix des garanties";
            outId = InitializeParams(id);
            LoadListeGarantiesInfos(id);
            return View(model);
        }

        private void LoadListeGarantiesInfos(string id)
        {
            model.ModelGaranties = new ModeleGarantiesRegularisation();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext = client.Channel;
                var infosBase = serviceContext.LoadInfosBase(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenantPage, model.ModeNavig);
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
            }

            SetContentData(model.Contrat);
            var reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);
            var lotId = GetAddParamValue(model.AddParamValue, AlbParameterName.LOTID);
            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            var codeRsq = GetAddParamValue(model.AddParamValue, AlbParameterName.RSQID);

            if (!string.IsNullOrWhiteSpace(reguleId))
            {
                model.InfoRegule = new ModeleCreationRegule()
                {
                    ReguleId = Int32.Parse(reguleId),
                    LotId = Int32.Parse(lotId)
                };
            }

            var garanties = new List<ModeleGarantie>();
            var risque = new ModeleRisque();
            var isReadonly = GetIsReadOnly(model.TabGuid, $"{model.CodePolicePage}_{model.VersionPolicePage}_{model.TypePolicePage}", (model.Contrat.NumAvenant).ToString());
            model.IsReadOnly = isReadonly;
            RegularisationGarDto result;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                result = client.Channel.GetListGarRegule(lotId, reguleId, codeRsq, isReadonly);
            }

            if (result != null)
            {
                risque = (ModeleRisque)result.Risque;
                risque.EntreeGarantie = result.Risque.DateEntreeDescr;
                risque.SortieGarantie = result.Risque.DateSortieDescr;
                result.Garanties.ForEach(g =>
                {
                    var garan = (ModeleGarantie)g;
                    garan.EntreeGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(g.DateDebGar));
                    garan.SortieGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(g.DateFinGar));
                    garanties.Add(garan);
                });

                model.ModelGaranties.DateDebGar = result.PeriodeRegule.DateDeb;
                model.ModelGaranties.DateFinGar = result.PeriodeRegule.DateFin;
            }

            model.ModelGaranties.CodeAvn = Convert.ToInt32(model.NumAvenantPage);
            model.ModelGaranties.TypeAvt = typeAvt;

            model.ModelGaranties.Risque = risque;
            model.ModelGaranties.Garanties = garanties;
            RegularisationNavigator.StandardInitContext(model, RegularisationStep.ChoixGaranties, Int64.Parse(codeRsq));
            SetBandeauNavigation(model.Contrat, id);
            if (model.NavigationArbre.IsMonoRisque || model.NavigationArbre.IsMonoGarantie)
            {
                model.IsValidRegul = IsValidRegul(reguleId);
            }
        }


        [ErrorHandler]
        
        public ActionResult Step4_ChoixPeriodeGarantie(string id)
        {
            id = HttpUtility.UrlDecode(id);
            string outId;
            model.PageTitle = "Choix des périodes";
            outId = InitializeParams(id);

            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), outId, model.NumAvenantPage);

            LoadListePeriodesGarantieInfos(id);
            return View(model);
        }

        private void LoadListePeriodesGarantieInfos(string id)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext = client.Channel;
                var infosBase = serviceContext.LoadInfosBase(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenantPage, model.ModeNavig);
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
            }

            SetContentData(model.Contrat);
            string reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);
            string lotId = GetAddParamValue(model.AddParamValue, AlbParameterName.LOTID);
            string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            string codeRsq = GetAddParamValue(model.AddParamValue, AlbParameterName.RSQID);
            string idGar = GetAddParamValue(model.AddParamValue, AlbParameterName.GARID);

            if (string.IsNullOrEmpty(idGar))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = client.Channel;
                    var matrice = serviceContext.GetRegulMatrice(model.Contrat.CodeContrat, (int)model.Contrat.VersionContrat, model.Contrat.Type, lotId, model.RgId);

                    var garIds = matrice.Select(m => new { m.GarId, m.GarLabel }).Distinct();
                    int nbGar = garIds.Count();

                    if (nbGar == 1)
                    {
                        idGar = garIds.FirstOrDefault().GarId.ToString();
                        model.AddParamValue += $"||GARID|{idGar}";
                    }
                    else if (nbGar > 1)
                    {
                        if (garIds.All(g => g.GarLabel.Trim().IsIn(AlbOpConstants.RCFrance, AlbOpConstants.RCExport, AlbOpConstants.RCUSA)))
                        {
                            idGar = garIds.First(g => g.GarLabel.Trim() == AlbOpConstants.RCFrance).GarId.ToString();
                            model.AddParamValue += $"||GARID|{idGar}";
                        }
                    }
                }
            }

            // End Bug 2184

            RegularisationGarInfoDto result;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var isReadonly = GetIsReadOnly(model.TabGuid, $"{model.CodePolicePage}_{model.VersionPolicePage}_{model.TypePolicePage}", (model.NumAvenantPage).ToString());
                model.IsReadOnly = isReadonly;

                model.GarInfo = new RegularisationGarInfo();
                result = client.Channel.GetInfoRegularisationGarantie(new RegularisationContext
                {
                    IdContrat = new IdContratDto
                    {
                        CodeOffre = model.CodePolicePage,
                        Version = int.Parse(model.VersionPolicePage),
                        Type = model.TypePolicePage
                    },
                    ModeleAvtRegul = new AvenantRegularisationDto
                    {
                        NumAvt = long.Parse(model.NumAvenantPage)
                    },
                    LotId = long.Parse(lotId),
                    GrId = long.Parse(idGar),
                    RsqId = long.Parse(codeRsq),
                    RgId = long.Parse(reguleId),
                    IsReadOnlyMode = isReadonly,
                    Mode = RegularisationMode.Standard,
                    Scope = RegularisationScope.Garantie,
                    TypeAvt = typeAvt
                });
            }

            if (result != null && !string.IsNullOrEmpty(result.ErreurStr))
            {
                throw new AlbFoncException(result.ErreurStr, true, true, true);
            }
            else
            {
                model.GarInfo = (RegularisationGarInfo)result;
                model.GarInfo.idLot = lotId;
                model.GarInfo.idregul = reguleId;
                model.GarInfo.GrId = long.Parse(idGar);
                if (model.GarInfo.ListPeriodRegulGar != null && model.GarInfo.ListPeriodRegulGar.Any())
                {
                    model.GarInfo.ListPeriodRegulGar.ForEach(el => el.idregul = reguleId);
                    model.GarInfo.ListPeriodRegulGar.ForEach(el => el.idLot = lotId);
                    model.GarInfo.ListPeriodRegulGar.ForEach(el => el.idGar = idGar);
                    model.GarInfo.ListPeriodRegulGar.ForEach(el => el.codeGar = result.GarantieInfo.CodeGarantie);
                    model.GarInfo.ListPeriodRegulGar.ForEach(el => el.codeOpt = result.GarantieInfo.CodeOption.ToString());
                    model.GarInfo.ListPeriodRegulGar.ForEach(el => el.codeRsq = codeRsq);
                    model.GarInfo.ListPeriodRegulGar.ForEach(el => el.codeFor = result.GarantieInfo.CodeFormule.ToString());
                    model.GarInfo.ListPeriodRegulGar.ForEach(el => el.libGar = result.GarantieInfo.Libelle);
                }
            }

            RegularisationNavigator.StandardInitContext(model, RegularisationStep.ChoixPeriodesGarantie, Int64.Parse(codeRsq), Int64.Parse(idGar));
            SetBandeauNavigation(model.Contrat, id);
            this.model.IsValidRegul = false;
            if (this.model.NavigationArbre.IsMonoRisque && (this.model.NavigationArbre.IsMonoGarantie || this.model.Context.IsMultiRC))
            {
                this.model.IsValidRegul = IsValidRegul(reguleId);
            }
        }


        [ErrorHandler]
        
        public ActionResult Step5_RegulContrat(string id)
        {
            id = HttpUtility.UrlDecode(id);
            string outId;
            model.PageTitle = "Saisie de régularisation";
            outId = InitializeParams(id);
            Step5_RegulContrat_LoadInfo(id);
            return View(model);
        }
        public void Step5_RegulContrat_LoadInfo(string id)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext = client.Channel;
                var infosBase = serviceContext.LoadInfosBase(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenantPage, model.ModeNavig);
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
            }

            SetContentData(model.Contrat);
            model.SaisieRegulGarantiePage = new ModeleSaisieRegulGarantiePage();
            model.SaisieRegulGarantiePage.modelInfoInitregul = new SaisieInfoRegulPeriod();
            model.SaisieRegulGarantiePage.reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);
            model.SaisieRegulGarantiePage.lotId = GetAddParamValue(model.AddParamValue, AlbParameterName.LOTID);
            model.SaisieRegulGarantiePage.codeRsq = GetAddParamValue(model.AddParamValue, AlbParameterName.RSQID);
            model.SaisieRegulGarantiePage.idGar = GetAddParamValue(model.AddParamValue, AlbParameterName.GARID);
            model.SaisieRegulGarantiePage.idregulgar = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULGARID);
            model.AvnMode = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNMODE);

            var isReadonly = GetIsReadOnly(model.TabGuid, $"{model.CodePolicePage}_{model.VersionPolicePage}_{model.TypePolicePage}", (model.NumAvenantPage).ToString(), modeAvenant: model.AvnMode);
            model.IsReadOnly = isReadonly;

            SaisieInfoRegulPeriodDto infosPeriode;
            RisqueDto appliqueA;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                infosPeriode = client.Channel.InitSaisieGarRegul(model.SaisieRegulGarantiePage.idregulgar, model.NumAvenantPage);
                appliqueA = client.Channel.GetAppliqueRegule(
                    model.CodePolicePage,
                    model.VersionPolicePage,
                    model.TypePolicePage,
                    model.NumAvenantPage,
                    infosPeriode.CodeFormule.ToString());
            }

            model.SaisieRegulGarantiePage.modelInfoInitregul.InfoInitregul = (InfoInitSaisieRegul)infosPeriode.LignePeriodRegul;
            model.SaisieRegulGarantiePage.modelInfoInitregul.UnitesDef = infosPeriode.UnitesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "UM").ToList();
            model.SaisieRegulGarantiePage.modelInfoInitregul.CodesTaxesDef = infosPeriode.CodesTaxesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
            model.SaisieRegulGarantiePage.modelInfoInitregul.UnitesPrev = infosPeriode.UnitesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "UM").ToList();
            model.SaisieRegulGarantiePage.modelInfoInitregul.CodesTaxesPrev = infosPeriode.CodesTaxesDef.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
            model.SaisieRegulGarantiePage.AppliqueA = appliqueA;
            model.SaisieRegulGarantiePage.CodeGar = infosPeriode.CodeGarantie;
            model.SaisieRegulGarantiePage.codeOpt = infosPeriode.CodeOption.ToString();
            model.SaisieRegulGarantiePage.codeFor = infosPeriode.CodeFormule.ToString();
            model.SaisieRegulGarantiePage.libGar = infosPeriode.Libelle;

            RegularisationMode reguleMode = RegularisationMode.Unknow;
            string mode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);

            if (mode == "STAND")
            {
                reguleMode = RegularisationMode.Standard;
            }
            else if (mode == "PB")
            {
                reguleMode = RegularisationMode.PB;
            }
            else if (mode == "BNS")
            {
                reguleMode = RegularisationMode.BNS;
            }
            else if (mode == "BURNER")
            {
                reguleMode = RegularisationMode.Burner;
            }


            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                model.Context = new RegularisationContext()
                {
                    IdContrat = new IdContratDto() { CodeOffre = model.Contrat.CodeContrat, Type = model.Contrat.Type, Version = (int)model.Contrat.VersionContrat },
                    RgId = Convert.ToInt64(model.SaisieRegulGarantiePage.reguleId),
                    LotId = Convert.ToInt64(model.SaisieRegulGarantiePage.lotId),
                    RsqId = Convert.ToInt64(model.SaisieRegulGarantiePage.codeRsq),
                    GrId = Convert.ToInt64(model.SaisieRegulGarantiePage.idGar),
                    RgGrId = Convert.ToInt64(model.SaisieRegulGarantiePage.idregulgar),
                    Mode = reguleMode

                };
                model.SaisieRegulGarantiePage.modelInfoInitregul.IsSimplifiedRegul = client.Channel.IsSimplifiedReguleFlow(model.Context);
            }



            RegularisationNavigator.StandardInitContext(
            model,
            RegularisationStep.Regularisation,
            Int64.Parse(model.SaisieRegulGarantiePage.codeRsq),
            Int64.Parse(model.SaisieRegulGarantiePage.idGar));

            SetBandeauNavigation(model.Contrat, id);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "codeOffre;version;type;codeAvn", Duration = 60)]
        public JsonResult GetTypeReguls(string codeOffre, string version, string type, string codeAvn)
        {

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var serviceContext = client.Channel;
                var listTypeRegul = serviceContext.GetModeleTypeRegul(codeOffre, version, type, codeAvn);
                listTypeRegul.Remove(listTypeRegul.FirstOrDefault(i => i.Code.Equals("PB", StringComparison.InvariantCulture)));
                listTypeRegul.Remove(listTypeRegul.FirstOrDefault(i => i.Code.Equals("BNS", StringComparison.InvariantCulture)));

                JsonResult toReturn = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = (listTypeRegul.Any()) ? listTypeRegul.Where(typ => !typ.Code.Equals("LTA")).ToList() : listTypeRegul
                };
                return toReturn;
            }
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult RedirectionRegul(string action, string controller, string codeAffaire, int version, string type, string numeroAvenant, string tabGuid, string addParam, NavAction? navAction, ModeConsultation modeNavig, bool returnHome = false)
        {
            if (returnHome) {
                return RedirectToAction("AutoUnlock", "Redirection", new { id = tabGuid });
            }
            RedirectMonoRsqMonGar(ref action, ref addParam, navAction, codeAffaire, version, type);
            if (action?.ToUpperInvariant() == "INDEX" && controller?.ToUpperInvariant() == "CREATIONREGULARISATION")
            {
                var avnID = GetAddParamValue(addParam, AlbParameterName.AVNID);
                if (avnID != numeroAvenant)
                {
                    SetAddParamValue(ref addParam, AlbParameterName.AVNID, numeroAvenant);
                }
            }
            var id = $"{codeAffaire}_{version}_{type}_{numeroAvenant}{GetSurroundedTabGuid(tabGuid)}addParamAVN|||{addParam}addParam{AlbParameters.ModeNavigKey}{modeNavig.AsCode()}{AlbParameters.ModeNavigKey}";
            return RedirectToAction(action, controller, new { id });
        }

        private void RedirectMonoRsqMonGar(ref string action, ref string addParam, NavAction? navAction, string codeAffaire, int version, string type)
        {
            if (!Enum.TryParse(action, out RegulSteps step)) {
                return;
            }
            var parameters = addParam.ToParamDictionary();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                string lotId = parameters[AlbParameterName.LOTID.ToString()];
                var matrice = serviceContext.GetRegulMatrice(codeAffaire, version, type, lotId);
                bool isOnlyRC = matrice.Count < 4 && matrice.Select(x => x.Formule).Distinct().Count() == 1
                    && matrice.All(x => x.GarLabel.IsIn(AlbOpConstants.RCFrance, AlbOpConstants.RCUSA, AlbOpConstants.RCExport));

                // step is current except when navAction is null, it's the target
                if (step == RegulSteps.Step2_ChoixRisque || step == RegulSteps.Step3_ChoixGarantie)
                {
                    var risqIds = matrice.Select(m => m.RisqueId).Distinct();
                    bool isMonoRisque = risqIds.Count() == 1;
                    switch (step)
                    {
                        case RegulSteps.Step2_ChoixRisque:
                            if (isMonoRisque) {
                                if (navAction == NavAction.Next) {
                                    var garIds = matrice.Select(m => m.GarId).Distinct();

                                    if (garIds.Count() == 1 || isOnlyRC) {
                                        long gid = isOnlyRC ? matrice.First(g => g.GarLabel == AlbOpConstants.RCFrance).GarId : garIds.First();
                                        parameters[AlbParameterName.RSQID.ToString()] = risqIds.FirstOrDefault().ToString();
                                        parameters[AlbParameterName.GARID.ToString()] = gid.ToString();
                                        action = RegulSteps.Step4_ChoixPeriodeGarantie.ToString();
                                    }
                                    else {
                                        parameters[AlbParameterName.RSQID.ToString()] = risqIds.FirstOrDefault().ToString();
                                        action = RegulSteps.Step3_ChoixGarantie.ToString();
                                    }
                                }
                                else {
                                    parameters.Remove(AlbParameterName.GARID.ToString());
                                    parameters.Remove(AlbParameterName.RSQID.ToString());
                                    parameters.Remove(AlbParameterName.REGULGARID.ToString());
                                    action = navAction.HasValue ? RegulSteps.Step1_ChoixPeriode.ToString() : step.ToString();
                                }
                                addParam = parameters.RebuildAddParamString();
                            }
                            break;
                        case RegulSteps.Step3_ChoixGarantie:
                            if (!parameters.ContainsKey(AlbParameterName.RSQID.ToString()) && isMonoRisque) {
                                parameters[AlbParameterName.RSQID.ToString()] = risqIds.First().ToString();
                            }
                            var risqueId = parameters[AlbParameterName.RSQID.ToString()];
                            var garIds2 = matrice.Where(m => m.RisqueId == Convert.ToInt32(risqueId)).Select(m => m.GarId).Distinct();

                            if (garIds2.Count() == 1 || isOnlyRC) {
                                long gid = isOnlyRC ? matrice.First(g => g.GarLabel == AlbOpConstants.RCFrance).GarId : garIds2.First();
                                if (navAction == NavAction.Next) {
                                    parameters[AlbParameterName.GARID.ToString()] = gid.ToString();
                                    action = RegulSteps.Step4_ChoixPeriodeGarantie.ToString();
                                }
                                else if (navAction == NavAction.Previous) {
                                    if (isMonoRisque) {
                                        parameters.Remove(AlbParameterName.GARID.ToString());
                                        parameters.Remove(AlbParameterName.RSQID.ToString());
                                        parameters.Remove(AlbParameterName.REGULGARID.ToString());
                                        action = RegulSteps.Step1_ChoixPeriode.ToString();
                                    }
                                    else {
                                        parameters.Remove(AlbParameterName.GARID.ToString());
                                        parameters.Remove(AlbParameterName.RSQID.ToString());
                                        parameters.Remove(AlbParameterName.REGULGARID.ToString());
                                        action = RegulSteps.Step2_ChoixRisque.ToString();
                                    }
                                }
                                else if (navAction is null) {
                                    parameters.Remove(AlbParameterName.GARID.ToString());
                                    parameters.Remove(AlbParameterName.REGULGARID.ToString());
                                    action = step.ToString();
                                }
                            }
                            addParam = parameters.RebuildAddParamString();
                            break;
                    }

                }
            }
        }

        private bool IsValidRegul(string reguleId)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                return serviceContext.IsValidRegul(reguleId);
            }
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
        /// <summary>
        /// B1738
        /// Dans le cas où un utilisateur crée une régularisation en mode « régul. + avenant »,
        /// si la date de fin de période de régularisation correspond à la date de fin d’effet du contrat, un contrôle bloquant doit être effectué sur l’écran « Choix de la période de régularisation »
        /// fin de lui indiquer qu’il doit effectuer une régularisation en mode « régul.seule ».
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="endRegulPeriodStringDate"></param>
        /// <param name="avnType"></param>
        private void CheckEndRegulPeriodDate(string codeContrat, string version, string type, string endRegulPeriodStringDate, string avnType)
        {

            try
            {
                // Si avnType = régul puis avenant de modif
                if (avnType == "AVNRG")
                {
                    using (var clientContrat = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        var endEffectDateContrat = clientContrat.Channel.GetEndEffectDate(codeContrat, version, type);

                        if (endEffectDateContrat != null && !string.IsNullOrEmpty(endRegulPeriodStringDate))
                        {
                            //  Vérification de la date fin effet
                            if (endEffectDateContrat.FinEffetAnnee != 0 && endEffectDateContrat.FinEffetMois != 0 && endEffectDateContrat.FinEffetJour != 0)
                            {
                                var endEffectDate = new DateTime(endEffectDateContrat.FinEffetAnnee, endEffectDateContrat.FinEffetMois, endEffectDateContrat.FinEffetJour);
                                var endRegulPeriodDate = AlbConvert.ConvertStrToDate(endRegulPeriodStringDate);
                                if (endEffectDate != null && endRegulPeriodDate != null)
                                {
                                    if (endRegulPeriodDate.Value.Date >= endEffectDate.Date)
                                    {
                                        // B1738
                                        // Message à présenter (sur 2 lignes):
                                        // La date de fin de période de régularisation correspond à la date de fin d’effet du contrat.
                                        // Veuillez choisir le mode "régul. seule" pour créer votre régularisation.
                                        throw new AlbFoncException(string.Format("{0}<br>{1}", "La date de fin de période de régularisation correspond à la date de fin d’effet du contrat.", "Veuillez choisir le mode \"régul.seule\" pour créer votre régularisation."), true, true, true);
                                    }
                                }
                            }


                        }
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public enum NavAction
        {
            Previous = 1,
            Next = 2
        }
    }
}
