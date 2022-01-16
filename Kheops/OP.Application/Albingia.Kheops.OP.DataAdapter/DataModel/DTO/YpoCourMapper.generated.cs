using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpoCourMapper : EntityMap<YpoCour>   {
        public YpoCourMapper () {
            Map(p => p.Pfipb).ToColumn("PFIPB");
            Map(p => p.Pfalx).ToColumn("PFALX");
            Map(p => p.Pfavn).ToColumn("PFAVN");
            Map(p => p.Pfhin).ToColumn("PFHIN");
            Map(p => p.Pfcti).ToColumn("PFCTI");
            Map(p => p.Pfict).ToColumn("PFICT");
            Map(p => p.Pfsaa).ToColumn("PFSAA");
            Map(p => p.Pfsam).ToColumn("PFSAM");
            Map(p => p.Pfsaj).ToColumn("PFSAJ");
            Map(p => p.Pfsah).ToColumn("PFSAH");
            Map(p => p.Pfsit).ToColumn("PFSIT");
            Map(p => p.Pfsta).ToColumn("PFSTA");
            Map(p => p.Pfstm).ToColumn("PFSTM");
            Map(p => p.Pfstj).ToColumn("PFSTJ");
            Map(p => p.Pfges).ToColumn("PFGES");
            Map(p => p.Pfsou).ToColumn("PFSOU");
            Map(p => p.Pfcom).ToColumn("PFCOM");
            Map(p => p.Pfoct).ToColumn("PFOCT");
            Map(p => p.Pfxcm).ToColumn("PFXCM");
            Map(p => p.Pfxcn).ToColumn("PFXCN");
            Map(p => p.Pftyp).ToColumn("PFTYP");
        }
    }
  

}
