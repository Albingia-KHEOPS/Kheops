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

    public  partial class  YparRepository {

            private IDbConnection connection;

            #region Query texts        
            //ZALBINKQUA
            const string select=@"SELECT
TCON, TFAM, TCOD, TPLIB, TPCN1
, TPCN2, TPCA1, TPCA2, TPTYP, TPLIL
, TFILT FROM YYYYPAR
WHERE TCON = :Concept
and TFAM = :Famille
and TCOD = :Code
FETCH FIRST 200 ROWS ONLY
";
            const string update=@"UPDATE YYYYPAR SET 
TCON = :TCON, TFAM = :TFAM, TCOD = :TCOD, TPLIB = :TPLIB, TPCN1 = :TPCN1, TPCN2 = :TPCN2, TPCA1 = :TPCA1, TPCA2 = :TPCA2, TPTYP = :TPTYP, TPLIL = :TPLIL
, TFILT = :TFILT
 WHERE 
TCON = :Concept and TFAM = :Famille and TCOD = :Code";
            const string delete=@"DELETE FROM YYYYPAR WHERE TCON = :Concept AND TFAM = :Famille AND TCOD = :Code";
            const string insert=@"INSERT INTO  YYYYPAR (
TCON, TFAM, TCOD, TPLIB, TPCN1
, TPCN2, TPCA1, TPCA2, TPTYP, TPLIL
, TFILT
) VALUES (
:TCON, :TFAM, :TCOD, :TPLIB, :TPCN1
, :TPCN2, :TPCA1, :TPCA2, :TPTYP, :TPLIL
, :TFILT)";
            const string select_GetFamille=@"SELECT
TCON, TFAM, TCOD, TPLIB, TPCN1
, TPCN2, TPCA1, TPCA2, TPTYP, TPLIL
, TFILT FROM YYYYPAR
WHERE TCON = :Concept
and TFAM = :Famille
";
            #endregion

            public YparRepository (IDbConnection connection) {
                this.connection = connection;
            }
            public Ypar Get(string Concept, string Famille, string Code){
                return connection.Query<Ypar>(select, new {Concept, Famille, Code}).SingleOrDefault();
            }
            private IDbDataParameter Param(string name, object value) {
                var com = this.connection.CreateCommand();
                var p=com.CreateParameter(); 
                p.ParameterName=name; 
                p.Value=value; 
                return p;
            }
            private IDbDataParameter OutParam(string name, object value, int size) {
                var p = Param(name, value);
                p.Size = size;
                p.Direction = ParameterDirection.Output;
                return p;
            }

            public void Insert(Ypar value){
                try {
                    var parameters = new DynamicParameters();
                    parameters.Add("TCON",value.Tcon??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TFAM",value.Tfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCOD",value.Tcod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("TPLIB",value.Tplib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("TPCN1",value.Tpcn1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 3);
                    parameters.Add("TPCN2",value.Tpcn2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 3);
                    parameters.Add("TPCA1",value.Tpca1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("TPCA2",value.Tpca2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("TPTYP",value.Tptyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TPLIL",value.Tplil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("TFILT",value.Tfilt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
                } finally {
                    connection.EnsureClosed();
                }
            }
            public void Delete(Ypar value){
                try {
                    var parameters = new DynamicParameters();
                    parameters.Add("Concept",value.Tcon);
                    parameters.Add("Famille",value.Tfam);
                    parameters.Add("Code",value.Tcod);
                        connection.EnsureOpened().Execute(delete, parameters);
                } finally {
                    connection.EnsureClosed();
                }
            }

            public void Update(Ypar value){
                try {
                    var parameters = new DynamicParameters();
                    parameters.Add("TCON",value.Tcon??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TFAM",value.Tfam??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("TCOD",value.Tcod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("TPLIB",value.Tplib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("TPCN1",value.Tpcn1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 3);
                    parameters.Add("TPCN2",value.Tpcn2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 3);
                    parameters.Add("TPCA1",value.Tpca1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("TPCA2",value.Tpca2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("TPTYP",value.Tptyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("TPLIL",value.Tplil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("TFILT",value.Tfilt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("Concept",value.Tcon);
                    parameters.Add("Famille",value.Tfam);
                    parameters.Add("Code",value.Tcod);
                    
                    connection.EnsureOpened().Execute(update, parameters);
                } finally {
                    connection.EnsureClosed();
                }
            }
    
            public IEnumerable<Ypar> GetFamille(string Concept, string Famille){
                try {
                    return connection.EnsureOpened().Query<Ypar>(select_GetFamille, new {Concept, Famille}).ToList();
                } finally {
                    connection.EnsureClosed();
                }
            }
    }
}
