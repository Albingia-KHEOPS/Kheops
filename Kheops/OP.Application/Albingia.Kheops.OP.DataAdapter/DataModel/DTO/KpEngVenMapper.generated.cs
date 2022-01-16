using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpEngVenMapper : EntityMap<KpEngVen>   {
        public KpEngVenMapper () {
            Map(p => p.Kdqid).ToColumn("KDQID");
            Map(p => p.Kdqtyp).ToColumn("KDQTYP");
            Map(p => p.Kdqipb).ToColumn("KDQIPB");
            Map(p => p.Kdqalx).ToColumn("KDQALX");
            Map(p => p.Kdqavn).ToColumn("KDQAVN");
            Map(p => p.Kdqhin).ToColumn("KDQHIN");
            Map(p => p.Kdqkdpid).ToColumn("KDQKDPID");
            Map(p => p.Kdqfam).ToColumn("KDQFAM");
            Map(p => p.Kdqven).ToColumn("KDQVEN");
            Map(p => p.Kdqengc).ToColumn("KDQENGC");
            Map(p => p.Kdqengf).ToColumn("KDQENGF");
            Map(p => p.Kdqengok).ToColumn("KDQENGOK");
            Map(p => p.Kdqcru).ToColumn("KDQCRU");
            Map(p => p.Kdqcrd).ToColumn("KDQCRD");
            Map(p => p.Kdqmaju).ToColumn("KDQMAJU");
            Map(p => p.Kdqmajd).ToColumn("KDQMAJD");
            Map(p => p.Kdqlct).ToColumn("KDQLCT");
            Map(p => p.Kdqcat).ToColumn("KDQCAT");
            Map(p => p.Kdqkdoid).ToColumn("KDQKDOID");
        }
    }
  

}
