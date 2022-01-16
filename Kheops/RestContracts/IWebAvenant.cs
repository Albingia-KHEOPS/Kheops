using OP.WSAS400.DTO.Avenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace RestContracts
{
    [ServiceContract]
    public interface IWebAvenant {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "Confirm")]
        string Confirm(AvenantCreation avenantCreation);

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "CreateAvenant")]
        string[] CreateAvenant(AvenantCreation avenantCreation);
    }
}
