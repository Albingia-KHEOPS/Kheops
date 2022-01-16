using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Model;
using System.Collections.Generic;

namespace Albingia.Kheops.DTO
{
    public abstract class OptionsDetailDto
    {
        public CaractereSelection Caractere { get; set; }

        public decimal Ordre { get; set; }

        public TypeOption Type { get; set; }

        public int? NumeroAvenant { get; set; }

        public long Id { get; set; }

        public bool IsChecked { get; set; }

        public ParamVoletDto ParamVolet { get; set; }
    }

    public class OptionsDetailVoletDto : OptionsDetailDto {
        public List<OptionsDetailBlocDto> Blocs { get; set; } = new List<OptionsDetailBlocDto>();
        public bool IsHidden => ParamVolet?.Code == Garantie.CodeGareat;
    }

    public class OptionsDetailBlocDto : OptionsDetailDto {
        public ParamBlocDto ParamBloc { get; set; }
        public List<GarantieDto> Garanties { get; set; } = new List<GarantieDto>();
        public bool IsHidden => ParamBloc?.Code == Garantie.CodeGareat;
    }
}