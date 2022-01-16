using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Hexavia.Console.Configurations
{
    class GeocodeAppSettings
    {
        public static bool ResetGeocoding { get; } = bool.Parse(ConfigurationManager.AppSettings["ResetGeocoding"] ?? "false");
        public static bool UseGoogleMaps { get; } = bool.Parse(ConfigurationManager.AppSettings["UseGoogleMaps"] ?? "false");
        public static List<string> ToMails { get; } = ConfigurationManager.AppSettings["ToMails"].Split(';').ToList();
        public static string UseGoogleMapsAPI = ConfigurationManager.AppSettings["UseGoogleMapsAPI"];
        public static string key = ConfigurationManager.AppSettings["GoogleMapsKey"];
        public static string Url = ConfigurationManager.AppSettings["Url"];
        public static string LogPath = ConfigurationManager.AppSettings["LogPath"];
        public static string LogFileName = ConfigurationManager.AppSettings["LogFileName"];
        public static string GetLogPathByFileName(DateTime logStartDate)
        {
            var fileName = $"{LogFileName}_{logStartDate.ToString("yyyyMMddhhmmss")}.txt";
            return Path.Combine(LogPath , fileName);
        }

    }
}
