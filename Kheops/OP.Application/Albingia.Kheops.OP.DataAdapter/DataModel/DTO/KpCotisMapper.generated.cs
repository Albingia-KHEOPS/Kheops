using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpCotisMapper : EntityMap<KpCotis>   {
        public KpCotisMapper () {
            Map(p => p.Kdmid).ToColumn("KDMID");
            Map(p => p.Kdmtap).ToColumn("KDMTAP");
            Map(p => p.Kdmtyp).ToColumn("KDMTYP");
            Map(p => p.Kdmipb).ToColumn("KDMIPB");
            Map(p => p.Kdmalx).ToColumn("KDMALX");
            Map(p => p.Kdmavn).ToColumn("KDMAVN");
            Map(p => p.Kdmhin).ToColumn("KDMHIN");
            Map(p => p.Kdmatgmht).ToColumn("KDMATGMHT");
            Map(p => p.Kdmatgkht).ToColumn("KDMATGKHT");
            Map(p => p.Kdmatgmtx).ToColumn("KDMATGMTX");
            Map(p => p.Kdmatgktx).ToColumn("KDMATGKTX");
            Map(p => p.Kdmatgmtt).ToColumn("KDMATGMTT");
            Map(p => p.Kdmatgktt).ToColumn("KDMATGKTT");
            Map(p => p.Kdmatgcot).ToColumn("KDMATGCOT");
            Map(p => p.Kdmatgkco).ToColumn("KDMATGKCO");
            Map(p => p.Kdmcnabas).ToColumn("KDMCNABAS");
            Map(p => p.Kdmcnakbs).ToColumn("KDMCNAKBS");
            Map(p => p.Kdmcnamht).ToColumn("KDMCNAMHT");
            Map(p => p.Kdmcnakht).ToColumn("KDMCNAKHT");
            Map(p => p.Kdmcnamtx).ToColumn("KDMCNAMTX");
            Map(p => p.Kdmcnaktx).ToColumn("KDMCNAKTX");
            Map(p => p.Kdmcnamtt).ToColumn("KDMCNAMTT");
            Map(p => p.Kdmcnaktt).ToColumn("KDMCNAKTT");
            Map(p => p.Kdmcnacob).ToColumn("KDMCNACOB");
            Map(p => p.Kdmcnacnc).ToColumn("KDMCNACNC");
            Map(p => p.Kdmcnatxf).ToColumn("KDMCNATXF");
            Map(p => p.Kdmcnacnm).ToColumn("KDMCNACNM");
            Map(p => p.Kdmcnacmf).ToColumn("KDMCNACMF");
            Map(p => p.Kdmcnakcm).ToColumn("KDMCNAKCM");
            Map(p => p.Kdmgarmht).ToColumn("KDMGARMHT");
            Map(p => p.Kdmgarmtx).ToColumn("KDMGARMTX");
            Map(p => p.Kdmgarmtt).ToColumn("KDMGARMTT");
            Map(p => p.Kdmhfmht).ToColumn("KDMHFMHT");
            Map(p => p.Kdmhfflag).ToColumn("KDMHFFLAG");
            Map(p => p.Kdmhfmhf).ToColumn("KDMHFMHF");
            Map(p => p.Kdmhfmtx).ToColumn("KDMHFMTX");
            Map(p => p.Kdmhfmtt).ToColumn("KDMHFMTT");
            Map(p => p.Kdmafrb).ToColumn("KDMAFRB");
            Map(p => p.Kdmafr).ToColumn("KDMAFR");
            Map(p => p.Kdmkfa).ToColumn("KDMKFA");
            Map(p => p.Kdmaft).ToColumn("KDMAFT");
            Map(p => p.Kdmkft).ToColumn("KDMKFT");
            Map(p => p.Kdmfga).ToColumn("KDMFGA");
            Map(p => p.Kdmkfg).ToColumn("KDMKFG");
            Map(p => p.Kdmmht).ToColumn("KDMMHT");
            Map(p => p.Kdmmhflag).ToColumn("KDMMHFLAG");
            Map(p => p.Kdmmhf).ToColumn("KDMMHF");
            Map(p => p.Kdmkht).ToColumn("KDMKHT");
            Map(p => p.Kdmmtx).ToColumn("KDMMTX");
            Map(p => p.Kdmktx).ToColumn("KDMKTX");
            Map(p => p.Kdmmtt).ToColumn("KDMMTT");
            Map(p => p.Kdmmtflag).ToColumn("KDMMTFLAG");
            Map(p => p.Kdmttf).ToColumn("KDMTTF");
            Map(p => p.Kdmktt).ToColumn("KDMKTT");
            Map(p => p.Kdmcob).ToColumn("KDMCOB");
            Map(p => p.Kdmcom).ToColumn("KDMCOM");
            Map(p => p.Kdmcmf).ToColumn("KDMCMF");
            Map(p => p.Kdmcot).ToColumn("KDMCOT");
            Map(p => p.Kdmcof).ToColumn("KDMCOF");
            Map(p => p.Kdmkco).ToColumn("KDMKCO");
            Map(p => p.Kdmcoefc).ToColumn("KDMCOEFC");
        }
    }
  

}
