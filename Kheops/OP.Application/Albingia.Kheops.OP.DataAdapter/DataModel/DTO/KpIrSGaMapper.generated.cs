using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpIrSGaMapper : EntityMap<KpIrSGa>   {
        public KpIrSGaMapper () {
            Map(p => p.Kfdtyp).ToColumn("KFDTYP");
            Map(p => p.Kfdipb).ToColumn("KFDIPB");
            Map(p => p.Kfdalx).ToColumn("KFDALX");
            Map(p => p.Kfdavn).ToColumn("KFDAVN");
            Map(p => p.Kfdhin).ToColumn("KFDHIN");
            Map(p => p.Kfdfor).ToColumn("KFDFOR");
            Map(p => p.Kfdopt).ToColumn("KFDOPT");
            Map(p => p.Kfdcrd).ToColumn("KFDCRD");
            Map(p => p.Kfdcrh).ToColumn("KFDCRH");
            Map(p => p.Kfdmaju).ToColumn("KFDMAJU");
            Map(p => p.Kfdmajd).ToColumn("KFDMAJD");
            Map(p => p.Kfdmajh).ToColumn("KFDMAJH");
            Map(p => p.Kfdan02).ToColumn("KFDAN02");
            Map(p => p.Kfdan03).ToColumn("KFDAN03");
            Map(p => p.Kfdbo01).ToColumn("KFDBO01");
            Map(p => p.Kfdbo02).ToColumn("KFDBO02");
            Map(p => p.Kfdbo03).ToColumn("KFDBO03");
            Map(p => p.Kfdim08).ToColumn("KFDIM08");
            Map(p => p.Kfdim09).ToColumn("KFDIM09");
            Map(p => p.Kfdim10).ToColumn("KFDIM10");
            Map(p => p.Kfdnbgr).ToColumn("KFDNBGR");
            Map(p => p.Kfdeffv).ToColumn("KFDEFFV");
            Map(p => p.Kfdcnvd).ToColumn("KFDCNVD");
            Map(p => p.Kfdfrdm).ToColumn("KFDFRDM");
            Map(p => p.Kfdsorn).ToColumn("KFDSORN");
            Map(p => p.Kfdsord).ToColumn("KFDSORD");
            Map(p => p.Kfdsorr).ToColumn("KFDSORR");
            Map(p => p.Kfd05).ToColumn("KFD05");
            Map(p => p.Kfd06).ToColumn("KFD06");
            Map(p => p.Kfd07).ToColumn("KFD07");
            Map(p => p.Kfd08).ToColumn("KFD08");
            Map(p => p.Kfd09).ToColumn("KFD09");
            Map(p => p.Kfdia01).ToColumn("KFDIA01");
            Map(p => p.Kfdia02).ToColumn("KFDIA02");
            Map(p => p.Kfdia03).ToColumn("KFDIA03");
            Map(p => p.Kfdiara17).ToColumn("KFDIARA17");
            Map(p => p.Kfdrsat).ToColumn("KFDRSAT");
            Map(p => p.Kfdrcps).ToColumn("KFDRCPS");
            Map(p => p.Kfdrcpf).ToColumn("KFDRCPF");
            Map(p => p.Kfdrcpd).ToColumn("KFDRCPD");
            Map(p => p.Kfdrasb).ToColumn("KFDRASB");
            Map(p => p.Kfdrasl).ToColumn("KFDRASL");
            Map(p => p.Kfdrass).ToColumn("KFDRASS");
            Map(p => p.Kfdcotnb).ToColumn("KFDCOTNB");
            Map(p => p.Kfdcotmt).ToColumn("KFDCOTMT");
            Map(p => p.Kfdmnt).ToColumn("KFDMNT");
            Map(p => p.Kfdmntnb).ToColumn("KFDMNTNB");
            Map(p => p.Kfdmntmt).ToColumn("KFDMNTMT");
            Map(p => p.Kfdrgu).ToColumn("KFDRGU");
            Map(p => p.Kfdnbji).ToColumn("KFDNBJI");
            Map(p => p.Kfdan04).ToColumn("KFDAN04");
            Map(p => p.Kfdgarav).ToColumn("KFDGARAV");
            Map(p => p.Kfdvolav).ToColumn("KFDVOLAV");
            Map(p => p.Kfdvolap).ToColumn("KFDVOLAP");
            Map(p => p.Kfdlma).ToColumn("KFDLMA");
            Map(p => p.Kfdmxm).ToColumn("KFDMXM");
            Map(p => p.Kfdray).ToColumn("KFDRAY");
            Map(p => p.Kfdan05).ToColumn("KFDAN05");
            Map(p => p.Kfdray5).ToColumn("KFDRAY5");
            Map(p => p.Kfdan06).ToColumn("KFDAN06");
            Map(p => p.Kfdan07).ToColumn("KFDAN07");
            Map(p => p.Kfdclal).ToColumn("KFDCLAL");
        }
    }
  

}
