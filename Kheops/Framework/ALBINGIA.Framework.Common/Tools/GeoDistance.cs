using System;

namespace ALBINGIA.Framework.Common.Tools
{
    public static class GeoDistance
    {
        public static double GetDistance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            var theta = lon1 - lon2;
            var dist = Math.Sin(Deg2Rad(lat1))*Math.Sin(Deg2Rad(lat2)) +
                       Math.Cos(Deg2Rad(lat1))*Math.Cos(Deg2Rad(lat2))*Math.Cos(Deg2Rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2Deg(dist);
            dist = dist*60*1.1515;
            switch (unit)
            {
                case 'K':
                    dist = dist*1.609344;
                    break;
                case 'N':
                    dist = dist*0.8684;
                    break;
            }
            return (dist);
        }
        private static double Deg2Rad(double deg)
        {
            return (deg*Math.PI/180.0);
        }
        private static double Rad2Deg(double rad)
        {
            return (rad/Math.PI*180.0);
        }

        public static Boolean WithinRectangle(Double lattitudeNorth,
            Double longitudeWest,
            Double lattitudeSouth,
            Double longitudeEast,
            Double lattitude,
            Double longitude)
        {
            if (lattitude > lattitudeNorth)
                return false;
            if (lattitude < lattitudeSouth)
                return false;

            if (longitudeEast >= longitudeWest)
                return ((longitude >= longitudeWest) && (longitude <= longitudeEast));
            return (longitude >= longitudeWest);
        }

        public static Boolean IsPointInPolygon(Double lattitudeNorth,
            Double longitudeWest,
            Double lattitudeSouth,
            Double longitudeEast,
            Double lattitude,
            Double longitude)
        {
           
            //Boolean c = false;
            ////for (i = 0, j = poly.size() - 1; i < poly.size(); j = i++)
            ////{

            //    if ((((lattitudeNorth <= lattitude) && (lattitude < lattitudeSouth)) ||
            //        ((lattitudeSouth <= lattitude) && (lattitude < lattitudeNorth))) &&
            //        (longitude < (longitudeEast - longitudeWest) * (lattitude - lattitudeNorth)
            //        / (lattitudeSouth - lattitudeNorth) + longitudeWest))
            //        c = !c;
            ////}
            //return c;

            return IsWithinArea( lattitudeSouth, longitudeEast,lattitudeNorth, longitudeWest, lattitude, longitude);

        }

        private static bool IsWithinArea(double topLeftLat, double topLeftLong,
       double bottomRightLat, double bottomRightLong, double testLat, double testLong)
        {

            return (testLat >= topLeftLat && testLat <= bottomRightLat && testLong >= topLeftLong && testLong <= bottomRightLong);

        }
			
    }
}

