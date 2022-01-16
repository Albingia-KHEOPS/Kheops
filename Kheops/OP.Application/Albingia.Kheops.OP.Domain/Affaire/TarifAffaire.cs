using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Affaire {
    public class TarifAffaire : TarifGeneral {
        public bool Indexe { get; set; }
        public void Reset(TarifAffaire newTarif = null) {
            base.Reset(newTarif);
            if (newTarif is null) Indexe = false;
            else if (Indexe != newTarif.Indexe) Indexe = newTarif.Indexe;
        }

        public bool ValuesEqual(TarifAffaire tarif) {
            return base.ValuesEqual(tarif) && Indexe == tarif?.Indexe;
        }
    }
}
