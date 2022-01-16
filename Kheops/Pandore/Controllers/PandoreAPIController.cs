using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Models.Common;
using ALBINGIA.Framework.Common.Tools;
using Aspose.Cells;
using Newtonsoft.Json.Linq;
using Pandore.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ALBINGIA.OP.OP_MVC.Controllers.API
{
    public class PandoreAPIController : ApiController
    {
        static string gatewayUrl = ConfigurationManager.AppSettings["RefinitivUrl"];//"/v2/";// 
        static string gatewayHost = ConfigurationManager.AppSettings["RefinitivHost"];//"api-worldcheck.refinitiv.com";//

        static string apiKey = ConfigurationManager.AppSettings["APIKey"];//"8ac85b73-1964-450a-9b8b-35a444f7b5fd";//
        static string apiSecret = ConfigurationManager.AppSettings["APISecret"];//"fYxa/lKwikRF27TWH8M6L6/WniKpc2K1DXhRw4yYpkNLkh/6uzeMPXeHIHGBAMeoxNifEq6n76SVQ7Mb5pWcWg==";//

        static string requestEndPoint = ConfigurationManager.AppSettings["RefinitivEndpoint"];//"https://api-worldcheck.refinitiv.com/v2/";//
        static string grpEndPoint = ConfigurationManager.AppSettings["GRPEndpoint"];
        static string groupId = ConfigurationManager.AppSettings["GroupId"];

        static string mailTo = ConfigurationManager.AppSettings["MailTo"];
        static string environnement = ConfigurationManager.AppSettings["Environnement"];

        static DateTime dateValue;
        static string date, dataToSign, hmac, authorisation;
        static byte[] byte1;

        #region public methods
        [HttpGet]
        public IHttpActionResult CheckPartenaireBatch()
        {
            using (StreamWriter sw = new StreamWriter(System.IO.File.Open(@"C:\Pandore\logPandore.txt", FileMode.Append)))
            {

                // Get fichier excel et lire les lignes
                Workbook workbook = new Workbook(@"C:\Pandore\testgroup.xlsx");
                Worksheet worksheet = workbook.Worksheets[0];
                Cells cells = worksheet.Cells;
                sw.WriteLine("Lecture des cellules du fichier: début de la boucle");

                for (int i = 1; i < cells.GetLastDataRow(0) + 1; i++)
                {
                    try
                    {
                        string caseId = cells[i, 0].Value.ToString();

                        //Vérification de l'existance du partenaire chez Refinitiv
                        var caseExist = checkAvailibility(caseId);
                        if (!caseExist) // vérification nécessaire dû a une erreur lors de l'enregistrement de plusieurs partenaire
                        {
                            var caseIdbis = caseId + "1";
                            caseExist = checkAvailibility(caseIdbis);
                            caseId = caseExist ? caseIdbis : caseId;
                        }

                        string caseSystemId = "";
                        bool hasResults = false;
                        bool isNew = !caseExist;

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
                            // penser à mettre .Split('-')[0] si besoin !!!!!!!!!!!!
                            RefinitivObject refobj = getCases(caseId, cells[i, 2].Value.ToString(), 1);
                            caseSystemId = refobj.CaseSystemId;
                            hasResults = refobj.HasResults;
                            //Assurer un suivi des recherches
                            setOnGoing(refobj.CaseSystemId);
                        }
                        worksheet.Cells[i, 13].PutValue(caseId);
                        worksheet.Cells[i, 14].PutValue(caseSystemId);
                        worksheet.Cells[i, 15].PutValue(hasResults.ToString());
                        worksheet.Cells[i, 16].PutValue(DateTime.Now.ToLongDateString());
                        worksheet.Cells[i, 17].PutValue(hasResults ? "1" : "0");
                        worksheet.Cells[i, 18].PutValue(isNew);
                    }
                    catch (Exception ex)
                    {
                        sw.WriteLine("Error: " + ex.Message);
                        sw.WriteLine("Une erreur est survenue sur la ligne " + i + "(id = " + cells[i, 3].Value + ")");
                    }
                }
                workbook.Save(@"C:\Pandore\testgroup.xlsx");
                var retour = new
                {
                    status = "200",
                    message = $"Partenaires vérifiés avec succès"
                };
                return Ok(retour);

            }
        }

        [HttpGet]
        public IHttpActionResult DeletePartenaireBatch()
        {
            using (StreamWriter sw = new StreamWriter(System.IO.File.Open(@"C:\Pandore\logPandore.txt", FileMode.Append)))
            {

                // Get fichier excel et lire les lignes
                Workbook workbook = new Workbook(@"C:\Pandore\CaseASupprimer.xlsx");
                Worksheet worksheet = workbook.Worksheets[0];
                Cells cells = worksheet.Cells;
                sw.WriteLine("Lecture des cellules du fichier: début de la boucle");

                for (int i = 1; i < cells.GetLastDataRow(0) + 1; i++)
                {
                    try
                    {
                        string caseId = cells[i, 0].Value.ToString();

                        var caseExist = checkAvailibility(caseId);
                        if (caseExist)
                        {
                            var caseSystemId = getSystemId(caseId);
                            archiveCase(caseSystemId);
                            deleteCase(caseSystemId);

                            worksheet.Cells[i, 34].PutValue("Supprimé");
                        }
                        else
                        {
                            worksheet.Cells[i, 34].PutValue("Non existant");
                        }
                    }
                    catch (Exception ex)
                    {
                        sw.WriteLine("Error: " + ex.Message);
                        sw.WriteLine("Une erreur est survenue sur la ligne " + i + "(id = " + cells[i, 0].Value + ")");
                    }
                }
                workbook.Save(@"C:\Pandore\CaseASupprimer.xlsx");
                var retour = new
                {
                    status = "200",
                    message = $"Les cases ont été supprimés avec succès"
                };
                return Ok(retour);

            }
        }

        [HttpGet]
        public IHttpActionResult ArchivePartenaireBatch()
        {
            using (StreamWriter sw = new StreamWriter(System.IO.File.Open(@"C:\Pandore\logPandore.txt", FileMode.Append)))
            {

                // Get fichier excel et lire les lignes
                Workbook workbook = new Workbook(@"C:\Pandore\CaseAArchiver.xlsx");
                Worksheet worksheet = workbook.Worksheets[0];
                Cells cells = worksheet.Cells;
                sw.WriteLine("Lecture des cellules du fichier: début de la boucle");

                for (int i = 1; i < cells.GetLastDataRow(0) + 1; i++)
                {
                    try
                    {
                        string caseId = cells[i, 0].Value.ToString();

                        var caseExist = checkAvailibility(caseId);
                        if (caseExist)
                        {
                            var caseSystemId = getSystemId(caseId);
                            archiveCase(caseSystemId);

                            worksheet.Cells[i, 34].PutValue("Archivé");
                        }
                        else
                        {
                            worksheet.Cells[i, 34].PutValue("Non existant");
                        }
                    }
                    catch (Exception ex)
                    {
                        sw.WriteLine("Error: " + ex.Message);
                        sw.WriteLine("Une erreur est survenue sur la ligne " + i + "(id = " + cells[i, 0].Value + ")");
                    }
                }
                workbook.Save(@"C:\Pandore\CaseASupprimer.xlsx");
                var retour = new
                {
                    status = "200",
                    message = $"Les cases ont été archivé avec succès"
                };
                return Ok(retour);

            }
        }

        [HttpGet]
        public IHttpActionResult CheckPartenaire(string caseNumber, string caseName, string type, int entityType = 1)
        {
            #region Return for PILOT
            if (environnement != "PROD")
            {
                var ret = new
                {
                    status = "200",
                    result = false,
                    message = $"recherche sur le partenaire {caseName} ({caseNumber})"
                };
                return Ok(ret);
            }
            #endregion

            try
            {
                string caseId = getGrpAccountId(caseNumber, type);
                //Vérifie si les paramètres fonctionnent
                //checkGroups();

                //Vérification de l'existance du partenaire chez Refinitiv
                var caseExist = checkAvailibility(caseId);
                if (!caseExist) // vérification nécessaire dû a une erreur lors de l'enregistrement de plusieurs partenaire
                {
                    var caseIdbis = caseId + "1";
                    caseExist = checkAvailibility(caseIdbis);
                    caseId = caseExist ? caseIdbis : caseId;
                }

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
                    RefinitivObject refobj = getCases(caseId, caseName, entityType);
                    caseSystemId = refobj.CaseSystemId;
                    hasResults = refobj.HasResults;
                    //Assurer un suivi des recherches
                    setOnGoing(refobj.CaseSystemId);
                    if (hasResults)
                    {
                        var user = AlbSessionHelper.ConnectedUser;
                        // Envoyer mail (destinataire a définir)
                        //var body = $"Un nouveau case a été créé pour le partenaire {caseName} ({caseNumber}), et celui-ci contient des résultats positifs.";
                        var body = "<p>Bonjour,</p>"
                                 + "<p>Une nouvelle entité suspecte a été détectée par Pandore.</p>"
                                 + "<p>Merci de vous connecter à votre espace Refinitiv pour procéder aux vérifications</p>"
                                 + "<p><strong>Plus de détails :</strong></p>"
                                 + $"<p>Nom : {caseName}</p>"
                                 + "<a href='https://worldcheck.refinitiv.com/'>Procéder à une vérification</a>";
                        var subject = $"Notification Automatique Pandore";
                        var to = mailTo.Split(';').ToList();
                        EmailSender.Send(to, subject, body, true);
                    }
                }
                var retour = new
                {
                    status = "200",
                    result = hasResults,
                    message = $"{(caseExist ? "" : "nouvelle ")}recherche sur le partenaire {caseName} ({caseNumber})"
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
        public IHttpActionResult CheckPartenaire(string caseId, string caseName, int entityType = 1)
        {
            #region Return for PILOT
            if (environnement != "PROD")
            {
                var ret = new
                {
                    status = "200",
                    result = false,
                    message = $"recherche sur le partenaire {caseName}"
                };
                return Ok(ret);
            }
            #endregion

            try
            {
                //Vérification de l'existance du partenaire chez Refinitiv
                var caseExist = checkAvailibility(caseId);
                if (!caseExist) // vérification nécessaire dû a une erreur lors de l'enregistrement de plusieurs partenaire
                {
                    var caseIdbis = caseId + "1";
                    caseExist = checkAvailibility(caseIdbis);
                    caseId = caseExist ? caseIdbis : caseId;
                }

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
                    RefinitivObject refobj = getCases(caseId, caseName, entityType);
                    caseSystemId = refobj.CaseSystemId;
                    hasResults = refobj.HasResults;
                    //Assurer un suivi des recherches
                    setOnGoing(refobj.CaseSystemId);
                    if (hasResults)
                    {
                        var user = AlbSessionHelper.ConnectedUser;
                        // Envoyer mail (destinataire a définir)
                        //var body = $"Un nouveau case a été créé pour le partenaire {caseName} ({caseNumber}), et celui-ci contient des résultats positifs.";
                        var body = "<p>Bonjour,</p>"
                                 + "<p>Une nouvelle entité suspecte a été détectée par Pandore.</p>"
                                 + "<p>Merci de vous connecter à votre espace Refinitiv pour procéder aux vérifications</p>"
                                 + "<p><strong>Plus de détails :</strong></p>"
                                 + $"<p>Nom : {caseName}</p>"
                                 + "<a href='https://worldcheck.refinitiv.com/'>Procéder à une vérification</a>";
                        var subject = $"Notification Automatique Pandore";
                        var to = mailTo.Split(';').ToList();
                        EmailSender.Send(to, subject, body, true);
                    }
                }
                var retour = new
                {
                    status = "200",
                    result = hasResults,
                    message = $"{(caseExist ? "" : "nouvelle ")} recherche sur le partenaire {caseName}"
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
        public IHttpActionResult CheckPartenaireModif(string caseId, string caseName, int entityType = 1)
        {
            #region Return for PILOT
            if (environnement != "PROD")
            {
                var ret = new
                {
                    status = "200",
                    result = false,
                    message = $"recherche sur le partenaire {caseName}"
                };
                return Ok(ret);
            }
            #endregion

            try
            {
                //Vérification de l'existance du partenaire chez Refinitiv
                var caseExist = checkAvailibility(caseId);
                if (!caseExist) // vérification nécessaire dû a une erreur lors de l'enregistrement de plusieurs partenaire
                {
                    var caseIdbis = caseId + "1";
                    caseExist = checkAvailibility(caseIdbis);
                    caseId = caseExist ? caseIdbis : caseId;
                }

                string caseSystemId = "";
                bool hasResults = false;

                if (caseExist)
                {
                    //Si le partenaire existe => récupération de l'ID chez Refinitiv
                    caseSystemId = getSystemId(caseId);

                    archiveCase(caseSystemId);
                    deleteCase(caseSystemId);

                    //création d'une nouvelle recherche pour le partenaire
                    RefinitivObject refobj = getCases(caseId, caseName, entityType);
                    caseSystemId = refobj.CaseSystemId;
                    hasResults = refobj.HasResults;
                    //Assurer un suivi des recherches
                    setOnGoing(refobj.CaseSystemId);
                    if (hasResults)
                    {
                        var user = AlbSessionHelper.ConnectedUser;
                        // Envoyer mail (destinataire a définir)
                        //var body = $"Un nouveau case a été créé pour le partenaire {caseName} ({caseNumber}), et celui-ci contient des résultats positifs.";
                        var body = "<p>Bonjour,</p>"
                                 + "<p>Une nouvelle entité suspecte a été détectée par Pandore.</p>"
                                 + "<p>Merci de vous connecter à votre espace Refinitiv pour procéder aux vérifications</p>"
                                 + "<p><strong>Plus de détails :</strong></p>"
                                 + $"<p>Nom : {caseName}</p>"
                                 + "<a href='https://worldcheck.refinitiv.com/'>Procéder à une vérification</a>";
                        var subject = $"Notification Automatique Pandore";
                        var to = mailTo.Split(';').ToList();
                        EmailSender.Send(to, subject, body, true);
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
            #region Return for PILOT
            if (environnement != "PROD")
            {
                var ret = new
                {
                    status = "200",
                    message = $"Le partenaire a été archivé avec succès"
                };
                return Ok(ret);
            }
            #endregion

            var caseExist = checkAvailibility(caseId);
            if (!caseExist) // vérification nécessaire dû a une erreur lors de l'enregistrement de plusieurs partenaire
            {
                var caseIdbis = caseId + "1";
                caseExist = checkAvailibility(caseIdbis);
                caseId = caseExist ? caseIdbis : caseId;
            }

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
        public IHttpActionResult UnarchivePartenaire(string caseId)
        {
            #region Return for PILOT
            if (environnement != "PROD")
            {
                var ret = new
                {
                    status = "200",
                    message = $"Le partenaire a été désarchivé avec succès"
                };
                return Ok(ret);
            }
            #endregion

            var caseExist = checkAvailibility(caseId);
            if (!caseExist) // vérification nécessaire dû a une erreur lors de l'enregistrement de plusieurs partenaire
            {
                var caseIdbis = caseId + "1";
                caseExist = checkAvailibility(caseIdbis);
                caseId = caseExist ? caseIdbis : caseId;
            }

            string caseSystemId = "";

            if (caseExist)
            {
                //Si le partenaire existe => récupération de l'ID chez Refinitiv
                caseSystemId = getSystemId(caseId);
                //Récupération des résultats chez Refinitiv
                unarchiveCase(caseSystemId);
                setOnGoing(caseSystemId);

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
            #region Return for PILOT
            if (environnement != "PROD")
            {
                var ret = new
                {
                    status = "200",
                    message = $"Le partenaire a été supprimé avec succès"
                };
                return Ok(ret);
            }
            #endregion

            var caseExist = checkAvailibility(caseId);
            if (!caseExist) // vérification nécessaire dû a une erreur lors de l'enregistrement de plusieurs partenaire
            {
                var caseIdbis = caseId + "1";
                caseExist = checkAvailibility(caseIdbis);
                caseId = caseExist ? caseIdbis : caseId;
            }

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
            //string grpServiceUrl = "https://grp365.albingia.fr/Albingia/api/data/v8.2/";
            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(grpEndPoint + $"accounts?$select=accountid&$filter=accountnumber eq '{accountNumber}' and customertypecode eq {type}");
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
            //string grpServiceUrl = "https://grp365.albingia.fr/Albingia/api/data/v8.2/";
            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(grpEndPoint + $"contacts?$select=contactid&$filter=_parentcustomerid_value eq {accountId} and new_numerointerlocuteur eq '{interlocuteurNumber}'");
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

        private static List<RefinitivListObject> getStatuses()
        {
            if (HttpContext.Current.Cache["status"] != null)
            {
                return HttpContext.Current.Cache["status"] as List<RefinitivListObject>;
            }
            getResolution();
            return HttpContext.Current.Cache["status"] as List<RefinitivListObject>;
        }
        private static List<RefinitivListObject> getRisks()
        {
            if (HttpContext.Current.Cache["risks"] != null)
            {
                return HttpContext.Current.Cache["risks"] as List<RefinitivListObject>;
            }
            getResolution();
            return HttpContext.Current.Cache["risks"] as List<RefinitivListObject>;
        }
        // Get my top-level groups
        private static void getResolution()
        {
            initWebRequest("get " + gatewayUrl + "groups/" + groupId + "/resolutionToolkit\n");
            //initWebRequest("get " + gatewayUrl + "groups/5jb6tmn1o2hd1fjcllpkuozi6/resolutionToolkit\n");

            // Send the Request to the API server
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestEndPoint + "groups/" + groupId + "/resolutionToolkit");
            //HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestEndPoint + "groups/5jb6tmn1o2hd1fjcllpkuozi6/resolutionToolkit");
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
            List<RefinitivListObject> statusesList = new List<RefinitivListObject>();
            for (int i = 0; i < statuses.Length; i++)
            {
                statusesList.Add(new RefinitivListObject()
                {
                    Id = statuses[i]["id"].ToString(),
                    Label = statuses[i]["label"].ToString(),
                });
            }
            HttpContext.Current.Cache["status"] = statusesList;

            var risks = obj["resolutionFields"]["risks"].ToArray();
            List<RefinitivListObject> risksList = new List<RefinitivListObject>();
            for (int i = 0; i < risks.Length; i++)
            {
                risksList.Add(new RefinitivListObject()
                {
                    Id = risks[i]["id"].ToString(),
                    Label = risks[i]["label"].ToString(),
                });
            }
            HttpContext.Current.Cache["risks"] = risksList;
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

            List<RefinitivListObject> statusesList = getStatuses();
            List<RefinitivListObject> risksList = getRisks();
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
                        (statusesList.FirstOrDefault(x => x.Label.ToUpper() == "POSITIVE").Id == obj[i]["resolution"]["statusId"].ToString()
                        && risksList.FirstOrDefault(x => x.Label.ToUpper() == "HIGH").Id == obj[i]["resolution"]["riskId"].ToString()))
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
        private static RefinitivObject getCases(string caseId, string name, int entityType)
        {
            name = name.Replace("\"", "\\\"");
            List<RefinitivListObject> statusesList = getStatuses();
            List<RefinitivListObject> risksList = getRisks();
            #region Groupes PROD
            //5jb71gsgrwyi1fjcmg0oxzare  Parent group
            //5jb8g1y3ocow1fkhvot3egbr0  1st group
            //5jb6u0gde8tu1fkhvponw97g6  2nd group
            //5jb7eqa1yzfi1fkhvqcc9lujh  3rd group
            //5jb8a0wpabea1fkhvv01w6jtj  4th group
            //5jb7o7megh5v1fkhvvrobrbd1  5th group
            //5jb69ekr1m1p1fkhvwv4y0epq  6th group
            //5jb8a0wpabea1fkhvyafqxucf  7th group
            #endregion
            #region Groupes PILOT
            //5jb6tmn1o2hd1fjcllpkuozi6  1st group
            #endregion

            string entityTypeStr = "";
            string secondaryField = "";
            switch (entityType)
            {
                case 1:
                    entityTypeStr = "ORGANISATION";
                    secondaryField = "{" +
            "\"typeId\": \"SFCT_6\"," +
            "\"value\": \"FRA\"" +
            "}";
                    break;
                case 2:
                    entityTypeStr = "INDIVIDUAL";
                    break;
                default:
                    entityTypeStr = "ORGANISATION";
                    secondaryField = "{" +
            "\"typeId\": \"SFCT_6\"," +
            "\"value\": \"FRA\"" +
            "}";
                    break;
            }

            string postData = "{\"secondaryFields\":[" + secondaryField + "]," +
            "\"entityType\":\"" + entityTypeStr + "\"," +
            "\"customFields\":[]," +
            "\"groupId\":\"" + groupId + "\"," +
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
                        (statusesList.FirstOrDefault(x => x.Label.ToUpper() == "POSITIVE").Id == obj["results"][i]["resolution"]["statusId"].ToString()
                        && risksList.FirstOrDefault(x => x.Label.ToUpper() == "HIGH").Id == obj["results"][i]["resolution"]["riskId"].ToString()))
                    {
                        hasPositiveResults = true;
                        break;
                    }
                }
            }

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
