using System.Collections.Generic;
using System.ServiceModel;
using ALBINGIA.Framework.Common.Constants;
using OPServiceContract.DynamicDreivedType;


namespace OPServiceContract.OffreSimple
{
  [ServiceContract]
  [ServiceKnownType(typeof(SimpleFolderState))]
  public interface IOffreSimplifieExcel
  {
    [OperationContract]
    SimpleFolderState EditSimpleFolder(string xmlParamExcel, string typeInfo, string branche,
                            List<KeyValuePair<string, string>> hsqlParam);
    [OperationContract]
    List<OP.WSAS400.DTO.ExcelDto.LigneInfo> GetLignesInfos(string typeInfo, string branche);
    [OperationContract, ServiceKnownType("GetDynamicType", typeof (DerivedType))]
    string LoadData(string xmlParamExcel, string typeInfo, string branche, bool nouvelleVersion,
                            SimpleFolderInfoData lineInfoType, string splitChars,
                            List<KeyValuePair<string, string>> hsqlParam, string user);
    [OperationContract]
    bool UpdatedData(string xmlParamExcel, string typeInfo, string branche, string spliChar,
                            List<KeyValuePair<string, string>> hsqlParam, string strData, string user);
  }
}
