using Hexavia.Business.Interfaces;
using Hexavia.Models;
using Hexavia.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexavia.Business
{
    public class ReferentielBusiness : IReferentielBusiness
    {
        private readonly IReferentielRepository ReferentielRepository;
        public ReferentielBusiness(IReferentielRepository referentielRepository)
        {
            ReferentielRepository = referentielRepository;
        }

        public List<CodeLibelle> GetAllBranches()
        {
            return ReferentielRepository.GetAllBranches();
        }

        public List<CodeLibelle> GetSituationByType(string type)
        {
            return ReferentielRepository.GetSituationByType(type);
        }

        public List<CodeLibelle> GetEtatByType(string type)
        {
            return ReferentielRepository.GetEtatByType(type);
        }

        public List<CodeLibelle> GetEvenement()
        {
            return ReferentielRepository.GetEvenement();
        }
    }
}
