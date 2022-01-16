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

    public partial class YpltGaaRepository : BaseTableRepository
    {

        private IdentifierGenerator idGenerator;

        #region Query texts        
        //ZALBINKMOD
        const string select = @"SELECT
C5TYP, C5SEQ, C5SEM FROM YPLTGAA
WHERE C5SEQ = :seq1
and C5SEM = :seq2
";
        const string update = @"UPDATE YPLTGAA SET 
C5TYP = :C5TYP, C5SEQ = :C5SEQ, C5SEM = :C5SEM
 WHERE 
C5SEQ = :seq1 and C5SEM = :seq2";
        const string delete = @"DELETE FROM YPLTGAA WHERE C5SEQ = :seq1 AND C5SEM = :seq2";
        const string insert = @"INSERT INTO  YPLTGAA (
C5TYP, C5SEQ, C5SEM
) VALUES (
:C5TYP, :C5SEQ, :C5SEM)";
        const string select_GetAll = @"SELECT
C5TYP, C5SEQ, C5SEM FROM YPLTGAA
";
        const string select_GetLien = @"SELECT
C5TYP AS C5TYP, C5SEQ AS C5SEQ, C5SEM AS C5SEM FROM YPLTGAA
WHERE C5TYP = :type
and C5SEQ = :seq
and C5SEM = :sem
";
        #endregion

        public YpltGaaRepository(IDbConnection connection, IdentifierGenerator idGenerator) : base(connection)
        {
            this.idGenerator = idGenerator;
        }

        public YpltGaa Get(Int64 seq1, Int64 seq2)
        {
            return connection.Query<YpltGaa>(select, new { seq1, seq2 }).SingleOrDefault();
        }


        public void Insert(YpltGaa value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("C5TYP", value.C5typ ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C5SEQ", value.C5seq, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("C5SEM", value.C5sem, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 10, scale: 0);

            connection.EnsureOpened().Execute(insert, parameters);
        }
        public void Delete(YpltGaa value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("seq1", value.C5seq);
            parameters.Add("seq2", value.C5sem);
            connection.EnsureOpened().Execute(delete, parameters);
        }

        public void Update(YpltGaa value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("C5TYP", value.C5typ ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("C5SEQ", value.C5seq, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("C5SEM", value.C5sem, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("seq1", value.C5seq);
            parameters.Add("seq2", value.C5sem);

            connection.EnsureOpened().Execute(update, parameters);
        }

        public IEnumerable<YpltGaa> GetAll()
        {
            return connection.EnsureOpened().Query<YpltGaa>(select_GetAll).ToList();
        }
        public IEnumerable<YpltGaa> GetLien(string type, Int64 seq, Int64 sem)
        {
            return connection.EnsureOpened().Query<YpltGaa>(select_GetLien, new { type, seq, sem }).ToList();
        }
    }
}
