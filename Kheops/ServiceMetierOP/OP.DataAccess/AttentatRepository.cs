using System;
using DataAccess.Helpers;
using System.Data;
using OP.WSAS400.DTO.AttentatGareat;
using System.Data.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Data;
using System.Linq;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.Common;
namespace OP.DataAccess
{
    public class AttentatRepository
    {
        #region Méthodes Publiques

        public static AttentatGareatDto InitAttentat(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            AttentatGareatDto toReturn = new AttentatGareatDto();
            string sql = string.Format(@"SELECT KAAATGKLC LCI, KAAATGKCA CAPITAUX, KAAATGSUR SURFACE, KAAATGBCN CATNAT, KAAATGCAF CAPITAUXFORCE,
                                    KAAATGAPT SOUMISSTD, KAAATGTRT TRANCHESTD, KAAATGTCT TAUXSTD, KAAATGTFT FRAISSTD, KAAATGCMT COMMISSIONSTD, KAAATGFAT FACTURESTD, 
                                    KAAATGAPR SOUMISRET, KAAATGTRR TRANCHERET, KAAATGTCR TAUXRET, KAAATGFRR FRAISRET, KAAATGTCM COMMISSIONRET, KAAATGTFA FACTURERET, IFNULL(KAJOBSV, '') COMMENTFORCE
                                FROM {0}
                                	LEFT JOIN {1} ON KDOIPB = KAAIPB AND KDOALX = KAAALX AND KDOTYP = KAATYP {7}
	                        		LEFT JOIN {2} ON KAJCHR = KAAOBSA
                                   WHERE KAATYP='{3}' AND KAAALX='{4}' AND KAAIPB='{5}' {6}",
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPENG"),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPOBSV"),
                                    type, version, codeOffre.PadLeft(9, ' '),
                                    modeNavig == ModeConsultation.Historique ? string.Format(" AND KAAAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                    modeNavig == ModeConsultation.Historique ? " AND KDOAVN = KAAAVN": string.Empty);

            var attentatGareatPlatDto = DbBase.Settings.ExecuteList<AttentatGareatPlatDto>(CommandType.Text, sql).FirstOrDefault();
            if (attentatGareatPlatDto != null)
            {
                toReturn.Capitaux = attentatGareatPlatDto.Capitaux.ToString();
                toReturn.CapitauxForces = attentatGareatPlatDto.CapitauxForces.ToString();
                toReturn.CATNAT = attentatGareatPlatDto.CATNAT.ToString();
                toReturn.CommentForce = attentatGareatPlatDto.CommentForce;
                toReturn.LCI = attentatGareatPlatDto.LCI.ToString();
                toReturn.MontantForce = attentatGareatPlatDto.MontantForce == "O" ? true : false;
                toReturn.Surface = attentatGareatPlatDto.Surface.ToString();
                toReturn.ParamStandard = new AttentatParametreDto
                {
                    Standard = true,
                    Commission = attentatGareatPlatDto.ParamStdrCommission.ToString(),
                    Facture = attentatGareatPlatDto.ParamStdrFacture.ToString(),
                    FraisGestion = attentatGareatPlatDto.ParamStdrFraisGestion.ToString(),
                    Soumis = attentatGareatPlatDto.ParamStdrSoumis == "O" ? true : false,
                    TauxCession = attentatGareatPlatDto.ParamStdrTauxCession.ToString(),
                    Tranche = attentatGareatPlatDto.ParamStdrTranche
                };
                toReturn.ParamRetenus = new AttentatParametreDto
                {
                    Standard = false,
                    Commission = attentatGareatPlatDto.ParamRetenusCommission.ToString(),
                    Facture = attentatGareatPlatDto.ParamRetenusFacture.ToString(),
                    FraisGestion = attentatGareatPlatDto.ParamRetenusFraisGestion.ToString(),
                    Soumis = attentatGareatPlatDto.ParamRetenusSoumis == "O" ? true : false,
                    TauxCession = attentatGareatPlatDto.ParamRetenusTauxCession.ToString(),
                    Tranche = attentatGareatPlatDto.ParamRetenusTranche
                };
            }

            return toReturn;
        }

        public static void UpdateAttentat(string codeOffre, string version, string type, string field, string value, string commentForce)
        {
            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_FIELD", field);
            param[4] = new EacParameter("P_VALUE", 0);
            param[4].Value = Convert.ToDecimal(value);
            param[5] = new EacParameter("P_COMMENTFORCE", commentForce);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDATT", param);
        }

        public static void UpdateAttentatComment(string codeOffre, string version, string type, string commentForce)
        {
            Int64? obsvId = 0;
            string sql = string.Format(@"SELECT IFNULL(KAAOBSA, 0) INT64RETURNCOL FROM KPENT WHERE KAATYP = '{0}' AND KAAIPB = '{1}' AND KAAALX = {2}", type, codeOffre.PadLeft(9, ' '), version);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Any())
                obsvId = result.FirstOrDefault().Int64ReturnCol;

            if (obsvId > 0)
            {
                sql = string.Format(@"UPDATE KPOBSV SET KAJOBSV = '{0}' WHERE KAJCHR = {1}", commentForce, obsvId);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }
            else
            {
                obsvId = CommonRepository.GetAS400Id("KAJCHR");
                sql = string.Format(@"INSERT INTO KPOBSV 
                                        ( KAJCHR , KAJIPB , KAJALX , KAJTYP , KAJOBSV ) 
                                    VALUES 
                                        ( {0}, '{1}', {2}, '{3}', '{4}')", obsvId, codeOffre.PadLeft(9, ' '), version, type, commentForce);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                sql = string.Format(@"UPDATE KPENT SET KAAOBSA = '{0}' WHERE KAAIPB = '{1}' AND KAAALX = {2} AND KAATYP = '{3}'", obsvId, codeOffre.PadLeft(9, ' '), version, type);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }
        }

        #endregion

        #region Méthodes Privées

        //private static AttentatGareatDto InitialiazeAttentat(DataRow row)
        //{
        //    AttentatGareatDto toReturn = new AttentatGareatDto();

        //    if (row.Table.Columns.Contains("LCI")) toReturn.LCI = row["LCI"].ToString();
        //    if (row.Table.Columns.Contains("CAPITAUX")) toReturn.Capitaux = row["CAPITAUX"].ToString();
        //    if (row.Table.Columns.Contains("SURFACE")) toReturn.Surface = row["SURFACE"].ToString();
        //    if (row.Table.Columns.Contains("CATNAT")) toReturn.CATNAT = row["CATNAT"].ToString();
        //    if (row.Table.Columns.Contains("CAPITAUXFORCE")) toReturn.CapitauxForces = row["CAPITAUXFORCE"].ToString();

        //    AttentatParametreDto paramStd = new AttentatParametreDto { Standard = true };
        //    if (row.Table.Columns.Contains("SOUMISSTD")) paramStd.Soumis = row["SOUMISSTD"].ToString() == "O" ? true : false;
        //    if (row.Table.Columns.Contains("TRANCHESTD")) paramStd.Tranche = row["TRANCHESTD"].ToString();
        //    if (row.Table.Columns.Contains("TAUXSTD")) paramStd.TauxCession = row["TAUXSTD"].ToString() != "0" ? row["TAUXSTD"].ToString() : "6,00";
        //    if (row.Table.Columns.Contains("FRAISSTD")) paramStd.FraisGestion = row["FRAISSTD"].ToString() != "0" ? row["FRAISSTD"].ToString() : "7,50";
        //    if (row.Table.Columns.Contains("COMMISSIONSTD")) paramStd.Commission = row["COMMISSIONSTD"].ToString() != "0" ? row["COMMISSIONSTD"].ToString() : "15,00";
        //    if (row.Table.Columns.Contains("FACTURESTD")) paramStd.Facture = row["FACTURESTD"].ToString();

        //    toReturn.ParamStandard = paramStd;

        //    AttentatParametreDto paramRet = new AttentatParametreDto { Standard = false };
        //    if (row.Table.Columns.Contains("SOUMISRET")) paramRet.Soumis = row["SOUMISRET"].ToString() == "O" ? true : false;
        //    if (row.Table.Columns.Contains("TRANCHERET")) paramRet.Tranche = row["TRANCHERET"].ToString();
        //    if (row.Table.Columns.Contains("TAUXRET")) paramRet.TauxCession = row["TAUXRET"].ToString();
        //    if (row.Table.Columns.Contains("FRAISRET")) paramRet.FraisGestion = row["FRAISRET"].ToString();
        //    if (row.Table.Columns.Contains("COMMISSIONRET")) paramRet.Commission = row["COMMISSIONRET"].ToString();
        //    if (row.Table.Columns.Contains("FACTURERET")) paramRet.Facture = row["FACTURERET"].ToString();

        //    if (row.Table.Columns.Contains("MONTANTFORCE")) toReturn.MontantForce = row["MONTANTFORCE"].ToString() == "O";
        //    if (row.Table.Columns.Contains("COMMENTFORCE")) toReturn.CommentForce = row["COMMENTFORCE"].ToString();


        //    toReturn.ParamRetenus = paramRet;

        //    return toReturn;
        //}

        #endregion
    }
}
