using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YpltGarMapper : EntityMap<YpltGar>   {
        public YpltGarMapper () {
            Map(p => p.C2seq).ToColumn("C2SEQ");
            Map(p => p.C2mga).ToColumn("C2MGA");
            Map(p => p.C2obe).ToColumn("C2OBE");
            Map(p => p.C2niv).ToColumn("C2NIV");
            Map(p => p.C2gar).ToColumn("C2GAR");
            Map(p => p.C2ord).ToColumn("C2ORD");
            Map(p => p.C2lib).ToColumn("C2LIB");
            Map(p => p.C2sem).ToColumn("C2SEM");
            Map(p => p.C2car).ToColumn("C2CAR");
            Map(p => p.C2nat).ToColumn("C2NAT");
            Map(p => p.C2ina).ToColumn("C2INA");
            Map(p => p.C2cna).ToColumn("C2CNA");
            Map(p => p.C2tax).ToColumn("C2TAX");
            Map(p => p.C2alt).ToColumn("C2ALT");
            Map(p => p.C2tri).ToColumn("C2TRI");
            Map(p => p.C2se1).ToColumn("C2SE1");
            Map(p => p.C2scr).ToColumn("C2SCR");
            Map(p => p.C2prp).ToColumn("C2PRP");
            Map(p => p.C2tcd).ToColumn("C2TCD");
            Map(p => p.C2mrf).ToColumn("C2MRF");
            Map(p => p.C2ntm).ToColumn("C2NTM");
            Map(p => p.C2mas).ToColumn("C2MAS");
        }
    }
  

}
