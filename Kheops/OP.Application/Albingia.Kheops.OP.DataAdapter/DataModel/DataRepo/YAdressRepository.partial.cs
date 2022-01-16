using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YAdressRepository {
        public int DeleteMultiple(IEnumerable<int> adhIds) {
            if (adhIds?.Any() != true) {
                return 0;
            }

            return this.connection.EnsureOpened().Execute("DELETE FROM YADRESS WHERE ABPCHR IN :adhIds ", new { adhIds });
        }
    }
}
