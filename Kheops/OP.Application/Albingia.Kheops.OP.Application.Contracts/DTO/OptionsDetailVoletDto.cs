using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Model;
using System.Collections.Generic;

namespace Albingia.Kheops.DTO {
    public class DetailVoletDto {
        public List<OptionsDetailBlocDto> Blocs { get; set; } = new List<OptionsDetailBlocDto>();
        public CaractereSelection Caractere { get; set; }

        public decimal Ordre { get; set; }

        public TypeOption Type { get; set; }

        public int? NumeroAvenant { get; set; }

        public long Id { get; set; }

        public bool IsChecked { get; set; }

        public ParamVoletDto ParamVolet { get; set; }
    }
}