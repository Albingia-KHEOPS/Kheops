using Albingia.Kheops.Common;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Domain.Formule
{
    public interface IValidatable
    {
        IEnumerable<ValidationError> Validate();
    }
}