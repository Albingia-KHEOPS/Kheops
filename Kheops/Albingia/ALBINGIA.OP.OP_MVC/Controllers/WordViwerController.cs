using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.WordViewer;
using OPServiceContract.IAdministration;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class WordViewerController : ControllersBase<WordViewer>
    {
        #region Méthodes publiques
        [ErrorHandler]
        public string GetDocumentPath(string typeDoc, string wOpenParam, string resolu, string rule, bool clauseLibre, string createClause, string modeNavig, string contexte, int? numAvenant, string paramGestionDossier = "", string isReadOnly = "False")
        {
            if (!numAvenant.HasValue) {
                throw new ArgumentNullException(nameof(numAvenant));
            }
            var fileFullPath = GetWordDocumentFilePath(typeDoc, wOpenParam, numAvenant.Value, resolu, clauseLibre, createClause, rule, modeNavig.ParseCode<ModeConsultation>(), contexte, paramGestionDossier, isReadOnly);
            if (typeDoc == AlbConstantesMetiers.WDOC_CLAUSE)
            {
                return HttpContext.Server.UrlEncode(fileFullPath);
            }

            return HttpContext.Server.UrlEncode(fileFullPath);
        }

        [ErrorHandler]
        public string OpenDirectWordDocument(string documentFullPath)
        {
            var finalFileName = string.Empty;

            if (System.IO.File.Exists(documentFullPath))
            {
                finalFileName = documentFullPath;
            }
            else
            {
                var fileName = documentFullPath.Split('\\').Last();
                var environment = AlbOpConstants.ClientWorkEnvironment;

                var newDocFullPath = documentFullPath.Replace(fileName, "") + "Reel\\" + fileName;

                if (System.IO.File.Exists(newDocFullPath))
                {
                    finalFileName = newDocFullPath;
                }
                else
                {
                    throw new AlbFoncException("Le document est introuvable");
                }
            }

            var userAD = System.Web.HttpContext.Current.User.Identity.Name.Trim().ToLower().Split(new[] { '\\' }, StringSplitOptions.None).LastOrDefault();
            var ip = Request.UserHostAddress;
            var idSessionClause = 0;
#if DEBUG
            ip = AlbNetworkInfo.GetIpMachine();
#endif
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = client.Channel;
                idSessionClause = serviceContext.GetFullPathDocument(documentFullPath, ip, userAD);
            }

            return HttpContext.Server.UrlEncode(finalFileName + "-__-" + idSessionClause + "-__-CV");
        }

        [ErrorHandler]
        public ActionResult SaveAsWordDocument(string clauseId)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var Context = client.Channel;
                var fileName = Context.GetClauseFileName(clauseId);
                var clausierPath = MvcApplication.CHEMIN_CLAUSIER;
                var fullPath = clausierPath + "\\" + fileName;
                return File(fullPath, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }

        }

        #endregion
        #region Méthodes privées
        private string GetWordDocumentFilePath(string typeDoc, string wOpenParam, int numAvenant, string resolu, bool clauseLibre, string createClause, string rule, ModeConsultation modeNavig = ModeConsultation.Standard, string contexte = "", string paramGestionDossier = "", string isReadOnly = "False")
        {

            bool clauseResolu;
            bool.TryParse(resolu, out clauseResolu);
            var resFile = string.Empty;
            var err = string.Empty;
            var machineName = AlbNetworkInfo.GetMachineName(Request);
            var userAD = System.Web.HttpContext.Current.User.Identity.Name.Trim().ToLower().Split(new[] { '\\' }, StringSplitOptions.None).LastOrDefault();
            var ip = Request.UserHostAddress;
#if DEBUG
            ip = AlbNetworkInfo.GetIpMachine();
#endif

            switch (typeDoc)
            {
                case AlbConstantesMetiers.WDOC_CLAUSE:
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var Context = client.Channel;

                        var creaClause = !string.IsNullOrEmpty(createClause) && (createClause.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None)[0] == "C");
                        resFile = Context.OpenWordDocument(typeDoc, wOpenParam + MvcApplication.SPLIT_CONST_HTML
                              , MvcApplication.SPLIT_CONST_HTML, clauseResolu, clauseLibre, creaClause, rule, modeNavig, contexte, paramGestionDossier, GetUser(), machineName, ip, userAD
                              , IsModuleGestDocOpen() ? 1 : 0, numAvenant);

                        if (!string.IsNullOrEmpty(createClause) && clauseLibre)
                        {
                            var txtLibreElems = createClause.Split(new[] { MvcApplication.SPLIT_CONST_HTML },
                                    StringSplitOptions.None);
                            if (txtLibreElems[0] == "C" && string.IsNullOrEmpty(resFile))
                            {
                                resFile = "NewWordFile";
                            }
                        }
                    }
                    rule = "C" + rule;
                    break;

                case AlbConstantesMetiers.WDOC_CP:
                    var param = wOpenParam.Split('_');
                    if (param.Length != 3)
                        return null;

                    int iVersion;
                    int.TryParse(param[1], out iVersion);
                    err = "Erreur CP: ";

                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var Context = client.Channel;
                        resFile = Context.GenererCP(param[0].PadLeft(9, ' '), iVersion, param[2], "AFFNOUV"
                            , string.Empty, MvcApplication.STORAGE_PREFIX_DIRECTORY, MvcApplication.STORAGE_MAX_FILES, MvcApplication.STORAGE_NUM_POS_DIRECTORY, GetUser(), machineName, ip, userAD
                            , IsModuleGestDocOpen() ? 1 : 0);
                    }
                    rule = "-__-DVP";
                    break;

                case AlbConstantesMetiers.WDOC_DOC:
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var Context = client.Channel;
                        resFile = Context.OpenWordDocument(typeDoc, wOpenParam + MvcApplication.SPLIT_CONST_HTML, MvcApplication.SPLIT_CONST_HTML,
                            false, false, false, rule, modeNavig, contexte, string.Empty, GetUser(), machineName, ip, userAD
                            , IsModuleGestDocOpen() ? 1 : 0, numAvenant);
                    }
                    if (resFile.ToLower().Contains("err"))
                    {
                        var msgErr = resFile;
                        if (resFile.ToLower().Contains("#docrule#"))
                        {
                            msgErr = resFile.Split(new[] { "#docrule#" }, StringSplitOptions.None)[0];
                        }
                        throw new AlbFoncException(msgErr.Replace("Err : ", ""), true, true);
                    }
                    if (resFile.ToLower().Contains("#docrule#"))
                    {
                        string docRule = resFile.Split(new[] { "#docrule#" }, StringSplitOptions.None)[1];
                        resFile = resFile.Split(new[] { "#docrule#" }, StringSplitOptions.None)[0];
                        if (!string.IsNullOrEmpty(docRule))
                        {
                            if (rule == "P" && !docRule.Contains('P'))
                                docRule += rule;

                            if (rule == "V")
                                docRule = docRule.Replace('M', 'V');
                            rule = "D" + docRule;
                        }
                        else
                        {
                            rule = "D" + rule;
                        }
                    }

                    break;

                case AlbConstantesMetiers.WDOC_BLOC:
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                    {
                        var Context = client.Channel;
                        resFile = Context.UpdDocCPCS(wOpenParam).Trim();
                        rule = "B" + rule;
                    }
                    break;
            }


            //if (string.IsNullOrEmpty(resFile) && typeDoc != AlbConstantesMetiers.WDOC_CP)
            //    throw new AlbFoncException("Impossible d'afficher le document " + err, true, true);
            if (resFile.ToLower().Contains("erreur"))
                throw new AlbFoncException(resFile, true, true);

            if (typeDoc == "Clause")// && clauseLibre)
            {
                var tFileUrl = resFile.Split('\\');
                var fileName = tFileUrl[tFileUrl.Count() - 1];

                var urlXML = MvcApplication.MAGNETIC_URL_XML;
                var fileUrlXml = urlXML + fileName.Split('.')[0] + "\\" + fileName.Split('.')[0] + ".xml";

                return !string.IsNullOrEmpty(rule) ? resFile.Trim() + "-__-" + rule : resFile.Trim();
            }

            if (typeDoc == AlbConstantesMetiers.WDOC_DOC)
                return string.Empty;

            var elmFilePath = resFile.ToLower().Split(new[] { AlbOpConstants.ClientWorkEnvironment.ToLower() }, StringSplitOptions.None);

            string toReturn = elmFilePath.Any() && elmFilePath.Length > 1 ? elmFilePath[1].Replace("\\", "/") : string.Empty;
            return !string.IsNullOrEmpty(rule) ? toReturn.Trim() + "-__-" + rule : toReturn.Trim();



        }
        #endregion
    }
}
