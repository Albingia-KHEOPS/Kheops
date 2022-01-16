using System.Collections.Generic;
using System.ServiceModel;
using ALBINGIA.Framework.Common.Models.FileModel;

namespace OPServiceContract.Print
{
    [ServiceContract]
    public interface IPrintJob
    {
        [OperationContract]
        List<FileDescription> GetParamDocument(string idDoc);

    }
}
