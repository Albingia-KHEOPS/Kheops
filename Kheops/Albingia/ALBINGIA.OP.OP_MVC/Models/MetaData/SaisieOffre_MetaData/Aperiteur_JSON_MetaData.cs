using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.ModifierOffre_MetaData
{
    public class Aperiteur_JSON_MetaData
    {
        public string Code { get; set; }
        public string Nom { get; set; }

        public Aperiteur_JSON_MetaData()
        {
            this.Code=string.Empty;
            this.Nom = string.Empty;
        }

    }
}
