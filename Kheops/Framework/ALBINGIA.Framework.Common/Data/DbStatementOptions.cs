using ALBINGIA.Framework.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace ALBINGIA.Framework.Common.Data {
    public abstract class DbStatementOptions : IDisposable {
        private static readonly Regex paramsExpression = new Regex(@"\:(\w+)", RegexOptions.Compiled);

        protected IDbConnection connection;
        protected bool isSelfConnected;
        protected DbStatementOptions(bool isSelfConnected = false) {
            this.isSelfConnected = isSelfConnected;
        }
        public string SqlText { get; set; }
        public virtual CommandType CommandType { get; set; } = CommandType.Text;
        public IEnumerable<IDbDataParameter> Parameters { get; set; }
        public virtual IDbConnection DbConnection {
            get {
                if (isSelfConnected && this.connection == null) {
                    (IDbConnection c, IDbTransaction t) = DatabaseExtensions.InitConnection(openTransaction: false);
                    this.connection = c;
                }
                return this.connection;
            }
            set {
                if (!isSelfConnected && this.connection == null) {
                    this.connection = value;
                }
            }
        }
        public IDbTransaction PrivateTransaction { get; set; }
        public bool UseDapper { get; set; }

        public void Dispose() {
            if (isSelfConnected) {
                this.connection?.Dispose();
            }
        }

        public CommandDefinition ToDapperCommand() {
            return new CommandDefinition(
                SqlText,
                new ConnectionStringSettingsExtensions.DbParams(Parameters),
                commandType: CommandType);
        }

        public void BuildParameters(params object[] parameterValues) {
            if (SqlText.IsEmptyOrNull()) {
                throw new InvalidOperationException("Cannot call this function without SqlText defined");
            }

            List<EacParameter> parameters = new List<EacParameter>();
            var names = GetParamNames(SqlText).ToList();
            if (parameterValues.Length != names.Count()) {
                throw new ArgumentException("Number of parameters does not match", nameof(parameterValues));
            }
            for (var i = 0; i < names.Count; i++) {
                var name = names[i];
                parameters.Add(new EacParameter(name, parameterValues[i]));
            }
            Parameters = parameters;
        }

        private static IEnumerable<string> GetParamNames(string sql) {
            HashSet<string> seen = new HashSet<string>();
            var match = paramsExpression.Match(sql);
            while (match.Success) {
                var name = match.Groups[1].Value;
                if (!seen.Contains(name)) {
                    seen.Add(name);
                    yield return name;
                }
                match = match.NextMatch();
            }
        }
    }
}
