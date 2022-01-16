using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    public class TarifGeneral {
        public long IdExpCpx { get; set; }
        public decimal ValeurOrigine { get; set; }
        public decimal ValeurActualisee { get; set; }
        public Unite Unite { get; set; }
        public BaseDeCalcul Base { get; set; }

        public void Reset(TarifGeneral newTarif = null) {
            if (newTarif is null) {
                IdExpCpx = default;
                ValeurActualisee = default;
                ValeurOrigine = default;
                Unite = new Unite { Code = string.Empty };
                Base = new BaseDeCalcul { Code = string.Empty };
            }
            else {
                if (Base.Code != newTarif.Base?.Code) Base = new BaseDeCalcul { Code = newTarif.Base?.Code ?? string.Empty };
                if (IdExpCpx != newTarif.IdExpCpx) IdExpCpx = newTarif.IdExpCpx;
                if (Unite.Code != newTarif.Unite?.Code) Unite = new Unite { Code = newTarif.Unite?.Code ?? string.Empty };
                if (ValeurActualisee != newTarif.ValeurActualisee) ValeurActualisee = newTarif.ValeurActualisee;
                if (ValeurOrigine != newTarif.ValeurOrigine) ValeurOrigine = newTarif.ValeurOrigine;
            }
        }

        public virtual bool ValuesEqual(TarifGeneral tarif) {
            return IdExpCpx == tarif?.IdExpCpx
                && ValeurActualisee == tarif?.ValeurActualisee
                && ValeurOrigine == tarif?.ValeurOrigine
                && Unite == tarif?.Unite
                && Base == tarif?.Base;
        }
    }
}
