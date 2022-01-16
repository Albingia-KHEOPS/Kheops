using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpOptDMapper : EntityMap<KpOptD>   {
        public KpOptDMapper () {
            Map(p => p.Kdcid).ToColumn("KDCID");
            Map(p => p.Kdctyp).ToColumn("KDCTYP");
            Map(p => p.Kdcipb).ToColumn("KDCIPB");
            Map(p => p.Kdcalx).ToColumn("KDCALX");
            Map(p => p.Kdcavn).ToColumn("KDCAVN");
            Map(p => p.Kdchin).ToColumn("KDCHIN");
            Map(p => p.Kdcfor).ToColumn("KDCFOR");
            Map(p => p.Kdcopt).ToColumn("KDCOPT");
            Map(p => p.Kdckdbid).ToColumn("KDCKDBID");
            Map(p => p.Kdcteng).ToColumn("KDCTENG");
            Map(p => p.Kdckakid).ToColumn("KDCKAKID");
            Map(p => p.Kdckaeid).ToColumn("KDCKAEID");
            Map(p => p.Kdckaqid).ToColumn("KDCKAQID");
            Map(p => p.Kdcmodele).ToColumn("KDCMODELE");
            Map(p => p.Kdckarid).ToColumn("KDCKARID");
            Map(p => p.Kdccru).ToColumn("KDCCRU");
            Map(p => p.Kdccrd).ToColumn("KDCCRD");
            Map(p => p.Kdcmaju).ToColumn("KDCMAJU");
            Map(p => p.Kdcmajd).ToColumn("KDCMAJD");
            Map(p => p.Kdcflag).ToColumn("KDCFLAG");
            Map(p => p.Kdcordre).ToColumn("KDCORDRE");
        }
    }
  

}
