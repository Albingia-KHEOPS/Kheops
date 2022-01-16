using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Avenant {
    public interface IAvenantDto {
        string Type { get; }
        long NumInterne { get; }
        string Description { get; }
        string Observations { get; }
        long Numero { get; }
        DateTime? Date { get; }
        string Motif { get; }
        bool IsModifHorsAvenant { get; }
    }
}
