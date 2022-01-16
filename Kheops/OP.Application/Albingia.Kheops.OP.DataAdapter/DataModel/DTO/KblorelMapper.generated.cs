using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KblorelMapper : EntityMap<Kblorel>   {
        public KblorelMapper () {
            Map(p => p.Kgjid).ToColumn("KGJID");
            Map(p => p.Kgjrel).ToColumn("KGJREL");
            Map(p => p.Kgjidblo1).ToColumn("KGJIDBLO1");
            Map(p => p.Kgjblo1).ToColumn("KGJBLO1");
            Map(p => p.Kgjidblo2).ToColumn("KGJIDBLO2");
            Map(p => p.Kgjblo2).ToColumn("KGJBLO2");
            Map(p => p.Kgjcru).ToColumn("KGJCRU");
            Map(p => p.Kgjcrd).ToColumn("KGJCRD");
            Map(p => p.Kgjcrh).ToColumn("KGJCRH");
            Map(p => p.Kgjmaju).ToColumn("KGJMAJU");
            Map(p => p.Kgjmajd).ToColumn("KGJMAJD");
            Map(p => p.Kgjmajh).ToColumn("KGJMAJH");
        }
    }
}
