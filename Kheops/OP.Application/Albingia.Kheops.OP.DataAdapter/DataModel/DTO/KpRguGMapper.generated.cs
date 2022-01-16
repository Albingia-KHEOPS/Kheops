using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpRguGMapper : EntityMap<KpRguG>   {
        public KpRguGMapper () {
            Map(p => p.Khxid).ToColumn("KHXID");
            Map(p => p.Khxkhwid).ToColumn("KHXKHWID");
            Map(p => p.Khxtyp).ToColumn("KHXTYP");
            Map(p => p.Khxipb).ToColumn("KHXIPB");
            Map(p => p.Khxalx).ToColumn("KHXALX");
            Map(p => p.Khxrsq).ToColumn("KHXRSQ");
            Map(p => p.Khxfor).ToColumn("KHXFOR");
            Map(p => p.Khxkdeid).ToColumn("KHXKDEID");
            Map(p => p.Khxgaran).ToColumn("KHXGARAN");
            Map(p => p.Khxdebp).ToColumn("KHXDEBP");
            Map(p => p.Khxfinp).ToColumn("KHXFINP");
            Map(p => p.Khxsit).ToColumn("KHXSIT");
            Map(p => p.Khxtrg).ToColumn("KHXTRG");
            Map(p => p.Khxnpe).ToColumn("KHXNPE");
            Map(p => p.Khxven).ToColumn("KHXVEN");
            Map(p => p.Khxcaf).ToColumn("KHXCAF");
            Map(p => p.Khxcau).ToColumn("KHXCAU");
            Map(p => p.Khxcae).ToColumn("KHXCAE");
            Map(p => p.Khxmhc).ToColumn("KHXMHC");
            Map(p => p.Khxfrc).ToColumn("KHXFRC");
            Map(p => p.Khxfr0).ToColumn("KHXFR0");
            Map(p => p.Khxmht).ToColumn("KHXMHT");
            Map(p => p.Khxmtx).ToColumn("KHXMTX");
            Map(p => p.Khxttt).ToColumn("KHXTTT");
            Map(p => p.Khxmtt).ToColumn("KHXMTT");
            Map(p => p.Khxcnh).ToColumn("KHXCNH");
            Map(p => p.Khxcnt).ToColumn("KHXCNT");
            Map(p => p.Khxgrm).ToColumn("KHXGRM");
            Map(p => p.Khxpro).ToColumn("KHXPRO");
            Map(p => p.Khxech).ToColumn("KHXECH");
            Map(p => p.Khxect).ToColumn("KHXECT");
            Map(p => p.Khxemh).ToColumn("KHXEMH");
            Map(p => p.Khxemt).ToColumn("KHXEMT");
            Map(p => p.Khxdm1).ToColumn("KHXDM1");
            Map(p => p.Khxdt1).ToColumn("KHXDT1");
            Map(p => p.Khxdm2).ToColumn("KHXDM2");
            Map(p => p.Khxdt2).ToColumn("KHXDT2");
            Map(p => p.Khxcoe).ToColumn("KHXCOE");
            Map(p => p.Khxca1).ToColumn("KHXCA1");
            Map(p => p.Khxct1).ToColumn("KHXCT1");
            Map(p => p.Khxcu1).ToColumn("KHXCU1");
            Map(p => p.Khxcp1).ToColumn("KHXCP1");
            Map(p => p.Khxcx1).ToColumn("KHXCX1");
            Map(p => p.Khxca2).ToColumn("KHXCA2");
            Map(p => p.Khxct2).ToColumn("KHXCT2");
            Map(p => p.Khxcu2).ToColumn("KHXCU2");
            Map(p => p.Khxcp2).ToColumn("KHXCP2");
            Map(p => p.Khxcx2).ToColumn("KHXCX2");
            Map(p => p.Khxaju).ToColumn("KHXAJU");
            Map(p => p.Khxlmr).ToColumn("KHXLMR");
            Map(p => p.Khxmba).ToColumn("KHXMBA");
            Map(p => p.Khxten).ToColumn("KHXTEN");
            Map(p => p.Khxhon).ToColumn("KHXHON");
            Map(p => p.Khxhox).ToColumn("KHXHOX");
            Map(p => p.Khxbrg).ToColumn("KHXBRG");
            Map(p => p.Khxbrl).ToColumn("KHXBRL");
            Map(p => p.Khxbas).ToColumn("KHXBAS");
            Map(p => p.Khxbat).ToColumn("KHXBAT");
            Map(p => p.Khxbau).ToColumn("KHXBAU");
            Map(p => p.Khxbam).ToColumn("KHXBAM");
            Map(p => p.Khxxf1).ToColumn("KHXXF1");
            Map(p => p.Khxxb1).ToColumn("KHXXB1");
            Map(p => p.Khxxm1).ToColumn("KHXXM1");
            Map(p => p.Khxxf2).ToColumn("KHXXF2");
            Map(p => p.Khxxb2).ToColumn("KHXXB2");
            Map(p => p.Khxxm2).ToColumn("KHXXM2");
            Map(p => p.Khxxf3).ToColumn("KHXXF3");
            Map(p => p.Khxxb3).ToColumn("KHXXB3");
            Map(p => p.Khxxm3).ToColumn("KHXXM3");
            Map(p => p.Khxreg).ToColumn("KHXREG");
            Map(p => p.Khxpei).ToColumn("KHXPEI");
            Map(p => p.Khxkea).ToColumn("KHXKEA");
            Map(p => p.Khxpbp).ToColumn("KHXPBP");
            Map(p => p.Khxktd).ToColumn("KHXKTD");
            Map(p => p.Khxasv).ToColumn("KHXASV");
            Map(p => p.Khxpbt).ToColumn("KHXPBT");
            Map(p => p.Khxsip).ToColumn("KHXSIP");
            Map(p => p.Khxpbs).ToColumn("KHXPBS");
            Map(p => p.Khxris).ToColumn("KHXRIS");
            Map(p => p.Khxpbr).ToColumn("KHXPBR");
            Map(p => p.Khxria).ToColumn("KHXRIA");
            Map(p => p.Khxavn).ToColumn("KHXAVN");
        }
    }
  

}
