using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{
    public partial class YpoTracRepository
    {
        const string select_ByAffaireLast = @"SELECT PYTYP, PYIPB, PYALX, PYAVN, PYTTR
, PYVAG, PYORD, PYTRA, PYTRM, PYTRJ
, PYTRH, PYLIB, PYINF, PYSDA, PYSDM
, PYSDJ, PYSFA, PYSFM, PYSFJ, PYMJU
, PYMJA, PYMJM, PYMJJ, PYMJH FROM YPOTRAC WHERE ( PYIPB , PYALX , PYAVN ) = ( :codeAffaire , :version , :numeroAvenant) 
ORDER BY (PYTRA * 100000000 + PYTRM * 1000000 + PYTRJ * 10000 + PYTRH) DESC  FETCH FIRST 1 ROWS ONLY";

        const string delete_ByAffaire = "DELETE FROM YPOTRAC WHERE ( PYIPB , PYALX , PYAVN ) = ( :codeAffaire , :version , :numeroAvenant)";

        public IEnumerable<YpoTrac> GetByAffaireLast(string codeAffaire, int version, int numeroAvenant)
        {
            return connection.EnsureOpened().Query<YpoTrac>(select_ByAffaireLast, new { codeAffaire, version, numeroAvenant }).ToList();
        }

        public int DeleteByAffaire(string codeAffaire, int version, int numeroAvenant)
        {
            return this.connection.EnsureClosed().Execute(delete_ByAffaire, new { codeAffaire, version, numeroAvenant });
        }
    }
}