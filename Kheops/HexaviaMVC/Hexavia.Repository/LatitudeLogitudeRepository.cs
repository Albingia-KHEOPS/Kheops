using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using Hexavia.Repository.Interfaces;
using Hexavia.Tools.Helpers;
using Hexavia.Tools.Validators;

namespace Hexavia.Repository
{
    /// <summary>
    /// KGEOLOCRepository
    /// </summary>
    public class LatitudeLogitudeRepository : BaseRepository, ILatitudeLogitudeRepository
    {
        private readonly string SqlAroundPart = "#AROUND-BLOC-SQL#";
        private readonly string SqlSinistrePartColumn = "#SINISTRE-BLOC-COLUMN-SQL#";
        private readonly string SqlDepartementPartColumn = "#DEPARTEMENT-BLOC-COLUMN-SQL#";
        private readonly string SqlSinistrePart = "#SINISTRE-BLOC-SQL#";
        private readonly string SqlDepartementPart = "#DEPARTEMENT-BLOC-SQL#";

        public LatitudeLogitudeRepository(DataAccessManager dataAccessManager)
           : base(dataAccessManager)
        {
        }

        public List<KGeoloc> GetAllLatLon()
        {
            var result = new List<KGeoloc>();
            var cmd = DataAccessManager.CmdWrapper;
            //selection des adresses localisées qui ont une correspondance dans YPRTADR
            cmd.CommandText = string.Format(@"SELECT KGEOCHR, LONGITUDE, LATITUDE
                                              FROM   KGEOLOC");
            cmd.Where(@"(LATITUDE <> 0 OR LONGITUDE <> 0)");
            var dataTable = DataAccessManager.ExecuteDataTable(cmd);

            foreach (DataRow row in dataTable.Rows)
            {
                var KGEOLOC = new KGeoloc
                {
                    NumeroChrono = int.Parse(row["KGEOCHR"].ToString()),
                    Lat = OutilsHelper.ToDouble(row["LATITUDE"].ToString()),
                    Lon = OutilsHelper.ToDouble(row["LONGITUDE"].ToString()),
                };
                result.Add(KGEOLOC);
            }
            return result;
        }

        public List<KGeolocCase> GetAllCasesLatLon(string code, string version, string designation, string typeDesignation, string etat, string situation, string branche, string garantie, string departement, string evenement, double? latitude, double? longitude, double? diametre)
        {
            var result = new List<KGeolocCase>();
            var cmd = DataAccessManager.CmdWrapper;
            var typeDesignationRecherche = typeDesignation;

            //selection des adresses localisées qui ont une correspondance dans YPRTADR
            cmd.CommandText = string.Format(@"WITH AROUND AS (
                                                SELECT KGEOCHR, {0} 
                                                LATITUDE, LONGITUDE
                                                FROM KGEOLOC
                                                {1}
                                                {2}
                                              )
                                              ,SMPTABLE1 AS (SELECT PBIPB, PBALX, SMPE.MGSM1, MFIPB, MFALX, MGSMP, MGSMO, 
                                                        ROW_NUMBER() OVER(PARTITION BY PBIPB, PBALX ORDER BY MGEFA DESC, MGEFM DESC, MGEFJ DESC) AS RN
                                              FROM AROUND
                                              INNER JOIN YPOBASE 
	                                              ON YPOBASE.PBADH = KGEOCHR
                                              INNER JOIN YPOSMP SMP 
	                                              ON PBIPB = SMP.MFIPB AND PBALX = SMP.MFALX
                                              INNER JOIN YPOSMPE SMPE
	                                              ON CASE WHEN SMP.MFSMR <> 0 THEN SMP.MFSMR ELSE SMP.MFSMP END = SMPE.MGSMP AND SMPE.MGACT = 'O'
                                              )
                                              ,SMPTABLE AS (
	                                              SELECT PBIPB, PBALX, MGSM1
	                                              FROM SMPTABLE1
	                                              INNER JOIN YPORHMD HMD 
	                                              ON HMD.HDCHR = (SELECT HDCHR FROM YPORHMD WHERE SMPTABLE1.MFIPB = HDIPB AND SMPTABLE1.MFALX = HDALX AND SMPTABLE1.MGSMP = HDSMP AND SMPTABLE1.MGSMO = HDSMO AND HMD.HDTYP IN ('10', '30', '40') LIMIT 1)
	                                              AND HMD.HDORD = 1
                                                  WHERE RN = 1
                                              )
                                              ,SUMMARY AS (
                                                SELECT KGEOCHR, LONGITUDE, LATITUDE,
                                                       UNIONBASE.PBIPB, UNIONBASE.PBALX,PBREF,
                                                       PBTYP,PBAVN,PBTTR, PBBRA, PBSAJ, PBSAM, PBSAA, PBETA, PBSIT,
                                                       YCGest.TNNOM NOMCOURTIERGEST, YAGest.ANNOM NOMASSUREGEST,PBORK, MGSM1, {3} {0}
                                                       ROW_NUMBER() OVER(PARTITION BY UNIONBASE.PBIPB) AS RK
                                                FROM
	                                                ((SELECT PBIPB, PBALX, PBREF, PBTYP,PBAVN,PBTTR, PBBRA, PBSAJ, PBSAM, PBSAA, PBETA, PBSIT, PBORK, PBICT, PBIAS, AROUND.* FROM AROUND
	                                                INNER JOIN YPOBASE ON YPOBASE.PBADH = AROUND.KGEOCHR
	                                                ORDER BY YPOBASE.PBIPB)
	                                                UNION ALL
	                                                (SELECT PBIPB, PBALX, PBREF, PBTYP,PBAVN,PBTTR, PBBRA, PBSAJ, PBSAM, PBSAA, PBETA, PBSIT, PBORK, PBICT, PBIAS, AROUND.* FROM AROUND
	                                                INNER JOIN YPRTADR ON YPRTADR.JFADH = AROUND.KGEOCHR
	                                                INNER JOIN YPOBASE ON YPRTADR.JFIPB = YPOBASE.PBIPB AND YPRTADR.JFALX = YPOBASE.PBALX
	                                                ORDER BY YPOBASE.PBIPB)) UNIONBASE
                                                    {4}
                                                LEFT JOIN YCOURTN YCGest ON YCGest.TNICT = PBICT AND YCGest.TNXN5 = 0 AND YCGest.TNTNM = 'A'
                                                LEFT JOIN YASSNOM YAGest ON YAGest.ANIAS = PBIAS AND YAGest.ANTNM = 'A'
                                                LEFT JOIN SMPTABLE ON SMPTABLE.PBIPB = UNIONBASE.PBIPB AND SMPTABLE.PBALX = UNIONBASE.PBALX
                                              )
                                              SELECT * FROM SUMMARY "
                                              , SqlDepartementPartColumn, SqlDepartementPart, SqlAroundPart, SqlSinistrePartColumn, SqlSinistrePart);

            if (latitude != null && longitude != null && diametre != null)
            {
                double? latitudeMin = latitude - (diametre * 180 / (6371 * Math.PI));
                double? latitudeMax = latitude + (diametre * 180 / (6371 * Math.PI));
                double? longitudeMin = longitude - (diametre * 180 / (6371 * Math.PI * Math.Cos(latitude.Value * Math.PI / 180)));
                double? longitudeMax = longitude + (diametre * 180 / (6371 * Math.PI * Math.Cos(latitude.Value * Math.PI / 180)));
                cmd.CommandText = cmd.CommandText.Replace(SqlAroundPart,
                    @"WHERE LATITUDE BETWEEN :latmin AND :latmax
                    AND LONGITUDE BETWEEN :lonmin AND :lonmax");
                var paramLatMin = new EacParameter("latmin", DbType.Double) { Value = latitudeMin };
                var paramLatMax = new EacParameter("latmax", DbType.Double) { Value = latitudeMax };
                var paramLonMin = new EacParameter("lonmin", DbType.Double) { Value = longitudeMin };
                var paramLonMax = new EacParameter("lonmax", DbType.Double) { Value = longitudeMax };
                cmd.Parameters.Add(paramLatMin);
                cmd.Parameters.Add(paramLatMax);
                cmd.Parameters.Add(paramLonMin);
                cmd.Parameters.Add(paramLonMax);
            }
            else
            {
                cmd.CommandText = cmd.CommandText.Replace(SqlAroundPart, "");
            }

            cmd.Where(@"(LATITUDE <> 0 OR LONGITUDE <> 0) AND SUMMARY.RK = 1");
            if (!string.IsNullOrEmpty(code))
            {
                cmd.Where("PBIPB = :code");
                var param = new EacParameter("code", DbType.AnsiStringFixedLength) { Value = code.PadLeft(9, ' ') };

                cmd.Parameters.Add(param);
            }
            else
            {
                cmd.Where("PBORK <> 'KHV'");
                //cmd.Where("TRIM(PBIPB) NOT LIKE 'CV%'");
            }
            if (!string.IsNullOrEmpty(version))
            {
                cmd.Where("PBALX = :version");
                var param = new EacParameter("version", DbType.Int32) { Value = version };

                cmd.Parameters.Add(param);
            }
            if (!string.IsNullOrEmpty(designation))
            {
                cmd.Where("UPPER(PBREF) LIKE :designation");
                var param = new EacParameter("designation", DbType.AnsiStringFixedLength) { Value = string.Concat("%",designation.ToUpper(),"%") };

                cmd.Parameters.Add(param);
            }
                 
            if (!string.IsNullOrEmpty(garantie))
            {
                using (var webClient = new WebClient())
                {
                    webClient.Credentials = CredentialCache.DefaultCredentials;
                    string url = AppSettingsHelper.BIGarantieUrl;
                    foreach (string gar in garantie.Split(','))
                    {
                        url += $"&GARANTIE={gar.Replace("'", "")}";
                    }
                    string xml = webClient.DownloadString(url);
                    
                    XDocument xdoc = new XDocument();
                    xdoc = XDocument.Parse(xml);
                    var listPolice = xdoc.Descendants("{Rapport_Garantie_SIG}Détails").Select(x => new { Police = x.Attribute("Contrat_Code").Value, Aliment = x.Attribute("Aliment").Value }).ToList();

                    if (listPolice != null && listPolice.Count() > 0)
                    {
                        string text = "(PBIPB, PBALX) IN ( VALUES ";
                        listPolice.ForEach(x => { text += "('" + x.Police + "'," + x.Aliment + "),"; });
                        text = text.Substring(0, text.Length - 1);
                        text += ")";
                        cmd.Where(text);
                    }
                }
            }

            if (!string.IsNullOrEmpty(etat))
            {
                if (typeDesignation.Contains("X"))
                {
                    cmd.Where(string.Concat("SIETA IN ( ", etat, " )"));
                } else
                {
                    cmd.Where(string.Concat("PBETA IN ( ", etat, " )"));
                }
            }

            if (!string.IsNullOrEmpty(situation))
            {
                if (typeDesignation.Contains("X"))
                {
                    cmd.Where(string.Concat("SISIT IN ( ", situation, " )"));
                } else
                {
                    cmd.Where(string.Concat("PBSIT IN ( ", situation, " )"));
                }
            }

            if (!string.IsNullOrEmpty(branche))
            {
                cmd.Where(string.Concat("PBBRA IN ( ", branche, " )"));
            }

            if (!string.IsNullOrEmpty(departement))
            {
                cmd.CommandText = cmd.CommandText.Replace(SqlDepartementPartColumn, "ABPDP6,");
                cmd.CommandText = cmd.CommandText.Replace(SqlDepartementPart, "INNER JOIN YADRESS ON ABPCHR = KGEOCHR WHERE ABPDP6 IN ( " + departement + " )");
            }
            else
            {
                cmd.CommandText = cmd.CommandText.Replace(SqlDepartementPartColumn, "");
                cmd.CommandText = cmd.CommandText.Replace(SqlDepartementPart, "");
            }

            if (!string.IsNullOrEmpty(evenement))
            {
                if (typeDesignation.Contains("X"))
                {
                    cmd.Where(string.Concat("SIEVN IN ( ", evenement, " )"));
                }
            }

            if (!string.IsNullOrEmpty(typeDesignation))
            {
                if (typeDesignation.Contains("X"))
                {
                    typeDesignation = typeDesignation.Replace("X", "");
                    cmd.CommandText = cmd.CommandText.Replace(SqlSinistrePartColumn, "SIETA, SISIT, SIEVN,");
                    cmd.CommandText = cmd.CommandText.Replace(SqlSinistrePart,"INNER JOIN YSINIST ON SIIPB = UNIONBASE.PBIPB AND SIALX = UNIONBASE.PBALX");
                }
                else
                {
                    cmd.CommandText = cmd.CommandText.Replace(SqlSinistrePartColumn, "");
                    cmd.CommandText = cmd.CommandText.Replace(SqlSinistrePart, "");
                }

                typeDesignation = typeDesignation.Replace("PO", "").Replace("OP", "");

                if (!string.IsNullOrEmpty(typeDesignation))
                {
                    cmd.Where("PBTYP =:type");
                    var param = new EacParameter("type", DbType.AnsiStringFixedLength) { Value = typeDesignation };

                    cmd.Parameters.Add(param);
                }
                    
            }

            if (latitude == null || longitude == null || diametre == null)
                cmd.CommandText += "LIMIT 500";

            var dataTable = DataAccessManager.ExecuteDataTable(cmd);

            foreach (DataRow row in dataTable.Rows)
            {
                var KGEOLOCCase = new KGeolocCase(int.Parse(row["PBSAA"].ToString()), int.Parse(row["PBSAM"].ToString()), int.Parse(row["PBSAJ"].ToString()))
                {
                    NumeroChrono = int.Parse(row["KGEOCHR"].ToString()),
                    Lat = OutilsHelper.ToDouble(row["LATITUDE"].ToString()),
                    Lon = OutilsHelper.ToDouble(row["LONGITUDE"].ToString()),
                    NumContrat = row["PBIPB"].ToString(),
                    Version = Convert.ToInt32(row["PBALX"].ToString()),
                    Reference = row["PBREF"].ToString(),
                    Type = row["PBTYP"].ToString(),
                    NumInterneAvenant = (short)row["PBAVN"],
                    TypeTraitement = row["PBTTR"].ToString(),
                    Branche = row["PBBRA"].ToString(),
                    CourtierGestionnaire = row["NOMCOURTIERGEST"].ToString(),
                    AssureGestionnaire = row["NOMASSUREGEST"].ToString(),
                    Smp = row["MGSM1"].ToString(),
                    IsKheops = row["PBORK"].ToString() == "KHE",
                    TypeAtlas = typeDesignationRecherche,
                };
                result.Add(KGEOLOCCase);
            }
            return result;
        }
        public List<KGeolocCase>  GetAllCasesLatLonByPartner(int? code, TypePartner type)
        {
            var result = new List<KGeolocCase>();
            if (code!=null)
            {
                var cmd = DataAccessManager.CmdWrapper;

                cmd.CommandText = @"SELECT DISTINCT KGEOCHR, LONGITUDE, LATITUDE,
                                                     PBIPB,PBALX,PBREF,
                                                     PBTYP,PBAVN,PBTTR, PBBRA, PBSAJ, PBSAM, PBSAA, SIIPB,
                                                     YCGest.TNNOM NOMCOURTIERGEST, YAGest.ANNOM NOMASSUREGEST, PBORK
                                              FROM   YPOBASE 
                                                     INNER JOIN KGEOLOC ON KGEOLOC.KGEOCHR = YPOBASE.PBADH
                                                     LEFT JOIN YCOURTN YCGest ON YCGest.TNICT = PBICT AND YCGest.TNXN5 = 0 AND YCGest.TNTNM = 'A'
                                                     LEFT JOIN YASSNOM YAGest ON YAGest.ANIAS = PBIAS AND YAGest.ANTNM = 'A'
                                                     LEFT JOIN YSINIST ON (SIIPB , SIALX ) = (PBIPB , PBALX )";

                cmd.Where(@"(LATITUDE <> 0 OR LONGITUDE <> 0)");
                switch (type)
                {
                    case TypePartner.Assure:
                        cmd.Where("PBIAS = :code");
                        break;
                    case TypePartner.Courtier:
                        cmd.Where("PBICT = :code");
                        break;
                    default:
                        return result;
                }
                var param = new EacParameter("code", DbType.Int32) { Value = code };
                cmd.Parameters.Add(param);
                var dataTable = DataAccessManager.ExecuteDataTable(cmd);

                foreach (DataRow row in dataTable.Rows)
                {
                    var KGEOLOCCase = new KGeolocCase(int.Parse(row["PBSAA"].ToString()), int.Parse(row["PBSAM"].ToString()), int.Parse(row["PBSAJ"].ToString()))
                    {
                        NumeroChrono = int.Parse(row["KGEOCHR"].ToString()),
                        Lat = OutilsHelper.ToDouble(row["LATITUDE"].ToString()),
                        Lon = OutilsHelper.ToDouble(row["LONGITUDE"].ToString()),
                        NumContrat = row["PBIPB"].ToString(),
                        Version = Convert.ToInt32(row["PBALX"].ToString()),
                        Reference = row["PBREF"].ToString(),
                        Type = row["PBTYP"].ToString(),
                        NumInterneAvenant = (short)row["PBAVN"],
                        TypeTraitement = row["PBTTR"].ToString(),
                        Branche = row["PBBRA"].ToString(),
                        CourtierGestionnaire = row["NOMCOURTIERGEST"].ToString(),
                        AssureGestionnaire = row["NOMASSUREGEST"].ToString(),
                        IsKheops = row["PBORK"].ToString() == "KHE",
                        TypeAtlas = row["SIIPB"].ToString() != "" ? "X": row["PBTYP"].ToString()
                    };
                    result.Add(KGEOLOCCase);
                }
            }
            return result;
        }
        public KGeolocCase GetOfferContractLatLon(string NumContract, int NumeroChrono)
        {
            var cmd = DataAccessManager.CmdWrapper;
            //selection des adresses localisées qui ont une correspondance dans YPRTADR
            cmd.CommandText = string.Format(@"SELECT KGEOCHR, LONGITUDE, LATITUDE,
                                                     PBIPB,PBALX,PBREF, PBBRA,
                                                     PBTYP,PBAVN,PBTTR, PBSAJ, PBSAM, PBSAA,
                                                     YCGest.TNNOM NOMCOURTIERGEST, YAGest.ANNOM NOMASSUREGEST
                                              FROM   YPOBASE 
                                                     INNER JOIN KGEOLOC ON KGEOLOC.KGEOCHR = YPOBASE.PBADH
                                                     LEFT JOIN YCOURTN YCGest ON YCGest.TNICT = PBICT AND YCGest.TNXN5 = 0 AND YCGest.TNTNM = 'A'
                                                     LEFT JOIN YASSNOM YAGest ON YAGest.ANIAS = PBIAS AND YAGest.ANTNM = 'A'
                                              WHERE  (LATITUDE <> 0 OR LONGITUDE <> 0)
                                                     AND PBALX =0 AND TRIM(PBIPB) NOT LIKE 'CV%'
                                                     AND PBIPB='{0}' AND KGEOCHR={1}", NumContract.PadLeft(9,' '), NumeroChrono);

            var dataTable = DataAccessManager.ExecuteDataTable(cmd);
            if((dataTable == null) || (dataTable.Rows.Count == 0))
            {
                return null;
            }

            var row = dataTable.Rows[0];
            var KGEOLOCCase = new KGeolocCase(int.Parse(row["PBSAA"].ToString()), int.Parse(row["PBSAM"].ToString()), int.Parse(row["PBSAJ"].ToString()))
            {
                NumeroChrono = int.Parse(row["KGEOCHR"].ToString()),
                Lat = OutilsHelper.ToDouble(row["LATITUDE"].ToString()),
                Lon = OutilsHelper.ToDouble(row["LONGITUDE"].ToString()),
                NumContrat = row["PBIPB"].ToString(),
                Version = Convert.ToInt32(row["PBALX"].ToString()),
                Reference = row["PBREF"].ToString(),
                Type = row["PBTYP"].ToString(),
                NumInterneAvenant = (short)row["PBAVN"],
                TypeTraitement = row["PBTTR"].ToString(),
                Branche = row["PBBRA"].ToString(),
                CourtierGestionnaire = row["NOMCOURTIERGEST"].ToString(),
                AssureGestionnaire = row["NOMASSUREGEST"].ToString(),
            };
            return KGEOLOCCase;
        }

        public List<KGeolocPartner> GetAllPartnerLatLon(TypePartner typePartner, int? partnerCod, string partnerName, int? partnerDept, string partnerCP, string partnerTown, double? latitude, double? longitude, double? diametre)
        {
            var result = new List<KGeolocPartner>();
            var cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = GetCmdSqlRequest(typePartner, partnerCod, partnerName, partnerDept, partnerCP, partnerTown, latitude, longitude, diametre);

            var dataTable = DataAccessManager.ExecuteDataTable(cmd);
            foreach (DataRow row in dataTable.Rows)
            {
                var KGEOLOCPartner = new KGeolocPartner
                {
                    NumeroChrono = int.Parse(row["KGEOCHR"].ToString()),
                    Lat = OutilsHelper.ToDouble(row["LATITUDE"].ToString()),
                    Lon = OutilsHelper.ToDouble(row["LONGITUDE"].ToString()),
                    Num = row["NUM"].ToString(),
                    Nom = row["NOM"].ToString(),
                    Adresse = row["ADR"].ToString(),
                };
                result.Add(KGEOLOCPartner);
            }
            return result;
        }

        public List<Partner> GetPartners(int? code, string name, TypePartner type, int? orias)
        {
            var result = new List<Partner>();
            var cmd = DataAccessManager.CmdWrapper;
            var query = "";

            if (type == TypePartner.Assure)
            {
                query = string.Format(@"SELECT 
                                                     ANIAS AS CODE,
                                                     ANNOM AS NAME,           
                                                     CAST ( JSON_ARRAYAGG( ANNOM1 ) AS VARCHAR(5000) ) AS SECONDARYNAME,
                                                     ABPVI6 AS CITY,
                                                     ABPDP6 AS DP,
                                                     ABPCP6 AS CP
                                             FROM    YASSNOM  
                                                     LEFT JOIN ( SELECT ANIAS ANIAS1 , ANNOM ANNOM1  FROM YASSNOM WHERE ANINL = 0 AND ANTNM = 'S' ) SNOM ON ANIAS = ANIAS1 AND ANNOM1 <> ANNOM 
                                                     INNER JOIN YASSURE ON YASSURE.ASIAS = YASSNOM.ANIAS
                                                     INNER JOIN KGEOLOC ON KGEOLOC.KGEOCHR = YASSURE.ASADH
                                                     INNER JOIN YADRESS ON KGEOLOC.KGEOCHR = YADRESS.ABPCHR
                                             WHERE   (LATITUDE <> 0 OR LONGITUDE <> 0) AND ANINL = 0  AND ANTNM = 'A'");
                if (code != null)
                {
                    query += string.Format(@" AND YASSURE.ASIAS ={0}", code.Value);
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query += string.Format(@" AND UPPER(YASSNOM.ANNOM)  LIKE UPPER('{0}%')", name);
                }

                query += " GROUP BY ANIAS , ANNOM , ABPVI6 ,ABPDP6,ABPCP6";
            }
            else
            {
                var whereCondition="WHERE 1=1";
                if (code != null)
                {
                    whereCondition += string.Format(@" AND courtn1.TNICT ={0}", code.Value);
                }

                if (orias != null)
                {
                    whereCondition += string.Format(@" AND TCIOO ={0}", orias.Value);
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    whereCondition += string.Format(@" AND UPPER(courtn1.TNNOM)  LIKE UPPER('%{0}%')", name);
                }
                query = string.Format(@"SELECT  courtn1.TNICT AS CODE,
                                                courtn1.TNNOM AS NAME,
                                                CAST ( JSON_ARRAYAGG( courtn2.TNNOM ORDER BY courtn2.TNNOM  ) AS VARCHAR(5000) ) AS SECONDARYNAME,
	                                            TCIOO AS ORIAS,
                                                ABPVI6 AS CITY,
                                                ABPDP6 AS DP,
                                                ABPCP6 AS CP
                                       FROM   YCOURTI  
                                       LEFT JOIN YCOURTN courtn1 ON TCICT = courtn1.TNICT AND courtn1.TNXN5 = 0 AND courtn1.TNTNM = 'A' 
									   LEFT JOIN YCOURTN courtn2 ON TCICT = courtn2.TNICT AND courtn2.TNXN5 = 0 AND courtn2.TNTNM = 'S'
                                       INNER JOIN YADRESS ON YCOURTI.TCADH = YADRESS.ABPCHR
                                       {0}
                                       GROUP BY courtn1.TNICT,courtn1.TNNOM,TCIOO,ABPVI6,ABPDP6,ABPCP6", whereCondition);

            }
            cmd.CommandText = query;
            var dataTable = DataAccessManager.ExecuteDataTable(cmd);
            foreach (DataRow row in dataTable.Rows)
            {
                var partner = new Partner
                {
                    Code = IntOutilsHelper.ToNullableInt(row["CODE"].ToString()),
                    Name = row["NAME"].ToString(),
                    Orias = type == TypePartner.Courtier ? IntOutilsHelper.ToNullableInt(row["ORIAS"].ToString()) : null,
                    Type = type,
                    Address = new PartnerAdresse
                    {
                        City = row["CITY"].ToString(),
                        PostalCode = row["DP"].ToString() + row["CP"].ToString().PadLeft(3, '0')
                    }
                };
                if (!string.IsNullOrEmpty(row["SECONDARYNAME"].ToString()))
                {
                    partner.SecondaryName = ConverterHelper.ConcatExactJsonArray(row["SECONDARYNAME"].ToString(), " , ");
                }
                result.Add(partner);
            }
            return result;
        }

        public List<Interlocuteur> GetInterlocuteurs(int? code, string name)
        {
            var result = new List<Interlocuteur>();
            var cmd = DataAccessManager.CmdWrapper;
                cmd.CommandText = string.Format(@"SELECT  courtn1.TNICT AS CODE,
                                                courtn1.TNNOM AS NAME,
                                                courtn2.TNNOM AS NAMEINTERLOCUTEUR,
                                                TCIOO AS ORIAS,
	                                            ABPVI6 AS CITY,
                                                ABPDP6 AS DP,
                                                ABPCP6 AS CP
                                       FROM   YCOURTI  
                                       LEFT JOIN YCOURTN courtn1 ON TCICT = courtn1.TNICT AND courtn1.TNXN5 = 0 AND courtn1.TNTNM = 'A' 
									   INNER JOIN YCOURTN courtn2 ON TCICT = courtn2.TNICT AND courtn2.TNXN5 > 0 AND courtn2.TNTNM = 'A'
                                       INNER JOIN YADRESS ON YCOURTI.TCADH = YADRESS.ABPCHR");
            if (code != null)
            {
                cmd.Where(string.Format(@"courtn1.TNICT ={0}", code.Value));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {

                cmd.Where(string.Format(@"UPPER(courtn2.TNNOM)  LIKE UPPER('%{0}%')", name));
            }
            
            var dataTable = DataAccessManager.ExecuteDataTable(cmd);
            foreach (DataRow row in dataTable.Rows)
            {
                var interlocuteur = new Interlocuteur()
                {
                    CodeCourtier = IntOutilsHelper.ToNullableInt(row["CODE"].ToString()),
                    NameCourtier = row["NAME"].ToString(),
                    Name = row["NAMEINTERLOCUTEUR"].ToString(),
                    Orias = IntOutilsHelper.ToNullableInt(row["ORIAS"].ToString()),
                    Address = new PartnerAdresse
                    {
                        City = row["CITY"].ToString(),
                        PostalCode = row["DP"].ToString() + row["CP"].ToString().PadLeft(3, '0')
                    }
                };
                result.Add(interlocuteur);
            }
            return result;
        }

        public string GetCmdSqlRequest(TypePartner typePartner, int? partnerCod, string partnerName, int? partnerDept, string partnerCP, string partnerTown, double? latitude, double? longitude, double? diametre)
        {
            var cmd = String.Empty;
            if(typePartner == TypePartner.Assure)
            {
                //cmd = string.Format(@"SELECT KGEOLOC.KGEOCHR, LONGITUDE, LATITUDE,
                //                                     ANIAS AS NUM,
                //                                     ANNOM AS NOM,
                //                                     ABPNUM concat ' ' concat trim( ABPLG4) concat ' - ' concat ABPVI6 AS ADR
                //                             FROM    YASSNOM  
                //                                     INNER JOIN YASSURE ON YASSURE.ASIAS = YASSNOM.ANIAS
                //                                     INNER JOIN KGEOLOC ON KGEOLOC.KGEOCHR = YASSURE.ASADH
                //                                     INNER JOIN YADRESS ON KGEOLOC.KGEOCHR = YADRESS.ABPCHR
                //                             WHERE   (LATITUDE <> 0 OR LONGITUDE <> 0) AND ANINL = 0  AND ANTNM = 'A'");

                cmd = string.Format(@"WITH AROUND AS (
                                          SELECT *
                                          FROM KGEOLOC
                                          {0}
                                      )
                                      SELECT AROUND.KGEOCHR, LONGITUDE, LATITUDE,
                                             ANIAS AS NUM,
                                             ANNOM AS NOM,
                                             ABPNUM concat ' ' concat trim( ABPLG4) concat ' - ' concat ABPVI6 AS ADR
                                      FROM AROUND  
                                      INNER JOIN YASSURE ON AROUND.KGEOCHR = YASSURE.ASADH
                                      INNER JOIN YASSNOM ON YASSURE.ASIAS = YASSNOM.ANIAS
                                      INNER JOIN YADRESS ON AROUND.KGEOCHR = YADRESS.ABPCHR
                                      WHERE   (LATITUDE <> 0 OR LONGITUDE <> 0) AND ANINL = 0  AND ANTNM = 'A'", SqlAroundPart);

                if (partnerCod != null)
                {
                    cmd += string.Format(@" AND YASSURE.ASIAS ={0}", partnerCod.Value);
                }

                if (!string.IsNullOrWhiteSpace(partnerName))
                {
                    cmd += string.Format(@" AND UPPER(YASSNOM.ANNOM)  LIKE UPPER('%{0}%')", partnerName);
                }
            }
            else if (typePartner == TypePartner.Courtier)
            {
                cmd = string.Format(@"WITH AROUND AS (
                                          SELECT *
                                          FROM KGEOLOC
                                          {0}
                                      )
                                      SELECT  AROUND.KGEOCHR, LONGITUDE, LATITUDE,
	                                      TNICT AS NUM,
                                          YCOURTN.TNNOM AS NOM,
	                                      ABPNUM concat ' ' concat trim( ABPLG4) concat ' - ' concat ABPVI6 AS ADR
                                      FROM   AROUND  
                                      INNER JOIN YCOURTI ON AROUND.KGEOCHR = YCOURTI.TCADH
                                      INNER JOIN YCOURTN ON YCOURTN.TNICT = YCOURTI.TCICT
                                      INNER JOIN YADRESS ON AROUND.KGEOCHR = YADRESS.ABPCHR
                                      WHERE  LATITUDE <> 0 AND LONGITUDE <> 0 AND TNINL = 0  AND TNTNM = 'A'", SqlAroundPart);

                if (partnerCod != null)
                {
                    cmd+= string.Format(@" AND TNICT ={0}", partnerCod.Value);
                }

                if (!string.IsNullOrWhiteSpace(partnerName))
                {
                    cmd += string.Format(@" AND UPPER(YCOURTN.TNNOM)  LIKE UPPER('%{0}%')", partnerName);
                }
            } else
            {
                cmd = string.Format(@"WITH AROUND AS (
                                          SELECT *
                                          FROM KGEOLOC
                                          {0}
                                      )
                                      SELECT AROUND.KGEOCHR, LONGITUDE, LATITUDE,
                                        Y2.IMIIN AS NUM,
                                        Y2.IMNOM AS NOM,
                                        ABPNUM concat ' ' concat trim( ABPLG4) concat ' - ' concat ABPVI6 AS ADR
                                      FROM AROUND
                                      INNER JOIN YINTERV Y1 ON Y1.INADH = KGEOCHR
                                      INNER JOIN YADRESS ON Y1.INADH = YADRESS.ABPCHR
                                      INNER JOIN YINTNOM Y2 ON (Y1.INIIN, 'A', 0, 'EX') = (Y2.IMIIN, Y2.IMTNM, Y2.IMINL, Y2.IMTYI)
                                      WHERE  LATITUDE <> 0 AND LONGITUDE <> 0", SqlAroundPart);
            }

            if (partnerDept != null)
            {
                cmd += string.Format(@" AND YADRESS.ABPDP6 ='{0}'", partnerDept.Value);
            }

            if (!string.IsNullOrWhiteSpace(partnerCP) )
            {
                cmd += string.Format(@" AND ABPDP6 CONCAT LPAD(ABPCP6, 3, '0') = '{0}'", partnerCP);
            }

            if (!string.IsNullOrWhiteSpace(partnerTown))
            {
                cmd += string.Format(@" AND UPPER(REPLACE(ABPVI6,'-',' ')) LIKE UPPER('%{0}%')", partnerTown.Replace("-", " ").ToUpper());
            }

            if (latitude != null && longitude != null && diametre != null)
            {
                string latitudeMin = (latitude - (diametre * 180 / (6371 * Math.PI))).Value.ToString(CultureInfo.InvariantCulture);
                string latitudeMax = (latitude + (diametre * 180 / (6371 * Math.PI))).Value.ToString(CultureInfo.InvariantCulture);
                string longitudeMin = (longitude - (diametre * 180 / (6371 * Math.PI * Math.Cos(latitude.Value * Math.PI / 180)))).Value.ToString(CultureInfo.InvariantCulture);
                string longitudeMax = (longitude + (diametre * 180 / (6371 * Math.PI * Math.Cos(latitude.Value * Math.PI / 180)))).Value.ToString(CultureInfo.InvariantCulture);
                cmd = cmd.Replace(SqlAroundPart,
                    $@"WHERE LATITUDE BETWEEN {latitudeMin} AND {latitudeMax} 
                    AND LONGITUDE BETWEEN {longitudeMin} AND {longitudeMax}");
            }
            else
            {
                cmd = cmd.Replace(SqlAroundPart, "");
            }

            return cmd;
        }

        /// <summary>
        /// InsertIntoKGEOLOC
        /// </summary>
        /// <param name="KGEOLOC"></param>
        public void InsertIntoKGEOLOC(KGeoloc KGEOLOC)
        {
            var cmd = DataAccessManager.CmdWrapper;
            var input = new List<Param>
            {
                new Param(DbType.Int16, "KGEOCHR", KGEOLOC.NumeroChrono),
                new Param(DbType.Double, "LONGITUDE", KGEOLOC.Lon),
                new Param(DbType.Double, "LATITUDE", KGEOLOC.Lat),
                new Param(DbType.DateTime, "TSMAJ", KGEOLOC.DateModification)
            };
            cmd.InsertInto("KGEOLOC", input);
            DataAccessManager.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// InsertIntoKGEOLOC
        /// </summary>
        /// <param name="KGEOLOCList"></param>
        public void InsertIntoKGEOLOC(List<KGeoloc> KGEOLOCList)
        {
            foreach (var KGEOLOC in KGEOLOCList)
            {
                InsertIntoKGEOLOC(KGEOLOC);
            }
        }

        /// <summary>
        /// ClearKGEOLOCTable
        /// </summary>
        public void ClearKGEOLOCTable()
        {
            var cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = "DELETE FROM  KGEOLOC";
            DataAccessManager.ExecuteNonQuery(cmd);
        }

        public List<KGeolocCase> ParseActeGestionFromFile(string filePath)
        {
           var document = XPathGeolocValidation.ValidateFile(filePath);
            XmlSerializer serializer = new XmlSerializer(typeof(FileGeoloc));
            List<KGeolocCase> result = new List<KGeolocCase>();

            using (XmlReader reader = new XmlNodeReader(document))
            {
                var fileGeoloc = (FileGeoloc)serializer.Deserialize(reader);

                foreach (var marker in fileGeoloc.Markers)
                {
                    KGeolocCase markPoint = new KGeolocCase(0, 0, 0)
                    {
                        Branche = marker.ActeGestion.Branche,
                        NumContrat = marker.ActeGestion.NumContrat,
                        Version = marker.ActeGestion.Version,
                        Type = marker.ActeGestion.Type,
                        Reference = marker.ActeGestion.Reference,
                        DateSaisie = marker.ActeGestion.DateSaisie,
                        CourtierGestionnaire = marker.Gestionnaire,
                        Lat = marker.Point.Lat,
                        Lon = marker.Point.Lon,
                        IsExternal = true
                    };

                    result.Add(markPoint);
                }
            }

            return result;
        }
    }
}
