using ALBINGIA.Framework.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess {
    public class ConnectionContext : IDisposable {
        private readonly IDbConnection connection;
        private readonly IDbTransaction ownTransaction;
        internal static readonly string GetNextIdSP = "SP_NCHRONO";
        public ConnectionContext(IDbConnection connection = null) {
            this.connection = connection;
            TransactionEnded = null;
            if (this.connection == null) {
                TransactionEnded = false;
                try {
                    var (cn, t) = InitSeparateConnection();
                    this.connection = cn;
                    this.ownTransaction = t;
                }
                catch {
                    this.connection = null;
                    this.ownTransaction = null;
                    TransactionEnded = null;
                }
            }
            Now = DateTime.Now;
        }

        public bool? TransactionEnded { get; private set; }
        public bool HasItsOwnConnection { get => TransactionEnded.HasValue; }
        public DateTime Now { get; private set; }

        private static (IDbConnection cn, IDbTransaction t) InitSeparateConnection(bool openTransaction = true) {
            return DatabaseExtensions.InitConnection(true, openTransaction);
        }

        internal int GetNextId(string columnName) {
            var outParam = new EacParameter("P_SEQ", DbType.Int32) { Value = 0, Direction = ParameterDirection.InputOutput };
            ExecuteSP(GetNextIdSP, new[] {
                new EacParameter("P_CHAMP", DbType.AnsiStringFixedLength, 40) { Value = columnName },
                outParam
            });
            return Convert.ToInt32(outParam.Value);
        }

        internal bool IsConnectionAvailable(ConnectionState expectedConnectionState = ConnectionState.Open) {
            return this.connection != null && this.connection.State == expectedConnectionState;
        }

        public IEnumerable<T> Select<T>(string query, IEnumerable<DbParameter> parameters = null) where T : new() {
            if (IsConnectionAvailable()) {
                var options = new DbSelectOptions {
                    CommandType = CommandType.Text,
                    SqlText = query,
                    DbConnection = this.connection,
                    Parameters = parameters ?? Enumerable.Empty<DbParameter>(),
                    AllowMissingColumnMappings = true,
                    PrivateTransaction = this.ownTransaction
                };
                return options.PerformSelect<T>();
            }
            return Enumerable.Empty<T>();
        }

        public IEnumerable<int> SelectInts(string query, IEnumerable<DbParameter> parameters = null) {
            if (IsConnectionAvailable()) {
                var options = new DbSelectInt32Options {
                    CommandType = CommandType.Text,
                    SqlText = query,
                    DbConnection = connection,
                    Parameters = parameters ?? Enumerable.Empty<DbParameter>(),
                    AllowMissingColumnMappings = true,
                    PrivateTransaction = this.ownTransaction
                };
                options.PerformSelect();
                return options.IntegerList.ToList();
            }
            return Enumerable.Empty<int>();
        }

        public T? SelectFirstField<T>(string query, IEnumerable<DbParameter> parameters = null) where T : struct {
            if (IsConnectionAvailable()) {
                var options = new DbSelectOptions {
                    CommandType = CommandType.Text,
                    SqlText = query,
                    DbConnection = this.connection,
                    Parameters = parameters ?? Enumerable.Empty<DbParameter>(),
                    AllowMissingColumnMappings = true,
                    PrivateTransaction = this.ownTransaction
                };
                return options.PerformGetFirstField<T>();
            }
            return default(T?);
        }

        public int Count(string query, IEnumerable<DbParameter> parameters = null) {
            if (IsConnectionAvailable()) {
                var options = new DbCountOptions {
                    CommandType = CommandType.Text,
                    SqlText = query,
                    DbConnection = connection,
                    Parameters = parameters ?? Enumerable.Empty<DbParameter>(),
                    PrivateTransaction = this.ownTransaction
                };
                options.PerformCount();
                return options.Count;
            }
            return 0;
        }

        public void Execute(string statement, IEnumerable<DbParameter> parameters = null) {
            if (IsConnectionAvailable()) {
                var options = new DbExecOptions {
                    CommandType = CommandType.Text,
                    SqlText = statement,
                    DbConnection = connection,
                    Parameters = parameters ?? Enumerable.Empty<DbParameter>(),
                    PrivateTransaction = this.ownTransaction
                };
                options.Exec();
            }
        }

        public void Execute(IDictionary<string, DbParameter[]> statements, bool useDecicatedConnection = false) {
            if (IsConnectionAvailable() || useDecicatedConnection) {
                (IDbConnection cnt, IDbTransaction trn) = useDecicatedConnection ? InitSeparateConnection(false) : (this.connection, this.ownTransaction);
                try {
                    var optionsList = statements.Select(x => new DbExecOptions {
                        CommandType = CommandType.Text,
                        SqlText = x.Key,
                        DbConnection = cnt,
                        Parameters = x.Value ?? Enumerable.Empty<DbParameter>(),
                        PrivateTransaction = trn
                    });
                    optionsList.ExecuteMultiple();
                }
                finally {
                    if (useDecicatedConnection) {
                        if (cnt.State == ConnectionState.Open) {
                            cnt.Close();
                        }
                        cnt.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Executes multiple statements, each time using the same parameters if <paramref name="globalParameters"/> is not NULL
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="globalParameters"></param>
        public void Execute(IEnumerable<string> statements, IEnumerable<DbParameter> globalParameters = null, bool useDecicatedConnection = false) {
            Execute(statements.ToDictionary(s => s, s => globalParameters?.ToArray()), useDecicatedConnection);
        }

        public int ExecuteSP(string spName, IEnumerable<DbParameter> parameters = null) {
            if (IsConnectionAvailable()) {
                var options = new DbSPOptions {
                    SqlText = spName,
                    DbConnection = connection,
                    Parameters = parameters ?? Enumerable.Empty<DbParameter>(),
                    PrivateTransaction = this.ownTransaction
                };
                options.ExecStoredProcedure();
                return options.ReturnedValue;
            }
            return default(int);
        }

        public void EndTransaction(bool commit) {
            if (this.ownTransaction != null) {
                if (commit) {
                    this.ownTransaction.Commit();
                }
                else {
                    this.ownTransaction.Rollback();
                }
                TransactionEnded = true;
            }
        }

        public void Dispose() {
            if (TransactionEnded.HasValue) {
                if (this.ownTransaction != null) {
                    if (TransactionEnded == false) {
                        // default final action is commit
                        this.ownTransaction.Commit();
                    }
                    this.ownTransaction.Dispose();
                }
                if (IsConnectionAvailable()) {
                    this.connection.Close();
                }
                this.connection.Dispose();
            }
        }
    }
}
