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

    public partial class YpltGarRepository : BaseTableRepository
    {

        private IdentifierGenerator idGenerator;

        #region Query texts        
        //ZALBINKMOD
        const string select = @"SELECT
C2SEQ, C2MGA, C2OBE, C2NIV, C2GAR
, C2ORD, C2LIB, C2SEM, C2CAR, C2NAT
, C2INA, C2CNA, C2TAX, C2ALT, C2TRI
, C2SE1, C2SCR, C2PRP, C2TCD, C2MRF
, C2NTM, C2MAS FROM YPLTGAR
WHERE C2SEQ = :parC2SEQ
";
        const string update = @"UPDATE YPLTGAR SET 
C2SEQ = :C2SEQ, C2MGA = :C2MGA, C2OBE = :C2OBE, C2NIV = :C2NIV, C2GAR = :C2GAR, C2ORD = :C2ORD, C2LIB = :C2LIB, C2SEM = :C2SEM, C2CAR = :C2CAR, C2NAT = :C2NAT
, C2INA = :C2INA, C2CNA = :C2CNA, C2TAX = :C2TAX, C2ALT = :C2ALT, C2TRI = :C2TRI, C2SE1 = :C2SE1, C2SCR = :C2SCR, C2PRP = :C2PRP, C2TCD = :C2TCD, C2MRF = :C2MRF
, C2NTM = :C2NTM, C2MAS = :C2MAS
 WHERE 
C2SEQ = :parC2SEQ";
        const string delete = @"DELETE FROM YPLTGAR WHERE C2SEQ = :parC2SEQ";
        const string insert = @"INSERT INTO  YPLTGAR (
C2SEQ, C2MGA, C2OBE, C2NIV, C2GAR
, C2ORD, C2LIB, C2SEM, C2CAR, C2NAT
, C2INA, C2CNA, C2TAX, C2ALT, C2TRI
, C2SE1, C2SCR, C2PRP, C2TCD, C2MRF
, C2NTM, C2MAS
) VALUES (
:C2SEQ, :C2MGA, :C2OBE, :C2NIV, :C2GAR
, :C2ORD, :C2LIB, :C2SEM, :C2CAR, :C2NAT
, :C2INA, :C2CNA, :C2TAX, :C2ALT, :C2TRI
, :C2SE1, :C2SCR, :C2PRP, :C2TCD, :C2MRF
, :C2NTM, :C2MAS)";
        const string select_GetAll = @"SELECT
C2SEQ, C2MGA, C2OBE, C2NIV, C2GAR
, C2ORD, C2LIB, C2SEM, C2CAR, C2NAT
, C2INA, C2CNA, C2TAX, C2ALT, C2TRI
, C2SE1, C2SCR, C2PRP, C2TCD, C2MRF
, C2NTM, C2MAS FROM YPLTGAR
";
        const string select_ExistTri = @"SELECT
C2SEQ, C2MGA, C2OBE, C2NIV, C2GAR
, C2ORD, C2LIB, C2SEM, C2CAR, C2NAT
, C2INA, C2CNA, C2TAX, C2ALT, C2TRI
, C2SE1, C2SCR, C2PRP, C2TCD, C2MRF
, C2NTM, C2MAS FROM YPLTGAR
WHERE C2SEQ != :seq
and C2MGA = :codeModele
and C2TRI = :tri
";
        const string select_GetByModele = @"SELECT
C2SEQ, C2MGA, C2OBE, C2NIV, C2GAR
, C2ORD, C2LIB, C2SEM, C2CAR, C2NAT
, C2INA, C2CNA, C2TAX, C2ALT, C2TRI
, C2SE1, C2SCR, C2PRP, C2TCD, C2MRF
, C2NTM, C2MAS FROM YPLTGAR
WHERE C2MGA = :codeModele
";
        #endregion

        public YpltGarRepository(IDbConnection connection, IdentifierGenerator idGenerator) : base(connection)
        {
            this.idGenerator = idGenerator;
        }

        public YpltGar Get(Int64 parC2SEQ)
        {
            return connection.Query<YpltGar>(select, new { parC2SEQ }).SingleOrDefault();
        }

        public int NewId()
        {
            return idGenerator.NewId("C2SEQ");
        }

        public void Insert(YpltGar value)
        {
            if (value.C2seq == default(Int64))
            {
                value.C2seq = NewId();
            }
            var parameters = new DynamicParameters();
            parameters.Add("C2SEQ", value.C2seq, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("C2MGA", value.C2mga ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("C2OBE", value.C2obe ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("C2NIV", value.C2niv, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2GAR", value.C2gar ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("C2ORD", value.C2ord, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("C2LIB", value.C2lib ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 60, scale: 0);
            parameters.Add("C2SEM", value.C2sem, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("C2CAR", value.C2car ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2NAT", value.C2nat ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2INA", value.C2ina ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2CNA", value.C2cna ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2TAX", value.C2tax ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("C2ALT", value.C2alt, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("C2TRI", value.C2tri ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 12, scale: 0);
            parameters.Add("C2SE1", value.C2se1, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("C2SCR", value.C2scr ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1500, scale: 0);
            parameters.Add("C2PRP", value.C2prp ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2TCD", value.C2tcd ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("C2MRF", value.C2mrf ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2NTM", value.C2ntm ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2MAS", value.C2mas ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);

            connection.EnsureOpened().Execute(insert, parameters);
        }
        public void Delete(YpltGar value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("parC2SEQ", value.C2seq);
            connection.EnsureOpened().Execute(delete, parameters);
        }

        public void Update(YpltGar value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("C2SEQ", value.C2seq, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("C2MGA", value.C2mga ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("C2OBE", value.C2obe ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("C2NIV", value.C2niv, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2GAR", value.C2gar ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("C2ORD", value.C2ord, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("C2LIB", value.C2lib ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 60, scale: 0);
            parameters.Add("C2SEM", value.C2sem, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("C2CAR", value.C2car ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2NAT", value.C2nat ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2INA", value.C2ina ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2CNA", value.C2cna ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2TAX", value.C2tax ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("C2ALT", value.C2alt, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 2, scale: 0);
            parameters.Add("C2TRI", value.C2tri ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 12, scale: 0);
            parameters.Add("C2SE1", value.C2se1, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("C2SCR", value.C2scr ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1500, scale: 0);
            parameters.Add("C2PRP", value.C2prp ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2TCD", value.C2tcd ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("C2MRF", value.C2mrf ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2NTM", value.C2ntm ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C2MAS", value.C2mas ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("parC2SEQ", value.C2seq);

            connection.EnsureOpened().Execute(update, parameters);
        }

        public IEnumerable<YpltGar> GetAll()
        {
            return connection.EnsureOpened().Query<YpltGar>(select_GetAll).ToList();
        }
        public IEnumerable<YpltGar> ExistTri(Int64 seq, string codeModele, string tri)
        {
            return connection.EnsureOpened().Query<YpltGar>(select_ExistTri, new { seq, codeModele, tri }).ToList();
        }
        public IEnumerable<YpltGar> GetByModele(string codeModele)
        {
            return connection.EnsureOpened().Query<YpltGar>(select_GetByModele, new { codeModele }).ToList();
        }
    }
}
