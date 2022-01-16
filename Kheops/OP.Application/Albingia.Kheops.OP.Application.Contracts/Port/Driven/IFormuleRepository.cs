using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Risque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driven
{
    public interface IFormuleReadRepository
    {
        IEnumerable<Formule> GetFormulesForAffaire(AffaireId id);
        (IEnumerable<Formule> formule, Expressions expressions) GetFormulesExpressionForAffaire(AffaireId id);
        IEnumerable<Option> GetOptionsApplicationsFullHisto(string codeAffaire, int version);
        IEnumerable<Option> GetOptionsSimple(AffaireId id);
        IEnumerable<Garantie> GetCurrentGaranties(string codeAffaire, int version);
        IEnumerable<Garantie> GetGarantiesFullHisto(string codeAffaire, int version);
        IEnumerable<int> GetNumerosFormules(AffaireId affaireId, int numeroRisque);
        IEnumerable<OptionBloc> GetOptionsBlocsByFormule(AffaireId affaireId, int formule);
    }
    public interface IFormuleSaveRepository : IRepriseAvenantRepository
    {
        void SaveFormule(Risque risque, Formule formuleToSave, string username);
        void Delete(Formule formule);
    }

    public interface IFormuleRepository : IFormuleReadRepository, IFormuleSaveRepository { }
}
