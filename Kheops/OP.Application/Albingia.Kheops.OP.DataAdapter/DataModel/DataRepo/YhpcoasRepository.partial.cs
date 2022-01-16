using Dapper;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YhpcoasRepository {
        const string delete_ByAffaire = @"DELETE FROM YHPCOAS WHERE ( PHIPB , PHALX , PHAVN ) = ( :code , :version , :numeroAvenant ) ";

        public int DeleteByAffaire(string code, int version, int numeroAvenant) {
            return this.connection.EnsureOpened().Execute(delete_ByAffaire, new { code, version, numeroAvenant });
        }
    }
}
