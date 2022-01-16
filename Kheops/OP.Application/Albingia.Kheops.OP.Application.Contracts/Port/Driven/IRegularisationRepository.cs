using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Regularisation;
using ALBINGIA.Framework.Common.Business;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IRegularisationRepository : IRepriseAvenantRepository {
        Regularisation GetById(long id);
        Regularisation GetByAffaire(AffaireId affaireId);
        Regularisation GetBNSByAffaire(AffaireId affaireId);
        void ResetByLot(long idLot);
        long InitLot();
        void AddSelection(SelectionRegularisation selection);
        IEnumerable<SelectionRegularisation> GetSelection(long idLot);
        IEnumerable<SelectionRegularisation> GetSelectionGaranties(long idLot);
        IEnumerable<PeriodeGarantie> GetPeriodesGaranties(long rgId);
        void DeleteSelectionsRisques(long idLot, IEnumerable<int> numsRisques);
        void DeleteSelectionsObjets(long idLot, int numRisque, IEnumerable<int> numsObjets);
        void DeleteSelectionsFormules(long idLot, IEnumerable<int> numsFormules);
        int PurgeTempPeriodes(string codeOffre, int version);
        void InsertBlankTempPeriode(AffaireId affaireId, int risque, int formule, long idGarantie, string codeGarantie, DateTime dateDebut, DateTime dateFin, string typeRegulGarantie);
        IEnumerable<LigneRegularisationGarantieDto> GetDetailsInfoGarantie(long id, long idLot, long idRegul);
        IEnumerable<MouvementGarantie> GetMouvementsGaranties(AffaireId affaireId, long idRegul, int numRisque, int numFormule, AccessMode mode);
        IEnumerable<MouvementGarantie> GetMouvementsGarantie(AffaireId affaireId, long idRegul, int numRisque, int numFormule, string codeGarantie);
        void EnsureInsertMouvementsRC(string codeOffre, int version, int numRisque, int numFormule, long idRegul, long idLot);
        void EnsureInsertMouvements(string codeOffre, int version, int numRisque, int numFormule, long idGarantie, long idRegul);
        Regularisation Create(Regularisation rg, string user);
        void Update(Regularisation rg, string user);
        IEnumerable<RegulRisque> GetRegulRisques(Regularisation rg);
        RegulRisque CreateRegulRisque(RegulRisque risque);
        void TraceInfo(TraceContext traceContext);
        void ResetRsqGar(long reguleId);
    }
}
