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

    public  partial class  HpoppRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KFPID, KFPTYP, KFPIPB, KFPALX, KFPAVN
, KFPHIN, KFPIDCB, KFPTFI, KFPDESI, KFPREF
, KFPDECH, KFPMNT, KFPCRU, KFPCRD, KFPCRH
, KFPMAJU, KFPMAJD, KFPMAJH, KFPTDS, KFPTYI
 FROM HPOPP
WHERE KFPID = :KFPID
and KFPAVN = :KFPAVN
and KFPHIN = :KFPHIN
";
            const string update=@"UPDATE HPOPP SET 
KFPID = :KFPID, KFPTYP = :KFPTYP, KFPIPB = :KFPIPB, KFPALX = :KFPALX, KFPAVN = :KFPAVN, KFPHIN = :KFPHIN, KFPIDCB = :KFPIDCB, KFPTFI = :KFPTFI, KFPDESI = :KFPDESI, KFPREF = :KFPREF
, KFPDECH = :KFPDECH, KFPMNT = :KFPMNT, KFPCRU = :KFPCRU, KFPCRD = :KFPCRD, KFPCRH = :KFPCRH, KFPMAJU = :KFPMAJU, KFPMAJD = :KFPMAJD, KFPMAJH = :KFPMAJH, KFPTDS = :KFPTDS, KFPTYI = :KFPTYI

 WHERE 
KFPID = :KFPID and KFPAVN = :KFPAVN and KFPHIN = :KFPHIN";
            const string delete=@"DELETE FROM HPOPP WHERE KFPID = :KFPID AND KFPAVN = :KFPAVN AND KFPHIN = :KFPHIN";
            const string insert=@"INSERT INTO  HPOPP (
KFPID, KFPTYP, KFPIPB, KFPALX, KFPAVN
, KFPHIN, KFPIDCB, KFPTFI, KFPDESI, KFPREF
, KFPDECH, KFPMNT, KFPCRU, KFPCRD, KFPCRH
, KFPMAJU, KFPMAJD, KFPMAJH, KFPTDS, KFPTYI

) VALUES (
:KFPID, :KFPTYP, :KFPIPB, :KFPALX, :KFPAVN
, :KFPHIN, :KFPIDCB, :KFPTFI, :KFPDESI, :KFPREF
, :KFPDECH, :KFPMNT, :KFPCRU, :KFPCRD, :KFPCRH
, :KFPMAJU, :KFPMAJD, :KFPMAJH, :KFPTDS, :KFPTYI
)";
            const string select_GetByAffaire=@"SELECT
KFPID, KFPTYP, KFPIPB, KFPALX, KFPAVN
, KFPHIN, KFPIDCB, KFPTFI, KFPDESI, KFPREF
, KFPDECH, KFPMNT, KFPCRU, KFPCRD, KFPCRH
, KFPMAJU, KFPMAJD, KFPMAJH, KFPTDS, KFPTYI
 FROM HPOPP
WHERE KFPTYP = :KFPTYP
and KFPIPB = :KFPIPB
and KFPALX = :KFPALX
and KFPAVN = :KFPAVN
";
            #endregion

            public HpoppRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOpp Get(Int64 KFPID, int KFPAVN, int KFPHIN){
                return connection.Query<KpOpp>(select, new {KFPID, KFPAVN, KFPHIN}).SingleOrDefault();
            }


            public void Insert(KpOpp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFPID",value.Kfpid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFPTYP",value.Kfptyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFPIPB",value.Kfpipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFPALX",value.Kfpalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFPAVN",value.Kfpavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFPHIN",value.Kfphin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFPIDCB",value.Kfpidcb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFPTFI",value.Kfptfi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFPDESI",value.Kfpdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFPREF",value.Kfpref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KFPDECH",value.Kfpdech, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFPMNT",value.Kfpmnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 2);
                    parameters.Add("KFPCRU",value.Kfpcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFPCRD",value.Kfpcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFPCRH",value.Kfpcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFPMAJU",value.Kfpmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFPMAJD",value.Kfpmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFPMAJH",value.Kfpmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFPTDS",value.Kfptds??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFPTYI",value.Kfptyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpOpp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFPID",value.Kfpid);
                    parameters.Add("KFPAVN",value.Kfpavn);
                    parameters.Add("KFPHIN",value.Kfphin);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOpp value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFPID",value.Kfpid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFPTYP",value.Kfptyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFPIPB",value.Kfpipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFPALX",value.Kfpalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFPAVN",value.Kfpavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFPHIN",value.Kfphin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFPIDCB",value.Kfpidcb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFPTFI",value.Kfptfi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFPDESI",value.Kfpdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFPREF",value.Kfpref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KFPDECH",value.Kfpdech, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFPMNT",value.Kfpmnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 2);
                    parameters.Add("KFPCRU",value.Kfpcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFPCRD",value.Kfpcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFPCRH",value.Kfpcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFPMAJU",value.Kfpmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFPMAJD",value.Kfpmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFPMAJH",value.Kfpmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFPTDS",value.Kfptds??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFPTYI",value.Kfptyi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFPID",value.Kfpid);
                    parameters.Add("KFPAVN",value.Kfpavn);
                    parameters.Add("KFPHIN",value.Kfphin);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOpp> GetByAffaire(string KFPTYP, string KFPIPB, int KFPALX, int KFPAVN){
                    return connection.EnsureOpened().Query<KpOpp>(select_GetByAffaire, new {KFPTYP, KFPIPB, KFPALX, KFPAVN}).ToList();
            }
    }
}
