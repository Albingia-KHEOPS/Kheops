using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{
    public partial class YpltGalRepository
    {
        internal void SupprimerBySeq(long[] seqs)
        {
            string strSeqs = string.Join(",", seqs);
            string sql = "DELETE FROM YPLTGAL WHERE C4SEQ IN (" + strSeqs + ")";
            var test = this.connection.EnsureOpened().Execute(sql);
        }
    }
}
