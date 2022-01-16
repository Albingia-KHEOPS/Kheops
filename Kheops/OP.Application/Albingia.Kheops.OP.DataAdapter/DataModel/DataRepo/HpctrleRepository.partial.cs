using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class HpctrleRepository {
        const string delete_byAffaire = @"DELETE FROM HPCTRLE WHERE ( KEVIPB , KEVALX , KEVTYP , KEVAVN ) = ( :ipb , :alx , :typ , :avn ) ";
        public void DeleteByAffaire(string KEVTYP, string KEVIPB, int KEVALX, int KEVAVN) {
            this.connection.EnsureOpened().Execute(delete_byAffaire, new { ipb = KEVIPB, alx = KEVALX, typ = KEVTYP, avn = KEVAVN });
        }
    }
}
