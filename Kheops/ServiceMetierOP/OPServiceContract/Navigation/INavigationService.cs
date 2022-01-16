using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.NavigationArbre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OPServiceContract {
    [ServiceContract]
    public interface INavigationService {
        [OperationContract]
        NavigationArbreDto GetTreeHierarchy(Folder folder, bool isModifHorsAvenant, ModeConsultation modeNavig);
    }
}
