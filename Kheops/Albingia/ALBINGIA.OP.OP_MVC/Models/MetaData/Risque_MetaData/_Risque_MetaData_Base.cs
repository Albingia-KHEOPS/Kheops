using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.Risque_MetaData
{
    public abstract class _Risque_MetaData_Base : ModelsBase 
    {
        public string DescriptifRisque { get; set; }

        public _Risque_MetaData_Base()
        {
            this.DescriptifRisque = string.Empty;
        }
    }
}