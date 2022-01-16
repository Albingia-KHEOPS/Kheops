using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpOppApMapper : EntityMap<KpOppAp>   {
        public KpOppApMapper () {
            Map(p => p.Kfqid).ToColumn("KFQID");
            Map(p => p.Kfqtyp).ToColumn("KFQTYP");
            Map(p => p.Kfqipb).ToColumn("KFQIPB");
            Map(p => p.Kfqalx).ToColumn("KFQALX");
            Map(p => p.Kfqavn).ToColumn("KFQAVN");
            Map(p => p.Kfqhin).ToColumn("KFQHIN");
            Map(p => p.Kfqkfpid).ToColumn("KFQKFPID");
            Map(p => p.Kfqperi).ToColumn("KFQPERI");
            Map(p => p.Kfqrsq).ToColumn("KFQRSQ");
            Map(p => p.Kfqobj).ToColumn("KFQOBJ");
            Map(p => p.Kfqcru).ToColumn("KFQCRU");
            Map(p => p.Kfqcrd).ToColumn("KFQCRD");
            Map(p => p.Kfqmaju).ToColumn("KFQMAJU");
            Map(p => p.Kfqmajd).ToColumn("KFQMAJD");
        }
    }
  

}
