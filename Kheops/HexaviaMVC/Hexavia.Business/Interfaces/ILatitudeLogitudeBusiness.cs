using Hexavia.Models;
using Hexavia.Models.EnumDir;
using System.Collections.Generic;

namespace Hexavia.Business.Interfaces
{
    public interface ILatitudeLogitudeBusiness
    {
        List<KGeoloc> GetAllLatLon();

        //List<KGeolocCase> GetAllCasesLatLon(string typeDesignation, string etat, string situation, string branche);

        KGeolocCase GetOfferContractLatLon(string NumContrat, int NumeroChrono);

        List<KGeolocPartner> GetAllPartnerLatLon(TypePartner typePartner,int? partnerCod, string partnerName, int? partnerDept, string partnerCP,string partnerTown, double? latitude, double? longitude, double? diametre);
        List<Partner> GetPartners(int? code, string name, TypePartner type, int? orias);

        List<Interlocuteur> GetInterlocuteurs(int? code, string name);

        void UpdateKGEOLOC(List<KGeoloc> kgeoloc);

        void InsertIntoKGEOLOC(KGeoloc kgeoloc);

        void InsertIntoKGEOLOC(List<KGeoloc> kgeoloc);

        void ClearKGEOLOCTable();      
    }
}
