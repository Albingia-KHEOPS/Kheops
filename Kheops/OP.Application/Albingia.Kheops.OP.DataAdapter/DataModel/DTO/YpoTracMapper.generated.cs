using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpoTracMapper : EntityMap<YpoTrac>   {
        public YpoTracMapper () {
            Map(p => p.Pytyp).ToColumn("PYTYP");
            Map(p => p.Pyipb).ToColumn("PYIPB");
            Map(p => p.Pyalx).ToColumn("PYALX");
            Map(p => p.Pyavn).ToColumn("PYAVN");
            Map(p => p.Pyttr).ToColumn("PYTTR");
            Map(p => p.Pyvag).ToColumn("PYVAG");
            Map(p => p.Pyord).ToColumn("PYORD");
            Map(p => p.Pytra).ToColumn("PYTRA");
            Map(p => p.Pytrm).ToColumn("PYTRM");
            Map(p => p.Pytrj).ToColumn("PYTRJ");
            Map(p => p.Pytrh).ToColumn("PYTRH");
            Map(p => p.Pylib).ToColumn("PYLIB");
            Map(p => p.Pyinf).ToColumn("PYINF");
            Map(p => p.Pysda).ToColumn("PYSDA");
            Map(p => p.Pysdm).ToColumn("PYSDM");
            Map(p => p.Pysdj).ToColumn("PYSDJ");
            Map(p => p.Pysfa).ToColumn("PYSFA");
            Map(p => p.Pysfm).ToColumn("PYSFM");
            Map(p => p.Pysfj).ToColumn("PYSFJ");
            Map(p => p.Pymju).ToColumn("PYMJU");
            Map(p => p.Pymja).ToColumn("PYMJA");
            Map(p => p.Pymjm).ToColumn("PYMJM");
            Map(p => p.Pymjj).ToColumn("PYMJJ");
            Map(p => p.Pymjh).ToColumn("PYMJH");
        }
    }
}
