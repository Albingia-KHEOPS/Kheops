using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.ParamTemplates;
using System.Linq;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.ParametreCibles;
using OP.WSAS400.DTO.Offres.Branches;


namespace OP.DataAccess
{
    public class ParamTemplatesRepository
    {
        #region Param Template BO

        #region Méthodes Publiques

        public static List<ModeleLigneTemplateDto> LoadListTemplates(Int64 idTemplate, string codeTemplate, string descriptionTemplate, string typeTemplate, string lienCible, string branche, bool modeAutocomplete, bool existOnly)
        {
            string sql = @"SELECT
                              KGOID ID,
                              KGOCNVA CODE,
                              KGODESC DESCRIPTION,
                              KGOTYP TYPE,
                              KGOKAIID CIBLEREF,
                              KGOSIT SITUATION, 
                              KGOCDEF DEFAULTTEMPLATE                          
                           FROM KCANEV {0}                                           
                              ";

            string andWhere = " WHERE ";

            //branche - cible
            if (!string.IsNullOrEmpty(branche) && (!string.IsNullOrEmpty(lienCible)))
            {
                Int64 idCible = 0;
                if (!Int64.TryParse(lienCible, out idCible))
                {
                    sql += string.Format(" INNER JOIN KCIBLEF ON KGOKAIID = KAIKAHID AND KAIBRA = '{0}' AND KAICIBLE = '{1}' ", branche, lienCible);
                }
            }

            #region idTemplate
            if (idTemplate > 0)
            {
                sql += andWhere;
                sql += string.Format(" KGOID = {0} ", idTemplate);
                andWhere = " AND ";
            }
            #endregion
            #region filtre KCIBLE

            if (!string.IsNullOrEmpty(lienCible))
            {
                Int64 idCible = 0;
                if (Int64.TryParse(lienCible, out idCible))
                {
                    if (modeAutocomplete && idCible > 0)
                    {
                        sql += andWhere;
                        sql += string.Format(" KGOKAIID <> {0}", idCible);
                        andWhere = " AND ";
                    }
                    else if (modeAutocomplete && idCible == 0)
                    {
                        sql += andWhere;
                        sql += string.Format(" KGOKAIID = {0}", idCible);
                        andWhere = " AND ";
                    }
                    else
                    {
                        sql += andWhere;
                        sql += string.Format(" KGOKAIID = {0}", idCible);
                        andWhere = " AND ";
                    }
                }
            }


            #endregion
            #region code

            if (!string.IsNullOrEmpty(codeTemplate) && !modeAutocomplete)
            {
                sql += andWhere;
                sql += string.Format(" TRIM(KGOCNVA)='{0}'", codeTemplate.Trim());
                andWhere = " AND ";
            }
            else if (!string.IsNullOrEmpty(codeTemplate) && modeAutocomplete)
            {
                sql += andWhere;
                sql += string.Format(" TRIM(UPPER(KGOCNVA)) LIKE '%{0}%'", codeTemplate.Trim().ToUpper());
                andWhere = " AND ";
            }
            #endregion
            #region description

            if (!string.IsNullOrEmpty(descriptionTemplate) && !modeAutocomplete)
            {
                sql += andWhere;
                sql += string.Format(" KGODESC='{0}'", descriptionTemplate);
                andWhere = " AND ";
            }
            else if (!string.IsNullOrEmpty(descriptionTemplate) && modeAutocomplete)
            {
                sql += andWhere;
                sql += string.Format(" TRIM(LOWER(KGODESC)) LIKE '%{0}%'", descriptionTemplate.Trim().ToLower());
                andWhere = " AND ";
            }

            #endregion
            #region type

            if (!string.IsNullOrEmpty(typeTemplate) && typeTemplate != "*")
            {
                sql += andWhere;
                sql += string.Format(" KGOTYP =  '{0}'", typeTemplate);
                andWhere = " AND ";
            }

            #endregion
            #region exist only
            if (existOnly)
            {
                sql = string.Format(sql, " INNER JOIN YPOBASE ON PBIPB=KGOCNVA AND PBTYP = KGOTYP ");
                sql += andWhere;
                sql += " NOT EXISTS ( SELECT * FROM KVERROU WHERE KAVIPB = PBIPB AND KAVALX = PBALX AND KAVTYP = PBTYP) ";
                andWhere = " AND ";
            }
            else
            {
                sql = string.Format(sql, string.Empty);
            }
            #endregion
            sql += " ORDER BY KGOTYP, KGOCNVA";

            return DbBase.Settings.ExecuteList<ModeleLigneTemplateDto>(CommandType.Text, sql);
        }

        public static List<ModeleLigneTemplateDto> LoadListTemplatesCNVA(string codeTemplate, string type, bool modeAutocomplete, bool existOnly)
        {
            string sql = @"SELECT KGOID ID,
                              SUBSTR(TRIM(KGOCNVA),3) CODE,
                              KGODESC DESCRIPTION,
                              KGOTYP TYPE,
                              KGOKAIID CIBLEREF,
                              KGOSIT SITUATION, 
                              KGOCDEF DEFAULTTEMPLATE FROM KCANEV {0} ";

            string andWhere = " WHERE KGOSIT = 'V' AND ";
            #region code

            if (!string.IsNullOrEmpty(codeTemplate) && !modeAutocomplete)
            {
                sql += andWhere;
                sql += string.Format(" TRIM(KGOCNVA)='{0}'", codeTemplate.Trim());
                andWhere = " AND ";
            }
            else if (!string.IsNullOrEmpty(codeTemplate) && modeAutocomplete)
            {
                sql += andWhere;
                sql += string.Format(" TRIM(UPPER(KGOCNVA)) LIKE '%{0}%'", codeTemplate.Trim().ToUpper());
                andWhere = " AND ";
            }
            if (!string.IsNullOrEmpty(type))
            {
                sql += andWhere;
                sql += string.Format(" KGOTYP = '{0}'", type);
                andWhere = " AND ";
            }

            #endregion
            #region exist only
            if (existOnly)
            {
                sql = string.Format(sql, " INNER JOIN YPOBASE ON PBIPB = KGOCNVA AND PBTYP = KGOTYP ");
                sql += andWhere;
                sql += " NOT EXISTS ( SELECT * FROM KVERROU WHERE KAVIPB = PBIPB AND KAVALX = PBALX AND KAVTYP = PBTYP) ";
                andWhere = " AND ";
            }
            else
            {
                sql = string.Format(sql, string.Empty);
            }
            #endregion

            sql += " ORDER BY KGOTYP, KGOCNVA";

            return DbBase.Settings.ExecuteList<ModeleLigneTemplateDto>(CommandType.Text, sql);
        }

        public static ModeleLigneTemplateDto GetDetailsTemplate(Int64 idTemplate)
        {
            var param = new EacParameter[1];
            param[0] = new EacParameter("idTemplate", DbType.Int64);
            param[0].Value = idTemplate;
            string sql = @"SELECT 
KGOID ID,
KGOCNVA CODE,
KGODESC DESCRIPTION,
KGOTYP TYPE,
KGOCRU USERCREATION,
KGOCRD DATECREATION,                                          
KGOMAJU USERMODIFICATION,
KGOMAJD DATEMODIFICATION                                       
FROM KCANEV 
WHERE KGOID = :idTemplate";

            var result = DbBase.Settings.ExecuteList<ModeleLigneTemplateDto>(CommandType.Text, sql, param);
            if (result.Any())
            {
                return result.FirstOrDefault();
            }
            return null;
        }

        public static string EnregistrerTemplate(string mode, ModeleLigneTemplateDto template, string user)
        {
            string sql = string.Empty;
            string errorMsg = string.Empty;
            EacParameter[] param = null;

            if (mode == "Insert")
            {
                //Verification de l'existence du code dans la table canevas
                EacParameter[] paramSqlExist = new EacParameter[1];
                paramSqlExist[0] = new EacParameter("codeTemplate", DbType.AnsiStringFixedLength);
                paramSqlExist[0].Value = template.CodeTemplate.PadLeft(9, ' ');
                string sqlExist = "SELECT 1 NBLIGN FROM KCANEV WHERE KGOCNVA = :codeTemplate";
                if (CommonRepository.ExistRowParam(sqlExist, paramSqlExist))
                    errorMsg = "Ce code de template est déjà utilisé";
                //Verification de l'existence du code dans la table ypobase
                EacParameter[] paramSqlExist2 = new EacParameter[2];
                paramSqlExist2[0] = new EacParameter("codeTemplate", DbType.AnsiStringFixedLength);
                paramSqlExist2[0].Value = template.CodeTemplate.PadLeft(9, ' ');
                paramSqlExist2[1] = new EacParameter("typeTemplate", DbType.AnsiStringFixedLength);
                paramSqlExist2[1].Value = template.TypeTemplate;
                string sqlExist2 = "SELECT 1 NBLIGN FROM YPOBASE WHERE PBIPB = :codeTemplate AND PBTYP = :typeTemplate AND PBALX = 0";
                if (CommonRepository.ExistRowParam(sqlExist2, paramSqlExist2))
                    errorMsg = "Ce code de template est déjà utilisé";

                

                if (string.IsNullOrEmpty(errorMsg))
                {                  
                    int id = CommonRepository.GetAS400Id("KGOID");

                    param = new EacParameter[10];
                    param[0] = new EacParameter("id", DbType.Int32);
                    param[0].Value = id;
                    param[1] = new EacParameter("typeTemplate", DbType.AnsiStringFixedLength);
                    param[1].Value = template.TypeTemplate;
                    param[2] = new EacParameter("codeTemplate", DbType.AnsiStringFixedLength);
                    param[2].Value = template.CodeTemplate.PadLeft(9, ' ');
                    param[3] = new EacParameter("descriptionTemplate", DbType.AnsiStringFixedLength);
                    param[3].Value = template.DescriptionTemplate;
                    param[4] = new EacParameter("aValue", DbType.Int32);
                    param[4].Value = 0;
                    param[5] = new EacParameter("emptyString", DbType.AnsiStringFixedLength);
                    param[5].Value = string.Empty;
                    param[6] = new EacParameter("user", DbType.AnsiStringFixedLength);
                    param[6].Value = user;
                    param[7] = new EacParameter("date", DbType.AnsiStringFixedLength);
                    param[7].Value = DateTime.Now.ToString("yyyyMMdd");
                    param[8] = new EacParameter("heure", DbType.AnsiStringFixedLength);
                    param[8].Value = DateTime.Now.ToString("HHmm");
                    param[9] = new EacParameter("aChar", DbType.AnsiStringFixedLength);
                    param[9].Value = "N";

                    sql = @"INSERT INTO KCANEV 
                                        (KGOID, KGOTYP, KGOCNVA, KGODESC, KGOKAIID, KGOCDEF, 
                                         KGOCRU, KGOCRD, KGOCRH, KGOMAJU, KGOMAJD, KGOMAJH, KGOSIT) 
                                      VALUES (:id, :typeTemplate, :codeTemplate, :descriptionTemplate, :aValue, :emptyString, :user, :date, :heure, :user, :date, :heure, :aChar)";
                }

            }
            else if (mode == "Update")
            {
                //Verification de l'existence du template 
                EacParameter[] paramSqlExist = new EacParameter[1];
                paramSqlExist[0] = new EacParameter("templateGuidId", DbType.Int64);
                paramSqlExist[0].Value = template.GuidId;
                string sqlExist = "SELECT 1 NBLIGN FROM KCANEV WHERE KGOID = :templateGuidId ";
                if (!CommonRepository.ExistRowParam(sqlExist, paramSqlExist))
                    errorMsg = "Ce template n'existe plus";

                if (string.IsNullOrEmpty(errorMsg))
                {
                    param = new EacParameter[5];
                    param[0] = new EacParameter("descriptionTemplate", DbType.AnsiStringFixedLength);
                    param[0].Value = template.DescriptionTemplate;
                    param[1] = new EacParameter("user", DbType.AnsiStringFixedLength);
                    param[1].Value = user;
                    param[2] = new EacParameter("date", DbType.AnsiStringFixedLength);
                    param[2].Value = DateTime.Now.ToString("yyyyMMdd");
                    param[3] = new EacParameter("heure", DbType.AnsiStringFixedLength);
                    param[3].Value = DateTime.Now.ToString("HHmm");
                    param[4] = new EacParameter("templateGuidId", DbType.Int64);
                    param[4].Value = template.GuidId;
                   
                    sql = @"UPDATE KCANEV 
                                        SET KGODESC = :descriptionTemplate,
                                            KGOMAJU = :user,
                                            KGOMAJD = :date,
                                            KGOMAJH = :heure
                                      WHERE KGOID = :templateGuidId";
                }
            }
            if (!string.IsNullOrEmpty(sql))
            {
                int res = DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                if (res != 1)
                    errorMsg = "Un erreur s'est produite lors de l'enregistrement du template.";
            }
            return errorMsg;
        }

        public static string SupprimerTemplate(Int64 idTemplate)
        {
            string errorMsg = string.Empty;

            //Verification de l'existence du template    
            EacParameter[] paramSqlExist = new EacParameter[1];
            paramSqlExist[0] = new EacParameter("idTemplate", DbType.Int64);
            paramSqlExist[0].Value = idTemplate;
            string sqlExist = "SELECT 1 NBLIGN FROM KCANEV WHERE KGOID = :idTemplate ";
            if (!CommonRepository.ExistRowParam(sqlExist, paramSqlExist))
                errorMsg = "Ce template n'existe plus";

            //Suppression
            if (string.IsNullOrEmpty(errorMsg))
            {
                EacParameter[] param = new EacParameter[5];
                param[0] = new EacParameter("P_CANEVAS", DbType.Int32);
                param[0].Value = 0;
                param[1] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[1].Value = string.Empty;
                param[2] = new EacParameter("P_VERSION", DbType.Int32);
                param[2].Value = 2;
                param[3] = new EacParameter("P_CANEVASID", DbType.Int64);
                param[3].Value = idTemplate;
                param[4] = new EacParameter("P_ERROROUT", DbType.Int32);
                param[4].Value = 0;
                param[4].Direction = ParameterDirection.InputOutput;
                param[4].DbType = DbType.Int32;

                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_REMOVEINFOTABLE", param);

                int res = 0;
                res = Convert.ToInt32(param[4].Value.ToString());

                if (res != 1)
                    errorMsg = "Un problème a eu lieu lors de la suppression du template";
            }
            return errorMsg;
        }
        public static void UpdateCibleTemplate(string idTemp, bool isChecked)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("P_IDTEMP", DbType.Int32);
            param[0].Value = Convert.ToInt32(idTemp);
            param[1] = new EacParameter("P_ISCHECKED", DbType.AnsiStringFixedLength);
            param[1].Value = isChecked ? "O" : "N";

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDATECIBLETEMPLATE", param);
        }

        public static List<ModeleLigneTemplateDto> DeleteCibleTemplate(string idCible, string idTemplate)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("P_IDTEMP", DbType.Int32);
            param[0].Value = Convert.ToInt32(idTemplate);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETECIBLETEMPLATE", param);

            return LoadListTemplates(0, null, null, null, idCible, null, false, false);
        }

        public static bool ExistCanevas(string idTemplate)
        {
            EacParameter[] paramExistRow = new EacParameter[1];
            paramExistRow[0] = new EacParameter("idTemplate", DbType.AnsiStringFixedLength);
            paramExistRow[0].Value = idTemplate;

            string sql = @"SELECT 1 NBLIGN 
                                         FROM YPOBASE 
                                         INNER JOIN KCANEV ON PBIPB=KGOCNVA AND PBTYP = KGOTYP 
		                                 WHERE KGOID = :idTemplate";
            return CommonRepository.ExistRowParam(sql, paramExistRow);
        }

        public static List<ModeleLigneTemplateDto> AssociateCibleTemplate(string idCible, string idTemplate)
        {
            InsertCibleTemplate(idCible, idTemplate);
            return LoadListTemplates(0, null, null, null, idCible, null, false, false);
        }

        public static string GetParamTemplate(string idTemp, string tempFlag)
        {
            var returnParam = string.Empty;

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("idTemp", DbType.Int32);
            param[0].Value = Convert.ToInt32(idTemp);

            string sql = @"SELECT KGOCNVA CODETEMP, KGOTYP TYPETEMP, KGOSIT SITUATIONTEMP
                            FROM KCANEV
                            WHERE KGOID = :idTemp";

            var result = DbBase.Settings.ExecuteList<TemplatePlatDto>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0)
            {
                var paramTemp = result.FirstOrDefault();
                if (paramTemp.Situation == "V")
                    returnParam = string.Format("{0}_{1}_{2}", paramTemp.Code, 0, paramTemp.Type);
                else
                    returnParam = idTemp + tempFlag;
            }
            return returnParam;
        }

        #endregion

        #region Méthodes Privées

        private static void InsertCibleTemplate(string idCible, string idTemplate)
        {
            string isDefault = "N";

            EacParameter[] paramExistRow = new EacParameter[2];
            paramExistRow[0] = new EacParameter("idTemplate", DbType.AnsiStringFixedLength);
            paramExistRow[0].Value = idTemplate;
            paramExistRow[1] = new EacParameter("idCible", DbType.AnsiStringFixedLength);
            paramExistRow[1].Value = idCible;

            string sql = @"SELECT COUNT(*) NBLIGN 
	                            FROM KCANEV CANEV1
		                            INNER JOIN KCANEV CANEV2 ON CANEV2.KGOID = :idTemplate
                            WHERE CANEV1.KGOKAIID = :idCible AND CANEV1.KGOCDEF = 'O' AND CANEV1.KGOTYP = CANEV2.KGOTYP";

            if (!CommonRepository.ExistRowParam(sql, paramExistRow))
                isDefault = "O";

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("idCible", DbType.Int32);
            param[0].Value = Convert.ToInt32(idCible);
            param[1] = new EacParameter("isDefault", DbType.AnsiStringFixedLength);
            param[1].Value = isDefault; 
            param[2] = new EacParameter("idTemplate", DbType.Int32);
            param[2].Value = Convert.ToInt32(idTemplate);

            sql = @"UPDATE KCANEV SET KGOKAIID = :idCible, KGOCDEF = :isDefault
                        WHERE KGOID = :idTemplate";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        #endregion

        #endregion

        #region Template

        public static OffreDto GetInfoTemplate(string idTemp)//, string idCible)
        {
            OffreDto model = new OffreDto();

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("idTemp", DbType.Int32);
            param[0].Value = Convert.ToInt32(idTemp);

            string sql = @"SELECT KGOCNVA CODETEMP, KGOTYP TYPETEMP, KAICIBLE CIBLETEMP, KAIBRA BRANCHETEMP
                            FROM KCANEV 
	                            INNER JOIN KCIBLEF ON KAIKAHID = KGOKAIID
                            WHERE KGOID = :idTemp";

            var result = DbBase.Settings.ExecuteList<TemplatePlatDto>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0)
            {
                model.CodeOffre = result[0].Code;
                model.Type = result[0].Type;
                model.Version = 0;
                model.Branche = new BrancheDto
                {
                    Code = result[0].Branche,
                    Cible = new CibleDto { Code = result[0].Cible }
                };
            }

            return model;
        }

        public static void UpdateTemplate(string codeTemp)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codeTemp", DbType.AnsiStringFixedLength);
            param[0].Value = codeTemp.PadLeft(9, ' ');
            string sql = @"UPDATE KCANEV SET KGOSIT = 'V' WHERE KGOCNVA = :codeTemp ";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        #endregion
    }
}
