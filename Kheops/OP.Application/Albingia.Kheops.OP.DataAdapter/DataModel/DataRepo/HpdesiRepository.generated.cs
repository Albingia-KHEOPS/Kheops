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

    public  partial class  HpdesiRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KADCHR, KADTYP, KADIPB, KADALX, KADAVN
, KADHIN, KADPERI, KADRSQ, KADOBJ, KADDESI
 FROM HPDESI
WHERE KADCHR = :idChrono
and KADAVN = :numeroAvenant
";
            const string update=@"UPDATE HPDESI SET 
KADCHR = :KADCHR, KADTYP = :KADTYP, KADIPB = :KADIPB, KADALX = :KADALX, KADAVN = :KADAVN, KADHIN = :KADHIN, KADPERI = :KADPERI, KADRSQ = :KADRSQ, KADOBJ = :KADOBJ, KADDESI = :KADDESI

 WHERE 
KADCHR = :idChrono and KADAVN = :numeroAvenant";
            const string delete=@"DELETE FROM HPDESI WHERE KADCHR = :idChrono AND KADAVN = :numeroAvenant";
            const string insert=@"INSERT INTO  HPDESI (
KADCHR, KADTYP, KADIPB, KADALX, KADAVN
, KADHIN, KADPERI, KADRSQ, KADOBJ, KADDESI

) VALUES (
:KADCHR, :KADTYP, :KADIPB, :KADALX, :KADAVN
, :KADHIN, :KADPERI, :KADRSQ, :KADOBJ, :KADDESI
)";
            const string select_GetByAffaire=@"SELECT
KADCHR, KADTYP, KADIPB, KADALX, KADAVN
, KADHIN, KADPERI, KADRSQ, KADOBJ, KADDESI
 FROM HPDESI
WHERE KADTYP = :typeAffaire
and KADIPB = :codeAffaire
and KADALX = :numeroAliment
and KADAVN = :numeroAvenant
";
            #endregion

            public HpdesiRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpDesi Get(int idChrono, int numeroAvenant){
                return connection.Query<KpDesi>(select, new {idChrono, numeroAvenant}).SingleOrDefault();
            }


            public void Insert(KpDesi value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KADCHR",value.Kadchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADTYP",value.Kadtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KADIPB",value.Kadipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KADALX",value.Kadalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADAVN",value.Kadavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADHIN",value.Kadhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADPERI",value.Kadperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KADRSQ",value.Kadrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADOBJ",value.Kadobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADDESI",value.Kaddesi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpDesi value){
                    var parameters = new DynamicParameters();
                    parameters.Add("idChrono",value.Kadchr);
                    parameters.Add("numeroAvenant",value.Kadavn);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpDesi value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KADCHR",value.Kadchr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADTYP",value.Kadtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KADIPB",value.Kadipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KADALX",value.Kadalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADAVN",value.Kadavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADHIN",value.Kadhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADPERI",value.Kadperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KADRSQ",value.Kadrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADOBJ",value.Kadobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KADDESI",value.Kaddesi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5000, scale: 0);
                    parameters.Add("idChrono",value.Kadchr);
                    parameters.Add("numeroAvenant",value.Kadavn);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpDesi> GetByAffaire(string typeAffaire, string codeAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpDesi>(select_GetByAffaire, new {typeAffaire, codeAffaire, numeroAliment, numeroAvenant}).ToList();
            }
    }
}
