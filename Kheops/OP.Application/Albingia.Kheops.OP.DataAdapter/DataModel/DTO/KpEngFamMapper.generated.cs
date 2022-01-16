using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpEngFamMapper : EntityMap<KpEngFam>   {
        public KpEngFamMapper () {
            Map(p => p.Kdpid).ToColumn("KDPID");
            Map(p => p.Kdptyp).ToColumn("KDPTYP");
            Map(p => p.Kdpipb).ToColumn("KDPIPB");
            Map(p => p.Kdpalx).ToColumn("KDPALX");
            Map(p => p.Kdpavn).ToColumn("KDPAVN");
            Map(p => p.Kdphin).ToColumn("KDPHIN");
            Map(p => p.Kdpkdoid).ToColumn("KDPKDOID");
            Map(p => p.Kdpfam).ToColumn("KDPFAM");
            Map(p => p.Kdpeng).ToColumn("KDPENG");
            Map(p => p.Kdpena).ToColumn("KDPENA");
            Map(p => p.Kdpcru).ToColumn("KDPCRU");
            Map(p => p.Kdpcrd).ToColumn("KDPCRD");
            Map(p => p.Kdpmaju).ToColumn("KDPMAJU");
            Map(p => p.Kdpmajd).ToColumn("KDPMAJD");
            Map(p => p.Kdplct).ToColumn("KDPLCT");
            Map(p => p.Kdplca).ToColumn("KDPLCA");
            Map(p => p.Kdpcat).ToColumn("KDPCAT");
            Map(p => p.Kdpcaa).ToColumn("KDPCAA");
        }
    }
  

}
