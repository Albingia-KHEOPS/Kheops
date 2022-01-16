using System.Collections.Generic;
using System.Runtime.Caching;
using DataAccess.Helpers;
using System.Data;
using System;
using OP.WSAS400.DTO.Volet;
using System.Data.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using System.Linq;


namespace OP.DataAccess
{
    public class VoletRepository
    {
        #region Méthode Publique

        public static List<DtoVolet> RechercherVoletsByCible(string codeCible, string codeBranche)
        {
            var param = new EacParameter[2];

            param[0] = new EacParameter("codeCible", DbType.AnsiStringFixedLength);
            param[0].Value = codeCible;

            param[1] = new EacParameter("codeBranche", DbType.AnsiStringFixedLength);
            param[1].Value = codeBranche;

            string sql = @"SELECT KAPID GUIDID, KAPVOLET CODE, KAKDESC DESCRIPTION, KAPCAR CARACTERE, KAPORDRE NUMORDRE, KAKFGEN ISVOLETGENERAL
                        FROM KVOLET
                            INNER JOIN KCATVOLET ON KAKVOLET = KAPVOLET
                        WHERE KAPKAIID = :codeCible AND KAPBRA = :codeBranche
                        ORDER BY KAPORDRE";

            return DbBase.Settings.ExecuteList<DtoVolet>(CommandType.Text, sql, param);
        }

        public static void EnregistrerVoletByCible(string codeId, string codeBranche, string codeCible, string codeIdCible, string codeVolet, string codeIdVolet, string codeCaractere, double ordreVolet, string user)
        {
            int idVolet = 0;
            int transformedOrderVolet = AlbConvert.ConvertDoubleToFirstGreaterInt(ordreVolet);
            if (!string.IsNullOrEmpty(codeId))
            {
                int.TryParse(codeId, out idVolet);
                UpdateVoletByCible(codeId, codeVolet, codeCaractere, user);
            }
            else
                idVolet = InsertVoletByCible(codeBranche, codeCible, codeIdCible, codeVolet, codeIdVolet, codeCaractere, user);
            //Mise à jour des numéros d'ordre
            //RenumVolets(codeBranche, codeIdCible);
            RenumVolets(codeIdCible, idVolet, transformedOrderVolet, ((int)ordreVolet != transformedOrderVolet) ? 1 : 0);
        }


        //public static void RenumVolets(string codeBranche, string codeIdCible)
        //{
        //    DbParameter[] param = new DbParameter[2];
        //    param[0] = new EacParameter("codeBranche", codeBranche);
        //    param[1] = new EacParameter("codeIdCible", codeIdCible);
        //    DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_RENUMVOLET", param);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cibleId"></param>
        /// <param name="voletId"></param>
        /// <param name="ordre"></param>
        /// <param name="isOrderTransformed"></param>
        public static void RenumVolets(string cibleId, int voletId, int ordre, int isOrderTransformed)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_IDCIBLE", DbType.AnsiStringFixedLength);
            param[0].Value = cibleId;
            param[1] = new EacParameter("P_IDVOLET", DbType.Int32);
            param[1].Value = voletId;
            param[2] = new EacParameter("P_ORDRE", DbType.Int32);
            param[2].Value = ordre;
            param[3] = new EacParameter("P_IS_TRANSFORMED", DbType.Int32);
            param[3].Value = isOrderTransformed;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_RENUMVOLET", param);
        }
        
        /// <summary>
        /// Récupère les volets
        /// </summary>
        public static List<DtoVolet> GetVolets(string code, string description)
        {
            List<EacParameter> parameters = new List<EacParameter> {
                    new EacParameter("desc", DbType.AnsiStringFixedLength) { Value = $"%{description.ToUpper()}%" }

            };

            string sql = @"SELECT KAKID GUIDID, 
                                                KAKVOLET CODE, 
                                                KAKDESC DESCRIPTION, 
                                                KAKFGEN ISVOLETGENERAL
                                         FROM KVOLET
                                         WHERE UPPER(KAKDESC) LIKE :desc";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND UPPER(KAKVOLET) LIKE :code";
                parameters.Add(new EacParameter("code", DbType.AnsiStringFixedLength) { Value = $"%{code.ToUpper()}%" });
            }
            sql += " ORDER BY KAKVOLET";
            return DbBase.Settings.ExecuteList<DtoVolet>(CommandType.Text, sql, parameters);
        }


        /// <summary>
        /// Obtenirs the volets.
        /// </summary>
        /// <returns></returns>
        public static List<DtoVolet> ObtenirVolets()
        {
            string sql = @"SELECT KAKVOLET CODE, 
                                  KAKDESC DESCRIPTION
                           FROM KVOLET
                           ORDER BY KAKVOLET";
            return DbBase.Settings.ExecuteList<DtoVolet>(CommandType.Text, sql);
        }

        /// <summary>
        /// Obtenir les volets.
        /// </summary>
        public static DtoVolet ObtenirVolet(string code)
        {
            var param = new EacParameter[1];

            param[0] = new EacParameter("code", DbType.AnsiStringFixedLength);
            param[0].Value = code.ToUpper();

            DtoVolet toReturn = null;
            string sql = @"SELECT KAKID GUIDID, 
                                                KAKVOLET CODE, 
                                                KAKDESC DESCRIPTION, 
                                                KAKCRD DATECREATION, 
                                                KAKCRH HEURECREATION, 
                                                KAKBRA BRANCHE,
                                                KAKFGEN ISVOLETGENERAL,
                                                KAKPRES ISVOLETCOLLAPSE 
                                         FROM KVOLET 
                                         WHERE KAKID = :code";

            var result = DbBase.Settings.ExecuteList<DtoVolet>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                toReturn = result.FirstOrDefault();
            }

            if (toReturn != null)
            {
                DateTime? date = AlbConvert.ConvertIntToDate(toReturn.iDateCreation);
                TimeSpan? time = AlbConvert.ConvertIntToTime(toReturn.iHeureCreation);

                if (date != null)
                {
                    if (time != null)
                        toReturn.DateCreation = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, time.Value.Hours, time.Value.Minutes, time.Value.Seconds);
                    else
                        toReturn.DateCreation = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0);
                }

                if (string.IsNullOrEmpty(toReturn.IsVoletGeneral))
                    toReturn.IsVoletGeneral = "N";
            }
            return toReturn;
        }

        /// <summary>
        /// Enregistre le volet.
        /// </summary>
        public static void EnregistrerVolet(string codeId, string code, string description, string branche, string isVoletGeneral, string isVoletCollapse, string update, string user)
        {
            EacParameter[] param = null;
            string sql = string.Empty;
            DateTime date = DateTime.Now;
            if (!string.IsNullOrEmpty(codeId))
            {
                #region Paramètres UPDATE

                param = new EacParameter[8];
                param[0] = new EacParameter("descr", DbType.AnsiStringFixedLength);
                param[0].Value = description;
                param[1] = new EacParameter("dateMaj", DbType.Int32);
                param[1].Value = AlbConvert.ConvertDateToInt(date);
                param[2] = new EacParameter("heureMaj", DbType.Int32);
                param[2].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
                param[3] = new EacParameter("userMaj", DbType.AnsiStringFixedLength);
                param[3].Value = user;
                param[4] = new EacParameter("branche", DbType.AnsiStringFixedLength);
                param[4].Value = branche;
                param[5] = new EacParameter("isVoletGeneral", DbType.AnsiStringFixedLength);
                param[5].Value = isVoletGeneral;
                param[6] = new EacParameter("isVoletCollapse", DbType.AnsiStringFixedLength);
                param[6].Value = isVoletCollapse;
                param[7] = new EacParameter("idVolet", DbType.Int32);
                param[7].Value = Convert.ToInt32(codeId);

                #endregion

                sql = @"UPDATE KVOLET 
                            SET KAKDESC = :descr, KAKMAJD = :dateMaj, KAKMAJH = :heureMaj, KAKMAJU = :userMaj, KAKBRA = :branche, KAKFGEN = :isVoletGeneral, KAKPRES = :isVoletCollapse 
                        WHERE KAKID = :idVolet";
            }
            else
            {
                param = new EacParameter[11];
                param[0] = new EacParameter("AS400Id", DbType.Int32);
                param[0].Value = CommonRepository.GetAS400Id("KAKID");
                param[1] = new EacParameter("code", DbType.AnsiStringFixedLength);
                param[1].Value = code.ToUpper();
                param[2] = new EacParameter("description", DbType.AnsiStringFixedLength);
                param[2].Value = description;
                param[3] = new EacParameter("date", DbType.Int32);
                param[3].Value = AlbConvert.ConvertDateToInt(date);
                param[4] = new EacParameter("time", DbType.Int32);
                param[4].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
                param[5] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[5].Value = user;
                param[6] = new EacParameter("date2", DbType.Int32);
                param[6].Value = AlbConvert.ConvertDateToInt(date);
                param[7] = new EacParameter("time2", DbType.Int32);
                param[7].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
                param[8] = new EacParameter("user2", DbType.AnsiStringFixedLength);
                param[8].Value = user;
                param[9] = new EacParameter("branche", DbType.AnsiStringFixedLength);
                param[9].Value = branche;
                param[10] = new EacParameter("isVoletGeneral", DbType.AnsiStringFixedLength);
                param[10].Value = isVoletGeneral;


                sql = @"INSERT INTO KVOLET (KAKID, KAKVOLET, KAKDESC, KAKCRD, KAKCRH, 
                                            KAKCRU, KAKMAJD, KAKMAJH, KAKMAJU, KAKBRA, KAKFGEN ) 
                        VALUES (:AS400Id, :code, :description, :date, :time, :user, :date2, :time2, :user2, :branche, :isVoletGeneral)";

            }
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Supprimers the volet.
        /// </summary>
        /// <param name="codeId">The code id.</param>
        public static string SupprimerVolet(string codeId, string infoUser)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_PARAM", DbType.AnsiStringFixedLength);
            param[0].Value = "VOLET";
            param[1] = new EacParameter("P_CODEPARAM", DbType.Int32);
            param[1].Value = Convert.ToInt32(codeId);
            param[2] = new EacParameter("P_INFOUSER", DbType.AnsiStringFixedLength);
            param[2].Value = infoUser;
            param[3] = new EacParameter("P_ERRORMSG", DbType.AnsiStringFixedLength);
            param[3].Value = string.Empty;
            param[3].Direction = ParameterDirection.InputOutput;
            param[3].Size = 100;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEPARAMASSOC", param);

            return param[3].Value.ToString();
        }

        /// <summary>
        /// Enregistrers the volet by categorie.
        /// </summary>
        /// <param name="guidId">The GUID id.</param>
        /// <param name="codeBranche">The code branche.</param>
        /// <param name="codeCategorie">The code categorie.</param>
        /// <param name="codeVolet">The code volet.</param>
        /// <param name="codeCaractere">The code caractere.</param>
        /// <param name="user">The user.</param>
        public static void EnregistrerVoletByCategorie(string codeId, string codeBranche, string codeCategorie, string codeIdCategorie, string codeVolet, string codeIdVolet, string codeCaractere, string user)
        {
            string sql = string.Empty;
            EacParameter[] param = null;
            DateTime date = DateTime.Now;

            if (!string.IsNullOrEmpty(codeId))
            {
                param = new EacParameter[6];
                param[0] = new EacParameter("codeVolet", DbType.AnsiStringFixedLength);
                param[0].Value = codeVolet;
                param[1] = new EacParameter("codeCaractere", DbType.AnsiStringFixedLength);
                param[1].Value = codeCaractere;
                param[2] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[2].Value = user;
                param[3] = new EacParameter("date", DbType.Int32);
                param[3].Value = AlbConvert.ConvertDateToInt(date);
                param[4] = new EacParameter("time", DbType.Int32);
                param[4].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
                param[5] = new EacParameter("codeId", DbType.AnsiStringFixedLength);
                param[5].Value = codeId;
                sql = @"UPDATE KCATVOLET 
                                        SET KAPVOLET = :codeVolet, 
                                            KAPCAR = :codeCaractere, 
                                            KAPMAJU = :user, 
                                            KAPMAJD = :date , 
                                            KAPMAJH = :time
                                      WHERE KAPID = :codeId";
            }
            else
            {
                param = new EacParameter[13];
                param[0] = new EacParameter("AS400Id", DbType.Int32);
                param[0].Value = CommonRepository.GetAS400Id("KAPID");
                param[1] = new EacParameter("codeBranche", DbType.AnsiStringFixedLength);
                param[1].Value = codeBranche;
                param[2] = new EacParameter("codeCategorie", DbType.AnsiStringFixedLength);
                param[2].Value = codeCategorie;
                param[3] = new EacParameter("codeIdCategorie", DbType.AnsiStringFixedLength);
                param[3].Value = codeIdCategorie;
                param[4] = new EacParameter("codeVolet", DbType.AnsiStringFixedLength);
                param[4].Value = codeVolet;
                param[5] = new EacParameter("guidVolet", DbType.AnsiStringFixedLength);
                param[5].Value = ObtenirGuidVolet(codeVolet);
                param[6] = new EacParameter("codeCaractere", DbType.AnsiStringFixedLength);
                param[6].Value = codeCaractere;
                param[7] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[7].Value = user;
                param[8] = new EacParameter("date", DbType.Int32);
                param[8].Value = AlbConvert.ConvertDateToInt(date);
                param[9] = new EacParameter("time", DbType.Int32);
                param[9].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
                param[10] = new EacParameter("user2", DbType.AnsiStringFixedLength);
                param[10].Value = user;
                param[11] = new EacParameter("date2", DbType.Int32);
                param[11].Value = AlbConvert.ConvertDateToInt(date);
                param[12] = new EacParameter("time2", DbType.Int32);
                param[12].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));

                sql = @"INSERT INTO KCATVOLET
                                            (KAPID, KAPBRA, KAPCAT, KAPKAMID, KAPVOLET, 
                                             KAPKAKID, KAPCAR, KAPCRU, KAPCRD, KAPCRH, 
                                             KAPMAJU, KAPMAJD, KAPMAJH)
                                     VALUES
                                            (:AS400Id, :codeBranche, :codeCategorie, :codeIdCategorie, :codeVolet, :guidVolet, :codeCaractere, 
                                             :user, :date , :time, :user2, :date2, :time2)";
            }

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Supprimers the volet by categorie.
        /// </summary>
        /// <param name="guidId">The GUID id.</param>
        public static string SupprimerVoletByCategorie(string codeId, string codeBranche, string codeIdCible, string infoUser)
        {
            int idVolet = Convert.ToInt32(codeId);
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_PARAM", DbType.AnsiStringFixedLength);
            param[0].Value = "CATVOLET";
            param[1] = new EacParameter("P_CODEPARAM", DbType.Int32);
            param[1].Value = idVolet;
            param[2] = new EacParameter("P_INFOUSER", DbType.AnsiStringFixedLength);
            param[2].Value = infoUser;
            param[3] = new EacParameter("P_ERRORMSG", DbType.AnsiStringFixedLength);
            param[3].Value = string.Empty;
            param[3].Direction = ParameterDirection.InputOutput;
            param[3].Size = 100;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEPARAMASSOC", param);

            //RenumVolets(codeIdCible,idVolet,0);
            return param[3].Value.ToString();
        }

        #endregion

        #region Méthode Privée

        private static void UpdateVoletByCible(string codeId, string codeVolet, string codeCaractere, string user)
        {
            DateTime date = DateTime.Now;
            var param = new EacParameter[7];
            param[0] = new EacParameter("codeVolet", DbType.AnsiStringFixedLength);
            param[0].Value = codeVolet;
            param[1] = new EacParameter("guidVolet", DbType.AnsiStringFixedLength);
            param[1].Value = GetGuidVolet(codeVolet);
            param[2] = new EacParameter("codeCaractere", DbType.AnsiStringFixedLength);
            param[2].Value = codeCaractere;
            param[3] = new EacParameter("user", DbType.AnsiStringFixedLength);
            param[3].Value = user;
            param[4] = new EacParameter("date", DbType.Int32);
            param[4].Value = AlbConvert.ConvertDateToInt(date);
            param[5] = new EacParameter("time", DbType.Int32);
            param[5].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
            param[6] = new EacParameter("codeId", DbType.Int32);
            param[6].Value = Convert.ToInt32(codeId);

            string sql = @"UPDATE KCATVOLET
                                            SET KAPVOLET = :codeVolet, 
                                                KAPKAKID = :guidVolet, 
                                                KAPCAR = :codeCaractere, 
                                                KAPMAJU = :user, 
                                                KAPMAJD = :date, 
                                                KAPMAJH = :time
                                         WHERE KAPID = :codeId";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static int InsertVoletByCible(string codeBranche, string codeCible, string codeIdCible, string codeVolet, string codeIdVolet, string codeCaractere, string user)
        {
            DateTime date = DateTime.Now;
            int idVolet = CommonRepository.GetAS400Id("KAPID");

            var param = new EacParameter[14];
            param[0] = new EacParameter("idVolet", DbType.Int32);
            param[0].Value = idVolet;
            param[1] = new EacParameter("codeBranche", DbType.AnsiStringFixedLength);
            param[1].Value = codeBranche;
            param[2] = new EacParameter("codeCible", DbType.AnsiStringFixedLength);
            param[2].Value = codeCible;
            param[3] = new EacParameter("codeIdCible", DbType.AnsiStringFixedLength);
            param[3].Value = codeIdCible;
            param[4] = new EacParameter("codeVolet", DbType.AnsiStringFixedLength);
            param[4].Value = codeVolet;
            param[5] = new EacParameter("guidVolet", DbType.AnsiStringFixedLength);
            param[5].Value = ObtenirGuidVolet(codeVolet);
            param[6] = new EacParameter("codeCaractere", DbType.AnsiStringFixedLength);
            param[6].Value = codeCaractere;
            param[7] = new EacParameter("user", DbType.AnsiStringFixedLength);
            param[7].Value = user;
            param[8] = new EacParameter("date", DbType.Int32);
            param[8].Value = AlbConvert.ConvertDateToInt(date);
            param[9] = new EacParameter("time", DbType.Int32);
            param[9].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
            param[10] = new EacParameter("user2", DbType.AnsiStringFixedLength);
            param[10].Value = user;
            param[11] = new EacParameter("date2", DbType.Int32);
            param[11].Value = AlbConvert.ConvertDateToInt(date);
            param[12] = new EacParameter("time2", DbType.Int32);
            param[12].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
            param[13] = new EacParameter("ordre", DbType.AnsiStringFixedLength);
            param[13].Value = short.MaxValue / 100;

            string sql = @"INSERT INTO KCATVOLET
                                            (KAPID, KAPBRA, KAPCIBLE, KAPKAIID, KAPVOLET, 
                                             KAPKAKID, KAPCAR, KAPCRU, KAPCRD, KAPCRH, 
                                             KAPMAJU, KAPMAJD, KAPMAJH, KAPORDRE)
                                         VALUES
                                         (:idVolet, :codeBranche, :codeCible, :codeIdCible, :codeVolet, :guidVolet, :codeCaractere, 
                                           :user, :date , :time, :user2, :date2, :time2, :ordre)";

            //(ordreVolet == 0 ? "(SELECT MAX(KAPORDRE) + 1 FROM KCATVOLET)" : "'" + ordreVolet.ToString().Replace(',', '.') + "'"));

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return idVolet;
        }

        private static string GetGuidVolet(string codeVolet)
        {
            var param = new EacParameter[1];
            param[0] = new EacParameter("codeVolet", DbType.AnsiStringFixedLength);
            param[0].Value = codeVolet;
            string sql = @"SELECT KAKID GUIDID FROM KVOLET WHERE KAKVOLET = :codeVolet";
            return DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param).ToString();
        }

        private static string ObtenirGuidVolet(string codeVolet)
        {
            var param = new EacParameter[1];
            param[0] = new EacParameter("codeVolet", DbType.AnsiStringFixedLength);
            param[0].Value = codeVolet;
            string sql = @"SELECT KAKID GUID FROM KVOLET WHERE KAKVOLET = :codeVolet";
            return DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param).ToString();
        }



        #endregion
    }
}

