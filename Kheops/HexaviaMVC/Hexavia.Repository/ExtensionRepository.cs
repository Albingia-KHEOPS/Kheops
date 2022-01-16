using Hexavia.Repository.Interfaces;
using Hexavia.Models;
using System.Collections.Generic;

namespace Hexavia.Repository
{
    public class ExtensionRepository : BaseRepository, IExtensionRepository
    {
        public ExtensionRepository(DataAccessManager dataAccessManager)
          : base(dataAccessManager)
        {
        }

        public List<CodeLibelle> GetList()
        {
            var liste = new List<CodeLibelle>
                {
                    new CodeLibelle { Code = "", Libelle = "" },
                    new CodeLibelle { Code = "B", Libelle = "Bis" },
                    new CodeLibelle { Code = "T", Libelle = "Ter" },
                    new CodeLibelle { Code = "a", Libelle = "a" },
                    new CodeLibelle { Code = "b", Libelle = "b" },
                    new CodeLibelle { Code = "c", Libelle = "c" },
                    new CodeLibelle { Code = "d", Libelle = "d" },
                    new CodeLibelle { Code = "e", Libelle = "e" },
                    new CodeLibelle { Code = "f", Libelle = "f" }
                };

            return liste;
        }
    }
}
