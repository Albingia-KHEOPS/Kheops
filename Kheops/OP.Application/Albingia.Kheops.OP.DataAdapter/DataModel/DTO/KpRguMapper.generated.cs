using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpRguMapper : EntityMap<KpRgu>   {
        public KpRguMapper () {
            Map(p => p.Khwid).ToColumn("KHWID");
            Map(p => p.Khwtyp).ToColumn("KHWTYP");
            Map(p => p.Khwipb).ToColumn("KHWIPB");
            Map(p => p.Khwalx).ToColumn("KHWALX");
            Map(p => p.Khwttr).ToColumn("KHWTTR");
            Map(p => p.Khwrgav).ToColumn("KHWRGAV");
            Map(p => p.Khwavn).ToColumn("KHWAVN");
            Map(p => p.Khwavnd).ToColumn("KHWAVND");
            Map(p => p.Khwexe).ToColumn("KHWEXE");
            Map(p => p.Khwdebp).ToColumn("KHWDEBP");
            Map(p => p.Khwfinp).ToColumn("KHWFINP");
            Map(p => p.Khwtrgu).ToColumn("KHWTRGU");
            Map(p => p.Khwipk).ToColumn("KHWIPK");
            Map(p => p.Khwict).ToColumn("KHWICT");
            Map(p => p.Khwicc).ToColumn("KHWICC");
            Map(p => p.Khwxcm).ToColumn("KHWXCM");
            Map(p => p.Khwcnc).ToColumn("KHWCNC");
            Map(p => p.Khwenc).ToColumn("KHWENC");
            Map(p => p.Khwafc).ToColumn("KHWAFC");
            Map(p => p.Khwafr).ToColumn("KHWAFR");
            Map(p => p.Khwatt).ToColumn("KHWATT");
            Map(p => p.Khwmht).ToColumn("KHWMHT");
            Map(p => p.Khwcnh).ToColumn("KHWCNH");
            Map(p => p.Khwgrg).ToColumn("KHWGRG");
            Map(p => p.Khwttt).ToColumn("KHWTTT");
            Map(p => p.Khwmtt).ToColumn("KHWMTT");
            Map(p => p.Khweta).ToColumn("KHWETA");
            Map(p => p.Khwsit).ToColumn("KHWSIT");
            Map(p => p.Khwstu).ToColumn("KHWSTU");
            Map(p => p.Khwstd).ToColumn("KHWSTD");
            Map(p => p.Khwsth).ToColumn("KHWSTH");
            Map(p => p.Khwcru).ToColumn("KHWCRU");
            Map(p => p.Khwcrd).ToColumn("KHWCRD");
            Map(p => p.Khwmaju).ToColumn("KHWMAJU");
            Map(p => p.Khwmajd).ToColumn("KHWMAJD");
            Map(p => p.Khwavp).ToColumn("KHWAVP");
            Map(p => p.Khwdesi).ToColumn("KHWDESI");
            Map(p => p.Khwobsv).ToColumn("KHWOBSV");
            Map(p => p.Khwobsc).ToColumn("KHWOBSC");
            Map(p => p.Khwmtf).ToColumn("KHWMTF");
            Map(p => p.Khwmrg).ToColumn("KHWMRG");
            Map(p => p.Khwacc).ToColumn("KHWACC");
            Map(p => p.Khwsuav).ToColumn("KHWSUAV");
            Map(p => p.Khwnem).ToColumn("KHWNEM");
            Map(p => p.Khwasv).ToColumn("KHWASV");
            Map(p => p.Khwmin).ToColumn("KHWMIN");
            Map(p => p.Khwbrnc).ToColumn("KHWBRNC");
            Map(p => p.Khwpbt).ToColumn("KHWPBT");
            Map(p => p.Khwpba).ToColumn("KHWPBA");
            Map(p => p.Khwpbs).ToColumn("KHWPBS");
            Map(p => p.Khwpbr).ToColumn("KHWPBR");
            Map(p => p.Khwpbp).ToColumn("KHWPBP");
            Map(p => p.Khwria).ToColumn("KHWRIA");
            Map(p => p.Khwriaf).ToColumn("KHWRIAF");
            Map(p => p.Khwemh).ToColumn("KHWEMH");
            Map(p => p.Khwemhf).ToColumn("KHWEMHF");
            Map(p => p.Khwcot).ToColumn("KHWCOT");
            Map(p => p.Khwbrnt).ToColumn("KHWBRNT");
            Map(p => p.Khwschg).ToColumn("KHWSCHG");
            Map(p => p.Khwsidf).ToColumn("KHWSIDF");
            Map(p => p.Khwsrec).ToColumn("KHWSREC");
            Map(p => p.Khwspro).ToColumn("KHWSPRO");
            Map(p => p.Khwspre).ToColumn("KHWSPRE");
            Map(p => p.Khwsrep).ToColumn("KHWSREP");
            Map(p => p.Khwsrim).ToColumn("KHWSRIM");
            Map(p => p.Khwmhc).ToColumn("KHWMHC");
            Map(p => p.Khwpbtr).ToColumn("KHWPBTR");
            Map(p => p.Khwemd).ToColumn("KHWEMD");
            Map(p => p.Khwspc).ToColumn("KHWSPC");
            Map(p => p.Khwmtx).ToColumn("KHWMTX");
            Map(p => p.Khwcnt).ToColumn("KHWCNT");
            Map(p => p.Khwect).ToColumn("KHWECT");
            Map(p => p.Khwemt).ToColumn("KHWEMT");
        }
    }
  

}
