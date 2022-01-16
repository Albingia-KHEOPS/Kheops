using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Services;
using OP.Services.Excel;
using OP.WSAS400.DTO.ExcelDto;
using OPServiceContract.IExcelInfo;

namespace OP.IOWebService.ExcelInfo
{
    /// <summary>
    /// Summary description for GenericIS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GenericIS : System.Web.Services.WebService
    {

       [WebMethod]
        public List<LigneInfo> GetLignesInfosSection(string branche, string section)
        {
            
            return (new BrancheRS()).GetLignesInfosSection(branche, section);
        }

  [WebMethod]
        public List<LibCodeDto> GetDropdownlist(string sqlRequest, List<KeyValuePair<string, string>> hsqlParam)
        {
            return (new BrancheRS()).GetDropdownlist(sqlRequest, hsqlParam);
            
        }
       [WebMethod]
        public dynamic LoadData(string xmlParamExcel, string branche, string section, string splitChars, KeyValuePair<string, string>[] hsqlParam)
        {
            return (new BrancheRS()).LoadData(xmlParamExcel, branche, section, splitChars, hsqlParam.ToList());

        }

        [WebMethod]
        public bool UpdatedData(string xmlParamExcel, string branche, string section, string spliChar, List<KeyValuePair<string, string>> hsqlParam, string strData)
        {
            return (new BrancheRS()).UpdatedData(xmlParamExcel, branche, section, spliChar, hsqlParam, strData);
        }
    }
}
