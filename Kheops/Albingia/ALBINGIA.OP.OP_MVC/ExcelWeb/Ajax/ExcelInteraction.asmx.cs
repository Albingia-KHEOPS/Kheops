using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.OP.OP_MVC.ExcelWeb.Common;
using System;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
namespace ALBINGIA.OP.OP_MVC.ExcelWeb.Ajax
{
    /// <summary>
    /// Summary description for AjaxExcelInteaction
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ExcelInteraction : WebService
    {
      [WebMethod(EnableSession = true), ScriptMethod]
      public string LoadDbDataToExcel(string branche, string tabGuid, string user, string nouvelleVersion, string splitChars, string strParameters)
      {
        try
        {
          var parameters = ExcelIOParam.GetParams(HttpUtility.UrlDecode(strParameters), HttpUtility.UrlDecode(splitChars));
          return ExcelDataEntity.GetExcelData(HttpUtility.UrlDecode(branche),HttpUtility.UrlDecode(nouvelleVersion)=="1", parameters, user, tabGuid);
        }
        catch (Exception ex)
        {
          
          throw new AlbTechException(ex);
        }
     
        
        //Appel de la méthode de génèration HTML
        //List<LigneInfo> lignesInfo = null;
        //List<LigneInfo> dbLigneInfo = ExcelIOParam.GetXmlParameters(branche);
        //string html = string.Empty;
        //if (lignesInfo != null)
        //{
        //  html = new GenerationHTML(HttpUtility.UrlDecode(branche), new List<LigneInfo>(dbLigneInfo), dataToMap, MvcApplication.SPLIT_CONST_HTML, "||", HttpUtility.UrlDecode(strParameters)).Generate(HttpUtility.UrlDecode(branche), HttpUtility.UrlDecode(section));
        //}

        //return html;
      }



      [WebMethod, ScriptMethod]
      public string SaveData(string branche, string dataToSave, string splitChars, string strParameters)
     // public string SaveData(string branche, string splitChars, string strParameters)
      {
        if (!AlbSessionHelper.IsReadOnly())
        {
          var parameters = ExcelIOParam.GetParams(HttpUtility.UrlDecode(strParameters), HttpUtility.UrlDecode(splitChars));
          //Format de données reçus :val1||cells1#**#val2||cells2#**#.....
          return ExcelDataEntity.SetExcelData(HttpUtility.UrlDecode(branche), HttpUtility.UrlDecode(dataToSave), parameters, AlbSessionHelper.ConnectedUser) ? "ok" : "ko";
        }
        return "ok";
      }
    }
}
