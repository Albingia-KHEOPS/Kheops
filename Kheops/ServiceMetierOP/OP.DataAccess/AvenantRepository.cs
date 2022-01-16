using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.PGM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess
{
    public class AvenantRepository
    {
        #region Ecran création avenant

        public static List<AvenantAlerteDto> GetListAlertesAvenant(string codeOffre, string version, string type, string user)
        {
            var dateNow = DateTime.Now;

            DbParameter[] param = new DbParameter[7];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_DATENOW", 0)
            {
                Value = AlbConvert.ConvertDateToInt(dateNow)
            };
            param[4] = new EacParameter("P_HOURNOW", 0)
            {
                Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow))
            };
            param[5] = new EacParameter("P_LASTYEAR", 0)
            {
                Value = AlbConvert.ConvertDateToInt(dateNow.AddYears(-1))
            };
            param[6] = new EacParameter("P_USER", user);

            return DbBase.Settings.ExecuteList<AvenantAlerteDto>(CommandType.StoredProcedure, "SP_GETLISTALERTE", param);
        }

        public static AvenantModificationDto GetInfoAvenantModification(string codeOffre, string version, string type, short numAvn, string typeAvt, string modeAvt, string modeNavig)
        {
            var enumModeNavig = modeNavig.ParseCode<ModeConsultation>();
            AvenantModificationDto modele = new AvenantModificationDto();
            string error = string.Empty;
            Int64 numInterne = 0;
            DateTime? prochEchHisto = null;

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength)
            {
                Value = codeOffre.PadLeft(9, ' ')
            };
            param[1] = new EacParameter("P_VERSION", DbType.Int32)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength)
            {
                Value = type
            };

            string sqlInterne = @"SELECT PBAVN INT32RETURNCOL FROM YPOBASE WHERE PBIPB = :P_CODEOFFRE AND PBALX = :P_VERSION AND PBTYP = :P_TYPE";
            var resultInterne = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlInterne, param);
            if (resultInterne != null && resultInterne.Any())
                numInterne = resultInterne.FirstOrDefault().Int32ReturnCol;

            if (numInterne == 0)
            {
                param = new EacParameter[2];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength)
                {
                    Value = codeOffre.PadLeft(9, ' ')
                };
                param[1] = new EacParameter("P_VERSION", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                };

                string sqlHisto = @"SELECT COUNT(*) NBLIGN FROM YHPBASE WHERE PBIPB = :P_CODEOFFRE AND PBALX = :P_VERSION AND PBHIN = 1";
                if (CommonRepository.ExistRowParam(sqlHisto, param))
                {
                    error = "Anomalie historique";
                }
                else
                {
                    param = new EacParameter[3];
                    param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength)
                    {
                        Value = codeOffre.PadLeft(9, ' ')
                    };
                    param[1] = new EacParameter("P_VERSION", DbType.Int32)
                    {
                        Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                    };
                    param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength)
                    {
                        Value = type
                    };

                    string sqlCodeRetour = @"SELECT PBTAC STRRETURNCOL FROM YPOBASE WHERE PBIPB = :P_CODEOFFRE AND PBALX = :P_VERSION AND PBTYP = :P_TYPE";
                    var resultCodeRetour = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlCodeRetour, param);
                    if (resultCodeRetour != null && resultCodeRetour.Any())
                    {
                        if (string.IsNullOrEmpty(resultCodeRetour.FirstOrDefault().StrReturnCol))
                            error = "Aucun accord sur l'affaire nouvelle";
                    }
                }
            }
            else
            {
                EacParameter[] paramHisto = new EacParameter[3];
                string sqlDateHisto = string.Empty;

                if (modeAvt == "CREATE")
                {
                    paramHisto = new EacParameter[2];
                    paramHisto[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength)
                    {
                        Value = codeOffre.PadLeft(9, ' ')
                    };
                    paramHisto[1] = new EacParameter("version", DbType.Int32)
                    {
                        Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                    };

                    sqlDateHisto = @"SELECT JDPEA * 10000 + JDPEM * 100 + JDPEJ INT64RETURNCOL FROM YPRTENT 
                    WHERE JDIPB = :codeContrat AND JDALX = :version";
                }
                else
                {
                    paramHisto = new EacParameter[3];
                    paramHisto[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength)
                    {
                        Value = codeOffre.PadLeft(9, ' ')
                    };
                    paramHisto[1] = new EacParameter("version", DbType.Int32)
                    {
                        Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                    };
                    paramHisto[2] = new EacParameter("codeAvn", DbType.Int64)
                    {
                        Value = modeAvt == "CREATE" ? numInterne : numInterne - 1
                    };

                    sqlDateHisto = @"SELECT JDPEA * 10000 + JDPEM * 100 + JDPEJ INT64RETURNCOL FROM YHRTENT 
                    WHERE JDIPB = :codeContrat AND JDALX = :version AND JDAVN = :codeAvn";
                }

                var resultHisto = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlDateHisto, paramHisto);
                if (resultHisto != null && resultHisto.Any())
                {
                    prochEchHisto = AlbConvert.ConvertIntToDate(Convert.ToInt32(resultHisto.FirstOrDefault().Int64ReturnCol));
                }
            }

            var param2 = new List<EacParameter>
            {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },
                new EacParameter("version", DbType.Int32) { Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0 },
                new EacParameter("type", DbType.AnsiStringFixedLength) { Value = type }
            };
            if (enumModeNavig == ModeConsultation.Historique)
            {
                param2.Add(new EacParameter("numAvn", DbType.Int16) { Value = numAvn });
            }

            string sql = string.Format(@"SELECT PBTTR TYPEAVT, PARTYP.TPLIL LIBELLEAVT, PBAVN NUMINTERNEAVT, PBAVA *10000 + PBAVM * 100 + PBAVJ DATEEFFET, KAAAVH HEUREEFFET,
                                PBAVK NUMAVT, PBAVC MOTIFAVT, PARMOTIF.TPLIL LIBMOTIFAVT, KADDESI DESCRIPTIONAVT, KAJOBSV OBSERVATIONSAVT
                                FROM {0}
                                LEFT JOIN {1} ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP {6}
                                LEFT JOIN {2} ON KAAAVDS = KADCHR
                                LEFT JOIN {3} ON KAAOBSV = KAJCHR
                                {4}
                                {5}
                            WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type {7}",
                /*0*/CommonRepository.GetPrefixeHisto(enumModeNavig, "YPOBASE"),
                /*1*/CommonRepository.GetPrefixeHisto(enumModeNavig, "KPENT"),
                /*2*/CommonRepository.GetPrefixeHisto(enumModeNavig, "KPDESI"),
                /*3*/CommonRepository.GetPrefixeHisto(enumModeNavig, "KPOBSV"),
                /*4*/CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", "PARTYP", " AND PARTYP.TCOD = PBTTR"),
                /*5*/CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBAVC", "PARMOTIF", " AND PARMOTIF.TCOD = PBAVC"),
                /*6*/enumModeNavig == ModeConsultation.Historique ? "AND PBAVN = KAAAVN" : string.Empty,
                /*7*/enumModeNavig == ModeConsultation.Historique ? "AND PBAVN = :numAvn" : string.Empty);

            var result = DbBase.Settings.ExecuteList<AvenantModificationDto>(CommandType.Text, sql, param2);
            if (result != null && result.Any())
            {
                modele = result.FirstOrDefault();
                modele.TypeAvt = typeAvt;
                modele.LibelleAvt = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "PRODU", "PBTTR", typeAvt).Libelle;

                if (modeAvt == "CREATE")
                {
                    if (modele.NumInterneAvt == 0)
                        modele.NumInterneAvt = numInterne + 1;
                    else
                        modele.NumInterneAvt = modele.NumInterneAvt + 1;
                    if (modele.NumAvt == 0)
                        modele.NumAvt = numInterne + 1;
                    else
                        modele.NumAvt = modele.NumAvt + 1;

                    modele.HeureEffetAvt = new TimeSpan(0);
                }

                modele.DateEffetAvt = AlbConvert.ConvertIntToDate(Convert.ToInt32(modele.DateEffet));
                modele.HeureEffetAvt = AlbConvert.ConvertIntToTime(modele.HeureEffet);
            }
            modele.Motifs = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBAVC");
            modele.ProchaineEchHisto = prochEchHisto;

            return modele;
        }

        public static AvenantResiliationDto GetInfoAvenantResiliation(string codeOffre, string version, string type, Int16 codeAvn, string typeAvt, string modeAvt, string modeNavig)
        {
            var enumModeNavig = modeNavig.ParseCode<ModeConsultation>();

            AvenantResiliationDto modele = new AvenantResiliationDto();
            string error = string.Empty;
            Int64 numInterne = 0;
            DateTime? dateFinHisto = null;
            DateTime? prochEchHisto = null;
            List<EacParameter> param = new List<EacParameter> {
                new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },
                new EacParameter("P_VERSION", DbType.Int32) {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                },
                new EacParameter("P_TYPE", DbType.AnsiStringFixedLength) {
                    Value = type
                }
            };

            if (enumModeNavig == ModeConsultation.Historique)
            {
                numInterne = codeAvn - 1;
            }
            else
            {
                string sqlInterne = @"SELECT PBAVN INT32RETURNCOL FROM YPOBASE WHERE PBIPB = :P_CODEOFFRE AND PBALX = :P_VERSION AND PBTYP = :P_TYPE";
                var resultInterne = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlInterne, param);
                if (resultInterne != null && resultInterne.Any())
                    numInterne = resultInterne.FirstOrDefault().Int32ReturnCol;
            }
            if (numInterne == 0)
            {
                string sqlHisto = string.Format(@"SELECT COUNT(*) NBLIGN FROM YHPBASE WHERE PBIPB = :P_CODEOFFRE AND PBALX = :P_VERSION AND PBHIN = 1",
                                            codeOffre.Trim(), !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0);
                if (CommonRepository.ExistRowParam(sqlHisto, param.Take(2)))
                    error = "Anomalie historique";
                else
                {
                    string sqlCodeRetour = @"SELECT PBTAC STRRETURNCOL FROM YPOBASE WHERE PBIPB = :P_CODEOFFRE AND PBALX = :P_VERSION AND PBTYP = :P_TYPE";
                    var resultCodeRetour = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlCodeRetour, param);
                    if (resultCodeRetour != null && resultCodeRetour.Any())
                    {
                        if (string.IsNullOrEmpty(resultCodeRetour.FirstOrDefault().StrReturnCol))
                            error = "Aucun accord sur l'affaire nouvelle";
                    }
                }
            }
            else if (modeAvt != "CREATE")
            {
                List<EacParameter> paramHisto = new List<EacParameter> {
                    new EacParameter("codeContrat", DbType.AnsiStringFixedLength) {
                        Value = codeOffre.PadLeft(9, ' ')
                    },
                    new EacParameter("version", DbType.Int32) {
                        Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                    },
                    new EacParameter("type", DbType.AnsiStringFixedLength) {
                        Value = type
                    },
                    new EacParameter("codeAvn", DbType.Int64) {
                        Value = numInterne - 1
                    }
                };

                string sqlDateHisto = @"SELECT PBFEA * 100000000 + PBFEM * 1000000 + PBFEJ * 10000 + PBFEH INT64RETURNCOL,
                    JDPEA * 10000 + JDPEM * 100 + JDPEJ DATEDEBRETURNCOL
                    FROM YHPBASE
                    INNER JOIN YHRTENT ON JDIPB = PBIPB AND JDALX = PBALX AND JDAVN = PBAVN 
                    WHERE PBIPB = :codeContrat AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvn";

                var resultHisto = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlDateHisto, paramHisto);
                if (resultHisto != null && resultHisto.Any())
                {
                    dateFinHisto = AlbConvert.ConvertIntToDateHour(resultHisto.FirstOrDefault().Int64ReturnCol);
                    prochEchHisto = AlbConvert.ConvertIntToDate(Convert.ToInt32(resultHisto.FirstOrDefault().DateDebReturnCol));
                }
            }

            param = new List<EacParameter> {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength) {
                    Value = codeOffre.PadLeft(9, ' ')
                },
                new EacParameter("version", DbType.Int32) {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                },
                new EacParameter("type", DbType.AnsiStringFixedLength) {
                    Value = type
                }
            };
            if (enumModeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("numAvn", DbType.Int16) { Value = codeAvn });
            }
            string sql = string.Format(@"SELECT PBTTR TYPEAVT, PARTYP.TPLIL LIBELLEAVT, PBAVN NUMINTERNEAVT, PBFEA *10000 + PBFEM * 100 + PBFEJ DATEFINGARANTIE, PBFEH HEUREFINGARANTIE,
                                PBAVK NUMAVT, PBRSC MOTIFAVT, PARMOTIF.TPLIL LIBMOTIFAVT, KADDESI DESCRIPTIONAVT, KAJOBSV OBSERVATIONSAVT, PARMOTIF.TPCA1 AVTECHEANCE
                                FROM {0}
                                LEFT JOIN {1} ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP {6}
                                LEFT JOIN {2} ON KAAAVDS = KADCHR
                                LEFT JOIN {3} ON KAAOBSV = KAJCHR
                                {4}
                                {5}
                            WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type {7}",
                /*0*/CommonRepository.GetPrefixeHisto(enumModeNavig, "YPOBASE"),
                /*1*/CommonRepository.GetPrefixeHisto(enumModeNavig, "KPENT"),
                /*2*/CommonRepository.GetPrefixeHisto(enumModeNavig, "KPDESI"),
                /*3*/CommonRepository.GetPrefixeHisto(enumModeNavig, "KPOBSV"),
                /*4*/CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", "PARTYP", " AND PARTYP.TCOD = PBTTR"),
                /*5*/CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBRSC", "PARMOTIF", " AND PARMOTIF.TCOD = PBRSC"),
                /*6*/enumModeNavig == ModeConsultation.Historique ? "AND PBAVN = KAAAVN" : string.Empty,
                /*7*/enumModeNavig == ModeConsultation.Historique ? "AND PBAVN = :numAvn" : string.Empty);
            var result = DbBase.Settings.ExecuteList<AvenantResiliationDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                modele = result.FirstOrDefault();
                modele.TypeAvt = typeAvt;
                modele.LibelleAvt = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "PRODU", "PBTTR", typeAvt).Libelle;

                modele.DateFinGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(modele.DateFin));
                modele.HeureFinGarantie = AlbConvert.ConvertIntToTimeMinute(modele.HeureFin);

                if (modeAvt == "CREATE")
                {
                    if (modele.NumInterneAvt == 0)
                        modele.NumInterneAvt = numInterne + 1;
                    else
                        modele.NumInterneAvt = modele.NumInterneAvt + 1;
                    if (modele.NumAvt == 0)
                        modele.NumAvt = numInterne + 1;
                    else
                        modele.NumAvt = modele.NumAvt + 1;

                    modele.HeureFinGarantie = AlbConvert.ConvertIntToTimeMinute(2359);
                }
            }

            modele.AvenantEch = modele.AvtEcheance == "O";
            modele.Motifs = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBRSC");

            var resultCtrl = CommonRepository.AvenantResilControleTypeDate(codeOffre, version, codeAvn, typeAvt, modeAvt);
            if (resultCtrl.Split('-')[0] == "Err")
            {
                modele.ErrorAvt = resultCtrl.Split('-')[1];
                modele.AvenantEchPossible = false;
                modele.DatesFin = new List<ParametreDto>();
            }
            else
            {
                modele.AvenantEchPossible = resultCtrl.Split('-')[0] == "O";
                if (!modele.AvenantEchPossible)
                {
                    modele.AvenantEch = false;
                    modele.DatesFin = new List<ParametreDto>();
                }
                else
                {
                    var listDates = resultCtrl.Split('-')[1];
                    modele.DatesFin = GetDatesFin(listDates);
                    if (modele.DatesFin.Count == 0 && !string.IsNullOrEmpty(listDates))
                        modele.ErrorAvt = "Problème lors de la récupération des échéances";
                }
            }
            modele.FinGarantieHisto = dateFinHisto;
            modele.ProchaineEchHisto = prochEchHisto;

            return modele;
        }

        public static AvenantRemiseEnVigueurDto GetInfoAvenantRemiseEnVig(string codeOffre, string version, string type, Int16 codeAvn, string typeAvt, string modeAvt, string modeNavig)
        {
            var modeleInfo = new AvenantRemiseEnVigueurDto();
            var enumModeNavig = modeNavig.ParseCode<ModeConsultation>();

            if (modeAvt.ToUpperInvariant() == AccessMode.CREATE.ToString())
            {
                var initParameter = new RemiseEnVigueurParams() { Result = string.Empty, CodeContrat = codeOffre, Version = Convert.ToInt32(version), Type = type };
                var resultRMV = RemiseEnVigueurRepository.CallKDA196(initParameter);
                if (int.TryParse(resultRMV.Result, out int i) && i == 1)
                {
                    modeleInfo = new AvenantRemiseEnVigueurDto
                    {
                        PrimeReglee = resultRMV.PrimeReglee,
                        PrimeReglementDate = AlbConvert.ConvertIntToDate(resultRMV.PrimeReglementDate),
                        DateSuspDeb = AlbConvert.ConvertIntToDate(resultRMV.SuspDebDate),
                        HeureSuspDeb = AlbConvert.ConvertIntToTimeMinute(resultRMV.SuspDebH),
                        DateSuspFin = AlbConvert.ConvertIntToDate(resultRMV.SuspFinDate),
                        HeureSuspFin = AlbConvert.ConvertIntToTimeMinute(resultRMV.SuspFinH),
                        DateResil = AlbConvert.ConvertIntToDate(resultRMV.ResileDebDate),
                        HeureResil = AlbConvert.ConvertIntToTimeMinute(resultRMV.ResileDebH)
                    };
                }
                else if (resultRMV.Result.Trim().ToUpper() == AlbConstantesMetiers.TYPE_CONTRAT)
                {
                    throw new AlbFoncException("Aucun enregistrement trouvé dans YPOBASE", true, true);
                }
                else
                {
                    modeleInfo.ErrorAvt = "Aucune période de suspension trouvée";
                }

            }
            var modele = new AvenantRemiseEnVigueurDto();
            List<EacParameter> param = new List<EacParameter> {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength) {
                    Value = codeOffre.PadLeft(9, ' ')
                },
                new EacParameter("version", DbType.Int32) {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                },
                new EacParameter("type", DbType.AnsiStringFixedLength) {
                    Value = type
                }
            };

            string error = string.Empty;
            long numInterne = 0;
            if (enumModeNavig == ModeConsultation.Historique)
            {
                numInterne = codeAvn;
            }
            else
            {
                var sqlInterne = @"SELECT PBAVN INT32RETURNCOL FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";
                var resultInterne = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlInterne, param);
                if (resultInterne != null && resultInterne.Any())
                {
                    numInterne = resultInterne.FirstOrDefault().Int32ReturnCol;
                }
            }

            string sql = string.Format(@"
SELECT PBTTR TYPEAVT, PARTYP.TPLIL LIBELLEAVT, PBAVN NUMINTERNEAVT, 
PBAVA *10000 + PBAVM * 100 + PBAVJ DATEEFFET, KAAAVH HEUREEFFET, 
PBAVK NUMAVT, KADDESI DESCRIPTIONAVT, KAJOBSV OBSERVATIONSAVT, 
KAAARTYG TYPEGESTION, KAAPKRD DATERGLT, KAASUDD DATEDEBSUSP, KAASUDH HEUREDEBSUSP, KAASUFD DATEFINSUSP, KAASUFH HEUREFINSUSP, 
PBFEA *10000 + PBFEM * 100 + PBFEJ DATE_FIN_EFFET, PBFEH HEURE_FIN_EFFET 
FROM {0} 
LEFT JOIN {1} ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP {6} 
LEFT JOIN {2} ON KAAAVDS = KADCHR 
LEFT JOIN {3} ON KAAOBSV = KAJCHR 
{4} 
{5} 
WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type {7} ",
                /*0*/CommonRepository.GetPrefixeHisto(enumModeNavig, "YPOBASE"),
                /*1*/CommonRepository.GetPrefixeHisto(enumModeNavig, "KPENT"),
                /*2*/CommonRepository.GetPrefixeHisto(enumModeNavig, "KPDESI"),
                /*3*/CommonRepository.GetPrefixeHisto(enumModeNavig, "KPOBSV"),
                /*4*/CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", "PARTYP", " AND PARTYP.TCOD = PBTTR"),
                /*5*/CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBRSC", "PARMOTIF", " AND PARMOTIF.TCOD = PBAVC"),
                /*6*/enumModeNavig == ModeConsultation.Historique ? "AND PBAVN = KAAAVN" : string.Empty,
                /*7*/enumModeNavig == ModeConsultation.Historique ? "AND PBAVN = :numAvn" : string.Empty);

            if (enumModeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("numAvn", DbType.Int64) { Value = numInterne });
            }

            var result = DbBase.Settings.ExecuteList<AvenantRemiseEnVigueurDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                modele = result.FirstOrDefault();
                modele.TypeAvt = typeAvt;
                modele.LibelleAvt = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "PRODU", "PBTTR", typeAvt).Libelle;

                if (modeAvt.ToUpperInvariant() == AccessMode.CREATE.ToString())
                {
                    if (modele.NumInterneAvt == 0)
                    {
                        modele.NumInterneAvt = numInterne + 1;
                    }
                    else
                    {
                        modele.NumInterneAvt = modele.NumInterneAvt + 1;
                    }
                    if (modele.NumAvt == 0)
                    {
                        modele.NumAvt = numInterne + 1;
                    }
                    else
                    {
                        modele.NumAvt = modele.NumAvt + 1;
                    }

                    sql = @"
SELECT PBFEA  * 10000 + PBFEM  * 100 + PBFEJ DATERESIL, PBFEH HEURERESIL 
FROM  YPOBASE 
WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";
                }
                else
                {
                    if (enumModeNavig == ModeConsultation.Historique)
                    {
                        var p = param.First(x => x.ParameterName == "numAvn");
                        p.ParameterName = "avn";
                        p.Value = codeAvn - 1;
                    }
                    else
                    {
                        param.Add(new EacParameter("avn", DbType.Int64) { Value = numInterne - 1 });
                    }
                    sql = @"
SELECT PBFEA  * 10000 + PBFEM  * 100 + PBFEJ DATERESIL, PBFEH HEURERESIL 
FROM  YHPBASE 
WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type AND PBAVN = :avn ";
                }

                var list = DbBase.Settings.ExecuteList<AvenantRemiseEnVigueurDto>(CommandType.Text, sql, param);

                if (list != null && list.Any())
                {
                    var item = list.FirstOrDefault();
                    modele.DateResil = AlbConvert.ConvertIntToDate(Convert.ToInt32(item.DateResiltInt));
                    modele.HeureResil = AlbConvert.ConvertIntToTimeMinute(Convert.ToInt32(item.HeureResilInt));

                    modele.DateSuspDeb = AlbConvert.ConvertIntToDate(Convert.ToInt32(item.DateDSusp));
                    modele.HeureSuspDeb = AlbConvert.ConvertIntToTimeMinute(Convert.ToInt32(item.HeureDSusp));

                    modele.DateSuspFin = AlbConvert.ConvertIntToDate(Convert.ToInt32(item.DateFSusp));
                    modele.HeureSuspFin = AlbConvert.ConvertIntToTimeMinute(Convert.ToInt32(item.HeureFSusp));
                }

                if (modeAvt.ToUpperInvariant() == AccessMode.CREATE.ToString())
                {
                    modele.PrimeReglee = modeleInfo.PrimeReglee;
                    modele.PrimeReglementDate = modeleInfo.PrimeReglementDate;
                    modele.DateSuspDeb = modeleInfo.DateSuspDeb;
                    modele.HeureSuspDeb = modeleInfo.HeureSuspDeb;
                    modele.DateSuspFin = modeleInfo.DateSuspFin;
                    modele.HeureSuspFin = modeleInfo.HeureSuspFin;
                    modele.DateResil = modeleInfo.DateResil;
                    modele.HeureResil = modeleInfo.HeureResil;
                    modele.DateDebNonSinistre = modeleInfo.DateSuspDeb;
                    modele.HeureDebNonSinistre = modeleInfo.HeureSuspDeb;
                    modele.DateRemiseVig = modele.PrimeReglementDate.HasValue ? modele.PrimeReglementDate.Value.AddDays(1) : default(DateTime?);
                    modele.HeureRemiseVig = AlbConvert.ConvertIntToTime(120000);
                    modele.DateFinNonSinistre = modele.DateRemiseVig;
                    modele.HeureFinNonSinistre = modele.HeureRemiseVig;
                    modele.ErrorAvt = modeleInfo.ErrorAvt;
                }

                if (modeAvt.ToUpperInvariant() == AccessMode.UPDATE.ToString())
                {
                    modele.DateRemiseVig = AlbConvert.ConvertIntToDate(Convert.ToInt32(modele.DateRemiseVigInt));
                    modele.HeureRemiseVig = AlbConvert.ConvertIntToTime(modele.HeureRemiseVigInt);
                    modele.PrimeReglementDate = AlbConvert.ConvertIntToDate(Convert.ToInt32(modele.DateRglt));
                    modele.DateSuspDeb = AlbConvert.ConvertIntToDate(Convert.ToInt32(modele.DateDSusp));
                    modele.HeureSuspDeb = AlbConvert.ConvertIntToTime(modele.HeureDSusp);
                    modele.DateSuspFin = AlbConvert.ConvertIntToDate(Convert.ToInt32(modele.DateFSusp));
                    modele.HeureSuspFin = AlbConvert.ConvertIntToTime(modele.HeureFSusp);
                    modele.DateDebNonSinistre = modele.DateSuspDeb;
                    modele.HeureDebNonSinistre = modele.HeureSuspDeb;
                    modele.DateFinNonSinistre = modele.DateRemiseVig;
                    modele.HeureFinNonSinistre = AlbConvert.ConvertIntToTime(modele.HeureRemiseVigInt);
                    modele.DateFinEffet = AlbConvert.ConvertIntToDate(Convert.ToInt32(modele.DFinEffet));
                    modele.HeureFinEffet = AlbConvert.ConvertIntToTimeMinute(modele.HFinEffet);
                    modele.ErrorAvt = modeleInfo.ErrorAvt;
                }

                if (enumModeNavig != ModeConsultation.Historique)
                {
                    sql = @"SELECT (RXMFA * 10000 + RXMFM * 100 + RXMFJ) DATE_FIN_EFFET, RXMFH HEURE_FIN_EFFET
FROM ARESIL
WHERE RXIPB = :codeOffre AND RXALX = :version
ORDER BY (RXCRA * 10000 + RXCRM * 100 + RXCRJ) DESC
FETCH FIRST 1 ROWS ONLY ";
                    param = new List<EacParameter> {
                        new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                        new EacParameter("version", DbType.Int32) { Value = int.TryParse(version, out int v) ? v : default }
                    };

                    var res = DbBase.Settings.ExecuteList<AvenantRemiseEnVigueurDto>(CommandType.Text, sql, param).FirstOrDefault();
                    if (res != null)
                    {
                        modele.DateFinEffet = AlbConvert.ConvertIntToDate(Convert.ToInt32(res.DFinEffet));
                        modele.HeureFinEffet = AlbConvert.ConvertIntToTimeMinute(res.HFinEffet);
                    }
                }
            }
            return modele;
        }

        public static List<ParametreDto> ReloadAvenantResilMotif()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBRSC");
        }

        #region Méthodes privées

        private static List<ParametreDto> GetDatesFin(string listeDates)
        {
            List<ParametreDto> toReturn = new List<ParametreDto>();
            var err = false;

            var tListDate = listeDates.Split(';');
            if (tListDate != null && tListDate.Any())
            {
                tListDate.ToList().ForEach(elm =>
                {
                    if (!string.IsNullOrEmpty(elm))
                    {
                        var dateStr = AlbConvert.ConvertDateToStr(AlbConvert.ConvertIntToDate(Convert.ToInt32(elm)));
                        if (!string.IsNullOrEmpty(dateStr))
                            toReturn.Add(new ParametreDto { Code = dateStr, Libelle = dateStr, Descriptif = dateStr });
                        err = true;
                    }
                });
                if (!err)
                    toReturn = new List<ParametreDto>();
            }

            return toReturn;
        }

        #endregion

        #endregion

        #region Ecran Infos Générales Avenant

        public static ContratDto GetAvenant(string codeContrat, string version, string type, string codeAvenant, ModeConsultation modeNavig)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.PadLeft(9, ' '));
            param[1] = new EacParameter(modeNavig == ModeConsultation.Historique ? "P_VERSIONCONTRAT" : "P_VERSION", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODEAVN", 0)
            {
                Value = !string.IsNullOrEmpty(codeAvenant) ? Convert.ToInt32(codeAvenant) : 0
            };

            var result = DbBase.Settings.ExecuteList<ContratDto>(CommandType.StoredProcedure, modeNavig == ModeConsultation.Historique ? "SP_GETAVENANTHIST" : "SP_GETAVENANT", param).FirstOrDefault();

            param[0] = new EacParameter("CODECONTRAT", DbType.AnsiStringFixedLength)
            {
                Value = codeContrat.PadLeft(9, ' ')
            };
            param[1] = new EacParameter("VERSION", DbType.Int32)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[2] = new EacParameter("TYPE", DbType.AnsiStringFixedLength)
            {
                Value = type
            };
            param[3] = new EacParameter("CODEAVN", DbType.Int32)
            {
                Value = !string.IsNullOrEmpty(codeAvenant) ? Convert.ToInt32(codeAvenant) - 1 : 0
            };

            string sql = @"SELECT PBFEA DATERESILANNNEE, PBFEM DATERESILMOIS, PBFEJ DATERESILJOUR, PBFEH DATERESILHEURE FROM YHPBASE WHERE PBIPB=:CODECONTRAT AND PBALX=:VERSION AND PBTYP=:TYPE AND PBAVN=:CODEAVN";

            var res = DbBase.Settings.ExecuteList<ContratDto>(CommandType.Text, sql, param).FirstOrDefault();

            if (res != null && result != null)
            {
                result.DateResilAnnee = res.DateResilAnnee;
                result.DateResilMois = res.DateResilMois;
                result.DateResilJour = res.DateResilJour;
                result.DateResilHeure = res.DateResilHeure;
                var cabinetCourtage = CabinetCourtageRepository.Obtenir(result.CourtierGestionnaire);
                result.Delegation = cabinetCourtage?.Delegation?.Nom;
                result.Inspecteur = cabinetCourtage?.Inspecteur;
            }

            return result;
        }

        public static void EnregistrerAvenant(ContratDto avenant, string user)
        {
            DateTime dateNow = DateTime.Now;

            DbParameter[] param = new DbParameter[69];
            param[0] = new EacParameter("P_CODEOFFRE", avenant.CodeContrat.PadLeft(0, ' '));
            param[1] = new EacParameter("P_VERSION", 0)
            {
                Value = avenant.VersionContrat
            };
            param[2] = new EacParameter("P_TYPE", avenant.Type);
            param[3] = new EacParameter("P_AVENANT", 0)
            {
                Value = avenant.NumAvenant
            };
            param[4] = new EacParameter("P_STOP", avenant.ZoneStop);
            param[5] = new EacParameter("P_DESCRIPTION", avenant.DescriptionAvenant);
            param[6] = new EacParameter("P_DESCRIPTIF", avenant.Descriptif);
            param[7] = new EacParameter("P_MOTCLE1", avenant.CodeMotsClef1);
            param[8] = new EacParameter("P_MOTCLE2", avenant.CodeMotsClef2);
            param[9] = new EacParameter("P_MOTCLE3", avenant.CodeMotsClef3);
            param[10] = new EacParameter("P_OBSERVATIONS", avenant.Observations);
            param[11] = new EacParameter("P_DEVISE", avenant.Devise);
            param[12] = new EacParameter("P_REGIMETAXE", avenant.CodeRegime);
            param[13] = new EacParameter("P_CATNAT", avenant.SoumisCatNat);
            param[14] = new EacParameter("P_ANTECEDENT", avenant.Antecedent);
            param[15] = new EacParameter("P_OBSANTECEDENT", avenant.Description);
            param[16] = new EacParameter("P_PERIODICITE", avenant.PeriodiciteCode);
            param[17] = new EacParameter("P_ECHPRINCIPJOUR", 0)
            {
                Value = avenant.Jour
            };
            param[18] = new EacParameter("P_ECHPRINCIPMOIS", 0)
            {
                Value = avenant.Mois
            };
            param[19] = new EacParameter("P_PROCHECHJOUR", 0)
            {
                Value = avenant.ProchaineEchJour
            };
            param[20] = new EacParameter("P_PROCHECHMOIS", 0)
            {
                Value = avenant.ProchaineEchMois
            };
            param[21] = new EacParameter("P_PROCHECHANNEE", 0)
            {
                Value = avenant.ProchaineEchAnnee
            };
            param[22] = new EacParameter("P_DEBEFFETJOUR", 0)
            {
                Value = avenant.DateEffetJour
            };
            param[23] = new EacParameter("P_DEBEFFETMOIS", 0)
            {
                Value = avenant.DateEffetMois
            };
            param[24] = new EacParameter("P_DEBEFFETANNEE", 0)
            {
                Value = avenant.DateEffetAnnee
            };
            param[25] = new EacParameter("P_DEBEFFETHEURE", 0)
            {
                Value = avenant.DateEffetHeure
            };
            param[26] = new EacParameter("P_FINEFFETJOUR", 0)
            {
                Value = avenant.FinEffetJour
            };
            param[27] = new EacParameter("P_FINEFFETMOIS", 0)
            {
                Value = avenant.FinEffetMois
            };
            param[28] = new EacParameter("P_FINEFFETANNEE", 0)
            {
                Value = avenant.FinEffetAnnee
            };
            param[29] = new EacParameter("P_FINEFFETHEURE", 0)
            {
                Value = avenant.FinEffetHeure
            };
            param[30] = new EacParameter("P_DUREE", 0)
            {
                Value = avenant.DureeGarantie
            };
            param[31] = new EacParameter("P_DUREEUNITE", avenant.UniteDeTemps);
            param[32] = new EacParameter("P_ACCORDJOUR", 0)
            {
                Value = avenant.DateAccordJour
            };
            param[33] = new EacParameter("P_ACCORDMOIS", 0)
            {
                Value = avenant.DateAccordMois
            };
            param[34] = new EacParameter("P_ACCORDANNEE", 0)
            {
                Value = avenant.DateAccordAnnee
            };
            param[35] = new EacParameter("P_NBMOISRESIL", 0)
            {
                Value = avenant.Preavis
            };
            param[36] = new EacParameter("P_INDICEREF", avenant.IndiceReference);
            param[37] = new EacParameter("P_VALEUR", 0)
            {
                Value = avenant.Valeur
            };
            param[38] = new EacParameter("P_APERITEUR", string.IsNullOrEmpty(avenant.AperiteurCode) ? string.Empty : avenant.AperiteurCode);
            param[39] = new EacParameter("P_NATURECONTRAT", avenant.NatureContrat);
            param[40] = new EacParameter("P_PARTALBINGIA", 0)
            {
                Value = avenant.PartAlbingia
            };
            param[41] = new EacParameter("P_FRAISAPE", 0)
            {
                Value = avenant.FraisAperition
            };
            param[42] = new EacParameter("P_COUVERTURE", avenant.Couverture);
            param[43] = new EacParameter("P_INTERCALAIRE", avenant.IntercalaireExiste);
            param[44] = new EacParameter("P_SOUSCRIPTEUR", avenant.SouscripteurCode);
            param[45] = new EacParameter("P_GESTIONNAIRE", avenant.GestionnaireCode);
            param[46] = new EacParameter("P_DEBPERIODJOUR", 0)
            {
                Value = avenant.DateDebutDernierePeriodeJour
            };
            param[47] = new EacParameter("P_DEBPERIODMOIS", 0)
            {
                Value = avenant.DateDebutDernierePeriodeMois
            };
            param[48] = new EacParameter("P_DEBPERIODANNEE", 0)
            {
                Value = avenant.DateDebutDernierePeriodeAnnee
            };
            param[49] = new EacParameter("P_FINPERIODJOUR", 0)
            {
                Value = avenant.DateFinDernierePeriodeJour
            };
            param[50] = new EacParameter("P_FINPERIODMOIS", 0)
            {
                Value = avenant.DateFinDernierePeriodeMois
            };
            param[51] = new EacParameter("P_FINPERIODANNEE", 0)
            {
                Value = avenant.DateFinDernierePeriodeAnnee
            };
            param[52] = new EacParameter("P_DATESTAT", 0)
            {
                Value = avenant.DateStatistique
            };
            param[53] = new EacParameter("P_USER", user);
            param[54] = new EacParameter("P_DATENOWJOUR", 0)
            {
                Value = dateNow.Day
            };
            param[55] = new EacParameter("P_DATENOWMOIS", 0)
            {
                Value = dateNow.Month
            };
            param[56] = new EacParameter("P_DATENOWANNEE", 0)
            {
                Value = dateNow.Year
            };
            param[57] = new EacParameter("P_HEURENOW", 0)
            {
                Value = AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(dateNow))
            };
            param[58] = new EacParameter("P_DATEAVTJOUR", 0)
            {
                Value = avenant.DateEffetAvenant.HasValue ? avenant.DateEffetAvenant.Value.Day : 0
            };
            param[59] = new EacParameter("P_DATEAVTMOIS", 0)
            {
                Value = avenant.DateEffetAvenant.HasValue ? avenant.DateEffetAvenant.Value.Month : 0
            };
            param[60] = new EacParameter("P_DATEAVTANNEE", 0)
            {
                Value = avenant.DateEffetAvenant.HasValue ? avenant.DateEffetAvenant.Value.Year : 0
            };
            param[61] = new EacParameter("P_HEUREAVT", 0)
            {
                Value = AlbConvert.ConvertTimeToInt(avenant.HeureEffetAvenant)
            };

            param[62] = new EacParameter("P_PARTAPERITEUR", 0)
            {
                Value = avenant.PartAperiteur
            };
            param[63] = new EacParameter("P_IDINTERLOAPERITEUR", 0)
            {
                Value = avenant.IdInterlocuteurAperiteur
            };
            param[64] = new EacParameter("P_REFAPERITEUR", string.IsNullOrEmpty(avenant.ReferenceAperiteur) ? string.Empty : avenant.ReferenceAperiteur);
            param[65] = new EacParameter("P_FRAISACCAPERITEUR", 0)
            {
                Value = avenant.FraisAccAperiteur
            };
            param[66] = new EacParameter("P_COMMAPERITEUR", 0)
            {
                Value = avenant.CommissionAperiteur
            };

            param[67] = new EacParameter("P_ACTEGESTION", avenant.NumAvenant == 0 ? AlbConstantesMetiers.TRAITEMENT_AFFNV : string.Empty);
            param[68] = new EacParameter("P_LTA", avenant.LTA);
            CommonRepository.DisableLinkNavigation(avenant, new List<string>() { "COT", "FIN" }, true);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVECONTRAT", param);
        }

        public static DtoCommon GetProchaineEcheanceHisto(string codeOffre, string version, string type, string codeAvenant)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength)
            {
                Value = codeOffre.PadLeft(9, ' ')
            };
            param[1] = new EacParameter("version", DbType.Int32)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength)
            {
                Value = type
            };
            param[3] = new EacParameter("codeAvn", DbType.Int32)
            {
                Value = !string.IsNullOrEmpty(codeAvenant) ? Convert.ToInt32(codeAvenant) - 1 : 0
            };

            string sql = @"SELECT PBPER STRRETURNCOL, JDPEA * 10000 + JDPEM * 100 + JDPEJ INT64RETURNCOL,
                                JDDPA * 10000 + JDDPM * 100 + JDDPJ DATEDEBRETURNCOL ,
                                PBETA ETAT, PBSIT SITUATION
                            FROM YHPBASE 
                                INNER JOIN YHRTENT ON PBIPB = JDIPB AND PBALX = JDALX AND PBAVN = JDAVN
                            WHERE PBIPB = :codeContrat AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvn";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                return result.FirstOrDefault();
            }
            return null;
        }

        #endregion

    }
}
