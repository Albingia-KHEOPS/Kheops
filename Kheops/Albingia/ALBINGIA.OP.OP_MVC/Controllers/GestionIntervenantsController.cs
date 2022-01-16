using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesGestionIntervenants;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.GestionIntervenants;
using OP.WSAS400.DTO.Partenaire;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class GestionIntervenantsController : ControllersBase<ModeleGestionIntervenantsPage>
    {
        #region Méthodes Publiques

        
        [ErrorHandler]
        public ActionResult OpenGestionIntervenants(string id)
        {
            id = InitializeParams(id);

            #region vérification des paramètres
            if (string.IsNullOrEmpty(id))
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", true, true);
            var tIds = id.Split('_');
            if (tIds.Length < 3)
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", true, true);
            var codeDossier = tIds[0];
            var versionDossier = tIds[1];
            var typeDossier = tIds[2];
            string codeAvenant = string.Empty;
            if (tIds.Length > 3)
                codeAvenant = tIds[3];
            int iVersion;
            if (string.IsNullOrEmpty(codeDossier) || string.IsNullOrEmpty(versionDossier) || string.IsNullOrEmpty(typeDossier))
                throw new AlbFoncException("Erreur de chargement de la page:Un des trois paramètres est vide (numéro coffre/Contrat, Version, Type)", true, true);
            if (!int.TryParse(versionDossier, out iVersion))
                throw new AlbFoncException("Erreur de chargement de la page:Version non valide", true, true);
            #endregion
           
            ModeleGestionIntervenants modelGestinoIntervenants = LoadInfoGestionIntervenants(codeDossier, versionDossier, typeDossier, codeAvenant, model.ModeNavig);
            return PartialView("GestionIntervenants", modelGestinoIntervenants);
        }

        [ErrorHandler]
        public ActionResult GetDetailsIntervenant(Int64 guidId, string modeEcran, bool isReadonly)
        {
            ModeleIntervenant toReturn = null;
            if (guidId > 0)//Mode edition
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.GetDetailsIntervenant(guidId);
                    if (result != null)
                        toReturn = (ModeleIntervenant)result;
                    toReturn.IsReadOnly = isReadonly;
                }
            }
            else//Mode création
            {
                toReturn = new ModeleIntervenant();
                toReturn.GuidId = guidId;
                toReturn.CodeInterlocuteur = 0;
                toReturn.CodeIntervenant = 0;
            }
            toReturn.ListeTypes = GetListeType();
            toReturn.ModeEcran = modeEcran;
            return PartialView("DetailsIntervenant", toReturn);
        }

        [HandleJsonError]
        [ErrorHandler]
        public ActionResult EnregistrerDetailsIntervenant(string codeDossier, string versionDossier, string typeDossier, Int64 guidId, string typeInter, int codeInterv, string denomination,
            int codeInterlo,string nomInterlo , string reference, bool isPrincipal, bool isMedecinConseil, string codeAvenant)
        {
            List<ModeleIntervenant> toReturn = new List<ModeleIntervenant>();
            var toSave = new IntervenantDto
            {
                GuidId = guidId,
                Type = typeInter,
                CodeInterlocuteur = codeInterlo,
                CodeIntervenant = codeInterv,
                Reference = reference,
                IsPrincipal = isPrincipal ? "O" : "N",
                IsMedecinConseil = isMedecinConseil ? "O" : "N"
            };

            //// T 3997 :Vérification Intervenant
            //VerificationPartenairesIntervenants(codeInterv, denomination, codeInterlo, nomInterlo);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext=client.Channel;
                var lstResult = serviceContext.EnregistrerDetailsIntervenant(codeDossier, versionDossier, typeDossier, codeAvenant, toSave, GetUser());
                if (lstResult != null)
                    lstResult.ForEach(elm => toReturn.Add((ModeleIntervenant)elm));
                var lstTypeTemp = GetListeType();
                toReturn.ForEach(elm => elm.Type = lstTypeTemp.FindAll(item => item.Value == elm.Type) != null && lstTypeTemp.FindAll(item => item.Value == elm.Type).Any() ? lstTypeTemp.FindAll(item => item.Value == elm.Type).FirstOrDefault().Text : string.Empty);
            }

            return PartialView("LstIntervenants", toReturn);
        }

        [ErrorHandler]
        public ActionResult SupprimerIntervenant(string codeDossier, string versionDossier, string typeDossier, Int64 guidId, string codeAvenant)
        {
            List<ModeleIntervenant> toReturn = new List<ModeleIntervenant>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext=client.Channel;
                var lstResult = serviceContext.SupprimerIntervenant(codeDossier, versionDossier, typeDossier, codeAvenant, guidId, GetUser());
                if (lstResult != null)
                    lstResult.ForEach(elm => toReturn.Add((ModeleIntervenant)elm));
                var lstTypeTemp = GetListeType();
                toReturn.ForEach(elm => elm.Type = lstTypeTemp.FindAll(item => item.Value == elm.Type) != null && lstTypeTemp.FindAll(item => item.Value == elm.Type).Any() ? lstTypeTemp.FindAll(item => item.Value == elm.Type).FirstOrDefault().Text : string.Empty);
            }

            return PartialView("LstIntervenants", toReturn);
        }

        [ErrorHandler]
        public ActionResult TrierIntervenants(string colTri, string codeDossier, string versionDossier, string typeDossier, string imgTri)
        {
            string ascDesc = "DESC";
            if (imgTri == "tri_asc")
                ascDesc = "ASC";
            return PartialView("LstIntervenants", GetListeIntervenants(codeDossier, versionDossier, typeDossier, colTri, ascDesc).Intervenants);
        }

        #endregion

        #region Méthodes Privées

        private ModeleGestionIntervenants LoadInfoGestionIntervenants(string codeDossier, string versionDossier, string typeDossier, string codeAvenant, string modeNavig)
        {
            ModeleGestionIntervenants toReturn = new ModeleGestionIntervenants();
            toReturn.CodeDossier = codeDossier.ToUpper();
            toReturn.VersionDossier = versionDossier;
            toReturn.TypeDossier = typeDossier;
            var intervenantsInfo = GetListeIntervenants(codeDossier, versionDossier, typeDossier, codeAvenant);
            toReturn.ListIntervenants = intervenantsInfo.Intervenants;
            toReturn.IsAvenantModificationLocale = intervenantsInfo.IsAvenantModificationLocale;
            if (!string.IsNullOrEmpty(codeAvenant) && codeAvenant != "0")
                toReturn.IsModeAvenant = true;
            toReturn.IsReadOnly = GetIsReadOnly(model.TabGuid, codeDossier + "_" + versionDossier + "_" + typeDossier, codeAvenant);
            return toReturn;
        }

        private ModeleIntervenantInfo GetListeIntervenants(string codeDossier, string versionDossier, string typeDossier, string codeAvn, string orderBy = "", string ascDesc = "")
        {
            ModeleIntervenantInfo toReturn = new ModeleIntervenantInfo { Intervenants = new List<ModeleIntervenant>() };
            bool isReadonly = GetIsReadOnly(model.TabGuid, codeDossier + "_" + versionDossier + "_" + typeDossier, codeAvn);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetListeIntervenants(codeDossier, versionDossier, typeDossier, orderBy, ascDesc);
                if (result != null)
                    result.Intervenants.ForEach(elm => toReturn.Intervenants.Add((ModeleIntervenant)elm));
                var lstTypeTemp = GetListeType();
                toReturn.Intervenants.ForEach(elm => elm.Type = lstTypeTemp.FindAll(item => item.Value == elm.Type) != null && lstTypeTemp.FindAll(item => item.Value == elm.Type).Any() ? lstTypeTemp.FindAll(item => item.Value == elm.Type).FirstOrDefault().Text : string.Empty);
                toReturn.IsAvenantModificationLocale = result.IsAvenantModificationLocale;
                toReturn.Intervenants.ForEach(elm => elm.IsReadOnly = isReadonly);
            }
            return toReturn;
        }

        private List<AlbSelectListItem> GetListeType()
        {

            var listeType = new List<AlbSelectListItem>();

            //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext=client.Channel;
                var lstTyp = serviceContext.GetListeTypesIntervenant();
                lstTyp.ForEach(elm => listeType.Add(new AlbSelectListItem
                {
                    Value = elm.Code,
                    Text = elm.Code + " - " + elm.Libelle,
                    Title = elm.Code + " - " + elm.Libelle,
                    Selected = false
                }
                ));
            }
            return listeType;
        }
        /// <summary>
        /// Vérification intervenant
        /// </summary>
        /// <param name="codeIntervenant">Code intervenant</param>
        /// <param name="nomIntervenant"> Nom intervenant</param>
        /// <param name="codeInterlocuteur">Code interlocuteur</param>
        /// <param name="nomInterlocuteur">Nom interlocuteur</param>
        private void VerificationPartenairesIntervenants(int codeIntervenant, string nomIntervenant, int codeInterlocuteur, string nomInterlocuteur)
        {
            var partenaires = new PartenairesDto
            {
                Intervenants = new List<PartenaireDto>{new PartenaireDto
                {
                    Code = codeIntervenant.ToString(),
                    Nom = nomIntervenant,
                    CodeInterlocuteur = codeInterlocuteur,
                    NomInterlocuteur = nomInterlocuteur
                } }
            };
            VerificationPartenaires(partenaires);        
        }
        #endregion
    }
}
