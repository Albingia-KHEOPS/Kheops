using System.Collections.Generic;
using DataAccess.Helpers;
using System.Data;
using System;
using OP.WSAS400.DTO.Categorie;
using ALBINGIA.Framework.Common.Data;
using System.Linq;
using System.Data.EasycomClient;

namespace OP.DataAccess
{
    public  class CategorieRepository
    {
        #region Méthode Publique
        public static List<CategorieDto> ObtenirCategories()
        {

            string sql = @"SELECT KAIID GUID, KAIBRA BRANCHE, KAICIBLE CODE, KAHDESC DESCRIPTION	 
                                    FROM  KCIBLEF
                                    INNER JOIN KCIBLE ON  KAICIBLE= KAHCIBLE
                                    ORDER BY KAHDESC";
            return DbBase.Settings.ExecuteList<CategorieDto>(CommandType.Text, sql);

        }
        public static List<CategorieDto> ObtenirBrancheParVolet(string code)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codevolet", DbType.AnsiStringFixedLength);
            param[0].Value = code;

            string sql = @"SELECT KAPBRA BRANCHE, KAPCIBLE CODE, KAHDESC DESCRIPTION , KAPCAR CARACTERE
                              FROM KCATVOLET
                              INNER JOIN KCIBLE ON KAPCIBLE = KAHCIBLE
                               WHERE KAPKAKID=:codevolet
                                 ORDER BY KAPKAIID";

            return DbBase.Settings.ExecuteList<CategorieDto>(CommandType.Text, sql, param);
        }
        public static CategorieDto ObtenirCategorieByCode(string code)
        {
            string sql = @"SELECT KAMID GUID, KAMBRA BRANCHE, KAMCAT CODE, KAMDESC DESCRIPTION	 
                        FROM KCATEGO WHERE KAMID";
            var categoriesDto = DbBase.Settings.ExecuteList<CategorieDto>(CommandType.Text, sql);
            if (categoriesDto != null && categoriesDto.Any())
            {
                return categoriesDto.FirstOrDefault();
            }
            return new CategorieDto();
        }
        #endregion
    }
}
