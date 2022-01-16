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

    public  partial class  HpioptRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KFCID, KFCTYP, KFCIPB, KFCALX, KFCAVN
, KFCHIN, KFCFOR, KFCOPT, KFCKDBID, KFCRRCR
, KFCRRC, KFCMNTE, KFCSEUI, KFCSEUR, KFCSEUC
, KFCPERR, KFCAUTM, KFCCRD, KFCCRH, KFCMAJU
, KFCMAJD, KFCMAJH FROM HPIOPT
WHERE KFCID = :KFCID
and KFCAVN = :KFCAVN
and KFCHIN = :KFCHIN
";
            const string update=@"UPDATE HPIOPT SET 
KFCID = :KFCID, KFCTYP = :KFCTYP, KFCIPB = :KFCIPB, KFCALX = :KFCALX, KFCAVN = :KFCAVN, KFCHIN = :KFCHIN, KFCFOR = :KFCFOR, KFCOPT = :KFCOPT, KFCKDBID = :KFCKDBID, KFCRRCR = :KFCRRCR
, KFCRRC = :KFCRRC, KFCMNTE = :KFCMNTE, KFCSEUI = :KFCSEUI, KFCSEUR = :KFCSEUR, KFCSEUC = :KFCSEUC, KFCPERR = :KFCPERR, KFCAUTM = :KFCAUTM, KFCCRD = :KFCCRD, KFCCRH = :KFCCRH, KFCMAJU = :KFCMAJU
, KFCMAJD = :KFCMAJD, KFCMAJH = :KFCMAJH
 WHERE 
KFCID = :KFCID and KFCAVN = :KFCAVN and KFCHIN = :KFCHIN";
            const string delete=@"DELETE FROM HPIOPT WHERE KFCID = :KFCID AND KFCAVN = :KFCAVN AND KFCHIN = :KFCHIN";
            const string insert=@"INSERT INTO  HPIOPT (
KFCID, KFCTYP, KFCIPB, KFCALX, KFCAVN
, KFCHIN, KFCFOR, KFCOPT, KFCKDBID, KFCRRCR
, KFCRRC, KFCMNTE, KFCSEUI, KFCSEUR, KFCSEUC
, KFCPERR, KFCAUTM, KFCCRD, KFCCRH, KFCMAJU
, KFCMAJD, KFCMAJH
) VALUES (
:KFCID, :KFCTYP, :KFCIPB, :KFCALX, :KFCAVN
, :KFCHIN, :KFCFOR, :KFCOPT, :KFCKDBID, :KFCRRCR
, :KFCRRC, :KFCMNTE, :KFCSEUI, :KFCSEUR, :KFCSEUC
, :KFCPERR, :KFCAUTM, :KFCCRD, :KFCCRH, :KFCMAJU
, :KFCMAJD, :KFCMAJH)";
            const string select_GetByAffaire=@"SELECT
KFCID, KFCTYP, KFCIPB, KFCALX, KFCAVN
, KFCHIN, KFCFOR, KFCOPT, KFCKDBID, KFCRRCR
, KFCRRC, KFCMNTE, KFCSEUI, KFCSEUR, KFCSEUC
, KFCPERR, KFCAUTM, KFCCRD, KFCCRH, KFCMAJU
, KFCMAJD, KFCMAJH FROM HPIOPT
WHERE KFCTYP = :KFCTYP
and KFCIPB = :KFCIPB
and KFCALX = :KFCALX
and KFCAVN = :KFCAVN
";
            #endregion

            public HpioptRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpIopt Get(Int64 KFCID, int KFCAVN, int KFCHIN){
                return connection.Query<KpIopt>(select, new {KFCID, KFCAVN, KFCHIN}).SingleOrDefault();
            }


            public void Insert(KpIopt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFCID",value.Kfcid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFCTYP",value.Kfctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFCIPB",value.Kfcipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFCALX",value.Kfcalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFCAVN",value.Kfcavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFCHIN",value.Kfchin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFCFOR",value.Kfcfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFCOPT",value.Kfcopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFCKDBID",value.Kfckdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFCRRCR",value.Kfcrrcr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFCRRC",value.Kfcrrc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFCMNTE",value.Kfcmnte, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KFCSEUI",value.Kfcseui, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFCSEUR",value.Kfcseur, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFCSEUC",value.Kfcseuc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFCPERR",value.Kfcperr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFCAUTM",value.Kfcautm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KFCCRD",value.Kfccrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFCCRH",value.Kfccrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFCMAJU",value.Kfcmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFCMAJD",value.Kfcmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFCMAJH",value.Kfcmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpIopt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFCID",value.Kfcid);
                    parameters.Add("KFCAVN",value.Kfcavn);
                    parameters.Add("KFCHIN",value.Kfchin);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpIopt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFCID",value.Kfcid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFCTYP",value.Kfctyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFCIPB",value.Kfcipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFCALX",value.Kfcalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFCAVN",value.Kfcavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFCHIN",value.Kfchin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFCFOR",value.Kfcfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFCOPT",value.Kfcopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFCKDBID",value.Kfckdbid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFCRRCR",value.Kfcrrcr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFCRRC",value.Kfcrrc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFCMNTE",value.Kfcmnte, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KFCSEUI",value.Kfcseui, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFCSEUR",value.Kfcseur, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFCSEUC",value.Kfcseuc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFCPERR",value.Kfcperr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFCAUTM",value.Kfcautm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("KFCCRD",value.Kfccrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFCCRH",value.Kfccrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFCMAJU",value.Kfcmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFCMAJD",value.Kfcmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFCMAJH",value.Kfcmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFCID",value.Kfcid);
                    parameters.Add("KFCAVN",value.Kfcavn);
                    parameters.Add("KFCHIN",value.Kfchin);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpIopt> GetByAffaire(string KFCTYP, string KFCIPB, int KFCALX, int KFCAVN){
                    return connection.EnsureOpened().Query<KpIopt>(select_GetByAffaire, new {KFCTYP, KFCIPB, KFCALX, KFCAVN}).ToList();
            }
    }
}
