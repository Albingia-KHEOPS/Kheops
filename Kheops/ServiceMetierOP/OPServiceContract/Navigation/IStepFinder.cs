using OP.WSAS400.DTO;
using OPWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OPServiceContract {
    [ServiceContract]
    public interface IStepFinder {
        [OperationContract]
        StepContext FindNext(StepContext stepContext);

        [OperationContract]
        StepContext FindPrevious(StepContext stepContext);

        [OperationContract]
        [FaultContract(typeof(BusinessError))]
        StepContext Find(StepContext stepContext);
    }
}
