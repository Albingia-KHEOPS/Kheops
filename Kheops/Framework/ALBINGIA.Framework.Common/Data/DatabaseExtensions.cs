using ALBINGIA.Framework.Common.Data.Dapper;
using ALBINGIA.Framework.Common.Data.Mapping;
using ALBINGIA.Framework.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Data {
    public static class DatabaseExtensions {
        public const string PrefixPgmAS400 = "*PGM/";
        private static readonly HashSet<Type> mappedTypes = new HashSet<Type>();
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["EasyCom"].ConnectionString;

        public static (IDbConnection cn, IDbTransaction t) InitConnection(bool openConnection = true, bool openTransaction = true) {
            var cn = new ConnectionWrapper(connectionString, new SuccessIndicator() { ShouldCommit = true });
            EacTransaction tr = null;
            if (openConnection || openTransaction) {
                cn.Open();
            }
            if (openTransaction) {
                tr = cn.BeginTransaction();
                cn.AddTransaction(tr);
            }
            return (cn, tr);
        }

        public static IEnumerable<T> PerformSelect<T>(this DbSelectOptions selectOptions) where T : new() {
            if (selectOptions.UseDapper) {
                return DapperQuery<T>(selectOptions);
            }
            else {
                using (var dbCommand = CreateCommand(selectOptions)) {
                    AttachEasycomParameters(dbCommand, selectOptions.Parameters.OfType<EacParameter>());
                    dbCommand.Prepare();

                    var selection = Activator.CreateInstance<List<T>>();
                    using (DbDataReader dataReader = dbCommand.ExecuteReader()) {
                        if (typeof(T).IsValueTuple()) {
                            selection.AddRange(BuildValueTuples<T>(selectOptions, dataReader));
                        }
                        else {
                            var colunmsMapping = GetEntityInformations(typeof(T), dataReader, selectOptions.AllowMissingColumnMappings);
                            selection.AddRange(BuildEntities<T>(selectOptions, dataReader, colunmsMapping));
                        }
                    }
                    return selection;
                }
            }
        }

        private static IEnumerable<T> BuildEntities<T>(DbSelectOptions selectOptions, DbDataReader dataReader, ColumnExpression[] colunmsMapping) where T : new() {
            var properties = Activator.CreateInstance<T>().GetType().GetProperties().Where(p => selectOptions.AllowMissingColumnMappings || ((ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), true)).Length != 0).ToArray();
            if (colunmsMapping.Length > 0) {
                var ordinals = new int[colunmsMapping.Length];
                for (var index = 0; index < colunmsMapping.Length; ++index) {
                    ordinals[index] = colunmsMapping[index].Ordinal;
                }
                while (dataReader.Read()) {
                    var entity = Activator.CreateInstance<T>();
                    foreach (var col in colunmsMapping) {

                        object value = dataReader[col.Name];
                        value = PreCastValue(properties[col.Ordinal], value);

                        properties[col.Ordinal].SetValue(entity, value == DBNull.Value ? null : value, null);
                    }
                    yield return entity;
                }
            }
        }

        private static IEnumerable<T> BuildValueTuples<T>(DbSelectOptions selectOptions, DbDataReader dataReader) where T : new() {
            Type type = Activator.CreateInstance<T>().GetType();
            var fields = type
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField)
                .OrderBy(f => f.Name)
                .ToArray();

            ConstructorInfo ctor = type.GetConstructor(fields.Select(f => f.FieldType).ToArray());
            while (dataReader.Read()) {
                var values = new object[fields.Length];
                for (int x = 0; x < fields.Length; x++) {
                    object value = PreCastValue(fields[x], dataReader[x]);
                    values[x] = value == DBNull.Value ? null : value;
                }
                yield return (T)ctor.Invoke(values);
            }
        }

        public static T? PerformGetFirstField<T>(this DbSelectOptions selectOptions) where T : struct {
            using (var dbCommand = CreateCommand(selectOptions)) {
                AttachEasycomParameters(dbCommand, selectOptions.Parameters.OfType<EacParameter>());
                object result = dbCommand.ExecuteScalar();
                if (result is T t) {
                    return t;
                }
            }

            return null;
        }

        public static void PerformSelect(this DbSelectStringsOptions selectOptions) {
            var list = new List<string>();
            using (var dbCommand = CreateCommand(selectOptions)) {
                AttachEasycomParameters(dbCommand, selectOptions.Parameters.OfType<EacParameter>());
                dbCommand.Prepare();

                using (DbDataReader dataReader = dbCommand.ExecuteReader()) {
                    while (dataReader.Read()) {
                        list.Add(dataReader.GetString(0));
                    }
                }
            }

            selectOptions.StringList = list;
        }

        public static void PerformSelect(this DbSelectInt32Options selectOptions) {
            var list = new List<int>();
            using (var dbCommand = CreateCommand(selectOptions)) {
                AttachEasycomParameters(dbCommand, selectOptions.Parameters.OfType<EacParameter>());
                dbCommand.Prepare();

                using (DbDataReader dataReader = dbCommand.ExecuteReader()) {
                    while (dataReader.Read()) {
                        list.Add(Convert.ToInt32(dataReader.GetValue(0)));
                    }
                }
            }

            selectOptions.IntegerList = list;
        }

        public static void PerformCount(this DbCountOptions countOptions) {
            object result = null;
            using (var dbCommand = CreateCommand(countOptions)) {
                AttachEasycomParameters(dbCommand, countOptions.Parameters.OfType<EacParameter>());
                dbCommand.Prepare();
                result = dbCommand.ExecuteScalar();
            }
            countOptions.Count = result == null ? 0 : Convert.ToInt32(result);
        }

        public static void ExecStoredProcedure(this DbSPOptions options) {
            if (options.DbConnection is ConnectionWrapper cn && !cn.Transactions.Any()) {
                cn.AddTransaction(cn.BeginTransaction());
            }
            using (var dbCommand = CreateCommand(options)) {
                AttachEasycomSPParameters(dbCommand, options.Parameters.OfType<EacParameter>());
                dbCommand.Prepare();
                options.ReturnedValue = dbCommand.ExecuteNonQuery();
            }
        }

        public static IEnumerable<T> SelectCursor<T>(this DbSPOptions options) where T : new() {
            if (options.UseDapper) {
                return DapperQuery<T>(options);
            }
            else {
                var selectOptions = options.ToSelectOptions();
                using (var dbCommand = CreateCommand(selectOptions)) {
                    AttachEasycomSPParameters(dbCommand, selectOptions.Parameters.OfType<EacParameter>());
                    dbCommand.Prepare();

                    var selection = Activator.CreateInstance<List<T>>();
                    using (DbDataReader dataReader = dbCommand.ExecuteReader()) {
                        if (typeof(T).IsValueTuple()) {
                            selection.AddRange(BuildValueTuples<T>(selectOptions, dataReader));
                        }
                        else {
                            var colunmsMapping = GetEntityInformations(typeof(T), dataReader, selectOptions.AllowMissingColumnMappings);
                            selection.AddRange(BuildEntities<T>(selectOptions, dataReader, colunmsMapping));
                        }
                    }
                    return selection;
                }
            }
        }

        public static void Exec(this DbExecOptions execOptions) {
            using (var dbCommand = CreateCommand(execOptions)) {
                AttachEasycomParameters(dbCommand, execOptions.Parameters.OfType<EacParameter>());
                dbCommand.Prepare();
                execOptions.ReturnedValue = dbCommand.ExecuteNonQuery();
            }
        }

        public static void ExecuteMultiple(this IEnumerable<DbExecOptions> execOptionsList) {
            execOptionsList?.ToList()?.ForEach(x => x.Exec());
        }

        internal static DbCommand CreateCommand(DbStatementOptions options) {
            var dbCommand = options.DbConnection.CreateCommand();
            dbCommand.Connection = options.DbConnection;
            dbCommand.CommandType = options.CommandType;
            dbCommand.CommandText = options.SqlText;

            // easycom fix transaction
            if (options.PrivateTransaction != null) {
                dbCommand.Transaction = options.PrivateTransaction;
            }
            else if (options.DbConnection is ConnectionWrapper wrapper) {
                dbCommand.Transaction = wrapper.Transactions.LastOrDefault();
            }

            var commandWrapper = new DbCommandWrapper(dbCommand);
            return commandWrapper;
        }

        internal static void AttachEasycomParameters(DbCommand dbCommand, IEnumerable<EacParameter> parameters) {
            if (dbCommand.CommandType == CommandType.StoredProcedure) {
                AttachEasycomSPParameters(dbCommand, parameters);
            }
            else {
                foreach (var eacParam in parameters.Where(p => p != null)) {
                    var paramDirection = eacParam.Direction;
                    if (eacParam.Value == null && (paramDirection == ParameterDirection.InputOutput || paramDirection == ParameterDirection.Input)) {
                        eacParam.Value = DBNull.Value;
                    }

                    dbCommand.Parameters.Add(eacParam);
                }
            }
        }

        internal static void AttachEasycomSPParameters(DbCommand dbCommand, IEnumerable<EacParameter> parameters) {
            Dictionary<string, EacParameter> eacParameters = ConnectionStringSettingsExtensions.GetParametersPrototype(dbCommand);
            foreach (var eacParam in parameters.Where(p => p != null)) {
                var paramDirection = eacParam.Direction;
                if (eacParam.Value == null && (paramDirection == ParameterDirection.InputOutput || paramDirection == ParameterDirection.Input)) {
                    eacParam.Value = DBNull.Value;
                }

                //HACK problèmes de nommage de paramètres
                var name = eacParam.ParameterName;
                if (name.StartsWith("P0") && !eacParameters.ContainsKey(name)) {
                    name = "PO" + name.Substring(2);
                }

                if (eacParameters.ContainsKey(name.ToUpper())) {
                    var tmp = eacParameters[name.ToUpper()];
                    eacParam.EacType = tmp.EacType;
                    eacParam.IsNullable = tmp.IsNullable;
                    eacParam.SetPrecision(tmp.Precision);
                    eacParam.SetSize(tmp.Size);
                    eacParam.SetScale(tmp.Scale);
                    dbCommand.Parameters.Add(eacParam);
                }
            }
        }

        internal static ColumnExpression[] GetEntityInformations(Type entityType, DbDataReader dataReader, bool allowMissingMappings, bool ignoreCase = true) {
            var infoList = new List<ColumnExpression>();
            int nb = 0;
            var properties = entityType
                .GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance)
                .ToDictionary(p => {
                    var attribute = p.GetCustomAttributes<ColumnAttribute>(true).FirstOrDefault();
                    return new {
                        ordinal = nb++,
                        column = attribute,
                        name = attribute != null ? attribute.Name : p.Name
                    };
                });

            for (int i = 0; i < dataReader.FieldCount; i++) {
                var pkv = properties.FirstOrDefault(kv => string.Compare(kv.Key.name, dataReader.GetName(i), ignoreCase) == 0);
                var attribute = pkv.Key.column;
                if (allowMissingMappings || attribute != null) {
                    infoList.Add(new ColumnExpression {
                        CanBeNull = attribute == null ? false : attribute.CanBeNull,
                        IsPrimaryKey = attribute == null ? false : attribute.IsPrimaryKey,
                        IsDbGenerated = attribute == null ? false : attribute.IsDbGenerated,
                        Name = attribute == null ? pkv.Key.name : attribute.Name,
                        Expression = attribute == null ? string.Empty : attribute.Expression,
                        PropertyName = attribute == null ? pkv.Key.name : attribute.Name,
                        Ordinal = pkv.Key.ordinal,
                        PropInfo = pkv.Value,
                        TypeInfo = pkv.Value.PropertyType
                    });
                }
            }

            return infoList.ToArray();
        }

        private static object PreCastValue(MemberInfo memberInfos, object value) {
            Type type = null;
            switch (memberInfos.MemberType) {
                case MemberTypes.Field:
                    type = ((FieldInfo)memberInfos).FieldType;
                    break;
                case MemberTypes.Property:
                    type = ((PropertyInfo)memberInfos).PropertyType;
                    break;
            }
            if (value != DBNull.Value) {
                if (type == typeof(Decimal) && value.GetType() == typeof(double)) {
                    value = Convert.ToDecimal(value);
                }
                else if (type == typeof(Int32)) {
                    value = Convert.ToInt32(value);
                }
                else if (type == typeof(Boolean)) {
                    if (value is string) {
                        if (Boolean.TryParse((string)value, out bool b)) {
                            value = b;
                        }
                        else if (value.Equals("O") || value.Equals("N")) {
                            value = value.Equals("O");
                        }
                    }
                    else {
                        value = Convert.ChangeType(value, typeof(Boolean));
                    }
                }
                else if (type == typeof(char) && value is string) {
                    value = ((string)value)[0];
                }
                else if (type.IsEnum) {
                    var ev = Enum.GetValues(type)
                        .Cast<Enum>()
                        .FirstOrDefault(en => en.GetAttributeOfType<DisplayAttribute>()?.Name == value as string);

                    value = ev;
                }
            }

            return value;
        }

        private static IEnumerable<T> DapperQuery<T>(DbStatementOptions options) {
            Type type = typeof(T);
            if (!mappedTypes.Contains(type)) {
                SqlMapper.SetTypeMap(type, new ColumnAttributeTypeMapper<T>());
                mappedTypes.Add(type);
            }
            return options.DbConnection.Query<T>(options.ToDapperCommand()).ToList();
        }
    }
}
