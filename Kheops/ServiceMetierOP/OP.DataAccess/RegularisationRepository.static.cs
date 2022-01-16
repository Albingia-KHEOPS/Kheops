using Albingia.Kheops.OP.Domain.Regularisation;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess {
    public partial class RegularisationRepository {
        /// <summary>
        /// Copie les infos JEPB* de la 1ére ligne de YPRTRSQ vers YPRTENT 
        /// </summary>
        /// <param name="codeOffre">Code offre</param>
        /// <param name="version">Numéro de version</param>
        /// <param name="codeAvn">Code avenant</param>
        public static void ReportDataRegulFromRsqToEnt(string codeOffre, string version, string codeAvn) {
            var sql = @"SELECT COUNT(*) NBLIGN FROM YPRTRSQ WHERE JEIPB = :P_CodeOffre AND JEALX = :P_Version AND JEPBN <> '' AND JEPBN <> 'N' AND JEAVA = :P_CodeAvn";
            var param = new List<EacParameter>();

            param.Add(new EacParameter("P_CodeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') });
            param.Add(new EacParameter("P_Version", DbType.Int32) { Value = string.IsNullOrEmpty(version) ? 0 : Convert.ToInt32(version) });
            param.Add(new EacParameter("P_CodeAvn", DbType.Int32) { Value = string.IsNullOrEmpty(codeAvn) ? 0 : Convert.ToInt32(codeAvn) });

            if (CommonRepository.ExistRowParam(sql, param)) {
                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_REPORT_REGULE_YPRTRSQ_YPRTENT", param);
            }
        }

        /// <summary>
        /// Perform the dividing of the computed amount by KDA312
        /// </summary>
        /// <param name="idRegul">Regularization ID</param>
        public static void PerformDividingAmount_KPGRGU_KPRGUR(long idRegul) {
            List<EacParameter> param = new List<EacParameter>();

            param.Add(new EacParameter("P_REGULEID", DbType.AnsiStringFixedLength) { Value = idRegul.ToString() });

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SET_REGULE_AMOUNTS", param);
        }

        /// <summary>
        /// Report the computed amount by KDA312 from KPRGU to KPRGUR when ReguleMode was 'E'
        /// </summary>
        /// <param name="idRegul">Regularization ID</param>
        public static void ReportAmount_KPGRGU_KPRGUR(long idRegul) {
            List<EacParameter> param = new List<EacParameter>();

            param.Add(new EacParameter("P_REGULEID", DbType.AnsiStringFixedLength) { Value = idRegul.ToString() });

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_REPORT_REGULE_AMOUNTS", param);
        }

        public static Int64 GetNumRegule(string codeAffaire, string version, string type, string codeAvn) {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeAffaire", DbType.AnsiStringFixedLength);
            param[0].Value = codeAffaire.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeAvn", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
            string sql = @"SELECT KHWID ID FROM KPRGU WHERE KHWIPB = :codeAffaire AND KHWALX = :version AND KHWTYP = :type AND KHWAVN = :codeAvn";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result.Any()) {
                return result.FirstOrDefault().Id;
            }
            return 0;
        }

        /// <summary>
        /// Assigns zero to every value of Risques and Garanties for a given Regularisation
        /// </summary>
        /// <param name="rgId">Regularisation identifier</param>
        /// <param name="rsqId">Risque number</param>
        public static void EraseFigures(long rgId, int rsqId) {
            var parameters = new EacParameter[] { new EacParameter("rgId", DbType.Int64) { Value = rgId }, new EacParameter("rsqId", DbType.Int32) { Value = rsqId } };

            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                "UPDATE KPRGUG SET KHXMHC = 0, KHXMHT = 0, KHXCNH = 0, KHXMTT = 0, KHXTTT = 0 WHERE KHXKHWID = :rgId AND KHXRSQ = :rsqId",
                parameters
                );

            // ARA
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                "UPDATE KPRGUR SET KILEMD = 0, KILSRIM = 0, KILCOT = 0, KILSCHG = 0, KILSIDF = 0, KILSPRE = 0, KILSPRO = 0, KILSREC = 0 WHERE KILKHWID = :rgId AND KILRSQ = :rsqId",
                parameters
                );
            SpreadSumMontantEmis(rgId, rsqId, true);
        }

        public static LigneRegularisationDto GetLastRegularisation(string codeContrat, string version, string type, string codeAvn) {
            var list = GetListeRegularisation(codeContrat, version, type);

            if (list != null && list.Any()) {
                return list.OrderByDescending(reg => reg.NumRegule).FirstOrDefault(reg => reg.CodeAvn.ToString().Equals(codeAvn));
            }

            return null;
        }

        public static bool ExistRegul(string numeroOffre, string version, string type, string numAvn) {
            var param = new List<EacParameter>
            {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength) {Value = numeroOffre.PadLeft(9, ' ')},
                new EacParameter("version", DbType.Int32) {Value = version},
                new EacParameter("type", DbType.AnsiStringFixedLength) {Value = type},
                new EacParameter("numAvn", DbType.Int32) {Value = numAvn},
            };

            string sql = @"SELECT COUNT(*) NBLIGN FROM KPRGU WHERE KHWIPB= :codeOffre AND KHWALX= :version
                                 AND KHWTYP= :type AND (KHWETA='A' OR KHWETA = 'N' ) AND KHWAVN= :numAvn";

            return CommonRepository.ExistRowParam(sql, param);
        }

        public static LigneRegularisationDto GetRegularisationByID(string codeContrat, string version, string type, long rgId) {
            var list = GetListeRegularisation(codeContrat, version, type);

            if (list != null && list.Any()) {
                return list.FirstOrDefault(reg => reg.NumRegule == rgId);
            }

            return null;
        }

        public static List<LigneRegularisationDto> GetListeRegularisation(string codeContrat, string version, string type) {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sql = string.Format(@"SELECT KHWID NUMREG, KHWAVN CODEAVN, KHWTTR CODETRAITEMENT, TRAITEMENT.TPLIB LIBTRAITEMENT, 
                        KHWDEBP DATEDEB, KHWFINP DATEFIN, KHWETA CODEETAT, KHWSIT CODESITUATION, SITUATION.TPLIB LIBSITUATION, 
                        KHWSTD DATESIT, KHWSTH HEURESIT, KHWSTU USERSIT,
	                    IFNULL(PKIPK, 0) NUMQUITT, IFNULL(PKSIT, '') CODEETATQUITT, IFNULL(ETATQUITT.TPLIB, '') LIBETATQUITT,PKAVI AVIS,
                        KHWMRG REGULMODE, KHWSUAV REGULTYPE, KHWACC REGULNIV, KHWRGAV REGULAVN
                    FROM KPRGU
	                    LEFT JOIN YPRIMES ON PKIPB = KHWIPB AND PKALX = KHWALX AND PKIPK = KHWIPK
	                    {0}
	                    {1}
	                    {2}
                    WHERE KHWIPB = :codeContrat AND KHWALX = :version AND KHWTYP = :type
                    ORDER BY CODEAVN ASC",
                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", "TRAITEMENT", " AND TRAITEMENT.TCOD = KHWTTR"),
                    CommonRepository.BuildJoinYYYYPAR("LEFT", "KHEOP", "RGUST", "SITUATION", " AND SITUATION.TCOD = KHWSIT"),
                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PKSIT", "ETATQUITT", " AND ETATQUITT.TCOD = PKSIT"));

            return DbBase.Settings.ExecuteList<LigneRegularisationDto>(CommandType.Text, sql, param);
        }

        public static void SetReportCharge(RegularisationContext context, RegularisationComputeData figures) {
            var sql = @"SELECT COUNT(*) NBLIGN FROM KPRGUC WHERE KIMTYP = :type AND KIMIPB = :codeContrat AND KIMALX = :version AND KIMSIT = 'A'";

            List<EacParameter> param = new List<EacParameter>() {
                new EacParameter("type", DbType.AnsiStringFixedLength) { Value = context.IdContrat.Type },
                new EacParameter("codeContrat", DbType.AnsiStringFixedLength) { Value = context.IdContrat.CodeOffre.PadLeft(9, ' ') },
                new EacParameter("version", DbType.Int32) { Value = context.IdContrat.Version }
            };
            var existReportCharge = CommonRepository.ExistRowParam(sql, param);
            if (!existReportCharge) {
                sql = @"INSERT INTO KPRGUC ( KIMID, KIMSTD, KIMSTH, KIMSTU, KIMMAJU, KIMMAJD, KIMSCHG, KIMTYP, KIMIPB, KIMALX, KIMCRU, KIMCRD, KIMSIT )
                                    VALUES ( :id, :date,  :heure, :user, :user2, :date2, :report, :type, :codeOffre, :version, :user3, :date3, 'A' )";

                var dateNow = DateTime.Now;

                param = new List<EacParameter>() {
                    new EacParameter("id", DbType.Int32) { Value =  CommonRepository.GenererNumeroChrono("KIMID")},
                    new EacParameter("date", DbType.Int32) { Value =  AlbConvert.ConvertDateToInt(dateNow)},
                    new EacParameter("heure", DbType.Int32) { Value =  AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow))},
                    new EacParameter("user", DbType.AnsiStringFixedLength) { Value = context.User },
                    new EacParameter("user2", DbType.AnsiStringFixedLength) { Value = context.User },
                    new EacParameter("date2", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(dateNow) },
                    new EacParameter("report", DbType.Decimal) { Value = Convert.ToDecimal(figures.ReportChargesNouveau) },
                    new EacParameter("type", DbType.AnsiStringFixedLength) { Value = context.IdContrat.Type },
                    new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = context.IdContrat.CodeOffre.PadLeft(9,' ')},
                    new EacParameter("version", DbType.Int32) { Value = context.IdContrat.Version },
                    new EacParameter("user3", DbType.AnsiStringFixedLength) { Value = context.User },
                    new EacParameter("date3", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(dateNow) },
                };

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            else {
                sql = @"UPDATE KPRGUC SET KIMSTD = :date, KIMSTH = :heure, KIMSTU = :user, KIMMAJU = :user2, KIMMAJD = :date2,
                        KIMSCHG = :report WHERE KIMTYP = :type AND KIMIPB = :codeContrat AND KIMALX = :version AND KIMSIT = 'A'";

                var dateNow = DateTime.Now;

                param = new List<EacParameter>() {
                    new EacParameter("date", DbType.Int32) { Value =  AlbConvert.ConvertDateToInt(dateNow)},
                    new EacParameter("heure", DbType.Int32) { Value =  AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow))},
                    new EacParameter("user", DbType.AnsiStringFixedLength) { Value = context.User },
                    new EacParameter("user2", DbType.AnsiStringFixedLength) { Value = context.User },
                    new EacParameter("date2", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(dateNow) },
                    new EacParameter("report", DbType.Decimal) { Value = figures.ReportChargesNouveau },
                    new EacParameter("type", DbType.AnsiStringFixedLength) { Value = context.IdContrat.Type },
                    new EacParameter("codeContrat", DbType.AnsiStringFixedLength) { Value = context.IdContrat.CodeOffre.PadLeft(9, ' ') },
                    new EacParameter("version", DbType.Int32) { Value = context.IdContrat.Version }
                };

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }


        }

        public static Nullable<decimal> GetMontantTotalGarantieEmis(long rgId, long rsqId = 0) {
            decimal mntGarantieEmis = 0;
            List<EacParameter> param = new List<EacParameter>();
            param.Add(new EacParameter("p0", DbType.Int64) { Value = rgId });

            if (rsqId > 0)
                param.Add(new EacParameter("p1", DbType.Int32) { Value = rsqId });

            //récupération de toutes les garanties du scope (entête ou risque)
            var sql = string.Format(@"SELECT KHXIPB CODEAFFAIRE, KHXALX VERSION, KHXTYP TYPEAFFAIRE, KHXRSQ IDRISQUE, KHXFOR CODEFOR, KHXGARAN CODEGARAN, KHWDEBP DATEDEBGAR, KHWFINP DATEFINGAR FROM KPRGUG 
                                    INNER JOIN KPRGU ON KHXKHWID = KHWID
                                WHERE KHXKHWID = :p0 {0}",
                rsqId > 0 ? " AND KHXRSQ = :p1" : string.Empty);

            var result = DbBase.Settings.ExecuteList<RegularisationGarantieDto>(CommandType.Text, sql, param);

            if (result != null && result.Any()) {
                foreach (var item in result) {
                    //appel au PGM 400 KDA310CL pour récupérer le montant déjà émis de chaque garantie
                    mntGarantieEmis += Convert.ToDecimal(CommonRepository.GetMontantGarantieEmis(
                       item.CodeAffaire,
                       item.Version.ToString(),
                       item.Type,
                       item.IdRisque.ToString(),
                       item.IdFormule.ToString(),
                       item.CodeGarantie,
                       (int)item.DateDebGar,
                       (int)item.DateFinGar).Split('_')[0]);
                }

                return mntGarantieEmis;
            }

            return null;
        }

        public static void SpreadSumMontantEmis(long rgId, long rsqId = 0, bool isCancel = false) {
            //récupération du montant de cotisations déjà émis
            var sql = string.Format("SELECT COUNT(*) NBLIGN FROM KPRGUG WHERE KHXKHWID = :p0 {0} AND KHXSIT = ''",
                rsqId > 0 ? " AND KHXRSQ = :p1" : string.Empty);

            List<EacParameter> param = new List<EacParameter>();
            param.Add(new EacParameter("p0", DbType.Int64) { Value = rgId });

            if (rsqId > 0)
                param.Add(new EacParameter("p1", DbType.Int32) { Value = rsqId });
            if (!isCancel) {
                if (!CommonRepository.ExistRowParam(sql, param))
                    return;
            }

            Nullable<decimal> mntGarantieEmis = GetMontantTotalGarantieEmis(rgId, rsqId);

            if (mntGarantieEmis.HasValue) {
                param = new List<EacParameter>();
                param.Add(new EacParameter("p0", DbType.Decimal) { Value = mntGarantieEmis.Value });
                param.Add(new EacParameter("p1", DbType.Int64) { Value = rgId });

                if (rsqId > 0) {
                    //mise à jour du montant de cotisations déjà émises
                    param.Add(new EacParameter("p2", DbType.Int32) { Value = rsqId });
                    sql = @"UPDATE KPRGUR SET KILEMD = :p0 WHERE KILKHWID = :p1 AND KILRSQ = :p2";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                    //mise à jour de la situation des garanties
                    param.RemoveAt(0);
                    sql = @"UPDATE KPRGUG SET KHXSIT = 'N' WHERE KHXKHWID = :p1 AND KHXRSQ = :p2";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                }
                else {
                    //mise à jour du montant de cotisations déjà émises
                    sql = "UPDATE KPRGU SET KHWEMD = :p0 WHERE KHWID = :p1";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                    //mise à jour de la situation des garanties
                    param.RemoveAt(0);
                    sql = @"UPDATE KPRGUG SET KHXSIT = 'N' WHERE KHXKHWID = :p1";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                }
            }
        }

        public static long GetIdRsqRegularisation(long rgId, int rsqId) {
            return DbBase.Settings.ExecuteListGetFirst<DtoCommon>(CommandType.Text,
                "SELECT KILID ID FROM KPRGUR WHERE KILKHWID = :p1 AND KILRSQ = :p2",
                new EacParameter[]
                {
                    new EacParameter("p1", DbType.Int64) { Value = rgId },
                    new EacParameter("p2", DbType.Int64) { Value = rsqId}
                }).Id;
        }

        public static void PrepareComputeContrat(long rgId, RegularisationComputeData figures) {
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                "UPDATE KPRGU SET KHWEMD = :p1, KHWSCHG = :p2, KHWSRIM = :p3, KHWCOT = :p4, KHWMHC = :p5, KHWPBTR = :P6, KHWSPC  = :P7, KHWASV = :P8,  KHWBRNC= :P9 WHERE KHWID = :rgId",
                new EacParameter[]
                {
                    new EacParameter("p1", DbType.Decimal) { Value = (figures.ReguleMode == RegularisationMode.BNS && figures.IsAnticipee)? Convert.ToDecimal(figures.TauxAppel) * figures.CotisationPeriode / 100 : figures.CotisationPeriode },
                    new EacParameter("p2", DbType.Decimal) { Value = figures.ChargementSinistres },
                    new EacParameter("p3", DbType.Decimal) { Value = figures.MontantRistourneAnticipee },
                    new EacParameter("p4", DbType.Decimal) { Value = figures.CotisationsRetenues },
                    new EacParameter("p5", DbType.Decimal) { Value = figures.ReguleMode==RegularisationMode.PB ? figures.MontantComptant : figures.MontantCalcule },
                    new EacParameter("p6", DbType.Decimal) { Value = figures.TauxAppelRetenu == 100? 0 : figures.TauxAppelRetenu },
                    new EacParameter("p7", DbType.Decimal) { Value = figures.SeuilSPRetenu },
                    new EacParameter("p8", DbType.Decimal) { Value = figures.Assiette },
                    new EacParameter("p9", DbType.Decimal) { Value = figures.PrimeMaxi },

                    new EacParameter("rgId", DbType.Int64) { Value = rgId }
                });
        }

        public static void PrepareComputeRisque(long rgId, long rsqId, RegularisationComputeData figures) {
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                @"UPDATE KPRGUR SET KILEMD = :p1, KILSCHG = :p2, KILSRIM = :p3, KILCOT = :p4, KILMHC = :p5, KILPBTR = :p6, KILSPC  = :P7, KILASV = :P8,  KILBRNC= :P9 
                WHERE KILRSQ = :codeRsq AND  KILKHWID = :rgId",
                new EacParameter[]
                {
                    new EacParameter("p1", DbType.Decimal) { Value = (figures.ReguleMode == RegularisationMode.BNS && figures.IsAnticipee)? Convert.ToDecimal(figures.TauxAppel) * figures.CotisationPeriode / 100 : figures.CotisationPeriode },
                    new EacParameter("p2", DbType.Decimal) { Value = figures.ChargementSinistres },
                    new EacParameter("p2", DbType.Decimal) { Value = figures.MontantRistourneAnticipee },
                    new EacParameter("p4", DbType.Decimal) { Value = figures.CotisationsRetenues },
                    new EacParameter("p5", DbType.Decimal) { Value = figures.MontantCalcule },
                    new EacParameter("p6", DbType.Decimal) { Value = figures.TauxAppelRetenu == 100? 0 : figures.TauxAppelRetenu },
                    new EacParameter("p7", DbType.Decimal) { Value = figures.SeuilSPRetenu },
                    new EacParameter("p8", DbType.Decimal) { Value = figures.Assiette },
                    new EacParameter("p9", DbType.Decimal) { Value = figures.PrimeMaxi },

                    new EacParameter("codeRsq", DbType.Int64) { Value = rsqId },
                    new EacParameter("rgId", DbType.Int64) { Value = rgId }
                });
        }

        public static void PrepareComputeContratTR(long rgId, RegularisationComputeData figures) {
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                @"UPDATE KPRGU SET KHWEMD = :p1, KHWSCHG = :p2, KHWSRIM = :p3, KHWCOT = :p4, KHWMHC = :p5, KHWSIDF = :p6,
                  KHWSREC = :p7, KHWSPRO = :p8, KHWSPRE = :p9, KHWSREP = :p10 WHERE KHWID = :p11",
                new EacParameter[]
                {
                    new EacParameter("p1", DbType.Decimal) { Value = figures.CotisationPeriode },
                    new EacParameter("p2", DbType.Decimal) { Value = figures.ChargementSinistres },
                    new EacParameter("p3", DbType.Decimal) { Value = figures.MontantRistourneAnticipee },
                    new EacParameter("p4", DbType.Decimal) { Value = figures.CotisationsRetenues },
                    new EacParameter("p5", DbType.Decimal) { Value = figures.MontantCalcule },
                    new EacParameter("p6", DbType.Decimal) { Value = figures.IndemnitesFrais },
                    new EacParameter("p7", DbType.Decimal) { Value = figures.Recours },
                    new EacParameter("p8", DbType.Decimal) { Value = figures.Provisions },
                    new EacParameter("p9", DbType.Decimal) { Value = figures.Previsions },
                    new EacParameter("p10", DbType.Decimal) { Value = figures.ReportChargesRetenu },
                    new EacParameter("p11", DbType.Int64) { Value = rgId }
                });
        }

        public static void PrepareComputeRisqueTR(long rgId, long rsqId, RegularisationComputeData figures) {
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                @"UPDATE KPRGUR SET KILEMD = :p1, KILSCHG = :p2, KILSRIM = :p3, KILCOT = :p4, KILMHC = :p5, KILSIDF = :p6,
                  KILSREC = :p7, KILSPRO = :p8, KILSPRE = :p9, KILSREP = :p10 WHERE KILRSQ = :codeRsq AND  KILKHWID = :rgId",
                new EacParameter[]
                {
                    new EacParameter("p1", DbType.Decimal) { Value = figures.CotisationPeriode },
                    new EacParameter("p2", DbType.Decimal) { Value = figures.ChargementSinistres },
                    new EacParameter("p2", DbType.Decimal) { Value = figures.MontantRistourneAnticipee },
                    new EacParameter("p4", DbType.Decimal) { Value = figures.CotisationsRetenues },
                    new EacParameter("p5", DbType.Decimal) { Value = figures.MontantCalcule },
                    new EacParameter("p6", DbType.Decimal) { Value = figures.IndemnitesFrais },
                    new EacParameter("p7", DbType.Decimal) { Value = figures.Recours },
                    new EacParameter("p8", DbType.Decimal) { Value = figures.Provisions },
                    new EacParameter("p9", DbType.Decimal) { Value = figures.Previsions },
                    new EacParameter("p10", DbType.Decimal) { Value = figures.ReportChargesRetenu },
                    new EacParameter("codeRsq", DbType.Int64) { Value = rsqId },
                    new EacParameter("rgId", DbType.Int64) { Value = rgId }
                });
        }

        public static void UpdateReportChargeTR(string codeOffre, string version, string type, string reguleId, string numPrimeRegule, string user) {
            //Passage à X de l'ancienne valeur stockée de report charge sur KPRGUC
            var sql = @"UPDATE KPRGUC SET KIMSIT = 'X', KIMSTD = :date, KIMSTH = :heure, KIMSTU = :user
                        WHERE KIMSIT = 'V' AND KIMIPB = :codeOffre AND KIMTYP = :type AND KIMALX = :version";

            var dateNow = DateTime.Now;

            var paramUpdate = new List<EacParameter>
            {
                new EacParameter("date", DbType.Int32) { Value =  AlbConvert.ConvertDateToInt(dateNow)},
                new EacParameter("heure", DbType.Int32) { Value =  AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow))},
                new EacParameter("user", DbType.AnsiStringFixedLength) { Value = user },
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },
                new EacParameter("type", DbType.AnsiStringFixedLength) { Value =type },
                new EacParameter("version", DbType.AnsiStringFixedLength) { Value = version }
            };

            //Passage à V de la nouvelle valeur stockée de report charge sur KPRGUC
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramUpdate);

            sql = @"UPDATE KPRGUC SET KIMSIT = 'V', KIMSTD = :date, KIMSTH = :heure, KIMSTU = :user
                    WHERE KIMSIT = 'A' AND KIMIPB = :codeOffre AND KIMTYP = :type AND KIMALX = :version";

            paramUpdate = new List<EacParameter>
            {
                new EacParameter("date", DbType.Int32) { Value =  AlbConvert.ConvertDateToInt(dateNow)},
                new EacParameter("heure", DbType.Int32) { Value =  AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow))},
                new EacParameter("user", DbType.AnsiStringFixedLength) { Value = user },
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },
                new EacParameter("type", DbType.AnsiStringFixedLength) { Value =type },
                new EacParameter("version", DbType.AnsiStringFixedLength) { Value = version }
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramUpdate);
        }

        public static void SpreadSumCotisation(long rgId, int rsqId = 0) {
            const string SqlSelectSums = @"
                                            SELECT KILKHWID RGID,
                                                   CAST(SUM(KILEMD) AS DECIMAL(11,2)) EMD, CAST(SUM(KILCOT) AS DECIMAL(11,2)) COT,
                                                   CAST(SUM(KILSCHG) AS DECIMAL(13,2)) SCHG, CAST(SUM(KILSIDF) AS DECIMAL(13,2)) SIDF, CAST(SUM(KILSREC) AS DECIMAL(13,2)) SREC, CAST(SUM(KILSRIM) AS DECIMAL(13,2)) SRIM, 
                                                   CAST(SUM(KILSPRO) AS DECIMAL(13,2)) SPRO, CAST(SUM(KILSPRE) AS DECIMAL(13,2)) SPRE, CAST(SUM(KILMHC) AS DECIMAL(13,2)) MHC,
                                                   CAST(SUM(KILSREP) AS DECIMAL(13,2)) RCH
                                            FROM KPRGUR
                                            WHERE KILKHWID = :rgId
                                            AND (KILSIT = 'V' OR KILRSQ = :rsqId)
                                            GROUP BY KILKHWID";

            var sums = DbBase.Settings.ExecuteListGetFirst<RegularisationComputeDataSum>(
                CommandType.Text,
                SqlSelectSums,
                new EacParameter[]
                {
                    new EacParameter("rgId", DbType.Int64) { Value = rgId },
                    new EacParameter("rsqId", DbType.Int32) { Value = rsqId }
                });

            var paramList = new EacParameter[]
            {
                new EacParameter("SRIM", DbType.Decimal) { Value = sums == null ? 0M : sums.RistourneAnticipee },
                new EacParameter("EMD", DbType.Decimal) { Value = sums == null ? 0M : sums.CotisationPeriode },
                new EacParameter("COT", DbType.Decimal) { Value = sums == null ? 0M : sums.CotisationsRetenues },
                new EacParameter("SCHG", DbType.Decimal) { Value = sums == null ? 0M : sums.ChargementSinistres },
                new EacParameter("SIDF", DbType.Decimal) { Value = sums == null ? 0M : sums.IndemnitesEtFrais },
                new EacParameter("SREC", DbType.Decimal) { Value = sums == null ? 0M : sums.Recours },
                new EacParameter("SPRO", DbType.Decimal) { Value = sums == null ? 0M : sums.Provisions },
                new EacParameter("SPRE", DbType.Decimal) { Value = sums == null ? 0M : sums.Previsions },
                new EacParameter("MHC", DbType.Decimal) { Value = sums == null ? 0M : sums.MontantCalcule },
                new EacParameter("RCH", DbType.Decimal) { Value = sums == null ? 0M : sums.ReportCharges },
                new EacParameter("rgId", DbType.Int64) { Value = rgId }
            };

            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                "UPDATE KPRGU SET KHWSRIM = :SRIM, KHWEMD = :EMD, KHWCOT = :COT, KHWSCHG = :SCHG, KHWSIDF = :SIDF, KHWSREC = :SREC, KHWSPRO = :SPRO, KHWSPRE = :SPRE, KHWMHC = :MHC, KHWSREP = :RCH WHERE KHWID = :rgId",
                paramList);
        }

        public static void SwitchScopeRisque(long rgId) {
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                "UPDATE KPRGU SET KHWACC = 'R' WHERE KHWID = :rgId",
                new EacParameter("rgId", DbType.Int64) { Value = rgId });
        }

        public static LigneRegularisationDto InsertCurrentActeGestionRegule(string codeAffaire, string version, string type, string codeAvn) {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeAffaire", DbType.AnsiStringFixedLength);
            param[0].Value = codeAffaire.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeAvn", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;

            string sql = string.Format(@"SELECT 0 NUMREG, PBAVN CODEAVN, PBTTR CODETRAITEMENT, TRAITEMENT.TPLIB LIBTRAITEMENT,
                        0 DATEDEB, 0 DATEFIN, PBETA CODEETAT, PBSIT CODESITUATION, SITUATION.TPLIB LIBSITUATION,
                        (PBSTA * 10000 + PBSTM * 100 + PBSTJ) DATESIT, 0 HEURESIT, PBMJU USERSITUATION,
                        0 NUMQUITT, '' CODEETATQUITT, '' LIBETATQUITT
                    FROM YPOBASE 
                        {0}
                        {1}
                    WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvn AND PBTTR IN ('AVNRG', 'REGUL')",
                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", "TRAITEMENT", " AND TRAITEMENT.TCOD = PBTTR"),
                    CommonRepository.BuildJoinYYYYPAR("LEFT", "KHEOP", "PBSIT", "SITUATION", " AND SITUATION.TCOD = PBSIT"));

            var result = DbBase.Settings.ExecuteList<LigneRegularisationDto>(CommandType.Text, sql, param);
            if (result != null && result.Any()) {
                return result.FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Récupère les informations de l'avenant
        /// de régularisation à l'initialisation de l'écran
        /// </summary>
        public static RegularisationInfoDto GetInfoAvenantRegule(string codeContrat, string version, string type, string typeAvt, string modeAvt, int exercice, DateTime? periodeDeb, DateTime? periodeFin, string user, string lotId, string reguleId, string regulMode) {
            RegularisationInfoDto modele = new RegularisationInfoDto();
            string error = string.Empty;
            Int64 numInterne = 0;

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sqlInterne = @"SELECT PBAVN INT32RETURNCOL FROM YPOBASE WHERE PBIPB = :codeContrat AND PBALX = :version AND PBTYP = :type";
            var resultInterne = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlInterne, param);
            if (resultInterne != null && resultInterne.Any()) {
                numInterne = resultInterne.FirstOrDefault().Int32ReturnCol;
            }
            if (numInterne == 0) {
                EacParameter[] paramHisto = new EacParameter[2];
                paramHisto[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
                paramHisto[0].Value = codeContrat.PadLeft(9, ' ');
                paramHisto[1] = new EacParameter("version", DbType.Int32);
                paramHisto[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;

                string sqlHisto = @"SELECT COUNT(*) NBLIGN FROM YHPBASE WHERE PBIPB = :codeContrat AND PBALX = :version AND PBHIN = 1";
                if (CommonRepository.ExistRowParam(sqlHisto, paramHisto)) {
                    error = "Anomalie historique";
                }
                else {
                    string sqlCodeRetour = @"SELECT PBTAC STRRETURNCOL FROM YPOBASE WHERE PBIPB = :codeContrat AND PBALX = :version AND PBTYP = :type";
                    var resultCodeRetour = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlCodeRetour, param);
                    if (resultCodeRetour != null && resultCodeRetour.Any()) {
                        if (string.IsNullOrEmpty(resultCodeRetour.FirstOrDefault().StrReturnCol)) {
                            error = "Aucun accord sur l'affaire nouvelle";
                        }
                    }
                }
            }

            string sql = string.Format(@"SELECT PBTTR TYPEAVT, PARTYP.TPLIL LIBELLEAVT, PBAVN NUMINTERNEAVT, 
                                PBAVK NUMAVT, KADDESI DESCRIPTIONAVT, KAJOBSV OBSERVATIONSAVT
                            FROM YPOBASE 
                                LEFT JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP
                                LEFT JOIN KPDESI ON KAAAVDS = KADCHR
                                LEFT JOIN KPOBSV ON KAAOBSV = KAJCHR
                                {0}
                            WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type",
                            CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", "PARTYP", " AND PARTYP.TCOD = PBTTR"));

            var result = DbBase.Settings.ExecuteList<RegularisationInfoDto>(CommandType.Text, sql, param);
            if (result != null && result.Any()) {
                modele = result.FirstOrDefault();
                modele.ModeAvt = modeAvt;
                modele.TypeAvt = typeAvt;
                modele.LibelleAvt = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "PRODU", "PBTTR", typeAvt).Libelle;
                if (modeAvt == AccessMode.CREATE.ToString()) {
                    // Pour BNS et BURNER aucune Mise en Histo n'est effectuée donc les NumAvt & NumInterneAvt ne sont pas incrémentés
                    if (regulMode != "BNS" && regulMode != "BURNER") {
                        if (modele.NumInterneAvt == 0) {
                            modele.NumInterneAvt = numInterne + 1;
                        }
                        else {
                            modele.NumInterneAvt = modele.NumInterneAvt + 1;
                        }

                        if (modele.NumAvt == 0) {
                            modele.NumAvt = numInterne + 1;
                        }
                        else {
                            modele.NumAvt = modele.NumAvt + 1;
                        }
                    }

                }
            }

            //récupération des infos à partir du PGM400 KDA300CL
            var resultPeriode = GetAvnRegule(codeContrat, version, type, modele.NumInterneAvt.ToString(), typeAvt, exercice, periodeDeb, periodeFin, user, lotId, reguleId, regulMode);
            modele.LotId = resultPeriode.LotId;
            modele.Exercice = resultPeriode.Exercice;
            modele.PeriodeDebInt = resultPeriode.PeriodeDebInt;
            modele.PeriodeFinInt = resultPeriode.PeriodeFinInt;
            modele.CodeCourtier = resultPeriode.CodeCourtier;
            modele.NomCourtier = resultPeriode.NomCourtier;
            modele.CodeCourtierCom = resultPeriode.CodeCourtierCom;
            modele.NomCourtierCom = resultPeriode.NomCourtierCom;
            modele.TauxHCATNAT = resultPeriode.TauxHCATNAT;
            modele.TauxCATNAT = resultPeriode.TauxCATNAT;
            modele.CodeQuittancement = resultPeriode.CodeQuittancement;
            modele.LibQuittancement = resultPeriode.LibQuittancement;
            modele.MultiCourtier = resultPeriode.MultiCourtier;
            modele.Courtiers = resultPeriode.Courtiers;
            modele.Alertes = AvenantRepository.GetListAlertesAvenant(codeContrat, version, type, user);
            modele.Quittancements = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "FBENC");
            modele.Motifs = CommonRepository.GetParametres(string.Empty, string.Empty, "ALSPK", "RGMTF");
            modele.RetourPGM = resultPeriode.RetourPGM;
            modele.MotifAvt = resultPeriode.MotifAvt;
            modele.NumAvt = resultPeriode.NumAvt;
            return modele;
        }

        private static int GetMaxDurationRisquePB(string codeOffre, string version) {
            var duree = DbBase.Settings.ExecuteScalar(
                CommandType.Text,
                "SELECT MAX(JEPBA) FROM YPRTRSQ WHERE JEIPB = :codeOffre AND JEALX = :version AND JEPBA <> 0",
                new[] {
                    new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },
                    new EacParameter("version", DbType.Int32) { Value = version.ToInteger() }
                });

            return duree.ToInteger().Value;
        }

        /// <summary>
        /// Récupère les infos à partir du PGM400 KDA300CL
        /// </summary>
        public static RegularisationInfoDto GetAvnRegule(string codeContrat, string version, string type, string codeAvn, string typeAvt,
            int exercice, DateTime? periodeDeb, DateTime? periodeFin, string user, string lotId, string reguleId, string regulMode, string deleteMod = "", string cancelMod = "",
            bool resetLot = false) {
            RegularisationInfoDto model = new RegularisationInfoDto {
                Quittancements = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "FBENC")
            };

            string codeErreur = string.Empty;
            if (string.IsNullOrEmpty(reguleId) || reguleId == "0" || cancelMod == "C" || deleteMod == "D") {
                var result = CommonRepository.ControlePeriode(codeContrat, version, exercice, periodeDeb, periodeFin);
                DtoCommon resultCATNAT = GetInfoCatNat(codeContrat, version, codeAvn, result.DernierAvn.ToString());
                model.Courtiers = GetListCourtierRegule(codeContrat, version);
                model.Exercice = exercice;
                model.PeriodeDebInt = (!string.IsNullOrEmpty(result.PeriodeDeb)) ? AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(result.PeriodeDeb)) : 0;
                model.PeriodeFinInt = (!string.IsNullOrEmpty(result.PeriodeFin)) ? AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(result.PeriodeFin)) : 0;
                model.CodeCourtier = Convert.ToInt32(result.CodeCourtierPayeur);
                model.NomCourtier = model.Courtiers.FirstOrDefault(x => x.Id == Convert.ToInt32(result.CodeCourtierPayeur))?.Descriptif;
                model.CodeCourtierCom = Convert.ToInt32(result.CodeCourtierCommission);
                model.NomCourtierCom = model.Courtiers.FirstOrDefault(x => x.Id == Convert.ToInt32(result.CodeCourtierCommission))?.Descriptif;
                model.TauxHCATNAT = resultCATNAT.DecReturnCol;
                model.TauxCATNAT = resultCATNAT.DecReturnCol2;
                model.CodeQuittancement = result.CodeEncaissement;
                model.LibQuittancement = model.Quittancements.FirstOrDefault(x => x.Code == result.CodeEncaissement)?.Libelle ?? string.Empty;
                model.MultiCourtier = result.MultiCourtier;
                model.NumAvt = string.IsNullOrEmpty(codeAvn) ? 0 : Convert.ToInt64(codeAvn);

                if (regulMode == RegularisationMode.PB.ToString()) {
                    var listRegularisations = GetListeRegularisation(codeContrat, version, type);
                    if (!listRegularisations.Any(rg => rg.RegulMode == RegularisationMode.PB.ToString())) {
                        int maxDureePB = GetMaxDurationRisquePB(codeContrat, version);
                        if (maxDureePB > 0) {
                            var periodeFinCalc = (model.PeriodeDebInt != 0) ? AlbConvert.ConvertDateToInt(AlbConvert.ConvertIntToDate(model.PeriodeDebInt).Value.AddYears(maxDureePB).AddDays(-1)) : model.PeriodeFinInt;
                            model.PeriodeFinInt = periodeFinCalc > model.PeriodeFinInt ? model.PeriodeFinInt : periodeFinCalc;
                        }
                    }
                }
                model.RetourPGM = string.Format("{0}_{1}_{2}", result.PeriodeDeb, result.PeriodeFin, result.CodeErreur);
                codeErreur = result.CodeErreur;
            }
            else {
                model = GetReguleInfo(reguleId);
                model.Courtiers = GetListCourtierRegule(codeContrat, version);
                model.RetourPGM = string.Format("{0}_{1}_{2}", AlbConvert.ConvertDateToStr(AlbConvert.ConvertIntToDate(model.PeriodeDebInt)), AlbConvert.ConvertDateToStr(AlbConvert.ConvertIntToDate(model.PeriodeFinInt)), string.Empty);
            }

            long.TryParse(lotId, out var idLot);
            if (string.IsNullOrEmpty(codeErreur) && (idLot == default || resetLot)) {
                var dateNow = DateTime.Now;

                #region Paramètres
                DbParameter[] param = new DbParameter[16];
                param[0] = new EacParameter("P_CODECONTRAT", codeContrat.PadLeft(9, ' '));
                param[1] = new EacParameter("P_VERSION", 0);
                param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                param[2] = new EacParameter("P_TYPE", type);
                param[3] = new EacParameter("P_NUMAVN", string.IsNullOrEmpty(codeAvn) ? 0 : Convert.ToInt32(codeAvn));
                param[4] = new EacParameter("P_FONCTION", AlbConstantesMetiers.FONC_REGULE);
                param[5] = new EacParameter("P_PERIODEDEB", 0);
                param[5].Value = model.PeriodeDebInt.HasValue ? model.PeriodeDebInt : 0;
                param[6] = new EacParameter("P_PERIODEFIN", 0);
                param[6].Value = model.PeriodeFinInt.HasValue ? model.PeriodeFinInt : 0;
                param[7] = new EacParameter("P_EXERCICE", 0);
                param[7].Value = exercice;
                param[8] = new EacParameter("P_TYPEATTES", string.Empty);
                param[9] = new EacParameter("P_COUVATTES", string.Empty);
                param[10] = new EacParameter("P_USER", user);
                param[11] = new EacParameter("P_DATENOW", 0);
                param[11].Value = AlbConvert.ConvertDateToInt(dateNow);
                param[12] = new EacParameter("P_OLDLOTID", 0);
                param[12].Value = resetLot ? Convert.ToInt32(lotId) : 0;
                param[13] = new EacParameter("P_REGULMODE", regulMode);
                param[14] = new EacParameter("P_ERREUR", string.Empty);
                param[14].Direction = ParameterDirection.InputOutput;
                param[14].DbType = DbType.AnsiStringFixedLength;
                param[15] = new EacParameter("P_LOTID", 0);
                param[15].Value = 0;
                param[15].Direction = ParameterDirection.InputOutput;
                param[15].DbType = DbType.Int32;
                #endregion
                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SETINFOSELW", param);

                model.LotId = !string.IsNullOrEmpty(param[15].Value.ToString()) ? Convert.ToInt64(param[15].Value.ToString()) : 0;
                if (!string.IsNullOrEmpty(param[14].Value.ToString())) {
                    model.RetourPGM = string.Format("{0}_{1}_{2}", string.Empty, string.Empty, param[14].Value.ToString());
                }
            }
            else {
                if (idLot != default) {
                    model.LotId = idLot;
                }
            }

            return model;
        }
        public static void DeleteReguleP(string reguleId) {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("P_REGULEID", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEPERIOD", param);
        }
        /// <summary>
        /// Obtient la liste des risques/objets pour la régularisation
        /// </summary>
        public static RegularisationRsqDto GetListRsqRegule(string lotId, string reguleId, string mode) {
            var model = new RegularisationRsqDto { Risques = new List<RisqueDto>() };
            var parameters = new EacParameter[]
            {
                new EacParameter("REGULEID", 0) { Value = Convert.ToInt64(reguleId) },
                new EacParameter("LOTID", 0) { Value = string.IsNullOrWhiteSpace(lotId) ? 0 : Convert.ToInt64(lotId) }
            };

            string sqlQuery;

            if (string.IsNullOrWhiteSpace(mode)) {
                sqlQuery = @"
                            SELECT JERSQ CODERSQ , KABDESC LIBRSQ , GUGRSQ . KHXDEBP DATEDEBRSQ , GUGRSQ . KHXFINP DATEFINRSQ , KABCIBLE CIBLERSQ , KCIBLERSQ . KAHDESC LIBCIBLERSQ , JERUT TYPEREGULRSQ , IFNULL ( YPARRSQ . TPLIL , '' ) LIBREGULRSQ , 
	                            JGOBJ CODEOBJ , KACDESC LIBOBJ , SELOBJ . KHVDEB DATEDEBOBJ , SELOBJ . KHVFIN DATEFINOBJ , KACCIBLE CIBLEOBJ , KCIBLEOBJ . KAHDESC LIBCIBLEOBJ , JGRUT TYPREGULOBJ , IFNULL ( YPAROBJ . TPLIL , '' ) LIBREGULOBJ , 
	                            ( SELECT COUNT ( * ) FROM KPRGUG WHERE KHXIPB = GUGRSQ . KHXIPB AND KHXALX = GUGRSQ . KHXALX AND KHXTYP = GUGRSQ . KHXTYP AND KHXRSQ = GUGRSQ . KHXRSQ AND KHXKHWID = :REGULEID AND KHXSIT = 'V' ) ISRSQUSED 
                            FROM KPRGUG GUGRSQ 
	                            INNER JOIN YPRTRSQ ON GUGRSQ . KHXIPB = JEIPB AND GUGRSQ . KHXALX = JEALX AND GUGRSQ . KHXRSQ = JERSQ 
	                            INNER JOIN KPRSQ ON JEIPB = KABIPB AND JEALX = KABALX AND JERSQ = KABRSQ 
	                            LEFT JOIN YYYYPAR YPARRSQ ON YPARRSQ . TCON = 'PRODU' AND YPARRSQ . TFAM = 'JERUT' AND YPARRSQ . TCOD = JERUT 
	                            INNER JOIN KPSELW SELOBJ ON :LOTID = SELOBJ . KHVID AND GUGRSQ . KHXRSQ = SELOBJ . KHVRSQ AND SELOBJ . KHVPERI = 'OB' 
	                            INNER JOIN YPRTOBJ ON SELOBJ . KHVIPB = JGIPB AND SELOBJ . KHVALX = JGALX AND SELOBJ . KHVRSQ = JGRSQ AND SELOBJ . KHVOBJ = JGOBJ 
	                            INNER JOIN KPOBJ ON JGIPB = KACIPB AND JGALX = KACALX AND JGRSQ = KACRSQ AND JGOBJ = KACOBJ 
	                            INNER JOIN KCIBLE KCIBLERSQ ON KCIBLERSQ . KAHCIBLE = KABCIBLE 
	                            INNER JOIN KCIBLE KCIBLEOBJ ON KCIBLEOBJ . KAHCIBLE = KACCIBLE 
	                            LEFT JOIN YYYYPAR YPAROBJ ON YPAROBJ . TCON = 'PRODU' AND YPAROBJ . TFAM = 'JERUT' AND YPAROBJ . TCOD = JGRUT 
                            WHERE GUGRSQ . KHXKHWID = :REGULEID 
                            ORDER BY GUGRSQ . KHXRSQ , SELOBJ . KHVOBJ";
            }
            else {
                sqlQuery = @"
                            SELECT JERSQ CODERSQ , KABDESC LIBRSQ , SELRSQ . KHVDEB DATEDEBRSQ , SELRSQ . KHVFIN DATEFINRSQ , KABCIBLE CIBLERSQ , KCIBLERSQ . KAHDESC LIBCIBLERSQ , JERUT TYPEREGULRSQ , IFNULL ( YPARRSQ . TPLIL , '' ) LIBREGULRSQ , 
	                            JGOBJ CODEOBJ , KACDESC LIBOBJ , SELOBJ . KHVDEB DATEDEBOBJ , SELOBJ . KHVFIN DATEFINOBJ , KACCIBLE CIBLEOBJ , KCIBLEOBJ . KAHDESC LIBCIBLEOBJ , JGRUT TYPREGULOBJ , IFNULL ( YPAROBJ . TPLIL , '' ) LIBREGULOBJ , 
	                            ( SELECT COUNT ( * ) FROM KPRGUG WHERE KHXIPB = SELRSQ . KHVIPB AND KHXALX = SELRSQ . KHVALX AND KHXTYP = SELRSQ . KHVTYP AND KHXRSQ = SELRSQ . KHVRSQ AND KHXSIT = 'V' AND KHXKHWID = :REGULEID ) ISRSQUSED 
                            FROM KPSELW SELRSQ 
	                            INNER JOIN YPRTRSQ ON SELRSQ . KHVIPB = JEIPB AND SELRSQ . KHVALX = JEALX AND SELRSQ . KHVRSQ = JERSQ 
	                            INNER JOIN KPRSQ ON JEIPB = KABIPB AND JEALX = KABALX AND JERSQ = KABRSQ 
	                            LEFT JOIN YYYYPAR YPARRSQ ON YPARRSQ . TCON = 'PRODU' AND YPARRSQ . TFAM = 'JERUT' AND YPARRSQ . TCOD = JERUT 
	                            INNER JOIN KPSELW SELOBJ ON SELRSQ . KHVID = SELOBJ . KHVID AND SELRSQ . KHVRSQ = SELOBJ . KHVRSQ AND SELOBJ . KHVPERI = 'OB' 
	                            INNER JOIN YPRTOBJ ON SELOBJ . KHVIPB = JGIPB AND SELOBJ . KHVALX = JGALX AND SELOBJ . KHVRSQ = JGRSQ AND SELOBJ . KHVOBJ = JGOBJ 
	                            INNER JOIN KPOBJ ON JGIPB = KACIPB AND JGALX = KACALX AND JGRSQ = KACRSQ AND JGOBJ = KACOBJ 
	                            INNER JOIN KCIBLE KCIBLERSQ ON KCIBLERSQ . KAHCIBLE = KABCIBLE 
	                            INNER JOIN KCIBLE KCIBLEOBJ ON KCIBLEOBJ . KAHCIBLE = KACCIBLE 
	                            LEFT JOIN YYYYPAR YPAROBJ ON YPAROBJ . TCON = 'PRODU' AND YPAROBJ . TFAM = 'JERUT' AND YPAROBJ . TCOD = JGRUT 
                            WHERE SELRSQ . KHVID = :LOTID AND SELRSQ . KHVPERI = 'RQ' 
                            ORDER BY SELRSQ . KHVRSQ , SELOBJ . KHVOBJ";
            }

            List<RegularisationRsqObjPlatDto> result = DbBase.Settings.ExecuteList<RegularisationRsqObjPlatDto>(CommandType.Text, sqlQuery, parameters);
            if (result != null && result.Any()) {
                #region Risques
                var lstRsq = result.GroupBy(el => el.CodeRsq).Select(el => el.First()).ToList();
                lstRsq.ForEach(rsq => {
                    #region Objets
                    var listObj = new List<ObjetDto>();
                    var lstObj = result.FindAll(r => r.CodeRsq == rsq.CodeRsq).GroupBy(el => el.CodeObj).Select(el => el.First()).ToList();
                    lstObj.ForEach(obj => {
                        listObj.Add(new ObjetDto {
                            Code = obj.CodeObj,
                            Designation = obj.LibObj,
                            EntreeGarantie = AlbConvert.ConvertIntToDate(obj.DateDebObj),
                            SortieGarantie = AlbConvert.ConvertIntToDate(obj.DateFinObj),
                            Cible = new CibleDto { Code = obj.CibleObj, Nom = obj.LibCibleObj.Trim() },
                            CodeTypeRegule = obj.TypeReguleObj,
                            LibTypeRegule = obj.LibTypeRegulObj.Trim(),
                        });
                    });
                    #endregion
                    model.Risques.Add(new RisqueDto {
                        Code = rsq.CodeRsq,
                        Designation = rsq.LibRsq,
                        EntreeGarantie = AlbConvert.ConvertIntToDate(rsq.DateDebRsq),
                        SortieGarantie = AlbConvert.ConvertIntToDate(rsq.DateFinRsq),
                        Cible = new CibleDto { Code = rsq.CibleRsq, Nom = rsq.LibCibleRsq.Trim() },
                        CodeTypeRegule = rsq.TypeReguleRsq,
                        LibTypeRegule = rsq.LibTypeRegulRsq.Trim(),
                        IsUsed = rsq.IsRsqUsed > 0,
                        Objets = listObj
                    });
                });
                #endregion
            }

            model.ReguleId = Convert.ToInt64(reguleId);
            model.PeriodeRegule = GetRegulPeriode(model.ReguleId);
            return model;
        }

        public static List<RegulMatriceDto> GetRegulMatrice(string codeAffaire, int version, string type, string lotId, long regulId) {
            var parameters = new List<EacParameter>()
            {
                new EacParameter("codeAffaire" , DbType.AnsiStringFixedLength) { Value = codeAffaire.PadLeft(9, ' ') },
                new EacParameter("version" , DbType.Int32) { Value = version },
                new EacParameter("type" , DbType.AnsiStringFixedLength) { Value = type }
            };

            if (string.IsNullOrWhiteSpace(lotId)) {
                return DbBase.Settings.ExecuteList<RegulMatriceDto>(
                    CommandType.Text,
                    "SELECT KHXRSQ RISQUEID, KHXFOR FORMULE , KHXKDEID GARID, KHXGARAN GARLIB, '' SITRSQ FROM KPRGUG WHERE KHXIPB = :codeAffaire AND KHXALX = :version  AND KHXTYP = :type GROUP BY KHXRSQ , KHXKDEID , KHXGARAN ",
                    parameters);
            }
            else {
                parameters.Add(new EacParameter("regulId", DbType.Int64) { Value = regulId });
                parameters.Add(new EacParameter("lotId", DbType.Int64) { Value = long.TryParse(lotId, out var x) ? x : 0 });
                const string SelectByLot = @"
SELECT KHVRSQ RISQUEID, KHVFOR FORMULE , KHVKDEID GARID, KHVKDEGAR GARLIB, IFNULL(KILSIT, '') SITRSQ 
FROM KPSELW 
LEFT JOIN KPRGUR ON KHVRSQ = KILRSQ 
AND KILIPB = :codeAffaire 
AND KILALX = :version 
AND KILTYP = :type 
AND KILKHWID = :regulId 
WHERE KHVID = :lotId 
AND KHVKDEID > 0 ";

                return DbBase.Settings.ExecuteList<RegulMatriceDto>(
                    CommandType.Text,
                    SelectByLot,
                    parameters);
            }
        }

        public static List<RegulMatriceDto> GetRegulMatrice(RegularisationContext context) {
            return GetRegulMatrice(context.IdContrat.CodeOffre, context.IdContrat.Version, context.IdContrat.Type, context.LotId.ToString(), context.RgId);
        }

        public static bool IsValidRegul(string reguleId) {
            var param = new List<EacParameter>();

            var p = new EacParameter("reguleId", DbType.Int64) {
                Value = Convert.ToInt64(reguleId)
            };
            param.Add(p);

            var sql = @"SELECT COUNT(*) NBLIGN FROM KPRGUG WHERE KHXSIT = 'V' AND KHXKHWID = :reguleId";
            return CommonRepository.ExistRowParam(sql, param);
        }

        public static IdContratDto GetContratInfo(string codeOffre, string version, string type) {
            var param = new List<DbParameter>();

            param.Add(new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') });
            param.Add(new EacParameter("P_VERSION", DbType.Int32) { Value = Convert.ToInt32(version) });
            param.Add(new EacParameter("P_TYPE", DbType.AnsiStringFixedLength) { Value = type });

            var sql = @"SELECT PBIPB IPB, PBALX ALX, PBTYP TYP, IFNULL(NULLIF(PBMER, ''), 'S') TYPECONTRAT, IFNULL(TPLIL, '') LIBTYPECONTRAT
                        FROM YPOBASE 
                        LEFT JOIN YYYYPAR ON TCON = 'KHEOP' AND TFAM = 'TYPOC' AND TCOD = IFNULL(NULLIF(PBMER, ''), 'S')
                        WHERE PBIPB = :P_CODECONTRAT AND PBALX = :P_VERSION AND PBTYP = :P_TYPE ";

            return DbBase.Settings.ExecuteList<IdContratDto>(CommandType.Text, sql, param).FirstOrDefault();
        }

        public static PeriodeReguleDto GetRegulPeriode(long reguleId) {
            var param = new List<EacParameter>();

            var p = new EacParameter("reguleId", DbType.Int64);
            p.Value = reguleId;
            param.Add(p);

            var sql = $"SELECT KHWDEBP DATEDEBINT,KHWFINP DATEFININT FROM KPRGU WHERE KHWID = :reguleId";

            var result = DbBase.Settings.ExecuteList<PeriodeReguleDto>(CommandType.Text, sql, param).FirstOrDefault();

            result.DateDeb = AlbConvert.ConvertIntToDate(result.DateDebInt);
            result.DateFin = AlbConvert.ConvertIntToDate(result.DateFinInt);

            return result;
        }

        public static IEnumerable<DtoCommon> ParamHorsCoassurance(string codeOffre, string version) {
            var param = new List<EacParameter>
            {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ')},
                new EacParameter("version", DbType.Int32) { Value = Convert.ToInt32(version)}
            };

            string sql = "SELECT  JERUL STRRETURNCOL, JEPBN STRRETURNCOL2 FROM YPRTRSQ WHERE JEIPB = :codeOffre AND JEALX = :version";
            return DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
        }

        public static void SetRisqueStatus(long rgId, int rsqId, bool status) {
            List<EacParameter> param = new List<EacParameter>();

            param.Add(new EacParameter("status", DbType.AnsiStringFixedLength) { Value = status ? "V" : "N" });
            param.Add(new EacParameter("rgId", DbType.Int64) { Value = rgId });
            param.Add(new EacParameter("codeRSQ", DbType.Int32) { Value = rsqId });

            var sql = @"UPDATE KPRGUR SET KILSIT = :status WHERE KILKHWID = :rgId AND KILRSQ = :codeRSQ";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param.ToArray());
        }

        public static void SetRisqueGarantiesStatus(long rgId, int rsqId, bool status) {
            List<EacParameter> param = new List<EacParameter>();

            param.Add(new EacParameter("status", DbType.AnsiStringFixedLength) { Value = status ? "V" : "N" });
            param.Add(new EacParameter("rgId", DbType.Int64) { Value = rgId });
            param.Add(new EacParameter("codeRSQ", DbType.Int32) { Value = rsqId });

            var sql = @"UPDATE KPRGUG SET KHXSIT = :status WHERE KHXKHWID = :rgId AND KHXRSQ = :codeRSQ";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param.ToArray());
        }

        public static int GetNumberOfRiskWithDifferentRate(IdContratDto contrat, long? rgId) {
            List<EacParameter> param = new List<EacParameter>();

            param.Add(new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = contrat.CodeOffre.PadLeft(9, ' ') });
            param.Add(new EacParameter("version", DbType.Int32) { Value = contrat.Version });
            param.Add(new EacParameter("type", DbType.AnsiStringFixedLength) { Value = contrat.Type });
            param.Add(new EacParameter("rgId", DbType.Int64) { Value = rgId.GetValueOrDefault() });

            string sql = @" SELECT COUNT(*) NBLIGNES FROM (
                                SELECT DISTINCT KILPBT TAUXRSQ, KILPBS SEUILSP, KILPBA NBYEARRSQ ,KILPBP TXCOTISRETRSQ, KILPBR TXRISTRSQ
                                FROM KPRGUR 
                                WHERE KILIPB = :codeOffre AND KILALX = :version AND KILTYP = :type AND KILKHWID = :rgId) A";


            var result = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param);

            int nbLines = 0;

            int.TryParse(result.ToString(), out nbLines);

            return nbLines;
        }

        public static List<SaisieRisqueInfoDto> GetListSaisieRisqueRegulatisation(long regulId, RegularisationMode mode) {
            string jepbn = string.Empty;

            switch (mode) {
                case RegularisationMode.BNS:
                    jepbn = "B";
                    break;
                case RegularisationMode.PB:
                    jepbn = "O";
                    break;
                case RegularisationMode.Burner:
                    jepbn = "U";
                    break;
                default:
                    jepbn = "N";
                    break;
            }

            List<EacParameter> paramList = new List<EacParameter>()
            {
                new EacParameter("rgId", DbType.Int64) { Value = regulId },
                new EacParameter("mode", DbType.AnsiStringFixedLength) {Value = jepbn }

            };

            string sql = @"
                            SELECT DISTINCT 
                            CASE CONCAT(CONCAT(CONCAT (CONCAT (JEVDJ, '/'),JEVDM), '/'), JEVDA)
                            WHEN '0/0/0' THEN
                            CONCAT(CONCAT(CONCAT (CONCAT (PBEFJ, '/'),PBEFM), '/'), PBEFA) 
                            ELSE
                            CONCAT(CONCAT(CONCAT (CONCAT (JEVDJ, '/'),JEVDM), '/'), JEVDA) 
                            END DEBUT_RSQ,

                            CASE CONCAT(CONCAT(CONCAT (CONCAT (JEVFJ, '/'),JEVFM), '/'), JEVFA)
                            WHEN '0/0/0' THEN
                            CONCAT(CONCAT(CONCAT (CONCAT (PBFEJ, '/'),PBFEM), '/'), PBFEA) 
                            ELSE
                            CONCAT(CONCAT(CONCAT (CONCAT (JEVFJ, '/'),JEVFM), '/'), JEVFA) 
                            END FIN_RSQ, 
                            CASE KILSIT WHEN 'V' THEN 1 ELSE 0 END ISCHECKED, KABDESC LIBELLE, KABCIBLE CIBLE, 
                            JERSQ CODERSQ, KILPBT TAUXRSQ, KILPBS SEUILSP, JEPBA NBYEARRSQ ,KILPBP TXCOTISRETRSQ, KILPBR TXRISTRSQ, 
                            KABBRNT TAUX_MAXI, KABBRNC PRIME_MAXI  
                            FROM KPRGUR
                            INNER JOIN YPRTRSQ ON KILKHWID = :rgId AND JEPBN = :mode AND JERSQ = KILRSQ AND JEIPB = KILIPB AND JEALX = KILALX
                            INNER JOIN KPRSQ ON KABRSQ = KILRSQ AND KABIPB = KILIPB AND KABALX = KILALX AND KABTYP = KILTYP
                            INNER JOIN YPOBASE ON PBIPB = KILIPB AND PBALX = KILALX AND PBTYP = KILTYP
                            ORDER BY JERSQ ASC";

            return DbBase.Settings.ExecuteList<SaisieRisqueInfoDto>(CommandType.Text, sql, paramList);
        }


        //recharge la liste des risques de la régule
        public static List<RisqueDto> ReloadListRsqRegule(string lotId, string reguleId, bool isReadonly) {
            var listRsq = new List<RisqueDto>();
            List<RegularisationRsqObjPlatDto> result = new List<RegularisationRsqObjPlatDto>();
            EacParameter[] param = new EacParameter[2];

            //if (!isReadonly)
            //{
            //    param = new DbParameter[1];
            //    param[0] = new EacParameter("lotId", 0);
            //    param[0].Value = !string.IsNullOrEmpty(lotId) ? Convert.ToInt32(lotId) : 0;
            //}
            //else
            //{
            param = new EacParameter[2];
            param[0] = new EacParameter("reguleId", DbType.Int64);
            param[0].Value = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt64(reguleId) : 0;
            param[1] = new EacParameter("lotId", DbType.Int64);
            param[1].Value = !string.IsNullOrEmpty(lotId) ? Convert.ToInt64(lotId) : 0;
            //}

            string sql = @"SELECT JERSQ CODERSQ , KABDESC LIBRSQ , SELRSQ . KHVDEB DATEDEBRSQ , SELRSQ . KHVFIN DATEFINRSQ , KABCIBLE CIBLERSQ ,KCIBLERSQ.KAHDESC LIBCIBLERSQ, JERUT TYPEREGULRSQ , IFNULL ( YPARRSQ . TPLIL , '' ) LIBREGULRSQ , 
			                    JGOBJ CODEOBJ , KACDESC LIBOBJ , SELOBJ . KHVDEB DATEDEBOBJ , SELOBJ . KHVFIN DATEFINOBJ , KACCIBLE CIBLEOBJ ,KCIBLEOBJ.KAHDESC LIBCIBLEOBJ , JGRUT TYPREGULOBJ , IFNULL ( YPAROBJ . TPLIL , '' ) LIBREGULOBJ , 
			                    ( SELECT COUNT ( * ) FROM KPRGUG WHERE KHXIPB = SELRSQ . KHVIPB AND KHXALX = SELRSQ . KHVALX AND KHXTYP = SELRSQ . KHVTYP AND KHXRSQ = SELRSQ . KHVRSQ and khxsit = 'V' AND KHXKHWID = :reguleId ) ISRSQUSED 
		                    FROM KPSELW SELRSQ 
			                    INNER JOIN YPRTRSQ ON SELRSQ . KHVIPB = JEIPB AND SELRSQ . KHVALX = JEALX AND SELRSQ . KHVRSQ = JERSQ 
			                    INNER JOIN KPRSQ ON JEIPB = KABIPB AND JEALX = KABALX AND JERSQ = KABRSQ 
			                    LEFT JOIN YYYYPAR YPARRSQ ON YPARRSQ . TCON = 'PRODU' AND YPARRSQ . TFAM = 'JERUT' AND YPARRSQ . TCOD = JERUT 
			                    INNER JOIN KPSELW SELOBJ ON SELRSQ . KHVID = SELOBJ . KHVID AND SELRSQ . KHVRSQ = SELOBJ . KHVRSQ AND SELOBJ . KHVPERI = 'OB' 
			                    INNER JOIN YPRTOBJ ON SELOBJ . KHVIPB = JGIPB AND SELOBJ . KHVALX = JGALX AND SELOBJ . KHVRSQ = JGRSQ AND SELOBJ . KHVOBJ = JGOBJ 
			                    INNER JOIN KPOBJ ON JGIPB = KACIPB AND JGALX = KACALX AND JGRSQ = KACRSQ AND JGOBJ = KACOBJ 
                                INNER JOIN KCIBLE KCIBLERSQ ON KCIBLERSQ.KAHCIBLE = KABCIBLE
			                    INNER JOIN KCIBLE KCIBLEOBJ ON  KCIBLEOBJ.KAHCIBLE = KACCIBLE
			                    LEFT JOIN YYYYPAR YPAROBJ ON YPAROBJ . TCON = 'PRODU' AND YPAROBJ . TFAM = 'JERUT' AND YPAROBJ . TCOD = JGRUT 
		                    WHERE SELRSQ . KHVID = :lotId AND SELRSQ . KHVPERI = 'RQ' 
		                    ORDER BY SELRSQ . KHVRSQ , SELOBJ . KHVOBJ ;";

            if (!isReadonly) {
                result = DbBase.Settings.ExecuteList<RegularisationRsqObjPlatDto>(CommandType.Text, sql, param);
            }
            else {
                result = DbBase.Settings.ExecuteList<RegularisationRsqObjPlatDto>(CommandType.StoredProcedure, "SP_GETLISTRSQREGULECONSULT", param);
            }

            if (result != null && result.Any()) {
                #region Risques
                var lstRsq = result.GroupBy(el => el.CodeRsq).Select(el => el.First()).ToList();
                lstRsq.ForEach(rsq => {
                    #region Objets
                    var listObj = new List<ObjetDto>();
                    var lstObj = result.FindAll(r => r.CodeRsq == rsq.CodeRsq).GroupBy(el => el.CodeObj).Select(el => el.First()).ToList();
                    lstObj.ForEach(obj => {
                        listObj.Add(new ObjetDto {
                            Code = obj.CodeObj,
                            Designation = obj.LibObj,
                            EntreeGarantie = AlbConvert.ConvertIntToDate(obj.DateDebObj),
                            SortieGarantie = AlbConvert.ConvertIntToDate(obj.DateFinObj),
                            Cible = new CibleDto { Code = obj.CibleObj, Nom = rsq.LibCibleObj },
                            CodeTypeRegule = obj.TypeReguleObj,
                            LibTypeRegule = obj.LibTypeRegulObj,
                        });
                    });
                    #endregion
                    listRsq.Add(new RisqueDto {
                        Code = rsq.CodeRsq,
                        Designation = rsq.LibRsq,
                        EntreeGarantie = AlbConvert.ConvertIntToDate(rsq.DateDebRsq),
                        SortieGarantie = AlbConvert.ConvertIntToDate(rsq.DateFinRsq),
                        Cible = new CibleDto { Code = rsq.CibleRsq, Nom = rsq.LibCibleRsq },
                        CodeTypeRegule = rsq.TypeReguleRsq,
                        LibTypeRegule = rsq.LibTypeRegulRsq,
                        IsUsed = rsq.IsRsqUsed > 0,
                        Objets = listObj
                    });
                });
                #endregion
            }

            return listRsq;
        }

        ///Obtient les informations du risques de la régule
        public static RisqueDto GetRisqueRegule(long? lotId, int? codeRsq) {
            RisqueDto rsq = new RisqueDto();
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("lotId", DbType.Int64);
            param[0].Value = lotId ?? 0;
            param[1] = new EacParameter("codeRsq", DbType.Int32);
            param[1].Value = codeRsq ?? 0;
            string sql = @"SELECT KABRSQ CODERSQ, KABDESC LIBRSQ, KHVDEB DATEDEBRSQ, KHVFIN DATEFINRSQ
                                FROM KPSELW 
	                                INNER JOIN KPRSQ ON KABIPB = KHVIPB AND KABALX = KHVALX AND KABTYP = KHVTYP AND KABRSQ = KHVRSQ
                                WHERE KHVID = :lotId AND KHVPERI = 'RQ' AND KHVRSQ = :codeRsq";
            var result = DbBase.Settings.ExecuteList<RegularisationRsqObjPlatDto>(CommandType.Text, sql, param);
            if (result != null && result.Any()) {
                var fstRsq = result.FirstOrDefault();
                rsq.Code = fstRsq.CodeRsq;
                rsq.Designation = fstRsq.LibRsq;
                rsq.DateEntreeDescr = AlbConvert.ConvertIntToDate(fstRsq.DateDebRsq);
                rsq.DateSortieDescr = AlbConvert.ConvertIntToDate(fstRsq.DateFinRsq);
            }

            return rsq;
        }

        /// <summary>
        /// Obtient la liste des garanties pour la régularisation
        /// </summary>
        public static List<RegularisationGarantieDto> GetListGarRegule(long? lotId, long? reguleId, int? codeRsq, bool isReadonly) {
            string sql = "";
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("reguleId", DbType.Int64);
            param[0].Value = reguleId ?? default;
            param[1] = new EacParameter("lotId", DbType.Int64);
            param[1].Value = lotId ?? default;
            param[2] = new EacParameter("codeRsq", DbType.Int32);
            param[2].Value = codeRsq ?? default;

            if (!isReadonly) {
                sql = $@"
SELECT DISTINCT KDAFOR CODEFOR, IFNULL(G.KDEOPT, HG.KDEOPT) CODEOPT, KDAALPHA LETTREFOR, KDADESC LIBFOR,
IFNULL(G.KDEID, HG.KDEID) IDGARAN, IFNULL(G.KDESEQ, HG.KDESEQ) SEQGARAN, IFNULL(G.KDEGARAN, HG.KDEGARAN) CODEGARAN, GADES LIBGARAN, SELGAR.KHVDEB DATEDEBGAR, SELGAR.KHVFIN DATEFINGAR,
GATRG CODETYPEREGULE, IFNULL(TPLIL, '') LIBTYPEREGULE,
(
    SELECT COUNT(*) FROM KPRGUG WHERE KHXIPB = SELGAR.KHVIPB AND KHXALX = SELGAR.KHVALX AND KHXTYP = SELGAR.KHVTYP 
    AND KHXRSQ = SELGAR.KHVRSQ AND KHXFOR = SELGAR.KHVFOR AND KHXKDEID = SELGAR.KHVKDEID AND KHXKHWID = :reguleId AND KHXSIT = 'V'
) ISGARUSED , KAPORDRE, KAQORDRE, IFNULL(G.KDETRI, HG.KDETRI) 
FROM KPSELW SELFOR 
INNER JOIN KPFOR ON KDAIPB = SELFOR.KHVIPB AND KDAALX = SELFOR.KHVALX AND KDATYP = SELFOR.KHVTYP AND KDAFOR = SELFOR.KHVFOR
INNER JOIN KPSELW SELGAR ON SELFOR.KHVID = SELGAR.KHVID AND SELFOR.KHVFOR = SELGAR.KHVFOR AND SELGAR.KHVPERI = '{PerimetreSelectionRegul.Garantie.AsCode()}' 
LEFT JOIN KPGARAN G ON SELGAR.KHVKDEID = G.KDEID 
LEFT JOIN HPGARAN HG ON SELGAR.KHVKDEID = HG.KDEID 
INNER JOIN KGARAN ON IFNULL(G.KDEGARAN, HG.KDEGARAN) = GAGAR AND GARGE = 'O' 
LEFT JOIN KPOPTD VB ON G.KDEKDCID = VB.KDCID 
LEFT JOIN HPOPTD HVB ON (HG.KDEKDCID, HG.KDEAVN) = (HVB.KDCID, HVB.KDCAVN) 
INNER JOIN KCATBLOC ON IFNULL(VB.KDCKAQID, HVB.KDCKAQID) = KAQID 
INNER JOIN KCATVOLET ON KAQKAPID = KAPID 
LEFT JOIN YYYYPAR ON TCON = 'PRODU' AND TFAM = 'GATRG' AND TCOD = GATRG 
WHERE SELFOR.KHVID = :lotId AND SELFOR.KHVRSQ = :codeRsq AND SELFOR.KHVPERI = '{PerimetreSelectionRegul.Formule.AsCode()}' 
ORDER BY KAPORDRE, KAQORDRE, IFNULL(G.KDETRI, HG.KDETRI)";
            }
            else {
                param = new EacParameter[2];
                param[0] = new EacParameter("reguleId", DbType.Int64);
                param[0].Value = reguleId ?? 0;
                param[1] = new EacParameter("codeRsq", DbType.Int32);
                param[1].Value = codeRsq ?? 0;

                sql = @"
SELECT DISTINCT KDAFOR CODEFOR, IFNULL(G.KDEOPT, HG.KDEID) CODEOPT, KDAALPHA LETTREFOR, KDADESC LIBFOR,
IFNULL(G.KDEID, HG.KDEID) IDGARAN, IFNULL(G.KDESEQ, HG.KDESEQ) SEQGARAN, IFNULL(G.KDEGARAN, HG.KDEGARAN) CODEGARAN, GADES LIBGARAN, KHXDEBP DATEDEBGAR, KHXFINP DATEFINGAR,
GATRG CODETYPEREGULE, IFNULL(TPLIL, '') LIBTYPEREGULE,
1 ISGARUSED , KAPORDRE, KAQORDRE, IFNULL(G.KDETRI, HG.KDETRI)
FROM KPRGUG GUGFOR 
INNER JOIN KPFOR ON KDAIPB = GUGFOR.KHXIPB AND KDAALX = GUGFOR.KHXALX AND KDATYP = GUGFOR.KHXTYP AND KDAFOR = GUGFOR.KHXFOR
LEFT JOIN KPGARAN G ON KHXKDEID = G.KDEID 
LEFT JOIN HPGARAN HG ON KHXKDEID = HG.KDEID 
INNER JOIN KGARAN ON IFNULL(G.KDEGARAN, HG.KDEGARAN) = GAGAR AND GARGE = 'O' 
LEFT JOIN KPOPTD VB ON G.KDEKDCID = VB.KDCID 
LEFT JOIN HPOPTD HVB ON (HG.KDEKDCID, HG.KDEAVN) = (HVB.KDCID, HVB.KDCAVN) 
INNER JOIN KCATBLOC ON IFNULL(VB.KDCKAQID, HVB.KDCKAKID) = KAQID
INNER JOIN KCATVOLET ON KAQKAPID = KAPID
LEFT JOIN YYYYPAR ON TCON = 'PRODU' AND TFAM = 'GATRG' AND TCOD = GATRG
WHERE GUGFOR.KHXKHWID = :reguleId AND GUGFOR.KHXRSQ = :codeRsq 
ORDER BY KAPORDRE, KAQORDRE, IFNULL(G.KDETRI, HG.KDETRI)";
            }
            var result = DbBase.Settings.ExecuteList<RegularisationGarantieDto>(CommandType.Text, sql, param);
            if (result != null && result.Any()) {
                return result;
            }
            return new List<RegularisationGarantieDto>();
        }

        /// <summary>
        /// Obtient le "S'applique à" pour une formule
        /// </summary>
        public static RisqueDto GetAppliqueRegule(string codeContrat, string version, string type, string codeAvn, string codeFor) {
            RisqueDto risque = new RisqueDto();

            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFor", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeFor) ? Convert.ToInt32(codeFor) : 0;


            const string SqlQuery = @"
SELECT KABRSQ CODERSQ, KABDESC DESCRSQ, KACOBJ CODEOBJ, KACDESC DESCOBJ FROM KPOPTAP
INNER JOIN KPRSQ ON KDDTYP = KABTYP AND KDDIPB = KABIPB AND KDDALX = KABALX AND KDDRSQ = KABRSQ
INNER JOIN KPOBJ ON KDDTYP = KACTYP AND KDDIPB = KACIPB AND KDDALX = KACALX AND KDDRSQ = KACRSQ AND (KDDOBJ = KACOBJ OR KDDOBJ = 0)
WHERE KDDIPB = :codeContrat AND KDDALX = :version AND KDDTYP = :type AND KDDFOR = :codeFor";

            var result = DbBase.Settings.ExecuteList<RisqueObjetPlatDto>(CommandType.Text, SqlQuery, param);
            if (result != null && result.Count > 0) {
                var lstRsq = result.GroupBy(el => el.CodeRsq).Select(r => r.First()).FirstOrDefault();
                risque.Code = Convert.ToInt32(lstRsq.CodeRsq);
                risque.Designation = lstRsq.DescRsq;
                risque.Descriptif = lstRsq.DescRsq;
                risque.Objets = new List<ObjetDto>();

                var lstObj = result.FindAll(o => o.CodeRsq == lstRsq.CodeRsq);
                foreach (var obj in lstObj) {
                    risque.Objets.Add(new ObjetDto {
                        Code = Convert.ToInt32(obj.CodeObj),
                        Designation = obj.DescObj,
                        Descriptif = obj.DescObj
                    });
                }
            }
            return risque;
        }

        /// <summary>
        /// Obtient détail d'une garantie de régul
        /// </summary>
        public static RegularisationGarInfoDto GetInfoRegularisationGarantie(GarantieInfo garantie, bool isMultiRC, string codeContrat, string version, string type, string codeAvn, string lotId, string codersq, string idregul, bool isReadonly) {
            RegularisationGarInfoDto toReturn = new RegularisationGarInfoDto();

            toReturn.GarantieInfo = garantie;
            var context = new RegularisationContext {
                RgId = Int64.Parse(idregul),
                LotId = Int64.Parse(lotId),
                IdContrat = new IdContratDto {
                    CodeOffre = codeContrat,
                    Version = Int32.Parse(version),
                    Type = type
                },
                RsqId = Int64.Parse(codersq),
                IsReadOnlyMode = isReadonly,
                GrId = garantie.Id,
                CodeFormule = garantie.CodeFormule
            };

            var repo = new RegularisationRepository(null);
            if (!isReadonly) {
                if (isMultiRC) {
                    toReturn.ErreurStr = CreateMouvementsGarantieRCPeriodes(context);
                }
                else {
                    toReturn.ErreurStr = MouvementsGarantiePeriode(codeContrat, version, type, codersq, garantie.CodeFormule.ToString(), garantie.Id.ToString(), lotId, idregul);
                }

                if (!toReturn.ErreurStr.IsEmptyOrNull()) {
                    return toReturn;
                }
            }

            toReturn.RegulPeriodDetail = repo.GetInfoGarantie(new Folder { CodeOffre = codeContrat, Version = int.Parse(version), Type = type }, long.Parse(lotId), garantie.Id, long.Parse(idregul));
            toReturn.AppliqueRegule = GetAppliqueRegule(codeContrat, version, type, codeAvn, garantie.CodeFormule.ToString());
            toReturn.ListMvtPeriod = repo.GetListMouvements(new Folder { CodeOffre = codeContrat, Version = int.Parse(version), Type = type }, int.Parse(codersq), garantie.CodeFormule, garantie.CodeGarantie);
            if (isMultiRC) {
                if (!isReadonly) {
                    EnsureGarantieRCMouvtPeriod(context);
                }
                toReturn.ListPeriodRegulGar = GetGarantiesRCPeriod(long.Parse(idregul), garantie.Id);
            }
            else {
                toReturn.ListPeriodRegulGar = GetListMouvtGarantie(codeContrat, version, type, codersq, garantie.CodeFormule.ToString(), garantie.Id.ToString(), garantie.CodeGarantie, idregul, isReadonly);
            }

            var result = GetDateMinMax(codeContrat, version, type, codersq, garantie.CodeFormule.ToString(), garantie.CodeGarantie);
            if (result != null) {
                toReturn.MouvementPeriodeDebMin = result.DateDebReturnCol;
                toReturn.MouvementPeriodeFinMax = result.DateFinReturnCol;
            }

            return toReturn;
        }

        public static RegularisationComputeData GetInfosRegularisationRisqueTR(long rgId, long rsqId) {
            SpreadSumMontantEmis(rgId, rsqId);

            const string Selection = @"
SELECT KILPBT PBT, KILPBA PBA, KILPBS PBS, KILPBR PBR, KILPBP PBP, KILRIA RIA, KILEMD EMD, KILSIT SIT,
KILCOT COT, KILSCHG SCHG, KILSIDF SIDF, KILSREC SREC, KILSPRO SPRO, KILSPRE SPRE, KILSRIM SRIM,
KABRSQ CONCAT ' - ' concat KABDESC LBL, KILMHC MHC, KILKHWID RGID, KILRSQ RSQID,
PBRGT CONCAT ' - ' CONCAT RG.TPLIB RGT, KILSIDF SIDF, KILSREC SREC, KILSPRO SPRO, KILSPRE PRE, KILSREP RCR,
IFNULL(NVX.KIMSCHG, 0) SRCN, IFNULL(TRV.KIMSCHG, 0) SREP, TRV.KIMCRD STD, KHWMRG MRG, KHWIPB IPB
FROM KPRGUR
INNER JOIN KPRGU ON KHWID = KILKHWID
INNER JOIN KPRSQ ON KILRSQ = KABRSQ AND KILIPB = KABIPB AND KILTYP = KABTYP AND KILALX = KABALX
INNER JOIN YPOBASE ON PBIPB = KHWIPB AND PBALX = KHWALX AND PBTYP = KHWTYP AND PBAVN = KHWAVN
LEFT JOIN KPRGUC NVX ON NVX.KIMIPB = KHWIPB AND NVX.KIMALX = KHWALX AND NVX.KIMTYP = KHWTYP AND NVX.KIMSIT = 'A'
LEFT JOIN KPRGUC TRV ON TRV.KIMIPB = KHWIPB AND TRV.KIMALX = KHWALX AND TRV.KIMTYP = KHWTYP AND TRV.KIMSIT = 'V'
LEFT JOIN YYYYPAR RG ON RG.TCOD = PBRGT AND RG.TFAM = 'TAXRG' AND RG.TCON = 'GENER'
WHERE KILRSQ = :codeRsq
AND KILKHWID = :rgid";

            var result = DbBase.Settings.ExecuteList<RegularisationComputeData>(
                CommandType.Text,
                Selection,
                new EacParameter[]
                {
                    new EacParameter("codeRsq", DbType.Int64) { Value = rsqId },
                    new EacParameter("rgid", DbType.Decimal) { Value = (decimal)rgId }

                });

            var data = result?.FirstOrDefault();


            return data;
        }

        public static RegularisationComputeData GetInfosRegularisationContratTR(long rgId, long numAvt) {
            SpreadSumMontantEmis(rgId);
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("REGULEID", 0) { Value = Convert.ToInt64(rgId) };
            var resultInterne = DbBase.Settings.ExecuteList<DtoCommon>(
                CommandType.Text,
                "SELECT PBAVN INT32RETURNCOL FROM YPOBASE INNER JOIN KPRGU ON KHWIPB = PBIPB WHERE KHWID = :regulId",
                new[] { new EacParameter("REGULEID", 0) { Value = Convert.ToInt64(rgId) } });
            int numInterne = -1;
            string modeNavig = "S";
            if (resultInterne != null && resultInterne.Any()) {
                numInterne = resultInterne.FirstOrDefault().Int32ReturnCol;
            }
            if (numInterne != numAvt) {
                modeNavig = "H";
            }
            string selection = $@"
SELECT KHWPBT PBT, KHWPBA PBA, KHWPBS PBS, KHWPBR PBR, KHWPBP PBP, KHWRIA RIA, KHWEMD EMD, KHWCOT COT, KHWSIT SIT,
KHWSCHG SCHG, KHWSIDF SIDF, KHWSREC SREC, KHWSPRO SPRO, KHWSPRE SPRE, KHWSRIM SRIM, '' LBL, KHWMHC MHC, KHWID RGID, 0 RSQID,
PBRGT CONCAT ' - ' CONCAT RG.TPLIB RGT, KHWSIDF SIDF, IFNULL(KHWSREC, 0) SREC, KHWSPRO SPRO, KHWSPRE PRE, KHWSREP RCR, IFNULL(NVX.KIMSCHG, 0) SRCN,
IFNULL(TRV.KIMSCHG, 0) SREP, TRV.KIMCRD STD, KHWMRG MRG, KHWIPB IPB
FROM KPRGU
INNER JOIN {CommonRepository.GetPrefixeHisto(modeNavig.ParseCode<ModeConsultation>(), "YPOBASE")} ON PBIPB = KHWIPB AND PBALX = KHWALX AND PBTYP = KHWTYP AND PBAVN = KHWAVN
LEFT JOIN KPRGUC NVX ON NVX.KIMIPB = KHWIPB AND NVX.KIMALX = KHWALX AND NVX.KIMTYP = KHWTYP AND NVX.KIMSIT = 'A'
LEFT JOIN KPRGUC TRV ON TRV.KIMIPB = KHWIPB AND TRV.KIMALX = KHWALX AND TRV.KIMTYP = KHWTYP AND TRV.KIMSIT = 'V'
LEFT JOIN YYYYPAR RG ON RG.TCOD = PBRGT AND RG.TFAM = 'TAXRG' AND RG.TCON = 'GENER'
WHERE KHWID = :rgid";

            var result = DbBase.Settings.ExecuteList<RegularisationComputeData>(
                CommandType.Text,
                selection,
                new [] { new EacParameter("rgid", DbType.Decimal) { Value = (decimal)rgId } });

            var data = result?.FirstOrDefault();
            return data.Refresh();
        }


        public static RegularisationComputeData GetInfosRegularisationContrat(long regularisationId, long numAvt) {
            SpreadSumMontantEmis(regularisationId);
            var resultInterne = DbBase.Settings.ExecuteList<DtoCommon>(
                CommandType.Text,
                "SELECT PBAVN INT32RETURNCOL FROM YPOBASE INNER JOIN KPRGU ON KHWIPB = PBIPB WHERE KHWID = :regulId",
                new[] { new EacParameter("REGULEID", 0) { Value = Convert.ToInt64(regularisationId) } });
            int numInterne = -1;
            string modeNavig = "S";
            if (resultInterne != null && resultInterne.Any()) {
                numInterne = resultInterne.FirstOrDefault().Int32ReturnCol;
            }
            if (numInterne != numAvt) {
                modeNavig = "H";
            }
            string selection = $@"
SELECT KHWMRG MRG, KHWPBT PBT, KHWPBTR PBTR, KHWSIT SIT,
KHWPBA PBA, KHWPBS PBS, KHWPBR PBR, KHWPBP PBP, KHWRIA RIA, KHWEMD EMD, KHWCOT COT,
KHWSCHG SCHG, KHWSIDF SIDF, KHWSREC SREC, KHWSPRO SPRO, KHWSPRE SPRE, KHWSRIM SRIM, '' LBL, KHWMHC MHC, KHWID RGID, 0 RSQID,
PBRGT CONCAT ' - ' CONCAT RG.TPLIB RGT, KHWBRNT BRNT, KHWBRNC BRNC, KHWASV ASV, KHWSPC SPC, 
'' FORM, KHWIPB IPB
FROM KPRGU
INNER JOIN {CommonRepository.GetPrefixeHisto(modeNavig.ParseCode<ModeConsultation>(), "YPOBASE")} ON PBIPB = KHWIPB AND PBALX = KHWALX AND PBTYP = KHWTYP AND PBAVN = KHWAVN 
LEFT JOIN YYYYPAR RG ON RG.TCOD = PBRGT AND RG.TFAM = 'TAXRG' AND RG.TCON = 'GENER'
WHERE KHWID = :rgid";

            var result = DbBase.Settings.ExecuteList<RegularisationComputeData>(
                CommandType.Text,
                selection,
                new [] { new EacParameter("rgid", DbType.Decimal) { Value = (decimal)regularisationId } });

            var data = result?.FirstOrDefault();
            return data;
        }

        public static RegularisationComputeData GetInfosRegularisationRisque(long regularisationId, long codeRsq, long numAvt) {
            SpreadSumMontantEmis(regularisationId, codeRsq);

            var resultInterne = DbBase.Settings.ExecuteList<DtoCommon>(
    CommandType.Text,
    "SELECT PBAVN INT32RETURNCOL FROM YPOBASE INNER JOIN KPRGUR ON KILIPB = PBIPB WHERE KILKHWID = :regulId",
    new[] { new EacParameter("REGULEID", 0) { Value = Convert.ToInt64(regularisationId) } });
            int numInterne = -1;
            string modeNavig = "S";
            if (resultInterne != null && resultInterne.Any())
            {
                numInterne = resultInterne.FirstOrDefault().Int32ReturnCol;
            }
            if (numInterne != numAvt)
            {
                modeNavig = "H";
            }

            string Selection = $@"
SELECT KHWMRG MRG, KILPBT PBT, KILPBTR PBTR, KILSIT SIT,
KILPBA PBA, KILPBS PBS, KILPBR PBR, KILPBP PBP, KILRIA RIA, KILEMD EMD, 
KILCOT COT, KILSCHG SCHG, KILSIDF SIDF, KILSREC SREC, KILSPRO SPRO, KILSPRE SPRE, KILSRIM SRIM,
KABRSQ CONCAT ' - ' CONCAT KABDESC LBL, KILMHC MHC, KILKHWID RGID, KILRSQ RSQID,
PBRGT CONCAT ' - ' CONCAT RG.TPLIB RGT , KILBRNT BRNT, KILBRNC BRNC, KILASV ASV, KILSPC SPC,
TRIM ( KDAALPHA ) CONCAT ' - ' CONCAT KDADESC FORM, KHWIPB IPB
FROM KPRGUR
INNER JOIN KPRGU ON KHWID = KILKHWID
INNER JOIN KPRGUG ON KHXKHWID  = KHWID AND KHXRSQ = KILRSQ
INNER JOIN {CommonRepository.GetPrefixeHisto(modeNavig.ParseCode<ModeConsultation>(), "KPRSQ")} ON KILRSQ = KABRSQ AND KILIPB = KABIPB AND KILTYP = KABTYP AND KILALX = KABALX
INNER JOIN {CommonRepository.GetPrefixeHisto(modeNavig.ParseCode<ModeConsultation>(), "KPFOR")} ON KDAIPB = KHXIPB AND KDAALX = KHXALX AND KDATYP = KHXTYP AND KDAFOR = KHXFOR
INNER JOIN {CommonRepository.GetPrefixeHisto(modeNavig.ParseCode<ModeConsultation>(), "YPOBASE")} ON PBIPB = KHWIPB AND PBALX = KHWALX AND PBTYP = KHWTYP AND PBAVN = KHWAVN
LEFT JOIN YYYYPAR RG ON RG.TCOD = PBRGT AND RG.TFAM = 'TAXRG' AND RG.TCON = 'GENER'
WHERE KILRSQ = :codeRsq
AND KILKHWID = :rgid";

            var result = DbBase.Settings.ExecuteList<RegularisationComputeData>(
                CommandType.Text,
                Selection,
                new [] {
                    new EacParameter("codeRsq", DbType.Int64) { Value = codeRsq },
                    new EacParameter("rgid", DbType.Decimal) { Value = (decimal)regularisationId }
                });

            var data = result?.FirstOrDefault();
            return data;
        }

        public static string CreateMouvementsGarantieRCPeriodes(RegularisationContext context) {
            // select first all available RC
            var messages = new List<string>();
            List<long> codesGaranties = FindAllGarantiesRCId(new GarantieFilter { LotId = context.LotId, RsqNum = (int)context.RsqId });
            foreach (var code in codesGaranties) {
                messages.Add(CommonRepository.MouvementsGarantiePeriode(
                    context.IdContrat.CodeOffre,
                    context.IdContrat.Version.ToString(),
                    context.IdContrat.Type,
                    context.RsqId.ToString(),
                    context.CodeFormule.ToString(),
                    code.ToString(),
                    context.LotId.ToString(),
                    context.RgId.ToString()));
            }

            return string.Join(Environment.NewLine, messages.Where(m => !m.IsEmptyOrNull()));
        }

        public static List<long> FindAllGarantiesRCId(GarantieFilter filter) {
            string selection = $@"
SELECT KHVKDEID FROM KPSELW 
WHERE KHVPERI = '{PerimetreSelectionRegul.Garantie.AsCode()}' AND KHVID = :lotId AND KHVRSQ = :rsqNum 
AND KHVKDEGAR IN ('{AlbOpConstants.RCFrance}', '{AlbOpConstants.RCExport}', '{AlbOpConstants.RCUSA}')";

            return DbBase.Settings.ExecuteNumericList<long>(
                CommandType.Text,
                selection,
                new[] {
                    new EacParameter("lotId", DbType.Int64) { Value = filter.LotId },
                    new EacParameter("rsqNum", DbType.Int32) { Value = filter.RsqNum }
                });
        }

        /********  Lancement de prog KDA301CL prédetermine les mouvements sur la garantie     ***/
        public static string MouvementsGarantiePeriode(string codeContrat, string version, string type, string rsq, string codfor, string garan, string idlot, string idregul) {
            return CommonRepository.MouvementsGarantiePeriode(codeContrat, version, type, rsq, codfor, garan, idlot, idregul);
        }

        /****    recupérer la liste des periode à regulariser sur la garantie ***/
        public static List<LigneMouvtGarantieDto> GetListMouvtGarantie(string codeContrat, string version, string type, string codersq, string codfor, string idGar, string codegar, string idregul, bool isReadonly) {
            EacParameter[] param = new EacParameter[9];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODERSQ", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codersq) ? Convert.ToInt32(codersq) : 0;
            param[4] = new EacParameter("P_CODEFOR", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codfor) ? Convert.ToInt32(codfor) : 0;
            param[5] = new EacParameter("P_CODEGAR", DbType.AnsiStringFixedLength);
            param[5].Value = codegar;
            param[6] = new EacParameter("P_IDGAR", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(idGar) ? Convert.ToInt32(idGar) : 0;
            param[7] = new EacParameter("P_IDREGUL", DbType.Int32);
            param[7].Value = !string.IsNullOrEmpty(idregul) ? Convert.ToInt32(idregul) : 0;
            param[8] = new EacParameter("P_ISREADONLY", DbType.Int32);
            param[8].Value = isReadonly == true ? 1 : 0;

            var result = DbBase.Settings.ExecuteList<LigneMouvtGarantieDto>(CommandType.StoredProcedure, "SP_GETLISTMOUVTGAR", param);
            if (result != null && result.Any()) {
                return result;
            }
            return null;

        }

        /// <summary>
        /// Tries to create KPRGUG in RCFR context (meaning RCFR, RCEX and RCUS)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="codeFormule"></param>
        /// <returns></returns>
        public static void EnsureGarantieRCMouvtPeriod(RegularisationContext context) {
            DbBase.Settings.ExecuteNonQuery(
                CommandType.StoredProcedure,
                "SP_ENSURELISTMOUVTGARRC",
                new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.IdContrat.CodeOffre.PadLeft(9, ' ') },
                new EacParameter("P_VERSION", DbType.Int32) { Value = context.IdContrat.Version },
                new EacParameter("P_TYPE", DbType.AnsiStringFixedLength, 1) { Value = context.IdContrat.Type },
                new EacParameter("P_CODERSQ", DbType.Int32) { Value = (int)context.RsqId },
                new EacParameter("P_CODEFOR", DbType.Int32) { Value = context.CodeFormule },
                new EacParameter("P_IDGAR", DbType.Decimal) { Value = Convert.ToDecimal(context.GrId) },
                new EacParameter("P_IDREGUL", DbType.Decimal) { Value = Convert.ToDecimal(context.RgId) },
                new EacParameter("P_IDLOT", DbType.Decimal) { Value = Convert.ToDecimal(context.LotId) });
        }

        /// <summary>
        /// Gets the periods for Garanties RC group
        /// </summary>
        /// <param name="rgId">Regularisation identifier</param>
        /// <param name="grId">Meant to be the RCFR Garantie identifier</param>
        /// <returns></returns>
        public static List<LigneMouvtGarantieDto> GetGarantiesRCPeriod(long rgId, long grId) {
            var result = DbBase.Settings.ExecuteList<LigneMouvtGarantieDto>(
                CommandType.Text,
                @"
SELECT KHXID CODE , KHXSIT SITUATION , KHXDEBP DATEDEB , KHXFINP DATEFIN , 
CASE WHEN NULLIF(KHXCU2, '') IS NULL THEN KHXBAS ELSE KHXCA2 END ASSIETTE,
CASE WHEN NULLIF(KHXCU2, '') IS NULL THEN CASE WHEN KHXBAU = 'D' THEN KHXBAM ELSE KHXBAT END ELSE CASE WHEN KHXCU2 = 'D' THEN KHXCP2 ELSE KHXCT2 END END TAUXFORFAITHTVALEUR,
CASE WHEN NULLIF(KHXCU2, '') IS NULL THEN KHXBAU ELSE KHXCU2 END TAUXFORFAITHTUNITE, KHXMHT MONTANTREGULHF
FROM KPRGUG
WHERE KHXKHWID = :IDREGUL AND KHXKDEID = :IDGAR",
                new[] {
                    new EacParameter("IDREGUL", DbType.Int64) { Value = rgId },
                    new EacParameter("IDGAR", DbType.Int64) { Value = grId }
                });

            return result;
        }

        /*  Recupération de DateMin & DateMax   */
        public static DtoCommon GetDateMinMax(string codeContrat, string version, string type, string codersq, string codfor, string codegar) {
            var result = new List<DtoCommon>();

            EacParameter[] param = new EacParameter[6];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeRsq", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codersq) ? Convert.ToInt32(codersq) : 0;
            param[4] = new EacParameter("codeFor", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codfor) ? Convert.ToInt32(codfor) : 0;
            param[5] = new EacParameter("codeGar", DbType.AnsiStringFixedLength);
            param[5].Value = codegar;

            string sql = @"
SELECT MIN(KHYDEBP) DATEDEBRETURNCOL, MAX(KHYFINP) DATEFINRETURNCOL FROM KPRGUWP 
WHERE ( KHYIPB , KHYALX , KHYTYP , KHYRSQ , KHYFOR , KHYGARAN ) = ( :codeContrat , :version , :type , :codeRsq , :codeFor , :codeGar ) ";

            result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            if (result != null && result.Any()) {
                return result.FirstOrDefault();
            }

            return result.FirstOrDefault();
        }
        /*  Ajouter period mouvt   */
        public static List<LigneMouvtGarantieDto> AjouterMouvtPeriod(string codeContrat, string version, string type, string codersq, string codfor, string codegar, string idregul, int datedeb, int datefin, string montantHt, string montantTaxe, bool isMultiRC) {
            EacParameter[] param = new EacParameter[12];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODERSQ", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codersq) ? Convert.ToInt32(codersq) : 0;
            param[4] = new EacParameter("P_CODEFOR", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codfor) ? Convert.ToInt32(codfor) : 0;
            param[5] = new EacParameter("P_CODEGAR", DbType.AnsiStringFixedLength);
            param[5].Value = codegar;
            param[6] = new EacParameter("P_IDREGUL", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(idregul) ? Convert.ToInt32(idregul) : 0;
            param[7] = new EacParameter("P_DATEDEB", DbType.Int32);
            param[7].Value = datedeb;
            param[8] = new EacParameter("P_DATEFIN", DbType.Int32);
            param[8].Value = datefin;
            param[9] = new EacParameter("P_MHT", DbType.Int32);
            param[9].Value = !string.IsNullOrEmpty(montantHt) ? Convert.ToDecimal(montantHt) : 0;
            param[10] = new EacParameter("P_MTX", DbType.Int32);
            param[10].Value = !string.IsNullOrEmpty(montantTaxe) ? Convert.ToDecimal(montantTaxe) : 0;
            param[11] = new EacParameter("P_MULTIRC", isMultiRC ? "O" : "N");
            var result = DbBase.Settings.ExecuteList<LigneMouvtGarantieDto>(CommandType.StoredProcedure, "SP_ADDMOUVTGAR", param);
            if (result != null && result.Any()) {
                return result;
            }
            return null;
        }

        public static string IsCheckChevauchementPeriodRegule(string codeContrat, string version, string type, string codersq, string codfor, string codegar, string idregul, int datedeb, int datefin) {
            /*Controle chevauchement dates */
            string retour = string.Empty;
            var listgar = new List<LigneMouvtGarantieDto>();
            listgar = GetListDatesPeriod(codeContrat, version, type, idregul, codersq, codfor, codegar);
            for (int i = 0; i <= listgar.Count - 1; i++) {
                if (datedeb < listgar[i].PeriodeRegulFin) {
                    int iDateFin = 0;
                    int.TryParse(listgar[i].PeriodeRegulFin.ToString(), out iDateFin);
                    DateTime? dDateDeb = AlbConvert.ConvertIntToDate(datedeb);
                    DateTime? dDateFin = AlbConvert.ConvertIntToDate(iDateFin);
                    retour = "Attention les dates " + dDateFin.Value.ToString("dd/MM/yyyy") + " et " + dDateDeb.Value.ToString("dd/MM/yyyy") + " se chevauchent";
                }
            }
            return retour;
        }

        /*  Supprimer period mouvt   */
        public static List<LigneMouvtGarantieDto> SupprimerMouvtPeriod(string codeContrat, string version, string type, string codersq, string codfor, string idgar, string coderegul, string code, bool isMultiRC) {
            var result = DbBase.Settings.ExecuteList<LigneMouvtGarantieDto>(
                CommandType.StoredProcedure,
                "SP_DELETEMOUVTPERIOD",
                new EacParameter[]
                {
                    new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength) { Value = codeContrat.PadLeft(9, ' ') },
                    new EacParameter("P_VERSION", DbType.Int32) { Value = Int32.Parse(version) },
                    new EacParameter("P_TYPE", DbType.AnsiStringFixedLength) { Value = type },
                    new EacParameter("P_CODERSQ", DbType.Int32) { Value = Convert.ToInt32(codersq) },
                    new EacParameter("P_CODEFOR", DbType.Int32) { Value = Convert.ToInt32(codfor) },
                    new EacParameter("P_IDGAR", DbType.Decimal) { Value = Convert.ToDecimal(idgar) },
                    new EacParameter("P_CODEREGULE", DbType.Decimal) { Value = Convert.ToDecimal(coderegul) },
                    new EacParameter("P_CODE", DbType.Decimal) { Value = Convert.ToDecimal(code) },
                    new EacParameter("P_MULTIRC", DbType.AnsiStringFixedLength) { Value = isMultiRC ? "O" : "N" }
                });

            return result?.Any() == true ? result : null;
        }

        public static List<LigneMouvtGarantieDto> ReloadMouvtPeriod(string codeAffaire, string version, string type, string codeRsq, Int32 codeFor, string codeGar, string codeRegul) {
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeAffaire.Trim();
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODERSQ", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0;
            param[4] = new EacParameter("P_CODEFOR", DbType.Int32);
            param[4].Value = codeFor;
            param[5] = new EacParameter("P_CODEGAR", DbType.AnsiStringFixedLength);
            param[5].Value = codeGar;
            param[6] = new EacParameter("P_CODEREGULE", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(codeRegul) ? Convert.ToInt32(codeRegul) : 0;

            var result = DbBase.Settings.ExecuteList<LigneMouvtGarantieDto>(CommandType.StoredProcedure, "SP_RELOADMOUVTPERIOD", param);
            if (result.Any()) {
                return result;
            }
            return null;
        }

        public static List<LigneMouvtGarantieDto> GetListDatesPeriod(string codeContrat, string version, string type, string reguleId, string codersq, string codfor, string codegar) {

            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeRsq", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codersq) ? Convert.ToInt32(codersq) : 0;
            param[4] = new EacParameter("codeFor", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codfor) ? Convert.ToInt32(codfor) : 0;
            param[5] = new EacParameter("codeGar", DbType.AnsiStringFixedLength);
            param[5].Value = codegar;
            param[6] = new EacParameter("reguleId", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0;

            string sql = @"
SELECT KHXDEBP DATEDEB,KHXFINP DATEFIN,KHXSIT SITUATION
FROM KPRGUG WHERE KHXIPB = :codeContrat AND KHXALX = :version  
AND  KHXTYP = :type  AND KHXRSQ = :codeRsq AND KHXFOR = :codeFor  AND KHXGARAN = :codeGar AND KHXKHWID = :reguleId
ORDER BY KHXDEBP ";
            var result = DbBase.Settings.ExecuteList<LigneMouvtGarantieDto>(CommandType.Text, sql, param);
            if (result != null && result.Any()) {
                return result;
            }
            return new List<LigneMouvtGarantieDto>();
        }

        public static string CheckDatesPeriodAllRsqIntegrity(string codeContrat, string version, string type, string idLot, string typAvt, string dateDebReg, string dateFinReg, string reguleId) {
            string result = string.Empty;
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("idLot", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(idLot) ? Convert.ToInt32(idLot) : 0;
            string sql = $@"
SELECT KHVKDEID CODEGAR, KHVDEB DATEDEB,KHVFIN DATEFIN,KHVRSQ CODERSQ 
FROM KPSELW 
WHERE KHVIPB = :codeContrat AND KHVALX = :version  
AND  KHVTYP = :type AND KHVID = :idLot AND  KHVPERI = '{PerimetreSelectionRegul.Garantie.AsCode()}' ORDER BY KHVRSQ,KHVDEB,KHVFIN";
            var garanties = DbBase.Settings.ExecuteList<InfoGarantiesRsqDto>(CommandType.Text, sql, param);

            EacParameter[] paramGar = new EacParameter[4];
            paramGar[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            paramGar[0].Value = codeContrat.PadLeft(9, ' ');
            paramGar[1] = new EacParameter("version", DbType.Int32);
            paramGar[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            paramGar[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramGar[2].Value = type;
            paramGar[3] = new EacParameter("idRegul", DbType.Int32);
            paramGar[3].Value = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0;

            string sqlkprgug = @"
SELECT KHXKDEID CODEGARP, KHXDEBP DATEDEBP,KHXFINP DATEFINP,KHXRSQ CODERSQP,KHXSIT SITUATION 
FROM KPRGUG 
WHERE KHXIPB = :codeContrat AND KHXALX = :version
AND  KHXTYP = :type AND KHXKHWID= :idRegul  ORDER BY KHXRSQ,KHXDEBP,KHXFINP";

            var garantiesKp = DbBase.Settings.ExecuteList<InfoGarantiesRsqDto>(CommandType.Text, sqlkprgug, paramGar);
            /**  Controle sur toute la période générale **/
            if (garantiesKp != null && garantiesKp.Any()) {
                var FiltreParRsq = garantiesKp.Select(p => p.Codersqp).Distinct<int>();
                foreach (var rsq in FiltreParRsq) {
                    var max_Period =
                      (from tab1 in garantiesKp.Where(t => t.Codersqp == rsq)
                       select tab1.PeriodeFin).Max();
                    var min_Period =
                       (from tab1 in garantiesKp.Where(t => t.Codersqp == rsq)
                        select tab1.PeriodDeb).Min();
                    if (max_Period != AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(dateFinReg)) || min_Period != AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(dateDebReg))) {
                        return result = "la régularisation des garanties n'a pas été faite sur toute la période générale";
                    }
                }
            }

            if (garanties != null && garanties.Any()) {
                var risques = garanties.Select(p => p.Codersq).Distinct<int>();
                foreach (var rsq in risques) {
                    var RegulesP = garanties.Where(p => p.Codersq == rsq).OrderBy(p => p.PeriodeRegulDeb).ToArray();
                    foreach (var garantie in RegulesP) {
                        var Periodes = garantiesKp.Where(m => m.CodGarp == garantie.CodGar).OrderBy(m => m.PeriodeRegulDeb).ToArray();
                        for (int i = 1; i <= Periodes.Count() - 1; i++) {
                            if (Periodes[i].PeriodDeb < Periodes[i - 1].PeriodeFin) {
                                DateTime? dDateDeb = AlbConvert.ConvertIntToDate(Periodes[i].PeriodDeb);
                                DateTime? dDateFin = AlbConvert.ConvertIntToDate(Periodes[i - 1].PeriodeFin);

                                result = "Attention les dates " + dDateFin.Value.ToString("dd/MM/yyyy") + " et " + dDateDeb.Value.ToString("dd/MM/yyyy") + " se chevauchent";
                            }
                            DateTime? date1 = AlbConvert.ConvertIntToDate(Periodes[i - 1].PeriodeFin);
                            DateTime? PeriodeFin = AlbConvert.GetFinPeriode(date1.Value, 2, AlbOpConstants.Jour);
                            long? PeriodeFinPlus = PeriodeFin.Value.Year * 10000 + PeriodeFin.Value.Month * 100 + PeriodeFin.Value.Day;
                            if (Periodes[i].PeriodDeb > PeriodeFinPlus) {
                                DateTime? dDateDeb = AlbConvert.ConvertIntToDate(Periodes[i].PeriodDeb);
                                DateTime? dDateFin = AlbConvert.ConvertIntToDate(Periodes[i - 1].PeriodeFin);
                                result = "Attention les dates " + dDateFin.Value.ToString("dd/MM/yyyy") + " et " + dDateDeb.Value.ToString("dd/MM/yyyy") + " se discontinuent";
                            }
                        }
                    }
                }
            }

            return result;
        }
        public static bool GetMouvementPeriode(string codeContrat, string version, string type, string codersq, string codfor, string codegar, int datedeb, int datefin) {

            EacParameter[] param = new EacParameter[8];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeRsq", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codersq) ? Convert.ToInt32(codersq) : 0;
            param[4] = new EacParameter("codeFor", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codfor) ? Convert.ToInt32(codfor) : 0;
            param[5] = new EacParameter("codeGaran", DbType.AnsiStringFixedLength);
            param[5].Value = codegar.Trim();
            param[6] = new EacParameter("dateDeb", DbType.Int32);
            param[6].Value = datedeb;
            param[7] = new EacParameter("dateFin", DbType.Int32);
            param[7].Value = datefin;

            string sql = @"
SELECT COUNT ( * ) NBLIGN
FROM KPRGUWP 
WHERE KHYIPB = :codeContrat AND KHYALX = :version AND KHYTYP = :type AND KHYRSQ = :codeRsq AND KHYFOR = :codeFor
AND KHYGARAN = :codeGaran
AND (KHYDEBP BETWEEN :dateDeb AND :dateFin) 
AND (KHYFINP BETWEEN :dateDeb AND :dateFin)";
//AND(:dateDeb BETWEEN KHYDEBP AND KHYFINP)
//AND(:dateFin BETWEEN KHYDEBP AND KHYFINP)";

            return CommonRepository.ExistRowParam(sql, param);
        }

        /// <summary>
        /// Obtient information de l'ecran saisie garantie
        /// </summary>
        public static SaisieInfoRegulPeriodDto InitSaisieGarRegul(string idRegulGar, string codeAvenant) {
            /* Lancement du AS400 KA039CL calcul montantht et taxe Prev    */
            string returnPrev = GetMhtTaxePrevisionnel(idRegulGar, codeAvenant);
            /* Lancement du AS400 KA039CL calcul montantht et taxe Def    */
            string returnDef = GetMhtTaxeDefinitif(idRegulGar, codeAvenant);

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("idRegulGar", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(idRegulGar) ? Convert.ToInt32(idRegulGar) : 0;

            string sql = @"
SELECT DISTINCT CASE KHWMTF WHEN 'INFERIEURE' THEN 1 ELSE 0 END MOTIF_INF, IFNULL ( G.KDETAXCOD , HG.KDETAXCOD ) CODETAXE_STD,TAXERSQ.TPLIB LIBTAXE_STD, KHXGARAN TITREGAR_STD,KHXDEBP DATEDEB_STD,KHXFINP DATEFIN_STD,KHXRSQ CODERSQ_STD, KABDESC LIBRSQ_STD,
CASE WHEN JERGT = '' THEN PBRGT ELSE JERGT END  CODERGT_STD,CASE WHEN JERGT = '' THEN RGTBASE.TPLIL ELSE RGTRSQ.TPLIL END  LIBRGT_STD,
KHXFOR CODEFOR_STD , KDADESC LIBFOR_STD , KDAALPHA LETTREFOR_STD, JERUT TYPEREGULGAR_STD, YGATRG.TPLIL LIBREGULGAR_STD,  
JEGAU GARAUTO_STD, JEGVL VALGARAUTO_STD, JEGUN UNITGARAUTO_STD, JEPBT TXAPPEL_STD, KAAATGTFA TXATTENTAT_STD, JDTFF MNTCOTISPROV_STD, JDACQ MNTCOTISACQUIS_STD,JDPRO PRIMEPRO,
KHXCA1 PREVASSIETTE_STD, CASE WHEN KHXCU1 = 'D' THEN KHXCP1 ELSE KHXCT1 END PREVTAUX_STD, KHXCU1 PREVUNITE_STD, KHXCX1 PREVCODTAXE_STD, 
KHXCA2 DEFASSIETTE_STD, CASE WHEN KHXCU2 = 'D' THEN KHXCP2 ELSE KHXCT2 END DEFTAUX_STD, KHXCU2 DEFUNITE_STD, KHXCX2 DEFCODTAXE_STD, 
KHXECH MNTCOTISEMIS_STD , KHXECT MNTTXEMIS_STD,KHXEMH MNTFORCEEMIS_STD,KHXEMT MNTFORCETX_STD, KHXCOE COEFF_STD ,
KHXMHC MNTREGULHT_STD,KHXGRM ATTENTAT_STD,KHXMHT MNTFORCECALC_STD,KHXFR0 FORCE0_STD,KHWDEBP DATEDEBREGUL,KHWFINP DATEFINREGUL,
JEPBA NBYEARRSQ_PB ,JEPBR TXRISTRSQ_PB,JEPBS SEUILSP_PB, JEPBP TXCOTISRETRSQ_PB, ((KHXKTD - KHXKEA) * (-1)) RISTANTICIPEE_PB,
KHXKEA COTISEMISE_PB, KHXPBT TXAPPELPBNS_PB, 
CAST((CASE WHEN KHXPBT = 0 THEN KHXKEA ELSE (KHXKEA * 100 / KHXPBT) END) AS DECIMAL(11, 2)) COTISDUE_PB,
KHXPEI NBYEARREGUL_PB  ,
CAST(((CAST((CASE WHEN KHXPBT = 0 THEN KHXKEA ELSE (KHXKEA * 100 / KHXPBT) END) AS DECIMAL(11, 2)) * KHXPBP) / 100) AS DECIMAL(11,2)) COTISRETENUE_PB,
KHXPBP TXCOTISRET_PB,KHXSIP CHARGESIN_PB,
CAST(((KHXKTD * KHXPBP / 100) - KHXSIP) * (KHXPBR / 100) AS DECIMAL(11, 2)) PBNS_PB, 
KHXPBR TXRISTREGUL_PB, KHXRIA RISTANTICIPEEREGUL_PB,KHXFRC TOPFRC,
IFNULL ( G.KDEFOR , HG.KDEFOR ) CODEFORMULE , IFNULL ( G.KDEOPT , HG.KDEOPT ) CODEOPTION , KHVKDEGAR CODEGARANTIE, GADES LIBELLE ,
JDPRO PRIMEPRO, JERUT TYPEGRILLE, JDTMC MNTCALCULREF 
FROM KPRGUG 
INNER JOIN KPSELW ON KHVPERI = 'GA' AND KHXKDEID = KHVKDEID 
INNER JOIN KPRGU ON KHWIPB = KHXIPB AND KHWALX = KHXALX AND KHWTYP= KHXTYP AND KHWID= KHXKHWID
INNER JOIN KGARAN ON KHXGARAN = GAGAR
INNER JOIN YPOBASE ON PBIPB = KHXIPB AND PBALX = KHXALX AND PBTYP= KHXTYP
INNER JOIN YPRTENT ON JDIPB = KHXIPB AND JDALX = KHXALX 
INNER JOIN KPENT ON KAAIPB = KHXIPB AND KAAALX = KHXALX AND KAATYP = KHXTYP
INNER JOIN YPRTRSQ ON JEIPB = KHXIPB AND JEALX=KHXALX  AND JERSQ=KHXRSQ 
INNER JOIN KPRSQ ON KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ=JERSQ
LEFT JOIN YYYYPAR RGTRSQ ON RGTRSQ.TCON = 'GENER' AND RGTRSQ.TFAM = 'TAXRG' AND RGTRSQ.TCOD = JERGT
LEFT JOIN YYYYPAR RGTBASE ON RGTBASE.TCON = 'GENER' AND RGTBASE.TFAM = 'TAXRG'  AND RGTBASE.TCOD = PBRGT
INNER JOIN KPFOR ON  KHXIPB = KDAIPB AND KHXALX = KDAALX AND KHXTYP = KDATYP AND KHXFOR = KDAFOR
LEFT JOIN KPGARAN G ON KHXKDEID = G.KDEID 
LEFT JOIN HPGARAN HG ON ( KHVKDEGAR , KHVKDESEQ , KHVFOR , KHVIPB , KHVALX , KHVAVN ) = ( HG.KDEGARAN , HG.KDESEQ , HG.KDEFOR , HG.KDEIPB , HG.KDEALX , HG.KDEAVN )
LEFT JOIN YYYYPAR TAXERSQ ON TAXERSQ.TCON = 'GENER' AND TAXERSQ.TFAM = 'TAXEC' AND TAXERSQ.TCOD = IFNULL ( G.KDETAXCOD , HG.KDETAXCOD )
LEFT JOIN YYYYPAR YGATRG ON YGATRG.TCON = 'PRODU' AND YGATRG.TFAM = 'JERUT' AND YGATRG.TCOD = JERUT
WHERE KHXID = :idRegulGar";

            var result = DbBase.Settings.ExecuteList<SaisieGarInfoDto>(CommandType.Text, sql, param);
            SaisieInfoRegulPeriodDto toReturn = new SaisieInfoRegulPeriodDto();
            SaisieGarInfoDto model = new SaisieGarInfoDto();
            if (result != null && result.Any()) {
                model = result.FirstOrDefault();
                model.PrevMntHt_STD = double.Parse(returnPrev.Split('_')[0]);
                model.PrevTax_STD = double.Parse(returnPrev.Split('_')[1]);
                if (returnDef.Split('_')[0] == "ERROR") {
                    model.ErrorStr = returnDef.Split('_')[1];
                    model.DefVmntHt_STD = 0;
                    model.DefVtax_STD = 0;
                }
                else {
                    model.DefVmntHt_STD = double.Parse(returnDef.Split('_')[0]);
                    model.DefVtax_STD = double.Parse(returnDef.Split('_')[1]);
                    // model.DefAssiette_STD = 0;
                }
                model.MntCotisProv_STD = model.MntCotisProv_STD > 0 ? model.MntCotisProv_STD : model.MntCalculRef;
                toReturn.LignePeriodRegul = model;
                toReturn.CodeFormule = model.CodeFormule;
                toReturn.CodeGarantie = model.CodeGarantie;
                toReturn.CodeOption = model.CodeOption;
                toReturn.Libelle = model.Libelle;
            }
            /* Les LISTES UNITES ET CODES TAXE DEFINITIF */
            toReturn.UnitesDef = GetListUnites();
            toReturn.CodesTaxesDef = GetListCodesTaxes();
            /*sab *08/7/16*/
            if (string.IsNullOrEmpty(toReturn.LignePeriodRegul.DefCodTaxe_STD) || string.IsNullOrEmpty(toReturn.LignePeriodRegul.DefUnite_STD)) {
                toReturn.LignePeriodRegul.DefCodTaxe_STD = toReturn.LignePeriodRegul.PrevCodTaxe_STD;
                //2017-01-31 : correction bug 2243 on ne reprend pas l'assiette
                toReturn.LignePeriodRegul.SuivantDesactiv = toReturn.LignePeriodRegul.DefAssiette_STD == 0;
                toReturn.LignePeriodRegul.DefTaux_STD = toReturn.LignePeriodRegul.PrevTaux_STD;
                toReturn.LignePeriodRegul.DefUnite_STD = toReturn.LignePeriodRegul.PrevUnite_STD;

                //2017-01-31 : correction bug 2243 on lance le calcul du montant std à l'initialisation
                if (toReturn.LignePeriodRegul.Coef_STD == 0) {
                    toReturn.LignePeriodRegul.MntRegulHt_STD = toReturn.LignePeriodRegul.DefVmntHt_STD - toReturn.LignePeriodRegul.MntForceEmis_STD;
                }
                else {
                    toReturn.LignePeriodRegul.MntRegulHt_STD = (toReturn.LignePeriodRegul.DefVmntHt_STD - toReturn.LignePeriodRegul.MntForceEmis_STD) * toReturn.LignePeriodRegul.Coef_STD / 100;
                }
                //Si motif régularisation "INFERIEURE" => montant de régul = 0
                if (toReturn.LignePeriodRegul.Motif_Inf == 1)
                    toReturn.LignePeriodRegul.MntRegulHt_STD = 0;
            }


            return toReturn;
        }
        public static List<ParametreDto> GetListUnites() {

            string sql = @"SELECT TCOD CODE , TPLIB LIBELLE FROM YYYYPAR WHERE TCON='ALSPK' AND TFAM ='UNPRI' ";

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }
        public static List<ParametreDto> GetListCodesTaxes() {
            string sql = @"SELECT TCOD CODE , TPLIB LIBELLE FROM YYYYPAR WHERE TCON='GENER' AND TFAM ='TAXEC' ";

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        public static bool HasGarantieRCGrouped(GarantieFilter filter) {
            string selection = $@"
SELECT 1 FROM KPSELW 
WHERE KHVPERI = '{PerimetreSelectionRegul.Garantie.AsCode()}' AND KHVID = :lotId AND KHVRSQ = :rsqNum AND KHVKDEGAR = '{AlbOpConstants.RCFrance}' 
UNION ALL 
SELECT 2 FROM KPSELW 
WHERE KHVPERI = '{PerimetreSelectionRegul.Garantie.AsCode()}' AND KHVID = :lotId AND KHVRSQ = :rsqNum AND KHVKDEGAR IN ('{AlbOpConstants.RCExport}', '{AlbOpConstants.RCUSA}') ";

            var result = DbBase.Settings.ExecuteNumericList<int>(
                CommandType.Text,
                selection,
                new EacParameter[]
                {
                    new EacParameter("lotId", DbType.Int64) { Value = filter.LotId },
                    new EacParameter("rsqNum", DbType.Int32) { Value = filter.RsqNum }
                });

            return result.Any(v => v == 1) && result.Count > 1;
        }

        public static long FindGarantieRCFRId(GarantieFilter filter) {
            string selection = $@"
SELECT KHXID FROM KPRGUG 
INNER JOIN KPRGU ON KHXKHWID = KHWID 
AND KHXKHWID = :rgId AND KHXRSQ = :rsqNum AND KHXDEBP = :debut AND KHXFINP = :fin AND KHXGARAN = '{AlbOpConstants.RCFrance}' ";

            var result = DbBase.Settings.ExecuteNumericList<long>(
                CommandType.Text,
                selection,
                new EacParameter[]
                {
                    new EacParameter("rgId", DbType.Int64) { Value = filter.RgId },
                    new EacParameter("rsqNum", DbType.Int32) { Value = filter.RsqNum },
                    new EacParameter("debut", DbType.Int32) { Value = filter.DateDebut.ToIntYMD() },
                    new EacParameter("fin", DbType.Int32) { Value = filter.DateFin.ToIntYMD() }
                });

            return result.FirstOrDefault();
        }

        public static SaisieGarantieInfosDto GetGarantiesRCFRHeader(GarantieFilter filter)
        {
            string selection = $@"
SELECT CASE KHWMTF WHEN 'INFERIEURE' THEN 1 ELSE 0 END MOTIF_INF,
	KDETAXCOD CODETAXE, TAXERSQ.TPLIB LIBTAXE, KHXGARAN TITREGAR,
	KHXDEBP DATEDEB, KHXFINP DATEFIN, KHXRSQ CODERSQ, KABDESC LIBRSQ,
    JERGT CODERGT, RGTRSQ.TPLIL LIBRGT, KHXFOR CODEFOR, KDADESC LIBFOR,
    KDAALPHA LETTREFOR, JERUT TYPEREGULGAR, IFNULL(YPAREGUL.TPLIL, '') LIBREGULGAR, 
    JEGAU GARAUTO, JEGVL VALGARAUTO, JEGUN UNITGARAUTO, JEPBT TXAPPEL,
    KAAATGTFA TXATTENTAT, JDTFF MNTCOTISPROV, JDACQ MNTCOTISACQUIS,
    JDPRO PRIMEPRO, KHWDEBP DATEDEBREGUL, KHWFINP DATEFINREGUL,
    KHXFRC TOPFRC, KDEFOR CODEFORMULE, KDEOPT CODEOPTION,
    KDEGARAN CODEGARANTIE, GADES LIBELLE
FROM KPRGUG 
INNER JOIN KPRGU ON KHXKHWID = KHWID 
AND KHXID = :rgGrId 
AND KHXGARAN = '{AlbOpConstants.RCFrance}' 
INNER JOIN YPOBASE ON PBIPB = KHXIPB AND PBALX = KHXALX AND PBTYP = KHXTYP AND PBAVN = KHXAVN 
INNER JOIN YPRTENT ON JDIPB = KHXIPB AND JDALX = KHXALX 
INNER JOIN KPENT ON KAAIPB = KHXIPB AND KAAALX = KHXALX AND KAATYP = KHXTYP 
INNER JOIN YPRTRSQ ON JEIPB = KHXIPB AND JEALX = KHXALX  AND JERSQ = KHXRSQ 
INNER JOIN KPRSQ ON KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ = JERSQ 
LEFT JOIN YYYYPAR RGTRSQ ON RGTRSQ.TCON = 'GENER' AND RGTRSQ.TFAM = 'TAXRG' AND RGTRSQ.TCOD = JERGT 
LEFT JOIN YYYYPAR RGTBASE ON RGTBASE.TCON = 'GENER' AND RGTBASE.TFAM = 'TAXRG' AND RGTBASE.TCOD = PBRGT 
INNER JOIN KPFOR ON  KHXIPB = KDAIPB AND KHXALX=KDAALX AND KHXTYP = KDATYP AND KHXFOR = KDAFOR 
INNER JOIN KPGARAN ON KHXKDEID = KDEID 
LEFT JOIN YYYYPAR TAXERSQ ON TAXERSQ.TCON = 'GENER' AND TAXERSQ.TFAM = 'TAXEC' AND TAXERSQ.TCOD = KDETAXCOD 
INNER JOIN KGARAN ON KHXGARAN = GAGAR 
LEFT JOIN YYYYPAR YPAREGUL ON YPAREGUL.TCON = 'PRODU' AND YPAREGUL.TFAM = 'JERUT' AND YPAREGUL.TCOD = JERUT ";

            var result = DbBase.Settings.ExecuteList<SaisieGarantieInfosDto>(
                CommandType.Text,
                selection,
                new EacParameter[]
                {
                    new EacParameter("rgGrId", DbType.Int64) { Value = filter.GrId }
                });

            return result.FirstOrDefault();
        }

        public static List<SaisieGarantieInfosDto> GetGarantiesRCGroup(GarantieFilter filter)
        {
            string Selection = $@"
SELECT CASE KHWMTF WHEN 'INFERIEURE' THEN 1 ELSE 0 END MOTIF_INF,
	KHXID ID,
    JDTFF MNTCOTISPROV,
    JDACQ MNTCOTISACQUIS,
    JDPRO PRIMEPRO,
    KHXCA1 PREVASSIETTE,
    CASE WHEN KHXCU1 = 'D' THEN KHXCP1 ELSE KHXCT1 END PREVTAUX,
    KHXCU1 PREVUNITE,
    KHXCX1 PREVCODTAXE, 
    KHXCA2 DEFASSIETTE,
    CASE WHEN KHXCU2 = 'D' THEN KHXCP2 ELSE KHXCT2 END DEFTAUX,
    KHXCU2 DEFUNITE,
    KHXCX2 DEFCODTAXE, 
    KHXECH MNTCOTISEMIS,
    KHXECT MNTTXEMIS,
    KHXEMH MNTFORCEEMIS,
    KHXEMT MNTFORCETX,
    KHXCOE COEFF,
    KHXMHC MNTREGULHT,
    KHXGRM ATTENTAT,
    KHXMHT MNTFORCECALC,
    KHXFR0 FORCE0,
    KHWDEBP DATEDEBREGUL,
    KHWFINP DATEFINREGUL,
    KHXFRC TOPFRC,
    KDEFOR CODEFORMULE,
    KDEOPT CODEOPTION,
    KDEGARAN CODEGARANTIE,
    GADES LIBELLE, 
    0 ISREADONLYRCUS
FROM KPRGUG 
INNER JOIN KPRGU ON KHXKHWID = KHWID 
AND KHXID = :rgGrId 
AND KHXGARAN = '{AlbOpConstants.RCFrance}' 
INNER JOIN YPOBASE ON PBIPB= KHXIPB AND PBALX = KHXALX AND PBTYP = KHXTYP AND PBAVN = KHXAVN 
INNER JOIN YPRTENT ON JDIPB= KHXIPB AND JDALX = KHXALX 
INNER JOIN KPENT ON KAAIPB = KHXIPB AND KAAALX = KHXALX AND KAATYP = KHXTYP 
INNER JOIN YPRTRSQ ON JEIPB = KHXIPB AND JEALX=KHXALX  AND JERSQ = KHXRSQ 
INNER JOIN KPRSQ ON KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ = JERSQ 
LEFT JOIN YYYYPAR RGTRSQ ON RGTRSQ.TCON = 'GENER' AND RGTRSQ.TFAM = 'TAXRG' AND RGTRSQ.TCOD = JERGT 
LEFT JOIN YYYYPAR RGTBASE ON RGTBASE.TCON = 'GENER' AND RGTBASE.TFAM = 'TAXRG' AND RGTBASE.TCOD = PBRGT 
INNER JOIN KPFOR ON  KHXIPB = KDAIPB AND KHXALX = KDAALX AND KHXTYP = KDATYP AND KHXFOR = KDAFOR 
INNER JOIN KPGARAN ON KHXKDEID = KDEID 
LEFT JOIN YYYYPAR TAXERSQ ON TAXERSQ.TCON = 'GENER' AND TAXERSQ.TFAM = 'TAXEC' AND TAXERSQ.TCOD = KDETAXCOD 
INNER JOIN KGARAN ON KHXGARAN = GAGAR 
LEFT JOIN YYYYPAR YGARUT ON YGARUT.TCON = 'PRODU' AND YGARUT.TFAM = 'JERUT' AND YGARUT.TCOD = GARUT 
UNION ALL 
SELECT DISTINCT 
    CASE KHWMTF WHEN 'INFERIEURE' THEN 1 ELSE 0 END MOTIF_INF, 
	G.KHXID ID, 
    JDTFF MNTCOTISPROV, 
    JDACQ MNTCOTISACQUIS, 
    JDPRO PRIMEPRO, 
    G.KHXCA1 PREVASSIETTE, 
    CASE WHEN G.KHXCU1 = 'D' THEN G.KHXCP1 ELSE G.KHXCT1 END PREVTAUX, 
    G.KHXCU1 PREVUNITE, 
    G.KHXCX1 PREVCODTAXE, 
    G.KHXCA2 DEFASSIETTE,
    CASE WHEN G.KHXCU2 = 'D' THEN G.KHXCP2 ELSE G.KHXCT2 END DEFTAUX,
    G.KHXCU2 DEFUNITE,
    G.KHXCX2 DEFCODTAXE, 
    G.KHXECH MNTCOTISEMIS,
    G.KHXECT MNTTXEMIS,
    G.KHXEMH MNTFORCEEMIS,
    G.KHXEMT MNTFORCETX,
    G.KHXCOE COEFF,
    G.KHXMHC MNTREGULHT,
    G.KHXGRM ATTENTAT, 
    G.KHXMHT MNTFORCECALC, 
    G.KHXFR0 FORCE0, 
    KHWDEBP DATEDEBREGUL, 
    KHWFINP DATEFINREGUL, 
    G.KHXFRC TOPFRC, 
    KDEFOR CODEFORMULE, 
    KDEOPT CODEOPTION, 
    KDEGARAN CODEGARANTIE, 
    GADES LIBELLE, 
    CASE WHEN TRIM ( G.KHXGARAN ) = '{AlbOpConstants.RCUSA}' THEN 
        (CASE WHEN 0 < ( SELECT COUNT(1) FROM KPGARTAR WHERE KDGGARAN = '{AlbOpConstants.USA_CAN}' AND ( KDGIPB , KDGALX , KDGOPT , KDGFOR ) = ( G.KHXIPB , G.KHXALX , KDEOPT , G.KHXFOR ) ) AND ((KDEASVALA > 0 AND GT.KDGPRIVALA > 0) OR GT.KDGPRIMPRO > 0) THEN 0 ELSE 1 END) 
    ELSE 0 END ISREADONLY 
FROM KPRGUG G 
INNER JOIN KPRGU ON G.KHXKHWID = KHWID 
AND G.KHXGARAN IN ('{AlbOpConstants.RCExport}', '{AlbOpConstants.RCUSA}') 
INNER JOIN KPRGUG FRG ON G.KHXDEBP = FRG.KHXDEBP 
AND G.KHXFINP = FRG.KHXFINP 
AND FRG.KHXID = :rgGrId 
AND G.KHXKHWID = FRG.KHXKHWID 
AND G.KHXRSQ = FRG.KHXRSQ 
AND G.KHXFOR = FRG.KHXFOR 
INNER JOIN YPOBASE ON PBIPB = G.KHXIPB AND PBALX = G.KHXALX AND PBTYP = G.KHXTYP AND PBAVN = G.KHXAVN 
INNER JOIN YPRTENT ON JDIPB = G.KHXIPB AND JDALX = G.KHXALX  
INNER JOIN KPENT ON KAAIPB = G.KHXIPB AND KAAALX = G.KHXALX AND KAATYP = G.KHXTYP 
INNER JOIN YPRTRSQ ON JEIPB = G.KHXIPB AND JEALX = G.KHXALX  AND JERSQ = G.KHXRSQ 
INNER JOIN KPRSQ ON KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ = JERSQ 
LEFT JOIN YYYYPAR RGTRSQ ON RGTRSQ.TCON = 'GENER' AND RGTRSQ.TFAM = 'TAXRG' AND RGTRSQ.TCOD = JERGT 
LEFT JOIN YYYYPAR RGTBASE ON RGTBASE.TCON = 'GENER' AND RGTBASE.TFAM = 'TAXRG'  AND RGTBASE.TCOD = PBRGT 
INNER JOIN KPFOR ON G.KHXIPB = KDAIPB AND G.KHXALX = KDAALX AND G.KHXTYP = KDATYP AND G.KHXFOR = KDAFOR 
INNER JOIN KPGARAN ON G.KHXKDEID = KDEID 
LEFT JOIN KPGARTAR GT ON KDGKDEID = KDEID 
LEFT JOIN YYYYPAR TAXERSQ ON TAXERSQ.TCON = 'GENER' AND TAXERSQ.TFAM = 'TAXEC' AND TAXERSQ.TCOD = KDETAXCOD 
INNER JOIN KGARAN ON G.KHXGARAN = GAGAR 
LEFT JOIN YYYYPAR YGARUT ON YGARUT.TCON = 'PRODU' AND YGARUT.TFAM = 'JERUT' AND YGARUT.TCOD = GARUT";

            var result = DbBase.Settings.ExecuteList<SaisieGarantieInfosDto>(
                CommandType.Text,
                Selection,
                new EacParameter[]
                {
                    new EacParameter("rgGrId", DbType.Int64) { Value = filter.GrId }
                });

            return result;
        }

        public static SaisieGarantieInfosDto GetGarantiesRCFRHeaderHisto(GarantieFilter filter)
        {
            string selection = $@"
SELECT CASE KHWMTF WHEN 'INFERIEURE' THEN 1 ELSE 0 END MOTIF_INF,
	KDETAXCOD CODETAXE,
    TAXERSQ.TPLIB LIBTAXE,
	KHXGARAN TITREGAR,
	KHXDEBP DATEDEB,
	KHXFINP DATEFIN,
	KHXRSQ CODERSQ,
	KABDESC LIBRSQ,
    JERGT CODERGT,
    RGTRSQ.TPLIL LIBRGT,
    KHXFOR CODEFOR,
    KDADESC LIBFOR,
    KDAALPHA LETTREFOR,
    JERUT TYPEREGULGAR,
    IFNULL(YPAREGUL.TPLIL, '') LIBREGULGAR,  
    JEGAU GARAUTO,
    JEGVL VALGARAUTO,
    JEGUN UNITGARAUTO,
    JEPBT TXAPPEL,
    KAAATGTFA TXATTENTAT,
    JDTFF MNTCOTISPROV,
    JDACQ MNTCOTISACQUIS,
    JDPRO PRIMEPRO,
    KHWDEBP DATEDEBREGUL,
    KHWFINP DATEFINREGUL,
    KHXFRC TOPFRC,
    KDEFOR CODEFORMULE,
    KDEOPT CODEOPTION,
    KDEGARAN CODEGARANTIE,
    GADES LIBELLE
FROM KPRGUG
INNER JOIN KPRGU ON KHXKHWID = KHWID
AND KHXID = :rgGrId
AND KHXGARAN = '{AlbOpConstants.RCFrance}'
INNER JOIN YHPBASE ON PBIPB= KHXIPB AND PBALX = KHXALX AND PBTYP = KHXTYP AND PBAVN = KHXAVN 
INNER JOIN YhRTENT ON JDIPB= KHXIPB AND JDALX = KHXALX AND JDAVN = KHXAVN 
INNER JOIN hPENT ON KAAIPB = KHXIPB AND KAAALX = KHXALX AND KAATYP = KHXTYP AND KAAAVN = KHXAVN 
INNER JOIN YhRTRSQ ON JEIPB = KHXIPB AND JEALX = KHXALX  AND JERSQ = KHXRSQ AND JEAVN = KHXAVN 
INNER JOIN hPRSQ ON KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ = JERSQ AND KABAVN = JEAVN 
LEFT JOIN YYYYPAR RGTRSQ ON RGTRSQ.TCON = 'GENER' AND RGTRSQ.TFAM = 'TAXRG' AND RGTRSQ.TCOD = JERGT 
LEFT JOIN YYYYPAR RGTBASE ON RGTBASE.TCON = 'GENER' AND RGTBASE.TFAM = 'TAXRG' AND RGTBASE.TCOD = PBRGT 
INNER JOIN hPFOR ON  KHXIPB = KDAIPB AND KHXALX=KDAALX AND KHXTYP = KDATYP AND KHXFOR = KDAFOR AND KHXAVN = KDAAVN 
INNER JOIN hPGARAN ON KHXKDEID = KDEID AND KHXAVN = KDEAVN 
LEFT JOIN YYYYPAR TAXERSQ ON TAXERSQ.TCON = 'GENER' AND TAXERSQ.TFAM = 'TAXEC' AND TAXERSQ.TCOD = KDETAXCOD
INNER JOIN KGARAN ON KHXGARAN = GAGAR
LEFT JOIN YYYYPAR YPAREGUL ON YPAREGUL.TCON = 'PRODU' AND YPAREGUL.TFAM = 'JERUT' AND YPAREGUL.TCOD = JERUT";

            var result = DbBase.Settings.ExecuteList<SaisieGarantieInfosDto>(
                CommandType.Text,
                selection,
                new EacParameter[]
                {
                    new EacParameter("rgGrId", DbType.Int64) { Value = filter.GrId }
                });

            return result.FirstOrDefault();
        }

        public static List<SaisieGarantieInfosDto> GetGarantiesRCGroupHisto(GarantieFilter filter)
        {
            string Selection = $@"
SELECT CASE KHWMTF WHEN 'INFERIEURE' THEN 1 ELSE 0 END MOTIF_INF,
	KHXID ID,
    JDTFF MNTCOTISPROV,
    JDACQ MNTCOTISACQUIS,
    JDPRO PRIMEPRO,
    KHXCA1 PREVASSIETTE,
    CASE WHEN KHXCU1 = 'D' THEN KHXCP1 ELSE KHXCT1 END PREVTAUX,
    KHXCU1 PREVUNITE,
    KHXCX1 PREVCODTAXE, 
    KHXCA2 DEFASSIETTE,
    CASE WHEN KHXCU2 = 'D' THEN KHXCP2 ELSE KHXCT2 END DEFTAUX,
    KHXCU2 DEFUNITE,
    KHXCX2 DEFCODTAXE, 
    KHXECH MNTCOTISEMIS,
    KHXECT MNTTXEMIS,
    KHXEMH MNTFORCEEMIS,
    KHXEMT MNTFORCETX,
    KHXCOE COEFF,
    KHXMHC MNTREGULHT,
    KHXGRM ATTENTAT,
    KHXMHT MNTFORCECALC,
    KHXFR0 FORCE0,
    KHWDEBP DATEDEBREGUL,
    KHWFINP DATEFINREGUL,
    KHXFRC TOPFRC,
    KDEFOR CODEFORMULE,
    KDEOPT CODEOPTION,
    KDEGARAN CODEGARANTIE,
    GADES LIBELLE, 
    0 ISREADONLYRCUS
FROM KPRGUG
INNER JOIN KPRGU ON KHXKHWID = KHWID
AND KHXID = :rgGrId
AND KHXGARAN = '{AlbOpConstants.RCFrance}'
INNER JOIN YhpBASE ON PBIPB= KHXIPB AND PBALX = KHXALX AND PBTYP = KHXTYP AND PBAVN = KHXAVN 
INNER JOIN YhRTENT ON JDIPB= KHXIPB AND JDALX = KHXALX AND JDAVN = PBAVN 
INNER JOIN hPENT ON KAAIPB = KHXIPB AND KAAALX = KHXALX AND KAATYP = KHXTYP AND KAAAVN = PBAVN 
INNER JOIN YhRTRSQ ON JEIPB = KHXIPB AND JEALX=KHXALX  AND JERSQ = KHXRSQ AND JEAVN = PBAVN 
INNER JOIN hPRSQ ON KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ = JERSQ AND KABAVN = JEAVN 
LEFT JOIN YYYYPAR RGTRSQ ON RGTRSQ.TCON = 'GENER' AND RGTRSQ.TFAM = 'TAXRG' AND RGTRSQ.TCOD = JERGT 
LEFT JOIN YYYYPAR RGTBASE ON RGTBASE.TCON = 'GENER' AND RGTBASE.TFAM = 'TAXRG' AND RGTBASE.TCOD = PBRGT
INNER JOIN hPFOR ON  KHXIPB = KDAIPB AND KHXALX = KDAALX AND KHXTYP = KDATYP AND KHXFOR = KDAFOR AND KDAAVN = PBAVN 
INNER JOIN hPGARAN ON KHXKDEID = KDEID AND KDEAVN = PBAVN 
LEFT JOIN YYYYPAR TAXERSQ ON TAXERSQ.TCON = 'GENER' AND TAXERSQ.TFAM = 'TAXEC' AND TAXERSQ.TCOD = KDETAXCOD
INNER JOIN KGARAN ON KHXGARAN = GAGAR 
LEFT JOIN YYYYPAR YGARUT ON YGARUT.TCON = 'PRODU' AND YGARUT.TFAM='JERUT' AND YGARUT.TCOD = GARUT
UNION ALL
SELECT DISTINCT
    CASE KHWMTF WHEN 'INFERIEURE' THEN 1 ELSE 0 END MOTIF_INF,
	G.KHXID ID,
    JDTFF MNTCOTISPROV,
    JDACQ MNTCOTISACQUIS,
    JDPRO PRIMEPRO,
    G.KHXCA1 PREVASSIETTE,
    CASE WHEN G.KHXCU1 = 'D' THEN G.KHXCP1 ELSE G.KHXCT1 END PREVTAUX,
    G.KHXCU1 PREVUNITE,
    G.KHXCX1 PREVCODTAXE, 
    G.KHXCA2 DEFASSIETTE,
    CASE WHEN G.KHXCU2 = 'D' THEN G.KHXCP2 ELSE G.KHXCT2 END DEFTAUX,
    G.KHXCU2 DEFUNITE,
    G.KHXCX2 DEFCODTAXE, 
    G.KHXECH MNTCOTISEMIS,
    G.KHXECT MNTTXEMIS,
    G.KHXEMH MNTFORCEEMIS,
    G.KHXEMT MNTFORCETX,
    G.KHXCOE COEFF,
    G.KHXMHC MNTREGULHT,
    G.KHXGRM ATTENTAT,
    G.KHXMHT MNTFORCECALC,
    G.KHXFR0 FORCE0,
    KHWDEBP DATEDEBREGUL,
    KHWFINP DATEFINREGUL,
    G.KHXFRC TOPFRC,
    KDEFOR CODEFORMULE,
    KDEOPT CODEOPTION,
    KDEGARAN CODEGARANTIE,
    GADES LIBELLE,
    CASE WHEN TRIM ( G.KHXGARAN ) = '{AlbOpConstants.RCUSA}' THEN 
        (CASE WHEN 0 < ( SELECT COUNT(1) FROM HPGARTAR WHERE KDGGARAN = '{AlbOpConstants.USA_CAN}' AND ( KDGIPB , KDGALX , KDGOPT , KDGFOR, KDGAVN ) = ( G.KHXIPB , G.KHXALX , KDEOPT , G.KHXFOR, G.KHXAVN ) ) AND ((KDEASVALA > 0 AND GT.KDGPRIVALA > 0) OR GT.KDGPRIMPRO > 0) THEN 0 ELSE 1 END) 
    ELSE 0 END ISREADONLY 
FROM KPRGUG G
INNER JOIN KPRGU ON G.KHXKHWID = KHWID
AND G.KHXGARAN IN ('{AlbOpConstants.RCExport}', '{AlbOpConstants.RCUSA}')
INNER JOIN KPRGUG FRG ON G.KHXDEBP = FRG.KHXDEBP
AND G.KHXFINP = FRG.KHXFINP
AND FRG.KHXID = :rgGrId
AND G.KHXKHWID = FRG.KHXKHWID
AND G.KHXRSQ = FRG.KHXRSQ
AND G.KHXFOR = FRG.KHXFOR 
AND G.KHXAVN = FRG.KHXAVN 
INNER JOIN YHPBASE ON PBIPB = G.KHXIPB AND PBALX = G.KHXALX AND PBTYP = G.KHXTYP AND PBAVN = G.KHXAVN 
INNER JOIN YHRTENT ON JDIPB = G.KHXIPB AND JDALX = G.KHXALX AND JDAVN = G.KHXAVN 
INNER JOIN HPENT ON KAAIPB = G.KHXIPB AND KAAALX = G.KHXALX AND KAATYP = G.KHXTYP AND KAAAVN = G.KHXAVN 
INNER JOIN YhRTRSQ ON JEIPB = G.KHXIPB AND JEALX = G.KHXALX  AND JERSQ = G.KHXRSQ AND JEAVN = G.KHXAVN 
INNER JOIN HPRSQ ON KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ = JERSQ AND KABAVN = JEAVN 
LEFT JOIN YYYYPAR RGTRSQ ON RGTRSQ.TCON = 'GENER' AND RGTRSQ.TFAM = 'TAXRG' AND RGTRSQ.TCOD = JERGT 
LEFT JOIN YYYYPAR RGTBASE ON RGTBASE.TCON = 'GENER' AND RGTBASE.TFAM = 'TAXRG'  AND RGTBASE.TCOD = PBRGT 
INNER JOIN HPFOR ON G.KHXIPB = KDAIPB AND G.KHXALX = KDAALX AND G.KHXTYP = KDATYP AND G.KHXFOR = KDAFOR AND KDAAVN = G.KHXAVN 
INNER JOIN HPGARAN ON G.KHXKDEID = KDEID AND KDEAVN = G.KHXAVN 
LEFT JOIN HPGARTAR GT ON KDGKDEID = KDEID AND KDGAVN = G.KHXAVN 
LEFT JOIN YYYYPAR TAXERSQ ON TAXERSQ.TCON = 'GENER' AND TAXERSQ.TFAM = 'TAXEC' AND TAXERSQ.TCOD = KDETAXCOD 
INNER JOIN KGARAN ON G.KHXGARAN = GAGAR 
LEFT JOIN YYYYPAR YGARUT ON YGARUT.TCON = 'PRODU' AND YGARUT.TFAM = 'JERUT' AND YGARUT.TCOD = GARUT";

            var result = DbBase.Settings.ExecuteList<SaisieGarantieInfosDto>(
                CommandType.Text,
                Selection,
                new EacParameter[]
                {
                    new EacParameter("rgGrId", DbType.Int64) { Value = filter.GrId }
                });

            return result;
        }

        public static List<string> FindOtherRCThanFR(long lotId, int rsqNum) {
            string selection = $@"
SELECT KHVKDEGAR FROM KPSELW 
WHERE KHVPERI = '{PerimetreSelectionRegul.Garantie.AsCode()}' AND KHVID = :lotId  AND KHVRSQ = :rsqNum 
AND KHVKDEGAR IN ('{AlbOpConstants.RCExport}', '{AlbOpConstants.RCUSA}')";

            return DbBase.Settings.ExecuteStringList(
                CommandType.Text,
                selection,
                new EacParameter("lotId", DbType.Int64) { Value = lotId },
                new EacParameter("rsqNum", DbType.Int32) { Value = rsqNum });
        }

        public static void UpdateInfosCotisRegulGarantie(SaisieGarantieRCValuesDto entry) {
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                "UPDATE KPRGUG SET KHXCOE = :coe, KHXMHT = 0, KHXFR0 = 'N' WHERE KHXID = :reguleGarId",
                new EacParameter("coe", DbType.Decimal) { Value = (decimal)entry.Definitif.Coefficient },
                new EacParameter("reguleGarId", DbType.Decimal) { Value = (decimal)entry.Definitif.Id });
        }

        public static void UpdateInfosCotisRegulGarantieForValidation(SaisieGarantieRCValuesDto entry, double montantTxForcee) {
            const string UpdateQuery = @"
UPDATE KPRGUG
SET KHXSIT = 'V', KHXMHC = :mhc, KHXFRC = :frc, KHXFR0 = :fr0, KHXMHT = :mht, KHXMTX = :mtx, KHXMTT = :mtt, KHXCNH = :cnh,
	KHXCNT = :cnt, KHXGRM = :grm, KHXEMH = :emh, KHXEMT = :emt, KHXCOE = :coe, KHXCA1 = :ca1, KHXCT1 = :ct1, KHXCU1 = :cu1, 
    KHXCP1 = :cp1, KHXCX1 = :cx1, KHXCA2 = :ca2, KHXCT2 = :ct2, KHXCU2 = :cu2, KHXCP2 = :cp2, KHXCX2 = :cx2
WHERE KHXID = :reguleGarId";

            var montantHT = entry.ComputeRegulAmount();
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                UpdateQuery,
                new EacParameter("mhc", DbType.Int32) { Value = montantHT },
                new EacParameter("frc", DbType.AnsiStringFixedLength) { Value = entry.RegulForcee != montantHT && entry.RegulForcee != 0 ? "O" : "N" },
                new EacParameter("fr0", DbType.AnsiStringFixedLength) { Value = entry.IsRegulZero ? "O" : "N" },
                new EacParameter("mht", DbType.Int32) { Value = entry.IsRegulZero ? 0 : ((entry.RegulForcee != montantHT && entry.RegulForcee != 0) && entry.IsRegulZero) ? entry.RegulForcee : montantHT },
                new EacParameter("mtx", DbType.Int32) { Value = montantTxForcee },
                new EacParameter("mtt", DbType.Int32) { Value = entry.RegulForcee + montantTxForcee },
                new EacParameter("cnh", DbType.Int32) { Value = 0 },
                new EacParameter("cnt", DbType.Int32) { Value = 0 },
                new EacParameter("grm", DbType.Int32) { Value = entry.Attentat },
                new EacParameter("emh", DbType.Decimal) { Value = (decimal)entry.CotisationForcee },
                new EacParameter("emt", DbType.Decimal) { Value = (decimal)entry.TaxesCotisationForcee },
                new EacParameter("coe", DbType.Decimal) { Value = (decimal)entry.Definitif.Coefficient },
                new EacParameter("ca1", DbType.Int32) { Value = entry.Previsionnel.BasicValues.Assiette },
                new EacParameter("ct1", DbType.Int32) { Value = entry.Previsionnel.BasicValues.Unite.Code == "D" ? 0 : entry.Previsionnel.BasicValues.TauxMontant },
                new EacParameter("cu1", DbType.AnsiStringFixedLength) { Value = entry.Previsionnel.BasicValues.Unite.Code ?? string.Empty },
                new EacParameter("cp1", DbType.Int32) { Value = entry.Previsionnel.CalculAssiette },
                new EacParameter("cx1", DbType.AnsiStringFixedLength) { Value = entry.Previsionnel.BasicValues.CodeTaxes.Code ?? string.Empty },
                new EacParameter("ca2", DbType.Int32) { Value = entry.Definitif.BasicValues.Assiette },
                new EacParameter("ct2", DbType.Int32) { Value = entry.Definitif.BasicValues.Unite.Code == "D" ? 0 : entry.Definitif.BasicValues.TauxMontant },
                new EacParameter("cu2", DbType.AnsiStringFixedLength) { Value = entry.Definitif.BasicValues.Unite.Code ?? string.Empty },
                new EacParameter("cp2", DbType.Int32) { Value = entry.Definitif.CalculAssiette },
                new EacParameter("cx2", DbType.AnsiStringFixedLength) { Value = entry.Definitif.BasicValues.CodeTaxes.Code ?? string.Empty },
                new EacParameter("reguleGarId", DbType.Decimal) { Value = (decimal)entry.Definitif.Id });
        }

        /* Reload Ecran saisie */
        public static SaisieInfoRegulPeriodDto ReloadSaisieGarRegul(string codeContrat, string version, string type, string codeAvenant, string idregulgar, string codeRsq,
            string assiettePrev, string valeurPrev, string unitePrev, string codetaxePrev, string assietteDef, string valeurDef, string uniteDef, string codetaxeDef,
            string cotisForceHT, string cotisForceTaxe, string coeff) {
            EacParameter[] paramUpd = new EacParameter[4];
            paramUpd[0] = new EacParameter("emh", DbType.Int32);
            paramUpd[0].Value = !string.IsNullOrEmpty(cotisForceHT) ? Convert.ToDouble(cotisForceHT) : 0;
            paramUpd[1] = new EacParameter("emt", DbType.Int32);
            paramUpd[1].Value = !string.IsNullOrEmpty(cotisForceTaxe) ? Convert.ToDouble(cotisForceTaxe) : 0;
            paramUpd[2] = new EacParameter("coe", DbType.Int32);
            paramUpd[2].Value = !string.IsNullOrEmpty(coeff) ? Convert.ToDouble(coeff) : 0;
            paramUpd[3] = new EacParameter("reguleGarId", DbType.Int32);
            paramUpd[3].Value = !string.IsNullOrEmpty(idregulgar) ? Convert.ToInt32(idregulgar) : 0;

            //string sqlUpd = @"UPDATE KPRGUG SET KHXEMH = :emh, KHXEMT = :emt, KHXCOE = :coe where khxid = :reguleGarId";
            // sab 04/10/2016 bug 2172
            string sqlUpd = @"UPDATE KPRGUG SET KHXEMH = :emh, KHXEMT = :emt, KHXCOE = :coe ,KHXMHT= 0,KHXFR0='N'
                                WHERE KHXID = :reguleGarId";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpd, paramUpd);

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("idRegulGar", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(idregulgar) ? Convert.ToInt32(idregulgar) : 0;
            string sql = @"SELECT DISTINCT 
CASE KHWMTF WHEN 'INFERIEURE' THEN 1 ELSE 0 END MOTIF_INF, KHXGARAN TITREGAR_STD,KHXDEBP DATEDEB_STD,KHXFINP DATEFIN_STD,KHXRSQ CODERSQ_STD, KABDESC LIBRSQ_STD,
CASE WHEN JERGT = '' THEN PBRGT ELSE JERGT END  CODERGT_STD,CASE WHEN JERGT = '' THEN RGTBASE.TPLIL ELSE RGTRSQ.TPLIL END  LIBRGT_STD,
KHXFOR CODEFOR_STD , KDADESC LIBFOR_STD , KDAALPHA LETTREFOR_STD, IFNULL(G.KDETAXCOD, HG.KDETAXCOD) CODETAXE_STD,TAXERSQ.TPLIB LIBTAXE_STD, GATRG TYPEREGULGAR_STD, YGATRG.TPLIL LIBREGULGAR_STD,  
JEGAU GARAUTO_STD, JEGVL VALGARAUTO_STD, JEGUN UNITGARAUTO_STD, JDTFP TXAPPEL_STD, KAAATGTFA TXATTENTAT_STD, JDTFF MNTCOTISPROV_STD, JDACQ MNTCOTISACQUIS_STD,
KHXCA1 PREVASSIETTE_STD, CASE WHEN KHXCU1 = 'D' THEN KHXCP1 ELSE KHXCT1 END PREVTAUX_STD, KHXCU1 PREVUNITE_STD, KHXCX1 PREVCODTAXE_STD, 
KHXCA2 DEFASSIETTE_STD, CASE WHEN KHXCU2 = 'D' THEN KHXCP2 ELSE KHXCT2 END DEFTAUX_STD, KHXCU2 DEFUNITE_STD, KHXCX2 DEFCODTAXE_STD, 
KHXECH MNTCOTISEMIS_STD , KHXECT MNTTXEMIS_STD,KHXEMH MNTFORCEEMIS_STD,KHXEMT MNTFORCETX_STD, KHXCOE COEFF_STD ,
KHXMHC MNTREGULHT_STD,KHXGRM ATTENTAT_STD,KHXMHT MNTFORCECALC_STD,KHXFR0 FORCE0_STD,KHWDEBP DATEDEBREGUL,KHWFINP DATEFINREGUL,
JEPBA NBYEARRSQ_PB ,JEPBR TXRISTRSQ_PB,JEPBS SEUILSP_PB, JEPBP TXCOTISRETRSQ_PB, ((KHXKTD - KHXKEA) * (-1)) RISTANTICIPEE_PB,
KHXKEA COTISEMISE_PB, KHXPBT TXAPPELPBNS_PB, 
CAST((CASE WHEN KHXPBT = 0 THEN KHXKEA ELSE (KHXKEA * 100 / KHXPBT) END) AS DECIMAL(11, 2)) COTISDUE_PB,
KHXPEI NBYEARREGUL_PB  ,
CAST(((CAST((CASE WHEN KHXPBT = 0 THEN KHXKEA ELSE (KHXKEA * 100 / KHXPBT) END) AS DECIMAL(11, 2)) * KHXPBP) / 100) AS DECIMAL(11,2)) COTISRETENUE_PB,
KHXPBP TXCOTISRET_PB,KHXSIP CHARGESIN_PB,
CAST(((KHXKTD * KHXPBP / 100) - KHXSIP) * (KHXPBR / 100) AS DECIMAL(11, 2)) PBNS_PB, 
KHXPBR TXRISTREGUL_PB, KHXRIA RISTANTICIPEEREGUL_PB,KHXFRC TOPFRC,KHWMTF MOTIF,JDPRO PRIMEPRO,GATRG TYPEGRILLE,JDTMC MNTCALCULREF
FROM KPRGUG
INNER JOIN KPRGU ON KHWIPB= KHXIPB AND KHWALX = KHXALX AND KHWTYP= KHXTYP AND KHWID= KHXKHWID
INNER JOIN YPOBASE ON PBIPB= KHXIPB AND PBALX = KHXALX AND PBTYP= KHXTYP
INNER JOIN YPRTENT ON JDIPB= KHXIPB AND JDALX = KHXALX 
INNER JOIN KPENT ON KAAIPB = KHXIPB AND KAAALX = KHXALX AND KAATYP = KHXTYP
INNER JOIN YPRTRSQ ON JEIPB=KHXIPB AND JEALX=KHXALX  AND JERSQ=KHXRSQ 
INNER JOIN KPRSQ ON KABIPB=JEIPB AND KABALX = JEALX AND KABRSQ=JERSQ
LEFT JOIN YYYYPAR RGTRSQ ON RGTRSQ.TCON = 'GENER' AND RGTRSQ.TFAM = 'TAXRG'  AND RGTRSQ.TCOD = JERGT
LEFT JOIN YYYYPAR RGTBASE ON RGTBASE.TCON = 'GENER' AND RGTBASE.TFAM = 'TAXRG'  AND RGTBASE.TCOD = PBRGT
INNER JOIN KPFOR ON  KHXIPB = KDAIPB AND KHXALX=KDAALX AND KHXTYP = KDATYP AND KHXFOR=KDAFOR
LEFT JOIN KPGARAN G ON KHXKDEID = G.KDEID
LEFT JOIN HPGARAN HG ON KHXKDEID = HG.KDEID
LEFT JOIN YYYYPAR TAXERSQ ON TAXERSQ.TCON = 'GENER' AND TAXERSQ.TFAM = 'TAXEC' AND TAXERSQ.TCOD = IFNULL(G.KDETAXCOD, HG.KDETAXCOD) 
INNER JOIN KGARAN ON KHXGARAN=GAGAR
LEFT JOIN YYYYPAR YGATRG ON YGATRG.TCON='PRODU' AND YGATRG.TFAM='GATRG' AND YGATRG.TCOD= GATRG
WHERE KHXID = :idRegulGar";

            //attente champ garut
            var result = DbBase.Settings.ExecuteList<SaisieGarInfoDto>(CommandType.Text, sql, param);
            /* Lancement du AS400 KA039CL calcul montantht et taxe Prev    */
            string returnPrev = CommonRepository.GetMontantHtTaxe(codeContrat, version, type, codeAvenant, codeRsq, assiettePrev, valeurPrev, unitePrev, codetaxePrev);
            /* Lancement du AS400 KA039CL calcul montantht et taxe Def    */
            string returnDef = CommonRepository.GetMontantHtTaxe(codeContrat, version, type, codeAvenant, codeRsq, assietteDef, valeurDef, uniteDef, codetaxeDef);
            var toReturn = new SaisieInfoRegulPeriodDto();
            var model = new SaisieGarInfoDto();
            if (result != null && result.Any()) {
                model = result.FirstOrDefault();
                model.PrevMntHt_STD = double.Parse(returnPrev.Split('_')[0]);
                model.PrevTax_STD = double.Parse(returnPrev.Split('_')[1]);

                if (returnDef.Split('_')[0] == "ERROR") {
                    model.ErrorStr = returnDef.Split('_')[1];
                    model.DefVmntHt_STD = 0;
                    model.DefVtax_STD = 0;
                }
                else {
                    model.DefVmntHt_STD = double.Parse(returnDef.Split('_')[0]);
                    model.DefVtax_STD = double.Parse(returnDef.Split('_')[1]);
                }

                model.DefAssiette_STD = double.Parse(assietteDef);
                model.DefCodTaxe_STD = codetaxeDef;
                model.DefUnite_STD = uniteDef;
                model.DefTaux_STD = double.Parse(valeurDef);
                if (model.Coef_STD == 0) {
                    if (model.MntForceEmis_STD != 0) {
                        model.MntRegulHt_STD = model.DefVmntHt_STD - model.MntForceEmis_STD;
                    }
                    else {
                        model.MntRegulHt_STD = model.DefVmntHt_STD - model.MntCotisEmis_STD;
                    }
                }
                else {
                    if (model.MntForceEmis_STD != 0) {
                        model.MntRegulHt_STD = (model.DefVmntHt_STD - model.MntForceEmis_STD) * model.Coef_STD / 100;
                    }
                    else {
                        model.MntRegulHt_STD = (model.DefVmntHt_STD - model.MntCotisEmis_STD) * model.Coef_STD / 100;
                    }
                }
                //Si motif régularisation "INFERIEURE" => montant de régul = 0
                if (model.Motif_Inf == 1)
                    model.MntRegulHt_STD = 0;
                toReturn.LignePeriodRegul = model;
            }
            /* Les LISTES UNITES ET CODES TAXE DEFINITIF */
            toReturn.UnitesDef = GetListUnites();
            toReturn.CodesTaxesDef = GetListCodesTaxes();

            return toReturn;
        }
        public static string GetMhtTaxePrevisionnel(string idRegulGar, string codeAvenant) {

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("idRegulGar", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(idRegulGar) ? Convert.ToInt32(idRegulGar) : 0;

            string sql = @"SELECT KHXIPB CODECONTRAT, KHXTYP TYPE , KHXALX VERSION , KHXRSQ CODERSQ, KHXCA1 ASSIETTE , 
                                CASE WHEN KHXCU1 = 'D' THEN KHXCP1 ELSE KHXCT1 END VALEUR , KHXCU1 UNITE, KHXCX1 CODETAXE 
                            FROM KPRGUG WHERE KHXID = :idRegulGar";
            var result = DbBase.Settings.ExecuteList<SaisieGarInfoDto>(CommandType.Text, sql, param);
            if (result != null && result.Any()) {
                var retour = result.FirstOrDefault();
                return CommonRepository.GetMontantHtTaxe(retour.codeContrat, retour.version.ToString(), retour.type, codeAvenant, retour.codeRsq.ToString(), retour.assiette.ToString(), retour.val.ToString(), retour.unite, retour.codeTaxe);
            }
            else {
                return string.Empty;
            }
        }
        public static string GetMhtTaxeDefinitif(string idRegulGar, string codeAvenant) {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("idRegulGar", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(idRegulGar) ? Convert.ToInt32(idRegulGar) : 0;

            //            string sql = @"SELECT KHXIPB CODECONTRAT, KHXTYP TYPE , KHXALX VERSION , KHXRSQ CODERSQ, KHXCA2 ASSIETTE , 
            //                                CASE WHEN KHXCU2 = 'D' THEN KHXCP2 ELSE KHXCT2 END VALEUR , 
            //                                KHXCU2 UNITE, KHXCX2 CODETAXE 
            //                            FROM KPRGUG WHERE KHXID = :idRegulGar";

            //2017-01-31 : correction bug 2243 ==> NON PRISE EN COMPTE DE CETTE CORRECTION
            string sql = @"SELECT KHXIPB CODECONTRAT, KHXTYP TYPE , KHXALX VERSION , KHXRSQ CODERSQ, KHXCA2 ASSIETTE , 
            	                            CASE WHEN NULLIF(KHXCU2, '') IS NULL  
            		                            THEN CASE WHEN KHXCU1 = 'D' THEN KHXCP1 ELSE KHXCT1 END 
            		                            ELSE CASE WHEN KHXCU2 = 'D' THEN KHXCP2 ELSE KHXCT2 END 
            	                            END VALEUR , 
            	                            IFNULL(NULLIF(KHXCU2, ''), KHXCU1) UNITE, 
            	                            CASE WHEN NULLIF(KHXCU2, '') IS NULL THEN KHXCX1 ELSE KHXCX2 END CODETAXE 
                                        FROM KPRGUG WHERE KHXID = :idRegulGar";

            var result = DbBase.Settings.ExecuteList<SaisieGarInfoDto>(CommandType.Text, sql, param);
            if (result != null && result.Any()) {
                var retour = result.FirstOrDefault();
                return CommonRepository.GetMontantHtTaxe(retour.codeContrat, retour.version.ToString(), retour.type, codeAvenant, retour.codeRsq.ToString(), retour.assiette.ToString(), retour.val.ToString(), retour.unite, retour.codeTaxe);
            }
            else {
                return string.Empty;
            }

        }
        /// <summary>
        /// Supprime une régularisation
        /// </summary>
        public static List<LigneRegularisationDto> DeleteRegule(string codeContrat, string version, string type, string reguleId) {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_REGULEID", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt16(reguleId) : 0;

            return DbBase.Settings.ExecuteList<LigneRegularisationDto>(CommandType.StoredProcedure, "SP_DELETEREGULE", param);
        }

        public static string GetMotifRegularisation(long reguleId) {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("reguleId", DbType.Int64) { Value = reguleId };

            string sql = @"SELECT KHWMTF MOTIF, KHWAVN NUM_AVT, KHWAVN   NUM_INTERNE_AVT
                            FROM KPRGU
                            WHERE KHWID = :reguleId";

            var result = DbBase.Settings.ExecuteList<AvenantRegularisationDto>(CommandType.Text, sql, param);
            if (result != null && result.Any()) {
                return result.FirstOrDefault().MotifAvt;
            }
            return string.Empty;
        }

        public static string ValidSaisiePeriodRegule(string codeContrat, string version, string type, string codeAvn, string codeRsq, string reguleGarId, string typeRegule, SaisieGarInfoDto modelDto) {
            var toReturn = CommonRepository.GetMontantHtTaxe(codeContrat, version, type, codeAvn, codeRsq, "0", modelDto.MntForceCalc_STD.ToString(), "D", string.Empty);
            if (toReturn.Split('_')[0] == "ERROR") {
                return toReturn.Split('_')[1];
            }
            var mtx = Convert.ToDouble(toReturn.Split('_')[1]);

            switch (typeRegule) {
                #region PB/BNS
                case "PB":
                case "BNS":
                    var asv = modelDto.TxAppelPbns_PB != 0 ? modelDto.CotisEmise_PB * 100 / modelDto.TxAppelPbns_PB : modelDto.CotisEmise_PB;
                    var ktd = asv * modelDto.TxCotisRetRsq_PB / 100;
                    var ris = ((asv * modelDto.TxCotisRet_PB / 100) - modelDto.ChargeSin_PB) * (modelDto.TxRistRegul_PB / 100);
                    var ria = (asv - modelDto.CotisEmise_PB) * (-1);
                    var mhc = ris - ria;

                    EacParameter[] paramPBNS = new EacParameter[20];
                    paramPBNS[0] = new EacParameter("sit", DbType.AnsiStringFixedLength);
                    paramPBNS[0].Value = "V";
                    paramPBNS[1] = new EacParameter("mhc", DbType.Int32);
                    paramPBNS[1].Value = mhc;
                    paramPBNS[2] = new EacParameter("frc", DbType.AnsiStringFixedLength);
                    paramPBNS[2].Value = modelDto.MntForceCalc_STD != mhc && modelDto.MntForceCalc_STD != 0 ? "O" : "N";
                    paramPBNS[3] = new EacParameter("fr0", DbType.AnsiStringFixedLength);
                    paramPBNS[3].Value = modelDto.MntForceCalc_STD != mhc ? modelDto.Force0_STD : "N";
                    paramPBNS[4] = new EacParameter("mht", DbType.Int32);
                    //ZBO 12092016 : Correction suite au point du 12092016 bug 2143
                    //paramPBNS[4].Value = modelDto.MntForceCalc_STD != mhc && modelDto.MntForceCalc_STD != 0 ? modelDto.MntForceCalc_STD : mhc;
                    paramPBNS[4].Value = modelDto.Force0_STD == "O" ? 0 : modelDto.MntForceCalc_STD != mhc && modelDto.MntForceCalc_STD != 0 ? modelDto.MntForceCalc_STD : mhc;
                    //paramPBNS[4].Value = modelDto.MntForceCalc_STD;
                    paramPBNS[5] = new EacParameter("mtx", DbType.Int32);
                    paramPBNS[5].Value = mtx;
                    paramPBNS[6] = new EacParameter("mtt", DbType.Int32);
                    paramPBNS[6].Value = modelDto.MntForceCalc_STD + mtx;
                    paramPBNS[7] = new EacParameter("grm", DbType.Int32);
                    paramPBNS[7].Value = modelDto.Attentat_STD;
                    paramPBNS[8] = new EacParameter("emh", DbType.Int32);
                    paramPBNS[8].Value = modelDto.CotisEmise_PB;
                    paramPBNS[9] = new EacParameter("kea", DbType.Int32);
                    paramPBNS[9].Value = modelDto.CotisEmise_PB;
                    paramPBNS[10] = new EacParameter("pei", DbType.Int32);
                    paramPBNS[10].Value = modelDto.NbYearRegul_PB;
                    paramPBNS[11] = new EacParameter("pbp", DbType.Int32);
                    paramPBNS[11].Value = modelDto.TxCotisRet_PB;
                    paramPBNS[12] = new EacParameter("ktd", DbType.Int32);
                    paramPBNS[12].Value = ktd;
                    paramPBNS[13] = new EacParameter("asv", DbType.Int32);
                    paramPBNS[13].Value = asv;
                    paramPBNS[14] = new EacParameter("pbt", DbType.Int32);
                    paramPBNS[14].Value = modelDto.TxAppelPbns_PB;
                    paramPBNS[15] = new EacParameter("sip", DbType.Int32);
                    paramPBNS[15].Value = modelDto.ChargeSin_PB;
                    paramPBNS[16] = new EacParameter("ris", DbType.Int32);
                    paramPBNS[16].Value = ris;
                    paramPBNS[17] = new EacParameter("pbr", DbType.Int32);
                    paramPBNS[17].Value = modelDto.TxRistRegul_PB;
                    paramPBNS[18] = new EacParameter("ria", DbType.Int32);
                    paramPBNS[18].Value = ria;
                    paramPBNS[19] = new EacParameter("reguleGarId", DbType.Int32);
                    paramPBNS[19].Value = !string.IsNullOrEmpty(reguleGarId) ? Convert.ToInt32(reguleGarId) : 0;

                    string sqlPBNS = @"
UPDATE KPRGUG
SET KHXSIT = :sit, KHXMHC = :mhc, KHXFRC = :frc, KHXFR0 = :fr0, KHXMHT = :mht, KHXMTX = :mtx, KHXMTT = :mtt, 
    KHXGRM = :grm, KHXEMH = :emh, KHXEMT = :emt, KHXKEA = :kea, KHXPEI = :pei, KHXPBP = :pbp, KHXKTD = :ktd,
    KHXASV = :asv, KHXPBT = :pbt, KHXSIP = :sip, KHXPBR = :pbr, KHXRIA = :ria
WHERE KHXID = :reguleGarId";

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlPBNS, paramPBNS);

                    break;
                #endregion
                #region Default
                default:
                    var parameters = new EacParameter[]
                    {
                        new EacParameter("sit", DbType.AnsiStringFixedLength) { Value = "V" },
                        new EacParameter("mhc", DbType.Int32) { Value = modelDto.MntRegulHt_STD },
                        new EacParameter("frc", DbType.AnsiStringFixedLength) { Value = modelDto.MntForceCalc_STD != modelDto.MntRegulHt_STD && modelDto.MntForceCalc_STD != 0 ? "O" : "N" },
                        new EacParameter("fr0", DbType.AnsiStringFixedLength) { Value = modelDto.MntForceCalc_STD != modelDto.MntRegulHt_STD ? modelDto.Force0_STD : "N" },

                        //ZBO 12092016 : Correction suite au point du 12092016 bug 2143    
                        new EacParameter("mht", DbType.Int32) { Value = modelDto.Force0_STD == "O" ? 0 : ((modelDto.MntForceCalc_STD != modelDto.MntRegulHt_STD && modelDto.MntForceCalc_STD != 0) && modelDto.Force0_STD == "N") ? modelDto.MntForceCalc_STD : modelDto.MntRegulHt_STD },
                        new EacParameter("mtx", DbType.Int32) { Value = mtx },
                        new EacParameter("mtt", DbType.Int32) { Value = modelDto.MntForceCalc_STD + mtx },
                        new EacParameter("cnh", DbType.Int32) { Value = 0 },
                        new EacParameter("cnt", DbType.Int32) { Value = 0 },
                        new EacParameter("grm", DbType.Int32) { Value = modelDto.Attentat_STD },
                        new EacParameter("emh", DbType.Int32) { Value = modelDto.MntForceEmis_STD },
                        new EacParameter("emt", DbType.Int32) { Value = modelDto.MntForceTx_STD },
                        new EacParameter("coe", DbType.Int32) { Value = modelDto.Coef_STD },
                        new EacParameter("ca1", DbType.Int32) { Value = modelDto.PrevAssiette_STD },
                        new EacParameter("cu1", DbType.AnsiStringFixedLength) { Value = modelDto.PrevUnite_STD },
                        new EacParameter("cx1", DbType.AnsiStringFixedLength) { Value = modelDto.PrevCodTaxe_STD },
                        new EacParameter("ca2", DbType.Int32) { Value = modelDto.DefAssiette_STD },
                        new EacParameter("ct2", DbType.Int32) { Value = modelDto.DefUnite_STD == "D" ? 0 : modelDto.DefTaux_STD },
                        new EacParameter("cu2", DbType.AnsiStringFixedLength) { Value = modelDto.DefUnite_STD },
                        new EacParameter("cp2", DbType.Int32) { Value = modelDto.DefVmntHt_STD },
                        new EacParameter("cx2", DbType.AnsiStringFixedLength) { Value = modelDto.DefCodTaxe_STD },
                        new EacParameter("reguleGarId", DbType.Int32) { Value = string.IsNullOrEmpty(reguleGarId) ? 0 : Convert.ToInt32(reguleGarId) }
                    };

                    string sqlStd = @"
UPDATE KPRGUG
SET KHXSIT = :sit, KHXMHC = :mhc, KHXFRC = :frc, KHXFR0 = :fr0, KHXMHT = :mht, KHXMTX = :mtx, KHXMTT = :mtt, KHXCNH = :cnh,
	KHXCNT = :cnt, KHXGRM = :grm, KHXEMH = :emh, KHXEMT = :emt, KHXCOE = :coe, KHXCA1 = :ca1, KHXCU1 = :cu1, 
    KHXCX1 = :cx1, KHXCA2 = :ca2, KHXCT2 = :ct2, KHXCU2 = :cu2, KHXCP2 = :cp2, KHXCX2 = :cx2
    WHERE KHXID = :reguleGarId";

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlStd, parameters);

                    DbBase.Settings.ExecuteNonQuery(
                        CommandType.Text,
                        @"UPDATE KPRGUR SET KILSIT = 'V' WHERE KILKHWID = ( SELECT KHXKHWID FROM KPRGUG WHERE KHXID = :reguleGarId AND KHXSIT = 'V' FETCH FIRST 1 ROWS ONLY ) AND KILRSQ = :rsq",
                        new EacParameter("reguleGarId", DbType.Int32) { Value = string.IsNullOrEmpty(reguleGarId) ? 0 : Convert.ToInt32(reguleGarId) },
                        new EacParameter("rsq", DbType.Int32) { Value = string.IsNullOrEmpty(codeRsq) ? 0 : Convert.ToInt32(codeRsq) });

                    break;
                    #endregion
            }

            return string.Empty;
        }

        public static ConfirmSaisieReguleDto GetPopupConfirm(string reguleGarId) {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("reguleGarId", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(reguleGarId) ? Convert.ToInt32(reguleGarId) : 0;

            string sql = @"SELECT KHXGARAN CODEGAR, GADES LIBGAR, KHXMHT MNTHT, KHXGRM ATTENTATHT
                            FROM KPRGUG
                                INNER JOIN KGARAN ON GAGAR = KHXGARAN
                            WHERE KHXID = :reguleGarId";

            var result = DbBase.Settings.ExecuteList<ConfirmSaisieReguleDto>(CommandType.Text, sql, param);
            if (result != null && result.Any()) {
                return result.FirstOrDefault();
            }
            return null;
        }

        public static LigneRegularisationDto GetLastValidInfoAvnRegul(string codeContrat, string version, string type) {
            LigneRegularisationDto result = new LigneRegularisationDto();
            String codeAvn = "0";
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            // var list = GetListeRegularisation(codeContrat, version, type);

            var sql1 = @"SELECT PBETA ETAT, PBAVN AVN_CREA FROM YPOBASE WHERE PBIPB = :codeContrat";
            var param1 = new List<EacParameter> {
                new EacParameter("codeContrat", DbType.AnsiStringFixedLength) { Value = codeContrat.PadLeft(9, ' ')}
            };

            var resultat = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql1, param1).First();
            if (resultat.Etat.Equals("V")) {
                codeAvn = resultat.AvenantCrea.ToString();
            }
            else {
                sql1 = @"SELECT PBETA ETAT, PBAVN AVN_CREA FROM YHPBASE WHERE PBIPB = :codeContrat ORDER BY PBAVN DESC FETCH FIRST 1 ROWS ONLY";
                var resultatHisto = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql1, param1);
                if (resultatHisto != null && resultatHisto.Any())
                    codeAvn = resultatHisto.First().AvenantCrea.ToString();
            };
            result = GetLastRegularisation(codeContrat.PadLeft(9, ' '), version, type, codeAvn);


            return result;
        }

        public static void ValidRegul(string codeContrat, string version, string reguleId, string primeId, string user) {
            EacParameter[] param = new EacParameter[6];
            param[0] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[2].Value = user;
            param[3] = new EacParameter("P_DATENOW", DbType.Int32);
            param[3].Value = AlbConvert.ConvertDateToInt(DateTime.Now);
            param[4] = new EacParameter("P_NUMPRIME", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(primeId) ? Convert.ToInt32(primeId) : 0;
            param[5] = new EacParameter("P_REGULEID", DbType.Int32);
            param[5].Value = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_VALIDREGULE", param);


            //string sql = @"UPDATE KPRGU SET KHWETA = 'V', KHWMAJU = :user, KHWMAJD = :dateNow, KHWIPK = :numPrime WHERE KHWID = :reguleId";

            //DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static bool IsCoassurance(string codeOffre, string version, string type, string codeAvn) {
            var param = new List<EacParameter>() {
                 new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },
                 new EacParameter("version", DbType.Int32){ Value = Convert.ToInt32(version) },
                 new EacParameter("type", DbType.AnsiStringFixedLength) { Value = type }
            };

            var sql = "SELECT COUNT(*) NBLIGN FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :TYPE AND PBNPL IN ('C','D')";

            return CommonRepository.ExistRowParam(sql, param);
        }

        public static RegularisationBandeauContratDto GetBandeauContratInfo(string codeOffre, string version, string type, bool lightVersion) {
            RegularisationBandeauContratDto contratInfo = null;

            if (lightVersion)
                contratInfo = GetBandeauLightContratInfo(codeOffre, version, type);
            else
                contratInfo = GetBandeauDetailedContratInfo(codeOffre, version, type);

            return contratInfo;
        }

        public static string ValidateRisquesContrat(RegularisationContext context) {
            if (!context.ComputeDone) {
                if (!context.ValidationDone) {
                    return "Le calcul n'a pas encore été effectué. Impossible de valider la regularisation de ce contrat";
                }
            }
            else {
                var parameters = new EacParameter[] { new EacParameter("rgid", DbType.Int64) { Value = context.RgId } };
                DbBase.Settings.ExecuteNonQuery(
                    CommandType.Text,
                    "UPDATE KPRGUR SET KILSIT = 'V' WHERE KILKHWID = :rgid",
                    parameters);

                DbBase.Settings.ExecuteNonQuery(
                    CommandType.Text,
                    "UPDATE KPRGUG SET KHXSIT = 'V' WHERE KHXKHWID = :rgid",
                    parameters);
            }

            return null;
        }

        public static string ValidateSingleRisque(RegularisationContext context) {
            if (!context.ComputeDone) {
                if (!context.ValidationDone) {
                    return "Le calcul n'a pas encore été effectué. Impossible de valider la regularisation de ce risque";
                }
            }
            else {
                var parameters = new EacParameter[]
                {
                    new EacParameter("rgid", DbType.Int64) { Value = context.RgId },
                    new EacParameter("rsqid", DbType.Int64) { Value = context.RsqId }
                };

                DbBase.Settings.ExecuteNonQuery(
                    CommandType.Text,
                    "UPDATE KPRGUR SET KILSIT = 'V' WHERE KILKHWID = :rgid AND KILRSQ = :rsqid",
                    parameters);

                DbBase.Settings.ExecuteNonQuery(
                    CommandType.Text,
                    "UPDATE KPRGUG SET KHXSIT = 'V' WHERE KHXKHWID = :rgid AND KHXRSQ = :rsqid",
                    parameters);
            }

            return null;
        }

        public static void ValidateRisqueForGarantie(int rsqNum, long regulGarId) {
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                @"UPDATE KPRGUR SET KILSIT = 'V' WHERE KILKHWID = ( SELECT KHXKHWID FROM KPRGUG WHERE KHXID = :reguleGarId AND KHXSIT = 'V' FETCH FIRST 1 ROWS ONLY ) AND KILRSQ = :rsq",
                new EacParameter("reguleGarId", DbType.Int32) { Value = Convert.ToDecimal(regulGarId) },
                new EacParameter("rsq", DbType.Int32) { Value = rsqNum });
        }

        public static bool IsFirstAccess(string codeContrat) {
            var sql = $"SELECT KHXSIT REGULSTATE FROM KPRGUG WHERE KHXIPB = :codeContrat AND KHXGARAN = '{AlbOpConstants.RCFrance}'";
            var param = new List<EacParameter> {
                new EacParameter("codeContrat", DbType.AnsiStringFixedLength) { Value = codeContrat.PadLeft(9, ' ')}
            };

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).First();
            if (result.RegulState == "N") {
                return true;
            }
            return false;
        }

        #region Programmes AS/400
        /// <summary>
        /// Retourne les montants de HT et Taxe (Régul)
        /// </summary>
        public static (string error, double amount, double tax) ComputeGarantieFigures(RegularisationContext context, GarantieValuesDto values) {
            var returnParam = new EacParameter("P0RET", DbType.AnsiStringFixedLength) { Value = string.Empty };
            var amountParam = new EacParameter("P0MHT", DbType.Decimal) { Value = 0M };
            var taxParam = new EacParameter("P0MTX", DbType.Decimal) { Value = 0M };

            DbBase.Settings.ExecuteNonQuery(
                CommandType.StoredProcedure,
                "*PGM/KA039CL",
                returnParam,
                new EacParameter("P0TYP", context.IdContrat.Type),
                new EacParameter("P0IPB", context.IdContrat.CodeOffre.PadLeft(9, ' ')),
                new EacParameter("P0ALX", DbType.Int16) { Value = (short)context.IdContrat.Version },
                new EacParameter("P0AVN", DbType.Int16) { Value = (context.ModeleAvtRegul != null) ? (short)context.ModeleAvtRegul.NumAvt : short.Parse(context.KeyValues[3]) },
                new EacParameter("P0RSQ", DbType.Int32) { Value = (int)context.RsqId },
                new EacParameter("P0ASS", DbType.Decimal) { Value = (decimal)values.Assiette },
                new EacParameter("P0VAL", DbType.Decimal) { Value = (decimal)values.TauxMontant },
                new EacParameter("P0UNI", values.Unite.Code == "%0" ? "§" : values.Unite.Code),
                new EacParameter("P0TAX", values.CodeTaxes.Code),
                amountParam,
                taxParam);

            if (!String.IsNullOrWhiteSpace(returnParam.Value as string)) {
                return ("Problème calcul montantHt et taxe, voir maintenance.", 0D, 0D);
            }
            else {
                return (null, Convert.ToDouble(amountParam.Value), Convert.ToDouble(taxParam.Value));
            }
        }

        public static bool IsReguleHisto(int reguleId) {
            var param = new List<EacParameter>
            {
                new EacParameter("reguleId", DbType.Int32) {Value = reguleId }
            };

            string sql = @"SELECT COUNT(*) NBLIGN FROM KPRGU
                                INNER JOIN YYYYPAR ON TCON = 'KHEOP' AND TFAM = 'RGMRG' AND TCOD = KHWMRG AND TPCN1 = 1
                            WHERE KHWID = :reguleId";

            return CommonRepository.ExistRowParam(sql, param);
        }
        #endregion

        #region Méthodes privées

        private static RegularisationBandeauContratDto GetBandeauDetailedContratInfo(string codeOffre, string version, string type) {
            var sql = @"SELECT 
YPOBASE.PBREF IDENTIFICATION,
CONCAT (CONCAT (TRIM(YPOBASE.PBCTA), ' - '),TRIM(Court.TNNOM)) COURTIER,
CONCAT (CONCAT (TRIM(YPOBASE.PBIAS), ' - '),TRIM(Assu.ANNOM)) ASSURE,
CONCAT (CONCAT (TRIM(YPOBASE.PBSOU), ' - '),TRIM(Sous.UTNOM)) SOUSCRIPTEUR,
REPLACE(CONCAT(CONCAT(CONCAT (CONCAT (YPOBASE.PBEFJ, '/'),YPOBASE.PBEFM), '/'), YPOBASE.PBEFA), '0/0/0', '') DEBUT_EFFET,
REPLACE(CONCAT(CONCAT(CONCAT (CONCAT (YPOBASE.PBFEJ, '/'),YPOBASE.PBFEM), '/'), YPOBASE.PBFEA), '0/0/0', '') FIN_EFFET,
CONCAT (CONCAT (TRIM(YPOBASE.PBPER), ' - '),TRIM(Per.TPLIB)) PERIODICITE,
REPLACE(CONCAT(CONCAT(CONCAT (CONCAT (Ech.JDPEJ, '/'),Ech.JDPEM), '/'), Ech.JDPEA), '0/0/0', '') ECHEANCE,
CASE TRIM(YPOBASE.PBNPL) WHEN '' THEN 
	TRIM(Nat.TPLIB) 
	ELSE CONCAT (CONCAT (TRIM(YPOBASE.PBNPL), ' - '),TRIM(Nat.TPLIB)) 
END NATURE,

Obs.KAJOBSV OBSERVATION,
CONCAT (CONCAT (TRIM(YPOBASE.PBGES), ' - '),TRIM(Gest.UTNOM)) GESTIONNAIRE,
CONCAT (CONCAT(IFNULL(NULLIF(YPOBASE.PBMER, ''), 'S'), ' - '), TRIM(IFNULL(Lib.TPLIL, ''))) TYPE_CONTRAT

FROM YPOBASE
INNER JOIN YPRTENT Ech ON Ech.JDIPB = YPOBASE.PBIPB AND Ech.JDALX = YPOBASE.PBALX
LEFT JOIN YUTILIS Gest ON Gest.UTIUT = YPOBASE.PBGES
LEFT JOIN YCOURTN Court ON Court.TNICT = YPOBASE.PBCTA AND Court.TNXN5 = 0 AND Court.TNTNM = 'A'
LEFT JOIN YUTILIS  Sous ON Sous.UTIUT = YPOBASE.PBSOU
LEFT JOIN YASSNOM Assu ON Assu.ANIAS = YPOBASE.PBIAS
LEFT JOIN YYYYPAR Lib ON Lib.TCON = 'KHEOP' AND Lib.TFAM = 'TYPOC' AND Lib.TCOD = IFNULL(NULLIF(YPOBASE.PBMER, ''), 'S')
LEFT JOIN YYYYPAR Per ON Per.TCON = 'PRODU' AND Per.TFAM = 'PBPER' AND Per.TCOD = YPOBASE.PBPER
LEFT JOIN YYYYPAR Nat ON Nat.TCON = 'PRODU' AND Nat.TFAM = 'PBNPL' AND Nat.TCOD = YPOBASE.PBNPL
LEFT JOIN YYYYPAR Ret ON Ret.TCON = 'PRODU' AND Ret.TFAM = 'PBTAC' AND Ret.TCOD = 
(
    SELECT CASE WHEN pb.PBETA='V' THEN pb.PBTAC
	ELSE hb.PBTAC END PBTAC FROM YPOBASE pb INNER JOIN YHPBASE hb ON hb.PBIPB = pb.PBIPB 
    WHERE pb.PBIPB = :codeOffre ORDER BY hb.PBAVN DESC FETCH FIRST 1 ROWS ONLY
)
LEFT JOIN KPENT EntObs ON  EntObs.KAAIPB = YPOBASE.PBIPB AND EntObs.KAAALX = YPOBASE.PBALX AND EntObs.KAATYP = YPOBASE.PBTYP
LEFT JOIN KPOBSV Obs ON Obs.KAJCHR = EntObs.KAAOBSV AND KAJTYPOBS = 'GENERALE'
LEFT JOIN YHPBASE hist ON hist.PBIPB = YPOBASE.PBIPB
WHERE YPOBASE.PBIPB = :codeOffre AND YPOBASE.PBALX = :version AND YPOBASE.PBTYP = :type
ORDER BY hist.PBAVN DESC";

            return RetrieveBandeauContratInfo(codeOffre, version, type, sql);
        }

        private static RegularisationBandeauContratDto GetBandeauLightContratInfo(string codeOffre, string version, string type) {
            const string SqlQuery = @"
SELECT PBREF IDENTIFICATION,
REPLACE(CONCAT(CONCAT(CONCAT (CONCAT (PBEFJ, '/'),PBEFM), '/'), PBEFA), '0/0/0', '') DEBUT_EFFET,
REPLACE(CONCAT(CONCAT(CONCAT (CONCAT (PBFEJ, '/'),PBFEM), '/'), PBFEA), '0/0/0', '') FIN_EFFET
FROM YPOBASE
WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

            return RetrieveBandeauContratInfo(codeOffre, version, type, SqlQuery);
        }

        private static RegularisationBandeauContratDto RetrieveBandeauContratInfo(string codeOffre, string version, string type, string sql) {
            List<EacParameter> param = new List<EacParameter>();

            param.Add(new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') });
            param.Add(new EacParameter("version", DbType.Int32) { Value = Convert.ToInt32(version) });
            param.Add(new EacParameter("type", DbType.AnsiStringFixedLength) { Value = type });


            var contratInfo = DbBase.Settings.ExecuteList<RegularisationBandeauContratDto>(CommandType.Text, sql, param).FirstOrDefault();

            //Requête pour le retour pièce ( table d'historique ou non )
            var paramRetourPiece = new List<EacParameter>()
            {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },
                new EacParameter("version", DbType.AnsiStringFixedLength) { Value = version },
                new EacParameter("type", DbType.AnsiStringFixedLength) { Value = type }
            };

            var sqlHistExist = @"SELECT COUNT(*) INT32RETURNCOL, ( SELECT PBETA FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type) STRRETURNCOL
                                         FROM YHPBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

            //Vérification d'une présence en histo
            var result2 = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlHistExist, paramRetourPiece).First();

            if (result2.StrReturnCol != "V" && result2.Int32ReturnCol > 0) {

                // SELECT HISTO PBTAC
                paramRetourPiece = new List<EacParameter>()
                {
                    new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },
                    new EacParameter("version", DbType.AnsiStringFixedLength) { Value = version },
                    new EacParameter("type", DbType.AnsiStringFixedLength) { Value = type }
                };

                sqlHistExist = @"SELECT PBTAC STRRETURNCOL FROM YHPBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

                result2 = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlHistExist, paramRetourPiece).First();


                paramRetourPiece = new List<EacParameter>()
                {
                     new EacParameter("pbtac", DbType.AnsiStringFixedLength) { Value = result2.StrReturnCol },
                     new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },

                };

                sqlHistExist = @"
SELECT YHPBASE.PBTAC CONCAT ' - ' CONCAT ret.TPLIB STRRETURNCOL FROM YHPBASE 
LEFT JOIN YYYYPAR Ret ON Ret.TCON = 'PRODU' AND Ret.TFAM = 'PBTAC' AND Ret.TCOD = :pbtac
WHERE YHPBASE.PBIPB = :codeOffre ORDER BY YHPBASE.PBAVN DESC FETCH FIRST 1 ROWS ONLY";

                //Sélection du pbtac présent en historique sur la dernière entrée + libellé associé.
                result2 = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlHistExist, paramRetourPiece).First();
            }
            else {
                // SELECT COURANT PBTAC
                paramRetourPiece = new List<EacParameter>()
                {
                    new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },
                    new EacParameter("version", DbType.AnsiStringFixedLength) { Value = version },
                    new EacParameter("type", DbType.AnsiStringFixedLength) { Value = type }
                };

                sqlHistExist = @"SELECT PBTAC STRRETURNCOL FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

                result2 = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlHistExist, paramRetourPiece).First();


                paramRetourPiece = new List<EacParameter>()
                {
                     new EacParameter("pbtac", DbType.AnsiStringFixedLength) { Value = result2.StrReturnCol },
                     new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') },
                };

                sqlHistExist = @"
SELECT TRIM(YPOBASE.PBTAC) CONCAT ' - ' CONCAT TRIM(ret.TPLIB) STRRETURNCOL FROM YPOBASE
LEFT JOIN YYYYPAR Ret ON Ret.TCON = 'PRODU' AND Ret.TFAM = 'PBTAC' AND Ret.TCOD = :pbtac
WHERE YPOBASE.PBIPB = :codeOffre";

                // Sélection du pbtac courant + libellé associé.
                result2 = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlHistExist, paramRetourPiece).First();
            }

            contratInfo.RetourPiece = result2.StrReturnCol;
            contratInfo.BasicInfo = GetContratInfo(codeOffre, version, type);

            return contratInfo;
        }

        private static DtoCommon GetInfoCatNat(string codeContrat, string version, string codeAvn, string dernierAvn) {
            EacParameter[] paramAvn = new EacParameter[2];
            paramAvn[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            paramAvn[0].Value = codeContrat.PadLeft(9, ' ');
            paramAvn[1] = new EacParameter("version", DbType.Int32);
            paramAvn[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            string sqlAvn = @"SELECT PBAVN INT32RETURNCOL FROM YPOBASE WHERE PBIPB = :codeContrat AND PBALX = :version";
            var resAvn = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlAvn, paramAvn);
            var numAvn = "0";
            if (resAvn != null && resAvn.Any()) {
                numAvn = resAvn.FirstOrDefault().Int32ReturnCol.ToString();
            }

            var modeSelect = ModeConsultation.Standard;
            var param = new List<EacParameter>() {
                    new EacParameter("codeContrat", DbType.AnsiStringFixedLength) { Value = codeContrat.PadLeft(9, ' ') },
                    new EacParameter("version", DbType.Int32) { Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0 }
                };

            if (numAvn != dernierAvn && !string.IsNullOrEmpty(dernierAvn)) {
                modeSelect = ModeConsultation.Historique;

                param.Add(new EacParameter("codeAvn", DbType.Int32) { Value = Convert.ToInt32(dernierAvn) });
            }

            string sql = string.Format(@"SELECT JDXCM DECRETURNCOL, JDCNC DECRETURNCOL2 FROM {0} WHERE JDIPB = :codeContrat AND JDALX = :version",
                            CommonRepository.GetPrefixeHisto(modeSelect, "YPRTENT"));
            if (modeSelect == ModeConsultation.Historique) {
                sql += @" AND JDAVN = :codeAvn AND JDHIN = 1";
            }

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any()) {
                return result.FirstOrDefault();
            }
            return new DtoCommon();
        }

        private static List<ParametreDto> GetListCourtierRegule(string codeContrat, string version) {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;

            string sql = @"
SELECT W2ICT ID, TNNOM DESCRIPTIF
FROM YDA300PF
INNER JOIN YCOURTN ON W2ICT = TNICT AND TNINL = 0 AND TNTNM = 'A' and TNXN5 = 0
WHERE W2IPB = :codeContrat AND W2ALX = :version";

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);
        }

        private static RegularisationInfoDto GetReguleInfo(string reguleId) {
            RegularisationInfoDto model = new RegularisationInfoDto();

            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("reguleId", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0;

            const string SqlQuery = @"
SELECT KHWEXE EXERCICE, KHWDEBP DATEDEB, KHWFINP DATEFIN, 
KHWICT CODECOURTIER, IFNULL(COURTIER.TNNOM,'') NOMCOURTIER, 
KHWICC CODECOURTIERCOM, IFNULL(COURTIERCOM.TNNOM,'') NOMCOURTIERCOM,  
KHWXCM TAUXCOMHCN, KHWCNC TAUXCOMCN, KHWMTF MOTIFAVT,
KHWENC CODEENC, TPLIL LIBENC, KHWAVN NUMAVT
FROM KPRGU 
LEFT JOIN YCOURTN COURTIER ON COURTIER.TNICT = KHWICT AND COURTIER.TNXN5 = 0 AND COURTIER.TNTNM = 'A'
LEFT JOIN YCOURTN COURTIERCOM ON COURTIERCOM.TNICT = KHWICC AND COURTIERCOM.TNXN5 = 0 AND COURTIERCOM.TNTNM = 'A'
LEFT JOIN YYYYPAR ON TCON = 'PRODU' AND TFAM = 'FBENC' AND TCOD = KHWENC
WHERE KHWID = :reguleId";
            var result = DbBase.Settings.ExecuteList<RegularisationInfoDto>(CommandType.Text, SqlQuery, param);
            if (result != null && result.Any()) {
                model = result.FirstOrDefault();
            }

            return model;
        }
        #endregion
    }
}
