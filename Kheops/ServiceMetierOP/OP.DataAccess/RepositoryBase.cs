using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace OP.DataAccess {
    public abstract class RepositoryBase {
        protected readonly IDbConnection connection;
        protected bool? historyMode;

        public RepositoryBase(IDbConnection connection) {
            this.historyMode = null;
            this.connection = connection;
        }

        public virtual void SetHistoryMode(ActivityMode mode) {
            this.historyMode = mode == ActivityMode.Active ? true : mode == 0 ? default(bool?) : false;
        }

        public virtual void ResetHistoryMode() {
            SetHistoryMode(0);
        }

        internal string FormatQuery(string query) {
            if (query.IsEmptyOrNull()) {
                return query;
            }
            switch (this.historyMode) {
                case false:
                    // forbidding the use of any history table
                    //TODO: prevent selection of histo tables
                    return query;
                case true:
                    query = Regex.Replace(query, @"\bYPRT(?<name>[A-Z0-9]+)\b", "YHRT${name}");
                    query = Regex.Replace(query, @"\bYPO(?<name>[A-Z0-9]+)\b", "YHP${name}");
                    query = Regex.Replace(query, @"\bKP(?<name>[A-Z0-9]+)\b", "HP${name}");
                    return query;
                default:
                    return query;
            }
        }

        protected int GetNextId(string key) => CommonRepository.GetAS400Id(key, this.connection);

        protected IEnumerable<T> Fetch<T>(string statement, params object[] parameters) where T : new() {
            using (var options = new DbSelectOptions(this.connection == null) {
                SqlText = FormatQuery(statement),
                DbConnection = this.connection
            }) {
                if (parameters?.All(p => p is DbParameter) ?? false) {
                    options.Parameters = parameters.Cast<DbParameter>();
                }
                else {
                    options.BuildParameters(parameters);
                    if (parameters?.Any(p => !(p is string) && p is IEnumerable) ?? false) {
                        var dapperParams = new DynamicParameters();
                        options.Parameters.ToList().ForEach(p => dapperParams.Add(p.ParameterName, p.Value));
                        return options.DbConnection.Query<T>(
                            sql: options.SqlText,
                            param: dapperParams,
                            commandType: options.CommandType).ToList();
                    }
                }
                return options.PerformSelect<T>();
            }
        }

        virtual protected void Exec(string sql, params object[] values) {
            using (var options = new DbExecOptions(this.connection == null) {
                SqlText = sql,
                DbConnection = this.connection
            }) {
                options.BuildParameters(values);
                options.Exec();
            }
        }

        virtual protected int ExecSp(string spname, params object[] values) {
            using(var options = new DbSPOptions(this.connection == null) {
                SqlText = spname,
                DbConnection = this.connection
            }) {
                options.Parameters = values.Select((p, x) => new EacParameter($"p{x}", p));
                options.ExecStoredProcedure();
                return options.ReturnedValue;
            }
        }

        virtual protected int ExecSp(string spname, IDictionary<string, object> parameters) {
            using (var options = new DbSPOptions(this.connection == null) {
                SqlText = spname,
                DbConnection = this.connection
            }) {
                options.Parameters = parameters.Select(x => new EacParameter(x.Key, x.Value));
                options.ExecStoredProcedure();
                return options.ReturnedValue;
            }
        }
    }
}
