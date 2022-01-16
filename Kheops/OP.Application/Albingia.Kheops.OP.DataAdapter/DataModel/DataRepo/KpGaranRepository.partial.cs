using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class KpGaranRepository {
        
        const string select_GarantieByCodeFilter = @"SELECT G.* FROM KPGARAN WHERE KDEGARAN IN :LST AND ( KDEIPB , KDEALX , KDETYP ) = ( :IPB , :ALX , :TYP ) ";

        public IEnumerable<KpGaran> GetGarantieByCodeFilter(string numeroAffaire, int numeroAliment, string typeAffaire, IEnumerable<string> codes) {
            return this.connection.EnsureOpened().Query<KpGaran>(
                select_GarantieByCodeFilter,
                new { LST = codes.ToList(), IPB = numeroAffaire, ALX = numeroAliment, TYP = typeAffaire }).ToList();
        }
    }
}
