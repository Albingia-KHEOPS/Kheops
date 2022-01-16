using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KedilogMapper : EntityMap<Kedilog>   {
        public KedilogMapper () {
            Map(p => p.Idsession).ToColumn("IDSESSION");
            Map(p => p.Typ).ToColumn("TYP");
            Map(p => p.Ipb).ToColumn("IPB");
            Map(p => p.Alx).ToColumn("ALX");
            Map(p => p.Statut).ToColumn("STATUT");
            Map(p => p.Methode).ToColumn("METHODE");
            Map(p => p.Dateheure).ToColumn("DATEHEURE");
            Map(p => p.Info).ToColumn("INFO");
            Map(p => p.Seq).ToColumn("SEQ");
        }
    }
  

}
