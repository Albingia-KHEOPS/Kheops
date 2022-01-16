using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YprtObjMapper : EntityMap<YprtObj>   {
        public YprtObjMapper () {
            Map(p => p.Jgipb).ToColumn("JGIPB");
            Map(p => p.Jgalx).ToColumn("JGALX");
            Map(p => p.Jgavn).ToColumn("JGAVN");
            Map(p => p.Jghin).ToColumn("JGHIN");
            Map(p => p.Jgrsq).ToColumn("JGRSQ");
            Map(p => p.Jgobj).ToColumn("JGOBJ");
            Map(p => p.Jgcch).ToColumn("JGCCH");
            Map(p => p.Jgigd).ToColumn("JGIGD");
            Map(p => p.Jgbra).ToColumn("JGBRA");
            Map(p => p.Jgsbr).ToColumn("JGSBR");
            Map(p => p.Jgcat).ToColumn("JGCAT");
            Map(p => p.Jgrcs).ToColumn("JGRCS");
            Map(p => p.Jgccs).ToColumn("JGCCS");
            Map(p => p.Jgval).ToColumn("JGVAL");
            Map(p => p.Jgvaa).ToColumn("JGVAA");
            Map(p => p.Jgvaw).ToColumn("JGVAW");
            Map(p => p.Jgvat).ToColumn("JGVAT");
            Map(p => p.Jgvau).ToColumn("JGVAU");
            Map(p => p.Jgvah).ToColumn("JGVAH");
            Map(p => p.Jgnoj).ToColumn("JGNOJ");
            Map(p => p.Jgmmq).ToColumn("JGMMQ");
            Map(p => p.Jgmty).ToColumn("JGMTY");
            Map(p => p.Jgmsr).ToColumn("JGMSR");
            Map(p => p.Jgmfa).ToColumn("JGMFA");
            Map(p => p.Jgtem).ToColumn("JGTEM");
            Map(p => p.Jgvgd).ToColumn("JGVGD");
            Map(p => p.Jgvgu).ToColumn("JGVGU");
            Map(p => p.Jgvda).ToColumn("JGVDA");
            Map(p => p.Jgvdm).ToColumn("JGVDM");
            Map(p => p.Jgvdj).ToColumn("JGVDJ");
            Map(p => p.Jgvdh).ToColumn("JGVDH");
            Map(p => p.Jgvfa).ToColumn("JGVFA");
            Map(p => p.Jgvfm).ToColumn("JGVFM");
            Map(p => p.Jgvfj).ToColumn("JGVFJ");
            Map(p => p.Jgvfh).ToColumn("JGVFH");
            Map(p => p.Jgrgt).ToColumn("JGRGT");
            Map(p => p.Jgtrr).ToColumn("JGTRR");
            Map(p => p.Jgcna).ToColumn("JGCNA");
            Map(p => p.Jgina).ToColumn("JGINA");
            Map(p => p.Jgind).ToColumn("JGIND");
            Map(p => p.Jgixc).ToColumn("JGIXC");
            Map(p => p.Jgixf).ToColumn("JGIXF");
            Map(p => p.Jgixl).ToColumn("JGIXL");
            Map(p => p.Jgixp).ToColumn("JGIXP");
            Map(p => p.Jgivo).ToColumn("JGIVO");
            Map(p => p.Jgiva).ToColumn("JGIVA");
            Map(p => p.Jgivw).ToColumn("JGIVW");
            Map(p => p.Jggau).ToColumn("JGGAU");
            Map(p => p.Jggvl).ToColumn("JGGVL");
            Map(p => p.Jggun).ToColumn("JGGUN");
            Map(p => p.Jgpbn).ToColumn("JGPBN");
            Map(p => p.Jgpbs).ToColumn("JGPBS");
            Map(p => p.Jgpbr).ToColumn("JGPBR");
            Map(p => p.Jgpbt).ToColumn("JGPBT");
            Map(p => p.Jgpbc).ToColumn("JGPBC");
            Map(p => p.Jgpbp).ToColumn("JGPBP");
            Map(p => p.Jgpba).ToColumn("JGPBA");
            Map(p => p.Jgclv).ToColumn("JGCLV");
            Map(p => p.Jgagm).ToColumn("JGAGM");
            Map(p => p.Jgave).ToColumn("JGAVE");
            Map(p => p.Jgava).ToColumn("JGAVA");
            Map(p => p.Jgavm).ToColumn("JGAVM");
            Map(p => p.Jgavj).ToColumn("JGAVJ");
            Map(p => p.Jgrul).ToColumn("JGRUL");
            Map(p => p.Jgrut).ToColumn("JGRUT");
            Map(p => p.Jgavf).ToColumn("JGAVF");
        }
    }
  

}
