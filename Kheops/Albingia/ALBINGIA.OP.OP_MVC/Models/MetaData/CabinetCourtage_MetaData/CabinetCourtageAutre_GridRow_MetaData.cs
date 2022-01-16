using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.CabinetCourtage_MetaData
{
    public class CabinetCourtageAutre_GridRow_MetaData : _CabinetCourtage_MetaData_Base  
    {
        public bool DisplayTitle { get; set; }
        public CabinetCourtageAutre_GridRow_MetaData():base(enTypesCabinetCourtage.Autres)
        {
            this.DisplayTitle = false;
        }

    }
}