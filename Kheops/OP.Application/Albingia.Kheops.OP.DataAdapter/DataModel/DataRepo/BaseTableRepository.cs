using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{
    public class BaseTableRepository
    {
        protected IDbConnection connection;

        public BaseTableRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        protected IDbDataParameter Param(string name, object value)
        {
            var com = this.connection.CreateCommand();
            var p = com.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            return p;
        }
        protected IDbDataParameter OutParam(string name, object value, int size)
        {
            var p = Param(name, value);
            p.Size = size;
            p.Direction = ParameterDirection.Output;
            return p;
        }
    }
}
