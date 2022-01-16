
using System;

namespace Albingia.Kheops.OP.DataAdapter {
    [Flags]
    internal enum RawDataFilter {
        Any = 0,
        Formula = 0x1,
        Options = 0x2,
        Designations = 0x4,
        Applications = 0x8,
        DetailsOption = 0x10,
        Garanties = 0x20,
        TarifsGaranties = 0x40,
        Portees = 0x80,
        Inventaires = 0x100,
        ExpressionComplexes = 0x200,
        GarantiesNatureModifiable = 0x400
    }
}