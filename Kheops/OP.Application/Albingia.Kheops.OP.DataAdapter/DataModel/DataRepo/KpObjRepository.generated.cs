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

    public  partial class  KpObjRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KACTYP, KACIPB, KACALX, KACRSQ, KACOBJ
, KACCIBLE, KACINVEN, KACDESC, KACDESI, KACOBSV
, KACAPE, KACTRE, KACCLASS, KACNMC01, KACNMC02
, KACNMC03, KACNMC04, KACNMC05, KACMAND, KACMANF
, KACDSPP, KACLCIVALO, KACLCIVALA, KACLCIVALW, KACLCIUNIT
, KACLCIBASE, KACKDIID, KACFRHVALO, KACFRHVALA, KACFRHVALW
, KACFRHUNIT, KACFRHBASE, KACKDKID, KACNSIR, KACMANDH
, KACMANFH, KACSURF, KACVMC, KACPROL, KACPBI
, KACBRNT, KACBRNC FROM KPOBJ
WHERE KACTYP = :typeAffaire
and KACIPB = :codeAffaire
and KACALX = :numeroAliment
and KACRSQ = :numeroRisque
and KACOBJ = :numeroObject
";
            const string update=@"UPDATE KPOBJ SET 
KACTYP = :KACTYP, KACIPB = :KACIPB, KACALX = :KACALX, KACRSQ = :KACRSQ, KACOBJ = :KACOBJ, KACCIBLE = :KACCIBLE, KACINVEN = :KACINVEN, KACDESC = :KACDESC, KACDESI = :KACDESI, KACOBSV = :KACOBSV
, KACAPE = :KACAPE, KACTRE = :KACTRE, KACCLASS = :KACCLASS, KACNMC01 = :KACNMC01, KACNMC02 = :KACNMC02, KACNMC03 = :KACNMC03, KACNMC04 = :KACNMC04, KACNMC05 = :KACNMC05, KACMAND = :KACMAND, KACMANF = :KACMANF
, KACDSPP = :KACDSPP, KACLCIVALO = :KACLCIVALO, KACLCIVALA = :KACLCIVALA, KACLCIVALW = :KACLCIVALW, KACLCIUNIT = :KACLCIUNIT, KACLCIBASE = :KACLCIBASE, KACKDIID = :KACKDIID, KACFRHVALO = :KACFRHVALO, KACFRHVALA = :KACFRHVALA, KACFRHVALW = :KACFRHVALW
, KACFRHUNIT = :KACFRHUNIT, KACFRHBASE = :KACFRHBASE, KACKDKID = :KACKDKID, KACNSIR = :KACNSIR, KACMANDH = :KACMANDH, KACMANFH = :KACMANFH, KACSURF = :KACSURF, KACVMC = :KACVMC, KACPROL = :KACPROL, KACPBI = :KACPBI
, KACBRNT = :KACBRNT, KACBRNC = :KACBRNC
 WHERE 
KACTYP = :typeAffaire and KACIPB = :codeAffaire and KACALX = :numeroAliment and KACRSQ = :numeroRisque and KACOBJ = :numeroObject";
            const string delete=@"DELETE FROM KPOBJ WHERE KACTYP = :typeAffaire AND KACIPB = :codeAffaire AND KACALX = :numeroAliment AND KACRSQ = :numeroRisque AND KACOBJ = :numeroObject";
            const string insert=@"INSERT INTO  KPOBJ (
KACTYP, KACIPB, KACALX, KACRSQ, KACOBJ
, KACCIBLE, KACINVEN, KACDESC, KACDESI, KACOBSV
, KACAPE, KACTRE, KACCLASS, KACNMC01, KACNMC02
, KACNMC03, KACNMC04, KACNMC05, KACMAND, KACMANF
, KACDSPP, KACLCIVALO, KACLCIVALA, KACLCIVALW, KACLCIUNIT
, KACLCIBASE, KACKDIID, KACFRHVALO, KACFRHVALA, KACFRHVALW
, KACFRHUNIT, KACFRHBASE, KACKDKID, KACNSIR, KACMANDH
, KACMANFH, KACSURF, KACVMC, KACPROL, KACPBI
, KACBRNT, KACBRNC
) VALUES (
:KACTYP, :KACIPB, :KACALX, :KACRSQ, :KACOBJ
, :KACCIBLE, :KACINVEN, :KACDESC, :KACDESI, :KACOBSV
, :KACAPE, :KACTRE, :KACCLASS, :KACNMC01, :KACNMC02
, :KACNMC03, :KACNMC04, :KACNMC05, :KACMAND, :KACMANF
, :KACDSPP, :KACLCIVALO, :KACLCIVALA, :KACLCIVALW, :KACLCIUNIT
, :KACLCIBASE, :KACKDIID, :KACFRHVALO, :KACFRHVALA, :KACFRHVALW
, :KACFRHUNIT, :KACFRHBASE, :KACKDKID, :KACNSIR, :KACMANDH
, :KACMANFH, :KACSURF, :KACVMC, :KACPROL, :KACPBI
, :KACBRNT, :KACBRNC)";
            const string select_GetByAffaire=@"SELECT
KACTYP, KACIPB, KACALX, KACRSQ, KACOBJ
, KACCIBLE, KACINVEN, KACDESC, KACDESI, KACOBSV
, KACAPE, KACTRE, KACCLASS, KACNMC01, KACNMC02
, KACNMC03, KACNMC04, KACNMC05, KACMAND, KACMANF
, KACDSPP, KACLCIVALO, KACLCIVALA, KACLCIVALW, KACLCIUNIT
, KACLCIBASE, KACKDIID, KACFRHVALO, KACFRHVALA, KACFRHVALW
, KACFRHUNIT, KACFRHBASE, KACKDKID, KACNSIR, KACMANDH
, KACMANFH, KACSURF, KACVMC, KACPROL, KACPBI
, KACBRNT, KACBRNC FROM KPOBJ
WHERE KACTYP = :typeAffaire
and KACIPB = :codeAffaire
and KACALX = :numeroAliment
";
            #endregion

            public KpObjRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpObj Get(string typeAffaire, string codeAffaire, int numeroAliment, int numeroRisque, int numeroObject){
                return connection.Query<KpObj>(select, new {typeAffaire, codeAffaire, numeroAliment, numeroRisque, numeroObject}).SingleOrDefault();
            }


            public void Insert(KpObj value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KACTYP",value.Kactyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KACIPB",value.Kacipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KACALX",value.Kacalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KACRSQ",value.Kacrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KACOBJ",value.Kacobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KACCIBLE",value.Kaccible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KACINVEN",value.Kacinven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACDESC",value.Kacdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KACDESI",value.Kacdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACOBSV",value.Kacobsv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACAPE",value.Kacape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KACTRE",value.Kactre??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KACCLASS",value.Kacclass??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KACNMC01",value.Kacnmc01??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACNMC02",value.Kacnmc02??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACNMC03",value.Kacnmc03??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACNMC04",value.Kacnmc04??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACNMC05",value.Kacnmc05??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACMAND",value.Kacmand, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KACMANF",value.Kacmanf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KACDSPP",value.Kacdspp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACLCIVALO",value.Kaclcivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACLCIVALA",value.Kaclcivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACLCIVALW",value.Kaclcivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACLCIUNIT",value.Kaclciunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KACLCIBASE",value.Kaclcibase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KACKDIID",value.Kackdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACFRHVALO",value.Kacfrhvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACFRHVALA",value.Kacfrhvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACFRHVALW",value.Kacfrhvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACFRHUNIT",value.Kacfrhunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KACFRHBASE",value.Kacfrhbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KACKDKID",value.Kackdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACNSIR",value.Kacnsir??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:14, scale: 0);
                    parameters.Add("KACMANDH",value.Kacmandh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KACMANFH",value.Kacmanfh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KACSURF",value.Kacsurf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KACVMC",value.Kacvmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KACPROL",value.Kacprol??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KACPBI",value.Kacpbi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KACBRNT",value.Kacbrnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KACBRNC",value.Kacbrnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpObj value){
                    var parameters = new DynamicParameters();
                    parameters.Add("typeAffaire",value.Kactyp);
                    parameters.Add("codeAffaire",value.Kacipb);
                    parameters.Add("numeroAliment",value.Kacalx);
                    parameters.Add("numeroRisque",value.Kacrsq);
                    parameters.Add("numeroObject",value.Kacobj);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpObj value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KACTYP",value.Kactyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KACIPB",value.Kacipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KACALX",value.Kacalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KACRSQ",value.Kacrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KACOBJ",value.Kacobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KACCIBLE",value.Kaccible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KACINVEN",value.Kacinven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACDESC",value.Kacdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KACDESI",value.Kacdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACOBSV",value.Kacobsv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACAPE",value.Kacape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KACTRE",value.Kactre??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KACCLASS",value.Kacclass??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KACNMC01",value.Kacnmc01??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACNMC02",value.Kacnmc02??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACNMC03",value.Kacnmc03??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACNMC04",value.Kacnmc04??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACNMC05",value.Kacnmc05??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACMAND",value.Kacmand, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KACMANF",value.Kacmanf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KACDSPP",value.Kacdspp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACLCIVALO",value.Kaclcivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACLCIVALA",value.Kaclcivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACLCIVALW",value.Kaclcivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACLCIUNIT",value.Kaclciunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KACLCIBASE",value.Kaclcibase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KACKDIID",value.Kackdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACFRHVALO",value.Kacfrhvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACFRHVALA",value.Kacfrhvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACFRHVALW",value.Kacfrhvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KACFRHUNIT",value.Kacfrhunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KACFRHBASE",value.Kacfrhbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KACKDKID",value.Kackdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KACNSIR",value.Kacnsir??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:14, scale: 0);
                    parameters.Add("KACMANDH",value.Kacmandh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KACMANFH",value.Kacmanfh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KACSURF",value.Kacsurf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KACVMC",value.Kacvmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KACPROL",value.Kacprol??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KACPBI",value.Kacpbi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KACBRNT",value.Kacbrnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KACBRNC",value.Kacbrnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("typeAffaire",value.Kactyp);
                    parameters.Add("codeAffaire",value.Kacipb);
                    parameters.Add("numeroAliment",value.Kacalx);
                    parameters.Add("numeroRisque",value.Kacrsq);
                    parameters.Add("numeroObject",value.Kacobj);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpObj> GetByAffaire(string typeAffaire, string codeAffaire, int numeroAliment){
                    return connection.EnsureOpened().Query<KpObj>(select_GetByAffaire, new {typeAffaire, codeAffaire, numeroAliment}).ToList();
            }
    }
}
