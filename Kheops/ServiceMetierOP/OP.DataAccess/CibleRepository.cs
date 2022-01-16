using System.Collections.Generic;
using System.Linq;
using OP.WSAS400.DTO.Offres.Branches;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Data;
using ALBINGIA.Framework.Common.Data;
using System;

namespace OP.DataAccess
{
    public class CibleRepository
    {

        #region Méthodes Publiques

        public static List<CibleDto> RechercherCibles(string codeBranche)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codeBranche", DbType.AnsiStringFixedLength);
            param[0].Value = codeBranche;

            string sql = @"SELECT KAIID GUID, KAICIBLE CODE, KAHDESC DESCRIPTION 
                    FROM KCIBLEF
                        INNER JOIN KCIBLE ON KAHID = KAIKAHID
                    WHERE KAIBRA = :codeBranche
                    ORDER BY KAICIBLE";

            return DbBase.Settings.ExecuteList<CibleDto>(CommandType.Text, sql, param);
        }

        public static string GetLibCibleF(string codeCible)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codeCible", DbType.AnsiStringFixedLength);
            param[0].Value = codeCible;

            string sql = @"SELECT KAICIBLE CODE
                    FROM KCIBLEF
                        WHERE KAIID = :codeCible
                        ORDER BY KAICIBLE";

            var firstOrDefault = DbBase.Settings.ExecuteList<CibleDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (firstOrDefault != null)
                return firstOrDefault.Code;
            return string.Empty;
        }

        public static Int64 GetIdCible(string codeCible)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codeCible", DbType.AnsiStringFixedLength);
            param[0].Value = codeCible;

            string sql = @"SELECT KAIID GUID FROM KCIBLEF WHERE KAICIBLE = :codeCible";
            //codeCible);
            var firstOrDefault = DbBase.Settings.ExecuteList<CibleDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (firstOrDefault != null)
                return firstOrDefault.GuidId;
            return 0;
        }


        #endregion

        #region Méthodes Privées


        #endregion

    }
}
