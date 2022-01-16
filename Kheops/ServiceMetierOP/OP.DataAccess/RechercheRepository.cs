using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EasycomClient;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;


namespace OP.DataAccess
{
    public class RechercheRepository
    {
        #region Méthodes Publiques

        public static long GetCountOffreByCode(string codeOffre, string version, string type)
        {
            EacParameter[] param = new EacParameter[] {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength)
                {
                    Value = codeOffre.ToUpper().PadLeft(9, ' ')
                },
                new EacParameter("version", DbType.AnsiStringFixedLength)
                {
                    Value = version
                },
                new EacParameter("type", DbType.AnsiStringFixedLength)
                {
                    Value = type
                }
            };

            string sql = @"SELECT COUNT(*) NBLIGN
                                         FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type AND PBORK = 'KHE'";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault();
            return result.NbLigne;
        }

        public static string GetBrancheOffreByCode(string codeOffre, string version, string type)
        {
            return GetBrancheOffre(codeOffre, version, type);
        }

        public static string GetCibleOffreByCode(string codeOffre, string version, string type)
        {
            return GetCibleOffre(codeOffre, version, type);
        }

        public static bool CheckBrancheOffres(string codeOffre, string version, string codeOffreCopy, string versionCopy, string type)
        {
            string brancheOffre = GetBrancheOffre(codeOffre, version, type);
            string brancheOffreCopy = GetBrancheOffre(codeOffreCopy, versionCopy, type);

            return brancheOffre == brancheOffreCopy;
        }

        public static bool CheckCibleOffres(string codeOffre, string version, string codeOffreCopy, string versionCopy, string type)
        {
            string cibleOffre = GetCibleOffre(codeOffre, version, type);
            string cibleOffreCopy = GetCibleOffre(codeOffreCopy, versionCopy, type);

            return cibleOffre == cibleOffreCopy;
        }

        public static string GetOffreMere(string codeContrat, string versionContrat)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength)
            {
                Value = codeContrat
            };
            param[1] = new EacParameter("versionContrat", DbType.Int32)
            {
                Value = Convert.ToInt32(versionContrat)
            };

            string sql = @"SELECT KFHIPB CONCAT '_' CONCAT KFHALX STRRETURNCOL, KHFSIT SITUATION FROM KPOFENT WHERE KFHPOG = :codeContrat AND KFHALG = :versionContrat";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0)
            {
                var resContrat = result.FirstOrDefault();
                if (resContrat.Situation == "A")
                {
                    return resContrat.StrReturnCol;
                }
            }

            return string.Empty;
        }


        public static string GetTypeAvenant(string paramOffre)
        {
            var codeOffre = paramOffre.Split('_')[0];
            var version = paramOffre.Split('_')[1];
            var type = paramOffre.Split('_')[2];
            var codeAvenant = paramOffre.Split('_')[3];

            EacParameter[] param = new EacParameter[] {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength)
                {
                    Value = codeOffre.PadLeft(9, ' ')
                },
                new EacParameter("version", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                },
                new EacParameter("type", DbType.AnsiStringFixedLength)
                {
                    Value = type
                },
                new EacParameter("codeAvenant", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(codeAvenant) ? Convert.ToInt32(codeAvenant) : 0
                }
            };

            string sql = @"SELECT PBTTR STRRETURNCOL FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvenant";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().StrReturnCol;
            }

            return string.Empty;
        }

        public static void ConfirmReprise(string codeOffre, string version, string type, string codeAvt, string typeAvt, string user)
        {
            DateTime dateNow = DateTime.Now;
            string libTraitement = string.Empty;
            string codeTraitement = string.Empty;

            switch (typeAvt)
            {
                case "AVNMD":
                    codeTraitement = AlbConstantesMetiers.TRAITEMENT_AVNMD;
                    libTraitement = AlbConstantesMetiers.LIBREPRISE_AVNMD;
                    break;
                case "AVNRG":
                    codeTraitement = AlbConstantesMetiers.TRAITEMENT_AVNRG;
                    libTraitement = AlbConstantesMetiers.LIBREPRISE_AVNRG;
                    break;
                case "REGUL":
                    codeTraitement = AlbConstantesMetiers.TRAITEMENT_REGUL;
                    libTraitement = AlbConstantesMetiers.LIBREPRISE_REGUL;
                    break;
                default:
                    codeTraitement = AlbConstantesMetiers.TRAITEMENT_AFFNV;
                    libTraitement = AlbConstantesMetiers.LIBREPRISE_AFFNV;
                    break;
            }

            EacParameter[] param = new EacParameter[] {
                new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength)
                {
                    Value = codeOffre.PadLeft(9, ' ')
                },
                new EacParameter("P_VERSION", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                },
                new EacParameter("P_TYPE", DbType.AnsiStringFixedLength)
                {
                    Value = type
                },
                new EacParameter("P_CODEAVN", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(codeAvt) ? Convert.ToInt32(codeAvt) : 0
                },
                new EacParameter("P_TYPEAVT", DbType.AnsiStringFixedLength)
                {
                    Value = typeAvt
                },
                new EacParameter("P_ETAT", DbType.AnsiStringFixedLength)
                {
                    Value = "N"
                },
                new EacParameter("P_USER", DbType.AnsiStringFixedLength)
                {
                    Value = user
                },
                new EacParameter("P_YEARNOW", DbType.Int32)
                {
                    Value = dateNow.Year
                },
                new EacParameter("P_MONTHNOW", DbType.Int32)
                {
                    Value = dateNow.Month
                },
                new EacParameter("P_DAYNOW", DbType.Int32)
                {
                    Value = dateNow.Day
                },
                new EacParameter("P_MINUTENOW", DbType.Int32)
                {
                    Value = AlbConvert.ConvertTimeToIntMinute(dateNow.TimeOfDay)
                },
                new EacParameter("P_CODETRAITEMENT", DbType.AnsiStringFixedLength)
                {
                    Value = codeTraitement
                },
                new EacParameter("P_LIBTRAITEMENT", DbType.AnsiStringFixedLength)
                {
                    Value = libTraitement
                },
                new EacParameter("P_ACTEGESTION", DbType.AnsiStringFixedLength)
                {
                    Value = AlbConstantesMetiers.ACTEGESTION_GESTION
                }
            };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CONFIRMREPRISE", param);
        }

        public static bool IsOffreCitrix(string codeAffaire, string version, string type)
        {
            string sql = string.Format("SELECT COUNT(*) NBLIGN FROM YPOBASE WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}' AND PBORK <> 'KHE'"
                , codeAffaire.PadLeft(9, ' '), version, type);
            return CommonRepository.ExistRow(sql);
            ;
        }

        public static string GetEtatOffre(string codeOffre, string version, string type)
        {
            EacParameter[] param = new EacParameter[] {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength)
                {
                    Value = codeOffre.PadLeft(9, ' ')
                },
                new EacParameter("version", DbType.AnsiStringFixedLength)
                {
                    Value = version
                },
                new EacParameter("type", DbType.AnsiStringFixedLength)
                {
                    Value = type
                }
            };

            var sql = @"SELECT PBETA STRRETURNCOL FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().StrReturnCol;
            }
            return string.Empty;
        }

        public static string CheckPrimeAvt(string codeContrat, string version, string type, string codeAvn)
        {
            EacParameter[] param = new EacParameter[] {
                new EacParameter("codeContrat", DbType.AnsiStringFixedLength)
                {
                    Value = codeContrat.PadLeft(9, ' ')
                },
                new EacParameter("version", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                },
                new EacParameter("codeAvn", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0
                }
            };

            string sql = "SELECT COUNT(*) NBLIGN FROM YPRIMES WHERE PKIPB = :codeContrat AND PKALX = :version AND PKAVN = :codeAvn AND PKSIT = 'S' AND PKOPE <> 20";

            if (CommonRepository.ExistRowParam(sql, param))
            {
                return "Des quittances soldées existent sur cet acte : <b>reprise impossible</b>";
            }

            return string.Empty;
        }

        public static bool HasSusp(string codeOffre, int? version, string type)
        {
            var dateLastYear = AlbConvert.ConvertDateToInt(DateTime.Now.AddYears(-1));
            EacParameter[] param = new EacParameter[] {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength)
                {
                    Value = codeOffre.PadLeft(9, ' ')
                },
                new EacParameter("version", DbType.Int32)
                {
                    Value = version
                },
                new EacParameter("type", DbType.AnsiStringFixedLength)
                {
                    Value = type
                },
                new EacParameter("dateLastYear", DbType.Int32)
                {
                    Value = dateLastYear
                }
            };

            string sql = "SELECT COUNT(*) NBLIGN FROM KPSUSP WHERE KICIPB = :codeOffre AND KICALX = :version AND KICTYP = :type AND KICTYE = 'S' " +
                         " AND KICFIND >= :dateLastYear";
            return CommonRepository.ExistRowParam(sql, param);
        }

        public static bool GetHasPrimeSoldee(string codeAffaire, int version, string type, int codeAvn)
        {
            long nbPrimes = 0;
            long nbPrimesReg = 0;

            string sqlPrimes = string.Format("SELECT COUNT(*) NBLIGN FROM YPRIMES WHERE PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {2} AND PKSIT = 'S' AND PKOPE <> 20",
                codeAffaire.PadLeft(9, ' '), version, codeAvn);
            var resPrimes = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlPrimes).FirstOrDefault();
            if (resPrimes != null)
            {
                nbPrimes = resPrimes.NbLigne;
            }

            string sqlPrimesReg = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPRIRES WHERE PKIPB = '{0}' AND PKALX = {1} AND PKAVN = {2} AND PKSIT = 'S' AND PKOPE <> 20",
                codeAffaire.PadLeft(9, ' '), version, codeAvn);
            var resPrimesReg = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlPrimesReg).FirstOrDefault();
            if (resPrimesReg != null)
            {
                nbPrimesReg = resPrimesReg.NbLigne;
            }

            return nbPrimes > 0 || nbPrimesReg > 0;
        }

        #endregion

        #region Méthodes Privées

        private static string GetBrancheOffre(string codeOffre, string version, string type)
        {
            string sql = string.Format(@"SELECT PBBRA BRANCHE
                                         FROM YPOBASE
                                         WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'", codeOffre.ToUpper().PadLeft(9, ' '),
                                                                                               version,
                                                                                               type);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().Branche;
            }
            return string.Empty;
        }

        private static string GetCibleOffre(string codeOffre, string version, string type)
        {
            string sql = string.Format(@"SELECT KABCIBLE CIBLE
                                         FROM KPRSQ
                                         WHERE KABIPB = '{0}' AND KABALX = {1} AND KABTYP = '{2}'", codeOffre.ToUpper().PadLeft(9, ' '),
                                                                                                   version,
                                                                                                   type);

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().Cible;
            }
            return string.Empty;
        }

        public static string DernierNumeroVersionOffreMotifSituation(string codeOffre, string type, string version)
        {
            string sql = string.Empty;
            string toReturn = string.Empty;
            object result = null;

            string lastVersion = "";

            sql = @"SELECT COUNT(*) NBLIGN FROM YPOBASE WHERE PBIPB = '" + codeOffre.PadLeft(9, ' ') + "' AND PBTYP = '" + type + "'";

            if (CommonRepository.ExistRow(sql))
            {
                sql = @"SELECT MAX(PBALX) FROM YPOBASE WHERE PBIPB = '" + codeOffre.PadLeft(9, ' ') + "' AND PBTYP = '" + type + "'";
                result = DbBase.Settings.ExecuteScalar(CommandType.Text, sql);
                if (result != null)
                {
                    toReturn = result.ToString();
                    lastVersion = result.ToString();
                }
            }

            sql = @"SELECT COUNT(*) NBLIGN FROM YPOBASE WHERE PBIPB = '" + codeOffre.PadLeft(9, ' ') + "' AND PBTYP = '" + type + "' AND PBALX = " + version;
            if (CommonRepository.ExistRow(sql))
            {
                sql = @"SELECT PBSTF FROM YPOBASE WHERE PBIPB = '" + codeOffre.PadLeft(9, ' ') + "' AND PBTYP = '" + type + "' AND PBALX = " + version;
                result = DbBase.Settings.ExecuteScalar(CommandType.Text, sql);

            }
            if (result != null)
            {
                toReturn += "_" + result.ToString();
            }

            if (!string.IsNullOrEmpty(lastVersion))
            {
                sql = @"SELECT PBETA FROM YPOBASE WHERE PBIPB = '" + codeOffre.PadLeft(9, ' ') + "' AND PBALX = " + lastVersion + " AND PBTYP = '" + type + "'";
                result = DbBase.Settings.ExecuteScalar(CommandType.Text, sql);
                if (result != null)
                {
                    toReturn += "_" + result.ToString();
                }
            }

            return toReturn;
        }

        private static void ReprisePrimes(string codeOffre, string version, string codeAvn)
        {
            EacParameter[] param = new EacParameter[] {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength)
                {
                    Value = codeOffre.PadLeft(9, ' ')
                },
                new EacParameter("version", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                },
                new EacParameter("codeAvn", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0
                }
            };

            string sql = @"INSERT INTO YWRTANP (WWIPB, WWALX, WWIPK)
                        SELECT PKIPB, PKALX, PKIPK FROM YPRIMES WHERE PKIPB = :codeOffre AND PKALX = :version AND PKAVN = :codeAvn
                                AND PKSIT NOT IN ('X', 'W') AND PKOPE <> 20";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static readonly Dictionary<string, string> ColumnAliases =
            typeof(OffreRechPlatDto).GetProperties()
            .Select(x => new { name = x.Name.ToUpperInvariant(), col = x.GetCustomAttributes<ColumnAttribute>().FirstOrDefault()?.Name.ToUpperInvariant() })
            .Where(x => x.col != null)
            .ToDictionary(x => x.name, x => x.col);

        private static readonly Dictionary<string, RechDataType> knownFields = new Dictionary<string, RechDataType>(StringComparer.InvariantCultureIgnoreCase)
        {
            ["PBIPB"] = RechDataType.String,
            ["PBALX"] = RechDataType.Int,
            ["PBTYP"] = RechDataType.String,
            ["PBBRA"] = RechDataType.String,
            ["KAACIBLE"] = RechDataType.String,
            ["PBIAS"] = RechDataType.Int,
            ["ASSIR"] = RechDataType.Int,
            ["ANNOM"] = RechDataType.String,
            ["ASCPO"] = RechDataType.String,
            ["ASDEP"] = RechDataType.String,
            ["ASVIL"] = RechDataType.String,
            ["ABPCP6"] = RechDataType.Int,
            ["ABPDP6"] = RechDataType.String,
            ["ABPVI6"] = RechDataType.String,
            ["PBSOU"] = RechDataType.String,
            ["PBGES"] = RechDataType.String,
            ["PBETA"] = RechDataType.String,
            ["PBSIT"] = RechDataType.String,
            ["$ADRPRINCIPALE"] = RechDataType.String,
            ["$MOTSCLE"] = RechDataType.UnquotedString,
            ["$DATESAISIE"] = RechDataType.Int,
            ["$DATECREATION"] = RechDataType.Int,
            ["$DATEMAJ"] = RechDataType.Int,
            ["$DATEEFFET"] = RechDataType.Int,
            ["$ORDERBY"] = RechDataType.Order,
            ["PBICT"] = RechDataType.Int,
            ["PBCTA"] = RechDataType.Int,

            ["(PBSAA * 10000 + PBSAM * 100 + PBSAJ)"] = RechDataType.Int,
            ["(PBTAA * 10000 + PBTAM * 100 + PBTAJ)"] = RechDataType.Int,
            ["(PBEFA * 10000 + PBEFM * 100 + PBEFJ)"] = RechDataType.Int,
            ["(PBMJA * 10000 + PBMJM * 100 + PBMJJ)"] = RechDataType.Int,
            ["(PBCRA * 10000 + PBCRM * 100 + PBCRJ)"] = RechDataType.Int,
            ["(PBSAA * 100000000 + PBSAM * 1000000 + PBSAJ * 10000 + PBSAH)"] = RechDataType.Int,
            ["(PBMJA * 100000000 + PBMJM * 1000000 + PBMJJ * 10000)"] = RechDataType.Int,

            ["ABPVI6"] = RechDataType.String,

            ["UPPER(TRIM(PBIPB) || ',' || PBALX)"] = RechDataType.String,

            ["CONCAT(ASDEP,ASCPO)"] = RechDataType.String,
            ["CONCAT(TCDEP,TCCPO)"] = RechDataType.String,
        };
        private static readonly Dictionary<string, string> AliasColumns = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            ["TYPEOFFRE"] = "PBTYP",
            ["CODEOFFRE"] = "PBIPB",
            ["VERSION"] = "PBALX",
            ["CODEAVN"] = "PBAVN",
            ["CODEBRANCHE"] = "PBBRA",
            ["TYPEPOLICE"] = "PBMER",
            ["CODEQUALITE"] = "PBSTQ",
            ["DESCRIPTIF"] = "PBREF",
            ["CODECIBLE"] = "KAACIBLE",
            ["LIBCIBLE"] = "KAHDESC",
            ["CODEETAT"] = "PBETA",
            ["CODEPERIODICITE"] = "PBPER",
            ["CODESIT"] = "PBSIT",
            ["AVNEXT"] = "PBAVK",
            ["HEUREFINEFFET"] = "PBFEH",
            ["CODEASS"] = "PBIAS",
            ["NOMASS"] = "ANNOM",
            ["VILASS"] = "ASVIL",
            ["CODECOURT"] = "PBICT",
            ["NOMCOURT"] = "TNNOM",
            ["VILCOURT"] = "TCVIL",
            ["STATUTKHEOPS"] = "PBORK",
            ["TYPETRAITEMENT"] = "PBTTR",
            ["TYPEACCORD"] = "PBTAC",

            ["LIBBRANCHE"] = "P1.TPLIL",
            ["LIBETAT"] = "P2.TPLIL",
            ["LIBSIT"] = "P3.TPLIL",

            ["CODEOFFREVERSION"] = "UPPER(TRIM(PBIPB) || ',' || PBALX)",

            ["DATESAISIE"] = "(PBSAA * 100000000 + PBSAM * 1000000 + PBSAJ * 10000 + PBSAH)",
            ["DATEMAJ"] = "(PBMJA * 100000000 + PBMJM * 1000000 + PBMJJ * 10000)",
            ["DATEFINEFFET"] = "(PBFEA * 10000 + PBFEM * 100 + PBFEJ)",

            ["$DATESAISIE"] = "(PBSAA * 10000 + PBSAM * 100 + PBSAJ)",
            ["$DATEACCORD"] = "(PBTAA * 10000 + PBTAM * 100 + PBTAJ)",
            ["$DATEEFFET"] = "(PBEFA * 10000 + PBEFM * 100 + PBEFJ)",
            ["$DATEMAJ"] = "(PBMJA * 10000 + PBMJM * 100 + PBMJJ)",
            ["$DATECREATION"] = "(PBCRA * 10000 + PBCRM * 100 + PBCRJ)",

            //["$ADRPRINCIPALE"] = "ABPVI6",

            ["CPASS"] = "CONCAT(ASDEP,ASCPO)",
            ["CPCOURT"] = "CONCAT(TCDEP,TCCPO)",

            ["CODECOURTAPPORT"] = "PBCTA"
        };

        private static readonly Dictionary<RechDataType, string> fieldValidationRegexps = new Dictionary<RechDataType, string>
        {
            [RechDataType.String] = @"^'([^']|'')+'",
            [RechDataType.UnquotedString] = @"([^']|'')",
            [RechDataType.Int] = @"\d+(\.\d+)",
            [RechDataType.Order] = @"(?!--)(?!;)(""[^""]+""|'[^']+'|)"
        };
        private class ReCache
        {
            private ReCache()
            {

            }
            private static readonly ReCache cache = new ReCache();

            public static ReCache Cache => cache;
            private static Dictionary<string, Regex> reCache = new Dictionary<string, Regex>();

            internal Regex this[string key]
            {
                get
                {
                    if (!reCache.ContainsKey(key))
                    {
                        reCache[key] = new Regex($@"^{key}$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    }
                    return reCache[key];
                }
            }
        }

        public static List<OffreRechPlatDto> RecherchePolices(IEnumerable<RechPolDto> WhereArgs, string TypeReq)
        {
            DateTime startTime = DateTime.Now;

            // Les critères sont présentés comme suit : PBBRA;=;'RS';AND/OR
            // Opérateurs : =, <>, >, >=, <, <=, IN
            var where = new StringBuilder();
            string OrderBy = "";
            var previousCriteria = new Stack<RechPolDto>();
            var previousOpenings = new Stack<string>();
            WhereArgs = WhereArgs.Where(x =>
                 (!WhereArgs.Any(y => y.Champ == "CodeAss") || x.Champ != "NomAss")
              && (!WhereArgs.Any(y => y.Champ == "CodeCourt") || x.Champ != "NomCourt")
            ).ToList();

            foreach (var critere in WhereArgs)
            {
                string express = "";
                // escape (double) non heading or trailing quotes

                /*
                 * ---------------- Traitement champs spéciaux ----------------
                 * $datesaisie : (PBSAA*10000 + PBSAM * 100 + PBSAJ) - Et critere.expression = 20170401 par exemple
                 * $dateaccord : (PBTAA*10000 + PBTAM * 100 + PBTAJ) - Et critere.expression = 20170401 par exemple
                 * $dateeffet : (PBEFA*10000 + PBEFM * 100 + PBEFJ) - Et critere.expression = 20170401 par exemple
                 * $datemja : (PBMJA*10000 + PBMJM * 100 + PBMJJ) - Et critere.expression = 20170401 par exemple
                 * $datecreation : (PBCRA*10000 + PBCRM * 100 + PBCRJ) - Et critere.expression = 20170401 par exemple
                 * $cpassure   : concat(ASDEP, ASCPO) = critere.expression
                 * $cpcourtier : concat(TCDEP, TCCPO) = critere.expression
                 * $motscle    : concat(TCDEP, TCCPO) = critere.expression
                 * $adrprincipale : ABPVI6
                 * $cpadrprincipale : ABPDP6 et ABPCP6
                 * $order : ordre
                 */
                //var fieldValidationRegexps = new Dictionary<RechDataType, string> {
                //    [RechDataType.String] = @"([^']|'')*", // @"'([^']|'')+'",
                //    [RechDataType.UnquotedString] = @"([^']|'')*",
                //    [RechDataType.Int] = @"\d+(\.\d+)?",
                //    [RechDataType.Order] = @"(""[^""]+""|'[^']+'|(?!--)(?!;).)*"
                //};
                string fieldOrig = critere.Champ;
                string field = ResolveField(critere);
                var fieldType = knownFields.ContainsKey(field) ? knownFields[field] : RechDataType.String;
                var re = fieldValidationRegexps[fieldType];
                Type effectiveType = null;
                Type[] paramtypes = new Type[] { };
                Type[] types = critere.GetType().GetGenericArguments();
                if (types.Length > 0)
                {
                    effectiveType = types[0];
                    paramtypes = effectiveType.GetGenericArguments();
                }
                var expression = critere.Expression as string;

                if (field.ToUpperInvariant() != "$ORDERBY")
                {
                    switch (critere.Operateur.ToUpper())
                    {
                        case "=":
                        case "<>":
                        case "<":
                        case "<=":
                        case ">":
                        case ">=":
                        case "LIKE":
                            if (fieldType == RechDataType.String && effectiveType != typeof(string))
                            {
                                goto case "erreur";
                            }
                            if (fieldType == RechDataType.Int && !IsNum(effectiveType))
                            {
                                goto case "erreur";
                            }
                            break;
                        case "NOT IN":
                        case "IN":
                            if (fieldType == RechDataType.String && !typeof(IEnumerable<string>).IsAssignableFrom(effectiveType))
                            {
                                goto case "erreur";
                            }
                            if (fieldType == RechDataType.Int && !typeof(IEnumerable<long>).IsAssignableFrom(effectiveType))
                            {
                                goto case "erreur";
                            }
                            break;
                        case "ENTRE":
                            if (fieldType == RechDataType.String && !typeof((string, string)).IsAssignableFrom(effectiveType))
                            {
                                goto case "erreur";
                            }
                            if (fieldType == RechDataType.Int && !typeof((long, long)).IsAssignableFrom(effectiveType))
                            {
                                goto case "erreur";
                            }
                            break;
                        case "(":
                        case ")":
                            break;
                        case "erreur":
                            throw new Exception($"Format invalide pour {fieldOrig}");
                        default:
                            throw new Exception($"Opérateur {critere.Operateur} Invalide");
                    }
                }
                else // if (field.ToUpperInvariant() == "$ORDERBY")
                {
                    var orders = expression.Split(',');
                    foreach (var order in orders)
                    {
                        const string numTimesField = @"(\s*\d+(\.\d+)?\s*\*\s*\w+\s*\+\s*)";
                        const string fieldTimesNum = @"(\s*\w+\s*\*\s*\d+(\.\d+)?\s*\+\s*)";
                        if (ReCache.Cache[@"\w+(\s+(A|DE)SC)?"].IsMatch(order.Trim()))
                        {
                            continue;
                        }
                        else if (!ReCache.Cache[$@"\(({numTimesField}|{fieldTimesNum})+\s*\w+\s*\)(\s+(A|DE)SC)?"].IsMatch(order.Trim()))
                        {
                            throw new Exception("Unsupported order");
                        }
                    }
                }
                bool processOperator = true;

                string expressionEscaped = expression?.Replace("'", "''");

                switch (critere.Champ.ToUpperInvariant())
                {
                    case "$MOTSCLE": // pbref kajobsv
                        expressionEscaped = expressionEscaped.ToUpper();
                        express = $@"({
                            string.Join(" OR ",
                                new[] { "PBMO1", "PBMO2", "PBMO3", "PBREF", "KAJOBSV" }.
                                Select(column => $"UPPER({column}) {critere.Operateur} '{expressionEscaped}'")
                            )
                        })";
                        processOperator = false;
                        break;
                    case "$ADRPRINCIPALE": // pbref kajobsv
                        expressionEscaped = expressionEscaped.ToUpper();
                        express = $@"({
                            string.Join(" OR ",
                                new[] { "ABPL4F", "ABPL6F", "ABPVI6", "ABPVIX" }.
                                Select(column => $"TRIM(UPPER({column})) {critere.Operateur} '{expressionEscaped}'")
                            )
                        })";
                        processOperator = false;
                        break;
                    case "$ORDERBY":
                        OrderBy = expression;
                        processOperator = false;
                        break;
                    default:
                        break;
                }

                if (processOperator)
                {
                    switch (critere.Operateur.ToUpper())
                    {
                        case "ENTRE":
                            // opérateur = entre => critere.expression = 20170401;20170430 par exemple
                            if (fieldType == RechDataType.Int)
                            {
                                var (a, b) = ((RechPolDto<(long, long)>)critere).Expression;
                                express = "(" + critere.Champ + ">= " + a + " AND " + critere.Champ + " <= " + b + ")";
                            }
                            else
                            {
                                var (a, b) = ((RechPolDto<(string, string)>)critere).Expression;
                                express = "(" + critere.Champ + ">= " + a.Replace("'", "''") + "' AND " + critere.Champ + " <= '" + b.Replace("'", "''") + "')";
                            }
                            break;

                        case "LIKE":
                            int p = expressionEscaped.IndexOf("%");
                            if (p < 0)
                            {
                                expressionEscaped = expressionEscaped + "%";
                            }
                            goto case "general";

                        case "=":
                        case "<>":
                        case "<":
                        case "<=":
                        case ">":
                        case ">=":
                        case "general":
                            if (fieldType != RechDataType.Int)
                            {
                                expressionEscaped = "'" + expressionEscaped + "'";
                            }
                            else
                            {
                                expressionEscaped = critere.Expression.ToString();
                            }
                            goto case "compute";

                        case "NOT IN":
                        case "IN":
                            if (fieldType == RechDataType.Int)
                            {
                                var exp = ((IEnumerable<long>)critere.Expression);
                                expressionEscaped = $"({String.Join(", ", exp.Select(x => x.ToString()))})";
                            }
                            else
                            {
                                var exp = ((IEnumerable<string>)critere.Expression);
                                expressionEscaped = $"({String.Join(", ", exp.Select(x => "'" + x.Replace("'", "''") + "'"))})";
                            }
                            goto case "compute";
                        case "compute":
                            express = critere.Champ + " " + critere.Operateur + " " + expressionEscaped;
                            break;
                    }
                }
                if (critere.Champ.ToLower() != "$orderby")
                {
                    var previousAsGrouping = previousCriteria.Any() ? (previousCriteria.Peek() as GroupingRechPolDto) : null;
                    bool isNewGroup = previousAsGrouping?.Operateur == "(";
                    var link = isNewGroup ? "" : critere.Liaison.ToUpper();

                    if (critere is GroupingRechPolDto)
                    {
                        if (critere.Operateur == ")")
                        {
                            if (previousOpenings.Count == 0)
                            {
                                throw new Exception("Impossible de fermer le groupe de critère, pas de groupe ouvert");
                            }
                            if (previousAsGrouping?.Operateur == "(")
                            {
                                previousCriteria.Pop();
                                where.Length -= previousOpenings.Pop().Length;
                            }
                            else
                            {
                                where.Append(" ) ");
                            }

                        }
                        else if (critere.Operateur == "(")
                        {
                            var opening = $" {link} (";
                            previousOpenings.Push(opening);
                            where.Append(opening);
                        }
                    }
                    else
                    {
                        where.AppendFormat(" {0} {1}", link, express);
                    }

                }
                previousCriteria.Push(critere);
            }

            previousCriteria = null;
            previousOpenings = null;

            var finalWhere = " WHERE PBORK <> 'KVS' AND PBIPB NOT LIKE 'SP_%'" + where.ToString();

            List<OffreRechPlatDto> Lpol = RecherchePolices(finalWhere, OrderBy, TypeReq);
            var ListSusp = RechercheSuspensions(Lpol.Select(x => x.CodeOffre)).ToDictionary(x => (x.codeOffre, x.aliment));

            Lpol.ForEach(pol =>
            {

                pol.NomAss = CleanInvalidXmlChars(pol.NomAss);
                pol.NomCourt = CleanInvalidXmlChars(pol.NomCourt);
                pol.VilleAss = CleanInvalidXmlChars(pol.VilleAss);
                pol.VilleCourt = CleanInvalidXmlChars(pol.VilleCourt);
                pol.Descriptif = CleanInvalidXmlChars(pol.Descriptif);
                if (pol.HeureFinEffet == 0)
                {
                    pol.HeureFinEffet = 2359;
                }

                // Rechercher des suspensions d'après IN
                ListSusp.TryGetValue((pol.CodeOffre, pol.Version), out var SuspPol);
                if (SuspPol != default)
                {
                    pol.HasSusp = 1;
                    pol.DtFinSusp = SuspPol.finSusp;
                }
                else
                {
                    pol.HasSusp = 0;
                    pol.DtFinSusp = -1;
                }
            });

            return Lpol;

        }

        private static bool IsNum(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        private static string ResolveField(RechPolDto critere)
        {
            var field = critere.Champ.ToUpperInvariant();
            if (critere is GroupingRechPolDto)
                return field;
            if (ColumnAliases.ContainsKey(field))
            {
                field = ColumnAliases[field];
            }
            if (AliasColumns.ContainsKey(field))
            {
                field = AliasColumns[field];
            }

            if (knownFields.ContainsKey(field))
            {
                critere.Champ = field;
            }
            else
            {
                throw new Exception($"Champ {critere.Champ} non supporté");
            }

            return field;
        }

        private static string StripQuote(string expressionEscaped)
        {
            expressionEscaped = Regex.Replace(expressionEscaped, "^'|'$", "", RegexOptions.Singleline);
            return expressionEscaped;
        }

        public static string CleanInvalidXmlChars(string text)
        {
            // From xml spec valid chars:
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF.
            string re = @"[^\x09\0x1A\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-u10FFFF]";
            return Regex.Replace(text, re, " ");
        }

        public static List<OffreRechPlatDto> RecherchePolices(string WhereArgs, string OrderBy, string Typereq, int count = 200, int offset = 0)
        {
            var order = OrderBy;
            if (string.IsNullOrWhiteSpace(order))
            {
                //default order
                order = "(PBSAA * 10000 + PBSAM * 100 + PBSAJ) DESC";
            }
            // prefixes
            var po = Typereq != "H" ? "PO" : "HP";
            var kp = Typereq != "H" ? "KP" : "HP";

            string sql =
$@"SELECT
    PBTYP AS TYPEOFFRE,
    PBIPB AS CODEOFFRE,
    PBALX AS VERSION,
    PBAVN AS CODEAVN,
    PBBRA AS CODEBRANCHE,
    PBSBR AS CODESOUSBRANCHE,
    PBMER AS TYPEPOLICE,
    (PBSAA*100000000 + PBSAM * 1000000 + PBSAJ * 10000 + PBSAH) AS DATESAISIE,
    PBSTQ AS CODEQUALITE,
    P4.TPLIL AS LIBQUALITE,
    PBREF AS DESCRIPTIF,
    UPPER(TRIM(TRANSLATE(PBREF,'aaaaAAAAAeeeeeEEEEEiiiiiIIIIIooooOOOOOuuuuUUUU','âãäåÁÂÃÄÅèééêëÈÉÉÊËìíîïìÌÍÎÏÌóôõöÒÓÔÕÖùúûüÙÚÛÜ'))) AS DESCRIPTIFSORT,
    P1.TPLIL AS LIBBRANCHE,
    KAACIBLE AS CODECIBLE,
    KAHDESC AS LIBCIBLE,
    PBCAT AS CODECATEGORIE,
    (PBMJA * 100000000 + PBMJM * 1000000 + PBMJJ * 10000) AS DATEMAJ,
    (PBCRA * 10000+ PBCRM * 100 + PBCRJ) AS DATECREATION,
    (PBEFA * 10000+ PBEFM * 100 + PBEFJ) AS DATEEFFET,
    PBETA AS CODEETAT,
    P2.TPLIL AS LIBETAT,
    PBPER AS CODEPERIODICITE,
    PBSIT AS CODESIT,
    P3.TPLIL AS LIBSIT,
    PBAVK AS AVNEXT,
    (PBFEA * 10000 + PBFEM * 100 + PBFEJ) AS DATEFINEFFET,
    PBFEH AS HEUREFINEFFET,
    PBIAS AS CODEASS,
    ANNOM AS NOMASS,
    CONCAT(ASDEP,ASCPO) AS CPASS,
    ASVIL AS VILASS,
    PBCTA AS CODECOURTAPPORT,
    PBICT AS CODECOURT,
    TNNOM AS NOMCOURT,
    CONCAT(TCDEP,TCCPO) AS CPCOURT,
    TCVIL AS VILCOURT,
    PBORK AS STATUTKHEOPS,
    PBTTR AS TYPETRAITEMENT,
    PBTAC AS TYPEACCORD,
    DOUBLESAISIE,
    MAXIDREGUL,
    TRTLOT
FROM Y{po}BASE
LEFT JOIN YYYYPAR P1 ON P1.TCON = 'GENER' AND P1.TFAM = 'BRSBR' AND P1.TCOD = PBBRA
LEFT JOIN YYYYPAR P2 ON P2.TCON = 'PRODU' AND P2.TFAM = 'PBETA' AND P2.TCOD = PBETA
LEFT JOIN YYYYPAR P3 ON P3.TCON = 'PRODU' AND P3.TFAM = 'PBSIT' AND P3.TCOD = PBSIT
LEFT JOIN YYYYPAR P4 ON P4.TCON = 'PRODU' AND P4.TFAM = 'PBSTQ' AND P4.TCOD = PBSTQ
LEFT JOIN {kp}ENT ON KAATYP = PBTYP AND KAAIPB = PBIPB AND KAAALX = PBALX {(kp.ToUpper() == "HP" ? "AND KAAAVN = PBAVN " : string.Empty)}
LEFT JOIN {kp}OBSV ON KAJCHR = KAAOBSV {(kp.ToUpper() == "HP" ? "AND KAJAVN = KAAAVN " : string.Empty)}
LEFT JOIN KCIBLE ON KAHCIBLE = KAACIBLE
LEFT JOIN YASSNOM ON ANIAS = PBIAS AND ANINL = 0 AND ANORD = 0
LEFT JOIN YASSURE ON ASIAS = PBIAS
LEFT JOIN YCOURTI ON TCICT = PBICT
LEFT JOIN YCOURTN ON TNICT = PBICT AND TNXN5 = 0 AND TNORD = 0
LEFT JOIN YADRESS ON ABPCHR = PBADH
LEFT JOIN (
    SELECT KAFTYP, KAFIPB, KAFALX, 'O' AS DOUBLESAISIE
    FROM KPODBLS
    GROUP BY KAFTYP, KAFIPB, KAFALX HAVING COUNT(*) > 1
) AS DOUBLETABLE ON
     DOUBLETABLE.KAFTYP = PBTYP AND DOUBLETABLE.KAFIPB = PBIPB AND DOUBLETABLE.KAFALX = PBALX
LEFT JOIN (
    SELECT KHWTYP, KHWIPB, KHWALX, MAX(KHWID) AS MAXIDREGUL
    FROM KPRGU
    GROUP BY KHWTYP, KHWIPB, KHWALX
) AS LASIDREGUL ON
    LASIDREGUL.KHWTYP = PBTYP AND LASIDREGUL.KHWIPB = PBIPB AND LASIDREGUL.KHWALX = PBALX
LEFT JOIN(
    SELECT KELTYP, KELIPB, KELALX, KELSIT AS TRTLOT
    FROM  KPDOCLW
    GROUP BY KELTYP, KELIPB, KELALX, KELSIT
) AS LOT ON
    LOT.KELTYP = PBTYP AND LOT.KELIPB = PBIPB
AND LOT.KELALX = PBALX  AND TRTLOT = 'W'
{WhereArgs}
ORDER BY { order }
OFFSET {offset} ROWS
FETCH FIRST {count} ROWS ONLY
WITH NC
";
            return DbBase.Settings.ExecuteList<OffreRechPlatDto>(CommandType.Text, sql);
        }
        public static List<(string codeOffre, int aliment, int finSusp)> RechercheSuspensions(IEnumerable<string> polices)
        {
            if (!polices.Any())
            {
                return new List<(string codeOffre, int aliment, int finSusp)>();
            }
            DateTime now = DateTime.Now;
            int dtstr = (now.Year - 1) * 10000 + now.Month * 100 + now.Day;
            string sql =
$@"SELECT KICIPB codeOffre, KICALX aliment, MAX(KICFIND) finSusp FROM KPSUSP
WHERE KICIPB IN ({string.Join(", ", polices.Select(x => $"'{x}'"))})
AND KICTYE = 'S' AND KICSIT <> 'X'
GROUP BY KICIPB, KICALX";

            return DbBase.Settings.ExecuteList<(string codeOffre, int aliment, int finSusp)>(CommandType.Text, sql);
        }

        #endregion

        private enum RechDataType
        {
            Int,
            String,
            UnquotedString,
            Order,
        }
    }
}
