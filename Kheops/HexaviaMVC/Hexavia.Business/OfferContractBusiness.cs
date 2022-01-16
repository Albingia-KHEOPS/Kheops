using Hexavia.Business.Interfaces;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using Hexavia.Repository.Interfaces;
using Hexavia.Tools;
using Hexavia.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hexavia.Business
{
    public class OfferContractBusiness : IOfferContractBusiness
    {
        private readonly ILatitudeLogitudeRepository LatitudeLogitudeRepository;
        private readonly IKheopsUrlBusiness KheopsUrlBusiness;
        private readonly int nbMaxContractToDisplay = 5;
        private readonly double RAYON_MARKER = Math.Log10(1.05);// double.Epsilon * Math.Exp(250);

        public OfferContractBusiness(ILatitudeLogitudeRepository latitudeLogitudeRepository,
            IKheopsUrlBusiness kheopsUrlBusiness)
        {
            LatitudeLogitudeRepository = latitudeLogitudeRepository;
            KheopsUrlBusiness = kheopsUrlBusiness;
        }

        public List<LatLong> LoadAffairesAroundGPSPoint(double longitude, double latitude, double diametre, string typeDesignation, string branche)
        {
            var listLatLon = LatitudeLogitudeRepository.GetAllCasesLatLon(null,null,null, typeDesignation, string.Empty, string.Empty, branche, string.Empty, string.Empty, string.Empty, latitude, longitude, diametre);
            var listLatLonInCircle = PickLocationInCercle(longitude, latitude, diametre, listLatLon);
            // List<LatLong> list = PickAffaireBeingInCercle(longitude, latitude, diametre, listLatLon);
            List<LatLong> list = GenerateMarkerListByBrancheDesignation(listLatLonInCircle);
            return list;
        }

        public List<LatLong> LoadAffairesByDesignation(string code, string version, string designation, string typeDesignation, string etat, string situation, string branche, string garantie, string departement, string evenement, DiffDisplayModeBranche displayMode, out int count)
        {
            designation = designation.Trim();
            List<KGeolocCase> listLatLon = LatitudeLogitudeRepository.GetAllCasesLatLon(code,version,designation, typeDesignation, etat, situation, branche, garantie, departement, evenement, null, null, null);
            count = listLatLon.Count;
            return GenerateMarkerListFromDisplayMode(listLatLon, displayMode);
        }

        public List<LatLong> LoadActeGestionFromFile(double longitude, double latitude, double diametre, string filePath)
        {
            List<KGeolocCase> listLatLon = LatitudeLogitudeRepository.ParseActeGestionFromFile(filePath);

            //take only contracts being around the cercle define by diametre
            listLatLon = listLatLon.Where(latlon => Geolocalisation.GetDistanceBetween(latitude, longitude, latlon.Lat, latlon.Lon) <= diametre).ToList();

            return GenerateMarkerListFromDisplayMode(listLatLon, DiffDisplayModeBranche.BrancheDesignation);
        }

        private List<LatLong> GenerateMarkerListFromDisplayMode(List<KGeolocCase> listLatLon, DiffDisplayModeBranche displayModeBranche)
        {
            var list = new List<LatLong>();

            if (displayModeBranche == DiffDisplayModeBranche.BrancheMarker)
                list = GenerateMarkerListByBrancheMarker(listLatLon);
            else
                list = GenerateMarkerListByBrancheDesignation(listLatLon);

            return list;
        }

        private List<LatLong> GenerateMarkerListByBrancheDesignation(List<KGeolocCase> listLatLon)
        {
            var listGrpLatLon = listLatLon.GroupBy(it => new { it.Lon, it.Lat }).ToList();
            var list = new List<LatLong>();

            foreach (var grpLatLon in listGrpLatLon)
            {
                var orderListLatLon = grpLatLon.OrderBy(grp => grp.Branche).ToList();
                string branche = "";
                var newLatLon = new LatLong
                {
                    NumeroChrono = orderListLatLon.First().NumeroChrono,
                    Lat = orderListLatLon.First().Lat,
                    Lon = orderListLatLon.First().Lon,
                    NBContrat = orderListLatLon.Count,
                    Types = orderListLatLon.Where(x=>!string.IsNullOrEmpty(x.TypeAtlas)).Select(x=>x.TypeAtlas).Distinct().ToArray()

                };
                int i = 1;
                foreach (var latlon in orderListLatLon)
                {
                    if (i <= nbMaxContractToDisplay)
                    {
                        newLatLon.Libelle += string.Concat(KheopsUrlBusiness.GetKheopsOfferContractLink(latlon, false), "<hr class='separator'>","</br>");
                        if (i == nbMaxContractToDisplay)
                        {
                            newLatLon.Libelle += "...";
                        }
                    }

                    if (branche != latlon.Branche)
                    {
                        branche = latlon.Branche;
                        if (newLatLon.FullLibelle != string.Empty)
                            newLatLon.FullLibelle += "</ul>";
                        newLatLon.FullLibelle += string.Concat("<b>Branche => ",latlon.Branche, "</b><ul class='list-unstyled'>");
                    }
                    newLatLon.FullLibelle += string.Concat("<li>", KheopsUrlBusiness.GetKheopsOfferContractLink(latlon, false), "<hr class='separator'>","</li>");
                   
                    i++;
                }
                newLatLon.FullLibelle += "</ul>";
                list.Add(newLatLon);

            }




            


            return list;
        }

        private List<LatLong> GenerateMarkerListByBrancheMarker(List<KGeolocCase> listLatLon)
        {
            var list = new List<LatLong>();
            foreach (var latlon in listLatLon)
            {
                var searchLatLon = list.FirstOrDefault(a => (a.Lat == latlon.Lat) && (a.Lon == latlon.Lon) && (a.Branche == latlon.Branche));
                if (searchLatLon != null)
                {
                    searchLatLon.NBContrat++;
                    if (searchLatLon.NBContrat <= nbMaxContractToDisplay)
                    {
                        searchLatLon.Libelle += string.Concat("</br>", KheopsUrlBusiness.GetKheopsOfferContractLink(latlon));
                        if (searchLatLon.NBContrat == nbMaxContractToDisplay)
                        {
                            searchLatLon.Libelle += "</br>...";
                        }
                    }
                    searchLatLon.FullLibelle += string.Concat("</br>", KheopsUrlBusiness.GetKheopsOfferContractLink(latlon));

                }
                else
                {
                    var libelle = string.Concat(KheopsUrlBusiness.GetKheopsOfferContractLink(latlon));
                    var newLatLon = new LatLong
                    {
                        NumeroChrono = latlon.NumeroChrono,
                        Lat = latlon.Lat,
                        Lon = latlon.Lon,
                        NBContrat = 1,
                        Num = string.Concat(latlon.NumContrat, "_", latlon.Version),
                        Ref = latlon.Reference,
                        Libelle = libelle,
                        FullLibelle = libelle,
                        Branche = latlon.Branche,
                        DateSaisie = latlon.DateSaisie,
                        CourtierGestionnaire = latlon.CourtierGestionnaire,
                        AssureGestionnaire = latlon.AssureGestionnaire,
                        Smp = latlon.Smp,
                        Type = latlon.Type
                    };
                    list.Add(newLatLon);
                }
            }

            return SetApproximateAroundGPSPoint(list);
        }

        private List<LatLong> PickAffaireBeingInCercle(double longitude, double latitude, double diametre, List<KGeolocCase> listLatLon)
        {
            var list = new List<LatLong>();

            foreach (var latlon in listLatLon)
            {
                var distance = Geolocalisation.GetDistanceBetween(latitude, longitude, latlon.Lat, latlon.Lon);
                if (distance <= diametre)
                {
                    var searchLatLon = list.FirstOrDefault(a => (a.Lat == latlon.Lat) && (a.Lon == latlon.Lon));
                    if (searchLatLon == null)
                    {
                        var libelle = string.Concat(KheopsUrlBusiness.GetKheopsOfferContractLink(latlon));
                        var newLatLon = new LatLong
                        {
                            NumeroChrono = latlon.NumeroChrono,
                            Lat = latlon.Lat,
                            Lon = latlon.Lon,
                            NBContrat = 1,
                            Num = string.Concat(latlon.NumContrat, "_", latlon.Version),
                            Ref = latlon.Reference,
                            Libelle = libelle,
                            FullLibelle = libelle
                        };
                        list.Add(newLatLon);
                    }
                    else
                    {
                        searchLatLon.NBContrat++;
                        var libelle = string.Concat("<hr class='separator'>", KheopsUrlBusiness.GetKheopsOfferContractLink(latlon));
                        if (searchLatLon.NBContrat <= nbMaxContractToDisplay)
                        {
                            searchLatLon.Libelle += libelle;
                            if (searchLatLon.NBContrat == nbMaxContractToDisplay)
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

        private List<KGeolocCase> PickLocationInCercle(double longitude, double latitude, double diametre, List<KGeolocCase> listLatLon)
        {
            var list = new List<KGeolocCase>();
            foreach (var latlon in listLatLon)
            {
                var distance = Geolocalisation.GetDistanceBetween(latitude, longitude, latlon.Lat, latlon.Lon);
                if (distance <= diametre)
                {
                    list.Add(latlon);
                }
            }
            return list;
        }

        private List<LatLong> SetApproximateAroundGPSPoint(List<LatLong> markers)
        {
            var grpMarker = markers.GroupBy(grpM => new { grpM.Lat, grpM.Lon }).ToList();
            List<LatLong> listApproximatedMarkerPoints = new List<LatLong>();
            foreach(var itGrp in grpMarker)
            {
                int nbMarkerInPerimeter = itGrp.Count();
                double angleAmplitude = 2 * Math.PI / nbMarkerInPerimeter;
                int i = 0;
                if (nbMarkerInPerimeter > 1)
                {
                    foreach(var marker in itGrp)
                    {

                        listApproximatedMarkerPoints.Add(new LatLong
                        {
                            NumeroChrono = marker.NumeroChrono,
                            Lon = marker.Lon + RAYON_MARKER * Math.Cos(angleAmplitude * i),
                            Lat = marker.Lat + RAYON_MARKER * Math.Sin(angleAmplitude * i),
                            NBContrat = marker.NBContrat,
                            Num = marker.Num,
                            Ref = marker.Ref,
                            Libelle = marker.Libelle,
                            FullLibelle = marker.FullLibelle,
                            Branche = marker.Branche,
                            DateSaisie = marker.DateSaisie,
                            CourtierGestionnaire = marker.CourtierGestionnaire,
                            AssureGestionnaire = marker.AssureGestionnaire
                        });
                        i++;
                    }
                }
                else
                {
                    listApproximatedMarkerPoints.Add(itGrp.First());
                }
            }

            return listApproximatedMarkerPoints;
        }

        public List<LatLong> SearchCasesLatLonByPartner(int? code, TypePartner type)
        {
            List<KGeolocCase> listLatLon = LatitudeLogitudeRepository.GetAllCasesLatLonByPartner(code, type);

            return GenerateMarkerListByBrancheDesignation(listLatLon);
          //  return GenerateMarkerListFromDisplayMode(listLatLon, DiffDisplayModeBranche.BrancheMarker);
        }

    }
}
