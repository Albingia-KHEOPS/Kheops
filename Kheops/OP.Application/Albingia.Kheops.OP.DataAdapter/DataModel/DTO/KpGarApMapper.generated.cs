using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpGarApMapper : EntityMap<KpGarAp>   {
        public KpGarApMapper () {
            Map(p => p.Kdfid).ToColumn("KDFID");
            Map(p => p.Kdftyp).ToColumn("KDFTYP");
            Map(p => p.Kdfipb).ToColumn("KDFIPB");
            Map(p => p.Kdfalx).ToColumn("KDFALX");
            Map(p => p.Kdfavn).ToColumn("KDFAVN");
            Map(p => p.Kdfhin).ToColumn("KDFHIN");
            Map(p => p.Kdffor).ToColumn("KDFFOR");
            Map(p => p.Kdfopt).ToColumn("KDFOPT");
            Map(p => p.Kdfgaran).ToColumn("KDFGARAN");
            Map(p => p.Kdfkdeid).ToColumn("KDFKDEID");
            Map(p => p.Kdfgan).ToColumn("KDFGAN");
            Map(p => p.Kdfperi).ToColumn("KDFPERI");
            Map(p => p.Kdfrsq).ToColumn("KDFRSQ");
            Map(p => p.Kdfobj).ToColumn("KDFOBJ");
            Map(p => p.Kdfinven).ToColumn("KDFINVEN");
            Map(p => p.Kdfinvep).ToColumn("KDFINVEP");
            Map(p => p.Kdfcru).ToColumn("KDFCRU");
            Map(p => p.Kdfcrd).ToColumn("KDFCRD");
            Map(p => p.Kdfmaju).ToColumn("KDFMAJU");
            Map(p => p.Kdfmajd).ToColumn("KDFMAJD");
            Map(p => p.Kdfprv).ToColumn("KDFPRV");
            Map(p => p.Kdfpra).ToColumn("KDFPRA");
            Map(p => p.Kdfprw).ToColumn("KDFPRW");
            Map(p => p.Kdfpru).ToColumn("KDFPRU");
            Map(p => p.Kdftyc).ToColumn("KDFTYC");
            Map(p => p.Kdfmnt).ToColumn("KDFMNT");
        }
    }
  

}
