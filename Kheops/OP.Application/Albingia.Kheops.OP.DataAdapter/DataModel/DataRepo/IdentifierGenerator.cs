using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{
    public class IdentifierGenerator
    {
        private IDbConnection connection;

        public IdentifierGenerator(IDbConnection connection)
        {
            this.connection = connection;
        }
        public int NewId(String columnName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("P_CHAMP", columnName, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input);
            parameters.Add("P_SEQ", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            try
            {
                connection.EnsureOpened().Execute("SP_NCHRONO", parameters, commandType: CommandType.StoredProcedure);
            }
            finally
            {
                connection.EnsureClosed();
            }
            var result = parameters.Get<int>("P_SEQ");
            return result;

        }
    }
}
