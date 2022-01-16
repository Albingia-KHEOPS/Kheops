using System;
namespace Hexavia.Tools.Helpers
{
    /// <summary>
    /// AppSettingsHelper
    /// </summary>
    public class AppSettingsHelper
    {
        /// <summary>
        /// Google maps key
        /// </summary>
        public static string GoogleMapsKey { get; } = System.Configuration.ConfigurationManager.AppSettings["GoogleMapsKey"];
        /// <summary>
        /// Google maps key Geocode
        /// </summary>
        public static string GoogleMapsKeyGeocode { get; } = System.Configuration.ConfigurationManager.AppSettings["GoogleMapsKeyGeocode"];
        
        /// <summary>
        /// Use google maps by default value is true
        /// </summary>
        public static bool UseGoogleMaps { get; } = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseGoogleMaps"]??"true");

        public static string BIGarantieUrl { get; } = System.Configuration.ConfigurationManager.AppSettings["BIGarantieUrl"];

        public static string GenerateGoogleMapsBaseLink()
        {
            return $"https://maps.googleapis.com/maps/api/js?key={GoogleMapsKey}";
        }

        public static string GenerateGoogleMapsGeocodeLink()
        {
            return $"https://maps.googleapis.com/maps/api/geocode/json?key={GoogleMapsKeyGeocode}";
        }
    }
}
