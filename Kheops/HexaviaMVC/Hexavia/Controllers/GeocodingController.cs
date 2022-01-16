using Hexavia.Business.Interfaces;
using Hexavia.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Hexavia.Controllers
{
    public class GeocodingController : BaseController
    {
        private readonly IAdresseBusiness AdresseBusiness;
        private readonly ILatitudeLogitudeBusiness LatitudeLogitudeBusiness;
        private const int TIME_MS_BETWEEM_TWO_CALLS = 1000;
        private const int MAX_NB_CALLS = 1000;
        private const string NOMINATIM_URL = "http://nominatim.openstreetmap.org/";
        private const string PHOTON_URL = "http://photon.komoot.de/api/";
       
        public GeocodingController(IAdresseBusiness adresseBusiness, ILatitudeLogitudeBusiness latitudeLogitudeBusiness)
        {
            AdresseBusiness = adresseBusiness;
            LatitudeLogitudeBusiness = latitudeLogitudeBusiness;
        }

        // GET: Geocoding
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Launch(bool fromStart)
        {
            IList<AdresseLight> adresses = null;
            if (fromStart)
            {
                LatitudeLogitudeBusiness.ClearKGEOLOCTable();
                adresses = AdresseBusiness.GetAllAdresses();
            }
            else
            {
                adresses = AdresseBusiness.GetAllAdressesNotGeolocalisated().OrderByDescending(n => n.NumeroChrono).ToList();
            }

            var listLatLon = LatitudeLogitudeBusiness.GetAllLatLon();

            var nbAddressGeolocalisated = 0;
            foreach (var adresse in adresses)
            {
                if (!fromStart)
                {
                    var alreadyGeolocalisated = listLatLon.Exists(l => l.NumeroChrono == adresse.NumeroChrono);
                    if (alreadyGeolocalisated)
                    {
                        continue;
                    }
                }

                if (nbAddressGeolocalisated == MAX_NB_CALLS)
                {
                    break;
                }

                //recherche sur adresse complete
                if (AdresseCoordonatesAdded(adresse.AdresseComplete4, adresse.NumeroChrono.Value, fromStart))
                {
                    nbAddressGeolocalisated++;
                    Thread.Sleep(TIME_MS_BETWEEM_TWO_CALLS);
                    continue;
                }

                //en cas d'echec on effectue une recherche sur l'adresse sans le cp
                if (AdresseCoordonatesAdded(adresse.AdresseCompleteSansCP4, adresse.NumeroChrono.Value, fromStart))
                {
                    nbAddressGeolocalisated++;
                    Thread.Sleep(TIME_MS_BETWEEM_TWO_CALLS);
                    continue;
                }
            }
            return Json(nbAddressGeolocalisated);
        }

        private bool AdresseCoordonatesAdded(string adresse, int numeroChrono, bool fromStart)
        {
            if (adresse == null)
            {
                return false;
            }

            var latLon = GetLatLon(adresse);
            if (latLon != null)
            {
                LatitudeLogitudeBusiness.InsertIntoKGEOLOC(new KGeoloc
                {
                    NumeroChrono = numeroChrono,
                    Lat = latLon.Lat,
                    Lon = latLon.Lon
                });
                return true;
            }
            return false;
        }

        private LatLong GetLatLon(string adresse)
        {
            if (string.IsNullOrWhiteSpace(adresse))
            {
                return null;
            }

            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            webClient.Headers.Add("Referer", "http://www.microsoft.com");
            var jsonString = webClient.DownloadString(NOMINATIM_URL + "search?q=" + HttpUtility.UrlEncode(adresse) + "&format=json");
             webClient.Dispose();

            //load into memorystream
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var ser = new DataContractJsonSerializer(typeof(PlaceInfo[]));
                var obj = (PlaceInfo[])ser.ReadObject(ms);
                if (obj != null && obj.Length > 0)
                {
                    return new LatLong
                    {
                        Lat = Convert.ToDouble(obj[0].lat, CultureInfo.InvariantCulture),
                        Lon = Convert.ToDouble(obj[0].lon, CultureInfo.InvariantCulture)
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// Test another Geocoding open source server url
        /// TO-DELETE
        /// </summary>
        /// <param name="adresse"></param>
        /// <returns></returns>
        private LatLong GetLatLonWithPhoton(string adresse)
        {
            if (string.IsNullOrWhiteSpace(adresse))
            {
                return null;
            }
            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            webClient.Headers.Add("Referer", "http://www.microsoft.com");
            var jsonString = webClient.DownloadString(PHOTON_URL + "?q=" + HttpUtility.UrlEncode(adresse) + "&limit=1");
            webClient.Dispose();
            //load into memorystream
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var ser = new DataContractJsonSerializer(typeof(RootObject));
                var obj = (RootObject)ser.ReadObject(ms);
                if (obj != null && obj?.features?.Count>0 )
                {
                    
                    return new LatLong
                    {
                        Lat = Convert.ToDouble(obj.features[0].geometry.coordinates[0], CultureInfo.InvariantCulture),
                        Lon = Convert.ToDouble(obj.features[0].geometry.coordinates[1], CultureInfo.InvariantCulture)
                    };
                }
                return null;
            }
        }
    }
    public class PlaceInfo
    {
        public string lat { get; set; }
        public string lon { get; set; }
        public string display_name { get; set; }
    }
    /// <summary>
    /// jSON structure 
    /// </summary>
    public class Geometry
    {
        public List<double> coordinates { get; set; }
        public string type { get; set; }
    }

    public class Properties
    {
        public string city { get; set; }
        public string country { get; set; }
        public string name { get; set; }
        public string postcode { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class RootObject
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }
}