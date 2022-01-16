using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OP.WSAS400.DTO.Ecran.ConfirmationSaisie;

namespace OP.WSAS400
{
    [ServiceContract]
    public interface IScreens
    {

        [OperationContract]
        ConfirmationSaisieGetResultDto ConfirmationSaisieGet(ConfirmationSaisieGetQueryDto query);
    }
}
