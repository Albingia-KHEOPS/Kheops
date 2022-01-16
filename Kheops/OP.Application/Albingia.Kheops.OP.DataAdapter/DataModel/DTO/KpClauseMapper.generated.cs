using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpClauseMapper : EntityMap<KpClause>   {
        public KpClauseMapper () {
            Map(p => p.Kcaid).ToColumn("KCAID");
            Map(p => p.Kcatyp).ToColumn("KCATYP");
            Map(p => p.Kcaipb).ToColumn("KCAIPB");
            Map(p => p.Kcaalx).ToColumn("KCAALX");
            Map(p => p.Kcaavn).ToColumn("KCAAVN");
            Map(p => p.Kcahin).ToColumn("KCAHIN");
            Map(p => p.Kcaetape).ToColumn("KCAETAPE");
            Map(p => p.Kcaperi).ToColumn("KCAPERI");
            Map(p => p.Kcarsq).ToColumn("KCARSQ");
            Map(p => p.Kcaobj).ToColumn("KCAOBJ");
            Map(p => p.Kcainven).ToColumn("KCAINVEN");
            Map(p => p.Kcainlgn).ToColumn("KCAINLGN");
            Map(p => p.Kcafor).ToColumn("KCAFOR");
            Map(p => p.Kcaopt).ToColumn("KCAOPT");
            Map(p => p.Kcagar).ToColumn("KCAGAR");
            Map(p => p.Kcactx).ToColumn("KCACTX");
            Map(p => p.Kcaajt).ToColumn("KCAAJT");
            Map(p => p.Kcanta).ToColumn("KCANTA");
            Map(p => p.Kcakduid).ToColumn("KCAKDUID");
            Map(p => p.Kcaclnm1).ToColumn("KCACLNM1");
            Map(p => p.Kcaclnm2).ToColumn("KCACLNM2");
            Map(p => p.Kcaclnm3).ToColumn("KCACLNM3");
            Map(p => p.Kcaver).ToColumn("KCAVER");
            Map(p => p.Kcatxl).ToColumn("KCATXL");
            Map(p => p.Kcamer).ToColumn("KCAMER");
            Map(p => p.Kcadoc).ToColumn("KCADOC");
            Map(p => p.Kcachi).ToColumn("KCACHI");
            Map(p => p.Kcachis).ToColumn("KCACHIS");
            Map(p => p.Kcaimp).ToColumn("KCAIMP");
            Map(p => p.Kcacxi).ToColumn("KCACXI");
            Map(p => p.Kcaian).ToColumn("KCAIAN");
            Map(p => p.Kcaiac).ToColumn("KCAIAC");
            Map(p => p.Kcasit).ToColumn("KCASIT");
            Map(p => p.Kcasitd).ToColumn("KCASITD");
            Map(p => p.Kcaavnc).ToColumn("KCAAVNC");
            Map(p => p.Kcacrd).ToColumn("KCACRD");
            Map(p => p.Kcaavnm).ToColumn("KCAAVNM");
            Map(p => p.Kcamajd).ToColumn("KCAMAJD");
            Map(p => p.Kcaspa).ToColumn("KCASPA");
            Map(p => p.Kcatypo).ToColumn("KCATYPO");
            Map(p => p.Kcaaim).ToColumn("KCAAIM");
            Map(p => p.Kcatae).ToColumn("KCATAE");
            Map(p => p.Kcaelgo).ToColumn("KCAELGO");
            Map(p => p.Kcaelgi).ToColumn("KCAELGI");
            Map(p => p.Kcaxtl).ToColumn("KCAXTL");
            Map(p => p.Kcatypd).ToColumn("KCATYPD");
            Map(p => p.Kcaetaff).ToColumn("KCAETAFF");
            Map(p => p.Kcaxtlm).ToColumn("KCAXTLM");
        }
    }
  

}
