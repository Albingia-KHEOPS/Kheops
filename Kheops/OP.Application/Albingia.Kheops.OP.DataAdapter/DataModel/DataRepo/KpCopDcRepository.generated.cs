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

    public  partial class  KpCopDcRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KHQTYP, KHQIPB, KHQALX, KHQAVN, KHQOLDC
, KHQCODE, KHQNOMD, KHQTABLE, KHQOLDID FROM KPCOPDC
WHERE KHQTYP = :parKHQTYP
and KHQIPB = :parKHQIPB
and KHQALX = :parKHQALX
and KHQAVN = :parKHQAVN
and KHQOLDC = :parKHQOLDC
";
            const string update=@"UPDATE KPCOPDC SET 
KHQTYP = :KHQTYP, KHQIPB = :KHQIPB, KHQALX = :KHQALX, KHQAVN = :KHQAVN, KHQOLDC = :KHQOLDC, KHQCODE = :KHQCODE, KHQNOMD = :KHQNOMD, KHQTABLE = :KHQTABLE, KHQOLDID = :KHQOLDID
 WHERE 
KHQTYP = :parKHQTYP and KHQIPB = :parKHQIPB and KHQALX = :parKHQALX and KHQAVN = :parKHQAVN and KHQOLDC = :parKHQOLDC";
            const string delete=@"DELETE FROM KPCOPDC WHERE KHQTYP = :parKHQTYP AND KHQIPB = :parKHQIPB AND KHQALX = :parKHQALX AND KHQAVN = :parKHQAVN AND KHQOLDC = :parKHQOLDC";
            const string insert=@"INSERT INTO  KPCOPDC (
KHQTYP, KHQIPB, KHQALX, KHQAVN, KHQOLDC
, KHQCODE, KHQNOMD, KHQTABLE, KHQOLDID
) VALUES (
:KHQTYP, :KHQIPB, :KHQALX, :KHQAVN, :KHQOLDC
, :KHQCODE, :KHQNOMD, :KHQTABLE, :KHQOLDID)";
            const string select_GetByAffaire=@"SELECT
KHQTYP, KHQIPB, KHQALX, KHQAVN, KHQOLDC
, KHQCODE, KHQNOMD, KHQTABLE, KHQOLDID FROM KPCOPDC
WHERE KHQTYP = :KHQTYP
and KHQIPB = :KHQIPB
and KHQALX = :KHQALX
and KHQAVN = :KHQAVN
";
            #endregion

            public KpCopDcRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpCopDc Get(string parKHQTYP, string parKHQIPB, int parKHQALX, int parKHQAVN, string parKHQOLDC){
                return connection.Query<KpCopDc>(select, new {parKHQTYP, parKHQIPB, parKHQALX, parKHQAVN, parKHQOLDC}).SingleOrDefault();
            }


            public void Insert(KpCopDc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHQTYP",value.Khqtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHQIPB",value.Khqipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KHQALX",value.Khqalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KHQAVN",value.Khqavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHQOLDC",value.Khqoldc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:250, scale: 0);
                    parameters.Add("KHQCODE",value.Khqcode, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHQNOMD",value.Khqnomd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:250, scale: 0);
                    parameters.Add("KHQTABLE",value.Khqtable??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHQOLDID",value.Khqoldid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpCopDc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKHQTYP",value.Khqtyp);
                    parameters.Add("parKHQIPB",value.Khqipb);
                    parameters.Add("parKHQALX",value.Khqalx);
                    parameters.Add("parKHQAVN",value.Khqavn);
                    parameters.Add("parKHQOLDC",value.Khqoldc);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpCopDc value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHQTYP",value.Khqtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHQIPB",value.Khqipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KHQALX",value.Khqalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KHQAVN",value.Khqavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHQOLDC",value.Khqoldc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:250, scale: 0);
                    parameters.Add("KHQCODE",value.Khqcode, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHQNOMD",value.Khqnomd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:250, scale: 0);
                    parameters.Add("KHQTABLE",value.Khqtable??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHQOLDID",value.Khqoldid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("parKHQTYP",value.Khqtyp);
                    parameters.Add("parKHQIPB",value.Khqipb);
                    parameters.Add("parKHQALX",value.Khqalx);
                    parameters.Add("parKHQAVN",value.Khqavn);
                    parameters.Add("parKHQOLDC",value.Khqoldc);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpCopDc> GetByAffaire(string KHQTYP, string KHQIPB, int KHQALX, int KHQAVN){
                    return connection.EnsureOpened().Query<KpCopDc>(select_GetByAffaire, new {KHQTYP, KHQIPB, KHQALX, KHQAVN}).ToList();
            }
    }
}
