using Hexavia.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hexavia.Console.Infrastructure.GoogleMaps
{

    public class GeolocResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public LatLong Result { get; set; }
    }
}
