using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public  class Kpgaran_GarContratMapper : EntityMap<Kpgaran_GarContrat> {
        public Kpgaran_GarContratMapper () {
            Map(p => p.Kdeid).ToColumn("Kdeid");
            Map(p => p.Kdetyp).ToColumn("Kdetyp");
            Map(p => p.Kdeipb).ToColumn("Kdeipb");
            Map(p => p.Kdealx).ToColumn("Kdealx");
            Map(p => p.Kdefor).ToColumn("Kdefor");
            Map(p => p.Kdeopt).ToColumn("Kdeopt");
            Map(p => p.Kdekdcid).ToColumn("Kdekdcid");
            Map(p => p.Kdegaran).ToColumn("Kdegaran");
            Map(p => p.Kdeseq).ToColumn("Kdeseq");
            Map(p => p.Kdeniveau).ToColumn("Kdeniveau");
            Map(p => p.Kdesem).ToColumn("Kdesem");
            Map(p => p.Kdese1).ToColumn("Kdese1");
            Map(p => p.Kdetri).ToColumn("Kdetri");
            Map(p => p.Kdenumpres).ToColumn("Kdenumpres");
            Map(p => p.Kdeajout).ToColumn("Kdeajout");
            Map(p => p.Kdecar).ToColumn("Kdecar");
            Map(p => p.Kdenat).ToColumn("Kdenat");
            Map(p => p.Kdegan).ToColumn("Kdegan");
            Map(p => p.Kdekdfid).ToColumn("Kdekdfid");
            Map(p => p.Kdedefg).ToColumn("Kdedefg");
            Map(p => p.Kdekdhid).ToColumn("Kdekdhid");
            Map(p => p.Kdedatdeb).ToColumn("Kdedatdeb");
            Map(p => p.Kdeheudeb).ToColumn("Kdeheudeb");
            Map(p => p.Kdedatfin).ToColumn("Kdedatfin");
            Map(p => p.Kdeheufin).ToColumn("Kdeheufin");
            Map(p => p.Kdeduree).ToColumn("Kdeduree");
            Map(p => p.Kdeduruni).ToColumn("Kdeduruni");
            Map(p => p.Kdeprp).ToColumn("Kdeprp");
            Map(p => p.Kdetypemi).ToColumn("Kdetypemi");
            Map(p => p.Kdealiref).ToColumn("Kdealiref");
            Map(p => p.Kdecatnat).ToColumn("Kdecatnat");
            Map(p => p.Kdeina).ToColumn("Kdeina");
            Map(p => p.Kdetaxcod).ToColumn("Kdetaxcod");
            Map(p => p.Kdetaxrep).ToColumn("Kdetaxrep");
            Map(p => p.Kdecravn).ToColumn("Kdecravn");
            Map(p => p.Kdecru).ToColumn("Kdecru");
            Map(p => p.Kdecrd).ToColumn("Kdecrd");
            Map(p => p.Kdemajavn).ToColumn("Kdemajavn");
            Map(p => p.Kdeasvalo).ToColumn("Kdeasvalo");
            Map(p => p.Kdeasvala).ToColumn("Kdeasvala");
            Map(p => p.Kdeasvalw).ToColumn("Kdeasvalw");
            Map(p => p.Kdeasunit).ToColumn("Kdeasunit");
            Map(p => p.Kdeasbase).ToColumn("Kdeasbase");
            Map(p => p.Kdeasmod).ToColumn("Kdeasmod");
            Map(p => p.Kdeasobli).ToColumn("Kdeasobli");
            Map(p => p.Kdeinvsp).ToColumn("Kdeinvsp");
            Map(p => p.Kdeinven).ToColumn("Kdeinven");
            Map(p => p.Kdewddeb).ToColumn("Kdewddeb");
            Map(p => p.Kdewhdeb).ToColumn("Kdewhdeb");
            Map(p => p.Kdewdfin).ToColumn("Kdewdfin");
            Map(p => p.Kdewhfin).ToColumn("Kdewhfin");
            Map(p => p.Kdetcd).ToColumn("Kdetcd");
            Map(p => p.Kdemodi).ToColumn("Kdemodi");
            Map(p => p.Kdepind).ToColumn("Kdepind");
            Map(p => p.Kdepcatn).ToColumn("Kdepcatn");
            Map(p => p.Kdepref).ToColumn("Kdepref");
            Map(p => p.Kdepprp).ToColumn("Kdepprp");
            Map(p => p.Kdepemi).ToColumn("Kdepemi");
            Map(p => p.Kdeptaxc).ToColumn("Kdeptaxc");
            Map(p => p.Kdepntm).ToColumn("Kdepntm");
            Map(p => p.Kdeala).ToColumn("Kdeala");
            Map(p => p.Kdepala).ToColumn("Kdepala");
            Map(p => p.Kdealo).ToColumn("Kdealo");
            Map(p => p.Kdgid).ToColumn("Kdgid");
            Map(p => p.Kdgtyp).ToColumn("Kdgtyp");
            Map(p => p.Kdgipb).ToColumn("Kdgipb");
            Map(p => p.Kdgalx).ToColumn("Kdgalx");
            Map(p => p.Kdgfor).ToColumn("Kdgfor");
            Map(p => p.Kdgopt).ToColumn("Kdgopt");
            Map(p => p.Kdggaran).ToColumn("Kdggaran");
            Map(p => p.Kdgkdeid).ToColumn("Kdgkdeid");
            Map(p => p.Kdgnumtar).ToColumn("Kdgnumtar");
            Map(p => p.Kdglcimod).ToColumn("Kdglcimod");
            Map(p => p.Kdglciobl).ToColumn("Kdglciobl");
            Map(p => p.Kdglcivalo).ToColumn("Kdglcivalo");
            Map(p => p.Kdglcivala).ToColumn("Kdglcivala");
            Map(p => p.Kdglcivalw).ToColumn("Kdglcivalw");
            Map(p => p.Kdglciunit).ToColumn("Kdglciunit");
            Map(p => p.Kdglcibase).ToColumn("Kdglcibase");
            Map(p => p.Kdgkdiid).ToColumn("Kdgkdiid");
            Map(p => p.Kdgfrhmod).ToColumn("Kdgfrhmod");
            Map(p => p.Kdgfrhobl).ToColumn("Kdgfrhobl");
            Map(p => p.Kdgfrhvalo).ToColumn("Kdgfrhvalo");
            Map(p => p.Kdgfrhvala).ToColumn("Kdgfrhvala");
            Map(p => p.Kdgfrhvalw).ToColumn("Kdgfrhvalw");
            Map(p => p.Kdgfrhunit).ToColumn("Kdgfrhunit");
            Map(p => p.Kdgfrhbase).ToColumn("Kdgfrhbase");
            Map(p => p.Kdgkdkid).ToColumn("Kdgkdkid");
            Map(p => p.Kdgfmivalo).ToColumn("Kdgfmivalo");
            Map(p => p.Kdgfmivala).ToColumn("Kdgfmivala");
            Map(p => p.Kdgfmivalw).ToColumn("Kdgfmivalw");
            Map(p => p.Kdgfmiunit).ToColumn("Kdgfmiunit");
            Map(p => p.Kdgfmibase).ToColumn("Kdgfmibase");
            Map(p => p.Kdgfmavalo).ToColumn("Kdgfmavalo");
            Map(p => p.Kdgfmavala).ToColumn("Kdgfmavala");
            Map(p => p.Kdgfmavalw).ToColumn("Kdgfmavalw");
            Map(p => p.Kdgfmaunit).ToColumn("Kdgfmaunit");
            Map(p => p.Kdgfmabase).ToColumn("Kdgfmabase");
            Map(p => p.Kdgprimod).ToColumn("Kdgprimod");
            Map(p => p.Kdgpriobl).ToColumn("Kdgpriobl");
            Map(p => p.Kdgprivalo).ToColumn("Kdgprivalo");
            Map(p => p.Kdgprivala).ToColumn("Kdgprivala");
            Map(p => p.Kdgprivalw).ToColumn("Kdgprivalw");
            Map(p => p.Kdgpriunit).ToColumn("Kdgpriunit");
            Map(p => p.Kdgpribase).ToColumn("Kdgpribase");
            Map(p => p.Kdgmntbase).ToColumn("Kdgmntbase");
            Map(p => p.Kdgprimpro).ToColumn("Kdgprimpro");
            Map(p => p.Kdgtmc).ToColumn("Kdgtmc");
            Map(p => p.Kdgtff).ToColumn("Kdgtff");
            Map(p => p.Kdgcmc).ToColumn("Kdgcmc");
            Map(p => p.Kdgcht).ToColumn("Kdgcht");
            Map(p => p.Kdgctt).ToColumn("Kdgctt");
            Map(p => p.Kdckdbid).ToColumn("Kdckdbid");
            Map(p => p.Kdcteng).ToColumn("Kdcteng");
            Map(p => p.Kdckakid).ToColumn("Kdckakid");
            Map(p => p.Kdckaeid).ToColumn("Kdckaeid");
            Map(p => p.Kdckaqid).ToColumn("Kdckaqid");
            Map(p => p.Kdcmodele).ToColumn("Kdcmodele");
            Map(p => p.Kdckarid).ToColumn("Kdckarid");
            Map(p => p.Kdcflag).ToColumn("Kdcflag");
        }
    }
  

}