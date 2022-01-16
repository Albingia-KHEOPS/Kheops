using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{
    public partial class YpltGaaRepository
    {
        internal void SupprimerBySeq(long[] seqs)
        {
            string strSeqs = string.Join(",", seqs);
            string sql = "DELETE FROM YPLTGAA WHERE C5SEQ IN (" + strSeqs + ") OR C5SEM IN (" + strSeqs + ")";
            this.connection.EnsureOpened().Execute(sql, new { strSeqs });
        }
    }
}
