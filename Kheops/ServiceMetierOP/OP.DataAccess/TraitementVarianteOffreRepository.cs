using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Data;
using System.Data.EasycomClient;
using System.Globalization;

namespace OP.DataAccess
{
    public class TraitementVarianteOffreRepository
    {
        public static bool CreationNouvelleVersionOffre(string codeOffre, string version, string type, string utilisateur, string traitement)
        {
            int nombreVersionsAvant = NombreVersionsOffre(codeOffre, type);

            DateTime now = DateTime.Now;
            EacParameter[] param = new EacParameter[8];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = int.Parse(version);
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_DATESAISIE", DbType.AnsiStringFixedLength);
            param[3].Value = AlbConvert.ConvertDateToInt(now).ToString();
            param[4] = new EacParameter("P_HEURESAISIE", DbType.AnsiStringFixedLength);
            param[4].Value = (now.Hour * 100 + now.Minute).ToString();
            param[5] = new EacParameter("P_DATESYSTEME", DbType.AnsiStringFixedLength);
            param[5].Value = AlbConvert.ConvertDateToInt(now).ToString();
            param[6] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[6].Value = utilisateur;
            param[7] = new EacParameter("P_TRAITEMENT", DbType.AnsiStringFixedLength);
            param[7].Value = AlbConstantesMetiers.Traitement.VersionOffre.AsCode();
            //param[7] = new EacParameter("P_TRAITEMENT", traitement);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_VERSION", param);

            // (string codeOffre, string version, string type, string codeAvn, string user, string acteGestion)            

            int nombreVersionsApres = NombreVersionsOffre(codeOffre, type);


            CommonRepository.LoadAS400Matrice(codeOffre, nombreVersionsApres.ToString(), type, string.Empty, utilisateur, string.Empty);

            CommonRepository.ReloadEngagement(codeOffre, nombreVersionsApres.ToString(), type, codeOffre, version, type, utilisateur, AlbConstantesMetiers.TRAITEMENT_OFFRE);

            //Copy des documents 
            CopieDocRepository.CopierDocuments(codeOffre, nombreVersionsApres.ToString(CultureInfo.InvariantCulture), type, "0");
            return ((nombreVersionsApres == nombreVersionsAvant + 1) ? true : false);
        }

        private static int NombreVersionsOffre(string codeOffre, string type)
        {
            EacParameter[] param = new EacParameter[2];
            string sql = @"SELECT MAX(PBALX) FROM YPOBASE WHERE PBIPB = :PBIPB AND PBTYP = :PBTYP";
            param = new EacParameter[2];
            param[0] = new EacParameter("PBIPB", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("PBTYP", DbType.AnsiStringFixedLength);
            param[1].Value = type;
            object result = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param);
            int count = int.Parse(result.ToString());
            return count;
        }
    }
}
