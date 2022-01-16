using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpCotgaMapper : EntityMap<KpCotga>   {
        public KpCotgaMapper () {
            Map(p => p.Kdnid).ToColumn("KDNID");
            Map(p => p.Kdntyp).ToColumn("KDNTYP");
            Map(p => p.Kdnipb).ToColumn("KDNIPB");
            Map(p => p.Kdnalx).ToColumn("KDNALX");
            Map(p => p.Kdnavn).ToColumn("KDNAVN");
            Map(p => p.Kdnhin).ToColumn("KDNHIN");
            Map(p => p.Kdnfor).ToColumn("KDNFOR");
            Map(p => p.Kdnfoa).ToColumn("KDNFOA");
            Map(p => p.Kdnopt).ToColumn("KDNOPT");
            Map(p => p.Kdngaran).ToColumn("KDNGARAN");
            Map(p => p.Kdnkdeid).ToColumn("KDNKDEID");
            Map(p => p.Kdnnumtar).ToColumn("KDNNUMTAR");
            Map(p => p.Kdnkdgid).ToColumn("KDNKDGID");
            Map(p => p.Kdntarok).ToColumn("KDNTAROK");
            Map(p => p.Kdnkdmid).ToColumn("KDNKDMID");
            Map(p => p.Kdnrsq).ToColumn("KDNRSQ");
            Map(p => p.Kdntri).ToColumn("KDNTRI");
            Map(p => p.Kdnht).ToColumn("KDNHT");
            Map(p => p.Kdnhf).ToColumn("KDNHF");
            Map(p => p.Kdnkh).ToColumn("KDNKH");
            Map(p => p.Kdnmht).ToColumn("KDNMHT");
            Map(p => p.Kdnkht).ToColumn("KDNKHT");
            Map(p => p.Kdnmtx).ToColumn("KDNMTX");
            Map(p => p.Kdnktx).ToColumn("KDNKTX");
            Map(p => p.Kdnttc).ToColumn("KDNTTC");
            Map(p => p.Kdnttf).ToColumn("KDNTTF");
            Map(p => p.Kdnmtt).ToColumn("KDNMTT");
            Map(p => p.Kdnktc).ToColumn("KDNKTC");
            Map(p => p.Kdncot).ToColumn("KDNCOT");
            Map(p => p.Kdnkco).ToColumn("KDNKCO");
            Map(p => p.Kdncnamht).ToColumn("KDNCNAMHT");
            Map(p => p.Kdncnakht).ToColumn("KDNCNAKHT");
            Map(p => p.Kdncnamtx).ToColumn("KDNCNAMTX");
            Map(p => p.Kdncnaktx).ToColumn("KDNCNAKTX");
            Map(p => p.Kdncnamtt).ToColumn("KDNCNAMTT");
            Map(p => p.Kdncnaktt).ToColumn("KDNCNAKTT");
            Map(p => p.Kdncnacom).ToColumn("KDNCNACOM");
            Map(p => p.Kdncnakcm).ToColumn("KDNCNAKCM");
            Map(p => p.Kdnatgmht).ToColumn("KDNATGMHT");
            Map(p => p.Kdnatgkht).ToColumn("KDNATGKHT");
            Map(p => p.Kdnatgmtx).ToColumn("KDNATGMTX");
            Map(p => p.Kdnatgktx).ToColumn("KDNATGKTX");
            Map(p => p.Kdnatgmtt).ToColumn("KDNATGMTT");
            Map(p => p.Kdnatgktt).ToColumn("KDNATGKTT");
        }
    }
  

}
