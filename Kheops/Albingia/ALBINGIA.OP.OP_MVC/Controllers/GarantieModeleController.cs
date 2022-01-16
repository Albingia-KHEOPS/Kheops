using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.GarantieModele;
using OPServiceContract.IClausesRisquesGaranties;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Services = Albingia.Kheops.OP.Application.Port.Driver;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class GarantieModeleController : ControllersBase<ModeleGarantieModelePage>
    {
        #region Méthode Publique

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Modeles de Garantie";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            SetDetailModelesGarantie();
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult Recherche(string code, string description)
        {
            List<ModeleGarantieModele> ToReturn = new List<ModeleGarantieModele>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                var result = client.Channel.GarantieModeleGet(code, description).ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => ToReturn.Add((ModeleGarantieModele)m));
            }
            return PartialView("ListGarantieModeles", ToReturn);
        }

        [ErrorHandler]
        public ActionResult ConsultModelesGarantie(string code, bool readOnly, bool isNew)
        {
            ModeleGarantieModele toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                GarantieModeleDto result = client.Channel.GarantieModeleInfoGet(code);

                toReturn = new ModeleGarantieModele
                {
                    Code = result.Code,
                    Description = result.Description,
                    ReadOnly = readOnly,
                    IsNew = isNew,
                };
                if (result.LstModeleGarantie != null && result.LstModeleGarantie.Any())
                {
                    toReturn.LstModeleGarantie = new List<ModeleGarantie>();
                    result.LstModeleGarantie.ToList().ForEach(m => toReturn.LstModeleGarantie.Add((ModeleGarantie)m));
                }
            }
            return PartialView("EditorTemplates/ConsultGarantieModele", toReturn);
        }

        [ErrorHandler]
        public JsonResult Enregistrer(string code, string description, bool isNew)
        {
            string toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                client.Channel.EnregistrerGarantieModele(code, description, isNew, out toReturn);
            }
            return Json(toReturn, JsonRequestBehavior.AllowGet);
        }

        [ErrorHandler]
        public JsonResult Copier(string code, string codeCopie)
        {
            string toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                client.Channel.CopierGarantieModele(code, codeCopie, out toReturn);
            }
            return Json(toReturn, JsonRequestBehavior.AllowGet);
        }

        [ErrorHandler]
        public JsonResult Supprimer(string code)
        {
            string toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                client.Channel.SupprimerGarantieModele(code, out toReturn);
            }
            return Json(toReturn, JsonRequestBehavior.AllowGet);
        }

        //[ErrorHandler]
        //public ActionResult Recherche(string code, string description)
        //{
        //    List<ModeleGarantieModele> ToReturn = new List<ModeleGarantieModele>();
        //    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
        //    {
        //        var risqueGaranties=client.Channel;
        //        List<GarantieModeleDto> result = risqueGaranties.GarantieModeleGet(code, description).ToList();

        //        if (result != null && result.Count > 0)
        //            result.ForEach(m => ToReturn.Add((ModeleGarantieModele)m));
        //    }

        //    return PartialView("ListGarantieModeles", ToReturn);
        //}

        //[ErrorHandler]
        //public ActionResult ConsultModelesGarantie(string code, bool readOnly, bool isNew)
        //{
        //    ModeleGarantieModele toReturn;
        //    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
        //    {
        //        var risqueGaranties=client.Channel;
        //        GarantieModeleDto result = risqueGaranties.GarantieModeleInfoGet(code);

        //        toReturn = new ModeleGarantieModele
        //         {
        //             Code = result.Code,
        //             Description = result.Description,
        //             ReadOnly = readOnly,
        //             IsNew = isNew,
        //        };
        //        if (result.LstModeleGarantie != null && result.LstModeleGarantie.Any())
        //        {
        //            toReturn.LstModeleGarantie = new List<ModeleGarantie>();
        //            result.LstModeleGarantie.ToList().ForEach(m => toReturn.LstModeleGarantie.Add((ModeleGarantie)m));
        //        }
        //    }
        //    return PartialView("EditorTemplates/ConsultGarantieModele", toReturn);
        //}

        //[ErrorHandler]
        //public JsonResult Enregistrer(string code, string description, bool isNew)
        //{
        //    string toReturn;
        //    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
        //    {
        //        var risqueGaranties = client.Channel;
        //        risqueGaranties.EnregistrerGarantieModele(code, description, isNew, out toReturn);
        //    }
        //    return Json(toReturn, JsonRequestBehavior.AllowGet);
        //}

        //[ErrorHandler]
        //public JsonResult Copier(string code, string codeCopie)
        //{
        //    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
        //    {
        //        var risqueGaranties = client.Channel;
        //        risqueGaranties.CopierGarantieModele(code, codeCopie);
        //    }
        //    return Json("", JsonRequestBehavior.AllowGet);
        //}

        //[ErrorHandler]
        //public JsonResult Supprimer(string code)
        //{
        //    string toReturn;
        //    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
        //    {
        //        var risqueGaranties = client.Channel;
        //        risqueGaranties.SupprimerGarantieModele(code, out toReturn);
        //    }
        //    return Json(toReturn, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Méthode Privée

        private void SetDetailModelesGarantie()
        {
            //TODO: ZBO : Voir avec ECM/HLC l'utilité
        }

        #endregion

    }
}
