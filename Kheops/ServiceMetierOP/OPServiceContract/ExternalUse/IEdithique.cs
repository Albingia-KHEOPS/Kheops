using System.ServiceModel;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.FormuleGarantie;
using System.Collections.Generic;

namespace OPServiceContract.ExternalUse
{
    [ServiceContract]
    public interface IEdithique
    {
        [OperationContract]
        FormGarDto GetFormuleGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule,
            string codeOption, string formGen, string codeCategorie, string codeAlpha, string branche, string libFormule,
            string user, int appliqueA);
        [OperationContract]
        bool SetCoordinateAdr();

        [OperationContract]
        List<BandeauDto> GetGeo(string numOffre, string type, int version, int perimetre, string departement);

        [OperationContract]
        List<BandeauDto> GetGeoInRectangle(double eastLatitude, double eastlongitude, double southWestLatitude,
            double southWestLongitude);
       

    }
}
