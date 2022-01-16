using System.Configuration;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Diagnostics.CodeAnalysis;

namespace Hexavia.Repository
{
    /// <summary>
    /// EasyComConnection
    /// </summary>
    public class EasyCom
    {
        private readonly string connectionString;

        public EasyCom()
        {
            connectionString = ConfigurationManager.ConnectionStrings["EasyCom"].ConnectionString;
        }

        [SuppressMessage("Gendarme.Rules.Correctness", "EnsureLocalDisposalRule")]
        public DbConnection Connection
        {
            get
            {
                var connection = new EacConnection
                {
                    ConnectionString = connectionString
                };
                connection.Open();
                return connection;
            }
        }

        public DbCommand EacCommand
        {
            get { return new EacCommand(); }
        }
    }
}
