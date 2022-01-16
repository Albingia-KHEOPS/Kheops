using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Helpers;
using System.Globalization;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.Offres.Aperiteur;
using System.Linq;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
namespace OP.DataAccess
{
    public class AperiteurRepository
    {
        public static AperiteurDto Initialiser(DataRow ligne, string prefixe = "")
        {
            var aperiteur = new AperiteurDto();

            if (ligne.Table.Columns.Contains("CIICI")) { aperiteur.Code = ligne["CIICI"].ToString(); };
            if (ligne.Table.Columns.Contains("CINOM")) { aperiteur.Nom = ligne["CINOM"].ToString(); };

            return aperiteur;
        }

        public static List<AperiteurDto> Rechercher(int debut, int fin, string nom)
        {
            bool necessiteChargement = !(debut == 1 && fin == 10 && String.IsNullOrEmpty(nom));
            if (necessiteChargement)
            {
                string sql = string.Format(CultureInfo.CurrentCulture, @"
                SELECT CIICI CODE, CINOM NOM
                FROM ( SELECT rownumber() OVER (ORDER BY CINOM) ID_NEXT, CIICI, CINOM
                    FROM YCOMPA
                    WHERE CINOM LIKE '{0}%') AS TABLE
                WHERE ID_NEXT BETWEEN {1} AND {2}", nom.Replace("'", "''").ToUpperInvariant().Trim(), debut, fin);
                return DbBase.Settings.ExecuteList<AperiteurDto>(CommandType.Text, sql);
            }
            return new List<AperiteurDto>();
        }

        public static AperiteurDto Obtenir(string codeAperiteur)
        {
            string sql = string.Format(CultureInfo.CurrentCulture, @"
             SELECT CIICI CODE, CINOM NOM                
                    FROM YCOMPA
                    WHERE UPPER(CIICI) = UPPER('{0}') ", codeAperiteur);
            var aperiteurDto = DbBase.Settings.ExecuteList<AperiteurDto>(CommandType.Text, sql);
            if (aperiteurDto != null && aperiteurDto.Any())
                return aperiteurDto.FirstOrDefault();
            return null;
        }

        public static AperiteurDto ObtenirByCodeNum(Int64 codeAperiteur)
        {
            string sql = string.Format(CultureInfo.CurrentCulture, @"
             SELECT CIICI CODE, CIICN CODENUM, CINOM NOM                
                    FROM YCOMPA
                    WHERE CIICN = {0} ", codeAperiteur);
            var aperiteurDto = DbBase.Settings.ExecuteList<AperiteurDto>(CommandType.Text, sql);
            if (aperiteurDto != null && aperiteurDto.Any())
                return aperiteurDto.FirstOrDefault();
            return null;
        }

        public static List<InterlocuteurDto> RechercherInterlocuteursAperiteur(string nomInterlocuteur, string codeAperiteur)
        {
            string sql = String.Format(@"SELECT CLNOM NOM, CLIN5 PBIN5
                                              FROM YCOMPAL
                                              WHERE CLICI = '{0}'  
                                              AND CLNOM LIKE '%{1}%'
                                              FETCH FIRST 10 ROWS ONLY", codeAperiteur.Trim(), nomInterlocuteur.Trim().ToUpper());
            return DbBase.Settings.ExecuteList<InterlocuteurDto>(CommandType.Text, sql);
        }
    }
}
