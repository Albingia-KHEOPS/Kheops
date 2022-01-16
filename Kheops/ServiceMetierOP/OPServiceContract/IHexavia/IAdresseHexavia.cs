using System.Collections.Generic;
using System.ServiceModel;
using OP.WSAS400.DTO.Inventaires;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OPServiceContract.IHexavia
{
    [ServiceContract]
    public interface IAdresseHexavia
    {
        [OperationContract]
        List<ParametreDto> GetVillesByCP(string codePostal);
        [OperationContract]
        List<ParametreDto> GetCPByVille(string ville);
        [OperationContract]
        List<CPVilleDto> GetCodePostalVille(string value, string mode);

        [OperationContract]
        List<(string nom, int cp)> SearchVilleByCP(int codePostal, bool matchAnywhere = false);
    }
}
