using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel {
    public class As400ProgramsRepository : Repository.BaseTableRepository {
        const string newid = @"*PGM/KA0001";

        public As400ProgramsRepository(IDbConnection connection) : base(connection) {
        }

        public int GetNewChrono(string key) {
            int result = -1;
            using (var command = this.connection.EnsureOpened().CreateCommand()) {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = newid;
                new Dictionary<string, object> { ["P0CLE"] = key, ["P0ACT"] = "INC" }
                    .Select(x => { return MakeParam(x.Key, x.Value, command); })
                    .ToList()
                    .ForEach(x => command.Parameters.Add(x));

                var resultParam = MakeParam("P0NUU", 0, command, ParameterDirection.InputOutput);
                var returnParam = MakeParam("P0RET", " ", command, ParameterDirection.InputOutput);
                command.Parameters.Add(resultParam);
                command.Parameters.Add(returnParam);
                command.ExecuteNonQuery();
                if (returnParam.Value is string val && val.Trim().Length == 0) {
                    result = int.Parse(resultParam.Value.ToString());
                }
            }
            return result;
        }

        private static IDbDataParameter MakeParam(string name, object value, IDbCommand command, ParameterDirection dir = ParameterDirection.Input, int? length = null) {
            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            p.Direction = dir;
            if (value?.GetType() == typeof(string)) {
                p.DbType = DbType.AnsiStringFixedLength;
            }
            if (length.HasValue) {
                p.Size = length.Value;
                if (p is EacParameter eac) {
                    eac.SetSize(length.Value);
                }

            }
            return p;
        }
    }
}
