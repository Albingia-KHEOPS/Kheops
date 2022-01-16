using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.ParametreEngagement;
using ALBINGIA.Framework.Common.Data;

namespace OP.DataAccess
{
    public class ParamEngagementRepository
    {
        #region Méthodes Publiques

        public static ParamEngagementDto InitParamEngagement()
        {
            ParamEngagementDto model = new ParamEngagementDto();
            model.Traites = CommonRepository.GetParametres(string.Empty, string.Empty, "REASS", "GARAN");
            return model;
        }
        public static ParamEngagementDto GetListColonne(string codeTraite)
        {
            ParamEngagementDto model = new ParamEngagementDto();
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("codeTraite", codeTraite);
            string sql = @"SELECT KGAFAM CODETRAITE, KGAVEN CODE, KGALIBV LIBELLE, KGASEPA SEPARATION
                            FROM KREAVEN 
                            WHERE KGAFAM = :codeTraite
                            ORDER BY KGAVEN ASC";
            model.ParamsColonne = DbBase.Settings.ExecuteList<ParamEngmentColonneDto>(CommandType.Text, sql, param);
            return model;
        }
        public static ParamEngmentColonneDto LoadColonne(string codeTraite, string code)
        {
            ParamEngmentColonneDto model = new ParamEngmentColonneDto();

            if (code == "0")
                return LoadAddColonne(codeTraite);
            else
                return LoadMAJColonn(codeTraite, code);
        }
        public static void SaveColonne(string codeTraite, string code, string libelle, string separation, string mode)
        {
            switch (mode)
            {
                case "INS":
                    InsertColonneTraite(codeTraite, code, libelle, separation);
                    break;
                case "MAJ":
                    UpdateColonneTraite(codeTraite, code, libelle, separation);
                    break;
            }
        }
        public static string DeleteColonne(string codeTraite, string code, string infoUser)
        {
            //DbParameter[] param = new DbParameter[4];
            //param[0] = new EacParameter("P_CODEFAMILLE", codeTraite);
            //param[1] = new EacParameter("P_CODEVENTILATION", 0);
            //param[1].Value = !string.IsNullOrEmpty(code) ? Convert.ToInt32(code) : 0;
            //param[2] = new EacParameter("P_INFOUSER", infoUser);
            //param[3] = new EacParameter("P_ERRORMSG", string.Empty);
            //param[3].Direction = ParameterDirection.InputOutput;
            //param[3].Size = 100;

            //DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEPARAMENGAGEMENT", param);

            //return param[3].Value.ToString();



            DbParameter[] param = new DbParameter[2];
            param[0] = new EacParameter("codeTraite", codeTraite);
            param[1] = new EacParameter("code", 0);
            param[1].Value = Convert.ToInt32(code);

            string sql = @"DELETE FROM KREAVEN
                            WHERE KGAFAM = :codeTraite AND KGAVEN = :code";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            return string.Empty;
        }

        #endregion
        #region Méthodes Privées

        /// <summary>
        /// Charge les infos pour un ajout d'une colonne
        /// </summary>
        private static ParamEngmentColonneDto LoadAddColonne(string codeTraite)
        {
            ParamEngmentColonneDto model = new ParamEngmentColonneDto { CodeTraite = codeTraite, Code = 1 };

            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("codeTraite", codeTraite);

            string sql = @"SELECT KGAFAM CODETRAITE, MAX(KGAVEN)+1 CODE
                            FROM KREAVEN
                            WHERE KGAFAM = :codeTraite
                            GROUP BY KGAFAM";

            var result = DbBase.Settings.ExecuteList<ParamEngmentColonneDto>(CommandType.Text, sql, param);
            if (result != null && result.FirstOrDefault() != null)
                model.Code = result.FirstOrDefault().Code;

            return model;
        }
        /// <summary>
        /// Charge les infos pour une MAJ d'une colonne
        /// </summary>
        private static ParamEngmentColonneDto LoadMAJColonn(string codeTraite, string code)
        {
            ParamEngmentColonneDto model = new ParamEngmentColonneDto();
            DbParameter[] param = new DbParameter[2];
            param[0] = new EacParameter("codeTraite", codeTraite);
            param[1] = new EacParameter("code", 0);
            param[1].Value = Convert.ToInt32(code);

            string sql = @"SELECT KGAFAM CODETRAITE, KGAVEN CODE, KGALIBV LIBELLE, KGASEPA SEPARATION
                            FROM KREAVEN
                            WHERE KGAFAM = :codeTraite AND KGAVEN = :code";

            var result = DbBase.Settings.ExecuteList<ParamEngmentColonneDto>(CommandType.Text, sql, param);
            if (result != null && result.FirstOrDefault() != null)
                model = result.FirstOrDefault();

            return model;
        }
        /// <summary>
        /// Insère une nouvelle colonne
        /// pour un traité
        /// </summary>
        private static void InsertColonneTraite(string codeTraite, string code, string libelle, string separation)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("codeTraite", codeTraite);
            param[1] = new EacParameter("code", 0);
            param[1].Value = Convert.ToInt32(code);
            param[2] = new EacParameter("libelle", libelle);
            param[3] = new EacParameter("separation", separation);

            string sql = @"INSERT INTO KREAVEN
                            (KGAFAM, KGAVEN, KGALIBV, KGASEPA)
                            VALUES
                            (:codeTraite, :code, :libelle, :separation)";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        /// <summary>
        /// Sauvegarde une colonne pour un traité
        /// </summary>
        private static void UpdateColonneTraite(string codeTraite, string code, string libelle, string separation)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("libelle", libelle);
            param[1] = new EacParameter("separation", separation);
            param[2] = new EacParameter("codeTraite", codeTraite);
            param[3] = new EacParameter("code", 0);
            param[3].Value = Convert.ToInt32(code);

            string sql = @"UPDATE KREAVEN SET KGALIBV = :libelle, KGASEPA = :separation
                            WHERE KGAFAM = :codeTraite AND KGAVEN = :code";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        #endregion
    }
}
