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

    public  partial class  KpRsqRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KABTYP, KABIPB, KABALX, KABRSQ, KABCIBLE
, KABDESC, KABDESI, KABOBSV, KABREPVAL, KABREPOBL
, KABAPE, KABTRE, KABCLASS, KABNMC01, KABNMC02
, KABNMC03, KABNMC04, KABNMC05, KABMAND, KABMANF
, KABDSPP, KABLCIVALO, KABLCIVALA, KABLCIVALW, KABLCIUNIT
, KABLCIBASE, KABKDIID, KABFRHVALO, KABFRHVALA, KABFRHVALW
, KABFRHUNIT, KABFRHBASE, KABKDKID, KABNSIR, KABMANDH
, KABMANFH, KABSURF, KABVMC, KABPROL, KABPBI
, KABBRNT, KABBRNC FROM KPRSQ
WHERE KABTYP = :typeAffaire
and KABIPB = :numeroAffaire
and KABALX = :numeroAliement
and KABRSQ = :numeroRisque
FETCH FIRST 200 ROWS ONLY
";
            const string update=@"UPDATE KPRSQ SET 
KABTYP = :KABTYP, KABIPB = :KABIPB, KABALX = :KABALX, KABRSQ = :KABRSQ, KABCIBLE = :KABCIBLE, KABDESC = :KABDESC, KABDESI = :KABDESI, KABOBSV = :KABOBSV, KABREPVAL = :KABREPVAL, KABREPOBL = :KABREPOBL
, KABAPE = :KABAPE, KABTRE = :KABTRE, KABCLASS = :KABCLASS, KABNMC01 = :KABNMC01, KABNMC02 = :KABNMC02, KABNMC03 = :KABNMC03, KABNMC04 = :KABNMC04, KABNMC05 = :KABNMC05, KABMAND = :KABMAND, KABMANF = :KABMANF
, KABDSPP = :KABDSPP, KABLCIVALO = :KABLCIVALO, KABLCIVALA = :KABLCIVALA, KABLCIVALW = :KABLCIVALW, KABLCIUNIT = :KABLCIUNIT, KABLCIBASE = :KABLCIBASE, KABKDIID = :KABKDIID, KABFRHVALO = :KABFRHVALO, KABFRHVALA = :KABFRHVALA, KABFRHVALW = :KABFRHVALW
, KABFRHUNIT = :KABFRHUNIT, KABFRHBASE = :KABFRHBASE, KABKDKID = :KABKDKID, KABNSIR = :KABNSIR, KABMANDH = :KABMANDH, KABMANFH = :KABMANFH, KABSURF = :KABSURF, KABVMC = :KABVMC, KABPROL = :KABPROL, KABPBI = :KABPBI
, KABBRNT = :KABBRNT, KABBRNC = :KABBRNC
 WHERE 
KABTYP = :typeAffaire and KABIPB = :numeroAffaire and KABALX = :numeroAliement and KABRSQ = :numeroRisque";
            const string delete=@"DELETE FROM KPRSQ WHERE KABTYP = :typeAffaire AND KABIPB = :numeroAffaire AND KABALX = :numeroAliement AND KABRSQ = :numeroRisque";
            const string insert=@"INSERT INTO  KPRSQ (
KABTYP, KABIPB, KABALX, KABRSQ, KABCIBLE
, KABDESC, KABDESI, KABOBSV, KABREPVAL, KABREPOBL
, KABAPE, KABTRE, KABCLASS, KABNMC01, KABNMC02
, KABNMC03, KABNMC04, KABNMC05, KABMAND, KABMANF
, KABDSPP, KABLCIVALO, KABLCIVALA, KABLCIVALW, KABLCIUNIT
, KABLCIBASE, KABKDIID, KABFRHVALO, KABFRHVALA, KABFRHVALW
, KABFRHUNIT, KABFRHBASE, KABKDKID, KABNSIR, KABMANDH
, KABMANFH, KABSURF, KABVMC, KABPROL, KABPBI
, KABBRNT, KABBRNC
) VALUES (
:KABTYP, :KABIPB, :KABALX, :KABRSQ, :KABCIBLE
, :KABDESC, :KABDESI, :KABOBSV, :KABREPVAL, :KABREPOBL
, :KABAPE, :KABTRE, :KABCLASS, :KABNMC01, :KABNMC02
, :KABNMC03, :KABNMC04, :KABNMC05, :KABMAND, :KABMANF
, :KABDSPP, :KABLCIVALO, :KABLCIVALA, :KABLCIVALW, :KABLCIUNIT
, :KABLCIBASE, :KABKDIID, :KABFRHVALO, :KABFRHVALA, :KABFRHVALW
, :KABFRHUNIT, :KABFRHBASE, :KABKDKID, :KABNSIR, :KABMANDH
, :KABMANFH, :KABSURF, :KABVMC, :KABPROL, :KABPBI
, :KABBRNT, :KABBRNC)";
            const string select_Liste=@"SELECT
KABTYP, KABIPB, KABALX, KABRSQ, KABCIBLE
, KABDESC, KABDESI, KABOBSV, KABREPVAL, KABREPOBL
, KABAPE, KABTRE, KABCLASS, KABNMC01, KABNMC02
, KABNMC03, KABNMC04, KABNMC05, KABMAND, KABMANF
, KABDSPP, KABLCIVALO, KABLCIVALA, KABLCIVALW, KABLCIUNIT
, KABLCIBASE, KABKDIID, KABFRHVALO, KABFRHVALA, KABFRHVALW
, KABFRHUNIT, KABFRHBASE, KABKDKID, KABNSIR, KABMANDH
, KABMANFH, KABSURF, KABVMC, KABPROL, KABPBI
, KABBRNT, KABBRNC FROM KPRSQ
WHERE KABTYP = :typeAffaire
and KABIPB = :numeroAffaire
and KABALX = :numeroAliment
FETCH FIRST 200 ROWS ONLY
";
            #endregion

            public KpRsqRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpRsq Get(string typeAffaire, string numeroAffaire, int numeroAliement, int numeroRisque){
                return connection.Query<KpRsq>(select, new {typeAffaire, numeroAffaire, numeroAliement, numeroRisque}).SingleOrDefault();
            }


            public void Insert(KpRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KABTYP",value.Kabtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KABIPB",value.Kabipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KABALX",value.Kabalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KABRSQ",value.Kabrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KABCIBLE",value.Kabcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KABDESC",value.Kabdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KABDESI",value.Kabdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABOBSV",value.Kabobsv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABREPVAL",value.Kabrepval??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KABREPOBL",value.Kabrepobl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KABAPE",value.Kabape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KABTRE",value.Kabtre??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KABCLASS",value.Kabclass??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KABNMC01",value.Kabnmc01??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABNMC02",value.Kabnmc02??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABNMC03",value.Kabnmc03??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABNMC04",value.Kabnmc04??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABNMC05",value.Kabnmc05??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABMAND",value.Kabmand, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KABMANF",value.Kabmanf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KABDSPP",value.Kabdspp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABLCIVALO",value.Kablcivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABLCIVALA",value.Kablcivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABLCIVALW",value.Kablcivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABLCIUNIT",value.Kablciunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KABLCIBASE",value.Kablcibase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KABKDIID",value.Kabkdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABFRHVALO",value.Kabfrhvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABFRHVALA",value.Kabfrhvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABFRHVALW",value.Kabfrhvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABFRHUNIT",value.Kabfrhunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KABFRHBASE",value.Kabfrhbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KABKDKID",value.Kabkdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABNSIR",value.Kabnsir??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:14, scale: 0);
                    parameters.Add("KABMANDH",value.Kabmandh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KABMANFH",value.Kabmanfh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KABSURF",value.Kabsurf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KABVMC",value.Kabvmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KABPROL",value.Kabprol??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KABPBI",value.Kabpbi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KABBRNT",value.Kabbrnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KABBRNC",value.Kabbrnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("typeAffaire",value.Kabtyp);
                    parameters.Add("numeroAffaire",value.Kabipb);
                    parameters.Add("numeroAliement",value.Kabalx);
                    parameters.Add("numeroRisque",value.Kabrsq);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KABTYP",value.Kabtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KABIPB",value.Kabipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KABALX",value.Kabalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KABRSQ",value.Kabrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KABCIBLE",value.Kabcible??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KABDESC",value.Kabdesc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KABDESI",value.Kabdesi, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABOBSV",value.Kabobsv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABREPVAL",value.Kabrepval??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KABREPOBL",value.Kabrepobl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KABAPE",value.Kabape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KABTRE",value.Kabtre??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KABCLASS",value.Kabclass??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KABNMC01",value.Kabnmc01??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABNMC02",value.Kabnmc02??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABNMC03",value.Kabnmc03??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABNMC04",value.Kabnmc04??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABNMC05",value.Kabnmc05??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABMAND",value.Kabmand, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KABMANF",value.Kabmanf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KABDSPP",value.Kabdspp, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABLCIVALO",value.Kablcivalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABLCIVALA",value.Kablcivala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABLCIVALW",value.Kablcivalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABLCIUNIT",value.Kablciunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KABLCIBASE",value.Kablcibase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KABKDIID",value.Kabkdiid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABFRHVALO",value.Kabfrhvalo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABFRHVALA",value.Kabfrhvala, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABFRHVALW",value.Kabfrhvalw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KABFRHUNIT",value.Kabfrhunit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KABFRHBASE",value.Kabfrhbase??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KABKDKID",value.Kabkdkid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KABNSIR",value.Kabnsir??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:14, scale: 0);
                    parameters.Add("KABMANDH",value.Kabmandh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KABMANFH",value.Kabmanfh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KABSURF",value.Kabsurf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KABVMC",value.Kabvmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KABPROL",value.Kabprol??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KABPBI",value.Kabpbi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KABBRNT",value.Kabbrnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KABBRNC",value.Kabbrnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("typeAffaire",value.Kabtyp);
                    parameters.Add("numeroAffaire",value.Kabipb);
                    parameters.Add("numeroAliement",value.Kabalx);
                    parameters.Add("numeroRisque",value.Kabrsq);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpRsq> Liste(string typeAffaire, string numeroAffaire, int numeroAliment){
                    return connection.EnsureOpened().Query<KpRsq>(select_Liste, new {typeAffaire, numeroAffaire, numeroAliment}).ToList();
            }
    }
}
