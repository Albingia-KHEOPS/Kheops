using Hexavia.Business.Interfaces;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using Hexavia.Tools.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Hexavia.Controllers
{
    public class OfferContractController : BaseController
    {
        private readonly IOfferContractBusiness OfferContractBusiness;
        private readonly ILatitudeLogitudeBusiness LatitudeLogitudeBusiness;
        private readonly IKheopsUrlBusiness KheopsUrlBusiness;
        private readonly IReferentielBusiness ReferentielBusiness;

        public OfferContractController(IOfferContractBusiness offerContractBusiness,
            ILatitudeLogitudeBusiness latitudeLogitudeBusiness,
            IKheopsUrlBusiness kheopsUrlBusiness, IReferentielBusiness referentielBusiness)
        {
            OfferContractBusiness = offerContractBusiness;
            LatitudeLogitudeBusiness = latitudeLogitudeBusiness;
            KheopsUrlBusiness = kheopsUrlBusiness;
            ReferentielBusiness = referentielBusiness;
        }

        [HttpGet]
        public ActionResult OpenKheops(string numContract, int numeroChrono)
        {
            var latLon = LatitudeLogitudeBusiness.GetOfferContractLatLon(numContract, numeroChrono);
            var url = KheopsUrlBusiness.GetKheopsContractUrl(latLon);

            return View(new UrlModel { Url = url });
        }

        [HttpPost]
        public ActionResult LoadAffairesAroundGPSPoint(string longitude, string latitude, string diametre, string typeDesignation, string branche)
        {
            var lon = Convert.ToDouble(longitude, CultureInfo.InvariantCulture);
            var lat = Convert.ToDouble(latitude, CultureInfo.InvariantCulture);
            var diam = Convert.ToDouble(diametre, CultureInfo.InvariantCulture);
            var list = OfferContractBusiness.LoadAffairesAroundGPSPoint(lon, lat, diam, typeDesignation, branche);
            return LargeJson(list);
        }

        [HttpPost]
        public ActionResult LoadAffairesByDesignation(string code, string version, string designation, string typeDesignation, string etat, string situation, string branche, string garantie, string departement, string evenement, string mode)
        {
            DiffDisplayModeBranche displayMode = (DiffDisplayModeBranche)Enum.Parse(typeof(DiffDisplayModeBranche), mode);

            int count = 0;
            var list = OfferContractBusiness.LoadAffairesByDesignation(code, version,designation, typeDesignation, etat, situation, branche, garantie, departement, evenement, displayMode, out count);
            var result = new { listAffaires = list, zoomMax = ApplyZoomMax(list), count };
            return LargeJson(result);
        }

        /// <summary>
        /// Get cases by partner code and type
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="type">Type</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadAffairesByPartner(int? code, TypePartner type)
        {      
            var list = OfferContractBusiness.SearchCasesLatLonByPartner(code,type);
            var result = new { listAffaires = list };
            return LargeJson(result);
        }
        [HttpPost]
        public ActionResult LoadActeGestionFromFile()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                
                var lon = Convert.ToDouble(Request.Form["longitude"], CultureInfo.InvariantCulture);
                var lat = Convert.ToDouble(Request.Form["latitude"], CultureInfo.InvariantCulture);
                var diam = Convert.ToDouble(Request.Form["diametre"], CultureInfo.InvariantCulture);

                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                    //string filename = Path.GetFileName(Request.Files[i].FileName);  

                    HttpPostedFileBase file = files[0];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    // Get the complete folder path and store the file inside it.  
                    fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                    file.SaveAs(fname);

                    var list = OfferContractBusiness.LoadActeGestionFromFile(lon, lat, diam, fname);
                    var result = new { listAffaires = list, zoomMax = ApplyZoomMax(list), errorMessage = string.Empty };
                    return Json(result);

                }
                catch (Exception ex)
                {
                    return Json(new { errorMessage = "Error occurred. Error details: " + ex.Message });
                }
            }
            else
            {
                return Json(new { errorMessage = "No files selected." });
            }


        }

        [HttpPost]
        public ActionResult GetSituationByType(string type)
        {
            var situations = ReferentielBusiness.GetSituationByType(type);
            return Json(situations);
        }
        [HttpPost]
        public ActionResult GetEtatByType(string type)
        {
            var etats = ReferentielBusiness.GetEtatByType(type);
            return Json(etats);
        }
    }
}