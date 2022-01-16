using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.Risque_MetaData
{
    public class Risque_Index_MetaData : _Risque_MetaData_Base
    {
        public IEnumerable<_Risque_MetaData_Base> Risques { get; set; }
    }
}