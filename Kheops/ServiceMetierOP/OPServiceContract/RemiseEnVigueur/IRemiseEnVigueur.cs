using OP.WSAS400.DTO;
using System.ServiceModel;

namespace OPServiceContract {
    [ServiceContract]
    public interface IRemiseEnVigueur
    {
        [OperationContract]
        RemiseEnVigueurDto InitializeRemiseEnVigueurParameters(string codeOfrre, int version, string type);
    }
}