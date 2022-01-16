using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ALBINGIA.Framework.Common.Tools
{
    //public interface ILongitudeLatitude
    //{
    //    decimal Latitude { get; set; }
    //    decimal Longitude { get; set; }
    //}
    //public struct GoogleCoordinate : ILongitudeLatitude
    //{
    //    private decimal _latitude; 
    //    private decimal _longitude;

    //    public GoogleCoordinate(decimal latitude, decimal longitude)
    //    {
    //        _latitude = latitude;
    //        _longitude = longitude; 
    //    }

    //    #region Membre ILongitudeLatitude
    //    public decimal Latitude
    //    {
    //        get
    //        {
    //            return _latitude;
    //        }
    //        set
    //        {
    //            _latitude=value;
    //        }
    //    }

    //    public decimal Longitude
    //    {
    //        get
    //        {
    //            return _longitude;
    //        }
    //        set
    //        {
    //             _longitude=value;
    //        }
    //    }
    //    #endregion
    //}

    //public static class Geocode
    //{
    //    private const string _googleUri = "http://maps.google.com/maps/geo?q=";
    //    //private const string _googleUri = "http://maps.googleapis.com/maps/api/geocode/json?parameters";
    //    private const string _googleKey = "AIzaSyBCjt9xrkelXgNj-jCQWq0_iukMQaGiQlU"; //le retour du site  http://www.google.com/apis/maps/signup.html
    //    private const string _outputType = "csv"; // options: csv, xml, kml, json

    //    private static Uri GetGeocodeUri(string address)
    //    {
    //        address = HttpUtility.UrlEncode(address);
    //        //return new Uri(String.Format("{0}{1}&output={2}&key={3}", _googleUri, address, _outputType, _googleKey));
    //        return new Uri(String.Format("{0}{1}&output={2}", _googleUri, address, _outputType));
    //        //return new Uri(String.Format("{0}{1}&sensor=true", _googleUri, address));
    //    }

    //    /// <summary>
    //    /// Gets a Coordinate from a address.
    //    /// </summary>
    //    /// <param name="address">An address.
    //    /// </param>
    //    /// <returns>A spatial coordinate that contains the latitude and longitude of the address.</returns>
    //    public static GoogleCoordinate GetCoordinates(string address)
    //    {
    //        var client = new WebClient();
    //        var uri = GetGeocodeUri(address);
    //        /*  The first number is the status code, 
    //         * the second is the accuracy, 
    //         * the third is the latitude, 
    //         * the fourth one is the longitude.
    //         */
    //        var geocodeInfo = client.DownloadString(uri).Split(',');

    //        return new GoogleCoordinate(Convert.ToDecimal(geocodeInfo[2]), Convert.ToDecimal(geocodeInfo[3]));
    //    }

    //}

    public class GeoCode:IDisposable
    {
        private WebClient _client;

        public GeoCode()
        {
            _client = new WebClient();
        }

        public Location GeocodeLocation(string address)
        {
            var url = "http://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address=" + HttpUtility.UrlEncode(address);

            var xmlString = _client.DownloadString(url);
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(xmlString);
            if (xmlDoc.SelectSingleNode("//GeocodeResponse/status").InnerText == "ZERO_RESULTS")
            {
                return new Location { Latitude = 0, Longitude = 0 };
            }
            if (xmlDoc.SelectSingleNode("//GeocodeResponse/status").InnerText == "OVER_QUERY_LIMIT")
            {
                //return new Location { Latitude = 0, Longitude = 0 };
                //Task.Run(() =>
                //{
                //    xmlString = _client.DownloadString(url);
                //    xmlDoc = new System.Xml.XmlDocument();
                //    xmlDoc.LoadXml(xmlString);
                //});
                var tst = string.Empty;

            }
            //
            var loc = new Location();
            loc.Latitude = Double.Parse(xmlDoc.SelectSingleNode("//geometry/location/lat").InnerText, CultureInfo.GetCultureInfo("en-US").NumberFormat);
            loc.Longitude = double.Parse(xmlDoc.SelectSingleNode("//geometry/location/lng").InnerText, CultureInfo.GetCultureInfo("en-US").NumberFormat);

            return loc;
        }

        //public Task GeocodeLocationAsync(string address) {
        //  return null;
        //}

        public void Dispose()
        {
            _client.Dispose();
        }
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
