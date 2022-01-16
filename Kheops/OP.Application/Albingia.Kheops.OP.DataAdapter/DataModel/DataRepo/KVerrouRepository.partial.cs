using Dapper;
using ALBINGIA.Framework.Common.Extensions;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class KVerrouRepository {
        const string count_Verrous = "SELECT COUNT( 1 ) FROM KVERROU WHERE KAVVERR = 'O' ";

		public int CountVerrou() {
            return this.connection.EnsureOpened().ExecuteScalar(count_Verrous).ToInteger().Value;
        }
    }
}
