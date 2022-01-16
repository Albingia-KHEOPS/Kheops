using Hexavia.Business.Interfaces;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hexavia.Business
{
    public class KheopsUrlBusiness : IKheopsUrlBusiness
    {
        private readonly string KheopsLink;
        private readonly string TabGuidKey = "tabGuid";
        private readonly string ModeNavigKey = "modeNavig";
        private readonly string ParamKey = "addParam";
        private readonly string DroitConsultation = "ConsultOnly";
        private readonly string openCitrixUrl;
      
        private readonly Dictionary<string, string> BranchColor = new Dictionary<string, string>()
        {
            {"CO" , "#D6EAF8"},
            {"IA" , "#CFCFC4"},
            {"MR" , "#D4EFDF"},
            {"RC" , "#C39BD3"},
            {"RS" , "#FFB347"},
            {"RT" , "#FDFD96"},
            {"TR" , "#FF6961"},
            {"IN" , "#74FB93"}
        };

        private readonly Dictionary<string, string> Type = new Dictionary<string, string>()
        {
            {"P" , "Police"},
            {"O" , "Offre"},
            {"S" , "Sinistre"}
        };

        public KheopsUrlBusiness()
        {
            KheopsLink = System.Configuration.ConfigurationManager.AppSettings["UrlKheops"];
            openCitrixUrl = System.Configuration.ConfigurationManager.AppSettings.Get("OpenCitrixUrl");         
        }
        
        public string GetKheopsOfferContractLink(KGeolocCase kGeoloc, bool setLinkWithSpecificColor = false)
        {
            if (kGeoloc == null)
            {
                return string.Empty;
            }

            var baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;
            var libelle = string.Concat("<b>", kGeoloc.NumContrat.Trim(), "_", kGeoloc.Version, "</b></br>");
            var url = "#";
            var target = "' ";

            if (!kGeoloc.IsExternal)
            {
                if (kGeoloc.TypeAtlas.Contains("O") || kGeoloc.TypeAtlas.Contains("P"))
              {
                    // Offre + contrat
                    if (kGeoloc.IsKheops)
                    {
                        // Offre + contrat kheops
                        url = baseUrl + "/OfferContract/OpenKheops?numcontract=" + kGeoloc.NumContrat.Trim() + "&numerochrono=" + kGeoloc.NumeroChrono;
                        target = "' target='_blank' ";
                    }
                    else
                    {
                        // Offre + contrat citrix                   
                        url = $"{openCitrixUrl}?action=VISUCONTRAT?type={kGeoloc.Type}?ipb={kGeoloc.NumContrat.Trim()}?Alx={kGeoloc.Version}";
                    }
                }
                else
                {
                    // sinistre
                    url = $"{openCitrixUrl}?action=SINISTRE?type={kGeoloc.Type}?ipb={kGeoloc.NumContrat.Trim()}?Alx={kGeoloc.Version}";

                }
                        
            }
            

            if (!setLinkWithSpecificColor)
            {
                libelle = string.Concat("<b>", kGeoloc.NumContrat.Trim(), "_", kGeoloc.Version, " [", kGeoloc.Branche, "]", "</b></br>");
                return "<a href='" + url + target + "style='color:black;'>" + libelle + "</a>" + kGeoloc.Reference + "<br/><b>Type : </b>" + Type[kGeoloc.Type] + "<br/><b>Date de saisie : </b>" + kGeoloc.DateSaisie + "<br/><b>Courtier : </b>" + kGeoloc.CourtierGestionnaire + "<br/><b>Assuré : </b>" + kGeoloc.AssureGestionnaire + "<br/><b>SMP : </b>" + (string.IsNullOrEmpty(kGeoloc.Smp) ? "Non défini" : kGeoloc.Smp) + "<br/>";
            }
            else
            {
                return "<a href='" + url + target + "style='color:" + BranchColor[kGeoloc.Branche] + "'>" + libelle + "</a>" + kGeoloc.Reference + "<br/><b>Type : </b>" + Type[kGeoloc.Type] + "<br/><b>Date de saisie : </b>" + kGeoloc.DateSaisie + "<br/><b>Courtier : </b>" + kGeoloc.CourtierGestionnaire + "<br/><b>Assuré : </b>" + kGeoloc.AssureGestionnaire + "<br/><b>SMP : </b>" + (string.IsNullOrEmpty(kGeoloc.Smp) ? "Non défini" : kGeoloc.Smp) + "<br/>";
            }

        }
           
        public string GetKheopsContractUrl(KGeolocCase kGeoloc)
        {
            var folderId = string.Join("_", new[] { kGeoloc.NumContrat, kGeoloc.Version.ToString(), kGeoloc.Type }).Trim();
            var guid = $"{TabGuidKey}{TabGuidKey}";
            var mode = $"{ModeNavigKey}{ModeConsultation.Standard.AsCode()}{ModeNavigKey}";
            string controller, avnParam;

            if (kGeoloc.NumInterneAvenant > 0)
            {
                controller = "AvenantInfoGenerales";
                avnParam = $"{ParamKey}AVN|||{AlbParameterName.AVNTYPE}|{kGeoloc.TypeTraitement}||{AlbParameterName.AVNID}|{kGeoloc.NumInterneAvenant}{ParamKey}";
            }
            else
            {
                controller = kGeoloc.Type == "O" ? "ModifierOffre" : "AnInformationsGenerales";
                avnParam = string.Empty;
            }
            return $"{KheopsLink}/{controller}/Index/{folderId}{guid}{avnParam}{mode}{DroitConsultation}";
        }
    }
}
