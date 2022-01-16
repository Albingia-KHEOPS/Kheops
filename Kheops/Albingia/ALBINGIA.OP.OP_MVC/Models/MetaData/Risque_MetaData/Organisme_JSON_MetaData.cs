using ALBINGIA.OP.OP_MVC.RisquesGaranties;
using EmitMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.Risque_MetaData
{
    public class Organisme_JSON_MetaData
    {
        public int Code { get; set; }
       
        public string Nom { get; set; }
    
        public string CP { get; set; }
     
        public string Ville { get; set; }

        public string Pays { get; set; }

        public static explicit operator Organisme_JSON_MetaData(OrganismeOppDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<OrganismeOppDto, Organisme_JSON_MetaData>().Map(data);
        }
    }
}