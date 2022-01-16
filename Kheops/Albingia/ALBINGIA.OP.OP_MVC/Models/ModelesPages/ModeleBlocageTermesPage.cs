using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleBlocageTermesPage : MetaModelsBase
    {
        public AlbConstantesMetiers.DroitBlocageTerme NiveauDroit { get; set; }
        public DateTime? DateDebutEffet { get; set; }
        public DateTime? DateFinEffet { get; set; }
        public string Periodicite { get; set; }
        public DateTime? EcheancePrincipale { get; set; }
        public DateTime? DernierePeriodeDebut { get; set; }
        public DateTime? DernierePeriodeFin { get; set; }
        public DateTime? ProchaineEcheance { get; set; }

        public string ZoneStop { get; set; }
        public List<AlbSelectListItem> ZonesStop { get; set; }
    }
}