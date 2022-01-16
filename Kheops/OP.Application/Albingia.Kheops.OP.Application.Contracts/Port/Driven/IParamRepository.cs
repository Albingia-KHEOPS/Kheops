using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formule;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Parametrage.Inventaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.Application.Port.Driven
{
    public interface IParamRepository : IRepositoryCache
    {
        IEnumerable<ParamVolet> GetParamVolets();
        ParamGarantieHierarchie GetParamHierarchie(long seq);

        IEnumerable<ParamExpressionComplexeLCI> GetExpressionLCIs();
        ParamExpressionComplexeLCI GetExpressionLCIByBase(string @base);
        IEnumerable<ParamExpressionComplexeFranchise> GetExpressionFranchises();
        ParamExpressionComplexeFranchise GetExpressionFranchiseByBase(string @base);

        IDictionary<(CaractereSelection caractere, NatureValue nature), NatureSelection> GetParamNatures();

        ParamExpressionComplexeLCI GetExpressionLCI(long id);
        ParamExpressionComplexeFranchise GetExpressionFranchise(long id);
        ParamModeleGarantie GetParamModele(int id);
        ParamGarantie GetGarantie(string code);
        IEnumerable<ParamGarantie> GetAllGaranties();
        ILookup<long, BlocRelation> GetRelationBlocs();
        ILookup<long, GarantieRelation> GetRelationGaranties();
        string GetDesignation(long id);

        IDictionary<long, string> GetDesignations();
        IDictionary<long, TypeInventaire> GetTypeInventaires();
    }
}
