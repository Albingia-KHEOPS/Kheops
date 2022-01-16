using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Data.EasycomClient;
using System.Configuration;
using System.Data;

namespace Albingia.Hexavia.DataAccess
{
    public class EasyComConnectionHelper
    {
        private string connectionString;

        public EasyComConnectionHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["EasyCom"].ConnectionString;
        }

        [SuppressMessage("Gendarme.Rules.Correctness", "EnsureLocalDisposalRule")]
        public DbConnection OpenConnection
        {
            get
            {
                DbConnection connection = new EacConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                return connection;
            }
        }

        public DbCommand IDbCommand
        {
            get { return new EacCommand(); }
        }

    }
}
