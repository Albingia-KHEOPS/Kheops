using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {

    public  partial class  KpdocwRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KEQID, KEQTYP, KEQIPB, KEQALX, KEQSUA
, KEQNUM, KEQSBR, KEQSERV, KEQACTG, KEQACTN
, KEQECO, KEQAVN, KEQETAP, KEQKEMID, KEQORD
, KEQTGL, KEQTDOC, KEQKESID, KEQAJT, KEQTRS
, KEQMAIT, KEQLIEN, KEQCDOC, KEQVER, KEQLIB
, KEQNTA, KEQDACC, KEQTAE, KEQNOM, KEQCHM
, KEQSTU, KEQSIT, KEQSTD, KEQSTH, KEQENVU
, KEQENVD, KEQENVH, KEQTEDI, KEQORID, KEQTYDS
, KEQTYI, KEQIDS, KEQINL, KEQNBEX, KEQCRU
, KEQCRD, KEQCRH, KEQMAJU, KEQMAJD, KEQMAJH
, KEQSTG, KEQDIMP FROM KPDOCW
WHERE KEQID = :id
";
            const string update=@"UPDATE KPDOCW SET 
KEQID = :KEQID, KEQTYP = :KEQTYP, KEQIPB = :KEQIPB, KEQALX = :KEQALX, KEQSUA = :KEQSUA, KEQNUM = :KEQNUM, KEQSBR = :KEQSBR, KEQSERV = :KEQSERV, KEQACTG = :KEQACTG, KEQACTN = :KEQACTN
, KEQECO = :KEQECO, KEQAVN = :KEQAVN, KEQETAP = :KEQETAP, KEQKEMID = :KEQKEMID, KEQORD = :KEQORD, KEQTGL = :KEQTGL, KEQTDOC = :KEQTDOC, KEQKESID = :KEQKESID, KEQAJT = :KEQAJT, KEQTRS = :KEQTRS
, KEQMAIT = :KEQMAIT, KEQLIEN = :KEQLIEN, KEQCDOC = :KEQCDOC, KEQVER = :KEQVER, KEQLIB = :KEQLIB, KEQNTA = :KEQNTA, KEQDACC = :KEQDACC, KEQTAE = :KEQTAE, KEQNOM = :KEQNOM, KEQCHM = :KEQCHM
, KEQSTU = :KEQSTU, KEQSIT = :KEQSIT, KEQSTD = :KEQSTD, KEQSTH = :KEQSTH, KEQENVU = :KEQENVU, KEQENVD = :KEQENVD, KEQENVH = :KEQENVH, KEQTEDI = :KEQTEDI, KEQORID = :KEQORID, KEQTYDS = :KEQTYDS
, KEQTYI = :KEQTYI, KEQIDS = :KEQIDS, KEQINL = :KEQINL, KEQNBEX = :KEQNBEX, KEQCRU = :KEQCRU, KEQCRD = :KEQCRD, KEQCRH = :KEQCRH, KEQMAJU = :KEQMAJU, KEQMAJD = :KEQMAJD, KEQMAJH = :KEQMAJH
, KEQSTG = :KEQSTG, KEQDIMP = :KEQDIMP
 WHERE 
KEQID = :id";
            const string delete=@"DELETE FROM KPDOCW WHERE KEQID = :id";
            const string insert=@"INSERT INTO  KPDOCW (
KEQID, KEQTYP, KEQIPB, KEQALX, KEQSUA
, KEQNUM, KEQSBR, KEQSERV, KEQACTG, KEQACTN
, KEQECO, KEQAVN, KEQETAP, KEQKEMID, KEQORD
, KEQTGL, KEQTDOC, KEQKESID, KEQAJT, KEQTRS
, KEQMAIT, KEQLIEN, KEQCDOC, KEQVER, KEQLIB
, KEQNTA, KEQDACC, KEQTAE, KEQNOM, KEQCHM
, KEQSTU, KEQSIT, KEQSTD, KEQSTH, KEQENVU
, KEQENVD, KEQENVH, KEQTEDI, KEQORID, KEQTYDS
, KEQTYI, KEQIDS, KEQINL, KEQNBEX, KEQCRU
, KEQCRD, KEQCRH, KEQMAJU, KEQMAJD, KEQMAJH
, KEQSTG, KEQDIMP
) VALUES (
:KEQID, :KEQTYP, :KEQIPB, :KEQALX, :KEQSUA
, :KEQNUM, :KEQSBR, :KEQSERV, :KEQACTG, :KEQACTN
, :KEQECO, :KEQAVN, :KEQETAP, :KEQKEMID, :KEQORD
, :KEQTGL, :KEQTDOC, :KEQKESID, :KEQAJT, :KEQTRS
, :KEQMAIT, :KEQLIEN, :KEQCDOC, :KEQVER, :KEQLIB
, :KEQNTA, :KEQDACC, :KEQTAE, :KEQNOM, :KEQCHM
, :KEQSTU, :KEQSIT, :KEQSTD, :KEQSTH, :KEQENVU
, :KEQENVD, :KEQENVH, :KEQTEDI, :KEQORID, :KEQTYDS
, :KEQTYI, :KEQIDS, :KEQINL, :KEQNBEX, :KEQCRU
, :KEQCRD, :KEQCRH, :KEQMAJU, :KEQMAJD, :KEQMAJH
, :KEQSTG, :KEQDIMP)";
            const string select_GetByAffaire=@"SELECT
KEQID, KEQTYP, KEQIPB, KEQALX, KEQSUA
, KEQNUM, KEQSBR, KEQSERV, KEQACTG, KEQACTN
, KEQECO, KEQAVN, KEQETAP, KEQKEMID, KEQORD
, KEQTGL, KEQTDOC, KEQKESID, KEQAJT, KEQTRS
, KEQMAIT, KEQLIEN, KEQCDOC, KEQVER, KEQLIB
, KEQNTA, KEQDACC, KEQTAE, KEQNOM, KEQCHM
, KEQSTU, KEQSIT, KEQSTD, KEQSTH, KEQENVU
, KEQENVD, KEQENVH, KEQTEDI, KEQORID, KEQTYDS
, KEQTYI, KEQIDS, KEQINL, KEQNBEX, KEQCRU
, KEQCRD, KEQCRH, KEQMAJU, KEQMAJD, KEQMAJH
, KEQSTG, KEQDIMP FROM KPDOCW
WHERE KEQTYP = :typeAffaire
and KEQIPB = :numeroAffaire
and KEQALX = :numeroAliment
and KEQAVN = :numeroAvenant
";
            #endregion

            public KpdocwRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpDoc Get(Int64 id){
                return connection.Query<KpDoc>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KEQID") ;
            }

            public void Insert(KpDoc value){
                    if(value.Keqid == default(Int64)) {
                        value.Keqid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KEQID",value.Keqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQTYP",value.Keqtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQIPB",value.Keqipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEQALX",value.Keqalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEQSUA",value.Keqsua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEQNUM",value.Keqnum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEQSBR",value.Keqsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KEQSERV",value.Keqserv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQACTG",value.Keqactg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQACTN",value.Keqactn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEQECO",value.Keqeco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQAVN",value.Keqavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEQETAP",value.Keqetap??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQKEMID",value.Keqkemid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQORD",value.Keqord, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KEQTGL",value.Keqtgl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQTDOC",value.Keqtdoc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQKESID",value.Keqkesid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQAJT",value.Keqajt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQTRS",value.Keqtrs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQMAIT",value.Keqmait??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQLIEN",value.Keqlien, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQCDOC",value.Keqcdoc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KEQVER",value.Keqver, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEQLIB",value.Keqlib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KEQNTA",value.Keqnta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQDACC",value.Keqdacc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQTAE",value.Keqtae??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQNOM",value.Keqnom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:255, scale: 0);
                    parameters.Add("KEQCHM",value.Keqchm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:255, scale: 0);
                    parameters.Add("KEQSTU",value.Keqstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQSIT",value.Keqsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQSTD",value.Keqstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEQSTH",value.Keqsth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEQENVU",value.Keqenvu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQENVD",value.Keqenvd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEQENVH",value.Keqenvh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEQTEDI",value.Keqtedi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQORID",value.Keqorid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQTYDS",value.Keqtyds??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEQTYI",value.Keqtyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEQIDS",value.Keqids, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEQINL",value.Keqinl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEQNBEX",value.Keqnbex, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KEQCRU",value.Keqcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQCRD",value.Keqcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEQCRH",value.Keqcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEQMAJU",value.Keqmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQMAJD",value.Keqmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEQMAJH",value.Keqmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEQSTG",value.Keqstg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQDIMP",value.Keqdimp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpDoc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Keqid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpDoc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEQID",value.Keqid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQTYP",value.Keqtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQIPB",value.Keqipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEQALX",value.Keqalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEQSUA",value.Keqsua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEQNUM",value.Keqnum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEQSBR",value.Keqsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KEQSERV",value.Keqserv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQACTG",value.Keqactg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQACTN",value.Keqactn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEQECO",value.Keqeco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQAVN",value.Keqavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEQETAP",value.Keqetap??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQKEMID",value.Keqkemid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQORD",value.Keqord, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KEQTGL",value.Keqtgl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQTDOC",value.Keqtdoc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQKESID",value.Keqkesid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQAJT",value.Keqajt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQTRS",value.Keqtrs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQMAIT",value.Keqmait??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQLIEN",value.Keqlien, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQCDOC",value.Keqcdoc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KEQVER",value.Keqver, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEQLIB",value.Keqlib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KEQNTA",value.Keqnta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQDACC",value.Keqdacc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQTAE",value.Keqtae??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQNOM",value.Keqnom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:255, scale: 0);
                    parameters.Add("KEQCHM",value.Keqchm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:255, scale: 0);
                    parameters.Add("KEQSTU",value.Keqstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQSIT",value.Keqsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQSTD",value.Keqstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEQSTH",value.Keqsth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEQENVU",value.Keqenvu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQENVD",value.Keqenvd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEQENVH",value.Keqenvh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEQTEDI",value.Keqtedi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQORID",value.Keqorid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEQTYDS",value.Keqtyds??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEQTYI",value.Keqtyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEQIDS",value.Keqids, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEQINL",value.Keqinl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEQNBEX",value.Keqnbex, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KEQCRU",value.Keqcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQCRD",value.Keqcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEQCRH",value.Keqcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEQMAJU",value.Keqmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEQMAJD",value.Keqmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEQMAJH",value.Keqmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEQSTG",value.Keqstg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEQDIMP",value.Keqdimp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("id",value.Keqid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpDoc> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpDoc>(select_GetByAffaire, new {typeAffaire, numeroAffaire, numeroAliment, numeroAvenant}).ToList();
            }
    }
}
