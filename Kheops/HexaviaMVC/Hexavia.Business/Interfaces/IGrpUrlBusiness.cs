using Hexavia.Models;
using Hexavia.Models.EnumDir;
using System;

namespace Hexavia.Business.Interfaces
{
    public interface IGrpUrlBusiness
    {
        string GetGrpPartnerLink(TypePartner type, KGeolocPartner kGeolocPartner);

        string GetGrpPartnerUrl(TypePartner type, string num);
    }
}
