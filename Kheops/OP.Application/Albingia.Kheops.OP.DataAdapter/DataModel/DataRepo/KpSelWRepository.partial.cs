using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Dapper;
using OP.WSAS400.DTO.Regularisation;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class KpSelWRepository {
        const string insert = @"
INSERT INTO KPSELW ( KHVID, KHVTYP, KHVIPB, KHVALX, KHVPERI, KHVRSQ, KHVOBJ, KHVFOR, KHVKDEID, KHVEDTB, KHVDEB, KHVFIN, KHVECO, KHVAVN, KHVKDESEQ, KHVKDEGAR ) 
VALUES ( :id, :typ, :ipb, :alx, :peri, :rsq, :obj, :for, :kdeid, :edtb, :deb, :fin, :eco, :avn, :seq, :gar ) ";
        const string deleteByLot = @"DELETE FROM KPSELW WHERE KHVID = :idLot ";
        const string delete_Risques = @"DELETE FROM KPSELW WHERE KHVID = :idLot AND KHVRSQ IN :nums ";
        const string delete_Objets = @"DELETE FROM KPSELW WHERE ( KHVID , KHVRSQ ) = ( :idLot , :rsq ) AND KHVOBJ IN :nums ";
        const string delete_Formules = @"DELETE FROM KPSELW WHERE KHVID = :idLot AND KHVFOR IN :nums ";
        const string select_GarantieDto = @"
SELECT DISTINCT KABRSQ CodeRisque , KABDESC RisqueIdentification , JERGT CodeTaxeRegime , IFNULL(YPAREGIME.TPLIL, '') RegimeTaxe, KDAFOR CodeFormule, KDADESC FormuleDescriptif , KDAALPHA LettreFor,
    GADES LibGarantie, GAGAR CodeGar, SELEGAR.KHVKDEID IdGar, KHWDEBP DateDebPeriodGenerale, KHWFINP DateFinPeriodGenerale , JERUT CodeTypeRegule, IFNULL(YPAREGUL.TPLIL, '') TypeRegul, 
    IFNULL(G.KDETAXCOD, HG.KDETAXCOD) CodeTaxeGar
FROM KPSELW SELEGAR
INNER JOIN KPRGU ON ( KHVIPB , KHVALX , KHVTYP ) = ( KHWIPB , KHWALX , KHWTYP ) 
INNER JOIN KPRSQ ON KABIPB = SELEGAR.KHVIPB AND KABALX = SELEGAR.KHVALX AND KABTYP = SELEGAR.KHVTYP AND KABRSQ = SELEGAR.KHVRSQ
INNER JOIN YPRTRSQ ON JEIPB = SELEGAR.KHVIPB AND JEALX = SELEGAR.KHVALX  AND JERSQ = SELEGAR.KHVRSQ   
LEFT JOIN YYYYPAR YPAREGIME ON YPAREGIME.TCON = 'GENER' AND YPAREGIME.TFAM = 'TAXRG'  AND YPAREGIME.TCOD = JERGT
INNER JOIN KPFOR ON KDAIPB = SELEGAR.KHVIPB AND KDAALX = SELEGAR.KHVALX AND KDATYP = SELEGAR.KHVTYP AND KDAFOR = SELEGAR.KHVFOR
LEFT JOIN KPGARAN G ON G.KDEID = SELEGAR.KHVKDEID 
LEFT JOIN HPGARAN HG ON HG.KDEID = SELEGAR.KHVKDEID 
INNER JOIN KGARAN ON IFNULL(G.KDEGARAN, HG.KDEGARAN) = GAGAR 
LEFT JOIN YYYYPAR YPAREGUL ON YPAREGUL.TCON = 'PRODU' AND YPAREGUL.TFAM = 'JERUT' AND YPAREGUL.TCOD = JERUT 
WHERE KHVID = :idLot  AND KHVKDEID = :idGarantie AND KHWID = :idRegul ;";

        public int DeleteLot(long idLot) {
            try {
                return connection.EnsureOpened().Execute(deleteByLot, new { idLot });
            }
            finally {
                this.connection.EnsureClosed();
            }
        }

        public int NewId() {
            var parameters = new DynamicParameters();
            parameters.Add("P_CHAMP", "KHVID", dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input);
            parameters.Add("P_SEQ", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            try {
                connection.EnsureOpened().Execute("SP_NCHRONO", parameters, commandType: CommandType.StoredProcedure);
            }
            finally {
                connection.EnsureClosed();
            }
            var result = parameters.Get<int>("P_SEQ");
            return result;
        }

        public int Insert(KpSelW selW) {
            return connection.EnsureOpened().Execute(
                insert,
                new {
                    id = selW.Khvid,
                    typ = selW.Khvtyp,
                    ipb = selW.Khvipb,
                    alx = selW.Khvalx,
                    peri = selW.Khvperi,
                    rsq = selW.Khvrsq,
                    obj = selW.Khvobj,
                    @for = selW.Khvfor,
                    kdeid = selW.Khvkdeid,
                    edtb = selW.Khvedtb,
                    deb = selW.Khvdeb,
                    fin = selW.Khvfin,
                    eco = selW.Khveco,
                    avn = selW.Khvavn,
                    seq = selW.Khvkdeseq,
                    gar = selW.Khvkdegar ?? string.Empty
                });
        }

        public int DeleteRisques(long idLot, IEnumerable<int> numsRisques) {
            return connection.EnsureOpened().Execute(delete_Risques, new { idLot, nums = numsRisques.Where(x => x > 0).ToList() });
        }

        public int DeleteObjets(long idLot, int numRisque, IEnumerable<int> numsObjets) {
            return connection.EnsureOpened().Execute(delete_Objets, new { idLot, rsq = numRisque, nums = numsObjets.Where(x => x > 0).ToList() });
        }

        public int DeleteFormules(long idLot, IEnumerable<int> numsFormules) {
            return connection.EnsureOpened().Execute(delete_Formules, new { idLot, nums = numsFormules.Where(x => x > 0).ToList() });
        }

        public IEnumerable<LigneRegularisationGarantieDto> SelectInfoDetailsGarantie(long idGarantie, long idLot, long idRegul) {
            var o = this.connection.EnsureOpened().Query<LigneRegularisationGarantieDto>(select_GarantieDto, new { idLot, idGarantie, idRegul }).ToList();
            var o1 = o.FirstOrDefault();
            return o;
        }
    }
}
