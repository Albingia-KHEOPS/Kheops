using System.Collections.Generic;
using System.ServiceModel;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.ParamIS;
using OPServiceContract.DynamicDreivedType;

namespace OPServiceContract.IS
{
  [ServiceContract]
  public interface IInfoSpecif
  {
    [OperationContract]
    List<AffichageISLineDto> GetISDisplayConditions(string codeOffre, string type, string version, string modeleId,
      ParametreGenClauseDto parmIS, out bool isError);
    [OperationContract]
    List<ParamISLigneInfoDto> GetParamISLignesInfo(string modeleId);
    [OperationContract]
    List<DtoCommon> GetDropdownlist(string sqlRequest, KeyValuePair<string, string>[] hsqlParam);
    [OperationContract]
    bool IsDataISExist(string sqlExist, KeyValuePair<string, string>[] param);
     [OperationContract]
    List<ModeleISDto> GetParamEntetModIs(string modeleId);
     
    [OperationContract, ServiceKnownType("GetDynamicType", typeof (DerivedType))]
    dynamic LoadData(string modeNavig,string codeOffre, string version, string type, ParametreGenClauseDto parmClause, string modeleId,
     string section, string splitChars, KeyValuePair<string
       , string>[] hsqlParam, string idModel, List<ModeleISDto> paramBranche);

      [OperationContract]
      bool RowsIsExist(string section, KeyValuePair<string, string>[] hsqlParam, List<ModeleISDto> paramBranche);
    [OperationContract]
    bool UpdatedData(string modeleId, string section, string spliChar,
                     KeyValuePair<string, string>[] hsqlParam, string strData);
    [OperationContract]
    bool InitISCache();


        [OperationContract]
        Dictionary<string, string> GetISDefaultValueData(List<string> parametersIS);
  }
}
