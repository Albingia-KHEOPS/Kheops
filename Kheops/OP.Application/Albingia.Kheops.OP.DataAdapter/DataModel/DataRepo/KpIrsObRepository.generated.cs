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

    public partial class KpIrsObRepository : BaseTableRepository
    {

        private IdentifierGenerator idGenerator;

        #region Query texts        
        //ZALBINKHEO
        const string select = @"SELECT
KFATYP, KFAIPB, KFAALX, KFAAVN, KFAHIN
, KFARSQ, KFAOBJ, KFANATS, KFANEGA, KFAFRQE
, KFANBPA, KFANBEX, KFANBVI, KFAGN08, KFAGN09
, KFAGN10, KFANBIN, KFANBPE, KFAGCT, KFANBEM
, KFATYTN, KFANMDF, KFAFENT, KFAFSVT, KFANMSC
, KFALABD, KFANAI, KFALMA, KFAIFP, KFATHF
, KFATU1, KFATU2, KFAASC, KFAAUTL, KFAQMD
, KFASURF, KFAVMC, KFAPROL, KFADEPD FROM KPIRSOB
WHERE KFATYP = :KFATYP
and KFAIPB = :KFAIPB
and KFAALX = :KFAALX
and KFARSQ = :KFARSQ
and KFAOBJ = :KFAOBJ
";
        const string update = @"UPDATE KPIRSOB SET 
KFATYP = :KFATYP, KFAIPB = :KFAIPB, KFAALX = :KFAALX, KFAAVN = :KFAAVN, KFAHIN = :KFAHIN, KFARSQ = :KFARSQ, KFAOBJ = :KFAOBJ, KFANATS = :KFANATS, KFANEGA = :KFANEGA, KFAFRQE = :KFAFRQE
, KFANBPA = :KFANBPA, KFANBEX = :KFANBEX, KFANBVI = :KFANBVI, KFAGN08 = :KFAGN08, KFAGN09 = :KFAGN09, KFAGN10 = :KFAGN10, KFANBIN = :KFANBIN, KFANBPE = :KFANBPE, KFAGCT = :KFAGCT, KFANBEM = :KFANBEM
, KFATYTN = :KFATYTN, KFANMDF = :KFANMDF, KFAFENT = :KFAFENT, KFAFSVT = :KFAFSVT, KFANMSC = :KFANMSC, KFALABD = :KFALABD, KFANAI = :KFANAI, KFALMA = :KFALMA, KFAIFP = :KFAIFP, KFATHF = :KFATHF
, KFATU1 = :KFATU1, KFATU2 = :KFATU2, KFAASC = :KFAASC, KFAAUTL = :KFAAUTL, KFAQMD = :KFAQMD, KFASURF = :KFASURF, KFAVMC = :KFAVMC, KFAPROL = :KFAPROL, KFADEPD = :KFADEPD
 WHERE 
KFATYP = :KFATYP and KFAIPB = :KFAIPB and KFAALX = :KFAALX and KFARSQ = :KFARSQ and KFAOBJ = :KFAOBJ";
        const string delete = @"DELETE FROM KPIRSOB WHERE KFATYP = :KFATYP AND KFAIPB = :KFAIPB AND KFAALX = :KFAALX AND KFARSQ = :KFARSQ AND KFAOBJ = :KFAOBJ";
        const string insert = @"INSERT INTO  KPIRSOB (
KFATYP, KFAIPB, KFAALX, KFAAVN, KFAHIN
, KFARSQ, KFAOBJ, KFANATS, KFANEGA, KFAFRQE
, KFANBPA, KFANBEX, KFANBVI, KFAGN08, KFAGN09
, KFAGN10, KFANBIN, KFANBPE, KFAGCT, KFANBEM
, KFATYTN, KFANMDF, KFAFENT, KFAFSVT, KFANMSC
, KFALABD, KFANAI, KFALMA, KFAIFP, KFATHF
, KFATU1, KFATU2, KFAASC, KFAAUTL, KFAQMD
, KFASURF, KFAVMC, KFAPROL, KFADEPD
) VALUES (
:KFATYP, :KFAIPB, :KFAALX, :KFAAVN, :KFAHIN
, :KFARSQ, :KFAOBJ, :KFANATS, :KFANEGA, :KFAFRQE
, :KFANBPA, :KFANBEX, :KFANBVI, :KFAGN08, :KFAGN09
, :KFAGN10, :KFANBIN, :KFANBPE, :KFAGCT, :KFANBEM
, :KFATYTN, :KFANMDF, :KFAFENT, :KFAFSVT, :KFANMSC
, :KFALABD, :KFANAI, :KFALMA, :KFAIFP, :KFATHF
, :KFATU1, :KFATU2, :KFAASC, :KFAAUTL, :KFAQMD
, :KFASURF, :KFAVMC, :KFAPROL, :KFADEPD)";
        const string select_GetByAffaire = @"SELECT
KFATYP, KFAIPB, KFAALX, KFAAVN, KFAHIN
, KFARSQ, KFAOBJ, KFANATS, KFANEGA, KFAFRQE
, KFANBPA, KFANBEX, KFANBVI, KFAGN08, KFAGN09
, KFAGN10, KFANBIN, KFANBPE, KFAGCT, KFANBEM
, KFATYTN, KFANMDF, KFAFENT, KFAFSVT, KFANMSC
, KFALABD, KFANAI, KFALMA, KFAIFP, KFATHF
, KFATU1, KFATU2, KFAASC, KFAAUTL, KFAQMD
, KFASURF, KFAVMC, KFAPROL, KFADEPD FROM KPIRSOB
WHERE KFATYP = :KFATYP
and KFAIPB = :KFAIPB
and KFAALX = :KFAALX
";
        #endregion

        public KpIrsObRepository(IDbConnection connection, IdentifierGenerator idGenerator) : base(connection)
        {
            this.idGenerator = idGenerator;
        }

        public KpIrsOb Get(string KFATYP, string KFAIPB, int KFAALX, int KFARSQ, int KFAOBJ)
        {
            return connection.Query<KpIrsOb>(select, new { KFATYP, KFAIPB, KFAALX, KFARSQ, KFAOBJ }).SingleOrDefault();
        }


        public void Insert(KpIrsOb value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("KFATYP", value.Kfatyp ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KFAIPB", value.Kfaipb ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 9, scale: 0);
            parameters.Add("KFAALX", value.Kfaalx, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KFAAVN", value.Kfaavn, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KFAHIN", value.Kfahin, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KFARSQ", value.Kfarsq, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFAOBJ", value.Kfaobj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFANATS", value.Kfanats ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFANEGA", value.Kfanega ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFAFRQE", value.Kfafrqe ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFANBPA", value.Kfanbpa, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 7, scale: 0);
            parameters.Add("KFANBEX", value.Kfanbex, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 7, scale: 0);
            parameters.Add("KFANBVI", value.Kfanbvi, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 7, scale: 0);
            parameters.Add("KFAGN08", value.Kfagn08, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KFAGN09", value.Kfagn09 ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KFAGN10", value.Kfagn10 ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KFANBIN", value.Kfanbin, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 7, scale: 0);
            parameters.Add("KFANBPE", value.Kfanbpe, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 7, scale: 0);
            parameters.Add("KFAGCT", value.Kfagct ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFANBEM", value.Kfanbem, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFATYTN", value.Kfatytn ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("KFANMDF", value.Kfanmdf ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFAFENT", value.Kfafent ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFAFSVT", value.Kfafsvt ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFANMSC", value.Kfanmsc ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFALABD", value.Kfalabd ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFANAI", value.Kfanai, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 8, scale: 0);
            parameters.Add("KFALMA", value.Kfalma, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KFAIFP", value.Kfaifp, dbType: DbType.Decimal, direction: ParameterDirection.Input, size: 6, scale: 3);
            parameters.Add("KFATHF", value.Kfathf ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFATU1", value.Kfatu1 ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFATU2", value.Kfatu2 ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFAASC", value.Kfaasc ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KFAAUTL", value.Kfaautl ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFAQMD", value.Kfaqmd ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KFASURF", value.Kfasurf, dbType: DbType.Decimal, direction: ParameterDirection.Input, size: 11, scale: 2);
            parameters.Add("KFAVMC", value.Kfavmc, dbType: DbType.Decimal, direction: ParameterDirection.Input, size: 11, scale: 2);
            parameters.Add("KFAPROL", value.Kfaprol ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KFADEPD", value.Kfadepd, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);

            connection.EnsureOpened().Execute(insert, parameters);
        }
        public void Delete(KpIrsOb value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("KFATYP", value.Kfatyp);
            parameters.Add("KFAIPB", value.Kfaipb);
            parameters.Add("KFAALX", value.Kfaalx);
            parameters.Add("KFARSQ", value.Kfarsq);
            parameters.Add("KFAOBJ", value.Kfaobj);
            connection.EnsureOpened().Execute(delete, parameters);
        }

        public void Update(KpIrsOb value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("KFATYP", value.Kfatyp ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KFAIPB", value.Kfaipb ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 9, scale: 0);
            parameters.Add("KFAALX", value.Kfaalx, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KFAAVN", value.Kfaavn, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KFAHIN", value.Kfahin, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KFARSQ", value.Kfarsq, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFAOBJ", value.Kfaobj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFANATS", value.Kfanats ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFANEGA", value.Kfanega ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFAFRQE", value.Kfafrqe ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFANBPA", value.Kfanbpa, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 7, scale: 0);
            parameters.Add("KFANBEX", value.Kfanbex, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 7, scale: 0);
            parameters.Add("KFANBVI", value.Kfanbvi, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 7, scale: 0);
            parameters.Add("KFAGN08", value.Kfagn08, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KFAGN09", value.Kfagn09 ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KFAGN10", value.Kfagn10 ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KFANBIN", value.Kfanbin, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 7, scale: 0);
            parameters.Add("KFANBPE", value.Kfanbpe, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 7, scale: 0);
            parameters.Add("KFAGCT", value.Kfagct ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFANBEM", value.Kfanbem, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFATYTN", value.Kfatytn ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("KFANMDF", value.Kfanmdf ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFAFENT", value.Kfafent ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFAFSVT", value.Kfafsvt ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFANMSC", value.Kfanmsc ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFALABD", value.Kfalabd ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFANAI", value.Kfanai, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 8, scale: 0);
            parameters.Add("KFALMA", value.Kfalma, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KFAIFP", value.Kfaifp, dbType: DbType.Decimal, direction: ParameterDirection.Input, size: 6, scale: 3);
            parameters.Add("KFATHF", value.Kfathf ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFATU1", value.Kfatu1 ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFATU2", value.Kfatu2 ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFAASC", value.Kfaasc ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KFAAUTL", value.Kfaautl ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 40, scale: 0);
            parameters.Add("KFAQMD", value.Kfaqmd ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KFASURF", value.Kfasurf, dbType: DbType.Decimal, direction: ParameterDirection.Input, size: 11, scale: 2);
            parameters.Add("KFAVMC", value.Kfavmc, dbType: DbType.Decimal, direction: ParameterDirection.Input, size: 11, scale: 2);
            parameters.Add("KFAPROL", value.Kfaprol ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KFADEPD", value.Kfadepd, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KFATYP", value.Kfatyp);
            parameters.Add("KFAIPB", value.Kfaipb);
            parameters.Add("KFAALX", value.Kfaalx);
            parameters.Add("KFARSQ", value.Kfarsq);
            parameters.Add("KFAOBJ", value.Kfaobj);

            connection.EnsureOpened().Execute(update, parameters);
        }

        public IEnumerable<KpIrsOb> GetByAffaire(string KFATYP, string KFAIPB, int KFAALX)
        {
            return connection.EnsureOpened().Query<KpIrsOb>(select_GetByAffaire, new { KFATYP, KFAIPB, KFAALX }).ToList();
        }
    }
}
