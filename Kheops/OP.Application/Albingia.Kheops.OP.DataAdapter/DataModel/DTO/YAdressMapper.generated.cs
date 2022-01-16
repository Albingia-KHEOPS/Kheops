using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YAdressMapper : EntityMap<YAdress>   {
        public YAdressMapper () {
            Map(p => p.Abpchr).ToColumn("ABPCHR");
            Map(p => p.Abplg3).ToColumn("ABPLG3");
            Map(p => p.Abpnum).ToColumn("ABPNUM");
            Map(p => p.Abpext).ToColumn("ABPEXT");
            Map(p => p.Abplbn).ToColumn("ABPLBN");
            Map(p => p.Abplg4).ToColumn("ABPLG4");
            Map(p => p.Abpl4f).ToColumn("ABPL4F");
            Map(p => p.Abplg5).ToColumn("ABPLG5");
            Map(p => p.Abpdp6).ToColumn("ABPDP6");
            Map(p => p.Abpcp6).ToColumn("ABPCP6");
            Map(p => p.Abpvi6).ToColumn("ABPVI6");
            Map(p => p.Abpcdx).ToColumn("ABPCDX");
            Map(p => p.Abpcex).ToColumn("ABPCEX");
            Map(p => p.Abpl6f).ToColumn("ABPL6F");
            Map(p => p.Abppay).ToColumn("ABPPAY");
            Map(p => p.Abploc).ToColumn("ABPLOC");
            Map(p => p.Abpmat).ToColumn("ABPMAT");
            Map(p => p.Abpret).ToColumn("ABPRET");
            Map(p => p.Abperr).ToColumn("ABPERR");
            Map(p => p.Abpmju).ToColumn("ABPMJU");
            Map(p => p.Abpmja).ToColumn("ABPMJA");
            Map(p => p.Abpmjm).ToColumn("ABPMJM");
            Map(p => p.Abpmjj).ToColumn("ABPMJJ");
            Map(p => p.Abpvix).ToColumn("ABPVIX");
        }
    }
  

}
