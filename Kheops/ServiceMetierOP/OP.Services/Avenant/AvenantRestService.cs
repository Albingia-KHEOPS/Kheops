using Albingia.Kheops.OP.Application.Infrastructure.Services;
using Albingia.Kheops.OP.Application.Port.Driver;
using ALBINGIA.Framework.Common.Extensions;
using OP.DataAccess;
using OP.Services.BLServices;
using OP.WSAS400.DTO.Avenant;
using RestContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace OP.Services.Web {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AvenantRestService : IWebAvenant {

        private readonly IDbConnection connection;
        private readonly RegularisationRepository regularisationRepository;
        private readonly RemiseEnVigueurRepository remiseEnVigueurRepository;
        private readonly CibleService cibleService;
        private readonly RegularisationService regularisationService;
        private readonly AffaireNouvelleRepository repository;
        private readonly IAffairePort affaireService;

        public AvenantRestService(IDbConnection connection, IAffairePort affaireService, AffaireNouvelleRepository repository, RegularisationRepository regularisationRepository, RemiseEnVigueurRepository remiseEnVigueurRepository, CibleService cibleService, RegularisationService regularisationService) {
            this.connection = connection;
            this.affaireService = affaireService;
            this.repository = repository;
            this.regularisationRepository = regularisationRepository;
            this.remiseEnVigueurRepository = remiseEnVigueurRepository;
            this.cibleService = cibleService;
            this.regularisationService = regularisationService;
        }

        public string[] CreateAvenant(AvenantCreation avenantCreation) {
            var service = new AvenantService(this.connection, this.repository, this.regularisationRepository, this.remiseEnVigueurRepository, this.cibleService, this.affaireService, this.regularisationService);
            var message = service.CreationAvenantModif(
                avenantCreation.ipb,
                avenantCreation.alx,
                avenantCreation.date,
                avenantCreation.description,
                avenantCreation.observations);

            return !message.Any() ? new string[] { "OK" } : message;
        }

        public string Confirm(AvenantCreation avenantCreation) {
            return avenantCreation.ipb;
        }
    }
}
