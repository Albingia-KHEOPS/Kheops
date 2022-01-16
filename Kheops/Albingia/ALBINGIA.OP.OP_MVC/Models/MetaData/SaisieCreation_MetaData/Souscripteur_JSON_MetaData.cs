using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData
{
    public class Souscripteur_JSON_MetaData
    {
        public string Code { get; set; }
        public string Nom { get; set; }

        public Souscripteur_JSON_MetaData()
        {
            Code = String.Empty;
            Nom = String.Empty;
        }
    }
}