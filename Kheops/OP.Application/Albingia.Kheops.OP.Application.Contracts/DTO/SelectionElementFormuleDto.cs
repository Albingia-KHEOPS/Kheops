using Albingia.Kheops.OP.Domain.Formule;

namespace Albingia.Kheops.DTO {
    public class SelectionElementFormuleDto {
        public TypeElement TypeElement { get; set; }
        public bool Selected { get; set; }
        public long IdElement { get; set; }
    }
}
