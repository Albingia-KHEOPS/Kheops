using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YprtForMapper : EntityMap<YprtFor>   {
        public YprtForMapper () {
            Map(p => p.Joipb).ToColumn("JOIPB");
            Map(p => p.Joalx).ToColumn("JOALX");
            Map(p => p.Joavn).ToColumn("JOAVN");
            Map(p => p.Johin).ToColumn("JOHIN");
            Map(p => p.Jorsq).ToColumn("JORSQ");
            Map(p => p.Jofor).ToColumn("JOFOR");
            Map(p => p.Jodes).ToColumn("JODES");
            Map(p => p.Jocch).ToColumn("JOCCH");
            Map(p => p.Jofrv).ToColumn("JOFRV");
            Map(p => p.Jofsp).ToColumn("JOFSP");
            Map(p => p.Jorle).ToColumn("JORLE");
            Map(p => p.Joacq).ToColumn("JOACQ");
            Map(p => p.Jotmc).ToColumn("JOTMC");
            Map(p => p.Jotff).ToColumn("JOTFF");
            Map(p => p.Jotfp).ToColumn("JOTFP");
            Map(p => p.Jopro).ToColumn("JOPRO");
            Map(p => p.Jotmi).ToColumn("JOTMI");
            Map(p => p.Jotfm).ToColumn("JOTFM");
            Map(p => p.Jotma).ToColumn("JOTMA");
            Map(p => p.Jotmg).ToColumn("JOTMG");
            Map(p => p.Jocmc).ToColumn("JOCMC");
            Map(p => p.Jocfo).ToColumn("JOCFO");
            Map(p => p.Jocht).ToColumn("JOCHT");
            Map(p => p.Joctt).ToColumn("JOCTT");
            Map(p => p.Joccp).ToColumn("JOCCP");
            Map(p => p.Jocpa).ToColumn("JOCPA");
            Map(p => p.Joval).ToColumn("JOVAL");
            Map(p => p.Jovaa).ToColumn("JOVAA");
            Map(p => p.Jovaw).ToColumn("JOVAW");
            Map(p => p.Jovat).ToColumn("JOVAT");
            Map(p => p.Jovau).ToColumn("JOVAU");
            Map(p => p.Jovah).ToColumn("JOVAH");
            Map(p => p.Joivo).ToColumn("JOIVO");
            Map(p => p.Joiva).ToColumn("JOIVA");
            Map(p => p.Joivw).ToColumn("JOIVW");
            Map(p => p.Joave).ToColumn("JOAVE");
            Map(p => p.Joava).ToColumn("JOAVA");
            Map(p => p.Joavm).ToColumn("JOAVM");
            Map(p => p.Joavj).ToColumn("JOAVJ");
            Map(p => p.Jopaq).ToColumn("JOPAQ");
            Map(p => p.Joehh).ToColumn("JOEHH");
            Map(p => p.Joehc).ToColumn("JOEHC");
            Map(p => p.Joehi).ToColumn("JOEHI");
            Map(p => p.Jocos).ToColumn("JOCOS");
            Map(p => p.Jotqu).ToColumn("JOTQU");
        }
    }
  

}
