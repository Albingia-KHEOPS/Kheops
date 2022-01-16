using Albingia.Kheops.OP.Application.Port.Driver;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using Mapster;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class OffreRelanceController : ControllersBase<ModeleOffreRelancePage>
    {
        // GET: OffreRelance
        [ErrorHandler]
        public ActionResult Index()
        {
            this.model.ListeRelances = new ModelRelances();
            try
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                using (var clientRef = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IReferentielPort>()) {
                    var pagingList = client.Channel.GetUserRelances();
                    if (pagingList != null) {
                        this.model.ListeRelances.Relances = pagingList.List.Adapt<List<RelanceOffre>>();
                        this.model.ListeRelances.NombreRelances = pagingList.NbTotalLines;
                        this.model.CurrentPage = 1;
                    }
                    this.model.MotifsSituation = clientRef.Channel.GetMotifsSituations().Select(x => new AlbSelectListItem { Value = x.Code, Text = x.LibelleLong, Title = x.LibelleLong }).ToList();
                    this.model.MotifsSituation.Insert(0, new AlbSelectListItem { Value = "", Text = "Choisir un motif..." });
                }
            }
            catch (Exception e)
            {
                AlbSessionHelper.MessageErreurEcran = "<b>Erreur lors de la récupération des relances.</b>";
                new Framework.Common.AlbingiaExceptions.AlbTechException(e, true, true);
                this.model.ListeRelances.DoNotShowAgainForToday = true;
            }

            SetPageTitle();
            this.model.AfficherBandeau = true;
            return View(model);
        }

        #region ControllersBase Init
        protected override void SetPageTitle()
        {
            model.PageTitle = "Liste des offres à relancer";
        }

        protected override void LoadInfoPage(string context)
        {
        }

        protected override void UpdateModel()
        {
            model.PageEnCours = NomsInternesEcran.RechercheSaisie.ToString();
            model.ModeNavig = ModeConsultation.Standard.AsCode();
            DisplayBandeau();
        }
        #endregion
    }
}