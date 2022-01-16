using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using OPWebService;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Albingia.Kheops.OP.Application.Port.Driver {
    [ServiceContract]
    public interface IInfosSpecifiquesPort {
        [OperationContract]
        InfosSpecifiquesDto GetByAffaireAndSection(AffaireId affaireId, SectionISDto section);
        [OperationContract]
        bool HasModeleLignes(AffaireId affaireId, SectionISDto section);
        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        void SaveIS(InfosSpecifiquesDto infos);
        [OperationContract]
        IEnumerable<(AffaireId affaire, SectionISDto section)> GetExistingOldIS(int maxResults = 1000, bool fromHisto = false);
        [OperationContract]
        void TraceOldISTransfert(AffaireId affaireId, SectionISDto section, string commantaires);
        [OperationContract]
        bool HasOldISTransfertLogId(Guid guid);
        [OperationContract]
        void CancelOldISTransfert(Guid initGuid);
    }
}
