using OP.Services.BLServices;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.PGM;
using OPServiceContract;
using System.ServiceModel.Activation;

namespace OP.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RemiseEnVigueur : IRemiseEnVigueur
    {
        public RemiseEnVigueurDto InitializeRemiseEnVigueurParameters(string codeOfrre, int version, string type)
        {
            return BLRemiseEnVigueur.InitializeRemiseEnVigueurParameters(codeOfrre, version, type);
        }
    }
}
