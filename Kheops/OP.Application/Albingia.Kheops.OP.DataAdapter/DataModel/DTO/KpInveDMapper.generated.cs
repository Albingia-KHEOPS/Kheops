using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpInveDMapper : EntityMap<KpInveD>   {
        public KpInveDMapper () {
            Map(p => p.Kbfid).ToColumn("KBFID");
            Map(p => p.Kbftyp).ToColumn("KBFTYP");
            Map(p => p.Kbfipb).ToColumn("KBFIPB");
            Map(p => p.Kbfalx).ToColumn("KBFALX");
            Map(p => p.Kbfavn).ToColumn("KBFAVN");
            Map(p => p.Kbfhin).ToColumn("KBFHIN");
            Map(p => p.Kbfkbeid).ToColumn("KBFKBEID");
            Map(p => p.Kbfnumlgn).ToColumn("KBFNUMLGN");
            Map(p => p.Kbfdesc).ToColumn("KBFDESC");
            Map(p => p.Kbfkadid).ToColumn("KBFKADID");
            Map(p => p.Kbfsite).ToColumn("KBFSITE");
            Map(p => p.Kbfntli).ToColumn("KBFNTLI");
            Map(p => p.Kbfcp).ToColumn("KBFCP");
            Map(p => p.Kbfville).ToColumn("KBFVILLE");
            Map(p => p.Kbfadh).ToColumn("KBFADH");
            Map(p => p.Kbfdatdeb).ToColumn("KBFDATDEB");
            Map(p => p.Kbfdebheu).ToColumn("KBFDEBHEU");
            Map(p => p.Kbfdatfin).ToColumn("KBFDATFIN");
            Map(p => p.Kbffinheu).ToColumn("KBFFINHEU");
            Map(p => p.Kbfmnt1).ToColumn("KBFMNT1");
            Map(p => p.Kbfmnt2).ToColumn("KBFMNT2");
            Map(p => p.Kbfnbevn).ToColumn("KBFNBEVN");
            Map(p => p.Kbfnbper).ToColumn("KBFNBPER");
            Map(p => p.Kbfnom).ToColumn("KBFNOM");
            Map(p => p.Kbfpnom).ToColumn("KBFPNOM");
            Map(p => p.Kbfdatnai).ToColumn("KBFDATNAI");
            Map(p => p.Kbffonc).ToColumn("KBFFONC");
            Map(p => p.Kbfcdec).ToColumn("KBFCDEC");
            Map(p => p.Kbfcip).ToColumn("KBFCIP");
            Map(p => p.Kbfaccs).ToColumn("KBFACCS");
            Map(p => p.Kbfavpr).ToColumn("KBFAVPR");
            Map(p => p.Kbfmsr).ToColumn("KBFMSR");
            Map(p => p.Kbfcmat).ToColumn("KBFCMAT");
            Map(p => p.Kbfsex).ToColumn("KBFSEX");
            Map(p => p.Kbfmdq).ToColumn("KBFMDQ");
            Map(p => p.Kbfmda).ToColumn("KBFMDA");
            Map(p => p.Kbfactp).ToColumn("KBFACTP");
            Map(p => p.Kbfkadfh).ToColumn("KBFKADFH");
            Map(p => p.Kbfext).ToColumn("KBFEXT");
            Map(p => p.Kbfmnt3).ToColumn("KBFMNT3");
            Map(p => p.Kbfmnt4).ToColumn("KBFMNT4");
            Map(p => p.Kbfqua).ToColumn("KBFQUA");
            Map(p => p.Kbfren).ToColumn("KBFREN");
            Map(p => p.Kbfrlo).ToColumn("KBFRLO");
            Map(p => p.Kbfpay).ToColumn("KBFPAY");
            Map(p => p.Kbfsit2).ToColumn("KBFSIT2");
            Map(p => p.Kbfadh2).ToColumn("KBFADH2");
            Map(p => p.Kbfsit3).ToColumn("KBFSIT3");
            Map(p => p.Kbfadh3).ToColumn("KBFADH3");
            Map(p => p.Kbfdes2).ToColumn("KBFDES2");
            Map(p => p.Kbfdes3).ToColumn("KBFDES3");
            Map(p => p.Kbfdes4).ToColumn("KBFDES4");
            Map(p => p.Kbfmrq).ToColumn("KBFMRQ");
            Map(p => p.Kbfmod).ToColumn("KBFMOD");
            Map(p => p.Kbfimm).ToColumn("KBFIMM");
            Map(p => p.Kbfmim).ToColumn("KBFMIM");
        }
    }
  

}
