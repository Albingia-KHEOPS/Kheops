using Hexavia.Models;
using Hexavia.Models.EnumDir;
using System.Collections.Generic;

namespace Hexavia.Repository.Interfaces
{
    public interface ILatitudeLogitudeRepository
    {
        List<KGeoloc> GetAllLatLon();

        List<KGeolocCase> GetAllCasesLatLon(string code, string version, string designation, string typeDesignation, string etat, string situation, string branche, string garantie, string departement, string evenement, double? latitude, double? longitude, double? diametre);

        List<KGeolocCase> GetAllCasesLatLonByPartner(int? code, TypePartner type);

        KGeolocCase GetOfferContractLatLon(string NumContract,int NumeroChrono);

        List<KGeolocPartner> GetAllPartnerLatLon(TypePartner typePartner,int? partnerCod, string partnerName, int? partnerDept, string partnerCP, string partnerTown, double? latitude, double? longitude, double? diametre);
        List<Partner> GetPartners(int? code, string name, TypePartner type, int? orias);
        List<Interlocuteur> GetInterlocuteurs(int? code, string name);

        void InsertIntoKGEOLOC(KGeoloc KGEOLOC);

        void InsertIntoKGEOLOC(List<KGeoloc> KGEOLOCList);

        void ClearKGEOLOCTable();

        List<KGeolocCase> ParseActeGestionFromFile(string filePath);
    }
}
