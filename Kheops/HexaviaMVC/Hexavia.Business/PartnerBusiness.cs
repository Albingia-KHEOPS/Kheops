using Hexavia.Business.Interfaces;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using Hexavia.Repository;
using Hexavia.Tools;
using Hexavia.Tools.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Hexavia.Business
{
    public class PartnerBusiness : IPartnerBusiness
    {
        private readonly ILatitudeLogitudeBusiness LatitudeLogitudeBusiness;
        private readonly IGrpUrlBusiness GrpBusiness;
        private readonly int NbMaxInsuredToDisplay = 5;

        public PartnerBusiness(ILatitudeLogitudeBusiness latitudeLogitudeBusiness, IGrpUrlBusiness grpBusiness)
        {
            LatitudeLogitudeBusiness = latitudeLogitudeBusiness;
            GrpBusiness = grpBusiness;
        }

        public List<LatLong> LoadPartnersAroundGPSPoint(TypePartner typePartner, double longitude, double latitude, double diametre)
        {
            List<KGeolocPartner> listLatLon = null;
            if (typePartner == TypePartner.Assure)
            {
                listLatLon = LatitudeLogitudeBusiness.GetAllPartnerLatLon(TypePartner.Assure, null, null, null, null, null, latitude, longitude, diametre);
            }
            else if (typePartner == TypePartner.Courtier)
            {
                listLatLon = LatitudeLogitudeBusiness.GetAllPartnerLatLon(TypePartner.Courtier, null, null, null, null, null, latitude, longitude, diametre);
            }
            else
            {
                listLatLon = LatitudeLogitudeBusiness.GetAllPartnerLatLon(TypePartner.Expert, null, null, null, null, null, latitude, longitude, diametre);
            }

            var list = new List<LatLong>();

            foreach (var latlon in listLatLon)
            {
                var distance = Geolocalisation.GetDistanceBetween(latitude, longitude, latlon.Lat, latlon.Lon);
                if (distance <= diametre)
                {
                    var searchLatLon = list.FirstOrDefault(a => (a.Lat == latlon.Lat) && (a.Lon == latlon.Lon));
                    if (searchLatLon == null)
                    {
                        var libelle = string.Concat(GrpBusiness.GetGrpPartnerLink(typePartner, latlon), latlon.Adresse);
                        var newLatLon = new LatLong
                        {
                            NumeroChrono = latlon.NumeroChrono,
                            Lat = latlon.Lat,
                            Lon = latlon.Lon,
                            NBContrat = 1,
                            Num = latlon.Num,
                            Libelle = libelle,
                            FullLibelle = libelle
                        };
                        list.Add(newLatLon);
                    }
                    else
                    {
                        searchLatLon.NBContrat++;
                        var libelle = string.Concat("</br>", GrpBusiness.GetGrpPartnerLink(typePartner, latlon), latlon.Adresse);
                        if (searchLatLon.NBContrat <= NbMaxInsuredToDisplay)
                        {
                            searchLatLon.Libelle += libelle;
                            if (searchLatLon.NBContrat == NbMaxInsuredToDisplay)
                            {
                                searchLatLon.Libelle += "</br>...";
                            }
                        }
                        searchLatLon.FullLibelle += libelle;
                    }
                }
            }
            return list;
        }

        public List<LatLong> SearchPartners(TypePartner typePartner, int? partnerCod, string partnerName, int? partnerDept, string partnerCP, string partnerTown)
        {
            List<KGeolocPartner> listLatLon = LatitudeLogitudeBusiness.GetAllPartnerLatLon(typePartner, partnerCod, partnerName, partnerDept, partnerCP, partnerTown, null, null, null);
            var list = new List<LatLong>();

            foreach (var latlon in listLatLon)
            {
                var searchLatLon = list.FirstOrDefault(a => (a.Lat == latlon.Lat) && (a.Lon == latlon.Lon));
                if (searchLatLon == null)
                {
                    var libelle = string.Concat(GrpBusiness.GetGrpPartnerLink(typePartner, latlon), latlon.Adresse);
                    var newLatLon = new LatLong
                    {
                        NumeroChrono = latlon.NumeroChrono,
                        Lat = latlon.Lat,
                        Lon = latlon.Lon,
                        NBContrat = 1,
                        Num = latlon.Num,
                        Libelle = libelle,
                        FullLibelle = libelle
                    };
                    list.Add(newLatLon);
                }
                else
                {
                    searchLatLon.NBContrat++;
                    var libelle = string.Concat("</br>", GrpBusiness.GetGrpPartnerLink(typePartner, latlon), latlon.Adresse);
                    if (searchLatLon.NBContrat <= NbMaxInsuredToDisplay)
                    {
                        searchLatLon.Libelle += libelle;
                        if (searchLatLon.NBContrat == NbMaxInsuredToDisplay)
                        {
                            searchLatLon.Libelle += "</br>...";
                        }
                    }
                    searchLatLon.FullLibelle += libelle;
                }
            }
            return list;
        }

        public Partner GetPartnerByCode(int? code ,TypePartner type)
        {         
            return LatitudeLogitudeBusiness.GetPartners(code, null, type, null).FirstOrDefault(); 
        }
        public Partner GetPartnerByOrias(int? orias)
        {
            return LatitudeLogitudeBusiness.GetPartners(null, null, TypePartner.Courtier, orias).FirstOrDefault();
        }
        public List<Partner> GetPartnerByNamePrefix(string name, TypePartner type)
        {         
            return LatitudeLogitudeBusiness.GetPartners(null, name, type, null); 
        }
        public List<Interlocuteur> GetInterlocuteurByNamePrefix(string name, int? code)
        {
            return LatitudeLogitudeBusiness.GetInterlocuteurs(code, name);
        }
    }
}
