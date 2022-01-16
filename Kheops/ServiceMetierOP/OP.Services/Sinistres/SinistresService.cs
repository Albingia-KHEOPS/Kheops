using Albingia.Kheops.DTO;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using ALBINGIA.Framework.Common.Tools;
using OP.Services.REST.wsadel;
using Albingia.Kheops.OP.Application.Port.Driver;

namespace OP.Services {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SinistresService : ISinistres {
        private readonly SinistresRestClient restClient;
        private readonly ISinistrePort sinistreService;
        public SinistresService(SinistresRestClient sinistresRestClient, ISinistrePort sinistreService) {
            this.restClient = sinistresRestClient;
            this.sinistreService = sinistreService;
        }
        public SinistreDto CalculProvisionsPrevisionsChargement(SinistreDto sinistre, DateTime? dateDebut = null, DateTime? dateFin = null) {
            return this.restClient.GetCalculsChargementAsync(sinistre, dateDebut, dateFin).Result;
        }

        public decimal TotalByPreneurCalculProvisionsPrevisionsChargement(int codePreneur) {
            //var sinistres = this.sinistreService.GetSinistresPreneur(codePreneur);
            //var list = new List<SinistreDto>();
            //sinistres.ParallelForEachAsync(async (s) => { var r = await this.restClient.GetCalculsChargementAsync(s); list.Add(r); }, 50).Wait();
            //return list.Any() ? list.Sum(x => x?.CalculChargement?.TotalChargement ?? decimal.Zero) : 0;
            return this.sinistreService.GetTotalChargementsSinistresPreneur(codePreneur);
        }
    }
}
