using System;

namespace Hexavia.Tools
{
    public class Geolocalisation
    {
        const double kEarthRadiusKms = 6376.5;
        ///<summary>
        /// Get distance between 2 points in kilometers
        ///</summary>
        ///<param name="latitudePointA">Latitute point A</param>
        ///<param name="longitudePointA">Longitute point A</param>
        ///<param name="latitudePointB">Latitude point B</param>
        ///<param name="longitudePointB">Longitude point B</param>
        ///<returns>The distance</returns>
        public static double GetDistanceBetween(double latitudePointA, double longitudePointA, double latitudePointB, double longitudePointB)
        {
            var radian = Math.PI / 180.0;
            // Convert coordinates to radians
            double dLatAInRad = latitudePointA * radian;
            double dLongAInRad = longitudePointA * radian;
            double dLatBInRad = latitudePointB * radian;
            double dLongBInRad = longitudePointB * radian;

            double dLongitude = dLongBInRad - dLongAInRad;
            double dLatitude = dLatBInRad - dLatAInRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                        Math.Cos(dLatAInRad) * Math.Cos(dLatBInRad) *
                        Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0 * Math.Asin(Math.Sqrt(a));

            // Distance            
             return kEarthRadiusKms * c;
        }

    }
}
