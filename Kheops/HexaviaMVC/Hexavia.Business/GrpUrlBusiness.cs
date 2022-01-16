using Hexavia.Business.Interfaces;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Hexavia.Business
{
    public class GrpUrlBusiness : IGrpUrlBusiness
    {
        private readonly string username;
        private readonly string password;
        private readonly string grpServiceUrl;
        private readonly string grpBaseUrl;

        public GrpUrlBusiness()
        {
            username = ConfigurationManager.AppSettings.Get("grpUserName");
            password = ConfigurationManager.AppSettings.Get("grpPassword");
            grpBaseUrl = ConfigurationManager.AppSettings.Get("grpBaseUrl");
            grpServiceUrl = ConfigurationManager.AppSettings.Get("grpServiceUrl");
        }

        public string GetGrpPartnerLink(TypePartner type, KGeolocPartner kGeolocPartner)
        {
            if (string.IsNullOrWhiteSpace(kGeolocPartner.Num))
            {
                return null;
            }

            if (type != TypePartner.Assure && type != TypePartner.Courtier && type != TypePartner.Expert)
            {
                return null;
            }

            var baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;
            var libelle = string.Concat("<b>", kGeolocPartner.Num, "-", kGeolocPartner.Nom, "</b></br>");
            return "<a href='" + baseUrl + "/Partner/OpenGrp?type=" + type.ToDescription() + "&num=" + kGeolocPartner.Num + "' target='_blank'>" + libelle + "</a>";
        }

        public string GetGrpPartnerUrl(TypePartner type, string num)
        {
            if (string.IsNullOrWhiteSpace(num))
            {
                return null;
            }

            if (type != TypePartner.Assure && type != TypePartner.Courtier && type != TypePartner.Expert)
            {
                return null;
            }

            var typeAS400 = "7";
            switch (type)
            {
                case TypePartner.Assure:
                    typeAS400 = "7";
                    break;
                case TypePartner.Courtier:
                    typeAS400 = "1";
                    break;
                case TypePartner.Expert:
                    typeAS400 = "2";
                    break;
            }

            var accountGuid = GetPartnerGrpId(typeAS400, num);
            if (accountGuid == Guid.Empty)
            {
                return null;
            }

            return grpBaseUrl + GetGrpUrl(accountGuid);
        }

        private Guid GetPartnerGrpId(string typeAS400, string idPartner)
        {
            if (string.IsNullOrWhiteSpace(idPartner))
            {
                return Guid.Empty;
            }

            if (string.IsNullOrWhiteSpace(typeAS400))
            {
                return Guid.Empty;
            }

            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(username, password);
                var uriBuilder = new UriBuilder(grpServiceUrl);
                uriBuilder.Path += "AccountSet()";

                var parameters = new NameValueCollection
                {
                    { "$filter", $"AccountNumber eq '{idPartner}'" }
                };
                client.QueryString = parameters;

                var stream = client.OpenRead(uriBuilder.Uri);
                var xElement = XElement.Load(stream);
                var entries = xElement.Elements();

                foreach (var e in entries)
                {
                    if (e.Name.LocalName != "entry")
                    {
                        continue;
                    }

                    var accountGuid = GetPartnerAccountId(e, typeAS400);
                    if (accountGuid != Guid.Empty)
                    {
                        return accountGuid;
                    }
                }
            }
            return Guid.Empty;
        }

        private Guid GetPartnerAccountId(XElement entrie, string typeAS400)
        {
            var content = entrie.Elements()?.FirstOrDefault(i => i.Name.LocalName == "content");
            var properties = content.Elements()?.FirstOrDefault(p => p.Name.LocalName == "properties");
            var customerTypeCode = properties.Elements()?.FirstOrDefault(p => p.Name.LocalName == "CustomerTypeCode")?.Value;
            
            if (string.Equals(customerTypeCode, typeAS400, StringComparison.InvariantCultureIgnoreCase))
            {
                var accountId = properties.Elements().FirstOrDefault(p => p.Name.LocalName == "AccountId")?.Value;
                if (Guid.TryParse(accountId, out Guid result))
                {
                    return result;
                }
            }
            return Guid.Empty;
        }

        private string GetGrpUrl(Guid grpId)
        {
            var sb = new StringBuilder();
            // TypeCode: ? etc = 1 / ? etc = 2 : Ce paramètre change en fonction du type 
            // 1 correspond aux partenaires 
            // 2 correspond aux interlocuteurs
            sb.Append($"?etc=1");
            sb.Append("&extraqs=%3f_gridType%3d1%26etc%3d1%26id%3d%257b");
            sb.Append(grpId.ToString().ToUpper());

            //Paramètres d’affichage et de cache
            sb.Append("%257d%26pagemode%3diframe%26preloadcache%3d1548770499421%26rskey%3d528681057");
            sb.Append("&pagetype=entityrecord");

            return sb.ToString();
        }
    }
}
