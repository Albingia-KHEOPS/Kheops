using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YprtEntMapper : EntityMap<YprtEnt>   {
        public YprtEntMapper () {
            Map(p => p.Jdipb).ToColumn("JDIPB");
            Map(p => p.Jdalx).ToColumn("JDALX");
            Map(p => p.Jdavn).ToColumn("JDAVN");
            Map(p => p.Jdhin).ToColumn("JDHIN");
            Map(p => p.Jdsht).ToColumn("JDSHT");
            Map(p => p.Jdenc).ToColumn("JDENC");
            Map(p => p.Jditc).ToColumn("JDITC");
            Map(p => p.Jdval).ToColumn("JDVAL");
            Map(p => p.Jdvaa).ToColumn("JDVAA");
            Map(p => p.Jdvaw).ToColumn("JDVAW");
            Map(p => p.Jdvat).ToColumn("JDVAT");
            Map(p => p.Jdvau).ToColumn("JDVAU");
            Map(p => p.Jdvah).ToColumn("JDVAH");
            Map(p => p.Jddrq).ToColumn("JDDRQ");
            Map(p => p.Jdnbr).ToColumn("JDNBR");
            Map(p => p.Jdtxl).ToColumn("JDTXL");
            Map(p => p.Jdtrr).ToColumn("JDTRR");
            Map(p => p.Jdxcm).ToColumn("JDXCM");
            Map(p => p.Jdnex).ToColumn("JDNEX");
            Map(p => p.Jdnpa).ToColumn("JDNPA");
            Map(p => p.Jdafc).ToColumn("JDAFC");
            Map(p => p.Jdafr).ToColumn("JDAFR");
            Map(p => p.Jdatt).ToColumn("JDATT");
            Map(p => p.Jdcna).ToColumn("JDCNA");
            Map(p => p.Jdcnc).ToColumn("JDCNC");
            Map(p => p.Jdina).ToColumn("JDINA");
            Map(p => p.Jdind).ToColumn("JDIND");
            Map(p => p.Jdixc).ToColumn("JDIXC");
            Map(p => p.Jdixf).ToColumn("JDIXF");
            Map(p => p.Jdixl).ToColumn("JDIXL");
            Map(p => p.Jdixp).ToColumn("JDIXP");
            Map(p => p.Jdivo).ToColumn("JDIVO");
            Map(p => p.Jdiva).ToColumn("JDIVA");
            Map(p => p.Jdivw).ToColumn("JDIVW");
            Map(p => p.Jdmht).ToColumn("JDMHT");
            Map(p => p.Jdrea).ToColumn("JDREA");
            Map(p => p.Jdreb).ToColumn("JDREB");
            Map(p => p.Jdreh).ToColumn("JDREH");
            Map(p => p.Jddpv).ToColumn("JDDPV");
            Map(p => p.Jdgau).ToColumn("JDGAU");
            Map(p => p.Jdgvl).ToColumn("JDGVL");
            Map(p => p.Jdgun).ToColumn("JDGUN");
            Map(p => p.Jdpbn).ToColumn("JDPBN");
            Map(p => p.Jdpbs).ToColumn("JDPBS");
            Map(p => p.Jdpbr).ToColumn("JDPBR");
            Map(p => p.Jdpbt).ToColumn("JDPBT");
            Map(p => p.Jdpbc).ToColumn("JDPBC");
            Map(p => p.Jdpbp).ToColumn("JDPBP");
            Map(p => p.Jdpba).ToColumn("JDPBA");
            Map(p => p.Jdrcg).ToColumn("JDRCG");
            Map(p => p.Jdccg).ToColumn("JDCCG");
            Map(p => p.Jdrcs).ToColumn("JDRCS");
            Map(p => p.Jdccs).ToColumn("JDCCS");
            Map(p => p.Jdclv).ToColumn("JDCLV");
            Map(p => p.Jdagm).ToColumn("JDAGM");
            Map(p => p.Jdlcv).ToColumn("JDLCV");
            Map(p => p.Jdlca).ToColumn("JDLCA");
            Map(p => p.Jdlcw).ToColumn("JDLCW");
            Map(p => p.Jdlcu).ToColumn("JDLCU");
            Map(p => p.Jdlce).ToColumn("JDLCE");
            Map(p => p.Jddpa).ToColumn("JDDPA");
            Map(p => p.Jddpm).ToColumn("JDDPM");
            Map(p => p.Jddpj).ToColumn("JDDPJ");
            Map(p => p.Jdfpa).ToColumn("JDFPA");
            Map(p => p.Jdfpm).ToColumn("JDFPM");
            Map(p => p.Jdfpj).ToColumn("JDFPJ");
            Map(p => p.Jdpea).ToColumn("JDPEA");
            Map(p => p.Jdpem).ToColumn("JDPEM");
            Map(p => p.Jdpej).ToColumn("JDPEJ");
            Map(p => p.Jdacq).ToColumn("JDACQ");
            Map(p => p.Jdtmc).ToColumn("JDTMC");
            Map(p => p.Jdtfo).ToColumn("JDTFO");
            Map(p => p.Jdtft).ToColumn("JDTFT");
            Map(p => p.Jdtff).ToColumn("JDTFF");
            Map(p => p.Jdtfp).ToColumn("JDTFP");
            Map(p => p.Jdpro).ToColumn("JDPRO");
            Map(p => p.Jdtmi).ToColumn("JDTMI");
            Map(p => p.Jdtfm).ToColumn("JDTFM");
            Map(p => p.Jdtma).ToColumn("JDTMA");
            Map(p => p.Jdtmg).ToColumn("JDTMG");
            Map(p => p.Jdcmc).ToColumn("JDCMC");
            Map(p => p.Jdcfo).ToColumn("JDCFO");
            Map(p => p.Jdcft).ToColumn("JDCFT");
            Map(p => p.Jdcfh).ToColumn("JDCFH");
            Map(p => p.Jdcht).ToColumn("JDCHT");
            Map(p => p.Jdctt).ToColumn("JDCTT");
            Map(p => p.Jdccp).ToColumn("JDCCP");
            Map(p => p.Jdehh).ToColumn("JDEHH");
            Map(p => p.Jdehc).ToColumn("JDEHC");
            Map(p => p.Jdsmp).ToColumn("JDSMP");
            Map(p => p.Jdivx).ToColumn("JDIVX");
            Map(p => p.Jdtcr).ToColumn("JDTCR");
            Map(p => p.Jdnpg).ToColumn("JDNPG");
            Map(p => p.Jdedi).ToColumn("JDEDI");
            Map(p => p.Jdedn).ToColumn("JDEDN");
            Map(p => p.Jdeda).ToColumn("JDEDA");
            Map(p => p.Jdedm).ToColumn("JDEDM");
            Map(p => p.Jdedj).ToColumn("JDEDJ");
            Map(p => p.Jdehi).ToColumn("JDEHI");
            Map(p => p.Jdiax).ToColumn("JDIAX");
            Map(p => p.Jdted).ToColumn("JDTED");
            Map(p => p.Jddoo).ToColumn("JDDOO");
            Map(p => p.Jdrua).ToColumn("JDRUA");
            Map(p => p.Jdrum).ToColumn("JDRUM");
            Map(p => p.Jdruj).ToColumn("JDRUJ");
            Map(p => p.Jdecg).ToColumn("JDECG");
            Map(p => p.Jdecp).ToColumn("JDECP");
            Map(p => p.Jdapt).ToColumn("JDAPT");
            Map(p => p.Jdapr).ToColumn("JDAPR");
            Map(p => p.Jdaat).ToColumn("JDAAT");
            Map(p => p.Jdaar).ToColumn("JDAAR");
            Map(p => p.Jdacr).ToColumn("JDACR");
            Map(p => p.Jdacv).ToColumn("JDACV");
            Map(p => p.Jdaxt).ToColumn("JDAXT");
            Map(p => p.Jdaxc).ToColumn("JDAXC");
            Map(p => p.Jdaxf).ToColumn("JDAXF");
            Map(p => p.Jdaxm).ToColumn("JDAXM");
            Map(p => p.Jdaxg).ToColumn("JDAXG");
            Map(p => p.Jdata).ToColumn("JDATA");
            Map(p => p.Jdatx).ToColumn("JDATX");
            Map(p => p.Jdaut).ToColumn("JDAUT");
            Map(p => p.Jdauf).ToColumn("JDAUF");
            Map(p => p.Jdlta).ToColumn("JDLTA");
            Map(p => p.Jdltasp).ToColumn("JDLTASP");
            Map(p => p.Jdldeb).ToColumn("JDLDEB");
            Map(p => p.Jdldeh).ToColumn("JDLDEH");
            Map(p => p.Jdlfin).ToColumn("JDLFIN");
            Map(p => p.Jdlfih).ToColumn("JDLFIH");
            Map(p => p.Jdldur).ToColumn("JDLDUR");
            Map(p => p.Jdlduu).ToColumn("JDLDUU");
        }
    }
  

}