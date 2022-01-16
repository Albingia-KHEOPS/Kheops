using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpGaranMapper : EntityMap<KpGaran>   {
        public KpGaranMapper () {
            Map(p => p.Kdeid).ToColumn("KDEID");
            Map(p => p.Kdetyp).ToColumn("KDETYP");
            Map(p => p.Kdeipb).ToColumn("KDEIPB");
            Map(p => p.Kdealx).ToColumn("KDEALX");
            Map(p => p.Kdeavn).ToColumn("KDEAVN");
            Map(p => p.Kdehin).ToColumn("KDEHIN");
            Map(p => p.Kdefor).ToColumn("KDEFOR");
            Map(p => p.Kdeopt).ToColumn("KDEOPT");
            Map(p => p.Kdekdcid).ToColumn("KDEKDCID");
            Map(p => p.Kdegaran).ToColumn("KDEGARAN");
            Map(p => p.Kdeseq).ToColumn("KDESEQ");
            Map(p => p.Kdeniveau).ToColumn("KDENIVEAU");
            Map(p => p.Kdesem).ToColumn("KDESEM");
            Map(p => p.Kdese1).ToColumn("KDESE1");
            Map(p => p.Kdetri).ToColumn("KDETRI");
            Map(p => p.Kdenumpres).ToColumn("KDENUMPRES");
            Map(p => p.Kdeajout).ToColumn("KDEAJOUT");
            Map(p => p.Kdecar).ToColumn("KDECAR");
            Map(p => p.Kdenat).ToColumn("KDENAT");
            Map(p => p.Kdegan).ToColumn("KDEGAN");
            Map(p => p.Kdekdfid).ToColumn("KDEKDFID");
            Map(p => p.Kdedefg).ToColumn("KDEDEFG");
            Map(p => p.Kdekdhid).ToColumn("KDEKDHID");
            Map(p => p.Kdedatdeb).ToColumn("KDEDATDEB");
            Map(p => p.Kdeheudeb).ToColumn("KDEHEUDEB");
            Map(p => p.Kdedatfin).ToColumn("KDEDATFIN");
            Map(p => p.Kdeheufin).ToColumn("KDEHEUFIN");
            Map(p => p.Kdeduree).ToColumn("KDEDUREE");
            Map(p => p.Kdeduruni).ToColumn("KDEDURUNI");
            Map(p => p.Kdeprp).ToColumn("KDEPRP");
            Map(p => p.Kdetypemi).ToColumn("KDETYPEMI");
            Map(p => p.Kdealiref).ToColumn("KDEALIREF");
            Map(p => p.Kdecatnat).ToColumn("KDECATNAT");
            Map(p => p.Kdeina).ToColumn("KDEINA");
            Map(p => p.Kdetaxcod).ToColumn("KDETAXCOD");
            Map(p => p.Kdetaxrep).ToColumn("KDETAXREP");
            Map(p => p.Kdecravn).ToColumn("KDECRAVN");
            Map(p => p.Kdecru).ToColumn("KDECRU");
            Map(p => p.Kdecrd).ToColumn("KDECRD");
            Map(p => p.Kdemajavn).ToColumn("KDEMAJAVN");
            Map(p => p.Kdeasvalo).ToColumn("KDEASVALO");
            Map(p => p.Kdeasvala).ToColumn("KDEASVALA");
            Map(p => p.Kdeasvalw).ToColumn("KDEASVALW");
            Map(p => p.Kdeasunit).ToColumn("KDEASUNIT");
            Map(p => p.Kdeasbase).ToColumn("KDEASBASE");
            Map(p => p.Kdeasmod).ToColumn("KDEASMOD");
            Map(p => p.Kdeasobli).ToColumn("KDEASOBLI");
            Map(p => p.Kdeinvsp).ToColumn("KDEINVSP");
            Map(p => p.Kdeinven).ToColumn("KDEINVEN");
            Map(p => p.Kdewddeb).ToColumn("KDEWDDEB");
            Map(p => p.Kdewhdeb).ToColumn("KDEWHDEB");
            Map(p => p.Kdewdfin).ToColumn("KDEWDFIN");
            Map(p => p.Kdewhfin).ToColumn("KDEWHFIN");
            Map(p => p.Kdetcd).ToColumn("KDETCD");
            Map(p => p.Kdemodi).ToColumn("KDEMODI");
            Map(p => p.Kdepind).ToColumn("KDEPIND");
            Map(p => p.Kdepcatn).ToColumn("KDEPCATN");
            Map(p => p.Kdepref).ToColumn("KDEPREF");
            Map(p => p.Kdepprp).ToColumn("KDEPPRP");
            Map(p => p.Kdepemi).ToColumn("KDEPEMI");
            Map(p => p.Kdeptaxc).ToColumn("KDEPTAXC");
            Map(p => p.Kdepntm).ToColumn("KDEPNTM");
            Map(p => p.Kdeala).ToColumn("KDEALA");
            Map(p => p.Kdepala).ToColumn("KDEPALA");
            Map(p => p.Kdealo).ToColumn("KDEALO");
        }
    }
  

}
