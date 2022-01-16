using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALBINGIA.OP.OP_MVC.Models.MetaData.CabinetCourtage_MetaData;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.DoubleSaisie_MetaData
{
    public class DoubleSaisie_Index_DemandeurUpd_MetaData : _DoubleSaisie_MetaData_Base
    {
        public CabinetCourtageDemandeur_GridRowUpd_MetaData MetaData { get; set; }

        public DoubleSaisie_Index_DemandeurUpd_MetaData()
        {
            MetaData = new CabinetCourtageDemandeur_GridRowUpd_MetaData();
        }
    }
}