using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

using ALBINGIA.Framework.Common.Data;
using System.Data;
using OP.WSAS400.DTO.Inventaires;

namespace OP.DataAccess
{
    public class AdresseHexaviaRepository : RepositoryBase
    {
        internal readonly static string SelectVillesByCP = "SELECT ABLLIB , CAST ( ABLCPO AS INTEGER ) ABLCPO FROM GENERALE . YHEXLOC WHERE ABLCPO LIKE '%' CONCAT CAST( :cp AS VARCHAR( 5 ) ) CONCAT '%' ;";
        internal readonly static string SelectVillesByCPStart = "SELECT ABLLIB , CAST ( ABLCPO AS INTEGER ) ABLCPO FROM GENERALE . YHEXLOC WHERE ABLCPO LIKE CAST( :cp AS VARCHAR( 5 ) ) CONCAT '%' ;";
        public AdresseHexaviaRepository(IDbConnection connection) : base(connection) { }

        #region Méthodes Publiques

        public static List<ParametreDto> GetVillesByCP(string codePostal)
        {
            return RechercheVillesParCodePostal(codePostal);
        }

        public static List<ParametreDto> GetCPByVille(string ville)
        {
            return RechercheCPParVille(ville);
        }

        public static List<CPVilleDto> GetCodePostalVille(string value, string mode)
        {
            List<CPVilleDto> toReturn = new List<CPVilleDto>();

            DbParameter[] param = new DbParameter[2];
            param[0] = new EacParameter("P_MODE", mode);
            param[1] = new EacParameter("P_VALUE", value);

            var result = DbBase.Settings.ExecuteList<CPVilleDto>(CommandType.StoredProcedure, "SP_GTCPVIL", param);
            if (result != null && result.Count > 0)
                return result;
            return new List<CPVilleDto>();
        }

        #endregion
        #region Méthodes Privées

        private static List<ParametreDto> RechercheVillesParCodePostal(string codePostal)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("P_CODEPOSTAL", 0);
            param[0].Value = Convert.ToInt32(codePostal);
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.StoredProcedure, "SP_RECHERCHEVILLESPARCODEPOSTAL", param);
        }



        private static List<ParametreDto> RechercheCPParVille(string ville)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("P_VILLE", ville);

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.StoredProcedure, "SP_RECHERCHECPPARVILLE", param);
        }


        #endregion

        public IEnumerable<(string abllib, int ablcpo)> FindVilles(int codePostal, bool machAnywhere = false) {
            return Fetch<(string, int)>(machAnywhere ? SelectVillesByCP : SelectVillesByCPStart, codePostal);
        }
    }
}
