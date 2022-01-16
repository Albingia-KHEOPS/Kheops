using Hexavia.Business.Interfaces;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using System;
using System.Globalization;
using System.Web.Mvc;

namespace Hexavia.Controllers
{
    public class PartnerController : BaseController
    {
        private readonly IPartnerBusiness PartnerBusiness;
        private readonly IGrpUrlBusiness GrpUrlBusiness;
        public PartnerController(IPartnerBusiness partnerBusiness, IGrpUrlBusiness grpUrlBusiness)
        {
            PartnerBusiness = partnerBusiness;
            GrpUrlBusiness = grpUrlBusiness;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OpenGrp(string type,string num)
        {
            Enum.TryParse(type, out TypePartner typePartner);
            var url = GrpUrlBusiness.GetGrpPartnerUrl(typePartner, num);

            return View(new UrlModel { Url = url });
        }

        [HttpPost]
        public ActionResult SearchPartners(string partnerType, string partnerCod, string partnerName, string partnerDept, string partnerCP, string partnerTown)
        {
            var partnerTypeConverted = (TypePartner)Convert.ToInt32(partnerType);
            int? cod = null;
            if (!string.IsNullOrWhiteSpace(partnerCod))
            {
                cod = Convert.ToInt32(partnerCod);
            }

            int? dept = null;
            if (!string.IsNullOrWhiteSpace(partnerDept))
            {
                dept = Convert.ToInt32(partnerDept);
            }

            var list = PartnerBusiness.SearchPartners(partnerTypeConverted,cod, partnerName, dept, partnerCP, partnerTown);
            var result = new { listPartners = list, zoomMax = ApplyZoomMax(list) };
            return LargeJson(result);
        }

        /// <summary>
        /// Get partner by code
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="type">Type</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPartnerByCode(int? code, TypePartner type)
        {
          
            if (code !=null)
            {
                var partner = PartnerBusiness.GetPartnerByCode(code , type);
               
                return LargeJson(new { partner = partner });
            }

            return null;
            
        }

        /// <summary>
        /// Get partner by orias
        /// </summary>
        /// <param name="orias">Orias</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPartnerByOrias(int? orias)
        {

            if (orias != null)
            {
                var partner = PartnerBusiness.GetPartnerByOrias(orias);

                return LargeJson(new { partner = partner });
            }

            return null;

        }

        /// <summary>
        /// Get partners by name prefix
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPartnersByName(string name, TypePartner type)
        {
            
                var list = PartnerBusiness.GetPartnerByNamePrefix(name, type);

                return LargeJson(new { partners = list });
           
        }

        /// <summary>
        /// Get interlocuteurs by name prefix and code courtier
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetInterlocuteursByName(string name, int? code)
        {

            var list = PartnerBusiness.GetInterlocuteurByNamePrefix(name, code);

            return LargeJson(new { interlocuteurs = list });

        }

        [HttpPost]
        public ActionResult LoadInsuredsAroundGPSPoint(string longitude, string latitude, string diametre)
        {
            var lon = Convert.ToDouble(longitude, CultureInfo.InvariantCulture);
            var lat = Convert.ToDouble(latitude, CultureInfo.InvariantCulture);
            var diam = Convert.ToDouble(diametre, CultureInfo.InvariantCulture);

            var list = PartnerBusiness.LoadPartnersAroundGPSPoint(TypePartner.Assure, lon, lat, diam);

            return LargeJson(list);
        }

        [HttpPost]
        public ActionResult LoadBrokersAroundGPSPoint(string longitude, string latitude, string diametre)
        {
            var lon = Convert.ToDouble(longitude, CultureInfo.InvariantCulture);
            var lat = Convert.ToDouble(latitude, CultureInfo.InvariantCulture);
            var diam = Convert.ToDouble(diametre, CultureInfo.InvariantCulture);

            var list = PartnerBusiness.LoadPartnersAroundGPSPoint(TypePartner.Courtier, lon, lat, diam);

            return LargeJson(list);
        }

        [HttpPost]
        public ActionResult LoadExpertsAroundGPSPoint(string longitude, string latitude, string diametre)
        {
            var lon = Convert.ToDouble(longitude, CultureInfo.InvariantCulture);
            var lat = Convert.ToDouble(latitude, CultureInfo.InvariantCulture);
            var diam = Convert.ToDouble(diametre, CultureInfo.InvariantCulture);

            var list = PartnerBusiness.LoadPartnersAroundGPSPoint(TypePartner.Expert, lon, lat, diam);

            return LargeJson(list);
        }
    }
}