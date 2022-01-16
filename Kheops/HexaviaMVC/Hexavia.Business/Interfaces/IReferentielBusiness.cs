using Hexavia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexavia.Business.Interfaces
{
    public interface IReferentielBusiness
    {
        List<CodeLibelle> GetAllBranches();

        List<CodeLibelle> GetSituationByType(string type);

        List<CodeLibelle> GetEtatByType(string type);
        List<CodeLibelle> GetEvenement();
    }
}
