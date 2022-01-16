using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using DataAccess.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres.Assures;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.EasycomClient;
using System.IO;
using System.Linq;
using System.Net;

namespace OP.DataAccess {
    public class AssureRepository
    {

        public static AssureDto Initialiser(DataRow ligne, string prefixe = "")
        {
            AssureDto assure = null;
            if (OutilsHelper.ContientUnDesChampsEtEstNonNull(ligne, "ASIAS", "ANIAS", "PBIAS"))
            {
                assure = new AssureDto();
                assure.NomSecondaires = new List<string>();
                if (ligne.Table.Columns.Contains("ASIAS")) { assure.Code = ligne["ASIAS"].ToString(); }
                else if (ligne.Table.Columns.Contains("ANIAS")) { assure.Code = ligne["ANIAS"].ToString(); }
                else if (ligne.Table.Columns.Contains("PBIAS")) { assure.Code = ligne["PBIAS"].ToString(); }

                if (ligne.Table.Columns.Contains("ANNOM")) { assure.NomAssure = ligne["ANNOM"].ToString(); }
                else if (ligne.Table.Columns.Contains("ANNOM1")) { assure.NomAssure = ligne["ANNOM1"].ToString(); };

                if (ligne.Table.Columns.Contains("ANNOM2")) { assure.NomSecondaires.Add(ligne["ANNOM2"].ToString()); };
                if (ligne.Table.Columns.Contains("ASSIR") && ligne["ASSIR"] != DBNull.Value) { assure.Siren = Int32.Parse(ligne["ASSIR"].ToString()); };
                assure.Adresse = AdresseRepository.Initialiser(ligne, prefixe);
            }
            return assure;
        }

        public static AssureDto Obtenir(int assurerId)
        {
            AssureDto assure = null;
            string sql = string.Format(@"SELECT ASIAS CODEASSU, ANNOM NOMASSURE, ASADH, ABPLG3 BATIMENT, ABPCHR NUMEROCHRONO, ABPNUM NUMEROVOIE, ABPEXT EXTENSIONVOIE, ABPLG4 NOMVOIE,
                                       ABPLG5 BOITEPOSTALE, ABPDP6 DEPARTEMENT, ABPCP6 CODEPOSTAL, ABPVI6 NOMVILLE,
                                       ABPPAY CODEPAYS, ABPCDX TYPECEDEX, ASSIR SIREN, ASTEL TELEPHONEBUREAU
                                       FROM YASSURE
                                       LEFT JOIN YASSNOM ON ANIAS = ASIAS AND ANINL = 0 AND ANTNM = 'A' 
                                       LEFT JOIN YADRESS ON ASADH = ABPCHR  
                                       WHERE ASIAS = '{0}'", assurerId);
            var assurePlatDto = DbBase.Settings.ExecuteList<AssurePlatDto>(CommandType.Text, sql).FirstOrDefault();
            if (assurePlatDto != null)
            {
                assure = new AssureDto();
                assure.NomSecondaires = new List<string>();
                assure.Code = assurePlatDto.Code.ToString();
                assure.NomAssure = assurePlatDto.NomAssure;
                assure.Siren = assurePlatDto.Siren;
                assure.NomSecondaires.Add(!string.IsNullOrEmpty(assurePlatDto.NomSecondaire) ? assurePlatDto.NomSecondaire : string.Empty);
                assure.Adresse = GetAdresse(assurePlatDto);
                assure.TelephoneBureau = assurePlatDto.TelephoneBureau;
                assure.EstActif = GetAssureIsActif(assurePlatDto.Code);
            }
            return assure;
        }
        /// <summary>
        /// Récuperation code et nom de l'assureur
        /// </summary>
        /// <param name="assurerId"></param>
        /// <returns></returns>
        public static DtoCommon GetBaseInfoAssureur(int assurerId)
        {
            DtoCommon assure = null;
            string sql = string.Format(@"SELECT ASIAS CODEASSU, ANNOM NOMASSURE
                                       FROM YASSURE
                                       LEFT JOIN YASSNOM ON ANIAS = ASIAS AND ANINL = 0 AND ANTNM = 'A' 
                                       LEFT JOIN YADRESS ON ASADH = ABPCHR  
                                       WHERE ASIAS = '{0}'", assurerId);
            var assurePlatDto = DbBase.Settings.ExecuteList<AssurePlatDto>(CommandType.Text, sql).FirstOrDefault();
            if (assurePlatDto != null)
            {
                assure = new DtoCommon
                {
                    Code = assurePlatDto.Code.ToString(),
                    Libelle = assurePlatDto.NomAssure
                };
            }
            return assure;
        }

        public static List<AssureDto> Rechercher(int debut, int fin, string codeAssur, string nomAssure, string codePostal, string order, int by, bool modeAutocomplete)
        {
            string[] sortFields = { "CODEASSU", "NOMASSURE", "NOMSECONDAIRE", "CODEPOSTAL", "NOMVILLE", "SIREN" };
            List<AssureDto> result = new List<AssureDto>();
            string assureName = !string.IsNullOrEmpty(nomAssure) ? nomAssure.ToUpperInvariant().Trim().Replace("'", "''") : string.Empty;
            var assuresPlatDto = DbBase.Settings.ExecuteList<AssurePlatDto>(
                CommandType.StoredProcedure,
                "SP_RECHERCHEPRENEURASSURANCE",
                new [] {
                    new EacParameter("P_CODEASSUR", 0) { Value = int.TryParse(codeAssur, out int i) ? i : 0 },
                    new EacParameter("P_NOMASSUR", assureName.Replace("'", "''").ToUpperInvariant().Trim()),
                    new EacParameter("P_CP", codePostal ?? string.Empty),
                    new EacParameter("P_STARTLINE", DbType.Int32){ Value = debut },
                    new EacParameter("P_ENDLINE", DbType.Int32){ Value = modeAutocomplete ? assureName.Replace("'", "''").ToUpperInvariant().Trim().Length > 3 ? 100 : fin : fin },
                    new EacParameter("P_SORTINGBY", sortFields[by - 1]),
                    new EacParameter("P_ORDERBY", order),
                    new EacParameter("P_MODE", modeAutocomplete ? "AUTOCOMPLETE" : string.Empty)
                });

            foreach (var item in assuresPlatDto) {
                AssureDto assure = new AssureDto();
                assure.NomSecondaires = new List<string>();
                assure.Code = item.Code.ToString();
                assure.NomAssure = item.NomAssure;
                assure.Siren = item.Siren;
                if (item.NomSecondaire.ContainsChars()) {
                    assure.NomSecondaires.AddRange(JsonConvert.DeserializeObject<string[]>(item.NomSecondaire));
                }
                assure.Adresse = GetAdresse(item);
                assure.NbSinistres = item.NombreSinistres;
                assure.RetardsPaiements = item.Retards == 1;
                assure.Impayes = item.Impayes == 1;
                assure.EstActif = GetAssureIsActif(item.Code);
                result.Add(assure);
            }

            return result;
        }

        public static List<AssurePlatDto> RechercheTransversePreneurAssurance(string codePreneur, string nomPreneur, string cpPreneur)
        {
            const string sql = @"
SELECT ANIAS CODEASSU, ANNOM1 NOMASSURE, IFNULL(NULLIF(ANNOM, ANNOM1),'') NOMSECONDAIRE, ABPDP6 DEPARTEMENT, ABPCP6 CODEPOSTAL, ABPVI6 NOMVILLE, ASAD1, ASAD2, ASSIR SIREN , IFNULL(SINUM,0) SINUM
FROM YASSNOM 
LEFT JOIN (SELECT ANIAS ANIAS1, ANNOM ANNOM1 FROM YASSNOM WHERE ANINL = 0 AND ANTNM ='A') YASSNOM1 ON ANIAS1 = ANIAS 
LEFT JOIN YASSURE ON ASIAS = ANIAS 
LEFT JOIN YADRESS ON ASADH = ABPCHR 
LEFT JOIN YPOBASE ON PBIAS = ANIAS 
LEFT JOIN YSINIST ON (SIIPB , SIALX , SIAVN) = (PBIPB , PBALX , PBAVN) 
WHERE ANINL = 0 AND (ANTNM = 'A')";

            string query = sql;
            bool isFiltered = false;
            if (!string.IsNullOrEmpty(codePreneur) && !string.IsNullOrEmpty(codePreneur.Trim()))
            {
                query += string.Format(" AND ASIAS = {0}", codePreneur.Trim());
                isFiltered = true;
            }
            if (!string.IsNullOrEmpty(nomPreneur) && !string.IsNullOrEmpty(nomPreneur.Trim()))
            {
                query += string.Format(" AND TRIM(LOWER(ANNOM)) LIKE '%{0}%'", nomPreneur.Trim().ToLower().Replace("'", "''"));
                isFiltered = true;
            }
            if (!string.IsNullOrEmpty(cpPreneur) && !string.IsNullOrEmpty(cpPreneur.Trim()))
            {
                query += string.Format(" AND ABPCP6 = {0}", cpPreneur.Trim());
                isFiltered = true;
            }
            if (!isFiltered)//Si aucun filtre, aucun résultat
            {
                query += " AND 1 = 0";
            }
            query += " ORDER BY NOMASSURE FETCH FIRST 250 ROWS ONLY";

            return DbBase.Settings.ExecuteList<AssurePlatDto>(CommandType.Text, query);
        }

        public static int RechercherCount(AssureGetQueryDto query)
        {
            string sqlWhere = string.Empty;
            if (!string.IsNullOrEmpty(query.Code))
            {
                sqlWhere += " AND ANIAS1 = " + query.Code;
            }
            if (!string.IsNullOrEmpty(query.NomAssure))
            {
                string assureName = query.NomAssure.ToUpperInvariant().Trim().Replace("'", "''");
                sqlWhere += " AND (ANNOM LIKE '" + assureName.Replace("'", "''") + "%' OR ANNOM1 LIKE '" + assureName.Replace("'", "''") + "%')";
            }

            string sql = "SELECT COUNT(*) FROM ( " +
                              "SELECT  rownumber() OVER (ORDER BY ANNOM, ANORD) AS ID_NEXT, " +
                              "ANORD, ANIAS, ANNOM1, ANNOM, ABPDP6, ABPCP6, ABPVI6, ASAD1, ASAD2, ASSIR " +
                              "FROM YASSNOM " +
                              "LEFT JOIN (SELECT ANIAS ANIAS1, ANNOM ANNOM1 FROM YASSNOM WHERE ANINL = 0 AND ANTNM = 'A') YASSNOM1 ON ANIAS1 = ANIAS " +
                              "LEFT JOIN YASSURE ON ASIAS = ANIAS " +
                              "LEFT JOIN YADRESS ON ASADH = ABPCHR " +
                              "WHERE ANINL = 0 AND (ANTNM = 'A' OR ANTNM = 'S') " +
                              sqlWhere + ") AS TABLE";
            var result = DbBase.Settings.ExecuteScalar(CommandType.Text, sql);
            int count = 0;
            if (result != null) {
                int.TryParse(result.ToString(), out count);
            }
            return count;
        }

        public static int Count(AssureGetQueryDto query) {
            var result = DbBase.Settings.ExecuteScalar(
                CommandType.StoredProcedure,
                "SP_COUNT_PRENEURS_ASSURANCES",
                new[] {
                    new EacParameter("P_CODE", 0) { Value = int.TryParse(query.Code, out int i) ? i : 0 },
                    new EacParameter("P_NOM", query.NomAssure.Replace("'", "''").ToUpperInvariant().Trim()),
                    new EacParameter("P_CP", query.CodePostal ?? string.Empty)
                });

            return result.ToInteger().Value;
        }

        public static bool GetAssureIsActif(int code)
        {
            var username = ConfigurationManager.AppSettings.Get("grpUserName");
            var password = ConfigurationManager.AppSettings.Get("grpPassword");
            var grpServiceUrl = ConfigurationManager.AppSettings.Get("grpServiceUrl");
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(grpServiceUrl + $"accounts?$select=statecode&$filter=accountnumber eq '{code}' and customertypecode eq 7");
            // Set the Headers
            WebReq.Method = "GET";
            WebReq.Credentials = new NetworkCredential("userlab", "userlab");
            WebReq.Headers.Add("Cache-Control", "no-cache");
            WebReq.ContentType = "application/json; charset=utf-8";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
            // Get the Response data
            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);
            string jsontxt = _Answer.ReadToEnd();

            // convert json text to a pretty printout
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(jsontxt);
            var isActif = false;
            if (obj["value"].HasValues)
            {
                isActif = obj["value"][0]["statecode"].ToString() == "0";
            }
            return isActif;
        }

        #region méthodes privées
        private static AdressePlatDto GetAdresse(AssurePlatDto assurePlatDto)
        {
            AdressePlatDto adresse = null;
            if (assurePlatDto != null)
            {
                if (!string.IsNullOrEmpty(assurePlatDto.Departement) || !string.IsNullOrEmpty(assurePlatDto.NomVille))
                {
                    int cp = 0;
                    int cpCedex = 0;
                    adresse = new AdressePlatDto
                    {
                        Batiment = assurePlatDto.Batiment,
                        BoitePostale = assurePlatDto.BoitePostale,
                        ExtensionVoie = assurePlatDto.ExtensionVoie,
                        MatriculeHexavia = assurePlatDto.MatriculeHexavia,
                        NomVoie = assurePlatDto.NomVoie,
                        NumeroChrono = assurePlatDto.NumeroChrono,
                        NumeroVoie = assurePlatDto.NumeroVoie,
                        CodeInsee = assurePlatDto.CodeInsee,
                        CodePostal = Int32.TryParse(assurePlatDto.Departement + assurePlatDto.CodePostal.ToString().PadLeft(3, '0'), out cp) ? cp : cp,
                        CodePostalCedex = Int32.TryParse(assurePlatDto.Departement + assurePlatDto.CodePostalCedex.ToString().PadLeft(3, '0'), out cpCedex) ? cpCedex : cpCedex,
                        CodePostalString = assurePlatDto.Departement + assurePlatDto.CodePostal.ToString().PadLeft(3, '0'),
                        NomVille = assurePlatDto.NomVille,
                        NomCedex = assurePlatDto.NomCedex,
                        TypeCedex = assurePlatDto.TypeCedex,
                        CodePays = assurePlatDto.CodePays,
                        NomPays = assurePlatDto.NomPays
                    };

                }
            }
            return adresse;
        }
        #endregion
    }
}
