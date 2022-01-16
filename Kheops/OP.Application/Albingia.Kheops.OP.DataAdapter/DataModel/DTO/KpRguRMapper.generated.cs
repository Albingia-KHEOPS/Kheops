using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpRguRMapper : EntityMap<KpRguR>   {
        public KpRguRMapper () {
            Map(p => p.Kilid).ToColumn("KILID");
            Map(p => p.Kilkhwid).ToColumn("KILKHWID");
            Map(p => p.Kiltyp).ToColumn("KILTYP");
            Map(p => p.Kilipb).ToColumn("KILIPB");
            Map(p => p.Kilalx).ToColumn("KILALX");
            Map(p => p.Kilrsq).ToColumn("KILRSQ");
            Map(p => p.Kildebp).ToColumn("KILDEBP");
            Map(p => p.Kilfinp).ToColumn("KILFINP");
            Map(p => p.Kilsit).ToColumn("KILSIT");
            Map(p => p.Kilmhc).ToColumn("KILMHC");
            Map(p => p.Kilfrc).ToColumn("KILFRC");
            Map(p => p.Kilfr0).ToColumn("KILFR0");
            Map(p => p.Kilmht).ToColumn("KILMHT");
            Map(p => p.Kilmtx).ToColumn("KILMTX");
            Map(p => p.Kilttt).ToColumn("KILTTT");
            Map(p => p.Kilmtt).ToColumn("KILMTT");
            Map(p => p.Kilcnh).ToColumn("KILCNH");
            Map(p => p.Kilcnt).ToColumn("KILCNT");
            Map(p => p.Kilgrm).ToColumn("KILGRM");
            Map(p => p.Kilpro).ToColumn("KILPRO");
            Map(p => p.Kilech).ToColumn("KILECH");
            Map(p => p.Kilect).ToColumn("KILECT");
            Map(p => p.Kilkea).ToColumn("KILKEA");
            Map(p => p.Kilktd).ToColumn("KILKTD");
            Map(p => p.Kilasv).ToColumn("KILASV");
            Map(p => p.Kilpbt).ToColumn("KILPBT");
            Map(p => p.Kilmin).ToColumn("KILMIN");
            Map(p => p.Kilbrnc).ToColumn("KILBRNC");
            Map(p => p.Kilpba).ToColumn("KILPBA");
            Map(p => p.Kilpbs).ToColumn("KILPBS");
            Map(p => p.Kilpbr).ToColumn("KILPBR");
            Map(p => p.Kilpbp).ToColumn("KILPBP");
            Map(p => p.Kilria).ToColumn("KILRIA");
            Map(p => p.Kilspre).ToColumn("KILSPRE");
            Map(p => p.Kilriaf).ToColumn("KILRIAF");
            Map(p => p.Kilemh).ToColumn("KILEMH");
            Map(p => p.Kilemt).ToColumn("KILEMT");
            Map(p => p.Kilemhf).ToColumn("KILEMHF");
            Map(p => p.Kilemtf).ToColumn("KILEMTF");
            Map(p => p.Kilcot).ToColumn("KILCOT");
            Map(p => p.Kilbrnt).ToColumn("KILBRNT");
            Map(p => p.Kilschg).ToColumn("KILSCHG");
            Map(p => p.Kilsidf).ToColumn("KILSIDF");
            Map(p => p.Kilsrec).ToColumn("KILSREC");
            Map(p => p.Kilspro).ToColumn("KILSPRO");
            Map(p => p.Kilsrep).ToColumn("KILSREP");
            Map(p => p.Kilsrim).ToColumn("KILSRIM");
            Map(p => p.Kilpbtr).ToColumn("KILPBTR");
            Map(p => p.Kilemd).ToColumn("KILEMD");
            Map(p => p.Kilspc).ToColumn("KILSPC");
        }
    }
}
