using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpIoptMapper : EntityMap<KpIopt>   {
        public KpIoptMapper () {
            Map(p => p.Kfcid).ToColumn("KFCID");
            Map(p => p.Kfctyp).ToColumn("KFCTYP");
            Map(p => p.Kfcipb).ToColumn("KFCIPB");
            Map(p => p.Kfcalx).ToColumn("KFCALX");
            Map(p => p.Kfcavn).ToColumn("KFCAVN");
            Map(p => p.Kfchin).ToColumn("KFCHIN");
            Map(p => p.Kfcfor).ToColumn("KFCFOR");
            Map(p => p.Kfcopt).ToColumn("KFCOPT");
            Map(p => p.Kfckdbid).ToColumn("KFCKDBID");
            Map(p => p.Kfcrrcr).ToColumn("KFCRRCR");
            Map(p => p.Kfcrrc).ToColumn("KFCRRC");
            Map(p => p.Kfcmnte).ToColumn("KFCMNTE");
            Map(p => p.Kfcseui).ToColumn("KFCSEUI");
            Map(p => p.Kfcseur).ToColumn("KFCSEUR");
            Map(p => p.Kfcseuc).ToColumn("KFCSEUC");
            Map(p => p.Kfcperr).ToColumn("KFCPERR");
            Map(p => p.Kfcautm).ToColumn("KFCAUTM");
            Map(p => p.Kfccrd).ToColumn("KFCCRD");
            Map(p => p.Kfccrh).ToColumn("KFCCRH");
            Map(p => p.Kfcmaju).ToColumn("KFCMAJU");
            Map(p => p.Kfcmajd).ToColumn("KFCMAJD");
            Map(p => p.Kfcmajh).ToColumn("KFCMAJH");
        }
    }
  

}
