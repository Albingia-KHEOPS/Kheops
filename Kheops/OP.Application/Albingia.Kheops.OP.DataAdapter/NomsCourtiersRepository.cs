using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter {
    public class NomsCourtiersRepository : INomsCourtiersRepository {
        private readonly YCourtNRepository yCourtnRepository;
        public NomsCourtiersRepository(YCourtNRepository yCourtnRepository) {
            this.yCourtnRepository = yCourtnRepository;
        }
        public IDictionary<int, string> GetNomsCabinets(IEnumerable<int> codeList) {
            if (codeList?.Any() != true) {
                return new Dictionary<int, string>();
            }
            var result = codeList.ToDictionary(i => i, i => string.Empty);
            var names = this.yCourtnRepository.SelectNomsCabinets(codeList.ToList());
            names.AsParallel().ForAll(x => result[x.Tnict] = x.Tnnom);
            return result;
        }
    }
}
