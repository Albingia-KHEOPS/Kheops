using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YhpassxRepository {
        const string delete_ByAffaire = @"DELETE FROM YHPASSX WHERE ( PDIPB , PDALX , PDAVN , PDHIN ) = ( :code , :version , :numeroAvenant , :versionHisto )";

        public int DeleteByAffaire(string code, int version, int numeroAvenant, int versionHisto = 1) {
            return this.connection.EnsureOpened().Execute(delete_ByAffaire, new { code, version, numeroAvenant, versionHisto });
        }
    }
}
