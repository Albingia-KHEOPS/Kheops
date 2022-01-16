using System.Collections.Generic;

namespace Albingia.Kheops.DTO
{
    public class VoletDto
    {
        public VoletDto()
        {
            this.Blocs = new List<BlocDto>();
        }
        public List<BlocDto> Blocs {get; private set; }
    }
}