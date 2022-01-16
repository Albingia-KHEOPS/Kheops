using Albingia.Kheops.DTO;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driver {
    [ServiceContract]
    public interface ISinistrePort {
        [OperationContract]
        PagingList<SinistreDto> GetSinistres(int page = 0, int codeAssure = 0);
        [OperationContract]
        IEnumerable<SinistreDto> GetSinistresPreneur(int codePreneur);
        [OperationContract]
        decimal GetTotalChargementsSinistresPreneur(int codePreneur);
    }
}
