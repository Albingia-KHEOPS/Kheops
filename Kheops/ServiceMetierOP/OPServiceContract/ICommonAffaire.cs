using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using OP.WSAS400.DTO.Avenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OPServiceContract {
    [ServiceContract]
    public interface ICommonAffaire {
        [OperationContract]
        List<AvenantAlerteDto> GetListAlertesAvenant(AffaireId affaireId);
    }
}
