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

    public  partial class  KISRefRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKSPP
            const string select=@"SELECT
KGBNMID, KGBDESC, KGBLIB, KGBTYPZ, KGBMAPP
, KGBTYPC, KGBPRES, KGBTYPU, KGBOBLI, KGBSCRAFF
, KGBSCRCTR, KGBOBSV, KGBNMBD, KGBLNGZ, KGBSAID2
, KGBSCID2, KGBNREF, KGBDVAL, KGBVDEC, KGBVDECN
, KGBVUCON, KGBVDATD, KGBVHEUD, KGBVDATF, KGBVHEUF
, KGBVTXT, KGBKFBID, KGBVUFAM FROM KISREF
WHERE KGBNMID = :KGBNMID
";
            const string update=@"UPDATE KISREF SET 
KGBNMID = :KGBNMID, KGBDESC = :KGBDESC, KGBLIB = :KGBLIB, KGBTYPZ = :KGBTYPZ, KGBMAPP = :KGBMAPP, KGBTYPC = :KGBTYPC, KGBPRES = :KGBPRES, KGBTYPU = :KGBTYPU, KGBOBLI = :KGBOBLI, KGBSCRAFF = :KGBSCRAFF
, KGBSCRCTR = :KGBSCRCTR, KGBOBSV = :KGBOBSV, KGBNMBD = :KGBNMBD, KGBLNGZ = :KGBLNGZ, KGBSAID2 = :KGBSAID2, KGBSCID2 = :KGBSCID2, KGBNREF = :KGBNREF, KGBDVAL = :KGBDVAL, KGBVDEC = :KGBVDEC, KGBVDECN = :KGBVDECN
, KGBVUCON = :KGBVUCON, KGBVDATD = :KGBVDATD, KGBVHEUD = :KGBVHEUD, KGBVDATF = :KGBVDATF, KGBVHEUF = :KGBVHEUF, KGBVTXT = :KGBVTXT, KGBKFBID = :KGBKFBID, KGBVUFAM = :KGBVUFAM
 WHERE 
KGBNMID = :KGBNMID";
            const string delete=@"DELETE FROM KISREF WHERE KGBNMID = :KGBNMID";
            const string insert=@"INSERT INTO  KISREF (
KGBNMID, KGBDESC, KGBLIB, KGBTYPZ, KGBMAPP
, KGBTYPC, KGBPRES, KGBTYPU, KGBOBLI, KGBSCRAFF
, KGBSCRCTR, KGBOBSV, KGBNMBD, KGBLNGZ, KGBSAID2
, KGBSCID2, KGBNREF, KGBDVAL, KGBVDEC, KGBVDECN
, KGBVUCON, KGBVDATD, KGBVHEUD, KGBVDATF, KGBVHEUF
, KGBVTXT, KGBKFBID, KGBVUFAM
) VALUES (
:KGBNMID, :KGBDESC, :KGBLIB, :KGBTYPZ, :KGBMAPP
, :KGBTYPC, :KGBPRES, :KGBTYPU, :KGBOBLI, :KGBSCRAFF
, :KGBSCRCTR, :KGBOBSV, :KGBNMBD, :KGBLNGZ, :KGBSAID2
, :KGBSCID2, :KGBNREF, :KGBDVAL, :KGBVDEC, :KGBVDECN
, :KGBVUCON, :KGBVDATD, :KGBVHEUD, :KGBVDATF, :KGBVHEUF
, :KGBVTXT, :KGBKFBID, :KGBVUFAM)";
            const string select_GetAll=@"SELECT
KGBNMID, KGBDESC, KGBLIB, KGBTYPZ, KGBMAPP
, KGBTYPC, KGBPRES, KGBTYPU, KGBOBLI, KGBSCRAFF
, KGBSCRCTR, KGBOBSV, KGBNMBD, KGBLNGZ, KGBSAID2
, KGBSCID2, KGBNREF, KGBDVAL, KGBVDEC, KGBVDECN
, KGBVUCON, KGBVDATD, KGBVHEUD, KGBVDATF, KGBVHEUF
, KGBVTXT, KGBKFBID, KGBVUFAM FROM KISREF
";
            #endregion

            public KISRefRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KISRef Get(string KGBNMID){
                return connection.Query<KISRef>(select, new {KGBNMID}).SingleOrDefault();
            }

            public string NewId () {
                return idGenerator.NewId("KGBNMID").ToString() ;
            }

            public void Insert(KISRef value){
                    if(value.Kgbnmid == default(string)) {
                        value.Kgbnmid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KGBNMID",value.Kgbnmid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KGBDESC",value.Kgbdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:80, scale: 0);
                    parameters.Add("KGBLIB",value.Kgblib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:80, scale: 0);
                    parameters.Add("KGBTYPZ",value.Kgbtypz??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGBMAPP",value.Kgbmapp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBTYPC",value.Kgbtypc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBPRES",value.Kgbpres, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGBTYPU",value.Kgbtypu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KGBOBLI",value.Kgbobli??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBSCRAFF",value.Kgbscraff??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBSCRCTR",value.Kgbscrctr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBOBSV",value.Kgbobsv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGBNMBD",value.Kgbnmbd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGBLNGZ",value.Kgblngz??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGBSAID2",value.Kgbsaid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGBSCID2",value.Kgbscid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGBNREF",value.Kgbnref, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGBDVAL",value.Kgbdval??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KGBVDEC",value.Kgbvdec??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVDECN",value.Kgbvdecn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVUCON",value.Kgbvucon??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KGBVDATD",value.Kgbvdatd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVHEUD",value.Kgbvheud??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVDATF",value.Kgbvdatf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVHEUF",value.Kgbvheuf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVTXT",value.Kgbvtxt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBKFBID",value.Kgbkfbid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVUFAM",value.Kgbvufam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KISRef value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGBNMID",value.Kgbnmid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KISRef value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KGBNMID",value.Kgbnmid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KGBDESC",value.Kgbdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:80, scale: 0);
                    parameters.Add("KGBLIB",value.Kgblib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:80, scale: 0);
                    parameters.Add("KGBTYPZ",value.Kgbtypz??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KGBMAPP",value.Kgbmapp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBTYPC",value.Kgbtypc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBPRES",value.Kgbpres, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGBTYPU",value.Kgbtypu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("KGBOBLI",value.Kgbobli??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBSCRAFF",value.Kgbscraff??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBSCRCTR",value.Kgbscrctr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBOBSV",value.Kgbobsv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("KGBNMBD",value.Kgbnmbd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KGBLNGZ",value.Kgblngz??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGBSAID2",value.Kgbsaid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGBSCID2",value.Kgbscid2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:50, scale: 0);
                    parameters.Add("KGBNREF",value.Kgbnref, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KGBDVAL",value.Kgbdval??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KGBVDEC",value.Kgbvdec??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVDECN",value.Kgbvdecn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVUCON",value.Kgbvucon??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KGBVDATD",value.Kgbvdatd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVHEUD",value.Kgbvheud??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVDATF",value.Kgbvdatf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVHEUF",value.Kgbvheuf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVTXT",value.Kgbvtxt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBKFBID",value.Kgbkfbid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KGBVUFAM",value.Kgbvufam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KGBNMID",value.Kgbnmid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KISRef> GetAll(){
                    return connection.EnsureOpened().Query<KISRef>(select_GetAll).ToList();
            }
    }
}
