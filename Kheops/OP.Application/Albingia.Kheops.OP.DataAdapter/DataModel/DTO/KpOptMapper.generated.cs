using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpOptMapper : EntityMap<KpOpt>   {
        public KpOptMapper () {
            Map(p => p.Kdbid).ToColumn("KDBID");
            Map(p => p.Kdbtyp).ToColumn("KDBTYP");
            Map(p => p.Kdbipb).ToColumn("KDBIPB");
            Map(p => p.Kdbalx).ToColumn("KDBALX");
            Map(p => p.Kdbavn).ToColumn("KDBAVN");
            Map(p => p.Kdbhin).ToColumn("KDBHIN");
            Map(p => p.Kdbfor).ToColumn("KDBFOR");
            Map(p => p.Kdbkdaid).ToColumn("KDBKDAID");
            Map(p => p.Kdbopt).ToColumn("KDBOPT");
            Map(p => p.Kdbdesc).ToColumn("KDBDESC");
            Map(p => p.Kdbforr).ToColumn("KDBFORR");
            Map(p => p.Kdbkdaidr).ToColumn("KDBKDAIDR");
            Map(p => p.Kdbspeid).ToColumn("KDBSPEID");
            Map(p => p.Kdbcru).ToColumn("KDBCRU");
            Map(p => p.Kdbcrd).ToColumn("KDBCRD");
            Map(p => p.Kdbcrh).ToColumn("KDBCRH");
            Map(p => p.Kdbmaju).ToColumn("KDBMAJU");
            Map(p => p.Kdbmajd).ToColumn("KDBMAJD");
            Map(p => p.Kdbmajh).ToColumn("KDBMAJH");
            Map(p => p.Kdbpaq).ToColumn("KDBPAQ");
            Map(p => p.Kdbacq).ToColumn("KDBACQ");
            Map(p => p.Kdbtmc).ToColumn("KDBTMC");
            Map(p => p.Kdbtff).ToColumn("KDBTFF");
            Map(p => p.Kdbtfp).ToColumn("KDBTFP");
            Map(p => p.Kdbpro).ToColumn("KDBPRO");
            Map(p => p.Kdbtmi).ToColumn("KDBTMI");
            Map(p => p.Kdbtfm).ToColumn("KDBTFM");
            Map(p => p.Kdbcmc).ToColumn("KDBCMC");
            Map(p => p.Kdbcfo).ToColumn("KDBCFO");
            Map(p => p.Kdbcht).ToColumn("KDBCHT");
            Map(p => p.Kdbctt).ToColumn("KDBCTT");
            Map(p => p.Kdbccp).ToColumn("KDBCCP");
            Map(p => p.Kdbval).ToColumn("KDBVAL");
            Map(p => p.Kdbvaa).ToColumn("KDBVAA");
            Map(p => p.Kdbvaw).ToColumn("KDBVAW");
            Map(p => p.Kdbvat).ToColumn("KDBVAT");
            Map(p => p.Kdbvau).ToColumn("KDBVAU");
            Map(p => p.Kdbvah).ToColumn("KDBVAH");
            Map(p => p.Kdbivo).ToColumn("KDBIVO");
            Map(p => p.Kdbiva).ToColumn("KDBIVA");
            Map(p => p.Kdbivw).ToColumn("KDBIVW");
            Map(p => p.Kdbave).ToColumn("KDBAVE");
            Map(p => p.Kdbavg).ToColumn("KDBAVG");
            Map(p => p.Kdbeco).ToColumn("KDBECO");
            Map(p => p.Kdbava).ToColumn("KDBAVA");
            Map(p => p.Kdbavm).ToColumn("KDBAVM");
            Map(p => p.Kdbavj).ToColumn("KDBAVJ");
            Map(p => p.Kdbehh).ToColumn("KDBEHH");
            Map(p => p.Kdbehc).ToColumn("KDBEHC");
            Map(p => p.Kdbehi).ToColumn("KDBEHI");
            Map(p => p.Kdbasvalo).ToColumn("KDBASVALO");
            Map(p => p.Kdbasvala).ToColumn("KDBASVALA");
            Map(p => p.Kdbasvalw).ToColumn("KDBASVALW");
            Map(p => p.Kdbasunit).ToColumn("KDBASUNIT");
            Map(p => p.Kdbasbase).ToColumn("KDBASBASE");
            Map(p => p.Kdbger).ToColumn("KDBGER");
        }
    }
  

}
