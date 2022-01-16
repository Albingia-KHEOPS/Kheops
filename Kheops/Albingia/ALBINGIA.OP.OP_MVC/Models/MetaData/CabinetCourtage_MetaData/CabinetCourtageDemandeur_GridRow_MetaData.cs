using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.CabinetCourtage_MetaData
{
    public class CabinetCourtageDemandeur_GridRow_MetaData : _CabinetCourtage_MetaData_Base  
    {
        public List<SelectListItem> Souscripteurs { get; set; }
        
        public CabinetCourtageDemandeur_GridRow_MetaData():base(enTypesCabinetCourtage.Demandeur)
        {
            this.Souscripteurs = new List<SelectListItem>();
        }

    }
}