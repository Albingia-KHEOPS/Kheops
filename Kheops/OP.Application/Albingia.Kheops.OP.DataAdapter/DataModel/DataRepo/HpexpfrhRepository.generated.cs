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

    public  partial class  HpexpfrhRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDKID, KDKTYP, KDKIPB, KDKALX, KDKAVN
, KDKHIN, KDKFHE, KDKDESC, KDKDESI, KDKORI
, KDKMODI FROM HPEXPFRH
WHERE KDKID = :id
and KDKAVN = :numeroAvenant
";
            const string update=@"UPDATE HPEXPFRH SET 
KDKID = :KDKID, KDKTYP = :KDKTYP, KDKIPB = :KDKIPB, KDKALX = :KDKALX, KDKAVN = :KDKAVN, KDKHIN = :KDKHIN, KDKFHE = :KDKFHE, KDKDESC = :KDKDESC, KDKDESI = :KDKDESI, KDKORI = :KDKORI
, KDKMODI = :KDKMODI
 WHERE 
KDKID = :id and KDKAVN = :numeroAvenant";
            const string delete=@"DELETE FROM HPEXPFRH WHERE KDKID = :id AND KDKAVN = :numeroAvenant";
            const string insert=@"INSERT INTO  HPEXPFRH (
KDKID, KDKTYP, KDKIPB, KDKALX, KDKAVN
, KDKHIN, KDKFHE, KDKDESC, KDKDESI, KDKORI
, KDKMODI
) VALUES (
:KDKID, :KDKTYP, :KDKIPB, :KDKALX, :KDKAVN
, :KDKHIN, :KDKFHE, :KDKDESC, :KDKDESI, :KDKORI
, :KDKMODI)";
            const string select_GetByAffaire=@"SELECT
KDKID, KDKTYP, KDKIPB, KDKALX, KDKAVN
, KDKHIN, KDKFHE, KDKDESC, KDKDESI, KDKORI
, KDKMODI FROM HPEXPFRH
WHERE KDKTYP = :typeAffaire
and KDKIPB = :numeroAffaire
and KDKALX = :numeroAliment
and KDKAVN = :numeroAvenant
";
            #endregion

            public HpexpfrhRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpExpFrh Get(Int64 id, int numeroAvenant){
                return connection.Query<KpExpFrh>(select, new {id, numeroAvenant}).SingleOrDefault();
            }


            public void Insert(KpExpFrh value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDKID",value.Kdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDKTYP",value.Kdktyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDKIPB",value.Kdkipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDKALX",value.Kdkalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDKAVN",value.Kdkavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDKHIN",value.Kdkhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDKFHE",value.Kdkfhe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDKDESC",value.Kdkdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDKDESI",value.Kdkdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDKORI",value.Kdkori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDKMODI",value.Kdkmodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpExpFrh value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kdkid);
                    parameters.Add("numeroAvenant",value.Kdkavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpExpFrh value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDKID",value.Kdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDKTYP",value.Kdktyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDKIPB",value.Kdkipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDKALX",value.Kdkalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDKAVN",value.Kdkavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDKHIN",value.Kdkhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDKFHE",value.Kdkfhe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDKDESC",value.Kdkdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KDKDESI",value.Kdkdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDKORI",value.Kdkori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDKMODI",value.Kdkmodi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("id",value.Kdkid);
                    parameters.Add("numeroAvenant",value.Kdkavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpExpFrh> GetByAffaire(string typeAffaire, string numeroAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpExpFrh>(select_GetByAffaire, new {typeAffaire, numeroAffaire, numeroAliment, numeroAvenant}).ToList();
            }
    }
}
