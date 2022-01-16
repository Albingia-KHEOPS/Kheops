using System.Collections.Generic;
using System.ServiceModel.Activation;
using ALBINGIA.Framework.Common.Models.FileModel;
using OP.DataAccess;
using OPServiceContract.Print;

namespace OP.Services.Print
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PrintJob : IPrintJob
    {

        public List<FileDescription> GetParamDocument(string idDoc)
        {
            return SuiviDocumentsRepository.GetParamDocument(idDoc);
        }


     
    }
}
