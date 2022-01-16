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

    public  partial class  KpDesiRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KADCHR, KADTYP, KADIPB, KADALX, KADPERI
, KADRSQ, KADOBJ, KADDESI FROM KPDESI
WHERE KADCHR = :idChrono
";
            const string update=@"UPDATE KPDESI SET 
KADCHR = :KADCHR, KADTYP = :KADTYP, KADIPB = :KADIPB, KADALX = :KADALX, KADPERI = :KADPERI, KADRSQ = :KADRSQ, KADOBJ = :KADOBJ, KADDESI = :KADDESI
 WHERE 
KADCHR = :idChrono";
            const string delete=@"DELETE FROM KPDESI WHERE KADCHR = :idChrono";
            const string insert=@"INSERT INTO  KPDESI (
KADCHR, KADTYP, KADIPB, KADALX, KADPERI
, KADRSQ, KADOBJ, KADDESI
) VALUES (
:KADCHR, :KADTYP, :KADIPB, :KADALX, :KADPERI
, :KADRSQ, :KADOBJ, :KADDESI)";
            const string select_GetByAffaire=@"SELECT
KADCHR, KADTYP, KADIPB, KADALX, KADPERI
, KADRSQ, KADOBJ, KADDESI FROM KPDESI
WHERE KADTYP = :parKADTYP
and KADIPB = :parKADIPB
and KADALX = :parKADALX
";
            #endregion

            public KpDesiRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpDesi Get(int idChrono){
                return connection.Query<KpDesi>(select, new {idChrono}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KADCHR") ;
            }

            public void Insert(KpDesi value){
                    if(value.Kadchr == default(int)) {
                        value.Kadchr = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KADCHR",value.Kadchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADTYP",value.Kadtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KADIPB",value.Kadipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KADALX",value.Kadalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADPERI",value.Kadperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KADRSQ",value.Kadrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADOBJ",value.Kadobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADDESI",value.Kaddesi, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpDesi value){
                    var parameters = new DynamicParameters();
                    parameters.Add("idChrono",value.Kadchr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpDesi value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KADCHR",value.Kadchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADTYP",value.Kadtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KADIPB",value.Kadipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KADALX",value.Kadalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADPERI",value.Kadperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KADRSQ",value.Kadrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADOBJ",value.Kadobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADDESI",value.Kaddesi, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("idChrono",value.Kadchr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpDesi> GetByAffaire(string parKADTYP, string parKADIPB, int parKADALX){
                    return connection.EnsureOpened().Query<KpDesi>(select_GetByAffaire, new {parKADTYP, parKADIPB, parKADALX}).ToList();
            }
    }
}
