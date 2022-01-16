using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Referentiel;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Application.Port.Driven
{
    public interface IAffaireRepository : IRepriseAvenantRepository
    {
        AffaireId GetCurrentId(string codeAffaire, int version);
        AffaireId GetPriorId(string codeAffaire, int version, int avenant);
        AffaireId ValidateId(AffaireId affaireId);
        Affaire GetById(AffaireType type, string codeAffaire, int aliment, int? numAvenant = null, bool isHisto=false);
        Affaire GetById(AffaireId id);
        Affaire GetByIdWithoutDependencies(string code, int aliment = 0, int? avenant = null);
        Affaire GetLightByIpbAlx(string ipb, int alx);
        IEnumerable<Affaire> GetFullHisto(string codeAffaire, int aliment);
        Affaire SaveAffaire(Affaire aff, User user);
        IEnumerable<NouvelleAffaire> GetTempAffairs(AffaireId id);
        void SaveConditions(Affaire affaire, string user);
        void SaveSelectionRisquesNouvelleAffaire(NouvelleAffaire nouvelleAffaire, int? numRisque = null);
        void SaveSelectionFormulesNouvelleAffaire(NouvelleAffaire nouvelleAffaire, int? numFormule = null);

        void SaveNewAffair(Affaire affair, string user, string ipb, int alx = 0);
        IDictionary<AffaireId, LockState> GetUserLocks(string user);
        LockState GetLockState(AffaireId affaireId);
        LockState SetLockState(AffaireId affaireId, LockState state);
        IEnumerable<Affaire> GetListByIds(IEnumerable<AffaireId> idList);
        Affaire GetAffaireCanevas(int templateId);
        void SetGareat(string ipb, int alx, string trancheMax, string flag);
        void UpdateLCI(string codeAffaire, int version, ValeursOptionsTarif lci, bool isIndexee);
        string GetAffaireTraitement(AffaireId id);
    }
}