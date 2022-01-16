using Albingia.Kheops.OP.Domain.Formule;
using ALBINGIA.Framework.Common.Extensions;

namespace Albingia.Kheops.DTO {
    public class PorteeGarantieDto
    {
        public bool IsSelected { get; set; }
        public long Id { get; set; }
        public long GarantieId { get; set; }
        public int RisqueId { get; set; }
        public int NumObjet { get; set; }
        public TypeCalcul TypeCalcul { get; set; }
        public ValeursUnite Valeurs { get; set; }
        public string CodeAction { get; set; }
        public bool IsRemoved {
            get {
                return !IsSelected || !CodeAction.ContainsChars();
            }
        }
    }
}
