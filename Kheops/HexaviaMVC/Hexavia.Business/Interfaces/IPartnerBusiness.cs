using Hexavia.Models;
using Hexavia.Models.EnumDir;
using System.Collections.Generic;

namespace Hexavia.Business.Interfaces
{
    public interface IPartnerBusiness
    {
        List<LatLong> SearchPartners(TypePartner typePartner,int? partnerCod, string partnerName, int? partnerDept, string partnerCP, string partnerTown);

        List<LatLong> LoadPartnersAroundGPSPoint(TypePartner typePartner, double longitude, double latitude, double diametre);
        Partner GetPartnerByCode(int? code, TypePartner type);
        Partner GetPartnerByOrias(int? orias);
        List<Partner> GetPartnerByNamePrefix(string name, TypePartner type);
        List<Interlocuteur> GetInterlocuteurByNamePrefix(string name, int? code);
    }
}
