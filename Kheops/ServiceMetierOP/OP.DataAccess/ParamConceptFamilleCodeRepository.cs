using OP.WSAS400.DTO.Offres.Parametres;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.ParametreFamilles;
using System.Data.Common;
using System.Data.EasycomClient;
using OP.WSAS400.DTO.ParametreFiltre;
using OP.WSAS400.DTO.ParametreTypeValeur;
using OP.WSAS400.DTO.Common;

namespace OP.DataAccess
{
    public class ParamConceptFamilleCodeRepository
    {

        #region Concepts

        public static List<ParametreDto> LoadListConcepts(string codeConcept, string descriptionConcept, bool modeAutocomplete, bool isAdmin = true)
        {
            string sql = @"SELECT 
	                           TCON CODE,
	                           TCLIB LIBELLE
                           FROM YYYYPCO";
            string whereAndOr = " WHERE";

            if (!string.IsNullOrEmpty(codeConcept))
            {
                sql += whereAndOr;
                sql += string.Format(" (TRIM(LOWER(TCON)) LIKE '%{0}%' {1})", codeConcept.Trim().ToLower(), !isAdmin ? " AND TRIM(LOWER(TCON)) = 'kheop'" : string.Empty);
                whereAndOr = (modeAutocomplete ? " OR" : " AND");
            }
            if (!string.IsNullOrEmpty(descriptionConcept))
            {
                sql += whereAndOr;
                sql += string.Format(" TRIM(LOWER(TCLIB)) LIKE '%{0}%'", descriptionConcept.Trim().ToLower());
                whereAndOr = (modeAutocomplete ? " OR" : " AND");
            }

            sql += " ORDER BY TCON";

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        public static List<ParametreDto> EnregistrerConcept(string mode, ParametreDto concept)
        {
            string sql = string.Empty;
            if (mode == "Insert")
            {
                sql = string.Format(@"INSERT INTO YYYYPCO (TCON, TCLIB) VALUES ('{0}', '{1}')", concept.Code, concept.Libelle);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                return LoadListConcepts(string.Empty, string.Empty, false);//renvoie toute la liste des concepts
            }
            else if (mode == "Update")
            {
                sql = string.Format(@"UPDATE YYYYPCO SET TCLIB = '{0}' WHERE TCON = '{1}'", concept.Libelle, concept.Code);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                return LoadListConcepts(concept.Code, string.Empty, false);//renvoie la ligne concept mise à jour
            }
            return LoadListConcepts(string.Empty, string.Empty, false);//renvoie toute la liste des concepts
        }

        public static string SupprimerConcept(ParametreDto concept)
        {
            DbParameter[] param = new DbParameter[5];
            param[0] = new EacParameter("P_ORIGINE", "CONCEPT");
            param[1] = new EacParameter("P_CONCEPT", concept.Code);
            param[2] = new EacParameter("P_FAMILLE", string.Empty);
            param[3] = new EacParameter("P_CODE", string.Empty);
            param[4] = new EacParameter("P_ERREUR", "");
            param[4].Direction = ParameterDirection.InputOutput;
            param[4].Size = 256;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELPARAMCFC", param);
            return param[4].Value.ToString();
        }

        #endregion
        #region Famille
        public static List<ParamFamilleDto> GetFamilles(string codeConcept, string codeFamille, string descriptionFamille, string additionalParam, string restriction)
        {
            string sql = @"SELECT TCON CODECONCPET,TFAM CODEFAMILLE,TFLIB LIBELLEFAMILLE
                                         FROM YYYYPFA";

            if (!string.IsNullOrEmpty(codeConcept) || !string.IsNullOrEmpty(codeFamille) || !string.IsNullOrEmpty(descriptionFamille) || !string.IsNullOrEmpty(restriction))
                sql += " WHERE";
            if (!string.IsNullOrEmpty(codeConcept))
                sql += " TRIM(LOWER(TCON)) ='{0}' ";
            if (!string.IsNullOrEmpty(codeConcept) && (!string.IsNullOrEmpty(codeFamille) || !string.IsNullOrEmpty(descriptionFamille) || !string.IsNullOrEmpty(restriction)))
                sql += "AND";
            if (!string.IsNullOrEmpty(codeFamille))
                sql += " TRIM(LOWER(TFAM)) LIKE '%{1}%' ";

            if (!string.IsNullOrEmpty(descriptionFamille) && !string.IsNullOrEmpty(codeFamille))
                sql += " AND TRIM(LOWER(TFLIB)) LIKE '%{2}%' ";

            else if (!string.IsNullOrEmpty(descriptionFamille))
                sql += " TRIM(LOWER(TFLIB)) LIKE '%{2}%'";

            if (!string.IsNullOrEmpty(restriction) && !string.IsNullOrEmpty(descriptionFamille))
                sql += " AND TFTYP='{3}'";
            else if (!string.IsNullOrEmpty(restriction))
                sql += " TFTYP='{3}'";


            sql += " ORDER BY TFAM";
            string formatedSql = string.Format(sql, codeConcept.ToLower().Trim(), codeFamille.ToLower().Trim(), descriptionFamille.ToLower().Trim().Replace("'", "''"), restriction);

            return DbBase.Settings.ExecuteList<ParamFamilleDto>(CommandType.Text, formatedSql);
        }
        public static ParamFamilleDto
            GetFamille(string codeConcept, string codeFamille, string additionalParam, string restriction)
        {
            string sql = string.Format(@"SELECT TCON CODECONCPET,
                                         TFAM CODEFAMILLE,
                                         TFLIB LIBELLEFAMILLE,
                                         TFLGR LONGEUR,
                                         TFTAN TYPECODE,
                                         TFBLC NULLVALUE,
                                         TFLN1 LIBELLECOURTNUM1,TFGN1 LIBELLELONGNUM1,TFTZ1 TYPENUM1,TFND1 NBRDECIMAL1,
                                         TFLN2 LIBELLECOURTNUM2,TFGN2 LIBELLELONGNUM2,TFTZ2 TYPENUM2,TFND2 NBRDECIMAL2,
                                         TFLA1 LIBELLECOURTALPHA1,TFGA1 LIBELLELONGALPHA1,
	                                     TFLA2 LIBELLECOURTALPHA2,TFGA2 LIBELLELONGALPHA2,
	                                     TFTYP RESTRICTION              
                                         FROM YYYYPFA
                                         WHERE TCON='{0}'AND TFAM='{1}'", codeConcept, codeFamille);
            return DbBase.Settings.ExecuteList<ParamFamilleDto>(CommandType.Text, sql).FirstOrDefault();
        }
        public static List<ParametreDto> GetFamillesByConcept(string codeConcept, string additionalParam, string restriction)
        {
            string sql = string.Empty;
            if (!string.IsNullOrEmpty(codeConcept))
                sql = string.Format(@"SELECT TFAM CODE,TFLIB LIBELLE
                                         FROM YYYYPFA WHERE TCON='{0}' ORDER BY TFAM", codeConcept);
            else sql = @"SELECT TFAM CODE,TFLIB LIBELLE
                                         FROM YYYYPFA ORDER BY TFAM";
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }
        public static List<ParametreDto> FamillesGet(string value, string mode, string additionalParam, string restriction)
        {
            var toReturn = new List<ParametreDto>();
            string whereQuery = string.Empty;
            string sql = string.Empty;

            if (mode == "Name" && !string.IsNullOrEmpty(value))
                whereQuery = string.Format(@"LOWER(TRIM(TFLIB)) like '%{0}%' FETCH FIRST 10 ROWS ONLY", value.Trim().ToLower());
            if (mode == "Code" && !string.IsNullOrEmpty(value))
                whereQuery = string.Format(@"LOWER(TRIM(TFAM)) like '%{0}%' FETCH FIRST 10 ROWS ONLY", value.Trim().ToLower());

            sql = string.Format(@"SELECT 
	                                    TFAM CODE,
	                                    TFLIB LIBELLE 	                                   
                                    FROM 
	                                    YYYYPFA
                                    WHERE {0}", whereQuery);

            if (!string.IsNullOrEmpty(sql))
            {
                return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
            }
            return null;
        }
        public static string EnregistrerFamille(string mode, ParamFamilleDto famille, string additionalParam)
        {
            DbParameter[] param = new DbParameter[21];
            param[0] = new EacParameter("P_CONCEPT", famille.CodeConcpet);
            param[1] = new EacParameter("P_FAMILLE", famille.CodeFamille);
            param[2] = new EacParameter("P_LIB_FAMILLE", famille.LibelleFamille);
            param[3] = new EacParameter("P_LONGUEUR", famille.Longueur);
            param[4] = new EacParameter("P_TYPE_FAMILLE", famille.TypeCode);
            param[5] = new EacParameter("P_BLANC", famille.NullValue);
            param[6] = new EacParameter("P_LIB_COURT_1", famille.LibelleCourtNum1);
            param[7] = new EacParameter("P_LIB_LONG_1", famille.LibelleLongNum1);
            param[8] = new EacParameter("P_TYPE_1", famille.TypeNum1);
            param[9] = new EacParameter("P_NB_DEC_1", famille.NbrDecimal1);
            param[10] = new EacParameter("P_LIB_COURT_2", famille.LibelleCourtNum2);
            param[11] = new EacParameter("P_LIB_LONG_2", famille.LibelleLongNum2);
            param[12] = new EacParameter("P_TYPE_2", famille.TypeNum2);
            param[13] = new EacParameter("P_NB_DEC_2", famille.NbrDecimal2);
            param[14] = new EacParameter("P_LIB_COURT_ALPHA_1", famille.LibelleCourtAlpha1);
            param[15] = new EacParameter("P_LIB_LONG_ALPHA_1", famille.LibelleLongAlpha1);
            param[16] = new EacParameter("P_LIB_COURT_ALPHA_2", famille.LibelleCourtAlpha2);
            param[17] = new EacParameter("P_LIB_LONG_ALPHA_2", famille.LibelleLongAlpha2);
            param[18] = new EacParameter("P_TYPE_RESTR", famille.Restriction);
            param[19] = new EacParameter("P_MODE", mode);
            param[20] = new EacParameter("P_ERREUR", "");
            param[20].Direction = ParameterDirection.InputOutput;
            param[20].Size = 256;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDFAMILLE", param);
            return param[20].Value.ToString();
        }
        public static string SupprimerFamille(ParamFamilleDto famille)
        {
            DbParameter[] param = new DbParameter[5];
            param[0] = new EacParameter("P_ORIGINE", "FAMILLE");
            param[1] = new EacParameter("P_CONCEPT", famille.CodeConcpet);
            param[2] = new EacParameter("P_FAMILLE", famille.CodeFamille);
            param[3] = new EacParameter("P_CODE", string.Empty);
            param[4] = new EacParameter("P_ERREUR", "");
            param[4].Direction = ParameterDirection.InputOutput;
            param[4].Size = 256;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELPARAMCFC", param);
            return param[4].Value.ToString();
        }
        public static List<ParamValeurDto> GetValeursByFamille(string codeConcept, string codeFamille, string additionalParam, string restriction)
        {
            string sql = @"SELECT TCON CODECONCPET,
                                         TFAM CODEFAMILLE,TCOD CODEVALEUR, TPLIB LIBELLEVALEUR, TFILT CODEFILTRE,KGGDESC LIBELLEFILTRE  
                                      FROM YYYYPAR
	                                  LEFT JOIN KFILTRE  ON KGGFILT =TFILT 
	                                  WHERE TCON='{0}' AND TFAM='{1}'";

            if (!string.IsNullOrEmpty(restriction))
                sql += " AND TPTYP='{2}'";
            sql += " ORDER BY TCOD";
            string formatedSql = string.Format(sql, codeConcept, codeFamille, restriction);

            return DbBase.Settings.ExecuteList<ParamValeurDto>(CommandType.Text, formatedSql);
        }
        public static ParamValeurDto GetValeur(string codeConcept, string codeFamille, string codeValeur, string additionalParam, string restriction)
        {
            string sql = string.Format(@"SELECT TPLIB LIBELLEVALEUR, 
                                        TPLIL LIBELLELONGVALEUR, DESC1.TXTXT DESCRIPTION1, DESC2.TXTXT DESCRIPTION2,DESC3.TXTXT DESCRIPTION3,
                                        TPCN1 VALEURNUM1, TPCN2 VALEURNUM2, TPCA1 VALEURALPHA1, TPCA2 VALEURALPHA2,
                                        TPTYP RESTRICTION,TFILT CODEFILTRE,KGGDESC LIBELLEFILTRE
                                        FROM YYYYPAR PAR
                                        LEFT JOIN KFILTRE  ON KGGFILT =TFILT 
                                        LEFT JOIN YYYYPAL DESC1 ON DESC1.TCON=PAR.TCON AND DESC1.TFAM=PAR.TFAM AND DESC1.TCOD=PAR.TCOD AND DESC1.TXORD = 1
                                        LEFT JOIN YYYYPAL DESC2 ON DESC2.TCON=PAR.TCON AND DESC2.TFAM=PAR.TFAM AND DESC2.TCOD=PAR.TCOD AND DESC2.TXORD = 2
                                        LEFT JOIN YYYYPAL DESC3 ON DESC3.TCON=PAR.TCON AND DESC3.TFAM=PAR.TFAM AND DESC3.TCOD=PAR.TCOD AND DESC3.TXORD = 3 
	                                    WHERE PAR.TCON='{0}' AND PAR.TFAM='{1}' AND PAR.TCOD='{2}'", codeConcept, codeFamille, codeValeur);
            return DbBase.Settings.ExecuteList<ParamValeurDto>(CommandType.Text, sql).FirstOrDefault();
        }
        public static string EnregistrerValeur(string mode, ParamValeurDto valeur, string additionalParam)
        {
            DbParameter[] param = new DbParameter[16];
            param[0] = new EacParameter("P_CONCEPT", valeur.CodeConcpet);
            param[1] = new EacParameter("P_FAMILLE", valeur.CodeFamille);
            param[2] = new EacParameter("P_VALEUR", valeur.CodeValeur);
            param[3] = new EacParameter("P_LIB_COURT", valeur.LibelleValeur);
            param[4] = new EacParameter("P_LIB_LONG", valeur.LibelleLongValeur);
            param[5] = new EacParameter("P_DESC1", valeur.Description1);
            param[6] = new EacParameter("P_DESC2", valeur.Description2);
            param[7] = new EacParameter("P_DESC3", valeur.Description3);
            param[8] = new EacParameter("P_VALNUM1", 0);
            param[8].Value = valeur.ValeurNum1;
            param[9] = new EacParameter("P_VALNUM2", 0);
            param[9].Value = valeur.ValeurNum2;
            param[10] = new EacParameter("P_ALPHA1", valeur.ValeurAlpha1);
            param[11] = new EacParameter("P_ALPHA2", valeur.ValeurAlpha2);
            param[12] = new EacParameter("P_FILTRE", valeur.CodeFiltre);
            param[13] = new EacParameter("P_RESTRICTION", valeur.Restriction);
            param[14] = new EacParameter("P_MODE", mode);
            param[15] = new EacParameter("P_ERREUR", "");
            param[15].Direction = ParameterDirection.InputOutput;
            param[15].Size = 256;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDVALEUR", param);
            return param[15].Value.ToString();
        }
        public static string SupprimerValeur(ParamValeurDto valeur)
        {
            DbParameter[] param = new DbParameter[5];
            param[0] = new EacParameter("P_ORIGINE", "CODE");
            param[1] = new EacParameter("P_CONCEPT", valeur.CodeConcpet);
            param[2] = new EacParameter("P_FAMILLE", valeur.CodeFamille);
            param[3] = new EacParameter("P_CODE", valeur.CodeValeur);
            param[4] = new EacParameter("P_ERREUR", "");
            param[4].Direction = ParameterDirection.InputOutput;
            param[4].Size = 256;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELPARAMCFC", param);
            return param[4].Value.ToString();
        }

        /// <summary>
        /// Vérifie la validité de l'association du concept/famille
        /// </summary>
        public static string CheckConceptFamille(string codeConcept, string codeFamille)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM YYYYPFA WHERE TCON = '{0}' AND TFAM = '{1}'", codeConcept, codeFamille);
            var resultCount = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (resultCount != null && resultCount.Any() && resultCount.FirstOrDefault().NbLigne > 0)
            {
                sql = string.Format(@"SELECT TCLIB STRRETURNCOL FROM YYYYPCO WHERE TCON = '{0}'", codeConcept);
                var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
                if (result != null && result.Any())
                    return result.FirstOrDefault().StrReturnCol;
            }
            return string.Empty;
        }

        #endregion
        #region Filtres
        public static List<ModeleFiltreLigneDto> LoadListFiltres(string codeFiltre, string descriptionFiltre, string idFiltre)
        {
            string sql = @"SELECT 
	                           KGGID ID,
	                           KGGFILT CODE,
                               KGGDESC LIBELLE
                           FROM KFILTRE";
            string whereAnd = " WHERE";

            if (!string.IsNullOrEmpty(codeFiltre))
            {
                sql += whereAnd;
                sql += string.Format(" TRIM(LOWER(KGGFILT)) LIKE '%{0}%'", codeFiltre.Trim().ToLower());
                whereAnd = " AND";
            }
            if (!string.IsNullOrEmpty(descriptionFiltre))
            {
                sql += whereAnd;
                sql += string.Format(" TRIM(LOWER(KGGDESC)) LIKE '%{0}%'", descriptionFiltre.Trim().ToLower());
                whereAnd = " AND";
            }
            if (!string.IsNullOrEmpty(idFiltre))
            {
                sql += whereAnd;
                sql += string.Format(" KGGID = {0}", idFiltre);
                whereAnd = " AND";
            }
            sql += " ORDER BY KGGFILT";

            return DbBase.Settings.ExecuteList<ModeleFiltreLigneDto>(CommandType.Text, sql);
        }

        public static ModeleDetailsFiltreDto GetFiltreDetails(Int64 idFiltre)
        {
            ModeleDetailsFiltreDto toReturn = null;
            string sql1 = string.Format(@"SELECT KGGID ID,
                                                 KGGFILT CODEFILTRE,
                                                 KGGDESC LIBELLEFILTRE,
                                                 KGGCRD DATECREATION
                                          FROM KFILTRE
                                          WHERE KGGID = {0}", idFiltre);
            var result = DbBase.Settings.ExecuteList<ModeleDetailsFiltreDto>(CommandType.Text, sql1);
            if (result.Any())
            {
                toReturn = result.FirstOrDefault();

                var resultPaire = GetListeBrancheCible(idFiltre, null, string.Empty, string.Empty);
                if (resultPaire.Any())
                {
                    toReturn.ListePairesBrancheCible = resultPaire;

                }
            }
            return toReturn;
        }

        public static List<ModeleLigneBrancheCibleDto> GetListeBrancheCible(Int64 idFiltre, Int64? idPaire, string branche, string cible)
        {
            string sql = string.Format(@"SELECT KGHID IDPAIRE,
                                  KGHACTF ACTION,
                                  KGHBRA BRANCHE,
                                  KGHCIBLE CIBLE
                           FROM KFILTRL 
                           WHERE KGHKGGID = {0}", idFiltre);

            string whereAnd = " AND";
            if (idPaire != null)
            {
                sql += whereAnd;
                sql += string.Format(" KGHID = {0}", idPaire.Value);
            }
            if (!string.IsNullOrEmpty(branche))
            {
                sql += whereAnd;
                sql += string.Format(" KGHBRA = '{0}'", branche);
            }
            if (!string.IsNullOrEmpty(cible))
            {
                sql += whereAnd;
                sql += string.Format(" KGHCIBLE = '{0}'", cible);
            }

            return DbBase.Settings.ExecuteList<ModeleLigneBrancheCibleDto>(CommandType.Text, sql);
        }

        public static List<ParametreDto> GetBranchesFiltre()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "BRCHE");
        }

        public static List<ParametreDto> GetCiblesFiltre(string codeBranche)
        {
            List<ParametreDto> toReturn = new List<ParametreDto>();
            if (!string.IsNullOrEmpty(codeBranche))
            {
                DbParameter[] param = new DbParameter[1];
                string sql = @"SELECT KAHCIBLE CODE, KAHDESC LIBELLE
                            FROM KCIBLE 
                                LEFT JOIN KCIBLEF ON KAHCIBLE = KAICIBLE 
                            WHERE KAIBRA = :KAIBRA
                            ORDER BY KAHCIBLE";
                param[0] = new EacParameter("KAIBRA", codeBranche);

                toReturn = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);
            }
            return toReturn;
        }

        public static List<ModeleFiltreLigneDto> EnregistrerFiltre(string mode, ModeleFiltreLigneDto filtre, string user)
        {
            string sql = string.Empty;
            if (mode == "Insert")
            {
                int id = CommonRepository.GetAS400Id("KGGID");
                sql = string.Format(@"INSERT INTO KFILTRE 
                                    (KGGID, KGGFILT, KGGDESC, KGGCRU, KGGCRD, KGGCRH, KGGMAJU, KGGMAJD,KGGMAJH)
                                    VALUES ({0}, '{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                      id, filtre.Code, filtre.Libelle, user, DateTime.Now.ToString("yyyyMMdd"),
                                      DateTime.Now.ToString("HHmm"), user, DateTime.Now.ToString("yyyyMMdd"),
                                      DateTime.Now.ToString("HHmm"));
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                return LoadListFiltres(string.Empty, string.Empty, string.Empty);//renvoie toute la liste des filtres
            }
            else if (mode == "Update")
            {
                sql = string.Format(@"UPDATE KFILTRE SET 
                                      KGGFILT = '{0}',
                                      KGGDESC = '{1}',
                                      KGGMAJU = '{2}',
                                      KGGMAJD = '{3}',
                                      KGGMAJH = '{4}'
                                      WHERE KGGID = {5}",
                                      filtre.Code, filtre.Libelle,
                                      user, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmm"),
                                      filtre.Id);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                return LoadListFiltres(filtre.Code, string.Empty, string.Empty);//renvoie la ligne filtre mise à jour
            }
            return LoadListFiltres(string.Empty, string.Empty, string.Empty);//renvoie toute la liste des filtres
        }
        public static List<ModeleLigneBrancheCibleDto> EnregistrerPaireBrancheCible(string mode, Int64 idFiltre, ModeleLigneBrancheCibleDto paire, string user)
        {
            string sql = string.Empty;
            if (mode == "Insert")
            {
                int id = CommonRepository.GetAS400Id("KGHID");
                sql = string.Format(@"INSERT INTO KFILTRL
                                      (KGHID, KGHKGGID, KGHFILT, KGHORDR, KGHACTF, KGHBRA, KGHCIBLE)
                                      VALUES ({0}, {1}, '{2}', {3}, '{4}', '{5}', '{6}')",
                                      id, idFiltre,
                                      LoadListFiltres(string.Empty, string.Empty, idFiltre.ToString()).FirstOrDefault().Code,
                                      0, paire.Action, paire.Branche, paire.Cible);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }
            else if (mode == "Update")
            {
                sql = string.Format(@"UPDATE KFILTRL 
                                      SET KGHACTF = '{0}',
                                          KGHBRA = '{1}',
                                          KGHCIBLE = '{2}'
                                      WHERE KGHID = {3}", paire.Action, paire.Branche, paire.Cible, paire.GuidIdPaire);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                return GetListeBrancheCible(idFiltre, paire.GuidIdPaire, string.Empty, string.Empty);
            }
            return GetListeBrancheCible(idFiltre, null, string.Empty, string.Empty);
        }

        public static string SupprimerFiltre(ModeleFiltreLigneDto filtre)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P_ORIGINE", "Filtre");
            param[1] = new EacParameter("P_ID", filtre.Id);
            param[2] = new EacParameter("P_ERREUR", "");
            param[2].Direction = ParameterDirection.InputOutput;
            param[2].Size = 256;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELPARAMFILTRE", param);
            return param[2].Value.ToString();
        }

        public static string SupprimerPaireBrancheCible(ModeleLigneBrancheCibleDto paire)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P_ORIGINE", "Paire");
            param[1] = new EacParameter("P_ID", paire.GuidIdPaire);
            param[2] = new EacParameter("P_ERREUR", "");
            param[2].Direction = ParameterDirection.InputOutput;
            param[2].Size = 256;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELPARAMFILTRE", param);
            return param[2].Value.ToString();
        }

        #endregion
        #region Type Valeur (code)

        public static List<ModeleLigneTypeValeurDto> LoadListTypesValeur(string codeTypeValeur, string descriptionTypeValeur)
        {
            string sql = @"SELECT 	                           
	                           KGLTYVAL CODE,
                               KGLDESC LIBELLE
                           FROM KTYPVAL";
            string whereAnd = " WHERE";

            if (!string.IsNullOrEmpty(codeTypeValeur))
            {
                sql += whereAnd;
                sql += string.Format(" TRIM(LOWER(KGLTYVAL)) LIKE '%{0}%'", codeTypeValeur.Trim().ToLower());
                whereAnd = " AND";
            }
            if (!string.IsNullOrEmpty(descriptionTypeValeur))
            {
                sql += whereAnd;
                sql += string.Format(" TRIM(LOWER(KGLDESC)) LIKE '%{0}%'", descriptionTypeValeur.Trim().ToLower());
                whereAnd = " AND";
            }
            sql += " ORDER BY KGLTYVAL";

            return DbBase.Settings.ExecuteList<ModeleLigneTypeValeurDto>(CommandType.Text, sql);
        }

        public static List<ModeleLigneTypeValeurDto> EnregistrerTypeValeur(string mode, ModeleLigneTypeValeurDto typeValeur, string user)
        {
            string sql = string.Empty;
            if (mode == "Insert")
            {
                sql = string.Format(@"INSERT INTO KTYPVAL
                                    (KGLTYVAL, KGLDESC)
                                    VALUES ('{0}', '{1}')",
                                     typeValeur.Code, typeValeur.Description);
            }
            else if (mode == "Update")
            {
                sql = string.Format(@"UPDATE KTYPVAL SET
                                      KGLDESC = '{0}'
                                      WHERE KGLTYVAL = '{1}'",
                                     typeValeur.Description, typeValeur.Code);
            }
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            return LoadListTypesValeur(string.Empty, string.Empty);//renvoie toute la liste des filtres
        }

        public static void EnregistrerTypeValeurComp(string mode, string typeValeurId, string typeValeurCompId, string typeValeurCompCode, string user)
        {
            string sql = string.Empty;
            if (mode == "Insert")
            {
                int id = CommonRepository.GetAS400Id("KGMID");
                sql = string.Format(@"INSERT INTO KTYPVALD
                                    (KGMID, KGMTYVAL, KGMBASE)
                                    VALUES ({0}, '{1}', '{2}')",
                                    id, typeValeurId, typeValeurCompCode);
            }
            else if (mode == "Update")
            {
                sql = string.Format(@"UPDATE KTYPVALD SET 
                                      KGMBASE = '{0}'                                    
                                      WHERE KGMID = {1}",
                                      typeValeurCompCode, typeValeurCompId);
            }
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        public static void SupprimerTypeValeur(string mode, string code)
        {
            DbParameter[] param = new DbParameter[2];
            param[0] = new EacParameter("P_ID", code);
            param[1] = new EacParameter("P_MODE", mode);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELTYPEVALEUR", param);
        }

        public static ModeleDetailsTypeValeurDto GetTypeValeurDetails(string codeTypeValeur)
        {
            ModeleDetailsTypeValeurDto toReturn = null;
            string sql1 = string.Format(@"SELECT KGLTYVAL CODETYPEVALEUR,
                                                 KGLDESC LIBELLETYPEVALEUR                                                
                                          FROM KTYPVAL
                                          WHERE KGLTYVAL = '{0}'", codeTypeValeur);
            var result = DbBase.Settings.ExecuteList<ModeleDetailsTypeValeurDto>(CommandType.Text, sql1);
            if (result.Any())
            {
                toReturn = result.FirstOrDefault();

                var resultComp = GetListeTypeValeurComp(codeTypeValeur, string.Empty, string.Empty);
                if (resultComp.Any())
                {
                    toReturn.ListeTypesValeurCompatible = resultComp;
                }
            }
            return toReturn;
        }

        public static bool IsValueFree(string codeValeur)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM KTYPVALD WHERE KGMBASE = '{0}'", codeValeur);
            return DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql).FirstOrDefault().NbLigne > 0;
        }

        public static List<ModeleLigneTypeValeurCompatibleDto> GetListeTypeValeurComp(string codeTypeValeur, string codeTypeValeurComp, string idTypeValeurComp)
        {
            string sql = string.Format(@"SELECT KGMID GUIDID, 
                                                KGMBASE CODETYPEVALEURCOMP, 
                                                TPLIB LIBELLETYPEVALEURCOMP
	                                     FROM KTYPVALD 
	                                     INNER JOIN YYYYPAR ON KGMBASE = TCOD 
                                                            AND TCON = 'PRODU' 
                                                            AND TFAM = 'QCVAT'
                                         WHERE KGMTYVAL = '{0}'", codeTypeValeur);

            string whereAnd = " AND";
            if (!string.IsNullOrEmpty(idTypeValeurComp))
            {
                sql += whereAnd;
                sql += string.Format(" KGMID = {0}", idTypeValeurComp);
            }
            if (!string.IsNullOrEmpty(codeTypeValeurComp))
            {
                sql += whereAnd;
                sql += string.Format(" KGMBASE = '{0}'", codeTypeValeurComp);
            }

            sql += " ORDER BY KGMBASE";

            return DbBase.Settings.ExecuteList<ModeleLigneTypeValeurCompatibleDto>(CommandType.Text, sql);
        }

        public static List<ModeleLigneTypeValeurCompatibleDto> GetListeReferentielTypeValeurComp(string typeValeur)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(typeValeur))
            {
                where = string.Format(" WHERE KGMTYVAL = '{0}'", typeValeur);
            }
            string sql = string.Format(@"SELECT TCOD CODETYPEVALEURCOMP, 
                                                TPLIL LIBELLETYPEVALEURCOMP
	                                     FROM YYYYPAR
	                                     WHERE TCOD NOT IN 
                                                    ( SELECT KGMBASE 
                                                       FROM KTYPVALD 
                                                       INNER JOIN KTYPVAL  ON KGMTYVAL=  KGLTYVAL 
                                                       {0}
                                                    )
	                                           AND TCON = 'PRODU' 
                                               AND TFAM = 'QCVAT'", where);
            return DbBase.Settings.ExecuteList<ModeleLigneTypeValeurCompatibleDto>(CommandType.Text, sql);
        }

        #endregion
    }
}
