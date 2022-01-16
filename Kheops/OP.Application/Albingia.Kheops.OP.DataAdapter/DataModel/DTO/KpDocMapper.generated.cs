using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpDocMapper : EntityMap<KpDoc>   {
        public KpDocMapper () {
            Map(p => p.Keqid).ToColumn("KEQID");
            Map(p => p.Keqtyp).ToColumn("KEQTYP");
            Map(p => p.Keqipb).ToColumn("KEQIPB");
            Map(p => p.Keqalx).ToColumn("KEQALX");
            Map(p => p.Keqsua).ToColumn("KEQSUA");
            Map(p => p.Keqnum).ToColumn("KEQNUM");
            Map(p => p.Keqsbr).ToColumn("KEQSBR");
            Map(p => p.Keqserv).ToColumn("KEQSERV");
            Map(p => p.Keqactg).ToColumn("KEQACTG");
            Map(p => p.Keqactn).ToColumn("KEQACTN");
            Map(p => p.Keqeco).ToColumn("KEQECO");
            Map(p => p.Keqavn).ToColumn("KEQAVN");
            Map(p => p.Keqetap).ToColumn("KEQETAP");
            Map(p => p.Keqkemid).ToColumn("KEQKEMID");
            Map(p => p.Keqord).ToColumn("KEQORD");
            Map(p => p.Keqtgl).ToColumn("KEQTGL");
            Map(p => p.Keqtdoc).ToColumn("KEQTDOC");
            Map(p => p.Keqkesid).ToColumn("KEQKESID");
            Map(p => p.Keqajt).ToColumn("KEQAJT");
            Map(p => p.Keqtrs).ToColumn("KEQTRS");
            Map(p => p.Keqmait).ToColumn("KEQMAIT");
            Map(p => p.Keqlien).ToColumn("KEQLIEN");
            Map(p => p.Keqcdoc).ToColumn("KEQCDOC");
            Map(p => p.Keqver).ToColumn("KEQVER");
            Map(p => p.Keqlib).ToColumn("KEQLIB");
            Map(p => p.Keqnta).ToColumn("KEQNTA");
            Map(p => p.Keqdacc).ToColumn("KEQDACC");
            Map(p => p.Keqtae).ToColumn("KEQTAE");
            Map(p => p.Keqnom).ToColumn("KEQNOM");
            Map(p => p.Keqchm).ToColumn("KEQCHM");
            Map(p => p.Keqstu).ToColumn("KEQSTU");
            Map(p => p.Keqsit).ToColumn("KEQSIT");
            Map(p => p.Keqstd).ToColumn("KEQSTD");
            Map(p => p.Keqsth).ToColumn("KEQSTH");
            Map(p => p.Keqenvu).ToColumn("KEQENVU");
            Map(p => p.Keqenvd).ToColumn("KEQENVD");
            Map(p => p.Keqenvh).ToColumn("KEQENVH");
            Map(p => p.Keqtedi).ToColumn("KEQTEDI");
            Map(p => p.Keqorid).ToColumn("KEQORID");
            Map(p => p.Keqtyds).ToColumn("KEQTYDS");
            Map(p => p.Keqtyi).ToColumn("KEQTYI");
            Map(p => p.Keqids).ToColumn("KEQIDS");
            Map(p => p.Keqinl).ToColumn("KEQINL");
            Map(p => p.Keqnbex).ToColumn("KEQNBEX");
            Map(p => p.Keqcru).ToColumn("KEQCRU");
            Map(p => p.Keqcrd).ToColumn("KEQCRD");
            Map(p => p.Keqcrh).ToColumn("KEQCRH");
            Map(p => p.Keqmaju).ToColumn("KEQMAJU");
            Map(p => p.Keqmajd).ToColumn("KEQMAJD");
            Map(p => p.Keqmajh).ToColumn("KEQMAJH");
            Map(p => p.Keqstg).ToColumn("KEQSTG");
            Map(p => p.Keqdimp).ToColumn("KEQDIMP");
        }
    }
  

}
