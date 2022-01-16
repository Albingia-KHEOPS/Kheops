using System.ServiceModel;
using System.Collections.Generic;
//using ALBINGIA.Framework.Common.ExcelXmlMap;
using OP.WSAS400.DTO.ExcelDto;
using OPServiceContract.DynamicDreivedType;


namespace OPServiceContract.IS
{
     [ServiceContract]
    public interface IExcelInfoSpecif
     {
         //[OperationContract, ServiceKnownType("GetDynamicType", typeof(DerivedType))]
         [OperationContract]
         List<LigneInfo> GetLignesInfosSection(string branche, string section);
        // [OperationContract, ServiceKnownType("GetDynamicType", typeof(DerivedType))]
         [OperationContract]
         List<LibCodeDto> GetDropdownlist(string sqlRequest, KeyValuePair<string, string>[] hsqlParam);
         [OperationContract, ServiceKnownType("GetDynamicType", typeof(DerivedType))]
         //[OperationContract]
         dynamic LoadData(string xmlParamExcel, string branche, string section, string splitChars, KeyValuePair<string, string>[] hsqlParam);
         [OperationContract]
         bool UpdatedData(string xmlParamExcel,string branche, string section, string spliChar, KeyValuePair<string, string>[] hsqlParam, string strData);
     }
}

