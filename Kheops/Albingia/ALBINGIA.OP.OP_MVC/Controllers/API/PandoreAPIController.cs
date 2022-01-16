using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Models.Common;
using ALBINGIA.Framework.Common.Tools;
using Aspose.Cells;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace ALBINGIA.OP.OP_MVC.Controllers.API
{
    public class PandoreAPIController : ApiController
    {
        static string gatewayUrl = ConfigurationManager.AppSettings["RefinitivUrl"];//"/v2/";
        static string gatewayHost = ConfigurationManager.AppSettings["RefinitivHost"];//"rms-world-check-one-api-pilot.thomsonreuters.com";

        static string apiKey = ConfigurationManager.AppSettings["APIKey"];//"8ac85b73-1964-450a-9b8b-35a444f7b5fd";
        static string apiSecret = ConfigurationManager.AppSettings["APISecret"];//"fYxa/lKwikRF27TWH8M6L6/WniKpc2K1DXhRw4yYpkNLkh/6uzeMPXeHIHGBAMeoxNifEq6n76SVQ7Mb5pWcWg==";

        static string requestEndPoint = ConfigurationManager.AppSettings["RefinitivEndpoint"];//"https://rms-world-check-one-api-pilot.thomsonreuters.com/v2/";

        static DateTime dateValue;
        static string date, dataToSign, hmac, authorisation;
        static byte[] byte1;

        static List<RefinitivListObject> statusesList;

        #region public methods
        [HttpGet]
        public IHttpActionResult CheckPartenaire(string caseNumber, string caseName, string type)
        {
            try
            {
                getResolution();
                string caseId = getGrpAccountId(caseNumber, type);
                //Vérifie si les paramètres fonctionnent
                //checkGroups();

                //Vérification de l'existance du partenaire chez Refinitiv
                var caseExist = checkAvailibility(caseId);

                string caseSystemId = "";
                bool hasResults = false;

                if (caseExist)
                {
                    //Si le partenaire existe => récupération de l'ID chez Refinitiv
                    caseSystemId = getSystemId(caseId);
                    //Récupération des résultats chez Refinitiv
                    hasResults = getScreeningResults(caseSystemId);
                }
                else
                {
                    //Si le partenaire n'existe pas => création de la recherche et ID chez Refinitiv
                    RefinitivObject refobj = getCases(caseId, caseName);
                    caseSystemId = refobj.CaseSystemId;
                    hasResults = refobj.HasResults;
                    //Assurer un suivi des recherches
                    setOnGoing(refobj.CaseSystemId);
                    if (hasResults)
                    {
                        var user = AlbSessionHelper.ConnectedUser;
                        // Envoyer mail (destinataire a définir)
                        MailModel mail = AlbOpConstants.MainInfoContent;
                        mail.Body = $"Un nouveau case a été créé pour le Courtier/Assuré {caseName} ({caseNumber}), et celui-ci contient des résultats positifs.";
                        mail.Subject = "Test Refinitiv";
                        mail.To = "jean-luc.weng@albingia.fr;erwan.coulm@albingia.fr";
                        new AlbMailing().SendMail(mail);
                    }
                }
                var retour = new
                {
                    status = "200",
                    result = hasResults,
                    message = $"{(caseExist ? "" : "nouvelle")}recherche sur le partenaire {caseName} ({caseNumber})"
                };
                return Ok(retour);
            }
            catch (Exception ex)
            {
                var retour = new
                {
                    status = "500",
                    result = "",
                    message = $"Erreur : {ex.Message}"
                };
                return Ok(retour);
            }

        }
        [HttpGet]
        public IHttpActionResult CheckPartenaire(string caseId, string caseName)
        {
            try
            {
                getResolution();

                //Vérification de l'existance du partenaire chez Refinitiv
                var caseExist = checkAvailibility(caseId);

                string caseSystemId = "";
                bool hasResults = false;

                if (caseExist)
                {
                    //Si le partenaire existe => récupération de l'ID chez Refinitiv
                    caseSystemId = getSystemId(caseId);
                    //Récupération des résultats chez Refinitiv
                    hasResults = getScreeningResults(caseSystemId);
                }
                else
                {
                    //Si le partenaire n'existe pas => création de la recherche et ID chez Refinitiv
                    RefinitivObject refobj = getCases(caseId, caseName);
                    caseSystemId = refobj.CaseSystemId;
                    hasResults = refobj.HasResults;
                    //Assurer un suivi des recherches
                    setOnGoing(refobj.CaseSystemId);
                    if (hasResults)
                    {
                        var user = AlbSessionHelper.ConnectedUser;
                        // Envoyer mail (destinataire a définir)
                        MailModel mail = AlbOpConstants.MainInfoContent;
                        mail.Body = $"Un nouveau case a été créé pour le partenaire {caseName}, et celui-ci contient des résultats positifs.";
                        mail.Subject = "Test Refinitiv";
                        mail.To = "jean-luc.weng@albingia.fr;erwan.coulm@albingia.fr";
                        new AlbMailing().SendMail(mail);
                    }
                }
                var retour = new
                {
                    status = "200",
                    result = hasResults,
                    message = $"{(caseExist ? "" : "nouvelle ")}recherche sur le partenaire {caseName}"
                };
                return Ok(retour);
            }
            catch (Exception ex)
            {
                var retour = new
                {
                    status = "500",
                    result = "",
                    message = $"Erreur : {ex.Message}"
                };
                return Ok(retour);
            }

        }
        [HttpGet]
        public IHttpActionResult CheckPartenaireModif(string caseId, string caseName)
        {
            try
            {
                getResolution();

                //Vérification de l'existance du partenaire chez Refinitiv
                var caseExist = checkAvailibility(caseId);

                string caseSystemId = "";
                bool hasResults = false;

                if (caseExist)
                {
                    //Si le partenaire existe => récupération de l'ID chez Refinitiv
                    caseSystemId = getSystemId(caseId);

                    archiveCase(caseSystemId);
                    deleteCase(caseSystemId);

                    //création d'une nouvelle recherche pour le partenaire
                    RefinitivObject refobj = getCases(caseId, caseName);
                    caseSystemId = refobj.CaseSystemId;
                    hasResults = refobj.HasResults;
                    //Assurer un suivi des recherches
                    setOnGoing(refobj.CaseSystemId);
                    if (hasResults)
                    {
                        var user = AlbSessionHelper.ConnectedUser;
                        // Envoyer mail (destinataire a définir)
                        MailModel mail = AlbOpConstants.MainInfoContent;
                        mail.Body = $"Un nouveau case a été créé pour le partenaire {caseName}, et celui-ci contient des résultats positifs.";
                        mail.Subject = "Test Refinitiv";
                        mail.To = "jean-luc.weng@albingia.fr;erwan.coulm@albingia.fr";
                        new AlbMailing().SendMail(mail);
                    }

                    var retour = new
                    {
                        status = "200",
                        result = hasResults,
                        message = $"nouvelle recherche sur le partenaire {caseName}"
                    };
                    return Ok(retour);
                }
                else
                {
                    var retour = new
                    {
                        status = "404",
                        message = $"Le partenaire spécifié n'existe pas"
                    };
                    return Ok(retour);
                }
            }
            catch (Exception ex)
            {
                var retour = new
                {
                    status = "500",
                    result = "",
                    message = $"Erreur : {ex.Message}"
                };
                return Ok(retour);
            }
        }


        [HttpGet]
        public IHttpActionResult ArchivePartenaire(string caseId)
        {
            var caseExist = checkAvailibility(caseId);

            string caseSystemId = "";

            if (caseExist)
            {
                //Si le partenaire existe => récupération de l'ID chez Refinitiv
                caseSystemId = getSystemId(caseId);
                //Récupération des résultats chez Refinitiv
                archiveCase(caseSystemId);

                var retour = new
                {
                    status = "200",
                    message = $"Le partenaire a été archivé avec succès"
                };
                return Ok(retour);
            } else
            {
                var retour = new
                {
                    status = "404",
                    message = $"Le partenaire spécifié n'existe pas"
                };
                return Ok(retour);
            }
        }

        [HttpGet]
        public IHttpActionResult UnarchivePartenaire(string caseId)
        {
            var caseExist = checkAvailibility(caseId);

            string caseSystemId = "";

            if (caseExist)
            {
                //Si le partenaire existe => récupération de l'ID chez Refinitiv
                caseSystemId = getSystemId(caseId);
                //Récupération des résultats chez Refinitiv
                unarchiveCase(caseSystemId);

                var retour = new
                {
                    status = "200",
                    message = $"Le partenaire a été désarchivé avec succès"
                };
                return Ok(retour);
            }
            else
            {
                var retour = new
                {
                    status = "404",
                    message = $"Le partenaire spécifié n'existe pas"
                };
                return Ok(retour);
            }
        }

        [HttpGet]
        public IHttpActionResult DeletePartenaire(string caseId)
        {
            var caseExist = checkAvailibility(caseId);

            string caseSystemId = "";

            if (caseExist)
            {
                //Si le partenaire existe => récupération de l'ID chez Refinitiv
                caseSystemId = getSystemId(caseId);
                //Récupération des résultats chez Refinitiv
                archiveCase(caseSystemId);
                deleteCase(caseSystemId);

                var retour = new
                {
                    status = "200",
                    message = $"Le partenaire a été supprimé avec succès"
                };
                return Ok(retour);
            }
            else
            {
                var retour = new
                {
                    status = "404",
                    message = $"Le partenaire spécifié n'existe pas"
                };
                return Ok(retour);
            }
        }
        #endregion

        #region GRP 
        private static string getGrpAccountId(string accountNumber, string type)
        {
            string grpServiceUrl = "https://grp365.albingia.fr/Albingia/api/data/v8.2/";
            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(grpServiceUrl + $"accounts?$select=accountid&$filter=accountnumber eq '{accountNumber}' and customertypecode eq {type}");
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
            string f = "";
            if (obj["value"].HasValues)
            {
                f = obj["value"][0]["accountid"].ToString();
            }
            else
            {
                throw new Exception("aucun GUID trouvé dans la GRP pour ce numero");
            }
            return f;
        }

        private static string getGrpInterlocuteurId(string accountId, string interlocuteurNumber)
        {
            string grpServiceUrl = "https://grp365.albingia.fr/Albingia/api/data/v8.2/";
            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(grpServiceUrl + $"contacts?$select=contactid&$filter=_parentcustomerid_value eq {accountId} and new_numerointerlocuteur eq '{interlocuteurNumber}'");
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
            string f = "";
            if (obj["value"].HasValues)
            {
                f = obj["value"][0]["contactid"].ToString();
            }
            else
            {
                throw new Exception("aucun GUID trouvé dans la GRP pour ce numero d'interlocuteur");
            }
            return f;
        }
        #endregion

        #region Refinitiv API
        // Get my top-level groups
        private static void checkGroups()
        {
            initWebRequest("get " + gatewayUrl + "groups\n");

            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestEndPoint + "groups");
            // Set the Headers
            WebReq.Method = "GET";
            WebReq.Headers.Add("Authorization", authorisation);
            WebReq.Headers.Add("Cache-Control", "no-cache");
            WebReq.Date = dateValue; // use datetime value GMT time

            // Get the Response - Status OK
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            // Get the Response data
            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);
            string jsontxt = _Answer.ReadToEnd();

            // convert json text to a pretty printout
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsontxt);
            var f = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }

        // Get my top-level groups
        private static void getResolution()
        {
            initWebRequest("get " + gatewayUrl + "groups/5jb6tmn1o2hd1fjcllpkuozi6/resolutionToolkit\n");

            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestEndPoint + "groups/5jb6tmn1o2hd1fjcllpkuozi6/resolutionToolkit");
            // Set the Headers
            WebReq.Method = "GET";
            WebReq.Headers.Add("Authorization", authorisation);
            WebReq.Headers.Add("Cache-Control", "no-cache");
            WebReq.Date = dateValue; // use datetime value GMT time

            // Get the Response - Status OK
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            // Get the Response data
            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);
            string jsontxt = _Answer.ReadToEnd();

            // convert json text to a pretty printout
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(jsontxt);

            var statuses = obj["resolutionFields"]["statuses"].ToArray();
            statusesList = new List<RefinitivListObject>();
            for (int i = 0; i < statuses.Length; i++)
            {
                statusesList.Add(new RefinitivListObject() {
                    Id = statuses[i]["id"].ToString(),
                    Label = statuses[i]["label"].ToString(),
                });
            }
        }

        // Check CaseId availibility
        private static bool checkAvailibility(string caseId)
        {
            initWebRequest("head " + gatewayUrl + "caseIdentifiers\n");

            try
            {
                // Send the Request to the API server
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create($"{requestEndPoint}caseIdentifiers?caseId={caseId}");
                // Set the Headers
                WebReq.Method = "HEAD";
                WebReq.Headers.Add("Authorization", authorisation);
                WebReq.Headers.Add("Cache-Control", "no-cache");
                WebReq.Date = dateValue; // use datetime value GMT time

                // Get the Response - Status OK
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

                // Get the Response data
                Stream Answer = WebResp.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);
                string jsontxt = _Answer.ReadToEnd();

                // convert json text to a pretty printout
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsontxt);
                var f = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);

                return (WebResp.StatusCode == HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        // SEQ-case-retrieve: Get the system ID of a case
        private static string getSystemId(string caseId)
        {
            //pour test
            if (caseId is null || caseId == "")
                caseId = "4444C";

            initWebRequest("get " + gatewayUrl + "caseReferences\n");

            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create($"{requestEndPoint}caseReferences?caseId={caseId}");
            // Set the Headers
            WebReq.Method = "GET";
            WebReq.Headers.Add("Authorization", authorisation);
            WebReq.Headers.Add("Cache-Control", "no-cache");
            WebReq.Date = dateValue; // use datetime value GMT time

            // Get the Response - Status OK
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            // Get the Response data
            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);
            string jsontxt = _Answer.ReadToEnd();

            // convert json text to a pretty printout
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(jsontxt);
            var f = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);

            //Retourner la propriété "caseSystemId" de obj
            return obj["caseSystemId"].ToString();
        }

        // SEQ-case-investigate-results: Get screening results
        private static bool getScreeningResults(string caseSystemId = "")
        {
            //if (caseSystemId is null || caseSystemId == "")
            //    caseSystemId = "5nzbfqankw2o1fbdph64ro8w5";

            initWebRequest("get " + gatewayUrl + "cases/" + caseSystemId + "/results\n");

            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create($"{requestEndPoint}cases/{caseSystemId}/results");
            // Set the Headers
            WebReq.Method = "GET";
            WebReq.Headers.Add("Authorization", authorisation);
            WebReq.Headers.Add("Cache-Control", "no-cache");
            WebReq.Date = dateValue; // use datetime value GMT time


            // Get the Response - Status OK
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();


            // Get the Response data
            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);
            string jsontxt = _Answer.ReadToEnd();


            // convert json text to a pretty printout
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(jsontxt);
            if (obj.HasValues)
            {
                bool hasPositiveResults = false;
                for (int i = 0; i < obj.ToArray().Length; i++)
                {
                    if (!obj[i]["resolution"].HasValues ||
                        statusesList.FirstOrDefault(x => x.Label.ToUpper() == "FALSE").Id != obj[i]["resolution"]["statusId"].ToString())
                    {
                        hasPositiveResults = true;
                        break;
                    }
                }
                return hasPositiveResults;
            }
            else
            {
                return false;
            }
        }

        // 
        private static RefinitivObject getCases(string caseId, string name)
        {
            //5nzbfqao1qzg1fcw1laqdg5ux  1st group
            //5nzbfkdmfcqy1fcw10qts7j98  2nd group
            //5nzbfqao1qzg1fcyt1u4gsr0d  3rd group
            string postData = "{\"secondaryFields\":[{" +
            "\"typeId\": \"SFCT_6\"," +
            "\"value\": \"FRA\"" +
            "}]," +
            "\"entityType\":\"ORGANISATION\"," +
            "\"customFields\":[]," +
            "\"groupId\":\"5jb6tmn1o2hd1fjcllpkuozi6\"," +
            "\"providerTypes\":[\"WATCHLIST\"]," +
            "\"name\":\"" + name + "\"," +
            "\"caseId\":\"" + caseId + "\"" +
            "}";
            initWebRequest("post " + gatewayUrl + "cases/screeningRequest\n", postData);

            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestEndPoint + "cases/screeningRequest");
            // Set the Headers
            WebReq.Method = "POST";
            WebReq.Headers.Add("Authorization", authorisation);
            WebReq.Headers.Add("Cache-Control", "no-cache");
            //WebReq.ContentLength = msg.Length;
            WebReq.Date = dateValue;
            // Set the content type of the data being posted.
            WebReq.ContentType = "application/json";
            WebReq.ContentLength = byte1.Length;

            Stream newStream = WebReq.GetRequestStream();
            newStream.Write(byte1, 0, byte1.Length);

            // Get the Response - Status OK
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            // Get the Response data
            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);
            string jsontxt = _Answer.ReadToEnd();

            // convert json text to a pretty printout
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(jsontxt);
            bool hasPositiveResults = false;
            if (obj["results"].HasValues)
            {
                for (int i = 0; i < obj["results"].ToArray().Length; i++)
                {
                    if (!obj["results"][i]["resolution"].HasValues ||
                        statusesList.FirstOrDefault(x => x.Label.ToUpper() == "FALSE").Id != obj["results"][i]["resolution"]["statusId"].ToString())
                    {
                        hasPositiveResults = true;
                        break;
                    }
                }
            }
            // L'envoi de mail se fait deja dans le chackpartenaire
            //if (hasPositiveResults)
            //{
            //    var user = AlbSessionHelper.ConnectedUser;
            //    // Envoyer mail (destinataire a définir)
            //    MailModel mail = new MailModel()
            //    {
            //        From = AlbOpConstants.MainInfoContent.From,
            //        FromDisplayName = AlbOpConstants.MainInfoContent.FromDisplayName,
            //        Body = $"Les résultats des partenaires ont été enregistrés avec succès",
            //        Subject = "Test Refinitiv",
            //        To = "jean-luc.weng@albingia.fr;erwan.coulm@albingia.fr"
            //    };
            //    new AlbMailing().SendMail(mail);
            //}
            return new RefinitivObject()
            {
                CaseSystemId = obj["caseSystemId"].ToString(),
                HasResults = hasPositiveResults
            };

        }

        private static void setOnGoing(string caseSystemId)
        {
            initWebRequest("put " + gatewayUrl + "cases/" + caseSystemId + "/ongoingScreening\n");

            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create($"{requestEndPoint}cases/{caseSystemId}/ongoingScreening");
            // Set the Headers
            WebReq.Method = "PUT";
            WebReq.Headers.Add("Authorization", authorisation);
            WebReq.Headers.Add("Cache-Control", "no-cache");
            WebReq.Date = dateValue; // use datetime value GMT time


            // Get the Response - Status OK
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        }

        private static void archiveCase(string caseSystemId)
        {
            initWebRequest("put " + gatewayUrl + "cases/" + caseSystemId + "/archive\n");

            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create($"{requestEndPoint}cases/{caseSystemId}/archive");
            // Set the Headers
            WebReq.Method = "PUT";
            WebReq.Headers.Add("Authorization", authorisation);
            WebReq.Headers.Add("Cache-Control", "no-cache");
            WebReq.Date = dateValue; // use datetime value GMT time


            // Get the Response - Status OK
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        }

        private static void unarchiveCase(string caseSystemId)
        {
            initWebRequest("delete " + gatewayUrl + "cases/" + caseSystemId + "/archive\n");

            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create($"{requestEndPoint}cases/{caseSystemId}/archive");
            // Set the Headers
            WebReq.Method = "DELETE";
            WebReq.Headers.Add("Authorization", authorisation);
            WebReq.Headers.Add("Cache-Control", "no-cache");
            WebReq.Date = dateValue; // use datetime value GMT time


            // Get the Response - Status OK
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        }

        private static void deleteCase(string caseSystemId)
        {
            initWebRequest("delete " + gatewayUrl + "cases/" + caseSystemId + "\n");

            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create($"{requestEndPoint}cases/{caseSystemId}");
            // Set the Headers
            WebReq.Method = "DELETE";
            WebReq.Headers.Add("Authorization", authorisation);
            WebReq.Headers.Add("Cache-Control", "no-cache");
            WebReq.Date = dateValue; // use datetime value GMT time


            // Get the Response - Status OK
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        }
        #endregion

        #region private method
        private static void initWebRequest(string dataToSignPart, string postData = "")
        {
            if (!string.IsNullOrEmpty(postData))
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte1 = encoding.GetBytes(postData);
            }

            dateValue = DateTime.UtcNow; // get the datetime NOW GMT
            date = dateValue.ToString("R"); // WC1 header requires GMT datetime stamp

            // Assemble the GET request - NOTE every character including spaces have to be EXACT
            // for the API server to decode the authorization signature
            dataToSign = "(request-target): " + dataToSignPart +
            "host: " + gatewayHost + "\n" + // no https only the host name
            "date: " + date; // GMT date as a string

            if (!string.IsNullOrEmpty(postData))
            {
                dataToSign += "\n" + "content-type: " + "application/json" + "\n" +
                              "content-length: " + byte1.Length + "\n" +
                              postData;
            }

            // The Request and API secret are now combined and encrypted
            hmac = generateAuthHeader(dataToSign, apiSecret);

            // Assemble the authorization string - This needs to match the dataToSign elements
            // i.e. requires ONLY host date (no content body for a GET request)
            //- NOTE every character including spaces have to be EXACT else decryption will fail with 401 Unauthorized
            if (string.IsNullOrEmpty(postData))
            {
                authorisation = "Signature keyId=\"" + apiKey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date\",signature=\"" + hmac + "\"";
            }
            else
            {
                authorisation = "Signature keyId=\"" + apiKey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date content-type content-length\",signature=\"" + hmac + "\"";
            }
        }

        // Combine the data signature and the API secret key to get the HMAC
        // This is the Microsoft HMACSHA256 code copied from the documentation
        private static string generateAuthHeader(string dataToSign, string apisecret)
        {
            byte[] secretKey = Encoding.UTF8.GetBytes(apisecret);
            HMACSHA256 hmac = new HMACSHA256(secretKey);
            hmac.Initialize();

            byte[] bytes = Encoding.UTF8.GetBytes(dataToSign);
            byte[] rawHmac = hmac.ComputeHash(bytes);

            return (Convert.ToBase64String(rawHmac));
        }
        #endregion

        #region Class
        public class RefinitivObject
        {
            public string CaseSystemId { get; set; }
            public bool HasResults { get; set; }
        }

        public class RefinitivListObject
        {
            public string Id { get; set; }
            public string Label { get; set; }
        }
        #endregion
    }
}
