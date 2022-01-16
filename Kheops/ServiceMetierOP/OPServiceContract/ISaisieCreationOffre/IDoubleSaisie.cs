using OP.WSAS400.DTO.Ecran.DoubleSaisie;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using System.ServiceModel;

namespace OPServiceContract.ISaisieCreationOffre
{
    [ServiceContract]
    public interface IDoubleSaisie
    {
        [OperationContract]
        DoubleSaisieGetResultDto DoubleSaisieGet(DoubleSaisieGetQueryDto query);
        [OperationContract]
        CabinetCourtageGetResultDto DoubleSaisieCabinetCourtageGet(CabinetCourtageGetQueryDto query);
        [OperationContract]
        DoubleSaisieListeDto ObtenirDoubleSaisieListes(string branche, string cible);
        [OperationContract]
        void EnregistrerDoubleSaisie(CabinetAutreDto cabinet, string user);
        [OperationContract]
        string GetNewVersionOffre(string codeOffre, string version, string type, string user);
    }
}
