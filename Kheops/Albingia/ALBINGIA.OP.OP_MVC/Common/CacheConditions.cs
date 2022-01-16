using Albingia.Kheops.DTO;
using Albingia.Mvc.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ALBINGIA.OP.OP_MVC.Common {
    public class CacheConditions {
        private readonly Dictionary<AccesAffaire, ConditionsAffaireDto> cacheGeneralInit = new Dictionary<AccesAffaire, ConditionsAffaireDto>();
        private readonly ConcurrentDictionary<AccesAffaire, ConditionsAffaireDto> cacheGeneral = new ConcurrentDictionary<AccesAffaire, ConditionsAffaireDto>();
        private readonly Dictionary<AccesAffaire, IEnumerable<ConditionGarantieDto>> cacheInit = new Dictionary<AccesAffaire, IEnumerable<ConditionGarantieDto>>();
        private readonly ConcurrentDictionary<AccesAffaire, IEnumerable<ConditionGarantieDto>> cache = new ConcurrentDictionary<AccesAffaire, IEnumerable<ConditionGarantieDto>>();

        public void SaveConditionsGenerales(AccesAffaire accesAffaire, ConditionsAffaireDto data, ConditionsAffaireDto initialValue = null) {
            if (!this.cacheGeneral.TryGetValue(accesAffaire, out var conditions)) {
                this.cacheGeneral.TryAdd(accesAffaire, initialValue);
                conditions = initialValue;
                this.cacheGeneralInit[accesAffaire] = initialValue;
            }

            if (ConditionUpdates(conditions, data)) {
                this.cacheGeneral.TryUpdate(accesAffaire, data, conditions);
            }
        }

        public void SaveConditionsGarantie(AccesAffaire accesAffaire, ConditionGarantieDto data, IEnumerable<ConditionGarantieDto> initialList = null) {
            if (!this.cache.TryGetValue(accesAffaire, out var conditions)) {
                this.cache.TryAdd(accesAffaire, initialList);
                conditions = initialList;
                this.cacheInit[accesAffaire] = initialList;
            }

            var current = conditions.Single(c => c.IdGarantie == data.IdGarantie);
            if (ConditionUpdates(current, data)) {
                var list = new List<ConditionGarantieDto>(conditions);
                list.Remove(current);
                list.Add(data);
                this.cache.TryUpdate(accesAffaire, list, conditions);
            }
        }

        public ConditionGarantieDto RollbackCondition(AccesAffaire accesAffaire, int idGarantie) {
            if (!this.cacheInit.ContainsKey(accesAffaire)) {
                return null;
            }
            var oldValue = this.cacheInit[accesAffaire].Single(c => c.IdGarantie == idGarantie);
            SaveConditionsGarantie(accesAffaire, oldValue);
            return oldValue;
        }

        private bool ConditionUpdates(ConditionGarantieDto current, ConditionGarantieDto data) {
            if (current.IdGarantie != data.IdGarantie) {
                throw new ArgumentException($"Unable to compare Conditions, {nameof(data.IdGarantie)} do not have the same value", nameof(data));
            }

            return !current.AssietteGarantie.ValuesEqual(data.AssietteGarantie)
                || !current.TarifsGarantie.LCI.ValuesEqual(data.TarifsGarantie.LCI)
                || !current.TarifsGarantie.Franchise.ValuesEqual(data.TarifsGarantie.Franchise)
                || !current.TarifsGarantie.PrimeValeur.ValuesEqual(data.TarifsGarantie.PrimeValeur);
        }

        private bool ConditionUpdates(ConditionsAffaireDto current, ConditionsAffaireDto data) {
            if (current.AffaireId.CodeAffaire != data.AffaireId.CodeAffaire
                || current.AffaireId.NumeroAliment != data.AffaireId.NumeroAliment) {
                throw new ArgumentException($"Unable to compare Conditions, {nameof(data.AffaireId)} do not have the same value", nameof(data));
            }
            if (current.AffaireId.IsHisto) {
                throw new ArgumentException("Impossible de mettre à jour des conditions historisées", $"{nameof(current)}.{nameof(current.AffaireId)}.{nameof(current.AffaireId.IsHisto)}");
            }
            //return false;
            return !current.Franchise.ValuesEqual(data.Franchise)
                || !current.LCI.ValuesEqual(data.LCI)
                || !current.FranchiseRisque.ValuesEqual(data.FranchiseRisque)
                || !current.LCIRisque.ValuesEqual(data.LCIRisque);
        }

        public bool MustSave(AccesAffaire accesAffaire) {
            if (this.cache.TryGetValue(accesAffaire, out var list)) {
                var initial = this.cacheInit[accesAffaire];
                return initial.Any(c => ConditionUpdates(c, list.Single(x => x.IdGarantie == c.IdGarantie)));
            }
            return false;
        }

        public void Erase(AccesAffaire accesAffaire) {
            if (this.cache.TryRemove(accesAffaire, out var x)) {
                this.cacheInit.Remove(accesAffaire);
            }
        }
    }
}