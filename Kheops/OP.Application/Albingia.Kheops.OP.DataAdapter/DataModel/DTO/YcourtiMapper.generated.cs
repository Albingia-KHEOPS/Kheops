using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YcourtiMapper : EntityMap<Ycourti>   {
        public YcourtiMapper () {
            Map(p => p.Tcict).ToColumn("TCICT");
            Map(p => p.Tctyp).ToColumn("TCTYP");
            Map(p => p.Tcici).ToColumn("TCICI");
            Map(p => p.Tcad1).ToColumn("TCAD1");
            Map(p => p.Tcad2).ToColumn("TCAD2");
            Map(p => p.Tcdep).ToColumn("TCDEP");
            Map(p => p.Tccpo).ToColumn("TCCPO");
            Map(p => p.Tcvil).ToColumn("TCVIL");
            Map(p => p.Tcpay).ToColumn("TCPAY");
            Map(p => p.Tccom).ToColumn("TCCOM");
            Map(p => p.Tcreg).ToColumn("TCREG");
            Map(p => p.Tcbur).ToColumn("TCBUR");
            Map(p => p.Tcfdc).ToColumn("TCFDC");
            Map(p => p.Tctel).ToColumn("TCTEL");
            Map(p => p.Tctlc).ToColumn("TCTLC");
            Map(p => p.Tcbqe).ToColumn("TCBQE");
            Map(p => p.Tcgui).ToColumn("TCGUI");
            Map(p => p.Tccpt).ToColumn("TCCPT");
            Map(p => p.Tcrib).ToColumn("TCRIB");
            Map(p => p.Tcicp).ToColumn("TCICP");
            Map(p => p.Tcori).ToColumn("TCORI");
            Map(p => p.Tctin).ToColumn("TCTIN");
            Map(p => p.Tccii).ToColumn("TCCII");
            Map(p => p.Tcapg).ToColumn("TCAPG");
            Map(p => p.Tclig).ToColumn("TCLIG");
            Map(p => p.Tcrgc).ToColumn("TCRGC");
            Map(p => p.Tcenc).ToColumn("TCENC");
            Map(p => p.Tcman).ToColumn("TCMAN");
            Map(p => p.Tcdcp).ToColumn("TCDCP");
            Map(p => p.Tcraf).ToColumn("TCRAF");
            Map(p => p.Tccha).ToColumn("TCCHA");
            Map(p => p.Tcgep).ToColumn("TCGEP");
            Map(p => p.Tcprd).ToColumn("TCPRD");
            Map(p => p.Tccra).ToColumn("TCCRA");
            Map(p => p.Tccrm).ToColumn("TCCRM");
            Map(p => p.Tccrj).ToColumn("TCCRJ");
            Map(p => p.Tcfva).ToColumn("TCFVA");
            Map(p => p.Tcfvm).ToColumn("TCFVM");
            Map(p => p.Tcfvj).ToColumn("TCFVJ");
            Map(p => p.Tcrpl).ToColumn("TCRPL");
            Map(p => p.Tcusr).ToColumn("TCUSR");
            Map(p => p.Tcmja).ToColumn("TCMJA");
            Map(p => p.Tcmjm).ToColumn("TCMJM");
            Map(p => p.Tcmjj).ToColumn("TCMJJ");
            Map(p => p.Tcold).ToColumn("TCOLD");
            Map(p => p.Tcape).ToColumn("TCAPE");
            Map(p => p.Tcins).ToColumn("TCINS");
            Map(p => p.Tcspe).ToColumn("TCSPE");
            Map(p => p.Tcyen).ToColumn("TCYEN");
            Map(p => p.Tcaem).ToColumn("TCAEM");
            Map(p => p.Tcioo).ToColumn("TCIOO");
            Map(p => p.Tcioc).ToColumn("TCIOC");
            Map(p => p.Tcioj).ToColumn("TCIOJ");
            Map(p => p.Tciom).ToColumn("TCIOM");
            Map(p => p.Tcioa).ToColumn("TCIOA");
            Map(p => p.Tciag).ToColumn("TCIAG");
            Map(p => p.Tciaj).ToColumn("TCIAJ");
            Map(p => p.Tciam).ToColumn("TCIAM");
            Map(p => p.Tciaa).ToColumn("TCIAA");
            Map(p => p.Tciii).ToColumn("TCIII");
            Map(p => p.Tciij).ToColumn("TCIIJ");
            Map(p => p.Tciim).ToColumn("TCIIM");
            Map(p => p.Tciia).ToColumn("TCIIA");
            Map(p => p.Tcimd).ToColumn("TCIMD");
            Map(p => p.Tcimj).ToColumn("TCIMJ");
            Map(p => p.Tcimm).ToColumn("TCIMM");
            Map(p => p.Tcima).ToColumn("TCIMA");
            Map(p => p.Tcadh).ToColumn("TCADH");
            Map(p => p.Tcrcv).ToColumn("TCRCV");
            Map(p => p.Tcrcs).ToColumn("TCRCS");
            Map(p => p.Tcinm).ToColumn("TCINM");
            Map(p => p.Tcsec).ToColumn("TCSEC");
            Map(p => p.Tcirn).ToColumn("TCIRN");
            Map(p => p.Tcirj).ToColumn("TCIRJ");
            Map(p => p.Tcirm).ToColumn("TCIRM");
            Map(p => p.Tcira).ToColumn("TCIRA");
            Map(p => p.Tcrcn).ToColumn("TCRCN");
            Map(p => p.Tcap5).ToColumn("TCAP5");
            Map(p => p.Tcedi).ToColumn("TCEDI");
            Map(p => p.Tcidn).ToColumn("TCIDN");
            Map(p => p.Tcist).ToColumn("TCIST");
            Map(p => p.Tcgem).ToColumn("TCGEM");
        }
    }
  

}
