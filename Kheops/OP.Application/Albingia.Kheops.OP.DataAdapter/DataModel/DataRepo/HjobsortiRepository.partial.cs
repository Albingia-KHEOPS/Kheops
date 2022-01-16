using Dapper;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class HjobsortiRepository {
        const string delete_ByRisque = "DELETE FROM KJOBSORTI WHERE ( IPB , ALX , AVN , RSQ ) = ( :codeAffaire , :version , :numeroAvenant , :risque )";
        const string delete_ByOption = "DELETE FROM KJOBSORTI WHERE ( IPB , ALX , AVN, OPT ) = ( :codeAffaire , :version , :numeroAvenant , :option )";

        public int DeleteByRisque(string codeAffaire, int version, int numeroAvenant, int risque) {
            return this.connection.EnsureOpened().Execute(delete_ByRisque, new { codeAffaire, version, numeroAvenant, risque });
        }

        public int DeleteByOption(string codeAffaire, int version, int numeroAvenant, int option) {
            return this.connection.EnsureOpened().Execute(delete_ByOption, new { codeAffaire, version, numeroAvenant, option });
        }
    }
}
