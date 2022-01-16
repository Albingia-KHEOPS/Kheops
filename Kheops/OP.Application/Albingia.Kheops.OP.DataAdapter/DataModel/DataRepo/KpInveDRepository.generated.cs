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

    public  partial class  KpInveDRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KBFID, KBFTYP, KBFIPB, KBFALX, KBFKBEID
, KBFNUMLGN, KBFDESC, KBFKADID, KBFSITE, KBFNTLI
, KBFCP, KBFVILLE, KBFADH, KBFDATDEB, KBFDEBHEU
, KBFDATFIN, KBFFINHEU, KBFMNT1, KBFMNT2, KBFNBEVN
, KBFNBPER, KBFNOM, KBFPNOM, KBFDATNAI, KBFFONC
, KBFCDEC, KBFCIP, KBFACCS, KBFAVPR, KBFMSR
, KBFCMAT, KBFSEX, KBFMDQ, KBFMDA, KBFACTP
, KBFKADFH, KBFEXT, KBFMNT3, KBFMNT4, KBFQUA
, KBFREN, KBFRLO, KBFPAY, KBFSIT2, KBFADH2
, KBFSIT3, KBFADH3, KBFDES2, KBFDES3, KBFDES4
, KBFMRQ, KBFMOD, KBFMIM FROM KPINVED
WHERE KBFID = :parKBFID
";
            const string update=@"UPDATE KPINVED SET 
KBFID = :KBFID, KBFTYP = :KBFTYP, KBFIPB = :KBFIPB, KBFALX = :KBFALX, KBFKBEID = :KBFKBEID, KBFNUMLGN = :KBFNUMLGN, KBFDESC = :KBFDESC, KBFKADID = :KBFKADID, KBFSITE = :KBFSITE, KBFNTLI = :KBFNTLI
, KBFCP = :KBFCP, KBFVILLE = :KBFVILLE, KBFADH = :KBFADH, KBFDATDEB = :KBFDATDEB, KBFDEBHEU = :KBFDEBHEU, KBFDATFIN = :KBFDATFIN, KBFFINHEU = :KBFFINHEU, KBFMNT1 = :KBFMNT1, KBFMNT2 = :KBFMNT2, KBFNBEVN = :KBFNBEVN
, KBFNBPER = :KBFNBPER, KBFNOM = :KBFNOM, KBFPNOM = :KBFPNOM, KBFDATNAI = :KBFDATNAI, KBFFONC = :KBFFONC, KBFCDEC = :KBFCDEC, KBFCIP = :KBFCIP, KBFACCS = :KBFACCS, KBFAVPR = :KBFAVPR, KBFMSR = :KBFMSR
, KBFCMAT = :KBFCMAT, KBFSEX = :KBFSEX, KBFMDQ = :KBFMDQ, KBFMDA = :KBFMDA, KBFACTP = :KBFACTP, KBFKADFH = :KBFKADFH, KBFEXT = :KBFEXT, KBFMNT3 = :KBFMNT3, KBFMNT4 = :KBFMNT4, KBFQUA = :KBFQUA
, KBFREN = :KBFREN, KBFRLO = :KBFRLO, KBFPAY = :KBFPAY, KBFSIT2 = :KBFSIT2, KBFADH2 = :KBFADH2, KBFSIT3 = :KBFSIT3, KBFADH3 = :KBFADH3, KBFDES2 = :KBFDES2, KBFDES3 = :KBFDES3, KBFDES4 = :KBFDES4
, KBFMRQ = :KBFMRQ, KBFMOD = :KBFMOD, KBFMIM = :KBFMIM
 WHERE 
KBFID = :parKBFID";
            const string delete=@"DELETE FROM KPINVED WHERE KBFID = :parKBFID";
            const string insert=@"INSERT INTO  KPINVED (
KBFID, KBFTYP, KBFIPB, KBFALX, KBFKBEID
, KBFNUMLGN, KBFDESC, KBFKADID, KBFSITE, KBFNTLI
, KBFCP, KBFVILLE, KBFADH, KBFDATDEB, KBFDEBHEU
, KBFDATFIN, KBFFINHEU, KBFMNT1, KBFMNT2, KBFNBEVN
, KBFNBPER, KBFNOM, KBFPNOM, KBFDATNAI, KBFFONC
, KBFCDEC, KBFCIP, KBFACCS, KBFAVPR, KBFMSR
, KBFCMAT, KBFSEX, KBFMDQ, KBFMDA, KBFACTP
, KBFKADFH, KBFEXT, KBFMNT3, KBFMNT4, KBFQUA
, KBFREN, KBFRLO, KBFPAY, KBFSIT2, KBFADH2
, KBFSIT3, KBFADH3, KBFDES2, KBFDES3, KBFDES4
, KBFMRQ, KBFMOD, KBFMIM
) VALUES (
:KBFID, :KBFTYP, :KBFIPB, :KBFALX, :KBFKBEID
, :KBFNUMLGN, :KBFDESC, :KBFKADID, :KBFSITE, :KBFNTLI
, :KBFCP, :KBFVILLE, :KBFADH, :KBFDATDEB, :KBFDEBHEU
, :KBFDATFIN, :KBFFINHEU, :KBFMNT1, :KBFMNT2, :KBFNBEVN
, :KBFNBPER, :KBFNOM, :KBFPNOM, :KBFDATNAI, :KBFFONC
, :KBFCDEC, :KBFCIP, :KBFACCS, :KBFAVPR, :KBFMSR
, :KBFCMAT, :KBFSEX, :KBFMDQ, :KBFMDA, :KBFACTP
, :KBFKADFH, :KBFEXT, :KBFMNT3, :KBFMNT4, :KBFQUA
, :KBFREN, :KBFRLO, :KBFPAY, :KBFSIT2, :KBFADH2
, :KBFSIT3, :KBFADH3, :KBFDES2, :KBFDES3, :KBFDES4
, :KBFMRQ, :KBFMOD, :KBFMIM)";
            const string select_GetByAffaire=@"SELECT
KBFID, KBFTYP, KBFIPB, KBFALX, KBFKBEID
, KBFNUMLGN, KBFDESC, KBFKADID, KBFSITE, KBFNTLI
, KBFCP, KBFVILLE, KBFADH, KBFDATDEB, KBFDEBHEU
, KBFDATFIN, KBFFINHEU, KBFMNT1, KBFMNT2, KBFNBEVN
, KBFNBPER, KBFNOM, KBFPNOM, KBFDATNAI, KBFFONC
, KBFCDEC, KBFCIP, KBFACCS, KBFAVPR, KBFMSR
, KBFCMAT, KBFSEX, KBFMDQ, KBFMDA, KBFACTP
, KBFKADFH, KBFEXT, KBFMNT3, KBFMNT4, KBFQUA
, KBFREN, KBFRLO, KBFPAY, KBFSIT2, KBFADH2
, KBFSIT3, KBFADH3, KBFDES2, KBFDES3, KBFDES4
, KBFMRQ, KBFMOD, KBFMIM FROM KPINVED
WHERE KBFTYP = :typeAffaire
and KBFIPB = :codeAffaire
and KBFALX = :numeroAliement
";
            const string select_GetByInven=@"SELECT
KBFID, KBFTYP, KBFIPB, KBFALX, KBFKBEID
, KBFNUMLGN, KBFDESC, KBFKADID, KBFSITE, KBFNTLI
, KBFCP, KBFVILLE, KBFADH, KBFDATDEB, KBFDEBHEU
, KBFDATFIN, KBFFINHEU, KBFMNT1, KBFMNT2, KBFNBEVN
, KBFNBPER, KBFNOM, KBFPNOM, KBFDATNAI, KBFFONC
, KBFCDEC, KBFCIP, KBFACCS, KBFAVPR, KBFMSR
, KBFCMAT, KBFSEX, KBFMDQ, KBFMDA, KBFACTP
, KBFKADFH, KBFEXT, KBFMNT3, KBFMNT4, KBFQUA
, KBFREN, KBFRLO, KBFPAY, KBFSIT2, KBFADH2
, KBFSIT3, KBFADH3, KBFDES2, KBFDES3, KBFDES4
, KBFMRQ, KBFMOD, KBFMIM FROM KPINVED
WHERE KBFKBEID = :invenId
";
            #endregion

            public KpInveDRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpInveD Get(Int64 parKBFID){
                return connection.Query<KpInveD>(select, new {parKBFID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KBFID") ;
            }

            public void Insert(KpInveD value){
                    if(value.Kbfid == default(Int64)) {
                        value.Kbfid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KBFID",value.Kbfid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFTYP",value.Kbftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFIPB",value.Kbfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KBFALX",value.Kbfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KBFKBEID",value.Kbfkbeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFNUMLGN",value.Kbfnumlgn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFDESC",value.Kbfdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFKADID",value.Kbfkadid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFSITE",value.Kbfsite??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFNTLI",value.Kbfntli??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFCP",value.Kbfcp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFVILLE",value.Kbfville??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KBFADH",value.Kbfadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KBFDATDEB",value.Kbfdatdeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KBFDEBHEU",value.Kbfdebheu, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KBFDATFIN",value.Kbfdatfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KBFFINHEU",value.Kbffinheu, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KBFMNT1",value.Kbfmnt1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBFMNT2",value.Kbfmnt2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBFNBEVN",value.Kbfnbevn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFNBPER",value.Kbfnbper, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFNOM",value.Kbfnom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFPNOM",value.Kbfpnom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KBFDATNAI",value.Kbfdatnai, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KBFFONC",value.Kbffonc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KBFCDEC",value.Kbfcdec, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KBFCIP",value.Kbfcip, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KBFACCS",value.Kbfaccs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFAVPR",value.Kbfavpr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFMSR",value.Kbfmsr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFCMAT",value.Kbfcmat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFSEX",value.Kbfsex??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFMDQ",value.Kbfmdq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFMDA",value.Kbfmda, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFACTP",value.Kbfactp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFKADFH",value.Kbfkadfh, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFEXT",value.Kbfext??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFMNT3",value.Kbfmnt3, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBFMNT4",value.Kbfmnt4, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBFQUA",value.Kbfqua??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFREN",value.Kbfren??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFRLO",value.Kbfrlo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFPAY",value.Kbfpay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KBFSIT2",value.Kbfsit2, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFADH2",value.Kbfadh2, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KBFSIT3",value.Kbfsit3, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFADH3",value.Kbfadh3, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KBFDES2",value.Kbfdes2, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFDES3",value.Kbfdes3, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFDES4",value.Kbfdes4, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFMRQ",value.Kbfmrq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFMOD",value.Kbfmod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFMIM",value.Kbfmim??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpInveD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKBFID",value.Kbfid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpInveD value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KBFID",value.Kbfid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFTYP",value.Kbftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFIPB",value.Kbfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KBFALX",value.Kbfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KBFKBEID",value.Kbfkbeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFNUMLGN",value.Kbfnumlgn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFDESC",value.Kbfdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFKADID",value.Kbfkadid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFSITE",value.Kbfsite??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFNTLI",value.Kbfntli??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFCP",value.Kbfcp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFVILLE",value.Kbfville??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KBFADH",value.Kbfadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KBFDATDEB",value.Kbfdatdeb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KBFDEBHEU",value.Kbfdebheu, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KBFDATFIN",value.Kbfdatfin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KBFFINHEU",value.Kbffinheu, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KBFMNT1",value.Kbfmnt1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBFMNT2",value.Kbfmnt2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBFNBEVN",value.Kbfnbevn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFNBPER",value.Kbfnbper, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFNOM",value.Kbfnom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFPNOM",value.Kbfpnom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KBFDATNAI",value.Kbfdatnai, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KBFFONC",value.Kbffonc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("KBFCDEC",value.Kbfcdec, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KBFCIP",value.Kbfcip, dbType:DbType.Int64, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("KBFACCS",value.Kbfaccs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFAVPR",value.Kbfavpr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFMSR",value.Kbfmsr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFCMAT",value.Kbfcmat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFSEX",value.Kbfsex??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFMDQ",value.Kbfmdq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFMDA",value.Kbfmda, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFACTP",value.Kbfactp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFKADFH",value.Kbfkadfh, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFEXT",value.Kbfext??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFMNT3",value.Kbfmnt3, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBFMNT4",value.Kbfmnt4, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KBFQUA",value.Kbfqua??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFREN",value.Kbfren??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KBFRLO",value.Kbfrlo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KBFPAY",value.Kbfpay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KBFSIT2",value.Kbfsit2, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFADH2",value.Kbfadh2, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KBFSIT3",value.Kbfsit3, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFADH3",value.Kbfadh3, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KBFDES2",value.Kbfdes2, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFDES3",value.Kbfdes3, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFDES4",value.Kbfdes4, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KBFMRQ",value.Kbfmrq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFMOD",value.Kbfmod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KBFMIM",value.Kbfmim??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("parKBFID",value.Kbfid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpInveD> GetByAffaire(string typeAffaire, string codeAffaire, int numeroAliement){
                    return connection.EnsureOpened().Query<KpInveD>(select_GetByAffaire, new {typeAffaire, codeAffaire, numeroAliement}).ToList();
            }
            public IEnumerable<KpInveD> GetByInven(Int64 invenId){
                    return connection.EnsureOpened().Query<KpInveD>(select_GetByInven, new {invenId}).ToList();
            }
    }
}
