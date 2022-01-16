using log4net;
using System;
using System.Data;

namespace Hexavia.Repository
{
    /// <summary>
    /// DataAccessManager
    /// </summary>
    public class DataAccessManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DataAccessManager));
        private static readonly ILog PerfLogger = LogManager.GetLogger(typeof(DataAccessManager));
        private EasyCom EasyCom;

        /// <summary>
        /// DataAccessManager
        /// </summary>
        /// <param name="easyCom"></param>
        public DataAccessManager(EasyCom easyCom)
        {
            EasyCom = easyCom;
        }

        public CmdWrapper CmdWrapper
        {
            get { return new CmdWrapper { Cmd = EasyCom.EacCommand }; }
        }

        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="cmdWrapper"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(CmdWrapper cmdWrapper)
        {
            if (cmdWrapper == null)
            {
                throw new ArgumentNullException(nameof(cmdWrapper));
            }

            var command = cmdWrapper.Cmd;
            Logger.Info("ExecuteDataTable with: " + command.CommandText);
            var startTime = DateTime.Now;
            var result = new DataTable();
            using (var connection = EasyCom.Connection)
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
            PerfLogger.Info("Executed in :" + DateTime.Now.Subtract(startTime).ToString());
            return result;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="cmdWrapper"></param>
        public void ExecuteNonQuery(CmdWrapper cmdWrapper)
        {
            if (cmdWrapper == null)
            {
                throw new ArgumentNullException(nameof(cmdWrapper));
            }

            var command = cmdWrapper.Cmd;
            Logger.Info("ExecuteNonQuery with: " + command.CommandText);
            using (var connection = EasyCom.Connection)
            {
                command.Connection = connection;
                using (command)
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="cmdWrapper"></param>
        /// <returns></returns>
        public object ExecuteScalar(CmdWrapper cmdWrapper)
        {
            if (cmdWrapper == null)
            {
                throw new ArgumentNullException(nameof(cmdWrapper));
            }

            var command = cmdWrapper.Cmd;
            Logger.Info("ExecuteScalar with: " + command.CommandText);
            object result = null;
            using (var connection = EasyCom.Connection)
            {
                command.Connection = connection;
                using (command)
                {
                    result = command.ExecuteScalar();
                }
            }
            return result;
        }

        /// <summary>
        /// ExecuteExist
        /// </summary>
        /// <param name="cmdWrapper"></param>
        /// <returns></returns>
        public bool ExecuteExist(CmdWrapper cmdWrapper)
        {
            if (cmdWrapper == null)
            {
                throw new ArgumentNullException(nameof(cmdWrapper));
            }

            var result = false;
            var cpt = (int)ExecuteScalar(cmdWrapper);
            if (cpt > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
