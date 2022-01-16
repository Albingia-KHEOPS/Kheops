using System;
using System.Collections.Generic;
using System.Linq;
using OP.WSAS400.DTO.ParametreCibles;
using System.Data.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Data;
using System.Data;
using OP.WSAS400.DTO.Offres.Parametres;
using ALBINGIA.Framework.Common.Tools;

namespace OP.DataAccess
{
    public class ParamCiblesRepository
    {
        public static List<ParamCiblesDto> LoadListCible(string codeCiblePattern, string libelleCiblePattern)
        {

            string sql = @"SELECT Cible.KAHID GUIDID, Cible.KAHCIBLE CODE, Cible.KAHDESC LIBELLE, Cible.KAHCRD DATECREA, Cible.KAHNMG CODEGRILLE, KHJDESI LIBGRILLE , KAHCON CODECONCEPT, KAHFAM CODEFAMILLE
                                FROM KCIBLE Cible 
                                    LEFT JOIN KNMGRI ON KAHNMG = KHJNMG ";
            if ((codeCiblePattern != string.Empty) || libelleCiblePattern != string.Empty)
                sql += "WHERE";
            if (codeCiblePattern != string.Empty)
                sql += " UPPER(KAHCIBLE) LIKE UPPER('%{0}%') ";
            if ((codeCiblePattern != string.Empty) && libelleCiblePattern != string.Empty)
                sql += "AND";
            if (libelleCiblePattern != string.Empty)
                sql += " UPPER(KAHDESC) LIKE UPPER('%{1}%') ";

            sql += " ORDER BY KAHCIBLE";

            return DbBase.Settings.ExecuteList<ParamCiblesDto>(CommandType.Text, string.Format(sql, codeCiblePattern, libelleCiblePattern));
        }

        public static List<ParamListCibleBranchesDto> GetCibleBranches(Int64 codeCibleId)
        {
            DbParameter[] param = new DbParameter[1];

            string sqlListeBrancheCible = string.Format(@"SELECT 	
                                            CibleRef.KAIID 		GUIDID,
	                                        Branche.TCOD        CODBRCH,
	                                        Branche.TPLIB       LIBBRCH, 
	                                        SousBranche.TCOD    CODSBRCH,
	                                        SousBranche.TPLIB   LIBSBRCH, 
                                            Categorie.CACAT     CODCAT,
	                                        Categorie.CADES     LIBCAT
                                          FROM  KCIBLE Cible
											INNER JOIN KCIBLEF CibleRef ON Cible.KAHID = CibleRef.KAIKAHID
                                            {0}
                                            {1}
                                            LEFT JOIN YCATEGO Categorie ON CibleRef.KAIBRA = Categorie.CABRA
								                                            AND CibleRef.KAISBR = Categorie.CASBR
								                                            AND CibleRef.KAICAT = Categorie.CACAT		
                                          WHERE Cible.KAHID = :CODE		
                                          ORDER BY Branche.TCOD",
                                CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "BRCHE", "Branche", " AND CibleRef.KAIBRA = Branche.TCOD"),
                                CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "BRSBR", "SousBranche", " AND CibleRef.KAIBRA || CibleRef.KAISBR = SousBranche.TCOD"));

            param[0] = new EacParameter("CODE", codeCibleId);

            return DbBase.Settings.ExecuteList<ParamListCibleBranchesDto>(CommandType.Text, sqlListeBrancheCible, param);
        }

        public static List<ParametreDto> GetBranches(int codeCibleId)
        {
            string sql = CommonRepository.BuildSelectYYYYPAR(string.Empty, string.Empty, string.Empty, "TCOD CODE, TPLIB LIBELLE, TPLIB DESCRIPTIF", "GENER", "BRCHE", otherCriteria: " AND TCOD != 'PP' AND TCOD != 'ZZ'");
            sql += string.Format(@" AND TCOD NOT IN (
 											SELECT Branche.TCOD                               
                                          	    FROM  KCIBLEF CibleRef 
                                                    {1}
								                WHERE CibleRef.KAIKAHID = {0}
 								            ) ", codeCibleId,
                            CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "BRCHE", "Branche", " AND CibleRef.KAIBRA = Branche.TCOD"));

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql).OrderBy(m=>m.Code).ToList();
        }
        public static List<ParametreDto> GetBranchesEdit(int codeCibleId, string codeBranche)
        {
            string sql = CommonRepository.BuildSelectYYYYPAR(string.Empty, string.Empty, string.Empty, "TCOD CODE, TPLIB LIBELLE, TPLIB DESCRIPTIF", "GENER", "BRCHE", otherCriteria: " AND TCOD != 'PP' AND TCOD != 'ZZ'");
            sql += string.Format(@" AND TCOD NOT IN (
 											SELECT Branche.TCOD                               
                                          	    FROM  KCIBLEF CibleRef 
                                                    {1}
								                WHERE CibleRef.KAIKAHID = {0}
 								            ) ", codeCibleId,
                            CommonRepository.BuildJoinYYYYPAR("INNER", "GENER", "BRCHE", "Branche", " AND CibleRef.KAIBRA = Branche.TCOD AND Branche.TCOD != '" + codeBranche + "'"));

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql).OrderBy(m => m.Code).ToList();
        }

        public static List<ParametreDto> GetSousBranches(string codeBranche)
        {
            string sql = CommonRepository.BuildSelectYYYYPAR(string.Empty, string.Empty, string.Empty, string.Format("IFNULL(SUBSTR(TCOD, {0}), TCOD) CODE, TPLIB LIBELLE, TPLIB DESCRIPTIF", codeBranche.Length + 1),
                                "GENER", "BRSBR",
                                otherCriteria: string.Format(" AND TCOD LIKE '{0}%'", codeBranche));

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql).OrderBy(m => m.Code).ToList();
        }

        public static List<ParametreDto> GetCategories(string codeBranche, string codeSousBranche)
        {
            string sql = string.Format(@"SELECT CACAT CODE, CADES LIBELLE FROM YCATEGO WHERE CABRA = '{0}' AND CASBR = '{1}'", codeBranche, codeSousBranche);


            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        public static int AjouterBSCCible(string guidIdCible, string codeCible, string codeBranche, string codeSousBranche, string codeCategorie)
        {
            string req1 = string.Format(@"SELECT count(*) NBLIGN FROM KCIBLEF WHERE KAIKAHID='{0}'
            AND KAICIBLE='{1}' AND KAIBRA='{2}' AND  KAISBR='{3}' AND KAICAT='{4}'", guidIdCible, codeCible, codeBranche, codeSousBranche, codeCategorie);
            if (CommonRepository.ExistRow(req1)) return 0;

            string req2 = string.Format(@"SELECT count(*) NBLIGN FROM YCATEGO WHERE CABRA='{0}'
            AND CASBR='{1}' AND CACAT='{2}'", codeBranche, codeSousBranche, codeCategorie);
            if (!CommonRepository.ExistRow(req2)) return 2;

            int codeParam = CommonRepository.GetAS400Id("KAIID");
            if (codeParam > 0)
            {
                var param = new DbParameter[6];
                string sql = @"INSERT INTO KCIBLEF
                        (KAIID, KAICIBLE, KAIKAHID, KAIBRA, KAISBR, KAICAT)
                        VALUES
                        (:KAIID, :KAICIBLE, :KAIKAHID, :KAIBRA, :KAISBR, :KAICAT)";
                param[0] = new EacParameter("KAIID", codeParam);
                param[2] = new EacParameter("KAIKAHID", guidIdCible);
                param[1] = new EacParameter("KAICIBLE", codeCible);
                param[3] = new EacParameter("KAIBRA", codeBranche);
                param[4] = new EacParameter("KAISBR", codeSousBranche);
                param[5] = new EacParameter("KAICAT", codeCategorie);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                return 1;
            }

            return -1;
        }
        public static int AjouterCible(string codeCible, string descriptionCible, string grille, string famille, string concept, string user)
        {
            string req = string.Format("SELECT count(*) NBLIGN FROM KCIBLE WHERE KAHCIBLE='{0}'", codeCible);
            if (CommonRepository.ExistRow(req)) return 0;
            int guid = CommonRepository.GetAS400Id("KAHID");
            if (guid > 0)
            {
                DbParameter[] param = new DbParameter[9];
                string sql = @"INSERT INTO KCIBLE
                        (KAHID, KAHCIBLE, KAHDESC, KAHCRU, KAHCRD, KAHCRH, KAHNMG,KAHCON,KAHFAM)
                        VALUES
                        (:KAHID, :KAHCIBLE, :KAHDESC, :KAHCRU, :KAHCRD, :KAHCRH, :KAHNMG, :KAHCON,:KAHFAM)";
                param[0] = new EacParameter("KAHID", guid);
                param[1] = new EacParameter("KAHCIBLE", codeCible.ToUpper());
                param[2] = new EacParameter("KAHDESC", descriptionCible);
                param[3] = new EacParameter("KAHCRU", user);
                int? dateCreation = AlbConvert.ConvertDateToInt(DateTime.Now);
                param[4] = new EacParameter("KAHCRD", dateCreation);
                param[5] = new EacParameter("KAHCRH", DateTime.Now.Hour.ToString());
                param[6] = new EacParameter("KAHNMG", grille);
                param[7] = new EacParameter("KAHCON", concept);
                param[8] = new EacParameter("KAHFAM", famille);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                return 1;
            }

            return -1;

        }

        public static void ModifierCible(string guidIdCible, string descriptionCible, string grille, string famille, string concept,string user)
        {
            string sql = @"UPDATE KCIBLE
                    SET KAHDESC='{1}',
                        KAHMAJD='{2}',
                        KAHMAJH='{3}',
                        KAHMAJU='{4}', 
                        KAHNMG='{5}',
                        KAHCON ='{6}',
                        KAHFAM ='{7}'
                    WHERE KAHID='{0}'";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, string.Format(sql, guidIdCible, descriptionCible, AlbConvert.ConvertDateToInt(DateTime.Now).ToString(), DateTime.Now.Hour.ToString(), user, grille,concept,famille));
        }

        public static string SupprimerCible(int guidIdCible, string infoUser)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P_PARAM", "CIBLE");
            param[1] = new EacParameter("P_CODEPARAM", 0);
            param[1].Value = Convert.ToInt32(guidIdCible);
            param[2] = new EacParameter("P_INFOUSER", infoUser);
            param[3] = new EacParameter("P_ERRORMSG", string.Empty);
            param[3].Direction = ParameterDirection.InputOutput;
            param[3].Size = 100;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEPARAMASSOC", param);

            return param[3].Value.ToString();
        }

        public static string SupprimerCibleBSCByGuidId(string guidId, string infoUser)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P_PARAM", "CIBLEF");
            param[1] = new EacParameter("P_CODEPARAM", 0);
            param[1].Value = Convert.ToInt32(guidId);
            param[2] = new EacParameter("P_INFOUSER", infoUser);
            param[3] = new EacParameter("P_ERRORMSG", string.Empty);
            param[3].Direction = ParameterDirection.InputOutput;
            param[3].Size = 100;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEPARAMASSOC", param);

            return param[3].Value.ToString();
        }
        public static int ModifierCibleBSCByGuidId(string guidIdCible, string codeCible, string codeBranche, string codeSousBranche, string codeCategorie)
        {
            string req1 = string.Format(@"SELECT count(*) NBLIGN FROM KCIBLEF WHERE KAIKAHID='{0}'
            AND KAIBRA='{1}' AND  KAISBR='{2}' AND KAICAT='{3}'", guidIdCible, codeBranche, codeSousBranche, codeCategorie);
            if (CommonRepository.ExistRow(req1)) return 0;

            string req2 = string.Format(@"SELECT count(*) NBLIGN FROM YCATEGO WHERE CABRA='{0}'{1}{2}",
                codeBranche,
                !string.IsNullOrEmpty(codeSousBranche) ? " AND CASBR='" + codeSousBranche + "'" : "",
                !string.IsNullOrEmpty(codeCategorie) ? " AND CACAT='" + codeCategorie + "'" : "");

            if (!CommonRepository.ExistRow(req2)) return 2;
            string sql = @"UPDATE KCIBLEF 
                          SET KAIBRA='{1}',
                          KAISBR='{2}',
                          KAICAT='{3}'
                         WHERE KAIID='{0}'";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, string.Format(sql, codeCible, codeBranche, codeSousBranche, codeCategorie));
            return -1;
        }

        public static ParamCiblesDto GetCible(string guidId)
        {
            DbParameter[] param = new DbParameter[1];

            string sqlListeCible = @"SELECT KAHID GUIDID,
                                    KAHCIBLE CODE,
                                    KAHDESC LIBELLE,
                                    KAHCRD DATECREA,
                                    KAHNMG CODEGRILLE,
                                     KAHCON CODECONCEPT, 
                                    KAHFAM CODEFAMILLE
                                    FROM KCIBLE WHERE KAHID =:ID";

            param[0] = new EacParameter("ID", guidId);
            return DbBase.Settings.ExecuteList<ParamCiblesDto>(CommandType.Text, sqlListeCible, param).FirstOrDefault();
        }

        public static bool ExisteCiblePortefeuille(int guidId)
        {
            string sqlExist = string.Format(@"SELECT COUNT(*) NBLIGN
                                            FROM KPOBJ 
                                            INNER JOIN KCIBLE ON KPOBJ.KACCIBLE = KCIBLE.KAHCIBLE
                                            WHERE KCIBLE.KAHID = {0}", guidId);
            return CommonRepository.ExistRow(sqlExist);
        }

        public static List<ParametreDto> GetGrilles()
        {
            string sql = @"SELECT KHJNMG CODE, KHJDESI LIBELLE 
                           FROM KNMGRI 
                           ORDER BY KHJNMG";

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }
    }
}
