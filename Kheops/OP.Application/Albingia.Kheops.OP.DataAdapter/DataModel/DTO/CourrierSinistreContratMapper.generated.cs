using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public  class CourrierSinistreContratMapper : EntityMap<CourrierSinistreContrat> {
        public CourrierSinistreContratMapper () {
            Map(p => p.Shsua).ToColumn("Shsua");
            Map(p => p.Shnum).ToColumn("Shnum");
            Map(p => p.Shsbr).ToColumn("Shsbr");
            Map(p => p.Shnuc).ToColumn("Shnuc");
            Map(p => p.Shtbr).ToColumn("Shtbr");
            Map(p => p.Shsrv).ToColumn("Shsrv");
            Map(p => p.Shttr).ToColumn("Shttr");
            Map(p => p.Shelg).ToColumn("Shelg");
            Map(p => p.Shcds).ToColumn("Shcds");
            Map(p => p.Shlet).ToColumn("Shlet");
            Map(p => p.Shnae).ToColumn("Shnae");
            Map(p => p.Shtev).ToColumn("Shtev");
            Map(p => p.Shtae).ToColumn("Shtae");
            Map(p => p.Shaff).ToColumn("Shaff");
            Map(p => p.Shlbc).ToColumn("Shlbc");
            Map(p => p.Shtlt).ToColumn("Shtlt");
            Map(p => p.Shtds).ToColumn("Shtds");
            Map(p => p.Shtyi).ToColumn("Shtyi");
            Map(p => p.Shids).ToColumn("Shids");
            Map(p => p.Shinl).ToColumn("Shinl");
            Map(p => p.Shsit).ToColumn("Shsit");
            Map(p => p.Shsta).ToColumn("Shsta");
            Map(p => p.Shstm).ToColumn("Shstm");
            Map(p => p.Shstj).ToColumn("Shstj");
            Map(p => p.Shspa).ToColumn("Shspa");
            Map(p => p.Shspm).ToColumn("Shspm");
            Map(p => p.Shspj).ToColumn("Shspj");
            Map(p => p.Shncp).ToColumn("Shncp");
            Map(p => p.Shajt).ToColumn("Shajt");
            Map(p => p.Shtvl).ToColumn("Shtvl");
            Map(p => p.Shcru).ToColumn("Shcru");
            Map(p => p.Shcra).ToColumn("Shcra");
            Map(p => p.Shcrm).ToColumn("Shcrm");
            Map(p => p.Shcrj).ToColumn("Shcrj");
            Map(p => p.Shmju).ToColumn("Shmju");
            Map(p => p.Shmja).ToColumn("Shmja");
            Map(p => p.Shmjm).ToColumn("Shmjm");
            Map(p => p.Shmjj).ToColumn("Shmjj");
            Map(p => p.Shles).ToColumn("Shles");
            Map(p => p.Shenv).ToColumn("Shenv");
            Map(p => p.Shchr).ToColumn("Shchr");
            Map(p => p.Shrcd).ToColumn("Shrcd");
            Map(p => p.Shrcc).ToColumn("Shrcc");
            Map(p => p.Shcou).ToColumn("Shcou");
            Map(p => p.Shcdo).ToColumn("Shcdo");
            Map(p => p.Shact).ToColumn("Shact");
            Map(p => p.Shtrf).ToColumn("Shtrf");
            Map(p => p.Shcop).ToColumn("Shcop");
            Map(p => p.Shrfg).ToColumn("Shrfg");
            Map(p => p.Shin5).ToColumn("Shin5");
            Map(p => p.Lgid4).ToColumn("Lgid4");
            Map(p => p.Lgchd).ToColumn("Lgchd");
            Map(p => p.Lgdoc).ToColumn("Lgdoc");
            Map(p => p.Lgext).ToColumn("Lgext");
            Map(p => p.Lgfpv).ToColumn("Lgfpv");
            Map(p => p.Lgcru).ToColumn("Lgcru");
        }
    }
  

}
