using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public  class DocLotMapper : EntityMap<DocLot> {
        public DocLotMapper () {
            Map(p => p.Kemid).ToColumn("Kemid");
            Map(p => p.Kemkelid).ToColumn("Kemkelid");
            Map(p => p.Kemord).ToColumn("Kemord");
            Map(p => p.Kemtypd).ToColumn("Kemtypd");
            Map(p => p.Kemtypl).ToColumn("Kemtypl");
            Map(p => p.Kemtyenv).ToColumn("Kemtyenv");
            Map(p => p.Kemtamp).ToColumn("Kemtamp");
            Map(p => p.Kemtyds).ToColumn("Kemtyds");
            Map(p => p.Kemtyi).ToColumn("Kemtyi");
            Map(p => p.Kemids).ToColumn("Kemids");
            Map(p => p.Kemdstp).ToColumn("Kemdstp");
            Map(p => p.Keminl).ToColumn("Keminl");
            Map(p => p.Kemnbex).ToColumn("Kemnbex");
            Map(p => p.Kemdoca).ToColumn("Kemdoca");
            Map(p => p.Kemtydif).ToColumn("Kemtydif");
            Map(p => p.Kemlmai).ToColumn("Kemlmai");
            Map(p => p.Kemaemo).ToColumn("Kemaemo");
            Map(p => p.Kemaem).ToColumn("Kemaem");
            Map(p => p.Kemkesid).ToColumn("Kemkesid");
            Map(p => p.Kemnta).ToColumn("Kemnta");
            Map(p => p.Kemstu).ToColumn("Kemstu");
            Map(p => p.Kemsit).ToColumn("Kemsit");
            Map(p => p.Kemstd).ToColumn("Kemstd");
            Map(p => p.Kemsth).ToColumn("Kemsth");
            Map(p => p.Kemenvu).ToColumn("Kemenvu");
            Map(p => p.Kemenvd).ToColumn("Kemenvd");
            Map(p => p.Kemenvh).ToColumn("Kemenvh");
            Map(p => p.Keltyp).ToColumn("Keltyp");
            Map(p => p.Kelipb).ToColumn("Kelipb");
            Map(p => p.Kelalx).ToColumn("Kelalx");
            Map(p => p.Kelactg).ToColumn("Kelactg");
            Map(p => p.Kelactn).ToColumn("Kelactn");
            Map(p => p.Kelavn).ToColumn("Kelavn");
            Map(p => p.Kellib).ToColumn("Kellib");
            Map(p => p.Kelemi).ToColumn("Kelemi");
            Map(p => p.Kelipk).ToColumn("Kelipk");
            Map(p => p.Keqid).ToColumn("Keqid");
            Map(p => p.Keqtyp).ToColumn("Keqtyp");
            Map(p => p.Keqipb).ToColumn("Keqipb");
            Map(p => p.Keqalx).ToColumn("Keqalx");
            Map(p => p.Keqsua).ToColumn("Keqsua");
            Map(p => p.Keqnum).ToColumn("Keqnum");
            Map(p => p.Keqsbr).ToColumn("Keqsbr");
            Map(p => p.Keqserv).ToColumn("Keqserv");
            Map(p => p.Keqactg).ToColumn("Keqactg");
            Map(p => p.Keqactn).ToColumn("Keqactn");
            Map(p => p.Keqeco).ToColumn("Keqeco");
            Map(p => p.Keqavn).ToColumn("Keqavn");
            Map(p => p.Keqetap).ToColumn("Keqetap");
            Map(p => p.Keqkemid).ToColumn("Keqkemid");
            Map(p => p.Keqord).ToColumn("Keqord");
            Map(p => p.Keqtgl).ToColumn("Keqtgl");
            Map(p => p.Keqtdoc).ToColumn("Keqtdoc");
            Map(p => p.Keqkesid).ToColumn("Keqkesid");
            Map(p => p.Keqajt).ToColumn("Keqajt");
            Map(p => p.Keqtrs).ToColumn("Keqtrs");
            Map(p => p.Keqmait).ToColumn("Keqmait");
            Map(p => p.Keqlien).ToColumn("Keqlien");
            Map(p => p.Keqcdoc).ToColumn("Keqcdoc");
            Map(p => p.Keqver).ToColumn("Keqver");
            Map(p => p.Keqlib).ToColumn("Keqlib");
            Map(p => p.Keqnta).ToColumn("Keqnta");
            Map(p => p.Keqdacc).ToColumn("Keqdacc");
            Map(p => p.Keqtae).ToColumn("Keqtae");
            Map(p => p.Keqnom).ToColumn("Keqnom");
            Map(p => p.Keqchm).ToColumn("Keqchm");
            Map(p => p.Keqstu).ToColumn("Keqstu");
            Map(p => p.Keqsit).ToColumn("Keqsit");
            Map(p => p.Keqstd).ToColumn("Keqstd");
            Map(p => p.Keqsth).ToColumn("Keqsth");
            Map(p => p.Keqenvu).ToColumn("Keqenvu");
            Map(p => p.Keqenvd).ToColumn("Keqenvd");
            Map(p => p.Keqenvh).ToColumn("Keqenvh");
            Map(p => p.Keqtedi).ToColumn("Keqtedi");
            Map(p => p.Keqorid).ToColumn("Keqorid");
            Map(p => p.Keqtyds).ToColumn("Keqtyds");
            Map(p => p.Keqtyi).ToColumn("Keqtyi");
            Map(p => p.Keqids).ToColumn("Keqids");
            Map(p => p.Keqinl).ToColumn("Keqinl");
            Map(p => p.Keqnbex).ToColumn("Keqnbex");
            Map(p => p.Keqcru).ToColumn("Keqcru");
            Map(p => p.Keqcrd).ToColumn("Keqcrd");
            Map(p => p.Keqcrh).ToColumn("Keqcrh");
            Map(p => p.Keqmaju).ToColumn("Keqmaju");
            Map(p => p.Keqmajd).ToColumn("Keqmajd");
            Map(p => p.Keqmajh).ToColumn("Keqmajh");
            Map(p => p.Keqstg).ToColumn("Keqstg");
            Map(p => p.Keqdimp).ToColumn("Keqdimp");
        }
    }
  

}
