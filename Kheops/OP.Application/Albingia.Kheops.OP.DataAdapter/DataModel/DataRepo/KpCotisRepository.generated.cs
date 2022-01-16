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

    public  partial class  KpCotisRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDMID, KDMTAP, KDMTYP, KDMIPB, KDMALX
, KDMATGMHT, KDMATGKHT, KDMATGMTX, KDMATGKTX, KDMATGMTT
, KDMATGKTT, KDMATGCOT, KDMATGKCO, KDMCNABAS, KDMCNAKBS
, KDMCNAMHT, KDMCNAKHT, KDMCNAMTX, KDMCNAKTX, KDMCNAMTT
, KDMCNAKTT, KDMCNACOB, KDMCNACNC, KDMCNATXF, KDMCNACNM
, KDMCNACMF, KDMCNAKCM, KDMGARMHT, KDMGARMTX, KDMGARMTT
, KDMHFMHT, KDMHFFLAG, KDMHFMHF, KDMHFMTX, KDMHFMTT
, KDMAFRB, KDMAFR, KDMKFA, KDMAFT, KDMKFT
, KDMFGA, KDMKFG, KDMMHT, KDMMHFLAG, KDMMHF
, KDMKHT, KDMMTX, KDMKTX, KDMMTT, KDMMTFLAG
, KDMTTF, KDMKTT, KDMCOB, KDMCOM, KDMCMF
, KDMCOT, KDMCOF, KDMKCO, KDMCOEFC FROM KPCOTIS
WHERE KDMTYP = :KDMTYP
and KDMIPB = :KDMIPB
and KDMALX = :KDMALX
and KDMTAP = :KDMTAP
and KDMID = :KDMID
";
            const string update=@"UPDATE KPCOTIS SET 
KDMID = :KDMID, KDMTAP = :KDMTAP, KDMTYP = :KDMTYP, KDMIPB = :KDMIPB, KDMALX = :KDMALX, KDMATGMHT = :KDMATGMHT, KDMATGKHT = :KDMATGKHT, KDMATGMTX = :KDMATGMTX, KDMATGKTX = :KDMATGKTX, KDMATGMTT = :KDMATGMTT
, KDMATGKTT = :KDMATGKTT, KDMATGCOT = :KDMATGCOT, KDMATGKCO = :KDMATGKCO, KDMCNABAS = :KDMCNABAS, KDMCNAKBS = :KDMCNAKBS, KDMCNAMHT = :KDMCNAMHT, KDMCNAKHT = :KDMCNAKHT, KDMCNAMTX = :KDMCNAMTX, KDMCNAKTX = :KDMCNAKTX, KDMCNAMTT = :KDMCNAMTT
, KDMCNAKTT = :KDMCNAKTT, KDMCNACOB = :KDMCNACOB, KDMCNACNC = :KDMCNACNC, KDMCNATXF = :KDMCNATXF, KDMCNACNM = :KDMCNACNM, KDMCNACMF = :KDMCNACMF, KDMCNAKCM = :KDMCNAKCM, KDMGARMHT = :KDMGARMHT, KDMGARMTX = :KDMGARMTX, KDMGARMTT = :KDMGARMTT
, KDMHFMHT = :KDMHFMHT, KDMHFFLAG = :KDMHFFLAG, KDMHFMHF = :KDMHFMHF, KDMHFMTX = :KDMHFMTX, KDMHFMTT = :KDMHFMTT, KDMAFRB = :KDMAFRB, KDMAFR = :KDMAFR, KDMKFA = :KDMKFA, KDMAFT = :KDMAFT, KDMKFT = :KDMKFT
, KDMFGA = :KDMFGA, KDMKFG = :KDMKFG, KDMMHT = :KDMMHT, KDMMHFLAG = :KDMMHFLAG, KDMMHF = :KDMMHF, KDMKHT = :KDMKHT, KDMMTX = :KDMMTX, KDMKTX = :KDMKTX, KDMMTT = :KDMMTT, KDMMTFLAG = :KDMMTFLAG
, KDMTTF = :KDMTTF, KDMKTT = :KDMKTT, KDMCOB = :KDMCOB, KDMCOM = :KDMCOM, KDMCMF = :KDMCMF, KDMCOT = :KDMCOT, KDMCOF = :KDMCOF, KDMKCO = :KDMKCO, KDMCOEFC = :KDMCOEFC
 WHERE 
KDMTYP = :KDMTYP and KDMIPB = :KDMIPB and KDMALX = :KDMALX and KDMTAP = :KDMTAP and KDMID = :KDMID";
            const string delete=@"DELETE FROM KPCOTIS WHERE KDMTYP = :KDMTYP AND KDMIPB = :KDMIPB AND KDMALX = :KDMALX AND KDMTAP = :KDMTAP AND KDMID = :KDMID";
            const string insert=@"INSERT INTO  KPCOTIS (
KDMID, KDMTAP, KDMTYP, KDMIPB, KDMALX
, KDMATGMHT, KDMATGKHT, KDMATGMTX, KDMATGKTX, KDMATGMTT
, KDMATGKTT, KDMATGCOT, KDMATGKCO, KDMCNABAS, KDMCNAKBS
, KDMCNAMHT, KDMCNAKHT, KDMCNAMTX, KDMCNAKTX, KDMCNAMTT
, KDMCNAKTT, KDMCNACOB, KDMCNACNC, KDMCNATXF, KDMCNACNM
, KDMCNACMF, KDMCNAKCM, KDMGARMHT, KDMGARMTX, KDMGARMTT
, KDMHFMHT, KDMHFFLAG, KDMHFMHF, KDMHFMTX, KDMHFMTT
, KDMAFRB, KDMAFR, KDMKFA, KDMAFT, KDMKFT
, KDMFGA, KDMKFG, KDMMHT, KDMMHFLAG, KDMMHF
, KDMKHT, KDMMTX, KDMKTX, KDMMTT, KDMMTFLAG
, KDMTTF, KDMKTT, KDMCOB, KDMCOM, KDMCMF
, KDMCOT, KDMCOF, KDMKCO, KDMCOEFC
) VALUES (
:KDMID, :KDMTAP, :KDMTYP, :KDMIPB, :KDMALX
, :KDMATGMHT, :KDMATGKHT, :KDMATGMTX, :KDMATGKTX, :KDMATGMTT
, :KDMATGKTT, :KDMATGCOT, :KDMATGKCO, :KDMCNABAS, :KDMCNAKBS
, :KDMCNAMHT, :KDMCNAKHT, :KDMCNAMTX, :KDMCNAKTX, :KDMCNAMTT
, :KDMCNAKTT, :KDMCNACOB, :KDMCNACNC, :KDMCNATXF, :KDMCNACNM
, :KDMCNACMF, :KDMCNAKCM, :KDMGARMHT, :KDMGARMTX, :KDMGARMTT
, :KDMHFMHT, :KDMHFFLAG, :KDMHFMHF, :KDMHFMTX, :KDMHFMTT
, :KDMAFRB, :KDMAFR, :KDMKFA, :KDMAFT, :KDMKFT
, :KDMFGA, :KDMKFG, :KDMMHT, :KDMMHFLAG, :KDMMHF
, :KDMKHT, :KDMMTX, :KDMKTX, :KDMMTT, :KDMMTFLAG
, :KDMTTF, :KDMKTT, :KDMCOB, :KDMCOM, :KDMCMF
, :KDMCOT, :KDMCOF, :KDMKCO, :KDMCOEFC)";
            const string select_GetByAffaire=@"SELECT
KDMID, KDMTAP, KDMTYP, KDMIPB, KDMALX
, KDMATGMHT, KDMATGKHT, KDMATGMTX, KDMATGKTX, KDMATGMTT
, KDMATGKTT, KDMATGCOT, KDMATGKCO, KDMCNABAS, KDMCNAKBS
, KDMCNAMHT, KDMCNAKHT, KDMCNAMTX, KDMCNAKTX, KDMCNAMTT
, KDMCNAKTT, KDMCNACOB, KDMCNACNC, KDMCNATXF, KDMCNACNM
, KDMCNACMF, KDMCNAKCM, KDMGARMHT, KDMGARMTX, KDMGARMTT
, KDMHFMHT, KDMHFFLAG, KDMHFMHF, KDMHFMTX, KDMHFMTT
, KDMAFRB, KDMAFR, KDMKFA, KDMAFT, KDMKFT
, KDMFGA, KDMKFG, KDMMHT, KDMMHFLAG, KDMMHF
, KDMKHT, KDMMTX, KDMKTX, KDMMTT, KDMMTFLAG
, KDMTTF, KDMKTT, KDMCOB, KDMCOM, KDMCMF
, KDMCOT, KDMCOF, KDMKCO, KDMCOEFC FROM KPCOTIS
WHERE KDMTYP = :KDMTYP
and KDMIPB = :KDMIPB
and KDMALX = :KDMALX
";
            #endregion

            public KpCotisRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpCotis Get(string KDMTYP, string KDMIPB, int KDMALX, string KDMTAP, Int64 KDMID){
                return connection.Query<KpCotis>(select, new {KDMTYP, KDMIPB, KDMALX, KDMTAP, KDMID}).SingleOrDefault();
            }


            public void Insert(KpCotis value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDMID",value.Kdmid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDMTAP",value.Kdmtap??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMTYP",value.Kdmtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMIPB",value.Kdmipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDMALX",value.Kdmalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDMATGMHT",value.Kdmatgmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGKHT",value.Kdmatgkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGMTX",value.Kdmatgmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGKTX",value.Kdmatgktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGMTT",value.Kdmatgmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGKTT",value.Kdmatgktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGCOT",value.Kdmatgcot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGKCO",value.Kdmatgkco, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNABAS",value.Kdmcnabas, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAKBS",value.Kdmcnakbs, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAMHT",value.Kdmcnamht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAKHT",value.Kdmcnakht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAMTX",value.Kdmcnamtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAKTX",value.Kdmcnaktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAMTT",value.Kdmcnamtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAKTT",value.Kdmcnaktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNACOB",value.Kdmcnacob??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMCNACNC",value.Kdmcnacnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KDMCNATXF",value.Kdmcnatxf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KDMCNACNM",value.Kdmcnacnm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNACMF",value.Kdmcnacmf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAKCM",value.Kdmcnakcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMGARMHT",value.Kdmgarmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMGARMTX",value.Kdmgarmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMGARMTT",value.Kdmgarmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMHFMHT",value.Kdmhfmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMHFFLAG",value.Kdmhfflag??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMHFMHF",value.Kdmhfmhf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMHFMTX",value.Kdmhfmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMHFMTT",value.Kdmhfmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMAFRB",value.Kdmafrb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMAFR",value.Kdmafr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMKFA",value.Kdmkfa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMAFT",value.Kdmaft, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMKFT",value.Kdmkft, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMFGA",value.Kdmfga, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMKFG",value.Kdmkfg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMMHT",value.Kdmmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMMHFLAG",value.Kdmmhflag??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMMHF",value.Kdmmhf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMKHT",value.Kdmkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMMTX",value.Kdmmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMKTX",value.Kdmktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMMTT",value.Kdmmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMMTFLAG",value.Kdmmtflag??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMTTF",value.Kdmttf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMKTT",value.Kdmktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCOB",value.Kdmcob??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMCOM",value.Kdmcom, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KDMCMF",value.Kdmcmf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KDMCOT",value.Kdmcot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCOF",value.Kdmcof, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMKCO",value.Kdmkco, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCOEFC",value.Kdmcoefc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpCotis value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDMTYP",value.Kdmtyp);
                    parameters.Add("KDMIPB",value.Kdmipb);
                    parameters.Add("KDMALX",value.Kdmalx);
                    parameters.Add("KDMTAP",value.Kdmtap);
                    parameters.Add("KDMID",value.Kdmid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpCotis value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDMID",value.Kdmid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDMTAP",value.Kdmtap??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMTYP",value.Kdmtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMIPB",value.Kdmipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDMALX",value.Kdmalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDMATGMHT",value.Kdmatgmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGKHT",value.Kdmatgkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGMTX",value.Kdmatgmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGKTX",value.Kdmatgktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGMTT",value.Kdmatgmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGKTT",value.Kdmatgktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGCOT",value.Kdmatgcot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMATGKCO",value.Kdmatgkco, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNABAS",value.Kdmcnabas, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAKBS",value.Kdmcnakbs, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAMHT",value.Kdmcnamht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAKHT",value.Kdmcnakht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAMTX",value.Kdmcnamtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAKTX",value.Kdmcnaktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAMTT",value.Kdmcnamtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAKTT",value.Kdmcnaktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNACOB",value.Kdmcnacob??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMCNACNC",value.Kdmcnacnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KDMCNATXF",value.Kdmcnatxf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KDMCNACNM",value.Kdmcnacnm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNACMF",value.Kdmcnacmf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCNAKCM",value.Kdmcnakcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMGARMHT",value.Kdmgarmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMGARMTX",value.Kdmgarmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMGARMTT",value.Kdmgarmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMHFMHT",value.Kdmhfmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMHFFLAG",value.Kdmhfflag??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMHFMHF",value.Kdmhfmhf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMHFMTX",value.Kdmhfmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMHFMTT",value.Kdmhfmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMAFRB",value.Kdmafrb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMAFR",value.Kdmafr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMKFA",value.Kdmkfa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMAFT",value.Kdmaft, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMKFT",value.Kdmkft, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMFGA",value.Kdmfga, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMKFG",value.Kdmkfg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KDMMHT",value.Kdmmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMMHFLAG",value.Kdmmhflag??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMMHF",value.Kdmmhf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMKHT",value.Kdmkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMMTX",value.Kdmmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMKTX",value.Kdmktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMMTT",value.Kdmmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMMTFLAG",value.Kdmmtflag??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMTTF",value.Kdmttf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMKTT",value.Kdmktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCOB",value.Kdmcob??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDMCOM",value.Kdmcom, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KDMCMF",value.Kdmcmf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KDMCOT",value.Kdmcot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCOF",value.Kdmcof, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMKCO",value.Kdmkco, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDMCOEFC",value.Kdmcoefc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KDMTYP",value.Kdmtyp);
                    parameters.Add("KDMIPB",value.Kdmipb);
                    parameters.Add("KDMALX",value.Kdmalx);
                    parameters.Add("KDMTAP",value.Kdmtap);
                    parameters.Add("KDMID",value.Kdmid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpCotis> GetByAffaire(string KDMTYP, string KDMIPB, int KDMALX){
                    return connection.EnsureOpened().Query<KpCotis>(select_GetByAffaire, new {KDMTYP, KDMIPB, KDMALX}).ToList();
            }
    }
}
