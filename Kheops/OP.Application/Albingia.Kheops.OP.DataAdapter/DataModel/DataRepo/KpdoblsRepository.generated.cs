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

    public  partial class  KpdoblsRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAFID, KAFTYP, KAFIPB, KAFALX, KAFICT
, KAFSOU, KAFSAID, KAFSAIH, KAFSIT, KAFSITD
, KAFSITH, KAFSITU, KAFCRD, KAFCRH, KAFCRU
, KAFACT, KAFMOT, KAFIN5, KAFOCT FROM KPODBLS
WHERE KAFID = :id
FETCH FIRST 200 ROWS ONLY
";
            const string update=@"UPDATE KPODBLS SET 
KAFID = :KAFID, KAFTYP = :KAFTYP, KAFIPB = :KAFIPB, KAFALX = :KAFALX, KAFICT = :KAFICT, KAFSOU = :KAFSOU, KAFSAID = :KAFSAID, KAFSAIH = :KAFSAIH, KAFSIT = :KAFSIT, KAFSITD = :KAFSITD
, KAFSITH = :KAFSITH, KAFSITU = :KAFSITU, KAFCRD = :KAFCRD, KAFCRH = :KAFCRH, KAFCRU = :KAFCRU, KAFACT = :KAFACT, KAFMOT = :KAFMOT, KAFIN5 = :KAFIN5, KAFOCT = :KAFOCT
 WHERE 
KAFID = :id";
            const string delete=@"DELETE FROM KPODBLS WHERE KAFID = :id";
            const string insert=@"INSERT INTO  KPODBLS (
KAFID, KAFTYP, KAFIPB, KAFALX, KAFICT
, KAFSOU, KAFSAID, KAFSAIH, KAFSIT, KAFSITD
, KAFSITH, KAFSITU, KAFCRD, KAFCRH, KAFCRU
, KAFACT, KAFMOT, KAFIN5, KAFOCT
) VALUES (
:KAFID, :KAFTYP, :KAFIPB, :KAFALX, :KAFICT
, :KAFSOU, :KAFSAID, :KAFSAIH, :KAFSIT, :KAFSITD
, :KAFSITH, :KAFSITU, :KAFCRD, :KAFCRH, :KAFCRU
, :KAFACT, :KAFMOT, :KAFIN5, :KAFOCT)";
            const string select_Liste=@"SELECT
KAFID, KAFTYP, KAFIPB, KAFALX, KAFICT
, KAFSOU, KAFSAID, KAFSAIH, KAFSIT, KAFSITD
, KAFSITH, KAFSITU, KAFCRD, KAFCRH, KAFCRU
, KAFACT, KAFMOT, KAFIN5, KAFOCT FROM KPODBLS
FETCH FIRST 200 ROWS ONLY
";
            #endregion

            public KpdoblsRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Kpdobls Get(Int64 id){
                return connection.Query<Kpdobls>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KAFID") ;
            }

            public void Insert(Kpdobls value){
                    if(value.Kafid == default(Int64)) {
                        value.Kafid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KAFID",value.Kafid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAFTYP",value.Kaftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAFIPB",value.Kafipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAFALX",value.Kafalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
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
                    parameters.Add("KAFIN5",value.Kafin5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAFOCT",value.Kafoct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Kpdobls value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Kafid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Kpdobls value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAFID",value.Kafid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KAFTYP",value.Kaftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAFIPB",value.Kafipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAFALX",value.Kafalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
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
                    parameters.Add("KAFIN5",value.Kafin5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAFOCT",value.Kafoct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("id",value.Kafid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<Kpdobls_Liste> Liste(string WhereArgs){
                    return connection.EnsureOpened().Query<Kpdobls_Liste>(select_Liste).ToList();
            }
    }
}
