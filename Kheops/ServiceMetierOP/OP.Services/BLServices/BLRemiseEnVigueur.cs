using OP.DataAccess;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.PGM;
using System.Linq;


namespace OP.Services.BLServices
{
    public class BLRemiseEnVigueur
    {
        public static RemiseEnVigueurDto InitializeRemiseEnVigueurParameters(string codeOfrre, int version, string type)
        {
            var initParameter = new RemiseEnVigueurParams() { Result = string.Empty, CodeContrat = codeOfrre, Version = (short)version, Type = type };

            var result = RemiseEnVigueurRepository.CallKDA196(initParameter);

            return result;
        }
    }
}
