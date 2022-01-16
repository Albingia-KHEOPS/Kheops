using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{

    public partial class KPISValRepository : BaseTableRepository
    {

        private IdentifierGenerator idGenerator;

        #region Query texts        
        //ZALBINKSPP
        const string select = @"SELECT
KKCID, KKCTYP, KKCIPB, KKCALX, KKCAVN
, KKCHIN, KKCRSQ, KKCOBJ, KKCFOR, KKCOPT
, KKCKGBNMID, KKCVDEC, KKCVUN, KKCVDATD, KKCVHEUD
, KKCVDATF, KKCVHEUF, KKCVTXT, KKCKFBID, KKCISVAL
 FROM KPISVAL
WHERE KKCID = :KKCID
";
        const string update = @"UPDATE KPISVAL SET 
KKCID = :KKCID, KKCTYP = :KKCTYP, KKCIPB = :KKCIPB, KKCALX = :KKCALX, KKCAVN = :KKCAVN, KKCHIN = :KKCHIN, KKCRSQ = :KKCRSQ, KKCOBJ = :KKCOBJ, KKCFOR = :KKCFOR, KKCOPT = :KKCOPT
, KKCKGBNMID = :KKCKGBNMID, KKCVDEC = :KKCVDEC, KKCVUN = :KKCVUN, KKCVDATD = :KKCVDATD, KKCVHEUD = :KKCVHEUD, KKCVDATF = :KKCVDATF, KKCVHEUF = :KKCVHEUF, KKCVTXT = :KKCVTXT, KKCKFBID = :KKCKFBID, KKCISVAL = :KKCISVAL

 WHERE 
KKCID = :KKCID";
        const string delete = @"DELETE FROM KPISVAL WHERE KKCID = :KKCID";
        const string insert = @"INSERT INTO  KPISVAL (
KKCID, KKCTYP, KKCIPB, KKCALX, KKCAVN
, KKCHIN, KKCRSQ, KKCOBJ, KKCFOR, KKCOPT
, KKCKGBNMID, KKCVDEC, KKCVUN, KKCVDATD, KKCVHEUD
, KKCVDATF, KKCVHEUF, KKCVTXT, KKCKFBID, KKCISVAL

) VALUES (
:KKCID, :KKCTYP, :KKCIPB, :KKCALX, :KKCAVN
, :KKCHIN, :KKCRSQ, :KKCOBJ, :KKCFOR, :KKCOPT
, :KKCKGBNMID, :KKCVDEC, :KKCVUN, :KKCVDATD, :KKCVHEUD
, :KKCVDATF, :KKCVHEUF, :KKCVTXT, :KKCKFBID, :KKCISVAL
)";
        const string select_GetByAffaire = @"SELECT
KKCID, KKCTYP, KKCIPB, KKCALX, KKCAVN
, KKCHIN, KKCRSQ, KKCOBJ, KKCFOR, KKCOPT
, KKCKGBNMID, KKCVDEC, KKCVUN, KKCVDATD, KKCVHEUD
, KKCVDATF, KKCVHEUF, KKCVTXT, KKCKFBID, KKCISVAL
 FROM KPISVAL
WHERE KKCIPB = :KKCIPB
and KKCALX = :KKCALX
";
        #endregion

        public KPISValRepository(IDbConnection connection, IdentifierGenerator idGenerator) : base(connection)
        {
            this.idGenerator = idGenerator;
        }

        public KPISVal Get(Int64 KKCID)
        {
            return connection.Query<KPISVal>(select, new { KKCID }).SingleOrDefault();
        }

        public int NewId()
        {
            return idGenerator.NewId("KKCID");
        }

        public void Insert(KPISVal value)
        {
            if (value.Kkcid == default(Int64))
            {
                value.Kkcid = NewId();
            }
            var parameters = new DynamicParameters();
            parameters.Add("KKCID", value.Kkcid, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("KKCTYP", value.Kkctyp ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KKCIPB", value.Kkcipb ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 9, scale: 0);
            parameters.Add("KKCALX", value.Kkcalx, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KKCAVN", value.Kkcavn, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KKCHIN", value.Kkchin, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KKCRSQ", value.Kkcrsq, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KKCOBJ", value.Kkcobj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KKCFOR", value.Kkcfor, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KKCOPT", value.Kkcopt, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KKCKGBNMID", value.Kkckgbnmid ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KKCVDEC", value.Kkcvdec, dbType: DbType.Decimal, direction: ParameterDirection.Input, size: 19, scale: 4);
            parameters.Add("KKCVUN", value.Kkcvun ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KKCVDATD", value.Kkcvdatd, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 8, scale: 0);
            parameters.Add("KKCVHEUD", value.Kkcvheud, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 6, scale: 0);
            parameters.Add("KKCVDATF", value.Kkcvdatf, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 8, scale: 0);
            parameters.Add("KKCVHEUF", value.Kkcvheuf, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 6, scale: 0);
            parameters.Add("KKCVTXT", value.Kkcvtxt ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 200, scale: 0);
            parameters.Add("KKCKFBID", value.Kkckfbid, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KKCISVAL", value.Kkcisval ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 200, scale: 0);

            connection.EnsureOpened().Execute(insert, parameters);
        }
        public void Delete(KPISVal value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("KKCID", value.Kkcid);
            connection.EnsureOpened().Execute(delete, parameters);
        }

        public void Update(KPISVal value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("KKCID", value.Kkcid, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("KKCTYP", value.Kkctyp ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KKCIPB", value.Kkcipb ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 9, scale: 0);
            parameters.Add("KKCALX", value.Kkcalx, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KKCAVN", value.Kkcavn, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KKCHIN", value.Kkchin, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KKCRSQ", value.Kkcrsq, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KKCOBJ", value.Kkcobj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KKCFOR", value.Kkcfor, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KKCOPT", value.Kkcopt, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KKCKGBNMID", value.Kkckgbnmid ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KKCVDEC", value.Kkcvdec, dbType: DbType.Decimal, direction: ParameterDirection.Input, size: 19, scale: 4);
            parameters.Add("KKCVUN", value.Kkcvun ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KKCVDATD", value.Kkcvdatd, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 8, scale: 0);
            parameters.Add("KKCVHEUD", value.Kkcvheud, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 6, scale: 0);
            parameters.Add("KKCVDATF", value.Kkcvdatf, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 8, scale: 0);
            parameters.Add("KKCVHEUF", value.Kkcvheuf, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 6, scale: 0);
            parameters.Add("KKCVTXT", value.Kkcvtxt ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 200, scale: 0);
            parameters.Add("KKCKFBID", value.Kkckfbid, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KKCISVAL", value.Kkcisval ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 200, scale: 0);
            parameters.Add("KKCID", value.Kkcid);

            connection.EnsureOpened().Execute(update, parameters);
        }

        public IEnumerable<KPISVal> GetByAffaire(string KKCIPB, int KKCALX)
        {
            return connection.EnsureOpened().Query<KPISVal>(select_GetByAffaire, new { KKCIPB, KKCALX }).ToList();
        }
    }
}
