using Hexavia.Business.Interfaces;
using Hexavia.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hexavia.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAdresseBusiness AdresseBusiness;
        private readonly IOfferContractBusiness GeolocalisationBusiness;
        private readonly IPartnerBusiness PartnerBusiness;
        private readonly IReferentielBusiness ReferentielBusiness;

        public HomeController(IAdresseBusiness adresseBusiness, 
            IOfferContractBusiness geolocalisationBusiness, IPartnerBusiness partnerBusiness, IReferentielBusiness referentielBusiness)
        {
            AdresseBusiness = adresseBusiness;
            GeolocalisationBusiness = geolocalisationBusiness;
            PartnerBusiness = partnerBusiness;
            ReferentielBusiness = referentielBusiness;
        }

        public ActionResult Index()
        {
            var adresseObject = new Adresse();

            if (!string.IsNullOrEmpty(Request["Data"]))
            {
                var dataAdresse = Request["Data"].ToString();
                var adresse = dataAdresse.Replace("adresse=", string.Empty);

                if (!string.IsNullOrEmpty(Request["host"]))
                {
                    adresseObject.UrlExterieur = Request["host"];
                }

                if (!string.IsNullOrEmpty(Request["separateur"]))
                {
                    adresseObject.Separateur = Request["separateur"].FirstOrDefault();
                } else
                {
                    adresseObject.Separateur = '¤';
                }

                var adress = adresse.Split(adresseObject.Separateur);
                var inBatiment = string.Empty;
                var inNumero = string.Empty;
                var valeurExtension = string.Empty;
                var inVoie = string.Empty;
                var inDistribution = string.Empty;
                var inVille = string.Empty;
                var txtCp = string.Empty;
                var matriculeHexavia = string.Empty;

                if (adress.Length >= 10)
                {
                    adresseObject.Batiment = adress[0];
                    if (adress[1].Contains("/") || adress[1].Contains("-"))
                    {
                        adresseObject.NumeroVoie = adress[1].Split('/', '-')[0];
                        adresseObject.NumeroVoie2 = adress[1].Split('/', '-')[1];
                    } else
                    {
                        adresseObject.NumeroVoie = adress[1];
                    }
                    adresseObject.ExtensionVoie = adress[2];
                    adresseObject.NomVoie = adress[3];
                    adresseObject.BoitePostale = adress[4];
                    if (adress[5].Contains("|"))
                    {
                        adresseObject.Ville = adress[5].Split('|')[1];
                        adresseObject.CodePostal = adress[5].Split('|')[0];
                    }
                    else
                    {
                        adresseObject.Ville = adress[5];
                        adresseObject.CodePostal = adress[6];
                    }
                    adresseObject.VilleCedex = adress[7];
                    adresseObject.CodePostalCedex = adress[8];

                    if (!string.IsNullOrEmpty(adress[9]))
                    {
                        var pays = adress[9];
                        if (pays.StartsWith("-"))
                        {
                            adresseObject.Pays.Libelle = pays.Substring(pays.IndexOf('-') + 1).Trim();
                        } else
                        {
                            adresseObject.Pays.Libelle = pays;
                        }
                    }

                    if (adress.Length >= 11)
                    {
                        adresseObject.MatriculeHexavia = null;

                        if (!string.IsNullOrEmpty(adress[10]))
                        {
                            adresseObject.MatriculeHexavia = Convert.ToInt32(adress[10]);
                        }
                    }

                    if (adress.Length >= 13)
                    {
                        adresseObject.Latitude = null;
                        adresseObject.Longitude = null;

                        if (!string.IsNullOrEmpty(adress[11]))
                        {
                            adresseObject.Latitude = adress[11];
                        }
                        if (!string.IsNullOrEmpty(adress[12]))
                        {
                            adresseObject.Longitude = adress[12];
                        }
                    }
                }
            }

            ViewBag.Branches = ReferentielBusiness.GetAllBranches();
            ViewBag.Situations = ReferentielBusiness.GetSituationByType("O");
            ViewBag.Etats = ReferentielBusiness.GetEtatByType("O");
            ViewBag.Evenements = ReferentielBusiness.GetEvenement();
            return View(adresseObject);
        }

        [HttpPost]
        public ActionResult SearchHexaviaMatricule(Adresse adresseRecherchee)
        {
            if ((adresseRecherchee == null) || (!ModelState.IsValid))
            {
                return Json(String.Empty);
            }

            var adressesWrapper = AdresseBusiness.RechercheAdresse(adresseRecherchee, 1, 10);
            Adresse adresseTrouvee = null;
            if (adressesWrapper.Adresses.Count() == 1)
            {
                adresseTrouvee = adressesWrapper.Adresses.FirstOrDefault();
            }

            if (adressesWrapper.Adresses.Count() > 1)
            {
                adresseTrouvee = adressesWrapper.Adresses.Where(adr =>
                            (adr.NomVoie.Trim().IndexOf(adresseRecherchee.NomVoie.Trim(), StringComparison.OrdinalIgnoreCase) >= 0) &&
                            (adr.CodePostal.Trim().IndexOf(adresseRecherchee.CodePostal.Trim()) >= 0)).FirstOrDefault();
            }

            if (adresseTrouvee != null)
            {
                return Json(adresseTrouvee.MatriculeHexavia);
            }

            return Json(String.Empty);
        }

        public ActionResult GetMapTiles(int zoom, int x, int y, string type)
        {
            using (var httpClient = new WebClient())
            {
                string url = "https://catastrophes-naturelles.ccr.fr/o/catnat-carto/contextProxy?url=/services/tilemetier2/112/" + zoom + "/" + y + "/" + x;
                switch (type)
                {
                    case "sismicite":
                        url = "https://catastrophes-naturelles.ccr.fr/o/catnat-carto/contextProxy?url=/services/tilemetier2/112/" + zoom + "/" + y + "/" + x; ;
                        break;
                    case "inondation":
                        url = "https://catastrophes-naturelles.ccr.fr/o/catnat-carto/contextProxy?url=/services/tilemetier2/68/" + zoom + "/" + y + "/" + x;
                        break;
                    case "secheresse":
                        url = "https://catastrophes-naturelles.ccr.fr/o/catnat-carto/contextProxy?url=/services/tilemetier2/100_101_102_103_99/" + zoom + "/" + y + "/" + x;
                        break;
                }
                string contentType = "image/png";
                
                httpClient.Headers.Add("Content-Type", contentType);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var test = httpClient.DownloadData(url);

                return base.File(test, "image/png");
            }
        }
    }
}