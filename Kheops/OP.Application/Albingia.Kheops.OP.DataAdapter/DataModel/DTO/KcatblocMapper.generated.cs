using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KcatblocMapper : EntityMap<Kcatbloc>   {
        public KcatblocMapper () {
            Map(p => p.Kaqid).ToColumn("KAQID");
            Map(p => p.Kaqbra).ToColumn("KAQBRA");
            Map(p => p.Kaqcible).ToColumn("KAQCIBLE");
            Map(p => p.Kaqvolet).ToColumn("KAQVOLET");
            Map(p => p.Kaqkapid).ToColumn("KAQKAPID");
            Map(p => p.Kaqbloc).ToColumn("KAQBLOC");
            Map(p => p.Kaqkaeid).ToColumn("KAQKAEID");
            Map(p => p.Kaqcar).ToColumn("KAQCAR");
            Map(p => p.Kaqordre).ToColumn("KAQORDRE");
            Map(p => p.Kaqcru).ToColumn("KAQCRU");
            Map(p => p.Kaqcrd).ToColumn("KAQCRD");
            Map(p => p.Kaqcrh).ToColumn("KAQCRH");
            Map(p => p.Kaqmaju).ToColumn("KAQMAJU");
            Map(p => p.Kaqmajd).ToColumn("KAQMAJD");
            Map(p => p.Kaqmajh).ToColumn("KAQMAJH");
        }
    }
}
