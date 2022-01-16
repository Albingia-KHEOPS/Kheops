using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Cotisations;
using OP.WSAS400.DTO.FraisAccessoires;
using OP.WSAS400.DTO.GestionIntervenants;
using OP.WSAS400.DTO.GestUtilisateurs;
using OP.WSAS400.DTO.Logging;
using OP.WSAS400.DTO.MenuContextuel;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Partenaire;
using OP.WSAS400.DTO.PGM;
using OP.WSAS400.DTO.Regularisation;
using OP.WSAS400.DTO.Stat;
using OP.WSAS400.DTO.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Globalization;
using System.Linq;
using System.Text;
using static DataAccess.Helpers.OutilsHelper;

namespace OP.DataAccess
{
    public class CommonRepository
    {
        #region Méthodes Métiers

        #endregion
        #region Méthodes Publiques

        public static string GetAs400User(string adUser, IDbConnection connection = null)
        {
            string user400 = string.Empty;
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("P_USER", adUser);

            string sql = @"SELECT UTIUT FROM YUTILIS WHERE UPPER(TRIM(UTPFX)) = UPPER(TRIM(:P_USER))";

            if (connection == null)
            {
                user400 = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param)?.FirstOrDefault().StrReturnCol;
            }
            else
            {
                var dbOptions = new DbSelectOptions()
                {
                    SqlText = sql,
                    DbConnection = connection,
                    Parameters = param
                };
                user400 = dbOptions.PerformSelect<DtoCommon>()?.FirstOrDefault().StrReturnCol;
            }

            if (!user400.IsEmptyOrNull())
            {
                sql = $@"DELETE FROM KVERROU WHERE KAVCRD != {AlbConvert.ConvertDateToInt(DateTime.Now)} AND UPPER(TRIM(KAVCRU)) = UPPER(TRIM('{user400}'))";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }

            return user400;
        }

        public static string GetEtatActeGestion(string codeOffre, int version, string type, string codeAvn = null, bool isBNS = false)
        {
            string sql = @"SELECT PBETA FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvn
                            UNION ALL
                            SELECT PBETA FROM YHPBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvn";

            if (isBNS)
            {
                sql = @"SELECT KHWETA FROM KPRGU WHERE KHWIPB = :codeOffre AND KHWALX = :version and KHWTYP = :type and KHWAVN = :codeAvn ORDER BY KHWID FETCH FIRST 1 ROWS ONLY";
            }

            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') };
            param[1] = new EacParameter("version", DbType.Int32) { Value = version };
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength) { Value = type };
            param[3] = new EacParameter("codeAvn", DbType.Int32) { Value = (String.IsNullOrEmpty(codeAvn)) ? 0 : Convert.ToInt32(codeAvn) };

            var etat = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param).ToString();

            return etat;
        }


        public static UsersDto GetListAlbUser()
        {
            string sql = @"SELECT UTPFX USERWIN, UTNOM USERNAME, UTPNM USERPNAME, UTIUT USERLOGIN,
UTGRP USERGROUP, UTSGR USERSGROUP
FROM YUTILIS;";


            var result = DbBase.Settings.ExecuteList<AlbUserDto>(CommandType.Text, sql);
            //var result = DbBase.Settings.ExecuteList<AlbUserDto>(CommandType.StoredProcedure, "SP_GETLISTALBUSER");
            UsersDto albUsers = new UsersDto { Users = result };
            return albUsers;
        }

        /// <summary>
        /// Obtenir les informations d'un utilisateur par son identifiant
        /// </summary>
        /// <param name="username">Utilisateur connecté</param>
        /// <returns></returns>
        public static AlbUserDto GetUserInformation(string username)
        {
            string sql = @"SELECT UTPFX USERWIN, UTNOM USERNAME, UTPNM USERPNAME, UTIUT USERLOGIN, UTAEM USERMAIL, UTGRP USERGROUP, UTSGR USERSGROUP
                           FROM YUTILIS
                           WHERE UTIUT = :username";
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("username", DbType.String) { Value = username };
            return DbBase.Settings.ExecuteList<AlbUserDto>(CommandType.Text, sql, param)?.FirstOrDefault();
        }
        /// <summary>
        /// MAJ de la table
        /// </summary>
        /// <param name="sql">requête de mise à jour (Insert/Update/Delete) à exécuter</param>
        /// <returns>True si la mise à jour a étét effectué avec succés sinon false</returns>
        public static bool UpdateDB(string sql)
        {
            return DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql) > 0;
        }

        /// <summary>
        /// retour une liste qui va servir à alimenter une DropDownList
        /// </summary>
        /// <param name="sqlRequest">requete Sql select</param>
        /// <returns>Liste d'objet Clé / valeur</returns>
        public static List<T> GetDropDownValue<T>(string sqlRequest) where T : class, new()
        {
            return DbBase.Settings.ExecuteList<T>(CommandType.Text, sqlRequest);
        }

        /// <summary>
        /// Test l'existance d'une ligne
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExistRow(string sql, params DbParameter[] pParams)
        {
            return ExistRowParam(sql, pParams);
        }

        /// <summary>
        /// Test l'existance d'une ligne
        /// </summary>
        public static bool ExistRowParam(string sql, IEnumerable<DbParameter> param)
        {
            var request = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (request == null)
            {
                return false;
            }

            var firstOrDefault = request.FirstOrDefault();
            return firstOrDefault != null && firstOrDefault.NbLigne > 0;
        }

        /// <summary>
        /// Retourne le nb ligne (requête count)
        /// </summary>
        /// <param name="sql">la requête count</param>
        /// <returns></returns>
        public static long RowNumber(string sql)
        {
            var request = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (request == null)
            {
                return 0;
            }

            var firstOrDefault = request.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.NbLigne : 0;
        }

        /// <summary>
        /// Retourne le nb ligne (requête count)
        /// </summary>
        /// <param name="sql">la requête count</param>
        /// <returns></returns>
        public static long RowNumber(string sql, IEnumerable<DbParameter> parameters)
        {
            var request = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, parameters);
            if (request == null)
            {
                return 0;
            }

            var firstOrDefault = request.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.NbLigne : 0;
        }

        /// <summary>
        /// Retourne une valeur à partir de la base de donnée ayant un Alias : "STRRETURNCOL"
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetStrValue(string sql)
        {
            var request = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (request == null)
            {
                return null;
            }

            var firstOrDefault = request.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.StrReturnCol : string.Empty;
        }

        /// <summary>
        /// Retourne une valeur à partir de la base de donnée ayant un Alias : "STRRETURNCOL"
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetStrValue(string sql, IEnumerable<DbParameter> parameters)
        {
            var request = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, parameters);
            if (request == null)
            {
                return null;
            }

            var firstOrDefault = request.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.StrReturnCol : string.Empty;
        }


        /// <summary>
        /// Retourne une valeur à partir de la base de donnée ayant un Alias : "INT64RETURNCOL"
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static long? GetInt64Value(string sql)
        {
            var request = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (request == null)
            {
                return null;
            }

            var firstOrDefault = request.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.Int64ReturnCol : null;
        }
        /// <summary>
        /// Retourne une valeur à partir de la base de donnée ayant un Alias : "INT64RETURNCOL"
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static long? GetInt64Value(string sql, IEnumerable<DbParameter> parameters)
        {
            var request = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, parameters);
            if (request == null)
            {
                return null;
            }

            var firstOrDefault = request.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.Int64ReturnCol : null;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="tCon"></param>
        /// <param name="tFam"></param>
        /// <param name="tPca1"></param>
        /// <param name="tCod"></param>
        /// <param name="notIn"></param>
        /// <returns></returns>
        public static List<ParametreDto> GetParametres(string branche, string cible, string tCon, string tFam, string tPca1 = null, List<string> tCod = null, bool notIn = false, bool isBO = false, string tPcn2 = null)
        {
            var sql = BuildSelectYYYYPAR(branche, cible, string.Empty, "TCOD CODE, TPLIB LIBELLE, TPLIL DESCRIPTIF,TPCN1 CODETPCN1, TPCN2 CODETPCN2, TPCA1 CODETPCA1,TPCA2 CODETPCA2", tCon, tFam, tCod: tCod, notIn: notIn, isBO: isBO, tPcn2: tPcn2,
                otherCriteria: !string.IsNullOrEmpty(tPca1) ? tPca1 == "O" ? " AND TPCA1 = '" + tPca1 + "'" : " AND TPCA1 <> 'O'" : string.Empty);
            var toReturn = !string.IsNullOrEmpty(sql) ? DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql).OrderBy(m => m.Code).ToList() : null;

            return toReturn;
        }

        public static ParametreDto GetParametreByCode(string branche, string cible, string tCon, string tFam, string tCod)
        {
            var sql = BuildSelectYYYYPAR(branche, cible, string.Empty, "TCOD CODE, TPLIB LIBELLE", tCon, tFam, otherCriteria: " AND TCOD = '" + tCod + "'");
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql).FirstOrDefault();
        }

        public static string GetLibParametre(string concept, string famille)
        {
            string sql = $@"SELECT TCOD CODE , TPLIB LIBELLE FROM YYYYPAR WHERE TCON='{concept}' AND TFAM ='{famille}' ";

            List<ParametreDto> result = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);

            return result.Any() ? result.FirstOrDefault().Libelle : string.Empty;
        }

        public static string GetAttrFromStr(string strSearch, string attr, string firstChar, string lastChar)
        {
            if (strSearch.Contains(attr + firstChar))
            {
                var lenAttr = attr.Length + firstChar.Length;

                var firstIndex = strSearch.IndexOf(attr + firstChar, 0);
                var lastIndex = strSearch.IndexOf(lastChar, firstIndex + lenAttr);

                return strSearch.Substring(firstIndex + lenAttr, lastIndex - (firstIndex + lenAttr));
            }
            return string.Empty;
        }

        public static BrancheDto GetBrancheCible(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var model = new BrancheDto { Code = string.Empty, Cible = new CibleDto { Code = string.Empty } };
            if (string.IsNullOrEmpty(codeOffre))
            {
                return model;
            }

            var param = new List<DbParameter>() {
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(version)),
                new EacParameter("type", type),
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                short.TryParse(codeAvn, out short avn);
                param.Add(new EacParameter("codeAvn", avn));
            }


            string sql = string.Format(@"SELECT DISTINCT PBBRA BRANCHE, KABCIBLE CIBLE, KABRSQ
                            FROM {0}
	                            INNER JOIN {1} ON PBIPB = KABIPB AND PBALX = KABALX AND {2} = KABTYP {4}
                            WHERE PBIPB = :codeOffre AND PBALX = :version AND {2} = :type {3}
                            ORDER BY KABRSQ",
                            GetPrefixeHisto(modeNavig, "YPOBASE"),
                            GetPrefixeHisto(modeNavig, "KPRSQ"),
                            modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                            modeNavig == ModeConsultation.Historique ? " AND PBAVN = :codeAvn" : string.Empty,
                            modeNavig == ModeConsultation.Historique ? " AND PBAVN = KABAVN" : string.Empty);

            var result = DbBase.Settings.ExecuteList<BrancheCiblePlatDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (result != null)
            {
                model.Code = result.Branche;
                model.Cible = new CibleDto { Code = result.Cible };
            }

            return model;
        }

        public static BrancheDto GetBrancheCibleFormule(string codeOffre, string version, string type, string codeAvn, string codeFormule, ModeConsultation modeNavig)
        {
            var model = new BrancheDto { Code = string.Empty, Cible = new CibleDto { Code = string.Empty } };

            var param = new List<DbParameter>() {
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(version)),
                new EacParameter("type", type),
                new EacParameter("codeFormule", Convert.ToInt32(codeFormule))
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                short.TryParse(codeAvn, out short avn);
                param.Add(new EacParameter("codeAvn", avn));
            }

            string sql = string.Format(@"SELECT KDABRA BRANCHE, KDACIBLE CIBLE , KDAKAIID IDCIBLE
                                FROM {0}
                                WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type AND KDAFOR = :codeFormule {1}",
                                GetPrefixeHisto(modeNavig, "KPFOR"),
                                modeNavig == ModeConsultation.Historique ? " AND KDAAVN = :codeAvn" : string.Empty);

            var result = DbBase.Settings.ExecuteList<BrancheCiblePlatDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (result != null)
            {
                model.Code = result.Branche;
                model.Cible = new CibleDto { Code = result.Cible, GuidId = result.Idcible };
            }

            return model;
        }

        public static DtoCommon GetSousBrancheCategorie(string branche, string cible)
        {
            DbParameter[] param = new DbParameter[2];
            param[0] = new EacParameter("branche", branche);
            param[1] = new EacParameter("cible", cible);
            string sql = @"SELECT KAISBR SOUSBRANCHE, KAICAT CATEGORIE FROM KCIBLEF WHERE KAIBRA = :branche AND KAICIBLE = :cible";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault();
            }

            return null;
        }


        public static bool GetIsAlertePeriode(DateTime? dateDeb, DateTime? dateFin, DateTime? datePerDeb, DateTime? datePerFin, string periodicite, DateTime? prochaineEch)
        {
            if (dateDeb > datePerFin)
            {
                return true;
            }

            //if (periodicite != "U" && periodicite != "R" && periodicite != "E")
            //{
            //    dateFin = prochaineEch.Value.AddDays(-1);
            //    if (AlbConvert.ConvertDateToInt(dateFin) == 0)
            //        dateFin = datePerFin;
            //}
            if (dateFin < datePerDeb)
            {
                return true;
            }

            return false;
        }


        #region Méthode utilisée dans l'offre et le contrat pour le suivi des actes de gestion
        public static List<ActeGestionDto> GetListActesGestion(string codeOffre, string version, string type, DateTime? dateDeb, DateTime? dateFin, string user, string traitement)
        {
            string sql = string.Empty;

            sql = @"SELECT DISTINCT PYTRJ DATEJOUR,PYTRM DATEMOIS,PYTRA DATEANNEE,PYTRH HEURE,PYAVN NUMERO,PYVAG TYPETRAITEMENT,
                        IFNULL(TPLIL, '') LIBELLE,PYLIB DESCRIPTION,PYMJU UTILISATEUR,PYTTR CODETRAITEMENT,
                        (PBDEA * 10000 + PBDEM * 100 + PBDEJ) DATECREATION
                        FROM YPOTRAC
                        INNER JOIN YPOBASE ON PBIPB=PYIPB AND PBALX=PYALX AND PBTYP=PYTYP
                        {3}
                        WHERE  PYIPB=:codeAffaire AND PYALX=:version AND PYTYP=:type {4} ORDER BY PYTRA DESC,PYTRM DESC,PYTRJ DESC,PYTRH DESC";

            var sqlWhere = "";
            if (dateDeb.HasValue)
            {
                sqlWhere += " AND (PYTRA * 10000 + PYTRM * 100 + PYTRJ) >= :dateDeb";
            }
            if (dateFin.HasValue)
            {
                sqlWhere += " AND (PYTRA * 10000 + PYTRM * 100 + PYTRJ) <= :dateFin";
            }
            if (!string.IsNullOrEmpty(user))
            {
                sqlWhere += " AND PYMJU = :user";
            }
            if (!string.IsNullOrEmpty(traitement))
            {
                sqlWhere += " AND PYTTR = :traitement";
            }

            sql = string.Format(sql, codeOffre.Trim().PadLeft(9, ' '), version, type,
                BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", otherCriteria: " AND TCOD = PYTTR"),
                sqlWhere);

            var param = MakeParamsDynamic(sql, new
            {
                codeAffaire = codeOffre.ToIPB(),
                version,
                type,
                dateDeb = AlbConvert.ConvertDateToInt(dateDeb),
                dateFin = AlbConvert.ConvertDateToInt(dateFin),
                user,
                traitement
            }, loose: true);

            var result = DbBase.Settings.ExecuteList<ActeGestionDto>(CommandType.Text, sql, param);

            if (result.Find(elem => elem.CodeTraitement == "REGUL") != null)
            {
                var paramRegul = new List<EacParameter>
                {
                    new EacParameter("codeContrat", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ')}
                };

                string query = "SELECT KHWAVN CODEAVN, KHWMRG REGULEMODVALUE FROM KPRGU WHERE KHWIPB = :codeContrat";
                var infos = DbBase.Settings.ExecuteList<ActeGestionRegularisation>(CommandType.Text, query, paramRegul);
                // B3938
                // Affectation des valeurs ReguleMode à partir ReguleModeStringValue
                infos.ForEach(x => x.ReguleMode = x.ReguleModeStringValue.ParseCode<RegularisationMode>());

                var tmp = result.GroupBy(elem => elem.Numero).Select(group => group.First()).Where(acteGestion => acteGestion.CodeTraitement == "REGUL").ToList();
                foreach (var acteGestion in tmp)
                {
                    var currentActeGestion = infos.Where(o => o.CodeAvn == acteGestion.Numero).First();
                    if (currentActeGestion.ReguleMode != RegularisationMode.Standard && currentActeGestion.ReguleMode != RegularisationMode.Coassurance)
                    {
                        result.Where(o => o.Numero == acteGestion.Numero).ToList().ForEach(o => o.Libelle = currentActeGestion.ReguleMode.ToString());
                    }
                }
            }
            // B2568
            // Ajouter Lib : Résiliation
            result.Where(o => o.CodeTraitement == AlbConstantesMetiers.TRAITEMENT_RESILHORSAVN || o.CodeTraitement == AlbConstantesMetiers.TRAITEMENT_RESILHORSAVN_ANNUL).ToList()
                  .ForEach(o => o.Libelle = "Résiliation");
            return result;
        }

        public static void AjouterActeGestion(string codeOffre, string version, string type, int numAvenant, string typeActeGestion, string codeTraitement, string libelle, string utilisateur)
        {
            //récupération de l'acte de gestion de l'affaire
            if (!codeTraitement.Equals(AlbConstantesMetiers.TRAITEMENT_MODIFHORSAVN, StringComparison.InvariantCulture)
                && !codeTraitement.Equals(AlbConstantesMetiers.TRAITEMENT_RESILHORSAVN, StringComparison.InvariantCulture)
                && !codeTraitement.Equals(AlbConstantesMetiers.TRAITEMENT_RESILHORSAVN_ANNUL, StringComparison.InvariantCulture))
            {

                string acteGes = string.Format(@"SELECT PBTTR STRRETURNCOL FROM YPOBASE WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}' AND PBAVN = '{3}'",
                    codeOffre.ToIPB(), !string.IsNullOrEmpty(version) ? version : "0", type, numAvenant);
                var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, acteGes);
                if (result != null && result.Any())
                {
                    codeTraitement = result.FirstOrDefault().StrReturnCol;
                }
            }

            /** Trace de la prime non émise  **/
            string sqlCount = string.Empty;
            if (typeActeGestion == "V")
            {
                sqlCount = string.Format(@"SELECT COUNT(*) NBLIGN  FROM KPAVTRC
                                                    INNER JOIN YPOTRAC ON PYIPB=KHOIPB AND PYALX=KHOALX AND PYTYP=KHOTYP AND PYVAG='V'
                                                    WHERE KHOIPB = '{0}' AND KHOALX = {1} AND KHOTYP = '{2}' AND KHOPERI = 'COT' AND KHOOEF = '{3}'", codeOffre.ToIPB(), Convert.ToInt32(version), type, codeTraitement == "REGUL" ? "NR" : "NG");
                if (ExistRow(sqlCount))
                {
                    libelle += "Non émise";
                }
            }
            //string retour = string.Empty;
            //try
            //{
            if ((codeTraitement != "REGUL" && codeTraitement != "AVNRG") || ((codeTraitement == "REGUL" || codeTraitement == "AVNRG") && typeActeGestion == "V"))
            {
                string sql = @"INSERT INTO YPOTRAC(PYTYP,PYIPB,PYALX,PYAVN,PYTTR,PYVAG,PYTRA,PYTRM,PYTRJ,PYTRH,PYLIB,PYINF,
                           PYSDA,PYSDM,PYSDJ,PYSFA,PYSFM,PYSFJ,PYMJU,PYMJA,PYMJM,PYMJJ,PYMJH)
                           VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',
                           '{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}')";
                string sqlFinal = string.Format(sql, type, codeOffre.PadLeft(9, ' '), version, numAvenant, codeTraitement, typeActeGestion, DateTime.Now.Year,
                                                 DateTime.Now.Month, DateTime.Now.Day, AlbConvert.ConvertTimeToIntMinute(DateTime.Now.TimeOfDay), libelle, 'I', 0, 0, 0,
                                                 0, 0, 0, utilisateur, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, AlbConvert.ConvertTimeToIntMinute(DateTime.Now.TimeOfDay));

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlFinal);
            }
            //}
            //catch (Exception)
            //{
            //    retour = "ERR";
            //}
            //return retour;
        }

        #endregion

        public static List<UtilisateurBrancheDto> GetUserRights(string userName)
        {
            string sql = string.Empty;

            if (!string.IsNullOrEmpty(userName))
            {

                DbParameter[] param = new DbParameter[1];
                param[0] = new EacParameter("UserName", userName.ToLower().Trim());

                sql = @"SELECT KHRUSR UTILISATEUR, KHRBRA BRANCHE, KHRTYD TYPEDROIT FROM KUSRDRT WHERE TRIM(LOWER(KHRUSR)) = :UserName";
                return DbBase.Settings.ExecuteList<UtilisateurBrancheDto>(CommandType.Text, sql, param);
            }
            sql = @"SELECT KHRUSR UTILISATEUR, KHRBRA BRANCHE, KHRTYD TYPEDROIT FROM KUSRDRT";
            return DbBase.Settings.ExecuteList<UtilisateurBrancheDto>(CommandType.Text, sql);
        }

        public static void ReloadEngagement(string code, string version, string type, string codeCopy, string versionCopy, string typeCopy, string user, string acteGestion)
        {
            ///Lancement des PGM400 des engagements et montant référence
            ///puis copie des commentaires et SMP Forcé
            LoadAS400Engagement(code.PadLeft(9, ' '), version, type, ModeConsultation.Standard, "0", user, acteGestion);
            InitMontantRef(code.PadLeft(9, ' '), version, type, "CALCUL", ModeConsultation.Standard, "0", user, acteGestion);

            DbParameter[] paramEng = new DbParameter[6];
            paramEng[0] = new EacParameter("P_CODEAFFAIRE", codeCopy.ToUpper().PadLeft(9, ' '));
            paramEng[1] = new EacParameter("P_VERSION", 0)
            {
                Value = Convert.ToInt32(versionCopy)
            };
            paramEng[2] = new EacParameter("P_TYPE", typeCopy);
            paramEng[3] = new EacParameter("P_CODEAFFAIREDEST", code.ToUpper().PadLeft(9, ' '));
            paramEng[4] = new EacParameter("P_VERSIONDEST", 0)
            {
                Value = Convert.ToInt32(version)
            };
            paramEng[5] = new EacParameter("P_TYPEDEST", type);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_COPYENGAGEMENT", paramEng);

            LoadAS400Engagement(code.PadLeft(9, ' '), version, type, ModeConsultation.Standard, "0", user, acteGestion);
        }

        public static string GetInfoActeGstion(string codeAffaire, string version, string type)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("codeAffaire", codeAffaire.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[2] = new EacParameter("type", type);

            string sql = @"SELECT PBAVN CONCAT '_' CONCAT PBTTR STRRETURNCOL FROM YPOBASE WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().StrReturnCol;
            }

            return string.Empty;
        }

        #endregion
        #region Méthodes Privées

        private static string ParametresQuery(ref int nbParam, string tPca1 = null, List<string> tCod = null, bool notIn = false)
        {
            string sql = string.Empty;
            sql = @"SELECT TCOD CODE, TPLIB LIBELLE
                FROM YYYYPAR
                WHERE TCON = :TCON AND TFAM = :TFAM";

            if (!string.IsNullOrEmpty(tPca1))
            {
                sql += " AND TPCA1 = :TPCA1";
                nbParam++;
            }
            if (tCod != null)
            {
                sql += notIn ? " AND TCOD NOT IN (" : " AND TCOD IN (";

                for (int i = 0; i < tCod.Count; i++)
                {
                    sql += ":TCOD" + i + ", ";
                    nbParam++;
                }

                sql = sql.Substring(0, sql.Length - 2);
                sql += ")";
            }
            return sql;
        }

        private static DbParameter[] ParametresQueryParam(string tCon, string tFam, int nbParam, string tPca1 = null, List<string> tCod = null)
        {
            int countParam = 2;
            DbParameter[] toReturn = new DbParameter[nbParam];
            toReturn[0] = new EacParameter("TCON", tCon);
            toReturn[1] = new EacParameter("TFAM", tFam);

            if (!string.IsNullOrEmpty(tPca1))
            {
                toReturn[countParam] = new EacParameter("TPCA1", tPca1);
                countParam++;
            }
            if (tCod != null)
            {
                for (int i = 0; i < tCod.Count; i++)
                {
                    toReturn[countParam] = new EacParameter("TCOD" + i, tCod[i]);
                    countParam++;
                }
            }
            return toReturn;
        }

        #endregion
        #region Log appels AS400

        private static void LockAS400User(string codeDossier, string typeDossier, string versionDossier, string avenantDossier, string acteGestion, string user)
        {
            UnlockAS400User(codeDossier, typeDossier, versionDossier, user);
            int.TryParse(versionDossier, out int iversion);
            int.TryParse(avenantDossier, out int iAvenant);
            if (!string.IsNullOrEmpty(typeDossier) && !string.IsNullOrEmpty(codeDossier) && !string.IsNullOrEmpty(user))
            {
                var (sql, param) = MakeParamsSql(@"INSERT INTO KPAS400
                                        (KHPTYP, KHPIPB, KHPALX, KHPAVN, KHPSUA, KHPNUM, KHPSBR, KHPACTG, KHPACID, KHPUSR, KHPUSED, KHPCRD, KHPCRH)
                                        VALUES (:typeDossier, :codeDossier, :iversion, :iAvenant, 0, 0, 0, :acteGestion, 0, :user, '', :date,:time)",
                                        typeDossier,
                                        codeDossier.PadLeft(9, ' '),
                                        iversion,
                                        iAvenant,
                                        string.IsNullOrEmpty(acteGestion) ? string.Empty : acteGestion,
                                        user,
                                        DateTime.Now.ToString("yyyyMMdd"),
                                        DateTime.Now.ToString("HHmmss")
                                        );
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
        }

        private static void UnlockAS400User(string codeDossier, string typeDossier, string versionDossier, string user)
        {

            codeDossier = codeDossier.ToIPB();
            int.TryParse(versionDossier, out int iversion);
            if (!string.IsNullOrEmpty(typeDossier) && !string.IsNullOrEmpty(codeDossier) && !string.IsNullOrEmpty(user))
            {
                var (sql, param) = MakeParamsSql(@"DELETE FROM KPAS400 WHERE KHPTYP = :typeDossier AND KHPIPB = :codeDossier
                                                                                  AND KHPALX = :iversion AND KHPUSR = :user", typeDossier,
                                                                                                                            codeDossier,
                                                                                                                            iversion, user);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
        }

        #endregion
        #region Appel programmes 400

        public static string ObtenirNumeroPolice(int annee, string branche, string sousBranche, string categorie, string typeContrat)
        {
            string result = null;

            DbParameter[] param = new DbParameter[9];
            string sql = @"*PGM/ALYDH002";
            param[0] = new EacParameter("P0RET", " ");
            param[1] = new EacParameter("P0TYP", typeContrat);
            param[2] = new EacParameter("P0IPB", "");
            param[3] = new EacParameter("P0ALX", 0)
            {
                Value = 0
            };
            param[4] = new EacParameter("P0EFA", 0)
            {
                Value = annee
            };
            param[5] = new EacParameter("P0BRA", branche);
            param[6] = new EacParameter("P0SBR", sousBranche);
            param[7] = new EacParameter("P0CAT", categorie);
            param[8] = new EacParameter("P0MTH", " ");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            result = param[2].Value.ToString();

            return result;
        }

        public static int GenererNumeroChrono(string paramAdr)
        {
            var param = new DbParameter[5];
            string sql = @"*PGM/ALYDH105CL";
            param[0] = new EacParameter("P0CLE", paramAdr);
            param[1] = new EacParameter("P0EXT", "***");
            param[2] = new EacParameter("P0ACT", "INC");
            param[3] = new EacParameter("P0NUU", 0)
            {
                Value = 0
            };
            param[4] = new EacParameter("P0RET", " ");
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            return (int)param[3].Value;
        }

        public static int GetAS400Id(string field)
        {
            return GetAS400Id(field, null);
        }

        /// <summary>
        /// Récupère un Id incrémenté
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static int GetAS400Id(string field, IDbConnection connection)
        {
            int result = -1;
            string pgname = @"*PGM/KA0001";
            var resultParam = new EacParameter("P0NUU", "0") { Value = 0 };
            var returnParam = new EacParameter("P0RET", " ");

            if (connection == null)
            {
                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, pgname, new[] {
                    new EacParameter("P0CLE", field),
                    new EacParameter("P0ACT", "INC"),
                    resultParam,
                    returnParam
                });
            }
            else
            {
                var options = new DbSPOptions
                {
                    DbConnection = connection,
                    Parameters = new[] {
                        new EacParameter("P0CLE", field),
                        new EacParameter("P0ACT", "INC"),
                        resultParam,
                        returnParam
                    },
                    SqlText = pgname
                };
                options.ExecStoredProcedure();
            }

            if (returnParam.Value is string val && val.Trim().Length == 0)
            {
                result = int.Parse(resultParam.Value.ToString());
            }

            return result;
        }

        public static string GetAS400IdContrat(string branche, string cible, string anneeEffet)
        {
            var param = new DbParameter[6];
            string sql = @"*PGM/KA000CL";
            param[0] = new EacParameter("P0TYP", "P");
            param[1] = new EacParameter("P0BRA", branche);
            param[2] = new EacParameter("P0CIB", cible);
            param[3] = new EacParameter("P0YEAR", anneeEffet);
            param[4] = new EacParameter("P0IPB", " ");
            param[5] = new EacParameter("P0RET", " ");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            var result = param[4].Value.ToString();

            return result;
        }

        /// <summary>
        /// Charge les données de l'engagement
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string LoadAS400Engagement(string codeOffre, string version, string type, ModeConsultation modeNavig, string codeAvenant, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, codeAvenant, acteGestion, user);
            var param = new DbParameter[4];
            string sql = @"*PGM/KA010CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", Convert.ToInt32(version));
            param[3] = new EacParameter("P0RET", " ");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
            return (string)param[3].Value;

        }

        public static string LoadAS400EngagementAvn(string codeOffre, string version, string type, string idEng, string codeAvenant, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, codeAvenant, acteGestion, user);
            var param = new DbParameter[5];
            string sql = @"*PGM/KA012CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0RET", string.Empty);
            param[4] = new EacParameter("P0ID", 0)
            {
                Value = !string.IsNullOrEmpty(idEng) ? Convert.ToInt32(idEng) : 0
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
            return (string)param[3].Value;
        }

        /// <summary>
        ///Met à jour les engagement lors de la modification de la part Albingia
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string UpdateAS400Engagement(string codeOffre, string version, string type, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, string.Empty, acteGestion, user);
            var param = new DbParameter[4];
            string sql = @"*PGM/KA011CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", Convert.ToInt32(version));
            param[3] = new EacParameter("P0RET", " ");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);

            return (string)param[3].Value;
        }

        /// <summary>
        ///Met à jour l'attentat en prenant en compte les modifications
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string UpdateAS400Attentat(string codeOffre, string version, string type, string field, ModeConsultation modeNavig, string capitalForce, string commissionForcee, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, string.Empty, acteGestion, user);
            DateTime dateNow = DateTime.Now;
            var param = new DbParameter[12];
            string sql = @"*PGM/KA020CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", Convert.ToInt32(version));
            param[3] = new EacParameter("P0ACT", field);
            param[4] = new EacParameter("P0RET", " ");
            param[5] = new EacParameter("P0TRT", string.Empty);
            param[6] = new EacParameter("P0DDA", 0)
            {
                Value = dateNow.Year
            };
            param[7] = new EacParameter("P0DDM", 0)
            {
                Value = dateNow.Month
            };
            param[8] = new EacParameter("P0DDJ", 0)
            {
                Value = dateNow.Day
            };
            param[9] = new EacParameter("P0CAPF", 0)
            {
                Value = !string.IsNullOrEmpty(capitalForce) ? Convert.ToDecimal(capitalForce) : 0
            };
            param[10] = new EacParameter("P0COMF", 0)
            {
                Value = !string.IsNullOrEmpty(commissionForcee) ? Convert.ToDecimal(commissionForcee) : 0
            };
            param[11] = new EacParameter("P0MAJ", "O");
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);

            return (string)param[4].Value;
        }

        /// <summary>
        /// Charge ou Met à jour la cotisations en prenant en compte les modifications
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="observation"></param>
        /// <returns></returns>
        public static string UpdateAS400Cotisations(string codeOffre, string version, string type, string field, string value, string oldvalue, string observation, ModeConsultation modeNavig, string codeAvn, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, codeAvn, acteGestion, user);
            var param = new DbParameter[8];
            string sql = @"*PGM/KA030CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", Convert.ToInt32(version));
            param[3] = new EacParameter("P0ACT", field);
            param[4] = new EacParameter("P0MNTC", 0)
            {
                Value = !string.IsNullOrEmpty(oldvalue) ? Convert.ToDecimal(oldvalue) : 0
            };
            param[5] = new EacParameter("P0MNTF", 0)
            {
                Value = !string.IsNullOrEmpty(value) ? Convert.ToDecimal(value) : 0
            };
            param[6] = new EacParameter("P0RET", " ");
            param[7] = new EacParameter("P0OBS", observation);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
            return (string)param[6].Value;
        }

        /// <summary>
        ///Alimente la matrice des risques
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string LoadAS400Matrice(string codeOffre, string version, string type, string codeAvn, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, codeAvn, acteGestion, user);
            var param = new DbParameter[4];
            string sql = @"*PGM/KA040CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", Convert.ToInt32(version));

            param[3] = new EacParameter("P0RET", " ");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);

            return (string)param[3].Value;
        }

        public static string LoadAS400MatriceHisto(string codeOffre, string version, string type, string avenant, string histo, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, avenant, acteGestion, user);
            var param = new DbParameter[6];
            string sql = @"*PGM/KA041CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", Convert.ToInt32(version));
            param[3] = new EacParameter("P0AVN", Convert.ToInt32(avenant));
            param[4] = new EacParameter("P0HIN", Convert.ToInt32(histo));
            param[5] = new EacParameter("P0RET", " ");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
            return (string)param[5].Value;
        }

        public static string LoadAS400ValidationOffre(string codeOffre, string version, string type, ModeConsultation modeNavig, string codeAvn, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, codeAvn, acteGestion, user);
            var param = new DbParameter[4];
            string sql = @"*PGM/KA050CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", Convert.ToInt32(version));

            param[3] = new EacParameter("P0RET", " ");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
            return (string)param[3].Value;
        }

        /// <summary>
        /// Charge les commissions standard d'un courtier
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static CommissionCourtierDto LoadAS400Commissions(string codeOffre, string version, string type, string codeAvn, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, codeAvn, acteGestion, user);
            CommissionCourtierDto toReturn;
            var param = new DbParameter[7];
            string sql = @"*PGM/KA032CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = Convert.ToInt32(version)
            };
            param[3] = new EacParameter("P0MNT", 0)
            {
                Value = 0
            };
            param[4] = new EacParameter("P0XCM", 0)
            {
                Value = 0
            };
            param[5] = new EacParameter("P0CNC", 0)
            {
                Value = 0
            };

            param[6] = new EacParameter("P0RET", " ");
            param[6] = new EacParameter("P0RET", " ");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
            if (param[6].Value.ToString() != "ERREUR")
            {
                toReturn = new CommissionCourtierDto
                {
                    Erreur = param[6].Value.ToString(),
                    TauxStandardHCAT = (double)param[4].Value,
                    TauxStandardCAT = (double)param[5].Value,
                    TauxContratHCAT = (double)param[4].Value,
                    TauxContratCAT = (double)param[5].Value,
                    IsStandardCAT = "O",
                    IsStandardHCAT = "O"
                };
            }
            else
            {
                toReturn = new CommissionCourtierDto
                {
                    Erreur = param[6].Value.ToString()
                };
            }
            return toReturn;
        }

        /// <summary>
        ///Recherche si un terme reste à émettre
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string RechercheAS400TermeEmission(string codeContrat, string version, string type, string user, string acteGestion)
        {
            LockAS400User(codeContrat, type, version, string.Empty, acteGestion, user);
            var param = new DbParameter[5];
            string sql = @"*PGM/KAE01CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = Convert.ToInt32(version)
            };
            param[3] = new EacParameter("P0RET", " ");
            param[4] = new EacParameter("P0REP", " ");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeContrat, type, version, user);
            if (param[3].Value.ToString() != "ERREUR")
            {
                return param[4].Value.ToString();
            }
            else
            {
                return (string)param[3].Value.ToString();
            }
        }

        /// <summary>
        ///Emet la prime et Recherche si un terme reste à émettre (02)
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string EmissionPrimeAS400(string codeContrat, string version, string type, string user, string acteGestion)
        {
            LockAS400User(codeContrat, type, version, string.Empty, acteGestion, user);
            var param = new DbParameter[5];
            string sql = @"*PGM/KAE02CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = Convert.ToInt32(version)
            };
            param[3] = new EacParameter("P0RET", " ");
            param[4] = new EacParameter("P0REP", " ");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeContrat, type, version, user);
            if (param[3].Value.ToString() != "ERREUR")
            {
                return param[4].Value.ToString();
            }
            else
            {
                return (string)param[3].Value.ToString();
            }
        }

        /// <summary>
        /// Récupère le code de la délégation et de l'inspecteur
        /// </summary>
        public static DelegationDto ObtenirCodeDelegationEtCodeInspecteur(int codeCabinetCourtage)
        {
            DelegationDto result = null;
            DbParameter[] param = new DbParameter[10];
            string sql = @"*PGM/YPO128";
            param[0] = new EacParameter("P0ICT", codeCabinetCourtage);
            param[1] = new EacParameter("P0CTA", codeCabinetCourtage);
            param[2] = new EacParameter("P0DEP", "");
            param[3] = new EacParameter("P0BUR", "");
            param[4] = new EacParameter("P0SEC", "");
            param[5] = new EacParameter("P0INS", "");
            param[6] = new EacParameter("P0GDP", "");
            param[7] = new EacParameter("P0GBU", "");
            param[8] = new EacParameter("P0GSE", "");
            param[9] = new EacParameter("P0GIN", "");
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            string codeDelegation = param[7].Value.ToString();
            string codeInspecteur = param[9].Value.ToString();
            string secteur = param[8].Value.ToString();


            if (!string.IsNullOrEmpty(codeInspecteur) || !string.IsNullOrEmpty(codeDelegation))
            {

                result = new DelegationDto { Code = codeDelegation };
                result.Inspecteur = new InspecteurDto { Code = codeInspecteur };
                result.Secteur = secteur;
                result.LibSecteur = GetLibSecteur(secteur);
            }
            return result;
        }

        public static DelegationDto ObtenirBureauSecteurDelegation(int codeCourtierApporteur, int codeCourtierGestionnaire)
        {
            DelegationDto result = null;
            DbParameter[] param = new DbParameter[10];
            string sql = @"*PGM/YPO128";
            param[0] = new EacParameter("P0ICT", codeCourtierGestionnaire);
            param[1] = new EacParameter("P0CTA", codeCourtierApporteur);
            param[2] = new EacParameter("P0DEP", "");
            param[3] = new EacParameter("P0BUR", "");
            param[4] = new EacParameter("P0SEC", "");
            param[5] = new EacParameter("P0INS", "");
            param[6] = new EacParameter("P0GDP", "");
            param[7] = new EacParameter("P0GBU", "");
            param[8] = new EacParameter("P0GSE", "");
            param[9] = new EacParameter("P0GIN", "");
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            string bureauApporteur = param[3].Value.ToString();
            string secteurApporteur = param[4].Value.ToString();
            string bureauGestion = param[7].Value.ToString();
            string secteurGestion = param[8].Value.ToString();

            result = new DelegationDto
            {
                SecteurApporteur = GetLibSecteur(secteurApporteur),
                SecteurGestionnaire = GetLibSecteur(secteurGestion),
                DelegationApporteur = GetLibBureau(bureauApporteur),
                DelegationGestionnaire = GetLibBureau(bureauGestion)
            };

            return result;

        }

        private static string GetLibSecteur(string codeSecteur)
        {
            string sql = string.Format(@"SELECT ABHDES STRRETURNCOL FROM YSECTEU WHERE ABHSEC = '{0}'", codeSecteur);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().StrReturnCol;
            }

            return string.Empty;
        }

        private static string GetLibBureau(string codeBureau)
        {
            string sql = string.Format(@"SELECT BUDBU LIBELLE FROM YBUREAU
                                          WHERE BUIBU = '{0}'", codeBureau);
            var result = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().Libelle;
            }

            return null;
        }

        /// <summary>
        /// Récupère les frais standards
        /// </summary>
        public static int GetFraisStandard(string codeOffre, string versionOffre, string typeOffre, int anneeEffet, string codeAvn, string user, string acteGestion)
        {
            var callPGM400 = "1";//FileContentManager.GetConfigValue("CallPGM400");

            if (Convert.ToInt32(callPGM400) == 0)
            {
                return GetFraisStandard_Kheo(codeOffre, versionOffre, typeOffre, anneeEffet, codeAvn, user, acteGestion);
            }

            LockAS400User(codeOffre, typeOffre, versionOffre, codeAvn, acteGestion, user);
            var param = new DbParameter[8];
            string sql = @"*PGM/KA031CL";
            param[0] = new EacParameter("P0TYP", typeOffre);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = Convert.ToInt32(versionOffre)
            };
            param[3] = new EacParameter("P0DPA", 0)
            {
                Value = anneeEffet
            };
            param[4] = new EacParameter("P0MNT", 0)
            {
                Value = 0
            };
            param[5] = new EacParameter("P0TOP", "N")
            {
                Value = 0
            };
            param[6] = new EacParameter("P0AFR", 0)
            {
                Value = 0
            };
            param[7] = new EacParameter("P0RET", "");
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, typeOffre, versionOffre, user);
            if (string.IsNullOrEmpty(param[7].Value.ToString()))
            {
                return int.Parse(param[6].Value.ToString());
            }
            //Vu avec FDU 23-01-2017 : retourner 0 si erreur
            return 0;
            //return -1;
        }

        /// <summary>
        /// Réinitialise les montants références
        /// </summary>
        public static string InitMontantRef(string codeOffre, string version, string type, string action, ModeConsultation modeNavig, string codeAvn, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, codeAvn, acteGestion, user);
            var param = new DbParameter[6];
            string sql = @"*PGM/KA035CL";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = Convert.ToInt32(version)
            };
            param[3] = new EacParameter("P0ACT", action);
            param[4] = new EacParameter("P0TOP", "");
            param[5] = new EacParameter("P0RET", "");
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
            if (string.IsNullOrEmpty(param[5].Value.ToString()))
            {
                return param[4].Value.ToString();
            }

            return param[5].Value.ToString();
        }

        /// <summary>
        /// Charge le préavis de résiliation
        /// </summary>
        public static string LoadPreavisResiliation(string codeContrat, string version, string codeAvn, DateTime? dateEffet, DateTime? dateFinEffet, DateTime? dateAvenant, string periodicite, DateTime? echeancePrincipale, string splitCharHtml, string user, string acteGestion)
        {
            LockAS400User(codeContrat, "P", version, string.Empty, acteGestion, user);
            var param = new DbParameter[26];
            string sql = @"*PGM/YDP100";

            param[0] = new EacParameter("P0RET", "");
            param[1] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = Convert.ToInt32(version)
            };
            param[3] = new EacParameter("P0AVN", 0)
            {
                Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0
            };
            param[4] = new EacParameter("P0HIS", "C");
            param[5] = new EacParameter("P0EFJ", 0)
            {
                Value = dateEffet != null ? dateEffet.Value.Day : 0
            };
            param[6] = new EacParameter("P0EFM", 0)
            {
                Value = dateEffet != null ? dateEffet.Value.Month : 0
            };
            param[7] = new EacParameter("P0EFA", 0)
            {
                Value = dateEffet != null ? dateEffet.Value.Year : 0
            };
            param[8] = new EacParameter("P0FEJ", 0)
            {
                Value = dateFinEffet != null ? dateFinEffet.Value.Day : 0
            };
            param[9] = new EacParameter("P0FEM", 0)
            {
                Value = dateFinEffet != null ? dateFinEffet.Value.Month : 0
            };
            param[10] = new EacParameter("P0FEA", 0)
            {
                Value = dateFinEffet != null ? dateFinEffet.Value.Year : 0
            };
            param[11] = new EacParameter("P0AVJ", 0)
            {
                Value = dateAvenant != null ? dateAvenant.Value.Day : 0
            };
            param[12] = new EacParameter("P0AVM", 0)
            {
                Value = dateAvenant != null ? dateAvenant.Value.Month : 0
            };
            param[13] = new EacParameter("P0AVA", 0)
            {
                Value = dateAvenant != null ? dateAvenant.Value.Year : 0
            };
            param[14] = new EacParameter("P0PER", periodicite);
            param[15] = new EacParameter("P0ECM", "")
            {
                Value = echeancePrincipale != null ? echeancePrincipale.Value.Month : 0
            };
            param[16] = new EacParameter("P0ECJ", "")
            {
                Value = echeancePrincipale != null ? echeancePrincipale.Value.Day : 0
            };
            param[17] = new EacParameter("P0DPJ", "")
            {
                Value = 0
            };
            param[18] = new EacParameter("P0DPM", "")
            {
                Value = 0
            };
            param[19] = new EacParameter("P0DPA", "")
            {
                Value = 0
            };
            param[20] = new EacParameter("P0FPJ", "")
            {
                Value = 0
            };
            param[21] = new EacParameter("P0FPM", "")
            {
                Value = 0
            };
            param[22] = new EacParameter("P0FPA", "")
            {
                Value = 0
            };
            param[23] = new EacParameter("P0PEJ", "")
            {
                Value = 0
            };
            param[24] = new EacParameter("P0PEM", "")
            {
                Value = 0
            };
            param[25] = new EacParameter("P0PEA", "")
            {
                Value = 0
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeContrat, "P", version, user);
            if (string.IsNullOrEmpty(param[0].Value.ToString()))
            {
                var periodDeb = param[17].Value.ToString().PadLeft(2, '0') + "/" + param[18].Value.ToString().PadLeft(2, '0') + "/" + param[19].Value.ToString();
                var periodFin = param[20].Value.ToString().PadLeft(2, '0') + "/" + param[21].Value.ToString().PadLeft(2, '0') + "/" + param[22].Value.ToString();
                var echeance = (param[23].Value.ToString() != "0" && param[24].Value.ToString() != "0" && param[25].Value.ToString() != "0") ? param[23].Value.ToString().PadLeft(2, '0') + "/" + param[24].Value.ToString().PadLeft(2, '0') + "/" + param[25].Value.ToString() : string.Empty;
                return periodDeb + splitCharHtml + periodFin + splitCharHtml + echeance;
            }
            return param[0].Value.ToString();
        }

        /// <summary>
        /// Controle l'echeance renseignee
        /// </summary>
        public static string ControleEcheance(string prochaineEcheance, string periodicite, string echeancePrincipale)
        {
            var dProchaineEcheance = AlbConvert.ConvertStrToDate(prochaineEcheance);
            DateTime? dEcheancePrincipale = null;
            if (!string.IsNullOrEmpty(echeancePrincipale))
            {
                dEcheancePrincipale = AlbConvert.ConvertStrToDate(echeancePrincipale + "/2012");
            }

            var param = new DbParameter[7];
            string sql = @"*PGM/YDP101";
            param[0] = new EacParameter("P0RET", "");
            param[1] = new EacParameter("P0PEJ", 0)
            {
                Value = dProchaineEcheance != null ? dProchaineEcheance.Value.Day : 0
            };
            param[2] = new EacParameter("P0PEM", 0)
            {
                Value = dProchaineEcheance != null ? dProchaineEcheance.Value.Month : 0
            };
            param[3] = new EacParameter("P0PEA", 0)
            {
                Value = dProchaineEcheance != null ? dProchaineEcheance.Value.Year : 0
            };
            param[4] = new EacParameter("P0PER", periodicite);
            param[5] = new EacParameter("P0ECM", "")
            {
                Value = dEcheancePrincipale != null ? dEcheancePrincipale.Value.Month : 0
            };
            param[6] = new EacParameter("P0ECJ", "")
            {
                Value = dEcheancePrincipale != null ? dEcheancePrincipale.Value.Day : 0
            };
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            if (string.IsNullOrEmpty(param[0].Value.ToString()))
            {
                return string.Empty;
            }

            return "Erreur : date prochaine échéance invalide";
        }

        /// <summary>
        /// Calcul Affaire Nouvelle
        /// </summary>
        public static string CalculAffaireNouvelle(string codeContrat, string version, string codeAvn, string user, string acteGestion, string attentatON, decimal fraisAccessoire = 0, bool updateacc = false)
        {
            var result = new List<FraisAccessoiresDto>();
            string typeFrais = string.Empty;
            string appliqueAtt = string.Empty;
            if (updateacc == false)
            {
                string sqlInfo = string.Format(@"SELECT PBTTR TYPETRAITEMENT, JDAFC TYPEFRAIS, CASE WHEN IFNULL(PKIPB,'') = '' THEN JDAFR
                 ELSE PKAFR END FRAISRETENUS, JDATT TAXEATTENTAT, IFNULL(PKIPB,'') HASPRIPES, PKATM TAXEATTENTATVALUE,
                         IFNULL(PKAFR,0) FRAISRETPRIPES
                                    FROM YPRTENT
                                        INNER JOIN YPOBASE ON PBIPB = JDIPB AND PBALX = JDALX
                                        LEFT JOIN {2} ON PKIPB = PBIPB AND PKALX = PBALX AND PBAVN = PKAVN
                                    WHERE JDIPB = '{0}' AND JDALX = {1}",
                                codeContrat.PadLeft(9, ' '), version,
                                GetSuffixeRegule(acteGestion, "YPRIPES"));

                result = DbBase.Settings.ExecuteList<FraisAccessoiresDto>(CommandType.Text, sqlInfo);

                appliqueAtt = string.IsNullOrEmpty(result.FirstOrDefault().HasPripes) ? "N" : result.FirstOrDefault().TaxeAttentatValue != 0 ? "O" : "N";
                typeFrais = string.IsNullOrEmpty(result.FirstOrDefault().HasPripes) ? result.FirstOrDefault().TypeFrais : result.FirstOrDefault().FraisRetenus != 0 ? "O" : "N";
            }
            LockAS400User(codeContrat, "P", version, codeAvn, acteGestion, user);
            DbParameter[] param = new DbParameter[7];
            string sql = @"*PGM/KDP192CL";
            param[0] = new EacParameter("P0RET", "");
            param[1] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0AFC", updateacc ? (fraisAccessoire != 0 ? "O" : "N") : typeFrais);
            param[4] = new EacParameter("P0AFR", 0)
            {
                Value = updateacc == true ? fraisAccessoire : result.FirstOrDefault().FraisRetenus
            };
            param[5] = new EacParameter("P0ATT", updateacc ? attentatON : appliqueAtt);
            param[6] = new EacParameter("P0AFV", 0)
            {
                Value = 0
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeContrat, "P", version, user);
            if (string.IsNullOrEmpty(param[0].Value.ToString()))
            {
                return string.Empty;
            }

            return "Erreur lors du calcul.";
        }

        /// <summary>
        ///Calcul sortie calcul forcé
        /// </summary>
        public static string CalculFinCalculForce(string code, string version, string codeAvn, string user, string acteGestion)
        {
            string sqlInfo = string.Format(@"SELECT IFNULL(NULLIF(TRIM(JDATT),''),'N') TAXEATTENTAT
                                             FROM YPRTENT
                                             WHERE JDIPB = '{0}' AND JDALX = {1}",
                                    code.PadLeft(9, ' '), version);
            var result = DbBase.Settings.ExecuteList<FraisAccessoiresDto>(CommandType.Text, sqlInfo);
            var fga = string.Empty;
            if (result != null && result.Any())
            {
                fga = result.FirstOrDefault().AppliqueTaxeAttentat;
            }

            LockAS400User(code, "P", version, codeAvn, acteGestion, user);
            DbParameter[] param = new DbParameter[4];
            string sql = @"*PGM/KDP196CL";
            param[0] = new EacParameter("P0RET", "")
            {
                Value = string.Empty
            };
            param[1] = new EacParameter("P0IPB", code.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0ATT", fga);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(code, "P", version, user);
            if (string.IsNullOrEmpty(param[0].Value.ToString()))
            {
                return string.Empty;
            }

            return "Erreur lors du calcul.";
        }

        //SAB: 09/10/2015 : calculavenant
        public static string CalculAvenant(string codeContrat, string version, string codeAvn, string user, string acteGestion, string isFGACocheIHM, decimal fraisAccessoire = 0, bool updateaccavn = false)
        {
            var result = new List<FraisAccessoiresDto>();
            string typefais = string.Empty;
            string appliqueAtt = string.Empty;
            if (updateaccavn == false)
            {
                string sqlInfo = string.Format(@"SELECT PBTTR TYPETRAITEMENT, JDAFC TYPEFRAIS, CASE WHEN IFNULL(PKIPB,'') = '' THEN JDAFR
                 ELSE PKAFR END FRAISRETENUS, JDATT TAXEATTENTAT, IFNULL(PKIPB,'') HASPRIPES, PKATM TAXEATTENTATVALUE,
                         IFNULL(PKAFR,0) FRAISRETPRIPES
                                    FROM YPRTENT
                                        INNER JOIN YPOBASE ON PBIPB = JDIPB AND PBALX = JDALX
                                        LEFT JOIN {2} ON PKIPB = PBIPB AND PKALX = PBALX AND PBAVN = PKAVN
                                    WHERE JDIPB = '{0}' AND JDALX = {1}",
                                codeContrat.PadLeft(9, ' '), version,
                                GetSuffixeRegule(acteGestion, "YPRIPES"));

                result = DbBase.Settings.ExecuteList<FraisAccessoiresDto>(CommandType.Text, sqlInfo);

                appliqueAtt = string.IsNullOrEmpty(result.FirstOrDefault().HasPripes) ? "N" : result.FirstOrDefault().TaxeAttentatValue != 0 ? "O" : "N";
                //if (!string.IsNullOrEmpty(isFGACocheIHM))
                //    appliqueAtt = isFGACocheIHM;
                typefais = string.IsNullOrEmpty(result.FirstOrDefault().HasPripes) ? result.FirstOrDefault().TypeFrais : result.FirstOrDefault().FraisRetenus != 0 ? "O" : "N";
            }

            LockAS400User(codeContrat, "P", version, codeAvn, acteGestion, user);
            DbParameter[] param = new DbParameter[10];
            string sql = @"*PGM/KDA192CL";
            param[0] = new EacParameter("P0RET", "");
            param[1] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0AVN", 0)
            {
                Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0
            };
            param[4] = new EacParameter("P0ACO", "O");
            param[5] = new EacParameter("P0TTR", updateaccavn == true ? isFGACocheIHM : result.FirstOrDefault().TypeTraitement);
            param[6] = new EacParameter("P0AFC", updateaccavn == true ? (fraisAccessoire != 0 ? "O" : "N") : typefais);
            param[7] = new EacParameter("P0AFR", 0)
            {
                Value = updateaccavn == true ? fraisAccessoire : result.FirstOrDefault().FraisRetenus
            };
            param[8] = new EacParameter("P0ATT", updateaccavn == true ? isFGACocheIHM : appliqueAtt);
            param[9] = new EacParameter("P0AFV", 0)
            {
                Value = 0
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeContrat, "P", version, user);
            if (string.IsNullOrEmpty(param[0].Value.ToString()))
            {
                return string.Empty;
            }

            return "Erreur lors du calcul.";
        }


        /// <summary>
        /// Alimente les statistiques pour l'affaire nouvelle
        /// et les montants références
        /// </summary>
        public static string AlimStatistiques(string codeContrat, string version, string user, string acteGestion, string selection = "N")
        {
            DateTime? dEffet = null;
            string sqlEffet = string.Format(@"SELECT PBEFA DATEEFFETA, PBEFM DATEEFFETM, PBEFJ DATEEFFETJ FROM YPOBASE WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'",
                                        codeContrat.ToIPB(), version, "P");
            var result = DbBase.Settings.ExecuteList<ContratDto>(CommandType.Text, sqlEffet);
            if (result != null && result.Any())
            {
                var firstResult = result.FirstOrDefault();
                dEffet = new DateTime(firstResult.DateEffetAnnee, firstResult.DateEffetMois, firstResult.DateEffetJour);
            }
            LockAS400User(codeContrat, "P", version, string.Empty, acteGestion, user);
            DbParameter[] param = new DbParameter[7];
            string sql = @"*PGM/KDP021CL";
            param[0] = new EacParameter("P0SEL", selection);
            param[1] = new EacParameter("P0RET", "");
            param[2] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[3] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[4] = new EacParameter("P0EFA", 0)
            {
                Value = dEffet != null ? dEffet.Value.Year : 0
            };
            param[5] = new EacParameter("P0EFM", 0)
            {
                Value = dEffet != null ? dEffet.Value.Month : 0
            };
            param[6] = new EacParameter("P0EFJ", 0)
            {
                Value = dEffet != null ? dEffet.Value.Day : 0
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeContrat, "P", version, user);
            if (string.IsNullOrEmpty(param[1].Value.ToString()))
            {
                return string.Empty;
            }

            return "Erreur lors l'alimentation.";
        }

        /// <summary>
        /// Récupère la période d'avenant
        /// </summary>
        public static string GetPeriodeAvenantAS400(string codeOffre, string version, int avenant, DateTime dateEffetAvenant, out DateTime? dateDebutPeriode, out DateTime? dateFinPeriode, string user, string acteGestion)
        {
            LockAS400User(codeOffre, "P", version, avenant.ToString(), acteGestion, user);
            var param = new DbParameter[14];
            string sql = @"*PGM/YDA970";
            param[0] = new EacParameter("P0RET", "");
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", "0") { Value = int.Parse(version) };
            param[3] = new EacParameter("P0AVN", "0") { Value = avenant };
            param[4] = new EacParameter("P0AVA", "0") { Value = dateEffetAvenant.Year };
            param[5] = new EacParameter("P0AVM", "0") { Value = dateEffetAvenant.Month };
            param[6] = new EacParameter("P0AVJ", "0") { Value = dateEffetAvenant.Day };
            param[7] = new EacParameter("P0CAS", "");
            param[8] = new EacParameter("P0DPA", "0") { Value = 0 };
            param[9] = new EacParameter("P0DPM", "0") { Value = 0 };
            param[10] = new EacParameter("P0DPJ", "0") { Value = 0 };
            param[11] = new EacParameter("P0FPA", "0") { Value = 0 };
            param[12] = new EacParameter("P0FPM", "0") { Value = 0 };
            param[13] = new EacParameter("P0FPJ", "0") { Value = 0 };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, "P", version, user);
            var result = param[0].Value.ToString();

            if (string.IsNullOrEmpty(result))
            {
                if (!string.IsNullOrEmpty(param[8].Value.ToString()) && param[8].Value.ToString() != "0" && !string.IsNullOrEmpty(param[9].Value.ToString()) && param[9].Value.ToString() != "0" && !string.IsNullOrEmpty(param[10].Value.ToString()) && param[10].Value.ToString() != "0")
                {
                    dateDebutPeriode = new DateTime(int.Parse(param[8].Value.ToString()), int.Parse(param[9].Value.ToString()), int.Parse(param[10].Value.ToString()));
                }
                else
                {
                    dateDebutPeriode = null;
                }

                if (!string.IsNullOrEmpty(param[11].Value.ToString()) && param[11].Value.ToString() != "0" && !string.IsNullOrEmpty(param[12].Value.ToString()) && param[12].Value.ToString() != "0" && !string.IsNullOrEmpty(param[13].Value.ToString()) && param[13].Value.ToString() != "0")
                {
                    dateFinPeriode = new DateTime(int.Parse(param[11].Value.ToString()), int.Parse(param[12].Value.ToString()), int.Parse(param[13].Value.ToString()));
                }
                else
                {
                    dateFinPeriode = null;
                }
            }
            else
            {
                dateDebutPeriode = null;
                dateFinPeriode = null;
            }

            return result;
        }

        public static string UpdateCalculForce(string codeOffre, string type, string version, string avenant, string codeRsq, string codeFor, string montantCal, string montantFor, string coeff,
                string aco, string tyc, string httc, string cas, string maj, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, avenant, acteGestion, user);
            double.TryParse(montantCal, out double montantCal2Decimal);
            double.TryParse(montantFor, out double montantFor2Decimal);
            montantCal2Decimal = Math.Round(montantCal2Decimal, 2);
            montantFor2Decimal = Math.Round(montantFor2Decimal, 2);

            var param = new DbParameter[14];
            string sql = @"*PGM/KDP196";
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0AVN", 0)
            {
                Value = !string.IsNullOrEmpty(avenant) ? Convert.ToInt32(avenant) : 0
            };
            param[4] = new EacParameter("P0ACO", aco);
            param[5] = new EacParameter("P0TYC", tyc);
            param[6] = new EacParameter("P0RSQ", 0)
            {
                Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0
            };
            param[7] = new EacParameter("P0FOR", 0)
            {
                Value = !string.IsNullOrEmpty(codeFor) ? Convert.ToInt32(codeFor) : 0
            };
            param[8] = new EacParameter("P0MNTC", 0)
            {
                Value = Convert.ToDecimal(montantCal2Decimal)
            };
            param[9] = new EacParameter("P0MNTF", 0)
            {
                Value = Convert.ToDecimal(montantFor2Decimal)
            };
            param[10] = new EacParameter("P0HTTC", httc);
            param[11] = new EacParameter("P0COEF", 0)
            {
                Value = !string.IsNullOrEmpty(coeff) ? Convert.ToDecimal(coeff) : 0
            };
            param[12] = new EacParameter("P0CAS", cas);
            param[13] = new EacParameter("P0MAJ", maj);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
            return param[0].Value.ToString();
        }

        public static string UpdateCalculMntForce(string codeOffre, string type, string version, string avenant, string aco, string codeRsq, string codeFor,
               string montantFor, string httc, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, avenant, acteGestion, user);
            double.TryParse(montantFor, out double montantFor2Decimal);
            montantFor2Decimal = Math.Round(montantFor2Decimal, 2);

            var param = new DbParameter[9];
            string sql = @"*PGM/KDP197";
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0AVN", 0)
            {
                Value = !string.IsNullOrEmpty(avenant) ? Convert.ToInt32(avenant) : 0
            };
            param[4] = new EacParameter("P0ACO", aco);
            param[5] = new EacParameter("P0RSQ", 0)
            {
                Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0
            };
            param[6] = new EacParameter("P0FOR", 0)
            {
                Value = !string.IsNullOrEmpty(codeFor) ? Convert.ToInt32(codeFor) : 0
            };
            param[7] = new EacParameter("P0MNTF", 0)
            {
                Value = Convert.ToDecimal(montantFor2Decimal)
            };
            param[8] = new EacParameter("P0HTTC", httc);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
            return param[0].Value.ToString();
        }
        public static string UpdateCalculForceRegule(string codeOffre, string type, string version, string avenant, string codeRsq, string codeFor, string montantCal, string montantFor, string coeff,
            string tyc, string httc, string cas, string user, string acteGestion, string reguleId)
        {
            //TODO:PGM
            LockAS400User(codeOffre, type, version, avenant, acteGestion, user);
            double.TryParse(montantCal, out double montantCal2Decimal);
            double.TryParse(montantFor, out double montantFor2Decimal);
            montantCal2Decimal = Math.Round(montantCal2Decimal, 2);
            montantFor2Decimal = Math.Round(montantFor2Decimal, 2);

            DbParameter[] param = new DbParameter[12];
            string sql = @"*PGM/KDA327CL";
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0TYC", tyc);
            param[4] = new EacParameter("P0RSQ", 0)
            {
                Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0
            };
            param[5] = new EacParameter("P0FOR", 0)
            {
                Value = !string.IsNullOrEmpty(codeFor) ? Convert.ToInt32(codeFor) : 0
            };
            param[6] = new EacParameter("P0MNTC", 0)
            {
                Value = Convert.ToDecimal(montantCal2Decimal)
            };
            param[7] = new EacParameter("P0MNTF", 0)
            {
                Value = Convert.ToDecimal(montantFor2Decimal)
            };
            param[8] = new EacParameter("P0HTTC", httc);
            param[9] = new EacParameter("P0COEF", 0)
            {
                Value = !string.IsNullOrEmpty(coeff) ? Convert.ToDecimal(coeff) : 0
            };
            param[10] = new EacParameter("P0CAS", cas);
            param[11] = new EacParameter("P0ID", 0)
            {
                Value = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0
            };
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
            return string.Empty;
        }

        public static string GenerationQuittanceComptable(string codeOffre, string type, string version, string avenant, string acteGestion, string user)
        {
            LockAS400User(codeOffre, type, version, avenant, acteGestion, user);
            DbParameter[] param = new DbParameter[6];
            string sql = @"*PGM/KPR290CL";
            param[0] = new EacParameter("P0RET", "");
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0AVN", 0)
            {
                Value = !string.IsNullOrEmpty(avenant) ? Convert.ToInt32(avenant) : 0
            };
            param[4] = new EacParameter("P0ACTG", acteGestion);
            param[5] = new EacParameter("P0IPK", 0)
            {
                Value = 0
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);

            if (string.IsNullOrEmpty(param[0].Value.ToString()))
            {
                return param[5].Value.ToString();
            }
            return string.Empty;
        }

        public static string GenerationPrimeRegule(string codeOffre, string type, string version, string avenant, string acteGestion, string user)
        {
            //TODO:PGM
            LockAS400User(codeOffre, type, version, avenant, acteGestion, user);
            DbParameter[] param = new DbParameter[9];
            string sql = @"*PGM/KDA325CL";
            param[0] = new EacParameter("P0RET", "");
            param[1] = new EacParameter("P0TYP", type);
            param[2] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[3] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[4] = new EacParameter("P0AVN", 0)
            {
                Value = !string.IsNullOrEmpty(avenant) ? Convert.ToInt32(avenant) : 0
            };
            param[5] = new EacParameter("P0ACO", "O");
            param[6] = new EacParameter("P0ANN", "N");
            param[7] = new EacParameter("P0DMD", "N");
            param[8] = new EacParameter("P0IPK", 0)
            {
                Value = 0
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);

            if (string.IsNullOrEmpty(param[0].Value.ToString()))
            {
                return param[8].Value.ToString();
            }
            return string.Empty;
        }

        public static string LancementCalculAffNouv(string codeContrat, string versionContrat, string typeContrat)
        {
            //TODO:PGM
            codeContrat = codeContrat.ToIPB();
            string sqlInfo = string.Format(@"SELECT PBEFA DATEEFFETA, PBEFM DATEEFFETM, PBEFJ DATEEFFETJ, JDAFR FRAISACC, JDATT ATTENTAT,JDAFC APPLICATIONACC,JDAFR MONTANTFRAIS
                                            FROM YPOBASE
                                            INNER JOIN YPRTENT ON JDIPB = PBIPB AND JDALX = PBALX
                                            WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'",
                            codeContrat, versionContrat, typeContrat);
            var result = DbBase.Settings.ExecuteList<OffreInfoDto>(CommandType.Text, sqlInfo);
            if (result != null && result.Any())
            {
                var firstRes = result.FirstOrDefault();
                DbParameter[] param = new DbParameter[12];
                string sql = @"*PGM/KDP025CL";
                param[0] = new EacParameter("P0TYP", typeContrat);
                param[1] = new EacParameter("P0IPB", codeContrat);
                param[2] = new EacParameter("P0ALX", 0)
                {
                    Value = !string.IsNullOrEmpty(versionContrat) ? Convert.ToInt16(versionContrat) : 0
                };
                param[3] = new EacParameter("P0ACT", "CALCUL");
                param[4] = new EacParameter("P0EFA", 0)
                {
                    Value = firstRes.DateEffetAnnee
                };
                param[5] = new EacParameter("P0EFM", 0)
                {
                    Value = firstRes.DateEffetMois
                };
                param[6] = new EacParameter("P0EFJ", 0)
                {
                    Value = firstRes.DateEffetJour
                };
                param[7] = new EacParameter("P0AFV", 0)
                {
                    Value = firstRes.FraisAccessoires
                };
                param[8] = new EacParameter("P0ATT", firstRes.Attentat);
                param[9] = new EacParameter("P0RET", "");
                param[10] = new EacParameter("P0AFC", firstRes.ApplicationAcc);
                param[11] = new EacParameter("P0AFR", 0)
                {
                    Value = firstRes.MontantFrais
                };

                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
                return param[9].Value.ToString();
            }
            return string.Empty;
        }

        public static string EmettreTermes(string codeContrat, string versionContrat, string typeContrat)
        {
            DbParameter[] param = new DbParameter[3];
            string sql = @"*PGM/YDP340CL";
            param[0] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[1] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(versionContrat) ? Convert.ToInt16(versionContrat) : 0
            };
            param[2] = new EacParameter("P0ERR", typeContrat);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            return param[2].Value.ToString();

        }

        /// <summary>
        /// Appelé avant les cotisations (Offre ou AN)
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <param name="acteGestion"></param>
        public static void AvantCotisationAS400(string codeOffre, string version, string type, string codeAvn, string acteGestion, string user)
        {
            LockAS400User(codeOffre, type, version, codeAvn, acteGestion, user);
            var param = new DbParameter[4];
            string sql = @"*PGM/KDP025CL1";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", Convert.ToInt32(version));
            param[3] = new EacParameter("P0ACT", acteGestion);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            UnlockAS400User(codeOffre, type, version, user);
        }

        /// <summary>
        /// Calcul YAPRMAN, sur quitte offre en gestion ou validation
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <param name="acteGestion"></param>
        public static string CalculYAPRMANAS400(string codeOffre, string version, string type, string codeAvn, string acteGestion, string user)
        {
            LockAS400User(codeOffre, type, version, codeAvn, acteGestion, user);
            var param = new DbParameter[4];
            string sql = @"*PGM/KDP791CL";
            param[0] = new EacParameter("P0RET", " ");
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", Convert.ToInt32(version));
            try
            {
                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            }
            catch (Exception)
            {
                return "Anomalie dans le calcul YAPRMAN – voir référent  de gestion";
            }
            finally
            {
                UnlockAS400User(codeOffre, type, version, user);
            }

            return param[0].Value.ToString();
        }

        /// <summary>
        /// Permet de savoir si la saisie est possible
        /// </summary>
        /// <returns></returns>
        public static BlocageSaisieDto GetBlocageSaisieAS400()
        {
            BlocageSaisieDto toReturn = new BlocageSaisieDto();
            var param = new DbParameter[3];
            string sql = @"*PGM/OUVERTUREK";
            param[0] = new EacParameter("P0PGOKSESS", "");
            param[1] = new EacParameter("P0REPHH", "");
            param[2] = new EacParameter("P0REPMN", "");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            toReturn.Etat = param[0].Value.ToString();
            toReturn.HeureReprise = param[1].Value.ToString();
            toReturn.MinuteReprise = param[2].Value.ToString();
            return toReturn;
        }

        public static string AvenantResilControleTypeDate(string codeOffre, string version, Int16 codeAvn, string typeAvt, string modeAvt)
        {
            string sql = @"*PGM/KDA19CL";

            DbParameter[] param = new DbParameter[9];
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0AVN", 0)
            {
                Value = Convert.ToInt32(codeAvn)
            };
            param[4] = new EacParameter("P0TTR", typeAvt);
            param[5] = new EacParameter("P0LIB", string.Empty);
            param[6] = new EacParameter("P0ECH", "N");
            param[7] = new EacParameter("P0DAT", string.Empty);
            if (modeAvt == "CREATE")
            {
                param[8] = new EacParameter("P0ECO", "E");
            }
            else
            {
                param[8] = new EacParameter("P0ECO", "H");
            }

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            if (!string.IsNullOrEmpty(param[0].Value.ToString()))
            {
                return string.Format("{0}-{1}", param[0].Value.ToString(), param[5].Value.ToString());
            }
            else
            {
                return string.Format("{0}-{1}", param[6].Value.ToString(), param[7].Value.ToString());
            }
            //List<ParametreDto> toReturn = new List<ParametreDto>();
            //toReturn.Add(new ParametreDto { Code = "24/12/2016", Libelle = "24/12/2016", Descriptif = "24/12/2016" });
            //toReturn.Add(new ParametreDto { Code = "31/12/2018", Libelle = "31/12/2018", Descriptif = "31/12/2018" });
            //return toReturn;
        }

        public static string UpdateTableReassu(string codeOffre, string version, string type)
        {
            string sql = @"*PGM/KA038CL";

            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0FIN", string.Empty);
            param[4] = new EacParameter("P0RET", string.Empty);
            param[5] = new EacParameter("P0REC", string.Empty);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            if (!string.IsNullOrEmpty(param[4].Value.ToString()))
            {
                return string.Format("{0} - {1}", param[4].Value.ToString(), param[5].Value.ToString());
            }

            return string.Empty;
        }

        /// <summary>
        /// Effectue un contrôle de la période et des variations dans celle-ci
        /// (Nature, Coassurance, Courtier ...)
        /// </summary>
        public static ModeleControlPeriodeDto ControlePeriode(string codeContrat, string version, int exercice, DateTime? periodeDeb, DateTime? periodeFin, string origine = "")
        {
            string sql = @"*PGM/KDA300CL";

            #region Déclaration des paramètres

            DbParameter[] param = new DbParameter[21];
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0EXE", 0)
            {
                Value = exercice
            };
            param[4] = new EacParameter("P0DPA", 0)
            {
                Value = periodeDeb.HasValue ? periodeDeb.Value.Year : 0
            };
            param[5] = new EacParameter("P0DPM", 0)
            {
                Value = periodeDeb.HasValue ? periodeDeb.Value.Month : 0
            };
            param[6] = new EacParameter("P0DPJ", 0)
            {
                Value = periodeDeb.HasValue ? periodeDeb.Value.Day : 0
            };
            param[7] = new EacParameter("P0FPA", 0)
            {
                Value = periodeFin.HasValue ? periodeFin.Value.Year : 0
            };
            param[8] = new EacParameter("P0FPM", 0)
            {
                Value = periodeFin.HasValue ? periodeFin.Value.Month : 0
            };
            param[9] = new EacParameter("P0FPJ", 0)
            {
                Value = periodeFin.HasValue ? periodeFin.Value.Day : 0
            };
            param[10] = new EacParameter("P0ERR", string.Empty);
            param[11] = new EacParameter("P0DAA", 0)
            {
                Value = 0
            };
            param[12] = new EacParameter("P0DAM", 0)
            {
                Value = 0
            };
            param[13] = new EacParameter("P0DAJ", 0)
            {
                Value = 0
            };
            param[14] = new EacParameter("P0DAV", 0)
            {
                Value = 0
            };
            param[15] = new EacParameter("P0ENC", string.Empty);
            param[16] = new EacParameter("P0ICT", 0)
            {
                Value = 0
            };
            param[17] = new EacParameter("P0DFI", string.Empty);
            param[18] = new EacParameter("P0AVK", 0)
            {
                Value = 0
            };
            param[19] = new EacParameter("P0ICC", 0)
            {
                Value = 0
            };
            param[20] = new EacParameter("P0ORI", origine);

            #endregion

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            DateTime? periodeDebRet = null;
            DateTime? periodeFinRet = null;
            if (!string.IsNullOrEmpty(param[4].Value.ToString()) && param[4].Value.ToString() != "0"
                && !string.IsNullOrEmpty(param[5].Value.ToString()) && param[5].Value.ToString() != "0"
                && !string.IsNullOrEmpty(param[6].Value.ToString()) && param[6].Value.ToString() != "0")
            {
                periodeDebRet = new DateTime(int.Parse(param[4].Value.ToString()), int.Parse(param[5].Value.ToString()), int.Parse(param[6].Value.ToString()));
            }

            if (!string.IsNullOrEmpty(param[7].Value.ToString()) && param[7].Value.ToString() != "0"
                && !string.IsNullOrEmpty(param[8].Value.ToString()) && param[8].Value.ToString() != "0"
                && !string.IsNullOrEmpty(param[9].Value.ToString()) && param[9].Value.ToString() != "0")
            {
                periodeFinRet = new DateTime(int.Parse(param[7].Value.ToString()), int.Parse(param[8].Value.ToString()), int.Parse(param[9].Value.ToString()));
            }

            ModeleControlPeriodeDto model = new ModeleControlPeriodeDto
            {
                CodeErreur = param[10].Value.ToString(),
                PeriodeDeb = periodeDebRet.HasValue ? periodeDebRet.Value.ToString("dd/MM/yyyy") : "0",
                PeriodeFin = periodeFinRet.HasValue ? periodeFinRet.Value.ToString("dd/MM/yyyy") : "0",
                CodeCourtierPayeur = !string.IsNullOrEmpty(param[16].Value.ToString()) ? Convert.ToInt64(param[16].Value.ToString()) : 0,
                CodeCourtierCommission = !string.IsNullOrEmpty(param[19].Value.ToString()) ? Convert.ToInt64(param[19].Value.ToString()) : 0,
                CodeEncaissement = param[15].Value.ToString(),
                DernierAvn = !string.IsNullOrEmpty(param[14].Value.ToString()) ? Convert.ToInt32(param[14].Value.ToString()) : 0
            };
            model.MultiCourtier = model.CodeCourtierPayeur != model.CodeCourtierCommission || param[17].Value.ToString() == "O";

            return model;
        }

        /// <summary>
        /// Sélectionne les risques/objets/formules/garanties
        /// pour une période donnée
        /// </summary>
        public static string SelectionElemAttestation(string codeContrat, string version, string type, DateTime? periodeDeb, DateTime? periodeFin)
        {
            string sql = @"*PGM/KDA060CL";

            #region Déclaration des paramètres

            DbParameter[] param = new DbParameter[12];
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0ID", 0)
            {
                Value = 0
            };
            param[4] = new EacParameter("P0DPA", 0)
            {
                Value = periodeDeb.HasValue ? periodeDeb.Value.Year : 0
            };
            param[5] = new EacParameter("P0DPM", 0)
            {
                Value = periodeDeb.HasValue ? periodeDeb.Value.Month : 0
            };
            param[6] = new EacParameter("P0DPJ", 0)
            {
                Value = periodeDeb.HasValue ? periodeDeb.Value.Day : 0
            };
            param[7] = new EacParameter("P0FPA", 0)
            {
                Value = periodeFin.HasValue ? periodeFin.Value.Year : 0
            };
            param[8] = new EacParameter("P0FPM", 0)
            {
                Value = periodeFin.HasValue ? periodeFin.Value.Month : 0
            };
            param[9] = new EacParameter("P0FPJ", 0)
            {
                Value = periodeFin.HasValue ? periodeFin.Value.Day : 0
            };
            param[10] = new EacParameter("P0RET", string.Empty);
            param[11] = new EacParameter("P0ERR", string.Empty);

            #endregion

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            return param[10].Value.ToString() + "_" + param[11].Value.ToString();
        }

        /// <summary>
        /// Défini les mouvements sur une garantie
        /// </summary>
        public static string MouvementsGarantiePeriode(string codeContrat, string version, string type, string codrsq, string codfor, string garan, string idlot, string idregul)
        {
            string sql = @"*PGM/KDA301CL";
            int DEB = 0;
            int FIN = 0;

            string delrguwp = string.Format(
                @"DELETE FROM KPRGUWP WHERE KHYIPB = '{0}' AND KHYALX = {1} AND KHYTYP = '{2}'",
                codeContrat.Trim().PadLeft(9, ' '), version, type);

            //DbBase.Settings.ExecuteNonQuery(CommandType.Text, delrguwp);

            /* Lecture  de KPGARAN AVEC KHVKDEID  */
            string codegarantie = "";
            string selcodegar = @"
SELECT KDEGARAN
FROM KPSELW
INNER JOIN KPGARAN ON KDEIPB = KHVIPB AND KDEALX = KHVALX AND KDETYP = KHVTYP AND KDEID = :idg
INNER JOIN KGARAN ON KDEGARAN = GAGAR
WHERE KHVIPB = :ipb AND KHVALX = :alx AND KHVTYP = :typ";

            var result = DbBase.Settings.ExecuteStringList(
                CommandType.Text,
                selcodegar,
                new EacParameter("idg", DbType.Decimal) { Value = Convert.ToDecimal(garan) },
                new EacParameter("ipb", DbType.AnsiStringFixedLength, 9) { Value = codeContrat.PadLeft(9, ' ') },
                new EacParameter("alx", DbType.Int32) { Value = Convert.ToInt32(version) },
                new EacParameter("typ", type));

            codegarantie = result?.FirstOrDefault();

            /* Determination de la periode de la garantie */
            string selecDateEntete = string.Format(@"select khwdebp DATEDEBRETURNCOL,khwfinp DATEFINRETURNCOL from kprgu
                                        WHERE khwipb = '{0}' AND KHWALX = {1} AND KHWTYP = '{2}' AND KHWID = '{3}'",
                                codeContrat.Trim().PadLeft(9, ' '), version, type, idregul);


            var res1 = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, selecDateEntete);

            string selDateKpslew = string.Format(@"select KHVDEB DATEDEBRETURNCOL ,KHVFIN DATEFINRETURNCOL from kpselw
                                        WHERE khvipb = '{0}' AND KHVALX = {1} AND KHVTYP = '{2}' AND KHVID = '{3}'",
                                codeContrat.Trim().PadLeft(9, ' '), version, type, idlot);
            var res2 = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, selDateKpslew);


            if (res1 != null && res1.Any() && res2 != null && res2.Any())
            {
                if (res2.FirstOrDefault().DateDebReturnCol < res1.FirstOrDefault().DateDebReturnCol)
                {

                    DEB = Convert.ToInt32(res1.FirstOrDefault().DateDebReturnCol);
                }
                else
                {
                    DEB = Convert.ToInt32(res2.FirstOrDefault().DateDebReturnCol);
                }

                if (res2.FirstOrDefault().DateFinReturnCol > res1.FirstOrDefault().DateFinReturnCol || res2.FirstOrDefault().DateFinReturnCol == 0)
                {

                    FIN = Convert.ToInt32((res1.FirstOrDefault().DateFinReturnCol));
                }
                else
                {
                    FIN = Convert.ToInt32(res2.FirstOrDefault().DateFinReturnCol);
                }
            }
            #region Déclaration des paramètres

            DbParameter[] param = new DbParameter[12];
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0TYP", type);
            param[2] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[3] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt16(version) : 0
            };
            param[4] = new EacParameter("P0RSQ", 0)
            {
                Value = !string.IsNullOrEmpty(codrsq) ? Convert.ToInt16(codrsq) : 0
            };
            param[5] = new EacParameter("P0FOR", 0)
            {
                Value = !string.IsNullOrEmpty(codfor) ? Convert.ToInt16(codfor) : 0
            };
            param[6] = new EacParameter("P0GARAN", codegarantie);
            param[7] = new EacParameter("P0DEB", 0)
            {
                Value = DEB
            };
            param[8] = new EacParameter("P0FIN", 0)
            {
                Value = FIN
            };
            param[9] = new EacParameter("P0ERR", string.Empty);
            param[10] = new EacParameter("P0DEBMAXI", 0)
            {
                Value = 0
            };
            param[11] = new EacParameter("P0FINMAXI", 0)
            {
                Value = 0
            };

            #endregion

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            return param[0].Value.ToString() + param[9].Value.ToString();

        }
        /// <summary>
        /// Annulation de primes
        /// </summary>
        public static void AnnulationPrimes(string codeOffre, string version)
        {
            string sql = @"*PGM/KDA002CL";

            #region Déclaration des paramètres

            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };

            #endregion

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
        }

        /// <summary>
        /// retourne les montants de dejà emis pour la garantie dans la période
        /// </summary>
        public static string GetMontantGarantieEmis(string codeContrat, string version, string type, string codeRsq, string codeFor, string codeGar, int dateDeb, int dateFin)
        {
            //TODO:PGM
            string sql = @"*PGM/KDA310CL";

            #region Déclaration des paramètres

            DbParameter[] param = new DbParameter[12];
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0TYP", type);
            param[2] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[3] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt16(version) : 0
            };
            param[4] = new EacParameter("P0RSQ", 0)
            {
                Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt16(codeRsq) : 0
            };
            param[5] = new EacParameter("P0FOR", 0)
            {
                Value = !string.IsNullOrEmpty(codeFor) ? Convert.ToInt16(codeFor) : 0
            };
            param[6] = new EacParameter("P0GAR", codeGar);
            param[7] = new EacParameter("P0DEB", 0)
            {
                Value = dateDeb
            };
            param[8] = new EacParameter("P0FIN", 0)
            {
                Value = dateFin
            };
            param[9] = new EacParameter("P0MHT", 0)
            {
                Value = 0
            };
            param[10] = new EacParameter("P0MTX", 0)
            {
                Value = 0
            };
            param[11] = new EacParameter("P0AHT", 0)
            {
                Value = 0
            };

            #endregion

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            if (!string.IsNullOrEmpty(param[0].Value.ToString()))
            {
                return "ERROR_Problème recherche déjà émis, voir maintenance.";
            }
            else
            {
                return param[9].Value.ToString() + "_" + param[10].Value.ToString();
            }
        }

        public static string ComputeRegularisation(ComputeRegularisationParams parameters)
        {
            const string NomProgramme = "*PGM/KDA313";
            var returnParam = new EacParameter(parameters.GetDisplayString(p => p.Result), DbType.AnsiString, 3)
            {
                Value = string.Empty,
                Direction = ParameterDirection.InputOutput
            };

            DbBase.Settings.ExecuteNonQuery(
                CommandType.StoredProcedure,
                NomProgramme,
                new EacParameter[]
                {
                    returnParam,
                    new EacParameter(parameters.GetDisplayString(p => p.TypeContrat), DbType.AnsiString, 1) { Value = parameters.TypeContrat },
                    new EacParameter(parameters.GetDisplayString(p => p.CodeOffre), DbType.AnsiString, 9) { Value = parameters.CodeOffre.PadLeft(9, ' ') },
                    new EacParameter(parameters.GetDisplayString(p => p.VersionContrat), DbType.Int32) { Value = parameters.VersionContrat },
                    new EacParameter(parameters.GetDisplayString(p => p.Risque), DbType.Int32) { Value = parameters.Risque },
                    new EacParameter(parameters.GetDisplayString(p => p.Formule), DbType.Int64) { Value = parameters.Formule },
                    new EacParameter(parameters.GetDisplayString(p => p.Garantie), DbType.AnsiString, 10) { Value = parameters.Garantie },
                    new EacParameter(parameters.GetDisplayString(p => p.Id), DbType.Int64) { Value = parameters.Id },
                    new EacParameter(parameters.GetDisplayString(p => p.Mode), DbType.AnsiString, 10) { Value = parameters.Mode },
                    new EacParameter(parameters.GetDisplayString(p => p.Acces), DbType.AnsiString, 1) { Value = parameters.Acces },
                    new EacParameter(parameters.GetDisplayString(p => p.ContextId), DbType.Int64) { Value = parameters.ContextId },
                    new EacParameter(parameters.GetDisplayString(p => p.Top), DbType.AnsiString, 1) { Value = parameters.Top }
                });

            return returnParam.Value as string;
        }

        /// <summary>
        /// Charge les données de régularisation dans YPRIR*.*
        /// </summary>
        public static string LoadTableRegule(string codeContrat, string version, string type, string idRegule)
        {
            //TODO:PGM
            string sql = @"*PGM/KDA320CL";

            #region Déclaration des paramètres

            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0TYP", type);
            param[2] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[3] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[4] = new EacParameter("P0ID", 0)
            {
                Value = !string.IsNullOrEmpty(idRegule) ? Convert.ToInt32(idRegule) : 0
            };
            param[5] = new EacParameter("P0AFV", 0)
            {
                Value = 0,
                DbType = DbType.Decimal,
                Direction = ParameterDirection.InputOutput
            };

            #endregion

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            return param[0].Value.ToString();
        }

        public static string CalculForceRegule(string codeOffre, string version, string reguleId)
        {
            //TODO:PGM
            string sql = @"*PGM/KDA324CL";

            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[3] = new EacParameter("P0ID", 0)
            {
                Value = !string.IsNullOrEmpty(reguleId) ? Convert.ToInt32(reguleId) : 0
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            return param[0].Value.ToString();
        }
        /// <summary>
        /// retourne les montants de HT et Taxe (Régul)
        /// </summary>
        ///
        public static string GetMontantHtTaxe(string codeContrat, string version, string type, string codeAvn, string codeRsq, string assiette, string val, string unite, string codetaxe)
        {
            //TODO:PGM
            string sql = @"*PGM/KA039CL";

            #region Déclaration des paramètres

            DbParameter[] param = new DbParameter[12];
            param[0] = new EacParameter("P0RET", string.Empty);
            param[1] = new EacParameter("P0TYP", type);
            param[2] = new EacParameter("P0IPB", codeContrat.PadLeft(9, ' '));
            param[3] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt16(version) : 0
            };
            param[4] = new EacParameter("P0AVN", 0)
            {
                Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt16(codeAvn) : 0
            };
            param[5] = new EacParameter("P0RSQ", 0)
            {
                Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt16(codeRsq) : 0
            };
            param[6] = new EacParameter("P0ASS", 0)
            {
                Value = !string.IsNullOrEmpty(assiette) ? Convert.ToDecimal(assiette) : 0
            };
            param[7] = new EacParameter("P0VAL", 0)
            {
                Value = !string.IsNullOrEmpty(val) ? Convert.ToDecimal(val) : 0
            };
            param[8] = new EacParameter("P0UNI", unite == "%0" ? "§" : unite);
            param[9] = new EacParameter("P0TAX", codetaxe);
            param[10] = new EacParameter("P0MHT", 0)
            {
                Value = 0
            };
            param[11] = new EacParameter("P0MTX", 0)
            {
                Value = 0
            };
            #endregion
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
            if (!string.IsNullOrEmpty(param[0].Value.ToString()))
            {
                return "ERROR_Problème calcul montantHt et taxe, voir maintenance.";
            }
            else
            {
                return param[10].Value.ToString() + "_" + param[11].Value.ToString();
            }
        }

        public static void ChangeSbr(string codeAffaire, string version, string type, string user)
        {
            DbParameter[] paramBrch = new DbParameter[3];
            paramBrch[0] = new EacParameter("codeAffaire", codeAffaire.Trim().PadLeft(9, ' '));
            paramBrch[1] = new EacParameter("version", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            paramBrch[2] = new EacParameter("type", type);

            string sqlBrch = @"
SELECT PBBRA BRANCHE, KAACIBLE CIBLE, PBTTR STRRETURNCOL, PBAVN INT32RETURNCOL
FROM YPOBASE
INNER JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP
WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type";

            var resultBrch = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlBrch, paramBrch);
            if (resultBrch != null && resultBrch.Any())
            {
                var branche = resultBrch.FirstOrDefault().Branche;
                var cible = resultBrch.FirstOrDefault().Cible;
                var acteGestion = resultBrch.FirstOrDefault().StrReturnCol;
                var codeAvn = resultBrch.FirstOrDefault().Int32ReturnCol.ToString();

                if (branche == "RC" && cible == "ETS")
                {
                    #region Suppression Trace

                    DbParameter[] paramTrace = new DbParameter[3];
                    paramTrace[0] = new EacParameter("codeAffaire", codeAffaire.Trim().PadLeft(9, ' '));
                    paramTrace[1] = new EacParameter("version", 0)
                    {
                        Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                    };
                    paramTrace[2] = new EacParameter("type", type);

                    string sqlTrace = @"DELETE FROM KPAVTRC WHERE KHOIPB = :codeAffaire AND KHOALX = :version AND KHOTYP = :type AND KHOOEF = 'CC'";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlTrace, paramTrace);

                    #endregion

                    #region Détermination de la sous-branche

                    var sbr = string.Empty;
                    DbParameter[] paramSbr = new DbParameter[3];
                    paramSbr[0] = new EacParameter("codeAffaire", codeAffaire.Trim().PadLeft(9, ' '));
                    paramSbr[1] = new EacParameter("version", 0)
                    {
                        Value = !string.IsNullOrEmpty(version) ? Convert.ToInt16(version) : 0
                    };
                    paramSbr[2] = new EacParameter("type", type);
                    string sqlCount = @"SELECT COUNT(*) NBLIGN FROM KPGARAN WHERE KDEIPB = :codeAffaire AND KDEALX = :version AND KDETYP = :type AND KDEDEFG = 'P'";
                    if (ExistRowParam(sqlCount, paramSbr))
                    {
                        sbr = "RP";
                    }
                    else
                    {
                        sbr = "H";
                    }

                    #endregion

                    DbParameter[] paramInfo = new DbParameter[3];
                    paramInfo[0] = new EacParameter("codeAffaire", codeAffaire.Trim().PadLeft(9, ' '));
                    paramInfo[1] = new EacParameter("version", 0)
                    {
                        Value = !string.IsNullOrEmpty(version) ? Convert.ToInt16(version) : 0
                    };
                    paramInfo[2] = new EacParameter("type", type);
                    string sqlInfo = @"SELECT PBBRA BRANCHE, PBSBR STRRETURNCOL, PBCAT STRRETURNCOL2 FROM YPOBASE WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type";
                    var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlInfo, paramInfo);
                    if (result != null && result.Any())
                    {
                        var curSbr = result.FirstOrDefault().StrReturnCol;
                        var curCategorie = result.FirstOrDefault().StrReturnCol2;
                        if (curSbr != sbr)
                        {
                            #region MAJ SBR

                            DbParameter[] paramUpd = new DbParameter[4];
                            paramUpd[0] = new EacParameter("sbranche", sbr);
                            paramUpd[1] = new EacParameter("codeAffaire", codeAffaire.Trim().PadLeft(9, ' '));
                            paramUpd[2] = new EacParameter("version", 0)
                            {
                                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt16(version) : 0
                            };
                            paramUpd[3] = new EacParameter("type", type);
                            string sqlUpd = @"UPDATE YPOBASE SET PBSBR = :sbranche WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type";
                            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpd, paramUpd);

                            #endregion

                            string sql = @"*PGM/KP081CL";

                            #region Déclaration des paramètres

                            DbParameter[] param = new DbParameter[7];
                            param[0] = new EacParameter("P0RET", string.Empty);
                            param[1] = new EacParameter("P0TYP", type);
                            param[2] = new EacParameter("P0IPB", codeAffaire.PadLeft(9, ' '));
                            param[3] = new EacParameter("P0ALX", 0)
                            {
                                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt16(version) : 0
                            };
                            param[4] = new EacParameter("P0BRA", branche);
                            param[5] = new EacParameter("P0SBR", sbr);
                            param[6] = new EacParameter("P0CAT", curCategorie);

                            #endregion

                            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

                            var res = LoadAS400Commissions(codeAffaire, version, type, codeAvn, user, acteGestion);
                            if (res != null)
                            {
                                DbParameter[] paramUpdCom = new DbParameter[4];
                                paramUpdCom[0] = new EacParameter("jdxcm", 0)
                                {
                                    Value = res.TauxContratHCAT
                                };
                                paramUpdCom[1] = new EacParameter("jdcnc", 0)
                                {
                                    Value = res.TauxContratCAT
                                };
                                paramUpdCom[2] = new EacParameter("codeAffaire", codeAffaire.Trim().PadLeft(9, ' '));
                                paramUpdCom[3] = new EacParameter("version", 0)
                                {
                                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                                };
                                //MAJ YPRTENT

                                string sqlUpdCom = @"UPDATE YPRTENT SET JDXCM = :jdxcm, JDCNC = :jdcnc WHERE JDIPB = :codeAffaire AND JDALX = :version";
                                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdCom, paramUpdCom);
                            }

                            #region Ajout Trace

                            var dateNow = DateTime.Now;

                            DbParameter[] paramIns = new DbParameter[] {
                                new EacParameter("khoid", 0) {
                                    Value = GetAS400Id("KHOID")
                                },
                                new EacParameter("khotyp", type),
                                new EacParameter("khoipb", codeAffaire.PadLeft(9, ' ')),
                                new EacParameter("khoalx", 0) {
                                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                                },
                                new EacParameter("khoperi", "COT"),
                                new EacParameter("khorsq", 0) {
                                    Value = 0
                                },
                                new EacParameter("khoobj", 0) {
                                    Value = 0
                                },
                                new EacParameter("khofor", 0) {
                                    Value = 0
                                },
                                new EacParameter("khoopt", 0) {
                                    Value = 0
                                },
                                new EacParameter("khoetape", "RGU"),
                                new EacParameter("khocham", string.Empty),
                                new EacParameter("khoact", "M"),
                                new EacParameter("khoanv", string.Empty),
                                new EacParameter("khonvv", string.Empty),
                                new EacParameter("khoavo", "N"),
                                new EacParameter("khooef", "CC"),
                                new EacParameter("khocru", user),
                                new EacParameter("khocrd", 0) {
                                    Value = AlbConvert.ConvertDateToInt(dateNow)
                                },
                                new EacParameter("khocrh", 0) {
                                    Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow))
                                }
                            };

                            string sqlInsTrace = @"INSERT INTO KPAVTRC
                                                        (KHOID, KHOTYP, KHOIPB, KHOALX, KHOPERI, KHORSQ, KHOOBJ, KHOFOR, KHOOPT, KHOETAPE, KHOCHAM, KHOACT, KHOANV, KHONVV, KHOAVO, KHOOEF, KHOCRU, KHOCRD, KHOCRH)
                                                    VALUES
                                                        (:khoid, :khotyp, :khoipb, :khoalx, :khoperi, :khorsq, :khoobj, :khofor, :khoopt, :khoetape, :khocham, :khoact, :khoanv, :khonvv, :khoavo, :khooef, :khocru, :khocrd, :khocrh)";
                            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlInsTrace, paramIns);

                            #endregion

                            #region Ajout Acte de Gestion

                            DbParameter[] paramActeGes = new DbParameter[] {
                                new EacParameter("type", type),
                                new EacParameter("codeAffaire", codeAffaire.PadLeft(9, ' ')),
                                new EacParameter("version", 0) {
                                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                                },
                                new EacParameter("codeAvn", 0) {
                                    Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0
                                },
                                new EacParameter("acteGes", acteGestion),
                                new EacParameter("dateAnnee", 0) {
                                    Value = dateNow.Year
                                },
                                new EacParameter("dateMois", 0) {
                                    Value = dateNow.Month
                                },
                                new EacParameter("dateJour", 0) {
                                    Value = dateNow.Day
                                },
                                new EacParameter("dateHeure", 0) {
                                    Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow))
                                },
                                new EacParameter("libelle", "Changmnt sous-branche : " + sbr),
                                new EacParameter("user", user),
                                new EacParameter("dateAnneeMAJ", DbType.Decimal) {
                                    Value = dateNow.Year
                                },
                                new EacParameter("dateMoisMAJ", DbType.Decimal) {
                                    Value = dateNow.Month
                                },
                                new EacParameter("dateJourMAJ", DbType.Decimal) {
                                    Value = dateNow.Day
                                },
                                new EacParameter("dateHeureMAJ", DbType.Decimal) {
                                    Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow))
                                }
                            };
                            string sqlActeGes = @"insert into ypotrac
                                                        (PYTYP , PYIPB , PYALX , PYAVN , PYTTR , PYVAG , PYTRA , PYTRM , PYTRJ , PYTRH , PYLIB , PYINF ,
                                                            PYSDA , PYSDM , PYSDJ , PYSFA , PYSFM , PYSFJ , PYMJU , PYMJA , PYMJM , PYMJJ , PYMJH)
                                                    values
                                                        (:type, :codeAffaire, :version, :codeAvn, :acteGes, 'G', :dateAnnee, :dateMois, :dateJour, :dateHeure, :libelle, 'I',
                                                            0, 0, 0, 0, 0, 0, :user, :dateAnneeMAJ, :dateMoisMAJ, :dateJourMAJ, :dateHeureMAJ)";

                            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlActeGes, paramActeGes);

                            #endregion

                            #region Suppression trace de l'arbre

                            DbParameter[] paramDel = new DbParameter[3];
                            paramDel[0] = new EacParameter("type", type);
                            paramDel[1] = new EacParameter("codeAffaire", codeAffaire.Trim().PadLeft(9, ' '));
                            paramDel[2] = new EacParameter("version", 0)
                            {
                                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                            };

                            string sqlDel = @"DELETE FROM KPCTRLE WHERE KEVTYP = :type AND KEVIPB = :codeAffaire AND KEVALX = :version AND KEVETAPE = 'COT'";

                            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDel, paramDel);

                            #endregion
                        }
                    }
                }
            }
        }

        #endregion
        #region Paramétrage YYYYPAR

        public static string GetYYYYPARUserRights(string user)
        {
            ///TODO : à compléter par la suite.
            return "**";
        }

        /// <summary>
        /// Construction de la jointure à la table YYYYPAR
        /// </summary>
        public static string BuildJoinYYYYPAR(string branche, string cible, string additionalParam, string typeJoin, string concept, string famille, string alias = "DEFAULTALIAS", string otherCriteria = "", string restriction = "")
        {
            string restrictionClause = String.Empty;
            if (!string.IsNullOrEmpty(restriction))
            {
                restrictionClause = $" AND {alias}.TPTYP = '{restriction}'";
            }
            return $" {typeJoin} JOIN YYYYPAR {alias} ON {alias}.TCON = '{concept}' AND {alias}.TFAM = '{famille}' {otherCriteria} {restrictionClause}";

        }

        public static string BuildJoinYYYYPAR(string typeJoin, string concept, string famille, string alias = "DEFAULTALIAS", string otherCriteria = "", string restriction = "")
        {
            return string.Format(
                " {0} JOIN YYYYPAR {1} ON {1}.TCON = '{2}' AND {1}.TFAM = '{3}' {4} {5}",
                typeJoin, alias, concept, famille, otherCriteria, !string.IsNullOrEmpty(restriction) ? " AND {1}.TPTYP = '" + restriction + "'" : string.Empty);
        }

        public static string BuildSelectYYYYPAR(string branche, string cible, string additionalParam, string selectField, string contexte, string famille, string alias = "", string otherCriteria = "", List<string> tCod = null, bool notIn = false, string restriction = "", bool isBO = false, string tPcn2 = null)
        {


            StringBuilder conditions = new StringBuilder();
            conditions.AppendFormat(" AND *.TCON = '{0}' AND *.TFAM = '{1}' {2}", contexte, famille, otherCriteria);

            if (tCod != null)
            {
                conditions.Append(notIn ? " AND *.TCOD NOT IN (" : " AND TCOD IN (");
                var prefix = "";
                foreach (var code in tCod)
                {
                    conditions.Append(prefix).Append($"'{code}'");
                    prefix = ", ";
                }
                conditions.Append(")");
            }

            if (!string.IsNullOrEmpty(restriction))
            {
                conditions.Append($" AND *.TPTYP = '{ restriction }'");
            }

            if (!isBO && !string.IsNullOrEmpty(tPcn2))
            {
                conditions.Append(" AND *.TPCN2 = ").Append(tPcn2);
            }

            string sql;
            if (!isBO)
            {
                sql = BuildQueryParam1(conditions.ToString(), branche, cible, selectField, !string.IsNullOrEmpty(alias) ? alias : "Y1");
            }
            else
            {
                sql = $@"SELECT {selectField} FROM YYYYPAR {(!string.IsNullOrEmpty(alias) ? alias : "Y1")} WHERE 1 = 1 {conditions.ToString()}";
            }

            return sql.Replace("*.", !string.IsNullOrEmpty(alias) ? alias + "." : string.Empty);
        }


        private static string BuildQueryParam1(string condition, string branche, string cible, string selectField = "*", string alias = "Y1")
        {
            return string.Format(@"SELECT {2}
                                            FROM YYYYPAR {3}
                                            WHERE NOT EXISTS(
                                                SELECT *
                                                    FROM YYYYPAR Y2
                                                        INNER JOIN KFILTRL F2 ON F2.KGHFILT = Y2.TFILT AND F2.KGHACTF = 'E'
                                                    WHERE (F2.KGHBRA = '{0}' OR F2.KGHBRA = '*') AND (F2.KGHCIBLE = '{1}' OR F2.KGHCIBLE = '*')
                                                        AND Y2.TCON = {3}.TCON AND Y2.TFAM = {3}.TFAM AND Y2.TCOD = {3}.TCOD
                                                UNION ALL
                                                SELECT *
                                                    FROM YYYYPAR Y3
                                                        INNER JOIN KFILTRL F3 ON F3.KGHFILT = Y3.TFILT AND F3.KGHACTF = 'I'
                                                    WHERE ((F3.KGHBRA <> '{0}' AND F3.KGHBRA <> '*') OR ((F3.KGHBRA = '{0}' OR F3.KGHBRA = '*') AND F3.KGHCIBLE <> '{1}' AND F3.KGHCIBLE <> '*'))
                                                        AND Y3.TCON = {3}.TCON AND Y3.TFAM = {3}.TFAM AND Y3.TCOD = {3}.TCOD)
                                            {4}
                                        UNION
                                        SELECT {2}
                                            FROM YYYYPAR {3}
                                            WHERE EXISTS(
                                                SELECT *
                                                    FROM YYYYPAR Y4
                                                        INNER JOIN KFILTRL F4 ON F4.KGHFILT = Y4.TFILT AND F4.KGHACTF = 'I'
                                                    WHERE (F4.KGHBRA = '{0}' AND (F4.KGHCIBLE = '{1}' OR F4.KGHCIBLE = '*'))
                                                        AND Y4.TCON = {3}.TCON AND Y4.TFAM = {3}.TFAM AND Y4.TCOD = {3}.TCOD)
                                            {4}",
                    branche, cible, selectField, alias, condition);
        }
        #endregion

        #region Offre Simplifiee

        public static List<DtoCommon> GetInfoSimpleFolder(string codeOffre, string version, string type, string splitChar)
        {
            List<DtoCommon> infosSimpleFolder = new List<DtoCommon>();

            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", type);

            string sql = string.Format(@"SELECT 'R{0}1{0}' CONCAT TRIM(KGNR1DES) CONCAT '{0}O{0}1{0}' CONCAT TRIM(KGNO11DS) CONCAT '{0}O{0}2{0}' CONCAT TRIM(KGNO12DS)
                                CONCAT '{0}O{0}3{0}' CONCAT TRIM(KGNO13DS) CONCAT '{0}O{0}4{0}' CONCAT TRIM(KGNO14DS) CONCAT '{0}O{0}5{0}' CONCAT TRIM(KGNO15DS)
                                CONCAT '{0}F{0}1{0}A{0}' CONCAT TRIM(KGNF1DES)
                                CONCAT '{0}OPT{0}1' STRRETURNCOL
                            FROM KPEXL
                            WHERE KGNIPB = :codeOffre AND KGNALX = :version AND KGNTYP = :type", splitChar);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0)
            {
                var tInfo = result.FirstOrDefault().StrReturnCol.Split(new[] { splitChar }, StringSplitOptions.None);
                infosSimpleFolder.Add(new DtoCommon { Type = tInfo[0], Code = tInfo[1], Libelle = tInfo[2] });
                infosSimpleFolder.Add(new DtoCommon { Type = tInfo[3], Code = tInfo[4], Libelle = tInfo[5] });
                infosSimpleFolder.Add(new DtoCommon { Type = tInfo[6], Code = tInfo[7], Libelle = tInfo[8] });
                infosSimpleFolder.Add(new DtoCommon { Type = tInfo[9], Code = tInfo[10], Libelle = tInfo[11] });
                infosSimpleFolder.Add(new DtoCommon { Type = tInfo[12], Code = tInfo[13], Libelle = tInfo[14] });
                infosSimpleFolder.Add(new DtoCommon { Type = tInfo[15], Code = tInfo[16], Libelle = tInfo[17] });
                infosSimpleFolder.Add(new DtoCommon { Type = tInfo[18], Code = tInfo[19], LettreLib = tInfo[20], Libelle = tInfo[21] });
                infosSimpleFolder.Add(new DtoCommon { Type = tInfo[22], Code = tInfo[23] });
            }

            return infosSimpleFolder;
        }

        #endregion

        #region LCI Franchise
        //toReturn.LCIUnites = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNLCI");
        //  toReturn.LCITypes = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BALCI");
        public static LCIFranchiseUnitesTypesDto GetLCIFranchiseUnitesTypes(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            LCIFranchiseUnitesTypesDto lciFranchiseUnitesTypes = new LCIFranchiseUnitesTypesDto();
            BrancheDto branche = GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);
            lciFranchiseUnitesTypes.UnitesLCI = GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNLCI");
            lciFranchiseUnitesTypes.TypesLCI = GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BALCI");
            lciFranchiseUnitesTypes.UnitesFranchise = GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNFRA");
            lciFranchiseUnitesTypes.TypesFranchise = GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BAFRA");
            return lciFranchiseUnitesTypes;
        }
        #endregion

        internal static string GetPrefixeHisto(ModeConsultation modeNavig, string table)
        {
            if (modeNavig == ModeConsultation.Historique)
            {
                if (table.StartsWith("YPO"))
                {
                    return string.Concat("YHP", table.Substring(3));
                }


                if (table.StartsWith("KP"))
                {
                    return string.Concat("HP", table.Substring(2));
                }

                if (table.StartsWith("YPRT"))
                {
                    return string.Concat("YHRT", table.Substring(4));
                }

                return table;
            }

            return table;
        }

        internal static string GetHistoTableName(string table)
        {
            if (table.StartsWith("YPO", StringComparison.InvariantCultureIgnoreCase))
            {
                return string.Concat("YHP", table.Substring(3));
            }
            else if (table.StartsWith("KP", StringComparison.InvariantCultureIgnoreCase))
            {
                return string.Concat("HP", table.Substring(2));
            }
            else if (table.StartsWith("YPRT", StringComparison.InvariantCultureIgnoreCase))
            {
                return string.Concat("YHRT", table.Substring(4));
            }

            return table;
        }

        public static string GetSuffixeRegule(string acteGestion, string table)
        {
            if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL)
            {
                return table.Replace("YPRIP", "YPRIR");
            }
            return table;
        }

        #region Menus Contextuels

        public static List<UsersContextMenuDto> GetAllUsersContextMenu(string appl)
        {
            var result = GetAllUsersFlatContextMenu(appl);

            if (result != null && result.Count > 0)
            {
                List<string> users = result.GroupBy(el => el.Utilisateur).Select(f => f.First().Utilisateur).ToList();
                List<UsersContextMenuDto> usersContextsMenu = new List<UsersContextMenuDto>();

                users.ForEach(elUser =>
                {

                    //Roles par utilisateur
                    var roles = result.FindAll(e => e.Utilisateur == elUser).GroupBy(el => el.Menu).Select(r => r.First()).ToList();

                    var lstRoles = new List<ContextMenuDto>();
                    int comptRole = 0;
                    roles.ForEach(role =>
                    {
                        var options =
                          result.FindAll(o => o.Menu == role.Menu && o.Utilisateur == elUser).GroupBy(el => el.Option).Select(opt => opt.First()).ToList();
                        List<ContextMenuDto> lstOptions = new List<ContextMenuDto>();

                        options.ForEach(opt =>
                        {
                            lstOptions.Add(new ContextMenuDto
                            {
                                text = opt.OptionLib,
                                icon = opt.Action,
                                alias = opt.Action,
                                action = opt.Action,
                                typeOffreContrat = opt.TypeOffreContrat,
                                orderby = int.TryParse(opt.Orderby, out int tmpParse) ? tmpParse : 0,
                                AlwBranche = opt.AlwBranche,
                                AlwEtat = opt.AlwEtat,
                                menu = Enum.TryParse(opt.Menu, out AlbContextMenu x) ? x : 0
                            });
                        });

                        int tmpParseRole = 0;
                        Enum.TryParse(role.Menu, true, out AlbContextMenu m);
                        if (lstOptions.Count == 1 && m.IsIn(AlbContextMenu.CREER, AlbContextMenu.OFFCONT, AlbContextMenu.AVENANT))
                        {
                            lstRoles.Add(new ContextMenuDto
                            {
                                orderby = int.TryParse(role.Orderby, out tmpParseRole) ? tmpParseRole : 0,
                                typeOffreContrat = role.TypeOffreContrat,
                                menu = m,
                                text = m.DisplayName(),
                                items = lstOptions,
                                type = "group",
                                alias = comptRole.ToString(),
                                icon = string.Empty,
                                AlwBranche = string.Empty,
                                AlwEtat = string.Empty
                            });
                        }
                        else
                        {
                            lstRoles.Add(new ContextMenuDto
                            {
                                orderby = int.TryParse(role.Orderby, out tmpParseRole) ? tmpParseRole : 0,
                                typeOffreContrat = role.TypeOffreContrat,
                                menu = m,
                                text = m.DisplayName(),
                                items = lstOptions.Count > 1 ? lstOptions : null,
                                type = lstOptions.Count > 1 ? "group" : null,
                                alias = lstOptions.Count == 1 ? lstOptions.FirstOrDefault().alias : comptRole.ToString(),
                                icon = lstOptions.Count == 1 ? lstOptions.FirstOrDefault().icon : string.Empty,
                                AlwBranche = lstOptions.Count == 1 ? lstOptions.FirstOrDefault().AlwBranche : string.Empty,
                                AlwEtat = lstOptions.Count == 1 ? lstOptions.FirstOrDefault().AlwEtat : string.Empty
                            });
                        }
                        lstRoles.Add(new ContextMenuDto { type = "splitLine" });
                        comptRole++;
                    });


                    usersContextsMenu.Add(new UsersContextMenuDto { Utilisateur = elUser, UserContextMenu = lstRoles });
                });
                return usersContextsMenu;
            }
            return null;
        }
        public static List<ContextMenuPlatDto> SverifDroitsByUser(string id)
        {
            var param = new DbParameter[1];
            param[0] = new EacParameter("P_ID", id.ToUpper());

            string sql_VerifDroit = $@"SELECT UTIUT FROM YUTILIS
                                            INNER JOIN Y£GDM_ALB . Y£UTIL ON BUSER = UTIUT
                                            INNER JOIN Y£GDM_ALB . Y£AUTC ON ECLS = BCLAS AND UPPER ( EAPPL ) = UPPER ( 'KDQ' )
                                            INNER JOIN Y£GDM_ALB . Y£MENU ON AAPPL = EAPPL AND AMENU = EMEN
                                            INNER JOIN Y£GDM_ALB . Y£MNOP ON MAPPL = EAPPL AND MMENU = EMEN
                                            INNER JOIN Y£GDM_ALB . Y£OPT ON GAPPL = EAPPL AND GOPT = MOPT
                                            LEFT JOIN Y£GDM_ALB . Y£INTC ON HCLAS = BCLAS AND HAPPL = EAPPL AND HOPT = MOPT
                                            LEFT JOIN Y£GDM_ALB . Y£AUTU ON SUSER = UTIUT AND SAPPL = EAPPL AND SOPT = MOPT
                                            WHERE UTIUT =:P_ID";

            return DbBase.Settings.ExecuteList<ContextMenuPlatDto>(CommandType.Text, sql_VerifDroit, param);

        }
        public static List<ContextMenuPlatDto> GetAllUsersFlatContextMenu(string appl)
        {
            var param = new DbParameter[1];
            param[0] = new EacParameter("P_APPLI", appl.ToUpper());

            string sql_Getcontextmenu = $@"SELECT 
TRIM(UTIUT) USER , 
MMENU MENU,
AICO MENUICO , 
GOPT OPTION,
GLOPT OPTIONLIB , 
GCDE1 ACTION,
IFNULL ( HOPT, 0 ) DENIEDROLE , 
IFNULL(SOPT, 0) DENIEDUSER , 
GZN3 BRCHE,
GZN4 TYPE , 
GZN5 ETAT,
SUBSTR ( ALMEN, 47 ) ORDERBY
FROM YUTILIS
INNER JOIN Y£GDM_ALB.Y£UTIL ON BUSER = UTIUT
INNER JOIN Y£GDM_ALB.Y£AUTC ON ECLS = BCLAS AND UPPER(EAPPL ) = :P_APPLI
INNER JOIN Y£GDM_ALB.Y£MENU ON AAPPL = EAPPL AND AMENU = EMEN
INNER JOIN Y£GDM_ALB.Y£MNOP ON MAPPL = EAPPL AND MMENU = EMEN
INNER JOIN Y£GDM_ALB.Y£OPT ON GAPPL = EAPPL AND GOPT = MOPT
LEFT JOIN Y£GDM_ALB.Y£INTC ON HCLAS = BCLAS AND HAPPL = EAPPL AND HOPT = MOPT
LEFT JOIN Y£GDM_ALB.Y£AUTU ON SUSER = UTIUT AND SAPPL = EAPPL AND SOPT = MOPT";

            return DbBase.Settings.ExecuteList<ContextMenuPlatDto>(CommandType.Text, sql_Getcontextmenu, param);

        }

        #endregion

        #region Contrôle Assiette

        public static bool CheckControlAssiette(string codeContrat, string version, string type)
        {
            string sql = string.Format("SELECT COUNT(*) NBLIGN FROM KPCTRLA WHERE KGTIPB = '{0}' AND KGTALX = {1} AND KGTTYP = '{2}' WITH NC",
                                codeContrat.PadLeft(9, ' '), Convert.ToInt32(version), type);
            return ExistRow(sql);
        }

        /// <summary>
        /// Suppression de l'enregistrement
        /// éventuel dans la table KPCTRLA
        /// </summary>
        public static void RemoveControlAssiette(string codeContrat, string version, string type)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("codeContrat", codeContrat.PadLeft(9, ' '));
            param[1] = new EacParameter("versionContrat", 0)
            {
                Value = Convert.ToInt32(version)
            };
            param[2] = new EacParameter("typeContrat", type);

            string sql = "DELETE FROM KPCTRLA WHERE KGTIPB = :codeContrat AND KGTALX = :versionContrat AND KGTTYP = :typeContrat";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Création de l'enregistrement
        /// dans la table KPCTRLA
        /// </summary>
        public static void InsertControlAssiette(string codeContrat, string version, string type, string etape, string libelle, string user)
        {
            var dateNow = DateTime.Now;
            var isTraceCotisation = false;

            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("codeContrat", codeContrat.PadLeft(9, ' '));
            param[1] = new EacParameter("versionContrat", 0)
            {
                Value = Convert.ToInt32(version)
            };
            param[2] = new EacParameter("typeContrat", type);
            param[3] = new EacParameter("etape", "COT");

            string sql = "SELECT COUNT(*) FROM KPCTRLE WHERE KEVIPB = :codeContrat AND KEVALX = :versionContrat AND KEVTYP = :typeContrat AND KEVETAPE = :etape";
            isTraceCotisation = ExistRow(sql);

            if (isTraceCotisation)
            {
                sql = "DELETE FROM KPCTRLE WHERE KEVIPB = :codeContrat AND KEVALX = :versionContrat AND KEVTYP = :typeContrat AND KEVETAPE = :etape";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                param = new DbParameter[11];
                param[0] = new EacParameter("codeContrat", codeContrat.PadLeft(9, ' '));
                param[1] = new EacParameter("versionContrat", 0)
                {
                    Value = Convert.ToInt32(version)
                };
                param[2] = new EacParameter("typeContrat", type);
                param[3] = new EacParameter("etape", etape);
                param[4] = new EacParameter("libelle", libelle);
                param[5] = new EacParameter("createUser", user);
                param[6] = new EacParameter("createDateNow", 0)
                {
                    Value = AlbConvert.ConvertDateToInt(dateNow)
                };
                param[7] = new EacParameter("createHeureNow", 0)
                {
                    Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow))
                };
                param[8] = new EacParameter("updateUser", user);
                param[9] = new EacParameter("updateDateNow", 0)
                {
                    Value = AlbConvert.ConvertDateToInt(dateNow)
                };
                param[10] = new EacParameter("updateHeureNow", 0)
                {
                    Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow))
                };

                sql = @"INSERT INTO KPCTRLA
                        (KGTIPB, KGTALX, KGTTYP, KGTETAPE, KGTLIB, KGTCRU, KGTCRD, KGTCRH, KGTMAJU, KGTMAJD, KGTMAJH)
                    VALUES
                        (:codeContrat, :versionContrat, :typeContrat, :etape, :libelle, :createUser, :createDateNow, :createHeureNow, :updateUser, :updateDateNow, :updateHeureNow)";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
        }

        #endregion

        #region Bandeau
        public static BandeauDto GetBandeau(string codeOffre, string versionOffre, string typeOffre)
        {
            var toReturn = new BandeauDto();
            string sql = $@"SELECT
PBIPB CODEOFFRE,
PBALX VERSIONOFFRE,
PBTYP TYPEOFFRE,
PBSAA DATESAISIEANNEE,
PBSAM DATESAISIEMOIS,
PBSAJ DATESAISIEJOUR,
PBSAH DATESAISIEHEURE,
PBBRA BRANCHECODE,
BRCH.TPLIB BRANCHELIB,
PBREF DESCRIPTIF,
KAACIBLE CIBLECODE,
KAHDESC CIBLELIB,
PBCTA CODECOURTIERAPPORTEUR,
YCApp.TNNOM NOMCOURTIERAPPO,
PBCTP CODECOURTIERPAYEUR,
YCPay.TNNOM NOMCOURTIERPAYEUR,
PBICT CODECOURTIERGESTIONNAIRE,
YCGest.TNNOM NOMCOURTIERGEST,
ANNOM NOMPRENASSUR,
ANIAS CODEPRENASSUR,
PBSOU SOUSCODE,
UT1.UTNOM SOUSNOM,
UT1.UTPNM SOUSPNM,
PBGES GESCODE,
UT2.UTNOM GESNOM,
UT2.UTPNM GESPNM,
PBEFA DATEEFFETA,
PBEFM DATEEFFETM,
PBEFJ DATEEFFETJ,
PBFEJ FINEFFETJOUR,
PBFEM FINEFFETMOIS,
PBFEA FINEFFETANNEE,
PBDEV CODEDEVISE,
DEVISE.TPLIB LIBDEVISE,
PBNPL NATURECONTRAT,
NATURE.TPLIL LIBELLENATURECONTRAT,
PBAPP PARTALBINGIA,
PBPCV COUVERTURE,
JDIND CODEINDICEREFERENCE,
IND.TPLIB LIBINDICE,
JDIVA VALEUR,
PBETA ETAT,
PBSIT CODESITUATION,
ETAT.TPLIB LIBETAT,
PBPER PERIODICITECODE,
PERD.TPLIB PERIODICITENOM,
JDENC CODEENCAISSEMENT,
ENCAISS.TPLIB LIBENCAISSEMENT,
PBECJ ECHJOUR,
PBECM ECHMOIS,
KAALCIVALO LCIGENERALE,
KAALCIUNIT LCIGENERALEUNIT,
KAALCIBASE LCIGENERALETYPE,
KAAFRHVALO FRCHGENERALE,
KAAFRHUNIT FRCHGENERALEUNIT,
KAAFRHBASE FRCHGENERALETYPE,
JDTRR TERRITORIALITE,
TERRITO.TPLIB TERRITORIALITELIB,
PBSTF CODEMOTIF,
MOTIF.TPLIB LIBMOTIF,
PBSTP STOP,
SITSTOP.TPLIL STOPLIB,
PBCON STOPCONTENTIEUX,
CONTSTOP.TPLIL STOPCONTENTIEUXLIB,
PBCTD DUREE,
PBCTU DUREEUNITE,
DR.TPLIB DUREESTR,
PBRGT CODEREGIME,
RG.TPLIB LIBREGIME,
JDCNA SOUMISCATNAT,
JDTFF MONTANTREF1,
JDTMC MONTANTREF2,
JDINA INDEXATION,
JDIXL LCI,
JDIXC ASSIETTE,
JDIXF FRANCHISE,
JDDPV PREAVIS,
PBTTR CODEACTION,
TR.TPLIB LIBACTION,
SIT.TPLIB LIBSITUATION,
PBSTJ DATESITJOUR,
PBSTM DATESITMOIS,
PBSTA DATESITANNEE,
PBCRU UCRCODE,
UCR.UTNOM UCRNOM,
PBMJU UUPCODE,
UUP.UTNOM UUPNOM,
PBAVN NUMAVENANT,
PBAVK NUMEXTERNE,
PBAVJ DATEEFFETAVNJOUR,
PBAVM DATEEFFETAVNMOIS,
PBAVA DATEEFFETAVNANNEE,
PBOFF CODEOFFREORIGINE,
PBOFV VERSIONOFFREORIGINE,
JDPEJ PROCHECHJ,
JDPEM PROCHECHM,
JDPEA PROCHECHA,
JDEHH HORSCATNAT,
JDEHC CATNAT,
JDXCM TAUXHORSCATNAT,
JDCNC TAUXCATNAT,
SSDTJ DATEAFFAIRENOUVELLEJOUR,
SSDTM DATEAFFAIRENOUVELLEMOIS,
SSDTA DATEAFFAIRENOUVELLEANNEE,
PBCRJ DATECRJOUR,
PBCRM DATECRMOIS,
PBCRA DATECRANNEE,
PBMJJ DATEUPJOUR,
PBMJM DATEUPMOIS,
PBMJA DATEUPANNEE,
ADRESSEGESTIONNAIRE.ABPDP6 DPGESTIONNAIRE,
ADRESSEGESTIONNAIRE.ABPCP6 CPGESTIONNAIRE,
ADRESSEGESTIONNAIRE.ABPVI6 VILLEGESTIONNAIRE,
ADRESSEASSURE.ABPDP6 DPASSURE,
ADRESSEASSURE.ABPCP6 CPASSURE,
ADRESSEASSURE.ABPVI6 VILLEASSURE ,
ADRESSEAPPORTEUR.ABPDP6 DPAPPORTEUR,
ADRESSEAPPORTEUR.ABPCP6 CPAPPORTEUR,
ADRESSEAPPORTEUR.ABPVI6 VILLEAPPORTEUR,
ADRESSEPAYEUR.ABPDP6 DPPAYEUR,
ADRESSEPAYEUR.ABPCP6 CPPAYEUR,
ADRESSEPAYEUR.ABPVI6 VILLEPAYEUR ,
SBR.TPLIB LIBSBR ,
PBORK,
PBTTR,
LASIDREGUL.MAXIDREGUL
FROM YPOBASE
LEFT JOIN YPRTENT ON PBIPB = JDIPB AND PBALX = JDALX
LEFT JOIN KPENT ON KAAIPB = PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP
LEFT JOIN YPOCOAS ON PBIPB = PHIPB AND PBALX = PHALX
LEFT JOIN KPOBSV ON KAAOBSV = KAJCHR
LEFT JOIN YUTILIS UT1 ON PBSOU = UT1.UTIUT
LEFT JOIN YUTILIS UT2 ON PBGES = UT2.UTIUT
LEFT JOIN YUTILIS UCR ON PBCRU = UCR.UTIUT
LEFT JOIN YUTILIS UUP ON PBMJU = UUP.UTIUT
LEFT JOIN YCOMPA ON CIICI = PHCIE
LEFT JOIN KCIBLE ON KAACIBLE = KAHCIBLE
LEFT JOIN YSTAPRO ON SSIPW = PBIPB AND SSALW = PBALX
LEFT JOIN YCOURTN YCGest ON YCGest.TNICT = PBICT AND YCGest.TNXN5 = 0 AND YCGest.TNTNM = 'A'
LEFT JOIN YCOURTI Gestionnaire ON Gestionnaire.TCICT=PBICT
LEFT JOIN YASSNOM ON ANIAS = PBIAS AND ANINL = 0 AND ANTNM = 'A'
LEFT JOIN YCOURTN YCApp ON YCApp.TNICT = PBCTA AND YCApp.TNXN5 = 0 AND YCApp.TNTNM = 'A'
LEFT JOIN YCOURTN YCPay ON YCPay.TNICT = PBCTP AND YCPay.TNXN5 = 0 AND YCPay.TNTNM = 'A'
{BuildJoinYYYYPAR("LEFT", "GENER", "TAXRG", "RG", " AND RG.TCOD = PBRGT")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", "TR", " AND TR.TCOD = PBTTR")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBSIT", "SIT", " AND SIT.TCOD = PBSIT")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBNPL", "NATURE", " AND PBNPL = NATURE.TCOD")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBETA", "ETAT", " AND ETAT.TCOD = PBETA")}
{BuildJoinYYYYPAR("LEFT", "GENER", "BRCHE", "BRCH", " AND BRCH.TCOD = PBBRA AND BRCH.TPCN2 = 1")}
{BuildJoinYYYYPAR("LEFT", "GENER", "DEVIS", "DEVISE", " AND DEVISE.TCOD = PBDEV")}
{BuildJoinYYYYPAR("LEFT", "GENER", "INDIC", "IND", " AND IND.TCOD = JDIND")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBPER", "PERD", " AND PERD.TCOD = PBPER")}
{BuildJoinYYYYPAR("LEFT", "GENER", "TCYEN", "ENCAISS", " AND ENCAISS.TCOD = JDENC")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "QATRR", "TERRITO", " AND TERRITO.TCOD = JDTRR")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBSTF", "MOTIF", " AND MOTIF.TCOD = PBSTF")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBSTP", "SITSTOP", " AND SITSTOP.TCOD = PBSTP")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PKREL", "CONTSTOP", " AND CONTSTOP.TCOD = PBCON")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBCTU", "DR", " AND DR.TCOD = PBCTU")}
{BuildJoinYYYYPAR("LEFT", "GENER", "BRSBR", "SBR", " AND (PBBRA = 'RC' AND SBR.TCOD = TRIM(PBBRA) CONCAT TRIM(PBSBR) OR SBR.TCOD='')")}
LEFT JOIN YADRESS ADRESSEGESTIONNAIRE ON Gestionnaire.TCADH = ADRESSEGESTIONNAIRE.ABPCHR
LEFT JOIN YASSURE ON PBIAS = ASIAS
LEFT JOIN YADRESS ADRESSEASSURE ON ASADH = ADRESSEASSURE.ABPCHR
LEFT JOIN YCOURTI Apporteur ON Apporteur.TCICT=PBCTA
LEFT JOIN YADRESS ADRESSEAPPORTEUR ON Apporteur.TCADH = ADRESSEAPPORTEUR.ABPCHR
LEFT JOIN YCOURTI Payeur ON Payeur.TCICT = PBCTP
LEFT JOIN YADRESS ADRESSEPAYEUR ON Payeur.TCADH = ADRESSEPAYEUR.ABPCHR
LEFT JOIN  (
    SELECT KHWTYP, KHWIPB, KHWALX, MAX(KHWID) AS MAXIDREGUL
    FROM KPRGU
    GROUP BY KHWTYP, KHWIPB, KHWALX
) AS LASIDREGUL ON
    LASIDREGUL.KHWTYP = PBTYP AND LASIDREGUL.KHWIPB = PBIPB AND LASIDREGUL.KHWALX = PBALX
WHERE PBIPB = :codeOffre AND PBTYP = :typeOffre AND PBALX = :versionOffre ";
            var parameters = MakeParams(sql, codeOffre.PadLeft(9, ' '), typeOffre, versionOffre);

            var result = DbBase.Settings.ExecuteList<BandeauDto>(CommandType.Text, sql, parameters);

            if (result == null || !result.Any())
            {
                return toReturn;
            }

            toReturn = result.FirstOrDefault();


            toReturn.CPCourtierGest = ToCodePostal(toReturn.DepartementGestionnaire, toReturn.CodePostalGestionnaire);
            toReturn.VilleCourtierGest = toReturn.VilleGestionnaire;

            toReturn.CPPreneurAssurance = ToCodePostal(toReturn.DepartementAssure, toReturn.CodePostalAssure);
            toReturn.VillePreneurAssurance = toReturn.VilleAssure;

            toReturn.VilleCourtierAppo = ToCodePostal(toReturn.DepartementApporteur, toReturn.CodePostalApporteur) + " " + toReturn.VilleApporteur;

            toReturn.VilleCourtierPayeur = ToCodePostal(toReturn.DepartementPayeur, toReturn.CodePostalPayeur) + " " + toReturn.VilleCourtierPayeur;

            toReturn.HasDoubleSaisie = PoliceRepository.GetDoubleSaisie(codeOffre, int.Parse(versionOffre), typeOffre);
            toReturn.TauxAvailable = TraceRepository.ObtenirTraceByEtape(codeOffre, versionOffre, typeOffre, "COT", "COT");

            if ((toReturn.TauxAvailable || toReturn.Origine == String.Empty) && (typeOffre == AlbConstantesMetiers.TYPE_OFFRE))
            {
                InitComFromCotisation(codeOffre, versionOffre, typeOffre, toReturn);
            }


            if (typeOffre != AlbConstantesMetiers.TYPE_CONTRAT)
            {
                return toReturn;
            }

            var delegation = DelegationRepository.Obtenir(toReturn.CodeCourtierApporteur);
            if (delegation != null)
            {
                toReturn.NomDelegation = delegation.Nom;
                toReturn.NomInspecteur = delegation.Inspecteur != null ? delegation.Inspecteur.Nom : string.Empty;
                toReturn.Secteur = delegation.Secteur;
                toReturn.LibSecteur = delegation.LibSecteur;
            }

            toReturn.MontantStatistique = AffaireNouvelleRepository.GetMontantStatistique(codeOffre, versionOffre);
            toReturn.HasSusp = RechercheRepository.HasSusp(codeOffre, Convert.ToInt32(versionOffre), typeOffre);


            return toReturn;
        }

        private static bool TryInitComFromQuittance(string codeOffre, string versionOffre, string typeOffre, BandeauDto toReturn, string idReg, string modeAffichage)
        {
            var quits = FinOffreRepository.GetQuittances(codeOffre, versionOffre, typeOffre, toReturn.NumAvenant.ToString(), modeAffichage: modeAffichage, numQuittanceVisu: string.Empty, modeNavig: ModeConsultation.Standard, user: string.Empty, acteGestion: toReturn.TypeTraitement, reguleId: idReg);
            var quit = quits.OrderByDescending(x => x.FinPeriodeAnnee).ThenByDescending(x => x.FinPeriodeMois).ThenByDescending(x => x.FinPeriodeJour).FirstOrDefault();
            if (quit == null) { return false; }
            toReturn.TauxCatNat = quit.TauxCatNatRetenu;
            toReturn.TauxHorsCatNat = quit.TauxHRCatNatRetenu;
            toReturn.CatNat = quit.MontantCommissRetenu;
            toReturn.HorsCatNat = quit.MontantCommissHRCatNatRetenu;
            return true;
        }

        private static void InitComFromCotisation(string codeOffre, string versionOffre, string typeOffre, BandeauDto toReturn)
        {
            var com = CotisationsRepository.InitCotisations(codeOffre, versionOffre, typeOffre, toReturn.NumAvenant.ToString(), modeNavig: ModeConsultation.Standard);
            if (com != null)
            {
                var hrCatNAt = (double)com.GarantiesInfo.Commission.MontantForce;
                const double almostZero = 1E-5;
                if (Math.Abs(hrCatNAt) < almostZero)
                {
                    hrCatNAt = (double)com.GarantiesInfo.Commission.MontantStd;
                }
                var catNAt = (double)com.GarantiesInfo.Commission.MontantForceCatNat;
                if (Math.Abs(catNAt) < almostZero)
                {
                    catNAt = (double)com.GarantiesInfo.Commission.MontantStdCatNat;
                }
                var tauxCatNat = double.TryParse(com.GarantiesInfo.Commission.TauxForceCatNat, out var tauxCatNat1) ? tauxCatNat1 : 0.0;
                if (Math.Abs(tauxCatNat) < almostZero)
                {
                    tauxCatNat = double.TryParse(com.GarantiesInfo.Commission.TauxStdCatNat, out var tauxCatNat2) ? tauxCatNat2 : 0.0;
                }
                var tauxHorsCatNat = double.TryParse(com.GarantiesInfo.Commission.TauxForce, out var taux1) ? taux1 : 0.0;
                if (Math.Abs(tauxHorsCatNat) < almostZero)
                {
                    tauxHorsCatNat = double.TryParse(com.GarantiesInfo.Commission.TauxStd, out var taux2) ? taux2 : 0.0;
                }
                toReturn.TauxCatNat = tauxCatNat;
                toReturn.TauxHorsCatNat = tauxHorsCatNat;
                toReturn.HorsCatNat = hrCatNAt;
                toReturn.CatNat = catNAt;
            }
            else
            {
                toReturn.TauxAvailable = false;
            }
        }

        private static string ToCodePostal(string departementGestionnaire, short codePostalGestionnaire)
        {
            _ = int.TryParse(departementGestionnaire + codePostalGestionnaire.ToString().PadLeft(3, '0'), out int cp);
            return cp.ToString();
        }

        public static BandeauDto GetBandeauHisto(string codeOffre, string versionOffre, string typeOffre, string numAvenant)
        {
            var toReturn = new BandeauDto();
            string sql = $@"
SELECT PBIPB CODEOFFRE,PBALX VERSIONOFFRE, PBTYP TYPEOFFRE,PBSAA DATESAISIEANNEE, PBSAM DATESAISIEMOIS, PBSAJ DATESAISIEJOUR, PBSAH DATESAISIEHEURE,
PBBRA BRANCHECODE, BRCH.TPLIB BRANCHELIB, PBREF DESCRIPTIF,
KAACIBLE CIBLECODE, KAHDESC CIBLELIB,
PBCTA CODECOURTIERAPPORTEUR, YCApp.TNNOM NOMCOURTIERAPPO,
PBCTP CODECOURTIERPAYEUR, YCPay.TNNOM NOMCOURTIERPAYEUR,
PBICT CODECOURTIERGESTIONNAIRE, YCGest.TNNOM NOMCOURTIERGEST,
ANNOM NOMPRENASSUR,ANIAS CODEPRENASSUR,
PBSOU SOUSCODE, UT1.UTNOM SOUSNOM, UT1.UTPNM SOUSPNM,
PBGES GESCODE, UT2.UTNOM GESNOM, UT2.UTPNM GESPNM,
PBEFA DATEEFFETA,PBEFM DATEEFFETM,  PBEFJ DATEEFFETJ,
PBFEJ FINEFFETJOUR, PBFEM FINEFFETMOIS, PBFEA FINEFFETANNEE,
PBDEV CODEDEVISE,DEVISE.TPLIB LIBDEVISE,
PBNPL NATURECONTRAT,NATURE.TPLIL LIBELLENATURECONTRAT,
PBAPP PARTALBINGIA,PBPCV COUVERTURE,
JDIND CODEINDICEREFERENCE,IND.TPLIB LIBINDICE,
JDIVA VALEUR,
PBETA ETAT, PBSIT CODESITUATION, ETAT.TPLIB LIBETAT,
PBPER PERIODICITECODE,PERD.TPLIB PERIODICITENOM,
JDENC CODEENCAISSEMENT, ENCAISS.TPLIB LIBENCAISSEMENT,
PBECJ ECHJOUR, PBECM ECHMOIS,
KAALCIVALO LCIGENERALE, KAALCIUNIT LCIGENERALEUNIT, KAALCIBASE LCIGENERALETYPE,
KAAFRHVALO FRCHGENERALE, KAAFRHUNIT FRCHGENERALEUNIT, KAAFRHBASE FRCHGENERALETYPE,
JDTRR TERRITORIALITE, TERRITO.TPLIB TERRITORIALITELIB,
PBSTF CODEMOTIF, MOTIF.TPLIB LIBMOTIF,
PBSTP STOP, SITSTOP.TPLIL STOPLIB, PBCON STOPCONTENTIEUX, CONTSTOP.TPLIL STOPCONTENTIEUXLIB,
PBCTD DUREE, PBCTU DUREEUNITE, DR.TPLIB DUREESTR,
PBRGT CODEREGIME, RG.TPLIB LIBREGIME, JDCNA SOUMISCATNAT, JDTFF MONTANTREF1, JDTMC MONTANTREF2, JDINA INDEXATION,JDIXL LCI,JDIXC ASSIETTE, JDIXF FRANCHISE,
JDDPV PREAVIS,PBTTR CODEACTION, TR.TPLIB LIBACTION,SIT.TPLIB LIBSITUATION,PBSTJ DATESITJOUR, PBSTM DATESITMOIS, PBSTA DATESITANNEE,PBCRU UCRCODE,UCR.UTNOM UCRNOM,
PBMJU UUPCODE, UUP.UTNOM UUPNOM,
PBAVN NUMAVENANT, PBAVK NUMEXTERNE,PBAVJ DATEEFFETAVNJOUR,PBAVM DATEEFFETAVNMOIS,PBAVA DATEEFFETAVNANNEE,PBOFF CODEOFFREORIGINE,PBOFV VERSIONOFFREORIGINE,
JDPEJ PROCHECHJ,JDPEM PROCHECHM, JDPEA PROCHECHA,JDEHH HORSCATNAT,JDEHC CATNAT,JDXCM TAUXHORSCATNAT,JDCNC TAUXCATNAT,
SSDTJ DATEAFFAIRENOUVELLEJOUR,SSDTM DATEAFFAIRENOUVELLEMOIS,SSDTA DATEAFFAIRENOUVELLEANNEE,PBCRJ DATECRJOUR, PBCRM DATECRMOIS, PBCRA DATECRANNEE,
PBMJJ DATEUPJOUR, PBMJM DATEUPMOIS, PBMJA DATEUPANNEE,
ADRESSEGESTIONNAIRE.ABPDP6 DPGESTIONNAIRE, ADRESSEGESTIONNAIRE.ABPCP6 CPGESTIONNAIRE, ADRESSEGESTIONNAIRE.ABPVI6 VILLEGESTIONNAIRE,
ADRESSEASSURE.ABPDP6 DPASSURE, ADRESSEASSURE.ABPCP6 CPASSURE, ADRESSEASSURE.ABPVI6 VILLEASSURE ,
ADRESSEAPPORTEUR.ABPDP6 DPAPPORTEUR, ADRESSEAPPORTEUR.ABPCP6 CPAPPORTEUR, ADRESSEAPPORTEUR.ABPVI6 VILLEAPPORTEUR,
ADRESSEPAYEUR.ABPDP6 DPPAYEUR, ADRESSEPAYEUR.ABPCP6 CPPAYEUR, ADRESSEPAYEUR.ABPVI6 VILLEPAYEUR
FROM YHPBASE
LEFT JOIN YHRTENT ON PBIPB = JDIPB AND PBALX = JDALX AND JDAVN = PBAVN
LEFT JOIN HPENT ON KAAIPB = PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP AND KAAAVN = PBAVN
LEFT JOIN YHPCOAS ON PBIPB = PHIPB AND PBALX = PHALX AND PHAVN = PBAVN
LEFT JOIN HPOBSV ON KAAOBSV = KAJCHR AND KAJAVN = PBAVN
LEFT JOIN YUTILIS UT1 ON PBSOU = UT1.UTIUT
LEFT JOIN YUTILIS UT2 ON PBGES = UT2.UTIUT
LEFT JOIN YUTILIS UCR ON PBCRU = UCR.UTIUT
LEFT JOIN YUTILIS UUP ON PBMJU = UUP.UTIUT
LEFT JOIN YCOMPA ON CIICI = PHCIE
LEFT JOIN KCIBLE ON KAACIBLE = KAHCIBLE
LEFT JOIN YSTAPRO ON SSIPW = PBIPB AND SSALW = PBALX
LEFT JOIN YCOURTN YCGest ON YCGest.TNICT = PBICT AND YCGest.TNXN5 = 0 AND YCGest.TNTNM = 'A'
LEFT JOIN YCOURTI Gestionnaire ON Gestionnaire.TCICT=PBICT
LEFT JOIN YASSNOM ON ANIAS = PBIAS AND ANINL = 0 AND ANTNM = 'A'
LEFT JOIN YCOURTN YCApp ON YCApp.TNICT = PBCTA AND YCApp.TNXN5 = 0 AND YCApp.TNTNM = 'A'
LEFT JOIN YCOURTN YCPay ON YCPay.TNICT = PBCTP AND YCPay.TNXN5 = 0 AND YCPay.TNTNM = 'A'
{BuildJoinYYYYPAR("LEFT", "GENER", "TAXRG", "RG", " AND RG.TCOD = PBRGT")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", "TR", " AND TR.TCOD = PBTTR")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBSIT", "SIT", " AND SIT.TCOD = PBSIT")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBNPL", "NATURE", " AND PBNPL = NATURE.TCOD")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBETA", "ETAT", " AND ETAT.TCOD = PBETA")}
{BuildJoinYYYYPAR("LEFT", "GENER", "BRCHE", "BRCH", " AND BRCH.TCOD = PBBRA AND BRCH.TPCN2 = 1")}
{BuildJoinYYYYPAR("LEFT", "GENER", "DEVIS", "DEVISE", " AND DEVISE.TCOD = PBDEV")}
{BuildJoinYYYYPAR("LEFT", "GENER", "INDIC", "IND", " AND IND.TCOD = JDIND")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBPER", "PERD", " AND PERD.TCOD = PBPER")}
{BuildJoinYYYYPAR("LEFT", "GENER", "TCYEN", "ENCAISS", " AND ENCAISS.TCOD = JDENC")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "QATRR", "TERRITO", " AND TERRITO.TCOD = JDTRR")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBSTF", "MOTIF", " AND MOTIF.TCOD = PBSTF")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBSTP", "SITSTOP", " AND SITSTOP.TCOD = PBSTP")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PKREL", "CONTSTOP", " AND CONTSTOP.TCOD = PBCON")}
{BuildJoinYYYYPAR("LEFT", "PRODU", "PBCTU", "DR", " AND DR.TCOD = PBCTU")}
LEFT JOIN YADRESS ADRESSEGESTIONNAIRE ON Gestionnaire.TCADH = ADRESSEGESTIONNAIRE.ABPCHR
LEFT JOIN YASSURE ON PBIAS = ASIAS
LEFT JOIN YADRESS ADRESSEASSURE ON ASADH = ADRESSEASSURE.ABPCHR
LEFT JOIN YCOURTI Apporteur ON Apporteur.TCICT=PBCTA
LEFT JOIN YADRESS ADRESSEAPPORTEUR ON Apporteur.TCADH = ADRESSEAPPORTEUR.ABPCHR
LEFT JOIN YCOURTI Payeur ON Payeur.TCICT = PBCTP
LEFT JOIN YADRESS ADRESSEPAYEUR ON Payeur.TCADH = ADRESSEPAYEUR.ABPCHR
WHERE PBIPB=:codeOffre AND PBTYP=:typeOffre AND PBALX=:versionOffre AND PBAVN = :numAvenenant";

            var parameters = MakeParams(sql, codeOffre.PadLeft(9, ' '), typeOffre, versionOffre, string.IsNullOrEmpty(numAvenant) ? "0" : numAvenant);

            var result = DbBase.Settings.ExecuteList<BandeauDto>(CommandType.Text, sql, parameters);

            if (result != null && result.Any())
            {
                toReturn = result.FirstOrDefault();
                toReturn.CPCourtierGest = ToCodePostal(toReturn.DepartementGestionnaire, toReturn.CodePostalGestionnaire);
                toReturn.VilleCourtierGest = toReturn.VilleGestionnaire;

                toReturn.CPPreneurAssurance = ToCodePostal(toReturn.DepartementAssure, toReturn.CodePostalAssure);
                toReturn.VillePreneurAssurance = toReturn.VilleAssure;

                toReturn.VilleCourtierAppo = ToCodePostal(toReturn.DepartementApporteur, toReturn.CodePostalApporteur) + " " + toReturn.VilleApporteur;

                toReturn.VilleCourtierPayeur = ToCodePostal(toReturn.DepartementPayeur, toReturn.CodePostalPayeur) + " " + toReturn.VilleCourtierPayeur;

                toReturn.HasDoubleSaisie = PoliceRepository.GetDoubleSaisie(codeOffre, int.Parse(versionOffre), typeOffre);

                if (typeOffre == AlbConstantesMetiers.TYPE_CONTRAT)
                {
                    var delegation = DelegationRepository.Obtenir(toReturn.CodeCourtierApporteur);
                    if (delegation != null)
                    {
                        toReturn.NomDelegation = delegation.Nom;
                        toReturn.NomInspecteur = delegation.Inspecteur != null ? delegation.Inspecteur.Nom : string.Empty;
                        toReturn.Secteur = delegation.Secteur;
                        toReturn.LibSecteur = delegation.LibSecteur;
                    }

                    toReturn.MontantStatistique = AffaireNouvelleRepository.GetMontantStatistique(codeOffre, versionOffre);
                }
                toReturn.TauxAvailable = true;

            }
            return toReturn;
        }

        public static InfosBaseDto GetInfosBaseOffre(string codeOffre, string version, string type, string codeAvn, string modeNavig)
        {
            // ARA - Basculement du mode navig S => H
            long numInterne = 0;
            EacParameter[] param = new EacParameter[3];
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

            string sqlInterne = @"SELECT PBAVN INT32RETURNCOL FROM YPOBASE WHERE PBIPB = :codeContrat AND PBALX = :version AND PBTYP = :type";
            var resultInterne = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlInterne, param);
            if (resultInterne != null && resultInterne.Any())
            {
                numInterne = resultInterne.FirstOrDefault().Int32ReturnCol;
            }
            if (numInterne.ToString() != codeAvn)
            {
                modeNavig = "H";
            }
            else
            {
                modeNavig = "S";
            }
            ModeConsultation modNavig = modeNavig.ParseCode<ModeConsultation>();
            InfosBaseDto toReturn = new InfosBaseDto();

            string sql = string.Format(
                @"
SELECT TRIM(PBIPB) CODEOFFRE, PBALX VERSIONOFFRE, PBTYP TYPEOFFRE, PBBRA CODEBRANCHE,
    KAACIBLE CODECIBLE, KAHDESC NOMCIBLE,
    PBICT CODECABINETCOURTAGE,
    PBIAS CODEASSURE,
    PBREF DESCRIPTIF,
    PBPER PERIODICITE,
    JDIND INDICEREFERENCE,
    PBIN5 CODEINTERLOCUTEUR,
    PBEFA DATEEFFETGARANTIEANNEE, PBEFM DATEEFFETGARANTIEMOIS, PBEFJ DATEEFFETGARANTIEJOUR, PBEFH DATEEFFETGARANTIEHEURE,
    PBFEA FINEFFETGARANTIEANNEE, PBFEM FINEFFETGARANTIEMOIS, PBFEJ FINEFFETGARANTIEJOUR, PBFEH FINEFFETGARANTIEHEURE,
    PBAVA DATEAVENANTANNEE, PBAVM DATEAVENANTMOIS, PBAVJ DATEAVENANTJOUR, KAAAVH DATEAVENANTHEURE,
    PBCTD DUREEGARANTIE, PBCTU UNITETEMPS,
    PBETA ETATOFFRE, PBSIT SITUATION,PBAVN NUMAVENANT,
    PBNPL CODENATURECONTRAT, NATURE.TPLIL LIBELLENATURECONTRAT,
    KAJOBSV OBSERVATION,
    KAAARTYG TYPEGESTION,PBTTR TYPETRAITEMENT
FROM {0}
LEFT JOIN YADRESS ON PBADH = ABPCHR
LEFT JOIN {1}  ON KAAIPB = PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP {2}
LEFT JOIN KCIBLE ON KAACIBLE = KAHCIBLE
LEFT JOIN YUTILIS UT1 ON UT1.UTIUT = PBSOU
LEFT JOIN YUTILIS UT2 ON UT2.UTIUT = PBGES
LEFT JOIN {3}  ON KAAOBSV = KAJCHR {6}
LEFT JOIN {4}  ON JDIPB = PBIPB  AND PBALX = JDALX {5}
LEFT JOIN YYYYPAR CPAYS ON CPAYS.TCON = 'GENER' AND CPAYS.TFAM = 'CPAYS' AND CPAYS.TCOD = ABPPAY
LEFT JOIN YYYYPAR NATURE ON NATURE.TCON = 'PRODU' AND NATURE.TFAM = 'PBNPL' AND PBNPL = NATURE.TCOD
WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvn",
                GetPrefixeHisto(modNavig, "YPOBASE"), // 0
                GetPrefixeHisto(modNavig, "KPENT"),
                modNavig == ModeConsultation.Historique ? "AND PBAVN = KAAAVN" : string.Empty,
                GetPrefixeHisto(modNavig, "KPOBSV"),
                GetPrefixeHisto(modNavig, "YPRTENT"),
                modNavig == ModeConsultation.Historique ? " AND PBAVN = JDAVN" : string.Empty, // 5
                modNavig == ModeConsultation.Historique ? " AND KAAAVN = KAJAVN" : string.Empty // 6
               );

            var parameters = MakeParams(sql, codeOffre.ToIPB(),
                Convert.ToInt32(version),
                type,
                codeAvn);

            var infosOffre = DbBase.Settings.ExecuteList<OffrePlatDto>(CommandType.Text, sql, parameters);

            if (infosOffre == null || !infosOffre.Any())
            {
                return toReturn;
            }



            var firstRes = infosOffre.FirstOrDefault();

            if (infosOffre != null)
            {
                toReturn = new InfosBaseDto
                {
                    CodeOffre = firstRes.CodeOffre.Trim(),
                    Version = firstRes.VersionOffre,
                    Type = firstRes.TypeOffre,
                    Descriptif = firstRes.Descriptif,
                    Branche = new BrancheDto { Code = firstRes.CodeBranche, Cible = new CibleDto { Code = firstRes.CodeCible, Nom = firstRes.NomCible } },
                    CabinetGestionnaire = CabinetCourtageRepository.Obtenir(firstRes.CodeCabinetCourtage),
                    PreneurAssurance = AssureRepository.Obtenir(firstRes.CodeAssure),
                    Interlocuteur = new InterlocuteurDto { Id = firstRes.CodeInterlocuteur },
                    Periodicite = firstRes.Periodicite,
                    IndiceReference = firstRes.IndiceReference,
                    DateEffetAnnee = firstRes.DateEffetGarantieAnnee,
                    DateEffetMois = firstRes.DateEffetGarantieMois,
                    DateEffetJour = firstRes.DateEffetGarantieJour,
                    DateEffetHeure = firstRes.DateEffetGarantieHeure,
                    FinEffetAnnee = firstRes.DateFinEffetGarantieAnnee,
                    FinEffetMois = firstRes.DateFinEffetGarantieMois,
                    FinEffetJour = firstRes.DateFinEffetGarantieJour,
                    FinEffetHeure = firstRes.DateFinEffetGarantieHeure,
                    Etat = firstRes.Etat,
                    DateAvnAnnee = firstRes.DateAvenantAnnee,
                    DateAvnMois = firstRes.DateAvenantMois,
                    DateAvnJour = firstRes.DateAvenantJour,
                    DateAvnHeure = firstRes.DateAvenantHeure,
                    Situation = firstRes.Situation,
                    Nature = firstRes.CodeNatureContrat,
                    LibNature = firstRes.LibelleNatureContrat,
                    Observation = firstRes.Observation,
                    NumAvenant = firstRes.NumAvenant,
                    Duree = (short)firstRes.DureeGarantie,
                    UniteTemps = firstRes.UniteTemps,
                    TypeGestionAvenant = firstRes.TypeGestionAvenant,
                    TypeTraitement = firstRes.TypeTraitement
                };
            }

            return toReturn;
        }
        /// <summary>
        /// Récupération des informations de base de AN
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <param name="modeNavig"></param>
        /// <returns></returns>
        public static InfosBaseDto LoadaffaireNouvelleBase(string codeOffre, string version, string type, string codeAvn, string modeNavig)
        {

            long numInterne = 0;
            EacParameter[] param = new EacParameter[3];
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

            string sqlInterne = @"SELECT PBAVN INT32RETURNCOL FROM YPOBASE WHERE PBIPB = :codeContrat AND PBALX = :version AND PBTYP = :type";
            var resultInterne = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlInterne, param);
            if (resultInterne != null && resultInterne.Any())
            {
                numInterne = resultInterne.FirstOrDefault().Int32ReturnCol;
            }
            if (numInterne.ToString() != codeAvn)
            {
                modeNavig = "H";
            }
            ModeConsultation modNavig = modeNavig.ParseCode<ModeConsultation>();
            InfosBaseDto toReturn = new InfosBaseDto();

            string sql = string.Format(
                @"
SELECT PBIPB CODEOFFRE, PBALX VERSIONOFFRE, PBTYP TYPEOFFRE, PBBRA CODEBRANCHE,
    KAACIBLE CODECIBLE, KAHDESC NOMCIBLE,
    PBICT CODECABINETCOURTAGE,
    PBIAS CODEASSURE,
    PBREF DESCRIPTIF,
    PBPER PERIODICITE,
    PBEFA DATEEFFETGARANTIEANNEE, PBEFM DATEEFFETGARANTIEMOIS, PBEFJ DATEEFFETGARANTIEJOUR, PBEFH DATEEFFETGARANTIEHEURE,
    PBFEA FINEFFETGARANTIEANNEE, PBFEM FINEFFETGARANTIEMOIS, PBFEJ FINEFFETGARANTIEJOUR, PBFEH FINEFFETGARANTIEHEURE,
    PBAVA DATEAVENANTANNEE, PBAVM DATEAVENANTMOIS, PBAVJ DATEAVENANTJOUR

FROM {0}
LEFT JOIN YADRESS ON PBADH = ABPCHR
LEFT JOIN {1}  ON KAAIPB = PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP {2}
LEFT JOIN KCIBLE ON KAACIBLE = KAHCIBLE
LEFT JOIN YUTILIS UT1 ON UT1.UTIUT = PBSOU
LEFT JOIN YUTILIS UT2 ON UT2.UTIUT = PBGES
LEFT JOIN {3}  ON KAAOBSV = KAJCHR
LEFT JOIN {4}  ON JDIPB = PBIPB  AND PBALX = JDALX {5}
LEFT JOIN YYYYPAR CPAYS ON CPAYS.TCON = 'GENER' AND CPAYS.TFAM = 'CPAYS' AND CPAYS.TCOD = ABPPAY
LEFT JOIN YYYYPAR NATURE ON NATURE.TCON = 'PRODU' AND NATURE.TFAM = 'PBNPL' AND PBNPL = NATURE.TCOD
WHERE PBIPB = '{6}' AND PBALX = {7} AND PBTYP = '{8}' AND PBAVN = {9}",
                GetPrefixeHisto(modNavig, "YPOBASE"), // 0
                GetPrefixeHisto(modNavig, "KPENT"),
                modNavig == ModeConsultation.Historique ? "AND PBAVN = KAAAVN" : string.Empty,
                GetPrefixeHisto(modNavig, "KPOBSV"),
                GetPrefixeHisto(modNavig, "YPRTENT"),
                modNavig == ModeConsultation.Historique ? " AND PBAVN = JDAVN" : string.Empty, // 5
                codeOffre.PadLeft(9, ' '),
                Convert.ToInt32(version),
                type,
                codeAvn);

            var infosOffre = DbBase.Settings.ExecuteList<OffrePlatDto>(CommandType.Text, sql);

            if (infosOffre == null || !infosOffre.Any())
            {
                return toReturn;
            }

            var firstRes = infosOffre.FirstOrDefault();
            var assureur = AssureRepository.GetBaseInfoAssureur(firstRes.CodeAssure);
            if (infosOffre != null)
            {
                toReturn = new InfosBaseDto
                {
                    CodeOffre = firstRes.CodeOffre.Trim(),
                    Version = firstRes.VersionOffre,
                    Type = firstRes.TypeOffre,
                    Descriptif = firstRes.Descriptif,
                    CabinetGestionnaire = CabinetCourtageRepository.Obtenir(firstRes.CodeCabinetCourtage),
                    Branche = new BrancheDto { Code = firstRes.CodeBranche, Cible = new CibleDto { Code = firstRes.CodeCible, Nom = firstRes.NomCible } },
                    PreneurAssurance = new AssureDto { Code = assureur?.Code, NomAssure = assureur?.Libelle },
                    Periodicite = firstRes.Periodicite,
                    DateEffetAnnee = firstRes.DateEffetGarantieAnnee,
                    DateEffetMois = firstRes.DateEffetGarantieMois,
                    DateEffetJour = firstRes.DateEffetGarantieJour,
                    DateEffetHeure = firstRes.DateEffetGarantieHeure,
                    FinEffetAnnee = firstRes.DateFinEffetGarantieAnnee,
                    FinEffetMois = firstRes.DateFinEffetGarantieMois,
                    FinEffetJour = firstRes.DateFinEffetGarantieJour,
                    FinEffetHeure = firstRes.DateFinEffetGarantieHeure,
                    DateAvnAnnee = firstRes.DateAvenantAnnee,
                    DateAvnMois = firstRes.DateAvenantMois,
                    DateAvnJour = firstRes.DateAvenantJour,

                };
            }

            return toReturn;
        }




        //SAB 110516 :bug2021
        public static VisuObservationsDto GetVisuObservations(string codeOffre, string version, string type)
        {
            string sql = string.Format(@"SELECT OBGEN.KAJOBSV OBSVGEN, OBCOM.KAJOBSV OBSVCOMM, OBENG.KAJOBSV OBSVENG, OBMNTREF.KAJOBSV OBSVMNTREF , OBREFGEST.KADDESI OBSVREFGEST
                                         FROM KPENT
                                         LEFT JOIN KPENG ON KAATYP = KDOTYP AND KAAIPB = KDOIPB AND KAAALX = KDOALX AND KDOECO = 'O'
                                         LEFT JOIN KPOBSV OBGEN ON OBGEN.KAJCHR = KAAOBSV
                                         LEFT JOIN KPOBSV OBCOM ON OBCOM.KAJCHR = KAAOBSC
                                         LEFT JOIN KPOBSV OBENG ON OBENG.KAJCHR = KDOOBSV
                                         LEFT JOIN KPOBSV OBMNTREF ON OBMNTREF.KAJCHR = KAAOBSF
                                         LEFT JOIN KPDESI OBREFGEST ON OBREFGEST.KADCHR = KAAAVDS
                                         WHERE KAATYP = '{0}' AND KAAIPB = '{1}' AND KAAALX = {2}", type, codeOffre.PadLeft(9, ' '), version);

            var result = DbBase.Settings.ExecuteList<VisuObservationsDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault();
            }
            else
            {
                return new VisuObservationsDto();
            }
        }

        public static VisuSuspensionDto GetVisuSuspension(string codeOffre, string version, string type)
        {
            var result = new VisuSuspensionDto();

            var param = new DbParameter[3];

            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') };
            param[1] = new EacParameter("P_TYPEOFFRE", DbType.AnsiStringFixedLength) { Value = type };
            param[2] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength) { Value = version };


            result.Suspensions = DbBase.Settings.ExecuteList<VisuListSuspensionDto>(CommandType.StoredProcedure, "SP_RECHERCHESUSPENSION", param) ??
                                 new List<VisuListSuspensionDto>();

            result.Suspensions.ForEach(i =>
            {
                i.DateDebut = AlbConvert.ConvertIntToDateHour((long?)i.DateDebutInt);
                i.DateFin = AlbConvert.ConvertIntToDateHour((long?)i.DateFinInt);
                i.DateMise = AlbConvert.ConvertIntToDate((int?)i.DateMiseInt);
                i.DateRemise = AlbConvert.ConvertIntToDate(i.DateRemiseInt);
                i.DateResil = AlbConvert.ConvertIntToDateHour((long?)(!i.DateResilInt.HasValue ? (decimal?)null : i.DateResilInt / 100));
            });


            result.InfosContrat = GetInfosContrat(codeOffre, version, type) ?? new VisuInfosContratDto();

            return result;
        }

        public static VisuInfosContratDto GetInfosContrat(string codeOffre, string version, string type)
        {
            var param = new DbParameter[3];

            param[0] = new EacParameter("codeOffre", codeOffre.Replace("'", "''").PadLeft(9, ' '));
            param[1] = new EacParameter("version", !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0);
            param[2] = new EacParameter("type", type);

            string sql = @"SELECT  ((YPO.PBEFA * 10000) + (YPO.PBEFM * 100) + (YPO.PBEFJ)) DATEDEBUTEFFET,
                                   ((YPO.PBFEA * 10000) + (YPO.PBFEM * 100) + (YPO.PBFEJ)) DATEFINEFFET,
                                   (YPO.PBPER CONCAT ' - ' CONCAT TRIM(PERIO.TPLIB)) PERIODICITE,
                                   PBCTA CONCAT ' - ' CONCAT TRIM(COURTAPP.TNNOM) COURTIERA,
                                   PBICT CONCAT ' - ' CONCAT TRIM(COURTGES.TNNOM) COURTIERG,
                                   YPO.PBIAS CONCAT ' - ' CONCAT TRIM(ASSUR.ANNOM) ASSURE,
                                   PBREF IDENTIF,
                                   NATURE.TPLIB  NATURECONTRAT,
                                   TRIM(TYPECON.TCOD) CONCAT ' - ' CONCAT TRIM(TYPECON.TPLIB) TYPECONTRAT,
                                   TRIM(SOUS.UTIUT) CONCAT ' - ' CONCAT TRIM(SOUS.UTNOM) SOUSCRIPTEUR,
                                   TRIM(GEST.UTIUT) CONCAT ' - ' CONCAT TRIM(GEST.UTNOM) GESTIONNAIRE,
                                   TRIM(OBSV.KAJOBSV) OBSERVATION ,
                                   PBTAC CONCAT ' - ' CONCAT TRIM(SIGNE.TPLIB) RETSIGNE,
                                   ((ENT.JDPEA * 10000) + (ENT.JDPEM * 100) + (ENT.JDPEJ)) DATEECHEANCE,
                                   YPO.PBIPB CODEOFFRE,
                                   YPO.PBALX VERSION

                         FROM YPOBASE YPO
		                         LEFT JOIN YYYYPAR PERIO ON PERIO.TCOD = pbper AND PERIO.TFAM = 'PBPER' AND PERIO.TCON = 'PRODU'
		                         LEFT JOIN YYYYPAR NATURE ON NATURE.TCOD = YPO.PBNPL AND NATURE.TFAM = 'PBNPL' AND NATURE.TCON = 'PRODU'
		                         LEFT JOIN YYYYPAR TYPECON ON TYPECON.TCOD = IFNULL(NULLIF(PBMER, ''), 'S') AND TYPECON.TFAM = 'TYPOC' AND TYPECON.TCON = 'KHEOP'
		                         LEFT JOIN YYYYPAR SIGNE ON SIGNE.TCOD = YPO.PBTAC AND SIGNE.TFAM = 'PBTAC' AND SIGNE.TCON = 'PRODU'
		                         LEFT JOIN YCOURTN COURTGES ON COURTGES.TNICT = PBICT AND COURTGES.TNXN5 = 0 AND COURTGES.TNTNM = 'A'
		                         LEFT JOIN YCOURTN COURTAPP ON COURTAPP.TNICT = PBCTA AND COURTAPP.TNXN5 = 0 AND COURTAPP.TNTNM = 'A'
		                         LEFT JOIN YCOURTI YCO ON YCO.TCICT = COURTAPP.TNICT
		                         LEFT JOIN YBUREAU BUR ON BUR.BUIBU = YCO.TCBUR
		                         LEFT JOIN YASSNOM ASSUR ON ASSUR.ANIAS = YPO.PBIAS AND ASSUR.ANINL = 0 AND ASSUR.ANTNM = 'A'
		                         LEFT JOIN YUTILIS SOUS ON SOUS.UTIUT = YPO.PBSOU
                                 LEFT JOIN YUTILIS GEST ON GEST.UTIUT = YPO.PBGES
                                 LEFT JOIN KPOBSV OBSV ON OBSV.KAJIPB = YPO.PBIPB AND OBSV.KAJALX = YPO.PBALX AND OBSV.KAJTYP = YPO.PBTYP AND OBSV.KAJTYPOBS =  'GENERALE'
                                 LEFT JOIN YPRTENT ENT ON ENT.JDIPB = YPO.PBIPB AND ENT.JDALX = YPO.PBALX
                         WHERE YPO.PBIPB = :codeOffre AND YPO.PBALX = :version AND YPO.PBTYP = :type";

            var result = DbBase.Settings.ExecuteList<VisuInfosContratDto>(CommandType.Text, sql, param).FirstOrDefault();

            if (result != null)
            {
                result.DateDebutEffet = AlbConvert.ConvertIntToDate((int?)result.DateDebutEffetInt);
                result.DateFinEffet = AlbConvert.ConvertIntToDate((int?)result.DateFinEffetInt);
                result.DateEcheance = AlbConvert.ConvertIntToDate((int?)result.DateEcheanceInt);
            }

            return result;
        }
        #endregion

        #region Gestion des intervenants

        public static List<IntervenantDto> GetListeIntervenants(string code, string version, string type, string orderby, string ascDesc)
        {
            string sql = string.Format(@"SELECT KHBID GUIDID,
                                                KHBTYI TYPEINTERVENANT,
                                                NOMINTERVENANT.IMIIN CODEINTERVENANT,
                                                NOMINTERVENANT.IMNOM NOMINTERVENANT,
                                                CONCAT(INDEP, INCPO) CODEPOSTAL,
                                                INVIL VILLE,
                                                KHBINL CODEINTERLO,
                                                NOMINTERLO.IMNOM NOMINTERLO,
                                                KHBREF REFERENCE,
                                                KHBMDC ISMEDECINCONSEIL,
                                                KHBPRP ISPRINCIPAL,
                                                CASE WHEN INFVA > 0 THEN 'O' ELSE 'N' END ISFINVALIDITE
                                                FROM KPINTER
                                                LEFT JOIN YINTNOM NOMINTERVENANT ON NOMINTERVENANT.IMIIN = KHBIIN AND NOMINTERVENANT.IMINL = 0 AND NOMINTERVENANT.IMTNM = 'A'
                                                LEFT JOIN YINTERV ON INIIN = KHBIIN
                                                LEFT JOIN YINTNOM NOMINTERLO ON NOMINTERLO.IMINL = KHBINL AND NOMINTERLO.IMTNM = 'A' AND NOMINTERLO.IMIIN = KHBIIN AND NOMINTERLO.IMINL > 0
                                                WHERE KHBTYP = '{0}'
                                                AND KHBIPB = '{1}'
                                                AND KHBALX = {2}", type, code.ToIPB(), version);
            if (!string.IsNullOrEmpty(orderby) && !string.IsNullOrEmpty(ascDesc))
            {
                sql += string.Format(" ORDER BY {0} {1}", orderby, ascDesc);
                if (orderby == "TypeIntervenant")
                {
                    sql += string.Format(" , NOMINTERVENANT.IMNOM");
                }
            }
            else
            {
                sql += string.Format(" ORDER BY KHBTYI, NOMINTERVENANT.IMNOM");
            }

            return DbBase.Settings.ExecuteList<IntervenantDto>(CommandType.Text, sql);
        }

        public static List<IntervenantDto> GetListeIntervenantsAutocomplete(string code, string version, string type, string name, string typeIntervenant, string codeIntervenant, bool fromAffaireOnly)
        {
            string sql = string.Format(@"SELECT
                                                IMIIN CODEINTERVENANT,
                                                NOMINTERVENANT.IMNOM NOMINTERVENANT,
                                                INTYI TYPEINTERVENANT,
                                                CONCAT(INDEP, INCPO) CODEPOSTAL,
                                                INVIL VILLE,
                                                INFVA ANNEEFINVALIDITE,
                                                INFVM MOISFINVALIDITE,
                                                INFVJ JOURFINVALIDITE,
                                                INAD1 ADRESSE1,
                                                INAD2 ADRESSE2,
                                                INTEL TELEPHONE,
                                                INAEM EMAIL
                                                FROM
                                                YINTNOM NOMINTERVENANT
                                                LEFT JOIN YINTERV ON INIIN = IMIIN
                                                WHERE NOMINTERVENANT.IMINL = 0 AND NOMINTERVENANT.IMTNM = 'A'
                                                      AND IMIIN {3} IN (SELECT KHBIIN
                                                                        FROM KPINTER
                                                                        WHERE KHBTYP = '{0}'
                                                                              AND KHBIPB = '{1}'
                                                                              AND KHBALX = {2})", type, code.ToIPB(), version,
                                                                                                  fromAffaireOnly ? string.Empty : "NOT");
            if (!string.IsNullOrEmpty(name))
            {
                sql += string.Format(" AND UPPER(IMNOM) LIKE '%{0}%'", name.ToUpper());
            }
            if (!string.IsNullOrEmpty(typeIntervenant) && !fromAffaireOnly)
            {
                sql += string.Format(" AND INTYI = '{0}'", typeIntervenant);
            }
            if (!string.IsNullOrEmpty(codeIntervenant))
            {
                sql += string.Format(" AND IMIIN = {0}", codeIntervenant);
            }
            sql += " ORDER BY NOMINTERVENANT FETCH FIRST 30 ROWS ONLY";
            return DbBase.Settings.ExecuteList<IntervenantDto>(CommandType.Text, sql);
        }

        public static IntervenantDto GetIntervenantByCodeAutocomplete(string codeOffre, string type, string version, string codeIntervenant, bool fromAffaireOnly)
        {
            string sql = string.Format(@"SELECT IMIIN CODEINTERVENANT,
                                  NOMINTERVENANT.IMNOM NOMINTERVENANT,
                                  CONCAT(INDEP, INCPO) CODEPOSTAL,
                                  INVIL VILLE,
                                  INFVA ANNEEFINVALIDITE,
                                  INFVM MOISFINVALIDITE,
                                  INFVJ JOURFINVALIDITE,
                                  INAD1 ADRESSE1,
                                  INAD2 ADRESSE2,
                                  INTEL TELEPHONE,
                                  INAEM EMAIL
                           FROM
                                  YINTNOM NOMINTERVENANT
                                  LEFT JOIN YINTERV ON INIIN = IMIIN
                                  WHERE NOMINTERVENANT.IMINL = 0 AND NOMINTERVENANT.IMTNM = 'A'
                                        AND IMIIN {3} IN (SELECT KHBIIN
                                                          FROM KPINTER
                                                          WHERE KHBTYP = '{0}'
                                                                AND KHBIPB = '{1}'
                                                                AND KHBALX = {2})", type, codeOffre.PadLeft(9, ' '), version,
                                                                                    fromAffaireOnly ? string.Empty : "NOT");

            if (!string.IsNullOrEmpty(codeIntervenant))
            {
                if (!int.TryParse(codeIntervenant, out int code))
                {
                    return null;
                }

                sql += string.Format(" AND IMIIN= {0}", codeIntervenant);
            }
            var result = DbBase.Settings.ExecuteList<IntervenantDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault();
            }

            return null;
        }

        public static List<IntervenantDto> GetListeInterlocuteursByIntervenant(long codeIntervenant, string name)
        {
            string sql = string.Format(@"SELECT IMINL CODEINTERVENANT,
                                                IMNOM NOMINTERVENANT,
                                                CONCAT(INDEP, INCPO) CODEPOSTAL,
                                                INVIL VILLE,
                                                INFVA ANNEEFINVALIDITE,
                                                INFVM MOISFINVALIDITE,
                                                INFVJ JOURFINVALIDITE
                                         FROM YINTNOM
                                         LEFT JOIN YINTERV ON INIIN = IMIIN
                                         WHERE IMIIN = {0} AND IMTNM = 'A' AND IMINL <> 0", codeIntervenant);
            if (!string.IsNullOrEmpty(name))
            {
                sql += string.Format(" AND UPPER(IMNOM) LIKE '%{0}%'", name.ToUpper());
            }
            sql += " ORDER BY NOMINTERVENANT FETCH FIRST 10 ROWS ONLY";
            return DbBase.Settings.ExecuteList<IntervenantDto>(CommandType.Text, sql);
        }

        public static List<IntervenantDto> EnregistrerDetailsIntervenant(string code, string version, string type, string codeAvenant, IntervenantDto toSave, string user)
        {
            string mode = "UPDATE";
            if (toSave.GuidId < 0)
            {
                //  toSave.GuidId = CommonRepository.GetAS400Id("KHBID");
                mode = "INSERT";
            }
            DateTime dateNow = DateTime.Now;

            DbParameter[] param = new DbParameter[15];
            param[0] = new EacParameter("P_MODE", mode);
            param[1] = new EacParameter("P_CODE", code.PadLeft(9, ' '));
            param[2] = new EacParameter("P_VERSION", 0)
            {
                Value = int.Parse(version)
            };
            param[3] = new EacParameter("P_TYPE", type);
            param[4] = new EacParameter("P_CODEAVENANT", 0)
            {
                Value = !string.IsNullOrEmpty(codeAvenant) ? Convert.ToInt32(codeAvenant) : 0
            };
            param[5] = new EacParameter("P_GUIDID", toSave.GuidId);
            param[6] = new EacParameter("P_TYPEINTERV", toSave.Type);
            param[7] = new EacParameter("P_IDINTERV", 0)
            {
                Value = toSave.CodeIntervenant
            };
            param[8] = new EacParameter("P_CODEINTERLOC", 0)
            {
                Value = toSave.CodeInterlocuteur
            };
            param[9] = new EacParameter("P_REFERENCE", toSave.Reference);
            param[10] = new EacParameter("P_ISPRINCIPAL", toSave.IsPrincipal == "O" ? "O" : "N");
            param[11] = new EacParameter("P_ISMEDECINCONSEIL", toSave.IsMedecinConseil == "O" ? "O" : "N");
            param[12] = new EacParameter("P_USER", user);
            param[13] = new EacParameter("P_DATENOW", 0)
            {
                Value = AlbConvert.ConvertDateToInt(dateNow)
            };
            param[14] = new EacParameter("P_HEURENOW", 0)
            {
                Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow))
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEINTERVENANT", param);

            return GetListeIntervenants(code, version, type, string.Empty, string.Empty);
        }

        public static IntervenantDto GetDetailsIntervenant(long guidId)
        {
            string sql = string.Format(@"SELECT KHBID GUIDID,
                                                KHBTYI TYPEINTERVENANT,
                                                KHBIIN CODEINTERVENANT,
                                                NOMINTERVENANT.IMNOM NOMINTERVENANT,
                                                CONCAT(INDEP, INCPO) CODEPOSTAL,
                                                INVIL VILLE,
                                                KHBINL CODEINTERLO,
                                                NOMINTERLO.IMNOM NOMINTERLO,
                                                KHBREF REFERENCE,
                                                KHBMDC ISMEDECINCONSEIL,
                                                KHBPRP ISPRINCIPAL,
                                                CASE WHEN INFVA > 0 THEN 'O' ELSE 'N' END ISFINVALIDITE,
                                                INFVA ANNEEFINVALIDITE,
                                                INFVM MOISFINVALIDITE,
                                                INFVJ JOURFINVALIDITE,
                                                INAD1 ADRESSE1,
                                                INAD2 ADRESSE2,
                                                INTEL TELEPHONE,
                                                INAEM EMAIL
                                        FROM KPINTER
                                                LEFT JOIN YINTNOM NOMINTERVENANT ON NOMINTERVENANT.IMIIN = KHBIIN AND NOMINTERVENANT.IMINL = 0 AND NOMINTERVENANT.IMTNM = 'A'
                                                LEFT JOIN YINTERV ON INIIN = KHBIIN
                                                LEFT JOIN YINTNOM NOMINTERLO ON NOMINTERLO.IMINL = KHBINL AND NOMINTERLO.IMTNM = 'A' AND NOMINTERLO.IMIIN = KHBIIN AND NOMINTERLO.IMINL > 0
                                                WHERE KHBID={0}", guidId);
            var result = DbBase.Settings.ExecuteList<IntervenantDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault();
            }

            return null;
        }

        public static List<IntervenantDto> SupprimerIntervenant(string code, string version, string type, string codeAvenant, long guidId, string user)
        {
            string sql = string.Format(@"DELETE FROM KPINTER
                                         WHERE KHBID={0}", guidId);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            if (!string.IsNullOrEmpty(codeAvenant) && codeAvenant != "0")
            {
                DateTime dateNow = DateTime.Now;
                sql = string.Format(@"DELETE FROM KPAVTRC WHERE KHOIPB = '{0}' AND KHOALX = {1} AND KHOTYP = '{2}' AND TRIM(KHOPERI) = 'ASSADDREF' AND KHOACT = 'X'",
                            code.ToIPB(), version, type);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                sql = string.Format(@"INSERT INTO KPAVTRC
                        (KHOID, KHOIPB, KHOALX, KHOTYP, KHOPERI, KHOETAPE, KHOACT, KHOCRU, KHOCRD, KHOCRH)
                    VALUES
                        ({0}, '{1}', {2}, '{3}', '{4}', '**********', 'X', '{5}', {6}, {7})",
                        GetAS400Id("KHOID"), code.PadLeft(9, ' '), version, type, "INTERVE", user, AlbConvert.ConvertDateToInt(dateNow), AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow)));
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }


            return GetListeIntervenants(code, version, type, string.Empty, string.Empty);
        }

        #endregion



        #region Avenants

        public static bool ExistTraceAvenant(string codeOffre, string version, string type, string perimetre, string codeRisque, string codeObjet, string codeFormule, string codeOption, string etape)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM KPAVTRC
                                         WHERE KHOTYP = '{0}' AND KHOIPB = '{1}' AND KHOALX = {2} ", type, codeOffre.ToIPB(), version);
            if (!string.IsNullOrEmpty(perimetre))
            {
                sql += string.Format(@" AND TRIM(KHOPERI) = TRIM('{0}')", perimetre);
            }

            if (!string.IsNullOrEmpty(codeRisque))
            {
                sql += string.Format(@" AND KHORSQ = '{0}'", codeRisque);
            }

            if (!string.IsNullOrEmpty(codeObjet))
            {
                sql += string.Format(@" AND KHOOBJ = '{0}'", codeObjet);
            }

            if (!string.IsNullOrEmpty(codeFormule))
            {
                sql += string.Format(@" AND KHOFOR = '{0}'", codeFormule);
            }

            if (!string.IsNullOrEmpty(codeOption))
            {
                sql += string.Format(@" AND KHOOPT = '{0}'", codeOption);
            }

            if (!string.IsNullOrEmpty(etape))
            {
                sql += string.Format(@" AND TRIM(KHOETAPE) = TRIM('{0}')", etape);
            }

            return ExistRow(sql);
        }

        public static void SaveTraceAvenant(string codeOffre, string version, string type, string perimetre, string codeRisque, string codeObjet, string codeFormule, string codeOption, string etape,
                                            string champ, string action, string oldValue, string newValue, string obligatoire, string operation, string user)
        {
            DbParameter[] param = new DbParameter[18];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_PERIMETRE", perimetre);
            param[4] = new EacParameter("P_CODERISQUE", 0)
            {
                Value = !string.IsNullOrEmpty(codeRisque) ? Convert.ToInt32(codeRisque) : 0
            };
            param[5] = new EacParameter("P_CODEOBJET", 0)
            {
                Value = !string.IsNullOrEmpty(codeObjet) ? Convert.ToInt32(codeObjet) : 0
            };
            param[6] = new EacParameter("P_CODEFORMULE", 0)
            {
                Value = !string.IsNullOrEmpty(codeFormule) ? Convert.ToInt32(codeFormule) : 0
            };
            param[7] = new EacParameter("P_CODEOPTION", 0)
            {
                Value = !string.IsNullOrEmpty(codeOption) ? Convert.ToInt32(codeOption) : 0
            };
            param[8] = new EacParameter("P_ETAPE", etape);
            param[9] = new EacParameter("P_CHAMP", champ);
            param[10] = new EacParameter("P_ACTION", action);
            param[11] = new EacParameter("P_OLDVAL", oldValue);
            param[12] = new EacParameter("P_NEWVAL", newValue);
            param[13] = new EacParameter("P_AVNOBLIG", obligatoire);
            param[14] = new EacParameter("P_OPERATION", operation);
            param[15] = new EacParameter("P_USER", user);
            param[16] = new EacParameter("P_DATENOW", 0)
            {
                Value = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"))
            };
            param[17] = new EacParameter("P_HEURENOW", 0)
            {
                Value = Convert.ToInt32(DateTime.Now.ToString("HHmmss"))
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_AVENANTRACE", param);
        }

        #endregion
        #region Edition des documents

        public static int GetNumeroPrime(string codeOffre, string version, string type, string avenant)
        {
            string sql = string.Format(@"SELECT PKIPK INT32RETURNCOL FROM YPRIMES WHERE PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {2}",
                                codeOffre.ToIPB(), version, !string.IsNullOrEmpty(avenant) ? avenant : "0");
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().Int32ReturnCol;
            }

            return 0;
        }

        #endregion
        #region Get Utilisateurs

        public static List<UtilisateurBrancheDto> GetUtilisateurBranchesByCriteria(KUSRDRT_COl criteriaCol,
            string criteriaValue,
            WHERE_OPER comapreOpertor)
        {
            string sql = string.Empty;
            string condition = string.Empty;
            string criteriaColValue = string.Empty;

            switch (criteriaCol)
            {
                case KUSRDRT_COl.Utilisateur:
                    criteriaColValue = "KHRUSR";
                    break;
                case KUSRDRT_COl.Branche:
                    criteriaColValue = "KHRBRA";
                    break;
                case KUSRDRT_COl.TypeDroit:
                    criteriaColValue = "KHRTYD";
                    break;
            }

            if (comapreOpertor == WHERE_OPER.EQ)
            {
                condition = string.Format("{0} = '{1}'", criteriaColValue, criteriaValue);
            }
            else
            {
                condition = string.Format("{0} like '%{1}%'", criteriaColValue, criteriaValue);
            }

            sql = string.Format(@"SELECT KHRUSR UTILISATEUR, KHRBRA BRANCHE, KHRTYD TYPEDROIT
                    FROM KUSRDRT
                    WHERE {0}
                    ORDER BY KHRUSR ASC", condition);

            return DbBase.Settings.ExecuteList<UtilisateurBrancheDto>(CommandType.Text, sql);
        }


        public static List<UtilisateurBrancheDto> GetUtilisateurBranches(string utilisateur, string branche, string typeDroit)
        {
            string sql = string.Empty;
            sql = string.Format(@"SELECT KHRUSR UTILISATEUR, KHRBRA BRANCHE, KHRTYD TYPEDROIT
                    FROM KUSRDRT
                    WHERE UPPER(KHRUSR) LIKE UPPER('%{0}%') ", utilisateur);

            if (!string.IsNullOrEmpty(branche))
            {
                sql += string.Format(" AND KHRBRA = '{0}' ", branche);
            }

            if (!string.IsNullOrEmpty(typeDroit))
            {
                sql += string.Format(" AND KHRTYD = '{0}' ", typeDroit);
            }

            sql += " ORDER BY KHRUSR";

            return DbBase.Settings.ExecuteList<UtilisateurBrancheDto>(CommandType.Text, sql);
        }

        public static UtilisateurBrancheDto FirstOrDefaultUtilisateurBranche(string utilisateur, string branche)
        {
            string sql = string.Empty;
            sql = string.Format(@"SELECT KHRUSR UTILISATEUR, KHRBRA BRANCHE, KHRTYD TYPEDROIT
                    FROM KUSRDRT
                    WHERE KHRUSR='{0}' AND KHRBRA='{1}'
                    ORDER BY KHRUSR ASC", utilisateur, branche);

            return DbBase.Settings.ExecuteList<UtilisateurBrancheDto>(CommandType.Text, sql).FirstOrDefault();
        }

        public static List<UtilisateurBrancheDto> LoadListUtilisateurBranche()
        {
            string sql = string.Empty;
            sql = @"SELECT KHRUSR UTILISATEUR, KHRBRA BRANCHE, KHRTYD TYPEDROIT
                    FROM KUSRDRT
                    ORDER BY KHRUSR ASC";

            return DbBase.Settings.ExecuteList<UtilisateurBrancheDto>(CommandType.Text, sql);
        }

        public static void AddUtilisateurBranche(UtilisateurBrancheDto utilisateurBranche)
        {

            string sqlCount = string.Format(@"SELECT COUNT(*) NBLIGN FROM KUSRDRT
                                              WHERE KHRUSR='{0}' AND KHRBRA='{1}'",
                                                          utilisateurBranche.Utilisateur,
                                                          utilisateurBranche.Branche);
            if (!ExistRow(sqlCount))
            {
                string sql = string.Format(@"INSERT INTO KUSRDRT(KHRUSR,KHRBRA,KHRTYD)
                                          VALUES('{0}','{1}','{2}')",
                                                         utilisateurBranche.Utilisateur,
                                                         utilisateurBranche.Branche,
                                                         utilisateurBranche.TypeDroit);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }
            else
            {
                throw new Exception("Utilisateur déja existant");
            }
        }

        public static void UpdaterUtilisateurBranche(UtilisateurBrancheDto utilisateurBranche, string newTypeDroit)
        {
            string sql = string.Format(@"UPDATE KUSRDRT
                                        SET KHRTYD='{0}'
                                        WHERE KHRUSR='{1}' AND KHRBRA='{2}'",
                                                      newTypeDroit,
                                                      utilisateurBranche.Utilisateur,
                                                      utilisateurBranche.Branche);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

        }

        public static void DeleteUtilisateurBranche(UtilisateurBrancheDto utilisateurBranche)
        {
            string sql = string.Format(@"DELETE FROM KUSRDRT
                                        WHERE KHRUSR='{0}' AND KHRBRA='{1}'",
                                                      utilisateurBranche.Utilisateur,
                                                      utilisateurBranche.Branche);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

        }

        #endregion
        #region ParamCibleRecup
        /// <summary>
        ///Alimente la matrice des risques
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static void RecupOffreContratAS400(string codeOffre, string version, string type)
        {
            var param = new DbParameter[3];
            string sql = @"*PGM/KR050CLD";
            param[0] = new EacParameter("P0TYP", type);
            param[1] = new EacParameter("P0IPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P0ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt16(version) : 0
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);
        }
        #endregion

        #region Stat offre/Contrat
        /// <summary>
        /// checkBox KHE	checkBox  Non KHE	                resultat requete
        ///0	                0	                            rien => cas 1
        ///1	                0	                            PBORK==  KHE     => cas 2
        ///0	                1	                            PBORK <> KHE     => cas 3
        ///1	                1	                            pas de filtre = all => cas 4
        /// </summary>
        /// <param name="filtre"></param>
        /// <returns></returns>
        public static List<StatOffreContratDto> GetStatOffreContrat(StatOffreContratFiltreDto filtre)
        {
            string sql = string.Empty;
            sql = @"SELECT PBIPB NUMERO,
                    PBALX VERSION,
                    PBTYP TYPE,
                    PBREF REFERENCE,
                    PBGES GESTIONNAIRE,
                    PBSOU SOUSCRIPTEUR,
                    PBTBR BRANCHE,
                    PBCAT CIBLE
                    FROM  YPOBASE
                    INNER JOIN  YSTAPRO ON PBIPB=SPOL
                    INNER JOIN YLGNSTP ON SSAFF=SLAFF AND SSOUS=SLSOU";

            List<string> filtres = new List<string>();
            string str = string.Empty;

            if (!string.IsNullOrEmpty(filtre.Branche))
            {
                str = string.Format(" PBBRA='{0}' ", filtre.Branche);
                filtres.Add(str);
            }

            if (filtre.Categorie == "2")
            {
                str = " PBORK ='KHE' ";
                filtres.Add(str);
            }
            else if (filtre.Categorie == "3")
            {
                str = " PBORK <> 'KHE' ";
                filtres.Add(str);
            }

            if (!string.IsNullOrEmpty(filtre.Situation))
            {
                str = string.Format(" PBSIT='{0}' ", filtre.Situation);
                filtres.Add(str);
            }

            if (!string.IsNullOrEmpty(filtre.Etat))
            {
                str = string.Format(" PBETA='{0}' ", filtre.Etat);
                filtres.Add(str);
            }

            if (!string.IsNullOrEmpty(filtre.Type))
            {
                str = string.Format(" PBTYP='{0}' ", filtre.Type);
                filtres.Add(str);
            }

            if (!string.IsNullOrEmpty(filtre.Annee))
            {
                str = string.Format(" PBMJA='{0}' ", filtre.Annee);
                filtres.Add(str);
            }

            if (!string.IsNullOrEmpty(filtre.Mois))
            {
                str = string.Format(" PBMJM='{0}' ", filtre.Mois);
                filtres.Add(str);
            }

            if (!string.IsNullOrEmpty(filtre.Jour))
            {
                str = string.Format(" PBMJJ='{0}' ", filtre.Jour);
                filtres.Add(str);
            }

            if (filtres.Count > 0)
            {
                sql += " WHERE " + string.Join(" AND ", filtres.ToArray());
            }

            sql += " ORDER BY PBBRA DESC";

            return DbBase.Settings.ExecuteList<StatOffreContratDto>(CommandType.Text, sql);
        }
        #endregion
        #region Stat Clauses Libres
        public static List<StatClausesLibresDto> GetStatClausesLibres(ParamRecherClausLibDto paramrecherche)
        {

            DbParameter[] param = new DbParameter[4];
            string sql = @"SELECT B2.BUDBU DELEGATIONUSER, PBCRU CREATEUSER,
	                RIGHT(REPEAT('0', 2) || PBCRJ, 2) CONCAT '/' CONCAT RIGHT(REPEAT('0', 2) || PBCRM, 2)
	                CONCAT '/' CONCAT PBCRA DATECREATION,
	                right(repeat('0', 2) || pbsaj, 2) concat '/' concat right(repeat('0', 2) || pbsam, 2)
	                concat '/' concat pbsaa datesaisie,
	                PBIPB NUMPOLICE, PBALX VERSION, PBICT CODECOURTIER, TNNOM NOMCOURTIER,
	                B1.BUDBU DELEGATIONCOURTIER,
	                PBIAS CODEPRENEURASS, ANNOM NOMPRENEURASS,
	                PBSOU SOUSCRIPTEUR

                    FROM YPOBASE
	                    INNER JOIN YCOURTN ON TNICT = PBICT AND TNINL = 0
	                    AND TNTNM = 'A' AND TNXN5 = 0
	                    INNER JOIN YCOURTI ON TNICT = TCICT
	                    INNER JOIN YBUREAU B1 ON B1.BUIBU = TCBUR
	                    INNER JOIN YASSNOM ON ANIAS = PBIAS AND ANINL = 0
	                    AND ANTNM = 'A'
	                    INNER JOIN YUTILIS ON UTIUT = PBCRU
	                    INNER JOIN YBUREAU B2 ON B2.BUIBU = UTBUR
	                    WHERE PBTYP = 'P' AND PBORK = 'KHE' AND EXISTS
	                    (SELECT KCAID FROM KPCLAUSE
	                    WHERE KCAIPB = PBIPB AND KCAALX = PBALX AND KCATYP = PBTYP AND KCACLNM2 = 'LIBRE')";


            List<string> filtres = new List<string>();
            string strFiltredatecreation = string.Empty;
            string strFiltredatesaisie = string.Empty;

            var dDebSaisie = AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(paramrecherche.DateSaisieDebut));
            param[2] = new EacParameter("datesaisiedebut", 0)
            {
                Value = dDebSaisie != null ? dDebSaisie : 0
            };
            var dFinSaisie = AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(paramrecherche.DateSaisieFin));
            param[3] = new EacParameter("datesaisiefin", 0)
            {
                Value = dFinSaisie != null ? dFinSaisie : 0
            };


            var dDebcreation = AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(paramrecherche.DateCreationDebut));
            param[0] = new EacParameter("datescreationdebut", 0)
            {
                Value = dDebcreation != null ? dDebcreation : 0
            };
            var dFinCreation = AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(paramrecherche.DateCreationFin));
            param[1] = new EacParameter("datecreationfin", 0)
            {
                Value = dFinCreation != null ? dFinCreation : 0
            };

            strFiltredatecreation = FiltreDateCreation(paramrecherche, param, filtres, strFiltredatecreation);
            strFiltredatesaisie = FiltreDateSaisie(paramrecherche, param, filtres, strFiltredatesaisie);

            if (filtres.Count > 0)
            {
                sql += " AND " + string.Join(" AND ", filtres.ToArray());
            }
            sql += "ORDER BY NUMPOLICE ";


            return DbBase.Settings.ExecuteList<StatClausesLibresDto>(CommandType.Text, sql, param);
        }

        private static string FiltreDateCreation(ParamRecherClausLibDto paramrecherche, DbParameter[] param, List<string> filtres, string str)
        {
            if (!string.IsNullOrEmpty(paramrecherche.DateCreationDebut))
            {
                str = "((pbcra * 10000 + pbcrm * 100 + pbcrj) >= :datecreationdebut)";
                filtres.Add(str);
            }
            else
            {
                str = "((pbcra * 10000 + pbcrm * 100 + pbcrj) >= :datecreationdebut or 1=1)";
                filtres.Add(str);
            }
            if (!string.IsNullOrEmpty(paramrecherche.DateCreationFin))
            {
                str = "((pbcra * 10000 + pbcrm * 100 + pbcrj) <= :datecreationFin)";
                filtres.Add(str);
            }
            else
            {
                str = "((pbcra * 10000 + pbcrm * 100 + pbcrj) <= :datecreationFin or 1=1)";
                filtres.Add(str);
            }
            return str;
        }
        private static string FiltreDateSaisie(ParamRecherClausLibDto paramrecherche, DbParameter[] param, List<string> filtres, string str)
        {
            if (!string.IsNullOrEmpty(paramrecherche.DateSaisieDebut))
            {
                str = "((pbsaa * 10000 + pbsam * 100 + pbsaj) >= :datesaisiedebut)";
                filtres.Add(str);
            }
            else
            {
                str = "((pbsaa * 10000 + pbsam * 100 + pbsaj) >= :datesaisiedebut or 1=1)";
                filtres.Add(str);
            }
            if (!string.IsNullOrEmpty(paramrecherche.DateSaisieFin))
            {
                str = "((pbsaa * 10000 + pbsam * 100 + pbsaj) <= :datesaisieFin)";
                filtres.Add(str);
            }
            else
            {
                str = "((pbsaa * 10000 + pbsam * 100 + pbsaj) <= :datesaisieFin or 1=1)";
                filtres.Add(str);
            }
            return str;
        }
        #endregion
        #region Log ParamCibleRecup
        public static List<LogParamCibleRecupDto> GetLogParamCibleRecup(LogParamCibleRecupDto filtre)
        {
            string sql = string.Empty;
            sql = @"SELECT  W1TYP TYPEOFFREPOLICE,
                    W1IPB NUMOFFREPOLICE,
                    W1ALX NUMALIMENT,
                    W1AVN NUMAVENANT,
                    W1HIN  NUMHISTO,
                    W1RSQ IDRISQUE,
                    W1OBJ IDOBJET,
                    W1OPT OPTIONRECUPERATION,
                    W1CSIT CODESITUATION,
                    W1DSIT DATESITUATION,
                    W1USIT USERSITUATION,
                    W1TSIT COMMENTAIRESITUATION,
                    W1NJOB NUMTRAVAIL
                    FROM KWRENT";

            List<string> filtres = new List<string>();
            string str = string.Empty;

            if (!string.IsNullOrEmpty(filtre.TypeOffrePolice))
            {
                str = string.Format(" W1TYP='{0}' ", filtre.TypeOffrePolice);
                filtres.Add(str);
            }

            if (!string.IsNullOrEmpty(filtre.NumOffrePolice))
            {
                str = string.Format(" W1IPB='{0}' ", filtre.NumOffrePolice.ToIPB());
                filtres.Add(str);
            }

            if (filtre.NumAliment > 0)
            {
                str = string.Format(" W1ALX ='{0}' ", filtre.NumAliment);
                filtres.Add(str);
            }

            if (filtre.NumAvenant > 0)
            {
                str = string.Format(" W1AVN='{0}' ", filtre.NumAvenant);
                filtres.Add(str);
            }

            if (filtre.NumHisto > 0)
            {
                str = string.Format(" W1HIN ='{0}' ", filtre.NumHisto);
                filtres.Add(str);
            }

            if (filtre.IdRisque > 0)
            {
                str = string.Format(" W1RSQ  ='{0}' ", filtre.IdRisque);
                filtres.Add(str);
            }

            if (filtre.IdObjet > 0)
            {
                str = string.Format(" W1OBJ='{0}' ", filtre.IdObjet);
                filtres.Add(str);
            }


            if (filtres.Count > 0)
            {
                sql += " WHERE " + string.Join(" AND ", filtres.ToArray());
            }

            sql += " ORDER BY W1TYP DESC";

            return DbBase.Settings.ExecuteList<LogParamCibleRecupDto>(CommandType.Text, sql);
        }
        #endregion


        #region  Frais Accessoires
        public static int GetFraisStandard_Kheo(string codeOffre, string version, string type, int anneeEffet, string codeAvn, string user, string acteGestion)
        {
            LockAS400User(codeOffre, type, version, codeAvn, acteGestion, user);

            DbParameter[] param = new DbParameter[7];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODEAVN", 0)
            {
                Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0
            };
            param[4] = new EacParameter("P_ANNEE", 0)
            {
                Value = anneeEffet
            };
            param[5] = new EacParameter("P_MONTANT", 0)
            {
                Value = 0
            };
            param[6] = new EacParameter("P_FRAIS", 0)
            {
                Value = 0,
                Direction = ParameterDirection.InputOutput,
                DbType = DbType.Int32
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_GETFRAISACC", param);

            UnlockAS400User(codeOffre, type, version, user);

            return int.Parse(param[6].Value.ToString());
        }

        public static List<FraisAccessoiresEngDto> GetFraisAccessoires(FraisAccessoiresEngDto filtre, bool likeCategorie)
        {
            string sql = string.Empty;
            sql = @"SELECT
                    BRANCHE ,
                    SBRANCHE ,
                    CATEGORIE,
                    ANNEE,
                    MONTANT,
                    FRAISACCMIN,
                    FRAISACCMAX
                    FROM KPFRAISACC";

            List<string> filtres = new List<string>();
            string str = string.Empty;

            if (!string.IsNullOrEmpty(filtre.Branche))
            {
                str = string.Format(" BRANCHE='{0}' ", filtre.Branche);
                filtres.Add(str);
            }

            if (!string.IsNullOrEmpty(filtre.SousBranche))
            {
                str = string.Format(" SBRANCHE='{0}' ", filtre.SousBranche);
                filtres.Add(str);
            }

            if (!string.IsNullOrEmpty(filtre.Categorie))
            {
                if (likeCategorie)
                {
                    str = string.Format(" UPPER(CATEGORIE) LIKE UPPER('%{0}%') ", filtre.Categorie);
                }
                else
                {
                    str = string.Format("CATEGORIE = '{0}' ", filtre.Categorie);
                }

                filtres.Add(str);
            }

            if (filtre.Annee > 0)
            {
                str = string.Format(" ANNEE='{0}' ", filtre.Annee);
                filtres.Add(str);
            }


            if (filtres.Count > 0)
            {
                sql += " WHERE " + string.Join(" AND ", filtres.ToArray());
            }

            sql += " ORDER BY BRANCHE, SBRANCHE, CATEGORIE, ANNEE ASC";

            return DbBase.Settings.ExecuteList<FraisAccessoiresEngDto>(CommandType.Text, sql);
        }

        public static void UpdateFraisAccessoires(FraisAccessoiresEngDto filtre, FraisAccessoiresEngDto toSave)
        {
            string sql = string.Format(@"UPDATE KPFRAISACC
                                        SET BRANCHE = '{0}' ,
                                        SBRANCHE  = '{1}' ,
                                        CATEGORIE = '{2}' ,
                                        ANNEE = '{3}' ,
                                        MONTANT = '{4}' ,
                                        FRAISACCMIN = '{5}' ,
                                        FRAISACCMAX = '{6}'
                                        WHERE
                                        BRANCHE = '{7}' AND
                                        SBRANCHE  = '{8}' AND
                                        CATEGORIE = '{9}' AND
                                        ANNEE = '{10}' ",
                                       toSave.Branche,
                                       toSave.SousBranche,
                                       toSave.Categorie,
                                       toSave.Annee,
                                       toSave.Montant,
                                       toSave.FRaiSACCMIN,
                                       toSave.FRaiSACCMAX,
                                       filtre.Branche,
                                       filtre.SousBranche,
                                       filtre.Categorie,
                                       filtre.Annee);


            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        public static void InsertFraisAccessoires(FraisAccessoiresEngDto toSave)
        {
            string sql = string.Format(@"INSERT INTO KPFRAISACC(BRANCHE,SBRANCHE,CATEGORIE,ANNEE,MONTANT,FRAISACCMIN,FRAISACCMAX)
                                          VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                                       toSave.Branche,
                                       toSave.SousBranche,
                                       toSave.Categorie,
                                       toSave.Annee,
                                       toSave.Montant,
                                       toSave.FRaiSACCMIN,
                                       toSave.FRaiSACCMAX);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        public static void DeleteFraisAccessoires(FraisAccessoiresEngDto filtre)
        {
            string sql = string.Format(@"DELETE FROM KPFRAISACC
                                        WHERE
                                        BRANCHE = '{0}' AND
                                        SBRANCHE  = '{1}' AND
                                        CATEGORIE = '{2}' AND
                                        ANNEE = '{3}' ",
                                       filtre.Branche,
                                       filtre.SousBranche,
                                       filtre.Categorie,
                                       filtre.Annee);


            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }
        #endregion

        public static string SetInformationSel(string codeContrat, string version, string type, string fonction, int? periodeDeb, int? periodeFin, string exercice,
            string typeAttestation, string couvAttestation, string user)
        {
            DbParameter[] param = new DbParameter[13];
            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_FONCTION", fonction);
            param[4] = new EacParameter("P_PERIODEDEB", 0)
            {
                Value = periodeDeb
            };
            param[5] = new EacParameter("P_PERIODEFIN", 0)
            {
                Value = periodeFin
            };
            param[6] = new EacParameter("P_EXERCICE", 0)
            {
                Value = !string.IsNullOrEmpty(exercice) ? Convert.ToInt32(exercice) : 0
            };
            param[7] = new EacParameter("P_TYPEATTES", typeAttestation);
            param[8] = new EacParameter("P_COUVATTES", couvAttestation);
            param[9] = new EacParameter("P_USER", user);
            param[10] = new EacParameter("P_DATENOW", 0)
            {
                Value = AlbConvert.ConvertDateToInt(DateTime.Now)
            };
            param[11] = new EacParameter("P_ERREUR", DbType.AnsiStringFixedLength)
            {
                Direction = ParameterDirection.InputOutput,
                Size = 50,
                Value = string.Empty
            };
            param[12] = new EacParameter("P_LOTID", DbType.Int64)
            {
                Direction = ParameterDirection.InputOutput,
                Value = 0
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SETINFOATTESTATION", param);

            return param[11].Value.ToString() + "_" + param[12].Value.ToString();
        }



        #region Informations Base de données

        public static IList<ColumnInfoDto> GeTableDescription(string env, string tableName)
        {
            var param = new DbParameter[2];

            param[0] = new EacParameter("P_TABLENAME", tableName);
            param[1] = new EacParameter("P_ENV", env);


            return DbBase.Settings.ExecuteList<ColumnInfoDto>(CommandType.StoredProcedure, "GETTABLEDESC", param);
        }

        #endregion

        #region Gestion des documents


        public static string SaveDocMagnetic(string codeAffaire, string version, string type, string codeAvn,
            string codeRsq, string codeObj, string codeFor, string codeOpt,
            string typeDoc, string service, string acteGestion, string ajout, string contexte,
            string idClause, string titleClause, string emplacement, string sousemplacement, string ordre,
            string fullFilePath, string etape)
        {
            if (string.IsNullOrEmpty(idClause) || idClause == "0")
            {
                idClause = ClauseRepository.SaveClauseLibre(codeAffaire, version, type, codeAvn, contexte, etape, codeRsq, codeObj, codeFor, codeOpt,
                        emplacement, sousemplacement, ordre);
                //Appel de la proc stock pour créer la ligne dans KPCLAUSE
                DbParameter[] paramClau = new DbParameter[10];
                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVECLAUSELIBRE", paramClau);
                idClause = paramClau[10].Value.ToString();
            }


            //Récupération du n° de document (KPDOC/KEQID)
            long idDoc = 0;

            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("idClause", 0)
            {
                Value = Convert.ToInt32(idClause)
            };

            string sql = @"SELECT KCATXL ID FROM KPCLAUSE WHERE KCAID = :idClause";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                idDoc = result.FirstOrDefault().Id;
            }

            if (string.IsNullOrEmpty(acteGestion))
            {
                switch (type)
                {
                    case "O":
                        acteGestion = AlbConstantesMetiers.TRAITEMENT_OFFRE;
                        break;
                    case "P":
                        acteGestion = AlbConstantesMetiers.TRAITEMENT_AFFNV;
                        break;
                }
            }

            if (idDoc == 0)
            {
                idDoc = GetAS400Id("KEQID");
                //Création du document dans KPDOC
                DbParameter[] paramIns = new DbParameter[12];
                paramIns[0] = new EacParameter("idDoc", 0)
                {
                    Value = idDoc
                };
                paramIns[1] = new EacParameter("codeAffaire", codeAffaire.PadLeft(9, ' '));
                paramIns[2] = new EacParameter("version", 0)
                {
                    Value = Convert.ToInt32(version)
                };
                paramIns[3] = new EacParameter("type", type);
                paramIns[4] = new EacParameter("service", service);
                paramIns[5] = new EacParameter("acteGestion", acteGestion);
                paramIns[6] = new EacParameter("codeAvn", 0)
                {
                    Value = Convert.ToInt32(codeAvn)
                };
                paramIns[7] = new EacParameter("etape", etape);
                paramIns[8] = new EacParameter("typeDoc", typeDoc);
                paramIns[9] = new EacParameter("ajout", ajout);
                paramIns[10] = new EacParameter("fileName", titleClause);
                paramIns[11] = new EacParameter("filePath", fullFilePath);

                string sqlIns = @"INSERT INTO KPDOC
                                        (KEQID, KEQIPB, KEQALX, KEQTYP, KEQSERV, KEQACTG, KEQAVN, KEQETAP, KEQTDOC, KEQAJT, KEQNOM, KEQCHM)
                                    VALUES
                                        (:idDoc, :codeAffaire, :version, :type, :service, :acteGestion, :codeAvn, :etape, :typeDoc, :ajout, :fileName, :filePath)";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlIns, paramIns);

            }

            //Mise à jour de KPCLAUSE
            DbParameter[] paramClause = new DbParameter[5];
            paramClause[0] = new EacParameter("idDoc", 0)
            {
                Value = idDoc
            };
            paramClause[1] = new EacParameter("situation", "V");
            paramClause[2] = new EacParameter("dateMaj", 0)
            {
                Value = AlbConvert.ConvertDateToInt(DateTime.Now)
            };
            paramClause[3] = new EacParameter("textModif", "O");
            paramClause[4] = new EacParameter("idClause", 0)
            {
                Value = Convert.ToInt32(idClause)
            };

            string sqlClause = @"UPDATE KPCLAUSE
                                    SET KCATXL = :idDoc, KCASIT = :situation, KCAMAJD = :dateMaj, KCAXTLM = :textModif
                                WHERE KCAID = :idClause";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlClause, paramClause);

            return idDoc.ToString();
        }

        #endregion

        public static int SetTraceLog(string codeAffaire, string version, string type, int id, string statut, string methode, string date, string diff)
        {
            if (id == 0)
            {
                id = GetAS400Id("SessionKheoDec");
            }

            DbParameter[] param = new DbParameter[8];
            param[0] = new EacParameter("IDSESSION", 0)
            {
                Value = id
            };
            param[1] = new EacParameter("TYP", type);
            param[2] = new EacParameter("IPB", codeAffaire.PadLeft(9, ' '));
            param[3] = new EacParameter("ALX", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[4] = new EacParameter("STATUT", statut);
            param[5] = new EacParameter("METHODE", methode);
            param[6] = new EacParameter("DATEHEURE", date);
            param[7] = new EacParameter("INFO", diff);

            string sql = @"INSERT INTO KEDILOG
                                (IDSESSION, TYP, IPB, ALX, STATUT, METHODE, DATEHEURE, INFO)
                            VALUES
                                (:IDSESSION, :TYP, :IPB, :ALX, :STATUT, :METHODE, :DATEHEURE, :INFO)";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return id;
        }

        /// <summary>
        /// Chargement des infos du bandeau (REFACTO)
        /// </summary>
        public static BandeauDto LoadInfoBandeau(string codeAffaire, string version, string type, string codeAvn, string modeNavig)
        {
            var param = new DbParameter[4];
            param[0] = new EacParameter("codeAffaire", codeAffaire.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0)
            {
                Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
            };
            param[2] = new EacParameter("type", type);
            param[3] = new EacParameter("codeAvn", 0)
            {
                Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0
            };

            var sql = (
@"SELECT TRIM(PBIPB) CODEOFFRE, PBALX VERSIONOFFRE, PBTYP TYPEOFFRE, PBAVN NUMAVENANT, PBAVK NUMEXTERNE,
    PBEFA DATEEFFETA, PBEFM DATEEFFETM, PBEFJ DATEEFFETJ,
    PBFEA FINEFFETANNEE, PBFEM FINEFFETMOIS, PBFEJ FINEFFETJOUR,
    PBAVA DATEEFFETAVNANNEE, PBAVM DATEEFFETAVNMOIS, PBAVJ DATEEFFETAVNJOUR,
    PBCTD DUREE, PBCTU DUREEUNITE,
    PBREF DESCRIPTIF,
    PBDEV CODEDEVISE, DEVISE.TPLIB LIBDEVISE,
    PBBRA BRANCHECODE, BRANCHE.TPLIB BRANCHELIB,
    KAACIBLE CIBLECODE, KAHDESC CIBLELIB,
    PBOFF CODEOFFREORIGINE, PBOFV VERSIONOFFREORIGINE,
    PBICT CODECOURTIERGESTIONNAIRE, COURTGEST.TNNOM NOMCOURTIERGEST, RIGHT(REPEAT('0', 5) || (COURTGESTADR.TCDEP CONCAT COURTGESTADR.TCCPO), 5) DPGESTIONNAIRE, COURTGESTADR.TCVIL VILLEGESTIONNAIRE,
    PBCTA CODECOURTIERAPPORTEUR, COURTAPP.TNNOM NOMCOURTIERAPPO, RIGHT(REPEAT('0', 5) || (COURTAPPADR.TCDEP CONCAT COURTAPPADR.TCCPO), 5) DPAPPORTEUR, COURTAPPADR.TCVIL VILLEAPPORTEUR,
    PBCTP CODECOURTIERPAYEUR, COURTPAY.TNNOM NOMCOURTIERPAYEUR, RIGHT(REPEAT('0', 5) || (COURTPAYADR.TCDEP CONCAT COURTPAYADR.TCCPO), 5) DPPAYEUR, COURTPAYADR.TCVIL VILLEPAYEUR,
    PBIAS CODEPRENASSUR,
    PBGES GESCODE, PBSOU SOUSCODE,
    PBNPL NATURECONTRAT, NATURE.TPLIB LIBELLENATURECONTRAT,
    JDDPV PREAVIS
FROM YPOBASE
    LEFT JOIN YPRTENT ON PBIPB = JDIPB AND PBALX = JDALX
    LEFT JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP
    LEFT JOIN YYYYPAR DEVISE ON DEVISE.TCON = 'GENER' AND DEVISE.TFAM = 'DEVIS' AND DEVISE.TCOD = PBDEV
    LEFT JOIN YYYYPAR BRANCHE ON BRANCHE.TCON = 'GENER' AND BRANCHE.TFAM = 'BRCHE' AND BRANCHE.TCOD = PBBRA
    LEFT JOIN YYYYPAR NATURE ON NATURE.TCON = 'PRODU' AND NATURE.TFAM = 'PBNPL' AND NATURE.TCOD = PBNPL
    LEFT JOIN KCIBLE ON KAHCIBLE = KAACIBLE
    LEFT JOIN YCOURTN COURTGEST ON PBICT = COURTGEST.TNICT AND COURTGEST.TNINL = 0
    LEFT JOIN YCOURTI COURTGESTADR ON COURTGEST.TNICT = COURTGESTADR.TCICT
    LEFT JOIN YCOURTN COURTAPP ON PBCTA = COURTAPP.TNICT AND COURTAPP.TNINL = 0
    LEFT JOIN YCOURTI COURTAPPADR ON COURTAPP.TNICT = COURTAPPADR.TCICT
    LEFT JOIN YCOURTN COURTPAY ON PBCTP = COURTPAY.TNICT AND COURTPAY.TNINL = 0
    LEFT JOIN YCOURTI COURTPAYADR ON COURTPAY.TNICT = COURTPAYADR.TCICT
    WHERE PBIPB = :codeAffaire AND PBALX = :version  AND PBTYP = :type ");
            //|| ' mois' preavis
            //(pbefa * 10000 + pbefm * 100 + pbefj) datedebeffet, pbefh heuredebeffet,
            //(pbfea * 10000 + pbfem * 100 + pbfej) datefineffet, pbfeh heurefineffet,
            //(pbava * 10000 + pbavm * 100 + pbavj) dateeffetavn,
            var result = DbBase.Settings.ExecuteList<BandeauDto>(CommandType.Text, sql, param).FirstOrDefault();

            return result;
            //return new BandeauDto();
        }

        public static string GetNumAvn(string codeOffre, string version, string type)
        {
            string sql = @"SELECT PBAVN FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";
            var parameters = MakeParams(sql, codeOffre.ToIPB(), version, type);

            return DbBase.Settings.ExecuteScalar(CommandType.Text, sql, parameters).ToString();
        }

        /// <summary>
        /// Supprime les traces OPT et FOR depuis KPCTRLE si et seulement si la date d'effet pour une contrat ou la date de saisie pour une offe  a changé
        /// </summary>
        /// <param name="codeAffaire"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        public static void DeleteTraceFormule(string codeAffaire, int version, string type, int year, int month, int day, int hour)
        {
            if (type == "P")
            {
                var (sql, param) = MakeParamsSql(@"SELECT IFNULL(PBEFA * 100000000 + PBEFM * 1000000 + PBEFJ * 10000 + PBEFH, 0) DATEDEBRETURNCOL
                                          FROM YPOBASE
                                          WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :typ", codeAffaire.PadLeft(9, ' '), version, type);

                var old = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault();
                if (old != null)
                {
                    var oldDateEffet = AlbConvert.ConvertIntToDateHour(old.DateDebReturnCol);

                    var newDateEffet = new DateTime(year, month, day);
                    var heureEffet = AlbConvert.ConvertIntToTimeMinute(hour);
                    if (heureEffet.HasValue)
                    {
                        newDateEffet = newDateEffet.Add(heureEffet.Value);
                    };

                    if (oldDateEffet != newDateEffet)
                    {
                        sql = string.Format(@"DELETE FROM KPCTRLE
	                                      WHERE KEVIPB = '{0}' AND KEVALX = {1} AND KEVTYP = '{2}' AND KEVETAPE IN ('OPT','FOR')", codeAffaire.PadLeft(9, ' '), version, type);

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                    }
                }
            }
            else if (type == "O")
            {

                var sql = string.Format(@"SELECT IFNULL(PBSAA * 100000000 + PBSAM * 1000000 + PBSAJ * 10000 + PBSAH, 0) DATEDEBRETURNCOL
                                     FROM YPOBASE
                                     WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'", codeAffaire.PadLeft(9, ' '), version, type);

                var old = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql).FirstOrDefault();
                if (old != null)
                {
                    var oldDateEffet = AlbConvert.ConvertIntToDateHour(old.DateDebReturnCol);

                    var newDateEffet = new DateTime(year, month, day);
                    var heureEffet = AlbConvert.ConvertIntToTimeMinute(hour);
                    if (heureEffet.HasValue)
                    {
                        newDateEffet = newDateEffet.Add(heureEffet.Value);
                    };

                    if (oldDateEffet != newDateEffet)
                    {
                        sql = string.Format(@"DELETE FROM KPCTRLE
	                                  WHERE KEVIPB = '{0}' AND KEVALX = {1} AND KEVTYP = '{2}' AND KEVETAPE IN ('OPT','FOR')", codeAffaire.PadLeft(9, ' '), version, type);

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                    }
                }


            }

        }

        /// <summary>
        /// Désactive un ou des liens de l'arborescence de navigation
        /// </summary>
        /// <param name="contrat">Contrat mis à jour</param>
        /// <param name="etapes">Les liens à désactiver</param>
        /// <param name="isAvenant">true si avenant sinon false</param>
        public static void DisableLinkNavigation(ContratDto contrat, List<String> etapes, bool isAvenant)
        {
            bool delLink = false;
            // Récupérer l'ancienne valeur de la périodicité/ échéance principale et la date début d'effet de l'avenant
            var (sql, param) = MakeParamsSql(@"SELECT PBPER PERIODICITE, IFNULL(PBEFA * 10000 + PBEFM * 100 + PBEFJ  , 0) DATEDEBRETURNCOL,  IFNULL(PBAVA * 10000 + PBAVM * 100 + PBAVJ  , 0) DATEDEBEFFRETURNCOL, PBECM INT32RETURNCOL, PBECJ INT32RETURNCOL2
                                          FROM YPOBASE
                                          WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type", contrat.CodeContrat.PadLeft(9, ' '), contrat.VersionContrat, contrat.Type);
            var old = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault();
            if (old != null)
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                // Comparaison des valeurs
                if ((isAvenant && DateTime.ParseExact(old.DateDebEffReturnCol.ToString(), "yyyymmdd", provider).Date != contrat.DateEffetAvenant) ||
                       old.Periodicite != contrat.PeriodiciteCode || (old.Int32ReturnCol != contrat.Mois || old.Int32ReturnCol2 != contrat.Jour))
                {
                    delLink = true;
                }
                if (delLink)
                {
                    sql = string.Format(@"DELETE FROM KPCTRLE
	                                  WHERE KEVIPB = '{0}' AND KEVALX = {1} AND KEVTYP = '{2}' AND KEVETAPE IN ('{3}')", contrat.CodeContrat.PadLeft(9, ' '), contrat.VersionContrat, contrat.Type, string.Join("','", etapes.ToArray()));
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                }

            }
        }

        #region Log Perf

        public static List<LogPerf> GetLogPerfs(DateTime? startDate, DateTime? endDate)
        {
            var filter = "";

            if (startDate.HasValue && !endDate.HasValue)
            {
                filter = string.Format(@" AND DATEHEURE >= '{0}'", startDate.Value.ToString("MM/dd/yyyy 00:00:00", CultureInfo.InvariantCulture));
            }
            else if (!startDate.HasValue && endDate.HasValue)
            {
                filter = string.Format(@" AND DATEHEURE <= '{0}'", endDate.Value.Date.ToString("MM/dd/yyyy 23:59:59", CultureInfo.InvariantCulture));
            }
            else
            {
                filter = string.Format(@" AND DATEHEURE BETWEEN '{0}' AND '{1}'", startDate.Value.Date.ToString("MM/dd/yyyy 00:00:00", CultureInfo.InvariantCulture), endDate.Value.Date.ToString("MM/dd/yyyy 23:59:59", CultureInfo.InvariantCulture));
            }

            string sql = string.Format(@"SELECT DATEHEURE  AS DateLog
	                                            ,METHODE AS  User
	                                            ,SUBSTR(INFO, 1,LOCATE('_',INFO)-1) as Screen
	                                            ,CASE SUBSTR(SUBSTR(INFO,LOCATE('_',INFO)+1), 1,LOCATE('_',SUBSTR(INFO,LOCATE('_',INFO)+1))-1)
                                                  WHEN 'Index' THEN 'Chargement'
                                                  ELSE SUBSTR(SUBSTR(INFO,LOCATE('_',INFO)+1), 1,LOCATE('_',SUBSTR(INFO,LOCATE('_',INFO)+1))-1)
                                                  END as Action
	                                            , SUBSTR(INFO, LOCATE('_',INFO, LOCATE('_',INFO)+1)+1) AS ElapsedTime
                                        FROM KEDILOG
                                        WHERE STATUT = 'KHEPER' AND SUBSTR(INFO, 1,LOCATE('_',INFO)-1) <> 'OffresVerrouillees' {0}
                                        ORDER BY DATEHEURE DESC", filter);

            var result = DbBase.Settings.ExecuteList<LogPerf>(CommandType.Text, sql);

            return result;
        }

        #endregion

        public static void CleanClauseRegule(string codeAffaire, string version, string type, IDbConnection connection = null)
        {
            var param = new List<EacParameter>() {
                new EacParameter("codeAffaire", DbType.AnsiStringFixedLength){ Value = codeAffaire.PadLeft(9, ' ') },
                new EacParameter("version", DbType.Int32){ Value = version},
                new EacParameter("type", DbType.AnsiStringFixedLength){Value = type },
                new EacParameter("etape", DbType.AnsiStringFixedLength){Value = "REGUL"}
            };

            string sql = "DELETE FROM KPCLAUSE WHERE KCAIPB = :codeAffaire AND KCAALX = :version AND KCATYP = :type AND KCAETAPE = :etape";

            if (connection == null)
            {
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            else
            {
                var dbOptions = new DbExecOptions()
                {
                    CommandType = CommandType.Text,
                    SqlText = sql,
                    DbConnection = connection,
                    Parameters = param
                };

                dbOptions.Exec();
            }
        }
        /// <summary>
        /// Obtenir partenaires : les courtiers / le preneur d'assurance
        /// </summary>
        /// <param name="codeOffre"> code</param>
        /// <param name="version">version</param>
        /// <param name="type">type</param>
        /// <param name="codeAvn">code AVN</param>
        /// <returns></returns>
        public static PartenairesBaseDto GetListPartenairesInfosBase(string code, string version, string type, string codeAvn)
        {
            var partenaires = new PartenairesBaseDto();
            var param = new List<EacParameter>()
            {
               new EacParameter("code", DbType.AnsiStringFixedLength){ Value = code.ToIPB()},
               new EacParameter("version", DbType.Int32){ Value = Convert.ToInt32(version)},
               new EacParameter("type", DbType.AnsiStringFixedLength) { Value =type },
               new EacParameter("avn", DbType.Int32) { Value =!string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0 }
           };

            var sql = string.Format(@" SELECT
            BASE.PBICT  CODECOURTIERGESTIONNAIRE,
            GEST.TNNOM NOMCOURTIERGESTIONNAIRE,
            BASE.PBIN5  CODEINTERLOCUTEUR,
            INL.TNNOM NOMINTERLOCUTEUR,
            BASE.PBCTA  CODECOURTIERAPPORTEUR,
            APPO.TNNOM NOMCOURTIERAPPORTEUR,
            BASE.PBCTP  CODECOURTIERPAYEUR,
            PAY.TNNOM NOMCOURTIERPAYEUR,
            BASE.PBIAS  CODEPRENEURASSURANCE,
            ASSU.ANNOM NOMPRENEURASSURANCE
            FROM YPOBASE BASE
            LEFT JOIN YCOURTN GEST ON GEST.TNICT = BASE.PBICT AND GEST.TNINL  = 0 AND GEST.TNTNM = 'A'
            LEFT JOIN YCOURTN INL  ON INL.TNICT = BASE.PBICT AND INL.TNINL  = BASE.PBIN5 AND INL.TNINL  <> 0 AND INL.TNTNM = 'A'
            LEFT JOIN YCOURTN APPO ON APPO.TNICT = BASE.PBCTA AND APPO.TNINL  = 0 AND APPO.TNTNM = 'A'
            LEFT JOIN YCOURTN PAY  ON PAY.TNICT = BASE.PBCTP AND PAY.TNINL  = 0 AND PAY.TNTNM = 'A'
            LEFT JOIN YASSNOM ASSU ON ASSU.ANIAS = BASE.PBIAS AND ASSU.ANINL = 0 AND ASSU.ANTNM = 'A'
            LEFT JOIN KPENT ENT  ON ENT.KAAIPB = BASE.PBIPB AND ENT.KAAALX = BASE.PBALX AND ENT.KAATYP = BASE.PBTYP
            WHERE BASE.PBIPB = :code AND BASE.PBALX = :version  AND BASE.PBTYP = :type {0}",
            type == AlbConstantesMetiers.TYPE_CONTRAT ? " AND BASE.PBAVN = :avn" : string.Empty
            );

            var result = DbBase.Settings.ExecuteList<PartenairesBaseDetailsDto>(CommandType.Text, sql, param)?.FirstOrDefault();
            if (result != null)
            {
                partenaires.CourtierGestionnaire = new PartenaireDto
                {
                    Code = result.CodeCourtierGestionnaire,
                    Nom = result.NomCourtierGestionnaire,
                    CodeInterlocuteur = result.CodeInterlocuteur ?? 0,
                    NomInterlocuteur = result.NomInterlocuteur

                };
                partenaires.CourtierApporteur = new PartenaireDto
                {
                    Code = result.CodeCourtierApporteur,
                    Nom = result.NomCourtierApporteur
                };
                partenaires.CourtierPayeur = new PartenaireDto
                {
                    Code = result.CodeCourtierPayeur,
                    Nom = result.NomCourtierPayeur
                };
                partenaires.PreneurAssurance = new PartenaireDto
                {
                    Code = result.CodePreneurAssurance,
                    Nom = result.NomPreneurAssurance
                };
            }
            return partenaires;

        }


        /// <summary>
        /// Obtenir informations de base des assurés addditonnels
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="version">Version</param>
        /// <param name="type">Type</param>
        /// <param name="codeAvn">AVN</param>
        /// <param name="modeNavig">mode Navig</param>
        /// <returns></returns>
        public static List<PartenaireDto> GetListAssuresAdditionnelsInfosBase(string code, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var result = new List<PartenaireDto>();
            try
            {
                var assures = CommonAffNouvRepository.GetListAssuresRef(code, version, type, codeAvn, modeNavig);
                if (assures?.Count > 0)
                {
                    result = assures.Where(x => x.CodeAssure != x.AssureBase).Select(x => new PartenaireDto { Code = x.CodeAssure.ToString(), Nom = x.NomAssure }).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
