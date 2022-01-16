using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public  class CourrierContratMapper : EntityMap<CourrierContrat> {
        public CourrierContratMapper () {
            Map(p => p.Dacou).ToColumn("Dacou");
            Map(p => p.Daipb).ToColumn("Daipb");
            Map(p => p.Daalx).ToColumn("Daalx");
            Map(p => p.Daavn).ToColumn("Daavn");
            Map(p => p.Daasi).ToColumn("Daasi");
            Map(p => p.Dansi).ToColumn("Dansi");
            Map(p => p.Dassi).ToColumn("Dassi");
            Map(p => p.Daarc).ToColumn("Daarc");
            Map(p => p.Danrc).ToColumn("Danrc");
            Map(p => p.Datbr).ToColumn("Datbr");
            Map(p => p.Dasrv).ToColumn("Dasrv");
            Map(p => p.Dattr).ToColumn("Dattr");
            Map(p => p.Datlt).ToColumn("Datlt");
            Map(p => p.Dalet).ToColumn("Dalet");
            Map(p => p.Daver).ToColumn("Daver");
            Map(p => p.Dafml).ToColumn("Dafml");
            Map(p => p.Datds).ToColumn("Datds");
            Map(p => p.Datyi).ToColumn("Datyi");
            Map(p => p.Daids).ToColumn("Daids");
            Map(p => p.Dainl).ToColumn("Dainl");
            Map(p => p.Dasit).ToColumn("Dasit");
            Map(p => p.Dasta).ToColumn("Dasta");
            Map(p => p.Dastm).ToColumn("Dastm");
            Map(p => p.Dastj).ToColumn("Dastj");
            Map(p => p.Daspa).ToColumn("Daspa");
            Map(p => p.Daspm).ToColumn("Daspm");
            Map(p => p.Daspj).ToColumn("Daspj");
            Map(p => p.Dalbc).ToColumn("Dalbc");
            Map(p => p.Dator).ToColumn("Dator");
            Map(p => p.Datev).ToColumn("Datev");
            Map(p => p.Datae).ToColumn("Datae");
            Map(p => p.Dancp).ToColumn("Dancp");
            Map(p => p.Dasou).ToColumn("Dasou");
            Map(p => p.Dages).ToColumn("Dages");
            Map(p => p.Dabuc).ToColumn("Dabuc");
            Map(p => p.Dabus).ToColumn("Dabus");
            Map(p => p.Dacru).ToColumn("Dacru");
            Map(p => p.Dacra).ToColumn("Dacra");
            Map(p => p.Dacrm).ToColumn("Dacrm");
            Map(p => p.Dacrj).ToColumn("Dacrj");
            Map(p => p.Damju).ToColumn("Damju");
            Map(p => p.Damja).ToColumn("Damja");
            Map(p => p.Damjm).ToColumn("Damjm");
            Map(p => p.Damjj).ToColumn("Damjj");
            Map(p => p.Dales).ToColumn("Dales");
            Map(p => p.Daenv).ToColumn("Daenv");
            Map(p => p.Dacrh).ToColumn("Dacrh");
            Map(p => p.Damjh).ToColumn("Damjh");
            Map(p => p.Dacrp).ToColumn("Dacrp");
            Map(p => p.Damjp).ToColumn("Damjp");
            Map(p => p.Dalto).ToColumn("Dalto");
            Map(p => p.Danur).ToColumn("Danur");
            Map(p => p.Darfg).ToColumn("Darfg");
            Map(p => p.Dain5).ToColumn("Dain5");
            Map(p => p.Lgid4).ToColumn("Lgid4");
            Map(p => p.Lgchd).ToColumn("Lgchd");
            Map(p => p.Lgdoc).ToColumn("Lgdoc");
            Map(p => p.Lgext).ToColumn("Lgext");
            Map(p => p.Lgfpv).ToColumn("Lgfpv");
            Map(p => p.Lgcru).ToColumn("Lgcru");
        }
    }
  

}
