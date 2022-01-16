using OP.WSAS400.DTO.Ecran.ModifierOffre;
using System.ServiceModel;

namespace OPServiceContract.ISaisieCreationOffre
{
    [ServiceContract]
    public interface ISaisieOffre
    {
        [OperationContract]
        ModifierOffreGetResultDto ModifierOffreGet(ModifierOffreGetQueryDto query);
             
        [OperationContract]
        string ModifierOffreSet(ModifierOffreSetQueryDto query, string utilisateur);
    }
}
