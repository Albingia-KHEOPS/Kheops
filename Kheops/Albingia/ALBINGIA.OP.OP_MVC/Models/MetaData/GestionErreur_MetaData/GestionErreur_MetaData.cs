using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.GestionErreur_MetaData
{
    public class GestionErreur_MetaData : _GestionErreur_MetaData_Base
    {
        public SimpleDialogbox_Models SimpleDialogbox { get; set; }
        public GestionErreur_MetaData()
            : base()
        {

        }
    }
}