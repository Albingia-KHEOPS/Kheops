using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpRguWpMapper : EntityMap<KpRguWp>   {
        public KpRguWpMapper () {
            Map(p => p.Khytyp).ToColumn("KHYTYP");
            Map(p => p.Khyipb).ToColumn("KHYIPB");
            Map(p => p.Khyalx).ToColumn("KHYALX");
            Map(p => p.Khyrsq).ToColumn("KHYRSQ");
            Map(p => p.Khyfor).ToColumn("KHYFOR");
            Map(p => p.Khykdeid).ToColumn("KHYKDEID");
            Map(p => p.Khygaran).ToColumn("KHYGARAN");
            Map(p => p.Khydebp).ToColumn("KHYDEBP");
            Map(p => p.Khyfinp).ToColumn("KHYFINP");
            Map(p => p.Khytrg).ToColumn("KHYTRG");
            Map(p => p.Khynpe).ToColumn("KHYNPE");
            Map(p => p.Khyven).ToColumn("KHYVEN");
            Map(p => p.Khycaf).ToColumn("KHYCAF");
            Map(p => p.Khycau).ToColumn("KHYCAU");
            Map(p => p.Khycae).ToColumn("KHYCAE");
            Map(p => p.Khydm1).ToColumn("KHYDM1");
            Map(p => p.Khydt1).ToColumn("KHYDT1");
            Map(p => p.Khydm2).ToColumn("KHYDM2");
            Map(p => p.Khydt2).ToColumn("KHYDT2");
            Map(p => p.Khycoe).ToColumn("KHYCOE");
            Map(p => p.Khyca1).ToColumn("KHYCA1");
            Map(p => p.Khyct1).ToColumn("KHYCT1");
            Map(p => p.Khycu1).ToColumn("KHYCU1");
            Map(p => p.Khycp1).ToColumn("KHYCP1");
            Map(p => p.Khycx1).ToColumn("KHYCX1");
            Map(p => p.Khyca2).ToColumn("KHYCA2");
            Map(p => p.Khyct2).ToColumn("KHYCT2");
            Map(p => p.Khycu2).ToColumn("KHYCU2");
            Map(p => p.Khycp2).ToColumn("KHYCP2");
            Map(p => p.Khycx2).ToColumn("KHYCX2");
            Map(p => p.Khyaju).ToColumn("KHYAJU");
            Map(p => p.Khylmr).ToColumn("KHYLMR");
            Map(p => p.Khymba).ToColumn("KHYMBA");
            Map(p => p.Khyten).ToColumn("KHYTEN");
            Map(p => p.Khybrg).ToColumn("KHYBRG");
            Map(p => p.Khybrl).ToColumn("KHYBRL");
            Map(p => p.Khybas).ToColumn("KHYBAS");
            Map(p => p.Khybat).ToColumn("KHYBAT");
            Map(p => p.Khybau).ToColumn("KHYBAU");
            Map(p => p.Khybam).ToColumn("KHYBAM");
            Map(p => p.Khyxf1).ToColumn("KHYXF1");
            Map(p => p.Khyxb1).ToColumn("KHYXB1");
            Map(p => p.Khyxm1).ToColumn("KHYXM1");
            Map(p => p.Khyxf2).ToColumn("KHYXF2");
            Map(p => p.Khyxb2).ToColumn("KHYXB2");
            Map(p => p.Khyxm2).ToColumn("KHYXM2");
            Map(p => p.Khyxf3).ToColumn("KHYXF3");
            Map(p => p.Khyxb3).ToColumn("KHYXB3");
            Map(p => p.Khyxm3).ToColumn("KHYXM3");
            Map(p => p.Khyreg).ToColumn("KHYREG");
            Map(p => p.Khypei).ToColumn("KHYPEI");
            Map(p => p.Khycnh).ToColumn("KHYCNH");
            Map(p => p.Khycnt).ToColumn("KHYCNT");
            Map(p => p.Khygrm).ToColumn("KHYGRM");
            Map(p => p.Khykea).ToColumn("KHYKEA");
            Map(p => p.Khypbp).ToColumn("KHYPBP");
            Map(p => p.Khyktd).ToColumn("KHYKTD");
            Map(p => p.Khyasv).ToColumn("KHYASV");
            Map(p => p.Khypbt).ToColumn("KHYPBT");
            Map(p => p.Khysip).ToColumn("KHYSIP");
            Map(p => p.Khypbs).ToColumn("KHYPBS");
            Map(p => p.Khyris).ToColumn("KHYRIS");
            Map(p => p.Khypbr).ToColumn("KHYPBR");
            Map(p => p.Khyria).ToColumn("KHYRIA");
            Map(p => p.Khyavn).ToColumn("KHYAVN");
        }
    }
  

}
