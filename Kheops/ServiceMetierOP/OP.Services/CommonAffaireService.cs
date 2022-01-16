using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Avenant;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace OP.Services {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CommonAffaireService : ICommonAffaire {
        public List<AvenantAlerteDto> GetListAlertesAvenant(AffaireId affaireId) {
            return DataAccess.AvenantRepository.GetListAlertesAvenant(
                affaireId.CodeAffaire,
                affaireId.NumeroAliment.ToString(),
                affaireId.TypeAffaire.AsCode(),
                WCFHelper.GetFromHeader("UserAS400"));
        }
    }
}
