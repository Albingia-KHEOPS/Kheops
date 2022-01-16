using Hexavia.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hexavia.Controllers.API
{
    public class GeocodingAPIController : ApiController
    {
        private readonly IAdresseBusiness AdresseBusiness;

        public GeocodingAPIController(IAdresseBusiness adresseBusiness)
        {
            AdresseBusiness = adresseBusiness;
        }

        //api/GeocodingAPI?adresse=value
        [HttpGet]
        public IHttpActionResult GetLocalisationFromAdresse(string adresse)
        {
            var result = AdresseBusiness.GetLocalisationFromAdresse(adresse);
            var retour = new
            {
                status = "200",
                result = new { latitude = result.Result?.Lat, longitude = result.Result?.Lon },
                message = result.Message
            };
            return Ok(retour);
        }
    }
}
