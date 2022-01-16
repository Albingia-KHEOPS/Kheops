using System;
using System.Data.Common;
using Albingia.Hexavia.DataAccess.Helper;
using NLog;
using System.Data;

namespace Albingia.Hexavia.DataAccess
{
    public class DataAccessManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Logger perfLogger = LogManager.GetLogger("PerfLogger");
        private EasyComConnectionHelper connectionHelper;

        public DataAccessManager(EasyComConnectionHelper connectionHelper)
        {
            this.connectionHelper = connectionHelper;
        }

        public CmdWrapper CmdWrapper
        {
            get { return new CmdWrapper {Cmd = connectionHelper.IDbCommand}; }
        }

        public DataTable ExecuteDataTable(CmdWrapper cmdWrapper)
        {
            IDbCommand command = cmdWrapper.Cmd;
            logger.Info("ExecuteDataTable with: " + command.CommandText);
            DateTime startTime = DateTime.Now;
            DataTable result = new DataTable();
            using (DbConnection connection = connectionHelper.OpenConnection)
            {
                command.Connection = connection;
                using (command)
                {
                    using (IDataReader myReader = command.ExecuteReader())
                    {
                        result.Load(myReader);
                    }
                }
            }
            perfLogger.Info("Executed in :" + DateTime.Now.Subtract(startTime).ToString());
            return result;
        }

        public void ExecuteNonQuery(CmdWrapper cmdWrapper)
        {
            IDbCommand command = cmdWrapper.Cmd;
            logger.Info("ExecuteNonQuery with: " + command.CommandText);
            using (DbConnection connection = connectionHelper.OpenConnection)
            {
                command.Connection = connection;
                using (command)
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public object ExecuteScalar(CmdWrapper cmdWrapper)
        {
            IDbCommand command = cmdWrapper.Cmd;
            logger.Info("ExecuteScalar with: " + command.CommandText);
            object result = null;
            using (DbConnection connection = connectionHelper.OpenConnection)
            {
                command.Connection = connection;
                using (command)
                {
                    result = command.ExecuteScalar();
                }
            }
            return result;
        }

        public bool ExecuteExist(CmdWrapper cmdWrapper)
        {
            bool result = false;
            int cpt = (int)ExecuteScalar(cmdWrapper);
            if (cpt > 0)
            {
                result = true;
            }
            return result;
        }

    }
}
