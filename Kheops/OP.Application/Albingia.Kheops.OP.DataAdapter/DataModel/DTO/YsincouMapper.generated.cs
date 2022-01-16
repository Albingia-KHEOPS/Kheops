using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YsincouMapper : EntityMap<Ysincou>   {
        public YsincouMapper () {
            Map(p => p.Shsua).ToColumn("SHSUA");
            Map(p => p.Shnum).ToColumn("SHNUM");
            Map(p => p.Shsbr).ToColumn("SHSBR");
            Map(p => p.Shnuc).ToColumn("SHNUC");
            Map(p => p.Shtbr).ToColumn("SHTBR");
            Map(p => p.Shsrv).ToColumn("SHSRV");
            Map(p => p.Shttr).ToColumn("SHTTR");
            Map(p => p.Shelg).ToColumn("SHELG");
            Map(p => p.Shcds).ToColumn("SHCDS");
            Map(p => p.Shlet).ToColumn("SHLET");
            Map(p => p.Shnae).ToColumn("SHNAE");
            Map(p => p.Shtev).ToColumn("SHTEV");
            Map(p => p.Shtae).ToColumn("SHTAE");
            Map(p => p.Shaff).ToColumn("SHAFF");
            Map(p => p.Shlbc).ToColumn("SHLBC");
            Map(p => p.Shtlt).ToColumn("SHTLT");
            Map(p => p.Shtds).ToColumn("SHTDS");
            Map(p => p.Shtyi).ToColumn("SHTYI");
            Map(p => p.Shids).ToColumn("SHIDS");
            Map(p => p.Shinl).ToColumn("SHINL");
            Map(p => p.Shsit).ToColumn("SHSIT");
            Map(p => p.Shsta).ToColumn("SHSTA");
            Map(p => p.Shstm).ToColumn("SHSTM");
            Map(p => p.Shstj).ToColumn("SHSTJ");
            Map(p => p.Shspa).ToColumn("SHSPA");
            Map(p => p.Shspm).ToColumn("SHSPM");
            Map(p => p.Shspj).ToColumn("SHSPJ");
            Map(p => p.Shncp).ToColumn("SHNCP");
            Map(p => p.Shajt).ToColumn("SHAJT");
            Map(p => p.Shtvl).ToColumn("SHTVL");
            Map(p => p.Shcru).ToColumn("SHCRU");
            Map(p => p.Shcra).ToColumn("SHCRA");
            Map(p => p.Shcrm).ToColumn("SHCRM");
            Map(p => p.Shcrj).ToColumn("SHCRJ");
            Map(p => p.Shmju).ToColumn("SHMJU");
            Map(p => p.Shmja).ToColumn("SHMJA");
            Map(p => p.Shmjm).ToColumn("SHMJM");
            Map(p => p.Shmjj).ToColumn("SHMJJ");
            Map(p => p.Shles).ToColumn("SHLES");
            Map(p => p.Shenv).ToColumn("SHENV");
            Map(p => p.Shchr).ToColumn("SHCHR");
            Map(p => p.Shrcd).ToColumn("SHRCD");
            Map(p => p.Shrcc).ToColumn("SHRCC");
            Map(p => p.Shcou).ToColumn("SHCOU");
            Map(p => p.Shcdo).ToColumn("SHCDO");
            Map(p => p.Shact).ToColumn("SHACT");
            Map(p => p.Shtrf).ToColumn("SHTRF");
            Map(p => p.Shcop).ToColumn("SHCOP");
            Map(p => p.Shrfg).ToColumn("SHRFG");
            Map(p => p.Shin5).ToColumn("SHIN5");
        }
    }
  

}
