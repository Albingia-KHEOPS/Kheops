using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public  class Utilisateur_ListeNomMapper : EntityMap<Utilisateur_ListeNom> {
        public Utilisateur_ListeNomMapper () {
            Map(p => p.Nom).ToColumn("Utnom");
            Map(p => p.Prenom).ToColumn("Utpnm");
            Map(p => p.B1nombureau).ToColumn("B1.budbu");
            Map(p => p.B1ad1).ToColumn("B1.buad1");
            Map(p => p.Utiut).ToColumn("Utiut");
        }
    }
  

}
