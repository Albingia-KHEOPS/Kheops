using ALBINGIA.Framework.Common.Data.Dapper;
using ALBINGIA.Framework.Common.Data.Mapping;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Dapper;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ALBINGIA.Framework.Common.Data
{
    /// <summary>
    /// La classe ConnectionStringSettingsExtensions est une encapsulation de ADO.NET 
    /// et rajoute des methodes d'extensions sur la classe ConnectionStringSettings
    /// </summary>
    public static class ConnectionStringSettingsExtensions
    {

        //static ConnectionStringSettingsExtensions() {
        //    SqlMapper.AddTypeHandler<Nullable<int16>>(new TypeHandler());
        //}
        private static TelemetryClient telemetry = new TelemetryClient();

        // Dictionnaire contenant les paramètres des procédures stockées
        private static Dictionary<string, Dictionary<string, EacParameter>> paramsCache;
        private static Dictionary<string, Dictionary<string, EacParameter>> programsParamsCache;
        private static readonly object paramCacheLock = new object();

        /// <summary>
        /// Defines AS400 schemas for each environment username
        /// </summary>
        internal static readonly Dictionary<string, string> EnvironmentSchemas = new Dictionary<string, string>()
        {
            { "ZKDEV", "ZALBINKDEV" },
            { "ZFORM", "ZALBINKFRM" },
            { "KPRE", "ZALBINKPPR" },
            { "KPROD", "YALBINFILE" },
            { "ZKALIF", "ZALBINKQUA" },
            { "ZKFIX", "ZALBINKPPR" }
        };

        //Dictionnaire pour le mapping des types entre ceux de la base et ceux d'EasyCom
        private static readonly Dictionary<string, Easycom.EasycomType> paramTypeMapping = new Dictionary<string, Easycom.EasycomType>
        {
            {"VARCHAR () FOR BIT DATA", Easycom.EasycomType.VarChar },
            {"CHAR", Easycom.EasycomType.Char },
            {"INTEGER", Easycom.EasycomType.Int },
            {"VARCHAR", Easycom.EasycomType.VarChar },
            {"BLOB", Easycom.EasycomType.Blob },
            {"DECIMAL", Easycom.EasycomType.Decimal },
            {"NUMERIC", Easycom.EasycomType.Decimal },
            {"SMALLINT", Easycom.EasycomType.Short },
            {"NCLOB", Easycom.EasycomType.Clob },
            {"BIGINT", Easycom.EasycomType.Int64 },
            {"VARBINARY", Easycom.EasycomType.Binary },
            {"CLOB", Easycom.EasycomType.Clob },
            {"TIMESTAMP", Easycom.EasycomType.TimeStamp},
        };
        private static readonly Dictionary<int, Easycom.EasycomType> programParamMapping = new Dictionary<int, Easycom.EasycomType>
        {
            {0, Easycom.EasycomType.Char },
            {1, Easycom.EasycomType.Short },
            {2, Easycom.EasycomType.Float },
            {3, Easycom.EasycomType.Int },
            {4, Easycom.EasycomType.Double }
        };

        private static void Trace(Stopwatch sw, DbCommand command, int count)
        {
            string text = command.CommandText;
            if (System.Diagnostics.Debugger.IsAttached)
            {
                var action = System.ServiceModel.OperationContext.Current.IncomingMessageHeaders.Action;
                Debug.WriteLine($"DB2 : {action} - {DateTime.Now:s} : {sw.ElapsedMilliseconds} - {text.Replace("\n", " ").Substring(0, text.Length > 100 ? 100 : text.Length)}");
            }
            var req = TelemetryHelper.GetEnv();
            var dt = new DependencyTelemetry() {
                Type = "SQL DB2 Fetch",
                Name = text.Length > 50 ? text.Substring(0, 50) : text,
                Target = command.Connection.Database,
                Data = text,
                Duration = sw.Elapsed,
                Timestamp = DateTimeOffset.Now.AddMilliseconds(-sw.ElapsedMilliseconds)
            };
            dt.Properties.Add("RowCount", count.ToString());
            dt.Properties.Add("Source", req);
            telemetry.TrackDependency(dt);
        }

        #region DbConnectionOwnership
        /// <summary>
        /// Cette énumération est utilisée pour indiquer si la connexion a été fourni par l'appelant,
        /// Ou créés par DbHelper, de telle sorte que
        /// Nous pouvons définir la CommandBehavior appropriée lors de l'appel ExecuteReader ()
        /// </summary>
        private enum DbConnectionOwnership
        {
            /// <summary>Connection is owned and managed by DbHelper</summary>
            Internal,
            /// <summary>Connection is owned and managed by the caller</summary>
            External
        }
        #endregion


        /// <summary>
        /// Determines the current schema (libray) name for a given connection
        /// </summary>
        /// <param name="connection">The IDbConnection which has to be open</param>
        /// <param name="deduceFromUserId">Defines whether the schema is deduced from the user id</param>
        /// <returns></returns>
        internal static string GetCurrentSchemaName(this IDbConnection connection, bool deduceFromUserId = true)
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                return null;
            }

            string username = null;
            if (deduceFromUserId)
            {
                try
                {
                    username = connection.ConnectionString.Split(';').First(s => s.StartsWith("user id=", StringComparison.InvariantCultureIgnoreCase)).Split('=')[1];
                }
                catch { }
                finally
                {
                    if (username.IsEmptyOrNull())
                    {
                        deduceFromUserId = false;
                    }
                }
            }

            if (!deduceFromUserId || !EnvironmentSchemas.TryGetValue(username.ToUpper(), out string schemaName))
            {
                var cnn = connection as DbConnection;
                if (cnn == null)
                {
                    return null;
                }

                schemaName = cnn.GetSchema().Rows
                    .Cast<DataRow>()
                    .Select(r => r["table_actual_library"] as string)
                    .FirstOrDefault(name => name != null && EnvironmentSchemas.Values.Contains(name.ToUpper()));
            }

            return schemaName;
        }

        #region Private utility methods

        /// <summary>
        /// Cette méthode est utilisée pour initialiser le cache de paramètres lié aux procédures stockées
        /// </summary>
        /// <param name="connection">Un DbConnection valide, sur laquelle exécuter cette p_cmd</param>
        /// <returns>
        /// Une nouvelle instance du dictionnaire paramsCache agissant comme cache 
        /// pour les paramètres des procédures stockées
        /// </returns>
        private static void InitParamsCache(DbConnection connection)
        {
            var dico = new Dictionary<string, Dictionary<string, EacParameter>>();
            var programDico = new Dictionary<string, Dictionary<string, EacParameter>>();
            var query = "SELECT PROCEDURE_NAME, COLUMN_NAME, TYPE_NAME, COLUMN_SIZE, " +
                "DECIMAL_DIGITS, IS_NULLABLE, ORDINAL_POSITION " +
                "FROM SYSIBM/SQLPROCEDURECOLS " +
                "WHERE PROCEDURE_SCHEM=:schemaName";

            var queryProgram = "SELECT PROCNAME, FIELDNAME, PCTYPE, PCLEN, PCDEC FROM EASYCOM/YPROCPARMS";

            if (connection != null)
            {
                if (connection.State != ConnectionState.Open)
                {
                    // Open the connection.
                    connection.Open();
                }

                // Create commands.
                //Cmd Stored Procedures
                var cmdSp = connection.CreateCommand();
                cmdSp.Connection = connection;
                cmdSp.CommandText = query;
                cmdSp.CommandType = CommandType.Text;
                var cmdParam = cmdSp.CreateParameter();
                cmdParam.ParameterName = "schemaName";
                cmdParam.Value = connection.GetCurrentSchemaName();
                cmdSp.Parameters.Add(cmdParam);
                //Cmd Programs
                var cmdProgram = connection.CreateCommand();
                cmdProgram.Connection = connection;
                cmdProgram.CommandText = queryProgram;
                cmdProgram.CommandType = CommandType.Text;

                // Retrieve the data.
                DbDataReader dr = cmdSp.ExecuteReader();

                string spName = null;

                while (dr.Read())
                {
                    spName = dr.GetString(0);

                    var param = new EacParameter();

                    param.ParameterName = dr.GetString(1);
                    param.IsNullable = (dr.GetString(5) == "YES");
                    param.EacType = paramTypeMapping[dr.GetString(2)];
                    param.SetSize(dr.GetInt32(3));
                    if (!dr.IsDBNull(4))
                    {
                        param.SetPrecision((byte)param.Size);
                        param.SetScale((byte)dr.GetInt16(4));
                    }

                    if (dico.ContainsKey(spName))
                    {
                        if (!dico[spName].ContainsKey(param.ParameterName))
                        {
                            dico[spName].Add(param.ParameterName.ToUpper(), param);
                        }
                    }
                    else
                    {
                        var subDico = new Dictionary<string, EacParameter>();
                        subDico.Add(param.ParameterName, param);
                        dico.Add(spName, subDico);
                    }
                }

                DbDataReader drProg = cmdProgram.ExecuteReader();

                string programName = null;

                while (drProg.Read())
                {
                    programName = drProg.GetString(0);

                    var param = new EacParameter();

                    param.ParameterName = drProg.GetString(1);
                    param.EacType = programParamMapping[drProg.GetInt16(2)];
                    param.SetSize(drProg.GetInt16(3));
                    if (!drProg.IsDBNull(4))
                    {
                        param.SetPrecision((byte)param.Size);
                        param.SetScale((byte)drProg.GetInt16(4));
                    }
                    param.IsNullable = true;
                    if (programDico.ContainsKey(programName))
                    {
                        if (!programDico[programName].ContainsKey(param.ParameterName))
                        {
                            programDico[programName].Add(param.ParameterName, param);
                        }
                    }
                    else
                    {
                        var subDico = new Dictionary<string, EacParameter>();
                        subDico.Add(param.ParameterName.ToUpper(), param);
                        programDico.Add(programName, subDico);
                    }
                }
            }
            paramsCache = dico;
            programsParamsCache = programDico;
        }

        /// <summary>
        /// Cette méthode est utilisée pour attacher des p_array DbParameters à DbCommand.
        ///
        /// Cette méthode attribue un v_value de DBNull à toute v_param avec un sens de
        /// InputOutput et un v_value null.
        ///
        ///  Ce comportement empêche les valeurs par défaut d'être utilisé, mais
        /// Ce sera le cas, moins fréquent qu'un v_param sortie pure prévue (calculée comme InputOutput)
        /// Où l'utilisateur n'a fourni aucune v_value entrée.
        /// </Summary>
        ///<param name="pCmd"> Le pcmd à laquelle les paramètres seront ajoutés </param>
        ///<param name="pParams"> Un parray de DbParameters à être ajouté à p_cmd </param>
        private static void AttachParameters(DbCommand pCmd, IEnumerable<DbParameter> pParams)
        {

            Dictionary<string, EacParameter> spParameters = GetParametersPrototype(pCmd);
            foreach (var vParam in pParams)
            {
                if (vParam == null)
                {
                    continue;
                }

                var vDir = vParam.Direction;
                if ((vDir == ParameterDirection.InputOutput || vDir == ParameterDirection.Input) &&
                    (vParam.Value == null))
                {
                    vParam.Value = DBNull.Value;
                }

                if (spParameters != null)
                {
                    var eParam = vParam as EacParameter;

                    //HACK problèmes de nommage de paramètres
                    var name = eParam.ParameterName;
                    if (name.StartsWith("P0") && !spParameters.ContainsKey(name))
                    {
                        name = "PO" + name.Substring(2);
                    }

                    if (spParameters.ContainsKey(name.ToUpper()))
                    {
                        var tmp = spParameters[name.ToUpper()];

                        eParam.EacType = tmp.EacType;
                        eParam.IsNullable = tmp.IsNullable;
                        eParam.SetPrecision(tmp.Precision);
                        eParam.SetSize(tmp.Size);
                        eParam.SetScale(tmp.Scale);
                        pCmd.Parameters.Add(eParam);
                    }
                }
                else
                {
                    pCmd.Parameters.Add(vParam);
                }
            }
        }

        internal static Dictionary<string, EacParameter> GetParametersPrototype(DbCommand dbCommand)
        {
            Dictionary<string, EacParameter> spParameters = null;
            if (dbCommand.CommandType == CommandType.StoredProcedure)
            {
                if (paramsCache == null || programsParamsCache == null)
                {
                    lock (paramCacheLock)
                    {
                        if (paramsCache == null)
                        {
                            InitParamsCache(dbCommand.Connection);
                        }
                    }

                }

                if (dbCommand.CommandText.Contains("*PGM/"))
                {
                    var programName = dbCommand.CommandText.Replace("*PGM/", "");
                    if (programsParamsCache.ContainsKey(programName))
                    {
                        spParameters = programsParamsCache[programName];
                    }
                }
                else
                {
                    if (paramsCache.ContainsKey(dbCommand.CommandText))
                    {
                        spParameters = paramsCache[dbCommand.CommandText];
                    }
                }
            }

            return spParameters;
        }

        /// <summary>
        /// Cette méthode ouvre (si nécessaire) et attribue une connexion, de transaction, p_cmd v_type et les paramètres
        /// À la condition pcmd
        /// </summary>
        /// <param name="pCommand"> Le DbCommand être préparés </param>
        /// <param name="pConnection"> Un DbConnection valide, sur laquelle exécuter cette p_cmd </param>
        /// <param name="pTransaction"> Un DbTransaction valide, ou «nul» </param>
        /// <param name="pParams"> Un p_array de DbParameters d'être associé à l'p_cmd ou «nul» si aucun paramètre n'est nécessaire </param>
        /// <param name="pMustCloseConnection"> <c> true </c> si la connexion a été ouverte par la méthode, otherwose est faux. </param>
        private static void PrepareCommand(
          DbCommand pCommand,
          DbConnection pConnection,
          DbTransaction pTransaction,
          IEnumerable<DbParameter> pParams,
          out bool pMustCloseConnection)
        {
            if (pConnection.State != ConnectionState.Open)
            {
                pMustCloseConnection = true;
                pConnection.Open();
            }
            else
            {
                pMustCloseConnection = false;
            }

            if (pTransaction != null)
            {
                if (pTransaction.Connection == null)
                {
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "pTransaction");
                }

                pCommand.Transaction = pTransaction;
            }
            if (pParams != null)
            {
                AttachParameters(pCommand, pParams);
            }
        }

        #endregion private utility methods & constructors
        #region DbProviderFactory methods
        /// <summary>
        /// Créer un DbProviderFactory fournisseur
        /// </summary>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// <returns> Une instance de DbProviderFactory </returns>
        private static DbProviderFactory GetFactoryAlias(this ConnectionStringSettings pSettings)
        {
            return DbProviderFactories.GetFactory(pSettings.ProviderName);
        }
        #endregion
        #region DbConnection methods
        /// <summary>
        /// Initialise une nouvelle instance de la classe DbConnection
        /// <remarks>La connection n'est pas ouverte par cette méthode.</remarks>
        /// </summary>
        /// <param name="pSettings">Represents a single, named connection string in the connection strings configuration file section.</param>
        /// <returns>nouvelle instance de la classe DbConnection</returns>
        public static DbConnection CreateConnection(this ConnectionStringSettings pSettings)
        {
            return new ConnectionWrapper() { ConnectionString = pSettings.ConnectionString };
        }
        #endregion

        #region DbCommand methods
        private static DbCommand CreateCommand(
          DbConnection pConnection,
          CommandType pCmdType,
          string pCmdText)
        {
            var vCmd = pConnection.CreateCommand();
            vCmd.CommandType = pCmdType;
            vCmd.CommandText = pCmdText;
            vCmd.Connection = pConnection;
            DbCommand d = new DbCommandWrapper(vCmd);
            return d;
        }
        #endregion
        #region Parameter Methods
        /// <summary>
        /// Créer une nouvelle instance de DbParameter.
        /// </summary>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// Une instance de <returns> DbParameter </returns>
        public static DbParameter CreateParameter(this ConnectionStringSettings pSettings)
        {
            return GetFactoryAlias(pSettings).CreateParameter();
        }


        /// <summary>
        /// Initialisation UNE instance de la classe Nouvelle DbParameter
        /// Avec Le NOM Du parametre et Le v_type de données des
        /// </summary>
        /// <param name="pParameterName"> Nom du paramètre </param>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// <param name="pDbType"> Type de data </param>
        /// <returns> Retourner une instance de DbParameter </returns>
        public static DbParameter CreateParameter(
          this ConnectionStringSettings pSettings,
          string pParameterName,
          DbType pDbType)
        {
            var vParam = CreateParameter(pSettings);
            vParam.ParameterName = pParameterName;
            vParam.DbType = pDbType;
            return vParam;
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe DbParameter
        /// avec le nom du paramètre, le v_type de données, et la taille.
        /// </summary>
        /// <param name="pParameterName"> Nom du paramètre </param>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// <param name="pDbType"> Type de data </param>
        /// <param name="pSize"> paramètre taille </param>
        /// <returns> Retourner une instance de DbParameter </returns>
        public static DbParameter CreateParameter(
          this ConnectionStringSettings pSettings,
          string pParameterName,
          DbType pDbType,
          int pSize)
        {
            var vParam = CreateParameter(pSettings, pParameterName, pDbType);
            vParam.Size = pSize;
            return vParam;
        }

        #endregion
        #region DataAdapter Methods
        /// <summary>
        /// Initialise une nouvelle instance de la classe DbDataAdapter
        /// </summary>
        /// <returns>nouvelle instance de la classe DbDataAdapter</returns>
        private static DbDataAdapter CreateDataAdapter(this ConnectionStringSettings pSettings)
        {
            return GetFactoryAlias(pSettings).CreateDataAdapter();
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe DbDataAdapter
        /// avec le DbCommand spécifié en tant que propriété SelectCommand.
        /// </summary>
        /// <param name="pSelectCommand">DbCommand spécifié en tant que propriété SelectCommand</param>
        /// <param name="pSettings">informations de connexions</param>
        /// <returns>nouvelle instance de la classe DbDataAdapter</returns>
        public static DbDataAdapter CreateDataAdapter(
          this ConnectionStringSettings pSettings,
          DbCommand pSelectCommand)
        {
            DbDataAdapter vAda = CreateDataAdapter(pSettings);
            vAda.SelectCommand = pSelectCommand;
            return vAda;
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// Exécuter une DbCommand (qui ne retourne pas de résultats) contre la base de données spécifiée dans la chaîne de connexion
        /// En utilisant les paramètres fournis
        /// </summary>
        /// <remarks>
        /// EX.:
        /// Int = v_maps ExecuteNonQuery (connString, CommandType.StoredProcedure, "PublishOrders", DbParameter nouvelle ("@ ProdID", 24));
        /// </remarks>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </ Param>
        /// <param name="pCommandType"> Le CommandType (procédure stockée, p_text, etc) </param>
        /// <param name="pCommandText"> Le p_propertyName procédure stockée ou T-SQL p_cmd </param>
        /// <param name="pParams"> Un p_array de SqlParamters utilisé pour exécuter la p_cmd </param>
        /// <returns> Un int représentant le nombre de lignes affectées par les p_cmd </ returns>
        public static int ExecuteNonQuery(this ConnectionStringSettings pSettings, CommandType pCommandType, string pCommandText, params DbParameter[] pParams) {
            return ExecuteNonQuery(pSettings, pCommandType, pCommandText, pParams.AsEnumerable());
        }

        /// <summary>
        /// Exécuter une DbCommand (qui ne retourne pas de résultats) contre la base de données spécifiée dans la chaîne de connexion
        /// En utilisant les paramètres fournis
        /// </summary>
        /// <remarks>
        /// EX.:
        /// Int = v_maps ExecuteNonQuery (connString, CommandType.StoredProcedure, "PublishOrders", DbParameter nouvelle ("@ ProdID", 24));
        /// </remarks>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </ Param>
        /// <param name="pCommandType"> Le CommandType (procédure stockée, p_text, etc) </param>
        /// <param name="pCommandText"> Le p_propertyName procédure stockée ou T-SQL p_cmd </param>
        /// <param name="pParams"> Un p_array de SqlParamters utilisé pour exécuter la p_cmd </param>
        /// <returns> Un int représentant le nombre de lignes affectées par les p_cmd </ returns>
        public static int ExecuteNonQuery(this ConnectionStringSettings pSettings, CommandType pCommandType, string pCommandText, IEnumerable<DbParameter> pParams) {
            using (DbConnection vConnection = CreateConnection(pSettings))
            {
                vConnection.Open();
                using (DbTransaction tran = vConnection.BeginTransaction())
                {
                    var result = ExecuteNonQuery(
                        pSettings,
                        vConnection,
                        pCommandType,
                        pCommandText,
                        pParams);
                    tran.Commit();
                    return result;
                }
            }
        }

        public static int ExecuteNonQueryWithoutTransaction(this ConnectionStringSettings pSettings, CommandType pCommandType, string pCommandText, IEnumerable<DbParameter> pParams) {
            using (DbConnection vConnection = CreateConnection(pSettings)) {
                vConnection.Open();
                return ExecuteNonQuery(pSettings, vConnection, pCommandType, pCommandText, pParams);
            }
        }

        public static int ExecuteNonQueryWithoutTransaction(this ConnectionStringSettings pSettings, CommandType pCommandType, string pCommandText) {
            return ExecuteNonQueryWithoutTransaction(pSettings, pCommandType, pCommandText, null);
        }

        public static int ExecuteNonQuery(
          this ConnectionStringSettings pSettings,
          CommandType pCommandType,
          string pCommandText
          )
        {
            using (DbConnection vConnection = CreateConnection(pSettings))
            {
                vConnection.Open();
                using (DbTransaction tran = vConnection.BeginTransaction())
                {
                    var result = ExecuteNonQuery(
                      pSettings,
                      vConnection,
                      pCommandType,
                      pCommandText,
                      null
                      );
                    tran.Commit();
                    return result;
                }
            }
        }

        /// <summary>
        /// Exécuter une DbCommand (qui ne retourne pas de résultats) contre le DbConnection spécifié
        /// En utilisant les paramètres fournis.
        /// </summary>
        /// <remarks>
        /// EX.:
        /// Int = v_maps ExecuteNonQuery (v_connection, CommandType.StoredProcedure, "PublishOrders", DbParameter nouvelle ("@ ProdID", 24));
        /// </remarks>
        /// <param name="pConnection"> Un DbConnection valide </ param>
        /// <param name="pCommandType"> Le CommandType (procédure stockée, p_text, etc) </param>
        /// <param name="pCommandText"> Le p_propertyName procédure stockée ou T-SQL p_cmd </param>
        /// <param name="pParams"> Un p_array de SqlParamters utilisé pour exécuter la p_cmd </param>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// <returns> Un int représentant le nombre de lignes affectées par les p_cmd </returns>
        public static int ExecuteNonQuery(
          this ConnectionStringSettings pSettings,
          DbConnection pConnection,
          CommandType pCommandType,
          string pCommandText,
          IEnumerable<DbParameter> pParams)
        {
            if (pConnection == null)
            {
                throw new ArgumentNullException("pConnection");
            }

            using (var vCmd = CreateCommand(pConnection, pCommandType, pCommandText))
            {

                PrepareCommand(
                  vCmd,
                  pConnection,
                  (DbTransaction)null,
                  pParams,
                  out bool vMustCloseConnection);

                vCmd.Prepare();

                var vRetval = vCmd.ExecuteNonQuery();

                vCmd.Parameters.Clear();
                if (vMustCloseConnection)
                {
                    pConnection.Close();
                }

                return vRetval;
            }
        }
        #endregion
        private static Dictionary<string, ColumnExpression[]> infoCache = new Dictionary<string, ColumnExpression[]>();
        /// <summary>
        /// entité information
        /// </summary>
        /// <param name="pType"> ptype.</param>
        /// <param name="pDr"> PDR.</param>
        /// <returns></returns>
        internal static ColumnExpression[] GetEntityInformations(Type pType, DbDataReader pDr)
        {
            var names = Enumerable.Range(0, pDr.FieldCount).Select(pDr.GetName).ToArray();
            string key = $"{pType.FullName}/{string.Join("_", names)}";
            if (!infoCache.ContainsKey(key))
            {
                infoCache.Add(key, GetEntityInformations(pType, names));
            }
            return infoCache[key];
        }

        private static ColumnExpression[] GetEntityInformations(Type pType, string[] names)
        {
            var vList = new List<ColumnExpression>();

            var vProps = pType.GetProperties(
              BindingFlags.Public
              | BindingFlags.SetProperty
              | BindingFlags.Instance);

            List<KeyValuePair<int, string>> dicProp = new List<KeyValuePair<int, string>>();

            //Selectionner seulement mes propriétes portant l'attribut spécifique
            var selectedProp =
             vProps.Where(el => ((ColumnAttribute[])el.GetCustomAttributes(typeof(ColumnAttribute), true)).Any()).ToArray();
            for (int i = 0; i < selectedProp.Count(); i++)
            {
                dicProp.Add(new KeyValuePair<int, string>(i, selectedProp[i].Name));
            }

            ColumnExpression returnColumnExp;

            foreach (var name in names)
            {
                vProps.FirstOrDefault(prop => {
                    ColumnAttribute[] attributes = ((ColumnAttribute[])prop.GetCustomAttributes(typeof(ColumnAttribute), true));
                    if (attributes.Length > 0 && attributes[0].Name == name)
                    {
                        var elemDict = dicProp.FirstOrDefault(e => e.Value == prop.Name);
                        returnColumnExp = GetLinqAttr(attributes[0]);
                        returnColumnExp.PropertyName = returnColumnExp.Name = attributes[0].Name;
                        returnColumnExp.Ordinal = elemDict.Key;
                        returnColumnExp.PropInfo = prop;
                        returnColumnExp.TypeInfo = prop.PropertyType;
                        vList.Add(returnColumnExp);
                        return true;
                    }
                    return false;
                });
            }
            return vList.ToArray();
        }

        /// <summary>
        /// Linq Attribut.
        /// </summary>
        /// <param name="pAttr">attribut.</param>
        /// <returns></returns>
        private static ColumnExpression GetLinqAttr(ColumnAttribute pAttr)
        {
            var vAttr = new ColumnExpression {
                CanBeNull = pAttr.CanBeNull,
                IsPrimaryKey = pAttr.IsPrimaryKey,
                IsDbGenerated = pAttr.IsDbGenerated,
                Name = pAttr.Name,
                Expression = pAttr.Expression
            };
            return vAttr;
        }

        /// <summary>
        /// Renvoie une liste d'entité.
        /// </summary>
        /// <typeparam p_propertyName="T">Type du Paramètre de retour.</typeparam>
        /// <param name="pSettings">Represents a single, named connection string in the connection strings configuration file section.</param>
        /// <param name="pCommandType">Type de commande</param>
        /// <param name="pCommandText">Texte de la commande</param>
        /// <returns></returns>
        public static List<T> ExecuteList<T>(
          this ConnectionStringSettings pSettings,
          CommandType pCommandType,
          string pCommandText
         ) where T : new()
        {
            return ExecuteList<T>(pSettings, pCommandType, pCommandText, null);
        }

        /// <summary>
        /// Tries getting the first row of selection
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="settings"></param>
        /// <param name="commandType">Type of supplied SQL</param>
        /// <param name="commandText">The SQL query</param>
        /// <param name="parameters">List of parameters</param>
        /// <param name="hasToBeSingle">Determines whether the list has to contain one single element</param>
        /// <returns></returns>
        public static T ExecuteListGetFirst<T>(this ConnectionStringSettings settings, CommandType commandType, string commandText, IEnumerable<DbParameter> parameters, bool hasToBeSingle = false) where T : new()
        {
            var list = ExecuteList<T>(settings, commandType, commandText, parameters);
            if (list != null && list.Any())
            {
                return list.First();
            }

            return default(T);
        }



        public class DbParams : SqlMapper.IDynamicParameters,
                        IEnumerable<IDbDataParameter>
        {

            public DbParams(IEnumerable<IDbDataParameter> parameters)
            {
                this.parameters = parameters.ToList();
            }
            private readonly List<IDbDataParameter> parameters =
                new List<IDbDataParameter>();
            public IEnumerator<IDbDataParameter> GetEnumerator()
            {
                return parameters.GetEnumerator();
            }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }
            public void Add(IDbDataParameter value)
            {
                parameters.Add(value);
            }
            void SqlMapper.IDynamicParameters.AddParameters(IDbCommand command,
                SqlMapper.Identity identity)
            {
                foreach (IDbDataParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
        }


        private static HashSet<Type> mappedTypes = new HashSet<Type>();
        /// <summary>
        /// Renvoie une liste d'entité.
        /// </summary>
        /// <typeparam p_propertyName="T">Type du Paramètre de retour.</typeparam>
        /// <param name="pSettings">Represents a single, named connection string in the connection strings configuration file section.</param>
        /// <param name="pCommandType">Type de commande</param>
        /// <param name="pCommandText">Texte de la commande</param>
        /// <param name="pParams">Liste des paramètres</param>
        /// <returns></returns>
        public static List<T> ExecuteList<T>(
            this ConnectionStringSettings pSettings,
            CommandType pCommandType,
            string pCommandText,
            IEnumerable<DbParameter> pParams) where T : new()
        {
            Type type = typeof(T);
            if (!mappedTypes.Contains(type))
            {
                SqlMapper.SetTypeMap(type, new ColumnAttributeTypeMapper<T>());
                mappedTypes.Add(type);
            }

            using (DbConnection vConn = CreateConnection(pSettings))
            {
                vConn.Open();
                using (var tran = vConn.BeginTransaction())
                {
                    using (var vCmd = CreateCommand(vConn, pCommandType, pCommandText))
                    {
                        if (pParams != null)
                        {
                            AttachParameters(vCmd, pParams);
                        }


                        var result =  vConn.Query<T>(new CommandDefinition(
                            vCmd.CommandText,
                            new DbParams(vCmd.Parameters.Cast<IDbDataParameter>()),
                            commandType: vCmd.CommandType)).ToList();

                        tran.Commit();

                        return result;
                    }
                }
                
            }
        }

        public static List<T> ExecuteList_Older<T>(
            this ConnectionStringSettings pSettings,
            CommandType pCommandType,
            string pCommandText,
            IEnumerable<DbParameter> pParams) where T : new()
        {
            Type type = typeof(T);
            if (!mappedTypes.Contains(type))
            {
                SqlMapper.SetTypeMap(type, new ColumnAttributeTypeMapper<T>());
                mappedTypes.Add(type);
            }

            using (DbConnection vConn = CreateConnection( pSettings ))
            {
                using (var vCmd = CreateCommand(vConn, pCommandType, pCommandText))
                {
                    if (pParams != null)
                    {
                        AttachParameters(vCmd, pParams);
                    }

                    vConn.Open();
                    vCmd.Prepare();
                    using (DbDataReader vDr = vCmd.ExecuteReader())
                    {
                        var sw = new Stopwatch();
                        try
                        {
                            sw.Start();

                            var vType = new T();
                            var vMaps = GetEntityInformations(type, vDr);
                            var vLen = vMaps.Length;

                            var vList = new List<T>();
                            var properties = typeof(T).GetProperties().Where(p => ((ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), true)).Length != 0).ToArray();
                            if (vLen > 0)
                            {
                                //// Affectation du Delegate

                                var vOrdinals = new int[vLen];
                                for (var vI = 0; vI < vLen; ++vI)
                                {
                                    vOrdinals[vI] = vMaps[vI].Ordinal;
                                }

                                while (vDr.Read())
                                {
                                    var item = Activator.CreateInstance<T>();
                                    foreach (var col in vMaps)
                                    {
                                        object value = vDr[col.Name];
                                        value = PreCastValue(properties[col.Ordinal], value);

                                        properties[col.Ordinal].SetValue(item, value == DBNull.Value ? null : value, null);
                                    }

                                    vList.Add(item);
                                }
                            }
                            return vList;
                        }
                        finally
                        {
                            sw.Stop();
                        }
                    }
                }
            }
        }

        public static List<T> ExecuteNumericList<T>(this ConnectionStringSettings settings, CommandType commandType, string commandText, params DbParameter[] parameters) where T : struct, IComparable<T>
        {
            using (DbConnection vConn = CreateConnection(settings))
            {
                vConn.Open();
                using (DbTransaction tran = vConn.BeginTransaction())
                {
                    using (var vCmd = CreateCommand(vConn, commandType, commandText))
                    {
                        if (parameters != null)
                        {
                            AttachParameters(vCmd, parameters);
                        }

                        vCmd.Prepare();
                        var values = new List<T>();
                        using (DbDataReader reader = vCmd.ExecuteReader())
                        {
                            var sw = new Stopwatch();
                            sw.Start();

                            while (reader.Read())
                            {
                                values.Add((T)reader.GetValue(0));
                            }

                            sw.Stop();
                            //  Trace(sw, vCmd, stringList.Count);
                        }
                        tran.Commit();
                        return values;
                    }
                }
            }
        }

        public static List<string> ExecuteStringList(this ConnectionStringSettings settings, CommandType commandType, string commandText, params DbParameter[] parameters)
        {
            using (DbConnection vConn = CreateConnection(settings))
            {
                vConn.Open();
                using (DbTransaction tran = vConn.BeginTransaction())
                {
                    using (var vCmd = CreateCommand(vConn, commandType, commandText))
                    {
                        if (parameters != null)
                        {
                            AttachParameters(vCmd, parameters);
                        }

                        vCmd.Prepare();
                        var values = new List<string>();
                        using (DbDataReader reader = vCmd.ExecuteReader())
                        {
                            var sw = new Stopwatch();
                            sw.Start();

                            while (reader.Read())
                            {
                                values.Add(reader.GetValue(0) as string);
                            }

                            sw.Stop();
                        }
                        tran.Commit();
                        return values;
                    }
                }
            }
        }


        #region ExecuteDataTable
        public static DataTable ExecuteDataTable(this ConnectionStringSettings pSettings, CommandType pCommandType, string pCommandText, params DbParameter[] pParams)
        {
            DataTable vDataTable;
            using (DbConnection vConn = CreateConnection(pSettings))
            {
                vConn.Open();
                using (DbTransaction tran = vConn.BeginTransaction())
                {
                    using (DbCommand vCmd = CreateCommand(vConn, pCommandType, pCommandText))
                    {
                        if (pParams != null)
                        {
                            AttachParameters(vCmd, pParams);
                        }
                        using (DbDataReader vDr = vCmd.ExecuteReader())
                        {
                            var sw = new Stopwatch();
                            sw.Start();
                            vDataTable = new DataTable();
                            vDataTable.Load(vDr);
                            sw.Stop();
                        }
                        tran.Commit();
                    }
                    return vDataTable;
                }
            }
        }



        #endregion


        #region ExecuteReader
        /// <summary>
        /// Insertion et de préparer un DbCommand, et appeler ExecuteReader avec le CommandBehavior appropriée.
        /// </summary>
        /// <remarks>
        /// Si nous avons créé et a ouvert la connexion, nous voulons que la connexion soit fermée lorsque la fermeture du DataReader.
        ///
        /// Si l'appelant à condition que la connexion, nous voulons leur laisser le soin de gérer.
        /// </remarks>
        /// <param name="pConnection"> Un DbConnection valide, sur laquelle exécuter cette p_cmd </param>
        /// <param name="pTransaction"> Un DbTransaction valide, ou «nul» </ param>
        /// <param name="pCommandType"> Le CommandType (procédure stockée, p_text, etc) </param>
        /// <param name="pCommandText"> Le p_propertyName procédure stockée ou T-SQL p_cmd </param>
        /// <param name="pParams"> Un p_array de DbParameters d'être associé à l'p_cmd ou «nul» si aucun paramètre n'est nécessaire </param>
        /// <param name="pConnectionOwnership"> Indique si la connexion a été v_param fournie par l'appelant, ou créés par DbHelper </param>
        /// DbDataReader <returns> / contenant les p_results des p_cmd </returns>
        private static DbDataReader ExecuteReaderInternal(
          DbConnection pConnection,
          DbTransaction pTransaction,
          CommandType pCommandType,
          string pCommandText,
          DbParameter[] pParams,
          DbConnectionOwnership pConnectionOwnership)
        {
            var vMustCloseConnection = false;
            var vCmd = pConnection.CreateCommand();
            vCmd.Connection = pConnection;
            vCmd.CommandText = pCommandText;
            vCmd.CommandType = pCommandType;
            try
            {
                PrepareCommand(
                  vCmd,
                  pConnection,
                  pTransaction,
                  pParams,
                  out vMustCloseConnection);

                var vDr = pConnectionOwnership == DbConnectionOwnership.External ? vCmd.ExecuteReader() : vCmd.ExecuteReader(CommandBehavior.CloseConnection);


                var vCanClear = vCmd.Parameters.Cast<DbParameter>().All(vParam => vParam.Direction == ParameterDirection.Input);

                if (vCanClear)
                {
                    vCmd.Parameters.Clear();
                }

                return vDr;
            }
            catch (DbException)
            {
                if (vMustCloseConnection)
                {
                    pConnection.Close();
                }

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSettings">Représente la connexion paramétré dans le fichier de configuration</param>
        /// <param name="pCommandText"></param>
        /// <returns></returns>
        public static DbDataReader ExecuteReader(
         this ConnectionStringSettings pSettings,
         string pCommandText)
        {
            return ExecuteReader(
              pSettings,
              CommandType.Text,
              pCommandText,
              null);
        }

        /// <summary>
        /// Exécuter une DbCommand (qui renvoie un jeu de résultats) contre la base de données spécifiée dans la chaîne de connexion
        /// En utilisant les paramètres fournis.
        /// </summary>
        /// <remarks>
        /// EX.:
        /// Dr = DbDataReader ExecuteReader (connString, CommandType.StoredProcedure, "GetOrders", DbParameter nouvelle ("@ ProdID", 24));
        /// </remarks>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// <param name="pCommandType"> Le CommandType (procédure stockée, p_text, etc) </param>
        /// <param name="pCommandText"> Le p_propertyName procédure stockée ou T-SQL p_cmd </param>
        /// <param name="pParams"> Un p_array de SqlParamters utilisé pour exécuter la p_cmd </param>
        /// Un <returns> DbDataReader contenant le jeu de résultats généré par les p_cmd </returns>
        public static DbDataReader ExecuteReader(
          this ConnectionStringSettings pSettings,
          CommandType pCommandType,
          string pCommandText,
          params DbParameter[] pParams)
        {
            using (var vConnection = CreateConnection(pSettings))
            {
                vConnection.Open();
                using (DbTransaction tran = vConnection.BeginTransaction())
                {
                    var result = ExecuteReaderInternal(
                        vConnection,
                        null,
                        pCommandType,
                        pCommandText,
                        pParams,
                        DbConnectionOwnership.Internal);
                    tran.Commit();
                    return result;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSettings">Représente la connexion paramétré dans le fichier de configuration</param>
        /// <param name="pCommandType"></param>
        /// <param name="pCommandText"></param>
        /// <param name="pParams"></param>
        /// <returns></returns>
        public static IEnumerable<IDataRecord> ExecuteReaderList(
          this ConnectionStringSettings pSettings,
          CommandType pCommandType,
          string pCommandText,
          params DbParameter[] pParams)
        {
            using (var vDr = ExecuteReader(
              pSettings,
              pCommandType,
              pCommandText,
              pParams))
            {
                while (vDr.Read())
                {
                    yield return vDr;
                }
            }
        }

        /// <summary>
        /// Exécuter une DbCommand (qui renvoie un jeu de résultats) contre la base de données spécifiée dans la chaîne de connexion
        /// En utilisant les paramètres fournis.
        /// </summary>
        /// <remarks>
        /// EX.:
        /// Dr = DbDataReader ExecuteReader (connString, CommandType.StoredProcedure, "GetOrders", DbParameter nouvelle ("@ ProdID", 24));
        /// </remarks>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// <param name="pConnection">Représente la connexion paramétré dans le fichier de configuration</param>
        /// <param name="pCommandType"> Le CommandType (procédure stockée, p_text, etc) </param>
        /// <param name="pCommandText"> Le p_propertyName procédure stockée ou T-SQL p_cmd </param>
        /// <param name="pParams"> Un p_array de SqlParamters utilisé pour exécuter la p_cmd </param>
        /// <returns> DbDataReader contenant le jeu de résultats généré par les p_cmd </returns>
        public static DbDataReader ExecuteReader(
          this ConnectionStringSettings pSettings,
          DbConnection pConnection,
          CommandType pCommandType,
          string pCommandText,
          params DbParameter[] pParams)
        {
            return ExecuteReaderInternal(
              pConnection,
              null,
              pCommandType,
              pCommandText,
              pParams,
              DbConnectionOwnership.External);
        }

        /// <summary>
        /// Exécuter une DbCommand (qui renvoie un jeu de résultats) contre la base de données spécifiée dans la chaîne de connexion
        /// En utilisant les paramètres fournis.
        /// </summary>
        /// <remarks>
        /// EX.:
        /// Dr = DbDataReader ExecuteReader (connString, CommandType.StoredProcedure, "GetOrders", DbParameter nouvelle ("@ ProdID", 24));
        /// </remarks>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// <param name="pTransaction">Transaction </param>
        /// <param name="pCommandType"> Le CommandType (procédure stockée, p_text, etc) </param>
        /// <param name="pCommandText"> Le p_propertyName procédure stockée ou T-SQL p_cmd </param>
        /// <param name="pParams"> Un p_array de SqlParamters utilisé pour exécuter la p_cmd </param>
        /// Un <returns> DbDataReader contenant le jeu de résultats généré par les p_cmd </returns>
        public static DbDataReader ExecuteReader(
          this ConnectionStringSettings pSettings,
          DbTransaction pTransaction,
          CommandType pCommandType,
          string pCommandText,
          params DbParameter[] pParams)
        {
            if (pTransaction == null)
            {
                throw new ArgumentNullException("pTransaction");
            }

            if (pTransaction != null && pTransaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "pTransaction");
            }

            return ExecuteReaderInternal(
              pTransaction.Connection,
              pTransaction,
              pCommandType,
              pCommandText,
              pParams,
              DbConnectionOwnership.External);
        }
        #endregion ExecuteReader
        #region ExecuteScalar
        /// <summary>
        /// Exécuter une DbCommand (qui renvoie un jeu de résultats) contre la base de données spécifiée dans la chaîne de connexion
        /// En utilisant les paramètres fournis.
        /// </summary>
        /// <remarks>
        /// EX.:
        /// Dr = DbDataReader ExecuteReader (connString, CommandType.StoredProcedure, "GetOrders", DbParameter nouvelle ("@ ProdID", 24));
        /// </remarks>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// <param name="pCommandType"> Le CommandType (procédure stockée, p_text, etc) </param>
        /// <param name="pCommandText"> Le p_propertyName procédure stockée ou T-SQL p_cmd </param>
        /// <param name="pParams"> Un p_array de SqlParamters utilisé pour exécuter la p_cmd </param>
        ///<returns> objet contenant le jeu de résultats généré par les p_cmd </returns>
        public static object ExecuteScalar(
          this ConnectionStringSettings pSettings,
          CommandType pCommandType,
          string pCommandText,
          params DbParameter[] pParams)
        {
            return ExecuteScalar(
                pSettings,
                pCommandType,
                pCommandText,
                (IEnumerable<DbParameter>)pParams);
        }


        /// <summary>
        /// Exécuter une DbCommand (qui renvoie un jeu de résultats) contre la base de données spécifiée dans la chaîne de connexion
        /// En utilisant les paramètres fournis.
        /// </summary>
        /// <remarks>
        /// EX.:
        /// Dr = DbDataReader ExecuteReader (connString, CommandType.StoredProcedure, "GetOrders", DbParameter nouvelle ("@ ProdID", 24));
        /// </remarks>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// <param name="pCommandType"> Le CommandType (procédure stockée, p_text, etc) </param>
        /// <param name="pCommandText"> Le p_propertyName procédure stockée ou T-SQL p_cmd </param>
        /// <param name="pParams"> Un p_array de SqlParamters utilisé pour exécuter la p_cmd </param>
        ///<returns> objet contenant le jeu de résultats généré par les p_cmd </returns>
        public static object ExecuteScalar(
          this ConnectionStringSettings pSettings,
          CommandType pCommandType,
          string pCommandText,
          IEnumerable<DbParameter> pParams)
        {
            using (var vConnection = CreateConnection(pSettings))
            {
                vConnection.Open();
                using (var tran = vConnection.BeginTransaction())
                {
                    var result = ExecuteScalar(
                      pSettings,
                      vConnection,
                      pCommandType,
                      pCommandText,
                      pParams);
                    tran.Commit();
                    return result;
                }
            }
        }

        /// <summary>
        /// Exécuter une DbCommand (qui renvoie un jeu de résultats) contre la base de données spécifiée dans la chaîne de connexion
        /// En utilisant les paramètres fournis.
        /// </summary>
        /// <remarks>
        /// EX.:
        /// Dr = DbDataReader ExecuteReader (connString, CommandType.StoredProcedure, "GetOrders", DbParameter nouvelle ("@ ProdID", 24));
        /// </remarks>
        /// <param name="pSettings"> Représente un seul, chaîne de connexion nommée dans le cadre des chaînes de la section du fichier de configuration. </param>
        /// <param name="pConnection">Représente la connexion paramétré dans le fichier de configuration</param>
        ///<param name="pCommandType"> Le CommandType (procédure stockée, p_text, etc) </param>
        /// <param name="pCommandText"> Le p_propertyName procédure stockée ou T-SQL p_cmd </param>
        /// <param name="pParams"> Un p_array de SqlParamters utilisé pour exécuter la p_cmd </param>
        ///<returns> un objet  contenant le  1 seule jeu de résultats généré par les p_cmd </returns>
        public static object ExecuteScalar(
          this ConnectionStringSettings pSettings,
          DbConnection pConnection,
          CommandType pCommandType,
          string pCommandText,
          IEnumerable<DbParameter> pParams)
        {
            if (pConnection == null)
            {
                throw new ArgumentNullException("pConnection");
            }

            using (var vCmd = CreateCommand(pConnection, pCommandType, pCommandText))
            {
                PrepareCommand(
                  vCmd,
                  pConnection,
                  null,
                  pParams,
                  out bool vMustCloseConnection);
                try
                {
                    var vRetval = vCmd.ExecuteScalar();
                    return vRetval;
                }
                finally
                {
                    if (vMustCloseConnection)
                    {
                        pConnection.Close();
                    }
                }
            }
        }
        #endregion ExecuteScalar

        private static object PreCastValue(PropertyInfo propInfos, object value)
        {
            if (value != DBNull.Value && value != null)
            {
                if (propInfos.PropertyType == typeof(decimal) && value.GetType() == typeof(double))
                {
                    value = Convert.ToDecimal(value);
                }
                else if ((propInfos.PropertyType == typeof(float?) || propInfos.PropertyType == typeof(float)) && value.GetType() == typeof(double))
                {
                    value = Convert.ToSingle(value);
                }
                else if (propInfos.PropertyType == typeof(int))
                {
                    value = Convert.ToInt32(value);
                }
                else if (propInfos.PropertyType == typeof(bool))
                {
                    if (value is string)
                    {
                        if (bool.TryParse((string)value, out bool b))
                        {
                            value = b;
                        }
                        else if (value.Equals("O") || value.Equals("N"))
                        {
                            value = value.Equals("O");
                        }
                    }
                    else
                    {
                        value = Convert.ChangeType(value, typeof(bool));
                    }
                }
                else if (propInfos.PropertyType.IsEnum)
                {
                    var ev = Enum.GetValues(propInfos.PropertyType)
                        .Cast<Enum>()
                        .FirstOrDefault(en => en.GetAttributeOfType<DisplayAttribute>()?.Name == value as string);

                    value = ev;
                }
            }

            return value;
        }
    }
}
