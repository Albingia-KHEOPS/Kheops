using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YprtRsqMapper : EntityMap<YprtRsq>   {
        public YprtRsqMapper () {
            Map(p => p.Jeipb).ToColumn("JEIPB");
            Map(p => p.Jealx).ToColumn("JEALX");
            Map(p => p.Jeavn).ToColumn("JEAVN");
            Map(p => p.Jehin).ToColumn("JEHIN");
            Map(p => p.Jersq).ToColumn("JERSQ");
            Map(p => p.Jecch).ToColumn("JECCH");
            Map(p => p.Jemgd).ToColumn("JEMGD");
            Map(p => p.Jebra).ToColumn("JEBRA");
            Map(p => p.Jesbr).ToColumn("JESBR");
            Map(p => p.Jecat).ToColumn("JECAT");
            Map(p => p.Jercs).ToColumn("JERCS");
            Map(p => p.Jeccs).ToColumn("JECCS");
            Map(p => p.Jeval).ToColumn("JEVAL");
            Map(p => p.Jevaa).ToColumn("JEVAA");
            Map(p => p.Jevaw).ToColumn("JEVAW");
            Map(p => p.Jevat).ToColumn("JEVAT");
            Map(p => p.Jevau).ToColumn("JEVAU");
            Map(p => p.Jevah).ToColumn("JEVAH");
            Map(p => p.Jetem).ToColumn("JETEM");
            Map(p => p.Jevgd).ToColumn("JEVGD");
            Map(p => p.Jevgu).ToColumn("JEVGU");
            Map(p => p.Jevda).ToColumn("JEVDA");
            Map(p => p.Jevdm).ToColumn("JEVDM");
            Map(p => p.Jevdj).ToColumn("JEVDJ");
            Map(p => p.Jevdh).ToColumn("JEVDH");
            Map(p => p.Jevfa).ToColumn("JEVFA");
            Map(p => p.Jevfm).ToColumn("JEVFM");
            Map(p => p.Jevfj).ToColumn("JEVFJ");
            Map(p => p.Jevfh).ToColumn("JEVFH");
            Map(p => p.Jeobj).ToColumn("JEOBJ");
            Map(p => p.Jeroj).ToColumn("JEROJ");
            Map(p => p.Jergt).ToColumn("JERGT");
            Map(p => p.Jetrr).ToColumn("JETRR");
            Map(p => p.Jecna).ToColumn("JECNA");
            Map(p => p.Jeina).ToColumn("JEINA");
            Map(p => p.Jeind).ToColumn("JEIND");
            Map(p => p.Jeixc).ToColumn("JEIXC");
            Map(p => p.Jeixf).ToColumn("JEIXF");
            Map(p => p.Jeixl).ToColumn("JEIXL");
            Map(p => p.Jeixp).ToColumn("JEIXP");
            Map(p => p.Jegau).ToColumn("JEGAU");
            Map(p => p.Jegvl).ToColumn("JEGVL");
            Map(p => p.Jegun).ToColumn("JEGUN");
            Map(p => p.Jepbn).ToColumn("JEPBN");
            Map(p => p.Jepbs).ToColumn("JEPBS");
            Map(p => p.Jepbr).ToColumn("JEPBR");
            Map(p => p.Jepbt).ToColumn("JEPBT");
            Map(p => p.Jepbc).ToColumn("JEPBC");
            Map(p => p.Jepbp).ToColumn("JEPBP");
            Map(p => p.Jepba).ToColumn("JEPBA");
            Map(p => p.Jeclv).ToColumn("JECLV");
            Map(p => p.Jeagm).ToColumn("JEAGM");
            Map(p => p.Jeipm).ToColumn("JEIPM");
            Map(p => p.Jeivx).ToColumn("JEIVX");
            Map(p => p.Jedro).ToColumn("JEDRO");
            Map(p => p.Jenbo).ToColumn("JENBO");
            Map(p => p.Jelcv).ToColumn("JELCV");
            Map(p => p.Jelca).ToColumn("JELCA");
            Map(p => p.Jelcw).ToColumn("JELCW");
            Map(p => p.Jelcu).ToColumn("JELCU");
            Map(p => p.Jelce).ToColumn("JELCE");
            Map(p => p.Jeave).ToColumn("JEAVE");
            Map(p => p.Jeava).ToColumn("JEAVA");
            Map(p => p.Jeavm).ToColumn("JEAVM");
            Map(p => p.Jeavj).ToColumn("JEAVJ");
            Map(p => p.Jerul).ToColumn("JERUL");
            Map(p => p.Jerut).ToColumn("JERUT");
            Map(p => p.Jeavf).ToColumn("JEAVF");
            Map(p => p.Jeext).ToColumn("JEEXT");
        }
    }
  

}
