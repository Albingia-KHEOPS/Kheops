using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KcatmodeleMapper : EntityMap<Kcatmodele>   {
        public KcatmodeleMapper () {
            Map(p => p.Karid).ToColumn("KARID");
            Map(p => p.Karkaqid).ToColumn("KARKAQID");
            Map(p => p.Kardateapp).ToColumn("KARDATEAPP");
            Map(p => p.Kartypo).ToColumn("KARTYPO");
            Map(p => p.Karmodele).ToColumn("KARMODELE");
            Map(p => p.Karcru).ToColumn("KARCRU");
            Map(p => p.Karcrd).ToColumn("KARCRD");
            Map(p => p.Karcrh).ToColumn("KARCRH");
            Map(p => p.Karmaju).ToColumn("KARMAJU");
            Map(p => p.Karmajd).ToColumn("KARMAJD");
            Map(p => p.Karmajh).ToColumn("KARMAJH");
        }
    }
}
