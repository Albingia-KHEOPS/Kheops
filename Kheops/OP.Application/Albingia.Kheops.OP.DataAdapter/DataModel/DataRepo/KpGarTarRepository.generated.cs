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

    public  partial class  KpGarTarRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KDGID, KDGTYP, KDGIPB, KDGALX, KDGFOR
, KDGOPT, KDGGARAN, KDGKDEID, KDGNUMTAR, KDGLCIMOD
, KDGLCIOBL, KDGLCIVALO, KDGLCIVALA, KDGLCIVALW, KDGLCIUNIT
, KDGLCIBASE, KDGKDIID, KDGFRHMOD, KDGFRHOBL, KDGFRHVALO
, KDGFRHVALA, KDGFRHVALW, KDGFRHUNIT, KDGFRHBASE, KDGKDKID
, KDGFMIVALO, KDGFMIVALA, KDGFMIVALW, KDGFMIUNIT, KDGFMIBASE
, KDGFMAVALO, KDGFMAVALA, KDGFMAVALW, KDGFMAUNIT, KDGFMABASE
, KDGPRIMOD, KDGPRIOBL, KDGPRIVALO, KDGPRIVALA, KDGPRIVALW
, KDGPRIUNIT, KDGPRIBASE, KDGMNTBASE, KDGPRIMPRO, KDGTMC
, KDGTFF, KDGCMC, KDGCHT, KDGCTT FROM KPGARTAR
WHERE KDGID = :parKDGID
";
            const string update=@"UPDATE KPGARTAR SET 
KDGID = :KDGID, KDGTYP = :KDGTYP, KDGIPB = :KDGIPB, KDGALX = :KDGALX, KDGFOR = :KDGFOR, KDGOPT = :KDGOPT, KDGGARAN = :KDGGARAN, KDGKDEID = :KDGKDEID, KDGNUMTAR = :KDGNUMTAR, KDGLCIMOD = :KDGLCIMOD
, KDGLCIOBL = :KDGLCIOBL, KDGLCIVALO = :KDGLCIVALO, KDGLCIVALA = :KDGLCIVALA, KDGLCIVALW = :KDGLCIVALW, KDGLCIUNIT = :KDGLCIUNIT, KDGLCIBASE = :KDGLCIBASE, KDGKDIID = :KDGKDIID, KDGFRHMOD = :KDGFRHMOD, KDGFRHOBL = :KDGFRHOBL, KDGFRHVALO = :KDGFRHVALO
, KDGFRHVALA = :KDGFRHVALA, KDGFRHVALW = :KDGFRHVALW, KDGFRHUNIT = :KDGFRHUNIT, KDGFRHBASE = :KDGFRHBASE, KDGKDKID = :KDGKDKID, KDGFMIVALO = :KDGFMIVALO, KDGFMIVALA = :KDGFMIVALA, KDGFMIVALW = :KDGFMIVALW, KDGFMIUNIT = :KDGFMIUNIT, KDGFMIBASE = :KDGFMIBASE
, KDGFMAVALO = :KDGFMAVALO, KDGFMAVALA = :KDGFMAVALA, KDGFMAVALW = :KDGFMAVALW, KDGFMAUNIT = :KDGFMAUNIT, KDGFMABASE = :KDGFMABASE, KDGPRIMOD = :KDGPRIMOD, KDGPRIOBL = :KDGPRIOBL, KDGPRIVALO = :KDGPRIVALO, KDGPRIVALA = :KDGPRIVALA, KDGPRIVALW = :KDGPRIVALW
, KDGPRIUNIT = :KDGPRIUNIT, KDGPRIBASE = :KDGPRIBASE, KDGMNTBASE = :KDGMNTBASE, KDGPRIMPRO = :KDGPRIMPRO, KDGTMC = :KDGTMC, KDGTFF = :KDGTFF, KDGCMC = :KDGCMC, KDGCHT = :KDGCHT, KDGCTT = :KDGCTT
 WHERE 
KDGID = :parKDGID";
            const string delete=@"DELETE FROM KPGARTAR WHERE KDGID = :parKDGID";
            const string insert=@"INSERT INTO  KPGARTAR (
KDGID, KDGTYP, KDGIPB, KDGALX, KDGFOR
, KDGOPT, KDGGARAN, KDGKDEID, KDGNUMTAR, KDGLCIMOD
, KDGLCIOBL, KDGLCIVALO, KDGLCIVALA, KDGLCIVALW, KDGLCIUNIT
, KDGLCIBASE, KDGKDIID, KDGFRHMOD, KDGFRHOBL, KDGFRHVALO
, KDGFRHVALA, KDGFRHVALW, KDGFRHUNIT, KDGFRHBASE, KDGKDKID
, KDGFMIVALO, KDGFMIVALA, KDGFMIVALW, KDGFMIUNIT, KDGFMIBASE
, KDGFMAVALO, KDGFMAVALA, KDGFMAVALW, KDGFMAUNIT, KDGFMABASE
, KDGPRIMOD, KDGPRIOBL, KDGPRIVALO, KDGPRIVALA, KDGPRIVALW
, KDGPRIUNIT, KDGPRIBASE, KDGMNTBASE, KDGPRIMPRO, KDGTMC
, KDGTFF, KDGCMC, KDGCHT, KDGCTT
) VALUES (
:KDGID, :KDGTYP, :KDGIPB, :KDGALX, :KDGFOR
, :KDGOPT, :KDGGARAN, :KDGKDEID, :KDGNUMTAR, :KDGLCIMOD
, :KDGLCIOBL, :KDGLCIVALO, :KDGLCIVALA, :KDGLCIVALW, :KDGLCIUNIT
, :KDGLCIBASE, :KDGKDIID, :KDGFRHMOD, :KDGFRHOBL, :KDGFRHVALO
, :KDGFRHVALA, :KDGFRHVALW, :KDGFRHUNIT, :KDGFRHBASE, :KDGKDKID
, :KDGFMIVALO, :KDGFMIVALA, :KDGFMIVALW, :KDGFMIUNIT, :KDGFMIBASE
, :KDGFMAVALO, :KDGFMAVALA, :KDGFMAVALW, :KDGFMAUNIT, :KDGFMABASE
, :KDGPRIMOD, :KDGPRIOBL, :KDGPRIVALO, :KDGPRIVALA, :KDGPRIVALW
, :KDGPRIUNIT, :KDGPRIBASE, :KDGMNTBASE, :KDGPRIMPRO, :KDGTMC
, :KDGTFF, :KDGCMC, :KDGCHT, :KDGCTT)";
            const string select_GetByAffaire=@"SELECT
KDGID, KDGTYP, KDGIPB, KDGALX, KDGFOR
, KDGOPT, KDGGARAN, KDGKDEID, KDGNUMTAR, KDGLCIMOD
, KDGLCIOBL, KDGLCIVALO, KDGLCIVALA, KDGLCIVALW, KDGLCIUNIT
, KDGLCIBASE, KDGKDIID, KDGFRHMOD, KDGFRHOBL, KDGFRHVALO
, KDGFRHVALA, KDGFRHVALW, KDGFRHUNIT, KDGFRHBASE, KDGKDKID
, KDGFMIVALO, KDGFMIVALA, KDGFMIVALW, KDGFMIUNIT, KDGFMIBASE
, KDGFMAVALO, KDGFMAVALA, KDGFMAVALW, KDGFMAUNIT, KDGFMABASE
, KDGPRIMOD, KDGPRIOBL, KDGPRIVALO, KDGPRIVALA, KDGPRIVALW
, KDGPRIUNIT, KDGPRIBASE, KDGMNTBASE, KDGPRIMPRO, KDGTMC
, KDGTFF, KDGCMC, KDGCHT, KDGCTT FROM KPGARTAR
WHERE KDGTYP = :typeAffaire
and KDGIPB = :numeroAffaire
and KDGALX = :version
";
            const string select_GetByFormule=@"SELECT
KDGID, KDGTYP, KDGIPB, KDGALX, KDGFOR
, KDGOPT, KDGGARAN, KDGKDEID, KDGNUMTAR, KDGLCIMOD
, KDGLCIOBL, KDGLCIVALO, KDGLCIVALA, KDGLCIVALW, KDGLCIUNIT
, KDGLCIBASE, KDGKDIID, KDGFRHMOD, KDGFRHOBL, KDGFRHVALO
, KDGFRHVALA, KDGFRHVALW, KDGFRHUNIT, KDGFRHBASE, KDGKDKID
, KDGFMIVALO, KDGFMIVALA, KDGFMIVALW, KDGFMIUNIT, KDGFMIBASE
, KDGFMAVALO, KDGFMAVALA, KDGFMAVALW, KDGFMAUNIT, KDGFMABASE
, KDGPRIMOD, KDGPRIOBL, KDGPRIVALO, KDGPRIVALA, KDGPRIVALW
, KDGPRIUNIT, KDGPRIBASE, KDGMNTBASE, KDGPRIMPRO, KDGTMC
, KDGTFF, KDGCMC, KDGCHT, KDGCTT FROM KPGARTAR
inner join KPGARAN on KDGKDEID = KDEID
inner join KPOPTD on KDEKDCID = KDCID
inner join KPOPT on KDCKDBID = KDBID
WHERE KDBKDAID = :formuleId
";
            const string select_GetByOption=@"SELECT
KDGID, KDGTYP, KDGIPB, KDGALX, KDGFOR
, KDGOPT, KDGGARAN, KDGKDEID, KDGNUMTAR, KDGLCIMOD
, KDGLCIOBL, KDGLCIVALO, KDGLCIVALA, KDGLCIVALW, KDGLCIUNIT
, KDGLCIBASE, KDGKDIID, KDGFRHMOD, KDGFRHOBL, KDGFRHVALO
, KDGFRHVALA, KDGFRHVALW, KDGFRHUNIT, KDGFRHBASE, KDGKDKID
, KDGFMIVALO, KDGFMIVALA, KDGFMIVALW, KDGFMIUNIT, KDGFMIBASE
, KDGFMAVALO, KDGFMAVALA, KDGFMAVALW, KDGFMAUNIT, KDGFMABASE
, KDGPRIMOD, KDGPRIOBL, KDGPRIVALO, KDGPRIVALA, KDGPRIVALW
, KDGPRIUNIT, KDGPRIBASE, KDGMNTBASE, KDGPRIMPRO, KDGTMC
, KDGTFF, KDGCMC, KDGCHT, KDGCTT FROM KPGARTAR
inner join KPGARAN on KDGKDEID = KDEID
inner join KPOPTD on KDEKDCID = KDCID
WHERE KDCKDBID = :optionId
";
            #endregion

            public KpGarTarRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpGarTar Get(Int64 parKDGID){
                return connection.Query<KpGarTar>(select, new {parKDGID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KDGID") ;
            }

            public void Insert(KpGarTar value){
                    if(value.Kdgid == default(Int64)) {
                        value.Kdgid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KDGID",value.Kdgid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDGTYP",value.Kdgtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGIPB",value.Kdgipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDGALX",value.Kdgalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDGFOR",value.Kdgfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDGOPT",value.Kdgopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDGGARAN",value.Kdggaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDGKDEID",value.Kdgkdeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDGNUMTAR",value.Kdgnumtar, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDGLCIMOD",value.Kdglcimod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGLCIOBL",value.Kdglciobl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGLCIVALO",value.Kdglcivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGLCIVALA",value.Kdglcivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGLCIVALW",value.Kdglcivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGLCIUNIT",value.Kdglciunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGLCIBASE",value.Kdglcibase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGKDIID",value.Kdgkdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDGFRHMOD",value.Kdgfrhmod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGFRHOBL",value.Kdgfrhobl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGFRHVALO",value.Kdgfrhvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFRHVALA",value.Kdgfrhvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFRHVALW",value.Kdgfrhvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFRHUNIT",value.Kdgfrhunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGFRHBASE",value.Kdgfrhbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGKDKID",value.Kdgkdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDGFMIVALO",value.Kdgfmivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMIVALA",value.Kdgfmivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMIVALW",value.Kdgfmivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMIUNIT",value.Kdgfmiunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGFMIBASE",value.Kdgfmibase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGFMAVALO",value.Kdgfmavalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMAVALA",value.Kdgfmavala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMAVALW",value.Kdgfmavalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMAUNIT",value.Kdgfmaunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGFMABASE",value.Kdgfmabase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGPRIMOD",value.Kdgprimod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGPRIOBL",value.Kdgpriobl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGPRIVALO",value.Kdgprivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDGPRIVALA",value.Kdgprivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDGPRIVALW",value.Kdgprivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDGPRIUNIT",value.Kdgpriunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGPRIBASE",value.Kdgpribase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGMNTBASE",value.Kdgmntbase, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGPRIMPRO",value.Kdgprimpro, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGTMC",value.Kdgtmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGTFF",value.Kdgtff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGCMC",value.Kdgcmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGCHT",value.Kdgcht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGCTT",value.Kdgctt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpGarTar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKDGID",value.Kdgid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpGarTar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KDGID",value.Kdgid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDGTYP",value.Kdgtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGIPB",value.Kdgipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KDGALX",value.Kdgalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KDGFOR",value.Kdgfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDGOPT",value.Kdgopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KDGGARAN",value.Kdggaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KDGKDEID",value.Kdgkdeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDGNUMTAR",value.Kdgnumtar, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KDGLCIMOD",value.Kdglcimod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGLCIOBL",value.Kdglciobl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGLCIVALO",value.Kdglcivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGLCIVALA",value.Kdglcivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGLCIVALW",value.Kdglcivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGLCIUNIT",value.Kdglciunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGLCIBASE",value.Kdglcibase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGKDIID",value.Kdgkdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDGFRHMOD",value.Kdgfrhmod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGFRHOBL",value.Kdgfrhobl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGFRHVALO",value.Kdgfrhvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFRHVALA",value.Kdgfrhvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFRHVALW",value.Kdgfrhvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFRHUNIT",value.Kdgfrhunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGFRHBASE",value.Kdgfrhbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGKDKID",value.Kdgkdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KDGFMIVALO",value.Kdgfmivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMIVALA",value.Kdgfmivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMIVALW",value.Kdgfmivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMIUNIT",value.Kdgfmiunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGFMIBASE",value.Kdgfmibase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGFMAVALO",value.Kdgfmavalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMAVALA",value.Kdgfmavala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMAVALW",value.Kdgfmavalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGFMAUNIT",value.Kdgfmaunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGFMABASE",value.Kdgfmabase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGPRIMOD",value.Kdgprimod??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGPRIOBL",value.Kdgpriobl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KDGPRIVALO",value.Kdgprivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDGPRIVALA",value.Kdgprivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDGPRIVALW",value.Kdgprivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:16, scale: 4);
                    parameters.Add("KDGPRIUNIT",value.Kdgpriunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGPRIBASE",value.Kdgpribase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KDGMNTBASE",value.Kdgmntbase, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGPRIMPRO",value.Kdgprimpro, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGTMC",value.Kdgtmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGTFF",value.Kdgtff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGCMC",value.Kdgcmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGCHT",value.Kdgcht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KDGCTT",value.Kdgctt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("parKDGID",value.Kdgid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpGarTar> GetByAffaire(string typeAffaire, string numeroAffaire, int version){
                    return connection.EnsureOpened().Query<KpGarTar>(select_GetByAffaire, new {typeAffaire, numeroAffaire, version}).ToList();
            }
            public IEnumerable<KpGarTar> GetByFormule(Int64 formuleId){
                    return connection.EnsureOpened().Query<KpGarTar>(select_GetByFormule, new {formuleId}).ToList();
            }
            public IEnumerable<KpGarTar> GetByOption(Int64 optionId){
                    return connection.EnsureOpened().Query<KpGarTar>(select_GetByOption, new {optionId}).ToList();
            }
    }
}
