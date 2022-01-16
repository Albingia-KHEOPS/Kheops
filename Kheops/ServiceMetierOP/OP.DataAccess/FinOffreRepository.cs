using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Cotisations;
using OP.WSAS400.DTO.DocumentGestion;
using OP.WSAS400.DTO.DocumentsJoints;
using OP.WSAS400.DTO.FinOffre;
using OP.WSAS400.DTO.GestionDocument;
using OP.WSAS400.DTO.MontantReference;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Quittance;
using OP.WSAS400.DTO.SyntheseDocuments;
using OP.WSAS400.DTO.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using static DataAccess.Helpers.OutilsHelper;


namespace OP.DataAccess
{
    using static CommonRepository;
    public class FinOffreRepository
    {

        public static FinOffreDto InitFinOffre(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            FinOffreDto toReturn = new FinOffreDto();
            BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);
            string sql = string.Format(@"SELECT PBANT ANTECEDENT, KADDESI DESCRIPTION, PBDUR VALIDITEOFFRE, KAADPRJ DATEPROJET, KAADSTA DATESTATISTIQUE, PBREL RELANCE, PBRLD RELANCEVALEUR, JDDPV PREAVIS,
                                                    KAJOBSV ANNOTATIONFIN, PBSAJ DATESTATISTIQUEJOUR, PBSAM DATESTATISTIQUEMOIS, PBSAA DATESTATISTIQUEANNEE 
                                FROM {0}
                                    LEFT JOIN {1} ON PBIPB = KAAIPB AND PBALX = KAAALX AND {8} = KAATYP {10}
                                    LEFT JOIN {2} ON KADCHR = KAAAND
                                    LEFT JOIN {3} ON PBIPB = JDIPB AND PBALX = JDALX {11}
                                    LEFT JOIN {4} ON KAAOBSV = KAJCHR
                                WHERE {8}='{5}' AND PBIPB='{6}' AND PBALX='{7}' {9}",
                                CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPOBSV"),
                                type, codeOffre.PadLeft(9, ' '), Convert.ToInt32(version),
                                modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                                modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                modeNavig == ModeConsultation.Historique ? " AND PBAVN = KAAAVN" : string.Empty,
                                modeNavig == ModeConsultation.Historique ? " AND PBAVN = JDAVN" : string.Empty);
            var finOffrePlatDto = DbBase.Settings.ExecuteList<FinOffrePlatDto>(CommandType.Text, sql).FirstOrDefault();
            if (finOffrePlatDto != null)
                toReturn = GetFinOffreDto(branche.Code, branche.Cible.Code, finOffrePlatDto);
            return toReturn;
        }

        [Obsolete]
        private static ParametreDto Initialiser(DataRow row)
        {
            ParametreDto Parametre = new ParametreDto();
            if (row.Table.Columns.Contains("TCOD")) { Parametre.Code = row["TCOD"].ToString(); Parametre.Libelle = row["TCOD"].ToString(); };
            return Parametre;
        }

        public static void FinOffreUpdate(string codeOffre, string version, string type, FinOffreDto finOffre)
        {
            UpdateKPENT(codeOffre, version, type, finOffre);
            UpdateYPOBASE(codeOffre, version, type, finOffre);
            UpdateYPRTENT(codeOffre, version, type, finOffre);
            UpdateKPDESI(codeOffre, version, type, finOffre.FinOffreInfosDto.Description);
            UpdateKPOBSV(codeOffre, version, type, finOffre);

            if (type == AlbConstantesMetiers.TYPE_CONTRAT)
            {
                //CommonRepository.AlimStatistiques(codeOffre, version);
            }
        }
        private static void UpdateKPENT(string codeOffre, string version, string type, FinOffreDto finOffre)
        {
            string sql = string.Format(@"UPDATE KPENT SET 
                                        KAADPRJ = '{3}' 
                                        WHERE KAATYP = '{0}' AND KAAIPB = '{1}' AND KAAALX = '{2}'", type, codeOffre.PadLeft(9, ' '),
                                                Convert.ToInt32(version), AlbConvert.ConvertDateToInt(finOffre.FinOffreInfosDto.DateProjet)
                                              );
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

        }
        private static void UpdateYPOBASE(string codeOffre, string version, string type, FinOffreDto finOffre)
        {
            int relanceValeur = finOffre.FinOffreInfosDto.Relance ? finOffre.FinOffreInfosDto.RelanceValeur : 0;
            string sql = string.Format(@"UPDATE YPOBASE 
                                    SET PBANT ='{3}', PBDUR  = '{4}', PBREL = '{5}', PBRLD = '{6}'
                                    WHERE PBTYP = '{0}' AND PBIPB = '{1}' AND PBALX = '{2}' ", type, codeOffre.PadLeft(9, ' '), Convert.ToInt32(version),
                                    finOffre.FinOffreInfosDto.Antecedent, finOffre.FinOffreInfosDto.ValiditeOffre, AlbConvert.ConvertBoolToString(finOffre.FinOffreInfosDto.Relance), relanceValeur);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }
        private static void UpdateYPRTENT(string codeOffre, string version, string type, FinOffreDto finOffre)
        {
            string sql = string.Format(@"UPDATE YPRTENT 
                                                SET JDDPV ='{2}'
                                                WHERE JDIPB='{0}' AND JDALX='{1}'", codeOffre.PadLeft(9, ' '), Convert.ToInt32(version), finOffre.FinOffreInfosDto.Preavis);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }
        public static void UpdateKPDESI(string codeOffre, string version, string type, string description)
        {
            string sql = string.Empty;
            string result = string.Empty;
            string KAAAND = string.Empty;

            sql = string.Format(@"SELECT KAAAND FROM KPENT
                                        WHERE KAATYP='{0}' AND KAAIPB='{1}' AND KAAALX='{2}'", type, codeOffre.PadLeft(9, ' '), Convert.ToInt32(version));
            var retourExecute = DbBase.Settings.ExecuteScalar(CommandType.Text, sql);
            if (retourExecute != null)
                result = retourExecute.ToString();

            if (!string.IsNullOrEmpty(result))
            {
                KAAAND = result;
                sql = string.Format(@"SELECT COUNT(*) FROM KPDESI WHERE KADCHR='{0}'", KAAAND);
                retourExecute = DbBase.Settings.ExecuteScalar(CommandType.Text, sql);
                result = retourExecute != null ? retourExecute.ToString() : string.Empty;
                if (!string.IsNullOrEmpty(result) && result != "0")
                {
                    sql = string.Format(@"UPDATE KPDESI SET KADDESI ='{1}'
                                        WHERE KADCHR='{0}'", KAAAND, description.Replace("'", "''"));
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                    return;
                }
            }

            int KADCHR = CommonRepository.GetAS400Id("KADCHR");

            sql = string.Format(@"INSERT INTO KPDESI (KADCHR, KADTYP, KADIPB, KADALX, KADDESI) 
                                        VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                                           KADCHR, type, codeOffre.PadLeft(9, ' '), Convert.ToInt32(version), description.Replace("'", "''"));
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"UPDATE KPENT SET KAAAND = '{3}'
                               WHERE KAATYP='{0}' AND KAAIPB='{1}' AND KAAALX='{2}'", type, codeOffre.PadLeft(9, ' '), Convert.ToInt32(version), KADCHR);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }
        private static void UpdateKPOBSV(string codeOffre, string version, string type, FinOffreDto finOffre)
        {
            //TODO refaire cette fonction
            string sql = string.Empty;
            string result = string.Empty;

            string KAAOBSV = string.Empty;
            sql = string.Format(@"SELECT KAAOBSV FROM KPENT 
                        WHERE KAATYP='{0}' AND KAAIPB='{1}' AND KAAALX='{2}' ", type, codeOffre.PadLeft(9, ' '), Convert.ToInt32(version));
            var retourExecute = DbBase.Settings.ExecuteScalar(CommandType.Text, sql);
            if (retourExecute != null)
                result = retourExecute.ToString();

            if (!string.IsNullOrEmpty(result) && result != "0")
            {
                KAAOBSV = result;
                sql = string.Format(@"SELECT COUNT(*) FROM KPOBSV WHERE KAJCHR='{0}'", KAAOBSV);
                retourExecute = DbBase.Settings.ExecuteScalar(CommandType.Text, sql);
                result = retourExecute != null ? retourExecute.ToString() : string.Empty;
                if (!string.IsNullOrEmpty(result) && result != "0")
                {
                    sql = string.Format(@"UPDATE KPOBSV SET KAJOBSV = '{1}' 
                                        WHERE KAJCHR='{0}'", KAAOBSV, finOffre.FinOffreAnnotationDto.AnnotationFin.Replace("'", "''"));
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                    return;
                }
            }
            else
            {
                int KAJCHR = CommonRepository.GetAS400Id("KAJCHR");
                sql = string.Format(@"INSERT INTO KPOBSV (KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJOBSV, KAJTYPOBS) 
                                        VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                               KAJCHR, type, codeOffre.PadLeft(9, ' '), Convert.ToInt32(version),
                               finOffre.FinOffreAnnotationDto.AnnotationFin.Replace("'", "''"), "GENERALE");
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

                sql = string.Format(@"UPDATE KPENT SET KAAOBSV = '{3}' 
                    WHERE KAATYP='{0}' AND KAAIPB='{1}' AND KAAALX='{2}'", type, codeOffre.PadLeft(9, ' '), Convert.ToInt32(version), KAJCHR);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }
        }

        #region ValidationOffre

        public static ValidationDto InitValidationOffre(string codeOffre, string version, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string acteGestion, string reguleId, string reguleMode)
        {
            ValidationDto toReturn = null;
            string sql = string.Empty;
            if (type == AlbConstantesMetiers.TYPE_OFFRE)
            {
                //CASE WHEN KEQID IS NULL THEN 'N' ELSE 'O' END ISDOCEDIT,
                sql = string.Format(@"SELECT KAADSTA DATESTATISTIQUE, KAAOBSV CODEOBSERVATION, KAJOBSV OBSERVATION, 
                                      CASE WHEN JDTFF = 0 THEN JDTMC ELSE JDTFF END MONTANTREFERENCE,
                                      PBCTA COURTIERAPP, COURTAPP.TCFVA VALIDAPP, PBICT COURTIERGEST, COURTGEST.TCFVA VALIDGEST,
                                      PBETA ETAT, PBSTF MOTIF,
                                      CASE WHEN (SELECT COUNT(*) FROM KPDOCLDW WHERE KEMTYPL = KEQID AND KEMSIT = 'O' AND KEQSIT = 'V' AND KEQDIMP = 'O') > 0 THEN 'O' ELSE 'N' END ISDOCEDIT,
                                      CASE WHEN (
	                                    (SELECT COUNT(*) FROM KPDOCW 
		                                    INNER JOIN YYYYPAR ON TCON = 'KHEOP' AND TFAM = 'TDOC' AND TCOD = KEQTDOC AND TPCN1 = 1
                                            WHERE KEQIPB = KAAIPB AND KEQALX = KAAALX AND KEQTYP = KAATYP AND TRIM(KEQACTG) = '{3}' AND KEQSTG = 'N') > 0) THEN 'N' ELSE 'O' END ISDOCGENER,
                                      (PBSAA * 10000 + PBSAM * 100 + PBSAJ) DATEACCORD
                                      FROM KPENT
                                      LEFT JOIN YPRTENT ON JDIPB = KAAIPB AND JDALX = KAAALX 
                                      LEFT JOIN YPOBASE ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP
                                      LEFT JOIN KPOBSV ON KAJCHR = KAAOBSV
                                      LEFT JOIN KPDOCW ON KAAIPB = KEQIPB AND KAAALX = KEQALX AND KAATYP = KEQTYP AND KEQDIMP = 'O' AND TRIM(KEQACTG) = '{3}'
                                      LEFT JOIN YCOURTI COURTAPP ON COURTAPP.TCICT = PBCTA
                                      LEFT JOIN YCOURTI COURTGEST ON COURTGEST.TCICT = PBICT
                                      WHERE KAAIPB = '{0}' AND KAAALX = {1} AND KAATYP = '{2}'", codeOffre.PadLeft(9, ' '), version, type, acteGestion);
            }
            else if (type == AlbConstantesMetiers.TYPE_CONTRAT)
            {
                bool isRegul = acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
                bool isRegulOrRegulModif = isRegul || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF;
                bool isHisto = modeNavig == ModeConsultation.Historique;
                int codeAvnNum = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
                int regulIdNum = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0;

                //CASE WHEN KEQID IS NULL THEN 'N' ELSE 'O' END ISDOCEDIT,
                sql = string.Format(@"SELECT KAADSTA DATESTATISTIQUE, IFNULL(KAJCHR, 0) CODEOBSERVATION, KAJOBSV OBSERVATION,
                                      JDTMC MONTANTREFCALC, JDTFF MONTANTREFFORCE, JDACQ MONTANTREFACQUIS,
                                      PBCTA COURTIERAPP, COURTAPP.TCFVA VALIDAPP, PBICT COURTIERGEST, COURTGEST.TCFVA VALIDGEST,
                                      PBETA ETAT,
                                      PBSTF MOTIF,
                                      CASE WHEN (SELECT COUNT(*) FROM KPDOCLDW WHERE KEMTYPL = KEQID AND KEMSIT = 'O' AND KEQSIT = 'V' AND KEQDIMP = 'O') > 0 THEN 'O' ELSE 'N' END ISDOCEDIT,
                                      CASE WHEN (
	                                    (SELECT COUNT(*) FROM KPDOCW
		                                    INNER JOIN YYYYPAR ON TCON = 'KHEOP' AND TFAM = 'TDOC' AND TCOD = KEQTDOC AND TPCN1 = 1
                                            WHERE KEQIPB = KAAIPB AND KEQALX = KAAALX AND KEQTYP = KAATYP AND TRIM(KEQACTG) = '{11}' AND KEQSTG = 'N') > 0
		                                    OR
	                                    ((SELECT COUNT(*) FROM KPDOCW
                                            INNER JOIN YYYYPAR ON TCON = 'KHEOP' AND TFAM = 'TDOC' AND TCOD = KEQTDOC AND TPCN1 = 1
		                                    WHERE KEQIPB = KAAIPB AND KEQALX = KAAALX AND KEQTYP = KAATYP AND TRIM(KEQACTG) = '{11}') = 0 AND ('{11}' = 'REGULE' OR '{11}' = 'AVNRG'))) THEN 'N' ELSE 'O' END ISDOCGENER,
                                      {14} ETATREGULE,
                                      CASE WHEN (PBTAA * 10000 + PBTAM * 100 + PBTAJ) > 0 THEN (PBTAA * 10000 + PBTAM * 100 + PBTAJ) ELSE (PBSAA * 10000 + PBSAM * 100 + PBSAJ) END DATEACCORD
                                      FROM {3}
                                      LEFT JOIN {4} ON JDIPB = KAAIPB AND JDALX = KAAALX {8}
                                      LEFT JOIN {5} ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP {9}
                                      {12}
                                      LEFT JOIN {6} ON KAJCHR = {13}
                                      LEFT JOIN KPDOCW ON KAAIPB = KEQIPB AND KAAALX = KEQALX AND KAATYP = KEQTYP AND KEQDIMP = 'O' AND TRIM(KEQACTG) = '{11}'
                                      LEFT JOIN YCOURTI COURTAPP ON COURTAPP.TCICT = PBCTA
                                      LEFT JOIN YCOURTI COURTGEST ON COURTGEST.TCICT = PBICT
                                      WHERE KAAIPB = '{0}' AND KAAALX = {1} AND KAATYP = '{2}' {7}",
                             /* 0*/   codeOffre.PadLeft(9, ' '),
                             /* 1*/   version,
                             /* 2*/   type,
                             /* 3*/   CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                             /* 4*/   CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                             /* 5*/   CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                             /* 6*/   CommonRepository.GetPrefixeHisto(modeNavig, "KPOBSV"),
                             /* 7*/   isHisto ? string.Format(" AND KAAAVN = {0}", codeAvnNum) : string.Empty,
                             /* 8*/   isHisto ? " AND JDAVN = KAAAVN" : string.Empty,
                             /* 9*/   isHisto ? " AND PBAVN = KAAAVN" : string.Empty,
                             /*10*/   isHisto ? " AND 1 = 0 " : string.Empty,//Si mode histo, on ignore la jointure => doc non editable
                             /*11*/   reguleMode == "BNS" || reguleMode == "PB" ? reguleMode : acteGestion,
                             /*12*/   isRegulOrRegulModif ? string.Format(" LEFT JOIN KPRGU ON KHWID = {0}", regulIdNum) : string.Empty,
                             /*13*/   isRegul ? "KHWOBSV" : "KAAOBSV",
                             /*14*/   isRegulOrRegulModif ? "KHWETA" : "''",
                             /*15*/   isRegulOrRegulModif ? "REGUL" : string.Empty);
            }

            if (!string.IsNullOrEmpty(sql))
            {
                var result = DbBase.Settings.ExecuteList<ValidationDto>(CommandType.Text, sql);
                var test = result.FindAll(m => m.IsDocEdit == "O").Count;
                if (result != null && result.Any())
                {
                    if (result.FindAll(m => m.IsDocEdit == "O").Count > 0)
                        toReturn = result.FirstOrDefault(m => m.IsDocEdit == "O");
                    else
                        toReturn = result.FirstOrDefault();
                }
            }
            if (toReturn != null)
            {

                var result = CommonRepository.ObtenirBureauSecteurDelegation(toReturn.CourtierApporteur, toReturn.CourtierGestionnaire);
                if (result != null)
                {
                    toReturn.DelegationApporteur = result.DelegationApporteur;
                    toReturn.DelegationGestionnaire = result.DelegationGestionnaire;
                    toReturn.SecteurApporteur = result.SecteurApporteur;
                    toReturn.SecteurGestionnaire = result.SecteurGestionnaire;
                }
            }
            else if (toReturn != null && type == AlbConstantesMetiers.TYPE_OFFRE)
            {
                var result = CommonRepository.ObtenirCodeDelegationEtCodeInspecteur(toReturn.CourtierGestionnaire);
                if (result != null)
                {
                    toReturn.SecteurOffre = result.LibSecteur;
                    var delelegation = DelegationRepository.ObtenirNomDelegationNomInspecteur(result);
                    if (delelegation != null)
                        toReturn.DelegationOffre = delelegation.Nom;
                    else
                        toReturn.DelegationOffre = string.Empty;
                }

            }

            if (toReturn == null)
                toReturn = new ValidationDto();


            if (CheckOffreValid(codeOffre, version, type, codeAvn, modeNavig) > 0)
            {
                toReturn.OffreComplete = "Non";
                toReturn.Motif = "SYS";
            }
            else
            {
                toReturn.OffreComplete = "Oui";
                toReturn.Motif = string.Empty;
            }
            toReturn.Docs = GetValidationEdition(codeOffre, version, type, codeAvn, modeNavig, acteGestion, "valider", reguleMode == "BNS");
            toReturn.Criteres = GetCritereValid(codeOffre, version, type, codeAvn, modeNavig);
            toReturn.ValidationRequise = acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL ? "Non" : toReturn.Criteres.Count > 0 ? "Oui" : "Non";

            toReturn.IsControleOk = !NavigationArbreRepository.ExisteKPCTRL(codeOffre, Int32.Parse(version), type);
            if (toReturn.IsControleOk)
            {
                //les courtiers apporteur et gestionnaire ne doivent pas être fermés
                //pour pouvoir valider l'offre, l'AN ou l'avenant
                if (type == AlbConstantesMetiers.TYPE_OFFRE)
                {
                    toReturn.IsControleOk = toReturn.ValidGest == 0;
                }
                else
                {
                    toReturn.IsControleOk = (codeAvn.IsEmptyOrNull() || codeAvn == "0") ? toReturn.ValidApp == 0 && toReturn.ValidGest == 0 : toReturn.ValidGest == 0;
                }
            }

            if (toReturn.Etat == "N" && toReturn.OffreComplete == "Oui" && toReturn.IsControleOk && toReturn.IsDocGener == "O")
            {
                toReturn.Etat = ChangeEtatAffaire(codeOffre, version, type);
            }

            return toReturn;
        }

        private static string ChangeEtatAffaire(string codeOffre, string version, string type)
        {
            var (sql, param) = MakeParamsSql(@"UPDATE YPOBASE SET PBETA = 'A' WHERE PBIPB = :ipb AND PBALX = :alx AND PBTYP = :type",
                codeOffre.ToIPB(), version, type);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            return "A";
        }

        public static void SaveEtatMotif(string codeOffre, string version, string type, string etat, string motif, string acteGestion, string regulId)
        {

            var (sql, param) = MakeParamsSql(@"SELECT PBETA STRRETURNCOL FROM YPOBASE WHERE PBIPB = :ipb AND PBALX = :alx AND PBTYP = :type",
                codeOffre.ToIPB(), version, type);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                //Vérification de l'état pour ne pas modifier une offre/acte de gestion à "V"
                if (result.FirstOrDefault().StrReturnCol != "V")
                {
                    var (sqlUpd, paramUpd) = MakeParamsSql(@"UPDATE YPOBASE 
                                         SET PBETA = :eta,
                                         PBSTF = :stf
                                         WHERE PBIPB = :ipb AND PBTYP = :type AND PBALX = :alx", etat, motif, codeOffre.ToIPB(), type, version);

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpd, paramUpd);
                }
            }

            /* Regul*/

            if (acteGestion == "REGUL" || acteGestion == "AVNRG")
            {
                long rgId = 0;

                if (!string.IsNullOrEmpty(regulId))
                    long.TryParse(regulId, out rgId);

                string sqlRegul = "SELECT KHWETA STRRETURNCOL FROM KPRGU WHERE KHWID = :rgId";

                var resultRegul = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlRegul, new List<EacParameter>() { new EacParameter("rgId", DbType.Int64) { Value = rgId } });

                if (resultRegul != null && resultRegul.Any())
                {
                    //Vérification de l'état pour ne pas modifier une offre/acte de gestion à "V"
                    if (resultRegul.FirstOrDefault().StrReturnCol != "V")
                    {
                        sql = "UPDATE KPRGU SET KHWETA = :etat WHERE KHWID = :rgId";

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, new List<EacParameter>() {
                            new EacParameter("etat", DbType.AnsiStringFixedLength) { Value = etat},
                            new EacParameter("rgId", DbType.Int64) { Value = rgId }
                        });
                    }
                }
            }
        }

        private static int CheckOffreValid(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var param = new List<DbParameter>() {
                new EacParameter("KEUTYP", type),
                new EacParameter("KEUIPB", codeOffre.PadLeft(9, ' ')),
                new EacParameter("KEUALX", Convert.ToInt32(version))
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", Convert.ToInt32(version)));
            }
            string sql = string.Format(@"SELECT COUNT(*) NBLGN FROM {0}
                        WHERE KEUTYP = :KEUTYP AND KEUIPB = :KEUIPB AND KEUALX = :KEUALX {1}
                            AND KEUNIVM IN ('B', 'N')",
                        CommonRepository.GetPrefixeHisto(modeNavig, "KPCTRL"),
                        modeNavig == ModeConsultation.Historique ? " AND KEUAVN = :avn" : string.Empty);

            var result = DbBase.Settings.ExecuteList<ValidationDto>(CommandType.Text, sql, param).FirstOrDefault();
            return result.NbLigne;
        }

        private static List<ValidationCritereDto> GetCritereValid(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var param = new List<DbParameter> {
                new EacParameter("KEXTYP", type),
                new EacParameter("KEXIPB", codeOffre.PadLeft(9, ' ')),
                new EacParameter("KEXALX", Convert.ToInt32(version))
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", Convert.ToInt32(version)));
            }

            string sql = string.Format(@"SELECT KEXLIB LIBELLE, KEXNIV NIVEAU FROM {0}
                        WHERE KEXTYP = :KEXTYP AND KEXIPB = :KEXIPB AND KEXALX = :KEXALX {1}",
                        CommonRepository.GetPrefixeHisto(modeNavig, "KPVALID"),
                        modeNavig == ModeConsultation.Historique ? " AND KEXAVN = :avn" : string.Empty);

            return DbBase.Settings.ExecuteList<ValidationCritereDto>(CommandType.Text, sql, param);
        }

        public static bool CheckValidateOffre(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion)
        {
            string offreComplete = CheckOffreValid(codeOffre, version, type, codeAvn, modeNavig) > 0 ? "Non" : "Oui";
            bool isControleOk = !NavigationArbreRepository.ExisteKPCTRL(codeOffre, Int32.Parse(version), type);

            return offreComplete == "Oui" && isControleOk;
        }

        public static string VerifCourtier(string codeOffre, string version, string type)
        {
            string toReturn = string.Empty;

            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", type);

            string sql = @"SELECT COURTAPP.TCFVA VALIDAPP, COURTGEST.TCFVA VALIDGEST, PBTTR 
                            FROM YPOBASE 
                                INNER JOIN YCOURTI COURTAPP ON COURTAPP.TCICT = PBCTA
                                INNER JOIN YCOURTI COURTGEST ON COURTGEST.TCICT = PBICT
                            WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

            var result = DbBase.Settings.ExecuteList<ValidationDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                var firstRes = result.FirstOrDefault();
                if (firstRes.ValidApp > 0 && firstRes.TypeTraitement == AlbConstantesMetiers.TRAITEMENT_AFFNV)
                {
                    toReturn += "<br/>Le courtier apporteur est fermé";
                }
                if (firstRes.ValidGest > 0 && firstRes.TypeTraitement == AlbConstantesMetiers.TRAITEMENT_AFFNV)
                {
                    toReturn += "<br/>Le courtier gestionnaire est fermé";
                }
            }
            return toReturn;
        }

        #endregion

        #region Gestion Document

        #region Méthodes Publiques

        public static GestionDocumentCreationDto InitGestionDocumentCreation(string codeOffre, string version, string type, string codeDoc)
        {
            GestionDocumentCreationDto toReturn = new GestionDocumentCreationDto
            {
                Courriers = RemplirCourrier(),
                Emails = RemplirEmail()
            };
            return toReturn;
        }

        public static void SaveGestionDocument(string codeOffre, string version, GestionDocumentCreationDto document)
        {

        }

        #endregion

        #region Méthodes Privées

        private static List<DocumentDto> RemplirGestionDocument()
        {
            List<DocumentDto> toReturn = new List<DocumentDto>();

            DocumentDto doc = null;
            doc = new DocumentDto
            {
                NomDoc = "Offre",
                Diffusions = RemplirDiffusion(1)
            };
            toReturn.Add(doc);
            doc = new DocumentDto
            {
                NomDoc = "Offre avec lettre d'accompagnement",
                Diffusions = RemplirDiffusion(2)
            };
            toReturn.Add(doc);
            doc = new DocumentDto
            {
                NomDoc = "Lettre d'accusé de réception PDF",
                Diffusions = RemplirDiffusion(3)
            };
            toReturn.Add(doc);

            return toReturn;
        }

        private static List<GestionDocumentDiffusionDto> RemplirDiffusion(int index)
        {
            List<GestionDocumentDiffusionDto> toReturn = new List<GestionDocumentDiffusionDto>();
            GestionDocumentDiffusionDto diff = null;

            switch (index)
            {
                case 1:
                    diff = new GestionDocumentDiffusionDto
                    {
                        TypeDiffusion = "cour",
                        Partenaire = "Courtier",
                        Destinataire = "10234 - Courtage de France",
                        CreationDate = new DateTime(2012, 09, 05)
                    };
                    toReturn.Add(diff);
                    break;
                case 2:
                    diff = new GestionDocumentDiffusionDto
                    {
                        TypeDiffusion = "mail",
                        Partenaire = "Courtier",
                        Destinataire = "10234 - Courtage de France",
                        CreationDate = new DateTime(2012, 09, 05)
                    };
                    toReturn.Add(diff);
                    diff = new GestionDocumentDiffusionDto
                    {
                        TypeDiffusion = "mail",
                        Partenaire = "Assuré",
                        Destinataire = "142085 - Festival international d'art lyrique",
                        CreationDate = new DateTime(2012, 09, 05)
                    };
                    toReturn.Add(diff);
                    diff = new GestionDocumentDiffusionDto
                    {
                        TypeDiffusion = "mail",
                        Partenaire = "Co-assureur",
                        Destinataire = "9454 - AGF assurance",
                        CreationDate = new DateTime(2012, 09, 05)
                    };
                    toReturn.Add(diff);
                    break;
                case 3:
                    diff = new GestionDocumentDiffusionDto
                    {
                        TypeDiffusion = "mail",
                        Partenaire = "Courtier",
                        Destinataire = "10234 - Courtage de France",
                        CreationDate = new DateTime(2012, 07, 31),
                        TraitementDate = new DateTime(2012, 08, 01)
                    };
                    toReturn.Add(diff);
                    diff = new GestionDocumentDiffusionDto
                    {
                        TypeDiffusion = "cour",
                        Partenaire = "Courtier",
                        Destinataire = "10234 - Courtage de France",
                        CreationDate = new DateTime(2012, 07, 31),
                        TraitementDate = new DateTime(2012, 08, 01)
                    };
                    toReturn.Add(diff);
                    break;
            }
            return toReturn;
        }

        private static List<GestionDocumentCourrierDto> RemplirCourrier()
        {
            List<GestionDocumentCourrierDto> toReturn = new List<GestionDocumentCourrierDto>();

            GestionDocumentCourrierDto courrier = new GestionDocumentCourrierDto
            {
                CodeCourrier = "1",
                TypePartenaire = "COURT",
                CodePartenaire = "10234",
                NomPartenaire = "Courtage de France",
                Interlocuteur = "Mme Savineau",
                TypeCourrierPart = "",
                NbExp = "3"
            };
            toReturn.Add(courrier);
            courrier = new GestionDocumentCourrierDto
            {
                CodeCourrier = "2",
                TypePartenaire = "COURT",
                CodePartenaire = "142095",
                NomPartenaire = "Festival d'art lyrique d'Aix en Provence",
                Interlocuteur = "",
                TypeCourrierPart = "",
                NbExp = "1"
            };
            toReturn.Add(courrier);

            return toReturn;
        }

        private static List<GestionDocumentCourrierDto> RemplirEmail()
        {
            List<GestionDocumentCourrierDto> toReturn = new List<GestionDocumentCourrierDto>();

            GestionDocumentCourrierDto email = new GestionDocumentCourrierDto
            {
                CodeCourrier = "1",
                TypePartenaire = "COURT",
                CodePartenaire = "10234",
                NomPartenaire = "Courtage de France",
                Interlocuteur = "Mme Savineau",
                DestinatairePart = "To"
            };
            toReturn.Add(email);

            return toReturn;
        }

        [Obsolete]
        private static string GetDocumentTexteCode(string codeOffre, int? version, string type, string tableW)
        {
            DbParameter[] param = new DbParameter[8];

            string sql = string.Format(@"SELECT KEQID CODEDOCUMENT, KEQKESID CODETEXTE
                        FROM KDOC{0}
                        WHERE KEQIPB = :KEQIPB AND KEQALX = :KEQALX AND KEQTYP = :KEQTYP AND KEQSERV = :KEQSERV
                            AND KEQACTG = :KEQACTG AND KEQETAP = :KEQETAP AND KEQDACC = :KEQDACC AND KEQAJT = :KEQAJT", tableW);

            param[0] = new EacParameter("KEQIPB", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("KEQALX", version);
            param[2] = new EacParameter("KEQTYP", type);
            param[3] = new EacParameter("KEQSERV", "PRODU");
            param[4] = new EacParameter("KEQACTG", "OFFRE");
            param[5] = new EacParameter("KEQETAP", "DOCU");
            param[6] = new EacParameter("KEQDACC", "N");
            param[7] = new EacParameter("KEQAJT", "N");

            var res = !string.IsNullOrEmpty(sql) ? DbBase.Settings.ExecuteList<GestionDocumentTableWDto>(CommandType.Text, sql, param) : null;

            string codeDocuments = string.Empty;
            string codeTextes = string.Empty;

            if (res != null)
            {
                res.ForEach(el =>
                {
                    codeDocuments += "," + el.CodeDocument.ToString();
                    codeTextes += "," + el.CodeTexte.ToString();
                });

                if (!string.IsNullOrEmpty(codeDocuments))
                    codeDocuments.Substring(1);
                if (!string.IsNullOrEmpty(codeTextes))
                    codeTextes.Substring(1);
            }

            return codeDocuments + "_" + codeTextes;
        }

        [Obsolete]
        private static string GetDiffusionCodes(string codeDocuments, string tableW)
        {
            string sql = string.Format(@"SELECT KERAEMT DIFFUSIONCODETEXTE, KERDOCA DIFFUSIONCODEACCOMPAGNANT 
                        FROM KDOCDF{0}
                        WHERE KERKEQID IN ({1})", tableW, codeDocuments);
            var res = !string.IsNullOrEmpty(sql) ? DbBase.Settings.ExecuteList<GestionDocumentTableWDto>(CommandType.Text, sql) : null;

            string diffusionCodeTextes = string.Empty;
            string diffusionCodeAccompagnant = string.Empty;

            if (res != null)
            {

                res.ForEach(el =>
                {
                    diffusionCodeTextes += "," + el.DiffusionCodeTexte.ToString();
                    diffusionCodeAccompagnant += "," + el.DiffusionCodeAccompagnant.ToString();
                });

                if (!string.IsNullOrEmpty(diffusionCodeTextes))
                    diffusionCodeTextes.Substring(1);
                if (!string.IsNullOrEmpty(diffusionCodeAccompagnant))
                    diffusionCodeAccompagnant.Substring(1);
            }

            return diffusionCodeTextes + "_" + diffusionCodeAccompagnant;
        }

        [Obsolete]
        private static string GetDocumentCodes(string codeDocuments, string tableW)
        {
            string sql = string.Format(@"SELECT KEQKESID CODETEXTE FROM KDOC{0} WHERE KEQID IN ({1})", tableW, codeDocuments);
            var res = DbBase.Settings.ExecuteList<GestionDocumentTableWDto>(CommandType.Text, sql);

            string codeTextes = string.Empty;

            if (res != null)
            {
                res.ForEach(el =>
                {
                    codeTextes += "," + el.CodeTexte.ToString();
                });

                if (!string.IsNullOrEmpty(codeTextes))
                    codeTextes.Substring(1);
            }
            return codeTextes;
        }

        #region Suppression infos tables documents

        [Obsolete]
        private static void DeleteDocuments(string codeOffre, int? version, string type, string tableW = "")
        {
            string codeDocuments = string.Empty;
            string codeTextes = string.Empty;

            string codeDocText = GetDocumentTexteCode(codeOffre, version, type, tableW);
            if (codeDocText != "_")
            {
                codeDocuments = codeDocText.Split('_')[0];
                codeTextes = codeDocText.Split('_')[1];

                DeleteKDOCJN(codeDocuments, tableW);
                DeleteKDOCTX(codeTextes, tableW);
                DeleteKDOCDF(codeDocuments, tableW);
                DeleteKDOC(codeDocuments, tableW);
            }
        }

        [Obsolete]
        private static void DeleteKDOCJN(string codeDocuments, string tableW)
        {
            string sql = string.Format(@"DELETE FROM KDOCJN{0} WHERE KETKEQID IN ({1})", tableW, codeDocuments);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        [Obsolete]
        private static void DeleteKDOCTX(string codeTextes, string tableW)
        {
            string sql = string.Format(@"DELETE FROM KDOCTX{0} WHERE KESID IN ({1})", tableW, codeTextes);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        [Obsolete]
        private static void DeleteKDOCDF(string codeDocuments, string tableW)
        {
            string diffusionCodeTextes = string.Empty;
            string diffusionCodeAccompagnant = string.Empty;

            string diffusionCodes = GetDiffusionCodes(codeDocuments, tableW);

            if (diffusionCodes != "_")
            {
                diffusionCodeTextes = diffusionCodes.Split('_')[0];
                diffusionCodeAccompagnant = diffusionCodes.Split('_')[1];

                DeleteKDOCTX(diffusionCodeTextes, tableW);
                DeleteKDOC(diffusionCodeAccompagnant, tableW, true);
            }

            string sql = string.Format(@"DELETE FROM KDOCDF{0} WHERE KERKEQID IN ({1})", tableW, codeDocuments);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        [Obsolete]
        private static void DeleteKDOC(string codeDocuments, string tableW, bool diffusion = false)
        {
            string sql = string.Empty;
            if (diffusion)
            {
                string codeTextes = GetDocumentCodes(codeDocuments, tableW);

                if (!string.IsNullOrEmpty(codeTextes))
                {
                    DeleteKDOCTX(codeTextes, tableW);
                }
            }

            sql = string.Format(@"DELETE FROM KDOC{0} WHERE KEQID IN ({1})", tableW, codeDocuments);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        #endregion

        #region Copie infos tables documents

        [Obsolete]
        private static void CopyDocuments(string codeOffre, int? version, string type, string tableW = "")
        {
            string codeDocuments = string.Empty;
            string codeTextes = string.Empty;

            string codeDocText = GetDocumentTexteCode(codeOffre, version, type, tableW);

            if (codeDocText != "_")
            {
                codeDocuments = codeDocText.Split('_')[0];
                codeTextes = codeDocText.Split('_')[1];

                CopyKDOCJN(codeDocuments, tableW);
                CopyKDOCTX(codeTextes, tableW);
                CopyKDOCDF(codeDocuments, tableW);
                CopyKDOC(codeDocuments, tableW);
            }
        }

        [Obsolete]
        private static void CopyKDOCJN(string codeDocuments, string tableW)
        {
            string sql = string.Format(@"INSERT INTO KDOCJN{0} SELECT * FROM  KDOCJN{1} WHERE KETKEQID IN ({2})", !string.IsNullOrEmpty(tableW) ? "" : "W", tableW, codeDocuments);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        [Obsolete]
        private static void CopyKDOCTX(string codeTextes, string tableW)
        {
            string sql = string.Format(@"INSERT INTO KDOCTX{0} SELECT * FROM KDOCTX{1} WHERE KESID IN ({2})", !string.IsNullOrEmpty(tableW) ? "" : "W", tableW, codeTextes);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        [Obsolete]
        private static void CopyKDOCDF(string codeDocuments, string tableW)
        {
            string diffusionCodeTextes = string.Empty;
            string diffusionCodeAccompagnant = string.Empty;

            string diffusionCodes = GetDiffusionCodes(codeDocuments, tableW);

            if (diffusionCodes != "_")
            {
                diffusionCodeTextes = diffusionCodes.Split('_')[0];
                diffusionCodeAccompagnant = diffusionCodes.Split('_')[1];

                CopyKDOCTX(diffusionCodeTextes, tableW);
                CopyKDOC(diffusionCodeAccompagnant, tableW, true);
            }

            string sql = string.Format(@"INSERT INTO KDOCDF{0} SELECT * FROM KDOCDF{1} WHERE KERKEQID IN ({2})", !string.IsNullOrEmpty(tableW) ? "" : "W", tableW, codeDocuments);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        [Obsolete]
        private static void CopyKDOC(string codeDocuments, string tableW, bool diffusion = false)
        {
            string sql = string.Empty;
            if (diffusion)
            {
                string codeTextes = GetDocumentCodes(codeDocuments, tableW);

                if (!string.IsNullOrEmpty(codeTextes))
                {
                    CopyKDOCTX(codeTextes, tableW);
                }
            }

            sql = string.Format(@"INSERT INTO KDOC{0} SELECT * FROM KDOC{1} WHERE KEQID IN ({2})", !string.IsNullOrEmpty(tableW) ? "" : "W", tableW, codeDocuments);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        #endregion

        #region Génération infos tables documents

        private static string GetElementCodes(string codeOffre, int? version, string type, string branche)
        {
            DbParameter[] param = new DbParameter[3];
            string sql = @"SELECT KEJID CODEELEMENT 
                        FROM KALCELG 
                        WHERE KEJSERV = :KEJSERV AND KEJACTG = :KEJACTG AND KEJETAPE = :KEJETAP";

            param[0] = new EacParameter("KEJSERV", "PRODU");
            param[1] = new EacParameter("KEJACTG", "OFFRE");
            param[2] = new EacParameter("KEJETAP", "DOCU");

            var res = DbBase.Settings.ExecuteList<ElementGenerateurDto>(CommandType.Text, sql, param);

            string codeElements = string.Empty;
            if (res != null)
            {
                res.ForEach(el =>
                {
                    codeElements += "," + el.CodeElement.ToString();
                });

                if (!string.IsNullOrEmpty(codeElements))
                    codeElements.Substring(1);
            }

            return codeElements;
        }

        private static bool CheckConditionsBranche(string codeElements, string codeBranche)
        {
            bool toReturn = true;
            string sql = string.Format(@"SELECT COUNT(*) NBENREGISTREMENTS 
                                    FROM KALCELB            
                                    WHERE (KEKKEJID IN ({0}))", codeElements);
            var res = DbBase.Settings.ExecuteList<ElementGenerateurConditionBrancheDto>(CommandType.Text, sql).FirstOrDefault();

            if (res != null)
            {
                if (res.NbEnregistrements > 0)
                {
                    sql = string.Format(@"SELECT COUNT(*) NBENREGISTREMENTS 
                                    FROM KALCELB            
                                    WHERE (KEKKEJID IN {0} AND KEKBRA = '{1}'", codeElements, codeBranche);
                    var resB = DbBase.Settings.ExecuteList<ElementGenerateurConditionBrancheDto>(CommandType.Text, sql).FirstOrDefault();
                    if (resB != null)
                    {
                        if (resB.NbEnregistrements == 0)
                            toReturn = false;
                    }
                }
            }
            return toReturn;
        }

        private static List<DocumentParametresDto> GetDocumentsParam(string codeElements)
        {
            string sql = string.Format(@"SELECT KEMID CODEDOCUMENT, KEMSERV SERVICE, KEMACTG ACTEGESTION, KEMETAPE ETAPE, KEMORD NUMORDRE,
                                        KEMTDOC TYPEDOC, KEMCDOC CODEDOC, KEMNTA NATUREGENERATION, KEMTAE ACTIONENCHAINEE, KEMKEOID CODETEXTE
                                    FROM KALCELD
                                    WHERE KEMKEJID IN ({0})
                                    ORDER BY KEMORD", codeElements);
            return DbBase.Settings.ExecuteList<DocumentParametresDto>(CommandType.Text, sql);
        }

        private static List<DocumentParametresDiffusionDto> GetDiffusionsParam(int codeDocParam)
        {
            DbParameter[] param = new DbParameter[1];
            string sql = @"SELECT KENORD NUMORDRE, KENTYENV TYPEENVOI, KENNAT NATURE, KENTYDIF TYPEDIFFUSION, KENTYDS TYPEDESTINATAIRE, KENKEOID CODEACCOMPAGNANT, KENNBEX NBEXEMPLAIRE
                            FROM KALCELF
                        WHERE KENKEMID = :KENKEMID
                        ORDER BY KENORD";
            param[0] = new EacParameter("KENKEMID", codeDocParam);

            return DbBase.Settings.ExecuteList<DocumentParametresDiffusionDto>(CommandType.Text, sql, param);
        }

        [Obsolete]
        private static void GenerateDocuments(string codeOffre, int? version, string type, string branche, string user)
        {
            string codeElements = GetElementCodes(codeOffre, version, type, branche);

            if (!string.IsNullOrEmpty(codeElements))
            {
                bool grantedBranche = CheckConditionsBranche(codeElements, branche);

                if (grantedBranche)
                {
                    List<DocumentParametresDto> docsParam = GetDocumentsParam(codeElements);
                    if (docsParam != null)
                    {
                        foreach (var docParam in docsParam)
                        {
                            int codeDocument = InsertKDOCW(codeOffre, version, type, docParam, user);
                            GenerateDiffusions(codeOffre, version, type, docParam.CodeDocument, codeDocument);
                        }
                    }
                }
            }
        }

        [Obsolete]
        private static int InsertKDOCW(string codeOffre, int? version, string type, DocumentParametresDto docParam, string user)
        {
            DbParameter[] param = new DbParameter[34];
            string sql = @"insert into kdocw
                            (KEQID, KEQTYP, KEQIPB, KEQALX, KEQSUA, KEQNUM, KEQSBR, KEQSERV, KEQACTG, KEQETAP, KEQKEMID, KEQORD, KEQTDOC, KEQCDOC, KEQVER, KEQLIB, KEQNTA, 
                            KEQAJT, KEQTRS, KEQDACC, KEQTAE, KEQKESID, KEQNOM, KEQCHM, KEQSTU, KEQSIT, KEQSTD, KEQSTH, KEQCRU, KEQCRD, KEQCRH, KEQMAJU, KEQMAJD, KEQMAJH)
                            values
                            (:KEQID, :KEQTYP, :KEQIPB, :KEQALX, :KEQSUA, :KEQNUM, :KEQSBR, :KEQSERV, :KEQACTG, :KEQETAP, :KEQKEMID, :KEQORD, :KEQTDOC, :KEQCDOC, :KEQVER, :KEQLIB, :KEQNTA, 
                            :KEQAJT, :KEQTRS, :KEQDACC, :KEQTAE, :KEQKESID, :KEQNOM, :KEQCHM, :KEQSTU, :KEQSIT, :KEQSTD, :KEQSTH, :KEQCRU, :KEQCRD, :KEQCRH, :KEQMAJU, :KEQMAJD, :KEQMAJH)";

            int codeDocument = CommonRepository.GetAS400Id("KEQID");

            int? date = AlbConvert.ConvertDateToInt(DateTime.Now);
            int? time = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(DateTime.Now));
            int rechVersion = 0;
            string rechLib = string.Empty;
            int codeTexte = 0;

            if (docParam.TypeDoc == "LETTYP")
            {
                string rechInfo = GetInfoLetTyp(docParam.CodeDoc);
                if (!string.IsNullOrEmpty(rechInfo) && rechInfo != "_")
                {
                    rechVersion = Convert.ToInt32(rechInfo.Split('_')[0]);
                    rechLib = rechInfo.Split('_')[1];
                }
                else
                {
                    rechVersion = 0;
                    rechLib = "Aucun document trouvé à cette date";
                }
            }

            if (docParam.CodeTexte != 0)
            {
                codeTexte = GetInfoTexte(docParam.CodeTexte);
            }

            param[0] = new EacParameter("KEQID", codeDocument);
            param[1] = new EacParameter("KEQTYP", type);
            param[2] = new EacParameter("KEQIPB", codeOffre.PadLeft(9, ' '));
            param[3] = new EacParameter("KEQALX", version);
            param[4] = new EacParameter("KEQSUA", 0);
            param[5] = new EacParameter("KEQNUM", 0);
            param[6] = new EacParameter("KEQSBR", "");
            param[7] = new EacParameter("KEQSERV", docParam.Service);
            param[8] = new EacParameter("KEQACTG", docParam.ActeGestion);
            param[9] = new EacParameter("KEQETAP", docParam.Etape);
            param[10] = new EacParameter("KEQKEMID", docParam.CodeDocument);
            param[11] = new EacParameter("KEQORD", docParam.NumOrdre);
            param[12] = new EacParameter("KEQTDOC", docParam.TypeDoc);
            param[13] = new EacParameter("KEQCDOC", docParam.CodeDoc);
            param[14] = new EacParameter("KEQVER", rechVersion);
            param[15] = new EacParameter("KEQLIB", rechLib);
            param[16] = new EacParameter("KEQNTA", docParam.NatureGeneration);
            param[17] = new EacParameter("KEQAJT", "N");
            param[18] = new EacParameter("KEQTRS", "N");
            param[19] = new EacParameter("KEQDACC", "N");
            param[20] = new EacParameter("KEQTAE", docParam.ActionEnchainee);
            param[21] = new EacParameter("KEQKESID", codeTexte);
            param[22] = new EacParameter("KEQNOM", "");
            param[23] = new EacParameter("KEQCHM", "");
            param[24] = new EacParameter("KEQSTU", user);
            param[25] = new EacParameter("KEQSIT", "A");
            param[26] = new EacParameter("KEQSTD", date);
            param[27] = new EacParameter("KEQSTH", time);
            param[28] = new EacParameter("KEQCRU", user);
            param[29] = new EacParameter("KEQCRD", date);
            param[30] = new EacParameter("KEQCRH", time);
            param[31] = new EacParameter("KEQMAJU", user);
            param[32] = new EacParameter("KEQMAJD", date);
            param[33] = new EacParameter("KEQMAJH", time);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return codeDocument;
        }

        [Obsolete]
        private static int InsertKDOCTXW(string texte)
        {
            int codeTexte = CommonRepository.GetAS400Id("KESID");

            DbParameter[] param = new DbParameter[2];
            string sql = @"INSERT INTO KDOCTXW
                            (KESID, KESTXT)
                            VALUES
                           (:KESID, :KESTXT)";
            param[0] = new EacParameter("KESID", codeTexte);
            param[1] = new EacParameter("KESTXT", texte);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return codeTexte;
        }

        [Obsolete]
        private static void GenerateDiffusions(string codeOffre, int? version, string type, int codeDocParam, int codeDocument)
        {
            List<DocumentParametresDiffusionDto> diffusionsParam = GetDiffusionsParam(codeDocParam);
            if (diffusionsParam != null)
            {
                foreach (var diffParam in diffusionsParam)
                {
                    InsertKDOCDFW(codeOffre, version, type, diffParam, codeDocument);
                }
            }
        }

        [Obsolete]
        private static void InsertKDOCDFW(string codeOffre, int? version, string type, DocumentParametresDiffusionDto diffParam, int codeDocument)
        {
            int codeDiffusion = CommonRepository.GetAS400Id("KERID");

            int codeTexte = 0;
            if (diffParam.CodeAccompagnant != 0)
            {
                codeTexte = GetInfoTexte(diffParam.CodeAccompagnant);
            }
            OffreInfoDto offreDestinataire = GetOffreDestinataire(codeOffre, version, type);
            int codeCourtier = 0;
            int codeAssure = 0;

            if (offreDestinataire != null)
            {
                codeAssure = diffParam.TypeDestinataire == "ASS" ? offreDestinataire.CodeAssure : 0;
                codeCourtier = diffParam.TypeDestinataire == "COURT" ? offreDestinataire.CodeCourtier : 0;
            }

            if (codeAssure != 0 || codeCourtier != 0)
            {
                DbParameter[] param = new DbParameter[15];
                string sql = @"INSERT INTO KDOCDFW
                            (KERID, KERKEQID, KERORD, KERTYENV, KERTYDS, KERTYI, KERIDS, KERINL, KERNAT, KERNBEX, KERDOCA, KERTYDIF, KERAEMO, KERAEM, KERAEMT)
                            VALUES
                            (:KERID, :KERKEQID, :KERORD, :KERTYENV, :KERTYDS, :KERTYI, :KERIDS, :KERINL, :KERNAT, :KERNBEX, :KERDOCA, :KERTYDIF, :KERAEMO, :KERAEM, :KERAEMT)";

                param[0] = new EacParameter("KERID", codeDiffusion);
                param[1] = new EacParameter("KERKEQID", codeDocument);
                param[2] = new EacParameter("KERORD", diffParam.NumOrdre);
                param[3] = new EacParameter("KERTYENV", diffParam.TypeEnvoi);
                param[4] = new EacParameter("KERTYDS", diffParam.TypeDestinataire);
                param[5] = new EacParameter("KERTYI", "");
                param[6] = new EacParameter("KERIDS", diffParam.TypeDestinataire == "ASS" ? codeAssure : codeCourtier);
                param[7] = new EacParameter("KERINL", 0);
                param[8] = new EacParameter("KERNAT", diffParam.Nature);
                param[9] = new EacParameter("KERNBEX", diffParam.NbExemplaire);
                param[10] = new EacParameter("KERDOCA", 0);
                param[11] = new EacParameter("KERTYDIF", diffParam.TypeDiffusion);
                param[12] = new EacParameter("KERAEMO", "");
                param[13] = new EacParameter("KERAEM", "");
                param[14] = new EacParameter("KERAEMT", codeTexte);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
        }

        private static OffreInfoDto GetOffreDestinataire(string codeOffre, int? version, string type)
        {
            string toReturn = string.Empty;

            DbParameter[] param = new DbParameter[3];
            string sql = @"SELECT PBIAS CODEASSURE, PBICT CODECOURTIER
                        FROM YPOBASE
                        WHERE PBIPB = :PBIPB AND PBALX = :PBALX AND PBTYP = :PBTYP";
            param[0] = new EacParameter("PBIPB", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("PBALX", version);
            param[2] = new EacParameter("PBTYP", type);

            return DbBase.Settings.ExecuteList<OffreInfoDto>(CommandType.Text, sql, param).FirstOrDefault();
        }

        private static string GetInfoLetTyp(string codeDoc)
        {
            string toReturn = string.Empty;

            DateTime date = DateTime.Now;

            DbParameter[] param = new DbParameter[4];
            string sql = @"SELECT LTVER NUMVERSION, LTLBL LIBELLE 
                    FROM YLETTYP
                    WHERE LTLET = :LTLET AND LTVEA = :LTVEA AND LTVEM = :LTVEM AND LTVEJ = :LTVEJ";

            param[0] = new EacParameter("LTLET", codeDoc);
            param[1] = new EacParameter("LTVEA", date.Year);
            param[1] = new EacParameter("LTVEM", date.Month);
            param[1] = new EacParameter("LTVEJ", date.Day);

            var res = DbBase.Settings.ExecuteList<LettreTypeDto>(CommandType.Text, sql, param).FirstOrDefault();

            if (res != null)
            {
                toReturn = res.NumVersion + "_" + res.Libelle;
            }

            return toReturn;
        }

        [Obsolete]
        private static int GetInfoTexte(int codeTexte)
        {
            int toReturn = 0;

            DbParameter[] param = new DbParameter[1];
            string sql = @"SELECT KEOTXT TEXTE FROM KALCELT WHERE KEOID = :KEOID";
            param[0] = new EacParameter("KEOID", codeTexte);

            var res = DbBase.Settings.ExecuteList<DocumentParametresTexteDto>(CommandType.Text, sql, param).FirstOrDefault();

            if (res != null)
            {
                toReturn = !string.IsNullOrEmpty(res.Texte) ? InsertKDOCTXW(res.Texte) : 0;
            }

            return toReturn;
        }

        #endregion

        #region Récupération infos tables documents

        [Obsolete]
        private static List<DocumentDto> GetGestionDocuments(string codeOffre, int? version, string type)
        {
            DbParameter[] param = new DbParameter[6];
            string sql = @"SELECT KEQID CODEDOCUMENT, KEQLIB LIBELLE, KERTYDIF DIFFUSION, KERTYDS TYPEPARTENAIRE, 
                            KERIDS CODEDESTINATAIRE, ANNOM NOMASSURE, TNNOM NOMCOURTIER, KEQCRD DATECREATION, 
                            KEQSIT CODESITUATION, KEQSTD DATESITUATION
                        FROM KDOCW
                            INNER JOIN KDOCDFW ON KERKEQID = KEQID
                            INNER JOIN YASSNOM ON ANIAS = KERIDS AND ANTNM = 'A' AND ANINL = 0
                            INNER JOIN YCOURTN ON TNICT = KERIDS AND TNTNM = 'A' AND TNXN5 = 0
                        WHERE KEQTYP = :KEQTYP AND KEQIPB = :KEQIPB AND KEQALX = :KEQALX AND KEQSERV = :KEQSERV AND KEQACTG = :KEQACTG AND KEQETAP = :KEQETAP
                        ORDER BY KEQORD, KERORD";
            param[0] = new EacParameter("KEQTYP", type);
            param[1] = new EacParameter("KEQIPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("KEQALX", version);
            param[3] = new EacParameter("KEQSERV", "PRODU");
            param[4] = new EacParameter("KEQACTG", "OFFRE");
            param[5] = new EacParameter("KEQETAP", "DOCU");

            var res = DbBase.Settings.ExecuteList<DocumentQueryDto>(CommandType.Text, sql, param);

            List<DocumentDto> documents = new List<DocumentDto>();

            var documentDto = res.GroupBy(d => d.CodeDocument).Select(d => d.First());
            if (documentDto != null)
            {
                documentDto.ToList().ForEach(el =>
                {
                    DocumentDto doc = new DocumentDto
                    {
                        CodeDoc = el.CodeDocument,
                        NomDoc = el.Libelle
                    };

                    List<GestionDocumentDiffusionDto> diffusions = new List<GestionDocumentDiffusionDto>();

                    var diffusionDto = res.FindAll(diff => diff.CodeDocument == el.CodeDocument);
                    if (diffusionDto != null)
                    {
                        diffusionDto.ToList().ForEach(d =>
                        {
                            GestionDocumentDiffusionDto gesDiff = new GestionDocumentDiffusionDto
                            {
                                TypeDiffusion = d.Diffusion,
                                Partenaire = d.TypePartenaire,
                                Destinataire = string.Format("{0} - {1}", d.CodeDestinataire, d.TypePartenaire == "ASS" ? d.NomAssure : d.NomCourtier),
                                CreationDate = AlbConvert.ConvertIntToDate(d.DateCreation),
                                TraitementDate = AlbConvert.ConvertIntToDate(d.DateSituation)
                            };
                            diffusions.Add(gesDiff);
                        });
                    }

                    doc.Diffusions = diffusions;
                    documents.Add(doc);
                });
            }
            return documents;
        }

        #endregion

        #endregion

        #endregion
        #region Quittance
        public static List<QuittanceDto> GetQuittances(string codeOffre, string version, string type, string codeAvn, string modeAffichage, string numQuittanceVisu, ModeConsultation modeNavig, string user, string acteGestion, string reguleId)
        {
            bool isRegule = acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF;
            bool isReguleWithId = isRegule && !string.IsNullOrEmpty(reguleId);
            bool isVisu = modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu);
            bool isHisto = modeNavig == ModeConsultation.Historique;
            int avn = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
            string regId = !string.IsNullOrEmpty(reguleId) ? reguleId : "0";
            string sql = $@"
SELECT PBPER PERIODICITE, JDPEJ PROCHECHJOUR, JDPEM PROCHECHMOIS, JDPEA PROCHECHAN, JDEHH MNTHTHC, JDEHC MNTC,
PTFOR CODEFORMULE, KDAALPHA LETTREFORMULE, KDADESC LIBELLEFORMULE,
PTRSQ CODERISQUE,KABDESC LIBELLERISQUE,
( PTMHT - PTCNH ) HTHORSCATNAT,
PTCNH CATNAT,
PTTTT TAXES,
PTMTT TTC,
PTFPA FINEFFETANNEE,
PTFPM FINEFFETMOIS,
PTFPJ FINEFFETJOUR,
TBLPRIM.PKPER CODEPERIODICITE,
PERIOD.TPLIB LIBELLEPERIODICITE,
TBLPRIM.PKOPE CODEOPERATION,
OPER.TPLIB LIBELLEOPERATION,
TBLPRIM.PKIDV INDICE,
TBLPRIM.PKDPA DEBUTPERIODEANNEE,
TBLPRIM.PKDPM DEBUTPERIODEMOIS,
TBLPRIM.PKDPJ DEBUTPERIODEJOUR,
TBLPRIM.PKPFJ FINPERIODEJOUR, TBLPRIM.PKFPM FINPERIODEMOIS, TBLPRIM.PKFPA FINPERIODEANNEE,
{(isReguleWithId ? "KHWXCM" : "TBLPRIM.PKCOM")} TAUXHRCATNATRETENU,
TBLPRIM.PKKCO MONTANTCOMMISSHRCATNATRETENU,
{(isReguleWithId ? "KHWCNC" : "TBLPRIM.PKCNC")} TAUXCATNATRETENU,
TBLPRIM.PKKNM MONTANTCOMMISSRETENU,
TBLPRIM.PKKCO + TBLPRIM.PKKNM TOTALCOMMISSRETENU,
TBLPRIM.PKKHT - TBLPRIM.PKKNH TOTALHRCATNATHT,
TBLPRIM.PKKHX TOTALHRCATNATTAXE,
TBLPRIM.PKKHT - TBLPRIM.PKKNH + TBLPRIM.PKKHX TOTALHRCATNATTTC,
TBLPRIM.PKKNH CATNATHT,
TBLPRIM.PKKNT CATNATTAXE,
TBLPRIM.PKKNL CATNATTTC,
TBLPRIM.PKKHT TOTALHRFRAISHT,
TBLPRIM.PKKTX - TBLPRIM.PKKFT - TBLPRIM.PKKAT TOTALHRFRAISTAXE,
TBLPRIM.PKKTT - TBLPRIM.PKKFA - TBLPRIM.PKKFT - TBLPRIM.PKKAT TOTALHRFRAISTTC,
TBLPRIM.PKKFA FRAISHT,
TBLPRIM.PKKFT FRAISTAXE,
TBLPRIM.PKKFA + TBLPRIM.PKKFT FRAISTTC,
TBLPRIM.PKKAT FGATAXE,
TBLPRIM.PKKAT FGATTC,
( TBLPRIM.PKKHT + TBLPRIM.PKKFA ) MONTANTTOTALHT , 
TBLPRIM.PKKTX MONTANTTOTALTAXE , 
TBLPRIM.PKKTT MONTANTTOTALTTC 
FROM {(isVisu ? "YPRIMES" : GetSuffixeRegule(acteGestion, "YPRIPES"))} TBLPRIM 
LEFT JOIN {(isVisu ? "YPRIMTA" : GetSuffixeRegule(acteGestion, "YPRIPTA"))} ON PTIPB = TBLPRIM.PKIPB AND PTALX = TBLPRIM.PKALX AND PTMTT != 0 {If(isVisu, " AND PTIPK = TBLPRIM.PKIPK")} 
LEFT JOIN {GetPrefixeHisto(modeNavig, "YPOBASE")} ON PBIPB = PKIPB AND PBALX = PKALX AND PBTYP = :typ {If(isHisto, $" AND PBAVN = {avn}")} 
LEFT JOIN {GetPrefixeHisto(modeNavig, "YPRTENT")} ON JDIPB = PKIPB AND JDALX = PKALX {If(isHisto, $" AND JDAVN = {avn}")} 
LEFT JOIN {GetPrefixeHisto(modeNavig, "KPFOR")} ON KDAIPB = PTIPB AND KDAALX = PTALX AND KDATYP = :typ {If(isHisto, $" AND KDAAVN = {avn}")} AND KDAFOR = PTFOR 
LEFT JOIN {GetPrefixeHisto(modeNavig, "KPRSQ")} ON KABIPB = PTIPB AND  KABALX = PTALX AND KABTYP = :typ {If(isHisto, $" AND KABAVN = {avn}")} AND KABRSQ = PTRSQ 
{If(isRegule, $" LEFT JOIN KPRGU ON KHWID = :regId")} 
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBPER", "PERIOD", " AND PERIOD.TCOD = TBLPRIM.PKPER")} 
{BuildJoinYYYYPAR("LEFT", "PRODU", "PKOPE", "OPER", " AND OPER.TCOD = TBLPRIM.PKOPE")} 
WHERE PKIPB = :ipb AND PKALX = :alx {If(isVisu, " AND PKIPK = " + numQuittanceVisu)} ";

            var parameters = new List<EacParameter>() {
                new EacParameter("typ", type),
                new EacParameter("ipb", codeOffre.ToIPB()),
                new EacParameter("alx", DbType.Int32) { Value = int.Parse(version) }
            };
            if (isRegule)
            {
                parameters.Insert(1, new EacParameter("regId", DbType.Int32) { Value = regId });
            }

            var toReturn = DbBase.Settings.ExecuteList<QuittanceDto>(CommandType.Text, sql, parameters.ToArray());

            if (toReturn != null && toReturn.Any() && !string.IsNullOrEmpty(user) && modeNavig == ModeConsultation.Standard)
            {
                var res = LoadAS400Commissions(codeOffre, version, type, codeAvn, user, acteGestion);
                if (res != null)
                {
                    toReturn[0].TauxStd = res.TauxStandardHCAT.ToString();
                    toReturn[0].TauxStdCatNat = res.TauxStandardCAT.ToString();
                }
            }
            return toReturn;
        }
        public static QuittanceDetailDto GetQuittanceDetail(string codeOffre, string version, string codeAvn, string modeAffichage, string numQuittanceVisu, ModeConsultation modeNavig, string acteGestion)
        {
            string sql = string.Format(@"SELECT 
                    PBNPL CODENATURECONTRAT,NATURE.TPLIB LIBELLENATURECONTRAT,
                    PBAPP PART,
                    TBLPRIM.PKAVN AVENANT,
                    TBLPRIM.PKDEV DEVISE,
                    TBLPRIM.PKOPE CODEOPERATION,OPERATION.TPLIB LIBELLEOPERATION,
                    TBLPRIM.PKKCA CAPITAL,
                    TBLPRIM.PKPER CODEPERIODICITE,PERIODICITE.TPLIB LIBELLEPERIODICITE,
                    TBLPRIM.PKDPA DEBUTPERIODEANNEE,TBLPRIM.PKDPM DEBUTPERIODEMOIS,TBLPRIM.PKDPJ DEBUTPERIODEJOUR,
                    TBLPRIM.PKFPA FINPERIODEANNEE,TBLPRIM.PKFPM FINPERIODEMOIS,TBLPRIM.PKPFJ FINPERIODEJOUR,
                    PBFEA FINEFFETPOLICEANNEE, PBFEM FINEFFETPOLICEMOIS, PBFEJ FINEFFETPOLICEJOUR,
                    PBCTD DUREEEFFETPOLICE, PBCTU UNITEDUREEEFFETPOLICE,
                    TBLPRIM.PKEMT CODEEMISSION,EMISSION.TPLIB LIBELLEEMISSION,
                    TBLPRIM.PKEMA DATEEMISSIONANNEE, TBLPRIM.PKEMM DATEEMISSIONMOIS, TBLPRIM.PKEMJ DATEEMISSIONJOUR,
                    TBLPRIM.PKEHA DATEECHEANCEANNEE,TBLPRIM.PKEHM DATEECHEANCEMOIS,TBLPRIM.PKEHJ DATEECHEANCEJOUR,
                    TBLPRIM.PKSIT CODESITUATION,SITUATION.TPLIB LIBELLESITUATION,
                    TBLPRIM.PKSTA DATESITUATIONANNEE,TBLPRIM.PKSTM DATESITUATIONMOIS,TBLPRIM.PKSTJ DATESITUATIONJOUR,
                    TBLPRIM.PKRLC CODERELANCE,RELANCE.TPLIB LIBELLERELANCE,
                    TBLPRIM.PKRLA DATERELANCEANNEE,TBLPRIM.PKRLM DATERELANCEMOIS,TBLPRIM.PKRLJ DATERELANCEJOUR,
                    TBLPRIM.PKIDV INDICE,
                    TBLPRIM.PKTAC CODEACCORD,ACCORD.TPLIB LIBELLEACCORD,
                    TBLPRIM.PKMVT CODEMOUVEMENT,MOUVEMENT.TPLIB LIBELLEMOUVEMENT,
                    TBLPRIM.PKCPT COMPTABILISE,
                    TBLPRIM.PKCPA DATECOMPTANNEE,TBLPRIM.PKCPM DATECOMPTMOIS,
                    TBLPRIM.PKENC CODEENCAISSEMENT,ENCAISSEMENT.TPLIB LIBELLEENCAISSEMENT,
                    PNICT PNICTICONE,TBLPRIM.PKICT PKICTICONE,
                    TBLPRIM.PKICT CODECOURTIER,TNNOM NOMCOURTIER,TCDEP DEPCOURTIER,TCCPO CPOCOURTIER,TCVIL VILLCOURTIER,
                    TBLPRIM.PKCRU USERCREATION,TBLPRIM.PKCRA DATECREATIONANNEE,TBLPRIM.PKCRM DATECREATIONMOIS,TBLPRIM.PKCRJ DATECREATIONJOUR,
                    TBLPRIM.PKMJU USERUPDATE,TBLPRIM.PKMJA DATEUPDATEANNEE,TBLPRIM.PKMJM DATEUPDATEMOIS,TBLPRIM.PKMJJ DATEUPDATEJOUR,
                    (SELECT COUNT(*) FROM YPOCOUR WHERE PFIPB='{0}' AND PFALX = {1}) NOMBRECOCOUTRIER,
                    TBLPRIM.PKKTT-TBLPRIM.PKKCO-TBLPRIM.PKKNM TTCAREGLER, 
                    TBLPRIM.PKKTR REGLE
                    FROM {13}
                    {11}
                    {2}
                    {3}
                    {4}
                    {5}
                    {6}
                    {7}
                    {8}
                    {9}
                    {10}
                    {12}
                    LEFT JOIN YCOURTN ON TNICT=TBLPRIM.PKICT AND TNXN5=0 AND TNTNM='A'
                    LEFT JOIN YCOURTI ON TCICT=PBICT
                    WHERE PBIPB='{0}' AND PBALX='{1}' {14}", codeOffre.PadLeft(9, ' '), version,
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBNPL", "NATURE", " AND NATURE.TCOD = PBNPL"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PKOPE", "OPERATION", " AND OPERATION.TCOD = TBLPRIM.PKOPE"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBPER", "PERIODICITE", " AND PERIODICITE.TCOD = PBPER"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PKEMT", "EMISSION", " AND EMISSION.TCOD = TBLPRIM.PKEMT"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PKSIT", "SITUATION", " AND SITUATION.TCOD = TBLPRIM.PKSIT"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PKREL", "RELANCE", " AND RELANCE.TCOD = TBLPRIM.PKRLC"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBTAC", "ACCORD", " AND ACCORD.TCOD = TBLPRIM.PKTAC"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PKMVT", "MOUVEMENT", " AND MOUVEMENT.TCOD = TBLPRIM.PKMVT"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "FBENC", "ENCAISSEMENT", " AND ENCAISSEMENT.TCOD = TBLPRIM.PKENC"),
                        (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "LEFT JOIN YPRIMES TBLPRIM ON PBIPB=TBLPRIM.PKIPB AND PBALX=TBLPRIM.PKALX AND TBLPRIM.PKIPK=" + numQuittanceVisu : string.Format("LEFT JOIN {0} TBLPRIM ON PBIPB=TBLPRIM.PKIPB AND PBALX=TBLPRIM.PKALX", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPES")),
                        (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "LEFT JOIN YPRIMCM ON PNIPB=TBLPRIM.PKIPB AND PNALX=PBALX AND PNIPK=" + numQuittanceVisu : string.Format("LEFT JOIN {0} ON PNIPB=TBLPRIM.PKIPB AND PNALX=PBALX", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPCM")),
                        CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                        modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);
            return DbBase.Settings.ExecuteList<QuittanceDetailDto>(CommandType.Text, sql).FirstOrDefault();
        }

        public static string GetCodeAvnQuittance(string codeOffre, string version, string numQuittanceVisu)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("numQuittance", 0);
            param[2].Value = !string.IsNullOrEmpty(numQuittanceVisu) ? Convert.ToInt32(numQuittanceVisu) : 0;

            string sqlAvn = @"SELECT PKAVN INT32RETURNCOL FROM YPRIMES WHERE PKIPB = :codeOffre AND PKALX = :version AND PKIPK = :numQuittance";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlAvn, param);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().Int32ReturnCol.ToString();
            }
            return string.Empty;
        }
        public static InfoCompQuittanceDto GetInfoComplementairesQuittance(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion, bool isReadonly, string user, bool isValidQuitt)
        {
            InfoCompQuittanceDto toReturn = new InfoCompQuittanceDto();
            if (string.IsNullOrEmpty(codeAvn))
                codeAvn = "0";
            string sqlEchNTraite = string.Format(@"SELECT COUNT(*) NBLIGN
                                                   FROM YPOECHE 
                                                   WHERE PIIPB = '{0}' AND PIALX = {1} AND PITYP = '{2}' AND PIIPK = 0", codeOffre.PadLeft(9, ' '), version, type);
            toReturn.IsEcheanceNonTraite = CommonRepository.RowNumber(sqlEchNTraite) > 0;

            string sqlPeriodicite = string.Format(@"SELECT PBPER STRRETURNCOL
                                                    FROM YPOBASE
                                                    WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'", codeOffre.PadLeft(9, ' '), version, type);
            var resultPeriode = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlPeriodicite);
            if (resultPeriode != null && resultPeriode.Any())
            {
                toReturn.Periodicite = resultPeriode.FirstOrDefault().StrReturnCol;
            }

            string sqlTypeCalcul = string.Format(@"SELECT PKPER STRRETURNCOL
                                                   FROM {3} 
                                                   WHERE PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {2}",
                                                codeOffre.PadLeft(9, ' '), version, codeAvn,
                                                CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPES"));
            var res = CommonRepository.GetStrValue(sqlTypeCalcul);
            toReturn.TypeCalcul = !string.IsNullOrEmpty(res) && res == "E" ? AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.TypesCalcul.Comptant) : AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.TypesCalcul.Total);

            string sqlCommentForce = string.Format(@"SELECT KAJOBSV STRRETURNCOL 
                                            FROM {3} 
                                                INNER JOIN {4} ON KAJCHR = KAAOBSC 
                                            WHERE KAAIPB = '{0}' AND KAAALX = {1} AND KAATYP = '{2}' {5}",
                                       codeOffre.PadLeft(9, ' '), version, type,
                                       CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                                       CommonRepository.GetPrefixeHisto(modeNavig, "KPOBSV"),
                                       modeNavig == ModeConsultation.Historique ? string.Format(" AND KAAAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);

            string sqlCommentForceRegul = string.Format(@"select kajobsv strreturncol
                                            FROM KPRGU 
                                                INNER JOIN KPOBSV ON KAJCHR = KHWOBSC 
                                            WHERE KHWIPB = '{0}' AND KHWALX = {1} AND KHWTYP = '{2}'",
                                        codeOffre, version, type);

            toReturn.CommentaireForce = CommonRepository.GetStrValue(acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL ? sqlCommentForceRegul : sqlCommentForce);

            string sqlExistEcheancier = string.Format(@" SELECT COUNT(*) NBLIGN
                                                         FROM {3}                                                                                                    
                                                         WHERE PIIPB='{0}' AND PIALX='{1}' AND PITYP='{2}' {4}",
                                                                 codeOffre, version, type,
                                                                 CommonRepository.GetPrefixeHisto(modeNavig, "YPOECHE"),
                                                                 modeNavig == ModeConsultation.Historique ? string.Format(" AND PIAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);
            toReturn.IsEcheancierExist = CommonRepository.RowNumber(sqlExistEcheancier) > 0;

            string sqlRepartition = string.Format(@"SELECT CAST(SUM(PIPCR) AS NUMERIC(13,2)) DECRETURNCOL, CAST(SUM(PIPCC) AS NUMERIC(13,2)) MONTANT
                                                    FROM YPOECHE
                                                    WHERE PITYP = '{0}' AND PIIPB = '{1}' AND PIALX = {2}", type, codeOffre.PadLeft(9, ' '), version);
            var result2 = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlRepartition);
            if (result2 != null && result2.Any())
            {
                toReturn.PourcentRepartition = result2.FirstOrDefault().DecReturnCol;
                toReturn.PourcentRepartitionCalc = result2.FirstOrDefault().Montant;
            }

            #region Traitement de la case à cocher "à émettre"
            toReturn.DisplayEmission = false;
            toReturn.AEmission = false;
            if (codeAvn != "0" && modeNavig == ModeConsultation.Standard && !isReadonly)
            {
                var dateNow = DateTime.Now;

                DbParameter[] param = new DbParameter[13];
                param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
                param[1] = new EacParameter("P_VERSION", 0);
                param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                param[2] = new EacParameter("P_TYPE", type);
                param[3] = new EacParameter("P_CONCEPT", "PRODU");
                param[4] = new EacParameter("P_FAMILLE", "LIMIT");
                param[5] = new EacParameter("P_CODE", "EMIPRIMERT");
                param[6] = new EacParameter("P_USER", user);
                param[7] = new EacParameter("P_DATENOW", 0);
                param[7].Value = AlbConvert.ConvertDateToInt(dateNow);
                param[8] = new EacParameter("P_HEURENOW", 0);
                param[8].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));
                param[9] = new EacParameter("P_VALIDQUITTANCE", isValidQuitt ? "O" : "N");
                param[10] = new EacParameter("P_ACTEGESTION", acteGestion);

                param[11] = new EacParameter("P_DISPLAYEMISSION", DbType.Int32);
                param[11].Direction = ParameterDirection.InputOutput;
                param[11].Value = 0;
                param[12] = new EacParameter("P_AEMISSION", DbType.Int32);
                param[12].Direction = ParameterDirection.InputOutput;
                param[12].Value = 0;

                var result = DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_GETINFOCOMPQUITTANCE", param);
                if (!string.IsNullOrEmpty(param[9].Value.ToString()) && param[11].Value.ToString() == "1")
                    toReturn.DisplayEmission = true;
                if (!string.IsNullOrEmpty(param[11].Value.ToString()) && param[12].Value.ToString() == "1")
                    toReturn.AEmission = true;
            }
            #endregion

            #region Modification des périodes

            if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_MODIF || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
            {
                DbParameter[] paramPer = new DbParameter[3];
                paramPer[0] = new EacParameter("codeAffaire", codeOffre.Trim().PadLeft(9, ' '));
                paramPer[1] = new EacParameter("version", 0);
                paramPer[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                paramPer[2] = new EacParameter("codeAvn", 0);
                paramPer[2].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;

                string sqlPer = @"SELECT CUR.JDPEA * 10000 + CUR.JDPEM * 100 + CUR.JDPEJ DATEDEBRETURNCOL,
	                                        HIST.JDPEA * 10000 + HIST.JDPEM * 100 + HIST.JDPEJ DATEFINRETURNCOL,
                                            PBAVA *10000 + PBAVM * 100 +PBAVJ INT64RETURNCOL,
                                            PBEFA *10000 + PBEFM * 100 + PBEFJ DATEDEBEFFRETURNCOL,
                                            PBFEA * 10000 + PBFEM * 100 + PBFEJ DATEFINEFFRETURNCOL
                                        FROM YPOBASE
                                        INNER JOIN YPRTENT CUR ON PBIPB = CUR.JDIPB AND PBALX = CUR.JDALX
                                        INNER JOIN YHRTENT HIST ON PBIPB = HIST.JDIPB AND PBALX = HIST.JDALX AND PBAVN - 1 = HIST.JDAVN
                                        WHERE PBIPB = :codeAffaire AND PBALX= :version AND PBAVN = :codeAvn";

                var resultPer = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlPer, paramPer);
                if (resultPer != null && resultPer.Any())
                {
                    toReturn.DateAvenant = AlbConvert.ConvertIntToDate(Convert.ToInt32(resultPer.FirstOrDefault().Int64ReturnCol));
                    toReturn.DateDebutEffetContrat = AlbConvert.ConvertIntToDate(Convert.ToInt32(resultPer.FirstOrDefault().DateDebEffReturnCol));
                    toReturn.DateFinEffetContrat = AlbConvert.ConvertIntToDate(Convert.ToInt32(resultPer.FirstOrDefault().DateFinEffReturnCol));
                    toReturn.ModifPeriode = resultPer.FirstOrDefault().DateDebReturnCol != resultPer.FirstOrDefault().DateFinReturnCol;
                }
            }

            if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_RESIL)
            {
                toReturn.ModifPeriode = true;
            }

            #endregion

            #region Modification periode-date fin

            if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_MODIF ||
                acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
            {
                var parameters = new DbParameter[2];
                parameters[0] = new EacParameter("codeAffaire", codeOffre.Trim().PadLeft(9, ' '));
                parameters[1] = new EacParameter("version", !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0);

                //TODO: soustraire une minute dans les contrôles dates
                var query = @"SELECT COUNT(*) NBLIGN
                              FROM  YPRTRSQ RISQUE
                                      INNER JOIN YPOBASE BASE  ON RISQUE.JEIPB = BASE.PBIPB AND RISQUE.JEALX = BASE.PBALX    
                                              WHERE RISQUE.JEIPB = :codeAffaire AND  RISQUE.JEALX = :version AND RISQUE.JETEM='O'
                                    AND (RISQUE.JEVFA * 10000 + RISQUE.JEVFM * 100 + RISQUE.JEVFJ) >= (BASE.PBAVA * 10000 +  BASE.PBAVM * 100 + BASE.PBAVJ )";

                toReturn.AuMoinsRisqueTempExiste = CommonRepository.ExistRowParam(query, parameters);
            }

            #endregion


            #region Trace RC

            DbParameter[] paramRC = new DbParameter[3];
            paramRC[0] = new EacParameter("codeAffaire", codeOffre.Trim().PadLeft(9, ' '));
            paramRC[1] = new EacParameter("version", 0);
            paramRC[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            paramRC[2] = new EacParameter("type", type);

            string sqlTraceRC = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC WHERE KHOIPB = :codeAffaire AND KHOALX = :version AND KHOTYP = :type AND KHOOEF = 'CC'";

            toReturn.TraceCC = CommonRepository.ExistRowParam(sqlTraceRC, paramRC);

            #endregion

            return toReturn;
        }
        public static void DeleteTraceAvt(string codeOffre, string version, string type, string acteGestion)
        {
            string sql = string.Format(@"DELETE FROM KPAVTRC WHERE KHOIPB = '{0}' AND KHOALX = {1} AND KHOTYP = '{2}' AND KHOPERI = 'COT' AND KHOOEF = '{3}'"
                , codeOffre.PadLeft(9, ' '), version, type, acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL ? "NR" : "NG");
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        public static void GestionTraceAvt(string codeOffre, string version, string type, bool isChecked, string user, string acteGestion)
        {
            DeleteTraceAvt(codeOffre, version, type, acteGestion);
            if (!isChecked)
            {
                var dateNow = DateTime.Now;

                DbParameter[] param = new DbParameter[9];
                param[0] = new EacParameter("newId", 0);
                param[0].Value = CommonRepository.GetAS400Id("KHOID");
                param[1] = new EacParameter("type", type);
                param[2] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
                param[3] = new EacParameter("version", 0);
                param[3].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                param[4] = new EacParameter("peri", "COT");
                param[5] = new EacParameter("oef", acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL ? "NR" : "NG");
                param[6] = new EacParameter("user", user);
                param[7] = new EacParameter("dateNow", 0);
                param[7].Value = AlbConvert.ConvertDateToInt(dateNow);
                param[8] = new EacParameter("heureNow", 0);
                param[8].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));

                string sql = @"INSERT INTO KPAVTRC
                           (KHOID, KHOTYP, KHOIPB, KHOALX, KHOPERI, KHOOEF, KHOCRU, KHOCRD, KHOCRH)
                                   VALUES
                                 (:newId, :type, :codeOffre, :version, :peri, :oef, :user, :dateNow, :heureNow)";

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
        }


        public static bool IsCheckedEcheance(string codeOffre, string version, string avenant)
        {
            string sqlDate = string.Empty;
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("codeContrat", codeOffre.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("codeAvn", 0);
            param[2].Value = !string.IsNullOrEmpty(avenant) ? Convert.ToInt32(avenant) : 0;
            sqlDate = @"SELECT PBFEA * 100000000 + PBFEM * 1000000 + PBFEJ * 10000 + PBFEH DATEFINRETURNCOL,
                         JDPEA  * 100000000 + JDPEM * 1000000 + JDPEJ * 10000  DATEDEBRETURNCOL
                         FROM YPOBASE 
                         left join YHRTENT ON PBIPB=JDIPB AND PBALX= JDALX AND jdavn = PBAVN - 1
                         WHERE PBIPB = :codeContrat AND PBALX = :version AND PBAVN = :codeAvn";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlDate, param);
            if (result != null && result.Any())
            {
                if (result.FirstOrDefault().DateFinReturnCol == 0 || result.FirstOrDefault().DateDebReturnCol == 0)
                    return false;
                else
                {
                    DateTime? date1 = AlbConvert.ConvertIntToDateHour(result.FirstOrDefault().DateDebReturnCol);
                    DateTime? date2 = AlbConvert.ConvertIntToDateHour(result.FirstOrDefault().DateFinReturnCol);
                    DateTime? dateEch = AlbConvert.GetFinPeriode(date1.Value, 0, AlbOpConstants.Jour);
                    Int64? anneeEffet = date2.Value.Year;
                    Int64? moisEch = dateEch.Value.Month;
                    Int64? jourEch = dateEch.Value.Day;
                    int? heureEch = AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(dateEch));
                    Int64? dateEchModif = anneeEffet * 100000000 + moisEch * 1000000 + jourEch * 10000 + heureEch;
                    return AlbConvert.ControlEqualDate(AlbConvert.ConvertIntToDateHour(dateEchModif), date2);
                }
            }
            return false;
        }
        #endregion
        #region Quittance - Ventilation détaillée

        public static List<QuittanceVentilationDetailleeGarantieDto> GetQuittanceVentilationDetailleeGaranties(string codeOffre, string version, string type, string modeAffichage, string numQuittanceVisu, string acteGestion)
        {
            string sql = string.Format(@"SELECT DISTINCT
                                               GARANTIE1.PLGAR   CODEGARANTIE, 
                                               GARANTIELBL.GADEA LIBELLEGARANTIE,
                                               GARANTIE1.PLKHT   HORSCATNAT,
                                               GARANTIE2.KVKNH   CATNAT,
                                               (GARANTIE1.PLKHX + GARANTIE2.KVKNT) MONTANTTAXES,
                                               (GARANTIE1.PLKHT + GARANTIE1.PLKHX + GARANTIE2.KVKNH + GARANTIE2.KVKNT) MONTANTTTC,
                                               OPTDV.KDCORDRE,OPTDB.KDCORDRE, KDETRI
                                        FROM {4} GARANTIE1
                                        INNER JOIN {2} GARANTIE2
                                               ON GARANTIE1.PLIPB = GARANTIE2.KVIPB
                                               AND GARANTIE1.PLALX = GARANTIE2.KVALX
                                               AND GARANTIE1.PLTYE = GARANTIE2.KVTYE
                                               AND GARANTIE1.PLGAR = GARANTIE2.KVGAR  
                                            {3}
                                       LEFT JOIN KGARAN GARANTIELBL
                                               ON GARANTIE1.PLGAR = GARANTIELBL.GAGAR                                         

                                       LEFT JOIN KPGARAN GAR
	                                       ON GARANTIE1.PLIPB = GAR.KDEIPB
	                                       AND GARANTIE1.PLALX = GAR.KDEALX
	                                       AND GARANTIE1.PLGAR = GAR.KDEGARAN  
                  
                                       INNER JOIN KPOPTD OPTDV
                                            ON OPTDV.KDCIPB = GAR.KDEIPB
                                            AND OPTDV.KDCTYP = GAR.KDETYP
                                            AND OPTDV.KDCALX = GAR.KDEALX
                                            AND OPTDV.KDCFOR = GAR.KDEFOR
                                            AND OPTDV.KDCOPT = GAR.KDEOPT
   		                                    AND OPTDV.KDCTENG = 'V'
                  
                                       INNER JOIN KPOPTD OPTDB
   	 	                                    ON OPTDB.KDCID = GAR.KDEKDCID
   	 	                                    AND OPTDB.KDCKAKID = OPTDV.KDCKAKID
   		                                    AND OPTDB.KDCTENG = 'B'

                                        WHERE GARANTIE1.PLTYE = '2'
                                               AND GARANTIE1.PLIPB = '{0}'
                                               AND GARANTIE1.PLALX = '{1}'

                                        ORDER BY OPTDV.KDCORDRE,OPTDB.KDCORDRE, KDETRI", codeOffre.PadLeft(9, ' '), version,
                                                                 (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "YPRIMGK" : CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPGK"),
                                                                 (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "AND Garantie2.KVIPK = Garantie1.PLIPK AND Garantie2.KVIPK = " + numQuittanceVisu : string.Empty,
                                                                 (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "YPRIMGA" : CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPGA"));

            var res = DbBase.Settings.ExecuteList<QuittanceVentilationDetailleeGarantieDto>(CommandType.Text, sql);

            res = res.GroupBy(t => t.LibelleGarantie).Select(grp => grp.First()).ToList();

            return res;
        }

        public static List<QuittanceVentilationDetailleeTaxeDto> GetQuittanceVentilationDetailleeTaxes(string codeOffre, string version, string type, string modeAffichage, string numQuittanceVisu, string acteGestion)
        {
            string sql = string.Format(@"SELECT Taxes.PMTXF CODETAXE, Labels.TPLIL LIBELLETAXE, Taxes.PMKHT BASETAXABLE, Taxes.PMKHX MONTANTTAXES
                                        FROM {3} Taxes
                                            {2}
                                        WHERE Taxes.PMTYE = '2' AND Taxes.PMIPB = '{0}' AND Taxes.PMALX = '{1}' {4}
                                        ORDER BY Taxes.PMTXF", codeOffre.PadLeft(9, ' '), version,
                                            CommonRepository.BuildJoinYYYYPAR("INNER", "GENER", "TAXFM", "Labels", " AND Taxes.PMTXF = Labels.TCOD"),
                                            (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "YPRIMTX" : CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTX"),
                                            (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "AND Taxes.PMIPK = " + numQuittanceVisu : string.Empty);

            return DbBase.Settings.ExecuteList<QuittanceVentilationDetailleeTaxeDto>(CommandType.Text, sql);
        }

        #endregion
        #region Quittance - Ventilation des commissions

        public static List<QuittanceVentilationCommissionCourtierDto> GetQuittanceVentilationCommissionCourtiers(string codeOffre, string version, string type, string modeAffichage, string numQuittanceVisu, string acteGestion)
        {
            string sql = string.Format(@"SELECT
                                               Courtiers.PNICT CODECOURTIER,
                                               (TRIM(Courtiers.PNICT) CONCAT ' - ' CONCAT TRIM(DetailsCourtier1.TNNOM) CONCAT ', ' CONCAT TRIM(DetailsCourtier2.TCCOM) CONCAT ' ' CONCAT TRIM(DetailsCourtier2.TCVIL)) LIBELLECOURTIER,
                                               Courtiers.PNCOM REPARTITION,
                                               Courtiers.PNKCO HORSCATNAT,
                                               Courtiers.PNKNM CATNAT,
                                               Courtiers.PNKTC TOTAL
                                        FROM {2} Courtiers
                                        INNER JOIN YCOURTN DetailsCourtier1
                                               ON Courtiers.PNICT = DetailsCourtier1.TNICT
                                               AND DetailsCourtier1.TNXN5 = 0
                                               AND DetailsCourtier1.TNTNM = 'A'
                                        INNER JOIN YCOURTI DetailsCourtier2
                                               ON DetailsCourtier1.TNICT = DetailsCourtier2.TCICT
                                        WHERE Courtiers.PNIPB = '{0}'
                                        AND Courtiers.PNALX = '{1}'
                                        {3}
                                        ORDER BY PNTYE", codeOffre.PadLeft(9, ' '), version,
                                                       (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "YPRIMCM" : CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPCM"),
                                                       (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "AND Courtiers.PNIPK = " + numQuittanceVisu : string.Empty);

            return DbBase.Settings.ExecuteList<QuittanceVentilationCommissionCourtierDto>(CommandType.Text, sql);
        }

        #endregion
        #region Quittance - Frais accessoires

        public static List<ParametreDto> GetListeTypesAccessoire()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "FBAFC");
        }

        public static FraisAccessoiresDto InitFraisAccessoire(string codeOffre, string versionOffre, string typeOffre, string codeAvn, int anneeEffet, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion, string reguleId, bool isModifHorsAvn)
        {
            var model = GetFraisAccessoires(codeOffre, versionOffre, codeAvn, modeNavig, acteGestion, reguleId, isModifHorsAvn);

            BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, versionOffre, typeOffre, codeAvn, modeNavig);
            model.TypesFrais = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "PRODU", "FBAFC");
            //model.FraisStandards = !isReadonly && modeNavig == ModeConsultation.Standard ?
            //    CommonRepository.GetFraisStandard(codeOffre, versionOffre, typeOffre, anneeEffet, codeAvn, user, acteGestion) :
            //    0;
            //SLA 24.04.2015 : vu avec FDU, aucune incidence à appeler ce P400 en readonly ou histo
            //model.FraisStandards = CommonRepository.GetFraisStandard(codeOffre, versionOffre, typeOffre, anneeEffet, codeAvn, user, acteGestion);
            //2017-02-14 : affectation des frais retenus au frais standards si typeFrais = "S" sinon appel du PGM400
            if (model.TypeFrais == "S" && model.FraisRetenus != 0)
                model.FraisStandards = model.FraisRetenus;
            else if (!isModifHorsAvn)
                model.FraisStandards = CommonRepository.GetFraisStandard(codeOffre, versionOffre, typeOffre, anneeEffet, codeAvn, user, acteGestion);

            //2017-02-09 : ajout propriété pour savoir si on peut modifier les frais stds
            model.ModifFraisStd = GetModifFraisStd(codeOffre, versionOffre, typeOffre, codeAvn);

            return model;
        }

        public static FraisAccessoiresDto GetFraisAccessoires(string codeOffre, string versionOffre, string codeAvn, ModeConsultation modeNavig, string acteGestion, string reguleId, bool isModifHorsAvn)
        {
            /* SAB bug 2040  si Codavn en cours different de Codavn passé en param et le modeNavig standard alors on passe mode histo */
            string sqlCodAvn = string.Format(@"SELECT PBAVN ID FROM YPOBASE WHERE PBIPB = '{0}' AND PBALX= {1}", codeOffre, versionOffre);
            var resultCodAvn = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlCodAvn);
            if (resultCodAvn != null && resultCodAvn.Any())
            {
                if ((resultCodAvn.FirstOrDefault().Id).ToString() != codeAvn && ModeConsultation.Standard == modeNavig)
                {
                    modeNavig = ModeConsultation.Historique;
                }
            }
            FraisAccessoiresDto toReturn = null;


            string sql = string.Format(@"SELECT {11} TYPEFRAIS, {12} FRAISRETENUS, {13} TAXEATTENTAT, KAAAFS APPLIQUEFRAISSPECIFIQUES,
                                        KAAAFS FRAISSPECIFIQUES,KAAOBSC CODECOMMENTAIRES,KAJOBSV COMMENTAIRES, PBEFA ANNEEEFFET, IFNULL(PKAFR, JDAFR) FRAIS,
                                        CAATT ATTCATEGO
                                        FROM {2}
                                        INNER JOIN {3} ON KAAIPB=JDIPB AND KAAALX=JDALX {6}
                                        LEFT JOIN {4} ON KAJCHR=KAAOBSC
                                        INNER JOIN {8} ON PBIPB = KAAIPB AND PBALX=KAAALX AND PBAVN={7}
                                        LEFT JOIN {9} ON PKIPB = PBIPB AND PKALX = PBALX AND PKAVN = {7}
                                        {10}
                                        LEFT JOIN YCATEGO ON CABRA = PBBRA AND CASBR = PBSBR AND CACAT = PBCAT
                                        WHERE JDIPB='{0}' AND JDALX='{1}' {5}", codeOffre.PadLeft(9, ' '), versionOffre,
                                    CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPOBSV"),
                                    modeNavig == ModeConsultation.Historique ? string.Format(" AND JDAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                    modeNavig == ModeConsultation.Historique ? " AND KAAAVN = JDAVN" : string.Empty,
                                   codeAvn,
                                    CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                                    CommonRepository.GetSuffixeRegule(acteGestion, isModifHorsAvn ? "YPRIMES" : "YPRIPES"),
                                    acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? string.Format("LEFT JOIN KPRGU ON KHWID = {0}", !string.IsNullOrEmpty(reguleId) ? reguleId : "0") : string.Empty,
                                    acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? "KHWAFC" : "IFNULL(NULLIF(JDAFC, ''), 'S')",
                                    acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? "KHWAFR" : "JDAFR",
                                    acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? "KHWATT" : "JDATT");
            var resultAN = DbBase.Settings.ExecuteList<FraisAccessoiresDto>(CommandType.Text, sql);
            if (resultAN != null && resultAN.Any())
            {
                toReturn = resultAN.FirstOrDefault();
                //Ajout (voir bug 1555) //changement correction bug 1880 (modeNavig)
                //suppression du controle avn suite 2040
                //!string.IsNullOrEmpty(codeAvn) && codeAvn != "0" &&
                if (modeNavig != ModeConsultation.Historique && acteGestion != "OFFRE")
                {

                    string sqlAvn = string.Format(@"SELECT PKATM TAXEATTENTATVALUE 
                                                FROM {3} 
                                                WHERE PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {2}", codeOffre.PadLeft(9, ' '), versionOffre, codeAvn,
                                                 isModifHorsAvn ? "YPRIMES" : CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPES"));
                    var resultAVN = DbBase.Settings.ExecuteList<FraisAccessoiresDto>(CommandType.Text, sqlAvn);
                    if (resultAVN != null && resultAVN.Any())
                    {
                        toReturn.AppliqueTaxeAttentat = resultAVN.FirstOrDefault().TaxeAttentatValue != 0 ? "O" : "N";
                    }
                }
            }
            return toReturn;
        }


        public static string UpdateFraisAccessoires(string codeOffre, string versionOffre, string typeOffre, int effetAnnee, string typeFrais, int fraisRetenus, bool taxeAttentat/*, int fraisSpecifiques*/, long codeCommentaires, string commentaires, string codeAvn, string user, string acteGestion, bool isModifHorsAvn)
        {
            //2017-02-14 Correction bug 2284 : ne plus récupérer les frais standards
            //if (typeOffre == AlbConstantesMetiers.TYPE_OFFRE && typeFrais == "S")
            //    fraisRetenus = CommonRepository.GetFraisStandard(codeOffre, versionOffre, typeOffre, effetAnnee, codeAvn, user, acteGestion);

            DbParameter[] param = new DbParameter[11];
            param[0] = new EacParameter("P_ID_OFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_TYPE_OFFRE", typeOffre);
            param[2] = new EacParameter("P_VERSION_OFFRE", versionOffre);
            param[3] = new EacParameter("P_TYPEFRAIS", typeFrais);
            param[4] = new EacParameter("P_FRAIS_RETENUS", 0);
            param[4].Value = fraisRetenus;
            param[5] = new EacParameter("P_TAXE_ATTENTAT", taxeAttentat == true ? "O" : "N");
            //param[6] = new EacParameter("P_FRAIS_SPECIFIQUES", 0);
            //param[6].Value = fraisSpecifiques;
            param[6] = new EacParameter("P_COMMENTAIRES", string.IsNullOrEmpty(commentaires) ? string.Empty : commentaires);
            param[7] = new EacParameter("P_CODE_COMMENTAIRES", 0);
            param[7].Value = codeCommentaires;
            param[8] = new EacParameter("P_ACTEGESTION", acteGestion);
            param[9] = new EacParameter("P_ISHORSAVN", Convert.ToInt32(isModifHorsAvn));
            param[10] = new EacParameter("P_ERREUR", "");
            param[10].Direction = ParameterDirection.InputOutput;
            param[10].Size = 256;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPFRACC", param);
            //if (!string.IsNullOrEmpty(codeAvn) && codeAvn != "0")
            //{


            if (!isModifHorsAvn)
                CommonRepository.CalculAvenant(codeOffre, versionOffre, codeAvn, user, acteGestion, taxeAttentat == true ? "O" : "N", fraisRetenus, true);
            //}
            return param[10].Value.ToString();
        }


        public static void UpdateFraisAccessoiresAvn(string codeOffre, string versionOffre, string typeOffre, decimal fraisforce, bool taxeAttentat, string codeAvn, string user, string acteGestion, string reguleId)
        {
            if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL)
            {
                string sqlAvn = string.Format(@"UPDATE KPRGU SET KHWAFR = {1}, KHWATT = '{2}' ,KHWAFC = '{3}'  WHERE KHWID = {0}", reguleId, fraisforce, taxeAttentat ? "O" : "N", "P");
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlAvn);
                CommonRepository.LoadTableRegule(codeOffre, versionOffre, typeOffre, reguleId);

                // OBO KDA320 : Dividing Regule Amount
                long rgId = 0;
                long.TryParse(reguleId, out rgId);
                RegularisationRepository.PerformDividingAmount_KPGRGU_KPRGUR(rgId);
                // END OBO KDA320
            }
            else
            {

                //Mettre a jour la table KPENT  KAAAFS = Frais forcé
                string sqlAvn = string.Format(@"UPDATE KPENT
                                                SET  KAAAFS = {3}
                                                WHERE KAAIPB = '{0}' AND KAAALX = {1} AND KAATYP = '{2}'", codeOffre.PadLeft(9, ' '), versionOffre, typeOffre, fraisforce);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlAvn);
                if (acteGestion == "AFFNOUV")
                {
                    CommonRepository.CalculAffaireNouvelle(codeOffre, versionOffre, codeAvn, user, acteGestion, taxeAttentat ? "O" : "N", fraisforce, true);
                }
                else
                {
                    CommonRepository.CalculAvenant(codeOffre, versionOffre, codeAvn, user, acteGestion, taxeAttentat == true ? "O" : "N", fraisforce, true);
                }
            }
        }

        public static void SaveCommentQuittance(string codeOffre, string version, string type, string codeAvn, string comment, string modifPeriod, string dateDebStr, string dateFinStr)
        {
            string sql = string.Format(@"SELECT KAAOBSC ID FROM KPENT WHERE KAAIPB = '{0}' AND KAAALX = {1} AND KAATYP = '{2}'", codeOffre.PadLeft(9, ' '), version, type);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                if (!string.IsNullOrEmpty(comment))
                    comment = comment.Replace("'", "''");
                else
                    comment = string.Empty;
                if (result.FirstOrDefault().Id != 0)
                {
                    string sqlUpdate = string.Format(@"UPDATE KPOBSV SET KAJOBSV = '{0}' WHERE KAJCHR = {1}", comment, result.FirstOrDefault().Id);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdate);
                }
                else
                {
                    var newId = CommonRepository.GetAS400Id("KAJCHR");
                    string sqlInsert = string.Format(@"INSERT INTO KPOBSV (KAJCHR, KAJIPB, KAJALX, KAJTYP, KAJOBSV) VALUES ({0}, '{1}', {2}, '{3}', '{4}')",
                                    newId, codeOffre.PadLeft(9, ' '), version, type, comment);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlInsert);

                    string sqlUpdate = string.Format(@"UPDATE KPENT SET KAAOBSC = {0} WHERE KAAIPB = '{1}' AND KAAALX = {2} AND KAATYP = '{3}'", newId, codeOffre.PadLeft(9, ' '), version, type);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdate);
                }
            }

            if (modifPeriod == "1")
            {
                DateTime? dateDeb = AlbConvert.ConvertStrToDate(dateDebStr);
                DateTime? dateFin = AlbConvert.ConvertStrToDate(dateFinStr);

                DbParameter[] paramPeriod = new DbParameter[12];
                paramPeriod[0] = new EacParameter("debJourPer", 0);
                paramPeriod[0].Value = dateDeb.HasValue ? dateDeb.Value.Day : 0;
                paramPeriod[1] = new EacParameter("debMoisPer", 0);
                paramPeriod[1].Value = dateDeb.HasValue ? dateDeb.Value.Month : 0;
                paramPeriod[2] = new EacParameter("debAnneePer", 0);
                paramPeriod[2].Value = dateDeb.HasValue ? dateDeb.Value.Year : 0;
                paramPeriod[3] = new EacParameter("finJourPer", 0);
                paramPeriod[3].Value = dateFin.HasValue ? dateFin.Value.Day : 0;
                paramPeriod[4] = new EacParameter("finMoisPer", 0);
                paramPeriod[4].Value = dateFin.HasValue ? dateFin.Value.Month : 0;
                paramPeriod[5] = new EacParameter("finAnneePer", 0);
                paramPeriod[5].Value = dateFin.HasValue ? dateFin.Value.Year : 0;
                paramPeriod[6] = new EacParameter("echJour", 0);
                paramPeriod[6].Value = dateDeb.HasValue ? dateDeb.Value.Day : 0;
                paramPeriod[7] = new EacParameter("echMois", 0);
                paramPeriod[7].Value = dateDeb.HasValue ? dateDeb.Value.Month : 0;
                paramPeriod[8] = new EacParameter("echAnnee", 0);
                paramPeriod[8].Value = dateDeb.HasValue ? dateDeb.Value.Year : 0;

                paramPeriod[9] = new EacParameter("codeAffaire", codeOffre.Trim().PadLeft(9, ' '));
                paramPeriod[10] = new EacParameter("version", 0);
                paramPeriod[10].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                paramPeriod[11] = new EacParameter("codeAvn", 0);
                paramPeriod[11].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;

                string sqlPeriode = @"UPDATE YPRIPES SET PKDPJ = :debJourPer, PKDPM = :debMoisPer, PKDPA = :debAnneePer,
                                            PKPFJ = :finJourPer, PKFPM = :finMoisPer, PKFPA = :finAnneePer, 
                                            PKEHJ = :echJour, PKEHM = :echMois, PKEHA = :echAnnee
                                        WHERE PKIPB = :codeAffaire AND PKALX = :version AND PKAVN = :codeAvn WITH NC";

                DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlPeriode, paramPeriod);
            }
        }
        public static void SaveCommentQuittanceRegule(string codeContrat, string version, string type, string comment, string reguleId)
        {
            string sql = string.Format(@"SELECT KHWOBSC ID FROM KPRGU WHERE KHWID = {0}", reguleId);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                if (!string.IsNullOrEmpty(comment))
                    comment = comment.Replace("'", "''");
                else
                    comment = string.Empty;
                if (result.FirstOrDefault().Id != 0)
                {
                    string sqlUpdate = string.Format(@"UPDATE KPOBSV SET KAJOBSV = '{0}' WHERE KAJCHR = {1}", comment, result.FirstOrDefault().Id);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdate);
                }
                else
                {
                    var newId = CommonRepository.GetAS400Id("KAJCHR");
                    string sqlInsert = string.Format(@"INSERT INTO KPOBSV (KAJCHR, KAJIPB, KAJALX, KAJTYP, KAJOBSV) VALUES ({0}, '{1}', {2}, '{3}', '{4}')",
                                    newId, codeContrat.PadLeft(9, ' '), version, type, comment);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlInsert);

                    string sqlUpdate = string.Format(@"UPDATE KPRGU SET KHWOBSC = {0} WHERE KHWID = {1}", newId, reguleId);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdate);
                }
            }
        }


        public static string GetCommentQuittance(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            string sql = string.Format(@"SELECT KAJOBSV STRRETURNCOL 
                                            FROM {3} 
                                                INNER JOIN {4} ON KAJCHR = KAAOBSC 
                                            WHERE KAAIPB = '{0}' AND KAAALX = {1} AND KAATYP = '{2}' {5}",
                                        codeOffre.PadLeft(9, ' '), version, type,
                                        CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                                        CommonRepository.GetPrefixeHisto(modeNavig, "KPOBSV"),
                                        modeNavig == ModeConsultation.Historique ? string.Format(" AND KAAAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().StrReturnCol;
            }
            return string.Empty;
        }

        /// <summary>
        /// Contrôle si les frais standards peuvent être modifiés
        /// </summary>
        private static string GetModifFraisStd(string codeOffre, string versionOffre, string typeOffre, string codeAvn)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("codeAffaire", codeOffre.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = !string.IsNullOrEmpty(versionOffre) ? Convert.ToInt32(versionOffre) : 0;
            param[2] = new EacParameter("type", typeOffre);
            param[3] = new EacParameter("codeAvn", 0);
            param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;

            string sql = $@"SELECT CAPE1 CONCAT '_' CONCAT CAPE2 CONCAT '_' CONCAT CAPE3 STRRETURNCOL
                            FROM YPOBASE 
	                            INNER JOIN YCATEGP ON PBBRA = CAPBRA AND PBSBR = CAPSBR AND PBCAT = CAPCAT AND CAPAA = {DateTime.Now.Year}
                            WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvn";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().StrReturnCol;
            }

            return string.Empty;
        }

        #endregion
        #region Quittance - Tab Part Albingia

        public static QuittanceVentilationAperitionDto GetQuittancePartAlbingia(string codeOffre, string version, string type, string codeAvn, string modeNavig, string acteGestion, string modeAffichage, string numQuittanceVisu)
        {
            QuittanceVentilationAperitionDto toReturn = new QuittanceVentilationAperitionDto();
            toReturn.ListeLignesGaranties = new List<QuittanceTabAperitionLigneDto>();
            int numValue = 0;
            DbParameter[] param = new DbParameter[6];

            if (ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>())
            {
                param[0] = new EacParameter("P_CODE", codeOffre.PadLeft(9, ' '));
                param[1] = new EacParameter("P_VERSION", 0);
                param[1].Value = version;
                param[2] = new EacParameter("P_TYPE", type);
                param[3] = new EacParameter("P_CODEAVN", 0);
                param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
                param[4] = new EacParameter("P_MODEAFF", modeAffichage);
                param[5] = new EacParameter("P_NUMVISU", 0);
                param[5].Value = Int32.TryParse(numQuittanceVisu, out numValue) ? numValue : numValue;
            }
            else
            {
                param[0] = new EacParameter("P_CODE", codeOffre.PadLeft(9, ' '));
                param[1] = new EacParameter("P_VERSION", 0);
                param[1].Value = version;
                param[2] = new EacParameter("P_TYPE", type);
                param[3] = new EacParameter("P_MODEAFF", modeAffichage);
                param[4] = new EacParameter("P_NUMVISU", 0);
                param[4].Value = Int32.TryParse(numQuittanceVisu, out numValue) ? numValue : numValue;
                param[5] = new EacParameter("P_ACTEGESTION", acteGestion);
            }

            var result = DbBase.Settings.ExecuteList<QuittanceVentilationAperitionPlatDto>(CommandType.StoredProcedure, modeNavig == ModeConsultation.Historique.AsCode() ? "SP_GETCOTITABPARTALB_AVT" : "SP_GETCOTITABPARTALB", param);

            if (result.Any())
            {
                var dataPartAlb = result.Find(elm => elm.TypeValeur == "PartAlb");
                var dataGarantie = result.FindAll(elm => elm.TypeValeur == "Garantie");
                var dataComm = result.Find(elm => elm.TypeValeur == "Commission");
                var dataPartCoass = result.Find(elm => elm.TypeValeur == "PartCoassurance");

                if (dataPartAlb != null)
                {
                    toReturn.PartAlbingia = dataPartAlb.PartAlbingia;
                }
                if (dataGarantie != null && dataGarantie.Any())
                {
                    dataGarantie.ForEach(elm => toReturn.ListeLignesGaranties.Add(
                        new QuittanceTabAperitionLigneDto
                        {
                            Code = elm.CodeGarantie,
                            Libelle = elm.LibelleGarantie,
                            MainHCatnat = elm.GarantieHCatnat,
                            MainCatnat = elm.GarantieCatnat,
                            MainTotal = elm.GarantieCatnat + elm.GarantieHCatnat
                        }
                        ));
                }
                if (dataComm != null)
                {
                    toReturn.CommissionTauxHCatnat = dataComm.CommissionTauxHCatnat;
                    toReturn.CommissionTauxCatnat = dataComm.CommissionTauxCatnat;
                    toReturn.CommissionValHCatnat = dataComm.CommissionValHCatnat;
                    toReturn.CommissionValCatnat = dataComm.CommissionValCatnat;
                    toReturn.CommissionTotal = dataComm.CommissionValHCatnat + dataComm.CommissionValCatnat;
                }
                if (dataPartCoass != null)
                {
                    toReturn.FraisAperition = dataPartCoass.FraisAperition;
                    toReturn.CoassuranceHTHCatnat = dataPartCoass.CoassuranceHTHCatnat;
                    toReturn.CoassuranceHTCatnat = dataPartCoass.CoassuranceHTCatnat;
                    toReturn.CoassuranceHTTotal = dataPartCoass.CoassuranceHTHCatnat + dataPartCoass.CoassuranceHTCatnat;
                    toReturn.CoassuranceCommHCatnat = dataPartCoass.CoassuranceCommHCatnat;
                    toReturn.CoassuranceCommCatnat = dataPartCoass.CoassuranceCommCatnat;
                    toReturn.CoassuranceCommTotal = dataPartCoass.CoassuranceCommHCatnat + dataPartCoass.CoassuranceCommCatnat;
                }
            }

            return toReturn;
        }

        public static List<QuittanceTabAperitionLigneDto> GetQuittanceVentilationCoassureurs(string codeOffre, string version, string type, string modeAffichage, string numQuittanceVisu, string acteGestion)
        {
            string sql = string.Format(@"SELECT       POCIE CODE,
                                                        CINOM LIBELLE,
                                                         POAPP PARTCOASS,
                                                         POKAP FRAISAPERITION,
                                                         POKHT - POKNH HTHCATNAT,
                                                         POKNH HTCATNAT,
                                                         POKHT HTTOTAL,
                                                         POKCO COMMHCATNAT,
                                                          POKNM COMMCATNAT,
                                                         POKCO + POKNM COMMTOTAL
                                            FROM {2} 
                                            INNER JOIN YCOMPA ON CIICI = POCIE
                                            WHERE POIPB = '{0}' AND POALX = {1} AND POTYE = '2' {3}", codeOffre.PadLeft(9, ' '), version,
                                                                                                            (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "YPRIMPA" : CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPPA"),
                                                                                                            (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? " AND POIPK= " + numQuittanceVisu : string.Empty);
            return DbBase.Settings.ExecuteList<QuittanceTabAperitionLigneDto>(CommandType.Text, sql);
        }

        public static List<QuittanceTabAperitionLigneDto> GetQuittanceVentilationCoassureursParGarantie(string codeOffre, string version, string type, string codeCoass, string modeAffichage, string numQuittanceVisu, string acteGestion)
        {
            string sql = string.Format(@"SELECT KYGAR  CODE,
                                                GADES LIBELLE,
                                                KYKHT - KYKNH HTHCATNAT,
                                                KYKNH HTCATNAT,
                                                KYKHT HTTOTAL,
                                                KYKCO COMMHCATNAT,
                                                KYKNM COMMCATNAT,
                                                KYKCO + KYKNM COMMTOTAL
                                         FROM {3} 
                                         INNER JOIN YGARANT ON GAGAR = KYGAR
                                         WHERE KYIPB = '{0}' AND KYALX = {1} AND KYTYE = '2' 
                                                AND TRIM(KYCIE) = TRIM('{2}') {4}", codeOffre.PadLeft(9, ' '), version, codeCoass,
                                                                              (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? "YPRIMPK" : CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPPK"),
                                                                              (modeAffichage == "Visu" && !string.IsNullOrEmpty(numQuittanceVisu)) ? " AND KYIPK = " + numQuittanceVisu : string.Empty);

            return DbBase.Settings.ExecuteList<QuittanceTabAperitionLigneDto>(CommandType.Text, sql);
        }

        #endregion
        #region Quittance - Visualisation

        public static List<QuittanceVisualisationLigneDto> GetListeVisualisationQuittances(string codeOffre, string version, DateTime? dateEmission, string typeOperation, string situation, DateTime? datePeriodeDebut, DateTime? datePeriodeFin, AlbConstantesMetiers.TypeQuittances typeQuittances, string colTri, string numAvn = "")
        {
            string sql = string.Format(@"SELECT  PKEMA EMISSIONANNEE, PKEMM EMISSIONMOIS, PKEMJ EMISSIONJOUR,
                                                 PKIPK NUMQUITTANCE, PKAVC CONCAT PKAVI AVIS,
                                                 PKDPA DATEDEBANNEE, PKDPM DATEDEBMOIS, PKDPJ DATEDEBJOURS,
                                                 PKFPA DATEFINANNEE, PKFPM DATEFINMOIS, PKPFJ DATEFINJOURS,  
                                                 PKEHA ECHEANCEANNEE, PKEHM ECHEANCEMOIS, PKEHJ ECHEANCEJOUR,
                                                 PKAVN AVN, PKDEM DEMCODE, PKMVT MVTCODE, MVT.TPLIL MVTLIBELLE,
                                                 PKOPE OPECODE, OPE.TPLIL OPELIBELLE, PKSIT SITCODE,
                                                 SIT.TPLIL SITLIBELLE, PKKHT HT, PKKTT TTC, PKKTR REGLE,
                                                 CASE WHEN WWIPK IS NULL THEN 'N' ELSE 'O' END ISANNULE,
                                                 CASE WHEN WWIPK IS NULL THEN '' ELSE 'C' END ISCHECK
                                         FROM YPRIMES
                                                 LEFT JOIN YYYYPAR MVT ON MVT.TCON = 'PRODU' AND MVT.TFAM = 'PKMVT' AND MVT.TCOD = PKMVT
                                                 LEFT JOIN YYYYPAR OPE ON OPE.TCON = 'PRODU' AND OPE.TFAM = 'PKOPE' AND OPE.TCOD = PKOPE
                                                 LEFT JOIN YYYYPAR SIT ON SIT.TCON = 'PRODU' AND SIT.TFAM = 'PKSIT' AND SIT.TCOD = PKSIT
                                                 LEFT JOIN YWRTANP ON WWIPB = PKIPB AND WWALX = PKALX AND WWIPK = PKIPK
                                         WHERE (PKIPB = '{0}' AND PKALX = {1}", codeOffre, version);

            if (dateEmission.HasValue && dateEmission.Value != default(DateTime))
            {
                //if (!string.IsNullOrEmpty(numAvn))
                //    sql += string.Format(@" AND ((PKEMA * 10000 + PKEMM * 100 + PKEMJ) >= {0} OR PKAVN = {1})", dateEmission.Value.Year * 10000 + dateEmission.Value.Month * 100 + dateEmission.Value.Day, numAvn);
                //else
                sql += string.Format(@" AND (PKEMA * 10000 + PKEMM * 100 + PKEMJ) >= {0}", dateEmission.Value.Year * 10000 + dateEmission.Value.Month * 100 + dateEmission.Value.Day);
            }
            if (!string.IsNullOrEmpty(typeOperation))
            {
                sql += string.Format(@" AND PKOPE = '{0}'", typeOperation);
            }
            if (!string.IsNullOrEmpty(situation))
            {
                switch (situation)
                {
                    case "NonSoldees":
                        sql += @" AND PKSIT = 'A'";
                        break;
                    case "NonAnnuleeNonAnnulation":
                        sql += @" AND PKSIT <> 'X' AND PKSIT <> 'W'";
                        break;
                    case "SoldeesOuAcompte":
                        sql += @" AND (PKSIT = 'S' OR PKKTR <> 0) ";
                        break;
                    case "Toutes":
                    default:
                        break;
                }

            }
            if (datePeriodeDebut.HasValue && datePeriodeDebut.Value != default(DateTime))
            {
                sql += string.Format(@" AND (PKDPA * 10000 + PKDPM * 100 + PKDPJ) >= {0}", datePeriodeDebut.Value.Year * 10000 + datePeriodeDebut.Value.Month * 100 + datePeriodeDebut.Value.Day);
            }
            if (datePeriodeFin.HasValue && datePeriodeFin.Value != default(DateTime))
            {
                sql += string.Format(@" AND (PKDPA * 10000 + PKDPM * 100 + PKDPJ) <= {0}", datePeriodeFin.Value.Year * 10000 + datePeriodeFin.Value.Month * 100 + datePeriodeFin.Value.Day);
            }
            if (typeQuittances != AlbConstantesMetiers.TypeQuittances.Toutes)
            {
                switch (typeQuittances)
                {
                    case AlbConstantesMetiers.TypeQuittances.IMPAYES:
                        sql += @" AND PKSIT = 'A'";
                        break;
                }
            }
            //sql += " ORDER BY PKIPK DESC";
            if (!string.IsNullOrEmpty(numAvn))
                sql += string.Format(@") OR (PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {2}", codeOffre.PadLeft(9, ' '), version, numAvn);
            var orderBy = ") ORDER BY ";
            switch (colTri)
            {
                case "QuittNum":
                    orderBy += "PKIPK";
                    break;
                case "QuittNumDESC":
                    orderBy += "PKIPK DESC";
                    break;
                case "QuittNumInt":
                    orderBy += "PKAVN, (PKDPA * 10000 + PKDPM * 100 + PKDPJ) DESC";
                    break;
                case "QuittNumIntDESC":
                    orderBy += "PKAVN DESC, (PKDPA * 10000 + PKDPM * 100 + PKDPJ) DESC";
                    break;         
                case "DateEch":
                    orderBy += "(PKEHA * 10000 + PKEHM * 100 + PKEHJ)";
                    break;
                case "DateEchDESC":
                    orderBy += "(PKEHA * 10000 + PKEHM * 100 + PKEHJ) DESC";
                    break;
                default:
                    orderBy += "PKIPK DESC";
                    break;
            }
            //if (!string.IsNullOrEmpty(colTri))
            //

            //    orderBy += "PKIPK";
            //}

            sql += orderBy;
            //sql += " ORDER BY (PKDPA * 10000 + PKDPM * 100 + PKDPJ) DESC";
            return DbBase.Settings.ExecuteList<QuittanceVisualisationLigneDto>(CommandType.Text, sql);
        }

        #endregion
        #region Quittance - Annulation quittances

        public static void EnregistrerQuittancesAnnulees(string codeOffre, string version, string listeQuittancesAnnulees)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = version;
            param[2] = new EacParameter("P_LISTEQUITT", listeQuittancesAnnulees);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEQUITTANCESANNULEES", param);
        }

        #endregion
        #region Quittance - Calcul Forcé

        #region Méthodes publiques

        public static QuittanceForceDto LoadCalculWindow(string codeOffre, string version, string avenant, string typeVal, string typeHTTTC, ModeConsultation modeNavig, string acteGestion)
        {
            QuittanceForceDto toReturn = new QuittanceForceDto();
            if (typeVal == "formule")
                toReturn.ForceFormule = GetInfoFormule(codeOffre, version, avenant, typeHTTTC, modeNavig, acteGestion);
            if (typeVal == "montant")
                toReturn.ForceTotal = GetInfoTotal(codeOffre, version, avenant, acteGestion);
            return toReturn;
        }

        public static string ExistMntCalcul(string codeOffre, string version, string avenant, ModeConsultation modeNavig, string acteGestion)
        {
            string mntStr = string.Empty;
            string sql = string.Format("SELECT PKMHT DecReturnCol  FROM {3} WHERE PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {2}",
                                            codeOffre.PadLeft(9, ' '), version, avenant,
                                            CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPES"));
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                if (result.FirstOrDefault().DecReturnCol != 0)
                {
                    return mntStr = "O";
                }
                else
                {
                    return mntStr = "N";
                }
            }

            return string.Empty;
        }


        public static string UpdateCalculForce(string codeOffre, string type, string version, string avenant, string typeVal, string typeHTTTC,
            string codeRsq, string codeFor, string montantForce, string user, string acteGestion, string reguleId)
        {
            if (typeVal == "montant")
            {
                switch (typeHTTTC)
                {
                    case "HT":
                        return UpdateCalculMontantHTForce(codeOffre, type, version, avenant, codeRsq, codeFor, montantForce, user, acteGestion, reguleId);
                    case "TTC":
                        return UpdateCalculMontantTTCForce(codeOffre, type, version, avenant, codeRsq, codeFor, montantForce, user, acteGestion, reguleId);
                }
            }
            if (typeVal == "formule")
            {
                string valeurReturn = string.Empty;
                /* Recuperer KDBCMC de KPOPT*/
                string sql = string.Format(@" SELECT KDBCMC MONTANT
                                                    FROM {0}
                                                WHERE KDBIPB = '{1}' AND KDBALX = {2} AND KDBTYP = '{3}' AND KDBFOR = {4}", "KPOPT", codeOffre.PadLeft(9, ' '), version, type, codeFor);
                var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
                if (result != null && result.Any())
                {
                    valeurReturn = result.FirstOrDefault().Montant.ToString();
                }

                switch (typeHTTTC)
                {
                    case "HT":
                        return UpdateCalculFormuleHTForce(codeOffre, type, version, avenant, codeRsq, codeFor, montantForce, user, acteGestion, reguleId, valeurReturn);
                    case "TTC":
                        return UpdateCalculFormuleTTCForce(codeOffre, type, version, avenant, codeRsq, codeFor, montantForce, user, acteGestion, reguleId, valeurReturn);
                }
            }
            return "Erreur de paramètres";
        }

        public static QuittanceForceGarantieDto LoadGaranInfo(string codeOffre, string version, string avenant, string codeFor, ModeConsultation modeNavig, string acteGestion)
        {
            QuittanceForceGarantieDto toReturn = new QuittanceForceGarantieDto();
            string sql = string.Format(@"SELECT KDAFOR CODEFOR, KDAALPHA FORMLETTRE, KDADESC FORMDESC, KDDRSQ CODERSQ, KABDESC RSQDESC, 
                                                     PUGAR CODEGAR, GADES LIBGAR, PUCNA CATNAT, PUMHT MONTANT, PUTAX CODETAXE, TPLIL LIBTAXE
                                               FROM {4}
                                                     INNER JOIN {5} ON KDBIPB = KDAIPB AND KDBALX = KDAALX AND KDBTYP = KDATYP AND KDBFOR = KDAFOR {9}
                                                     INNER JOIN {6} ON KDDKDBID = KDBID
                                                     INNER JOIN {7} ON KABIPB = KDDIPB AND KABALX = KDDALX AND KABRSQ = KDDRSQ {10}
                                                     INNER JOIN {11} ON PTIPB = KDAIPB AND PTALX = KDAALX AND PTFOR = KDAFOR
                                                     INNER JOIN {12} ON PUIPB = PTIPB AND PUALX = PTALX AND PURSQ = PTRSQ AND PUFOR = PTFOR AND PUTYE = '2'
                                                     INNER JOIN KGARAN ON GAGAR = PUGAR
                                                     {3}
                                               WHERE KDAIPB = '{0}' AND KDAALX = {1} AND KDAID = {2} {8}",
                                codeOffre.PadLeft(9, ' '), version, codeFor,
                                CommonRepository.BuildJoinYYYYPAR("INNER", "GENER", "TAXEC", "TAXE", " AND TAXE.TCOD = PUTAX"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPOPT"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTAP"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                                modeNavig == ModeConsultation.Historique ? string.Format(" AND KDAAVN = {0}", !string.IsNullOrEmpty(avenant) ? Convert.ToInt32(avenant) : 0) : string.Empty,
                                modeNavig == ModeConsultation.Historique ? " AND KDBAVN = KDAAVN" : string.Empty,
                                modeNavig == ModeConsultation.Historique ? " AND KABAVN = KDDAVN" : string.Empty,
                                CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTA"),
                                CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTG"));

            var result = DbBase.Settings.ExecuteList<QuittanceForceGarantiePlatDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                var firstRecord = result.FirstOrDefault();
                toReturn.Formule = new QuittanceForceInfoFormuleDto
                {
                    CodeFor = firstRecord.CodeFor,
                    FormLettre = firstRecord.FormLettre,
                    FormDesc = firstRecord.FormDesc,
                    CodeRsq = firstRecord.CodeRsq,
                    RsqDesc = firstRecord.RsqDesc
                };
                toReturn.ListGaranties = new List<QuittanceForceInfoGarantieDto>();
                result.ForEach(m =>
                {
                    QuittanceForceInfoGarantieDto garan = new QuittanceForceInfoGarantieDto
                    {
                        CodeGarantie = m.CodeGarantie,
                        LibGarantie = m.LibGarantie,
                        CatNat = m.CatNat == "O",
                        MontantCal = m.MontantCal,
                        CodeTaxe = m.CodeTaxe,
                        LibTaxe = m.LibTaxe
                    };
                    toReturn.ListGaranties.Add(garan);
                });
            }

            toReturn.CodesTaxe = CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "TAXEC");

            return toReturn;
        }

        public static QuittanceForceGarantieDto UpdateGaranForce(string codeOffre, string version, string avenant, string formId, string codeFor, string codeRsq, string codeGaran, string montantForce, string catnatForce, string codeTaxeForce, ModeConsultation modeNavig, string acteGestion)
        {
            string sql = string.Format(@"UPDATE {8} 
                                               SET PUMHT = {5}, PUCNA = '{6}', PUTAX = '{7}'
                                        WHERE PUIPB = '{0}' AND PUALX = {1} AND PURSQ = {2} AND PUFOR = {3} AND TRIM(PUGAR) = TRIM('{4}')",
                                codeOffre, version, codeRsq, codeFor, codeGaran,
                                !string.IsNullOrEmpty(montantForce) ? montantForce : "0", catnatForce, codeTaxeForce,
                                CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTG"));
            DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sql);

            return LoadGaranInfo(codeOffre, version, avenant, formId, modeNavig, acteGestion);
        }

        public static string ValidFormGaranForce(string codeOffre, string type, string version, string avenant, string codeFor, string codeRsq, string user, string acteGestion)
        {
            return CommonRepository.UpdateCalculForce(codeOffre, type, version, avenant, codeRsq, codeFor, "0", "0", "0", "O", "T", "H", "F", "O", user, acteGestion);
        }

        #endregion

        #region Méthodes privées

        private static QuittanceForceTotalDto GetInfoTotal(string codeOffre, string version, string avenant, string acteGestion)
        {
            string sql = string.Format("SELECT PKKHT MONTANTHT, PKMTT MONTANTTTC  FROM {3} WHERE PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {2}",
                                            codeOffre.PadLeft(9, ' '), version, avenant,
                                            CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPES"));
            var result = DbBase.Settings.ExecuteList<QuittanceForceTotalDto>(CommandType.Text, sql);
            if (result != null && result.Any())
                return result.FirstOrDefault();

            return new QuittanceForceTotalDto();
        }




        private static string UpdateCalculMontantHTForce(string codeOffre, string type, string version, string avenant, string codeRsq, string codeFor, string montantForce, string user, string acteGestion, string reguleId)
        {
            double montantCal = 0;
            string sql = string.Format(@"SELECT PKKHT MONTANTHT FROM {3} WHERE PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {2}",
                                codeOffre.PadLeft(9, ' '), version, avenant,
                                CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPES"));
            var result = DbBase.Settings.ExecuteList<QuittanceForceTotalDto>(CommandType.Text, sql);
            if (result != null && result.Any())
                montantCal = result.FirstOrDefault().MontantCalculeHT;

            double coeff = 0;
            if (montantCal != 0)
                coeff = Convert.ToDouble(montantForce) / montantCal;

            if (Math.Abs(coeff) > 9999)
            {
                coeff = 1;
            }

            switch (acteGestion)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    return CommonRepository.UpdateCalculForceRegule(codeOffre, type, version, avenant, codeRsq, codeFor, montantCal.ToString("F9"), montantForce, coeff.ToString("F9"), "P", "H", "C", user, acteGestion, reguleId);
                default:
                    return CommonRepository.UpdateCalculForce(codeOffre, type, version, avenant, codeRsq, codeFor, montantCal.ToString("F9"), montantForce, coeff.ToString("F9"), "O", "P", "H", "C", "O", user, acteGestion);
            }
        }

        private static string UpdateCalculMontantTTCForce(string codeOffre, string type, string version, string avenant, string codeRsq, string codeFor, string montantForce, string user, string acteGestion, string reguleId)
        {
            double montantTTC = 0;
            double fraisAcc = 0;
            double taxeAcc = 0;
            double taxeAtt = 0;

            string sql = string.Format("SELECT PKMTT MONTANTTTC, PKAFR FRAISACC, PKAFT TAXEACC, PKATM TAXEATT FROM {3} WHERE PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {2}",
                                codeOffre.PadLeft(9, ' '), version, avenant,
                                CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPES"));
            var result = DbBase.Settings.ExecuteList<QuittanceForceTotalDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                montantTTC = result.FirstOrDefault().MontantCalculeTTC;
                fraisAcc = result.FirstOrDefault().FraisAccessoires;
                taxeAcc = result.FirstOrDefault().TaxeAccessoires;
                taxeAtt = result.FirstOrDefault().TaxeAttentat;
            }
            montantForce = (Convert.ToDouble(montantForce) - (fraisAcc + taxeAcc + taxeAtt)).ToString("F9");
            montantTTC = montantTTC - (fraisAcc + taxeAcc + taxeAtt);

            double coeff = 0;
            if (montantTTC != 0)
                coeff = Convert.ToDouble(montantForce) / montantTTC;

            if (Math.Abs(coeff) > 9999)
            {
                coeff = 1;
            }

            switch (acteGestion)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    return CommonRepository.UpdateCalculForceRegule(codeOffre, type, version, avenant, codeRsq, codeFor, montantTTC.ToString("F9"), montantForce, coeff.ToString("F9"), "P", "T", "C", user, acteGestion, reguleId);
                default:
                    return CommonRepository.UpdateCalculForce(codeOffre, type, version, avenant, codeRsq, codeFor, montantTTC.ToString("F9"), montantForce, coeff.ToString("F9"), "O", "P", "T", "C", "O", user, acteGestion);
            }
        }

        private static QuittanceForceFormuleDto GetInfoFormule(string codeOffre, string version, string codeAvn, string typeHTTC, ModeConsultation modeNavig, string acteGestion)
        {
            QuittanceForceFormuleDto toReturn = new QuittanceForceFormuleDto();
            string sql = string.Format(@"SELECT DISTINCT KDAID CODEFOR, KDAALPHA FORMLETTRE, KDADESC FORMDESC, KDDRSQ CODERSQ, {2} MONTANTCAL, {3} MONTANTFORCE, KDBFOR NUMFOR, PTFOR
                                            FROM {4}
                                                   INNER JOIN {5} ON KDBIPB = KDAIPB AND KDBALX = KDAALX AND KDBTYP = KDATYP AND KDBFOR = KDAFOR {8}
                                                   INNER JOIN {6} ON KDDKDBID = KDBID
                                                   INNER JOIN {9} ON PTIPB = KDAIPB AND PTALX = KDAALX AND PTFOR = KDAFOR
                                            WHERE KDAIPB = '{0}' AND KDAALX = {1} {7}
                                            ORDER BY PTFOR",
                                            codeOffre.PadLeft(9, ' '), version, typeHTTC == "HT" ? "PTMHT" : "PTMTT", typeHTTC == "HT" ? "KDBCHT" : "KDBCTT",
                                            CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                                            CommonRepository.GetPrefixeHisto(modeNavig, "KPOPT"),
                                            CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTAP"),
                                            modeNavig == ModeConsultation.Historique ? string.Format(" AND KDAAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                            modeNavig == ModeConsultation.Historique ? " AND KDBAVN = KDAAVN" : string.Empty,
                                            CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTA"));

            var result = DbBase.Settings.ExecuteList<QuittanceForceInfoFormuleDto>(CommandType.Text, sql);
            if (result != null && result.Any())
                toReturn.ListFormule = result;
            return toReturn;
        }

        private static string UpdateCalculFormuleTTCForce(string codeOffre, string type, string version, string avenant, string codeRsq, string codeFor, string montantForce, string user, string acteGestion, string reguleId, string valeurReturn)
        {
            double montantCal = 0;
            string sql = string.Format(@"SELECT PTMTT MONTANTTTC FROM {4} WHERE PTIPB = '{0}' AND PTALX = {1} AND PTRSQ = {2} AND PTFOR = {3}",
                                codeOffre.PadLeft(9, ' '), version, codeRsq, codeFor,
                                CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTA"));
            var result = DbBase.Settings.ExecuteList<QuittanceForceTotalDto>(CommandType.Text, sql);
            if (result != null && result.Any())
                montantCal = result.FirstOrDefault().MontantCalculeTTC;

            double coeff = 0;
            if (montantCal != 0)
                coeff = Convert.ToDouble(montantForce) / montantCal;

            if (Math.Abs(coeff) > 9999)
            {
                coeff = 1;
            }

            switch (acteGestion)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    return CommonRepository.UpdateCalculForceRegule(codeOffre, type, version, avenant, codeRsq, codeFor, montantCal.ToString("F9"), montantForce, coeff.ToString("F9"), "P", "H", "C", user, acteGestion, reguleId);
                default:
                    if (valeurReturn == "0")
                    {
                        CommonRepository.UpdateCalculMntForce(codeOffre, type, version, avenant, "O", codeRsq, codeFor, montantForce, "T", user, acteGestion);
                        return CommonRepository.UpdateCalculForce(codeOffre, type, version, avenant, codeRsq, codeFor, montantForce, montantForce, "1", "O", "T", "T", "F", "O", user, acteGestion);
                    }
                    else
                    {
                        return CommonRepository.UpdateCalculForce(codeOffre, type, version, avenant, codeRsq, codeFor, montantCal.ToString("F9"), montantForce, coeff.ToString("F9"), "O", "T", "T", "C", "O", user, acteGestion);
                    }
            }
        }

        private static string UpdateCalculFormuleHTForce(string codeOffre, string type, string version, string avenant, string codeRsq, string codeFor, string montantForce, string user, string acteGestion, string reguleId, string valeurReturn)
        {
            double montantCal = 0;
            string sql = string.Format(@"SELECT PTMHT MONTANTHT FROM {4} WHERE PTIPB = '{0}' AND PTALX = {1} AND PTRSQ = {2} AND PTFOR = {3}",
                                codeOffre.PadLeft(9, ' '), version, codeRsq, codeFor,
                                CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTA"));
            var result = DbBase.Settings.ExecuteList<QuittanceForceTotalDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                var quittanceForceTotalDto = result.FirstOrDefault();
                if (quittanceForceTotalDto != null) montantCal = quittanceForceTotalDto.MontantCalculeHT;
            }

            double coeff = 0;
            if (montantCal != 0)
                coeff = Convert.ToDouble(montantForce) / montantCal;

            if (Math.Abs(coeff) > 9999)
            {
                coeff = 1;
            }

            switch (acteGestion)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    return CommonRepository.UpdateCalculForceRegule(codeOffre, type, version, avenant, codeRsq, codeFor, montantCal.ToString("F9"), montantForce, coeff.ToString("F9"), "P", "H", "C", user, acteGestion, reguleId);
                default:
                    if (valeurReturn == "0")
                    {
                        CommonRepository.UpdateCalculMntForce(codeOffre, type, version, avenant, "O", codeRsq, codeFor, montantForce, "H", user, acteGestion);
                        return CommonRepository.UpdateCalculForce(codeOffre, type, version, avenant, codeRsq, codeFor, montantForce, montantForce, "1", "O", "T", "H", "F", "O", user, acteGestion);
                    }
                    else
                    {
                        return CommonRepository.UpdateCalculForce(codeOffre, type, version, avenant, codeRsq, codeFor, montantCal.ToString("F9"), montantForce, coeff.ToString("F9"), "O", "T", "H", "C", "O", user, acteGestion);
                    }
            }
        }

        #endregion

        #endregion

        #region Echeancier
        public static EcheancierDto InitEcheancier(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string modeSaisieEcheancierParam)
        {
            var echeancierDto = new EcheancierDto();
            //Gestion du mode de saisie (par montant ou %)
            echeancierDto.IsModeSaisieParMontant = true;
            string sqlModeSaisie = string.Format(@"SELECT PIPCR POURCENTAGEPRIME 
                                                   FROM YPOECHE 
                                                   WHERE PIIPB = '{0}' AND PIALX = {1} AND PITYP = '{2}' AND PIIPK = 0", codeOffre.PadLeft(9, ' '), version, type);
            var resModeSaisie = DbBase.Settings.ExecuteList<EcheanceDto>(CommandType.Text, sqlModeSaisie);
            if (resModeSaisie != null && resModeSaisie.Any())
            {
                if (resModeSaisie.FirstOrDefault().PourcentagePrime > 0)
                    echeancierDto.IsModeSaisieParMontant = false;
            }
            //Si le mode de saisie demandé est différent du mode de saisie présent en base => RAZ des valeurs
            if (!string.IsNullOrEmpty(modeSaisieEcheancierParam))
            {
                if (echeancierDto.IsModeSaisieParMontant != (modeSaisieEcheancierParam == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.ModeSaisieEcheancier.Montant)))
                {
                    string sqlRAZ = string.Format(@"UPDATE YPOECHE
                                                SET PIPCR = 0, PIPCC = 0, PIPMR = 0, PIPMC = 0
                                                WHERE PIIPB = '{0}' AND PIALX = {1} AND PITYP = '{2}' 
                                                AND PIIPK = 0", codeOffre.PadLeft(9, ' '), version, type);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlRAZ);
                    echeancierDto.IsModeSaisieParMontant = (modeSaisieEcheancierParam == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.ModeSaisieEcheancier.Montant));
                }
            }

            var comptant = GetComptant(codeOffre, version, type, codeAvn, modeNavig);
            if (comptant != null)
            {
                if (string.IsNullOrEmpty(codeAvn) || codeAvn == "0")
                    echeancierDto.ComptantHT = comptant.Montant == 0 ? comptant.MontantCalcule : comptant.Montant;
                else
                    echeancierDto.ComptantHT = comptant.Montant == 0 ? 0 : comptant.Montant;

                echeancierDto.PrimeComptant = comptant.PourcentagePrime;
                echeancierDto.FraisAccessoire = comptant.FraisAccessoire;
            }
            echeancierDto.Echeances = GetEcheances(codeOffre, version, type, codeAvn, 2, modeNavig);

            var quittances = GetQuittances(codeOffre, version, type, codeAvn, string.Empty, string.Empty, modeNavig, string.Empty, string.Empty, string.Empty);
            if (quittances != null && quittances.Any())
            {
                if (quittances.FirstOrDefault().DebutPeriodeAnnee != 0 && quittances.FirstOrDefault().DebutPeriodeMois != 0 && quittances.FirstOrDefault().DebutPeriodeJour != 0)
                    echeancierDto.PeriodeDebut = new DateTime(quittances.FirstOrDefault().DebutPeriodeAnnee, quittances.FirstOrDefault().DebutPeriodeMois, quittances.FirstOrDefault().DebutPeriodeJour).ToShortDateString();
                //echeancierDto.PrimeHT = quittances.FirstOrDefault().TotalHorsFraisHT;
                //Récup prime HT
                string sqlPrimeHT = string.Format(@"SELECT  CAST(JDTMG AS NUMERIC(13,2)) MONTANT
                                                    FROM YPRTENT
                                                    WHERE JDIPB = '{0}' AND JDALX = {1}", codeOffre.PadLeft(9, ' '), version);
                var resultPrimeHT = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlPrimeHT);
                if (resultPrimeHT != null && resultPrimeHT.Any())
                {
                    echeancierDto.PrimeHT = resultPrimeHT.FirstOrDefault().Montant;
                }
                else
                {
                    echeancierDto.PrimeHT = quittances.FirstOrDefault().TotalHorsFraisHT;
                }

                echeancierDto.FraisAccessoiresHT = quittances.FirstOrDefault().FraisHT;
                echeancierDto.TaxeAttentat = quittances.FirstOrDefault().FGATaxe != 0;

                //SLA (25.08.15) : ajout suite bug 1551
                //En affaire nouvelle : Si l'enregistrement sur lecture de YPOECHE sur arguments TYP, IPB, ALX  PIEHE = '1'  ET PIIPK = 0
                //n'est pas trouvé : alimenter par PKKHT :
                if ((comptant == null || echeancierDto.ComptantHT == 0) && (codeAvn == "0"))
                {
                    echeancierDto.ComptantHT = quittances.FirstOrDefault().TotalHorsFraisHT;
                }
            }
            return echeancierDto;
        }

        public static List<EcheanceDto> GetEcheances(string codeOffre, string version, string type, string codeAvn, int typeEcheance, ModeConsultation modeNavig)
        {
            string sql = string.Format(@" SELECT
                                    PIPMR MONTANT,
                                    PIPMC MONTANTCALCULE,
                                    PIPCR POURCENTAGEPRIME,
                                    PIPCC POURCENTAGECALCULE,
                                    PIEHA DATEECHA,
                                    PIEHM DATEECHM,
                                    PIEHJ DATEECHJ,
                                    PIAFR FRAISACCESSOIRE,
                                    PIATT APPLIQUETAXEATTENTAT,
                                    PIIPK NUMPRIME
                                    FROM {0} 
                                    WHERE PIIPB='{1}' AND PIALX='{2}' AND PITYP='{3}' AND {4} AND PIIPK=0 {5} AND PIIPK = 0
                                    ORDER BY PIEHE, DATEECHA,DATEECHM,DATEECHJ",
                            CommonRepository.GetPrefixeHisto(modeNavig, "YPOECHE"),
                            codeOffre.Trim().PadLeft(9, ' '), version, type, typeEcheance == 0 ? "(PIEHE = 1 OR PIEHE = 2)" : string.Format("PIEHE = {0}", typeEcheance),
                            modeNavig == ModeConsultation.Historique ? string.Format(" AND PIAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);
            return DbBase.Settings.ExecuteList<EcheanceDto>(CommandType.Text, sql);
        }
        public static List<EcheanceDto> GetAllEcheances(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            string sql = string.Format(@" SELECT  PIPMR MONTANT,
                                    PIPMC MONTANTCALCULE,
                                    PIPCR POURCENTAGEPRIME,
                                    PIPCC POURCENTAGECALCULE,
                                    PIEHA DATEECHA,
                                    PIEHM DATEECHM,
                                    PIEHJ DATEECHJ,
                                    PIAFR FRAISACCESSOIRE,
                                    PIATT APPLIQUETAXEATTENTAT
                                    FROM {3}                                                                                                    
                                    WHERE PIIPB='{0}' AND PIALX='{1}' AND PITYP='{2}' {4}",
                                    codeOffre.PadLeft(9, ' '), version, type,
                                    CommonRepository.GetPrefixeHisto(modeNavig, "YPOECHE"),
                                    modeNavig == ModeConsultation.Historique ? string.Format(" AND PIAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);
            return DbBase.Settings.ExecuteList<EcheanceDto>(CommandType.Text, sql);
        }
        public static bool PossedeEcheances(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            string sql = string.Format(@" SELECT  COUNT(*) NBLIGN
                                    FROM {3}                                                                                                    
                                    WHERE PIIPB='{0}' AND PIALX='{1}' AND PITYP='{2}' {4}",
                                    codeOffre, version, type,
                                    CommonRepository.GetPrefixeHisto(modeNavig, "YPOECHE"),
                                    modeNavig == ModeConsultation.Historique ? string.Format(" AND PIAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);

            return CommonRepository.ExistRow(sql);
        }

        private static EcheanceDto GetComptant(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            string sql = string.Format(@" SELECT
                            PIPMR MONTANT,
                            PIPMC MONTANTCALCULE,
                            PIPCR POURCENTAGEPRIME,
                            PIPCC POURCENTAGECALCULE
                            FROM {0}
                             WHERE PIIPB='{1}' AND PIALX='{2}' AND PITYP='{3}' AND PIEHE=1 AND PIIPK=0",
                        CommonRepository.GetPrefixeHisto(modeNavig, "YPOECHE"),
                        codeOffre.Trim().PadLeft(9, ' '), version, type,
                        modeNavig == ModeConsultation.Historique ? string.Format(" AND PIAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);
            return DbBase.Settings.ExecuteList<EcheanceDto>(CommandType.Text, sql).FirstOrDefault();
        }
        public static string UpdateEcheance(string codeOffre, string version, string type, DateTime? dateEcheance, decimal PrimePourcent, decimal montantEcheance, decimal montantCalcule, decimal fraisAccessoires, bool taxeAttentat, string typeOperation, int typeEcheance)
        {
            DbParameter[] param = new DbParameter[14];
            param[0] = new EacParameter("P_ID_OFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_TYPE_OFFRE", type);
            param[2] = new EacParameter("P_VERSION_OFFRE", version);
            param[3] = new EacParameter("P_ANNEE_ECH", 0);
            param[3].Value = dateEcheance.HasValue ? dateEcheance.Value.Year : 0;
            param[4] = new EacParameter("P_MOIS_ECH", 0);
            param[4].Value = dateEcheance.HasValue ? dateEcheance.Value.Month : 0;
            param[5] = new EacParameter("P_JOUR_ECH", 0);
            param[5].Value = dateEcheance.HasValue ? dateEcheance.Value.Day : 0;
            param[6] = new EacParameter("P_PRIME_POURCENT", PrimePourcent);
            param[7] = new EacParameter("P_MONTANT_ECH", montantEcheance);
            param[8] = new EacParameter("P_MONTANT_CALC", montantCalcule);
            param[9] = new EacParameter("P_FRAIS_ACCESSOIRS", fraisAccessoires);
            param[10] = new EacParameter("P_ATTENTAT", taxeAttentat ? "O" : "N");
            param[11] = new EacParameter("P_MODE", typeOperation);
            param[12] = new EacParameter("P_TYPE_ECH", 0);
            param[12].Value = typeEcheance;
            param[13] = new EacParameter("P_ERREUR", "");
            param[13].Direction = ParameterDirection.InputOutput;
            param[13].Size = 256;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDECH", param);
            return param[13].Value.ToString();
        }
        public static string UpdatePourcentCalcule(string codeOffre, string version, string type, string codeAvn, double comptantHT, double primeHT, ModeConsultation modeNavig)
        {
            string toReturn = string.Empty;
            var echeancesDto = GetEcheances(codeOffre, version, type, codeAvn, 2, modeNavig);

            double totalPourcent = 0;
            double diff = 0;
            double maxMontant = 0;
            int maxIndex = 0;
            int i = 0;
            double primeComptantCalcule = 0;
            short anneeFGACourante = 0;
            foreach (var ech in echeancesDto)
            {

                ech.PourcentageCalcule = Math.Round(ech.Montant != 0 ? double.Parse((ech.Montant / primeHT).ToString()) : double.Parse((ech.MontantCalcule / primeHT).ToString()), 6);
                totalPourcent += double.Parse(ech.PourcentageCalcule.ToString());

                if (ech.Montant > maxMontant || ech.MontantCalcule > maxMontant)
                {
                    maxMontant = ech.Montant != 0 ? ech.Montant : ech.MontantCalcule;
                    maxIndex = i;
                }
                if (ech.AppliqueTaxeAttentat == "O")
                {
                    if (anneeFGACourante == ech.DateEcheanceAnnee)
                    {
                        toReturn = "Erreur : La taxe FGA ne peut être perçue qu'une fois par année.";
                        return toReturn;
                    }
                    else
                    {
                        anneeFGACourante = ech.DateEcheanceAnnee;
                    }
                }
                i++;
            }
            primeComptantCalcule = Math.Round(double.Parse((comptantHT / primeHT).ToString()), 6);
            totalPourcent += double.Parse(primeComptantCalcule.ToString());

            diff = 1 - totalPourcent;
            if (diff != 0)
            {
                double val = 0;
                if (comptantHT > maxMontant)
                    primeComptantCalcule += double.TryParse(diff.ToString(), out val) ? val : val;
                else
                    echeancesDto[maxIndex].PourcentageCalcule += double.TryParse(diff.ToString(), out val) ? val : val;
            }

            //Update pourcent calculé du comptant
            DbParameter[] param1 = new DbParameter[4];
            string sql1 = @"UPDATE YPOECHE     
                                     SET PIPCC = :POURCENT                                        
                                     WHERE PIIPB = :IDOFFRE  AND PIALX = :VERSIONOFFRE AND PITYP =:TYPEOFFRE
                                        AND PIEHE = 1";
            param1[0] = new EacParameter("POURCENT", 0);
            param1[0].Value = Convert.ToDecimal(primeComptantCalcule * 100);
            param1[1] = new EacParameter("IDOFFRE", codeOffre.PadLeft(9, ' '));
            param1[2] = new EacParameter("VERSIONOFFRE", version);
            param1[3] = new EacParameter("TYPEOFFRE", type);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql1, param1);

            //Update pourcent calculé des échéances
            string sql2 = string.Empty;
            foreach (var ech in echeancesDto)
            {
                DbParameter[] param2 = new DbParameter[7];
                sql2 = @"UPDATE YPOECHE 
                                     SET       
                                     PIPCC = :POURCENT
                                    WHERE  PIIPB = :IDOFFRE  AND PIALX = :VERSIONOFFRE AND PITYP =:TYPEOFFRE
                                        AND PIEHA = :ANNEE AND PIEHM = :MOIS AND PIEHJ = :JOUR";
                param2[0] = new EacParameter("POURCENT", 0);
                param2[0].Value = Convert.ToDecimal(ech.PourcentageCalcule * 100);
                param2[1] = new EacParameter("IDOFFRE", codeOffre.PadLeft(9, ' '));
                param2[2] = new EacParameter("VERSIONOFFRE", version);
                param2[3] = new EacParameter("TYPEOFFRE", type);
                param2[4] = new EacParameter("ANNEE", ech.DateEcheanceAnnee);
                param2[5] = new EacParameter("MOIS", ech.DateEcheanceMois);
                param2[6] = new EacParameter("JOUR", ech.DateEcheanceJour);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql2, param2);
            }
            return toReturn;
        }

        public static string UpdateMontantCalcule(string codeOffre, string version, string type, string codeAvn, string primePourcent, double comptantHT, double primeHT, ModeConsultation modeNavig)
        {
            string toReturn = string.Empty;
            var echeancesDto = GetEcheances(codeOffre, version, type, codeAvn, 2, modeNavig);
            double totalMontant = 0;
            double diff = 0;
            double maxMontant = 0;
            int maxIndex = 0;
            int i = 0;
            decimal primeComptantCalcule = 0;
            short anneeFGACourante = 0;
            foreach (var ech in echeancesDto)
            {
                ech.MontantCalcule = Math.Round(ech.PourcentagePrime != 0 ? double.Parse(((ech.PourcentagePrime / 100) * primeHT).ToString()) : double.Parse(((double.Parse(ech.PourcentageCalcule.ToString()) / 100) * primeHT).ToString()), 2);
                //ech.MontantCalcule = Math.Round(ech.PourcentagePrime != 0 ? double.Parse(((ech.PourcentagePrime / 100) * primeHT).ToString()) : double.Parse(((double.Parse(ech.PourcentageCalcule.ToString()) / 100) * primeHT).ToString()), 6);
                totalMontant += ech.MontantCalcule;

                if (ech.Montant > maxMontant || ech.MontantCalcule > maxMontant)
                {
                    maxMontant = ech.Montant != 0 ? ech.Montant : ech.MontantCalcule;
                    maxIndex = i;
                }
                if (ech.AppliqueTaxeAttentat == "O")
                {
                    if (anneeFGACourante == ech.DateEcheanceAnnee)
                    {
                        toReturn = "Erreur : La taxe FGA ne peut être perçue qu'une fois par année.";
                        return toReturn;
                    }
                    else
                    {
                        anneeFGACourante = ech.DateEcheanceAnnee;
                    }
                }
                i++;
            }

            primeComptantCalcule = Math.Round(decimal.Parse(comptantHT.ToString()), 2);
            //primeComptantCalcule = Math.Round(decimal.Parse(comptantHT.ToString()), 6);
            totalMontant += double.Parse(primeComptantCalcule.ToString());

            diff = primeHT - totalMontant;
            if (diff != 0 && Math.Abs(diff) <= 10)
            {

                if (comptantHT > maxMontant)
                {
                    decimal val = 0;
                    primeComptantCalcule += decimal.TryParse(diff.ToString(), out val) ? val : val;
                }
                else
                {
                    double val = 0;
                    echeancesDto[maxIndex].MontantCalcule += double.TryParse(diff.ToString(), out val) ? val : val;
                }
            }
            else if (diff != 0)
            {
                toReturn = "Erreur d'arrondi dans la ventilation";
                return toReturn;
            }

            //Update montant calculé du comptant
            DbParameter[] param1 = new DbParameter[4];
            string sql1 = @"UPDATE YPOECHE     
                                     SET PIPMC = :MONTANT                                         
                                     WHERE PIIPB = :IDOFFRE  AND PIALX = :VERSIONOFFRE AND PITYP =:TYPEOFFRE
                                        AND PIEHE = 1";
            param1[0] = new EacParameter("MONTANT", 0);
            param1[0].Value = Convert.ToDecimal(primeComptantCalcule);
            param1[1] = new EacParameter("IDOFFRE", codeOffre.PadLeft(9, ' '));
            param1[2] = new EacParameter("VERSIONOFFRE", version);
            param1[3] = new EacParameter("TYPEOFFRE", type);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql1, param1);

            //Update pourcent calculé des échéances
            string sql2 = string.Empty;
            double totalMontantEch = 0;
            double pourcentCommule = Convert.ToInt64(primePourcent);
            foreach (var ech in echeancesDto)
            {


                pourcentCommule += ech.PourcentagePrime;


                if (pourcentCommule == 100)
                {
                    ech.MontantCalcule = primeHT - (Math.Round(double.Parse(primeComptantCalcule.ToString()), 2) + totalMontantEch);
                }

                totalMontantEch += Math.Round(ech.MontantCalcule, 2);

                DbParameter[] param2 = new DbParameter[7];
                sql2 = @"UPDATE YPOECHE 
                                     SET       
                                     PIPMC = :MONTANT
                                    WHERE  PIIPB = :IDOFFRE  AND PIALX = :VERSIONOFFRE AND PITYP =:TYPEOFFRE
                                        AND PIEHA = :ANNEE AND PIEHM = :MOIS AND PIEHJ = :JOUR";
                param2[0] = new EacParameter("MONTANT", 0);
                param2[0].Value = Convert.ToDecimal(ech.MontantCalcule);
                param2[1] = new EacParameter("IDOFFRE", codeOffre.PadLeft(9, ' '));
                param2[2] = new EacParameter("VERSIONOFFRE", version);
                param2[3] = new EacParameter("TYPEOFFRE", type);
                param2[4] = new EacParameter("ANNEE", ech.DateEcheanceAnnee);
                param2[5] = new EacParameter("MOIS", ech.DateEcheanceMois);
                param2[6] = new EacParameter("JOUR", ech.DateEcheanceJour);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql2, param2);
            }
            return toReturn;
        }


        public static void SupprimerEcheance(string codeOffre, string version, string type, DateTime dateEcheance)
        {
            DbParameter[] param = new DbParameter[6];
            string sql = @"DELETE FROM YPOECHE
                                       WHERE  PIIPB = :IDOFFRE  AND PIALX = :VERSIONOFFRE AND PITYP =:TYPEOFFRE   
                                       AND PIEHA = :ANNEE AND PIEHM = :MOIS AND PIEHJ = :JOUR";
            param[0] = new EacParameter("IDOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("VERSIONOFFRE", version);
            param[2] = new EacParameter("TYPEOFFRE", type);
            param[3] = new EacParameter("ANNEE", dateEcheance.Year);
            param[4] = new EacParameter("MOIS", dateEcheance.Month);
            param[5] = new EacParameter("JOUR", dateEcheance.Day);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        public static void SupprimerEcheances(string codeOffre, string version, string type)
        {
            DbParameter[] param = new DbParameter[3];
            string sql = @"DELETE FROM YPOECHE
                                       WHERE  PIIPB = :IDOFFRE  AND PIALX = :VERSIONOFFRE AND PITYP =:TYPEOFFRE ";
            param[0] = new EacParameter("IDOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("VERSIONOFFRE", version);
            param[2] = new EacParameter("TYPEOFFRE", type);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        public static void SupprimerEcheancier(string codeOffre, string version, string type, string codeAvn)
        {
            string sqlDelete = string.Format(@"DELETE FROM YPOECHE
                                               WHERE PIIPB='{0}' AND PIALX = {1} AND PITYP = '{2}' AND PIIPK = 0", codeOffre.PadLeft(9, ' '), version, type);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDelete);

            string sqlPeriod = string.Format(@"UPDATE YPOBASE
                                               SET PBPER = 'U' 
                                               WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'", codeOffre.PadLeft(9, ' '), version, type);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlPeriod);
        }
        #endregion



        #region Montant Référence
        #region Méthodes Publiques
        public static MontantReferenceDto InitInfoMontantReference(string codeOffre, string version, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion, string mode = "CALCUL")
        {
            var result = string.Empty;
            if (!isReadonly && modeNavig == ModeConsultation.Standard)
            {
                result = CommonRepository.InitMontantRef(codeOffre, version, type, mode, modeNavig, codeAvn, user, acteGestion);
                //Update Periodes
            }
            if (result == "ERREUR")
            {
                return null;
            }
            else
            {
                return GetInfoMontantRef(codeOffre, version, type, codeAvn, result, modeNavig, isReadonly);
            }
        }

        public static MontantReferenceInfoDto GetMontantFormule(string codeOffre, string version, string type, string codeAvn, string codeRsq, string codeForm, ModeConsultation modeNavig)
        {
            var param = new List<DbParameter>
            {
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(version)),
                new EacParameter("type", type),
                new EacParameter("codeRsq", Convert.ToInt32(codeRsq)),
                new EacParameter("codeForm", Convert.ToInt32(codeForm))
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string sql = string.Format(@"SELECT KDAALPHA LETTREFORM, KDBFOR CODEFORM, KDBDESC LIBFORM, KDDRSQ CODERSQ, 
                                   KDBTMC MNTCALCULE, KDBTFF MNTFORCE, KDBPRO MNTPROVI, KDBACQ MNTACQUIS, KDBPAQ CHKMNTACQUIS
                                 FROM {0}
                                INNER JOIN {1} ON KDBKDAID = KDAID AND KDBOPT = 1
                                        INNER JOIN {2} ON KDDIPB = KDAIPB AND KDDALX = KDAALX AND KDDTYP = KDATYP {5}
                                        INNER JOIN {3} ON KDDIPB = KABIPB AND KDDALX = KABALX AND KDDTYP = KABTYP AND KDDRSQ = KABRSQ {6}
                               WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type AND KABRSQ = :codeRsq AND KDAFOR = :codeForm {4}",
                        /*0*/    CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                        /*1*/    CommonRepository.GetPrefixeHisto(modeNavig, "KPOPT"),
                        /*2*/    CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTAP"),
                        /*3*/    CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                        /*4*/    modeNavig == ModeConsultation.Historique ? " AND KABAVN = :avn" : string.Empty,
                        /*5*/    modeNavig == ModeConsultation.Historique ? " AND KDDAVN = KDAAVN" : string.Empty,
                        /*6*/    modeNavig == ModeConsultation.Historique ? " AND KDDAVN = KABAVN" : string.Empty);

            var result = DbBase.Settings.ExecuteList<MontantReferenceInfoDto>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0)
                return result.FirstOrDefault();
            return null;
        }

        public static MontantReferenceDto GetMontantTotal(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var param = new List<DbParameter> {
                new EacParameter("type", type),
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(version))
            };


            string sql = string.Format(@"SELECT JDTMC TOTALMNTCALCULE, JDTFF TOTALMNTFORCE, JDPRO TOTALMNTPROVI, JDACQ TOTALMNTACQUIS, KAAPAQ ISACQUIS
                            FROM {0} 
                            INNER JOIN KPENT ON KAAIPB = JDIPB AND KAAALX = JDALX AND KAATYP = :type
                            WHERE JDIPB = :codeOffre AND JDALX = :version {1}",
                            CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                            modeNavig == ModeConsultation.Historique ? string.Format(" AND JDAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);

            var result = DbBase.Settings.ExecuteList<MontantReferenceDto>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0)
                return result.FirstOrDefault();
            return null;
        }

        public static void ValidMontantFormule(string codeOffre, string version, string type, string codeRsq, string codeForm, decimal mntForce, bool mntProvi, decimal mntAcquis, bool chkMntAcquis)
        {
            //SLA (18/09/14) : désactivation sur Spec "- Contrat - montant de référence-V3.docx " du 04/09/14
            DbParameter[] param = new DbParameter[7];
            param[0] = new EacParameter("mntForce", 0);
            param[0].Value = mntForce;
            param[1] = new EacParameter("mntProvi", mntProvi ? "O" : "N");
            param[2] = new EacParameter("mntAcquis", 0);
            param[2].Value = mntAcquis;
            param[3] = new EacParameter("chkMntAcquis", chkMntAcquis ? "O" : "N");
            param[4] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
            param[5] = new EacParameter("version", 0);
            param[5].Value = Convert.ToInt32(version);
            //param[5] = new EacParameter("codeRsq", 0);
            //param[5].Value = Convert.ToInt32(codeRsq);
            param[6] = new EacParameter("codeForm", 0);
            param[6].Value = Convert.ToInt32(codeForm);

            //            string sql = @"UPDATE YPRTFOR 
            //                                            SET JOTFF = :mntForce, JOPRO = :mntProvi, JOACQ = :mntAcquis
            //                                        WHERE JOIPB = :codeOffre AND JOALX = :version AND JORSQ = :codeRsq AND JOFOR = :codeForm";

            string sql = @"UPDATE KPOPT 
                                            SET KDBTFF = :mntForce, KDBPRO = :mntProvi, KDBACQ = :mntAcquis, KDBPAQ = :chkMntAcquis
                                        WHERE KDBIPB = :codeOffre AND KDBALX = :version AND KDBFOR = :codeForm AND KDBOPT = 1";

            //string sql = string.Format(@"UPDATE KPOPT SET KDBACQ = {0} WHERE KDBIPB = '{1}' AND KDBALX = {2} AND KDBFOR = '{3}' AND KDBOPT = 1 ", mntAcquis, codeOffre, version, codeForm);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static void ValidMontantTotal(string codeOffre, string version, string type, decimal mntForce, decimal mntAcquis, bool checkedA, bool checkedP)
        {
            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("isRefForce", mntForce == 0 ? "N" : "O");
            param[1] = new EacParameter("mntForce", 0);
            param[1].Value = mntForce;
            param[2] = new EacParameter("mntProvi", checkedP ? "O" : "N");
            param[3] = new EacParameter("acquis", 0);
            param[3].Value = mntAcquis;
            param[4] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
            param[5] = new EacParameter("version", 0);
            param[5].Value = Convert.ToInt32(version);

            string sql = @"UPDATE YPRTENT 
                                SET JDTFO = :isRefForce, JDTFT = 'N', JDTFF = :mntForce, JDPRO = :mntProvi, JDACQ = :acquis
                            WHERE JDIPB = :codeOffre AND JDALX = :version";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            param = new DbParameter[4];
            param[0] = new EacParameter("acquis", checkedA ? "O" : "N");
            param[1] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("version", 0);
            param[2].Value = Convert.ToInt32(version);
            param[3] = new EacParameter("type", type);

            sql = @"UPDATE KPENT
                            SET KAAPAQ = :acquis
                        WHERE KAAIPB = :codeOffre AND KAAALX = :version AND KAATYP = :type";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static MontantReferenceDto UpdateMontantRef(string codeOffre, string version, string type, string codeAvn, bool topForce, bool topAcquis, bool topForceTotal, string commentForce, Int64 codeCommentForce, string user, ModeConsultation modeNavig, string acteGestion, bool reset = false)
        {
            var model = new MontantReferenceDto();

            if (reset)
            {
                var resultReset = CommonRepository.InitMontantRef(codeOffre, version, type, "INITFORCE", modeNavig, codeAvn, user, acteGestion);
                if (resultReset == "ERREUR")
                {
                    return null;
                }
                else
                {
                    return GetInfoMontantRef(codeOffre, version, type, codeAvn, resultReset, modeNavig, false);
                }
            }

            SaveCommentairesMontantRef(codeOffre, version, type, codeCommentForce, commentForce);

            if (topForce)
            {
                //---ZBO:14/04/2016 : Modification Specification Contrat – Montant de référence V5 --------------------------
                DbParameter[] param = new DbParameter[2];
                param[0] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
                param[1] = new EacParameter("version", 0);
                param[1].Value = Convert.ToInt32(version);

                string sql = @"UPDATE YPRTENT 
                                SET  JDTFT = 'O' WHERE JDIPB = :codeOffre AND JDALX = :version";

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                //------------------------------------------------------------------------------------
                var resultForce = CommonRepository.InitMontantRef(codeOffre, version, type, "FORCE", modeNavig, codeAvn, user, acteGestion);
                if (resultForce == "ERREUR")
                    return null;
                else if (topAcquis)
                {
                    var resultAcquis = CommonRepository.InitMontantRef(codeOffre, version, type, "ACQUIS", modeNavig, codeAvn, user, acteGestion);
                    if (resultAcquis == "ERREUR")
                        return null;
                    else
                        return GetInfoMontantRef(codeOffre, version, type, codeAvn, resultAcquis, modeNavig, false);
                }
                else
                    return GetInfoMontantRef(codeOffre, version, type, codeAvn, resultForce, modeNavig, false);
            }
            else if (topAcquis)
            {
                var resultAcquis = CommonRepository.InitMontantRef(codeOffre, version, type, "ACQUIS", modeNavig, codeAvn, user, acteGestion);
                if (resultAcquis == "ERREUR")
                    return null;
                else
                    return GetInfoMontantRef(codeOffre, version, type, codeAvn, resultAcquis, modeNavig, false);
            }

            if (topForceTotal)
            {
                var resultForceTotal = CommonRepository.InitMontantRef(codeOffre, version, type, "TOTALFORCE", modeNavig, codeAvn, user, acteGestion);
                if (resultForceTotal == "ERREUR")
                    return null;
                else
                    return GetInfoMontantRef(codeOffre, version, type, codeAvn, resultForceTotal, modeNavig, false);
            }

            return model;
        }

        public static void SaveCommentairesMontantRef(string codeOffre, string version, string type, Int64 codeCommentForce, string commentForce)
        {
            DbParameter[] param = new DbParameter[1];

            if (codeCommentForce == 0)
            {
                codeCommentForce = CommonRepository.GetAS400Id("KAJCHR");
                param = new DbParameter[5];
                param[0] = new EacParameter("CodeComment", 0);
                param[0].Value = codeCommentForce;
                param[1] = new EacParameter("CodeOffre", codeOffre.PadLeft(9, ' '));
                param[2] = new EacParameter("Version", 0);
                param[2].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                param[3] = new EacParameter("Type", type);
                param[4] = new EacParameter("Comment", commentForce);

                string sql = @"INSERT INTO KPOBSV (KAJCHR, KAJIPB, KAJALX, KAJTYP, KAJOBSV) VALUES (:CodeComment, :CodeOffre, :Version, :Type, :Comment)";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                param = new DbParameter[4];
                param[0] = new EacParameter("CodeComment", 0);
                param[0].Value = codeCommentForce;
                param[1] = new EacParameter("CodeOffre", codeOffre.PadLeft(9, ' '));
                param[2] = new EacParameter("Version", 0);
                param[2].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                param[3] = new EacParameter("Type", type);

                string sqlUpdate = @"UPDATE KPENT SET KAAOBSF  = :CodeComment WHERE KAAIPB = :CodeOffre AND KAAALX = :Version AND KAATYP = :Type";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdate, param);
            }
            else
            {
                param = new DbParameter[2];
                param[0] = new EacParameter("Comment", commentForce);
                param[1] = new EacParameter("CodeComment", 0);
                param[1].Value = codeCommentForce;

                string sqlUpdate = @"UPDATE KPOBSV SET KAJOBSV = :Comment WHERE KAJCHR = :CodeComment";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdate, param);
            }
        }

        #endregion
        #region Méthodes Privées

        private static MontantReferenceDto GetInfoMontantRef(string codeOffre, string version, string type, string codeAvn, string resultTop, ModeConsultation modeNavig, bool isReadonly)
        {
            var model = new MontantReferenceDto();


            var result = new List<MontantReferencePlatDto>();

            if (modeNavig == ModeConsultation.Historique)
            {
                DbParameter[] param = new DbParameter[4];
                param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
                param[1] = new EacParameter("P_VERSION", 0);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("P_TYPE", type);
                param[3] = new EacParameter("P_CODEAVENANT", 0);
                param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
                result = DbBase.Settings.ExecuteList<MontantReferencePlatDto>(CommandType.StoredProcedure, "SP_GTLSTMRHIST", param);
            }
            else
            {
                if (!isReadonly)
                {
                    //Mise à jour periode (SLA 28.05.2015 : sortie de la spo SP_GTLSTMR
                    DbParameter[] paramInit = new DbParameter[3];
                    paramInit[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
                    paramInit[1] = new EacParameter("P_VERSION", 0);
                    paramInit[1].Value = Convert.ToInt32(version);
                    paramInit[2] = new EacParameter("P_TYPE", type);
                    DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_INITMONTANTREF", paramInit);
                }

                DbParameter[] param = new DbParameter[4];
                param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
                param[1] = new EacParameter("P_VERSION", 0);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("P_TYPE", type);
                param[3] = new EacParameter("P_CODEAVT", 0);
                param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
                result = DbBase.Settings.ExecuteList<MontantReferencePlatDto>(CommandType.StoredProcedure, "SP_GTLSTMR", param);
            }

            if (result != null && result.Count > 0)
            {
                model.Periodicite = string.Format("{0} - {1}", result[0].CodePeriodicite, result[0].LibPeriodicite);
                if (result[0].EcheanceJour != 0 && result[0].EcheanceJour != 0)
                    model.EcheancePrincipale = string.Format("{0}/{1}", result[0].EcheanceJour.ToString().PadLeft(2, '0'), result[0].EcheanceMois.ToString().PadLeft(2, '0'));
                if (result[0].NextEcheanceAnnee != 0 && result[0].NextEcheanceMois != 0 && result[0].NextEcheanceJour != 0 && result[0].CodePeriodicite != "U" && result[0].CodePeriodicite != "E")
                    model.ProchaineEcheance = new DateTime(result[0].NextEcheanceAnnee,
                                                        result[0].NextEcheanceMois,
                                                        result[0].NextEcheanceJour, 0, 0, 0);
                if (result[0].PeriodeDebAnnee != 0 && result[0].PeriodeDebMois != 0 && result[0].PeriodeDebJour != 0)
                    model.PeriodeDeb = new DateTime(result[0].PeriodeDebAnnee,
                                                        result[0].PeriodeDebMois,
                                                        result[0].PeriodeDebJour, 0, 0, 0);

                model.TypeFraisAccessoires = result[0].FraisAccessoires;
                model.Montant = result[0].MntFraisAccessoires;
                model.TaxeAttentat = result[0].TaxeAttentat == "O";
                model.MontantForce = resultTop == "O";
                model.CommentForce = result[0].Observation;
                model.CodeCommentaire = result[0].CodeObservation;
                model.MontantsReference = GetListMontantRef(result);
                model.TotalMntCalcule = result[0].TotalMntCalcule;
                model.TotalMntForce = result[0].TotalMntForce;
                model.TotalMntAcquis = result[0].TotalMntAcquis;
                model.TotalMntProvi = result[0].TotalMntProvi;
            }

            return model;
        }

        private static List<MontantReferenceInfoDto> GetListMontantRef(List<MontantReferencePlatDto> result)
        {
            var lstMontantRef = new List<MontantReferenceInfoDto>();

            var lstInfo = result.GroupBy(el => el.CodeFormule).Select(r => r.First()).ToList();
            lstInfo.ForEach(i =>
            {
                var montantRef = new MontantReferenceInfoDto
                {
                    LettreForm = i.LettreFormule,
                    CodeForm = i.CodeFormule,
                    LibFormule = i.LibFormule,
                    CodeRsq = i.CodeRsq,
                    LibRisque = i.LibRisque,
                    MontantCalcule = i.MntCalcule,
                    MontantForce = i.MntForce,
                    MontantAcquis = i.MntAcquis,
                    MontantProvisionnel = i.MontantProvisionnel
                };
                lstMontantRef.Add(montantRef);
            });

            return lstMontantRef;
        }

        #endregion
        #endregion

        #region Fin Offre
        private static FinOffreDto GetFinOffreDto(string branche, string cible, FinOffrePlatDto finOffrePlatDto)
        {
            FinOffreDto toReturn = new FinOffreDto();

            toReturn.FinOffreInfosDto = new FinOffreInfosDto();
            toReturn.FinOffreInfosDto.Antecedent = finOffrePlatDto.Antecedent;
            toReturn.FinOffreInfosDto.Description = finOffrePlatDto.Description;
            toReturn.FinOffreInfosDto.ValiditeOffre = finOffrePlatDto.ValiditeOffre;
            toReturn.FinOffreInfosDto.DateProjet = AlbConvert.ConvertIntToDate(finOffrePlatDto.DateProjet);
            toReturn.FinOffreInfosDto.DateStatistique = AlbConvert.ConvertIntToDate(finOffrePlatDto.DateStatistique);
            toReturn.FinOffreInfosDto.Relance = finOffrePlatDto.Relance == "O";
            toReturn.FinOffreInfosDto.RelanceValeur = finOffrePlatDto.RelanceValeur;
            toReturn.FinOffreInfosDto.Preavis = finOffrePlatDto.Preavis;
            toReturn.FinOffreInfosDto.Antecedents = CommonRepository.GetParametres(branche, cible, "PRODU", "PBANT");

            if (toReturn.FinOffreInfosDto.DateProjet == null)
            {
                toReturn.FinOffreInfosDto.DateProjet = DateTime.Now;
            }
            if (toReturn.FinOffreInfosDto.DateStatistique == null)
            {
                toReturn.FinOffreInfosDto.DateStatistique = new DateTime(finOffrePlatDto.DateStatistiqueAnnee, finOffrePlatDto.DateStatistiqueMois, finOffrePlatDto.DateStatistiqueJour);
            }
            if (toReturn.FinOffreInfosDto.ValiditeOffre == 0)
            {
                toReturn.FinOffreInfosDto.ValiditeOffre = 60;
            }

            toReturn.FinOffreAnnotationDto = new FinOffreAnnotationDto();
            toReturn.FinOffreAnnotationDto.AnnotationFin = finOffrePlatDto.AnnotationFin;

            return toReturn;
        }

        public static ValidationEditionDto GetValidationEdition(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion, string modeEcran, bool isBNS)
        {
            ValidationEditionDto toReturn = null;
            if (type == AlbConstantesMetiers.TYPE_CONTRAT)
            {
                var table = string.Empty;


                var isValidate = CommonRepository.GetEtatActeGestion(codeOffre, int.Parse(version), type, codeAvn, isBNS);

                if (isValidate == "V")
                    table = "YPRIMES";
                else
                    table = "YPRIPES";

                var sql = string.Format(@"SELECT PKKHT C100HT,
                                                PKKNH C100CATNAT,
                                                PKKTT C100TTC,
                                                IFNULL(POKHT, PKKHT) ALBHT,
                                                IFNULL(POKNH, PKKNH) ALBCATNAT,
                                                IFNULL(POKTT, PKKTT) ALBTTC,
                                                CASE WHEN (SELECT COUNT(*) FROM KPAVTRC WHERE KHOIPB = PKIPB AND KHOALX = PKALX AND KHOTYP = '{3}' AND KHOOEF = 'NG') > 0 THEN 'O' ELSE 'N' END TRACEEMISS
                                         FROM {0}
                                         LEFT JOIN YPRIPPA ON POIPB = PKIPB AND POALX = PKALX AND POTYE = '1'
                                         WHERE PKIPB = '{1}' AND PKALX = {2} AND PKAVN = {4}",
                                            table, codeOffre.PadLeft(9, ' '), version, type,
                                            !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0);

                var result = DbBase.Settings.ExecuteList<ValidationEditionDto>(CommandType.Text, sql);
                if (result != null && result.Any())
                    toReturn = result.FirstOrDefault();
                if (toReturn == null)
                    toReturn = new ValidationEditionDto();

                if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
                {
                    string sqlRegule = string.Format(@"SELECT IFNULL(POKHT, PKKHT) REGULEHT,
                                                IFNULL(POKNH, PKKNH) REGULECATNAT,
                                                IFNULL(POKTT, PKKTT) REGULETTC,
                                                CASE WHEN (SELECT COUNT(*) FROM KPAVTRC WHERE KHOIPB = PKIPB AND KHOALX = PKALX AND KHOTYP = '{2}' AND KHOOEF = 'NR') > 0 THEN 'O' ELSE 'N' END TRACEEMISS
                                         FROM YPRIRES
                                         LEFT JOIN YPRIRPA ON POIPB = PKIPB AND POALX = PKALX AND POTYE = '1'
                                         WHERE PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {3}",
                                            codeOffre.PadLeft(9, ' '), version, type,
                                            !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0);

                    var resultRegule = DbBase.Settings.ExecuteList<ValidationEditionDto>(CommandType.Text, sqlRegule);
                    if (resultRegule != null && resultRegule.Any())
                    {
                        toReturn.CotReguleHT = resultRegule.FirstOrDefault().CotReguleHT;
                        toReturn.CotReguleCatNat = resultRegule.FirstOrDefault().CotReguleCatNat;
                        toReturn.CotReguleTTC = resultRegule.FirstOrDefault().CotReguleTTC;
                        toReturn.TraceEmissRegule = resultRegule.FirstOrDefault().TraceEmiss;
                    }
                }



                //Récup des engagements
                //AMO EPURATION
                var resEng = EngagementRepository.InitEngagement(codeOffre, version, type, codeAvn, string.Empty, modeNavig, false);
                if (resEng != null && resEng.Traites != null)
                    toReturn.EngagementsTraites = resEng.Traites;
                else
                    toReturn.EngagementsTraites = new List<WSAS400.DTO.Engagement.EngagementTraiteDto>();
            }
            if (toReturn == null)
                toReturn = new ValidationEditionDto();

            //Récup des documents éditables
            //AMO EPURATION
            toReturn.ListeDocuments = GetListeDocumentsEditables(codeOffre, version, type, modeNavig, modeEcran);

            return toReturn;
        }
        #endregion

        #region Document Gestion

        #region Méthodes publiques

        public static List<DocumentGestionDocDto> InitDocumentsGestion(string codeOffre, string version, string type, string codeAvt, string acteGestion, string typeAvenant, string user,
            ModeConsultation modeNavig, bool isReadOnly, bool init, bool isValidation, long[] docsId, bool firstLoad, string attesId, string regulid, Func<IEnumerable<DocumentGestionDocDto>> generateDocs = null)
        {
            if (firstLoad && modeNavig == ModeConsultation.Standard && !isReadOnly && !isValidation && (string.IsNullOrEmpty(attesId) || attesId == "0"))
            {
                List<DocumentGestionDocDto> listDocGen = GetListDocumentsLibreGenere(codeOffre, version, type, typeAvenant, user, modeNavig);

                if (listDocGen != null)
                    return listDocGen;
            }
            if (generateDocs != null)
            {
                return generateDocs().ToList();
            }
            //Ancien appel de génération des documents
            return GenerateDocumentsGestion(codeOffre, version, type, codeAvt, acteGestion, user, modeNavig, isReadOnly, init, isValidation, docsId, attesId, regulid);
        }

        public static DocumentGestionInfoDestDto ShowInfoDest(string idDest, string typeDest)
        {
            DocumentGestionInfoDestDto model = new DocumentGestionInfoDestDto();
            string sql = string.Empty;
            switch (typeDest)
            {
                case "CT":
                    sql = string.Format(@"SELECT COURTIER.TNNOM raisonsociale, IFNULL(INTERLOCUTEUR.TNNOM, '') NOMINTER, '' prenominter, IFNULL(INTERLOCUTEURDETAIL.TLAEM, '') EMAILINTER, 
                                                IFNULL(ABPLG3, TCAD1) BATIMENT, ABPNUM NUMEROVOIE, IFNULL(ABPL4F, TCAD2) EXTENSIONVOIE, ABPLG4 NOMVOIE, ABPDP6 departement, ABPCP6 CODEPOSTAL, ABPVI6 NOMVILLE
                                            FROM YCOURTI 
                                                LEFT JOIN YCOURTN COURTIER ON TCICT = COURTIER.TNICT AND COURTIER.TNXN5 = 0 AND COURTIER.TNTNM = 'A' 
                                                LEFT JOIN YCOURTN INTERLOCUTEUR ON INTERLOCUTEUR.TNICT = COURTIER.TNICT AND INTERLOCUTEUR.TNXN5 = 0 AND INTERLOCUTEUR.TNXN5 > 0 AND INTERLOCUTEUR.TNTNM = 'A' 
                                                LEFT JOIN YCOURTL INTERLOCUTEURDETAIL ON INTERLOCUTEURDETAIL.TLICT = TCICT AND INTERLOCUTEURDETAIL.TLINL = INTERLOCUTEUR.TNXN5
                                                LEFT JOIN YADRESS ON  TCADH = ABPCHR 
                                            WHERE TCICT = {0} AND TCICI = ''", idDest);
                    break;
                case "AS":
                    sql = string.Format(@"SELECT ANNOM RAISONSOCIALE, '' NOMINTER, '' PRENOMINTER, '' EMAILINTER, ABPLG3 BATIMENT, ABPNUM NUMEROVOIE, ABPEXT EXTENSIONVOIE,
                                                ABPLG4 NOMVOIE, ABPLG5 BOITEPOSTALE, ABPDP6 DEPARTEMENT, ABPCP6 CODEPOSTAL, ABPVI6 NOMVILLE 
                                            FROM YASSURE
                                               LEFT JOIN YASSNOM ON ANIAS = ASIAS AND ANINL = 0 AND ANTNM = 'A' 
                                               LEFT JOIN YADRESS ON ASADH = ABPCHR  
                                            WHERE ASIAS = {0}", idDest);
                    break;
            }

            if (!string.IsNullOrEmpty(sql))
            {
                var result = DbBase.Settings.ExecuteList<DocumentGestionInfoDestDto>(CommandType.Text, sql);
                if (result != null && result.Any())
                {
                    model = result.FirstOrDefault();
                    model.CodePostal = string.Format("{0}{1}", model.Departement, model.CP.ToString().PadLeft(3, '0'));
                }
            }

            return model;
        }

        public static void ValidSupprDoc(string selectDoc, string unselectDoc)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P_SELECTDOC", selectDoc);
            param[1] = new EacParameter("P_UNSELECTDOC", unselectDoc);
            param[2] = new EacParameter("P_CHARSEPFIELD", "|");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_VALIDSUPPRDOC", param);
        }

        public static List<SyntheseDocumentsDocDto> InitSyntheseDocument(string codeOffre, string version, string type)
        {
            List<SyntheseDocumentsDocDto> model = new List<SyntheseDocumentsDocDto>();

            string sql = string.Format(@"SELECT IFNULL(K2.KEMID, IFNULL(K1.KEMID, 0)) LOTID, IFNULL(K2.KEMORD, IFNULL(K1.KEMORD, 0)) ORDRE,
                        IFNULL(K2.KEMTYDS, IFNULL(K1.KEMTYDS, '')) TYPEDEST, IFNULL(K2.KEMIDS, IFNULL(K1.KEMIDS, 0)) IDDEST,
                        IFNULL(K2.KEMTYENV, IFNULL(K1.KEMTYENV, '')) TYPEENVOI, IFNULL(TPLIL, '') LIBTYPEENVOI,
                        KEQID DOCID, KEQNOM NOMDOC,
                        IFNULL(K2.KEMNBEX, IFNULL(KEQNBEX, 0)) NBEXEMPL, IFNULL(KEQDIMP, '') IMPRIMABLE
                                FROM KPDOCW
                                    LEFT JOIN KPDOCLDW K1 ON K1.KEMDOCA = KEQID
                                    LEFT JOIN KPDOCLDW K2 ON K2.KEMTYPL = KEQID
                                    LEFT JOIN YYYYPAR ON TCON = 'KHEOP' AND TFAM = 'TYDS' AND TCOD = IFNULL(K1.KEMTYENV, IFNULL(K1.KEMTYENV, ''))
                                WHERE KEQIPB = '{0}' AND KEQALX = {1} AND KEQTYP = '{2}'
                                ORDER BY ORDRE", codeOffre.PadLeft(9, ' '), version, type);

            var result = DbBase.Settings.ExecuteList<SyntheseDocumentsDocPlatDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                Int64 lotId = 0;
                SyntheseDocumentsDocDto doc = new SyntheseDocumentsDocDto { DocInfos = new List<SyntheseDocumentsDocInfoDto>() };
                foreach (var item in result)
                {
                    if (lotId != item.LotId)
                    {
                        if (lotId != 0)
                            model.Add(doc);
                        lotId = item.LotId;
                        doc = new SyntheseDocumentsDocDto
                        {
                            LotId = item.LotId,
                            Ordre = item.Ordre,
                            TypeDestinataire = item.TypeDest,
                            DestinataireId = item.IdDest,
                            TypeEnvoi = item.TypeEnvoi,
                            LibEnvoi = item.LibTypeEnvoi,
                            DocInfos = new List<SyntheseDocumentsDocInfoDto>()
                        };
                    }
                    SyntheseDocumentsDocInfoDto docInfo = new SyntheseDocumentsDocInfoDto
                    {
                        DocId = item.DocId,
                        Document = item.NomDoc,
                        NbExemp = item.NbExempl,
                        Imprim = item.Imprimable
                    };
                    doc.DocInfos.Add(docInfo);
                }
                model.Add(doc);
            }
            return model;
        }

        public static void SaveTraceArbreFinAffnouv(string codeOffre, string version, string type, string user)
        {
            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = codeOffre.PadLeft(9, ' '),
                Version = Convert.ToInt32(version),
                Type = type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin),
                NumeroOrdreDansEtape = 70,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin),
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
        }

        public static void ChangeSituationDoc(string idDoc, string situation)
        {
            string sql = string.Format(@"UPDATE KPDOCLDW SET KEMSIT = '{0}' WHERE KEMID = {1}",
                                    situation, idDoc);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        public static void RegenerateDocLibre(string codeOffre, string version, string type, string idsDoc, string user)
        {
            var dateNow = DateTime.Now;

            DbParameter[] param = new DbParameter[7];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_IDSDOC", idsDoc);
            param[4] = new EacParameter("P_USER", user);
            param[5] = new EacParameter("P_DATENOW", 0);
            param[5].Value = AlbConvert.ConvertDateToInt(dateNow);
            param[6] = new EacParameter("P_HOURNOW", 0);
            param[6].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_REGENERATEDOCUMENTLIBRE", param);
        }

        #endregion

        #region Méthodes privées



        private static List<DocumentGestionDocPlatDto> LoadFilteredInfoDocuments(List<DocumentGestionDocPlatDto> listeDocuments, string typeAvt)
        {
            List<DocumentGestionDocPlatDto> listFiltered = null;
            IEnumerable<DocumentGestionDocPlatDto> query = null;

            if (listeDocuments != null && listeDocuments.Any())
                switch (typeAvt)
                {
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        listFiltered = listeDocuments;
                        break;

                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                        query = listeDocuments.Where(it => it.TypeDoc.Trim().Equals("REGUL") || it.TypeDoc.Trim().Equals("LETTYP"));
                        break;

                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        listFiltered = listeDocuments;
                        break;

                    default:
                        listFiltered = listeDocuments;
                        break;
                }

            if (query != null && query.Any())
                listFiltered = query.ToList();

            return listFiltered;
        }



        private static List<DocumentGestionDocDto> GetListDocumentsLibreGenere(string codeOffre, string version, string type, string typeAvenant, string user, ModeConsultation modeNavig)
        {
            List<DocumentGestionDocDto> model = null;
            //20160422 : ajout KEQSTG = 'G'
            string sql = string.Format(@"SELECT KELID IDLOT , KEMID IDLOTSDETAIL , KEMTYPD , KEMSIT SITUATION , KEMNTA NATURE , IFNULL ( DOC . KEQDIMP , '' ) IMPRIMABLE , IFNULL ( DOC . KEQCHM , '' ) CHEMIN , IFNULL ( DOC . KEQSTG , '' ) STATUT , IFNULL ( DOC . KEQTDOC , '' ) TYPEDOC , 
                                                          DOC . KEQID IDDOC , IFNULL ( DOC . KEQNOM , '' ) NOMDOC , IFNULL ( DOC . KEQLIB , '' ) LIBDOC , 
                                                          KEMTYDS TYPEDEST , KEMIDS IDDEST , KEMTYENV TYPEENVOI , IFNULL ( ENVOI . TPLIL , '' ) LIBTYPEENVOI , KEMNBEX NBEXEMPL , IFNULL ( DOC . KEQNBEX , 0 ) NBEXEMPLSUPP , KEMTAMP TAMPON , IFNULL ( TAMPON . TPLIL , '' ) LIBTAMPON , 
                                                          LETTRE . KEQID IDLETTRE , IFNULL ( LETTRE . KEQTDOC , '' ) TYPELETTRE , 
                                                          IFNULL ( LETTRE . KEQNOM , '' ) NOMLETTRE , IFNULL ( LETTRE . KEQLIB , '' ) LIBLETTRE , KEMLMAI LOTMAIL , IFNULL ( DOC . KEQTGL , '' ) ISLIBRE  
                                                    FROM KPDOCLW 
                                                          LEFT JOIN KPDOCLDW ON KEMKELID = KELID AND KEMSIT <> 'E'
                                                          LEFT JOIN KPDOCW DOC ON DOC . KEQID = KEMTYPL AND DOC.KEQSTG = 'G' AND DOC.KEQTGL = 'L'
                                                          LEFT JOIN KPDOCW LETTRE ON LETTRE . KEQID = KEMDOCA  AND LETTRE.KEQTGL = 'L' AND LETTRE.KEQSTG = 'G'
                                                          LEFT JOIN YYYYPAR ENVOI ON ENVOI . TCON = 'KHEOP' AND ENVOI . TFAM = 'TYDS' AND ENVOI . TCOD = KEMTYENV 
                                                          LEFT JOIN YYYYPAR TAMPON ON TAMPON . TCON = 'KHEOP' AND TAMPON . TFAM = 'TAMP' AND TAMPON . TCOD = KEMTAMP 
                                                 WHERE KELIPB =  '{0}' AND KELALX = {1} AND KELTYP = '{2}' AND DOC.KEQTGL = 'L'
                                                 ORDER BY KEMORD"
                        , codeOffre.PadLeft(9, ' '), version, type);

            var listDocument = LoadFilteredInfoDocuments(DbBase.Settings.ExecuteList<DocumentGestionDocPlatDto>(CommandType.Text, sql), typeAvenant);

            #region Traitement des données retournées

            if (listDocument != null && listDocument.Any())
            {
                model = new List<DocumentGestionDocDto>();
                Int64 docId = 0;
                DocumentGestionDocDto doc = new DocumentGestionDocDto();
                foreach (var item in listDocument)
                {
                    if (docId != item.DocId)
                    {
                        if (docId != 0)
                            model.Add(doc);
                        docId = item.DocId;
                        doc = new DocumentGestionDocDto
                        {
                            DocId = item.DocId,
                            ListDocInfos = new List<DocumentGestionDocInfoDto>(),
                            FirstGeneration = true
                        };
                    }
                    DocumentGestionDocInfoDto docInfo = new DocumentGestionDocInfoDto
                    {
                        IdDoc = item.IdDoc,
                        Situation = item.Situation,
                        Nature = item.Nature,
                        Imprimable = item.Imprimable,
                        Chemin = item.Chemin,
                        Statut = item.Statut,
                        TypeDoc = item.TypeDoc,
                        NomDoc = item.NomDoc,
                        LibDoc = item.LibDoc,
                        IdLotDetail = item.IdLotDetail,
                        TypeDestinataire = item.TypeDest,
                        Destinataire = item.IdDest,
                        CodeTypeEnvoi = item.TypeEnvoi,
                        TypeEnvoi = item.LibTypeEnvoi,
                        NbExemple = item.NbExempl,
                        NbExempleSupp = item.NbExemplSupp,
                        Tampon = item.Tampon,
                        LibTampon = item.LibTampon,
                        IdLettre = item.IdLettre,
                        TypeLettre = item.TypeLettre,
                        LettreAccomp = item.NomLettre,
                        LibLettre = item.LibLettre,
                        Email = item.LotMail.ToString(),
                        IsLibre = item.IsLibre == "L"
                    };
                    doc.ListDocInfos.Add(docInfo);
                }
                model.Add(doc);
            }
            #endregion

            return model;
        }

        private static List<DocumentGestionDocDto> GenerateDocumentsGestion(string codeOffre, string version, string type, string codeAvt, string acteGestion,
            string user, ModeConsultation modeNavig, bool isReadOnly, bool init, bool isValidation, long[] docsId, string attesId, string regulid)
        {
            List<DocumentGestionDocPlatDto> listDocument = null;
            List<DocumentGestionDocDto> model = new List<DocumentGestionDocDto>();

            if (isValidation || isReadOnly)
            {
                /*if( acteGestion == AlbConstantesMetiers.TRAITEMENT_AFFNV.ToString())
                {
                    acteGestion = "AFFNOUV";
                }*/
                string sql = string.Format(@"SELECT KELID IDLOT , KEMID IDLOTSDETAIL , KEMTYPD , KEMSIT SITUATION , KEMNTA NATURE , IFNULL ( DOC . KEQDIMP , '' ) IMPRIMABLE , IFNULL ( DOC . KEQCHM , '' ) CHEMIN , IFNULL ( DOC . KEQSTG , '' ) STATUT , IFNULL ( DOC . KEQTDOC , '' ) TYPEDOC , 
                                                          DOC . KEQID IDDOC , IFNULL ( DOC . KEQNOM , '' ) NOMDOC , IFNULL ( DOC . KEQLIB , '' ) LIBDOC , 
                                                          KEMTYDS TYPEDEST , KEMIDS IDDEST , KEMTYENV TYPEENVOI , IFNULL ( ENVOI . TPLIL , '' ) LIBTYPEENVOI , KEMNBEX NBEXEMPL , IFNULL ( DOC . KEQNBEX , 0 ) NBEXEMPLSUPP , KEMTAMP TAMPON , IFNULL ( TAMPON . TPLIL , '' ) LIBTAMPON , 
                                                          LETTRE . KEQID IDLETTRE , IFNULL ( LETTRE . KEQTDOC , '' ) TYPELETTRE , 
                                                          IFNULL ( LETTRE . KEQNOM , '' ) NOMLETTRE , IFNULL ( LETTRE . KEQLIB , '' ) LIBLETTRE , KEMLMAI LOTMAIL 
                                                    FROM KPDOCLW 
                                                          LEFT JOIN KPDOCLDW ON KEMKELID = KELID AND KEMSIT <> 'E'
                                                          LEFT JOIN KPDOCW DOC ON DOC . KEQID = KEMTYPL 
                                                          LEFT JOIN KPDOCW LETTRE ON LETTRE . KEQID = KEMDOCA 
                                                          LEFT JOIN YYYYPAR ENVOI ON ENVOI . TCON = 'KHEOP' AND ENVOI . TFAM = 'TYENV' AND ENVOI . TCOD = KEMTYENV 
                                                          LEFT JOIN YYYYPAR TAMPON ON TAMPON . TCON = 'KHEOP' AND TAMPON . TFAM = 'TAMP' AND TAMPON . TCOD = KEMTAMP 
                                                 WHERE KELIPB = '{0}' AND KELALX = {1} AND KELTYP = '{2}' 
                                                 ORDER BY KEMORD ;"
                                , codeOffre.PadLeft(9, ' '), version, type, acteGestion);

                listDocument = DbBase.Settings.ExecuteList<DocumentGestionDocPlatDto>(CommandType.Text, sql);
            }
            else
            {
                DateTime dateNow = DateTime.Now;
                #region Appel de la procédure stockée

                DbParameter[] param = new DbParameter[13];
                param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
                param[1] = new EacParameter("P_VERSION", 0);
                param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                param[2] = new EacParameter("P_TYPE", type);
                param[3] = new EacParameter("P_CODEAVN", 0);
                param[3].Value = !string.IsNullOrEmpty(codeAvt) ? Convert.ToInt32(codeAvt) : 0;
                param[4] = new EacParameter("P_SERVICE", "PRODU");
                //param[5] = new EacParameter("P_ACTEGES", type == "O" ? "OFFRE" : "AFFNOUV");
                param[5] = new EacParameter("P_ACTEGES", acteGestion);
                param[6] = new EacParameter("P_USER", user);
                param[7] = new EacParameter("P_DATENOW", 0);
                param[7].Value = AlbConvert.ConvertDateToInt(dateNow);
                param[8] = new EacParameter("P_HOURNOW", 0);
                param[8].Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow));
                param[9] = new EacParameter("P_INIT", (init && (!isReadOnly || acteGestion == AlbConstantesMetiers.TYPE_ATTESTATION) && modeNavig == ModeConsultation.Standard) ? "O" : "N");
                param[10] = new EacParameter("P_DOCSID", String.Join(";", docsId.Select(x => x.ToString())));
                param[11] = new EacParameter("P_ATTESID", 0);
                param[11].Value = !string.IsNullOrEmpty(attesId) ? Convert.ToInt32(attesId) : 0;
                param[12] = new EacParameter("P_REGULID", 0);
                param[12].Value = Convert.ToInt32(regulid);

                listDocument = DbBase.Settings.ExecuteList<DocumentGestionDocPlatDto>(CommandType.StoredProcedure, "SP_GENERATEDOCUMENTSGESTION", param);

                #endregion

                CopieDocRepository.CopierDocuments(codeOffre, version, type, codeAvt);
            }

            #region Traitement des données retournées

            if (listDocument != null && listDocument.Any())
            {
                Int64 docId = 0;
                DocumentGestionDocDto doc = new DocumentGestionDocDto();
                foreach (var item in listDocument)
                {
                    if (docId != item.DocId)
                    {
                        if (docId != 0)
                            model.Add(doc);
                        docId = item.DocId;
                        doc = new DocumentGestionDocDto
                        {
                            DocId = item.DocId,
                            ListDocInfos = new List<DocumentGestionDocInfoDto>(),
                            FirstGeneration = false
                        };
                    }
                    DocumentGestionDocInfoDto docInfo = new DocumentGestionDocInfoDto
                    {
                        IdDoc = item.IdDoc,
                        Situation = item.Situation,
                        Nature = item.Nature,
                        Imprimable = item.Imprimable,
                        Chemin = item.Chemin,
                        Statut = item.Statut,
                        TypeDoc = item.TypeDoc,
                        NomDoc = item.NomDoc,
                        LibDoc = item.LibDoc,
                        IdLotDetail = item.IdLotDetail,
                        TypeDestinataire = item.TypeDest,
                        Destinataire = item.IdDest,
                        CodeTypeEnvoi = item.TypeEnvoi,
                        TypeEnvoi = item.LibTypeEnvoi,
                        NbExemple = item.NbExempl,
                        NbExempleSupp = item.NbExemplSupp,
                        Tampon = item.Tampon,
                        LibTampon = item.LibTampon,
                        IdLettre = item.IdLettre,
                        TypeLettre = item.TypeLettre,
                        LettreAccomp = item.NomLettre,
                        LibLettre = item.LibLettre,
                        Email = item.LotMail.ToString(),
                        IsLibre = item.IsLibre == "L"
                    };
                    doc.ListDocInfos.Add(docInfo);
                }
                model.Add(doc);
            }

            #endregion


            return model;
        }

        public static List<DocumentGestionDocDto> GetListeDocumentsEditables(string codeOffre, string version, string type, ModeConsultation modeNavig, string modeEcran = "")
        {
            List<DocumentGestionDocDto> model = new List<DocumentGestionDocDto>();
            string sql = string.Format(@"SELECT KELID IDLOT , KEMID IDLOTSDETAIL , KEMTYPD , KEMSIT SITUATION , KEMNTA NATURE , IFNULL ( DOC . KEQDIMP , '' ) IMPRIMABLE , IFNULL ( DOC . KEQCHM , '' ) CHEMIN , IFNULL ( DOC . KEQSTG , '' ) STATUT , IFNULL ( DOC . KEQTDOC , '' ) TYPEDOC , 
                                                          DOC . KEQID IDDOC , IFNULL ( DOC . KEQNOM , '' ) NOMDOC, IFNULL ( DOC . KEQLIB , '' ) LIBDOC , 
                                                          KEMTYDS TYPEDEST , KEMIDS IDDEST , KEMTYENV TYPEENVOI , IFNULL ( ENVOI . TPLIL , '' ) LIBTYPEENVOI , KEMNBEX NBEXEMPL , IFNULL ( DOC . KEQNBEX , 0 ) NBEXEMPLSUPP , KEMTAMP TAMPON , IFNULL ( TAMPON . TPLIL , '' ) LIBTAMPON , 
                                                          LETTRE . KEQID IDLETTRE , IFNULL ( LETTRE . KEQTDOC , '' ) TYPELETTRE , 
                                                          IFNULL ( LETTRE . KEQNOM , '' ) NOMLETTRE , IFNULL ( LETTRE . KEQLIB , '' ) LIBLETTRE , KEMLMAI LOTMAIL 
                                                    FROM KPDOCLW 
                                                          LEFT JOIN KPDOCLDW ON KEMKELID = KELID AND KEMSIT <> 'E'
                                                          INNER JOIN KPDOCW DOC ON DOC . KEQID = KEMTYPL {3}
                                                          LEFT JOIN KPDOCW LETTRE ON LETTRE . KEQID = KEMDOCA {4}
                                                          LEFT JOIN YYYYPAR ENVOI ON ENVOI . TCON = 'KHEOP' AND ENVOI . TFAM = 'TYDS' AND ENVOI . TCOD = KEMTYENV 
                                                          LEFT JOIN YYYYPAR TAMPON ON TAMPON . TCON = 'KHEOP' AND TAMPON . TFAM = 'TAMP' AND TAMPON . TCOD = KEMTAMP 
                                             WHERE KELIPB = '{0}' AND KELALX = {1} AND KELTYP = '{2}' 
                                             ORDER BY KEMORD ", codeOffre.PadLeft(9, ' '), version, type,
                                                         modeEcran != "valider" ? " AND DOC.KEQDIMP = 'O'" : string.Empty,
                                                         modeEcran != "valider" ? " AND LETTRE.KEQDIMP = 'O'" : string.Empty);

            var result = DbBase.Settings.ExecuteList<DocumentGestionDocPlatDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                Int64 docId = 0;
                DocumentGestionDocDto doc = new DocumentGestionDocDto();
                foreach (var item in result)
                {
                    if (docId != item.DocId)
                    {
                        if (docId != 0)
                            model.Add(doc);
                        docId = item.DocId;
                        doc = new DocumentGestionDocDto
                        {
                            DocId = item.DocId,
                            ListDocInfos = new List<DocumentGestionDocInfoDto>()
                        };
                    }
                    DocumentGestionDocInfoDto docInfo = new DocumentGestionDocInfoDto
                    {
                        LotId = item.DocId,
                        IdDoc = item.IdDoc,
                        Situation = item.Situation,
                        Nature = item.Nature,
                        Imprimable = item.Imprimable,
                        Chemin = item.Chemin,
                        Statut = item.Statut,
                        TypeDoc = item.TypeDoc,
                        NomDoc = item.NomDoc,
                        LibDoc = item.LibDoc,
                        IdLotDetail = item.IdLotDetail,
                        TypeDestinataire = item.TypeDest,
                        Destinataire = item.IdDest,
                        CodeTypeEnvoi = item.TypeEnvoi,
                        TypeEnvoi = item.LibTypeEnvoi,
                        NbExemple = item.NbExempl,
                        NbExempleSupp = item.NbExemplSupp,
                        Tampon = item.Tampon,
                        LibTampon = item.LibTampon,
                        IdLettre = item.IdLettre,
                        TypeLettre = item.TypeLettre,
                        LettreAccomp = item.NomLettre,
                        LibLettre = item.LibLettre,
                        Email = item.LotMail.ToString(),
                    };
                    doc.ListDocInfos.Add(docInfo);
                }
                model.Add(doc);
            }
            return model;
        }

        #endregion

        #endregion

        #region Document Gestion Détails

        #region Méthodes publiques

        public static List<ParametreDto> GetListeDocumentsDispo()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "TDOC", tCod: new List<string> { "LETTYP" });
        }

        public static List<ParametreDto> GetListeTypesDestinataire()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "TYDS");
        }

        public static List<ParametreDto> GetListeTypesEnvoi()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "TYENV");
        }

        public static List<ParametreDto> GetListeTampons()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "TAMP");
        }


        public static List<CourrierTypeDto> GetListeCourriersType(string filtre, string typeDoc)
        {
            string sql = string.Format(@"SELECT KDUID GUIDID, KDUNM2 CODE, KDULIB LIBELLE
                           FROM KCLAUSE
                           WHERE KDUTDOC = '{0}'", typeDoc);

            if (!string.IsNullOrEmpty(filtre))
            {
                sql += string.Format(@" AND (UPPER(KDUNM2) LIKE '%{0}%' OR UPPER(KDULIB) LIKE '%{0}%')", filtre.ToUpper());
            }
            return DbBase.Settings.ExecuteList<CourrierTypeDto>(CommandType.Text, sql);
        }

        public static List<DestinataireDto> GetListeDestinatairesDetails(string codeDocument)
        {
            string sql = string.Format(@"SELECT KEMID GUIDID, KEMDSTP ISPRINCIPAL, KEMTYDS CODETYPEDESTINATAIRE, TYPEDEST.TPLIB LIBTYPEDESTINATAIRE,
                                                KEMIDS CODEDESTINATAIRE,
                                                CASE KEMTYDS WHEN 'CT' THEN COURTIER.TNNOM 
                                                             WHEN 'COURT' THEN COURTIER.TNNOM
                                                               WHEN 'AS' THEN ASSURE.ANNOM
                                                               WHEN 'CI' THEN COMPAGNIE.CINOM
                                                               WHEN 'IN' THEN INTERV.IMNOM END  LIBDESTINATAIRE,
                                                CASE KEMTYDS WHEN 'IN' THEN INTERV.IMTYI ELSE '' END TYPEINTERVENANT,
                                                KEMINL CODEINTERLOCUTEUR, 
                                                CASE KEMTYDS WHEN 'CT' THEN COURTIERINTER.TNNOM 
                                                               WHEN 'AS' THEN ASSUREINTER.ANNOM
                                                               WHEN 'CI' THEN COMPINTER.CLNOM
                                                               WHEN 'IN' THEN INTERVINTER.IMNOM END  NOMINTERLOCUTEUR,                                             
                                                KEMTYENV TYPEENVOI,
                                                KEMNBEX NOMBREEX,
                                                KEMTAMP TAMPON,
                                                KEMDOCA LETTREACCOMP,
                                                KEMLMAI LOTEMAIL,
                                                KEMNTA ISGENERE
                                                FROM KPDOCLDW
                                                LEFT JOIN YYYYPAR TYPEDEST ON TYPEDEST.TCON = 'KHEOP' AND TFAM = 'TYDS' AND TCOD = KEMTYDS
                                                LEFT JOIN YCOURTN COURTIERINTER ON COURTIERINTER.TNICT = KEMIDS AND COURTIERINTER.TNXN5=KEMINL AND COURTIERINTER.TNXN5 != 0 
                                                LEFT JOIN YASSNOM ASSUREINTER ON ASSUREINTER.ANIAS = KEMIDS AND ASSUREINTER.ANINL = KEMINL AND ASSUREINTER.ANINL != 0
                                                LEFT JOIN YCOMPA COMPAGNIE ON COMPAGNIE.CIICN = KEMIDS 
                                                LEFT JOIN YCOMPAL COMPINTER ON COMPINTER.CLICI = COMPAGNIE.CIICI AND COMPINTER.CLIN5 = KEMINL AND COMPINTER.CLINL != 0
                                                LEFT JOIN YINTNOM INTERVINTER ON INTERVINTER.IMIIN = KEMIDS AND INTERVINTER.IMINL = KEMINL AND INTERVINTER.IMINL != 0                                                                           
                                                                               LEFT JOIN YCOURTN COURTIER ON COURTIER.TNICT = KEMIDS AND COURTIER.TNXN5 = 0 AND COURTIER.TNTNM = 'A'
                                                LEFT JOIN YASSNOM ASSURE ON ASSURE.ANIAS = KEMIDS AND  ASSURE.ANINL = 0 AND ASSURE.ANTNM = 'A'  
                                                LEFT JOIN YINTNOM INTERV ON INTERV.IMIIN = KEMIDS AND INTERV.IMINL = 0
                                         WHERE KEMID = {0}", codeDocument);
            return DbBase.Settings.ExecuteList<DestinataireDto>(CommandType.Text, sql);
        }

        public static List<DestinataireDto> GetListeCourtiers(string code, string type, string version, string codeDocument)
        {
            List<DestinataireDto> toReturn = new List<DestinataireDto>();

            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("P_CODEOFFRE", code.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODEDOC", codeDocument);
            param[4] = new EacParameter("P_TYPEDESTINATAIRE", "CT");
            param[5] = new EacParameter("P_TYPEINTERVENANT", string.Empty);
            #region pour debug de la requete
            //param[6] = new EacParameter("P_REQUEST_OUT", "");
            //param[6].Size = 8000;
            //param[6].Direction = ParameterDirection.InputOutput;
            #endregion
            var result = DbBase.Settings.ExecuteList<DestinataireDto>(CommandType.StoredProcedure, "SP_GETLISTEDESTINATAIRES", param);
            if (result != null && result.Any())
            {
                //détermination du rôle des courtiers
                var courtierApp = result.Find(elm => elm.TypeDestinataire == "COURTIERAPP");
                var courtier = result.Find(elm => elm.TypeDestinataire == "COURTIER");
                var courtierPayeur = result.Find(elm => elm.TypeDestinataire == "COURTIERPAYEUR");
                if (courtierApp != null && courtier != null && courtierPayeur != null && courtierApp.Code == courtier.Code)
                {
                    if (type == AlbConstantesMetiers.TYPE_OFFRE || (type == AlbConstantesMetiers.TYPE_CONTRAT && courtierApp.Code == courtierPayeur.Code))
                        toReturn.Add(new DestinataireDto
                        {
                            Code = courtier.Code,
                            GuidId = courtier.GuidId,
                            Libelle = courtier.Libelle,
                            Role = "Gestionnaire",
                            IsSelected = courtier.IsSelected
                        });
                }
                if (courtierApp != null && courtier != null && courtierApp.Code != courtier.Code)
                {
                    toReturn.Add(new DestinataireDto
                    {
                        Code = courtierApp.Code,
                        GuidId = courtierApp.GuidId,
                        Libelle = courtierApp.Libelle,
                        Role = "Apporteur",
                        IsSelected = courtierApp.IsSelected
                    });
                    toReturn.Add(new DestinataireDto
                    {
                        Code = courtier.Code,
                        GuidId = courtier.GuidId,
                        Libelle = courtier.Libelle,
                        Role = "Gestionnaire",
                        IsSelected = courtier.IsSelected
                    });

                    if (courtierPayeur != null && (courtierPayeur.Code != courtierApp.Code || courtierPayeur.Code != courtier.Code))
                    {
                        toReturn.Add(new DestinataireDto
                        {
                            Code = courtierPayeur.Code,
                            GuidId = courtierPayeur.GuidId,
                            Libelle = courtierPayeur.Libelle,
                            IsSelected = courtierPayeur.IsSelected,
                            Role = "Payeur"
                        });
                    }
                }

                //Ajout des cocourtiers
                var lstCocourtier = result.FindAll(elm => elm.TypeDestinataire == "COCOURTIER");
                if (lstCocourtier != null && lstCocourtier.Any())
                {
                    lstCocourtier.ForEach(elm =>
                                          toReturn.Add(new DestinataireDto
                                          {
                                              Code = elm.Code,
                                              GuidId = elm.GuidId,
                                              Libelle = elm.Libelle,
                                              Role = "Co-courtier",
                                              IsSelected = elm.IsSelected
                                          })
                                               );
                }

                //Ajout des autres courtiers
                var lstAutresCourtiers = result.FindAll(elm => elm.TypeDestinataire == "AUTRECOURTIER");
                if (lstAutresCourtiers != null && lstAutresCourtiers.Any())
                {
                    lstAutresCourtiers.ForEach(elm =>
                                              toReturn.Add(new DestinataireDto
                                              {
                                                  Code = elm.Code,
                                                  GuidId = elm.GuidId,
                                                  Libelle = elm.Libelle,
                                                  Role = "Courtier",
                                                  IsSelected = elm.IsSelected
                                              })
                                             );
                }

            }
            return toReturn;
        }

        public static List<DestinataireDto> GetListeAssures(string code, string type, string version, string codeDocument)
        {
            List<DestinataireDto> toReturn = new List<DestinataireDto>();

            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("P_CODEOFFRE", code.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODEDOC", codeDocument);
            param[4] = new EacParameter("P_TYPEDESTINATAIRE", "AS");
            param[5] = new EacParameter("P_TYPEINTERVENANT", string.Empty);

            var result = DbBase.Settings.ExecuteList<DestinataireDto>(CommandType.StoredProcedure, "SP_GETLISTEDESTINATAIRES", param);
            if (result != null && result.Any())
            {
                //ajout du preneur d'assurance
                var preneurAssurance = result.Find(elm => elm.TypeDestinataire == "PRENEURASSU");
                if (preneurAssurance != null && preneurAssurance.GuidId != 0)
                {
                    toReturn.Add(new DestinataireDto
                    {
                        Code = preneurAssurance.Code,
                        GuidId = preneurAssurance.GuidId,
                        Libelle = preneurAssurance.Libelle,
                        Role = "Preneur d'assurance",
                        IsSelected = preneurAssurance.IsSelected
                    });
                }

                //Ajout des co-assurés
                var lstCoAssure = result.FindAll(elm => elm.TypeDestinataire == "COASSURE");
                if (lstCoAssure != null && lstCoAssure.Any())
                {
                    lstCoAssure.ForEach(elm =>
                                          toReturn.Add(new DestinataireDto
                                          {
                                              Code = elm.Code,
                                              GuidId = elm.GuidId,
                                              Libelle = elm.Libelle,
                                              Role = "Co-assuré",
                                              IsSelected = elm.IsSelected
                                          })
                                               );
                }

                //Ajout des autres assurés
                var lstAutresAssures = result.FindAll(elm => elm.TypeDestinataire == "AUTREASSURE");
                if (lstAutresAssures != null && lstAutresAssures.Any())
                {
                    lstAutresAssures.ForEach(elm =>
                                              toReturn.Add(new DestinataireDto
                                              {
                                                  Code = elm.Code,
                                                  GuidId = elm.GuidId,
                                                  Libelle = elm.Libelle,
                                                  Role = "",
                                                  IsSelected = elm.IsSelected
                                              })
                                             );
                }
            }
            return toReturn;
        }

        public static List<DestinataireDto> GetListeCompagnies(string code, string type, string version, string codeDocument)
        {
            List<DestinataireDto> toReturn = new List<DestinataireDto>();

            DbParameter[] param = new DbParameter[7];
            param[0] = new EacParameter("P_CODEOFFRE", code.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODEDOC", codeDocument);
            param[4] = new EacParameter("P_TYPEDESTINATAIRE", "CI");
            param[5] = new EacParameter("P_TYPEINTERVENANT", string.Empty);
            #region pour debug de la requete
            //param[6] = new EacParameter("P_REQUEST_OUT", "");
            //param[6].Size = 8000;
            //param[6].Direction = ParameterDirection.InputOutput;
            #endregion
            var result = DbBase.Settings.ExecuteList<DestinataireDto>(CommandType.StoredProcedure, "SP_GETLISTEDESTINATAIRES", param);
            if (result != null && result.Any())
            {
                //Ajout des compagnies
                var lstCompagnies = result.FindAll(elm => elm.TypeDestinataire != "AUTRECOASSUREUR");
                if (lstCompagnies != null && lstCompagnies.Any())
                {
                    lstCompagnies.ForEach(elm =>
                                          toReturn.Add(new DestinataireDto
                                          {
                                              Code = elm.Code,
                                              GuidId = elm.GuidId,
                                              Libelle = elm.Libelle,
                                              Role = elm.TypeDestinataire == "APERITEUR" ? "Apériteur" : elm.TypeDestinataire == "COASSUREUR" ? "Co-Assureur" : string.Empty,
                                              IsSelected = elm.IsSelected
                                          })
                                               );
                }

                //Ajout des autres compagnies
                var lstAutresCompagnies = result.FindAll(elm => elm.TypeDestinataire == "AUTRECOASSUREUR");
                if (lstAutresCompagnies != null && lstAutresCompagnies.Any())
                {
                    lstAutresCompagnies.ForEach(elm =>
                                              toReturn.Add(new DestinataireDto
                                              {
                                                  Code = elm.Code,
                                                  GuidId = elm.GuidId,
                                                  Libelle = elm.Libelle,
                                                  Role = "",
                                                  IsSelected = elm.IsSelected
                                              })
                                             );
                }
            }
            return toReturn;
        }

        public static List<DestinataireDto> GetListeIntervenants(string code, string type, string version, string codeDocument, string typeIntervenant)
        {
            List<DestinataireDto> toReturn = new List<DestinataireDto>();

            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("P_CODEOFFRE", code.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODEDOC", codeDocument);
            param[4] = new EacParameter("P_TYPEDESTINATAIRE", "IN");
            param[5] = new EacParameter("P_TYPEINTERVENANT", typeIntervenant);
            #region pour debug de la requete
            //param[6] = new EacParameter("P_REQUEST_OUT", "");
            //param[6].Size = 8000;
            //param[6].Direction = ParameterDirection.InputOutput;
            #endregion
            var result = DbBase.Settings.ExecuteList<DestinataireDto>(CommandType.StoredProcedure, "SP_GETLISTEDESTINATAIRES", param);
            if (result != null && result.Any())
            {
                //Ajout des intervenants
                result.ForEach(elm =>
                                      toReturn.Add(new DestinataireDto
                                      {
                                          Code = elm.Code,
                                          GuidId = elm.GuidId,
                                          Libelle = elm.Libelle,
                                          Role = elm.TypeDestinataire,
                                          IsSelected = elm.IsSelected
                                      })
                               );

            }
            return toReturn;
        }

        public static DocumentGestionDetailsInfoGen GetInfoComplementairesDetailsDocumentGestion(string codeDocument)
        {
            string sql = string.Format(@"SELECT KEQTDOC TYPEDOCUMENT, KEQNBEX NBEXEMPLAIRE, KEMKELID LOTID, KDUID COURRIERID, KDUNM2 COURRIERCODE, KDULIB COURRIERLIB, KEQTGL DOCLIBRE, KEQSTG DOCGENER
                                          FROM KPDOCLDW
                                             LEFT JOIN KPDOCW ON KEMTYPL = KEQID
                                             INNER JOIN KCLAUSE ON TRIM(KDUNM1) CONCAT '_' CONCAT TRIM(KDUNM2) CONCAT '_' CONCAT TRIM(KDUNM3) = KEQCDOC
                                          WHERE KEMID = {0}", codeDocument);

            //            string sql = string.Format(@"SELECT KEQTDOC TYPEDOCUMENT, KEQNBEX NBEXEMPLAIRE, KEMKELID LOTID
            //                                          FROM KPDOCLDW
            //                                         LEFT JOIN KPDOCW ON KEMTYPL = KEQID
            //                                         WHERE KEMID = {0}", codeDocument);

            var result = DbBase.Settings.ExecuteList<DocumentGestionDetailsInfoGen>(CommandType.Text, sql);
            if (result != null && result.Any())
                return result.FirstOrDefault();
            else
                return null;
        }

        public static List<DestinataireDto> SaveLigneDestinataireDetails(string code, string version, string type, string user, string lotId,
            string typeDoc, string courrierType, string codeDocument, DestinataireDto destinataire, string acteGestion)
        {
            var dateNow = DateTime.Now;

            DbParameter[] param = new DbParameter[25];
            param[0] = new EacParameter("P_CODEOFFRE", code.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_SERVICE", "PRODU");
            param[4] = new EacParameter("P_ACTEGES", acteGestion);
            param[5] = new EacParameter("P_USER", user);
            param[6] = new EacParameter("P_DATENOW", 0);
            param[6].Value = AlbConvert.ConvertDateToInt(dateNow);
            param[7] = new EacParameter("P_HOURNOW", 0);
            param[7].Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow));
            param[8] = new EacParameter("P_LOTID", 0);
            param[8].Value = lotId;
            param[9] = new EacParameter("P_TYPEDOC", typeDoc);
            param[10] = new EacParameter("P_COURRIERTYPE", 0);
            param[10].Value = !string.IsNullOrEmpty(courrierType) ? Convert.ToInt32(courrierType) : 0;
            param[11] = new EacParameter("P_CODEDOCUMENT", 0);
            param[11].Value = codeDocument;
            param[12] = new EacParameter("P_ID", 0);
            param[12].Value = destinataire.GuidId;
            param[13] = new EacParameter("P_ISPRINCIPAL", destinataire.IsPrincipal);
            param[14] = new EacParameter("P_TYPEDESTINATAIRE", destinataire.TypeDestinataire);
            param[15] = new EacParameter("P_TYPEINTERVENANT", destinataire.TypeIntervenant);
            param[16] = new EacParameter("P_CODEDESTINATAIRE", 0);
            param[16].Value = destinataire.Code;
            param[17] = new EacParameter("P_CODEINTERLOCUTEUR", 0);
            param[17].Value = destinataire.CodeInterlocuteur;
            param[18] = new EacParameter("P_TYPEENVOI", destinataire.TypeEnvoi);
            param[19] = new EacParameter("P_NBEXEMPLAIRE", 0);
            param[19].Value = destinataire.NombreEx;
            param[20] = new EacParameter("P_TAMPON", destinataire.Tampon);
            param[21] = new EacParameter("P_LETTREACC", 0);
            param[21].Value = destinataire.LettreAccompagnement;
            param[22] = new EacParameter("P_LOTMAIL", 0);
            param[22].Value = destinataire.LotEmail;
            param[23] = new EacParameter("P_ISGENERE", destinataire.IsGenere);

            param[24] = new EacParameter("P_NEWCODELOTD", DbType.Int64);
            param[24].Value = 0;
            param[24].Direction = ParameterDirection.InputOutput;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEDETAILSDESTINATAIRE", param);

            codeDocument = param[24].Value.ToString();

            return GetListeDestinatairesDetails(codeDocument);
        }

        public static void SaveInformationsComplementairesDetailsDocument(string code, string type, string version, Int64 codeDocument, string document, Int64 courrierType, int nbExSupp, string user)
        {
            string sql = string.Empty;
            if (codeDocument >= 1)
            {
                //mode mise à jour
                sql = string.Format(@"UPDATE KPDOCW
                                      SET KEQNBEX = {0},
                                      KEQTDOC = '{1}'
                                      WHERE KEQID = {2}", nbExSupp, document, codeDocument);
            }
            if (!string.IsNullOrEmpty(sql))
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

        }

        public static List<DestinataireDto> DeleteLigneDestinataireDetails(string codeDocument, string guidIdLigne)
        {
            if (!string.IsNullOrEmpty(guidIdLigne))
            {
                Int64 guidId = 0;
                string sql = string.Empty;
                if (Int64.TryParse(guidIdLigne, out guidId))
                {
                    sql = string.Format(@"SELECT KEMTYPL INT64RETURNCOL FROM KPDOCLDW WHERE KEMID = {0}", guidId);
                    var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
                    if (result != null && result.Any())
                    {
                        var docId = result.FirstOrDefault().Int64ReturnCol;
                        sql = string.Format(@"DELETE FROM KPDOCW WHERE KEQID = {0}", docId);
                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                    }


                    sql = string.Format(@"DELETE FROM KPDOCLDW WHERE KEMID = {0}", guidId);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            return GetListeDestinatairesDetails(codeDocument);
        }

        public static void DeleteLotDocumentsGestion(Int64 codeLot)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("P_CODELOT", codeLot);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETELOTDOCUMENTSGESTION", param);
        }

        #endregion

        #region Méthodes privées

        #endregion

        #endregion

        #region Documents Joints

        #region Méthodes publiques

        /// <summary>
        /// Récupère la listes des documents joints
        /// pour une offre ou un contrat donné
        /// </summary>
        public static DocumentsJointsDto GetListDocumentsJoints(string codeOffre, string version, string type, string modeNavig, bool isReadOnly, string orderField = "", string orderType = "")
        {
            string orderField1 = string.Empty;
            string orderField2 = string.Empty;

            DocumentsJointsDto model = new DocumentsJointsDto { ListDocuments = new List<DocumentsDto>() };

            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", type);

            string sql = @"SELECT KERID DOCID, PO.PBAVN CODEAVN, PO.PBAVA * 10000 + PO.PBAVM * 100 + PO.PBAVJ DATEAVNCUR, 
                                   KERAVN CODEAVNDOC, HP.PBAVA * 10000 + HP.PBAVM * 100 + HP.PBAVJ DATEAVNHIST, 
                                KERCRD DATEAJOUT, KERACTG ACTECODE, IFNULL(TPLIB, '') ACTELIB, KERLIB TITRE, 
                                   KERNOM FICHIER, KERREF REFERENCE, KERCHM CHEMIN,
                                   (SELECT COUNT(*) FROM KPCLAUSE WHERE KCATXL = DOC.KERID AND KCAIPB = DOC.KERIPB AND KCAALX = DOC.KERALX AND KCATYP = DOC.KERTYP) REFERENCECP
                            FROM KPDOCEXT DOC
                                   LEFT JOIN YYYYPAR ON TCON = 'KHEOP' AND TFAM = 'ACTGS' AND TRIM(TCOD) = TRIM(KERACTG)
                                INNER JOIN YPOBASE PO ON PO.PBIPB = KERIPB AND PO.PBALX = KERALX AND PO.PBTYP = KERTYP
                                LEFT JOIN YHPBASE HP ON HP.PBIPB = KERIPB AND HP.PBALX = KERALX AND HP.PBTYP = KERTYP AND HP.PBHIN = 1 AND HP.PBAVN = KERAVN
                            WHERE KERIPB = :codeOffre AND KERALX = :version AND KERTYP = :type";

            string orderSql = string.Empty;
            switch (orderField)
            {
                case "DateAjout":
                    orderField1 = "KERCRD";
                    orderField2 = "KERCRH";
                    break;
                case "Acte":
                    orderField1 = "KERACTG";
                    break;
                case "Titre":
                    orderField1 = "KERLIB";
                    break;
                default:
                    orderField1 = "KERCRD";
                    orderField2 = "KERCRH";
                    orderType = "DESC";
                    break;
            }

            orderSql = string.Format(" ORDER BY {0} {1}{2}", orderField1, orderType, !string.IsNullOrEmpty(orderField2) ? string.Format(", {0} {1}", orderField2, orderType) : string.Empty);

            sql = sql + orderSql;

            var result = DbBase.Settings.ExecuteList<DocumentsDto>(CommandType.Text, sql, param);

            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.DateAjout = AlbConvert.ConvertIntToDate(item.DateAjt);
                    if (item.RefCp > 0)
                        item.ReferenceCP = true;
                    item.IsReadOnly = isReadOnly;

                    item.DateAvn = item.CodeAvn != item.CodeAvnDoc ? AlbConvert.ConvertIntToDate(Convert.ToInt32(item.DateAvnHist)) : AlbConvert.ConvertIntToDate(Convert.ToInt32(item.DateAvnCur));
                }
                model.ListDocuments = result;
            }

            if (type == AlbConstantesMetiers.TYPE_CONTRAT)
            {
                sql = string.Format(@"SELECT PBETA STRRETURNCOL FROM YPOBASE WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'"
                , codeOffre.PadLeft(9, ' '), version, type);
                var resultEtat = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
                if (resultEtat != null && resultEtat.Any())
                {
                    model.IsValide = resultEtat.FirstOrDefault().StrReturnCol == "V";
                }
            }

            return model;
        }

        /// <summary>
        /// Récupère les informations du document joint en édition
        /// </summary>
        public static DocumentsAddDto OpenEditionDocsJoints(string idDoc)
        {
            DocumentsAddDto model = GetInfoDocJoint(idDoc);

            model.TypesDoc = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "TDOC");

            return model;
        }

        /// <summary>
        /// Sauvegarde le document joint
        /// </summary>
        public static DocumentsJointsDto SaveDocsJoints(string codeOffre, string version, string type, string idDoc, string typeDoc, string titleDoc, string fileDoc, string pathDoc, string refDoc, string user, string modeNavig, bool isReadOnly, string acteGestion)
        {
            DateTime dateNow = DateTime.Now;

            DbParameter[] param = new DbParameter[14];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_IDDOC", 0);
            param[3].Value = !string.IsNullOrEmpty(idDoc) ? Convert.ToInt32(idDoc) : 0;
            param[4] = new EacParameter("P_TYPEDOC", typeDoc);
            param[5] = new EacParameter("P_TITLEDOC", titleDoc);
            param[6] = new EacParameter("P_FILEDOC", fileDoc);
            param[7] = new EacParameter("P_PATHDOC", pathDoc);
            param[8] = new EacParameter("P_REFDOC", refDoc);
            param[9] = new EacParameter("P_USER", user);
            param[10] = new EacParameter("P_DATENOW", 0);
            param[10].Value = AlbConvert.ConvertDateToInt(dateNow);
            param[11] = new EacParameter("P_HOURNOW", 0);
            param[11].Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow));
            param[12] = new EacParameter("P_ACTEGES", acteGestion);
            param[13] = new EacParameter("P_OUTIDDOC", DbType.Int32) { Value = 0 };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEDOCSJOINTS", param);

            return GetListDocumentsJoints(codeOffre, version, type, modeNavig, isReadOnly);
        }

        /// <summary>
        /// Supprime un document joint
        /// </summary>
        public static DocumentsJointsDto DeleteDocsJoints(string idDoc, string codeOffre, string version, string type, string modeNavig, bool isReadOnly)
        {
            //TODO : supprimer le fichier également
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("docId", 0);
            param[0].Value = !string.IsNullOrEmpty(idDoc) ? Convert.ToInt32(idDoc) : 0;

            string sql = @"DELETE FROM KPDOCEXT WHERE KERID = :docId";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return GetListDocumentsJoints(codeOffre, version, type, modeNavig, isReadOnly);
        }

        /// <summary>
        /// Charge le chemin suivant la typologie sélectionnée
        /// </summary>
        public static string ReloadPathFile(string typologie)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("typologie", "DOC_" + typologie);

            string sql = @"SELECT '//' CONCAT TRIM(KHMSRV) CONCAT '/' CONCAT TRIM(KHMRAC) CONCAT '/' CONCAT TRIM(KHMENV) CONCAT TRIM(KHMCHM) CONCAT '/' STRRETURNCOL FROM KCHEMIN WHERE KHMCLE = :typologie";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
                return result.FirstOrDefault().StrReturnCol.Replace('/', '\\');

            return string.Empty;
        }

        #endregion

        #region Méthodes privées

        private static DocumentsAddDto GetInfoDocJoint(string idDoc)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("docId", 0);
            param[0].Value = !string.IsNullOrEmpty(idDoc) ? Convert.ToInt32(idDoc) : 0;

            string sql = @"SELECT KERID DOCID, KERTYPO TYPEDOC, KERLIB TITREDOC, KERNOM FILEDOC, KERCHM PATHDOC, KERREF REFDOC
                                FROM KPDOCEXT
                            WHERE KERID = :docId";

            var result = DbBase.Settings.ExecuteList<DocumentsAddDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault();
            }
            return new DocumentsAddDto();
        }

        #endregion

        #endregion

    }
}
