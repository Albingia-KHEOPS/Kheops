using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALBINGIA.OP.OP_MVC.Models.MetaJson._DialogBoxes_Models;

namespace ALBINGIA.OP.OP_MVC.Models.MetaJson
{
    [Serializable]
    public class JQueryHttpPostResult : _MetaJson_Base
    {
        public string ActionTag { get; set; }
        public string Script { get; set; }
        public List<string> Scripts { get; set; }
        public string CasPopup { get; set; }
        public SimpleDialogbox_Models DialogBox { get; set; }

        public JQueryHttpPostResult()
        {
            DialogBox = new SimpleDialogbox_Models();
            Scripts = new List<string>();
        }
    }
}