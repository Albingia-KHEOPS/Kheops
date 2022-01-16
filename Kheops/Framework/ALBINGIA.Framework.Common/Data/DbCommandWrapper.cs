using ALBINGIA.Framework.Common.Tools;
using Easycom;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ALBINGIA.Framework.Common.Data
{
    internal class DbCommandWrapper : DbCommand
    {
        private DbCommand vCmd;
        private TelemetryClient telemetry = new TelemetryClient();

        public DbCommandWrapper(DbCommand vCmd)
        {
            this.vCmd = vCmd;
        }

        public DbCommandWrapper(IDbCommand command) {
            this.vCmd = command as DbCommand;
            if (vCmd == null) {
                throw new ArgumentException("Parameter 'command' must inherit DbCommand");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                vCmd.Dispose();
            }
            base.Dispose(disposing);
        }

        public override string CommandText { get => this.vCmd.CommandText; set => this.vCmd.CommandText = value; }
        public override int CommandTimeout { get => this.vCmd.CommandTimeout; set => this.vCmd.CommandTimeout = value; }
        public override CommandType CommandType { get => this.vCmd.CommandType; set => this.vCmd.CommandType = value; }
        public override bool DesignTimeVisible { get => this.vCmd.DesignTimeVisible; set => this.vCmd.DesignTimeVisible = value; }
        public override UpdateRowSource UpdatedRowSource { get => this.vCmd.UpdatedRowSource; set => this.vCmd.UpdatedRowSource = value; }
        protected override DbConnection DbConnection { get => this.vCmd.Connection; set => this.vCmd.Connection = value; }

        protected override DbParameterCollection DbParameterCollection => this.vCmd.Parameters;

        protected override DbTransaction DbTransaction { get => this.vCmd.Transaction; set => this.vCmd.Transaction = value; }

        public override void Cancel()
        {
            this.vCmd.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            try
            {
                var result = this.vCmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex) when (ex is DbException || ex is EasycomException)
            {
                TraceCommand(nameof(ExecuteNonQuery));
                throw;
            }
        }



        public override object ExecuteScalar()
        {
            try
            {
                var result = this.vCmd.ExecuteScalar();
                return result;
            }
            catch (Exception ex) when (ex is DbException || ex is EasycomException)
            {
                TraceCommand(nameof(ExecuteScalar));
                throw;
            }
        }

        public override void Prepare()
        {
            this.vCmd.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return this.vCmd.CreateParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            try
            {
                var result = this.vCmd.ExecuteReader();
                return result;
            }
            catch (Exception ex) when (ex is DbException || ex is EasycomException)
            {
                TraceCommand(nameof(ExecuteDbDataReader));
                throw;
            }
        }

        private void TraceCommand(string function)
        {
            try
            {
                if (vCmd != null)
                {
                    var logMessage = new StringBuilder();
                    logMessage.AppendLine($"An error occured when executing {function}.");
                    logMessage.AppendLine("Look up message below for further details:");
                    logMessage.AppendLine($"Command: {vCmd.CommandText}");
                    logMessage.Append("Parameters: ");
                    if (vCmd.Parameters.Count > 0)
                    {
                        logMessage.AppendLine();
                        logMessage.AppendLine(String.Join(
                            Environment.NewLine,
                            vCmd.Parameters.Cast<DbParameter>().Select(p => $"{p.ParameterName} {p.Direction} {DecodeParamType(p)} {p.Value ?? "[null]"}")));
                    }
                    else
                    {
                        logMessage.AppendLine("None");
                    }

                    AlbLog.Log(logMessage.ToString(), AlbLog.LogTraceLevel.Erreur);
                }
            }
            catch { }
        }

        private string DecodeParamType(DbParameter p)
        {
            switch (p.DbType)
            {
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Single:
                case DbType.VarNumeric:
                    return $"{p.DbType}({p.Scale},{p.Precision})";
                case DbType.AnsiString:
                case DbType.String:
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                    return $"{p.DbType}({p.Size})";
                default:
                    return p.DbType.ToString();
            }
        }
    }
}