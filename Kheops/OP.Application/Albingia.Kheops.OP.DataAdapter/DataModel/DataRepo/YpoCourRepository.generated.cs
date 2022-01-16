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

    public  partial class  YpoCourRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PFTYP, PFIPB, PFALX, PFCTI, PFICT
, PFSAA, PFSAM, PFSAJ, PFSAH, PFSIT
, PFSTA, PFSTM, PFSTJ, PFGES, PFSOU
, PFCOM, PFOCT, PFXCM, PFXCN FROM YPOCOUR
WHERE PFTYP = :PFTYP
and PFIPB = :PFIPB
and PFALX = :PFALX
and PFICT = :PFICT
";
            const string update=@"UPDATE YPOCOUR SET 
PFTYP = :PFTYP, PFIPB = :PFIPB, PFALX = :PFALX, PFCTI = :PFCTI, PFICT = :PFICT, PFSAA = :PFSAA, PFSAM = :PFSAM, PFSAJ = :PFSAJ, PFSAH = :PFSAH, PFSIT = :PFSIT
, PFSTA = :PFSTA, PFSTM = :PFSTM, PFSTJ = :PFSTJ, PFGES = :PFGES, PFSOU = :PFSOU, PFCOM = :PFCOM, PFOCT = :PFOCT, PFXCM = :PFXCM, PFXCN = :PFXCN
 WHERE 
PFTYP = :PFTYP and PFIPB = :PFIPB and PFALX = :PFALX and PFICT = :PFICT";
            const string delete=@"DELETE FROM YPOCOUR WHERE PFTYP = :PFTYP AND PFIPB = :PFIPB AND PFALX = :PFALX AND PFICT = :PFICT";
            const string insert=@"INSERT INTO  YPOCOUR (
PFTYP, PFIPB, PFALX, PFCTI, PFICT
, PFSAA, PFSAM, PFSAJ, PFSAH, PFSIT
, PFSTA, PFSTM, PFSTJ, PFGES, PFSOU
, PFCOM, PFOCT, PFXCM, PFXCN
) VALUES (
:PFTYP, :PFIPB, :PFALX, :PFCTI, :PFICT
, :PFSAA, :PFSAM, :PFSAJ, :PFSAH, :PFSIT
, :PFSTA, :PFSTM, :PFSTJ, :PFGES, :PFSOU
, :PFCOM, :PFOCT, :PFXCM, :PFXCN)";
            const string select_GetByAffaire=@"SELECT
PFTYP, PFIPB, PFALX, PFCTI, PFICT
, PFSAA, PFSAM, PFSAJ, PFSAH, PFSIT
, PFSTA, PFSTM, PFSTJ, PFGES, PFSOU
, PFCOM, PFOCT, PFXCM, PFXCN FROM YPOCOUR
WHERE PFTYP = :PFTYP
and PFIPB = :PFIPB
and PFALX = :PFALX
";
            #endregion

            public YpoCourRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YpoCour Get(string PFTYP, string PFIPB, int PFALX, int PFICT){
                return connection.Query<YpoCour>(select, new {PFTYP, PFIPB, PFALX, PFICT}).SingleOrDefault();
            }


            public void Insert(YpoCour value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PFTYP",value.Pftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PFIPB",value.Pfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PFALX",value.Pfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PFCTI",value.Pfcti??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PFICT",value.Pfict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PFSAA",value.Pfsaa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PFSAM",value.Pfsam, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PFSAJ",value.Pfsaj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PFSAH",value.Pfsah, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PFSIT",value.Pfsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PFSTA",value.Pfsta, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PFSTM",value.Pfstm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PFSTJ",value.Pfstj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PFGES",value.Pfges??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PFSOU",value.Pfsou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PFCOM",value.Pfcom, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PFOCT",value.Pfoct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("PFXCM",value.Pfxcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("PFXCN",value.Pfxcn, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YpoCour value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PFTYP",value.Pftyp);
                    parameters.Add("PFIPB",value.Pfipb);
                    parameters.Add("PFALX",value.Pfalx);
                    parameters.Add("PFICT",value.Pfict);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YpoCour value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PFTYP",value.Pftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PFIPB",value.Pfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PFALX",value.Pfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PFCTI",value.Pfcti??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PFICT",value.Pfict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PFSAA",value.Pfsaa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PFSAM",value.Pfsam, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PFSAJ",value.Pfsaj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PFSAH",value.Pfsah, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PFSIT",value.Pfsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PFSTA",value.Pfsta, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PFSTM",value.Pfstm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PFSTJ",value.Pfstj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PFGES",value.Pfges??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PFSOU",value.Pfsou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PFCOM",value.Pfcom, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PFOCT",value.Pfoct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("PFXCM",value.Pfxcm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("PFXCN",value.Pfxcn, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("PFTYP",value.Pftyp);
                    parameters.Add("PFIPB",value.Pfipb);
                    parameters.Add("PFALX",value.Pfalx);
                    parameters.Add("PFICT",value.Pfict);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YpoCour> GetByAffaire(string PFTYP, string PFIPB, int PFALX){
                    return connection.EnsureOpened().Query<YpoCour>(select_GetByAffaire, new {PFTYP, PFIPB, PFALX}).ToList();
            }
    }
}
