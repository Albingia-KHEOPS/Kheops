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

    public  partial class  YprtPerRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KAIPB, KAALX, KARSQ, KAFOR, KATYP
, KADPA, KADPM, KADPJ, KAFPA, KAFPM
, KAPFJ, KATPE, KAIVA, KAVAA, KACOP
 FROM YPRTPER
WHERE KAIPB = :KAIPB
and KAALX = :KAALX
and KARSQ = :KARSQ
and KAFOR = :KAFOR
and KATYP = :KATYP
and KADPA = :KADPA
and KADPM = :KADPM
and KADPJ = :KADPJ
";
            const string update=@"UPDATE YPRTPER SET 
KAIPB = :KAIPB, KAALX = :KAALX, KARSQ = :KARSQ, KAFOR = :KAFOR, KATYP = :KATYP, KADPA = :KADPA, KADPM = :KADPM, KADPJ = :KADPJ, KAFPA = :KAFPA, KAFPM = :KAFPM
, KAPFJ = :KAPFJ, KATPE = :KATPE, KAIVA = :KAIVA, KAVAA = :KAVAA, KACOP = :KACOP
 WHERE 
KAIPB = :KAIPB and KAALX = :KAALX and KARSQ = :KARSQ and KAFOR = :KAFOR and KATYP = :KATYP and KADPA = :KADPA and KADPM = :KADPM and KADPJ = :KADPJ";
            const string delete=@"DELETE FROM YPRTPER WHERE KAIPB = :KAIPB AND KAALX = :KAALX AND KARSQ = :KARSQ AND KAFOR = :KAFOR AND KATYP = :KATYP AND KADPA = :KADPA AND KADPM = :KADPM AND KADPJ = :KADPJ";
            const string insert=@"INSERT INTO  YPRTPER (
KAIPB, KAALX, KARSQ, KAFOR, KATYP
, KADPA, KADPM, KADPJ, KAFPA, KAFPM
, KAPFJ, KATPE, KAIVA, KAVAA, KACOP

) VALUES (
:KAIPB, :KAALX, :KARSQ, :KAFOR, :KATYP
, :KADPA, :KADPM, :KADPJ, :KAFPA, :KAFPM
, :KAPFJ, :KATPE, :KAIVA, :KAVAA, :KACOP
)";
            const string select_GetByAffaire=@"SELECT
KAIPB, KAALX, KARSQ, KAFOR, KATYP
, KADPA, KADPM, KADPJ, KAFPA, KAFPM
, KAPFJ, KATPE, KAIVA, KAVAA, KACOP
 FROM YPRTPER
WHERE KAIPB = :KAIPB
and KAALX = :KAALX
and KATYP = :KATYP
";
            #endregion

            public YprtPerRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YprtPer Get(string KAIPB, int KAALX, int KARSQ, int KAFOR, int KATYP, int KADPA, int KADPM, int KADPJ){
                return connection.Query<YprtPer>(select, new {KAIPB, KAALX, KARSQ, KAFOR, KATYP, KADPA, KADPM, KADPJ}).SingleOrDefault();
            }


            public void Insert(YprtPer value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAIPB",value.Kaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAALX",value.Kaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KARSQ",value.Karsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAFOR",value.Kafor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KATYP",value.Katyp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KADPA",value.Kadpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADPM",value.Kadpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KADPJ",value.Kadpj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAFPA",value.Kafpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAFPM",value.Kafpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAPFJ",value.Kapfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KATPE",value.Katpe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAIVA",value.Kaiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAVAA",value.Kavaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KACOP",value.Kacop??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YprtPer value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAIPB",value.Kaipb);
                    parameters.Add("KAALX",value.Kaalx);
                    parameters.Add("KARSQ",value.Karsq);
                    parameters.Add("KAFOR",value.Kafor);
                    parameters.Add("KATYP",value.Katyp);
                    parameters.Add("KADPA",value.Kadpa);
                    parameters.Add("KADPM",value.Kadpm);
                    parameters.Add("KADPJ",value.Kadpj);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YprtPer value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KAIPB",value.Kaipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KAALX",value.Kaalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KARSQ",value.Karsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KAFOR",value.Kafor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KATYP",value.Katyp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KADPA",value.Kadpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADPM",value.Kadpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KADPJ",value.Kadpj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAFPA",value.Kafpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KAFPM",value.Kafpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KAPFJ",value.Kapfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KATPE",value.Katpe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAIVA",value.Kaiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KAVAA",value.Kavaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KACOP",value.Kacop??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KAIPB",value.Kaipb);
                    parameters.Add("KAALX",value.Kaalx);
                    parameters.Add("KARSQ",value.Karsq);
                    parameters.Add("KAFOR",value.Kafor);
                    parameters.Add("KATYP",value.Katyp);
                    parameters.Add("KADPA",value.Kadpa);
                    parameters.Add("KADPM",value.Kadpm);
                    parameters.Add("KADPJ",value.Kadpj);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YprtPer> GetByAffaire(string KAIPB, int KAALX, int KATYP){
                    return connection.EnsureOpened().Query<YprtPer>(select_GetByAffaire, new {KAIPB, KAALX, KATYP}).ToList();
            }
    }
}
