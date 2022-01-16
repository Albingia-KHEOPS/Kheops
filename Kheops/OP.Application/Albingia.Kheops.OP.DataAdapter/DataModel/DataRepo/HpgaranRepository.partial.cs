using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class HpgaranRepository {

        const string selectGarantiesNatureModifiable = @"SELECT
G.KDEID, HG.KDEAVN , 
HG.KDEFOR, HG.KDEOPT, HG.KDEGARAN , 
G.KDESEQ, G.KDENIVEAU, G.KDESEM , 
HG.KDENUMPRES, HG.KDEAJOUT, HG.KDECAR, HG.KDENAT, HG.KDEGAN , 
HG.KDECRAVN, HG.KDEMAJAVN 

FROM HPGARAN HG
INNER JOIN HPOPTD HO ON HG.KDEKDCID = HO.KDCID AND HG.KDEAVN = HO.KDCAVN
INNER JOIN KPOPTD BLOC ON BLOC.KDCIPB = HG.KDEIPB AND BLOC.KDCALX = HG.KDEALX AND BLOC.KDCTYP = HG.KDETYP AND BLOC.KDCKAQID = HO.KDCKAQID
INNER JOIN KPGARAN G ON G.KDEKDCID = BLOC.KDCID AND HG.KDESEQ = G.KDESEQ

WHERE HG.KDETYP = :type
AND HG.KDEIPB = :numeroAffaire
AND HG.KDEALX = :numeroAliment
AND HG.KDEPNTM = 'O'

ORDER BY HG.KDEAVN DESC";

        const string select_GarantieByCodeFilter = @"SELECT G.* FROM HPGARAN WHERE KDEGARAN IN :LST AND ( KDEIPB , KDEALX ) = ( :IPB , :ALX ) ";
        public IEnumerable<KpGaran> GetByAffaireWithNatureModifiable(string type, string numeroAffaire, int numeroAliment) {
            try {
                return connection.EnsureOpened().Query<KpGaran>(selectGarantiesNatureModifiable, new { type, numeroAffaire, numeroAliment }).ToList();
            }
            finally {
                this.connection.EnsureClosed();
            }
        }

        public IEnumerable<KpGaran> GetGarantieByCodeFilter(string numeroAffaire, int numeroAliment, IEnumerable<string> codes)
        {
            return this.connection.EnsureOpened().Query<KpGaran>(select_GarantieByCodeFilter, new { LST = codes.ToList(), IPB = numeroAffaire, ALX = numeroAliment }).ToList();
        }
    }
}
