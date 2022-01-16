using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.DetailsRisque_MetaModels;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.DetailsRisque_MetaData
{
    public class ObjetsRisque_MetaData : _DetailsRisque_MetaData_Base
    {
        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public string CodeRisque { get; set; }

        public List<DetailsRisque_Objets_MetaModel> Objets { get; set; }

        public ObjetsRisque_MetaData()
        {
            this.Objets = new List<DetailsRisque_Objets_MetaModel>();
        }

    }
}