using OP.WSAS400.DTO.GestionDocumentContrat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using System.Data;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.DocumentGestion;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Extensions;

namespace OP.DataAccess
{
    public class GestionDocumentRepository
    {
        #region Méthodes publiques

        public static List<LigneDocumentDto> GetListeDocuments(string code, string version, string type, string user)
        {
            InsertDemoData(code, version, type, user);
            //TODO appel BDD
            string sql = string.Format(@"SELECT KEQID GUIDID,
                                                KEQENVD DATEENVOI,
                                                KEQLIB LIBELLEDOCUMENT,
                                                KEQTDOC TYPEDOCUMENTCODE,
                                                KEQCDOC TYPEDOCUMENTLIB,
                                                KEQTYDIF TYPEDIFFUSION,
                                                KEQTYDS TYPEDESTINATAIRE,
                                                KEQNOM NOMDOCUMENT,
                                                KEQIDS IDDESTINATAIRE,
                                                KEQSERV SERVICE,
                                                KEQTEDI TYPEEDITION,
                                                KEQNOM NOMFICHIER,
                                                KEQCHM CHEMINFICHIER,
                                                CASE(KEQTYDS) WHEN 'CT' THEN TNNOM 
                                                			  WHEN 'AS' THEN ANNOM END NOMDESTINATAIRE,
                                                KEQACTG LOT,
                                                KEQAVN NUMAVENANT,
                                                (SELECT COUNT(*) FROM KPDOCJN WHERE KETKEQID = KEQID) NBPJ,
                                                KEQENVU UTILISATEUR
                                         FROM KPDOC
                                                LEFT JOIN YCOURTN ON TNICT = KEQIDS AND TNXN5 = 0 AND TNTNM = 'A'
                                                LEFT JOIN YASSNOM ON ANIAS = KEQIDS AND ANINL = 0 AND ANTNM = 'A'
                                         WHERE KEQTYP = '{0}' 
                                                AND KEQIPB = '{1}'
                                                AND KEQALX = {2}
                                         ORDER BY KEQENVD DESC", type, code.PadLeft(9, ' '), version);

            return DbBase.Settings.ExecuteList<LigneDocumentDto>(CommandType.Text, sql);
        }

        public static List<LignePieceJointeDto> GetListePiecesJointes(string idDocument)
        {
            string sql = string.Format(@"SELECT KGWLIB DESIGNATION,
                                                KGWNOM NOMDOCUMENT,
                                                KGWCHM CHEMINDOCUMENT
                                         FROM KPDOCJN 
                                                INNER JOIN KPELJN ON KGWID = KETKGWID
                                         WHERE KETKEQID = {0}
   	                                     ORDER BY KETORD", idDocument);
            return DbBase.Settings.ExecuteList<LignePieceJointeDto>(CommandType.Text, sql);
        }

        public static List<LigneCheminDocumentDto> GetListeLignesDocumentChemin()
        {
            string sql = @"SELECT KHMCLE IDENTIFIANT, KHMDES LIBELLE, KHMTCH TYPE, KHMCHM CHEMIN
                            FROM KCHEMIN";

            return DbBase.Settings.ExecuteList<LigneCheminDocumentDto>(CommandType.Text, sql);
        }

        public static LigneCheminDocumentDto GetLigneDocumentChemin(string identifiant)
        {
            string sql = string.Format(@"SELECT KHMCLE IDENTIFIANT, KHMDES LIBELLE, KHMTCH TYPE, KHMCHM CHEMIN,
                                         KHMSRV SERVEUR, KHMRAC RACINE, KHMENV ENVIRONNEMENT 
                                         FROM KCHEMIN
                                         WHERE KHMCLE = '{0}'", identifiant);
            var result = DbBase.Settings.ExecuteList<LigneCheminDocumentDto>(CommandType.Text, sql);
            if (result != null && result.Any())
                return result.FirstOrDefault();
            else
                return null;
        }

        public static List<LigneCheminDocumentDto> AddLigneDocumentChemin(string identifiant, string libelle, string type, string chemin, string serveur, string racine, string environnement, string user)
        {
            string sql = string.Format(@"INSERT INTO KCHEMIN 
                                         (KHMCLE, KHMDES, KHMTCH, KHMCHM, KHMCRU, KHMCRD, KHMMJU, KHMMJD,
                                          KHMSRV, KHMRAC, KHMENV) 
                                         VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')",
                                        identifiant, libelle, type, chemin, user, DateTime.Now.ToString("yyyyMMdd"), user, DateTime.Now.ToString("yyyyMMdd"), serveur, racine, environnement);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);
            return GetListeLignesDocumentChemin();
        }

        public static List<LigneCheminDocumentDto> UpdateLigneDocumentChemin(string identifiant, string libelle, string chemin, string serveur, string racine, string user)
        {
            string sql = string.Format(@"UPDATE KCHEMIN 
                                         SET KHMDES = '{0}',                                            
                                             KHMCHM = '{1}',
                                             KHMMJU = '{2}',
                                             KHMMJD = '{3}',
                                             KHMSRV = '{5}',
                                             KHMRAC = '{6}'                                             
                                         WHERE KHMCLE = '{4}'", libelle, chemin, user, DateTime.Now.ToString("yyyyMMdd"), identifiant, serveur, racine);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);
            return GetListeLignesDocumentChemin();
        }

        public static List<LigneCheminDocumentDto> DeleteLigneDocumentChemin(string identifiant)
        {
            string sql = string.Format(@"DELETE FROM KCHEMIN WHERE KHMCLE = '{0}'", identifiant);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);
            return GetListeLignesDocumentChemin();
        }

        public static List<ParametreDto> GetListeTypesChemin()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "TCHM");
        }

        public static List<ParametreDto> GetListeTypologiesChemin()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "TDOC");
        }

        public static string GetParamPrintDoc(long docId)
        {
            string sqlValidDoc = string.Format(@"SELECT COUNT(*) INT32RETURNCOL FROM KPDOCW WHERE KEQID = {0}", docId);
            var resultValidDoc = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlValidDoc);

            string sql = string.Format(@"SELECT KEQTGL CODE, KEQDIMP STRRETURNCOL FROM {0} WHERE KEQID = {1}", (resultValidDoc != null && resultValidDoc.FirstOrDefault().Int32ReturnCol > 0) ? "KPDOCW" : "KPDOC", docId);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(System.Data.CommandType.Text, sql);
            if (result != null && result.Any())
            {
                string print = result.FirstOrDefault().StrReturnCol == "O" ? "P" : string.Empty;
                string modif = result.FirstOrDefault().Code == "L" ? "M" : "V";
                return modif + print;
            }
            return string.Empty;
        }

        public static bool IsDocExterne(long docId)
        {
            var parameters = new List<EacParameter>()
            {
                new EacParameter("docId", DbType.Int32) { Value = docId }
            };
            return CommonRepository.GetInt64Value("SELECT KEQKESID INT64RETURNCOL FROM KPDOCW WHERE KEQID = :docId", parameters) > 0;
        }

        public static string GetFullPathDoc(long docId)
        {
            // ARA - 3071 
            string sqlValidDoc = string.Format(@"SELECT COUNT(*) INT32RETURNCOL FROM KPDOCW WHERE KEQID = {0}", docId);
            var resultValidDoc = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlValidDoc);

            string sql = string.Format(@"SELECT CASE WHEN KEQSTG = 'G' THEN TRIM(KEQCHM) CONCAT TRIM(KEQNOM) ELSE '' END STRRETURNCOL FROM {0} WHERE KEQID = {1}",(resultValidDoc!=null && resultValidDoc.FirstOrDefault().Int32ReturnCol > 0) ? "KPDOCW" : "KPDOC", docId);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().StrReturnCol;
            }

            return string.Empty;
        }

        public static void DeleteDocTableWork(string codeAffaire, string version, string type, long codeAvn)
        {
            var parameters = new List<EacParameter>()
            {
                new EacParameter("codeAffaire" , DbType.AnsiStringFixedLength) { Value = codeAffaire.PadLeft(9, ' ') },
                new EacParameter("version" , DbType.Int32) { Value = version },
                new EacParameter("type" , DbType.AnsiStringFixedLength) { Value = type },
                new EacParameter("codeAvn", DbType.Int64) {Value = codeAvn}
            };

            string sql = @"DELETE FROM KPDOCW WHERE KEQIPB = :codeAffaire AND KEQALX = :version AND KEQTYP = :type AND KEQAVN = :codeAvn";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, parameters.ToArray());

            sql = @"DELETE FROM KPDOCLDW WHERE KEMKELID IN (SELECT KELID FROM KPDOCLW WHERE KELIPB = :codeAffaire AND KELALX = :version AND KELTYP = :type AND KELAVN = :codeAvn)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, parameters.ToArray());

            sql = @"DELETE FROM KPDOCLW WHERE KELIPB = :codeAffaire AND KELALX = :version AND KELTYP = :type AND KELAVN = :codeAvn";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, parameters.ToArray());
        }

        public static void DeleteDocTableWork(string codeAffaire, string version, string type, long codeAvn, IDbConnection connection) {
            using (var dbOptions = new DbExecOptions(connection == null) {
                CommandType = CommandType.Text,
                DbConnection = connection,
                SqlText = "DELETE FROM KPDOCW WHERE KEQIPB = :codeAffaire AND KEQALX = :version AND KEQTYP = :type AND KEQAVN = :codeAvn"
            }) {
                dbOptions.BuildParameters(codeAffaire.ToIPB(), version, type, codeAvn);
                dbOptions.Exec();
                dbOptions.SqlText = "DELETE FROM KPDOCLDW WHERE KEMKELID IN (SELECT KELID FROM KPDOCLW WHERE KELIPB = :codeAffaire AND KELALX = :version AND KELTYP = :type AND KELAVN = :codeAvn)";
                dbOptions.Exec();
                dbOptions.SqlText = @"DELETE FROM KPDOCLW WHERE KELIPB = :codeAffaire AND KELALX = :version AND KELTYP = :type AND KELAVN = :codeAvn";
                dbOptions.Exec();
            }
        }

        #endregion

        #region Méthodes privées

        private static void InsertDemoData(string code, string version, string type, string user)
        {
            int assure = 4444;//Valeur par défaut
            int courtier = 4444;//Valeur par défaut
            //Effacage des données pour le contrat en cours
            string sqlDelete = @"DELETE FROM KPDOCJN";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDelete);

            sqlDelete = @"DELETE FROM KPELJN";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDelete);

            sqlDelete = string.Format(@"DELETE FROM KPDOC WHERE KEQTYP='{0}' AND KEQIPB='{1}' AND KEQALX={2}", type, code.PadLeft(9, ' '), version);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDelete);

            //Récupération des données complémentaires
            string sqlSelect = string.Format(@"SELECT PBIAS CODEPRENASSUR, PBICT COURTIERGESTIONNAIRE FROM YPOBASE WHERE PBIPB = '{0}' AND  PBTYP = '{1}' AND PBALX = {2}", code.PadLeft(9, ' '), type, version);
            var res = DbBase.Settings.ExecuteList<ContratDto>(CommandType.Text, sqlSelect);
            if (res != null && res.Any())
            {
                assure = res.FirstOrDefault().CodePreneurAssurance;
                courtier = res.FirstOrDefault().CourtierGestionnaire;
            }

            int id1 = CommonRepository.GenererNumeroChrono("KEQID");
            string nomDocument = @"Consultation des documents";
            string cheminDocument = @"C:\Users\slargemain\Desktop\Documents\- Consultation des documents.docx";


            string sql = string.Format(@"INSERT INTO KPDOC 
                                                (KEQID,KEQTYP,KEQIPB,KEQALX,KEQSUA,KEQNUM,KEQSBR,KEQSERV,KEQACTG,
                                                KEQAVN,KEQETAP,KEQKEMID,KEQORD,KEQTDOC,KEQKESID,KEQAJT,KEQTRS,KEQMAIT,
                                                KEQLIEN,KEQCDOC,KEQVER,KEQLIB,KEQNTA,KEQDACC,KEQTAE,KEQNOM,KEQCHM,
                                                KEQSTU,KEQSIT,KEQSTD,KEQSTH,KEQENVU,KEQENVD,KEQENVH,KEQTEDI,KEQORID
                                                ,KEQTYDS,KEQTYI,KEQIDS,KEQINL,KEQNBEX,KEQTYDIF,KEQAEMO,KEQAEM,KEQAEMT,
                                                KEQCRU,KEQCRD,KEQCRH,KEQMAJU,KEQMAJD,KEQMAJH)
                                         VALUES
                                               ({0},
                                               '{1}',
                                               '{2}',
                                               {3},
                                               0,
                                               0,
                                               0,
                                               'PRODU',
                                               'AFFNOUV',
                                               0,
                                               'FIN',
                                               0, 
                                               1,
                                               'CP',
                                               0,
                                               'N',
                                               'N',
                                               '',
                                               0,
                                               'CPRSSPE',
                                               0,
                                               'CP RS SPE',
                                               'O',
                                               'N',
                                               '',
                                               '{4}',
                                               '{5}',
                                               '{6}',
                                               'V',
                                               {7},
                                               {8},
                                               '{6}',
                                               {7},
                                               {8},
                                               'O',
                                               0,
                                               'CT',
                                               '',
                                               {9},
                                               0,
                                               2,
                                               'COURR',
                                               '',
                                               '',
                                               0,
                                               '{6}',
                                               {7},
                                               {8},
                                               '{6}',
                                               {7},
                                               {8}
                                               )", id1, type, code.PadLeft(9, ' '), version, nomDocument, cheminDocument, user,
                                                  DateTime.Now.ToString("yyyyMMdd"),
                                                  DateTime.Now.ToString("HHmm"),
                                                  courtier);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            int id2 = CommonRepository.GenererNumeroChrono("KEQID");

            sql = string.Format(@"INSERT INTO KPDOC 
                                              (KEQID,KEQTYP,KEQIPB,KEQALX,KEQSUA,KEQNUM,KEQSBR,KEQSERV,KEQACTG,
                                              KEQAVN,KEQETAP,KEQKEMID,KEQORD,KEQTDOC,KEQKESID,KEQAJT,KEQTRS,KEQMAIT,
                                              KEQLIEN,KEQCDOC,KEQVER,KEQLIB,KEQNTA,KEQDACC,KEQTAE,KEQNOM,KEQCHM,
                                              KEQSTU,KEQSIT,KEQSTD,KEQSTH,KEQENVU,KEQENVD,KEQENVH,KEQTEDI,KEQORID
                                              ,KEQTYDS,KEQTYI,KEQIDS,KEQINL,KEQNBEX,KEQTYDIF,KEQAEMO,KEQAEM,KEQAEMT,
                                              KEQCRU,KEQCRD,KEQCRH,KEQMAJU,KEQMAJD,KEQMAJH)
                                  VALUES
                                              ({0},
                                              '{1}',
                                              '{2}',
                                              {3},
                                              0,
                                              0,
                                              0,
                                              'PRODU',
                                              'AFFNOUV',
                                              0,
                                              'FIN',
                                              0, 
                                              2,
                                              'CP',
                                              0,
                                              'N',
                                              'N',
                                              '',
                                              0,
                                              'CPRSSPE',
                                              0,
                                              'CP RS SPE',
                                              'P',
                                              'N',
                                              '',
                                              '{4}',
                                              '{5}',
                                              '{6}',
                                              'V',
                                              {7},
                                              {8},
                                              '{6}',
                                              {7},
                                              {8},
                                              'C',
                                              {10},
                                              'AS',
                                              '',
                                              {9},
                                              0,
                                              1,
                                              'COURR',
                                              '',
                                              '',
                                              0,
                                              '{6}',
                                              {7},
                                              {8},
                                              '{6}',
                                              {7},
                                              {8}
                                              )", id2, type, code.PadLeft(9, ' '), version, nomDocument, cheminDocument, user,
                                                  DateTime.Now.ToString("yyyyMMdd"),
                                                  DateTime.Now.ToString("HHmm"),
                                                  assure, id1);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            int id3 = CommonRepository.GenererNumeroChrono("KEQID");

            sql = string.Format(@"INSERT INTO KPDOC 
                                        (KEQID,KEQTYP,KEQIPB,KEQALX,KEQSUA,KEQNUM,KEQSBR,KEQSERV,KEQACTG,
                                        KEQAVN,KEQETAP,KEQKEMID,KEQORD,KEQTDOC,KEQKESID,KEQAJT,KEQTRS,KEQMAIT,
                                        KEQLIEN,KEQCDOC,KEQVER,KEQLIB,KEQNTA,KEQDACC,KEQTAE,KEQNOM,KEQCHM,
                                        KEQSTU,KEQSIT,KEQSTD,KEQSTH,KEQENVU,KEQENVD,KEQENVH,KEQTEDI,KEQORID
                                        ,KEQTYDS,KEQTYI,KEQIDS,KEQINL,KEQNBEX,KEQTYDIF,KEQAEMO,KEQAEM,KEQAEMT,
                                        KEQCRU,KEQCRD,KEQCRH,KEQMAJU,KEQMAJD,KEQMAJH)
                                  VALUES
                                        ({0},
                                        '{1}',
                                        '{2}',
                                        {3},
                                        0,
                                        0,
                                        0,
                                        'PRODU',
                                        'AFFNOUV',
                                        0,
                                        'FIN',
                                        0, 
                                        3,
                                        'AVISECH',
                                        0,
                                        'N',
                                        'N',
                                        '',
                                        0,
                                        'AVISECH',
                                        0,
                                        'Avis d''échéance',
                                        'O',
                                        'N',
                                        '',
                                        '{4}',
                                        '{5}',
                                        '{6}',
                                        'V',
                                        {7},
                                        {8},
                                        '{6}',
                                        {7},
                                        {8},
                                        'O',
                                        0,
                                        'CT',
                                        '',
                                        {9},
                                        0,
                                        1,
                                        'COURR',
                                        '',
                                        '',
                                        0,
                                        '{6}',
                                        {7},
                                        {8},
                                        '{6}',
                                        {7},
                                        {8}
                                        )", id3, type, code.PadLeft(9, ' '), version, nomDocument, cheminDocument, user,
                                                  DateTime.Now.ToString("yyyyMMdd"),
                                                  DateTime.Now.ToString("HHmm"),
                                                  courtier);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            //Insertion données pièces jointes
            int idPJPrincipal = CommonRepository.GenererNumeroChrono("KETID");
            int idPJSecondaire = CommonRepository.GenererNumeroChrono("KGWID");

            sql = string.Format(@"INSERT INTO KPELJN 
                                   (KGWID,KGWTYP,KGWIPB	,KGWALX	,KGWSUA	,KGWNUM	,KGWSBR	,KGWSERV,KGWACTG,KGWAVN	,KGWORD	,KGWTYPO,KGWLIB	,KGWNOM	,KGWCHM	,KGWSTU	,KGWSIT	,KGWSTD	,KGWSTH	,KGWCRU	,KGWCRD	,KGWCRH)
                                   VALUES
                                    ({0}, '{1}', '{2}', {3}, 0, 0, '', 'Produ', '', 0, 1, '', 'Mon document', 'pj1511.docx', 'C:\Users\slargemain\Desktop\Documents\pj1511.docx', '','', 0, 0, '', 0,0)",
                                    idPJSecondaire, type, code.PadLeft(9, ' '), version);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"INSERT INTO KPDOCJN
                                   VALUES ({0},{1}, 1, {2})", idPJPrincipal, id1, idPJSecondaire);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        #endregion
    }
}
