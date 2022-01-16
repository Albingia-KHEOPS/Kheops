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

    public  partial class  KpOdblsRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAFID, KAFTYP, KAFIPB, KAFALX, KAFAVN
, KAFHIN, KAFICT, KAFSOU, KAFSAID, KAFSAIH
, KAFSIT, KAFSITD, KAFSITH, KAFSITU, KAFCRD
, KAFCRH, KAFCRU, KAFACT, KAFMOT FROM HPODBLS
WHERE KAFID = :KAFID
and KAFAVN = :KAFAVN
and KAFHIN = :KAFHIN
";
            const string update=@"UPDATE HPODBLS SET 
KAFID = :KAFID, KAFTYP = :KAFTYP, KAFIPB = :KAFIPB, KAFALX = :KAFALX, KAFAVN = :KAFAVN, KAFHIN = :KAFHIN, KAFICT = :KAFICT, KAFSOU = :KAFSOU, KAFSAID = :KAFSAID, KAFSAIH = :KAFSAIH
, KAFSIT = :KAFSIT, KAFSITD = :KAFSITD, KAFSITH = :KAFSITH, KAFSITU = :KAFSITU, KAFCRD = :KAFCRD, KAFCRH = :KAFCRH, KAFCRU = :KAFCRU, KAFACT = :KAFACT, KAFMOT = :KAFMOT
 WHERE 
KAFID = :KAFID and KAFAVN = :KAFAVN and KAFHIN = :KAFHIN";
            const string delete=@"DELETE FROM HPODBLS WHERE KAFID = :KAFID AND KAFAVN = :KAFAVN AND KAFHIN = :KAFHIN";
            const string insert=@"INSERT INTO  HPODBLS (
KAFID, KAFTYP, KAFIPB, KAFALX, KAFAVN
, KAFHIN, KAFICT, KAFSOU, KAFSAID, KAFSAIH
, KAFSIT, KAFSITD, KAFSITH, KAFSITU, KAFCRD
, KAFCRH, KAFCRU, KAFACT, KAFMOT
) VALUES (
:KAFID, :KAFTYP, :KAFIPB, :KAFALX, :KAFAVN
, :KAFHIN, :KAFICT, :KAFSOU, :KAFSAID, :KAFSAIH
, :KAFSIT, :KAFSITD, :KAFSITH, :KAFSITU, :KAFCRD
, :KAFCRH, :KAFCRU, :KAFACT, :KAFMOT)";
            const string select_GetByAffaire=@"SELECT
KAFID, KAFTYP, KAFIPB, KAFALX, KAFAVN
, KAFHIN, KAFICT, KAFSOU, KAFSAID, KAFSAIH
, KAFSIT, KAFSITD, KAFSITH, KAFSITU, KAFCRD
, KAFCRH, KAFCRU, KAFACT, KAFMOT FROM HPODBLS
WHERE KAFTYP = :KAFTYP
and KAFIPB = :KAFIPB
and KAFALX = :KAFALX
and KAFAVN = :KAFAVN
";
            #endregion

            public KpOdblsRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpOdbls Get(Int64 KAFID, int KAFAVN, int KAFHIN){
                return connection.Query<KpOdbls>(select, new {KAFID, KAFAVN, KAFHIN}).SingleOrDefault();
            }


            public void Insert(KpOdbls value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAFID",value.Kafid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAFTYP",value.Kaftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAFIPB",value.Kafipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAFALX",value.Kafalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAFAVN",value.Kafavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAFHIN",value.Kafhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAFICT",value.Kafict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAFSOU",value.Kafsou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAFSAID",value.Kafsaid, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAFSAIH",value.Kafsaih, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAFSIT",value.Kafsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAFSITD",value.Kafsitd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAFSITH",value.Kafsith, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAFSITU",value.Kafsitu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAFCRD",value.Kafcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAFCRH",value.Kafcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAFCRU",value.Kafcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAFACT",value.Kafact??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAFMOT",value.Kafmot??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpOdbls value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAFID",value.Kafid);
                    parameters.Add("KAFAVN",value.Kafavn);
                    parameters.Add("KAFHIN",value.Kafhin);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpOdbls value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAFID",value.Kafid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAFTYP",value.Kaftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAFIPB",value.Kafipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAFALX",value.Kafalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAFAVN",value.Kafavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAFHIN",value.Kafhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAFICT",value.Kafict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAFSOU",value.Kafsou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAFSAID",value.Kafsaid, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAFSAIH",value.Kafsaih, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAFSIT",value.Kafsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAFSITD",value.Kafsitd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAFSITH",value.Kafsith, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAFSITU",value.Kafsitu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAFCRD",value.Kafcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KAFCRH",value.Kafcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KAFCRU",value.Kafcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KAFACT",value.Kafact??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KAFMOT",value.Kafmot??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAFID",value.Kafid);
                    parameters.Add("KAFAVN",value.Kafavn);
                    parameters.Add("KAFHIN",value.Kafhin);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpOdbls> GetByAffaire(string KAFTYP, string KAFIPB, int KAFALX, int KAFAVN){
                    return connection.EnsureOpened().Query<KpOdbls>(select_GetByAffaire, new {KAFTYP, KAFIPB, KAFALX, KAFAVN}).ToList();
            }
    }
}
