
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
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess {
    public partial class RegularisationRepository : RepositoryBase {

        internal static readonly string CountReportCharge = "SELECT COUNT(*) NBLIGN FROM KPRGUC WHERE KIMIPB = :codeContrat AND KIMALX = :version AND KIMTYP = :type AND KIMSIT = :sit";
        internal static readonly string UpdateRegulReportCharge = @"
UPDATE KPRGU SET KHWSREP = ( 
    SELECT KIMSCHG FROM KPRGUC WHERE KIMIPB = :codeContrat AND KIMALX = :version AND KIMTYP = :type AND KIMSIT = :sit 
) 
WHERE KHWID = :rgid ;";
        internal static readonly string UpdateDateAndNumeroAvenant = "UPDATE KPRGU SET KHWAVND = :dateAvt WHERE KHWIPB = :codeOffre AND KHWALX = :version AND KHWTYP = :type  AND KHWAVN = :numAvt ;";
        internal static readonly string SelectByFolder = $@"
SELECT KHWID NUMREG, KHWAVN CODEAVN, KHWTTR CODETRAITEMENT, TRAITEMENT.TPLIB LIBTRAITEMENT, 
    KHWDEBP DATEDEB, KHWFINP DATEFIN, KHWETA CODEETAT, KHWSIT CODESITUATION, SITUATION.TPLIB LIBSITUATION, 
    KHWSTD DATESIT, KHWSTH HEURESIT, KHWSTU USERSIT, 
	IFNULL(PKIPK, 0) NUMQUITT, IFNULL(PKSIT, '') CODEETATQUITT, IFNULL(ETATQUITT.TPLIB, '') LIBETATQUITT,PKAVI AVIS, 
    KHWMRG REGULMODE, KHWSUAV REGULTYPE, KHWACC REGULNIV, KHWRGAV REGULAVN 
FROM KPRGU 
LEFT JOIN YPRIMES ON ( PKIPB , PKALX , PKIPK , PKAVN ) = ( KHWIPB , KHWALX , KHWIPK , KHWAVN ) 
{CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", "TRAITEMENT", " AND TRAITEMENT.TCOD = KHWTTR")} 
{CommonRepository.BuildJoinYYYYPAR("LEFT", "KHEOP", "RGUST", "SITUATION", " AND SITUATION.TCOD = KHWSIT")} 
{CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PKSIT", "ETATQUITT", " AND ETATQUITT.TCOD = PKSIT")} 
WHERE ( KHWIPB , KHWALX , KHWTYP , KHWAVN ) = ( :ipb , :alx , :typ , :avn ) ;";
        internal static readonly string CountSimilarRisques = @"
SELECT COUNT(*) NBLIGNES FROM ( 
    SELECT DISTINCT JEPBT, JEPBS, JEPBA, JEPBP, JEPBR, KABBRNT, KABBRNC
    FROM YPRTRSQ
    INNER JOIN KPSELW ON JEIPB = KHVIPB AND JEALX = KHVALX
    AND JERSQ = KHVRSQ
    AND JEIPB = :codeOffre
    AND JEALX = :version
    AND KHVID = :lotId
    INNER JOIN KPRSQ ON KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ = JERSQ
) A ;";
        
        internal static readonly string SelectGarantieDto = @"
SELECT DISTINCT KABRSQ CODERSQ , KABDESC LIBRSQ ,JERGT CODTAXEREGIME,IFNULL(YPAREGIME.TPLIL, '') TAXEREGIME, KDAFOR CODEFOR, KDADESC LIBFOR ,KDAALPHA LETTREFOR,
    GADES LIBGAR, GAGAR CODEGAR, SELEGAR.KHVKDEID IDGAR, KHWDEBP DATEDEBGAR, KHWFINP DATEFINGAR,JERUT CODETYPEREGULE, IFNULL(YPAREGUL.TPLIL, '') LIBTYPEREGULE, 
    IFNULL(G.KDETAXCOD, HG.KDETAXCOD) CODTAXEGAR
FROM KPSELW SELEGAR
INNER JOIN KPRSQ ON KABIPB = SELEGAR.KHVIPB AND KABALX = SELEGAR.KHVALX AND KABTYP = SELEGAR.KHVTYP AND KABRSQ = SELEGAR.KHVRSQ
INNER JOIN YPRTRSQ ON JEIPB = SELEGAR.KHVIPB AND JEALX = SELEGAR.KHVALX  AND JERSQ = SELEGAR.KHVRSQ   
LEFT JOIN YYYYPAR YPAREGIME ON YPAREGIME.TCON = 'GENER' AND YPAREGIME.TFAM = 'TAXRG'  AND YPAREGIME.TCOD = JERGT
INNER JOIN KPFOR ON KDAIPB = SELEGAR.KHVIPB AND KDAALX = SELEGAR.KHVALX AND KDATYP = SELEGAR.KHVTYP AND KDAFOR = SELEGAR.KHVFOR
LEFT JOIN KPGARAN G ON G.KDEID = SELEGAR.KHVKDEID 
LEFT JOIN HPGARAN HG ON HG.KDEID = SELEGAR.KHVKDEID 
INNER JOIN KGARAN ON IFNULL(G.KDEGARAN, HG.KDEGARAN) = GAGAR 
INNER JOIN KPRGU ON KHWIPB = :codeContrat AND KHWALX = :version AND KHWTYP = :type 
LEFT JOIN YYYYPAR YPAREGUL ON YPAREGUL.TCON = 'PRODU' AND YPAREGUL.TFAM = 'JERUT' AND YPAREGUL.TCOD = JERUT 
WHERE KHVIPB = :codeContrat AND KHVALX = :version  AND  KHVTYP = :type  AND KHVID = :lotId  AND KHVKDEID = :codeGar AND KHWID = :rgId ;";
        internal static readonly string SelectWListMouvements = @"
SELECT KHYDEBP DATEDEB, KHYFINP DATEFIN, KHYBAS ASSIETTE, CASE WHEN KHYBAU = 'D' THEN KHYBAM ELSE KHYBAT END TAUX, KHYBAU UNITE 
FROM KPRGUWP 
WHERE KHYIPB = :codeContrat AND KHYALX = :version AND KHYTYP = :type AND KHYRSQ = :codeRsq AND KHYFOR = :codeFor AND KHYGARAN = :codeGar 
ORDER BY KHYDEBP ;";
        internal static readonly string SelectGarRCIds = $@"
SELECT KHVKDEID 
FROM KPSELW 
WHERE KHVPERI = '{PerimetreSelectionRegul.Garantie.AsCode()}' 
AND KHVID = :lotId AND KHVRSQ = :rsqNum 
AND KHVKDEGAR IN ('{AlbOpConstants.RCFrance}', '{AlbOpConstants.RCExport}', '{AlbOpConstants.RCUSA}') ;";
        internal static readonly string SelectPeriodBoundsRegul = "SELECT KHWDEBP , KHWFINP FROM KPRGU WHERE KHWIPB = :ipb AND KHWALX = :alx AND KHWTYP = :typ AND KHWID = :rgid ;";
        internal static readonly string SelectSELWPeriodBoundsRegul = "SELECT KHVDEB , KHVFIN FROM KPSELW WHERE KHVIPB = :ipb AND KHVALX = :alx AND KHVTYP = :typ AND KHVID = :lotid ;";
        internal static readonly string SelectSELWPeriodGarBoundsRegul = "SELECT KHVDEB , KHVFIN FROM KPSELW WHERE ( KHVIPB , KHVALX , KHVTYP , KHVID , KHVKDEID ) = (:ipb , :alx , :typ , :lotid , :idg ); ";
        internal static readonly string SelectWPeriodBoundsRegul = @"
SELECT MIN(KHYDEBP) DEBP , MAX(KHYFINP) FINP 
FROM KPRGUWP 
WHERE KHYIPB = :CODECONTRAT AND KHYALX = :VERSION AND KHYTYP = :TYPE 
AND KHYRSQ = :CODERSQ AND KHYFOR = :CODEFOR  AND KHYGARAN = :CODEGAR ;";
        internal static readonly string SelectCodeGarantieRegul = @"SELECT KHVKDEGAR FROM KPSELW WHERE KHVKDEID = :idgar ;";
        internal static readonly string SelectLignesMouvementsGaranties = @"
SELECT KHXID CODE , KHXSIT SITUATION , KHXDEBP DATEDEB , KHXFINP DATEFIN , 
CASE WHEN NULLIF(KHXCU2, '') IS NULL THEN KHXBAS ELSE KHXCA2 END ASSIETTE , 
CASE WHEN NULLIF(KHXCU2, '') IS NULL THEN CASE WHEN KHXBAU = 'D' THEN KHXBAM ELSE KHXBAT END ELSE CASE WHEN KHXCU2 = 'D' THEN KHXCP2 ELSE KHXCT2 END END TAUXFORFAITHTVALEUR , 
CASE WHEN NULLIF(KHXCU2, '') IS NULL THEN KHXBAU ELSE KHXCU2 END TAUXFORFAITHTUNITE , KHXMHT MONTANTREGULHF 
FROM KPRGUG 
WHERE KHXKHWID = :IDREGUL AND KHXKDEID = :IDGAR ;";
        internal static readonly string SelectWMatriceSimple = @"
SELECT KHVRSQ RISQUEID , KHVFOR FORMULE , KHVKDEID GARID , KHVKDEGAR GARLIB , IFNULL( KILSIT , '' ) SITRSQ 
FROM KPSELW 
LEFT JOIN KPRGUR ON KHVRSQ = KILRSQ AND KILKHWID = :regulId 
WHERE KHVID = :lotId AND KHVKDEID > 0;";
        internal static readonly string SelectMatriceSimple = "SELECT KHXRSQ RISQUEID , KHXFOR FORMULE , KHXKDEID GARID , KHXGARAN GARLIB , '' SITRSQ FROM KPRGUG WHERE KHXIPB = :codeAffaire AND KHXALX = :version  AND KHXTYP = :type ;";
        internal static readonly string SelectCurrentStates = @"
SELECT 'GUG' TYP, COUNT(*) NB,
(SELECT COUNT(*) FROM KPRGUG WHERE KHXSIT = 'V' AND KHXKHWID = :reguleId) NBV
FROM KPRGUG WHERE KHXKHWID = :reguleId
UNION ALL
SELECT 'GUR' TYP, COUNT(*) NB,
(SELECT COUNT(*) FROM KPRGUR WHERE KILSIT = 'V' AND KILKHWID = :reguleId) NBV
FROM KPRGUR WHERE KILKHWID = :reguleId
UNION ALL
SELECT 'GU' TYP, 1 NB, COUNT(*) NBV FROM KPRGU WHERE KHWSIT = 'V' AND KHWID = :reguleId";
        internal static readonly string SelectCourtiers = @"SELECT W2ICT , TNNOM 
FROM YDA300PF 
INNER JOIN YCOURTN ON W2ICT = TNICT AND TNINL = 0 AND TNTNM = 'A' and TNXN5 = 0 
WHERE W2IPB = :IPB AND W2ALX = :ALX ";

        public RegularisationRepository(IDbConnection connection) : base(connection) { }

        public void UpdateAvenantNumDate(Folder folder, DateTime? dateAvenant) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = UpdateDateAndNumeroAvenant
            }) {
                options.BuildParameters(dateAvenant.ToIntYMD(), folder.CodeOffre.ToIPB(), folder.Version, folder.Type, folder.NumeroAvenant);
                options.Exec();
            }
        }

        public LigneRegularisationDto GetByFolder(Folder folder, int? numAvn = null) {
            return Fetch<LigneRegularisationDto>(
                SelectByFolder,
                folder.CodeOffre.ToIPB(),
                folder.Version,
                folder.Type,
                numAvn.GetValueOrDefault(folder.NumeroAvenant))?.FirstOrDefault();
        }

        public long GetNbRegRisques(RegularisationMode regulMode, Folder folder, long? lotId) {
            var param = new List<EacParameter>();

            param.Add(new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = folder.CodeOffre.ToIPB() });
            param.Add(new EacParameter("version", DbType.Int32) { Value = folder.Version });
            param.Add(new EacParameter("type", DbType.AnsiStringFixedLength) { Value = folder.Type });
            param.Add(new EacParameter("idLot", DbType.Int64) { Value = lotId.GetValueOrDefault() });

            var sql = string.Empty;

            if (regulMode != RegularisationMode.PB && regulMode != RegularisationMode.BNS && regulMode != RegularisationMode.Burner) {
                sql = @"SELECT COUNT(*) FROM KPSELW WHERE KHVIPB = :codeOffre AND KHVALX = :version AND KHVTYP = :type AND KHVID = :idLot AND KHVPERI = 'RQ' ;";
            }
            else {
                string pbn = string.Empty;

                switch (regulMode) {
                    case RegularisationMode.Burner:
                        pbn = "U";
                        break;

                    case RegularisationMode.PB:
                        pbn = "O";
                        break;

                    case RegularisationMode.BNS:
                        pbn = "B";
                        break;
                }

                param.Add(new EacParameter("regulMode", DbType.AnsiStringFixedLength) { Value = pbn });

                sql = @"SELECT COUNT(*) FROM KPSELW INNER JOIN YPRTRSQ ON JERSQ = KHVRSQ AND  JEIPB = KHVIPB AND JEALX = KHVALX WHERE KHVIPB = :codeOffre AND KHVALX = :version AND KHVTYP = :type AND KHVID = :idLot AND JEPBN = :regulMode AND KHVPERI = 'RQ'";
            }

            using (var dbOptions = new DbCountOptions(this.connection == null) {
                SqlText = sql,
                DbConnection = this.connection,
                Parameters = param
            }) {
                dbOptions.PerformCount();
                return dbOptions.Count;
            }
        }

        public long GetNbRegGaranties(RegularisationMode regulMode, Folder folder, long? lotId) {
            var param = new List<EacParameter>();

            param.Add(new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = folder.CodeOffre.ToIPB() });
            param.Add(new EacParameter("version", DbType.Int32) { Value = folder.Version });
            param.Add(new EacParameter("type", DbType.AnsiStringFixedLength) { Value = folder.Type });
            param.Add(new EacParameter("idLot", DbType.Int64) { Value = lotId.GetValueOrDefault() });

            var sql = string.Empty;

            if (regulMode != RegularisationMode.PB && regulMode != RegularisationMode.BNS && regulMode != RegularisationMode.Burner) {
                sql = $@"
SELECT COUNT(*) FROM (
SELECT 1
FROM KPSELW 
WHERE KHVIPB = :codeOffre AND KHVALX = :version AND KHVTYP = :type AND KHVID = :idLot 
AND KHVPERI = '{PerimetreSelectionRegul.Garantie.AsCode()}'
AND KHVKDEGAR NOT IN ('{AlbOpConstants.RCFrance}', '{AlbOpConstants.RCExport}', '{AlbOpConstants.RCUSA}')
UNION ALL
SELECT DISTINCT 1 
FROM KPSELW 
WHERE KHVIPB = :codeOffre AND KHVALX = :version AND KHVTYP = :type AND KHVID = :idLot 
AND KHVPERI = '{PerimetreSelectionRegul.Garantie.AsCode()}' 
AND KHVKDEGAR IN ('{AlbOpConstants.RCFrance}', '{AlbOpConstants.RCExport}', '{AlbOpConstants.RCUSA}') 
) T";
            }
            else {
                string trg = string.Empty;

                if (regulMode == RegularisationMode.Burner)
                    trg = "BU";
                else if (regulMode == RegularisationMode.BNS)
                    trg = "BN";
                else
                    trg = "PB";

                param.Add(new EacParameter("regulMode", DbType.AnsiStringFixedLength) { Value = trg });

                sql = $@"
SELECT COUNT(*) 
FROM KGARTRG 
INNER JOIN KPSELW ON TRIM ( KIHGAR ) = TRIM ( KHVKDEGAR ) 
WHERE KHVIPB = :codeOffre AND KHVALX = :version AND KHVTYP = :type AND KHVID = :idLot AND KIHTRG = :regulMode AND KHVPERI = '{PerimetreSelectionRegul.Garantie.AsCode()}'";

            }

            using (var dbOptions = new DbCountOptions(this.connection == null) {
                SqlText = sql,
                DbConnection = this.connection,
                Parameters = param
            }) {
                dbOptions.PerformCount();
                return dbOptions.Count;
            }
        }

        public RegularisationScope GetCurrentScope(long rgId) {
            using (var options = new DbSelectStringsOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = "SELECT KHWACC FROM KPRGU WHERE KHWID = :rgId"
            }) {

                options.BuildParameters(rgId);
                options.PerformSelect();
                if (options.StringList.Any()) {
                    return options.StringList.First() == "E" ? RegularisationScope.Contrat : RegularisationScope.Risque;
                }
            }
            return 0;
        }

        /// <summary>
        /// Defines whether the regularisation may be mono-risque before creating it
        /// </summary>
        /// <param name="contrat"></param>
        /// <param name="rgId"></param>
        /// <returns></returns>
        public int GetDistinctRisques(Folder folder, long lotId) {
            using (var options = new DbCountOptions(this.connection == null) {
                SqlText = CountSimilarRisques,
                DbConnection = this.connection
            }) {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, lotId);
                options.PerformCount();
                return options.Count;
            }
        }

        public int GetNbRegPeriodesGaranties(RegularisationMode regulMode, long rgId) {
            using (var dbOptions = new DbSelectStringsOptions(this.connection == null) {
                SqlText = "SELECT KHXGARAN FROM KPRGUG WHERE KHXKHWID = :rgId",
                DbConnection = this.connection,
                CommandType = CommandType.Text
            }) {
                dbOptions.BuildParameters(rgId);
                dbOptions.PerformSelect();
                if (!dbOptions.StringList.Any()) {
                    return 0;
                }

                if (!dbOptions.StringList.Any(s => string.Compare(s, "RCFR", true) == 0)) {
                    return dbOptions.StringList.Count();
                }

                return dbOptions.StringList.Count(s => string.Compare(s, "RCUS", true) != 0 && string.Compare(s, "RCEX", true) != 0);
            }
        }

        public int SetListRsqRegule(string lotId, string reguleId, string codeContrat, string version, string type, string typeAvt,
            string codeAvn, string exercice, string dateDeb, string dateFin,
            string codeICT, string codeICC, string tauxCom, string tauxComCATNAT,
            string codeEnc, string user, string mode, string MotifAvt, string regulMode, string regulType, string regulNiveau, string regulAvn) {
            
            var outputParam = new EacParameter("P_NEWREGULEID", DbType.Int32) { Value = 0, Direction = ParameterDirection.InputOutput };

            var parameters = new List<DbParameter> {
                new EacParameter("P_CODECONTRAT", codeContrat.ToIPB()),
                new EacParameter("P_VERSION", DbType.Int32) { Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0 },
                new EacParameter("P_TYPE", type),
                new EacParameter("P_CODEAVN", DbType.Int32) { Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0 },
                new EacParameter("P_LOTID", DbType.Int64) { Value = !string.IsNullOrEmpty(lotId) ? Convert.ToInt64(lotId) : 0L },
                new EacParameter("P_REGULEID", DbType.Int64) { Value = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt64(reguleId) : 0L },
                new EacParameter("P_TYPEREGULE", typeAvt),
                new EacParameter("P_PERIODEDEB", DbType.Int32) { Value = !string.IsNullOrEmpty(dateDeb) ? AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(dateDeb)) : 0 },
                new EacParameter("P_PERIODEFIN", DbType.Int32) { Value = !string.IsNullOrEmpty(dateFin) ? AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(dateFin)) : 0 },
                new EacParameter("P_EXERCICE", DbType.Int32) { Value = !string.IsNullOrEmpty(exercice) ? Convert.ToInt32(exercice) : 0 },
                new EacParameter("P_CODEICT", DbType.Int32) { Value = !string.IsNullOrEmpty(codeICT) ? Convert.ToInt32(codeICT) : 0 },
                new EacParameter("P_CODEICC", DbType.Int32) { Value = !string.IsNullOrEmpty(codeICC) ? Convert.ToInt32(codeICC) : 0 },
                new EacParameter("P_TAUXCOM", DbType.Decimal) { Value = !string.IsNullOrEmpty(tauxCom) ? Convert.ToDecimal(tauxCom) : 0M },
                new EacParameter("P_TAUXCOMCATNAT", DbType.Decimal) { Value = !string.IsNullOrEmpty(tauxComCATNAT) ? Convert.ToDecimal(tauxComCATNAT) : 0M },
                new EacParameter("P_CODEENC", codeEnc),
                new EacParameter("P_USER", user),
                new EacParameter("P_DATENOW", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(DateTime.Now).Value },
                new EacParameter("P_HOURNOW", DbType.Int32) { Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(DateTime.Now)).Value },
                new EacParameter("P_MODE", mode),
                new EacParameter("P_MOTIF", !string.IsNullOrEmpty(MotifAvt) ? MotifAvt : string.Empty),
                new EacParameter("P_REGULMODE", regulMode),
                new EacParameter("P_REGULTYPE", regulType),
                new EacParameter("P_REGULNIV", regulNiveau),
                new EacParameter("P_REGULAVN", regulAvn),
                outputParam
            };
            using (var dbOptions = new DbSPOptions(this.connection == null) {
                SqlText = "SP_SETLISTRSQREGULE",
                DbConnection = this.connection,
                Parameters = parameters
            }) {
                dbOptions.ExecStoredProcedure();
            }
            return (mode == AccessMode.CREATE.ToString() && outputParam.Value != null) ? Convert.ToInt32(outputParam.Value.ToString()) : Convert.ToInt32(reguleId);
        }

        public long SetListRsqPBRegule(RegularisationContext context) {
            var paramList = new List<DbParameter>();
            var outParam = new EacParameter("P_NEWREGULEID", DbType.Int64) { Value = 0L, Direction = ParameterDirection.Output };

            paramList.Add(new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength) { Value = context.IdContrat.CodeOffre.PadLeft(9, ' ') });
            paramList.Add(new EacParameter("P_VERSION", DbType.Int32) { Value = context.IdContrat.Version });
            paramList.Add(new EacParameter("P_TYPE", DbType.AnsiStringFixedLength) { Value = context.IdContrat.Type });

            paramList.Add(new EacParameter("P_CODEAVN", DbType.Int64) { Value = context.ModeleAvtRegul.NumAvt });
            paramList.Add(new EacParameter("P_LOTID", DbType.Int64) { Value = context.LotId });
            paramList.Add(new EacParameter("P_REGULEID", DbType.AnsiStringFixedLength) { Value = context.RgId.ToString() });

            paramList.Add(new EacParameter("P_TYPEREGULE", DbType.AnsiStringFixedLength) { Value = context.Type });
            paramList.Add(new EacParameter("P_PERIODEDEB", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.DateDebut)) ? Convert.ToInt32(context.DateDebut) : 0 });
            paramList.Add(new EacParameter("P_PERIODEFIN", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.DateFin)) ? Convert.ToInt32(context.DateFin) : 0 });

            paramList.Add(new EacParameter("P_EXERCICE", DbType.Int32) { Value = context.Exercice });
            paramList.Add(new EacParameter("P_CODEICT", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.CodeICT)) ? Convert.ToInt32(context.CodeICT) : 0 });
            paramList.Add(new EacParameter("P_CODEICC", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.CodeICC)) ? Convert.ToInt32(context.CodeICC) : 0 });
            paramList.Add(new EacParameter("P_TAUXCOM", DbType.Double) { Value = (!string.IsNullOrEmpty(context.TauxCom)) ? Convert.ToDecimal(context.TauxCom) : 0.0m });
            paramList.Add(new EacParameter("P_TAUXCOMCATNAT", DbType.Double) { Value = (!string.IsNullOrEmpty(context.TauxComCATNAT)) ? Convert.ToDecimal(context.TauxComCATNAT) : 0.0m });
            paramList.Add(new EacParameter("P_CODEENC", DbType.AnsiStringFixedLength) { Value = !string.IsNullOrEmpty(context.CodeEnc) ? context.CodeEnc : string.Empty });

            paramList.Add(new EacParameter("P_USER", DbType.AnsiStringFixedLength) { Value = context.User });
            paramList.Add(new EacParameter("P_DATENOW", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(DateTime.Now).Value });
            paramList.Add(new EacParameter("P_HOURNOW", DbType.Int32) { Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(DateTime.Now)).Value });
            paramList.Add(new EacParameter("P_MODE", DbType.AnsiStringFixedLength) { Value = context.AccessMode.ToString() });
            paramList.Add(new EacParameter("P_MOTIF", DbType.AnsiStringFixedLength) { Value = !string.IsNullOrEmpty(context.ModeleAvtRegul.MotifAvt) ? context.ModeleAvtRegul.MotifAvt : string.Empty });

            paramList.Add(new EacParameter("P_REGULMODE", DbType.AnsiStringFixedLength) { Value = context.Mode.AsCode() });
            paramList.Add(new EacParameter("P_REGULTYPE", DbType.AnsiStringFixedLength) { Value = context.Type });
            paramList.Add(new EacParameter("P_REGULNIV", DbType.AnsiStringFixedLength) { Value = context.Scope.AsCode() });
            paramList.Add(new EacParameter("P_REGULAVN", DbType.AnsiStringFixedLength) { Value = context.RgHisto.ToString() });

            paramList.Add(outParam);

            using (var options = new DbSPOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = "SP_SETLISTRSQREGULE_PB",
                Parameters = paramList
            }) {
                options.ExecStoredProcedure();

                if (context.RgId == 0) {
                    context.RgId = Convert.ToInt64(outParam.Value);
                }
            }

            return context.RgId;
        }

        public int GetNbReportCharge(Folder folder, string situation) {
            using (var options = new DbCountOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = CountReportCharge
            }) {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type, situation);
                options.PerformCount();
                return options.Count;
            }
        }

        public void ModifyReportCharge(Folder folder, string situation, long regulId) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = UpdateRegulReportCharge
            }) {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type, situation, regulId);
                options.Exec();
            }
        }

        public long SetListRsqBNSRegule(RegularisationContext context) {
            var paramList = new List<DbParameter>();
            var outParam = new EacParameter("P_NEWREGULEID", DbType.Int64) { Value = 0L, Direction = ParameterDirection.Output };

            paramList.Add(new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength) { Value = context.IdContrat.CodeOffre.PadLeft(9, ' ') });
            paramList.Add(new EacParameter("P_VERSION", DbType.Int32) { Value = context.IdContrat.Version });
            paramList.Add(new EacParameter("P_TYPE", DbType.AnsiStringFixedLength) { Value = context.IdContrat.Type });

            paramList.Add(new EacParameter("P_CODEAVN", DbType.AnsiStringFixedLength) { Value = context.ModeleAvtRegul.NumAvt.ToString() });
            paramList.Add(new EacParameter("P_LOTID", DbType.Int64) { Value = context.LotId });
            paramList.Add(new EacParameter("P_REGULEID", DbType.AnsiStringFixedLength) { Value = context.RgId.ToString() });

            paramList.Add(new EacParameter("P_TYPEREGULE", DbType.AnsiStringFixedLength) { Value = context.Type });
            paramList.Add(new EacParameter("P_PERIODEDEB", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.DateDebut)) ? Convert.ToInt32(context.DateDebut) : 0 });
            paramList.Add(new EacParameter("P_PERIODEFIN", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.DateFin)) ? Convert.ToInt32(context.DateFin) : 0 });

            paramList.Add(new EacParameter("P_EXERCICE", DbType.Int32) { Value = context.Exercice });
            paramList.Add(new EacParameter("P_CODEICT", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.CodeICT)) ? Convert.ToInt32(context.CodeICT) : 0 });
            paramList.Add(new EacParameter("P_CODEICC", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.CodeICC)) ? Convert.ToInt32(context.CodeICC) : 0 });
            paramList.Add(new EacParameter("P_TAUXCOM", DbType.Double) { Value = (!string.IsNullOrEmpty(context.TauxCom)) ? Convert.ToDecimal(context.TauxCom) : 0.0m });
            paramList.Add(new EacParameter("P_TAUXCOMCATNAT", DbType.Double) { Value = (!string.IsNullOrEmpty(context.TauxComCATNAT)) ? Convert.ToDecimal(context.TauxComCATNAT) : 0.0m });
            paramList.Add(new EacParameter("P_CODEENC", DbType.AnsiStringFixedLength) { Value = !string.IsNullOrEmpty(context.CodeEnc) ? context.CodeEnc : string.Empty });

            paramList.Add(new EacParameter("P_USER", DbType.AnsiStringFixedLength) { Value = context.User });
            paramList.Add(new EacParameter("P_DATENOW", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(DateTime.Now).Value });
            paramList.Add(new EacParameter("P_HOURNOW", DbType.Int32) { Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(DateTime.Now)).Value });
            paramList.Add(new EacParameter("P_MODE", DbType.AnsiStringFixedLength) { Value = context.AccessMode.ToString() });
            paramList.Add(new EacParameter("P_MOTIF", DbType.AnsiStringFixedLength) { Value = !string.IsNullOrEmpty(context.ModeleAvtRegul.MotifAvt) ? context.ModeleAvtRegul.MotifAvt : string.Empty });

            paramList.Add(new EacParameter("P_REGULMODE", DbType.AnsiStringFixedLength) { Value = context.Mode.AsCode() });
            paramList.Add(new EacParameter("P_REGULTYPE", DbType.AnsiStringFixedLength) { Value = context.Type });
            paramList.Add(new EacParameter("P_REGULNIV", DbType.AnsiStringFixedLength) { Value = context.Scope.AsCode() });
            paramList.Add(new EacParameter("P_REGULAVN", DbType.AnsiStringFixedLength) { Value = context.RgHisto.ToString() });

            paramList.Add(outParam);

            using (var options = new DbSPOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = "SP_SETLISTRSQREGULE_BNS",
                Parameters = paramList
            }) {
                options.ExecStoredProcedure();

                if (context.RgId == 0) {
                    context.RgId = Convert.ToInt64(outParam.Value);
                }
            }

            return context.RgId;
        }

        public long SetListRsqBurnerRegule(RegularisationContext context) {
            var paramList = new List<DbParameter>();
            var outParam = new EacParameter("P_NEWREGULEID", DbType.Int64) { Value = 0L, Direction = ParameterDirection.Output };

            paramList.Add(new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength) { Value = context.IdContrat.CodeOffre.PadLeft(9, ' ') });
            paramList.Add(new EacParameter("P_VERSION", DbType.Int32) { Value = context.IdContrat.Version });
            paramList.Add(new EacParameter("P_TYPE", DbType.AnsiStringFixedLength) { Value = context.IdContrat.Type });

            paramList.Add(new EacParameter("P_CODEAVN", DbType.AnsiStringFixedLength) { Value = context.ModeleAvtRegul.NumAvt.ToString() });
            paramList.Add(new EacParameter("P_LOTID", DbType.Int64) { Value = context.LotId });
            paramList.Add(new EacParameter("P_REGULEID", DbType.AnsiStringFixedLength) { Value = context.RgId.ToString() });

            paramList.Add(new EacParameter("P_TYPEREGULE", DbType.AnsiStringFixedLength) { Value = context.Type });
            paramList.Add(new EacParameter("P_PERIODEDEB", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.DateDebut)) ? Convert.ToInt32(context.DateDebut) : 0 });
            paramList.Add(new EacParameter("P_PERIODEFIN", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.DateFin)) ? Convert.ToInt32(context.DateFin) : 0 });

            paramList.Add(new EacParameter("P_EXERCICE", DbType.Int32) { Value = context.Exercice });
            paramList.Add(new EacParameter("P_CODEICT", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.CodeICT)) ? Convert.ToInt32(context.CodeICT) : 0 });
            paramList.Add(new EacParameter("P_CODEICC", DbType.Int32) { Value = (!string.IsNullOrEmpty(context.CodeICC)) ? Convert.ToInt32(context.CodeICC) : 0 });
            paramList.Add(new EacParameter("P_TAUXCOM", DbType.Double) { Value = (!string.IsNullOrEmpty(context.TauxCom)) ? Convert.ToDecimal(context.TauxCom) : 0.0m });
            paramList.Add(new EacParameter("P_TAUXCOMCATNAT", DbType.Double) { Value = (!string.IsNullOrEmpty(context.TauxComCATNAT)) ? Convert.ToDecimal(context.TauxComCATNAT) : 0.0m });
            paramList.Add(new EacParameter("P_CODEENC", DbType.AnsiStringFixedLength) { Value = !string.IsNullOrEmpty(context.CodeEnc) ? context.CodeEnc : string.Empty });

            paramList.Add(new EacParameter("P_USER", DbType.AnsiStringFixedLength) { Value = context.User });
            paramList.Add(new EacParameter("P_DATENOW", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(DateTime.Now).Value });
            paramList.Add(new EacParameter("P_HOURNOW", DbType.Int32) { Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(DateTime.Now)).Value });
            paramList.Add(new EacParameter("P_MODE", DbType.AnsiStringFixedLength) { Value = context.AccessMode.ToString() });
            paramList.Add(new EacParameter("P_MOTIF", DbType.AnsiStringFixedLength) { Value = !string.IsNullOrEmpty(context.ModeleAvtRegul.MotifAvt) ? context.ModeleAvtRegul.MotifAvt : string.Empty });

            paramList.Add(new EacParameter("P_REGULMODE", DbType.AnsiStringFixedLength) { Value = context.Mode.AsCode() });
            paramList.Add(new EacParameter("P_REGULTYPE", DbType.AnsiStringFixedLength) { Value = context.Type });
            paramList.Add(new EacParameter("P_REGULNIV", DbType.AnsiStringFixedLength) { Value = context.Scope.AsCode() });
            paramList.Add(new EacParameter("P_REGULAVN", DbType.AnsiStringFixedLength) { Value = context.RgHisto.ToString() });

            paramList.Add(outParam);

            using (var options = new DbSPOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = "SP_SETLISTRSQREGULE_BURNER",
                Parameters = paramList
            }) {
                options.ExecStoredProcedure();

                if (context.RgId == 0) {
                    context.RgId = Convert.ToInt64(outParam.Value);
                }
            }

            return context.RgId;
        }

        public LigneRegularisationGarantieDto GetInfoGarantie(Folder folder, long lotId, long codeGar, long idregul) {
            return Fetch<LigneRegularisationGarantieDto>(SelectGarantieDto, folder.CodeOffre.ToIPB(), folder.Version, folder.Type, lotId, codeGar, idregul).FirstOrDefault();
        }

        /// <summary>
        /// Récupère la liste des mouvements sur la periode
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="codersq"></param>
        /// <param name="codfor"></param>
        /// <param name="codegar"></param>
        /// <returns></returns>
        public List<LigneMouvementDto> GetListMouvements(Folder folder, int codersq, int codfor, string codegar) {
            return Fetch<LigneMouvementDto>(SelectWListMouvements, folder.CodeOffre.ToIPB(), folder.Version, folder.Type, codersq, codfor, codegar).ToList();
        }

        public List<long> FindAllGarantiesRCIds(GarantieFilter filter) {
            return Fetch<long>(SelectGarRCIds, filter.LotId, filter.RsqNum).ToList();
        }

        public (long? khwdebp, long? khwfinp) GetPeriodBounds(Folder folder, long rgId) {
            var periods = Fetch<(long, long)>(SelectPeriodBoundsRegul, folder.CodeOffre.ToIPB(), folder.Version, folder.Type, rgId);
            if (periods.Any()) {
                return periods.First();
            }
            return (null, null);
        }

        public (long? khwdebp, long? khwfinp) GetTempPeriodBounds(Folder folder, long lotId) {
            var periods = Fetch<(long, long)>(SelectSELWPeriodBoundsRegul, folder.CodeOffre.ToIPB(), folder.Version, folder.Type, lotId);
            if (periods.Any()) {
                return periods.First();
            }
            return (null, null);
        }

        public (long? khwdebp, long? khwfinp) GetTempPeriodBoundsByGarantie(Folder folder, long lotId, int idg) {
            var periods = Fetch<(long, long)>(SelectSELWPeriodGarBoundsRegul, folder.CodeOffre.ToIPB(), folder.Version, folder.Type, lotId, idg);
            if (periods.Any()) {
                return periods.FirstOrDefault();
            }
            return (null, null);
        }

        public (long? minP, long? maxP) GetTempPeriodBounds(Folder folder, int codeRisque, int codeFormule, string codeGarantie) {
            var periods = Fetch<(long, long)>(SelectWPeriodBoundsRegul, folder.CodeOffre.ToIPB(), folder.Version, folder.Type, codeRisque, codeFormule, codeGarantie);
            if (periods.Any()) {
                return periods.First();
            }
            return (null, null);
        }

        public string GetCodeGarantieRegul(Folder folder, int idGarantie) {
            using (var options = new DbSelectStringsOptions(this.connection == null) {
                DbConnection = connection,
                SqlText = SelectCodeGarantieRegul
            }) {
                options.BuildParameters(idGarantie);
                options.PerformSelect();
                return options.StringList.FirstOrDefault() ?? string.Empty;
            }
        }

        /// <summary>
        /// Tries to create KPRGUG in RCFR context (meaning RCFR, RCEX and RCUS)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="codeFormule"></param>
        /// <returns></returns>
        public void EnsureMouvementGarantieRCPeriod(RegularisationContext context) {
            using (var options = new DbSPOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = "SP_ENSURELISTMOUVTGARRC",
                Parameters = new[] {
                    new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.IdContrat.CodeOffre.PadLeft(9, ' ') },
                    new EacParameter("P_VERSION", DbType.Int32) { Value = context.IdContrat.Version },
                    new EacParameter("P_TYPE", DbType.AnsiStringFixedLength, 1) { Value = context.IdContrat.Type },
                    new EacParameter("P_CODERSQ", DbType.Int32) { Value = (int)context.RsqId },
                    new EacParameter("P_CODEFOR", DbType.Int32) { Value = context.CodeFormule },
                    new EacParameter("P_IDGAR", DbType.Decimal) { Value = Convert.ToDecimal(context.GrId) },
                    new EacParameter("P_IDREGUL", DbType.Decimal) { Value = Convert.ToDecimal(context.RgId) },
                    new EacParameter("P_IDLOT", DbType.Decimal) { Value = Convert.ToDecimal(context.LotId) }
                }
            }) {
                options.ExecStoredProcedure();
            }
        }

        /// <summary>
        /// Gets the periods for Garanties RC group
        /// </summary>
        /// <param name="rgId">Regularisation identifier</param>
        /// <param name="grId">Meant to be the RCFR Garantie identifier</param>
        /// <returns></returns>
        public List<LigneMouvtGarantieDto> GetMouvementsGaranties(long rgId, long grId) {
            return Fetch<LigneMouvtGarantieDto>(SelectLignesMouvementsGaranties, rgId, grId).ToList();
        }

        /// <summary>
        /// Tries to create KPRGUG in RCFR context (meaning RCFR, RCEX and RCUS)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="codeFormule"></param>
        /// <returns></returns>
        public List<LigneMouvtGarantieDto> EnsureMouvementGarantiePeriod(RegularisationContext context) {
            using (var options = new DbSPOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = "SP_GETLISTMOUVTGAR",
                UseDapper = true,
                Parameters = new[] {
                    new EacParameter("P_CODEOFFRE", context.IdContrat.CodeOffre.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = context.IdContrat.Version },
                    new EacParameter("P_TYPE", context.IdContrat.Type),
                    new EacParameter("P_CODERSQ", DbType.Int32) { Value = (int)context.RsqId },
                    new EacParameter("P_CODEFOR", DbType.Int32) { Value = context.CodeFormule },
                    new EacParameter("P_IDGAR", DbType.Int32) { Value = Convert.ToDecimal(context.GrId) },
                    new EacParameter("P_IDREGUL", DbType.Int32) { Value = Convert.ToDecimal(context.RgId) },
                    new EacParameter("P_ISREADONLY", DbType.Int32) { Value = context.IsReadOnlyMode ? 1 : 0 }
                }
            }) {
                return options.SelectCursor<LigneMouvtGarantieDto>()?.ToList();
            }
        }

        public IEnumerable<RegulMatriceDto> GetTempMatrice(Folder folder, long lotId, long rgId) {
            return Fetch<RegulMatriceDto>(SelectWMatriceSimple, rgId, lotId);
        }

        public IEnumerable<RegulMatriceDto> GetMatrice(Folder folder) {
            return Fetch<RegulMatriceDto>(SelectMatriceSimple, folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
        }

        public IEnumerable<RegularisationStateDto> GetCurrentStates(long rgId) {
            using (var options = new DbSelectOptions(this.connection == null) {
                DbConnection = connection,
                SqlText = SelectCurrentStates
            }) {
                options.BuildParameters(rgId);
                return options.PerformSelect<RegularisationStateDto>();
            }
        }

        public IEnumerable<(int id, string nom)> GetCourtiersRegularisation(Folder folder) {
            return Fetch<(int, string)>(SelectCourtiers, folder.CodeOffre.ToIPB(), folder.Version);
        }
    }
}
