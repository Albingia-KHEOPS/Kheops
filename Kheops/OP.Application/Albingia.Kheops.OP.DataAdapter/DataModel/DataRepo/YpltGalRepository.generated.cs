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

    public  partial class  YpltGalRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKMOD
            const string select=@"SELECT
C4SEQ, C4TYP, C4BAS, C4VAL, C4UNT
, C4MAJ, C4OBL, C4ALA FROM YPLTGAL
WHERE C4SEQ = :seq
and C4TYP = :type
";
            const string update=@"UPDATE YPLTGAL SET 
C4SEQ = :C4SEQ, C4TYP = :C4TYP, C4BAS = :C4BAS, C4VAL = :C4VAL, C4UNT = :C4UNT, C4MAJ = :C4MAJ, C4OBL = :C4OBL, C4ALA = :C4ALA
 WHERE 
C4SEQ = :seq and C4TYP = :type";
            const string delete=@"DELETE FROM YPLTGAL WHERE C4SEQ = :seq AND C4TYP = :type";
            const string insert=@"INSERT INTO  YPLTGAL (
C4SEQ, C4TYP, C4BAS, C4VAL, C4UNT
, C4MAJ, C4OBL, C4ALA
) VALUES (
:C4SEQ, :C4TYP, :C4BAS, :C4VAL, :C4UNT
, :C4MAJ, :C4OBL, :C4ALA)";
            const string select_GetAll=@"SELECT
C4SEQ, C4TYP, C4BAS, C4VAL, C4UNT
, C4MAJ, C4OBL, C4ALA FROM YPLTGAL
";
            #endregion

            public YpltGalRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YpltGal Get(Int64 seq, string type){
                return connection.Query<YpltGal>(select, new {seq, type}).SingleOrDefault();
            }


            public void Insert(YpltGal value){
                    var parameters = new DynamicParameters();
                    parameters.Add("C4SEQ",value.C4seq, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("C4TYP",value.C4typ??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("C4BAS",value.C4bas??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("C4VAL",value.C4val, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 4);
                    parameters.Add("C4UNT",value.C4unt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("C4MAJ",value.C4maj??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("C4OBL",value.C4obl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("C4ALA",value.C4ala??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YpltGal value){
                    var parameters = new DynamicParameters();
                    parameters.Add("seq",value.C4seq);
                    parameters.Add("type",value.C4typ);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YpltGal value){
                    var parameters = new DynamicParameters();
                    parameters.Add("C4SEQ",value.C4seq, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("C4TYP",value.C4typ??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("C4BAS",value.C4bas??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("C4VAL",value.C4val, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 4);
                    parameters.Add("C4UNT",value.C4unt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("C4MAJ",value.C4maj??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("C4OBL",value.C4obl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("C4ALA",value.C4ala??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("seq",value.C4seq);
                    parameters.Add("type",value.C4typ);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YpltGal> GetAll(){
                    return connection.EnsureOpened().Query<YpltGal>(select_GetAll).ToList();
            }
    }
}
