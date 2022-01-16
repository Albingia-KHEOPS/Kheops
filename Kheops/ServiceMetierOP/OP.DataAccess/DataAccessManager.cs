using System.Data;
using System.Data.Common;
using OP.DataAccess.Helpers;
using ALBINGIA.Framework.Common.Data;
using System.Data.EasycomClient;

namespace OP.DataAccess {
    public class DataAccessManager {


        public static CmdWrapper CmdWrapper {
            get { return new CmdWrapper(); }
        }

        public static DataTable ExecuteDataTable(CmdWrapper cmdWrapper, bool defaultConnection = true) {
            if (defaultConnection)
                return DbBase.Settings.ExecuteDataTable(cmdWrapper.CommandType, cmdWrapper.CommandText,
                                               cmdWrapper.Cmd.Parameters == null ? null : GetArrayParameters(cmdWrapper));

            return DbBase.SettingsClauseVsto.ExecuteDataTable(cmdWrapper.CommandType, cmdWrapper.CommandText,
                                             cmdWrapper.Cmd.Parameters == null ? null : GetArrayParameters(cmdWrapper));
        }

        public static void ExecuteNonQuery(CmdWrapper cmdWrapper, bool defaultConnection = true) {
            var dbParam = GetArrayParameters(cmdWrapper);
            if (defaultConnection) {
                DbBase.Settings.ExecuteNonQuery(cmdWrapper.Cmd.CommandType, cmdWrapper.CommandText, dbParam);
            }
            else {
                DbBase.SettingsClauseVsto.ExecuteNonQuery(cmdWrapper.Cmd.CommandType, cmdWrapper.CommandText, dbParam);
            }

        }

        public static object ExecuteScalar(CmdWrapper cmdWrapper, bool defaultConnection = true) {

            var dbParam = GetArrayParameters(cmdWrapper);
            if (defaultConnection)
                return DbBase.Settings.ExecuteScalar(cmdWrapper.Cmd.CommandType, cmdWrapper.CommandText, dbParam);
            return DbBase.SettingsClauseVsto.ExecuteScalar(cmdWrapper.Cmd.CommandType, cmdWrapper.CommandText, dbParam);
        }

        public static bool ExecuteExist(CmdWrapper cmdWrapper, bool defaultConnection = true) {
            bool result = false;
            int cpt = (int)ExecuteScalar(cmdWrapper, defaultConnection);
            if (cpt > 0) {
                result = true;
            }
            return result;
        }

        #region Méthode privées
        private static DbParameter[] GetArrayParameters(CmdWrapper cmdWrapper) {
            if (cmdWrapper.Parameters == null)
                return null;
            var dbParam = new DbParameter[cmdWrapper.Parameters.Count];
            var i = 0;
            foreach (DbParameter item in cmdWrapper.Parameters) {
                //   item.
                dbParam[i] = new EacParameter {
                    Value = item.Value,
                    ParameterName = item.ParameterName,
                    Direction = item.Direction,
                    DbType = item.DbType,
                    Size = item.Size,
                    SourceColumn = item.SourceColumn,
                    SourceVersion = item.SourceVersion,

                };
                i++;
            }
            return dbParam;
        }
        #endregion

    }
}
