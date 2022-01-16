using Autofac;
using Hexavia.Console.Configurations;
using Hexavia.Tools.Helpers;
using System;

namespace Hexavia.Console
{
    class Program
    {
        private static void DisplyHeader()
        {
            System.Console.WriteLine("===== HEXAVIA CONSOLE =====");
            System.Console.WriteLine("===== GEOCODING =====");
            System.Console.WriteLine($"Use google maps API : {GeocodeAppSettings.UseGoogleMaps}");
        }
      
        static void Main(string[] args)
        {
            ////Display app information 
             DisplyHeader();
            //Execute geocoding
            BuildContainer.CompositionRoot().Resolve<GeoCoding>().Run();
           
        }
    }
}
