using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamCible;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamFiltres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleExcelContratPage : MetaModelsBase
    {
        public string Type { get; set; }
        public string Designation { get; set; }
        public string Branche { get; set; }
        public string Cible { get; set; }
        public string Gestionnaire { get; set; }
        public string Souscripteur { get; set; }
        public Nullable<DateTime> DateEffetDebut { get; set; }
        public Nullable<DateTime> DateEffetFin { get; set; }
        public string DateEcheance { get; set; }
        public string Periodicite { get; set; }
        public string OffreId { get; set; }
        public string NumAliment { get; set; }

        public string CodeCourtierGestionnaire { get; set; }
        public string NomCourtierGestionnaire { get; set; }
        public string CodeCourtierApporteur { get; set; }
        public string NomCourtierApporteur { get; set; }
        public string CodeCourtierPayeur { get; set; }
        public string NomCourtierPayeur { get; set; }
        public string CodeAssure { get; set; }
        public string NomAssure { get; set; }

        public int? NumeroVoie { get; set; }
        public string ExtensionVoie { get; set; }
        public string NomVoie { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }

        public List<AlbSelectListItem> Branches { get; set; }
        public List<AlbSelectListItem> Cibles { get; set; }
        public List<SelectListItem> Periodicites { get; set; }

        public string Files { get; set; }

        public ModeleExcelContratPage()
        {
            Type = "P";
            Branches = new List<AlbSelectListItem>();
            Cibles = new List<AlbSelectListItem>();
            Periodicites = new List<SelectListItem>();
        }
    }
}