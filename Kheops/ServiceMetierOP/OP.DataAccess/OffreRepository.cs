using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.DataAccess.Data;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.OffresRapide;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using static DataAccess.Helpers.OutilsHelper;

namespace OP.DataAccess
{
    public class OffreRepository : RepositoryBase
    {
        internal static readonly string SelectByIPB = "SELECT PBIPB IPB, PBTYP TYP, PBALX ALX, PBETA ETA, PBTTR TTR, PBSIT SIT, PBBRA BRA FROM YPOBASE WHERE PBTYP = 'O' AND PBIPB = :IPB";

        public OffreRepository(IDbConnection dbConnection) : base(dbConnection) { }

        public IEnumerable<FolderBasicData> GetAllByIPB(string ipb)
        {
            return Fetch<FolderBasicData>(SelectByIPB, ipb.ToIPB());
        }

        #region Méthodes Publiques

        /// <summary>
        /// Récupère la branche et la cible d'une offre
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetBrancheCibleOffre(string codeOffre, string version, string type)
        {
            var param = new DbParameter[3];
            param[0] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", type);

            string sql = @"SELECT PBBRA BRANCHE, KAACIBLE CIBLE, KAHDESC CIBLEDESC 
                    FROM YPOBASE 
                        INNER JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP
                        INNER JOIN KCIBLE ON KAACIBLE = KAHCIBLE
                    WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault();

            return string.Format("{0}_{1}_{2}", result.Branche, result.Cible, result.CibleDesc);
        }



        /// <summary>
        /// Copie une offre dans une autre offre
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="copyCodeOffre"></param>
        /// <param name="copyVersion"></param>
        /// <param name="dateSysteme"></param>
        /// <param name="user"></param>
        public static void CopyOffreFromOffre(string codeOffre, string version, string type, string copyCodeOffre, string copyVersion, DateTime dateSysteme, string user, string traitement, string acteGestion)
        {
            string strDate = AlbConvert.ConvertDateToInt(dateSysteme).ToString();

            string modeCopie = "OFFRE";
            if (copyCodeOffre.Contains("CV"))
                modeCopie = "CNVA";

            DbParameter[] param = new DbParameter[9];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_COPYCODEOFFRE", copyCodeOffre.PadLeft(9, ' '));
            param[3] = new EacParameter("P_COPYVERSION", 0);
            param[3].Value = Convert.ToInt32(copyVersion);
            param[4] = new EacParameter("P_TYPE", type);
            param[5] = new EacParameter("P_DATESYSTEME", strDate);
            param[6] = new EacParameter("P_USER", user);
            param[7] = new EacParameter("P_TRAITEMENT", traitement);
            param[8] = new EacParameter("P_MODECOPY", modeCopie);


            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_OFCOPIE", param);

            if (modeCopie == "CNVA")
            {
                CommonRepository.ReloadEngagement(codeOffre, version, type, copyCodeOffre, copyVersion, type, user, acteGestion);
            }


            //Copy des documents 
            CopieDocRepository.CopierDocuments(codeOffre, version, type, "0");
        }

        public static void RefusOffre(string codeOffre, string version, string codeMotif)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("codeMotif", codeMotif);
            param[1] = new EacParameter("codeoffre", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("version", 0);
            param[2].Value = Convert.ToInt32(version);

            string sql = @"UPDATE YPOBASE SET PBETA = 'V', PBSIT = 'W', PBSTF = :CODEMOTIF
                        WHERE PBIPB = :codeOffre AND PBALX = :version";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static string ValiderOffre(string codeOffre, string version, string type, string avenant, string acteGestion, string validable, string complet, string motif, string mode, string lotsId, string user, bool isModifHorsAvn)
        {
            int numavn = avenant.ParseInt().Value;
            int numalx = version.ParseInt().Value;
            DateTime dateNow = DateTime.Now;
            var returnParam = new EacParameter("P_ERREUR", string.Empty) { Direction = ParameterDirection.InputOutput, Size = 128 };
            var parameters = new[] {
                new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' ')),
                new EacParameter("P_VERSION", DbType.Int32) { Value = numalx },
                new EacParameter("P_TYPE", type),
                new EacParameter("P_AVENANT", DbType.Int32) { Value = numavn },
                new EacParameter("P_MODE", mode.ToUpper()),
                new EacParameter("P_ETAT", mode == "editer" ? (validable == "Oui" ? "A" : "N") : mode == "valider" ? "V" : string.Empty),
                new EacParameter("P_MOTIF", motif ?? string.Empty),
                new EacParameter("P_YEARNOW", DbType.Int32) { Value = dateNow.Year },
                new EacParameter("P_MONTHNOW", DbType.Int32) { Value = dateNow.Month },
                new EacParameter("P_DAYNOW", DbType.Int32) { Value = dateNow.Day },
                new EacParameter("P_HOURNOW", DbType.Int32) { Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(dateNow)) },
                new EacParameter("P_USER", user),
                returnParam
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_VALIDATIONAFFAIRE", parameters);
            string result = returnParam.Value.ToString();

            if (result.IsEmptyOrNull() && mode.ToUpper() == "VALIDER")
            {
                var traitement = type == AlbConstantesMetiers.TYPE_OFFRE ? AlbConstantesMetiers.TRAITEMENT_OFFRE : numavn > 0 ? AlbConstantesMetiers.TRAITEMENT_AVNMD : AlbConstantesMetiers.TRAITEMENT_AFFNV;
                if (!isModifHorsAvn)
                {
                    CommonRepository.AjouterActeGestion(codeOffre, version, type, !string.IsNullOrEmpty(avenant) ? Convert.ToInt32(avenant) : 0, AlbConstantesMetiers.ACTEGESTION_VALIDATION, traitement, "", user);
                }
                if (numavn > 0)
                {
                    var rmvRepo = new ProgramAS400Repository(null);
                    var folder = new PGMFolder() { CodeOffre = codeOffre, Version = numalx, Type = type, NumeroAvenant = numavn, User = user, ActeGestion = acteGestion };
                    rmvRepo.ValidationAvenant(folder);
                    if (!isModifHorsAvn)
                    {
                        rmvRepo.SetStop(folder);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Supprimer Offre
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [Obsolete]
        public static void SupprimerOffre(string codeOffre, string version, string type)
        {
            DateTime now = DateTime.Now;
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = int.Parse(version);
            param[2] = new EacParameter("P_TYPE", type);


            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELOFFRE", param);


        }

        /// <summary>
        /// Copie des informations de l'offre simplifiée
        /// vers les tables
        /// </summary>
        public static string ConvertSimpleFolderToStd(string codeOffre, string version, string type, string branche, string cible, string user)
        {
            DateTime dateNow = DateTime.Now;
            var date = AlbConvert.ConvertDateToInt(dateNow);
            var hour = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));

            DbParameter[] param = new DbParameter[10];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_BRANCHE", branche);
            param[4] = new EacParameter("P_CIBLE", cible);
            param[5] = new EacParameter("P_USER", user);
            param[6] = new EacParameter("P_DATENOW", date.ToString());
            param[7] = new EacParameter("P_HOURNOW", hour);
            param[8] = new EacParameter("P_TYPO", FormuleRepository.GetTypologieModele(codeOffre, Convert.ToInt32(version), type, Convert.ToInt32(CibleRepository.GetIdCible(cible)), branche, date));
            param[9] = new EacParameter("P_OUTMESSAGE", "");
            param[9].Value = string.Empty;
            param[9].Size = 100;
            param[9].Direction = ParameterDirection.InputOutput;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CONVSIMPLEFOLDERTOSTD", param);

            return param[9].Value.ToString();
        }

        public static string DeleteOffre(string codeOffre, string version, string type)
        {
            string toReturn = string.Empty;

            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPOBASE WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'", codeOffre.PadLeft(9, ' '), version, type);
            var resultCount = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (resultCount != null && resultCount.Any())
            {
                if (resultCount.FirstOrDefault().NbLigne <= 0)
                    toReturn = "n'existe pas.";
            }

            if (string.IsNullOrEmpty(toReturn))
            {
                sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPOBASE WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}' AND PBETA = 'V'", codeOffre.PadLeft(9, ' '), version, type);
                var resultEtat = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
                if (resultEtat != null && resultEtat.Any())
                {
                    if (resultEtat.FirstOrDefault().NbLigne > 0)
                        toReturn = "est validé(e), impossible d'effectuer la suppression.";
                }
            }

            if (string.IsNullOrEmpty(toReturn))
            {
                DbParameter[] param = new DbParameter[3];
                param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
                param[1] = new EacParameter("P_VERSION", 0);
                param[1].Value = int.Parse(version);
                param[2] = new EacParameter("P_TYPE", type);

                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEOFFRE", param);
            }

            return toReturn;
        }

        #endregion

        #region Méthodes privées

        public static void DeletePripeFiles(string codeOffre, string version, string type, string acteGestion)
        {
            /* 2015-12-16 : suite au bug 1770 => kheops - sf13 - validation.V3.2.docx */
            /* s'il existe un enregistrement dans kpavtrc sur arguments IPB/ALX/TYP et KHOOEF = 'NG' ; on supprime les enregistrements dans YPRIPES et satellites */
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", type);

            string sql = string.Empty;

            switch (acteGestion)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    sql = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC WHERE KHOIPB = :codeOffre AND KHOALX = :version AND KHOTYP = :type AND KHOOEF = 'NR'";
                    DeletePrimesW(codeOffre, version, acteGestion, sql, param);
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                    sql = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC WHERE KHOIPB = :codeOffre AND KHOALX = :version AND KHOTYP = :type AND KHOOEF = 'NR'";
                    DeletePrimesW(codeOffre, version, acteGestion, sql, param);
                    sql = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC WHERE KHOIPB = :codeOffre AND KHOALX = :version AND KHOTYP = :type AND KHOOEF = 'NG'";
                    DeletePrimesW(codeOffre, version, string.Empty, sql, param);
                    break;
                default:
                    sql = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC WHERE KHOIPB = :codeOffre AND KHOALX = :version AND KHOTYP = :type AND KHOOEF = 'NG'";
                    DeletePrimesW(codeOffre, version, acteGestion, sql, param);
                    break;
            }



            //if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
            //{
            //    sql = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC WHERE KHOIPB = :codeOffre AND KHOALX = :version AND KHOTYP = :type AND KHOOEF = 'NR'";
            //}
            //else
            //{
            //    sql = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC WHERE KHOIPB = :codeOffre AND KHOALX = :version AND KHOTYP = :type AND KHOOEF = 'NG'";
            //}
            //if (CommonRepository.ExistRowParam(sql, param))
            //{
            //    DbParameter[] paramDel = new DbParameter[2];
            //    paramDel[0] = new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' '));
            //    paramDel[1] = new EacParameter("version", 0);
            //    paramDel[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            //    string sqlDelete = string.Format(@"DELETE FROM {0} WHERE PKIPB = :codeOffre AND PKALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPES"));
            //    DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
            //    sqlDelete = string.Format(@"DELETE FROM {0} WHERE PLIPB = :codeOffre AND PLALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPGA"));
            //    DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
            //    sqlDelete = string.Format(@"DELETE FROM {0} WHERE PNIPB = :codeOffre AND PNALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPCM"));
            //    DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
            //    sqlDelete = string.Format(@"DELETE FROM {0} WHERE KVIPB = :codeOffre AND KVALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPGK"));
            //    DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
            //    sqlDelete = string.Format(@"DELETE FROM {0} WHERE POIPB = :codeOffre AND POALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPPA"));
            //    DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
            //    sqlDelete = string.Format(@"DELETE FROM {0} WHERE PTIPB = :codeOffre AND PTALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTA"));
            //    DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
            //    sqlDelete = string.Format(@"DELETE FROM {0} WHERE PUIPB = :codeOffre AND PUALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTG"));
            //    DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
            //    sqlDelete = string.Format(@"DELETE FROM {0} WHERE PMIPB = :codeOffre AND PMALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTX"));
            //    DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
            //}
        }

        private static void DeletePrimesW(string codeOffre, string version, string acteGestion, string sql, DbParameter[] param)
        {
            if (CommonRepository.ExistRowParam(sql, param))
            {
                DbParameter[] paramDel = new DbParameter[2];
                paramDel[0] = new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' '));
                paramDel[1] = new EacParameter("version", 0);
                paramDel[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                string sqlDelete = string.Format(@"DELETE FROM {0} WHERE PKIPB = :codeOffre AND PKALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPES"));
                DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
                sqlDelete = string.Format(@"DELETE FROM {0} WHERE PLIPB = :codeOffre AND PLALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPGA"));
                DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
                sqlDelete = string.Format(@"DELETE FROM {0} WHERE PNIPB = :codeOffre AND PNALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPCM"));
                DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
                sqlDelete = string.Format(@"DELETE FROM {0} WHERE KVIPB = :codeOffre AND KVALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPGK"));
                DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
                sqlDelete = string.Format(@"DELETE FROM {0} WHERE POIPB = :codeOffre AND POALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPPA"));
                DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
                sqlDelete = string.Format(@"DELETE FROM {0} WHERE PTIPB = :codeOffre AND PTALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTA"));
                DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
                sqlDelete = string.Format(@"DELETE FROM {0} WHERE PUIPB = :codeOffre AND PUALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTG"));
                DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
                sqlDelete = string.Format(@"DELETE FROM {0} WHERE PMIPB = :codeOffre AND PMALX = :version WITH NC", CommonRepository.GetSuffixeRegule(acteGestion, "YPRIPTX"));
                DbBase.Settings.ExecuteNonQueryWithoutTransaction(CommandType.Text, sqlDelete, paramDel);
            }
        }

        #endregion

        #region ParamCibleRecup
        /// <summary>
        /// retourner l'offre/contrat avant la recuperation
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static OffreRecupDto GetOffreRecup(string codeOffre, string version)
        {
            string sql = string.Empty;
            sql = string.Format(@"SELECT PBTYP TYPE FROM YPOBASE WHERE PBORK NOT IN ('KHE','KVS') AND PBIPB = '{0}' AND PBALX = {1}"
                    , codeOffre.PadLeft(9, ' '), version);

            return DbBase.Settings.ExecuteList<OffreRecupDto>(CommandType.Text, sql).FirstOrDefault();
        }
        /// <summary>
        /// retourner l'offre/contrat avant la recuperation  PBORK = 'KHE'
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static OffreRecupDto GetOffreAfterRecup(string codeOffre, string version)
        {
            var returnOffre = new OffreRecupDto();


            var (sql, param) = MakeParamsSql(@"SELECT KAACIBLE CIBLE, KAICIBLE CIBLELABEL, PBTYP TYPE, PBTBR BRANCHE
	                                FROM YPOBASE
                                        LEFT JOIN KPENT ON KAAIPB = PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP
		                                LEFT JOIN KCIBLEF ON KAACIBLE = KAICIBLE
                                    WHERE PBORK = 'REP' AND PBIPB = :ipb AND PBALX = :alx"
                    , codeOffre.ToIPB(), version);

            var result = DbBase.Settings.ExecuteList<OffreRecupDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                returnOffre = result.FirstOrDefault();
                returnOffre.MultiObj = CommonRepository.RowNumber(string.Format(@"SELECT COUNT(*) NBLIGN FROM YPRTRSQ WHERE JEIPB = '{0}' AND JEALX = {1}"
                                            , codeOffre.ToIPB(), version)) > 1;
            }

            return returnOffre;
        }

        /// <summary>
        /// recuperer offre/contrat PBORK='KHE'
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        public static void UpdateOffreRecup(string codeOffre, string version)
        {
            string sql = string.Format(@"UPDATE  YPOBASE Y 
                                        SET PBORK='KHE'
                                        WHERE  PBORK NOT IN ('KHE','KVS')
                                        AND PBIPB =  '{0}'
                                        AND PBALX ={1}",
                                                      codeOffre.PadLeft(9, ' '),
                                                      version);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

        }

        /// <summary>
        /// lancer la migration vers cible par la proc SP_MIGRATIONCIBLE
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="fromCible"></param>
        /// <param name="toCible"></param>
        public static void UpdateCible(string codeOffre, string version, string type, string fromCible, string toCible)
        {
            DbParameter[] param = new DbParameter[5];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_TYPE", type);
            param[2] = new EacParameter("P_VERSION", 0);
            param[2].Value = Convert.ToInt32(version);
            param[3] = new EacParameter("P_CODECIBLE", fromCible);
            param[4] = new EacParameter("P_CODENEWCIBLE", toCible);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_MIGRATIONCIBLE", param);
        }

        #endregion

        #region Offre Rapide

        public static OffreRapideResultDto GetOffresRapideByFiltre(OffreRapideFiltreDto filtre)
        {
            var param = new DbParameter[17];

            #region Paramètres


            param[0] = new EacParameter("P_CODEOFFRE", !string.IsNullOrEmpty(filtre.CodeOffre) ? filtre.CodeOffre.Replace("'", "''") : string.Empty);
            param[1] = new EacParameter("P_VERSION", filtre.Version ?? 0);
            param[2] = new EacParameter("P_TYPEOFFRE", !string.IsNullOrEmpty(filtre.TypeOffre) ? filtre.TypeOffre.Replace("'", "''") : string.Empty);
            param[3] = new EacParameter("P_CODEAVENANT", filtre.CodeAvenant ?? 0);
            param[4] = new EacParameter("P_TYPETRAITEMENT", !string.IsNullOrEmpty(filtre.TypeTraitement) ? filtre.TypeTraitement.Replace("'", "''") : string.Empty);

            var dateEffetAvnDeb = AlbConvert.ConvertDateToInt(filtre.DateEffetAvnDebut);
            var dateEffetAvnFin = AlbConvert.ConvertDateToInt(filtre.DateEffetAvnFin);

            param[5] = new EacParameter("P_DATEEFFETAVNDEBUT", dateEffetAvnDeb ?? 0);
            param[6] = new EacParameter("P_DATEEFFETAVNFIN", dateEffetAvnFin ?? 0);
            param[7] = new EacParameter("P_CODEPERIODICITE", !string.IsNullOrEmpty(filtre.CodePeriodicite) ? filtre.CodePeriodicite.Replace("'", "''") : string.Empty);
            param[8] = new EacParameter("P_CODEBRANCHE", !string.IsNullOrEmpty(filtre.CodeBranche) ? filtre.CodeBranche.Replace("'", "''") : string.Empty);
            param[9] = new EacParameter("P_CODECIBLE", !string.IsNullOrEmpty(filtre.CodeCible) ? filtre.CodeCible.Replace("'", "''") : string.Empty);
            param[10] = new EacParameter("P_USERCREA", !string.IsNullOrEmpty(filtre.UserCrea) ? filtre.UserCrea.Replace("'", "''") : string.Empty);
            param[11] = new EacParameter("P_USERMAJ", !string.IsNullOrEmpty(filtre.UserMaj) ? filtre.UserMaj.Replace("'", "''") : string.Empty);
            param[12] = new EacParameter("P_SORTINGBY", !string.IsNullOrEmpty(filtre.SortingBy) ? filtre.SortingBy.Replace("'", "''") : string.Empty);
            param[13] = new EacParameter("P_LINECOUNT", filtre.LineCount);
            param[14] = new EacParameter("P_STARTLINE", filtre.StartLine);
            param[15] = new EacParameter("P_ENDLINE", filtre.EndLine);

            param[16] = new EacParameter("P_COUNT", 0)
            {
                Direction = ParameterDirection.InputOutput,
                DbType = DbType.Int64,
                Value = 0
            };

            #endregion

            var list = DbBase.Settings.ExecuteList<OffreRapideInfoDto>(CommandType.StoredProcedure, "SP_RECHERCHERAPIDE", param);
            list.ForEach(o =>
            {
                o.DateDeSaisieDate = AlbConvert.ConvertIntToDateHour(o.DateDeSaisie);
            });



            var nbCount = Convert.ToInt32(param[16].Value.ToString());
            return new OffreRapideResultDto
            {
                NbCount = nbCount,
                Offres = list
            };
        }

        #endregion
    }
}
