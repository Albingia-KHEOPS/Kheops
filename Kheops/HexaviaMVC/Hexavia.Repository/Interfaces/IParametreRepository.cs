using Hexavia.Models;
using System;
using System.Collections.Generic;

namespace Hexavia.Repository.Interfaces
{
    /// <summary>
    /// IParametresRepository
    /// </summary>
    public interface IParametreRepository
    {
        List<Parametre> Load(string TCON, string TFAM, string TPCA1 = null, List<String> TCOD = null);
    }
}
