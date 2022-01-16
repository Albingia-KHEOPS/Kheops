using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamTypesValeur;
using OP.WSAS400.DTO.ParametreTypeValeur;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamTypesValeurController : ControllersBase<ModeleParamTypesValeurPage>
    {
        #region Variables membres
        public static readonly string ID_LIGNE_VIDE = "-9999";
        public static readonly string MODE_EDITION = "Update";
        public static readonly string MODE_CREATION = "Insert";
        private List<AlbSelectListItem> _lstTypes;
        #endregion

        #region Méthodes publiques
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrages des types de valeur";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            model.AdditionalParam = "**";
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult RechercheTypesValeur(string codeTypeValeur, string descriptionTypeValeur, string userRights)
        {
            var toReturn = new List<ModeleLigneTypeValeur>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadListTypesValeur(codeTypeValeur, descriptionTypeValeur);
                if (result.Any())
                    result.ForEach(m => toReturn.Add((ModeleLigneTypeValeur)m));
                toReturn.ForEach(elm => elm.AdditionalParam = userRights);
            }
            return PartialView("ListeTypesValeur", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetDetailsTypeValeur(string codeTypeValeur, string userRights)
        {
            var toReturn = new ModeleDetailsTypeValeur();
            if (!string.IsNullOrEmpty(codeTypeValeur.Replace(" ", "").Replace("'", "''")))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.GetTypeValeurDetails(codeTypeValeur);
                    if (result != null)
                    {
                        toReturn = (ModeleDetailsTypeValeur)result;
                        toReturn.LigneVideTypesValeur = new ModeleLigneTypeValeurCompatible { Code = ID_LIGNE_VIDE, ListeTypesCompatibles = LstTypes(codeTypeValeur), AdditionalParam = userRights };
                        toReturn.ModeSaisie = MODE_EDITION;
                        toReturn.AdditionalParam = userRights;
                        if (toReturn.ListeTypesValeurCompatible != null)
                        {
                            toReturn.ListeTypesValeurCompatible.ForEach(elm => elm.AdditionalParam = userRights);
                            toReturn.ListeTypesValeurCompatible.ForEach(elm => elm.ListeTypesCompatibles = LstTypes(codeTypeValeur));
                            toReturn.ListeTypesValeurCompatible.ForEach(elm => elm.ListeTypesCompatibles.Add(
                                new AlbSelectListItem
                                {
                                    Text = elm.Code + " - " + elm.Description,
                                    Title = elm.Code + " - " + elm.Description,
                                    Selected = true,
                                    Value = elm.Code
                                }
                                ));
                        }
                    }
                }
            }
            else
            {
                toReturn.LigneVideTypesValeur = new ModeleLigneTypeValeurCompatible { Code = ID_LIGNE_VIDE, ListeTypesCompatibles = LstTypes(codeTypeValeur), AdditionalParam = userRights };
                toReturn.ModeSaisie = MODE_CREATION;
                toReturn.AdditionalParam = userRights;
            }
            return PartialView("DetailsTypeValeur", toReturn);
        }

        [ErrorHandler]
        public ActionResult EnregistrerTypeValeur(string mode, string codeTypeValeur, string descriptionTypeValeur, string userRights)
        {
            if (!string.IsNullOrEmpty(mode) && (!string.IsNullOrEmpty(codeTypeValeur) && (!string.IsNullOrEmpty(descriptionTypeValeur))))
            {
                if (mode == MODE_EDITION)
                {
                    var toReturn = new List<ModeleLigneTypeValeur>();
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var serviceContext=client.Channel;
                        if (serviceContext.LoadListTypesValeur(codeTypeValeur.Replace(" ", "").Replace("'", "''"), string.Empty).Any())
                        {
                            var param = new ModeleLigneTypeValeurDto { Code = codeTypeValeur.Replace(" ", "").Replace("'","''"), Description = descriptionTypeValeur.Replace("'","''") };
                            var result = serviceContext.EnregistrerTypeValeur(mode, param, GetUser());
                            if (result.Any())
                                result.ForEach(m => toReturn.Add((ModeleLigneTypeValeur)m));
                            toReturn.ForEach(elm => elm.AdditionalParam = userRights);
                        }
                        else
                            throw new AlbFoncException("Le type valeur que vous voulez modifier n'existe plus", trace: false, sendMail: false, onlyMessage: true);
                    }
                    return PartialView("ListeTypesValeur", toReturn);
                }
                if (mode == MODE_CREATION)
                {
                    var toReturn = new List<ModeleLigneTypeValeur>();
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var serviceContext=client.Channel;
                        if (!serviceContext.LoadListTypesValeur(codeTypeValeur.Replace(" ", "").Replace("'", "''"), string.Empty).Any())
                        {
                            var param = new ModeleLigneTypeValeurDto { Code = codeTypeValeur.Replace(" ", "").Replace("'", "''"), Description = descriptionTypeValeur.Replace("'", "''") };
                            var result = serviceContext.EnregistrerTypeValeur(mode, param, GetUser());
                            if (result.Any())
                                result.ForEach(m => toReturn.Add((ModeleLigneTypeValeur)m));
                            toReturn.ForEach(elm => elm.AdditionalParam = userRights);
                            return PartialView("ListeTypesValeur", toReturn);
                        }
                        throw new AlbFoncException("Ce code est déjà utilisé");
                    }
                }
            }
            throw new AlbFoncException("Code et description ne peuvent être vides");
        }

        [ErrorHandler]
        public ActionResult EnregistrerTypeValeurComp(string codeTypeValeur, string idTypeValeurComp, string codeTypeValeurComp, string userRights)
        {
            string mode = (idTypeValeurComp == ID_LIGNE_VIDE ? MODE_CREATION : MODE_EDITION);

            if (!string.IsNullOrEmpty(codeTypeValeur) && !string.IsNullOrEmpty(idTypeValeurComp) && !string.IsNullOrEmpty(codeTypeValeurComp))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    //var lstTypeValeurTmp = serviceContext.GetListeTypeValeurComp(codeTypeValeur, codeTypeValeurComp, string.Empty);
                    var lstTypeValeurTmp = serviceContext.IsValueFree(codeTypeValeurComp);
                    Int64 tIdTypeValeur = 0;
                    if (!Int64.TryParse(idTypeValeurComp, out tIdTypeValeur))
                        throw new AlbFoncException("Impossible de modifier ce type valeur, veuillez réessayer après avoir recharger l'écran", trace: false, sendMail: false, onlyMessage: true);

                    //if (lstTypeValeurTmp.Any() && !lstTypeValeurTmp.Exists(elm => elm.GuidId == tIdTypeValeur))
                    if(lstTypeValeurTmp)
                        throw new AlbFoncException("Ce type valeur est déjà associé", trace: false, sendMail: false, onlyMessage: true);
                    else
                        serviceContext.EnregistrerTypeValeurComp(mode, codeTypeValeur, idTypeValeurComp, codeTypeValeurComp, GetUser());
                }
            }
            return GetDetailsTypeValeur(codeTypeValeur, userRights);
        }

        [ErrorHandler]
        public ActionResult SupprimerTypeValeur(string idTypeValeur, string userRights)
        {
            if (!string.IsNullOrEmpty(idTypeValeur))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    if (serviceContext.LoadListTypesValeur(idTypeValeur, string.Empty).Any())
                    {
                        serviceContext.SupprimerTypeValeur("TypeValeur", idTypeValeur);
                    }
                    else
                        throw new AlbFoncException("Le type valeur que vous voulez supprimer n'existe plus");
                }
            }
            else
                throw new AlbFoncException("Le code ne peut être nul");
            return RechercheTypesValeur(string.Empty, string.Empty, userRights);
        }

        [ErrorHandler]
        public ActionResult SupprimerTypeValeurComp(string idTypeValeur, string idTypeValeurComp, string userRights)
        {
            if (!string.IsNullOrEmpty(idTypeValeurComp))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    if (serviceContext.GetListeTypeValeurComp(idTypeValeur, string.Empty, idTypeValeurComp).Any())
                    {
                        serviceContext.SupprimerTypeValeur("TypeValeurCompatible", idTypeValeurComp);
                    }
                    else
                        throw new AlbFoncException("Le type valeur compatible que vous voulez supprimer n'existe plus");
                }
            }
            else
                throw new AlbFoncException("Le code ne peut être nul");
            return GetDetailsTypeValeur(idTypeValeur, userRights);
        }

        #endregion

        #region Méthodes privées
        private List<AlbSelectListItem> LstTypes(string typeValeur)
        {
            //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
            if (_lstTypes != null)
            {
                var toReturn = new List<AlbSelectListItem>();
                _lstTypes.ForEach(elm => toReturn.Add(new AlbSelectListItem
                {
                    Value = elm.Value,
                    Text = elm.Text,
                    Title = elm.Title,
                    Selected = false
                }));
                return toReturn;
            }

            //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
            var value = new List<AlbSelectListItem>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var lstTyp = serviceContext.GetListeReferentielTypeValeurComp(typeValeur);
                lstTyp.ForEach(elm => value.Add(new AlbSelectListItem
                {
                    Value = elm.CodeTypeValeurComp.ToString(CultureInfo.InvariantCulture),
                    Text = elm.CodeTypeValeurComp + " - " + elm.DescriptionTypeValeurComp,
                    Title = elm.CodeTypeValeurComp + " - " + elm.DescriptionTypeValeurComp,
                    Selected = false
                }
                ));
            }
            _lstTypes = value;
            return value;
        }
        #endregion
    }
}
