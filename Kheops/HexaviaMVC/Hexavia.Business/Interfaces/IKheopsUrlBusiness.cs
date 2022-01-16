using Hexavia.Models;

namespace Hexavia.Business.Interfaces
{
    public interface IKheopsUrlBusiness
    {
        string GetKheopsOfferContractLink(KGeolocCase kGeoloc, bool setLinkWithSpecificColor = false);

        string GetKheopsContractUrl(KGeolocCase KGeoloc);
    }
}
