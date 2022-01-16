using System.Collections.Generic;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Clauses;

namespace Albingia.Kheops.OP.Application.Port.Driven
{
    public interface IClauseRepository : IRepriseAvenantRepository
    {
        ParamClause GetClauseParam(long id);
        IEnumerable<ParamClause> GetClauseParams();
        IEnumerable<Clause> GetClauses(AffaireId id);
    }
}