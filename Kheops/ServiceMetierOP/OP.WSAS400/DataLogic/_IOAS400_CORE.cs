using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.EasycomClient;
using System.Configuration;
using System.Data;

namespace OP.WSAS400.DataLogic
{
    public partial class IOAS400 : IDisposable
    {
        private string _connectionString;
        private EacConnection _oConn;

        public IOAS400(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected EacConnection oConn
        {
            get
            {
                if (_oConn == null)
                {
                    _oConn = new EacConnection(_connectionString);
                    _oConn.Open();
                }
                return _oConn;
            }
        }

        public void Dispose()
        {
            if (_oConn.State == ConnectionState.Open)
            {
                _oConn.Close();
            }
        }
    }
}