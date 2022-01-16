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

    public  partial class  HpcotgaRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDNID, KDNTYP, KDNIPB, KDNALX, KDNAVN
, KDNHIN, KDNFOR, KDNFOA, KDNOPT, KDNGARAN
, KDNKDEID, KDNNUMTAR, KDNKDGID, KDNTAROK, KDNKDMID
, KDNRSQ, KDNTRI, KDNHT, KDNHF, KDNKH
, KDNMHT, KDNKHT, KDNMTX, KDNKTX, KDNTTC
, KDNTTF, KDNMTT, KDNKTC, KDNCOT, KDNKCO
, KDNCNAMHT, KDNCNAKHT, KDNCNAMTX, KDNCNAKTX, KDNCNAMTT
, KDNCNAKTT, KDNCNACOM, KDNCNAKCM, KDNATGMHT, KDNATGKHT
, KDNATGMTX, KDNATGKTX, KDNATGMTT, KDNATGKTT FROM HPCOTGA
WHERE KDNTYP = :KDNTYP
and KDNIPB = :KDNIPB
and KDNALX = :KDNALX
and KDNAVN = :KDNAVN
and KDNHIN = :KDNHIN
and KDNID = :KDNID
";
            const string update=@"UPDATE HPCOTGA SET 
KDNID = :KDNID, KDNTYP = :KDNTYP, KDNIPB = :KDNIPB, KDNALX = :KDNALX, KDNAVN = :KDNAVN, KDNHIN = :KDNHIN, KDNFOR = :KDNFOR, KDNFOA = :KDNFOA, KDNOPT = :KDNOPT, KDNGARAN = :KDNGARAN
, KDNKDEID = :KDNKDEID, KDNNUMTAR = :KDNNUMTAR, KDNKDGID = :KDNKDGID, KDNTAROK = :KDNTAROK, KDNKDMID = :KDNKDMID, KDNRSQ = :KDNRSQ, KDNTRI = :KDNTRI, KDNHT = :KDNHT, KDNHF = :KDNHF, KDNKH = :KDNKH
, KDNMHT = :KDNMHT, KDNKHT = :KDNKHT, KDNMTX = :KDNMTX, KDNKTX = :KDNKTX, KDNTTC = :KDNTTC, KDNTTF = :KDNTTF, KDNMTT = :KDNMTT, KDNKTC = :KDNKTC, KDNCOT = :KDNCOT, KDNKCO = :KDNKCO
, KDNCNAMHT = :KDNCNAMHT, KDNCNAKHT = :KDNCNAKHT, KDNCNAMTX = :KDNCNAMTX, KDNCNAKTX = :KDNCNAKTX, KDNCNAMTT = :KDNCNAMTT, KDNCNAKTT = :KDNCNAKTT, KDNCNACOM = :KDNCNACOM, KDNCNAKCM = :KDNCNAKCM, KDNATGMHT = :KDNATGMHT, KDNATGKHT = :KDNATGKHT
, KDNATGMTX = :KDNATGMTX, KDNATGKTX = :KDNATGKTX, KDNATGMTT = :KDNATGMTT, KDNATGKTT = :KDNATGKTT
 WHERE 
KDNTYP = :KDNTYP and KDNIPB = :KDNIPB and KDNALX = :KDNALX and KDNAVN = :KDNAVN and KDNHIN = :KDNHIN and KDNID = :KDNID";
            const string delete=@"DELETE FROM HPCOTGA WHERE KDNTYP = :KDNTYP AND KDNIPB = :KDNIPB AND KDNALX = :KDNALX AND KDNAVN = :KDNAVN AND KDNHIN = :KDNHIN AND KDNID = :KDNID";
            const string insert=@"INSERT INTO  HPCOTGA (
KDNID, KDNTYP, KDNIPB, KDNALX, KDNAVN
, KDNHIN, KDNFOR, KDNFOA, KDNOPT, KDNGARAN
, KDNKDEID, KDNNUMTAR, KDNKDGID, KDNTAROK, KDNKDMID
, KDNRSQ, KDNTRI, KDNHT, KDNHF, KDNKH
, KDNMHT, KDNKHT, KDNMTX, KDNKTX, KDNTTC
, KDNTTF, KDNMTT, KDNKTC, KDNCOT, KDNKCO
, KDNCNAMHT, KDNCNAKHT, KDNCNAMTX, KDNCNAKTX, KDNCNAMTT
, KDNCNAKTT, KDNCNACOM, KDNCNAKCM, KDNATGMHT, KDNATGKHT
, KDNATGMTX, KDNATGKTX, KDNATGMTT, KDNATGKTT
) VALUES (
:KDNID, :KDNTYP, :KDNIPB, :KDNALX, :KDNAVN
, :KDNHIN, :KDNFOR, :KDNFOA, :KDNOPT, :KDNGARAN
, :KDNKDEID, :KDNNUMTAR, :KDNKDGID, :KDNTAROK, :KDNKDMID
, :KDNRSQ, :KDNTRI, :KDNHT, :KDNHF, :KDNKH
, :KDNMHT, :KDNKHT, :KDNMTX, :KDNKTX, :KDNTTC
, :KDNTTF, :KDNMTT, :KDNKTC, :KDNCOT, :KDNKCO
, :KDNCNAMHT, :KDNCNAKHT, :KDNCNAMTX, :KDNCNAKTX, :KDNCNAMTT
, :KDNCNAKTT, :KDNCNACOM, :KDNCNAKCM, :KDNATGMHT, :KDNATGKHT
, :KDNATGMTX, :KDNATGKTX, :KDNATGMTT, :KDNATGKTT)";
            const string select_GetByAffaire=@"SELECT
KDNID, KDNTYP, KDNIPB, KDNALX, KDNAVN
, KDNHIN, KDNFOR, KDNFOA, KDNOPT, KDNGARAN
, KDNKDEID, KDNNUMTAR, KDNKDGID, KDNTAROK, KDNKDMID
, KDNRSQ, KDNTRI, KDNHT, KDNHF, KDNKH
, KDNMHT, KDNKHT, KDNMTX, KDNKTX, KDNTTC
, KDNTTF, KDNMTT, KDNKTC, KDNCOT, KDNKCO
, KDNCNAMHT, KDNCNAKHT, KDNCNAMTX, KDNCNAKTX, KDNCNAMTT
, KDNCNAKTT, KDNCNACOM, KDNCNAKCM, KDNATGMHT, KDNATGKHT
, KDNATGMTX, KDNATGKTX, KDNATGMTT, KDNATGKTT FROM HPCOTGA
WHERE KDNTYP = :KDNTYP
and KDNIPB = :KDNIPB
and KDNALX = :KDNALX
and KDNAVN = :KDNAVN
";
            #endregion

            public HpcotgaRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpCotga Get(string KDNTYP, string KDNIPB, int KDNALX, int KDNAVN, int KDNHIN, Int64 KDNID){
                return connection.Query<KpCotga>(select, new {KDNTYP, KDNIPB, KDNALX, KDNAVN, KDNHIN, KDNID}).SingleOrDefault();
            }


            public void Insert(KpCotga value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDNID",value.Kdnid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDNTYP",value.Kdntyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDNIPB",value.Kdnipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDNALX",value.Kdnalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDNAVN",value.Kdnavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDNHIN",value.Kdnhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDNFOR",value.Kdnfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDNFOA",value.Kdnfoa??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDNOPT",value.Kdnopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDNGARAN",value.Kdngaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDNKDEID",value.Kdnkdeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDNNUMTAR",value.Kdnnumtar, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDNKDGID",value.Kdnkdgid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDNTAROK",value.Kdntarok??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDNKDMID",value.Kdnkdmid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDNRSQ",value.Kdnrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDNTRI",value.Kdntri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:18, scale: 0);
                    parameters.Add("KDNHT",value.Kdnht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNHF",value.Kdnhf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNKH",value.Kdnkh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNMHT",value.Kdnmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNKHT",value.Kdnkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNMTX",value.Kdnmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNKTX",value.Kdnktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNTTC",value.Kdnttc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNTTF",value.Kdnttf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNMTT",value.Kdnmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNKTC",value.Kdnktc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCOT",value.Kdncot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNKCO",value.Kdnkco, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAMHT",value.Kdncnamht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAKHT",value.Kdncnakht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAMTX",value.Kdncnamtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAKTX",value.Kdncnaktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAMTT",value.Kdncnamtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAKTT",value.Kdncnaktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNACOM",value.Kdncnacom, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAKCM",value.Kdncnakcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGMHT",value.Kdnatgmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGKHT",value.Kdnatgkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGMTX",value.Kdnatgmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGKTX",value.Kdnatgktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGMTT",value.Kdnatgmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGKTT",value.Kdnatgktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpCotga value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDNTYP",value.Kdntyp);
                    parameters.Add("KDNIPB",value.Kdnipb);
                    parameters.Add("KDNALX",value.Kdnalx);
                    parameters.Add("KDNAVN",value.Kdnavn);
                    parameters.Add("KDNHIN",value.Kdnhin);
                    parameters.Add("KDNID",value.Kdnid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpCotga value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDNID",value.Kdnid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDNTYP",value.Kdntyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDNIPB",value.Kdnipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDNALX",value.Kdnalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDNAVN",value.Kdnavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDNHIN",value.Kdnhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDNFOR",value.Kdnfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDNFOA",value.Kdnfoa??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDNOPT",value.Kdnopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDNGARAN",value.Kdngaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDNKDEID",value.Kdnkdeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDNNUMTAR",value.Kdnnumtar, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDNKDGID",value.Kdnkdgid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDNTAROK",value.Kdntarok??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDNKDMID",value.Kdnkdmid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDNRSQ",value.Kdnrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDNTRI",value.Kdntri??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:18, scale: 0);
                    parameters.Add("KDNHT",value.Kdnht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNHF",value.Kdnhf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNKH",value.Kdnkh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNMHT",value.Kdnmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNKHT",value.Kdnkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNMTX",value.Kdnmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNKTX",value.Kdnktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNTTC",value.Kdnttc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNTTF",value.Kdnttf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNMTT",value.Kdnmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNKTC",value.Kdnktc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCOT",value.Kdncot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNKCO",value.Kdnkco, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAMHT",value.Kdncnamht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAKHT",value.Kdncnakht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAMTX",value.Kdncnamtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAKTX",value.Kdncnaktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAMTT",value.Kdncnamtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAKTT",value.Kdncnaktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNACOM",value.Kdncnacom, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNCNAKCM",value.Kdncnakcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGMHT",value.Kdnatgmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGKHT",value.Kdnatgkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGMTX",value.Kdnatgmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGKTX",value.Kdnatgktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGMTT",value.Kdnatgmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNATGKTT",value.Kdnatgktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDNTYP",value.Kdntyp);
                    parameters.Add("KDNIPB",value.Kdnipb);
                    parameters.Add("KDNALX",value.Kdnalx);
                    parameters.Add("KDNAVN",value.Kdnavn);
                    parameters.Add("KDNHIN",value.Kdnhin);
                    parameters.Add("KDNID",value.Kdnid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpCotga> GetByAffaire(string KDNTYP, string KDNIPB, int KDNALX, int KDNAVN){
                    return connection.EnsureOpened().Query<KpCotga>(select_GetByAffaire, new {KDNTYP, KDNIPB, KDNALX, KDNAVN}).ToList();
            }
    }
}
