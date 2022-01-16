using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamTemplates;
using OP.WSAS400.DTO.ParamTemplates;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamTemplatesController : ControllersBase<ModeleParamTemplatesPage>
    {
        #region Membres statiques
        public static readonly string MODE_EDITION = "Update";
        public static readonly string MODE_CREATION = "Insert";
        public static readonly Int64 ID_LIGNE_VIDE = -9999;

        public static List<AlbSelectListItem> LstTypesTemplate(bool withAll)
        {
            //Nouvelle instance à chaque récupération de la référence
            List<AlbSelectListItem> value = new List<AlbSelectListItem>();
            if (withAll)
                value.Add(new AlbSelectListItem() { Value = "*", Text = "Tous", Title = "Tous", Selected = true });

            value.Add(new AlbSelectListItem() { Value = "O", Text = "O - Offre", Title = "O - Offre", Selected = false });
            value.Add(new AlbSelectListItem() { Value = "P", Text = "C - Contrat", Title = "C - Contrat", Selected = false });
            return value;

        }

        #endregion

        #region Méthodes Publiques
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Référentiel canevas";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult RechercheTemplates(string codeTemplate, string descriptionTemplate, string type)
        {
            var toReturn = new List<ModeleLigneTemplate>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadListTemplates(0, codeTemplate, descriptionTemplate, type, string.Empty, string.Empty, false, false);
                if (result.Any())
                    result.ForEach(m => toReturn.Add((ModeleLigneTemplate)m));
            }

            return PartialView("ListeTemplates", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetDetailsTemplate(string idTemplate)
        {
            ModeleLigneTemplate toReturn = null;
            if (string.IsNullOrEmpty(idTemplate))
            {
                toReturn = new ModeleLigneTemplate();
                toReturn.ModeSaisie = MODE_CREATION;
                toReturn.GuidId = ID_LIGNE_VIDE;
                toReturn.ListeTypesTemplate = LstTypesTemplate(false);
                SetSelectedItem(toReturn.ListeTypesTemplate, "O");
            }
            else
            {
                Int64 id = 0;
                if (Int64.TryParse(idTemplate, out id))
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var serviceContext=client.Channel;
                        var result = serviceContext.GetDetailsTemplate(id);
                        if (result != null)
                        {
                            toReturn = (ModeleLigneTemplate)result;
                            toReturn.ModeSaisie = MODE_EDITION;
                            toReturn.ListeTypesTemplate = LstTypesTemplate(false);
                            SetSelectedItem(toReturn.ListeTypesTemplate, toReturn.TypeTemplate);

                        }
                    }
                }
            }
            if (toReturn != null)
                return PartialView("DetailsTemplate", toReturn);
            else
                throw new AlbTechException(new Exception("Impossible d'afficher les détails de ce template"));
        }

        [ErrorHandler]
        public ActionResult EnregistrerTemplate(string mode, Int64 idTemplate, string code, string description, string type)
        {
            if (!string.IsNullOrEmpty(mode) && !string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(type))
            {
                code = Server.UrlDecode(code);
                description = Server.UrlDecode(description);
                if (code.Length > 7)
                    throw new AlbFoncException("Le code est trop long, il ne peut dépasser 9 caractères (avec l'entête CV)", trace: false, sendMail: false, onlyMessage: true);
                //Verif des caractères spéciaux
                var regexSpecialChar = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9 ]*$");
                if (!regexSpecialChar.IsMatch(code))
                {
                    throw new AlbFoncException("Le code est incorrect. Il ne peut contenir de caractères spéciaux", trace: false, sendMail: false, onlyMessage: true);
                }

                string errorMsg = string.Empty;
                var myTemplate = new ModeleLigneTemplateDto
                {
                    GuidId = idTemplate,
                    CodeTemplate = ("CV" + code.Replace(" ", "").Replace("'", "").ToUpper()).PadLeft(9, ' '),
                    DescriptionTemplate = description.Replace("'", "''"),
                    TypeTemplate = type
                };

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    if (mode == MODE_CREATION)
                    {
                        //Verification de l'existence du code avant creation
                        var res = serviceContext.LoadListTemplates(0, myTemplate.CodeTemplate, string.Empty, string.Empty, string.Empty, string.Empty, false, false);
                        if (res.Any())
                            throw new AlbFoncException("Ce code de template est déjà utilisé", false, false);
                    }
                    errorMsg = serviceContext.EnregistrerTemplate(mode, myTemplate, GetUser());
                }
                if (!string.IsNullOrEmpty(errorMsg))
                    throw new AlbTechException(new Exception(errorMsg));
            }
            return null;
        }

        [ErrorHandler]
        public ActionResult SupprimerTemplate(string idTemplate)
        {
            string errorMsg = string.Empty;
            Int64 id = 0;
            if (Int64.TryParse(idTemplate, out id))
            {
                if (id > 0)
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var serviceContext=client.Channel;
                        errorMsg = serviceContext.SupprimerTemplate(id);
                    }
                }
                else
                    errorMsg = "L'identifiant du template est incorrect";
            }
            else
                errorMsg = "L'identifiant du template est incorrect";
            if (!string.IsNullOrEmpty(errorMsg))
                throw new AlbTechException(new Exception(errorMsg));

            return null;
        }

        [ErrorHandler]
        public string ConfirmSuppr(string cibleRef)
        {
            string toReturn = string.Empty;
            if (!string.IsNullOrEmpty(cibleRef))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var res = serviceContext.GetCible(cibleRef);
                    if (res != null)
                    {
                        toReturn += "Le template est lié à la cible suivante : " + res.Code + " - " + res.Libelle;
                    }
                    else
                        toReturn += "Le template va être supprimé.";
                }
            }
            else
                toReturn += "Le template va être supprimé.";
            return toReturn;
        }

        public void RegenerateCanevas(bool totalRegeneration, string codeCanevas)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                serviceContext.RegenerateCanevas(GetUser(), totalRegeneration, codeCanevas);
            }
        }

        public void CopyCanevas(String source, String cible)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext = client.Channel;                
                serviceContext.CopyCanevas(source, cible, GetUser());
            }
        }

        #endregion

        #region Méthodes Privées

        /// <summary>
        /// Initialise les éléments de base de la page
        /// </summary>
        [ErrorHandler]
        protected override void LoadInfoPage(string context = null)
        {
            model.ListeTypesTemplate = LstTypesTemplate(true);
        }

        private static void SetSelectedItem(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            selectedItem = lookUp.FirstOrDefault(elm => elm.Value == compareValue);
            if (selectedItem != null)
                selectedItem.Selected = true;
        }

        #endregion
    }
}
