using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModelePeriodeDeConnexite
    {
        public long Code { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public bool IsUtilisee { get; set; }
        public bool IsActif { get; set; }

        public string CodeOffre { get; set; }
        public int Version { get; set; }
        public string Type { get; set; }

        public List<ModeleEngTrait> ListeDeTraites { get; set; }

    }
}