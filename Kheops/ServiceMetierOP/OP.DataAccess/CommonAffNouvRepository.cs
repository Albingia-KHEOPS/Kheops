using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.AssuresAdditionnels;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.Offres.Parametres;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Common;

namespace OP.DataAccess
{
    public class CommonAffNouvRepository
    {
        #region Méthodes Publiques

        /// <summary>
        /// Initialise l'écran Assurés Additionnels
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static AssuresAdditionnelsDto InitAssuresAdditionnels(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            AssuresAdditionnelsDto model = new AssuresAdditionnelsDto
            {
                AssuresReference = GetListAssuresRef(codeOffre, version, type, codeAvn, modeNavig),
                AssuresNonReference = GetListAssuresNonRef(codeOffre, version, type, codeAvn, modeNavig)
            };

            model.IsAvenantModificationLocale = CommonRepository.ExistTraceAvenant(codeOffre, version, type, "ASSADDREF", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty) ||
                                                CommonRepository.ExistTraceAvenant(codeOffre, version, type, "ASSADDNREF", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            return model;

            //return RemplirAssuresAdditionnels();
        }

        /// <summary>
        /// Récupère les informations de l'assuré référencé
        /// </summary>
        public static AssuresRefDto GetAssureRef(string codeOffre, string version, string type, string codeAssu)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeAssu", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeAssu);

            string sql = @"SELECT PCIAS CODE, ANNOM NOM,  
	                    PCQL1 CODEQUALITE1, PCQL2 CODEQUALITE2, PCQL3 CODEQUALITE3, PCQLD QUALITEAUTRE,
                        PCDESI IDDESI, KADDESI DESIGNATION
                    FROM YPOASSU
	                    INNER JOIN YASSURE ON ASIAS = PCIAS
	                    INNER JOIN YASSNOM ON ANIAS = PCIAS AND ANINL = 0 AND ANTNM = 'A'
                        LEFT JOIN KPDESI ON PCDESI = KADCHR
                    WHERE PCIPB = :codeOffre AND PCALX = :version AND PCTYP = :type AND PCIAS = :codeAssu";

            return DbBase.Settings.ExecuteList<AssuresRefDto>(CommandType.Text, sql, param).FirstOrDefault();
        }

        /// <summary>
        /// Charge les informations de l'assuré 
        /// en fonction du code fournit
        /// </summary>
        /// <param name="codeAssu"></param>
        /// <returns></returns>
        public static string LoadInfoAssure(string codeAssu)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("codeAssu", DbType.Int32);
            param[0].Value = Convert.ToInt32(codeAssu);

            string sql = @"SELECT ANNOM NOM
                    FROM YASSURE 
                        INNER JOIN YASSNOM ON ANIAS = ASIAS AND ANINL = 0
                    WHERE ASIAS = :codeAssu";
            var result = DbBase.Settings.ExecuteList<AssuresRefDto>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0)
                return result.FirstOrDefault().NomAssure;
            return string.Empty;
        }

        /// <summary>
        /// Sauvegarde les informations de l'assuré référencé
        /// </summary>
        /// <param name="modeEdit"></param>
        /// <param name="codeAssure"></param>
        /// <param name="nomAssure"></param>
        /// <param name="codeQualite1"></param>
        /// <param name="codeQualite2"></param>
        /// <param name="codeQualite3"></param>
        /// <param name="qualiteAutre"></param>
        /// <returns></returns>
        public static AssuresAdditionnelsDto SaveAssureRef(string codeOffre, string version, string type, string codeAvenant, string modeEdit, string codeAssure, string codeQualite1, string codeQualite2,
            string codeQualite3, string qualiteAutre, string idDesi, string designation, ModeConsultation modeNavig, string user)
        {
            Int32 codeError = 0;

            if (modeEdit == "0")
                codeError = InsertAssureRef(codeOffre, version, type, codeAvenant, codeAssure, codeQualite1, codeQualite2, codeQualite3, qualiteAutre, !string.IsNullOrEmpty(idDesi) ? Convert.ToInt32(idDesi) : 0, designation, user);
            else
                UpdateAssureRef(codeOffre, version, type, codeAvenant, codeAssure, codeQualite1, codeQualite2, codeQualite3, qualiteAutre, !string.IsNullOrEmpty(idDesi) ? Convert.ToInt32(idDesi) : 0, designation, user);

            AssuresAdditionnelsDto model = new AssuresAdditionnelsDto();
            model.AssuresReference = GetListAssuresRef(codeOffre, version, type, codeAvenant, modeNavig);
            model.CodeError = codeError;

            return model;
        }

        /// <summary>
        /// Supprime les informations de l'assuré référencé
        /// passé en paramètre dans la table YPOASSU
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeAssu"></param>
        /// <returns></returns>
        public static AssuresAdditionnelsDto DeleteAssureRef(string codeOffre, string version, string type, string codeAvenant, string codeAssu, ModeConsultation modeNavig, string user)
        {
            Int64 idDesi = 0;

            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeAssu", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeAssu);

            string sqlSel = @"SELECT PCDESI ID FROM YPOASSU
                        WHERE PCIPB = :codeOffre AND PCALX = :version AND PCTYP = :type AND PCIAS = :codeAssu";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlSel, param);
            if (result != null && result.Any())
            {
                idDesi = result.FirstOrDefault().Id;
            }

            #region Delete Déisgnation

            DbParameter[] paramDesi = new DbParameter[1];
            paramDesi[0] = new EacParameter("idDesi", DbType.Int64);
            paramDesi[0].Value = idDesi;

            string sqlDesi = @"DELETE FROM KPDESI WHERE KADCHR = :idDesi";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDesi, paramDesi);

            #endregion

            string sql = @"DELETE FROM YPOASSU
                        WHERE PCIPB = :codeOffre AND PCALX = :version AND PCTYP = :type AND PCIAS = :codeAssu";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (!string.IsNullOrEmpty(codeAvenant) && codeAvenant != "0")
            {
                param = new EacParameter[3];
                param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("version", DbType.Int32);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[2].Value = type;

                DateTime dateNow = DateTime.Now;
                sql = @"DELETE FROM KPAVTRC WHERE KHOIPB = :codeOffre AND KHOALX = :version AND KHOTYP = :type AND TRIM(KHOPERI) = 'ASSADDREF' AND KHOACT = 'X'";
                //codeOffre, version, type);

                param = new EacParameter[8];
                param[0] = new EacParameter("khoid", DbType.Int32);
                param[0].Value = CommonRepository.GetAS400Id("KHOID");
                param[1] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[1].Value = codeOffre.PadLeft(9, ' ');
                param[2] = new EacParameter("version", DbType.Int32);
                param[2].Value = Convert.ToInt32(version);
                param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[3].Value = type;
                param[4] = new EacParameter("ref", DbType.AnsiStringFixedLength);
                param[4].Value = "ASSADDREF";
                param[5] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[5].Value = user;
                param[6] = new EacParameter("date", DbType.Int32);
                param[6].Value = AlbConvert.ConvertDateToInt(dateNow);
                param[7] = new EacParameter("time", DbType.Int32);
                param[7].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                sql = @"INSERT INTO KPAVTRC
                        (KHOID, KHOIPB, KHOALX, KHOTYP, KHOPERI, KHOETAPE, KHOACT, KHOCRU, KHOCRD, KHOCRH)
                    VALUES
                        (:khoid, :codeoffre, :version, :type, :ref, '**********', 'X', :user, :date, :time)";
                //CommonRepository.GetAS400Id("KHOID"), codeOffre.PadLeft(9, ' '), version, type, "ASSADDREF", user, AlbConvert.ConvertDateToInt(dateNow), AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow)));

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }

            AssuresAdditionnelsDto model = new AssuresAdditionnelsDto();
            model.AssuresReference = GetListAssuresRef(codeOffre, version, type, codeAvenant, modeNavig);

            return model;
        }

        /// <summary>
        /// Sauvegarde les informations de l'assuré non référencé
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="modeEdit"></param>
        /// <param name="codeQualite1"></param>
        /// <param name="codeQualite2"></param>
        /// <param name="codeQualite3"></param>
        /// <param name="qualiteAutre"></param>
        /// <returns></returns>
        public static AssuresAdditionnelsDto SaveAssureNonRef(string codeOffre, string version, string type, string codeAvenant, string modeEdit, string codeQualite1, string codeQualite2, string codeQualite3, string qualiteAutre,
            string codeOldQualite1, string codeOldQualite2, string codeOldQualite3, string oldQualiteAutre, ModeConsultation modeNavig, string user)
        {
            if (modeEdit == "0")
                InsertAssureNonRef(codeOffre, version, type, codeAvenant, codeQualite1, codeQualite2, codeQualite3, qualiteAutre, user);
            else
                UpdateAssureNonRef(codeOffre, version, type, codeAvenant, codeQualite1, codeQualite2, codeQualite3, qualiteAutre, codeOldQualite1, codeOldQualite2, codeOldQualite3, oldQualiteAutre, user);

            AssuresAdditionnelsDto model = new AssuresAdditionnelsDto();
            model.AssuresNonReference = GetListAssuresNonRef(codeOffre, version, type, codeAvenant, modeNavig);

            return model;
        }

        /// <summary>
        /// Supprime les informations de l'assuré non référencé
        /// passé en paramètre dans la table YPOASSX
        /// </summary>
        public static AssuresAdditionnelsDto DeleteAssureNonRef(string codeOffre, string version, string type, string codeAvenant,
            string codeOldQualite1, string codeOldQualite2, string codeOldQualite3, string oldQualiteAutre, ModeConsultation modeNavig, string user)
        {
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeQ1", DbType.AnsiStringFixedLength);
            param[3].Value = !string.IsNullOrEmpty(codeOldQualite1) ? codeOldQualite1.Trim() : string.Empty;
            param[4] = new EacParameter("codeQ2", DbType.AnsiStringFixedLength);
            param[4].Value = !string.IsNullOrEmpty(codeOldQualite2) ? codeOldQualite2.Trim() : string.Empty;
            param[5] = new EacParameter("codeQ3", DbType.AnsiStringFixedLength);
            param[5].Value = !string.IsNullOrEmpty(codeOldQualite3) ? codeOldQualite3.Trim() : string.Empty;
            param[6] = new EacParameter("codeQA", DbType.AnsiStringFixedLength);
            param[6].Value = !string.IsNullOrEmpty(oldQualiteAutre) ? oldQualiteAutre.Trim() : string.Empty;

            string sql = @"DELETE FROM YPOASSX
                                         WHERE PDIPB = :codeoffre AND PDALX = :version AND PDTYP = :type 
                                         AND TRIM(PDQL1) = :codeQ1 AND TRIM(PDQL2) = :codeQ2 AND TRIM(PDQL3) = :codeQ3 
                                         AND TRIM(PDQLD) = :codeQA";
            //codeOffre, 
            //version, 
            //type,
            //!string.IsNullOrEmpty(codeOldQualite1) ? codeOldQualite1 : string.Empty,
            //!string.IsNullOrEmpty(codeOldQualite2) ? codeOldQualite2 : string.Empty,
            //!string.IsNullOrEmpty(codeOldQualite3) ? codeOldQualite3 : string.Empty,
            //!string.IsNullOrEmpty(oldQualiteAutre) ? oldQualiteAutre : string.Empty
            //);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (!string.IsNullOrEmpty(codeAvenant) && codeAvenant != "0")
            {
                param = new EacParameter[3];
                param[0] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("version", DbType.Int32);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[2].Value = type;

                DateTime dateNow = DateTime.Now;
                sql = @"DELETE FROM KPAVTRC WHERE KHOIPB = :codeoffre AND KHOALX = :version AND KHOTYP = :type AND TRIM(KHOPERI) = 'ASSADDNREF' AND KHOACT = 'X'";
                //codeOffre, version, type);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                param = new EacParameter[8];
                param[0] = new EacParameter("khoid", DbType.Int32);
                param[0].Value = CommonRepository.GetAS400Id("KHOID");
                param[1] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[1].Value = codeOffre.PadLeft(9, ' ');
                param[2] = new EacParameter("version", DbType.Int32);
                param[2].Value = Convert.ToInt32(version);
                param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[3].Value = type;
                param[4] = new EacParameter("ref", DbType.AnsiStringFixedLength);
                param[4].Value = "ASSADDNREF";
                param[5] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[5].Value = user;
                param[6] = new EacParameter("date", DbType.Int32);
                param[6].Value = AlbConvert.ConvertDateToInt(dateNow);
                param[7] = new EacParameter("time", DbType.Int32);
                param[7].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));

                sql = @"INSERT INTO KPAVTRC
                        (KHOID, KHOIPB, KHOALX, KHOTYP, KHOPERI, KHOETAPE, KHOACT, KHOCRU, KHOCRD, KHOCRH)
                    VALUES
                        (:khoid, :codeoffre, :version, :type, :ref, '**********', 'X', :user, :date, :time)";
                //CommonRepository.GetAS400Id("KHOID"), codeOffre.PadLeft(9, ' '), version, type, "ASSADDNREF", user, AlbConvert.ConvertDateToInt(dateNow), AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow)));
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }

            AssuresAdditionnelsDto model = new AssuresAdditionnelsDto();
            model.AssuresNonReference = GetListAssuresNonRef(codeOffre, version, type, codeAvenant, modeNavig);

            return model;
        }

        #endregion

        #region Méthodes Privées

        /// <summary>
        /// Récupère la liste des assurés référencés
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<AssuresRefDto> GetListAssuresRef(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var param = new List<EacParameter>{
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(version)),
                new EacParameter("type", type)
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string sql = string.Format(@"SELECT PCIAS CODE, ANNOM NOM, ASDEP CONCAT ASCPO CODEPOSTAL, ASVIL VILLE, 
	                    PCQL1 CODEQUALITE1, IFNULL(PAR1.TPLIB, '') LIBQUALITE1, PCQL2 CODEQUALITE2, IFNULL(PAR2.TPLIB, '') LIBQUALITE2, 
	                    PCQL3 CODEQUALITE3, IFNULL(PAR3.TPLIB, '') LIBQUALITE3, PCQLD QUALITEAUTRE,
                        PBIAS ASSUBASE
                    FROM {0}
                    	INNER JOIN {1} ON PBIPB = PCIPB AND PBALX = PCALX AND PBTYP = PCTYP {6}
	                    INNER JOIN YASSURE ON ASIAS = PCIAS
	                    INNER JOIN YASSNOM ON ANIAS = PCIAS AND ANINL = 0 AND ANTNM = 'A'
                        {2}
                        {3}
                        {4}
                    WHERE PCIPB = :codeOffre AND PCALX = :version AND PCTYP = :type {5}
                    ORDER BY PCPRI DESC",
                   /*0*/     CommonRepository.GetPrefixeHisto(modeNavig, "YPOASSU"),
                   /*1*/     CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                   /*2*/     CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "OJQLT", "PAR1", " AND PAR1.TCOD = PCQL1"),
                   /*3*/     CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "OJQLT", "PAR2", " AND PAR2.TCOD = PCQL2"),
                   /*4*/     CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "OJQLT", "PAR3", " AND PAR3.TCOD = PCQL3"),
                   /*5*/     modeNavig == ModeConsultation.Historique ? " AND PCAVN = :avn" : string.Empty,
                   /*6*/     modeNavig == ModeConsultation.Historique ? " AND PBAVN = PCAVN" : string.Empty);

            return DbBase.Settings.ExecuteList<AssuresRefDto>(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Récupère la liste des assurés non référencés
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static List<AssuresNonRefDto> GetListAssuresNonRef(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var param = new List<EacParameter>{
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(version)),
                new EacParameter("type", type)
            };
            if (modeNavig == ModeConsultation.Historique) {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string sql = string.Format(@"SELECT PDQL1 CODEQUALITE1, IFNULL(PAR1.TPLIB, '') LIBQUALITE1, PDQL2 CODEQUALITE2, IFNULL(PAR2.TPLIB, '') LIBQUALITE2, 
	                    PDQL3 CODEQUALITE3, IFNULL(PAR3.TPLIB, '') LIBQUALITE3, PDQLD QUALITEAUTRE
                    FROM {0}
	                    {1}
                        {2}
                        {3}
                    WHERE PDIPB = :codeOffre AND PDALX = :version AND PDTYP = :type {4}",
                        CommonRepository.GetPrefixeHisto(modeNavig, "YPOASSX"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "OJQLT", "PAR1", " AND PAR1.TCOD = PDQL1"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "OJQLT", "PAR2", " AND PAR2.TCOD = PDQL2"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "OJQLT", "PAR3", " AND PAR3.TCOD = PDQL3"),
                        modeNavig == ModeConsultation.Historique ? " AND PDAVN = :avn" : string.Empty);

            return DbBase.Settings.ExecuteList<AssuresNonRefDto>(CommandType.Text, sql, param); ;
        }

        /// <summary>
        /// Insère en BDD le nouvel assuré référencé
        /// </summary>
        private static Int32 InsertAssureRef(string codeOffre, string version, string type, string codeAvenant, string codeAssure, string codeQualite1, string codeQualite2,
            string codeQualite3, string qualiteAutre, Int32 idDesi, string designation, string user)
        {
            bool existData = CommonRepository.ExistRow(string.Format(@"SELECT COUNT(*) NBLIGN 
                                                                            FROM YPOASSU 
                                                                            WHERE PCIPB = '{0}' AND PCALX = {1} AND PCTYP = '{2}' AND PCIAS = {3}",
                                                            codeOffre.PadLeft(9, ' '), Convert.ToInt32(version), type, Convert.ToInt32(codeAssure)));
            if (existData)
                return 1;

            if (idDesi == 0)
            {
                idDesi = CommonRepository.GetAS400Id("KADCHR");

                EacParameter[] paramDesi = new EacParameter[8];
                paramDesi[0] = new EacParameter("KADCHR", DbType.Int32);
                paramDesi[0].Value = idDesi;
                paramDesi[1] = new EacParameter("KADTYP", DbType.AnsiStringFixedLength);
                paramDesi[1].Value = type;
                paramDesi[2] = new EacParameter("KADIPB", DbType.AnsiStringFixedLength);
                paramDesi[2].Value = codeOffre.PadLeft(9, ' ');
                paramDesi[3] = new EacParameter("KADALX", DbType.Int32);
                paramDesi[3].Value = Convert.ToInt32(version);
                paramDesi[4] = new EacParameter("KADPERI", DbType.AnsiStringFixedLength);
                paramDesi[4].Value = string.Empty;
                paramDesi[5] = new EacParameter("KADRSQ", DbType.Int32);
                paramDesi[5].Value = 0;
                paramDesi[6] = new EacParameter("KADOBJ", DbType.Int32);
                paramDesi[6].Value = 0;
                paramDesi[7] = new EacParameter("KADDESI", DbType.AnsiStringFixedLength);
                paramDesi[7].Value = designation;

                string sqlDesi = @"INSERT INTO KPDESI
                                        (KADCHR, KADTYP, KADIPB, KADALX, KADPERI, KADRSQ, KADOBJ, KADDESI)
                                    VALUES
                                        (:KADCHR, :KADTYP, :KADIPB, :KADALX, :KADPERI, :KADRSQ, :KADOBJ, :KADDESI)";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDesi, paramDesi);
            }

            EacParameter[] param = new EacParameter[13];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeAssure", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeAssure);
            param[4] = new EacParameter("souscPrinc", DbType.AnsiStringFixedLength);
            param[4].Value = "N";
            param[5] = new EacParameter("codeQualite1", DbType.AnsiStringFixedLength);
            param[5].Value = codeQualite1;
            param[6] = new EacParameter("codeQualite2", DbType.AnsiStringFixedLength);
            param[6].Value = codeQualite2;
            param[7] = new EacParameter("codeQualite3", DbType.AnsiStringFixedLength);
            param[7].Value = codeQualite3;
            param[8] = new EacParameter("qualiteAutre", DbType.AnsiStringFixedLength);
            param[8].Value = qualiteAutre;
            param[9] = new EacParameter("cnr", DbType.AnsiStringFixedLength);
            param[9].Value = "N";
            param[10] = new EacParameter("assure", DbType.AnsiStringFixedLength);
            param[10].Value = "O";
            param[11] = new EacParameter("souscripteur", DbType.AnsiStringFixedLength);
            param[11].Value = "N";
            param[12] = new EacParameter("idDesi", DbType.Int32);
            param[12].Value = idDesi;

            string sql = @"INSERT INTO YPOASSU
                        (PCIPB, PCALX, PCTYP, PCIAS, PCPRI, PCQL1, PCQL2, PCQL3, PCQLD, PCCNR, PCASS, PCSCP, PCDESI)
                    VALUES
                        (:codeOffre, :version, :type, :codeAssure, :souscPrinc, :codeQualite1, :codeQualite2, :codeQualite3, :qualiteAutre, :cnr, :assure, :souscripteur, :idDesi)";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (!string.IsNullOrEmpty(codeAvenant) && codeAvenant != "0")
            {
                param = new EacParameter[3];
                param[0] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("version", DbType.Int32);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[2].Value = type;

                DateTime dateNow = DateTime.Now;
                sql = @"DELETE FROM KPAVTRC WHERE KHOIPB = :codeoffre AND KHOALX = :version AND KHOTYP = :type AND TRIM(KHOPERI) = 'ASSADDREF' AND KHOACT = 'C'";
                //codeOffre, version, type);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                param = new EacParameter[8];
                param[0] = new EacParameter("khoid", DbType.Int32);
                param[0].Value = CommonRepository.GetAS400Id("KHOID");
                param[1] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[1].Value = codeOffre.PadLeft(9, ' ');
                param[2] = new EacParameter("version", DbType.Int32);
                param[2].Value = Convert.ToInt32(version);
                param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[3].Value = type;
                param[4] = new EacParameter("ref", DbType.AnsiStringFixedLength);
                param[4].Value = "ASSADDREF";
                param[5] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[5].Value = user;
                param[6] = new EacParameter("date", DbType.Int32);
                param[6].Value = AlbConvert.ConvertDateToInt(dateNow);
                param[7] = new EacParameter("time", DbType.Int32);
                param[7].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));

                sql = @"INSERT INTO KPAVTRC
                        (KHOID, KHOIPB, KHOALX, KHOTYP, KHOPERI, KHOETAPE, KHOACT, KHOCRU, KHOCRD, KHOCRH)
                    VALUES
                        (:khoid, :codeoffre, :version, :type, :ref, '**********', 'C', :user, :date, :time)";
                //CommonRepository.GetAS400Id("KHOID"), codeOffre.PadLeft(9, ' '), version, type, "ASSADDREF", user, AlbConvert.ConvertDateToInt(dateNow), AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow)));
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            return 0;
        }

        /// <summary>
        /// Met à jour en BDD l'assuré référencé
        /// </summary>
        private static void UpdateAssureRef(string codeOffre, string version, string type, string codeAvenant, string codeAssure, string codeQualite1, string codeQualite2,
            string codeQualite3, string qualiteAutre, Int32 idDesi, string designation, string user)
        {
            if (idDesi == 0)
            {
                idDesi = CommonRepository.GetAS400Id("KADCHR");

                EacParameter[] paramDesi = new EacParameter[8];
                paramDesi[0] = new EacParameter("KADCHR", DbType.Int32);
                paramDesi[0].Value = idDesi;
                paramDesi[1] = new EacParameter("KADTYP", DbType.AnsiStringFixedLength);
                paramDesi[1].Value = type;
                paramDesi[2] = new EacParameter("KADIPB", DbType.AnsiStringFixedLength);
                paramDesi[2].Value = codeOffre.PadLeft(9, ' ');
                paramDesi[3] = new EacParameter("KADALX", DbType.Int32);
                paramDesi[3].Value = Convert.ToInt32(version);
                paramDesi[4] = new EacParameter("KADPERI", DbType.AnsiStringFixedLength);
                paramDesi[4].Value = string.Empty;
                paramDesi[5] = new EacParameter("KADRSQ", DbType.Int32);
                paramDesi[5].Value = 0;
                paramDesi[6] = new EacParameter("KADOBJ", DbType.Int32);
                paramDesi[6].Value = 0;
                paramDesi[7] = new EacParameter("KADDESI", DbType.AnsiStringFixedLength);
                paramDesi[7].Value = designation;

                string sqlDesi = @"INSERT INTO KPDESI
                                        (KADCHR, KADTYP, KADIPB, KADALX, KADPERI, KADRSQ, KADOBJ, KADDESI)
                                    VALUES
                                        (:KADCHR, :KADTYP, :KADIPB, :KADALX, :KADPERI, :KADRSQ, :KADOBJ, :KADDESI)";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDesi, paramDesi);
            }
            else
            {
                EacParameter[] paramDesi = new EacParameter[2];
                paramDesi[0] = new EacParameter("KADDESI", DbType.AnsiStringFixedLength);
                paramDesi[0].Value = designation;
                paramDesi[1] = new EacParameter("KADCHR", DbType.Int32);
                paramDesi[1].Value = idDesi;

                string sqlDesi = @"UPDATE KPDESI SET KADDESI = :KADDESI WHERE KADCHR = :KADCHR;";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDesi, paramDesi);
            }

            EacParameter[] param = new EacParameter[9];
            param[0] = new EacParameter("codeQualite1", DbType.AnsiStringFixedLength);
            param[0].Value = codeQualite1;
            param[1] = new EacParameter("codeQualite2", DbType.AnsiStringFixedLength);
            param[1].Value = codeQualite2;
            param[2] = new EacParameter("codeQualite3", DbType.AnsiStringFixedLength);
            param[2].Value = codeQualite3;
            param[3] = new EacParameter("qualiteAutre", DbType.AnsiStringFixedLength);
            param[3].Value = qualiteAutre;
            param[4] = new EacParameter("idDesi", DbType.Int32);
            param[4].Value = idDesi;
            param[5] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[5].Value = codeOffre.PadLeft(9, ' ');
            param[6] = new EacParameter("version", DbType.Int32);
            param[6].Value = Convert.ToInt32(version);
            param[7] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[7].Value = type;
            param[8] = new EacParameter("codeAssure", DbType.Int32);
            param[8].Value = Convert.ToInt32(codeAssure);

            string sql = @"UPDATE YPOASSU
                        SET PCQL1 = :codeQualite1, PCQL2 = :codeQualite2, PCQL3 = :codeQualite3, PCQLD = :qualiteAutre, PCDESI = :idDesi
                        WHERE PCIPB = :codeOffre AND PCALX = :version AND PCTYP = :type AND PCIAS = :codeAssure";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (!string.IsNullOrEmpty(codeAvenant) && codeAvenant != "0")
            {
                param = new EacParameter[3];
                param[0] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("version", DbType.Int32);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[2].Value = type;

                DateTime dateNow = DateTime.Now;
                sql = @"DELETE FROM KPAVTRC WHERE KHOIPB = :codeoffre AND KHOALX = :version AND KHOTYP = :type AND TRIM(KHOPERI) = 'ASSADDREF' AND KHOACT = 'M'";
                //codeOffre, version, type);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                param = new EacParameter[8];
                param[0] = new EacParameter("khoid", DbType.Int32);
                param[0].Value = CommonRepository.GetAS400Id("KHOID");
                param[1] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[1].Value = codeOffre.PadLeft(9, ' ');
                param[2] = new EacParameter("version", DbType.Int32);
                param[2].Value = Convert.ToInt32(version);
                param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[3].Value = type;
                param[4] = new EacParameter("ref", DbType.AnsiStringFixedLength);
                param[4].Value = "ASSADDREF";
                param[5] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[5].Value = user;
                param[6] = new EacParameter("date", DbType.Int32);
                param[6].Value = AlbConvert.ConvertDateToInt(dateNow);
                param[7] = new EacParameter("time", DbType.Int32);
                param[7].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));

                sql = @"INSERT INTO KPAVTRC
                        (KHOID, KHOIPB, KHOALX, KHOTYP, KHOPERI, KHOETAPE, KHOACT, KHOCRU, KHOCRD, KHOCRH)
                    VALUES
                        (:khoid, :codeoffre, :version, :type, :ref, '**********', 'M', :user, :date, :time)";
                //CommonRepository.GetAS400Id("KHOID"), codeOffre.PadLeft(9, ' '), version, type, "ASSADDREF", user, AlbConvert.ConvertDateToInt(dateNow), AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow)));
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
        }

        /// <summary>
        /// Insère en BDD le nouvel assuré non référencé
        /// </summary>
        private static void InsertAssureNonRef(string codeOffre, string version, string type, string codeAvenant, string codeQualite1, string codeQualite2,
            string codeQualite3, string qualiteAutre, string user)
        {
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeQualite1", DbType.AnsiStringFixedLength);
            param[3].Value = codeQualite1;
            param[4] = new EacParameter("codeQualite2", DbType.AnsiStringFixedLength);
            param[4].Value = codeQualite2;
            param[5] = new EacParameter("codeQualite3", DbType.AnsiStringFixedLength);
            param[5].Value = codeQualite3;
            param[6] = new EacParameter("qualiteAutre", DbType.AnsiStringFixedLength);
            param[6].Value = qualiteAutre;

            string sql = @"INSERT INTO YPOASSX
                        (PDIPB, PDALX, PDTYP, PDQL1, PDQL2, PDQL3, PDQLD)
                    VALUES
                        (:codeOffre, :version, :type, :codeQualite1, :codeQualite2, :codeQualite3, :qualiteAutre)";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (!string.IsNullOrEmpty(codeAvenant) && codeAvenant != "0")
            {
                param = new EacParameter[3];
                param[0] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("version", DbType.Int32);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[2].Value = type;

                DateTime dateNow = DateTime.Now;
                sql = string.Format(@"DELETE FROM KPAVTRC WHERE KHOIPB = :codeoffre AND KHOALX = :version AND KHOTYP = :type AND TRIM(KHOPERI) = 'ASSADDNREF' AND KHOACT = 'C'",
                            codeOffre, version, type);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                param = new EacParameter[8];
                param[0] = new EacParameter("khoid", DbType.Int32);
                param[0].Value = CommonRepository.GetAS400Id("KHOID");
                param[1] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[1].Value = codeOffre.PadLeft(9, ' ');
                param[2] = new EacParameter("version", DbType.Int32);
                param[2].Value = Convert.ToInt32(version);
                param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[3].Value = type;
                param[4] = new EacParameter("ref", DbType.AnsiStringFixedLength);
                param[4].Value = "ASSADDNREF";
                param[5] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[5].Value = user;
                param[6] = new EacParameter("date", DbType.Int32);
                param[6].Value = AlbConvert.ConvertDateToInt(dateNow);
                param[7] = new EacParameter("time", DbType.Int32);
                param[7].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));

                sql = string.Format(@"INSERT INTO KPAVTRC
                        (KHOID, KHOIPB, KHOALX, KHOTYP, KHOPERI, KHOETAPE, KHOACT, KHOCRU, KHOCRD, KHOCRH)
                    VALUES
                        (:khoid, :codeoffre, :version, :type, :ref, '**********', 'C', :user, :date, :time)",
                        CommonRepository.GetAS400Id("KHOID"), codeOffre.PadLeft(9, ' '), version, type, "ASSADDNREF", user, AlbConvert.ConvertDateToInt(dateNow), AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow)));
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }

        }

        /// <summary>
        /// Met à jour en BDD l'assuré non référencé
        /// </summary>
        private static void UpdateAssureNonRef(string codeOffre, string version, string type, string codeAvenant, string codeQualite1, string codeQualite2,
            string codeQualite3, string qualiteAutre, string codeOldQualite1, string codeOldQualite2, string codeOldQualite3, string oldQualiteAutre, string user)
        {
            EacParameter[] param = new EacParameter[11];
            param[0] = new EacParameter("codeQualite1", DbType.AnsiStringFixedLength);
            param[0].Value = codeQualite1;
            param[1] = new EacParameter("codeQualite2", DbType.AnsiStringFixedLength);
            param[1].Value = codeQualite2;
            param[2] = new EacParameter("codeQualite3", DbType.AnsiStringFixedLength);
            param[2].Value = codeQualite3;
            param[3] = new EacParameter("qualiteAutre", DbType.AnsiStringFixedLength);
            param[3].Value = qualiteAutre;
            param[4] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[4].Value = codeOffre.PadLeft(9, ' ');
            param[5] = new EacParameter("version", DbType.Int32);
            param[5].Value = Convert.ToInt32(version);
            param[6] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[6].Value = type;
            param[7] = new EacParameter("codeOldQualite1", DbType.AnsiStringFixedLength);
            param[7].Value = codeOldQualite1;
            param[8] = new EacParameter("codeOldQualite2", DbType.AnsiStringFixedLength);
            param[8].Value = codeOldQualite2;
            param[9] = new EacParameter("codeOldQualite3", DbType.AnsiStringFixedLength);
            param[9].Value = codeOldQualite3;
            param[10] = new EacParameter("oldQualiteAutre", DbType.AnsiStringFixedLength);
            param[10].Value = oldQualiteAutre;


            string sql = @"UPDATE YPOASSX
                        SET PDQL1 = :codeQualite1, PDQL2 = :codeQualite2, PDQL3 = :codeQualite3, PDQLD = :qualiteAutre
                        WHERE PDIPB = :codeOffre AND PDALX = :version AND PDTYP = :type AND PDQL1 = :codeOldQualite1 AND PDQL2 = :codeOldQualite2 AND PDQL3 = :codeOldQualite3 AND PDQLD = :oldQualiteAutre";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (!string.IsNullOrEmpty(codeAvenant) && codeAvenant != "0")
            {
                param = new EacParameter[3];
                param[0] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("version", DbType.Int32);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[2].Value = type;

                DateTime dateNow = DateTime.Now;
                sql = string.Format(@"DELETE FROM KPAVTRC WHERE KHOIPB = :codeoffre AND KHOALX = :version AND KHOTYP = :type AND TRIM(KHOPERI) = 'ASSADDNREF' AND KHOACT = 'M'");

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                param = new EacParameter[8];
                param[0] = new EacParameter("khoid", DbType.Int32);
                param[0].Value = CommonRepository.GetAS400Id("KHOID");
                param[1] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                param[1].Value = codeOffre.PadLeft(9, ' ');
                param[2] = new EacParameter("version", DbType.Int32);
                param[2].Value = Convert.ToInt32(version);
                param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[3].Value = type;
                param[4] = new EacParameter("ref", DbType.AnsiStringFixedLength);
                param[4].Value = "ASSADDNREF";
                param[5] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[5].Value = user;
                param[6] = new EacParameter("date", DbType.Int32);
                param[6].Value = AlbConvert.ConvertDateToInt(dateNow);
                param[7] = new EacParameter("time", DbType.Int32);
                param[7].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));

                sql = @"INSERT INTO KPAVTRC
                        (KHOID, KHOIPB, KHOALX, KHOTYP, KHOPERI, KHOETAPE, KHOACT, KHOCRU, KHOCRD, KHOCRH)
                    VALUES
                        (:khoid, :codeoffre, :version, :type, :ref, '**********', 'M', :user, :date, :time)";
                //CommonRepository.GetAS400Id("KHOID"), codeOffre.PadLeft(9, ' '), version, type, "ASSADDNREF", user, AlbConvert.ConvertDateToInt(dateNow), AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow)));
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
        }

        private static AssuresAdditionnelsDto RemplirAssuresAdditionnels()
        {
            List<AssuresRefDto> assuresRef = new List<AssuresRefDto>();
            AssuresRefDto assureRef = new AssuresRefDto
            {
                CodeAssure = 589,
                NomAssure = "Marseille Provence",
                CodePostal = "13000",
                Ville = "Marseille",
                CodeQualite1 = "MOA",
                LibQualite1 = "MOA"
            };
            assuresRef.Add(assureRef);
            assureRef = new AssuresRefDto
            {
                CodeAssure = 24116,
                NomAssure = "SEDIM",
                CodePostal = "13000",
                Ville = "Marseille",
                CodeQualite1 = "MOA",
                LibQualite1 = "MOA",
                QualiteAutre = "AZE"
            };
            assuresRef.Add(assureRef);
            List<AssuresNonRefDto> assuresNonRef = new List<AssuresNonRefDto>();
            AssuresNonRefDto assureNonRef = new AssuresNonRefDto
            {
                CodeQualite1 = "MOA",
                LibQualite1 = "MOA",
                QualiteAutre = "IOP"
            };
            assuresNonRef.Add(assureNonRef);

            AssuresAdditionnelsDto model = new AssuresAdditionnelsDto
            {
                AssuresReference = assuresRef,
                AssuresNonReference = assuresNonRef
            };

            return model;

        }

        #endregion
    }
}
