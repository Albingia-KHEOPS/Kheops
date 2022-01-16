using Dapper;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YhrtfooRepository {
        const string delete_ForAffaire = @"DELETE FROM YHRTFOO WHERE ( JPIPB , JPALX , JPAVN ) = ( :codeAffaire , :version , :numeroAvenant ) ";

        public int DeleteForAffaire(string codeAffaire, int version, int numeroAvenant) {
            return this.connection.EnsureOpened().Execute(delete_ForAffaire, new { codeAffaire, version, numeroAvenant });
        }
    }
}
