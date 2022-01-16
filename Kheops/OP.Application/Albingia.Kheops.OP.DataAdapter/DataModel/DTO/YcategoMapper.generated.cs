using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class YcategoMapper : EntityMap<Ycatego>   {
        public YcategoMapper () {
            Map(p => p.Cabra).ToColumn("CABRA");
            Map(p => p.Casbr).ToColumn("CASBR");
            Map(p => p.Cacat).ToColumn("CACAT");
            Map(p => p.Cades).ToColumn("CADES");
            Map(p => p.Cadea).ToColumn("CADEA");
            Map(p => p.Carcg).ToColumn("CARCG");
            Map(p => p.Carcs).ToColumn("CARCS");
            Map(p => p.Caafr).ToColumn("CAAFR");
            Map(p => p.Catax).ToColumn("CATAX");
            Map(p => p.Cargc).ToColumn("CARGC");
            Map(p => p.Capmi).ToColumn("CAPMI");
            Map(p => p.Caatt).ToColumn("CAATT");
            Map(p => p.Cacnp).ToColumn("CACNP");
            Map(p => p.Cacnc).ToColumn("CACNC");
            Map(p => p.Casmp).ToColumn("CASMP");
            Map(p => p.Causr).ToColumn("CAUSR");
            Map(p => p.Camja).ToColumn("CAMJA");
            Map(p => p.Camjm).ToColumn("CAMJM");
            Map(p => p.Camjj).ToColumn("CAMJJ");
            Map(p => p.Cacnx).ToColumn("CACNX");
            Map(p => p.Caatx).ToColumn("CAATX");
            Map(p => p.Cavat).ToColumn("CAVAT");
            Map(p => p.Casai).ToColumn("CASAI");
            Map(p => p.Caina).ToColumn("CAINA");
            Map(p => p.Caind).ToColumn("CAIND");
            Map(p => p.Caixc).ToColumn("CAIXC");
            Map(p => p.Caixf).ToColumn("CAIXF");
            Map(p => p.Caixl).ToColumn("CAIXL");
            Map(p => p.Caixp).ToColumn("CAIXP");
            Map(p => p.Caipm).ToColumn("CAIPM");
            Map(p => p.Caaut).ToColumn("CAAUT");
            Map(p => p.Calib).ToColumn("CALIB");
            Map(p => p.Carst).ToColumn("CARST");
            Map(p => p.Caapr).ToColumn("CAAPR");
            Map(p => p.Cagri).ToColumn("CAGRI");
            Map(p => p.Caedi).ToColumn("CAEDI");
        }
    }
}
