using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Engagement;
using OP.WSAS400.DTO.SMP;
using OP.WSAS400.DTO.Traite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using static DataAccess.Helpers.OutilsHelper;

namespace OP.DataAccess
{
    public partial class EngagementRepository
    {

        #region Engagement

        #region Méthodes Publiques

        public static EngagementDto InitEngagement(string codeOffre, string version, string type, string codeAvn, string codePeriode, ModeConsultation modeNavig, bool enregistrementEncoursOnly)
        {
            EngagementDto toReturn = new EngagementDto();
            var param = new List<DbParameter>() {
                new EacParameter("codeOffre", codeOffre.ToIPB()),
                new EacParameter("version", DbType.Int32) { Value = Convert.ToInt32(version) },
                new EacParameter("type", type)
            };
            if (modeNavig == ModeConsultation.Historique) {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }
            if (!string.IsNullOrEmpty(codePeriode)) {
                param.Add(new EacParameter("per", codePeriode));
            }

            bool isAffNv = type == AlbConstantesMetiers.TYPE_CONTRAT && (!int.TryParse(codeAvn, out int i) || i < 1);
            string typ = modeNavig == ModeConsultation.Standard ? "PBTYP" : $"'{AlbConstantesMetiers.TypeHisto}'";
            string sql = $@"
SELECT CASE WHEN H.KDOID IS NULL THEN ENT.KAALCIVALO ELSE H2.KAALCIVALO END LCIVALEUR, 
    CASE WHEN H.KDOID IS NULL THEN ENT.KAALCIUNIT ELSE H2.KAALCIUNIT END LCIUNITE, 
    CASE WHEN H.KDOID IS NULL THEN ENT.KAALCIBASE ELSE H2.KAALCIBASE END LCIBASE, 
    CASE WHEN H.KDOID IS NULL THEN ENT.KAALCIINA ELSE H2.KAALCIINA END LCIINDEXEE, 
    PBNPL NATURECONTRAT, NATURE.TPLIB NATURECONTRATLIB, PBAPP PARTALBINGIA, PBPCV COUVERTURE, TRIM(IFNULL(KDPFAM, '')) FAMTRAITE, 
    TRIM(IFNULL(GARAN.TPLIB, '')) LIBTRAITE, IFNULL(NULLIF(KDPSMP, 0), IFNULL(KDPENG, 0)) ENGAGEMENTTOTAL, IFNULL(NULLIF(KDPSMA, 0), IFNULL(KDPENA, 0)) ENGAGMENTALBINGIA, 
    IFNULL(COTIST.KDMCNAKBS, 0) CATNATTOTAL, IFNULL(COTISA.KDMCNAKBS, 0) CATNATALBINGIA,IFNULL(KAJOBSV,'') COMMENTFORCE, 
    ENT.KAAKDIID LIENCOMPLEXE , KDILCE CODECOMPLEXE , KDIDESC LIBCOMPLEXE, 
    CASE IFNULL( KDRSMF , 0 ) WHEN 0 THEN IFNULL( KDRSMP , 0 ) ELSE IFNULL( KDRSMF , 0 ) END SMPTOTAL , 
    {(isAffNv ? "PBEFA * 10000 + PBEFM * 100 + PBEFJ DATEDEB" : "ENG.KDODATD DATEDEB")}, 
    {(isAffNv ? "PBFEA * 10000 + PBFEM * 100 + PBFEJ DATEFIN" : "ENG.KDODATF DATEFIN")} 
FROM {CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE")} 
    LEFT JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPENT")} ENT ON ENT.KAAIPB = PBIPB 
    AND ENT.KAAALX = PBALX AND ENT.KAATYP = {typ} {(modeNavig == ModeConsultation.Historique ? " AND ENT.KAAAVN = PBAVN" : string.Empty)} 
    LEFT JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPENG")} ENG ON ENG.KDOIPB = PBIPB AND ENG.KDOALX = PBALX 
    AND ENG.KDOTYP = {typ} {(modeNavig == ModeConsultation.Historique ? " AND ENG.KDOAVN = PBAVN" : string.Empty)} 
    LEFT JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPENGRSQ")} ON KDRIPB = PBIPB AND KDRALX = PBALX 
    AND ENG.KDOID = KDRKDOID
    LEFT JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPENGFAM")} ON ENG.KDOID = KDPKDOID AND KDPFAM = KDRFAM 
    {CommonRepository.BuildJoinYYYYPAR("LEFT", "REASS", "GARAN", otherCriteria: " AND GARAN.TCOD = KDPFAM", alias: "GARAN")} 
    LEFT JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPCOTIS")} COTIST ON COTIST.KDMIPB = PBIPB AND COTIST.KDMALX = PBALX 
    AND COTIST.KDMTYP = {typ} AND COTIST.KDMTAP = 'T' {(modeNavig == ModeConsultation.Historique ? " AND COTIST.KDMAVN = PBAVN" : string.Empty)} 
    LEFT JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPCOTIS")} COTISA ON COTISA.KDMIPB = PBIPB AND COTISA.KDMALX = PBALX 
    AND COTISA.KDMTYP = {typ} AND COTISA.KDMTAP = 'A' {(modeNavig == ModeConsultation.Historique ? " AND COTISA.KDMAVN = PBAVN" : string.Empty)} 
    LEFT JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPOBSV")} ON KAJCHR = ENG.KDOOBSV 
    LEFT JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPEXPLCI")} ON KDIID = ENT.KAAKDIID 
    {CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBNPL", otherCriteria: " AND NATURE.TCOD = PBNPL", alias: "NATURE")}
    LEFT JOIN HPENG H ON ENG.KDOID=H.KDOID AND H.KDOAVN = {(!codeAvn.IsEmptyOrNull() ? 0 : Convert.ToInt32(codeAvn) - 1)}
    LEFT JOIN HPENT h2 ON (H.KDOIPB, H.KDOALX, H.KDOTYP, H.KDOAVN) = (h2.KAAIPB, h2.KAAALX, h2.KAATYP, h2.KAAAVN)
WHERE PBIPB = :codeOffre AND PBALX = :version AND {typ} = :type 
{(modeNavig == ModeConsultation.Historique ? " AND PBAVN = :avn" : string.Empty)} 
{(!string.IsNullOrEmpty(codePeriode) ? "AND ENG.KDOID = :per" : string.Empty)} ";

            var result = DbBase.Settings.ExecuteList<EngagementPlatDto>(CommandType.Text, sql, param);
            if (result?.Any() ?? false) {
                var lstEngTraite = result.GroupBy(x => new { x.FamilleTraite, x.LibTraite }).ToList().Select(g => new EngagementTraiteDto {
                    FamilleTraite = g.Key.FamilleTraite,
                    NomTraite = g.Key.LibTraite,
                    EngagementTotal = g.First().EngagementTotal.ToString(),
                    EngagementAlbingia = g.First().EngagmentAlbingia.ToString(),
                    SMPTotal = g.Sum(e => e.SMPTotal).ToString(),
                    SMPAlbingia = Math.Round((g.Sum(e => e.SMPTotal) * g.First().PartAlbingia) / 100F, 0, MidpointRounding.AwayFromZero).ToString()
                }).ToList();

                toReturn.LCIValeur = result[0].LciValeur.ToString();
                toReturn.LCIUnite = result[0].LciUnite;
                toReturn.LCIType = result[0].LciBase;
                toReturn.LCIIndexee = result[0].LciIndexee == "O";
                toReturn.Nature = $"{result[0].NatureContrat} - {result[0].NatureContratLib}";
                toReturn.PartAlb = result[0].PartAlbingia.ToString();
                toReturn.Couverture = result[0].Couverture.ToString();
                toReturn.Traites = lstEngTraite;
                toReturn.BaseAlb = result[0].CATNATAlbingia.ToString();
                toReturn.BaseTotale = result[0].CATNATTotal.ToString();
                toReturn.MontantForce = result[0].MontantForce == "O";
                toReturn.CommentForce = result[0].CommentForce;
                toReturn.LienComplexeLCI = result[0].LienComplexeLCI.ToString();
                toReturn.LibComplexeLCI = result[0].LibComplexeLCI;
                toReturn.CodeComplexeLCI = result[0].CodeComplexeLCI;
                toReturn.DateDeb = AlbConvert.ConvertIntToDate(Convert.ToInt32(result[0].DateDeb));
                toReturn.DateFin = AlbConvert.ConvertIntToDate(Convert.ToInt32(result[0].DateFin));
            }

            return toReturn;
        }

        public static void EngagementUpdate(string codeOffre, string version, string type, EngagementDto engagement, string codePeriode)
        {
            DbParameter[] param = new DbParameter[12];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_LCIVALEUR", 0);
            param[3].Value = Convert.ToDecimal(string.IsNullOrEmpty(engagement.LCIValeur) ? "0" : engagement.LCIValeur);
            param[4] = new EacParameter("P_LCIUNITE", engagement.LCIUnite);
            param[5] = new EacParameter("P_LCITYPE", engagement.LCIType);
            param[6] = new EacParameter("P_LCIINDEXEE", engagement.LCIIndexee ? "O" : "N");
            param[7] = new EacParameter("P_PARTALB", 0);
            param[7].Value = Convert.ToDecimal(engagement.PartAlb);
            param[8] = new EacParameter("P_COMMENTFORCE", engagement.CommentForce);
            param[9] = new EacParameter("P_MODECOMMENTONLY", "N");
            param[10] = new EacParameter("P_LienCpxLCIGenerale", 0);
            param[10].Value = Convert.ToInt64(string.IsNullOrEmpty(engagement.LienComplexeLCI) ? "0" : engagement.LienComplexeLCI);
            param[11] = new EacParameter("P_CODEPERIODE", 0);
            param[11].Value = !string.IsNullOrEmpty(codePeriode) ? Int32.Parse(codePeriode) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDENG", param);


        }

        public static List<EngagementPeriodeDto> GetEngagementPeriodes(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            //CASE WHEN (SELECT COUNT(*) FROM YPORHMD WHERE HDSMP = KDOID) > 0 THEN 'U' ELSE 'N' END MODEUTILISE,
            codeOffre = codeOffre.ToIPB();
            bool modeHisto = modeNavig == ModeConsultation.Historique && !string.IsNullOrEmpty(codeAvn);
            string sql = $@"SELECT K.KDOID CODE,
                                   K.KDODATD DATEDEBUT, 
                                   K.KDODATF DATEFIN,
                                   K.KDOAPP PART,
                                   CASE
	                                   WHEN H.KDOID IS NULL THEN 
		                                   CASE 
			                                   WHEN K2.KAALCIVALA <> 0 THEN K2.KAALCIVALA
			                                   ELSE K.KDOENG
		                                   END
	                                   ELSE 
		                                   CASE 
			                                   WHEN H2.KAALCIVALA <> 0 THEN H2.KAALCIVALA
			                                   ELSE H.KDOENG
		                                   END
                                   END ENGAGTOTAL,
                                   CASE
	                                   WHEN H.KDOID IS NULL THEN 
		                                   CASE 
			                                   WHEN K2.KAALCIVALA <> 0 THEN K2.KAALCIVALA * K.KDOAPP / 100
			                                   ELSE K.KDOENA
		                                   END
	                                   ELSE 
		                                   CASE 
			                                   WHEN H2.KAALCIVALA <> 0 THEN H2.KAALCIVALA * H.KDOAPP / 100
			                                   ELSE H.KDOENA
		                                   END
                                   END ENGAGALBINGIA,
                                   CASE WHEN K.KDOACT = 'O' THEN 'A' ELSE 'I' END MODEACTIF , 
                                   CASE WHEN (SELECT COUNT(*) FROM YPORHMD WHERE HDSMP <> 0 AND K.KDOENGID <> 0 AND (HDSMP  =  K.KDOENGID/100000) and HDSMO = MOD(K.KDOENGID,100000)) > 0 THEN 'U' ELSE 'N' END MODEUTILISE ,
                                   CASE WHEN H.KDOID IS NULL  THEN  'N' ELSE 'O' END INHPENG
                            FROM {CommonRepository.GetPrefixeHisto(modeNavig, "KPENG")} K
                                   INNER JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPENT")} k2 ON (K.KDOIPB, K.KDOALX, K.KDOTYP) = (k2.KAAIPB, k2.KAAALX, k2.KAATYP)
                                   LEFT JOIN HPENG H ON K.KDOID=H.KDOID AND H.KDOAVN = {(!codeAvn.IsEmptyOrNull()?0:Convert.ToInt32(codeAvn) -1)}
                                   LEFT JOIN HPENT h2 ON (H.KDOIPB, H.KDOALX, H.KDOTYP, H.KDOAVN) = (h2.KAAIPB, h2.KAAALX, h2.KAATYP, h2.KAAAVN)
                            WHERE 
                                 K.KDOTYP = :type 
                             AND K.KDOIPB = :codeOffre 
                             AND K.KDOALX = :version";

            var paramValues = new List<object> { type, codeOffre, version };
            if (modeHisto)
            {
                sql += " AND K.KDOAVN = :codeAvn";
                paramValues.Add(Convert.ToInt32(codeAvn));
            }
            sql += " ORDER BY K.KDODATD";

            var param = MakeParams(sql, paramValues);
            return DbBase.Settings.ExecuteList<EngagementPeriodeDto>(CommandType.Text, sql, param);
        }
        public static List<ModeleDetailsConnexitesEngPeriodeDto> GetEngagementPeriodesDetails(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            bool modeHisto = modeNavig == ModeConsultation.Historique && !string.IsNullOrEmpty(codeAvn);
            //CASE WHEN (SELECT COUNT(*) FROM YPORHMD WHERE HDSMP = KDOID) > 0 THEN 'U' ELSE 'N' END MODEUTILISE,
            string sql = $@"SELECT
	                        KDOID CODE,
	                        CASE WHEN KDOACT = 'O' THEN 'A' ELSE 'I' END MODEACTIF,
                            CASE WHEN (SELECT COUNT(*) FROM YPORHMD WHERE HDSMP <> 0 AND KDOENGID <> 0 AND (HDSMP  =  KDOENGID/100000) and HDSMO = MOD(KDOENGID,100000)) > 0 THEN 'U' ELSE 'N' END MODEUTILISE,
	                        KDODATD DATEDEBUT,
	                        KDODATF DATEFIN,
	                        KDPFAM CODEENGAGEMENT,
                            KDPENA VALEURENGAGEMENT
                        FROM {CommonRepository.GetPrefixeHisto(modeNavig, "KPENG")} 
                            LEFT JOIN KPENGFAM ON KDPKDOID=KDOID
                        WHERE KDOTYP = :type
	                        AND KDOIPB = :codeOffre
	                        AND KDOALX = :version
                            { (modeHisto ? " AND KDOAVN = :codeAvn " : "") }
                        ORDER BY KDODATD";
            var paramValues = new List<object> { type, codeOffre.ToIPB(), version };
            if (modeHisto) {
                paramValues.Add(Convert.ToInt32(codeAvn));
            }
            var param = MakeParams(sql, paramValues);

            return DbBase.Settings.ExecuteList<ModeleDetailsConnexitesEngPeriodeDto>(CommandType.Text, sql, param);
        }

        public static List<ModeleDetailsConnexitesEngPeriodeDto> GetPeriodesConnexitesEngagement(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            bool modeHisto = modeNavig == ModeConsultation.Historique && !string.IsNullOrEmpty(codeAvn);
            //CASE WHEN (SELECT COUNT(*) FROM YPORHMD WHERE HDSMP = KDOID) > 0 THEN 'U' ELSE 'N' END MODEUTILISE,
            string sql = $@"SELECT
	                        KDOID CODE,
	                        CASE WHEN KDOACT = 'O' THEN 'A' ELSE 'I' END MODEACTIF,
                            CASE WHEN (SELECT COUNT(*) FROM YPORHMD WHERE HDSMP <> 0 AND KDOENGID <> 0 AND (HDSMP  =  KDOENGID/100000) and HDSMO = MOD(KDOENGID,100000)) > 0 THEN 'U' ELSE 'N' END MODEUTILISE,
	                        KDODATD DATEDEBUT,
	                        KDODATF DATEFIN,
	                            KDPFAM CODEENGAGEMENT,
                            KDPENA VALEURENGAGEMENT
                        FROM YPOCONX
		                        INNER JOIN KPENGCNX ON PJIDE = KIEPJID
		                        INNER JOIN { CommonRepository.GetPrefixeHisto(modeNavig, "KPENG") }  ON KIEKDOID = KDOID
                                LEFT JOIN KPENGFAM ON KDPKDOID = KDOID
                        WHERE PJTYP = :type
	                            AND PJIPB = :codeOffre
	                            AND PJALX = :version
                                { (modeHisto ? "AND KDOAVN = :codeAvn" : "") }
                        ORDER BY KDODATD";
            var paramValues = new List<object> { type, codeOffre.ToIPB(), version };
            if (modeHisto) {
                paramValues.Add(Convert.ToInt32(codeAvn));
            }
            var param = MakeParams(sql, paramValues);


            var result = DbBase.Settings.ExecuteList<ModeleDetailsConnexitesEngPeriodeDto>(CommandType.Text, sql, param) ?? new List<ModeleDetailsConnexitesEngPeriodeDto>();
            result.ForEach(i =>
                               {
                                   i.DateDebut = AlbConvert.ConvertIntToDate(i.DateDebutInt);
                                   i.DateFin = AlbConvert.ConvertIntToDate(i.DateFinInt);

                               });

            return result;
        }

        public static string SaveEngagementPeriode(string codeOffre, string version, string type, string codeAvn, string user, EngagementPeriodeDto engagementPeriode, string typeOperation, ModeConsultation modeNavig)
        {
            string codeEngOut = string.Empty;

            DbParameter[] param = new DbParameter[12];
            param[0] = new EacParameter("P_TYPE_OFFRE", type);
            param[1] = new EacParameter("P_ID_OFFRE", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P_VERSION_OFFRE", version);
            param[3] = new EacParameter("P_CODE_ENG", 0);
            param[3].Value = engagementPeriode.Code;
            param[4] = new EacParameter("P_DATE_DEBUT", engagementPeriode.DateDebut);
            param[5] = new EacParameter("P_DATE_FIN", engagementPeriode.DateFin);
            param[6] = new EacParameter("P_STATUT_ACTIF", engagementPeriode.Actif == "A" ? "O" : "N");
            param[7] = new EacParameter("P_USER", user);
            param[8] = new EacParameter("P_DATE_OPE", 0);
            param[8].Value = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            param[9] = new EacParameter("P_TYPE_OPERATION", typeOperation);
            param[10] = new EacParameter("P_MODEAVN", !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? "O" : "N");
            param[11] = new EacParameter("P_CODE_OUT", DbType.Int32);
            param[11].Value = 0;
            param[11].Direction = ParameterDirection.InputOutput;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDENG", param);


            codeEngOut = param[11].Value.ToString();

            return codeEngOut;
        }

        public static string GetDateControle(string branche, string cible)
        {
            var sql = CommonRepository.BuildSelectYYYYPAR(branche, cible, string.Empty, "CHAR(INT(TPCN1)) STRRETURNCOL", "KHEOP", "ENGCT");
            return CommonRepository.GetStrValue(sql);
        }

        public static void SaveEngagementCommentaire(string codeOffre, string version, string type, string commentaire, string codePeriode)
        {
            DbParameter[] param = new DbParameter[12];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = int.Parse(version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_LCIVALEUR", 0);
            param[3].Value = 0;
            param[4] = new EacParameter("P_LCIUNITE", string.Empty);
            param[5] = new EacParameter("P_LCITYPE", string.Empty);
            param[6] = new EacParameter("P_LCIINDEXEE", string.Empty);
            param[7] = new EacParameter("P_PARTALB", 0);
            param[7].Value = 0;
            param[8] = new EacParameter("P_COMMENTFORCE", commentaire);
            param[9] = new EacParameter("P_MODECOMMENTONLY", "O");
            param[10] = new EacParameter("P_LienCpxLCIGenerale", 0);
            param[10].Value = 0;
            param[11] = new EacParameter("P_CODEPERIODE", 0);
            param[11].Value = !string.IsNullOrEmpty(codePeriode) ? Int32.Parse(codePeriode) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDENG", param);
        }

        public static DtoCommon VerifEngagementExist(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            DtoCommon toReturn = new DtoCommon();
            var param = new List<DbParameter> {
                new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' ')),
                new EacParameter("version", !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0),
                new EacParameter("type", type)
            };
            bool isModeHisto = modeNavig == ModeConsultation.Historique && !string.IsNullOrEmpty(codeAvn);
            if (isModeHisto) {
                param.Add(new EacParameter("avn", Convert.ToInt32(codeAvn)));
            }
            string sql = $@"SELECT KDOID INT64RETURNCOL
FROM {CommonRepository.GetPrefixeHisto(modeNavig, "KPENG")}
WHERE KDOIPB = :codeOffre
AND KDOALX = :version
AND KDOTYP = :type
AND KDOECO = 'O'
{(isModeHisto ? " AND KDOAVN = :avn" : "")}";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                toReturn.Int64ReturnCol = result.FirstOrDefault().Int64ReturnCol;
            }

            sql = $@"SELECT PBTTR STRRETURNCOL
FROM {CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE")}
WHERE PBIPB = :codeOffre
AND PBALX = :version
AND PBTYP = :type
{(isModeHisto ? " AND PBAVN = :avn" : "")}";
            result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                toReturn.StrReturnCol = result.FirstOrDefault().StrReturnCol;
            }

            return toReturn;
        }

        #endregion

        #region Méthodes Privées

        private static void UpdateKPENT(string codeOffre, string version, string type, EngagementDto engagement)
        {
            if (!string.IsNullOrEmpty(codeOffre) && !string.IsNullOrEmpty(version) && !string.IsNullOrEmpty(type) && engagement != null)
            {
                int iVersion = 0;
                if (int.TryParse(version, out iVersion))
                {
                    string sql = string.Format(@"UPDATE KPENT
                                                    SET KAALCIVALO = '{0}',
                                                    KAALCIVALA = '{0}',
                                                    KAALCIUNIT = '{1}',
                                                    KAALCIBASE = '{2}',
                                                    KAALCIINA = '{3}',
                                                    KAAKDIID= '{7}'
                                                WHERE
                                                    KAATYP = '{4}'
                                                    AND KAAIPB = '{5}'
                                                    AND KAAALX = {6}",
                                                !string.IsNullOrEmpty(engagement.LCIValeur) ? Convert.ToDecimal(engagement.LCIValeur) : 0,
                                                engagement.LCIUnite,
                                                engagement.LCIType,
                                                engagement.LCIIndexee ? "O" : "N",
                                                type,
                                                codeOffre.PadLeft(9, ' '),
                                                iVersion,
                                                !string.IsNullOrEmpty(engagement.LienComplexeLCI) ? engagement.LienComplexeLCI : "0");

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
        }
        #endregion

        #endregion

        #region Traite

        #region Méthodes Publiques

        public static TraiteDto InitTraite(string codeOffre, string version, string type, string codeAvn, string traite, ModeConsultation modeNavig, bool isReadonly, string user, string acteGestion, string codePeriode, string accessMode)
        {
            if (!isReadonly && modeNavig == ModeConsultation.Standard) {
                if (codeAvn == "0" && accessMode.IsEmptyOrNull()) {
                    CommonRepository.LoadAS400Engagement(codeOffre, version, type, modeNavig, codeAvn, user, acteGestion);
                }
                else {
                    CommonRepository.LoadAS400EngagementAvn(codeOffre, version, type, codePeriode, codeAvn, user, acteGestion);
                }
            }
            TraiteDto toReturn = null;
            var selection = new Dictionary<string, string> {
                ["CASE WHEN H.KDOID IS NULL THEN ENT.KAALCIVALO ELSE H2.KAALCIVALO END"] = "LCIVALEUR",
                ["CASE WHEN H.KDOID IS NULL THEN ENT.KAALCIUNIT ELSE H2.KAALCIUNIT END"] = "LCIUNITE",
                ["CASE WHEN H.KDOID IS NULL THEN ENT.KAALCIBASE ELSE H2.KAALCIBASE END"] = "LCITYPE",
                ["CASE WHEN H.KDOID IS NULL THEN ENT.KAALCIINA ELSE H2.KAALCIINA END"] = "LCIINDEXEE",
                ["TPLIB"] = "NOMTRAITE",
                ["ENG.KDODATD"] = "EFFETDEB",
                ["ENG.KDODATF"] = "EFFETFIN",
                ["IFNULL(NULLIF(KDPSMP, 0), IFNULL(KDPENG, 0))"] = "ENGAGEMENTTOTAL",
                ["PBAPP"] = "PARTALB",
                ["IFNULL(NULLIF(KDPSMA, 0), KDPENA)"] = "ENGAGEMENTALB",
                ["KDPFAM"] = "FAMREASSU",
                ["IFNULL( KAJOBSV , '' )"] = "COMMENTFORCE",
                ["ENT.KAAKDIID"] = "LIENCOMPLEXE",
                ["IFNULL( KDILCE , '' )"] = "CODECOMPLEXE",
                ["IFNULL( KDIDESC , '' )"] = "LIBCOMPLEXE"
            };
            string mainTable = CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE");
            var joins = new Dictionary<string, string[]> {
                [CommonRepository.GetPrefixeHisto(modeNavig, "KPENT ENT")] = new[] {
                    "( PBIPB , PBALX , PBTYP ) = ( ENT.KAAIPB , ENT.KAAALX , ENT.KAATYP )",
                    modeNavig == ModeConsultation.Historique ? "PBAVN = ENT.KAAAVN" : string.Empty
                }
            };
            var leftJoins = new Dictionary<string, string[]> {
                [CommonRepository.GetPrefixeHisto(modeNavig, "KPENG ENG")] = new[] {
                    "( PBIPB , PBALX , PBTYP ) = ( ENG.KDOIPB , ENG.KDOALX , ENG.KDOTYP )",
                    modeNavig == ModeConsultation.Historique ? "PBAVN = ENG.KDOAVN" : string.Empty
                },
                [CommonRepository.GetPrefixeHisto(modeNavig, "KPENGFAM")] = new[] { "ENG.KDOID = KDPKDOID" },
                [CommonRepository.GetPrefixeHisto(modeNavig, "KPOBSV")] = new[] { "KAJCHR = ENG.KDOOBSV" },
                [CommonRepository.GetPrefixeHisto(modeNavig, "KPEXPLCI")] = new[] { "KDIID = ENT.KAAKDIID" },
                ["HPENG H"] = new [] { $"ENG.KDOID=H.KDOID AND H.KDOAVN = {(!codeAvn.IsEmptyOrNull() ? 0 : Convert.ToInt32(codeAvn) - 1)}" },
                ["HPENT H2"] = new [] { "(H.KDOIPB, H.KDOALX, H.KDOTYP, H.KDOAVN) = (H2.KAAIPB, H2.KAAALX, H2.KAATYP, H2.KAAAVN)" }
            };
            var wheres = new[] {
                "( PBIPB , PBALX , PBTYP , KDPFAM ) = ( :ipb , :alx  , :typ , :fam )",
                modeNavig == ModeConsultation.Historique ? "PBAVN = :avn" : string.Empty,
                codePeriode.IsEmptyOrNull() ? string.Empty : "ENG.KDOID = :prd"
            };
            string sql = $@"SELECT {string.Join(", ", selection.Select(x => $"{x.Key} {x.Value}"))} 
FROM {mainTable} 
INNER JOIN {string.Join(" INNER JOIN ", joins.Select(x => $"{x.Key} ON {string.Join(" AND ", x.Value.Where(k => k.ContainsChars()))}"))} 
LEFT JOIN {string.Join(" LEFT JOIN ", leftJoins.Select(x => $"{x.Key} ON {string.Join(" AND ", x.Value.Where(k => k.ContainsChars()))}"))} 
{CommonRepository.BuildJoinYYYYPAR("LEFT", "REASS", "GARAN", otherCriteria: " AND TCOD = KDPFAM")} 
WHERE {string.Join(" AND ", wheres.Where(s => s.ContainsChars()))}";

            var dbParams = new List<EacParameter> {
                new EacParameter("ipb", codeOffre.ToIPB()),
                new EacParameter("alx", DbType.Int32) { Value = int.Parse(version) },
                new EacParameter("typ", type),
                new EacParameter("fam", traite.NullIfEmpty() ?? string.Empty)
            };
            if (modeNavig == ModeConsultation.Historique) {
                dbParams.Add(new EacParameter("avn", DbType.Int32) { Value = int.Parse(codeAvn) });
            }
            if (!string.IsNullOrEmpty(codePeriode)) {
                dbParams.Add(new EacParameter("prd", codePeriode));
            }

            var result = DbBase.Settings.ExecuteList<TraiteDto>(CommandType.Text, sql, dbParams.ToArray());
            if (result?.Any() ?? false) {
                toReturn = result.FirstOrDefault();

                // Initialisation des données complémentaires
                if (toReturn.SDebutEffet > 0)
                    toReturn.DDebutEffet = AlbConvert.ConvertIntToDate(toReturn.SDebutEffet);
                if (toReturn.SFinEffet > 0)
                    toReturn.DFinEffet = AlbConvert.ConvertIntToDate(toReturn.SFinEffet);
                if (!string.IsNullOrEmpty(toReturn.SLCIIndexee))
                    toReturn.LCIIndexee = toReturn.SLCIIndexee == "O";

                toReturn.TraiteInfoRsqVen = GetTraiteInfoRsqVol(codeOffre, version, type, codeAvn, toReturn.FamilleReassurance, codePeriode, toReturn.PartAlb, modeNavig);

            }
            if (toReturn == null)
            {
                toReturn = new TraiteDto {
                    TraiteInfoRsqVen = new TraiteInfoRsqVenDto {
                        TraiteRisques = new List<TraiteRisqueDto>(),
                        TraiteVentilations = new List<TraiteVentilationDto>()
                    }
                };
            }
            return toReturn;
        }

        public static void UpdateTraite(string codeOffre, string version, string type, TraiteDto traiteDto, string user, string codePeriode)
        {
            EngagementDto engagement = new EngagementDto {
                LCIValeur = traiteDto.LCIValeur.ToString(),
                LCIType = traiteDto.LCIType,
                LCIUnite = traiteDto.LCIUnite,
                LCIIndexee = traiteDto.LCIIndexee,
                LienComplexeLCI = traiteDto.LienComplexeLCI.ToString()
            };

            UpdateKPENT(codeOffre, version, type, engagement);

            foreach (var rsq in traiteDto.TraiteInfoRsqVen.TraiteRisques) {
                UpdateTraiteRisque(codeOffre, version, type, rsq, user, codePeriode);
            }

            foreach (var vent in traiteDto.TraiteInfoRsqVen.TraiteVentilations) {
                UpdateTraiteVentilationGenerale(codeOffre, version, type, vent, user, codePeriode);
            }
        }


        public static List <SMPTaiteDto> GetSmpT(int id)  
        {
            string sql = $@"SELECT KDRID,KDRSMP,KDRSMF FROM KPENGRSQ WHERE KDRKDQID ={id}";
            var toreturn = DbBase.Settings.ExecuteList<SMPTaiteDto>(CommandType.Text, sql);
            return toreturn;// DbBase.Settings.ExecuteList<SMPTaiteDto>(CommandType.Text, sql);
        }

        public static void SaveSmpT(int SmpCptF, int id)
        {
            
             string sql = $@"UPDATE KPENGRSQ SET KDRSMF= '{SmpCptF}', KDRENGF = {SmpCptF} WHERE KDRID = {id} ";
             DbBase.Settings.ExecuteList<SMPTaiteDto>(CommandType.Text, sql);
        
        }

        #endregion

        #region Méthodes Privées
        /// <summary>
        /// 
        /// </summary>
        /// <![CDATA[
        /// // exemple
        /// #region Données exemple
        /// if (result == null || !result.Any())
        ///     result = new List<TraiteRisquesVentilationsDto>();
        /// 
        /// TraiteRisquesVentilationsDto traite = new TraiteRisquesVentilationsDto
        /// {
        ///     CodeRisque = 1,
        ///     NomVentilation = "Ventilation2",
        ///     IdVentilation = 2,
        ///     SMP = 30000,
        ///     LCI = 0,
        ///     RisqueSel = "O",
        ///     EngagementForce = 45000,
        ///     EngagementVentilationCalcule = 45000,
        ///     EngagementVentilationForce = 0,
        ///     VentilationSel = "O"
        /// };
        /// result.Add(traite);
        /// 
        /// traite = new TraiteRisquesVentilationsDto
        /// {
        ///     CodeRisque = 1,
        ///     NomVentilation = "Ventilation3",
        ///     IdVentilation = 3,
        ///     SMP = 1000000,
        ///     LCI = 500000,
        ///     RisqueSel = "O",
        ///     EngagementForce = 10000,
        ///     EngagementVentilationCalcule = 1500000,
        ///     EngagementVentilationForce = 500000,
        ///     VentilationSel = "O"
        /// };
        /// result.Add(traite);
        /// traite = new TraiteRisquesVentilationsDto
        /// {
        ///     CodeRisque = 1,
        ///     NomVentilation = "Ventilation4",
        ///     IdVentilation = 3,
        ///     SMP = 1000000,
        ///     LCI = 500000,
        ///     RisqueSel = "O",
        ///     EngagementForce = 10000,
        ///     EngagementVentilationCalcule = 1500000,
        ///     EngagementVentilationForce = 500000,
        ///     VentilationSel = "O"
        /// };
        /// result.Add(traite);
        /// #endregion
        /// ]]>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <param name="famReassu"></param>
        /// <param name="codePeriode"></param>
        /// <param name="modeNavig"></param>
        /// <returns></returns>
        private static TraiteInfoRsqVenDto GetTraiteInfoRsqVol(string codeOffre, string version, string type, string codeAvn, string famReassu, string codePeriode, double partAlb, ModeConsultation modeNavig)
        {
            var selection = new Dictionary<string, string> {
                ["KDRRSQ"] = "RISQUE",
                ["KABDESC"] = "DESCRRSQ",
                ["KDRKDQID"] = "IDVENTILATION",
                ["KGALIBV"] = "NOMVENTILATION",
                ["KDRSMP"] = "SMP",
                ["KDRSMF"] = "SMPFORCE",
                ["KDRLCI"] = "LCI",
                ["KDRENGF"] = "ENGAGEMENTFORCE",
                ["KDRENGC"] = "ENGAGEMENT",
                ["KDRENGOK"] = "RSQSEL",
                ["KDQENGC"] = "ENGAGEMENTVENTCALCULE",
                ["KDQENGF"] = "ENGAGEMENTVENTFORCE",
                ["KDQENGOK"] = "VENTSEL"
            };
            string mainTable = CommonRepository.GetPrefixeHisto(modeNavig, "KPENG");
            var joins = new Dictionary<string, string[]> {
                [CommonRepository.GetPrefixeHisto(modeNavig, "KPENGRSQ")] = new[] {
                    "( KDOIPB , KDOALX , KDOTYP ) = ( :ipb , :alx , :typ )",
                    codePeriode.IsEmptyOrNull() ? string.Empty : "KDOID = :per",
                    "( KDRIPB , KDRALX , KDRTYP , KDRFAM ) = ( KDOIPB , KDOALX , KDOTYP , :fam )",
                    codePeriode.IsEmptyOrNull() ? string.Empty : "KDRKDOID = KDOID",
                    modeNavig == ModeConsultation.Historique ? "KDOAVN = KDRAVN" : string.Empty
                },
                [CommonRepository.GetPrefixeHisto(modeNavig, "KPENGVEN")] = new[] {
                    "( KDQIPB , KDQALX , KDQTYP , KDQFAM , KDQID ) = ( KDRIPB , KDRALX , KDRTYP , KDRFAM , KDRKDQID )",
                    codePeriode.IsEmptyOrNull() ? string.Empty : "KDQKDOID = KDRKDOID",
                    modeNavig == ModeConsultation.Historique ? "KDOAVN = KDQAVN" : string.Empty
                },
                ["KREAVEN"] = new[] { "KDRVEN = KGAVEN", "KDRFAM = KGAFAM" },
                [CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ")] = new[] {
                    "( KDRIPB , KDRALX , KDRTYP , KDRRSQ ) = ( KABIPB , KABALX , KABTYP , KABRSQ )",
                    modeNavig == ModeConsultation.Historique ? "KDOAVN = KABAVN" : string.Empty
                }
            };

            var dbParams = new HashSet<EacParameter> {
                new EacParameter("ipb", codeOffre.ToIPB()),
                new EacParameter("alx", DbType.Int32) { Value = int.Parse(version) },
                new EacParameter("typ", type),
                new EacParameter("per", codePeriode),
                new EacParameter("fam", famReassu.NullIfEmpty() ?? string.Empty)
            };
            if (codePeriode.IsEmptyOrNull()) {
                dbParams.RemoveWhere(p => p.ParameterName == "per");
            }

            string sqlStatement = $@"
SELECT {string.Join(", ", selection.Select(x => $"{x.Key} {x.Value}"))} 
FROM {mainTable} 
INNER JOIN {string.Join(" INNER JOIN ", joins.Select(x => $"{x.Key} ON {string.Join(" AND ", x.Value.Where(k => k.ContainsChars()))}"))}";

            var result = DbBase.Settings.ExecuteList<TraiteRisquesVentilationsDto>(CommandType.Text, sqlStatement, dbParams.ToArray());
            if (result is null) {
                return new TraiteInfoRsqVenDto {
                    TraiteRisques = new List<TraiteRisqueDto>(),
                    TraiteVentilations = new List<TraiteVentilationDto>()
                };
            }
            return InitializeTraiteInfoRsqVol(result, partAlb);
        }

        private static void UpdateTraiteVentilationGenerale(string codeOffre, string version, string type, TraiteVentilationDto ventilationGen, string user, string codePeriode)
        {
            Int64 engagFor = 0;
            if (ventilationGen.IdVentilation > 0 && Int64.TryParse(ventilationGen.EngagementVentilationForce, out engagFor))
            {
                string sql = string.Format(@"UPDATE KPENGVEN
                                                SET KDQENGF = {0},
                                                KDQENGOK = '{1}',
                                                KDQMAJU = '{6}',
                                                KDQMAJD = '{7}'
                                             WHERE KDQTYP = '{2}'
                                                AND KDQIPB = '{3}'
                                                AND KDQALX = {4}
                                                AND KDQID = {5}
                                                {8}", engagFor,
                                                      ventilationGen.VentilationSel ? 'O' : 'N',
                                                      type,
                                                      codeOffre.PadLeft(9, ' '),
                                                      version,
                                                      ventilationGen.IdVentilation,
                                                      user,
                                                      DateTime.Now.ToString("yyyyMMdd"),
                                                      string.Empty
                    //!string.IsNullOrEmpty(codePeriode) ? string.Format(" AND KDQKDOID = {0}", codePeriode) : string.Empty
                                                      );

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }
        }

        private static void UpdateTraiteRisque(string codeOffre, string version, string type, TraiteRisqueDto risque, string user, string codePeriode)
        {
            if (risque.CodeRisque > 0 && (risque.TraiteVentilations != null))
            {
                foreach (var ventilation in risque.TraiteVentilations)
                {
                    UpdateTraiteVentilation(codeOffre, version, type, risque.CodeRisque, ventilation, user, codePeriode);
                }
            }
        }

        private static void UpdateTraiteVentilation(string codeOffre, string version, string type, int codeRisque, TraiteVentilationDto ventilation, string user, string codePeriode)
        {
            if (!string.IsNullOrEmpty(codeOffre) && !string.IsNullOrEmpty(version) && !string.IsNullOrEmpty(type))
            {
                if (int.TryParse(version, out int iVersion))
                {
                    long.TryParse(ventilation.Engagement, out long engagFor);
                    if (ventilation.IdVentilation > 0) {
                        string sql = string.Format(@"
UPDATE KPENGRSQ SET KDRENGF = {0}, KDRENGOK = '{1}', KDRMAJU = '{7}', KDRMAJD = '{8}'
WHERE KDRTYP = '{2}' AND KDRIPB = '{3}' AND KDRALX = {4} AND KDRRSQ = {5} AND KDRKDQID = {6} {9}",
                            engagFor,
                            ventilation.RisqueSel ? 'O' : 'N',
                            type,
                            codeOffre.PadLeft(9, ' '),
                            iVersion,
                            codeRisque,
                            ventilation.IdVentilation,
                            user,
                            DateTime.Now.ToString("yyyyMMdd"),
                            !string.IsNullOrEmpty(codePeriode) ? string.Format("AND KDRKDOID = {0}", codePeriode) : string.Empty);

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                    }
                }
            }
        }

        private static TraiteInfoRsqVenDto InitializeTraiteInfoRsqVol(List<TraiteRisquesVentilationsDto> modele, double partAlb)
        {
            TraiteInfoRsqVenDto toReturn = new TraiteInfoRsqVenDto();

            List<TraiteVentilationDto> lstStrVentilation = new List<TraiteVentilationDto>();
            modele.GroupBy(v => new { v.IdVentilation }).ToList().ForEach(g => {
                var ligne = g.First();
                var traiteVentilationDto = new TraiteVentilationDto {
                    NomVentilation = ligne.NomVentilation,
                    IdVentilation = ligne.IdVentilation,
                    EngagementVentilationCalcule = ligne.EngagementVentilationCalcule.ToString(),
                    EngagementVentilationForce = ligne.EngagementVentilationForce.ToString(),
                    SMP = g.Sum(x => x.SMP > 0 ? x.SMP : x.SMPForce).ToString(),
                    SMPF = g.Sum(x => x.SMPForce > 0 ? x.SMPForce : x.SMP).ToString(),
                    VentilationSel = ligne.VentilationSel == "O",
                    Position = lstStrVentilation.Count + 1
                };

                lstStrVentilation.Add(traiteVentilationDto);
            });

            List<TraiteRisqueDto> listRsq = new List<TraiteRisqueDto>();
            // Sélectionner les risques (DISTINCT)
            modele.GroupBy(r => r.CodeRisque).Select(r => r.First()).ToList().ForEach(el =>
            {
                TraiteRisqueDto traiteRisqueDto = new TraiteRisqueDto();

                traiteRisqueDto.CodeRisque = el.CodeRisque;
                traiteRisqueDto.DescrRsq = el.DescrRsq;

                // Traitement des ventilations
                modele.Where(elem => elem.CodeRisque == el.CodeRisque).ToList().ForEach(
                    vl => traiteRisqueDto.TraiteVentilations.Add(
                        new TraiteVentilationDto
                        {
                            Engagement = vl.Engagement.ToString(),
                            EngagementVentilationCalcule = vl.EngagementVentilationCalcule.ToString(),
                            EngagementVentilationForce = vl.EngagementVentilationForce.ToString(),
                            CapitauxTotaux = vl.Engagement.ToString(),
                            CapitauxAlbingia = Math.Round(vl.Engagement * partAlb / 100, 0, MidpointRounding.AwayFromZero).ToString(),
                            LCI = vl.LCI.ToString(),
                            NomVentilation = vl.NomVentilation,
                            IdVentilation = vl.IdVentilation,
                            RisqueSel = vl.RisqueSel == "O",
                            SMP = (vl.SMPForce != 0 ? vl.SMPForce : vl.SMP).ToString(),
                            SMPAlbingia = Math.Round((vl.SMPForce != 0 ? vl.SMPForce : vl.SMP) * partAlb / 100, 0, MidpointRounding.AwayFromZero).ToString(),
                            VentilationSel = vl.VentilationSel == "O",
                            Position = lstStrVentilation.Where(v => v.NomVentilation == vl.NomVentilation).First().Position
                        }
                    )
                );

                listRsq.Add(traiteRisqueDto);
            });

            toReturn.TraiteRisques = listRsq.OrderBy(elm => elm.CodeRisque).ToList();
            toReturn.TraiteVentilations = lstStrVentilation.OrderBy(elm => elm.IdVentilation).ToList(); ;

            return toReturn;
        }

        #endregion

        #endregion

        #region SMP

        #region Methodes public static

        public static SMPdto ObtenirDetailCalculSMP(string argCodeOffre, string argVersion, string argType, string codeAvn, string risque, string ventilation, ModeConsultation modeNavig)
        {
            SMPdto toReturn = null;
            if (!string.IsNullOrEmpty(argCodeOffre) && !string.IsNullOrEmpty(argVersion) && !string.IsNullOrEmpty(argType) && !string.IsNullOrEmpty(risque) && !string.IsNullOrEmpty(ventilation))
            {
                int iVersion = 0;
                int iRisque = 0;
                int iVentilation = 0;
                if (int.TryParse(argVersion, out iVersion) && int.TryParse(risque, out iRisque) && int.TryParse(ventilation, out iVentilation))
                {
                    string sql = string.Format(@"SELECT
	                                tplib 		NomTraite,
	                                kdrrsq 		Risque,
	                                KGALIBV		NOMVENTILATION,
	                                kdrengc 	SMPtotal
                                FROM
	                                ((yyyypar a INNER JOIN {0} b ON a.tcon='spkrs' and a.tfam='garan' and a.tcod=b.kdpfam)
	                                INNER JOIN {1} c ON b.kdptyp=c.kdstyp and b.kdpipb=c.kdsipb and b.kdpalx=c.kdsalx {9})
	                                INNER JOIN {2} d ON c.kdsrsq=d.kdrrsq and c.kdsfam=d.kdrfam and c.kdsven=d.kdrven and c.kdstyp=d.kdrtyp and c.kdsipb=d.kdripb and c.kdsalx=d.kdralx {10}
	                                INNER JOIN KREAVEN ON KDRVEN = KGAVEN
                                WHERE
                                    KDRIPB = '{3}'
                                    AND KDRALX = {4}
                                    AND KDRTYP = '{5}'
                                    AND KDRRSQ = {6}
                                    AND KDSVEN = {7} {8}
                                FETCH FIRST 1 ROWS ONLY",
                                        CommonRepository.GetPrefixeHisto(modeNavig, "KPENGFAM"),
                                        CommonRepository.GetPrefixeHisto(modeNavig, "KPENGGAR"),
                                        CommonRepository.GetPrefixeHisto(modeNavig, "KPENGRSQ"),
                                         argCodeOffre.PadLeft(9, ' '),
                                         iVersion,
                                         argType,
                                         iRisque,
                                         iVentilation,
                                         modeNavig == ModeConsultation.Historique ? string.Format(" AND KDRAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                         modeNavig == ModeConsultation.Historique ? " AND B.KDPAVN = C.KDSAVN" : string.Empty,
                                         modeNavig == ModeConsultation.Historique ? " AND C.KDSAVN = D.KDRAVN" : string.Empty);

                    var result = DbBase.Settings.ExecuteList<SMPdto>(CommandType.Text, sql);

                    if (result != null && result.Any())
                    {
                        toReturn = result.FirstOrDefault();
                        toReturn.ListeGarantie = GetLignesSMP(argCodeOffre, argVersion, argType, codeAvn, risque, ventilation, modeNavig);
                    }
                    else
                    {
                        toReturn = new SMPdto();
                        toReturn.ListeGarantie = new List<LigneSMPdto>();

                        #region exemple données
                        toReturn = RempliExempleSMP();
                        #endregion
                    }
                }
            }

            if (toReturn == null)
            {
                toReturn = new SMPdto();
                toReturn.ListeGarantie = new List<LigneSMPdto>();

                #region exemple données
                toReturn = RempliExempleSMP();
                #endregion
            }

            return toReturn;
        }

        public static SMPdto RecalculDetailSMP(SMPdto argQuery, string argCodeOffre, string argVersion, string argType, string codeAvn, string risque, string ventilation, ModeConsultation modeNavig)
        {
            if (modeNavig == ModeConsultation.Standard)
            {
                foreach (LigneSMPdto ligne in argQuery.ListeGarantie)
                {
                    string sql = string.Format(@"UPDATE KPENGGAR
                                                SET KDSSMPU = '{0}',
                                                KDSSMPF = '{1}'
                                             WHERE
                                                KDSID = {2}
                                                AND KDSTYP = '{3}'
                                                AND KDSIPB = '{4}'
                                                AND KDSALX = {5}",
                                                 ligne.Type,
                                                 ligne.Valeur,
                                                 ligne.IdGarantie,
                                                 argType,
                                                 argCodeOffre.PadLeft(9, ' '),
                                                 argVersion);

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            return ObtenirDetailCalculSMP(argCodeOffre, argVersion, argType, risque, codeAvn, ventilation, modeNavig);
        }

        #endregion

        #region Methodes private static

        private static List<LigneSMPdto> GetLignesSMP(string argCodeOffre, string argVersion, string argType, string codeAvn, string risque, string ventilation, ModeConsultation modeNavig)
        {
            if (!string.IsNullOrEmpty(argCodeOffre) && !string.IsNullOrEmpty(argVersion) && !string.IsNullOrEmpty(argType) && !string.IsNullOrEmpty(risque) && !string.IsNullOrEmpty(ventilation))
            {
                int iVersion = 0;
                int iRisque = 0;
                int iVentilation = 0;
                if (int.TryParse(argVersion, out iVersion) && int.TryParse(risque, out iRisque) && int.TryParse(ventilation, out iVentilation))
                {
                    string sql = string.Format(@"SELECT
	                                kdsid 		IdGarantie,
	                                kdsengok 	CheckBox,
	                                kdsgaran 	Garantie,
	                                kdssmp 		SMPcalcule,
	                                kdssmpu 	TypeSMPforce,
	                                kdssmpf 	ValeurSMPforce,
	                                kdssmpr 	SMPretenu
                                FROM
	                                ((yyyypar a INNER JOIN {0} b ON a.tcon='spkrs' and a.tfam='garan' and a.tcod=b.kdpfam)
	                                INNER JOIN {1} c ON b.kdptyp=c.kdstyp and b.kdpipb=c.kdsipb and b.kdpalx=c.kdsalx {10})
	                                INNER JOIN {2} d ON c.kdsrsq=d.kdrrsq and c.kdsfam=d.kdrfam and c.kdsven=d.kdrven and c.kdstyp=d.kdrtyp and c.kdsipb=d.kdripb and c.kdsalx=d.kdralx {11}
	                                INNER JOIN KREAVEN ON KDRVEN = KGAVEN
                                WHERE
                                    KDRIPB = '{3}'
                                    AND KDRALX = {4}
                                    AND KDRTYP = '{5}'
                                    AND KDRRSQ = {6}
                                    AND KDSVEN = {7} {8}",
                                            CommonRepository.GetPrefixeHisto(modeNavig, "KPENGFAM"),
                                            CommonRepository.GetPrefixeHisto(modeNavig, "KPENGGAR"),
                                            CommonRepository.GetPrefixeHisto(modeNavig, "KPENGRSQ"),
                                            argCodeOffre.PadLeft(9, ' '),
                                            iVersion,
                                            argType,
                                            iRisque,
                                            iVentilation,
                                            modeNavig == ModeConsultation.Historique ? string.Format(" AND KDRAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                            modeNavig == ModeConsultation.Historique ? " AND B.KDPAVN = C.KDSAVN" : string.Empty,
                                            modeNavig == ModeConsultation.Historique ? " AND C.KDSAVN = D.KDRAVN" : string.Empty);

                    return DbBase.Settings.ExecuteList<LigneSMPdto>(CommandType.Text, sql);
                }
            }
            return new List<LigneSMPdto>();
        }

        private static SMPdto RempliExempleSMP()
        {
            SMPdto result = new SMPdto();
            result.NomTraite = "leNomDuTraite";
            result.Risque = 1;
            result.Ventilation = "LeNomDuVolet";
            result.SMPtotal = 0;
            result.ListeGarantie = new List<LigneSMPdto>();
            LigneSMPdto ligne = null;
            for (int i = 0; i <= 6; i++)
            {
                ligne = new LigneSMPdto();
                ligne.IdGarantie = i;
                ligne.CheckBox = "O";// (i % 2 != 0 ? "O" : "N");
                ligne.NomGarantie = "Garantie " + i.ToString();
                ligne.LCI = "50" + i;
                ligne.SMPcalcule = 10 + i;
                ligne.Type = (i % 2 != 0 ? "D" : "%");
                ligne.Valeur = 2 + i;
                ligne.SMPretenu = 0;

                result.ListeGarantie.Add(ligne);
            }
            ligne = new LigneSMPdto();
            ligne.IdGarantie = 7;
            ligne.CheckBox = "O";
            ligne.NomGarantie = "Garantie 7";
            ligne.LCI = "507";
            ligne.SMPcalcule = 107;
            ligne.Type = "";
            ligne.Valeur = 0;
            ligne.SMPretenu = 0;

            result.ListeGarantie.Add(ligne);
            return result;
        }

        #endregion

        #endregion

        #region Connexité

        public static void SaveObsvConnexite(string codeAffaire, string version, string type, int codeObservation, string observation, string acteGestion, string reguleId)
        {
            string sql = string.Empty;

            if (codeObservation == 0)
            {
                codeObservation = CommonRepository.GetAS400Id("KAJCHR");
                sql = string.Format(@"INSERT INTO KPOBSV
                                            (KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJTYPOBS, KAJOBSV)
                                        VALUES
                                            ({0}, '{1}', '{2}', {3}, '{4}', '{5}')",
                                    codeObservation, type, codeAffaire.PadLeft(9, ' '), version, "GENERALE", !string.IsNullOrEmpty(observation) ? observation.Replace("'", "''") : string.Empty);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

                if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && !string.IsNullOrEmpty(reguleId) && reguleId != "0")
                {
                    sql = string.Format(@"UPDATE KPRGU SET KHWOBSV = {1} WHERE KHWID = {0}",
                                 reguleId, codeObservation);
                }
                else
                {
                    sql = string.Format(@"UPDATE KPENT SET KAAOBSV = {3} WHERE KAAIPB = '{0}' AND KAAALX = {1} AND KAATYP = '{2}'",
                                    codeAffaire.Trim().PadLeft(9, ' '), version, type, codeObservation);
                }
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }
            else
            {
                sql = string.Format(@"UPDATE KPOBSV
                                         SET KAJOBSV = TRIM ( '{0}' )
                                         WHERE KAJCHR = {1} ", !string.IsNullOrEmpty(observation) ? observation.Replace("'", "''") : string.Empty, codeObservation);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }
        }

        public static string GetNumeroConnexite(string codeOffre, string version, string type, string codeTypeConnexite)
        {
            string sql = @"SELECT PJCNX NUMCONNEXITE from YPOCONX
                            WHERE  PJIPB='{0}' AND PJALX ='{1}' AND PJTYP ='{2}' AND PJCCX = '{3}'";
            var connexite = DbBase.Settings.ExecuteList<ContratConnexeDto>(CommandType.Text, string.Format(sql, codeOffre.ToUpper().PadLeft(9, ' '), version, type, codeTypeConnexite)).FirstOrDefault();
            return connexite != null ? connexite.NumConnexite.ToString() : string.Empty;

        }
        public static List<ContratConnexeDto> GetContratsConnexes(string typeOffre, string codeTypeConnexite, string numeroConnexite)
        {
            string sql = string.Format(@"SELECT
                            PJCNX NUMCONNEXITE,
                            PJIDE IDECONNEXITE,
                            PJCCX CODETYPECONNEXITE,
                            PJIPB NUMCONTRAT,
                            PJALX VERSIONCONTRAT,
                            PJTYP TYPECONTRAT,
                            PBREF DESCRIPTIONCONTRAT,
                            BASE.PBBRA CODEBRANCHE,
                            PAR.TPLIL LIBELLEBRANCHE,
                            BASE.PBIAS CODEPRENEUR,
                            NOM.ANNOM NOMPRENEUR,
                            RSQ.KABCIBLE CODECIBLE,
                            CIBLE.KAHDESC LIBELLECIBLE,
                            PJOBS CODEOBSERVATION,
                            TRIM(OBSV.KAJOBSV) OBSERVATION,
                            ASSUR.ASAD1 AD1,
                            ASSUR.ASAD2 AD2,
                            ASSUR.ASDEP DEP,
                            ASSUR.ASCPO CP,
                            ASSUR.ASVIL VILLE,
                            PBSIT SITUATION,
                            PBETA ETAT
                            FROM YPOCONX
                            INNER JOIN YPOBASE BASE ON LOWER(TRIM(BASE.PBIPB))=LOWER(TRIM(PJIPB)) AND BASE.PBALX=PJALX AND BASE.PBTYP=PJTYP
                            {3}
                            LEFT JOIN KPRSQ RSQ ON RSQ.KABIPB=PJIPB AND RSQ.KABALX=PJALX AND RSQ.KABTYP=PJTYP AND KABRSQ='1'
                            LEFT JOIN KCIBLE CIBLE ON CIBLE.KAHCIBLE=RSQ.KABCIBLE
                            LEFT JOIN KPOBSV OBSV ON OBSV.KAJCHR=PJOBS
                            LEFT JOIN YASSNOM NOM  ON NOM.ANIAS = BASE.PBIAS AND NOM.ANINL = 0 AND NOM.ANTNM = 'A'
                            LEFT JOIN YASSURE ASSUR ON ASSUR.ASIAS=BASE.PBIAS
                            WHERE PJTYP='{0}' AND PJCCX ='{1}' AND PJCNX ='{2}'", typeOffre, codeTypeConnexite, numeroConnexite,
                            CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "BRCHE", "PAR", " AND PAR.TCOD= PJBRA AND PAR.TPCN2 = 1"));

            return DbBase.Settings.ExecuteList<ContratConnexeDto>(CommandType.Text, sql);
        }

        public static List<ContratConnexeDto> GetContratsConnexesEngagement(string typeOffre, string codeTypeConnexite, string numeroConnexite)
        {
            string sql = string.Format(@"SELECT  PJCNX NUMCONNEXITE,
                                                 PJIDE IDECONNEXITE,
                                                 PJCCX CODETYPECONNEXITE,
                                                 PJIPB NUMCONTRAT,
                                                 PJALX VERSIONCONTRAT,
                                                 PJTYP TYPECONTRAT,
                                                 PBREF DESCRIPTIONCONTRAT,
                                                 PBBRA CODEBRANCHE,
                                                 TPLIL LIBELLEBRANCHE,
                                                 PBIAS CODEPRENEUR,
                                                 NOM.ANNOM NOMPRENEUR,
                                                 KAACIBLE CODECIBLE,
                                                 KAHDESC LIBELLECIBLE,
                                                 PJOBS CODEOBSERVATION,
                                                 IFNULL(OBSV.KAJOBSV,'') OBSERVATION,
                                                 PBSIT SITUATION,
                                                 PBETA ETAT,
                                                 KDPFAM CODEENGAGEMENT,
                                                 KDPENA VALEURENGAGEMENT
                            FROM YPOCONX
                                    LEFT JOIN KPENG  ON KDOTYP = PJTYP AND KDOIPB = PJIPB AND  KDOALX = PJALX
                                    LEFT JOIN KPENGFAM ON KDPKDOID = KDOID
                                    INNER JOIN YPOBASE  ON PBIPB = PJIPB AND PBALX = PJALX AND PBTYP = PJTYP
                                    LEFT JOIN YYYYPAR ON  TCON ='GENER' AND TFAM = 'BRCHE' AND TCOD= PBBRA
                                    LEFT JOIN KPENT ON KAAIPB= PJIPB AND KAAALX = PJALX AND KAATYP = PJTYP
                                    LEFT JOIN KCIBLE ON  KAACIBLE = KAHCIBLE
                                    LEFT JOIN KPOBSV OBSV ON OBSV.KAJCHR = PJOBS
                                    LEFT JOIN YASSNOM NOM  ON NOM.ANIAS = PBIAS AND NOM.ANINL = 0 AND NOM.ANTNM = 'A'
                            WHERE PJTYP='{0}' AND PJCCX ='{1}' AND PJCNX ='{2}'", typeOffre, codeTypeConnexite, numeroConnexite);
            return DbBase.Settings.ExecuteList<ContratConnexeDto>(CommandType.Text, sql);
        }
        public static bool IsContratInConnexite(string codeOffre, string version, string type, string codeTypeConnexite, string numConnexite)
        {
            var (sql, param) = MakeParamsSql(@"SELECT PJCNX NUMCONNEXITE from YPOCONX
                            WHERE  PJIPB=:ipb AND PJALX =:alx AND PJTYP =:typs AND PJCCX = :ccx AND PJCNX = :cnx",
                            codeOffre.PadLeft(9, ' '), version, type, codeTypeConnexite, numConnexite
                            );
            var connexite = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param);
            return (connexite != null && connexite != DBNull.Value) ? true : false;
        }

        public static string AddConnexite(string codeOffre_connexe, string version_connexe, string type_connexe, string branche_connexe, string sousBranche_connexe, string categorie_connexe,
                                          string codeOffre_courant, string version_courant, string type_courant, string branche_courant, string sousBranche_courant, string categorie_courant,
                                          string codeObservation, string observation, string codeTypeConnexite, string numConnexite, string mode)
        {
            if (string.IsNullOrEmpty(numConnexite))
                numConnexite = CommonRepository.GenererNumeroChrono("POLICE_CONNEX_" + codeTypeConnexite).ToString();

            DbParameter[] param = new DbParameter[18];
            param[0] = new EacParameter("P_ID_OFFRE_CONNEXE", codeOffre_connexe.ToUpper().PadLeft(9, ' '));
            param[1] = new EacParameter("P_TYPE_OFFRE_CONNEXE", type_connexe);
            param[2] = new EacParameter("P_ID_ALIMENT_CONNEXE", version_connexe);
            param[3] = new EacParameter("P_BRANCHE_CONNEXE", branche_connexe);
            param[4] = new EacParameter("P_SOUS_BRANCHE_CONNEXE", sousBranche_connexe);
            param[5] = new EacParameter("P_CAT_CONNEXE", categorie_connexe);

            param[6] = new EacParameter("P_ID_OFFRE_COURANT", codeOffre_courant.ToUpper().PadLeft(9, ' '));
            param[7] = new EacParameter("P_TYPE_OFFRE_COURANT", type_courant);
            param[8] = new EacParameter("P_ID_ALIMENT_COURANT", version_courant);
            param[9] = new EacParameter("P_BRANCHE_COURANT", branche_courant);
            param[10] = new EacParameter("P_SOUS_BRANCHE_COURANT", sousBranche_courant);
            param[11] = new EacParameter("P_CAT_COURANT", categorie_courant);

            param[12] = new EacParameter("P_TYPE_CONNEXITE", codeTypeConnexite);
            param[13] = new EacParameter("P_NUM_CONNEXITE", numConnexite);
            param[14] = new EacParameter("P_OBSV", observation);
            param[15] = new EacParameter("P_CODE_OBSV", int.Parse(codeObservation));

            param[16] = new EacParameter("P_MODE", mode);
            param[17] = new EacParameter("P_ERREUR", "");
            param[17].Direction = ParameterDirection.InputOutput;
            param[17].Size = 256;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDCNX", param);
            return param[17].Value.ToString();
        }


        public static string AddConnexiteEngagement(string codeOffre_connexe, string version_connexe, string type_connexe, string branche_connexe, string sousBranche_connexe, string categorie_connexe,
                                          string codeOffre_courant, string version_courant, string type_courant, string branche_courant, string sousBranche_courant, string categorie_courant,
                                          string codeObservation, string observation, string codeTypeConnexite, string numConnexite, string mode)
        {
            if (string.IsNullOrEmpty(numConnexite))
                numConnexite = CommonRepository.GenererNumeroChrono("POLICE_CONNEX_" + codeTypeConnexite).ToString();

            DbParameter[] param = new DbParameter[18];
            param[0] = new EacParameter("P_ID_OFFRE_CONNEXE", codeOffre_connexe.ToUpper().PadLeft(9, ' '));
            param[1] = new EacParameter("P_TYPE_OFFRE_CONNEXE", type_connexe);
            param[2] = new EacParameter("P_ID_ALIMENT_CONNEXE", version_connexe);
            param[3] = new EacParameter("P_BRANCHE_CONNEXE", branche_connexe);
            param[4] = new EacParameter("P_SOUS_BRANCHE_CONNEXE", sousBranche_connexe);
            param[5] = new EacParameter("P_CAT_CONNEXE", categorie_connexe);

            param[6] = new EacParameter("P_ID_OFFRE_COURANT", codeOffre_courant.ToUpper().PadLeft(9, ' '));
            param[7] = new EacParameter("P_TYPE_OFFRE_COURANT", type_courant);
            param[8] = new EacParameter("P_ID_ALIMENT_COURANT", version_courant);
            param[9] = new EacParameter("P_BRANCHE_COURANT", branche_courant);
            param[10] = new EacParameter("P_SOUS_BRANCHE_COURANT", sousBranche_courant);
            param[11] = new EacParameter("P_CAT_COURANT", categorie_courant);

            param[12] = new EacParameter("P_TYPE_CONNEXITE", codeTypeConnexite);
            param[13] = new EacParameter("P_NUM_CONNEXITE", numConnexite);
            param[14] = new EacParameter("P_OBSV", observation);
            param[15] = new EacParameter("P_CODE_OBSV", int.Parse(codeObservation));

            param[16] = new EacParameter("P_MODE", mode);
            param[17] = new EacParameter("P_ERREUR", "");
            param[17].Direction = ParameterDirection.InputOutput;
            param[17].Size = 256;
            //DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SAB_ADDCNXENG", param);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDCNXENG", param);
            return param[17].Value.ToString();
        }



        public static string DeleteConnexite(string codeOffre_connexe, string version_connexe, string typeOffre_connexe, string numConnexite, string type_connexe, string ideConnexite, string user)
        {
            DbParameter[] param = new DbParameter[9];
            param[0] = new EacParameter("P_ID_OFFRE_CONNEXE", codeOffre_connexe.ToUpper().PadLeft(9, ' '));
            param[1] = new EacParameter("P_ALIMENT_CONNEXE", version_connexe);
            param[2] = new EacParameter("P_TYPE_OFFRE_CONNEXE", typeOffre_connexe);
            param[3] = new EacParameter("P_TYPE_CONNEXITE", type_connexe);
            param[4] = new EacParameter("P_NUM_CONNEXITE", numConnexite);
            param[5] = new EacParameter("P_IDE", int.Parse(ideConnexite));
            param[6] = new EacParameter("P_MAJ_USER", user);
            param[7] = new EacParameter("P_MAJ_DATE", AlbConvert.ConvertDateToInt(DateTime.Now).Value);

            param[8] = new EacParameter("P_ERREUR", "");
            param[8].Direction = ParameterDirection.InputOutput;
            param[8].Size = 256;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELCNX", param);
            return param[8].Value.ToString();
        }

        public static void DeleteConnexiteEng(string codeOffre , int version  , string type)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", version);
            param[2] = new EacParameter("P_TYPE", type);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELCNXENG", param);
            //DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SAB_DELCNXENG", param);

        }


        #endregion

        public static bool IsContratConnexe(string codeOffre, string version, string type, string codeTypeConnexite)
        {
            var (sql, param) = MakeParamsSql(
                            @"SELECT PJCNX NUMCONNEXITE from YPOCONX
                            WHERE PJIPB = :ipb AND PJALX = :alx AND PJTYP = :ipb AND PJCCX = :ccx",
                            codeOffre.ToIPB(), version, type, codeTypeConnexite);
            var connexite = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param);
            return (connexite != null && connexite != DBNull.Value) ? true : false;
        }

        public static string FusionDetachConnexite(
            string numOffreOrigine, string typeOffreOrigine, string versionOffreOrigine, string brancheOrigine, string sousBrancheOrigine, string catOrigine,
            string numConnexiteOrigine, long codeObsvOrigine, string obsvOrigine, string ideConnexiteOrigine,
            string numOffreActuelle, string typeOffreActuelle, string versionOffreActuelle, string brancheActuelle, string sousBrancheActuelle, string catActuelle,
            string numConnexiteActuelle, long codeObsvActuelle, string obsvActuelle, string codeTypeConnexite,
            string user, string modeAction)
        {
            if (string.IsNullOrEmpty(numConnexiteActuelle))
                numConnexiteActuelle = CommonRepository.GenererNumeroChrono("POLICE_CONNEX_" + codeTypeConnexite).ToString();

            string obsv = string.Empty;
            if (!string.IsNullOrEmpty(obsvActuelle) && !string.IsNullOrEmpty(obsvOrigine))
            {
                obsv = obsvActuelle + Environment.NewLine + obsvOrigine;
                if (obsv.Length > 5000)
                    return "La taille de la concaténation des commentaires ne doit pas dépasser 5000 caractères";
            }
            else
            {
                if (!string.IsNullOrEmpty(obsvActuelle))
                    obsv = obsvActuelle;
                else if (!string.IsNullOrEmpty(obsvOrigine))
                    obsv = obsvOrigine;
            }

            DbParameter[] param = new DbParameter[23];
            param[0] = new EacParameter("P_TYPE_CONNEXITE", codeTypeConnexite);

            param[1] = new EacParameter("P_ID_OFFRE_ORIGINE", numOffreOrigine.ToUpper().PadLeft(9, ' '));
            param[2] = new EacParameter("P_TYPE_OFFRE_ORIGINE", typeOffreOrigine);
            param[3] = new EacParameter("P_ALIMENT_ORIGINE", versionOffreOrigine);
            param[4] = new EacParameter("P_BRANCHE_ORIGINE", brancheOrigine);
            param[5] = new EacParameter("P_SOUS_BRANCHE_ORIGINE", sousBrancheOrigine);
            param[6] = new EacParameter("P_CAT_ORIGINE", catOrigine);
            param[7] = new EacParameter("P_NUM_CONNEXITE_ORIGINE", numConnexiteOrigine);
            param[8] = new EacParameter("P_CODE_OBSV_ORIGINE", int.Parse(codeObsvOrigine.ToString()));
            param[9] = new EacParameter("P_IDE_ORIGINE", int.Parse(ideConnexiteOrigine));

            param[10] = new EacParameter("P_ID_OFFRE_ACTUELLE", numOffreActuelle.ToUpper().PadLeft(9, ' '));
            param[11] = new EacParameter("P_TYPE_OFFRE_ACTUELLE", typeOffreActuelle);
            param[12] = new EacParameter("P_ALIMENT_ACTUELLE", versionOffreActuelle);
            param[13] = new EacParameter("P_BRANCHE_ACTUELLE", brancheActuelle);
            param[14] = new EacParameter("P_SOUS_BRANCHE_ACTUELLE", sousBrancheActuelle);
            param[15] = new EacParameter("P_CAT_ACTUELLE", catActuelle);
            param[16] = new EacParameter("P_NUM_CONNEXITE_ACTUELLE", numConnexiteActuelle);
            param[17] = new EacParameter("P_CODE_OBSV_ACTUELLE", int.Parse(codeObsvActuelle.ToString()));

            param[18] = new EacParameter("P_OBSV", obsv);

            param[19] = new EacParameter("P_MAJ_USER", user);
            param[20] = new EacParameter("P_MAJ_DATE", AlbConvert.ConvertDateToInt(DateTime.Now).Value);

            param[21] = new EacParameter("P_MODE_ACTION", modeAction);

            param[22] = new EacParameter("P_ERREUR", "");
            param[22].Direction = ParameterDirection.InputOutput;
            param[22].Size = 256;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_FDCNX", param);
            return param[22].Value.ToString();
        }
        public static List<EngagementConnexiteTraiteDto> GetEngagementsTraites(string idEngagement)
        {
            string sql = string.Format(@"SELECT
	                       KDPID IDTRAITE,
	                       KDPKDOID IDENGAGEMENT,
                           KDPFAM CODETRAITE,
                           TPLIL LIBELLETRIATE,
                           KDPENA ENGAGEMENTALBINGIA,
                           KDPENG ENGAGEMENTTOTAL
                           FROM KPENGFAM
                           {1}
                           INNER JOIN KPENG ON  KDOID=KDPKDOID AND KDOECO ='O'
                           WHERE KDPKDOID='{0}'", idEngagement,
                       CommonRepository.BuildJoinYYYYPAR("INNER", "REASS", "GARAN", otherCriteria: " AND TCOD = KDPFAM"));

            return DbBase.Settings.ExecuteList<EngagementConnexiteTraiteDto>(CommandType.Text, sql);
        }
        public static List<ContratConnexeTraiteDto> GetContratsConnexesTraite(string typeOffre, string codeTypeConnexite, string numeroConnexite)
        {
            string sql = @"SELECT PJIDE IDECONNEXITE,
	                           PJCNX NUMCONNEXITE,
                               PJIPB NUMCONTRAT,
                               PJALX VERSIONCONTRAT,
                               KDODATD DATEDEBUTENGAGEMENT,
                               KDODATF DATEFINENGAGEMENT,
                               KDOID IDENGAGEMENT
                               FROM YPOCONX
                               INNER JOIN KPENG ON KDOTYP=PJTYP AND KDOIPB=PJIPB AND KDOALX=PJALX
                               WHERE PJTYP='{0}' AND PJCCX ='{1}' AND PJCNX ='{2}' AND KDOECO='O'
                               ORDER BY PJIPB, PJALX";
            return DbBase.Settings.ExecuteList<ContratConnexeTraiteDto>(CommandType.Text, string.Format(sql, typeOffre, codeTypeConnexite, numeroConnexite));
        }

        public static List<EngagementConnexiteDto> GetEngagementsConnexite(string IdeConnexiteEngagement)
        {
            string sql = string.Format(@"SELECT
                              KDODATD DATEDEBUTENGAGEMENT,
                              KDODATF DATEFINENGAGEMENT,
                              KDOID IDENGAGEMENT,
                              KDOACT STATUTACTIF,
                              CASE WHEN KDOACT = 'O' THEN 'A' ELSE 'I' END MODEACTIF,
                              CASE WHEN HDSMP IS NULL THEN 'N' ELSE 'U' END MODEUTILISE,
                              KDODATD DATEDEBUTENGAGEMENT,
                              KDODATF DATEFINENGAGEMENT
                              FROM KPENG
                              LEFT JOIN YPORHMD ON KPENG.KDOID = YPORHMD.HDSMP
                               WHERE KDOENGID='{0}'
                               ORDER BY KDODATD", IdeConnexiteEngagement);
            return DbBase.Settings.ExecuteList<EngagementConnexiteDto>(CommandType.Text, sql);
        }

        public static long UpdateEngagementTraite(string codeOffre, string versionOffre, string typeOffre, int dateDeb, int dateFin, int idEng, int idTraite, string codeFamille, long engTotal, long engAlbin, string user, string modeMaj)
        {
            DbParameter[] param = new DbParameter[14];
            param[0] = new EacParameter("P_TYPE_OFFRE", typeOffre);
            param[1] = new EacParameter("P_ID_OFFRE", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P_VERSION_OFFRE", versionOffre);
            param[3] = new EacParameter("P_DATE_DEBUT", dateDeb);
            param[4] = new EacParameter("P_DATE_FIN", dateFin);
            param[5] = new EacParameter("P_ID_ENGAGEMENT", idEng);
            param[6] = new EacParameter("P_ID_TRAITE", idTraite);
            param[7] = new EacParameter("P_FAMILLE", codeFamille);
            param[8] = new EacParameter("P_ENG_TOTAL", engTotal);
            param[9] = new EacParameter("P_ENG_ALBIN", engAlbin);
            param[10] = new EacParameter("P_MAJ_USER", user);
            param[11] = new EacParameter("P_MAJ_DATE", AlbConvert.ConvertDateToInt(DateTime.Now));
            param[12] = new EacParameter("P_MODE_MAJ", modeMaj);

            param[13] = new EacParameter("P_RETOUR", 0);
            param[13].Direction = ParameterDirection.InputOutput;
            param[13].Size = 256;
            param[13].Value = 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPEGTR", param);
            return long.Parse(param[13].Value.ToString());
        }
        public static string AddEngagementFamille(long idEng, int dateDeb, int dateFin, string codeFamille, long engTotal, long engAlbin, string user)
        {
            DbParameter[] param = new DbParameter[11];
            param[0] = new EacParameter("P_DATE_DEBUT", dateDeb);
            param[1] = new EacParameter("P_DATE_FIN", dateFin);
            param[2] = new EacParameter("P_ID_ENGAGEMENT", idEng);
            param[3] = new EacParameter("P_FAMILLE", codeFamille);
            param[4] = new EacParameter("P_ENG_TOTAL", engTotal);
            param[5] = new EacParameter("P_ENG_ALBIN", engAlbin);
            param[6] = new EacParameter("P_CR_USER", user);
            param[7] = new EacParameter("P_CR_DATE", AlbConvert.ConvertDateToInt(DateTime.Now));
            param[8] = new EacParameter("P_MAJ_USER", user);
            param[9] = new EacParameter("P_MAJ_DATE", AlbConvert.ConvertDateToInt(DateTime.Now));

            param[10] = new EacParameter("P_ERREUR", "");
            param[10].Direction = ParameterDirection.InputOutput;
            param[10].Size = 256;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_INEGFM", param);
            return param[10].Value.ToString();
        }

        public static bool CheckDatePeriode(string codeOffre, string version, string type, string codePeriode)
        {
            DbParameter[] param = new DbParameter[4];

            param[0] = new EacParameter("codePeriode", 0);
            param[0].Value = !string.IsNullOrEmpty(codePeriode) ? Convert.ToInt32(codePeriode) : 0;
            param[1] = new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' '));
            param[2] = new EacParameter("version", 0);
            param[2].Value = Convert.ToInt32(version);
            param[3] = new EacParameter("type", type);


            string sql = @"SELECT KDODATD DATEDEBRETURNCOL, (PBEFA * 10000 + PBEFM * 100 + PBEFJ) DATEFINRETURNCOL
                                FROM KPENG
                                    INNER JOIN YPOBASE ON PBIPB = KDOIPB AND PBALX = KDOALX AND PBTYP = KDOTYP
                            WHERE KDOID = :codePeriode AND KDOIPB = :codeOffre AND KDOALX = :version AND KDOTYP = :type";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                if (result.FirstOrDefault().DateDebReturnCol < result.FirstOrDefault().DateFinReturnCol)
                {
                    return false;
                }
            }
            return true;
        }

        public static void InsertPeriodeCnx(long codeEng, DateTime? dateDebut, DateTime? dateFin, Dictionary<string, long> traites, string codeOffre , int version ,string type ,string user)
        {
            var now = AlbConvert.ConvertDateToInt(DateTime.Now);
            var idEng = CommonRepository.GetAS400Id("KDOID");

            var sql = string.Format(@"INSERT INTO KPENG(KDOID,KDODATD,KDODATF,KDOCRU,KDOCRD,KDOMAJU,KDOMAJD)
                                      VALUES		   ({0},{1},{2},'{4}',{3},'{4}',{3})", idEng, AlbConvert.ConvertDateToInt(dateDebut) ??  0, AlbConvert.ConvertDateToInt(dateFin) ?? 0,now,user);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);



            foreach (var key in traites.Keys)
            {
                var idFam = CommonRepository.GetAS400Id("KDPID");
                sql = string.Format(@"INSERT INTO KPENGFAM (KDPID,KDPKDOID,KDPFAM,KDPENA,KDPCRU,KDPCRD,KDPMAJU,KDPMAJD)
                                      VALUES                ({0},{1},'{2}',{3},'{5}',{4},'{5}',{4})", idFam, idEng, key, traites[key], now, user);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }

            sql = string.Format(@"SELECT PJCNX INT32RETURNCOL,PJIDE ID FROM YPOCONX WHERE PJIPB  = '{0}' AND PJALX = {1} AND PJTYP = '{2}' AND PJCCX = {3} ", codeOffre.PadLeft(9, ' '), version, type, 20);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if(result != null )
            {
                var first  = result.FirstOrDefault();
                if(first != null )
                {
                    var pjcnx = first.Int32ReturnCol;
                    var pjide = first.Id;

                    if(pjide == 0)
                    {
                        var idCnx = CommonRepository.GetAS400Id("KIEPJID");
                        sql = string.Format(@"INSERT INTO KPENGCNX(KIEPJID,KIEORD,KIEKDOID)
                                              VALUES              ({0},1,{1})",idCnx ,idEng);
                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

                        sql = string.Format(@"UPDATE YPOCONX SET PJIDE = {0}  WHERE  PJCNX = {1}", idCnx, pjcnx);
                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                    }
                    else
                    {
                        sql = string.Format(@"INSERT INTO KPENGCNX(KIEPJID,KIEORD,KIEKDOID)
                                              VALUES              ({0},(SELECT IFNULL(MAX(KIEORD)+1,1) FROM KPENGCNX WHERE KIEPJID = {0}),{1})", pjide, idEng);
                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);
                    }

                }
            }


        }

        public static void UpdatePeriodeCnx(long codeEng, DateTime? dateDebut, DateTime? dateFin, Dictionary<string, long> traites , string user)
        {
            var now = AlbConvert.ConvertDateToInt(DateTime.Now);
            var sql = string.Format(@"UPDATE KPENG SET KDODATD = {0},KDODATF = {1},KDOMAJU  = '{4}', KDOMAJD = {3} WHERE KDOID  = {2}  ", AlbConvert.ConvertDateToInt(dateDebut) ?? 0, AlbConvert.ConvertDateToInt(dateFin) ?? 0, codeEng, now, user);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);

            foreach (var key in traites.Keys)
            {
                sql = string.Format(@"UPDATE KPENGFAM SET KDPENA = {0} ,KDPMAJU = '{4}',KDPMAJD ={3} WHERE KDPKDOID  = {1} AND KDPFAM  = '{2}'", traites[key], codeEng, key, now, user);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);
            }
        }

        public static void DeletePeriodeCnx(long codeEng)
        {

            var sql = string.Format("SELECT KIEPJID FROM KPENGCNX WHERE KIEKDOID = {0}" ,codeEng);
            var key = (long)DbBase.Settings.ExecuteScalar(CommandType.Text, sql, null);

            sql = string.Format(@"DELETE FROM KPENG WHERE KDOID  = {0}",codeEng);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);
            sql = string.Format(@"DELETE FROM KPENGFAM WHERE KDPKDOID  = {0}", codeEng);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);
            sql = string.Format(@"DELETE FROM KPENGCNX WHERE KIEKDOID  = {0}", codeEng);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);

            sql = string.Format(@"UPDATE YPOCONX SET PJIDE = 0  WHERE PJIDE = {0} AND NOT EXISTS(SELECT KIEPJID FROM KPENGCNX WHERE  KIEPJID = {0})", key);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);
        }


    }
}
