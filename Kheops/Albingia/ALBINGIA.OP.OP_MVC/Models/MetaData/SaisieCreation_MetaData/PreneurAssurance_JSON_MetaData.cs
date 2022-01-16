using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData
{
    public class PreneurAssurance_JSON_MetaData
    {
        public string Code { get; set; }
        public string Nom { get; set; }
        public string[] NomSecondaires { get; set; }
        public string Departement { get; set; }
        public string Ville { get; set; }
        public string SIREN { get; set; }

        public PreneurAssurance_JSON_MetaData()
        {
            Code = String.Empty;
            Nom = String.Empty;
            Departement = String.Empty;
            Ville = String.Empty;
            SIREN = String.Empty;
        }
    }
}