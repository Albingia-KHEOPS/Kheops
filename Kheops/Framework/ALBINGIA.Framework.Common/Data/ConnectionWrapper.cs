using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Tools;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Data
{
    public class ConnectionWrapper : EacConnection, IDisposable, IScopedTransaction
    {
        private Stack<IDbTransaction> trans = new Stack<IDbTransaction>();
        private bool IsDisposed = false;
        private readonly ISuccessIndicator successIndicator;
        private static TelemetryClient telemetryClient;
        private static readonly object syncRoot = new object();

        public ConnectionWrapper() : base()
        {
            InitTelemetry();
        }
        public ConnectionWrapper(string connectionString, ISuccessIndicator success = null) : base(connectionString)
        {
            InitTelemetry();
            this.successIndicator = success;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            var t = new TransactionWrapper(base.BeginTransaction(isolationLevel));
            Trace("beginTran");
            return t;
        }

        [Conditional("DEBUG_TRAN")]
        private void Trace(string st)
        {
            var con = typeof(EacConnection);
            var ass = con.Assembly;
            var iconf = con.GetField("m_connect", BindingFlags.Instance | BindingFlags.NonPublic);
            var iconi = iconf.GetValue(this);
            var icon = ass.GetType("Easycom.Core.Eac_IConnection");
            var trf = icon.GetField("transacStart", BindingFlags.Instance | BindingFlags.NonPublic);
            var hdlf = icon.GetField("m_CnxHdl", BindingFlags.Instance | BindingFlags.NonPublic);
            var txt = $"{DateTime.Now:o} {st} hdl:{(iconi == null ? "(null)" : hdlf.GetValue(iconi))}, transacStart {(iconi == null ? "(null)" : trf.GetValue(iconi))}, tranCount: {trans.Count} ";
            var stt = new StackTrace();
            var frs = stt.GetFrames();
            var fr = frs.Select(x => $"{x.GetMethod()}").Skip(2).Take(10).ToArray();
            System.Diagnostics.Trace.WriteLine(txt + String.Join("/", fr));
        }

        public IEnumerable<IDbTransaction> Transactions => trans;

        public bool IsTransactionShared { get; set; } = true;

        public void CommitTransations()
        {
            if (this.State == ConnectionState.Open)
            {
                while (trans.Count > 0)
                {
                    var t = trans.Pop();
                    t.Commit();
                }
            }
        }

        public void CommitAndRestart()
        {
            CommitTransations();
            AddTransaction(BeginTransaction());
        }

        protected override void Dispose(bool disposing)
        {
            Trace($"Disposing {disposing}");
            if (this.State == ConnectionState.Open)
            {
                if (successIndicator != null)
                {
                    while (trans.Count > 0)
                    {
                        var t = trans.Pop();
                        if (successIndicator.ShouldCommit == true)
                        {
                            t.Commit();
                        }
                        else
                        {
                            t.Rollback();
                        }
                        t.Dispose();
                    }
                }
                this.Close();
            }
            base.Dispose(disposing);
            this.IsDisposed = true;
        }


        private void InitTelemetry()
        {
            if (telemetryClient == default(TelemetryClient))
            {
                lock (syncRoot)
                {
                    if (telemetryClient == default(TelemetryClient))
                    {
                        telemetryClient = new TelemetryClient();
                    }
                }
            }
        }

        public void AddTransaction(IDbTransaction tran)
        {
            trans.Push(tran);
        }
        protected override DbCommand CreateDbCommand()
        {
            var c = new CommandWrapper(this);
            return c;
        }


        private class CommandWrapper : EacCommand
        {
            private ConnectionWrapper eacConnection => (ConnectionWrapper)Connection;

            private void Trace(Stopwatch sw, bool success)
            {
                var req = TelemetryHelper.GetEnv();
                var dt = new DependencyTelemetry()
                {
                    Type = "SQL DB2",
                    Name = this.CommandText,
                    Target = this.DbConnection.Database,
                    Data = this.CommandText,
                    Duration = sw.Elapsed,
                    Timestamp = DateTimeOffset.Now.AddTicks(-sw.ElapsedTicks),
                    Success = success
                };
                dt.Properties.Add("Source", req);
                telemetryClient.TrackDependency(dt);

                //Console.WriteLine($"{sw.ElapsedTicks*1000/Stopwatch.Frequency:.00};\"{dt.Name.Replace("\n"," ").Replace("\r"," ")}\"");
            }

            private T ExecuteAndTrace<T>(Func<T> action)
            {
                if (this.Connection.State == ConnectionState.Closed)
                {
                    this.Connection.Open();
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();
                if (Connection is ConnectionWrapper cw && !cw.IsTransactionShared && this.CommandText.StartsWith(DatabaseExtensions.PrefixPgmAS400))
                {
                    cw.CommitAndRestart();
                }
                bool success = false;
                try
                {
                    var res = action();
                    success = true;
                    return res;
                }
                finally
                {
                    sw.Stop();
                     Trace(sw, success);
                }

            }
            private async Task<T> ExecuteAndTraceAsync<T>(Func<Task<T>> action)
            {
                if (this.Connection.State == ConnectionState.Closed)
                {
                    this.Connection.Open();
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();
                if (Connection is ConnectionWrapper cw && !cw.IsTransactionShared && this.CommandText.StartsWith(DatabaseExtensions.PrefixPgmAS400))
                {
                    cw.CommitAndRestart();
                }
                bool success = false;
                try
                {
                    var res = await action();
                    success = true;
                    return res;
                }
                finally
                {
                    sw.Stop();
                    Trace(sw, success);
                }
            }


            public CommandWrapper() : base()
            {
            }
            public CommandWrapper(EacConnection c) : base(c)
            {
            }
            public CommandWrapper(string txt, EacConnection con, EacTransaction tran) : base(txt, con, tran)
            {
            }
            public CommandWrapper(string txt, EacConnection con) : base(txt, con)
            {
            }
            public CommandWrapper(string txt) : base(txt)
            {
            }
            public override void Prepare()
            {
                try
                {
                    base.Prepare();
                }
                catch
                {
                    Trace(new Stopwatch(), false);
                }
            }
            public override object ExecuteScalar()
            {
                if (eacConnection.IsDisposed)
                {
                    throw new InvalidOperationException("Connection Disposed");
                }
                return this.ExecuteAndTrace(() => base.ExecuteScalar());
            }
            protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
            {
                if (eacConnection.IsDisposed)
                {
                    throw new InvalidOperationException("Connection Disposed");
                }
                return this.ExecuteAndTrace(() => new WrappedEacDataReader((EacDataReader)base.ExecuteDbDataReader(behavior)));
            }
            public override int ExecuteNonQuery()
            {
                if (eacConnection.IsDisposed)
                {
                    throw new InvalidOperationException("Connection Disposed");
                }
                return this.ExecuteAndTrace(() => base.ExecuteNonQuery());
            }
        }

        private class TransactionWrapper : DbTransaction
        {
            private DbTransaction wrapped;

            public TransactionWrapper(DbTransaction tran)
            {
                this.wrapped = tran;
            }

            public override IsolationLevel IsolationLevel => wrapped.IsolationLevel;

            protected override DbConnection DbConnection => wrapped.Connection;

            public override void Commit()
            {
                wrapped.Commit();
                ((ConnectionWrapper)Connection).Trace("Commit");
            }

            public override void Rollback()
            {
                wrapped.Rollback();
                ((ConnectionWrapper)Connection).Trace("Rollback");
            }
        }
        private class WrappedEacDataReader : DbDataReader
        {
            public WrappedEacDataReader(EacDataReader dr)
            {
                this.dr = dr;
            }
            public override object this[int ordinal] => this.GetValue(ordinal);

            public override object this[string name] => this.GetValue(this.GetOrdinal(name));

            public override int Depth => dr.Depth;

            public override int FieldCount => dr.FieldCount;

            public override bool HasRows => dr.HasRows;

            public override bool IsClosed => dr.IsClosed;

            public override int RecordsAffected => dr.RecordsAffected;

            public EacDataReader dr { get; }

            public override bool GetBoolean(int ordinal) => dr.GetBoolean(ordinal);

            public override byte GetByte(int ordinal) => dr.GetByte(ordinal);

            public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) => dr.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);


            public override char GetChar(int ordinal) => dr.GetChar(ordinal);

            public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) => dr.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);


            public override string GetDataTypeName(int ordinal) => dr.GetDataTypeName(ordinal);

            public override DateTime GetDateTime(int ordinal) => dr.GetDateTime(ordinal);

            public override decimal GetDecimal(int ordinal) => dr.GetDecimal(ordinal);


            public override double GetDouble(int ordinal) => dr.GetDouble(ordinal);

            public override IEnumerator GetEnumerator() => new DbEnumerator(this);


            public override Type GetFieldType(int ordinal) => dr.GetFieldType(ordinal);

            public override float GetFloat(int ordinal) => dr.GetFloat(ordinal);

            public override Guid GetGuid(int ordinal) => dr.GetGuid(ordinal);

            public override short GetInt16(int ordinal) => dr.GetInt16(ordinal);

            public override int GetInt32(int ordinal) => dr.GetInt32(ordinal);

            public override long GetInt64(int ordinal) => dr.GetInt64(ordinal);

            public override string GetName(int ordinal) => dr.GetName(ordinal);

            public override int GetOrdinal(string name) => dr.GetOrdinal(name);

            public override string GetString(int ordinal) => dr.GetString(ordinal);

            public override object GetValue(int ordinal) => dr.GetValue(ordinal) ?? System.DBNull.Value;

            public override int GetValues(object[] values)
            {
                int ordinal;
                for (ordinal = 0; ordinal < values.Length && ordinal < this.FieldCount; ordinal++)
                {
                    values[ordinal] = this.GetValue(ordinal);
                }
                return ordinal;
            }

            public override bool IsDBNull(int ordinal) => dr.IsDBNull(ordinal);

            public override bool NextResult() => dr.NextResult();

            public override bool Read() => dr.Read();
        }
    }
}
