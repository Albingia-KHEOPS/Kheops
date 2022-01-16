using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YSinCumMapper : EntityMap<YSinCum>   {
        public YSinCumMapper () {
            Map(p => p.Susua).ToColumn("SUSUA");
            Map(p => p.Sunum).ToColumn("SUNUM");
            Map(p => p.Susbr).ToColumn("SUSBR");
            Map(p => p.Suipb).ToColumn("SUIPB");
            Map(p => p.Sualx).ToColumn("SUALX");
            Map(p => p.Suavn).ToColumn("SUAVN");
            Map(p => p.Sutbr).ToColumn("SUTBR");
            Map(p => p.Sutsb).ToColumn("SUTSB");
            Map(p => p.Sutpo).ToColumn("SUTPO");
            Map(p => p.Sutpe).ToColumn("SUTPE");
            Map(p => p.Sutpa).ToColumn("SUTPA");
            Map(p => p.Sutin).ToColumn("SUTIN");
            Map(p => p.Sutfr).ToColumn("SUTFR");
            Map(p => p.Sutre).ToColumn("SUTRE");
            Map(p => p.Sutch).ToColumn("SUTCH");
            Map(p => p.Suapo).ToColumn("SUAPO");
            Map(p => p.Suape).ToColumn("SUAPE");
            Map(p => p.Suapa).ToColumn("SUAPA");
            Map(p => p.Suain).ToColumn("SUAIN");
            Map(p => p.Suafr).ToColumn("SUAFR");
            Map(p => p.Suare).ToColumn("SUARE");
            Map(p => p.Suach).ToColumn("SUACH");
            Map(p => p.Sukpo).ToColumn("SUKPO");
            Map(p => p.Sukpe).ToColumn("SUKPE");
            Map(p => p.Sukpa).ToColumn("SUKPA");
            Map(p => p.Sukin).ToColumn("SUKIN");
            Map(p => p.Sukfr).ToColumn("SUKFR");
            Map(p => p.Sukre).ToColumn("SUKRE");
            Map(p => p.Sukch).ToColumn("SUKCH");
            Map(p => p.Sucpo).ToColumn("SUCPO");
            Map(p => p.Sucpe).ToColumn("SUCPE");
            Map(p => p.Sucpa).ToColumn("SUCPA");
            Map(p => p.Sucin).ToColumn("SUCIN");
            Map(p => p.Sucfr).ToColumn("SUCFR");
            Map(p => p.Sucre).ToColumn("SUCRE");
            Map(p => p.Succh).ToColumn("SUCCH");
            Map(p => p.Sumju).ToColumn("SUMJU");
            Map(p => p.Sumja).ToColumn("SUMJA");
            Map(p => p.Sumjm).ToColumn("SUMJM");
            Map(p => p.Sumjj).ToColumn("SUMJJ");
            Map(p => p.Sumjh).ToColumn("SUMJH");
            Map(p => p.Sutpp).ToColumn("SUTPP");
            Map(p => p.Sutpf).ToColumn("SUTPF");
            Map(p => p.Suapp).ToColumn("SUAPP");
            Map(p => p.Suapf).ToColumn("SUAPF");
            Map(p => p.Sukpp).ToColumn("SUKPP");
            Map(p => p.Sukpf).ToColumn("SUKPF");
            Map(p => p.Sucpp).ToColumn("SUCPP");
            Map(p => p.Sucpf).ToColumn("SUCPF");
        }
    }
}
