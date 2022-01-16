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

    public  partial class  KUsrDrtRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHRUSR, KHRBRA, KHRTYD FROM KUSRDRT
WHERE KHRUSR = :KHRUSR
and KHRBRA = :KHRBRA
";
            const string update=@"UPDATE KUSRDRT SET 
KHRUSR = :KHRUSR, KHRBRA = :KHRBRA, KHRTYD = :KHRTYD
 WHERE 
KHRUSR = :KHRUSR and KHRBRA = :KHRBRA";
            const string delete=@"DELETE FROM KUSRDRT WHERE KHRUSR = :KHRUSR AND KHRBRA = :KHRBRA";
            const string insert=@"INSERT INTO  KUSRDRT (
KHRUSR, KHRBRA, KHRTYD
) VALUES (
:KHRUSR, :KHRBRA, :KHRTYD)";
            const string select_GetAccesBranches=@"SELECT
KHRUSR, KHRBRA, KHRTYD FROM KUSRDRT
WHERE KHRUSR = :userId
";
            #endregion

            public KUsrDrtRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KUsrDrt Get(string KHRUSR, string KHRBRA){
                return connection.Query<KUsrDrt>(select, new {KHRUSR, KHRBRA}).SingleOrDefault();
            }


            public void Insert(KUsrDrt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHRUSR",value.Khrusr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHRBRA",value.Khrbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHRTYD",value.Khrtyd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KUsrDrt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHRUSR",value.Khrusr);
                    parameters.Add("KHRBRA",value.Khrbra);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KUsrDrt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHRUSR",value.Khrusr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHRBRA",value.Khrbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHRTYD",value.Khrtyd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHRUSR",value.Khrusr);
                    parameters.Add("KHRBRA",value.Khrbra);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KUsrDrt> GetAccesBranches(string userId){
                    return connection.EnsureOpened().Query<KUsrDrt>(select_GetAccesBranches, new {userId}).ToList();
            }
    }
}
