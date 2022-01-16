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

    public partial class YpoTracRepository : BaseTableRepository
    {

        private IdentifierGenerator idGenerator;

        #region Query texts        
        //ZALBINKHEO
        const string select = @"SELECT
PYTYP, PYIPB, PYALX, PYAVN, PYTTR
, PYVAG, PYORD, PYTRA, PYTRM, PYTRJ
, PYTRH, PYLIB, PYINF, PYSDA, PYSDM
, PYSDJ, PYSFA, PYSFM, PYSFJ, PYMJU
, PYMJA, PYMJM, PYMJJ, PYMJH FROM YPOTRAC
WHERE PYTYP = :parPYTYP
and PYIPB = :parPYIPB
and PYALX = :parPYALX
and PYAVN = :parPYAVN
";
        const string update = @"UPDATE YPOTRAC SET 
PYTYP = :PYTYP, PYIPB = :PYIPB, PYALX = :PYALX, PYAVN = :PYAVN, PYTTR = :PYTTR, PYVAG = :PYVAG, PYORD = :PYORD, PYTRA = :PYTRA, PYTRM = :PYTRM, PYTRJ = :PYTRJ
, PYTRH = :PYTRH, PYLIB = :PYLIB, PYINF = :PYINF, PYSDA = :PYSDA, PYSDM = :PYSDM, PYSDJ = :PYSDJ, PYSFA = :PYSFA, PYSFM = :PYSFM, PYSFJ = :PYSFJ, PYMJU = :PYMJU
, PYMJA = :PYMJA, PYMJM = :PYMJM, PYMJJ = :PYMJJ, PYMJH = :PYMJH
 WHERE 
PYTYP = :parPYTYP and PYIPB = :parPYIPB and PYALX = :parPYALX and PYAVN = :parPYAVN";
        const string delete = @"DELETE FROM YPOTRAC WHERE PYTYP = :parPYTYP AND PYIPB = :parPYIPB AND PYALX = :parPYALX AND PYAVN = :parPYAVN";
        const string insert = @"INSERT INTO  YPOTRAC (
PYTYP, PYIPB, PYALX, PYAVN, PYTTR
, PYVAG, PYORD, PYTRA, PYTRM, PYTRJ
, PYTRH, PYLIB, PYINF, PYSDA, PYSDM
, PYSDJ, PYSFA, PYSFM, PYSFJ, PYMJU
, PYMJA, PYMJM, PYMJJ, PYMJH
) VALUES (
:PYTYP, :PYIPB, :PYALX, :PYAVN, :PYTTR
, :PYVAG, :PYORD, :PYTRA, :PYTRM, :PYTRJ
, :PYTRH, :PYLIB, :PYINF, :PYSDA, :PYSDM
, :PYSDJ, :PYSFA, :PYSFM, :PYSFJ, :PYMJU
, :PYMJA, :PYMJM, :PYMJJ, :PYMJH)";
        const string select_GetByAffaire = @"SELECT
PYTYP, PYIPB, PYALX, PYAVN, PYTTR
, PYVAG, PYORD, PYTRA, PYTRM, PYTRJ
, PYTRH, PYLIB, PYINF, PYSDA, PYSDM
, PYSDJ, PYSFA, PYSFM, PYSFJ, PYMJU
, PYMJA, PYMJM, PYMJJ, PYMJH FROM YPOTRAC
WHERE PYTYP = :PYTYP
and PYIPB = :PYIPB
and PYALX = :PYALX
and PYAVN = :PYAVN
";
        const string select_TraceInfo = @"SELECT
PYTYP, PYIPB, PYALX, PYAVN, PYTTR
, PYVAG, PYORD, PYTRA, PYTRM, PYTRJ
, PYTRH, PYLIB, PYINF, PYSDA, PYSDM
, PYSDJ, PYSFA, PYSFM, PYSFJ, PYMJU
, PYMJA, PYMJM, PYMJJ, PYMJH FROM YPOTRAC
WHERE PYIPB = :parPYIPB
and PYALX = :parPYALX
and PYTYP = :parPYTYP
and PYAVN = :parPYAVN
and PYTTR = :parPYTTR
and PYVAG = :parPYVAG
and PYORD = :parPYORD
and PYMJU = :parPYMJU
and PYTRA = :parPYTRA
and PYTRM = :parPYTRM
and PYTRJ = :parPYTRJ
and PYTRH = :parPYTRH
and PYLIB = :parPYLIB
and PYINF = :parPYINF
and PYMJA = :parPYMJA
and PYMJM = :parPYMJM
and PYMJJ = :parPYMJJ
and PYMJH = :parPYMJH
";
        #endregion

        public YpoTracRepository(IDbConnection connection, IdentifierGenerator idGenerator) : base(connection)
        {
            this.idGenerator = idGenerator;
        }

        public YpoTrac Get(string parPYTYP, string parPYIPB, int parPYALX, int parPYAVN)
        {
            return connection.Query<YpoTrac>(select, new { parPYTYP, parPYIPB, parPYALX, parPYAVN }).SingleOrDefault();
        }


        public void Insert(YpoTrac value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("PYTYP", value.Pytyp ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("PYIPB", value.Pyipb ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 9, scale: 0);
            parameters.Add("PYALX", value.Pyalx, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYAVN", value.Pyavn, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("PYTTR", value.Pyttr ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("PYVAG", value.Pyvag ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("PYORD", value.Pyord, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("PYTRA", value.Pytra, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYTRM", value.Pytrm, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYTRJ", value.Pytrj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYTRH", value.Pytrh, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYLIB", value.Pylib ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 30, scale: 0);
            parameters.Add("PYINF", value.Pyinf ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("PYSDA", value.Pysda, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYSDM", value.Pysdm, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYSDJ", value.Pysdj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYSFA", value.Pysfa, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYSFM", value.Pysfm, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYSFJ", value.Pysfj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYMJU", value.Pymju ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("PYMJA", value.Pymja, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYMJM", value.Pymjm, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYMJJ", value.Pymjj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYMJH", value.Pymjh, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);

            connection.EnsureOpened().Execute(insert, parameters);
        }
        public void Delete(YpoTrac value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("parPYTYP", value.Pytyp);
            parameters.Add("parPYIPB", value.Pyipb);
            parameters.Add("parPYALX", value.Pyalx);
            parameters.Add("parPYAVN", value.Pyavn);
            connection.EnsureOpened().Execute(delete, parameters);
        }

        public void Update(YpoTrac value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("PYTYP", value.Pytyp ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("PYIPB", value.Pyipb ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 9, scale: 0);
            parameters.Add("PYALX", value.Pyalx, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYAVN", value.Pyavn, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("PYTTR", value.Pyttr ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("PYVAG", value.Pyvag ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("PYORD", value.Pyord, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("PYTRA", value.Pytra, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYTRM", value.Pytrm, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYTRJ", value.Pytrj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYTRH", value.Pytrh, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYLIB", value.Pylib ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 30, scale: 0);
            parameters.Add("PYINF", value.Pyinf ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("PYSDA", value.Pysda, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYSDM", value.Pysdm, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYSDJ", value.Pysdj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYSFA", value.Pysfa, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYSFM", value.Pysfm, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYSFJ", value.Pysfj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYMJU", value.Pymju ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("PYMJA", value.Pymja, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("PYMJM", value.Pymjm, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYMJJ", value.Pymjj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("PYMJH", value.Pymjh, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("parPYTYP", value.Pytyp);
            parameters.Add("parPYIPB", value.Pyipb);
            parameters.Add("parPYALX", value.Pyalx);
            parameters.Add("parPYAVN", value.Pyavn);

            connection.EnsureOpened().Execute(update, parameters);
        }

        public IEnumerable<YpoTrac> GetByAffaire(string PYTYP, string PYIPB, int PYALX, int PYAVN)
        {
            return connection.EnsureOpened().Query<YpoTrac>(select_GetByAffaire, new { PYTYP, PYIPB, PYALX, PYAVN }).ToList();
        }
        public IEnumerable<YpoTrac> TraceInfo(string parPYIPB, int parPYALX, string parPYTYP, int parPYAVN, string parPYTTR, string parPYVAG, int parPYORD, string parPYMJU, int parPYTRA, int parPYTRM, int parPYTRJ, int parPYTRH, string parPYLIB, string parPYINF, int parPYMJA, int parPYMJM, int parPYMJJ, int parPYMJH)
        {
            return connection.EnsureOpened().Query<YpoTrac>(select_TraceInfo, new { parPYIPB, parPYALX, parPYTYP, parPYAVN, parPYTTR, parPYVAG, parPYORD, parPYMJU, parPYTRA, parPYTRM, parPYTRJ, parPYTRH, parPYLIB, parPYINF, parPYMJA, parPYMJM, parPYMJJ, parPYMJH }).ToList();
        }
    }
}
