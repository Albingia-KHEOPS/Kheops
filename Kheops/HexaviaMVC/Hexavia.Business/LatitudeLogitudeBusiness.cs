using System.Collections.Generic;
using Hexavia.Business.Interfaces;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using Hexavia.Repository.Interfaces;

namespace Hexavia.Business
{
    public class LatitudeLogitudeBusiness : ILatitudeLogitudeBusiness
    {
        private readonly ILatitudeLogitudeRepository LatitudeLogitudeRepository;
        public LatitudeLogitudeBusiness(ILatitudeLogitudeRepository latitudeLogitudeRepository)
        {
            LatitudeLogitudeRepository = latitudeLogitudeRepository;
        }

        public List<KGeoloc> GetAllLatLon()
        {
            return LatitudeLogitudeRepository.GetAllLatLon();
        }

        //public List<KGeolocCase> GetAllCasesLatLon(string typeDesignation, string etat, string situation, string branche)
        //{
        //    return LatitudeLogitudeRepository.GetAllCasesLatLon(typeDesignation,etat, situation, branche);
        //}

        public KGeolocCase GetOfferContractLatLon(string NumContract, int NumeroChrono)
        {
            return LatitudeLogitudeRepository.GetOfferContractLatLon(NumContract, NumeroChrono);
        }

        public List<KGeolocPartner> GetAllPartnerLatLon(TypePartner typePartner, int? partnerCod, string partnerName, int? partnerDept, string partnerCP, string partnerTown, double? latitude, double? longitude, double? diametre)
        {
            return LatitudeLogitudeRepository.GetAllPartnerLatLon(typePartner,partnerCod, partnerName, partnerDept, partnerCP, partnerTown, latitude, longitude, diametre);
        }
        public List<Partner> GetPartners(int? code, string name, TypePartner type, int? orias)
        {
            return LatitudeLogitudeRepository.GetPartners(code, name , type, orias);
        }
        public List<Interlocuteur> GetInterlocuteurs(int? code, string name)
        {
            return LatitudeLogitudeRepository.GetInterlocuteurs(code, name);
        }

        public void ClearKGEOLOCTable()
        {
            LatitudeLogitudeRepository.ClearKGEOLOCTable();
        }       

        public void InsertIntoKGEOLOC(KGeoloc kgeoloc)
        {
            LatitudeLogitudeRepository.InsertIntoKGEOLOC(kgeoloc);
        }

        public void InsertIntoKGEOLOC(List<KGeoloc> kgeolocList)
        {
            LatitudeLogitudeRepository.InsertIntoKGEOLOC(kgeolocList);
        }

        public void UpdateKGEOLOC(List<KGeoloc> kgeoloc)
        {
            if(kgeoloc == null)
            {
                return;
            }

            InsertIntoKGEOLOC(kgeoloc);
        }
    }
}
