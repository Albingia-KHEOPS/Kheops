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

    public  partial class  YhpcourRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PFIPB, PFALX, PFAVN, PFHIN, PFCTI
, PFICT, PFSAA, PFSAM, PFSAJ, PFSAH
, PFSIT, PFSTA, PFSTM, PFSTJ, PFGES
, PFSOU, PFCOM, PFOCT, PFXCM, PFXCN
, PFTYP FROM YHPCOUR
WHERE PFIPB = :PFIPB
and PFALX = :PFALX
and PFAVN = :PFAVN
and PFHIN = :PFHIN
and PFICT = :PFICT
";
            const string update=@"UPDATE YHPCOUR SET 
PFIPB = :PFIPB, PFALX = :PFALX, PFAVN = :PFAVN, PFHIN = :PFHIN, PFCTI = :PFCTI, PFICT = :PFICT, PFSAA = :PFSAA, PFSAM = :PFSAM, PFSAJ = :PFSAJ, PFSAH = :PFSAH
, PFSIT = :PFSIT, PFSTA = :PFSTA, PFSTM = :PFSTM, PFSTJ = :PFSTJ, PFGES = :PFGES, PFSOU = :PFSOU, PFCOM = :PFCOM, PFOCT = :PFOCT, PFXCM = :PFXCM, PFXCN = :PFXCN
, PFTYP = :PFTYP
 WHERE 
PFIPB = :PFIPB and PFALX = :PFALX and PFAVN = :PFAVN and PFHIN = :PFHIN and PFICT = :PFICT";
            const string delete=@"DELETE FROM YHPCOUR WHERE PFIPB = :PFIPB AND PFALX = :PFALX AND PFAVN = :PFAVN AND PFHIN = :PFHIN AND PFICT = :PFICT";
            const string insert=@"INSERT INTO  YHPCOUR (
PFIPB, PFALX, PFAVN, PFHIN, PFCTI
, PFICT, PFSAA, PFSAM, PFSAJ, PFSAH
, PFSIT, PFSTA, PFSTM, PFSTJ, PFGES
, PFSOU, PFCOM, PFOCT, PFXCM, PFXCN
, PFTYP
) VALUES (
:PFIPB, :PFALX, :PFAVN, :PFHIN, :PFCTI
, :PFICT, :PFSAA, :PFSAM, :PFSAJ, :PFSAH
, :PFSIT, :PFSTA, :PFSTM, :PFSTJ, :PFGES
, :PFSOU, :PFCOM, :PFOCT, :PFXCM, :PFXCN
, :PFTYP)";
            const string select_GetByAffaire=@"SELECT
PFIPB, PFALX, PFAVN, PFHIN, PFCTI
, PFICT, PFSAA, PFSAM, PFSAJ, PFSAH
, PFSIT, PFSTA, PFSTM, PFSTJ, PFGES
, PFSOU, PFCOM, PFOCT, PFXCM, PFXCN
, PFTYP FROM YHPCOUR
WHERE PFIPB = :PFIPB
and PFALX = :PFALX
and PFAVN = :PFAVN
and PFTYP = :PFTYP
";
            #endregion

            public YhpcourRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YpoCour Get(string PFIPB, int PFALX, int PFAVN, int PFHIN, int PFICT){
                return connection.Query<YpoCour>(select, new {PFIPB, PFALX, PFAVN, PFHIN, PFICT}).SingleOrDefault();
            }


            public void Insert(YpoCour value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PFIPB",value.Pfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PFALX",value.Pfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PFAVN",value.Pfavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PFHIN",value.Pfhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("PFTYP",value.Pftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YpoCour value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PFIPB",value.Pfipb);
                    parameters.Add("PFALX",value.Pfalx);
                    parameters.Add("PFAVN",value.Pfavn);
                    parameters.Add("PFHIN",value.Pfhin);
                    parameters.Add("PFICT",value.Pfict);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YpoCour value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PFIPB",value.Pfipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PFALX",value.Pfalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PFAVN",value.Pfavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PFHIN",value.Pfhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
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
                    parameters.Add("PFTYP",value.Pftyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PFIPB",value.Pfipb);
                    parameters.Add("PFALX",value.Pfalx);
                    parameters.Add("PFAVN",value.Pfavn);
                    parameters.Add("PFHIN",value.Pfhin);
                    parameters.Add("PFICT",value.Pfict);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YpoCour> GetByAffaire(string PFIPB, int PFALX, int PFAVN, string PFTYP){
                    return connection.EnsureOpened().Query<YpoCour>(select_GetByAffaire, new {PFIPB, PFALX, PFAVN, PFTYP}).ToList();
            }
    }
}
