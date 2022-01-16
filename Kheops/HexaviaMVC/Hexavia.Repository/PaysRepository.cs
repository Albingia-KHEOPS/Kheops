using System.Collections.Generic;
using System.Linq;
using Hexavia.Repository.Interfaces;
using Hexavia.Models;
using log4net;

namespace Hexavia.Repository
{
    public class PaysRepository : BaseRepository, IPaysRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PaysRepository));
        private readonly IParametreRepository parametreRepository;

        public PaysRepository(DataAccessManager dataAccessManager, IParametreRepository parametreRepository)
           : base(dataAccessManager)
        {
            this.parametreRepository = parametreRepository;
        }

        public List<Pays> GetPays()
        {
            var result = parametreRepository.Load("GENER", "CPAYS")
                .Select(x => new Pays
                {
                    Code = x.Code,
                    Libelle = x.Libelle.ToString().Substring(2)
                }).ToList();

            return result;
        }
    }
}
