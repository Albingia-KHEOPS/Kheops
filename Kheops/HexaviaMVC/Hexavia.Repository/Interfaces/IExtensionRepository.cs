using Hexavia.Models;
using System.Collections.Generic;

namespace Hexavia.Repository.Interfaces
{
    public interface IExtensionRepository
    {
        List<CodeLibelle> GetList();
    }
}
