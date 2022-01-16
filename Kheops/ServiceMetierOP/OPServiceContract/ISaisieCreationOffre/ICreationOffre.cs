using OP.WSAS400.DTO.Ecran.CreationSaisie;
using System.ServiceModel;
using OP.WSAS400.DTO.Offres;
using ALBINGIA.Framework.Common.Constants;

namespace OPServiceContract.ISaisieCreationOffre
{
    [ServiceContract]
    public interface ICreationOffre
    {

        [OperationContract]
        CreationSaisieGetResultDto CreationSaisieGet(CreationSaisieGetQueryDto query);
        //[OperationContract]
        //CreationSaisieSetResultDto CreationSaisieSet(CreationSaisieSetQueryDto query);

        [OperationContract]
        CreationSaisieSetResultDto CreationSaisieEnregistrement(CreationSaisieSetQueryDto query, string user);

        [OperationContract]
        string GetBrancheCibleOffre(string codeOffre, string version, string type, ModeConsultation modeNavig);

        [OperationContract]
        OffreDto GetInfoTemplate(string idTemp);//, string idCible);

        [OperationContract]
        void SaveInfoBase(OffreDto offre);
    }
}
