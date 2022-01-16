using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.MetaJson._DialogBoxes_Models
{
    public abstract class _Dialogboxes_Models_Base : _MetaJson_Base
    {
        public string Titre { get; set; }
        public string Body { get; set; }
        public string Trace { get; set; }
        public string Icon { get; set; }
        public string ButtonOkText { get; set; }
        public string ButtonCancelText { get; set; }
    }
}