using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using OPWS = OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common.Business;

namespace Albingia.Kheops.OP.Application.Port.Driver {
    [ServiceContract]
    public interface IRegularisationPort {
        [OperationContract]
        RegularisationDto GetDto(long id);
        [OperationContract]
        RegularisationDto GetDtoByAffaire(AffaireId affaireId);
        [OperationContract]
        RegularisationDto GetBNSDtoByAffaire(AffaireId affaireId);
        [OperationContract]
        long ResetByLot(long idLot);
        [OperationContract]
        void AddSelectionRisque(SelectionRisqueRegularisationDto selectionRsq);
        [OperationContract]
        void AddSelectionObjet(SelectionObjetRegularisationDto selectionObj);
        [OperationContract]
        void AddSelectionFormule(SelectionFormuleRegularisationDto selectionFrm);
        [OperationContract]
        void AddSelection(SelectionRegularisationDto selection);
        [OperationContract]
        RegularisationDto AddOrUpdateWithRsq(OPWS.RegularisationContext regularisationContext);
        [OperationContract]
        RegularisationDto AddOrUpdate(RegularisationDto regularisationDto, long lotId);

        /// <summary>
        /// Delete risques+formules without garantie
        /// </summary>
        [OperationContract]
        void CleanUpSelections(long idLot);

        [OperationContract]
        IEnumerable<SelectionRegularisationDto> GetSelection(long idLot);

        /// <summary>
        /// Gets simple regularisation selection. Assumes that the number of periods has been checked
        /// </summary>
        /// <param name="idLot">Regul batch id</param>
        /// <returns>The unique regul. selection and RC garanties group if found</returns>
        [OperationContract]
        IEnumerable<SelectionRegularisationDto> GetSelectionGaranties(long idLot);

        [OperationContract]
        bool HasMultiRCSelection(long idLot, long garId = 0);

        [OperationContract]
        (DateTime debut, DateTime fin) GetPeriodBounds(AffaireId affaireId, long reguleId);

        [OperationContract]
        IEnumerable<GarantieDto> GetGaranties(string codeAffaire, int version, DateTime debut, DateTime fin, bool fullHisto = true);

        /// <summary>
        /// Créer les périodes vides (guwp) si le programme 400 n'identifie pas de mouvements de garantie pour un contrat en périodicité R
        /// Doit être exécuté apres l'appel de KDA301CL
        /// </summary>
        /// <param name="affaireId"></param>
        /// <param name="rgId"></param>
        /// <param name="risque"></param>
        /// <param name="formule"></param>
        /// <param name="idGarantie"></param>
        /// <param name="codeGarantie"></param>
        [OperationContract]
        void EnsureTempPeriodes(AffaireId affaireId, long rgId, int risque, int formule, long idGarantie, string codeGarantie, AccessMode mode);

        /// <summary>
        /// Créer les périodes vides (guwp) si le programme 400 n'identifie pas de mouvements des garanties RC pour un contrat en périodicité R
        /// Doit être exécuté apres l'appel de KDA301CL pour toutes RC et avant le KDA302CL
        /// </summary>
        /// <param name="affaireId"></param>
        /// <param name="rgId"></param>
        /// <param name="risque"></param>
        /// <param name="formule"></param>
        /// <param name="idsRC">Identifiants des garanties RC (KPGARAN)</param>
        [OperationContract]
        void EnsureTempPeriodesRC(AffaireId affaireId, long rgId, int risque, int formule, long[] idsRC, AccessMode mode);

        [OperationContract]
        IEnumerable<PeriodeGarantieDto> GetPeriodesGaranties(long rgId);

        [OperationContract]
        int PurgeTempPeriodes(AffaireId affaireId);

        [OperationContract]
        bool CanSimplifyStandard(long idLot);

        [OperationContract]
        OPWS.LigneRegularisationGarantieDto GetDetailsInfoGarantie(long id, long idLot, long idRegul);

        [OperationContract]
        (DateTime min, DateTime max) GetMouvementsMinMax(AffaireId affaireId, long rgId, int numRisque, int numFormule, AccessMode mode);

        [OperationContract]
        IEnumerable<OPWS.LigneMouvementDto> GetMouvementsGarantie(AffaireId affaireId, long rgId, int numRisque, int numFormule, string codeGarantie);

        [OperationContract]
        void EnsureInsertMouvements(OPWS.RegularisationContext context);
    }
}
