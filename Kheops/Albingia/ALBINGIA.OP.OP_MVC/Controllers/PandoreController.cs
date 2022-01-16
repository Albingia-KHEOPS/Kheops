using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Models.Common;
using ALBINGIA.Framework.Common.Tools;
using Aspose.Cells;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class PandoreController : Controller
    {
        const string gatewayUrl = "/v2/";
        const string gatewayHost = "rms-world-check-one-api-pilot.thomsonreuters.com";

        const string apiKey = "66942a49-341e-4ee8-9130-e1e966e79024";
        const string apiSecret = "XjLZzOq9S7UYz4L4u3m+0euj7A9zdfPDCz0LBfjzIg4Sg5zbRFmFRixFhc/2zSL56JPzC5DoyF3ESwfSNF22vQ==";

        const string requestEndPoint = "https://rms-world-check-one-api-pilot.thomsonreuters.com/v2/";

        static DateTime dateValue;
        static string date, dataToSign, hmac, authorisation;
        static byte[] byte1;

        // GET: Pandore
        public ActionResult CheckPartenaire()
        {
            using (StreamWriter sw = new StreamWriter(System.IO.File.Open(@"C:\Pandore\logPandore.txt", FileMode.Append)))
            {

                // Get fichier excel et lire les lignes
                Workbook workbook = new Workbook(@"C:\Pandore\test.xlsx");
                Worksheet worksheet = workbook.Worksheets[0];
                Cells cells = worksheet.Cells;
                sw.WriteLine("Lecture des cellules du fichier: début de la boucle");
                //RefinitivObject refobj = getCases("test01", "test");
                //archiveCase(caseSystemId);
                //unarchiveCase("5nzbfqaju06w1fd3dj9rm4pl2");
                //deleteCase(refobj.CaseSystemId);

                //for (int i = 1; i < 3/*cells.GetLastDataRow(3) + 1*/; i++)
                //{
                //    try
                //    {
                //        var type = "7";
                //        switch (cells[i, 1].Value.ToString())
                //        {
                //            case "Assuré":
                //                type = "7";
                //                break;
                //            case "Courtier":
                //                type = "1";
                //                break;
                //        }
                //        string caseId = getGrpAccountId(cells[i, 3].Value.ToString(), type);
                //        caseId += "2";
                //        //Vérifie si les paramètres fonctionnent
                //        //checkGroups();

                //        //Vérification de l'existance du partenaire chez Refinitiv
                //        var caseExist = checkAvailibility(caseId);

                //        string caseSystemId = "";
                //        bool hasResults = false;
                //        bool isNew = !caseExist;

                //        if (caseExist)
                //        {
                //            //Si le partenaire existe => récupération de l'ID chez Refinitiv
                //            caseSystemId = getSystemId(caseId);
                //            //Récupération des résultats chez Refinitiv
                //            hasResults = getScreeningResults(caseSystemId);

                //            //archiveCase(caseSystemId);
                //            unarchiveCase(caseSystemId);
                //        }
                //        else
                //        {
                //            //Si le partenaire n'existe pas => création de la recherche et ID chez Refinitiv
                //            RefinitivObject refobj = getCases(caseId, cells[i, 2].Value.ToString().Split('-')[0]);
                //            caseSystemId = refobj.CaseSystemId;
                //            hasResults = refobj.HasResults;
                //            //Assurer un suivi des recherches
                //            setOnGoing(refobj.CaseSystemId);
                //        }
                //        worksheet.Cells[i, 13].PutValue(caseId);
                //        worksheet.Cells[i, 14].PutValue(caseSystemId);
                //        worksheet.Cells[i, 15].PutValue(hasResults.ToString());
                //        worksheet.Cells[i, 16].PutValue(DateTime.Now.ToLongDateString());
                //        worksheet.Cells[i, 17].PutValue(hasResults ? "1" : "0");
                //        worksheet.Cells[i, 18].PutValue(isNew);
                //    }
                //    catch (Exception ex)
                //    {
                //        sw.WriteLine("Error: " + ex.Message);
                //        sw.WriteLine("Une erreur est survenue sur la ligne " + i + "(id = " + cells[i, 3].Value + ")", JsonRequestBehavior.AllowGet);
                //    }
                //}
                var user = AlbSessionHelper.ConnectedUser;
                // Envoyer mail (destinataire a définir)
                MailModel mail = new MailModel()
                {
                    From = AlbOpConstants.MainInfoContent.From,
                    FromDisplayName = AlbOpConstants.MainInfoContent.FromDisplayName,
                    Body = $"Les résultats des partenaires ont été enregistrés avec succès",
                    Subject = "Test Refinitiv",
                    To = "jean-luc.weng@albingia.fr",//;erwan.coulm@albingia.fr";
                };


                new AlbMailing().SendMail(mail);
                workbook.Save(@"C:\Pandore\test.xlsx");
                return Json("Toutes les lignes ont été traitées.", JsonRequestBehavior.AllowGet);

            }
        }

        public static string getGrpAccountId(string accountNumber, string type)
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

        public static string getGrpInterlocuteurId(string accountId, string interlocuteurNumber)
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

        // Get my top-level groups
        public static void checkGroups()
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

        // Check CaseId availibility
        public static bool checkAvailibility(string caseId)
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
        public static string getSystemId(string caseId)
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
        public static bool getScreeningResults(string caseSystemId = "")
        {
            if (caseSystemId is null || caseSystemId == "")
                caseSystemId = "5nzbfqankw2o1fbdph64ro8w5";

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
            var f = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            return obj.HasValues;
        }

        // 
        public static RefinitivObject getCases(string caseId, string name)
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
            "\"groupId\":\"5nzbfkdmfcqy1fcw10qts7j98\"," +
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
            return new RefinitivObject()
            {
                CaseSystemId = obj["caseSystemId"].ToString(),
                HasResults = obj["results"].HasValues
            };

        }

        public static void setOnGoing(string caseSystemId)
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

        public static void archiveCase(string caseSystemId)
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

        public static void unarchiveCase(string caseSystemId)
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

        public static void deleteCase(string caseSystemId)
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

        public class RefinitivObject
        {
            public string CaseSystemId { get; set; }
            public bool HasResults { get; set; }
        }
    }
}