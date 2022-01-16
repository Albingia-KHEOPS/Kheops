using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YprtGarMapper : EntityMap<YprtGar>   {
        public YprtGarMapper () {
            Map(p => p.Jhipb).ToColumn("JHIPB");
            Map(p => p.Jhalx).ToColumn("JHALX");
            Map(p => p.Jhavn).ToColumn("JHAVN");
            Map(p => p.Jhhin).ToColumn("JHHIN");
            Map(p => p.Jhrsq).ToColumn("JHRSQ");
            Map(p => p.Jhfor).ToColumn("JHFOR");
            Map(p => p.Jhgap).ToColumn("JHGAP");
            Map(p => p.Jhgar).ToColumn("JHGAR");
            Map(p => p.Jhcar).ToColumn("JHCAR");
            Map(p => p.Jhnat).ToColumn("JHNAT");
            Map(p => p.Jhgan).ToColumn("JHGAN");
            Map(p => p.Jhobn).ToColumn("JHOBN");
            Map(p => p.Jhob1).ToColumn("JHOB1");
            Map(p => p.Jhob2).ToColumn("JHOB2");
            Map(p => p.Jhob3).ToColumn("JHOB3");
            Map(p => p.Jhob4).ToColumn("JHOB4");
            Map(p => p.Jhob5).ToColumn("JHOB5");
            Map(p => p.Jhgav).ToColumn("JHGAV");
            Map(p => p.Jhrqv).ToColumn("JHRQV");
            Map(p => p.Jhfov).ToColumn("JHFOV");
            Map(p => p.Jhrep).ToColumn("JHREP");
            Map(p => p.Jhtax).ToColumn("JHTAX");
            Map(p => p.Jhlcv).ToColumn("JHLCV");
            Map(p => p.Jhlca).ToColumn("JHLCA");
            Map(p => p.Jhlcw).ToColumn("JHLCW");
            Map(p => p.Jhlcu).ToColumn("JHLCU");
            Map(p => p.Jhlce).ToColumn("JHLCE");
            Map(p => p.Jhlc1).ToColumn("JHLC1");
            Map(p => p.Jhlc2).ToColumn("JHLC2");
            Map(p => p.Jhlc3).ToColumn("JHLC3");
            Map(p => p.Jhlc4).ToColumn("JHLC4");
            Map(p => p.Jhlc5).ToColumn("JHLC5");
            Map(p => p.Jhlov).ToColumn("JHLOV");
            Map(p => p.Jhloa).ToColumn("JHLOA");
            Map(p => p.Jhlow).ToColumn("JHLOW");
            Map(p => p.Jhlou).ToColumn("JHLOU");
            Map(p => p.Jhloe).ToColumn("JHLOE");
            Map(p => p.Jhasv).ToColumn("JHASV");
            Map(p => p.Jhasa).ToColumn("JHASA");
            Map(p => p.Jhasw).ToColumn("JHASW");
            Map(p => p.Jhasu).ToColumn("JHASU");
            Map(p => p.Jhasn).ToColumn("JHASN");
            Map(p => p.Jhfhv).ToColumn("JHFHV");
            Map(p => p.Jhfha).ToColumn("JHFHA");
            Map(p => p.Jhfhw).ToColumn("JHFHW");
            Map(p => p.Jhfhu).ToColumn("JHFHU");
            Map(p => p.Jhfhe).ToColumn("JHFHE");
            Map(p => p.Jhprv).ToColumn("JHPRV");
            Map(p => p.Jhpra).ToColumn("JHPRA");
            Map(p => p.Jhprw).ToColumn("JHPRW");
            Map(p => p.Jhpru).ToColumn("JHPRU");
            Map(p => p.Jhprp).ToColumn("JHPRP");
            Map(p => p.Jhpre).ToColumn("JHPRE");
            Map(p => p.Jhprf).ToColumn("JHPRF");
            Map(p => p.Jhcna).ToColumn("JHCNA");
            Map(p => p.Jhina).ToColumn("JHINA");
            Map(p => p.Jhefa).ToColumn("JHEFA");
            Map(p => p.Jhefm).ToColumn("JHEFM");
            Map(p => p.Jhefj).ToColumn("JHEFJ");
            Map(p => p.Jhefh).ToColumn("JHEFH");
            Map(p => p.Jhfea).ToColumn("JHFEA");
            Map(p => p.Jhfem).ToColumn("JHFEM");
            Map(p => p.Jhfej).ToColumn("JHFEJ");
            Map(p => p.Jhfeh).ToColumn("JHFEH");
            Map(p => p.Jhefd).ToColumn("JHEFD");
            Map(p => p.Jhefu).ToColumn("JHEFU");
            Map(p => p.Jhtmc).ToColumn("JHTMC");
            Map(p => p.Jhtff).ToColumn("JHTFF");
            Map(p => p.Jhcmc).ToColumn("JHCMC");
            Map(p => p.Jhcht).ToColumn("JHCHT");
            Map(p => p.Jhctt).ToColumn("JHCTT");
            Map(p => p.Jhajt).ToColumn("JHAJT");
            Map(p => p.Jhdfg).ToColumn("JHDFG");
            Map(p => p.Jhifc).ToColumn("JHIFC");
            Map(p => p.Jhcda).ToColumn("JHCDA");
            Map(p => p.Jhcdm).ToColumn("JHCDM");
            Map(p => p.Jhcdj).ToColumn("JHCDJ");
            Map(p => p.Jhcfa).ToColumn("JHCFA");
            Map(p => p.Jhcfm).ToColumn("JHCFM");
            Map(p => p.Jhcfj).ToColumn("JHCFJ");
            Map(p => p.Jhave).ToColumn("JHAVE");
            Map(p => p.Jhavf).ToColumn("JHAVF");
        }
    }
  

}
