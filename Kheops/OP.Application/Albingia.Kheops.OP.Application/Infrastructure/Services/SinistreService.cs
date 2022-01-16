using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services {
    public class SinistreService : ISinistrePort {
        private readonly ISinistreRepository sinistreRepository;
        private readonly IUserPort userService;
        public SinistreService(ISinistreRepository sinistreRepository, IUserPort userService) {
            this.sinistreRepository = sinistreRepository;
            this.userService = userService;
        }

        public PagingList<SinistreDto> GetSinistres(int page = 0, int codeAssure = 0) {
            var profile = this.userService.GetProfile();
            var pagingList = this.sinistreRepository.GetAll(profile.Branches.Select(x => x.branche).ToArray(), page, codeAssure);
            var impayes = pagingList.List.Select(s => s.Adapt<SinistreDto>()).ToList();

            return new PagingList<SinistreDto> {
                NbTotalLines = pagingList.NbTotalLines,
                PageNumber = pagingList.PageNumber,
                List = impayes,
                Totals = pagingList.Totals
            };
        }

        public IEnumerable<SinistreDto> GetSinistresPreneur(int codePreneur) {
            return this.sinistreRepository.GetAllOfPreneur(codePreneur).Select(s => s.Adapt<SinistreDto>()).ToList();
        }

        public decimal GetTotalChargementsSinistresPreneur(int codePreneur) {
            return this.sinistreRepository.GetSumOfChargementsSinistresPreneur(codePreneur);
        }
    }
}
