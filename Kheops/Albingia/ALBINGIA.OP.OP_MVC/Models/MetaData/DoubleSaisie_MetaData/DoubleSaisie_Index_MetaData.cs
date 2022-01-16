using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALBINGIA.OP.OP_MVC.Models.MetaData.CabinetCourtage_MetaData;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.DoubleSaisie_MetaData
{
    public class DoubleSaisie_Index_MetaData
    {
        public object GroupInformationTiersTitle { get { return "Tiers impliqués"; } }

        public object GroupActionsARealiserTitle { get { return "Actions à réaliser"; } }

        public CabinetCourtageApporteur_GridRow_MetaData CabinetApporteur { get; set; }
        public CabinetCourtageGestionnaire_GridRow_MetaData CabinetGestionnaire { get; set; }
        public List<CabinetCourtageAutre_GridRow_MetaData> CabinetsAutres { get; set; }
        public CabinetCourtageDemandeur_GridRow_MetaData CabinetDemandeur { get; set; }
        //ToDo ECM à vérifier si utilisé
        public IEnumerable<SelectListItem> MotifsRefusEnregistrerCourtier { get; set; }
        public IEnumerable<AlbSelectListItem> MotifsRefusNotifierGestionnaire { get; set; }
        public IEnumerable<AlbSelectListItem> MotifsRefusNotifierDemandeur { get; set; }

        #region Actions à réaliser

        public bool EnregistrerCourtierDemandeur { get; set; }
        public bool MaintenirRemplacerCourtier { get; set; }
        public bool NotifierCourtierGestionnaire { get; set; }
        public bool NotifierCourtierDemandeur { get; set; }

        [Display(Name = "MotifRefus")]
        public string MotifRefus { get; set; }

        public List<AlbSelectListItem> MotifsRefus { get; set; }

        [Display(Name = "MotifRefus")]
        public string MotifRefusGestionnaire { get; set; }

        public List<AlbSelectListItem> MotifsRefusGestionnaire { get; set; }

        [Display(Name = "MotifRefus")]
        public string MotifRefusDemandeur { get; set; }

        public List<AlbSelectListItem> MotifsRefusDemandeur { get; set; }

        #endregion

        public DoubleSaisie_Index_MetaData()
        {
            this.CabinetsAutres = new List<CabinetCourtageAutre_GridRow_MetaData>();
            this.MotifsRefusEnregistrerCourtier = new List<AlbSelectListItem>();
            this.MotifsRefusNotifierGestionnaire = new List<AlbSelectListItem>();
            this.MotifsRefusNotifierDemandeur = new List<AlbSelectListItem>();
        }
    }
}