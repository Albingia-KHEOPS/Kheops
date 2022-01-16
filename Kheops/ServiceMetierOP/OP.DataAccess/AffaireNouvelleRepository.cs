using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Dapper;
using DataAccess.Helpers;
using OP.DataAccess.Data;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Contrats;
using OP.WSAS400.DTO.NavigationArbre;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Globalization;
using System.Linq;

namespace OP.DataAccess
{
    public class AffaireNouvelleRepository : RepositoryBase
    {
        internal static readonly string UpdateTaux = "UPDATE YPRTENT SET JDXCM = :jdxcm, JDCNC = :jdcnc WHERE JDIPB = :codeAffaire AND JDALX = :version ;";
        internal static readonly string SelectDateAvenant = @"SELECT 
CAST ( CAST ( IFNULL ( PBAVA * 10000 + PBAVM * 100 + PBAVJ , 0 ) * 1000000 AS VARCHAR(32) ) AS TIMESTAMP ) 
FROM YPOBASE 
WHERE PBIPB = :CODEOFFRE AND PBALX = :VERSION AND PBTYP = :TYPE AND PBAVN = :NUMAVT ; ";
        internal static readonly string SelectProchaineEcheance = "SELECT JDPEA , JDPEM , JDPEJ FROM YPRTENT WHERE JDIPB = :CODEOFFRE AND JDALX = :VERSION ;";
        internal static readonly string SelectAffNouvProchaineEcheance = @"SELECT PBPER PERIODICITE, 
RIGHT(REPEAT('0', 2) CONCAT PBEFJ, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 2) CONCAT PBEFM, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 4) CONCAT PBEFA, 4) EFFETGARANTIE,
RIGHT(REPEAT('0', 2) CONCAT PBFEJ, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 2) CONCAT PBFEM, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 4) CONCAT PBFEA, 4) FINGARANTIE,
RIGHT(REPEAT('0', 2) CONCAT PBECJ, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 2) CONCAT PBECM, 2) ECHPRINC,
PBCTD DUREE, PBCTU DUREEUNITE 
FROM YPOBASE WHERE PBIPB = :P_CODEOFFRE AND PBALX = :P_VERSION AND PBTYP = :P_TYPE";
        internal static readonly string SelectPeriodeBetween = @"SELECT KADPA , KADPM , KADPJ 
FROM YPRTPER 
WHERE KAIPB = :CODEOFFRE AND KAALX = :VERSION AND KARSQ = 0 AND KAFOR = 0 AND KATYP = 1 
AND :DT >= KADPA * 10000 + KADPM * 100 + KADPJ 
AND :DT <= KAFPA * 10000 + KAFPM * 100 + KAPFJ ;";
        internal static readonly string UpdateProchaineEcheanceCancel = "UPDATE YPRTENT SET JDPEA = 0 , JDPEM = 0 , JDPEJ = 0 WHERE JDIPB = :CODEOFFRE AND JDALX = :VERSION ;";
        internal static readonly string UpdateAffNouvProchaineEcheance = @"UPDATE YPRTENT SET 
JDPEJ = :P_PROECHDAY, JDPEM = :P_PROECHMONTH, JDPEA = :P_PROECHYEAR, JDDPJ = :P_PDEBDAY, JDDPM = :P_PDEBMONTH, 
JDDPA = :P_PDEBYEAR, JDFPJ = :P_PFINDAY, JDFPM = :P_PFINMONTH, JDFPA = :P_PFINYEAR
WHERE JDIPB = :P_CODEOFFRE AND JDALX = :P_VERSION ;";
        internal static readonly string UpdateExcerciceCloturer = @"UPDATE YPRTENT 
SET JDDPA = :PERYEAR , JDDPM = :PERMONTH , JDDPJ = :PERDAY , 
    JDFPA = :YEARRSL , JDFPM = :MONTHRSL , JDFPJ = :DAYRSL 
WHERE JDIPB = :CODEOFFRE AND JDALX = :VERSION ;";
        internal static readonly string SelectCiblage = @"SELECT PBBRA , PBSBR , PBCAT , KAACIBLE 
FROM YPOBASE INNER JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX 
WHERE PBIPB = :CODEOFFRE AND PBALX = :VERSION AND PBTYP = :TYPE ;";
        internal static readonly string SelectDatesEffets = @"SELECT PBIPB IPB , PBALX ALX , PBTYP TYP , PBAVN AVN , 
    CAST ( LPAD ( PBEFA , 4 , '0' ) || LPAD ( PBEFM , 2 , '0' ) || LPAD ( PBEFJ , 2 , '0' ) || RPAD ( LPAD(PBEFH,4,'0') , 6 , '0' ) AS TIMESTAMP ) AS DEBUT , 
	CAST ( LPAD ( PBFEA , 4 , '0' ) || LPAD ( PBFEM , 2 , '0' ) || LPAD ( PBFEJ , 2 , '0' ) || RPAD ( LPAD(PBFEH,4,'0') , 6 , '0' ) AS TIMESTAMP ) AS FIN 
FROM YPOBASE 
WHERE PBIPB IN :IPB ;";

        internal static readonly string SimpleSelectFolder = @"SELECT PBIPB IPB , PBALX ALX , PBTYP TYP , PBAVN AVN , 
    PBREF REF , PBBRA BRA , IFNULL ( KAACIBLE , '' ) CIBLE , PBSTF STF , 
    PBEFA EFA , PBEFA EFM , PBEFA EFJ , PBEFH EFH , 
    PBAVA AVNEFA , PBAVM AVNEFM , PBAVJ AVNEFJ , KAAAVH AVNEFH , 
    JDPEA PEA , JDPEM PEM , JDPEJ PEJ , 
    PBOFF BASEOFFRE , PBTTR TTR , PBSIT SIT , PBETA ETA , ACLUIN UIN , TCBUR BUR , BUDBU , ANNOM ASNOM , ANIAS ASID 
FROM YPOBASE 
INNER JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX 
INNER JOIN YPRTENT ON ( PBIPB , PBALX ) = ( JDIPB , JDALX )
LEFT JOIN YCOURTI ON PBICT = TCICT 
LEFT JOIN YASSNOM ON PBIAS = ANIAS AND ANINL = 0 AND ANTNM = 'A' 
LEFT JOIN YBUREAU ON BUIBU = TCBUR 
LEFT JOIN YSECTEU ON ABHSEC = TCSEC 
LEFT JOIN YINSPEC ON ACLINS = ABHINS 
WHERE PBIPB = :IPB AND PBALX = :ALX AND PBTYP = :TYP ";

        internal static readonly string CountByNPL = @"SELECT COUNT(*) FROM YPOBASE WHERE PBIPB = :IPB AND PBALX = :ALX AND PBNPL IN :CODES ;";
        internal static readonly string SelectNouvelleAffaire = "SELECT KFHPOG IPB, KFHALG ALX, KFHIPB OIPB, KFHALX OALX, KFHTYPO TYPO , KHFSIT SIT FROM KPOFENT WHERE ( KFHPOG , KFHALG ) = ( :POG , :ALG ) ;";
        internal static readonly string CountOffreSelections = @"SELECT COUNT(*) FROM KPOFRSQ WHERE ( KFIPOG , KFIALG , KFIIPB , KFIALX ) = ( :POG , :ALG , :IPB , :ALX )";
        internal static readonly string SelectOffreSelections = $@"SELECT JEIPB IPB , JEALX ALX , JEVAL VAL , JEVAU UNIT, JEVAT TYPEVAL, KABDESC DESC, 
SEL.KFIRSQ RSQ , SEL.KFISEL SEL, SEL.KFIOBJ OBJ, SEL.KFIPOG POG, SEL.KFIALG ALG ,
{OutilsHelper.MakeCastTimestamp("JEVD", true)} AS DEB ,
{OutilsHelper.MakeCastTimestamp("JEVF", true)} AS FIN 
FROM YPRTRSQ
INNER JOIN KPRSQ ON ( JEIPB , JEALX ) = ( :IPB , :ALX ) AND ( JEIPB , JEALX , JERSQ ) = ( KABIPB , KABALX , KABRSQ ) 
INNER JOIN (
	SELECT KFIIPB, KFIALX, KFIRSQ , KFISEL , KFIOBJ, KFITYE, KFIPOG, KFIALG FROM KPOFRSQ WHERE KFIOBJ = 0 AND ( KFIPOG , KFIALG ) = ( :POG , :ALG ) 
) SEL ON ( JEIPB , JEALX , JERSQ ) = ( SEL.KFIIPB , SEL.KFIALX , SEL.KFIRSQ ) 
UNION ALL 
SELECT JGIPB , JGALX , JGVAL, JGVAU, JGVAT, KACDESC, SEL.KFIRSQ , SEL.KFISEL , SEL.KFIOBJ, SEL.KFIPOG, SEL.KFIALG ,
{OutilsHelper.MakeCastTimestamp("JGVD", true)} ,
{OutilsHelper.MakeCastTimestamp("JGVF", true)} 
FROM YPRTOBJ
INNER JOIN KPOBJ ON ( JGIPB , JGALX ) = ( :IPB , :ALX ) AND ( JGIPB , JGALX , JGRSQ , JGOBJ ) = ( KACIPB , KACALX , KACRSQ , KACOBJ )
INNER JOIN (
	SELECT KFIIPB, KFIALX, KFIRSQ , KFISEL , KFIOBJ, KFITYE, KFIPOG, KFIALG FROM KPOFRSQ WHERE KFIOBJ > 0 AND ( KFIPOG , KFIALG ) = ( :POG , :ALG ) 
) SEL ON ( JGIPB , JGALX , JGRSQ , JGOBJ ) = ( SEL.KFIIPB , SEL.KFIALX , SEL.KFIRSQ , SEL.KFIOBJ ) ;";

        internal static readonly string SelectApplicationsFormulesOptions = @"SELECT KABRSQ RSQ , 0 OBJ , KABDESC LIBRSQ , '' LIBOBJ, KDAID IDFOR , KDAFOR ""FOR"" , KDADESC LIBFOR , KDBID IDOPT , KDBOPT OPT , KABIPB IPB , KABALX ALX , KABTYP TYP
FROM KPRSQ
INNER JOIN KPOPTAP ON KABIPB = KDDIPB AND KABALX = KDDALX AND KABRSQ = KDDRSQ AND KDDPERI = 'RQ' 
AND ( :IPB , :ALX , :TYP ) = ( KABIPB , KABALX , KABTYP )
INNER JOIN KPFOR ON KDDIPB = KDAIPB AND KDDALX = KDAALX AND KDDFOR = KDAFOR
INNER JOIN KPOPT ON KDDIPB = KDBIPB AND KDDALX = KDBALX AND KDDFOR = KDBFOR AND KDDOPT = KDBOPT AND KDAFOR = KDBFOR
UNION ALL
SELECT KACRSQ RSQ , KACOBJ OBJ, '' LIBRSQ , KACDESC LIBOBJ, KDAID IDFOR , KDAFOR , KDADESC LIBFOR, KDBID IDOPT , KDBOPT OPT, KACIPB IPB , KACALX ALX, KACTYP TYP
FROM KPOBJ
INNER JOIN KPOPTAP ON KACIPB = KDDIPB AND KACALX = KDDALX AND KACRSQ = KDDRSQ AND KACOBJ = KDDOBJ AND KDDPERI = 'OB'
AND ( :IPB , :ALX , :TYP ) = ( KACIPB , KACALX , KACTYP )
INNER JOIN KPFOR ON KDDIPB = KDAIPB AND KDDALX = KDAALX AND KDDFOR = KDAFOR
INNER JOIN KPOPT ON KDDIPB = KDBIPB AND KDDALX = KDBALX AND KDDFOR = KDBFOR AND KDDOPT = KDBOPT AND KDAFOR = KDBFOR ;";
        internal static readonly string InsertKpofOpt = @"
INSERT INTO KPOFOPT 
( KFJPOG , KFJALG , KFJIPB , KFJALX , KFJCHR , KFJTENG , KFJFOR , KFJOPT , KFJKDAID , KFJKDBID , KFJKAKID , KFJSEL ) 
VALUES 
( :POG , :ALG , :IPB , :ALX , :ID , :TENG , :FOR , :OPT , :IDF , :IDO , 0 , 'N' ) ; ";
        internal static readonly string UpdateSingleOptionForContrat = "UPDATE {0} SET {1}OPT = 1 WHERE {1}IPB = :IPB AND {1}ALX = :ALX AND {1}TYP = 'P' and {1}OPT > 0;";

        internal static readonly IDictionary<string, string> TablesWithOPT = new Dictionary<string, string> {
            { "KPAVTRC", "KHO" },
            { "KPCLAUSE", "KCA" },
            { "KPCOTGA", "KDN" },
            { "KPCTRL", "KEU" },
            { "KPCTRLE", "KEV" },
            { "KPGARAH", "KDE" },
            { "KPGARAN", "KDE" },
            { "KPGARAP", "KDF" },
            { "KPGARTAR", "KDG" },
            { "KPHAVH", "KIG" },
            { "KPIOPT", "KFC" },
            { "KPIRSGA", "KFD" },
            { "KPIRSGR", "KHC" },
            { "KPOPT", "KDB" },
            { "KPOPTAP", "KDD" },
            { "KPOPTD", "KDC" },
            { "KPTRACE", "KCC" },
            { "KPVALH", "KIF" }
        };

        internal static readonly string SelectInfoBase = @"SELECT 
    PBEFA DATEEFFETA, PBEFM DATEEFFETM, PBEFJ DATEEFFETJ, PBEFH DATEEFFETJH,
    PBFEA FINEFFETANNEE, PBFEM FINEFFETMOIS, PBFEJ FINEFFETJOUR, PBFEH FINEFFETHEURE,
    PBPER PERIODICITECODE, PERD.TPLIB PERIODICITENOM, JDPEA PROCHECHA, JDPEM PROCHECHM, JDPEJ PROCHECHJ
FROM YPOBASE
    LEFT JOIN YPRTENT ON JDIPB = PBIPB AND JDALX = PBALX
WHERE PBIPB = :CODEOFFRE AND PBALX = :VERSION AND PBTYP = :TYPE ;";

        internal static readonly string UpdInfoBaseContract = @"UPDATE YPOBASE
SET PBECM = :ECM, PBECJ = :ECJ, PBEFA = :EFA, PBEFM = :EFM, PBEFJ = :EFJ, PBEFH = 0,
PBFEA = :FEA, PBFEM = :FEM, PBFEJ = :FEJ, PBFEH = :FEH,
PBTIL = 'T', PBANT = 'X', PBRGT = '1', PBPCV = 100,
PBAVA = :AVA, PBAVM = :AVM, PBAVJ = :AVJ, PBPER = :PER
WHERE PBIPB = :IPB AND PBALX = :ALX AND PBTYP = :TYP";
        internal static readonly string UpdEnteteContract = @"UPDATE YPRTENT 
SET JDDPV = :DPV , JDIND= :IND, JDIVA= :IVA, JDITC= :ITC,
JDCNA=:CNA , JDDPJ=:DPJ , JDDPM=:DPM , JDDPA=:DPA,
JDFPJ=:FPJ, JDFPM=:FPM, JDFPA=:FPA,
JDPEJ=:PEJ, JDPEM=:PEM, JDPEA=:PEA,
JDINA=:INA, JDIVO=:IVO, JDIVW=:IVW, JDLTA=:LTA 
WHERE  JDIPB=:IPB AND JDALX=:ALX";

        internal static readonly string UpdKpentContract = @"UPDATE KPENT
SET KAADSTA = :STA
WHERE KAAIPB = :IPB AND KAAALX = :ALX AND KAATYP = :TYP";

        internal static readonly string UpdObjContract = @"UPDATE YPRTOBJ
SET JGIVO = :IVO, JGIVA = :IVA, JGIVW = :IVW
WHERE JGIPB = :IPB AND JGALX = :ALX";

        internal static readonly string UpdLCIComplexe = @"UPDATE KPEXPLCID 
SET KDJLCVAL = :VAL, KDJLOVAL = :VAL 
WHERE KDJKDIID IN (SELECT KDIID FROM KPEXPLCI WHERE KDIIPB = :IPB AND KDIALX = :ALX AND KDITYP = :TYP AND KDILCE = :LCI)";

        internal static readonly string SaveActeGestion = @"INSERT INTO YPOTRAC(PYTYP, PYIPB, PYALX, PYAVN, PYTTR, PYVAG, PYTRA, PYTRM, PYTRJ, PYTRH, PYLIB, PYINF,
PYSDA, PYSDM, PYSDJ, PYSFA, PYSFM, PYSFJ, PYMJU, PYMJA, PYMJM, PYMJJ, PYMJH)
VALUES(:TYP,:IPB,:ALX,:AVN,:TTR,:VAG,:TRA,:TRM,:TRJ,:TRH,:LIB,:INF,
:SDA, :SDM, :SDJ, :SFA, :SFM, :SFJ, :MJU, :MJA, :MJM, :MJJ, :MJH)";

        internal static readonly string SelectStatutKheops = @"SELECT PBORK FROM YPOBASE WHERE ( PBIPB , PBALX , PBTYP ) = ( :IPB , :ALX , :TYP )";

        internal static readonly string InsertLock = @"INSERT INTO KVERROU 
( KAVID , KAVSERV , KAVTYP , KAVIPB , KAVALX , KAVAVN , KAVSUA , KAVNUM , KAVSBR , KAVACTG , KAVACT , KAVVERR , KAVCRU , KAVCRD , KAVCRH , KAVLIB ) 
VALUES ( :KAVID , :KAVSERV , :KAVTYP , :KAVIPB , :KAVALX , :KAVAVN , :KAVSUA , :KAVNUM , :KAVSBR , :KAVACTG , :KAVACT , :KAVVERR , :KAVCRU , :KAVCRD , :KAVCRH , :KAVLIB ) ;";
        internal static readonly string DeleteLock = "DELETE FROM KVERROU WHERE ( KAVIPB , KAVALX , KAVTYP , KAVCRU ) = ( :IPB , :ALX , :TYP , :USER ) ;";

        internal static readonly string DeletePripesContract = @"DELETE FROM YPRIPES WHERE PKIPB = :IPB AND PKALX = :ALX WITH NC";
        internal static readonly string DeletePripesGAContract = @"DELETE FROM YPRIPGA WHERE PLIPB = :IPB AND PLALX = :ALX WITH NC";
        internal static readonly string DeletePripesCMContract = @"DELETE FROM YPRIPCM WHERE PNIPB = :IPB AND PNALX = :ALX WITH NC";
        internal static readonly string DeletePripesGKContract = @"DELETE FROM YPRIPGK WHERE KVIPB = :IPB AND KVALX = :ALX WITH NC";
        internal static readonly string DeletePripesPAContract = @"DELETE FROM YPRIPPA WHERE POIPB = :IPB AND POALX = :ALX WITH NC";
        internal static readonly string DeletePripesTAContract = @"DELETE FROM YPRIPTA WHERE PTIPB = :IPB AND PTALX = :ALX WITH NC";
        internal static readonly string DeletePripesTGContract = @"DELETE FROM YPRIPTG WHERE PUIPB = :IPB AND PUALX = :ALX WITH NC";
        internal static readonly string DeletePripesTXContract = @"DELETE FROM YPRIPTX WHERE PMIPB = :IPB AND PMALX = :ALX WITH NC";

        internal static readonly string GetLotIdDocument = @"select kelid from kpdoclw where (KELIPB, kelalx, keltyp) = (:ipb, :alx, :typ)";
        internal static readonly string GetUrlTypologie = "SELECT '//' CONCAT TRIM(KHMSRV) CONCAT '/' CONCAT TRIM(KHMRAC) CONCAT '/' CONCAT TRIM(KHMENV) CONCAT TRIM(KHMCHM) CONCAT '/' STRRETURNCOL FROM KCHEMIN WHERE KHMCLE = :typologie";

        internal static readonly string SelectMontantCalcule = @"SELECT PKKHT FROM YPRIPES WHERE (PKIPB, PKALX, PKAVN) = (:IPB, :ALX, :AVN)";
        internal static readonly string SelectAttentat = @"SELECT JDATT FROM YPRTENT WHERE (JDIPB, JDALX) = (:IPB, :ALX)";

        public AffaireNouvelleRepository(IDbConnection connection) : base(connection) { }

        #region Ecran Creation Affaire Nouvelle

        #region Méthodes Publiques

        /// <summary>
        /// Affiche les informations de l'offre pour la préparation à 
        /// la création d'un nouveau contrat
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static CreationAffaireNouvelleDto InitCreateAffaireNouvelle(string codeOffre, string version, string type)
        {
            CreationAffaireNouvelleDto result = new CreationAffaireNouvelleDto();

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.ToIPB();
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sql = string.Format(@"SELECT PBIPB CODEOFFRE, PBALX VERSION, PBSAJ CONCAT '/' CONCAT PBSAM CONCAT '/' CONCAT PBSAA DATESAISIE, PBBRA CODEBRANCHE, PARBRANCHE.TPLIB LIBBRANCHE, 
                                KAACIBLE CODECIBLE, KAHDESC LIBCIBLE, PBDEV CODEDEVISE, PARDEVISE.TPLIB LIBDEVISE, PBREF IDENTIFICATION, PBICT CODECOURTIER, 
                                TNNOM NOMCOURTIER, PBIAS CODEASSURE, ANNOM NOMASSURE, PBNPL CODENATCON, PARNATCON.TPLIB LIBNATCON, 
                                TRIM(SOUS.UTNOM) CONCAT ' ' CONCAT TRIM(SOUS.UTPNM) SOUSCRIPTEUR, TRIM(GEST.UTNOM) CONCAT ' ' CONCAT TRIM(GEST.UTPNM) GESTIONNAIRE,
                                TRIM(OBSV.KAJOBSV) OBSERVATION
                        FROM YPOBASE
                            INNER JOIN YCOURTN ON TNICT = PBICT AND TNXN5 = 0 AND TNTNM = 'A'
                            LEFT JOIN YASSNOM ON ANIAS = PBIAS AND ANINL = 0 AND ANTNM = 'A'
                            INNER JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX
                            INNER JOIN KCIBLE ON KAHCIBLE = KAACIBLE
                            {0}
                            {1}
                            {2}
                            LEFT JOIN YUTILIS SOUS ON SOUS.UTIUT = PBSOU
                            LEFT JOIN YUTILIS GEST ON GEST.UTIUT = PBGES
                            LEFT JOIN KPOBSV OBSV ON OBSV.KAJIPB = PBIPB AND OBSV.KAJALX = PBALX AND OBSV.KAJTYP = PBTYP
                        WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type",
                        CommonRepository.BuildJoinYYYYPAR("INNER", "GENER", "BRCHE", "PARBRANCHE", " AND PARBRANCHE.TCOD = PBBRA AND PARBRANCHE.TPCN2 = 1"),
                        CommonRepository.BuildJoinYYYYPAR("INNER", "GENER", "DEVIS", "PARDEVISE", " AND PARDEVISE.TCOD = PBDEV"),
                        CommonRepository.BuildJoinYYYYPAR("INNER", "PRODU", "PBNPL", "PARNATCON", " AND PARNATCON.TCOD = PBNPL"));

            result = DbBase.Settings.ExecuteList<CreationAffaireNouvelleDto>(CommandType.Text, sql, param).FirstOrDefault();

            var lstContrats = LoadListContrat(codeOffre, version);
            if (lstContrats != null)
            {
                lstContrats.ForEach(c =>
                {
                    if (!string.IsNullOrEmpty(c.DateAccordDB))
                        c.DateAccord = Convert.ToDateTime(c.DateAccordDB);
                    if (!string.IsNullOrEmpty(c.DateEffetDB))
                        c.DateEffet = Convert.ToDateTime(c.DateEffetDB);
                });
            }
            if (result != null)
                result.Contrats = lstContrats;

            return result;
        }

        /// <summary>
        /// Charge les informations des listes déroulantes
        /// pour la div flottante de création du contrat
        /// </summary>
        /// <returns></returns>
        public static CreationAffaireNouvelleContratDto InitAffaireNouvelleContrat(string codeOffre, string version, string type, string codeAvn, string user, ModeConsultation modeNavig)
        {
            BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);
            CreationAffaireNouvelleContratDto model = GetInfoOffre(codeOffre, version, type);
            model.Branches = GetListBranches(codeOffre, version, type);// CommonRepository.GetParametres("GENER", "BRCHE");
            model.TypesContrat = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "KHEOP", "TYPOC");
            model.TypeContrat = "S";
            model.DateAccord = DateTime.Now;

            var gestionnaire = GestionnaireRepository.ObtenirGestionnaire(user);
            if (gestionnaire != null)
            {
                model.Gestionnaire = string.Format("{0} - {1}", gestionnaire.Id, gestionnaire.Nom);
                model.GestionnaireCode = gestionnaire.Id;
            }

            return model;
        }

        public static ContratDto GetInfoBaseAvenant(string codeOffre, string version, string type)
        {
            ContratDto result = new ContratDto();

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.ToIPB();
            param[1] = new EacParameter("VERSION", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sql = @"SELECT 
                PBEFA DATEEFFETA, PBEFM DATEEFFETM, PBEFJ DATEEFFETJ, PBEFH DATEEFFETJH,
                PBFEA FINEFFETANNEE, PBFEM FINEFFETMOIS, PBFEJ FINEFFETJOUR, PBFEH FINEFFETHEURE, PBECJ ECHJOUR, PBECM ECHMOIS,
                TNNOM NOMCOURTIERGEST,
                PBSOU SOUSCODE, SOU.UTNOM SOUSNOM, PBGES GESCODE, GES.UTNOM GESNOM,
                PBTAC TYPERETOUR, SIGNE.TPLIB LIBRETOUR, PBPER PERIODICITECODE, PERD.TPLIB PERIODICITENOM, JDPEA PROCHECHA, JDPEM PROCHECHM, JDPEJ PROCHECHJ,
                IFNULL(KHWID, 0) REGULEID
            FROM YPOBASE
                LEFT JOIN YCOURTN ON TNICT = PBICT AND TNXN5 = 0 AND TNTNM = 'A'
                LEFT JOIN YYYYPAR SIGNE ON SIGNE.TCOD = PBTAC AND SIGNE.TFAM = 'PBTAC' AND SIGNE.TCON = 'PRODU'
                LEFT JOIN YYYYPAR PERD ON PERD.TCOD = PBPER AND PERD.TFAM = 'PBPER' AND PERD.TCON = 'PRODU'
                LEFT JOIN YUTILIS SOU ON PBSOU = SOU.UTIUT
                LEFT JOIN YUTILIS GES ON PBGES = GES.UTIUT
                LEFT JOIN YPRTENT ON JDIPB = PBIPB AND JDALX = PBALX
                LEFT JOIN KPRGU ON KHWIPB = PBIPB AND KHWALX = PBALX AND KHWTYP = PBTYP AND KHWAVN = PBAVN
            WHERE PBIPB = :CODEOFFRE AND PBALX = :VERSION AND PBTYP = :TYPE";

            result = DbBase.Settings.ExecuteList<ContratDto>(CommandType.Text, sql, param).FirstOrDefault();

            return result;
        }

        public static ContratInfoBaseDto InitContratInfoBase(string id, string version, string type, string codeAvn, string user, ModeConsultation modeNavig)
        {
            ContratInfoBaseDto model = new ContratInfoBaseDto();
            BrancheDto branche = CommonRepository.GetBrancheCible(id, version, type, codeAvn, modeNavig);

            model.TypesContrat = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "KHEOP", "TYPOC");
            //model.TypeContrat = "S";

            List<string> listBranche = new List<string> { "PP", "ZZ" };
            model.Branches = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "GENER", "BRCHE", tCod: listBranche, notIn: true, tPcn2: "1");


            model.Encaissements = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "GENER", "TCYEN");
            if (id != null && version != null)
            {
                #region Récupération des données d'un contrat existant
                ContratDto contratInfoDto = GetContrat(id, version, type, "0", modeNavig);
                if (contratInfoDto == null) return null;
                model.ContratRemplace = contratInfoDto.ContratRemplace == "O" ? true : false;
                model.NumContratRemplace = contratInfoDto.CodeContratRemplace;
                model.NumAlimentRemplace = contratInfoDto.ContratRemplaceAliment.ToString();
                model.CodeContrat = contratInfoDto.CodeContrat;
                model.VersionContrat = contratInfoDto.VersionContrat;
                model.Branche = contratInfoDto.Branche;
                model.Encaissement = contratInfoDto.CodeEncaissement;
                model.Cible = contratInfoDto.Cible;
                model.TypePolice = contratInfoDto.TypePolice;
                model.DateAccordAnnee = contratInfoDto.DateAccordAnnee;
                model.DateAccordMois = contratInfoDto.DateAccordMois;
                model.DateAccordJour = contratInfoDto.DateAccordJour;
                model.SouscripteurCode = contratInfoDto.SouscripteurCode;
                model.SouscripteurNom = contratInfoDto.SouscripteurNom;
                model.GestionnaireCode = contratInfoDto.GestionnaireCode;
                model.GestionnaireNom = contratInfoDto.GestionnaireNom;
                model.CourtierApporteur = contratInfoDto.CourtierApporteur;
                model.NomCourtierAppo = contratInfoDto.NomCourtierAppo;
                model.CourtierGestionnaire = contratInfoDto.CourtierGestionnaire;
                model.NomCourtierGest = contratInfoDto.NomCourtierGest;
                model.CourtierPayeur = contratInfoDto.CourtierPayeur;
                model.NomCourtierPayeur = contratInfoDto.NomCourtierPayeur;
                model.CodeInterlocuteur = contratInfoDto.CodeInterlocuteur;
                model.NomInterlocuteur = contratInfoDto.NomInterlocuteur;
                model.RefCourtier = contratInfoDto.RefCourtier;
                model.CodePreneurAssurance = contratInfoDto.CodePreneurAssurance;
                model.NomPreneurAssurance = contratInfoDto.NomPreneurAssurance;
                model.PreneurEstAssure = string.IsNullOrEmpty(contratInfoDto.PreneurEstAssure) ? false : (contratInfoDto.PreneurEstAssure == "O" ? true : false);
                model.DepAssurance = contratInfoDto.DepAssure;
                model.VilleAssurance = contratInfoDto.VilleAssure;
                model.CodeMotsClef1 = contratInfoDto.CodeMotsClef1;
                model.CodeMotsClef2 = contratInfoDto.CodeMotsClef2;
                model.CodeMotsClef3 = contratInfoDto.CodeMotsClef3;
                model.Descriptif = contratInfoDto.Descriptif;
                model.NumChronoObsv = contratInfoDto.NumChronoOsbv;
                model.Obersvations = contratInfoDto.Observations;
                model.DateEffetAnnee = contratInfoDto.DateEffetAnnee;
                model.DateEffetMois = contratInfoDto.DateEffetMois;
                model.DateEffetJour = contratInfoDto.DateEffetJour;
                model.DateEffetHeure = contratInfoDto.DateEffetHeure;
                model.FinEffetAnnee = contratInfoDto.FinEffetAnnee;
                model.FinEffetMois = contratInfoDto.FinEffetMois;
                model.FinEffetJour = contratInfoDto.FinEffetJour;
                model.FinEffetHeure = contratInfoDto.FinEffetHeure;
                model.Mois = contratInfoDto.Mois;
                model.Jour = contratInfoDto.Jour;
                model.PeriodiciteCode = contratInfoDto.PeriodiciteCode;
                model.PeriodiciteNom = contratInfoDto.PeriodiciteNom;
                model.Type = contratInfoDto.Type;
                model.Etat = contratInfoDto.Etat;
                model.Situation = contratInfoDto.Situation;
                model.Antecedent = contratInfoDto.Antecedent;
                model.Description = contratInfoDto.Description;
                if (contratInfoDto.AdresseContrat != 0)
                {
                    AdressePlatDto adresse = AdresseRepository.ObtenirAdresse(contratInfoDto.AdresseContrat);
                    if (adresse != null)
                    {
                        model.AdresseContrat = new AdressePlatDto()
                        {
                            Batiment = adresse.Batiment,
                            BoitePostale = adresse.BoitePostale,
                            ExtensionVoie = adresse.ExtensionVoie,
                            MatriculeHexavia = adresse.MatriculeHexavia,
                            NomVoie = adresse.NomVoie,
                            NumeroChrono = adresse.NumeroChrono,
                            NumeroVoie = adresse.NumeroVoie,
                            NumeroVoie2 = adresse.NumeroVoie2,
                            CodePays = adresse.CodePays,
                            NomPays = adresse.NomPays,
                            CodeInsee = adresse.CodeInsee,
                            CodePostal = adresse.CodePostal,
                            CodePostalCedex = adresse.CodePostalCedex,
                            NomVille = adresse.NomVille,
                            NomCedex = adresse.NomCedex,
                            TypeCedex = adresse.TypeCedex,
                            Departement = adresse.Departement,
                            Latitude = adresse.Latitude,
                            Longitude = adresse.Longitude
                        };
                    }
                }
                #endregion
            }
            else
            {
                #region Récupération des données d'un nouveau contrat
                model.PreneurEstAssure = true;

                //récupération du gestionnaire utilisateur
                var gestionnaire = GestionnaireRepository.ObtenirGestionnaire(user);
                if (gestionnaire != null)
                {
                    model.GestionnaireCode = gestionnaire.Id;
                    model.GestionnaireNom = gestionnaire.Nom;
                }

                #endregion
            }
            return model;
        }


        /// <summary>
        /// Créer le contrat avec les paramètres
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="typeContrat"></param>
        /// <param name="dateAccord"></param>
        /// <param name="dateEffet"></param>
        /// <param name="heureEffet"></param>
        /// <param name="contratRemp"></param>
        /// <param name="versionRemp"></param>
        /// <param name="souscripteur"></param>
        /// <param name="gestionnaire"></param>
        /// <param name="branche"></param>
        /// <param name="cible"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string CreateContrat(string codeOffre, string version, string type, string codeContrat, string versionContrat, string typeContrat, DateTime? dateAccord,
                DateTime? dateEffet, int heureEffet, string contratRemp, string versionRemp, string souscripteur, string gestionnaire, string branche, string cible, string observation, string user,
            string acteGestion)
        {
            //ToDo ECM : faire toutes les vérificationes des données en paramètres

            if (string.IsNullOrEmpty(codeContrat))
            {
                codeContrat = CommonRepository.GetAS400IdContrat(branche, cible, dateEffet.Value.Year.ToString());
                versionContrat = "0";
            }
            if (string.IsNullOrEmpty(versionRemp))
            {
                versionRemp = "0";
            }
            DbParameter[] param = new DbParameter[19];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.ToIPB());
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODECONTRAT", codeContrat.ToUpper().ToIPB());
            param[4] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[4].Value = Convert.ToInt32(versionContrat);
            param[5] = new EacParameter("P_TYPECONTRAT", typeContrat);
            param[6] = new EacParameter("P_DATEACCORD", AlbConvert.ConvertDateToInt(dateAccord).ToString());
            param[7] = new EacParameter("P_DATEEFFET", AlbConvert.ConvertDateToInt(dateEffet).ToString());
            param[8] = new EacParameter("P_HEUREEFFET", heureEffet.ToString());
            param[9] = new EacParameter("P_CONTRATREMP", contratRemp.ToUpper().ToIPB());
            param[10] = new EacParameter("P_VERSIONREMP", 0);
            param[10].Value = Convert.ToInt32(versionRemp);
            param[11] = new EacParameter("P_SOUSCRIPTEUR", souscripteur);
            param[12] = new EacParameter("P_GESTIONNAIRE", gestionnaire);
            param[13] = new EacParameter("P_BRANCHE", branche);
            param[14] = new EacParameter("P_CIBLE", cible);
            param[15] = new EacParameter("P_DATENOW", AlbConvert.ConvertDateToInt(DateTime.Now).ToString());
            param[16] = new EacParameter("P_USER", user);
            param[17] = new EacParameter("P_TRAITEMENT", AlbConstantesMetiers.Traitement.Police.AsCode());
            param[18] = new EacParameter("P_OBSERVATION", observation);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CONTRAT", param);

            CommonRepository.AlimStatistiques(codeContrat, versionContrat, user, acteGestion);

            return string.Format("{0}_{1}", codeContrat, versionContrat);
        }

        public static ContratDto GetContrat(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            if (string.IsNullOrEmpty(version))
                version = "0";

            if (modeNavig == ModeConsultation.Historique)
            {
                DbParameter[] param = new DbParameter[4];
                param[0] = new EacParameter("P_CODECONTRAT", contratId.ToIPB());
                param[1] = new EacParameter("P_VERSIONCONTRAT", version);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("P_TYPE", type);
                param[3] = new EacParameter("P_CODEAVN", 0);
                param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
                var resultHist = DbBase.Settings.ExecuteList<ContratDto>(CommandType.StoredProcedure, "SP_GETAVENANTHIST", param);
                if (resultHist != null && resultHist.Any())
                    return resultHist.FirstOrDefault();
            }
            DbParameter[] paramavn = new DbParameter[4];
            paramavn[0] = new EacParameter("P_CODECONTRAT", contratId.ToIPB());
            paramavn[1] = new EacParameter("P_VERSION", version);
            paramavn[1].Value = Convert.ToInt32(version);
            paramavn[2] = new EacParameter("P_TYPE", type);
            paramavn[3] = new EacParameter("P_CODEAVN", 0);
            paramavn[3].Value = 0;
            var result = DbBase.Settings.ExecuteList<ContratDto>(CommandType.StoredProcedure, "SP_GETAVENANT", paramavn);
            if (result != null && result.Any())
            {
                var contrat = result.FirstOrDefault();
                var cabinetCourtage = CabinetCourtageRepository.Obtenir(contrat.CourtierGestionnaire);
                contrat.Delegation = cabinetCourtage?.Delegation?.Nom;
                contrat.Inspecteur = cabinetCourtage?.Inspecteur;
                return contrat;
            }

            return null;
        }

        public static List<CourtierDto> GetListCourtiers(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            string sql = string.Format(@"SELECT PFICT CODE, TNNOM NOM,CONCAT(TCDEP,TCCPO) CP,PFCOM COMMISSION,PBCTA APPORTEUR,PBICT GEST,PBCTP PAYEUR
                                        FROM {0}
                                        INNER JOIN {1} ON PFIPB=PBIPB AND PFALX=PBALX AND {9}={7} {8}
                                        INNER JOIN {2} ON TCICT=PFICT
                                        INNER JOIN {3} ON TNICT=PFICT AND TNXN5=0 AND TNTNM='A'
                                        WHERE {7}='{4}' AND PBALX='{5}' AND PBIPB='{6}' {10}",
                                        CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                                        CommonRepository.GetPrefixeHisto(modeNavig, "YPOCOUR"),
                                        CommonRepository.GetPrefixeHisto(modeNavig, "YCOURTI"),
                                        CommonRepository.GetPrefixeHisto(modeNavig, "YCOURTN"),
                                        type, version, contratId.Trim().PadLeft(9, ' '),
                                        modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                                        modeNavig == ModeConsultation.Standard ? string.Empty : " AND PFAVN = PBAVN",
                                        modeNavig == ModeConsultation.Standard ? "PFTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                                        modeNavig == ModeConsultation.Standard ? string.Empty : string.Format(" AND PBAVN = {0}",
                                        !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            return DbBase.Settings.ExecuteList<CourtierDto>(CommandType.Text, sql);
        }
        public static CourtierDto GetCourtier(int CodeCourtier, string contratId, string version, string type)
        {
            string sql = @"SELECT PFICT CODE, TNNOM NOM,CONCAT(TCDEP,TCCPO) CP,PFCOM COMMISSION,PBCTA APPORTEUR,PBICT GEST,PBCTP PAYEUR
                                        FROM YPOBASE
                                        LEFT JOIN YPOCOUR ON PFIPB=PBIPB AND PFALX=PBALX AND PFTYP=PBTYP
                                        LEFT JOIN YCOURTI ON TCICT=PFICT
                                        LEFT JOIN YCOURTN ON TNICT=PFICT AND TNXN5=0 AND TNTNM='A'
                                        WHERE PBTYP='{0}' AND PBALX='{1}' AND PBIPB='{2}' AND PFICT='{3}'";
            return DbBase.Settings.ExecuteList<CourtierDto>(CommandType.Text, string.Format(sql, type, version, contratId, CodeCourtier)).FirstOrDefault();
        }

        public static void InfoGeneralesContratModifier(ContratDto contrat, string utilisateur, bool isModifHorsAvn)
        {
            //using (var con = DbBase.Settings.CreateConnection())
            //{
            //    var eacCon = (EacConnection)con;
            //    eacCon.Open();

            //    var dbTransaction = new EacTransaction((EacConnection)con);
            //try
            //{
            #region Informations de Base
            //InfoGeneralesContratModifierBase(contrat, dbTransaction);

            InfoGeneralesContratModifierBase(contrat, utilisateur);

            #endregion
            #region Observation
            InfoGeneralesContratModifierObsv(contrat);
            #endregion
            #region YPRTENT
            InfoGeneralesContratModifierYprtent(contrat);
            #endregion
            #region YPRTOBJ
            InfoGeneralesContratModifierYprtObj(contrat);
            #endregion
            #region KPOPT
            InfoGeneralesContratModifierKpopt(contrat);
            #endregion
            #region Frais et aperiteur code
            InfoGeneralesContratModifierTpocoas(contrat);
            #endregion
            #region date statistique
            InfoGeneralesContratModifierKpent(contrat);
            #endregion
            #region Ajout de l'acte de gestion
            if (!isModifHorsAvn)
                CommonRepository.AjouterActeGestion(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type, 0, AlbConstantesMetiers.ACTEGESTION_GESTION, AlbConstantesMetiers.TRAITEMENT_AFFNV, "", utilisateur);
            //string retourAjoutActeGestion = CommonRepository.AjouterActeGestion(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type, 0, "G", "AFFNV", "", utilisateur);
            //if (retourAjoutActeGestion == "ERR")
            //    throw new Exception();
            #endregion
            #region KPDESI
            FinOffreRepository.UpdateKPDESI(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type, !string.IsNullOrEmpty(contrat.Description) ? contrat.Description : string.Empty);
            #endregion
            //dbTransaction.Commit();
            //}
            //catch (Exception)
            //{
            //    //dbTransaction.Rollback();
            //    throw new AlbTechException(new Exception("erreur transaction"));
            //}
            //}

        }

        /// <summary>
        /// Enregistre le contrat
        /// </summary>
        /// <returns>
        /// Retourne soit le code contrat + version
        /// soit le message d'erreur précédé par MESSAGE :
        /// </returns>
        public static string UpdateContrat(ContratInfoBaseDto contrat, string utilisateur, string acteGestion, string user)
        {
            var toReturn = string.Empty;
            if (!contrat.IsModifHorsAvn)
            {
                if (!string.IsNullOrEmpty(contrat.ContratMere) && !string.IsNullOrEmpty(contrat.NumAliment))
                {
                    toReturn = string.Concat(toReturn, VerifContratMere(contrat.ContratMere, Convert.ToInt32(contrat.NumAliment), contrat.Branche, contrat.Cible));
                    if (string.IsNullOrEmpty(toReturn))
                    {
                        contrat.CodeContrat = contrat.ContratMere.ToUpper().ToIPB();
                        contrat.VersionContrat = Convert.ToInt32(contrat.NumAliment);
                    }
                }
                if (!string.IsNullOrEmpty(contrat.NumContratRemplace))
                {
                    toReturn = string.Concat(toReturn, VerifContratRemp(contrat.NumContratRemplace, Convert.ToInt32(contrat.NumAlimentRemplace)));
                    if (string.IsNullOrEmpty(toReturn))
                        contrat.NumChronoConx = CommonRepository.GenererNumeroChrono("POLICE_CONNEX_01");
                }
            }

            if (!string.IsNullOrEmpty(toReturn))
                return string.Concat("MESSAGE : ", toReturn);

            ///ECM : Changement pour la génération du numéro de contrat///
            /// => vérification que le modèle passé (ContratInfoBaseDto) contient 
            /// bien un numéro de contrat ; si non : génération du numéro
            if (string.IsNullOrEmpty(contrat.CodeContrat) || (contrat.CodeContrat.Contains("CV") && (!contrat.TemplateMode)))
            {
                contrat.CodeContrat = CommonRepository.GetAS400IdContrat(contrat.Branche, contrat.Cible, contrat.DateEffetAnnee.ToString());
                contrat.VersionContrat = 0;
            }

            var updateMode = CommonRepository.ExistRow(string.Format("SELECT COUNT(*) NBLIGN FROM YPOBASE WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'", contrat.CodeContrat.Trim().PadLeft(9, ' '), contrat.VersionContrat, contrat.Type));


            toReturn = SaveContrat(contrat, utilisateur, updateMode, contrat.AdresseContrat.NumeroChrono == 0);

            if (contrat.CodeContrat.Contains("CV"))
                ParamTemplatesRepository.UpdateTemplate(contrat.CodeContrat);

            if (contrat.CopyMode)
            {
                AffaireNouvelleRepository.CopyContratFromContrat(contrat.CodeContrat, contrat.VersionContrat.ToString(), AlbConstantesMetiers.TYPE_CONTRAT, contrat.CodeContratCopy, contrat.VersionCopy, utilisateur, acteGestion);
            }

            //Ajout appel PGM KDP025CL suite au doc remarques2.docx du 01/04/2015
            if (!updateMode)
                CommonRepository.LancementCalculAffNouv(contrat.CodeContrat, contrat.VersionContrat.ToString(), AlbConstantesMetiers.TYPE_CONTRAT);

            CommonRepository.ChangeSbr(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type, user);

            return toReturn;
        }

        /// <summary>
        /// Vérifie les données du contrat mère
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string VerifContratMere(string codeOffre, int version, string branche, string cible)
        {
            string errorStr = string.Empty;

            errorStr += CheckTypeContratMere(codeOffre, branche, cible);
            if (string.IsNullOrEmpty(errorStr))
                errorStr += CheckDispoContratMere(codeOffre, version);

            return errorStr;
        }
        /// <summary>
        /// Vérifie que le gestionnaire et le souscripteur
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>



        public static string ControleSousGest(string souscripteur, string gestionnaire)
        {
            string RetourMsg = string.Empty;

            if (gestionnaire != null && !string.IsNullOrEmpty(gestionnaire) && !GestionnaireRepository.TesterExistenceGestionnaire(gestionnaire))
            {

                RetourMsg = "Code gestionnaire inconnu";
            }
            if (souscripteur != null && !string.IsNullOrEmpty(souscripteur))
            {
                if (!SouscripteurRepository.TesterExistenceSouscripteur(souscripteur))
                {

                    RetourMsg = "Code souscripteur inconnu";
                }
            }

            return RetourMsg;



        }

        /// <summary>
        /// Contrôle la date de validité du contrat
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="dateAccord"></param>
        /// <param name="dateEffet"></param>
        /// <param name="heureEffet"></param>
        /// <returns></returns>
        public static string ControleValidateOffer(string codeOffre, string version, string type, string dateAccord, string dateEffet)
        {
            var parameters = new List<EacParameter>()
            {
                new EacParameter("codeOffre", codeOffre.ToIPB()),
                new EacParameter("version", DbType.Int32){ Value = int.Parse(version)},
                new EacParameter("type", type)
            };

            string sql = "SELECT PBDUR INT32RETURNCOL, KAADPRJ INT32RETURNCOL2 FROM YPOBASE INNER JOIN KPENT ON (KAAIPB, KAAALX, KAATYP) = (PBIPB, PBALX, PBTYP) WHERE (PBIPB, PBALX, PBTYP) = (:codeOffre, :version, :type)";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, parameters);

            if (result.Any())
            {
                var dureeProjet = result.FirstOrDefault().Int32ReturnCol;
                var dateProjet = result.FirstOrDefault().Int32ReturnCol2;

                DateTime? dEffet = AlbConvert.ConvertStrToDate(dateEffet);
                DateTime? dAccord = AlbConvert.ConvertStrToDate(dateAccord);
                DateTime? dProjet = AlbConvert.ConvertIntToDate(dateProjet);

                if (dProjet.Value.AddDays(dureeProjet) > dEffet)
                {
                    return "La date de fin de validité de l'offre ne peut être supérieure ou égale à la date de début d'effet du contrat";
                }

            }
            return string.Empty;
        }



        /// Vérifie que le contrat à être remplacé existe
        /// et qu'il est bien résilié
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string VerifContratRemp(string codeOffre, int version)
        {
            string errorStr = string.Empty;

            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.ToIPB();
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = version;

            string sql = @"SELECT PBETA ETAT, PBSIT SITUATION FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = 'P'";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault();

            if (result == null)
            {
                errorStr += "Ce contrat remplacé n'existe pas<br/>";
            }
            else if (result.Etat != "V" || result.Situation != "X")
            {
                errorStr += "Ce contrat n'est pas résilié<br/>";
            }

            return errorStr;
        }

        /// <summary>
        /// Récupère le numero d'aliment maximal +1 du contrat mère en paramètre
        /// </summary>
        /// <param name="contratMere"></param>
        /// <returns></returns>
        public static string GetNumeroAliment(string contratMere)
        {
            string sql = string.Format(@"SELECT MAX(PBALX) + 1 INT64RETURNCOL
                                         FROM YPOBASE
                                         WHERE PBIPB = :codeAffaire AND PBTYP='P' AND (PBMER = 'A' OR PBMER = 'M')");
            var param = OutilsHelper.MakeParams(sql, contratMere.ToIPB());
            var res = CommonRepository.GetInt64Value(sql, param);
            return res.HasValue ? res.Value.ToString() : string.Empty;
        }

        public static void UpdateEtatContrat(string codeContrat, long version, string type, string etat)
        {
            var (sql, param) = OutilsHelper.MakeParamsSql(@"UPDATE YPOBASE SET PBETA = :etat WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type",
                etat, codeContrat.ToIPB(), version, type);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        #region CopyAllInfo

        /// <summary>
        /// Copier informations offre - contrat
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="user"></param>
        /// <param name="splitChar"></param>
        /// <param name="acteGestion"></param>
        public static void CopyAllInfo(string codeOffre, string version, string type, string codeContrat, string versionContrat, string user, string splitChar, string acteGestion)//, bool isModifHorsAvn)
        {
            //01-Copier informations de base
            //ParallelHelper.Execute(2,
            //() => CopyAllRisques(codeOffre, version, codeContrat, versionContrat),
            //() => CopyFormules(codeOffre, version, type, codeContrat, versionContrat));
            CopyAllRisques(codeOffre, version, codeContrat, versionContrat);
            CopyFormules(codeOffre, version, type, codeContrat, versionContrat);

            if (VerifFormule(codeContrat, versionContrat))
            {
                CopyTarif(codeContrat, versionContrat);
            }
            CopyOfferToContract(codeOffre, version, type, codeContrat, versionContrat, user);
            //02-Engagement
            CommonRepository.ReloadEngagement(codeContrat, versionContrat, AlbConstantesMetiers.Traitement.Police.AsCode(), codeOffre, version, type, user, acteGestion);

            //03-Copier documents  
            CopieDocRepository.CopierDocuments(codeContrat, versionContrat, AlbConstantesMetiers.TYPE_CONTRAT, "0");
            //04-Recalcul écheance
            ComputeEcheance(codeContrat, versionContrat, AlbConstantesMetiers.TYPE_CONTRAT, user, splitChar, acteGestion, null);

            //05-Alimentation statistique
            CommonRepository.AlimStatistiques(codeContrat, version, user, acteGestion);
            //06-Calcul AN
            CommonRepository.LancementCalculAffNouv(codeContrat, versionContrat, AlbConstantesMetiers.TYPE_CONTRAT);
            //07-Ajouter un acte de gestion
            string libTrace = $"Création via {codeOffre}-{version}";
            CommonRepository.AjouterActeGestion(codeContrat, version, AlbConstantesMetiers.TYPE_CONTRAT, 0, AlbConstantesMetiers.ACTEGESTION_GESTION, AlbConstantesMetiers.TRAITEMENT_AFFNV, libTrace, user);

        }
        /// <summary>
        /// Recalcul de l'échéance principale
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="user"></param>
        /// <param name="splitChar"></param>
        /// <param name="acteGestion"></param>
        public static void ComputeEcheance(string codeContrat, string versionContrat, string typeContrat, string user, string splitChar, string acteGestion, IDbConnection connection)
        {
            var param = new EacParameter[3];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.ToIPB();
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = int.Parse(versionContrat);
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = typeContrat;

            string sql = @"SELECT PBPER PERIODICITE, 
                                                RIGHT(REPEAT('0', 2) CONCAT PBEFJ, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 2) CONCAT PBEFM, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 4) CONCAT PBEFA, 4) EFFETGARANTIE,
                                                RIGHT(REPEAT('0', 2) CONCAT PBFEJ, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 2) CONCAT PBFEM, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 4) CONCAT PBFEA, 4) FINGARANTIE,
                                                RIGHT(REPEAT('0', 2) CONCAT PBECJ, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 2) CONCAT PBECM, 2) ECHPRINC,
                                                PBCTD DUREE, PBCTU DUREEUNITE
                                            FROM YPOBASE WHERE PBIPB = :P_CODEOFFRE AND PBALX = :P_VERSION AND PBTYP = :P_TYPE WITH NC";
            // Récupération périodicité
            List<AffNouvProchaineEcheanceDto> result = null;
            if (connection == null)
            {
                result = DbBase.Settings.ExecuteList<AffNouvProchaineEcheanceDto>(CommandType.Text, sql, param);
            }
            else
            {
                using (var options = new DbSelectOptions(connection == null)
                {
                    DbConnection = connection,
                    SqlText = sql
                })
                {
                    options.BuildParameters(codeContrat.ToIPB(), int.Parse(versionContrat), typeContrat);
                    result = options.PerformSelect<AffNouvProchaineEcheanceDto>().ToList();
                }
            }
            if (result != null && result.Any())
            {
                var firstRes = result.FirstOrDefault();

                if (firstRes.Periodicite == "A" || firstRes.Periodicite == "S" || firstRes.Periodicite == "T")
                {
                    var ddEffet = AlbConvert.ConvertStrToDate(firstRes.EffetGarantie);
                    var dfEffet = AlbConvert.ConvertStrToDate(firstRes.FinGarantie);
                    var dAvenant = AlbConvert.ConvertStrToDate(firstRes.EffetGarantie);
                    DateTime? dEcheance = null;
                    if (!string.IsNullOrEmpty(firstRes.EcheancePrincipale))
                        dEcheance = AlbConvert.ConvertStrToDate(firstRes.EcheancePrincipale + "/2012");

                    if (!string.IsNullOrEmpty(firstRes.DureeUnite))
                        dfEffet = AlbConvert.GetFinPeriode(AlbConvert.ConvertStrToDate(firstRes.EffetGarantie), firstRes.Duree, firstRes.DureeUnite);
                    string dataProchEch = string.Empty;
                    if (dEcheance != null)
                        dataProchEch = CommonRepository.LoadPreavisResiliation(codeContrat, versionContrat, string.Empty, ddEffet, dfEffet, dAvenant, firstRes.Periodicite, dEcheance, splitChar, user, acteGestion);
                    if (!string.IsNullOrEmpty(dataProchEch))
                    {
                        string[] tDataProchEch = dataProchEch.Split(new[] { splitChar }, StringSplitOptions.None);
                        var prochEch = AlbConvert.ConvertStrToDate(tDataProchEch[2]);
                        var periodeDeb = AlbConvert.ConvertStrToDate(tDataProchEch[0]);
                        var periodeFin = AlbConvert.ConvertStrToDate(tDataProchEch[1]);

                        param = new EacParameter[11];
                        param[0] = new EacParameter("P_PROECHDAY", DbType.Int32);
                        param[0].Value = prochEch.HasValue ? prochEch.Value.Day : 0;
                        param[1] = new EacParameter("P_PROECHMONTH", DbType.Int32);
                        param[1].Value = prochEch.HasValue ? prochEch.Value.Month : 0;
                        param[2] = new EacParameter("P_PROECHYEAR", DbType.Int32);
                        param[2].Value = prochEch.HasValue ? prochEch.Value.Year : 0;
                        param[3] = new EacParameter("P_DATDEBDAY", DbType.Int32);
                        param[3].Value = periodeDeb.HasValue ? periodeDeb.Value.Day : 0;
                        param[4] = new EacParameter("P_DATDEBMONTH", DbType.Int32);
                        param[4].Value = periodeDeb.HasValue ? periodeDeb.Value.Month : 0;
                        param[5] = new EacParameter("P_DATDEBYEAR", DbType.Int32);
                        param[5].Value = periodeDeb.HasValue ? periodeDeb.Value.Year : 0;
                        param[6] = new EacParameter("P_DATFINDAY", DbType.Int32);
                        param[6].Value = periodeFin.HasValue ? periodeFin.Value.Day : 0;
                        param[7] = new EacParameter("P_DATFINMONTH", DbType.Int32);
                        param[7].Value = periodeFin.HasValue ? periodeFin.Value.Month : 0;
                        param[8] = new EacParameter("P_DATFINYEAR", DbType.Int32);
                        param[8].Value = periodeFin.HasValue ? periodeFin.Value.Year : 0;
                        param[9] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                        param[9].Value = codeContrat.ToIPB();
                        param[10] = new EacParameter("P_VERSION", DbType.Int32);
                        param[10].Value = versionContrat;

                        sql = @"UPDATE YPRTENT SET JDPEJ = :P_PROECHDAY, JDPEM = :P_PROECHMONTH, JDPEA = :P_PROECHYEAR,
                                            JDDPJ = :P_DATDEBDAY, JDDPM = :P_DATDEBMONTH, JDDPA = :P_DATDEBYEAR, JDFPJ = :P_DATFINDAY, JDFPM = :P_DATFINMONTH, JDFPA = :P_DATFINYEAR
                                WHERE JDIPB = :P_CODECONTRAT AND JDALX = :P_VERSION";

                        if (connection == null)
                        {
                            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                        }
                        else
                        {
                            DbBase.Settings.ExecuteNonQuery((DbConnection)connection, CommandType.Text, sql, param);
                        }
                    }
                }
            }



        }
        /// <summary>
        /// Copier tous les risques de l'offre dans KPOFRSQ
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        public static void CopyAllRisques(string codeOffre, string version, string codeContrat, string versionContrat)
        {
            DbParameter[] param = new DbParameter[5];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.ToIPB());
            param[1] = new EacParameter("P_VERSION", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };

            param[2] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[3] = new EacParameter("P_VERSIONCONTRAT", 0)
            {
                Value = !string.IsNullOrEmpty(versionContrat) ? Convert.ToInt32(versionContrat) : 0
            };

            // modifier SP_CORQOB par SP_CORISQUE
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CORISQUE", param);

        }
        /// <summary>
        /// Copier formules/options
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        public static void CopyFormules(string codeOffre, string version, string type, string codeContrat, string versionContrat)
        {
            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.ToIPB());
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[4] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[4].Value = !string.IsNullOrEmpty(versionContrat) ? Convert.ToInt32(versionContrat) : 0;
            param[5] = new EacParameter("P_RETURNVAL", 0);
            param[5].Value = 0;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_COOPVOL", param);

        }
        /// <summary>
        /// Copier offre vers un contrat
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="user"></param>
        public static void CopyOfferToContract(string codeOffre, string version, string type, string codeContrat, string versionContrat, string user)
        {
            DbParameter[] param = new DbParameter[8];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.ToIPB());
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[4] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[4].Value = !string.IsNullOrEmpty(versionContrat) ? Convert.ToInt32(versionContrat) : 0;
            param[5] = new EacParameter("P_DATESYSTEME", 0);
            param[5].Value = AlbConvert.ConvertDateToInt(DateTime.Now).ToString();
            param[6] = new EacParameter("P_USER", user);
            param[7] = new EacParameter("P_TRAITEMENT", AlbConstantesMetiers.Traitement.Police.AsCode());

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_AFFNOUV", param);

        }

        private static bool VerifFormule(string codeContrat, string versionContrat)
        {


            DbParameter[] param = new DbParameter[2];

            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[1] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[1].Value = !string.IsNullOrEmpty(versionContrat) ? Convert.ToInt32(versionContrat) : 0;

            string sql =
                @"SELECT COUNT ( * ) NBLIGN 
		          FROM KPOFOPT 
			INNER JOIN KPGARAN ON KFJIPB = KDEIPB AND KFJALX = KDEALX AND KDETYP = 'O' AND KFJFOR = KDEFOR AND KFJOPT = KDEOPT 
			INNER JOIN KPGARTAR ON KDEID = KDGKDEID 
		    WHERE KFJPOG = :P_CODECONTRAT AND KFJALG = :P_VERSIONCONTRAT AND KFJTENG = 'V' AND KFJSEL = 'O' 
			AND EXISTS ( SELECT KDGID FROM KPGARTAR WHERE KDGNUMTAR > 1 AND KDGIPB = KDEIPB AND KDGALX = KDEALX AND KDGTYP = KDETYP ) ; ";


            return CommonRepository.ExistRow(sql, param);
        }

        public static void CopyTarif(string codeContrat, string versionContrat)
        {
            DbParameter[] param = new DbParameter[2];

            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[1] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[1].Value = !string.IsNullOrEmpty(versionContrat) ? Convert.ToInt32(versionContrat) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_COTARIF", param);

        }

        #endregion
        public static void UpdateTypeRegul(string codeOffre, string version, string type, string newType, string dateDebAvn, string codeAvn, string codeReg)
        {
            EacParameter[] param = new EacParameter[8];
            param[0] = new EacParameter("typeTraitement", DbType.AnsiStringFixedLength);
            param[0].Value = newType.Trim();
            param[1] = new EacParameter("DebAvnYear", DbType.Int32);
            param[1].Value = Convert.ToInt32(dateDebAvn.Split(new[] { '/' })[2]);
            param[2] = new EacParameter("DebAvnMonth", DbType.Int32);
            param[2].Value = Convert.ToInt32(dateDebAvn.Split(new[] { '/' })[1]);
            param[3] = new EacParameter("DebAvnDay", DbType.Int32);
            param[3].Value = Convert.ToInt32(dateDebAvn.Split(new[] { '/' })[0]);
            param[4] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[4].Value = codeOffre.ToIPB();
            param[5] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[5].Value = version;
            param[6] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[6].Value = type;
            param[7] = new EacParameter("codeAvn", DbType.AnsiStringFixedLength);
            param[7].Value = codeAvn;

            string sql = @"UPDATE YPOBASE 
                            SET PBTTR = :typeTraitement,
                                PBAVA = :DebAvnYear,
                                PBAVM = :DebAvnMonth,
                                PBAVJ = :DebAvnDay
                            WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvn";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            var dateDeb = dateDebAvn.Split(new[] { '/' })[2] + dateDebAvn.Split(new[] { '/' })[1] + dateDebAvn.Split(new[] { '/' })[0];

            param = new EacParameter[6];
            param[0] = new EacParameter("typeTraitement", DbType.AnsiStringFixedLength);
            param[0].Value = newType.Trim();
            param[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[1].Value = codeOffre.ToIPB();
            param[2] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[2].Value = version;
            param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[3].Value = type;
            param[4] = new EacParameter("codeAvn", DbType.AnsiStringFixedLength);
            param[4].Value = codeAvn;
            param[5] = new EacParameter("codeReg", DbType.AnsiStringFixedLength);
            param[5].Value = codeReg;

            sql = @"UPDATE KPRGU 
                            SET KHWTTR = :typeTraitement, KHWAFC = '', KHWAFR = 0
                            WHERE KHWIPB = :codeOffre AND KHWALX = :version AND KHWTYP = :type AND KHWAVN = :codeAvn AND KHWID = :codeReg";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        #endregion

        #region Méthodes Privées

        /// <summary>
        /// Appel de la procédure stockée pour l'enregistrement du contrat
        /// </summary>
        private static string SaveContrat(ContratInfoBaseDto contrat, string utilisateur, bool updateMode = false, bool insAdr = false)
        {
            if (contrat.Type != AlbConstantesMetiers.TYPE_CONTRAT)
                throw new AlbFoncException(string.Format("Erreur d'enregistrement du contrat n°", contrat.CodeContrat.Trim()), true, true);

            string codePostalCedex = string.Empty;
            string codePostal = string.Empty;
            string numeroVoie = string.Empty;

            if (contrat.AdresseContrat != null)
            {

                int cp = 0;

                if (contrat.AdresseContrat.CodePostal.ToString().Length == 5)
                {
                    int.TryParse(contrat.AdresseContrat.CodePostal.ToString().Substring(2, 3), out cp);
                    codePostal = cp.ToString().PadLeft(3, '0');
                }
                else if (contrat.AdresseContrat.CodePostal != -1)
                {
                    codePostal = contrat.AdresseContrat.CodePostal.ToString().PadLeft(3, '0');
                }

                if (contrat.AdresseContrat.NumeroVoie != -1)
                {
                    numeroVoie = contrat.AdresseContrat.NumeroVoie.ToString();
                }

                int cpCedex = 0;

                if (contrat.AdresseContrat.CodePostalCedex != 0)
                {
                    if (contrat.AdresseContrat.CodePostalCedex.ToString().Length == 5)
                    {
                        Int32.TryParse(contrat.AdresseContrat.CodePostalCedex.ToString().Substring(2, 3), out cpCedex);
                        codePostalCedex = cpCedex.ToString().PadLeft(3, '0');
                    }
                    else if (contrat.AdresseContrat.CodePostalCedex != -1)
                    {
                        codePostalCedex = contrat.AdresseContrat.CodePostalCedex.ToString().PadLeft(3, '0');
                    }
                }
            }

            DbParameter[] param = new DbParameter[64];
            param[0] = new EacParameter("P_CODECONTRAT", contrat.CodeContrat.ToIPB());
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = contrat.VersionContrat;
            param[2] = new EacParameter("P_TYPE", contrat.Type);
            param[3] = new EacParameter("P_UPDATEMODE", updateMode ? "O" : "N");
            param[4] = new EacParameter("P_DATENOW", 0);
            param[4].Value = AlbConvert.ConvertDateToInt(DateTime.Now);
            param[5] = new EacParameter("P_BRANCHE", contrat.Branche);
            param[6] = new EacParameter("P_CIBLE", contrat.Cible);
            param[7] = new EacParameter("P_PRENEURASS", 0);
            param[7].Value = contrat.CodePreneurAssurance;
            param[8] = new EacParameter("P_DESCRIPTIF", contrat.Descriptif);
            param[9] = new EacParameter("P_COURTIERGEST", 0);
            param[9].Value = contrat.CourtierGestionnaire;
            param[10] = new EacParameter("P_INTERLOCUTEUR", 0);
            param[10].Value = contrat.CodeInterlocuteur;
            param[11] = new EacParameter("P_REFCOURTIER", !string.IsNullOrEmpty(contrat.RefCourtier) ? contrat.RefCourtier : string.Empty);
            param[12] = new EacParameter("P_ANNEEACCORD", 0);
            param[12].Value = contrat.DateAccordAnnee;
            param[13] = new EacParameter("P_MOISACCORD", 0);
            param[13].Value = contrat.DateAccordMois;
            param[14] = new EacParameter("P_JOURACCORD", 0);
            param[14].Value = contrat.DateAccordJour;
            param[15] = new EacParameter("P_HEURENOW", 0);
            param[15].Value = AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(DateTime.Now));
            param[16] = new EacParameter("P_MOTCLE1", !string.IsNullOrEmpty(contrat.CodeMotsClef1) ? contrat.CodeMotsClef1 : string.Empty);
            param[17] = new EacParameter("P_MOTCLE2", !string.IsNullOrEmpty(contrat.CodeMotsClef2) ? contrat.CodeMotsClef2 : string.Empty);
            param[18] = new EacParameter("P_MOTCLE3", !string.IsNullOrEmpty(contrat.CodeMotsClef3) ? contrat.CodeMotsClef3 : string.Empty);
            param[19] = new EacParameter("P_SOUSCRIPTEUR", contrat.SouscripteurCode);
            param[20] = new EacParameter("P_GESTIONNAIRE", contrat.GestionnaireCode);
            param[21] = new EacParameter("P_ANNEEEFFET", 0);
            param[21].Value = contrat.DateEffetAnnee;
            param[22] = new EacParameter("P_MOISEFFET", 0);
            param[22].Value = contrat.DateEffetMois;
            param[23] = new EacParameter("P_JOUREFFET", 0);
            param[23].Value = contrat.DateEffetJour;
            param[24] = new EacParameter("P_HEUREEFFET", 0);
            param[24].Value = contrat.DateEffetHeure;
            param[25] = new EacParameter("P_COURTIERAPP", 0);
            param[25].Value = contrat.CourtierApporteur;
            param[26] = new EacParameter("P_TYPECONTRAT", contrat.TypePolice == "S" ? string.Empty : contrat.TypePolice);
            param[27] = new EacParameter("P_USER", utilisateur);
            param[28] = new EacParameter("P_COURTIERPAY", 0);
            param[28].Value = contrat.CourtierPayeur;
            param[29] = new EacParameter("P_CODEREMP", contrat.ContratRemplace ? "O" : "N");
            param[30] = new EacParameter("P_ADRCHR", 0);
            param[30].Value = contrat.AdresseContrat.NumeroChrono;
            param[31] = new EacParameter("P_OBSERVATION", !string.IsNullOrEmpty(contrat.Obersvations) ? contrat.Obersvations : string.Empty);
            param[32] = new EacParameter("P_OBSVCHR", 0);
            param[32].Value = contrat.NumChronoObsv;
            param[33] = new EacParameter("P_ENCAISSEMENT", contrat.Encaissement);
            param[34] = new EacParameter("P_CONTRATREMPLACE", contrat.ContratRemplace ? "O" : "N");
            param[35] = new EacParameter("P_CONXCHR", 0);
            param[35].Value = contrat.NumChronoConx;
            param[36] = new EacParameter("P_CODEREMPLACE", !string.IsNullOrEmpty(contrat.NumContratRemplace) ? contrat.NumContratRemplace : string.Empty);
            param[37] = new EacParameter("P_VERSIONREMPLACE", 0);
            param[37].Value = !string.IsNullOrEmpty(contrat.NumAlimentRemplace) ? Convert.ToInt32(contrat.NumAlimentRemplace) : 0;
            param[38] = new EacParameter("P_BRANCHEREMPLACE", !string.IsNullOrEmpty(contrat.BrancheRemp) ? contrat.BrancheRemp : string.Empty);
            param[39] = new EacParameter("P_INSADR", insAdr ? "O" : "N");
            param[40] = new EacParameter("P_ADRBATIMENT", !string.IsNullOrEmpty(contrat.AdresseContrat.Batiment) ? contrat.AdresseContrat.Batiment : string.Empty);
            param[41] = new EacParameter("P_ADRNUMVOIE", numeroVoie);// contrat.AdresseContrat.NumeroVoie.ToString());
            param[42] = new EacParameter("P_ADRNUMVOIE2", !string.IsNullOrEmpty(contrat.AdresseContrat.NumeroVoie2) ? contrat.AdresseContrat.NumeroVoie2 : string.Empty);// contrat.AdresseContrat.NumeroVoie.ToString());
            param[43] = new EacParameter("P_ADREXTVOIE", !string.IsNullOrEmpty(contrat.AdresseContrat.ExtensionVoie) ? contrat.AdresseContrat.ExtensionVoie.Substring(0, 1) : string.Empty);
            param[44] = new EacParameter("P_ADRNOMVOIE", !string.IsNullOrEmpty(contrat.AdresseContrat.NomVoie) ? contrat.AdresseContrat.NomVoie : string.Empty);
            param[45] = new EacParameter("P_ADRBP", !string.IsNullOrEmpty(contrat.AdresseContrat.BoitePostale) ? contrat.AdresseContrat.BoitePostale : string.Empty);
            param[46] = new EacParameter("P_ADRCP", codePostal); //!string.IsNullOrEmpty(contrat.AdresseContrat.CodePostal.ToString()) ? contrat.AdresseContrat.CodePostal.ToString() : !string.IsNullOrEmpty(contrat.AdresseContrat.CodePostalCedex.ToString()) ? contrat.AdresseContrat.CodePostalCedex.ToString() : string.Empty);


            //param[46] = new EacParameter("P_ADRDEP", !string.IsNullOrEmpty(contrat.AdresseContrat.Ville.CodePostal) && contrat.AdresseContrat.Ville.CodePostal.Length > 4 ? contrat.AdresseContrat.Ville.CodePostal.Substring(0, 2) : string.Empty);
            param[47] = new EacParameter("P_ADRDEP", !string.IsNullOrEmpty(contrat.AdresseContrat.Departement) ? contrat.AdresseContrat.Departement : string.Empty);
            param[48] = new EacParameter("P_ADRVILLE", !string.IsNullOrEmpty(contrat.AdresseContrat.NomVille) ? contrat.AdresseContrat.NomVille : !string.IsNullOrEmpty(contrat.AdresseContrat.NomCedex) ? contrat.AdresseContrat.NomCedex : string.Empty);
            param[49] = new EacParameter("P_ADRCPX", codePostalCedex); // !string.IsNullOrEmpty(contrat.AdresseContrat.CodePostalCedex.ToString()) ? contrat.AdresseContrat.CodePostalCedex.ToString() : string.Empty);
            param[50] = new EacParameter("P_ADRVILLEX", !string.IsNullOrEmpty(contrat.AdresseContrat.NomCedex) ? contrat.AdresseContrat.NomCedex : string.Empty);
            param[51] = new EacParameter("P_ADRMATHEX", 0);
            param[51].Value = !string.IsNullOrEmpty(contrat.AdresseContrat.MatriculeHexavia) ? Convert.ToInt32(contrat.AdresseContrat.MatriculeHexavia) : 0;
            param[52] = new EacParameter("P_PRENEURESTASSURE", contrat.PreneurEstAssure ? "O" : "N");

            param[53] = new EacParameter("P_AVENANT", 0);
            param[53].Value = !string.IsNullOrEmpty(contrat.NumAvenant) ? Convert.ToInt32(contrat.NumAvenant) : 0;
            param[54] = new EacParameter("P_TYPEAVT", contrat.TypeAvt);

            param[55] = new EacParameter("P_ANNEEEFFETAVENANT", 0);
            param[55].Value = !string.IsNullOrEmpty(contrat.NumAvenant) && Convert.ToInt32(contrat.NumAvenant) > 0 ? contrat.AnneeEffetAvenant : contrat.DateEffetAnnee;
            param[56] = new EacParameter("P_MOISEFFETAVENANT", 0);
            param[56].Value = !string.IsNullOrEmpty(contrat.NumAvenant) && Convert.ToInt32(contrat.NumAvenant) > 0 ? contrat.MoisEffetAvenant : contrat.DateEffetMois;
            param[57] = new EacParameter("P_JOUREFFETAVENANT", 0);
            param[57].Value = !string.IsNullOrEmpty(contrat.NumAvenant) && Convert.ToInt32(contrat.NumAvenant) > 0 ? contrat.JourEffetAvenant : contrat.DateEffetJour;
            param[58] = new EacParameter("P_HEUREEFFETAVENANT", 0);
            param[58].Value = !string.IsNullOrEmpty(contrat.NumAvenant) && Convert.ToInt32(contrat.NumAvenant) > 0 ? contrat.HeureEffetAvenant : contrat.DateEffetHeure;
            param[59] = new EacParameter("P_ISADDRESSEMPTY", DbType.Int32);


            /***** Ajout Latitude et Longitude *****/
            param[60] = new EacParameter("P_ADRLATITUDE", contrat.AdresseContrat.Latitude != null ? contrat.AdresseContrat.Latitude : 0);
            param[61] = new EacParameter("P_ADRLONGITUDE", contrat.AdresseContrat.Longitude != null ? contrat.AdresseContrat.Longitude : 0);
            /****** Fin  Ajout Latitude et Longitude ******/

            param[62] = new EacParameter("P_INSRSQOBJ", DbType.Int32) { Value = 1 };

            param[63] = new EacParameter("P_OUTCODECONTRAT", string.Empty);
            param[63].Direction = ParameterDirection.InputOutput;
            param[63].Size = 9;

            var tmp = string.Concat(param[40].Value.ToString(), param[41].Value.ToString(), param[42].Value.ToString(), param[43].Value.ToString(), param[44].Value.ToString(),
                param[45].Value.ToString(), param[46].Value.ToString(), param[47].Value.ToString(), param[48].Value.ToString(), param[49].Value.ToString(), param[50].Value.ToString());

            if (tmp.Trim() == "")
            {
                param[59].Value = 1;
            }
            else
            {
                param[59].Value = 0;
            }

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVCONT", param);

            return param[63].Value.ToString();
        }

        private static void CopyContratFromContrat(string code, string version, string type, string codeCopy, string versionCopy, string user, string acteGestion)
        {
            string traitement = "N";

            string strDate = AlbConvert.ConvertDateToInt(DateTime.Now).ToString();

            string modeCopie = "AFNCOPY";
            if (codeCopy.Contains("CV"))
                modeCopie = "CNVA";

            DbParameter[] param = new DbParameter[9];
            param[0] = new EacParameter("P_CODECONTRAT", code.ToUpper().ToIPB());
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_COPYCODECONTRAT", codeCopy.ToUpper().ToIPB());
            param[3] = new EacParameter("P_COPYVERSION", 0);
            param[3].Value = Convert.ToInt32(versionCopy);
            param[4] = new EacParameter("P_TYPE", type);
            param[5] = new EacParameter("P_DATESYSTEME", strDate);
            param[6] = new EacParameter("P_USER", user);
            param[7] = new EacParameter("P_TRAITEMENT", traitement);
            param[8] = new EacParameter("P_MODECOPY", modeCopie);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CONTRATCOPIE", param);

            if (modeCopie == "CNVA")
            {
                CommonRepository.ReloadEngagement(code, version, type, codeCopy, versionCopy, type, user, acteGestion);
            }

            CommonRepository.AlimStatistiques(code, version, user, acteGestion);

            //Copy des documents 
            CopieDocRepository.CopierDocuments(code.ToUpper().ToIPB(), version, type, "0");

        }

        /// <summary>
        /// Charge la liste des contrats créés à partir cette offre
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static List<CreationAffaireNouvelleContratDto> LoadListContrat(string codeOffre, string version)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[0].Value = "P";
            param[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[1].Value = codeOffre.ToIPB();
            param[2] = new EacParameter("version", DbType.Int32);
            param[2].Value = Convert.ToInt32(version);

            string sql = string.Format(@"SELECT PBIPB CODECONTRAT, PBALX VERSCONTRAT, IFNULL(NULLIF(PBTYP, ''), 'S') TYPECONTRAT, PBAVN CODEAVN, IFNULL(TPLIL, '') LIBTYPECONTRAT, 
                            PBEFJ CONCAT '/' CONCAT PBEFM CONCAT '/' CONCAT PBEFA DATEEFFET,  
                            PBSAJ CONCAT '/' CONCAT PBSAM CONCAT '/' CONCAT PBSAA DATEACCORD,  
                            IFNULL(B.PJIPB, '') CODECONTRATREMP, IFNULL(B.PJALX, 0) VERSCONTRATREMP,  
                            PBSOU SOUSCRIPTEUR, PBGES GESTIONNAIRE, IFNULL(KHFSIT, '') ETAT
                    FROM YPOBASE 
                        {0}
                        LEFT JOIN KPOFENT ON KFHPOG = PBIPB AND KFHALG = PBALX
                        LEFT JOIN YPOCONX A ON A.PJTYP = 'P' AND A.PJCCX = '01' AND A.PJIPB = PBIPB AND A.PJALX = PBALX
                        LEFT JOIN YPOCONX B ON B.PJTYP = 'P' AND B.PJCCX = '01' AND B.PJCNX = A.PJCNX AND B.PJIPB <> PBIPB AND B.PJALX <> PBALX
                    WHERE PBTYP = :type AND PBOFF = :codeOffre AND PBOFV = :version
                    ORDER BY PBIPB, PBALX",
                CommonRepository.BuildJoinYYYYPAR("LEFT", "KHEOP", "TYPOC", otherCriteria: " AND TCOD = IFNULL(NULLIF(PBMER, ''), 'S')"));

            return DbBase.Settings.ExecuteList<CreationAffaireNouvelleContratDto>(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Vérifie si le contrat passé est bien un contrat mère
        /// </summary>
        /// <param name="codeOffre"></param>
        private static string CheckTypeContratMere(string codeOffre, string branche, string cible)
        {
            string errorStr = string.Empty;

            string sql = string.Format(@"SELECT PBMER TYPECONTRAT, PBETA ETAT, PBBRA BRANCHE, KAACIBLE CIBLE  
                           FROM YPOBASE 
                           INNER JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP
                           WHERE PBIPB = '{0}' AND PBALX = 0 AND PBTYP = 'P'", codeOffre.ToUpper().ToIPB());
            var resReqTypeContrat = DbBase.Settings.ExecuteList<CreationAffaireNouvelleContratDto>(CommandType.Text, sql);

            if (resReqTypeContrat == null || !resReqTypeContrat.Any())
                errorStr = "Erreur : le numéro de contrat saisi est inexistant<br/>";
            else
            {
                if (resReqTypeContrat.FirstOrDefault().TypeContrat != "M" || resReqTypeContrat.FirstOrDefault().Etat != "V")
                    errorStr = "Erreur : le numéro de contrat saisi doit correspondre à un contrat mère validé<br/>";
                if (resReqTypeContrat.FirstOrDefault().Branche != branche || resReqTypeContrat.FirstOrDefault().Cible != cible)
                    errorStr = "Erreur : La branche et la cible du contrat aliment doivent être identiques à celles du contrat mère sélectionné<br/>";

            }
            return errorStr;
        }

        /// <summary>
        /// Vérifie si le contrat passé avec la version n'existe pas
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        private static string CheckDispoContratMere(string codeOffre, int version)
        {
            string errorStr = string.Empty;

            if (version == 0)
                return "Erreur : le numéro d'aliment doit être unique et différent de 0<br/>";

            string sql = string.Format(@"SELECT COUNT(*) NBLIGN 
                           FROM YPOBASE 
                           WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = 'P'", codeOffre.ToUpper().ToIPB(), version);

            Int64 nbRow = CommonRepository.RowNumber(sql);
            if (nbRow > 0)
                errorStr = "Erreur : le numéro d'aliment doit être unique et différent de 0<br/>";

            return errorStr;
        }



        private static void InfoGeneralesContratModifierBase(ContratDto contrat, string user)
        {
            string sqlFinal = string.Empty;
            if (contrat.NumAvenant == 0 && contrat.Type == "P")
            {

                CommonRepository.DeleteTraceFormule(contrat.CodeContrat, (int)contrat.VersionContrat, contrat.Type,
                                                    contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour, contrat.DateEffetHeure);
                CommonRepository.DisableLinkNavigation(contrat, new List<string>() { "COT", "FIN" }, false);

                var sql = @"UPDATE YPOBASE 
                           SET PBREF='{2}',
                           PBMO1='{3}' , PBMO2='{4}', PBMO3='{5}',
                           PBDEV='{6}',
                           PBPER='{7}',
                           PBECM='{8}',
                           PBECJ='{9}',
                           PBEFA='{10}', PBEFM='{11}', PBEFJ='{12}',                        
                           PBFEA='{13}',PBFEM='{14}', PBFEJ='{15}', PBFEH='{16}',
                           PBCTD='{17}',PBCTU='{18}',
                           PBSAA='{19}', PBSAM ='{20}',PBSAJ ='{21}',
                           PBNPL='{22}',
                           PBAPP='{23}',
                           PBPCV='{24}',
                           PBSOU='{25}',
                           PBGES='{26}',
                           PBEFH='{27}',
                           PBMJU='{29}',
                           PBMJA='{30}',
                           PBMJM='{31}',
                           PBMJJ='{32}',
                           PBRGT='{33}',
                           PBANT='{34}', PBAPR = '{35}',
                           PBAVA = '{36}', PBAVM='{37}',PBAVJ='{38}',PBSTP='{39}'
                           WHERE  PBIPB='{0}' AND PBALX='{1}' AND PBTYP = '{28}'";
                sqlFinal = string.Format(CultureInfo.InvariantCulture, sql, contrat.CodeContrat.ToIPB(), contrat.VersionContrat, contrat.Descriptif.Replace("'", "''"), contrat.CodeMotsClef1, contrat.CodeMotsClef2, contrat.CodeMotsClef3,
                                               contrat.Devise, contrat.PeriodiciteCode, contrat.Mois, contrat.Jour, contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour,
                                               contrat.FinEffetAnnee, contrat.FinEffetMois, contrat.FinEffetJour, contrat.FinEffetHeure, contrat.DureeGarantie, contrat.UniteDeTemps,
                                               contrat.DateAccordAnnee, contrat.DateAccordMois, contrat.DateAccordJour, contrat.NatureContrat, contrat.PartAlbingia, contrat.Couverture,
                                               contrat.SouscripteurCode, contrat.GestionnaireCode, contrat.DateEffetHeure, contrat.Type, user,
                                               DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, contrat.CodeRegime, contrat.Antecedent, contrat.AperiteurCode, contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour, contrat.ZoneStop);
            }
            else
            {
                string sql = @"UPDATE YPOBASE 
                           SET PBREF='{2}',
                           PBMO1='{3}' , PBMO2='{4}', PBMO3='{5}',
                           PBDEV='{6}',
                           PBPER='{7}',
                           PBECM='{8}',
                           PBECJ='{9}',
                           PBEFA='{10}', PBEFM='{11}', PBEFJ='{12}',                           
                           PBFEA='{13}',PBFEM='{14}', PBFEJ='{15}', PBFEH='{16}',
                           PBCTD='{17}',PBCTU='{18}',
                           PBSAA='{19}', PBSAM ='{20}',PBSAJ ='{21}',
                           PBNPL='{22}',
                           PBAPP='{23}',
                           PBPCV='{24}',
                           PBSOU='{25}',
                           PBGES='{26}',
                           PBEFH='{27}',
                           PBMJU='{29}',
                           PBMJA='{30}',
                           PBMJM='{31}',
                           PBMJJ='{32}',
                           PBRGT='{33}',
                           PBANT='{34}', PBAPR = '{35}',PBSTP='{36}'
                           WHERE  PBIPB='{0}' AND PBALX='{1}' AND PBTYP = '{28}'";
                sqlFinal = string.Format(CultureInfo.InvariantCulture, sql, contrat.CodeContrat.ToIPB(), contrat.VersionContrat, contrat.Descriptif.Replace("'", "''"), contrat.CodeMotsClef1, contrat.CodeMotsClef2, contrat.CodeMotsClef3,
                                               contrat.Devise, contrat.PeriodiciteCode, contrat.Mois, contrat.Jour, contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour,
                                               contrat.FinEffetAnnee, contrat.FinEffetMois, contrat.FinEffetJour, contrat.FinEffetHeure, contrat.DureeGarantie, contrat.UniteDeTemps,
                                               contrat.DateAccordAnnee, contrat.DateAccordMois, contrat.DateAccordJour, contrat.NatureContrat, contrat.PartAlbingia, contrat.Couverture,
                                               contrat.SouscripteurCode, contrat.GestionnaireCode, contrat.DateEffetHeure, contrat.Type, user,
                                               DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, contrat.CodeRegime, contrat.Antecedent, contrat.AperiteurCode, contrat.ZoneStop);
            }
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlFinal);


        }
        private static void InfoGeneralesContratModifierObsv(ContratDto contrat)
        {
            Int64 idObsv = 0;
            string observations = !string.IsNullOrEmpty(contrat.Observations) ? contrat.Observations : string.Empty;

            var param = new EacParameter[3];
            param[0] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[0].Value = contrat.CodeContrat.ToIPB();
            param[1] = new EacParameter("P_VERSION", DbType.Int64);
            param[1].Value = contrat.VersionContrat;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = contrat.Type;

            string sqlCodeObsv = @"SELECT KAAOBSV INT64RETURNCOL
                        				         FROM KPENT 
                        				         WHERE KAAIPB=:P_CODECONTRAT
                        				         AND KAAALX=:P_VERSION 
                        				         AND KAATYP=:P_TYPE";
            //, contrat.CodeContrat.ToIPB(), contrat.VersionContrat, contrat.Type);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlCodeObsv, param);
            if (result != null && result.Any())
                idObsv = result.FirstOrDefault().Int64ReturnCol.HasValue ? result.FirstOrDefault().Int64ReturnCol.Value : 0;
            if (idObsv == 0)
            {
                idObsv = CommonRepository.GetAS400Id("KAJCHR");

                param = new EacParameter[6];
                param[0] = new EacParameter("P_IDOBSV", DbType.Int64);
                param[0].Value = idObsv;
                param[1] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[1].Value = contrat.Type;
                param[2] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                param[2].Value = contrat.CodeContrat.ToIPB();
                param[3] = new EacParameter("P_VERSION", DbType.Int64);
                param[3].Value = contrat.VersionContrat;
                param[4] = new EacParameter("P_TYPOBS", DbType.AnsiStringFixedLength);
                param[4].Value = "GENERALE";
                param[5] = new EacParameter("P_OBS", DbType.AnsiStringFixedLength);
                param[5].Value = observations.Trim();

                string sqlInsertObsv = @"INSERT INTO KPOBSV (KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJTYPOBS, KAJOBSV) 
                                                        VALUES (:P_IDOBSV, :P_TYPE, :P_CODECONTRAT, :P_VERSION, :P_TYPOBS, :P_OBS)";
                //idObsv, contrat.Type, contrat.CodeContrat.ToIPB(), contrat.VersionContrat, "GENERALE", observations.Trim());
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlInsertObsv, param);

                param = new EacParameter[4];
                param[0] = new EacParameter("P_IDOBSV", DbType.Int64);
                param[0].Value = idObsv;
                param[1] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                param[1].Value = contrat.CodeContrat.ToIPB();
                param[2] = new EacParameter("P_VERSION", DbType.Int64);
                param[2].Value = contrat.VersionContrat;
                param[3] = new EacParameter("P_TYPE", DbType.Int64);
                param[3].Value = contrat.Type;

                string sqlUpdateKPENT = @"UPDATE KPENT SET KAAOBSV = :P_IDOBSV 
                                                        WHERE KAAIPB=:P_CODECONTRAT AND KAAALX=:P_VERSION AND KAATYP=:P_TYPE";
                //, idObsv, contrat.CodeContrat.ToIPB(), contrat.VersionContrat, contrat.Type);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdateKPENT, param);
            }
            else
            {
                int nbLigne = 0;

                param = new EacParameter[1];
                param[0] = new EacParameter("P_IDOBSV", DbType.Int64);
                param[0].Value = idObsv;

                string sqlExistKPOBSV = @"SELECT COUNT(*) INT32RETURNCOL FROM KPOBSV WHERE KAJCHR = :P_IDOBSV";

                var res = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlExistKPOBSV, param);
                if (res != null && res.Any())
                    nbLigne = res.FirstOrDefault().Int32ReturnCol;
                if (nbLigne > 0)
                {
                    param = new EacParameter[2];
                    param[0] = new EacParameter("P_OBS", DbType.AnsiStringFixedLength);
                    param[0].Value = observations.Trim();
                    param[1] = new EacParameter("P_IDOBSV", DbType.Int64);
                    param[1].Value = idObsv;

                    string sql = @"UPDATE KPOBSV 
                                             SET KAJOBSV=:P_OBS
                                             WHERE KAJCHR = :P_IDOBSV";

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                }
                else
                {
                    param = new EacParameter[6];
                    param[0] = new EacParameter("P_IDOBSV", DbType.Int64);
                    param[0].Value = idObsv;
                    param[1] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                    param[1].Value = contrat.Type;
                    param[2] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                    param[2].Value = contrat.CodeContrat.ToIPB();
                    param[3] = new EacParameter("P_VERSION", DbType.Int64);
                    param[3].Value = contrat.VersionContrat;
                    param[4] = new EacParameter("P_TYPOBS", DbType.AnsiStringFixedLength);
                    param[4].Value = "GENERALE";
                    param[5] = new EacParameter("P_OBS", DbType.AnsiStringFixedLength);
                    param[5].Value = observations.Trim();

                    string sqlInsertObsv = @"INSERT INTO KPOBSV (KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJTYPOBS, KAJOBSV) 
                                                        VALUES (:P_IDOBSV, :P_TYPE, :P_CODECONTRAT, :P_VERSION, :P_TYPOBS, :P_OBS)";
                    //idObsv, contrat.Type, contrat.CodeContrat.ToIPB(), contrat.VersionContrat, "GENERALE", observations.Trim());
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlInsertObsv, param);
                }
            }
        }
        private static void InfoGeneralesContratModifierYprtent(ContratDto contrat)
        {
            //todo indice

            string sql = @"UPDATE YPRTENT 
                           SET JDDPV ='{2}',
                           JDIND='{3}',
                           JDIVA='{4}',
                           JDITC='{5}',
                           JDCNA='{6}',
                           JDDPJ='{7}',
                           JDDPM='{8}',
                           JDDPA='{9}',
                           JDFPJ='{10}',
                           JDFPM='{11}',
                           JDFPA='{12}',
                           JDPEJ='{13}',
                           JDPEM='{14}',
                           JDPEA='{15}',
                           JDINA='{16}',                           
                           JDIVO='{17}',
                           JDIVW='{18}',
                           JDLTA='{19}'                          
                           WHERE  JDIPB='{0}' AND JDALX='{1}'";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, string.Format(CultureInfo.InvariantCulture, sql, contrat.CodeContrat.ToIPB(), contrat.VersionContrat, contrat.Preavis,
                contrat.IndiceReference, contrat.Valeur, contrat.IntercalaireExiste, contrat.SoumisCatNat,
                contrat.DateDebutDernierePeriodeJour, contrat.DateDebutDernierePeriodeMois, contrat.DateDebutDernierePeriodeAnnee,
                contrat.DateFinDernierePeriodeJour, contrat.DateFinDernierePeriodeMois, contrat.DateFinDernierePeriodeAnnee,
                contrat.ProchaineEchJour, contrat.ProchaineEchMois, contrat.ProchaineEchAnnee,
                string.IsNullOrEmpty(contrat.IndiceReference) ? "N" : "O", contrat.Valeur, 0, contrat.LTA));

            if (contrat.PeriodiciteCode == "U" || contrat.PeriodiciteCode == "E")
            {
                EacParameter[] param = new EacParameter[3];
                param[0] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                param[0].Value = contrat.CodeContrat.ToIPB();
                param[1] = new EacParameter("P_VERSION", DbType.Int64);
                param[1].Value = contrat.VersionContrat;
                param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[2].Value = contrat.Type;

                sql = @"SELECT PBEFJ DATEEFFETGARANTIEJOUR, PBEFM DATEEFFETGARANTIEMOIS, PBEFA DATEEFFETGARANTIEANNEE,
                                            PBFEJ FINEFFETGARANTIEJOUR, PBFEM FINEFFETGARANTIEMOIS, PBFEA FINEFFETGARANTIEANNEE
                                    FROM YPOBASE WHERE PBIPB = :P_CODECONTRAT AND PBALX = :P_VERSION AND PBTYP = :P_TYPE";
                var result = DbBase.Settings.ExecuteList<OffrePlatDto>(CommandType.Text, sql, param);
                if (result != null && result.Any())
                {
                    var firstRes = result.FirstOrDefault();

                    param = new EacParameter[8];
                    param[0] = new EacParameter("P_DEBDAY", DbType.Int16);
                    param[0].Value = firstRes.DateEffetGarantieJour;
                    param[1] = new EacParameter("P_DEBMONTH", DbType.Int16);
                    param[1].Value = firstRes.DateEffetGarantieMois;
                    param[2] = new EacParameter("P_DEBYEAR", DbType.Int16);
                    param[2].Value = firstRes.DateEffetGarantieAnnee;
                    param[3] = new EacParameter("P_FINDAY", DbType.Int16);
                    param[3].Value = firstRes.DateFinEffetGarantieJour;
                    param[4] = new EacParameter("P_FINMONTH", DbType.Int16);
                    param[4].Value = firstRes.DateFinEffetGarantieMois;
                    param[5] = new EacParameter("P_FINYEAR", DbType.Int16);
                    param[5].Value = firstRes.DateFinEffetGarantieAnnee;
                    param[6] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                    param[6].Value = contrat.CodeContrat.ToIPB();
                    param[7] = new EacParameter("P_VERSION", DbType.Int64);
                    param[7].Value = contrat.VersionContrat;

                    sql = @"UPDATE YPRTENT SET JDDPJ = :P_DEBDAY, JDDPM = :P_DEBMONTH, JDDPA = :P_DEBYEAR, JDFPJ = :P_FINDAY, JDFPM = :P_FINMONTH, JDFPA = :P_FINYEAR
                                WHERE JDIPB = :P_CODECONTRAT AND JDALX = :P_VERSION";

                    //, firstRes.DateEffetGarantieJour, firstRes.DateEffetGarantieMois, firstRes.DateEffetGarantieAnnee
                    //, firstRes.DateFinEffetGarantieJour, firstRes.DateFinEffetGarantieMois, firstRes.DateFinEffetGarantieAnnee
                    //, contrat.CodeContrat, contrat.VersionContrat);

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                }
            }

        }

        private static void InfoGeneralesContratModifierYprtObj(ContratDto contrat)
        {
            List<EacParameter> param = new List<EacParameter>();
            param.Add(new EacParameter("p0", DbType.Decimal) { Value = contrat.Valeur });
            param.Add(new EacParameter("p1", DbType.Decimal) { Value = contrat.Valeur });
            param.Add(new EacParameter("p2", DbType.Int32) { Value = 0 });
            param.Add(new EacParameter("p3", DbType.AnsiStringFixedLength) { Value = contrat.CodeContrat.ToIPB() });
            param.Add(new EacParameter("p4", DbType.Int64) { Value = contrat.VersionContrat });

            string sql = @"UPDATE YPRTOBJ
                                SET JGIVO = :p0, JGIVA = :p1, JGIVW = :p2
                           WHERE JGIPB = :p3 AND JGALX = :p4";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static void InfoGeneralesContratModifierKpopt(ContratDto contrat)
        {
            List<EacParameter> param = new List<EacParameter>();
            param.Add(new EacParameter("p0", DbType.Decimal) { Value = contrat.Valeur });
            param.Add(new EacParameter("p1", DbType.Decimal) { Value = contrat.Valeur });
            param.Add(new EacParameter("p2", DbType.Int32) { Value = 0 });
            param.Add(new EacParameter("p3", DbType.AnsiStringFixedLength) { Value = contrat.CodeContrat.ToIPB() });
            param.Add(new EacParameter("p4", DbType.AnsiStringFixedLength) { Value = contrat.Type });
            param.Add(new EacParameter("p5", DbType.Int64) { Value = contrat.VersionContrat });

            string sql = @"UPDATE KPOPT
                                SET KDBIVO = :p0, KDBIVA = :p1, KDBIVW = :p2
                            WHERE KDBIPB = :p3 AND KDBTYP = :p4 AND KDBALX = :p5";


            //EacParameter[] param = new EacParameter[6];
            //param[0] = new EacParameter("P_IVO", DbType.AnsiStringFixedLength);
            //param[0].Value = contrat.Valeur.ToString().Replace(",", ".");
            //param[1] = new EacParameter("P_IVA", DbType.AnsiStringFixedLength);
            //param[1].Value = contrat.Valeur.ToString().Replace(",", ".");
            //param[2] = new EacParameter("P_IVW", DbType.Int32);
            //param[2].Value = 0;
            //param[3] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            //param[3].Value = contrat.CodeContrat.ToIPB();
            //param[4] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            //param[4].Value = contrat.Type;
            //param[5] = new EacParameter("P_VERSION", DbType.Int64);
            //param[5].Value = contrat.VersionContrat;

            //string sql = @"UPDATE KPOPT
            //                             SET KDBIVO = :P_IVO,
            //                             KDBIVA = :P_IVA,
            //                             KDBIVW = :P_IVW
            //                             WHERE KDBIPB = :P_CODECONTRAT AND KDBTYP = :P_TYPE AND KDBALX = :P_VERSION";
            //contrat.Valeur.ToString().Replace(",", "."), contrat.Valeur.ToString().Replace(",", "."), 0, contrat.CodeContrat, contrat.Type, contrat.VersionContrat);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static void InfoGeneralesContratModifierTpocoas(ContratDto contrat)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[0].Value = contrat.CodeContrat.ToIPB();
            param[1] = new EacParameter("P_VERSION", DbType.Int64);
            param[1].Value = contrat.VersionContrat;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = contrat.Type;

            string delSql = string.Empty;
            switch (contrat.NatureContrat)
            {
                case "A":
                case "E":
                    delSql = @"DELETE FROM YPOCOAS WHERE PHIPB = :P_CODECONTRAT AND PHALX = :P_VERSION AND PHTYP = :P_TYPE AND PHTAP = 'A'";
                    //contrat.CodeContrat.ToIPB(), contrat.VersionContrat, contrat.Type);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, delSql, param);
                    break;
                case "C":
                case "D":
                    delSql = @"DELETE FROM YPOCOAS WHERE PHIPB = :P_CODECONTRAT AND PHALX = :P_VERSION AND PHTYP = :P_TYPE";
                    //contrat.CodeContrat.ToIPB(), contrat.VersionContrat, contrat.Type);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, delSql, param);
                    float fraisAperition = contrat.FraisAperition.HasValue ? contrat.FraisAperition.Value : 0;
                    string insSql = string.Format(CultureInfo.InvariantCulture, @"INSERT INTO YPOCOAS (PHIPB, PHALX, PHTYP, PHTAP, PHCIE,PHTXF,
                                                                                                        PHAPP, PHIN5, PHPOL, PHAFR, PHCOM )
                                                        VALUES
                                                    ('{0}', {1}, '{2}', 'A', '{3}', {4}, {5}, {6}, '{7}', {8}, {9})",
                                            contrat.CodeContrat.ToIPB(), contrat.VersionContrat, contrat.Type, contrat.AperiteurCode, fraisAperition,
                                            contrat.PartAperiteur, contrat.IdInterlocuteurAperiteur, contrat.ReferenceAperiteur, contrat.FraisAccAperiteur, contrat.CommissionAperiteur);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, insSql);
                    break;
                default:
                    delSql = @"DELETE FROM YPOCOAS WHERE PHIPB = :P_CODECONTRAT AND PHALX = :P_VERSION AND PHTYP = :P_TYPE";
                    //contrat.CodeContrat.ToIPB(), contrat.VersionContrat, contrat.Type);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, delSql, param);
                    break;
            }

            //            string sql = @"UPDATE YPOCOAS 
            //                           SET PHTXF ='{2}',
            //                           PHCIE='{3}'                         
            //                           WHERE  PHIPB='{0}' AND PHALX='{1}' AND PHTYP='P'";
            //            float fraisAperition = contrat.FraisAperition.HasValue ? contrat.FraisAperition.Value : 0;
            //            DbBase.Settings.ExecuteNonQuery(CommandType.Text, string.Format(sql, contrat.CodeContrat, contrat.VersionContrat, fraisAperition, contrat.AperiteurCode));
        }
        private static void InfoGeneralesContratModifierKpent(ContratDto contrat)
        {
            string sql = @"UPDATE KPENT 
                           SET KAADSTA ='{3}'                                                   
                           WHERE  KAAIPB='{0}' AND KAAALX='{1}' AND KAATYP='{2}'";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, string.Format(sql, contrat.CodeContrat.ToIPB(), contrat.VersionContrat, contrat.Type, contrat.DateStatistique));
        }

        //        private static void SupprimerCourtierYpocour(ContratInfoBaseDto contrat)
        //        {
        //            string sql = string.Format(@"DELETE FROM YPOCOUR
        //                        WHERE PFIPB = '{0}' AND PFALX = {1} AND PFTYP = '{2}'",
        //                       contrat.CodeContrat, contrat.VersionContrat, contrat.Type);
        //            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        //        }

        //        private static void UpdateBrancheCibleTypePoliceContrat(ContratInfoBaseDto contrat, string utilisateur, bool updateMode)
        //        {
        //            string sql = string.Empty;
        //            if (updateMode)
        //            {
        //                sql = @"UPDATE KPOFENT 
        //                           SET KFHCIBLE='{2}',KFHTYPO='{3}'
        //                           WHERE  KFHPOG='{0}' AND KFHALX='{1}'";
        //                //                sql = @"UPDATE KPRSQ 
        //                //                           SET KABCIBLE='{2}',KFHTYPO='{3}'
        //                //                           WHERE  KFHPOG='{0}' AND KFHALX='{1}'";
        //                DbBase.Settings.ExecuteNonQuery(CommandType.Text, string.Format(sql, contrat.CodeContrat, contrat.VersionContrat, contrat.Cible, contrat.TypePolice));
        //            }
        //            else
        //            {
        //                //TODO :Qu'est ce qu'on ajoute dans les colonnes
        //                sql = @"INSERT INTO KPOFENT(KFHPOG, KFHALG, KFHIPB, KFHALX, KFHNPO, KFHEFD, KFHSAD, KFHBRA, KFHCIBLE, KFHIPR, KFHALR, KFHTYPO, KFHIPM, KHFSIT, KFHSTU, KFHSTD)
        //                        VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')";

        //                string sqlFinal = string.Format(sql, contrat.CodeContrat, 0, 0, 0, 1, AlbConvert.ConvertDateToInt(new DateTime(contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour)).ToString(),
        //                    AlbConvert.ConvertDateToInt(new DateTime(contrat.DateAccordAnnee, contrat.DateAccordMois, contrat.DateAccordJour)).ToString(), contrat.Branche, contrat.Cible,
        //                    /*KFHIPR*/"         ", 0, contrat.TypePolice, contrat.CodeContrat, 'A', utilisateur, AlbConvert.ConvertDateToInt(DateTime.Now).ToString());
        //                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlFinal);
        //            }
        //        }
        //        private static void UpdateEncaissementContrat(ContratInfoBaseDto contrat, bool updateMode)
        //        {
        //            string sql = string.Empty;
        //            if (updateMode)
        //            {
        //                sql = @"UPDATE YPRTENT 
        //                           SET JDENC ='{2}'                          
        //                           WHERE  JDIPB='{0}' AND JDALX='{1}'";
        //                DbBase.Settings.ExecuteNonQuery(CommandType.Text, string.Format(sql, contrat.CodeContrat, contrat.VersionContrat, contrat.Encaissement));
        //            }
        //            else
        //            {
        //                sql = @"INSERT INTO YPRTENT (JDIPB,JDALX,JDDRQ,JDNBR,JDENC)
        //                           VALUES('{0}','{1}','{2}','{3}','{4}')";
        //                DbBase.Settings.ExecuteNonQuery(CommandType.Text, string.Format(sql, contrat.CodeContrat.ToIPB(), 0, 1, 1, contrat.Encaissement));
        //            }
        //        }
        //        private static bool CheckOffreAssure(string codeContrat, long version, string type)
        //        {
        //            string sql = string.Format(@"SELECT COUNT(*) NBLIGNES FROM YPOASSU WHERE PCIPB='{0}'  
        //            AND PCALX='{1}' AND PCTYP='{2}'",
        //            codeContrat, version, type);
        //            if (!string.IsNullOrEmpty(DbBase.Settings.ExecuteScalar(CommandType.Text, sql).ToString()) &&
        //                 int.Parse(DbBase.Settings.ExecuteScalar(CommandType.Text, sql).ToString()) > 0)
        //                return true;
        //            return false;
        //        }
        //        private static bool CheckAssureBaseValide(string codeContrat, long version, string type, int preneurAssuranceBase)
        //        {
        //            string sql = string.Format(@"SELECT COUNT(*) NBLIGNES FROM YPOASSU WHERE PCIPB='{0}'  
        //            AND PCALX='{1}' AND PCTYP='{2}' AND PCIAS='{3}'",
        //                        codeContrat, version, type, preneurAssuranceBase);
        //            if (!string.IsNullOrEmpty(DbBase.Settings.ExecuteScalar(CommandType.Text, sql).ToString()) &&
        //                 int.Parse(DbBase.Settings.ExecuteScalar(CommandType.Text, sql).ToString()) > 0)
        //                return true;
        //            return false;
        //        }
        //        private static int GetAssureBase(string codeContrat, long version, string type)
        //        {
        //            string sql = string.Format(@"SELECT PCIAS FROM YPOASSU WHERE PCIPB='{0}'  
        //            AND PCALX='{1}' AND PCTYP='{2}'",
        //                      codeContrat, version, type);
        //            string codeAssureBase = DbBase.Settings.ExecuteScalar(CommandType.Text, sql).ToString();
        //            if (!string.IsNullOrEmpty(codeAssureBase))
        //                return int.Parse(codeAssureBase);
        //            return 0;

        //        }
        #region Risques
        //        private static void UpdateRisquePrincipal(ContratInfoBaseDto contrat, bool updateMode)
        //        {
        //            string sql = string.Empty;
        //            //contrat.RisquePrincipal.Adresse.NumeroChrono

        //            sql = @"UPDATE YPOBASE 
        //                           SET PBADH='{3}'                                             
        //                           WHERE  PBIPB='{0}' AND PBALX='{1}' AND PBTYP='{2}'";

        //            string sqlFinal = string.Format(sql, contrat.CodeContrat, contrat.VersionContrat, contrat.Type, contrat.AdresseContrat.NumeroChrono);
        //            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlFinal);
        //        }
        #endregion
        private static CreationAffaireNouvelleContratDto GetInfoOffre(string codeOffre, string version, string type)
        {
            CreationAffaireNouvelleContratDto model = new CreationAffaireNouvelleContratDto();

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.ToIPB();
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sql =
                @"SELECT PBBRA BRANCHE, TRIM(PBSOU) CONCAT ' - ' CONCAT TRIM(UTNOM) SOUSCRIPTEUR,(PBEFA * 10000 + PBEFM * 100 + PBEFJ) DATEEFFETDATE ,PBEFH HEUREEFFETINT
                            FROM YPOBASE 
                                LEFT JOIN YUTILIS ON PBSOU = UTIUT AND UTSOU ='O'
                            WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

            var result = DbBase.Settings.ExecuteList<CreationAffaireNouvelleContratDto>(CommandType.Text, sql, param);

            result.ForEach(i =>
            {
                i.DateEffet = AlbConvert.ConvertIntToDate((int?)i.DateEffetDate);
                i.HeureEffet = AlbConvert.ConvertIntToTimeMinute(i.HeureEffetInt);
            });

            return result.Count > 0 ? result.FirstOrDefault() : new CreationAffaireNouvelleContratDto();
        }
        //private static string GetBranche(string codeOffre, string version, string type)
        //{
        //    DbParameter[] param = new DbParameter[3];
        //    param[0] = new EacParameter("codeOffre", codeOffre.ToIPB());
        //    param[1] = new EacParameter("version", 0);
        //    param[1].Value = Convert.ToInt32(version);
        //    param[2] = new EacParameter("type", type);

        //    string sql = @"SELECT PBBRA BRANCHE FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

        //    var result = DbBase.Settings.ExecuteList<CreationAffaireNouvelleContratDto>(CommandType.Text, sql, param);


        //    return result.Count > 0 ? result.FirstOrDefault().Branche : string.Empty;
        //}
        private static List<ParametreDto> GetListBranches(string codeOffre, string version, string type)
        {
            string sql = string.Format(@"SELECT TCOD CODE, TPLIB LIBELLE 
                            FROM KPOPTD
                                INNER JOIN KVOLET ON KDCKAKID = KAKID
                                {3}
                            WHERE KDCIPB = '{0}' AND KDCALX = {1} AND KDCTYP = '{2}' AND KDCTENG = 'V'
                          UNION
                          SELECT TCOD CODE, TPLIB LIBELLE 
                            FROM YPOBASE
	                            {4}
                            WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'
                          ORDER BY CODE", codeOffre.ToIPB(), version, type,
                        CommonRepository.BuildJoinYYYYPAR("INNER", "GENER", "BRCHE", otherCriteria: " AND TCOD = KAKBRA AND TPCN2 = 1"),
                        CommonRepository.BuildJoinYYYYPAR("INNER", "GENER", "BRCHE", otherCriteria: " AND TCOD = PBBRA AND TPCN2 = 1"));

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }
        //        private static bool ExisteContrat(string id, long version)
        //        {
        //            if (string.IsNullOrEmpty(id)) return false;
        //            string sql = string.Format(@"SELECT PBIPB FROM YPOBASE                         
        //                            WHERE PBTYP = 'P' AND PBALX = '{0}' AND TRIM(PBIPB)= '{1}'", version, id);
        //            if (!string.IsNullOrEmpty(DbBase.Settings.ExecuteScalar(CommandType.Text, sql).ToString()))
        //                return true;
        //            else return false;
        //        }
        public static void UpdatePeriodicite(string codeOffre, string version, string type, string periodicite)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_PERIO", DbType.AnsiStringFixedLength);
            param[0].Value = periodicite;
            param[1] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[1].Value = codeOffre.ToIPB();
            param[2] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[2].Value = version;
            param[3] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[3].Value = type;

            string sql = @"UPDATE YPOBASE 
                          SET PBPER= :P_PERIO
                          WHERE  PBIPB=:P_CODECONTRAT AND PBALX=:P_VERSION AND PBTYP = :P_TYPE";
            //string sqlFinal = string.Format(CultureInfo.InvariantCulture, sql, codeOffre, version, type, periodicite);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        #endregion

        #endregion
        #region Ecran Choix Risque/Objet Affaire Nouvelle

        #region Méthodes Publiques

        /// <summary>
        /// Affiche les informations des risques/objets 
        /// de l'offre pour le nouveau contrat
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static RsqObjAffNouvDto InitRsqObjAffNouv(string codeOffre, string version, string type, string codeContrat, string versionContrat)
        {
            RsqObjAffNouvDto result = new RsqObjAffNouvDto
            {
                CodeOffre = codeOffre,
                Version = Convert.ToInt32(version),
                Type = type,
                CodeContrat = codeContrat,
                VersionContrat = Convert.ToInt32(versionContrat)
            };

            result.ListRsqObj = LoadListRsqObj(codeOffre, version, codeContrat, versionContrat);
            return result;
        }

        /// <summary>
        /// Met à jour le risque/objet dans la table KPOFRSQ
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="type"></param>
        /// <param name="codeRsq"></param>
        /// <param name="codeObj"></param>
        /// <param name="isChecked"></param>
        public static void UpdateRsqObj(string codeContrat, string versionContrat, string type, string codeRsq, string codeObj, string isChecked)
        {
            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[1] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[1].Value = Convert.ToInt32(versionContrat);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODERSQ", 0);
            param[3].Value = Convert.ToInt32(codeRsq);
            param[4] = new EacParameter("P_CODEOBJ", 0);
            param[4].Value = Convert.ToInt32(codeObj);
            param[5] = new EacParameter("P_CHECK", isChecked);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_COMAJ", param);
        }

        #endregion

        #region Méthodes Privées

        /// <summary>
        /// Charge la liste des risques/objets
        /// de l'offre
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <returns></returns>
        private static List<RsqObjAffNouvRowDto> LoadListRsqObj(string codeOffre, string version, string codeContrat, string versionContrat)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.ToIPB());
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[3] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[3].Value = Convert.ToInt32(versionContrat);

            return DbBase.Settings.ExecuteList<RsqObjAffNouvRowDto>(CommandType.StoredProcedure, "SP_CORQOB", param);
        }

        #endregion

        #endregion
        #region Ecran Choix Formule/Volet Affaire Nouvelle

        #region Méthodes Publiques

        /// <summary>
        /// Affiche les informations des formules/option/volets
        /// de l'offre pour le nouveau contrat en fonction des 
        /// risques sélectionnés
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <returns></returns>
        public static FormVolAffNouvDto InitFormVolAffNouv(string codeOffre, string version, string type, string codeContrat, string versionContrat)
        {
            FormVolAffNouvDto model = new FormVolAffNouvDto();
            model.CodeContrat = codeContrat;
            model.VersionContrat = Convert.ToInt32(versionContrat);
            model.Type = type;
            model.Risques = GetListRisques(codeOffre, version, type, codeContrat, versionContrat);

            return model;
        }

        /// <summary>
        /// Met à jour la formule/option/volet dans la table KPOFOPT
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="codeForm"></param>
        /// <param name="guidForm"></param>
        /// <param name="codeOpt"></param>
        /// <param name="guidOpt"></param>
        /// <param name="guidVol"></param>
        /// <param name="type"></param>
        /// <param name="isChecked"></param>
        public static void UpdateFormVol(string codeContrat, string versionContrat, string codeOffre, string version, string typeOffre, string codeForm, string guidForm, string codeOpt,
                string guidOpt, string guidVol, string type, string isChecked)
        {
            DbParameter[] param = new DbParameter[11];
            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[1] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[1].Value = Convert.ToInt32(versionContrat);
            param[2] = new EacParameter("P_CODEOFFRE", codeOffre.ToIPB());
            param[3] = new EacParameter("P_VERSION", 0);
            param[3].Value = Convert.ToInt32(version);
            param[4] = new EacParameter("P_CODEFORM", 0);
            param[4].Value = Convert.ToInt32(codeForm);
            param[5] = new EacParameter("P_GUIDFORM", 0);
            param[5].Value = Convert.ToInt32(guidForm);
            param[6] = new EacParameter("P_CODEOPT", 0);
            param[6].Value = Convert.ToInt32(codeOpt);
            param[7] = new EacParameter("P_GUIDOPT", 0);
            param[7].Value = Convert.ToInt32(guidOpt);
            param[8] = new EacParameter("P_GUIDVOL", 0);
            param[8].Value = Convert.ToInt32(guidVol);
            param[9] = new EacParameter("P_TYPEOPTION", type);
            param[10] = new EacParameter("P_CHECK", isChecked);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_MAJOPVO", param);
        }

        /// <summary>
        /// Récupération de la liste des risques et formules
        /// pour l'affaire nouvelle créée
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        public static FormVolAffNouvRecapDto GetListRsqForm(string codeContrat, string versionContrat)
        {
            FormVolAffNouvRecapDto toReturn = new FormVolAffNouvRecapDto();

            toReturn.CodeContrat = codeContrat;
            toReturn.VersionContrat = versionContrat;
            toReturn.Risques = GetListRisquesChecked(codeContrat, versionContrat);
            toReturn.Formules = GetListFormulesChecked(codeContrat, versionContrat);
            toReturn.CountGarTar = GetCountGarTarif(codeContrat, versionContrat);

            return toReturn;
        }

        #endregion

        #region Méthodes Privées

        /// <summary>
        /// Construction du modèle à partir de l'objet à plat
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <returns></returns>
        private static List<FormVolAffNouvRsqDto> GetListRisques(string codeOffre, string version, string type, string codeContrat, string versionContrat)
        {
            DbParameter[] param = new DbParameter[5];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.ToIPB());
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[4] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[4].Value = Convert.ToInt32(versionContrat);

            var result = DbBase.Settings.ExecuteList<FormVolAffNouvPlatDto>(CommandType.StoredProcedure, "SP_COOPVOL", param);

            //liste des risques
            var lstRsq = result.GroupBy(el => el.CodeRisque).Select(r => r.First()).ToList();
            //liste des formules
            var lstForm = result.GroupBy(el => el.CodeForm).Select(f => f.First()).ToList();
            //liste des options
            var lstOpt = result.GroupBy(el => new { el.CodeOpt, el.CodeForm }).Select(o => o.First()).ToList();
            //liste des volets
            var lstVol = result.GroupBy(el => new { el.CodeVolet, el.CodeOpt, el.CodeForm }).Select(v => v.First()).ToList();

            List<FormVolAffNouvRsqDto> listRisques = new List<FormVolAffNouvRsqDto>();

            lstRsq.ForEach(rsq =>
            {

                //préparer la liste des formules
                var listForm = new List<FormVolAffNouvFormDto>();
                var resForm = lstForm.FindAll(risque => risque.CodeRisque == rsq.CodeRisque);
                resForm.ForEach(form =>
                {
                    //préparer la liste des options
                    var listOpt = new List<FormVolAffNouvOptDto>();
                    var resOpt = lstOpt.FindAll(opt => opt.CodeRisque == form.CodeRisque && opt.GuidForm == form.GuidForm);
                    resOpt.ForEach(option =>
                    {
                        //préparer la liste des volets
                        var listVol = new List<FormVolAffNouvVolDto>();
                        var resVol = lstVol.FindAll(vol => vol.CodeRisque == option.CodeRisque && vol.GuidForm == option.GuidForm && vol.GuidOpt == option.GuidOpt);
                        resVol.ForEach(volet =>
                        {
                            listVol.Add(new FormVolAffNouvVolDto
                            {
                                CodeVolet = volet.CodeVolet,
                                GuidVolet = volet.GuidVolet,
                                DescVolet = volet.DescVolet,
                                CheckRow = volet.CheckRowDb == "O" ? true : false
                            });
                        });

                        listOpt.Add(new FormVolAffNouvOptDto
                        {
                            CodeOpt = option.CodeOpt,
                            GuidOpt = option.GuidOpt,
                            CheckRow = option.CheckRowDb == "O" ? true : false,
                            Volets = listVol
                        });
                    });

                    listForm.Add(new FormVolAffNouvFormDto
                    {
                        CodeForm = form.CodeForm,
                        GuidForm = form.GuidForm,
                        LettreForm = form.LettreForm,
                        DescFormule = form.DescFormule,
                        CheckRow = form.CheckRowDb == "O" ? true : false,
                        Options = listOpt
                    });
                });


                listRisques.Add(new FormVolAffNouvRsqDto
                {
                    CodeRisque = rsq.CodeRisque,
                    DescRisque = rsq.DescRisque,
                    Formules = listForm
                });
            });

            return listRisques;
        }

        /// <summary>
        /// Récupération de la liste des risques
        /// de l'affaire nouvelle créée
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        private static List<FormVolAffNouvRsqDto> GetListRisquesChecked(string codeContrat, string versionContrat)
        {
            List<FormVolAffNouvRsqDto> toReturn = new List<FormVolAffNouvRsqDto>();

            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.ToIPB();
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = Convert.ToInt32(versionContrat);
            param[2] = new EacParameter("typeEnr", DbType.AnsiStringFixedLength);
            param[2].Value = "R";
            param[3] = new EacParameter("selEnr", DbType.AnsiStringFixedLength);
            param[3].Value = "O";

            string sql = @"SELECT KFIRSQ CODERISQUE 
                    FROM KPOFRSQ 
                        WHERE KFIPOG = :codeContrat AND KFIALG = :version AND KFITYE = :typeEnr AND KFISEL = :selEnr
                    ORDER BY KFICHR";

            var result = DbBase.Settings.ExecuteList<FormVolAffNouvPlatDto>(CommandType.Text, sql, param);

            if (result.Count > 0)
            {
                //liste des risques
                var lstRsq = result.GroupBy(el => el.CodeRisque).Select(r => r.First()).ToList();

                lstRsq.ForEach(rsq =>
                {
                    toReturn.Add(new FormVolAffNouvRsqDto { CodeRisque = rsq.CodeRisque });
                });
            }

            return toReturn;
        }

        /// <summary>
        /// Récupération de la liste des formules
        /// options de l'affaire nouvelle créée
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <returns></returns>
        private static List<FormVolAffNouvFormDto> GetListFormulesChecked(string codeContrat, string versionContrat)
        {
            List<FormVolAffNouvFormDto> toReturn = new List<FormVolAffNouvFormDto>();

            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.ToIPB();
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(versionContrat);
            param[2] = new EacParameter("typeEnr", DbType.AnsiStringFixedLength);
            param[2].Value = "O";
            param[3] = new EacParameter("selEnr", DbType.AnsiStringFixedLength);
            param[3].Value = "O";

            string sql = @"SELECT KFJFOR CODEFORM, KFJOPT CODEOPT, KDAALPHA LETTREFORM 
                    FROM KPOFOPT 
                        INNER JOIN KPFOR ON KDAIPB = KFJIPB AND KDAALX = KFJALX AND KDAFOR = KFJFOR 
                        WHERE KFJPOG = :codeContrat AND KFJALG = :version AND KFJTENG = :typeEnr AND KFJSEL = :selEnr 
                    ORDER BY KFJCHR";

            var result = DbBase.Settings.ExecuteList<FormVolAffNouvPlatDto>(CommandType.Text, sql, param);

            if (result.Count > 0)
            {
                //liste des formules
                var lstForm = result.GroupBy(el => el.CodeForm).Select(f => f.First()).ToList();
                //liste des options
                var lstOpt = result.GroupBy(el => el.CodeOpt).Select(o => o.First()).ToList();

                lstForm.ForEach(form =>
                {
                    //préparer la liste des options
                    var listOpt = new List<FormVolAffNouvOptDto>();
                    var resOpt = lstOpt.FindAll(opt => opt.CodeRisque == form.CodeRisque);
                    resOpt.ForEach(option =>
                    {
                        listOpt.Add(new FormVolAffNouvOptDto
                        {
                            CodeOpt = option.CodeOpt
                        });
                    });

                    toReturn.Add(new FormVolAffNouvFormDto
                    {
                        CodeForm = form.CodeForm,
                        LettreForm = form.LettreForm,
                        Options = listOpt
                    });
                });
            }

            return toReturn;
        }

        /// <summary>
        /// Récupération du nombre de garantie sélectionnée
        /// qui comporte plus d'un tarif
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <returns></returns>
        private static Int64 GetCountGarTarif(string codeContrat, string versionContrat)
        {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.ToIPB();
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(versionContrat);

            string sql = @"SELECT COUNT(*) NBLIGN
                    FROM KPOFOPT
                        INNER JOIN KPGARAN ON KFJIPB = KDEIPB AND KFJALX = KDEALX AND KDETYP = 'O' AND KFJFOR = KDEFOR AND KFJOPT = KDEOPT
                        INNER JOIN KPGARTAR ON KDEID = KDGKDEID
                    WHERE KFJPOG = :codeContrat AND KFJALG = :version AND KFJTENG = 'V' AND KFJSEL = 'O' AND EXISTS( SELECT KDGID FROM KPGARTAR WHERE KDGNUMTAR > 1 AND KDGIPB = KDEIPB AND KDGALX = KDEALX AND KDGTYP = KDETYP)";

            return DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault().NbLigne;
        }

        #endregion

        #endregion
        #region Ecran Choix Options tarif

        #region Méthodes Publiques

        /// <summary>
        /// Affiche les informations des tarifs 
        /// de l'offre pour le nouveau contrat en fonctions des
        /// formules sélectionnées
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <returns></returns>
        public static OptTarAffNouvDto InitOptTarifAffNouv(string codeContrat, string versionContrat)
        {
            OptTarAffNouvDto model = new OptTarAffNouvDto();
            model.CodeContrat = codeContrat;
            model.VersionContrat = Convert.ToInt32(versionContrat);
            model.Garanties = GetListGaranties(codeContrat, versionContrat);

            return model;
        }

        /// <summary>
        /// Met à jour le tarif sélectionné
        /// dans la table KPOFTAR
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="guidTarif"></param>
        public static void UpdateOptTarif(string codeContrat, string versionContrat, string guidTarif)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[1] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[1].Value = Convert.ToInt32(versionContrat);
            param[2] = new EacParameter("P_CODETARIF", 0);
            param[2].Value = Convert.ToInt32(guidTarif);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_MAJTAR", param);
        }

        /// <summary>
        /// Sauvegarde le contrat créé
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="user"></param>
        public static void ValidContrat(string codeOffre, string version, string type, string codeContrat, string versionContrat, string user, string splitChar, bool isModifHorsAvn, string acteGestion)
        {
            EacParameter[] param = new EacParameter[8];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.ToIPB());
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[4] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[4].Value = Convert.ToInt32(versionContrat);
            param[5] = new EacParameter("P_DATESYSTEME", AlbConvert.ConvertDateToInt(DateTime.Now).ToString());
            param[6] = new EacParameter("P_USER", user);
            param[7] = new EacParameter("P_TRAITEMENT", AlbConstantesMetiers.Traitement.Police.AsCode());

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_AFFNOUV", param);

            CommonRepository.ReloadEngagement(codeContrat, versionContrat, AlbConstantesMetiers.Traitement.Police.AsCode(), codeOffre, version, type, user, acteGestion);

            //Copy des documents 
            CopieDocRepository.CopierDocuments(codeContrat, versionContrat, AlbConstantesMetiers.TYPE_CONTRAT, "0");

            #region Recalcul de l'échéance principale
            param = new EacParameter[3];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.ToIPB();
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = Convert.ToInt32(versionContrat);
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = "P";

            string sql = @"SELECT PBPER PERIODICITE, 
                                            RIGHT(REPEAT('0', 2) CONCAT PBEFJ, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 2) CONCAT PBEFM, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 4) CONCAT PBEFA, 4) EFFETGARANTIE,
                                            RIGHT(REPEAT('0', 2) CONCAT PBFEJ, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 2) CONCAT PBFEM, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 4) CONCAT PBFEA, 4) FINGARANTIE,
                                            RIGHT(REPEAT('0', 2) CONCAT PBECJ, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 2) CONCAT PBECM, 2) ECHPRINC,
                                            PBCTD DUREE, PBCTU DUREEUNITE
                                        FROM YPOBASE WHERE PBIPB = :P_CODEOFFRE AND PBALX = :P_VERSION AND PBTYP = :P_TYPE";
            //codeContrat, versionContrat, "P");

            var result = DbBase.Settings.ExecuteList<AffNouvProchaineEcheanceDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                var firstRes = result.FirstOrDefault();

                if (firstRes.Periodicite == "A")
                {
                    var ddEffet = AlbConvert.ConvertStrToDate(firstRes.EffetGarantie);
                    var dfEffet = AlbConvert.ConvertStrToDate(firstRes.FinGarantie);
                    var dAvenant = AlbConvert.ConvertStrToDate(firstRes.EffetGarantie);
                    DateTime? dEcheance = null;
                    if (!string.IsNullOrEmpty(firstRes.EcheancePrincipale))
                        dEcheance = AlbConvert.ConvertStrToDate(firstRes.EcheancePrincipale + "/2012");

                    if (!string.IsNullOrEmpty(firstRes.DureeUnite))
                        dfEffet = AlbConvert.GetFinPeriode(AlbConvert.ConvertStrToDate(firstRes.EffetGarantie), firstRes.Duree, firstRes.DureeUnite);
                    string dataProchEch = string.Empty;
                    if (dEcheance != null)
                        dataProchEch = CommonRepository.LoadPreavisResiliation(codeContrat, versionContrat, string.Empty, ddEffet, dfEffet, dAvenant, firstRes.Periodicite, dEcheance, splitChar, user, acteGestion);
                    if (!string.IsNullOrEmpty(dataProchEch))
                    {
                        string[] tDataProchEch = dataProchEch.Split(new[] { splitChar }, StringSplitOptions.None);
                        var prochEch = AlbConvert.ConvertStrToDate(tDataProchEch[2]);
                        var periodeDeb = AlbConvert.ConvertStrToDate(tDataProchEch[0]);
                        var periodeFin = AlbConvert.ConvertStrToDate(tDataProchEch[1]);

                        param = new EacParameter[11];
                        param[0] = new EacParameter("P_PROECHDAY", DbType.Int32);
                        param[0].Value = prochEch.HasValue ? prochEch.Value.Day : 0;
                        param[1] = new EacParameter("P_PROECHMONTH", DbType.Int32);
                        param[1].Value = prochEch.HasValue ? prochEch.Value.Month : 0;
                        param[2] = new EacParameter("P_PROECHYEAR", DbType.Int32);
                        param[2].Value = prochEch.HasValue ? prochEch.Value.Year : 0;
                        param[3] = new EacParameter("P_PDEBDAY", DbType.Int32);
                        param[3].Value = periodeDeb.HasValue ? periodeDeb.Value.Day : 0;
                        param[4] = new EacParameter("P_PDEBMONTH", DbType.Int32);
                        param[4].Value = periodeDeb.HasValue ? periodeDeb.Value.Month : 0;
                        param[5] = new EacParameter("P_PDEBYEAR", DbType.Int32);
                        param[5].Value = periodeDeb.HasValue ? periodeDeb.Value.Year : 0;
                        param[6] = new EacParameter("P_PFINDAY", DbType.Int32);
                        param[6].Value = periodeFin.HasValue ? periodeFin.Value.Day : 0;
                        param[7] = new EacParameter("P_PFINMONTH", DbType.Int32);
                        param[7].Value = periodeFin.HasValue ? periodeFin.Value.Month : 0;
                        param[8] = new EacParameter("P_PFINYEAR", DbType.Int32);
                        param[8].Value = periodeFin.HasValue ? periodeFin.Value.Year : 0;
                        param[9] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                        param[9].Value = codeContrat.ToIPB();
                        param[10] = new EacParameter("P_VERSION", DbType.Int32);
                        param[10].Value = Convert.ToInt32(versionContrat);

                        sql = @"UPDATE YPRTENT SET JDPEJ = :P_PROECHDAY, JDPEM = :P_PROECHMONTH, JDPEA = :P_PROECHYEAR,
                                        JDDPJ = :P_PDEBDAY, JDDPM = :P_PDEBMONTH, JDDPA = :P_PDEBYEAR, JDFPJ = :P_PFINDAY, JDFPM = :P_PFINMONTH, JDFPA = :P_PFINYEAR
                            WHERE JDIPB = :P_CODEOFFRE AND JDALX = :P_VERSION";

                        //prochEch.HasValue ? prochEch.Value.Day : 0, 
                        //prochEch.HasValue ? prochEch.Value.Month : 0, 
                        //prochEch.HasValue ? prochEch.Value.Year : 0,
                        //periodeDeb.HasValue ? periodeDeb.Value.Day : 0, 
                        //periodeDeb.HasValue ? periodeDeb.Value.Month : 0, 
                        //periodeDeb.HasValue ? periodeDeb.Value.Year : 0,
                        //periodeFin.HasValue ? periodeFin.Value.Day : 0, 
                        //periodeFin.HasValue ? periodeFin.Value.Month : 0, 
                        //periodeFin.HasValue ? periodeFin.Value.Year : 0,
                        //codeContrat, 
                        //versionContrat);

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                    }
                }
            }


            #endregion

            CommonRepository.LancementCalculAffNouv(codeContrat, versionContrat, AlbConstantesMetiers.TYPE_CONTRAT);

            #region Ajout de l'acte de gestion
            if (!isModifHorsAvn)
            {
                string libTrace = $"Création via {codeOffre}-{version}";
                CommonRepository.AjouterActeGestion(codeContrat, version, AlbConstantesMetiers.TYPE_CONTRAT, 0, AlbConstantesMetiers.ACTEGESTION_GESTION, AlbConstantesMetiers.TRAITEMENT_AFFNV, libTrace, user);
            }
            #endregion
        }

        #endregion

        #region Méthodes Privées

        private static List<OptTarAffNouvGaranDto> GetListGaranties(string codeContrat, string versionContrat)
        {
            DbParameter[] param = new DbParameter[2];
            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.ToIPB());
            param[1] = new EacParameter("P_VERSIONCONTRAT", 0);
            param[1].Value = Convert.ToInt32(versionContrat);

            var result = DbBase.Settings.ExecuteList<OptTarAffNouvPlatDto>(CommandType.StoredProcedure, "SP_COTARIF", param);

            //liste des garanties
            var lstGaran = result.GroupBy(el => el.CodeGaran).Select(g => g.First()).ToList();
            //liste des tarifs
            var lstTarif = result.GroupBy(el => new { el.CodeGaran, el.CodeTarif }).Select(t => t.First()).ToList();

            List<OptTarAffNouvGaranDto> listGaran = new List<OptTarAffNouvGaranDto>();
            lstGaran.ForEach(garan =>
            {
                //préparter la liste des tarifs
                var listTar = new List<OptTarAffNouvTarifDto>();
                var resTar = lstTarif.FindAll(tar => tar.CodeForm == garan.CodeForm && tar.CodeGaran == garan.CodeGaran);
                resTar.ForEach(tarif =>
                {
                    listTar.Add(new OptTarAffNouvTarifDto
                    {
                        CodeTarif = tarif.CodeTarif,
                        GuidTarif = tarif.GuidTarif,
                        LCIVal = tarif.LCIVal,
                        LCIUnit = tarif.LCIUnit,
                        LCIType = tarif.LCIType,
                        IdLCICpx = tarif.IdLCICpx,
                        FRHVal = tarif.FRHVal,
                        FRHUnit = tarif.FRHUnit,
                        FRHType = tarif.FRHType,
                        IdFRHCpx = tarif.IdFRHCpx,
                        ASSVal = tarif.ASSVal,
                        ASSUnit = tarif.ASSUnit,
                        ASSType = tarif.ASSType,
                        PRIVal = tarif.PRIVal,
                        PRIUnit = tarif.PRIUnit,
                        PRIType = tarif.PRIType,
                        PRIMPro = tarif.PRIMPro,
                        CheckRow = tarif.CheckRowDb == "O" ? true : false
                    });
                });
                listGaran.Add(new OptTarAffNouvGaranDto
                {
                    CodeForm = garan.CodeForm,
                    CodeGaran = garan.CodeGaran,
                    DescGaran = garan.DescGaran,
                    LettreForm = garan.LettreForm,
                    Tarifs = listTar
                });
            });

            return listGaran;
        }

        #endregion

        #endregion
        #region coourtier
        public static string UpdateCourtier(string codeContrat, string versionContrat, string type, string typeCourtier, int identifiantCourtier, Single partCommission, string typeOperation)
        {
            //            string sql = string.Format(@"INSERT INTO YPOCOUR(PFTYP,PFIPB,PFALX,PFCTI,PFICT,PFSAA,PFSAM,PFSAJ,PFSAH,PFSIT,PFSTA,PFSTM,PFSTJ,
            //                         PFCOM,PFXCM,PFXCN)
            //                        VALUES('P','{0}','{1}','{2}','{3}','0','0','0','0','A','{4}','{5}','{6}','{7}','0','0')",
            //                           codeContrat, versionContrat, typeCourtier, identifiantCourtier, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, partCommission);

            //            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            //            return 0;
            int idAliment;
            if (!int.TryParse(versionContrat, out idAliment))
                idAliment = 0;

            DbParameter[] param = new DbParameter[17];
            param[0] = new EacParameter("P_ID_OFFRE", codeContrat.ToIPB());
            param[1] = new EacParameter("P_TYPE_OFFRE", type);
            param[2] = new EacParameter("P_ID_ALIMENT", idAliment);

            param[3] = new EacParameter("P_TYPE_COURTIER", typeCourtier);
            param[4] = new EacParameter("P_ID_COURTIER", identifiantCourtier);
            param[5] = new EacParameter("P_COMMISSION", partCommission);
            param[6] = new EacParameter("P_MODE", typeOperation);

            param[7] = new EacParameter("P_ERREUR", "");
            param[7].Direction = ParameterDirection.InputOutput;
            param[7].Size = 256;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDCORT", param);
            return param[7].Value.ToString();
        }
        public static void SupprimerCourtier(string codeContrat, string versionContrat, int identifiantCourtier)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_IDCOURTI", DbType.Int32);
            param[0].Value = identifiantCourtier;
            param[1] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[1].Value = codeContrat.ToIPB();
            param[2] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[2].Value = versionContrat;
            param[3] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[3].Value = "P";

            string sql = @"DELETE FROM YPOCOUR
                        WHERE PFICT= :P_IDCOURTI AND PFIPB = :P_CODECONTRAT AND PFALX = :P_VERSION AND PFTYP = :P_TYPE";
            //identifiantCourtier, codeContrat.ToIPB(), versionContrat, 'P');
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        public static void ModifierCommissionCourtier(string codeContrat, string versionContrat, int identifiantCourtier, Single commission)
        {
            EacParameter[] param = new EacParameter[5];
            param[0] = new EacParameter("P_COM", DbType.Single);
            param[0].Value = commission;
            param[1] = new EacParameter("P_IDCOURTI", DbType.Int32);
            param[1].Value = identifiantCourtier;
            param[2] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[2].Value = codeContrat.ToIPB();
            param[3] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[3].Value = versionContrat;
            param[4] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[4].Value = "P";

            string sql = @"UPDATE YPOCOUR
                        SET PFCOM= :P_COM
                        WHERE PFICT= :P_IDCOURTI AND PFIPB = :P_CODECONTRAT AND PFALX = :P_VERSION AND PFTYP = :P_TYPE";
            //identifiantCourtier, codeContrat.ToIPB(), versionContrat, 'P', commission);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        public static CommissionCourtierDto GetCommissionsStandardCourtier(string codeContrat, string versionContrat, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion)
        {
            CommissionCourtierDto toReturn = new CommissionCourtierDto();
            if (modeNavig == ModeConsultation.Standard)
            {
                toReturn = CommonRepository.LoadAS400Commissions(codeContrat, versionContrat, type, codeAvn, user, acteGestion);
            }
            string sql = string.Format(@"SELECT JDXCM, JDCNC, KAAXCMS, KAACNCS, KAJOBSV
                                            FROM {0}
                                                LEFT JOIN {1} ON JDIPB = KAAIPB AND JDALX = KAAALX {6}
                                                LEFT JOIN {2} ON KAJCHR= KAAOBSC                                        
                                            WHERE JDIPB='{3}' AND JDALX={4} {5}",
                                        CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                                        CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                                        CommonRepository.GetPrefixeHisto(modeNavig, "KPOBSV"),
                                        codeContrat.Trim().PadLeft(9, ' '), versionContrat,
                                        modeNavig == ModeConsultation.Historique ? string.Format(" AND JDAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                        modeNavig == ModeConsultation.Historique ? " AND JDAVN = KAAAVN" : string.Empty);

            var lstCommission = DbBase.Settings.ExecuteList<CommissionCourtierDto>(CommandType.Text, sql).FirstOrDefault();
            if (lstCommission != null)
            {
                toReturn.Commentaires = (string.IsNullOrEmpty(lstCommission.Commentaires) ? string.Empty : lstCommission.Commentaires.Trim());
                toReturn.IsStandardCAT = (string.IsNullOrEmpty(lstCommission.IsStandardCAT) && !isReadonly ? "O" : lstCommission.IsStandardCAT);
                toReturn.IsStandardHCAT = (string.IsNullOrEmpty(lstCommission.IsStandardHCAT) && !isReadonly ? "O" : lstCommission.IsStandardHCAT);
                toReturn.TauxContratCAT = string.IsNullOrEmpty(lstCommission.IsStandardCAT) && !isReadonly ? toReturn.TauxStandardCAT : lstCommission.TauxContratCAT;
                toReturn.TauxContratHCAT = string.IsNullOrEmpty(lstCommission.IsStandardHCAT) && !isReadonly ? toReturn.TauxStandardHCAT : lstCommission.TauxContratHCAT;
            }

            #region Récupération Commission Apérition

            sql = string.Format(@"SELECT PHCOM DECRETURNCOL
                                    FROM {0} 
                                            INNER JOIN {1} ON PHIPB = PBIPB AND PHALX = PBALX AND PHTYP = PBTYP AND PHTAP = 'A' {6}
                                    WHERE PBIPB = '{2}' AND PBALX = {3} AND PBTYP = '{4}' AND (PBNPL='C' OR PBNPL='D') {5}",
                                    CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "YPOCOAS"),
                                    codeContrat, versionContrat, type,
                                    modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                    modeNavig == ModeConsultation.Historique ? " AND PBAVN = PHAVN" : string.Empty);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            toReturn.CommissionAperition = 0;
            if (result != null && result.Any())
                toReturn.CommissionAperition = result.FirstOrDefault().DecReturnCol;

            #endregion

            #region Vérification des échéances

            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(versionContrat) ? Convert.ToInt32(versionContrat) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODEAVN", 0);
            param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
            param[4] = new EacParameter("P_DATENOW", 0);
            param[4].Value = AlbConvert.ConvertDateToInt(DateTime.Now);
            param[5] = new EacParameter("P_ERROR", 0);
            param[5].Value = 0;
            param[5].Direction = ParameterDirection.InputOutput;
            param[5].DbType = DbType.Int32;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CHECKECH", param);

            toReturn.EchError = Convert.ToInt32(param[5].Value.ToString());

            #endregion

            toReturn.IsTraceAvnExist = CommonRepository.ExistTraceAvenant(codeContrat, versionContrat, type, "KCOMM", string.Empty, string.Empty, string.Empty, string.Empty, "GEN");
            return toReturn;
        }
        public static void UpdateCommissionsStandardCourtier(string codeContrat, string versionContrat, string type, CommissionCourtierDto commissionStandard)
        {
            DbParameter[] param = new DbParameter[8];
            param[0] = new EacParameter("P_ID_OFFRE", codeContrat.ToIPB());
            param[1] = new EacParameter("P_VERSION_OFFRE", versionContrat);
            param[2] = new EacParameter("P_TYPE_OFFRE", type);
            param[3] = new EacParameter("P_TAUX_CONTRAT_HCAT", commissionStandard.TauxContratHCAT);
            param[4] = new EacParameter("P_TAUX_CONTRAT_CAT", commissionStandard.TauxContratCAT);
            param[5] = new EacParameter("P_IS_HORSCATNAT", commissionStandard.IsStandardHCAT);
            param[6] = new EacParameter("P_IS_CATNAT", commissionStandard.IsStandardCAT);
            param[7] = new EacParameter("P_OBSV", commissionStandard.Commentaires.Trim());

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDCOMM", param);
        }
        #endregion
        #region coassureur

        #region méthodes publiques

        public static ContratDto GetInfoRegulPage(string codeOffre, string version, string type, string codeAvn)
        {
            ContratDto contrat = new ContratDto();
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.ToIPB();
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeavn", DbType.AnsiStringFixedLength);
            param[3].Value = codeAvn;

            string sql = @"SELECT 
                PBEFA DATEEFFETA, PBEFM DATEEFFETM, PBEFJ DATEEFFETJ,
                PBFEA FINEFFETANNEE, PBFEM FINEFFETMOIS, PBFEJ FINEFFETJOUR,
                PBPER PERIODICITECODE, PERD.TPLIB PERIODICITENOM,
                NAT.TPLIL LIBELLENATURECONTRAT, PBAPP PARTALBINGIA,
                JDPEA PROCHECHA, JDPEM PROCHECHM, JDPEJ PROCHECHJ,
                PBRGT CODEREGIME, RG.TPLIB LIBREGIME,
                PBDEV DEVISE, DEV.TPLIB LIBDEVISE,
                PBSOU SOUSCODE, SOU.UTNOM SOUSNOM, PBGES GESCODE, GES.UTNOM GESNOM,
                PBICT COURTIERGESTIONNAIRE, GEST.TNNOM NOMCOURTIERGEST, PBCTA COURTIERAPPORTEUR, APPO.TNNOM NOMCOURTIERAPPO
            from YPOBASE
                LEFT JOIN YCOURTN GEST ON GEST.TNICT = PBICT AND GEST.TNXN5 = 0 AND GEST.TNTNM = 'A'
                LEFT JOIN YCOURTN APPO ON APPO.TNICT = PBCTA AND APPO.TNXN5 = 0 AND APPO.TNTNM = 'A'
                LEFT JOIN YYYYPAR PERD ON PERD.TCOD = PBPER AND PERD.TFAM = 'PBPER' AND PERD.TCON = 'PRODU'
                LEFT JOIN YUTILIS SOU ON PBSOU = SOU.UTIUT
                LEFT JOIN YUTILIS GES ON PBGES = GES.UTIUT
                LEFT JOIN YPRTENT ON JDIPB = PBIPB AND JDALX = PBALX
                LEFT JOIN YYYYPAR RG ON RG.TCOD = PBRGT AND RG.TFAM = 'TAXRG' AND RG.TCON = 'GENER'
                LEFT JOIN YYYYPAR DEV ON DEV.TCOD = PBDEV AND DEV.TFAM = 'DEVIS' AND DEV.TCON = 'GENER'
                LEFT JOIN YYYYPAR NAT ON NAT.TCOD = PBNPL AND NAT.TFAM = 'PBNPL' AND NAT.TCON = 'PRODU'
            where pbipb = :codeOffre AND PBALX = :version AND PBTYP = :type and PBAVN = :codeavn";

            var res = DbBase.Settings.ExecuteList<ContratDto>(CommandType.Text, sql, param).FirstOrDefault();

            return res;
        }

        /// <summary>
        /// Affiche les coassureurs, la part Albingia et la part couverte d'une offre
        /// </summary>
        /// <param name="type">Le type (offre ou police)</param>
        /// <param name="idOffre">Le numéro de l'offre (ou de la police)</param>
        /// <param name="idAliment">Le numéro de l'aliment (ou du connexe)</param>
        /// <returns></returns>
        public static FormCoAssureurDto InitCoAssureurs(string type, string idOffre, string idAliment, string codeAvn, ModeConsultation modeNavig)
        {
            FormCoAssureurDto result = null;

            string sql = string.Format(@" SELECT PBAPP PARTALBINGIA, PBPCV PARTCOUVERTE, PBAVJ DATEEFFETAVNJOUR, PBAVM DATEEFFETAVNMOIS,PBAVA DATEEFFETAVNANNEE 
                                          FROM {3} Base
                                          WHERE Base.PBIPB = '{0}' AND Base.PBALX = {1} AND {4} = '{2}' AND (PBNPL = 'A' OR PBNPL = 'E') ",
                                        idOffre.Trim().PadLeft(9, ' '), idAliment, type,
                                        CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                                        modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                                        modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);

            var res = DbBase.Settings.ExecuteList<FormCoAssureurDto>(CommandType.Text, sql);
            if (res != null && res.Any())
                result = res.FirstOrDefault();

            //Si result est nul, Albingia n'est pas apériteur du contrat/offre selectionné
            if (result == null) return null;

            //            //etat du Verrouillage de l'offre/contrat
            //            string req = string.Format(@"SELECT count(*) NBLIGN      
            //                                         FROM 
            //	                                     KVERROU                                        
            //                                         WHERE KAVIPB = '{0}'", idOffre.ToIPB());
            //            result.EstVerrouillee = CommonRepository.ExistRow(req);

            var lstCoAssureurs = LoadListCoAssureur(type, idOffre.ToIPB(), idAliment, codeAvn, modeNavig);
            if (lstCoAssureurs != null)
            {
                lstCoAssureurs.ForEach(c =>
                {
                    if (!string.IsNullOrEmpty(c.DateDebutDB) && c.DateDebutDB != "0/0/0")
                        c.DateDebut = Convert.ToDateTime(c.DateDebutDB);
                    if (!string.IsNullOrEmpty(c.DateFinDB) && c.DateFinDB != "0/0/0")
                        c.DateFin = Convert.ToDateTime(c.DateFinDB);
                });
            }
            result.ListeCoAssureurs = lstCoAssureurs;
            result.IsTraceAvnExist = CommonRepository.ExistTraceAvenant(idOffre, idAliment, type, "KCOASS", string.Empty, string.Empty, string.Empty, string.Empty, "GEN");
            return result;
        }

        /// <summary>
        /// Fonction qui permet de savoir si l'offre en paramètre peut avoir des co-assureurs ou non
        /// </summary>
        /// <param name="idContrat"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ExistCoAs(string idContrat, string version, string type, ModeConsultation modeNavig)
        {
            string sql = string.Empty;

            if (modeNavig == ModeConsultation.Historique)
                sql = string.Format(@" SELECT 
                               COUNT(*) NBLIGN
                            FROM YHPBASE
                            WHERE
                            PBIPB = '{0}' 
                            AND PBALX = {1}
                            AND	(PBNPL = 'A' OR PBNPL = 'E') ", idContrat.ToIPB(), version, type);
            else
                sql = string.Format(@" SELECT 
                               COUNT(*) NBLIGN
                            FROM YPOBASE
                            WHERE
                            PBIPB = '{0}' 
                            AND PBALX = {1}
                            AND PBTYP = '{2}'
                            AND	(PBNPL = 'A' OR PBNPL = 'E') ", idContrat.ToIPB(), version, type);


            return CommonRepository.ExistRow(sql);
        }

        /// <summary>
        /// Obtient un co assureur spécifique
        /// </summary>
        /// <param name="type">Le type d'offre.</param>
        /// <param name="idOffre">L'id de l'offre.</param>
        /// <param name="idAliment">L'id de l'aliment.</param>
        /// <param name="idCoAssureur">L'id du co assureur.</param>
        /// <returns></returns>
        public static CoAssureurDto GetCoAssureurDetail(string type, string idOffre, string idAliment, string idCoAssureur, bool modeCoAss)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("idOffre", DbType.AnsiStringFixedLength);
            param[0].Value = idOffre.ToIPB();
            param[1] = new EacParameter("idAliment", DbType.AnsiStringFixedLength);
            param[1].Value = idAliment;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("idCoAssureur", DbType.AnsiStringFixedLength);
            param[3].Value = idCoAssureur;

            string strNpl = string.Empty;
            if (modeCoAss)
                strNpl = "(PBNPL = 'A' OR PBNPL = 'E') AND	CoAssureur.PHTAP= 'C' ";
            else
                strNpl = "(PBNPL = 'C' OR PBNPL = 'D')  AND	CoAssureur.PHTAP= 'A' ";

            string sql = string.Format(@"SELECT 
                                CoAssureur.PHCIE GUIDID,
                                CoAssureur.PHCIE CODE,
                                Compagnies.CINOM NOM,	
                                CONCAT(Compagnies.CIDEP, Compagnies.CICPO) CODEPOSTAL,
                                Compagnies.CIVIL VILLE,
                                CoAssureur.PHAPP POURCENTPART,
                                CoAssureur.PHIN5 IDINTERLOCUTEUR,
                                IFNULL(Interlocuteurs.CLNOM, '')  INTERLOCUTEUR,
                                CoAssureur.PHPOL REFERENCE,
                                CoAssureur.PHAFR FRAISACC,
                                CoAssureur.PHEPJ CONCAT '/' CONCAT CoAssureur.PHEPM CONCAT '/' CONCAT CoAssureur.PHEPA DATEDEBUT, 
                                CoAssureur.PHFPJ CONCAT '/' CONCAT CoAssureur.PHFPM CONCAT '/' CONCAT CoAssureur.PHFPA DATEFIN,
                                CoAssureur.PHCOM COMMISSIONAPE, 
                                CoAssureur.PHTXF FRAISAPEALB
                            FROM 
                                YPOBASE Base 
                            INNER JOIN YPOCOAS CoAssureur ON 
                                Base.PBIPB = CoAssureur.PHIPB AND
                                Base.PBALX = CoAssureur.PHALX
                            INNER JOIN YCOMPA Compagnies ON
                                CoAssureur.PHCIE = Compagnies.CIICI
                            LEFT JOIN YCOMPAL Interlocuteurs ON
                                CoAssureur.PHIN5 = Interlocuteurs.CLIN5 AND
                                CoAssureur.PHCIE = Interlocuteurs.CLICI
                            WHERE Base.PBIPB = 	:idOffre
                                AND	{0} 	                                                      
                                AND PHALX = :idAliment
                                AND PHTYP = :type	
                                AND PHCIE = :idCoAssureur", strNpl);

            CoAssureurDto result = DbBase.Settings.ExecuteList<CoAssureurDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.DateDebutDB) && result.DateDebutDB != "0/0/0")
                    result.DateDebut = Convert.ToDateTime(result.DateDebutDB);
                if (!string.IsNullOrEmpty(result.DateFinDB) && result.DateFinDB != "0/0/0")
                    result.DateFin = Convert.ToDateTime(result.DateFinDB);
            }
            return result;
        }

        /// <summary>
        /// Applique les opérations de la liste en paramètre
        /// </summary>
        public static string EnregistrerListeCoAssureurs(string code, string version, string type, string typeAvenant, string avenant, List<CoAssureurDto> listeCoass, string user)
        {
            string error = string.Empty;
            bool saveTraceAvt = false;//flag qui permet de savoir si la trace a déjà été enregistré dans cette fonction
            foreach (CoAssureurDto coass in listeCoass)
            {
                if (string.IsNullOrEmpty(error))
                {
                    switch (coass.TypeOperation)
                    {
                        case "I":
                        case "U":
                            error = EnregistrerCoAssureur(type, code, version, coass, coass.TypeOperation);
                            if ((typeAvenant == AlbConstantesMetiers.TYPE_AVENANT_MODIF || typeAvenant == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF) && !saveTraceAvt)
                            {
                                if (!CommonRepository.ExistTraceAvenant(code, version, type, "KCOASS", string.Empty, string.Empty, string.Empty, string.Empty, "GEN"))
                                    CommonRepository.SaveTraceAvenant(code, version, type, "KCOASS", string.Empty, string.Empty, string.Empty, string.Empty, "GEN", string.Empty, "C", string.Empty, string.Empty, "O", string.Empty, user);
                                saveTraceAvt = true;
                            }
                            break;
                        case "D":
                            error = SupprimerCoAssureur(type, code, version, coass.GuidId);
                            break;
                        default: //Rien à faire, aucun changement sur ce coass                           
                            break;
                    }
                }
            }
            return error;
        }

        public static string EnregistrerCoAssureur(string type, string idOffre, string idAliment, CoAssureurDto newCoAssureur, string typeOperation)
        {
            int iIdAliment = 0;
            int.TryParse(idAliment, out iIdAliment);

            DbParameter[] param = new DbParameter[19];
            param[0] = new EacParameter("P_TYPE_OFFRE", type);
            param[1] = new EacParameter("P_ID_OFFRE", idOffre.ToIPB());
            param[2] = new EacParameter("P_ID_ALIMENT", iIdAliment);
            param[3] = new EacParameter("P_CODE_COMPAGNIE", newCoAssureur.Code);
            param[4] = new EacParameter("P_NOM_COMPAGNIE", newCoAssureur.Nom);
            param[5] = new EacParameter("P_CODEINTERLOCUTEUR", newCoAssureur.IdInterlocuteur);
            param[6] = new EacParameter("P_REFERENCE_POLICE", newCoAssureur.Reference);
            param[7] = new EacParameter("P_PART_ASSUREUR", newCoAssureur.PourcentPart);
            param[8] = new EacParameter("P_FRAIS_ACC", newCoAssureur.FraisAcc);
            param[9] = new EacParameter("P_DATE_DA", newCoAssureur.DateDebut == null ? 0 : newCoAssureur.DateDebut.Value.Year);
            param[10] = new EacParameter("P_DATE_DM", newCoAssureur.DateDebut == null ? 0 : newCoAssureur.DateDebut.Value.Month);
            param[11] = new EacParameter("P_DATE_DJ", newCoAssureur.DateDebut == null ? 0 : newCoAssureur.DateDebut.Value.Day);
            param[12] = new EacParameter("P_DATE_FA", newCoAssureur.DateFin == null ? 0 : newCoAssureur.DateFin.Value.Year);
            param[13] = new EacParameter("P_DATE_FM", newCoAssureur.DateFin == null ? 0 : newCoAssureur.DateFin.Value.Month);
            param[14] = new EacParameter("P_DATE_FJ", newCoAssureur.DateFin == null ? 0 : newCoAssureur.DateFin.Value.Day);
            param[15] = new EacParameter("P_FRAISAPEALB", newCoAssureur.FraisApeAlb);
            param[16] = new EacParameter("P_COMMAPERITEUR", newCoAssureur.CommissionAperiteur);
            param[17] = new EacParameter("P_TYPE_OPERATION", typeOperation);
            param[18] = new EacParameter("P_ERREUR", "");
            param[18].Direction = ParameterDirection.InputOutput;
            param[18].Size = 256;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDCOAS", param);
            return param[18].Value.ToString();
        }

        public static string SupprimerCoAssureur(string type, string idOffre, string idAliment, string guidId)
        {
            string req = string.Format(@"SELECT count(*) NBLIGN      
                                         FROM 
	                                     YPOBASE Base 
                                         INNER JOIN YPOCOAS CoAssureur ON 
	                                            Base.PBIPB = CoAssureur.PHIPB	
                                         INNER JOIN YCOMPA Compagnies ON
	                                            CoAssureur.PHCIE = Compagnies.CIICI
                                         LEFT JOIN YCOMPAL Interlocuteurs ON
	                                            CoAssureur.PHIN5 = Interlocuteurs.CLIN5 AND
	                                            CoAssureur.PHCIE = Interlocuteurs.CLICI
                                         WHERE Base.PBIPB = '{0}'
	                                            AND	(PBNPL = 'A' OR PBNPL = 'E') 	     
                                                AND	CoAssureur.PHTAP= 'C'                      
	                                            AND PHALX = '{1}'
	                                            AND PHTYP = '{2}'	
                                                AND PHCIE = '{3}'", idOffre.ToIPB(), idAliment, type, guidId);
            if (!CommonRepository.ExistRow(req)) return "Le co-assureur n'existe pas";

            string sql = @"DELETE FROM YPOCOAS                         
                           WHERE PHTYP = :type 
                            AND PHIPB = :idOffre
                            AND PHALX = :idAliment
                            AND PHCIE = :code";

            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[0].Value = type;
            param[1] = new EacParameter("idOffre", DbType.AnsiStringFixedLength);
            param[1].Value = idOffre.ToIPB();
            param[2] = new EacParameter("idAliment", DbType.AnsiStringFixedLength);
            param[2].Value = idAliment;
            param[3] = new EacParameter("code", DbType.AnsiStringFixedLength);
            param[3].Value = guidId;

            if (DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param) > 0)
                return "";
            else
                return "Aucune mise à jour, le co-assureur n'existe pas";
        }
        public static double GetMontantStatistique(string codeContrat, string version)
        {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.ToIPB();
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);

            string sql = @"SELECT CAST(SUM(SSTPR) AS NUMERIC(13,2)) MONTANT from YLGNSTP  
                            INNER JOIN YSTAPRO ON SLSOU = SSOUS AND SLAFF = SSAFF
                            WHERE SSIPW = :codeOffre AND SSALW = :version";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0)
                return result.FirstOrDefault().Montant;

            return 0;
        }

        #endregion

        #region méthodes privées

        /// <summary>
        /// Charge la liste des coassureurs d'une offre
        /// </summary>
        /// <param name="type">Le type (offre ou police)</param>
        /// <param name="idOffre">Le numéro de l'offre (ou de la police)</param>
        /// <param name="idAliment">Le numéro de l'aliment (ou du connexe)</param>
        /// <returns></returns>
        public static List<CoAssureurDto> LoadListCoAssureur(string type, string idOffre, string idAliment, string codeAvn, ModeConsultation modeNavig)
        {
            var param = new List<EacParameter>() {
                new EacParameter("idOffre", DbType.AnsiStringFixedLength){ Value = idOffre.ToIPB() },
                new EacParameter("idAliment", DbType.Int32){ Value = idAliment},
                new EacParameter("type", DbType.AnsiStringFixedLength){Value = type }
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter(":avn", DbType.Int32) { Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0 });
            }
            string sql = string.Format(@"SELECT 
                                CoAssureur.PHCIE GUIDID,
                                CoAssureur.PHCIE CODE,
                                Compagnies.CINOM NOM,	
                                CONCAT(Compagnies.CIDEP, Compagnies.CICPO) CODEPOSTAL,
                                Compagnies.CIVIL VILLE,
                                CoAssureur.PHAPP POURCENTPART,
                                CoAssureur.PHIN5 IDINTERLOCUTEUR,
                                IFNULL(Interlocuteurs.CLNOM, '')  INTERLOCUTEUR,
                                CoAssureur.PHPOL REFERENCE,
                                CoAssureur.PHAFR FRAISACC,
                                CoAssureur.PHEPJ CONCAT '/' CONCAT CoAssureur.PHEPM CONCAT '/' CONCAT CoAssureur.PHEPA DATEDEBUT, 
                                CoAssureur.PHFPJ CONCAT '/' CONCAT CoAssureur.PHFPM CONCAT '/' CONCAT CoAssureur.PHFPA DATEFIN,
                                CoAssureur.PHCOM COMMISSIONAPE, 
                                CoAssureur.PHTXF FRAISAPEALB	
                            FROM 
                                {3} Base 
                            INNER JOIN {4} CoAssureur ON 
                                Base.PBIPB = CoAssureur.PHIPB AND
                                PBALX = CoAssureur.PHALX {6}
                            INNER JOIN YCOMPA Compagnies ON
                                CoAssureur.PHCIE = Compagnies.CIICI
                            LEFT JOIN YCOMPAL Interlocuteurs ON
                                CoAssureur.PHIN5 = Interlocuteurs.CLIN5 AND
                                CoAssureur.PHCIE = Interlocuteurs.CLICI
                            WHERE Base.PBIPB = 	'{0}'
                                AND	(PBNPL = 'A' OR PBNPL = 'E') 	                           
                                AND PHALX = {1}
                                AND PHTYP = '{2}'	
                                AND	CoAssureur.PHTAP= 'C'
                                {7}
                            ORDER BY CoAssureur.PHAPP DESC",
                        /*0*/    idOffre.Trim().PadLeft(9, ' '),
                        /*1*/    idAliment,
                        /*2*/    type,
                        /*3*/    CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                        /*4*/    CommonRepository.GetPrefixeHisto(modeNavig, "YPOCOAS"),
                        /*5*/    modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                        /*6*/    modeNavig == ModeConsultation.Historique ? " AND CoAssureur.PHAVN = Base.PBAVN" : string.Empty,
                        /*7*/    modeNavig == ModeConsultation.Historique ? " AND Base.PBAVN = :avn" : string.Empty
                        );

            return DbBase.Settings.ExecuteList<CoAssureurDto>(CommandType.Text, sql, param);
        }


        #endregion

        #endregion
        #region retours signatures

        #region Méthodes publiques

        public static List<ParametreDto> GetListeTypesAccord()
        {
            string sql = @"SELECT 
                                TCOD CODE, 
                                TPLIL LIBELLE 
                           FROM YYYYPAR 
                           WHERE TCON = 'PRODU' 
                                AND TFAM = 'PBTAC'";

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        //        public static RetourPreneurDto GetRetourPreneur(string codeContrat, string versionContrat, string typeContrat, string codeAvenant, ModeConsultation modeNavig)
        //        {
        //            string sql = string.Format(@"SELECT 
        //	                           INT(PBTAA * 10000 + PBTAM * 100 + PBTAJ) DATERETOUR,
        //	                           CASE PBTAC WHEN '' THEN 'N' ELSE PBTAC END TYPEACCORD,
        //	                           CASE WHEN PQIPB IS NULL THEN 'N' ELSE 'O' END ISREGLEMENTRECU
        //                           FROM {3} 
        //                           LEFT JOIN YPOCUMU
        //	                           ON PQIPB = PBIPB 
        //	                           AND PQALX = PBALX
        //	                           AND PQMTR <> 0
        //	                           AND PQSRG <> 0
        //                           WHERE PBIPB = '{0}' AND PBALX = '{1}' AND PBTYP = '{2}'", 
        //                                                                                   codeContrat.ToIPB(), versionContrat, typeContrat,
        //                                                                                   CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE")
        //                                                                                 );
        //            var res = DbBase.Settings.ExecuteList<RetourPreneurDto>(CommandType.Text, sql);
        //            if (res.Any())
        //                return res.FirstOrDefault();
        //            return new RetourPreneurDto();
        //        }

        public static List<RetourCoassureurDto> GetRetoursCoassureurs(string codeContrat, string versionContrat, string typeContrat)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.ToIPB();
            param[1] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[1].Value = versionContrat;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = typeContrat;

            string sql = @"SELECT 
                            PHCIE GUIDID,
                            CINOM NOMCOASSUREUR,
                            PHAPP PARTCOASSUREUR,
                            INT(PHTAA * 10000 + PHTAM * 100 + PHTAJ) DATERETOUR,
                            CASE PHTAC WHEN '' THEN 'N' ELSE PHTAC END TYPEACCORD
                        FROM 
                            YPOBASE  
                        INNER JOIN YPOCOAS 
                            ON PBIPB = PHIPB	
                            AND PBALX = PHALX
                            AND PBTYP = PHTYP
                            AND (PBNPL = 'A' OR PBNPL = 'E')
                            AND PHTAP = 'C'
                        INNER JOIN YCOMPA 
                            ON PHCIE = CIICI
                        WHERE PBIPB = :P_CODECONTRAT AND PBALX = :P_VERSION AND PBTYP = :P_TYPE
                        ORDER BY PHAPP DESC";
            //codeContrat.ToIPB(), versionContrat, typeContrat);

            return DbBase.Settings.ExecuteList<RetourCoassureurDto>(CommandType.Text, sql, param);
        }

        public static RetourPreneurDto GetRetourPreneur(string codeContrat, string version, string type, string codeAvt, ModeConsultation modeNavig)
        {
            //            string sql = string.Format(@"SELECT INT(PBTAA * 10000 + PBTAM * 100 + PBTAJ) DATERETOUR,
            //	                                            CASE PBTAC WHEN '' THEN 'N' ELSE PBTAC END TYPEACCORD,
            //	                                            CASE IFNULL(PKKTR, 0) WHEN 0 THEN 'N' ELSE 'O' END ISREGLEMENTRECU    
            //                                        FROM {4} 
            //                                            LEFT JOIN YPRIMES ON PBIPB = PKIPB AND PBALX = PKALX AND PBAVN = PKAVN
            //                                        WHERE PBIPB = '{0}' AND PBALX = '{1}' AND PBTYP = '{2}'{3}",
            //                                        codeContrat.ToIPB(), version, type,
            //                                        !string.IsNullOrEmpty(codeAvt) && modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", codeAvt) : string.Empty,
            //                                        CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"));
            RetourPreneurDto toReturn = null;

            if (modeNavig == ModeConsultation.Standard)
            {
                string sql = string.Format(@"SELECT INT(PBTAA * 10000 + PBTAM * 100 + PBTAJ) DATERETOUR,
                                                                CASE PBTAC WHEN '' THEN 'N' ELSE PBTAC END TYPEACCORD,
                                                                CASE IFNULL(PKKTR, 0) WHEN 0 THEN 'N' ELSE 'O' END ISREGLEMENTRECU    
                                                        FROM YPOBASE 
                                                            LEFT JOIN YPRIMES ON PBIPB = PKIPB AND PBALX = PKALX AND PBAVN = PKAVN
                                                        WHERE PBIPB = '{0}' AND PBALX = '{1}' AND PBTYP = '{2}'{3}",
                                            codeContrat.ToIPB(), version, type,
                                            !string.IsNullOrEmpty(codeAvt) && modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", codeAvt) : string.Empty);
                var res = DbBase.Settings.ExecuteList<RetourPreneurDto>(CommandType.Text, sql);
                if (res.Any())
                    toReturn = res.FirstOrDefault();
            }
            if (modeNavig == ModeConsultation.Historique)
            {
                EacParameter[] param = new EacParameter[4];
                param[0] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                param[0].Value = codeContrat.ToIPB();
                param[1] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
                param[1].Value = version;
                param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[2].Value = type;
                param[3] = new EacParameter("P_AVT", DbType.AnsiStringFixedLength);
                param[3].Value = codeAvt;

                string sql = @"SELECT INT(PBTAA * 10000 + PBTAM * 100 + PBTAJ) DATERETOUR,
                                                    CASE PBTAC WHEN '' THEN 'N' ELSE PBTAC END TYPEACCORD,
                                                    CAST(SUM(PKKTR) AS NUMERIC(13,2)) MNTREGLEMENT
                                             FROM YHPBASE
                                             LEFT JOIN YPRIMES ON PBIPB = PKIPB AND PBALX = PKALX AND PBAVN = PKAVN
                                             WHERE PBIPB = :P_CODECONTRAT AND PBALX = :P_VERSION AND PBTYP = :P_TYPE AND PBAVN = :P_AVT
                                             GROUP BY INT(PBTAA * 10000 + PBTAM * 100 + PBTAJ), CASE PBTAC WHEN '' THEN 'N' ELSE PBTAC END";
                //codeContrat.ToIPB(), version, type, codeAvt);
                var res = DbBase.Settings.ExecuteList<RetourPreneurDto>(CommandType.Text, sql, param);
                if (res.Any())
                    toReturn = res.FirstOrDefault();
                toReturn.IsReglementRecu = toReturn.MntReglement != 0 ? "O" : "N";
            }
            if (toReturn == null)
                toReturn = new RetourPreneurDto();
            return toReturn;
        }

        public static List<RetourCoassureurDto> GetRetoursCoassureurs(string codeContrat, string version, string type, string codeAvt, ModeConsultation modeNavig)
        {
            string sql = string.Format(@"SELECT PHCIE GUIDID, CINOM NOMCOASSUREUR, PHAPP PARTCOASSUREUR, INT(PHTAA * 10000 + PHTAM * 100 + PHTAJ) DATERETOUR,
                                            CASE PHTAC WHEN '' THEN 'N' ELSE PHTAC END TYPEACCORD
                                        FROM {4}  
                                            INNER JOIN {5} ON PBIPB = PHIPB	AND PBALX = PHALX AND PBTYP = PHTYP AND (PBNPL = 'A' OR PBNPL = 'E') AND PHTAP = 'C'{6}
                                            INNER JOIN YCOMPA ON PHCIE = CIICI
                                        WHERE PBIPB = '{0}' AND PBALX = '{1}' AND PBTYP = '{2}'{3}
                                        ORDER BY PHAPP DESC",
                                        codeContrat.ToIPB(), version, type,
                                        modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvt) ? codeAvt : "0") : string.Empty,
                                        CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                                        CommonRepository.GetPrefixeHisto(modeNavig, "YPOCOAS"),
                                        modeNavig == ModeConsultation.Historique ? " AND PHAVN = PBAVN" : string.Empty);

            return DbBase.Settings.ExecuteList<RetourCoassureurDto>(CommandType.Text, sql);
        }

        public static void EnregistrerRetours(string codeContrat, string versionContrat, string typeContrat, string codeAvt, RetourPreneurDto retourPreneur, List<RetourCoassureurDto> retoursCoAssureurs, string user, bool isModifHorsAvn)
        {
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("P_TYPACC", DbType.AnsiStringFixedLength);
            param[0].Value = retourPreneur.TypeAccord;
            param[1] = new EacParameter("P_DATRETYEAR", DbType.Int32);
            param[1].Value = AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).HasValue ? AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).Value.Year : 0;
            param[2] = new EacParameter("P_DATRETMONTH", DbType.Int32);
            param[2].Value = AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).HasValue ? AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).Value.Month : 0;
            param[3] = new EacParameter("P_DATRETDAY", DbType.Int32);
            param[3].Value = AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).HasValue ? AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).Value.Day : 0;
            param[4] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[4].Value = codeContrat.ToIPB();
            param[5] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[5].Value = versionContrat;
            param[6] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[6].Value = typeContrat;

            //Enregistrement des données du retour du preneur d'assurance
            string sqlPreneur = @"UPDATE YPOBASE
                                  SET 
                                  PBTAC = :P_TYPACC,
                                  PBTAA = :P_DATRETYEAR,
                                  PBTAM = :P_DATRETMONTH,
                                  PBTAJ = :P_DATRETDAY
                                  WHERE PBIPB = :P_CODECONTRAT 
                                        AND PBALX = :P_VERSION 
                                        AND PBTYP = :P_TYPE";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlPreneur, param);

            param = new EacParameter[4];
            param[0] = new EacParameter("P_TYPACC", DbType.AnsiStringFixedLength);
            param[0].Value = retourPreneur.TypeAccord;
            param[1] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[1].Value = codeContrat.ToIPB();
            param[2] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[2].Value = versionContrat;
            param[3] = new EacParameter("P_AVENANT", DbType.AnsiStringFixedLength);
            param[3].Value = codeAvt;
            //Enregistrement des données du retour du preneur d'assurance (type d'accord) dans les primes
            string sqlPreneurPrimes = @"UPDATE YPRIMES
                                  SET 
                                  PKTAC = :P_TYPACC
                                  WHERE PKIPB = :P_CODECONTRAT 
                                        AND PKALX = :P_VERSION 
                                        AND PKAVN = :P_AVENANT";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlPreneurPrimes, param);

            //Enregistrement de la liste des coassureurs
            if (retoursCoAssureurs != null)
            {
                string sqlCoass = string.Empty;
                foreach (RetourCoassureurDto coAss in retoursCoAssureurs)
                {
                    param = new EacParameter[8];
                    param[0] = new EacParameter("P_TYPACC", DbType.AnsiStringFixedLength);
                    param[0].Value = coAss.TypeAccordVal;
                    param[1] = new EacParameter("P_DATRETYEAR", DbType.Int32);
                    param[1].Value = AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).HasValue ? AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).Value.Year : 0;
                    param[2] = new EacParameter("P_DATRETMONTH", DbType.Int32);
                    param[2].Value = AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).HasValue ? AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).Value.Month : 0;
                    param[3] = new EacParameter("P_DATRETDAY", DbType.Int32);
                    param[3].Value = AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).HasValue ? AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).Value.Day : 0;
                    param[4] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                    param[4].Value = codeContrat.ToIPB();
                    param[5] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
                    param[5].Value = versionContrat;
                    param[6] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                    param[6].Value = typeContrat;
                    param[7] = new EacParameter("P_GUID", DbType.AnsiStringFixedLength);
                    param[7].Value = coAss.GuidId;

                    sqlCoass = @"UPDATE YPOCOAS
                                               SET
                                                    PHTAC = :P_TYPACC,
                                                    PHTAA = :P_DATRETYEAR,
                                                    PHTAM = :P_DATRETMONTH,
                                                    PHTAJ = :P_DATRETDAY
                                               WHERE PHIPB = :P_CODECONTRAT 
                                                    AND PHALX = :P_VERSION 
                                                    AND PHTYP = :P_TYPE
                                                    AND PHCIE = :P_GUID";

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlCoass, param);
                }
            }

            #region Ajout de l'acte de gestion
            var libTraitement = string.Format("Accord {0} {1}/{2}/{3}", retourPreneur.TypeAccord,
                AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).HasValue ? AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).Value.Day : 0,
                AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).HasValue ? AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).Value.Month : 0,
                AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).HasValue ? AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).Value.Year : 0);
            if (!isModifHorsAvn)
                CommonRepository.AjouterActeGestion(codeContrat, versionContrat, typeContrat, 0, AlbConstantesMetiers.ACTEGESTION_VALIDATION, AlbConstantesMetiers.TRAITEMENT_SELPO, libTraitement, user);
            #endregion
        }

        public static void EnregistrerRetoursHisto(string codeContrat, string versionContrat, string typeContrat, string codeAvt, RetourPreneurDto retourPreneur, List<RetourCoassureurDto> retoursCoAssureurs, ModeConsultation modeNavig)
        {
            EacParameter[] param = new EacParameter[8];
            param[0] = new EacParameter("P_DATRETYEAR", DbType.Int32);
            param[0].Value = AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).HasValue ? AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).Value.Year : 0;
            param[1] = new EacParameter("P_DATRETMONTH", DbType.Int32);
            param[1].Value = AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).HasValue ? AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).Value.Month : 0;
            param[2] = new EacParameter("P_DATRETDAY", DbType.Int32);
            param[2].Value = AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).HasValue ? AlbConvert.ConvertIntToDate(retourPreneur.DateRetour).Value.Day : 0;
            param[3] = new EacParameter("P_TYPACC", DbType.AnsiStringFixedLength);
            param[3].Value = retourPreneur.TypeAccord;
            param[4] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[4].Value = codeContrat.ToIPB();
            param[5] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[5].Value = versionContrat;
            param[6] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[6].Value = typeContrat;
            param[7] = new EacParameter("P_AVT", DbType.AnsiStringFixedLength);
            param[7].Value = codeAvt;


            string sql = @"UPDATE YHPBASE SET PBTAA = :P_DATRETYEAR, PBTAM = :P_DATRETMONTH, PBTAJ = :P_DATRETDAY, PBTAC = :P_TYPACC
                                            WHERE PBIPB = :P_CODECONTRAT AND PBALX = :P_VERSION AND PBTYP = :P_TYPE AND PBAVN = :P_AVT";


            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            param = new EacParameter[4];
            param[0] = new EacParameter("P_TYPACC", DbType.AnsiStringFixedLength);
            param[0].Value = retourPreneur.TypeAccord;
            param[1] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[1].Value = codeContrat.ToIPB();
            param[2] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[2].Value = versionContrat;
            param[3] = new EacParameter("P_AVENANT", DbType.AnsiStringFixedLength);
            param[3].Value = codeAvt;
            //Enregistrement des données du retour du preneur d'assurance (type d'accord) dans les primes
            string sqlPreneurPrimes = @"UPDATE YPRIMES
                                  SET 
                                  PKTAC = :P_TYPACC
                                  WHERE PKIPB = :P_CODECONTRAT 
                                        AND PKALX = :P_VERSION 
                                        AND PKAVN = :P_AVENANT";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlPreneurPrimes, param);

            if (retoursCoAssureurs != null)
            {
                foreach (RetourCoassureurDto coAss in retoursCoAssureurs)
                {
                    param = new EacParameter[9];
                    param[0] = new EacParameter("P_DATRETYEAR", DbType.Int32);
                    param[0].Value = AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).HasValue ? AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).Value.Year : 0;
                    param[1] = new EacParameter("P_DATRETMONTH", DbType.Int32);
                    param[1].Value = AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).HasValue ? AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).Value.Month : 0;
                    param[2] = new EacParameter("P_DATRETDAY", DbType.Int32);
                    param[2].Value = AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).HasValue ? AlbConvert.ConvertIntToDate(coAss.DateRetourCoAss).Value.Day : 0;
                    param[3] = new EacParameter("P_TYPACC", DbType.AnsiStringFixedLength);
                    param[3].Value = coAss.TypeAccordVal;
                    param[4] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                    param[4].Value = codeContrat.ToIPB();
                    param[5] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
                    param[5].Value = versionContrat;
                    param[6] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                    param[6].Value = typeContrat;
                    param[7] = new EacParameter("P_AVT", DbType.AnsiStringFixedLength);
                    param[7].Value = codeAvt;
                    param[8] = new EacParameter("P_GUID", DbType.AnsiStringFixedLength);
                    param[8].Value = coAss.GuidId;

                    sql = @"UPDATE YHPCOAS SET PHTAA = :P_DATRETYEAR, PHTAM = :P_DATRETMONTH, PHTAJ = :P_DATRETDAY, PHTAC = :P_TYPACC
                                            WHERE PHIPB = :P_CODECONTRAT AND PHALX = :P_VERSION AND PHTYP = :P_TYPE AND PHAVN = :P_AVT AND PHCIE = :P_GUID";

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                }
            }
        }

        #endregion

        #endregion

        #region Template

        public static ContratInfoBaseDto GetInfoTemplate(string idTemp)
        {
            var infoTemp = ParamTemplatesRepository.GetInfoTemplate(idTemp);
            ContratInfoBaseDto model = new ContratInfoBaseDto();
            if (infoTemp != null)
            {
                model.CodeContrat = infoTemp.CodeOffre.ToIPB();
                model.VersionContrat = infoTemp.Version.HasValue ? infoTemp.Version.Value : 0;
                model.Type = infoTemp.Type;
                model.Branche = infoTemp.Branche != null ? infoTemp.Branche.Code : string.Empty;
                model.Cible = infoTemp.Branche != null && infoTemp.Branche.Cible != null ? infoTemp.Branche.Cible.Code : string.Empty;
            }

            model.TypesContrat = CommonRepository.GetParametres(model.Branche, model.Cible, "KHEOP", "TYPOC");
            model.Branches = CommonRepository.GetParametres(model.Branche, model.Cible, "GENER", "BRCHE", tPcn2: "1");
            model.Encaissements = CommonRepository.GetParametres(model.Branche, model.Cible, "GENER", "TCYEN");


            return model;
        }

        #endregion
        #region Blocage termes

        #region Méthodes publiques
        public static List<ParametreDto> GetListeZonesStop()
        {
            string sql = @"SELECT TCOD CODE , 
                                  TPLIB LIBELLE 
                           FROM YYYYPAR 
                           WHERE TCON = 'PRODU' AND TFAM = 'PBSTP'";

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        public static string GetZoneStop(string codeContrat, string versionContrat, string typeContrat)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[0].Value = codeContrat.ToIPB();
            param[1] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[1].Value = typeContrat;
            param[2] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[2].Value = versionContrat;

            string sql = @"SELECT PBSTP STRRETURNCOL FROM YPOBASE
                                         WHERE PBIPB = :P_CODECONTRAT AND PBTYP = :P_TYPE AND PBALX = :P_VERSION";
            //codeContrat.Trim(), typeContrat, versionContrat);

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
                return result.FirstOrDefault().StrReturnCol;
            return string.Empty;
        }
        public static void SaveZoneStop(string codeContrat, string versionContrat, string typeContrat, string zoneStop)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_STOP", DbType.AnsiStringFixedLength);
            param[0].Value = zoneStop;
            param[1] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
            param[1].Value = codeContrat.ToIPB();
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = typeContrat;
            param[3] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[3].Value = versionContrat;

            string sql = @"UPDATE YPOBASE
                                              SET PBSTP = :P_STOP
                                         WHERE PBIPB = :P_CODECONTRAT
                                              AND PBTYP = :P_TYPE
                                              AND PBALX = :P_VERSION";
            //zoneStop, codeContrat.Trim(), typeContrat, versionContrat);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static DeblocageTermeDto GetEcheanceEmission(string codeContrat, string versionContrat, string typeContrat, string mode, string user, string acteGestion, AlbConstantesMetiers.DroitBlocageTerme niveauDroit, bool emission)
        {
            DeblocageTermeDto toReturn = new DeblocageTermeDto();
            string[] tab = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            if (mode == "Init")
            {
                EacParameter[] param = new EacParameter[3];
                param[0] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                param[0].Value = codeContrat.ToIPB();
                param[1] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[1].Value = typeContrat;
                param[2] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
                param[2].Value = versionContrat;

                /*SAB: Conditions de deblocage */
                string sqlBase = @"SELECT PBSTP STRRETURNCOL ,PBCON STRRETURNCOL2,PBETA ETAT, PBPER PERIODICITE FROM YPOBASE
                                                   WHERE PBIPB= :P_CODECONTRAT AND PBTYP = :P_TYPE AND PBALX= :P_VERSION";
                //codeContrat.Trim(), typeContrat, versionContrat);

                var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlBase, param);
                if (result != null && result.Any())
                {
                    if (niveauDroit == AlbConstantesMetiers.DroitBlocageTerme.Niveau1 && result.FirstOrDefault().Etat == "V"
                    && !(tab.Contains(result.FirstOrDefault().StrReturnCol)) && !(tab.Contains(result.FirstOrDefault().StrReturnCol2)))
                    {
                        toReturn = DeblocageDesTermes(codeContrat, versionContrat);
                    }
                }
            }
            if (mode == "Loop")
            {
                if (emission)
                {
                    // Appel progAS400 YDP340CL
                    string result = CommonRepository.EmettreTermes(codeContrat, versionContrat, typeContrat);
                    if (string.IsNullOrEmpty(result))
                    {
                        toReturn = DeblocageDesTermes(codeContrat, versionContrat);
                    }
                    else
                    {
                        // retour Problème dans l'émission du terme
                        toReturn.MsgErreur = "Problème dans l'émission du terme";
                    }
                }
            }
            return toReturn;
        }
        private static DeblocageTermeDto DeblocageDesTermes(string codeContrat, string versionContrat)
        {
            /* SAB :Recherche du dernier mois/année de traitement */
            int dateDernierTerme = 0;
            string sqlyyypar = string.Format(@"SELECT TPCN1 MONTANT FROM YYYYPAR WHERE TCON='YMAG' AND TFAM='TERME' AND TCOD='TRAITEM_RT'");
            var retour = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlyyypar);
            if (retour != null && retour.Any())
            {
                dateDernierTerme = Convert.ToInt32(Math.Floor(retour.FirstOrDefault().Montant));
            }
            /* SAB: recherche la prochaine écheance */
            string sqlYprtent = string.Format(@"SELECT  JDDPJ DATEDEBDERNIEREPERIODEJOUR,
                                                JDDPM DATEDEBDERNIEREPERIODEMOIS,
                                                JDDPA DATEDEBDERNIEREPERIODEANNEE,
                                                JDFPJ DATEFINDERNIEREPERIODEJOUR,
                                                JDFPM DATEFINDERNIEREPERIODEMOIS,
                                                JDFPA DATEFINDERNIEREPERIODEANNEE,
                                                JDPEJ DATEECHEANCEEMISSIONJOUR,
                                                JDPEM DATEECHEANCEEMISSIONMOIS,
                                                JDPEA DATEECHEANCEEMISSIONANNEE
                                        FROM
                                                YPRTENT
                                        WHERE   
                                                JDIPB = '{0}'
                                                AND JDALX = '{1}'", codeContrat.Trim().PadLeft(9, ' '), versionContrat);
            var retourProcEch = DbBase.Settings.ExecuteList<DeblocageTermeDto>(CommandType.Text, sqlYprtent);
            if (retourProcEch != null && retourProcEch.Any())
            {
                int dateProchaineEch = (retourProcEch.FirstOrDefault().DateEcheanceEmissionAnnee * 100 + retourProcEch.FirstOrDefault().DateEcheanceEmissionMois);
                if (dateProchaineEch != 0 && (dateProchaineEch <= dateDernierTerme))
                {
                    return retourProcEch.FirstOrDefault();
                }
            }
            return new DeblocageTermeDto();
        }
        #endregion
        #endregion

        public static bool ContratHasQuittances(string codeOffre, string version, string type)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPRIMES WHERE PKIPB = '{0}' AND PKALX = {1}", codeOffre.PadLeft(9, ' '), version);
            return CommonRepository.ExistRow(sql);
        }

        public bool ContratHasPrimesEnCours(string codeOffre, string version, string type)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPRIMES WHERE PKIPB = '{0}' AND PKALX = {1} AND PKSIT = 'A' AND (PKKRG <> 0 OR PKMTR <> 0)", codeOffre.PadLeft(9, ' '), version);
            return CommonRepository.ExistRow(sql);
        }

        public bool ContratHasPrimesReglees(string codeOffre, string version, string type)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPRIMES WHERE PKIPB = '{0}' AND PKALX = {1} AND PKSIT = 'S' AND (PKKRG <> 0 OR PKMTR <> 0)", codeOffre.PadLeft(9, ' '), version);
            return CommonRepository.ExistRow(sql);
        }

        public static void CorrectionECM(string codeContrat, string versionContrat, string splitChar, string user, string acteGestion)
        {
            //DbParameter[] param = new DbParameter[4];
            //param[0] = new EacParameter("CODE", "RS1709163");
            //param[1] = new EacParameter("VERS", 0);
            //param[1].Value = 0;
            //param[2] = new EacParameter("TYPE", "P");
            //param[3] = new EacParameter("ID", string.Empty);
            //param[3].Direction = ParameterDirection.InputOutput;
            //param[3].DbType = DbType.AnsiStringFixedLength;

            //DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "PROC_TEST");//, param);

            //string sql = @"SELECT DISTINCT PBIPB CODECONTRAT, PBALX VERSIONCONTRAT, PBAVN NUMAVENANT, PBTTR CODEACTION , PBMJU SOUSNOM , KDOID REGULEID
            //    FROM YPOBASE y 
            //     INNER JOIN KPENG k ON (y.PBIPB, y.PBALX) = (k.KDOIPB, k.KDOALX)
            //     INNER JOIN KPENGFAM k2 ON k.KDOID = k2.KDPKDOID 
            //     INNER JOIN KPENGVEN k3 ON k.KDOID = k3.KDQKDOID 
            //     INNER JOIN KPENGRSQ k4 ON k3.KDQID = k4.KDRKDQID 
            //    WHERE (PBMJA * 10000 + PBMJM * 100 + PBMJJ ) >= 20210712
            //     AND PBSIT = 'A'
            //     AND PBORK ='KHE' 
            //     AND PBTYP = 'P' 
            //     AND PBBRA ='RT' 
            //     AND PBTTR NOT IN ('REGUL', 'AVNRM','AVNRS') 
            //     AND PBMJU != 'NUIT'
            //     AND k.KDOENG <> k2.KDPSMP 
            //    ORDER BY PBIPB , PBALX ";

            //var result = DbBase.Settings.ExecuteList<ContratDto>(CommandType.Text, sql);

            //if (result != null && result.Any())
            //{
            //    result.ForEach(r =>
            //    {
            //        var ipb = r.CodeContrat;
            //        var alx = r.VersionContrat;
            //        var avn = r.NumAvenant;
            //        var ttr = r.TypeTraitement;
            //        var majd = r.SouscripteurNom;

            //        if (r.NumAvenant > 0)
            //        {
            //            _ = CommonRepository.LoadAS400EngagementAvn(ipb.ToString(), alx.ToString(), "P", r.ReguleId.ToString(), avn.ToString(), majd.ToString(), ttr.ToString());
            //        }
            //        else
            //        {
            //            _ = CommonRepository.LoadAS400Engagement(ipb.ToString(), alx.ToString(), "P", ModeConsultation.Standard, avn.ToString(), majd.ToString(), ttr.ToString());
            //        }
            //    });
            //}


            CommonRepository.LoadAS400Engagement("IA2108691", "0", "P", ModeConsultation.Standard, "0", "POTEL", "AFFNV");
            //CommonRepository.LoadAS400Engagement("RC2200076", "0", "P", ModeConsultation.Standard, "0", "VIEU", "AFFNV");
            //CommonRepository.LoadAS400Engagement("RC2105762", "0", "P", ModeConsultation.Standard, "1", "MARECHAL", "AVNMD");
            //CommonRepository.LoadAS400Engagement("RC2104263", "0", "P", ModeConsultation.Standard, "0", "MARECHAL", "AFFNV");
            //CommonRepository.LoadAS400Engagement("RC2105520", "0", "P", ModeConsultation.Standard, "1", "MARECHAL", "AVNMD");
            //CommonRepository.LoadAS400Engagement("RC2105813", "0", "P", ModeConsultation.Standard, "0", "MARECHAL", "AFFNV");
            //CommonRepository.LoadAS400Engagement("RC2103544", "0", "P", ModeConsultation.Standard, "0", "MARECHAL", "AFFNV");
        }

        #region Vérification trace date fin effet
        /// <summary>
        /// B3101
        /// Vérification trace date fin effet
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HaveTraceOfEndEffectDate(string codeContrat, string version, string type, string numAvn)
        {
            var result = false;
            try
            {
                string sql = string.Format("SELECT KIGFEA FINEFFETANNEE, KIGFEM FINEFFETMOIS, KIGFEJ FINEFFETJOUR FROM KPHAVH WHERE KIGIPB = '{0}' AND KIGALX = {1} AND KIGTYP = '{2}' AND KIGAVN = {3}",
                                               codeContrat.PadLeft(9, ' '), Convert.ToInt32(version), type, Convert.ToInt32(numAvn));
                var TraceList = DbBase.Settings.ExecuteList<ContratDto>(CommandType.Text, sql);
                result = TraceList.Any(x => x.FinEffetAnnee != 0 && x.FinEffetMois != 0 && x.FinEffetJour != 0);
            }
            catch (Exception)
            {

                throw;
            }
            return result;

        }

        /// <summary>
        /// Recuperation date fin effet 
        /// </summary>
        /// <param name="contratId"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <param name="modeNavig"></param>
        /// <returns></returns>
        public static ContratDto GetEndEffectDate(string contratId, string version, string type)
        {
            ContratDto result = null;
            try
            {
                var param = new EacParameter[3];
                param[0] = new EacParameter("type", DbType.AnsiStringFixedLength)
                {
                    Value = type
                };
                param[1] = new EacParameter("contrat", DbType.AnsiStringFixedLength)
                {
                    Value = contratId.PadLeft(9, ' ')
                };
                param[2] = new EacParameter("version", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                };

                string sql = @"SELECT PBFEA FINEFFETANNEE, PBFEM FINEFFETMOIS, PBFEJ FINEFFETJOUR FROM YPOBASE WHERE PBTYP =:type AND  PBIPB =:contrat AND PBALX =:version ";

                result = DbBase.Settings.ExecuteList<ContratDto>(CommandType.Text, sql, param).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
            return result;

        }
        #endregion

        public DateTime? GetDateAvenant(Folder folder)
        {
            if (this.connection != null)
            {
                var options = new DbSelectOptions
                {
                    CommandType = CommandType.Text,
                    SqlText = SelectDateAvenant,
                    DbConnection = this.connection,
                    AllowMissingColumnMappings = true
                };
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type, folder.NumeroAvenant);
                return options.PerformGetFirstField<DateTime>();
            }
            return default(DateTime?);
        }

        public void ModifyTaux(Folder folder, double taux, double tauxCAT)
        {
            var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = UpdateTaux
            };
            options.BuildParameters(taux, tauxCAT, folder.CodeOffre.ToIPB(), folder.Version);
            options.Exec();
        }

        public (int jdpea, int jdpem, int jdpej) GetProchaineEcheance(Folder folder)
        {
            var options = new DbSelectOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = SelectProchaineEcheance
            };
            options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version);
            return options.PerformSelect<(int jdpea, int jdpem, int jdpej)>().FirstOrDefault();
        }

        public IEnumerable<AffNouvProchaineEcheanceDto> GetProchaineEcheanceAffaireNouvelle(Folder folder)
        {
            return Fetch<AffNouvProchaineEcheanceDto>(SelectAffNouvProchaineEcheance, folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
        }

        public void UpdateProchaineEcheanceAffaireNouvelle(Folder folder, DateTime? prochaineEcheance, DateTime? debutPeriode, DateTime? finPeriode)
        {
            using (var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = UpdateAffNouvProchaineEcheance
            })
            {
                options.BuildParameters(
                    prochaineEcheance.HasValue ? prochaineEcheance.Value.Day : default,
                    prochaineEcheance.HasValue ? prochaineEcheance.Value.Month : default,
                    prochaineEcheance.HasValue ? prochaineEcheance.Value.Year : default,
                    debutPeriode.HasValue ? debutPeriode.Value.Day : default,
                    debutPeriode.HasValue ? debutPeriode.Value.Month : default,
                    debutPeriode.HasValue ? debutPeriode.Value.Year : default,
                    finPeriode.HasValue ? finPeriode.Value.Day : default,
                    finPeriode.HasValue ? finPeriode.Value.Month : default,
                    finPeriode.HasValue ? finPeriode.Value.Year : default,
                    folder.CodeOffre.ToIPB(),
                    folder.Version);
                options.Exec();
            }
        }

        public void CancelProchaineEcheance(Folder folder)
        {
            var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = UpdateProchaineEcheanceCancel
            };
            options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version);
            options.Exec();
        }

        public (string pbbra, string pbsbr, string pbcat, string kaacible) GetCiblage(Folder folder)
        {
            return Fetch<(string, string, string, string)>(SelectCiblage, folder.CodeOffre.ToIPB(), folder.Version, folder.Type).FirstOrDefault();
        }


        public FolderBasicData GetBasicFolder(Folder folder)
        {
            IEnumerable<FolderBasicData> list;
            if (this.historyMode == true)
            {
                list = Fetch<FolderBasicData>($"{SimpleSelectFolder} AND PBAVN = :AVN", folder.CodeOffre.ToIPB(), folder.Version, folder.Type, folder.NumeroAvenant);
            }
            else
            {
                list = Fetch<FolderBasicData>(SimpleSelectFolder, folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
            }
            return list?.FirstOrDefault();
        }

        public string GetStatutKheops(Folder folder)
        {
            using (var options = new DbSelectStringsOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = SelectStatutKheops
            })
            {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
                options.PerformSelect();
                return options.StringList?.FirstOrDefault() ?? string.Empty;
            }
        }


        public (string, string, string) GetInfoBase(Folder folder)
        {
            var options = new DbSelectOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = SelectInfoBase
            };
            options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
            return options.PerformSelect<(string, string, string)>().FirstOrDefault();
        }

        public IEnumerable<(int jdpea, int jdpem, int jdpej)> GetPeriodeBetween(Folder folder, DateTime date)
        {
            using (var options = new DbSelectOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = SelectPeriodeBetween
            })
            {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, date.ToIntYMD());
                return options.PerformSelect<(int jdpea, int jdpem, int jdpej)>();
            }
        }

        public void CloturerExcercice(Folder folder, DateTime datePeriode, DateTime dateResiliation)
        {
            using (var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = UpdateExcerciceCloturer
            })
            {
                options.BuildParameters(
                    datePeriode.Year, datePeriode.Month, datePeriode.Day,
                    dateResiliation.Year, dateResiliation.Month, dateResiliation.Day,
                    folder.CodeOffre.ToIPB(), folder.Version);
                options.Exec();
            }
        }

        public FolderBasicData GetFolderLight(Folder folder)
        {
            using (var options = new DbSelectOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = SimpleSelectFolder
            })
            {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
                return options.PerformSelect<FolderBasicData>()?.FirstOrDefault();
            }
        }

        public IEnumerable<PeriodeEffetAffaireData> GetDatesEffets(IEnumerable<string> ipbList)
        {
            return Fetch<PeriodeEffetAffaireData>(SelectDatesEffets, ipbList);
        }

        public int GetCountByNPL(Folder folder, IEnumerable<string> codesNPL, ModeConsultation mode)
        {
            try
            {
                SetHistoryMode(mode == ModeConsultation.Historique ? ActivityMode.Active : 0);
                using (var options = new DbCountOptions(this.connection == null)
                {
                    DbConnection = this.connection,
                    SqlText = FormatQuery(CountByNPL)
                })
                {
                    options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, codesNPL.ToList());
                    var dapperParams = new DynamicParameters();
                    options.Parameters.ToList().ForEach(p => dapperParams.Add(p.ParameterName, p.Value));
                    return options.DbConnection.ExecuteScalar<int>(
                        sql: options.SqlText,
                        param: dapperParams,
                        commandType: options.CommandType);
                }
            }
            finally
            {
                ResetHistoryMode();
            }
        }

        public int GetCountOffreSelections(Folder offre, Folder contrat)
        {
            using (var options = new DbCountOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = CountOffreSelections
            })
            {
                options.BuildParameters(contrat.CodeOffre.ToIPB(), contrat.Version, offre.CodeOffre.ToIPB(), offre.Version);
                options.PerformCount();
                return options.Count;
            }
        }

        public int InitOffreSelectionRisques(Folder offre, Folder contrat)
        {
            using (var options = new DbSPOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = "SP_CORISQUE"
            })
            {
                options.Parameters = new[] {
                    new EacParameter("P_CODEOFFRE", offre.CodeOffre.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = offre.Version },
                    new EacParameter("P_CODECONTRAT", contrat.CodeOffre.ToIPB()),
                    new EacParameter("P_VERSIONCONTRAT", DbType.Int32) { Value = contrat.Version }
                };
                options.ExecStoredProcedure();
                return options.ReturnedValue;
            }
        }

        public IEnumerable<ApplicationsFrmlOptData> GetApplicationsFrmlOpt(Folder folder)
        {
            return Fetch<ApplicationsFrmlOptData>(SelectApplicationsFormulesOptions, folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
        }

        public IEnumerable<OffreSelectionData> GetOffreSelections(Folder offre, Folder contrat)
        {
            return Fetch<OffreSelectionData>(SelectOffreSelections, offre.CodeOffre.ToIPB(), offre.Version, contrat.CodeOffre.ToIPB(), contrat.Version);
        }

        public int AddOffreSelectionFormule(ApplicationsFrmlOptData data, Folder contract)
        {
            return AddOffreSelection(data, contract, "F");
        }

        public int AddOffreSelectionOption(ApplicationsFrmlOptData data, Folder contract)
        {
            return AddOffreSelection(data, contract, "O");
        }

        public void SetOption1ForNewAffair(Folder folder)
        {
            using (var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection
            })
            {
                foreach (var key in TablesWithOPT.Keys)
                {
                    options.SqlText = string.Format(UpdateSingleOptionForContrat, key, TablesWithOPT[key]);
                    options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version);
                    options.Exec();
                }
            }
        }

        public NouvelleAffaireData GetNouvelleAffaire(Folder contrat)
        {
            return Fetch<NouvelleAffaireData>(SelectNouvelleAffaire, contrat.CodeOffre.ToIPB(), contrat.Version)?.FirstOrDefault();
        }

        public bool LockFolder(FolderLock folder, string action)
        {
            string affair = folder.BuildId(" ");
            try
            {
                int lockId = GetNextId("KAVID");
                var now = DateTime.Now;
                Exec(
                    InsertLock,
                    lockId,
                    "PRODU",
                    folder.Type, folder.CodeOffre.ToIPB(), folder.Version, folder.NumeroAvenant,
                    0, 0, string.Empty,
                    folder.ActeGestion, action, Booleen.Oui.AsCode(),
                    folder.User, now.ToString("yyyyMMdd").ParseInt(), now.ToString("HHmmss").ParseInt(),
                    $"Verrouillage {folder.Name} {affair}");
            }
            catch (Exception ex)
            {
                AlbLog.Warn($"Echec de verrouillage de l'affaire {affair} {Environment.NewLine}{ex.ToString()}");
                return false;
            }
            return true;
        }

        public bool UnlockFolder(FolderKey folder)
        {
            if (folder.User != WCFHelper.GetFromHeader("UserAS400"))
            {
                return false;
            }
            try
            {
                Exec(DeleteLock, folder.CodeOffre.ToIPB(), folder.Version, folder.Type, folder.User);
            }
            catch (Exception ex)
            {
                AlbLog.Warn($"Echec du déverrouillage de l'affaire {folder.Identifier} {Environment.NewLine}{ex.ToString()}");
                return false;
            }
            return true;
        }

        public int SP_AFFNOUV(params object[] @params)
        {
            var parameters = new Dictionary<string, object>() {
                { "P_CODEOFFRE", @params[0].ToString().ToIPB() },
                { "P_VERSION", @params[1] },
                { "P_TYPE", @params[2] },
                { "P_CODECONTRAT", @params[3].ToString().ToIPB() },
                { "P_VERSIONCONTRAT", @params[4] },
                { "P_DATESYSTEME", @params[5] },
                { "P_USER", @params[6] },
                { "P_TRAITEMENT", @params[7] }
            };
            return ExecSp(nameof(SP_AFFNOUV), parameters);
        }

        public int SP_CANAFNO(params object[] @params)
        {
            var parameters = new Dictionary<string, object>() {
                { "P_CODEOFFRE", @params[0].ToString().ToIPB() },
                { "P_VERSION", @params[1] }
            };
            return ExecSp(nameof(SP_CANAFNO), parameters);
        }

        public int SP_RESET_NEW_AFFAIR(params object[] @params)
        {
            var parameters = new Dictionary<string, object>() {
                { "P_CODEAFFAIRE", @params[0].ToString().ToIPB() },
                { "P_VERSION", @params[1] },
                { "P_TYPE", @params[2] }
            };
            return ExecSp(nameof(SP_RESET_NEW_AFFAIR), parameters);
        }

        private int AddOffreSelection(ApplicationsFrmlOptData data, Folder contract, string type)
        {
            if (!type?.IsIn("F", "O") ?? false)
            {
                type = "F";
            }
            int newId = GetNextId("KFJCHR");
            using (var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = InsertKpofOpt
            })
            {
                options.BuildParameters(contract.CodeOffre.ToIPB(), contract.Version, data.Ipb, data.Alx, newId, type, data.For, data.Opt, data.IdFor, data.IdOpt);
                options.Exec();
                return options.ReturnedValue;
            }
        }

        #region Methods Contracts Kheops

        public void CreationContractKheops(ContractJsonDto contract, string user)
        {
            using (var dbOptions = new DbSPOptions(this.connection == null)
            {
                SqlText = "SP_SAVCONT",
                DbConnection = this.connection
            })
            {
                dbOptions.Parameters = new[] {
                    new EacParameter("P_CODECONTRAT", contract.CodeAffaire.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = Convert.ToInt32(contract.Aliment) },
                    new EacParameter("P_TYPE", AlbConstantesMetiers.TYPE_CONTRAT),
                    new EacParameter("P_UPDATEMODE", "N"),
                    new EacParameter("P_DATENOW", DbType.Int32) { Value = DateTime.Now.ToIntYMD() },
                    new EacParameter("P_BRANCHE", contract.Branche.Code),
                    new EacParameter("P_CIBLE", contract.Branche.Cible.Code),
                    new EacParameter("P_PRENEURASS", DbType.Int32) { Value = Convert.ToInt32(contract.Assure.Code) },
                    new EacParameter("P_DESCRIPTIF", contract.Designation),
                    new EacParameter("P_COURTIERGEST", DbType.Int32) { Value = Convert.ToInt32(contract.Courtier.Gestionnaire.Code) },
                    new EacParameter("P_INTERLOCUTEUR", DbType.Int32) { Value = int.TryParse(contract.Courtier.Gestionnaire.Interlocuteur.Code, out var cd) ? cd : 0 },
                    new EacParameter("P_REFCOURTIER", ""),
                    new EacParameter("P_ANNEEACCORD", DbType.Int32) { Value =  DateTime.TryParse(contract.DateAccord, out var dtAccordYear) ? dtAccordYear.Year : 0 },
                    new EacParameter("P_MOISACCORD", DbType.Int32) { Value =  DateTime.TryParse(contract.DateAccord, out var dtAccordMonth) ? dtAccordMonth.Month : 0 },
                    new EacParameter("P_JOURACCORD", DbType.Int32) { Value =  DateTime.TryParse(contract.DateAccord, out var dtAccordDay) ? dtAccordDay.Day : 0 },
                    new EacParameter("P_HEURENOW", DbType.Int32) { Value = AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(DateTime.Now)) },
                    new EacParameter("P_MOTCLE1", contract.MotsCles.Count > 0 ? contract.MotsCles[0].Code : string.Empty),
                    new EacParameter("P_MOTCLE2", contract.MotsCles.Count > 1 ? contract.MotsCles[1].Code : string.Empty),
                    new EacParameter("P_MOTCLE3", contract.MotsCles.Count > 2 ? contract.MotsCles[2].Code : string.Empty),
                    new EacParameter("P_SOUSCRIPTEUR", contract.Souscripteur),
                    new EacParameter("P_GESTIONNAIRE", contract.Gestionnaire),
                    new EacParameter("P_ANNEEEFFET", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Debut, out var dtEffetYear) ? dtEffetYear.Year : 0 },
                    new EacParameter("P_MOISEFFET", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Debut, out var dtEffetMonth) ? dtEffetMonth.Month : 0 },
                    new EacParameter("P_JOUREFFET", DbType.Int32) { Value =  DateTime.TryParse(contract.DateEffet.Debut, out var dtEffetDay) ? dtEffetDay.Day : 0 },
                    new EacParameter("P_HEUREEFFET", DbType.Int32) { Value =  0 },
                    new EacParameter("P_COURTIERAPP", DbType.Int32) { Value = Convert.ToInt32(contract.Courtier.Apporteur.Code) },
                    new EacParameter("P_TYPECONTRAT", "S"),
                    new EacParameter("P_USER", user),
                    new EacParameter("P_COURTIERPAY", DbType.Int32) { Value = Convert.ToInt32(contract.Courtier.Payeur.Code) },
                    new EacParameter("P_CODEREMP", "N"),
                    new EacParameter("P_ADRCHR", DbType.Int32) { Value = 0 },
                    new EacParameter("P_OBSERVATION", ""),
                    new EacParameter("P_OBSVCHR", DbType.Int32) { Value = 0 },
                    new EacParameter("P_ENCAISSEMENT", contract.Courtier.Gestionnaire.Quittancement),
                    new EacParameter("P_CONTRATREMPLACE", "N"),
                    new EacParameter("P_CONXCHR", DbType.Int32) { Value = 0 },
                    new EacParameter("P_CODEREMPLACE", ""),
                    new EacParameter("P_VERSIONREMPLACE", DbType.Int32) { Value = 0 },
                    new EacParameter("P_BRANCHEREMPLACE", ""),
                    new EacParameter("P_INSADR", !string.IsNullOrEmpty(contract.Adresse.Numero) ? "O" : "N"),
                    new EacParameter("P_ADRBATIMENT", ""),
                    new EacParameter("P_ADRNUMVOIE", contract.Adresse.Numero.Split(new char[] {'/', '-'})[0]),
                    new EacParameter("P_ADRNUMVOIE2", contract.Adresse.Numero.Contains('/') || contract.Adresse.Numero.Contains('-') ? contract.Adresse.Numero.Split(new char[] {'/', '-'})[1] : ""),
                    new EacParameter("P_ADREXTVOIE", contract.Adresse.Extension),
                    new EacParameter("P_ADRNOMVOIE", contract.Adresse.Rue.ToUpper()),
                    new EacParameter("P_ADRBP", ""),
                    new EacParameter("P_ADRCP", contract.Adresse.CodePostal.Length == 5 ? contract.Adresse.CodePostal.Substring(2, 3) : string.Empty),
                    new EacParameter("P_ADRDEP", contract.Adresse.CodePostal.Length == 5 ? contract.Adresse.CodePostal.Substring(0, 2) : string.Empty),
                    new EacParameter("P_ADRVILLE", contract.Adresse.Ville.ToUpper()),
                    new EacParameter("P_ADRCPX", contract.Adresse.CodePostal.Length == 5 ? contract.Adresse.CodePostal.Substring(2, 3) : string.Empty),
                    new EacParameter("P_ADRVILLEX",  contract.Adresse.Ville.ToUpper()),
                    new EacParameter("P_ADRMATHEX", DbType.Int32) { Value = 0 },
                    new EacParameter("P_PRENEURESTASSURE", "O"),
                    new EacParameter("P_AVENANT", DbType.Int32) { Value = 0 },
                    new EacParameter("P_TYPEAVT", ""),
                    new EacParameter("P_ANNEEEFFETAVENANT", DbType.Int32) { Value = 0 },
                    new EacParameter("P_MOISEFFETAVENANT", DbType.Int32) { Value = 0 },
                    new EacParameter("P_JOUREFFETAVENANT", DbType.Int32) { Value = 0 },
                    new EacParameter("P_HEUREEFFETAVENANT", DbType.Int32) { Value = 0 },
                    new EacParameter("P_ISADDRESSEMPTY", DbType.Int32) { Value = !string.IsNullOrEmpty(contract.Adresse.Numero) ? 0 : 1 },
                    new EacParameter("P_ADRLATITUDE", DbType.Int32) { Value = 0 },
                    new EacParameter("P_ADRLONGITUDE", DbType.Int32) { Value = 0 },
                    new EacParameter("P_INSRSQOBJ", DbType.Int32) { Value = 0 },
                    new EacParameter("P_OUTCODECONTRAT", "")
                };
                dbOptions.ExecStoredProcedure();
                int result = dbOptions.ReturnedValue;
            }
        }

        public void CreationOfferKheops(ContractJsonDto contract, string user)
        {
            using (var dbOptions = new DbSPOptions(this.connection == null)
            {
                SqlText = "ECM_CREATEOFFRE",
                DbConnection = this.connection
            })
            {
                dbOptions.Parameters = new[] {
                    new EacParameter("P_CODEOFFRE", contract.CodeAffaire.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = Convert.ToInt32(contract.Aliment) },
                    new EacParameter("P_TYPE", AlbConstantesMetiers.TYPE_OFFRE),
                    new EacParameter("P_INTERLOCUTEUR", DbType.Int32) { Value = int.TryParse(contract.Courtier.Gestionnaire.Interlocuteur.Code, out var cd) ? cd : 0 },
                    new EacParameter("P_DESCRIPTIF", contract.Designation),
                    new EacParameter("P_MOTCLE1", contract.MotsCles.Count > 0 ? contract.MotsCles[0].Code : string.Empty),
                    new EacParameter("P_MOTCLE2", contract.MotsCles.Count > 1 ? contract.MotsCles[1].Code : string.Empty),
                    new EacParameter("P_MOTCLE3", contract.MotsCles.Count > 2 ? contract.MotsCles[2].Code : string.Empty),
                    new EacParameter("P_CODEPRENEURASSU", DbType.Int32) { Value = Convert.ToInt32(contract.Assure.Code) },
                    new EacParameter("P_COURTIERGESTCODE", DbType.Int32) { Value = Convert.ToInt32(contract.Courtier.Gestionnaire.Code) },
                    new EacParameter("P_COURTIERAPPCODE", DbType.Int32) { Value = Convert.ToInt32(contract.Courtier.Apporteur.Code) },
                    new EacParameter("P_REFCOURTIER", ""),
                    new EacParameter("P_BRANCHE", contract.Branche.Code),
                    new EacParameter("P_USER", user),
                    new EacParameter("P_YEARNOW", DbType.Int32) { Value = DateTime.Now.Year },
                    new EacParameter("P_MONTHNOW", DbType.Int32) { Value = DateTime.Now.Month },
                    new EacParameter("P_DAYNOW", DbType.Int32) { Value = DateTime.Now.Day },
                    new EacParameter("P_SOUSCRIPTEURCODE", contract.Souscripteur),
                    new EacParameter("P_GESTIONNAIRECODE", contract.Gestionnaire),
                    new EacParameter("P_YEARSAISIE", DbType.Int32) { Value = DateTime.Now.Year },
                    new EacParameter("P_MONTHSAISIE", DbType.Int32) { Value = DateTime.Now.Month },
                    new EacParameter("P_DAYSAISIE", DbType.Int32) { Value = DateTime.Now.Day },
                    new EacParameter("P_HOURSAISIE",DbType.Int32) { Value = DateTime.Now.Hour },
                    new EacParameter("P_OBSERVATION", ""),
                    new EacParameter("P_CIBLE", contract.Branche.Cible.Code),
                    new EacParameter("P_INDICEREF", contract.IndiceRef.Code),
                    new EacParameter("P_VALEUR", DbType.Decimal) { Value = decimal.TryParse(contract.IndiceRef.Valeur, out var v) ? v : 0 },
                    new EacParameter("P_INTERCALAIRE", contract.IntercalaireCourtier),
                    new EacParameter("P_CATNAT", contract.CATNAT),
                    new EacParameter("P_DATENOW", DbType.Int32) { Value = DateTime.Now.ToIntYMD() },
                    new EacParameter("P_HOURNOW", DbType.Int32) { Value = DateTime.Now.Hour },
                    new EacParameter("P_NATURECONTRAT", ""), //TODO
                    new EacParameter("P_APERITEURCODE", ""), //TODO
                    new EacParameter("P_PARTAPERITEUR", DbType.Decimal) { Value = 0 }, //TODO
                    new EacParameter("P_FRAISAPERITION", DbType.Decimal) { Value = 0 }, //TODO
                    new EacParameter("P_HASADRESSE", contract.Adresse == null ? "N" : "O"),
                    new EacParameter("P_ADRCHRONO", DbType.Int32) { Value = 0 },
                    new EacParameter("P_BATIMENT", ""),
                    new EacParameter("P_NUMVOIE", contract.Adresse.Numero),
                    new EacParameter("P_EXTVOIE", contract.Adresse.Extension),
                    new EacParameter("P_NOMVOIE", contract.Adresse.Rue.ToUpper()),
                    new EacParameter("P_BP", ""),
                    new EacParameter("P_LOC", ""),
                    new EacParameter("P_DEPARTEMENT", contract.Adresse.CodePostal.Length == 5 ? contract.Adresse.CodePostal.Substring(0, 2) : string.Empty),
                    new EacParameter("P_CP", contract.Adresse.CodePostal.Length == 5 ? contract.Adresse.CodePostal.Substring(2, 3) : string.Empty),
                    new EacParameter("P_VILLE", contract.Adresse.Ville.ToUpper()),
                    new EacParameter("P_VOIECOMPLETE", ""), //TODO
                    new EacParameter("P_VILLECOMPLETE", ""), //TODO
                    new EacParameter("P_CPCDX", contract.Adresse.CodePostal.Length == 5 ? contract.Adresse.CodePostal.Substring(2, 3) : string.Empty),
                    new EacParameter("P_VILLECDX",  contract.Adresse.Ville.ToUpper()),
                    new EacParameter("P_MATRICULEHEX", DbType.Int32) { Value = 0 },
                    new EacParameter("P_DATESAISIE", DbType.Int32) { Value = DateTime.Now.ToIntYMD() },
                    new EacParameter("P_PRENEURESTASSURE", "O"),
                    new EacParameter("P_ISADDRESSEMPTY", DbType.Int32) { Value = !string.IsNullOrEmpty(contract.Adresse.Numero) ? 0 : 1 },
                    new EacParameter("P_LATITUDE", DbType.Int32) { Value = 0 },
                    new EacParameter("P_LONGITUDE", DbType.Int32) { Value = 0 },
                    new EacParameter("P_INSRSQOBJ", DbType.Int32) { Value = 0 },
                    new EacParameter("P_MSGERROR", "")
                };
                dbOptions.ExecStoredProcedure();
                int result = dbOptions.ReturnedValue;
            }
        }

        public void ModificationOfferKheops(ContractJsonDto contract, string user)
        {
            using (var dbOptions = new DbSPOptions(this.connection == null)
            {
                SqlText = "SP_MODIFIEROFFRE",
                DbConnection = this.connection
            })
            {
                dbOptions.Parameters = new[] {
                    new EacParameter("P_CODE", contract.CodeAffaire.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = Convert.ToInt32(contract.Aliment) },
                    new EacParameter("P_TYPE", AlbConstantesMetiers.TYPE_OFFRE),
                    new EacParameter("P_REFERENCE", contract.Designation),
                    new EacParameter("P_MOTCLE1", contract.MotsCles.Count > 0 ? contract.MotsCles[0].Code : string.Empty),
                    new EacParameter("P_MOTCLE2", contract.MotsCles.Count > 1 ? contract.MotsCles[1].Code : string.Empty),
                    new EacParameter("P_MOTCLE3", contract.MotsCles.Count > 2 ? contract.MotsCles[2].Code : string.Empty),
                    new EacParameter("P_CODEDEVISE", contract.Devise != null ? contract.Devise.Code : string.Empty),
                    new EacParameter("P_PERIODICITE", contract.Periodicite != null ? contract.Periodicite.Code : string.Empty),
                    new EacParameter("P_ECHEANCEJOUR", DbType.Int32) { Value = Int32.TryParse(contract.EcheancePrincipale.Split('/').FirstOrDefault(), out var dtEchanceDay) ? dtEchanceDay : 0 },
                    new EacParameter("P_ECHEANCEMOIS", DbType.Int32) { Value = Int32.TryParse(contract.EcheancePrincipale.Split('/').LastOrDefault(), out var dtEcheanceMonth) ? dtEcheanceMonth : 0 },
                    new EacParameter("P_EFFETJOUR", DbType.Int32) { Value =  DateTime.TryParse(contract.DateEffet.Debut, out var dtEffetDay) ? dtEffetDay.Day : 0 },
                    new EacParameter("P_EFFETMOIS", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Debut, out var dtEffetMonth) ? dtEffetMonth.Month : 0 },
                    new EacParameter("P_EFFETANNEE", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Debut, out var dtEffetYear) ? dtEffetYear.Year : 0 },
                    new EacParameter("P_EFFETHEURE", DbType.Int32) { Value =  0 },
                    new EacParameter("P_FINEFFETJOUR", DbType.Int32) { Value =  DateTime.TryParse(contract.DateEffet.Fin, out var dtFinEffetDay) ? dtFinEffetDay.Day : 0 },
                    new EacParameter("P_FINEFFETMOIS", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Fin, out var dtFinEffetMonth) ? dtFinEffetMonth.Month : 0 },
                    new EacParameter("P_FINEFFETANNEE", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Fin, out var dtFinEffetYear) ? dtFinEffetYear.Year : 0 },
                    new EacParameter("P_FINEFFETHEURE", DbType.Int32) { Value =  0 },
                    new EacParameter("P_DUREE", DbType.Int32) { Value = 0 },
                    new EacParameter("P_UNITEDUREE", ""),
                    new EacParameter("P_NATURE", ""),
                    new EacParameter("P_APERITION", DbType.Decimal) { Value = Decimal.TryParse(contract.PartAlbingia, out var part) ? part : 0 },
                    new EacParameter("P_COUVERT", DbType.Decimal) { Value = 0 },
                    new EacParameter("P_SOUSCRIPTEUR", contract.Souscripteur),
                    new EacParameter("P_GESTIONNAIRE", contract.Gestionnaire),
                    new EacParameter("P_REGIMETAXE", "1"),
                    new EacParameter("P_IDASSURE", DbType.Int32) { Value = Convert.ToInt32(contract.Assure.Code) },
                    new EacParameter("P_COURTIER", DbType.Int32) { Value = Convert.ToInt32(contract.Courtier.Gestionnaire.Code) },
                    new EacParameter("P_COURTIERAPP", DbType.Int32) { Value = Convert.ToInt32(contract.Courtier.Apporteur.Code) },
                    new EacParameter("P_REFCOURTIER", ""),
                    new EacParameter("P_INTERLOCUTEUR", DbType.Int32) { Value = int.TryParse(contract.Courtier.Gestionnaire.Interlocuteur.Code, out var cd) ? cd : 0 },
                    new EacParameter("P_TYPEINTERLOCUTEUR", ""),
                    new EacParameter("P_BRANCHE", contract.Branche.Code),
                    new EacParameter("P_ANNEESAISIE", DbType.Int32) { Value = DateTime.Now.Year },
                    new EacParameter("P_MOISSAISIE", DbType.Int32) { Value = DateTime.Now.Month },
                    new EacParameter("P_JOURSAISIE", DbType.Int32) { Value = DateTime.Now.Day },
                    new EacParameter("P_HEURESAISIE",DbType.Int32) { Value = DateTime.Now.Hour },
                    new EacParameter("P_ETAT", ""),
                    new EacParameter("P_TYPETRAITEMENT", ""),
                    new EacParameter("P_CIBLE", contract.Branche.Cible.Code),
                    new EacParameter("P_LIENKPINVEN", DbType.Int32) { Value = 0 },
                    new EacParameter("P_LIENKPDESI", DbType.Int32) { Value = 0 },
                    new EacParameter("P_LIENKPOBSV", DbType.Int32) { Value = 0 },
                    new EacParameter("P_OBSERVATION", ""),
                    new EacParameter("P_OBSVCHRONO", DbType.Int32) { Value = CommonRepository.GetAS400Id("KAJCHR") },
                    new EacParameter("P_CODEINDICE", contract.IndiceRef.Code),
                    new EacParameter("P_VALINDICEORIGINE", DbType.Decimal) { Value = decimal.TryParse(contract.IndiceRef.Valeur, out var v) ? v : 0 },
                    new EacParameter("P_VALINDICEACTUALISE", DbType.Decimal) { Value = decimal.TryParse(contract.IndiceRef.Valeur, out var va) ? v : 0 },
                    new EacParameter("P_INTERCALCOURTIER", contract.IntercalaireCourtier),
                    new EacParameter("P_APPLICATNAT", contract.CATNAT),
                    new EacParameter("P_IDAPERITEUR", ""),
                    new EacParameter("P_POURCENTAPERITION", DbType.Decimal) { Value = 0 },
                    new EacParameter("P_TAUXFRAISAPERITION", DbType.Decimal) { Value = 0 },
                    new EacParameter("P_AVENANT", DbType.Int32) { Value = 0 },
                    new EacParameter("P_TYPEACTEGESTION", ""),
                    new EacParameter("P_TODAYYEAR", DbType.Int32) { Value = DateTime.Now.Year },
                    new EacParameter("P_TODAYMONTH", DbType.Int32) { Value = DateTime.Now.Month },
                    new EacParameter("P_TODAYDAY", DbType.Int32) { Value = DateTime.Now.Day },
                    new EacParameter("P_TODAYHOUR", DbType.Int32) { Value = DateTime.Now.Hour },
                    new EacParameter("P_USERTODAY", user),
                    new EacParameter("P_DATESTATISTIQUE", DbType.Int32) { Value = 0 },
                    new EacParameter("P_LTA", ""),
                };
                dbOptions.ExecStoredProcedure();
                int result = dbOptions.ReturnedValue;
            }
        }

        public void UpdBaseContractKheops(ContractJsonDto contract, string user)
        {
            var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = UpdInfoBaseContract
            };
            var parameters = new[] {
                new EacParameter("ECM", DbType.Int32) { Value = DateTime.TryParse(contract.EcheancePrincipale, out var dtEchPrinMonth) ? dtEchPrinMonth.Month : 0 },
                new EacParameter("ECJ", DbType.Int32) { Value = DateTime.TryParse(contract.EcheancePrincipale, out var dtEchPrinDay) ? dtEchPrinDay.Day : 0 },
                new EacParameter("EFA", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Debut, out var dtEffetYear) ? dtEffetYear.Year : 0 },
                new EacParameter("EFM", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Debut, out var dtEffetMonth) ? dtEffetMonth.Month : 0 },
                new EacParameter("EFJ", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Debut, out var dtEffetDay) ? dtEffetDay.Day : 0 },
                new EacParameter("FEA", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Fin, out var dtFinYear) ? dtFinYear.Year : 0 },
                new EacParameter("FEM", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Fin, out var dtFinMonth) ? dtFinMonth.Month : 0 },
                new EacParameter("FEJ", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Fin, out var dtFinDay) ? dtFinDay.Day : 0 },
                new EacParameter("FEH", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Fin, out var dtFinHour) ? 2359 : 0 },
                new EacParameter("AVA", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Debut, out var dtAvnYear) ? dtAvnYear.Year : 0 },
                new EacParameter("AVM", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Debut, out var dtAvnMonth) ? dtAvnMonth.Month : 0 },
                new EacParameter("AVJ", DbType.Int32) { Value = DateTime.TryParse(contract.DateEffet.Debut, out var dtAvnDay) ? dtAvnDay.Day : 0 },
                new EacParameter("PER", contract.Periodicite.Code),
                new EacParameter("IPB", contract.CodeAffaire.ToIPB()),
                new EacParameter("ALX", DbType.Int32) { Value = int.Parse(contract.Aliment) },
                new EacParameter("TYP", contract.Type)
            };
            options.Parameters = parameters;
            options.Exec();
        }

        public void UpdCommission(ContractJsonDto contract, CommissionCourtierDto commission, string user)
        {
            var isTauxForce = contract.Commissions.TauxHCATNAT != commission.TauxStandardHCAT.ToString()
                || contract.Commissions.TauxCATNAT != commission.TauxStandardCAT.ToString();

            using (var dbOptions = new DbSPOptions(this.connection == null)
            {
                SqlText = "SP_UPDCOMM",
                DbConnection = this.connection
            })
            {
                dbOptions.Parameters = new[]
                {
                new EacParameter("P_ID_OFFRE", contract.CodeAffaire.ToIPB()),
                new EacParameter("P_VERSION_OFFRE", DbType.Int32) { Value = int.Parse(contract.Aliment) },
                new EacParameter("P_TYPE_OFFRE", contract.Type),
                new EacParameter("P_TAUX_CONTRAT_HCAT", DbType.Double) { Value = contract.Commissions.TauxHCATNAT},
                new EacParameter("P_TAUX_CONTRAT_CAT", DbType.Double) { Value = contract.Commissions.TauxCATNAT },
                new EacParameter("P_IS_HORSCATNAT", contract.Commissions.TauxHCATNAT == commission.TauxStandardHCAT.ToString() ? "O" : "N"),
                new EacParameter("P_IS_CATNAT", contract.Commissions.TauxCATNAT == commission.TauxStandardCAT.ToString() ? "O" : "N"),
                new EacParameter("P_OBSV", isTauxForce ? "Commission forcée EQDOM" : string.Empty)
            };
                dbOptions.ExecStoredProcedure();
            }

            //CommonRepository.LoadAS400Commissions(codeContrat, versionContrat, type, codeAvn, user, acteGestion);
        }

        public void UpdEnteteContractKheops(ContractJsonDto contract, string user)
        {
            //Partie YPRTENT
            var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = UpdEnteteContract
            };

            var debPeriod = contract.Periodicite.Code == "E" || contract.Periodicite.Code == "U" ? contract.DateEffet.Debut : contract.DebutPeriode;
            var finPeriod = contract.Periodicite.Code == "E" || contract.Periodicite.Code == "U" ? contract.DateEffet.Fin : contract.FinPeriode;

            var parameters = new[] {
                new EacParameter("DPV", DbType.Int32) { Value = int.TryParse(contract.Preavis, out var preavis) ? preavis : 0 },
                new EacParameter("IND", contract.IndiceRef.Code) ,
                new EacParameter("IVA", DbType.Decimal) { Value = decimal.TryParse(contract.IndiceRef.Valeur, out var indRef) ? indRef : 0 },
                new EacParameter("ITC", contract.IntercalaireFiles.Count > 0 ? "O" : "N"),
                new EacParameter("CNA", contract.CATNAT),
                new EacParameter("DPJ", DbType.Int32) { Value = DateTime.TryParse(debPeriod, out var dtPeriodDebDay) ? dtPeriodDebDay.Day : 0 },
                new EacParameter("DPM", DbType.Int32) { Value = DateTime.TryParse(debPeriod, out var dtPeriodDebMonth) ? dtPeriodDebMonth.Month : 0 },
                new EacParameter("DPA", DbType.Int32) { Value = DateTime.TryParse(debPeriod, out var dtPeriodDebYear) ? dtPeriodDebYear.Year : 0 },
                new EacParameter("FPJ", DbType.Int32) { Value = DateTime.TryParse(finPeriod, out var dtPeriodFinDay) ? dtPeriodFinDay.Day : 0 },
                new EacParameter("FPM", DbType.Int32) { Value = DateTime.TryParse(finPeriod, out var dtPeriodFinMonth) ? dtPeriodFinMonth.Month : 0 },
                new EacParameter("FPA", DbType.Int32) { Value = DateTime.TryParse(finPeriod, out var dtPeriodFinYear) ? dtPeriodFinYear.Year : 0 },
                new EacParameter("PEJ", DbType.Int32) { Value = DateTime.TryParse(contract.ProchaineEcheance, out var dtProchEchDay) ? dtProchEchDay.Day : 0 },
                new EacParameter("PEM", DbType.Int32) { Value = DateTime.TryParse(contract.ProchaineEcheance, out var dtProchEchMonth) ? dtProchEchMonth.Month : 0 },
                new EacParameter("PEA", DbType.Int32) { Value = DateTime.TryParse(contract.ProchaineEcheance, out var dtProchEchYear) ? dtProchEchYear.Year : 0 },
                new EacParameter("INA", !contract.IndiceRef.Valeur.IsEmptyOrNull() ? "O" : "N"),
                new EacParameter("IVO", DbType.Decimal) { Value = decimal.TryParse(contract.IndiceRef.Valeur, out var indRefO) ? indRefO : 0 },
                new EacParameter("IVW", DbType.Int32) { Value = 0 },
                new EacParameter("LTA", "N"),
                new EacParameter("IPB", contract.CodeAffaire.ToIPB()),
                new EacParameter("ALX", DbType.Int32) { Value = int.Parse(contract.Aliment) }
            };
            options.Parameters = parameters;
            options.Exec();

            //Partie KPENT
            options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = UpdKpentContract
            };

            parameters = new[]
            {
                new EacParameter("STA", DbType.Int32) { Value = DateTime.TryParse(contract.DateAccord, out var dtSta) ? dtSta.ToIntYMD() : 0},
                new EacParameter("IPB", contract.CodeAffaire.ToIPB()),
                new EacParameter("ALX", DbType.Int32) { Value = int.Parse(contract.Aliment) },
                new EacParameter("TYP", "P")
            };

            options.Parameters = parameters;
            options.Exec();
        }

        public void UpdObjContractKheops(ContractJsonDto contract, string user)
        {
            var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = UpdObjContract
            };

            var parameters = new[]
            {
                new EacParameter("IVO", DbType.Decimal) { Value = decimal.TryParse(contract.IndiceRef.Valeur, out var ind) ? ind : 0 },
                new EacParameter("IVA", DbType.Decimal) { Value = decimal.TryParse(contract.IndiceRef.Valeur, out var ind2) ? ind2 : 0 },
                new EacParameter("IVW", DbType.Decimal) { Value = 0 },
                new EacParameter("IPB", contract.CodeAffaire.ToIPB()),
                new EacParameter("ALX", DbType.Int32) { Value = int.Parse(contract.Aliment) }
            };
            options.Parameters = parameters;
            options.Exec();
        }

        public void SaveActeGestionContract(ContractJsonDto contract, string user)
        {
            var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = SaveActeGestion
            };

            var parameters = new[]
            {
                new EacParameter("TYP", "P"),
                new EacParameter("IPB", contract.CodeAffaire.ToIPB()),
                new EacParameter("ALX", DbType.Int32) { Value = int.Parse(contract.Aliment) },
                new EacParameter("AVN", DbType.Int32) { Value = 0 },
                new EacParameter("TTR", "AFFNV"),
                new EacParameter("VAG", "AFFNV"),
                new EacParameter("TRA", DbType.Int32) { Value = DateTime.Now.Year },
                new EacParameter("TRM", DbType.Int32) { Value = DateTime.Now.Month },
                new EacParameter("TRJ", DbType.Int32) { Value = DateTime.Now.Day },
                new EacParameter("TRH", DbType.Int32) { Value = AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(DateTime.Now)) },
                new EacParameter("LIB", ""),
                new EacParameter("INF", "I"),
                new EacParameter("SDA", DbType.Int32) { Value = 0 },
                new EacParameter("SDM", DbType.Int32) { Value = 0 },
                new EacParameter("SDJ", DbType.Int32) { Value = 0 },
                new EacParameter("SFA", DbType.Int32) { Value = 0 },
                new EacParameter("SFM", DbType.Int32) { Value = 0 },
                new EacParameter("SFJ", DbType.Int32) { Value = 0 },
                new EacParameter("MJU", user),
                new EacParameter("MJA", DbType.Int32) { Value = DateTime.Now.Year },
                new EacParameter("MJM", DbType.Int32) { Value = DateTime.Now.Month },
                new EacParameter("MJJ", DbType.Int32) { Value = DateTime.Now.Day },
                new EacParameter("MJH", DbType.Int32) { Value = AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(DateTime.Now)) }
            };

            options.Parameters = parameters;
            options.Exec();
        }

        public void GenerateRsqObjContract(ContractJsonDto contract, Risque risque, string user)
        {
            var codeRsq = "0";

            using (var options = new DbSPOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = "SP_SAVEOBJ"
            })
            {
                foreach (var objet in risque.Objets)
                {
                    var retCodeRsq = new EacParameter("P_OUTCODERSQOBJ", "");
                    retCodeRsq.Direction = ParameterDirection.Output;
                    options.Parameters = new[]
                    {
                        new EacParameter("P_CODEOFFRE", contract.CodeAffaire.ToIPB()),
                        new EacParameter("P_VERSION", DbType.Int32) { Value = int.Parse(contract.Aliment) },
                        new EacParameter("P_TYPE", contract.Type),
                        new EacParameter("P_CODERSQ", DbType.Int32) { Value = int.Parse(codeRsq) },
                        new EacParameter("P_CODEOBJ", DbType.Int32) { Value = 0 },
                        new EacParameter("P_CHRONODESI", DbType.Int32) { Value = 0 },
                        new EacParameter("P_DESIGNATION", ""),
                        new EacParameter("P_ENTREEJOUR", DbType.Int32) { Value = DateTime.TryParse(objet.PeriodeGarantie.Debut, out var dtDebDay) ? dtDebDay.Day : 0 },
                        new EacParameter("P_ENTREEMOIS", DbType.Int32) { Value = DateTime.TryParse(objet.PeriodeGarantie.Debut, out var dtDebMonth) ? dtDebMonth.Month : 0 },
                        new EacParameter("P_ENTREEANNEE", DbType.Int32) { Value = DateTime.TryParse(objet.PeriodeGarantie.Debut, out var dtDebYear) ? dtDebYear.Year : 0 },
                        new EacParameter("P_ENTREEHEURE", DbType.Int32) { Value = 0 },
                        new EacParameter("P_SORTIEJOUR", DbType.Int32) { Value =  DateTime.TryParse(objet.PeriodeGarantie.Fin, out var dtFinDay) ? dtFinDay.Day : 0 },
                        new EacParameter("P_SORTIEMOIS", DbType.Int32) { Value =  DateTime.TryParse(objet.PeriodeGarantie.Fin, out var dtFinMonth) ? dtFinMonth.Month : 0 },
                        new EacParameter("P_SORTIEANNEE", DbType.Int32) { Value = DateTime.TryParse(objet.PeriodeGarantie.Fin, out var dtFinYear) ? dtFinYear.Year : 0 },
                        new EacParameter("P_SORTIEHEURE", DbType.Int32) { Value =  0 },
                        new EacParameter("P_VALEUR", DbType.Decimal) { Value = int.TryParse(objet.Valeur, out var val) ? val : 0 },
                        new EacParameter("P_CODEUNITE", objet.Unite),
                        new EacParameter("P_CODETYPE", objet.Type),
                        new EacParameter("P_VALEURHT", objet.ValeurHT),
                        new EacParameter("P_COUTM2", DbType.Int64) { Value = decimal.TryParse(objet.CoutM2, out var cout) ? cout : 0 },
                        new EacParameter("P_CODEBRANCHE", contract.Branche.Code),
                        new EacParameter("P_CODEOBJET", DbType.Int32) { Value = 1 },
                        new EacParameter("P_DERNIEROBJET", DbType.Int32) { Value = 1 },
                        new EacParameter("P_NBOBJET", DbType.Int32) { Value = 1 },
                        new EacParameter("P_CIBLE", contract.Branche.Cible.Code),
                        new EacParameter("P_DESCRIPTIF", objet.Designation),
                        new EacParameter("P_REPORTVALEUR", ""),
                        new EacParameter("P_REPORTOBLIG", ""),
                        new EacParameter("P_INSADR", !string.IsNullOrEmpty(contract.Adresse.Numero) ? "O" : "N"),
                        new EacParameter("P_ADRBATIMENT", ""),
                        new EacParameter("P_ADRNUMVOIE", contract.Adresse.Numero),
                        new EacParameter("P_ADRNUMVOIE2", ""),
                        new EacParameter("P_ADREXTVOIE", contract.Adresse.Extension),
                        new EacParameter("P_ADRNOMVOIE", contract.Adresse.Rue.ToUpper()),
                        new EacParameter("P_ADRBP", ""),
                        new EacParameter("P_ADRCP", contract.Adresse.CodePostal.Length == 5 ? contract.Adresse.CodePostal.Substring(2, 3) : string.Empty),
                        new EacParameter("P_ADRDEP", contract.Adresse.CodePostal.Length == 5 ? contract.Adresse.CodePostal.Substring(0, 2) : string.Empty),
                        new EacParameter("P_ADRVILLE", contract.Adresse.Ville.ToUpper()),
                        new EacParameter("P_ADRCPX", contract.Adresse.CodePostal.Length == 5 ? contract.Adresse.CodePostal.Substring(2, 3) : string.Empty),
                        new EacParameter("P_ADRVILLEX", contract.Adresse.Ville.ToUpper()),
                        new EacParameter("P_ADRMATHEX", DbType.Int32) { Value = 0 },
                        new EacParameter("P_ADRNUMCHRONO", DbType.Int32) { Value = 0 },
                        new EacParameter("P_APE", ""),
                        new EacParameter("P_NOMENCLATURE1", objet.Nomenclatures[0].Code),
                        new EacParameter("P_NOMENCLATURE2", objet.Nomenclatures[1].Code),
                        new EacParameter("P_NOMENCLATURE3", objet.Nomenclatures[2].Code),
                        new EacParameter("P_NOMENCLATURE4", objet.Nomenclatures[3].Code),
                        new EacParameter("P_NOMENCLATURE5", objet.Nomenclatures[4].Code),
                        new EacParameter("P_TERRITORIALITE", objet.Territorialite.Code),
                        new EacParameter("P_TRE", ""),
                        new EacParameter("P_CLASSE", ""),
                        new EacParameter("P_TYPERISQUE", ""),
                        new EacParameter("P_TYPEMATERIEL", ""),
                        new EacParameter("P_NATURELIEUX", ""),
                        new EacParameter("P_DATEENTREEDESC", DbType.Int32) { Value = 0 },
                        new EacParameter("P_HEUREENTREEDESC", DbType.Int32) { Value = 0 },
                        new EacParameter("P_DATESORTIEDESC", DbType.Int32) { Value = 0 },
                        new EacParameter("P_HEURESORTIEDESC", DbType.Int32) { Value = 0 },
                        new EacParameter("P_MODAVENANTLOCALE", "N"),
                        new EacParameter("P_DATEEFFETAVNLOCALANNEE", DbType.Int32) { Value = 0 },
                        new EacParameter("P_DATEEFFETAVNLOCALMOIS", DbType.Int32) { Value = 0 },
                        new EacParameter("P_DATEEFFETAVNLOCALJOUR", DbType.Int32) { Value = 0 },
                        new EacParameter("P_ISRISQUETEMPORAIRE", "N"),
                        new EacParameter("P_DATESYSTEM", DbType.Decimal) { Value = DateTime.Now.ToString("yyyyMMdd") },
                        new EacParameter("P_HEURESYSTEM", DbType.Decimal) { Value = DateTime.Now.ToString("HHmmss") },
                        new EacParameter("P_USER", user),
                        new EacParameter("P_ISADDRESSEMPTY", DbType.Int32) { Value = !string.IsNullOrEmpty(contract.Adresse.Numero) ? 0 : 1 },
                        new EacParameter("P_LATITUDE", DbType.Decimal) { Value = 0 },
                        new EacParameter("P_LONGITUDE", DbType.Decimal) { Value = 0 },
                        retCodeRsq
                    };
                    options.ExecStoredProcedure();
                    codeRsq = retCodeRsq.Value.ToString().Split('_')[0];
                }
            };
        }

        public void UpdateLCIComplexe(ContractJsonDto contract, IEnumerable<Garanty> allGarantiesLCI)
        {
            foreach (var form in allGarantiesLCI)
            {
                var options = new DbExecOptions(this.connection == null)
                {
                    DbConnection = this.connection,
                    SqlText = UpdLCIComplexe
                };

                var parameters = new[]
                {
                    new EacParameter("VAL", DbType.Decimal) { Value = decimal.TryParse(form.LCI.Valeur, out var lciVal) ? lciVal : 0 },
                    new EacParameter("IPB", contract.CodeAffaire.ToIPB()),
                    new EacParameter("ALX", DbType.Int32) { Value = int.Parse(contract.Aliment)},
                    new EacParameter("TYP", contract.Type),
                    new EacParameter("LCI", form.LCI.Type)
                };
                options.Parameters = parameters;
                options.Exec();
            }
        }

        public decimal GetMontantCalcule(PGMFolder folder, ContractJsonDto contract)
        {
            var retVal = Fetch<decimal>(SelectMontantCalcule, folder.CodeOffre.ToIPB(), folder.Version, folder.NumeroAvenant)?.FirstOrDefault();
            return retVal ?? 0;
        }

        public bool IsFolderAttentat(PGMFolder folder, ContractJsonDto contract)
        {
            var attentat = "";

            using (var dbOptions = new DbSelectStringsOptions(this.connection == null)
            {
                SqlText = SelectAttentat,
                DbConnection = this.connection
            })
            {
                dbOptions.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version);
                dbOptions.PerformSelect();
                attentat = dbOptions.StringList.FirstOrDefault();
            }
            if (!attentat.IsEmptyOrNull())
                return attentat == "O";

            return false;
        }

        public void DeletePrimesContract(ContractJsonDto contract)
        {
            var parameters = new[] {
                new EacParameter("IPB", contract.CodeAffaire.ToIPB()),
                new EacParameter("ALX", DbType.Int32) { Value = int.Parse(contract.Aliment) }
            };

            var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = DeletePripesContract
            };

            //Suppression de YPRIPES
            options.Parameters = parameters;
            options.Exec();
            //Suppression de YPRIPGA
            options.SqlText = DeletePripesGAContract;
            options.Exec();
            //Suppression de YPRIPCM
            options.SqlText = DeletePripesCMContract;
            options.Exec();
            //Suppression de YPRIPGK
            options.SqlText = DeletePripesGKContract;
            options.Exec();
            //Suppression de YPRIPPA
            options.SqlText = DeletePripesPAContract;
            options.Exec();
            //Suppression de YPRIPTA
            options.SqlText = DeletePripesTAContract;
            options.Exec();
            //Suppression de YPRIPTG
            options.SqlText = DeletePripesTGContract;
            options.Exec();
            //Suppression de YPRIPTX
            options.SqlText = DeletePripesTXContract;
            options.Exec();
        }

        public void ValidationContract(PGMFolder folder)
        {
            using (var dbOptions = new DbSPOptions(this.connection == null)
            {
                SqlText = "SP_VALIDATIONAFFAIRE",
                DbConnection = this.connection
            })
            {
                dbOptions.Parameters = new[]
                {
                    new EacParameter("P_CODEOFFRE", folder.CodeOffre),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = folder.Version },
                    new EacParameter("P_TYPE", folder.Type),
                    new EacParameter("P_AVENANT", DbType.Int32) { Value = 0 },
                    new EacParameter("P_MODE", "VALIDER"),
                    new EacParameter("P_ETAT", "V"),
                    new EacParameter("P_MOTIF", ""),
                    new EacParameter("P_YEARNOW", DbType.Int32) { Value = DateTime.Now.Year },
                    new EacParameter("P_MONTHNOW", DbType.Int32) { Value = DateTime.Now.Month },
                    new EacParameter("P_DAYNOW", DbType.Int32) { Value = DateTime.Now.Day },
                    new EacParameter("P_HOURNOW", DbType.Int32) { Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(DateTime.Now)) },
                    new EacParameter("P_USER", folder.User),
                    new EacParameter("P_ERREUR", "")
                };
                dbOptions.ExecStoredProcedure();
            }
        }

        public void GenerateDocumentsContract(PGMFolder folder, ContractJsonDto contract)
        {
            using (var dbOptions = new DbSPOptions(this.connection == null)
            {
                SqlText = "SP_GENERATEDOCUMENTSGESTION",
                DbConnection = this.connection
            })
            {
                dbOptions.Parameters = new[]
                {
                    new EacParameter("P_CODEOFFRE", contract.CodeAffaire.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = int.Parse(contract.Aliment) },
                    new EacParameter("P_TYPE", contract.Type),
                    new EacParameter("P_CODEAVN", DbType.Int32) { Value = 0 },
                    new EacParameter("P_SERVICE", "PRODU"),
                    new EacParameter("P_ACTEGES", folder.ActeGestion),
                    new EacParameter("P_USER", folder.User),
                    new EacParameter("P_DATENOW", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(DateTime.Now) },
                    new EacParameter("P_HOURNOW", DbType.Int32) { Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(DateTime.Now)) },
                    new EacParameter("P_INIT", "O"),
                    new EacParameter("P_DOCSID", ""),
                    new EacParameter("P_ATTESID", DbType.Int32) { Value = 0 },
                    new EacParameter("P_REGULID", DbType.Int32) { Value = 0 }
                };
                dbOptions.ExecStoredProcedure();
            }
        }

        public int GetLotDocumentContract(PGMFolder folder)
        {
            return Fetch<int>(GetLotIdDocument, folder.CodeOffre.ToIPB(), folder.Version, folder.Type).FirstOrDefault();
        }

        public void SetTraceContract(TraceDto trace)
        {
            using (var dbOptions = new DbSPOptions(this.connection == null)
            {
                SqlText = "SP_ARBRESV",
                DbConnection = this.connection
            })
            {
                dbOptions.Parameters = new[]
                {
                    new EacParameter("P_TYPE", trace.Type),
                    new EacParameter("P_CODEOFFRE", trace.CodeOffre.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = trace.Version },
                    new EacParameter("P_ETAPE", DbType.Int32) { Value = trace.EtapeGeneration },
                    new EacParameter("P_NUMETAPE", DbType.Int32) { Value = trace.NumeroOrdreDansEtape },
                    new EacParameter("P_ORDRE", DbType.Int32) { Value = trace.NumeroOrdreEtape },
                    new EacParameter("P_PERIMETRE", trace.Perimetre),
                    new EacParameter("P_CODERSQ", DbType.Int32) { Value = trace.Risque },
                    new EacParameter("P_CODEOBJ", DbType.Int32) { Value = trace.Objet },
                    new EacParameter("P_CODEINVEN", DbType.Int32) { Value = trace.IdInventaire },
                    new EacParameter("P_CODEFOR", DbType.Int32) { Value = trace.Formule },
                    new EacParameter("P_CODEOPT", DbType.Int32) { Value = trace.Option },
                    new EacParameter("P_NIVCLSST", trace.Niveau),
                    new EacParameter("P_USER", trace.CreationUser),
                    new EacParameter("P_DATESYSTEME", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(DateTime.Now) },
                    new EacParameter("P_HEURESYSTEME", DbType.Int32) { Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(DateTime.Now)) },
                    new EacParameter("P_PASSTAG", trace.PassageTag),
                    new EacParameter("P_PASSTAGCLAUSE", trace.PassageTagClause)
                };
                dbOptions.ExecStoredProcedure();
            }
        }

        public string GetUrlTypo(string typologie)
        {
            var strTypo = $"DOC_{typologie}";
            var result = "";

            using (var dbOptions = new DbSelectStringsOptions(this.connection == null)
            {
                SqlText = GetUrlTypologie,
                DbConnection = this.connection
            })
            {
                dbOptions.BuildParameters(strTypo);
                dbOptions.PerformSelect();
                result = dbOptions.StringList.FirstOrDefault();
            }

            if (!result.IsEmptyOrNull())
                return result.Replace('/', '\\');

            return string.Empty;
        }

        public int SaveDocsJoints(PGMFolder pgmFolder, ContractJsonDto contract, IntercalaireFile file)
        {
            var idDoc = 0;
            using (var dbOptions = new DbSPOptions(this.connection == null)
            {
                SqlText = "SP_SAVEDOCSJOINTS",
                DbConnection = this.connection
            })
            {
                var retIdDoc = new EacParameter("P_OUTIDDOC", DbType.Int32) { Value = 0 };
                retIdDoc.Direction = ParameterDirection.Output;

                dbOptions.Parameters = new[]
                {
                    new EacParameter("P_CODEOFFRE", contract.CodeAffaire.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = int.Parse(contract.Aliment) },
                    new EacParameter("P_TYPE", contract.Type),
                    new EacParameter("P_IDDOC", DbType.Int32) { Value = 0 },
                    new EacParameter("P_TYPEDOC", "INTER"),
                    new EacParameter("P_TITLEDOC", file.Nom),
                    new EacParameter("P_FILEDOC", file.FileDoc),
                    new EacParameter("P_PATHDOC", file.FilePath),
                    new EacParameter("P_REFDOC", file.Reference),
                    new EacParameter("P_USER", pgmFolder.User),
                    new EacParameter("P_DATENOW", DbType.Int32) { Value = DateTime.Now.ToIntYMD() },
                    new EacParameter("P_HOURNOW", DbType.Int32) { Value = AlbConvert.ToIntHM(DateTime.Now) },
                    new EacParameter("P_ACTEGES", pgmFolder.ActeGestion),
                    retIdDoc
                };
                dbOptions.ExecStoredProcedure();
                idDoc = int.Parse(retIdDoc.Value.ToString());
            }
            return idDoc;
        }

        public void InsertClauseJointe(PGMFolder pgmFolder, ContractJsonDto contract, int idDoc)
        {
            DateTime? dateNow = DateTime.Now;
            using (var dbOptions = new DbSPOptions(this.connection == null)
            {
                SqlText = "SP_SAVEPIECESJOINTES",
                DbConnection = this.connection
            })
            {
                dbOptions.Parameters = new[]
                {
                    new EacParameter("P_CODEOFFRE", contract.CodeAffaire.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = int.Parse(contract.Aliment) },
                    new EacParameter("P_TYPE", contract.Type),
                    new EacParameter("P_CONTEXTE", "SY60INTER"),
                    new EacParameter("P_ETAPE", "GEN"),
                    new EacParameter("P_DATE",  DbType.Int32) { Value = dateNow.ToIntYMD() },
                    new EacParameter("P_CODERISQUE", DbType.Int32) { Value = 0 },
                    new EacParameter("P_CODEOBJET", DbType.Int32) { Value = 0 },
                    new EacParameter("P_CODEFORMULE", DbType.Int32) { Value = 0 },
                    new EacParameter("P_CODEOPTION", DbType.Int32) { Value = 0 },
                    new EacParameter("P_EMPLACEMENT", "ANNEXE"),
                    new EacParameter("P_SOUSEMPLACEMENT", "ANDOC"),
                    new EacParameter("P_ORDRE", DbType.Int32) { Value = 0 },
                    new EacParameter("P_PIECESJOINTESID", idDoc.ToString() + ";")
                };
                dbOptions.ExecStoredProcedure();
            }
        }

        #endregion
    }
}
