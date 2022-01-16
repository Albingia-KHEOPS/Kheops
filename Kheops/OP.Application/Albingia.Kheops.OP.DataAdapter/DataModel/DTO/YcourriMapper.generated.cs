using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YcourriMapper : EntityMap<Ycourri>   {
        public YcourriMapper () {
            Map(p => p.Dacou).ToColumn("DACOU");
            Map(p => p.Daipb).ToColumn("DAIPB");
            Map(p => p.Daalx).ToColumn("DAALX");
            Map(p => p.Daavn).ToColumn("DAAVN");
            Map(p => p.Daasi).ToColumn("DAASI");
            Map(p => p.Dansi).ToColumn("DANSI");
            Map(p => p.Dassi).ToColumn("DASSI");
            Map(p => p.Daarc).ToColumn("DAARC");
            Map(p => p.Danrc).ToColumn("DANRC");
            Map(p => p.Datbr).ToColumn("DATBR");
            Map(p => p.Dasrv).ToColumn("DASRV");
            Map(p => p.Dattr).ToColumn("DATTR");
            Map(p => p.Datlt).ToColumn("DATLT");
            Map(p => p.Dalet).ToColumn("DALET");
            Map(p => p.Daver).ToColumn("DAVER");
            Map(p => p.Dafml).ToColumn("DAFML");
            Map(p => p.Datds).ToColumn("DATDS");
            Map(p => p.Datyi).ToColumn("DATYI");
            Map(p => p.Daids).ToColumn("DAIDS");
            Map(p => p.Dainl).ToColumn("DAINL");
            Map(p => p.Dasit).ToColumn("DASIT");
            Map(p => p.Dasta).ToColumn("DASTA");
            Map(p => p.Dastm).ToColumn("DASTM");
            Map(p => p.Dastj).ToColumn("DASTJ");
            Map(p => p.Daspa).ToColumn("DASPA");
            Map(p => p.Daspm).ToColumn("DASPM");
            Map(p => p.Daspj).ToColumn("DASPJ");
            Map(p => p.Dalbc).ToColumn("DALBC");
            Map(p => p.Dator).ToColumn("DATOR");
            Map(p => p.Datev).ToColumn("DATEV");
            Map(p => p.Datae).ToColumn("DATAE");
            Map(p => p.Dancp).ToColumn("DANCP");
            Map(p => p.Dasou).ToColumn("DASOU");
            Map(p => p.Dages).ToColumn("DAGES");
            Map(p => p.Dabuc).ToColumn("DABUC");
            Map(p => p.Dabus).ToColumn("DABUS");
            Map(p => p.Dacru).ToColumn("DACRU");
            Map(p => p.Dacra).ToColumn("DACRA");
            Map(p => p.Dacrm).ToColumn("DACRM");
            Map(p => p.Dacrj).ToColumn("DACRJ");
            Map(p => p.Damju).ToColumn("DAMJU");
            Map(p => p.Damja).ToColumn("DAMJA");
            Map(p => p.Damjm).ToColumn("DAMJM");
            Map(p => p.Damjj).ToColumn("DAMJJ");
            Map(p => p.Dales).ToColumn("DALES");
            Map(p => p.Daenv).ToColumn("DAENV");
            Map(p => p.Dacrh).ToColumn("DACRH");
            Map(p => p.Damjh).ToColumn("DAMJH");
            Map(p => p.Dacrp).ToColumn("DACRP");
            Map(p => p.Damjp).ToColumn("DAMJP");
            Map(p => p.Dalto).ToColumn("DALTO");
            Map(p => p.Danur).ToColumn("DANUR");
            Map(p => p.Darfg).ToColumn("DARFG");
            Map(p => p.Dain5).ToColumn("DAIN5");
        }
    }
}
