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

    public  partial class  KpDocExtRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KERID, KERTYP, KERIPB, KERALX, KERSUA
, KERNUM, KERSBR, KERSERV, KERACTG, KERAVN
, KERORD, KERTYPO, KERLIB, KERNOM, KERCHM
, KERSTU, KERSIT, KERSTD, KERSTH, KERCRU
, KERCRD, KERCRH, KERREF FROM KPDOCEXT
WHERE KERID = :KERID
";
            const string update=@"UPDATE KPDOCEXT SET 
KERID = :KERID, KERTYP = :KERTYP, KERIPB = :KERIPB, KERALX = :KERALX, KERSUA = :KERSUA, KERNUM = :KERNUM, KERSBR = :KERSBR, KERSERV = :KERSERV, KERACTG = :KERACTG, KERAVN = :KERAVN
, KERORD = :KERORD, KERTYPO = :KERTYPO, KERLIB = :KERLIB, KERNOM = :KERNOM, KERCHM = :KERCHM, KERSTU = :KERSTU, KERSIT = :KERSIT, KERSTD = :KERSTD, KERSTH = :KERSTH, KERCRU = :KERCRU
, KERCRD = :KERCRD, KERCRH = :KERCRH, KERREF = :KERREF
 WHERE 
KERID = :KERID";
            const string delete=@"DELETE FROM KPDOCEXT WHERE KERID = :KERID";
            const string insert=@"INSERT INTO  KPDOCEXT (
KERID, KERTYP, KERIPB, KERALX, KERSUA
, KERNUM, KERSBR, KERSERV, KERACTG, KERAVN
, KERORD, KERTYPO, KERLIB, KERNOM, KERCHM
, KERSTU, KERSIT, KERSTD, KERSTH, KERCRU
, KERCRD, KERCRH, KERREF
) VALUES (
:KERID, :KERTYP, :KERIPB, :KERALX, :KERSUA
, :KERNUM, :KERSBR, :KERSERV, :KERACTG, :KERAVN
, :KERORD, :KERTYPO, :KERLIB, :KERNOM, :KERCHM
, :KERSTU, :KERSIT, :KERSTD, :KERSTH, :KERCRU
, :KERCRD, :KERCRH, :KERREF)";
            const string select_GetByAffaire=@"SELECT
KERID, KERTYP, KERIPB, KERALX, KERSUA
, KERNUM, KERSBR, KERSERV, KERACTG, KERAVN
, KERORD, KERTYPO, KERLIB, KERNOM, KERCHM
, KERSTU, KERSIT, KERSTD, KERSTH, KERCRU
, KERCRD, KERCRH, KERREF FROM KPDOCEXT
WHERE KERTYP = :typeAffaire
and KERIPB = :codeAffaire
and KERALX = :numroAliment
";
            const string select_GetByAffaireAvenant=@"SELECT
KERID, KERTYP, KERIPB, KERALX, KERSUA
, KERNUM, KERSBR, KERSERV, KERACTG, KERAVN
, KERORD, KERTYPO, KERLIB, KERNOM, KERCHM
, KERSTU, KERSIT, KERSTD, KERSTH, KERCRU
, KERCRD, KERCRH, KERREF FROM KPDOCEXT
WHERE KERTYP = :typeAffaire
and KERIPB = :codeAffaire
and KERALX = :numeroAliment
and KERAVN = :numeroAvenant
";
            #endregion

            public KpDocExtRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpDocExt Get(Int64 KERID){
                return connection.Query<KpDocExt>(select, new {KERID}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KERID") ;
            }

            public void Insert(KpDocExt value){
                    if(value.Kerid == default(Int64)) {
                        value.Kerid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KERID",value.Kerid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KERTYP",value.Kertyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KERIPB",value.Keripb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KERALX",value.Keralx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KERSUA",value.Kersua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KERNUM",value.Kernum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KERSBR",value.Kersbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KERSERV",value.Kerserv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KERACTG",value.Keractg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KERAVN",value.Keravn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KERORD",value.Kerord, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KERTYPO",value.Kertypo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KERLIB",value.Kerlib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:255, scale: 0);
                    parameters.Add("KERNOM",value.Kernom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:255, scale: 0);
                    parameters.Add("KERCHM",value.Kerchm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:255, scale: 0);
                    parameters.Add("KERSTU",value.Kerstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KERSIT",value.Kersit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KERSTD",value.Kerstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KERSTH",value.Kersth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KERCRU",value.Kercru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KERCRD",value.Kercrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KERCRH",value.Kercrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KERREF",value.Kerref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpDocExt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KERID",value.Kerid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpDocExt value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KERID",value.Kerid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KERTYP",value.Kertyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KERIPB",value.Keripb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KERALX",value.Keralx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KERSUA",value.Kersua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KERNUM",value.Kernum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KERSBR",value.Kersbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KERSERV",value.Kerserv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KERACTG",value.Keractg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KERAVN",value.Keravn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KERORD",value.Kerord, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KERTYPO",value.Kertypo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KERLIB",value.Kerlib??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:255, scale: 0);
                    parameters.Add("KERNOM",value.Kernom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:255, scale: 0);
                    parameters.Add("KERCHM",value.Kerchm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:255, scale: 0);
                    parameters.Add("KERSTU",value.Kerstu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KERSIT",value.Kersit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KERSTD",value.Kerstd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KERSTH",value.Kersth, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KERCRU",value.Kercru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KERCRD",value.Kercrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KERCRH",value.Kercrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KERREF",value.Kerref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KERID",value.Kerid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpDocExt> GetByAffaire(string typeAffaire, string codeAffaire, int numroAliment){
                    return connection.EnsureOpened().Query<KpDocExt>(select_GetByAffaire, new {typeAffaire, codeAffaire, numroAliment}).ToList();
            }
            public IEnumerable<KpDocExt> GetByAffaireAvenant(string typeAffaire, string codeAffaire, int numeroAliment, int numeroAvenant){
                    return connection.EnsureOpened().Query<KpDocExt>(select_GetByAffaireAvenant, new {typeAffaire, codeAffaire, numeroAliment, numeroAvenant}).ToList();
            }
    }
}
