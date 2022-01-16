using System.Collections.Generic;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using DataAccess.Helpers;
using System.Data;
using System;
using OP.WSAS400.DTO.Bloc;

namespace OP.DataAccess
{
    public class BlocRepository
    {
        #region Méthode Publique

        public static void EnregistrerBlocByCible(string codeId, string codeBranche, string codeCible, string codeVolet, string codeIdVolet, string codeBloc, string codeIdBloc, string codeCaractere, double ordreBloc, string user)
        {
            int idBloc = 0;
            int transformedOrderBloc = AlbConvert.ConvertDoubleToFirstGreaterInt(ordreBloc);

            if (!string.IsNullOrEmpty(codeId))
            {
                int.TryParse(codeId, out idBloc);
                UpdateBlocByCible(codeId, codeBloc, codeCaractere, user);
            }
            else
                idBloc = InsertBlocByCible(codeBranche, codeCible, codeVolet, codeIdVolet, codeBloc, codeIdBloc, codeCaractere, user);

            //Mise à jour des numéros d'ordre
            //RenumBlocs(codeIdVolet);
            RenumBlocs(codeIdVolet, idBloc, transformedOrderBloc, ((int)ordreBloc != transformedOrderBloc) ? 1 : 0);
        }

        //public static void RenumBlocs(string codeIdVolet)
        //{
        //    DbParameter[] param = new DbParameter[1];
        //    param[0] = new EacParameter("codeIdVolet", codeIdVolet);
        //    DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_RENUMBLOC", param);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voletId"></param>
        /// <param name="blocId"></param>
        /// <param name="ordre"></param>
        /// <param name="isOrderTransformed"></param>
        public static void RenumBlocs(string voletId, int blocId, int ordre, int isOrderTransformed)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P_IDVOLET", voletId);
            param[1] = new EacParameter("P_IDBLOC", blocId);
            param[2] = new EacParameter("P_ORDRE", ordre);
            param[3] = new EacParameter("P_IS_TRANSFORMED", isOrderTransformed);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_RENUMBLOC", param);
        }

        public static List<BlocDto> RechercherBlocsByVolet(string codeId, bool saveOptions = false)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codeId", DbType.AnsiStringFixedLength);
            param[0].Value = codeId;


            string sql = @"SELECT KAQID GUIDID, 
                                                KAEBLOC CODE, 
                                                KAEDESC DESCRIPTION, 
                                                KAQCAR CARACTERE, 
                                                KAEID GUIDBLOC, 
                                                KAQORDRE NUMORDRE
                                        FROM KBLOC 
                                            INNER JOIN KCATBLOC ON KAEBLOC = KAQBLOC 
                                            AND KAQKAPID = :codeId
                                        ORDER BY KAQORDRE, KAEBLOC";
            var result = DbBase.Settings.ExecuteList<BlocDto>(CommandType.Text, sql, param);

            return result;
        }

        /// <summary>
        /// Récupère les blocs
        /// </summary>
        public static List<BlocDto> GetBlocs(string code, string description)
        {
            string sql = string.Format(@"SELECT KAEID GUIDID, 
                                                KAEBLOC CODE, 
                                                KAEDESC DESCRIPTION
                                         FROM KBLOC
                                         WHERE UPPER(KAEDESC) LIKE '%{0}%'", description.ToUpper());
            if (!string.IsNullOrEmpty(code))
                sql += string.Format(@" AND UPPER(KAEBLOC) LIKE '%{0}%'", code.ToUpper());

            sql += " ORDER BY KAEBLOC";

            return DbBase.Settings.ExecuteList<BlocDto>(CommandType.Text, sql);
        }



        public static List<BlocDto> ObtenirBlocs()
        {
            string sql = @"SELECT KAEBLOC CODE, 
                                  KAEDESC DESCRIPTION
                           FROM KBLOC
                           ORDER BY KAEBLOC ";
            return DbBase.Settings.ExecuteList<BlocDto>(CommandType.Text, sql);
        }

        public static BlocDto ObtenirBloc(string code)
        {
            BlocDto toReturn = null;

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("code", DbType.AnsiStringFixedLength);
            param[0].Value = code;

            string sql = @"SELECT KAEID GUIDID, 
                                               KAEBLOC CODE, 
                                               KAEDESC DESCRIPTION, 
                                               KAECRD DATECREATION, 
                                               KAECRH HEURECREATION
                                        FROM KBLOC
                                        WHERE KAEID = :code";

            var result = DbBase.Settings.ExecuteList<BlocDto>(CommandType.Text, sql, param);
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
            }
            return toReturn;
        }

        public static void EnregistrerBloc(string codeId, string code, string description, string update, string user)
        {
            EacParameter[] param = null;
            string sql = string.Empty;
            DateTime date = DateTime.Now;

            if (!string.IsNullOrEmpty(codeId))
            {
                param = new EacParameter[5];
                param[0] = new EacParameter("description", DbType.AnsiStringFixedLength);
                param[0].Value = description;
                param[1] = new EacParameter("date", DbType.Int32);
                param[1].Value = AlbConvert.ConvertDateToInt(date);
                param[2] = new EacParameter("time", DbType.Int32);
                param[2].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
                param[3] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[3].Value = user;
                param[4] = new EacParameter("codeId", DbType.AnsiStringFixedLength);
                param[4].Value = codeId;

                sql = @"UPDATE KBLOC 
                                         SET KAEDESC = :description, 
                                             KAEMAJD = :date, 
                                             KAEMAJH = :time, 
                                             KAEMAJU = :user 
                                      WHERE  KAEID = :codeId";
                //description,
                //AlbConvert.ConvertDateToInt(date),
                //AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date)),
                //user,
                //codeId);
            }
            else
            {
                param = new EacParameter[9];
                param[0] = new EacParameter("kaeid", DbType.Int32);
                param[0].Value = CommonRepository.GetAS400Id("KAEID");
                param[1] = new EacParameter("code", DbType.AnsiStringFixedLength);
                param[1].Value = code.ToUpper();
                param[2] = new EacParameter("description", DbType.AnsiStringFixedLength);
                param[2].Value = description;
                param[3] = new EacParameter("dateC", DbType.Int32);
                param[3].Value = AlbConvert.ConvertDateToInt(date);
                param[4] = new EacParameter("timeC", DbType.Int32);
                param[4].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
                param[5] = new EacParameter("userC", DbType.AnsiStringFixedLength);
                param[5].Value = user;
                param[6] = new EacParameter("dateU", DbType.Int32);
                param[6].Value = AlbConvert.ConvertDateToInt(date);
                param[7] = new EacParameter("timeU", DbType.Int32);
                param[7].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
                param[8] = new EacParameter("userU", DbType.AnsiStringFixedLength);
                param[8].Value = user;

                sql = @"INSERT INTO KBLOC 
                                        (KAEID, KAEBLOC, KAEDESC, KAECRD, KAECRH, KAECRU, KAEMAJD, KAEMAJH, KAEMAJU) 
                                      VALUES (:kaeid, :code, :description, :dateC, :timeC, :userC, :dateU, :timeU, :userU)";
                //CommonRepository.GetAS400Id("KAEID"),
                //code.ToUpper(),
                //description,
                //AlbConvert.ConvertDateToInt(date),
                //AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date)),
                //user,
                //AlbConvert.ConvertDateToInt(date),
                //AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date)),
                //user);
            }
            if (!string.IsNullOrEmpty(sql))
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static string SupprimerBloc(string codeId, string infoUser)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P_PARAM", "BLOC");
            param[1] = new EacParameter("P_CODEPARAM", 0);
            param[1].Value = Convert.ToInt32(codeId);
            param[2] = new EacParameter("P_INFOUSER", infoUser);
            param[3] = new EacParameter("P_ERRORMSG", string.Empty);
            param[3].Direction = ParameterDirection.InputOutput;
            param[3].Size = 100;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEPARAMASSOC", param);

            return param[3].Value.ToString();
        }

        public static void EnregistrerBlocByCategorie(string codeId, string codeBranche, string codeCategorie, string codeVolet, string codeIdVolet, string codeBloc, string codeIdBloc, string codeCaractere, string user)
        {
            DateTime? date = DateTime.Now;
            string sql = string.Empty;
            EacParameter[] param = null;

            if (!string.IsNullOrEmpty(codeId))
            {
                param = new EacParameter[6];
                param[0] = new EacParameter("codebloc", DbType.AnsiStringFixedLength);
                param[0].Value = codeBloc;
                param[1] = new EacParameter("codecar", DbType.AnsiStringFixedLength);
                param[1].Value = codeCaractere;
                param[2] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[2].Value = user;
                param[3] = new EacParameter("date", DbType.Int32);
                param[3].Value = AlbConvert.ConvertDateToInt(date);
                param[4] = new EacParameter("time", DbType.Int32);
                param[4].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
                param[5] = new EacParameter("codeid", DbType.AnsiStringFixedLength);
                param[5].Value = codeId;

                sql = @"UPDATE KCATBLOC 
                                SET KAQBLOC = :codebloc, 
                                KAQCAR = :codecar, 
                                KAQMAJU = :user, 
                                KAQMAJD = :date, 
                                KAQMAJH = :time 
                        WHERE KAQID = :codeid";
                //codeBloc,
                //codeCaractere,
                //user,
                //AlbConvert.ConvertDateToInt(date),
                //AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date)),
                //codeId
                //);
            }
            else
            {
                param = new EacParameter[14];
                param[0] = new EacParameter("kaqid", DbType.Int32);
                param[0].Value = CommonRepository.GetAS400Id("KAQID");
                param[1] = new EacParameter("codebranche", DbType.AnsiStringFixedLength);
                param[1].Value = codeBranche;
                param[2] = new EacParameter("codecategorie", DbType.AnsiStringFixedLength);
                param[2].Value = codeCategorie;
                param[3] = new EacParameter("codevolet", DbType.AnsiStringFixedLength);
                param[3].Value = codeVolet;
                param[4] = new EacParameter("codeidvolet", DbType.AnsiStringFixedLength);
                param[4].Value = codeIdVolet;
                param[5] = new EacParameter("codebloc", DbType.AnsiStringFixedLength);
                param[5].Value = codeBloc;
                param[6] = new EacParameter("guidbloc", DbType.AnsiStringFixedLength);
                param[6].Value = GetGuidBloc(codeBloc);
                param[7] = new EacParameter("codecar", DbType.AnsiStringFixedLength);
                param[7].Value = codeCaractere;
                param[8] = new EacParameter("userC", DbType.AnsiStringFixedLength);
                param[8].Value = user;
                param[9] = new EacParameter("dateC", DbType.Int32);
                param[9].Value = AlbConvert.ConvertDateToInt(date);
                param[10] = new EacParameter("timeC", DbType.Int32);
                param[10].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
                param[11] = new EacParameter("userU", DbType.AnsiStringFixedLength);
                param[11].Value = user;
                param[12] = new EacParameter("dateU", DbType.Int32);
                param[12].Value = AlbConvert.ConvertDateToInt(date);
                param[13] = new EacParameter("timeU", DbType.Int32);
                param[13].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));

                sql = @"INSERT INTO KCATBLOC
                            (KAQID, KAQBRA, KAQCAT, KAQVOLET, KAQKAPID, 
                                KAQBLOC, KAQKAEID, KAQCAR, KAQCRU, KAQCRD, 
                                KAQCRH, KAQMAJU, KAQMAJD, KAQMAJH)
                        VALUES
                            (:kaqid, :codebranche, :codecategorie, :codevolet, :codeidvolet, :codebloc, :guidbloc, 
                                :codecar, :userC, :dateC, :timeC, :userU, :dateU, :timeU)";
                //CommonRepository.GetAS400Id("KAQID"),
                //codeBranche,
                //codeCategorie,
                //codeVolet,
                //codeIdVolet,
                //codeBloc,
                //GetGuidBloc(codeBloc),
                //codeCaractere,
                //user,
                //AlbConvert.ConvertDateToInt(date),
                //AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date)),
                //user,
                //AlbConvert.ConvertDateToInt(date),
                //AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date)));
            }

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static string SupprimerBlocByCategorie(string codeId, string codeIdVolet, string infoUser)
        {
            int idBloc = Convert.ToInt32(codeId);
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P_PARAM", "CATBLOC");
            param[1] = new EacParameter("P_CODEPARAM", 0);
            param[1].Value = idBloc;
            param[2] = new EacParameter("P_INFOUSER", infoUser);
            param[3] = new EacParameter("P_ERRORMSG", string.Empty);
            param[3].Direction = ParameterDirection.InputOutput;
            param[3].Size = 100;


            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEPARAMASSOC", param);

            //RenumBlocs(codeIdVolet, idBloc,0);
            return param[3].Value.ToString();
        }

        public static List<BlocDto> GetListeBlocsIncompatiblesAssocies(string codeIdBloc, string typeBloc)
        {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("typbloc", DbType.AnsiStringFixedLength);
            param[0].Value = typeBloc;
            param[1] = new EacParameter("idbloc", DbType.AnsiStringFixedLength);
            param[1].Value = codeIdBloc;


            string sql = @"SELECT 
                                KGJID GUIDID,
                                KAEBLOC CODE, 
                                KAEDESC DESCRIPTION
                                FROM KBLOC INNER JOIN
                                KBLOREL ON KAEID = KGJIDBLO1
                                AND KGJREL = :typbloc
                            WHERE KGJIDBLO2 = :idbloc
                            ORDER BY KAEBLOC";
            //typeBloc, codeIdBloc);

            return DbBase.Settings.ExecuteList<BlocDto>(CommandType.Text, sql, param);
        }

        public static List<BlocDto> GetListeBlocsReferentielIncompatiblesAssocies(string codeIdBloc, string typeBloc)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("idbloc", DbType.AnsiStringFixedLength);
            param[0].Value = codeIdBloc;

            string sql = @"SELECT KAEID GUIDID, 
                                         KAEBLOC CODE, 
                                         KAEDESC DESCRIPTION
                                         FROM KBLOC 
                                         WHERE KAEID NOT IN 
                                            (SELECT KGJIDBLO2 FROM KBLOREL WHERE KGJIDBLO1 = :idbloc)
                                            AND KAEID <> :idbloc
                                         ORDER BY KAEBLOC";
            //codeIdBloc);
            return DbBase.Settings.ExecuteList<BlocDto>(CommandType.Text, sql, param);
        }

        public static string EnregistrerBlocIncompatibleAssocie(string idAssociation, string mode, string idBloctraite, string idBlocIncompatible, string typeBloc, string user)
        {
            DbParameter[] param = new DbParameter[9];
            param[0] = new EacParameter("P_ID_ASSOCIATION", 0);
            param[0].Value = idAssociation;
            param[1] = new EacParameter("P_BLOC_TRAITE", 0);
            param[1].Value = !string.IsNullOrEmpty(idBloctraite) ? Convert.ToInt32(idBloctraite) : 0;
            param[2] = new EacParameter("P_BLOC_ID", idBlocIncompatible);
            param[2].Value = !string.IsNullOrEmpty(idBlocIncompatible) ? Convert.ToInt32(idBlocIncompatible) : 0;
            param[3] = new EacParameter("P_TYPE_RELATION", typeBloc);
            param[4] = new EacParameter("P_MODE", mode);
            param[5] = new EacParameter("P_USER", user);
            param[6] = new EacParameter("P_DATE", DateTime.Now.ToString("yyyyMMdd"));
            param[7] = new EacParameter("P_HEURE", DateTime.Now.ToString("HHmm"));
            param[8] = new EacParameter("P_ERREUR", "");
            param[8].Direction = ParameterDirection.InputOutput;
            param[8].Size = 256;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDBLOCINCOMPATIBLE", param);
            return param[8].Value.ToString();


        }

        #endregion

        #region Méthode Privée

        private static void UpdateBlocByCible(string codeId, string codeBloc, string codeCaractere, string user)
        {
            //Int32 ordrebloc = Convert.ToInt32(Math.Round(ordreBloc));
            DateTime date = DateTime.Now;

            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("codebloc", DbType.AnsiStringFixedLength);
            param[0].Value = codeBloc;
            param[1] = new EacParameter("guidbloc", DbType.AnsiStringFixedLength);
            param[1].Value = GetGuidBloc(codeBloc);
            param[2] = new EacParameter("codecar", DbType.AnsiStringFixedLength);
            param[2].Value = codeCaractere;
            param[3] = new EacParameter("user", DbType.AnsiStringFixedLength);
            param[3].Value = user;
            param[4] = new EacParameter("date", DbType.Int32);
            param[4].Value = AlbConvert.ConvertDateToInt(date);
            param[5] = new EacParameter("time", DbType.Int32);
            param[5].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
            param[6] = new EacParameter("codeid", DbType.AnsiStringFixedLength);
            param[6].Value = codeId;

            string sql = @"UPDATE KCATBLOC 
                                SET KAQBLOC = :codebloc, 
                                KAQKAEID = :guidbloc, 
                                KAQCAR = :codecar, 
                                KAQMAJU = :user, 
                                KAQMAJD = :date, 
                                KAQMAJH = :time
                            WHERE KAQID = :codeid";
            //codeBloc,
            //GetGuidBloc(codeBloc),
            //codeCaractere,
            //user,
            //AlbConvert.ConvertDateToInt(date),
            //AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date)),
            //codeId);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static int InsertBlocByCible(string codeBranche, string codeCible, string codeVolet, string codeIdVolet, string codeBloc, string codeIdBloc, string codeCaractere, string user)
        {
            int idBloc = CommonRepository.GetAS400Id("KAQID");
            DateTime date = DateTime.Now;

            EacParameter[] param = new EacParameter[15];
            param[0] = new EacParameter("idbloc", DbType.Int32);
            param[0].Value = idBloc;
            param[1] = new EacParameter("codebranche", DbType.AnsiStringFixedLength);
            param[1].Value = codeBranche;
            param[2] = new EacParameter("codecible", DbType.AnsiStringFixedLength);
            param[2].Value = codeCible;
            param[3] = new EacParameter("codevolet", DbType.AnsiStringFixedLength);
            param[3].Value = codeVolet;
            param[4] = new EacParameter("idvolet", DbType.AnsiStringFixedLength);
            param[4].Value = codeIdVolet;
            param[5] = new EacParameter("codebloc", DbType.AnsiStringFixedLength);
            param[5].Value = codeBloc;
            param[6] = new EacParameter("guidbloc", DbType.AnsiStringFixedLength);
            param[6].Value = GetGuidBloc(codeBloc);
            param[7] = new EacParameter("codecar", DbType.AnsiStringFixedLength);
            param[7].Value = codeCaractere;
            param[8] = new EacParameter("userC", DbType.AnsiStringFixedLength);
            param[8].Value = user;
            param[9] = new EacParameter("dateC", DbType.Int32);
            param[9].Value = AlbConvert.ConvertDateToInt(date);
            param[10] = new EacParameter("timeC", DbType.Int32);
            param[10].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
            param[11] = new EacParameter("userU", DbType.AnsiStringFixedLength);
            param[11].Value = user;
            param[12] = new EacParameter("dateU", DbType.Int32);
            param[12].Value = AlbConvert.ConvertDateToInt(date);
            param[13] = new EacParameter("timeU", DbType.Int32);
            param[13].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
            param[14] = new EacParameter("ordre", DbType.Int16);
            param[14].Value = short.MaxValue / 100;

            string sql = @"INSERT INTO KCATBLOC
                            (KAQID, KAQBRA, KAQCIBLE, KAQVOLET, KAQKAPID, 
                            KAQBLOC, KAQKAEID, KAQCAR, KAQCRU, KAQCRD, 
                            KAQCRH, KAQMAJU, KAQMAJD, KAQMAJH, KAQORDRE)
                        VALUES
                            (:idbloc, :codebranche, :codecible, :codevolet, :idvolet, 
                            :codebloc, :guidbloc, :codecar, :userC, 
                            :dateC, :timeC, :userU, :dateU, :timeU, :ordre)";
            //idBloc,
            //codeBranche,
            //codeCible,
            //codeVolet,
            //codeIdVolet,
            //codeBloc,
            //GetGuidBloc(codeBloc),
            //codeCaractere,
            //user,
            //AlbConvert.ConvertDateToInt(date),
            //AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date)),
            //user,
            //AlbConvert.ConvertDateToInt(date),
            //AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date)),
            //short.MaxValue / 100);
            //(ordreBloc == 0 ? "(SELECT MAX(KAQORDRE) + 1 FROM KCATBLOC)" : "'" + ordreBloc.ToString().Replace(',', '.') + "'"));

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return idBloc;
        }

        private static string GetGuidBloc(string codeBloc)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codebloc", DbType.AnsiStringFixedLength);
            param[0].Value = codeBloc;

            string sql = @"SELECT KAEID GUIDID FROM KBLOC WHERE KAEBLOC = :codebloc";
            //codeBloc);
            return DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param).ToString();
        }
        #endregion
    }
}
