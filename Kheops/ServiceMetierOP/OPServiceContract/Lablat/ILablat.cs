
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;

namespace OPServiceContract
{
    [ServiceContract]
    [ServiceKnownType(typeof(Partner))]
    public interface ILablat
    {
        /// <summary>
        /// Vérification des partenaires par Lablat.
        /// </summary>
        /// <param name="as400Type">type de partenaire As400</param>
        /// <param name="partnerAs400Id">ID As400 partenaire</param>
        /// <param name="partnerLastName">Nom partenaire</param>
        /// <param name="connectedUserName">Login utilisateur connecté</param>
        /// <param name="partnerFirstName">Prénom partenaire (facultatif)</param>
        /// <param name="countryName">Pays (facultatif)</param>
        /// <param name="contactAs400Id">ID As400 contact (facultatif)</param>
        /// <returns></returns>
        [OperationContract]
        LablatResult VerificationPartenaire(int as400Type, string partnerAs400Id, string PartnerLastName, string connectedUserName, string partnerFirstName = null, string countryName = null, int? contactAs400Id = null);

        [OperationContract]
        IList<LablatResult> CheckMultipleParterns(IList<IPartner> partner);
    }
}
