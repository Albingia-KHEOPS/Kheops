using Albingia.Kheops.OP.Domain.Model;
using System.Collections.Generic;

namespace Albingia.Kheops.DTO
{
    public  class BlocDto
    {
        public CaractereSelection Caractere { get; set; }

        public List<GarantieDto> Garanties { get; private set; } = new List<GarantieDto>();
    }
}