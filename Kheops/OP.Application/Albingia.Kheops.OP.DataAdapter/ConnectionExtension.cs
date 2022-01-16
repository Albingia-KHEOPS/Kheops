using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter
{
    public static class ConnectionExtension
    {
        public static IDbConnection EnsureOpened(this IDbConnection con) {
            if (con.State != ConnectionState.Open) {
                if (con.State != ConnectionState.Broken) {
                    con.Close();
                }
                if (con.State == ConnectionState.Closed) {
                    con.Open();
                }
            }
            return con;
        }
        public static IDbConnection EnsureClosed(this IDbConnection con)
        {
            return con;
            //if (con.State != ConnectionState.Closed)
            //{
            //    con.Close();
            //}
        }
    }
}
