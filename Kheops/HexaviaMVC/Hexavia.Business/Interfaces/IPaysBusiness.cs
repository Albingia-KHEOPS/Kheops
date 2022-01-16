using Hexavia.Models;
using System.Collections.Generic;

namespace Hexavia.Business.Interfaces
{
    public interface IPaysBusiness
    {
        List<Pays> GetPays();
    }
}
