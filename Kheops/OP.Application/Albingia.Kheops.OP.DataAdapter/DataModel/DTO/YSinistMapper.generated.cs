using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YSinistMapper : EntityMap<YSinist>   {
        public YSinistMapper () {
            Map(p => p.Sisua).ToColumn("SISUA");
            Map(p => p.Sinum).ToColumn("SINUM");
            Map(p => p.Sisbr).ToColumn("SISBR");
            Map(p => p.Sisum).ToColumn("SISUM");
            Map(p => p.Sisuj).ToColumn("SISUJ");
            Map(p => p.Sioua).ToColumn("SIOUA");
            Map(p => p.Sioum).ToColumn("SIOUM");
            Map(p => p.Siouj).ToColumn("SIOUJ");
            Map(p => p.Siipb).ToColumn("SIIPB");
            Map(p => p.Sialx).ToColumn("SIALX");
            Map(p => p.Siavn).ToColumn("SIAVN");
            Map(p => p.Sitch).ToColumn("SITCH");
            Map(p => p.Sitbr).ToColumn("SITBR");
            Map(p => p.Sitsb).ToColumn("SITSB");
            Map(p => p.Siges).ToColumn("SIGES");
            Map(p => p.Sirca).ToColumn("SIRCA");
            Map(p => p.Sircm).ToColumn("SIRCM");
            Map(p => p.Sircj).ToColumn("SIRCJ");
            Map(p => p.Sintr).ToColumn("SINTR");
            Map(p => p.Sicau).ToColumn("SICAU");
            Map(p => p.Sijja).ToColumn("SIJJA");
            Map(p => p.Sijjm).ToColumn("SIJJM");
            Map(p => p.Sijjj).ToColumn("SIJJJ");
            Map(p => p.Siref).ToColumn("SIREF");
            Map(p => p.Siad1).ToColumn("SIAD1");
            Map(p => p.Siad2).ToColumn("SIAD2");
            Map(p => p.Sidep).ToColumn("SIDEP");
            Map(p => p.Sicpo).ToColumn("SICPO");
            Map(p => p.Sivil).ToColumn("SIVIL");
            Map(p => p.Sipay).ToColumn("SIPAY");
            Map(p => p.Siexp).ToColumn("SIEXP");
            Map(p => p.Siine).ToColumn("SIINE");
            Map(p => p.Sirex).ToColumn("SIREX");
            Map(p => p.Siexm).ToColumn("SIEXM");
            Map(p => p.Sierp).ToColumn("SIERP");
            Map(p => p.Siejp).ToColumn("SIEJP");
            Map(p => p.Sierd).ToColumn("SIERD");
            Map(p => p.Siejd).ToColumn("SIEJD");
            Map(p => p.Sinju).ToColumn("SINJU");
            Map(p => p.Siavo).ToColumn("SIAVO");
            Map(p => p.Siina).ToColumn("SIINA");
            Map(p => p.Sirav).ToColumn("SIRAV");
            Map(p => p.Siict).ToColumn("SIICT");
            Map(p => p.Siinc).ToColumn("SIINC");
            Map(p => p.Sirco).ToColumn("SIRCO");
            Map(p => p.Sidct).ToColumn("SIDCT");
            Map(p => p.Sicct).ToColumn("SICCT");
            Map(p => p.Siapr).ToColumn("SIAPR");
            Map(p => p.Sirap).ToColumn("SIRAP");
            Map(p => p.Siapp).ToColumn("SIAPP");
            Map(p => p.Sipcv).ToColumn("SIPCV");
            Map(p => p.Sinpl).ToColumn("SINPL");
            Map(p => p.Singe).ToColumn("SINGE");
            Map(p => p.Siacc).ToColumn("SIACC");
            Map(p => p.Sirfu).ToColumn("SIRFU");
            Map(p => p.Sicru).ToColumn("SICRU");
            Map(p => p.Sicra).ToColumn("SICRA");
            Map(p => p.Sicrm).ToColumn("SICRM");
            Map(p => p.Sicrj).ToColumn("SICRJ");
            Map(p => p.Simju).ToColumn("SIMJU");
            Map(p => p.Simja).ToColumn("SIMJA");
            Map(p => p.Simjm).ToColumn("SIMJM");
            Map(p => p.Simjj).ToColumn("SIMJJ");
            Map(p => p.Sisit).ToColumn("SISIT");
            Map(p => p.Sista).ToColumn("SISTA");
            Map(p => p.Sistm).ToColumn("SISTM");
            Map(p => p.Sistj).ToColumn("SISTJ");
            Map(p => p.Sieta).ToColumn("SIETA");
            Map(p => p.Sidre).ToColumn("SIDRE");
            Map(p => p.Sidrn).ToColumn("SIDRN");
            Map(p => p.Sicvg).ToColumn("SICVG");
            Map(p => p.Sirpa).ToColumn("SIRPA");
            Map(p => p.Sirpm).ToColumn("SIRPM");
            Map(p => p.Sirpj).ToColumn("SIRPJ");
            Map(p => p.Sirda).ToColumn("SIRDA");
            Map(p => p.Sirdm).ToColumn("SIRDM");
            Map(p => p.Sirdj).ToColumn("SIRDJ");
            Map(p => p.Sihia).ToColumn("SIHIA");
            Map(p => p.Sihim).ToColumn("SIHIM");
            Map(p => p.Sihij).ToColumn("SIHIJ");
            Map(p => p.Sihih).ToColumn("SIHIH");
            Map(p => p.Sihiu).ToColumn("SIHIU");
            Map(p => p.Sircd).ToColumn("SIRCD");
            Map(p => p.Siihb).ToColumn("SIIHB");
            Map(p => p.Siroa).ToColumn("SIROA");
            Map(p => p.Sirom).ToColumn("SIROM");
            Map(p => p.Siroj).ToColumn("SIROJ");
            Map(p => p.Sirou).ToColumn("SIROU");
            Map(p => p.Sidra).ToColumn("SIDRA");
            Map(p => p.Sidrm).ToColumn("SIDRM");
            Map(p => p.Sidrj).ToColumn("SIDRJ");
            Map(p => p.Sidba).ToColumn("SIDBA");
            Map(p => p.Sidbm).ToColumn("SIDBM");
            Map(p => p.Sidbj).ToColumn("SIDBJ");
            Map(p => p.Sicvr).ToColumn("SICVR");
            Map(p => p.Sis36).ToColumn("SIS36");
            Map(p => p.Simng).ToColumn("SIMNG");
            Map(p => p.Siclo).ToColumn("SICLO");
            Map(p => p.Siexn).ToColumn("SIEXN");
            Map(p => p.Sirct).ToColumn("SIRCT");
            Map(p => p.Simtb).ToColumn("SIMTB");
            Map(p => p.Simts).ToColumn("SIMTS");
            Map(p => p.Siemi).ToColumn("SIEMI");
            Map(p => p.Sioup).ToColumn("SIOUP");
            Map(p => p.Sicna).ToColumn("SICNA");
            Map(p => p.Sispp).ToColumn("SISPP");
            Map(p => p.Sisps).ToColumn("SISPS");
            Map(p => p.Simfp).ToColumn("SIMFP");
            Map(p => p.Sipf).ToColumn("SIPF");
            Map(p => p.Sicbc).ToColumn("SICBC");
            Map(p => p.Sibcr).ToColumn("SIBCR");
            Map(p => p.Sibou).ToColumn("SIBOU");
            Map(p => p.Sievn).ToColumn("SIEVN");
            Map(p => p.Sigar).ToColumn("SIGAR");
            Map(p => p.Sinnc).ToColumn("SINNC");
            Map(p => p.Siejr).ToColumn("SIEJR");
            Map(p => p.Siejs).ToColumn("SIEJS");
            Map(p => p.Sirua).ToColumn("SIRUA");
            Map(p => p.Sirum).ToColumn("SIRUM");
            Map(p => p.Siruj).ToColumn("SIRUJ");
            Map(p => p.Si15a).ToColumn("SI15A");
            Map(p => p.Si15m).ToColumn("SI15M");
            Map(p => p.Si15j).ToColumn("SI15J");
            Map(p => p.Siadh).ToColumn("SIADH");
            Map(p => p.Silco).ToColumn("SILCO");
            Map(p => p.Sifil).ToColumn("SIFIL");
            Map(p => p.Sicok).ToColumn("SICOK");
            Map(p => p.Sinlc).ToColumn("SINLC");
            Map(p => p.Sie1a).ToColumn("SIE1A");
            Map(p => p.Sie1m).ToColumn("SIE1M");
            Map(p => p.Sie1j).ToColumn("SIE1J");
            Map(p => p.Siin5).ToColumn("SIIN5");
        }
    }
}
