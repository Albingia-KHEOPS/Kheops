using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{
    public partial class KpCopDcRepository
    {
        public void ClearForAffaire(string type, string code, int version,int avenant)
        {
            string sql = string.Format(@"DELETE 
                                         FROM KPCOPDC
                                         WHERE KHQIPB=:code AND KHQALX = :version AND KHQTYP = :type AND KHQAVN = :avn ");
            this.connection.Execute(sql, new { code = code.PadLeft(9, ' '), version, type, avenant });
        }

    }
}
