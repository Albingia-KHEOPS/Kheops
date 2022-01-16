using System.ServiceModel;
using OP.WSAS400.DTO.Ecran.ConfirmationSaisie;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Collections.Generic;

namespace OPServiceContract.ISaisieCreationOffre
{
   [ServiceContract]
    public interface IConfirmationSaisie
    {
        [OperationContract]
        ConfirmationSaisieGetResultDto ConfirmationSaisieGet(ConfirmationSaisieGetQueryDto query);
        [OperationContract]
        ConfirmationSaisieSetResultDto ConfirmationSaisieSet(ConfirmationSaisieSetQueryDto query);
        [OperationContract]
        List<ParametreDto> GetListeMotifs();
        [OperationContract]
        void EnregistrerNouvellePosition(string codeOffre, string version, string type, string newEtat, string newSituation, string motif, string user);
    }


}
