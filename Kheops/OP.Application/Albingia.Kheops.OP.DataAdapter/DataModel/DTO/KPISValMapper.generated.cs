using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO
{
    public partial class KPISValMapper : EntityMap<KPISVal>
    {
        public KPISValMapper()
        {
            Map(p => p.Kkcid).ToColumn("KKCID");
            Map(p => p.Kkctyp).ToColumn("KKCTYP");
            Map(p => p.Kkcipb).ToColumn("KKCIPB");
            Map(p => p.Kkcalx).ToColumn("KKCALX");
            Map(p => p.Kkcavn).ToColumn("KKCAVN");
            Map(p => p.Kkchin).ToColumn("KKCHIN");
            Map(p => p.Kkcrsq).ToColumn("KKCRSQ");
            Map(p => p.Kkcobj).ToColumn("KKCOBJ");
            Map(p => p.Kkcfor).ToColumn("KKCFOR");
            Map(p => p.Kkcopt).ToColumn("KKCOPT");
            Map(p => p.Kkckgbnmid).ToColumn("KKCKGBNMID");
            Map(p => p.Kkcvdec).ToColumn("KKCVDEC");
            Map(p => p.Kkcvun).ToColumn("KKCVUN");
            Map(p => p.Kkcvdatd).ToColumn("KKCVDATD");
            Map(p => p.Kkcvheud).ToColumn("KKCVHEUD");
            Map(p => p.Kkcvdatf).ToColumn("KKCVDATF");
            Map(p => p.Kkcvheuf).ToColumn("KKCVHEUF");
            Map(p => p.Kkcvtxt).ToColumn("KKCVTXT");
            Map(p => p.Kkckfbid).ToColumn("KKCKFBID");
            Map(p => p.Kkcisval).ToColumn("KKCISVAL");
        }
    }


}
