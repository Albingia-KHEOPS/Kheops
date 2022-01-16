using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpOppMapper : EntityMap<KpOpp>   {
        public KpOppMapper () {
            Map(p => p.Kfpid).ToColumn("KFPID");
            Map(p => p.Kfptyp).ToColumn("KFPTYP");
            Map(p => p.Kfpipb).ToColumn("KFPIPB");
            Map(p => p.Kfpalx).ToColumn("KFPALX");
            Map(p => p.Kfpavn).ToColumn("KFPAVN");
            Map(p => p.Kfphin).ToColumn("KFPHIN");
            Map(p => p.Kfpidcb).ToColumn("KFPIDCB");
            Map(p => p.Kfptfi).ToColumn("KFPTFI");
            Map(p => p.Kfpdesi).ToColumn("KFPDESI");
            Map(p => p.Kfpref).ToColumn("KFPREF");
            Map(p => p.Kfpdech).ToColumn("KFPDECH");
            Map(p => p.Kfpmnt).ToColumn("KFPMNT");
            Map(p => p.Kfpcru).ToColumn("KFPCRU");
            Map(p => p.Kfpcrd).ToColumn("KFPCRD");
            Map(p => p.Kfpcrh).ToColumn("KFPCRH");
            Map(p => p.Kfpmaju).ToColumn("KFPMAJU");
            Map(p => p.Kfpmajd).ToColumn("KFPMAJD");
            Map(p => p.Kfpmajh).ToColumn("KFPMAJH");
            Map(p => p.Kfptds).ToColumn("KFPTDS");
            Map(p => p.Kfptyi).ToColumn("KFPTYI");
        }
    }
  

}
