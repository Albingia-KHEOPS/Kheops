using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.DataAccess;
using OP.Services.BLServices.Regularisations;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Attestation;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Cotisations;
using OP.WSAS400.DTO.Engagement;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.FormuleGarantie.GarantieModele;
using OP.WSAS400.DTO.Inventaires;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Regularisation;
using OP.WSAS400.DTO.Traite;
using OP.WSAS400.DTO.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OP.Services.BLServices
{
    public class BLCommon
    {
        #region Adresse

        private static List<string> motsIgnores = new List<string> { " RUE ", " BLD ", " BD ", " AVE ", " AV ", " AVENUE ", " BOULEVARD ", " RTE ", " ROUTE ", " DU ", " DES ", " LES ", " LE ", " LA ", " ET ", " DE ", " L'", " D'", " Z'", " ST ", " STE ", " ZI ", " PLACE ", " QUAI ", " À ", " LIEU ", " DIT ", " SAINT ", " SAINT-", " SAINTE ", " SAINTE-", " CEDEX " };

        public static List<CPVilleDto> GetCodePostalVille(string value, string mode)
        {
            if (mode == "VILLE")
                value = BuildWhereCondition(value.Trim());

            return !string.IsNullOrEmpty(value.Trim()) ? AdresseHexaviaRepository.GetCodePostalVille(value.Trim(), mode) : new List<CPVilleDto>();
        }

        #endregion

        #region Avenant

        #region Ecran Création Avenant

        public static AvenantInfoPageDto GetInfoAvenantPage(string codeOffre, string version, string type, string codeAvn, string typeAvt, string modeAvt, string user, string modeNavig)
        {
            AvenantInfoPageDto model = new AvenantInfoPageDto();

            model.avenant = GetInfoAvenant(codeOffre, version, type, codeAvn, typeAvt, modeAvt, user, modeNavig);

            model.contrat = AffaireNouvelleRepository.GetInfoBaseAvenant(codeOffre, version, type);

            return model;
        }

        public static AvenantDto GetInfoAvenant(string codeOffre, string version, string type, string codeAvn, string typeAvt, string modeAvt, string user, string modeNavig)
        {
            AvenantDto model = new AvenantDto();

            switch (typeAvt)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                    model.AvenantModif = AvenantRepository.GetInfoAvenantModification(codeOffre, version, type, Convert.ToInt16(codeAvn), typeAvt, modeAvt, modeNavig);
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    model.AvenantResil = AvenantRepository.GetInfoAvenantResiliation(codeOffre, version, type, Convert.ToInt16(codeAvn), typeAvt, modeAvt, modeNavig);
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    model.AvenantRemiseEnVigueur = AvenantRepository.GetInfoAvenantRemiseEnVig(codeOffre, version, type, Convert.ToInt16(codeAvn), typeAvt, modeAvt, modeNavig);
                    break;
                default:
                    model.AvenantModif = AvenantRepository.GetInfoAvenantModification(codeOffre, version, type, Convert.ToInt16(codeAvn), typeAvt, modeAvt, modeNavig);
                    break;

            }
            model.Alertes = AvenantRepository.GetListAlertesAvenant(codeOffre, version, type, user);
            if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR && !string.IsNullOrEmpty(model.AvenantRemiseEnVigueur.ErrorAvt))
            {
                model.Alertes.Add(new AvenantAlerteDto() { LienAlerte = string.Empty, TypeBloquante = string.Empty, MessageAlerte = model.AvenantRemiseEnVigueur.ErrorAvt, TypeAlerte = string.Empty });
                model.AvenantRemiseEnVigueur.ErrorAvt = string.Empty;
            }

            model.TypesContrat = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "TYPOC");

            return model;
        }


        #endregion

        #region Ecran Infos Générales Avenant

        public static AvenantInfoDto GetAvenant(string codeOffre, string version, string type, string codeAvenant, ModeConsultation modeNavig)
        {
            AvenantInfoDto model = new AvenantInfoDto();

            model.Avenant = AvenantRepository.GetAvenant(codeOffre, version, type, codeAvenant, modeNavig);

            model.Periodicites = CommonRepository.GetParametres(model.Avenant.Branche, model.Avenant.Cible, "PRODU", "PBPER");

            model.NaturesContrat = CommonRepository.GetParametres(model.Avenant.Branche, model.Avenant.Cible, "PRODU", "PBNPL");
            model.Durees = CommonRepository.GetParametres(model.Avenant.Branche, model.Avenant.Cible, "PRODU", "PBCTU");
            model.RegimesTaxe = CommonRepository.GetParametres(model.Avenant.Branche, model.Avenant.Cible, "GENER", "TAXRG");
            model.Antecedents = CommonRepository.GetParametres(model.Avenant.Branche, model.Avenant.Cible, "PRODU", "PBANT");
            model.Stops = CommonRepository.GetParametres(model.Avenant.Branche, model.Avenant.Cible, "PRODU", "PBSTP");
            model.Motifs = CommonRepository.GetParametres(model.Avenant.Branche, model.Avenant.Cible, "PRODU", "PBAVC");
            //model.Devises = CommonRepository.GetParametres(branche, cible, "GENER", "DEVIS");

            if (model.Avenant != null)
            {
                model.MotsClef = PoliceRepository.ObtenirMotClef(model.Avenant.Branche, model.Avenant.Cible);
                model.Avenant.DateEffetAvenant = AlbConvert.ConvertIntToDate(model.Avenant.DateEffetAvenantAnnee * 10000 + model.Avenant.DateEffetAvenantMois * 100 + model.Avenant.DateEffetAvenantJour);
                model.Avenant.HeureEffetAvenant = AlbConvert.ConvertIntToTime(model.Avenant.HeureAvn);
                model.Indices = CommonRepository.GetParametres(model.Avenant.Branche, model.Avenant.Cible, "GENER", "INDIC");
                model.HasOppBenef = model.Avenant.NbOppBenef > 0;
            }
            else
            {
                model.MotsClef = PoliceRepository.ObtenirMotClef(string.Empty, string.Empty);
                model.Indices = CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "INDIC");
            }

            //var echeances = FinOffreRepository.GetAllEcheances(codeOffre, version, type, codeAvenant, modeNavig);
            //model.ExistEcheancier = echeances != null && echeances.Any();
            model.ExistEcheancier = FinOffreRepository.PossedeEcheances(codeOffre, version, type, codeAvenant, modeNavig);

            var result = AvenantRepository.GetProchaineEcheanceHisto(codeOffre, version, type, codeAvenant);
            if (result != null)
            {
                model.PeriodiciteHisto = result.StrReturnCol;
                model.ProchEchHisto = AlbConvert.ConvertIntToDate(Convert.ToInt32(result.Int64ReturnCol));
                model.DebPeriodHisto = AlbConvert.ConvertIntToDate(Convert.ToInt32(result.DateDebReturnCol));
                model.EtatHisto = result.Etat;
                model.SituationHisto = result.Situation;
            }
            return model;
        }

        #endregion

        #endregion

        #region Attestation

        public static AttestationDto GetInfoAttestation(string codeContrat, string version, string type, string user)
        {
            AttestationDto model = new AttestationDto();

            ModeleControlPeriodeDto result = CommonRepository.ControlePeriode(codeContrat, version, 0, null, null, "ATTEST");

            model.Alertes = AvenantRepository.GetListAlertesAvenant(codeContrat, version, type, user);
            model.TypesContrat = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "TYPOC");
            model.TypesAttes = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "ATTTY");

            return model;
        }

        public static string ChangeExercicePeriode(string codeContrat, string version, string type, int exercice, DateTime? periodeDeb, DateTime? periodeFin)
        {
            var result = CommonRepository.ControlePeriode(codeContrat, version, exercice, periodeDeb, periodeFin, "ATTEST");

            var strReturn = string.Format("{0}_{1}_{2}",
                result.PeriodeDeb, result.PeriodeFin, result.CodeErreur);

            return strReturn;
        }

        public static AttestationSelRsqDto OpenTabRsqObj(string codeContrat, string version, string type, string codeAvn,
            string lotId, string exercice, DateTime? periodeDeb, DateTime? periodeFin, string typeAttes, bool integralite, string user)
        {
            var resultSetInfo = string.IsNullOrEmpty(lotId) ? CommonRepository.SetInformationSel(codeContrat, version, type, "ATTEST", AlbConvert.ConvertDateToInt(periodeDeb), AlbConvert.ConvertDateToInt(periodeFin),
                    exercice, typeAttes, "", user) : "_" + lotId;

            AttestationSelRsqDto toReturn = new AttestationSelRsqDto
            {
                Erreur = resultSetInfo.Split('_')[0],
                LotId = resultSetInfo.Split('_')[1],
            };
            toReturn.Risques = AttestationRepository.GetRisqueAttestation(codeContrat, version, type, toReturn.LotId);


            return toReturn;
        }

        public static AttestationSelGarDto OpenTabGarantie(string codeContrat, string version, string type, string codeAvn,
            string lotId, string exercice, DateTime? periodeDeb, DateTime? periodeFin, string typeAttes, bool integralite, string user)
        {
            var resultSetInfo = string.IsNullOrEmpty(lotId) ? CommonRepository.SetInformationSel(codeContrat, version, type, "ATTEST", AlbConvert.ConvertDateToInt(periodeDeb), AlbConvert.ConvertDateToInt(periodeFin),
                    exercice, typeAttes, "", user) : "_" + lotId;

            AttestationSelGarDto toReturn = new AttestationSelGarDto
            {
                Erreur = resultSetInfo.Split('_')[0],
                LotId = resultSetInfo.Split('_')[1],
            };
            toReturn.Risques = AttestationRepository.GetGarantieAttestation(codeContrat, version, type, toReturn.LotId);


            return toReturn;
        }

        public static AttestationSelGarDto LoadAttestationGarantie(string codeContrat, string version, string type, string lotId)
        {
            AttestationSelGarDto toReturn = new AttestationSelGarDto
            {
                Erreur = string.Empty,//CommonRepository.SelectionElemAttestation(codeContrat, version, type, periodeDeb, periodeFin),
                Risques = AttestationRepository.GetGarantieAttestation(codeContrat, version, type, lotId)
            };

            return toReturn;
        }


        #endregion

        #region Regularisation

        public static RegularisationDto GetInfoRegularisation(string codeContrat, string version, string type, string codeAvn, string user)
        {
            RegularisationDto model = new RegularisationDto();

            model.Regularisations = RegularisationRepository.GetListeRegularisation(codeContrat, version, type);

            var lastActeGestionExist = model.Regularisations.Exists(el => el.CodeAvn.ToString() == codeAvn);
            if (!lastActeGestionExist)
            {
                var lastActeGest = RegularisationRepository.InsertCurrentActeGestionRegule(codeContrat, version, type, codeAvn);
                if (lastActeGest != null)
                {
                    model.Regularisations.Add(lastActeGest);
                }
            }

            model.Alertes = AvenantRepository.GetListAlertesAvenant(codeContrat, version, type, user);
            model.TypesContrat = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "TYPOC");

            return model;
        }

        public static RegularisationInfoDto GetInfoAvnRegule(string codeContrat, string version, string type, string codeAvn, string modeAvt, int exercice, DateTime? periodeDeb, DateTime? periodeFin)
        {
            return null;
        }

        public static RegularisationGarDto GetListGarRegule(string plotId, string preguleId, string pcodeRsq, bool isReadonly)
        {
            long.TryParse(plotId, out var lotId);
            long.TryParse(preguleId, out var reguleId);
            int.TryParse(pcodeRsq, out var codeRsq);

            var risque = new WSAS400.DTO.Offres.Risque.RisqueDto();
            var model = new RegularisationGarDto
            {
                Risque = RegularisationRepository.GetRisqueRegule(lotId, codeRsq),
                Garanties = RegularisationRepository.GetListGarRegule(lotId, reguleId, codeRsq, isReadonly),
                PeriodeRegule = RegularisationRepository.GetRegulPeriode(reguleId)
            };

            // add check over the RCFR group
            foreach (var group in model.Garanties.GroupBy(g => g.CodeFormule))
            {
                var firstGarantie = group.FirstOrDefault(g => g.CodeGarantie == AlbOpConstants.RCFrance);
                RegularisationManager.TryIncludeGarantiesInRCFRGroup(
                    new GarantieFilter { LotId = lotId, RgId = reguleId, RsqNum = codeRsq },
                    group.ToList(),
                    (firstGarantie != null) ? firstGarantie.IdGarantie : 0);
            }

            return model;
        }

        public static RegularisationRsqDto GetListRsqRegul(string lotId, string reguleId, string mode)
        {
            return RegularisationRepository.GetListRsqRegule(lotId, reguleId, mode);
        }

        public static List<LigneRegularisationDto> DeleteRegule(string codeContrat, string version, string type, string codeAvn, string reguleId)
        {
            var model = RegularisationRepository.DeleteRegule(codeContrat, version, type, reguleId);

            var lastActeGestionExist = model.Exists(el => el.CodeAvn.ToString() == codeAvn);
            if (!lastActeGestionExist)
            {
                var lastActeGest = RegularisationRepository.InsertCurrentActeGestionRegule(codeContrat, version, type, codeAvn);
                if (lastActeGest != null)
                {
                    model.Add(lastActeGest);
                }
            }
            return model;
        }

        public static AjoutMouvtGarantieDto AjouterMouvtPeriod(string codeContrat, string version, string type, string codersq, string codfor, string codegar, string idregul, string idlot, int datedeb, int datefin)
        {
            var model = new AjoutMouvtGarantieDto { LignesMouvement = new List<LigneMouvtGarantieDto>() };

            bool toReturn = RegularisationRepository.GetMouvementPeriode(codeContrat, version, type, codersq, codfor, codegar, datedeb, datefin);
            if (toReturn)
            {
                string retourControle = RegularisationRepository.IsCheckChevauchementPeriodRegule(codeContrat, version, type, codersq, codfor, codegar, idregul, datedeb, datefin);
                if (string.IsNullOrEmpty(retourControle))
                {
                    var str = CommonRepository.GetMontantGarantieEmis(codeContrat, version, type, codersq, codfor, codegar, datedeb, datefin);
                    if (str.Split('_')[0] != "ERROR")
                    {
                        var garantieFilter = new GarantieFilter
                        {
                            LotId = Int64.Parse(idlot),
                            RgId = Int64.Parse(idregul),
                            RsqNum = Int32.Parse(codersq),
                            DateDebut = datedeb.YMDToDate().Value,
                            DateFin = datefin.YMDToDate().Value
                        };

                        bool isMultiRC = RegularisationRepository.HasGarantieRCGrouped(garantieFilter);
                        model.LignesMouvement = RegularisationRepository.AjouterMouvtPeriod(codeContrat, version, type, codersq, codfor, codegar, idregul, datedeb, datefin, str.Split('_')[0], str.Split('_')[1], isMultiRC);

                        // add check over the RCFR group
                        RegularisationManager.TryIncludeGarantiesInRCFRGroup(
                            garantieFilter,
                            model.LignesMouvement);
                    }
                    else
                    {
                        model.StrReturn = str.Split('_')[1];
                    }
                }
                else
                {
                    model.StrReturn = retourControle;
                }
            }
            else
            {
                model.StrReturn = "Erreur lors de la création de la période.";
            }
            return model;
        }

        #endregion

        #region Blocage des termes

        public static void SaveZoneStop(string codeContrat, string versionContrat, string typeContrat, string zoneStop, string user)
        {
            var oldZoneStop = AffaireNouvelleRepository.GetZoneStop(codeContrat, versionContrat, typeContrat);
            var libTraitement = string.Empty;
            if (!string.IsNullOrEmpty(oldZoneStop) && string.IsNullOrEmpty(zoneStop))
            {
                libTraitement = "Déblocage en code " + oldZoneStop;
            }
            else if (!string.IsNullOrEmpty(zoneStop))
            {
                libTraitement = "Blocage en code " + zoneStop;
            }
            AffaireNouvelleRepository.SaveZoneStop(codeContrat, versionContrat, typeContrat, zoneStop);
            #region Ajout de l'acte de gestion
            CommonRepository.AjouterActeGestion(codeContrat, versionContrat, typeContrat, 0, AlbConstantesMetiers.ACTEGESTION_VALIDATION, AlbConstantesMetiers.TRAITEMENT_BQDBQ, libTraitement, user);
            #endregion
        }

        #endregion

        #region Common

        public static string GetAs400User(string adUser, IDbConnection connection = null)
        {
            return CommonRepository.GetAs400User(adUser, connection);


            //var sql = string.Format("SELECT UTIUT STRRETURNCOL FROM YUTILIS WHERE UPPER(TRIM(UTPFX))='{0}'", adUser.ToUpper());
            //var as400User = CommonRepository.GetStrValue(sql);
            //var convertDateToInt = ALBINGIA.Framework.Common.Tools.AlbConvert.ConvertDateToInt(DateTime.Today).Value.ToString(CultureInfo.InvariantCulture);

            //var delSqlVerrou = string.Format("DELETE FROM KVERROU WHERE KAVCRD !={0} AND UPPER(TRIM(KAVCRU))='{1}'",
            //    convertDateToInt, as400User);
            //CommonRepository.UpdateDB(delSqlVerrou);
            //return as400User;
        }

        #endregion

        #region Cotisations

        public static CotisationsDto InitCotisations(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, bool isChecked, bool reload, string user, string acteGestion, bool isreadonly)
        {
            //Question FDU : Changement de sous-branche en historique/consultation
            if (!reload && !isreadonly && modeNavig == ModeConsultation.Standard)
            {
                CotisationsRepository.CheckCodeTaxeGarantie(codeOffre, version, type);

                CommonRepository.ChangeSbr(codeOffre, version, type, user);

                CommonRepository.AvantCotisationAS400(codeOffre, version, type, codeAvn, acteGestion, user);
                string result = CommonRepository.UpdateAS400Cotisations(codeOffre, version, type, "INIT", "0", "0", "", modeNavig, codeAvn, user, acteGestion);

                //2015-05-20 : Ajout de l'appel du PGM400 KDP021CL suite au mail de CCI
                //2015-06-26 : remplacement du paramètre "X" par la condition ternaire
                CommonRepository.AlimStatistiques(codeOffre, version, user, acteGestion, codeAvn == "0" && type == AlbConstantesMetiers.TYPE_CONTRAT ? "N" : "X");

                /// Mise à jour des informations de taux de com sur YPRTENT
                CotisationsRepository.SetTauxCom(codeOffre, version, type);
            }


            CotisationsDto toReturn = CotisationsRepository.InitCotisations(codeOffre, version, type, codeAvn, modeNavig, isChecked);

            return toReturn;
        }

        public static void UpdateCotisations(string codeOffre, string version, string type, CotisationsDto cotisationsDto,
            string field, string value, string oldvalue, string user, ModeConsultation modeNavig, string codeAvn, string acteGestion)
        {
            CotisationsRepository.UpdateCotisations(codeOffre, version, type, cotisationsDto);

        //        string result = CommonRepository.UpdateAS400Cotisations(codeOffre, version, type, field, value, oldvalue, "", modeNavig, codeAvn, user, acteGestion);
        //        if (result == "ERREUR")
        //        {
        //            //erreur
        //        }

        //        CommonRepository.AlimStatistiques(codeOffre, version, user, acteGestion, codeAvn == "0" && type == AlbConstantesMetiers.TYPE_CONTRAT ? "N" : "X");

        //        NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
        //        {
        //            CodeOffre = codeOffre.PadLeft(9, ' '),
        //            Version = Convert.ToInt32(version),
        //            Type = type,
        //            EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
        //            NumeroOrdreDansEtape = 64,
        //            NumeroOrdreEtape = 1,
        //            Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
        //            Risque = 0,
        //            Objet = 0,
        //            IdInventaire = 0,
        //            Formule = 0,
        //            Option = 0,
        //            Niveau = string.Empty,
        //            CreationUser = user,
        //            PassageTag = "O",
        //            PassageTagClause = "N"
        //        });
        //    }
        }


        #endregion

        #region Création Affaire Nouvelle

        //public static ContratDto GetContrat(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig)
        //{
        //    //List<OP.WSAS400.DTO.Offres.Risque.RisqueDto> risquesDto = new List<OP.WSAS400.DTO.Offres.Risque.RisqueDto>();
        //    ContratDto contrat = AffaireNouvelleRepository.GetContrat(contratId, version, type, codeAvn, modeNavig);
        //    if (contrat != null)
        //    {
        //        if (contrat.DateEffetAvenantAnnee > 0 && contrat.DateEffetAvenantMois > 0 && contrat.DateEffetAvenantJour > 0)
        //            contrat.DateEffetAvenant = new DateTime(contrat.DateEffetAvenantAnnee, contrat.DateEffetAvenantMois, contrat.DateEffetAvenantJour);
        //        contrat.HeureEffetAvenant = AlbConvert.ConvertIntToTime(contrat.HeureAvn);
        //        var risques = PoliceRepository.ObtenirRisques(modeNavig, contratId, int.Parse(version), type, codeAvn);
        //        if (risques != null && risques.Count == 1) contrat.IsMonoRisque = true;
        //        else contrat.IsMonoRisque = false;
        //        //foreach (var risque in risques)
        //        //{
        //        //    var rsq = risque;// ObjectMapperManager.DefaultInstance.GetMapper<Risque, RisqueDto>().Map(risque);
        //        //    if (rsq != null)
        //        //    {
        //        //        if (risque.IdAdresseRisque > 0)
        //        //        {
        //        //            var adresseRsq = AdresseRepository.ObtenirAdresse(risque.IdAdresseRisque);
        //        //            if (adresseRsq != null)
        //        //                rsq.AdresseRisque = adresseRsq;// ObjectMapperManager.DefaultInstance.GetMapper<Adresse, AdresseDto>().Map(adresseRsq);
        //        //        }
        //        //        risquesDto.Add(rsq);
        //        //    }
        //        //}
        //        contrat.Risques = risques;
        //        contrat.HasOppBenef = contrat.NbOppBenef > 0;
        //        contrat.IsCheckedEcheance = FinOffreRepository.IsCheckedEcheance(contratId, version, codeAvn);
        //    }
        //    return contrat;
        //}

        public static ContratDto GetContrat(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig, bool LoadRisques = true)
        {
            //List<OP.WSAS400.DTO.Offres.Risque.RisqueDto> risquesDto = new List<OP.WSAS400.DTO.Offres.Risque.RisqueDto>();
            ContratDto contrat = AffaireNouvelleRepository.GetContrat(contratId, version, type, codeAvn, modeNavig);
            if (contrat != null)
            {
                if (contrat.DateEffetAvenantAnnee > 0 && contrat.DateEffetAvenantMois > 0 && contrat.DateEffetAvenantJour > 0)
                    contrat.DateEffetAvenant = new DateTime(contrat.DateEffetAvenantAnnee, contrat.DateEffetAvenantMois, contrat.DateEffetAvenantJour);
                contrat.HeureEffetAvenant = AlbConvert.ConvertIntToTime(contrat.HeureAvn);
                if (LoadRisques)
                {
                    var risques = PoliceRepository.ObtenirRisques(modeNavig, contratId, int.Parse(version), type, codeAvn);
                    if (risques != null && risques.Count == 1) contrat.IsMonoRisque = true;
                    else contrat.IsMonoRisque = false;
                    contrat.Risques = risques;
                }
                else
                {
                    contrat.IsMonoRisque = PoliceRepository.NombreRisques(modeNavig, contratId, int.Parse(version), type, codeAvn) == 1;
                }

                contrat.HasOppBenef = contrat.NbOppBenef > 0;
                contrat.IsCheckedEcheance = FinOffreRepository.IsCheckedEcheance(contratId, version, codeAvn);
            }
            return contrat;
        }



        public static string UpdateContrat(ContratInfoBaseDto contrat, string utilisateur, string acteGestion, string user)
        {
            #region Récupération des risques et des objets de risque
            if (!string.IsNullOrEmpty(contrat.CodeContrat) && !string.IsNullOrEmpty(contrat.Type))
            {
                var risques = PoliceRepository.ObtenirRisques(ModeConsultation.Standard, contrat.CodeContrat, int.Parse(contrat.VersionContrat.ToString()), contrat.Type);
                contrat.Risques = risques;
            }
            #endregion

            string libTrace = string.Empty;
            if (contrat.CopyMode)
            {
                libTrace = $"Création via {contrat.CodeContratCopy}-{contrat.VersionCopy}";
            }
            else
            {
                if (contrat.CodeContrat.IsEmptyOrNull())
                {
                    libTrace = "Création contrat";
                }
                else
                {
                    libTrace = contrat.IsModifHorsAvn ? "Modification Hors avenant" : "Modification contrat";
                }
            }

            var result = AffaireNouvelleRepository.UpdateContrat(contrat, utilisateur, acteGestion, user);
            #region Ajout de l'acte de gestion
            if (!result.Contains("MESSAGE : "))
            {
                var traitement = contrat.IsModifHorsAvn ? AlbConstantesMetiers.TRAITEMENT_MODIFHORSAVN :
                    !string.IsNullOrEmpty(contrat.NumAvenant) && !contrat.NumAvenant.Equals("0", StringComparison.InvariantCulture) ? AlbConstantesMetiers.TRAITEMENT_AVNMD :
                    AlbConstantesMetiers.TRAITEMENT_AFFNV;

                CommonRepository.AjouterActeGestion(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type,
                    !string.IsNullOrEmpty(contrat.NumAvenant) ? Convert.ToInt32(contrat.NumAvenant) : 0, AlbConstantesMetiers.ACTEGESTION_GESTION,
                                                    traitement, libTrace, utilisateur);

            }

            #endregion
            return result;
        }

        public static string ChangePreavisResil(string codeContrat, string version, string codeAvn, string dateEffet, string dateFinEffet, string dateAvenant, string periodicite, string echeancePrincipale, string splitCharHtml, string user, string acteGestion)
        {
            var ddEffet = AlbConvert.ConvertStrToDate(dateEffet);
            var dfEffet = AlbConvert.ConvertStrToDate(dateFinEffet);
            var dAvenant = AlbConvert.ConvertStrToDate(dateAvenant);
            DateTime? dEcheance = null;
            if (!string.IsNullOrEmpty(echeancePrincipale))
                dEcheance = AlbConvert.ConvertStrToDate(echeancePrincipale + "/2012");

            return CommonRepository.LoadPreavisResiliation(codeContrat, version, codeAvn, ddEffet, dfEffet, dAvenant, periodicite, dEcheance, splitCharHtml, user, acteGestion);
        }

        #endregion

        #region Engagements

        public static EngagementDto InitEngagement(string codeOffre, string version, string type, string codeAvn, string codePeriode, ModeConsultation modeNavig, bool isReadonly, bool enregistrementEncoursOnly, string user, string acteGestion, string accessMode, string screen)
        {
            EngagementDto toReturn = new EngagementDto();
            string result = string.Empty;
            if (!isReadonly && modeNavig == ModeConsultation.Standard)
            {
                if (codeAvn == "0" && string.IsNullOrEmpty(accessMode)) {
                    result = CommonRepository.LoadAS400Engagement(codeOffre, version, type, modeNavig, codeAvn, user, acteGestion);
                }
                else {
                    //Vérification de la date de début de la période d'engagement, si date déb < date d'avenant on n'appelle pas le PGM
                    var callEngagementAvn = EngagementRepository.CheckDatePeriode(codeOffre, version, type, codePeriode);
                    if (screen.Trim() != "valider" && callEngagementAvn) {
                        result = CommonRepository.LoadAS400EngagementAvn(codeOffre, version, type, codePeriode, codeAvn, user, acteGestion);
                    }
                }
            }
            if (result != "ERREUR")
            {
                toReturn = EngagementRepository.InitEngagement(codeOffre, version, type, codeAvn, codePeriode, modeNavig, enregistrementEncoursOnly);
                BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);
                ParallelHelper.Execute(2,
                    () =>
                    {
                        toReturn.LCIUnites = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNLCI");
                    },
                    () =>
                    {
                        toReturn.LCITypes = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BALCI");
                    }
                   );
            }
            return toReturn;
        }

        public static void UpdateEngagement(string codeOffre, string version, string type, EngagementDto engagement, string field, string user, string acteGestion, string codePeriode)
        {
            EngagementRepository.EngagementUpdate(codeOffre, version, type, engagement, codePeriode);

            if (field == "PART")
            {
                string result = CommonRepository.UpdateAS400Engagement(codeOffre, version, type, user, acteGestion);
                if (result == "ERREUR")
                {
                    //erreur
                }
            }

            #region Arbre de navigation

            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
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
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });

            #endregion
        }

        #endregion

        #region Formule Garantie

        public static string FormulesGarantiesSet(string codeOffre, string version, string type, string codeAvenant, string dateAvenant, string codeFormule, string codeOption, string formGen, string libelle, FormuleGarantieSaveDto formuleGarantie, string codeObjetRisque, string user)
        {
            var erreurDate = string.Empty;
            var brancheCible = CommonRepository.GetBrancheCibleFormule(codeOffre, version, type, codeAvenant, codeFormule, ModeConsultation.Standard);
            var codeCible = (int)brancheCible.Cible.GuidId;
            var branche = brancheCible.Code;
            var paramsVoletBlocs = FormuleRepository.LoadVoletBlocParameters(codeOffre, Convert.ToInt32(version), type, codeCible, branche);
            var paramsGaranties = FormuleRepository.LoadParameters(paramsVoletBlocs.Select(i => i.LibModele.Trim()).Distinct().ToList());

            if (!string.IsNullOrEmpty(codeAvenant) && codeAvenant != "0")
            {
                erreurDate = FormuleRepository.CheckDateModifFormule(codeOffre, version, type, codeObjetRisque, dateAvenant);
                if (!string.IsNullOrEmpty(erreurDate))
                {
                    return erreurDate;
                }
            }

            FormuleRepository.DeleteTraceFormule(codeOffre, version, type, codeFormule, codeOption);

            #region Arbre de navigation

            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = codeOffre.PadLeft(9, ' '),
                Version = Convert.ToInt32(version),
                Type = type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option),
                NumeroOrdreDansEtape = 50,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option),
                Risque = formGen == "0" ? Convert.ToInt32(codeObjetRisque.Split(';')[0]) : 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = Convert.ToInt32(codeFormule),
                Option = Convert.ToInt32(codeOption),
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = "N"
            });
            #endregion

            var kdbid = FormuleRepository.GetLienKpOpt(codeOffre, version, type, codeFormule, codeOption);
            var dateNow = (long)AlbConvert.ConvertDateToInt(DateTime.Now);
            var codeRisque = codeObjetRisque.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[0];
            var codeObjets = codeObjetRisque.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var nbObj = FormuleRepository.GetNbrObj(codeOffre, version, codeRisque);
            var kddid = 0;
            if (nbObj == codeObjets.Count)
            {
                kddid = CommonRepository.GetAS400Id("KDDID");
                FormuleRepository.PopulateKpoptap(codeOffre, version, type, new KpoptapDto
                {
                    KDDID = kddid,
                    KDDKDBID = kdbid,
                    KDDIPB = codeOffre,
                    KDDALX = string.IsNullOrEmpty(version) ? 0 : Convert.ToInt32(version),
                    KDDTYP = type,
                    KDDFOR = string.IsNullOrEmpty(codeFormule) ? 0 : Convert.ToInt32(codeFormule),
                    KDDOPT = string.IsNullOrEmpty(codeOption) ? 0 : Convert.ToInt32(codeOption),
                    KDDRSQ = Convert.ToInt32(codeObjetRisque.Split(';')[0]),
                    KDDOBJ = 0,
                    KDDCRD = dateNow,
                    KDDCRU = user,
                    KDDMAJD = dateNow,
                    KDDMAJU = user,
                    KDDPERI = "RQ",
                    KDDINVEN = 0,
                    KDDINVEP = 0
                });
            }
            else
            {
                codeObjets.ForEach(i =>
                {
                    kddid = CommonRepository.GetAS400Id("KDDID");
                    FormuleRepository.PopulateKpoptap(codeOffre, version, type, new KpoptapDto
                    {
                        KDDID = kddid,
                        KDDKDBID = kdbid,
                        KDDIPB = codeOffre,
                        KDDALX = string.IsNullOrEmpty(version) ? 0 : Convert.ToInt32(version),
                        KDDTYP = type,
                        KDDFOR = string.IsNullOrEmpty(codeFormule) ? 0 : Convert.ToInt32(codeFormule),
                        KDDOPT = string.IsNullOrEmpty(codeOption) ? 0 : Convert.ToInt32(codeOption),
                        KDDRSQ = Convert.ToInt32(codeObjetRisque.Split(';')[0]),
                        KDDOBJ = Convert.ToInt32(i),
                        KDDCRD = dateNow,
                        KDDCRU = user,
                        KDDMAJD = dateNow,
                        KDDMAJU = user,
                        KDDPERI = "OB",
                        KDDINVEN = 0,
                        KDDINVEP = 0
                    });
                });
            }

            KpgartarDto garantieTarDto = null;

            #region Instanciation de l'objet GestionBlocFormuleGarantieSaveDto
            var infoCommonForGarantie = new InfoCommonFormuleGarantieSaveDto()
            {
                CodeOffre = codeOffre,
                Version = version,
                Type = type,
                CodeFormule = codeFormule,
                CodeOption = codeOption,
                User = user,
                DateNow = dateNow,
                IdLienOpt = kdbid
            };
            #endregion

            foreach (var volet in formuleGarantie.Volets)
            {
                if (!VerifyGestionVolets(infoCommonForGarantie, paramsVoletBlocs, volet))
                    continue;

                foreach (var bloc in volet.Blocs)
                {
                    if (!VerifyGestionBlocs(infoCommonForGarantie, paramsVoletBlocs, volet, bloc))
                        continue;

                    #region Instanciation de l'objet GestionModeleFormuleGarantieSaveDto
                    var gestionModeleForGarantie = new InfoModeleFormuleGarantieSaveDto(infoCommonForGarantie)
                    {
                        CodeAvenant = codeAvenant,
                        ParamsGaranties = paramsGaranties,
                        IdBloc = infoCommonForGarantie.Id,
                        GarantieTarif = garantieTarDto
                    };
                    #endregion

                    foreach (var modele in bloc.Modeles)
                        GestionNiveaux(gestionModeleForGarantie, volet, bloc, modele, dateAvenant);
                }
            }

            FormuleRepository.ClearOldGars(codeOffre, version, type, codeFormule, codeOption, formuleGarantie, paramsGaranties);

            FormuleRepository.PopulateKpexp(codeOffre, version, type, codeFormule, codeOption);

            // FormuleRepository.CalculateAndUpdateDatesGar(codeOffre, Convert.ToInt32(version), type, codeAvenant, Convert.ToInt32(codeFormule), Convert.ToInt32(codeOption), null);

            var errors = FormulesGarantiesCheckRelations(codeOffre, version, type, codeFormule, codeOption, formuleGarantie, paramsVoletBlocs, paramsGaranties);

            if (errors.Any())
            {
                return string.Format(@"##;ERRORREL;{0}", errors.FirstOrDefault());
            }

            //2- on update les choix user
            string errorMsg = FormuleRepository.FormulesGarantiesSet(codeOffre, Convert.ToInt32(version), type, codeAvenant, dateAvenant, Convert.ToInt32(codeFormule), Convert.ToInt32(codeOption), Convert.ToInt32(formGen), libelle, formuleGarantie, user);

            return errorMsg;
        }

        #region Formule Garantie Refacto Sauvegarde
        private static bool VerifyGestionVolets(InfoFormuleGarantieSave_Base commonInfoForGarantie, List<GaranModelDto> paramsVoletBlocs, VoletSaveDto volet)
        {
            var modeleGarant = paramsVoletBlocs.FirstOrDefault(i => i.Guidv == volet.GuidId);
            if (modeleGarant == null)
                return false;

            bool update = false;
            long? guidM = FormuleRepository.getGuidMVoletbyGuidId(volet.GuidId, commonInfoForGarantie.CodeOffre, commonInfoForGarantie.Version, commonInfoForGarantie.Type, commonInfoForGarantie.CodeFormule, commonInfoForGarantie.CodeOption);

            if (volet.MAJ || (volet.isChecked && (modeleGarant.CaracVolet == "O" || modeleGarant.CaracVolet == "P")) || (modeleGarant.GuidM != guidM && guidM != 0))
                update = true;

            return PopulateKPOPTD(commonInfoForGarantie, modeleGarant, volet.isChecked, modeleGarant.VoletOrdre, "V", update);
        }

        private static bool VerifyGestionBlocs(InfoFormuleGarantieSave_Base commonInfoForGarantie, List<GaranModelDto> paramsVoletBlocs, VoletSaveDto volet, BlocSaveDto bloc)
        {
            var modeleGarant = paramsVoletBlocs.FirstOrDefault(i => i.Guidb == bloc.GuidId);
            if (modeleGarant == null)
                return false;

            bool update = false;
            long? guidM = FormuleRepository.getGuidMBlocbyGuidId(bloc.GuidId, commonInfoForGarantie.CodeOffre, commonInfoForGarantie.Version, commonInfoForGarantie.Type, commonInfoForGarantie.CodeFormule, commonInfoForGarantie.CodeOption);

            if (bloc.MAJ || (volet.isChecked && bloc.isChecked && (modeleGarant.CaracBloc == "O" || modeleGarant.CaracBloc == "P")) || (modeleGarant.GuidM != guidM && guidM != 0))
                update = true;

            return PopulateKPOPTD(commonInfoForGarantie, modeleGarant, bloc.isChecked, modeleGarant.BlocOrdre, "B", update);
        }

        private static bool PopulateKPOPTD(InfoFormuleGarantieSave_Base commonInfoForGarantie, GaranModelDto modeleGarant, bool isChecked, float ordre, string kdcteng, bool update)
        {
            var kpoptd = new KpoptdDto
            {
                KDCIPB = commonInfoForGarantie.CodeOffre,
                KDCALX = Convert.ToInt32(commonInfoForGarantie.Version),
                KDCTYP = commonInfoForGarantie.Type,
                KDCFOR = Convert.ToInt32(commonInfoForGarantie.CodeFormule),
                KDCOPT = Convert.ToInt32(commonInfoForGarantie.CodeOption),
                KDCFLAG = Convert.ToInt32(isChecked),
                KDCKAEID = (kdcteng.Equals("B")) ? modeleGarant.GuidBloc : 0,
                KDCKAKID = modeleGarant.GuidVolet,
                KDCKAQID = (kdcteng.Equals("B")) ? modeleGarant.Guidb : 0,
                KDCKARID = modeleGarant.GuidM,
                KDCKDBID = commonInfoForGarantie.IdLienOpt,
                KDCMAJD = commonInfoForGarantie.DateNow,
                KDCMAJU = commonInfoForGarantie.User,
                KDCMODELE = modeleGarant.LibModele,
                KDCTENG = kdcteng,
                KDCCRD = commonInfoForGarantie.DateNow,
                KDCCRU = commonInfoForGarantie.User,
                KDCORDRE = ordre
            };

            commonInfoForGarantie.Id = FormuleRepository.PopulateKpoptd(commonInfoForGarantie.CodeOffre, Convert.ToInt32(commonInfoForGarantie.Version), commonInfoForGarantie.Type, kpoptd, update);

            return commonInfoForGarantie.Id != 0;
        }

        private static void GestionNiveaux(InfoModeleFormuleGarantieSaveDto gestionModeleForGarantie, VoletSaveDto volet, BlocSaveDto bloc, ModeleSaveDto modele, String dateAvenant)
        {
            foreach (var niv1 in modele.Modeles)
                GestionSousNiveaux(gestionModeleForGarantie, volet, bloc, niv1, new List<KpgaranDto>(), dateAvenant);
        }

        private static void GestionSousNiveaux(InfoModeleFormuleGarantieSaveDto gestionModeleForGarantie, VoletSaveDto volet, BlocSaveDto bloc, ModeleNiveauSave_Base niveau, List<KpgaranDto> garantieNiveaux, String dateAvenant)
        {
            var gar = gestionModeleForGarantie.ParamsGaranties.FirstOrDefault(i => i.C2SEQNIV.ToString(CultureInfo.InvariantCulture).Trim() == niveau.GuidGarantie);

            if (gar == null)
                return;

            var garantieNiveauActuel = Populate_Tables_Garantie(gestionModeleForGarantie, volet, bloc, gar, niveau, garantieNiveaux, dateAvenant);

            if (niveau != null && niveau.Modeles?.Any() == true)
            {
                List<KpgaranDto> newGarantieNiveaux = new List<KpgaranDto>();
                newGarantieNiveaux.AddRange(garantieNiveaux);
                newGarantieNiveaux.Add(garantieNiveauActuel);

                foreach (var sousNiveau in niveau.Modeles)
                    GestionSousNiveaux(gestionModeleForGarantie, volet, bloc, sousNiveau, newGarantieNiveaux, dateAvenant);
            }
        }

        private static KpgaranDto Populate_Tables_Garantie(InfoModeleFormuleGarantieSaveDto gestionModeleForGarantie, VoletSaveDto volet, BlocSaveDto bloc, GarantiesDto gar, ModeleNiveauSave_Base niveau, List<KpgaranDto> garantieNiveaux, String dateAvenant)
        {
            var nivExists = FormuleRepository.GetCodeGarantieBySeq(niveau.GuidGarantie, gestionModeleForGarantie.CodeOffre, gestionModeleForGarantie.Version, gestionModeleForGarantie.Type, gestionModeleForGarantie.CodeFormule, gestionModeleForGarantie.CodeOption, ModeConsultation.Standard) != 0;

            if ((niveau.Caractere == "O") || (!nivExists && niveau.Caractere == "P" && !niveau.MAJ))
                niveau.NatureParam = niveau.Nature;

            #region Populate
            KpgaranDto garantieNiveau = Populate_NiveauGarantie(gestionModeleForGarantie, niveau, gar);
            gestionModeleForGarantie.GarantieTarif = Populate_TarifGarantie(gestionModeleForGarantie, gar);
            #endregion

            bool allPrecedentsNiveauxChecked = IsAllNiveauxChecked(garantieNiveaux);
            if (niveau.MAJ || (volet.isChecked && bloc.isChecked && allPrecedentsNiveauxChecked && garantieNiveau.IsChecked))
            {
                gestionModeleForGarantie.IdGar = FormuleRepository.PopulateKpgaran(gestionModeleForGarantie.CodeOffre, Convert.ToInt32(gestionModeleForGarantie.Version), gestionModeleForGarantie.Type, garantieNiveau);

                var sqlDateFinAvt = string.Format(@"SELECT PBFEA * 10000 + PBFEM * 100 + PBFEJ DATEFINRETURNCOL FROM YPOBASE WHERE PBIPB= '{0}' AND PBAVN= {1}", gestionModeleForGarantie.CodeOffre.PadLeft(9, ' '), gestionModeleForGarantie.CodeAvenant);
                var resDateFinAvt = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlDateFinAvt).FirstOrDefault();

                var sqlDateGar = string.Format(@"select kdedatdeb DATEDEBRETURNCOL, KDEWDDEB DATEDEBEFFRETURNCOL, KDEDATFIN DATEFINRETURNCOL, KDEWDFIN DATEFINEFFRETURNCOL, KDECRAVN INT32RETURNCOL FROM KPGARAN WHERE KDEID = {0}", gestionModeleForGarantie.IdGar);
                var resDateGar = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlDateGar);
                foreach (DtoCommon garDate in resDateGar)
                {
                    if (garDate != null && (garDate.DateDebEffReturnCol == 0 || garDate.DateFinEffReturnCol == 0 || (resDateFinAvt != null && (garDate.Int32ReturnCol == Convert.ToInt32(gestionModeleForGarantie.CodeAvenant) && garDate.DateFinEffReturnCol != ((DtoCommon)resDateFinAvt).DateFinEffReturnCol))))
                    {
                        DateTime? dateAvt = dateAvenant.Length >= 8 ? AlbConvert.ConvertIntToDate(Convert.ToInt32(dateAvenant.Substring(0, 4)) * 10000 + Convert.ToInt32(dateAvenant.Substring(4, 2)) * 100 + Convert.ToInt32(dateAvenant.Substring(6, 2))) : null;
                        FormuleRepository.CalculateAndUpdateDatesGar(gestionModeleForGarantie.CodeOffre, Convert.ToInt32(gestionModeleForGarantie.Version), gestionModeleForGarantie.Type, gestionModeleForGarantie.CodeAvenant, Convert.ToInt32(gestionModeleForGarantie.CodeFormule), Convert.ToInt32(gestionModeleForGarantie.CodeOption), dateAvt, Convert.ToInt32(gestionModeleForGarantie.IdGar));
                    }
                }

                FormuleRepository.PopulateKpgartar(gestionModeleForGarantie.CodeOffre, Convert.ToInt32(gestionModeleForGarantie.Version), gestionModeleForGarantie.Type, gestionModeleForGarantie.IdGar, gestionModeleForGarantie.GarantieTarif);
            }

            return garantieNiveau;
        }

        private static bool IsAllNiveauxChecked(List<KpgaranDto> garantieNiveaux)
        {
            #region Verification de la selection de tous les niveaux
            bool allPrecedentsNiveauxChecked = true;
            foreach (var it in garantieNiveaux)
                if (!it.IsChecked)
                {
                    allPrecedentsNiveauxChecked = false;
                    break;
                }
            #endregion
            return allPrecedentsNiveauxChecked;
        }

        private static KpgartarDto Populate_TarifGarantie(InfoModeleFormuleGarantieSaveDto gestionModeleForGarantie, GarantiesDto gar)
        {
            return new KpgartarDto
            {
                KDGIPB = gestionModeleForGarantie.CodeOffre,
                KDGALX = Convert.ToInt32(gestionModeleForGarantie.Version),
                KDGFOR = Convert.ToInt32(gestionModeleForGarantie.CodeFormule),
                KDGOPT = Convert.ToInt32(gestionModeleForGarantie.CodeOption),
                KDGCHT = 0,
                KDGCMC = 0,
                KDGCTT = 0,
                KDGTFF = 0,
                KDGTMC = 0,
                KDGTYP = gestionModeleForGarantie.Type,
                KDGFMABASE = gar.FRHBASMAXNIV,
                KDGFMAUNIT = gar.FRHBASMAXNIV,
                KDGFMAVALA = gar.FRHVALMAXNIV,
                KDGFMAVALO = gar.FRHVALMAXNIV,
                KDGFMAVALW = 0,
                KDGFMIBASE = gar.FRHBASMINNIV,
                KDGFMIUNIT = gar.FRHUNTMINNIV,
                KDGFMIVALA = gar.FRHVALMINNIV,
                KDGFMIVALO = gar.FRHVALMINNIV,
                KDGFMIVALW = 0,
                KDGFRHBASE = gar.FRHBASNIV,
                KDGFRHMOD = gar.FRHMODNIV,
                KDGFRHOBL = gar.FRHOBLNIV,
                KDGFRHUNIT = gar.FRHUNTNIV,
                KDGFRHVALA = gar.FRHVALNIV,
                KDGFRHVALO = gar.FRHVALNIV,
                KDGFRHVALW = 0,
                KDGGARAN = gar.C2GARNIV,
                KDGKDEID = gestionModeleForGarantie.IdGar,
                KDGLCIBASE = gar.LCIBASNIV,
                KDGLCIMOD = gar.LCIMODNIV,
                KDGLCIOBL = gar.LCIOBLNIV,
                KDGLCIUNIT = gar.LCIUNTNIV,
                KDGLCIVALA = gar.LCIVALNIV,
                KDGLCIVALO = gar.LCIVALNIV,
                KDGLCIVALW = 0,
                KDGMNTBASE = 0,
                KDGNUMTAR = 1,
                KDGPRIBASE = gar.PRIBASNIV,
                KDGPRIMOD = gar.PRIMODNIV,
                KDGPRIMPRO = 0,
                KDGPRIOBL = gar.PRIOBLNIV,
                KDGPRIUNIT = gar.PRIUNTNIV,
                KDGPRIVALA = gar.PRIVALNIV,
                KDGPRIVALO = gar.PRIVALNIV,
                KDGPRIVALW = 0
            };
        }

        private static KpgaranDto Populate_NiveauGarantie(InfoModeleFormuleGarantieSaveDto gestionModeleForGarantie, ModeleNiveauSave_Base niveau, GarantiesDto gar)
        {
            return new KpgaranDto
            {
                KDETYP = gestionModeleForGarantie.Type,
                KDEIPB = gestionModeleForGarantie.CodeOffre,
                KDEALX = Convert.ToInt32(gestionModeleForGarantie.Version),
                KDEFOR = Convert.ToInt32(gestionModeleForGarantie.CodeFormule),
                KDEOPT = Convert.ToInt32(gestionModeleForGarantie.CodeOption),
                KDEKDCID = gestionModeleForGarantie.IdBloc,
                KDEGARAN = gar.GAGARNIV,
                KDESEQ = gar.C2SEQNIV,
                KDENIVEAU = gar.C2NIVNIV,
                KDESEM = gar.C2SEMNIV,
                KDESE1 = gar.C2SE1NIV,
                KDETRI = gar.C2TRINIV,
                KDENUMPRES = 1,
                KDEAJOUT = "N",
                KDECAR = gar.C2CARNIV,
                KDENAT = gar.C2NATNIV,
                KDEGAN = niveau.NatureParam,
                KDEKDFID = 0,
                KDEDEFG = gar.GADFGNIV,
                KDEKDHID = 0,
                KDEDUREE = 0,
                KDEDURUNI = "",
                KDEPRP = "A",
                KDETYPEMI = "P",
                KDEALIREF = gar.C2MRFNIV,
                KDECATNAT = gar.C2CNANIV,
                KDEINA = gar.C2INANIV,
                KDETAXCOD = gar.C2TAXNIV,
                KDETAXREP = 0,
                KDECRAVN = string.IsNullOrEmpty(gestionModeleForGarantie.CodeAvenant) ? 0 : Convert.ToInt32(gestionModeleForGarantie.CodeAvenant),
                KDECRU = gestionModeleForGarantie.User,
                KDECRD = gestionModeleForGarantie.DateNow,
                KDEMAJAVN = string.IsNullOrEmpty(gestionModeleForGarantie.CodeAvenant) ? 0 : Convert.ToInt32(gestionModeleForGarantie.CodeAvenant),
                KDEASVALO = gar.C4VALNIV,
                KDEASVALA = gar.C4VALNIV,
                KDEASVALW = 0,
                KDEASUNIT = gar.C4UNTNIV,
                KDEASBASE = gar.C4BASNIV,
                KDEASMOD = gar.C4MAJNIV,
                KDEASOBLI = gar.C4OBLNIV,
                KDEINVSP = gar.GAINVNIV,
                KDEINVEN = 0,
                KDETCD = gar.C2TCDNIV,
                KDEMODI = "N",
                KDEPIND = gar.C2INANIV,
                KDEPCATN = gar.C2CNANIV,
                KDEPREF = gar.C2MRFNIV,
                KDEPPRP = "A",
                KDEPEMI = "P",
                KDEPTAXC = gar.C2TAXNIV,
                KDEPNTM = gar.C2NTMNIV,
                KDEALA = gar.C4ALANIV,
                KDEPALA = gar.C4ALANIV,
                KDEALO = string.Empty,
                Nature = niveau.Nature
            };
        }
        #endregion

        public static List<string> FormulesGarantiesCheckRelations(string codeOffre, string version, string type, string codeFormule, string codeOption,
                                        FormuleGarantieSaveDto formuleGarantie, List<GaranModelDto> paramsVoletBlocs, List<GarantiesDto> paramsGaranties)
        {
            var messages = new List<string>();

            var blocs = new List<long>();
            var garanties = new List<long>();

            foreach (var volet in formuleGarantie.Volets)
            {
                foreach (var bloc in volet.Blocs)
                {
                    if (!IsBlocAdded(paramsVoletBlocs, blocs, volet, bloc))
                        continue;
                    foreach (var modele in bloc.Modeles)
                    {
                        foreach (var niv1 in modele.Modeles)
                        {
                            GestionSousNiveauxCheckRelations(paramsGaranties, volet, bloc, niv1, new List<KpgaranDto>(), garanties);
                        }
                    }
                }
            }

            var datesGaranties = FormuleRepository.GetDatesByGaranties(codeOffre, version, type, codeFormule, codeOption, garanties.Select(i => (int)i).ToArray());
            var garantiesBlocsRelations = FormuleRepository.GetGarBlocsRelations(garanties, blocs);

            const string msgBlocIncompatibles = @"Les blocs <br/><br/>{0}-{1}<br/>et<br/>{2}-{3}<br/><br/> ne sont pas compatibles.<br/>Vous devez en retirer un des deux pour pouvoir continuer.";
            const string msgBlocAssociees = @"Les blocs <br/><br/>{0}-{1}<br/>et<br/>{2}-{3}<br/><br/> sont associés.<br/>Si vous en sélectionner un, vous devez sélectionner l'autre pour pouvoir continuer.";
            const string msgGarantiesAssociees = @"Les garanties <br/><br/>{0}-{1}<br/>et<br/>{2}-{3}<br/><br/>sont associées.<br/>Vous devez sélectionner l'autre pour pouvoir continuer.";
            const string msgGarantiesIncompatibles = @"Les garanties <br/><br/>{0}-{1}<br/>et<br/>{2}-{3},<br/><br/>doivent avoir des plages de dates qui ne se croisent pas";
            const string msgGarantiesAssocieesDates = @"Les garanties <br/><br/>{0}-{1}<br/>et<br/>{2}-{3},<br/><br/>doivent avoir des plages de dates Identiques";
            //const string msgGarantiesAlternatives = @"Les garanties <br/><br/>{0}-{1}<br/>et<br/>{2}-{3}<br/><br/>sont alternatives.<br/>Vous devez en cocher une des deux (mais pas les deux) pour pouvoir continuer.";
            const string msgGarantiesDependantes = @"La garantie <br/><br/>{0}-{1}<br/>est dépendante de la garantie <br/>{2}-{3}<br/><br/>.Vous devez cocher la garantie {2} pour pouvoir cocher la garantie {0}.";
            const string msgGarantiesDependantesDates = @"La garantie <br/><br/>{0}-{1}<br/>est dépendante de la garantie <br/>{2}-{3}<br/><br/>.La garantie {2} doit être comprise dans la plage de date de la garantie {0}.";

            //Blocs
            var relBlocs = garantiesBlocsRelations.Where(rel => rel.Type == "B");
            GaranModelDto blocP1;
            GaranModelDto blocP2;

            foreach (var rel in relBlocs)
            {
                blocP1 = paramsVoletBlocs.FirstOrDefault(i => i.Guidb == rel.Id1);
                blocP2 = paramsVoletBlocs.FirstOrDefault(i => i.Guidb == rel.Id2);
                if (blocP1 == null || blocP2 == null) continue;

                //Bloc - Incompatibles
                if (rel.Relation == "I" && blocs.Any(i => i == rel.Id1) && blocs.Any(i => i == rel.Id2))
                {
                    messages.Add(string.Format(msgBlocIncompatibles, blocP1.CodeBloc, blocP1.DescrBloc, blocP2.CodeBloc, blocP2.DescrBloc));
                    break;
                }

                //Bloc - Associées
                if (rel.Relation == "A" && ((blocs.Any(i => i == rel.Id1) && blocs.All(i => i != rel.Id2))))
                {
                    messages.Add(string.Format(msgBlocAssociees, blocP1.CodeBloc, blocP1.DescrBloc, blocP2.CodeBloc, blocP2.DescrBloc));
                    break;
                }
            }

            if (messages.Any()) return messages;

            //Garanties
            var relGaranties = garantiesBlocsRelations.Where(rel => rel.Type == "G");

            foreach (var rel in relGaranties)
            {
                GarantiesDto garantieP1 = paramsGaranties.FirstOrDefault(i => i.C2SEQNIV == rel.Id1);
                GarantiesDto garantieP2 = paramsGaranties.FirstOrDefault(i => i.C2SEQNIV == rel.Id2);
                GarantiePeriodeDto periode1 = datesGaranties.FirstOrDefault(i => i.IdGarantie == rel.Id1);
                GarantiePeriodeDto periode2 = datesGaranties.FirstOrDefault(i => i.IdGarantie == rel.Id2);

                if (garantieP1 == null || garantieP2 == null) continue;

                //Garanties - Incompatibles
                if (rel.Relation == "I" && ((garanties.Any(i => i == rel.Id1) && garanties.Any(i => i == rel.Id2))))
                {
                    if (AreDateRangesOverlaps(periode1, periode2))
                    {
                        messages.Add(string.Format(msgGarantiesIncompatibles, garantieP1.GAGARNIV, garantieP1.GADESNIV, garantieP2.GAGARNIV, garantieP2.GADESNIV));
                        break;
                    }

                }

                //Garanties - Associéés
                if (rel.Relation == "A" && ((garanties.Any(i => i == rel.Id1) && garanties.All(i => i != rel.Id2)) || (garanties.Any(i => i == rel.Id2) && garanties.All(i => i != rel.Id1))))
                {
                    messages.Add(string.Format(msgGarantiesAssociees, garantieP1.GAGARNIV, garantieP1.GADESNIV, garantieP2.GAGARNIV, garantieP2.GADESNIV));
                    break;
                }

                //Garanties - Associés - dates
                if (rel.Relation == "A" && (garanties.Any(i => i == rel.Id1) && garanties.Any(i => i == rel.Id2)))
                {
                    if (!AreDateRangesEquals(periode1, periode2))
                    {
                        messages.Add(string.Format(msgGarantiesAssocieesDates, garantieP1.GAGARNIV, garantieP1.GADESNIV, garantieP2.GAGARNIV, garantieP2.GADESNIV));
                        break;
                    }
                }

                //Garanties - Dépendantes
                if (rel.Relation == "D" && (garanties.Any(i => i == rel.Id1) && garanties.All(i => i != rel.Id2)))
                {
                    messages.Add(string.Format(msgGarantiesDependantes, garantieP1.GAGARNIV, garantieP1.GADESNIV, garantieP2.GAGARNIV, garantieP2.GADESNIV));
                    break;
                }

                //Garanties - Dépendantes - Dates
                if (rel.Relation == "D" && (garanties.Any(i => i == rel.Id1) && garanties.Any(i => i == rel.Id2)))
                {
                    if (!IsAInDateRangeB(periode1, periode2))
                    {
                        messages.Add(string.Format(msgGarantiesDependantesDates, garantieP1.GAGARNIV, garantieP1.GADESNIV, garantieP2.GAGARNIV, garantieP2.GADESNIV));
                        break;
                    }
                }



            }
            return messages;
        }




        #region Formule Garantie Refacto Sauvegarde : CheckRelations


        private static bool IsBlocAdded(List<GaranModelDto> paramsVoletBlocs, List<long> blocs, VoletSaveDto volet, BlocSaveDto bloc)
        {
            var b = paramsVoletBlocs.FirstOrDefault(i => i.Guidb == bloc.GuidId);
            if (b == null)
                return false;

            if (volet.isChecked && bloc.isChecked)
                blocs.Add(bloc.GuidId);

            return true;
        }

        private static void GestionSousNiveauxCheckRelations(List<GarantiesDto> paramsGaranties, VoletSaveDto volet, BlocSaveDto bloc, ModeleNiveauSave_Base niveau, List<KpgaranDto> garantieNiveaux, List<long> garanties)
        {
            var gar = paramsGaranties.FirstOrDefault(i => i.C2SEQNIV.ToString(CultureInfo.InvariantCulture).Trim() == niveau.GuidGarantie);

            if (gar == null)
                return;

            var garantieNiveauActuel = new KpgaranDto { KDECAR = gar.C2CARNIV, Nature = niveau.Nature, KDEGAN = niveau.NatureParam };


            bool allPrecedentsNiveauxChecked = IsAllNiveauxChecked(garantieNiveaux);

            if (volet.isChecked && bloc.isChecked && allPrecedentsNiveauxChecked && garantieNiveauActuel.IsChecked)
                garanties.Add(Convert.ToInt64(niveau.GuidGarantie));

            if (niveau.Modeles != null && niveau.Modeles.Any())
            {
                List<KpgaranDto> newGarantieNiveaux = new List<KpgaranDto>();
                newGarantieNiveaux.AddRange(garantieNiveaux);
                newGarantieNiveaux.Add(garantieNiveauActuel);
                foreach (var sousNiveau in niveau.Modeles)
                    GestionSousNiveauxCheckRelations(paramsGaranties, volet, bloc, sousNiveau, newGarantieNiveaux, garanties);
            }

        }

        #endregion

        private static bool AreDateRangesEquals(GarantiePeriodeDto itemA, GarantiePeriodeDto itemB)
        {
            itemA.DateDebut = itemA.DateDebut ?? DateTime.MinValue;
            itemA.DateFin = itemA.DateFin ?? DateTime.MaxValue;

            itemB.DateDebut = itemB.DateDebut ?? DateTime.MinValue;
            itemB.DateFin = itemB.DateFin ?? DateTime.MaxValue;

            return itemA.DateDebut == itemB.DateDebut && itemA.DateFin == itemB.DateFin;
        }

        private static bool AreDateRangesOverlaps(GarantiePeriodeDto itemA, GarantiePeriodeDto itemB)
        {
            itemA.DateDebut = itemA.DateDebut ?? DateTime.MinValue;
            itemA.DateFin = itemA.DateFin ?? DateTime.MaxValue;

            itemB.DateDebut = itemB.DateDebut ?? DateTime.MinValue;
            itemB.DateFin = itemB.DateFin ?? DateTime.MaxValue;

            if (itemA.DateDebut == itemB.DateDebut) return true;


            GarantiePeriodeDto minRange;
            GarantiePeriodeDto maxRange;

            if (itemA.DateDebut < itemB.DateDebut)
            {
                minRange = itemA;
                maxRange = itemB;
            }
            else
            {
                minRange = itemB;
                maxRange = itemA;
            }

            return maxRange.DateDebut <= minRange.DateFin;
        }

        public static bool IsAInDateRangeB(GarantiePeriodeDto itemA, GarantiePeriodeDto itemB)
        {
            itemA.DateDebut = itemA.DateDebut ?? DateTime.MinValue;
            itemA.DateFin = itemA.DateFin ?? DateTime.MaxValue;

            itemB.DateDebut = itemB.DateDebut ?? DateTime.MinValue;
            itemB.DateFin = itemB.DateFin ?? DateTime.MaxValue;


            return itemA.DateDebut >= itemB.DateDebut && itemA.DateFin <= itemB.DateFin;
        }

        public static FormuleGarantieDetailsDto ObtenirGarantieDetails(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, string codeObjetRisque, FormuleGarantieSaveDto volets, DateTime? dateEffAvnModifLocale, string user, bool isReadonly, ModeConsultation modeNavig)
        {

            BrancheDto branche = CommonRepository.GetBrancheCibleFormule(codeOffre, version, type, codeAvn, codeFormule, modeNavig);
            FormuleGarantieDetailsDto toReturn = new FormuleGarantieDetailsDto();

            toReturn = ObtenirGarantieDetailsInfo(codeOffre, version, type, codeAvn, codeFormule, (int)branche.Cible.GuidId, branche.Code, codeOption, Convert.ToInt64(codeGarantie), codeObjetRisque, volets, dateEffAvnModifLocale, user, isReadonly, modeNavig);
            toReturn.DureeUnites = ReferenceRepository.RechercherParametres(branche.Code, branche.Cible.Code, "PRODU", "QBVGU");
            toReturn.Natures = ReferenceRepository.RechercherParametres(branche.Code, branche.Cible.Code, "PRODU", "CBNAT");
            toReturn.Applications = ReferenceRepository.RechercherParametres(branche.Code, branche.Cible.Code, "PRODU", "JHPRP");
            toReturn.TypesEmission = ReferenceRepository.RechercherParametres(branche.Code, branche.Cible.Code, "PRODU", "JHPRE");
            toReturn.CodesTaxe = ReferenceRepository.RechercherParametres(branche.Code, branche.Cible.Code, "GENER", "TAXEC");
            toReturn.AlimAssiettes = ReferenceRepository.RechercherParametres(branche.Code, branche.Cible.Code, "KHEOP", "C4ALA");


            return toReturn;
        }

        #endregion

        #region Quittances

        public static List<QuittanceDto> GetQuittances(string codeOffre, string version, string type, string codeAvn, string modeAffichage, string numQuittanceVisu, bool launchPGM, bool isModeCalculForce, ModeConsultation modeNavig, string user, string acteGestion, string idRegule, bool isreadonly, string isFGACocheIHM)
        {
            if (type == AlbConstantesMetiers.TYPE_CONTRAT && (acteGestion == "AFFNOUV" || acteGestion == AlbConstantesMetiers.TRAITEMENT_AVNMD || acteGestion == AlbConstantesMetiers.TRAITEMENT_AVNRG))
            {
                CommonRepository.ChangeSbr(codeOffre, version, type, user);
            }

            if (modeNavig == ModeConsultation.Standard)
            {
                if (!isreadonly) {
                    CotisationsRepository.CheckCodeTaxeGarantie(codeOffre, version, type);
                }
                if (launchPGM && !isreadonly && !isModeCalculForce)
                {
                    if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
                    {
                        FinOffreRepository.DeleteTraceAvt(codeOffre, version, type, acteGestion);
                        CommonRepository.LoadTableRegule(codeOffre, version, type, idRegule);
                        _ = long.TryParse(idRegule, out long rgId);
                        RegularisationRepository.PerformDividingAmount_KPGRGU_KPRGUR(rgId);
                    }
                    else if (!string.IsNullOrEmpty(codeAvn) && codeAvn != "0")
                    {
                        FinOffreRepository.DeleteTraceAvt(codeOffre, version, type, acteGestion);
                        CommonRepository.CalculAvenant(codeOffre, version, codeAvn, user, acteGestion, isFGACocheIHM);
                    }
                    else
                    {
                        CommonRepository.AvantCotisationAS400(codeOffre, version, type, codeAvn, acteGestion, user);
                        CommonRepository.CalculAffaireNouvelle(codeOffre, version, codeAvn, user, acteGestion, isFGACocheIHM);
                    }

                }
                if (acteGestion != AlbConstantesMetiers.TYPE_AVENANT_REGUL && acteGestion != AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
                {
                    if (launchPGM && !isreadonly && isModeCalculForce)
                    {
                        CommonRepository.CalculFinCalculForce(codeOffre, version, codeAvn, user, acteGestion);
                    }

                    if (!isreadonly)
                    {
                        CommonRepository.AlimStatistiques(codeOffre, version, user, acteGestion, codeAvn != "0" ? "X" : "N");
                    }
                }
                else
                {
                    if (!isreadonly) {
                        CommonRepository.CalculForceRegule(codeOffre, version, idRegule);
                    }
                }
            }
            var toReturn = FinOffreRepository.GetQuittances(codeOffre, version, type, codeAvn, modeAffichage, numQuittanceVisu, modeNavig, user, acteGestion, idRegule);

            return toReturn;
        }

        public static QuittanceVisualisationDto GetListeQuittancesAnnulation(bool init, string codeOffre, string version, int avenant, DateTime dateEffetAvenant, DateTime? dateEmission, string typeOperation, string situation, DateTime? datePeriodeDebut, DateTime? datePeriodeFin, AlbConstantesMetiers.TypeQuittances typeQuittances, string user, string acteGestion, bool isreadonly, ModeConsultation modeNavig, string colTri)
        //public static List<QuittanceVisualisationLigneDto> GetListeQuittancesAnnulation(bool init, string codeOffre, string version, int avenant, DateTime dateEffetAvenant, DateTime? dateEmission, string typeOperation, string situation, DateTime? datePeriodeDebut, DateTime? datePeriodeFin, AlbConstantesMetiers.TypeQuittances typeQuittances, string user, string acteGestion, bool isreadonly, ModeConsultation modeNavig)
        {
            QuittanceVisualisationDto model = new QuittanceVisualisationDto();
            if ((init || (datePeriodeDebut == null && datePeriodeFin == null)) && !isreadonly && modeNavig == ModeConsultation.Standard)//récupération des périodes par appel d'un P400
            {
                CommonRepository.GetPeriodeAvenantAS400(codeOffre, version, avenant, dateEffetAvenant, out datePeriodeDebut, out datePeriodeFin, user, acteGestion);
            }
            if (datePeriodeDebut != null || datePeriodeFin != null)
            {
                model.PeriodeDebut = datePeriodeDebut;
                model.ListQuittances = FinOffreRepository.GetListeVisualisationQuittances(codeOffre, version, dateEmission, typeOperation, situation, datePeriodeDebut, datePeriodeFin, typeQuittances, colTri, avenant.ToString());
            }
            return model;
        }

        public static void CalculAvenant(string codeContrat, string version, string codeAvn, string user, string acteGestion, string isFGACocheIHM, decimal fraisAccessoire = 0, bool updateaccavn = false)
        {
            CommonRepository.CalculAvenant(codeContrat, version, codeAvn, user, acteGestion, isFGACocheIHM, fraisAccessoire, updateaccavn);
        }

        #endregion

        #region Traite

        public static TraiteDto InitTraite(string codeOffre, string version, string type, string codeAvn, string traite, ModeConsultation modeNavig, bool isReadonly, string user, string acteGestion, string codePeriode, string accesMode)
        {
            TraiteDto toReturn = EngagementRepository.InitTraite(codeOffre, version, type, codeAvn, traite, modeNavig, isReadonly, user, acteGestion, codePeriode, accesMode);

            BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);

            ParallelHelper.Execute(2,
            () =>
            {
                toReturn.LCIUnites = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNLCI");
            },
            () =>
            {
                toReturn.LCITypes = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BALCI");
            });

            return toReturn;
        }

        #endregion

        #region Validation

        public static ValidationDto InitValidationOffre(string codeOffre, string version, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion, string reguleId, string reguleMode)
        {
            ValidationDto toReturn = new ValidationDto();
            string result = string.Empty;
            if (!isReadonly && modeNavig == ModeConsultation.Standard)
                result = CommonRepository.LoadAS400ValidationOffre(codeOffre, version, type, modeNavig, codeAvn, user, acteGestion);
            if (result == "ERREUR")
            {
                //erreur
            }
            else
            {
                BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);

                toReturn = FinOffreRepository.InitValidationOffre(codeOffre, version, type, codeAvn, isReadonly, modeNavig, acteGestion, reguleId, reguleMode);
                toReturn.Motifs = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "PRODU", "PBSTF");
            }
            return toReturn;
        }

        #endregion

        #region Méthodes privées

        private static FormuleGarantieDetailsDto ObtenirGarantieDetailsInfo(string codeOffre, string version, string type, string codeAvn, string codeFormule, int codeCible, string branche, string codeOption, long idGarantie, string codeObjetRisque, FormuleGarantieSaveDto volets, DateTime? dateEffAvnModifLocale, string user, bool isReadonly, ModeConsultation modeNavig)
        {
            var result = new FormuleGarantieDetailsDto();
            var formuleGarantiesPlat = new List<FormuleGarantieDetailsPlatDto>();

            // Récupération des détails en mode lecture ou historique
            if (isReadonly || modeNavig == ModeConsultation.Historique)
            {
                var idGar = idGarantie;
                // BUG 1795
                //if (!string.IsNullOrEmpty(codeObjetRisque))
                // End BUG 1795
                idGar = FormuleRepository.GetCodeGarantieBySeq(idGarantie.ToString(CultureInfo.InvariantCulture), codeOffre, version, type, codeFormule, codeOption, modeNavig);
                formuleGarantiesPlat = FormuleRepository.ObtenirGarantieDetailsInfo(codeOffre, version, type, codeAvn, codeFormule, codeOption, idGar.ToString(CultureInfo.InvariantCulture), isReadonly, modeNavig);
            }
            else
            {
                #region Récupération des détails en mode modification
                var paramsVoletsBloc = FormuleRepository.LoadVoletBlocParameters(codeOffre, string.IsNullOrEmpty(version) ? 0 : Convert.ToInt32(version), type, codeCible, branche);
                var paramsGaranties = FormuleRepository.LoadParameters(paramsVoletsBloc.Select(i => i.LibModele.Trim()).Distinct().ToList());

                if (paramsGaranties != null && paramsGaranties.Any())
                {
                    var item = new FormuleGarantieDetailsPlatDto();
                    formuleGarantiesPlat.Add(item);
                    var garantie = paramsGaranties.FirstOrDefault(i => i.C2SEQNIV == idGarantie);

                    if (garantie != null)
                    {
                        item.Caractere = garantie.C2CARNIV;
                        item.NatureStd = garantie.C2NATNIV;
                        item.GarantieIndexe = garantie.C2INANIV;
                        item.CATNAT = garantie.C2CNANIV;
                        item.InclusMontant = garantie.C2MRFNIV;
                        item.Application = "A";
                        item.TypeEmission = "P";
                        item.CodeTaxe = garantie.C2TAXNIV;
                        item.DefGarantie = garantie.GADFGNIV;
                        item.Garantie = garantie.C2GARNIV;
                        item.LibelleGarantie = garantie.GADEANIV;
                        item.TypeControleDate = garantie.C2TCDNIV;
                        item.AlimAssiette = garantie.C4ALANIV;
                        item.IsRegul = garantie.GARGENIV;

                        if (string.IsNullOrEmpty(garantie.GATRGNIV) || string.IsNullOrEmpty(garantie.LIBGATRGNIV))
                            item.LibGrilleRegul = "";
                        else
                            item.LibGrilleRegul = garantie.GATRGNIV + " - " + garantie.LIBGATRGNIV;
                    }

                    GarantiePortee garDateStd = new GarantiePortee();

                    var codeGarantie = FormuleRepository.GetCodeGarantieBySeq(idGarantie.ToString(), codeOffre, version, type, codeFormule, codeOption, modeNavig);
                    if (codeGarantie != 0)
                    {

                        //CALL PACHEV
                        var sqlParAchev = string.Format(@"select kdedatdeb DATEDEBRETURNCOL, kdedatfin DATEFINRETURNCOL from kpgaran where kdeid = {0}", codeGarantie);
                        var resParAchev = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlParAchev);
                        Int64 dateDeb = 0;
                        Int64 dateFin = 0;
                        if (resParAchev != null && resParAchev.Any())
                        {
                            dateDeb = resParAchev.FirstOrDefault().DateDebReturnCol;
                            dateFin = resParAchev.FirstOrDefault().DateFinReturnCol;
                        }
                        if (dateDeb == 0 && dateFin == 0)
                            InitParfaitAchevement(codeOffre, version, type, codeGarantie.ToString(), codeFormule, codeOption, codeAvn, codeObjetRisque, user, volets);
                    }

                    var sql = string.Format(@"SELECT KDECRAVN  CREATENUMAVN,
                                                   KDECAR CARACTERE,
                                                   KDENAT NATURE,
	                                               KDEGAN NATRETENUE,
	                                               KDEINA GARANTIEINDEXE,
	                                               KDECATNAT SOUMISCATNAT,
	                                               KDEALIREF MONTANTREF,
	                                               KDEPRP APPLICATION,
	                                               KDETYPEMI TYPEMISSION,
	                                               KDETAXCOD CODETAXE,
	                                               KDEDEFG DEFGARANTIE,
	                                               KDEGARAN GARANTIE,
	                                               KDETCD TYPECONTROLEDATE,
	                                               KDEALA ALIMASSIETTE ,
                                                   KDEDATDEB DATEDEB , 
                                                   KDEHEUDEB HEUREDEB , 
                                                   KDEDATFIN DATEFIN , 
                                                   KDEHEUFIN HEUREFIN,
                                                   KDEWDDEB DATEDEBW,
                                                   KDEWHDEB HEUREDEBW,
                                                   KDEWDFIN DATEFINW,
                                                   KDEWHFIN HEUREFINW, 
                                                   KDEDUREE DUREE, 
                                                   KDEDURUNI DUREEUNITE
                                            FROM KPGARAN WHERE KDEIPB = '{0}' AND KDEALX = {1} AND KDETYP = '{2}' AND KDESEQ = '{3}' AND KDEFOR = '{4}'", codeOffre.PadLeft(9, ' '), version, type, idGarantie, codeFormule);

                    var garantieUser = DbBase.Settings.ExecuteList<FormuleGarantieDetailsPlatDto>(CommandType.Text, sql).FirstOrDefault();

                    if (garantieUser != null)
                    {
                        var infos = FormuleRepository.GetGarantieInfos(codeOffre, Convert.ToInt32(version), type, Convert.ToInt32(codeFormule), Convert.ToInt32(codeOption));

                        var stdDates = FormuleRepository.CalculateAndUpdateDatesGar(codeOffre, Convert.ToInt32(version), type, codeAvn, Convert.ToInt32(codeFormule), Convert.ToInt32(codeOption), dateEffAvnModifLocale, Convert.ToInt32(codeGarantie));

                        if (garantieUser.CreateNumAvn.ToString() == codeAvn && stdDates.DateDebW < AlbConvert.ConvertDateToInt(dateEffAvnModifLocale))
                            stdDates.DateDebW = AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).HasValue ? AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value : stdDates.DateDebW;
                        if (garantieUser.CreateNumAvn.ToString() == codeAvn)
                            if (garantieUser.DateDeb == 0)
                            {
                                stdDates.DateDeb = AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).HasValue ? AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value : garantieUser.DateDeb;
                            }
                            else
                            {
                                stdDates.DateDeb = AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).HasValue ? (garantieUser.DateDeb > AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value ? garantieUser.DateDeb : AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value) : garantieUser.DateDeb;
                                //AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).HasValue ? AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value : garantieUser.DateDeb;
                            }


                        item.Caractere = garantieUser.Caractere;
                        item.Nature = garantieUser.Nature;

                        item.DateDeb = garantieUser.DateDeb;
                        item.HeureDeb = garantieUser.HeureDeb;
                        item.DateFin = garantieUser.DateFin;
                        item.HeureDebW = stdDates.HeureDebW;
                        item.DateDebW = stdDates.DateDebW;
                        item.DateFinW = stdDates.DateFinW;
                        item.HeureFinW = stdDates.HeureFinW;
                        item.HeureFin = garantieUser.HeureFin;
                        item.Duree = garantieUser.Duree;
                        item.DureeUnite = garantieUser.DureeUnite;
                        item.GarantieIndexe = garantieUser.GarantieIndexe;
                        item.CATNAT = garantieUser.CATNAT;
                        item.InclusMontant = garantieUser.InclusMontant;
                        item.Application = garantieUser.Application;
                        item.TypeEmission = garantieUser.TypeEmission;
                        item.CodeTaxe = garantieUser.CodeTaxe;
                        item.DefGarantie = garantieUser.DefGarantie;
                        item.Garantie = garantieUser.Garantie;
                        //item.LibelleGarantie = garantieUser.LibelleGarantie;
                        item.TypeControleDate = garantieUser.TypeControleDate;
                        item.AlimAssiette = garantieUser.AlimAssiette;

                        // ARA
                        item.CreateNumAvn = garantieUser.CreateNumAvn;

                        item.GarantieIndexe = infos.GarantieIndex;
                        item.LCIW = infos.Lci;
                        item.FranchiseW = infos.Franchise;
                        item.AssietteW = infos.Assiette;
                        item.CATNATW = infos.CatNat;
                        //Start  BUG 1362
                        //item.GarTemp = FormuleRepository.GetGarantiePeriodicite(codeOffre, Convert.ToInt32(version), type);
                        //END  BUG 1362
                    }
                    else
                    {
                        var garantieDateStd = FormuleRepository.CalculateDatesGarDetails(codeOffre, Convert.ToInt32(version), type, codeAvn, Convert.ToInt32(codeFormule), Convert.ToInt32(codeOption));
                        //CALL PACHEV

                        var res = InitParfaitAchevementNoUpdate(item.Garantie);

                        var datefin = AlbConvert.ConvertIntToDateHour(garantieDateStd.DateFin);
                        var heurefin = AlbConvert.GetTimeFromDate(datefin);

                        var datedeb = AlbConvert.ConvertIntToDateHour(garantieDateStd.DateDebut);
                        var heuredeb = AlbConvert.GetTimeFromDate(datedeb);

                        item.DateDebW = AlbConvert.ConvertDateToInt(datedeb) ?? 0;
                        item.HeureDebW = AlbConvert.ConvertTimeToInt(heuredeb) ?? 0;
                        item.DateFinW = AlbConvert.ConvertDateToInt(datefin) ?? 0;
                        item.HeureFinW = AlbConvert.ConvertTimeToInt(heurefin) ?? 0;
                        // ARA
                        item.CreateNumAvn = garantieDateStd.AvenantCreation;

                        if (res.StrReturnCol == "H" && datefin != null)
                        {

                            var datCalDeb = datefin.Value.AddMonths(-Convert.ToInt32(Math.Floor(res.DecReturnCol))).AddMinutes(1);

                            var heuCalDeb = AlbConvert.GetTimeFromDate(datCalDeb);

                            item.DateDeb = AlbConvert.ConvertDateToInt(datCalDeb) ?? 0;
                            item.HeureDeb = AlbConvert.ConvertTimeToInt(heuCalDeb) ?? 0;
                            item.DateFin = AlbConvert.ConvertDateToInt(datefin) ?? 0;
                            item.HeureFin = AlbConvert.ConvertTimeToInt(heurefin) ?? 0;
                        }
                    }
                }
                #endregion
            }


            if (formuleGarantiesPlat != null && formuleGarantiesPlat.Any())
            {
                var firstRow = formuleGarantiesPlat.FirstOrDefault();

                result.CodeGarantie = firstRow.CodeGarantie.ToString();
                result.Garantie = firstRow.Garantie;
                result.LibelleGarantie = firstRow.LibelleGarantie;
                result.Caractere = firstRow.Caractere;
                result.NatureStd = firstRow.NatureStd;
                result.Nature = firstRow.Nature;
                result.TypeControleDate = firstRow.TypeControleDate;
                result.Duree = firstRow.Duree == 0 ? "" : firstRow.Duree.ToString();
                result.DureeUnite = firstRow.DureeUnite;
                result.GarantieIndexe = firstRow.GarantieIndexe == "O";
                result.CATNAT = firstRow.CATNAT == "O";
                result.InclusMontant = firstRow.InclusMontant == "O";
                result.Application = firstRow.Application;
                result.TypeEmission = firstRow.TypeEmission;
                result.CodeTaxe = firstRow.CodeTaxe;
                result.DefGarantie = firstRow.DefGarantie;
                result.AlimAssiette = firstRow.AlimAssiette;
                result.IsRegul = firstRow.IsRegul;
                result.LibGrilleRegul = firstRow.LibGrilleRegul;
                result.AvnCreation = firstRow.CreateNumAvn;

                if (Convert.ToInt32(codeAvn) > 0)
                {
                    if (firstRow.CreateNumAvn == Convert.ToInt32(codeAvn))
                    {
                        result.DateDebStd = dateEffAvnModifLocale;
                        firstRow.DateDeb = Convert.ToInt32(firstRow.DateDebW) == 0 ? AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value : Convert.ToInt32(firstRow.DateDebW) < AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value ? AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value : Convert.ToInt32(firstRow.DateDebW);
                    }
                    else
                    {
                        result.DateDebStd = AlbConvert.ConvertIntToDate(Convert.ToInt32(firstRow.DateDebW));
                        firstRow.DateFin = firstRow.Duree > 0 ? firstRow.DateFin = 0 : firstRow.DateFin;
                        firstRow.HeureFin = firstRow.Duree > 0 ? firstRow.HeureFin = 0 : firstRow.HeureFin;
                    }

                }
                else
                {
                    result.DateDebStd = AlbConvert.ConvertIntToDate(Convert.ToInt32(firstRow.DateDebW));
                }

                //AlbConvert.ConvertIntToDate(Convert.ToInt32(firstRow.DateDebW));
                result.HeureDebStd = AlbConvert.ConvertIntToTime(firstRow.HeureDebW);
                result.DateFinStd = AlbConvert.ConvertIntToDate(Convert.ToInt32(firstRow.DateFinW));
                result.HeureFinStd = AlbConvert.ConvertIntToTime(firstRow.HeureFinW);

                result.DateDeb = AlbConvert.ConvertIntToDate(firstRow.DateDeb);
                //result.DateDeb = firstRow.DateDeb > 0 ? AlbConvert.ConvertIntToDate(firstRow.DateDeb) : AlbConvert.ConvertIntToDate(Convert.ToInt32(firstRow.DateDebW));
                result.HeureDeb = AlbConvert.ConvertIntToTime(firstRow.HeureDeb);
                //result.HeureDeb = firstRow.HeureDeb > 0 ? AlbConvert.ConvertIntToTime(firstRow.HeureDeb) : AlbConvert.ConvertIntToTime(firstRow.HeureDebW);
                result.DateFin = AlbConvert.ConvertIntToDate(firstRow.DateFin);
                result.HeureFin = AlbConvert.ConvertIntToTime(firstRow.HeureFin);

                result.GarantieIndexeW = firstRow.GarantieIndexeW;
                result.LCIW = firstRow.LCIW;
                result.LCI = firstRow.LCIW == "O";
                result.FranchiseW = firstRow.FranchiseW;
                result.Franchise = firstRow.FranchiseW == "O";
                result.AssietteW = firstRow.AssietteW;
                result.Assiette = firstRow.AssietteW == "O";
                result.CATNATW = firstRow.CATNATW;
                result.CATNAT = firstRow.CATNATW == "O";
                result.AvnCreation = firstRow.CreateNumAvn;
                if (!string.IsNullOrEmpty(codeAvn) && codeAvn != "0")
                    result.AvnReadOnly = firstRow.CreateNumAvn != Convert.ToInt64(codeAvn);
                //Start  BUG 1362
                //result.GarTemp = firstRow.GarTemp;
                //END  BUG 1362

            }

            //Start  BUG 1362
            if (result != null)
                result.GarTemp = FormuleRepository.GetGarantiePeriodicite(codeOffre, Convert.ToInt32(version), type);
            //END  BUG 1362

            return result;
        }

        private static string BuildWhereCondition(string value)
        {
            string cleanValue = " " + value + " ";
            motsIgnores.ForEach(m =>
            {
                cleanValue = cleanValue.Replace(m, "||");
            });

            var tabValue = cleanValue.Split(new[] { "||" }, StringSplitOptions.None).ToList();
            StringBuilder whereCondition = new StringBuilder();
            tabValue.ForEach(t =>
            {
                if (!string.IsNullOrEmpty(t))
                    whereCondition.Append(string.Format(" LOWER(ABLLIB) LIKE '%{0}%' OR", t.Trim().ToLower()));
            });

            string result = whereCondition.ToString();

            return !string.IsNullOrEmpty(result) ? result.Substring(0, result.Length - 2) : result;
        }

        #endregion

        public static void InsertGarantieRecursive(string codeOffre, string version, string type, string codeFormule, string codeOption, string codeAvenant, string user, long date, int idBloc, string codeSeq, GarantiesDto gar, List<GarantiesDto> paramsGaranties, ref long codeGarantie)
        {
            if (gar == null) return;

            var idGar = FormuleRepository.PopulateKpgaran(codeOffre, Convert.ToInt32(version), type,
                                                                      new KpgaranDto
                                                                      {
                                                                          KDETYP = type,
                                                                          KDEIPB = codeOffre,
                                                                          KDEALX = Convert.ToInt32(version),
                                                                          KDEFOR = Convert.ToInt32(codeFormule),
                                                                          KDEOPT = Convert.ToInt32(codeOption),
                                                                          KDEKDCID = idBloc,
                                                                          KDEGARAN = gar.GAGARNIV,
                                                                          KDESEQ = gar.C2SEQNIV,
                                                                          KDENIVEAU = gar.C2NIVNIV,
                                                                          KDESEM = gar.C2SEMNIV,
                                                                          KDESE1 = gar.C2SE1NIV,
                                                                          KDETRI = gar.C2TRINIV,
                                                                          KDENUMPRES = 1,
                                                                          KDEAJOUT = "N",
                                                                          KDECAR = gar.C2CARNIV,
                                                                          KDENAT = gar.C2NATNIV,
                                                                          KDEGAN = gar.C2CARNIV == "O" ? gar.C2NATNIV : "A",//niv2.NatureParam,
                                                                          KDEKDFID = 0,
                                                                          KDEDEFG = gar.GADFGNIV,
                                                                          KDEKDHID = 0,
                                                                          //KDEDATDEB = niv1.,
                                                                          //KDEHEUDEB = ,
                                                                          //KDEDATFIN = ,
                                                                          //KDEHEUFIN = ,
                                                                          KDEDUREE = 0,
                                                                          KDEDURUNI = "",
                                                                          KDEPRP = "A",
                                                                          KDETYPEMI = "P",
                                                                          KDEALIREF = gar.C2MRFNIV,
                                                                          KDECATNAT = gar.C2CNANIV,
                                                                          KDEINA = gar.C2INANIV,
                                                                          KDETAXCOD = gar.C2TAXNIV,
                                                                          KDETAXREP = 0,
                                                                          KDECRAVN = string.IsNullOrEmpty(codeAvenant) ? 0 : Convert.ToInt32(codeAvenant),
                                                                          KDECRU = user,
                                                                          KDECRD = date,
                                                                          KDEMAJAVN = string.IsNullOrEmpty(codeAvenant) ? 0 : Convert.ToInt32(codeAvenant),
                                                                          KDEASVALO = gar.C4VALNIV,
                                                                          KDEASVALA = gar.C4VALNIV,
                                                                          KDEASVALW = 0,
                                                                          KDEASUNIT = gar.C4UNTNIV,
                                                                          KDEASBASE = gar.C4BASNIV,
                                                                          KDEASMOD = gar.C4MAJNIV,
                                                                          KDEASOBLI = gar.C4OBLNIV,
                                                                          KDEINVSP = gar.GAINVNIV,
                                                                          KDEINVEN = 0,
                                                                          //KDEWDDEB = ,
                                                                          //KDEWHDEB = ,
                                                                          //KDEWDFIN = ,
                                                                          //KDEWHFIN = ,
                                                                          KDETCD = gar.C2TCDNIV,
                                                                          KDEMODI = "N",
                                                                          KDEPIND = gar.C2INANIV,
                                                                          KDEPCATN = gar.C2CNANIV,
                                                                          KDEPREF = gar.C2MRFNIV,
                                                                          KDEPPRP = "A",
                                                                          KDEPEMI = "P",
                                                                          KDEPTAXC = gar.C2TAXNIV,
                                                                          KDEPNTM = gar.C2NTMNIV,
                                                                          KDEALA = gar.C4ALANIV,
                                                                          KDEPALA = gar.C4ALANIV,
                                                                          KDEALO = string.Empty
                                                                      });
            FormuleRepository.PopulateKpgartar(codeOffre, Convert.ToInt32(version), type, idGar,
                                               new KpgartarDto
                                               {
                                                   KDGIPB = codeOffre,
                                                   KDGALX = Convert.ToInt32(version),
                                                   KDGFOR = Convert.ToInt32(codeFormule),
                                                   KDGOPT = Convert.ToInt32(codeOption),
                                                   KDGCHT = 0,
                                                   KDGCMC = 0,
                                                   KDGCTT = 0,
                                                   KDGTFF = 0,
                                                   KDGTMC = 0,
                                                   KDGTYP = type,
                                                   KDGFMABASE = gar.FRHBASMAXNIV,
                                                   KDGFMAUNIT = gar.FRHBASMAXNIV,
                                                   KDGFMAVALA = gar.FRHVALMAXNIV,
                                                   KDGFMAVALO = gar.FRHVALMAXNIV,
                                                   KDGFMAVALW = 0,
                                                   KDGFMIBASE = gar.FRHBASMINNIV,
                                                   KDGFMIUNIT = gar.FRHUNTMINNIV,
                                                   KDGFMIVALA = gar.FRHVALMINNIV,
                                                   KDGFMIVALO = gar.FRHVALMINNIV,
                                                   KDGFMIVALW = 0,
                                                   KDGFRHBASE = gar.FRHBASNIV,
                                                   KDGFRHMOD = gar.FRHMODNIV,
                                                   KDGFRHOBL = gar.FRHOBLNIV,
                                                   KDGFRHUNIT = gar.FRHUNTNIV,
                                                   KDGFRHVALA = gar.FRHVALNIV,
                                                   KDGFRHVALO = gar.FRHVALNIV,
                                                   KDGFRHVALW = 0,
                                                   KDGGARAN = gar.C2GARNIV,
                                                   KDGKDEID = idGar,
                                                   //KDGKDIID = NEWCODELCIEXP,
                                                   //KDGKDKID = NEWCODEFRHEXP,
                                                   KDGLCIBASE = gar.LCIBASNIV,
                                                   KDGLCIMOD = gar.LCIMODNIV,
                                                   KDGLCIOBL = gar.LCIOBLNIV,
                                                   KDGLCIUNIT = gar.LCIUNTNIV,
                                                   KDGLCIVALA = gar.LCIVALNIV,
                                                   KDGLCIVALO = gar.LCIVALNIV,
                                                   KDGLCIVALW = 0,
                                                   KDGMNTBASE = 0,
                                                   KDGNUMTAR = 1, // mettre 2 ???
                                                   KDGPRIBASE = gar.PRIBASNIV,
                                                   KDGPRIMOD = gar.PRIMODNIV,
                                                   KDGPRIMPRO = 0,
                                                   KDGPRIOBL = gar.PRIOBLNIV,
                                                   KDGPRIUNIT = gar.PRIUNTNIV,
                                                   KDGPRIVALA = gar.PRIVALNIV,
                                                   KDGPRIVALO = gar.PRIVALNIV,
                                                   KDGPRIVALW = 0
                                               });

            InsertGarantieRecursive(codeOffre, version, type, codeFormule, codeOption, codeAvenant, user, date, idBloc, codeSeq, paramsGaranties.FirstOrDefault(i => i.C2SEQNIV == gar.C2SEMNIV), paramsGaranties, ref codeGarantie);

            if (gar.C2SEQNIV.ToString(CultureInfo.InvariantCulture) == codeSeq)
                codeGarantie = idGar;
        }

        public static long InitFormuleIfNotExistsForGar(string codeOffre, string version, string type, string codeFormule, string codeOption,
                                            string codeAvenant, string codeObjetRisque, string user,
                                            string codeSeq,
                                            FormuleGarantieSaveDto formulesGarantiesSave)
        {
            var idGar = FormuleRepository.GetCodeGarantieBySeq(codeSeq, codeOffre, version, type, codeFormule,
                                                               codeOption, ModeConsultation.Standard);

            if (idGar == 0)
            {
                long guidBloc = 0;
                long guidVolet = 0;
                foreach (var volet in formulesGarantiesSave.Volets)
                {
                    foreach (var bloc in volet.Blocs)
                    {
                        foreach (var modele in bloc.Modeles)
                        {
                            foreach (var niv1 in modele.Modeles)
                            {
                                if (niv1.GuidGarantie == codeSeq)
                                {
                                    guidVolet = volet.GuidId;
                                    guidBloc = bloc.GuidId;
                                    break;
                                }

                                foreach (var niv2 in niv1.Modeles)
                                {
                                    if (niv2.GuidGarantie == codeSeq)
                                    {
                                        guidVolet = volet.GuidId;
                                        guidBloc = bloc.GuidId;
                                        break;
                                    }
                                    foreach (var niv3 in niv2.Modeles)
                                    {
                                        if (niv3.GuidGarantie == codeSeq)
                                        {
                                            guidVolet = volet.GuidId;
                                            guidBloc = bloc.GuidId;
                                            break;
                                        }
                                        foreach (var niv4 in niv3.Modeles)
                                        {
                                            if (niv4.GuidGarantie == codeSeq)
                                            {
                                                guidVolet = volet.GuidId;
                                                guidBloc = bloc.GuidId;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                //TODO MODIF
                var brancheCible = CommonRepository.GetBrancheCibleFormule(codeOffre, version, type, codeAvenant, codeFormule, ModeConsultation.Standard);
                var codeCible = (int)brancheCible.Cible.GuidId;
                var branche = brancheCible.Code;
                var kdbid = FormuleRepository.GetLienKpOpt(codeOffre, version, type, codeFormule, codeOption);
                var dateNow = (long)AlbConvert.ConvertDateToInt(DateTime.Now);

                var codeRisque = codeObjetRisque.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[0];
                var codeObjets =
                    codeObjetRisque.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new[] { '_' },
                                                                                                       StringSplitOptions.
                                                                                                           RemoveEmptyEntries).
                        ToList();

                var nbObj = FormuleRepository.GetNbrObj(codeOffre, version, codeRisque);
                var kddid = 0;
                if (nbObj == codeObjets.Count)
                {
                    kddid = CommonRepository.GetAS400Id("KDDID");
                    FormuleRepository.PopulateKpoptap(codeOffre, version, type, new KpoptapDto
                    {
                        KDDID = kddid,
                        KDDKDBID = kdbid,
                        KDDIPB = codeOffre,
                        KDDALX =
                            string.IsNullOrEmpty(version)
                                ? 0
                                : Convert.ToInt32(version),
                        KDDTYP = type,
                        KDDFOR =
                            string.IsNullOrEmpty(codeFormule)
                                ? 0
                                : Convert.ToInt32(codeFormule),
                        KDDOPT =
                            string.IsNullOrEmpty(codeOption)
                                ? 0
                                : Convert.ToInt32(codeOption),
                        KDDRSQ =
                            Convert.ToInt32(
                                codeObjetRisque.Split(';')[0]),
                        KDDOBJ = 0,
                        KDDCRD = dateNow,
                        KDDCRU = user,
                        KDDMAJD = dateNow,
                        KDDMAJU = user,
                        KDDPERI = "RQ",
                        KDDINVEN = 0,
                        KDDINVEP = 0
                    });
                }
                else
                {
                    codeObjets.ForEach(i =>
                    {
                        kddid = CommonRepository.GetAS400Id("KDDID");
                        FormuleRepository.PopulateKpoptap(codeOffre, version, type, new KpoptapDto
                        {
                            KDDID = kddid,
                            KDDKDBID =
                                kdbid,
                            KDDIPB =
                                codeOffre,
                            KDDALX =
                                string.
                                    IsNullOrEmpty
                                    (version)
                                    ? 0
                                    : Convert
                                          .
                                          ToInt32
                                          (version),
                            KDDTYP = type,
                            KDDFOR =
                                string.
                                    IsNullOrEmpty
                                    (codeFormule)
                                    ? 0
                                    : Convert
                                          .
                                          ToInt32
                                          (codeFormule),
                            KDDOPT =
                                string.
                                    IsNullOrEmpty
                                    (codeOption)
                                    ? 0
                                    : Convert
                                          .
                                          ToInt32
                                          (codeOption),
                            KDDRSQ =
                                Convert.
                                ToInt32(
                                    codeObjetRisque
                                        .
                                        Split
                                        (';')
                                        [
                                            0
                                        ]),
                            KDDOBJ =
                                Convert.
                                ToInt32(i),
                            KDDCRD =
                                dateNow,
                            KDDCRU = user,
                            KDDMAJD =
                                dateNow,
                            KDDMAJU =
                                user,
                            KDDPERI =
                                "OB",
                            KDDINVEN = 0,
                            KDDINVEP = 0
                        });
                    });
                }


                var paramsVoletBlocs = FormuleRepository.LoadVoletBlocParameters(codeOffre, Convert.ToInt32(version), type,
                                                                                 codeCible, branche);
                var paramsGaranties =
                    FormuleRepository.LoadParameters(paramsVoletBlocs.Select(i => i.LibModele.Trim()).Distinct().ToList());
                var v = paramsVoletBlocs.FirstOrDefault(i => i.Guidv == guidVolet);
                var b = paramsVoletBlocs.FirstOrDefault(i => i.Guidb == guidBloc);
                var g = paramsGaranties.FirstOrDefault(i => i.C2SEQNIV.ToString().Trim() == codeSeq);

                var idVolet = FormuleRepository.PopulateKpoptd(codeOffre, Convert.ToInt32(version), type, new KpoptdDto
                {
                    KDCIPB =
                        codeOffre,
                    KDCALX =
                        Convert.
                        ToInt32(
                            version),
                    KDCTYP = type,
                    KDCFOR =
                        Convert.
                        ToInt32(
                            codeFormule),
                    KDCOPT =
                        Convert.
                        ToInt32(
                            codeOption),
                    KDCFLAG = 1,
                    KDCKAEID = 0,
                    KDCKAKID =
                        v.
                        GuidVolet,
                    KDCKAQID = 0,
                    KDCKARID =
                        v.GuidM,
                    KDCKDBID =
                        kdbid,
                    KDCMAJD =
                        dateNow,
                    KDCMAJU = user,
                    KDCMODELE =
                        v.
                        LibModele,
                    KDCTENG = "V",
                    KDCCRD =
                        dateNow,
                    KDCCRU = user,
                    KDCORDRE = v.VoletOrdre
                }, true);

                var idBloc = FormuleRepository.PopulateKpoptd(codeOffre, Convert.ToInt32(version), type, new KpoptdDto
                {
                    KDCIPB =
                        codeOffre,
                    KDCALX =
                        Convert.
                        ToInt32(
                            version),
                    KDCTYP = type,
                    KDCFOR =
                        Convert.
                        ToInt32(
                            codeFormule),
                    KDCOPT =
                        Convert.
                        ToInt32(
                            codeOption),
                    KDCFLAG = 1,
                    KDCKAEID =
                        b.GuidBloc,
                    KDCKAKID =
                        b.GuidVolet,
                    KDCKAQID =
                        b.Guidb,
                    KDCKARID =
                        b.GuidM,
                    KDCKDBID =
                        kdbid,
                    KDCMAJD =
                        dateNow,
                    KDCMAJU = user,
                    KDCMODELE =
                        b.LibModele,
                    KDCTENG = "B",
                    KDCCRD =
                        dateNow,
                    KDCCRU = user,
                    KDCORDRE = b.BlocOrdre
                }, true);

                //insert gar
                InsertGarantieRecursive(codeOffre, version, type, codeFormule, codeOption, codeAvenant, user, dateNow, idBloc,
                                        codeSeq, g, paramsGaranties, ref idGar);
            }
            return idGar;
        }

        public static void InitParfaitAchevement(string codeOffre, string version, string type, string codeGarantie, string codeFormule, string codeOption, string codeAvenant, string codeObjetRisque, string user, FormuleGarantieSaveDto volets)
        {
            //long idGar = 0;
            if (!string.IsNullOrEmpty(codeGarantie))
            {
                FormuleRepository.InitParfaitAchevement(codeOffre, version, type, codeGarantie); //idGar.ToString(CultureInfo.InvariantCulture)
                //idGar = InitFormuleIfNotExistsForGar(codeOffre, version, type, codeFormule, codeOption, codeAvenant, codeObjetRisque, user, codeGarantie, volets);
            }
        }

        public static DtoCommon InitParfaitAchevementNoUpdate(string gar)
        {
            return FormuleRepository.InitParfaitAchevementNoUpdate(gar);
        }
    }
}

