using System.ServiceModel.Activation;
using System;
using System.Configuration;
using OPServiceContract;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.Framework.Common.Constants;
using OP.DataAccess;
using System.Net;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using System.Collections.Generic;
using ALBINGIA.Framework.Common.Business;
using OP.WSAS400.DTO.User;
using System.Threading.Tasks;
using System.Linq;
using ALBINGIA.Framework.Common.Extensions;
using System.Runtime.Serialization;

namespace OP.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Lablat : ILablat
    {

        private readonly string baseUrl = ConfigurationManager.AppSettings["UrlLablatWebApi"];
        /// <summary>
        /// Verification Partenaire
        /// </summary>
        /// <param name="as400Type">type de partenaire As400</param>
        /// <param name="partnerAs400Id">ID As400 partenaire</param>
        /// <param name="partnerLastName">Nom partenaire</param>
        /// <param name="connectedUserName">Login utilisateur connecté</param>
        /// <param name="partnerFirstName">Prénom partenaire (facultatif)</param>
        /// <param name="countryName">Pays (facultatif)</param>
        /// <param name="contactAs400Id">ID As400 contact (facultatif)</param>
        /// <returns></returns>
        public LablatResult VerificationPartenaire(int as400Type, string partnerAs400Id, string partnerLastName, string connectedUserName, string partnerFirstName = null, string countryName = null, int? contactAs400Id = null)
        {
            var user = CommonRepository.GetUserInformation(connectedUserName);
            return VerificationPartenaireLablat(as400Type, partnerAs400Id, partnerLastName, user?.UserLogin, user?.UserMail, partnerFirstName, countryName, contactAs400Id);
        }

        public IList<LablatResult> CheckMultipleParterns(IList<IPartner> partners) {
            string loginAS400 = WCFHelper.GetFromHeader("UserAS400");
            var user = CommonRepository.GetUserInformation(loginAS400);
            return CheckMultipleParterns(partners, user);
        }

        private static (string url, int timeout) BuildUrl(IPartner partner, AlbUserDto user) {
            var verificationPartenaireBaseUrl = ConfigurationManager.AppSettings["UrlVerificationPartenaire"];
            if (int.TryParse(ConfigurationManager.AppSettings["LablatTimeOut"], out int timeout) == false) {
                timeout = 5;
            }
            var source = "KHEOPS";
            var idContactAs400ParamUrl = partner.RepresentativeCode.IsEmptyOrNull() ? string.Empty : $"idContactAs400={partner.RepresentativeCode}&";
            var firstNameParamUrl = string.Empty;
            var countryNameParamUrl = partner.CountryName.IsEmptyOrNull() ? string.Empty : $"countryName ={partner.CountryName}&";
            string url = $"{verificationPartenaireBaseUrl}?{countryNameParamUrl}source={source}&{firstNameParamUrl}lastName={partner.Name}&idPartnerAs400={partner.Code}&{idContactAs400ParamUrl}typeAs400={(int)partner.TypeAS400}&username={user.UserLogin}&userEmail={user.UserMail}";
            return (url, timeout);
        }

        /// <summary>
        /// Vérification partenaire par LABLAT
        /// </summary>
        /// <param name="as400Type">type de partenaire As400 </param>
        /// <param name="partnerAs400Id">As400 id partenaire</param>
        /// <param name="partnerLastName">Nom</param>
        /// <param name="connectedUserName">UserName</param>
        /// <param name="connectedUserEmail">Email</param> 
        /// <param name="partnerFirstName">Prénom</param>
        /// <param name="countryName">Pays</param>
        /// <param name="contactAs400Id">As400 id contact</param>
        /// <returns></returns>
        private LablatResult VerificationPartenaireLablat(int as400Type, string partnerAs400Id, string partnerLastName, string connectedUserName, string connectedUserEmail, string partnerFirstName, string countryName, int? contactAs400Id)
        {
            var result = LablatResult.Error;
            try
            {

                // Vérification des Champs obligatoires
                if (!string.IsNullOrEmpty(partnerLastName) && !string.IsNullOrEmpty(partnerAs400Id) && as400Type != 0 && !string.IsNullOrEmpty(connectedUserName) && !string.IsNullOrEmpty(connectedUserEmail))
                {
                    var verificationPartenaireBaseUrl = ConfigurationManager.AppSettings["UrlVerificationPartenaire"];
                    if (int.TryParse(ConfigurationManager.AppSettings["LablatTimeOut"], out int timeout) == false) {
                        timeout = 5;
                    }
                    var source = "KHEOPS";
                    var idContactAs400ParamUrl = contactAs400Id == null ? "" : $"idContactAs400={contactAs400Id}&";
                    var firstNameParamUrl = string.IsNullOrEmpty(partnerFirstName) ? "" : $"firstName ={partnerFirstName}&";
                    var countryNameParamUrl = string.IsNullOrEmpty(countryName) ? "" : $"countryName ={countryName}&";
                    var actionUrl = $"{verificationPartenaireBaseUrl}?{countryNameParamUrl}source={source}&{firstNameParamUrl}lastName={partnerLastName}&idPartnerAs400={partnerAs400Id}&{idContactAs400ParamUrl}typeAs400={as400Type}&username={connectedUserName}&userEmail={connectedUserEmail}";

                    using (var client = new RestfulServiceHelper<string>(this.baseUrl, actionUrl, timeout))
                    {
                        var response = client.Get();
                        if (int.TryParse(response, out var code))
                        {
                            switch (code)
                            {
                                case 0: result = LablatResult.Notsuspicious; break;
                                case 1: result = LablatResult.Suspicious; break;
                                case -1: result = LablatResult.Blacklisted; break;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                var albException = e as AlbApiException;

                if (albException?.StatusCode == HttpStatusCode.BadRequest || albException?.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    return result;
                }
                throw;
            }
        }

        private IList<LablatResult> CheckMultipleParterns(IList<IPartner> partners, AlbUserDto user) {
            var tasks = partners.Select(p => CheckParternAsync(p, user)).ToArray();
            var result = Task.WhenAll(tasks).Result.ToList();
            return result;
        }

        private async Task<LablatResult> CheckParternAsync(IPartner partner, AlbUserDto user) {
            try {
                var (url, timeout) = BuildUrl(partner, user);
                using (var client = new RestfulServiceHelper<string>(baseUrl, url, timeout)) {
                    string result = await client.GetAsync();
                    if (int.TryParse(result, out var code)) {
                        switch (code) {
                            case 0: return LablatResult.Notsuspicious;
                            case 1: return LablatResult.Suspicious;
                            case -1: return LablatResult.Blacklisted;
                        }
                    }
                }
            }
            catch (AlbApiException e) {
                if (e.StatusCode == HttpStatusCode.BadRequest || e.StatusCode == HttpStatusCode.RequestTimeout) {
                    return LablatResult.Error;
                }
                throw;
            }
            return LablatResult.Error;
        }
    }
}
