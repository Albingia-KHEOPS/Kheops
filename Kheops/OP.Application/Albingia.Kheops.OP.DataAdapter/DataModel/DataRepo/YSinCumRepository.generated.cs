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

    public  partial class  YSinCumRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
SUSUA, SUNUM, SUSBR, SUIPB, SUALX
, SUAVN, SUTBR, SUTSB, SUTPO, SUTPE
, SUTPA, SUTIN, SUTFR, SUTRE, SUTCH
, SUAPO, SUAPE, SUAPA, SUAIN, SUAFR
, SUARE, SUACH, SUKPO, SUKPE, SUKPA
, SUKIN, SUKFR, SUKRE, SUKCH, SUCPO
, SUCPE, SUCPA, SUCIN, SUCFR, SUCRE
, SUCCH, SUMJU, SUMJA, SUMJM, SUMJJ
, SUMJH, SUTPP, SUTPF, SUAPP, SUAPF
, SUKPP, SUKPF, SUCPP, SUCPF FROM YSINCUM
WHERE SUSUA = :SUSUA
and SUNUM = :SUNUM
and SUSBR = :SUSBR
";
            const string update=@"UPDATE YSINCUM SET 
SUSUA = :SUSUA, SUNUM = :SUNUM, SUSBR = :SUSBR, SUIPB = :SUIPB, SUALX = :SUALX, SUAVN = :SUAVN, SUTBR = :SUTBR, SUTSB = :SUTSB, SUTPO = :SUTPO, SUTPE = :SUTPE
, SUTPA = :SUTPA, SUTIN = :SUTIN, SUTFR = :SUTFR, SUTRE = :SUTRE, SUTCH = :SUTCH, SUAPO = :SUAPO, SUAPE = :SUAPE, SUAPA = :SUAPA, SUAIN = :SUAIN, SUAFR = :SUAFR
, SUARE = :SUARE, SUACH = :SUACH, SUKPO = :SUKPO, SUKPE = :SUKPE, SUKPA = :SUKPA, SUKIN = :SUKIN, SUKFR = :SUKFR, SUKRE = :SUKRE, SUKCH = :SUKCH, SUCPO = :SUCPO
, SUCPE = :SUCPE, SUCPA = :SUCPA, SUCIN = :SUCIN, SUCFR = :SUCFR, SUCRE = :SUCRE, SUCCH = :SUCCH, SUMJU = :SUMJU, SUMJA = :SUMJA, SUMJM = :SUMJM, SUMJJ = :SUMJJ
, SUMJH = :SUMJH, SUTPP = :SUTPP, SUTPF = :SUTPF, SUAPP = :SUAPP, SUAPF = :SUAPF, SUKPP = :SUKPP, SUKPF = :SUKPF, SUCPP = :SUCPP, SUCPF = :SUCPF
 WHERE 
SUSUA = :SUSUA and SUNUM = :SUNUM and SUSBR = :SUSBR";
            const string delete=@"DELETE FROM YSINCUM WHERE SUSUA = :SUSUA AND SUNUM = :SUNUM AND SUSBR = :SUSBR";
            const string insert=@"INSERT INTO  YSINCUM (
SUSUA, SUNUM, SUSBR, SUIPB, SUALX
, SUAVN, SUTBR, SUTSB, SUTPO, SUTPE
, SUTPA, SUTIN, SUTFR, SUTRE, SUTCH
, SUAPO, SUAPE, SUAPA, SUAIN, SUAFR
, SUARE, SUACH, SUKPO, SUKPE, SUKPA
, SUKIN, SUKFR, SUKRE, SUKCH, SUCPO
, SUCPE, SUCPA, SUCIN, SUCFR, SUCRE
, SUCCH, SUMJU, SUMJA, SUMJM, SUMJJ
, SUMJH, SUTPP, SUTPF, SUAPP, SUAPF
, SUKPP, SUKPF, SUCPP, SUCPF
) VALUES (
:SUSUA, :SUNUM, :SUSBR, :SUIPB, :SUALX
, :SUAVN, :SUTBR, :SUTSB, :SUTPO, :SUTPE
, :SUTPA, :SUTIN, :SUTFR, :SUTRE, :SUTCH
, :SUAPO, :SUAPE, :SUAPA, :SUAIN, :SUAFR
, :SUARE, :SUACH, :SUKPO, :SUKPE, :SUKPA
, :SUKIN, :SUKFR, :SUKRE, :SUKCH, :SUCPO
, :SUCPE, :SUCPA, :SUCIN, :SUCFR, :SUCRE
, :SUCCH, :SUMJU, :SUMJA, :SUMJM, :SUMJJ
, :SUMJH, :SUTPP, :SUTPF, :SUAPP, :SUAPF
, :SUKPP, :SUKPF, :SUCPP, :SUCPF)";
            const string select_GetByAffaire=@"SELECT
SUSUA, SUNUM, SUSBR, SUIPB, SUALX
, SUAVN, SUTBR, SUTSB, SUTPO, SUTPE
, SUTPA, SUTIN, SUTFR, SUTRE, SUTCH
, SUAPO, SUAPE, SUAPA, SUAIN, SUAFR
, SUARE, SUACH, SUKPO, SUKPE, SUKPA
, SUKIN, SUKFR, SUKRE, SUKCH, SUCPO
, SUCPE, SUCPA, SUCIN, SUCFR, SUCRE
, SUCCH, SUMJU, SUMJA, SUMJM, SUMJJ
, SUMJH, SUTPP, SUTPF, SUAPP, SUAPF
, SUKPP, SUKPF, SUCPP, SUCPF FROM YSINCUM
WHERE SUIPB = :SUIPB
and SUALX = :SUALX
and SUAVN = :SUAVN
";
            #endregion

            public YSinCumRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YSinCum Get(int SUSUA, int SUNUM, string SUSBR){
                return connection.Query<YSinCum>(select, new {SUSUA, SUNUM, SUSBR}).SingleOrDefault();
            }


            public void Insert(YSinCum value){
                    var parameters = new DynamicParameters();
                    parameters.Add("SUSUA",value.Susua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SUNUM",value.Sunum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SUSBR",value.Susbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SUIPB",value.Suipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("SUALX",value.Sualx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SUAVN",value.Suavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SUTBR",value.Sutbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SUTSB",value.Sutsb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SUTPO",value.Sutpo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTPE",value.Sutpe, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTPA",value.Sutpa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTIN",value.Sutin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTFR",value.Sutfr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTRE",value.Sutre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTCH",value.Sutch, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAPO",value.Suapo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAPE",value.Suape, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAPA",value.Suapa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAIN",value.Suain, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAFR",value.Suafr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUARE",value.Suare, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUACH",value.Suach, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKPO",value.Sukpo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKPE",value.Sukpe, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKPA",value.Sukpa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKIN",value.Sukin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKFR",value.Sukfr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKRE",value.Sukre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKCH",value.Sukch, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCPO",value.Sucpo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCPE",value.Sucpe, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCPA",value.Sucpa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCIN",value.Sucin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCFR",value.Sucfr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCRE",value.Sucre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCCH",value.Succh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUMJU",value.Sumju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SUMJA",value.Sumja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SUMJM",value.Sumjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SUMJJ",value.Sumjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SUMJH",value.Sumjh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("SUTPP",value.Sutpp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTPF",value.Sutpf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAPP",value.Suapp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAPF",value.Suapf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKPP",value.Sukpp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKPF",value.Sukpf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCPP",value.Sucpp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCPF",value.Sucpf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YSinCum value){
                    var parameters = new DynamicParameters();
                    parameters.Add("SUSUA",value.Susua);
                    parameters.Add("SUNUM",value.Sunum);
                    parameters.Add("SUSBR",value.Susbr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YSinCum value){
                    var parameters = new DynamicParameters();
                    parameters.Add("SUSUA",value.Susua, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SUNUM",value.Sunum, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("SUSBR",value.Susbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SUIPB",value.Suipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("SUALX",value.Sualx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SUAVN",value.Suavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SUTBR",value.Sutbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SUTSB",value.Sutsb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("SUTPO",value.Sutpo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTPE",value.Sutpe, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTPA",value.Sutpa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTIN",value.Sutin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTFR",value.Sutfr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTRE",value.Sutre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTCH",value.Sutch, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAPO",value.Suapo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAPE",value.Suape, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAPA",value.Suapa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAIN",value.Suain, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAFR",value.Suafr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUARE",value.Suare, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUACH",value.Suach, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKPO",value.Sukpo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKPE",value.Sukpe, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKPA",value.Sukpa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKIN",value.Sukin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKFR",value.Sukfr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKRE",value.Sukre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKCH",value.Sukch, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCPO",value.Sucpo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCPE",value.Sucpe, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCPA",value.Sucpa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCIN",value.Sucin, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCFR",value.Sucfr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCRE",value.Sucre, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCCH",value.Succh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUMJU",value.Sumju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("SUMJA",value.Sumja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("SUMJM",value.Sumjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SUMJJ",value.Sumjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("SUMJH",value.Sumjh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("SUTPP",value.Sutpp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUTPF",value.Sutpf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAPP",value.Suapp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUAPF",value.Suapf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKPP",value.Sukpp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUKPF",value.Sukpf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCPP",value.Sucpp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUCPF",value.Sucpf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("SUSUA",value.Susua);
                    parameters.Add("SUNUM",value.Sunum);
                    parameters.Add("SUSBR",value.Susbr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YSinCum> GetByAffaire(string SUIPB, int SUALX, int SUAVN){
                    return connection.EnsureOpened().Query<YSinCum>(select_GetByAffaire, new {SUIPB, SUALX, SUAVN}).ToList();
            }
    }
}
