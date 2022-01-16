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

    public  partial class  YhrtforRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
JOIPB, JOALX, JOAVN, JOHIN, JORSQ
, JOFOR, JODES, JOCCH, JOFRV, JOFSP
, JORLE, JOACQ, JOTMC, JOTFF, JOTFP
, JOPRO, JOTMI, JOTFM, JOTMA, JOTMG
, JOCMC, JOCFO, JOCHT, JOCTT, JOCCP
, JOCPA, JOVAL, JOVAA, JOVAW, JOVAT
, JOVAU, JOVAH, JOIVO, JOIVA, JOIVW
, JOAVE, JOAVA, JOAVM, JOAVJ, JOPAQ
, JOEHH, JOEHC, JOEHI, JOCOS, JOTQU
 FROM YHRTFOR
WHERE JOIPB = :JOIPB
and JOALX = :JOALX
and JOAVN = :JOAVN
and JOHIN = :JOHIN
and JORSQ = :JORSQ
and JOFOR = :JOFOR
";
            const string update=@"UPDATE YHRTFOR SET 
JOIPB = :JOIPB, JOALX = :JOALX, JOAVN = :JOAVN, JOHIN = :JOHIN, JORSQ = :JORSQ, JOFOR = :JOFOR, JODES = :JODES, JOCCH = :JOCCH, JOFRV = :JOFRV, JOFSP = :JOFSP
, JORLE = :JORLE, JOACQ = :JOACQ, JOTMC = :JOTMC, JOTFF = :JOTFF, JOTFP = :JOTFP, JOPRO = :JOPRO, JOTMI = :JOTMI, JOTFM = :JOTFM, JOTMA = :JOTMA, JOTMG = :JOTMG
, JOCMC = :JOCMC, JOCFO = :JOCFO, JOCHT = :JOCHT, JOCTT = :JOCTT, JOCCP = :JOCCP, JOCPA = :JOCPA, JOVAL = :JOVAL, JOVAA = :JOVAA, JOVAW = :JOVAW, JOVAT = :JOVAT
, JOVAU = :JOVAU, JOVAH = :JOVAH, JOIVO = :JOIVO, JOIVA = :JOIVA, JOIVW = :JOIVW, JOAVE = :JOAVE, JOAVA = :JOAVA, JOAVM = :JOAVM, JOAVJ = :JOAVJ, JOPAQ = :JOPAQ
, JOEHH = :JOEHH, JOEHC = :JOEHC, JOEHI = :JOEHI, JOCOS = :JOCOS, JOTQU = :JOTQU
 WHERE 
JOIPB = :JOIPB and JOALX = :JOALX and JOAVN = :JOAVN and JOHIN = :JOHIN and JORSQ = :JORSQ and JOFOR = :JOFOR";
            const string delete=@"DELETE FROM YHRTFOR WHERE JOIPB = :JOIPB AND JOALX = :JOALX AND JOAVN = :JOAVN AND JOHIN = :JOHIN AND JORSQ = :JORSQ AND JOFOR = :JOFOR";
            const string insert=@"INSERT INTO  YHRTFOR (
JOIPB, JOALX, JOAVN, JOHIN, JORSQ
, JOFOR, JODES, JOCCH, JOFRV, JOFSP
, JORLE, JOACQ, JOTMC, JOTFF, JOTFP
, JOPRO, JOTMI, JOTFM, JOTMA, JOTMG
, JOCMC, JOCFO, JOCHT, JOCTT, JOCCP
, JOCPA, JOVAL, JOVAA, JOVAW, JOVAT
, JOVAU, JOVAH, JOIVO, JOIVA, JOIVW
, JOAVE, JOAVA, JOAVM, JOAVJ, JOPAQ
, JOEHH, JOEHC, JOEHI, JOCOS, JOTQU

) VALUES (
:JOIPB, :JOALX, :JOAVN, :JOHIN, :JORSQ
, :JOFOR, :JODES, :JOCCH, :JOFRV, :JOFSP
, :JORLE, :JOACQ, :JOTMC, :JOTFF, :JOTFP
, :JOPRO, :JOTMI, :JOTFM, :JOTMA, :JOTMG
, :JOCMC, :JOCFO, :JOCHT, :JOCTT, :JOCCP
, :JOCPA, :JOVAL, :JOVAA, :JOVAW, :JOVAT
, :JOVAU, :JOVAH, :JOIVO, :JOIVA, :JOIVW
, :JOAVE, :JOAVA, :JOAVM, :JOAVJ, :JOPAQ
, :JOEHH, :JOEHC, :JOEHI, :JOCOS, :JOTQU
)";
            const string select_GetByAffaire=@"SELECT
JOIPB, JOALX, JOAVN, JOHIN, JORSQ
, JOFOR, JODES, JOCCH, JOFRV, JOFSP
, JORLE, JOACQ, JOTMC, JOTFF, JOTFP
, JOPRO, JOTMI, JOTFM, JOTMA, JOTMG
, JOCMC, JOCFO, JOCHT, JOCTT, JOCCP
, JOCPA, JOVAL, JOVAA, JOVAW, JOVAT
, JOVAU, JOVAH, JOIVO, JOIVA, JOIVW
, JOAVE, JOAVA, JOAVM, JOAVJ, JOPAQ
, JOEHH, JOEHC, JOEHI, JOCOS, JOTQU
 FROM YHRTFOR
WHERE JOIPB = :JOIPB
and JOALX = :JOALX
and JOAVN = :JOAVN
";
            #endregion

            public YhrtforRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YprtFor Get(string JOIPB, int JOALX, int JOAVN, int JOHIN, int JORSQ, int JOFOR){
                return connection.Query<YprtFor>(select, new {JOIPB, JOALX, JOAVN, JOHIN, JORSQ, JOFOR}).SingleOrDefault();
            }


            public void Insert(YprtFor value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JOIPB",value.Joipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JOALX",value.Joalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JOAVN",value.Joavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JOHIN",value.Johin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JORSQ",value.Jorsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JOFOR",value.Jofor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JODES",value.Jodes??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("JOCCH",value.Jocch, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JOFRV",value.Jofrv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JOFSP",value.Jofsp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JORLE",value.Jorle??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JOACQ",value.Joacq, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JOTMC",value.Jotmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOTFF",value.Jotff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOTFP",value.Jotfp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("JOPRO",value.Jopro??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOTMI",value.Jotmi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOTFM",value.Jotfm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JOTMA",value.Jotma, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOTMG",value.Jotmg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOCMC",value.Jocmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOCFO",value.Jocfo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOCHT",value.Jocht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOCTT",value.Joctt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOCCP",value.Joccp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("JOCPA",value.Jocpa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("JOVAL",value.Joval, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JOVAA",value.Jovaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JOVAW",value.Jovaw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JOVAT",value.Jovat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JOVAU",value.Jovau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOVAH",value.Jovah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOIVO",value.Joivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JOIVA",value.Joiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JOIVW",value.Joivw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JOAVE",value.Joave, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JOAVA",value.Joava, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JOAVM",value.Joavm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JOAVJ",value.Joavj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JOPAQ",value.Jopaq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOEHH",value.Joehh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOEHC",value.Joehc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOEHI",value.Joehi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOCOS",value.Jocos??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOTQU",value.Jotqu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YprtFor value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JOIPB",value.Joipb);
                    parameters.Add("JOALX",value.Joalx);
                    parameters.Add("JOAVN",value.Joavn);
                    parameters.Add("JOHIN",value.Johin);
                    parameters.Add("JORSQ",value.Jorsq);
                    parameters.Add("JOFOR",value.Jofor);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YprtFor value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JOIPB",value.Joipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JOALX",value.Joalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JOAVN",value.Joavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JOHIN",value.Johin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JORSQ",value.Jorsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JOFOR",value.Jofor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JODES",value.Jodes??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:60, scale: 0);
                    parameters.Add("JOCCH",value.Jocch, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JOFRV",value.Jofrv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JOFSP",value.Jofsp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JORLE",value.Jorle??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JOACQ",value.Joacq, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JOTMC",value.Jotmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOTFF",value.Jotff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOTFP",value.Jotfp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("JOPRO",value.Jopro??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOTMI",value.Jotmi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOTFM",value.Jotfm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JOTMA",value.Jotma, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOTMG",value.Jotmg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOCMC",value.Jocmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOCFO",value.Jocfo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOCHT",value.Jocht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOCTT",value.Joctt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOCCP",value.Joccp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("JOCPA",value.Jocpa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 9);
                    parameters.Add("JOVAL",value.Joval, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JOVAA",value.Jovaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JOVAW",value.Jovaw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JOVAT",value.Jovat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JOVAU",value.Jovau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOVAH",value.Jovah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOIVO",value.Joivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JOIVA",value.Joiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JOIVW",value.Joivw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JOAVE",value.Joave, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JOAVA",value.Joava, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JOAVM",value.Joavm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JOAVJ",value.Joavj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JOPAQ",value.Jopaq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOEHH",value.Joehh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOEHC",value.Joehc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOEHI",value.Joehi, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JOCOS",value.Jocos??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOTQU",value.Jotqu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JOIPB",value.Joipb);
                    parameters.Add("JOALX",value.Joalx);
                    parameters.Add("JOAVN",value.Joavn);
                    parameters.Add("JOHIN",value.Johin);
                    parameters.Add("JORSQ",value.Jorsq);
                    parameters.Add("JOFOR",value.Jofor);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YprtFor> GetByAffaire(string JOIPB, int JOALX, int JOAVN){
                    return connection.EnsureOpened().Query<YprtFor>(select_GetByAffaire, new {JOIPB, JOALX, JOAVN}).ToList();
            }
    }
}
