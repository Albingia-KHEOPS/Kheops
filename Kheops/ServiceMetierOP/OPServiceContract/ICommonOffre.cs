using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using OP.WSAS400.DTO.NavigationArbre;

namespace OPServiceContract.ICommon
{
    [ServiceContract]
    public interface ICommonOffre
    {
        [OperationContract]
        NavigationArbreDto GetHierarchy(string codeOffre, int version, string type, string utilisateur);

        [OperationContract]
        void SetTrace(TraceDto trace);
    }
}
