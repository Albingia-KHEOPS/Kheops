using Hexavia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexavia.Repository.Interfaces
{
    public interface IReferentielRepository
    {
        List<CodeLibelle> GetAllBranches();

        List<CodeLibelle> GetSituationByType(string type);

        List<CodeLibelle> GetEtatByType(string type);

        List<CodeLibelle> GetEvenement();
    }
}
